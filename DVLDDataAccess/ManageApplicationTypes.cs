using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public static class ManageApplicationTypes
    {
        private static string connectionString => DatabaseSettings.ConnectionString;


        public static DataTable applicationTypesTable()
        {
            DataTable applicationTypes = new DataTable();
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {

                string query = "select * from applicationtypes";

                using (SqlCommand command = new SqlCommand(query, sqlConnection))
                {
                    try
                    {
                        sqlConnection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            applicationTypes.Load(reader);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return applicationTypes;
        }

        public static bool findApplicationType(int ApplicationID, ref string applicationName, ref decimal Fees)
        {
            bool found = false;
            using (SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string Query = "select * from applicationtypes where applicationTypeId=@id";

                using (SqlCommand command = new SqlCommand(Query, sqlConnection))
                {

                    command.Parameters.AddWithValue("@id", ApplicationID);

                    try
                    {
                        sqlConnection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                applicationName = (string)reader["applicationtypetitle"];
                                Fees = (decimal)reader["applicationfees"];
                                found = true;
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return found;
        }

        public static bool updateApplicationTypeInfo(int ApplicationID,string Title,decimal Fees)
        {
            int affectedRows = 0;

            using(SqlConnection sqlConnection = new SqlConnection(connectionString))
            {
                string Query = "update applicationTypes " +
                    "set applicationTypeTitle=@title , applicationFees=@fees " +
                    "where applicationtypeID=@ID";

                using(SqlCommand command = new SqlCommand(Query, sqlConnection))
                {
                    command.Parameters.AddWithValue("@title", Title);
                    command.Parameters.AddWithValue("@fees", Fees);
                    command.Parameters.AddWithValue("@ID", ApplicationID);

                    try
                    {
                        sqlConnection.Open();

                        affectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }
                }
            }
            return affectedRows > 0;
        }
    }
}
