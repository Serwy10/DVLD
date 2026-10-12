using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public  static class ManageTestsData
    {
        private static string ConnectionString => DatabaseSettings.ConnectionString;

        public static bool saveNewTest(ref int testID, int testAppointmentID, bool testResult, string Notes, int createdBy)
        {
            using (var connection = new SqlConnection(ConnectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                {
                    using (var command = new SqlCommand(@"UPDATE TestAppointments SET IsLocked=1
WHERE TestAppointmentID=@id AND IsLocked=0 AND AppointmentDate<=@now
AND EXISTS(SELECT 1 FROM LocalDrivingLicenseApplications l JOIN Applications a ON a.ApplicationID=l.ApplicationID
           WHERE l.LocalDrivingLicenseApplicationID=TestAppointments.LocalDrivingLicenseApplicationID AND a.ApplicationStatus=1)
AND NOT EXISTS(SELECT 1 FROM Tests WHERE TestAppointmentID=@id)", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@id", testAppointmentID);
                        command.Parameters.AddWithValue("@now", DateTime.Now);
                        if (command.ExecuteNonQuery() != 1) return false;
                    }
                    int id;
                    using (var command = new SqlCommand(@"INSERT INTO Tests(TestAppointmentID,TestResult,Notes,CreatedByUserID)
VALUES(@app,@result,@notes,@user); SELECT SCOPE_IDENTITY();", connection, transaction))
                    {
                        command.Parameters.AddWithValue("@app", testAppointmentID);
                        command.Parameters.AddWithValue("@result", testResult);
                        command.Parameters.AddWithValue("@notes", string.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);
                        command.Parameters.AddWithValue("@user", createdBy);
                        id = Convert.ToInt32(command.ExecuteScalar());
                    }
                    transaction.Commit();
                    testID = id;
                    return true;
                }
            }
        }

        public static bool findTest(int appointmentID,ref  int testID,ref bool result , ref string notes,ref int createdby)
        {
            bool found=false;

            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string Query = "Select * from tests where testAppointmentID=@ID";

                using (SqlCommand cmd=new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", appointmentID);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                testID=reader.GetInt32(0);
                                result = (bool)reader["testResult"];
                                if (reader["notes"] == DBNull.Value)
                                    notes = string.Empty;
                                else
                                    notes = (string)reader["notes"];
                                createdby = (int)reader["createdByUserId"];

                                found = true;

                            }
                            return found;
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
        }
    }
}
