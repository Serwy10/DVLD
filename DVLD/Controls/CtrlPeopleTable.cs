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
    public partial class CtrlPeopleTable : UserControl
    {
        public CtrlPeopleTable()
        {
            InitializeComponent();
            dataGridView1.DataSource = ClsPerson.PeopleList();
        }


    }
}
