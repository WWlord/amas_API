namespace ClassPattern
{
    partial class PictersLibrary
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
            this.panelpicters = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.edtsAdd = new System.Windows.Forms.ToolStripButton();
            this.edtsKill = new System.Windows.Forms.ToolStripButton();
            this.edtsSave = new System.Windows.Forms.ToolStripButton();
            this.WktsLeft = new System.Windows.Forms.ToolStripButton();
            this.WktsRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.edtsAddList = new System.Windows.Forms.ToolStripButton();
            this.edtsRemovelist = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.edWktsComboBox = new System.Windows.Forms.ToolStripComboBox();
            this.edWktsPicRollSit = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.edWktsWidth = new System.Windows.Forms.ToolStripButton();
            this.ededWktsHeight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.edtsClose = new System.Windows.Forms.ToolStripButton();
            this.openFilePicters = new System.Windows.Forms.OpenFileDialog();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panelPicter = new System.Windows.Forms.Panel();
            this.pbPicter = new System.Windows.Forms.PictureBox();
            this.trackBarPicter = new System.Windows.Forms.TrackBar();
            this.toolStrip1.SuspendLayout();
            this.panelPicter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPicter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPicter)).BeginInit();
            this.SuspendLayout();
            // 
            // panelpicters
            // 
            this.panelpicters.AutoScroll = true;
            this.panelpicters.BackColor = System.Drawing.SystemColors.Info;
            this.panelpicters.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panelpicters.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelpicters.Location = new System.Drawing.Point(0, 193);
            this.panelpicters.Name = "panelpicters";
            this.panelpicters.Size = new System.Drawing.Size(598, 100);
            this.panelpicters.TabIndex = 1;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.edtsAdd,
            this.edtsKill,
            this.edtsSave,
            this.WktsLeft,
            this.WktsRight,
            this.toolStripSeparator2,
            this.edtsAddList,
            this.edtsRemovelist,
            this.toolStripSeparator1,
            this.edWktsComboBox,
            this.edWktsPicRollSit,
            this.toolStripSeparator3,
            this.edWktsWidth,
            this.ededWktsHeight,
            this.toolStripSeparator4,
            this.edtsClose});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(598, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // edtsAdd
            // 
            this.edtsAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsAdd.Image = global::ClassPattern.Properties.Resources.FOLDER02;
            this.edtsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsAdd.Name = "edtsAdd";
            this.edtsAdd.Size = new System.Drawing.Size(23, 22);
            this.edtsAdd.Text = "Загрузить библиотеку";
            this.edtsAdd.Click += new System.EventHandler(this.tsAdd_Click);
            // 
            // edtsKill
            // 
            this.edtsKill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsKill.Image = global::ClassPattern.Properties.Resources.WASTE;
            this.edtsKill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsKill.Name = "edtsKill";
            this.edtsKill.Size = new System.Drawing.Size(23, 22);
            this.edtsKill.Text = "В корзину";
            this.edtsKill.Click += new System.EventHandler(this.edtsKill_Click);
            // 
            // edtsSave
            // 
            this.edtsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsSave.Image = global::ClassPattern.Properties.Resources.DISK03;
            this.edtsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsSave.Name = "edtsSave";
            this.edtsSave.Size = new System.Drawing.Size(23, 22);
            this.edtsSave.Text = "Сохранить ";
            this.edtsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // WktsLeft
            // 
            this.WktsLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.WktsLeft.Image = global::ClassPattern.Properties.Resources.ARW05LT;
            this.WktsLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WktsLeft.Name = "WktsLeft";
            this.WktsLeft.Size = new System.Drawing.Size(23, 22);
            this.WktsLeft.Text = "предыдущая картинка";
            this.WktsLeft.Click += new System.EventHandler(this.tsLeft_Click);
            // 
            // WktsRight
            // 
            this.WktsRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.WktsRight.Image = global::ClassPattern.Properties.Resources.ARW05RT;
            this.WktsRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.WktsRight.Name = "WktsRight";
            this.WktsRight.Size = new System.Drawing.Size(23, 22);
            this.WktsRight.Text = "следующая картинка";
            this.WktsRight.Click += new System.EventHandler(this.tsRight_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // edtsAddList
            // 
            this.edtsAddList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsAddList.Image = global::ClassPattern.Properties.Resources.PLUS;
            this.edtsAddList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsAddList.Name = "edtsAddList";
            this.edtsAddList.Size = new System.Drawing.Size(23, 22);
            this.edtsAddList.Text = "Добавить картинку";
            this.edtsAddList.Click += new System.EventHandler(this.edtsAddList_Click);
            // 
            // edtsRemovelist
            // 
            this.edtsRemovelist.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsRemovelist.Image = global::ClassPattern.Properties.Resources.MINUS;
            this.edtsRemovelist.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsRemovelist.Name = "edtsRemovelist";
            this.edtsRemovelist.Size = new System.Drawing.Size(23, 22);
            this.edtsRemovelist.Text = "Удалить картинку";
            this.edtsRemovelist.Click += new System.EventHandler(this.edtsRemovelist_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // edWktsComboBox
            // 
            this.edWktsComboBox.Items.AddRange(new object[] {
            "10",
            "25",
            "50",
            "75",
            "100",
            "150",
            "200",
            "300",
            "500",
            "другой..."});
            this.edWktsComboBox.Name = "edWktsComboBox";
            this.edWktsComboBox.Size = new System.Drawing.Size(121, 25);
            this.edWktsComboBox.Text = "Масштаб";
            this.edWktsComboBox.Click += new System.EventHandler(this.edWktsComboBox_Click);
            // 
            // edWktsPicRollSit
            // 
            this.edWktsPicRollSit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edWktsPicRollSit.Image = global::ClassPattern.Properties.Resources.REMTEACC;
            this.edWktsPicRollSit.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edWktsPicRollSit.Name = "edWktsPicRollSit";
            this.edWktsPicRollSit.Size = new System.Drawing.Size(23, 22);
            this.edWktsPicRollSit.Text = "Горизонтальная или вертикальная галрея";
            this.edWktsPicRollSit.Click += new System.EventHandler(this.edtsPicRollSit_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // edWktsWidth
            // 
            this.edWktsWidth.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edWktsWidth.Image = global::ClassPattern.Properties.Resources.VW_DTLS;
            this.edWktsWidth.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edWktsWidth.Name = "edWktsWidth";
            this.edWktsWidth.Size = new System.Drawing.Size(23, 22);
            this.edWktsWidth.Text = "По ширине";
            this.edWktsWidth.Click += new System.EventHandler(this.tsWidth_Click);
            // 
            // ededWktsHeight
            // 
            this.ededWktsHeight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ededWktsHeight.Image = global::ClassPattern.Properties.Resources.RECTANGL;
            this.ededWktsHeight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ededWktsHeight.Name = "ededWktsHeight";
            this.ededWktsHeight.Size = new System.Drawing.Size(23, 22);
            this.ededWktsHeight.Text = "По высоте";
            this.ededWktsHeight.Click += new System.EventHandler(this.tsHeight_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // edtsClose
            // 
            this.edtsClose.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsClose.Image = global::ClassPattern.Properties.Resources.none;
            this.edtsClose.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsClose.Name = "edtsClose";
            this.edtsClose.Size = new System.Drawing.Size(23, 22);
            this.edtsClose.Text = "Завершить сканирование";
            // 
            // openFilePicters
            // 
            this.openFilePicters.Filter = "Графика|*.jpg;*.jpeg;*.giff;*.png";
            this.openFilePicters.Multiselect = true;
            this.openFilePicters.Title = "Выбор картинок";
            // 
            // panelPicter
            // 
            this.panelPicter.AutoScroll = true;
            this.panelPicter.Controls.Add(this.pbPicter);
            this.panelPicter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelPicter.Location = new System.Drawing.Point(0, 25);
            this.panelPicter.Name = "panelPicter";
            this.panelPicter.Size = new System.Drawing.Size(598, 168);
            this.panelPicter.TabIndex = 6;
            // 
            // pbPicter
            // 
            this.pbPicter.Location = new System.Drawing.Point(77, 62);
            this.pbPicter.Name = "pbPicter";
            this.pbPicter.Size = new System.Drawing.Size(350, 84);
            this.pbPicter.TabIndex = 5;
            this.pbPicter.TabStop = false;
            // 
            // trackBarPicter
            // 
            this.trackBarPicter.Dock = System.Windows.Forms.DockStyle.Top;
            this.trackBarPicter.Location = new System.Drawing.Point(0, 25);
            this.trackBarPicter.Name = "trackBarPicter";
            this.trackBarPicter.Size = new System.Drawing.Size(598, 42);
            this.trackBarPicter.TabIndex = 8;
            this.trackBarPicter.Visible = false;
            // 
            // PictersLibrary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.Controls.Add(this.trackBarPicter);
            this.Controls.Add(this.panelPicter);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.panelpicters);
            this.Name = "PictersLibrary";
            this.Size = new System.Drawing.Size(598, 293);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelPicter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbPicter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarPicter)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelpicters;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton edtsAdd;
        private System.Windows.Forms.ToolStripButton edtsSave;
        private System.Windows.Forms.ToolStripButton WktsLeft;
        private System.Windows.Forms.ToolStripButton WktsRight;
        private System.Windows.Forms.ToolStripButton edtsKill;
        private System.Windows.Forms.OpenFileDialog openFilePicters;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox edWktsComboBox;
        private System.Windows.Forms.ToolStripButton edtsAddList;
        private System.Windows.Forms.ToolStripButton edtsRemovelist;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panelPicter;
        private System.Windows.Forms.PictureBox pbPicter;
        private System.Windows.Forms.TrackBar trackBarPicter;
        private System.Windows.Forms.ToolStripButton edWktsPicRollSit;
        private System.Windows.Forms.ToolStripButton edWktsWidth;
        private System.Windows.Forms.ToolStripButton ededWktsHeight;
        private System.Windows.Forms.ToolStripButton edtsClose;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
    }
}
