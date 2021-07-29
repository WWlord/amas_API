using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AMAS_Query;
using CommonValues;
using ClassPattern;
using System.Drawing;
using AMAS_DBI;
using ClassDocuments.baseLayer;

namespace ClassDocuments
{
    public class MetaDataDocument
    {
        private int DocumentID = 0;
        private int DocumentTIP = -1;
        TabControl MetaDataView;
        Formularing Formular;
        private ToolStrip tsFormular;
        private Panel ForPanel;
        private ToolStrip tsExecutor;
        private Panel ExePanel;

        public delegate void PickBackDocument();
        public event PickBackDocument BackDocumentPicked;

        public delegate void PickSubDocument(int id);
        public event PickSubDocument SubDocumentPicked;

        public delegate void PickTaskList(bool Own);
        public event PickTaskList TaskListPicked;

        public delegate void PickTaskSend(int Task, int TSN);
        public event PickTaskSend PickTaskSended;

        private MetadataSetting MySetting = new MetadataSetting();

        public int DocTema
        { get { return tema; } }

        public int DocKind
        { get { return kind; } }
    
        public int DOID
        {
            get { return DocumentID; }
            set
            {
                Events(DocumentID);
                if (DocumentID == 0 && value > 0) Events(value);
                DocumentID = value;
                if(DocumentID>0) Requery();
                else NewDocument();
            }
        }
        public int DTIP
        {
            get { return DocumentTIP; }
            set { DocumentTIP = value; }//if (DocumentID > 0) Requery(); }
        }

        public int DSND
        {
            get { return chief; }
            set { chief = value; }
        }

        public string KindOfDocument
        {
            get { return documentKind; }
        }

        public string DocumentNumber
        {
            get { return findCod; }
        }

        private AMAS_DBI.Class_syb_acc AMASacc;

        private int typist = 0;
        private DateTime date_reg = DateTime.MinValue;
        private int kind = 0;
        private int tema = 0;
        private int lists = 0;
        private int security = 0;
        private string annotation = "";
        private int chief = 0;
        private string findCod = "";
        private string documentKind = "";

        private System.Windows.Forms.TextBox textBoxAnnotation=null;
        private System.Windows.Forms.ListBox textBoxCommon=null;
        private System.Windows.Forms.ListView listBoxExecutor = null;
        public System.Windows.Forms.ListView ExecList { get { return listBoxExecutor; } }
        private TabPage PgAnnotation ;
        private TabPage PgCommon ;
        private TabPage PgFormular ;
        private TabPage PgNotes ;
        private TabPage PgExecutor ;

        public string annot { get { if (textBoxAnnotation != null) return textBoxAnnotation.Text.Trim(); else return ""; } }

        private void pages()
        {
            MetaDataView.TabPages.Clear();
            PgAnnotation = new TabPage("Аннотация");
            PgCommon = new TabPage("Сведения");
            PgFormular = new TabPage("Формуляр");
            PgNotes = new TabPage("Заметки");
            PgExecutor= new TabPage("Список исполнителей");

            PgAnnotation.Name = "PgAnnotation";
            PgCommon.Name = "PgCommon";
            PgFormular.Name = "PgFormular";
            PgNotes.Name = "PgNotes";
            PgExecutor.Name="PgExecutor";
        }

        private bool Editing = false;

        public bool Editable
        {
            get { return Editing; }
            set
            {
                switch(value)
                {
                    case false:
                        if (Editing)
                        {
                            Requery();
                        }
                        break;
                    case true:
                        if (!Editing)
                        {
                            string annot = annotation;
                            NewDocument();
                            annotation = annot;
                            this.textBoxAnnotation.Text = annotation;
                        }
                        break;
                }
                Editing=value;
            }
        }

        public MetaDataDocument(TabControl MDV, AMAS_DBI.Class_syb_acc ACC)
        {
            AMASacc = ACC;
            MetaDataView = MDV;
            pages();
            DocumentTIP = 0;
            DocumentID = 0;
            //NewDocument();
        }

        public MetaDataDocument(int document, int TIP, TabControl MDV, AMAS_DBI.Class_syb_acc ACC)
        {
            AMASacc = ACC;
            MetaDataView = MDV;
            pages();
            DocumentTIP = TIP;
            DocumentID = document;
           //Requery();
        }

        public MetaDataDocument(int document, int TIP, TabControl MDV, AMAS_DBI.Class_syb_acc ACC, int sender)
        {
            chief = sender;
            AMASacc = ACC;
            MetaDataView = MDV;
            pages();
            DocumentTIP = TIP;
            DocumentID = document;
            //Requery();
        }

        private void Events(int docId)
        {
            if (docId > 0)
                if(PgNotes!=null)
                    foreach(Editor EDt in PgNotes.Controls)
                        if (EDt.Edited)
                            AMAS_DBI.AMASCommand.Denote_of_document_write(docId, EDt.RTF, EDt.TEXT);
        }

        private Panel AssignToolsPanel(ref Panel APanel, string APanelname, ref ToolStrip AToolStrip, string AToolStripname, ref ListView AListView, string AListViewname)
        {
            //
            //APanel
            //
            APanel = new Panel();
            APanel.Dock = DockStyle.Fill;
            APanel.Visible = true;
            APanel.Name = APanelname;
            APanel.Location = new System.Drawing.Point(3, 3);
            APanel.TabIndex = 0;
            APanel.BackColor = System.Drawing.Color.Aquamarine;
            // 
            // AToolStrip
            // 
            AToolStrip = new ToolStrip();
            AToolStrip.Name = AToolStripname;
            AToolStrip.Dock = DockStyle.Right;
            AToolStrip.TabIndex = 1;
            AToolStrip.Visible = true;
            AToolStrip.BackColor = System.Drawing.Color.White;
            AToolStrip.ForeColor = System.Drawing.Color.White;
            APanel.Controls.Add(AToolStrip);

            if (AListView != null)
            {
                // 
                // AListView
                // 
                AListView = new System.Windows.Forms.ListView();
                AListView.Dock = System.Windows.Forms.DockStyle.Fill;
                AListView.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                AListView.Location = new System.Drawing.Point(3, 3);
                AListView.Name = AListViewname;
                AListView.Size = new System.Drawing.Size(616, 349);
                AListView.View = View.Details;
                AListView.TabIndex = 2;
                AListView.Visible = true;
                APanel.Controls.Add(AListView);
                AListView.Items.Clear();
            }
            AToolStrip.BringToFront();
            return APanel;
        }

        public void NewDocument()
        {
            if (!MetaDataView.TabPages.Contains(PgAnnotation))
                MetaDataView.TabPages.Add(PgAnnotation);
            PgAnnotation.Controls.Clear();
            // 
            // textBoxAnnotation
            // 
            this.textBoxAnnotation = new System.Windows.Forms.TextBox();
            this.textBoxAnnotation.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxAnnotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.textBoxAnnotation.Location = new System.Drawing.Point(3, 3);
            this.textBoxAnnotation.Multiline = true;
            this.textBoxAnnotation.Name = "textBoxAnnotation";
            this.textBoxAnnotation.Size = new System.Drawing.Size(616, 349);
            this.textBoxAnnotation.TabIndex = 0;
            PgAnnotation.Controls.Add(this.textBoxAnnotation);

            this.textBoxAnnotation.Enabled = true;

            if (!MetaDataView.TabPages.Contains(PgFormular))
                MetaDataView.TabPages.Add(PgFormular);
            else PgFormular.Controls.Clear();
            if (!PgFormular.Controls.Contains(ForPanel))
            {
                ListView lv = null;
                PgFormular.Controls.Add(AssignToolsPanel(ref ForPanel, "ForPanel", ref tsFormular, "tsFormular", ref lv, ""));
            }
            Formular = new Formularing(ForPanel);
        }

        
        public void Requery()
        {
            Common_DataFormats();
            get_common();
            Get_Annotation();
            get_formular();
            get_Notes();
            Get_Executors();
        }

        string OName = "";

        private void Common_DataFormats()
        {
            typist = 0;
            date_reg = DateTime.MinValue;
            kind = 0;
            lists = 0;
            security = 0;
            annotation = "";
            if (AMASacc.Set_table("TMDDoc4", AMAS_Query.Class_AMAS_Query.MetadataCommon(DocumentID), null))
            {
                try
                {
                    AMASacc.Get_row(0);
                    AMASacc.Find_Field("annot");
                    OName = AMASacc.get_current_Field().GetType().ToString();
                    if (OName.CompareTo("System.DBNull") != 0)
                        annotation = (string)AMASacc.get_current_Field();

                    AMASacc.Find_Field("Find_cod");
                    OName = AMASacc.get_current_Field().GetType().ToString();
                    if (OName.CompareTo("System.DBNull") != 0)
                        findCod = (string)AMASacc.get_current_Field();

                    AMASacc.Find_Field("typist");
                    OName = AMASacc.get_current_Field().GetType().ToString();
                    if (OName.CompareTo("System.DBNull") != 0)
                        typist = (int)AMASacc.get_current_Field();

                    AMASacc.Find_Field("date_f");
                    OName = AMASacc.get_current_Field().GetType().ToString();
                    if (OName.CompareTo("System.DBNull") != 0)
                        date_reg = (DateTime)AMASacc.get_current_Field();

                    AMASacc.Find_Field("kind");
                    OName = AMASacc.get_current_Field().GetType().ToString();
                    if (OName.CompareTo("System.DBNull") != 0)
                        kind = (int)AMASacc.get_current_Field();

                    AMASacc.Find_Field("lists");
                    OName = AMASacc.get_current_Field().GetType().ToString();
                    if (OName.CompareTo("System.DBNull") != 0)
                        lists = (int)AMASacc.get_current_Field();

                    AMASacc.Find_Field("security");
                    OName = AMASacc.get_current_Field().GetType().ToString();
                    if (OName.CompareTo("System.DBNull") != 0)
                        security = (int)AMASacc.get_current_Field();
                }

                catch (Exception e)
                {
                    AMASacc.EBBLP.AddError(e.Message, "Metadata - 1", e.StackTrace);
                }
                AMASacc.ReturnTable();
            }
        }

        private void Get_Annotation()
        {
            if (MetaDataView.TabPages.ContainsKey(PgAnnotation.Name) == false)
            {
                MetaDataView.TabPages.Add(PgAnnotation);
                // 
                // textBoxAnnotation
                // 
                this.textBoxAnnotation = new System.Windows.Forms.TextBox();
                this.textBoxAnnotation.Dock = System.Windows.Forms.DockStyle.Fill;
                this.textBoxAnnotation.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                this.textBoxAnnotation.Location = new System.Drawing.Point(3, 3);
                this.textBoxAnnotation.Multiline = true;
                this.textBoxAnnotation.Name = "textBoxAnnotation";
                this.textBoxAnnotation.Size = new System.Drawing.Size(616, 349);
                this.textBoxAnnotation.TabIndex = 0;
                PgAnnotation.Controls.Add(this.textBoxAnnotation);
            }
            else this.textBoxAnnotation.Text = "";
            this.textBoxAnnotation.Text = annotation;
       }
        
        private void get_formular()
        {
            if (!MetaDataView.TabPages.Contains(PgFormular))
                MetaDataView.TabPages.Add(PgFormular);
            else PgFormular.Controls.Clear();
            if (!PgFormular.Controls.Contains(ForPanel))
            {
                ListView lv = null;
                PgFormular.Controls.Add(AssignToolsPanel(ref ForPanel, "ForPanel", ref tsFormular, "tsFormular", ref lv, ""));
            }
            Formular = new Formularing(ForPanel, DocumentID, AMASacc);
        }

        private void get_Notes()
        {
            if (!MetaDataView.TabPages.ContainsKey(PgNotes.Name))
            {
                Editor Edt = new Editor();
                Edt.Set_color(Color.LightYellow);
                MetaDataView.TabPages.Add(PgNotes);
                PgNotes.Controls.Add(Edt);
                Edt.Dock = DockStyle.Fill;
                Edt.SaveEdit += new Editor.Unfocused(Edt_SaveEdit);
            }
            foreach (Editor Et in PgNotes.Controls)
                readDenote(Et);
        }

        private void readDenote(Editor Edt)
        {
            Edt.Clear();
            if (DocumentID > 0)
                if (AMASacc.Set_table("TMDDenote1", AMAS_Query.Class_AMAS_Query.DocumentDenote(DocumentID), null))
                {
                    if (AMASacc.Rows_count > 0)
                        try
                        {
                            AMASacc.Find_Field("RTFtext");
                            string filename = CommonClass.TempDirectory;
                            int bufsize = 100000;
                            byte[] buf = new byte[bufsize];
                            Stream s = AMASacc.get_current_Stream();
                            if (filename.Substring(filename.Length - 1, 1).CompareTo("/") != 0)
                                filename += "/";
                            filename += "Denote.rtf";
                            if (File.Exists(filename)) File.Delete(filename);
                            FileStream stream = new FileStream(filename, FileMode.CreateNew, FileAccess.Write);

                            long pos = 0;
                            while (pos < (s.Length - bufsize))
                            {
                                s.Read(buf, 0, bufsize);
                                stream.Write(buf, 0, bufsize);
                                pos += bufsize;
                            }
                            bufsize = (int)(s.Length - pos);
                            s.Read(buf, 0, bufsize);
                            stream.Write(buf, 0, bufsize);
                            s.Close();
                            stream.Close();

                            Edt.LoadFromFile(filename);
                        }
                        catch (Exception e)
                        {
                            AMASacc.EBBLP.AddError(e.Message, "Metadata - 1.2", e.StackTrace);
                        }
                    else
                        this.textBoxCommon.Items.Add(findCod.Trim());
                    AMASacc.ReturnTable();
                }
        }
   
        private void Edt_SaveEdit()
        {
            Events(DocumentID);
        }

        private void get_common()
        {
            if (!MetaDataView.TabPages.ContainsKey(PgCommon.Name))
            {
                MetaDataView.TabPages.Add(PgCommon);
                // 
                // textBoxCommon
                // 
                this.textBoxCommon = new System.Windows.Forms.ListBox(); // .ListView();
                this.textBoxCommon.Dock = System.Windows.Forms.DockStyle.Fill;
                this.textBoxCommon.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                this.textBoxCommon.Location = new System.Drawing.Point(3, 3);
                this.textBoxCommon.Name = "textBoxCommon";
                this.textBoxCommon.Size = new System.Drawing.Size(616, 349);

                this.textBoxCommon.TabIndex = 1;
                PgCommon.Controls.Add(this.textBoxCommon);
            }
            else textBoxCommon.Items.Clear();
            string OName = "";
            documentKind = "";
            if (kind > 0)
                if (AMASacc.Set_table("TMDDoc5", AMAS_Query.Class_AMAS_Query.MetadataKind(kind), null))
                {
                    if (AMASacc.Rows_count > 0)
                        try
                        {
                            AMASacc.Find_Field("kind");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0)
                            {
                                this.textBoxCommon.Items.Add((string)AMASacc.get_current_Field() + " " + findCod.Trim());
                                documentKind = (string)AMASacc.get_current_Field();
                            }
                        }
                        catch (Exception e)
                        {
                            AMASacc.EBBLP.AddError(e.Message, "Metadata - 2", e.StackTrace);
                        }
                    else
                        this.textBoxCommon.Items.Add(findCod.Trim());
                    AMASacc.ReturnTable();
                }
                else
                    this.textBoxCommon.Items.Add(findCod.Trim());

            this.textBoxCommon.Items.Add("Зарегистрировано " + date_reg.ToShortDateString());

            if (chief > 0)
                if (AMASacc.Set_table("TMDDoc6", AMAS_Query.Class_AMAS_Query.MetadataComing(chief), null))
                {
                    if (AMASacc.Rows_count > 0)
                        try
                        {
                            string fio = "";
                            AMASacc.Find_Field("family");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0)
                                fio += (string)AMASacc.get_current_Field() + " ";
                            AMASacc.Find_Field("Name");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0)
                                fio += (string)AMASacc.get_current_Field() + " ";
                            AMASacc.Find_Field("father");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0)
                                fio += (string)AMASacc.get_current_Field();
                            if (fio.Trim().Length > 1)
                            {
                                this.textBoxCommon.Items.Add("Направил: " + fio);
                                AMASacc.Find_Field("signing");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0)
                                    this.textBoxCommon.Items.Add("с резолюцией: " + (string)AMASacc.get_current_Field());
                            }
                        }
                        catch (Exception e)
                        {
                            AMASacc.EBBLP.AddError(e.Message, "Metadata - 3", e.StackTrace);
                        }
                    AMASacc.ReturnTable();
                }
            switch (DocumentTIP)
            {
                case (int)CommonValues.CommonClass.TypeofDocument.Wellcome:
                case (int)CommonValues.CommonClass.TypeofDocument.Archive:
                    string sql = "";
                    if (DocumentTIP == (int)CommonValues.CommonClass.TypeofDocument.Wellcome) sql = AMAS_Query.Class_AMAS_Query.MetadataWellcome(DocumentID);
                    else sql = AMAS_Query.Class_AMAS_Query.MetadatArchive(DocumentID);
                    if (AMASacc.Set_table("TMDDoc7", sql, null))
                    {
                        if (AMASacc.Rows_count > 0)
                            try
                            {
                                AMASacc.Get_row(0);
                                string outcoming = "";

                                AMASacc.Find_Field("outcoming");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0)
                                    outcoming = ": " + (string)AMASacc.get_current_Field();
                                outcoming = outcoming.Trim();

                                AMASacc.Find_Field("date_out");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0)
                                {
                                    DateTime dt = (DateTime)AMASacc.get_current_Field();
                                    outcoming += " от " + dt.ToShortDateString();
                                }

                                if (outcoming.Trim().Length > 0)
                                    this.textBoxCommon.Items.Add("Исходящая " + outcoming);
                                int autor = 0;
                                AMASacc.Find_Field("autor");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0)
                                    autor = (int)AMASacc.get_current_Field();
                                if (autor > 0)
                                    if (AMASacc.Set_table("TMDDoc8", AMAS_Query.Class_AMAS_Query.MetadatAutor(autor), null))
                                    {
                                        if (AMASacc.Rows_count > 0)
                                            try
                                            {
                                                AMASacc.Find_Field("fio");
                                                OName = AMASacc.get_current_Field().GetType().ToString();
                                                if (OName.CompareTo("System.DBNull") != 0)
                                                {
                                                    this.textBoxCommon.Items.Add("Автор:" + (string)AMASacc.get_current_Field());
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                AMASacc.EBBLP.AddError(e.Message, "Metadata - 4", e.StackTrace);
                                            }
                                        AMASacc.ReturnTable();
                                    }
                                int enterprice = 0;
                                AMASacc.Find_Field("enterprice");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0)
                                    enterprice = (int)AMASacc.get_current_Field();
                                if (enterprice > 0)
                                    if (AMASacc.Set_table("TMDDoc9", AMAS_Query.Class_AMAS_Query.MetadataOrg(enterprice), null))
                                    {
                                        if (AMASacc.Rows_count > 0)
                                            try
                                            {
                                                AMASacc.Find_Field("full_name");
                                                OName = AMASacc.get_current_Field().GetType().ToString();
                                                if (OName.CompareTo("System.DBNull") != 0)
                                                {
                                                    this.textBoxCommon.Items.Add("Из организации: " + (string)AMASacc.get_current_Field());
                                                    //item.SubItems.Add(enterprice.ToString());
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                AMASacc.EBBLP.AddError(e.Message, "Metadata - 5", e.StackTrace);
                                            }
                                        AMASacc.ReturnTable();
                                    }
                                int leader = 0;
                                AMASacc.Find_Field("leader");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0)
                                    leader = (int)AMASacc.get_current_Field();
                                if (leader > 0)
                                    if (AMASacc.Set_table("TMDDoc10", AMAS_Query.Class_AMAS_Query.MetadataLeader(leader), null))
                                    {
                                        if (AMASacc.Rows_count > 0)
                                            try
                                            {
                                                AMASacc.Find_Field("fio");
                                                OName = AMASacc.get_current_Field().GetType().ToString();
                                                if (OName.CompareTo("System.DBNull") != 0)
                                                {
                                                    this.textBoxCommon.Items.Add("от сотрудника: " + (string)AMASacc.get_current_Field());
                                                    //item.SubItems.Add(leader.ToString());
                                                }
                                            }
                                            catch (Exception e)
                                            {
                                                AMASacc.EBBLP.AddError(e.Message, "Metadata - 7", e.StackTrace);
                                            }
                                        AMASacc.ReturnTable();
                                    }
                            }
                            catch (Exception e)
                            {
                                AMASacc.EBBLP.AddError(e.Message, "Metadata - 8", e.StackTrace);
                            }
                        AMASacc.Find_Field("tema");
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0)
                            tema = (int)AMASacc.get_current_Field();
                        else tema = 0;
                        if (tema > 0)
                            if (AMASacc.Set_table("TMDDoc11", AMAS_Query.Class_AMAS_Query.MetadataTema(tema), null))
                            {
                                if (AMASacc.Rows_count > 0)
                                    try
                                    {
                                        AMASacc.Find_Field("description_");
                                        OName = AMASacc.get_current_Field().GetType().ToString();
                                        if (OName.CompareTo("System.DBNull") != 0)
                                        {
                                            this.textBoxCommon.Items.Add("Тема: " + (string)AMASacc.get_current_Field());
                                            //item.SubItems.Add(tema.ToString());
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        AMASacc.EBBLP.AddError(e.Message, "Metadata - 9", e.StackTrace);
                                    }
                                AMASacc.ReturnTable();
                            }
                        int coming = 0;
                        AMASacc.Find_Field("coming");
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0)
                            coming = (int)AMASacc.get_current_Field();
                        if (coming > 0)
                            if (AMASacc.Set_table("TMDDoc1", AMAS_Query.Class_AMAS_Query.MetadataComing(coming), null))
                            {
                                if (AMASacc.Rows_count > 0)
                                    try
                                    {
                                        AMASacc.Find_Field("coming");
                                        OName = AMASacc.get_current_Field().GetType().ToString();
                                        if (OName.CompareTo("System.DBNull") != 0)
                                        {
                                            this.textBoxCommon.Items.Add("Доставка: " + (string)AMASacc.get_current_Field());
                                            //item.SubItems.Add(coming.ToString());
                                        }
                                    }
                                    catch (Exception e)
                                    {
                                        AMASacc.EBBLP.AddError(e.Message, "Metadata - 10", e.StackTrace);
                                    }
                                AMASacc.ReturnTable();
                            }
                    }
                    break;
                case (int)CommonValues.CommonClass.TypeofDocument.Indoor:
                    if (AMASacc.Set_table("TMDDoc2", AMAS_Query.Class_AMAS_Query.MetadataIndoor(DocumentID), null))
                    {
                        if (AMASacc.Rows_count > 0)
                            try
                            {
                                //AMASacc.Find_Field("when_m");
                                //OName = AMASacc.get_current_Field().GetType().ToString();
                                //if (OName.CompareTo("System.DBNull") != 0)
                                //    this.textBoxCommon.Items.Add("Срок исполнения: " + (string)Convert.ToString((DateTime)AMASacc.get_current_Field()));

                                AMASacc.Find_Field("tema");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0)
                                    tema = (int)AMASacc.get_current_Field();
                                else tema = 0;

                                int employee = 0;
                                AMASacc.Find_Field("employee");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0)
                                    employee = (int)AMASacc.get_current_Field();
                                if (employee > 0)
                                    if (AMASacc.Set_table("TMDDoc3", AMAS_Query.Class_AMAS_Query.MetadataEmployee(employee), null))
                                    {
                                        if (AMASacc.Rows_count > 0)
                                            try
                                            {
                                                string fio = "";
                                                AMASacc.Find_Field("family");
                                                OName = AMASacc.get_current_Field().GetType().ToString();
                                                if (OName.CompareTo("System.DBNull") != 0)
                                                    fio += (string)AMASacc.get_current_Field() + "";
                                                AMASacc.Find_Field("name");
                                                OName = AMASacc.get_current_Field().GetType().ToString();
                                                if (OName.CompareTo("System.DBNull") != 0)
                                                    fio += (string)AMASacc.get_current_Field() + "";
                                                AMASacc.Find_Field("father");
                                                OName = AMASacc.get_current_Field().GetType().ToString();
                                                if (OName.CompareTo("System.DBNull") != 0)
                                                    fio += (string)AMASacc.get_current_Field() + "";
                                                this.textBoxCommon.Items.Add("Автор: " + fio);
                                            }
                                            catch (Exception e)
                                            {
                                                AMASacc.EBBLP.AddError(e.Message, "Metadata - 11", e.StackTrace);
                                            }
                                        AMASacc.ReturnTable();
                                    }
                            }
                            catch (Exception e)
                            {
                                AMASacc.EBBLP.AddError(e.Message, "Metadata - 12", e.StackTrace);
                            }
                        AMASacc.ReturnTable();
                    }
                    break;
                case (int)CommonValues.CommonClass.TypeofDocument.Outdoor:
                    break;
            }
            AMASacc.ReturnTable();
        }
        
        public void save_document(int DocId)
        {
            Formular.Save_formular(DocId);
            Events(DocId); 
        }

        private ColumnHeader columnHeaderImage;
        private ColumnHeader columnHeaderExecutor;
        private ColumnHeader columnHeaderGet;
        private ColumnHeader columnHeaderTo;
        private ColumnHeader columnHeaderExet;
        private ColumnHeader columnHeaderNote;
        private ColumnHeader columnHeaderExetEmployee;
        private ColumnHeader columnHeaderIssueDocId;
        private ColumnHeader columnHeaderMoving;
        private ToolStripButton tsiKillAnswer;
        private ToolStripButton tsiKillSend;
        private ToolStripButton tsiMainExecutor;
        private ToolStripButton tsiOwnSings;
        private ToolStripButton tsiSubDocument;
        private ToolStripButton tsiBackDocument;
        private ToolStripButton tsiAlterTime;

        private int[] UnvisibleColumns = null;

        public void Get_Executors()
        {
            if (!MetaDataView.TabPages.ContainsKey(PgExecutor.Name))
            {
                MetaDataView.TabPages.Add(PgExecutor);
                // 
                // ListBoxExecutops
                // 
                listBoxExecutor = new System.Windows.Forms.ListView();
                listBoxExecutor.SuspendLayout();
                listBoxExecutor.Dock = System.Windows.Forms.DockStyle.Fill;
                listBoxExecutor.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
                listBoxExecutor.Location = new System.Drawing.Point(3, 3);
                listBoxExecutor.Name = "listBoxExecutor";
                listBoxExecutor.Size = new System.Drawing.Size(616, 349);
                listBoxExecutor.LabelEdit = false;
                listBoxExecutor.MultiSelect = false;
                listBoxExecutor.UseCompatibleStateImageBehavior = false;
                listBoxExecutor.ShowGroups = true;
                listBoxExecutor.View = System.Windows.Forms.View.Details;
                listBoxExecutor.GridLines = true;
                listBoxExecutor.SmallImageList =new ImageList();
                listBoxExecutor.SmallImageList.Images.Add("task", (System.Drawing.Icon)ClassDocuments.Resources.task);
                listBoxExecutor.SmallImageList.Images.Add("mainExecutor", (System.Drawing.Icon)ClassDocuments.Resources.mainExe);
                listBoxExecutor.ItemChecked += new ItemCheckedEventHandler(listBoxExecutor_ItemChecked);
                PgExecutor.Controls.Add(AssignToolsPanel(ref ExePanel, "ExePanel", ref tsExecutor, "tsExecutor", ref listBoxExecutor, "listBoxExecutor"));

                listBoxExecutor.Items.Clear();
                listBoxExecutor.Columns.Clear();
                columnHeaderImage = new System.Windows.Forms.ColumnHeader();
                columnHeaderExecutor = new System.Windows.Forms.ColumnHeader();
                columnHeaderGet = new System.Windows.Forms.ColumnHeader();
                columnHeaderTo = new System.Windows.Forms.ColumnHeader();
                columnHeaderExet = new System.Windows.Forms.ColumnHeader();
                columnHeaderNote = new System.Windows.Forms.ColumnHeader();
                columnHeaderExetEmployee = new System.Windows.Forms.ColumnHeader();
                columnHeaderIssueDocId = new System.Windows.Forms.ColumnHeader();
                columnHeaderMoving = new System.Windows.Forms.ColumnHeader();
                listBoxExecutor.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
 //                   columnHeaderImage,
             columnHeaderExecutor,
             columnHeaderGet,
             columnHeaderTo,
             columnHeaderExet,
             columnHeaderNote,
             columnHeaderExetEmployee,
                    columnHeaderIssueDocId,
                    columnHeaderMoving
                });
                // 
                // columnHeaderImage
                // 
                columnHeaderImage.Name = "columnHeaderImage";
                columnHeaderImage.Text = "";
                columnHeaderImage.Width = ClassDocuments.Resources.mainExe.Width + 1;
                // 
                // columnHeaderExecutor
                // 
                columnHeaderExecutor.Name = "columnHeaderExecutor";
                columnHeaderExecutor.Text = "Исполнитель";
                columnHeaderExecutor.Width = listBoxExecutor.Width / 4;
                // 
                // columnHeaderGet
                // 
                columnHeaderGet.Name = "columnHeaderGet";
                columnHeaderGet.Text = "направлено";
                columnHeaderGet.Width = listBoxExecutor.Width / 10;
                // 
                // columnHeaderTo
                // 
                columnHeaderTo.Name = "columnHeaderTo";
                columnHeaderTo.Text = "срок до";
                columnHeaderTo.Width = listBoxExecutor.Width / 10;
                // 
                // columnHeaderExet
                // 
                columnHeaderExet.Name = "columnHeaderExet";
                columnHeaderExet.Text = "исполнено";
                columnHeaderExet.Width = listBoxExecutor.Width / 10;

                // 
                // columnHeaderNote
                // 
                columnHeaderNote.Name = "columnHeaderNote";
                columnHeaderNote.Text = "резолюция";
                columnHeaderNote.Width = listBoxExecutor.Width / 4;

                // 
                // columnHeaderExetEmployee
                // 
                columnHeaderExetEmployee.Name = "columnHeaderExetEmployee";
                columnHeaderExetEmployee.Text = "исполнил";
                columnHeaderExetEmployee.Width = listBoxExecutor.Width / 5;

                // 
                // columnHeaderIssueDocId
                // 
                columnHeaderIssueDocId.Name = "columnHeaderIssueDocId";
                columnHeaderIssueDocId.Width = 0;
                columnHeaderIssueDocId.AutoResize(ColumnHeaderAutoResizeStyle.None);
                if (UnvisibleColumns == null)
                {
                    UnvisibleColumns = new int[1];
                    UnvisibleColumns[0] = columnHeaderIssueDocId.Index;
                }
                else
                {
                    int[] ucl = new int[UnvisibleColumns.Length + 1];
                    UnvisibleColumns.CopyTo(ucl, 0);
                    ucl[UnvisibleColumns.Length] = columnHeaderIssueDocId.Index;
                    UnvisibleColumns = ucl;
                }

                // 
                // columnHeaderMoving
                // 
                columnHeaderMoving.Name = "columnHeaderMoving";
                columnHeaderMoving.Width = 0;
                columnHeaderMoving.AutoResize(ColumnHeaderAutoResizeStyle.None);
                if (UnvisibleColumns == null)
                {
                    UnvisibleColumns = new int[1];
                    UnvisibleColumns[0] = columnHeaderMoving.Index;
                }
                else
                {
                    int[] ucl = new int[UnvisibleColumns.Length + 1];
                    UnvisibleColumns.CopyTo(ucl, 0);
                    ucl[UnvisibleColumns.Length] = columnHeaderMoving.Index;
                    UnvisibleColumns = ucl;
                }

                //listBoxExecutor.ColumnWidthChanging += new ColumnWidthChangingEventHandler(listBoxExecutor_ColumnWidthChanging);

                //
                // tsiKillAnswer
                //
                this.tsiKillAnswer = new System.Windows.Forms.ToolStripButton();
                this.tsiKillAnswer.BackColor = System.Drawing.Color.Tan;
                this.tsiKillAnswer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsiKillAnswer.Image = ((System.Drawing.Image)ClassDocuments.Resources.KillAnswer);
                this.tsiKillAnswer.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsiKillAnswer.Name = "tsiKillAnswer";
                this.tsiKillAnswer.Size = new System.Drawing.Size(23, 22);
                this.tsiKillAnswer.Text = "Удалить ответ";
                this.tsiKillAnswer.ToolTipText = "Удалить ответ";
                this.tsiKillAnswer.Visible = true;
                this.tsiKillAnswer.Click += new EventHandler(tsiKillAnswer_click);
                //
                // tsiKillSend
                //
                this.tsiKillSend = new System.Windows.Forms.ToolStripButton();
                this.tsiKillSend.BackColor = System.Drawing.Color.Tan;
                this.tsiKillSend.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsiKillSend.Image = ((System.Drawing.Image)ClassDocuments.Resources.CUT);
                this.tsiKillSend.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsiKillSend.Name = "tsiKillAnswer";
                this.tsiKillSend.Size = new System.Drawing.Size(23, 22);
                this.tsiKillSend.Text = "Удалить рассылку";
                this.tsiKillSend.ToolTipText = "Удалить рассылку";
                this.tsiKillSend.Visible = true;
                this.tsiKillSend.Click += new EventHandler(tsiKillSend_Click);
                //
                // tsiMainExecutor
                //
                this.tsiMainExecutor = new System.Windows.Forms.ToolStripButton();
                this.tsiMainExecutor.BackColor = System.Drawing.Color.Tan;
                this.tsiMainExecutor.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsiMainExecutor.Image = ((System.Drawing.Image)ClassDocuments.Resources.MainExecutor);
                this.tsiMainExecutor.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsiMainExecutor.Name = "tsiMainExecutor";
                this.tsiMainExecutor.Size = new System.Drawing.Size(23, 22);
                this.tsiMainExecutor.Text = "Главный исполнитель";
                this.tsiMainExecutor.ToolTipText = "Главный исполнитель";
                this.tsiMainExecutor.Visible = true;
                this.tsiMainExecutor.Click += new System.EventHandler(tsiMainExecutor_Click);
                //
                // tsiOwnSings
                //
                this.tsiOwnSings = new System.Windows.Forms.ToolStripButton();
                this.tsiOwnSings.BackColor = System.Drawing.Color.Tan;
                this.tsiOwnSings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsiOwnSings.Image = ((System.Drawing.Image)ClassDocuments.Resources.OwnSings);
                this.tsiOwnSings.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsiOwnSings.Name = "tsiOwnSings";
                this.tsiOwnSings.Size = new System.Drawing.Size(23, 22);
                this.tsiOwnSings.Text = "Свои рассылки";
                this.tsiOwnSings.ToolTipText = "Свои рассылки";
                this.tsiOwnSings.Visible = true;
                this.tsiOwnSings.CheckOnClick = true;
                this.tsiOwnSings.Click += new System.EventHandler(tsiOwnSings_Click);
                //
                // tsiSubDocument
                //
                this.tsiSubDocument = new System.Windows.Forms.ToolStripButton();
                this.tsiSubDocument.BackColor = System.Drawing.Color.Tan;
                this.tsiSubDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsiSubDocument.Image = ((System.Drawing.Image)ClassDocuments.Resources.SubDocument);
                this.tsiSubDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsiSubDocument.Name = "tsiSubDocument";
                this.tsiSubDocument.Size = new System.Drawing.Size(23, 22);
                this.tsiSubDocument.Text = "Перейти к ответу";
                this.tsiSubDocument.ToolTipText = "Перейти к ответу";
                this.tsiSubDocument.Visible = true;
                this.tsiSubDocument.Click += new System.EventHandler(tsiSubDocument_Click);
                //
                // tsiBackDocument
                //
                this.tsiBackDocument = new System.Windows.Forms.ToolStripButton();
                this.tsiBackDocument.BackColor = System.Drawing.Color.Tan;
                this.tsiBackDocument.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsiBackDocument.Image = ((System.Drawing.Image)ClassDocuments.Resources.LastDocument);
                this.tsiBackDocument.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsiBackDocument.Name = "tsiBackDocument";
                this.tsiBackDocument.Size = new System.Drawing.Size(23, 22);
                this.tsiBackDocument.Text = "Вернуться к предыдущему документу";
                this.tsiBackDocument.ToolTipText = "Вернуться к предыдущему документу";
                this.tsiBackDocument.Visible = true;
                this.tsiBackDocument.Click += new System.EventHandler(tsiBackDocument_Click);
                //
                // tsiAlterTime
                //
                this.tsiAlterTime = new System.Windows.Forms.ToolStripButton();
                this.tsiAlterTime.BackColor = System.Drawing.Color.Tan;
                this.tsiAlterTime.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                this.tsiAlterTime.Image = ((System.Drawing.Image)ClassDocuments.Resources.AlterExeTime);
                this.tsiAlterTime.ImageTransparentColor = System.Drawing.Color.Magenta;
                this.tsiAlterTime.Name = "tsiAlterTime";
                this.tsiAlterTime.Size = new System.Drawing.Size(23, 22);
                this.tsiAlterTime.Text = "Изменить срок исполнения";
                this.tsiAlterTime.ToolTipText = "Изменить срок исполнения";
                this.tsiAlterTime.Visible = true;
                this.tsiAlterTime.Click += new System.EventHandler(tsiAlterTime_Click);

                this.tsExecutor.Items.Clear();
                this.tsExecutor.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tsiOwnSings,
                    this.tsiKillAnswer,
                    this.tsiKillSend,
                    this.tsiAlterTime,
                    this.tsiMainExecutor,
                    this.tsiSubDocument,
                    this.tsiBackDocument
                });
                listBoxExecutor.ResumeLayout();
            }
            if (!PgExecutor.Controls.Contains(ExePanel))
                PgExecutor.Controls.Add(AssignToolsPanel(ref ExePanel, "ExePanel", ref tsExecutor, "tsExecutor", ref listBoxExecutor, "listBoxExecutor"));


            getSetting();

        }

        void listBoxExecutor_ItemChecked(object sender, ItemCheckedEventArgs e)
        {

        }

        void getSetting()
        {
            MySetting.SettingsKey = "Executors";
            columnHeaderExecutor.Width = MySetting.Executor;
            columnHeaderGet.Width = MySetting.SendDate;
            columnHeaderTo.Width = MySetting.ToDate;
            columnHeaderExet.Width = MySetting.ExeDate;
            columnHeaderNote.Width = MySetting.Note;
            columnHeaderExetEmployee.Width = MySetting.ExecBy;
            listBoxExecutor.ColumnWidthChanged += new ColumnWidthChangedEventHandler(listBoxExecutor_ColumnWidthChanged);
        }


        public void Close()
        {
            SaveSetting();
        }

        private void SaveSetting()
        {
            MySetting.Executor = columnHeaderExecutor.Width;
            MySetting.SendDate = columnHeaderGet.Width;
            MySetting.ToDate = columnHeaderTo.Width;
            MySetting.ExeDate = columnHeaderExet.Width;
            MySetting.Note = columnHeaderNote.Width;
            MySetting.ExecBy = columnHeaderExetEmployee.Width;
            MySetting.Save();
        }

        void tsiKillSend_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBoxExecutor.SelectedItems.Count; i++)
                try
                {
                    int EXEVizaNew = (int)Convert.ToInt32(listBoxExecutor.SelectedItems[i].SubItems[7].Text);
                    switch (listBoxExecutor.SelectedItems[i].Group.Name)
                    {
                        case "ExeGroup":
                            if (AMAS_DBI.AMASCommand.KillSend(EXEVizaNew))
                            {
                                listBoxExecutor.SelectedItems[i].Remove();
                                PickTaskSended(EXEVizaNew, 1);
                            }
                            break;
                        case "VizaGroup":
                            if (AMAS_DBI.AMASCommand.KillSendViza(EXEVizaNew))
                            {
                                listBoxExecutor.SelectedItems[i].Remove();
                                PickTaskSended(EXEVizaNew, 2);
                            }
                            break;
                        case "NewsGroup":
                            if (AMAS_DBI.AMASCommand.KillSendNews(EXEVizaNew))
                            {
                                listBoxExecutor.SelectedItems[i].Remove();
                                PickTaskSended(EXEVizaNew, 3);
                            }
                            break;
                    }
                }
                catch
                { }
        }

        private bool IsReWidetColumn = true;

        void listBoxExecutor_ColumnWidthChanged(object sender, ColumnWidthChangedEventArgs e)
        {
            if (IsReWidetColumn)
                if (e != null)
                    if (e.ColumnIndex >= 0)
                    {
                        int ColInd = e.ColumnIndex;
                        if (UnvisibleColumns != null)
                            try
                            {
                                for (int i = 0; i < UnvisibleColumns.Length; i++)
                                    if (ColInd == UnvisibleColumns[i])
                                    {
                                        bool isit = false;
                                        for (int l = ColInd - 1; l >= 0; l--)
                                        {
                                            isit = false;
                                            for (int k = 0; k < UnvisibleColumns.Length; k++)
                                            {
                                                if (!isit)
                                                    if (l == UnvisibleColumns[k])
                                                    {
                                                        isit = true;
                                                        break;
                                                    }
                                            }
                                            if (!isit)
                                                try
                                                {
                                                    IsReWidetColumn = false;
                                                    listBoxExecutor.Columns[l].Width += listBoxExecutor.Columns[ColInd].Width;
                                                    listBoxExecutor.Columns[ColInd].Width = 0;
                                                    goto iswidet;
                                                }
                                                catch { }
                                        }
                                    }
                            }
                            catch { }
                    iswidet:
                        IsReWidetColumn = true;
                    }
                    else IsReWidetColumn = true;
            SaveSetting();
        }

        private void tsiKillAnswer_click(object sender, EventArgs e)
        {
            for (int i = 0; i < listBoxExecutor.SelectedItems.Count; i++)
                try
                {
                    if (listBoxExecutor.SelectedItems[i].Group.Name.CompareTo("ExeGroup") == 0)
                        AMAS_DBI.AMASCommand.KillAnswer((int)Convert.ToInt32(listBoxExecutor.SelectedItems[i].SubItems[7].Text));
                }
                catch
                { }
        }

        private void tsiMainExecutor_Click(object sender, EventArgs e)
        {
            for( int i=0;i< listBoxExecutor.SelectedItems.Count;i++)
                try
                {
                    if (listBoxExecutor.SelectedItems[i].Group.Name.CompareTo("ExeGroup") == 0)
                        AMAS_DBI.AMASCommand.SetMainExecutor((int)Convert.ToInt32(listBoxExecutor.SelectedItems[i].SubItems[7].Text));
                }
                catch { }
        }

        private void tsiOwnSings_Click(object sender, EventArgs e)
        {
            if (TaskListPicked != null)
            {
                listBoxExecutor.Items.Clear();
                if (tsiOwnSings.Checked)
                    TaskListPicked(true);
                else
                    TaskListPicked(false);
            }
        }

        private void tsiSubDocument_Click(object sender, EventArgs e)
        {
            ListViewItem ItemInd = null;
            try
            {
                if (this.ExecList.SelectedItems.Count > 0) ItemInd = this.ExecList.SelectedItems[0];
                if (ItemInd != null)
                    if (ItemInd.Group.Name.CompareTo("ExeGroup") == 0)
                    {
                        int id = Convert.ToInt32(ItemInd.SubItems[6].Text);
                        if (id > 0) SubDocumentPicked(id);
                    }

            }
            catch { }
        }

        private void tsiBackDocument_Click(object sender, EventArgs e)
        {
            BackDocumentPicked();
        }

        private void tsiAlterTime_Click(object sender, EventArgs e)
        {
            string dat = "";
            ListViewItem ItemInd = null;

            if (this.ExecList.SelectedItems.Count > 0)
            {
                ItemInd = this.ExecList.SelectedItems[0];
                dat = this.ExecList.SelectedItems[0].SubItems[2].Text;
            }

            if (ItemInd != null)
                if (ItemInd.Group.Name.CompareTo("ExeGroup") == 0)
                {
                System.Windows.Forms.MonthCalendar monthCalendarExe;
                monthCalendarExe = new System.Windows.Forms.MonthCalendar();
                listBoxExecutor.Parent.Controls.Add(monthCalendarExe);
                // 
                // monthCalendarExe
                // 
                monthCalendarExe.Location = new System.Drawing.Point((listBoxExecutor.Parent.Width - monthCalendarExe.Width) / 2, 20);
                monthCalendarExe.Name = "monthCalendarExe";
                monthCalendarExe.TabIndex = 33;
                monthCalendarExe.Visible = true;
                monthCalendarExe.BringToFront();
                monthCalendarExe.SetDate(Convert.ToDateTime(dat));
                monthCalendarExe.DateSelected += new DateRangeEventHandler(monthCalendarExe_DateSelected);
            }
        }

        void monthCalendarExe_DateSelected(object sender, DateRangeEventArgs e)
        {
            DateTime dt = AMAS_DBI.AMASCommand.AlterDateExecuting(Convert.ToInt32(listBoxExecutor.SelectedItems[0].SubItems[7].Text), e.Start);
            if (dt != DateTime.MinValue) listBoxExecutor.SelectedItems[0].SubItems[2].Text = dt.ToShortDateString();
            System.Windows.Forms.MonthCalendar monthCalendarExe =(System.Windows.Forms.MonthCalendar) sender;
            monthCalendarExe.Visible = false;
            monthCalendarExe.Dispose();
        }

    }
}
