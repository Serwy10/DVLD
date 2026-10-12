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
    public partial class frmTest : Form
    {
        private string _testType = "";
        private int _AppointmentID = -1;
        private clsTestAppointment testAppointment = null;
        private bool _isLocked=false;
        private clsTest Test=null;

        public delegate void delRefreshing();
        public event delRefreshing refreshing;


        public frmTest(string testType,int appointmentID,bool islocked=false)
        {
            InitializeComponent();
            _testType = testType;
            _AppointmentID = appointmentID;
            _isLocked = islocked;
            if (_AppointmentID != -1)
            {
                testAppointment = clsTestAppointment.Find(_AppointmentID);
                Test=clsTest.Find(_AppointmentID);
            }

            if (_isLocked)
                lockedTestBehavior();
        }

        private void ShowAppointmentInfo()
        {
            lbDLAppID.Text=testAppointment.localDrivingID.ToString();
            lbClassName.Text = ApplicationDetailsDTO._className;
            lbDate.Text=testAppointment.AppointmentDate.ToString();
            lbName.Text = ApplicationDetailsDTO._applicantName;
            lbTrial.Text=clsTestAppointment.countApplicantTestTrials(_testType,testAppointment.localDrivingID).ToString();
            lbFees.Text=testAppointment.paidFees.ToString();


        }

        public void lockedTestBehavior()
        {

            if (Test != null)
            {

                rbPass.Checked = Test.TestResult;
                rbFail.Checked = !Test.TestResult;
                tbNotes.Text = Test.Notes;
                lbTestID.Text=Test.testID.ToString();
            }
            btnSave.Enabled = false;
            rbPass.Enabled = false;
            rbFail.Enabled = false;
            tbNotes.Enabled = false;

        }

        public void formNameAndImage()
        {
            switch (_testType.ToLower())
            {
                case "vision":
                    groupBox1.Text = "Vision Test";
                    pictureBoxTitle.ImageLocation = AppPaths.Asset("icons8-vision-80.png");
                    break;

                case "writting":
                    groupBox1.Text = "Writting Text";
                    pictureBoxTitle.ImageLocation = AppPaths.Asset("icons8-write-100.png");
                    break;

                case "street":
                    groupBox1.Text = "Street Test";
                    pictureBoxTitle.ImageLocation= AppPaths.Asset("icons8-fiat-500-100.png");
                    break;
            }
        }

        private void frmTest_Load(object sender, EventArgs e)
        {
            formNameAndImage();
            ShowAppointmentInfo();
        }

        private bool takeTest()
        {
            clsTest test = new clsTest();
            test.testAppointmentID = _AppointmentID;
            test.TestResult = rbPass.Checked;
            test.Notes = tbNotes.Text;
            test.CreatedBy=CurrentUser.userID;

            return test.Save();

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           if( MessageBox.Show("Are you sure you want to save ? After that you can't change test Result. ","Saving"
               ,MessageBoxButtons.OKCancel,MessageBoxIcon.Question)==DialogResult.OK)
            {
                if (takeTest())
                {
                    refreshing?.Invoke();
                    if(MessageBox.Show("Data Saved Successfully", "Saving", MessageBoxButtons.OK, MessageBoxIcon.Information)==DialogResult.OK)
                        Close();
                    Close();
                }
                else
                {
                    MessageBox.Show("Something Wrong Happened and Nothing Saved , Try Again", "Saving", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
        }
    }
}
