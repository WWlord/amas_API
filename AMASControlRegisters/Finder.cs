using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DocumentsByPeriod;
using CommonValues;
using AMAS_DBI;
using AMAS_Query;

namespace AMASControlRegisters
{
    public partial class Finder : UserControl
    {
        private DocsOfPeriod SeekDocsList=null;
        CommonValues.FindProperty FndPr;

        public DocsOfPeriod FindDocsOfPeriod
        {
            set
            {
                SeekDocsList = value;
                switch (AMAS_Query.Class_AMAS_Query.DocIndex)
                {
                    case DocEnumeration.WellcomeDocs.Value:
                        ModeView = Mode.Wellcome;
                        break;
                    case DocEnumeration.IndoorDosc.Value:
                        ModeView = Mode.Indoor;
                        break;
                    case DocEnumeration.OutDocs.Value:
                        ModeView = Mode.Outdoc;
                        break;
                    case DocEnumeration.ArchiveDocs.Value:
                        ModeView = Mode.Archive;
                        break;
                    case DocEnumeration.OwnDocs.Value:
                    case DocEnumeration.DepartmentDocs.Value:
                    case DocEnumeration.NewsDocs.Value:
                    case DocEnumeration.VizingDocs.Value:
                        ModeView = Mode.Unknown;
                        break;
                }
            }
        }
        private ArrayList TemyList;
        private ArrayList KindList;
        private ArrayList EmployeesList;
        AMAS_DBI.Class_syb_acc SQLAcc;

        private class TKE
        {
            public string Ident
            { get { return id.ToString(); } }
            public string Naming
            { get { return name.Trim(); } }

            private string name;
            private int id;

            public TKE(string s, int i )
            {
                id = i;
                name = s;
            }
        }

        public Finder(Class_syb_acc Acc)
        {
            InitializeComponent();

            SQLAcc = Acc;

            FndPr = new FindProperty();

            TemyList = new ArrayList();
            TemyList.Add(new TKE("Пусто    ",-1));
            if (SQLAcc.Set_table("FinderTemy", AMAS_Query.Class_AMAS_Query.Get_Temy_Kind_Employee(1), null))
            {
                for (int i = 0; i < SQLAcc.Rows_count; i++)
                {
                    SQLAcc.Get_row(i);
                    TemyList.Add(new TKE((string) SQLAcc.Find_Field("description_"), (int)SQLAcc.Find_Field("tema")));
                }
                SQLAcc.ReturnTable();
            }
            Tema.DataSource = TemyList;
            Tema.DisplayMember = "Naming";
            Tema.ValueMember = "Ident";

            KindList = new ArrayList();
            KindList.Add(new TKE("Пусто    ", -1));
            if (SQLAcc.Set_table("FinderKind", AMAS_Query.Class_AMAS_Query.Get_Temy_Kind_Employee(2), null))
            {
                for (int i = 0; i < SQLAcc.Rows_count; i++)
                {
                    SQLAcc.Get_row(i);
                    KindList.Add(new TKE((string)SQLAcc.Find_Field("kind"), (int)SQLAcc.Find_Field("kod")));
                }
                SQLAcc.ReturnTable();
            }
            Kind.DataSource = KindList;
            Kind.DisplayMember = "Naming";
            Kind.ValueMember = "Ident";

            EmployeesList = new ArrayList();
            EmployeesList.Add(new TKE("Пусто    ", -1 ));
            if (SQLAcc.Set_table("FinderEmployee", AMAS_Query.Class_AMAS_Query.Get_Temy_Kind_Employee(3), null))
            {
                for (int i = 0; i < SQLAcc.Rows_count; i++)
                {
                    SQLAcc.Get_row(i);
                    EmployeesList.Add(new TKE((string)SQLAcc.Find_Field("FIO"), (int)SQLAcc.Find_Field("employee")));
                }
                SQLAcc.ReturnTable();
            }
            listEmployees.DataSource = EmployeesList;
            listEmployees.DisplayMember = "Naming";
            listEmployees.ValueMember = "Ident";

            this.Resize += new EventHandler(Finder_Resize);

            Clear_property();
        }

        void Finder_Resize(object sender, EventArgs e)
        {
            groupBox1.Width = this.Width - groupBox1.Left - 2;
            Surname.Width = groupBox1.Width - Surname.Left - 2;
            Enterprise.Width = Surname.Width;
            Employee.Width = Surname.Width;
            Firstname.Width = Surname.Width;
            Lastname.Width = Surname.Width;
            Annotation.Width = Surname.Width;
            Contect.Width = Surname.Width;
            Note.Width = Surname.Width;
            Tema.Width = GBoxControl.Width - Tema.Left - 2;
            if (panel2.Dock == DockStyle.None) resizePanel2();
        }

        public enum Mode
        {
            Unknown=-1,
            Wellcome=1,
            Indoor,
            Outdoc,
            Archive,
            Registrator
        }

        private Mode A_Mode;

        public Mode ModeView
        {
            get { return A_Mode; }
            set 
            { 
                A_Mode = value;
                switch (A_Mode)
                {
                    case Mode.Archive:
                    case Mode.Registrator:
                        //panel1.Dock = DockStyle.Top;
                        //panel2.Dock = DockStyle.None;
                        //panel1.Visible = true;
                        //panel2.Visible = true;
                        panel1.Enabled = true;
                        panel2.Enabled = true;
                        break;
                    case Mode.Wellcome:
                        //panel1.Visible = true;
                        //panel2.Visible = false;
                        panel1.Enabled = true;
                        panel2.Enabled = false;
                        break;
                    case Mode.Indoor:
                        //panel1.Visible = false;
                        //panel2.Visible = true;
                        //panel2.Dock = DockStyle.Fill;
                        panel1.Enabled = false;
                        panel2.Enabled = true;
                        break;
                    case Mode.Outdoc:
                        //panel1.Visible = false;
                        //panel2.Visible = true;
                        //panel2.Dock = DockStyle.Fill;
                        panel1.Enabled = false;
                        panel2.Enabled = true;
                        break;
                    case Mode.Unknown:
                        panel1.Enabled = false;
                        panel2.Enabled = false;
                        break;
                    default:
                        panel1.Enabled = false;
                        panel2.Enabled = false;
                        break;
                }
            }
        }

        private void resizePanel2()
        {
            panel2.Width = panel1.Width;
            panel2.Left = panel1.Left;
            panel2.Top = panel1.Top + panel1.Height;
            panel2.Height = this.Height - GBoxControl.Height - panel1.Height - panel1.Top;

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Clear_property();
        }

        public void Clear_property()
        {
            NumOutdoc.Text = "";
            Enterprise.Text = "";
            Employee.Text = "";
            Surname.Text = "";
            Firstname.Text = "";
            Lastname.Text = "";
            Annotation.Text = "";
            Contect.Text = "";
            Note.Text = "";
            RKK.Text = "";

            try
            {
                Kind.SelectedIndex = 0;
                Tema.SelectedIndex = 0;
            }
            catch { }
            DateOutdoc.Value = DateOutdoc.MinDate;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FndPr.field_org = Enterprise.Text;
            FndPr.Combo_kind =(int) Convert.ToInt32( (string)Kind.SelectedValue);
            FndPr.Combo_tema = (int)Convert.ToInt32((string)Tema.SelectedValue);
            FndPr.Executor = (int)Convert.ToInt32((string)listEmployees.SelectedValue);
            FndPr.field_autor = Employee.Text;
            FndPr.OUT_cod = NumOutdoc.Text;
            FndPr.find_cod = RKK.Text;
            if (DateOutdoc.Value == DateOutdoc.MinDate)
                FndPr.OUT_date = DateTime.MinValue;
            else FndPr.OUT_date = DateOutdoc.Value;
            FndPr.FirstName = Firstname.Text;
            FndPr.Surname = Surname.Text;
            FndPr.LastName = Lastname.Text;
            FndPr.Text_Note = Note.Text;
            FndPr.Text_ANNOT = Annotation.Text;
            FndPr.Text_Content = Contect.Text;

            if (SeekDocsList!=null) SeekDocsList.DocsGroup.ExecSteps(FndPr);
        }
    }
}
