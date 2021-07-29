namespace Chief
{
    partial class GSOrganisation
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
            this.juridicRegister1 = new AMASControlRegisters.JuridicRegister();
            this.buttonYes = new System.Windows.Forms.Button();
            this.buttonNo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // juridicRegister1
            // 
            this.juridicRegister1.Location = new System.Drawing.Point(12, 0);
            this.juridicRegister1.Name = "juridicRegister1";
            this.juridicRegister1.Size = new System.Drawing.Size(590, 245);
            this.juridicRegister1.TabIndex = 0;
            // 
            // buttonYes
            // 
            this.buttonYes.Location = new System.Drawing.Point(332, 256);
            this.buttonYes.Name = "buttonYes";
            this.buttonYes.Size = new System.Drawing.Size(130, 23);
            this.buttonYes.TabIndex = 1;
            this.buttonYes.Text = "Назначить";
            this.buttonYes.UseVisualStyleBackColor = true;
            this.buttonYes.Click += new System.EventHandler(this.buttonYes_Click);
            // 
            // buttonNo
            // 
            this.buttonNo.Location = new System.Drawing.Point(468, 256);
            this.buttonNo.Name = "buttonNo";
            this.buttonNo.Size = new System.Drawing.Size(127, 23);
            this.buttonNo.TabIndex = 2;
            this.buttonNo.Text = "Отказаться";
            this.buttonNo.UseVisualStyleBackColor = true;
            this.buttonNo.Click += new System.EventHandler(this.buttonNo_Click);
            // 
            // GSOrganisation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(607, 291);
            this.Controls.Add(this.buttonNo);
            this.Controls.Add(this.buttonYes);
            this.Controls.Add(this.juridicRegister1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GSOrganisation";
            this.ShowInTaskbar = false;
            this.Text = "Веберите организацию";
            this.ResumeLayout(false);

        }

        #endregion

        private AMASControlRegisters.JuridicRegister juridicRegister1;
        private System.Windows.Forms.Button buttonYes;
        private System.Windows.Forms.Button buttonNo;
    }
}