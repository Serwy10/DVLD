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
    public partial class frmReleaseDetainedLicenses : Form
    {
        private int _licenseID = -1;
        private decimal _applicationFees=decimal.MinValue;
        private decimal _fineFees=decimal.MinValue;

        private clsDetainedLicense detainedLicense = null;

        public delegate void DelLoad();
        public event DelLoad eventLoad;
        public frmReleaseDetainedLicenses(int licenseID=-1)
        {
            InitializeComponent();
            _licenseID = licenseID;

            if (_licenseID != -1)
            {
                lockSearchForLicense();
                ctrlFindDriver1._localLicenseID=_licenseID;
                ctrlFindDriver1.ctrlLicenseInfo1.localLicenseID = _licenseID;
                ctrlFindDriver1.ctrlLicenseInfo1.showLicenseInfo();
                behaviorDependOnFoundation(true);
                detainedLicense = clsDetainedLicense.findDetainedLicense(licenseID);
            }
            else
            {
                ctrlFindDriver1.eventIfLicenseFound += behaviorDependOnFoundation;
            }


        }

        private void lockSearchForLicense()
        {
            ctrlFindDriver1.gbFilter.Enabled = false;
        }

        private void behaviorDependOnFoundation(bool found)
        {
            reset();
            if (found)
            {
                linklabelShowLicensesHistory.Enabled = true;
                _licenseID = ctrlFindDriver1._localLicenseID;
                lbLicenseID.Text = _licenseID.ToString();

                if(clsDetainedLicense.isLicenseDetained(_licenseID))
                {
                    btnRelease.Enabled = true;
                    showDetainInfo();
                }
                else
                {
                    btnRelease.Enabled=false;
                    MessageBox.Show("this license Is not Detained","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                linklabelShowLicensesHistory.Enabled=false;
                _licenseID = -1;
            }
        }

        private void showDetainInfo()
        {
            detainedLicense = clsDetainedLicense.findDetainedLicense(_licenseID);
            if (detainedLicense == null) { btnRelease.Enabled = false; return; }

            lbDetainID.Text=detainedLicense.detainID.ToString();
            lbDetainDate.Text=detainedLicense.detainDate.ToString();
            lbCreatedBY.Text=detainedLicense.createdByUserID.ToString();
            _fineFees=detainedLicense.fineFees;
            lbFineFees.Text=_fineFees.ToString();
            _applicationFees = clsApplication.getApplicationFees(5);
            lbApplicationFees.Text=_applicationFees.ToString();
            lbTotalFees.Text=(_applicationFees+_fineFees).ToString();


        }

        private int applyForReleaseApplication()
        {
            clsApplication application=new clsApplication();

            application._ApplicationStatus = "New";
            application._ApplicationStartDate = DateTime.Now;
            application.lastStatusDate=application._ApplicationStartDate;
            application.paidFees = _applicationFees;
            application.createdByUserID = CurrentUser.userID;
            application._ApplicationPersonID=clsLicense.getPersonID(_licenseID);
            application._TypeID = 5;

            if (application.Save())
                return application._applicationID;
            return -1;
        }


        private void ReleaseDetainedLicense()
        {
            if (MessageBox.Show("Release this license?", "Release", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            int applicationID;
            if (clsDetainedLicense.ReleaseLicense(_licenseID, CurrentUser.userID, out applicationID))
            {
                btnRelease.Enabled = false;
                ctrlFindDriver1.gbFilter.Enabled = false;
                linkLabelShowNewLicense.Enabled = true;
                eventLoad?.Invoke();
                MessageBox.Show("License released successfully.", "Release", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("This license is no longer detained. Refresh and try again.", "Release", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            ReleaseDetainedLicense();
        }

        private void reset()
        {
            detainedLicense = null;
            btnRelease.Enabled = false;
            linkLabelShowNewLicense.Enabled = false;
            linklabelShowLicensesHistory.Enabled = false;
        }

        private void linklabelShowLicensesHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int personID=clsLicense.getPersonID(_licenseID);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(personID);
            frm.ShowDialog();
        }

        private void linkLabelShowNewLicense_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmDrivingLicenseInfo frm=new frmDrivingLicenseInfo(_licenseID);
            frm.ShowDialog();
        }
    }
}
