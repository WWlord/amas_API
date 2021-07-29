using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Sql;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using AMAS_DBI;
using Chief.baseLayer;

namespace Chief
{
    public partial class FormConnect : Form
    {
        public string conn_ODBC;
        public int conn_select_DB;
        public string UID;
        public string PWD;
        private ChefSettings frmSettings1 = new ChefSettings();

        public FormConnect()
        {
            InitializeComponent();
            conn_select_DB = (int)AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL;
            this.Load += new EventHandler(FormConnect_Load);
            this.FormClosing+=new FormClosingEventHandler(FormConnect_FormClosing);
            this.SizeChanged += new EventHandler(FormConnect_SizeChanged);
        }

        void FormConnect_SizeChanged(object sender, EventArgs e)
        {
            if (this.Height < 577) this.Height =466;
            if (this.Width < 466) this.Width = 577;
        }

        private void FormConnect_Load(Object sender, EventArgs e)
        {
            frmSettings1.SettingsKey = "Connecting";
            //Data bind settings properties with straightforward associations.
            Binding bndBackColor = new Binding("BackColor", frmSettings1,
                "FormBackColor", true, DataSourceUpdateMode.OnPropertyChanged);
            this.DataBindings.Add(bndBackColor);
            Binding bndSize = new Binding("Size", frmSettings1, "FormSize",
                true, DataSourceUpdateMode.OnPropertyChanged);
            this.DataBindings.Add(bndSize);
            Binding bndLocation = new Binding("Location", frmSettings1,
                "FormLocation", true, DataSourceUpdateMode.OnPropertyChanged);
            this.DataBindings.Add(bndLocation);

            //For more complex associations, manually assign associations.
            String savedText = frmSettings1.FormText;
            //Since there is no default value for FormText.
            if (savedText != null)
                this.Text = savedText;

            System.Data.Sql.SqlDataSourceEnumerator SQLServersList = System.Data.Sql.SqlDataSourceEnumerator.Instance;
            try
            {
                DataTable Serverstable = SQLServersList.GetDataSources();
                DataRow row;
                for (int l = 0; l < Serverstable.Rows.Count; l++)
                {
                    row = Serverstable.Rows[l];
                    this.comboBox1.Items.Add(row["ServerName"]);
                }
                String Server = frmSettings1.ServerName;
                if (Server != null)
                {
                    bool selected = false;
                    for (int i = 0; i < comboBox1.Items.Count; i++)
                    {
                        if (comboBox1.Items[i].ToString().ToLower().Trim().CompareTo(Server.ToLower().Trim()) == 0)
                        {
                            this.comboBox1.SelectedIndex = i;
                            selected = true;
                            break;
                        }
                    }
                    if (!selected)
                    {
                        int i = this.comboBox1.Items.Add(Server);
                        this.comboBox1.SelectedIndex = i;
                    }
                }
            }
            catch
            {
                if(frmSettings1.ServerName!=null)
                this.comboBox1.Text = frmSettings1.ServerName;
            }

            String Login = frmSettings1.LoginName;
            if (Login != null)
                this.textBox5.Text = Login;

            String Password = frmSettings1.Password;
            if (Password != null)
                this.textBox6.Text = Password;

        }

        private void FormConnect_FormClosing(Object sender, FormClosingEventArgs e)
        {
            frmSettings1.ServerName = this.comboBox1.Text;
            frmSettings1.LoginName = this.textBox5.Text;
            frmSettings1.Password = this.textBox6.Text;
            frmSettings1.Save();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            conn_select_DB = (int)AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL;
            UID = this.textBox5.Text;
            PWD = this.textBox6.Text;
            conn_ODBC = this.comboBox1.Text;
            this.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            conn_select_DB = (int)AMAS_DBI.Class_syb_acc.AMAS_connections.Sybase;
            UID = this.textBox7.Text;
            PWD = this.textBox8.Text;
            conn_ODBC = this.textBox2.Text;
            this.Close();
        }
        
    }
}