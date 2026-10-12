using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public static class manageDriversData
    {
        private static string ConnectionString => DatabaseSettings.ConnectionString;
        public static DataTable DriversTable()
        {
            DataTable DriversTable = new DataTable();

            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {
                string Query = " select * from DriversTable_View";

                using (SqlCommand command = new SqlCommand(Query, connection))
                {
                    try
                    {
                        connection.Open();

                        using (SqlDataReader Reader = command.ExecuteReader())
                        {
                            DriversTable.Load(Reader);
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return DriversTable;
        }
    }
}
