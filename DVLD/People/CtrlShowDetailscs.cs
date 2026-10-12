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
    public partial class CtrlShowDetailscs : UserControl
    {
        public int _personID { get; set; } = -1;
        public string _nationaNO { get; set; } = string.Empty;

        public bool personFound = false;
        public CtrlShowDetailscs()
        {
            InitializeComponent();


        }


        public void ShowPersonInfo()
        {
            ClsPerson person = null;
            if (_personID != -1)
                 person = ClsPerson.Find(_personID);
            else
                person = ClsPerson.Find(_nationaNO);



            if (person == null)
            {
                MessageBox.Show($"No person Existed with that data", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                refreshDetails();
                personFound = false;
                return ;
            }

            _personID=person.PersonID;
            lbPersonID.Text=person.PersonID.ToString();
            lbName.Text=person.FirstName+' '+person.LastName;
            lbNationaNO.Text = person.NationalNum;
            lbGendor.Text = person.Gendor;
            lbEmail.Text = person.Email;
            lbAddress.Text = person.Address;
            lbPhone.Text = person.Phone;
            lbDateOfBirth.Text=person.DateOfBirth.ToString();
            lbCountry.Text=person.Nationality;
            if(person.ImagePath!= string.Empty)
            personIMG.ImageLocation=person.ImagePath;
            else
            {
                if (lbGendor.Text.ToLower() == "male")
                    personIMG.ImageLocation = AppPaths.Asset("male.png");
                else
                    personIMG.ImageLocation = AppPaths.Asset("male-and-female.png");

            }
            personFound = true;
            linkLabel1.Enabled = true;

        }

        public void refreshDetails()
        {
            _personID = -1;
            personFound = false;
            linkLabel1.Enabled = false;
            lbDateOfBirth.Text = string.Empty;
            lbCountry.Text = string.Empty;
            lbPersonID.Text=string.Empty;
            lbName.Text=string.Empty;
            lbNationaNO.Text=string.Empty;
            lbGendor.Text=string.Empty;
            lbEmail.Text=string.Empty;
            lbAddress.Text=string.Empty;
            lbPhone.Text=string.Empty;
            personIMG.ImageLocation = AppPaths.Asset("male.png");

        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddPerson frmAddPerson = new frmAddPerson(_personID);
            frmAddPerson._Refresh += ShowPersonInfo;
            frmAddPerson.ShowDialog();
        }

        private void CtrlShowDetailscs_Load(object sender, EventArgs e)
        {
            //ShowPersonInfo();
        }


    }
}
