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
    public partial class frmshowApplicationDetails : Form
    {
        private int _localDrivingID {  get; set; }
        public frmshowApplicationDetails(int localDrivingID)
        {
            InitializeComponent();
            _localDrivingID = localDrivingID;
        }

        private void showApplicationInfo()
        {
            ctrlLicenseApplication1.localDrivingID= _localDrivingID;
            ctrlLicenseApplication1.showLocalDrivingInfo();
        }

        private void frmshowApplicationDetails_Load(object sender, EventArgs e)
        {
            showApplicationInfo();
        }
    }
}
