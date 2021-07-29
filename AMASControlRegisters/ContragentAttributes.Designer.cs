namespace AMASControlRegisters
{
    partial class ContragentAttributes
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
            this.tcAttributes = new System.Windows.Forms.TabControl();
            this.tpPostIphones = new System.Windows.Forms.TabPage();
            this.panelContragent = new System.Windows.Forms.Panel();
            this.splitContainerCA = new System.Windows.Forms.SplitContainer();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dataGridViewPhone = new System.Windows.Forms.DataGridView();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dataGridViewEmail = new System.Windows.Forms.DataGridView();
            this.cmsPhone = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.удалитьТелефонToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsEMail = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tcAttributes.SuspendLayout();
            this.tpPostIphones.SuspendLayout();
            this.panelContragent.SuspendLayout();
            this.splitContainerCA.Panel1.SuspendLayout();
            this.splitContainerCA.Panel2.SuspendLayout();
            this.splitContainerCA.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPhone)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEmail)).BeginInit();
            this.cmsPhone.SuspendLayout();
            this.cmsEMail.SuspendLayout();
            this.SuspendLayout();
            // 
            // tcAttributes
            // 
            this.tcAttributes.Controls.Add(this.tpPostIphones);
            this.tcAttributes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcAttributes.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.tcAttributes.Location = new System.Drawing.Point(0, 0);
            this.tcAttributes.Name = "tcAttributes";
            this.tcAttributes.SelectedIndex = 0;
            this.tcAttributes.Size = new System.Drawing.Size(481, 292);
            this.tcAttributes.TabIndex = 0;
            // 
            // tpPostIphones
            // 
            this.tpPostIphones.Controls.Add(this.panelContragent);
            this.tpPostIphones.Location = new System.Drawing.Point(4, 25);
            this.tpPostIphones.Name = "tpPostIphones";
            this.tpPostIphones.Padding = new System.Windows.Forms.Padding(3);
            this.tpPostIphones.Size = new System.Drawing.Size(473, 263);
            this.tpPostIphones.TabIndex = 0;
            this.tpPostIphones.Text = "Почта и телефоны";
            this.tpPostIphones.UseVisualStyleBackColor = true;
            // 
            // panelContragent
            // 
            this.panelContragent.Controls.Add(this.splitContainerCA);
            this.panelContragent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContragent.Location = new System.Drawing.Point(3, 3);
            this.panelContragent.Name = "panelContragent";
            this.panelContragent.Size = new System.Drawing.Size(467, 257);
            this.panelContragent.TabIndex = 3;
            // 
            // splitContainerCA
            // 
            this.splitContainerCA.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainerCA.Location = new System.Drawing.Point(0, 0);
            this.splitContainerCA.Name = "splitContainerCA";
            // 
            // splitContainerCA.Panel1
            // 
            this.splitContainerCA.Panel1.Controls.Add(this.groupBox1);
            // 
            // splitContainerCA.Panel2
            // 
            this.splitContainerCA.Panel2.Controls.Add(this.groupBox2);
            this.splitContainerCA.Size = new System.Drawing.Size(467, 257);
            this.splitContainerCA.SplitterDistance = 215;
            this.splitContainerCA.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.dataGridViewPhone);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(215, 257);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Телефоны";
            // 
            // dataGridViewPhone
            // 
            this.dataGridViewPhone.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewPhone.ContextMenuStrip = this.cmsPhone;
            this.dataGridViewPhone.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewPhone.Location = new System.Drawing.Point(3, 18);
            this.dataGridViewPhone.Name = "dataGridViewPhone";
            this.dataGridViewPhone.Size = new System.Drawing.Size(209, 236);
            this.dataGridViewPhone.TabIndex = 3;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dataGridViewEmail);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(0, 0);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(248, 257);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "E-mail";
            // 
            // dataGridViewEmail
            // 
            this.dataGridViewEmail.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEmail.ContextMenuStrip = this.cmsEMail;
            this.dataGridViewEmail.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridViewEmail.Location = new System.Drawing.Point(3, 18);
            this.dataGridViewEmail.Name = "dataGridViewEmail";
            this.dataGridViewEmail.Size = new System.Drawing.Size(242, 236);
            this.dataGridViewEmail.TabIndex = 3;
            // 
            // cmsPhone
            // 
            this.cmsPhone.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.удалитьТелефонToolStripMenuItem});
            this.cmsPhone.Name = "cmsPhone";
            this.cmsPhone.Size = new System.Drawing.Size(177, 26);
            // 
            // удалитьТелефонToolStripMenuItem
            // 
            this.удалитьТелефонToolStripMenuItem.Name = "удалитьТелефонToolStripMenuItem";
            this.удалитьТелефонToolStripMenuItem.Size = new System.Drawing.Size(176, 22);
            this.удалитьТелефонToolStripMenuItem.Text = "Удалить телефон";
            this.удалитьТелефонToolStripMenuItem.Click += new System.EventHandler(this.удалитьТелефонToolStripMenuItem_Click_1);
            // 
            // cmsEMail
            // 
            this.cmsEMail.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1});
            this.cmsEMail.Name = "cmsPhone";
            this.cmsEMail.Size = new System.Drawing.Size(213, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(212, 22);
            this.toolStripMenuItem1.Text = "Удалить почтовый ящик";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click_1);
            // 
            // ContragentAttributes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tcAttributes);
            this.Name = "ContragentAttributes";
            this.Size = new System.Drawing.Size(481, 292);
            this.tcAttributes.ResumeLayout(false);
            this.tpPostIphones.ResumeLayout(false);
            this.panelContragent.ResumeLayout(false);
            this.splitContainerCA.Panel1.ResumeLayout(false);
            this.splitContainerCA.Panel2.ResumeLayout(false);
            this.splitContainerCA.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewPhone)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEmail)).EndInit();
            this.cmsPhone.ResumeLayout(false);
            this.cmsEMail.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tcAttributes;
        private System.Windows.Forms.TabPage tpPostIphones;
        private System.Windows.Forms.Panel panelContragent;
        private System.Windows.Forms.SplitContainer splitContainerCA;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.DataGridView dataGridViewPhone;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.DataGridView dataGridViewEmail;
        private System.Windows.Forms.ContextMenuStrip cmsPhone;
        private System.Windows.Forms.ToolStripMenuItem удалитьТелефонToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsEMail;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
    }
}
