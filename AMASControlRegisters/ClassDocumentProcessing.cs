using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Office.Interop.Word;
using CommonValues;
using AMAS_DBI;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
//using AMASControlRegisters;
using ClassErrorProvider;

namespace AMASControlRegisters
{
    public class DocumentProcessing
    {
        private AMASControlRegisters.Document_Viewer document_New;

        object missing = Missing.Value;
        Microsoft.Office.Interop.Word.Application Word_App = null;
        Document Word_doc = null;
        AutoCorrect autocorrect;
        AutoCorrectEntries autoEntries;
        Documents Docs;
        _Document my_Doc;
        private System.Windows.Forms.Timer timer2;
        public AMAS_DBI.Class_syb_acc SYB_acc;
        private bool ReadyState = false;

        public DocumentProcessing(AMAS_DBI.Class_syb_acc ACC, AMASControlRegisters.Document_Viewer docum)
        {
            SYB_acc=ACC;
            timer2 = new System.Windows.Forms.Timer();
            document_New = docum;
        }

        public void AddDot(int kind, int tema)
        {
            string Fil = "";
            bool loopDoc = true;
            int i = 0;
            do
            {
                Fil = CommonValues.CommonClass.TempDirectory + "MYDoc" + kind.ToString() + "t" + tema.ToString() + i.ToString() + ".docx";
                //Fil = SYB_acc.PDFDirectory+ "MYDoc" + kind.ToString() + "t" + tema.ToString() + i.ToString() + ".docx";
                FileInfo FF = new FileInfo(Fil);
                if (FF.Exists)
                {
                    try
                    {
                        FF.Delete();
                        loopDoc = false;
                    }
                    catch { loopDoc = true; }
                }
                else loopDoc = false;
                i++;
            }
            while (loopDoc);
            try
            {
                byte[] Buff = AMASCommand.GetFromDotLibrary(kind, tema, true);
                FileStream FS = new FileStream(Fil, FileMode.CreateNew, FileAccess.Write);
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

                Window win = Word_App.ActiveWindow;
                win.Activate();
                Word_App.DocumentBeforeClose += new ApplicationEvents4_DocumentBeforeCloseEventHandler(Word_App_DocumentBeforeClose);
                Word_App.Visible = true;

                timer2.Interval = 300;
                timer2.Start();
                ReadyState = false;
                timer2.Enabled = true;
                while (!ReadyState && timer2.Enabled)
                {
                    System.Windows.Forms.Application.DoEvents();
                }
                timer2.Stop();
                timer2.Enabled = false;
                document_New.SelectedFile_Append(FileLoad);
            }
            catch (Exception e)
            {
                MessageBox.Show("Невозможно открыть шаблон документа ");
                SYB_acc.EBBLP.AddError("Невозможно открыть шаблон документа " + e.Message, "DocumentProcessing - 15", e.StackTrace);
                Fil = "";
            }
        }

        void Word_App_DocumentBeforeClose(Document Doc, ref bool Cancel)
        {
            SaveDot(Doc);
        }

        string FileLoad = "";

        void SaveDot(Document Doc)
        {
            FileLoad = Doc.FullName;
            if (FileLoad.Length > 0)
            {
                string ss = "";
                object Fname = FileLoad;
                object wdFormatXMLDocument = (int)Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXMLDocument;
                object FalseObj = false;
                object TrueObj = true;
                object NullStr = ss;
                object fch = WdSaveOptions.wdSaveChanges;
                object oft = WdOriginalFormat.wdWordDocument;
                object rfl = null;
                try
                {
                    //Word_App.Visible = false;
                    Word_doc.SaveAs(ref Fname, ref wdFormatXMLDocument, ref FalseObj, ref NullStr, ref FalseObj, ref NullStr, ref FalseObj, ref FalseObj, ref FalseObj, ref FalseObj, ref FalseObj,
                        ref missing, ref missing, ref missing, ref missing, ref missing);
                    object wdSaveChanges = (int)Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges;
                    object wdWordDocument = (int)Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
                    Word_App.ActiveWindow.Close(ref wdSaveChanges, ref FalseObj);
                    Word_App.Quit(ref fch, ref oft, ref rfl);
                    ReadyState = true;
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

    }
}
