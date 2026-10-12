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
    public partial class frmInternationalLicenseApplication : Form
    {
        private int _INTLicenseID { get; set; } = -1;
        public frmInternationalLicenseApplication()
        {
            InitializeComponent();
            ctrlFindDriver1.eventIfLicenseFound += behaviorIfPersonAlreadyHaveInternationaLicense;
        }

        private int ApplyApplicationForInternationalLicense()
        {
            clsApplication internationalApp=new clsApplication();
            internationalApp._ApplicationStatus = "New";
            internationalApp._ApplicationStartDate = DateTime.Now;
            internationalApp.lastStatusDate = DateTime.Now;
            internationalApp.paidFees = clsApplication.getApplicationFees(6);
            internationalApp._ApplicationPersonID = ClsPerson.getPersonID(DriverLicenseDTO.nationalNo);
            internationalApp.createdByUserID = CurrentUser.userID;
            internationalApp._LicenseClassID = clsApplication.getLicenseClassID("Class 3 - Ordinary driving license");
            internationalApp._TypeID = clsLicense.getApplicationTypeID("new international license");

            if(internationalApp.Save())
            {
                lbInternationalAPPID.Text=internationalApp._applicationID.ToString();
                return internationalApp._applicationID;
            }
            else
            {
                return -1;
            }

        }

        private void behaviorIfPersonAlreadyHaveInternationaLicense(bool found)
        {
            if (found)
            {
                _INTLicenseID = clsInternationalLicense.isthatLicenseALreayHasInternationalLicense(ctrlFindDriver1._localLicenseID);
                if (_INTLicenseID != -1)
                {

                    linkLabel2.Enabled = true;
                }
                else
                {
                    linkLabel2.Enabled = false;
                }
            }
            else
                linkLabel2.Enabled = false;

        }

        private void IssueNewInternationalLicense()
        {
            int localID = ctrlFindDriver1._localLicenseID;
            if (localID < 1) return;
            if (MessageBox.Show("Issue an international license?", "International license", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            int applicationID, internationalID;
            if (clsInternationalLicense.IssueForLocalLicense(localID, CurrentUser.userID, out applicationID, out internationalID))
            {
                _INTLicenseID = internationalID;
                lbLocalLicenseID.Text = localID.ToString();
                lbInternationalAPPID.Text = applicationID.ToString();
                lbILicenseID.Text = internationalID.ToString();
                linkLabel2.Enabled = true;
                button1.Enabled = false;
                ctrlFindDriver1.gbFilter.Enabled = false;
                MessageBox.Show("International license issued successfully.", "International license", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else MessageBox.Show("Use an active, unexpired, released Class 3 license. The driver must have no current international license.", "International license", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void internationalApplicationData()
        {
            lbCreatedBY.Text=CurrentUser.username;
            lbIssuedate.Text=DateTime.Now.ToString();
            lbAppDate.Text=DateTime.Now.ToString();
            lbExpirationDate.Text=DateTime.Now.AddYears(1).ToString();
            lbFees.Text = clsApplication.getFeesForApplication(6).ToString();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            IssueNewInternationalLicense();
        }

        private void frmInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            internationalApplicationData();
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (DriverLicenseDTO.nationalNo != string.Empty)
            {
                int personID = ClsPerson.getPersonID(DriverLicenseDTO.nationalNo);
                if (personID != -1)
                {
                    frmShowPersonLicenseHistory frmShowPersonLicenseHistory = new frmShowPersonLicenseHistory(personID);
                    frmShowPersonLicenseHistory.ShowDialog();
                }
            }
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(_INTLicenseID);
            frm.ShowDialog();
        }
    }
}
