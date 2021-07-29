using System;
using System.Data;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Windows.Forms;
using AMAS_DBI;
using AMAS_Query;
using CommonValues;

namespace AMASDocuments
{
    public class ClassDocsItem
    {
        static int ListCount = 1;
        private int IDent;
        public bool Own_pass = false;
        public const int DocAsTask = 0;
        public const int DocAsViza = 1;
        public const int DocAsNews = 2;
        public const int DocAsTaskViza = 3;
        public const int DocAsVizaNews = 4;
        public const int DocAsTaskNews = 5;
        public const int DocAsTaskVizaNews = 6;
        public const int DocAsTaskExec = 7;
        public const int DocAsVizaExec = 8;
        public const int DocAsNewExec = 9;
        public const int DocAsOpen = 10;
        public const int AskAsTask = 11;
        public const int AskAsViza = 12;
        public const int AskAsNews = 13;

        private string RKK;
        private int DocID;
        private string annotation;
        private int kind;
        //private int tema=0;
        private DateTime created;
        private int typist;
        private int ORG=0;
        private int AUTOR=0;
        private int MAN=0;
        public ClassDocsItem.refer_NodeToDoc LastReferNodeToDoc = null;

        private int parent_viza = 0;
        private int parent_moving = 0;
        private int parent_new = 0;

        public int From_vizing { get { return parent_viza; } }
        public int From_moving { get { return parent_moving; } }
        public int From_newing { get { return parent_new; } }
        public bool Viewed = false;
        public bool Newed=false;
        public bool TopList = false;

        public bool True_Moving
        {
            get
            {
                bool ret=false;
                if (Doc_id > 0)
                {
                    int master_doc = 0;
                    int parent_doc = 0;
                    string sql = "";
                    // поручение исполнителю пересылается далее
                    try
                    {
                        sql = "select * from dbo.RKK_flow_document where kod=" + Doc_id.ToString() + " and typist=dbo.user_ident()";
                        if (SybAcc.Set_table("ClassDocumentTbl111", sql, null))
                        {
                            if (SybAcc.Rows_count > 0) 
                                ret = true;
                            SybAcc.ReturnTable();
                        }
                        if (!ret)
                        sql = "select * from dbo.RKK_flow_document where kod=" + Doc_id.ToString() ;
                        if (SybAcc.Set_table("ClassDocumentTbl111", sql, null))
                        {
                            if (SybAcc.Rows_count > 0)
                            {
                                master_doc = (int)SybAcc.Find_Field("master_document");
                                parent_doc = (int)SybAcc.Find_Field("parent_document");
                            }
                            SybAcc.ReturnTable();
                        }
                    }
                    catch (Exception ex)
                    {
                        SybAcc.AddError(ex.Message, ex.StackTrace, "ClassDocumentTbl111"+sql);
                    }
                    // рассылка собственного письма либо ответа на поручение
                    if (!ret)
                    try
                    {
                        sql = "select * from dbo.RKK_moving where (document in (" + Doc_id.ToString() +","+master_doc.ToString()+","+ parent_doc.ToString()+ ") or exe_doc=" + Doc_id.ToString() + ") and for_ in ( select cod from dbo.Emp_dep_degrees where employee=dbo.user_ident())";
                        if (SybAcc.Set_table("ClassDocumentTbl112", sql, null))
                        {
                            if (SybAcc.Rows_count > 0) ret = true;
                            SybAcc.ReturnTable();
                        }
                    }
                    catch (Exception ex) 
                    {
                        SybAcc.AddError(ex.Message, ex.StackTrace, "ClassDocumentTbl111"+sql);
                    }
                }
                return ret;
            }
        }

        private System.Windows.Forms.TreeNode Node;
        private System.Windows.Forms.TreeNode Parent_Node;
        private System.Windows.Forms.ListView TVNListing = null;

        public class refer_NodeToDoc
        {
            private  ClassDocsItem Doc = null;
            private  System.Windows.Forms.TreeNode Node=null;
            private  refer_NodeToDoc next=null;
            private  refer_NodeToDoc prev=null;
            private  refer_NodeToDoc first=null;
            public  refer_NodeToDoc Go_First { get { return first; } }
            public refer_NodeToDoc Get_Next { get { return next; } }
            public refer_NodeToDoc Get_Prev { get { return prev; } }
            public ClassDocsItem GetDoc { get { return Doc; } }
            public System.Windows.Forms.TreeNode GetNode { get { return Node; } }

            public refer_NodeToDoc(ClassDocsItem DC, TreeNode ND, ClassDocsItem.refer_NodeToDoc LastReferNodeToDoc)
            {
                Node = ND;
                ND.Tag = this;
                Doc = DC;
                prev = LastReferNodeToDoc;
                if (LastReferNodeToDoc == null) first = this;
                else  
                {
                    next = LastReferNodeToDoc.next;
                    LastReferNodeToDoc.next = this;
                    first = LastReferNodeToDoc.first;
                }
            }
        }

        public refer_NodeToDoc NodeAtDOcument;
        public System.Windows.Forms.TreeNode Doc_Node { get { return Node; } }
        public System.Windows.Forms.TreeNode Parent_Doc_Node
        {
            get { return Parent_Node; }
            set
            {
                Parent_Node = value;
                if (Parent_Node != null)
                {
                    TreeNode ND = Parent_Node.Nodes.Add(RKK);
                    Locate_Node(ND);
                }
            }
        }

        public void Picked()
        {
            //if (parent_new>0) AMASCommand.Document_Newed(parent_new);

        }

        public bool Delete()
        {
            bool RES = false;
            try
            {
                AMASCommand.Delete_Own_Document(Doc_id);
                try
                {
                    try
                    {
                        foreach (DocTask Task in Tasklist)
                            try
                            {
                                if (Task != null) Task.Node.Remove();
                            }
                            catch { }
                    }
                    catch {}
                    try
                    {
                        foreach (DocViza Viza in Vizalist)
                            try
                            {
                                if (Viza != null) Viza.Node.Remove();
                            }
                            catch { }
                    }
                catch{}
                    try
                    {
                        foreach (DocNew News in Newlist)
                            try
                            {
                                if (News != null) News.Node.Remove();
                            }
                            catch { }
                    }
                    catch { }
                }
                catch { }
                Doc_Node.Remove();
                RES = true;
            }
            catch { RES = false; }
            return RES;
        }

        public string ResultString = "";

        private static Class_syb_acc SybAcc;

        public class DocTask
        {
            private int moving;
            private DateTime start;
            private DateTime finish;
            private DateTime executed;
            private string resolution = "";
            private int Employee = 0;
            private int Department = 0;
            public bool Viewing = false;
            private ClassDocsItem IssueDocument;
            private System.Windows.Forms.TreeNode DocNode;
            System.Windows.Forms.TreeNode Parent_Node;
            private string degree_i_FIO = "";
            private int for_ = 0;
            private bool MainExec = false;
            private Class_syb_acc SybAcc;
            private int IssueDocId = 0;
            private int ImageIndex = AskAsTask;
            public int TASK_DocId;

            public System.Windows.Forms.TreeNode Node
            {
                get { return Parent_Node; }
                set
                {
                    string prefix = "m";
                    Parent_Node = value;
                    DocNode = new System.Windows.Forms.TreeNode();
                    if (IssueDocument != null) prefix = "x"; else prefix = "m";
                    DocNode.Name = prefix + (string)Convert.ToString(moving);
                    DocNode.ImageIndex = ImageIndex;
                    Parent_Node.Nodes.Add(DocNode);
                    DocNode.Text = degree_i_FIO;
                }
            }

            public int Task_ID
            {
                get { return moving; }
                set
                {
                    moving = value;
                    if (moving > 0)
                    {
                        if (degree_i_FIO.Length == 0) Doc_executor();
                    }
                }
            }
            public string Task_resolution { get { return resolution; } set { resolution = value; } }
            public int Task_Employee { get { return Employee; } set { Employee = value; } }
            public int Task_Department { get { return Department; } set { Department = value; } }
            public DateTime Task_start { get { return start; } set { start = value; } }
            public DateTime Task_finish { get { return finish; } set { finish = value; } }
            public int Task_For { get { return for_; } set { for_ = value; } }
            public DateTime Task_executed { get { return executed; } set { executed = value; } }
            public ClassDocsItem TASK_IssueDocument { get { return IssueDocument; } }
            public System.Windows.Forms.TreeNode TASK_DocNode { get { return DocNode; } }
            public bool Task_MainExecutor { get { return MainExec; } set { MainExec = value; } }

            public int TASK_IssueDocId
            {
                get { return IssueDocId; }
                set
                {
                    IssueDocId = value;
                    if (IssueDocId > 0)
                    {

                        if (SybAcc.Set_table("TCDoc1", AMAS_Query.Class_AMAS_Query.Get_document_by_ID(IssueDocId),null))
                        {
                            try
                            {
                                IssueDocument = new ClassDocsItem(SybAcc);
                            }
                            catch (Exception e) 
                            {
                                SybAcc.EBBLP.AddError(e.Message, "Document - 1", e.StackTrace);
                            }
                            SybAcc.ReturnTable();
                        }
                        ImageIndex = DocAsTaskExec;
                    }
                }
            }
            public string Task_degree_i_FIO { get { return degree_i_FIO; } }

            public DocTask(Class_syb_acc Syb)
            {
                moving = 0;
                SybAcc = Syb;
            }

            private void Doc_executor()
            {
                string sql;
                int cc = 0;
                AMAS_DBI.Class_syb_acc.PrepareParameters[] pD = new Class_syb_acc.PrepareParameters[1];
                try
                {
                    pD[0] = new AMAS_DBI.Class_syb_acc.PrepareParameters("@executed", SqlDbType.DateTime, executed);
                }
                catch
                {
                    pD[0] = null;
                }

                if (IssueDocId > 0)
                {
                    if (for_ > 0)
                    {
                        sql = AMAS_Query.Class_AMAS_Query.Get_rank_from_heap(executed, for_);
                    }
                    else
                    {
                        sql = AMAS_Query.Class_AMAS_Query.Get_moving_from_heap(moving);
                        if (SybAcc.Set_table("TCDoc2", sql, null))
                        {
                            if (SybAcc.Rows_count > 0)
                            {
                                sql = AMAS_Query.Class_AMAS_Query.Get_rank_from_heap(executed, (int)Convert.ToInt32(SybAcc.Find_Field("for_")));
                            }
                            else { sql = ""; }
                            SybAcc.ReturnTable();
                        }
                    }
                    if (sql.Length > 0)
                    {
                        if (SybAcc.Set_table("TCDoc3", sql, pD))
                        {
                            sql = "";
                            if (SybAcc.Rows_count > 0)
                            {
                                int Id = (int)Convert.ToInt32(SybAcc.Find_Field("id"));
                                sql = AMAS_Query.Class_AMAS_Query.Get_employee_by_rank_on_heap(Id);
                            }
                            else
                                sql = AMAS_Query.Class_AMAS_Query.Get_employee_of_movie(for_);
                            SybAcc.ReturnTable();
                        }
                    }
                    else
                        sql = AMAS_Query.Class_AMAS_Query.Get_employee_of_movie(for_);

                    if (sql.Length > 0)
                    {
                        if (SybAcc.Set_table("TCDoc4", sql, null))
                        {
                            if (SybAcc.Rows_count > 0)
                                try
                                {
                                    degree_i_FIO = (string)SybAcc.Find_Field("rank") + " " + (string)SybAcc.Find_Field("fio");
                                    Department = (int)SybAcc.Find_Field("department");
                                    cc = 1;
                                }
                                catch (Exception e) 
                                {
                                    SybAcc.EBBLP.AddError(e.Message, "Document - 2", e.StackTrace);
                                }
                            SybAcc.ReturnTable();
                        }
                    }
                }
                else if (for_ > 0)
                {
                    sql = AMAS_Query.Class_AMAS_Query.Get_employee_of_movie(for_);
                    if (SybAcc.Set_table("TCDoc5", sql, null))
                    {
                        try
                        {
                            degree_i_FIO = (string)SybAcc.Find_Field("rank") + " " + (string)SybAcc.Find_Field("fio");
                            Department = (int)SybAcc.Find_Field("department");
                            cc = 1;
                        }
                        catch (Exception e) 
                        {
                            SybAcc.EBBLP.AddError(e.Message, "Document - 3", e.StackTrace);
                        }
                        SybAcc.ReturnTable();
                    }
                }

                if (Department > 0 && cc == 0)
                    try
                    {
                        if (SybAcc.Set_table("TCDoc6", AMAS_Query.Class_AMAS_Query.Get_department_of_movie(Department), null))
                        {
                            if (SybAcc.Rows_count > 0)
                                try
                                {
                                    degree_i_FIO = (string)SybAcc.Find_Field("name");
                                    Department = (int)SybAcc.Find_Field("department");
                                    cc = 1;
                                }
                                catch (Exception e) 
                                {
                                    SybAcc.EBBLP.AddError(e.Message, "Document - 4", e.StackTrace);
                                }
                            SybAcc.ReturnTable();
                        }
                    }
                    catch (Exception e) 
                    {
                        SybAcc.EBBLP.AddError(e.Message, "Document - 5", e.StackTrace);
                    }
            }

            public void DeleteTaskView()
            {
                if (DocNode != null) try { DocNode.Remove(); }
                    catch { }
            }
        }

        public class DocViza
        {
            private int vizing;
            private int viza_sheet;
            private DateTime start;
            private DateTime finish;
            private DateTime executed;
            private bool viza;
            private string viza_denote;
            private int Employee;
            private int Department;
            private System.Windows.Forms.TreeNode DocNode;
            System.Windows.Forms.TreeNode Parent_Node;
            private string degree_i_FIO;
            private int for_ = 0;
            private Class_syb_acc SybAcc;
            private int ImageIndex = AskAsViza;

            public int Viza_DocId;

            public System.Windows.Forms.TreeNode Node
            {
                get { return Parent_Node; }
                set
                {
                    Parent_Node = value;
                    DocNode = new System.Windows.Forms.TreeNode();
                    DocNode.Name = "v" + (string)Convert.ToString(vizing);
                    DocNode.ImageIndex = ImageIndex;
                    Parent_Node.Nodes.Add(DocNode);
                    DocNode.Text = degree_i_FIO;
                }
            }

            public string Viza_Note
            {
                get { return viza_denote; }
                set
                {
                    if (value == null)
                        viza_denote = "";
                    else viza_denote = value;
                }
            }

            public int Viza_ID
            {
                get { return vizing; }
                set
                {
                    if (vizing == 0)
                    {
                        vizing = value;
                        if (vizing > 0)
                        {
                            degree_i_FIO = Doc_for_dep(for_, Department);
                        }
                    }
                }
            }

            public int Viza_vizaSheet { get { return viza_sheet; } set { viza_sheet = value; } }
            public bool Viza_viza { get { return viza; } set { viza = value; } }
            public string Viza_Denote { get { return viza_denote; } set { viza_denote = value; } }
            public int Viza_Employee { get { return Employee; } set { Employee = value; } }
            public int Viza_For { get { return for_; } set { for_ = value; } }
            public int Viza_Department { get { return Department; } set { Department = value; } }
            public DateTime Viza_start { get { return start; } set { start = value; } }
            public DateTime Viza_finish { get { return finish; } set { finish = value; } }
            public DateTime Viza_executed { get { return executed; } set { executed = value; } }
            public System.Windows.Forms.TreeNode Viza_DocNode { get { return DocNode; } set { DocNode = value; } }

            public string Viza_degree_i_FIO { get { return degree_i_FIO; } }

            public DocViza(Class_syb_acc Syb)
            {
                vizing = 0;
                SybAcc = Syb;
            }

            public void DeleteVizaView()
            {
                if (DocNode != null) try { DocNode.Remove(); }
                    catch { }
            }

        }

        private static string Doc_for_dep(int for_, int Department)
        {
            string sql;
            string degree_i_FIO = "";
            if (for_ > 0)
            {
                sql =AMAS_Query.Class_AMAS_Query.Get_short_fio_of_movie(for_);
                if (SybAcc.Set_table("TCDoc7", sql, null))
                {
                    try
                    {
                        degree_i_FIO = (string)Convert.ToString(SybAcc.Find_Field("name")) + " " + (string)Convert.ToString(SybAcc.Find_Field("short_fio"));
                    }
                    catch (Exception e) 
                    {
                        SybAcc.EBBLP.AddError(e.Message, "Document - 6", e.StackTrace);
                    }
                    SybAcc.ReturnTable();
                }
                if (degree_i_FIO.Trim().Length < 1)
                {
                    sql = AMAS_Query.Class_AMAS_Query.Get_short_fio_by_rank(for_);
                    if (SybAcc.Set_table("TCDoc8", sql, null))
                    {
                        try
                        {
                            degree_i_FIO = (string)Convert.ToString(SybAcc.Find_Field("short_fio"));
                        }
                        catch (Exception e) 
                        {
                            SybAcc.EBBLP.AddError(e.Message, "Document - 7", e.StackTrace);
                        }
                        SybAcc.ReturnTable();
                    }
                }
            }
            if (Department > 0 && degree_i_FIO.Trim().Length < 1)
            {
                if (SybAcc.Set_table("TCDoc9", AMAS_Query.Class_AMAS_Query.Get_department_of_movie(Department), null))
                {
                    try
                    {
                        degree_i_FIO = (string)Convert.ToString(SybAcc.Find_Field("name"));
                    }
                    catch (Exception e) 
                    {
                        SybAcc.EBBLP.AddError(e.Message, "Document - 8", e.StackTrace);
                    }
                    SybAcc.ReturnTable();
                }
            }
            return degree_i_FIO;
        }

        public class DocNew
        {
            private int news;
            private DateTime start;
            private DateTime finish;
            private DateTime executed;
            private int Employee;
            private int Department;
            public bool Newed = false;
            private System.Windows.Forms.TreeNode DocNode;
            System.Windows.Forms.TreeNode Parent_Node;
            public string degree_i_FIO;
            private int for_ = 0;
            private Class_syb_acc SybAcc;
            private int ImageIndex = AskAsNews;

            public System.Windows.Forms.TreeNode Node
            {
                get { return Parent_Node; }
                set
                {
                    Parent_Node = value;
                    DocNode = new System.Windows.Forms.TreeNode();
                    DocNode.Name = "n" + (string)Convert.ToString(news);
                    DocNode.ImageIndex = ImageIndex;
                    Parent_Node.Nodes.Add(DocNode);
                    DocNode.Text = degree_i_FIO;
                }
            }

            public int News_ID
            {
                get { return news; }
                set
                {
                    news = value;
                    if (news > 0)
                    {
                        degree_i_FIO = Doc_for_dep(for_, Department);
                    }
                }
            }
            public int News_Employee { get { return Employee; } set { Employee = value; } }
            public int News_Department { get { return Department; } set { Department = value; } }
            public int News_For { get { return for_; } set { for_ = value; } }
            public DateTime News_start { get { return start; } set { start = value; } }
            public DateTime News_finish { get { return finish; } set { finish = value; } }
            public DateTime News_executed
            {
                get { return executed; }
                set
                {
                    executed = value;
                    if (executed > DateTime.MinValue) ImageIndex = DocAsNewExec;
                }
            }

            public DocNew(Class_syb_acc Syb)
            {
                news = 0;
                SybAcc = Syb;
            }

            public void DeleteNewsView()
            {
                if (DocNode != null) try { DocNode.Remove(); }
                    catch { }
            }

        }

        public ClassDocsItem(Class_syb_acc DB, System.Windows.Forms.TreeNode ND, ClassDocsItem.refer_NodeToDoc LRND)
        {
            SybAcc = DB;
            LastReferNodeToDoc = LRND;
            IDent = ListCount++;
            fill_data();
            if (ND != null) Locate_Node(ND);
            //DocContent = new Document_content(DocID, DB);
        }

        public ClassDocsItem(Class_syb_acc DB)
        {
            SybAcc = DB;
            LastReferNodeToDoc = null;
            IDent = ListCount++;
            fill_data();
            //DocContent = new Document_content(DocID, DB);
        }

        public ClassDocsItem(Class_syb_acc DB, ClassDocsItem.refer_NodeToDoc LRND)
        {
            SybAcc = DB;
            LastReferNodeToDoc = LRND;
            IDent = ListCount++;
            fill_data();
            //DocContent = new Document_content(DocID, DB);
        }

        private void fill_data()
        {
            try { RKK = (string)SybAcc.Find_Field("find_cod"); }
            catch { RKK = ""; }
            try { DocID = (int)SybAcc.Find_Field("kod"); }
            catch { DocID = 0; }
            try { typist = (int)SybAcc.Find_Field("typist"); }
            catch { typist = 0; }
            try { kind = (int)SybAcc.Find_Field("kind"); }
            catch { kind = 0; }
            try { annotation = (string)SybAcc.Find_Field("annot"); }
            catch { annotation = ""; }
            try { created = (DateTime)SybAcc.Find_Field("date_f"); }
            catch { created = DateTime.MinValue; }
            try { parent_moving = (int)SybAcc.Find_Field("moving"); }
            catch { parent_moving = 0; }
            try { parent_new = (int)SybAcc.Find_Field("news"); }
            catch { parent_new = 0; }
            try { parent_viza = (int)SybAcc.Find_Field("vizing"); }
            catch { parent_viza = 0; }
            try { Viewed = (bool)SybAcc.Find_Field("viewing"); }
            catch { Viewed = false; }
            try { Newed = (bool)SybAcc.Find_Field("Newed"); }
            catch { Newed = false; }
        }

        public void Locate_Node(System.Windows.Forms.TreeNode ND)
        {
            if (ND != null)
            {
                Node = ND;
                //while (Node.Parent != null) Node = Node.Parent;
                //Node.Nodes.Clear();
                Node.Name = "d" + (string)Convert.ToString(IDent);
                Node.Text = RKK;
                Tasklist = get_task_list(Doc_id);
                Vizalist = get_viza_list(Doc_id);
                Newlist = get_new_list(Doc_id);
                if (Tasklist != null && Vizalist != null && Newlist != null) Node.ImageIndex = DocAsTaskVizaNews;
                else if (Tasklist != null && Vizalist != null && Newlist == null) Node.ImageIndex = DocAsTaskViza;
                else if (Tasklist != null && Vizalist == null && Newlist != null) Node.ImageIndex = DocAsTaskNews;
                else if (Tasklist == null && Vizalist != null && Newlist != null) Node.ImageIndex = DocAsVizaNews;
                else if (Tasklist != null && Vizalist == null && Newlist == null) Node.ImageIndex = DocAsTask;
                else if (Tasklist == null && Vizalist != null && Newlist == null) Node.ImageIndex = DocAsViza;
                else if (Tasklist == null && Vizalist == null && Newlist != null) Node.ImageIndex = DocAsNews;
                else Node.ImageIndex = DocAsTask;
                NodeAtDOcument = new refer_NodeToDoc(this, ND, LastReferNodeToDoc);
            }
            else NodeAtDOcument = null;
        }

        ~ClassDocsItem()
        {
            try
            {
                if (Node.PrevVisibleNode != null)
                {
                    Node.PrevVisibleNode.Checked = true;
                }
                else if (Node.NextVisibleNode != null)
                {
                    Node.NextVisibleNode.Checked = true;
                }
                else if (Node.Parent != null)
                {
                    Node.Parent.Checked = true;
                }
                else if (Node.FirstNode != null)
                {
                    Node.FirstNode.Checked = true;
                }
                Node.Remove();
            }
            catch (Exception e) 
            {
                SybAcc.EBBLP.AddError(e.Message, "Document - 129", e.StackTrace);
            }
        }

        public int IDENT { get { return IDent; } }
        public string Doc_RKK { get { return RKK; } }
        public string DOC_annot { get { return annotation; } }
        public int Doc_id { get { return DocID; } }
        public int Doc_kind { get { return kind; } }
        public int DOC_typist { get { return typist; } }
        public DateTime DOC_created { get { return created; } }
        public int Task_ORG { get { return ORG; } }
        public int Task_AUTOR { get { return AUTOR; } }
        public int Task_MAN { get { return MAN; } }

        public DocTask[] Tasklist = null;
        public DocViza[] Vizalist = null;
        public DocNew[] Newlist = null;
        public ClassDocsItem[] Doclist = null;

        private DocTask[] get_task_list(int Docid)
        {
            DocTask[] Tsklist = null;
            ResultString = "";
            string sql;
            if (Own_pass)
                sql = AMAS_Query.Class_AMAS_Query.Get_own_movies_of_doc(Docid);
            else
                sql = AMAS_Query.Class_AMAS_Query.Get_movies_of_doc(Docid);


            if (SybAcc.Set_table("TCDoc10", sql, null))
            {
                try
                {
                    if (SybAcc.Rows_count > 0)
                        Tsklist = set_Tasklist();
                    else Tsklist = new DocTask[0];
                }
                catch (Exception ex)
                {
                    SybAcc.EBBLP.AddError(ex.Message, "Document - 9", ex.StackTrace);
                }
                SybAcc.ReturnTable();
            }
            return Tsklist;
        }

        public void add_taskToList(int[] mmm)
        {
            string sql = "select * from dbo.rkk_moving where moving in (";
            for (int i = 0; i < mmm.Length; i++)
                if (i == 0) sql += mmm[i].ToString();
                else sql +=","+ mmm[i].ToString();
                sql += ")";

                if (SybAcc.Set_table("TCDoc10.1", sql, null))
                {
                    try
                    {
                        if (SybAcc.Rows_count > 0)
                        {
                            DocTask[] Addlist = set_Tasklist();
                            DocTask[] NewList= new DocTask[Tasklist.Length+Addlist.Length];
                            for (int i = 0; i < Tasklist.Length; i++)
                                NewList[i] = Tasklist[i];
                           for (int i = 0; i < Addlist.Length; i++)
                               NewList[i + Tasklist.Length] = Addlist[i];
                           Tasklist = NewList;
                            foreach (DocTask tsk in Addlist)
                            {
                                foreach (DocTask Task in Tasklist)
                                {
                                        try
                                        {
                                            if (Task.Task_For == tsk.Task_For)
                                            {
                                                tsk.Node = Task.TASK_DocNode;
                                                break;
                                            }
                                        }
                                        catch { }
                                }
                                if (tsk != null) 
                                    if (tsk.Node == null) 
                                        tsk.Node = Node;
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        SybAcc.EBBLP.AddError(ex.Message, "Document - 10.1", ex.StackTrace);
                    }
                    SybAcc.ReturnTable();
                }
            }

        public void add_vizaToList(int[] mmm)
        {
            string sql = "select * from dbo.rkk_vizing where id in (";
            for (int i = 0; i < mmm.Length; i++)
                if (i == 0) sql += mmm[i].ToString();
                else sql += "," + mmm[i].ToString();
            sql += ")";

            if (SybAcc.Set_table("TCDoc10.2", sql, null))
            {
                try
                {
                    if (SybAcc.Rows_count > 0)
                    {
                        DocViza[] Addlist = set_Vizalist();
                        DocViza[] NewList = new DocViza[Vizalist.Length + Addlist.Length];
                        for (int i = 0; i < Vizalist.Length; i++)
                            NewList[i] = Vizalist[i];
                        for (int i = 0; i < Addlist.Length; i++)
                            NewList[i + Vizalist.Length] = Addlist[i];
                        Vizalist = NewList;
                        foreach (DocViza viza in Addlist)
                        {
                            foreach (DocViza Viz in Vizalist)
                            {
                                try
                                {
                                    if (Viz.Viza_For == viza.Viza_For)
                                    {
                                        viza.Node = Viz.Viza_DocNode;
                                        break;
                                    }
                                }
                                catch { }
                            }
                            if (viza != null) 
                                if (viza.Node == null) 
                                    viza.Node = Node;
                        }
                    }
                }
                catch (Exception ex)
                {
                    SybAcc.EBBLP.AddError(ex.Message, "Document - 10.2", ex.StackTrace);
                }
                SybAcc.ReturnTable();
            }
        }

        public void add_newsToList(int[] mmm)
        {
            string sql = "select * from dbo.rkk_news where news in (";
            for (int i = 0; i < mmm.Length; i++)
                if (i == 0) sql += mmm[i].ToString();
                else sql += "," + mmm[i].ToString();
            sql += ")";

            if (SybAcc.Set_table("TCDoc10.3", sql, null))
            {
                try
                {
                    if (SybAcc.Rows_count > 0)
                    {
                        DocNew[] Addlist = set_Newlist();
                        foreach (DocNew news in Addlist)
                        {
                            foreach (DocTask Task in Tasklist)
                            {
                                try
                                {
                                    if (Task.Task_For == news.News_Employee)
                                    {
                                        news.Node = Task.TASK_DocNode;
                                        break;
                                    }
                                }
                                catch { }
                            }
                        }
                        DocNew[] NList = new DocNew[Newlist.Length + Addlist.Length];
                        for (int i = 0; i < Newlist.Length; i++)
                            NList[i] = Newlist[i];
                        for (int i = 0; i < Addlist.Length; i++)
                            NList[i + Newlist.Length] = Addlist[i];
                        Newlist = NList;
                    }
                }
                catch (Exception ex)
                {
                    SybAcc.EBBLP.AddError(ex.Message, "Document - 10.3", ex.StackTrace);
                }
                SybAcc.ReturnTable();
            }
        }

        private DocTask[] set_Tasklist()
        {
            DocTask[] Tsklist=new DocTask[SybAcc.Rows_count];
            try
            {
                        for (int i = 0; i < SybAcc.Rows_count; i++)
                        {
                            if (SybAcc.Rows_count > 0)
                            {
                                if (Tsklist[i] == null) Tsklist[i] = new DocTask(SybAcc);
                                SybAcc.Get_row(i);
                                try { Tsklist[i].Task_Department = (int)SybAcc.Find_Field("department"); }
                                catch { Tsklist[i].Task_Department = -1; }
                                try { Tsklist[i].TASK_DocId = (int)SybAcc.Find_Field("document"); }
                                catch { Tsklist[i].TASK_DocId = -1; }
                                try { Tsklist[i].Task_Employee = (int)SybAcc.Find_Field("last_employee"); }
                                catch { Tsklist[i].Task_Employee = -1; }
                                try { Tsklist[i].Task_For = (int)SybAcc.Find_Field("for_"); }
                                catch { Tsklist[i].Task_For = -1; }
                                try { Tsklist[i].Task_start = (DateTime)SybAcc.Find_Field("time_m"); }
                                catch { Tsklist[i].Task_start = DateTime.MinValue; }
                                try { Tsklist[i].Task_finish = (DateTime)SybAcc.Find_Field("when_m"); }
                                catch { Tsklist[i].Task_finish = DateTime.MinValue; }
                                try { Tsklist[i].Task_executed = (DateTime)SybAcc.Find_Field("executed"); }
                                catch { Tsklist[i].Task_executed = DateTime.MinValue; }
                                try { Tsklist[i].TASK_IssueDocId = (int)SybAcc.Find_Field("exe_doc"); }
                                catch { Tsklist[i].TASK_IssueDocId = -1; }
                                try { Tsklist[i].Task_ID = (int)SybAcc.Find_Field("moving"); }
                                catch { Tsklist[i].Task_ID = -1; }
                                try { Tsklist[i].Viewing = (bool)SybAcc.Find_Field("Viewing"); }
                                catch { Tsklist[i].Viewing = false; }
                                try { Tsklist[i].Task_MainExecutor = (bool)SybAcc.Find_Field("main_executor"); }
                                catch { Tsklist[i].Task_MainExecutor = false; }
                                try
                                {
                                    if (SybAcc.Find_Field("signing") != null)
                                        Tsklist[i].Task_resolution = (string)SybAcc.Find_Field("signing");
                                }
                                catch {Tsklist[i].Task_resolution ="";}
                            }
                        }
            }
            catch (Exception ex)
            {
                SybAcc.EBBLP.AddError(ex.Message, "Document - 11", ex.StackTrace);
            }
            return Tsklist;
        }

        public void ShowTasks()
        {
            int i = 0;
            foreach (DocTask Task in Tasklist)
            {
                for (int k = 0; k < i; k++)
                {
                    try
                    {
                        if (Tasklist[k].Task_For == Task.Task_Employee)
                        {
                            Task.Node = Tasklist[k].TASK_DocNode;
                            break;
                        }
                    }
                    catch { }
                }
                i++;
                if (Task != null) if (Task.Node == null) Task.Node = Node;
            }
        }

        public void ShowVizy()
        {
            int i = 0;
            foreach (DocViza Viza in Vizalist)
            {
                if (Tasklist != null)
                    foreach (DocTask Task in Tasklist)
                    {
                        try
                        {
                            if (Viza.Viza_For == Task.Task_Employee)
                            {
                                Viza.Node = Task.TASK_DocNode;
                                break;
                            }
                        }
                        catch { }
                    }
                if (Viza != null) if (Viza.Node == null)
                        for (int k = 0; k < i; k++)
                        {
                            try
                            {
                                if (Vizalist[k].Viza_For == Viza.Viza_Employee)
                                {
                                    Viza.Node = Vizalist[k].Viza_DocNode;
                                    break;
                                }
                            }
                            catch { }
                        }
                if (Viza != null) if (Viza.Node == null) Viza.Node = Node;
                i++;
            }
        }

        public void ShowNews()
        {
            foreach (DocNew News in Newlist)
                if (News != null) if (News.Node == null) News.Node = Node;
        }

        private string IconShow(DocTask Task)
        {
            if (Task.Task_MainExecutor) return "mainExecutor";
            else return "task";
        }

        public void ListTasks(System.Windows.Forms.ListView Listing, int DId)
        {
            DocTask[] Tsklist = null;
            if (Listing != null)
                TVNListing = Listing;

            if (TVNListing != null)
            {
                TVNListing.ShowGroups = true;
                ListViewGroup TaskGroup = null;
                foreach (ListViewGroup Group in TVNListing.Groups)
                    if (Group.Name.CompareTo("ExeGroup") == 0)
                    {
                        TaskGroup = Group;
                        TaskGroup.Items.Clear();
                        break;
                    }
                if (TaskGroup == null) TaskGroup = TVNListing.Groups.Add("ExeGroup", "Поручение");

                Tsklist = get_task_list(DId);
                if (Tsklist != null)
                    foreach (DocTask Task in Tsklist)
                        if (Task != null)
                        {
                            ListViewItem itemL = TVNListing.Items.Add("T" + Task.Task_ID.ToString(), Task.Task_degree_i_FIO, IconShow(Task));
                            if (Task.Task_MainExecutor)
                                itemL.ForeColor = System.Drawing.Color.Blue;
                            else itemL.ForeColor = System.Drawing.Color.Black;
                            TaskGroup.Items.Add(itemL);
                            itemL.SubItems.Add(Task.Task_start.ToShortDateString());
                            itemL.SubItems.Add(Task.Task_finish.ToShortDateString());
                            if (Task.Task_executed != null)
                                if (Task.Task_executed > DateTime.MinValue) itemL.SubItems.Add(Task.Task_executed.ToShortDateString());
                                else itemL.SubItems.Add("");
                            else itemL.SubItems.Add("");
                            itemL.SubItems.Add(Task.Task_resolution.Trim());
                            if (Task.TASK_IssueDocId > 0)
                                itemL.SubItems.Add(AMASCommand.GetDocumentTypist(Task.TASK_IssueDocId));
                            else
                                itemL.SubItems.Add("");
                            itemL.SubItems.Add(Task.TASK_IssueDocId.ToString());
                            itemL.SubItems.Add(Task.Task_ID.ToString());
                        }
            }
        }

            DocViza[] Vizlist = null;
            
        public void ListVisa(System.Windows.Forms.ListView Listing, int DId)
        {
            string IMGKey = "Viza";
            if (Listing != null)
                TVNListing = Listing;

            if (TVNListing != null)
            {
                TVNListing.ShowGroups = true;
                ListViewGroup VisaGroup = null;
                foreach (ListViewGroup Group in TVNListing.Groups)
                    if (Group.Name.CompareTo("VizaGroup") == 0)
                    {
                        VisaGroup = Group;
                        VisaGroup.Items.Clear();
                        break;
                    }
                if (VisaGroup == null) VisaGroup = TVNListing.Groups.Add("VizaGroup", "Визирование");

                Vizlist = get_viza_list(DId);
                if (Vizlist != null)
                {
                    foreach (DocViza Viza in Vizlist)
                        if (Viza != null)
                        {
                            if (Viza.Viza_vizaSheet > 0)
                                if (Viza.Viza_viza)
                                    IMGKey = "VizaTrue";
                                else IMGKey = "VizaFalse";
                            ListViewItem itemL = TVNListing.Items.Add("V" + Viza.Viza_ID.ToString(), Viza.Viza_degree_i_FIO, IMGKey);
                            if (Viza.Viza_executed > DateTime.MinValue)
                                if (Viza.Viza_viza) itemL.ForeColor = System.Drawing.Color.Green;
                                else itemL.ForeColor = System.Drawing.Color.Red;
                            else itemL.ForeColor = System.Drawing.Color.Black;
                            VisaGroup.Items.Add(itemL);
                            itemL.SubItems.Add(Viza.Viza_start.ToShortDateString());
                            itemL.SubItems.Add(Viza.Viza_finish.ToShortDateString());
                            if (Viza.Viza_executed != null)
                                if (Viza.Viza_executed > DateTime.MinValue)
                                {
                                    itemL.SubItems.Add(Viza.Viza_executed.ToShortDateString());
                                    itemL.SubItems.Add(AMASCommand.GetViza(Viza.Viza_viza, Viza.Viza_vizaSheet));
                                }
                                else
                                {
                                    itemL.SubItems.Add("");
                                    itemL.SubItems.Add("");
                                }
                            else
                            {
                                itemL.SubItems.Add("");
                                itemL.SubItems.Add("");
                            }
                            //if (Viza.Viza_Note != null) itemL.SubItems.Add(Viza.Viza_Note.Trim());
                            //else itemL.SubItems.Add("");
                            itemL.SubItems.Add("");
                            itemL.SubItems.Add(Viza.Viza_vizaSheet.ToString());
                            itemL.SubItems.Add(Viza.Viza_ID.ToString());
                        }
                 }
            }
        }

        public string VisaDenote(int id)
        {
            string res = "";
            if (Vizlist != null)
                foreach (DocViza Viza in Vizlist)
                    if (Viza != null)
                        if (Viza.Viza_ID == id)
                            res = Viza.Viza_Denote;
            return res;
        }

        public void ListNews(System.Windows.Forms.ListView Listing, int DId)
        {
                        
            DocNew[] Newslist = null;
            if (Listing != null)
                TVNListing = Listing;

            if (TVNListing != null)
            {
                TVNListing.ShowGroups = true;
                ListViewGroup NewsGroup = null;
                foreach (ListViewGroup Group in TVNListing.Groups)
                    if (Group.Name.CompareTo("NewsGroup") == 0)
                    {
                        NewsGroup = Group;
                        NewsGroup.Items.Clear();
                        break;
                    }
                if (NewsGroup == null) NewsGroup = TVNListing.Groups.Add("NewsGroup", "Для информации");

                Newslist = get_new_list(DId);
                if (Newslist != null)
                    foreach (DocNew News in Newslist)
                        if (News != null)
                        {
                            ListViewItem itemL = TVNListing.Items.Add("N" + News.News_ID.ToString(), News.degree_i_FIO,"newIMG");
                            NewsGroup.Items.Add(itemL);
                            itemL.SubItems.Add(News.News_start.ToShortDateString());
                            itemL.SubItems.Add(News.News_finish.ToShortDateString());
                            if (News.News_executed != null)
                                if (News.News_executed > DateTime.MinValue)
                                    itemL.SubItems.Add(News.News_executed.ToShortDateString());
                                else itemL.SubItems.Add("");
                            else itemL.SubItems.Add("");
                            itemL.SubItems.Add("");
                            itemL.SubItems.Add("");
                            itemL.SubItems.Add("");
                            itemL.SubItems.Add(News.News_ID.ToString());
                        }
            }

        }

        private DocViza[] get_viza_list(int Docid)
        {
            DocViza[] Vizlist = null;
            ResultString = "";
            string sql;
            if (Own_pass)
                sql = AMAS_Query.Class_AMAS_Query.Get_own_Vizy_of_doc(Docid);
            else
                sql = AMAS_Query.Class_AMAS_Query.Get_Vizy_of_doc(Docid);
            try
            {
                if (SybAcc.Set_table("TCDoc11", sql, null))
                {
                    if (SybAcc.Rows_count > 0)
                    {
                        Vizlist = set_Vizalist();
                    }
                    SybAcc.ReturnTable();
                }
            }
            catch (Exception ex)
            {
                SybAcc.EBBLP.AddError(ex.Message, "Document - 12", ex.StackTrace);
                ResultString = ex.Message;
            }
            return Vizlist;
        }

        private DocViza[] set_Vizalist()
        {
            DocViza[] Vizlist = new DocViza[SybAcc.Rows_count];
            for (int i = 0; i < SybAcc.Rows_count; i++)
                try
                {
                    SybAcc.Get_row(i);
                    Vizlist[i] = new DocViza(SybAcc);
                    try { Vizlist[i].Viza_Department = (int)SybAcc.Find_Field("department"); }
                    catch { Vizlist[i].Viza_Department = -1; }
                    try { Vizlist[i].Viza_DocId = (int)SybAcc.Find_Field("document"); }
                    catch { Vizlist[i].Viza_DocId = -1; }
                    try { Vizlist[i].Viza_Employee = (int)SybAcc.Find_Field("last_employee"); }
                    catch { Vizlist[i].Viza_Employee = -1; }
                    try { Vizlist[i].Viza_start = (DateTime)SybAcc.Find_Field("dat_change"); }
                    catch { Vizlist[i].Viza_start = DateTime.MinValue; }
                    try { Vizlist[i].Viza_finish = (DateTime)SybAcc.Find_Field("when_v"); }
                    catch { Vizlist[i].Viza_finish = DateTime.MinValue; }
                    try { Vizlist[i].Viza_executed = (DateTime)SybAcc.Find_Field("executed"); }
                    catch { Vizlist[i].Viza_executed = DateTime.MinValue; }
                    try { Vizlist[i].Viza_For = (int)SybAcc.Find_Field("for_"); }
                    catch { Vizlist[i].Viza_For = -1; }
                    try { Vizlist[i].Viza_viza = (bool)SybAcc.Find_Field("YES_NO"); }
                    catch { Vizlist[i].Viza_viza = false; }
                    try { Vizlist[i].Viza_vizaSheet = (int)SybAcc.Find_Field("viza"); }
                    catch { Vizlist[i].Viza_vizaSheet = -1;  }
                    try { Vizlist[i].Viza_ID = (int)SybAcc.Find_Field("id"); }
                    catch { Vizlist[i].Viza_ID = -1; }
                    try { Vizlist[i].Viza_Note = (string)SybAcc.Find_Field("notes"); }
                    catch { Vizlist[i].Viza_Note = ""; }
                }
                catch (Exception ex)
                {
                    SybAcc.EBBLP.AddError(ex.Message, "Document - 12.1", ex.StackTrace);
                    ResultString = ex.Message;
                }
            return Vizlist;
        }

        private DocNew[] get_new_list(int Docid)
        {
            DocNew[] Nwlist = null;
            ResultString = "";
            string sql;
            if (Own_pass)
                sql = AMAS_Query.Class_AMAS_Query.Get_own_news_of_doc(Docid);
            else
                sql = AMAS_Query.Class_AMAS_Query.Get_news_of_doc(Docid);
            try
            {
                if (SybAcc.Set_table("TCDoc13", sql, null))
                {
                    if (SybAcc.Rows_count > 0)
                    {
                        Nwlist = set_Newlist();
                    }
                    SybAcc.ReturnTable();
                }
            }
            catch (Exception ex)
            {
                SybAcc.EBBLP.AddError(ex.Message, "Document - 13", ex.StackTrace);
                ResultString = ex.Message;
            }
            return Nwlist;
        }

        private DocNew[] set_Newlist()
        {
            DocNew[] Nwlist = new DocNew[SybAcc.Rows_count];
            for (int i = 0; i < SybAcc.Rows_count; i++)
            try
            {
                Nwlist[i] = new DocNew(SybAcc);
                SybAcc.Get_row(i);
                try { Nwlist[i].News_Department = (int)SybAcc.Find_Field("department"); }
                catch { Nwlist[i].News_Department = -1; }
                try { Nwlist[i].News_start = (DateTime)SybAcc.Find_Field("time_n"); }
                catch { Nwlist[i].News_start = DateTime.MinValue; }
                try { Nwlist[i].News_finish = (DateTime)SybAcc.Find_Field("when_n"); }
                catch { Nwlist[i].News_finish = DateTime.MinValue; }
                try { Nwlist[i].News_executed = (DateTime)SybAcc.Find_Field("newed"); }
                catch { Nwlist[i].News_executed = DateTime.MinValue; }
                try { Nwlist[i].News_For = (int)SybAcc.Find_Field("for_"); }
                catch { Nwlist[i].News_For = -1; }
                try { Nwlist[i].News_ID = (int)SybAcc.Find_Field("news"); }
                catch { Nwlist[i].News_ID = -1; }
                try { Nwlist[i].Newed = (bool)SybAcc.Find_Field("Newed"); }
                catch { Nwlist[i].Newed = false; }
            }
            catch (Exception ex)
            {
                SybAcc.EBBLP.AddError(ex.Message, "Document - 13.1", ex.StackTrace);
                ResultString = ex.Message;
            }
        return Nwlist;
        }
    }

    public class AMAS_Node : System.Windows.Forms.TreeNode
    {
        public ClassDocsItem AMAS_Doc;
        public AMAS_Node(ClassDocsItem document)
        {
            AMAS_Doc = document;
        }
    }
}
  

