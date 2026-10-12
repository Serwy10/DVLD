using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public static class manageLicensesData
    {
        private static string connectionString => DatabaseSettings.ConnectionString;

        private static int isthatDriverExisted(int personID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                object result = null;

                string Query = "select driverID from drivers where personID=@id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", personID);

                    try
                    {
                        conn.Open();

                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int driverID))
                            return driverID;
                        else return -1;
                    }
                    catch (Exception) { throw; }
                }
            }
        }
        private static bool AddNewDriver(ref int driverID, int personID, int createdbyUserID, DateTime createdDate)
        {
            int id = isthatDriverExisted(personID);
            if (id != -1)
            {
                driverID = id;
                return true;
            }
            object result = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                string query = "insert into drivers (personid,createdbyuserid,createddate) " +
                    "values " +
                    "(@person,@user,@date) " +
                    "select SCOPE_IDENTITY()";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@person", personID);
                    cmd.Parameters.AddWithValue("@user", createdbyUserID);
                    cmd.Parameters.AddWithValue("@date", createdDate);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            driverID = ID;
                            return true;
                        }
                        else
                            return false;
                    }
                    catch (Exception) { throw; }
                }
            }

        }

        public static bool isThatAPplicationAlreadyHasLicense(int applicatioID)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select found=1 from licenses where applicationid=@id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", applicatioID);
                    try
                    {
                        conn.Open();

                        return cmd.ExecuteScalar() != null;
                    }
                    catch (Exception) { throw; }
                }
            }
        }
        public static bool SaveNewLicense(int personID, ref int LicenseID, int applicationID, ref int driverID, int licenseClass, DateTime IssueDate,
            DateTime expirationDate, string notes, decimal paidFees, bool isActive, int issueReason, int userID)
        {
            var result = LicenseWorkflowData.IssueFirst(personID, applicationID, licenseClass, IssueDate, expirationDate, notes, paidFees, userID);
            if (result == null) return false;
            LicenseID = result.LicenseID;
            driverID = result.DriverID;
            return true;
        }

        public static int getValidityLengthForLicenseClass(string className)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select LicenseClasses.DefaultValidityLength from LicenseClasses where className=@Name";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@Name", className);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int validity))
                            return validity;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static decimal getLicenseClassFees(string className)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select LicenseClasses.ClassFees from LicenseClasses where className=@name";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@name", className);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();

                        if (result != null && decimal.TryParse(result.ToString(), out decimal fees))
                            return fees;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static int getApplicationTypeID(string applicationTypeName)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = "select ApplicationTypes.ApplicationTypeID from ApplicationTypes where ApplicationTypes.ApplicationTypeTitle=@title";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@title", applicationTypeName);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int applicationTypeID))
                            return applicationTypeID;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static bool markApplicationAsCompleted(int applicationID)
        {
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "update applications " +
                    "set applicationStatus=3 , LastStatusDate=SYSDATETIME() " +
                    " where applicationID=@ID";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ID", applicationID);

                    try
                    {
                        conn.Open();
                        affectedRows = command.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }
                }
            }
            return affectedRows > 0;
        }

        public static int getLicenseID(int applicationID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select licenseID from licenses where applicationID=@ID";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ID", applicationID);

                    try
                    {
                        conn.Open();
                        result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                            return ID;
                        return -1;
                    }
                    catch (Exception) { throw; }

                }
            }
        }

        public static bool findDriverLicenseInfo(int licenseID, ref string className, ref string Name, ref string nationalNo, ref string Gendor, ref DateTime IssueDate
            , ref DateTime expirationDate, ref string IssueReason, ref string notes, ref string isActive, ref int driverID, ref DateTime dateOfBirth, ref string isDetained, ref string imageLocation)
        {

            bool found = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                string query = "select * from DriverLicense_View where licenseID=@ID";

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    command.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                className = (string)reader["className"];
                                Name = (string)reader["fullName"];
                                nationalNo = (string)reader["NationalNo"];
                                Gendor = (string)reader["gender"];
                                IssueDate = (DateTime)reader["issueDate"];
                                expirationDate = (DateTime)reader["expirationDate"];
                                IssueReason = (string)reader["ApplicationtypeTitle"];

                                if (reader["notes"] == DBNull.Value)
                                    notes = "No Notes";
                                else
                                    notes = (string)reader["notes"];

                                isActive = (string)reader["isActive"];
                                driverID = (int)reader["driverID"];
                                dateOfBirth = (DateTime)reader["dateOfBirth"];
                                isDetained = (string)reader["isDetained"];

                                if (reader["imagePath"] == DBNull.Value)
                                    imageLocation = string.Empty;
                                else
                                    imageLocation = (string)reader["imagePath"];
                                found = true;

                            }
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return found;
        }

        public static DataTable localLicensesTable(int personID)
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select Licenses.LicenseID,Licenses.ApplicationID,LicenseClasses.ClassName,Licenses.IssueDate,Licenses.ExpirationDate,Licenses.IsActive from " +
                    "Licenses\r\ninner join LicenseClasses\r\non Licenses.LicenseClass=LicenseClasses.LicenseClassID\r\ninner join Applications\r\non " +
                    "Applications.ApplicationID=Licenses.ApplicationID\r\ninner join people\r\non People.PersonID=Applications.ApplicantPersonID" +
                    " where People.PersonID=@ID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", personID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader Reader = cmd.ExecuteReader())
                        {
                            table.Load(Reader);
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return table;
        }

        public static DataTable internationalLicensesTable(int personID)
        {
            DataTable table = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "select * from InternationalLicenses inner join Drivers " +
                    "on Drivers.DriverID=InternationalLicenses.DriverID " +
                    "where Drivers.PersonID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", personID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader Reader = cmd.ExecuteReader())
                        {
                            table.Load(Reader);
                        }
                    }
                    catch (Exception) { throw; }

                }

            }
            return table;
        }

        public static int getLocalDrivingID(int licenseID)
        {
            object result = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID from Licenses\r\ninner join Applications\r\n" +
                    "on Applications.ApplicationID=Licenses.ApplicationID\r\ninner join LocalDrivingLicenseApplications\r\n" +
                    "on LocalDrivingLicenseApplications.ApplicationID=Applications.ApplicationID\r\nwhere licenses.LicenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();

                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int iD))
                            return iD;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }

        }

        public static bool isThatLicenseIDExisted(int licenseID)
        {



            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string query = "Select found=1 from licenses where licenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();
                        return cmd.ExecuteScalar() != null;

                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static bool isThatLicenseActive(int licenseID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "Select found=1 from licenses where licenseID=@ID and isActive=1";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();
                        return cmd.ExecuteScalar() != null;

                    }
                    catch (Exception) { throw; }
                }
            }

        }

        public static bool isThatLicenseNotExpired(int licenseID)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select found=1 from Licenses where LicenseID=@ID and ExpirationDate>SYSDATETIME()";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();
                        return cmd.ExecuteScalar() != null;

                    }
                    catch (Exception) { throw; }
                }
            }

        }

        public static bool isThatLicenseFromOrdinaryLicenseClass(int licenseID)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = "select Licenses.LicenseID from Licenses where Licenses.LicenseClass=3 and Licenses.LicenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();
                        return cmd.ExecuteScalar() != null;

                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static bool isThatLicenseExpired(int licenseID)
        {

            using (SqlConnection connection = new SqlConnection(connectionString))
            {

                string Query = " select found=1 from licenses where Licenses.ExpirationDate<=SYSDATETIME() and Licenses.LicenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();
                        return cmd.ExecuteScalar() != null;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static DateTime ExpirationDate(int licenseID)
        {
            object result = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = " select CONVERT(date,Licenses.ExpirationDate )as expirationDate from licenses where LicenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();
                        result = cmd.ExecuteScalar();
                        if (result != null && DateTime.TryParse(result.ToString(), out DateTime expireDate))
                            return expireDate;
                        else return DateTime.MinValue;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static int getPersonID(int licenseID)
        {
            object result = null;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = " select Drivers.personID from Licenses  inner join drivers  on Drivers.DriverID=Licenses.DriverID where licenses.licenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();
                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int personID))
                            return personID;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static bool FIndLicense(int licenseID, ref int applicationID, ref int driverID, ref int licenseClass, ref DateTime issueDate,
            ref DateTime expirationDate
            , ref string Notes, ref decimal paidFees, ref bool isActive, ref int issueReason, ref int createdBy)
        {

            bool found = false;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string Query = "select *  from licenses where licenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, connection))
                {
                    cmd.Parameters.AddWithValue("@ID", licenseID);

                    try
                    {
                        connection.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                applicationID = (int)reader["applicationid"];
                                driverID = (int)reader["driverid"];
                                licenseClass = (int)reader["licenseclass"];
                                issueDate = (DateTime)reader["issuedate"];
                                if (reader["notes"] == DBNull.Value)
                                    Notes = string.Empty;
                                else
                                    Notes = (string)reader["notes"];
                                paidFees = (decimal)reader["paidfees"];
                                isActive = (bool)reader["isActive"];
                                issueReason = (byte)reader["issueReason"];
                                createdBy = (int)reader["createdbyUserID"];
                                expirationDate = (DateTime)reader["expirationDate"];
                                found = true;

                            }
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return found;
        }

        public static bool DeActivateLisence(int lisenceID)
        {
            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = "update licenses " +
                    "set isActive=0 " +
                    "where licenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {

                    cmd.Parameters.AddWithValue("@ID", lisenceID);

                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();

                        return affectedRows > 0;

                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static int isPersonHasAlreadyLicenseFromThatClass(int driverID, string className)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = " select licenses.licenseID from licenses inner join LicenseClasses  on Licenses.LicenseClass=LicenseClasses.LicenseClassID " +
                    " inner join Drivers  on Drivers.DriverID=Licenses.DriverID " +
                    " where Licenses.IsActive=1 and Licenses.ExpirationDate>SYSDATETIME() and drivers.DriverID=@ID and LicenseClasses.ClassName=@Class";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", driverID);
                    cmd.Parameters.AddWithValue("@Class", className);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int licenseID))
                            return licenseID;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }
        }

        public static int getLicenseClassID(int localLicenseID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select Licenses.LicenseClass from Licenses where LicenseID=@ID";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@ID", localLicenseID);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int licenseClassID))
                            return licenseClassID;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }
        }


    }
}
