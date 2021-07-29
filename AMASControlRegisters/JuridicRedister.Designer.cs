namespace AMASControlRegisters
{
    partial class JuridicRegister
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JuridicRegister));
            this.tabOrgMan = new System.Windows.Forms.TabControl();
            this.Juridic = new System.Windows.Forms.TabPage();
            this.JuridicList = new System.Windows.Forms.ListBox();
            this.addressReg = new AMASControlRegisters.AddressRegister();
            this.toolStripOrg = new System.Windows.Forms.ToolStrip();
            this.Select_jurs = new System.Windows.Forms.ToolStripButton();
            this.ClearOrgs = new System.Windows.Forms.ToolStripButton();
            this.set_orgadr = new System.Windows.Forms.ToolStripButton();
            this.undorg = new System.Windows.Forms.ToolStripButton();
            this.redorg = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.newOrg = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.ORG_Name = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripLabel5 = new System.Windows.Forms.ToolStripLabel();
            this.Short_Name = new System.Windows.Forms.ToolStripTextBox();
            this.Employes = new System.Windows.Forms.TabPage();
            this.listEmployees = new System.Windows.Forms.ListBox();
            this.panelADDEmployee = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Select_emp = new System.Windows.Forms.ToolStripButton();
            this.Add_emp = new System.Windows.Forms.ToolStripButton();
            this.label1 = new System.Windows.Forms.Label();
            this.Father = new System.Windows.Forms.TextBox();
            this.Naming = new System.Windows.Forms.TextBox();
            this.Family = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.Degree = new System.Windows.Forms.TextBox();
            this.Select_men = new System.Windows.Forms.ToolStripButton();
            this.Clear = new System.Windows.Forms.ToolStripButton();
            this.set_address = new System.Windows.Forms.ToolStripButton();
            this.undo = new System.Windows.Forms.ToolStripButton();
            this.redo = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.add_record = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton4 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripButton6 = new System.Windows.Forms.ToolStripButton();
            this.tabOrgMan.SuspendLayout();
            this.Juridic.SuspendLayout();
            this.toolStripOrg.SuspendLayout();
            this.Employes.SuspendLayout();
            this.panelADDEmployee.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabOrgMan
            // 
            this.tabOrgMan.Controls.Add(this.Juridic);
            this.tabOrgMan.Controls.Add(this.Employes);
            this.tabOrgMan.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabOrgMan.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tabOrgMan.HotTrack = true;
            this.tabOrgMan.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.tabOrgMan.Location = new System.Drawing.Point(0, 0);
            this.tabOrgMan.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabOrgMan.Name = "tabOrgMan";
            this.tabOrgMan.SelectedIndex = 0;
            this.tabOrgMan.Size = new System.Drawing.Size(787, 302);
            this.tabOrgMan.TabIndex = 0;
            // 
            // Juridic
            // 
            this.Juridic.BackColor = System.Drawing.Color.Transparent;
            this.Juridic.Controls.Add(this.JuridicList);
            this.Juridic.Controls.Add(this.addressReg);
            this.Juridic.Controls.Add(this.toolStripOrg);
            this.Juridic.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Juridic.Location = new System.Drawing.Point(4, 31);
            this.Juridic.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Juridic.Name = "Juridic";
            this.Juridic.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Juridic.Size = new System.Drawing.Size(779, 267);
            this.Juridic.TabIndex = 0;
            this.Juridic.Text = "Организации";
            this.Juridic.UseVisualStyleBackColor = true;
            // 
            // JuridicList
            // 
            this.JuridicList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.JuridicList.Font = new System.Drawing.Font("Times New Roman", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.JuridicList.FormattingEnabled = true;
            this.JuridicList.ItemHeight = 16;
            this.JuridicList.Location = new System.Drawing.Point(4, 4);
            this.JuridicList.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.JuridicList.Name = "JuridicList";
            this.JuridicList.Size = new System.Drawing.Size(370, 232);
            this.JuridicList.TabIndex = 32;
            // 
            // addressReg
            // 
            this.addressReg.Dock = System.Windows.Forms.DockStyle.Right;
            this.addressReg.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.addressReg.Location = new System.Drawing.Point(374, 4);
            this.addressReg.Margin = new System.Windows.Forms.Padding(12, 7, 12, 7);
            this.addressReg.Name = "addressReg";
            this.addressReg.Size = new System.Drawing.Size(401, 232);
            this.addressReg.TabIndex = 31;
            // 
            // toolStripOrg
            // 
            this.toolStripOrg.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripOrg.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Select_jurs,
            this.ClearOrgs,
            this.set_orgadr,
            this.undorg,
            this.redorg,
            this.toolStripLabel3,
            this.newOrg,
            this.toolStripLabel4,
            this.ORG_Name,
            this.toolStripLabel5,
            this.Short_Name});
            this.toolStripOrg.Location = new System.Drawing.Point(4, 236);
            this.toolStripOrg.Name = "toolStripOrg";
            this.toolStripOrg.Size = new System.Drawing.Size(771, 27);
            this.toolStripOrg.TabIndex = 28;
            this.toolStripOrg.Text = " ";
            // 
            // Select_jurs
            // 
            this.Select_jurs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Select_jurs.Image = ((System.Drawing.Image)(resources.GetObject("Select_jurs.Image")));
            this.Select_jurs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Select_jurs.Name = "Select_jurs";
            this.Select_jurs.Size = new System.Drawing.Size(23, 24);
            this.Select_jurs.Tag = "Найти людей";
            this.Select_jurs.ToolTipText = "Поиск";
            this.Select_jurs.Click += new System.EventHandler(this.Select_Click_1);
            // 
            // ClearOrgs
            // 
            this.ClearOrgs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ClearOrgs.Image = ((System.Drawing.Image)(resources.GetObject("ClearOrgs.Image")));
            this.ClearOrgs.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ClearOrgs.Name = "ClearOrgs";
            this.ClearOrgs.Size = new System.Drawing.Size(23, 24);
            this.ClearOrgs.Tag = "Очистить чписок";
            this.ClearOrgs.ToolTipText = "Очистить";
            this.ClearOrgs.Click += new System.EventHandler(this.ClearOrgs_Click);
            // 
            // set_orgadr
            // 
            this.set_orgadr.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.set_orgadr.Image = ((System.Drawing.Image)(resources.GetObject("set_orgadr.Image")));
            this.set_orgadr.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.set_orgadr.Name = "set_orgadr";
            this.set_orgadr.Size = new System.Drawing.Size(23, 24);
            this.set_orgadr.Tag = "Дать адрес";
            this.set_orgadr.ToolTipText = "Сменить адрес";
            this.set_orgadr.Click += new System.EventHandler(this.set_orgadr_Click);
            // 
            // undorg
            // 
            this.undorg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undorg.Image = ((System.Drawing.Image)(resources.GetObject("undorg.Image")));
            this.undorg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undorg.Name = "undorg";
            this.undorg.Size = new System.Drawing.Size(23, 24);
            this.undorg.Tag = "Назад";
            this.undorg.ToolTipText = "Предыдущий список";
            this.undorg.Click += new System.EventHandler(this.undorg_Click);
            // 
            // redorg
            // 
            this.redorg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redorg.Image = ((System.Drawing.Image)(resources.GetObject("redorg.Image")));
            this.redorg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redorg.Name = "redorg";
            this.redorg.Size = new System.Drawing.Size(23, 24);
            this.redorg.Tag = "Вперёд";
            this.redorg.ToolTipText = "Последующий список";
            this.redorg.Click += new System.EventHandler(this.redorg_Click);
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(0, 24);
            // 
            // newOrg
            // 
            this.newOrg.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.newOrg.Image = ((System.Drawing.Image)(resources.GetObject("newOrg.Image")));
            this.newOrg.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.newOrg.Name = "newOrg";
            this.newOrg.Size = new System.Drawing.Size(23, 24);
            this.newOrg.Tag = "Внести в реестр";
            this.newOrg.ToolTipText = "Внести в реестр";
            this.newOrg.Click += new System.EventHandler(this.newOrg_Click);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(77, 24);
            this.toolStripLabel4.Text = "Название";
            // 
            // ORG_Name
            // 
            this.ORG_Name.AcceptsReturn = true;
            this.ORG_Name.AcceptsTab = true;
            this.ORG_Name.AutoSize = false;
            this.ORG_Name.MaxLength = 500;
            this.ORG_Name.Name = "ORG_Name";
            this.ORG_Name.Size = new System.Drawing.Size(265, 27);
            // 
            // toolStripLabel5
            // 
            this.toolStripLabel5.Name = "toolStripLabel5";
            this.toolStripLabel5.Size = new System.Drawing.Size(98, 24);
            this.toolStripLabel5.Text = "Сокращённо";
            // 
            // Short_Name
            // 
            this.Short_Name.AcceptsReturn = true;
            this.Short_Name.AcceptsTab = true;
            this.Short_Name.AutoSize = false;
            this.Short_Name.Name = "Short_Name";
            this.Short_Name.Size = new System.Drawing.Size(80, 21);
            this.Short_Name.Click += new System.EventHandler(this.Short_Name_Click);
            // 
            // Employes
            // 
            this.Employes.Controls.Add(this.listEmployees);
            this.Employes.Controls.Add(this.panelADDEmployee);
            this.Employes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Employes.ForeColor = System.Drawing.Color.Navy;
            this.Employes.Location = new System.Drawing.Point(4, 31);
            this.Employes.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Employes.Name = "Employes";
            this.Employes.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Employes.Size = new System.Drawing.Size(779, 267);
            this.Employes.TabIndex = 1;
            this.Employes.UseVisualStyleBackColor = true;
            // 
            // listEmployees
            // 
            this.listEmployees.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listEmployees.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.listEmployees.FormattingEnabled = true;
            this.listEmployees.ItemHeight = 17;
            this.listEmployees.Location = new System.Drawing.Point(4, 4);
            this.listEmployees.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listEmployees.Name = "listEmployees";
            this.listEmployees.Size = new System.Drawing.Size(340, 259);
            this.listEmployees.TabIndex = 3;
            // 
            // panelADDEmployee
            // 
            this.panelADDEmployee.Controls.Add(this.toolStrip1);
            this.panelADDEmployee.Controls.Add(this.label1);
            this.panelADDEmployee.Controls.Add(this.Father);
            this.panelADDEmployee.Controls.Add(this.Naming);
            this.panelADDEmployee.Controls.Add(this.Family);
            this.panelADDEmployee.Controls.Add(this.label4);
            this.panelADDEmployee.Controls.Add(this.label3);
            this.panelADDEmployee.Controls.Add(this.label5);
            this.panelADDEmployee.Controls.Add(this.Degree);
            this.panelADDEmployee.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelADDEmployee.Location = new System.Drawing.Point(344, 4);
            this.panelADDEmployee.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelADDEmployee.Name = "panelADDEmployee";
            this.panelADDEmployee.Size = new System.Drawing.Size(431, 259);
            this.panelADDEmployee.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Select_emp,
            this.Add_emp});
            this.toolStrip1.Location = new System.Drawing.Point(285, 186);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(35, 25);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Select_emp
            // 
            this.Select_emp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Select_emp.Image = ((System.Drawing.Image)(resources.GetObject("Select_emp.Image")));
            this.Select_emp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Select_emp.Name = "Select_emp";
            this.Select_emp.Size = new System.Drawing.Size(23, 22);
            this.Select_emp.Visible = false;
            // 
            // Add_emp
            // 
            this.Add_emp.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Add_emp.Image = ((System.Drawing.Image)(resources.GetObject("Add_emp.Image")));
            this.Add_emp.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Add_emp.Name = "Add_emp";
            this.Add_emp.Size = new System.Drawing.Size(23, 22);
            this.Add_emp.ToolTipText = "Назначить сотрудника";
            this.Add_emp.Click += new System.EventHandler(this.Add_emp_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(29, 41);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 15;
            this.label1.Text = "Должность";
            // 
            // Father
            // 
            this.Father.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Father.Location = new System.Drawing.Point(137, 140);
            this.Father.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Father.Name = "Father";
            this.Father.Size = new System.Drawing.Size(263, 23);
            this.Father.TabIndex = 14;
            // 
            // Naming
            // 
            this.Naming.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Naming.Location = new System.Drawing.Point(137, 108);
            this.Naming.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Naming.Name = "Naming";
            this.Naming.Size = new System.Drawing.Size(263, 23);
            this.Naming.TabIndex = 13;
            // 
            // Family
            // 
            this.Family.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Family.Location = new System.Drawing.Point(137, 78);
            this.Family.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Family.Name = "Family";
            this.Family.Size = new System.Drawing.Size(263, 23);
            this.Family.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label4.Location = new System.Drawing.Point(31, 112);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "Имя";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(29, 81);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 17);
            this.label3.TabIndex = 16;
            this.label3.Text = "Фамилия";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label5.Location = new System.Drawing.Point(31, 144);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 17);
            this.label5.TabIndex = 18;
            this.label5.Text = "Отчество";
            // 
            // Degree
            // 
            this.Degree.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Degree.Location = new System.Drawing.Point(137, 37);
            this.Degree.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Degree.Name = "Degree";
            this.Degree.Size = new System.Drawing.Size(227, 23);
            this.Degree.TabIndex = 11;
            // 
            // Select_men
            // 
            this.Select_men.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Select_men.Image = ((System.Drawing.Image)(resources.GetObject("Select_men.Image")));
            this.Select_men.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Select_men.Name = "Select_men";
            this.Select_men.Size = new System.Drawing.Size(23, 22);
            this.Select_men.Tag = "Найти людей";
            // 
            // Clear
            // 
            this.Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Clear.Image = ((System.Drawing.Image)(resources.GetObject("Clear.Image")));
            this.Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(23, 22);
            this.Clear.Tag = "Очистить чписок";
            // 
            // set_address
            // 
            this.set_address.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.set_address.Image = ((System.Drawing.Image)(resources.GetObject("set_address.Image")));
            this.set_address.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.set_address.Name = "set_address";
            this.set_address.Size = new System.Drawing.Size(23, 22);
            this.set_address.Tag = "Дать адрес";
            // 
            // undo
            // 
            this.undo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undo.Image = ((System.Drawing.Image)(resources.GetObject("undo.Image")));
            this.undo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undo.Name = "undo";
            this.undo.Size = new System.Drawing.Size(23, 22);
            this.undo.Tag = "Назад";
            // 
            // redo
            // 
            this.redo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redo.Image = ((System.Drawing.Image)(resources.GetObject("redo.Image")));
            this.redo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redo.Name = "redo";
            this.redo.Size = new System.Drawing.Size(23, 22);
            this.redo.Tag = "Вперёд";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(0, 22);
            // 
            // add_record
            // 
            this.add_record.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.add_record.Image = ((System.Drawing.Image)(resources.GetObject("add_record.Image")));
            this.add_record.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.add_record.Name = "add_record";
            this.add_record.Size = new System.Drawing.Size(23, 22);
            this.add_record.Tag = "Внести в реестр";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Tag = "Найти людей";
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Tag = "Очистить чписок";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Tag = "Дать адрес";
            // 
            // toolStripButton4
            // 
            this.toolStripButton4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton4.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton4.Image")));
            this.toolStripButton4.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton4.Name = "toolStripButton4";
            this.toolStripButton4.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton4.Tag = "Назад";
            // 
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton5.Image")));
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Tag = "Вперёд";
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(0, 22);
            // 
            // toolStripButton6
            // 
            this.toolStripButton6.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton6.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton6.Image")));
            this.toolStripButton6.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton6.Name = "toolStripButton6";
            this.toolStripButton6.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton6.Tag = "Внести в реестр";
            // 
            // JuridicRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabOrgMan);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "JuridicRegister";
            this.Size = new System.Drawing.Size(787, 302);
            this.tabOrgMan.ResumeLayout(false);
            this.Juridic.ResumeLayout(false);
            this.Juridic.PerformLayout();
            this.toolStripOrg.ResumeLayout(false);
            this.toolStripOrg.PerformLayout();
            this.Employes.ResumeLayout(false);
            this.panelADDEmployee.ResumeLayout(false);
            this.panelADDEmployee.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabOrgMan;
        private System.Windows.Forms.TabPage Juridic;
        private System.Windows.Forms.TabPage Employes;
        private System.Windows.Forms.ToolStripButton Select_men;
        private System.Windows.Forms.ToolStripButton Clear;
        private System.Windows.Forms.ToolStripButton set_address;
        private System.Windows.Forms.ToolStripButton undo;
        private System.Windows.Forms.ToolStripButton redo;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton add_record;
        private System.Windows.Forms.ToolStrip toolStripOrg;
        private System.Windows.Forms.ToolStripButton Select_jurs;
        private System.Windows.Forms.ToolStripButton ClearOrgs;
        private System.Windows.Forms.ToolStripButton set_orgadr;
        private System.Windows.Forms.ToolStripButton undorg;
        private System.Windows.Forms.ToolStripButton redorg;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripButton newOrg;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton4;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton toolStripButton6;
        private AddressRegister addressReg;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.ToolStripLabel toolStripLabel5;
        private System.Windows.Forms.ListBox JuridicList;
        private System.Windows.Forms.ListBox listEmployees;
        private System.Windows.Forms.Panel panelADDEmployee;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton Select_emp;
        private System.Windows.Forms.ToolStripButton Add_emp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Father;
        private System.Windows.Forms.TextBox Naming;
        private System.Windows.Forms.TextBox Family;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox Degree;
    }
}
