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
    public partial class applicationTypesfrm : Form
    {
        public applicationTypesfrm()
        {
            InitializeComponent();
        }

        private void applicationTypesfrm_Load(object sender, EventArgs e)
        {
            Form parent = this.MdiParent ?? this;

            this.Location = new Point(
                (parent.ClientSize.Width - this.Width) / 2,
                (parent.ClientSize.Height - this.Height) / 2
                );


            dataGridView1.DataSource = cclsApplicationTypecs.applicationTypesTable();
            modifyDataGridView();
            lbRecords.Text=dataGridView1.RowCount.ToString();
        }

        private void modifyDataGridView()
        {

            dataGridView1.Columns[1].Width = 300;
            dataGridView1.Columns[0].HeaderText = "ID";
            dataGridView1.Columns[1].HeaderText = "Title";
            dataGridView1.Columns[2].HeaderText = "Fees";
        }

        private void UpdateAPplicationTypesTable()
        {
            dataGridView1.DataSource=cclsApplicationTypecs.applicationTypesTable();
            lbRecords.Text = dataGridView1.RowCount.ToString();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select Application first");
                return;
            }
            int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int ApplicationID);

            FrmEditApplicationType frmEdit = new FrmEditApplicationType(ApplicationID);
            frmEdit.UpdateApplicationTypes += UpdateAPplicationTypesTable;
            frmEdit.ShowDialog();
        }
    }
}
