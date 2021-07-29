using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using ClassDocuments;
using ClassPattern;
using CommonValues;
using AMAS_DBI;
using AMASDocuments;

namespace AMASControlRegisters
{
    public partial class Document_Viewer : UserControl
    {
        public delegate void PickDocument(int Document);
        public event PickDocument DocumentPicked;

        public delegate void TipDocument(int Document);
        public event TipDocument DocumenTiped;

        public delegate void DeleteTaskId(int task,int TVN);
        public event DeleteTaskId TaskIdKilll;

        public TreeNode TNode = null;

        private TreeView FileExplorer = null;
        private FileDirExplorer FileSelect = null;
        private MetaDataDocument MDT;
        private ClassPattern.Editor DocEditor;
        private ClassDocsItem DocumentValue;
        private AMAS_DBI.Class_syb_acc SyB_Acc;

        private int documentTIP = -1;
        
        private XMLContent_document DOC_CONT;
        private int Document_ID=0;
        private int Sender_id=0;

        private string HomeDirectory = "";

        public XMLContent_document ContentOfDocument
        { get { return DOC_CONT; } }

        public bool CanEdit
        {
            get
            {
                int cnt = 0;

                if (SyB_Acc.Set_table("DVDoc10", AMAS_Query.Class_AMAS_Query.Get_movies_of_doc(Doc_ID), null))
                {
                    cnt = SyB_Acc.Rows_count;
                    SyB_Acc.ReturnTable();
                }
                if (cnt > 0) return false;

                if (SyB_Acc.Set_table("DVDoc10", AMAS_Query.Class_AMAS_Query.Get_Vizy_of_doc(Doc_ID), null))
                {
                    cnt = SyB_Acc.Rows_count;
                    SyB_Acc.ReturnTable();
                }
                if (cnt > 0) return false;
                
                return true;
            }
        }

        public int Kind
        {
            get
            {
                if (MDT != null)
                    return MDT.DocKind;
                else return 0;
            }
        }

        public int Tema
        {
            get
            {
                if (MDT != null)
                    return MDT.DocTema;
                else return 0;
            }
        }

        public int TipOfDocument
        {
            get { return documentTIP; }
        }

        public string DocumentDirectory
        {
            get
            {
                return HomeDirectory;
            }
            set
            {
                HomeDirectory = value;
                //FileSelect.setHomeDirectory(HomeDirectory);
            }
        }

        private string aPDFDirectory = "";
        public string PDFDirectory
        {
            get
            {
                return aPDFDirectory;
            }
            set
            {
                aPDFDirectory = value;
                if (DOC_CONT!=null) DOC_CONT.PDFDirectory = aPDFDirectory;
            }
        }

        public string Annotation { get { if(MDT!=null) return MDT.annot; else return ""; } }
        
        public string DocumentNumber
        {
            get
            {
                if (MDT != null)
                    return MDT.DocumentNumber;
                else return "";
            }
        }

        public int Sender
        {
            get { return Sender_id; }
            set { Sender_id = value; }
        }

        public ClassDocsItem ValueOfDocument
        {
            get { return DocumentValue; }
            set 
            {
                DocumentValue = value;
                if (DocumentValue != null)
                {
                    Doc_ID = DocumentValue.Doc_id;
                    if (DocumentValue.From_newing > 0 && !DocumentValue.Newed) 
                    {
                        AMAS_DBI.AMASCommand.Document_Newed(DocumentValue.From_newing);
                        DocumentValue.Newed = true;
                    }
                    if (DocumentValue.From_newing > 0 && !DocumentValue.Viewed) 
                    {
                        AMAS_DBI.AMASCommand.Document_Viewed(DocumentValue.From_moving);
                        DocumentValue.Viewed = true;
                    }

                }
                else
                {
                    Doc_ID = 0;
                }
            }
        }


        public int Doc_ID
        {
            get { return Document_ID; }
            set
            {
                if(value>0 && Document_ID != value)
                {
                    Document_ID = value;
                    try
                    {
                        DocumenTiped(Document_ID);
                    }
                    catch { }
                }
                else Document_ID = value;
                if (Document_ID > 0)
                {
                    try
                    {
                        load_document_content(Document_ID);
                    }
                    catch { }
                    
                    documentTIP = AMAS_DBI.AMASCommand.DocumentTip(Document_ID);
                    if (MDT == null)
                    {
                        if (Sender_id == 0)
                            MDT = new MetaDataDocument(Document_ID, documentTIP, tabMetadata, SyB_Acc);
                        else
                            MDT = new MetaDataDocument(Document_ID, documentTIP, tabMetadata, SyB_Acc, Sender_id);
                        MDT.BackDocumentPicked += new MetaDataDocument.PickBackDocument(MDT_BackDocumentPicked);
                        MDT.SubDocumentPicked += new MetaDataDocument.PickSubDocument(MDT_SubDocumentPicked);
                        MDT.TaskListPicked += new MetaDataDocument.PickTaskList(MDT_TaskListPicked);
                        MDT.PickTaskSended += new MetaDataDocument.PickTaskSend(MDT_PickTaskSended);

                        try
                        {
                            MDT.ExecList.DoubleClick -= new EventHandler(ExecList_DoubleClick);
                        }
                        catch { }
                        MDT.ExecList.DoubleClick += new EventHandler(ExecList_DoubleClick);
                    }
                    else
                    {
                        MDT.DSND = Sender_id;
                        MDT.DTIP = documentTIP;
                        MDT.DOID = Document_ID;

                        TaskVisaList(false);

                        //MDT.PickTaskSended += new MetaDataDocument.PickTaskSend(MDT_PickTaskSended);

                    }
                }
                else
                    if (!New_doc)
                    {
                        if (MDT == null)
                        {
                            MDT = new MetaDataDocument(tabMetadata, SyB_Acc);
                            MDT.BackDocumentPicked += new MetaDataDocument.PickBackDocument(MDT_BackDocumentPicked);
                            MDT.SubDocumentPicked += new MetaDataDocument.PickSubDocument(MDT_SubDocumentPicked);
                            MDT.TaskListPicked += new MetaDataDocument.PickTaskList(MDT_TaskListPicked);
                            MDT.PickTaskSended += new MetaDataDocument.PickTaskSend(MDT_PickTaskSended);
                        }
                        else MDT.DOID = Document_ID;
                        if (MDT != null)
                            if (tabDocument != null)
                                if (tabDocument.TabCount > 0)
                                    tabDocument.TabPages[0].Text = MDT.KindOfDocument.Trim() + " № " + MDT.DocumentNumber.Trim();
                    }
                    else
                    {
                        MDT = new MetaDataDocument(0, 0, tabMetadata, SyB_Acc);
                        MDT.NewDocument();
                        //MDT.BackDocumentPicked += new MetaDataDocument.PickBackDocument(MDT_BackDocumentPicked);
                        //MDT.SubDocumentPicked += new MetaDataDocument.PickSubDocument(MDT_SubDocumentPicked);
                        //MDT.TaskListPicked += new MetaDataDocument.PickTaskList(MDT_TaskListPicked);
                        MDT.PickTaskSended += new MetaDataDocument.PickTaskSend(MDT_PickTaskSended);

                    }
            }
        }

        void ExecList_DoubleClick(object sender, EventArgs e)
        {
            foreach(ListViewItem lis in MDT.ExecList.SelectedItems)
                if (lis.Group.Name.CompareTo("VizaGroup") == 0)
                if (DocVal != null)
                    MessageBox.Show(DocVal.VisaDenote((int)Convert.ToInt32(lis.SubItems[7].Text)));

        }

        void MDT_PickTaskSended(int Task, int TSN)
        {
            TaskIdKilll(Task, TSN);
        }

        ClassDocsItem DocVal;

        void TaskVisaList(bool all)
        {
            DocVal = new ClassDocsItem(SyB_Acc);
            MDT.ExecList.Items.Clear();
            MDT.ExecList.Groups.Clear();
            DocVal.Own_pass = all;
            DocVal.ListTasks(MDT.ExecList, Document_ID);
            DocVal.ListVisa(MDT.ExecList, Document_ID);
            DocVal.ListNews(MDT.ExecList, Document_ID);
            try
            {
                MDT.ExecList.DoubleClick -= new EventHandler(ExecList_DoubleClick);
            }
            catch { }
            MDT.ExecList.DoubleClick += new EventHandler(ExecList_DoubleClick);
        }

        void MDT_TaskListPicked(bool Own)
        {
                if (Own)
                    TaskVisaList (true);
                else
                    TaskVisaList (false);
        }

        void MDT_SubDocumentPicked(int id)
        {
            try
            {
                DocumentPicked(id);
            }
            catch { }
        }

        void MDT_BackDocumentPicked()
        {
            int id = AMASCommand.GetParentDoc(Document_ID);
            try
            {
                if (id > 0) DocumentPicked(id);
            }
            catch { }
        }

        public Document_Viewer(AMAS_DBI.Class_syb_acc aaa,TreeNode nod)
        {
            InitializeComponent();
            SyB_Acc = aaa;
            Document_ID = 0;
            Sender_id = 0;
            TNode = nod;
            splitContainer1.Panel1.ForeColor = Color.Orange;
            if (SyB_Acc != null)
                if (SyB_Acc.DocumentDirectory != null)
                    HomeDirectory = SyB_Acc.DocumentDirectory;
        }

        private bool New_doc = false;
        private bool Edit_doc = false;

        public bool Edit_document
        {
            get
            {
                return Edit_doc;
            }
            set
            {
                switch (value)
                {
                    case false:
                        if (Edit_doc)
                        {
                            if (FileExplorer != null)
                            {
                                TabPage tab = null;
                                for (int i = 0; i < tabDocument.TabPages.Count; i++)
                                {
                                    tab = tabDocument.TabPages[i];
                                    foreach (Control control in tab.Controls)
                                        if (control == (Control)FileExplorer)
                                        {
                                            tabDocument.TabPages.Remove(tab);
                                            break;
                                        }
                                }
                                FileExplorer = null;
                                try
                                {
                                    FileSelect.FilePicked -= new FileDirExplorer.FilePathHandler(FileSelect_FilePicked);
                                    FileSelect.Picked1 -= new FileDirExplorer.PickedHandler(File_Append);
                                    FileSelect.Picked2 -= new FileDirExplorer.PickedHandler(File_Delete);
                                    FileSelect.Picked3 -= new FileDirExplorer.PickedHandler(Editor_append);
                                }
                                catch { }
                                FileSelect = null;
                                SaveChangeDocument(Doc_ID);
                                AMASCommand.Edit_FlowDocument(Doc_ID, Annotation);
                                if (MDT != null) MDT.Editable = false;
                            }
                        }
                        break;
                    case true:
                        if (!Edit_doc)
                        {
                            string[] sss = new string[3];
                            sss[0] = "1; Вложить";
                            sss[1] = "2; Удалить";
                            sss[2] = "3; Редактор";
                            tabDocument.TabPages.Clear();
                            load_document_content(Doc_ID);
                            tabDocument.TabPages.Add("FileExplorer", "Путеводитель");
                            TabPage tab = tabDocument.TabPages["FileExplorer"];
                            FileExplorer = new TreeView();
                            tab.Controls.Add(FileExplorer);
                            FileExplorer.Dock = DockStyle.Fill;
                            FileExplorer.ImageList = imageFS;
                            FileSelect = new FileDirExplorer(FileExplorer, "*.*", sss);
                            FileSelect.setHomeDirectory(HomeDirectory);
                            tabMetadata.TabPages.Clear();
                        }
                        FileSelect.FilePicked += new FileDirExplorer.FilePathHandler(FileSelect_FilePicked);
                        FileSelect.Picked1 += new FileDirExplorer.PickedHandler(File_Append);
                        FileSelect.Picked2 += new FileDirExplorer.PickedHandler(File_Delete);
                        FileSelect.Picked3 += new FileDirExplorer.PickedHandler(Editor_append);
                        if (SyB_Acc.PDFDirectory != null) if (SyB_Acc.PDFDirectory.Trim().Length > 1) DOC_CONT.PDFDirectory = SyB_Acc.PDFDirectory;
                        if (MDT != null) MDT.Editable = true;
                        break;
                }
                Edit_doc = value;
            }
        }

        public bool New_document
        {
            get 
            { 
                return New_doc; 
            }
            set
            {
                New_doc = value;
                switch (New_doc)
                {
                    case  false:
                        if (FileExplorer != null)
                        {
                            TabPage tab = null;
                            for (int i = 0; i < tabDocument.TabPages.Count; i++)
                            {
                                tab = tabDocument.TabPages[i];
                                foreach (Control control in tab.Controls)
                                    if (control == (Control)FileExplorer)
                                    {
                                        tabDocument.TabPages.Remove(tab);
                                        break;
                                    }
                            }
                            FileExplorer = null;
                            try
                            {
                                FileSelect.FilePicked -= new FileDirExplorer.FilePathHandler(FileSelect_FilePicked);
                                FileSelect.Picked1 -= new FileDirExplorer.PickedHandler(File_Append);
                                FileSelect.Picked2 -= new FileDirExplorer.PickedHandler(File_Delete);
                                FileSelect.Picked3 -= new FileDirExplorer.PickedHandler(Editor_append);
                            }
                            catch { }
                            FileSelect = null;
                        }

                break;
                    case true:
                        //if (FileExplorer == null)
                        {
                            string [] sss= new string[3];
                            sss[0] = "1; Вложить";
                            sss[1] = "2; Удалить";
                            sss[2] = "3; Редактор";
                            tabDocument.TabPages.Clear();
                            tabDocument.TabPages.Add("FileExplorer","Путеводитель");
                            TabPage tab = tabDocument.TabPages["FileExplorer"];
                            FileExplorer = new TreeView();
                            tab.Controls.Add(FileExplorer);
                            FileExplorer.Dock = DockStyle.Fill;
                            FileExplorer.ImageList = imageFS;
                            FileSelect=new FileDirExplorer(FileExplorer,"*.*",sss);
                            FileSelect.setHomeDirectory(HomeDirectory);
                            tabMetadata.TabPages.Clear();
                        }
                        FileSelect.FilePicked+=new FileDirExplorer.FilePathHandler(FileSelect_FilePicked);
                        FileSelect.Picked1+=new FileDirExplorer.PickedHandler(File_Append);
                        FileSelect.Picked2 += new FileDirExplorer.PickedHandler(File_Delete);
                        FileSelect.Picked3 += new FileDirExplorer.PickedHandler(Editor_append);
                        DOC_CONT = new XMLContent_document(New_document);
                        if (SyB_Acc.PDFDirectory!=null) if (SyB_Acc.PDFDirectory.Trim().Length > 1) DOC_CONT.PDFDirectory = SyB_Acc.PDFDirectory;
                        break;
                }
            }
        }

        public void Editor_append()
        {
            int index = tabDocument.SelectedIndex;
            for (int i = index; i < tabDocument.TabPages.Count - 1; i++)
            {
                tabDocument.TabPages[i].Name = "Tab" + (i + 1).ToString();
                tabDocument.TabPages[i].Text = "Приложение " + (i + 1).ToString();
            }
            string tabkey = "Tab" + index.ToString();
            if (index > 0)
                tabDocument.TabPages.Insert(index, tabkey, "Приложение " + index.ToString());
            else
                tabDocument.TabPages.Insert(index, tabkey, "Документ");
            TabPage tab = tabDocument.TabPages[tabkey];
            DocEditor = new Editor();
            if (New_document) DocEditor.Editable = true;
            else DocEditor.Editable = false;
            tab.Controls.Add(DocEditor);
            // 
            // DocEditor
            // 
            this.DocEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.DocEditor.Location = new System.Drawing.Point(0, 0);
            this.DocEditor.Name = "DocEditor" + tabkey;
            this.DocEditor.Size = new System.Drawing.Size(468, 267);
            this.DocEditor.TabIndex = 0;
        }

        public void File_Append()
        {
            try
            {
                DialogResult dr = fdInsert.ShowDialog();
                SelectedFile_Append(fdInsert.FileName);
            }
            catch
            {
            }
        }

        public void SelectedFile_Append(string FileName)
        {
            int tib = 0;
            try { tib = tabDocument.SelectedIndex; }
            catch { tib = 0; }
            tabDocument.SelectedIndex = 0;
            try
            {
                    FileAppend(FileName, tib);
            }
            catch
            {
            }
        }

        //public void File_Replace(string FileName, int index)
        //{
            //DOC_CONT.ReplaceDocument( index, FileName);
        //}

        public void File_Delete()
        {
            DOC_CONT.DELDocument(tabDocument, tabDocument.SelectedTab);
        }

        public void SaveDocument(int DocId)
        {
            for (int i = 0; i < tabDocument.TabCount; i++)
                if (tabDocument.TabPages[i].Name.Substring(0, 3).CompareTo("Tab") == 0)
                {
                    try
                    {
                        foreach (WebBrowser web in tabDocument.TabPages[i].Controls)
                            try
                            {
                                web.Stop();
                                //if (web.Url != null) DOC_CONT.ADDocument(web.Url.LocalPath, i, tabDocument);
                                web.Dispose();
                            }
                            catch { }
                    }
                    catch { }
                    try
                    {
                        foreach (Editor edt in tabDocument.TabPages[i].Controls)
                            try
                            {
                                //DOC_CONT.ADDocument(edt.SaveToFile(), i, tabDocument);
                                int ret = 0;
                                DOC_CONT.SaveDocument(edt.SaveToFile(), ref ret, tabDocument.TabCount);
                            }
                            catch { }
                    }
                    catch { }
                }
            string FileName =  (string)Path.GetTempPath() + "Newdoc" + DocId.ToString() + ".xml";
            DOC_CONT.CloseDocument(FileName);

            MDT.save_document(DocId);
            AMAS_DBI.AMASCommand.Append_Content(DocId, CommonValues.CommonClass.GetImage(CommonValues.CommonClass.SaveFilewithHead(FileName)));
            File.Delete(FileName);
            for (int i = 0; i < tabDocument.TabCount - 1; i++)
                tabDocument.TabPages[i].Dispose();
        }

        public void SaveChangeDocument(int DocId)
        {
            for (int i = 0; i < tabDocument.TabCount; i++)
                if (tabDocument.TabPages[i].Name.Substring(0, 3).CompareTo("Tab") == 0)
                {
                    try
                    {
                        foreach (WebBrowser web in tabDocument.TabPages[i].Controls)
                            try
                            {
                                web.Stop();
                                //if (web.Url != null) DOC_CONT.ADDocument(web.Url.LocalPath, i, tabDocument);
                                web.Dispose();
                            }
                            catch { }
                    }
                    catch { }
                    try
                    {
                        foreach (Editor edt in tabDocument.TabPages[i].Controls)
                            try
                            {
                                //DOC_CONT.ADDocument(edt.SaveToFile(), i, tabDocument);
                                int ret = 0;
                                DOC_CONT.SaveDocument(edt.SaveToFile(), ref ret, tabDocument.TabCount);
                            }
                            catch { }
                    }
                    catch { }
                }
            string FileName = (string)Path.GetTempPath() + "Newdoc" + DocId.ToString() + ".xml";
            DOC_CONT.CloseDocument(FileName);

            MDT.save_document(DocId);
            AMAS_DBI.AMASCommand.Edit_Content(DocId, CommonValues.CommonClass.GetImage(CommonValues.CommonClass.SaveFilewithHead(FileName)));
            File.Delete(FileName);
            for (int i = 0; i < tabDocument.TabCount - 1; i++)
                tabDocument.TabPages[i].Dispose();
        }

        private void FileSelect_FilePicked(string filepath)
        {
            int ind = tabDocument.TabPages.IndexOfKey("FileExplorer");
            FileAppend(filepath, ind);
        }

        private void FileAppend(string filepath, int index)
        {
            if (DOC_CONT != null)
            {
                panelLockDoc.Visible = true;
                panelLockDoc.BringToFront();
                DOC_CONT.ADDocument(filepath, index, tabDocument, listBoxSteps);
                panelLockDoc.Visible = false;
            }
        }

        private void webBrows_Navigated(Object sender, WebBrowserNavigatedEventArgs e)
        {
            WebBrowser web = (WebBrowser)sender;
            if (web.Url == null)
                web.Parent.Dispose();
            else if (New_doc)
            {
                //web.Stop();
                bool readable = true;
                try
                {
                    StreamReader f1 = new StreamReader(web.Url.LocalPath);
                    f1.Close();
                }
                catch
                {
                    readable = false;
                }

                if (!readable)web.Parent.Dispose();
            }
        }

        private void tab_DoubleClick(Object sender, EventArgs e)
        {
            TabPage tab = (TabPage)sender;
            TextBox text = new TextBox();
            tab.Controls.Add(text);
            text.Left = 0;
            text.Top = tab.Height = text.Height - 2;
            text.Width = tab.Width;
            text.Enter+=new EventHandler(text_Enter);
        }

        private void text_Enter(Object sender, EventArgs e)
        {
            TextBox text = (TextBox) sender;
            TabPage tab = (TabPage)text.Parent;
            tab.Text = text.Text;
            text.Dispose();
        }

        public string BaseFile()
        {
            return DOC_CONT.GETBaseDocument(0);
        }

        public string[] Load_Document_Corrections()
        {
            string[] files = null;
            if (SyB_Acc.Set_table("TDView1", AMAS_Query.Class_AMAS_Query.DocumentCorrect(Doc_ID), null))
            {
                if (SyB_Acc.Rows_count > 0)
                {
                    files = new string[SyB_Acc.Rows_count];
                    for (int i = 0; i < SyB_Acc.Rows_count; i++)
                    {
                        SyB_Acc.Get_row(i);
                        SyB_Acc.Find_Stream("correct");
                        files[i] = SyB_Acc.get_current_File();
                    }
                }
                SyB_Acc.ReturnTable();
            }
            return files;
        }

        private void load_document_content(int doc_ident)
        {
            foreach (TabPage page in tabDocument.TabPages)
                page.Dispose();
            string filename = "";
            if (SyB_Acc.Set_table("TDView1", AMAS_Query.Class_AMAS_Query.Documentcontent(doc_ident), null))
            {
                SyB_Acc.Get_row(0);
                SyB_Acc.Find_Stream("ole_doc");
                filename = SyB_Acc.get_current_File();
                SyB_Acc.ReturnTable();
            }
            DOC_CONT = new XMLContent_document(filename,New_document);
            if (SyB_Acc.PDFDirectory!=null) if (SyB_Acc.PDFDirectory.Trim().Length > 1) DOC_CONT.PDFDirectory = SyB_Acc.PDFDirectory;
            DOC_CONT.Show_documents_content(tabDocument);
        }
        
    }
}
