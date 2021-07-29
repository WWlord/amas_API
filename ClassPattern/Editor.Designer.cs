namespace ClassPattern
{
    partial class Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.tsCommands = new System.Windows.Forms.ToolStrip();
            this.tsbClear = new System.Windows.Forms.ToolStripButton();
            this.tbFind = new System.Windows.Forms.ToolStripTextBox();
            this.tsbFind = new System.Windows.Forms.ToolStripButton();
            this.tsbCut = new System.Windows.Forms.ToolStripButton();
            this.tsbPaste = new System.Windows.Forms.ToolStripButton();
            this.tsbUndo = new System.Windows.Forms.ToolStripButton();
            this.lbUndoaction = new System.Windows.Forms.ToolStripLabel();
            this.tsbRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbFonts = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.tsFontsize = new System.Windows.Forms.ToolStripComboBox();
            this.tsfontBolt = new System.Windows.Forms.ToolStripButton();
            this.tsfontitalic = new System.Windows.Forms.ToolStripButton();
            this.tsfontunderline = new System.Windows.Forms.ToolStripButton();
            this.rtEditor = new System.Windows.Forms.RichTextBox();
            this.tsCommands.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsCommands
            // 
            this.tsCommands.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbClear,
            this.tbFind,
            this.tsbFind,
            this.tsbCut,
            this.tsbPaste,
            this.tsbUndo,
            this.lbUndoaction,
            this.tsbRedo,
            this.toolStripLabel1,
            this.cbFonts,
            this.toolStripLabel2,
            this.tsFontsize,
            this.tsfontBolt,
            this.tsfontitalic,
            this.tsfontunderline});
            this.tsCommands.Location = new System.Drawing.Point(0, 0);
            this.tsCommands.Name = "tsCommands";
            this.tsCommands.Size = new System.Drawing.Size(692, 25);
            this.tsCommands.TabIndex = 0;
            this.tsCommands.Text = "toolStrip1";
            // 
            // tsbClear
            // 
            this.tsbClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbClear.Image = ((System.Drawing.Image)(resources.GetObject("tsbClear.Image")));
            this.tsbClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbClear.Name = "tsbClear";
            this.tsbClear.Size = new System.Drawing.Size(23, 22);
            this.tsbClear.Text = "Удалить текст";
            this.tsbClear.Click += new System.EventHandler(this.tsbClear_Click);
            // 
            // tbFind
            // 
            this.tbFind.Name = "tbFind";
            this.tbFind.Size = new System.Drawing.Size(100, 25);
            // 
            // tsbFind
            // 
            this.tsbFind.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbFind.Image = ((System.Drawing.Image)(resources.GetObject("tsbFind.Image")));
            this.tsbFind.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFind.Name = "tsbFind";
            this.tsbFind.Size = new System.Drawing.Size(23, 22);
            this.tsbFind.Text = "Найти";
            this.tsbFind.Click += new System.EventHandler(this.tsbFind_Click);
            // 
            // tsbCut
            // 
            this.tsbCut.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbCut.Image = ((System.Drawing.Image)(resources.GetObject("tsbCut.Image")));
            this.tsbCut.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCut.Name = "tsbCut";
            this.tsbCut.Size = new System.Drawing.Size(23, 22);
            this.tsbCut.Text = "Вырезать";
            this.tsbCut.Click += new System.EventHandler(this.tsbCut_Click);
            // 
            // tsbPaste
            // 
            this.tsbPaste.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbPaste.Image = ((System.Drawing.Image)(resources.GetObject("tsbPaste.Image")));
            this.tsbPaste.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPaste.Name = "tsbPaste";
            this.tsbPaste.Size = new System.Drawing.Size(23, 22);
            this.tsbPaste.Text = "Вставить";
            this.tsbPaste.Click += new System.EventHandler(this.tsbPaste_Click);
            // 
            // tsbUndo
            // 
            this.tsbUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbUndo.Image = ((System.Drawing.Image)(resources.GetObject("tsbUndo.Image")));
            this.tsbUndo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUndo.Name = "tsbUndo";
            this.tsbUndo.Size = new System.Drawing.Size(23, 22);
            this.tsbUndo.Text = "Отменить действие";
            this.tsbUndo.Click += new System.EventHandler(this.tsbUndo_Click);
            // 
            // lbUndoaction
            // 
            this.lbUndoaction.Name = "lbUndoaction";
            this.lbUndoaction.Size = new System.Drawing.Size(28, 22);
            this.lbUndoaction.Text = "       ";
            // 
            // tsbRedo
            // 
            this.tsbRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRedo.Image = ((System.Drawing.Image)(resources.GetObject("tsbRedo.Image")));
            this.tsbRedo.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbRedo.Name = "tsbRedo";
            this.tsbRedo.Size = new System.Drawing.Size(23, 22);
            this.tsbRedo.Text = "Возвратить действие";
            this.tsbRedo.Click += new System.EventHandler(this.tsbRedo_Click);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(43, 22);
            this.toolStripLabel1.Text = "Шрифт";
            // 
            // cbFonts
            // 
            this.cbFonts.Name = "cbFonts";
            this.cbFonts.Size = new System.Drawing.Size(121, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(85, 22);
            this.toolStripLabel2.Text = "Размер шрифта";
            // 
            // tsFontsize
            // 
            this.tsFontsize.Items.AddRange(new object[] {
            "8",
            "9",
            "10",
            "12",
            "14",
            "16",
            "18",
            "20",
            "24",
            "26",
            "32",
            "36",
            "42",
            "48",
            "56",
            "72",
            "96"});
            this.tsFontsize.Name = "tsFontsize";
            this.tsFontsize.Size = new System.Drawing.Size(75, 25);
            this.tsFontsize.Text = "12";
            // 
            // tsfontBolt
            // 
            this.tsfontBolt.CheckOnClick = true;
            this.tsfontBolt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsfontBolt.Image = ((System.Drawing.Image)(resources.GetObject("tsfontBolt.Image")));
            this.tsfontBolt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsfontBolt.Name = "tsfontBolt";
            this.tsfontBolt.Size = new System.Drawing.Size(23, 22);
            this.tsfontBolt.Text = "Жирный шрифт";
            this.tsfontBolt.Click += new System.EventHandler(this.tsfontBolt_Click);
            // 
            // tsfontitalic
            // 
            this.tsfontitalic.CheckOnClick = true;
            this.tsfontitalic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsfontitalic.Image = ((System.Drawing.Image)(resources.GetObject("tsfontitalic.Image")));
            this.tsfontitalic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsfontitalic.Name = "tsfontitalic";
            this.tsfontitalic.Size = new System.Drawing.Size(23, 22);
            this.tsfontitalic.Text = "Курсив";
            this.tsfontitalic.Click += new System.EventHandler(this.tsfontitalic_Click);
            // 
            // tsfontunderline
            // 
            this.tsfontunderline.CheckOnClick = true;
            this.tsfontunderline.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsfontunderline.Image = ((System.Drawing.Image)(resources.GetObject("tsfontunderline.Image")));
            this.tsfontunderline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsfontunderline.Name = "tsfontunderline";
            this.tsfontunderline.Size = new System.Drawing.Size(23, 22);
            this.tsfontunderline.Text = "Подчёркивание";
            this.tsfontunderline.Click += new System.EventHandler(this.tsfontunderline_Click);
            // 
            // rtEditor
            // 
            this.rtEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtEditor.Location = new System.Drawing.Point(0, 25);
            this.rtEditor.Name = "rtEditor";
            this.rtEditor.Size = new System.Drawing.Size(692, 457);
            this.rtEditor.TabIndex = 1;
            this.rtEditor.Text = "";
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rtEditor);
            this.Controls.Add(this.tsCommands);
            this.Name = "Editor";
            this.Size = new System.Drawing.Size(692, 482);
            this.tsCommands.ResumeLayout(false);
            this.tsCommands.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsCommands;
        private System.Windows.Forms.RichTextBox rtEditor;
        private System.Windows.Forms.ToolStripButton tsbClear;
        private System.Windows.Forms.ToolStripTextBox tbFind;
        private System.Windows.Forms.ToolStripButton tsbFind;
        private System.Windows.Forms.ToolStripButton tsbCut;
        private System.Windows.Forms.ToolStripButton tsbPaste;
        private System.Windows.Forms.ToolStripButton tsbUndo;
        private System.Windows.Forms.ToolStripButton tsbRedo;
        private System.Windows.Forms.ToolStripComboBox cbFonts;
        private System.Windows.Forms.ToolStripLabel lbUndoaction;
        private System.Windows.Forms.ToolStripComboBox tsFontsize;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton tsfontBolt;
        private System.Windows.Forms.ToolStripButton tsfontitalic;
        private System.Windows.Forms.ToolStripButton tsfontunderline;
    }
}
