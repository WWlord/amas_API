using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chief
{
    public partial class EditOutcoming : Form
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private int DocID = 0;

        public EditOutcoming(AMAS_DBI.Class_syb_acc ACC, int id, string FK)
        {
            InitializeComponent();
            
            AMASacc = ACC;
            DocID = id;

            this.Text = "¬вести исход€щий номер дл€ документа " + FK;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AMAS_DBI.AMASCommand.EditOutcoming_Recieved_document(textBoxoutCod.Text, maskedTextBoxDateOut.Text, DocID);
            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }


    }
}