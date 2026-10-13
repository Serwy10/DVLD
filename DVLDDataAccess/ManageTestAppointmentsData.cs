using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public static class ManageTestAppointmentsData
    {
        private static string connectionString => DatabaseSettings.ConnectionString;

        private static int whichTestTypeID(string testTypeName)
        {
            switch ((testTypeName ?? string.Empty).Trim().ToLowerInvariant())
            {
                case "vision": return 1;
                case "writting": case "writing": case "written": return 2;
                case "street": return 3;
                default: return -1;
            }
        }
        public static DataTable GetTestAppointmentTable(string appointmentsType, int localdrivingID)
        {
            DataTable dt = new DataTable();

            int testTypeId = whichTestTypeID(appointmentsType);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "select t.TestAppointmentID,t.AppointmentDate,t.PaidFees,t.IsLocked from " +
                    "TestAppointments as t where t.LocalDrivingLicenseApplicationID=@lID and t.TestTypeID=@tID";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {

                    command.Parameters.AddWithValue("@lID", localdrivingID);
                    command.Parameters.AddWithValue("@tID", testTypeId);

                    try
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            dt.Load(reader);

                        }
                    }
                    catch (Exception) { throw; }

                }
            }

            return dt;
        }

        public static bool ifApplicantHasAppointmentAndNotLocked(int localDrivingID, string testType)
        {
            object result = null;
            int testTypeID = whichTestTypeID(testType);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "select TestAppointments.TestAppointmentID from TestAppointments where LocalDrivingLicenseApplicationID=@LID and" +
                    " TestTypeID=@TID and IsLocked=0";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@LID", localDrivingID);
                    command.Parameters.AddWithValue("@TID", testTypeID);

                    try
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                        return (result != null);

                    }
                    catch (Exception) { throw; }
                }


            }
        }

        public static bool checkApplicantPassedThatTest(int localDrivingLicenseID, string testType)
        {

            object result = null;
            int testTypeID = whichTestTypeID(testType);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string Query = "select found=1 from tests inner join TestAppointments on Tests.TestAppointmentID=TestAppointments.TestAppointmentID" +
                    " where TestAppointments.LocalDrivingLicenseApplicationID=@LID and " +
                    "TestAppointments.TestTypeID=@TID and Tests.TestResult=1";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@LID", localDrivingLicenseID);
                    command.Parameters.AddWithValue("@TID", testTypeID);

                    try
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                        return (result != null);
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static decimal GetTestTypeFees(string testType)
        {
            object result = null;

            int testTypeID = whichTestTypeID(testType);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "select TestTypes.TestTypeFees from TestTypes where TestTypes.TestTypeID=@ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", testTypeID);

                    try
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                        if (result != null)
                        {
                            if (result is decimal fees)
                                return fees;
                        }
                    }
                    catch (Exception) { throw; }

                }
            }
            return 0;
        }

        public static bool checkIfApplicantHasFailedOnThatTest(int localDrivingID, string testType)
        {
            object result = null;
            int testTypeID = whichTestTypeID(testType);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "select found=1 from tests inner join TestAppointments " +
                    "on TestAppointments.TestAppointmentID=Tests.TestAppointmentID " +
                    "where TestAppointments.TestTypeID=@TID and Tests.TestResult=0 and TestAppointments.LocalDrivingLicenseApplicationID=@LID";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@TID", testTypeID);
                    command.Parameters.AddWithValue("@LID", localDrivingID);

                    try
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                        return (result != null);
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static int getTestTypeID(string testType)
        {
            return whichTestTypeID(testType);
        }

        public static bool saveNewTestAppointment(ref int appointmentID, int localDrivingID, DateTime AppDate, int testTypeID, decimal paidFees,
            int createdby, bool isLocked)
        {
            if (testTypeID < 1 || testTypeID > 3 || isLocked || paidFees < 0 || AppDate < DateTime.Today) return false;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                using (var command = new SqlCommand(@"INSERT INTO TestAppointments
(TestTypeID,LocalDrivingLicenseApplicationID,AppointmentDate,PaidFees,CreatedByUserID,IsLocked)
SELECT @type,l.LocalDrivingLicenseApplicationID,@date,@fees,@user,0
FROM LocalDrivingLicenseApplications l WITH (UPDLOCK,HOLDLOCK) JOIN Applications a ON a.ApplicationID=l.ApplicationID
WHERE l.LocalDrivingLicenseApplicationID=@local AND a.ApplicationStatus=1
AND EXISTS (SELECT 1 FROM Users WHERE UserID=@user AND IsActive=1)
AND NOT EXISTS(SELECT 1 FROM TestAppointments WHERE LocalDrivingLicenseApplicationID=@local AND TestTypeID=@type AND IsLocked=0)
AND NOT EXISTS(SELECT 1 FROM TestAppointments t JOIN Tests r ON r.TestAppointmentID=t.TestAppointmentID
               WHERE t.LocalDrivingLicenseApplicationID=@local AND t.TestTypeID=@type AND r.TestResult=1)
AND (SELECT COUNT(DISTINCT t.TestTypeID) FROM TestAppointments t JOIN Tests r ON r.TestAppointmentID=t.TestAppointmentID
     WHERE t.LocalDrivingLicenseApplicationID=@local AND t.TestTypeID<@type AND r.TestResult=1)=@type-1;
SELECT SCOPE_IDENTITY();", connection, transaction))
                {
                    command.Parameters.AddWithValue("@type", testTypeID);
                    command.Parameters.AddWithValue("@local", localDrivingID);
                    command.Parameters.AddWithValue("@date", AppDate);
                    command.Parameters.AddWithValue("@fees", paidFees);
                    command.Parameters.AddWithValue("@user", createdby);
                    object id = command.ExecuteScalar();
                    if (id == null || id == DBNull.Value) return false;
                    transaction.Commit();
                    appointmentID = Convert.ToInt32(id);
                    return true;
                }
            }
        }

        public static bool findAppointment(int appointmentID, ref int testTypeID, ref int localDrivingID, ref DateTime appDate, ref decimal fees, ref int createdby
            , ref bool isLocked)
        {
            bool Found = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "Select * from testAppointments where testAppointmentID=@ID";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", appointmentID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                testTypeID = (int)reader["TestTypeID"];
                                localDrivingID = (int)reader["LocalDrivingLicenseApplicationID"];
                                appDate = (DateTime)reader["AppointmentDate"];
                                fees = (decimal)reader["PaidFees"];
                                createdby = (int)reader["CreatedByUserID"];
                                isLocked = (bool)reader["IsLocked"];
                                Found = true;

                            }
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return Found;
        }

        public static int countApplicantTestTrials(string testType, int localDrivingID)
        {
            int testTypeID = whichTestTypeID(testType);
            object result = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "select count(Tests.TestID )as trials from Tests inner join TestAppointments " +
                    "on TestAppointments.TestAppointmentID=Tests.TestAppointmentID " +
                    "where TestAppointments.TestTypeID=@TypeID and TestAppointments.LocalDrivingLicenseApplicationID=@LID";
                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@TypeID", testTypeID);
                    command.Parameters.AddWithValue("@LID", localDrivingID);

                    try
                    {
                        connection.Open();

                        result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int Trials))
                            return Trials;
                    }
                    catch (Exception) { throw; }
                }
            }
            return -1;
        }

        public static bool updateAppointmentDate(int appointmentID, DateTime date)
        {
            if (checkIfAppointmentLocked(appointmentID))
                return false;
            int affectedRows = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "Update testAppointments " +
                    "set appointmentDate=@D " +
                    "where testAppointmentID=@ID AND IsLocked=0";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@D", date);
                    command.Parameters.AddWithValue("@ID", appointmentID);

                    try
                    {
                        connection.Open();
                        affectedRows = command.ExecuteNonQuery();
                        return affectedRows > 0;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static bool checkIfAppointmentLocked(int appointmentID)
        {

            object result = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "SELECT 1 FROM TestAppointments WHERE TestAppointmentID=@ID AND IsLocked=1";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", appointmentID);
                    command.Parameters.AddWithValue("@IID", appointmentID);

                    try
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                        return result != null;

                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static bool markAppointmentAsLocked(int appointmentID)
        {

            int affectedRows = 0;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "update testAppointments " +
                    "set islocked=1 " +
                    "where testAppointmentID=@ID AND IsLocked=0";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@ID", appointmentID);

                    try
                    {
                        connection.Open();

                        affectedRows = command.ExecuteNonQuery();
                        return affectedRows > 0;
                    }
                    catch (Exception) { throw; }
                }
            }

        }

        public static int countPassedTests(int localDrivingID)
        {
            object result= null;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "select count(distinct case when tests.TestResult=1 then TestAppointments.TestTypeID end) as passedTests from Tests  inner join TestAppointments " +
                    "on tests.TestAppointmentID=TestAppointments.TestAppointmentID" +
                    " where TestAppointments.LocalDrivingLicenseApplicationID=@ID";
                using(SqlCommand command = new SqlCommand(Query, connection))
                {
                    command.Parameters.AddWithValue("@ID", localDrivingID);

                    try
                    {
                        connection.Open();
                        result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int count))
                            return count;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }

        }
    }
}
