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
    public partial class frmManageDriver : Form
    {
        private DataTable _driversTable=null;
        public frmManageDriver()
        {
            InitializeComponent();
            _driversTable= clsDrivers.driversTable();
        }

        private void loadDriversTable()
        {
            dataGridView1.DataSource = _driversTable;
            dataGridView1.Columns[3].Width = 200;
            lbRecords.Text=dataGridView1.RowCount.ToString();
        }

        private void frmManageDriver_Load(object sender, EventArgs e)
        {
            loadDriversTable();
            loadCbFilter();
            comboBox1.SelectedIndex = 0;

        }

        private void loadCbFilter()
        {
            comboBox1.Items.Add("None");
            for (int i = 0; i < _driversTable.Columns.Count - 2; i++)
            {
                comboBox1.Items.Add(_driversTable.Columns[i].ColumnName);
            }
        }

        private DataView DriversViewOnStringsFilter(string filter,string value)
        {
            DataView view = new DataView(_driversTable);
            view.RowFilter = filter + " like " + $"'%{TableFilter.EscapeLike(value)}%'";
            return view;
        }

        private DataView DriversViewOnNumericFilters(string filter,string value)
        {
            DataView view = new DataView(_driversTable);
            view.RowFilter= filter +" = "+value;
            return view;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == 0)
            {
                tbFilter.Visible = false;
            }
            else
            {
                tbFilter.Visible = true;
            }
            tbFilter.Text = string.Empty;
            loadDriversTable();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {

            if (tbFilter.Text == string.Empty) {
                loadDriversTable();
                return;
            }

            if(comboBox1.SelectedItem.ToString().ToLower() != "fullname" && comboBox1.SelectedItem.ToString().ToLower() != "nationalno")
            {
                if (!int.TryParse(tbFilter.Text, out _))
                {
                    tbFilter.Text = string.Empty;
                }
            }

            try
            {
                if (comboBox1.SelectedItem.ToString().ToLower() == "fullname" || comboBox1.SelectedItem.ToString().ToLower() =="nationalno" )
                {
                    dataGridView1.DataSource = DriversViewOnStringsFilter(comboBox1.SelectedItem.ToString(), tbFilter.Text);
                    lbRecords.Text = dataGridView1.RowCount.ToString();
                }
                else
                {
                    dataGridView1.DataSource = DriversViewOnNumericFilters(comboBox1.SelectedItem.ToString(), tbFilter.Text);
                    lbRecords.Text = dataGridView1.RowCount.ToString();
                }
            }
            catch {
                loadDriversTable() ;
            }
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
           if(comboBox1.SelectedItem.ToString().ToLower() != "fullname" && comboBox1.SelectedItem.ToString().ToLower() != "nationalno")
            {
                if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }
    }
}
