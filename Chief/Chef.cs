using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using AMAS_DBI;
using AMAS_Query;
using AMASDocuments;
using ClassDocuments.DocsTree;
using DocumentsByPeriod;
using ClassDocuments;
using ClassInterfases;
using ClassStructure;
using Chief.baseLayer;
using ClassPattern;
using ClassErrorProvider;
using AMASControlRegisters;
using CommonValues;
using Microsoft.Office.Interop.Word;
using System.Reflection;

namespace Chief
{
    public partial class Chef : Form, FormShowCon
    {
        private ClassStructure.Structure Structure;
        AMAS_Query.KINDdoc Select_docs;
        private AMASControlRegisters.Document_Viewer document_View;
        private AMASControlRegisters.Document_Viewer document_New;
        private ClassDocuments.DocsTree.ClassDocsTree DocumentsTree;
        public AMAS_DBI.Class_syb_acc SYB_acc;
        public DocsOfPeriod[] Period ;
        private Address_ids KindBox;
        private Address_ids TemaBox;
        private Sign_ids SignaturesBox;

        private AMASControlRegisters.Finder FindDocList;

        public int ModuleId;
        private ChefSettings frmSettings1 = new ChefSettings();

        public Chef(AMAS_DBI.Class_syb_acc S_acc)
        {
            InitializeComponent();
            SYB_acc = S_acc;
            SEDO();
            Structure = new Structure(SYB_acc, treeViewDepts);
            treeViewDepts.CheckBoxes = true;
            Structure.UseGroups(treeViewGroups,true);
            document_View = new AMASControlRegisters.Document_Viewer(SYB_acc, null);//tvDocsTree.SelectedNode);
            this.document_View.Doc_ID = 0;
            this.document_View.Dock = System.Windows.Forms.DockStyle.Fill;
            this.document_View.Location = new System.Drawing.Point(250, 0);
            this.document_View.Name = "documentCH_View";
            this.document_View.New_document = false;
            this.document_View.Sender = 0;
            this.document_View.Size = new System.Drawing.Size(this.panelNewDoc.Size.Width-250, 521);
            this.document_View.TabIndex = 3;
            this.splitContainer1.Panel2.Controls.Add(this.document_View);
            document_View.DocumentPicked += new AMASControlRegisters.Document_Viewer.PickDocument(document_View_DocumentPicked);
            document_View.DocumenTiped += new Document_Viewer.TipDocument(document_View_DocumenTiped);
            document_View.TaskIdKilll += new Document_Viewer.DeleteTaskId(document_View_TaskIdKilll);
            this.document_New = new AMASControlRegisters.Document_Viewer(SYB_acc,null);
            // 
            // document_New
            // 
            this.document_New.Doc_ID = 0;
            this.document_New.Dock = System.Windows.Forms.DockStyle.Left; 
            this.document_New.Location = new System.Drawing.Point(0, 0);
            this.document_New.Name = "documentCH_Show";
            this.document_New.New_document = false;
            this.document_New.Sender = 0;
            this.document_New.Size = new System.Drawing.Size(this.panelNewDoc.Size.Width-250, 521);
            this.document_New.TabIndex = 4;
            this.panelNewDoc.Controls.Add(this.document_New); //.splitContainer1.Panel2

            //
            // Finder
            //
            FindDocList = new Finder(SYB_acc);
            FindDocList.Dock = System.Windows.Forms.DockStyle.Fill;
            FindDocList.Location = new System.Drawing.Point(0, 0);
            FindDocList.Name = "FindDocList";
            FindDocList.TabIndex = 5;
            FindDocList.Visible = false;
            this.splitContainer1.Panel2.Controls.Add(FindDocList);

            tscbSelectDocs.SelectedIndexChanged+=new EventHandler(tscbSelectDocs_SelectedIndexChanged);
            this.Load+=new EventHandler(Chef_Load);
            this.FormClosed+=new FormClosedEventHandler(Chef_FormClosed);
            this.Resize+=new EventHandler(Chef_Resize);

            KindBox = new Address_ids(cbKinds);
            TemaBox = new Address_ids(cbTemy);
            KindBox.connect(SYB_acc);
            TemaBox.connect(SYB_acc);
            KindBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_kinds(), "kind","kod");
            TemaBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_temy(KindBox.get_ident()), "description_", "tema");
            KindBox.Child = TemaBox;
            cbKinds.SelectedIndexChanged +=new EventHandler(cbKinds_SelectedIndexChanged);
            ModuleId = (int)ClassErrorProvider.ErrorBBLProvider.Modules.Chief;
            SYB_acc.ErrOfChief+=new Class_syb_acc.ErrorBBLChief(SYB_acc_ErrOfChief);

            this.monthCalendarExe.DateSelected += new DateRangeEventHandler(monthCalendarExe_DateSelected);
            this.tsSendToExe.Click += new EventHandler(tsSendToExe_Click);
            this.tsSendToSign.Click += new EventHandler(tsSendToSign_Click);
            this.tsSendInfo.Click += new EventHandler(tsSendInfo_Click);


            CBSigns = new ComboBox();
            CBSigns.Visible = true;
            CBSigns.Name = "CBSigns";
            //CBSigns.Dock = DockStyle.Right;
            this.Controls.Add(CBSigns);
            CBSigns.Top = 0;
            CBSigns.Width = CBSignsWid(toolStrip1);
            CBSigns.Left = CBSignsLeft(toolStrip1);
            CBSigns.Height = toolStrip1.Height;
            CBSigns.BringToFront();
            mySignatures();
            timer2.Tick += new EventHandler(timer2_Tick);

            this.Refresh();
        }

        void document_View_TaskIdKilll(int task, int TVN)
        {
            try
            {
                if (Period != null)
                    foreach (DocsOfPeriod Docs in Period)
                    {
                        if (Docs != null)
                            try
                            {
                                DocsOfPeriod.Body DocsBdy = Docs.DocsGroup;
                                if (DocsBdy != null)
                                    switch (TVN)
                                    {
                                        case 1:
                                            foreach (ClassDocsItem.DocTask DT in DocsBdy.ItDocument.Tasklist)
                                            {
                                                if (DT.Task_ID == task) DT.DeleteTaskView();
                                            }
                                            break;
                                        case 2:
                                            foreach (ClassDocsItem.DocViza DT in DocsBdy.ItDocument.Vizalist)
                                            {
                                                if (DT.Viza_ID == task) DT.DeleteVizaView();
                                            }
                                            break;
                                        case 3:
                                            foreach (ClassDocsItem.DocNew DT in DocsBdy.ItDocument.Newlist)
                                            {
                                                if (DT.News_ID == task) DT.DeleteNewsView();
                                            }
                                            break;
                                    }
                            }
                            catch { }
                    }
            }
            catch { }
        }

        void document_View_DocumenTiped(int Document)
        {
            tsDenote.Enabled =AMASCommand.MeAnswer(Document);
            tsSeeNote.Enabled = AMASCommand.DocCorrecting(Document);
        }

        void timer2_Tick(object sender, EventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        ComboBox CBSigns;

        void document_View_DocumentPicked(int Document)
        {
            document_View.Doc_ID = Document;
        }

        void tsSendInfo_Click(object sender, EventArgs e)
        {
            ToInfo();
        }

        void tsSendToSign_Click(object sender, EventArgs e)
        {
            ToSign();
        }

        void tsSendToExe_Click(object sender, EventArgs e)
        {
            ToExecute();
        }

        void monthCalendarExe_DateSelected(object sender, DateRangeEventArgs e)
        {
            monthCalendarExe.Visible = false;
            tscExeTimer.Text = e.Start.ToShortDateString();
        }

        private void cbKinds_SelectedIndexChanged(Object sender, EventArgs e)
        {
            TemaBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_temy(KindBox.get_ident()), "description_", "tema");
        }

        private void Chef_Load(Object sender, EventArgs e)
        {
            frmSettings1.SettingsKey = "Chef";
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

            Binding bndSplitterDistance1 = new Binding("SplitterDistance", frmSettings1, "Splitter1", true, DataSourceUpdateMode.OnPropertyChanged);
            splitContainer1.DataBindings.Add(bndSplitterDistance1);

            //Binding bndSplitterDistance2 = new Binding("SplitterDistance", frmSettings1, "Splitter2", true, DataSourceUpdateMode.OnPropertyChanged);
            //splitContainer2.DataBindings.Add(bndSplitterDistance2);

            panelNewDoc.SendToBack();
            splitContainer1.BringToFront();
            this.treeViewDepts.BringToFront();
            if (SYB_acc.GetRights != null) if (SYB_acc.GetRights.Mail) tsForMail.Visible = true;
            tscbSelectDocs.SelectedIndex = 0;
            FindDocList.Visible = true;
            FindDocList.BringToFront();
        }

        private void Chef_FormClosed(Object sender, FormClosedEventArgs e)
        {
            frmSettings1.Save();
        }
                
        private void Chef_Resize(Object sender, EventArgs e)
        {
            frmSettings1.Save();
            document_New.Width = panelNewDoc.Width - gbCommom.Width - 3;
            //gbCommom.Left = this.panelNewDoc.Size.Width - gbCommom.Size.Width-2;
            //gbExecution.Left = gbCommom.Left;

            //splitContainerViza.Height=(panelViza.Height-labelViza.Height)/2;

            //this.document_New.Size = new System.Drawing.Size(this.panelNewDoc.Size.Width - gbCommom.Size.Width - 4, 521);

            CBSigns.Width = CBSignsWid(toolStrip1);
            CBSigns.Left = CBSignsLeft(toolStrip1);
            //CBSigns.Width = this.Width - CBSigns.Left - 8;
        }

        private int CBSignsWid(ToolStrip tS)
        {
            int wdt = 0;
            int LastItemWidth = 0;
            foreach (ToolStripItem TSI in tS.Items)
            {
                if (TSI.Visible) wdt += TSI.Width;
                LastItemWidth = TSI.Width;
            }
            return tS.Width - wdt - LastItemWidth - 1;
        }

        private int CBSignsLeft(ToolStrip tS)
        {
            int wdt = 0;
            int LastItemWidth = 0;
            foreach (ToolStripItem TSI in tS.Items)
            {
                if (TSI.Visible) wdt += TSI.Width;
                LastItemWidth = TSI.Width;
            }
            return tS.Left + wdt + LastItemWidth;
        }

        private void mySignatures()
        {
            try
            {
                CBSigns.Items.Clear();
                //SignaturesBox = new Sign_ids(CBSigns);
                //SignaturesBox.Child = null;
                //SignaturesBox.connect(SYB_acc);
                //SignaturesBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Structure_signatures(), "sign", "id");
                //CBSigns.LostFocus += new EventHandler(ComboBox_LostFocus);
                SYB_acc.Set_table("TChief1", AMAS_Query.Class_AMAS_Query.Structure_signatures(),null);
                for (int i=0;i<SYB_acc.Rows_count;i++)
                {
                    SYB_acc.Get_row(i);
                    CBSigns.Items.Add(SYB_acc.Find_Field("sign"));
                }
                SYB_acc.ReturnTable();
            }
            catch { }
        }

        void ComboBox_LostFocus(object sender, EventArgs e)
        {
            if (SignaturesBox != null)
            {
                SignaturesBox.AdditionalSign();
                string sign = SignaturesBox.NewSignName;
                if (SignaturesBox.NewSignName.Length > 0)
                {
                    AMASCommand.AddSign(sign);
                    SignaturesBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Structure_signatures(), "sign", "id");
                    SignaturesBox.get_index_by_text(sign);
                }
            }
        }

        private System.Windows.Forms.Panel[] Seek_panels;

        private void SEDO()
        {
            Select_docs = new KINDdoc(0);
            Seek_panels = new Panel[Select_docs.DS_cnt];
            
            tscbSelectDocs.Items.Clear();
            for (int i = 0; i < Select_docs.DS_cnt; i++)
            {
                //if (Select_docs.DocSeek[i].val != (int) DocEnumeration.NewsDocs.Value)
                {
                    tscbSelectDocs.Items.Add(Select_docs.DocSeek[i].desc);
                    Seek_panels[i] = new Panel();
                    Seek_panels[i].Dock = DockStyle.Fill;
                    Seek_panels[i].BackColor = splitContainer2.Panel1.BackColor;
                    Seek_panels[i].Visible = false;
                    splitContainer2.Panel1.Controls.Add(Seek_panels[i]);
                }
            }
            Period = new DocsOfPeriod[Select_docs.DS_cnt];
            Period[0] = new DocsOfPeriod(Select_docs.DocSeek[0],Seek_panels[0], this);
            Period[0].NodePicked+=new DocsOfPeriod.PickedHandler(Chef_NodePicked);
            tscbSelectDocs.SelectedItem = 0;

            DocumentsTree = new ClassDocsTree(tvDocsTree, SYB_acc);
            DocumentsTree.DocPicked += new ClassDocsTree.PickedDocument(DocumentsTree_DocPicked);
        }

        private void Chef_NodePicked(int DocId,TreeNode Nod)
        {
            document_View.BringToFront();
            document_View.TNode = Nod;
            if (document_View.Doc_ID != DocId)
            {
                //Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc
                try
                {
                    document_View.TNode = Period[tscbSelectDocs.SelectedIndex].DocsGroup.treeDocsView.SelectedNode; // tvDocsTree.SelectedNode;
                    document_View.ValueOfDocument = Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc;
                }
                catch
                {
                }
                //document_View.Doc_ID = DocId;
                document_View.Show();
                DocumentsTree.Refresh(DocId, document_View.DocumentNumber);
            }
        }

        private void DocumentsTree_DocPicked(int DocId)
        {
            document_View.BringToFront();
            if (document_View.Doc_ID != DocId)
            {
                document_View.ValueOfDocument = null;
                document_View.Doc_ID = DocId;
                document_View.Show();
            }
        }

        public System.Windows.Forms.ImageList imagelib() { return imageList1; }
        public System.Windows.Forms.Panel panel() { return this.splitContainer2.Panel1; }
        public System.Windows.Forms.ToolStripProgressBar FuelBar() { return toolStripProgressBar1; }
        public Class_syb_acc DB_acc() { return SYB_acc; }

        private void tscbSelectDocs_SelectedIndexChanged(object sender, EventArgs e)
        {
            FindDocList.Visible = true;
            FindDocList.BringToFront();

            for (int i = 0; i < Select_docs.DS_cnt; i++)
                try
                {
                    Seek_panels[i].Visible = false;
                }
                catch { }
            Seek_panels[tscbSelectDocs.SelectedIndex].Visible = true;
            if (Seek_panels[tscbSelectDocs.SelectedIndex].Controls.Count == 0)
            {
                Period[tscbSelectDocs.SelectedIndex] = new DocsOfPeriod(Select_docs.DocSeek[tscbSelectDocs.SelectedIndex], Seek_panels[tscbSelectDocs.SelectedIndex], this);
            }
            AMAS_Query.Class_AMAS_Query.DocIndex = Select_docs.DocSeek[tscbSelectDocs.SelectedIndex].val;
            Period[tscbSelectDocs.SelectedIndex].DocsGroup.Resize();
            Period[tscbSelectDocs.SelectedIndex].NodePicked += new DocsOfPeriod.PickedHandler(Chef_NodePicked);
            FindDocList.FindDocsOfPeriod = Period[tscbSelectDocs.SelectedIndex];
        }

        private void рассылкаПоОтделамToolStripMenuItem_Click(object sender, EventArgs e)
        {
            
        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            treeViewDepts.Visible = true;
        }

        private int[] Morray = null;

        private void ToExecute()
        {
                    tssSended.DropDownItems.Clear();
            try
            {
                if (true_Moving())
                {
                    Morray = Structure.push_letter(false, tscExeTimer.Text, Find_ID_selecteDoc(), CBSigns.Text, from_moving());
                    if (Morray != null)
                    {
                        Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.add_taskToList(Morray);
                        Period[tscbSelectDocs.SelectedIndex].DocsGroup.CurrentItemRefresh();
                        foreach (string To in Structure.taskSended)
                        {
                            tssSended.DropDownItems.Add(document_View.DocumentNumber.Trim()+ " на исполнение "+To);
                            tssSended.Text = document_View.DocumentNumber.Trim() + " на исполнение " + To;
                        }
                    }
                }
                else
                {
                    SYB_acc.ModuleId = ModuleId;
                    SYB_acc.AddError("Вы не можете отправить документ к исполнению, поскольку документ не был назначен Вам на исполнение.", "Chief - 1.1", "");
                }
            }
            catch { }
        }

        private void ToSign()
        {
            tssSended.DropDownItems.Clear();
            try
            {
                if (true_Moving())
                {
                    Morray = Structure.push_viza(tscExeTimer.Text, Find_ID_selecteDoc(), from_moving());
                    if (Morray != null)
                    {
                        Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.add_vizaToList(Morray);
                        Period[tscbSelectDocs.SelectedIndex].DocsGroup.CurrentItemRefresh();
                        foreach (string To in Structure.vizaSended)
                        {
                            tssSended.DropDownItems.Add(document_View.DocumentNumber.Trim() + " на визирование " + To);
                            tssSended.Text = document_View.DocumentNumber.Trim() + " на визирование " + To;
                        }
                    }
                }
                else
                {
                    SYB_acc.ModuleId = ModuleId;
                    SYB_acc.AddError("Вы не можете отправить документ на визирование, поскольку документ не был назначен Вам на исполнение.", "Chief - 1.1", "");
                }
            }
            catch { }
        }

        public void ToInfo()
        {
            tssSended.DropDownItems.Clear();
            try
            {
                if (true_Moving())
                {
                    Morray = Structure.push_new(tscExeTimer.Text, Find_ID_selecteDoc());
                    //if (Morray != null)
                    //Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.add_taskToList(Morray);
                    foreach (string To in Structure.newsSended)
                    {
                        tssSended.DropDownItems.Add(document_View.DocumentNumber.Trim() + " для информации " + To);
                        tssSended.Text = document_View.DocumentNumber.Trim() + " для информации " + To;
                    }
                }
                else
                {
                    SYB_acc.ModuleId = ModuleId;
                    SYB_acc.AddError("Вы не можете отправить документ для работы, поскольку документ не был назначен Вам на исполнение.", "Chief - 1.1", "");
                }
            }
            catch { }
        }

        private void кИсполнениюToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {

        }

        private void treeViewDepts_AfterSelect(object sender, TreeViewEventArgs e)
        {
            treeViewDepts.CheckBoxes = true;
        }

        private int Find_ID_selecteDoc()
        {
            return Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.Doc_id;
        }

        private int from_moving()
        {
            return Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.From_moving;
        }
        private int from_vizing()
        {
            return Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.From_vizing;
        }
        private int from_newing()
        {
            return Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.From_newing;
        }

        private bool true_Moving()
        {
            return Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.True_Moving;
        }

        private void кИсполнениюToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (true_Moving())
                    Morray = Structure.push_letter(false, tscExeTimer.Text, Find_ID_selecteDoc(), CBSigns.Text, from_moving());
                if (Morray != null)
                    Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.add_taskToList(Morray);
                else
                {
                    SYB_acc.ModuleId = ModuleId;
                    SYB_acc.AddError("Вы не можете отправить документ к исполнению, поскольку документ не был назначен Вам на исполнение.","Chief - 1.2","");
                }
            }
            catch (Exception ex) 
            {
                SYB_acc.EBBLP.AddError(ex.Message, "Chief - 1", ex.StackTrace);
                tssConsole.Text = (ex.Message); }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {
            
        }

        private void toolStripProgressBar1_Click(object sender, EventArgs e)
        {
            
        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.document_View.ValueOfDocument != null)
                {
                    if (document_View.ValueOfDocument.From_moving > 0)
                        document_New.Sender = document_View.ValueOfDocument.From_moving;
                    //if (Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.Tasklist[0].TASK_DocId == document_View.Doc_ID)
                    //    if(Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.Tasklist[0].Node.Name.CompareTo()
                    //        document_New.Sender = Period[tscbSelectDocs.SelectedIndex].DocsGroup.selectedDoc.Tasklist[0].Task_ID;
                    if(document_New.Sender>0)
                    {
                        txbmetadata.Text = "Ответ на поручение по документу " + this.document_View.DocumentNumber;
                        document_New.New_document = true;
                        document_New.Doc_ID = 0;
                        panelNewDoc.BringToFront();
                    }
                    else
                    {
                        SYB_acc.ModuleId = ModuleId;
                        SYB_acc.AddError("Вы не можете дать ответ, поскольку отсутствует поручение по данному документу", "Chief - 1.3", "");
                    }
                }
                else
                {
                    SYB_acc.ModuleId = ModuleId;
                    SYB_acc.AddError("Укажите документ, на который вам необходимо дать ответ.", "Chief - 1.4", "");
                }
            }
            catch { panelNewDoc.SendToBack(); }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            txbmetadata.Text = "Новый документ";
            document_New.New_document = true;
            document_New.Doc_ID = 0;
            panelNewDoc.BringToFront();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (document_New == null) document_New = new AMASControlRegisters.Document_Viewer(SYB_acc, tvDocsTree.SelectedNode);
                
            document_New.New_document = true;
            document_New.Doc_ID = 0;
        }

        private void DocSave()
        {
            if (document_New != null)
            {
                int parentDoc=0;
                if (document_New.Sender > 0)
                    parentDoc = document_View.Doc_ID;
                int document = AMAS_DBI.AMASCommand.Append_Indoor_document(KindBox.get_ident(), TemaBox.get_ident(), document_New.Annotation, parentDoc);
                if (document > 0)
                {
                    document_New.SaveDocument(document);
                    AMASCommand.AnswerDocument(document, document_New.Sender);
                }
                document_New.New_document = true;
                document_New.Doc_ID = 0;
            }
        }

        private void btExit_Click(object sender, EventArgs e)
        {
            panelNewDoc.SendToBack();
        }

        private void SYB_acc_ErrOfChief(string s, int ident)
        {
            tssConsole.Text = s;
            timer1.Interval = 20000;
            timer1.Enabled=true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            tssConsole.Text = "";
            timer1.Enabled = false;
        }

        private void наВизированиеToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void кИсполнениюToolStripMenuItem2_Click(object sender, EventArgs e)
        {

        }

            object missing = Missing.Value;
            Microsoft.Office.Interop.Word.Application Word_App = null; 
            Document Word_doc = null;
            AutoCorrect autocorrect;
            AutoCorrectEntries autoEntries;
            Documents Docs;
            _Document my_Doc;

        string FileLoad = "";



        public class Arraysheet
        {
            private int the_Id;
            private string the_Name;

            public Arraysheet(string Name_, int Id_)
            {

                the_Name = Name_;
                the_Id = Id_;
            }

            public string Name
            {
                get
                {
                    return the_Name;
                }
            }

            public string Id
            {
                get
                {
                    return the_Id.ToString();
                }
            }

            public override string ToString()
            {
                return the_Name;
            }
        }

        ArrayList TrueViza = new ArrayList();
        ArrayList FalseViza = new ArrayList();

        private void tsbViza_Click(object sender, EventArgs e)
        {
            panelViza.BringToFront();
            TrueViza.Clear();
            FalseViza.Clear();
            string sql = "select * from dbo.rkk_vizing_list";
            if (SYB_acc.Set_table("ChiefVizaSheet", sql, null))
                try
                {
                    int id;
                    string name;
                    for (int i = 0; i < SYB_acc.Rows_count; i++)
                    {
                        SYB_acc.Get_row(i);
                        id = (int)SYB_acc.Find_Field("id");
                        name = (string)SYB_acc.Find_Field("viza_yes");
                        if(name.Trim().Length>0)TrueViza.Add(new Arraysheet(name.Trim(), id));
                        name = (string)SYB_acc.Find_Field("viza_no");
                        if (name.Trim().Length > 0) FalseViza.Add(new Arraysheet(name.Trim(), id));
                    }
                }
                catch (Exception ex)
                {
                    SYB_acc.AddError(ex.Message, "" + sql, ex.StackTrace);
                }
                finally
                {
                    SYB_acc.ReturnTable();
                }
            listBoxtrue.DataSource = TrueViza;
            listBoxfalse.DataSource = FalseViza;
            if (TrueViza.Count > 0)
            {
                listBoxtrue.DisplayMember = "name";
                listBoxtrue.ValueMember = "id";
            }
            if (TrueViza.Count > 0)
            {
                listBoxfalse.DisplayMember = "name";
                listBoxfalse.ValueMember = "id";
            }
            labelViza.Text = "Визирование документа № "+document_View.DocumentNumber;
        }

        void listBoxfalse_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            AMASCommand.Document_Vized((int)Convert.ToInt32(listBoxtrue.SelectedValue), false, document_View.Doc_ID, textBoxViza.Text.Trim());
            panelViza.SendToBack();
        }

        void listBoxtrue_MouseDoubleClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            AMASCommand.Document_Vized((int)Convert.ToInt32(listBoxtrue.SelectedValue), true, document_View.Doc_ID, textBoxViza.Text.Trim());
            panelViza.SendToBack();
        }

        private void tsShowStructure_Click(object sender, EventArgs e)
        {
            treeViewDepts.BringToFront();
        }

        private void tsCalendarExe_Click(object sender, EventArgs e)
        {
            if (monthCalendarExe.Visible)
            {
                monthCalendarExe.Visible = false;
                monthCalendarExe.SendToBack();
            }
            else
            {
                monthCalendarExe.Visible = true;
                monthCalendarExe.BringToFront();
                monthCalendarExe.Top = toolStrip1.Top + toolStrip1.Height + 3;
                monthCalendarExe.Left = (this.Width - monthCalendarExe.Width) / 2;
            }
        }

        private void btAddFile_Click_1(object sender, EventArgs e)
        {
            if (document_New != null) document_New.File_Append();
        }

        private void btRemFile_Click_1(object sender, EventArgs e)
        {
            if (document_New != null) document_New.File_Delete();
        }

        private void tsSendAnswer_Click(object sender, EventArgs e)
        {
            AMASCommand.FastSendAnswer(document_View.Doc_ID);
        }

        private void tsShowGroups_Click(object sender, EventArgs e)
        {
            treeViewGroups.Visible = true;
            treeViewGroups.BringToFront();
        }

        private void btSave_Click_1(object sender, EventArgs e)
        {
            DocSave();
        }

        private void btExit_Click_1(object sender, EventArgs e)
        {
            panelNewDoc.SendToBack();
            document_New.New_document = false;
        }

        private void tsForMail_Click(object sender, EventArgs e)
        {
            SendToMail();
        }

        private void SendToMail()
        {
            int SendDocId=0;
            
            if(document_New!=null)
                if (document_New.New_document)
                {
                    int parentDoc = 0;
                    if (document_New.Sender > 0)
                        parentDoc = document_View.Doc_ID;
                    int document = AMAS_DBI.AMASCommand.Append_Indoor_document(KindBox.get_ident(), TemaBox.get_ident(), document_New.Annotation, parentDoc);
                    if (document > 0)
                    {
                        document_New.SaveDocument(document);
                        AMASCommand.AnswerDocument(document, document_New.Sender);

                        int kind = AMASCommand.SendKindToMail(document);
                        if (AMASCommand.SendOneKindToMail(kind))
                            if (AMASCommand.SendOneDocumentKindSended(kind, document))
                            {
                                SendDocId = -1;
                                MessageBox.Show("Попытка создания повторного исходящего документа для входящего неудачна");
                            }
                        if (SendDocId == 0)
                        {
                            SendDocId = document;
                            tssSended.DropDownItems.Clear();
                            tssSended.DropDownItems.Add(document_New.DocumentNumber.Trim() + " в почту ");
                            tssSended.Text = document_New.DocumentNumber.Trim() + " в почту " ;

                        }
                    }
                    document_New.New_document = true;
                    document_New.Doc_ID = 0;
                }
            if (SendDocId == 0)
                if (document_View.Doc_ID > 0)
                {
                    int kind = AMASCommand.SendKindToMail(document_View.Doc_ID);
                    if (AMASCommand.SendOneKindToMail(kind))
                        if (AMASCommand.SendOneDocumentKindSended(kind, document_View.Doc_ID))
                        {
                            SendDocId = -1;
                            MessageBox.Show("Попытка создания повторного исходящего документа для входящего неудачна");
                        }
                        else
                            if (document_View.TipOfDocument != (int)CommonValues.CommonClass.TypeofDocument.Indoor)
                            {
                                SendDocId = -1;
                                MessageBox.Show("Исходящий документ формируется только из документов внутренней корреспонденции");
                            }
                            else
                                if (AMASCommand.SendNotVizingDocument(document_View.Doc_ID))
                                    if (MessageBox.Show("Документ не всеми завизирован либо есть замечания. Вы готовы направить несогласованный документ в почту?", "Внимание! Документ не согласован.", MessageBoxButtons.YesNo) == DialogResult.Yes)
                                        SendDocId = document_View.Doc_ID;
                                    else SendDocId = -1;

                    /*
                     *Set myrs = ConPubs.OpenResultset("select count(*) as cnt from dba.mail_limit join dba.i_am_employee on dep_degree=cod where mail_limit.kind=" & Str(kind))
               i = myrs!cnt
               myrs.Close
          
              If i < 1 Then
              MsgBox "Вам не дано право посылать '" + kind_txt + "' в почту"
                     */
                    if (SendDocId == 0)
                    {
                        SendDocId = document_View.Doc_ID;
                        tssSended.DropDownItems.Clear();
                        tssSended.DropDownItems.Add(document_View.DocumentNumber.Trim() + " в почту ");
                        tssSended.Text = document_View.DocumentNumber.Trim() + " в почту ";
                    }
                }
            if (SendDocId > 0)
            {
                AMASCommand.SendDocumentToMail(SendDocId);
            }
        }

        private void tscbSelectDocs_Click(object sender, EventArgs e)
        {

        }

        private void tsFindDoc_Click(object sender, EventArgs e)
        {
            FindDocList.Visible = true;
            FindDocList.BringToFront();
        }

        private void btEditor_Click(object sender, EventArgs e)
        {
            document_New.Editor_append();
        }

        private void btDot_Click(object sender, EventArgs e)
        {
            DocumentProcessing DoPr = new DocumentProcessing(SYB_acc, document_New);
            DoPr.AddDot(KindBox.get_ident(), TemaBox.get_ident());
        }

        private void tsDenote_Click(object sender, EventArgs e)
        {
            string Fil=document_View.BaseFile();
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

            object PageN = 0;
            object IDI = document_View.Doc_ID;
            Variable VarDocId = Word_doc.Variables.Add("AMASDocId", ref IDI);
            Variable VarPageN = Word_doc.Variables.Add("AMASDocPageN", ref PageN);

           my_Doc = (_Document)Word_doc;

            Window win = Word_App.ActiveWindow;
            win.Visible = true;
            Word_App.DocumentBeforeClose += new ApplicationEvents4_DocumentBeforeCloseEventHandler(Word_App_DocPreClose);
        }

        void Word_App_DocPreClose(Document Doc, ref bool Cancel)
        {
            if (!Cancel) SaveDenoteDoc(Doc);
        }

        void SaveDenoteDoc(Document Doc)
        {
            string AMASDocId = "AMASDocId";
            object AMASDI = AMASDocId;
            Variable VarDocId = Doc.Variables.get_Item(ref AMASDI);
            int Document_ID = (int)Convert.ToInt32(VarDocId.Value);

            AMASDocId = "AMASDocPageN";
            AMASDI = AMASDocId;
            Variable VarDocPage = Doc.Variables.get_Item(ref AMASDI);
            int PageNum = (int)Convert.ToInt32(VarDocPage.Value);

            FileLoad = Doc.FullName;
            if (FileLoad.Length > 0)
            {
                string ss = "";
                object Fname = FileLoad;
                object wdFormatXMLDocument = (int)Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatXMLDocument;
                object FalseObj = false;
                object TrueObj = true;
                object NullStr = ss;
                try
                {
                    Word_doc.SaveAs(ref Fname, ref wdFormatXMLDocument, ref FalseObj, ref NullStr, ref FalseObj, ref NullStr, ref FalseObj, ref FalseObj, ref FalseObj, ref FalseObj, ref FalseObj,
                        ref missing, ref missing, ref missing, ref missing, ref missing);
                    object wdSaveChanges = (int)Microsoft.Office.Interop.Word.WdSaveOptions.wdSaveChanges;
                    object wdWordDocument = (int)Microsoft.Office.Interop.Word.WdOriginalFormat.wdOriginalDocumentFormat;
                    //Word_doc.Close(ref wdSaveChanges, ref wdWordDocument, ref FalseObj);
                    //Word_App.Quit(ref wdSaveChanges, ref wdWordDocument, ref FalseObj);
                    Word_App.ActiveWindow.Close(ref wdSaveChanges, ref FalseObj);

                    AMASCommand.Append_Correct(Document_ID, PageNum, CommonValues.CommonClass.GetImage(CommonValues.CommonClass.SaveFilewithHead(FileLoad)));//SaveFilewithHead(Fil)));
                }
                catch (Exception ex)
                { MessageBox.Show(ex.Message); }
            }
        }

        private void tsSeeNote_Click(object sender, EventArgs e)
        {
            string NothingString = "";
            object WdOpenFormat = (int)Microsoft.Office.Interop.Word.WdOpenFormat.wdOpenFormatAuto;
            object DocFalse = false;
            object DocTrue = true;
            object Nostring = NothingString;

            Word_App = new Microsoft.Office.Interop.Word.Application();
            Docs = Word_App.Documents;
            autocorrect = Word_App.AutoCorrect;
            autoEntries = autocorrect.Entries;

            string[] CrFiles = document_View.Load_Document_Corrections();
            foreach (string Fil in CrFiles)
            {
            object Template = Fil;
                Word_doc = Docs.Open(ref Template, ref DocFalse, ref DocFalse, ref DocFalse, ref Nostring, ref Nostring,
                    ref DocFalse, ref Nostring, ref Nostring, ref WdOpenFormat, ref missing, ref missing, ref missing, ref missing, ref missing, ref Nostring);
            
                Window win = Word_App.ActiveWindow;
                win.Caption = "Замечания к документу " + document_View.DocumentNumber;
            win.Visible = true;
            }
        }

        System.Windows.Forms.Form FindDeartmentOREmployee = null;
        private void btnFindEmpDep_Click(object sender, EventArgs e)
        {
            if (FindDeartmentOREmployee == null)
                FindDeartmentOREmployee = new FormSeekEmDep(Structure);
            else if (FindDeartmentOREmployee.Disposing)
                FindDeartmentOREmployee = new FormSeekEmDep(Structure);
            FindDeartmentOREmployee.ShowDialog();
        }

    }

    public class Sign_ids
    {
        private int[] Ident;
        private int[] Index;
        private string[] SignName;

        private int current_number;
        private string textBuffer = "";
        private string backtextBuffer = "";
        private int counter = 0;
        private int array_dimention = 0;
        private bool new_Address = false;

        private AMAS_DBI.Class_syb_acc AMASacc;

        public string NewSignName = "";
        public string ResultErr = "";
        public Sign_ids Child = null;
        public System.Windows.Forms.ComboBox SignBox;

        public Sign_ids(System.Windows.Forms.ComboBox CBox)
        {
            SignBox = CBox;
            this.SignBox.Click += new EventHandler(this_SignBox_Click);
            //this.SignBox.TextChanged += new EventHandler(this_SignBox_TextChanged);
            //this.SignBox.LostFocus += new EventHandler(this_SignBox_LostFocus);
            this.SignBox.SelectedIndexChanged += new EventHandler(this_SelectedIndexChanged);
            //this.SignBox.KeyPress += new KeyPressEventHandler(this_KeyPress);
            //this.SignBox.KeyUp += new KeyEventHandler(this_SignBox_KeyUp);
        }

        private void this_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Child != null) Child.clear();
        }

        private void this_SignBox_Click(object sender, EventArgs e)
        {
            cleanNewAddress();
        }

        private void cleanNewAddress()
        {
            new_Address = false;
            textBuffer = "";
            backtextBuffer = "";
        }

        private void this_SignBox_TextChanged(object sender, EventArgs e)
        {
            if (new_Address)
            {
                SignBox.Text = textBuffer ;//
                //SignBox.Text = textBuffer + backtextBuffer;
                //SignBox.SelectionStart = textBuffer.Length;
                //SignBox.SelectionLength = backtextBuffer.Length;
            }
        }

        private void this_SignBox_LostFocus(object sender, EventArgs e)
        {
        }

        public void AdditionalSign()
        {
            new_Address = true;
            try
            {
                for (int i = 0; i < SignName.Length; i++)
                {
                    if (SignName[i].Trim().CompareTo(SignBox.Text.Trim()) == 0)
                    {
                        new_Address = false;
                        break;
                    }
                }
            }
            catch { new_Address = false; }
            if (new_Address)
            {
                NewSignName = SignBox.Text.Trim();
                cleanNewAddress();
                if (NewSignName.Length == 0)
                {
                    //altSignName = new string[SignName.Length+1];
                    //altIdent = new int[Ident];
                    //get_number_by_text(NewSignName);
                }
            }
            else NewSignName = "";
        }

        private void this_SignBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (new_Address)
                    {
                        if (textBuffer.Length > 0)
                            textBuffer = textBuffer.Substring(0, textBuffer.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = SignName[number].Substring(textBuffer.Length);
                        SignBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Address = true;
                        backtextBuffer = "";
                        textBuffer = SignBox.Text.Substring(0, SignBox.Text.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = SignName[number].Substring(textBuffer.Length);
                        SignBox.Text = textBuffer;
                    }
                    break;
                case Keys.Right:
                    if (new_Address)
                    {
                        if (backtextBuffer.Length > 0)
                            textBuffer += backtextBuffer.Substring(0, 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = SignName[number].Substring(textBuffer.Length);
                        SignBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Address = true;
                        backtextBuffer = "";
                        int number = get_number_by_text(textBuffer);
                        textBuffer = SignBox.Text;
                        SignBox.Text = textBuffer;
                    }
                    break;
                case Keys.Delete:
                    if (new_Address)
                    {
                        backtextBuffer = "";
                        SignBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Address = true;
                        backtextBuffer = "";
                        textBuffer = SignBox.Text;
                        if (Child != null)
                            Child.clear();
                    }
                    break;
            }
        }

        private void this_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((char)e.KeyChar)
            {
                case (char)Keys.Back:
                    if (new_Address)
                    {
                        if (textBuffer.Length > 0)
                            textBuffer = textBuffer.Substring(0, textBuffer.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = SignName[number].Substring(textBuffer.Length);
                    }
                    else
                    {
                        new_Address = true;
                        backtextBuffer = "";
                        textBuffer = SignBox.Text;
                    }
                    break;
                case (char)Keys.End:
                    if (new_Address)
                    {
                        textBuffer += backtextBuffer;
                        backtextBuffer = "";
                    }
                    break;
                case (char)Keys.Home:
                    if (new_Address)
                    {
                        backtextBuffer = textBuffer + backtextBuffer;
                        textBuffer = "";
                    }
                    break;
                case (char)Keys.Escape:
                    backtextBuffer = "";
                    textBuffer = "";
                    SignBox.Text = "";
                    if (Child != null)
                        Child.clear();
                    break;
                default:
                    if ((e.KeyChar >= " ".ToCharArray()[0] && e.KeyChar <= "z".ToCharArray()[0]) || (e.KeyChar >= "А".ToCharArray()[0] && e.KeyChar <= "я".ToCharArray()[0]))
                    {
                        if (new_Address)
                        {
                            textBuffer += Convert.ToString(e.KeyChar);
                            int number = get_number_by_text(textBuffer);
                            if (number >= 0) backtextBuffer = SignName[number].Substring(textBuffer.Length);
                            else backtextBuffer = "";
                        }
                        else
                        {
                            new_Address = true;
                            textBuffer = SignBox.Text + Convert.ToString(e.KeyChar); 
                            backtextBuffer = "";
                            if (Child != null)
                                Child.clear();
                        }
                    }
                    break;
            }
        }

        public void connect(AMAS_DBI.Class_syb_acc Acc)
        {
            AMASacc = Acc;
        }

        private void add_ident(int indx, int idnt, string text)
        {
            Ident[counter] = idnt;
            Index[counter] = indx;
            SignName[counter] = text;
            counter++;
            current_number = counter;
        }

        public void clear()
        {
            counter = 0;
            cleanNewAddress();
            NewSignName = "";
            new_Address = false;
            current_number = -1;
            array_dimention = 0;
            if (SignBox != null)
            {
                SignBox.Items.Clear();
                SignBox.Refresh();
                SignBox.Text = "";
            }
            if (Child != null) Child.clear();
        }

        public int get_ident()
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (Index[i] == SignBox.SelectedIndex)
                    {
                        current_number = i;
                        return Ident[i];
                    }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 1", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        private int get_number_by_text(string text)
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (text.Length <= SignName[i].Length)
                        if (text.ToLower().CompareTo(SignName[i].Substring(0, text.Length).ToLower()) == 0)
                        {
                            current_number = i;
                            return i;
                        }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 2", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        public int get_index_by_text(string txt)
        {
            int num = get_number_by_text(txt);
            if (num >= 0)
            {
                SignBox.SelectedIndex = Index[num];
                return Index[num];
            }
            else
            {
                return -1;
            }
        }

        public int get_index(int idnt)
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (Ident[i] == idnt)
                    {
                        SignBox.SelectedIndex = Index[i];
                        current_number = i;
                        return Index[i];
                    }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 3", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        public void Select_Subject(string sql, string fld, string ids)
        {
            if (AMASacc!=null)
            if (AMASacc.Set_table("ChPat1", sql, null))
            {
                try
                {
                    string OName = "";
                    clear();
                    int ind = 0;
                    int id = 0;
                    array_dimention = AMASacc.Rows_count;
                    if (array_dimention>0)
                    {
                    Index = new int[array_dimention];
                    Ident = new int[array_dimention];
                    SignName = new string[array_dimention];

                    for (int i = 0; i < array_dimention; i++)
                    {
                        AMASacc.Get_row(i);
                        AMASacc.Find_Field(fld);
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                        else OName = "";
                        ind = SignBox.Items.Add(OName.Trim());
                        id = (int)AMASacc.Find_Field(ids);
                        add_ident(ind, id, OName);
                    }
                    }
                }
                catch (Exception err)
                {
                    AMASacc.EBBLP.AddError(err.Message, "Pattern - 4", err.StackTrace);
                    ResultErr = err.Message;
                }
                AMASacc.ReturnTable();
            }
            if (array_dimention > 0)
            {
                SignBox.SelectedIndex = 0;
                SignBox.Refresh();
            }
            else
            {
                current_number = -1;
                SignBox.SelectedIndex = -1;
                SignBox.Text = "";
                SignBox.Refresh();
            }
        }

        public void ADDress(string name, int idx)
        {
            int DIM = -1;
            int[] Id = new int[array_dimention + 1];
            int[] Ind = new int[array_dimention + 1];
            string[] AddrN = new string[array_dimention + 1];
            for (int i = 0; i < array_dimention; i++)
            {
                Id[i] = Ident[i];
                Ind[i] = Index[i];
                AddrN[i] = SignName[i];
            }
            Id[array_dimention] = idx;
            AddrN[array_dimention] = name;
            DIM = SignBox.Items.Add(name);
            Ind[array_dimention] = DIM;
            Ident = Id;
            Index = Ind;
            SignName = AddrN;
            array_dimention++;
            SignBox.SelectedIndex = DIM;

        }
   
    }



}

