using System;
using System.Data;
using System.Linq;
using DVLDDataAccess;
using DVLDBusinessLogic2;

namespace DVLD.Tests
{
    internal static class Program
    {
        private static int assertions;
        internal static void Assert(bool value, string message)
        {
            if (!value) throw new Exception(message);
            assertions++;
        }

        static int Main(string[] args)
        {
            try
            {
                string hash = PasswordHasher.Hash("A valid password 123!");
                Assert(hash != "A valid password 123!" && PasswordHasher.IsHash(hash), "Password must be stored as a hash.");
                Assert(PasswordHasher.Verify("A valid password 123!", hash), "Correct password must verify.");
                Assert(!PasswordHasher.Verify("Wrong password!", hash), "Incorrect password must fail.");
                Assert(hash != PasswordHasher.Hash("A valid password 123!"), "Each password must get a random salt.");
                foreach (string broken in new[] { "", "plaintext", "PBKDF2-SHA256$0$a$b", "PBKDF2-SHA256$600000$invalid$invalid", "PBKDF2-SHA256$2147483647$a$b" })
                    Assert(!PasswordHasher.Verify("password", broken), "Malformed hashes must fail safely.");
                var table = new DataTable();
                table.Columns.Add("Active", typeof(bool));
                table.Columns.Add("ID", typeof(int)); table.Columns.Add("Name", typeof(string));
                table.Rows.Add(true, 1, "O'Brien"); table.Rows.Add(false, 2, "100% [*] complete"); table.Rows.Add(true, 3, "Ordinary");
                Assert(TableFilter.Text(table, "Name", "O'B").Count == 1, "Apostrophe searches must work.");
                Assert(TableFilter.Text(table, "Name", "%").Count == 1, "Percent must be literal.");
                Assert(TableFilter.Text(table, "Name", "[*]").Count == 1, "Brackets and asterisks must be literal.");
                Assert(TableFilter.Text(table, "Name", "' OR 1=1").Count == 0, "Input cannot become a filter expression.");
                Assert(TableFilter.Number(table, "ID", "2").Count == 1, "ID filtering must work.");
                Assert(TableFilter.Number(table, "ID", "2 OR 1=1").Count == 0, "Numeric expression injection must fail.");
                Assert(TableFilter.Number(table, "ID", "").Count == 3, "Blank filter restores all rows.");
                Assert(TableFilter.Number(table, "Active", "true").Count == 2, "Active-user filtering must work.");
                Assert(TableFilter.Number(table, "Active", "false").Count == 1, "Inactive-user filtering must work.");
                Assert(TableFilter.Number(table, "Active", "false OR true").Count == 0, "Boolean expressions must be rejected.");
                bool rejected = false;
                try { TableFilter.Text(table, "Unknown", "a"); } catch (ArgumentException) { rejected = true; }
                Assert(rejected, "Unknown columns must be rejected.");
                string server = Environment.GetEnvironmentVariable("DVLD_TEST_SERVER_CONNECTION");
                if (!string.IsNullOrWhiteSpace(server))
                    DatabaseTests.Run(server, args.Length == 0 ? Environment.CurrentDirectory : args[0]);
                else Console.WriteLine("SQL integration tests skipped: set DVLD_TEST_SERVER_CONNECTION to a disposable test server.");
                Console.WriteLine("PASS: " + assertions + " regression assertions.");
                return 0;
            }
            catch (Exception exception) { Console.Error.WriteLine(exception); return 1; }
        }
    }
}
