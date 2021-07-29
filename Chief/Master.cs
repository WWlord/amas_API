using System;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;
using ClassErrorProvider;
using ClassStructure;
using Chief.baseLayer;
using CommonValues;
using AMASControlRegisters;


namespace Chief
{
    public partial class Master : Form
    {
        private AMAS_DBI.Class_syb_acc AMAS_access;
        private ClassStructure.Structure StructMyOrg;
        private ClassPattern.FIleSystemShow FSS;

        private RegisrationSettings RegProperty = new RegisrationSettings();

        private Form ARM_form;
        private Form Address_form;
        private Form Population_form;
        private Form Juridic_form;
        private Form Structure_form;
        private Form Personnel_form;
        private Form Rights_form;
        private Form Resources_form;
        private Form Registration_in_form;
        private Form Light_Registration_in_form;
        private Form Outdocs_form;
        private Form BusinessProcess_form;
        private Form BPRTasksForm;
        public int ModuleId;
        

        private ChefSettings frmSettings1 = new ChefSettings();
        private AMASControlRegisters.Document_Viewer document_View;
        private AMASControlRegisters.Document_Viewer document_New;
        private AMASControlRegisters.Document_Viewer Mail_document;
        private AMASControlRegisters.Document_Viewer document_one_View;
        private UCDocsTree DocsTree;

        public Master(AMAS_DBI.Class_syb_acc S_acc)
        {
            InitializeComponent();
            Resizzze();
            AMAS_access = S_acc;
            if (AMAS_DBI.AMASCommand.Access == null) AMAS_DBI.AMASCommand.AccessCommands(AMAS_access);
            this.Resize += new EventHandler(Master_Resize);
            AMAS_access.ErrOfBBL += new Class_syb_acc.ErrorBBLHand(AMAS_access_ErrOfBBL);
            lbErrorsLog.DataSource = null;
            lbErrorsLog.Items.Clear();
            lbErrorsLog.DisplayMember = "Error";
            lbErrorsLog.ValueMember = "ErrId";
            ModuleId = (int)ClassErrorProvider.ErrorBBLProvider.Modules.Master;
            tcALARM.SelectedIndexChanged += new EventHandler(tcALARM_SelectedIndexChanged);
            btnChangePassword.Click += new EventHandler(btnChangePassword_Click);
            this.Load += new EventHandler(Master_Load);
            this.FormClosing += new FormClosingEventHandler(Master_FormClosing);
            ALARMView();
            AMAS_access.ScanDirectory = "";
            AMAS_access.DocumentDirectory = "";
            AMAS_access.PDFDirectory = CommonClass.TempDirectory;
            if (RegProperty != null)
            {
                RegProperty.SettingsKey = "REGpro";
                if (RegProperty.DocumentDirectory != null)
                    if (RegProperty.DocumentDirectory.Trim().Length > 1)
                        AMAS_access.DocumentDirectory = RegProperty.DocumentDirectory;
                if (RegProperty.ScanDirectory != null)
                    if (RegProperty.ScanDirectory.Trim().Length > 1)
                        AMAS_access.ScanDirectory = RegProperty.ScanDirectory;
                if (RegProperty.PDFDirectory != null)
                    if (RegProperty.PDFDirectory.Trim().Length > 1)
                        AMAS_access.PDFDirectory = RegProperty.PDFDirectory;
                RegProperty.PropertyChanged += new PropertyChangedEventHandler(RegProperty_PropertyChanged);
            }
            this.Text = AMAS_access.UserName ;
            if (Application.ProductVersion.Trim().CompareTo(AMAS_access.ApplicationVersion.Trim()) != 0)
            {
                MessageBox.Show("Необходимо обновить версию клиентской программы AMAS в соответствии с версией сервера AMAS.При запуске программы установки выберите режим ОБНОВИТЬ", "Обновите версию программы");
                Process UpdateApplication = new Process();
                string UpdFN=Path.GetTempPath() + "/AMAS.msi";
                if (File.Exists(UpdFN)) File.Delete(UpdFN);
                FileStream UpdFile = new FileStream( UpdFN, FileMode.CreateNew, FileAccess.Write);
                byte[] IMG = AMASCommand.GetUpdateModuleAMAS();
                UpdFile.Write(IMG, 0, IMG.Length);
                UpdFile.Close();
                UpdateApplication.StartInfo.FileName = UpdFN;
                UpdateApplication.Start();
                Application.Exit();
            }

            tcProperty.Selected += new TabControlEventHandler(tcProperty_Selected);

            toolStrip3.Resize += new EventHandler(toolStrip3_Resize);

            ResolutionsListRefresh();

            Show_one_document();
            DocsTree = new UCDocsTree(AMAS_access, document_one_View);
            DocsTree.Dock = DockStyle.Fill;
            DocsTree.SendToBack();
            this.splitContainer1.Panel1.Controls.Add(this.DocsTree);

            getMailAMAS.MessagePicked += new GetMail.MailSelected(getMailAMAS_MessagePicked);
        }

        void getMailAMAS_MessagePicked(DateTime sended, string From, string Subject, ArrayList Attach)
        {
            MessageBox.Show("отправлено " + sended.ToLocalTime().ToShortDateString() + " от " + (From == null ? "" : From) + " аннотация " + (Subject == null ? "" : Subject) + " приложений " + (Attach == null ? "0" : Attach.Count.ToString()));
        }

        void toolStrip3_Resize(object sender, EventArgs e)
        {
            tsResolution.Width = toolStrip3.Width - tsDelete.Width*3;
        }

        void tcProperty_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name.CompareTo("tpDots") == 0)
                if (FSS == null) FSS = new ClassPattern.FIleSystemShow(treeViewFiles, webBrowserPattern, treeViewKT, AMAS_access, ClassPattern.FIleSystemShow.ExecModule.Master);
        }

        void RegProperty_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            AMAS_access.ScanDirectory = "";
            AMAS_access.DocumentDirectory = "";
            AMAS_access.PDFDirectory = CommonClass.TempDirectory;
            if (RegProperty.DocumentDirectory != null)
                if (RegProperty.DocumentDirectory.Trim().Length > 1)
                    AMAS_access.DocumentDirectory = RegProperty.DocumentDirectory;
            if (RegProperty.ScanDirectory != null)
                if (RegProperty.ScanDirectory.Trim().Length > 1)
                    AMAS_access.ScanDirectory = RegProperty.ScanDirectory;
            if (RegProperty.PDFDirectory != null)
                if (RegProperty.PDFDirectory.Trim().Length > 1)
                    AMAS_access.PDFDirectory = RegProperty.PDFDirectory;

            RegProperty.Save();
        }

        private void Master_Load(object sender, EventArgs e)
        {
            ToolStripItemCollection MenuProgramms = null;
            ToolStripItemCollection MenuConsole = null;
            ToolStripItem Itm = menuStripMaster.Items["TSMIProgramms"];
            ToolStripDropDownItem item = Itm as ToolStripDropDownItem;
            //if (item.HasDropDownItems)
                MenuProgramms = item.DropDownItems;

            Itm = menuStripMaster.Items["TSMIConsole"];
            item = Itm as ToolStripDropDownItem;
            //if (item.HasDropDownItems)
                MenuConsole = item.DropDownItems;

            if (AMAS_access.GetRights != null)
            {
                if (AMAS_access.GetRights.Address)
                {
                    this.toolStripMain.Items["tsbAddress"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIAddress"].Visible = true;
                }
                if (AMAS_access.GetRights.Building)
                {
                    this.toolStripMain.Items["tsbAddress"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIAddress"].Visible = true;
                }
                if (AMAS_access.GetRights.Employee)
                {
                    this.toolStripMain.Items["tsbPersonnel"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIPersonal"].Visible = true;
                }
                if (AMAS_access.GetRights.Entrprice)
                {
                    this.toolStripMain.Items["tsbOrganizations"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIOrgs"].Visible = true;
                }
                if (AMAS_access.GetRights.People)
                {
                    this.toolStripMain.Items["tsbPopulaion"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIPopulation"].Visible = true;
                }
                if (AMAS_access.GetRights.Registrator)
                {
                    this.toolStripMain.Items["tsbOutDoc"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIRegOut"].Visible = true;
                }
                if (AMAS_access.GetRights.Structure)
                {
                    this.toolStripMain.Items["tsbStructMyOrg"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIStructure"].Visible = true;
                }
                if (AMAS_access.GetRights.Workflow)
                {
                    this.toolStripMain.Items["tsbChief"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIDocflow"].Visible = true;
                }
                if (AMAS_access.GetRights.Workflow_admin)
                {
                    this.toolStripMain.Items["tsbResources"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIResourse"].Visible = true;
                }
                if (AMAS_access.GetRights.BusinessProcess)
                {
                    this.toolStripMain.Items["tsbBusynessProcesses"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMBizProc"].Visible = true;
                }
                if (AMAS_access.GetRights.Leader)
                {
                    this.toolStripMain.Items["tsbControl"].Visible = true;
                    if (MenuConsole != null) MenuConsole["TSMICID"].Visible = true;
                }
                if (AMAS_access.GetRights.Post)
                {
                    this.toolStripMain.Items["tsMail"].Visible = true;
                    Mail_document = new AMASControlRegisters.Document_Viewer(AMAS_access, null);
                    this.Mail_document.Dock = System.Windows.Forms.DockStyle.Fill;
                    this.Mail_document.Location = new System.Drawing.Point(0, 0);
                    this.Mail_document.Name = "Mail_document";
                    this.Mail_document.New_document = true;
                    this.Mail_document.Doc_ID = 0;
                    this.Mail_document.Sender = 0;
                    this.Mail_document.Size = new System.Drawing.Size(622, 355);
                    this.Mail_document.TabIndex = 0;
                    Mail_document.Dock = DockStyle.Fill;
                    Mail_document.Visible = true;
                    this.splitMail.Panel2.Controls.Add(Mail_document);
                    if (MenuConsole != null) MenuConsole["TSMIMail"].Visible = true;
                }
                if (AMAS_access.GetRights.Inpost)
                {
                    this.toolStripMain.Items["tsbRegistration_in"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIRegIn"].Visible = true;
                    this.toolStripMain.Items["tsbJournalsIn"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMIJourIn"].Visible = true;
                }
                if (AMAS_access.GetRights.Security)
                {
                    this.toolStripMain.Items["tsbRights"].Visible = true;
                    if (MenuProgramms != null) MenuProgramms["TSMISecurity"].Visible = true;
                }
                if (AMAS_access.GetRights.Debug)
                {
                    this.toolStripMain.Items["tsbErrLog"].Visible = true;
                    if (MenuProgramms != null) MenuConsole["TSMIErrors"].Visible = true;
                }
            }
            frmSettings1.SettingsKey = "Master";
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

            String Server = frmSettings1.ServerName;
            if (Server != null)
                this.getMailAMAS.ServerSMTP = Server;

            String Login = frmSettings1.LoginName;
            if (Login != null)
                this.getMailAMAS.UserName = Login;

            String Password = frmSettings1.Password;
            if (Password != null)
                this.getMailAMAS.Password = Password;
        }

        private void Master_FormClosing(Object sender, FormClosingEventArgs e)
        {
            frmSettings1.ServerName = this.getMailAMAS.ServerSMTP;
            frmSettings1.LoginName = this.getMailAMAS.UserName;
            frmSettings1.Password = this.getMailAMAS.Password;
            frmSettings1.Save();
        }

        private void Master_Resize(object sender, EventArgs e)
        {
            Resizzze();
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            bool res = true;
            if (tbPassword.Text.CompareTo(tbPasswordConfirm.Text) == 0)
                res = AMAS_DBI.AMASCommand.Change_password(tbPassword.Text);
            else
                MessageBox.Show("Несовпадение паролей. Повторите ввод пароля.");
            if (!res) MessageBox.Show("У вас недостаточно прав для смены пароля. Обратитесь к администратору.");
        }

        private void Resizzze()
        {
            gbPassword.Left = (panelPassword.Width - gbPassword.Width) / 2;
            gbPassword.Top = (panelPassword.Height - gbPassword.Height) / 2;
        }

        private class ErrContent
        {
            private string name;
            private int id;

            public ErrContent(string s, int i)
            {
                name = s;
                id = i;
            }

            public string Error
            {
                get { return name; }
            }

            public int ErrId
            {
                get { return id; }
            }
        }

        private void AMAS_access_ErrOfBBL(string sss, int Ident)
        {
            try
            {
                lbErrorsLog.Items.Add(new ErrContent(sss, Ident));
            }
            catch { }
        }

        private void tsbChief_Click(object sender, EventArgs e)
        {
            ModuleWorkflow();
        }

        private void ModuleWorkflow()
        {
            if (ARM_form == null || ARM_form.Enabled == false || ARM_form.CanSelect == false)
            {
                ARM_form = new Chef(AMAS_access);
                ARM_form.Show();
            }
            else if (!ARM_form.Disposing)
            {
                ARM_form.Show();
                ARM_form.Activate();
            }
            else
            {
                ARM_form = new Chef(AMAS_access);
                ARM_form.Show();
            }
        }

        private void tsbAddress_Click(object sender, EventArgs e)
        {
            ModuleAddress();
        }

        private void ModuleAddress()
        {
            if (Address_form == null || Address_form.Enabled == false || Address_form.CanSelect == false)
            {
                Address_form = new Address(AMAS_access);
                Address_form.Show();
            }
            else if (!Address_form.Disposing)
            {
                Address_form.Show();
                Address_form.Activate();
            }
            else
            {
                Address_form = new Address(AMAS_access);
                Address_form.Show();
            }
        }

        private void tsbPopulaion_Click(object sender, EventArgs e)
        {
            ModulePopulation();
        }

        private void ModulePopulation()
        {
            if (Population_form == null || Population_form.Enabled == false || Population_form.CanSelect == false)
            {
                Population_form = new Population(AMAS_access);
                Population_form.Show();
            }
            else if (!Population_form.Disposing)
            {
                Population_form.Show();
                Population_form.Activate();
            }
            else
            {
                Population_form = new Population(AMAS_access);
                Population_form.Show();
            }
        }

        private void ModuleCargo()
        {
            MessageBox.Show("Функционал отключен");
            
        }

        private void BPRTasks()
        {
            if (BPRTasksForm == null || BPRTasksForm.Enabled == false || BPRTasksForm.CanSelect == false)
            {
                BPRTasksForm = new BusinessTasks(AMAS_access);
                BPRTasksForm.Show();
            }
            else if (!BPRTasksForm.Disposing)
            {
                BPRTasksForm.Show();
                BPRTasksForm.Activate();
            }
            else
            {
                BPRTasksForm = new BusinessTasks(AMAS_access);
                BPRTasksForm.Show();
            }
        }

        private void ModuleCargoRoutes()
        {
            MessageBox.Show("Функционал отключен");
            
        }

       

        private void ModuleOrgs()
        {
            if (Juridic_form == null || Juridic_form.Enabled == false || Juridic_form.CanSelect == false)
            {
                Juridic_form = new Organizations(AMAS_access);
                Juridic_form.Show();
            }
            else if (!Juridic_form.Disposing)
            {
                Juridic_form.Show();
                Juridic_form.Activate();
            }
            else
            {
                Juridic_form = new Organizations(AMAS_access);
                Juridic_form.Show();
            }
        }

        private void tsbStructMyOrg_Click(object sender, EventArgs e)
        {
            ModuleStructure();
        }

        private void ModuleStructure()
        {
            if (Structure_form == null || Structure_form.Enabled == false || Structure_form.CanSelect == false)
            {
                Structure_form = new StructORG(AMAS_access);
                Structure_form.Show();
            }
            else if (!Structure_form.Disposing)
            {
                Structure_form.Show();
                Structure_form.Activate();
            }
            else
            {
                Structure_form = new StructORG(AMAS_access);
                Structure_form.Show();
            }
        }

        private void tsbPersonnel_Click(object sender, EventArgs e)
        {
            ModulePersonal();
        }

        private void ModulePersonal()
        {
            if (Personnel_form == null || Personnel_form.Enabled == false || Personnel_form.CanSelect == false)
            {
                Personnel_form = new Personel(AMAS_access, imageListStd);
                Personnel_form.Show();
            }
            else if (!Personnel_form.Disposing)
            {
                Personnel_form.Show();
                Personnel_form.Activate();
            }
            else
            {
                Personnel_form = new Personel(AMAS_access, imageListStd);
                Personnel_form.Show();
            }
        }

        private void tsbRights_Click(object sender, EventArgs e)
        {
            ModuleSecurity();
        }

        private void ModuleSecurity()
        {
            if (Rights_form == null || Rights_form.Enabled == false || Rights_form.CanSelect == false)
            {
                Rights_form = new Rights(AMAS_access, imageListStd);
                Rights_form.Show();
            }
            else if (!Rights_form.Disposing)
            {
                Rights_form.Show();
                Rights_form.Activate();
            }
            else
            {
                Rights_form = new Rights(AMAS_access, imageListStd);
                Rights_form.Show();
            }
        }

        private void tsbResources_Click(object sender, EventArgs e)
        {
            ModuleResources();
        }

        private void ModuleResources()
        {
            if (Resources_form == null || Resources_form.Enabled == false || Resources_form.CanSelect == false)
            {
                Resources_form = new Resources(AMAS_access, imageListStd);
                Resources_form.Show();
            }
            else if (!Resources_form.Disposing)
            {
                Resources_form.Show();
                Resources_form.Activate();
            }
            else
            {
                Resources_form = new Resources(AMAS_access, imageListStd);
                Resources_form.Show();
            }
        }

        private void ModuleBusynessProcess()
        {
            if (BusinessProcess_form == null || BusinessProcess_form.Enabled == false || BusinessProcess_form.CanSelect == false)
            {
                BusinessProcess_form = new BusnessProcesses( AMAS_access);
                BusinessProcess_form.Show();
            }
            else if (!BusinessProcess_form.Disposing)
            {
                BusinessProcess_form.Show();
                BusinessProcess_form.Activate();
            }
            else
            {
                BusinessProcess_form = new BusnessProcesses(AMAS_access);
                BusinessProcess_form.Show();
            }
        }

        private void tsbRegistration_in_Click(object sender, EventArgs e)
        {
            ModuleRegIn();
        }

        private void ModuleRegIn()
        {
            if (Registration_in_form == null || Registration_in_form.Enabled == false || Registration_in_form.CanSelect == false)
            {
                Registration_in_form = new Registration_in(AMAS_access, imageListStd);
                Registration_in_form.Show();
            }
            else if (!Registration_in_form.Disposing)
            {
                Registration_in_form.Show();
                Registration_in_form.Activate();
            }
            else
            {
                Registration_in_form = new Registration_in(AMAS_access, imageListStd);
                Registration_in_form.Show();
            }
        }

        private void ModuleLightRegIn()
        {
            if (Light_Registration_in_form == null || Light_Registration_in_form.Enabled == false || Light_Registration_in_form.CanSelect == false)
            {
                Light_Registration_in_form = new JournalWelcome(AMAS_access, imageListStd);
                Light_Registration_in_form.Show();
            }
            else if (!Light_Registration_in_form.Disposing)
            {
                Light_Registration_in_form.Show();
                Light_Registration_in_form.Activate();
            }
            else
            {
                Light_Registration_in_form = new JournalWelcome(AMAS_access, imageListStd);
                Light_Registration_in_form.Show();
            }
        }

        private void tsbOutDoc_Click(object sender, EventArgs e)
        {
            ModuleRegOut();
        }

        private void ModuleRegOut()
        {
            if (Outdocs_form == null || Outdocs_form.Enabled == false || Outdocs_form.CanSelect == false)
            {
                Outdocs_form = new OutDocs(AMAS_access);
                Outdocs_form.Show();
            }
            else if (!Outdocs_form.Disposing)
            {
                Outdocs_form.Show();
                Outdocs_form.Activate();
            }
            else
            {
                Outdocs_form = new OutDocs(AMAS_access);
                Outdocs_form.Show();
            }
        }

        private void tsbFolder_Click(object sender, EventArgs e)
        {
            DocsFolder();
        }

        private void DocsFolder()
        {
            splitContainer1.BringToFront();
            //panelFind.BringToFront();
            DocsTree.BringToFront();
            panelALARM.BringToFront();
            document_one_View.BringToFront();
        }

        ArrayList RankList = null;
        ArrayList UnrankList = null;
        ArrayList EmployeeList = null;

        private class RUE
        {
            private string sss;
            private int ident;

            public RUE(int Id, string str)
            {
                sss = str;
                ident = Id;
            }

            public int ID
            {
                get { return ident; }
            }

            public string name
            {
                get { return sss; }
            }
        }

        private void tsbDelegate_Click(object sender, EventArgs e)
        {
            DelegateToEmployee();
        }

        private void DelegateToEmployee()
        {
            splitContainer1.BringToFront();
            splitContainerUnrank.BringToFront();
            panelUnrank.BringToFront();
            Delegate_load();
        }

        private void Delegate_load()
        {
            string sql;

            lbRank.DataSource = null;
            lbRank.Items.Clear();
            RankList = new ArrayList();
            sql = "select emp_dep_degrees.department,org_jrd_degree.name, emp_dep_degrees.cod from dbo.emp_dep_degrees join dbo.org_jrd_degree on emp_dep_degrees.degree=org_jrd_degree.degree where emp_dep_degrees.deleted is null and emp_dep_degrees.employee=dbo.user_ident()";
            if (AMAS_access.Set_table("MST21", sql, null))
            {
                for (int i = 0; i < AMAS_access.Rows_count; i++)
                {
                    AMAS_access.Get_row(i);
                    int id = (int)AMAS_access.Find_Field("cod");
                    string name = (string)AMAS_access.Find_Field("name");
                    RankList.Add(new RUE(id, name.Trim()));
                }
                AMAS_access.ReturnTable();
                if (RankList.Count > 0)
                {
                    lbRank.DataSource = RankList;
                    lbRank.ValueMember = "ID";
                    lbRank.DisplayMember = "name";
                }
            }
            lbEmployee.DataSource = null;
            lbEmployee.Items.Clear();
            EmployeeList = new ArrayList();
            sql = "select org_our_employees.fio,org_our_employees.id,emp_dep_degrees.leader from dbo.emp_dep_degrees join dbo.org_our_employees on emp_dep_degrees.cod=org_our_employees.cod join dbo.org_jrd_departments_list on org_our_employees.department=org_jrd_departments_list.department where emp_dep_degrees.deleted is null and emp_dep_degrees.leader=1 and org_jrd_departments_list.under in (select department from dbo.emp_dep_degrees where employee=dbo.user_ident()) order by org_our_employees.fio asc";
            if (AMAS_access.Set_table("MST22", sql, null))
            {
                for (int i = 0; i < AMAS_access.Rows_count; i++)
                {
                    AMAS_access.Get_row(i);
                    int id = (int)AMAS_access.Find_Field("id");
                    string name = (string)AMAS_access.Find_Field("fio");
                    EmployeeList.Add(new RUE(id, name));
                }
                AMAS_access.ReturnTable();
            }
            sql = "select fio,id from dbo.emp_ent_employee where id <> dbo.user_ident() and department in (select department from dbo.emp_dep_degrees where employee=dbo.user_ident()) order by fio asc";
            if (AMAS_access.Set_table("MST23", sql, null))
            {
                for (int i = 0; i < AMAS_access.Rows_count; i++)
                {
                    AMAS_access.Get_row(i);
                    int id = (int)AMAS_access.Find_Field("id");
                    string name = (string)AMAS_access.Find_Field("fio");
                    EmployeeList.Add(new RUE(id, name));
                }
                AMAS_access.ReturnTable();
                if (EmployeeList.Count > 0)
                {
                    lbEmployee.DataSource = EmployeeList;
                    lbEmployee.ValueMember = "ID";
                    lbEmployee.DisplayMember = "name";
                }
            }
            lbUnrank.DataSource = null;
            lbUnrank.Items.Clear();
            UnrankList = new ArrayList();
            sql = "select emp_view_unrank.id,org_our_employees.fio, org_jrd_degree.[name] as degree  from dbo.emp_view_unrank join dbo.org_our_employees on emp_view_unrank.degree=org_our_employees.cod join dbo.org_jrd_degree on org_our_employees.degree=org_jrd_degree.degree";
            if (AMAS_access.Set_table("MST24", sql, null))
            {
                for (int i = 0; i < AMAS_access.Rows_count; i++)
                {
                    AMAS_access.Get_row(i);
                    int id = (int)AMAS_access.Find_Field("id");
                    string name = (string)AMAS_access.Find_Field("degree");
                    UnrankList.Add(new RUE(id, name));
                }
                AMAS_access.ReturnTable();
                if (UnrankList.Count > 0)
                {
                    lbUnrank.DataSource = UnrankList;
                    lbUnrank.ValueMember = "ID";
                    lbUnrank.DisplayMember = "name";
                }
            }
        }

        private void btnRunk_Click(object sender, EventArgs e)
        {
            if (lbEmployee.DataSource != null)
                if (lbRank.DataSource != null)
                    if (AMASCommand.RankDegree((int)lbEmployee.SelectedValue, (int)lbRank.SelectedValue))
                        Delegate_load();
                    else
                        MessageBox.Show("Вы не можете назначить. Обратитесь к администратору системы.");
                else
                    MessageBox.Show("Укажите должность");
            else
                MessageBox.Show("Укажите сотрудника");

        }

        private void btnUnrank_Click(object sender, EventArgs e)
        {
            if (lbUnrank.DataSource != null)
                if (AMASCommand.UnrankDegree((int)lbUnrank.SelectedValue))
                    Delegate_load();
                else
                    MessageBox.Show("Вы не можете вернуть назначения. Обратитесь к администратору системы.");
            else
                MessageBox.Show("Укажите замещаемую должность");
        }

        private void tsbControl_Click(object sender, EventArgs e)
        {
            EmployeeControl();
        }

        private void EmployeeControl()
        {
            splitContainer1.BringToFront();
            splitContainerCED.BringToFront();
            panelALARM.BringToFront();
            tcALARM.TabPages.Clear();

            CED();
        }

        private void CED()
        {
            StructMyOrg = new Structure(AMAS_access, treeViewCED, true);
            StructMyOrg.My_DeptStructure();
        }

        private void tsbPassword_Click(object sender, EventArgs e)
        {
            panelPassword.BringToFront();
        }

        private void tsbALARM_Click(object sender, EventArgs e)
        {
            ALARMView();
        }

        private void ALARMView()
        {
            splitContainer1.BringToFront();
            panelALARM.BringToFront();
             tcALARM.TabPages.Clear();
             if (lvALARM != null) lvALARM.Dispose();
            lvALARM = new ListView();
            // 
            // lvALARM
            // 
            this.lvALARM.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lvALARM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvALARM.ForeColor = System.Drawing.Color.BlueViolet;
            this.lvALARM.LargeImageList = this.imageListLarge;
            this.lvALARM.Location = new System.Drawing.Point(0, 0);
            this.lvALARM.Margin = new System.Windows.Forms.Padding(4);
            this.lvALARM.Name = "lvALARM";
            this.lvALARM.Size = new System.Drawing.Size(510, 493);
            this.lvALARM.SmallImageList = this.imageListSmall;
            this.lvALARM.TabIndex = 12;
            this.lvALARM.View = System.Windows.Forms.View.LargeIcon;
            this.lvALARM.SelectedIndexChanged += new System.EventHandler(this.lvALARM_SelectedIndexChanged);
            this.lvALARM.DoubleClick += new EventHandler(lvALARM_DoubleClick);
            lvALARM.Sorting = SortOrder.Ascending;

            this.splitContainer1.Panel1.Controls.Add(this.lvALARM);

            DocId = 0;

            RefreshLevel1();
 
        }

        void lvALARM_DoubleClick(object sender, EventArgs e)
        {
            if (Is_DoubleClick) FillList((ListView)sender);
        }
        private void RefreshLevel1()
        {
            lvALARM.Items.Clear();
            lvALARM.Groups.Clear();

            ListViewGroup WellGroup = new ListViewGroup("WellDocs", "Входящая корреспондеция");
            ListViewGroup InGroup = new ListViewGroup("InDocs", "Внутренняя корреспондеция");
            ListViewGroup VizaGroup = new ListViewGroup("SignDocs", "Корреспондеция на подпись");
            ListViewGroup NewsGroup = new ListViewGroup("NewsDocs", "Уведомления");

            /*            this.lvALARM.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
                        WellGroup,
                        InGroup,
                        VizaGroup,
                        NewsGroup}); */
            WellGroup.HeaderAlignment = HorizontalAlignment.Left;
            InGroup.HeaderAlignment = HorizontalAlignment.Left;
            VizaGroup.HeaderAlignment = HorizontalAlignment.Left;
            NewsGroup.HeaderAlignment = HorizontalAlignment.Left;
            lvALARM.BringToFront();

            int Count;
            // WellGroup.Items.Add("ZWellDocsAlarm", "Входящая корреспонденция", 2);
            lvALARM.Items.Add("WellDocsToday", "Входящая корреспонденция", 2);
            Count = 0;
            if (AMAS_access.Set_table("MST1", AMAS_Query.Class_AMAS_Query.RKK_executor_docs_alarm(), null))
            {
                Count = AMAS_access.Rows_count;
                AMAS_access.ReturnTable();
            }
            if (Count > 0)
                //WellGroup.Items.Add("WellDocsAlarm", " Просрочено " + Count.ToString() + " документов.", 1);
                lvALARM.Items.Add("WellDocsAlarm", " Просрочено " + Count.ToString() + " документов.", 1);
            Count = 0;
            if (AMAS_access.Set_table("MST2", AMAS_Query.Class_AMAS_Query.rkk_executor_docs3days(), null))
            {
                Count = AMAS_access.Rows_count;
                AMAS_access.ReturnTable();
            }
            if (Count > 0)
                //WellGroup.Items.Add("WellDocsP", " Срок исполнения истекает через 3 дня для " + Count.ToString() + " документов.", 1);
                lvALARM.Items.Add("WellDocsP", " Срок исполнения истекает через 3 дня для " + Count.ToString() + " документов.", 1);

            //InGroup.Items.Add("ZInDocsAlarm", "Внутренняя корреспонденция", 2);
            lvALARM.Items.Add("InDocsToday", "Внутренняя корреспонденция", 2);
            Count = 0;
            if (AMAS_access.Set_table("MST3", AMAS_Query.Class_AMAS_Query.RKK_exe_in_docs_alarm(), null))
            {
                Count = AMAS_access.Rows_count;
                AMAS_access.ReturnTable();
            }
            if (Count > 0)
                //InGroup.Items.Add("InDocsAlarm", " Просрочено " + Count.ToString() + " документов.", 1);
                lvALARM.Items.Add("InDocsAlarm", " Просрочено " + Count.ToString() + " документов.", 1);
            Count = 0;
            if (AMAS_access.Set_table("MST4", AMAS_Query.Class_AMAS_Query.rkk_exe_in_docs3days(), null))
            {
                Count = AMAS_access.Rows_count;
                AMAS_access.ReturnTable();
            }
            if (Count > 0)
                //lnGroup.Items.Add("InDocsP", " Срок исполнения истекает через 3 дня для " + Count.ToString() + " документов.", 1);
                lvALARM.Items.Add("InDocsP", " Срок исполнения истекает через 3 дня для " + Count.ToString() + " документов.", 1);

            //VizaGroup.Items.Add("ZSDocsAlarm", "Корреспонденция на согласовании", 2);
            lvALARM.Items.Add("SDocsToday", "Корреспонденция на согласовании", 2);
            Count = 0;
            if (AMAS_access.Set_table("MST5", AMAS_Query.Class_AMAS_Query.RKK_vizing_docs_alarm(), null))
            {
                Count = AMAS_access.Rows_count;
                AMAS_access.ReturnTable();
            }
            if (Count > 0)
                //VizaGroup.Items.Add("SDocsAlarm", " Просрочено " + Count.ToString() + " документов.", 1);
                lvALARM.Items.Add("SDocsAlarm", " Просрочено " + Count.ToString() + " документов.", 1);
            Count = 0;
            if (AMAS_access.Set_table("MST6", AMAS_Query.Class_AMAS_Query.rkk_vizing_docs3days(), null))
            {
                Count = AMAS_access.Rows_count;
                AMAS_access.ReturnTable();
            }
            if (Count > 0)
                //VizaGroup.Items.Add("SDocsP", " Срок исполнения истекает через 3 дня для " + Count.ToString() + " документов.", 1);
                lvALARM.Items.Add("SDocsP", " Срок исполнения истекает через 3 дня для " + Count.ToString() + " документов.", 1);

            lvALARM.Show();
            lvALARM.ShowGroups = true;
            lvALARM.Refresh();
        }

        private void RefreshLevel2()
        {
            lvALARM.Items.Clear();
            lvALARM.Groups.Clear();

            lvALARM.Show();
            lvALARM.ShowGroups = false;
            lvALARM.Refresh();
        }

        private void tsbErrLog_Click(object sender, EventArgs e)
        {
            ErrLogView();
        }

        private void ErrLogView()
        {
            splitContainer1.BringToFront();
            lbErrorsLog.BringToFront();
            panelErrLog.BringToFront();
        }

        private void lbErrorsLog_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (lbErrorsLog != null)
                    if (lbErrorsLog.SelectedItem != null)
                    {
                        ErrContent ErC = (ErrContent)lbErrorsLog.SelectedItem;
                        string[] erContent = AMAS_access.EBBLP.ErrorContent(ErC.ErrId);
                        if (erContent != null)
                        {
                            tbErrLogTitle.Text = erContent[0];
                            tbErrLogChar.Text = erContent[1];
                            tbErrLogDesc.Text = erContent[2];
                        }
                    }
            }
            catch (Exception ex)
            { Console.WriteLine(ex.Message); }
        }

        bool Is_DoubleClick = true;

        private void lvALARM_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!Is_DoubleClick) FillList((ListView)sender);
        }

        private void FillList(ListView sender)
        {
            DocId = 0;
            ListView lv = (ListView)sender;
            string sql = "";
            tcALARM.TabPages.Clear();
            foreach (ListViewItem lvi in lv.SelectedItems)
            {
                switch (lvi.Name)
                {
                    case "WellDocsAlarm":
                        sql = "select * from dbo.RKK_executor_docs_alarm";
                        break;
                    case "WellDocsP":
                        sql = "select * from dbo.rkk_executor_docs where when_m<dateadd( d, 3, getdate()) and kod not in (select kod from dbo.RKK_executor_docs_alarm) and executed is null";
                        break;
                    case "InDocsAlarm":
                        sql = "select * from dbo.RKK_exe_in_docs_alarm";
                        break;
                    case "InDocsP":
                        sql = "select * from dbo.rkk_exe_in_docs where when_m<dateadd( d, 3, getdate()) and kod not in (select kod from dbo.RKK_exe_in_docs_alarm) and executed is null";
                        break;
                    case "SDocsAlarm":
                        sql = "select * from dbo.RKK_vizing_docs_alarm";
                        break;
                    case "SDocsP":
                        sql = "select * from dbo.rkk_vizing_docs where when_v<dateadd( d, 3, getdate()) and kod not in (select kod from dbo.RKK_vizing_docs_alarm) and executed is null";
                        break;
                    case "WellDocsToday":
                        sql = "select * from dbo.RKK_executor_docs_new";
                        break;
                    case "InDocsToday":
                        sql = "select * from dbo.RKK_exe_in_docs_new";
                        break;
                    case "SDocsToday":
                        sql = "select * from dbo.RKK_vizing_docs_new";
                        break;
                    default:
                        sql = "";
                        break;

                }
                if (sql.Length > 0)
                {
                    RefreshLevel2();
                    lvALARM.Items.Add("Return", " К списку каталогов", 0);
                    if (AMAS_access.Set_table("MST7", sql, null))
                    {
                        int firstkod = 0;
                        for (int i = 0; i < AMAS_access.Rows_count; i++)
                        {
                            AMAS_access.Get_row(i);
                            int kod = (int)AMAS_access.Find_Field("kod");
                            if (firstkod == 0) firstkod = kod;
                            string FC = (string)AMAS_access.Find_Field("find_Cod");
                            //tcALARM.TabPages.Add("t" + kod.ToString(), FC.Trim());
                            lvALARM.Items.Add("Doc" + kod.ToString(), FC.Trim(), 4);
                        }
                        if (AMAS_access.Rows_count > 0)
                            Fill_document(panelALARM, firstkod);
                        //if (tcALARM.TabPages.Count > 0) Show_document(tcALARM.TabPages[0]);
                        AMAS_access.ReturnTable();
                    }
                    lvALARM.Items.Add("Answer", "Дать ответ", 5);
                }
                else
                {
                    switch (lvi.Name)
                    {
                        case "Return":
                            RefreshLevel1();
                            break;
                        case "Answer":
                            if (DocumentAnswer != null) this.lvALARM.Controls.Remove(DocumentAnswer);
                            if (document_View != null)
                            {
                                try
                                {
                                    int Answer_count = 0;
                                    int[] Movings = null;
                                    if (AMAS_access.Set_table("MST71", "select * from dbo.rkk_moving where document= " + document_View.Doc_ID.ToString() + " and for_  in (select cod from dbo.emp_dep_degrees where employee=dbo.user_ident() and executed is null)", null))
                                    {
                                        Answer_count = AMAS_access.Rows_count;
                                        Movings = new int[Answer_count];
                                        for (int i = 0; i < AMAS_access.Rows_count; i++)
                                        {
                                            AMAS_access.Find_Field("moving");
                                            Movings[i] = (int)AMAS_access.get_current_Field();
                                        }
                                        AMAS_access.ReturnTable();
                                    }

                                    if (Answer_count >0)
                                    {
                                        int movId = 0;
                                        if (Answer_count > 1)
                                        {
                                            FormMovingList MovList = new FormMovingList(AMAS_access, document_View.Doc_ID);
                                            MovList.ShowDialog();
                                            movId = MovList.movId[0];
                                        }
                                        else movId=Movings[0];
                                        if (document_New != null) document_View.Controls.Remove(document_New);
                                        document_New = new Document_Viewer(AMAS_access, null);
                                        document_View.Controls.Add(document_New);
                                        document_New.Dock = DockStyle.Fill;
                                        document_New.Visible = false;
                                        document_New.Sender = movId;

                                        if (document_New.Sender > 0)
                                        {
                                            DocumentAnswer = new UCNewDocument(AMAS_access, document_New, document_View.Doc_ID,true);
                                            this.lvALARM.Controls.Add(DocumentAnswer);
                                            DocumentAnswer.Dock = DockStyle.Fill;
                                            DocumentAnswer.Visible = true;
                                            DocumentAnswer.BringToFront();
                                            DocumentAnswer.answer("Ответ на поручение по документу " + this.document_View.DocumentNumber);

                                            document_New.New_document = true;
                                            document_New.Doc_ID = 0;
                                            document_New.Refresh();
                                            document_New.Visible = true;
                                            document_New.BringToFront();
                                        }
                                    }
                                    else if (Answer_count == 0)
                                    {
                                        AMAS_access.AddError("Вы не можете дать ответ, поскольку отсутствует поручение по данному документу", "Master - 11.3", "");
                                        MessageBox.Show("Вы не можете дать ответ, поскольку отсутствует поручение по данному документу.");
                                        document_New.Visible = false;
                                        document_View.Controls.Remove(document_New);
                                    }
                               }
                                catch { document_View.Controls.Remove(document_New); }
                            }
                            break;
                        default:
                            if (lvi.Name.Substring(0, 3).ToLower().CompareTo("doc") == 0)
                            {
                                DocId = (int)Convert.ToInt32(lvi.Name.Substring(3));
                                Fill_document(panelALARM, DocId);
                            }
                            break;
                    }
                }
            }
        }
 
        private UCNewDocument DocumentAnswer;
        private int DocId=0;

        private void Show_document(Control Tab)
        {
            document_View = new AMASControlRegisters.Document_Viewer(AMAS_access, null);
            this.document_View.Doc_ID = 0;
            this.document_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.document_View.Location = new System.Drawing.Point(250, 0);
            this.document_View.Name = "document_View" + Tab.Name.Trim();
            this.document_View.New_document = false;
            this.document_View.Sender = 0;
            this.document_View.Size = new System.Drawing.Size(this.tcALARM.Size.Width - 250, 521);
            this.document_View.TabIndex = 3;
            Tab.Controls.Add(this.document_View);
            document_View.Doc_ID = Convert.ToInt32(Tab.Name.Substring(1));
        }

        private void Fill_document(Control Tab,int kod)
        {
            document_View = new AMASControlRegisters.Document_Viewer(AMAS_access, null);
            this.document_View.Doc_ID = 0;
            this.document_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.document_View.Location = new System.Drawing.Point(250, 0);
            this.document_View.Name = "document_Fill" + kod.ToString();
            this.document_View.New_document = false;
            this.document_View.Sender = 0;
            this.document_View.TabIndex = 33;
            Tab.Controls.Add(this.document_View);
            document_View.Doc_ID = kod;
            this.document_View.BringToFront();
            this.document_View.Visible = true;
        }

        private void Show_one_document()
        {
            document_one_View = new AMASControlRegisters.Document_Viewer(AMAS_access, null);
            this.document_one_View.Doc_ID = 0;
            this.document_one_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.document_one_View.Location = new System.Drawing.Point(250, 0);
            this.document_one_View.Name = "document_one_View";
            this.document_one_View.New_document = false;
            this.document_one_View.Sender = 0;
            this.document_one_View.Size = new System.Drawing.Size(this.tcALARM.Size.Width - 250, 521);
            this.document_one_View.TabIndex = 3;
            this.panelALARM.Controls.Add(this.document_one_View);
            this.document_one_View.SendToBack();
            this.document_one_View.Visible = true;
        }

        private void tcALARM_SelectedIndexChanged(object sender, EventArgs e)
        {
            TsbSelect((TabControl)sender);
        }

        private void TsbSelect(TabControl TabC)
        {
            TabPage Tab = TabC.SelectedTab;
            bool IsDoc = false;
            if (Tab != null)
            {
                foreach (Control d in Tab.Controls)
                    if (d.Name.CompareTo("document_View" + Tab.Name.Trim()) == 0)
                        IsDoc = true;
                if (!IsDoc) Show_document(Tab);
            }
        }

        string[] CED_sql;

        private void alarm_messages()
        {
            string sql;
            listViewCED.Items.Clear();
            CED_sql = new string[64];
            for (int i = 0; i < 64; i++)
            {
                CED_sql[i] = "";
            }
            if (treeViewCED.SelectedNode.Name.Substring(0, 1).ToLower().CompareTo("d") == 0)
            {
                //----------- ОТДЕЛ --------------
                //входящие
                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f, rkk_moving.moving, rkk_moving.main_executor, rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where  rkk_moving.exe_doc is null and rkk_moving.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.1", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("В подразделении на исполнении " + AMAS_access.Rows_count.ToString() + " заданий входящих документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.when_m < getdate() and rkk_moving.exe_doc is null and rkk_moving.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.2", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Из них просрочено " + AMAS_access.Rows_count.ToString() + " заданий");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }


                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where  rkk_moving.exe_doc is not null and rkk_moving.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.3", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Подразделением исполнено " + AMAS_access.Rows_count.ToString() + " заданий входящих документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                //внутренние

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_indoor_document join DBo.rkk_flow_document on rkk_indoor_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.exe_doc is null and rkk_moving.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.4", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("В подразделении на исполнении " + AMAS_access.Rows_count.ToString() + " заданий внутренних документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_indoor_document join DBo.rkk_flow_document on rkk_indoor_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.when_m < getdate() and rkk_moving.exe_doc is null and rkk_moving.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.5", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Из них просрочено " + AMAS_access.Rows_count.ToString() + " заданий");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_indoor_document join DBo.rkk_flow_document on rkk_indoor_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.exe_doc is not null and rkk_moving.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.6", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Подразделением исполнено " + AMAS_access.Rows_count.ToString() + " заданий внутренних документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                //новости

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_news.news,rkk_news.for_,rkk_news.newed,rkk_news.when_n  from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_news on rkk_flow_document.kod=rkk_news.[document]  Where rkk_news.newed is null and rkk_news.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.7", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("В подразделении " + AMAS_access.Rows_count.ToString() + " текущих документов для работы");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_news.news,rkk_news.for_,rkk_news.newed,rkk_news.when_n  from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_news on rkk_flow_document.kod=rkk_news.[document]  Where rkk_news.when_n < getdate() and rkk_news.newed is null and rkk_news.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.8", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Из них просрочено " + AMAS_access.Rows_count.ToString() + " документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_news.news,rkk_news.for_,rkk_news.newed,rkk_news.when_n  from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_news on rkk_flow_document.kod=rkk_news.[document]  Where rkk_news.newed is not null and rkk_news.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.9", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Подразделением просмотрено " + AMAS_access.Rows_count.ToString() + " документов для работы");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                //визирование

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_vizing.id as vizing,rkk_vizing.for_,rkk_vizing.executed,rkk_vizing.when_v from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_vizing on rkk_flow_document.kod=rkk_vizing.[document]  Where rkk_vizing.executed is null and rkk_vizing.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.10", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("В подразделении " + AMAS_access.Rows_count.ToString() + " визирование очередных документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_vizing.id as vizing,rkk_vizing.for_,rkk_vizing.executed,rkk_vizing.when_v from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_vizing on rkk_flow_document.kod=rkk_vizing.[document]  Where rkk_vizing.when_v < getdate() and rkk_vizing.executed is null and rkk_vizing.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.11", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Из них просрочено " + AMAS_access.Rows_count.ToString() + " визирований документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_vizing.id as vizing,rkk_vizing.for_,rkk_vizing.executed,rkk_vizing.when_v from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_vizing on rkk_flow_document.kod=rkk_vizing.[document]  Where rkk_vizing.executed is not null and rkk_vizing.department = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.12", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("В подразделении " + AMAS_access.Rows_count.ToString() + " завизированных документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

            }

            else
            {

                //------------ Сотрудник ---------------

                //входящие

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.exe_doc is null and rkk_moving.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.13", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("У сотрудника " + AMAS_access.Rows_count.ToString() + " неисполненных заданий входящих документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.when_m < getdate() and rkk_moving.exe_doc is null and rkk_moving.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.14", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Из них просрочено " + AMAS_access.Rows_count.ToString() + " заданий");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.exe_doc is not null and rkk_moving.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.15", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Сотрудником исполнено " + AMAS_access.Rows_count.ToString() + " заданий входящих документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                //внутренние

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_indoor_document join DBo.rkk_flow_document on rkk_indoor_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.exe_doc is null and rkk_moving.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.16", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("У сотрудника " + AMAS_access.Rows_count.ToString() + " неисполненных заданий внутренних документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_indoor_document join DBo.rkk_flow_document on rkk_indoor_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.when_m < getdate() and rkk_moving.exe_doc is null and rkk_moving.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.17", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Из них просрочено " + AMAS_access.Rows_count.ToString() + " заданий");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_moving.moving,rkk_moving.main_executor,rkk_moving.for_,rkk_moving.executed,rkk_moving.when_m from DBo.rkk_indoor_document join DBo.rkk_flow_document on rkk_indoor_document.kod=rkk_flow_document.kod join DBo.rkk_moving on rkk_flow_document.kod=rkk_moving.[document]  Where rkk_moving.exe_doc is not null and rkk_moving.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.18", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("У сотрудника " + AMAS_access.Rows_count.ToString() + " исполненных заданий внутренних документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                //новости

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_news.news,rkk_news.for_,rkk_news.newed,rkk_news.when_n from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_news on rkk_flow_document.kod=rkk_news.[document]  Where rkk_news.newed is null and rkk_news.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.19", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("У сотрудника " + AMAS_access.Rows_count.ToString() + " очередных документов для работы");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_news.news,rkk_news.for_,rkk_news.newed,rkk_news.when_n from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_news on rkk_flow_document.kod=rkk_news.[document]  Where rkk_news.when_n < getdate() and rkk_news.newed is null and rkk_news.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.20", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Из них просрочено " + AMAS_access.Rows_count.ToString() + " документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_news.news,rkk_news.for_,rkk_news.newed,rkk_news.when_n from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_news on rkk_flow_document.kod=rkk_news.[document]  Where rkk_news.newed is not null and rkk_news.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.21", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Сотрудником рассмотрено " + AMAS_access.Rows_count.ToString() + " документов для работы");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                //визирование

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_vizing.id as vizing,rkk_vizing.for_,rkk_vizing.executed,rkk_vizing.when_v from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_vizing on rkk_flow_document.kod=rkk_vizing.[document]  Where rkk_vizing.executed is null and rkk_vizing.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.22", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("У сотрудника " + AMAS_access.Rows_count.ToString() + " визирование документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_vizing.id as vizing,rkk_vizing.for_,rkk_vizing.executed,rkk_vizing.when_v from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_vizing on rkk_flow_document.kod=rkk_vizing.[document]  Where rkk_vizing.when_v < getdate() and rkk_vizing.executed is null and rkk_vizing.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.23", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Из них просрочено " + AMAS_access.Rows_count.ToString() + " визирований");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

                sql = "select rkk_flow_document.kod, rkk_flow_document.find_cod,rkk_flow_document.date_f,rkk_vizing.id as vizing,rkk_vizing.for_,rkk_vizing.executed,rkk_vizing.when_v from DBo.rkk_wellcome_document join DBo.rkk_flow_document on rkk_wellcome_document.kod=rkk_flow_document.kod join DBo.rkk_vizing on rkk_flow_document.kod=rkk_vizing.[document]  Where rkk_vizing.executed is not null and rkk_vizing.for_ = " + treeViewCED.SelectedNode.Name.Substring(1);
                if (AMAS_access.Set_table("MST27.24", sql, null))
                {
                    if (AMAS_access.Rows_count > 0)
                    {
                        ListViewItem LVI = listViewCED.Items.Add("Сотрудником завизировано " + AMAS_access.Rows_count.ToString() + " документов");
                        CED_sql[LVI.Index] = sql;
                    }
                    AMAS_access.ReturnTable();
                }

            }

        }

        private void treeViewCED_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tcALARM.TabPages.Clear();
            alarm_messages();
        }

        private void listViewCED_SelectedIndexChanged(object sender, EventArgs e)
        {
            tcALARM.TabPages.Clear();
            int c = 0;
            foreach (int l in listViewCED.SelectedIndices)
            {
                if (AMAS_access.Set_table("MST27.25", CED_sql[l], null))
                {
                    for (int i = 0; i < AMAS_access.Rows_count; i++)
                    {
                        AMAS_access.Get_row(i);
                        int kod = (int)AMAS_access.Find_Field("kod");
                        string FC = (string)AMAS_access.Find_Field("find_Cod");
                        tcALARM.TabPages.Add("t" + kod.ToString(), FC.Trim());
                    }
                    c = AMAS_access.Rows_count;
                    AMAS_access.ReturnTable();
                }
            }
            if (c > 0)
            {
                tcALARM.SelectedIndex = 0;
                TsbSelect(tcALARM);
            }
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void buttonSelect_Click(object sender, EventArgs e)
        {
            AMAS_Query.Class_AMAS_Query.DocIndex = DocEnumeration.NewsDocs.Value;

            DateTime fd;
            DateTime ft;

            try { fd = dtFrom.Value.Date; }
            catch { fd = DateTime.MinValue.Date; }
            try { ft = dtTo.Value.Date.AddDays(1); }
            catch { ft = System.DateTime.Today.Date.AddDays(1); }
            AMAS_Query.Class_AMAS_Query.DocsListofPeriod(fd, ft);

            switch (listBoxDocIndex.SelectedIndex)
            {
                case 0:
                    AMAS_Query.Class_AMAS_Query.FiltrIndex = (int)DocEnumeration.NewsDocs.Filters.executor_news;
                    break;
                case 1:
                    AMAS_Query.Class_AMAS_Query.FiltrIndex = (int)DocEnumeration.NewsDocs.Filters.executor_news_new;
                    break;
                case 2:
                    AMAS_Query.Class_AMAS_Query.FiltrIndex = (int)DocEnumeration.NewsDocs.Filters.executor_news_old;
                    break;
                case 3:
                    AMAS_Query.Class_AMAS_Query.FiltrIndex = (int)DocEnumeration.NewsDocs.Filters.executor_news_alarm;
                    break;
            }
            AMAS_Query.Class_AMAS_Query.Get_List_Docs("");
            if (AMAS_access.Set_table("MasterDocsFolder", AMAS_Query.Class_AMAS_Query.Fill_Tree(0, 0, 0), AMAS_Query.Class_AMAS_Query.PrepareDate()))
            {
                try
                {
                    tcALARM.TabPages.Clear();
                    int kod = 0;
                    for (int i = 0; i < AMAS_access.Rows_count; i++)
                    {
                        kod = (int)AMAS_access.Find_Field("kod");
                        tcALARM.TabPages.Add("D" + kod.ToString(), (string)AMAS_access.Find_Field("find_cod"));
                    }
                }
                catch (Exception ex)
                {
                    AMAS_access.EBBLP.AddError(ex.Message, "Master MyFolder - 1", ex.StackTrace);
                }
                AMAS_access.ReturnTable();
            }
        }

        private void tsMail_Click(object sender, EventArgs e)
        {
            panelMail.BringToFront();
        }

        private void tsbProperty_Click(object sender, EventArgs e)
        {
            panelProrerty.BringToFront();
            pGSetting.SelectedObject = RegProperty;

        }

        private void PrDocumentDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialogpro.SelectedPath = RegProperty.DocumentDirectory;
            if (folderBrowserDialogpro.ShowDialog() == DialogResult.OK)
                RegProperty.DocumentDirectory = folderBrowserDialogpro.SelectedPath;
            pGSetting.Refresh();
        }

        private void PrScanDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialogpro.SelectedPath = RegProperty.ScanDirectory;
            if (folderBrowserDialogpro.ShowDialog() == DialogResult.OK)
                RegProperty.ScanDirectory = folderBrowserDialogpro.SelectedPath;
            pGSetting.Refresh();
        }

        private void PrPDFDir_Click(object sender, EventArgs e)
        {
            folderBrowserDialogpro.SelectedPath = RegProperty.PDFDirectory;
            if (folderBrowserDialogpro.ShowDialog() == DialogResult.OK)
                RegProperty.PDFDirectory = folderBrowserDialogpro.SelectedPath;
            pGSetting.Refresh();
        }

        private void версияСПРAMASToolStripMenuItem_Click(object sender, EventArgs e)
        {
            License LC = new License(Application.ProductVersion.Trim());
            LC.ShowDialog();
        }

        private void TSMIDocflow_Click(object sender, EventArgs e)
        {
            ModuleWorkflow();
        }

        private void TSMIAddress_Click(object sender, EventArgs e)
        {
            ModuleAddress();
        }

        private void TSMIPopulation_Click(object sender, EventArgs e)
        {
            ModulePopulation();
        }

        private void TSMIOrgs_Click(object sender, EventArgs e)
        {
            ModuleOrgs();
        }

        private void TSMIStructure_Click(object sender, EventArgs e)
        {
            ModuleStructure();
        }

        private void TSMIPersonal_Click(object sender, EventArgs e)
        {
            ModulePersonal();
        }

        private void TSMISecurity_Click(object sender, EventArgs e)
        {
            ModuleSecurity();
        }

        private void TSMIResourse_Click(object sender, EventArgs e)
        {
            ModuleResources();
        }

        private void TSMIRegIn_Click(object sender, EventArgs e)
        {
            ModuleRegIn();
        }

        private void TSMIRegOut_Click(object sender, EventArgs e)
        {
            ModuleRegOut();
        }

        ArrayList Resolutions = null;

        private void ResolutionsListRefresh()
        {
            listBoxResolutions.Items.Clear();
            Resolutions = AMASCommand.Resolutions_Refresh();
            if (Resolutions != null)
            {
                listBoxResolutions.DisplayMember = "name";
                listBoxResolutions.ValueMember = "id";
                listBoxResolutions.DataSource = Resolutions;
                listBoxResolutions.Refresh();
            }
        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            //reorgres r = null;
            int si=AMASCommand.AddSign(tsResolution.Text.Trim());

            if (si > 0)
            {
               //r = new reorgres (si, tsResolution.Text.Trim());
               Resolutions = AMASCommand.Resolutions_Refresh();
               listBoxResolutions.DisplayMember = "name";
               listBoxResolutions.ValueMember = "id";
               listBoxResolutions.DataSource = Resolutions;
               listBoxResolutions.Refresh();
            }

        }

        private void tsDelete_Click(object sender, EventArgs e)
        {
            if (AMASCommand.DeleteSign((int)Convert.ToInt32( listBoxResolutions.SelectedValue)))
            {
                Resolutions = AMASCommand.Resolutions_Refresh();
                listBoxResolutions.DisplayMember = "name";
                listBoxResolutions.ValueMember = "id";
                listBoxResolutions.DataSource = Resolutions;
                listBoxResolutions.Refresh();
            }

        }

        private void tsbJournalsIn_Click(object sender, EventArgs e)
        {
            ModuleLightRegIn();
        }

        private void TSMIPassword_Click(object sender, EventArgs e)
        {
            panelPassword.BringToFront();
        }

        private void TSMIAlarm_Click(object sender, EventArgs e)
        {
            ALARMView();
        }

        private void TSMICID_Click(object sender, EventArgs e)
        {
            EmployeeControl();
        }

        private void TSMIRankAlign_Click(object sender, EventArgs e)
        {
            DelegateToEmployee();
        }

        private void TSMIErrors_Click(object sender, EventArgs e)
        {
            ErrLogView();
        }

        private void TSMIMail_Click(object sender, EventArgs e)
        {
            panelMail.BringToFront();
        }

        private void TSMIProperty_Click(object sender, EventArgs e)
        {
            panelProrerty.BringToFront();
            pGSetting.SelectedObject = RegProperty;
        }

        private void tsbBusynessProcesses_Click(object sender, EventArgs e)
        {
            ModuleBusynessProcess();
        }

        private void TSMBizProc_Click(object sender, EventArgs e)
        {
            ModuleBusynessProcess();
        }

        private void tsbCargo_Click(object sender, EventArgs e)
        {
            ModuleCargo();
        }

        private void tsbCargoRoutes_Click(object sender, EventArgs e)
        {
            ModuleCargoRoutes();
        }

        private void tsbExecutions_Click(object sender, EventArgs e)
        {
            BPRTasks();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            
        }

    }
    public class reorgres
    {
        int ids = 0;
        string names = "";

        public int Id { get { return ids; } }
        public string Name { get { return names; } }

        public reorgres(int ident, string Nm)
        {
            ids = ident;
            names = Nm;
        }
    }

}