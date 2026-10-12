using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLDBusinessLogic2;

namespace DVLD
{
    public partial class FrmEditApplicationType : Form
    {
        public  delegate void UpdateApplicatonsTable();
        public event UpdateApplicatonsTable UpdateApplicationTypes;
        private int _applicationID = -1;
        private cclsApplicationTypecs applicationType = null;
        public FrmEditApplicationType(int ApplicationID)
        {
            InitializeComponent();
            this._applicationID = ApplicationID;
        }

        private void showApplicationInfo()
        {
            applicationType = cclsApplicationTypecs.Find(_applicationID);

            if (applicationType == null)
            {
                btnSave.Enabled = false;
                MessageBox.Show($"No Application ID found with ID = {_applicationID}");
                return;
            }
            lbID.Text=_applicationID.ToString();
            tbTitle.Text = applicationType._applicationName;
            tbFees.Text=applicationType._fees.ToString();
        }

        private void FrmEditApplicationType_Load(object sender, EventArgs e)
        {
            showApplicationInfo();
        }

        private void SaveUpdatedAPplicationInfo()
        {
            applicationType._applicationName = tbTitle.Text;
            decimal Fees =default;
            decimal.TryParse(tbFees.Text, out Fees);
            applicationType._fees = Fees;

            if(applicationType.Update())
                MessageBox.Show($"Application Type With ID = {_applicationID} Updated Successfully","Updated",MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show($"Application Type With ID = {_applicationID} Not Updated ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void tbFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
                e.Handled = true;

        }

        private void tbFees_TextChanged(object sender, EventArgs e)
        {
            if(!decimal.TryParse(tbFees.Text, out _))
                tbFees.Text = string.Empty;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            SaveUpdatedAPplicationInfo();
            UpdateApplicationTypes?.Invoke();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
