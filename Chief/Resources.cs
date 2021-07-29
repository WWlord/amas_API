using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;
using ClassStructure;
using CommonValues;
using ClassPattern;
using ClassErrorProvider;

namespace Chief
{
    public partial class Resources : Form
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private ListOfAny ArchiveList;
        private ListOfAny KindList;
        private ListOfAny TemaList;
        private ListOfAny ComingList;
        private ListOfAny PrefixList;
        private ListOfAny SuffixList;
        private ListOfAny VizaList;

        private AssignList AssignTema;
        private AssignList AssignKind;
        private AssignList AssignComing;

        private PrefixSuffix PrefixToKind;
        private PrefixSuffix PrefixForKindJournal;
        private PrefixSuffix SuffixToTema;
        private PrefixSuffix SuffixForTemaJournal;

        private ClassPattern.FIleSystemShow FSS;

        private TemaForKind KndTm;
        private MultiSendKind MSK;
        public int ModuleId;


        public Resources(AMAS_DBI.Class_syb_acc ACC, ImageList ImageStd)
        {
            InitializeComponent();
            AMASacc = ACC;
            if (AMASCommand.Access == null) AMAS_DBI.AMASCommand.AccessCommands(ACC);
            tabResources.SelectedIndexChanged += new EventHandler(tabResources_SelectedIndexChanged);
            tabResources.SelectTab(0);
            ArchiveList = new ListOfAny((int)CommonClass.Lists.Archive, (Control)Archive, "Резолюции архивации", AMASacc);
            ModuleId = (int)ClassErrorProvider.ErrorBBLProvider.Modules.Resources;
            DeloRefresh();
        }

        ArrayList DeLoListing = null;

        private void DeloRefresh()
        {
            DeLoListing = (ArrayList)AMAS_DBI.AMASCommand.A_resolutions_list((int)CommonValues.CommonClass.Lists.Delo);
            cbJournal.DataSource = DeLoListing;
            cbJournal.Refresh();
            cbJournalTema.DataSource = DeLoListing;
            cbJournalTema.Refresh();
        }

        private void tabResources_SelectedIndexChanged(Object sender, EventArgs e)
        {
            switch (tabResources.SelectedTab.Name)
            {
                case "Archive":
                    if (ArchiveList == null) ArchiveList = new ListOfAny((int)CommonClass.Lists.Archive, (Control)Archive, "Резолюции архивации", AMASacc);
                    break;
                case "correspondence":
                    if (KindList == null)
                    {
                        KindList = new ListOfAny((int)CommonClass.Lists.Kind, (Control)panelCorrespondence, "Список видов документов", AMASacc);
                        KindList.LISTofKT.SelectedIndexChanged += new EventHandler(LISTofKT_SelectedIndexChanged);
                    }
                    break;
                case "Tema":
                    if (TemaList == null) TemaList = new ListOfAny((int)CommonClass.Lists.Tema, (Control)panelTema, "Список тем", AMASacc);
                    break;
                case "coming":
                    if (ComingList == null) ComingList = new ListOfAny((int)CommonClass.Lists.Coming, (Control)coming, "Список видов рассылки", AMASacc);
                    break;
                case "Prefix":
                    if (PrefixList == null) PrefixList = new ListOfAny((int)CommonClass.Lists.Prefix, (Control)panelPrefixList, "Список префиксов", AMASacc);
                    if (PrefixForKindJournal == null)
                    {
                        PrefixForKindJournal = new PrefixSuffix((int)CommonClass.Lists.Prefix, (Control)panelCorrPrefix, AMASacc, PrefixList.LISTofKT);
                        PrefixForKindJournal.listBoxPS.SelectedIndexChanged += new EventHandler(listBoxPS_SelectedIndexChanged);
                        PrefixForKindJournal.WelInOut = 1;
                        PrefixForKindJournal.Refresh();
                    }
                    else PrefixForKindJournal.Refresh();
                    break;
                case "Suffix":
                    if (SuffixList == null) SuffixList = new ListOfAny((int)CommonClass.Lists.Suffix, (Control)panelSuffixList, "Список суффиксов", AMASacc);
                    if (SuffixForTemaJournal == null)
                    {
                        SuffixForTemaJournal = new PrefixSuffix((int)CommonClass.Lists.Suffix, (Control)panelTemasList, AMASacc, SuffixList.LISTofKT);
                        SuffixForTemaJournal.listBoxPS.SelectedIndexChanged += new EventHandler(SuffixlistBoxPS_SelectedIndexChanged);
                        SuffixForTemaJournal.WelInOut = 1;
                        SuffixForTemaJournal.Refresh();
                    }
                    else SuffixForTemaJournal.Refresh();
                    break;
                case "Viza":
                    if (VizaList == null) VizaList = new ListOfAny((int)CommonClass.Lists.Viza, (Control)Viza, "Список виз ЗА / ПРОТИВ", AMASacc);
                    break;
                case "SelectTema":
                    if (AssignTema == null) AssignTema = new AssignList(AMASacc, (int)CommonClass.Lists.Tema, (Control)SelectTema);
                    break;
                case "SelectCorresp":
                    if (AssignKind == null) AssignKind = new AssignList(AMASacc, (int)CommonClass.Lists.Kind, (Control)SelectCorresp);
                    break;
                case "SelectComing":
                    if (AssignComing == null) AssignComing = new AssignList(AMASacc, (int)CommonClass.Lists.Coming, (Control)SelectComing);
                    break;
                case "Prefixkind":
                    if (PrefixToKind == null) PrefixToKind = new PrefixSuffix((int)CommonClass.Lists.Prefix, (Control)Prefixkind, AMASacc);
                    else PrefixToKind.Refresh();
                    break;
                case "SuffixTema":
                    if (SuffixToTema == null) SuffixToTema = new PrefixSuffix((int)CommonClass.Lists.Suffix, (Control)SuffixTema, AMASacc);
                    else SuffixToTema.Refresh();
                    break;
                case "Pattern":
                    if (FSS == null) FSS = new FIleSystemShow(treeViewFiles, webBrowserPattern, treeViewKT, AMASacc, FIleSystemShow.ExecModule.Resourse);
                    break;
                case "KindTema":
                    if (KndTm == null) KndTm = new TemaForKind(listBoxKindTema, listViewKindTema);
                    else KndTm.Refresh();
                    break;
                case "Send":
                    if (MSK == null) MSK = new MultiSendKind(listViewMultisend, AMASacc);
                    else MSK.Refresh();
                    break;
            }
        }

        
        void listBoxPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            mtbCounter.Text = AMASCommand.SeekPrefixCount(PrefixForKindJournal.Ident).ToString();
        }

        void SuffixlistBoxPS_SelectedIndexChanged(object sender, EventArgs e)
        {
            maskedTextBox1.Text = AMASCommand.SeekSuffixCount(SuffixForTemaJournal.Ident).ToString();
        }

        void LISTofKT_SelectedIndexChanged(object sender, EventArgs e)
        {
            ResWIO = false;
            KLSelect();
            ResWIO = true;
        }

        void KindList_IndexChecked(int Id)
        {
            if (!ResWIO)
            {
                int WIO = -1;
                if (radioButton6.Checked) WIO = 0;
                else if (radioButton5.Checked) WIO = 1;
                else if (radioButton4.Checked) WIO = 2;
                AMASCommand.CorrespondenTypSet(Id, WIO);
            }
        }

        void KLSelect()
        {
            int Id = (int)KindList.LISTofKT.SelectedIndex;
            if (Id >= 0)
                try
                {
                    CommonValues.CommonClass.Arraysheet ASh = (CommonClass.Arraysheet)KindList.RList[Id];
                    int WellInOut = AMASCommand.CorrespondenTyp((int)Convert.ToInt32(ASh.Id));
                    switch (WellInOut)
                    {
                        case 0:
                            radioButton6.Checked = true;
                            radioButton5.Checked = false;
                            radioButton4.Checked = false;
                            break;
                        case 1:
                            radioButton6.Checked = false;
                            radioButton5.Checked = true;
                            radioButton4.Checked = false;
                            break;
                        case 2:
                            radioButton6.Checked = false;
                            radioButton5.Checked = false;
                            radioButton4.Checked = true;
                            break;
                        default:
                            radioButton6.Checked = false;
                            radioButton5.Checked = false;
                            radioButton4.Checked = false;
                            break;
                    }
                }
                catch { }
        }

        private class JournalAttributes
        {
            private int WellInOut = -1;
            //private int Counter = 0;
            //private string JouName = "";
            private bool Modify = false;

            public int WIO
            {
                get { return WellInOut; }
                set
                {
                    WellInOut = value;
                    if (Modify)
                    {

                    }
                }
            }

            JournalAttributes(bool Mod)
            {
                Modify = Mod;
            }

        }

        private class ListOfAny
        {

            public ListBox LISTofKT
            {
                get { return listResolutions; }
            }

            private System.Windows.Forms.GroupBox groupBox1;
            private System.Windows.Forms.GroupBox groupBoxNewResolution;
            private System.Windows.Forms.ListBox listResolutions;
            private System.Windows.Forms.TextBox Resolution;
            private System.Windows.Forms.Label AddResolution;
            private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
            private AMAS_DBI.Class_syb_acc AMASacc;

            System.Windows.Forms.ToolStripMenuItem add;
            System.Windows.Forms.ToolStripMenuItem del;
            System.Windows.Forms.ToolStripMenuItem ren;
            System.Windows.Forms.ToolStripMenuItem loc;

            public ArrayList RList;

            private int EnumListing;

            public ListOfAny(int deck, Control ParentControl, string TEXT, Class_syb_acc ACC)
            {
                AMASacc = ACC;
                EnumListing = deck;
                this.groupBox1 = new System.Windows.Forms.GroupBox();
                this.groupBoxNewResolution = new System.Windows.Forms.GroupBox();
                this.listResolutions = new System.Windows.Forms.ListBox();
                this.Resolution = new System.Windows.Forms.TextBox();
                this.groupBox1.SuspendLayout();
                this.AddResolution = new System.Windows.Forms.Label();
                ParentControl.Controls.Add(this.groupBox1);
                // 
                // groupBox1
                // 
                this.groupBox1.Controls.Add(this.groupBoxNewResolution);
                this.groupBox1.Controls.Add(this.listResolutions);
                this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
                this.groupBox1.Location = new System.Drawing.Point(0, 0);
                this.groupBox1.Name = "groupBox1";
                this.groupBox1.Size = new System.Drawing.Size(543, 261);
                this.groupBox1.TabIndex = 0;
                this.groupBox1.TabStop = false;
                this.groupBox1.Text = TEXT;
                // 
                // listResolutions
                // 
                this.listResolutions.Dock = System.Windows.Forms.DockStyle.Top;
                this.listResolutions.FormattingEnabled = true;
                this.listResolutions.Location = new System.Drawing.Point(3, 16);
                this.listResolutions.Name = "listResolutions";
                this.listResolutions.Size = new System.Drawing.Size(537, 225);
                this.listResolutions.TabIndex = 1;
                // 
                // groupBoxNewResolution
                // 
                this.groupBoxNewResolution.Controls.Add(this.Resolution);
                this.groupBoxNewResolution.Controls.Add(this.AddResolution);
                this.groupBoxNewResolution.Dock = System.Windows.Forms.DockStyle.Bottom;
                this.groupBoxNewResolution.Location = new System.Drawing.Point(0, 0);
                this.groupBoxNewResolution.Name = "groupBoxNewResolution";
                this.groupBoxNewResolution.Size = new System.Drawing.Size(543, 45);
                this.groupBoxNewResolution.TabIndex = 2;
                this.groupBoxNewResolution.TabStop = false;
                // Label AddResolution
                // 
                this.AddResolution.Dock = System.Windows.Forms.DockStyle.Left;
                this.AddResolution.Location = new System.Drawing.Point(0, 0);
                this.AddResolution.Name = "AddResolution";
                this.AddResolution.Size = new System.Drawing.Size(60, 20);
                this.AddResolution.Text = "Добавить";
                this.AddResolution.TabIndex = 3;
                // 
                // Resolution
                // 
                this.Resolution.Dock = System.Windows.Forms.DockStyle.Fill;
                this.Resolution.Location = new System.Drawing.Point(33, 0);
                this.Resolution.Name = "Resolution";
                this.Resolution.Size = new System.Drawing.Size(20, 20);
                this.Resolution.Enabled = true;
                this.Resolution.TabIndex = 4;
                this.groupBox1.ResumeLayout(false);
                this.groupBox1.PerformLayout();
                //
                // Menu
                //
                this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
                this.contextMenuStrip1.SuspendLayout();
                this.add = new System.Windows.Forms.ToolStripMenuItem();
                this.del = new System.Windows.Forms.ToolStripMenuItem();
                this.ren = new System.Windows.Forms.ToolStripMenuItem();
                this.loc = new System.Windows.Forms.ToolStripMenuItem();
                this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    //this.add,
                    this.del,
                    this.ren
                    //,
                    //this.loc
                });
                this.contextMenuStrip1.Name = "contextMenuStrip1";
                this.contextMenuStrip1.Size = new System.Drawing.Size(224, 92);
                // 
                // add
                // 
                this.add.Name = "add";
                this.add.Size = new System.Drawing.Size(223, 22);
                this.add.Text = "Добавить";
                // 
                // del
                // 
                this.del.Name = "del";
                this.del.Size = new System.Drawing.Size(223, 22);
                this.del.Text = "Удалить";
                // 
                // ren
                // 
                this.ren.Name = "ren";
                this.ren.Size = new System.Drawing.Size(223, 22);
                this.ren.Text = "Обновить список";
                // 
                // loc
                // 
                this.loc.Name = "loc";
                this.loc.Size = new System.Drawing.Size(223, 22);
                this.loc.Text = "Назначить";

                this.listResolutions.ContextMenuStrip = this.contextMenuStrip1;
                this.contextMenuStrip1.ResumeLayout(false);

                contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip3_ItemClicked);

                listResolutions.Height = groupBox1.Height - Resolution.Height;

                groupBox1.Resize += new EventHandler(groupBox1_Resize);
                Resolution.KeyPress += new KeyPressEventHandler(Resolution_KeyPress);
                resolution_list();
            }

            private void contextMenuStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
            {
                switch (e.ClickedItem.Name)
                {
                    case "add":
                        //addDegree();
                        break;
                    case "del":
                        if (AMAS_DBI.AMASCommand.DeleteThe_resolution(EnumListing, (int)Convert.ToInt32(listResolutions.SelectedValue)))
                        {
                            resolution_list();
                        }
                        break;
                    case "ren":
                        resolution_list();
                        break;
                    case "loc":
                        //loc_degree();
                        break;
                }
            }

            private void groupBox1_Resize(object sender, EventArgs e)
            {
                Control control = (Control)sender;
                listResolutions.Height = control.Height - Resolution.Height;
            }

            private void Resolution_KeyPress(Object sender, KeyPressEventArgs e)
            {
                switch ((char)e.KeyChar)
                {
                    case (char)Keys.Enter:
                        if (AMAS_DBI.AMASCommand.AddThe_resolution(EnumListing, Resolution.Text.Trim()))
                            resolution_list();
                        break;
                }
            }

            public void Refresh(int deck)
            {
                EnumListing = deck;
                resolution_Flash();
            }

            private void resolution_list()
            {
                if (resolution_Flash())
                    try
                    {
                        Resolution.Clear();
                        listResolutions.SelectedIndexChanged += new EventHandler(listResolutions_SelectedIndexChanged);
                    }
                    catch (Exception e)
                    {
                        AMASacc.EBBLP.AddError(e.Message, "Resources - 1", e.StackTrace);
                    }
            }

            private bool resolution_Flash()
            {
                bool res = false;
                listResolutions.DataSource = null;
                listResolutions.Items.Clear();
                try
                {
                    RList = AMAS_DBI.AMASCommand.A_resolutions_list(EnumListing);
                    listResolutions.DataSource = RList;
                    listResolutions.DisplayMember = "name";
                    listResolutions.ValueMember = "id";
                    res = true;
                }
                catch (Exception e)
                {
                    AMASacc.EBBLP.AddError(e.Message, "Resources - 1.1", e.StackTrace);
                    res = false;
                }
                return res;
            }

            public int ListIdent = -1;

            void listResolutions_SelectedIndexChanged(object sender, EventArgs e)
            {
                try
                {
                    CommonClass.Arraysheet LX = (CommonClass.Arraysheet)RList[(int)listResolutions.SelectedIndex];
                    ListIdent = Convert.ToInt32(LX.Id);
                }
                catch { }
            }
        }

        private class AssignList
        {
            private ClassStructure.Structure StructWithDegree;
            private System.Windows.Forms.SplitContainer splitContain;
            private System.Windows.Forms.TreeView treeViewAll;
            private System.Windows.Forms.ListView listViewS;

            private int enumerate = 0;
            private int Degree_ID = 0;
            private int Employee_ID = 0;
            private int Man_ID = 0;
            private TreeNode thisNode = null;

            bool check_role_enable = false;

            public AssignList(Class_syb_acc SYB, int deck, Control control)
            {
                enumerate = deck;
                splitContain = new System.Windows.Forms.SplitContainer();
                treeViewAll = new System.Windows.Forms.TreeView();
                listViewS = new System.Windows.Forms.ListView();
                StructWithDegree = new Structure(SYB, treeViewAll);

                control.Controls.Add(this.splitContain);

                this.splitContain.Panel1.SuspendLayout();
                this.splitContain.Panel2.SuspendLayout();
                this.splitContain.SuspendLayout();
                // 
                // splitContain
                // 
                this.splitContain.Dock = System.Windows.Forms.DockStyle.Fill;
                this.splitContain.Location = new System.Drawing.Point(0, 0);
                this.splitContain.Name = "splitContain";
                // 
                // splitContain.Panel1
                // 
                this.splitContain.Panel1.Controls.Add(this.treeViewAll);
                // 
                // splitContain.Panel2
                // 
                this.splitContain.Panel2.Controls.Add(this.listViewS);
                this.splitContain.Size = new System.Drawing.Size(543, 261);
                this.splitContain.SplitterDistance = 219;
                this.splitContain.TabIndex = 0;
                // 
                // treeViewAll
                // 
                this.treeViewAll.Dock = System.Windows.Forms.DockStyle.Fill;
                this.treeViewAll.Location = new System.Drawing.Point(0, 0);
                this.treeViewAll.Name = "treeViewAll";
                this.treeViewAll.Size = //new System.Drawing.Size(219, 261);
                    this.splitContain.Panel1.ClientSize;
                this.treeViewAll.TabIndex = 0;
                // 
                // listViewS
                // 
                this.listViewS.CheckBoxes = true;
                this.listViewS.Dock = System.Windows.Forms.DockStyle.Fill;
                this.listViewS.Location = new System.Drawing.Point(0, 0);
                this.listViewS.Name = "listViewS";
                this.listViewS.Size = //new System.Drawing.Size(320, 261);
                    this.splitContain.Panel2.ClientSize;
                this.listViewS.TabIndex = 0;
                this.listViewS.UseCompatibleStateImageBehavior = false;
                this.listViewS.View = System.Windows.Forms.View.List;
                this.listViewS.Sorting = SortOrder.Ascending;

                this.splitContain.Panel1.ResumeLayout(false);
                this.splitContain.Panel2.ResumeLayout(false);
                this.splitContain.ResumeLayout(false);

                treeViewAll.ExpandAll();

                treeViewAll.AfterSelect += new TreeViewEventHandler(treeViewAll_AfterSelect);
                listViewS.ItemCheck += new ItemCheckEventHandler(listViewS_ItemCheck);
                this.listViewS.Refresh();
                this.treeViewAll.Refresh();
            }

            private void listViewS_ItemCheck(object sender, ItemCheckEventArgs e)
            {
                if (check_role_enable)
                {
                    int assg = (int)Convert.ToInt32(listViewS.Items[e.Index].SubItems[1].Text);
                    if (e.CurrentValue == CheckState.Unchecked)
                        AMAS_DBI.AMASCommand.AddWFL_assign(enumerate, assg, Degree_ID);
                    else
                        AMAS_DBI.AMASCommand.DeleteWFL_assign(enumerate, assg, Degree_ID);
                }
            }

            private void treeViewAll_AfterSelect(Object sender, TreeViewEventArgs e)
            {
                if (thisNode != e.Node)
                {
                    listViewS.Clear();
                    if (e.Node.Name.Substring(0, 1).ToLower().CompareTo("e") == 0)
                    {
                        thisNode = e.Node;
                        Man_ID = (int)StructWithDegree.Get_ManId(thisNode);
                        Employee_ID = (int)Convert.ToInt32(thisNode.Name.Substring(1));
                        Degree_ID = (int)StructWithDegree.Get_DegreeId(thisNode);
                        List_of_assigns();
                    }
                    else
                    {
                        Man_ID = 0;
                        Employee_ID = 0;
                        Degree_ID = 0;
                        thisNode = null;
                    }
                }
            }

            private void List_of_assigns()
            {
                if (Degree_ID > 0)
                {
                    check_role_enable = false;
                    Array roles = AMAS_DBI.AMASCommand.AssignWFL_list(enumerate, Degree_ID);// Employee_ID ); //Man_ID );
                    if (roles != null)
                    {
                        ListViewItem itemPerson;
                        int rows = roles.Length / 3;
                        string b = "0";
                        for (int i = 0; i < rows; i++)
                        {
                            itemPerson = listViewS.Items.Add((string)roles.GetValue(i, 1));
                            itemPerson.SubItems.Add((string)roles.GetValue(i, 0));
                            b = (string)roles.GetValue(i, 2);
                            if (b.Trim().CompareTo("1") == 0)
                                itemPerson.Checked = true;
                            else
                                itemPerson.Checked = false;
                        }
                        listViewS.Sort();
                    }
                    check_role_enable = true;
                }
            }
        }
        private class PrefixSuffix
        {
            private System.Windows.Forms.SplitContainer splitContainerPS;
            public System.Windows.Forms.ListBox listBoxPS;
            private System.Windows.Forms.ListView listViewPS;
            private AMAS_DBI.Class_syb_acc AMASacc;

            private bool check_role_enable = false;
            private int enumerate = 0;
            private int PS_ID = 0;

            public int Ident { get { return PS_ID; } } 

            public PrefixSuffix(int enumer, Control control, Class_syb_acc Acc, ListBox KTList)
            {
                AMASacc = Acc;
                enumerate = enumer;
                this.listViewPS = new System.Windows.Forms.ListView();


                // 
                // listBoxPS
                // 
                listBoxPS = KTList;
                // 
                // listViewPS
                // 
                this.listViewPS.Dock = System.Windows.Forms.DockStyle.Fill;
                this.listViewPS.Location = new System.Drawing.Point(0, 0);
                this.listViewPS.Name = "listViewPS" + enumer.ToString();
                this.listViewPS.TabIndex = 0;
                this.listViewPS.UseCompatibleStateImageBehavior = false;
                this.listViewPS.View = View.List;
                this.listViewPS.Sorting = SortOrder.Ascending;
                this.listViewPS.CheckBoxes = true;
                control.Controls.Add(this.listViewPS);

                listBoxPS.SelectedIndexChanged += new EventHandler(listBoxPS_SelectedIndexChanged);
                listViewPS.ItemCheck += new ItemCheckEventHandler(listViewPS_ItemCheck);
                Refresh();
                this.listBoxPS.Refresh();
                this.listViewPS.Refresh();
                control.Refresh();
            }

            public PrefixSuffix(int enumer, Control control, Class_syb_acc Acc)
            {
                AMASacc = Acc;
                enumerate = enumer;
                this.splitContainerPS = new System.Windows.Forms.SplitContainer();
                this.listBoxPS = new System.Windows.Forms.ListBox();
                this.listViewPS = new System.Windows.Forms.ListView();
                this.splitContainerPS.Panel1.SuspendLayout();
                this.splitContainerPS.Panel2.SuspendLayout();
                this.splitContainerPS.SuspendLayout();

                control.Controls.Add(this.splitContainerPS);

                // 
                // splitContainerPS
                // 
                this.splitContainerPS.Dock = System.Windows.Forms.DockStyle.Fill;
                this.splitContainerPS.Location = new System.Drawing.Point(0, 0);
                this.splitContainerPS.Name = "splitContainerPS";
                // 
                // splitContainerPS.Panel1
                // 
                this.splitContainerPS.Panel1.Controls.Add(this.listBoxPS);
                // 
                // splitContainerPS.Panel2
                // 
                this.splitContainerPS.Panel2.Controls.Add(this.listViewPS);
                this.splitContainerPS.Size = new System.Drawing.Size(543, 243);
                this.splitContainerPS.SplitterDistance = 181;
                this.splitContainerPS.TabIndex = 0;
                // 
                // listBoxPS
                // 
                this.listBoxPS.Dock = System.Windows.Forms.DockStyle.Fill;
                this.listBoxPS.FormattingEnabled = true;
                this.listBoxPS.Location = new System.Drawing.Point(0, 0);
                this.listBoxPS.Name = "listBoxPS";
                this.listBoxPS.Size = //new System.Drawing.Size(181, 238);
                this.splitContainerPS.Panel1.ClientSize;
                this.listBoxPS.TabIndex = 0;
                // 
                // listViewPS
                // 
                this.listViewPS.Dock = System.Windows.Forms.DockStyle.Fill;
                this.listViewPS.Location = new System.Drawing.Point(0, 0);
                this.listViewPS.Name = "listViewPS";
                this.listViewPS.Size = //new System.Drawing.Size(358, 243);
                    this.splitContainerPS.Panel2.ClientSize;
                this.listViewPS.TabIndex = 0;
                this.listViewPS.UseCompatibleStateImageBehavior = false;
                this.listViewPS.View = View.List;
                this.listViewPS.Sorting = SortOrder.Ascending;
                this.listViewPS.CheckBoxes = true;

                this.splitContainerPS.Panel1.ResumeLayout(false);
                this.splitContainerPS.Panel2.ResumeLayout(false);
                this.splitContainerPS.ResumeLayout(false);

                listBoxPS.SelectedIndexChanged += new EventHandler(listBoxPS_SelectedIndexChanged);
                listViewPS.ItemCheck += new ItemCheckEventHandler(listViewPS_ItemCheck);
                Refresh();
                this.listBoxPS.Refresh();
                this.listViewPS.Refresh();
                control.Refresh();
            }


            private void listBoxPS_SelectedIndexChanged(object sender, System.EventArgs e)
            {
                try
                {
                    PS_ID = (int)Convert.ToInt32(listBoxPS.SelectedValue);
                }
                catch { PS_ID = 0; }
                List_of_KindTema();
            }

            private void listViewPS_ItemCheck(object sender, ItemCheckEventArgs e)
            {
                if (check_role_enable)
                {
                    int assg = (int)Convert.ToInt32(listViewPS.Items[e.Index].SubItems[1].Text);
                    if (e.CurrentValue == CheckState.Unchecked)
                        AMAS_DBI.AMASCommand.Add_PS(enumerate, assg, PS_ID);
                    else
                        AMAS_DBI.AMASCommand.Remove_PS(enumerate, assg);
                }
            }

            ArrayList PSList;
            public void Refresh()
            {
                listBoxPS.DataSource = null;
                listBoxPS.Items.Clear();
                listViewPS.Clear();
                try
                {
                    PSList = AMAS_DBI.AMASCommand.A_resolutions_list(enumerate);
                    listBoxPS.DataSource = PSList;
                    listBoxPS.DisplayMember = "name";
                    listBoxPS.ValueMember = "id";
                }
                catch (Exception e)
                {
                    AMASacc.EBBLP.AddError(e.Message, "Resources - 2", e.StackTrace);
                }
            }

            public int WelInOut = -1;

            private void List_of_KindTema()
            {
                listViewPS.Clear();
                if (PS_ID > 0)
                {
                    check_role_enable = false;
                    Array roles = AMAS_DBI.AMASCommand.PS_list(enumerate, PS_ID, WelInOut);
                    if (roles != null)
                    {
                        ListViewItem itemPerson;
                        int rows = roles.Length / 3;
                        string b = "0";
                        for (int i = 0; i < rows; i++)
                        {
                            itemPerson = listViewPS.Items.Add((string)roles.GetValue(i, 1));
                            itemPerson.SubItems.Add((string)roles.GetValue(i, 0));
                            b = (string)roles.GetValue(i, 2);
                            b = b.Trim();
                            switch (b)
                            {
                                case "0":
                                    itemPerson.Checked = false;
                                    itemPerson.ForeColor = Color.DarkGray;
                                    break;
                                case "1":
                                    itemPerson.Checked = true;
                                    itemPerson.ForeColor = Color.DarkGreen;
                                    break;
                                case "2":
                                    itemPerson.Checked = false;
                                    itemPerson.ForeColor = Color.DarkViolet;
                                    break;
                            }
                        }
                        listViewPS.Sort();
                    }
                    check_role_enable = true;
                }
            }
        }


        private class TemaForKind
        {
            ArrayList KindList = null;
            Array TemaList = null;
            ListBox Kinds;
            ListView Temy;

            public TemaForKind(ListBox Kd, ListView Tm)
            {
                Kinds = Kd;
                Temy = Tm;
                Refresh();
                Kinds.SelectedIndexChanged += new EventHandler(Kinds_SelectedIndexChanged);
                Temy.ItemChecked += new ItemCheckedEventHandler(Temy_ItemChecked);
            }

            public void Refresh()
            {
                Kinds.DataSource = null;
                int en = (int)CommonClass.Lists.Kind;
                KindList = AMASCommand.A_resolutions_list(en);
                Kinds.DataSource = KindList;
                Kinds.DisplayMember = "name";
                Kinds.ValueMember = "id";
                Temy.Clear();
            }

            private bool CheckTema = false;
            private void Temy_ItemChecked(Object sender, ItemCheckedEventArgs e)
            {
                if (CheckTema)
                    if (Kinds.SelectedIndex >= 0)
                        AMASCommand.ADDelTemaForKind((int)Convert.ToInt32(Kinds.SelectedValue), (int)Convert.ToInt32(e.Item.SubItems[1].Text), e.Item.Checked);
            }

            private void Kinds_SelectedIndexChanged(object sender, System.EventArgs e)
            {
                Temy.Clear();
                TemaList = AMASCommand.TemaForKind_list((int)Convert.ToInt32(Kinds.SelectedValue));
                if (TemaList != null)
                {
                    CheckTema = false;
                    int rows = TemaList.Length / 3;
                    ListViewItem itema;
                    string s;
                    for (int i = 0; i < rows; i++)
                    {
                        itema = Temy.Items.Add((string)TemaList.GetValue(i, 1));
                        itema.SubItems.Add((string)TemaList.GetValue(i, 0));
                        s = (string)TemaList.GetValue(i, 2);
                        if (s.Trim().CompareTo("0") == 0) itema.Checked = false;
                        else itema.Checked = true;
                    }
                }
                CheckTema = true;
            }
        }

        private class MultiSendKind
        {
            private AMAS_DBI.Class_syb_acc AMASacc;
            Array KindList = null;
            ListView Kinds;

            private bool CheckK = false;

            public MultiSendKind(ListView KD, Class_syb_acc Acc)
            {
                AMASacc = Acc;
                Kinds = KD;
                CheckK = false;
                Kinds.ItemChecked += new ItemCheckedEventHandler(Kinds_ItemChecked);
                Refresh();
            }
            public void Dispose()
            {
                CheckK = false;
            }

            private void Kinds_ItemChecked(Object sender, ItemCheckedEventArgs e)
            {
                try
                {
                    if (CheckK)
                        AMASCommand.MultiSendChange((int)Convert.ToInt32(e.Item.SubItems[1].Text));
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Resources - 5", ex.StackTrace);
                }
            }

            public void Refresh()
            {
                Kinds.Clear();
                KindList = AMASCommand.MultiSendKind();
                if (KindList != null)
                {
                    CheckK = false;
                    int rows = KindList.Length / 3;
                    ListViewItem ikind;
                    string s;
                    for (int i = 0; i < rows; i++)
                    {
                        ikind = Kinds.Items.Add((string)KindList.GetValue(i, 1));
                        ikind.SubItems.Add((string)KindList.GetValue(i, 0));
                        s = (string)KindList.GetValue(i, 2);
                        if (s.Trim().CompareTo("False") == 0) ikind.Checked = false;
                        else ikind.Checked = true;
                    }
                }
                CheckK = true;
            }
        }

        private void treeViewFiles_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void treeViewKT_AfterSelect(object sender, TreeViewEventArgs e)
        {

        }

        private void webBrowserPattern_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            int wiodoc = 0;
            if (radioButton6.Checked) wiodoc = 0;
            if (radioButton5.Checked) wiodoc = 1;
            if (radioButton4.Checked) wiodoc = 2;

            AMASCommand.AddCorrWIO(wiodoc, 1);
        }

        public bool ResWIO = true;

        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            if (ResWIO)
            {
                ResWIO = false;
                radioButton6.Checked = true;
                radioButton5.Checked = false;
                radioButton4.Checked = false;
                if (KindList.ListIdent >= 0)
                    KindList_IndexChecked(KindList.ListIdent);
                ResWIO = true;
            }
        }

        private void radioButton5_CheckedChanged(object sender, EventArgs e)
        {
            if (ResWIO)
            {
                ResWIO = false;
                radioButton6.Checked = false;
                radioButton5.Checked = true;
                radioButton4.Checked = false;
                if (KindList.ListIdent >= 0)
                    KindList_IndexChecked(KindList.ListIdent);
                ResWIO = true;
            }
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            if (ResWIO)
            {
                ResWIO = false;
                radioButton6.Checked = false;
                radioButton5.Checked = false;
                radioButton4.Checked = true;
                if (KindList.ListIdent >= 0)
                    KindList_IndexChecked(KindList.ListIdent);
                ResWIO = true;
            }
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            PrefixForKindJournal.WelInOut = 1;
            PrefixForKindJournal.Refresh();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            PrefixForKindJournal.WelInOut = 2;
            PrefixForKindJournal.Refresh();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int Id = AMASCommand.SeekDelo(cbJournal.Text);
            if (Id <0)
            {
                Id = AMASCommand.AddDelo(cbJournal.Text.Trim(), tbJourDescr.Text.Trim());
            }
            if(Id>=0)
                AMASCommand.assignKindDelo(PrefixForKindJournal.Ident, Id);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int Id = AMASCommand.SeekDelo(cbJournalTema.Text);
            if (Id < 0)
            {
                Id = AMASCommand.AddDelo(cbJournalTema.Text.Trim(), tbJourTemaDescr.Text.Trim());
            }
            if (Id >= 0)
                AMASCommand.assignTemaDelo(SuffixForTemaJournal.Ident, Id);
        }

        private void cbJournalTema_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbJourTemaDescr.Text = AMASCommand.SeekDeloDescription(cbJournalTema.Text.Trim());
        }

        private void cbJournal_SelectedIndexChanged(object sender, EventArgs e)
        {
            tbJourDescr.Text = AMASCommand.SeekDeloDescription(cbJournal.Text.Trim());
        }

        private void radioButton7_CheckedChanged(object sender, EventArgs e)
        {
            SuffixForTemaJournal.WelInOut = 1;
            SuffixForTemaJournal.Refresh();
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            SuffixForTemaJournal.WelInOut = 2;
            SuffixForTemaJournal.Refresh();
        }
    }

}