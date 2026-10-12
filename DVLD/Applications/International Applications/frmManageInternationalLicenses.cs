using System;
using System.Data;
using System.Windows.Forms;
using DVLDBusinessLogic2;

namespace DVLD
{
    public partial class frmManageInternationalLicenses : Form
    {
        private DataTable licensesTable = null;
        public frmManageInternationalLicenses()
        {
            InitializeComponent();
            licensesTable = clsInternationalLicense.InternationalLicensesTable();
        }

        private void loadLicensesTable()
        {

            dataGridView1.DataSource = licensesTable;
            lbRecords.Text=dataGridView1.RowCount.ToString();
        }

        private void loadFilterCategories()
        {
            comboBox1.Items.Add("None");
            for(int i = 0; i < licensesTable.Columns.Count; i++)
            {
                if( !licensesTable.Columns[i].ColumnName.Contains("Date"))
                comboBox1.Items.Add(licensesTable.Columns[i].ColumnName);
            }
            comboBox1.SelectedIndex = 0;
        }
        private void frmManageInternationalLicenses_Load(object sender, EventArgs e)
        {

            loadLicensesTable();
            loadFilterCategories();

        }


        private DataView DataViewFOrNumeric(string filter,string value)
        {

            DataView View = new DataView(licensesTable);
            View.RowFilter = filter + '=' + value;
            return View;

        }



        private DataView dataViewForBolean(string filter,string value)
        {
            DataView View = new DataView(licensesTable);
            View.RowFilter= filter + "=" + value;
            return View;
        }
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                tbFilter.Visible = false;
                CbActivation.Visible = false;
            }
            else if (comboBox1.SelectedItem.ToString().ToLower() == "isactive")
            {
                CbActivation.Visible = true;
                tbFilter.Visible = false;
                CbActivation.SelectedIndex = 0;
            }
            else
            {
                tbFilter.Visible = true;
                CbActivation.Visible = false;
            }

            loadLicensesTable();
            tbFilter.Text = string.Empty;

        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {

            if (tbFilter.Text == string.Empty)
                loadLicensesTable();


            else
            {
                dataGridView1.DataSource = DataViewFOrNumeric(comboBox1.SelectedItem.ToString(), tbFilter.Text);
                lbRecords.Text = dataGridView1.RowCount.ToString();
            }

            if(!int.TryParse(tbFilter.Text, out _))
                tbFilter.Text = string.Empty;


        }

        private void CbActivation_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (CbActivation.SelectedItem.ToString()) {
                case "All":
                    loadLicensesTable();
                    break;
                    case "Yes":
                   dataGridView1.DataSource= dataViewForBolean(comboBox1.SelectedItem.ToString(),"true");
                    lbRecords.Text= dataGridView1.RowCount.ToString();
                    break;
                    case "No":
                   dataGridView1.DataSource= dataViewForBolean(comboBox1.SelectedItem.ToString(), "false");
                    lbRecords.Text = dataGridView1.RowCount.ToString();
                    break;
            }
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseApplication application = new frmInternationalLicenseApplication();
            application.ShowDialog();
        }

        private void showPersonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("select person first");
                return;
            }
            int personID = -1;
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int internationalID)) {
               personID= clsInternationalLicense.getPersonID(internationalID);
                     }
            if (personID != -1)
            {
                ShowPersonDetailscs show = new ShowPersonDetailscs(personID);
                show.ShowDialog();
            }
        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("select person first");
                return;
            }
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int internationalID))
            {
                frmShowInternationalLicenseInfo frmShow = new frmShowInternationalLicenseInfo(internationalID);
                frmShow.ShowDialog();
            }

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("select person first");
                return;
            }
            int personID = -1;
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int internationalID))
                personID = clsInternationalLicense.getPersonID(internationalID);

            if(personID != -1)
            {
                frmShowPersonLicenseHistory frmShow= new frmShowPersonLicenseHistory(personID);
                frmShow.ShowDialog();
            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }
    }
}
