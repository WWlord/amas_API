using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClassInterfases;

namespace Chief
{
    public partial class GSOrganisation : Form, GSORG
    {
        private AMAS_DBI.Class_syb_acc AMASacc;

        public GSOrganisation(AMAS_DBI.Class_syb_acc Acc)
        {
            InitializeComponent();
            AMASacc = Acc;
            juridicRegister1.connect(AMASacc);
        }
        public string GSOrgName()
        {
            return juridicRegister1.Current_ORG;
        }

        public int GSOrgID()
        {
            return juridicRegister1.Current_Ident;
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Yes;
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}