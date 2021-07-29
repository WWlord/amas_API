using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Chief
{
    public partial class FormKLADR : Form
    {

        private AMAS_DBI.Class_syb_acc SyB_Acc;

        public FormKLADR(AMAS_DBI.Class_syb_acc ACC)
        {
            InitializeComponent();

            SyB_Acc = ACC;

            tbAddress = new DataTable();
            for (int i = 0; i < dgvAddress.Columns.Count; i++)
            {
                DataColumn daco = tbAddress.Columns.Add();
                daco.ColumnName = dgvAddress.Columns[i].Name;
            }

            backgroundWorkerKLARK = new BackgroundWorker();
            backgroundWorkerKLARK.WorkerReportsProgress = true;
            backgroundWorkerKLARK.WorkerSupportsCancellation = true;
            backgroundWorkerKLARK.DoWork += new DoWorkEventHandler(backgroundWorkerKLARK_DoWork);
            backgroundWorkerKLARK.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerKLARK_ProgressChanged);
            backgroundWorkerKLARK.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerKLARK_RunWorkerCompleted);

            backgroundWorkerStreets = new BackgroundWorker();
            backgroundWorkerStreets.WorkerReportsProgress = true;
            backgroundWorkerStreets.WorkerSupportsCancellation = true;
            backgroundWorkerStreets.DoWork += new DoWorkEventHandler(backgroundWorkerStreets_DoWork);
            backgroundWorkerStreets.ProgressChanged += new ProgressChangedEventHandler(backgroundWorkerStreets_ProgressChanged);
            backgroundWorkerStreets.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorkerStreets_RunWorkerCompleted);
        }

        void backgroundWorkerStreets_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvAddress.DataSource = tbAddress;
            dgvAddress.Refresh();

        }

        void backgroundWorkerStreets_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tsProgressBarLoad.Value = RowsKla;
            tsLabelMin.Text = tsProgressBarLoad.Value.ToString();

        }

        void backgroundWorkerStreets_DoWork(object sender, DoWorkEventArgs e)
        {
            int Percent = 400;
            int inprocent = 0;
            RowsKla = 0;
            foreach (DataRow KlaRow in tbSTREET.Rows)
            {
                if (backgroundWorkerStreets.CancellationPending) break;
                inprocent++;
                RowsKla++;
                if (inprocent >= Percent)
                {
                    inprocent = 0;
                    backgroundWorkerStreets.ReportProgress(RowsKla);
                }
                AMAS_DBI.AMASCommand.ADD_KLADR_Address(KlaRow[0].ToString().Trim(), KlaRow[2].ToString().Trim());
            }

        }

        void backgroundWorkerKLARK_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            dgvAddress.DataSource = tbAddress;
            dgvAddress.Refresh();
        }

        void backgroundWorkerKLARK_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            tsProgressBarLoad.Value = RowsKla;
            tsLabelMin.Text = tsProgressBarLoad.Value.ToString();
        }

        private System.ComponentModel.BackgroundWorker backgroundWorkerKLARK;
        private System.ComponentModel.BackgroundWorker backgroundWorkerStreets;

        private DataTable tbAddress ;

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            tsProgressBarLoad.Maximum = tbKLADR.Rows.Count;
            tsProgressBarLoad.Minimum = 0;
            tsLabelMax.Text = tsProgressBarLoad.Maximum.ToString();
            backgroundWorkerKLARK.RunWorkerAsync();
        }

        int RowsKla = 0;
           
        void backgroundWorkerKLARK_DoWork(object sender, DoWorkEventArgs e)
        {
            int Percent = 400;
            int inprocent = 0;
            RowsKla = 0;
            foreach (DataRow KlaRow in tbKLADR.Rows)
            {
                if (backgroundWorkerKLARK.CancellationPending) break;
               inprocent++;
               RowsKla++;
                if (inprocent >= Percent)
                {
                    inprocent = 0;
                    backgroundWorkerKLARK.ReportProgress(RowsKla);
                }
                AMAS_DBI.AMASCommand.ADD_KLADR_Address(KlaRow[0].ToString().Trim(), KlaRow[2].ToString().Trim());
             }
        }

        public void LoadAddressTable()
        {
            string Reg = "";
            string Ri = "";
            string City = "";
            string District = "";
            int KLAlen = 0;
            int Percent = 400;
            int inprocent = 0;

            object[] KlarowVals = new object[6];

            foreach (DataRow KlaRow in tbKLADR.Rows )
            {
                 Reg = "";
                 Ri = "";
                 City = "";
                 District = "";
                
                RowsKla++;
                inprocent++;
                if (inprocent >= Percent)
                {
                    inprocent = 0;
                    backgroundWorkerKLARK.ReportProgress(RowsKla);
                }
                if (RowsKla > 100000) break;
                if (backgroundWorkerKLARK.CancellationPending) break;

                Reg = KlaRow[2].ToString().Substring(0, 2);
                Ri = KlaRow[2].ToString().Substring(2, 3);
                City = KlaRow[2].ToString().Substring(5, 3);
                if ((int)Convert.ToInt32(Ri) == 0 && (int)Convert.ToInt32(City) == 0)
                {
                    KlarowVals[0] = KlaRow[0];
                    KlarowVals[1] = KlaRow[2].ToString();
                    tbAddress.Rows.Add(KlarowVals);

                    if (KlaRow[2].ToString().Length > KLAlen) KLAlen = KlaRow[2].ToString().Length;
                }
                else if ((int)Convert.ToInt32(Ri) != 0 && (int)Convert.ToInt32(City) == 0)
                {
                    string rrs = "";

                    foreach (DataRow RegRow in tbKLADR.Rows)
                    //for (int iregrow = tbAddress.Rows.Count - 1; iregrow <= 0; iregrow--)
                    {
                        //DataRow RegRow = tbAddress.Rows[iregrow];
                        if (RegRow[2].ToString().CompareTo(Reg.PadRight(KLAlen, '0')) == 0)
                        //if (RegRow[3].ToString().CompareTo(Reg.PadRight(KLAlen, '0')) == 0)
                        {
                            rrs = RegRow[0].ToString();
                            //rrs = RegRow[3].ToString();
                            KlarowVals[0] = rrs;
                            KlarowVals[1] = Reg.PadRight(KLAlen, '0');
                            KlarowVals[2] = KlaRow[0];
                            KlarowVals[3] = KlaRow[2].ToString();
                            tbAddress.Rows.Add(KlarowVals);
                            break;
                        }
                    }
                }
                else if ((int)Convert.ToInt32(Ri) != 0 && (int)Convert.ToInt32(City) != 0)
                {
                    string rrs = "";
                    string rsd = "";

                    foreach (DataRow RegRow in tbKLADR.Rows)
                    {
                        if (RegRow[2].ToString().CompareTo(Reg.PadRight(KLAlen, '0')) == 0)
                        {
                            rrs = RegRow[0].ToString();
                            //foreach (DataRow RiRow in tbKLADR.Rows)
                            for (int iregrow = tbAddress.Rows.Count - 1; iregrow <= 0; iregrow--)
                            {
                                DataRow RiRow = tbAddress.Rows[iregrow];
                                //if (RiRow[2].ToString().CompareTo(Reg.PadRight(KLAlen, '0')) == 0)
                                if (RiRow[3].ToString().CompareTo(Reg.PadRight(KLAlen, '0')) == 0)
                                {                                    
                                    //rsd = RiRow[0].ToString();
                                    rsd = RiRow[2].ToString();
                                    KlarowVals[0] = rrs;
                                    KlarowVals[1] = Reg.PadRight(KLAlen, '0');
                                    KlarowVals[2] = rsd;
                                    KlarowVals[3] = (string)(Reg + Ri).PadRight(KLAlen, '0');
                                    KlarowVals[4] = KlaRow[0];
                                    KlarowVals[5] = KlaRow[2].ToString();
                                    tbAddress.Rows.Add(KlarowVals);
                                    break;
                                }
                            }
                            break;
                        }
                    }
                }
                else if ((int)Convert.ToInt32(Ri) == 0 && (int)Convert.ToInt32(City) != 0)
                {
                    string rrs = "";
                    foreach (DataRow RegRow in tbKLADR.Rows)
                    //for (int iregrow = tbAddress.Rows.Count-1; iregrow <= 0; iregrow--)
                    {
                        //DataRow RegRow = tbAddress.Rows[iregrow];
                        if (RegRow[2].ToString().CompareTo(Reg.PadRight(KLAlen, '0')) == 0)
                        {
                            rrs = RegRow[0].ToString();
                            KlarowVals[0] = rrs;
                            KlarowVals[1] = Reg.PadRight(KLAlen, '0');
                            KlarowVals[4] = KlaRow[0];
                            KlarowVals[5] = KlaRow[2].ToString();
                            tbAddress.Rows.Add(KlarowVals);
                            break;
                        }
                    }
                }
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            foreach (DataRow KlRow in dgvAddress.Rows)
            {
                try
                {
                    SyB_Acc.SQLCommand.CommandType = CommandType.Text;
                    SyB_Acc.SQLCommand.Parameters.Clear();
                    SyB_Acc.SQLCommand.Parameters.Add("@Region", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[0].Value = KlRow[0].ToString();
                    SyB_Acc.SQLCommand.Parameters.Add("@RegionCod", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[1].Value = KlRow[1].ToString();
                    SyB_Acc.SQLCommand.Parameters.Add("@Areal", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[2].Value = KlRow[2].ToString();
                    SyB_Acc.SQLCommand.Parameters.Add("@ArealCod", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[3].Value = KlRow[3].ToString();
                    SyB_Acc.SQLCommand.Parameters.Add("@City", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[4].Value = KlRow[4].ToString();
                    SyB_Acc.SQLCommand.Parameters.Add("@CityCod", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[5].Value = KlRow[5].ToString();
                    SyB_Acc.SQLCommand.Parameters.Add("@Street", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[6].Value = KlRow[6].ToString();
                    SyB_Acc.SQLCommand.Parameters.Add("@StreetCod", SqlDbType.VarChar);
                    SyB_Acc.SQLCommand.Parameters[7].Value = KlRow[7].ToString();
                    SyB_Acc.SQLCommand.CommandText = "insert into dbo.ADR_KLADR ( Region,RegionCod,Areal,ArealCod,City,CityCod,Street,StreetCod) values ( @Region,@RegionCod,@Areal,@ArealCod,@City,@CityCod,@Street,@StreetCod)";
                    SyB_Acc.SQLCommand.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    SyB_Acc.EBBLP.AddError(ex.Message, "KLADR - 4", ex.StackTrace);
                }

            }
        }

        private DataTable tbKLADR;
        private DataTable tbALTNAME;
        private DataTable tbFLAT;
        private DataTable tbDOMA;
        private DataTable tbSOCRBASE;
        private DataTable tbSTREET;

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();
            string connectionString = "Dsn=dBASE Files;dbq=" + folderBrowserDialog1.SelectedPath.ToString() + ";defaultdir=" + folderBrowserDialog1.SelectedPath.ToString() + ";fil=dBase 5.0;maxbuffersize=2048;pagetimeout=5"; //";driverid=533;maxbuffersize=2048;pagetimeout=5";
            //string connectionString = "Dsn=KLBase;dbq=" + folderBrowserDialog1.SelectedPath.ToString() + ";defaultdir=" + folderBrowserDialog1.SelectedPath.ToString() + ";fil=dBase 5.0;maxbuffersize=2048;pagetimeout=5";
            using (OdbcConnection connection = new OdbcConnection(connectionString))
            {
                connection.Open();

                OdbcCommand command;

                OdbcDataReader KlaReader;

                command = new OdbcCommand("Select * from KLADR", connection);
                KlaReader = command.ExecuteReader();
                tbKLADR = new DataTable();
                tbKLADR.Load(KlaReader, LoadOption.OverwriteChanges);

                command = new OdbcCommand("Select * from ALTNAMES", connection);
                KlaReader = command.ExecuteReader();
                tbALTNAME = new DataTable();
                tbALTNAME.Load(KlaReader);

                command = new OdbcCommand("Select * from FLAT", connection);
                KlaReader = command.ExecuteReader();
                tbFLAT = new DataTable();
                tbFLAT.Load(KlaReader);

                command = new OdbcCommand("Select * from DOMA", connection);
                KlaReader = command.ExecuteReader();
                tbDOMA = new DataTable();
                tbDOMA.Load(KlaReader);

                command = new OdbcCommand("Select * from SOCRBASE", connection);
                KlaReader = command.ExecuteReader();
                tbSOCRBASE = new DataTable();
                tbSOCRBASE.Load(KlaReader);

                command = new OdbcCommand("Select * from STREET", connection);
                KlaReader = command.ExecuteReader();
                tbSTREET = new DataTable();
                tbSTREET.Load(KlaReader);

                dgvKLADR.DataSource = tbKLADR;
                dgvKLADR.Refresh();
                dgvSOCRBASE.DataSource = tbALTNAME;
                dgvSOCRBASE.Refresh();
                dgvSTREET.DataSource = tbSTREET;
                dgvSTREET.Refresh();
                dgvDOMA.DataSource = tbDOMA;
                dgvDOMA.Refresh();
                dgvFLAT.DataSource = tbFLAT;
                dgvFLAT.Refresh();
                dgvSOCRBASE.DataSource = tbSOCRBASE;
                dgvSOCRBASE.Refresh();

                MessageBox.Show("Загрузка данных завершена");
            }
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            backgroundWorkerKLARK.CancelAsync();
            dgvAddress.DataSource = tbAddress;
            dgvAddress.Refresh();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            tsProgressBarLoad.Maximum =  tbSTREET.Rows.Count;
            tsProgressBarLoad.Minimum = 0;
            tsLabelMax.Text = tsProgressBarLoad.Maximum.ToString();
            tsLabelMin.Text = tsProgressBarLoad.Minimum.ToString();
            backgroundWorkerStreets.RunWorkerAsync();

        }
    }
}
