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
    public partial class CtrlAddNewPerson : UserControl
    {
        string defaultImage = AppPaths.Asset("male.png");
        public CtrlAddNewPerson()
        {
            InitializeComponent();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox1.ImageLocation = defaultImage;
            linkLabelRemove.Visible = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void loadCountriedListIntoComboBox()
        {
            foreach(DataRow row in ClsPerson.CountriesList().Rows)
            {
                cbCountries.Items.Add(row["countryname"]);
            }


        }

        private string PersonGendor()
        {
            if (rbMale.Checked)
                return "Male";
            else
                return "Female";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ClsPerson person = new ClsPerson();

           person.NationalNum=tbNationalNo.Text;
            person.FirstName=tbFirstname.Text;
            person.LastName=tbLastName.Text;
            person.Email=tbEmail.Text;
            person.Phone=tbPhone.Text;
            person.Address=tbAddress.Text;
            person.SecondName=tbSecondName.Text;
            person.ThirdName=tbThirdName.Text;
            person.Gendor = PersonGendor();
            person.DateOfBirth = dateTimePicker1.Value;
            person.Nationality=cbCountries.SelectedItem.ToString();

            if (pictureBox1.ImageLocation == defaultImage)
                person.ImagePath = string.Empty;
            else

                person.ImagePath = pictureBox1.ImageLocation;

            if (person.Save())
            {
                lbPersonID.Text=person.PersonID.ToString();

                MessageBox.Show($"Person Added Successfully with person id = {person.PersonID}");
            }
            else
            {
                MessageBox.Show("Something went wrong and nothing added ");
            }

        }

        private void CtrlAddNewPerson_Load(object sender, EventArgs e)
        {
            loadCountriedListIntoComboBox();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            OpenFileDialog fileDialog = new OpenFileDialog();

            fileDialog.Title = "Select Picture";
            fileDialog.InitialDirectory = Environment.CurrentDirectory;
            fileDialog.ShowDialog();

            pictureBox1.ImageLocation=fileDialog.FileName;
            linkLabelRemove.Visible = true;

        }
    }
}
