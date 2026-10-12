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
    public partial class frmNewLocalDLApplication : Form
    {
        private enum _enMode { Addnew=0,Update=1};
        private _enMode mode=_enMode.Addnew;

        private int personID {  get; set; }

        private clsApplication application=null;
        private int _applicationID = -1;

       public delegate void delRefreshing();
        public event delRefreshing eventRefresh;
        public frmNewLocalDLApplication(int LocalapplicationID=-1)
        {
            InitializeComponent();
            if (LocalapplicationID != -1)
                _applicationID =clsApplication.getApplicationID(LocalapplicationID);


            if(_applicationID != -1)
            {
                mode = _enMode.Update;
                lbTitle.Text = "Update Application";
                btnSave.Enabled = true;
                application=clsApplication.FindApplication(_applicationID);
            }

            ctrlFindPerson1.delPerson += refreshAddNewMode;
        }

        private void showPersonInfo()
        {
            ctrlFindPerson1._personID=clsApplication.getPersonID(_applicationID);
            ctrlFindPerson1.QuickRecall();
        }

        private void ExecutionIfUpdateMode()
        {
            showPersonInfo();

            if (application != null)
            {
                lbApplicationID.Text = application._applicationID.ToString();
                LbApplicationDate.Text=application._ApplicationStartDate.ToString();
                cbLicensesClasses.SelectedItem = clsApplication.getClassName(application._LicenseClassID);
                lbFees.Text=application.paidFees.ToString();
                lbCreatedBy.Text=clsApplication.getUserName(application.createdByUserID);
            }
        }

        private void frmNewLocalDLApplication_Load(object sender, EventArgs e)
        {
            if (this.MdiParent != null)
            {
                Form Parent = this.MdiParent ?? this;

                this.Location = new Point((Parent.ClientSize.Width - this.Width) / 2
                    , (Parent.ClientSize.Height - this.Height) / 2);
            }

            loadLicenseClasses();
            AddApplicationInfo();

            if (mode == _enMode.Update)
                ExecutionIfUpdateMode();
        }

        private void loadLicenseClasses()
        {
           foreach(DataRow row in clsApplication.licensesClasses().Rows)
            {
                cbLicensesClasses.Items.Add(row[0]);
            }

           cbLicensesClasses.SelectedIndex = 0;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (mode == _enMode.Addnew)
            {
                Control ShowPerson = ctrlFindPerson1.Controls["ctrlShowDetailscs1"];
                if (((CtrlShowDetailscs)ShowPerson).personFound)
                {
                    tabControl1.SelectedIndex = 1;
                    btnSave.Enabled = false;
                }
                else
                {
                    MessageBox.Show("Select Person First");
                    btnSave.Enabled = true;
                }
            }


        }

        private void refreshAddNewMode()
        {
            mode=_enMode.Addnew;
            this.lbTitle.Text = "New Local Driving License Application";
            lbApplicationID.Text = "???";

        }

        private void AddApplicationInfo()
        {
            lbFees.Text = clsApplication.getApplicationFees(1).ToString();
            LbApplicationDate.Text=DateTime.Now.ToShortDateString();
            lbCreatedBy.Text=CurrentUser.username;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (mode == _enMode.Addnew)
            {
                if (e.TabPageIndex == 1)
                {
                    Control ShowPerson = ctrlFindPerson1.Controls["ctrlShowDetailscs1"];
                    if (((CtrlShowDetailscs)ShowPerson).personFound)
                    {
                        tabControl1.SelectedIndex = 1;
                        btnSave.Enabled = true;
                    }
                    else
                    {
                        MessageBox.Show("Select Person First");
                        e.Cancel = true;
                        btnSave.Enabled = false;
                    }
                }
            }
        }

        private bool checkIfpersonHasApplicationFromThatClass()
        {
           Control showPerson= ctrlFindPerson1.Controls["ctrlShowDetailscs1"];

            personID = (((CtrlShowDetailscs)showPerson)._personID);

            return clsApplication.IsThisPersonHasApplicationFromThatCategory(personID,cbLicensesClasses.SelectedItem.ToString());

        }

        private void SaveNewApplication()
        {
            if(checkIfpersonHasApplicationFromThatClass())
            {
                MessageBox.Show("This Person has Already Application from that class","Error",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

             application = new clsApplication();
            application._ApplicationStatus = "New";
            application._ApplicationStartDate = DateTime.Now;
            application.lastStatusDate = DateTime.Now;
            application.createdByUserID = CurrentUser.userID;
            application.paidFees = clsApplication.getFeesForApplication(1);
            application._ApplicationPersonID = personID;
            application._LicenseClassID =clsApplication.getLicenseClassID(cbLicensesClasses.SelectedItem.ToString());
            application._TypeID = 1;

            if (application.Save())
            {
                lbApplicationID.Text=application._applicationID.ToString();
                mode = _enMode.Update;
                lbTitle.Text = "Update Application";

                MessageBox.Show("Application Saved SuccessFully","Saved",MessageBoxButtons.OK,MessageBoxIcon.Information);

                eventRefresh?.Invoke();
            }
            else
                MessageBox.Show("Something Error ANd Application not Saved TryAgain", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void UpdateApplication()
        {
            if(checkIfpersonHasApplicationFromThatClass())
            {
                MessageBox.Show("This Person has Already Application from that class", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            application.lastStatusDate= DateTime.Now;
            application._LicenseClassID= clsApplication.getLicenseClassID(cbLicensesClasses.SelectedItem.ToString());

            if (application.Save())
            {
                MessageBox.Show("Application Updated SuccessFully", "Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);

                eventRefresh?.Invoke();
            }
            else
                MessageBox.Show("Something Error ANd Application not Updated TryAgain", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            switch (mode) {
                case _enMode.Addnew:
            SaveNewApplication();
                    break;

                case _enMode.Update:
                    UpdateApplication();
                    break;

        }

        }


    }
}
