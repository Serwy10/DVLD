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
    public partial class ctrlInternationalDriverLicenseInfo : UserControl
    {
        public int INTLicenseID { get; set; } = -1;
        public ctrlInternationalDriverLicenseInfo()
        {
            InitializeComponent();
        }

        public void showInternationalLicenseInfo()
        {
            if (INTLicenseID == -1)
                return;
            if (internationalLicenseDTO.findINTDriverLicenseInfo(INTLicenseID))
            {
               lbApplicationID.Text=internationalLicenseDTO.ApplicationID.ToString();
                lbDateOfBirth.Text= internationalLicenseDTO.dateOfBirth.ToString();
                lbDriverID.Text= internationalLicenseDTO.DriverID.ToString();
                lbExpirationDate.Text= internationalLicenseDTO.ExpirationDate.ToString();
                lbGender.Text= internationalLicenseDTO.gender.ToString();
                lbIntLicenseID.Text= internationalLicenseDTO.IntLicenseID.ToString();
                lbIsActive.Text= internationalLicenseDTO.IsActive.ToString();
                lbIssueDate.Text=internationalLicenseDTO.IssueDate.ToString();
                lbLicenseID.Text= internationalLicenseDTO.LicensesID.ToString();
                lbName.Text= internationalLicenseDTO.Name.ToString();
                lbNationalNo.Text= internationalLicenseDTO.nationaNo.ToString();

                if (internationalLicenseDTO.imagePath == string.Empty)
                    personalImage.ImageLocation = null;
                else
                    personalImage.ImageLocation = internationalLicenseDTO.imagePath;


            }
        }
    }
}
