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
    public partial class frmScheduleTest : Form
    {
        string _testType = string.Empty;
        private bool retakedApp = false;
        public enum _enMode {addNew=1,Edit=2};
        public _enMode _mode;

        private int _appointmentID = -1;

        private decimal testTypeFees=decimal.MinValue;
        clsTestAppointment testAppointment = null;
        private bool _isLocked=false;

        public delegate void delegateRefreshing();
        public event delegateRefreshing RefreshAppointments;
        public frmScheduleTest(string testType, int appointmentID=-1,bool isLocked=false)
        {
            InitializeComponent();
            _testType = testType;
            _appointmentID = appointmentID;
            _isLocked = isLocked;
            if(_isLocked)
            {
                lockedAppointmentBehavior();
            }

            if (_appointmentID != -1)
            {
                _mode = _enMode.Edit;
                testAppointment = clsTestAppointment.Find(_appointmentID);
            }
            else
                _mode = _enMode.addNew;


        }

        private void lockedAppointmentBehavior()
        {
            btnSave.Enabled = false;
            dateTimePicker1.Enabled = false;
            lbLocked.Text = "The applicant already sat for that Test you cannot Edit it";
        }

        private void modifyDateTimePickerMinValue()
        {
            if (_mode == _enMode.addNew)
                dateTimePicker1.MinDate = DateTime.Now;
            else
            {
                if(testAppointment!=null)
                dateTimePicker1.Value = testAppointment.AppointmentDate;
            }

        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            testTypeFees = clsTestAppointment.getTestTypeFees(_testType);
            determineLogo();
            modifyDateTimePickerMinValue();
            gbRetakeTestStatus();
            showApplicantInfo();
        }

        private void determineLogo()
        {
            switch (_testType.ToLower())
            {
                case "vision":
                    gbScheduleTest.Text = "Vision Schedule Test";
                    break;
                case "writting":
                    gbScheduleTest.Text = "Writting Schedule Test";
                    pictureBoxTitle.ImageLocation = AppPaths.Asset("icons8-write-100.png");
                    break;
                case "street":
                    gbScheduleTest.Text = "Street Schedule Test";
                    pictureBoxTitle.ImageLocation= AppPaths.Asset("icons8-fiat-500-100.png");
                    break;
            }
        }

        private void gbRetakeTestStatus()
        {
            if (clsTestAppointment.checkIfApplicantFailedThatTestBefore(ApplicationDetailsDTO._localDrivingAppID, _testType))
            {
                lbTitle.Text = "Schedule Retake Test";

                gbRetakeTest.Enabled = true;
                retakedApp = true;
                lbRetakeFees.Text = "5";
                lbTotalFees.Text=(testTypeFees+5).ToString();

                if (_mode == _enMode.Edit)
                    lbAppointmentID.Text = _appointmentID.ToString();
            }
            else
                gbRetakeTest.Enabled = false;
        }

        private void showApplicantInfo()
        {
            lbDLAppID.Text=ApplicationDetailsDTO._localDrivingAppID.ToString();
            lbClassName.Text = ApplicationDetailsDTO._className;
            lbName.Text=ApplicationDetailsDTO._applicantName;
            lbTrial.Text=clsTestAppointment.countApplicantTestTrials(_testType,ApplicationDetailsDTO._localDrivingAppID).ToString();
            lbFees.Text = testTypeFees.ToString();

        }


        private void saveNewAppointment()
        {
             testAppointment= new clsTestAppointment();
            testAppointment.AppointmentDate = dateTimePicker1.Value;
            testAppointment.localDrivingID=ApplicationDetailsDTO._localDrivingAppID;
            testAppointment.createdBy=CurrentUser.userID;
            testAppointment.isLocked=false;
            testAppointment.testTypeID = clsTestAppointment.getTestTypeID(_testType);
            if(retakedApp)
                testAppointment.paidFees=testTypeFees +5;
            else
                testAppointment.paidFees=testTypeFees;

            if (testAppointment.Save())
            {
                _mode = _enMode.Edit;
                RefreshAppointments?.Invoke();
                if (MessageBox.Show("Data Saved Successfully", "Saving", MessageBoxButtons.OK, MessageBoxIcon.Information)==DialogResult.OK)
                {
                    Close();
                }
                Close();
            }
            else
            {
                MessageBox.Show("an error occured and nothing saved , try again","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void updateAppointment()
        {
            if (testAppointment != null)
            {
                testAppointment.AppointmentDate = dateTimePicker1.Value;

                if (testAppointment.Save())
                {
                    RefreshAppointments?.Invoke();
                    if (MessageBox.Show("Data Saved Successfully", "Saving", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                    {
                        Close();
                    }
                    Close();
                }
                else
                {
                    MessageBox.Show("an error occured and nothing saved , try again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }


            else
                MessageBox.Show("No test Appointment Found To Update It");
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (_mode) {
                case _enMode.addNew:
            saveNewAppointment();
                    break;
                case _enMode.Edit:
            updateAppointment();
                    break;
        }
        }
    }
}
