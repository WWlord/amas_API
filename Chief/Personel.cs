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
using Chief.baseLayer;
using ClassErrorProvider;
using ClassInterfases;

namespace Chief
{
    public partial class Personel : Form
    {
        private AMAS_DBI.Class_syb_acc AMASacc;
        private ClassStructure.Structure StructWithDegree;
        private TreeNode thisNode = null;

        public int ModuleId;

        private ChefSettings frmSettings1 = new ChefSettings();

        public Personel(AMAS_DBI.Class_syb_acc ACC, ImageList ImageStd)
        {
            InitializeComponent();
            AMASacc = ACC;
            treeStructure.ImageList = ImageStd;
            StructWithDegree = new Structure(AMASacc, treeStructure, true,true);
            treeStructure.ExpandAll();
            tabPersonnel.SelectedIndexChanged += new EventHandler(tabPersonnel_SelectedIndexChanged);
            this.peopleReg.connect(AMASacc);
            treeStructure.NodeMouseClick += new TreeNodeMouseClickEventHandler(treeStructure_AfterSelect);
            listBoxReserv.SelectedIndexChanged+=new EventHandler(listBoxReserv_SelectedIndexChanged);
            Reserve_Listing(AMAS_Query.Class_AMAS_Query.Get_Personel_reserve(), listBoxReserv);
            tabControlStuff.SelectedIndexChanged+=new EventHandler(tabControlStuff_SelectedIndexChanged);
            this.Load+=new EventHandler(Personel_Load);
            this.FormClosed+=new FormClosedEventHandler(Personel_FormClosed);
            ModuleId = (int)ClassErrorProvider.ErrorBBLProvider.Modules.Personel;
            tabPersonnel.Selected+=new TabControlEventHandler(tabPersonnel_Selected);
        }

        private void Personel_Load(object sender, EventArgs e)
        {
            frmSettings1.SettingsKey = "Personel";
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

        private void Personel_FormClosed(Object sender, FormClosedEventArgs e)
        {
            frmSettings1.Save();
        }

        private void tabControlStuff_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl TAS = (TabControl)sender;
            SelectTabloid( TAS);
        }

        private int current_employee()
        {
            int employee = -1;
            switch (tabPersonnel.SelectedTab.Name)
            {
                case "tabPageReserv":
                    employee =(int) listBoxReserv.SelectedValue;
                    break;
                case "tabPageAll":
                    Employeesheet ESH = (Employeesheet)Employees[listBoxALL.SelectedIndex];
                    employee = ESH.Id;
                    //employee =(int) listBoxALL.SelectedValue;
                    break;
            }
            return employee;
        }

        private void SelectTabloid(TabControl TAS)
        {
            try
            {
                switch (TAS.SelectedTab.Name)
                {
                    case "tabStructure":
                        break;
                    case "tabAnketa":
                        webAnketa.Stop();
                        webAnketa.Navigate(getAnketa(), false);
                        break;
                    case "tabDelo":
                        Photo.Image = StructWithDegree.Get_Photo(current_employee());
                        break;
                }
            }
            catch (Exception ex) 
            {
                AMASacc.EBBLP.AddError(ex.Message, "Personel - 1", ex.StackTrace);
            }
        }


        private string getAnketa()
        {
            string filename="";
            AMASacc.Set_table("TPersonel1", AMAS_Query.Class_AMAS_Query.Get_Anketa(current_employee()),null);
            AMASacc.Get_row(0);
            AMASacc.Find_Stream("anketa");
            filename = AMASacc.get_current_File();
            AMASacc.ReturnTable();
            return filename;
        }

        private void  tabPersonnel_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl TAS = (TabControl)sender;
            switch (TAS.SelectedTab.Name)
            {
                case "tabPageReserv":
                    //NewEmployeeMenuItem.Visible = true;
                    //DeassignFromDegreeMenuItem.Visible = false;
                    for (int i = 0; i < toolStripPersonal.Items.Count;i++ )
                        if (toolStripPersonal.Items[i].Name.Contains("Ps") )
                            toolStripPersonal.Items[i].Visible = true;
                        else toolStripPersonal.Items[i].Visible = false;
                    Reserve_Listing(AMAS_Query.Class_AMAS_Query.Get_Personel_reserve(), listBoxReserv);
                    break;
                case "tabPageAll":
                    //NewEmployeeMenuItem.Visible = false;
                    //DeassignFromDegreeMenuItem.Visible = true;
                    //DeassignFromDegreeMenuItem.Visible = false;
                    for (int i = 0; i < toolStripPersonal.Items.Count; i++)
                        if (toolStripPersonal.Items[i].Name.Contains("Em"))
                            toolStripPersonal.Items[i].Visible = true  ;
                        else toolStripPersonal.Items[i].Visible = false;
                    Reserve_Listing(AMAS_Query.Class_AMAS_Query.Get_Personel_ALL(), listBoxALL);
                    break;
            }
        }

        private void treeStructure_AfterSelect(object sender, TreeNodeMouseClickEventArgs e)
        {
            thisNode = e.Node;
        }

        private void listBoxReserv_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBoxReserv.SelectedIndex>= 0)
            {
                toolStripStatusLabel1.Text = listBoxReserv.SelectedValue.ToString();
                SelectTabloid(tabControlStuff);
            }
        }

        ArrayList Employees = null;

        private void RemoveEmployeeMenuItem_Click(object sender, EventArgs e)
        {
            int index = 0; 
                {
                    switch (this.tabPersonnel.SelectedTab.Name)
                    {
                        case "tabPageReserv":
                            index = listBoxReserv.SelectedIndex;
                            if (index >= 0)
                                if (AMASCommand.remove_Employee((int)listBoxReserv.SelectedValue))
                                {
                                    Employees.RemoveAt(index);
                                    refresh_employee_List(listBoxReserv);
                                }
                            break;
                        case "tabPageAll":
                            index = listBoxALL.SelectedIndex;
                            if (index >= 0)
                            {
                                Employeesheet ESH = (Employeesheet)Employees[listBoxALL.SelectedIndex];
                                int cod = ESH.Id;
                                //if (AMASCommand.remove_Employee((int)listBoxALL.SelectedValue))
                                if (AMASCommand.remove_Employee(cod))
                                    {
                                    Employees.RemoveAt(index);
                                    refresh_employee_List(listBoxALL);
                                }
                            }
                            break;
                    }
                }
        }

        private void AddToReservMenuItem_Click(object sender, EventArgs e)
        {
        }

        private void Reserve_Listing(string sql, ListBox list)
        {
            try
            {
                AMASacc.Set_table("TPersonel2", sql, null);
                Employees = new ArrayList();
                for (int l = 0; l < AMASacc.Rows_count; l++)
                {
                    AMASacc.Get_row(l);
                    Employees.Add(new Employeesheet((string)AMASacc.Find_Field("fio"), (int)AMASacc.Find_Field("employee")));
                }
                refresh_employee_List(list);
                if (list.Items.Count > 0) list.SelectedIndex = 0;
                AMASacc.ReturnTable();
            }
            catch (Exception e)
            {
                AMASacc.EBBLP.AddError(e.Message, "Personel - 2", e.StackTrace);
            }
        }

        private int refresh_employee_List(ListBox list)
        {
            try
            {
                list.DataSource = null;
                list.Items.Clear();
                list.DataSource = Employees;
                if (Employees!=null)
                    if (Employees.Count > 0)
                    {
                        list.DisplayMember = "Name";
                        list.ValueMember = "Id";
                    }
            }
            catch (Exception e) 
            { 
                AMASacc.EBBLP.AddError(e.Message, "Personel - 3", e.StackTrace);
                //Console.WriteLine(e.Message); 
            }
            return Employees.Count;
        }

        public class Employeesheet
        {
            private int Employee_Id;
            private string Employee_Name;

            public Employeesheet(string Name, int Id)
            {

                Employee_Name = Name;
                Employee_Id = Id;
            }

            public string Name
            {
                get
                {
                    return Employee_Name;
                }
            }

            public int Id
            {
                get
                {
                    return Employee_Id;
                }
            }

            public override string ToString()
            {
                return Employee_Name;
            }

        }

        private void AssignToDegreeMenuItem_Click(object sender, EventArgs e)
        {
            AssingDegree();
        }

        private void AssingDegree()
        {
            ListBox listBox = selectListBox();
            int index = listBox.SelectedIndex;
            long degree = -1;
            if (thisNode != null) degree = StructWithDegree.select_Employee(thisNode);
            try
            {
                if (assgn_empl(degree))
                    if (index >= 0)
                    {
                        Employeesheet Es = (Employeesheet)Employees[index];
                        StructWithDegree.Assign_Employee(degree, Es.Name);
                        switch (this.tabPersonnel.SelectedTab.Name)
                        {
                            case "tabPageReserv":
                                Employees.RemoveAt(index);
                                break;
                            case "tabPageAll":

                                break;
                        }
                        refresh_employee_List(listBox);
                    }
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "Personel - 4", ex.StackTrace);
            }

        }

        private ListBox selectListBox()
        {
            ListBox lb;
            try
            {
                switch (this.tabPersonnel.SelectedTab.Name)
                {
                    case "tabPageReserv":
                        lb= listBoxReserv;
                        break;
                    case "tabPageAll":
                        lb= listBoxALL;
                        break;
                    default:
                        lb= null;
                        break;
                }
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "Personel - 5", ex.StackTrace);
                lb = null;
            }
            return lb;
        }

        private bool assgn_empl(long degree)
        {
            int id = -1;
            try
            {
                switch (this.tabPersonnel.SelectedTab.Name)
                {
                    case "tabPageReserv":
                        id = (int)listBoxReserv.SelectedValue;
                        break;
                    case "tabPageAll":
                            Employeesheet ESH = (Employeesheet)Employees[listBoxALL.SelectedIndex];
                            id = ESH.Id;
                        //id = (int)listBoxALL.SelectedValue;
                        break;
                }
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "Personel - 6", ex.StackTrace);
            }
            if (id > 0 && degree > 0)
                return AMASCommand.Assign_Employee(id, degree);
            else return false;
        }

        private void listBoxALL_SelectedIndexChanged(object sender, EventArgs e)
        {
            Ranks_Listing();
            SelectTabloid(tabControlStuff);
        }

        ArrayList Ranks = null;

        private void Ranks_Listing()
        {
            try
            {
                int cod = -1;
                if (listBoxALL.SelectedIndex >= 0)
                {
                    Employeesheet ESH = (Employeesheet)Employees[ listBoxALL.SelectedIndex];
                    cod=ESH.Id;
                    //cod = (int)listBoxALL.SelectedValue;
                }
                string OName = "";
                AMASacc.Set_table("TPersonel3", AMAS_Query.Class_AMAS_Query.Get_Ranks_of_empl((long)cod), null);
                try
                {
                    Ranks = new ArrayList();
                    for (int l = 0; l < AMASacc.Rows_count; l++)
                    {
                        AMASacc.Get_row(l);
                        AMASacc.Find_Field("name");
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                        else OName = "";
                        Ranks.Add(new Employeesheet(OName, (int)AMASacc.Find_Field("cod")));
                    }
                    refresh_rank_List(listBoxdegree);
                    if (listBoxdegree.Items.Count > 0) listBoxdegree.SelectedIndex = 0;
                }
                catch (Exception e) 
                { 
                    AMASacc.EBBLP.AddError(e.Message, "Personel - 7", e.StackTrace);
                }
                AMASacc.ReturnTable();
            }
            catch (Exception e) 
            { 
                AMASacc.EBBLP.AddError(e.Message, "Personel - 8", e.StackTrace);
            }
        }

        private void refresh_rank_List(ListBox list)
        {
            try
            {
                list.DataSource = null;
                list.Items.Clear();
                if (Ranks != null)
                    if (Ranks.Count>0)
                    {
                        list.DataSource = Ranks;
                        list.DisplayMember = "Name";
                        list.ValueMember = "Id";
                    }
            }
            catch (Exception e) 
            { 
                AMASacc.EBBLP.AddError(e.Message, "Personel - 9", e.StackTrace);
            }
        }

        private void DeassignFromDegreeMenuItem_Click(object sender, EventArgs e)
        {
            deassignDegree();
        }

        private void deassignDegree()
        {
            long degree = -1;
            try
            {
                switch (this.tabPersonnel.SelectedTab.Name)
                {
                    case "tabPageReserv":
                        if (thisNode != null) degree = StructWithDegree.select_Employee(thisNode);
                        break;
                    case "tabPageAll":
                        if (listBoxdegree.SelectedIndex >= 0)
                        {
                            Employeesheet rank = (Employeesheet)Ranks[listBoxdegree.SelectedIndex];
                            degree = rank.Id;
                        }
                        break;
                }
                if (AMASCommand.Deassign_Employee(degree))
                    deassingDegreeFromScreen(degree);
            }
            catch (Exception e)
            {
                AMASacc.EBBLP.AddError(e.Message, "Personel - 10", e.StackTrace);
            }

        }

        private void deassingDegreeFromScreen(long degree)
        {
            try
            {
                StructWithDegree.Dessign_Employee(degree);
                switch (this.tabPersonnel.SelectedTab.Name)
                {
                    case "tabPageReserv":
                        break;
                    case "tabPageAll":
                        Employeesheet rank = null;
                        for (int i = 0; i < Ranks.Count; i++)
                        {
                            rank = (Employeesheet)Ranks[i];
                            if (rank.Id == degree)
                            {
                                Ranks.RemoveAt(i);
                                break;
                            }
                        }
                        if (Ranks.Count > 0) refresh_rank_List(listBoxdegree);
                        break;
                }
            }
            catch (Exception e)
            {
                AMASacc.EBBLP.AddError(e.Message, "Personel - 11", e.StackTrace);
            }

        }

        private void AssignNewEmployeeMenuItem_Click(object sender, EventArgs e)
        {
            int id=peopleReg.Current_Man;
            long degree = -1;
            if (thisNode != null) degree = StructWithDegree.select_Employee(thisNode);
            if (id > 0 && degree>0)
                if(AMASCommand.Reserve_Employee_with_degree(id, degree))
                    StructWithDegree.Assign_Employee(degree, peopleReg.FIO_of_Current_Man);
        }

        private void AddAnketa()
        {
            int employee = -1;
            try
            {
                switch (this.tabPersonnel.SelectedTab.Name)
                {
                    case "tabPageReserv":
                        employee = (int)listBoxReserv.SelectedValue;
                        break;
                    case "tabPageAll":
                        Employeesheet ESH = (Employeesheet)Employees[listBoxALL.SelectedIndex];
                        employee = ESH.Id;
                        //employee = (int)listBoxALL.SelectedValue;
                        break;
                }
                if (employee > 0) append_anketa(employee);
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "Personel - 12", ex.StackTrace);
            }

        }

        private void append_anketa(int employee)
        {
            openFileDialogAnketa.Filter = "òåêñòîâûå ôàéëû (*.txt;*.doc;*.rtf)|*.txt;*.doc;*.rtf";
            if (openFileDialogAnketa.ShowDialog() == DialogResult.OK)
            {
                string anketaFilePath = openFileDialogAnketa.FileName;
                AMASCommand.Anketa_Employee_write(employee, CommonValues.CommonClass.SetImageHead(anketaFilePath), "");
            }
        }

        private void AddEmpPhoto()
        {
            int employee = -1;
            try
            {
                switch (this.tabPersonnel.SelectedTab.Name)
                {
                    case "tabPageReserv":
                        employee = (int)listBoxReserv.SelectedValue;
                        break;
                    case "tabPageAll":
                        Employeesheet ESH = (Employeesheet)Employees[listBoxALL.SelectedIndex];
                        employee = ESH.Id;
                        //employee = (int)listBoxALL.SelectedValue;
                        break;
                }
                if (employee > 0) append_Photo(employee);
            }
            catch (Exception ex)
            {
                AMASacc.EBBLP.AddError(ex.Message, "Personel - 13", ex.StackTrace);
            }

        }

        private void append_Photo(int employee)
        {
            openFileDialogAnketa.Filter = "Bitmap ôàéëû (*.bmp)|*.bmp";
            if (openFileDialogAnketa.ShowDialog() == DialogResult.OK)
            {
                string photoFilePath = openFileDialogAnketa.FileName;
                byte[] image = CommonClass.GetImage(photoFilePath);
                AMASCommand.Photo_Employee_write(employee, image);
            }
        }

        private void óäàëèòüÑîòðóäíèêàToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemovEmployee();
        }

        private void AppendEmplToReserv()
        {
            int id = peopleReg.Current_Man;
            if (id > 0)
                if (AMASCommand.Reserve_Employee(id))
                {
                    if (Employees == null) Employees = new ArrayList();
                    Employees.Add(new Employeesheet(peopleReg.FIO_of_Current_Man, id));
                    refresh_employee_List(listBoxReserv);
                }
        }

        private void AppendEmployee()
        {
            int id = peopleReg.Current_Man;
            long degree = -1;
            if (thisNode != null) degree = StructWithDegree.select_Employee(thisNode);
            if (id > 0 && degree > 0)
                if (AMASCommand.Reserve_Employee_with_degree(id, degree))
                    StructWithDegree.Assign_Employee(degree, peopleReg.FIO_of_Current_Man);
        }

        private void RemovEmployee()
        {
            int index = -1;
            int employee = 0;
            {
                switch (this.tabPersonnel.SelectedTab.Name)
                {
                    case "tabPageReserv":
                        index = listBoxReserv.SelectedIndex;
                        if (index >= 0)
                            try
                            {
                                Employeesheet ESH = (Employeesheet)Employees[listBoxALL.SelectedIndex];
                                employee = ESH.Id;
                                //employee=(int)listBoxReserv.SelectedValue;
                                if (AMASCommand.remove_Employee(employee))
                                {
                                    Employees.RemoveAt(index);
                                    refresh_employee_List(listBoxReserv);
                                }
                            }
                            catch { }
                        break;
                    case "tabPageAll":
                        index = listBoxALL.SelectedIndex;
                        if (index >= 0)
                        {
                            Employeesheet ESH = (Employeesheet)Employees[listBoxALL.SelectedIndex];
                            employee = ESH.Id;
                            //employee=(int)listBoxALL.SelectedValue;
                            if (AMASCommand.remove_Employee(employee))
                            {
                                Employees.RemoveAt(index);
                                refresh_employee_List(listBoxALL);
                            }
                        }
                        break;
                }
            }
            StructWithDegree.Dessign_Employee(employee);
        }

        private void âÐåçåðâToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppendEmplToReserv();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            AppendEmplToReserv();
        }

        private void íàÄîëæíîñòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AppendEmployee();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            AppendEmployee();
        }

        private void íàçíà÷èòüÍàÄîëæíîñòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AssingDegree();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            AssingDegree();
        }

        private void ñíÿòüÑÄîëæíîñòèToolStripMenuItem_Click(object sender, EventArgs e)
        {
            deassignDegree();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            deassignDegree();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            RemovEmployee();
        }

        private void äîáàâèòüÀíêåòóToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddAnketa();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            AddAnketa();
        }

        private void äîáàâèòüÔîòîToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddEmpPhoto();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            AddEmpPhoto();
        }

        private void listBoxALL_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBoxALL.SelectedIndex >= 0)
                toolStripStatusLabel1.Text =(string) listBoxALL.SelectedItem.ToString(); //  .SelectedValue.ToString();
            Ranks_Listing();
        }

        private void listBoxReserv_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBoxReserv.SelectedIndex >= 0)
                toolStripStatusLabel1.Text = (string)listBoxReserv.SelectedItem.ToString(); //  .SelectedValue.ToString();
        }

        private void tabPersonnel_Selected(Object sender, TabControlEventArgs e)
        {
            toolStripStatusLabel1.Text = "";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {

        }

        private void label12_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void label16_Click(object sender, EventArgs e)
        {

        }

        private void tbEduOKIN_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonBirthPlace_Click(object sender, EventArgs e)
        {
            Form BhPlace;
            ClassInterfases.GSADR iNF;
            BhPlace = new GSAddress(AMASacc);
            iNF = BhPlace as ClassInterfases.GSADR;
            if (BhPlace.ShowDialog() == DialogResult.Yes)
            {
                iNF.GSAddressID();
                tbbirthPlace.Text= iNF.GSAddressString();
            }
            BhPlace.Close();
            BhPlace.Dispose();
        }

        private void buttonSchool_Click(object sender, EventArgs e)
        {
            Form HiSchool;
            ClassInterfases.GSORG iNF;
            HiSchool = new GSOrganisation(AMASacc);
            iNF = HiSchool as ClassInterfases.GSORG;
            if (HiSchool.ShowDialog() == DialogResult.Yes)
            {
                iNF.GSOrgID();
                tbSchool.Text = iNF.GSOrgName();
            }
            HiSchool.Close();
            HiSchool.Dispose();
        }
    }
}