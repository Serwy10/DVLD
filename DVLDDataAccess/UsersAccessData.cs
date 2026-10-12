using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLDDataAccess
{
    public static class UsersAccessData
    {



        private static string connectionString => DatabaseSettings.ConnectionString;
        public static DataTable UsersTable()
        {
            DataTable usersTable = new DataTable();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select UserID,users.PersonID,UserName,(firstname+' '+secondName+' '+lastname) as FullName,ISActive from users " +
                    "inner join people " +
                    "on users.personid=people.personid";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {



                            usersTable.Load(reader);



                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return usersTable;
        }

        public static bool isValidLogin(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(password)) return false;
            using (var conn = new SqlConnection(connectionString))
            using (var cmd = new SqlCommand("SELECT Password FROM Users WHERE UserName=@user AND IsActive=1", conn))
            {
                cmd.Parameters.AddWithValue("@user", username);
                conn.Open();
                string stored = cmd.ExecuteScalar() as string;
                if (stored == null) return false;
                if (PasswordHasher.IsHash(stored)) return PasswordHasher.Verify(password, stored);
                // Compatibility with the original database: replace plaintext on the first valid login.
                if (!string.Equals(password, stored, StringComparison.Ordinal)) return false;
                using (var upgrade = new SqlCommand("UPDATE Users SET Password=@hash WHERE UserName=@user AND Password=@old AND IsActive=1", conn))
                {
                    upgrade.Parameters.AddWithValue("@hash", PasswordHasher.Hash(password));
                    upgrade.Parameters.AddWithValue("@user", username);
                    upgrade.Parameters.AddWithValue("@old", stored);
                    return upgrade.ExecuteNonQuery() == 1;
                }
            }
        }

        public static bool isUserActive(string username)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                string query = "select found=1 from users where username=@user and isactive=1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("user", username);


                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }
            }
            return (result != null);
        }

        public static bool Find(string userName, ref int userid, ref int personID, ref bool isActive)
        {
            bool Existed = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select userid,username,personid,isactive from users where username=@user";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@user", userName);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                Existed = true;
                                userid = (int)reader["userid"];
                                personID = (int)reader["personID"];
                                isActive = ((bool)reader["isactive"] == true ? true : false);
                            }
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return Existed;
        }


        public static bool Find(int userid, ref string userName, ref string password, ref bool isActive)
        {
            bool Existed = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select username,isactive from users where userid=@userid";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userid", userid);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userName = (string)reader["username"];
                                password = string.Empty;
                                isActive = (bool)reader["isactive"];
                                Existed = true;
                            }

                        }
                    }
                    catch (Exception)
                    {
                        throw;

                    }
                }
            }
            return Existed;
        }

        public static bool Find(int personID, ref int userID, ref string userName, ref bool isActive)
        {
            bool Existed = false;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = "select userid,username,isactive from users where personid=@id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", personID);

                    try
                    {
                        conn.Open();

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                userID = (int)reader["userid"];
                                userName = (string)reader["username"];
                                isActive = (bool)reader["isactive"];
                                Existed = true;
                            }
                        }
                    }
                    catch (Exception) { throw; }
                }
            }
            return Existed;
        }
        public static bool saveNewPassword(string username, string password)
        {
            int rowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = "update users set password=@pass where username=@user";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@pass", PasswordHasher.Hash(password));
                    cmd.Parameters.AddWithValue("@user", username);

                    try
                    {
                        conn.Open();
                        rowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return rowsAffected > 0;
        }

        public static bool checkIfUserNameExisted(string userName)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {

                string query = "select found=1 from users where username=@username";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", userName);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }

            }
            return result != null;
        }

        public static bool AddNewUser(ref int userID, int personID, string UserName, string password, bool isActive)
        {
            object result = null;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "insert into users (PersonID,UserName,Password,IsActive) " +
                    "values " +
                    "(@personID,@UserName,@Password,@isActive) " +
                    "select scope_identity()";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@personID", personID);
                    cmd.Parameters.AddWithValue("@UserName", UserName);
                    cmd.Parameters.AddWithValue("@Password", PasswordHasher.Hash(password));
                    cmd.Parameters.AddWithValue("@isActive", isActive);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            userID = id;
                            return true;
                        }

                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return result != null;
        }

        public static bool checkIfthatPersonHasUserAccount(int personID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "select found=1 from users where personid=@id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@id", personID);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();

                        return (result != null);
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

        }

        public static bool UpdateUser(int userID, string userName, string Password, bool isActive)
        {
            int RowsAffected = 0;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = "update users set username=@username,password=COALESCE(@pass,password),isactive=@isActive where userid=@id";

                using (SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@username", userName);
                    cmd.Parameters.AddWithValue("@pass", string.IsNullOrEmpty(Password) ? (object)DBNull.Value : PasswordHasher.Hash(Password));
                    cmd.Parameters.AddWithValue("@isActive", isActive);
                    cmd.Parameters.AddWithValue("@id", userID);

                    try
                    {
                        conn.Open();

                        RowsAffected = cmd.ExecuteNonQuery();
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return RowsAffected > 0;
        }

        public static int getPersonIDOfUser(int UserID)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string Query = "Select personid from users where userid=@id";

                using(SqlCommand cmd = new SqlCommand(Query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", UserID);

                    try
                    {
                        conn.Open();
                        result = cmd.ExecuteScalar();

                        if(result != null)
                        {
                            if (result is int id)
                                return id;
                        }

                    }
                    catch (Exception) { throw; }
                }
            }
            return -1;
        }

        public static bool DeleteUser(int UserID) {

            int affectedRows = 0;

            using (SqlConnection conn = new SqlConnection(connectionString)) {

                string Query = "Delete from Users where userID=@id";

                using (SqlCommand cmd = new SqlCommand(Query, conn)) {
                    cmd.Parameters.AddWithValue("@id",UserID);

                    try
                    {
                        conn.Open();
                        affectedRows = cmd.ExecuteNonQuery();
                    }
                    catch (System.Data.SqlClient.SqlException e) when (e.Number == 547) { return false; }
                }

            }
            return affectedRows != 0;
        }
    }
}
