using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;
using ClassInterfases;

namespace Chief
{
    public partial class GSAddress : Form, GSADR
    {
        private AMAS_DBI.Class_syb_acc AMASacc;

        public int GSAddressID()
        {
             return addressRegister1.get_address(); 
        }

        public string GSAddressString()
        {
             return addressRegister1.AddressString();
        }

        public GSAddress(AMAS_DBI.Class_syb_acc Acc)
        {
            InitializeComponent();
            AMASacc = Acc;
            addressRegister1.connect(AMASacc);
        }

        private void buttonYes_Click(object sender, EventArgs e)
        {
            this.DialogResult=DialogResult.Yes;
        }

        private void buttonNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
        }
    }
}