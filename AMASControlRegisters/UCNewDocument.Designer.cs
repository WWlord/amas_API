namespace AMASControlRegisters
{
    partial class UCNewDocument
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
            this.btnGoBack = new System.Windows.Forms.Button();
            this.buaatonAddFile = new System.Windows.Forms.Button();
            this.btnEditor = new System.Windows.Forms.Button();
            this.btnRemoveFile = new System.Windows.Forms.Button();
            this.btnPattern = new System.Windows.Forms.Button();
            this.panelKIndTema = new System.Windows.Forms.Panel();
            this.btnSaveDocument = new System.Windows.Forms.Button();
            this.tbxmetadata = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnGoBack
            // 
            this.btnGoBack.BackColor = System.Drawing.Color.ForestGreen;
            this.btnGoBack.Location = new System.Drawing.Point(3, 106);
            this.btnGoBack.Name = "btnGoBack";
            this.btnGoBack.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.btnGoBack.Size = new System.Drawing.Size(106, 89);
            this.btnGoBack.TabIndex = 0;
            this.btnGoBack.Text = "Вернуться";
            this.btnGoBack.UseCompatibleTextRendering = true;
            this.btnGoBack.UseVisualStyleBackColor = false;
            this.btnGoBack.Click += new System.EventHandler(this.btnGoBack_Click);
            // 
            // buaatonAddFile
            // 
            this.buaatonAddFile.BackColor = System.Drawing.Color.CadetBlue;
            this.buaatonAddFile.Location = new System.Drawing.Point(115, 106);
            this.buaatonAddFile.Name = "buaatonAddFile";
            this.buaatonAddFile.Size = new System.Drawing.Size(106, 89);
            this.buaatonAddFile.TabIndex = 1;
            this.buaatonAddFile.Text = "Добавить файл";
            this.buaatonAddFile.UseVisualStyleBackColor = false;
            this.buaatonAddFile.Click += new System.EventHandler(this.buaatonAddFile_Click);
            // 
            // btnEditor
            // 
            this.btnEditor.BackColor = System.Drawing.Color.DarkGoldenrod;
            this.btnEditor.Location = new System.Drawing.Point(115, 206);
            this.btnEditor.Name = "btnEditor";
            this.btnEditor.Size = new System.Drawing.Size(106, 89);
            this.btnEditor.TabIndex = 2;
            this.btnEditor.Text = "Редактор";
            this.btnEditor.UseVisualStyleBackColor = false;
            this.btnEditor.Click += new System.EventHandler(this.btnEditor_Click);
            // 
            // btnRemoveFile
            // 
            this.btnRemoveFile.BackColor = System.Drawing.Color.CadetBlue;
            this.btnRemoveFile.Location = new System.Drawing.Point(3, 206);
            this.btnRemoveFile.Name = "btnRemoveFile";
            this.btnRemoveFile.Size = new System.Drawing.Size(106, 89);
            this.btnRemoveFile.TabIndex = 3;
            this.btnRemoveFile.Text = "Удалить файл";
            this.btnRemoveFile.UseVisualStyleBackColor = false;
            this.btnRemoveFile.Click += new System.EventHandler(this.btnRemoveFile_Click);
            // 
            // btnPattern
            // 
            this.btnPattern.BackColor = System.Drawing.Color.Pink;
            this.btnPattern.Location = new System.Drawing.Point(115, 301);
            this.btnPattern.Name = "btnPattern";
            this.btnPattern.Size = new System.Drawing.Size(106, 89);
            this.btnPattern.TabIndex = 4;
            this.btnPattern.Text = "Шаблон";
            this.btnPattern.UseVisualStyleBackColor = false;
            this.btnPattern.Click += new System.EventHandler(this.btnPattern_Click);
            // 
            // panelKIndTema
            // 
            this.panelKIndTema.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelKIndTema.Location = new System.Drawing.Point(0, 0);
            this.panelKIndTema.Name = "panelKIndTema";
            this.panelKIndTema.Size = new System.Drawing.Size(241, 100);
            this.panelKIndTema.TabIndex = 5;
            // 
            // btnSaveDocument
            // 
            this.btnSaveDocument.BackColor = System.Drawing.Color.DarkKhaki;
            this.btnSaveDocument.Location = new System.Drawing.Point(3, 301);
            this.btnSaveDocument.Name = "btnSaveDocument";
            this.btnSaveDocument.Size = new System.Drawing.Size(106, 89);
            this.btnSaveDocument.TabIndex = 6;
            this.btnSaveDocument.Text = "Сохранить документ";
            this.btnSaveDocument.UseVisualStyleBackColor = false;
            this.btnSaveDocument.Click += new System.EventHandler(this.btnSaveDocument_Click);
            // 
            // tbxmetadata
            // 
            this.tbxmetadata.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbxmetadata.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.tbxmetadata.Location = new System.Drawing.Point(0, 426);
            this.tbxmetadata.Multiline = true;
            this.tbxmetadata.Name = "tbxmetadata";
            this.tbxmetadata.Size = new System.Drawing.Size(241, 76);
            this.tbxmetadata.TabIndex = 7;
            // 
            // UCNewDocument
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tbxmetadata);
            this.Controls.Add(this.btnSaveDocument);
            this.Controls.Add(this.panelKIndTema);
            this.Controls.Add(this.btnPattern);
            this.Controls.Add(this.btnRemoveFile);
            this.Controls.Add(this.btnEditor);
            this.Controls.Add(this.buaatonAddFile);
            this.Controls.Add(this.btnGoBack);
            this.Name = "UCNewDocument";
            this.Size = new System.Drawing.Size(241, 502);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGoBack;
        private System.Windows.Forms.Button buaatonAddFile;
        private System.Windows.Forms.Button btnEditor;
        private System.Windows.Forms.Button btnRemoveFile;
        private System.Windows.Forms.Button btnPattern;
        private System.Windows.Forms.Panel panelKIndTema;
        private System.Windows.Forms.Button btnSaveDocument;
        private System.Windows.Forms.TextBox tbxmetadata;
    }
}
