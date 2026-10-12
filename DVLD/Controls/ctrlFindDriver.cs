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
    public partial class ctrlFindDriver : UserControl
    {
        public delegate void delBehavior(bool found);
        public event delBehavior eventIfLicenseFound;
        public int _localLicenseID { get;  set; } = -1;
        public ctrlFindDriver()
        {
            InitializeComponent();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && (!char.IsControl(e.KeyChar)))
                e.Handled = true;

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!int.TryParse(textBox1.Text, out int _))
                textBox1.Text = string.Empty;
        }

        private void searchingByLicenseID()
        {
            if (int.TryParse(textBox1.Text, out int LicenseID))
            {

                if (clsLicense.isThatLicenseExisted(LicenseID))
                {

                    _localLicenseID = LicenseID;

                    ctrlLicenseInfo1.localLicenseID = _localLicenseID;
                    ctrlLicenseInfo1.showLicenseInfo();
                    eventIfLicenseFound?.Invoke(true);



                }
                else
                {
                    _localLicenseID = -1;
                    MessageBox.Show("No License found With That ID", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlLicenseInfo1.resetControl();
                    DriverLicenseDTO.reset();
                    eventIfLicenseFound?.Invoke(false);
                }

            }


        }
        private void btnSearch_Click(object sender, EventArgs e)
        {
           searchingByLicenseID();
        }
    }
}
