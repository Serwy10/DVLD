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
    public partial class frmShowPersonLicenseHistory : Form
    {
        private int _personID {  get; set; }
        public frmShowPersonLicenseHistory(int personID)
        {
            InitializeComponent();
            this._personID = personID;
        }

        private void showPersonInfo()
        {
            ctrlShowDetailscs1._personID=_personID;
            ctrlShowDetailscs1.ShowPersonInfo();
        }

        private void showLocalDrivingLicenses()
        {
            localdataGrid.DataSource = clsLicense.localDrivingLicensesTable(_personID);
            lbRecords.Text=localdataGrid.RowCount.ToString();
        }

        private void showInternationalLicenses()
        {
            internationalDataGrid.DataSource = clsLicense.InternationalLicensesTable(_personID);
            lbInternationalRecords.Text=internationalDataGrid.RowCount.ToString();
        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            showPersonInfo();
            showLocalDrivingLicenses();
            showInternationalLicenses();

        }
    }
}
