using System;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Generic;
using AMAS_DBI;
using AMAS_Query;

namespace ClassStructure
{
    public class Structure
    {
        const int Icon_dept = 21;
        const int Icon_web_dept = 19;

        const int its_moving = 1;
        const int its_vizing = 2;
        const int its_news = 3;

        public ArrayList taskSended;
        public ArrayList vizaSended;
        public ArrayList newsSended;

        private Groups Groups;
        public Groups UsedGroups { get { return Groups;  } }
        private bool Drag_droping = false;

        public bool Drag_drop
        {
            get { return Drag_droping; }
            set
            {
                bool res = (bool)value;
                if (!Drag_droping && res) 
                    allow_dragDrop();
                if (Drag_droping && !res) 
                    disallow_dragDrop();
            }
        }

        private int count_enter_price;
        private int my_count_enter_price;
        private int[] cur_pttp;
        private int[] my_cur_pttp;

        private string[] dep_ments;
        private long[] cods_enter;
        private long[] unders_s;
        private int[] point_s;
        public bool Show_Empl = true;

        private string[] my_dep_ments;
        private long[] my_cods_enter;
        private long[] my_unders_s;
        private int[] my_point_s;

        public long senD_Document;

        public string[] Files_in_list;
        public int Files_in_Count;

        public long shut_DELO;
        public long doC_Kod;

        public byte exe_or_viz;
        private bool Check_Ikon = false;

        private AMAS_DBI.Class_syb_acc SybAcc;
        private System.Windows.Forms.TreeView TreeStructure;

        public System.Windows.Forms.TreeView TreeV { get { return TreeStructure; } }
        Color fromColor; 
        Color targetColor; 

        private void base_color()
        {
           fromColor = TreeStructure.ForeColor;
           targetColor = TreeStructure.ForeColor;
        }

        
        public Structure(AMAS_DBI.Class_syb_acc Syb)
        {
            SybAcc = Syb;
            if (AMASCommand.Access == null) AMASCommand.AccessCommands(SybAcc);
            if (enterprise_str()) order_departments(count_enter_price, unders_s, cods_enter, dep_ments, cur_pttp);
            draw_WebSshema_enterprice();
            taskSended = new ArrayList();
            vizaSended = new ArrayList();
            newsSended = new ArrayList();
        }
 
        public Structure(AMAS_DBI.Class_syb_acc Syb, System.Windows.Forms.TreeView Tree)
        {
            SybAcc = Syb;
            if (AMASCommand.Access == null) AMASCommand.AccessCommands(SybAcc);
            TreeStructure = Tree;
            TreeStructure.SelectedImageIndex = 29;
            TreeStructure.Visible = true;
            if (enterprise_str()) order_departments(count_enter_price,unders_s,cods_enter,dep_ments,cur_pttp);
            draw_shema_enterprice();
            base_color();
            taskSended = new ArrayList();
            vizaSended = new ArrayList();
            newsSended = new ArrayList();
        }
        
        public Structure(AMAS_DBI.Class_syb_acc Syb, System.Windows.Forms.TreeView Tree, bool showusers)
        {
            SybAcc = Syb;
            if (AMASCommand.Access == null) AMASCommand.AccessCommands(SybAcc);
            TreeStructure = Tree;
            TreeStructure.SelectedImageIndex = 29;
            TreeStructure.Visible = true;

            Show_Empl = showusers;

            if (enterprise_str()) order_departments(count_enter_price, unders_s, cods_enter, dep_ments, cur_pttp);
            draw_shema_enterprice();
            base_color();
            taskSended = new ArrayList();
            vizaSended = new ArrayList();
            newsSended = new ArrayList();
        }

        public Structure(AMAS_DBI.Class_syb_acc Syb, System.Windows.Forms.TreeView Tree, bool showusers, bool ikons)
        {
            Check_Ikon = ikons;
            SybAcc = Syb;
            if (AMASCommand.Access == null) AMASCommand.AccessCommands(SybAcc);
            TreeStructure = Tree;
            TreeStructure.SelectedImageIndex = 29;
            TreeStructure.Visible = true;

            Show_Empl = showusers;

            if (enterprise_str()) order_departments(count_enter_price, unders_s, cods_enter, dep_ments, cur_pttp);
            draw_shema_enterprice();
            base_color();
            taskSended = new ArrayList();
            vizaSended = new ArrayList();
            newsSended = new ArrayList();
        }

        public class Metains
        {
            private string line;
            private string cod;

            public Metains(string inLine, string inCod)
            {
                line = inLine;
                cod = inCod;
            }

            public string Instruction
            {
                get
                {
                    return line;
                }
            }

            public string Ident
            {
                get
                {
                    return cod;
                }
            }
        }

        public void Instruction_append(string Text, int ident, ListBox listInstructions, ArrayList MetaInstruct)
        {
            Metains MIns = new Metains(Text, ident.ToString());
            MetaInstruct.Add(MIns);
            listInstructions.DataSource = null;
            listInstructions.Items.Clear();
            listInstructions.DataSource = MetaInstruct;
            listInstructions.DisplayMember = "Instruction";
            listInstructions.ValueMember = "Ident";
        }

        public void Instruction_list(TreeNode instrNode, ListBox listInstructions, ArrayList MetaInstruct)
        {
            MetaInstruct.Clear();
            listInstructions.DataSource = null;
            listInstructions.Items.Clear();
            if (instrNode != null)
            {
                string sql = "";
                int ident = (int)Convert.ToInt32(instrNode.Name.Substring(1, instrNode.Name.Length - 1));
                switch (instrNode.Name.Substring(0, 1).ToLower())
                {
                    case "d":
                        sql = AMAS_Query.Class_AMAS_Query.List_of_dep_instructions(ident);
                        break;
                    case "e":
                        sql = AMAS_Query.Class_AMAS_Query.List_of_rank_instructions(ident);
                        break;
                    default:
                        sql = "";
                        break;
                }
                if (sql.Length > 0)
                {
                    if (SybAcc.Set_table("TStruct5", sql, null))
                    {
                        try
                        {
                            for (int l = 0; l < SybAcc.Rows_count; l++)
                            {
                                SybAcc.Get_row(l);
                                Metains MIns = new Metains((string)SybAcc.Find_Field("line"), SybAcc.Find_Field("cod").ToString());
                                MetaInstruct.Add(MIns);
                            }
                            if (MetaInstruct.Count > 0)
                            {
                                listInstructions.DataSource = MetaInstruct;
                                listInstructions.DisplayMember = "Instruction";
                                listInstructions.ValueMember = "Ident";
                            }

                        }
                        catch (Exception e) 
                        { 
                            SybAcc.EBBLP.AddError(e.Message, "Structure - 1", e.StackTrace);
                        }
                        SybAcc.ReturnTable();
                    }
                }
            }
        }

        public void UseGroups(TreeView TreeGroups, bool MyGroups)
        {
            if (TreeGroups != null)
            {
                Groups = new Groups(SybAcc, TreeGroups, MyGroups);
                Groups.CheckTree = true;
            }
        }


        public Image Get_Photo(int employee)
        {
            Image Photo = null;
            if (SybAcc.Set_table("TStruct6", AMAS_Query.Class_AMAS_Query.Get_photo(employee), null))
            {
                try
                {

                    SybAcc.Get_row(0);
                    SybAcc.Find_Stream("photo");
                    Photo = Image.FromStream(SybAcc.get_current_Stream());
                }
                catch { };
                SybAcc.ReturnTable();
            }
            return Photo;
        }

        public long select_dep(TreeNode Node)
        {
            long id = 0;
            Its_Dep d = null;
            if (NDep != null && Node!=null)
            {
                d = NDep.first;
                while (d != null)
                {
                    if (d.node.Name.CompareTo(Node.Name) == 0)
                    //if (d.node.Index == Node.Index)
                    {
                        id = d.ident;
                        break;
                    }
                    d = d.next;
                }
            }
            return id;
        }

        public long select_Employee(TreeNode Node)
        {
            long id = 0;
            NoEmpl Np = null;
            if (NEm != null && Node != null)
            {
                Np = NEm.first;
                while (Np != null)
                {
                    if (Np.node.Name.CompareTo(Node.Name) == 0)
                    {
                        id = Np.ident;
                        break;
                    }
                    Np = Np.next;
                }
            }
            return id;
        }

        public long Get_ManId(TreeNode Node)
        {
            int id = 0;
            NoEmpl Np = null;
            if (NEm != null && Node != null)
            {
                Np = NEm.first;
                while (Np != null)
                {
                    if (Np.node.Name.CompareTo(Node.Name) == 0)
                    {
                        id = Np.Employee;
                        break;
                    }
                    Np = Np.next;
                }
            }
            return id;
        }

        public long Get_DegreeId(TreeNode Node)
        {
            int id = 0;
            NoEmpl Np = null;
            if (NEm != null && Node != null)
            {
                Np = NEm.first;
                while (Np != null)
                {
                    if (Np.node.Name.CompareTo(Node.Name) == 0)
                    {
                        id = Np.ident;
                        break;
                    }
                    Np = Np.next;
                }
            }
            return id;
        }

        public long[] checked_Employee()
        {
            long[] empId=null;
            int count = 0;
            NoEmpl Np = null;
            if (NEm != null)
            {
                Np = NEm.first;
                while (Np != null)
                {
                    if (Np.node.Checked)
                        count+=1;
                    Np = Np.next;
                }
                Np = NEm.first;
                int i = 0;
                empId = new long[count];
                while (Np != null)
                {
                    if (Np.node.Checked)
                    {
                        empId[i] = Np.ident;
                        i += 1;
                    }
                    Np = Np.next;
                }

            }
            return empId;
        }

        public void Assign_Employee(long id,  string FIO)
        {
            NoEmpl Np = null;
            if (NEm != null)
            {
                Np = NEm.first;
                while (Np != null)
                {
                    if (Np.ident == id)
                    {
                        Np.fio = FIO;
                        Np.node.Text = Np.degree + " " + Np.fio;
                        Np.node.NodeFont = new Font("Arial", 8);
                        Np.node.ForeColor = Color.DarkViolet;
                        break;
                    }
                    Np = Np.next;
                }
            }
        }

        public void Dessign_Employee(long id)
        {
            NoEmpl Np = null;
            if (NEm != null)
            {
                Np = NEm.first;
                while (Np != null)
                {
                    if (Np.ident == id)
                    {
                        Np.fio = "?";
                        Np.node.Text = Np.degree + " " + Np.fio;
                        Np.node.NodeFont = new Font("Arial", 8);
                        Np.node.ForeColor = Color.DarkViolet;
                        break;
                    }
                    Np = Np.next;
                }
            }
        }

        public TreeNode Find_Department(string Dep, TreeNode LastNode, bool Forward)
        {
            TreeNode FindNode = null;
            Its_Dep d = null;
            if (NDep != null)
                try
                {
                    if (LastNode == null)
                    {
                        d = NDep.first;
                        while (d != null)
                        {
                            if (d.node.Text.ToLower().Contains(Dep.ToLower()))
                            {
                                FindNode = d.node;
                                break;
                            }
                            d = d.next;
                        }
                    }
                    else
                    {
                        if (Forward)
                        {
                            d = NDep.first;
                            while (d != null)
                            {
                                if (d.node.Name.CompareTo(LastNode.Name) == 0)
                                    break;
                                d = d.next;
                            }
                            if (d != null)
                                while (d != null)
                                {
                                    if (d.node.Text.ToLower().Contains(Dep.ToLower()))
                                    {
                                        FindNode = d.node;
                                        break;
                                    }
                                    d = d.next;
                                }

                        }
                        else
                        {
                            d = NDep.first;
                            while (d != null && d.node.Name.CompareTo(LastNode.Name) != 0)
                            {
                                if (d.node.Text.Contains(Dep))
                                    FindNode = d.node;
                                d = d.next;
                            }
                        }
                    }
                }
                catch { FindNode = null; }
            return FindNode;
        }

        public TreeNode Find_Employee(string Emp, TreeNode LastNode, bool Forward)
        {
            TreeNode FindNode = null;
            NoEmpl d = null;
            if (NEm != null)
                try
                {
                    if (LastNode == null)
                    {
                        d = NEm.first;
                        while (d != null)
                        {
                            if (d.node.Text.ToLower().Contains(Emp.ToLower()))
                            {
                                FindNode = d.node;
                                break;
                            }
                            d = d.next;
                        }
                    }
                    else
                    {
                        if (Forward)
                        {
                            d = NEm.first;
                            while (d != null)
                            {
                                if (d.node.Name.CompareTo(LastNode.Name) == 0)
                                    break;
                                d = d.next;
                            }
                            if (d != null)
                                while (d != null)
                                {
                                    if (d.node.Text.ToLower().Contains(Emp.ToLower()))
                                    {
                                        FindNode = d.node;
                                        break;
                                    }
                                    d = d.next;
                                }

                        }
                        else
                        {
                            d = NEm.first;
                            while (d != null && d.node.Name.CompareTo(LastNode.Name) != 0)
                            {
                                if (d.node.Text.ToLower().Contains(Emp.ToLower()))
                                    FindNode = d.node;
                                d = d.next;
                            }
                        }
                    }
                }
                catch { FindNode = null; }
            return FindNode;
        }

        public long Add_dept(string Dept_name)
        {
            long Id = -1;
            long underD = SelDept_ID;
            if (underD > 0)
                Id = AMAS_DBI.AMASCommand.append_DEP(Dept_name, underD);

            if (Id > 0)
            {
                Its_Dep ddd = Parentdept();
                if (ddd != null)
                {
                    Its_Dep adDep = new Its_Dep(ddd);
                    if (ddd.node != null)
                        adDep.node = ddd.node.Nodes.Add("D" + Id.ToString(), Dept_name);
                    else
                        adDep.node = TreeStructure.Nodes.Add("D" + Id.ToString(), Dept_name);
                }
                else
                {
                    TreeNode node = TreeStructure.Nodes.Add("D" + Id.ToString(), Dept_name);

                    Its_Dep adDep = new Its_Dep();
                    adDep.node = node;
                    adDep.ident = Id;
                    adDep.name = Dept_name;

                    NoDep[] nnd = new NoDep[ND.Length + 1];
                    ND.CopyTo(nnd,0);
                    nnd[ND.Length].Ident = Id;
                    nnd[ND.Length].name = Dept_name;
                    nnd[ND.Length].node = node;
                    nnd[ND.Length].NumDep = adDep;
                    ND = nnd;
                }

            }

            return Id;
        }

        private Its_Dep Parentdept()
        {
            Its_Dep dnd = null;
            if (NDep != null)
            {
                Its_Dep n = NDep.first;
                do
                {
                    if (n.node.Name.CompareTo( TreeStructure.SelectedNode.Name)==0)
                    {
                        dnd = n;
                        break;
                    }
                    n = n.next;
                }
                while (n.next != null);
            }
            return dnd;
        }

        public bool enterprise_str()
        {
            ResultMessage = "";
            bool Res = false;
            if (SybAcc.Set_table("TStruct7", AMAS_Query.Class_AMAS_Query.EnterpriceDeps(), null))
            {
                try
                {
                    dep_ments = new string[SybAcc.Rows_count];
                    cods_enter = new long[SybAcc.Rows_count];
                    unders_s = new long[SybAcc.Rows_count];
                    point_s = new int[SybAcc.Rows_count];
                    cur_pttp = new int[SybAcc.Rows_count];
                    my_dep_ments = new string[SybAcc.Rows_count];
                    my_cods_enter = new long[SybAcc.Rows_count];
                    my_unders_s = new long[SybAcc.Rows_count];
                    my_point_s = new int[SybAcc.Rows_count];
                    my_cur_pttp = new int[SybAcc.Rows_count];

                    count_enter_price = SybAcc.Rows_count;

                    for (int i = 0; i < SybAcc.Rows_count; i++)
                    {
                        SybAcc.Get_row(i);
                        dep_ments[i] = (string)SybAcc.Find_Field("name");
                        cods_enter[i] = (long)Convert.ToInt32(SybAcc.Find_Field("department"));
                        try
                        {
                            unders_s[i] = (long)Convert.ToInt32(SybAcc.Find_Field("under"));
                        }
                        catch //(Exception ex) 
                        { 
                            //SybAcc.EBBLP.AddError(ex.Message, "Structure - 2", ex.StackTrace);
                                unders_s[i] = 0;
                        }
                    }
                    Res = true;
                }
                catch { Res = false; }
                SybAcc.ReturnTable();
            }
            return Res;
        }

        private void order_departments(int Order_count_enter_price, long[] Order_unders_s, long[] Order_cods_enter, string[] Order_dep_ments, int[] Order_cur_pttp)
        {
            string tempo_d; long tempo_c; long tempo_u;
            int current_point = 0;
                for (int current_line = 0; current_line < Order_count_enter_price - 1; current_line++)
                {
                    for (int i = current_line + 1; i < Order_count_enter_price; i++)
                    {
                        if (Order_unders_s[i] == Order_cods_enter[current_line]) // && i > current_line + 1)
                        {
                            tempo_d = Order_dep_ments[i];
                            tempo_c = Order_cods_enter[i];
                            tempo_u = Order_unders_s[i];
                            for (int n = i; n > current_line + 1; n--)
                            {
                                Order_dep_ments[n] = Order_dep_ments[n - 1];
                                Order_cods_enter[n] = Order_cods_enter[n - 1];
                                Order_unders_s[n] = Order_unders_s[n - 1];
                            }
                            Order_dep_ments[current_line + 1] = tempo_d;
                            Order_cods_enter[current_line + 1] = tempo_c;
                            Order_unders_s[current_line + 1] = tempo_u;
                        }
                    }
                }
            int maxpt = 0;
            for (int i = 0; i < Order_count_enter_price; i++)
            {
                if (Order_unders_s[i] == 0)
                {
                    current_point = 1;
                    point_s[i] = 0;
                    maxpt = 0;
                }
                else
                {
                    int n;
                    for (n = 0; n < current_point; n++)
                    {
                        if (Order_unders_s[i] == Order_cods_enter[n]) break;
                    }
                    point_s[n] = (int)point_s[i]+1;
                    current_point = i;
                    if (maxpt < n+1) maxpt = n+1;
                }
                Order_cur_pttp[i] = maxpt;
            }
        }

        public void My_DeptStructure()
        {
            my_order_dep_ments();
            draw_shema_own_deps();
        }

        private void my_order_dep_ments()
        {
            string OName;
            int dep_c = 0;
            long[] under_dep_id=null;
            long[] dep_id = null;
            string sql = "select org_department.* from dbo.org_department join dbo.emp_dep_degrees  on org_department.department=emp_dep_degrees.department where emp_dep_degrees.deleted is null and emp_dep_degrees.leader=1 and emp_dep_degrees.employee=dbo.user_ident()";
            if (SybAcc.Set_table("TStruct7.2", sql, null))
            {
                dep_c=SybAcc.Rows_count;
                under_dep_id = new long[dep_c];
                dep_id = new long[dep_c];
                int buf = 0;
                try
               {
                   for (int i = 0; i < dep_c; i++)
                    {
                        SybAcc.Get_row(i);
                        SybAcc.Find_Field("under");
                        OName = SybAcc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0)
                        {
                            buf= (int)SybAcc.get_current_Field();
                            under_dep_id[i] = (long)buf;
                        }
                        else
                            under_dep_id[i] = 0;
                        buf= (int)SybAcc.Find_Field("department");
                        dep_id[i] = (long)buf;
                    }
                }
                catch (Exception err) 
                {
                    SybAcc.EBBLP.AddError(err.Message, "Structure - 18", err.StackTrace);
                }

                SybAcc.ReturnTable();
            }

            if (under_dep_id.Length > 0)
            {
                MyDepsIdent = new ArrayList();
                DrowMyShema(under_dep_id, dep_id, 0,-1);

                my_count_enter_price = MyDepsIdent.Count;
                my_dep_ments = new string[my_count_enter_price];
                for (int i = 0; i < my_count_enter_price; i++)
                {
                    my_dep_ments[i] = dep_ments[(int)MyDepsIdent[i]];
                }
                my_cods_enter = new long[my_count_enter_price];
                for (int i = 0; i < my_count_enter_price; i++)
                {
                    my_cods_enter[i] = cods_enter[(int)MyDepsIdent[i]];
                }
                my_unders_s = new long[my_count_enter_price];
                for (int i = 0; i < my_count_enter_price; i++)
                {
                    my_unders_s[i] = unders_s[(int)MyDepsIdent[i]];
                }
            }
        }

        ArrayList MyDepsIdent = null;
        private int DrowMyShema(long[] under_dep_id, long[] dep_id, int current_line, long under)
        {
            for (Int32 i = current_line; i < count_enter_price; i++)
            {
                for (int j = 0; j < dep_id.Length; j++)
                {
                    if (cods_enter[i] == dep_id[j])
                    {
                        MyDepsIdent.Add(i);
                        current_line = i;
                        break;
                    }
                    else if (unders_s[i] == dep_id[j])
                    {
                        long[] di = new long[1];
                        di[0] = cods_enter[i];
                        long[] udi = new long[1];
                        udi[0] = unders_s[i];
                        current_line = DrowMyShema(udi, di, i, cods_enter[i]);
                        i = current_line;
                        break;
                    }
                }
            }
            return current_line;
        }

        private void draw_shema_own_deps()
        {
            Draw_Shema(DrawBand.subdepts);
        }

        private bool loop_underpl(long parent_dep, long child_dep)
        {
            int i = -1;
            bool lookun = false;
            do
            {
                i++;
            }
            while (cods_enter[i] != parent_dep && i < count_enter_price);

                    i++;
            if (i >= count_enter_price - 1)
            {

                lookun = false;
            }
            else
            {
                int lvl = cur_pttp[i];
                do
                {
                    if (cur_pttp[i] < lvl || i >= count_enter_price)
                    {
                        lookun = false;
                        break;
                    }
                    else lookun = true;
                    i++;
                }
                while (i < count_enter_price && cods_enter[i] != child_dep);
            }
            return lookun;
        }

        public struct NoDepWebS { public string name; public long Ident; public Its_Dep_WebS NumDep;}
        public NoDepWebS[] NDWebS;
        private Its_Dep_WebS NDepWebS = null;

        private struct NoDep { public string name; public System.Windows.Forms.TreeNode node; public long Ident; public Its_Dep NumDep;}
        private NoDep[] ND;
        private Its_Dep NDep = null;

        private class Its_Dep
        {
            public string name="";
            public System.Windows.Forms.TreeNode node;
            public Its_Dep first;
            public Its_Dep next;
            public long ident;

            public Its_Dep()
            {
                first = this;
                next = null;
            }

            public Its_Dep(Its_Dep D)
            {
                first = D.first;
                next = D.next;
                D.next = this;
                //D = this;
            }
        }

        public class Its_Dep_WebS
        {
            public string name = "";
            public Its_Dep_WebS first;
            public Its_Dep_WebS next;
            public long ident;

            public Its_Dep_WebS()
            {
                first = this;
                next = null;
            }

            public Its_Dep_WebS(Its_Dep_WebS D)
            {
                first = D.first;
                next = D.next;
                D.next = this;
                //D = this;
            }
        }

        public long SelDept_ID
        {
            get 
            {
                long Ident = -1;
                if (TreeStructure.SelectedNode != null)
                    if (NDep != null)
                    {
                        Its_Dep n = NDep.first;
                        while (n != null)
                        {
                            if (n.node.Name.CompareTo(TreeStructure.SelectedNode.Name) == 0)
                            {
                                Ident = n.ident;
                                break;
                            }
                            n = n.next;
                        }
                    }
                    else Ident = -1;
                else Ident = -1;
                return Ident; 
            } 
        }

        private long SelDept_by_Node(System.Windows.Forms.TreeNode NoD)
        {
            long Ident = -1;
            //if (NDep != null)
            //{
            //    Its_Dep n = NDep.first;
            //    while (n != null)
            //    {
            //        if (n.node.Name.CompareTo( NoD.Name)==0)
            //        {
            //            Ident = n.ident;
            //            break;
            //        }
            //        n = n.next;
            //    }
            //}
            if (ND != null)
                foreach (NoDep Ndp in ND)
                    if (Ndp.node.Name.CompareTo(NoD.Name) == 0)
                    {
                        Ident = Ndp.Ident;
                        break;
            }

                return Ident;
        }

        private class NoEmpl 
        { 
            public string name;
            public string degree = "";
            public string fio = "";
            public System.Windows.Forms.TreeNode node; 
            public NoEmpl first; 
            public NoEmpl next;
            public int ident;
            public int Employee;

            public NoEmpl()
            {
                first = this;
                next = null;
            }
            public NoEmpl(NoEmpl f)
            {
                first = f.first;
                f.next = this;
                next = null;
                f = this;
            }
        }
        private NoEmpl NEm = null;

        private class NoEmplWebS
        {
            public string name;
            public string degree = "";
            public string fio = "";
            public NoEmplWebS first;
            public NoEmplWebS next;
            public int ident;
            public int Employee;

            public NoEmplWebS()
            {
                first = this;
                next = null;
            }
            public NoEmplWebS(NoEmplWebS f)
            {
                first = f.first;
                f.next = this;
                next = null;
                f = this;
            }
        }
        private NoEmplWebS NEmWebS = null;

        private enum DrawBand {enterprise = 1, subdepts=2 }

        public void draw_shema_enterprice()
        {
            Draw_Shema(DrawBand.enterprise);
        }

        public string[] draw_WebSshema_enterprice()
        {
            string[] Deps; 
            NoDepWebS[] NDW= Draw_Shema_WebService(DrawBand.enterprise);
            Deps = new string[NDW.Length];
            for (int i=0;i<NDW.Length;i++)
                Deps[i]=NDW[i].name;
            return Deps;
        }

        private void Draw_Shema(DrawBand EntORdep)
        {
            int countEnDep;
            long[] cods_enterOrdep;
            long[] unders_sEntOrDep;
            string[] entORdep_ments;
            switch (EntORdep)
            {
                case DrawBand.enterprise:
                    countEnDep = count_enter_price;
                    cods_enterOrdep = cods_enter;
                    unders_sEntOrDep = unders_s;
                    entORdep_ments = dep_ments;
                    break;
                case DrawBand.subdepts:
                    countEnDep = my_count_enter_price;
                    cods_enterOrdep = my_cods_enter;
                    unders_sEntOrDep = my_unders_s;
                    entORdep_ments = my_dep_ments;
                    break;
                default:
                    countEnDep = count_enter_price;
                    cods_enterOrdep = cods_enter;
                    unders_sEntOrDep = unders_s;
                    entORdep_ments = dep_ments;
                    break;
            }
            ND = new NoDep[countEnDep];
            try
            {
                TreeStructure.Nodes.Clear();
                for (int i = 0; i < countEnDep; i++)
                {
                   bool finden = false;
                   ND[i].name = "D" + (string)Convert.ToString(cods_enterOrdep[i]);
                   //if (unders_sEntOrDep[i] > 0)
                       
                    for (int n = i - 1; n >= 0; n--)
                    {
                        if (ND[n].name.CompareTo("D" + (string)Convert.ToString(unders_sEntOrDep[i])) == 0)
                        {
                            ND[i].node = ND[n].node.Nodes.Add(ND[i].name, entORdep_ments[i], Icon_dept);
                            finden = true;
                            break;
                        }
                    }
                   //else finden = true;
                    if (!finden)
                    {
                        if (unders_sEntOrDep[i]>0) AMAS_DBI.AMASCommand.reOrder_DEP(0, cods_enterOrdep[i]);
                        ND[i].node = TreeStructure.Nodes.Add(ND[i].name, entORdep_ments[i], Icon_dept);
                    }
                    if (NDep == null)
                    {
                        NDep = new Its_Dep();
                    }
                    else
                    {
                        NDep = new Its_Dep(NDep);
                    }
                    NDep.node = ND[i].node;
                    NDep.name = ND[i].name;
                    NDep.ident = cods_enterOrdep[i];
                    NDep.node.NodeFont=new Font("Arial",10);
                    ND[i].NumDep = NDep;
                    ND[i].Ident = cods_enterOrdep[i];
                    string nnn;
                    if (Show_Empl)
                    {
                        int leader = 0;
                        string fio = "";
                        if (SybAcc.Set_table("TStruct8", AMAS_Query.Class_AMAS_Query.Degrees_in_Dep(NDep.ident), null))
                        {
                            try
                            {
                                for (int l = 0; l < SybAcc.Rows_count; l++)
                                {
                                    SybAcc.Get_row(l);
                                    nnn = "E" + (string)Convert.ToString(SybAcc.Find_Field("cod"));
                                    leader = (int)SybAcc.Find_Field("leader");
                                    fio = (string)SybAcc.Find_Field("fio");
                                    {
                                        if (NEm == null)
                                        {
                                            NEm = new NoEmpl();
                                        }
                                        else
                                        {
                                            NEm = new NoEmpl(NEm);
                                        }
                                        NEm.degree = (string)SybAcc.Find_Field("name");
                                        NEm.fio = fio;
                                        NEm.node = ND[i].node.Nodes.Add(nnn, NEm.degree + " " + fio, IkonEmpl.NumIkon(fio, leader, Check_Ikon));
                                        NEm.name = nnn;
                                        NEm.ident = (int)SybAcc.Find_Field("cod");
                                        NEm.node.NodeFont = new Font("Arial", 8);
                                        NEm.node.ForeColor = Color.DarkViolet;
                                        try
                                        {
                                            NEm.Employee = (int)SybAcc.Find_Field("employee");
                                            
                                        }
                                        catch { NEm.Employee = 0; }
                                        try
                                        {
                                            NEm.Employee = (int)SybAcc.Find_Field("employee");
                                        }
                                        catch { NEm.Employee = 0; }
                                    }
                                }
                            }
                            catch (Exception e) 
                            {
                                SybAcc.EBBLP.AddError(e.Message, "Structure - 3", e.StackTrace);
                            }
                            SybAcc.ReturnTable();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SybAcc.EBBLP.AddError(e.Message, "Structure - 4", e.StackTrace);
            }
        }

        private NoDepWebS[] Draw_Shema_WebService(DrawBand EntORdep)
        {
            int countEnDep;
            long[] cods_enterOrdep;
            long[] unders_sEntOrDep;
            string[] entORdep_ments;
            switch (EntORdep)
            {
                case DrawBand.enterprise:
                    countEnDep = count_enter_price;
                    cods_enterOrdep = cods_enter;
                    unders_sEntOrDep = unders_s;
                    entORdep_ments = dep_ments;
                    break;
                case DrawBand.subdepts:
                    countEnDep = my_count_enter_price;
                    cods_enterOrdep = my_cods_enter;
                    unders_sEntOrDep = my_unders_s;
                    entORdep_ments = my_dep_ments;
                    break;
                default:
                    countEnDep = count_enter_price;
                    cods_enterOrdep = cods_enter;
                    unders_sEntOrDep = unders_s;
                    entORdep_ments = dep_ments;
                    break;
            }

                NDWebS= new NoDepWebS[countEnDep];
            try
            {
                for (int i = 0; i < countEnDep; i++)
                {
                    bool finden = false;
                    NDWebS[i].name = "D" + (string)Convert.ToString(cods_enterOrdep[i]);
                    //if (unders_sEntOrDep[i] > 0)

                    for (int n = i - 1; n >= 0; n--)
                    {
                        if (NDWebS[n].name.CompareTo("D" + (string)Convert.ToString(unders_sEntOrDep[i])) == 0)
                        {
                            finden = true;
                            break;
                        }
                    }
                    //else finden = true;
                    if (!finden)
                    {
                        if (unders_sEntOrDep[i] > 0) AMAS_DBI.AMASCommand.reOrder_DEP(0, cods_enterOrdep[i]);
                    }
                    if (NDepWebS == null)
                    {
                        NDepWebS =new Its_Dep_WebS();
                    }
                    else
                    {
                        NDepWebS = new Its_Dep_WebS(NDepWebS);
                    }
                    NDepWebS.name = ND[i].name;
                    NDepWebS.ident = cods_enterOrdep[i];
                    NDWebS[i].NumDep = NDepWebS;
                    NDWebS[i].Ident = cods_enterOrdep[i];
                    string nnn;
                    if (Show_Empl)
                    {
                        int leader = 0;
                        string fio = "";
                        if (SybAcc.Set_table("TStruct8", AMAS_Query.Class_AMAS_Query.Degrees_in_Dep(NDep.ident), null))
                        {
                            try
                            {
                                for (int l = 0; l < SybAcc.Rows_count; l++)
                                {
                                    SybAcc.Get_row(l);
                                    nnn = "E" + (string)Convert.ToString(SybAcc.Find_Field("cod"));
                                    leader = (int)SybAcc.Find_Field("leader");
                                    fio = (string)SybAcc.Find_Field("fio");
                                    {
                                        if (NEmWebS == null)
                                        {
                                            NEmWebS = new NoEmplWebS();
                                        }
                                        else
                                        {
                                            NEmWebS = new NoEmplWebS(NEmWebS);
                                        }
                                        NEmWebS.degree = (string)SybAcc.Find_Field("name");
                                        NEmWebS.fio = fio;
                                        NEm.name = nnn;
                                        NEmWebS.ident = (int)SybAcc.Find_Field("cod");
                                        try
                                        {
                                            NEmWebS.Employee = (int)SybAcc.Find_Field("employee");

                                        }
                                        catch { NEmWebS.Employee = 0; }
                                        try
                                        {
                                            NEmWebS.Employee = (int)SybAcc.Find_Field("employee");
                                        }
                                        catch { NEmWebS.Employee = 0; }
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                SybAcc.EBBLP.AddError(e.Message, "Structure - Web3", e.StackTrace);
                            }
                            SybAcc.ReturnTable();
                        }
                    }
                }
            }
            catch (Exception e)
            {
                SybAcc.EBBLP.AddError(e.Message, "Structure - Web4", e.StackTrace);
            }
            return NDWebS;
        }

        private bool under_dep(long department)
        {
            bool bool_dep = false;
            if (SybAcc.GetRights.Registrator)
            {
                bool_dep = true;
            }
            else
            {
                if (my_count_enter_price < 1) my_order_dep_ments();
                for (int i = 0; i < my_count_enter_price; i++)
                    if (department == my_cods_enter[i])
                    {
                        bool_dep = true;
                        break;
                    }
            }
            return bool_dep;
        }

        public bool Check_main_exe = false;
        public string ResultMessage="";

        public int[] push_viza(string tiksDate, long Doc_ID, long moving)
        {
            DateTime dat_exe;
            ResultMessage = "";
            TreeNode rankNode;
            int[] mmm = null;
            vizaSended.Clear();

            try
            {
                dat_exe = CalcDate(tiksDate);

                if (Check_main_exe)
                    if (SybAcc.Set_table("TStruct1", AMAS_Query.Class_AMAS_Query.Structure_Is_Moving(Doc_ID), null))
                    {
                        try
                        {
                            if ((int)SybAcc.Find_Field("cnt") == 0) Check_main_exe = true;
                        }
                        catch  (Exception e)
                        {
                            SybAcc.EBBLP.AddError(e.Message, "Structure - 6", e.StackTrace);
                            Check_main_exe = false; 
                        }
                        SybAcc.ReturnTable();
                    }

                if (dat_exe >= DateTime.Now)
                {
                    foreach (NoDep rankindep in ND) 
                    {
                        rankNode = rankindep.node;
                        if (rankNode.Checked == true)
                        {
                            if ((under_dep((long)Convert.ToInt32(rankNode.Name.Substring(1))) == true) ||
                                (under_group((int)Convert.ToInt32(rankNode.Name.Substring(1)), 0, 2) > 0))
                            {
                                mmm = resize_array(mmm, AMASCommand.Document_Vizing(Doc_ID, (int)Convert.ToInt32(rankNode.Name.Substring(1)), 0, dat_exe, moving, 0));
                                vizaSended.Add(rankNode.Text.Trim());
                            }
                            else
                                SybAcc.EBBLP.AddError("Попытка получения визы от подчинённого отдела", "Structure - 7", "");
                        }

                    }
                    NoEmpl curempl=NEm.first;
                    while (curempl!=null)
                    {
                        if (curempl.node.Checked == true)
                        {
                            if ((under_dep((long)Convert.ToInt64(curempl.node.Parent.Name.Substring(1))) == true)
                                || (under_group(0, (int)Convert.ToInt64(curempl.node.Name.Substring(1, curempl.node.Name.Length - 1)), 2) > 0))
                            {
                                mmm = resize_array(mmm, AMASCommand.Document_Vizing(Doc_ID, 0, (int)Convert.ToInt32(curempl.node.Name.Substring(1)), dat_exe, moving, 0));
                                vizaSended.Add(curempl.node.Text.Trim());
                            }
                            else
                                SybAcc.EBBLP.AddError("Попытка получения визы от подчинённого сотрудника", "Structure - 7", "");
                        }
                        curempl = curempl.next;
                    }
                    foreach (int empl in Groups.getSelectedExeVizUsers())
                        if ((under_dep((long)empl) == true)
                            || (under_group(0, empl, 2) > 0))
                        {
                            mmm = resize_array(mmm, AMASCommand.Document_Vizing(Doc_ID, 0, empl, dat_exe, moving, 0));
                            vizaSended.Add(Groups.getUserNamebyCod(empl));
                        }
                        else
                            SybAcc.EBBLP.AddError("Попытка дать поручение не подчинённому сотруднику", "Structure - 7", "");
                }

            }
            catch (Exception e)
            {
                SybAcc.EBBLP.AddError(e.Message, "Structure - 7.V", e.StackTrace);
            }
            return mmm;
        }

        private int[] resize_array(int[] moving, int m)
        {
            int[] mov;
            int cnt = 0;
            if (moving == null)
                cnt = 1;
            else
                cnt = moving.Length + 1;
            mov = new int[cnt];
            for (int i = 0; i < cnt - 1; i++) mov[i] = moving[i];
            mov[cnt - 1] = m;
            return mov;
        }

        public int[] push_letter(bool is_main_executor, string tiksDate, long Doc_ID, string signature, long moving)
        {
            DateTime dat_exe;
            ResultMessage = "";
            TreeNode rankNode;
            int[] mmm = null;
            taskSended.Clear();
            try
            {
                dat_exe = CalcDate(tiksDate);

                if (Check_main_exe)
                    if (SybAcc.Set_table("TStruct1", AMAS_Query.Class_AMAS_Query.Structure_Is_Moving(Doc_ID), null))
                    {
                        try
                        {
                            if ((int)SybAcc.Find_Field("cnt") == 0) Check_main_exe = true;
                        }
                        catch (Exception e)
                        {
                            SybAcc.EBBLP.AddError(e.Message, "Structure - 6", e.StackTrace);
                            Check_main_exe = false;
                        }
                        SybAcc.ReturnTable();
                    }

                if (dat_exe >= DateTime.Now)
                {
                    foreach (NoDep rankindep in ND)
                    {
                        rankNode = rankindep.node;
                        if (rankNode.Checked == true)
                        {
                            if ((under_dep((long)Convert.ToInt32(rankNode.Name.Substring(1))) == true) ||
                                (under_group((int)Convert.ToInt32(rankNode.Name.Substring(1)), 0, 1) > 0))
                            {
                                mmm = resize_array(mmm, AMASCommand.Document_moving(Check_main_exe, Doc_ID, (int)Convert.ToInt32(rankNode.Name.Substring(1)), 0, dat_exe, signature, moving, 0));
                                taskSended.Add(rankNode.Text.Trim());
                            }
                            else
                                SybAcc.EBBLP.AddError("Попытка дать поручение не подчинённому отделу", "Structure - 7", "");
                        }
                    }
                    NoEmpl curempl = NEm.first;
                    while (curempl != null)
                    {
                        if (curempl.node.Checked == true)
                        {
                            if ((under_dep((long)Convert.ToInt64(curempl.node.Parent.Name.Substring(1))) == true)
                                || (under_group(0, (int)Convert.ToInt64(curempl.node.Name.Substring(1, curempl.node.Name.Length - 1)), 1) > 0))
                            {
                                mmm = resize_array(mmm, AMASCommand.Document_moving(Check_main_exe, Doc_ID, 0, (int)Convert.ToInt32(curempl.node.Name.Substring(1)), dat_exe, signature, moving, 0));
                                taskSended.Add(curempl.node.Text.Trim());
                            }
                            else
                                SybAcc.EBBLP.AddError("Попытка дать поручение не подчинённому сотруднику", "Structure - 7", "");
                        }
                        curempl = curempl.next;
                    }
                    if (Groups != null)
                    {
                        foreach (int grp in Groups.getSelectedExeVizGroups(true))
                        {
                        }

                        foreach (int empl in Groups.getSelectedExeVizUsers())
                            if ((under_dep((long)empl) == true)
                                || (under_group(0, empl, 1) > 0))
                            {
                                mmm = resize_array(mmm, AMASCommand.Document_moving(Check_main_exe, Doc_ID, 0, empl, dat_exe, signature, moving, 0));
                                taskSended.Add(Groups.getUserNamebyCod(empl));
                            }
                            else
                                SybAcc.EBBLP.AddError("Попытка дать поручение не подчинённому сотруднику", "Structure - 7", "");
                    }
                }
            }
            catch (Exception e)
            {
                SybAcc.EBBLP.AddError(e.Message, "Structure - 7", e.StackTrace);
            }
            
            return mmm;
        }

        public int[] push_new( string tiksDate, long Doc_ID)
        {
            DateTime dat_exe;
            ResultMessage = "";
            TreeNode rankNode;
            int[] mmm = null;
            newsSended.Clear();
            try
            {
                dat_exe = CalcDate(tiksDate);

                if (dat_exe >= DateTime.Now)
                {

                    foreach (NoDep rankindep in ND)
                    {
                        rankNode = rankindep.node;
                        if (rankNode.Checked == true)
                        {
                            mmm = resize_array(mmm, AMASCommand.Document_Newing((int) Doc_ID, dat_exe, (int)Convert.ToInt32(rankNode.Name.Substring(1)), 0));
                            newsSended.Add(rankNode.Text.Trim());
                        }
                    }
                    
                    NoEmpl curempl = NEm.first;
                    while (curempl != null)
                    {
                        if (curempl.node.Checked == true)
                        {
                            mmm = resize_array(mmm, AMASCommand.Document_Newing((int)Doc_ID, dat_exe, 0, (int)Convert.ToInt32(curempl.node.Name.Substring(1))));
                            newsSended.Add(curempl.node.Text.Trim());
                        }
                        curempl = curempl.next;
                    }
                }
            }
            catch (Exception e)
            {
                SybAcc.EBBLP.AddError(e.Message, "Structure - 7.7", e.StackTrace);
            }
            return mmm;
        }

        private DateTime CalcDate(string ddt)
        {
            string swt;
            int tiks;
            DateTime At = new DateTime();
            At = DateTime.Now;
            int blank;
            if (ddt.Split('.').Length > 2)
            {
                At = Convert.ToDateTime(ddt);
            }
            else
            {
                for (blank = 0; blank < ddt.Length; blank++)
                {
                    if (ddt.Substring(blank, 1).CompareTo(" ") == 0) break;
                }
                try
                {
                    tiks = (int)Convert.ToInt16(ddt.Substring(0, blank));
                }
                catch { tiks = 1; }
                try
                {
                    swt = ddt.Substring(blank).Trim().Substring(0, 1);
                }
                catch { swt = "д"; }
                switch (swt.ToLower())
                {
                    case "д":
                        At = DateTime.Now.AddDays((double)Convert.ToDouble(tiks));
                        break;
                    case "н":
                        At = DateTime.Now.AddDays((double)Convert.ToDouble(tiks * 7));
                        break;
                    case "м":
                        At = DateTime.Now.AddMonths(tiks);
                        break;
                    case "г":
                        At = DateTime.Now.AddMonths(tiks * 12);
                        break;
                }
            }
            return At;
        }

        public int under_group(int department, int member,int lay)
        {
            int rank=0;
            string sql;
            if (member < 1)
                 sql = "select rank from dbo.Emp_My_Groups where rank in ( select cod from dbo.emp_dep_degrees where emp_dep_degrees.deleted is null and leader=1 and department=" + (string)Convert.ToString(department) + ")";
            else
                 sql = "select rank from dbo.Emp_My_Groups where rank =" + member.ToString();
                if (lay == 1)
                    sql += " and executing=1";
                else
                    sql += " and vizing=1";
                if (SybAcc.Set_table("TStruct2", sql, null))
                {
                    try
                    {
                        if (SybAcc.Rows_count > 0)
                        {
                            SybAcc.Get_row(0);
                            rank = (int)SybAcc.Find_Field("rank");
                        }
                        else rank = 0;
                    }
                    catch (Exception e)
                    {
                        SybAcc.EBBLP.AddError(e.Message, "Structure - 9", e.StackTrace);
                    }
                    SybAcc.ReturnTable();
                }

            //if ((rank <1 ) && (member > 0))
            //{
            //    rank = member;
            //}

            //if (rank > 0)
            //{
            //    if (AMAS_DBI.AMASCommand.Assign_to_Dep(rank, lay) == false) rank = -1; 
            //}
            return rank;
        }

        //drag & drop

        private Point screenOffset;

        private System.Windows.Forms.TreeNode fromNode = null;
        private System.Windows.Forms.TreeNode EditNode = null;
        private System.Windows.Forms.TreeNode targetNode = null;
        
        private System.Windows.Forms.TreeNode usedfromNode = null;
        private System.Windows.Forms.TreeNode usedtoNode = null;

        private void allow_dragDrop()
        {
            TreeStructure.AllowDrop = true;
            TreeStructure.MouseDown+=new MouseEventHandler(TreeStructure_MouseDown);
            TreeStructure.MouseMove+=new MouseEventHandler(TreeStructure_MouseMove);
            TreeStructure.MouseUp+=new MouseEventHandler(TreeStructure_MouseUp);
            TreeStructure.DragDrop+=new DragEventHandler(TreeStructure_DragDrop);
            TreeStructure.DragOver+=new DragEventHandler(TreeStructure_DragOver);
            TreeStructure.DragEnter+=new DragEventHandler(TreeStructure_DragEnter);
            TreeStructure.DragLeave+=new EventHandler(TreeStructure_DragLeave);
            TreeStructure.QueryContinueDrag+=new QueryContinueDragEventHandler(TreeStructure_QueryContinueDrag);
            TreeStructure.GiveFeedback+=new GiveFeedbackEventHandler(TreeStructure_GiveFeedback);
            TreeStructure.ItemDrag+=new ItemDragEventHandler(TreeStructure_ItemDrag);
            TreeStructure.DoubleClick+=new EventHandler(TreeStructure_DoubleClick);
            TreeStructure.BeforeLabelEdit += new NodeLabelEditEventHandler(TreeStructure_BeforeLabelEdit);
            TreeStructure.AfterLabelEdit += new NodeLabelEditEventHandler(TreeStructure_AfterLabelEdit);
            Drag_droping = true;
        }

        private void disallow_dragDrop()
        {
            TreeStructure.MouseDown -= new MouseEventHandler(TreeStructure_MouseDown);
            TreeStructure.MouseMove -= new MouseEventHandler(TreeStructure_MouseMove);
            TreeStructure.MouseUp -= new MouseEventHandler(TreeStructure_MouseUp);
            TreeStructure.DragDrop -= new DragEventHandler(TreeStructure_DragDrop);
            TreeStructure.DragOver -= new DragEventHandler(TreeStructure_DragOver);
            TreeStructure.DragEnter -= new DragEventHandler(TreeStructure_DragEnter);
            TreeStructure.DragLeave -= new EventHandler(TreeStructure_DragLeave);
            TreeStructure.QueryContinueDrag -= new QueryContinueDragEventHandler(TreeStructure_QueryContinueDrag);
            TreeStructure.GiveFeedback -= new GiveFeedbackEventHandler(TreeStructure_GiveFeedback);
            TreeStructure.ItemDrag -= new ItemDragEventHandler(TreeStructure_ItemDrag);
            TreeStructure.DoubleClick -= new EventHandler(TreeStructure_DoubleClick);
            TreeStructure.BeforeLabelEdit -= new NodeLabelEditEventHandler(TreeStructure_BeforeLabelEdit);
            TreeStructure.AfterLabelEdit -= new NodeLabelEditEventHandler(TreeStructure_AfterLabelEdit);
            TreeStructure.AllowDrop = false;
            Drag_droping = false;
        }


        private long ORGID = -1;

        private void TreeStructure_BeforeLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            EditNode = TreeStructure.SelectedNode;
            ORGID = SelDept_ID;
        }

        private void TreeStructure_AfterLabelEdit(object sender, System.Windows.Forms.NodeLabelEditEventArgs e)
        {
            AMAS_DBI.AMASCommand.rename_DEP(e.Label, SelDept_by_Node(EditNode));
        }

        private void TreeStructure_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                //TreeStructure.SelectedNode.BeginEdit();
            }
            catch (Exception err)
            {
                SybAcc.EBBLP.AddError(err.Message, "Structure - 10", err.StackTrace);
            }
        }

        private void TreeStructure_MouseDown(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            //if (targetNode != null)
            //    targetNode.ForeColor = targetColor; //TreeStructure.ForeColor;
            if (TreeStructure.SelectedNode!=null)
            {
                fromNode = TreeStructure.SelectedNode;
                fromColor = fromNode.ForeColor;
                targetNode = fromNode;
                targetColor = targetNode.ForeColor;
                fromNode.ForeColor = Color.DarkGreen;
                //fromNode.ForeColor = fromColor; //TreeStructure.ForeColor;
            }
                //if (fromNode != null)
        }

        private void TreeStructure_MouseUp(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if (fromNode != null)
                fromNode.ForeColor = TreeStructure.ForeColor; //fromColor; 
        }

        private void TreeStructure_MouseMove(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left)
            {
            screenOffset = TreeStructure.Location;
            }
        }

        private void TreeStructure_ItemDrag(object sender, ItemDragEventArgs e)
        {
            TreeNode nnn = (TreeNode)e.Item;
            if(fromNode!=null)
                if(fromNode.Name.CompareTo(nnn.Name)==0)
                    if (e.Button == MouseButtons.Left)
                    {
                        TreeStructure.DoDragDrop(e.Item, DragDropEffects.Move);
                    }
        }

        private void TreeStructure_DragDrop(object sender, System.Windows.Forms.DragEventArgs e)
        {
            Point targetPoint = TreeStructure.PointToClient(new Point(e.X, e.Y));
            targetNode = TreeStructure.GetNodeAt(targetPoint);
            targetNode.ForeColor = Color.Orange;
            if (fromNode != null)
                if (!fromNode.Equals(targetNode))
                    if (e.Effect == DragDropEffects.Move)
                        reorderdepts();
            e.Effect = DragDropEffects.None;
            fromNode.ForeColor = fromColor;
            fromNode = null;
            targetNode.ForeColor = targetColor;
            targetNode = null;
        }

        private void reorderdepts()
        {
            try
            {
                bool more = false;
                if (usedfromNode == null) more = true;
                else if (fromNode.Name.CompareTo(usedfromNode.Name) != 0) more = true;
                if (usedtoNode == null) more = true;
                else if(targetNode.Name.CompareTo(usedtoNode.Name) != 0) more=true;
                if (more)
                {
                    usedfromNode = fromNode;
                    usedtoNode = targetNode;
                    if (MessageBox.Show(fromNode.Text + " подчинить " + targetNode.Text, "Переназначение", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        foreach (System.Windows.Forms.TreeNode n in fromNode.Nodes)
                        {
                            if (n.Name.CompareTo(targetNode.Name) == 0)
                            {
                                foreach (System.Windows.Forms.TreeNode m in fromNode.Nodes)
                                {
                                    if (m.Parent.Name.CompareTo(fromNode.Name) == 0)
                                        if (fromNode.Parent != null)
                                        {
                                            if (AMAS_DBI.AMASCommand.reOrder_DEP(SelDept_by_Node(fromNode.Parent), SelDept_by_Node(m)))
                                            {
                                                m.Remove();
                                                fromNode.Parent.Nodes.Add(m);
                                            }
                                        }
                                        else
                                        {
                                            if (AMAS_DBI.AMASCommand.reOrder_DEP(-1, SelDept_by_Node(m)))
                                            {
                                                m.Remove();
                                                TreeStructure.Nodes.Add(m);
                                            }
                                        }
                                }
                                break;
                            }
                        }
                        if (AMAS_DBI.AMASCommand.reOrder_DEP(SelDept_by_Node(targetNode), SelDept_by_Node(fromNode)))
                        {
                            fromNode.Remove();
                            targetNode.Nodes.Add(fromNode);
                        }
                        TreeStructure.EndUpdate();
                        targetNode.Expand();
                    }
                }
            }
            catch (Exception err) 
            { 
                SybAcc.EBBLP.AddError(err.Message, "Structure - 11", err.StackTrace);
            }
        }

        private void TreeStructure_DragOver(object sender, System.Windows.Forms.DragEventArgs e)
        {
            if (fromNode==null)
            {

                e.Effect = DragDropEffects.None;
            }
            else
            {
                e.Effect = DragDropEffects.Move;
            }
        }

        private void TreeStructure_DragEnter(object sender, System.Windows.Forms.DragEventArgs e)
        {
            e.Effect = e.AllowedEffect;
        }

        private void TreeStructure_DragLeave(object sender, System.EventArgs e)
        {
            //MessageBox.Show(fromNode.Text + " подчинить " + targetNode.Text);
        }

        private void TreeStructure_QueryContinueDrag(object sender, System.Windows.Forms.QueryContinueDragEventArgs e)
        {
            System.Windows.Forms.TreeView tb = sender as System.Windows.Forms.TreeView;
            if (tb != null)
            {
                int MX = Control.MousePosition.X;
                int MY = Control.MousePosition.Y;
                Form f = tb.FindForm();
                int Yshift = 0;
                Control Ctb=tb;
                while (Ctb.Parent!=null)
                {
                    Yshift += Ctb.Location.Y;
                    Ctb = Ctb.Parent;
                }
                if (((MX - screenOffset.X) < f.DesktopBounds.Left + tb.Left) ||
                    ((MX - screenOffset.X) > f.DesktopBounds.Left + tb.Left + tb.Width) ||
                    ((MY - screenOffset.Y) < f.DesktopBounds.Top + Yshift + tb.Top) ||
                    ((MY - screenOffset.Y) > f.DesktopBounds.Top + Yshift + tb.Top + tb.Height))
                {
                    e.Action = DragAction.Cancel;
                }
            }
        }

        private void TreeStructure_GiveFeedback(object sender, System.Windows.Forms.GiveFeedbackEventArgs e)
        {
        }

        public void delete_organization()
        {
            long sid = 0;
            TreeNode delNode = TreeStructure.SelectedNode;
            if (fromNode != null) sid = SelDept_by_Node(delNode);
            if(sid>0)
                if (MessageBox.Show("Удалить " + delNode.Text, "Удаление", MessageBoxButtons.YesNo) == DialogResult.Yes)
                if (AMAS_DBI.AMASCommand.delete_DEP(sid))
                {
                    TreeNode parent = delNode.Parent;
                    foreach (TreeNode n in delNode.Nodes)
                    {
                        if (n.Parent.Name.CompareTo( delNode.Name)==0)
                        {
                            n.Remove();
                            if (parent != null)
                                parent.Nodes.Add(n);
                            else TreeStructure.Nodes.Add(n);
                        }
                    }
                    try
                    {
                        if (NDep != null)
                        {
                            Its_Dep first = NDep.first;
                            if (first.node == delNode)
                            {
                                Its_Dep n = first.next;
                                if(first.next==null) NDep=null;
                                while (first.next != null)
                                {
                                    first.next.first = n;
                                    first = first.next;
                                }
                            }
                            else
                                while (first.next != null)
                                {
                                    if (first.next.node == delNode)
                                    {
                                        first = first.next.next;
                                        break;
                                    }
                                    first = first.next;
                                }
                        }
                    }
                    catch(Exception e)
                    {
                        SybAcc.EBBLP.AddError(e.Message, "Structure - 12", e.StackTrace);
                    }
                    delNode.Remove();
                }
        }
    }


    public class Groups
    {
        private int groups_count=0;
        public TreeView GroupsTree=null;
        private AMAS_DBI.Class_syb_acc SybAcc;
        
        public bool Check_Ikon = true;
        public bool Show_Empl=true;

        private bool ChTree = false;
        public bool CheckTree
        {
            get { return ChTree; }
            set 
            {
                if (GroupsTree != null)
                {
                    if (value)
                        GroupsTree.CheckBoxes = true;
                    else GroupsTree.CheckBoxes = false;
                    GroupsTree.Refresh();
                    ChTree = value;
                }
                else ChTree = false;
            }
        }

        public Groups(AMAS_DBI.Class_syb_acc Syb,TreeView tree,bool MyGroups)
        {
            SybAcc = Syb;
            GroupsTree=tree;
            draw_schema(MyGroups);
            GroupsTree.AfterSelect+=new TreeViewEventHandler(GroupsTree_AfterSelect);
            GroupsTree.AfterLabelEdit+=new NodeLabelEditEventHandler(GroupsTree_AfterLabelEdit);
        }

        private void GroupsTree_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            if (e.Node != null)
                if (e.Node.Name.Substring(0, 1).CompareTo("G") == 0)
                    rename_group(e.Label);
        }

        private TreeNode CURNOD = null;
        private void GroupsTree_AfterSelect(object sender,TreeViewEventArgs  e)
        {
            CURNOD = e.Node;
        }

        public TreeNode Find_Employee(string Emp, TreeNode LastNode, bool Forward)
        {
            TreeNode FindNode = null;
            NoEmpl d = null;
            if (NEm != null)
                try
                {
                    if (LastNode == null)
                    {
                        d = NEm.first;
                        while (d != null)
                        {
                            if (d.name.Contains(Emp))
                            {
                                FindNode = d.node;
                                break;
                            }
                            d = d.next;
                        }
                    }
                    else
                    {
                        if (Forward)
                        {
                            d = NEm.first;
                            while (d != null)
                            {
                                if (d.node.Name.CompareTo(LastNode.Name) == 0)
                                    break;
                                d = d.next;
                            }
                            if (d != null)
                                while (d != null)
                                {
                                    if (d.name.Contains(Emp))
                                    {
                                        FindNode = d.node;
                                        break;
                                    }
                                    d = d.next;
                                }

                        }
                        else
                        {
                            d = NEm.first;
                            while (d != null && d.node.Name.CompareTo(LastNode.Name) != 0)
                            {
                                if (d.name.Contains(Emp))
                                    FindNode = d.node;
                                d = d.next;
                            }
                        }
                    }
                }
                catch { FindNode = null; }
            return FindNode;
        }

        private Its_Grp NGrp = null;
        private class Its_Grp
        {
            public string name;
            public System.Windows.Forms.TreeNode node;
            public Its_Grp first;
            public Its_Grp next;
            public bool is_vizing;
            public bool is_moving;

            public int cod;

            public Its_Grp()
            {
                first = this;
                next = null;
            }

            public Its_Grp(Its_Grp D)
            {
                first = D.first;
                D.next = this;
                next = null;
                D = this;
            }
        }

        private bool draw_schema(bool MyGroupsShow)
        {
            GroupsTree.Nodes.Clear();
            string sql = "";
            if (MyGroupsShow) sql = AMAS_Query.Class_AMAS_Query.My_Groups_listing();
            else sql = AMAS_Query.Class_AMAS_Query.Groups_listing();
            bool Res = false;
                if (SybAcc.Set_table("TStruct3", sql, null))
                {
                    groups_count= SybAcc.Rows_count;
 
                    for (int i = 0; i < SybAcc.Rows_count; i++)
                        try
                        {
                            SybAcc.Get_row(i);
                            if (NGrp == null)
                                NGrp = new Its_Grp();
                            else
                                NGrp = new Its_Grp(NGrp);
                            NGrp.cod = (int)SybAcc.Find_Field("id");
                            NGrp.name = (string)SybAcc.Find_Field("denote");
                            NGrp.is_vizing = (bool)SybAcc.Find_Field("vizing");
                            NGrp.is_moving = (bool)SybAcc.Find_Field("executing");
                            NGrp.node = GroupsTree.Nodes.Add("G" + NGrp.cod.ToString(), NGrp.name);
                            int img = 0;
                            if (NGrp.is_moving) img = 1;
                            if (NGrp.is_vizing) img = img | 2;
                            node_image(img);

                            if (Show_Empl) EmplInGrp(NGrp);
                        }
                        catch (Exception e)
                        {
                            SybAcc.EBBLP.AddError(e.Message, "Structure - 13", e.StackTrace);
                            Res = false;
                        }
                SybAcc.ReturnTable();
                }
                Res = true;
            return Res;
        }


        public int[] getSelectedExeVizGroups(bool Execute)
        {
            int[] res = null;
            bool takeit = false;
            try
            {
                if (NGrp != null)
                {
                    Its_Grp IG = NGrp.first;
                    while (IG != null)
                    {
                        if (IG.is_moving && Execute) takeit = true;
                        else if (IG.is_vizing && !Execute) takeit = true;
                        else takeit = false;
                        try
                        {
                            if (takeit && IG.node.Checked)
                            {
                                if (res == null)
                                {
                                    res = new int[1];
                                    res[0] = IG.cod;
                                }
                                else
                                {
                                    int[] r = new int[res.Length + 1];
                                    res.CopyTo(r, 0);
                                    r[r.Length - 1] = IG.cod;
                                    res = r;
                                }
                            }
                        }
                            catch{}
                        IG = IG.next;
                    }
                }
            }
            catch {  }
            return res;
        }

        public int[] getSelectedExeVizUsers()
        {
            int[] res=null;
            NoEmpl Ne=null;
            try
            {
                if (NEm != null)
                {
                    Ne = NEm.first;
                    while (Ne != null)
                    {
                        try
                        {
                            if (Ne.node.Checked)
                            {
                                if (res == null)
                                {
                                    res = new int[1];
                                    res[0] = Ne.cod;
                                }
                                else
                                {
                                    int[] r = new int[res.Length + 1];
                                    res.CopyTo(r, 0);
                                    r[r.Length - 1] = Ne.cod;
                                    res = r;
                                }
                            }
                        }
                            catch{}
                        Ne = Ne.next;
                    }
                }
            }
            catch {  }
            return res;
        }

        public string getUserNamebyCod(int id)
        {
            string res = "";
            NoEmpl Ne = null;
            try
            {
                if (NEm != null)
                {
                    Ne = NEm.first;
                    while (Ne != null)
                    {
                        try
                        {
                            if (Ne.cod==id)
                            {
                                res = Ne.name;
                            }
                        }
                        catch { }
                        Ne = Ne.next;
                    }
                }
            }
            catch { res = ""; }
            return res;
        }

        private void remove_all_empl_in_grp(Its_Grp NGrp)
        {
                GroupsTree.SelectedNode = NGrp.node;
                if (NEm != null)
                {
                    NoEmpl empl = NEm.first;
                    NoEmpl firstempl = NEm.first;
                    NoEmpl lastempl = empl;
                    while (empl != null)
                    {
                        if (empl.group == NGrp.cod)
                        {
                            empl.node.Remove();
                            if (firstempl == empl)
                                if (empl.next == null) break;
                                else
                                {
                                    firstempl = empl.next;
                                    lastempl = empl.next;
                                }
                            else
                                lastempl.next = empl.next;
                        }
                        empl.first = firstempl;
                        lastempl = empl;
                        empl = empl.next;
                    }
                }
        }

        private void EmplInGrp(Its_Grp NG)
        {
            NGrp = NG;
            show_empl_in_grp(AMAS_Query.Class_AMAS_Query.Group_Employee(NGrp.cod));
        }

        private void show_empl_in_grp(string sql)
        {
            string nnn = "";
            int mankikon = 22;
            string fio = "";
            int leader = 0;
            if (SybAcc.Set_table("TStruct4", sql, null))
            {
                try
                {
                    for (int l = 0; l < SybAcc.Rows_count; l++)
                    {
                        SybAcc.Get_row(l);
                        int id = (int)SybAcc.Find_Field("id");
                        int cod = (int)SybAcc.Find_Field("cod");
                        nnn = "E" + id.ToString();
                        {
                            if (NEm == null)
                                NEm = new NoEmpl();
                            else
                                NEm = new NoEmpl(NEm);
                            fio = (string)SybAcc.Find_Field("fio");
                            leader = (int)SybAcc.Find_Field("leader");
                            if (fio.Trim().CompareTo("?") == 0) mankikon = 22;
                            else mankikon = 23;
                            if (leader == 1) mankikon += 2;
                            NEm.node = NGrp.node.Nodes.Add(nnn, (string)SybAcc.Find_Field("naming") + " " + fio, IkonEmpl.NumIkon(fio, leader, Check_Ikon));
                            NEm.name = nnn;
                            NEm.ident = id;
                            NEm.cod = cod;
                            NEm.node.NodeFont = new Font("Arial", 8);
                            NEm.node.ForeColor = Color.DarkViolet;
                            NEm.group = NGrp.cod;
                        }
                    }
                }
                catch (Exception e) 
                { 
                    SybAcc.EBBLP.AddError(e.Message, "Structure - 14", e.StackTrace);
                }
                SybAcc.ReturnTable();
            }
            NGrp.node.Expand();
        }

        private void EmpltoGrp(int wgp)
        {
                show_empl_in_grp( AMAS_Query.Class_AMAS_Query.Get_Employee_of_Group(wgp));
        }

        public void add_group(string name)
        {
            int id = AMASCommand.Add_Group(name);
            if (id == -11)
                MessageBox.Show("Попытка дублирования группы");
            else
            {
                if (NGrp == null)
                    NGrp = new Its_Grp();
                else
                    NGrp = new Its_Grp(NGrp);
                NGrp.cod = id;
                NGrp.name = "Новая группа";
                NGrp.node = GroupsTree.Nodes.Add("G" + NGrp.cod.ToString(), NGrp.name);
                NGrp.node.TreeView.SelectedNode = NGrp.node;
            }
        }

        public void rename_group(string name)
        {
            AMASCommand.Rename_Group(get_ident(), name);
                
        }

        public void delete_group()
        {
            if(NGrp!=null)
            if (AMASCommand.Delete_Group(get_ident()))
            {
                remove_all_empl_in_grp(NGrp);
                {
                    Its_Grp delgrp = NGrp;
                    Its_Grp grp = NGrp.first;
                    Its_Grp firstgrp = NGrp.first;
                    Its_Grp lastgrp = grp;
                    while (grp != null)
                    {
                        if (grp == delgrp)
                        {
                            grp.node.Remove();
                            if (firstgrp == grp)
                                if (grp.next == null) break;
                                else
                                {
                                    firstgrp = grp.next;
                                    lastgrp = grp.next;
                                }
                            else
                                lastgrp.next = grp.next;
                        }
                        grp.first = firstgrp;
                        lastgrp = grp;
                        grp = grp.next;
                    }
                }

            }

        }

        public int get_ident()
        {
            string ResultErr = "";
            int id = -1;
            if (NGrp != null && CURNOD != null)
            {
                try
                {
                    if (CURNOD.Name.Substring(0, 1).CompareTo("E") == 0) CURNOD = CURNOD.Parent;
                    Its_Grp g = NGrp.first;
                    while (g != null)
                    {
                        if (g.node.Name.CompareTo(CURNOD.Name) == 0)
                        {
                            id = g.cod;
                            NGrp = g;
                            break;
                        }
                        g = g.next;
                    }
                }
                catch (Exception err)
                {
                    SybAcc.EBBLP.AddError(err.Message, "Structure - 15", err.StackTrace);
                    ResultErr = err.Message; id = -2;
                }
                CURNOD = NGrp.node;
                GroupsTree.SelectedNode = CURNOD;
            }
            return id;
        }
        
        public int get_index()
        {
            string ResultErr = "";
            int id = -1;
            if (NGrp != null && CURNOD != null)
            {
                try
                {
                    if (CURNOD.Name.Substring(0, 1).CompareTo("E") == 0) CURNOD = CURNOD.Parent;
                    Its_Grp g = NGrp.first;
                    while (g != null)
                    {
                        if (g.node.Name.CompareTo(CURNOD.Name) == 0)
                        {
                            id = g.node.Index;
                            NGrp = g;
                            break;
                        }
                        g = g.next;
                    }
                }
                catch (Exception err)
                {
                    SybAcc.EBBLP.AddError(err.Message, "Structure - 16", err.StackTrace);
                    ResultErr = err.Message; id = -2;
                }
                CURNOD = NGrp.node;
                GroupsTree.SelectedNode = CURNOD;
            }
            return id;
        }

        public void Set_groupVizing()
        {
            if (CURNOD != null)
            {
                int img = AMASCommand.Group_For_vizing(get_ident());
                node_image(img);
            }
        }
        
        public void Set_groupMoving()
        {
            if (CURNOD != null)
            {
                int img=AMASCommand.Group_For_moving(get_ident());
                node_image(img);
            }
        }

        public void Set_groupFull()
        {
            if (CURNOD != null)
            {
                int img=AMASCommand.Group_For_all(get_ident());
                node_image(img);
            }
        }

        public void append_empl(long empl)
        {
            int cod=AMASCommand.Employee_to_Group(get_ident(), empl);
            if (cod > 0)
                EmpltoGrp(cod);
        }

        public void remove_empl()
        {
            NoEmpl NE = NEm;
            if (NE != null)
            {
                TreeNode curnd = CURNOD;
                NE=NE.first;
                NoEmpl first = NE;
                NoEmpl last = NE;
                while (NE!=null)
                {
                    if (NE.node.Name.CompareTo(curnd.Name) == 0)
                        if (AMASCommand.Employee_from_Group(NE.ident))
                        {
                            NE.node.Remove();
                            if (NE.first == NE)
                            {
                                if (NE.next != null) first = NE.next;
                                else NEm = null;
                            }
                            else
                            {
                                last.next = NE.next;
                            }
                        }
                    NE.first = first;
                    last = NE;
                    NE=NE.next;
                }
            }
        }

        private void node_image(int img)
        {
            switch (img)
            {
                case 1:
                    NGrp.node.ImageIndex = 26;
                    NGrp.is_moving = true;
                    NGrp.is_vizing = false;
                    break;
                case 2:
                    NGrp.node.ImageIndex = 27;
                    NGrp.is_moving = false;
                    NGrp.is_vizing = true;
                    break;
                case 3:
                    NGrp.node.ImageIndex = 28;
                    NGrp.is_moving = true;
                    NGrp.is_vizing = true;
                    break;
            }

        }

        private class NoEmpl
        {
            public string name;
            public System.Windows.Forms.TreeNode node;
            public NoEmpl first;
            public NoEmpl next;
            public int ident;
            public int group;
            public int cod;

            public NoEmpl()
            {
                first = this;
                next = null;
            }
            public NoEmpl(NoEmpl f)
            {
                first = f.first;
                f.next = this;
                next = null;
                f = this;
            }
        }
        private NoEmpl NEm = null;

    }

    static public class IkonEmpl
    {
        const int Icon_empl = 20;

        public static int NumIkon(string fio, int leader,bool manky)
        {
            if (manky)
            {
                int mankikon = 22;
                if (fio.Trim().CompareTo("?") == 0) mankikon = 22;
                else mankikon = 23;
                if (leader == 1) mankikon += 2;
                return mankikon;
            }
            else return Icon_empl;
        }
    }
}

