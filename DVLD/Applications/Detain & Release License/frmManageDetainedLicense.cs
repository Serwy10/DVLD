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
    public partial class frmManageDetainedLicense : Form
    {

        private DataTable DetainLicensesTable=null;

        private int licenseID = -1;
        public frmManageDetainedLicense()
        {
            InitializeComponent();
            DetainLicensesTable = clsDetainedLicense.DetainedLicensesTable();
        }

        private void loadDefaultDatainedLicensesTable()
        {
            dataGridView1.DataSource = DetainLicensesTable;
            modifyDataGridViewColumnNames();
            lbRecords.Text=dataGridView1.RowCount.ToString();
        }

        private void loadDetainedLicenseTable(DataView view)
        {
            dataGridView1.DataSource = view;
            modifyDataGridViewColumnNames();
            lbRecords.Text = dataGridView1.RowCount.ToString();

        }

        private void loadDetainedLicenseTable()
        {
            dataGridView1.DataSource = clsDetainedLicense.DetainedLicensesTable();
            DetainLicensesTable = (DataTable)dataGridView1.DataSource;
            modifyDataGridViewColumnNames();
            lbRecords.Text = dataGridView1.RowCount.ToString();
        }
        private void modifyDataGridViewColumnNames()
        {
            dataGridView1.Columns["DetainID"].HeaderText = "D.ID";
            dataGridView1.Columns["LicenseID"].HeaderText = "L.ID";
            dataGridView1.Columns["detaindate"].HeaderText = "D.Date";
            dataGridView1.Columns["isReleased"].HeaderText = "Is Realesed";
            dataGridView1.Columns["finefees"].HeaderText = "Fine Fees";
            dataGridView1.Columns["releasedate"].HeaderText = "Release Date";
            dataGridView1.Columns["NationalNo"].HeaderText = "N.NO";
            dataGridView1.Columns["fullname"].HeaderText = "Full Name";
            dataGridView1.Columns["ReleaseApplicationID"].HeaderText = "Release App.ID";

            dataGridView1.Columns["fullname"].Width = 200;

        }


        private DataView dataViewOnStringsFilter(string filter,string value)
        {
            DataView dv = new DataView(DetainLicensesTable);

            dv.RowFilter = filter + " like " + $"'%{TableFilter.EscapeLike(value)}%'";

            return dv;

        }

        private DataView dataViewOnNumericFilters(string filter,string value)
        {
            DataView dv = new DataView(DetainLicensesTable);
            dv.RowFilter=filter +" = "+value;
            return dv;
        }


        private void frmManageDetainedLicense_Load(object sender, EventArgs e)
        {
            loadDefaultDatainedLicensesTable();
            cbFilter.SelectedIndex = 0;
        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            if(cbFilter.SelectedIndex == 0)
            {
                tbFilter.Text= string.Empty;
                tbFilter.Visible= false;
                cbReleased.Visible= false;
                loadDefaultDatainedLicensesTable();
            }
            else if(cbFilter.SelectedItem.ToString().ToLower()=="is released")
            {
                tbFilter.Text= string.Empty;
                tbFilter.Visible = false;
                cbReleased.Visible= true;
                loadDefaultDatainedLicensesTable();
            }
            else
            {
                tbFilter.Text = string.Empty;
                tbFilter.Visible = true;
                cbReleased.Visible = false;
                loadDefaultDatainedLicensesTable();
            }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if(tbFilter.Text == string.Empty)
            {
                loadDefaultDatainedLicensesTable();
                return;
            }

            if(cbFilter.SelectedItem.ToString().ToLower()=="detain id" || cbFilter.SelectedItem.ToString().ToLower()=="release application id")
            {
                if (!int.TryParse(tbFilter.Text, out _))
                {
                    tbFilter.Text = string.Empty;
                    return;
                }
            }

            switch (cbFilter.SelectedItem.ToString().ToLower())
            {
                case "detain id":
                    loadDetainedLicenseTable(dataViewOnNumericFilters("detainid", tbFilter.Text));
                    break;

                case "national no":
                    loadDetainedLicenseTable(dataViewOnStringsFilter("nationalno", tbFilter.Text));
                    break;

                case "full name":
                    loadDetainedLicenseTable(dataViewOnStringsFilter("fullname",tbFilter.Text));
                    break;

                case "release application id":
                    loadDetainedLicenseTable(dataViewOnNumericFilters("releaseapplicationid", tbFilter.Text));
                    break;
            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilter.SelectedItem.ToString().ToLower() == "detain id" || cbFilter.SelectedItem.ToString().ToLower() == "release application id")
            {
                if(!char.IsDigit(e.KeyChar) && ! char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void cbReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbReleased.SelectedIndex)
            {
                case 0:
                    loadDefaultDatainedLicensesTable();
                    break;
                case 1:
                    loadDetainedLicenseTable(dataViewOnNumericFilters("isreleased", "true"));
                    break;
                    case 2:
                    loadDetainedLicenseTable(dataViewOnNumericFilters("isreleased", "false"));
                    break;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            FrmDetainLicense frmDetain=new FrmDetainLicense();
            frmDetain.eventload += loadDetainedLicenseTable;
            frmDetain.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenses frmRelease=new frmReleaseDetainedLicenses();
            frmRelease.eventLoad += loadDetainedLicenseTable;
            frmRelease.ShowDialog();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {
            if(dataGridView1.SelectedRows.Count==0)
            {
                MessageBox.Show("Select row first","error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(),out int detainID))
            {
                 licenseID=clsDetainedLicense.isThatLicenseStillDetained(detainID);

                if (licenseID == -1)
                    releaseLicenseTool.Enabled = false;
                else
                    releaseLicenseTool.Enabled = true;
            }
        }

        private void showPerosnDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            string nationalNo = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            int personID=ClsPerson.getPersonID(nationalNo);
            if (personID != -1)
            {
                ShowPersonDetailscs showPerson = new ShowPersonDetailscs(personID);
                showPerson.ShowDialog();
            }
        }

        private void showLicenseDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString(),out int licenseID))
            {
                frmDrivingLicenseInfo licenseInfo=new frmDrivingLicenseInfo(licenseID);
                licenseInfo.ShowDialog();
            }
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            string nationalNo = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
            int personID = ClsPerson.getPersonID(nationalNo);
            if (personID != -1)
            {
                frmShowPersonLicenseHistory frmShow=new frmShowPersonLicenseHistory(personID);
                frmShow.ShowDialog();
            }
        }

        private void releaseLicenseTool_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenses releaseDetainedLicenses=new frmReleaseDetainedLicenses(licenseID);
            releaseDetainedLicenses.eventLoad += loadDetainedLicenseTable;
            releaseDetainedLicenses.ShowDialog();
        }
    }
}
