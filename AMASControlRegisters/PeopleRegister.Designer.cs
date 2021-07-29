namespace AMASControlRegisters
{
    partial class PeopleRegister
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PeopleRegister));
            this.toolStripMan = new System.Windows.Forms.ToolStrip();
            this.Select_men = new System.Windows.Forms.ToolStripButton();
            this.Clear = new System.Windows.Forms.ToolStripButton();
            this.set_address = new System.Windows.Forms.ToolStripButton();
            this.undo = new System.Windows.Forms.ToolStripButton();
            this.redo = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.add_record = new System.Windows.Forms.ToolStripButton();
            this.panelAddr = new System.Windows.Forms.Panel();
            this.addressReg = new AMASControlRegisters.AddressRegister();
            this.panelView = new System.Windows.Forms.Panel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.Father = new System.Windows.Forms.TextBox();
            this.Naming = new System.Windows.Forms.TextBox();
            this.Family = new System.Windows.Forms.TextBox();
            this.PeopleList = new System.Windows.Forms.ListBox();
            this.toolStripMan.SuspendLayout();
            this.panelAddr.SuspendLayout();
            this.panelView.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripMan
            // 
            this.toolStripMan.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStripMan.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Select_men,
            this.Clear,
            this.set_address,
            this.undo,
            this.redo,
            this.toolStripLabel1,
            this.add_record});
            this.toolStripMan.Location = new System.Drawing.Point(0, 189);
            this.toolStripMan.Name = "toolStripMan";
            this.toolStripMan.Size = new System.Drawing.Size(259, 25);
            this.toolStripMan.TabIndex = 5;
            this.toolStripMan.Text = "toolStrip1";
            // 
            // Select_men
            // 
            this.Select_men.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Select_men.Image = ((System.Drawing.Image)(resources.GetObject("Select_men.Image")));
            this.Select_men.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Select_men.Name = "Select_men";
            this.Select_men.Size = new System.Drawing.Size(23, 22);
            this.Select_men.Tag = "Найти людей";
            this.Select_men.ToolTipText = "Поиск";
            this.Select_men.Click += new System.EventHandler(this.Select_Click);
            // 
            // Clear
            // 
            this.Clear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Clear.Image = ((System.Drawing.Image)(resources.GetObject("Clear.Image")));
            this.Clear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Clear.Name = "Clear";
            this.Clear.Size = new System.Drawing.Size(23, 22);
            this.Clear.Tag = "Очистить чписок";
            this.Clear.ToolTipText = "Очистить";
            this.Clear.Click += new System.EventHandler(this.Clear_Click);
            // 
            // set_address
            // 
            this.set_address.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.set_address.Image = ((System.Drawing.Image)(resources.GetObject("set_address.Image")));
            this.set_address.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.set_address.Name = "set_address";
            this.set_address.Size = new System.Drawing.Size(23, 22);
            this.set_address.Tag = "Дать адрес";
            this.set_address.ToolTipText = "Сменить адрес";
            this.set_address.Click += new System.EventHandler(this.set_address_Click);
            // 
            // undo
            // 
            this.undo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.undo.Image = ((System.Drawing.Image)(resources.GetObject("undo.Image")));
            this.undo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.undo.Name = "undo";
            this.undo.Size = new System.Drawing.Size(23, 22);
            this.undo.Tag = "Назад";
            this.undo.ToolTipText = "Предыдущий просмотр";
            this.undo.Click += new System.EventHandler(this.undo_Click);
            // 
            // redo
            // 
            this.redo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.redo.Image = ((System.Drawing.Image)(resources.GetObject("redo.Image")));
            this.redo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.redo.Name = "redo";
            this.redo.Size = new System.Drawing.Size(23, 22);
            this.redo.Tag = "Вперёд";
            this.redo.ToolTipText = "Последующий просмотр";
            this.redo.Click += new System.EventHandler(this.redo_Click);
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
            this.add_record.ToolTipText = "Внести в реестр";
            this.add_record.Click += new System.EventHandler(this.add_record_Click);
            // 
            // panelAddr
            // 
            this.panelAddr.Controls.Add(this.addressReg);
            this.panelAddr.Controls.Add(this.toolStripMan);
            this.panelAddr.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelAddr.Location = new System.Drawing.Point(331, 0);
            this.panelAddr.Name = "panelAddr";
            this.panelAddr.Size = new System.Drawing.Size(259, 214);
            this.panelAddr.TabIndex = 9;
            // 
            // addressReg
            // 
            this.addressReg.Dock = System.Windows.Forms.DockStyle.Top;
            this.addressReg.Location = new System.Drawing.Point(0, 0);
            this.addressReg.Name = "addressReg";
            this.addressReg.Size = new System.Drawing.Size(259, 187);
            this.addressReg.TabIndex = 5;
            // 
            // panelView
            // 
            this.panelView.Controls.Add(this.panel1);
            this.panelView.Controls.Add(this.PeopleList);
            this.panelView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelView.Location = new System.Drawing.Point(0, 0);
            this.panelView.Name = "panelView";
            this.panelView.Size = new System.Drawing.Size(331, 214);
            this.panelView.TabIndex = 10;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.Father);
            this.panel1.Controls.Add(this.Naming);
            this.panel1.Controls.Add(this.Family);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 135);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(331, 79);
            this.panel1.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label3.Location = new System.Drawing.Point(7, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(62, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Отчество";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label2.Location = new System.Drawing.Point(7, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(32, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Имя";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(7, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "Фамилия";
            // 
            // Father
            // 
            this.Father.Location = new System.Drawing.Point(86, 54);
            this.Father.Name = "Father";
            this.Father.Size = new System.Drawing.Size(239, 20);
            this.Father.TabIndex = 18;
            // 
            // Naming
            // 
            this.Naming.Location = new System.Drawing.Point(86, 31);
            this.Naming.Name = "Naming";
            this.Naming.Size = new System.Drawing.Size(239, 20);
            this.Naming.TabIndex = 17;
            // 
            // Family
            // 
            this.Family.Location = new System.Drawing.Point(86, 8);
            this.Family.Name = "Family";
            this.Family.Size = new System.Drawing.Size(239, 20);
            this.Family.TabIndex = 16;
            // 
            // PeopleList
            // 
            this.PeopleList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PeopleList.Font = new System.Drawing.Font("Times New Roman", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.PeopleList.FormattingEnabled = true;
            this.PeopleList.ItemHeight = 15;
            this.PeopleList.Location = new System.Drawing.Point(0, 0);
            this.PeopleList.Name = "PeopleList";
            this.PeopleList.Size = new System.Drawing.Size(331, 214);
            this.PeopleList.TabIndex = 9;
            // 
            // PeopleRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelView);
            this.Controls.Add(this.panelAddr);
            this.Name = "PeopleRegister";
            this.Size = new System.Drawing.Size(590, 214);
            this.toolStripMan.ResumeLayout(false);
            this.toolStripMan.PerformLayout();
            this.panelAddr.ResumeLayout(false);
            this.panelAddr.PerformLayout();
            this.panelView.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStripMan;
        private System.Windows.Forms.ToolStripButton Select_men;
        private System.Windows.Forms.ToolStripButton Clear;
        private System.Windows.Forms.ToolStripButton set_address;
        private System.Windows.Forms.ToolStripButton undo;
        private System.Windows.Forms.ToolStripButton redo;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton add_record;
        private System.Windows.Forms.Panel panelAddr;
        private System.Windows.Forms.Panel panelView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Father;
        private System.Windows.Forms.TextBox Naming;
        private System.Windows.Forms.TextBox Family;
        private System.Windows.Forms.ListBox PeopleList;
        private AddressRegister addressReg;
    }
}
