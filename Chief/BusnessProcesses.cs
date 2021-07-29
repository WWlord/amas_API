using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;
using AMAS_Query;
using ClassPattern;
using CommonValues;

namespace Chief
{
    public partial class BusnessProcesses : Form
    {
        RolesClass RolesList = null;
        private AMAS_DBI.Class_syb_acc SybAcc;
        private bool InitialFaza = false;

        public BusnessProcesses(Class_syb_acc ACC)
        {
            TimeDelayAssign = false;
            InitializeComponent();

            SybAcc = ACC;

            InitialFaza = true;
            Clear();

            RolesList = new RolesClass(treeViewRoles, null, listViewMetadata, SybAcc, edTaskDescr, propertyGridExecution);
            InstructList();
            lvInstructionsList.ItemSelectionChanged += new ListViewItemSelectionChangedEventHandler(lvInstructionsList_ItemSelectionChanged);
            TimeDelayAssign = true;
            tabControl1.Selecting += new TabControlCancelEventHandler(tabControl1_Selecting);
            tabControl1.Selected += new TabControlEventHandler(tabControl1_Selected);
            InitialFaza = false;
       }

        void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            InitialFaza=false;
        }

        void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            InitialFaza=true;
        }

        void RoutesListing_CheckIdEvent(int id)
        {
            if (!InitialFaza)
            {
                //dataGridViewEvents.CurrentRow.Cells["route_"].Value = id;
                DataRow rrr = EventsTable.Rows[dataGridViewEvents.CurrentRow.Index];
                rrr["route_"] = id;

            }
        }

        void KindListing_CheckIdEvent(int id)
        {
            if (!InitialFaza)
            {
                //dataGridViewEvents.CurrentRow.Cells["kind"].Value = id;
                DataRow rrr = EventsTable.Rows[dataGridViewEvents.CurrentRow.Index];
                rrr["kind"] = id;
            }
        }

        void TemaListing_CheckIdEvent(int id)
        {
            if (!InitialFaza)
            {
                //dataGridViewEvents.CurrentRow.Cells["tema"].Value = id;
                DataRow rrr = EventsTable.Rows[dataGridViewEvents.CurrentRow.Index];
                rrr["tema"] = id;
            }
        }

        void RolesList_RoleCH(string RoleName)
        {
            tsslInstruction.Text = RoleName;
        }

        void lvInstructionsList_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            int sss = 0;
            treeViewRoles.Nodes.Clear();
            try
            {
                sss = (int)Convert.ToInt32(lvInstructionsList.Items[e.ItemIndex].SubItems[1].Text);
                RolesList.Requery(sss);
            }
            catch (Exception ex)
            {
                string qes = ex.Message;
            }
            tsslInstruction.Text = e.Item.SubItems[0].Text;
        }

        private bool RolesTreeChecking = false;

        private void InstructList()
        {
            lvInstructionsList.Clear();
            lvInstructionsList.View = View.Details;
            lvInstructionsList.Columns.Clear();
            lvInstructionsList.Columns.Add("Instructions", "Инструкции");
            lvInstructionsList.Columns[0].Width = lvInstructionsList.Width - 2;

            if (SybAcc.Set_table("BPInstructions", AMAS_Query.ClassAMAS_Buissnes_Process.InstructionsList, null))
            {
                for (int l = 0; l < SybAcc.Rows_count; l++)
                {
                    SybAcc.Get_row(l);
                    ListViewItem ItemIns = lvInstructionsList.Items.Add((string)SybAcc.Find_Field("line"));
                    ItemIns.SubItems.Add((string)Convert.ToString((int)SybAcc.Find_Field("cod")));
                    ItemIns.SubItems.Add((string)Convert.ToString((int)SybAcc.Find_Field("rank")));
                }
                SybAcc.ReturnTable();
            }


            if (SybAcc.Set_table("BPRoutes", AMAS_Query.ClassAMAS_Buissnes_Process.RoutesList, null))
            {
                RoutesTable = SybAcc.Current_table;
                dataGridViewRoutes.DataSource = RoutesTable;
                RoutesTable.RowChanged += new DataRowChangeEventHandler(RoutesTable_RowChanged);
                RoutesTable.RowDeleting += new DataRowChangeEventHandler(RoutesTable_RowDeleting);               
                if(dataGridViewRoutes.ColumnCount>0)
                {
                dataGridViewRoutes.Columns["id"].Visible = false;
                dataGridViewRoutes.Columns["name"].HeaderText = "Имя процедуры";
                dataGridViewRoutes.Columns["description_"].HeaderText = "Описание";
                dataGridViewRoutes.Click += new EventHandler(dataGridViewRoutes_Click);
                }
                SybAcc.ReturnTable();
            }

            if (SybAcc.Set_table("BPRoles", AMAS_Query.ClassAMAS_Buissnes_Process.AllRoulesList, null))
            {
                tvExeRoles.Nodes.Clear();
                TreeNode BD = null;
                TreeNode TD = null;
                AMAS_DBI.Class_syb_acc.PrepareParameters[] parroles = new Class_syb_acc.PrepareParameters[1];
                for (int l = 0; l < SybAcc.Rows_count; l++)
                {
                    SybAcc.Get_row(l);
                    BD = tvExeRoles.Nodes.Add("R" + ((int)SybAcc.Find_Field("id")).ToString(), (string)SybAcc.Find_Field("name"));
                    BD.Checked = false;
                    parroles[0] = new Class_syb_acc.PrepareParameters("@role", SqlDbType.Int, (int)SybAcc.Find_Field("id"));
                    if (SybAcc.Set_table("BPTasks", AMAS_Query.ClassAMAS_Buissnes_Process.BPTasksConnectList, parroles))
                    {
                        for (int i = 0; i < SybAcc.Rows_count; i++)
                        {
                            SybAcc.Get_row(i);
                            TD = BD.Nodes.Add("T" + ((int)SybAcc.Find_Field("id")).ToString(), (string)SybAcc.Find_Field("name"));
                            if ((int)SybAcc.Find_Field("connTask") == 0)
                                TD.Checked = false;
                            else TD.Checked = true;
                        }
                        SybAcc.ReturnTable();
                    }
                }
                tvExeRoles.AfterCheck += new TreeViewEventHandler(tvExeRoles_AfterCheck);
                RolesTreeChecking = true;
                SybAcc.ReturnTable();
            }

            if (SybAcc.Set_table("BPConditionsList", AMAS_Query.ClassAMAS_Buissnes_Process.ConditionsList, null))
            {
                DataRow rrr;
                DataColumn col;
                InitialFaza = true;

                ConditionsTable = SybAcc.Current_table;
                ConditionsTable.AcceptChanges();
                if (ConditionsTable.Columns.Count == 0)
                {
                    col = ConditionsTable.Columns.Add("id");
                    col.DataType = System.Type.GetType("System.Int32");
                    col = ConditionsTable.Columns.Add("role");
                    col.DataType = System.Type.GetType("System.Int32");
                    col = ConditionsTable.Columns.Add("execution_");
                    col.DataType = System.Type.GetType("System.Int32");
                    col = ConditionsTable.Columns.Add("name");
                    col.DataType = System.Type.GetType("System.String");
                    col = ConditionsTable.Columns.Add("description_");
                    col.DataType = System.Type.GetType("System.String");
                }
                if (ConditionsTable.Rows.Count == 0)
                {
                    rrr = ConditionsTable.NewRow();
                    rrr["id"] = -1;
                    rrr["role"] = -1;
                    rrr["execution_"] = -1;
                    rrr["name"] = "Введите новое событие";
                    rrr["description_"] = "";
                    ConditionsTable.Rows.Add(rrr);
                }

                dgvConditions.DataSource = ConditionsTable;
                dgvConditions.Columns["id"].Visible = false;
                dgvConditions.Columns["role"].Visible = false;
                dgvConditions.Columns["execution_"].Visible = false;
                dgvConditions.Columns["name"].HeaderText = "Условие";
                dgvConditions.Columns["description_"].HeaderText = "Описание";
                dgvConditions.CurrentCell = dgvConditions.Rows[0].Cells["name"];

                RolesConditionListing = new CheckedListing(SybAcc, AMAS_Query.ClassAMAS_Buissnes_Process.AllRoulesList, "name", "id");
                groupBoxCondRoles.Controls.Add(RolesConditionListing.listViewCheckup);
                ExecutionConditionListing = new CheckedListing(SybAcc, AMAS_Query.ClassAMAS_Buissnes_Process.AllTasksList, "name", "id");
                groupBoxCondExecutions.Controls.Add(ExecutionConditionListing.listViewCheckup);

                try { RolesConditionListing.CheckById((int)dgvConditions.CurrentRow.Cells["role"].Value); }
                catch { }
                try { ExecutionConditionListing.CheckById((int)dgvConditions.CurrentRow.Cells["execution_"].Value); }
                catch { }

                ConditionsTable.RowChanged += new DataRowChangeEventHandler(ConditionsTable_RowChanged);
                ConditionsTable.RowDeleting += new DataRowChangeEventHandler(ConditionsTable_RowDeleting);
                dgvConditions.Click += new EventHandler(dgvConditions_Click);
                ExecutionConditionListing.CheckIdEvent += new CheckedListing.CheckId(ExecutionConditionListing_CheckIdEvent);
                RolesConditionListing.CheckIdEvent += new CheckedListing.CheckId(RolesConditionListing_CheckIdEvent);

                SybAcc.ReturnTable();
            }

            if (SybAcc.Set_table("BPEventsList", AMAS_Query.ClassAMAS_Buissnes_Process.EventsList, null))
            {
                DataRow rrr;
                DataColumn col;
                EventsTable = SybAcc.Current_table;
                if (EventsTable.Columns.Count == 0)
                {
                    col = EventsTable.Columns.Add("id");
                    col.DataType = System.Type.GetType("System.Int32");
                    col = EventsTable.Columns.Add("kind");
                    col.DataType = System.Type.GetType("System.Int32");
                    col = EventsTable.Columns.Add("tema");
                    col.DataType = System.Type.GetType("System.Int32");
                    col = EventsTable.Columns.Add("route_");
                    col.DataType = System.Type.GetType("System.Int32");
                    col = EventsTable.Columns.Add("TimeDelay");
                    col.DataType = System.Type.GetType("System.Int32");
                    col = EventsTable.Columns.Add("name");
                    col.DataType = System.Type.GetType("System.String");
                    col = EventsTable.Columns.Add("description_");
                    col.DataType = System.Type.GetType("System.String");
                }
                if (EventsTable.Rows.Count == 0)
                {
                    rrr = EventsTable.NewRow();
                    rrr["id"] = -1;
                    rrr["kind"] = -1;
                    rrr["tema"] = -1;
                    rrr["route_"] = -1;
                    rrr["TimeDelay"] = 1;
                    rrr["name"] = "Введите новое событие";
                    rrr["description_"] = "";
                    EventsTable.Rows.Add(rrr);
                }
                dataGridViewEvents.DataSource = EventsTable;
                EventsTable.RowChanged += new DataRowChangeEventHandler(EventsTable_RowChanged);
                EventsTable.RowDeleting += new DataRowChangeEventHandler(EventsTable_RowDeleting);
                dataGridViewEvents.Columns["id"].Visible = false;
                dataGridViewEvents.Columns["kind"].Visible = false;
                dataGridViewEvents.Columns["tema"].Visible = false;
                dataGridViewEvents.Columns["route_"].Visible = false;
                dataGridViewEvents.Columns["TimeDelay"].Visible = false;
                dataGridViewEvents.Columns["name"].HeaderText = "Событие";
                dataGridViewEvents.Columns["description_"].HeaderText = "Описание";
                dataGridViewEvents.Click += new EventHandler(dataGridViewEvents_Click);
                TemaListing = new CheckedListing(SybAcc, AMAS_Query.Class_AMAS_Query.Get_Temy_Kind_Employee(1), "description_", "tema");
                gbTemy.Controls.Add(TemaListing.listViewCheckup);
                TemaListing.CheckIdEvent += new CheckedListing.CheckId(TemaListing_CheckIdEvent);
                KindListing = new CheckedListing(SybAcc, AMAS_Query.Class_AMAS_Query.Get_Temy_Kind_Employee(2), "kind", "kod");
                gbKind.Controls.Add(KindListing.listViewCheckup);
                KindListing.CheckIdEvent += new CheckedListing.CheckId(KindListing_CheckIdEvent);
                RoutesListing = new CheckedListing(SybAcc, AMAS_Query.ClassAMAS_Buissnes_Process.RoutesList, "name", "id");
                gbRoutes.Controls.Add(RoutesListing.listViewCheckup);
                RoutesListing.CheckIdEvent += new CheckedListing.CheckId(RoutesListing_CheckIdEvent);
                try { TemaListing.CheckById((int)dataGridViewEvents.CurrentRow.Cells["tema"].Value); }
                catch { }
                try { KindListing.CheckById((int)dataGridViewEvents.CurrentRow.Cells["kod"].Value); }
                catch { }
                try { RoutesListing.CheckById((int)dataGridViewEvents.CurrentRow.Cells["id"].Value); }
                catch { }
                
                mtbDays.TextChanged += new EventHandler(mtbDays_TextChanged);
                mtbhours.TextChanged += new EventHandler(mtbhours_TextChanged);
                mtbMinutes.TextChanged += new EventHandler(mtbMinutes_TextChanged);

                SybAcc.ReturnTable();
            }

        }

        void ExecutionConditionListing_CheckIdEvent(int id)
        {
            if (!InitialFaza)
            {
                //dgvConditions.CurrentRow.Cells["execution_"].Value = id;
                DataRow rrr = ConditionsTable.Rows[dgvConditions.CurrentRow.Index];
                rrr["execution_"] = id;
            }
        }

        void RolesConditionListing_CheckIdEvent(int id)
        {
            if (!InitialFaza)
            {
                //dgvConditions.CurrentRow.Cells["role"].Value = id;
                DataRow rrr = ConditionsTable.Rows[dgvConditions.CurrentRow.Index];
                rrr["role"] = id;
            }
        }

        void ConditionsTable_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
             if (!InitialFaza)
           AMASCommand.DeleteCondition((int)e.Row["id"]);
        }

        void dgvConditions_Click(object sender, EventArgs e)
        {
            TimeDelayAssign = false;
            if (dgvConditions.CurrentRow != CurrenConditionRow)
            {
                if (CurrenConditionRow != null) CurrenConditionRow.Selected = false;
                CurrenConditionRow = dgvConditions.CurrentRow;
                try
                {
                    RolesConditionListing.CheckById((int)CurrenConditionRow.Cells["role"].Value);
                    ExecutionConditionListing.CheckById((int)CurrenConditionRow.Cells["execution_"].Value);

                }
                catch { }
                CurrenConditionRow.Selected = true;
            }
            TimeDelayAssign = true;
            //InitialFaza = false;
        }

        void ConditionsTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            int id = -1;
            string descr = "";
            if (!InitialFaza)
            {

                int role__ = -1;
                int execution__ = -1;

                if (e.Row["id"].GetType().ToString().CompareTo("System.DBNull") == 0) id = -1; else id = (int)e.Row["id"];
                if (e.Row["description_"].GetType().ToString().CompareTo("System.DBNull") != 0) descr = (string)e.Row["description_"];
                if (e.Row["role"].GetType().ToString().CompareTo("System.DBNull") == 0) 
                {
                    //if(RolesConditionListing!=null)
                    //    RolesConditionListing.
                    role__ = -1; 
                }
                else role__ = (int)e.Row["role"];
                if (e.Row["execution_"].GetType().ToString().CompareTo("System.DBNull") == 0)
                {
                    execution__ = -1;
                }
                else execution__ = (int)e.Row["execution_"];

                try
                {
                    if (id < 1)
                    {
                        AMASCommand.InsertCondition((string)e.Row["name"], descr, role__, execution__);
                    }
                    else
                    {
                        AMASCommand.UpdateCondition((int)e.Row["id"], (string)e.Row["name"], descr, role__, execution__);
                    }
                }
                catch (Exception ex) { SybAcc.EBBLP.AddError(ex.Message, "BusinessProcess - 40", ex.StackTrace); }
            }
        }

        int A_minutes = 1;
        int A_Hours = 0;
        int A_Days = 0;
        bool TimeDelayAssign = false;

        void mtbMinutes_TextChanged(object sender, EventArgs e)
        {
            if (TimeDelayAssign)
            {
                try
                {
                    A_minutes = (int)Convert.ToInt32(mtbMinutes.Text);
                }
                catch { A_minutes = 0; }
                TimeDelayValue();
            }
        }

        void mtbhours_TextChanged(object sender, EventArgs e)
        {
            if (TimeDelayAssign)
            {
                try
                {
                    A_Hours = (int)Convert.ToInt32(mtbhours.Text);
                }
                catch { A_Hours = 0; }
                TimeDelayValue();
            }
        }

        void mtbDays_TextChanged(object sender, EventArgs e)
        {
            if (TimeDelayAssign)
            {
                try
                {
                    A_Days = (int)Convert.ToInt32(mtbDays.Text);
                }
                catch { A_Days = 0; }
                TimeDelayValue();
            }
        }

        private int TimeDelayValue()
        {
           int res= A_Days * 24 * 60 + A_Hours * 60 + A_minutes;
           dataGridViewEvents.CurrentRow.Cells["TimeDelay"].Value = res;
           return res;
        }

        CheckedListing TemaListing;
        CheckedListing KindListing;
        CheckedListing RoutesListing;
        CheckedListing RolesConditionListing;
        CheckedListing ExecutionConditionListing;

        void dataGridViewEvents_Click(object sender, EventArgs e)
        {
            TimeDelayAssign = false;
            mtbDays.Text = "";
            mtbhours.Text = "";
            mtbMinutes.Text = "";
            if (dataGridViewEvents.CurrentRow != CurrenEventedRow)
            {
                if (CurrenEventedRow != null) CurrenEventedRow.Selected = false;
                CurrenEventedRow = dataGridViewEvents.CurrentRow;
                try
                {
                    TemaListing.CheckById((int)CurrenEventedRow.Cells["tema"].Value);
                    KindListing.CheckById((int)CurrenEventedRow.Cells["kind"].Value);
                    RoutesListing.CheckById((int)CurrenEventedRow.Cells["route_"].Value);

                    int Timescale = (int)CurrenEventedRow.Cells["TimeDelay"].Value;

                    mtbDays.Text = (Timescale / 60 / 24).ToString();
                    mtbhours.Text = ((Timescale - (int)Convert.ToInt32(mtbDays.Text) * 24 * 60) / 60).ToString();
                    mtbMinutes.Text = (Timescale - (int)Convert.ToInt32(mtbDays.Text) * 24 * 60 - (int)Convert.ToInt32(mtbhours.Text) * 60).ToString();

                    EventExecutionList();
                }
                catch { }
                CurrenEventedRow.Selected = true;
            }
            TimeDelayAssign = true;
        }

        void EventExecutionList()
        {
            Class_syb_acc.PrepareParameters[] EventExecutors = new Class_syb_acc.PrepareParameters[1];
            EventExecutors[0] = new Class_syb_acc.PrepareParameters("@event_", SqlDbType.Int, (int)CurrenEventedRow.Cells["id"].Value);
            if (SybAcc.Set_table("BPEventsExecList", AMAS_Query.ClassAMAS_Buissnes_Process.event_execution_list, EventExecutors))
            {
                lvExecutions.Clear();
                for (int l = 0; l < SybAcc.Rows_count; l++)
                {
                    try
                    {
                        SybAcc.Get_row(l);
                        ListViewItem ItemIns = lvExecutions.Items.Add((string)SybAcc.Find_Field("name"));
                        ItemIns.SubItems.Add((string)Convert.ToString((int)SybAcc.Find_Field("id")));
                        if ((int)SybAcc.Find_Field("checked")==-1)
                            ItemIns.Checked = false;
                        else ItemIns.Checked = true;
                    }
                    catch (Exception ex) { SybAcc.EBBLP.AddError(ex.Message, "busynessprocess - 10", ex.StackTrace); }
                }
                SybAcc.ReturnTable();
            }

        }

        void EventsTable_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            if (!InitialFaza)
                AMASCommand.DeleteEvent((int)e.Row["id"]);
        }

        void EventsTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            int id = -1;
            string descr = "";
            if (!InitialFaza)
            {
                if (e.Row["id"].GetType().ToString().CompareTo("System.DBNull") == 0) id = -1; else id = (int)e.Row["id"];
                if (e.Row["description_"].GetType().ToString().CompareTo("System.DBNull") != 0) descr = (string)e.Row["description_"];

                try
                {
                    if (id < 1)
                    {
                        AMASCommand.InsertEvent((string)e.Row["name"], descr, (int)e.Row["kind"], (int)e.Row["tema"], (int)e.Row["route_"], (int)e.Row["TimeDelay"]);
                    }
                    else
                    {
                        AMASCommand.UpdateEvent((int)e.Row["id"], (string)e.Row["name"], descr, (int)e.Row["kind"], (int)e.Row["tema"], (int)e.Row["route_"], (int)e.Row["TimeDelay"]);
                    }
                }
                catch { }
            }
        }

        DataGridViewRow CurrenRoutedRow = null;
        DataGridViewRow CurrenEventedRow = null;
        DataGridViewRow CurrenConditionRow = null;

        void dataGridViewRoutes_Click(object sender, EventArgs e)
        {
            if (dataGridViewRoutes.CurrentRow != CurrenRoutedRow)
            {
                if (CurrenRoutedRow != null) CurrenRoutedRow.Selected = false;
                CurrenRoutedRow = dataGridViewRoutes.CurrentRow;
                CurrenRoutedRow.Selected = true;
                RecheckConnections();
            }
        }

        void RecheckConnections()
        {
            RolesTreeChecking = false;
            foreach (TreeNode Nd in tvExeRoles.Nodes)
            {
                Nd.Checked = false;
                foreach (TreeNode Nud in Nd.Nodes)
                    Nud.Checked = false;
            }
            try
            {
                AMAS_DBI.Class_syb_acc.PrepareParameters[] PreRoutes = new Class_syb_acc.PrepareParameters[1];
                PreRoutes[0] = new Class_syb_acc.PrepareParameters("@route", SqlDbType.Int, (int)CurrenRoutedRow.Cells["id"].Value);
                if (SybAcc.Set_table("BPRoutesConnList", AMAS_Query.ClassAMAS_Buissnes_Process.BPRouteConnectList, PreRoutes))
                {
                    foreach (DataRow Rrr in SybAcc.Current_table.Rows)
                        try
                        {
                            foreach (TreeNode nn in tvExeRoles.Nodes.Find("T" + Rrr["execution_"], true))
                                nn.Checked = true;
                        }
                        catch { }
                }
            }
            catch { }
            RolesTreeChecking = true;
        }

        void tvExeRoles_AfterCheck(object sender, TreeViewEventArgs e)
        {
            if (RolesTreeChecking)
                switch (e.Node.Name.Substring(0, 1).ToUpper())
                {
                    case "R":
                        if (e.Node.Nodes.Count > 0)
                            foreach (TreeNode NN in e.Node.Nodes)
                            {
                                if (NN.Name.Substring(0, 1).ToUpper().CompareTo("T") == 0)
                                    if (NN.Checked) NN.Checked = false; else NN.Checked = true;
                                if (e.Node.Checked)
                                {
                                    RolesTreeChecking = false;
                                    e.Node.Checked = false;
                                    RolesTreeChecking = true;
                                }
                            }
                        else
                            if (e.Node.Checked)
                            {
                                RolesTreeChecking = false;
                                e.Node.Checked = false;
                                RolesTreeChecking = true;
                            }
                            else
                            {
                                RolesTreeChecking = false;
                                e.Node.Checked = true;
                                RolesTreeChecking = true;
                            }
                        break;
                    case "T":
                        if (dataGridViewRoutes.SelectedRows.Count == 0)
                        {
                            if (e.Node.Checked)
                            {
                                RolesTreeChecking = false;
                                e.Node.Checked = false;
                                RolesTreeChecking = true;
                            }
                            else
                            {
                                RolesTreeChecking = false;
                                e.Node.Checked = true;
                                RolesTreeChecking = true;
                            }
                        }
                        else
                        {
                            if (e.Node.Checked)
                            {
                                //foreach (DataRow Dr  in  dataGridViewRoutes.SelectedRows )
                                for (int k = 0; k < dataGridViewRoutes.SelectedRows.Count; k++)
                                    if (AMASCommand.AddExecutionForRoute((int)Convert.ToInt32(e.Node.Name.Substring(1)), (int)dataGridViewRoutes.SelectedRows[k].Cells["id"].Value, tbConnect.Text, "") != true)
                                    {
                                        RolesTreeChecking = false;
                                        e.Node.Checked = false;
                                        RolesTreeChecking = true;
                                    }
                            }
                            else
                            {
                                //foreach (DataRow Dr in dataGridViewRoutes.SelectedRows)
                                for (int k = 0; k < dataGridViewRoutes.SelectedRows.Count; k++)
                                    if (AMASCommand.RemoveExecutionForRoute((int)Convert.ToInt32(e.Node.Name.Substring(1)), (int)dataGridViewRoutes.SelectedRows[k].Cells["id"].Value) != true)
                                    {
                                        RolesTreeChecking = false;
                                        e.Node.Checked = true;
                                        RolesTreeChecking = true;
                                    }
                            }
                        }
                        break;
                }
        }

        void RoutesTable_RowDeleting(object sender, DataRowChangeEventArgs e)
        {
            if (!InitialFaza)
            AMASCommand.DeleteBusinessRoute((int)e.Row["id"]);
        }

        void RoutesTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            int id = -1;
            string descr = "";
            if (!InitialFaza)
            {
                if (e.Row["id"].GetType().ToString().CompareTo("System.DBNull") == 0) id = -1; else id = (int)e.Row["id"];
                if (e.Row["description_"].GetType().ToString().CompareTo("System.DBNull") != 0) descr = (string)e.Row["description_"];

                if (id < 1)
                {
                    AMASCommand.InsertBusinessRoute((string)e.Row["name"], descr);
                }
                else
                {
                    AMASCommand.UpdateBusinessRoute((int)e.Row["id"], (string)e.Row["name"], descr);
                }
            }
        }

        private DataTable RoutesTable;
        private DataTable EventsTable;
        private DataTable ConditionsTable;
        private DataTable EventsExecTable;

        private void tsbShowRankInstructions_Click(object sender, EventArgs e)
        {
            if (lvInstructionsList.Visible)
            {
                lvInstructionsList.Visible = false;
            }
            else
            {
                lvInstructionsList.View = View.List;
                lvInstructionsList.GridLines = true;
                lvInstructionsList.MultiSelect = false;
                lvInstructionsList.FullRowSelect = true;
                lvInstructionsList.Sorting = System.Windows.Forms.SortOrder.Ascending;
                InstructList();
                lvInstructionsList.Visible = true;

                lvInstructionsList.BringToFront();
            }
        }


        public void Clear()
        {
            treeViewRoles.Nodes.Clear();
            tsslInstruction.Text = "";
        }

        public void ReQueryRoles()
        {
            tsslInstruction.Text = lvInstructionsList.SelectedItems[0].Text;
            int NumofInstruction = (int)Convert.ToInt32(lvInstructionsList.SelectedItems[1].SubItems[0].Text);
            RolesList.Requery(NumofInstruction);
        }

        private void tsbAddRole_Click(object sender, EventArgs e)
        {
            string NewRolName = "Новая роль к инструкции " + tsslInstruction.Text;
            int id = AMASCommand.AddBusinessRole((int)Convert.ToInt32(lvInstructionsList.SelectedItems[0].SubItems[1].Text), NewRolName, "");
            if (id > 0)
            {
                treeViewRoles.LabelEdit = true;
                RolesList.AddRole(NewRolName, id, "R");
            }
        }

        private void tsbRemoveRole_Click(object sender, EventArgs e)
        {
            RolesList.RemoveRole();

        }

        private void tsbUpdateRole_Click(object sender, EventArgs e)
        {
            try
            {
                treeViewRoles.LabelEdit = true;
                treeViewRoles.SelectedNode.BeginEdit();
            }
            catch { }
        }

        private void tsbAssignEventExecution_Click(object sender, EventArgs e)
        {
            int[] EvEx = new int[lvExecutions.CheckedItems.Count];
            for (int Lit = 0; Lit < lvExecutions.CheckedItems.Count; Lit++)
                EvEx[Lit] = (int) Convert.ToInt32( lvExecutions.CheckedItems[Lit].SubItems[1].Text);
            AMASCommand.SetEventExecution((int)dataGridViewEvents.CurrentRow.Cells["id"].Value, EvEx);
        }

        private void tsbAddExecution_Click(object sender, EventArgs e)
        {
            RolesList.AddTask();
        }

        private void tsbDeleteExecution_Click(object sender, EventArgs e)
        {
            RolesList.RemoveTask();
        }

        private void tsbEditExecution_Click(object sender, EventArgs e)
        {
            RolesList.EditTask();
        }     

    }

    public class RolesClass
    {
        private AMAS_DBI.Class_syb_acc SybAcc;
        private TreeView TVRoles;
        private TreeNode ParentNode;
        private RollNode[] Rolls = null;
        private RollNode selected_RolNod = null;
        private TasksList TasksList_ = null;
        private ListView LVM = null;
        ClassPattern.Editor EditDescr = null;
        PropertyGrid PpG = null;

        //public delegate void RoleChange(string RoleName)  ;
        //public event RoleChange RoleCH;

        public int Instriction_ = -1;
        public int CurrentTask { get { if (TasksList_ != null) return TasksList_.TaskId; else return -1; } }
        public RollNode[] GetRoles
        {
            get { return Rolls; }
        }

        public RolesClass(TreeView TV, TreeNode Node, ListView Lim, AMAS_DBI.Class_syb_acc ACC, ClassPattern.Editor EditDescr_, PropertyGrid PpG_)
        {
            TVRoles = TV;
            ParentNode = Node;
            SybAcc = ACC;
            LVM = Lim;
            setting();
            EditDescr = EditDescr_;
            PpG = PpG_;
        }

        public RolesClass(TreeView TV, TreeNode Node, ListView Lim, AMAS_DBI.Class_syb_acc ACC, ClassPattern.Editor EditDescr_, PropertyGrid PpG_, int instr)
        {
            TVRoles = TV;
            ParentNode = Node;
            SybAcc = ACC;
            LVM = Lim;
            Instriction_ = instr;
            setting();
            EditDescr = EditDescr_;
            PpG = PpG_;
        }

        private void setting()
        {
            if (TVRoles != null)
            {
                TVRoles.Nodes.Clear();
                TVRoles.AfterSelect += new TreeViewEventHandler(TVRoles_AfterSelect);
                selected_RolNod = GetSelectedRollNode();
                TVRoles.AfterLabelEdit += new NodeLabelEditEventHandler(TVRoles_AfterLabelEdit);
                if (LVM != null)
                {
                    LVM.View = View.Details;
                    LVM.Columns.Clear();
                    LVM.Columns.Add("Tasks", "Задачи");
                    LVM.Columns[0].Width = LVM.Width - 2;
                }
            }
        }

        public void AddTask()
        {
            string TaskName = "Новая задача для роли " + selected_RolNod.NName ?? "";
            if (SelectedNodID > 0)
            {
                int id = AMASCommand.AddBusinessTask(SelectedNodID, TaskName, 60);
                if (id > 0)
                    if (selected_RolNod.ID > 0)
                    {
                        if (TasksList_ == null)
                            TasksList_ = new TasksList(LVM, SybAcc, selected_RolNod.ID, EditDescr, PpG);
                        TasksList_.AddTask(TaskName, id, 0, 0, 0, 1, 0);
                    }
                    else MessageBox.Show("Выберите роль");
            }
        }

        public void RemoveTask()
        {
            TasksList_.RemoveTask();
        }

        public void EditTask()
        {
            TasksList_.TaskEdit();
        }

        private void UpdateRoleName(string name)
        {
            if (selected_RolNod != null)
                AMASCommand.UpdateBusinessRole(selected_RolNod.ID, name, "");
            //RoleCH(name);
        }

        void TVRoles_AfterLabelEdit(object sender, NodeLabelEditEventArgs e)
        {
            UpdateRoleName(e.Label);
        }

        void TVRoles_AfterSelect(object sender, TreeViewEventArgs e)
        {
            selected_RolNod = GetSelectedRollNode();
            LVM.Items.Clear();
            if (SelectedNodID > 0)
                TasksList_ = new TasksList(LVM, SybAcc, SelectedNodID, EditDescr, PpG);
            //RoleCH(selected_RolNod.NName);
        }

        private RollNode GetSelectedRollNode()
        {
            RollNode RNd = null;
            if (TVRoles.SelectedNode != null)
                try
                {
                    for (int i = 0; i < Rolls.Length; i++)
                        if (Rolls[i].NetKod.CompareTo(TVRoles.SelectedNode.Name) == 0)
                        {
                            RNd = Rolls[i];
                            break;
                        }
                }
                catch { RNd = null; }
            return RNd;
        }

        public int AddRole(string name, int id, string kod)
        {
            int index = GetRole(name, id, kod);
            TVRoles.LabelEdit = true;
            selected_RolNod.BeginEdit();
            return index;
        }

        private int GetRole(string name, int id, string kod)
        {
            int index = -1;
            if (Rolls == null)
            {
                Rolls = new RollNode[1];
                index = 0;
            }
            else
            {
                RollNode[] reRolls = new RollNode[Rolls.Length + 1];
                for (int i = 0; i < Rolls.Length; i++)
                    reRolls[i] = Rolls[i];
                index = Rolls.Length;
                Rolls = reRolls;
            }
            if (index >= 0)
            {
                Rolls[index] = new RollNode(name, kod, id);
                selected_RolNod = Rolls[index];
                TreeNode Ten = selected_RolNod as TreeNode;
                if (TVRoles != null)
                    Ten = TVRoles.Nodes.Add(Rolls[index].NetKod, Rolls[index].NName);
            }
            return index;
        }

        public int SelectedNodID
        {
            get
            {
                try
                {
                    return selected_RolNod.ID;
                }
                catch { return -1; }
            }
        }

        public string SelectedNodName
        {
            get
            {
                try
                {
                    return selected_RolNod.Name;
                }
                catch { return ""; }
            }
        }

        public string SelectedNodKod
        {
            get
            {
                try
                {
                    return selected_RolNod.Kod;
                }
                catch { return ""; }
            }
        }

        public void RemoveRole(int ID = -1)
        {
            RollNode[] reRolls;
            RollNode SelNode = GetSelectedRollNode();

            if (ID == -1 && SelNode != null) ID = SelNode.ID;
            if (AMASCommand.RemoveBusinessRole(ID))
                if (ID > 0)
                {
                    if (Rolls.Length == 1)
                    {
                        Rolls = null;
                    }
                    else
                    {
                        int lil = 0;
                        for (int i = 0; i < Rolls.Length; i++)
                            if (Rolls[i].ID != ID)
                            {
                                Rolls[lil] = Rolls[i];
                                lil++;
                            }
                            else
                            {
                                if (TVRoles != null) TVRoles.Nodes.RemoveByKey(Rolls[i].NetKod);
                            }
                        reRolls = new RollNode[lil];
                        for (int ii = 0; ii < lil; ii++)
                            reRolls[ii] = Rolls[ii];

                        Rolls = reRolls;
                    }
                }
        }

        public void Requery(int NumofInstruction)
        {
            Instriction_ = NumofInstruction;
            Rolls = null;
            AMAS_DBI.Class_syb_acc.PrepareParameters[] Parameters = new Class_syb_acc.PrepareParameters[1];
            Parameters[0] = new Class_syb_acc.PrepareParameters("@instr", SqlDbType.Int, (object)NumofInstruction);
            try
            {
                if (SybAcc.Set_table("BPRoles", AMAS_Query.ClassAMAS_Buissnes_Process.RoulesList, Parameters))
                {
                    for (int l = 0; l < SybAcc.Rows_count; l++)
                    {
                        SybAcc.Get_row(l);
                        GetRole((string)SybAcc.Find_Field("name"), (int)SybAcc.Find_Field("id"), "R");
                    }
                    SybAcc.ReturnTable();
                }
            }
            catch
            {
            }
        }
    }

    public class RollNode : TreeNode
    {
        private string Noname = "";
        private int NoID = 0;
        private string NoKod = "";
        public string NetKod = "";

        public int ID { get { return NoID; } }
        public string Kod { get { return NoKod; } }
        public string NName { get { return Noname; } }

        public RollNode(string name, string kod, int index)
        {
            NetKod = kod + index.ToString();
            Noname = name;
            NoKod = kod;
            NoID = index;
        }
    }

    public class TasksList
    {
        private AMAS_DBI.Class_syb_acc SybAcc;
        Disition[] ListOfTasts = null;
        ListView ListViewTasks = null;
        private int Role_ = -1;
        Disition SelectedTask_ = null;
        PropertyGrid PpG = null;
        public int TaskId { get { if (SelectedTask_ != null) return SelectedTask_.Id(); else return -1; } }

        private ClassPattern.Editor DescEditor;

        public TasksList(ListView LVT, AMAS_DBI.Class_syb_acc Acc, int _role, ClassPattern.Editor Edt, PropertyGrid PpG_)
        {
            ListViewTasks = LVT;
            SybAcc = Acc;
            Role_ = _role;
            DescEditor = Edt;
            PpG = PpG_;

            if (Role_ > 0) Refresh(Role_);

            if (ListViewTasks != null)
            {
                ListViewTasks.SelectedIndexChanged += new EventHandler(LVT_SelectedIndexChanged);
                ListViewTasks.AfterLabelEdit += new LabelEditEventHandler(ListViewTasks_AfterLabelEdit);
            }
            if (DescEditor != null)
            {
                DescEditor.Leave += new EventHandler(DescEditor_Leave);
            }
        }

        void DescEditor_Leave(object sender, EventArgs e)
        {
        }

        public void TaskEdit()
        {
        }

        void ListViewTasks_AfterLabelEdit(object sender, LabelEditEventArgs e)
        {
            try
            {
                if (SelectedTask_.Id() > 0)
                {
                    SelectedTask_.____Имя_Задачи = e.Label;
                    ListView Liv = sender as ListView;
                    if (Liv != null) Liv.LabelEdit = false;
                }
            }
            catch { }
        }

        void LVT_SelectedIndexChanged(object sender, EventArgs e)
        {
            string res = "";
            if (ListOfTasts != null)
            {
                try
                {
                    if (SelectedTask_ != null)
                        if (DescEditor.Edited)
                        {
                            string Flnm = DescEditor.SaveToFile();
                            AMASCommand.UpdateBusinessTaskDescription(SelectedTask_.Id(), File.ReadAllText(Flnm));
                        }
                }
                catch { }
                try
                {
                    foreach (Disition dis in ListOfTasts)
                        foreach (ListViewItem IT in ListViewTasks.SelectedItems)
                            if (dis.Compare(IT.Name))
                            {
                                SelectedTask_ = dis;
                                break;
                            }
                    DescEditor.Clear();
                    string Flnm_ = CommonClass.TempDirectory + "TaskDescr.rtf";
                    string[] strdesc = new string[1];
                    strdesc[0] = (string)AMASCommand.GetTaskDescription(SelectedTask_.Id());
                    File.WriteAllLines(Flnm_, strdesc);
                    DescEditor.LoadFromFile(Flnm_);
                }
                catch (Exception ex) { res = ex.Message; }
                    PpG.SelectedObject = SelectedTask_;
            }
        }

        public void RemoveTask()
        {
            try
            {
                if (SelectedTask_ != null)
                {
                    string Flnm = DescEditor.SaveToFile();
                    AMASCommand.RemoveBusinessTask (SelectedTask_.Id());
                }
            }
            catch { }
        }

        public int AddTask(string name, int id, int viza, int kind, int tema, int minutes, int Execs)
        {
            int index = -1;
            string name_ = name;
            int id_ = id;
            int viza_ = viza;
            int kind_ = kind;
            int tema_ = tema;
            int minutes_ = minutes;
            int Execs_ = Execs;
            index = GetTask(name_, id_, viza_, kind_, tema_, minutes_, Execs_);
            if (index >= 0)
                SelectedTask_.BeginEdit();
            return index;
        }

        private int GetTask(string name, int id, int viza, int kind, int tema, int minutes, int Execs)
        {
            int index = -1;
            if (ListOfTasts == null)
            {
                ListOfTasts = new Disition[1];
                index = 0;
            }
            else
            {
                Disition[] reTasks = new Disition[ListOfTasts.Length + 1];
                for (int i = 0; i < ListOfTasts.Length; i++)
                    reTasks[i] = ListOfTasts[i];
                index = ListOfTasts.Length;
                ListOfTasts = reTasks;
            }
            if (index >= 0)
            {
                if (ListViewTasks != null)
                {
                    switch (Execs)
                    {
                        case (int)Taskie.Исполнение:
                            ListOfTasts[index] = new Disition(name, id, kind, tema, minutes, SybAcc);
                            break;
                        case (int)Taskie.Визирование:
                            ListOfTasts[index] = new Disition(name, id, viza, minutes, SybAcc);
                            break;
                        case (int)Taskie.Ознакомление:
                            ListOfTasts[index] = new Disition(name, id, minutes, SybAcc);
                            break;
                    }
                    SelectedTask_ = ListOfTasts[index];
                    ListViewItem Ten = ListViewTasks.Items.Add("T" + ListOfTasts[index].Id().ToString(), ListOfTasts[index].____Имя_Задачи, "Image1");
                    SelectedTask_.Assign(Ten);
                }

            }
            return index;
        }

        public void Refresh(int Role)
        {
            Role_ = Role;
            ListOfTasts = null;
            AMAS_DBI.Class_syb_acc.PrepareParameters[] Parameters = new Class_syb_acc.PrepareParameters[1];
            Parameters[0] = new Class_syb_acc.PrepareParameters("@Role", SqlDbType.Int, (object)Role);
            try
            {
                if (SybAcc.Set_table("BPTasks", AMAS_Query.ClassAMAS_Buissnes_Process.BPTasksList, Parameters))
                {
                    string OName = "";
                    for (int l = 0; l < SybAcc.Rows_count; l++)
                    {
                        SybAcc.Get_row(l);
                        int id = (int)SybAcc.Find_Field("id");
                        int viza = 0;
                        OName = SybAcc.Find_Field("viza").GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) viza = (int)SybAcc.get_current_Field();
                        int kind = 0;
                        OName = SybAcc.Find_Field("kind").GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) kind = (int)SybAcc.get_current_Field();
                        int tema = 0;
                        OName = SybAcc.Find_Field("tema").GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) tema = (int)SybAcc.get_current_Field();
                        int minutes = 0;
                        OName = SybAcc.Find_Field("minutes").GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) minutes = (int)SybAcc.get_current_Field();
                        int executing = 0;
                        OName = SybAcc.Find_Field("executing_").GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) executing = (int)SybAcc.get_current_Field();

                        GetTask((string)SybAcc.Find_Field("name"), id, viza, kind, tema, minutes, executing);
                    }
                    SybAcc.ReturnTable();
                }
            }
            catch (Exception e)
            {
                SybAcc.EBBLP.AddError(e.Message, "Business Process - 20", e.StackTrace);
            }
        }
    }

    public enum Taskie { Исполнение = 0, Визирование, Ознакомление };

    public class Disition
    {
        private AMAS_DBI.Class_syb_acc SybAcc;
        private int minutes_ = 0;
        private int kind_ = 0;
        private int tema_ = 0;
        private int viza_ = 0;
        private ListViewItem TaskListItem = null;
        private Taskie task_ = 0;
        private int Id_ = -1;
        private string name_ = "";

        /// <summary>
        /// ___Дней
        /// срок исполнения - дней
        /// </summary>
        public int ___Дней
        {
            get { return (int)minutes_ / (24 * 60); }
            set { int H = ___Часов; int M = __Минут; minutes_ = value * (24 * 60) + M * 60 + M; }
        }
        /// <summary>
        /// ___Часов
        /// срок исполнения - часов (не более 23 часов)
        /// </summary>
        public int ___Часов
        {
            get { return (int)(minutes_ - ___Дней * 24 * 60) / 60; }
            set { int D = ___Дней; int M = __Минут; minutes_ = D * (24 * 60) + value * 60 + M; }
        }
        /// <summary>
        /// __Минут
        /// срок исполнения - минут (не более 59 минут)
        /// </summary>
        public int __Минут
        {
            get { return (int)minutes_ - ___Дней * 24 * 60 - ___Часов * 60; }
            set { int D = ___Дней; int H = ___Часов; minutes_ = D * (24 * 60) + H * 60 + value; }
        }
        /// <summary>
        /// _Вид_документа
        /// назначение документа в качестве ответа на полученное задание
        /// </summary>

        string myStr1;

        [TypeConverter(typeof(MyConverterVID))]
        public string _Вид_документа
        {

            get { return myStr1; }

            set
            {
                myStr1 = value;
                if (VID_ != null)
                    foreach (CommonClass.Arraysheet ah in VID_)
                        if (ah.Name.Trim().CompareTo(value.Trim()) == 0)
                            kind_ = (int)Convert.ToInt32(ah.Id);
                AMASCommand.SetBusinessTaskExecuting(Id_, kind_, tema_);
            }
        }

        List<CommonClass.Arraysheet> list1;
        ArrayList VID_;
        [Browsable(false)]
        public List<CommonClass.Arraysheet> VID
        {
            get
            {
                if (list1 == null)
                {
                    list1 = new List<CommonClass.Arraysheet>();
                    foreach (CommonClass.Arraysheet ah in VID_)
                        list1.Add(ah);
                }
                return list1;
            }
        }

        /// <summary>
        /// _Тематика
        /// выбор ии изменение темы ответа
        /// </summary>

        string myStr2;

        [TypeConverter(typeof(MyConverterTema))]
        public string _Тематика
        {

            get { return myStr2; }

            set
            {
                myStr2 = value;
                if (Tema_ != null)
                    foreach (CommonClass.Arraysheet ah in Tema_)
                        if (ah.Name.Trim().CompareTo(value.Trim()) == 0)
                            tema_ = (int)Convert.ToInt32(ah.Id);
                AMASCommand.SetBusinessTaskExecuting(Id_, kind_, tema_);

            }
        }

        List<CommonClass.Arraysheet> list2;
        ArrayList Tema_;
        [Browsable(false)]
        public List<CommonClass.Arraysheet> Tema
        {
            get
            {
                if (list2 == null)
                {
                    list2 = new List<CommonClass.Arraysheet>();
                    foreach (CommonClass.Arraysheet ah in Tema_)
                        list2.Add(ah);
                }
                return list2;
            }
        }

        /// <summary>
        /// Виза
        /// выбор пары да-нет для визирования
        /// </summary>

        string myStr3;

        [TypeConverter(typeof(MyConverterViza))]
        public string Виза
        {

            get { return myStr3; }

            set
            {
                myStr3 = value;
                if (Vizza_ != null)
                    foreach (CommonClass.Arraysheet ah in Vizza_)
                        if (ah.Name.Trim().CompareTo(value.Trim()) == 0)
                            viza_ = (int)Convert.ToInt32(ah.Id);
                AMASCommand.SetBusinessTaskVizing(Id_, viza_);
            }
        }

        List<CommonClass.Arraysheet> list3;
        ArrayList Vizza_;
        [Browsable(false)]
        public List<CommonClass.Arraysheet> Vizza
        {
            get
            {
                if (list3 == null)
                {
                    list3 = new List<CommonClass.Arraysheet>();
                    foreach (CommonClass.Arraysheet ah in Vizza_)
                        list3.Add(ah);
                }
                return list3;
            }
        }

        /// <summary>
        /// ____Имя_Задачи
        /// название задания для роли
        /// </summary>
        public string ____Имя_Задачи { get { return name_; } set { name_ = value; AMASCommand.UpdateBusinessTaskName(Id_, name_); if (TaskListItem != null)TaskListItem.Text = name_; } }
        public Taskie ____Задание
        {
            get { return task_; }
            set
            {
                task_ = value;
                switch (task_)
                {
                    case Taskie.Визирование:
                        tema_ = -1;
                        kind_ = -1;
                        break;
                    case Taskie.Исполнение:
                        viza_ = -1;
                        break;
                    case Taskie.Ознакомление:
                        tema_ = -1;
                        kind_ = -1;
                        viza_ = -1;
                        break;

                }
            }
        }

        public void BeginEdit()
        {
            TaskListItem.ListView.LabelEdit = true;
            TaskListItem.BeginEdit();
        }

        public Disition(string _name, int _id, int _kind, int _tema, int _minutes, AMAS_DBI.Class_syb_acc Acc)
        {
            SybAcc = Acc;
            name_ = _name;
            Id_ = _id;
            kind_ = _kind;
            tema_ = _tema;
            minutes_ = _minutes;
            task_ = 0;
            setting();
        }

        public Disition(string _name, int _id, int _viza, int _minutes, AMAS_DBI.Class_syb_acc Acc)
        {
            SybAcc = Acc;
            name_ = _name;
            Id_ = _id;
            viza_ = _viza;
            minutes_ = _minutes;
            task_ = 0;
            setting();
        }

        public Disition(string _name, int _id, int _minutes, AMAS_DBI.Class_syb_acc Acc)
        {
            SybAcc = Acc;
            name_ = _name;
            Id_ = _id;
            minutes_ = _minutes;
            task_ = 0;
            setting();
        }

        private void setting()
        {
            try
            {
                VID_ = AMASCommand.A_resolutions_list((int)CommonClass.Lists.Kind);
                foreach (CommonClass.Arraysheet ah in VID_)
                    if ((int)Convert.ToInt32(ah.Id) == kind_) myStr1 = ah.Name;
            }
            catch (Exception ex)
            {
                SybAcc.EBBLP.AddError(ex.Message, "Busyprocess - 20.1", ex.StackTrace);
            }

            try
            {
                Tema_ = AMASCommand.A_resolutions_list((int)CommonClass.Lists.Tema);
                foreach (CommonClass.Arraysheet ah in Tema_)
                    if ((int)Convert.ToInt32(ah.Id) == tema_) myStr2 = ah.Name;
            }
            catch (Exception ex)
            {
                SybAcc.EBBLP.AddError(ex.Message, "Busyprocess - 20.2", ex.StackTrace);
            }

            try
            {
                Vizza_ = AMASCommand.A_resolutions_list((int)CommonClass.Lists.Viza);
                foreach (CommonClass.Arraysheet ah in Vizza_)
                    if ((int)Convert.ToInt32(ah.Id) == viza_) myStr3 = ah.Name;
            }
            catch (Exception ex)
            {
                SybAcc.EBBLP.AddError(ex.Message, "Busyprocess - 20.3", ex.StackTrace);
            }
        }

        public void Assign(ListViewItem TaskListItem_)
        {
            TaskListItem = TaskListItem_;
        }

        public bool Compare(string str)
        {
            bool res = false;
            try
            {
                if (TaskListItem.Name.ToLower().Trim().CompareTo(str.ToLower().Trim()) == 0) res = true; else res = false;
            }
            catch { }
            return res;
        }

        public int Id() { return Id_; }

        class MyConverterVID : TypeConverter
        {

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {

                return true;

            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {

                List<CommonClass.Arraysheet> list1 = (context.Instance as Disition).VID;

                StandardValuesCollection cols = new StandardValuesCollection(list1);

                return cols;

            }

        }

        class MyConverterTema : TypeConverter
        {

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {

                return true;

            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {

                List<CommonClass.Arraysheet> list2 = (context.Instance as Disition).Tema;

                StandardValuesCollection cols = new StandardValuesCollection(list2);

                return cols;

            }

        }

        class MyConverterViza : TypeConverter
        {

            public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
            {

                return true;

            }

            public override StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
            {

                List<CommonClass.Arraysheet> list3 = (context.Instance as Disition).Vizza;

                StandardValuesCollection cols = new StandardValuesCollection(list3);

                return cols;

            }
        }
    }

    class CheckedListing
    {
        public System.Windows.Forms.ListView listViewCheckup;

        // Declare the delegate 
        public delegate void CheckId(int id);

        // Declare the event.
        public event CheckId CheckIdEvent;

        private AMAS_DBI.Class_syb_acc SybAcc;
        private string name;
        private string id;
        private bool CheckEvent = false;

        public CheckedListing(AMAS_DBI.Class_syb_acc Acc, string sql, string name_, string id_)
        {
            SybAcc = Acc;
            name = name_;
            id = id_;

            listViewCheckup = new System.Windows.Forms.ListView();
            listViewCheckup.CheckBoxes = true;
            listViewCheckup.Dock = System.Windows.Forms.DockStyle.Fill;
            listViewCheckup.Location = new System.Drawing.Point(3, 18);
            listViewCheckup.MultiSelect = false;
            listViewCheckup.Name = "listViewCheckup" + name_;
            listViewCheckup.Size = new System.Drawing.Size(410, 172);
            listViewCheckup.TabIndex = 0;
            listViewCheckup.UseCompatibleStateImageBehavior = false;
            listViewCheckup.View = System.Windows.Forms.View.List;

            if (SybAcc.Set_table("BPCheckedListing" + name_, sql, null))
            {
                for (int l = 0; l < SybAcc.Rows_count; l++)
                {
                    SybAcc.Get_row(l);
                    ListViewItem ItemIns = listViewCheckup.Items.Add((string)SybAcc.Find_Field(name));
                    ItemIns.SubItems.Add((string)Convert.ToString((int)SybAcc.Find_Field(id)));
                }
                SybAcc.ReturnTable();
            }
            listViewCheckup.ItemChecked += new ItemCheckedEventHandler(listViewCheckup_ItemChecked);
            CheckEvent = true;
         }

        void listViewCheckup_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (CheckEvent)
            {
                CheckEvent = false;
                for (int i = 0; i < listViewCheckup.Items.Count; i++)
                    if (listViewCheckup.Items[i].SubItems[1].Text.ToLower().CompareTo(e.Item.SubItems[1].Text.ToLower()) != 0)
                        listViewCheckup.Items[i].Checked = false;
                    else CheckIdEvent((int)Convert.ToInt32(e.Item.SubItems[1].Text));
                CheckEvent = true;
            }
        }

        public int GetCheckedId()
        {
            int res = -1;
            for (int i = 0; i < listViewCheckup.Items.Count; i++)
                if (listViewCheckup.Items[i].Checked)
                    res = (int)Convert.ToInt32(listViewCheckup.Items[i].SubItems[1].Text);
            return res;
        }

        public void CheckById(int id)
        {
            CheckEvent = false;
            for (int i = 0; i < listViewCheckup.Items.Count; i++)
                if (listViewCheckup.Items[i].SubItems[1].Text.CompareTo(id.ToString()) == 0)
                    listViewCheckup.Items[i].Checked = true;
                else listViewCheckup.Items[i].Checked = false;
            CheckEvent = true;
        }

    }
}
 