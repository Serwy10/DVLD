using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public static class manageDetainLicenseData
    {
        private static string connectionString => DatabaseSettings.ConnectionString;

        public static bool isLicenseDetained(int licenseID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = " select found=1 from DetainedLicenses where DetainedLicenses.LicenseID=@ID and IsReleased=0";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {

                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        conn.Open();

                        return cmd.ExecuteScalar() != null;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static bool saveNewDetainedLicense(ref int detainID, int licenseID, DateTime detainDate, decimal fineFees, int createdByUserID)
        {
            if (fineFees < 0) return false;
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.Serializable))
                using (var command = new SqlCommand(@"INSERT INTO DetainedLicenses(LicenseID,DetainDate,FineFees,CreatedByUserID,IsReleased)
SELECT LicenseID,@date,@fees,@user,0 FROM Licenses WITH(UPDLOCK,HOLDLOCK)
WHERE LicenseID=@id AND IsActive=1
AND EXISTS(SELECT 1 FROM Users WHERE UserID=@user AND IsActive=1)
AND NOT EXISTS(SELECT 1 FROM DetainedLicenses WHERE LicenseID=@id AND IsReleased=0);
SELECT SCOPE_IDENTITY();", connection, transaction))
                {
                    command.Parameters.AddWithValue("@id", licenseID);
                    command.Parameters.AddWithValue("@date", detainDate);
                    command.Parameters.AddWithValue("@fees", fineFees);
                    command.Parameters.AddWithValue("@user", createdByUserID);
                    object id = command.ExecuteScalar();
                    if (id == null || id == DBNull.Value) return false;
                    transaction.Commit();
                    detainID = Convert.ToInt32(id);
                    return true;
                }
            }
        }

        public static bool findDetainedLicense(int licenseID, ref int detainID, ref DateTime detainDate, ref decimal fineFees, ref int createdByUserID,
            ref bool isReleased, ref DateTime releasedDate, ref int releasedByUserID, ref int applicationID)
        {
            bool found = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = " select TOP (1) * from DetainedLicenses where LicenseID=@ID ORDER BY IsReleased, DetainID DESC";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {

                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                detainID = (int)reader["detainID"];
                                detainDate = (DateTime)reader["DetainDate"];
                                fineFees = (decimal)reader["fineFees"];
                                createdByUserID = (int)reader["createdbyuserid"];
                                isReleased = (bool)reader["isreleased"];
                                if (reader["releasedate"] == DBNull.Value)
                                    releasedDate = DateTime.MinValue;
                                else
                                    releasedDate = (DateTime)reader["releasedate"];

                                if (reader["releasedbyUserID"] == DBNull.Value)
                                    releasedByUserID = -1;
                                else
                                    releasedByUserID = (int)reader["releasedByUserID"];

                                if (reader["releaseApplicationID"] == DBNull.Value)
                                    applicationID = -1;
                                else
                                    applicationID = (int)reader["releaseApplicationID"];

                                found = true;
                            }
                        }
                    }
                    catch (Exception) { throw; }
                }

            }
            return found;
        }

        public static bool releaseLicense(int detainID, DateTime releaseddate, int releasedByUserID, int releaseApplicationID)
        {
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "update DetainedLicenses " +
                    "set ReleaseApplicationID=@AID,IsReleased=1,ReleaseDate=@Date,ReleasedByUserID=@UserID " +
                    "where DetainID=@DID ";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AID", releaseApplicationID);
                    cmd.Parameters.AddWithValue("@Date", releaseddate);
                    cmd.Parameters.AddWithValue("@UserID", releasedByUserID);
                    cmd.Parameters.AddWithValue("@DID", detainID);

                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }

                }
            }
            return affectedRows == 1;
        }

        public static DataTable detainedLicensesTable()
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = " select * from detainedLicense_View";

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

        public static int isThatLicenseStillDetained(int detainID)
        {
            object result= null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = " select DetainedLicenses.LicenseID from DetainedLicenses where DetainedLicenses.DetainID=@ID and IsReleased=0";

                using(SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", detainID);

                    try
                    {
                        conn.Open();

                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                            return id;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }
        }
    }
}
