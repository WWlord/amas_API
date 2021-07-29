using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AMAS_Query;
using AMAS_DBI;
using ClassStructure;

namespace AMASControlRegisters
{
    public partial class UCDocsTree : UserControl
    {
        private Class_syb_acc SybAcc;
        const int MAX_TREE_Fall = 20;
        private int year = 0;
        private int month = 0;
        DocTipGrow DTG = null;
        private AMASControlRegisters.Document_Viewer ShowDocument;
        private ClassStructure.Structure A_Struct;

        public UCDocsTree(Class_syb_acc Acc, AMASControlRegisters.Document_Viewer DocView )
        {
            InitializeComponent();

            SybAcc = Acc;

            treeViewDocs.Nodes.Add("Current", "Очередные документы", "Folder_stuffed");
            treeViewDocs.Nodes.Add("Executed", "Исполненные документы", "Folder_stuffed");
            treeViewDocs.Nodes.Add("Sended", "Назначенные документы", "Folder_stuffed");
            treeViewDocs.Nodes.Add("New", "Новые документы", "Folder_stuffed");
            treeViewDocs.AfterSelect += new TreeViewEventHandler(treeViewDocs_AfterSelect);

            DTG = new DocTipGrow(MAX_TREE_Fall);

            ShowDocument = DocView;

            A_Struct = new Structure(SybAcc,treeViewStructure);
            A_Struct.Show_Empl = true;
            panelStructure.SendToBack();
        }

        void treeViewDocs_AfterSelect(object sender, TreeViewEventArgs e)
        {
            Grow_Folders(e.Node.Name.Trim(), e.Node);
        }

        public void Grow_Folders(string Crone, TreeNode DocNod)
        {
            int Count_fall = 0;
            try
            {
                switch (Crone)
                {
                    case "y":
                    case "Y":
                        year = (int)Convert.ToInt32(DocNod.Name.Substring(3, DocNod.Name.Length - 3));
                        break;
                    case "m":
                    case "M":
                        month = (int)Convert.ToInt32(DocNod.Name.Substring(3, DocNod.Name.Length - 3));
                        year = (int)Convert.ToInt32(DocNod.Parent.Name.Substring(3, DocNod.Name.Length - 3));
                        break;
                }
            }
            catch { }

            Crone = ThePrefix(Crone);

            switch (Crone.ToLower())
            {
                case "cta":
                    Count_fall = CountDocsInFolder(DTG.YearsCountCurrentmoving()) + CountDocsInFolder(DTG.YearsCountCurrentvizing()) + CountDocsInFolder(DTG.YearsCountCurrentnews());
                    break;
                case "eda":
                    Count_fall = CountDocsInFolder(DTG.YearsCountExecutedmoving()) + CountDocsInFolder(DTG.YearsCountExecutedvizing()) + CountDocsInFolder(DTG.YearsCountExecutednews());
                    break;
                case "sda":
                    Count_fall = CountDocsInFolder(DTG.YearsCountSendedmoving()) + CountDocsInFolder(DTG.YearsCountSendedvizing()) + CountDocsInFolder(DTG.YearsCountSendednews());
                    break;
                case "nwa":
                    Count_fall = CountDocsInFolder(DTG.YearsCountNew());
                   break;
                case "cty":
                   Count_fall = CountDocsInFolder(DTG.MoonthCountCurrentmoving(year)) + CountDocsInFolder(DTG.MoonthCountCurrentvizing(year)) + CountDocsInFolder(DTG.MoonthCountCurrentnews(year));
                   break;
                case "edy":
                   Count_fall = CountDocsInFolder(DTG.MoonthCountExecutedmoving(year)) + CountDocsInFolder(DTG.MoonthCountExecutedvizing(year)) + CountDocsInFolder(DTG.MoonthCountExecutednews(year));
                   break;
                case "sdy":
                   Count_fall = CountDocsInFolder(DTG.MoonthCountSendedmoving(year)) + CountDocsInFolder(DTG.MoonthCountSendedvizing(year)) + CountDocsInFolder(DTG.MoonthCountSendednews(year));
                   break;
                case "nwy":
                   Count_fall = CountDocsInFolder(DTG.MoonthCountNew(year));
                    break;
                case "ctm":
                    Count_fall = CountDocsInFolder(DTG.DayCountCurrentmoving(year, month)) + CountDocsInFolder(DTG.DayCountCurrentvizing(year, month)) + CountDocsInFolder(DTG.DayCountCurrentnews(year, month));
                    break;
                case "edm":
                    Count_fall = CountDocsInFolder(DTG.DayCountExecutedmoving(year, month)) + CountDocsInFolder(DTG.DayCountExecutedvizing(year, month)) + CountDocsInFolder(DTG.DayCountExecutednews(year, month));
                    break;
                case "sdm":
                    Count_fall = CountDocsInFolder(DTG.DayCountSendedmoving(year, month)) + CountDocsInFolder(DTG.DayCountSendedvizing(year, month)) + CountDocsInFolder(DTG.DayCountSendednews(year, month));
                    break;
                case "nwm":
                    Count_fall = CountDocsInFolder(DTG.DayCountNew(year, month));
                    break;
            }


            if (Count_fall > MAX_TREE_Fall)
            {

                switch (Crone.ToLower())
                {
                    case "cta":
                        DocsYearList(DTG.YearsCurrentmovingID(), Crone.Substring(0,2)+"y", DocNod,0);
                        DocsYearList(DTG.YearsCurrentvizingID(), Crone.Substring(0,2)+"y", DocNod,0);
                        DocsYearList(DTG.YearsCurrentnewsID(), Crone.Substring(0,2)+"y", DocNod,0);
                        break;
                    case "eda":
                        DocsYearList(DTG.YearsExecutedmovingID(), Crone.Substring(0,2)+"y", DocNod,0) ;
                        DocsYearList(DTG.YearsExecutedvizingID(), Crone.Substring(0, 2) + "y", DocNod, 0);
                        DocsYearList(DTG.YearsExecutednewsID(), Crone.Substring(0, 2) + "y", DocNod, 0);
                        break;
                    case "sda":
                        DocsYearList(DTG.YearsSendedmovingID(), Crone.Substring(0,2)+"y", DocNod,0) ;
                        DocsYearList(DTG.YearsSendedvizingID(), Crone.Substring(0, 2) + "y", DocNod, 0);
                        DocsYearList(DTG.YearsSendednewsID(), Crone.Substring(0, 2) + "y", DocNod, 0);
                        break;
                    case "nwa":
                        DocsYearList(DTG.YearsNewID(), Crone.Substring(0,2)+"y", DocNod,0);
                        break;
                    case "cty":
                        DocsMonthList(DTG.MoonthCurrentmovingID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        DocsMonthList(DTG.MoonthCurrentvizingID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        DocsMonthList(DTG.MoonthCurrentnewsID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        break;
                    case "edy":
                        DocsMonthList(DTG.MoonthExecutedmovingID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        DocsMonthList(DTG.MoonthExecutedvizingID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        DocsMonthList(DTG.MoonthExecutednewsID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        break;
                    case "sdy":
                        DocsMonthList(DTG.MoonthSendedmovingID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        DocsMonthList(DTG.MoonthSendedvizingID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        DocsMonthList(DTG.MoonthSendednewsID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        break;
                    case "nwy":
                        DocsMonthList(DTG.MoonthNewID(year), Crone.Substring(0, 2) + "m", DocNod, 0);
                        break;
                    case "ctm":
                        DocsDayList(DTG.DayCountCurrentmoving(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        DocsDayList(DTG.DayCountCurrentvizing(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        DocsDayList(DTG.DayCountCurrentnews(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        break;
                    case "edm":
                        DocsDayList(DTG.DayCountExecutedmoving(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        DocsDayList(DTG.DayCountExecutedvizing(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        DocsDayList(DTG.DayCountExecutednews(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        break;
                    case "sdm":
                        DocsDayList(DTG.DayCountSendedmoving(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        DocsDayList(DTG.DayCountSendedvizing(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        DocsDayList(DTG.DayCountSendednews(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        break;
                    case "nwm":
                        DocsDayList(DTG.DayCountNew(year, month), Crone.Substring(0, 2) + "d", DocNod, 0);
                        break;
                    case "nwd":
                    case "ctd":
                    case "edd":
                    case "sdd":
                        Fill_Tree(DocNod);
                        break;
                    case "nwf":
                    case "ctf":
                    case "edf":
                    case "sdf":
                        ShowDocument.Doc_ID = Convert.ToInt32(DocNod.Name.Substring(3));
                        break;
                }
            }
            else if (Crone.Substring(2,1).CompareTo("f")==0) ShowDocument.Doc_ID = Convert.ToInt32(DocNod.Name.Substring(3));
            else Fill_Tree(DocNod);
        }

        private string ThePrefix(string Crone)
        {
            switch (Crone)
            {
                case "Current":
                    Crone = "cta";
                    break;
                case "Executed":
                    Crone = "eda";
                    break;
                case "Sended":
                    Crone = "sda";
                    break;
                case "New":
                    Crone = "nwa";
                    break;
                default:
                    Crone = Crone.Substring(0, 3);
                    break;
            }
            return Crone;
        }

        private void DocsYearList(string sql, string cron, TreeNode DocNod, int Docimage)
        {
            if (SybAcc.Set_table("TCSDocs5", sql, null))
            {
                try
                {
                    for (int i = 0; i < SybAcc.Rows_count; i++)
                        if (DocNod.Nodes.Find( cron + (string)Convert.ToString(SybAcc.Find_Field("Year")),false)==null)
                        {
                        try
                        {
                            SybAcc.Get_row(i);
                            DocNod = DocNod.Nodes.Add(Convert.ToString(SybAcc.get_current_Field()) + " год");
                            DocNod.Name = cron + (string)Convert.ToString(SybAcc.Find_Field("Year"));
                            DocNod.ImageIndex = Docimage;
                        }
                        catch { }
                        }
                }
                catch { }
                SybAcc.ReturnTable();
            }
        }

        private void DocsMonthList(string sql, string cron, TreeNode DocNod, int Docimage)
        {
            if (SybAcc.Set_table("TCSDocs7", sql, null))
            {
                try
                {
                    for (int i = 0; i < SybAcc.Rows_count; i++)
                        if (DocNod.Nodes.Find(cron + (string)Convert.ToString(SybAcc.Find_Field("month")), false) == null)
                        {
                            try
                            {
                                SybAcc.Get_row(i);
                                DocNod.Nodes.Add(cron + (string)Convert.ToString(SybAcc.Find_Field("month")), Get_month((int)SybAcc.get_current_Field()), Docimage);
                            }
                            catch { }
                        }
                }
                catch { }
            }
            SybAcc.ReturnTable();
        }

        private void DocsDayList(string sql, string cron, TreeNode DocNod, int Docimage)
        {
            if (SybAcc.Set_table("TCSDocs7", sql, null))
            {
                try
                {
                    for (int i = 0; i < SybAcc.Rows_count; i++)
                        if (DocNod.Nodes.Find(cron + (string)Convert.ToString(SybAcc.Find_Field("day")), false) == null)
                        {
                            try
                            {
                                SybAcc.Get_row(i);
                                DocNod.Nodes.Add(cron + (string)Convert.ToString(SybAcc.Find_Field("day")), (string)Convert.ToString(SybAcc.get_current_Field()), Docimage);
                            }
                            catch { }
                        }
                }
                catch { }
                SybAcc.ReturnTable();
            }
        }
       

        private int CountDocsInFolder(string sql)
        {
            int Count_fall = 0;
            if (SybAcc.Set_table("TCSDocs3", sql, null))
            {
                try
                {
                    Count_fall = (int)SybAcc.Find_Field("cnt");
                }
                catch { Count_fall = 0; }
                SybAcc.ReturnTable();
            }
            return Count_fall;
        }

        public void Fill_Tree( TreeNode DocNod)
        {
            int day = 0; int month = 0; int year = 0;
            string cron = "";

            switch (DocNod.Name)
            {
                case "Current":
                    cron = "cta";
                    break;
                case "Executed":
                    cron = "eda";
                    break;
                case "Sended":
                    cron = "sda";
                    break;
                case "New":
                    cron = "nwa";
                    break;
                default:
                    cron = DocNod.Name.Substring(0, 3);
                    break;
            }
            try
            {
                switch (cron.Substring(2,1).ToUpper())
                {
                    case "Y":
                        year = (int)Convert.ToInt32(DocNod.Name.Substring(3, DocNod.Name.Length - 3));
                        break;
                    case "M":
                        month = (int)Convert.ToInt32(DocNod.Name.Substring(3, DocNod.Name.Length - 3));
                        year = (int)Convert.ToInt32(DocNod.Parent.Name.Substring(3, DocNod.Parent.Name.Length - 3));
                        break;
                    case "D":
                        day = (int)Convert.ToInt32(DocNod.Name.Substring(3, DocNod.Name.Length - 3));
                        month = (int)Convert.ToInt32(DocNod.Parent.Name.Substring(3, DocNod.Parent.Name.Length - 3));
                        year = (int)Convert.ToInt32(DocNod.Parent.Parent.Name.Substring(3, DocNod.Parent.Parent.Name.Length - 3));
                        break;
                }
            }
            catch { }

            cron=cron.Substring(0, 2);
            Documents_Catalog(DTG.Documents_Catalog(day, month, year, cron), DocNod, cron);
        }

        public string Get_month(int month)
        {
            string mon = "";
            switch (month)
            {
                case 1:
                    mon = "январь";
                    break;
                case 2:
                    mon = "февраль";
                    break;
                case 3:
                    mon = "март";
                    break;
                case 4:
                    mon = "апрель";
                    break;
                case 5:
                    mon = "май";
                    break;
                case 6:
                    mon = "июнь";
                    break;
                case 7:
                    mon = "июль";
                    break;
                case 8:
                    mon = "август";
                    break;
                case 9:
                    mon = "сентябрь";
                    break;
                case 10:
                    mon = "октябрь";
                    break;
                case 11:
                    mon = "ноябрь";
                    break;
                case 12:
                    mon = "декарь";
                    break;
            }
            return mon;
        }

        public void Documents_Catalog(string sql, TreeNode DocNod,string cron)
        {
            int kod = 0;
            string Resultset = "";
            TreeNode Node = null;
            if (SybAcc.Set_table("TCSDocsList", sql, null))
                {
                    try
                    {
                        if (FuelBar != null)
                        {
                            FuelBar.Minimum = 0;
                            if (SybAcc.Rows_count > 1)
                                FuelBar.Maximum = SybAcc.Rows_count - 1;
                            else
                                FuelBar.Maximum = 1;
                            FuelBar.Visible = true;
                        }
                        for (int i = 0; i < SybAcc.Rows_count; i++)
                        {
                            SybAcc.Get_row(i);
                            if (FuelBar != null) FuelBar.Value = i;
                            try
                            {
                                string findKod = "???";
                                try
                                {
                                    findKod = (string)SybAcc.Find_Field("find_cod");
                                    kod = (int)SybAcc.Find_Field("kod");
                                }
                                catch { findKod = "???"; }

                                string keynod=cron + "f" + kod.ToString();

                                if (DocNod != null)
                                {
                                    if (!DocNod.Nodes.ContainsKey(keynod))
                                        Node = DocNod.Nodes.Add(keynod, findKod, 4);
                                }
                                else
                                    if (!DocNod.Nodes.ContainsKey(keynod))
                                        treeViewDocs.Nodes.Add(keynod, findKod, 4);
                            }
                            catch (Exception ex)
                            {
                                SybAcc.EBBLP.AddError(ex.Message, "Select Document - 1", ex.StackTrace);
                                Resultset = ex.Message;
                            }
                        }
                        if (FuelBar != null) FuelBar.Visible = false;
                    }
                    catch (Exception ex)
                    {
                        SybAcc.EBBLP.AddError(ex.Message, "Select Document - 2", ex.StackTrace);
                        Resultset = ex.Message;
                    }
                    SybAcc.ReturnTable();
                }
        }

        private void tsbNewDoc_Click(object sender, EventArgs e)
        {
            ShowDocument.Doc_ID = 0;
            ShowDocument.New_document = true;
            ShowDocument.Edit_document = true;
            UCNewDocument NewDocExecution = new UCNewDocument(SybAcc, ShowDocument, 0,false);
            this.Controls.Add(NewDocExecution);
            NewDocExecution.Dock = DockStyle.Fill;
            NewDocExecution.Visible = true;
            NewDocExecution.BringToFront();
        }

        private void tsbAnswer_Click(object sender, EventArgs e)
        {
            if (ShowDocument.Doc_ID > 0)
            {
                Document_Viewer Newdoc = new Document_Viewer(SybAcc, null);
                Newdoc.Doc_ID = 0;
                Newdoc.New_document = true;
                Newdoc.Edit_document = true;
                ShowDocument.Controls.Add(Newdoc);
                Newdoc.Dock = DockStyle.Fill;
                UCNewDocument NewDocExecution = new UCNewDocument(SybAcc, Newdoc, ShowDocument.Doc_ID,true);
                this.Controls.Add(NewDocExecution);
                NewDocExecution.Dock = DockStyle.Fill;
                NewDocExecution.Visible = true;
                NewDocExecution.BringToFront();
            }
            else MessageBox.Show("Не выбран документ");
        }

        enum ToDo {nothing, executing, vizing, news}
        ToDo whatToDo = ToDo.nothing;

        private void tsbExecute_Click(object sender, EventArgs e)
        {
            whatToDo = ToDo.executing;
            panelStructure.BringToFront();
        }

        private void tbsVizing_Click(object sender, EventArgs e)
        {
            whatToDo = ToDo.vizing;
            panelStructure.BringToFront();
        }

        private void tsbNews_Click(object sender, EventArgs e)
        {
            whatToDo = ToDo.news;
            panelStructure.BringToFront();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            panelStructure.SendToBack();
            switch (whatToDo)
            {
                case ToDo.news:
                    //A_Struct.push_new(,(long)ShowDocument.Doc_ID);
                    break;
                case ToDo.executing:
                    break ;
                case ToDo.vizing:
                    break;
            }
            
        }
    }
}
