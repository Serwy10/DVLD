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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

namespace DVLD
{
    public partial class frmManageTestTypes : Form
    {
        public frmManageTestTypes()
        {
            InitializeComponent();
        }

        private void editFormLocation()
        {
            Form Parent = this.MdiParent ?? this;

            this.Location = new Point(
                (Parent.ClientSize.Width - this.Width) / 2,
                (Parent.ClientSize.Height - this.Height) / 2
                );
        }

        private void dataGridViewPerparation()
        {
            dataGridView1.DataSource = clsTestType.TestTypesTable();
            dataGridView1.Columns[2].Width = 450;
            dataGridView1.Columns[1].Width = 200;
            lbRecords.Text=dataGridView1.RowCount.ToString();
        }

        private void frmManageTestTypes_Load(object sender, EventArgs e)
        {
            editFormLocation();
            dataGridViewPerparation();
        }

        private void editTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select test first");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(),out int TestID))
            {
                frmEditTestType frmEditTest = new frmEditTestType(TestID);
                frmEditTest.UpdaTable += dataGridViewPerparation;
                frmEditTest.ShowDialog();
            }
        }
    }
}
