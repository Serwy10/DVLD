using System;
using System.Data;
using System.Data.SqlClient;


namespace DVLDDataAccess
{
    public class PersonsData
    {
        private static string StringConnection => DatabaseSettings.ConnectionString;


        static private string getPersonNationality(int countryID)
        {
            using (SqlConnection conn = new SqlConnection(StringConnection))
            {

                string query = "Select countryname from countries where countryid=@c";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@c", countryID);

                    try
                    {
                        conn.Open();

                        object result = cmd.ExecuteScalar();

                        if (result.ToString() != string.Empty)
                            return result.ToString();

                    }
                    catch (Exception e)
                    {

                        throw new Exception(e.ToString());
                    }

                }


            }
            return string.Empty;
        }

        static public bool getPersonByID(int personID, ref string nationalNo, ref string firstName, ref string secondname, ref string thirdname, ref string lastname
            , ref string phone, ref string email, ref string imagepath, ref string address, ref DateTime dateOfBirth, ref string gendor,
            ref string nationality)
        {
            using (SqlConnection conn = new SqlConnection(StringConnection))
            {

                string query = "Select * from people where personid=@personid";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@personid", personID);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                nationalNo = (string)reader["nationalno"];
                                firstName = (string)reader["firstname"];
                                secondname = (string)reader["secondname"];

                                if (reader["thirdname"] == DBNull.Value)
                                    thirdname = string.Empty;
                                else
                                    thirdname = (string)reader["thirdname"];

                                lastname = (string)reader["lastname"];
                                phone = (string)reader["phone"];

                                if (reader["email"] == DBNull.Value)
                                    email = string.Empty;
                                else
                                    email = (string)reader["email"];

                                if (reader["imagepath"] == DBNull.Value)
                                    imagepath = string.Empty;
                                else
                                    imagepath = (string)reader["imagepath"];

                                address = (string)reader["address"];

                                dateOfBirth = (DateTime)reader["dateofbirth"];

                                if ((byte)reader["gendor"] == 0)
                                    gendor = "Male";
                                else
                                    gendor = "Female";

                                nationality = getPersonNationality((int)reader["nationalitycountryid"]);

                                return true;

                            }


                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return false;
        }

        static public bool getPersonByNationalNo(string nationalNo, ref int personID, ref string firstName, ref string secondname, ref string thirdname, ref string lastname
            , ref string phone, ref string email, ref string imagepath, ref string address, ref DateTime dateOfBirth, ref string gendor,
            ref string nationality)
        {
            using (SqlConnection conn = new SqlConnection(StringConnection))
            {

                string query = "Select * from people where nationalno=@n";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@n", nationalNo);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {

                            while (reader.Read())
                            {

                                personID = (int)reader["personid"];
                                firstName = (string)reader["firstname"];
                                secondname = (string)reader["secondname"];
                                if (reader["thirdname"] != DBNull.Value)
                                    thirdname = (string)reader["thirdname"];
                                else
                                    thirdname = string.Empty;
                                lastname = (string)reader["lastname"];
                                phone = (string)reader["phone"];
                                email = (string)reader["email"];

                                if (reader["imagepath"] == DBNull.Value)
                                    imagepath = string.Empty;
                                else
                                    imagepath = (string)reader["imagepath"];

                                address = (string)reader["address"];

                                dateOfBirth = (DateTime)reader["dateofbirth"];

                                if ((byte)reader["gendor"] == 0)
                                    gendor = "Male";
                                else
                                    gendor = "Female";

                                nationality = getPersonNationality((int)reader["nationalitycountryid"]);

                                return true;

                            }


                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
            return false;
        }


        public static DataTable PeopleList()
        {
            DataTable dt = new DataTable();

            dt.Columns.Add("PersonID", typeof(int));
            dt.Columns.Add("NationalNO", typeof(string));
            dt.Columns.Add("FirstName", typeof(string));
            dt.Columns.Add("SecondName", typeof(string));
            dt.Columns.Add("ThirdName", typeof(string));
            dt.Columns.Add("LastName", typeof(string));
            dt.Columns.Add("Gendor", typeof(string));
            dt.Columns.Add("DateOfBirth", typeof(DateTime));
            dt.Columns.Add("Nationality", typeof(string));
            dt.Columns.Add("Phone", typeof(string));
            dt.Columns.Add("Email", typeof(string));

            using (SqlConnection conn = new SqlConnection(StringConnection))
            {
                string query = "select * from people";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {

                                dt.Rows.Add(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3),
                                    (reader["thirdname"] == DBNull.Value) ? string.Empty : reader.GetString(4), reader.GetString(5), (reader.GetByte(7) == 0) ? "Male" : "Female",
                                    reader.GetDateTime(6), getPersonNationality(reader.GetInt32(11)), reader.GetString(9)
                                    , (reader["email"] == DBNull.Value) ? string.Empty : reader.GetString(10));


                            }



                        }


                    }
                    catch (Exception)
                    {
                        throw;
                    }

                }

            }
            return dt;
        }




        private static int getCountryID(string country)
        {
            using (SqlConnection conn = new SqlConnection(StringConnection))
            {
                string query = "select countryid from countries where countryname=@c";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@c", country);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int id))
                            return id;
                        else
                            return -1;
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        static public bool AddNewPerson(ref int personID, string nationalNO, string firstName, string secondName, string thirdName, string lastname,
            DateTime DateOfBirth, string Nationality, string gendor, string phone, string email, string address, string imagePath)
        {

            using (SqlConnection conn = new SqlConnection(StringConnection))
            {

                string query = "insert into people (nationalno,firstname,secondname,thirdname,lastname,dateofbirth,gendor,address" +
                    ",phone,email,nationalityCountryid,imagepath) " +
                    "values(@nNO,@f,@s,@t,@l,@d,@g,@a,@p,@e,@national,@i)" +
                    " select SCOPE_IDENTITY()";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@nNO", nationalNO);
                    cmd.Parameters.AddWithValue("@f", firstName);
                    cmd.Parameters.AddWithValue("@s", secondName);
                    if (thirdName == string.Empty)
                        cmd.Parameters.AddWithValue("@t", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@t", thirdName);
                    cmd.Parameters.AddWithValue("@l", lastname);
                    cmd.Parameters.AddWithValue("@d", DateOfBirth);
                    if (gendor.ToLower() == "male")
                        cmd.Parameters.AddWithValue("@g", 0);
                    else
                        cmd.Parameters.AddWithValue("@g", 1);

                    cmd.Parameters.AddWithValue("@a", address);
                    cmd.Parameters.AddWithValue("@p", phone);
                    if (email == string.Empty)
                        cmd.Parameters.AddWithValue("@e", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@e", email);

                    cmd.Parameters.AddWithValue("@national", getCountryID(Nationality));

                    if (imagePath == string.Empty)
                        cmd.Parameters.AddWithValue("@i", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@i", imagePath);

                    try
                    {
                        conn.Open();

                        object result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out personID))
                            return true;
                        else
                            return false;
                    }
                    catch (Exception)
                    {

                        throw;

                    }



                }
            }
        }

        static public DataTable CountriesList()
        {
            DataTable dt = new DataTable();

            using (SqlConnection conn = new SqlConnection(StringConnection))
            {
                string query = "select countryname from countries";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {



                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            dt.Load(reader);
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }

            return dt;
        }

        public static bool updatePerson(int personid, string nationalNO, string firstName, string secondName, string thirdName, string lastname,
            DateTime DateOfBirth, string Nationality, string gendor, string phone, string email, string address, string imagePath)
        {
            int affectedrows = 0;
            using (SqlConnection conn = new SqlConnection(StringConnection))
            {
                //@nNO,@f,@s,@t,@l,@d,@g,@a,@p,@e,@national,@i
                string query = "update people " +
                    "set nationalno=@nNO , firstname=@f,secondname=@s,thirdname=@t,lastname=@l,dateofbirth=@d," +
                    "gendor=@g,address=@a,phone=@p,email=@e,nationalitycountryid=@national,imagepath=@i " +
                    "where personid=@id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {

                    cmd.Parameters.AddWithValue("@nNO", nationalNO);
                    cmd.Parameters.AddWithValue("@f", firstName);
                    cmd.Parameters.AddWithValue("@s", secondName);
                    if (thirdName == string.Empty)
                        cmd.Parameters.AddWithValue("@t", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@t", thirdName);
                    cmd.Parameters.AddWithValue("@l", lastname);
                    cmd.Parameters.AddWithValue("@d", DateOfBirth);
                    if (gendor.ToLower() == "male")
                        cmd.Parameters.AddWithValue("@g", 0);
                    else
                        cmd.Parameters.AddWithValue("@g", 1);

                    cmd.Parameters.AddWithValue("@a", address);
                    cmd.Parameters.AddWithValue("@p", phone);
                    if (email == string.Empty)
                        cmd.Parameters.AddWithValue("@e", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@e", email);

                    cmd.Parameters.AddWithValue("@national", getCountryID(Nationality));

                    if (imagePath == string.Empty)
                        cmd.Parameters.AddWithValue("@i", DBNull.Value);
                    else
                        cmd.Parameters.AddWithValue("@i", imagePath);

                    cmd.Parameters.AddWithValue("@id", personid);

                    try
                    {
                        conn.Open();
                        affectedrows = cmd.ExecuteNonQuery();
                    }
                    catch (Exception) { throw; }
                }
            }
            return (affectedrows > 0);
        }

        public static bool DeletePerson(int personid)
        {
            int affectedrows = 0;
            using (SqlConnection conn = new SqlConnection(StringConnection))
            {
                string query = "delete from people where personid=@id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", personid);

                    try
                    {
                        conn.Open();
                        affectedrows = cmd.ExecuteNonQuery();


                    }
                    catch { return false; }
                }
            }
            return (affectedrows > 0);
        }

        public static bool checkIfPersonExistedByID(int personid)
        {

            using (SqlConnection conn = new SqlConnection(StringConnection))
            {
                string query = "select personid from people where personid=@id";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", personid);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return (result != null);


                    }
                    catch (Exception) { throw; }
                }
            }

        }

        public static bool checkIfPersonExistedByNationalNO(string nationalNO)
        {
            using (SqlConnection conn = new SqlConnection(StringConnection))
            {
                string query = "select personid from people where nationalno=@N";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@N", nationalNO);

                    try
                    {
                        conn.Open();
                        object result = cmd.ExecuteScalar();
                        return (result != null);


                    }
                    catch (Exception) { throw; }
                }
            }

        }


        public static int getPersonID(string nationalNO)
        {
            object result = null;

            using (SqlConnection conn = new SqlConnection(StringConnection))
            {
                string query = "select People.PersonID from People where NationalNo=@NO";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@NO", nationalNO);

                    try
                    {
                        conn.Open();

                        result = cmd.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int ID))
                            return ID;
                        return -1;
                    }
                    catch (Exception) { throw; }
                }
            }

        }
    }
}
