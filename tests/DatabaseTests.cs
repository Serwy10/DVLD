using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text.RegularExpressions;
using DVLDDataAccess;
using DVLDBusinessLogic2;

namespace DVLD.Tests
{
    internal static class DatabaseTests
    {
        private static object Sql(string connectionString, string sql, params object[] parameters)
        {
            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
            {
                for (int i=0; i<parameters.Length; i+=2) command.Parameters.AddWithValue((string)parameters[i], parameters[i+1]);
                connection.Open();
                return command.ExecuteScalar();
            }
        }
        private static int Count(string cs, string table) => Convert.ToInt32(Sql(cs, "SELECT COUNT(*) FROM dbo." + table));
        private static int Person(string cs, string national) => Convert.ToInt32(Sql(cs, @"
INSERT dbo.People(NationalNo,FirstName,SecondName,LastName,DateOfBirth,Gendor,Address,Phone,NationalityCountryID)
VALUES(@national,N'Test',N'Synthetic',N'Person','19900101',0,N'Test address',N'0000000000',1); SELECT SCOPE_IDENTITY();", "@national", national));
        private static void ExpectSqlFailure(Action action)
        {
            bool failed=false;
            try { action(); } catch (SqlException) { failed=true; }
            Program.Assert(failed, "Injected database failure must reach the caller.");
        }
        private static void Script(SqlConnection connection, string content)
        {
            foreach (string batch in Regex.Split(content, @"^\s*GO\s*$", RegexOptions.Multiline | RegexOptions.IgnoreCase))
                if (!string.IsNullOrWhiteSpace(batch)) using (var command = new SqlCommand(batch, connection)) command.ExecuteNonQuery();
        }

        public static void Run(string serverConnection, string root)
        {
            string name="DVLD_Test_" + Guid.NewGuid().ToString("N");
            var master = new SqlConnectionStringBuilder(serverConnection) { InitialCatalog="master" };
            var test = new SqlConnectionStringBuilder(serverConnection) { InitialCatalog=name };
            string previous=Environment.GetEnvironmentVariable("DVLD_CONNECTION_STRING");
            try
            {
                using (var connection = new SqlConnection(master.ConnectionString))
                {
                    connection.Open();
                    string schema=File.ReadAllText(Path.Combine(root,"database","01-schema.sql"));
                    Script(connection, Regex.Replace(schema,@"\bDVLD\b",name));
                    Script(connection,Regex.Replace(File.ReadAllText(Path.Combine(root,"database","02-reference-data.sql")),@"\bDVLD\b",name));
                }
                string cs=test.ConnectionString;
                Environment.SetEnvironmentVariable("DVLD_CONNECTION_STRING",cs);
                Program.Assert(ManageApplications.LocalDrivingApplicationsTable().Columns.Contains("LDLAppID"),"Empty local-application list must retain columns.");
                Program.Assert(manageDetainLicenseData.detainedLicensesTable().Columns.Contains("DetainID"),"Empty detention list must retain columns.");
                Program.Assert(manageDriversData.DriversTable().Columns.Contains("DriverID"),"Empty driver list must retain columns.");
                Program.Assert(UsersAccessData.UsersTable().Columns.Contains("UserID"),"Empty user list must retain columns.");
                int adminPerson=Person(cs,"ADMIN-TEST");
                int userID=Convert.ToInt32(Sql(cs,"INSERT Users(PersonID,UserName,Password,IsActive) VALUES(@person,N'admin',@password,1); SELECT SCOPE_IDENTITY();",
                    "@person",adminPerson,"@password",PasswordHasher.Hash("Test account password 123!")));
                int legacyPerson=Person(cs,"LEGACY-TEST");
                Sql(cs,"INSERT Users(PersonID,UserName,Password,IsActive) VALUES(@person,N'legacy',N'Legacy test password',1)","@person",legacyPerson);
                Program.Assert(clsUser.isValidLogin("legacy","Legacy test password"),"Original database credentials must continue working.");
                Program.Assert(PasswordHasher.IsHash((string)Sql(cs,"SELECT Password FROM Users WHERE UserName=N'legacy'")),"Legacy login must upgrade plaintext.");
                Sql(cs,"UPDATE Users SET IsActive=0 WHERE UserName=N'legacy'");
                Program.Assert(!clsUser.isValidLogin("legacy","Legacy test password"),"Inactive users cannot log in.");
                var existing=clsUser.Find(userID);
                string before=(string)Sql(cs,"SELECT Password FROM Users WHERE UserID=@id","@id",userID);
                existing.password=""; existing.IsActive=true;
                Program.Assert(existing.Save(),"User can be edited without changing the password.");
                Program.Assert(before==(string)Sql(cs,"SELECT Password FROM Users WHERE UserID=@id","@id",userID),"Blank edit preserves hash.");
                var retryUser=new clsUser { personID=-1,username="retry",password="Retry password 123!",IsActive=true };
                ExpectSqlFailure(()=>retryUser.Save());
                retryUser.personID=Person(cs,"RETRY-TEST");
                Program.Assert(retryUser.Save(),"Failed user insert must remain retryable.");
                int applicant=Person(cs,"APPLICANT-TEST");
                var app=new clsApplication { _ApplicationPersonID=applicant,_ApplicationStartDate=DateTime.Now,_TypeID=1,
                    _ApplicationStatus="New",lastStatusDate=DateTime.Now,paidFees=15,createdByUserID=userID,_LicenseClassID=3 };
                Program.Assert(app.Save(),"Local application and details must be created.");
                int localID=Convert.ToInt32(Sql(cs,"SELECT LocalDrivingLicenseApplicationID FROM LocalDrivingLicenseApplications WHERE ApplicationID=@id","@id",app._applicationID));
                Program.Assert(LicenseWorkflowData.IssueFirst(applicant,app._applicationID,3,DateTime.Now,DateTime.Now.AddYears(10),"",20,userID)==null,"Issuance requires three different passed test types.");
                int premature=-1;
                Program.Assert(!ManageTestAppointmentsData.saveNewTestAppointment(ref premature,localID,DateTime.Now,3,10,userID,false),"Street appointment requires the earlier test types.");
                for (int type=1;type<=3;type++)
                {
                    int appointment=-1;
                    Program.Assert(ManageTestAppointmentsData.saveNewTestAppointment(ref appointment,localID,DateTime.Now,type,10,userID,false),"Appointment must follow the passed-test order.");
                    int duplicate=-1;
                    Program.Assert(!ManageTestAppointmentsData.saveNewTestAppointment(ref duplicate,localID,DateTime.Now,type,10,userID,false),"Only one open appointment per test type.");
                    int testID=-1;
                    if (type==1)
                    {
                        Sql(cs,"CREATE TRIGGER FailTest ON Tests AFTER INSERT AS THROW 51003,'Injected test failure',1;");
                        ExpectSqlFailure(()=>ManageTestsData.saveNewTest(ref testID,appointment,true,"",userID));
                        Sql(cs,"DROP TRIGGER FailTest");
                        Program.Assert(!clsTestAppointment.checkIfAppointmentIsLocked(appointment),"Failed test insert must roll back the lock.");
                    }
                    Program.Assert(ManageTestsData.saveNewTest(ref testID,appointment,true,"",userID),"Test result and lock must save together.");
                    Program.Assert(!ManageTestsData.saveNewTest(ref testID,appointment,false,"",userID),"A test result cannot be recorded twice.");
                }
                Sql(cs,"CREATE TRIGGER FailLicense ON Licenses AFTER INSERT AS THROW 51004,'Injected issuance failure',1;");
                ExpectSqlFailure(()=>LicenseWorkflowData.IssueFirst(applicant,app._applicationID,3,DateTime.Now,DateTime.Now.AddYears(10),"",20,userID));
                Sql(cs,"DROP TRIGGER FailLicense");
                Program.Assert(Count(cs,"Drivers")==0 && Count(cs,"Licenses")==0,"Failed issuance must roll back the driver and license.");
                Program.Assert(Convert.ToInt32(Sql(cs,"SELECT ApplicationStatus FROM Applications WHERE ApplicationID=@id","@id",app._applicationID))==1,"Failed issuance must leave application new.");
                var first=LicenseWorkflowData.IssueFirst(applicant,app._applicationID,3,DateTime.Now,DateTime.Now.AddYears(10),"",20,userID);
                Program.Assert(first!=null,"First license must issue after the three tests.");
                Program.Assert(LicenseWorkflowData.IssueFirst(applicant,app._applicationID,3,DateTime.Now,DateTime.Now.AddYears(10),"",20,userID)==null,"Application cannot issue twice.");
                DateTime expiry=(DateTime)Sql(cs,"SELECT ExpirationDate FROM Licenses WHERE LicenseID=@id","@id",first.LicenseID);
                int appCount=Count(cs,"Applications");
                Sql(cs,"CREATE TRIGGER FailReplacement ON Licenses AFTER INSERT AS THROW 51005,'Injected replacement failure',1;");
                ExpectSqlFailure(()=>LicenseWorkflowData.RenewOrReplace(first.LicenseID,3,userID,""));
                Sql(cs,"DROP TRIGGER FailReplacement");
                Program.Assert(Count(cs,"Applications")==appCount,"Failed replacement must not leave an orphan application.");
                Program.Assert(clsLicense.isThatLicenseActive(first.LicenseID),"Failed replacement must leave old license active.");
                var replacement=LicenseWorkflowData.RenewOrReplace(first.LicenseID,3,userID,"");
                Program.Assert(replacement!=null,"Active license can be replaced.");
                Program.Assert((DateTime)Sql(cs,"SELECT ExpirationDate FROM Licenses WHERE LicenseID=@id","@id",replacement.LicenseID)==expiry,"Replacement preserves expiry.");
                Program.Assert(!clsLicense.isThatLicenseActive(first.LicenseID),"Replaced license must be inactive.");
                Program.Assert((decimal)Sql(cs,"SELECT PaidFees FROM Licenses WHERE LicenseID=@id","@id",replacement.LicenseID)==0m,"Replacement must not charge class fees again.");
                int detainID=-1;
                Program.Assert(manageDetainLicenseData.saveNewDetainedLicense(ref detainID,replacement.LicenseID,DateTime.Now,10,userID),"License can be detained.");
                int duplicateDetain=-1;
                Program.Assert(!manageDetainLicenseData.saveNewDetainedLicense(ref duplicateDetain,replacement.LicenseID,DateTime.Now,10,userID),"Only one open detention per license.");
                Program.Assert(LicenseWorkflowData.RenewOrReplace(replacement.LicenseID,4,userID,"")==null,"Detained license cannot be replaced.");
                Program.Assert(LicenseWorkflowData.IssueInternational(replacement.LicenseID,userID)==null,"Detained license cannot issue internationally.");
                appCount=Count(cs,"Applications");
                Sql(cs,"CREATE TRIGGER FailRelease ON DetainedLicenses AFTER UPDATE AS THROW 51006,'Injected release failure',1;");
                ExpectSqlFailure(()=>LicenseWorkflowData.Release(replacement.LicenseID,userID));
                Sql(cs,"DROP TRIGGER FailRelease");
                Program.Assert(Count(cs,"Applications")==appCount && clsDetainedLicense.isLicenseDetained(replacement.LicenseID),"Failed release rolls back its application and detention update.");
                Program.Assert(LicenseWorkflowData.Release(replacement.LicenseID,userID)!=null,"Detained license can be released.");
                Program.Assert(LicenseWorkflowData.Release(replacement.LicenseID,userID)==null,"Released license cannot be released twice.");
                var international=LicenseWorkflowData.IssueInternational(replacement.LicenseID,userID);
                Program.Assert(international!=null,"Valid Class 3 license can issue internationally.");
                Program.Assert(LicenseWorkflowData.IssueInternational(replacement.LicenseID,userID)==null,"Driver cannot hold duplicate current international licenses.");
                Sql(cs,"UPDATE Licenses SET IssueDate=DATEADD(year,-11,GETDATE()),ExpirationDate=DATEADD(day,-1,GETDATE()) WHERE LicenseID=@id","@id",replacement.LicenseID);
                var renewed=LicenseWorkflowData.RenewOrReplace(replacement.LicenseID,2,userID,"");
                Program.Assert(renewed!=null && clsLicense.isThatLicenseActive(renewed.LicenseID),"Expired active license must renew.");
                Console.WriteLine("PASS: SQL Server schema and workflow integration tests.");
            }
            finally
            {
                Environment.SetEnvironmentVariable("DVLD_CONNECTION_STRING",previous);
                SqlConnection.ClearAllPools();
                object exists = Sql(master.ConnectionString, "SELECT DB_ID(@name)", "@name", name);
                if (exists != null && exists != DBNull.Value) Sql(master.ConnectionString,"ALTER DATABASE ["+name+"] SET SINGLE_USER WITH ROLLBACK IMMEDIATE; DROP DATABASE ["+name+"];");
            }
        }
    }
}
