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
    public partial class frmRenewDrivingLicense : Form
    {

        private int _localLicenseID { get; set; } = -1;

        private int _newLocalLicenseID=-1;

        private decimal applicationFees=decimal.MinValue;
        private decimal licenseFees=decimal.MinValue;
        public frmRenewDrivingLicense()
        {
            InitializeComponent();
            ctrlFindDriver1.eventIfLicenseFound += behaviordependOnFOundation;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void loadApplicationInfoDependOnLicenseClass()
        {
               licenseFees= clsLicense.getLicenseClassFees(DriverLicenseDTO.className);
            lbLicenseFees.Text =licenseFees.ToString();
          lbTotalFees.Text=(applicationFees+licenseFees).ToString();

        }
        private void behaviordependOnFOundation(bool found)
        {
            linkLabelShowNewLicense.Enabled = false;
            if (found)
            {
                _localLicenseID=ctrlFindDriver1._localLicenseID;
                linklabelShowLicensesHistory.Enabled = true;
                loadApplicationInfoDependOnLicenseClass();



                if (clsLicense.isThatLicenseExpired(_localLicenseID))
                {
                    int licenseid = clsLicense.isPersonHasAlreadyLicenseFromThatClass(DriverLicenseDTO.driverID, DriverLicenseDTO.className);
                    if (licenseid != -1)
                    {
                        MessageBox.Show($"the license is actually expired but this driver has another License from that class with license ID ={licenseid}"
                            , "Behavior", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                    }
                    else
                    {

                        btnRenew.Enabled = true;
                        lbOldLocalLicenseID.Text = _localLicenseID.ToString();
                    }
                }
                else
                {
                    btnRenew.Enabled = false;
                    string expirationdate = clsLicense.Expirationdate(_localLicenseID).ToString();
                    MessageBox.Show($"This License Is Not expired yet it will expired on {expirationdate}:", "not Expired", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }


            }
            else
            {
                _localLicenseID = -1;
                linklabelShowLicensesHistory.Enabled=false;
            }
        }

        private void RenewApplicationInfo()
        {
            lbAppDate.Text = DateTime.Now.ToString();
            lbIssuedate.Text = DateTime.Now.ToString();
            applicationFees = clsApplication.getApplicationFees(2);
            lbApplicationFees.Text =applicationFees.ToString();
            lbCreatedBY.Text = CurrentUser.username;

        }

        private int makeRenewApplication()
        {
            clsApplication renewApplication=new  clsApplication();
            renewApplication._ApplicationStartDate = DateTime.Now;
            renewApplication._ApplicationStatus = "New";
            renewApplication.lastStatusDate = renewApplication._ApplicationStartDate;
            renewApplication.createdByUserID=CurrentUser.userID;
            renewApplication.paidFees = applicationFees;
            //renewApplication._LicenseClassID =clsApplication.getLicenseClassID(DriverLicenseDTO.className);
            renewApplication._TypeID = 2;
            renewApplication._ApplicationPersonID = ClsPerson.getPersonID(DriverLicenseDTO.nationalNo);

            if (renewApplication.Save())
                return renewApplication._applicationID;
            else
                return -1;

        }

        private void renewLicense()
        {
            if (MessageBox.Show("Renew this license?", "Renew", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            int applicationID, newLicenseID;
            if (clsLicense.RenewOrReplace(_localLicenseID, 2, CurrentUser.userID, tbNotes.Text, out applicationID, out newLicenseID))
            {
                _newLocalLicenseID = newLicenseID;
                lbRenewApplicationID.Text = applicationID.ToString();
                lbRenwedLicenseID.Text = newLicenseID.ToString();
                linkLabelShowNewLicense.Enabled = true;
                btnRenew.Enabled = false;
                ctrlFindDriver1.gbFilter.Enabled = false;
                MessageBox.Show("License renewed successfully.", "Renew", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("This license cannot be renewed. Check its status, expiry, and detention state.", "Renew", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        private void linklabelShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int personID=clsLicense.getPersonID(_localLicenseID);
            if (personID != -1)
            {
                frmShowPersonLicenseHistory frmShowPersonLicenseHistory = new frmShowPersonLicenseHistory(personID);
                frmShowPersonLicenseHistory.ShowDialog();
            }
        }

        private void frmRenewDrivingLicense_Load(object sender, EventArgs e)
        {
            RenewApplicationInfo();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            renewLicense();
        }

        private void linkLabelShowNewLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
                if (_newLocalLicenseID != -1)
            {
                frmDrivingLicenseInfo frm = new frmDrivingLicenseInfo(_newLocalLicenseID);
                frm.ShowDialog();
            }
        }
    }
}
