using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AMASControlRegisters;

namespace Chief
{
    public partial class FormMovingList : Form
    {
        private UCMoviesList Movis;
        public int[] movId;

        public FormMovingList(AMAS_DBI.Class_syb_acc ACC, int doc)
        {
            InitializeComponent();

            Movis = new UCMoviesList(doc,ACC);
            this.Controls.Add(Movis);
            Movis.Dock = DockStyle.Fill;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            movId = Movis.movlist();
            this.Close();
        }
    }
}
