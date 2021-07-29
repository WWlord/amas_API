using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;
using CommonValues;

namespace Chief
{
    public partial class Organizations : Form
    {
        private int top_Panel = 0;
        private AMAS_DBI.Class_syb_acc ACC;
        private int agent = 0;

        public Organizations(AMAS_DBI.Class_syb_acc AACC)
        {
            InitializeComponent();
            ACC = AACC;
            contragentAttr.Conect(ACC);
            this.JuridicRegister1.connect(AACC);
            top_Panel = contragentAttr.Top;
            this.Resize+=new EventHandler(Organizations_Resize);
            JuridicRegister1.Orged+=new AMASControlRegisters.JuridicRegister.OrgSelected(JuridicRegister1_Orged);
            JuridicRegister1.Employed+=new AMASControlRegisters.JuridicRegister.EmployeeSelected(JuridicRegister1_Employed);
        }

        private void Organizations_Resize(Object sender, EventArgs e)
        {
            int height = contragentAttr.Height + contragentAttr.Top - top_Panel;
            if (height > 0)
                contragentAttr.Height = height;
            else
                contragentAttr.Height = 0;
        }

        private void JuridicRegister1_Orged(string name, int agentId)
        {
            agent = agentId;
            lblContragent.Text = name;
            contragentAttr.EstablePost(agent);
        }

        private void JuridicRegister1_Employed(string name, int agentId)
        {
            agent = agentId;
            lblContragent.Text = name;
            contragentAttr.EstablePost(agent);
        }
    }
}