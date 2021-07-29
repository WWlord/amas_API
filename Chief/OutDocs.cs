using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClassErrorProvider;
using AMASControlRegisters;
using ClassDocuments;

namespace Chief
{
    public partial class OutDocs : Form
    {
        public int ModuleId;
        private AMAS_DBI.Class_syb_acc AMAS_access;
        private AMASControlRegisters.ContragentRegister contragentRegisterout;
        private AMASControlRegisters.Document_Viewer document_View;
        private AMASControlRegisters.Document_Viewer document_Out;
        private WordFeel WordSheet;

        public OutDocs(AMAS_DBI.Class_syb_acc Acc)
        {
            InitializeComponent();
            ModuleId = (int)ClassErrorProvider.ErrorBBLProvider.Modules.OutputDoc;
            AMAS_access=Acc;
            this.contragentRegisterout = new AMASControlRegisters.ContragentRegister();
            this.tpListing.Controls.Add(this.contragentRegisterout);
            // 
            // contragentRegisterout
            // 
            this.contragentRegisterout.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contragentRegisterout.Document = 0;
            this.contragentRegisterout.Location = new System.Drawing.Point(3, 3);
            this.contragentRegisterout.Name = "contragentRegisterout";
            this.contragentRegisterout.Size = new System.Drawing.Size(867, 551);
            this.contragentRegisterout.TabIndex = 0;

            document_View = new AMASControlRegisters.Document_Viewer(AMAS_access, null);
            document_View.Doc_ID = 0;
            document_View.Dock = System.Windows.Forms.DockStyle.Fill;
            document_View.Location = new System.Drawing.Point(250, 0);
            document_View.Name = "document_View";
            document_View.New_document = false;
            document_View.Sender = 0;
            document_View.Size = new System.Drawing.Size(this.splitContainer3.Panel2.Size.Width - 250, 521);
            document_View.TabIndex = 3;
            splitContainer3.Panel2.Controls.Add(document_View);
            //document_View.DocumentPicked += new AMASControlRegisters.Document_Viewer.PickDocument(document_View_DocumentPicked);

            document_Out = new AMASControlRegisters.Document_Viewer(AMAS_access, null);
            document_Out.Doc_ID = 0;
            document_Out.Dock = System.Windows.Forms.DockStyle.Fill;
            document_Out.Location = new System.Drawing.Point(250, 0);
            document_Out.Name = "document_Out";
            document_Out.New_document = false;
            document_Out.Sender = 0;
            document_Out.Size = new System.Drawing.Size(this.splitContainer3.Panel2.Size.Width - 250, 521);
            document_Out.TabIndex = 3;
            splitContainer4.Panel2.Controls.Add(document_Out);
            //document_Out.DocumentPicked += new AMASControlRegisters.Document_Viewer.PickDocument(document_View_DocumentPicked);

            WordSheet = new WordFeel(AMAS_access);
            WordSheet.Dock = System.Windows.Forms.DockStyle.Fill;
            WordSheet.Name = "WordSheet";
            WordSheet.Location = new System.Drawing.Point(250, 0);
            WordSheet.Size = new System.Drawing.Size(100, 100);
            WordSheet.Visible = false;
            splitContainer3.Panel2.Controls.Add(WordSheet);

            tvForMailDocs.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvForMailDocs_NodeMouseClick);
            tvForMailDocs.AfterSelect+=new TreeViewEventHandler(tvForMailDocs_AfterSelect);
            contragentRegisterout.connect(AMAS_access);
            tvSendOutDocument.NodeMouseClick += new TreeNodeMouseClickEventHandler(tvSendOutDocument_NodeMouseClick);
            tvSendOutDocument.AfterSelect+=new TreeViewEventHandler(tvSendOutDocument_AfterSelect);
            ListDocsToMail();
        }

        void tvSendOutDocument_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (document_Out.Doc_ID != (int)Convert.ToInt32(e.Node.Name.Substring(1)))
            document_Out.Doc_ID = (int)Convert.ToInt32(e.Node.Name.Substring(1));
        }

        private void tvSendOutDocument_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (document_Out.Doc_ID != (int)Convert.ToInt32(e.Node.Name.Substring(1)))
            document_Out.Doc_ID = (int)Convert.ToInt32(e.Node.Name.Substring(1));
        }

        void tvForMailDocs_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (document_View.Doc_ID != (int)Convert.ToInt32(e.Node.Name.Substring(1)))
            document_View.Doc_ID = (int)Convert.ToInt32( e.Node.Name.Substring(1));
        }
        private void tvForMailDocs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (document_View.Doc_ID != (int)Convert.ToInt32(e.Node.Name.Substring(1)))
            document_View.Doc_ID = (int)Convert.ToInt32(e.Node.Name.Substring(1));
        }

        private void ListDocsToMail()
        {
            tvForMailDocs.Nodes.Clear();
            if (AMAS_access.Set_table("OutDocsToMail", AMAS_Query.OutDocument.ListDocsToMail, null))
            {
                int id;
                try
                {
                    for (int i = 0; i < AMAS_access.Rows_count; i++)
                    {
                        AMAS_access.Get_row(i);
                        id = (int)AMAS_access.Find_Field("kod");
                        tvForMailDocs.Nodes.Add("m"+id.ToString(), (string)AMAS_access.Find_Field("find_cod"));
                    }
                }
                catch (Exception ex)
                {
                    AMAS_access.EBBLP.AddError(ex.Message, "OutDoc - 1", ex.StackTrace);
                }
                AMAS_access.ReturnTable();
            }
        }
        private void ListOutDocs()
        {
            tvSendOutDocument.Nodes.Clear();
            if (AMAS_access.Set_table("OutDocsList", AMAS_Query.OutDocument.ListOutDocs,null))
            {
                int id;
                try
                {
                    for (int i = 0; i < AMAS_access.Rows_count; i++)
                    {
                        AMAS_access.Get_row(i);
                        id = (int)AMAS_access.Find_Field("kod");
                        string OFC = "---";
                        try
                        {
                            OFC=(string)AMAS_access.Find_Field("find_cod");
                        }
                        catch { }
                        tvSendOutDocument.Nodes.Add("m" + id.ToString(), OFC);
                    }
                }
                catch (Exception ex)
                {
                    AMAS_access.EBBLP.AddError(ex.Message, "OutDoc - 1", ex.StackTrace);
                }
                AMAS_access.ReturnTable();
            }
        }

        private void tcOutMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (tcOutMode.SelectedTab.Name)
            {
                case "tpPrepare":
                    ListDocsToMail();
                    break;
                case "tpListing":
                    ListOutDocs();
                    break;
                case "tpAgentGroups":
                    break;
            }
        }


        private void splitContainer2_SplitterMoved(object sender, SplitterEventArgs e)
        {

        }

        private void создатьИсходящийДокументToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //WordSheet.Visible = true;
            //WordSheet.BringToFront();
            string fl = WordSheet.LoadTemplate(document_View);
        }

        private void pDFФорматToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string filename = "";
            int InDocId = (int)Convert.ToInt32(tvSendOutDocument.SelectedNode.Name.Substring(1));
            if (AMAS_access.Set_table("OutDoc1", AMAS_Query.Class_AMAS_Query.Documentcontent(InDocId), null))
            {
                AMAS_access.Get_row(0);
                AMAS_access.Find_Stream("ole_doc");
                filename = AMAS_access.get_current_File();
                AMAS_access.ReturnTable();
            }
            XMLContent_document DOCont = new XMLContent_document(filename, false);
            DOCont.SetPDFDocument(0);
            string Fil = "";
            bool loopDoc = true;
            int i = 0;
            do
            {
                Fil = Path.GetTempPath() + "OUTdoc" + InDocId.ToString() + i.ToString() + ".xml";
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

            DOCont.CloseDocument(Fil);

            AMAS_DBI.AMASCommand.Update_Content(InDocId, CommonValues.CommonClass.GetImage(CommonValues.CommonClass.SaveFilewithHead(Fil)));
        }
    }
}