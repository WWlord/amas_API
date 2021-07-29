using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Security.Cryptography;
using System.Security.Cryptography.Xml;
using System.Xml;
using CommonValues;
using ClassPattern;
using Microsoft.Office;
using Microsoft.Office.Interop.Word;
using System.Reflection;

namespace ClassDocuments
{
    public class XMLContent_document
    {
        private string aPDFDir="c:\\";
        public string PDFDirectory
        {
            set { aPDFDir = value; }
            get { return aPDFDir;  }
        }

        private static System.Data.DataTable Documents;
        ClassPattern.Editor DocEditor;
        ClassPattern.PictersLibrary ShowPicters;


        //private PDFCreator.clsPDFCreator  _PDFCreator;
        //private PDFCreator.clsPDFCreatorError pErr;

        private int Rows=0;
        System.Windows.Forms.Timer timer1;

        private bool editable = true;

        public XMLContent_document(string Filename, bool edit) //(string Filename,TabControl tab, bool edit)
        {
            //if (tab != null)
            //{
            //    tabDoc = tab;
            //    tabDoc.TabPages.Clear();
            //}
            Structure_Document();
            if (Filename.Length>0) Documents.ReadXml(Filename);
            Rows = 0;
                initPDFPrinter();

                timer1 = new Timer();
                timer1.Tick += new EventHandler(timer1_Tick);

                editable = edit;
         }

        public XMLContent_document(string Filename, bool edit,int Kind,int Tema) //(string Filename,TabControl tab, bool edit)
        {
            Structure_Document();
            if (Filename.Length > 0) Documents.ReadXml(Filename);
            Rows = 0;
            initPDFPrinter();

            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);

            editable = edit;
        }

        public XMLContent_document( bool edit)//(TabControl tab, bool edit)
        {
            //if (tab != null)
            //{
            //    tabDoc = tab;
            //    tabDoc.TabPages.Clear();
            //}
            Structure_Document();
            Rows = 0;

                initPDFPrinter();

            timer1 = new Timer();
            timer1.Tick += new EventHandler(timer1_Tick);

            editable = edit;
        }

        private void initPDFPrinter()
        {
            //string parameters;
            //pErr = new PDFCreator.clsPDFCreatorError();

            //_PDFCreator = new PDFCreator.clsPDFCreator();
            //_PDFCreator.eError += new PDFCreator.__clsPDFCreator_eErrorEventHandler(_PDFCreator_eError);
            //_PDFCreator.eReady += new PDFCreator.__clsPDFCreator_eReadyEventHandler(_PDFCreator_eReady);

            //parameters = "/NoProcessingAtStartup";

            //if (_PDFCreator.cStart(parameters, false))
            //{
            //    _PDFCreator.cClearCache();
            //    _PDFCreator.set_cOption("UseAutosave", 0);
            //    _PDFCreator.cPrinterStop = false;
            //}
        }

        private int TicksNumber = 0;

        void timer1_Tick(object sender, EventArgs e)
        {
            if (TicksNumber == 8)
            {
                timer1.Enabled = false;
                TicksNumber = 0;
            }
            else
            {
                FileInfo FL = new FileInfo(PDFExt(AutoSavePDFFile));
                if (FL.Exists)
                {
                    timer1.Enabled = false;
                    TicksNumber = 0;
                }
                else
                    TicksNumber++;

               // _PDFCreator.cPrinterStop = false;
            }
        }

        private void _PDFCreator_eReady()
        {
            //AddStatus("Status: \"" + _PDFCreator.cOutputFilename + "\" was created!", false);
            ReadyState = true;
            //_PDFCreator.cPrinterStop = true;
            timer1.Stop();
        }

        private void _PDFCreator_eError()
        {
            //pErr = _PDFCreator.cError;
            //AddStatus("Status: Error[" + pErr.Number + "]: " + pErr.Description, false);
        }

        private void Structure_Document()
        {
            Documents = new System.Data.DataTable("Documents");

            Documents.Columns.Add("id", typeof(System.Int32));
            Documents.Columns["id"].AutoIncrement = true;
            Documents.Columns["id"].AllowDBNull = false;
            Documents.Columns["id"].AutoIncrementStep = 1;
            Documents.PrimaryKey = new DataColumn[] { Documents.Columns["id"] };

            Documents.Columns.Add("page", typeof(System.Int32));
            Documents.Columns["page"].AllowDBNull = false;

            Documents.Columns.Add("content", typeof(System.Byte[]));
            Documents.Columns["content"].AllowDBNull = false;

            Documents.Columns.Add("file_typ", typeof(System.String));
            Documents.Columns["file_typ"].AllowDBNull = false;
            
            Documents.Columns.Add("BASE_content", typeof(System.Byte[]));
            Documents.Columns["BASE_content"].AllowDBNull = true;

            Documents.Columns.Add("BASE_file_typ", typeof(System.String));
            Documents.Columns["BASE_file_typ"].AllowDBNull = true;
        }


        public int[] Pages()
        {
            int[] DocPages = new int[Documents.Rows.Count];
            int i = 0;
            foreach (DataRow row in Documents.Rows)
            {
                DocPages[i] = (int) row["page"];
                i++;
            }
            return DocPages;
        }

        public string ExtFile(string InputFile)
        {
            int lp = LastPoint(InputFile);
            string ret = "";
            if(lp>=0)
            ret=InputFile.Substring(lp + 1, InputFile.Length - LastPoint(InputFile) - 1);
            else
            ret = "";
            return ret;
        }

        public void ReplaceDocument( int rowindex, string FileName)
        {
            if (Documents.Rows.Count > 0)
            {
                byte[] buff = GetData(FileName);
                DataRow Row = Documents.Rows[rowindex];
                string att = ExtFile(FileName);
                Row.BeginEdit();
                //if (att.ToLower().CompareTo("txt") == 0 || att.ToLower().CompareTo("rtf") == 0 || att.ToLower().CompareTo("bmp") == 0 || att.ToLower().CompareTo("jpg") == 0 || att.ToLower().CompareTo("pdf") == 0 || att.ToLower().CompareTo("gif") == 0)
                //{
                    Row["content"] = buff;
                    Row["file_typ"] = att;
                    Row["BASE_content"] = null;
                    Row["BASE_file_typ"] = null;
                //}
                //else
                //{
                //    int ftyp = 0;
                //    string PDFFILE = "";
                //    if (att.ToLower().CompareTo("bmp") == 0) ftyp = 5;
                //    if (PrintIt(FileName, PDFDirectory, ftyp, ref PDFFILE))
                //    {
                //        Row["BASE_content"] = buff;
                //        Row["BASE_file_typ"] = att;
                //        Row["file_typ"] = "pdf";
                //        Row["content"] = GetData(PDFFILE);
                //    }
                //    else
                //    {
                //        Row["content"] = buff;
                //        Row["file_typ"] = att;
                //    }
                //}
                Row.EndEdit();
            }
        }

        public void SetPDFDocument(int rowindex)
        {
            DataRow Row = Documents.Rows[rowindex];
            if (Documents.Rows.Count > rowindex)
            {
                byte[] buff;
                string FileName = "";
                string att;
                string DBnul = (string)Row["BASE_content"].GetType().ToString();
                if (DBnul.CompareTo("System.DBNull") == 0)
                {
                    buff = (byte[])Row["content"];
                    att = (string)Row["file_typ"];
                }
                else
                {
                    buff = (byte[])Row["BASE_content"];
                    att = (string)Row["BASE_file_typ"];
                }
                bool loopDoc = true;
                int i = 0;
                do
                {
                    FileName = Path.GetTempPath() + "Outdoc" + i.ToString() + "." + att;
                    FileInfo FF = new FileInfo(FileName);
                    if (FF.Exists)
                        try
                        {
                            FF.Delete();
                            loopDoc = false;
                        }
                        catch { loopDoc = true; }
                    else loopDoc = false;
                    i++;
                }
                while (loopDoc);

                FileStream fff = new FileStream(FileName, System.IO.FileMode.Create, FileAccess.Write);
                fff.Write(buff, 0, buff.Length);
                fff.Flush();
                fff.Close();

                int ftyp = 0;
                //DBnul = (string)Row["BASE_content"].GetType().ToString();
                //if (DBnul.CompareTo("System.DBNull") == 0)
                string[] PDFS=PrintIt(FileName, PDFDirectory, ftyp);
                if (PDFS != null)
                    foreach(string PDFFILE in PDFS)
                    {
                        Row.BeginEdit();
                        //    {
                        //        Row["file_typ"] = "pdf";
                        //        Row["content"] = GetData(PDFFILE);
                        //        Row["BASE_content"] = null;
                        //        Row["BASE_file_typ"] = null;
                        //    }
                        //    else
                        //    {
                        //        Row["content"] = buff;
                        //        Row["file_typ"] = att;
                        //        Row["BASE_content"] = buff;
                        //        Row["BASE_file_typ"] = att;
                        //    }

                        Row["file_typ"] = "pdf";
                        Row["content"] = GetData(PDFFILE);
                        Row["BASE_content"] = buff;
                        Row["BASE_file_typ"] = att;
                        Row.EndEdit();
                        File.Delete(PDFFILE);
                    }
            }
        }

        public void DELDocument( TabControl TC,TabPage tbl)
        {
            TabPage tab = tbl;
            if (tab != null)
                if (tab.Name.CompareTo("FileExplorer") != 0)
                {
                    try
                    {
                        int page = -1;
                        int index = TC.SelectedIndex;
                        DataRow Row = Documents.Rows.Find((int)Convert.ToInt32(tab.Name.Substring(3)));
                        page = (int)Row["page"];
                        Row.Delete();
                        if (page >= 0)
                            foreach (DataRow Rrr in Documents.Select("page>=" + page.ToString()))
                            {
                                page = (int)Rrr["page"] - 1;
                                if (page == 0)
                                    TC.TabPages["tab" + Rrr["id"].ToString()].Text = "Документ";
                                else
                                    TC.TabPages["tab" + Rrr["id"].ToString()].Text = "Приложение " + page.ToString();

                                Rrr["page"] = (int)Rrr["page"] - 1;
                            }
                    }
                    catch { }
                    tab.Dispose();
                }

        }

        public int SaveDocument(string InputFile, ref int ret, int index)
        {
            int res = 0;
            byte[] buff = null;
            string att = "";
            buff = GetData(InputFile);
            if (buff != null)
            {
                try
                {
                    DataRow Row = Documents.NewRow();
                    att = ExtFile(InputFile);
                    ret = Rows++;
                    Row["page"] = index;
                    if (att.ToLower().CompareTo("txt") == 0 || att.ToLower().CompareTo("rtf") == 0 || att.ToLower().CompareTo("bmp") == 0 || att.ToLower().CompareTo("jpg") == 0 || att.ToLower().CompareTo("pdf") == 0 || att.ToLower().CompareTo("gif") == 0 || att.ToLower().CompareTo("AMASplb".ToLower()) == 0)
                    {
                        Row["content"] = buff;
                        Row["file_typ"] = att;
                    }
                    else
                    {
                        int ftyp = 0;
                        if (att.ToLower().CompareTo("bmp") == 0) ftyp = 5;
                        string[] PDFS=PrintIt(InputFile, PDFDirectory, ftyp);
                        if (PDFS != null)
                            foreach (string PDFFILE in PDFS)
                            {
                                Row["BASE_content"] = buff;
                                Row["BASE_file_typ"] = att;
                                Row["file_typ"] = "pdf";
                                Row["content"] = GetData(PDFFILE);
                            }
                        //else
                        //{
                        //    Row["content"] = buff;
                        //    Row["file_typ"] = att;
                        //}
                    }

                    Documents.Rows.Add(Row);
                    Row.EndEdit();
                    res = (int)Row["id"];
                }
                catch (Exception ex)
                {
                    string err = ex.Message; ret = -1;
                }
            }
            return res;
        }

        public int ADDocument(string InputFile, int index, TabControl tabDocument, ListBox StepsList)
        {
            int ret = -1;
            byte[] buff = null;
            string att = "";
            if (StepsList != null)
                StepsList.Items.Clear();
            buff = GetData(InputFile);
            if (buff != null)
            {
                if (StepsList != null) StepsList.Items.Add("Файл обнаружен");
                try
                {
                    att = ExtFile(InputFile);
                    DataRow Row;
                    int ftyp = 0;
                    bool bContent = false;
                    string[] PDFS;
                    if (att.ToLower().CompareTo("bmp") == 0) ftyp = 5;
                    if (att.ToLower().CompareTo("txt") == 0 || att.ToLower().CompareTo("rtf") == 0 || att.ToLower().CompareTo("bmp") == 0 || att.ToLower().CompareTo("jpg") == 0 || att.ToLower().CompareTo("pdf") == 0 || att.ToLower().CompareTo("gif") == 0 || att.ToLower().CompareTo("AMASplb".ToLower()) == 0)
                    {
                        bContent = false;
                        PDFS = new string[1];
                        //PDFS[0] = "";
                        PDFS[0] = InputFile;
                    }
                    else
                    {
                        bContent = true;
                        PDFS = PrintIt(InputFile, PDFDirectory, ftyp);
                    }
                    if (PDFS != null)
                        foreach (string PDFFILE in PDFS)
                        {
                            Row = Documents.NewRow();
                            ret = Rows++;
                            Row["page"] = index;
                            if (bContent)
                            {
                                if (StepsList != null) StepsList.Items.Add("Файл преобразован");
                                Row["BASE_content"] = buff;
                                Row["BASE_file_typ"] = att;
                                Row["file_typ"] = "pdf";
                                Row["content"] = GetData(PDFFILE);
                            }
                            else
                            {
                                Row["content"] = buff;
                                Row["file_typ"] = att;
                                Row["BASE_content"] = null;
                                Row["BASE_file_typ"] = null;
                            }
                            Documents.Rows.Add(Row);
                            Row.EndEdit();

                            int key = (int)Row["id"];
                            string tabkey = "Tab" + key.ToString();
                            int page = (int)Row["page"];
                            int PageCore = page;
                            string DocCaptur = "";
                            if (index > 0)
                                DocCaptur = "Приложение " + page.ToString();
                            else
                                DocCaptur = "Документ";
                            tabDocument.TabPages.Insert(index, tabkey, DocCaptur);
                            if (StepsList != null) StepsList.Items.Add(DocCaptur + " установлен");
                            tabDocument.TabPages[index].Show();
                            foreach (DataRow Rrr in Documents.Select("page>" + page.ToString()))
                            {
                                page = (int)Rrr["page"] + 1;
                                tabDocument.TabPages["tab" + Rrr["id"].ToString()].Text = "Приложение " + page.ToString();
                                Rrr["page"] = (int)Rrr["page"] + 1;
                            }

                            TabPage tab = tabDocument.TabPages[tabkey];

                            string showFile = "";
                            try
                            {
                                showFile = DocToFile(PageCore);
                            }
                            catch //(Exception e)
                            {
                                //SyB_Acc.EBBLP.AddError(e.Message, "Document_View - 1", e.StackTrace);
                            }
                            try
                            {
                                if (showFile.Length > 3)
                                    FileContentView(tab, showFile, ExtFile(showFile), true);
                                else
                                {
                                    System.Windows.Forms.WebBrowser webBrows = new System.Windows.Forms.WebBrowser();
                                    tab.Controls.Add(webBrows);
                                    // 
                                    // webBrowser1
                                    // 
                                    webBrows.Dock = System.Windows.Forms.DockStyle.Fill;
                                    webBrows.Location = new System.Drawing.Point(0, 0);
                                    webBrows.Margin = new System.Windows.Forms.Padding(2);
                                    webBrows.MinimumSize = new System.Drawing.Size(15, 16);
                                    webBrows.Name = "webBrows" + tabkey;
                                    webBrows.Size = new System.Drawing.Size(468, 267);
                                    webBrows.TabIndex = 0;
                                    try
                                    {
                                        webBrows.Navigate(InputFile);
                                    }
                                    catch { }
                                }
                            }
                            catch { }
                            index++;
                        }
                    if (StepsList != null) StepsList.Items.Add("Файл загружен");
                }
                catch (Exception ex)
                {
                    string err = ex.Message; ret = -1;
                }
            }
            else ret = -1;
            return ret;
        }

        private bool ReadyState;
        private int maxTime = 2;
        private string AutoSavePDFFile = "";

        public int WaitPrint
        {
            get { return maxTime; }
            set { maxTime = value; }
        }

        private string[] PrintIt(string InputFile, string PDFDirectory, int FileTyp)
        {
            string PDFFILE = "";

            string[] PDFiles = null;

            string fname;
            FileInfo fi;
            fi = new FileInfo(InputFile);
            fname = fi.Name;

            Microsoft.Office.Interop.Word.Application Word_App = null;
            Microsoft.Office.Interop.Word.Document Word_doc = null;
            Microsoft.Office.Interop.Word.AutoCorrect autocorrect;
            Microsoft.Office.Interop.Word.AutoCorrectEntries autoEntries;
            Microsoft.Office.Interop.Word.Documents Docs;
            Microsoft.Office.Interop.Word._Document my_Doc;

            Microsoft.Office.Interop.Excel.Application Excel_App = null;
            Microsoft.Office.Interop.Excel.Sheets Excel_sheets = null;

            object missing = Missing.Value;
            object missing2 = Missing.Value;
            object missing3 = Missing.Value;
            object missing4 = Missing.Value;
            object missing5 = Missing.Value;
            object missing6 = Missing.Value;
            object missing7 = Missing.Value;
            object missing8 = Missing.Value;
            object missing9 = Missing.Value;
            object missing10 = Missing.Value;
            object missing11 = Missing.Value;
            object missing12 = Missing.Value;
            object missing13 = Missing.Value;

            object wdNOSaveChanges = (int)Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges;

            string stringFILExt = CommonValues.CommonClass.getfileExtention(InputFile).ToLower();
            switch (stringFILExt)
            {
                case "xml":
                    string res = "";
                    int i = 0;
                    ClassPattern.XMLDocument XMLD = new XMLDocument(InputFile);
                    int Kind = XMLD.Kind();
                    int Tema = XMLD.Tema();
                    
                    bool loopDoc = true;
                    do
                    {
                        res = CommonValues.CommonClass.TempDirectory + "XMLDoc" + Kind.ToString() + "t" + Tema.ToString() + i.ToString() + ".docx";
                        FileInfo FF = new FileInfo(res);
                        try
                        {
                            FF.Delete();
                            loopDoc = false;
                        }
                        catch { loopDoc = true; }
                        i++;
                    }
                    while (loopDoc);

                    try
                    {
                        byte[] Buff = AMAS_DBI.AMASCommand.GetFromDotLibrary(Kind, Tema, false);
                        if (Buff == null) Buff = AMAS_DBI.AMASCommand.GetFromDotLibrary(Kind, 0, false);
                        if (Buff != null)
                        {
                            FileStream FS = new FileStream(res, FileMode.CreateNew);
                            long len = Buff.LongLength;
                            FS.Write(Buff, 0, (int)len);
                            FS.Flush();
                            FS.Close();
                            FS.Dispose();
                            string NothingString = "";
                            object WdOpenFormat = (int)Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatAuto;
                            object Template = res;
                            object DocFalse = false;
                            object DocTrue = true;
                            object Nostring = NothingString;

                            Word_App = new Microsoft.Office.Interop.Word.Application();
                            string appVersion = Word_App.Version;
                            appVersion = appVersion.Split(".".ToCharArray()[0])[0];
                            Docs = Word_App.Documents;
                            autocorrect = Word_App.AutoCorrect;
                            autoEntries = autocorrect.Entries;
                        switch (appVersion)
                        {
                            case "12":
                            case "14":
                            Word_doc = Docs.Open(ref Template, ref DocFalse, ref DocFalse, ref DocFalse, ref Nostring, ref Nostring,
                                ref DocFalse, ref Nostring, ref Nostring, ref WdOpenFormat, ref missing, ref missing, ref missing, ref missing, ref missing, ref Nostring);
                            break;
                            default:
                            MessageBox.Show("Поддерживается только MS OFFICE 2007/2010");
                            break;
                        }

                            my_Doc = (_Document)Word_doc;

                            Microsoft.Office.Interop.Word.Window win = Word_App.ActiveWindow;
                            win.Visible = true;
                            //Word_App.DocumentBeforeClose += new ApplicationEvents4_DocumentBeforeCloseEventHandler(Word_App_DocumentBeforeClose);
                            //Word_App.WindowDeactivate += new ApplicationEvents4_WindowDeactivateEventHandler(Word_App_WindowDeactivate);
                        }
                        else MessageBox.Show("Отсутствует шаблон документа ");
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("Невозможно открыть шаблон документа " + e.ToString());
                        res = "";
                    }
            if (res.Length > 0)
                try
                {
                    System.Data.DataTable DocTable = new System.Data.DataTable();
                    DocTable.ReadXml(InputFile);

                    Microsoft.Office.Interop.Word.Window myWindow = Word_App.ActiveWindow;
                    myWindow.Caption = "Создание документа ";
                    Word_App.Visible = true;
                    string appVersion = Word_App.Version;
                    appVersion = appVersion.Split(".".ToCharArray()[0])[0];

                    DataRow xmlRow;
                    try
                    {
                        object what = (int)Microsoft.Office.Interop.Word.WdGoToItem.wdGoToBookmark;
                        for (int iw = DocTable.Rows.Count-1; iw >0; iw--)
                        {
                            object mark = "Numberow";
                        switch (appVersion)
                        {
                            case "12":
                            case "14":
                             Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                             if (iw == DocTable.Rows.Count - 1) Word_App.ActiveWindow.Selection.InsertAfter((DocTable.Rows.Count).ToString());
                           Word_doc.ActiveWindow.Selection.Rows.Add(ref missing);
                           Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                            Word_doc.ActiveWindow.Selection.MoveUp(ref missing);
                            Word_App.ActiveWindow.Selection.InsertAfter((DocTable.Rows.Count- iw).ToString());
                            break;
                            default:
                            MessageBox.Show("Поддерживается только MS OFFICE 2007/2010");
                            break;
                        }
                        }
                    }
                    catch (Exception ex) { string Errmess = ex.Message; }

                    for (int ir = 0; ir < DocTable.Rows.Count; ir++)
                    {
                        string txt = "";
                        string rng = "";
                        object RangInsert = rng;

                        string ClassType = "Word.Document.12";
                        object classT = ClassType;

                        object what = (int)Microsoft.Office.Interop.Word.WdGoToItem.wdGoToBookmark;

                        object FalseObj = false;
                        object TrueObj = true;

                        for (int ic = 0; ic < DocTable.Columns.Count; ic++)
                            try
                            {
                                string lbl = DocTable.Columns[ic].ColumnName;
                                object mark = lbl;

                                xmlRow = DocTable.Rows[ir];
                                txt = xmlRow[lbl].ToString();
                                object addtxt = txt;


                                switch (appVersion)
                                {
                                    case "14":
                                Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                                        for (int iu = DocTable.Rows.Count - 1 - ir; iu > 0; iu--)
                                            Word_doc.ActiveWindow.Selection.MoveUp(ref missing);

                                        Word_App.ActiveWindow.Selection.InsertAfter(txt);
                                        break;
                                    default:
                                        MessageBox.Show("Поддерживается только MS OFFICE 2010");
                                        break;
                                }
                            }
                            catch (Exception ex) { string Errmess = ex.Message; }
                    }
                    PDFFILE = PDFDirectory;
                    if (PDFDirectory.Substring(PDFDirectory.Length - 1).CompareTo("\\") == 0)
                        PDFFILE += fname;
                    else PDFFILE += "\\" + fname;
                    PDFFILE = PDFExt(PDFFILE);
                    AutoSavePDFFile = PDFFILE;

                    switch (appVersion)
                    {
                        case "12":
                        case "14":
                            Word_App.ActiveWindow.Document.ExportAsFixedFormat(PDFFILE, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF, false,
                                 Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument, 1, 1,
                                 Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent, true, true, Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks,
                                 true, true, false, ref missing2);

                            Word_doc.Close(ref wdNOSaveChanges, ref missing, ref missing2);
                            break;
                        default:
                            MessageBox.Show("Поддерживается только MS OFFICE 2007/2010");
                            break;
                    }
                        Word_doc = null;

                    timer1.Interval = maxTime * (int)fi.Length / 1000;
                    if (timer1.Interval < 200) timer1.Interval = 300;
                    timer1.Enabled = true;
                    while (timer1.Enabled)
                    {
                        System.Windows.Forms.Application.DoEvents();
                    }
                    timer1.Enabled = false;

                    PDFiles = new string[1];
                    PDFiles[0] = PDFFILE;
                }
                catch (Exception e)
                {
                    MessageBox.Show("Разрушены метки заполнения полей" + e.ToString());
                    res = "";
                }
                finally
                {
                    if (Word_doc != null)
                        Word_doc.Close(ref wdNOSaveChanges, ref missing, ref missing2);
                    if (Word_App != null)
                        Word_App.Quit(ref wdNOSaveChanges, ref missing, ref missing2);
                }
    
                                        
                break;

                case "doc":
                case "docx":
                case "rtf":
                    //                  if (stringFILExt.CompareTo("doc") == 0 || stringFILExt.CompareTo("docx") == 0 || stringFILExt.CompareTo("rtf") == 0)
                    try
                    {

                        string NothingString = "";
                        object inFile = InputFile;
                        object WdOpenFormat = (int)Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatAuto;
                        object DocFalse = false;
                        object DocTrue = true;
                        object Nostring = NothingString;

                        Word_App = new Microsoft.Office.Interop.Word.Application();
                        string appVersion = Word_App.Version;
                        appVersion = appVersion.Split(".".ToCharArray()[0])[0];
                        Docs = Word_App.Documents;
                        autocorrect = Word_App.AutoCorrect;
                        autoEntries = autocorrect.Entries;
                        switch (appVersion)
                        {
                            case "12":
                            case "14":
                                Word_doc = Docs.Open(ref inFile, ref DocFalse, ref DocFalse, ref DocFalse, ref Nostring, ref Nostring,
                                     ref DocFalse, ref Nostring, ref Nostring, ref WdOpenFormat, ref missing, ref missing, ref missing, ref missing, ref missing, ref Nostring);
                                break;
                            default:
                                MessageBox.Show("Поддерживается только MS OFFICE 2007/2010");
                                break;
                        }

                        my_Doc = (Microsoft.Office.Interop.Word._Document)Word_doc;

                        Microsoft.Office.Interop.Word.Window win = Word_App.ActiveWindow;
                        Word_App.Visible = false;

                        PDFFILE = PDFDirectory;
                        if (PDFDirectory.Substring(PDFDirectory.Length - 1).CompareTo("\\") == 0)
                            PDFFILE += fname;
                        else PDFFILE += "\\" + fname;
                        PDFFILE = PDFExt(PDFFILE);
                        AutoSavePDFFile = PDFFILE;
                        switch (appVersion)
                        {
                            case "12":
                            case "14":
                                Word_App.ActiveWindow.Document.ExportAsFixedFormat(PDFFILE, Microsoft.Office.Interop.Word.WdExportFormat.wdExportFormatPDF, false,
                                    Microsoft.Office.Interop.Word.WdExportOptimizeFor.wdExportOptimizeForPrint, Microsoft.Office.Interop.Word.WdExportRange.wdExportAllDocument, 1, 1,
                                    Microsoft.Office.Interop.Word.WdExportItem.wdExportDocumentContent, true, true, Microsoft.Office.Interop.Word.WdExportCreateBookmarks.wdExportCreateNoBookmarks,
                                    true, true, false, ref missing2);
                                Word_doc.Close(ref wdNOSaveChanges, ref missing, ref missing2);
                                break;
                            default:
                                MessageBox.Show("Поддерживается только MS OFFICE 2007/2010");
                                break;
                        }
                        Word_doc = null;

                        timer1.Interval = maxTime * (int)fi.Length / 1000;
                        if (timer1.Interval < 200) timer1.Interval = 300;
                        timer1.Enabled = true;
                        while (timer1.Enabled)
                        {
                            System.Windows.Forms.Application.DoEvents();
                        }
                        timer1.Enabled = false;

                        PDFiles = new string[1];
                        PDFiles[0] = PDFFILE;
                    }
                    catch { PDFiles = null; }
                    finally
                    {
                        if (Word_doc != null)
                            Word_doc.Close(ref wdNOSaveChanges, ref missing, ref missing2);
                        if (Word_App != null)
                            Word_App.Quit(ref wdNOSaveChanges, ref missing, ref missing2);
                    }
                    break;

                case "xls":
                case "xlsx":
                    try
                    {
                        string NothingString = "";
                        object inFile = InputFile;
                        object DocFalse = false;
                        object DocTrue = true;
                        object Nostring = NothingString;

                        int indexx = 0;
                        object indexSh = indexx;

                        Excel_App = new Microsoft.Office.Interop.Excel.Application();
                        string appVersion = Excel_App.Version;
                        appVersion = appVersion.Split(".".ToCharArray()[0])[0];

                        Microsoft.Office.Interop.Excel.Workbooks ExcelBooks = Excel_App.Workbooks;

                        Microsoft.Office.Interop.Excel.Workbook ThisBook = ExcelBooks._Open(InputFile, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);

                        Excel_sheets = ThisBook.Sheets;


                        Microsoft.Office.Interop.Excel.Worksheet activeWorksheet = ThisBook.ActiveSheet;

                        if (ThisBook.Sheets.Count > 0)
                        {
                            PDFiles = new string[ThisBook.Sheets.Count];
                            int Ipage = 0;
                            int kstr = 0;

                            for (; Ipage < PDFiles.Length; Ipage++)
                            {
                                do
                                {
                                    PDFFILE = PDFDirectory;
                                    if (PDFDirectory.Substring(PDFDirectory.Length - 1).CompareTo("\\") == 0)
                                        if (kstr > 0) PDFFILE += kstr.ToString() + fname;
                                        else PDFFILE += fname;
                                    else
                                        if (kstr > 0) PDFFILE += "\\" + kstr.ToString() + fname;
                                        else PDFFILE += "\\" + fname;

                                    PDFFILE = PDFExt(PDFFILE);
                                    fi = new FileInfo(PDFFILE);

                                    kstr++;
                                }
                                while (fi.Exists == true);

                                AutoSavePDFFile = PDFFILE;

                                //ActiveSheet.ExportAsFixedFormat Type:=xlTypePDF, Filename:= _
                                //"C:\Documents and Settings\Administrator\My Documents\Книга1.pdf", Quality:= _
                                //xlQualityStandard, IncludeDocProperties:=True, IgnorePrintAreas:=False, _
                                //OpenAfterPublish:=False

                                activeWorksheet = (Microsoft.Office.Interop.Excel.Worksheet)ThisBook.Sheets.get_Item((object)(Ipage + 1));


                                //activeWorksheet.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, PDFFILE, missing, true, false, missing, missing, false, missing);

                        switch (appVersion)
                        {
                            case "12":
                            case "14":
                                activeWorksheet.ExportAsFixedFormat(Microsoft.Office.Interop.Excel.XlFixedFormatType.xlTypePDF, (object)PDFFILE, missing, (object)true, (object)false, missing, missing, (object)false, missing);
                                //Word_doc.Close(ref wdNOSaveChanges, ref missing, ref missing2);
                                break;
                            default:
                                MessageBox.Show("Поддерживается только MS OFFICE 2007/2010");
                                break;
                        }
                        //Word_doc = null;

                                fi = new FileInfo(InputFile);
                                timer1.Interval = maxTime * (int)fi.Length / 1000;
                                if (timer1.Interval < 200) timer1.Interval = 300;
                                timer1.Enabled = true;
                                while (timer1.Enabled)
                                {
                                    System.Windows.Forms.Application.DoEvents();
                                }
                                timer1.Enabled = false;

                                PDFiles[Ipage] = PDFFILE;
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        string Mess = ex.Message;
                        if (PDFiles != null)
                        {
                            int cnt = 0;
                            foreach (string s in PDFiles)
                                if (s != null)
                                    cnt++;
                                else break;
                            if (cnt > 0)
                            {
                                string[] sss = new string[cnt];
                                for (int gi = 0; gi < cnt; gi++)
                                    sss[gi] = PDFiles[gi];
                                PDFiles = sss;
                            }
                            else PDFiles = null;
                        }
                        timer1.Enabled = false;
                    }
                    finally
                    {
                        if (Excel_App != null) Excel_App.Quit();
                    }
                    break;
            }
            if (PDFiles == null)
            {
                string DefaultPrinter;

                //PDFCreator.clsPDFCreatorOptions opt;

                //if (!_PDFCreator.cIsPrintable(fi.FullName))
                //{
                //    MessageBox.Show("File '" + fi.FullName + "' is not printable!");
                //    PDFiles=null;
                //}
                //else
                //    try
                //   {
                //       {
                //            opt = _PDFCreator.cOptions;
                //            opt.UseAutosave = 1;
                //            opt.UseAutosaveDirectory = 1;
                //            opt.AutosaveDirectory = PDFDirectory;
                //            opt.AutosaveFormat = FileTyp; //???
                //            if (FileTyp == 5)
                //            {
                //                opt.BMPResolution = 72;
                //            }
                //            opt.AutosaveFilename = fname;
                //            PDFFILE = opt.AutosaveDirectory;
                //            if (opt.AutosaveDirectory.Substring(opt.AutosaveDirectory.Length - 1).CompareTo("\\") == 0)
                //                PDFFILE += fname;
                //            else PDFFILE += "\\" + fname;
                //            AutoSavePDFFile = PDFFILE;
                //            ReadyState = false;
                //            _PDFCreator.cOptions = opt;
                //            _PDFCreator.cClearCache();
                //            DefaultPrinter = _PDFCreator.cDefaultPrinter;
                //            _PDFCreator.cDefaultPrinter = "PDFCreator";
                //            _PDFCreator.cPrinterStop = false;
                //            _PDFCreator.cPrintFile(fi.FullName);
                //            timer1.Interval = maxTime * (int)fi.Length / 1000;
                //            if (timer1.Interval < 200) timer1.Interval = 200;
                //            timer1.Enabled = true;
                //            _PDFCreator.cPrinterStop = false;
                //            while (!ReadyState && timer1.Enabled)
                //            {
                //                Application.DoEvents();
                //            }
                //            timer1.Enabled = false;
                //if (!ReadyState)
                //{
                //    MessageBox.Show("Creating AMAS page as pdf.\n\r\n\r" +
                //        "An error is occured: Time is up!");
                //}
                //_PDFCreator.cPrinterStop = true;
                //            _PDFCreator.cDefaultPrinter = DefaultPrinter;

                //            PDFiles = new string[1];
                //            PDFiles[0] = PDFFILE;
                //        }
                //    }
                //    catch { }
            }
            //AutoSavePDFFile = "";
            return PDFiles;
        }

        private int LastPoint(string DOTstring)
        {
            int dot = -1;
            for (int i = 0; i < DOTstring.Length; i++)
            {
                if (DOTstring.Substring(i, 1).CompareTo(".") == 0) dot = i;
            }
            return dot;
        }

        private string PDFExt(string ret)
        {
            char[] s ={'.' };
            string[] rows = ret.Split(s,1024);
            ret = "";
            for (int i = 0; i < rows.Length - 1; i++)
                ret += rows[i];
            return ret+".pdf";
        }

        private byte[] GetData(string InputFileName)
        {
            byte[] buffer = null;
            try
            {
                StreamReader f1 = new StreamReader(InputFileName);
                buffer = new byte[f1.BaseStream.Length];
                //for (int i = 0; i < f1.BaseStream.Length; i++)
                //    buffer[i] = (byte)f1.BaseStream.ReadByte();
                f1.BaseStream.Read(buffer, 0, (int)f1.BaseStream.Length);
                f1.Close();
            }
            catch { buffer = null; }
            return buffer;
        }

        public string DocToFile(int page)
        {
            string filename = "";
            const int bufsize = 32000;
            byte[] buffer = new byte[bufsize];
            DataTableReader reader = Documents.CreateDataReader();

            if (reader.HasRows)
            {
                long i = 0;
                long res;
                bool isrecord;
                do
                {
                    isrecord = reader.Read();
                }
                while ((reader.GetInt32(1) != page) && (isrecord));
                if (isrecord)
                {
                    int testnum = 0;
                    StreamWriter fil1;
                    bool trueFile = false;
                    do
                    {
                        try
                        {
                            filename = CommonValues.CommonClass.TempDirectory + "Document" + page.ToString() + testnum.ToString() + "."; //+ Row["file_typ"]; 
                            filename += reader.GetString(3);
                            if (File.Exists(filename))
                            {
                                File.Delete(filename);
                                trueFile = true;
                            }
                            else trueFile = true;
                        }
                        catch //(Exception ex)
                        {
                            trueFile = false;
                        }
                        testnum++;
                    }
                    while (!trueFile && testnum < 8);
                    if (trueFile)
                    {
                        fil1 = new StreamWriter(filename);
                        do
                        {
                            res = reader.GetBytes(2, i, buffer, 0, bufsize);
                            fil1.BaseStream.Write(buffer, 0, (int)res);
                            i += bufsize;
                        }
                        while (res > 0);
                        fil1.Flush();
                        fil1.Close();
                    }
                }
            }
            reader.Close();
            return filename;
        }

        public string BaseDocToFile(int page)
        {
            string filename = "Document" + page.ToString() + ".";
            const int bufsize = 32000;
            byte[] buffer = new byte[bufsize];
            DataTableReader reader = Documents.CreateDataReader();
            if (reader.HasRows)
            {
            int Clmn ;
                long i = 0;
                long res;
                bool isrecord;
                do
                {
                    isrecord = reader.Read();
                }
                while ((reader.GetInt32(1) != page) && (!isrecord));
                if (isrecord)
                {
                    if (reader.IsDBNull(4))
                        Clmn = 2;
                    else Clmn = 4;
                    int testnum = 0;
                    StreamWriter fil1;
                    bool trueFile = false;
                    do
                    {
                        try
                        {
                            filename = CommonValues.CommonClass.TempDirectory + page.ToString() + testnum.ToString() + "."; //+ Row["file_typ"]; 

                            filename += reader.GetString(Clmn+1);
                            fil1 = new StreamWriter(filename);
                            trueFile = true;
                            fil1.Close();
                        }
                        catch //(Exception ex)
                        {
                            trueFile = false;
                        }
                        testnum++;
                    }
                    while (!trueFile && testnum < 8);
                    if (trueFile)
                    {
                        fil1 = new StreamWriter(filename);
                        do
                        {
                            res = reader.GetBytes(Clmn, i, buffer, 0, bufsize);
                            fil1.BaseStream.Write(buffer, 0, (int)res);
                            i += bufsize;
                        }
                        while (res > 0);
                        fil1.Flush();
                        fil1.Close();
                    }
                }
            }
            reader.Close();
            return filename;
        }

        public void GETDocument(string OutputFile,int page)
        {
            const int bufsize=32000;
            byte[] buffer = new byte [bufsize];
            DataTableReader reader= Documents.CreateDataReader();
            if (reader.HasRows)
            {
                long i = 0;
                long res;
                bool isrecord;
                
                do
                {
                isrecord=reader.Read();
                }
                while ((reader.GetInt32(1) != page) && (isrecord));

                StreamWriter f1 = new StreamWriter(OutputFile);

                if (isrecord)
                {
                    do
                    {
                        res = reader.GetBytes(2, i, buffer, 0, bufsize);
                        f1.BaseStream.Write(buffer, 0, (int) res);
                        i += bufsize;
                    }
                    while (res >0);
                }
                else f1.Flush();

                f1.Close();
            }
            reader.Close();
        }

        public string GETBaseDocument( int page)
        {
            string FilTYP = "";
            string OutputFile="";
            FileStream f1 = null;
            const int bufsize = 32000;
            byte[] buffer = new byte[bufsize];
            DataTableReader reader = Documents.CreateDataReader();
            if (reader.HasRows)
            {
                long i = 0;
                long res;
                bool isrecord;

                //do
                {
                    isrecord = reader.Read();
                }
                //while ((reader.GetInt32(1) != page+1) && (isrecord));

                int num=1;
                bool noFile = false;
                do
                {
                    OutputFile = CommonValues.CommonClass.TempDirectory + "BaseDoc" + num.ToString();
                    if (File.Exists(OutputFile))
                    {
                        try
                        {
                            File.Delete(OutputFile);
                            noFile = true;
                        }
                        catch { noFile = false; num++; }
                    }
                    else noFile = true;
                }
                while (!noFile);

                 //f1 = new StreamWriter(OutputFile);
                f1 = new FileStream( OutputFile,FileMode.Create,FileAccess.Write); //(OutputFile);
                
                if (isrecord)
                {
                    do
                    {
                        if (reader.IsDBNull(4))
                            res = reader.GetBytes(2, i, buffer, 0, bufsize);
                        else
                            res = reader.GetBytes(4, i, buffer, 0, bufsize);
                        f1.Write(buffer, 0, (int)res);
                        //f1.BaseStream.Write(buffer, 0, (int)res);
                        f1.Write(buffer, 0, (int)res);
                        i += bufsize;
                    }
                    while (res > 0);
                    try
                    { FilTYP = reader.GetString(5); }
                    catch { FilTYP = ""; }
                    
                }
                else f1.Flush();
                f1.Close();
                //f1.Dispose();

                /*if (File.Exists(OutputFile + "." + FilTYP))
                {
                    num = 1;
                    try
                    {
                        File.Delete(OutputFile + "." + FilTYP);
                        File.Move(OutputFile, OutputFile + "." + FilTYP);
                        OutputFile = OutputFile + "." + FilTYP;
                    }
                    catch 
                    {
                        Random ran = new Random();
                        string iran = ran.Next(1000000).ToString();
                        File.Move(OutputFile, OutputFile + iran + "." + FilTYP);
                        OutputFile = OutputFile + iran + "." + FilTYP;
                    }
                }
                else
                {
                    File.Move(OutputFile, OutputFile + "." + FilTYP);
                    OutputFile=OutputFile + "." + FilTYP;
                }*/
            }
            try
            {
                f1.Close();
//                f1.Dispose();
            }
            catch { }
            reader.Close();
            reader.Dispose();
            return OutputFile;
        }

        public void CloseDocument(string filename)
        {
            Documents.WriteXml(filename);
        }

        public void SigningDocument(string FileName,string SignFile)
        {
            Documents.WriteXml(FileName);
            SignEnvelope.RSAKey(FileName, SignFile);
        }

        public void Show_documents_content(TabControl tabDoc)
        {
            tabDoc.TabPages.Clear();
            int[] pages = this.Pages();
            string filename;
            for (int i = 0; i < pages.Length; i++)
            {
                DataRow Row =Documents.Rows[i];
                string File_typ = (string)Row["file_typ"];
                filename = "Document" + pages[i].ToString();
                //CommonValues.CommonClass.tempFiles.AddFile(filename, false);
                bool FileTrue = false;
                int FileShift = 0;
                do
                {
                    filename = CommonValues.CommonClass.TempDirectory + pages[i].ToString() +FileShift.ToString() + "." + File_typ; //+ Row["file_typ"];
                    try 
                    { 
                        if (File.Exists(filename)) 
                            File.Delete(filename);
                        if (!File.Exists(filename)) FileTrue = true;
                        else FileTrue = false;
                    }
                    catch { FileTrue = false; }
                    FileShift++;
                }
                while (!FileTrue && FileShift < 800);
                string tabkey = "Tab" + i.ToString();
                if (pages[i] == 0)
                    tabDoc.TabPages.Insert(i, tabkey, "Документ");
                else
                    tabDoc.TabPages.Insert(i, tabkey, "Приложение " + pages[i].ToString());
                TabPage tab = tabDoc.TabPages[tabkey];
                GETDocument(filename, pages[i]);
                FileContentView(tab, filename, File_typ,false);
            }
        }

        public bool FileContentView(TabPage tab, string filename, string File_typ, bool EditablePage)
        {
            bool res = false;

            WebBrowser webBrows;

            
            
            if (File_typ.ToLower().Contains("AMASplb".ToLower()))
            {
                ShowPicters = new PictersLibrary();
                tab.Controls.Add(ShowPicters);
                // 
                // PicterLibrary
                // 
                this.ShowPicters.Dock = System.Windows.Forms.DockStyle.Fill;
                this.ShowPicters.Location = new System.Drawing.Point(0, 0);
                this.ShowPicters.Name = "ShowPicters" + tab.Name;
                this.ShowPicters.Size = new System.Drawing.Size(468, 267);
                this.ShowPicters.TabIndex = 0;

                ShowPicters.Editable=EditablePage;
                ShowPicters.OpenLibrary(filename);

                res = true;
            }

            else if (File_typ.ToLower().Contains("rtf") || File_typ.Contains("txt"))
            {
                DocEditor = new ClassPattern.Editor();
                DocEditor.Editable = editable;
                tab.Controls.Add(DocEditor);
                // 
                // DocEditor
                // 
                this.DocEditor.Dock = System.Windows.Forms.DockStyle.Fill;
                this.DocEditor.Location = new System.Drawing.Point(0, 0);
                this.DocEditor.Name = "RTFView" + tab.Name;
                this.DocEditor.Size = new System.Drawing.Size(468, 267);
                this.DocEditor.TabIndex = 0;

                if (File_typ.Contains("rtf")) DocEditor.LoadFromFile(filename);
                if (File_typ.Contains("txt")) DocEditor.LoadFromTextFile(filename);

                res = true;
            }
            else if (File_typ.ToLower().Contains("bmp") || File_typ.ToLower().Contains("jpg") || File_typ.ToLower().Contains("jpeg") || File_typ.ToLower().Contains("gif") || File_typ.ToLower().Contains("giff") || File_typ.ToLower().Contains("png"))
            {
                /*PictureBox PicterViewer = new System.Windows.Forms.PictureBox();
                tab.Controls.Add(PicterViewer);
                // 
                // BMPView
                // 
                PicterViewer.Dock = System.Windows.Forms.DockStyle.Fill;
                PicterViewer.Location = new System.Drawing.Point(0, 0);
                PicterViewer.Name = "BMPView" + tab.Name;
                PicterViewer.Size = new System.Drawing.Size(468, 267);
                PicterViewer.TabIndex = 0;

                PicterViewer.Load(filename);
                */

                ShowPicters = new PictersLibrary();
                tab.Controls.Add(ShowPicters);

                // 
                // PicterLibrary
                // 
                this.ShowPicters.Dock = System.Windows.Forms.DockStyle.Fill;
                this.ShowPicters.Location = new System.Drawing.Point(0, 0);
                this.ShowPicters.Name = "PicterShow" + tab.Name;
                this.ShowPicters.Size = new System.Drawing.Size(468, 267);
                this.ShowPicters.TabIndex = 0;

                ShowPicters.Editable = EditablePage;

                string [] picters=new string[1];
                picters[0]=filename;
                ShowPicters.ShowPicters(picters);


                res = true;
            }
            else
            {
                webBrows = new System.Windows.Forms.WebBrowser();
                tab.Controls.Add(webBrows);
                // 
                // webBrowser1
                // 
                webBrows.Dock = System.Windows.Forms.DockStyle.Fill;
                webBrows.Location = new System.Drawing.Point(0, 0);
                webBrows.Margin = new System.Windows.Forms.Padding(2);
                webBrows.MinimumSize = new System.Drawing.Size(15, 16);
                webBrows.Name = "webBrows";
                webBrows.Size = new System.Drawing.Size(468, 267);
                webBrows.TabIndex = 0;

                webBrows.Navigate(filename);
                
                res = true;
            }
            tab.Disposed += new EventHandler(tab_Disposed);

            return res;
        }
     
        private void tab_Disposed(Object sender, EventArgs e)
        {
            TabPage tab = (TabPage)sender;
            Control[] webs = tab.Controls.Find("webBrows", false);
            WebBrowser bro;
            foreach (Control web in webs)
            {
                bro = (WebBrowser)web;
                bro.Stop();
                //bro.Navigate("");
            }
        }
    }


public class SignEnvelope
{
    public static string ResultString;
    public static void RSAKey(string FileName, string SignFile)
    {
        // Генерация ключа электронной подписи.
       RSACryptoServiceProvider Key = new RSACryptoServiceProvider();
       try
       {

           // Подпись  XML файла и сохранение электронной подписи в 
           // новом файле.

           SignXmlFile(FileName, SignFile, Key);
           ResultString="XML file signed.";

           // Проверка электронной подписи XML.
           ResultString+="Проверка подписи...";

           bool result = VerifyXmlFile(SignFile);

           // Отображение результатов на консоли 

           if (result)
           {
               ResultString+="XML подпись верна.";
           }
           else
           {
               ResultString+="XML подпись неверна.";
           }
       }
       catch (CryptographicException e)
       {
           ResultString+=e.Message;
       }
       finally
       {
           // Очистка ресурсов  RSACryptoServiceProvider.
           Key.Clear();
       }
   }

    // Подпись XML файла и сохранение в новом файле.
    public static void SignXmlFile(string FileName, string SignedFileName, RSA Key)
    {
        // Проверка аргументов.  
        if (FileName == null)
            throw new ArgumentNullException("FileName");
        if (SignedFileName == null)
            throw new ArgumentNullException("SignedFileName");
        if (Key == null)
            throw new ArgumentNullException("Key");


        // Создать новый XML документ.
        XmlDocument doc = new XmlDocument();

        // Форматирование документа с игнорированием дублирующих пробелов.
        doc.PreserveWhitespace = false;

        // Загрузка XML файла.
        doc.Load(new XmlTextReader(FileName));

        // Создание SignedXml объекта.
        SignedXml signedXml = new SignedXml(doc);

        // Добавление ключа в SignedXml документ. 
        signedXml.SigningKey = Key;

        // Получение экземпляра электронной подписи от SignedXml объекта.
        Signature XMLSignature = signedXml.Signature;

        // Создать ссылку.
        Reference reference = new Reference("");

        // Добавить  конверт.
        XmlDsigEnvelopedSignatureTransform env = new XmlDsigEnvelopedSignatureTransform();
        reference.AddTransform(env);

        // Добавить Reference объект в объект Signature.
        XMLSignature.SignedInfo.AddReference(reference);

        // Добавить RSAKeyValue KeyInfo 
        KeyInfo keyInfo = new KeyInfo();
        keyInfo.AddClause(new RSAKeyValue((RSA)Key));

        // Добавить KeyInfo объект объекту Reference.
        XMLSignature.KeyInfo = keyInfo;

        // Расчитать электронную подпись.
        signedXml.ComputeSignature();

        // Получить XML сведения об электронной подписи и сохранить
        // в XmlElement объекте.
        XmlElement xmlDigitalSignature = signedXml.GetXml();

        // Добавить элемент в XML документ.
        doc.DocumentElement.AppendChild(doc.ImportNode(xmlDigitalSignature, true));


        if (doc.FirstChild is XmlDeclaration)
        {
            doc.RemoveChild(doc.FirstChild);
        }

        // Сохранить подписаный XML документ в указанный файл.
         
        XmlTextWriter xmltw = new XmlTextWriter(SignedFileName, new UTF8Encoding(false));
        doc.WriteTo(xmltw);
        xmltw.Close();
    }
    // Проверить подпись XML файла и вернуть результат.
    public static Boolean VerifyXmlFile(String Name)
    {
        // Проверить аргументы.  
        if (Name == null)
            throw new ArgumentNullException("Name");

        // Создать новый XML документ.
        XmlDocument xmlDocument = new XmlDocument();

        // Форматирование с пробелами.
        xmlDocument.PreserveWhitespace = true;

        // Зазрузить и отправить XML файл в документ. 
        xmlDocument.Load(Name);

        // Создать новый SignedXml объект и отправить его
        // в класс XML документа.
        SignedXml signedXml = new SignedXml(xmlDocument);

        // Найти "Signature" узел и создать новый
        // XmlNodeList объект.
        XmlNodeList nodeList = xmlDocument.GetElementsByTagName("Signature");

        // Загрузить узел электронной подписи.
        signedXml.LoadXml((XmlElement)nodeList[0]);

        // Проверить подпись и вернуть результат.
        return signedXml.CheckSignature();
    }

}

}
