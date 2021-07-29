using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;

namespace Chief
{
    public partial class Address : Form
    {
        AMAS_DBI.Class_syb_acc Ass_base;

        public Address(AMAS_DBI.Class_syb_acc AACC)
        {
            InitializeComponent();

            Ass_base = AACC;
            addressReg.connect(AACC);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            button1.Text = addressReg.get_address().ToString();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            addressReg.set_address(Convert.ToInt32( numericUpDown1.Value));
        }

        FormKLADR FmKLADR; 
        private void btnKLADR_Click(object sender, EventArgs e)
        {
            if (FmKLADR == null) FmKLADR = new FormKLADR(Ass_base);
            FmKLADR.Show();
        }

    }
}