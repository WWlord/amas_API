using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;
using AMASControlRegisters;

namespace Chief
{
    public partial class Population : Form
    {
        private AMAS_DBI.Class_syb_acc ACC;
        private int top_Panel = 0;
        private int agent = 0;

        public Population(AMAS_DBI.Class_syb_acc AACC)
        {
            InitializeComponent();

            ACC = AACC;
            contragentAttr.Conect(ACC);
            peopleReg.connect(ACC);
            peopleReg.Maned+=new AMASControlRegisters.PeopleRegister.ManSelected(peopleReg_Maned);
            top_Panel = contragentAttr.Top;
            this.Resize += new EventHandler(Population_Resize);
        }

        private void Population_Resize(Object sender, EventArgs e)
        {
            int height = contragentAttr.Height + contragentAttr.Top - top_Panel;
            if (height > 0)
                contragentAttr.Height = height;
            else
                contragentAttr.Height = 0;
        }

        private void peopleReg_Maned(string Man, int agentId)
        {
            agent = agentId;
            lblContragent.Text = Man;
            contragentAttr.EstablePost(agent);
        }
    }
}