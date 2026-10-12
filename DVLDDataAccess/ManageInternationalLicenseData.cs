using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public static class ManageInternationalLicenseData
    {

        private static string ConnectionString => DatabaseSettings.ConnectionString;

        public static bool saveNewInternationalLicense(ref int internationalLicenseID, int applicationID, int DriverID, int localLicenseID, DateTime Issuedate
            , DateTime ExpirationDate, bool isActive, int createdBy)
        {
            if (!manageLicensesData.isThatLicenseIDExisted(localLicenseID) || !manageLicensesData.isThatLicenseActive(localLicenseID) ||
                !manageLicensesData.isThatLicenseNotExpired(localLicenseID))
                return false;

            object result = null;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {

                string query = "insert into InternationalLicenses(applicationID,DriverID,issuedUsingLocalLicenseID,issuedate,expirationdate,isActive,createdByUserID) " +
                    "values " +
                    "(@AppID,@Driver,@LocalID,@IssueDate,@Expiration,@isActive,@createdBy) " +
                    "select SCOPE_IDENTITY()";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@AppID", applicationID);
                    cmd.Parameters.AddWithValue("@Driver", DriverID);
                    cmd.Parameters.AddWithValue("@LocalID", localLicenseID);
                    cmd.Parameters.AddWithValue("@IssueDate", Issuedate);
                    cmd.Parameters.AddWithValue("@Expiration", ExpirationDate);
                    cmd.Parameters.AddWithValue("@isActive", isActive);
                    cmd.Parameters.AddWithValue("@createdBy", createdBy);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int iD))
                        {
                            internationalLicenseID = iD;
                            return true;
                        }
                        return false;
                    }
                    catch (Exception) { throw; }

                }
            }
        }


        public static int isthatLicenseHasAlreadyInternationalLicenseAndNotExpired(int localLicenseID)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string Query = "select InternationalLicenses.internationalLicenseID from InternationalLicenses where IssuedUsingLocalLicenseID=@ID and IsActive=1 and ExpirationDate>SYSDATETIME()";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", localLicenseID);

                    try
                    {
                        conn.Open();

                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int iD))
                            return iD;
                        return -1;

                    }
                    catch (Exception) { throw; }
                }
            }


        }

        public static bool findInternationalDriverLicenseInfo(int IntLicenseID, ref string Name, ref int localLicenseID, ref string nationaNo, ref string gender
            , ref DateTime Issuedate, ref int applicationID, ref string isActive, ref DateTime dateOfBirth, ref int driverID, ref DateTime expirationDate
            , ref string ImagePath)
        {
            bool found = false;

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string Query = "select * from internationalDriverLicenseInfo_view where InternationalLicenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", IntLicenseID);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {

                            while (rdr.Read())
                            {

                                Name = rdr["fullName"].ToString();
                                localLicenseID = (int)rdr["issuedUsingLocalLicenseID"];
                                nationaNo = (string)rdr["nationalNo"];
                                gender = (string)rdr["gender"];
                                Issuedate = (DateTime)rdr["issueDate"];
                                applicationID = (int)rdr["applicationid"];
                                isActive = (string)rdr["isActive"];
                                dateOfBirth = (DateTime)rdr["dateOfBirth"];
                                driverID = (int)rdr["driverid"];
                                expirationDate = (DateTime)rdr["expirationdate"];

                                if (rdr["imagepath"] == DBNull.Value)
                                    ImagePath = string.Empty;
                                else
                                    ImagePath = (string)rdr["imagepath"];
                                found = true;

                            }
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return found;
        }

        public static DataTable internationalLicensesTable()
        {
            DataTable table = new DataTable();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string Query = "select il.InternationalLicenseID,il.ApplicationID,il.DriverID,il.IssuedUsingLocalLicenseID as locaLicenseID,il.IssueDate," +
                    "il.ExpirationDate,il.IsActive from InternationalLicenses as Il";

                using (SqlCommand cmd = new SqlCommand(Query, conn)) {

                    try
                    {
                        conn.Open();

                        using(SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            table.Load(rdr);
                        }
                    }
                    catch (Exception) { throw; }


            }
            }
            return table;
        }

        public static int getPersonID(int internationalID)
        {
            object result=null;

            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = " select drivers.PersonID from InternationalLicenses inner join Drivers" +
                    " on InternationalLicenses.DriverID=Drivers.DriverID where InternationalLicenses.InternationalLicenseID=@ID";

                using(SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID",internationalID);

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

        public static bool checkIfPersonHaveINTLicenseAndNotExpired(int driverID)
        {
            using(SqlConnection conn = new SqlConnection(ConnectionString))
            {
                string query = "  select found=1 from InternationalLicenses inner join drivers on " +
                    "Drivers.DriverID=InternationalLicenses.DriverID where Drivers.DriverID=@ID " +
                    "and InternationalLicenses.IsActive=1 and InternationalLicenses.ExpirationDate>SYSDATETIME()";

                using (SqlCommand cmd = new SqlCommand(query, conn)) {

                    cmd.Parameters.AddWithValue("@ID", driverID);

                    try
                    {
                        conn.Open();

                        return cmd.ExecuteScalar() != null;
                    }
                    catch (Exception) { throw; }
                }
            }
        }
    }
}
