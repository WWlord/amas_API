namespace AMASControlRegisters
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
            this.tsLeft = new System.Windows.Forms.ToolStripButton();
            this.tsRight = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.edtsAddList = new System.Windows.Forms.ToolStripButton();
            this.edtsRemovelist = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripComboBox1 = new System.Windows.Forms.ToolStripComboBox();
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
            this.panelpicters.BackColor = System.Drawing.SystemColors.ControlLightLight;
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
            this.tsLeft,
            this.tsRight,
            this.toolStripSeparator2,
            this.edtsAddList,
            this.edtsRemovelist,
            this.toolStripSeparator1,
            this.toolStripComboBox1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(598, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // edtsAdd
            // 
            this.edtsAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsAdd.Image = global::AMASControlRegisters.Properties.Resources.FOLDRS02;
            this.edtsAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsAdd.Name = "edtsAdd";
            this.edtsAdd.Size = new System.Drawing.Size(23, 22);
            this.edtsAdd.Text = "Загрузить библиотеку";
            this.edtsAdd.Click += new System.EventHandler(this.tsAdd_Click);
            // 
            // edtsKill
            // 
            this.edtsKill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsKill.Image = global::AMASControlRegisters.Properties.Resources.WASTE;
            this.edtsKill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsKill.Name = "edtsKill";
            this.edtsKill.Size = new System.Drawing.Size(23, 22);
            this.edtsKill.Text = "В корзину";
            this.edtsKill.Click += new System.EventHandler(this.edtsKill_Click);
            // 
            // edtsSave
            // 
            this.edtsSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsSave.Image = global::AMASControlRegisters.Properties.Resources.DISK03;
            this.edtsSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsSave.Name = "edtsSave";
            this.edtsSave.Size = new System.Drawing.Size(23, 22);
            this.edtsSave.Text = "Сохранить ";
            this.edtsSave.Click += new System.EventHandler(this.tsSave_Click);
            // 
            // tsLeft
            // 
            this.tsLeft.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsLeft.Image = global::AMASControlRegisters.Properties.Resources.ARW05LT;
            this.tsLeft.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsLeft.Name = "tsLeft";
            this.tsLeft.Size = new System.Drawing.Size(23, 22);
            this.tsLeft.Text = "предыдущая картинка";
            this.tsLeft.Click += new System.EventHandler(this.tsLeft_Click);
            // 
            // tsRight
            // 
            this.tsRight.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsRight.Image = global::AMASControlRegisters.Properties.Resources.ARW05RT;
            this.tsRight.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsRight.Name = "tsRight";
            this.tsRight.Size = new System.Drawing.Size(23, 22);
            this.tsRight.Text = "следующая картинка";
            this.tsRight.Click += new System.EventHandler(this.tsRight_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // edtsAddList
            // 
            this.edtsAddList.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsAddList.Image = global::AMASControlRegisters.Properties.Resources.PLUS;
            this.edtsAddList.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.edtsAddList.Name = "edtsAddList";
            this.edtsAddList.Size = new System.Drawing.Size(23, 22);
            this.edtsAddList.Text = "Добавить картинку";
            this.edtsAddList.Click += new System.EventHandler(this.edtsAddList_Click);
            // 
            // edtsRemovelist
            // 
            this.edtsRemovelist.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.edtsRemovelist.Image = global::AMASControlRegisters.Properties.Resources.MINUS;
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
            // toolStripComboBox1
            // 
            this.toolStripComboBox1.Items.AddRange(new object[] {
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
            this.toolStripComboBox1.Name = "toolStripComboBox1";
            this.toolStripComboBox1.Size = new System.Drawing.Size(121, 25);
            this.toolStripComboBox1.Text = "Масштаб";
            this.toolStripComboBox1.Click += new System.EventHandler(this.toolStripComboBox1_Click);
            // 
            // openFilePicters
            // 
            this.openFilePicters.Filter = "Графика|*.jpg;*.jpeg;*.giff;*.png";
            this.openFilePicters.InitialDirectory = "c:/";
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
        private System.Windows.Forms.ToolStripButton tsLeft;
        private System.Windows.Forms.ToolStripButton tsRight;
        private System.Windows.Forms.ToolStripButton edtsKill;
        private System.Windows.Forms.OpenFileDialog openFilePicters;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripComboBox toolStripComboBox1;
        private System.Windows.Forms.ToolStripButton edtsAddList;
        private System.Windows.Forms.ToolStripButton edtsRemovelist;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panelPicter;
        private System.Windows.Forms.PictureBox pbPicter;
        private System.Windows.Forms.TrackBar trackBarPicter;
    }
}
