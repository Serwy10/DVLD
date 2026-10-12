using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLogic2;

namespace DVLD
{
    public partial class CtrlPersonCardcs : UserControl
    {
        public CtrlPersonCardcs()
        {
            InitializeComponent();
        }

        private void FillPersonCard()
        {
            ClsPerson person = null;
            bool checkedByID = false;
            object searchby = textBox1.Text;
            if (comboBox1.SelectedItem == null) {
                MessageBox.Show("select what you want to search by");
            return;
        }
            switch (comboBox1.SelectedItem.ToString())
            {
                case "PersonID":
                    person = ClsPerson.Find(Convert.ToInt32( textBox1.Text));
                    checkedByID = true;
                    break;
                case "NationalNO":
                    person = ClsPerson.Find(textBox1.Text);
                    checkedByID = false;
                    break;


            }


            if (person != null) {
                lbPesonId.Text=person.PersonID.ToString();
                lbNationalNO.Text=person.NationalNum.ToString();
                lbFullName.Text = person.FullName();
                lbDateOfBirth.Text = person.DateOfBirth.ToString();
                lbGendor.Text = person.Gendor.ToString();
                lbNationality.Text = person.Nationality.ToString();
                lbAddress.Text = person.Address.ToString();
                lbPhone.Text = person.Phone.ToString();
                lbEmail.Text = person.Email.ToString();
                lbImagePath.Text = person.ImagePath.ToString();

            }

            else
            {
                lbPesonId.Text =string.Empty;
                lbNationalNO.Text = string.Empty;
                lbFullName.Text = string.Empty ;
                lbDateOfBirth.Text=string.Empty ;
                lbGendor.Text = string.Empty;
                lbNationality.Text=string.Empty ;
                lbAddress.Text =string.Empty ;
                lbPhone.Text = string.Empty;
                lbEmail.Text = string.Empty;
                lbImagePath.Text = string.Empty;

                if(checkedByID)
                MessageBox.Show($"ERRor failed to find this person with id ={searchby}");
                else
                    MessageBox.Show($"ERRor failed to find this person with nationalNO ={searchby}");


            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
                return;

            FillPersonCard();


        }
    }
}
