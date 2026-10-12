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
    public partial class ctrlLicenseApplication : UserControl
    {
        public int localDrivingID {  get; set; }=-1;
        public ctrlLicenseApplication()
        {
            InitializeComponent();
        }

        public void refreshPassedTests()
        {
            lbPassedTests.Text=clsTestAppointment.countPassedTests(localDrivingID).ToString()+@"\3";
        }

        public void showLocalDrivingInfo()
        {
            if (localDrivingID == -1)
                return;
            if (!ApplicationDetailsDTO.findApplicationInfo(localDrivingID))
                return;
            else
            {
                lbDLAppID.Text=ApplicationDetailsDTO._localDrivingAppID.ToString();
                lbClassName.Text = ApplicationDetailsDTO._className;
                lbPassedTests.Text = ApplicationDetailsDTO._passedTests.ToString()+@"\3";
                lbID.Text=ApplicationDetailsDTO._AppID.ToString();
                lbStatus.Text = ApplicationDetailsDTO._status;
                lbFees.Text = ApplicationDetailsDTO._paidFees.ToString();
                lbType.Text = ApplicationDetailsDTO._AppType;
                lbApplicant.Text = ApplicationDetailsDTO._applicantName;
                lbDate.Text = ApplicationDetailsDTO._AppDate.ToString();
                lbStatusDate.Text=ApplicationDetailsDTO._lastStatusDate.ToString();
                lbCreatedBy.Text = ApplicationDetailsDTO._createdBy;



            }



        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            ShowPersonDetailscs personDetailscs = new ShowPersonDetailscs(ApplicationDetailsDTO._personID);
            personDetailscs.ShowDialog();
        }
    }
}
