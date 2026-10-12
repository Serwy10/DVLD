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
    public partial class frmEditTestType : Form
    {
        private int _testID;
        private clsTestType _Test=null;

        public delegate void UpdateTestTable();
        public event UpdateTestTable UpdaTable;
        public frmEditTestType(int testID)
        {
            InitializeComponent();
            _testID = testID;
        }

        private void showTestInfo()
        {
            _Test = clsTestType.Find(_testID);

            if( _Test == null)
            {
                btnSave.Enabled = false;
                MessageBox.Show($"No Test with that Test ID = {_testID}");
                return;
            }

            lbID.Text = _Test.TestID.ToString();
            tbTitle.Text = _Test.TestTitle;
            tbDescription.Text = _Test.TestDescription;
            tbFees.Text=_Test.TestFees.ToString();
        }

        private void tbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;
        }

        private void tbFees_TextChanged(object sender, EventArgs e)
        {
            if (!decimal.TryParse(tbFees.Text, out _))
                tbFees.Text = string.Empty;
        }

        private void frmEditTestType_Load(object sender, EventArgs e)
        {
            showTestInfo();
        }

        private void SaveTestUpdatedInfo()
        {
            _Test.TestTitle = tbTitle.Text;
            _Test.TestDescription = tbDescription.Text;
            _Test.TestFees=decimal.Parse(tbFees.Text);

            if(_Test.Update())
                MessageBox.Show($"Test Type With ID : {_testID} updated Successfully","Update",MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"Test Type With ID : {_testID} was not update some thing went wrong " +
                    $"try Again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveTestUpdatedInfo();
            UpdaTable?.Invoke();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
