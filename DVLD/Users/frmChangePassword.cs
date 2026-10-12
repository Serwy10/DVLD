using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLogic2;

namespace DVLD
{
    public partial class frmChangePassword : Form
    {
        private int _personID{ get; set; }
        private clsUser user = null;
        public frmChangePassword(int personID)
        {
            InitializeComponent();
            tbCurrentPassword.UseSystemPasswordChar = true;
            tbNewPassword.UseSystemPasswordChar = true;
            tbConfirmPassword.UseSystemPasswordChar = true;

            _personID = personID;
            user=clsUser.FindbyPersonID(personID);

            ctrlUserInfo1.Controls.Clear();
            ctrlUserInfo newControl = new ctrlUserInfo(_personID);
            newControl.Dock= DockStyle.Fill;
            ctrlUserInfo1.Controls.Add(newControl);
        }

        private  bool isPasswordRight()
        {
            return user != null && clsUser.isValidLogin(user.username, tbCurrentPassword.Text);
        }

        private void tbCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!isPasswordRight()) {

                errorProvider1.SetError((TextBox)sender, "Password Wrong");
            }
            else
            {

                errorProvider1.SetError((TextBox)sender, "");
            }
        }



        private void saveNewPassword()
        {
            if (!isPasswordRight())
            {
                MessageBox.Show("the current password is wrong,enter hte right password");
                return;
            }

            if (tbNewPassword.Text.Length < 8 || tbNewPassword.Text != tbConfirmPassword.Text)
            {
                MessageBox.Show("New Password and Confirm Password Should to be Same");
                return;
            }

          if(  clsUser.saveNewPassword(user.username,tbNewPassword.Text))
                MessageBox.Show("The Password Saved Successfully","Saving Password",MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error,Something Wrong and Nothing Updated", "Saving Password", MessageBoxButtons.OK, MessageBoxIcon.Error);


        }



        private void tbNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbNewPassword.Text.Length < 8)
            {
                tbConfirmPassword.Visible = false;
                errorProvider1.SetError((TextBox)sender, "enter valid password that consists of at least 8 characters");
            }
            else
            {
                tbConfirmPassword.Visible = true;
                errorProvider1.SetError((TextBox)sender, "");
            }

        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (tbConfirmPassword.Text != tbNewPassword.Text)
                errorProvider1.SetError((TextBox)sender, "Confirm password should equal the new password");
            else
                errorProvider1.SetError((TextBox)sender, "");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            saveNewPassword();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
