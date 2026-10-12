using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public static class ManageTestTypes
    {
        private static string ConnesctionString => DatabaseSettings.ConnectionString;

        public static DataTable TestTypesTable()
        {
            DataTable TestTypesTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnesctionString))
            {
                string Query = "Select * from testtypes";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {


                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            TestTypesTable.Load(reader);


                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return TestTypesTable;
        }

        public static bool FindTest(int TestID, ref string TestName, ref string TestDescription, ref decimal fees)
        {
            bool existed = false;

            using (SqlConnection conn = new SqlConnection(ConnesctionString))
            {
                string Query = "Select * from testtypes where testtypeid=@id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {

                    cmd.Parameters.AddWithValue("@id", TestID);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                TestName = (string)reader["testtypetitle"];
                                TestDescription = (string)reader["testtypedescription"];
                                fees = (decimal)reader["testtypefees"];
                                existed = true;
                            }
                        }
                    }
                    catch (Exception) {
                        throw;
                    }
                }

            }
            return existed;
        }

        public static bool UpdateTestType(int testID,string testTitle,string testDescription,decimal testFees)
        {
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(ConnesctionString))
            {
                string Query = "update testtypes " +
                    "set testTypeTitle=@title,testTypeDescription=@description,testtypeFees=@fees " +
                    "where testTypeId=@id";

                using(SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", testTitle);
                    cmd.Parameters.AddWithValue("@description", testDescription);
                    cmd.Parameters.AddWithValue("@fees", testFees);
                    cmd.Parameters.AddWithValue("@id", testID);

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
    }
}
