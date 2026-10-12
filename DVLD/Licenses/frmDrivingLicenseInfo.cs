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
    public partial class frmDrivingLicenseInfo : Form
    {

        public int _localLicenseID {  get; set; }
        public frmDrivingLicenseInfo(int licenseID)
        {
            InitializeComponent();
            _localLicenseID = licenseID;
        }

        private void frmDrivingLicenseInfo_Load(object sender, EventArgs e)
        {
            showLicenseInfo();
        }

        private void showLicenseInfo()
        {

                ctrlLicenseInfo1.localLicenseID= _localLicenseID;
                ctrlLicenseInfo1.showLicenseInfo();
        }
    }
}
