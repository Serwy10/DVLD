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
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
            this.StartPosition = FormStartPosition.Manual;
        }
        private void UpdateUsersTable()
        {
            dataGridView1.DataSource = clsUser.UsersTable();
            lbRecords.Text = dataGridView1.RowCount.ToString();
        }
        private void fillUsersTable(DataView usersView)
        {
            dataGridView1.DataSource = usersView;
        }

        private void RefreshNumOfRecords()
        {
            lbRecords.Text = dataGridView1.RowCount.ToString();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            fillUsersTable(clsUser.UsersTable().DefaultView);
            RefreshNumOfRecords();
            fillFilteringComboBox();
            comboBox1.SelectedIndex = 0;
        }

        private void fillFilteringComboBox()
        {
            comboBox1.Items.Add("None");
            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                comboBox1.Items.Add(column.Name);
            }
        }

        private void frmManageUsers_Shown(object sender, EventArgs e)
        {
            Form parent = this.MdiParent ?? this;

            this.Location = new Point(
                (parent.ClientSize.Width - this.Width) / 2,
                (parent.ClientSize.Height - this.Height) / 2
                );
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString() != "None" && comboBox1.SelectedItem.ToString().ToLower() != "isactive")
            {
                textBox1.Visible = true;
                cbActivition.Visible = false;
            }

            else if (comboBox1.SelectedItem.ToString().ToLower() == "isactive")
            {
                cbActivition.Visible = true;
                textBox1.Visible = false;
            }

            else
            {
                dataGridView1.DataSource = clsUser.UsersTable().DefaultView;
                RefreshNumOfRecords();
                textBox1.Visible = false;
                cbActivition.Visible = false;
            }

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().ToLower() == "userid" || comboBox1.SelectedItem.ToString().ToLower() == "personid")
            {
                if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        private void checkIfFilteringByNumericCategories()
        {
            if (comboBox1.SelectedItem.ToString().ToLower() == "userid" || comboBox1.SelectedItem.ToString().ToLower() == "personid")
            {
                if (!int.TryParse(textBox1.Text, out _))
                    textBox1.Text = string.Empty;
            }
        }

        private void filterOnNumericCategories()
        {
            dataGridView1.DataSource = clsUser.viewFilterdBy(comboBox1.SelectedItem.ToString(), textBox1.Text);
            RefreshNumOfRecords();
        }

        private void filterOnStringCategories()
        {
            dataGridView1.DataSource = clsUser.viewFilteredByStrings(comboBox1.SelectedItem.ToString(), textBox1.Text);
            RefreshNumOfRecords();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            checkIfFilteringByNumericCategories();

            if (textBox1.Text == string.Empty)
            {
                dataGridView1.DataSource = clsUser.UsersTable().DefaultView;
                RefreshNumOfRecords();
            }

            else if (comboBox1.SelectedItem.ToString().ToLower() == "userid" || comboBox1.SelectedItem.ToString().ToLower() == "personid")
                filterOnNumericCategories();
            else
                filterOnStringCategories();



        }

        private void cbActivition_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbActivition.SelectedItem.ToString().ToLower())
            {
                case "all":
                    dataGridView1.DataSource = clsUser.UsersTable().DefaultView;
                    RefreshNumOfRecords();
                    break;
                case "yes":
                    dataGridView1.DataSource = clsUser.viewFilterdBy(comboBox1.SelectedItem.ToString(), "true");
                    RefreshNumOfRecords();
                    break;
                case "no":
                    dataGridView1.DataSource = clsUser.viewFilterdBy(comboBox1.SelectedItem.ToString(), "false");
                    RefreshNumOfRecords();
                    break;
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {


        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmAddUpdateUsers frmAddUpdate = new frmAddUpdateUsers();
            frmAddUpdate.delUpdate += UpdateUsersTable;
            frmAddUpdate.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select user first");
                return;
            }
            int.TryParse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString(), out int personID);
            ShowUserInfo showUserInfo = new ShowUserInfo(personID);
            showUserInfo.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select user first");
                return;
            }
            int.TryParse(dataGridView1.SelectedRows[0].Cells[1].Value.ToString(), out int PersonID);
            frmChangePassword changePassword = new frmChangePassword(PersonID);
            changePassword.ShowDialog();
        }

        private void addNewUserToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUsers newUser = new frmAddUpdateUsers();
            newUser.delUpdate += UpdateUsersTable;
            newUser.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select USer First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int UserID))
            {
                frmAddUpdateUsers editUser = new frmAddUpdateUsers(UserID);
                editUser.delUpdate += UpdateUsersTable;
                editUser.ShowDialog();

            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0) return;
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Select USer First");
                return;
            }

            if (int.TryParse(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), out int UserID))
            {
                if (MessageBox.Show("Are you sure you want to delete that user ? ", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    if (clsUser.Delete(UserID))
                    {
                        MessageBox.Show("User Deleted Successfully", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Hand);
                        UpdateUsersTable();
                    }
                    else
                    {
                        MessageBox.Show("Cannot delete that user because it associated with other Relations", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }


                }

            }
        }
    }
}
