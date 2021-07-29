namespace AMASControlRegisters
{
    partial class Document_Viewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Document_Viewer));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabDocument = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.OdcBrowser = new System.Windows.Forms.WebBrowser();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabMetadata = new System.Windows.Forms.TabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.listView1 = new System.Windows.Forms.ListView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.fdInsert = new System.Windows.Forms.OpenFileDialog();
            this.ilIcons = new System.Windows.Forms.ImageList(this.components);
            this.imageFS = new System.Windows.Forms.ImageList(this.components);
            this.panelLockDoc = new System.Windows.Forms.Panel();
            this.listBoxSteps = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabDocument.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabMetadata.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panelLockDoc.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabDocument);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabMetadata);
            this.splitContainer1.Size = new System.Drawing.Size(572, 447);
            this.splitContainer1.SplitterDistance = 289;
            this.splitContainer1.SplitterWidth = 5;
            this.splitContainer1.TabIndex = 0;
            // 
            // tabDocument
            // 
            this.tabDocument.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tabDocument.Controls.Add(this.tabPage1);
            this.tabDocument.Controls.Add(this.tabPage2);
            this.tabDocument.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabDocument.Location = new System.Drawing.Point(0, 0);
            this.tabDocument.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabDocument.Name = "tabDocument";
            this.tabDocument.SelectedIndex = 0;
            this.tabDocument.Size = new System.Drawing.Size(572, 289);
            this.tabDocument.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.OdcBrowser);
            this.tabPage1.Location = new System.Drawing.Point(4, 4);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage1.Size = new System.Drawing.Size(564, 260);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // OdcBrowser
            // 
            this.OdcBrowser.Dock = System.Windows.Forms.DockStyle.Fill;
            this.OdcBrowser.Location = new System.Drawing.Point(4, 4);
            this.OdcBrowser.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.OdcBrowser.MinimumSize = new System.Drawing.Size(27, 25);
            this.OdcBrowser.Name = "OdcBrowser";
            this.OdcBrowser.Size = new System.Drawing.Size(556, 252);
            this.OdcBrowser.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 4);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage2.Size = new System.Drawing.Size(564, 260);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabMetadata
            // 
            this.tabMetadata.Controls.Add(this.tabPage3);
            this.tabMetadata.Controls.Add(this.tabPage4);
            this.tabMetadata.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabMetadata.Location = new System.Drawing.Point(0, 0);
            this.tabMetadata.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabMetadata.Name = "tabMetadata";
            this.tabMetadata.SelectedIndex = 0;
            this.tabMetadata.Size = new System.Drawing.Size(572, 153);
            this.tabMetadata.TabIndex = 1;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.listView1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage3.Size = new System.Drawing.Size(564, 124);
            this.tabPage3.TabIndex = 0;
            this.tabPage3.Text = "tabPage3";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // listView1
            // 
            this.listView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listView1.Location = new System.Drawing.Point(4, 4);
            this.listView1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(556, 116);
            this.listView1.TabIndex = 0;
            this.listView1.UseCompatibleStateImageBehavior = false;
            // 
            // tabPage4
            // 
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tabPage4.Size = new System.Drawing.Size(564, 124);
            this.tabPage4.TabIndex = 1;
            this.tabPage4.Text = "tabPage4";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // ilIcons
            // 
            this.ilIcons.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.ilIcons.ImageSize = new System.Drawing.Size(16, 16);
            this.ilIcons.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // imageFS
            // 
            this.imageFS.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageFS.ImageStream")));
            this.imageFS.TransparentColor = System.Drawing.Color.Transparent;
            this.imageFS.Images.SetKeyName(0, "CDROM01.ICO");
            this.imageFS.Images.SetKeyName(1, "FOLDER03.ICO");
            this.imageFS.Images.SetKeyName(2, "CLIP03.ICO");
            this.imageFS.Images.SetKeyName(3, "CHECKMRK.ICO");
            // 
            // panelLockDoc
            // 
            this.panelLockDoc.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.panelLockDoc.Controls.Add(this.listBoxSteps);
            this.panelLockDoc.Controls.Add(this.label1);
            this.panelLockDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelLockDoc.Location = new System.Drawing.Point(0, 0);
            this.panelLockDoc.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panelLockDoc.Name = "panelLockDoc";
            this.panelLockDoc.Size = new System.Drawing.Size(572, 447);
            this.panelLockDoc.TabIndex = 1;
            this.panelLockDoc.Visible = false;
            // 
            // listBoxSteps
            // 
            this.listBoxSteps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.listBoxSteps.FormattingEnabled = true;
            this.listBoxSteps.ItemHeight = 16;
            this.listBoxSteps.Location = new System.Drawing.Point(9, 74);
            this.listBoxSteps.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.listBoxSteps.Name = "listBoxSteps";
            this.listBoxSteps.Size = new System.Drawing.Size(549, 272);
            this.listBoxSteps.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(13, 23);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Загрузка документа";
            // 
            // Document_Viewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelLockDoc);
            this.Controls.Add(this.splitContainer1);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "Document_Viewer";
            this.Size = new System.Drawing.Size(572, 447);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabDocument.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabMetadata.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panelLockDoc.ResumeLayout(false);
            this.panelLockDoc.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TabControl tabDocument;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.WebBrowser OdcBrowser;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabControl tabMetadata;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.OpenFileDialog fdInsert;
        private System.Windows.Forms.ImageList ilIcons;
        private System.Windows.Forms.ImageList imageFS;
        private System.Windows.Forms.Panel panelLockDoc;
        private System.Windows.Forms.ListBox listBoxSteps;
        private System.Windows.Forms.Label label1;

    }
}
