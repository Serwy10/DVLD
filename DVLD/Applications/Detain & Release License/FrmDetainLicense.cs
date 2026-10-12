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
    public partial class FrmDetainLicense : Form
    {

        private int _localLicenseID = -1;

        public delegate void DelLoad();
        public event DelLoad eventload;
        public FrmDetainLicense()
        {
            InitializeComponent();
            ctrlFindDriver1.eventIfLicenseFound += behaviorAfterSearchingLicense;
        }


        private void behaviorAfterSearchingLicense(bool found)
        {
            if (found)
            {
                linklabelShowLicensesHistory.Enabled = true;
                _localLicenseID = ctrlFindDriver1._localLicenseID;
                CheckIfLicenseAlreadyDetainedorNotActive();

            }
            else
            {
                linklabelShowLicensesHistory.Enabled= false;
                btnDetain.Enabled = false;
                _localLicenseID=-1;
            }
        }

        private void CheckIfLicenseAlreadyDetainedorNotActive()
        {

            if(!clsDetainedLicense.isLicenseDetained(_localLicenseID))
            {
                if (clsLicense.isThatLicenseActive(_localLicenseID))
                {
                    lbLicenseID.Text = _localLicenseID.ToString();
                    btnDetain.Enabled = true;
                }
                else
                {
                    MessageBox.Show("this license is not active so you can't detain it", "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    btnDetain.Enabled = false;
                }
            }
            else
            {
                btnDetain.Enabled = false;
                MessageBox.Show("this license is already Detained", "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand);

            }


        }

        private void showDetainLicenseApplicationInfo()
        {
            lbCreatedBY.Text = CurrentUser.username;
            lbDetainDate.Text = DateTime.Now.ToString();


        }

        private void detainLicense()
        {
            if(tbFineFees.Text==string.Empty)
            {
                errorProvider1.SetError(tbFineFees, "Enter fine amount");
                return;
            }

            clsDetainedLicense license = new clsDetainedLicense();

            license.licenseID = _localLicenseID;
            license.detainDate= DateTime.Now;

           if (!decimal.TryParse(tbFineFees.Text, out decimal fineFees) || fineFees < 0)
            {
                MessageBox.Show("Enter a valid nonnegative fine.", "Detain", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            license.fineFees = fineFees;
            license.createdByUserID = CurrentUser.userID;
            license.isRealesed = false;

            if(MessageBox.Show("are you sure you want to detain this license ? ","Detain",MessageBoxButtons.YesNo,MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                if (license.saveNewDetainLicense())
                {
                    MessageBox.Show("license Detained Successfully", "Detained", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnDetain.Enabled = false;
                    linkLabelShowNewLicense.Enabled = true;
                    lbDetainID.Text = license.detainID.ToString();

                    eventload?.Invoke();
                }

                else
                {
                    MessageBox.Show("An error occured and license didn't detained , try again","error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void linklabelShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int personID=clsLicense.getPersonID(_localLicenseID);
            frmShowPersonLicenseHistory frmShow = new frmShowPersonLicenseHistory(personID);
            frmShow.ShowDialog();
        }

        private void linkLabelShowNewLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDrivingLicenseInfo frm = new frmDrivingLicenseInfo(_localLicenseID);
            frm.ShowDialog();
        }

        private void FrmDetainLicense_Load(object sender, EventArgs e)
        {
            showDetainLicenseApplicationInfo();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            detainLicense();
        }
    }
}
