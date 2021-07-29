using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Excel;
using AMAS_DBI;
using ClassDocuments;

namespace AMASControlRegisters
{
    public partial class WordFeel : UserControl
    {
        private AMAS_DBI.Class_syb_acc AMAS_access;
        private int Doc_ID=0;
        Microsoft.Office.Interop.Word.Application Word_App = null;
        Microsoft.Office.Interop.Excel.Application Excel_App = null;
        Document Word_doc = null;
        Microsoft.Office.Interop.Word.AutoCorrect autocorrect;
        AutoCorrectEntries autoEntries ;
        Documents Docs ;
        _Document my_Doc;

        static object missing = Missing.Value;
        static object missing2 = Missing.Value;
        static object missing3 = Missing.Value;
        static object missing4 = Missing.Value;
        static object missing5 = Missing.Value;
        static object missing6 = Missing.Value;
        static object missing7 = Missing.Value;
        static object missing8 = Missing.Value;
        static object missing9 = Missing.Value;
        static object missing10 = Missing.Value;
        static object missing11 = Missing.Value;
        static object missing12 = Missing.Value;
        static object missing13 = Missing.Value;

        private string FileLoad="";
        private bool XMLOUTDoc=false;

        public WordFeel(Class_syb_acc ACC)
        {
            InitializeComponent();
            AMAS_access = ACC;
        }

        XMLContent_document DoContent=null;
        Document_Viewer document_View;

        public string LoadXMLDocument(string FileDoc, int kind, int tema)
        {
            string res = "";
            int i = 0;

            bool loopDoc = true; do
            {
                res = CommonValues.CommonClass.TempDirectory + "outDoc" + kind.ToString() + "t" + tema.ToString() + i.ToString() + ".docx";
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
                byte[] Buff = AMASCommand.GetFromDotLibrary(kind, tema, false);
                if (Buff == null) Buff = AMASCommand.GetFromDotLibrary(kind, 0, false);
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
                    Docs = Word_App.Documents;
                    autocorrect = Word_App.AutoCorrect;
                    autoEntries = autocorrect.Entries;
                    Word_doc = Docs.Open(ref Template, ref DocFalse, ref DocFalse, ref DocFalse, ref Nostring, ref Nostring,
                        ref DocFalse, ref Nostring, ref Nostring, ref WdOpenFormat, ref missing, ref missing, ref missing, ref missing, ref missing, ref Nostring);

                    my_Doc = (_Document)Word_doc;

                    Microsoft.Office.Interop.Word.Window win = Word_App.ActiveWindow;
                    win.Visible = true;
                    Word_App.DocumentBeforeClose += new ApplicationEvents4_DocumentBeforeCloseEventHandler(Word_App_DocumentBeforeClose);
                    Word_App.WindowDeactivate += new ApplicationEvents4_WindowDeactivateEventHandler(Word_App_WindowDeactivate);
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
                    DocTable.ReadXml(FileDoc);

                    Microsoft.Office.Interop.Word.Window myWindow = Word_App.ActiveWindow;
                    myWindow.Caption = "Создание документа ";
                    Word_App.Visible = true;

                    DataRow xmlRow;

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
                        {
                            string lbl = DocTable.Columns[ic].ColumnName;
                            object mark = lbl;

                            xmlRow =DocTable.Rows[ir];
                            txt = xmlRow[lbl].ToString();
                            object addtxt = txt;

                            Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                            Word_App.ActiveWindow.Selection.InsertAfter(txt);
                        }
                    }
                }
                catch (Exception e)
                {
                    MessageBox.Show("Разрушены метки заполнения полей" + e.ToString());
                    res = "";
                }

            return res;
        }

        public string LoadTemplate(Document_Viewer DV)
        {
            document_View = DV;
            int kind=document_View.Kind;
            int tema=document_View.Tema; 
            Doc_ID = document_View.Doc_ID;
            DoContent = document_View.ContentOfDocument;
            string Fil = "";
            bool loopDoc = true;
            int i = 0;
            do
            {
                Fil = CommonValues.CommonClass.TempDirectory + "outDoc" + kind.ToString() + "t" + tema.ToString() + i.ToString() + ".docx";
                FileInfo FF = new FileInfo(Fil);
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
                byte[] Buff =AMASCommand.GetFromDotLibrary(kind, tema,false);
                if (Buff == null) Buff = AMASCommand.GetFromDotLibrary(kind, 0, false);
                if (Buff != null)
                {
                    FileStream FS = new FileStream(Fil, FileMode.CreateNew);
                    long len = Buff.LongLength;
                    FS.Write(Buff, 0, (int)len);
                    FS.Flush();
                    FS.Close();
                    FS.Dispose();
                    string NothingString = "";
                    object WdOpenFormat = (int)Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatAuto;
                    object Template = Fil;
                    object DocFalse = false;
                    object DocTrue = true;
                    object Nostring = NothingString;

                    Word_App = new Microsoft.Office.Interop.Word.Application();
                    Docs = Word_App.Documents;
                    autocorrect = Word_App.AutoCorrect;
                    autoEntries = autocorrect.Entries;
                    Word_doc = Docs.Open(ref Template, ref DocFalse, ref DocFalse, ref DocFalse, ref Nostring, ref Nostring,
                        ref DocFalse, ref Nostring, ref Nostring, ref WdOpenFormat, ref missing, ref missing, ref missing, ref missing, ref missing, ref Nostring);

                    my_Doc = (_Document)Word_doc;

                    Microsoft.Office.Interop.Word.Window win = Word_App.ActiveWindow;
                    win.Visible = true;
                    Word_App.DocumentBeforeClose += new ApplicationEvents4_DocumentBeforeCloseEventHandler(Word_App_DocumentBeforeClose);
                    Word_App.WindowDeactivate += new ApplicationEvents4_WindowDeactivateEventHandler(Word_App_WindowDeactivate);
                }
                else MessageBox.Show("Отсутствует шаблон документа ");
            }
            catch (Exception e)
            {
                MessageBox.Show("Невозможно открыть шаблон документа " + e.ToString());
                Fil = "";
            }
            if (Fil.Length > 0)
            try
            {
                string txt = AMASCommand.CreateOutDoc(Doc_ID); //Исходящая
                if (txt.Length > 0)
                {
                    int OutDocId = AMASCommand.GetOutDocID(Doc_ID);
                    object ODI=OutDocId;
                    object IDI = Doc_ID;
                    Variable VarDocId = Word_doc.Variables.Add("AMASDocId", ref ODI);
                    Variable VarInDocId = Word_doc.Variables.Add("AMASInDocId", ref IDI);

                    string lbl = "q2";
                    object mark = lbl;

                    string rng="";
                    object RangInsert=rng;

                    string ClassType = "Word.Document.12";
                    object classT = ClassType;
                     
                    object what =(int)Microsoft.Office.Interop.Word.WdGoToItem.wdGoToBookmark;
                    object FileName = FileLoad;
                    object LnkFl = false;
                    object DispIconFile = false;
                    object addtxt =txt;
                    object FalseObj=false;
                    object TrueObj=true;

                    Microsoft.Office.Interop.Word.Window myWindow = Word_App.ActiveWindow;
                    myWindow.Caption = "Создание документа " + txt;
                    Word_App.Visible = true;

                    Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                    Word_App.ActiveWindow.Selection.InsertAfter(txt);
                    
                    lbl = "q3";
                    mark = lbl;
                    txt = AMASCommand.OutDocOutcome(OutDocId); // В ответ на входящую
                    Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                    Word_App.ActiveWindow.Selection.InsertAfter(txt);
                    
                    lbl = "q5";
                    mark = lbl;
                    txt = AMASCommand.OutDocSigner(OutDocId); // Руководитель
                    Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                    Word_App.ActiveWindow.Selection.InsertAfter(txt);
                    
                    lbl = "q6";
                    mark = lbl;
                    txt = AMASCommand.OutDocExecutor(Doc_ID); //Исполнитель
                    Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                    Word_App.ActiveWindow.Selection.InsertAfter(txt);
                    
                    lbl = "q7";
                    mark = lbl;
                    txt = AMASCommand.OutDocAnnotation(OutDocId); //Аннотация
                    Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                    Word_App.ActiveWindow.Selection.InsertAfter(txt);
                    
                    lbl = "q4";
                    mark = lbl;
                    FileLoad=DoContent.BaseDocToFile(0);
                    FileName = FileLoad;
                    Word_App.ActiveWindow.Selection.GoTo(ref what, ref missing, ref missing, ref mark);
                    Word_App.ActiveWindow.Selection.InsertFile(FileLoad, ref RangInsert, ref FalseObj, ref FalseObj, ref FalseObj); //.InlineShapes.AddOLEObject (ref classT, ref FileName, ref LnkFl, ref DispIconFile, ref missing, ref missing2, ref missing3, ref missing);

                }
            }
            catch (Exception e)
            {
                MessageBox.Show("Разрушены данные для заполнения полей" + e.ToString());
                Fil = "";
            }
        return Fil;
        }

        void Word_App_WindowDeactivate(Document Doc, Microsoft.Office.Interop.Word.Window Wn)
        {
           //SaveOutDoc(Doc);
        }

        void Word_App_DocumentBeforeClose(Document Doc, ref bool Cancel)
        {
            SaveOutDoc( Doc);
        }

        void SaveOutDoc(Document Doc)
        {
            string AMASDocId ="AMASDocId";
            object AMASDI=AMASDocId;
            Variable VarDocId = Doc.Variables.get_Item(ref AMASDI);
            AMASDocId ="AMASInDocId";
            AMASDI=AMASDocId;
            Variable VarInDocId = Doc.Variables.get_Item(ref AMASDI);
            int Document_ID = (int)Convert.ToInt32( VarDocId.Value);
            int InDocId = (int)Convert.ToInt32(VarInDocId.Value);
            FileLoad = Path.GetTempPath() + Path. GetRandomFileName() + ".docx";
            if (FileLoad.Length > 0 ) 
            {
                string ss="";
                object Fname=FileLoad;
                object wdFormatXMLDocument= (int) Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXMLDocument;
                object FalseObj=false;
                object TrueObj=true;
                object NullStr =ss;
                try
                {
                    Word_doc.SaveAs(ref Fname, ref wdFormatXMLDocument, ref FalseObj, ref NullStr, ref FalseObj, ref NullStr, ref FalseObj, ref FalseObj, ref FalseObj, ref FalseObj, ref FalseObj,
                        ref missing, ref missing, ref missing, ref missing, ref missing);
                    object wdSaveChanges= (int) Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges;
                    object wdWordDocument= (int) Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
                    //Word_doc.Close(ref wdSaveChanges, ref wdWordDocument, ref FalseObj);
                    //Word_App.Quit(ref wdSaveChanges, ref wdWordDocument, ref FalseObj);
                    Word_App.ActiveWindow.Close(ref wdSaveChanges, ref FalseObj);
                    string filename = "";
                    if (AMAS_access.Set_table("WFiew1", AMAS_Query.Class_AMAS_Query.Documentcontent(InDocId), null))
                    {
                        AMAS_access.Get_row(0);
                        AMAS_access.Find_Stream("ole_doc");
                        filename = AMAS_access.get_current_File();
                        AMAS_access.ReturnTable();
                    }
                    XMLContent_document DOC_CONT = new XMLContent_document(filename,  false);
                    DOC_CONT.ReplaceDocument(0, FileLoad);

                    string Fil = "";
                    bool loopDoc = true;
                    int i = 0;
                    do
                    {
                        Fil = Path.GetTempPath() + "Newdoc" + Document_ID.ToString()+i.ToString() + ".xml";
                        FileInfo FF = new FileInfo(Fil);
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

                    DOC_CONT.CloseDocument(Fil);

                    AMASCommand.Append_Content(Document_ID, CommonValues.CommonClass.GetImage(CommonValues.CommonClass.SaveFilewithHead(Fil)));
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void tsLoadContent_Click(object sender, EventArgs e)
        {

            if (Word_doc != null)
            {
                string lbl = "q7";
                string ClassType="Word.Document.12";

                object what = -1;
                object whith = 1;
                object count = 1;
                object mark = lbl;
                object classT =ClassType;
                object FileName=FileLoad;
                object LnkFl = false;
                object DispIconFile=false;

                Bookmarks MarkDoc = my_Doc.Bookmarks;
                if (MarkDoc.Exists(lbl))
                {
                    Bookmark AMASLabel;
                    foreach (Bookmark label in MarkDoc)
                        if (label.Name.CompareTo(lbl) == 0)
                            AMASLabel = label;
                    Word_doc.GoTo(ref what, ref whith, ref count, ref mark);
                    Word_doc.InlineShapes.AddOLEObject(ref classT, ref FileName, ref LnkFl, ref DispIconFile, ref missing, ref missing2, ref missing3, ref missing);
                }
            }
        }
    }
}
