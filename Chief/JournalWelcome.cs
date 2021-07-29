using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClassPattern;
using CommonValues;
using TwainGui;
using ClassStructure;

namespace Chief
{
    public partial class JournalWelcome : Form
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private Formularing JrnFormular;
        AMASControlRegisters.Document_Viewer document_Show;

        public JournalWelcome(AMAS_DBI.Class_syb_acc ACC, ImageList ImageStd)
        {
            InitializeComponent();
            AMASacc = ACC;
            dataGridView1.Columns.Add("Fname", "Τΰιλ");
            JrnFormular = new Formularing(tpFormular);

            panelRegistrator.Enabled = false;
            panelRKK.Enabled = false;
            panelId.Enabled = true;

            buttonGetNumber.BackColor = Color.YellowGreen;
            buttonContent.BackColor = Color.Gray;
            buttonSave.BackColor = Color.Gray;

            document_Show = new AMASControlRegisters.Document_Viewer(AMASacc, null);
            // 
            // document_Show
            // 
            this.document_Show.Doc_ID = 0;
            this.document_Show.Dock = System.Windows.Forms.DockStyle.Fill;
            this.document_Show.Location = new System.Drawing.Point(0, 0);
            this.document_Show.Name = "document_Show";
            this.document_Show.New_document = false;
            this.document_Show.Sender = 0;
            this.document_Show.Size = new System.Drawing.Size(416, 521);
            this.document_Show.TabIndex = 3;
            this.splitContainer1.Panel2.Controls.Add(this.document_Show);

            listViewNewDocs.View = View.Details;
            listViewNewDocs.SelectedIndexChanged+=new EventHandler(listViewNewDocs_SelectedIndexChanged);
            Refresh();
       }

        ArrayList Kinds_list = null;
        ArrayList Tema_list = null;
        ArrayList Coming_list = null;
        ArrayList Employees_list = null;
        private bool SelectTema = true;

        private void Refresh()
        {
            Kinds_list = new ArrayList();
            Coming_list = new ArrayList();
            cbKindDoc.DataSource = null;
            cbComingdoc.DataSource = null;
            cbKindDoc.Items.Clear();
            cbComingdoc.Items.Clear();
            string name = "";
            int id = -1;
            if (AMASacc.Set_table("TRiN1", AMAS_Query.Class_AMAS_Query.Wflow_kinds(), null))
            {
                try
                {
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                    {
                        AMASacc.Get_row(i);
                        id = (int)AMASacc.Find_Field("kod");
                        name = (string)AMASacc.Find_Field("kind");
                        Kinds_list.Add(new CommonClass.Arraysheet(name.Trim(), id));
                    }
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Registration Jour - 1", ex.StackTrace);
                }
                AMASacc.ReturnTable();
            }
            SelectTema = false;
            cbKindDoc.DataSource = Kinds_list;
            if (Kinds_list.Count > 0)
            {
                cbKindDoc.DisplayMember = "name";
                cbKindDoc.ValueMember = "id";
            }
            SelectTema = true;

            if (AMASacc.Set_table("TRiN2", AMAS_Query.Class_AMAS_Query.Wflow_comings(), null))
            {
                try
                {
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                    {
                        AMASacc.Get_row(i);
                        id = (int)AMASacc.Find_Field("cod");
                        name = (string)AMASacc.Find_Field("coming");
                        Coming_list.Add(new CommonClass.Arraysheet(name.Trim(), id));
                    }
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Registration Jour - 2", ex.StackTrace);
                }
                AMASacc.ReturnTable();
            }
            cbComingdoc.DataSource = Coming_list;
            if (Coming_list.Count > 0)
            {
                cbComingdoc.DisplayMember = "name";
                cbComingdoc.ValueMember = "id";
            }

            Select_Temy();

            Employees_list = new ArrayList();
            if (AMASacc.Set_table("TRiNPers", AMAS_Query.Class_AMAS_Query.Get_Personel_I_Degree(), null))
            {
                Employees_list.Clear();
                try
                {
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                    {
                        AMASacc.Get_row(i);
                        id = (int)AMASacc.Find_Field("cod");
                        name = (string)AMASacc.Find_Field("fio");
                        Employees_list.Add(new CommonClass.Arraysheet(name.Trim(), id));
                    }
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Registration Jour - 3", ex.StackTrace);
                }
                AMASacc.ReturnTable();
            }
            cbExecutor.DataSource = Employees_list;
            if (Employees_list.Count > 0)
            {
                cbExecutor.DisplayMember = "name";
                cbExecutor.ValueMember = "id";
            }

            dataGridView1.EditMode = DataGridViewEditMode.EditProgrammatically;
        }

        private void Select_Temy()
        {
            if (SelectTema)
            {
                Tema_list = new ArrayList();
                cbTemadoc.DataSource = null;
                cbTemadoc.Items.Clear();
                string name = "";
                int id = -1;
                if (cbKindDoc.Items.Count > 0)
                {
                    if (AMASacc.Set_table("TRiN3", AMAS_Query.Class_AMAS_Query.Wflow_temy((int)Convert.ToInt32(cbKindDoc.SelectedValue)), null))
                    {
                        try
                        {
                            for (int i = 0; i < AMASacc.Rows_count; i++)
                            {
                                AMASacc.Get_row(i);
                                id = (int)AMASacc.Find_Field("tema");
                                name = (string)AMASacc.Find_Field("description_");
                                Tema_list.Add(new CommonClass.Arraysheet(name.Trim(), id));
                            }
                        }
                        catch (Exception ex)
                        {
                            AMASacc.EBBLP.AddError(ex.Message, "Registration In - 4", ex.StackTrace);
                        }
                        AMASacc.ReturnTable();
                    }
                    cbTemadoc.DataSource = Tema_list;
                    if (Tema_list.Count > 0)
                    {
                        cbTemadoc.DisplayMember = "name";
                        cbTemadoc.ValueMember = "id";
                    }
                }
            }
        }

        private bool RegOrRKK = false;
        private int RegIdRKK = 0;

        private void IsRegistration(bool Reg)
        {
            this.Activate(); RegOrRKK = Reg;
            if (panelId.Enabled)
            {
                RegIdRKK = AMAS_DBI.AMASCommand.Append_Recieved_document((int)Convert.ToInt32(cbKindDoc.SelectedValue), 0, 0, 0, "", "", (int)Convert.ToInt32(cbTemadoc.SelectedValue), (int)Convert.ToInt32(cbComingdoc.SelectedValue), tbAnnotation.Text, 0);

                panelRegistrator.Enabled = true;
                panelRegistrator.Focus();
                panelRKK.Enabled = false;
                panelId.Enabled = false;
                labelRKK.Text = AMAS_DBI.AMASCommand.ShowDocumentFK(RegIdRKK);
                
                buttonGetNumber.BackColor = Color.Gray;
                buttonContent.BackColor = Color.YellowGreen;
                buttonSave.BackColor = Color.Gray;
            }
            else if (panelRegistrator.Enabled)
            {
                panelRegistrator.Enabled = false;
                panelRKK.Enabled = true;
                panelRKK.Focus();
                panelId.Enabled = false;

                buttonGetNumber.BackColor = Color.Gray;
                buttonContent.BackColor = Color.Gray;
                buttonSave.BackColor = Color.YellowGreen;

                AddContent();
            }
            else if (panelRKK.Enabled)
            {

                DateTime toDate = dateTimePickerExe.Value;
                int mv = 0;
                try
                {
                    if (cbExecutor.Items.Count > 0)
                        if (cbExecutor.SelectedIndex >= 0)
                            mv = AMAS_DBI.AMASCommand.Document_moving_short
                                (RegIdRKK,
                                (int) Convert.ToInt32 (cbExecutor.SelectedValue),
                                toDate);
                }
                catch(Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Journal Wellcome - 7", ex.StackTrace);
                }
                RegIdRKK = 0;
                labelRKK.Text = "";

                panelRegistrator.Enabled = false;
                panelRKK.Enabled = false;
                panelId.Enabled = true;
                panelId.Focus();

                buttonGetNumber.BackColor = Color.YellowGreen;
                buttonContent.BackColor = Color.Gray;
                buttonSave.BackColor = Color.Gray;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AppendDocumentContent();
        }

        private void WriteToJournal()
        {
            IsRegistration( true);
        }

        private void AppendDocumentContent()
        {
            IsRegistration(false);
        }

        private void SendDocument()
        {
            IsRegistration(false);
        }

        void Who_FormClosed(object sender, FormClosedEventArgs e)
        {
            cbSender.Items.Clear();
            if (AMASacc.Set_table("RegAutDoc", AMAS_Query.Class_AMAS_Query.Wflow_Autor(RegIdRKK), null))
            {
                try
                {
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                    {
                        AMASacc.Get_row(i);
                        cbSender.Items.Add( (string)AMASacc.Find_Field("fio"));
                    }
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Journal Wellcome - 10", ex.StackTrace);
                }
                AMASacc.ReturnTable();
            }

            if (AMASacc.Set_table("RegOrgDoc", AMAS_Query.Class_AMAS_Query.Wflow_organization(RegIdRKK), null))
            {
                try
                {
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                    {
                        AMASacc.Get_row(i);
                        cbSender.Items.Add((string)AMASacc.Find_Field("full_Name"));
                    }
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Journal Wellcome - 11", ex.StackTrace);
                }
                AMASacc.ReturnTable();
            }
            if (cbSender.Items.Count > 0) cbSender.SelectedItem = 0;
        }

        private void buttonGetNumber_Click_1(object sender, EventArgs e)
        {
            WriteToJournal();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string s = labelRKK.Text;
            string key=RegIdRKK.ToString();
            SendDocument();
            ListViewItem LVI = listViewNewDocs.Items.Add("KEY" + key, labelRKK.Text, 0);
            LVI.SubItems[0].Text = s;
            LVI.SubItems.Add( dateTimePickerExe.Text);
            LVI.SubItems.Add( cbExecutor.Text);
        }

        private void buttonContent_Click(object sender, EventArgs e)
        {
            AppendDocumentContent();
        }

        private void buttonSender_Click_1(object sender, EventArgs e)
        {
            EditAutor Who = new EditAutor(AMASacc, RegIdRKK, labelRKK.Text);
            Who.FormClosed += new FormClosedEventHandler(Who_FormClosed);
            Who.ShowDialog();
        }

        private void tsbAddFile_Click_1(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            if (openFileDialog1.CheckFileExists)
            {
                foreach (string fstring in openFileDialog1.FileNames)
                {
                    string[] s = new string[1];
                    s[0] = fstring.Trim();

                    dataGridView1.Rows[dataGridView1.Rows.Add()].SetValues(s);
                }
            }
        }

        private void tsbRemoveFile_Click_1(object sender, EventArgs e)
        {
            foreach (DataGridViewRow DR in dataGridView1.SelectedRows)
                try
                {
                    dataGridView1.Rows.Remove(DR);
                }
                catch { }
        }

        private void tsbScanning_Click_1(object sender, EventArgs e)
        {
            if (ScanPicters == null)
                ScanPicters = new TwainFrame();
            ScanPicters.Show();
            if (!Scanning)
            {
                ScanPicters.Scanned += new TwainFrame.ScanDoc(ScanPicters_Scanned);
                Scanning = true;
            }
        }

        TwainGui.TwainFrame ScanPicters = null;
        bool Scanning = false;

        void ScanPicters_Scanned(string Filename)
        {
            string[] s = new string[1];
            s[0] = Filename.Trim();

            dataGridView1.Rows[dataGridView1.Rows.Add()].SetValues(s);
        }

        private void AddContent()
        {
            CommonClass.CommonDocumentLibrary.Structure_Document();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                string fname = (string)row.Cells[0].Value;
                CommonClass.CommonDocumentLibrary.SaveDocument(fname); 
            }
            string FileName = (string)Path.GetTempPath() + "Newdoc" + RegIdRKK.ToString() + ".xml";
            CommonClass.CommonDocumentLibrary.CloseDocument (FileName);

            AMAS_DBI.AMASCommand.Append_Content(RegIdRKK, CommonValues.CommonClass.GetImage(CommonValues.CommonClass.SaveFilewithHead(FileName)));
            File.Delete(FileName);

            JrnFormular.Save_formular(RegIdRKK);
        }

        private void listViewNewDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            foreach (ListViewItem item in listViewNewDocs.SelectedItems)
            {
                document_Show.Doc_ID = (int)Convert.ToInt32(item.Name.Substring(3));
                document_Show.Edit_document = false;
                document_Show.Refresh();
            }
        }
    }
}