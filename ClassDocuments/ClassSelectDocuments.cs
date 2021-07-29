using System;
using System.Collections.Generic;
using System.Text;
using AMAS_DBI;
using System.Windows.Forms;
using AMAS_Query;
using AMASDocuments;
using CommonValues;

namespace AMASDocuments.SelectDocuments
{

    public class ClassSelectDocuments
    {
        public const int DocCatYear = 14;
        public const int DocCatMonth = 15;
        public const int DocCatDay = 16;

        private Class_syb_acc SybAcc;
        public System.Windows.Forms.TreeView treeDocsView;
        public System.Windows.Forms.TreeNode DocNod;
        private ClassDocsItem.refer_NodeToDoc LastReferNodeToDoc = null;
        private TreeGrow TreG;

        public System.Windows.Forms.ToolStripProgressBar FuelBar;
        private AMASDocuments.ClassDocsItem selecteDocument = null;
        public AMASDocuments.ClassDocsItem Selected_document { get { return selecteDocument; } }
        public CommonValues.FindProperty FProp=null;

        public ClassSelectDocuments(Class_syb_acc acc, System.Windows.Forms.TreeView treeView, TreeGrow TG)
        {
            SybAcc = acc;
            treeDocsView = treeView;
            treeDocsView.Nodes.Clear();
            TreG = TG;
            FuelBar = null;
            FProp = null;
            CSD();
        }

        public ClassSelectDocuments(Class_syb_acc acc, System.Windows.Forms.TreeView treeView, TreeGrow TG, System.Windows.Forms.ToolStripProgressBar PrBr, CommonValues.FindProperty FP)
        {
            SybAcc = acc;
            treeDocsView = treeView;
            treeDocsView.Nodes.Clear();
            TreG = TG;
            FuelBar = PrBr;
            FProp = FP;
            CSD();
        }

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem del;
        
        private void CSD()
        {
            treeDocsView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeDocsView_AfterSelect);
            treeDocsView.NodeMouseClick+=new TreeNodeMouseClickEventHandler(treeDocsView_NodeMouseClick);
            FindMode mode = FindMode.Nothing;
            switch (AMAS_Query.Class_AMAS_Query.DocIndex)
            {
                case DocEnumeration.NewsDocs.Value:
                case DocEnumeration.WellcomeDocs.Value:
                    mode = FindMode.Wellcome;
                    break;
                case DocEnumeration.IndoorDosc.Value:
                case DocEnumeration.VizingDocs.Value:
                    mode = FindMode.Indoor;
                    break;
                case DocEnumeration.OutDocs.Value:
                    mode = FindMode.Outdoor;
                    break;
                case DocEnumeration.ArchiveDocs.Value:
                    mode = FindMode.Arcive;
                    break;
                case DocEnumeration.OwnDocs.Value:
                    if (treeDocsView != null)
                    {
                        try
                        {
                            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
                            this.del = new System.Windows.Forms.ToolStripMenuItem();
                            this.contextMenuStrip1.SuspendLayout();
                            // 
                            // contextMenuStrip1
                            // 
                            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.del});
                            this.contextMenuStrip1.Name = "contextMenuStrip1";
                            this.contextMenuStrip1.Size = new System.Drawing.Size(224, 92);
                            // 
                            // del
                            // 
                            this.del.Name = "del";
                            this.del.Size = new System.Drawing.Size(223, 22);
                            this.del.Text = "Удалить документ";
                            this.contextMenuStrip1.ResumeLayout(false);
                            treeDocsView.ContextMenuStrip = this.contextMenuStrip1;
                            contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
                        }
                        catch { }
                    }
                    mode = FindMode.Nothing;
                    break;
                default:
                    mode = FindMode.Nothing;
                    break;
            }
            if (AMAS_Query.WellWork.Work)
                AMAS_Query.Class_AMAS_Query.Get_List_Docs(Find_query(mode));
            else AMAS_Query.Class_AMAS_Query.Get_List_WellDocs();

            Grow_tree("a");
        }

        void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "del":
                    try
                    {

                        selecteDocument.Delete();
                    }
                    catch { }
                    break;
            }
        }

        // Режим поиска
        private enum FindMode
        {
            Wellcome=1,     // Входящие
            Indoor,         // Внутренние
            Outdoor,        // Исходящие
            Arcive,         // Архив
            Nothing         //Пусто
        }

        private string Find_query(FindMode mode)
        {
            string sql_where = "";
            if (FProp != null)
            {
                try
                {
                    switch (mode)
                    {
                        case FindMode.Wellcome:
                            if (FProp.field_org.Trim().Length > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and enterprice in (select id from dbo.org_jrd_juridic where full_name like '" + FProp.field_org.Trim() + "')";
                                else sql_where += " enterprice in (select id from dbo.org_jrd_juridic where full_name like '" + FProp.field_org.Trim() + "')";

                            if (FProp.field_autor.Trim().Length > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and leader in (select cod from dbo.emp_leaders where fio like '" + FProp.field_autor.Trim() + "')";
                                else sql_where += " leader in (select cod from dbo.emp_leaders where fio like '" + FProp.field_autor.Trim() + "')";

                            if (FProp.Surname.Trim().Length > 0)
                                if (FProp.FirstName.Trim().Length > 0)
                                    if (FProp.LastName.Trim().Length > 0)
                                        if (sql_where.Length > 0)
                                            sql_where += " and autor in (select id from dbo.rkk_autor where name like '" + FProp.FirstName.Trim() + "' and father like '" + FProp.LastName.Trim() + "' and family like '" + FProp.Surname.Trim() + "')";
                                        else sql_where += " autor in (select id from dbo.rkk_autor where name like '" + FProp.FirstName.Trim() + "' and father like '" + FProp.LastName.Trim() + "' and family like '" + FProp.Surname.Trim() + "')";
                                    else if (sql_where.Length > 0)
                                        sql_where += " and autor in (select id from dbo.rkk_autor where name like '" + FProp.FirstName.Trim() + "' and family like '" + FProp.Surname.Trim() + "')";
                                    else sql_where += " autor in (select id from dbo.rkk_autor where name like '" + FProp.FirstName.Trim() + "' and family like '" + FProp.Surname.Trim() + "')";
                                else if (sql_where.Length > 0)
                                    sql_where += " and autor in (select id from dbo.rkk_autor where family like '" + FProp.Surname.Trim() + "')";
                                else sql_where += " autor in (select id from dbo.rkk_autor where family like '" + FProp.Surname.Trim() + "')";

                            if (FProp.OUT_cod.Trim().Length > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and outcoming like '" + FProp.OUT_cod + "')";
                                else sql_where += " outcoming like '" + FProp.OUT_cod + "')";

                            if (FProp.OUT_date.Date > DateTime.MinValue)
                                if (sql_where.Length > 0)
                                    sql_where += " and date_out = cast('" + FProp.OUT_date.ToShortDateString() + " as datetime')";
                                else sql_where += " date_out = cast('" + FProp.OUT_date.ToShortDateString() + " as datetime')";
                            break;

                        case FindMode.Indoor:
                            if (FProp.Executor > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and employee= " + FProp.Executor.ToString();
                                else sql_where += " employee= " + FProp.Executor.ToString();
                            break;

                        case FindMode.Outdoor:
                            if (FProp.Executor > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and employee= " + FProp.Executor.ToString();
                                else sql_where += " employee= " + FProp.Executor.ToString();
                            break;

                        case FindMode.Arcive:
                            if (FProp.field_org.Trim().Length > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and enterprice in (select id from dbo.org_jrd_juridic where full_name like '" + FProp.field_org.Trim() + "')";
                                else sql_where += " enterprice in (select id from dbo.org_jrd_juridic where full_name like '" + FProp.field_org.Trim() + "')";

                            if (FProp.field_autor.Trim().Length > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and leader in (select cod from dbo.emp_leaders where fio like '" + FProp.field_autor.Trim() + "')";
                                else sql_where += " leader in (select cod from dbo.emp_leaders where fio like '" + FProp.field_autor.Trim() + "')";

                            if (FProp.LastName.Trim().Length > 0)
                                if (FProp.FirstName.Trim().Length > 0)
                                    if (FProp.Surname.Trim().Length > 0)
                                        if (sql_where.Length > 0)
                                            sql_where += " and autor in (select id from dbo.rkk_autor where name like '" + FProp.FirstName.Trim() + "' and father like '" + FProp.Surname.Trim() + "' and family like '" + FProp.LastName.Trim() + "')";
                                        else sql_where += " autor in (select id from dbo.rkk_autor where name like '" + FProp.FirstName.Trim() + "' and father like '" + FProp.Surname.Trim() + "' and family like '" + FProp.LastName.Trim() + "')";
                                    else if (sql_where.Length > 0)
                                        sql_where += " and autor in (select id from dbo.rkk_autor where name like '" + FProp.FirstName.Trim() + "' and family like '" + FProp.LastName.Trim() + "')";
                                    else sql_where += " autor in (select id from dbo.rkk_autor where name like '" + FProp.FirstName.Trim() + "' and family like '" + FProp.LastName.Trim() + "')";
                                else if (sql_where.Length > 0)
                                    sql_where += " and autor in (select id from dbo.rkk_autor where family like '" + FProp.LastName.Trim() + "')";
                                else sql_where += " autor in (select id from dbo.rkk_autor where family like '" + FProp.LastName.Trim() + "')";

                            if (FProp.OUT_cod.Trim().Length > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and outcoming like '" + FProp.OUT_cod + "'";
                                else sql_where += " outcoming like '" + FProp.OUT_cod + "'";

                            if (FProp.OUT_date.Date > DateTime.MinValue)
                                if (sql_where.Length > 0)
                                    sql_where += " and date_out = cast('" + FProp.OUT_date.ToShortDateString() + " as datetime')";
                                else sql_where += " annot date_out = cast('" + FProp.OUT_date.ToShortDateString() + " as datetime')";

                            if (FProp.Executor > 0)
                                if (sql_where.Length > 0)
                                    sql_where += " and employee= " + FProp.Executor.ToString();
                                else sql_where += " employee= " + FProp.Executor.ToString();
                            break;

                        case FindMode.Nothing:
                            sql_where = "";
                            break;
                    }

                    if (FProp.find_cod.Trim().Length > 0)
                        if (sql_where.Length > 0)
                            sql_where += " and find_cod like '" + FProp.find_cod + "' ";
                        else sql_where += " find_cod like '" + FProp.find_cod + "' ";

                    if (FProp.Text_ANNOT.Trim().Length > 0)
                        if (sql_where.Length > 0)
                            sql_where += " and annot like '" + FProp.Text_ANNOT + "' ";
                        else sql_where += " annot like '" + FProp.Text_ANNOT + "' ";

                    if (FProp.Text_Note.Trim().Length > 0)
                        if (sql_where.Length > 0)
                            sql_where += " and kod in ( select document from dbo. RKK_denote_of_document where denote like '" + FProp.Text_Note + "')";
                        else sql_where += " kod in ( select document from dbo. RKK_denote_of_document where denote like '" + FProp.Text_Note + "')";

                    if (FProp.Combo_kind > 0)
                        if (sql_where.Length > 0)
                            sql_where += " and kind= " + FProp.Combo_kind.ToString();
                        else sql_where += " kind= " + FProp.Combo_kind.ToString();

                    if (FProp.Combo_tema > 0)
                        if (sql_where.Length > 0)
                            sql_where += " and tema= " + FProp.Combo_tema.ToString();
                        else sql_where += " tema= " + FProp.Combo_tema.ToString();
                }
                catch { }
            }
            return sql_where;
        }

        AMASDocuments.ClassDocsItem[] docs;

        public void Documents_Catalog(int day , int month , int year )
        {
            int kod = 0;
            string Resultset = "";
            TreeNode Node = null;
                if (SybAcc.Set_table ("TCSDocs1", AMAS_Query.Class_AMAS_Query.Fill_Tree(day, month, year), AMAS_Query.Class_AMAS_Query.PrepareDate()))
                {
                    try
                    {
                        docs = new AMASDocuments.ClassDocsItem[SybAcc.Rows_count];
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
                                string findKod="???";
                                try
                                {
                                    findKod=(string)SybAcc.Find_Field("find_cod");
                                }
                                catch {findKod="???";}
                                if (DocNod == null)
                                {
                                    kod = (int)SybAcc.Find_Field("kod");
                                    Node = treeDocsView.Nodes.Add("d" + kod.ToString(), findKod);
                                    docs[i] = new AMASDocuments.ClassDocsItem(SybAcc);
                                    docs[i].Locate_Node(Node);
                                }
                                else
                                    docs[i] = new AMASDocuments.ClassDocsItem(SybAcc, DocNod.Nodes.Add(findKod), LastReferNodeToDoc);
                                LastReferNodeToDoc = docs[i].NodeAtDOcument;
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

        public delegate void PickedHandler(int DocId,TreeNode nod);
        public event PickedHandler NodePicked;

        private void treeDocsView_NodeMouseClick(Object sender, TreeNodeMouseClickEventArgs e)
        {
            try
            {
                if (NodTreeSelected == null)
                    TreeNodeSelect(e.Node);
                else if (e.Node.Name.CompareTo(NodTreeSelected.Name) != 0)
                    TreeNodeSelect(e.Node);
            }
            catch { }
            NodTreeSelected = e.Node;
        }

        private void treeDocsView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            try
            {
                if (NodTreeSelected == null)
                    TreeNodeSelect(e.Node);
                else if (e.Node.Name.CompareTo(NodTreeSelected.Name) != 0)
                    TreeNodeSelect(e.Node);
            }
            catch { }
            NodTreeSelected = e.Node;
        }

        private TreeNode NodTreeSelected = null;

        public void CurrentNodeRefresh()
        {
            if (NodTreeSelected != null) TreeNodeSelect(NodTreeSelected);
        }

        private void TreeNodeSelect(TreeNode Nod)
        {
            selecteDocument = null;
            DocNod = Nod;
            int DocId = 0;
            try
            {
                if (DocNod.Name.Substring(0, 1).CompareTo("#") != 0)
                    switch (DocNod.Name.Substring(0, 1).ToLower())
                    {
                        case "d":
                            foreach (AMASDocuments.ClassDocsItem Doc in docs)
                                if (Doc != null)
                                    if (DocNod.Name.CompareTo(Doc.Doc_Node.Name) == 0)
                                    {
                                        selecteDocument = Doc;
                                        LastReferNodeToDoc = selecteDocument.NodeAtDOcument;
                                        DocId = (int)Doc.Doc_id;
                                        break;
                                    }
                            break;
                        case "m":
                            foreach (AMASDocuments.ClassDocsItem Doc in docs)
                            {
                                try
                                {
                                    foreach (ClassDocsItem.DocTask Task in Doc.Tasklist)
                                        if (DocNod.Name.CompareTo(Task.TASK_DocNode.Name) == 0)
                                        {
                                            selecteDocument = Doc;
                                            LastReferNodeToDoc = selecteDocument.NodeAtDOcument;
                                            DocId = (int)Task.TASK_DocId;
                                            break;
                                        }
                                }
                                catch { }
                                if (selecteDocument == Doc) break;
                            }
                            break;
                        case "v":
                            foreach (AMASDocuments.ClassDocsItem Doc in docs)
                            {
                                try
                                {
                                    foreach (ClassDocsItem.DocViza Viza in Doc.Vizalist)
                                        if (DocNod.Name.CompareTo(Viza.Viza_DocNode.Name) == 0)
                                        {
                                            selecteDocument = Doc;
                                            LastReferNodeToDoc = selecteDocument.NodeAtDOcument;
                                            DocId = (int)Viza.Viza_DocId;
                                            break;
                                        }
                                }
                                catch { }
                                if (selecteDocument == Doc) break;
                            }
                            break;
                        case "n":
                            break;
                    }

                if (DocNod.GetNodeCount(true) == 0)
                    DocId = Tree_select(DocNod);
                else
                    if (DocNod.Parent != null)
                        if (DocNod.Parent.Name.Substring(0, 1).CompareTo("#") != 0)
                            DocId = Tree_select(DocNod);
                if (DocId > 0) NodePicked(DocId, DocNod);
            }
            catch (Exception ex)
            {
                SybAcc.EBBLP.AddError(ex.Message, "Select Document - 3", ex.StackTrace);
            }
        }

        void timeShift_Tick(object sender, EventArgs e)
        {
            
        }


        private int Tree_select(TreeNode Nod)
        {
            int DocId = 0;
            string Resultset="";
            string sel = Nod.Name.Substring(0, 1);
            AMASDocuments.ClassDocsItem.refer_NodeToDoc ND=null;
            try { ND = LastReferNodeToDoc.Go_First; }
            catch { ND = null; }
            try
            {
                switch (sel)
                {
                    case "d":
                        while (ND != null)
                        {
                            if (ND.GetNode.Name.CompareTo(Nod.Name) == 0)
                            {
                                selecteDocument = ND.GetDoc;
                                Resultset = ND.GetDoc.DOC_annot;
                                Resultset += ND.GetDoc.ResultString;
                                if (ND.GetDoc.Tasklist != null) foreach (AMASDocuments.ClassDocsItem.DocTask task in ND.GetDoc.Tasklist)
                                    {
                                        try
                                        {
                                            if (task != null)
                                            {
                                                Resultset += " to: " + Convert.ToString(task.Task_finish);
                                                Resultset += " res: " + Convert.ToString(task.Task_resolution);
                                            }
                                        }
                                        catch { }
                                        ND.GetDoc.ShowTasks();
                                        break;
                                    }
                                if (ND.GetDoc.Vizalist != null) foreach (AMASDocuments.ClassDocsItem.DocViza viza in ND.GetDoc.Vizalist)
                                    {
                                        try
                                        {
                                            if (viza != null)
                                            {
                                                Resultset += " to: " + Convert.ToString(viza.Viza_finish);
                                                Resultset += " res: " + Convert.ToString(viza.Viza_vizaSheet);
                                            }
                                        }
                                        catch { }
                                        ND.GetDoc.ShowVizy();
                                        break;
                                    }
                                if (ND.GetDoc.Newlist != null) foreach (AMASDocuments.ClassDocsItem.DocNew news in ND.GetDoc.Newlist)
                                    {
                                        ND.GetDoc.ShowNews();
                                        break;
                                    }
                                //ND.GetDoc.DocContent.Get_MultiPages(DocShowWeb, DocShowText, DocShowPicter);
                                DocId = ND.GetDoc.Doc_id;
                                break;
                            }
                            ND = ND.Get_Next;
                        }
                        
                        break;
                    case "m":
                        foreach (AMASDocuments.ClassDocsItem Doc in docs)
                        {
                            ND = Doc.NodeAtDOcument;
                            while (ND != null)
                            {
                                try
                                {
                                    foreach (AMASDocuments.ClassDocsItem.DocTask task in ND.GetDoc.Tasklist)
                                    {
                                        if (task.TASK_DocNode.Name.CompareTo(Nod.Name)==0)
                                        {
                                            Resultset = task.Task_resolution;
                                            DocId = ND.GetDoc.Doc_id;
                                        }
                                    }
                                }
                                catch { }
                                ND = ND.Get_Next;
                            }
                        }
                        break;
                    case "v":
                        foreach (AMASDocuments.ClassDocsItem Doc in docs)
                        {
                            ND = Doc.NodeAtDOcument;
                            while (ND != null)
                            {
                                try
                                {
                                    foreach (ClassDocsItem.DocViza viza in ND.GetDoc.Vizalist)
                                    {
                                        if (viza.Viza_DocNode.Name.CompareTo( Nod.Name)==0)
                                        {
                                            Resultset = (string)Convert.ToString(viza.Viza_vizaSheet);
                                            DocId = ND.GetDoc.Doc_id;
                                        }
                                    }
                                }
                                catch { }
                                ND = ND.Get_Next;
                            }
                        }
                        break;
                    case "x":
                        foreach (AMASDocuments.ClassDocsItem Doc in docs)
                        {
                            ND = Doc.NodeAtDOcument;
                            while (ND != null)
                            {
                                try
                                {
                                    foreach (ClassDocsItem.DocTask task in ND.GetDoc.Tasklist)
                                    {
                                        if (task.TASK_DocNode.Name.CompareTo( Nod.Name)==0)
                                        {
                                            Resultset = (string)Convert.ToString(task.TASK_IssueDocument.Doc_RKK);
                                            DocId = ND.GetDoc.Doc_id;
                                            if (task.TASK_IssueDocument.Doc_Node == null)
                                            {
                                                task.TASK_IssueDocument.LastReferNodeToDoc = LastReferNodeToDoc;
                                                task.TASK_IssueDocument.Parent_Doc_Node = Nod;
                                            }
                                        }
                                    }
                                }
                                catch { }
                                ND = ND.Get_Next;
                            }
                        }

                        break;
                    case "#":
                        Grow_tree(Nod.Name.Substring(1, 1));
                        break;
                }
            }
            catch { }
            return DocId;
        }

        const int MAX_TREE_Fall = 20;
        private int year = 0; 
        private int month = 0; 

        public void Grow_tree(string Crone)
        {
            int Count_fall=0;
                try
                {
                    switch (Crone)
                    {
                        case "y":
                        case "Y":
                            year = (int)Convert.ToInt32(DocNod.Name.Substring(2, DocNod.Name.Length - 2));
                            break;
                        case "m":
                        case "M":
                            month = (int)Convert.ToInt32(DocNod.Name.Substring(2, DocNod.Name.Length - 2));
                            break;
                    }
                }
                catch { }

                switch (Crone)
                {
                    case "a":
                    case "A":
                            if (SybAcc.Set_table("TCSDocs2", TreG.YearsCount(), AMAS_Query.Class_AMAS_Query.PrepareDate()))
                            {
                                try
                                {
                                    Count_fall = (int)SybAcc.Find_Field("cnt");
                                }
                                catch 
                                { 
                                    Count_fall = 0; 
                                }
                                SybAcc.ReturnTable();
                            }
                        break;
                    case "y":
                    case "Y":
                            if (SybAcc.Set_table("TCSDocs3", TreG.MonthCount(year), AMAS_Query.Class_AMAS_Query.PrepareDate()))
                            {
                                try
                                {
                                    Count_fall = (int)SybAcc.Find_Field("cnt");
                                }
                                catch { }
                                SybAcc.ReturnTable();
                            }
                        break;
                    case "m":
                    case "M":
                            if (SybAcc.Set_table("TCSDocs4", TreG.DaysCount(year, month), AMAS_Query.Class_AMAS_Query.PrepareDate()))
                            {
                                try
                                {
                                    Count_fall = (int)SybAcc.Find_Field("cnt");
                                }
                                catch { }
                                SybAcc.ReturnTable();
                            }
                        break;
                }

                if (Count_fall > MAX_TREE_Fall)
                {

                    switch (Crone)
                    {
                        case "a":
                        case "A":
                                if (SybAcc.Set_table("TCSDocs5", TreG.Years(), AMAS_Query.Class_AMAS_Query.PrepareDate()))
                                {
                            try
                            {
                                    for (int i = 0; i < SybAcc.Rows_count; i++)
                                        try
                                        {
                                            SybAcc.Get_row(i);
                                            DocNod = treeDocsView.Nodes.Add(Convert.ToString(SybAcc.get_current_Field()) + " год");
                                            DocNod.Name = "#Y" + (string)Convert.ToString(SybAcc.Find_Field("Year"));
                                            DocNod.ImageIndex = DocCatYear;
                                        }
                                        catch { }
                            }
                            catch { }
                                    SybAcc.ReturnTable();
                                }
                            break;
                        case "y":
                        case "Y":
                                if (SybAcc.Set_table("TCSDocs6", TreG.Months(year), AMAS_Query.Class_AMAS_Query.PrepareDate()))
                                {
                            try
                            {
                                    for (int i = 0; i < SybAcc.Rows_count; i++)
                                        try
                                        {
                                            SybAcc.Get_row(i);
                                            DocNod.Nodes.Add("#M" + (string)Convert.ToString(SybAcc.Find_Field("month")), Get_month((int)SybAcc.get_current_Field()), DocCatMonth);
                                        }
                                        catch { }
                            }
                            catch { }
                                    SybAcc.ReturnTable();
                                }
                            break;

                        case "m":
                        case "M":
                                if (SybAcc.Set_table("TCSDocs7", TreG.Days(year, month), AMAS_Query.Class_AMAS_Query.PrepareDate()))
                                {
                            try
                            {
                                    for (int i = 0; i < SybAcc.Rows_count; i++)
                                        try
                                        {
                                            SybAcc.Get_row(i);
                                            DocNod.Nodes.Add("#D" + (string)Convert.ToString(SybAcc.Find_Field("day")), (string)Convert.ToString(SybAcc.get_current_Field()), DocCatDay);
                                        }
                                        catch { }
                            }
                            catch { }
                                    SybAcc.ReturnTable();
                                }
                            break;
                        case "d":
                        case "D":
                            Fill_Tree();
                            break;
                    }
                }
                else Fill_Tree();
        }

        public string Get_month(int month ) 
        {
            string mon="";
            switch (month)
            {
            case 1:
                    mon= "январь";
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

        public void Fill_Tree()
        {
            int day = 0; int month = 0; int year = 0;
            selecteDocument = null;
            string s;
            try { s = DocNod.Name.Substring(1, 1); }
            catch { s = "a"; }
            try
            {
            switch (s.ToUpper())
            {
                case "Y":
                    year = (int)Convert.ToInt32(DocNod.Name.Substring(2,DocNod.Name.Length-2));
                    break;
                case "M":
                    month = (int)Convert.ToInt32(DocNod.Name.Substring(2,DocNod.Name.Length-2));
                    year = (int)Convert.ToInt32(DocNod.Parent.Name.Substring(2,DocNod.Parent.Name.Length-2));
                    break;
                case "D":
                    day = (int)Convert.ToInt32(DocNod.Name.Substring(2,DocNod.Name.Length-2));
                    month = (int)Convert.ToInt32(DocNod.Parent.Name.Substring(2,DocNod.Parent.Name.Length-2));
                    year = (int)Convert.ToInt32(DocNod.Parent.Parent.Name.Substring(2,DocNod.Parent.Parent.Name.Length-2));
                    break;
            }
            }catch {}
            Documents_Catalog(day, month, year);
        }
    }
}
