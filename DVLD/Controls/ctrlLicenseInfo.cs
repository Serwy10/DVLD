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
    public partial class ctrlLicenseInfo : UserControl
    {
        public int localDrivingID = -1;
        public int localLicenseID = -1;
        public ctrlLicenseInfo()
        {
            InitializeComponent();
        }


        public void showLicenseInfo()
        {
            if (localDrivingID == -1 && localLicenseID == -1)
                return;
            if (localDrivingID != -1)
            {
                int applicationID = clsApplication.getApplicationID(localDrivingID);
                localLicenseID=clsLicense.getLicenseID(applicationID);
            }

            showLicense(localLicenseID);
        }

        public void showLicense(int localLicenseID)
        {


             if (DriverLicenseDTO.findDriverLicenseInfo(localLicenseID))
             {
                 lbActive.Text = DriverLicenseDTO.isActive.ToString();
                 lbClass.Text = DriverLicenseDTO.className;
                 lbDateOfbirth.Text = DriverLicenseDTO.dateOfBirth.ToString();
                 lbDriverID.Text = DriverLicenseDTO.driverID.ToString();
                 lbExpirationDate.Text = DriverLicenseDTO.expirationDate.ToString();
                 lbGendor.Text = DriverLicenseDTO.Gendor.ToString();
                 lbIsDetained.Text = DriverLicenseDTO.isDetained.ToString();
                 lbIssuedate.Text = DriverLicenseDTO.issueDate.ToString();
                 lbIssueReason.Text = DriverLicenseDTO.issueReason.ToString();
                 lbLicenseID.Text = DriverLicenseDTO.licenseID.ToString();
                 lbName.Text = DriverLicenseDTO.FullName;
                 lbNationalNo.Text = DriverLicenseDTO.nationalNo;
                 lbNotes.Text = DriverLicenseDTO.Notes;

                 if (DriverLicenseDTO.imageLocation == string.Empty)
                     personalImage.ImageLocation = null;
                 else
                     personalImage.ImageLocation = DriverLicenseDTO.imageLocation;

             }

        }

        public void resetControl()
        {
            lbActive.Text = string.Empty;
            lbClass.Text = string.Empty ;
            lbDateOfbirth.Text = string.Empty ;
            lbDriverID.Text = string .Empty ;
            lbExpirationDate.Text= string .Empty ;
            lbGendor.Text = string.Empty;
            lbIsDetained.Text= string .Empty ;
            lbIssuedate.Text =string.Empty ;
            lbIssueReason.Text = string .Empty ;
            lbLicenseID.Text = string.Empty;
            lbName.Text = string.Empty;
            lbNationalNo.Text=string.Empty ;
            lbNotes.Text = string.Empty;

            personalImage.ImageLocation = null;

        }
    }
}
