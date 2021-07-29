using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chief
{
    public partial class BusinessTasks : Form
    {
        private AMAS_DBI.Class_syb_acc SybAcc;

        public BusinessTasks(AMAS_DBI.Class_syb_acc Acc)
        {
            InitializeComponent();

            SybAcc = Acc;
            
        }
    }
}
