using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Chief
{
    public partial class EditAutor : Form
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private AMASControlRegisters.JuridicRegister juridicRegister1;
        private AMASControlRegisters.PeopleRegister peopleRegister1;

        private int DocID = 0;

        public EditAutor(AMAS_DBI.Class_syb_acc ACC, int id, string FK)
        {
            InitializeComponent();

            AMASacc = ACC;
            DocID = id;

            this.Text = "Ввести автора для документа " + FK;

            peopleRegister1 = new AMASControlRegisters.PeopleRegister();
            this.tabPage1.Controls.Add(this.peopleRegister1);
            // 
            // peopleRegister1
            // 
            this.peopleRegister1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.peopleRegister1.Location = new System.Drawing.Point(3, 3);
            this.peopleRegister1.Name = "peopleRegister1";
            this.peopleRegister1.Size = new System.Drawing.Size(611, 216);
            this.peopleRegister1.TabIndex = 0;

            juridicRegister1 = new AMASControlRegisters.JuridicRegister();
            this.tabPage2.Controls.Add(this.juridicRegister1);
            // 
            // juridicRegister1
            // 
            this.juridicRegister1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.juridicRegister1.Location = new System.Drawing.Point(3, 3);
            this.juridicRegister1.Name = "juridicRegister1";
            this.juridicRegister1.Size = new System.Drawing.Size(611, 216);
            this.juridicRegister1.TabIndex = 0;

            peopleRegister1.connect(ACC);
            juridicRegister1.connect(ACC);

            peopleRegister1.Maned += new AMASControlRegisters.PeopleRegister.ManSelected(peopleRegister1_Maned);
            juridicRegister1.Employed += new AMASControlRegisters.JuridicRegister.EmployeeSelected(juridicRegister1_Employed);
            juridicRegister1.Orged += new AMASControlRegisters.JuridicRegister.OrgSelected(juridicRegister1_Orged);

        }

        void juridicRegister1_Orged(string Org, int Ident)
        {
            AMAS_DBI.AMASCommand.EditAutor_Recieved_document(0, 0, juridicRegister1.Current_Ident, DocID);
            this.Close();
        }

        void juridicRegister1_Employed(string Org, int Ident)
        {
            AMAS_DBI.AMASCommand.EditAutor_Recieved_document(0, juridicRegister1.Current_Employee_ID, juridicRegister1.Current_Ident, DocID);
            this.Close();
        }

        void peopleRegister1_Maned(string Man, int Ident)
        {
            AMAS_DBI.AMASCommand.EditAutor_Recieved_document(peopleRegister1.Current_Man, 0, 0, DocID);
            this.Close();
        }
    }
}