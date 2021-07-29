namespace AMASControlRegisters
{
    partial class UCDocsTree
    {
        /// <summary> 
        /// Требуется переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Обязательный метод для поддержки конструктора - не изменяйте 
        /// содержимое данного метода при помощи редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UCDocsTree));
            this.imageListFolders = new System.Windows.Forms.ImageList(this.components);
            this.toolStripDocTree = new System.Windows.Forms.ToolStrip();
            this.tsbAnswer = new System.Windows.Forms.ToolStripButton();
            this.tsbNewDoc = new System.Windows.Forms.ToolStripButton();
            this.tsbCalendar = new System.Windows.Forms.ToolStripComboBox();
            this.tsbComment = new System.Windows.Forms.ToolStripTextBox();
            this.tsbExecute = new System.Windows.Forms.ToolStripButton();
            this.tbsVizing = new System.Windows.Forms.ToolStripButton();
            this.tsbNews = new System.Windows.Forms.ToolStripButton();
            this.FuelBar = new System.Windows.Forms.ToolStripProgressBar();
            this.treeViewDocs = new System.Windows.Forms.TreeView();
            this.panelStructure = new System.Windows.Forms.Panel();
            this.treeViewStructure = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDocTree.SuspendLayout();
            this.panelStructure.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // imageListFolders
            // 
            this.imageListFolders.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageListFolders.ImageStream")));
            this.imageListFolders.TransparentColor = System.Drawing.Color.Transparent;
            this.imageListFolders.Images.SetKeyName(0, "Book_Green");
            this.imageListFolders.Images.SetKeyName(1, "112_Tick_Grey");
            this.imageListFolders.Images.SetKeyName(2, "Reminder");
            this.imageListFolders.Images.SetKeyName(3, "clock");
            this.imageListFolders.Images.SetKeyName(4, "envelope");
            this.imageListFolders.Images.SetKeyName(5, "Flag_Yellow");
            this.imageListFolders.Images.SetKeyName(6, "Lightbulb");
            this.imageListFolders.Images.SetKeyName(7, "Favorites");
            this.imageListFolders.Images.SetKeyName(8, "Folder_Back");
            this.imageListFolders.Images.SetKeyName(9, "Folder_stuffed");
            this.imageListFolders.Images.SetKeyName(10, "Task");
            this.imageListFolders.Images.SetKeyName(11, "SecurityLock");
            // 
            // toolStripDocTree
            // 
            this.toolStripDocTree.BackColor = System.Drawing.Color.MidnightBlue;
            this.toolStripDocTree.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAnswer,
            this.tsbNewDoc,
            this.tsbCalendar,
            this.tsbComment,
            this.tsbExecute,
            this.tbsVizing,
            this.tsbNews,
            this.FuelBar});
            this.toolStripDocTree.Location = new System.Drawing.Point(0, 0);
            this.toolStripDocTree.Name = "toolStripDocTree";
            this.toolStripDocTree.Size = new System.Drawing.Size(496, 28);
            this.toolStripDocTree.TabIndex = 2;
            // 
            // tsbAnswer
            // 
            this.tsbAnswer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbAnswer.Image = global::AMASControlRegisters.Properties.Resources.CLIP07;
            this.tsbAnswer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAnswer.Name = "tsbAnswer";
            this.tsbAnswer.Size = new System.Drawing.Size(23, 25);
            this.tsbAnswer.Text = "Ответить";
            this.tsbAnswer.Click += new System.EventHandler(this.tsbAnswer_Click);
            // 
            // tsbNewDoc
            // 
            this.tsbNewDoc.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNewDoc.Image = global::AMASControlRegisters.Properties.Resources.PLUS;
            this.tsbNewDoc.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNewDoc.Name = "tsbNewDoc";
            this.tsbNewDoc.Size = new System.Drawing.Size(23, 25);
            this.tsbNewDoc.Text = "Новый документ";
            this.tsbNewDoc.Click += new System.EventHandler(this.tsbNewDoc_Click);
            // 
            // tsbCalendar
            // 
            this.tsbCalendar.AutoCompleteCustomSource.AddRange(new string[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"});
            this.tsbCalendar.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24",
            "25",
            "26",
            "27",
            "28",
            "29",
            "30"});
            this.tsbCalendar.Name = "tsbCalendar";
            this.tsbCalendar.Size = new System.Drawing.Size(121, 28);
            // 
            // tsbComment
            // 
            this.tsbComment.AutoSize = false;
            this.tsbComment.Name = "tsbComment";
            this.tsbComment.Size = new System.Drawing.Size(100, 28);
            // 
            // tsbExecute
            // 
            this.tsbExecute.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbExecute.Image = global::AMASControlRegisters.Properties.Resources.Utility_VBScript;
            this.tsbExecute.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbExecute.Name = "tsbExecute";
            this.tsbExecute.Size = new System.Drawing.Size(23, 25);
            this.tsbExecute.Text = " На исполнение";
            this.tsbExecute.Click += new System.EventHandler(this.tsbExecute_Click);
            // 
            // tbsVizing
            // 
            this.tbsVizing.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tbsVizing.Image = global::AMASControlRegisters.Properties.Resources.Utility_VBScript;
            this.tbsVizing.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tbsVizing.Name = "tbsVizing";
            this.tbsVizing.Size = new System.Drawing.Size(23, 25);
            this.tbsVizing.Text = "На визирование";
            this.tbsVizing.Click += new System.EventHandler(this.tbsVizing_Click);
            // 
            // tsbNews
            // 
            this.tsbNews.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbNews.Image = global::AMASControlRegisters.Properties.Resources.SingleMessage;
            this.tsbNews.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbNews.Name = "tsbNews";
            this.tsbNews.Size = new System.Drawing.Size(23, 25);
            this.tsbNews.Text = "К сведению";
            this.tsbNews.Click += new System.EventHandler(this.tsbNews_Click);
            // 
            // FuelBar
            // 
            this.FuelBar.Name = "FuelBar";
            this.FuelBar.Size = new System.Drawing.Size(100, 25);
            // 
            // treeViewDocs
            // 
            this.treeViewDocs.BackColor = System.Drawing.Color.LightSteelBlue;
            this.treeViewDocs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewDocs.ImageIndex = 0;
            this.treeViewDocs.ImageList = this.imageListFolders;
            this.treeViewDocs.Location = new System.Drawing.Point(0, 28);
            this.treeViewDocs.Name = "treeViewDocs";
            this.treeViewDocs.SelectedImageIndex = 5;
            this.treeViewDocs.Size = new System.Drawing.Size(496, 385);
            this.treeViewDocs.TabIndex = 4;
            // 
            // panelStructure
            // 
            this.panelStructure.Controls.Add(this.toolStrip1);
            this.panelStructure.Controls.Add(this.treeViewStructure);
            this.panelStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStructure.Location = new System.Drawing.Point(0, 28);
            this.panelStructure.Name = "panelStructure";
            this.panelStructure.Size = new System.Drawing.Size(496, 385);
            this.panelStructure.TabIndex = 5;
            // 
            // treeViewStructure
            // 
            this.treeViewStructure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeViewStructure.Location = new System.Drawing.Point(0, 0);
            this.treeViewStructure.Name = "treeViewStructure";
            this.treeViewStructure.Size = new System.Drawing.Size(496, 385);
            this.treeViewStructure.TabIndex = 6;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(496, 25);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::AMASControlRegisters.Properties.Resources.SingleMessage;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "Назначить";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // UCDocsTree
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.panelStructure);
            this.Controls.Add(this.treeViewDocs);
            this.Controls.Add(this.toolStripDocTree);
            this.Name = "UCDocsTree";
            this.Size = new System.Drawing.Size(496, 413);
            this.toolStripDocTree.ResumeLayout(false);
            this.toolStripDocTree.PerformLayout();
            this.panelStructure.ResumeLayout(false);
            this.panelStructure.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ImageList imageListFolders;
        private System.Windows.Forms.ToolStrip toolStripDocTree;
        private System.Windows.Forms.ToolStripButton tsbAnswer;
        private System.Windows.Forms.ToolStripButton tsbNewDoc;
        private System.Windows.Forms.ToolStripButton tsbExecute;
        private System.Windows.Forms.ToolStripButton tbsVizing;
        private System.Windows.Forms.ToolStripButton tsbNews;
        private System.Windows.Forms.ToolStripTextBox tsbComment;
        private System.Windows.Forms.ToolStripProgressBar FuelBar;
        private System.Windows.Forms.TreeView treeViewDocs;
        private System.Windows.Forms.ToolStripComboBox tsbCalendar;
        private System.Windows.Forms.Panel panelStructure;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TreeView treeViewStructure;
    }
}
