using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chief
{
    public partial class License : Form
    {
        public License(string AMASVersion)
        {
            InitializeComponent();

            label1.Text = AMASVersion;
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}