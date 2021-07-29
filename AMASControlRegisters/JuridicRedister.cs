using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;

namespace AMASControlRegisters
{
    public partial class JuridicRegister : UserControl
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private juridic_ids subOrg;
        private array_selecting CurrentSELECT;

        public delegate void OrgSelected(string Org, int Ident);
        public event OrgSelected Orged;
        public delegate void EmployeeSelected(string Org, int Ident);
        public event EmployeeSelected Employed;

        public string Current_ORG
        {
            get
            {
                string ORG = "";
                if (CurrentSELECT != null)
                    if (CurrentSELECT.current_select != null)
                        ORG = CurrentSELECT.current_select.get_name();
                return ORG;
            }
        }

        public int Current_Ident
        {
            get
            {
                int Id = -1;
                if (CurrentSELECT != null)
                    if (CurrentSELECT.current_select != null)
                        Id = CurrentSELECT.current_select.get_ident();
                return Id;
            }
        }

        System.Windows.Forms.ToolStripTextBox ORG_Name ;
        System.Windows.Forms.ToolStripTextBox Short_Name ;
        private int toolStripOrg_Width;
        public JuridicRegister()
        {
            InitializeComponent();
            ORG_Name = (ToolStripTextBox)toolStripOrg.Items["ORG_Name"];
            ORG_Name.Dock = DockStyle.Fill;
            toolStripOrg_Width = toolStripOrg.Width;
            Short_Name = (ToolStripTextBox)toolStripOrg.Items["Short_Name"];
            this.ORG_Name.TextChanged+=new EventHandler(ORG_TextChanged);
            this.Short_Name.TextChanged += new EventHandler(ORG_TextChanged);
            toolStripOrg.SizeChanged+=new EventHandler(toolStripOrg_SizeChanged);
            JuridicList.DoubleClick+=new EventHandler(JuridicList_DoubleClick);
            listEmployees.DoubleClick+=new EventHandler(listEmployees_DoubleClick);
            JuridicList.SelectedIndexChanged += new EventHandler(JuridicList_SelectedIndexChanged_1);
        }

        private void listEmployees_DoubleClick(object sender, EventArgs e)
        {
            if (Employed!=null) Employed(EmployeesOfOrg.get_Degree_Name() + " " + subOrg.get_name().Trim() + " " + EmployeesOfOrg.get_FIO(), EmployeesOfOrg.get_agent());
        }

        private void toolStripOrg_SizeChanged(object sender, EventArgs e)
        {
            ORG_Name.Width += toolStripOrg.Width-toolStripOrg_Width ;
            toolStripOrg_Width = toolStripOrg.Width;
        }

        private void JuridicList_DoubleClick(object sender, EventArgs e)
        {
            if (Orged!=null) Orged(subOrg.get_name(), subOrg.get_contragent());
        }

        public void connect(AMAS_DBI.Class_syb_acc SybAcc)
        {
            AMASacc = SybAcc;
            addressReg.connect(AMASacc);
            CurrentSELECT = new array_selecting();
            CurrentSELECT.current_select = null;
            CurrentSELECT.last_select = null;
            CurrentSELECT.current_select = null;
        }

        private void ORG_TextChanged(object sender, EventArgs e)
        {
            if (subOrg != null)
                subOrg.get_number_by_text(ORG_Name.Text.Trim());
        }
        
        private class array_selecting
        {
            public object last_select;
            public juridic_ids current_select;
            public object next_select;
            public array_selecting(juridic_ids s)
            {
                last_select = null;
                current_select = s;
                next_select = null;
            }
            public array_selecting()
            {
                last_select = null;
                current_select = null;
                next_select = null;
            }
        }

        private class juridic_ids
        {
            private int Array_Dimention = 0;
            private int CURNUM = 0;
            private int[] Index = null;
            private int[] Ident = null;
            private int[] Address = null;
            private string[] OrgName = null;
            private int[] Contragent = null;
            private System.Windows.Forms.ListBox JuridicList;
            private AMAS_DBI.Class_syb_acc AMASacc;

            public int Current_Number { get { return CURNUM; } set { CURNUM = value; JuridicList.SelectedIndex = Index[CURNUM]; } }

            public juridic_ids(AMAS_DBI.Class_syb_acc SybAcc, System.Windows.Forms.ListBox list)
            {
                AMASacc = SybAcc;
                JuridicList = list;
                JuridicList.Items.Clear();
                try
                {
                    if (AMASacc.Set_table("TJReg1", AMAS_Query.Class_AMAS_Query.SeekJuridic,null))
                    {
                        string OName = "";
                        JuridicList.Items.Clear();
                        Array_Dimention = AMASacc.Rows_count;
                        Index = new int[Array_Dimention];
                        Ident = new int[Array_Dimention];
                        Address = new int[Array_Dimention];
                        OrgName = new string[Array_Dimention];
                        Contragent= new int[Array_Dimention];

                        for (int i = 0; i < Array_Dimention; i++)
                        {
                            AMASacc.Get_row(i);
                            AMASacc.Find_Field("Org");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                            else OName = "";
                            OrgName[i] = OName;
                            Address[i] = (int)AMASacc.Find_Field("fla_id");
                            Ident[i] = (int)AMASacc.Find_Field("jur_id");
                            Index[i] = JuridicList.Items.Add(OName);
                            Contragent[i] = (int)AMASacc.Find_Field("agentid");
                        }
                    }
                }
                catch (Exception e) 
                { 
                    AMASacc.EBBLP.AddError(e.Message, "Juridic Register - 1", e.StackTrace);
                }
            }

            private string ResultErr = "";

            public void refresh()
            {
                JuridicList.Items.Clear();
                for (int i = 0; i < Array_Dimention; i++)
                {
                    Index[i] = JuridicList.Items.Add(OrgName[i]);
                }
            }

            public int get_ident()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == JuridicList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Ident[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 2", err.StackTrace);
                    ResultErr = err.Message; return -2;
                }
                return -1;
            }

            public int get_contragent()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == JuridicList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Contragent[i];
                        }
                    }
                }
                catch (Exception err)
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 2.1", err.StackTrace);
                    ResultErr = err.Message; return 0;
                }
                return 0;
            }

            public int get_flat()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == JuridicList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Address[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 3", err.StackTrace);
                    ResultErr = err.Message; return -2;
                }
                return -1;
            }

            public string get_name()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == JuridicList.SelectedIndex)
                        {
                            Current_Number = i;
                            return OrgName[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 4", err.StackTrace);
                    ResultErr = err.Message; return "";
                }
                return "";
            }

            public bool set_flat(int flat)
            {
                try
                {
                    for (int i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == JuridicList.SelectedIndex)
                        {
                            Address[i] = flat;
                            return true;
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 5", err.StackTrace);
                    ResultErr = err.Message;
                }
                return false;

            }

            public int get_number_by_text(string aOrgName)
            {
                int i;
                int curnumber = -1;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (aOrgName.Length <= OrgName[i].Length)
                            if (aOrgName.ToLower().CompareTo(OrgName[i].Substring(0, aOrgName.Length).ToLower()) == 0)
                            {
                                curnumber = i;
                                break;
                            }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 6", err.StackTrace);
                    ResultErr = err.Message; curnumber = -2;
                }
                if (curnumber >= 0)
                {
                    Current_Number = curnumber;
                }
                return curnumber;
            }

            public int get_index_by_text(string Full)
            {
                int num = get_number_by_text(Full);
                if (num >= 0)
                {
                    JuridicList.SelectedIndex = Index[num];
                    return Index[num];
                }
                else
                {
                    return -1;
                }
            }
        }

        array_selecting now_select;

        public void get_juridic_list()
        {
            int flatid = -1;
            int houseid = -1;
            int streetid = -1;
            int districtid = -1;
            int cityid = -1;
            int arealid = -1;
            int trcid = -1;
            int stateid = -1;

            if (addressReg.flat.Trim().Length > 0) flatid = addressReg.flatID;
            if (addressReg.house.Trim().Length > 0) houseid = addressReg.houseID;
            if (addressReg.street.Trim().Length > 0) streetid = addressReg.streetID;
            if (addressReg.district.Trim().Length > 0) districtid = addressReg.districtID;
            if (addressReg.city.Trim().Length > 0) cityid = addressReg.cityID;
            if (addressReg.areal.Trim().Length > 0) arealid = addressReg.arealID;
            if (addressReg.trc.Trim().Length > 0) trcid = addressReg.trcID;
            if (addressReg.state.Trim().Length > 0) stateid = addressReg.stateID;
            if (AMAS_DBI.AMASCommand.Seek_Juridic(ORG_Name.Text.Trim(), Short_Name.Text.Trim(), flatid, houseid, streetid, districtid, cityid, arealid, trcid, stateid))
            {
                subOrg = new juridic_ids(AMASacc, this.JuridicList);
                now_select = new array_selecting();
                now_select.current_select = subOrg;
                now_select.last_select = null;
                now_select.next_select = null;
                if (CurrentSELECT.current_select != null)
                {
                    now_select.next_select = CurrentSELECT.next_select;
                    if (CurrentSELECT.next_select != null)
                    {
                        array_selecting is_select = (array_selecting)CurrentSELECT.next_select;
                        is_select.last_select = now_select;
                    }
                    CurrentSELECT.next_select = now_select;
                    now_select.last_select = CurrentSELECT;
                }
                CurrentSELECT = now_select;
            }
            //if (JuridicList.Items.Count > 0) JuridicList.SelectedIndex = 0;
        }

        public void Add_ORG()
        {
            AMAS_DBI.AMASCommand.add_ORG(ORG_Name.Text.Trim(), Short_Name.Text.Trim(), addressReg.get_address());
            get_juridic_list();
        }

        private void Select_Click_1(object sender, EventArgs e)
        {
            get_juridic_list();
        }

        private void ClearOrgs_Click(object sender, EventArgs e)
        {
            addressReg.clear_address();
            CurrentSELECT = new array_selecting();
            subOrg = null;
            JuridicList.Items.Clear();
            ORG_Name.Text = "";
            Short_Name.Text = "";
        }

        private void set_orgadr_Click(object sender, EventArgs e)
        {
            int fla_id = addressReg.get_address();
            if (AMASCommand.UpdateOrgAddress(subOrg.get_ident(), fla_id))
                subOrg.set_flat(fla_id);
        }

        private void undorg_Click(object sender, EventArgs e)
        {
            if (CurrentSELECT.last_select != null)
            {
                CurrentSELECT = (array_selecting)CurrentSELECT.last_select;
                subOrg = CurrentSELECT.current_select;
                subOrg.refresh();
            }
        }

        private void redorg_Click(object sender, EventArgs e)
        {
            if (CurrentSELECT.next_select != null)
            {
                CurrentSELECT = (array_selecting)CurrentSELECT.next_select;
                subOrg = CurrentSELECT.current_select;
                subOrg.refresh();
            }
        }

        private void newOrg_Click(object sender, EventArgs e)
        {
            Add_ORG();
        }

        employee_ids EmployeesOfOrg;

        private void JuridicList_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            addressReg.set_address(subOrg.get_flat());
            Employes.Text=subOrg.get_name();
            EmployeesOfOrg = new employee_ids(AMASacc, listEmployees, subOrg.get_ident());
            Degree.Text = "";
            Family.Text = "";
            Naming.Text = "";
            Father.Text = "";
        }

        public int Current_Employee_ID
        {
            get
            {
                if (EmployeesOfOrg != null)
                    return EmployeesOfOrg.get_ident();
                else
                    return -1;
            }
        }

        public string Current_Man
        {
            get
            {
                if (EmployeesOfOrg != null)
                    return EmployeesOfOrg.get_FIO();
                else
                    return "";
            }
        }

        public int Current_Man_ID
        {
            get
            {
                if (EmployeesOfOrg != null)
                    return EmployeesOfOrg.get_Man_ID();
                else
                    return -1;
            }
        }

        public int Current_Degree_ID
        {
            get
            {
                if (EmployeesOfOrg != null)
                    return EmployeesOfOrg.get_Degree_ID();
                else
                    return -1;
            }
        }

        public string Current_Degree
        {
            get
            {
                if (EmployeesOfOrg != null)
                    return EmployeesOfOrg.get_Degree_Name();
                else
                    return "";
            }
        }

        private class employee_ids
        {
            private int Array_Dimention = 0;
            private int CURNUM = 0;
            private int[] Index = null;
            private int[] Ident = null;
            private int[] Address = null;
            private int[] Degree_ID = null;
            private int[] Man_ID = null;
            private int[] AgentId = null;
            private string[] Family = null;
            private string[] Naming = null;
            private string[] Father = null;
            private string[] Degree = null;
            private string[] FIO = null;
            private int JURID = -1;
            private System.Windows.Forms.ListBox employeeList;
            private AMAS_DBI.Class_syb_acc AMASacc;

            public int Current_Number { get { return CURNUM; } set { CURNUM = value; employeeList.SelectedIndex = Index[CURNUM]; } }
            public int Juridic_id { get { return JURID; } }

            public employee_ids(AMAS_DBI.Class_syb_acc SybAcc, System.Windows.Forms.ListBox list, int juridic)
            {
                AMASacc = SybAcc;
                employeeList = list;
                JURID = juridic;
                employeeList.Items.Clear();
                requery_list_emp();
            }
            
            public void requery_list_emp()
            {
                try
                {
                    if (AMASacc.Set_table("TJReg2", AMAS_Query.Class_AMAS_Query.SeekEmployee(JURID), null))
                    {
                        string OName = "";
                        employeeList.Items.Clear();
                        Array_Dimention = AMASacc.Rows_count;
                        Index = new int[Array_Dimention];
                        Ident = new int[Array_Dimention];
                        Address = new int[Array_Dimention];
                        Family = new string[Array_Dimention];
                        Naming = new string[Array_Dimention];
                        Father = new string[Array_Dimention];
                        Degree = new string[Array_Dimention];
                        Degree_ID = new int[Array_Dimention];
                        Man_ID = new int[Array_Dimention];
                        FIO = new string[Array_Dimention];
                        AgentId = new int[Array_Dimention];

                        for (int i = 0; i < Array_Dimention; i++)
                        {
                            AMASacc.Get_row(i);
                            AMASacc.Find_Field("Family");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                            else OName = "";
                            Family[i] = OName.Trim();
                            AMASacc.Find_Field("Name");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                            else OName = "";
                            Naming[i] = OName.Trim();
                            AMASacc.Find_Field("Father");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                            else OName = "";
                            Father[i] = OName.Trim();
                            AMASacc.Find_Field("fio");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                            else OName = "";
                            FIO[i] = OName.Trim();
                            AMASacc.Find_Field("degree");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                            else OName = "";
                            Degree[i] = OName.Trim();
                            AMASacc.Find_Field("id");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) Man_ID[i] = (int)AMASacc.get_current_Field();
                            else Man_ID[i] = -1;
                            AMASacc.Find_Field("iddeg");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) Degree_ID[i] = (int)AMASacc.get_current_Field();
                            else Degree_ID[i] = -1;
                            AMASacc.Find_Field("agent");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) AgentId[i] = (int)AMASacc.get_current_Field();
                            else AgentId[i] = 0;
                            Ident[i] = (int)AMASacc.Find_Field("cod");
                            Index[i] = employeeList.Items.Add(Degree[i] + " " + FIO[i]);
                        }
                    }
                }
                catch (Exception e) 
                { 
                    AMASacc.EBBLP.AddError(e.Message, "Juridic Register - 7", e.StackTrace);
                }
            }

            private string ResultErr = "";

            public void refresh()
            {
                employeeList.Items.Clear();
                for (int i = 0; i < Array_Dimention; i++)
                {
                    Index[i] = employeeList.Items.Add(Degree[i] + " " + FIO[i]);
                }
            }

            public int get_ident()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == employeeList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Ident[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 8", err.StackTrace);
                    ResultErr = err.Message; return -2;
                }
                return -1;
            }

            public int get_agent()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == employeeList.SelectedIndex)
                        {
                            Current_Number = i;
                            return AgentId[i];
                        }
                    }
                }
                catch (Exception err)
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 8.1", err.StackTrace);
                    ResultErr = err.Message; return 0;
                }
                return 0;
            }

            public string get_FIO()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == employeeList.SelectedIndex)
                        {
                            Current_Number = i;
                            return FIO[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 9", err.StackTrace);
                    ResultErr = err.Message; return "";
                }
                return "";
            }

            public int get_Man_ID()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == employeeList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Man_ID[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 10", err.StackTrace);
                    ResultErr = err.Message; return -2;
                }
                return -1;
            }

            public int get_Degree_ID()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == employeeList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Degree_ID[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 11", err.StackTrace);
                    ResultErr = err.Message; return -2;
                }
                return -1;
            }

            public string get_Degree_Name()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == employeeList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Degree[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 12", err.StackTrace);
                    ResultErr = err.Message; return "";
                }
                return "";
            }
            public int get_flat()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == employeeList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Address[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 13", err.StackTrace);
                    ResultErr = err.Message; return -2;
                }
                return -1;
            }

            public bool set_flat(int flat)
            {
                try
                {
                    for (int i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == employeeList.SelectedIndex)
                        {
                            Address[i] = flat;
                            return true;
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 14", err.StackTrace);
                    ResultErr = err.Message;
                }
                return false;

            }

            public int get_number_by_text(string afamily, string aname, string afather,string adegree)
            {
                int i;
                int curnumber = -1;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (adegree.Length <= Degree[i].Length)
                            if (afamily.ToLower().CompareTo(Degree[i].Substring(0, afamily.Length).ToLower()) == 0)
                            {
                                curnumber = i;
                                if (afamily.Length <= Family[i].Length)
                                    if (afamily.ToLower().CompareTo(Family[i].Substring(0, afamily.Length).ToLower()) == 0)
                                    {
                                        curnumber = i;
                                        if (aname.Length <= Naming[i].Length && curnumber >= 0)
                                            if (aname.ToLower().CompareTo(Naming[i].Substring(0, aname.Length).ToLower()) == 0)
                                            {
                                                curnumber = i;
                                                if (aname.Length <= Father[i].Length && curnumber >= 0)
                                                    if (afather.ToLower().CompareTo(Father[i].Substring(0, afather.Length).ToLower()) == 0)
                                                    {
                                                        curnumber = i;
                                                        break;
                                                    }
                                            }
                                    }
                            }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "Juridic Register - 15", err.StackTrace);
                    ResultErr = err.Message; curnumber = -2;
                }
                if (curnumber >= 0)
                {
                    Current_Number = curnumber;
                }
                return curnumber;
            }

            public int get_index_by_text(string fm, string nm, string fw, string dg)
            {
                int num = get_number_by_text(fm, nm, fw, dg);
                if (num >= 0)
                {
                    employeeList.SelectedIndex = Index[num];
                    return Index[num];
                }
                else
                {
                    return -1;
                }
            }
        }

        private void Add_emp_Click_1(object sender, EventArgs e)
        {
            try
            {
                AMAS_DBI.AMASCommand.ADD_Employee(Family.Text.Trim(), Naming.Text.Trim(), Father.Text.Trim(), Degree.Text.Trim(), EmployeesOfOrg.Juridic_id);
                EmployeesOfOrg.requery_list_emp();
                EmployeesOfOrg.get_number_by_text(Family.Text.Trim(), Naming.Text.Trim(), Father.Text.Trim(), Degree.Text.Trim());
            }
            catch (Exception ex) 
            {
                AMASacc.EBBLP.AddError(ex.Message, "Juridic Register - 17", ex.StackTrace);
            }
        }

        private void ORGName_TextChanged(object sender, EventArgs e)
        {

        }

        private void Short_Name_Click(object sender, EventArgs e)
        {

        }

        private void Add_emp_Click(object sender, EventArgs e)
        {
            try
            {
                AMAS_DBI.AMASCommand.ADD_Employee(Family.Text.Trim(), Naming.Text.Trim(), Father.Text.Trim(), Degree.Text.Trim(), EmployeesOfOrg.Juridic_id);
                EmployeesOfOrg.requery_list_emp();
                EmployeesOfOrg.get_number_by_text(Family.Text.Trim(), Naming.Text.Trim(), Father.Text.Trim(), Degree.Text.Trim());
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "Juridic Register - 17", ex.StackTrace);
            }
        }

    }
}
