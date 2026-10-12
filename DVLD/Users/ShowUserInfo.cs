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
    public partial class ShowUserInfo : Form
    {
        private int _personID { get; set; } = -1;
        public ShowUserInfo(int personID=-1)
        {
            InitializeComponent();
            _personID=personID;
            if (_personID != -1)
            {

                ctrlUserInfo1.Controls.Clear();
                ctrlUserInfo ctrlUser = new ctrlUserInfo(_personID);

                ctrlUser.Dock=DockStyle.Fill;

                ctrlUserInfo1.Controls.Add(ctrlUser);
            }



        }


    }
}
