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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManagePeople managePeople = new frmManagePeople();
            managePeople.MdiParent = this;

            managePeople.Show();
        }



        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ShowUserInfo userInfo = new ShowUserInfo();
            userInfo.MdiParent = this;
            userInfo.Show();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (var form = new frmChangePassword(DVLDBusinessLogic2.CurrentUser.PersonID))
                form.ShowDialog(this);
        }

        private void toolStripMenuItemSignOut_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Retry;
            Close();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageUsers users = new frmManageUsers();
            users.MdiParent = this;

            users.Show();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            applicationTypesfrm frm=new applicationTypesfrm();
            frm.MdiParent= this;
            frm.Show();
        }

        private void TSMManageTestTypes_Click(object sender, EventArgs e)
        {
            frmManageTestTypes frmManageTestTypes = new frmManageTestTypes();
            frmManageTestTypes.MdiParent = this;
            frmManageTestTypes.Show();
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            frmNewLocalDLApplication application = new frmNewLocalDLApplication();
            application.MdiParent = this;
            application.Show();
        }

        private void toolStripMenuItem5_Click(object sender, EventArgs e)
        {
            frmManageLocalDrivingApplications frm=new frmManageLocalDrivingApplications();
            frm.MdiParent=this;
            frm.Show();
        }

        private void toolStripMenuItem4_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseApplication application = new frmInternationalLicenseApplication();
            application.MdiParent = this;
            application.Show();
        }

        private void toolStripMenuItem6_Click(object sender, EventArgs e)
        {
            frmManageInternationalLicenses frm=new frmManageInternationalLicenses();
            frm.MdiParent=this;
            frm.Show();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDriver frmManageDriver = new frmManageDriver();
            frmManageDriver.MdiParent= this;
            frmManageDriver.Show();
        }

        private void renewDrivingLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewDrivingLicense frmRenew=new frmRenewDrivingLicense();
            frmRenew.MdiParent= this;
            frmRenew.Show();
        }

        private void replacementForDamagedOrLostLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplacmentLicense frm=new frmReplacmentLicense();
            frm.MdiParent= this;
            frm.Show();
        }



        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FrmDetainLicense frm = new FrmDetainLicense();
            frm.MdiParent = this;
            frm.Show();
        }

        private void releaseLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicenses frm=new frmReleaseDetainedLicenses();
            frm.MdiParent = this;
            frm.Show();
        }

        private void manageDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageDetainedLicense frm=new frmManageDetainedLicense();
            frm.MdiParent = this;
            frm.Show();
        }
    }
}
