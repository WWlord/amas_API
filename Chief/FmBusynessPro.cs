using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;

namespace Chief
{
    public partial class FmBusynessPro : Form
    {
        private AMAS_DBI.Class_syb_acc SYB_acc;

        public FmBusynessPro()
        {
            InitializeComponent();


        }

        private void большиеЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvTasks.View = View.LargeIcon;
        }

        private void маленькиеЗначкиToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvTasks.View = View.SmallIcon;
        }

        private void списокToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvTasks.View = View.List;
        }

        private void таблицаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            lvTasks.View = View.Details;
        }
    }
}
