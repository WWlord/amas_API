namespace Chief
{
    partial class FormSeekEmDep
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
            this.checkedDepEmp = new System.Windows.Forms.CheckedListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxED = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // checkedDepEmp
            // 
            this.checkedDepEmp.BackColor = System.Drawing.SystemColors.Control;
            this.checkedDepEmp.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.checkedDepEmp.CheckOnClick = true;
            this.checkedDepEmp.FormattingEnabled = true;
            this.checkedDepEmp.Items.AddRange(new object[] {
            "Подразделение",
            "Сотрудник"});
            this.checkedDepEmp.Location = new System.Drawing.Point(286, 12);
            this.checkedDepEmp.Name = "checkedDepEmp";
            this.checkedDepEmp.Size = new System.Drawing.Size(130, 30);
            this.checkedDepEmp.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 58);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Текст";
            // 
            // textBoxED
            // 
            this.textBoxED.Location = new System.Drawing.Point(81, 55);
            this.textBoxED.Name = "textBoxED";
            this.textBoxED.Size = new System.Drawing.Size(335, 20);
            this.textBoxED.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Image = global::Chief.Properties.Resources.DataContainer_MoveNextHS;
            this.button2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button2.Location = new System.Drawing.Point(315, 91);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(101, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "следующий";
            this.button2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button1
            // 
            this.button1.Image = global::Chief.Properties.Resources.DataContainer_MovePreviousHS;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(213, 91);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(104, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "предыдущий";
            this.button1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormSeekEmDep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(440, 127);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.textBoxED);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.checkedDepEmp);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormSeekEmDep";
            this.ShowInTaskbar = false;
            this.Text = "Поиск сотрудников и подразделений";
            this.TopMost = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckedListBox checkedDepEmp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxED;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;

    }
}