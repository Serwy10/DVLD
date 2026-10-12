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
    public partial class frmManageLocalDrivingApplications : Form
    {
        public frmManageLocalDrivingApplications()
        {
            InitializeComponent();
        }

        private void RefreshTableAndRecords()
        {

            dataGridView1.DataSource = clsApplication.localDrivingApplicationsTable();
            dataGridView1.Columns[1].Width = 200;
            lbRecords.Text = dataGridView1.RowCount.ToString();
        }

        private void frmManageLocalDrivingApplications_Load(object sender, EventArgs e)
        {
            Form Parent = this.MdiParent ?? this;
            this.Location = new Point((Parent.ClientSize.Width - this.Width) / 2
                , (Parent.ClientSize.Height - this.Height) / 2);

            RefreshTableAndRecords();
            cbFilter.SelectedIndex = 0;
        }

        private void TbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.SelectedItem.ToString() == "LDLAppID")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void TbFilter_TextChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedItem.ToString() == "LDLAppID")
            {
                if (!int.TryParse(TbFilter.Text, out int _))
                    TbFilter.Text = string.Empty;
            }

            makeFilteration();

        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilter.SelectedIndex != 0)
                TbFilter.Visible = true;
            else
                TbFilter.Visible = false;

            TbFilter.Text = string.Empty;
        }

        private void makeFilteration()
        {
            if (TbFilter.Text == string.Empty)
                RefreshTableAndRecords();

            else
            {

                if (cbFilter.SelectedItem.ToString() == "LDLAppID")
                {
                    if (TbFilter.Text.Length > 0)
                        dataGridView1.DataSource = clsApplication.getappLicationsViewForNumeric(cbFilter.SelectedItem.ToString(), TbFilter.Text);
                }

                else
                {
                    if (TbFilter.Text.Length > 0)
                        dataGridView1.DataSource = clsApplication.getApplicationsViewForText(cbFilter.SelectedItem.ToString(), TbFilter.Text);
                }
            }
            lbRecords.Text = dataGridView1.RowCount.ToString();

        }

        private void completeApplicationBehavior()
        {
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int value))
            {
                if (clsApplication.isApplicationCompleted(value))
                {
                    contextMenuStrip1.Items["editApplication"].Enabled = false;
                    contextMenuStrip1.Items["cancelApplication"].Enabled = false;
                    contextMenuStrip1.Items["ScheduleTest"].Enabled = false;

                }
                else
                {
                    contextMenuStrip1.Items["editApplication"].Enabled = true;
                    contextMenuStrip1.Items["cancelApplication"].Enabled = true;
                    contextMenuStrip1.Items["ScheduleTest"].Enabled = true;
                }
            }
        }

        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select Application You want To cancel First", "Cancelling", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            if (dataGridView1.SelectedRows[0].Cells["AppStatus"].Value.ToString().ToLower() == "cancelled")
                return;

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int value))
            {
                //if(clsApplication.isApplicationCompleted(value))
                //{

                //}
                if (MessageBox.Show("Are you sure You want To cancel this Application ? ", "cancelling", MessageBoxButtons.YesNo, MessageBoxIcon.Hand
                    ) == DialogResult.Yes)
                {
                    if (clsApplication.cancelApplication(value))
                    {
                        MessageBox.Show("Application cancelled Successfully", "Cancelling", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        RefreshTableAndRecords();
                        cbFilter.SelectedIndex = 0;
                    }
                    else
                    {
                        MessageBox.Show("Something Error and Application not Cancelled Try Again", "Cancelling", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btnAddNewApplication_Click(object sender, EventArgs e)
        {
            frmNewLocalDLApplication frm = new frmNewLocalDLApplication();
            frm.eventRefresh += RefreshTableAndRecords;
            frm.ShowDialog();
        }

        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select application first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                frmNewLocalDLApplication frm = new frmNewLocalDLApplication(id);
                frm.eventRefresh += RefreshTableAndRecords;
                frm.ShowDialog();
            }
        }

        private void ScheduleTestBehavior()
        {
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                if (clsApplication.isApplicationNew(id) && !clsApplication.isApplicantPassedAllTests(id))
                    ScheduleTest.Enabled = true;
                else
                {
                    ScheduleTest.Enabled = false;
                    return;
                }
            }

            if (!clsApplication.isSucessededInThatTest(1, id))
            {

                VisionTestTool.Enabled = true;
                WrittenTestTool.Enabled = false;
                StreetTestTool.Enabled = false;
            }




            else if (!clsApplication.isSucessededInThatTest(2, id))
            {
                WrittenTestTool.Enabled = true;
                VisionTestTool.Enabled = false;
                StreetTestTool.Enabled = false;
            }
            else if (!clsApplication.isSucessededInThatTest(3, id))
            {
                StreetTestTool.Enabled = true;
                VisionTestTool.Enabled = false;
                WrittenTestTool.Enabled = false;
            }
            else
            {
                StreetTestTool.Enabled = false;
                VisionTestTool.Enabled = false;
                WrittenTestTool.Enabled = false;
            }

        }

        private void IssueLicenseFirstTimeApperance()
        {
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                if (clsApplication.isApplicantPassedAllTests(id) && !clsApplication.isApplicationHasLicense(id))
                    Issue.Enabled = true;
                else
                    Issue.Enabled = false;
            }
        }

        private void SHowLicenseInfoApperance()
        {
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                if (clsApplication.isApplicationHasLicense(id))
                    showLicenseToolStripMenuItem.Enabled = true;
                else
                    showLicenseToolStripMenuItem.Enabled = false;
            }
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            completeApplicationBehavior();
            IssueLicenseFirstTimeApperance();
            ScheduleTestBehavior();
            SHowLicenseInfoApperance();

        }

        private void VisionTestTool_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                frmTestAppointment frmTestAppointment = new frmTestAppointment("vision", id);
                frmTestAppointment.refreshApplications += RefreshTableAndRecords;
                frmTestAppointment.ShowDialog();
            }
        }

        private void WrittenTestTool_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                frmTestAppointment frmTestAppointment = new frmTestAppointment("writting", id);
                frmTestAppointment.refreshApplications += RefreshTableAndRecords;
                frmTestAppointment.ShowDialog();
            }
        }

        private void StreetTestTool_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                frmTestAppointment frmTestAppointment = new frmTestAppointment("street", id);
                frmTestAppointment.refreshApplications += RefreshTableAndRecords;
                frmTestAppointment.ShowDialog();
            }
        }

        private void Issue_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                frmIssueLicenseFirstTime frmIssue = new frmIssueLicenseFirstTime(id);
                frmIssue.refreshApplications += RefreshTableAndRecords;
                frmIssue.ShowDialog();
            }
        }

        private void showApplicationInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                frmshowApplicationDetails frm = new frmshowApplicationDetails(id);
                frm.ShowDialog();
            }
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                int applicationID=clsApplication.getApplicationID(id);
                int licenseID=clsLicense.getLicenseID(applicationID);
                frmDrivingLicenseInfo frm= new frmDrivingLicenseInfo(licenseID);
                frm.ShowDialog();
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select First");
                return;
            }

            string nationalNO = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();

            int personID=ClsPerson.getPersonID(nationalNO);

            frmShowPersonLicenseHistory history= new frmShowPersonLicenseHistory(personID);
            history.ShowDialog();
        }

        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int id))
            {
                if (clsApplication.isApplicationHasLicense(id))
                {
                    MessageBox.Show("You can't delete this Application because it has license associated with it", "Error", MessageBoxButtons.OK
                        , MessageBoxIcon.Hand);
                    return;
                }

                if (clsTestAppointment.countPassedTests(id)>=1)
                {
                    MessageBox.Show("it is not allowed to delete application after passed at least one test if you want you can cancel it", "Error", MessageBoxButtons.OK
                       , MessageBoxIcon.Hand);
                    return;
                }
                    if (MessageBox.Show("Are you Sure You Want To delete that Application ? ","Delete",MessageBoxButtons.YesNo,MessageBoxIcon.Hand
                    ) == DialogResult.Yes)
                {
                    if (clsApplication.DeleteApplication(id))
                    {
                        MessageBox.Show("Application Deleted SuccessFully", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        RefreshTableAndRecords();
                    }
                    else
                        MessageBox.Show("Something went Wrong and Nothing Deleted , try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

        }
    }
}
