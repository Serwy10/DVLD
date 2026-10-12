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
    public partial class frmIssueLicenseFirstTime : Form
    {

        public delegate void refreshing();
        public event refreshing refreshApplications;
        public int _localDrivingID { get; set; }
        public frmIssueLicenseFirstTime(int localDrivingID)
        {
            InitializeComponent();
            _localDrivingID = localDrivingID;
        }

        private void showApplicationInfo()
        {
            ctrlLicenseApplication1.localDrivingID = _localDrivingID;
            ctrlLicenseApplication1.showLocalDrivingInfo();
        }

        private void frmIssueLicenseFirstTime_Load(object sender, EventArgs e)
        {
            showApplicationInfo();
        }

        private void IssueLicense()
        {
            clsLicense license = new clsLicense();
            license.applicationID=clsApplication.getApplicationID(_localDrivingID);
            clsApplication application = clsApplication.FindApplication(license.applicationID);
            if (application == null) return;
            license.licenseClass=application._LicenseClassID;
            license.isActive = true;
            license.Notes=tbNotes.Text;
            license.issueDate=DateTime.Now;
            license.expirationDate = license.issueDate.AddYears(clsLicense.getValidityLegnth(clsApplication.getClassName(license.licenseClass)));
            license.createdBY=CurrentUser.userID;
            license.paidFees=clsLicense.getLicenseClassFees(clsApplication.getClassName(license.licenseClass));
            license.issueReason = clsLicense.getApplicationTypeID("New Local Driving License Service");

            if (license.SaveNewLicense(clsLicense.enMode.NewLicense))
            {
                refreshApplications?.Invoke();
                if(MessageBox.Show($"New License Issued with LicenseID = {license.licenseID}","Issued",MessageBoxButtons.OK,MessageBoxIcon.Information)==DialogResult.OK)
                {
                    Close();

                }
                Close();
            }

            else
                MessageBox.Show("SOmething went Wrong Try Again","error",MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            IssueLicense();
        }
    }
}
