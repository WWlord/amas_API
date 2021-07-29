namespace AMASControlRegisters
{
    partial class UCMoviesList
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
            this.listViewMovies = new System.Windows.Forms.ListView();
            this.chSender = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.chMessage = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // listViewMovies
            // 
            this.listViewMovies.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.chSender,
            this.chMessage});
            this.listViewMovies.Dock = System.Windows.Forms.DockStyle.Fill;
            this.listViewMovies.GridLines = true;
            this.listViewMovies.Location = new System.Drawing.Point(0, 0);
            this.listViewMovies.MultiSelect = false;
            this.listViewMovies.Name = "listViewMovies";
            this.listViewMovies.Size = new System.Drawing.Size(546, 379);
            this.listViewMovies.TabIndex = 0;
            this.listViewMovies.UseCompatibleStateImageBehavior = false;
            this.listViewMovies.View = System.Windows.Forms.View.Details;
            this.listViewMovies.SelectedIndexChanged += new System.EventHandler(this.listViewMovies_SelectedIndexChanged);
            // 
            // chSender
            // 
            this.chSender.Text = "Отправитель";
            this.chSender.Width = 241;
            // 
            // chMessage
            // 
            this.chMessage.Text = "Задание";
            this.chMessage.Width = 339;
            // 
            // UCMoviesList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.listViewMovies);
            this.Name = "UCMoviesList";
            this.Size = new System.Drawing.Size(546, 379);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView listViewMovies;
        private System.Windows.Forms.ColumnHeader chSender;
        private System.Windows.Forms.ColumnHeader chMessage;
    }
}
