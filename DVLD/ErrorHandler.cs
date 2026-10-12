using System;
using System.Data.SqlClient;
using System.IO;
using System.Windows.Forms;

namespace DVLD
{
    internal static class ErrorHandler
    {
        public static void Handle(Exception exception)
        {
            try
            {
                Directory.CreateDirectory(AppPaths.DataDirectory);
                File.AppendAllText(Path.Combine(AppPaths.DataDirectory, "errors.log"),
                    DateTime.UtcNow.ToString("O") + " " + exception.GetType().FullName +
                    Environment.NewLine + exception.StackTrace + Environment.NewLine);
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
            var sql = exception as SqlException;
            string message = sql != null && (sql.Number == 2601 || sql.Number == 2627)
                ? "This record already exists. Refresh the list and check your input."
                : sql != null && sql.Number == 547
                    ? "This record is linked to other data or contains an invalid reference."
                    : "The operation could not be completed. Check your input, database setup, and connection, then try again.";
            MessageBox.Show(message, "DVLD", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
