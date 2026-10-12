using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlFindPerson : UserControl
    {
        public int _personID=-1;

        public delegate void delPersonChanged();
        public event delPersonChanged delPerson;
        public ctrlFindPerson()
        {
            InitializeComponent();
        }

        private void tbFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().ToLower() == "personid")
            {
                if(!char.IsDigit(e.KeyChar) && ! char.IsControl(e.KeyChar))
                    e.Handled = true;
            }
        }

        public void QuickRecall()
        {
            gbFilter.Enabled = false;
            if (_personID != -1) {
                ctrlShowDetailscs1._personID = _personID;
                ctrlShowDetailscs1.ShowPersonInfo();
                    }
        }

        private void tbFilter_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem.ToString().ToLower() == "personid")
            {

                if (!int.TryParse(tbFilter.Text, out _))
                    tbFilter.Text = string.Empty;

            }
        }

        private void whichFilterUsed()
        {
            switch (comboBox1.SelectedItem.ToString().ToLower()) {

                case "personid":
                    //ctrlShowDetailscs1._personID=int.Parse(tbFilter.Text);
                    if(int.TryParse(tbFilter.Text, out int value)){

                        ctrlShowDetailscs1._nationaNO = string.Empty;
                        ctrlShowDetailscs1._personID = value;
                    }
                    break;

                case "national no.":
                    ctrlShowDetailscs1._personID = -1;
                    ctrlShowDetailscs1._nationaNO=tbFilter.Text;
                    break;
            }

            ctrlShowDetailscs1.ShowPersonInfo();
            _personID = ctrlShowDetailscs1.personFound ? ctrlShowDetailscs1._personID : -1;
            delPerson?.Invoke();

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if (tbFilter.Text == string.Empty)
                return;
            whichFilterUsed();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddPerson frmAdd = new frmAddPerson();
            frmAdd._DataBack += ShowPersonIndoAfterAddNew;
            frmAdd.ShowDialog();

            _personID=ctrlShowDetailscs1._personID;

            if(_personID!=-1)
                delPerson?.Invoke();


        }

        private void ShowPersonIndoAfterAddNew(int personID){
            ctrlShowDetailscs1._personID=personID;
            ctrlShowDetailscs1.ShowPersonInfo();
        }

        private void ctrlFindPerson_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;
        }
    }
}
