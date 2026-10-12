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
    public partial class frmShowInternationalLicenseInfo : Form
    {
        private int _INTLicenseID {  get; set; }=-1;
        public frmShowInternationalLicenseInfo(int INTLicenseID)
        {
            InitializeComponent();
            _INTLicenseID = INTLicenseID;
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            if (_INTLicenseID != -1)
            {
                ctrlInternationalDriverLicenseInfo1.INTLicenseID = _INTLicenseID;
                ctrlInternationalDriverLicenseInfo1.showInternationalLicenseInfo();
            }
        }
    }
}
