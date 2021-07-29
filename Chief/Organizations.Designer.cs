namespace Chief
{
    partial class Organizations
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
            this.JuridicRegister1 = new AMASControlRegisters.JuridicRegister();
            this.panel1 = new System.Windows.Forms.Panel();
            this.contragentAttr = new AMASControlRegisters.ContragentAttributes();
            this.lblContragent = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // JuridicRegister1
            // 
            this.JuridicRegister1.Dock = System.Windows.Forms.DockStyle.Top;
            this.JuridicRegister1.Location = new System.Drawing.Point(0, 0);
            this.JuridicRegister1.Name = "JuridicRegister1";
            this.JuridicRegister1.Size = new System.Drawing.Size(614, 256);
            this.JuridicRegister1.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.contragentAttr);
            this.panel1.Controls.Add(this.lblContragent);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 256);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(614, 232);
            this.panel1.TabIndex = 1;
            // 
            // contragentAttr
            // 
            this.contragentAttr.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contragentAttr.Location = new System.Drawing.Point(0, 16);
            this.contragentAttr.Name = "contragentAttr";
            this.contragentAttr.Size = new System.Drawing.Size(614, 216);
            this.contragentAttr.TabIndex = 4;
            // 
            // lblContragent
            // 
            this.lblContragent.AutoSize = true;
            this.lblContragent.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblContragent.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lblContragent.ForeColor = System.Drawing.Color.DarkViolet;
            this.lblContragent.Location = new System.Drawing.Point(0, 0);
            this.lblContragent.Name = "lblContragent";
            this.lblContragent.Size = new System.Drawing.Size(94, 16);
            this.lblContragent.TabIndex = 3;
            this.lblContragent.Text = "Контрагент";
            // 
            // Organizations
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 488);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.JuridicRegister1);
            this.Name = "Organizations";
            this.Text = "Организации";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private AMASControlRegisters.JuridicRegister JuridicRegister1;
        private System.Windows.Forms.Panel panel1;
        private AMASControlRegisters.ContragentAttributes contragentAttr;
        private System.Windows.Forms.Label lblContragent;
    }
}