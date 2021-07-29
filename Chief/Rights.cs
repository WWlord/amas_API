using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClassStructure;
using System.Collections;
using ClassErrorProvider;

namespace Chief
{
    public partial class Rights : Form
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private ClassStructure.Structure StructWithDegree;
        public int ModuleId ;

        private TreeNode thisNode = null;

        private int Emplyee_ID = 0;
        private int Degree_ID = 0;
        private string Employee_FIO = "";
        private string Employee_Login = "";
        private int Employee_index = 0;
        private int Instriction_ID = 0;

        public Rights(AMAS_DBI.Class_syb_acc ACC, ImageList ImageStd)
        {
            InitializeComponent();
            AMASacc = ACC;
            treeStructure.ImageList = ImageStd;
            StructWithDegree = new Structure(AMASacc, treeStructure, true, true);
            treeStructure.ExpandAll();
            Reserve_Listing(AMAS_Query.Class_AMAS_Query.Get_Personel_Rights(), listViewAll);
            panelMeta.Resize+=new EventHandler(panelMeta_Resize);
            treeStructure.AfterSelect+=new TreeViewEventHandler(treeStructure_AfterSelect);
            listViewAll.SelectedIndexChanged+=new EventHandler(listViewAll_SelectedIndexChanged);
            tabControlSet.SelectedIndexChanged+=new EventHandler(tabControlSet_SelectedIndexChanged);
            tabControlEmp.SelectedIndexChanged+=new EventHandler(tabControlEmp_SelectedIndexChanged);
            listViewRole.ItemCheck+=new ItemCheckEventHandler(listViewRole_ItemCheck);
            listViewinstrRights.ItemCheck+=new ItemCheckEventHandler(listViewinstrRights_ItemCheck);
            ModuleId = (int)ClassErrorProvider.ErrorBBLProvider.Modules.Rights;
        }

        private bool check_role_enable = false;
        private void listViewRole_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (check_role_enable)
            {
                string role = listViewRole.Items[e.Index].SubItems[1].Text;
                bool check = false;
                if (e.CurrentValue == CheckState.Unchecked)
                    check = true;
                else
                    check = false;
                AMAS_DBI.AMASCommand.roles_EmployeeAddRemove(Employee_Login, role, check);
            }
        }

        private void listViewinstrRights_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (check_role_enable)
            {
                string role = listViewinstrRights.Items[e.Index].SubItems[1].Text;
                bool check = false;
                if (e.CurrentValue == CheckState.Unchecked)
                    check = true;
                else
                    check = false;
                AMAS_DBI.AMASCommand.roles_InstructionAddRemove(Instriction_ID, (int) Convert.ToInt32( role), check);
            }
        }

        private void tabControlEmp_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            switch (tabControlEmp.TabPages[tabControlEmp.SelectedIndex].Name)
            {
                case "Structure":
                    tabControlSet.Visible = false;
                    tabControlDoc.Visible = true;
                    break;
                case "Personel":
                    tabControlSet.Visible = true;
                    tabControlDoc.Visible = false;
                    if(listViewAll.Items.Count>0)
                        listViewAll.Items[0].Selected=true;
                    selectedEMplAll(listViewAll.SelectedItems);
                    break;
            }
            select_tab();
        }

        private void tabControlSet_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            select_tab();
        }

        private void listViewAll_SelectedIndexChanged(object sender, System.EventArgs e)
        {
            selectedEMplAll(listViewAll.SelectedItems);
        }

        private void selectedEMplAll(ListView.SelectedListViewItemCollection Emplfast)
        {
            foreach (ListViewItem item in Emplfast)
            {
                Emplyee_ID = (int)Convert.ToInt32(item.SubItems[1].Text);
                Employee_FIO = item.Text;
                Employee_Login = item.SubItems[2].Text;
                Employee_index = item.Index;
            }
            select_tab();
        }

        private void treeStructure_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            if (thisNode != e.Node)
            {
                thisNode = e.Node;
                if (thisNode.Name.Substring(1, 1).ToLower().CompareTo("e") == 0)
                {
                    Degree_ID = (int)Convert.ToInt32(thisNode.Name.Substring(2));
                    Employee_FIO = thisNode.Text;
                }
                else
                {
                    Degree_ID = 0;
                    Employee_FIO = "";
                }
                select_tab();
            }
        }

        private void Reserve_Listing(string sql, ListView Employees)
        {
            Employees.Clear();
            Employees.LargeImageList.Images.Clear();
            AMASacc.Set_table("TRights1", sql,null);
            try
            {
                ListViewItem itemPerson;
                int employee;
                string KeyImage;
                string login = "";
                for (int l = 0; l < AMASacc.Rows_count; l++)
                {
                    AMASacc.Get_row(l);
                    itemPerson=Employees.Items.Add( (string)AMASacc.Find_Field("fio"));
                    employee= (int)AMASacc.Find_Field("employee");
                    KeyImage="e"+employee.ToString();
                    itemPerson.SubItems.Add(employee.ToString());
                    login = (string)AMASacc.Find_Field("indoor_name");
                    itemPerson.SubItems.Add(login.Trim());
                    try
                    {
                        AMASacc.Set_table("TRights2", AMAS_Query.Class_AMAS_Query.Get_photo(employee), null);
                        
                        try
                        {
                            AMASacc.Get_row(0);
                            if (AMASacc.Find_Stream("photo") != null)
                            {
                                Employees.LargeImageList.Images.Add(KeyImage, Image.FromStream(AMASacc.get_current_Stream()));
                                itemPerson.StateImageIndex = Employees.LargeImageList.Images.Keys.IndexOf(KeyImage);
                            }
                        }
                        catch (Exception e) 
                        {
                            AMASacc.EBBLP.AddError(e.Message, "Rights - 1", e.StackTrace);
                            //Console.WriteLine(e.Message); 
                        }
                        AMASacc.ReturnTable();
                    }
                    catch (Exception e) 
                    {
                        AMASacc.EBBLP.AddError(e.Message, "Rights - 2", e.StackTrace);
                        //Console.WriteLine(e.Message);
                    }
                }
            }
            catch (Exception e) 
            {
                AMASacc.EBBLP.AddError(e.Message, "Rights - 3", e.StackTrace);
                //Console.WriteLine(e.Message);
            }
            AMASacc.ReturnTable();
        }

        private void select_tab()
        {
            switch (tabControlEmp.TabPages[tabControlEmp.SelectedIndex].Name)
            {
                case "Personel":
                    switch (tabControlSet.TabPages[tabControlSet.SelectedIndex].Name)
                    {
                        case "tabPagerights":
                            Photo.Image = StructWithDegree.Get_Photo(Emplyee_ID);
                            labelFio.Text = Employee_FIO;
                            if (Employee_Login.Length < 1)
                            {
                                panellogin.Enabled = true;
                                password.Text = "";
                                passwordconfirm.Text = "";
                                textBoxlogname.Text = "";
                            }
                            else
                            {
                                panellogin.Enabled = false;
                                password.Text = "";
                                passwordconfirm.Text = "";
                                textBoxlogname.Text = Employee_Login;
                            }
                            break;
                        case "tabPageRole":
                            List_of_roles();
                            break;
                    }
                    break;
                case "Structure":
                    try
                    {
                        switch (tabControlDoc.TabPages[tabControlSet.SelectedIndex].Name)
                        {
                            case "InstructionsList":
                                StructWithDegree.Instruction_list(thisNode, listViewInstr, MetaInstruct);
                                break;
                        }
                    }
                    catch { tabControlSet.SelectedIndex = 0; }
                    break;
            }
        }

        ArrayList MetaInstruct = new ArrayList();

        private void panelMeta_Resize(object sender, System.EventArgs e)
        {
            Control control = (Control)sender;
            labelFio.Width = control.Left + control.Width - labelFio.Left - labelFio.Width - 3;
        }

        private void List_of_roles()
        {
            check_role_enable = false; 
            listViewRole.Clear();
            Array roles = AMAS_DBI.AMASCommand.Roles_Employee_list (Employee_Login);
            if (roles != null)
            {
                ListViewItem itemPerson;
                int rows = roles.Length/4;
                string b ="0";
                string color = "0";
                for (int i = 0; i < rows; i++)
                {
                    itemPerson = listViewRole.Items.Add((string)roles.GetValue(i, 1));
                    itemPerson.SubItems.Add((string)roles.GetValue(i, 0));
                    b = (string)roles.GetValue(i, 2);
                    color = (string)roles.GetValue(i, 3);
                    if (b.Trim().CompareTo("1") == 0)
                    {
                        itemPerson.Checked = true;
                        if (b.Trim().CompareTo(color.Trim()) == 0) itemPerson.ForeColor = Color.DarkBlue;
                        else itemPerson.ForeColor = Color.DarkRed;
                    }
                    else
                    {
                        itemPerson.Checked = false;
                        if (b.Trim().CompareTo(color.Trim()) == 0) itemPerson.ForeColor = Color.DarkSlateGray;
                        else itemPerson.ForeColor = Color.DarkViolet;

                    }
                }
            }
            check_role_enable = true;
        }

        private void List_of_instruction_roles()
        {
            check_role_enable = false;
            listViewinstrRights.Clear();
            Array roles = AMAS_DBI.AMASCommand.Roles_Instruction_list(Instriction_ID);
            if (roles != null)
            {
                ListViewItem itemPerson;
                int rows = roles.Length / 3;
                string b = "0";
                for (int i = 0; i < rows; i++)
                {
                    itemPerson = listViewinstrRights.Items.Add((string)roles.GetValue(i, 1));
                    itemPerson.SubItems.Add((string)roles.GetValue(i, 0));
                    b = (string)roles.GetValue(i, 2);
                    if (b.Trim().CompareTo("1") == 0)
                        itemPerson.Checked = true;
                    else
                        itemPerson.Checked = false;
                }
            }
            check_role_enable = true;
        }

        private void maskedTextBox2_MaskInputRejected(object sender, MaskInputRejectedEventArgs e)
        {

        }

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            if (password.Text.CompareTo(passwordconfirm.Text) != 0)
                MessageBox.Show("Пароль ведён неверно. Повторите ввод пароля");
            else
            {
                AMAS_DBI.AMASCommand.ADDEmployeeLogin(password.Text, textBoxlogname.Text, Emplyee_ID);
                Employee_Login = textBoxlogname.Text.Trim();
                listViewAll.Items[Employee_index].SubItems[2].Text = Employee_Login;
            }
        }

        private void listViewInstr_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                Instriction_ID = (int)Convert.ToInt32(listViewInstr.SelectedValue);
                List_of_instruction_roles();
            }
            catch { }
        }

        private void listViewAll_SelectedIndexChanged_1(object sender, EventArgs e)
        {

        }

    }
}