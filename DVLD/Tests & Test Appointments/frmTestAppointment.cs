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
    public partial class frmTestAppointment : Form
    {
        private string _testType = "";
        private int _applicationID = -1;

        public delegate void refreshing();
        public event refreshing refreshApplications;
        public frmTestAppointment(string testType,int applicationID)
        {
            InitializeComponent();
            _testType = testType;
            _applicationID = applicationID;

        }

        private void determineFormType()
        {
            if (_testType.Contains("vision"))
            {
                lbTitle.Text = "Vision Test Appointment";
                pictureBoxTitle.ImageLocation = AppPaths.Asset("icons8-vision-80.png");
            }
            else if (_testType.Contains("writting"))
            {
                lbTitle.Text = "Writting Test Appointment";
                pictureBoxTitle.ImageLocation = AppPaths.Asset("icons8-write-100.png");
            }
            else if (_testType.Contains("street"))
            {
                lbTitle.Text = "Street Test Appointment";
                pictureBoxTitle.ImageLocation = AppPaths.Asset("icons8-fiat-500-100.png");
            }
        }

        private bool checkIfHasAppointmentandNotLocked()
        {
            return clsTestAppointment.checkIfApplicantHasAppointmentAndNotLocked(_applicationID, _testType);
        }

        private bool checkIfApplicantPassedthatTestType()
        {
            return clsTestAppointment.checkIfPassedThatTestType(_applicationID, _testType);
        }
        private void getAppointmentsTable()
        {
           dataGridView1.DataSource= clsTestAppointment.testAppointmentsTable(_testType, _applicationID);
            lbRecords.Text=dataGridView1.RowCount.ToString();

        }

        private void frmTestAppointment_Load(object sender, EventArgs e)
        {
            determineFormType();
            getAppointmentsTable();
            ctrlLicenseApplication1.localDrivingID = _applicationID;
            ctrlLicenseApplication1.showLocalDrivingInfo();
        }

        private void btnTestAppointment_Click(object sender, EventArgs e)
        {
            if (checkIfHasAppointmentandNotLocked())
            {
                MessageBox.Show("that Applicant has an Appointment you can't schedule another One","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                return;
            }

            if (checkIfApplicantPassedthatTestType())
            {
                MessageBox.Show("this Applicant already Passed That Test ","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest scheduleTest = new frmScheduleTest(_testType);
            scheduleTest.RefreshAppointments += getAppointmentsTable;
            scheduleTest.ShowDialog();


        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if(dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select Appointment First");
                return;
            }
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(),out int AppointmentID))
            {
               if (clsTestAppointment.checkIfAppointmentIsLocked(AppointmentID))
                {
                    frmScheduleTest scheduleTest= new frmScheduleTest(_testType,AppointmentID,true);
                    scheduleTest.RefreshAppointments += getAppointmentsTable;
                    scheduleTest.ShowDialog();
                }
                else
                {
                    frmScheduleTest frmSchedule= new frmScheduleTest(_testType,AppointmentID);
                    frmSchedule.RefreshAppointments += getAppointmentsTable;
                    frmSchedule.ShowDialog();
                }
            }
        }



        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select Appointment First");
                return;
            }
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int AppointmentID))
            {
                if (clsTestAppointment.checkIfAppointmentIsLocked(AppointmentID))
                {
                    frmTest test= new frmTest(_testType,AppointmentID,true);
                    test.ShowDialog();
                }
                else
                {
                    frmTest test=new frmTest(_testType,AppointmentID);
                    test.refreshing += getAppointmentsTable;
                    test.refreshing += ctrlLicenseApplication1.refreshPassedTests;

                    test.ShowDialog();
                }

            }

        }

        private void frmTestAppointment_FormClosing(object sender, FormClosingEventArgs e)
        {
            refreshApplications?.Invoke();
        }
    }
}
