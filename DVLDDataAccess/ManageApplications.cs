using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;



namespace DVLDDataAccess
{
    public static class ManageApplications
    {

        private static string connsectionString => DatabaseSettings.ConnectionString;

        public static DataTable LicensesClasses()
        {
            DataTable LicensesClasses = new DataTable();

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "select classname from licenseClasses";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            LicensesClasses.Load(reader);
                        }
                    }
                    catch (Exception)
                    {

                        throw;
                    }

                }
            }
            return LicensesClasses;
        }

        public static int getLicenseClassID(string licenseClassName)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "Select LicenseClassID from licenseClasses where className=@Name";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@Name", licenseClassName);

                    try
                    {
                        conn.Open();

                        result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int ID))
                            return ID;


                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }
            return -1;
        }
        public static bool PersonAlreadyHasApplicationOfThatClass(int personID, string className, int excludedApplicationID = -1)
        {
            bool Existed = false;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "select l.ApplicationID,l.LicenseClassID,a.ApplicantPersonID\r\n\r\n" +
                    "from LocalDrivingLicenseApplications as l inner join Applications as a\r\non l.ApplicationID=a.ApplicationID\r\n" +
                    "where a.ApplicantPersonID=@PID and l.LicenseClassID=@LID and a.ApplicationStatus <> 2 and a.ApplicationID<>@excluded";


                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@PID", personID);
                    cmd.Parameters.AddWithValue("@LID", getLicenseClassID(className));
                    cmd.Parameters.AddWithValue("@excluded", excludedApplicationID);

                    try
                    {
                        conn.Open();

                        if (cmd.ExecuteScalar() != null)
                            Existed = true;
                    }
                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
            return Existed;
        }

        public static decimal GetApplicationFees(int ApplicationTypeID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "Select ApplicationFees from ApplicationTypes where ApplicationTypeID=@Id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", ApplicationTypeID);

                    try
                    {
                        conn.Open();

                        result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            if (result is decimal Fees)
                                return Fees;
                        }



                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return -1;
        }

        private static int getApplicationStatusNumber(string applicationStatus)
        {
            switch (applicationStatus.ToLower())
            {
                case "new":
                    return 1;
                case "cancelled":
                    return 2;
                case "completed":
                    return 3;


            }
            return -1;
        }
        public static bool SaveNewApplication(ref int ApplicationID, int PersonID, DateTime ApplicationStartDate, int typeID, string ApplicationSatus,
            DateTime lastStatusDate, decimal paidFees, int createdByUserID, int licenseClassID)
        {
            using (var connection = new SqlConnection(connsectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    if (typeID == 1)
                    {
                        using (var eligible = new SqlCommand(@"SELECT 1 FROM People p JOIN LicenseClasses c ON c.LicenseClassID=@class
WHERE p.PersonID=@person AND DATEADD(year,c.MinimumAllowedAge,p.DateOfBirth)<=@now
AND EXISTS(SELECT 1 FROM Users WHERE UserID=@user AND IsActive=1)", connection, transaction))
                        {
                            eligible.Parameters.AddWithValue("@class", licenseClassID);
                            eligible.Parameters.AddWithValue("@person", PersonID);
                            eligible.Parameters.AddWithValue("@now", ApplicationStartDate);
                            eligible.Parameters.AddWithValue("@user", createdByUserID);
                            if (eligible.ExecuteScalar() == null) return false;
                        }
                        using (var check = new SqlCommand(@"SELECT 1 FROM LocalDrivingLicenseApplications l WITH(UPDLOCK,HOLDLOCK)
JOIN Applications a ON a.ApplicationID=l.ApplicationID
WHERE a.ApplicantPersonID=@person AND l.LicenseClassID=@class AND a.ApplicationStatus<>2", connection, transaction))
                        {
                            check.Parameters.AddWithValue("@person", PersonID);
                            check.Parameters.AddWithValue("@class", licenseClassID);
                            if (check.ExecuteScalar() != null) return false;
                        }
                    }
                    int id;
                    using (var command = new SqlCommand(@"INSERT INTO Applications
(ApplicantPersonID,ApplicationDate,ApplicationTypeID,ApplicationStatus,LastStatusDate,PaidFees,CreatedByUserID)
VALUES(@person,@start,@type,@status,@last,@fees,@user); SELECT SCOPE_IDENTITY();", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@person", PersonID);
                        command.Parameters.AddWithValue("@start", ApplicationStartDate);
                        command.Parameters.AddWithValue("@type", typeID);
                        command.Parameters.AddWithValue("@status", getApplicationStatusNumber(ApplicationSatus));
                        command.Parameters.AddWithValue("@last", lastStatusDate);
                        command.Parameters.AddWithValue("@fees", paidFees);
                        command.Parameters.AddWithValue("@user", createdByUserID);
                        id = Convert.ToInt32(command.ExecuteScalar());
                    }
                    if (typeID == 1)
                    {
                        using (var command = new SqlCommand("INSERT INTO LocalDrivingLicenseApplications(ApplicationID,LicenseClassID) VALUES(@app,@class)", connection, transaction))
                        {
                            command.Parameters.AddWithValue("@app", id);
                            command.Parameters.AddWithValue("@class", licenseClassID);
                            command.ExecuteNonQuery();
                        }
                    }
                    transaction.Commit();
                    ApplicationID = id;
                    return true;
                }
            }
        }



        private static bool saveNewApplicationInfoInLocalDrivingLicenseApplications(int ApplicaionID, int ClassID)
        {
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "insert into LocalDrivingLicenseApplications (ApplicationID,LicenseClassID) " +
                    "Values " +
                    "(@appID,@ClassID)";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@appID", ApplicaionID);
                    cmd.Parameters.AddWithValue("@ClassID", ClassID);

                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }
                }

            }
            return affectedRows > 0;
        }

        public static bool FindApplication(int ApplicationID, ref int _ApplicationPersonID, ref DateTime _ApplicationStartDate, ref int _TypeID
            , ref string _ApplicationStatus
            , ref DateTime lastStatusDate, ref decimal paidFees, ref int createdByUserID, ref int _LicenseClassID)
        {
            bool existed = false;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {

                string Query = "select Applications.ApplicantPersonID,Applications.ApplicationDate,Applications.ApplicationTypeID," +
                    "Applications.CreatedByUserID,Applications.LastStatusDate," +
                    " AppStatus =\r\ncase\r\nwhen Applications.ApplicationStatus=1 then 'New'\r\nwhen Applications.ApplicationStatus=2 then 'Cancelled'" +
                    "\r\nwhen Applications.ApplicationStatus=3 then 'Completed'\r\nend\r\n,Applications.PaidFees " +
                    ",LocalDrivingLicenseApplications.LicenseClassID\r\nfrom Applications\r\ninner join LocalDrivingLicenseApplications\r\non" +
                    " Applications.ApplicationID=LocalDrivingLicenseApplications.ApplicationID\r\nWhere Applications.ApplicationID=@Id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {

                    cmd.Parameters.AddWithValue("@Id", ApplicationID);

                    try
                    {
                        conn.Open();

                        SqlDataReader reader = cmd.ExecuteReader();
                        while (reader.Read())
                        {
                            _ApplicationPersonID = (int)reader["ApplicantPersonID"];
                            _ApplicationStartDate = (DateTime)reader["ApplicationDate"];
                            _TypeID = (int)reader["ApplicationTypeID"];
                            _ApplicationStatus = (string)reader["AppStatus"];
                            lastStatusDate = (DateTime)reader["LastStatusDate"];
                            paidFees = (decimal)reader["PaidFees"];
                            createdByUserID = (int)reader["CreatedByUserID"];
                            _LicenseClassID = (int)reader["LicenseClassID"];
                            existed = true;
                        }

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return existed;
        }

        public static bool UpdateApplicationInfo(int ApplicationID, DateTime lastStatusDate, int LicenseClassID)
        {
            using (var connection = new SqlConnection(connsectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    using (var eligible = new SqlCommand(@"SELECT 1 FROM Applications a WITH(UPDLOCK,HOLDLOCK)
JOIN LocalDrivingLicenseApplications l ON l.ApplicationID=a.ApplicationID
JOIN People p ON p.PersonID=a.ApplicantPersonID JOIN LicenseClasses c ON c.LicenseClassID=@class
WHERE a.ApplicationID=@id AND a.ApplicationStatus=1 AND DATEADD(year,c.MinimumAllowedAge,p.DateOfBirth)<=@now
AND (@class=l.LicenseClassID OR NOT EXISTS(SELECT 1 FROM TestAppointments WHERE LocalDrivingLicenseApplicationID=l.LocalDrivingLicenseApplicationID))
AND NOT EXISTS(SELECT 1 FROM LocalDrivingLicenseApplications other JOIN Applications o ON o.ApplicationID=other.ApplicationID
               WHERE o.ApplicantPersonID=a.ApplicantPersonID AND other.LicenseClassID=@class AND o.ApplicationStatus<>2 AND o.ApplicationID<>@id)", connection, transaction))
                    {
                        eligible.Parameters.AddWithValue("@id", ApplicationID);
                        eligible.Parameters.AddWithValue("@class", LicenseClassID);
                        eligible.Parameters.AddWithValue("@now", lastStatusDate);
                        if (eligible.ExecuteScalar() == null) return false;
                    }
                    using (var command = new SqlCommand(@"UPDATE Applications SET LastStatusDate=@date WHERE ApplicationID=@id;
UPDATE LocalDrivingLicenseApplications SET LicenseClassID=@class WHERE ApplicationID=@id;", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@date", lastStatusDate);
                        command.Parameters.AddWithValue("@id", ApplicationID);
                        command.Parameters.AddWithValue("@class", LicenseClassID);
                        if (command.ExecuteNonQuery() != 2) return false;
                    }
                    transaction.Commit();
                    return true;
                }
            }
        }

        public static DataTable LocalDrivingApplicationsTable()
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "Select * from LocalApplicationsView";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            table.Load(reader);
                        }
                    }
                    catch (Exception) { throw; }

                }
            }
            return table;
        }

        public static int getApplicationID(int localApplicatonID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string query = "select applicationID from LocalDrivingLicenseApplications " +
                    "where localDrivingLicenseApplicationID=@ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", localApplicatonID);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            if (result is int ID)
                                return ID;
                        }
                    }
                    catch (Exception) { throw; }
                }

            }
            return -1;
        }

        public static bool isApplicationCompleted(int LocalApplicationID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string query = "select found=1 from applications where applicationstatus=3 and applicationID=@ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", getApplicationID(LocalApplicationID));

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                        return result != null;
                    }
                    catch (Exception) { throw; }

                }
            }
        }
        public static bool CancelApplication(int LocalDrivingApplicationID)
        {
            if (isApplicationCompleted(LocalDrivingApplicationID))
                return false;
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = " Update Applications " +
                    "set applicationStatus=2, LastStatusDate=SYSDATETIME() " +
                    "where applicationID=@ID and applicationstatus <>3";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", getApplicationID(LocalDrivingApplicationID));

                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();

                    }
                    catch (Exception) { throw; }
                }
            }
            return affectedRows > 0;
        }

        public static int getPersonID(int ApplicationID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string query = "select applicantpersonid from applications " +
                    "where applicationid=@id";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@id", ApplicationID);

                    try
                    {
                        conn.Open();

                        result = command.ExecuteScalar();
                        if (result != null)
                        {
                            if (result is int ID)
                                return ID;
                        }
                    }
                    catch (Exception) { throw; }

                }
            }
            return -1;
        }

        public static string getClassName(int licenseClassID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string query = "select classname from licenseclasses where licenseclassid=@ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseClassID);

                    try
                    {
                        conn.Open();

                        result = cmd.ExecuteScalar();

                        if (result != null)
                            return result.ToString();
                    }
                    catch (Exception) { throw; }
                }
            }
            return "";
        }

        public static string getUserName(int userID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "Select username from users where userid=@id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", userID);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();

                        if (result != null)
                            return result.ToString();
                    }
                    catch (Exception) { throw; }
                }
            }
            return string.Empty;
        }

        public static bool isApplicationNew(int LocalapplicationID)
        {

            int applicationID = getApplicationID(LocalapplicationID);

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string query = "select found=1 from applications where applicationid=@id and applicationStatus=1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", applicationID);

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteScalar() != null;

                    }
                    catch (Exception) { throw; }
                }
            }

        }

        public static bool IsSuccededInThatTest(int testTypeID, int localApplicationID)
        {
            using (SqlConnection conn = new SqlConnection(connsectionString))
            {

                string Query = "Select found=1 from Tests inner join testAppointments " +
                    "on Tests.TestAppointmentID=TestAppointments.TestAppointmentID " +
                    "where TestAppointments.TestTypeID=@typeID and tests.TestResult=1 " +
                    "and TestAppointments.LocalDrivingLicenseApplicationID=@id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {

                    cmd.Parameters.AddWithValue("@typeID", testTypeID);
                    cmd.Parameters.AddWithValue("@id", localApplicationID);

                    try
                    {
                        conn.Open();
                        return cmd.ExecuteScalar() != null;
                    }
                    catch (Exception) { throw; }

                }
            }


        }

        public static bool FindApplicationInfo(int LocalApplicationID, ref string className, ref int passedTests, ref int AppID,
            ref string status, ref decimal paidFees, ref string AppType, ref string applicantName, ref DateTime AppDate, ref DateTime lastStatusDate
            , ref string createdBy, ref int personID)
        {
            bool found = false;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string query = "Select * from retrivingApplicationDataView " +
                    "where retrivingApplicationDataView.LDLAppID=@ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", LocalApplicationID);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                className = (string)reader["className"];
                                passedTests = (int)reader["passedTestCount"];
                                AppID = (int)reader["ApplicationID"];
                                status = (string)reader["status"];
                                paidFees = (decimal)reader["paidfees"];
                                AppType = (string)reader["applicationtypetitle"];
                                applicantName = (string)reader["fullName"];
                                AppDate = (DateTime)reader["applicationDate"];
                                lastStatusDate = (DateTime)reader["lastStatusDate"];
                                createdBy = (string)reader["userName"];
                                personID = (int)reader["applicantPersonID"];
                                found = true;


                            }
                        }
                    }
                    catch (Exception) { throw; }

                }

            }
            return found;
        }

        public static decimal getApplicationFees(int ApplicationTypeID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connsectionString)) {

                string Query = "select applicationFees from applicationTypes " +
                    "where ApplicationTypeID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, conn)) {

                    cmd.Parameters.AddWithValue("@ID", ApplicationTypeID);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            if (result is decimal Fees)
                                return Fees;
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return 0;
        }
        public static bool isApplicantPassedAllTests(int localDrivingID)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "select count(distinct case when Tests.TestResult=1 then app.TestTypeID end) as passedTests from TestAppointments as app " +
                    "inner join Tests on Tests.TestAppointmentID=app.TestAppointmentID " +
                    "where app.LocalDrivingLicenseApplicationID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", localDrivingID);

                    try
                    {
                        conn.Open();

                        result = cmd.ExecuteScalar();
                        if (result != null)
                        {
                            if (result is int passedTests)
                                return passedTests == 3;
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return false;
        }

        public static bool isAPplicationHasLicense(int localDrivingID) {

            int applicationID = getApplicationID(localDrivingID);

            object result = null;

            using (SqlConnection conn = new SqlConnection(connsectionString))
            {
                string Query = "select found=1 from licenses where applicationID=@id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", applicationID);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                        return result != null;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static bool deleteApplication(int localDrivingID)
        {
            using (var connection = new SqlConnection(connsectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    int applicationID;
                    using (var find = new SqlCommand(@"SELECT l.ApplicationID FROM LocalDrivingLicenseApplications l WITH(UPDLOCK,HOLDLOCK)
JOIN Applications a ON a.ApplicationID=l.ApplicationID
WHERE l.LocalDrivingLicenseApplicationID=@id AND a.ApplicationStatus<>3
AND NOT EXISTS(SELECT 1 FROM TestAppointments WHERE LocalDrivingLicenseApplicationID=@id)
AND NOT EXISTS(SELECT 1 FROM Licenses WHERE ApplicationID=l.ApplicationID)", connection, transaction))
                    {
                        find.Parameters.AddWithValue("@id", localDrivingID);
                        object result = find.ExecuteScalar();
                        if (result == null) return false;
                        applicationID = Convert.ToInt32(result);
                    }
                    using (var delete = new SqlCommand(@"DELETE FROM LocalDrivingLicenseApplications WHERE LocalDrivingLicenseApplicationID=@local;
DELETE FROM Applications WHERE ApplicationID=@app;", connection, transaction))
                    {
                        delete.Parameters.AddWithValue("@local", localDrivingID);
                        delete.Parameters.AddWithValue("@app", applicationID);
                        if (delete.ExecuteNonQuery() != 2) return false;
                    }
                    transaction.Commit();
                    return true;
                }
            }
        }


    } }
