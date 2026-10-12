using System;
using System.Windows.Forms;
using DVLDBusinessLogic2;

namespace DVLD
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, e) => ErrorHandler.Handle(e.Exception);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            while (true)
            {
                using (var login = new frmLogincs())
                    if (login.ShowDialog() != DialogResult.OK) break;
                DialogResult result;
                using (var main = new MainForm()) result = main.ShowDialog();
                CurrentUser.username = string.Empty;
                CurrentUser.userID = -1;
                CurrentUser.PersonID = -1;
                CurrentUser.isActive = false;
                if (result != DialogResult.Retry) break;
            }
        }
    }
}
