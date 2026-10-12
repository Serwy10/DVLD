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
    public partial class frmAddUpdateUsers : Form
    {
        public delegate void delUpdateUsersTable();
        public event delUpdateUsersTable delUpdate;
        public bool _personExisted = default;
        private int _userID = -1;
        private enum enMode { addNew=0, Update=1 };
        enMode _mode;
        public frmAddUpdateUsers(int userID=-1)
        {
            InitializeComponent();
            tbPassword.UseSystemPasswordChar = true;
            tbConfirmPassword.UseSystemPasswordChar = true;

            _userID = userID;

            if (_userID == -1)
                _mode = enMode.addNew;
            else
            {
                _personExisted=true;
                _mode = enMode.Update;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (_personExisted)
                tabControl1.SelectedTab = tabPage2;
            else
                MessageBox.Show("select person first");

            if (_mode == enMode.addNew)
            {
                if (isThisPersonHasUserAccount())
                    MessageBox.Show("this person is actually has user account", "NotValid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                else
                    tabControl1.SelectedTab = tabPage2;
            }

        }

        private void findByPersonID()
        {

            ClsPerson person=ClsPerson.Find(Convert.ToInt32( tbFilter.Text));

            if (person != null)
            {
                _personExisted = true;
                ctrlShowDetailscs1._personID = person.PersonID;
                ctrlShowDetailscs1.ShowPersonInfo();
            }
            else
            {
                _personExisted=false;
                ctrlShowDetailscs1.refreshDetails();
                MessageBox.Show("No person Existed Has tha Person ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void _findByNationalNo()
        {
            ClsPerson person = ClsPerson.Find(tbFilter.Text);
            if (person != null)
            {
                _personExisted = true;
                ctrlShowDetailscs1._personID = person.PersonID;
                ctrlShowDetailscs1.ShowPersonInfo();
            }

            else
            {
                _personExisted = false;
                ctrlShowDetailscs1.refreshDetails();
                MessageBox.Show("No person Existed Has tha Person ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void WhichWayOfFind()
        {
            switch (comboBox1.SelectedItem.ToString().ToLower())
            {
                case "personid":
                    findByPersonID();
                    break;
                case "national no.":
                    _findByNationalNo();
                    break;

            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().ToLower() == "personid")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (tbFilter.Text == string.Empty)
                return;
            WhichWayOfFind();
        }

        private void frmAddUpdateUsers_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            if (_userID != -1)
            {
                gbFilter.Enabled = false;

                ctrlShowDetailscs1._personID =clsUser.getPersonIdOfUser(_userID);
                ctrlShowDetailscs1.ShowPersonInfo();
                showUserInfo();
            }

            else
                gbFilter.Enabled = true;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddPerson addPerson= new frmAddPerson();
            addPerson._DataBack += showPersonDataAfterAddNewPerson;
            addPerson.ShowDialog();
        }

        private void showPersonDataAfterAddNewPerson(int personID)
        {
            ctrlShowDetailscs1._personID = personID;
            ctrlShowDetailscs1.ShowPersonInfo();
            _personExisted = true;
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex==0)
            {
                if(!int.TryParse(tbFilter.Text, out _))
                    tbFilter.Text = string.Empty;
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {

            if (_mode ==enMode.addNew)
            {
                if (!_personExisted || isThisPersonHasUserAccount())
                    e.Cancel = true;
                else
                    e.Cancel = false;
            }
        }

        private void showUserInfo()
        {
            clsUser User = clsUser.Find(_userID);
            if (User != null)
            {
                lbUserID.Text=User.userID.ToString();
                tbUserName.Text=User.username.ToString();
                tbPassword.Clear();
                tbConfirmPassword.Clear();
                new ToolTip().SetToolTip(tbPassword, "Leave blank to keep the current password.");

                cbActive.Checked = User.IsActive;
            }
        }
        private void tbUserName_Validating(object sender, CancelEventArgs e)
        {
            if (clsUser.IsUserNameExsted(tbUserName.Text) && (_mode == enMode.addNew || clsUser.Find(_userID)?.username != tbUserName.Text))
            {

                errorProvider1.SetError((TextBox)sender, "this userName Already Existed enter another one");
            }

            else
            {

                errorProvider1.SetError((TextBox)sender, "");
            }
        }

        private bool IsPasswordValid()
        {
            return (_mode == enMode.Update && tbPassword.Text.Length == 0) || tbPassword.Text.Length >= 8;

        }

        private bool isThisPersonHasUserAccount()
        {
            return clsUser.isThisPersonHasUserAcc(ctrlShowDetailscs1._personID);
        }

        private bool checkPasswordAndConfirmationIsTheSame()
        {
            return(tbPassword.Text==tbConfirmPassword.Text);
        }

        private void tbPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!IsPasswordValid())
            {

                errorProvider1.SetError((TextBox)sender, "password should at least 8 charcters");
            }
            else
            {

                errorProvider1.SetError((TextBox)sender, "");
            }
        }

        private void tbConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (!checkPasswordAndConfirmationIsTheSame())
            {
                errorProvider1.SetError((TextBox)sender, "Confirmation is not same as Password");
            }
            else
                errorProvider1.SetError((TextBox)sender, "");

        }

        private bool CheckValidUserInfo()
        {
            if (tbUserName.Text == string.Empty)
            {
                errorProvider1.SetError(tbUserName, "enter UserName");
                return false;
            }
            else
                errorProvider1.SetError(tbUserName, "");



            if (!IsPasswordValid())
            {
                errorProvider1.SetError(tbPassword, "enter valid password that contins at least 8 char")
                    ; return false;
            }
            else
                errorProvider1.SetError(tbPassword, "");

            if (!checkPasswordAndConfirmationIsTheSame())
            {
                errorProvider1.SetError(tbConfirmPassword, "password antd its confirmation should be the same")
                    ; return false;
            }
            else
                errorProvider1.SetError(tbConfirmPassword, "");

            return true;
        }

        private void saveNewUser()
        {
            if (!CheckValidUserInfo())
                return;
            if (clsUser.IsUserNameExsted(tbUserName.Text) && (_mode == enMode.addNew || clsUser.Find(_userID)?.username != tbUserName.Text))
            {
                errorProvider1.SetError(tbUserName, "this userName AlreadyExisted enter Another one");
                return;
            }
            else
                errorProvider1.SetError(tbUserName, "");

            clsUser user=new clsUser();
            user.personID = ctrlShowDetailscs1._personID;
            user.username=tbUserName.Text;
            user.password=tbPassword.Text;
            user.IsActive=cbActive.Checked;

            if (user.Save())
            {
                lbUserID.Text=user.userID.ToString();
                delUpdate?.Invoke();
                _userID=user.userID;
                lbTitle.Text = "Update User";
                _mode=enMode.Update;
                MessageBox.Show("User Added Successfully");
            }

        }

        private void _updateUser()
        {
            if(!CheckValidUserInfo())
                return;


            clsUser user = clsUser.Find(_userID);


            if (user == null)
                return;

            if (user.username != tbUserName.Text)
            {
                if (clsUser.IsUserNameExsted(tbUserName.Text) && (_mode == enMode.addNew || clsUser.Find(_userID)?.username != tbUserName.Text))
                {
                    errorProvider1.SetError(tbUserName, "this userName AlreadyExisted enter Another one");
                    return;
                }
                else
                    errorProvider1.SetError(tbUserName, "");
            }
            user.username = tbUserName.Text;
            user.password=tbPassword.Text;
            user.IsActive=cbActive.Checked;

            if (user.Save()) {

                delUpdate?.Invoke();
                MessageBox.Show("User Updated Successfully");
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            switch (_mode) {
                case enMode.addNew:
            saveNewUser();
                    break;
                case enMode.Update:
                    _updateUser();
                    break;


        }
        }
    }
}
