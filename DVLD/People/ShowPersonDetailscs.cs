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
    public partial class ShowPersonDetailscs : Form
    {
       static private int _personID = -1;
        public ShowPersonDetailscs(int personID)
        {
            InitializeComponent();
            _personID = personID;
            ctrlShowDetailscs1._personID = _personID;
            ctrlShowDetailscs1.ShowPersonInfo();
        }



    }
}
