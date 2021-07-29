namespace AMASControlRegisters
{
    partial class ContragentRegister
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ContragentRegister));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.peopleRegister = new AMASControlRegisters.PeopleRegister();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.JuridicRegister = new AMASControlRegisters.JuridicRegister();
            this.lvContragent = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.óäàëèòüÊîíòğàToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tabControl1.Location = new System.Drawing.Point(0, 122);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(605, 272);
            this.tabControl1.TabIndex = 0;
            // 
            // peopleRegister
            // 
            this.peopleRegister.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.peopleRegister.Location = new System.Drawing.Point(3, 6);
            this.peopleRegister.Name = "peopleRegister";
            this.peopleRegister.Size = new System.Drawing.Size(591, 237);
            this.peopleRegister.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.peopleRegister);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(597, 246);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Íàñåëåíèå";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // JuridicRegister
            // 
            this.JuridicRegister.Location = new System.Drawing.Point(3, 0);
            this.JuridicRegister.Name = "JuridicRegister";
            this.JuridicRegister.Size = new System.Drawing.Size(590, 247);
            this.JuridicRegister.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.JuridicRegister);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(597, 246);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Îğãàíèçàöèè";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lvContragent
            // 
            this.lvContragent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvContragent.Location = new System.Drawing.Point(0, 0);
            this.lvContragent.Name = "lvContragent";
            this.lvContragent.Size = new System.Drawing.Size(605, 122);
            this.lvContragent.TabIndex = 1;
            this.lvContragent.UseCompatibleStateImageBehavior = false;
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "NET12.ICO");
            this.imageList1.Images.SetKeyName(1, "MISC26.ICO");
            this.imageList1.Images.SetKeyName(2, "PHONE06.ICO");
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.óäàëèòüÊîíòğàToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(198, 26);
            // 
            // óäàëèòüÊîíòğàToolStripMenuItem
            // 
            this.óäàëèòüÊîíòğàToolStripMenuItem.Name = "óäàëèòüÊîíòğàToolStripMenuItem";
            this.óäàëèòüÊîíòğàToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.óäàëèòüÊîíòğàToolStripMenuItem.Text = "Óäàëèòü êîíòğàãåíòà";
            this.óäàëèòüÊîíòğàToolStripMenuItem.Click += new System.EventHandler(this.óäàëèòüÊîíòğàToolStripMenuItem_Click);
            // 
            // ContragentRegister
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lvContragent);
            this.Controls.Add(this.tabControl1);
            this.Name = "ContragentRegister";
            this.Size = new System.Drawing.Size(605, 394);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private PeopleRegister peopleRegister;
        private System.Windows.Forms.TabPage tabPage2;
        private JuridicRegister JuridicRegister;
        private System.Windows.Forms.ListView lvContragent;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem óäàëèòüÊîíòğàToolStripMenuItem;
    }
}
