using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CommonValues;

namespace AMASControlRegisters
{
    public partial class ContragentRegister : UserControl
    {
        AMAS_DBI.Class_syb_acc AMASacc;
        private int The_Document=0;
        public ContragentRegister()
        {
            InitializeComponent();
            peopleRegister.Maned+=new PeopleRegister.ManSelected(peopleRegister_Maned);
            JuridicRegister.Orged+=new JuridicRegister.OrgSelected(JuridicRegister_Orged);
            JuridicRegister.Employed+=new JuridicRegister.EmployeeSelected(JuridicRegister_Employed);
            lvContragent.ContextMenuStrip = contextMenuStrip1;
        }

        private void JuridicRegister_Employed(string Employee, int agent)
        {
            Add_Contragent(agent, Employee, 3);
        }

        private void JuridicRegister_Orged(string Org, int agent)
        {
            Add_Contragent(agent, Org, 1);
        }

        private void peopleRegister_Maned(string Man, int agent)
        {
            Add_Contragent(agent, Man, 2);
        }

        public void connect(AMAS_DBI.Class_syb_acc SybAcc)
        {
            AMASacc = SybAcc;
            peopleRegister.connect(AMASacc);
            JuridicRegister.connect(AMASacc);
            Document=0;
        }

        public int Document
        {
            get { return The_Document; }
            set 
            {
                int doc = value;
                lvContragent.View = View.List;
                if (doc != The_Document)
                    if (The_Document == 0)
                        foreach (ListViewItem Item in lvContragent.Items)
                            AMAS_DBI.AMASCommand.Contragent_append(doc, Convert.ToInt32(Item.Name.Substring(1)));
                        
                The_Document = doc;
                Show_contragents();
            }
        }

        private void Show_contragents()
        {
            try
            {
                lvContragent.Clear();
                ArrayList agents = AMAS_DBI.AMASCommand.Doc_contragents(The_Document);
                if (agents != null)
                    foreach (CommonClass.Arrayagents sheet in agents)
                        lvContragent.Items.Add("a" + sheet.Id.ToString(), sheet.Name, sheet.Type - 1);
            }
            catch (Exception ex)
            {
                AMASacc.AddError("Contragents - 10", ex.Message, ex.StackTrace);
            }
        }

        public bool Add_Contragent(int contragent,string name,int type)
        {
            bool b = true;
            if (The_Document > 0)
                b=AMAS_DBI.AMASCommand.Contragent_append(The_Document, contragent);
            if (b) lvContragent.Items.Add("a" + contragent.ToString(),name,type-1);
            return b;
        }

        public bool Erise_Contragent(int contragent)
        {
            bool b = true;
            if (The_Document > 0)
                b = AMAS_DBI.AMASCommand.Contragent_remove(The_Document, contragent);
            if (b) lvContragent.Items["a"+contragent.ToString()].Remove();
            return b;
        }

        private void óäàëèòüÊîíòğàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem Item in lvContragent.SelectedItems)
                Erise_Contragent(Convert.ToInt32( Item.Name.Substring(1)));
            
        }
    }
}
