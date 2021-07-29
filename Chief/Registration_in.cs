using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using CommonValues;
using ClassPattern;
using ClassStructure;
using DocumentsByPeriod;
using ClassInterfases;
using ClassDocuments;
using AMASDocuments;
using Chief.baseLayer;
using ClassErrorProvider;
using AMASControlRegisters;

namespace Chief
{
    public partial class Registration_in : Form, FormShowCon
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private AppendDocument AppDocs = null;
        public DocsOfPeriod[] Period;
        private System.Windows.Forms.Panel[] Seek_panels;
        private ClassStructure.Structure Structure;
        AMAS_Query.Workflowdoc Select_docs;
        public int ModuleId;
        private ChefSettings frmSettings1 = new ChefSettings();
        private AMASControlRegisters.ContragentRegister contragentRegistering;

        TwainGui.TwainFrame PicterShow = null;

        Pattern_ids ClueBox;
        Pattern_ids FolderBox;
        Pattern_ids Attach_Autor;
        Pattern_ids Attach_Org;
        Pattern_ids Attach_Degree;
        Pattern_ids Attach_Emp;

        private int form_wight = 0;
        private int form_height = 0;

        private AMASControlRegisters.Document_Viewer document_New;
        private AMASControlRegisters.Document_Viewer document_Show;
        private AMASControlRegisters.JuridicRegister juridicRegister1;
        private AMASControlRegisters.PeopleRegister peopleRegister1;

        public Registration_in(AMAS_DBI.Class_syb_acc ACC, ImageList ImageStd)
        {
            InitializeComponent();
            AMASacc = ACC;

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

            this.contragentRegistering = new AMASControlRegisters.ContragentRegister();
            this.tabPage4.Controls.Add(this.contragentRegistering);
            // 
            // contragentRegistering
            // 
            this.contragentRegistering.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contragentRegistering.Document = 0;
            this.contragentRegistering.Location = new System.Drawing.Point(0, 0);
            this.contragentRegistering.Name = "contragentRegistering";
            this.contragentRegistering.Size = new System.Drawing.Size(631, 534);
            this.contragentRegistering.TabIndex = 0;
            contragentRegistering.connect(ACC);

            if (AMAS_DBI.AMASCommand.Access == null) AMAS_DBI.AMASCommand.AccessCommands(ACC);
            AppDocs = new AppendDocument(AMASacc, this.groupBoxKTC);
            ClueBox = new Pattern_ids(Cluelist);
            FolderBox = new Pattern_ids(BoxInFolder);
            ClueBox.connect(ACC);
            FolderBox.connect(ACC);
            ClueBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_Clue(), "name", "id");
            FolderBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_Folder(), "name", "id");
            Attach_Autor = new Pattern_ids(comboBoxPopulation);
            Attach_Org = new Pattern_ids(comboBoxJuridic);
            Attach_Degree = new Pattern_ids(comboBoxDegree);
            Attach_Emp = new Pattern_ids(comboBoxEmployee);
            Attach_Autor.connect(ACC);
            Attach_Org.connect(ACC);
            Attach_Emp.connect(ACC);
            Attach_Degree.connect(ACC);
            Attach_Emp.connect(ACC);
            Attach_Org.Child = Attach_Degree;
            Attach_Degree.Child = Attach_Emp;
            Attach_Autor.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_attached_Autors(), "fio", "id");
            Attach_Org.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_attached_organizations(), "full_name", "id");
            comboBoxJuridic.SelectedIndexChanged += new EventHandler(comboBoxJuridic_SelectedIndexChanged);
            comboBoxDegree.SelectedIndexChanged += new EventHandler(comboBoxDegree_SelectedIndexChanged);
            form_wight = this.Width;
            form_height = this.Height;

            this.document_New = new AMASControlRegisters.Document_Viewer(AMASacc, null);
            // 
            // document_New
            // 
            this.document_New.Doc_ID = 0;
            this.document_New.Dock = System.Windows.Forms.DockStyle.Fill;
            this.document_New.Location = new System.Drawing.Point(0, 0);
            this.document_New.Name = "document_New";
            this.document_New.New_document = true;
            this.document_New.Sender = 0;
            this.document_New.Size = new System.Drawing.Size(622, 355);
            this.document_New.TabIndex = 0;
            this.tabPage6.Controls.Add(this.document_New);

            this.document_Show = new AMASControlRegisters.Document_Viewer(AMASacc, null);
            // 
            // document_Show
            // 
            this.document_Show.Doc_ID = 0;
            this.document_Show.Dock = System.Windows.Forms.DockStyle.Fill;
            this.document_Show.Location = new System.Drawing.Point(0, 0);
            this.document_Show.Name = "document_Show";
            this.document_Show.New_document = false;
            this.document_Show.Sender = 0;
            this.document_Show.Size = new System.Drawing.Size(416, 521);
            this.document_Show.TabIndex = 3;
            this.splitContainer1.Panel2.Controls.Add(this.document_Show);

            this.monthCalendarExe.DateSelected += new DateRangeEventHandler(monthCalendarExe_DateSelected);
            this.Resize += new EventHandler(Registration_in_Resize);

            Structure = new Structure(AMASacc, treeViewDepts);
            treeViewDepts.CheckBoxes = true;
            SEDO();
            new_document();
            listViewDocs.SelectedIndexChanged += new EventHandler(listViewDocs_SelectedIndexChanged);
            this.Load += new EventHandler(Registration_in_Load);
            this.FormClosed += new FormClosedEventHandler(Registration_in_FormClosed);
            ModuleId = (int)ClassErrorProvider.ErrorBBLProvider.Modules.Registration;
            if (AMASacc.DocumentDirectory != null) document_New.DocumentDirectory = AMASacc.DocumentDirectory;
            peopleRegister1.Maned += new PeopleRegister.ManSelected(peopleRegister1_Maned);
            juridicRegister1.Employed += new JuridicRegister.EmployeeSelected(juridicRegister1_Employed);
            juridicRegister1.Orged += new JuridicRegister.OrgSelected(juridicRegister1_Orged);
        }

        void juridicRegister1_Orged(string Org, int Ident)
        {
            OrgToCombobox();
        }

        void juridicRegister1_Employed(string Org, int Ident)
        {
            OrgToCombobox();
        }

        void peopleRegister1_Maned(string Man, int Ident)
        {
            ManToCombobox();
        }

        private DataGridView DocFormulars=null;

        private void FormularHistory(int DocId)
        {
            if (DocFormulars == null)
            {
                DocFormulars = new DataGridView();
                this.splitContainer1.Panel2.Controls.Add(DocFormulars);
                DocFormulars.Name = "DocFormulars";
                DocFormulars.Dock = DockStyle.Fill;
            }
            DataTable FormularsDoc = null;

            try
            {
                DocFormulars.Rows.Clear();
                DocFormulars.Columns.Clear();
            }
            catch { }
            if (DocFormulars.Visible == false)
            {
                if (AMASacc.Set_table("RegForm1", AMAS_Query.Class_AMAS_Query.DocumentFormular(DocId), null))
                {
                    FormularsDoc = AMASacc.Current_table;
                    AMASacc.ReturnTable();
                }
                if (FormularsDoc != null)
                    //if (FormularsDoc.Rows.Count > 0)
                        try
                        {
                            DocFormulars.DataSource = FormularsDoc;
                            DocFormulars.Columns["employee"].HeaderText = "Сотрудник";
                            DocFormulars.Columns["document"].HeaderText = "Документ";
                            DocFormulars.Columns["dateGet"].HeaderText = "Получил";
                        }
                        catch { }
                DocFormulars.Refresh();
                DocFormulars.ReadOnly = true;
                DocFormulars.BringToFront();
                DocFormulars.Visible = true;
            }
            else DocFormulars.Visible = false;
        }

        void monthCalendarExe_DateSelected(object sender, DateRangeEventArgs e)
        {
            monthCalendarExe.Visible = false;
            tsWkDatExe.Text = e.Start.ToShortDateString();
        }

        private void Registration_in_Load(Object sender, EventArgs e)
        {
            frmSettings1.SettingsKey = "Reg_In";
            //Data bind settings properties with straightforward associations.
            Binding bndBackColor = new Binding("BackColor", frmSettings1,
                "FormBackColor", true, DataSourceUpdateMode.OnPropertyChanged);
            this.DataBindings.Add(bndBackColor);
            Binding bndSize = new Binding("Size", frmSettings1, "FormSize",
                true, DataSourceUpdateMode.OnPropertyChanged);
            this.DataBindings.Add(bndSize);
            Binding bndLocation = new Binding("Location", frmSettings1,
                "FormLocation", true, DataSourceUpdateMode.OnPropertyChanged);
            this.DataBindings.Add(bndLocation);

            //For more complex associations, manually assign associations.
            String savedText = frmSettings1.FormText;
            //Since there is no default value for FormText.
            if (savedText != null)
                this.Text = savedText;
        }

        private void Registration_in_FormClosed(Object sender, FormClosedEventArgs e)
        {
            frmSettings1.Save();
        }

        private void SEDO()
        {
            Select_docs = new AMAS_Query.Workflowdoc(0);
            Seek_panels = new Panel[Select_docs.DS_cnt];

            listViewDocs.Items.Clear();
            for (int i = 0; i < Select_docs.DS_cnt; i++)
            {
                listViewDocs.Items.Add(Select_docs.DocSeek[i].desc);
                Seek_panels[i] = new Panel();
                Seek_panels[i].Dock = DockStyle.Fill;
                Seek_panels[i].BackColor = splitContainer2.Panel1.BackColor;
                Seek_panels[i].Visible = false;
                splitContainer2.Panel1.Controls.Add(Seek_panels[i]);
            }
            Period = new DocsOfPeriod[Select_docs.DS_cnt];
            Period[0] = new DocsOfPeriod(Select_docs.DocSeek[0], Seek_panels[0], this);
            listViewDocs.SelectedIndex = 0;
            Period[0].NodePicked += new DocsOfPeriod.PickedHandler(Registration_in_NodePicked);
        }

        private void Registration_in_NodePicked(int DocId, TreeNode Nod)
        {
            document_Show.BringToFront();
            document_Show.TNode = Nod;
            document_Show.Doc_ID = DocId;

            TsEdAddFile.Visible = false;
            TsEdDelFile.Visible = false;
        }

        public System.Windows.Forms.ImageList imagelib() { return imageList1; }
        public System.Windows.Forms.Panel panel() { return this.splitContainer2.Panel1; }
        public System.Windows.Forms.ToolStripProgressBar FuelBar() { return null; }
        public AMAS_DBI.Class_syb_acc DB_acc() { return AMASacc; }

        private void listViewDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            for (int i = 0; i < Select_docs.DS_cnt; i++) Seek_panels[i].Visible = false;
            Seek_panels[listViewDocs.SelectedIndex].Visible = true;
            if (Seek_panels[listViewDocs.SelectedIndex].Controls.Count == 0)
            {
                Period[listViewDocs.SelectedIndex] = new DocsOfPeriod(Select_docs.DocSeek[listViewDocs.SelectedIndex], Seek_panels[listViewDocs.SelectedIndex], this);
                Period[listViewDocs.SelectedIndex].NodePicked += new DocsOfPeriod.PickedHandler(Registration_in_NodePicked);
            }
            AMAS_Query.Class_AMAS_Query.DocIndex = Select_docs.DocSeek[listViewDocs.SelectedIndex].val;
            Period[listViewDocs.SelectedIndex].DocsGroup.Resize();
        }

        private void Registration_in_Resize(object sender, EventArgs e)
        {
            if (this.Width < form_wight) this.Width = form_wight;
            if (this.Height < form_height) this.Height = form_height;
            tabControlDoc.Top = groupBoxKTC.Top + groupBoxKTC.Height + 2;
            tabControlDoc.Height = groupBoxAddDoc.Height - tabControlDoc.Top - 2;
            tabControlDoc.Left = groupBoxKTC.Left;
            tabControlDoc.Width = groupBoxAddDoc.Width - tabControlDoc.Left - 2;
        }

        private void comboBoxJuridic_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Degrees();
        }

        private void comboBoxDegree_SelectedIndexChanged(object sender, EventArgs e)
        {
            Select_Employees();
        }

        private void зарегистрироватьДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void назначитьДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void свойстваToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.panelProrerty.BringToFront();
            //toolStripMenuItemDoc.Visible = false;
            //спискиToolStripMenuItem.Visible = false;
        }

        private class AppendDocument
        {
            private AMAS_DBI.Class_syb_acc AMASacc;
            private DocAttribute_list DAL = null;

            public AppendDocument(AMAS_DBI.Class_syb_acc ACC, Control KTC)
            {
                AMASacc = ACC;
                DAL = new DocAttribute_list(AMASacc, KTC);
            }

            public int current_Kind { get { return (int)Convert.ToInt32(DAL.comboBoxKind.SelectedValue); } }
            public int current_Tema { get { return (int)Convert.ToInt32(DAL.comboBoxTema.SelectedValue); } }
            public int current_Coming { get { return (int)Convert.ToInt32(DAL.comboBoxComing.SelectedValue); } }

            private class DocAttribute_list
            {
                private AMAS_DBI.Class_syb_acc AMASacc;

                private System.Windows.Forms.Label labelComing;
                private System.Windows.Forms.Label labelTema;
                private System.Windows.Forms.Label labelKind;
                public System.Windows.Forms.ComboBox comboBoxComing;
                public System.Windows.Forms.ComboBox comboBoxTema;
                public System.Windows.Forms.ComboBox comboBoxKind;

                public DocAttribute_list(AMAS_DBI.Class_syb_acc ACC, Control control)
                {
                    AMASacc = ACC;

                    this.comboBoxKind = new System.Windows.Forms.ComboBox();
                    this.comboBoxTema = new System.Windows.Forms.ComboBox();
                    this.comboBoxComing = new System.Windows.Forms.ComboBox();
                    this.labelKind = new System.Windows.Forms.Label();
                    this.labelTema = new System.Windows.Forms.Label();
                    this.labelComing = new System.Windows.Forms.Label();

                    // 
                    // comboBoxKind
                    // 
                    this.comboBoxKind.FormattingEnabled = true;
                    this.comboBoxKind.Location = new System.Drawing.Point(70, 19);
                    this.comboBoxKind.Name = "comboBoxKind";
                    this.comboBoxKind.Size = new System.Drawing.Size(253, 21);
                    this.comboBoxKind.TabIndex = 1;
                    SetFont(comboBoxKind);
                    // 
                    // comboBoxTema
                    // 
                    this.comboBoxTema.FormattingEnabled = true;
                    this.comboBoxTema.Location = new System.Drawing.Point(70, 46);
                    this.comboBoxTema.Name = "comboBoxTema";
                    this.comboBoxTema.Size = new System.Drawing.Size(253, 21);
                    this.comboBoxTema.TabIndex = 2;
                    SetFont(comboBoxTema);
                    // 
                    // comboBoxComing
                    // 
                    this.comboBoxComing.FormattingEnabled = true;
                    this.comboBoxComing.Location = new System.Drawing.Point(70, 73);
                    this.comboBoxComing.Name = "comboBoxComing";
                    this.comboBoxComing.Size = new System.Drawing.Size(253, 21);
                    this.comboBoxComing.TabIndex = 3;
                    SetFont(comboBoxComing);
                    // 
                    // labelKind
                    // 
                    this.labelKind.AutoSize = true;
                    this.labelKind.Location = new System.Drawing.Point(6, 19);
                    this.labelKind.Name = "labelKind";
                    this.labelKind.Size = new System.Drawing.Size(58, 13);
                    this.labelKind.TabIndex = 5;
                    this.labelKind.Text = "Документ";
                    // 
                    // labelTema
                    // 
                    this.labelTema.AutoSize = true;
                    this.labelTema.Location = new System.Drawing.Point(6, 46);
                    this.labelTema.Name = "labelTema";
                    this.labelTema.Size = new System.Drawing.Size(34, 13);
                    this.labelTema.TabIndex = 6;
                    this.labelTema.Text = "Тема";
                    // 
                    // labelComing
                    // 
                    this.labelComing.AutoSize = true;
                    this.labelComing.Location = new System.Drawing.Point(6, 73);
                    this.labelComing.Name = "labelComing";
                    this.labelComing.Size = new System.Drawing.Size(57, 13);
                    this.labelComing.TabIndex = 7;
                    this.labelComing.Text = "Доставка";

                    control.Controls.Add(this.labelComing);
                    control.Controls.Add(this.labelTema);
                    control.Controls.Add(this.labelKind);
                    control.Controls.Add(this.comboBoxComing);
                    control.Controls.Add(this.comboBoxTema);
                    control.Controls.Add(this.comboBoxKind);
                    comboBoxKind.SelectedIndexChanged += new EventHandler(comboBoxKind_SelectedIndexChanged);
                    Refresh();
                }

                private void SetFont(ComboBox Cmb)
                {
                    //Cmb.Font=new Font("Times New Roman",(float)10,FontStyle.Regular);
                    Cmb.Font = new Font("Calibri", (float)10, FontStyle.Regular);
                    
                }

                private void comboBoxKind_SelectedIndexChanged(Object sender, EventArgs e)
                {
                    Select_Temy();
                }

                ArrayList Kinds_list = null;
                ArrayList Tema_list = null;
                ArrayList Coming_list = null;
                private bool SelectTema = true;

                private void Refresh()
                {
                    Kinds_list = new ArrayList();
                    Coming_list = new ArrayList();
                    comboBoxKind.DataSource = null;
                    comboBoxComing.DataSource = null;
                    comboBoxKind.Items.Clear();
                    comboBoxComing.Items.Clear();
                    string name = "";
                    int id = -1;
                    if (AMASacc.Set_table("TRiN1", AMAS_Query.Class_AMAS_Query.Wflow_kinds(), null))
                    {
                        try
                        {
                            for (int i = 0; i < AMASacc.Rows_count; i++)
                            {
                                AMASacc.Get_row(i);
                                id = (int)AMASacc.Find_Field("kod");
                                name = (string)AMASacc.Find_Field("kind");
                                Kinds_list.Add(new CommonClass.Arraysheet(name.Trim(), id));
                            }
                        }
                        catch (Exception ex)
                        {
                            AMASacc.EBBLP.AddError(ex.Message, "Registration In - 1", ex.StackTrace);
                        }
                        AMASacc.ReturnTable();
                    }
                    SelectTema = false;
                    comboBoxKind.DataSource = Kinds_list;
                    if (Kinds_list.Count > 0)
                    {
                        comboBoxKind.DisplayMember = "name";
                        comboBoxKind.ValueMember = "id";
                    }
                    SelectTema = true;

                    if (AMASacc.Set_table("TRiN2", AMAS_Query.Class_AMAS_Query.Wflow_comings(), null))
                    {
                        try
                        {
                            for (int i = 0; i < AMASacc.Rows_count; i++)
                            {
                                AMASacc.Get_row(i);
                                id = (int)AMASacc.Find_Field("cod");
                                name = (string)AMASacc.Find_Field("coming");
                                Coming_list.Add(new CommonClass.Arraysheet(name.Trim(), id));
                            }
                        }
                        catch (Exception ex)
                        {
                            AMASacc.EBBLP.AddError(ex.Message, "Registration In - 2", ex.StackTrace);
                        }
                        AMASacc.ReturnTable();
                    }
                    comboBoxComing.DataSource = Coming_list;
                    if (Coming_list.Count > 0)
                    {
                        comboBoxComing.DisplayMember = "name";
                        comboBoxComing.ValueMember = "id";
                    }

                    Select_Temy();
                }

                private void Select_Temy()
                {
                    if (SelectTema)
                    {
                        Tema_list = new ArrayList();
                        comboBoxTema.DataSource = null;
                        comboBoxTema.Items.Clear();
                        string name = "";
                        int id = -1;
                        if (comboBoxKind.Items.Count > 0)
                        {
                            if (AMASacc.Set_table("TRiN3", AMAS_Query.Class_AMAS_Query.Wflow_temy((int)Convert.ToInt32(comboBoxKind.SelectedValue)), null))
                            {
                                try
                                {
                                    for (int i = 0; i < AMASacc.Rows_count; i++)
                                    {
                                        AMASacc.Get_row(i);
                                        id = (int)AMASacc.Find_Field("tema");
                                        name = (string)AMASacc.Find_Field("description_");
                                        Tema_list.Add(new CommonClass.Arraysheet(name.Trim(), id));
                                    }
                                }
                                catch (Exception ex)
                                {
                                    AMASacc.EBBLP.AddError(ex.Message, "Registration In - 3", ex.StackTrace);
                                }
                                AMASacc.ReturnTable();
                            }
                            comboBoxTema.DataSource = Tema_list;
                            if (Tema_list.Count > 0)
                            {
                                comboBoxTema.DisplayMember = "name";
                                comboBoxTema.ValueMember = "id";
                            }
                        }
                    }
                }
            }
        }

        private void Select_Degrees()
        {
            int ident = Attach_Org.get_ident();
            if (ident >= 0)
                Attach_Degree.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_attached_degrees(ident), "name", "id");
            else Attach_Degree.clear();
        }

        private void Select_Employees()
        {
            int ident = Attach_Degree.get_ident();
            if (ident >= 0)
                Attach_Emp.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_attached_employees(ident), "fio", "id");
            else Attach_Emp.clear();
        }

        private void сохранитьToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void Append_document()
        {
            int document = AMAS_DBI.AMASCommand.Append_Recieved_document(AppDocs.current_Kind, Attach_Org.get_ident(), Attach_Emp.get_ident(), Attach_Autor.get_ident(), textBoxoutCod.Text.Trim(), maskedTextBoxDateOut.Text, AppDocs.current_Tema, AppDocs.current_Coming, document_New.Annotation, ClueBox.get_ident());
            if (document > 0)
            {
                document_New.SaveDocument(document);
                contragentRegistering.Document = document;

                MessageBox.Show("Документ зарегистрирован под номером " + AMAS_DBI.AMASCommand.ShowDocumentFK(document), "РКК", MessageBoxButtons.OK);
            }
            else if (document == 0)
                MessageBox.Show("Документ не зарегистрирован. Повторите попытку.", "РКК", MessageBoxButtons.OK);
            else if (document == -1)
                MessageBox.Show("Сбой при попытке регистрации документа. Для решения проблемы обратитесь к администратору.", "РКК", MessageBoxButtons.OK);
        }

        private void входящаяКорреспонденцияToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void назначенныеДокументыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void новыеДокументыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void архивToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void исполненныеДокументыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void отпечатанныеДокументыToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void исходящаяКорреспонденцияToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void новыйДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void new_document()
        {
            comboBoxPopulation.SelectedIndex = -1;
            comboBoxJuridic.SelectedIndex = -1;
            comboBoxDegree.SelectedIndex = -1;
            comboBoxEmployee.SelectedIndex = -1;
            Cluelist.SelectedIndex = -1;
            BoxInFolder.SelectedIndex = -1;
            textBoxoutCod.Text = "";
            maskedTextBoxDateOut.Text = "";

            document_New.Doc_ID = 0;
            contragentRegistering.Document = 0;
        }

        private void деньToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "1 день";
            Structure.push_letter(true, sql, document_Show.Doc_ID, "", 0);
        }

        private void дняToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "2 дня";
            Structure.push_letter(true, sql, document_Show.Doc_ID, "", 0);
        }

        private void дняToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string sql = "3 дня";
            Structure.push_letter(true, sql, document_Show.Doc_ID, "", 0);
        }

        private void дняToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            string sql = "1 неделя";
            Structure.push_letter(true, sql, document_Show.Doc_ID, "", 0);
        }

        private void днейToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "2 недели";
            Structure.push_letter(true, sql, document_Show.Doc_ID, "", 0);
        }

        private void вСрокДоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string sql = "1 месяц";
            Structure.push_letter(true, sql, document_Show.Doc_ID, "", 0);
        }

        private void вСрокДоToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            //Structure.push_letter(true, sql, document_Show.Doc_ID, "", 0);
        }

        private void исполнениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeViewDepts.BringToFront();
        }

        private void зарегистрироватьДокументToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            NewDocumentMode();
        }

        private void каталогДокументовToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ViewDoCatalogMode();
        }

        private void SetToolTrip(string sel)
        {
            for (int i = 0; i < toolStripMain.Items.Count; i++)
                if (toolStripMain.Items[i].Name.Contains(sel))
                    toolStripMain.Items[i].Visible = true;
                else toolStripMain.Items[i].Visible = false;
        }

        private void RegistationProperty()
        {
            //panelProrerty.BringToFront();
            //pGSetting.SelectedObject = RegProperty;
            //SetToolTrip("Pr");
        }

        private void NewDocumentMode()
        {
            try
            {
                this.panelADDoc.BringToFront();
                //toolStripMenuItemDoc.Visible = true;
                //спискиToolStripMenuItem.Visible = false;
                //исполнениеToolStripMenuItem.Visible = false;
                document_New.New_document = true;
                document_New.Doc_ID = 0;
                Cluelist.SelectedIndex = -1;
                comboBoxPopulation.SelectedIndex = -1;
                comboBoxJuridic.SelectedIndex = -1;
                maskedTextBoxDateOut.Text = "";
                textBoxoutCod.Text = "";
            }
            catch { this.panelADDoc.SendToBack(); }
            SetToolTrip("Dc");
        }

        private void ViewDoCatalogMode()
        {
            this.panelAssignDoc.BringToFront();
            //toolStripMenuItemDoc.Visible = false;
            //спискиToolStripMenuItem.Visible = true;
            //исполнениеToolStripMenuItem.Visible = true;
            SetToolTrip("Wk");
        }

        private void входящаяКорреспонденцияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewDoCatalogMode();
            listViewDocs.SelectedIndex = 0;
        }

        private void новыеДокументыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewDoCatalogMode();
            listViewDocs.SelectedIndex = 2;
        }

        private void назначенныеДокументыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewDoCatalogMode();
            listViewDocs.SelectedIndex = 1;
        }

        private void исполненныеДокументыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewDoCatalogMode();
            listViewDocs.SelectedIndex = 4;
        }

        private void отпечатанныеДокументыToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewDoCatalogMode();
            listViewDocs.SelectedIndex = 5;
        }

        private void исходящаяКорреспонденцияToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewDoCatalogMode();
            listViewDocs.SelectedIndex = 6;
        }

        private void архивToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ViewDoCatalogMode();
            listViewDocs.SelectedIndex = 3;
        }

        private void новыйДокументToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            new_document();
        }

        private void вложитьФайлToolStripMenuItem_Click(object sender, EventArgs e)
        {
            document_New.File_Append();
        }

        private void удалитьФайлToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            document_New.File_Delete();
        }

        private void сохранитьДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Append_document();
        }

        private void вложитьФToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            string sql = tsWkDatExe.Text.Trim();
            Structure.push_letter(true, sql, document_Show.Doc_ID, "", 0);
        }

        private void tsCalendar_Click(object sender, EventArgs e)
        {
            monthCalendarExe.Visible = true;
            monthCalendarExe.Left = 48; //toolStripMain.Items["tsWkDcCalendar"].Placement; //tsWkDcDatExe.
            monthCalendarExe.BringToFront();
            monthCalendarExe.Top = toolStripMain.Top + toolStripMain.Height + 2;
        }

        private void TsDcNewDoc_Click(object sender, EventArgs e)
        {
            //new_document();
            NewDocumentMode();
        }

        private void TsDcAddFile_Click(object sender, EventArgs e)
        {
            document_New.File_Append();
        }

        private void TsDcRemoveFile_Click(object sender, EventArgs e)
        {
            document_New.File_Delete();
        }

        private void TsDcSaveDocument_Click(object sender, EventArgs e)
        {
            Append_document();
            NewDocumentMode();
        }

        private void свойстваToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            RegistationProperty();
        }

        private void tsWkStructure_Click(object sender, EventArgs e)
        {
            treeViewDepts.BringToFront();
        }

        private void buttonADDAutor_Click_1(object sender, EventArgs e)
        {
            ManToCombobox();
        }

        private void ManToCombobox()
        {
            if (peopleRegister1.Current_Man > 0)
                Attach_Autor.Pattern(peopleRegister1.FIO_of_Current_Man, peopleRegister1.Current_Man);
        }

        private void OrgToCombobox()
        {
            if (juridicRegister1.Current_Ident > 0)
            {
                Attach_Degree.clear();
                Attach_Emp.clear();
                Attach_Degree.Pattern("", -1);
                Attach_Emp.Pattern("", -1);
                Attach_Org.Pattern(juridicRegister1.Current_ORG, juridicRegister1.Current_Ident);
                if (juridicRegister1.Current_Degree_ID > 0)
                    Attach_Degree.Pattern(juridicRegister1.Current_Degree, juridicRegister1.Current_Degree_ID);
                if (juridicRegister1.Current_Man_ID > 0)
                    Attach_Emp.Pattern(juridicRegister1.Current_Man, juridicRegister1.Current_Man_ID);
            }
        }

        private void buttonADDEmployee_Click_1(object sender, EventArgs e)
        {
            OrgToCombobox();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if (PicterShow == null)
            {
                LoadPicterShow();
            }
            else
            {
                try
                {
                    PicterShow.Show();
                }
                catch
                {
                    LoadPicterShow();
                }
            }
        }

        private void LoadPicterShow()
        {
            try
            {
                PicterShow = new TwainGui.TwainFrame();
                if (AMASacc.ScanDirectory != null)
                    if (AMASacc.ScanDirectory.Length > 0)
                        PicterShow.SetScanDir(AMASacc.ScanDirectory);
                PicterShow.Show();
                PicterShow.Scanned += new TwainGui.TwainFrame.ScanDoc(PicterShow_Scanned);
            }
            catch
            {
            }

        }

        void PicterShow_Scanned(string Filename)
        {
            document_New.SelectedFile_Append(Filename);
        }

        private void toolStripMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void tsWkEditDoc_Click(object sender, EventArgs e)
        {
            if (document_Show != null)
            {
                int DocID = document_Show.Doc_ID;
                if (AMAS_DBI.AMASCommand.MyDocument(DocID))
                    if (document_Show.CanEdit)
                    {
                        document_Show.Edit_document = true;
                        TsEdAddFile.Visible = true;
                        TsEdDelFile.Visible = true;
                    }
                    else MessageBox.Show("Документ, направленный на согласование или исполнение, не может редактироваться.");
                else MessageBox.Show("Вы не можете редактировать чужой документ");

            }
            else MessageBox.Show("Выберите документ для редактирования");

        }

        private void TsWkSaveDocument_Click(object sender, EventArgs e)
        {
            document_Show.Edit_document = false;
            TsEdAddFile.Visible = false;
            TsEdDelFile.Visible = false;
        }

        private void tsWkFormular_Click(object sender, EventArgs e)
        {
            if (document_Show != null)
                FormularHistory(document_Show.Doc_ID);
        }

        private void tsWkFind_Click(object sender, EventArgs e)
        {

        }

        private void TsEdAddFile_Click(object sender, EventArgs e)
        {
            document_Show.File_Append();
        }

        private void TsEdDelFile_Click(object sender, EventArgs e)
        {
            document_Show.File_Delete();
        }

        private void TsEdAutor_Click(object sender, EventArgs e)
        {
            Form Fautor=null;
            if (document_Show.CanEdit)
            {
                Fautor = new EditAutor(AMASacc, document_Show.Doc_ID, document_Show.DocumentNumber);
            }
            else if(AMAS_DBI.AMASCommand.NullAutor_Recieved_document(document_Show.Doc_ID))
            {
                Fautor = new EditAutor(AMASacc, document_Show.Doc_ID, document_Show.DocumentNumber);
            }
            if(Fautor!=null)
                Fautor.ShowDialog();
        }

        private void TsEdOutcoming_Click(object sender, EventArgs e)
        {
            Form Fout = null;
            if (document_Show.CanEdit)
            {
                Fout = new EditOutcoming(AMASacc, document_Show.Doc_ID, document_Show.DocumentNumber);
            }
            if (Fout != null)
                Fout.ShowDialog();
        }
    }

    public class Pattern_ids
    {
        private int[] Ident;
        private int[] Index;
        private string[] PatternName;

        private int current_number;
        private string textBuffer = "";
        private string backtextBuffer = "";
        private int counter = 0;
        private int array_dimention = 0;
        private bool new_Pattern = false;

        private AMAS_DBI.Class_syb_acc AMASacc;

        public string NewPatternName = "";
        public string ResultErr = "";
        public Pattern_ids Child = null;
        public System.Windows.Forms.ComboBox PatternBox;

        public Pattern_ids(System.Windows.Forms.ComboBox CBox)
        {
            PatternBox = CBox;
            this.PatternBox.Click += new EventHandler(this_PatternBox_Click);
            this.PatternBox.TextChanged += new EventHandler(this_PatternBox_TextChanged);
            this.PatternBox.LostFocus += new EventHandler(this_PatternBox_LostFocus);
            this.PatternBox.SelectedIndexChanged += new EventHandler(this_SelectedIndexChanged);
            this.PatternBox.KeyPress += new KeyPressEventHandler(this_KeyPress);
            this.PatternBox.KeyUp += new KeyEventHandler(this_PatternBox_KeyUp);
        }

        private void this_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Child != null) Child.clear();
        }

        private void this_PatternBox_Click(object sender, EventArgs e)
        {
            cleanNewPattern();
        }

        private void cleanNewPattern()
        {
            new_Pattern = false;
            textBuffer = "";
            backtextBuffer = "";
        }

        private void this_PatternBox_TextChanged(object sender, EventArgs e)
        {
            if (new_Pattern)
            {
                PatternBox.Text = textBuffer + backtextBuffer;
                PatternBox.SelectionStart = textBuffer.Length;
                PatternBox.SelectionLength = backtextBuffer.Length;
            }
        }

        private void this_PatternBox_LostFocus(object sender, EventArgs e)
        {
            if (new_Pattern)
            {
                NewPatternName = PatternBox.Text.Trim();
                cleanNewPattern();
                if (NewPatternName.Length == 0)
                {
                    //altPatternName = new string[PatternName.Length+1];
                    //altIdent = new int[Ident];
                    //get_number_by_text(NewPatternName);
                }
            }
            else NewPatternName = "";
        }
        private void this_PatternBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (new_Pattern)
                    {
                        if (textBuffer.Length > 0)
                            textBuffer = textBuffer.Substring(0, textBuffer.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = PatternName[number].Substring(textBuffer.Length);
                        PatternBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Pattern = true;
                        backtextBuffer = "";
                        textBuffer = PatternBox.Text.Substring(0, PatternBox.Text.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = PatternName[number].Substring(textBuffer.Length);
                        PatternBox.Text = textBuffer;
                    }
                    break;
                case Keys.Right:
                    if (new_Pattern)
                    {
                        if (backtextBuffer.Length > 0)
                            textBuffer += backtextBuffer.Substring(0, 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = PatternName[number].Substring(textBuffer.Length);
                        PatternBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Pattern = true;
                        backtextBuffer = "";
                        int number = get_number_by_text(textBuffer);
                        textBuffer = PatternBox.Text;
                        PatternBox.Text = textBuffer;
                    }
                    break;
                case Keys.Delete:
                    if (new_Pattern)
                    {
                        backtextBuffer = "";
                        PatternBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Pattern = true;
                        backtextBuffer = "";
                        textBuffer = PatternBox.Text;
                        if (Child != null)
                            Child.clear();
                    }
                    break;
            }
        }

        private void this_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((char)e.KeyChar)
            {
                case (char)Keys.Back:
                    if (new_Pattern)
                    {
                        if (textBuffer.Length > 0)
                            textBuffer = textBuffer.Substring(0, textBuffer.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = PatternName[number].Substring(textBuffer.Length);
                    }
                    else
                    {
                        new_Pattern = true;
                        backtextBuffer = "";
                        textBuffer = PatternBox.Text;
                    }
                    break;
                case (char)Keys.End:
                    if (new_Pattern)
                    {
                        textBuffer += backtextBuffer;
                        backtextBuffer = "";
                    }
                    break;
                case (char)Keys.Home:
                    if (new_Pattern)
                    {
                        backtextBuffer = textBuffer + backtextBuffer;
                        textBuffer = "";
                    }
                    break;
                case (char)Keys.Escape:
                    backtextBuffer = "";
                    textBuffer = "";
                    if (Child != null)
                        Child.clear();
                    break;
                default:
                    if ((e.KeyChar >= " ".ToCharArray()[0] && e.KeyChar <= "z".ToCharArray()[0]) || (e.KeyChar >= "А".ToCharArray()[0] && e.KeyChar <= "я".ToCharArray()[0]))
                    {
                        if (new_Pattern)
                        {
                            textBuffer += Convert.ToString(e.KeyChar);
                            int number = get_number_by_text(textBuffer);
                            if (number >= 0) backtextBuffer = PatternName[number].Substring(textBuffer.Length);
                            else backtextBuffer = "";
                        }
                        else
                        {
                            new_Pattern = true;
                            textBuffer = PatternBox.Text + Convert.ToString(e.KeyChar); ;
                            backtextBuffer = "";
                            if (Child != null)
                                Child.clear();
                        }
                    }
                    break;
            }
        }

        public void connect(AMAS_DBI.Class_syb_acc Acc)
        {
            AMASacc = Acc;
        }

        private void add_ident(int indx, int idnt, string text)
        {
            Ident[counter] = idnt;
            Index[counter] = indx;
            PatternName[counter] = text;
            counter++;
            current_number = counter;
        }

        public void clear()
        {
            counter = 0;
            cleanNewPattern();
            NewPatternName = "";
            new_Pattern = false;
            current_number = -1;
            array_dimention = 0;
            if (PatternBox != null)
            {
                PatternBox.Items.Clear();
                PatternBox.Refresh();
                PatternBox.Text = "";
            }
            if (Child != null) Child.clear();
        }

        public int get_ident()
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (Index[i] == PatternBox.SelectedIndex)
                    {
                        current_number = i;
                        return Ident[i];
                    }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 1", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        private int get_number_by_text(string text)
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (text.Length <= PatternName[i].Length)
                        if (text.ToLower().CompareTo(PatternName[i].Substring(0, text.Length).ToLower()) == 0)
                        {
                            current_number = i;
                            return i;
                        }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 2", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        public int get_index_by_text(string txt)
        {
            int num = get_number_by_text(txt);
            if (num >= 0)
            {
                PatternBox.SelectedIndex = Index[num];
                return Index[num];
            }
            else
            {
                return -1;
            }
        }

        public int get_index(int idnt)
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (Ident[i] == idnt)
                    {
                        PatternBox.SelectedIndex = Index[i];
                        current_number = i;
                        return Index[i];
                    }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 3", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        public void Select_Subject(string sql, string fld, string ids)
        {
            if (AMASacc.Set_table("TPat1", sql, null))
            {
                try
                {
                    string OName = "";
                    clear();
                    int ind = 0;
                    int id = 0;
                    array_dimention = AMASacc.Rows_count;
                    Index = new int[array_dimention+1];
                    Ident = new int[array_dimention+1];
                    PatternName = new string[array_dimention+1];
                    ind = PatternBox.Items.Add("");
                    add_ident(ind, -1, "");

                    for (int i = 0; i < array_dimention; i++)
                    {
                        AMASacc.Get_row(i);
                        AMASacc.Find_Field(fld);
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                        else OName = "";
                        ind = PatternBox.Items.Add(OName);
                        id = (int)AMASacc.Find_Field(ids);
                        add_ident(ind, id, OName);
                    }
                    array_dimention++;
                }
                catch (Exception err)
                {
                    AMASacc.EBBLP.AddError(err.Message, "Pattern - 4", err.StackTrace);
                    ResultErr = err.Message;
                }
                AMASacc.ReturnTable();
            }
            if (array_dimention > 0)
            {
                PatternBox.SelectedIndex = 0;
                PatternBox.Refresh();
            }
            else
            {
                current_number = -1;
                PatternBox.SelectedIndex = -1;
                PatternBox.Text = "";
                PatternBox.Refresh();
            }
        }

        public void Pattern(string name, int idx)
        {
            int DIM = -1;
            int[] Id = new int[array_dimention + 1];
            int[] Ind = new int[array_dimention + 1];
            string[] AddrN = new string[array_dimention + 1];
            for (int i = 0; i < array_dimention; i++)
            {
                Id[i] = Ident[i];
                Ind[i] = Index[i];
                AddrN[i] = PatternName[i];
            }
            Id[array_dimention] = idx;
            AddrN[array_dimention] = name;
            DIM = PatternBox.Items.Add(name);
            Ind[array_dimention] = DIM;
            Ident = Id;
            Index = Ind;
            PatternName = AddrN;
            array_dimention++;
            PatternBox.Refresh();
            PatternBox.SelectedIndex = DIM;
            
        }
    }

    public class Selected_Depesh
    {
        private AMAS_DBI.Class_syb_acc AMASacc;

        public ArrayList SelectedAutors;
        public ArrayList SelectedOrgs;
        public ArrayList SelectedDegrees;
        public ArrayList SelectedEmployees;

        private ComboBox cbOrg;
        private ComboBox cbMan;
        private ComboBox cbRank;
        private ComboBox cbEmpl;

        private class SelAO
        {
            public int ID { get { return IDAO; } }
            public string Name { get { return NameAO; } }

            private int IDAO;
            private string NameAO;

            public SelAO(int ident, string Nm)
            {
                NameAO = Nm;
                IDAO = ident;
            }
        }

        public Selected_Depesh(AMAS_DBI.Class_syb_acc Acc, ComboBox Man, ComboBox Org, ComboBox Rank, ComboBox Empl)
        {
            AMASacc = Acc;
            cbOrg = Org;
            cbMan = Man;
            cbRank = Rank;
            cbEmpl = Empl;

            Select_People();
            Select_Orgs();
        }

        private void Select_People()
        {
            SelectedAutors = new ArrayList();
            SelectedAutors.Add( new SelAO(0, ""));
            int id;
            string name;

            if (AMASacc.Set_table("RegAutSel", AMAS_Query.Class_AMAS_Query.Registrator_selected_autors(), null))
            {
                try
                {
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                    {
                        AMASacc.Get_row(i);
                        id = (int)AMASacc.Find_Field("id");
                        name = (string)AMASacc.Find_Field("fio");
                        SelectedAutors.Add(new SelAO(id,name.Trim() ));
                    }
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Registration In - 100", ex.StackTrace);
                }
                AMASacc.ReturnTable();
            }
            cbMan.DataSource = SelectedAutors;
            cbMan.DisplayMember = "Name";
            cbMan.ValueMember = "ID";
        }

        private void Select_Orgs()
        {
            SelectedOrgs = new ArrayList();
            SelectedOrgs.Add(new SelAO(0, ""));
            int id;
            string name;

            if (AMASacc.Set_table("RegOrgSel", AMAS_Query.Class_AMAS_Query.Registrator_selected_Orgs(), null))
            {
                try
                {
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                    {
                        AMASacc.Get_row(i);
                        id = (int)AMASacc.Find_Field("id");
                        name = (string)AMASacc.Find_Field("full_name");
                        SelectedOrgs.Add(new SelAO(id,name.Trim() ));
                    }
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Registration In - 100", ex.StackTrace);
                }
                AMASacc.ReturnTable();
            }
            cbOrg.DataSource = SelectedOrgs;
            cbOrg.DisplayMember="Name";
            cbOrg.ValueMember = "ID";
        }

        public void AddMan(string name, int id)
        {
            cbMan.DataSource = null;
            int ind=SelectedAutors.Add(new SelAO(id, name));
            cbMan.DataSource = SelectedAutors;
            cbMan.DisplayMember = "Name";
            cbMan.ValueMember = "ID";

            cbMan.Refresh();
            cbMan.SelectedIndex = ind;
        }

        public void AddOrg(string name, int id)
        {
            cbOrg.DataSource = null;
            int ind = SelectedOrgs.Add(new SelAO(id, name));
            cbOrg.DataSource = SelectedOrgs;
            cbOrg.DisplayMember = "Name";
            cbOrg.ValueMember = "ID";

            cbOrg.Refresh();
            cbOrg.SelectedIndex = ind;
        }
    }
}