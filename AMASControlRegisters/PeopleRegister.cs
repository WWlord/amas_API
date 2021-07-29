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
    public partial class PeopleRegister : UserControl
    {
        public delegate void ManSelected(string Man, int Ident);
        public event ManSelected Maned;

        public int Current_Man
        {
            get 
            {
                if (subPopulation != null)
                    return subPopulation.get_ident();
                else return -1;
            }
        }

        public string FIO_of_Current_Man
        {
            get
            {
                if (subPopulation != null)
                    return subPopulation.get_FIO();
                else return "";
            }
        }

        private AMAS_DBI.Class_syb_acc AMASacc;
        private  people_ids subPopulation=null;
        
        private class array_selecting
        {
            public object last_select;
            public people_ids current_select;
            public object next_select;
            public array_selecting(people_ids s)
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

        private array_selecting CurrentSELECT;

        public  PeopleRegister()
        {
            InitializeComponent();

            this.Family.TextChanged +=new EventHandler(FIO_TextChanged);
            this.Naming.TextChanged += new EventHandler(FIO_TextChanged);
            this.Father.TextChanged += new EventHandler(FIO_TextChanged);
            this.PeopleList.DoubleClick+=new EventHandler(PeopleList_DoubleClick);
        }

        private void PeopleList_DoubleClick(object sender, EventArgs e)
        {
            string FIO=subPopulation.get_FIO();
            int agent=subPopulation.get_agent();
            if (Maned!=null) Maned(FIO, agent);
        }

        private void FIO_TextChanged(object sender, EventArgs e)
        {
            if (subPopulation !=null) 
                subPopulation.get_number_by_text(Family.Text.Trim(), Naming.Text.Trim(), Father.Text.Trim());
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


        private class people_ids
        {
            int Array_Dimention = 0;
            int CURNUM =0;
            int Current_Number { get { return CURNUM; } set { CURNUM = value; peopleList.SelectedIndex = Index[CURNUM]; } }
            int[] Index = null;
            int[] Ident = null;
            int[] Address = null;
            int[] AgentId = null;
            string[] Family = null;
            string[] Naming = null;
            string[] Father = null;
            string[] FIO = null;

            private System.Windows.Forms.ListBox peopleList;
            private AMAS_DBI.Class_syb_acc AMASacc;

            public people_ids(AMAS_DBI.Class_syb_acc SybAcc, System.Windows.Forms.ListBox list)
            {
                AMASacc = SybAcc;
                peopleList = list;
                peopleList.Items.Clear();
                try
                {
                    if (AMASacc.Set_table("TPReg1", AMAS_Query.Class_AMAS_Query.SeekPeople,null))
                    {
                        string OName = "";
                        peopleList.Items.Clear();
                        Array_Dimention = AMASacc.Rows_count;
                        Index = new int[Array_Dimention];
                        Ident = new int[Array_Dimention];
                        Address = new int[Array_Dimention];
                        Family = new string[Array_Dimention];
                        Naming = new string[Array_Dimention];
                        Father = new string[Array_Dimention];
                        FIO = new string[Array_Dimention];
                        AgentId = new int[Array_Dimention];

                        for (int i = 0; i < Array_Dimention; i++)
                        {
                            try
                            {
                                AMASacc.Get_row(i);
                                AMASacc.Find_Field("Family");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                                else OName = "";
                                Family[i] = OName;
                                AMASacc.Find_Field("Naming");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                                else OName = "";
                                Naming[i] = OName;
                                AMASacc.Find_Field("Father");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                                else OName = "";
                                Father[i] = OName;
                                AMASacc.Find_Field("fio");
                                OName = AMASacc.get_current_Field().GetType().ToString();
                                if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                                else OName = "";
                                FIO[i] = OName;
                                Address[i] = (int)AMASacc.Find_Field("fla_id");
                                Ident[i] = (int)AMASacc.Find_Field("pip_id");
                                Index[i] = peopleList.Items.Add((string)AMASacc.Find_Field("fio"));
                                AgentId[i] = (int)AMASacc.Find_Field("agentid");
                            }
                            catch (Exception ex)
                            {
                                AMASacc.EBBLP.AddError(ex.Message, "People - 3", ex.StackTrace);
                            }
                        }
                    }
                }
                catch (Exception e) { Console.WriteLine(e.Message); }
           }

            private string ResultErr = "";

            public void refresh()
            {
                peopleList.Items.Clear();
                for (int i = 0; i < Array_Dimention; i++)
                {
                    Index[i] = peopleList.Items.Add(FIO[i]);
                }
            }

            public int get_ident()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == peopleList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Ident[i];
                        }
                    }
                }
                catch (Exception err) { ResultErr = err.Message; return -2; }
                return -1;
            }

            public int get_agent()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == peopleList.SelectedIndex)
                        {
                            Current_Number = i;
                            return AgentId[i];
                        }
                    }
                }
                catch (Exception err) { ResultErr = err.Message; return 0; }
                return 0;
            }

            public string get_FIO()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == peopleList.SelectedIndex)
                        {
                            Current_Number = i;
                            return FIO[i];
                        }
                    }
                }
                catch (Exception err) { ResultErr = err.Message; return ""; }
                return "";
            }
            
            public int get_flat()
            {
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == peopleList.SelectedIndex)
                        {
                            Current_Number = i;
                            return Address[i];
                        }
                    }
                }
                catch (Exception err) { ResultErr = err.Message; return -2; }
                return -1;
            }

            public bool set_flat(int flat)
            {
                try
                {
                    for (int i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == peopleList.SelectedIndex)
                        {
                            Address[i]=flat;
                            return true;
                        }
                    }
                }
                catch (Exception err) { ResultErr = err.Message; }
                return false;

            }

            public int get_number_by_text(string afamily, string aname, string afather)
            {
                int i;
                int curnumber = -1;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
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
                catch (Exception err) { ResultErr = err.Message; curnumber = -2; }
                if (curnumber >= 0)
                {
                    Current_Number = curnumber;
                }
                return curnumber;
            }

            public int get_index_by_text(string fm, string nm, string fw)
            {
                int num = get_number_by_text(fm,nm,fw);
                if (num >= 0)
                {
                    peopleList.SelectedIndex = Index[num];
                    return Index[num];
                }
                else
                {
                    return -1;
                }
            }
        }

        array_selecting now_select;
        
        public void get_piople_list()
        {
            int flatid = -1;
            int houseid = -1;
            int streetid = -1;
            int cityid = -1;
            try
            {
                if (addressReg.flat.Trim().Length > 0) flatid = addressReg.flatID;
                if (addressReg.house.Trim().Length > 0) houseid = addressReg.houseID;
                if (addressReg.street.Trim().Length > 0) streetid = addressReg.streetID;
                if (addressReg.city.Trim().Length > 0) cityid = addressReg.cityID;

                if (AMAS_DBI.AMASCommand.Seek_People(Family.Text.Trim(), Naming.Text.Trim(), Father.Text.Trim(),
                    flatid, houseid, streetid, cityid))
                {
                    subPopulation = new people_ids(AMASacc, this.PeopleList);
                    now_select = new array_selecting();
                    now_select.current_select = subPopulation;
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
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "People - 4", ex.StackTrace);
            }

        }

        private void Select_Click(object sender, EventArgs e)
        {
            get_piople_list();
        }

        public void Add_Man()
        {
            AMAS_DBI.AMASCommand.ADD_Man(Family.Text.Trim(), Naming.Text.Trim(), Father.Text.Trim(), addressReg.get_address());
            get_piople_list();
        }

        private void add_record_Click(object sender, EventArgs e)
        {
            Add_Man();
        }

        private void undo_Click(object sender, EventArgs e)
        {
            if (CurrentSELECT.last_select!=null)
            try
            {
                CurrentSELECT =(array_selecting) CurrentSELECT.last_select;
                subPopulation = CurrentSELECT.current_select;
                subPopulation.refresh();
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "People - 5", ex.StackTrace);
            }
        }

        private void redo_Click(object sender, EventArgs e)
        {
            if (CurrentSELECT.next_select != null)
                try
                {
                    CurrentSELECT = (array_selecting)CurrentSELECT.next_select;
                    subPopulation = CurrentSELECT.current_select;
                    subPopulation.refresh();
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "People - 6", ex.StackTrace);
                }
    }

        private void PeopleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            addressReg.set_address(subPopulation.get_flat());
        }

        private void Clear_Click(object sender, EventArgs e)
        {
            addressReg.clear_address();
            CurrentSELECT = new array_selecting();
            subPopulation = null;
            PeopleList.Items.Clear();
            Family.Text = "";
            Naming.Text = "";
            Father.Text = "";
        }

        private void set_address_Click(object sender, EventArgs e)
        {
            try
            {
                int fla_id = addressReg.get_address();
                if (AMASCommand.UpdateManAddress(subPopulation.get_ident(), fla_id))
                    subPopulation.set_flat(fla_id);
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "People - 7", ex.StackTrace);
            }
        }

    }
}
