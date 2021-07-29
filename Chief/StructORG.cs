using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;
using ClassStructure;
using Chief.baseLayer;
using ClassErrorProvider;

namespace Chief
{
    public partial class StructORG : Form
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private ClassStructure.Structure StructMyOrg;
        private ClassStructure.Structure StructForInstructions;
        private ClassStructure.Structure StructWithDegree;
        private degree_ids Degree_Listing;
        private TreeNode thisNode = null;
        private TreeNode instrNode = null;
        public int  ModuleId ;

        private ChefSettings frmSettings1 = new ChefSettings();

        ArrayList MetaInstruct = new ArrayList();
        
        public StructORG(AMAS_DBI.Class_syb_acc ACC)
        {
            InitializeComponent();
            AMASacc = ACC;
            tabStructure.SelectedIndexChanged+=new EventHandler(tabStructure_SelectedIndexChanged);
            ADDdegreebox.LostFocus += new EventHandler(ADDdegreebox_Enter);
            contextMenuStrip1.ItemClicked+=new ToolStripItemClickedEventHandler(contextMenuStrip1_ItemClicked);
            treeViewDepts.AfterSelect+=new TreeViewEventHandler(treeViewDepts_AfterSelect);
            treeViewOrgInstr.AfterSelect+=new TreeViewEventHandler(treeViewOrgInstr_AfterSelect);
            panelinstruction.Resize+=new EventHandler(panelinstruction_Resize);
            this.Load+=new EventHandler(StructORG_Load);
            this.FormClosed+=new FormClosedEventHandler(StructORG_FormClosed);
            ModuleId = (int)ClassErrorProvider.ErrorBBLProvider.Modules.Structure;
        }

        private void StructORG_Load(Object sender, EventArgs e)
        {
            frmSettings1.SettingsKey = "Structure";
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
        }

        private void StructORG_FormClosed(Object sender, FormClosedEventArgs e)
        {
            frmSettings1.Save();
        }

        private void panelinstruction_Resize(object sender, EventArgs e)
        {
            textBoxInstruction.Top = panelinstruction.Top + (panelinstruction.Height - textBoxInstruction.Height);
            buttonAddinstruction.Top = textBoxInstruction.Top;
            buttonAddinstruction.Left = panelinstruction.Left + (panelinstruction.Width - buttonAddinstruction.Width);
            textBoxInstruction.Left = panelinstruction.Left;
            textBoxInstruction.Width = panelinstruction.Width - buttonAddinstruction.Width;
            listInstructions.Width = panelinstruction.Width;
            listInstructions.Left = panelinstruction.Left;
            listInstructions.Top = panelinstruction.Top;
            listInstructions.Height = panelinstruction.Height - textBoxInstruction.Height;
        }

        private void treeViewOrgInstr_AfterSelect(object sender, TreeViewEventArgs e)
        {
            instrNode = e.Node;
            StructForInstructions.Instruction_list(instrNode,listInstructions,MetaInstruct);
        }

        private void treeViewDepts_AfterSelect(object sender, TreeViewEventArgs e)
        {
            thisNode = e.Node;
            degrees_in_dep(StructMyOrg.select_dep(thisNode));
        }

            private void degrees_in_dep(long dep)
            {
                try
                {
                    string nnn = "";
                    string fio = "";
                    int leader = 0;
                    int mankikon = 21;
                    int cod=0;
                    listViewDegDep.Items.Clear();
                    AMASacc.Set_table("TStrORG1", AMAS_Query.Class_AMAS_Query.Degrees_in_Dep(dep),null);
                    for (int l = 0; l < AMASacc.Rows_count; l++)
                    {
                        AMASacc.Get_row(l);
                        nnn = "E" + (string)Convert.ToString(AMASacc.Find_Field("cod"));
                        try
                        {
                            fio = (string)AMASacc.Find_Field("fio");
                            AMASacc.Find_Field("leader");
                            leader = (int)AMASacc.Find_Field("leader");
                            if (fio.Trim().CompareTo("?") == 0) mankikon = 22;
                            else mankikon = 23;
                            if (leader == 1) mankikon += 2;
                            ListViewItem DD = listViewDegDep.Items.Add(nnn, (string)AMASacc.Find_Field("name") + " " + fio, mankikon);
                            cod=(int)AMASacc.Find_Field("cod");
                            DD.SubItems.Add(cod.ToString());
                            DD.ForeColor = Color.DarkViolet;
                        }
                        catch (Exception e) 
                        {
                            AMASacc.EBBLP.AddError(e.Message, "StructOrg - 1", e.StackTrace);
                            //Console.WriteLine(e.Message);
                        }
                    }
                }
                catch { }
                AMASacc.ReturnTable();
            }

        private void contextMenuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            switch (e.ClickedItem.Name)
            {
                case "add":
                    addDegree();
                    break;
                case "del":
                    Degree_Listing.delete_degree();
                    break;
                case "ren":
                    renameDegree();
                    break;
                case "loc":
                    loc_degree();
                    break;
            }
        }

        Groups groups_listing;
        private void tabStructure_SelectedIndexChanged(object sender, EventArgs e)
        {
            seTAb( (TabControl)sender);
        }

        private void seTAb(TabControl TAS)
        {
                    switch (TAS.SelectedTab.Name)
            {
                case "PageDegrees":
                    break;
                case "PageGroups":
                    StructWithDegree = new Structure(AMASacc, treeViewDeptWithDegrees, true);
                    treeViewDeptWithDegrees.ExpandAll();
                    treeViewDeptWithDegrees.CheckBoxes = true;
                    groups_listing = new Groups(AMASacc, treeViewGroups,false);
                    break;
                case "PageStructure":
                    StructMyOrg = new Structure(AMASacc, treeViewDepts, false);
                    StructMyOrg.Drag_drop = true;
                    Degree_Listing = new degree_ids(AMASacc, Degreelist);
                    listViewDegDep.Items.Clear();
                    thisNode = null; 
                    break;
                case "PageInstructions":
                    StructForInstructions = new Structure(AMASacc, treeViewOrgInstr, true);
                    break;
            }
        }

        private void Structure_Load(object sender, EventArgs e)
        {
            tabStructure.SelectedIndex = 0;
            seTAb(tabStructure);
            Degree_Listing = new degree_ids(AMASacc, Degreelist);
            listViewDegDep.Items.Clear();
        }

        private void Expandsubstr(TreeNode node)
        {
            try
            {
                node.ExpandAll();
            }
            catch { }
        }

        private void óäàëèòüÎòäåëToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StructMyOrg.delete_organization();
        }

        private class degree_ids
        {
            int Array_Dimention = 0;
            int CURNUM = 0;
            int Current_Number { get { return CURNUM; } set { CURNUM = value; Degreelist.SelectedIndex = Index[CURNUM]; } }
            int[] Index = null;
            int[] Ident = null;
            string[] Naming = null;

            private System.Windows.Forms.ListBox Degreelist;
            private AMAS_DBI.Class_syb_acc AMASacc;

            public degree_ids(AMAS_DBI.Class_syb_acc SybAcc, System.Windows.Forms.ListBox list)
            {
                AMASacc = SybAcc;
                Degreelist = list;
                Degreelist.ForeColor = Color.DarkViolet;
                refresh_list();
            }
            private void refresh_list()
            {
                try
                {
                    if (AMASacc.Set_table("TStrORG2", AMAS_Query.Class_AMAS_Query.SeekDegree, null))
                    {
                        string OName = "";
                        Degreelist.Items.Clear();
                        Array_Dimention = AMASacc.Rows_count;
                        Index = new int[Array_Dimention];
                        Ident = new int[Array_Dimention];
                        Naming = new string[Array_Dimention];


                        for (int i = 0; i < Array_Dimention; i++)
                        {
                            AMASacc.Get_row(i);
                            AMASacc.Find_Field("Name");
                            OName = AMASacc.get_current_Field().GetType().ToString();
                            if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                            else OName = "";
                            Naming[i] = OName;
                            Ident[i] = (int)AMASacc.Find_Field("degree");
                            Index[i] = Degreelist.Items.Add(Naming[i]);
                        }

                    }
                }
                catch (Exception e) 
                {
                    AMASacc.EBBLP.AddError(e.Message, "StructOrg - 2", e.StackTrace);
                }
            }

            public void add_degree(string name)
            {
                int id = AMASCommand.Add_Degree(name);
                if (id > 0)
                {
                    refresh_list();
                }
            }

            public void rename_degree(string name)
            {
                int id = get_ident();
                if (id >= 0)
                    if (AMASCommand.Rename_Degree(id, name))
                    {
                        Degreelist.Items[Index[CURNUM]] = name;
                        Naming[CURNUM] = name;
                    }
            }

            public void delete_degree()
            {
                int id = get_ident();
                if (id >= 0)
                if(AMASCommand.Erise_Degree(id))
                    refresh_list();
            }

            public int get_ident()
            {
                string ResultErr = "";
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == Degreelist.SelectedIndex)
                        {
                            Current_Number = i;
                            return Ident[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "StructOrg - 3", err.StackTrace);
                    ResultErr = err.Message; return -2;
                }
                return -1;
            }

            public string get_name()
            {
                string ResultErr = "";
                int i;
                try
                {
                    for (i = 0; i < Array_Dimention; i++)
                    {
                        if (Index[i] == Degreelist.SelectedIndex)
                        {
                            Current_Number = i;
                            return Naming[i];
                        }
                    }
                }
                catch (Exception err) 
                {
                    AMASacc.EBBLP.AddError(err.Message, "StructOrg - 4", err.StackTrace);
                    ResultErr = err.Message; return "";
                }
                return "";
            }
        }

        private bool ad_state = true;

        private void addDegree()
        {
            ad_state = true;
            int ind= Degreelist.Items.Add("äîëæíîñòü");
            if (ind >= 0)
            //if (Degreelist.SelectedIndex > 0)
            {
                Degreelist.SelectedIndex = ind;
                ADDdegreebox.Top = Degreelist.GetItemRectangle(Degreelist.SelectedIndex).Top;
            }
            else
                ADDdegreebox.Top = Degreelist.Top;
            ADDdegreebox.Width = Degreelist.Width;
            ADDdegreebox.Visible = true;
            ADDdegreebox.Text = "";
            ADDdegreebox.Focus();
        }

        private void renameDegree()
        {
            ad_state = false; 
            int id = Degree_Listing.get_ident();
            if (id >= 0)
            {
                ADDdegreebox.Top = Degreelist.GetItemRectangle(Degreelist.SelectedIndex).Top;
                ADDdegreebox.Width = Degreelist.Width;
                ADDdegreebox.Visible = true;
                ADDdegreebox.Text = Degree_Listing.get_name();
                ADDdegreebox.Focus();
            }
        }

        private void ADDdegreebox_Enter(object sender, EventArgs e)
        {
            if(ad_state)
                Degree_Listing.add_degree(ADDdegreebox.Text.Trim());
            else
                Degree_Listing.rename_degree(ADDdegreebox.Text.Trim());
            ADDdegreebox.Visible = false;
        }

        private void loc_degree()
        {
            if (thisNode != null)
                if (AMASCommand.Locate_Degree(StructMyOrg.select_dep(thisNode), Degree_Listing.get_ident()))
                    degrees_in_dep(StructMyOrg.select_dep(thisNode));
        }

        private TreeNode grpNode = null;

        private void buttonAddinstruction_Click(object sender, EventArgs e)
        {
            append_instruction();
        }

        private void append_instruction ()
        {
            if (instrNode != null)
            {
                int ident = (int)Convert.ToInt32(instrNode.Name.Substring(1, instrNode.Name.Length - 1));
                switch (instrNode.Name.Substring(0, 1).ToLower())
                {
                    case "d":
                        ident = AMAS_DBI.AMASCommand.add_dep_instruction(ident, textBoxInstruction.Text);
                        break;
                    case "e":
                        ident = AMAS_DBI.AMASCommand.add_rank_instruction(ident, textBoxInstruction.Text);
                        break;
                    default:
                        ident = 0;
                        break;
                }
                if (ident > 0) StructForInstructions.Instruction_append(textBoxInstruction.Text, ident, listInstructions, MetaInstruct);
            }
        }

        private void remove_instruction()
        {
            if (listInstructions.SelectedIndex >=0)
            {
                bool good = false;
                int ident = (int)Convert.ToInt32( listInstructions.SelectedValue);
                switch (instrNode.Name.Substring(0, 1).ToLower())
                {
                    case "d":
                        good = AMAS_DBI.AMASCommand.remove_dept_instruction(ident);
                        break;
                    case "e":
                        good = AMAS_DBI.AMASCommand.remove_rank_instruction(ident);
                        break;
                    default:
                        good = false;
                        break;
                }
                if (good)
                {
                    MetaInstruct.RemoveAt(listInstructions.SelectedIndex);
                    listInstructions.DataSource = null;
                    listInstructions.Items.Clear();
                    listInstructions.DataSource = MetaInstruct;
                    listInstructions.DisplayMember = "Instruction";
                    listInstructions.ValueMember = "Ident";
                }
            }
        }

        private void äîáàâèòüÄîëæíîñòüToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            addDegree();
        }

        private void óäàëèòüÄîëæíîñòüToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            Degree_Listing.delete_degree();
        }

        private void ğåäàêòèğîâàòüÄîëæíîñòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renameDegree();
        }

        private void íàçíà÷èòüÄîëæíîñòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loc_degree();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            addDegree();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Degree_Listing.delete_degree();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            loc_degree();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            renameDegree();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            StructMyOrg.Add_dept("Îòäåë " + treeViewDepts.Nodes.Count.ToString());
            StructMyOrg.enterprise_str();
            StructMyOrg.draw_shema_enterprice();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            try { treeViewDepts.SelectedNode.BeginEdit(); }
            catch { }
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            StructMyOrg.delete_organization();
        }

        private void toolStripButton8_Click(object sender, EventArgs e)
        {
            StructMyOrg = new Structure(AMASacc, treeViewDepts, false);
            StructMyOrg.Drag_drop = true;
        }

        private void toolStripButton9_Click(object sender, EventArgs e)
        {
            treeViewDepts.ExpandAll();
        }

        private void toolStripButton10_Click(object sender, EventArgs e)
        {
            Expandsubstr(treeViewDepts.SelectedNode);
        }

        private void äîáàâèòüÎòäåëToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            StructMyOrg.Add_dept("Îòäåë " + treeViewDepts.Nodes.Count.ToString());
            StructMyOrg.enterprise_str();
            StructMyOrg.draw_shema_enterprice();
        }

        private void ïåğåèìåíîâàòüÎòäåëToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try { treeViewDepts.SelectedNode.BeginEdit(); }
            catch { }
        }

        private void óäàëèòüÎòäåëToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StructMyOrg.delete_organization();
        }

        private void ïåğåğèñîâàòüÑòğóêòóğóToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            StructMyOrg = new Structure(AMASacc, treeViewDepts, false);
            StructMyOrg.Drag_drop = true;
        }

        private void ğàçâåğíóòüÑòğóêòóğóToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            treeViewDepts.ExpandAll();
        }

        private void ğàçâåğíóòüÂåòâüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Expandsubstr(treeViewDepts.SelectedNode);
        }

        private void toolStripButton11_Click(object sender, EventArgs e)
        {
            addDegree();
        }

        private void toolStripButton12_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem DD in listViewDegDep.CheckedItems)
                    if (AMASCommand.Delete_L_degree(DD.SubItems[1].Text))
                        degrees_in_dep(StructMyOrg.select_dep(thisNode));
            }
            catch { }
        }

        private void toolStripButton13_Click(object sender, EventArgs e)
        {
            try
            {
                string cod = "";
                foreach (ListViewItem DD in listViewDegDep.CheckedItems)
                {
                    cod = DD.SubItems[1].Text;
                    break;
                }
                if (cod.Length > 0)
                    if (AMASCommand.assing_leader(cod))
                        degrees_in_dep(StructMyOrg.select_dep(thisNode));
            }
            catch { }
        }

        private void íàçíà÷èòüÄîëæíîñòüToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            addDegree();
        }

        private void îòìåíèòüÍàçíà÷åíèåToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (ListViewItem DD in listViewDegDep.CheckedItems)
                    if (AMASCommand.Delete_L_degree(DD.SubItems[1].Text))
                        degrees_in_dep(StructMyOrg.select_dep(thisNode));
            }
            catch { }
        }

        private void íàçíà÷èòüĞóêîâîäèòåëÿToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                string cod = "";
                foreach (ListViewItem DD in listViewDegDep.CheckedItems)
                {
                    cod = DD.SubItems[1].Text;
                    break;
                }
                if (cod.Length > 0)
                    if (AMASCommand.assing_leader(cod))
                        degrees_in_dep(StructMyOrg.select_dep(thisNode));
            }
            catch { }
        }

        private void toolStripButton14_Click(object sender, EventArgs e)
        {
            groups_listing.add_group("Íîâàÿ ãğóïïà");
        }

        private void toolStripButton15_Click(object sender, EventArgs e)
        {
            groups_listing.delete_group();
        }

        private void toolStripButton16_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeViewGroups.SelectedNode != null) grpNode = treeViewGroups.SelectedNode;
                else grpNode = null;
                if (grpNode.Name.Substring(0, 1).CompareTo("G") == 0)
                    grpNode.BeginEdit();
            }
            catch { }
        }

        private void äîáàâèòüÃğóïïóToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            groups_listing.add_group("Íîâàÿ ãğóïïà");
        }

        private void óäàëèòüÃğóïïóToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            groups_listing.delete_group();
        }

        private void ïåğåèìåíîâàòüÃğóïïóToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            try
            {
                if (treeViewGroups.SelectedNode != null) grpNode = treeViewGroups.SelectedNode;
                else grpNode = null;
                if (grpNode.Name.Substring(0, 1).CompareTo("G") == 0)
                    grpNode.BeginEdit();
            }
            catch { }
        }

        private void toolStripButton17_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupVizing();
        }

        private void toolStripButton18_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupMoving();
        }

        private void toolStripButton19_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupFull();
        }

        private void ãğóïïàÂèçèğîâàíèÿToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupVizing();
        }

        private void íğóïïàÈñïîëíåíèÿToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupMoving();
        }

        private void ïîëíàÿÃğóïïàToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupFull();
        }

        private void toolStripButton22_Click(object sender, EventArgs e)
        {
            long[] gem = StructWithDegree.checked_Employee();
            if (gem != null)
                foreach (long emp in gem)
                    groups_listing.append_empl(emp);
        }

        private void toolStripButton20_Click(object sender, EventArgs e)
        {
            groups_listing.remove_empl();
        }

        private void óäàëèòüÑîòğóäíèêàÈçÃğóïïûToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groups_listing.remove_empl();
        }

        private void äîáàâèòüÑîòğóäíèêîâÂÃğóïïóToolStripMenuItem_Click(object sender, EventArgs e)
        {
            long[] gem = StructWithDegree.checked_Employee();
            if (gem != null)
                foreach (long emp in gem)
                    groups_listing.append_empl(emp);
        }

        private void äîáàâèòüÈíñòğóêöèşToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            append_instruction();
        }

        private void óäàëèòüÈíñòğóêöèşToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            remove_instruction();
        }

        private void tsAddInstruction_Click(object sender, EventArgs e)
        {
            append_instruction();
        }

        private void tsRemInstruction_Click(object sender, EventArgs e)
        {
            remove_instruction();
        }

        private void äîáàâèòüÎòäåëToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppendDept();
        }

        private void AppendDept()
        {
            StructMyOrg.Add_dept("Îòäåë " + (treeViewDepts.Nodes.Count +1).ToString());
            StructMyOrg.enterprise_str();
            StructMyOrg.draw_shema_enterprice();
        }

        private void óäàëèòüÎòäåëToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            StructMyOrg.delete_organization();
        }

        private void ïåğåèìåíîâàòüÎòäåëToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try { treeViewDepts.SelectedNode.BeginEdit(); }
            catch { }
        }

        private void toolStripButton23_Click(object sender, EventArgs e)
        {
            AppendDept();
        }

        private void toolStripButton24_Click(object sender, EventArgs e)
        {
            StructMyOrg.delete_organization();
        }

        private void toolStripButton25_Click(object sender, EventArgs e)
        {
            try { treeViewDepts.SelectedNode.BeginEdit(); }
            catch { }
        }

        private void ïåğåğèñîâàòüÑòğóêòóğóToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StructMyOrg = new Structure(AMASacc, treeViewDepts, false);
            StructMyOrg.Drag_drop = true;
        }

        private void ğàçâåğíóòüÑòğóêòóğóToolStripMenuItem_Click(object sender, EventArgs e)
        {
            treeViewDepts.ExpandAll();
        }

        private void ğàçâåğíóòüÂåğâüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Expandsubstr(treeViewDepts.SelectedNode);
        }

        private void toolStripButton26_Click(object sender, EventArgs e)
        {
            StructMyOrg = new Structure(AMASacc, treeViewDepts, false);
            StructMyOrg.Drag_drop = true;
        }

        private void toolStripButton27_Click(object sender, EventArgs e)
        {
            treeViewDepts.ExpandAll();
        }

        private void toolStripButton28_Click(object sender, EventArgs e)
        {
            Expandsubstr(treeViewDepts.SelectedNode);
        }

        private void íàçíà÷èòüÄîëæíîñòüToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            loc_degree();
        }

        private void îòìåíèòüÄîëæíîñòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dealloc_degree();
        }

        private void íàçíà÷èòüĞóêîâîäèòåëÿToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AssignLeader();
        }

        private void dealloc_degree()
        {
            try
            {
                foreach (ListViewItem DD in listViewDegDep.CheckedItems)
                    if (AMASCommand.Delete_L_degree(DD.SubItems[1].Text))
                        degrees_in_dep(StructMyOrg.select_dep(thisNode));
            }
            catch { }
        }

        private void AssignLeader()
        {
            try
            {
                string cod = "";
                foreach (ListViewItem DD in listViewDegDep.CheckedItems)
                {
                    cod = DD.SubItems[1].Text;
                    break;
                }
                if (cod.Length > 0)
                    if (AMASCommand.assing_leader(cod))
                        degrees_in_dep(StructMyOrg.select_dep(thisNode));
            }
            catch { }

        }

        private void toolStripButton29_Click(object sender, EventArgs e)
        {
            loc_degree();
        }

        private void toolStripButton30_Click(object sender, EventArgs e)
        {
            dealloc_degree();
        }

        private void toolStripButton31_Click(object sender, EventArgs e)
        {
            AssignLeader();
        }

        private void äîáàâèòüÄîëæíîñòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            addDegree();
        }

        private void óäàëèòüÄîëæíîñòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Degree_Listing.delete_degree();
        }

        private void ïåğåèìåíîâàòüÄîëæíîñòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            renameDegree();
        }

        private void íàçíà÷èòüÄîëæíîñòüToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            loc_degree();
        }

        private void toolStripButton32_Click(object sender, EventArgs e)
        {
            addDegree();
        }

        private void toolStripButton33_Click(object sender, EventArgs e)
        {
            Degree_Listing.delete_degree();
        }

        private void toolStripButton34_Click(object sender, EventArgs e)
        {
            renameDegree();
        }

        private void toolStripButton35_Click(object sender, EventArgs e)
        {
            loc_degree();
        }

        private void äîáàâèòüÃğóïïóToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            groups_listing.add_group("Íîâàÿ ãğóïïà");
        }

        private void óäàëèòüÃğóïïóToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            groups_listing.delete_group();
        }

        private void ïåğåèìåíîâàòüÃğóïïóToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            RenameGroup();
        }

        private void RenameGroup()
        {
            try
            {
                if (treeViewGroups.SelectedNode != null) grpNode = treeViewGroups.SelectedNode;
                else grpNode = null;
                if (grpNode.Name.Substring(0, 1).CompareTo("G") == 0)
                    grpNode.BeginEdit();
            }
            catch { }
        }

        private void ãğóïïàÂèçèğîâàíèÿToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupVizing();
        }

        private void ãğóïïàÈñïîëíåíèÿToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupMoving();
        }

        private void ïîëíàÿÃğóïïàToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupFull();
        }

        private void toolStripButton36_Click(object sender, EventArgs e)
        {
            groups_listing.add_group("Íîâàÿ ãğóïïà");
        }

        private void toolStripButton37_Click(object sender, EventArgs e)
        {
            groups_listing.delete_group();
        }

        private void toolStripButton38_Click(object sender, EventArgs e)
        {
            RenameGroup();
        }

        private void toolStripButton39_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupVizing();
        }

        private void toolStripButton40_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupMoving();
        }

        private void toolStripButton41_Click(object sender, EventArgs e)
        {
            groups_listing.Set_groupFull();
        }

        private void íàçíà÷èòüÂÃğóïïóToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmplInGroup();
        }

        private void èñêëş÷èòüÑîòğToolStripMenuItem_Click(object sender, EventArgs e)
        {
            groups_listing.remove_empl();
        }
        private void AddEmplInGroup()
        {
            long[] gem = StructWithDegree.checked_Employee();
            if (gem != null)
                foreach (long emp in gem)
                    groups_listing.append_empl(emp);
        }

        private void toolStripButton21_Click(object sender, EventArgs e)
        {
            AddEmplInGroup();
        }

        private void toolStripButton42_Click(object sender, EventArgs e)
        {
            groups_listing.remove_empl();
        }

        private void äîáàâèòüÈíñòğóêöèşToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void äîáàâèòüÈíñòğóêöèşToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            append_instruction();
        }

        private void óäàëèòüÈíñòğóêöèşToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            remove_instruction();
        }

        private void toolStripButton43_Click(object sender, EventArgs e)
        {
            remove_instruction();
        }

        private void toolStripButton44_Click(object sender, EventArgs e)
        {
            append_instruction();
        }

        private void Degreelist_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}