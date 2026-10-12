using System;
using System.Data;
using System.Data.SqlClient;

namespace DVLDDataAccess
{
    public sealed class LicenseWorkflowResult
    {
        public int ApplicationID { get; internal set; }
        public int LicenseID { get; internal set; }
        public int DriverID { get; internal set; }
    }

    // Each public operation uses one connection and one transaction, including the application.
    public static class LicenseWorkflowData
    {
        private static LicenseWorkflowResult Execute(int userID, Func<SqlConnection, SqlTransaction, LicenseWorkflowResult> action)
        {
            using (var connection = new SqlConnection(DatabaseSettings.ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    if (Scalar(connection, transaction, "SELECT 1 FROM Users WHERE UserID=@id AND IsActive=1", "@id", userID) == null)
                        return null;
                    var result = action(connection, transaction);
                    if (result != null) transaction.Commit();
                    // Disposing an uncommitted transaction rolls back all writes.
                    return result;
                }
            }
        }

        private static SqlCommand Command(SqlConnection connection, SqlTransaction transaction, string sql, params object[] values)
        {
            var command = new SqlCommand(sql, connection, transaction);
            for (int i = 0; i < values.Length; i += 2)
                command.Parameters.AddWithValue((string)values[i], values[i + 1] ?? DBNull.Value);
            return command;
        }

        private static object Scalar(SqlConnection connection, SqlTransaction transaction, string sql, params object[] values)
        {
            using (var command = Command(connection, transaction, sql, values)) return command.ExecuteScalar();
        }

        private static int Write(SqlConnection connection, SqlTransaction transaction, string sql, params object[] values)
        {
            using (var command = Command(connection, transaction, sql, values)) return command.ExecuteNonQuery();
        }

        private static DataRow License(SqlConnection connection, SqlTransaction transaction, int licenseID)
        {
            using (var command = Command(connection, transaction, @"
SELECT l.*, d.PersonID, c.ClassFees, c.DefaultValidityLength
FROM Licenses l WITH (UPDLOCK, HOLDLOCK)
JOIN Drivers d ON d.DriverID=l.DriverID
JOIN LicenseClasses c ON c.LicenseClassID=l.LicenseClass
WHERE l.LicenseID=@id", "@id", licenseID))
            using (var reader = command.ExecuteReader())
            {
                var table = new DataTable();
                table.Load(reader);
                return table.Rows.Count == 0 ? null : table.Rows[0];
            }
        }

        private static bool Detained(SqlConnection connection, SqlTransaction transaction, int licenseID)
        {
            return Scalar(connection, transaction,
                "SELECT 1 FROM DetainedLicenses WITH (UPDLOCK,HOLDLOCK) WHERE LicenseID=@id AND IsReleased=0", "@id", licenseID) != null;
        }

        private static int Application(SqlConnection connection, SqlTransaction transaction, int personID, int type, int userID, DateTime now)
        {
            object id = Scalar(connection, transaction, @"
INSERT INTO Applications (ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID)
SELECT @person,@now,ApplicationTypeID,1,@now,ApplicationFees,@user FROM ApplicationTypes WHERE ApplicationTypeID=@type;
SELECT SCOPE_IDENTITY();", "@person", personID, "@now", now, "@user", userID, "@type", type);
            if (id == null || id == DBNull.Value) throw new InvalidOperationException("Application type is missing.");
            return Convert.ToInt32(id);
        }

        private static void Complete(SqlConnection connection, SqlTransaction transaction, int applicationID, DateTime now)
        {
            if (Write(connection, transaction,
                "UPDATE Applications SET ApplicationStatus=3,LastStatusDate=@now WHERE ApplicationID=@id AND ApplicationStatus=1",
                "@now", now, "@id", applicationID) != 1) throw new InvalidOperationException("Application is no longer new.");
        }

        private static int InsertLicense(SqlConnection connection, SqlTransaction transaction, int applicationID, int driverID,
            int licenseClass, DateTime now, DateTime expiration, string notes, decimal fees, int reason, int userID)
        {
            return Convert.ToInt32(Scalar(connection, transaction, @"
INSERT INTO Licenses (ApplicationID,DriverID,LicenseClass,IssueDate,ExpirationDate,Notes,PaidFees,IsActive,IssueReason,CreatedByUserID)
VALUES (@app,@driver,@class,@now,@expiry,@notes,@fees,1,@reason,@user);
SELECT SCOPE_IDENTITY();", "@app", applicationID, "@driver", driverID, "@class", licenseClass, "@now", now,
                "@expiry", expiration, "@notes", string.IsNullOrWhiteSpace(notes) ? null : notes,
                "@fees", fees, "@reason", reason, "@user", userID));
        }

        public static LicenseWorkflowResult IssueFirst(int personID, int applicationID, int licenseClass, DateTime now,
            DateTime expiration, string notes, decimal fees, int userID)
        {
            return Execute(userID, (connection, transaction) =>
            {
                object eligible = Scalar(connection, transaction, @"
SELECT 1 FROM Applications a WITH (UPDLOCK,HOLDLOCK)
JOIN LocalDrivingLicenseApplications local ON local.ApplicationID=a.ApplicationID
JOIN LicenseClasses c ON c.LicenseClassID=local.LicenseClassID
JOIN People p ON p.PersonID=a.ApplicantPersonID
WHERE a.ApplicationID=@app AND a.ApplicantPersonID=@person AND a.ApplicationStatus=1 AND a.ApplicationTypeID=1
AND local.LicenseClassID=@class AND DATEADD(year,c.MinimumAllowedAge,p.DateOfBirth)<=@now
AND (SELECT COUNT(DISTINCT t.TestTypeID) FROM TestAppointments t JOIN Tests r ON r.TestAppointmentID=t.TestAppointmentID
     WHERE t.LocalDrivingLicenseApplicationID=local.LocalDrivingLicenseApplicationID AND r.TestResult=1 AND t.TestTypeID IN (1,2,3))=3
AND NOT EXISTS(SELECT 1 FROM Licenses WHERE ApplicationID=@app)
AND NOT EXISTS(SELECT 1 FROM Licenses l JOIN Drivers d ON d.DriverID=l.DriverID
               WHERE d.PersonID=@person AND l.LicenseClass=@class AND l.IsActive=1);",
                    "@app", applicationID, "@person", personID, "@class", licenseClass, "@now", now);
                if (eligible == null || expiration <= now || fees < 0) return null;
                object driver = Scalar(connection, transaction, "SELECT DriverID FROM Drivers WITH (UPDLOCK,HOLDLOCK) WHERE PersonID=@person", "@person", personID);
                int driverID = driver == null ? Convert.ToInt32(Scalar(connection, transaction, @"
INSERT INTO Drivers(PersonID,CreatedByUserID,CreatedDate) VALUES(@person,@user,@now); SELECT SCOPE_IDENTITY();",
                    "@person", personID, "@user", userID, "@now", now)) : Convert.ToInt32(driver);
                int licenseID = InsertLicense(connection, transaction, applicationID, driverID, licenseClass, now, expiration, notes, fees, 1, userID);
                Complete(connection, transaction, applicationID, now);
                return new LicenseWorkflowResult { ApplicationID=applicationID, DriverID=driverID, LicenseID=licenseID };
            });
        }

        public static LicenseWorkflowResult RenewOrReplace(int oldLicenseID, int applicationType, int userID, string notes)
        {
            if (applicationType < 2 || applicationType > 4) return null;
            return Execute(userID, (connection, transaction) =>
            {
                DataRow old = License(connection, transaction, oldLicenseID);
                DateTime now = DateTime.Now;
                if (old == null || !(bool)old["IsActive"] || Detained(connection, transaction, oldLicenseID)) return null;
                bool renew = applicationType == 2;
                DateTime expiry = (DateTime)old["ExpirationDate"];
                if ((renew && expiry > now) || (!renew && expiry <= now)) return null;
                int driverID = (int)old["DriverID"];
                int licenseClass = (int)old["LicenseClass"];
                if (Scalar(connection, transaction,
                    "SELECT 1 FROM Licenses WHERE DriverID=@driver AND LicenseClass=@class AND LicenseID<>@id AND IsActive=1",
                    "@driver", driverID, "@class", licenseClass, "@id", oldLicenseID) != null) return null;
                int appID = Application(connection, transaction, (int)old["PersonID"], applicationType, userID, now);
                if (Write(connection, transaction, "UPDATE Licenses SET IsActive=0 WHERE LicenseID=@id AND IsActive=1", "@id", oldLicenseID) != 1) return null;
                // Replacement preserves expiry and charges only the replacement application fee.
                int newID = InsertLicense(connection, transaction, appID, driverID, licenseClass, now,
                    renew ? now.AddYears(Convert.ToInt32(old["DefaultValidityLength"])) : expiry,
                    notes, renew ? (decimal)old["ClassFees"] : 0m, applicationType, userID);
                Complete(connection, transaction, appID, now);
                return new LicenseWorkflowResult { ApplicationID=appID, DriverID=driverID, LicenseID=newID };
            });
        }

        public static LicenseWorkflowResult IssueInternational(int localLicenseID, int userID)
        {
            return Execute(userID, (connection, transaction) =>
            {
                DataRow local = License(connection, transaction, localLicenseID);
                DateTime now = DateTime.Now;
                if (local == null || !(bool)local["IsActive"] || (int)local["LicenseClass"] != 3 ||
                    (DateTime)local["ExpirationDate"] <= now || Detained(connection, transaction, localLicenseID)) return null;
                int driverID = (int)local["DriverID"];
                if (Scalar(connection, transaction,
                    "SELECT 1 FROM InternationalLicenses WITH (UPDLOCK,HOLDLOCK) WHERE DriverID=@driver AND IsActive=1 AND ExpirationDate>@now",
                    "@driver", driverID, "@now", now) != null) return null;
                int appID = Application(connection, transaction, (int)local["PersonID"], 6, userID, now);
                int id = Convert.ToInt32(Scalar(connection, transaction, @"
INSERT INTO InternationalLicenses(ApplicationID,DriverID,IssuedUsingLocalLicenseID,IssueDate,ExpirationDate,IsActive,CreatedByUserID)
VALUES(@app,@driver,@local,@now,@expiry,1,@user); SELECT SCOPE_IDENTITY();",
                    "@app", appID, "@driver", driverID, "@local", localLicenseID, "@now", now, "@expiry", now.AddYears(1), "@user", userID));
                Complete(connection, transaction, appID, now);
                return new LicenseWorkflowResult { ApplicationID=appID, DriverID=driverID, LicenseID=id };
            });
        }

        public static LicenseWorkflowResult Release(int licenseID, int userID)
        {
            return Execute(userID, (connection, transaction) =>
            {
                DataRow license = License(connection, transaction, licenseID);
                if (license == null || !Detained(connection, transaction, licenseID)) return null;
                DateTime now = DateTime.Now;
                int appID = Application(connection, transaction, (int)license["PersonID"], 5, userID, now);
                if (Write(connection, transaction, @"
UPDATE DetainedLicenses SET IsReleased=1,ReleaseDate=@now,ReleasedByUserID=@user,ReleaseApplicationID=@app
WHERE LicenseID=@id AND IsReleased=0;", "@now", now, "@user", userID, "@app", appID, "@id", licenseID) != 1) return null;
                Complete(connection, transaction, appID, now);
                return new LicenseWorkflowResult { ApplicationID=appID, DriverID=(int)license["DriverID"], LicenseID=licenseID };
            });
        }
    }
}
