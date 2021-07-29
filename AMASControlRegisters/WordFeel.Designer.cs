namespace AMASControlRegisters
{
    partial class WordFeel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WordFeel));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.tsLoadContent = new System.Windows.Forms.ToolStripButton();
            this.tsLAssignNumber = new System.Windows.Forms.ToolStripButton();
            this.tsSaveDocument = new System.Windows.Forms.ToolStripButton();
            this.tsCanselDocument = new System.Windows.Forms.ToolStripButton();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.webBrowser1 = new System.Windows.Forms.WebBrowser();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsLoadContent,
            this.tsLAssignNumber,
            this.tsSaveDocument,
            this.tsCanselDocument});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(559, 25);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // tsLoadContent
            // 
            this.tsLoadContent.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsLoadContent.Image = global::AMASControlRegisters.Properties.Resources.CLIP03;
            this.tsLoadContent.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsLoadContent.Name = "tsLoadContent";
            this.tsLoadContent.Size = new System.Drawing.Size(23, 22);
            this.tsLoadContent.Text = "Загрузить содержимое документа";
            this.tsLoadContent.Click += new System.EventHandler(this.tsLoadContent_Click);
            // 
            // tsLAssignNumber
            // 
            this.tsLAssignNumber.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsLAssignNumber.Image = global::AMASControlRegisters.Properties.Resources.LABELS;
            this.tsLAssignNumber.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsLAssignNumber.Name = "tsLAssignNumber";
            this.tsLAssignNumber.Size = new System.Drawing.Size(23, 22);
            this.tsLAssignNumber.Text = "Присвоить регистрационный номер";
            // 
            // tsSaveDocument
            // 
            this.tsSaveDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsSaveDocument.Image = global::AMASControlRegisters.Properties.Resources.DISK03;
            this.tsSaveDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsSaveDocument.Name = "tsSaveDocument";
            this.tsSaveDocument.Size = new System.Drawing.Size(23, 22);
            this.tsSaveDocument.Text = "Сохранить документ";
            // 
            // tsCanselDocument
            // 
            this.tsCanselDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsCanselDocument.Image = global::AMASControlRegisters.Properties.Resources.MSGBOX01;
            this.tsCanselDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsCanselDocument.Name = "tsCanselDocument";
            this.tsCanselDocument.Size = new System.Drawing.Size(23, 22);
            this.tsCanselDocument.Text = "Не сохранять документ";
            // 
            // richTextBox1
            // 
            this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.richTextBox1.ForeColor = System.Drawing.SystemColors.Desktop;
            this.richTextBox1.Location = new System.Drawing.Point(0, 0);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(559, 298);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
            this.richTextBox1.TextChanged += new System.EventHandler(this.richTextBox1_TextChanged);
            // 
            // webBrowser1
            // 
            this.webBrowser1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowser1.Location = new System.Drawing.Point(0, 0);
            this.webBrowser1.MinimumSize = new System.Drawing.Size(20, 20);
            this.webBrowser1.Name = "webBrowser1";
            this.webBrowser1.Size = new System.Drawing.Size(559, 298);
            this.webBrowser1.TabIndex = 3;
            // 
            // WordFeel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.webBrowser1);
            this.Controls.Add(this.richTextBox1);
            this.Name = "WordFeel";
            this.Size = new System.Drawing.Size(559, 298);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton tsLoadContent;
        private System.Windows.Forms.ToolStripButton tsSaveDocument;
        private System.Windows.Forms.ToolStripButton tsLAssignNumber;
        private System.Windows.Forms.ToolStripButton tsCanselDocument;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.WebBrowser webBrowser1;
    }
}
