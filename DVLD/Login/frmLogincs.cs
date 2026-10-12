using System;
using System.IO;
using System.Windows.Forms;
using DVLDBusinessLogic2;

namespace DVLD
{
    public partial class frmLogincs : Form
    {
        private static string RememberedUserFile => Path.Combine(AppPaths.DataDirectory, "remembered-user.txt");

        public frmLogincs()
        {
            InitializeComponent();
            tbPassword.UseSystemPasswordChar = true;
            cbRememberme.Text = "Remember username";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string username = tbUserName.Text.Trim();
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrEmpty(tbPassword.Text))
            {
                MessageBox.Show("Enter your username and password.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                if (!clsUser.isValidLogin(username, tbPassword.Text))
                {
                    MessageBox.Show("Invalid credentials or an inactive account.", "Login", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                clsUser user = clsUser.Find(username);
                if (user == null || !user.IsActive) return;
                CurrentUser.username = user.username;
                CurrentUser.userID = user.userID;
                CurrentUser.PersonID = user.personID;
                CurrentUser.isActive = user.IsActive;
                try
                {
                    if (cbRememberme.Checked)
                    {
                        Directory.CreateDirectory(AppPaths.DataDirectory);
                        File.WriteAllText(RememberedUserFile, username);
                    }
                    else if (File.Exists(RememberedUserFile)) File.Delete(RememberedUserFile);
                }
                catch (IOException) { }
                catch (UnauthorizedAccessException) { }
                tbPassword.Clear();
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex) { ErrorHandler.Handle(ex); }
        }

        private void frmLogincs_Load(object sender, EventArgs e)
        {
            try
            {
                if (!File.Exists(RememberedUserFile)) return;
                tbUserName.Text = File.ReadAllText(RememberedUserFile).Trim();
                cbRememberme.Checked = tbUserName.Text.Length > 0;
            }
            catch (IOException) { }
            catch (UnauthorizedAccessException) { }
        }
    }
}
