namespace Chief
{
    partial class BusinessTasks
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.ListViewGroup listViewGroup1 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup2 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            System.Windows.Forms.ListViewGroup listViewGroup3 = new System.Windows.Forms.ListViewGroup("ListViewGroup", System.Windows.Forms.HorizontalAlignment.Left);
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listViewExecutions = new System.Windows.Forms.ListView();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.listViewDVT = new System.Windows.Forms.ListView();
            this.panelTasks = new System.Windows.Forms.Panel();
            this.panelVisa = new System.Windows.Forms.Panel();
            this.panelDocs = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.listViewExecutions);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1326, 422);
            this.splitContainer1.SplitterDistance = 354;
            this.splitContainer1.TabIndex = 1;
            // 
            // listViewExecutions
            // 
            this.listViewExecutions.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewExecutions.Location = new System.Drawing.Point(0, 0);
            this.listViewExecutions.Name = "listViewExecutions";
            this.listViewExecutions.Size = new System.Drawing.Size(354, 422);
            this.listViewExecutions.TabIndex = 0;
            this.listViewExecutions.UseCompatibleStateImageBehavior = false;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.listViewDVT);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panelTasks);
            this.splitContainer2.Panel2.Controls.Add(this.panelVisa);
            this.splitContainer2.Panel2.Controls.Add(this.panelDocs);
            this.splitContainer2.Size = new System.Drawing.Size(968, 422);
            this.splitContainer2.SplitterDistance = 321;
            this.splitContainer2.TabIndex = 1;
            // 
            // listViewDVT
            // 
            this.listViewDVT.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewGroup1.Header = "ListViewGroup";
            listViewGroup1.Name = "lvgDocs";
            listViewGroup1.Tag = "Документы";
            listViewGroup2.Header = "ListViewGroup";
            listViewGroup2.Name = "lvgVisa";
            listViewGroup2.Tag = "Согласование";
            listViewGroup3.Header = "ListViewGroup";
            listViewGroup3.Name = "lvgTasks";
            listViewGroup3.Tag = "Задание";
            this.listViewDVT.Groups.AddRange(new System.Windows.Forms.ListViewGroup[] {
            listViewGroup1,
            listViewGroup2,
            listViewGroup3});
            this.listViewDVT.Location = new System.Drawing.Point(0, 0);
            this.listViewDVT.Name = "listViewDVT";
            this.listViewDVT.Size = new System.Drawing.Size(321, 422);
            this.listViewDVT.TabIndex = 0;
            this.listViewDVT.UseCompatibleStateImageBehavior = false;
            // 
            // panelTasks
            // 
            this.panelTasks.Location = new System.Drawing.Point(304, 260);
            this.panelTasks.Name = "panelTasks";
            this.panelTasks.Size = new System.Drawing.Size(200, 100);
            this.panelTasks.TabIndex = 2;
            // 
            // panelVisa
            // 
            this.panelVisa.Location = new System.Drawing.Point(285, 117);
            this.panelVisa.Name = "panelVisa";
            this.panelVisa.Size = new System.Drawing.Size(200, 100);
            this.panelVisa.TabIndex = 1;
            // 
            // panelDocs
            // 
            this.panelDocs.Location = new System.Drawing.Point(68, 138);
            this.panelDocs.Name = "panelDocs";
            this.panelDocs.Size = new System.Drawing.Size(200, 100);
            this.panelDocs.TabIndex = 0;
            // 
            // BusinessTasks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1326, 422);
            this.Controls.Add(this.splitContainer1);
            this.Name = "BusinessTasks";
            this.Text = "Задания";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView listViewExecutions;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.ListView listViewDVT;
        private System.Windows.Forms.Panel panelTasks;
        private System.Windows.Forms.Panel panelVisa;
        private System.Windows.Forms.Panel panelDocs;


    }
}