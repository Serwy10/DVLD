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
    public partial class ctrlUserInfo : UserControl
    {

        public int personID { get; private set; }

        public ctrlUserInfo()
        {
            InitializeComponent();
            this.personID = CurrentUser.PersonID;
            loadPersonInfo();
        }

        public ctrlUserInfo(int personID)
        {
           InitializeComponent ();
            this.personID = personID;
            loadPersonInfo ();
        }
        private void loadPersonInfo()
        {
            ctrlShowDetailscs1._personID = personID;
            ctrlShowDetailscs1.ShowPersonInfo();

        }

        private void _loadUserInfo()
        {
            clsUser user = clsUser.FindbyPersonID(personID);

            if (user != null) {
                lbUserID.Text=user.userID.ToString();
                lbUserName.Text = user.username;
                lbIsActive.Text=user.IsActive.ToString();
            }
        }

        private void loadUserInfo()
        {
            lbUserName.Text = CurrentUser.username;
            lbUserID.Text=CurrentUser.userID.ToString();
            lbIsActive.Text = (CurrentUser.isActive == true ? "Yes" : "No");
        }

        private void ctrlUserInfo_Load(object sender, EventArgs e)
        {

            if (personID!=CurrentUser.PersonID)
            _loadUserInfo();
            else
                loadUserInfo();
        }
    }
}
