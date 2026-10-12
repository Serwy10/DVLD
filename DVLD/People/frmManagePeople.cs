using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLogic2;

namespace DVLD
{
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();


        }

        private void LoadPeopleList()
        {
            dataGridView1.DataSource = ClsPerson.PeopleList();
            lbNumOfRecords.Text=dataGridView1.RowCount.ToString();

        }

        private void fillItemsInFiltersComboBox()
        {
            cbFilter.Items.Add("None");

            foreach (DataGridViewColumn column in dataGridView1.Columns)
            {
                cbFilter.Items.Add(((DataGridViewColumn)column).Name);
            }
            cbFilter.SelectedIndex = 0;
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            LoadPeopleList();
            fillItemsInFiltersComboBox();

        }

        private void CheckFilterBoxSelectedItem()
        {
            if (cbFilter.SelectedItem.ToString().ToLower() == "none")
                tbFilter.Visible = false;
            else
                tbFilter.Visible = true;



        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckFilterBoxSelectedItem();
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if (tbFilter.Text == string.Empty)
            {
                dataGridView1.DataSource = ClsPerson.PeopleList();
                lbNumOfRecords.Text = dataGridView1.RowCount.ToString();
                return;
            }
            try
            {
                if (cbFilter.SelectedItem.ToString().ToLower() == "personid")
                {
                    if (!int.TryParse(tbFilter.Text, out int id))
                    {
                        tbFilter.Text = string.Empty;
                        return;
                    }
                    dataGridView1.DataSource = ClsPerson.PeopleTableViewFilteredBy($"{cbFilter.SelectedItem.ToString()}='{tbFilter.Text.Replace("'", "''")}'");
                }
                else
                    dataGridView1.DataSource = ClsPerson.PeopleTableViewFilteredBy($"{cbFilter.SelectedItem.ToString()} like '%{TableFilter.EscapeLike(tbFilter.Text)}%' " +
                        $" or {cbFilter.SelectedItem.ToString()}='{tbFilter.Text.Replace("'", "''")}'");

            }
            catch { dataGridView1.DataSource = ClsPerson.PeopleList(); }

           lbNumOfRecords.Text=dataGridView1.RowCount.ToString();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddPerson frmAdd= new frmAddPerson();
            frmAdd.delLoadPeople += LoadPeopleList;
            frmAdd.ShowDialog();
        }

        private bool checkPersonSelected()
        {
            return (dataGridView1.SelectedRows.Count != 0);

        }

        private int getPersonID()
        {
            if (int.TryParse(dataGridView1.SelectedRows[0].Cells["personid"].Value.ToString(),out int id))
                return id;
            return -1;
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!checkPersonSelected())
            {
                MessageBox.Show("select Person First");
                return;
            }
            ShowPersonDetailscs personDetailForm = new ShowPersonDetailscs(getPersonID());
            //personDetailForm.show
            personDetailForm.ShowDialog();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddPerson frmAddPerson = new frmAddPerson();
            frmAddPerson.ShowDialog();
            LoadPeopleList();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!checkPersonSelected())
            {
                MessageBox.Show("select Person First");
                return;
            }
            frmAddPerson editPersonForm = new frmAddPerson(getPersonID());
            editPersonForm.ShowDialog();
            LoadPeopleList();
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!checkPersonSelected())
            {
                MessageBox.Show("select Person First");
                return;
            }
            ClsPerson person=ClsPerson.Find(getPersonID());
            if (person != null) {
                if (person.Delete())
                {
                    if (person.ImagePath != string.Empty)
                    {
                        File.Delete(person.ImagePath);
                    }
                    MessageBox.Show("person delete successfully");
                    LoadPeopleList();
                }
                else
                    MessageBox.Show("nothing deleted something went wrong");

            }


        }
    }
}
