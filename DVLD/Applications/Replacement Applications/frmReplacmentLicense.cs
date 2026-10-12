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
    public partial class frmReplacmentLicense : Form
    {

        private int _localLicenseID = -1;
        private decimal applicationFees=decimal.MinValue;
        private int applicationTypeID = -1;
        private int _newLocalLicenseID=-1;
        public frmReplacmentLicense()
        {
            InitializeComponent();
            ctrlFindDriver1.eventIfLicenseFound += behaviorDependOnFOundationOfLicense;
        }

        private void behaviorDependOnFOundationOfLicense(bool found)
        {
            resetApplicationINFO();
            if (found)
            {
                _localLicenseID=ctrlFindDriver1._localLicenseID;
                linklabelShowLicensesHistory.Enabled = true;
                behaviortoCheckIfLicenseValidToReplace();
            }
            else {
                _localLicenseID=-1;
                linklabelShowLicensesHistory.Enabled = false;
            }

        }

        private void behaviortoCheckIfLicenseValidToReplace()
        {
            if (clsLicense.isThatLicenseActive(_localLicenseID))
            {
                btnReplace.Enabled = true;
                lbOldLocalLicenseID.Text=_localLicenseID.ToString();
            }
            else {

                MessageBox.Show("this License Is Not Active Choose Active License To Replace IT","Replace",MessageBoxButtons.OK,MessageBoxIcon.Error);
                btnReplace.Enabled=false;
            }
        }


        private void showApplicationInfo()
        {
            lbAppDate.Text = DateTime.Now.ToString();
            lbCreatedBY.Text = CurrentUser.username;


        }

        private void frmReplacmentLicense_Load(object sender, EventArgs e)
        {
            RbDamaged.Checked = true;
            showApplicationInfo();
        }

        private void RbDamaged_CheckedChanged(object sender, EventArgs e)
        {
            if (RbDamaged.Checked)
            {
                lbFrmTitle.Text = "Replacement For Damaged License";
                applicationFees = clsApplication.getApplicationFees(4);
                applicationTypeID = 4;

            }
            else
            {
                lbFrmTitle.Text = "Replacement For Lost License";
                applicationFees = clsApplication.getApplicationFees(3);
                applicationTypeID = 3;

            }
            lbApplicationFees.Text = applicationFees.ToString();
        }

        private int MakeApplicationForReplacement()
        {
            clsApplication Application = new clsApplication();
            Application._ApplicationStartDate = DateTime.Now;
            Application._ApplicationStatus = "New";
            Application.lastStatusDate = Application._ApplicationStartDate;
            Application.paidFees=applicationFees;
            Application.createdByUserID = CurrentUser.userID;
            //Application._LicenseClassID = clsLicense.getLicenseClassID(_localLicenseID);
            Application._TypeID=applicationTypeID;
            Application._ApplicationPersonID =clsLicense.getPersonID(_localLicenseID);

            if (Application.Save())
                return Application._applicationID;
            return -1;

        }

        private void IssueReplacmentLicense()
        {
            if (MessageBox.Show("Replace this license?", "Replacement", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            int applicationID, newLicenseID;
            if (clsLicense.RenewOrReplace(_localLicenseID, applicationTypeID, CurrentUser.userID, string.Empty, out applicationID, out newLicenseID))
            {
                _newLocalLicenseID = newLicenseID;
                lbRenewApplicationID.Text = applicationID.ToString();
                lbReplacedLicenseID.Text = newLicenseID.ToString();
                btnReplace.Enabled = false;
                ctrlFindDriver1.gbFilter.Enabled = false;
                linkLabelShowNewLicense.Enabled = true;
                MessageBox.Show("License replaced successfully.", "Replacement", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("This license cannot be replaced. It must be active, unexpired, and released.", "Replacement", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void resetApplicationINFO()
        {
            linkLabelShowNewLicense.Enabled = false;
            lbRenewApplicationID.Text=string.Empty;
            lbOldLocalLicenseID.Text=string.Empty;
            lbReplacedLicenseID.Text=string.Empty ;
            lbAppDate.Text=DateTime.Now.ToString();

        }

        private void linklabelShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (_localLicenseID != -1)
            {
                int personID=clsLicense.getPersonID(_localLicenseID);
                frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(personID);
                frm.ShowDialog();
            }
        }

        private void btnReplace_Click(object sender, EventArgs e)
        {
            IssueReplacmentLicense();
        }

        private void linkLabelShowNewLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDrivingLicenseInfo frm = new frmDrivingLicenseInfo(_newLocalLicenseID);
            frm.ShowDialog();
        }
    }
}
