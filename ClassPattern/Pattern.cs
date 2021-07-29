using System;
using System.Resources;
using System.IO;
using System.Data;
using System.Xml;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using AMAS_DBI;
using CommonValues;

namespace ClassPattern
{
    public class Address_ids
    {
        private int[] Ident;
        private int[] Index;
        private string[] AddressName;

        private int current_number;
        private string textBuffer = "";
        private string backtextBuffer = "";
        private int counter = 0;
        private int array_dimention = 0;
        private bool new_Address = false;

        private AMAS_DBI.Class_syb_acc AMASacc;

        public string NewAddressName = "";
        public string ResultErr = "";
        public Address_ids Child = null;
        public System.Windows.Forms.ComboBox AddressBox;

        public Address_ids(System.Windows.Forms.ComboBox CBox)
        {
            AddressBox = CBox;
            this.AddressBox.Click += new EventHandler(this_AddressBox_Click);
            this.AddressBox.TextChanged += new EventHandler(this_AddressBox_TextChanged);
            this.AddressBox.LostFocus += new EventHandler(this_AddressBox_LostFocus);
            this.AddressBox.SelectedIndexChanged += new EventHandler(this_SelectedIndexChanged);
            this.AddressBox.KeyPress += new KeyPressEventHandler(this_KeyPress);
            this.AddressBox.KeyUp += new KeyEventHandler(this_AddressBox_KeyUp);
        }

        private void this_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Child != null) Child.clear();
        }

        private void this_AddressBox_Click(object sender, EventArgs e)
        {
            cleanNewAddress();
        }

        private void cleanNewAddress()
        {
            new_Address = false;
            textBuffer = "";
            backtextBuffer = "";
        }

        private void this_AddressBox_TextChanged(object sender, EventArgs e)
        {
            if (new_Address)
            {
                AddressBox.Text = textBuffer + backtextBuffer;
                AddressBox.SelectionStart = textBuffer.Length;
                AddressBox.SelectionLength = backtextBuffer.Length;
            }
        }

        private void this_AddressBox_LostFocus(object sender, EventArgs e)
        {
            if (new_Address)
            {
                NewAddressName = AddressBox.Text.Trim();
                cleanNewAddress();
                if (NewAddressName.Length == 0)
                {
                    //altAddressName = new string[AddressName.Length+1];
                    //altIdent = new int[Ident];
                    //get_number_by_text(NewAddressName);
                }
            }
            else NewAddressName = "";
        }
        private void this_AddressBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    if (new_Address)
                    {
                        if (textBuffer.Length > 0)
                            textBuffer = textBuffer.Substring(0, textBuffer.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = AddressName[number].Substring(textBuffer.Length);
                        AddressBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Address = true;
                        backtextBuffer = "";
                        textBuffer = AddressBox.Text.Substring(0, AddressBox.Text.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = AddressName[number].Substring(textBuffer.Length);
                        AddressBox.Text = textBuffer;
                    }
                    break;
                case Keys.Right:
                    if (new_Address)
                    {
                        if (backtextBuffer.Length > 0)
                            textBuffer += backtextBuffer.Substring(0, 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = AddressName[number].Substring(textBuffer.Length);
                        AddressBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Address = true;
                        backtextBuffer = "";
                        int number = get_number_by_text(textBuffer);
                        textBuffer = AddressBox.Text;
                        AddressBox.Text = textBuffer;
                    }
                    break;
                case Keys.Delete:
                    if (new_Address)
                    {
                        backtextBuffer = "";
                        AddressBox.Text = textBuffer;
                    }
                    else
                    {
                        new_Address = true;
                        backtextBuffer = "";
                        textBuffer = AddressBox.Text;
                        if (Child != null)
                            Child.clear();
                    }
                    break;
            }
        }

        private void this_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch ((char)e.KeyChar)
            {
                case (char)Keys.Back:
                    if (new_Address)
                    {
                        if (textBuffer.Length > 0)
                            textBuffer = textBuffer.Substring(0, textBuffer.Length - 1);
                        int number = get_number_by_text(textBuffer);
                        if (number >= 0) backtextBuffer = AddressName[number].Substring(textBuffer.Length);
                    }
                    else
                    {
                        new_Address = true;
                        backtextBuffer = "";
                        textBuffer = AddressBox.Text;
                    }
                    break;
                case (char)Keys.End:
                    if (new_Address)
                    {
                        textBuffer += backtextBuffer;
                        backtextBuffer = "";
                    }
                    break;
                case (char)Keys.Home:
                    if (new_Address)
                    {
                        backtextBuffer = textBuffer + backtextBuffer;
                        textBuffer = "";
                    }
                    break;
                case (char)Keys.Escape:
                    backtextBuffer = "";
                    textBuffer = "";
                    AddressBox.Text = "";
                    if (Child != null)
                        Child.clear();
                    break;
                default:
                    if ((e.KeyChar >= " ".ToCharArray()[0] && e.KeyChar <= "z".ToCharArray()[0]) || (e.KeyChar >= "А".ToCharArray()[0] && e.KeyChar <= "я".ToCharArray()[0]))
                    {
                        if (new_Address)
                        {
                            textBuffer += Convert.ToString(e.KeyChar);
                            int number = get_number_by_text(textBuffer);
                            if (number >= 0) backtextBuffer = AddressName[number].Substring(textBuffer.Length);
                            else backtextBuffer = "";
                        }
                        else
                        {
                            new_Address = true;
                            textBuffer = AddressBox.Text + Convert.ToString(e.KeyChar); ;
                            backtextBuffer = "";
                            if (Child != null)
                                Child.clear();
                        }
                    }
                    break;
            }
        }

        public void connect(AMAS_DBI.Class_syb_acc Acc)
        {
            AMASacc = Acc;
        }

        private void add_ident(int indx, int idnt, string text)
        {
            Ident[counter] = idnt;
            Index[counter] = indx;
            AddressName[counter] = text;
            counter++;
            current_number = counter;
        }

        public void clear()
        {
            counter = 0;
            cleanNewAddress();
            NewAddressName = "";
            new_Address = false;
            current_number = -1;
            array_dimention = 0;
            if (AddressBox != null)
            {
                AddressBox.Items.Clear();
                AddressBox.Refresh();
                AddressBox.Text = "";
            }
            if (Child != null) Child.clear();
        }

        public int get_ident()
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (Index[i] == AddressBox.SelectedIndex)
                    {
                        current_number = i;
                        return Ident[i];
                    }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 1", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        private int get_number_by_text(string text)
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (text.Length <= AddressName[i].Length)
                        if (text.ToLower().CompareTo(AddressName[i].Substring(0, text.Length).ToLower()) == 0)
                        {
                            current_number = i;
                            return i;
                        }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 2", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        public int get_index_by_text(string txt)
        {
            int num = get_number_by_text(txt);
            if (num >= 0)
            {
                AddressBox.SelectedIndex = Index[num];
                return Index[num];
            }
            else
            {
                return -1;
            }
        }

        public int get_index(int idnt)
        {
            int i;
            try
            {
                for (i = 0; i < array_dimention; i++)
                {
                    if (Ident[i] == idnt)
                    {
                        AddressBox.SelectedIndex = Index[i];
                        current_number = i;
                        return Index[i];
                    }
                }
            }
            catch (Exception err)
            {
                AMASacc.EBBLP.AddError(err.Message, "Pattern - 3", err.StackTrace);
                ResultErr = err.Message; return -2;
            }
            return -1;
        }

        public void Select_Subject(string sql, string fld, string ids)
        {
            if (AMASacc.Set_table("TPat1", sql, null))
            {
                try
                {
                    string OName = "";
                    clear();
                    int ind = 0;
                    int id = 0;
                    array_dimention = AMASacc.Rows_count;
                    Index = new int[array_dimention];
                    Ident = new int[array_dimention];
                    AddressName = new string[array_dimention];

                    for (int i = 0; i < array_dimention; i++)
                    {
                        AMASacc.Get_row(i);
                        AMASacc.Find_Field(fld);
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0) OName = (string)AMASacc.get_current_Field();
                        else OName = "";
                        ind = AddressBox.Items.Add(OName);
                        id = (int)AMASacc.Find_Field(ids);
                        add_ident(ind, id, OName);
                    }
                }
                catch (Exception err)
                {
                    AMASacc.EBBLP.AddError(err.Message, "Pattern - 4", err.StackTrace);
                    ResultErr = err.Message;
                }
                AMASacc.ReturnTable();
            }
            if (array_dimention > 0)
            {
                AddressBox.SelectedIndex = 0;
                AddressBox.Refresh();
            }
            else
            {
                current_number = -1;
                AddressBox.SelectedIndex = -1;
                AddressBox.Text = "";
                AddressBox.Refresh();
            }
        }

        public void ADDress(string name, int idx)
        {
            int DIM = -1;
            int[] Id = new int[array_dimention + 1];
            int[] Ind = new int[array_dimention + 1];
            string[] AddrN = new string[array_dimention + 1];
            for (int i = 0; i < array_dimention; i++)
            {
                Id[i] = Ident[i];
                Ind[i] = Index[i];
                AddrN[i] = AddressName[i];
            }
            Id[array_dimention] = idx;
            AddrN[array_dimention] = name;
            DIM = AddressBox.Items.Add(name);
            Ind[array_dimention] = DIM;
            Ident = Id;
            Index = Ind;
            AddressName = AddrN;
            array_dimention++;
            AddressBox.SelectedIndex = DIM;

        }
    }

    public class FileDirExplorer
    {
        private TreeView FileExplorer;
        private TreeNode Node = null;
        private string[] Drives;
        private string FilesSelector = "*.*";
        private bool aFile = false;
        private string HomeDirectory = "";
        // events

        public delegate void FilePathHandler(string FilePath);
        public delegate void PickedHandler();
        public event FilePathHandler FilePicked;

        public event PickedHandler Picked1;
        public event PickedHandler Picked2;
        public event PickedHandler Picked3;
        public event PickedHandler Picked4;

        public bool IS_File { get { return aFile; } }
        public byte[] FileImage
        {
            get { return CommonClass.GetImage(FilePath()); }
        }

        public string File_Path { get { return FilePath(); } }

        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        System.Windows.Forms.ToolStripMenuItem add;
        //ToolStripItem TSI;

        public FileDirExplorer(TreeView Explorer, string pattern)
        {
            FileExplorer = Explorer;
            FilesSelector = pattern;
            useWebbros = true;
            InitFDE();
        }

        public FileDirExplorer(TreeView Explorer, string pattern, string[] Context_menu)
        {
            FileExplorer = Explorer;
            FilesSelector = pattern;
            useWebbros = false;
            //
            // Menu
            //
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip();
            this.contextMenuStrip1.SuspendLayout();
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(224, 92);
            int semicolon = 0;
            foreach (string menu in Context_menu)
            {
                semicolon = menu.IndexOf(';');
                this.add = new System.Windows.Forms.ToolStripMenuItem();
                this.add.Name = menu.Substring(0, semicolon).Trim();
                this.add.Size = new System.Drawing.Size(223, 22);
                this.add.Text = menu.Substring(semicolon + 1);
                this.contextMenuStrip1.Items.Add(this.add);
            }

            Explorer.ContextMenuStrip = this.contextMenuStrip1;
            this.contextMenuStrip1.ResumeLayout(false);

            contextMenuStrip1.ItemClicked += new ToolStripItemClickedEventHandler(contextMenuStrip3_ItemClicked);

            InitFDE();

            FileExplorer.DoubleClick += new EventHandler(FileExplorer_DoubleClick);
        }

        private void FileExplorer_DoubleClick(Object sender, EventArgs e)
        {
            if (Node != null)
                if (Node.Name.Substring(0, 1).CompareTo("f") == 0)
                    FilePicked(FilePath());
        }

        private void contextMenuStrip3_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            string s = e.ClickedItem.Name;
            contextMenuStrip1.Close();
            switch (s)
            {
                case "1":
                    Picked1();
                    break;
                case "2":
                    Picked2();
                    break;
                case "3":
                    Picked3();
                    break;
                case "4":
                    Picked4();
                    break;
            }
        }

        public void setHomeDirectory(string dir)
        {
            TreeNode NNN;
            HomeDirectory = dir;
            NNN = FileExplorer.Nodes.Add("homedir", "Личный каталог", 1);
            NNN.ForeColor = Color.DarkOrange;
        }

        private void InitFDE()
        {
            FileExplorer.SelectedImageIndex = 3;
            Drives = Directory.GetLogicalDrives();
            int i = 1;
            foreach (string s in Drives)
            {
                Node = FileExplorer.Nodes.Add("d" + i.ToString(), s, 0);
                Node.ImageIndex = 0;
                Node.ForeColor = Color.Black;
                i++;
            }
            FileExplorer.AfterSelect += new TreeViewEventHandler(FileExplorer_AfterSelect);
        }

        private void FileExplorer_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            string[] DDF = null;
            if (Node != e.Node)
            {
                int Patthh = 0;
                Node = e.Node;
                TreeNode NNN;
                Node.Nodes.Clear();
                switch (Node.Name.Substring(0, 1))
                {
                    case "h":
                    case "d":
                    case "r":
                        aFile = false;
                        try
                        {
                            DDF = Directory.GetDirectories(FilePath());
                            int i = 1;
                            foreach (string s in DDF)
                            {
                                Patthh = FilePath().Length;
                                if (FilePath().Substring(FilePath().Length - 1).CompareTo("\\") != 0) Patthh++;
                                NNN = Node.Nodes.Add("r" + Node.Name + "_" + i.ToString(), s.Substring(Patthh), 1);
                                NNN.ForeColor = Color.Black;
                                i++;
                            }
                        }
                        catch { }
                        try
                        {
                            DDF = Directory.GetFiles(FilePath(), FilesSelector);
                            int i = 1;
                            foreach (string s in DDF)
                            {
                                Patthh = FilePath().Length;
                                if (FilePath().Substring(FilePath().Length - 1).CompareTo("\\") != 0) Patthh++;
                                NNN = Node.Nodes.Add("f" + Node.Name + "_" + i.ToString(), s.Substring(Patthh), 2);
                                NNN.ForeColor = Color.DarkBlue;
                                i++;
                            }
                        }
                        catch { }
                        break;
                    case "f":
                        if (useWebbros)
                        {
                            aFile = true;
                            FilePicked(FilePath());
                        }
                        break;
                }
            }
        }
        private bool useWebbros = false;

        private string FilePath()
        {
            TreeNode nnn = Node;
            string path = nnn.Text;
            if (nnn.Parent == null)
            {
                if (nnn.Name.Substring(0, 1).CompareTo("h") == 0)
                {
                    if (HomeDirectory.Substring(HomeDirectory.Length - 1).CompareTo("\\") != 0)
                        path = HomeDirectory + "\\";
                    else
                        path = HomeDirectory;
                }
            }
            else
                while (nnn.Parent != null)
                {
                    nnn = nnn.Parent;
                    if (nnn.Name.Substring(0, 1).CompareTo("h") == 0)
                    {
                        if (HomeDirectory.Substring(HomeDirectory.Length - 1).CompareTo("\\") != 0)
                            path = HomeDirectory + "\\" + path;
                        else
                            path = HomeDirectory + path;
                    }
                    else
                    {
                        if (nnn.Text.Substring(nnn.Text.Length - 1).CompareTo("\\") != 0)
                            path = nnn.Text + "\\" + path;
                        else
                            path = nnn.Text + path;
                    }
                }
            return path;
        }
    }

    public class Formularing
    {
        private TextBox[] TextFormular;
        private PictureBox FormularPicture;
        private ListView Formular;
        private ContextMenuStrip contextMenuStripFormular;
        private ToolStripMenuItem добавитьЗаписьToolStripMenuItem;
        private ToolStripMenuItem удалитьЗаписьToolStripMenuItem;
        private ToolStripMenuItem редактироватьЗаписьToolStripMenuItem;
        private ToolStripMenuItem ЗабратьФормулярToolStripMenuItem;
        private ColumnHeader columnHeaderName;
        private ColumnHeader columnHeaderPage;
        private ColumnHeader columnHeaderNote;
        private ColumnHeader columnHeaderKeaper;
        private ToolStrip tsFormular;
        private ToolStripItem tsiAddFormular;
        private ToolStripItem tsiEditFormular;
        private ToolStripItem tsiDeleteFormular;
        private ToolStripItem tsiGetFormular;
        private ToolStripItem tsiSaveFormular;

        ListViewItem ItmFmr = null;
        private AMAS_DBI.Class_syb_acc AMASacc = null;
        private int DocumentID = 0;

        public Formularing(Control ConFormul, int document, AMAS_DBI.Class_syb_acc ACC)
        {
            DocumentID = document;
            AMASacc = ACC;
            ListFormular(ConFormul);

            FormularToolStrip();

            if (Formular==null) Formular = new ListView();
            Formular.Items.Clear();
            Formular.Columns.Clear();
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPage = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNote = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderKeaper = new System.Windows.Forms.ColumnHeader();
            this.Formular.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderPage,
            this.columnHeaderKeaper,
            this.columnHeaderNote});
            this.Formular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Formular.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Formular.LabelEdit = true;
            this.Formular.Location = new System.Drawing.Point(0, 0);
            this.Formular.MultiSelect = false;
            this.Formular.Name = "Formular";
            this.Formular.ShowGroups = false;
            this.Formular.Size = new System.Drawing.Size(630, 492);
            this.Formular.TabIndex = 0;
            this.Formular.UseCompatibleStateImageBehavior = false;
            this.Formular.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Наименование приложения";
            this.columnHeaderName.Width = 174;
            // 
            // columnHeaderPage
            // 
            this.columnHeaderPage.Text = "Листов";
            this.columnHeaderPage.Width = 64;
            // 
            // columnHeaderKeaper
            // 
            this.columnHeaderKeaper.Text = "Держатель";
            this.columnHeaderKeaper.Width = 176;
            // 
            // columnHeaderNote
            // 
            this.columnHeaderNote.Text = "Описание";
            this.columnHeaderNote.Width = 200;

            this.contextMenuStripFormular = new System.Windows.Forms.ContextMenuStrip();
            this.добавитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFormular.SuspendLayout();
            Formular.ContextMenuStrip = this.contextMenuStripFormular;

            this.contextMenuStripFormular = new System.Windows.Forms.ContextMenuStrip();
            this.ЗабратьФормулярToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFormular.SuspendLayout();
            Formular.ContextMenuStrip = this.contextMenuStripFormular;

            Formular.Controls.Add(FormularPicture);
            // 
            // contextMenuStripFormular
            // 
            this.contextMenuStripFormular.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.ЗабратьФормулярToolStripMenuItem});
            this.contextMenuStripFormular.Name = "contextMenuStrip1";
            this.contextMenuStripFormular.Size = new System.Drawing.Size(173, 70);
            // 
            // добавитьЗаписьToolStripMenuItem
            // 
            this.ЗабратьФормулярToolStripMenuItem.Name = "ЗабратьФормулярToolStripMenuItemToolStripMenuItem";
            this.ЗабратьФормулярToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.ЗабратьФормулярToolStripMenuItem.Text = "Забрать формуляр";
            this.ЗабратьФормулярToolStripMenuItem.Click += new System.EventHandler(this.ЗабратьФормулярToolStripMenuItem_Click);

            // список формуляра
            string OName = "";
            int num = 0;
            if (AMASacc.Set_table("TPat2", AMAS_Query.Class_AMAS_Query.MetadataFormular(DocumentID), null))
            {
                try
                {
                    Formular.Items.Clear();
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                    {
                        AMASacc.Get_row(i);
                        ItmFmr = this.Formular.Items.Add("");
                        ItmFmr.SubItems.Add("");
                        ItmFmr.SubItems.Add("");
                        ItmFmr.SubItems.Add("");
                        AMASacc.Find_Field("doc_name");
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0)
                            ItmFmr.SubItems[0].Text = (string)AMASacc.get_current_Field();
                        AMASacc.Find_Field("lists");
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0)
                        {
                            num = (int)AMASacc.get_current_Field();
                            ItmFmr.SubItems[1].Text = num.ToString();
                        }
                        AMASacc.Find_Field("denote");
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0)
                            ItmFmr.SubItems[3].Text = (string)AMASacc.get_current_Field();
                        AMASacc.Find_Field("fio");
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0)
                            ItmFmr.SubItems[2].Text = (string)AMASacc.get_current_Field();
                        AMASacc.Find_Field("id");
                        OName = AMASacc.get_current_Field().GetType().ToString();
                        if (OName.CompareTo("System.DBNull") != 0)
                        {
                            int Key = (int)AMASacc.get_current_Field();
                            ItmFmr.Name = "K" + Key.ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    AMASacc.EBBLP.AddError(ex.Message, "Pattern - 5", ex.StackTrace);
                }
                AMASacc.ReturnTable();
            }
        }

        private void FormularToolStrip()
        {
            if (tsFormular != null)
                if (DocumentID == 0)
                {
                    //
                    // tsiAddFormular
                    //
                    this.tsiAddFormular = new System.Windows.Forms.ToolStripButton();
                    this.tsiAddFormular.BackColor = System.Drawing.Color.Tan;
                    this.tsiAddFormular.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                    this.tsiAddFormular.Image = ((System.Drawing.Image)ClassPattern.Properties.Resources.AddRecord);
                    this.tsiAddFormular.ImageTransparentColor = System.Drawing.Color.Magenta;
                    this.tsiAddFormular.Name = "tsiAddFormular";
                    this.tsiAddFormular.Size = new System.Drawing.Size(23, 22);
                    this.tsiAddFormular.Text = "Добавить приложение";
                    this.tsiAddFormular.ToolTipText = "Добавить приложение";
                    this.tsiAddFormular.Visible = true;
                    this.tsiAddFormular.Click += new System.EventHandler(добавитьЗаписьToolStripMenuItem_Click);
                    //
                    // tsiEditFormular
                    //
                    this.tsiEditFormular = new System.Windows.Forms.ToolStripButton();
                    this.tsiEditFormular = new System.Windows.Forms.ToolStripButton();
                    this.tsiEditFormular.BackColor = System.Drawing.Color.Tan;
                    this.tsiEditFormular.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                    this.tsiEditFormular.Image = ((System.Drawing.Image)(ClassPattern.Properties.Resources.Editrecord));
                    this.tsiEditFormular.ImageTransparentColor = System.Drawing.Color.Magenta;
                    this.tsiEditFormular.Name = "tsiAddFormular";
                    this.tsiEditFormular.Size = new System.Drawing.Size(23, 22);
                    this.tsiEditFormular.Text = "Редактировать запись";
                    this.tsiEditFormular.ToolTipText = "Редактировать запись";
                    this.tsiEditFormular.Visible = true;
                    this.tsiAddFormular.Click += new System.EventHandler(редактироватьЗаписьToolStripMenuItem_Click);
                    //
                    // tsiDeleteFormular
                    //
                    this.tsiDeleteFormular = new System.Windows.Forms.ToolStripButton();
                    this.tsiDeleteFormular = new System.Windows.Forms.ToolStripButton();
                    this.tsiDeleteFormular.BackColor = System.Drawing.Color.Tan;
                    this.tsiDeleteFormular.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                    this.tsiDeleteFormular.Image = ((System.Drawing.Image)ClassPattern.Properties.Resources.DeleteRecord);
                    this.tsiDeleteFormular.ImageTransparentColor = System.Drawing.Color.Magenta;
                    this.tsiDeleteFormular.Name = "tsiDeleteFormular";
                    this.tsiDeleteFormular.Size = new System.Drawing.Size(23, 22);
                    this.tsiDeleteFormular.Text = "Удалить запись";
                    this.tsiDeleteFormular.ToolTipText = "Удалить запись";
                    this.tsiDeleteFormular.Visible = true;
                    this.tsiDeleteFormular.Click += new System.EventHandler(удалитьЗаписьToolStripMenuItem_Click);
                    //
                    // tsiSaveFormular
                    //
                    this.tsiSaveFormular = new System.Windows.Forms.ToolStripButton();
                    this.tsiSaveFormular = new System.Windows.Forms.ToolStripButton();
                    this.tsiSaveFormular.BackColor = System.Drawing.Color.Tan;
                    this.tsiSaveFormular.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                    this.tsiSaveFormular.Image = ((System.Drawing.Image)ClassPattern.Properties.Resources.SaveRecord);
                    this.tsiSaveFormular.ImageTransparentColor = System.Drawing.Color.Magenta;
                    this.tsiSaveFormular.Name = "tsiSaveFormular";
                    this.tsiSaveFormular.Size = new System.Drawing.Size(23, 22);
                    this.tsiSaveFormular.Text = "Сохранить запись";
                    this.tsiSaveFormular.ToolTipText = "Сохранить запись";
                    this.tsiSaveFormular.Visible = true;
                    this.tsiSaveFormular.Click += new EventHandler(tsiSaveFormular_Click);

                    this.tsFormular.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tsiAddFormular,
                    this.tsiEditFormular,
                    this.tsiDeleteFormular,
                    this.tsiSaveFormular
                });
                }
                else
                {
                    //
                    // tsiGetFormular
                    //
                    this.tsiGetFormular = new System.Windows.Forms.ToolStripButton();
                    this.tsiGetFormular.BackColor = System.Drawing.Color.Tan;
                    this.tsiGetFormular.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                    this.tsiGetFormular.Image = (System.Drawing.Image)ClassPattern.Properties.Resources.AssignRecord;
                    this.tsiGetFormular.ImageTransparentColor = System.Drawing.Color.Magenta;
                    this.tsiGetFormular.Name = "tsiGetFormular";
                    this.tsiGetFormular.Size = new System.Drawing.Size(23, 22);
                    this.tsiGetFormular.Text = "Забрать приложение";
                    this.tsiGetFormular.ToolTipText = "Забрать приложение";
                    this.tsiGetFormular.Visible = true;
                    this.tsiGetFormular.Click += new System.EventHandler(ЗабратьФормулярToolStripMenuItem_Click);

                    this.tsFormular.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                    this.tsiGetFormular});

                }
        }


        private void ListFormular(Control ConLoc)
        {
            if (ConLoc.Controls.Find("Formular", true).Length < 1)
            {
                //
                // ListviewFormular
                //
                Formular = new ListView();
                Formular.Dock = DockStyle.Fill;
                Formular.Location = new System.Drawing.Point(147, 28);
                Formular.Name = "Formular";
                Formular.Size = new System.Drawing.Size(121, 97);
                Formular.TabIndex = 0;
                Formular.UseCompatibleStateImageBehavior = false;
                Formular.Dock = DockStyle.Fill;
                ConLoc.Controls.Add(Formular);
            //}

            //if (ConLoc.Controls.Find("tsFormular", true).Length < 1)
            //{
                // 
                // toolStrip
                // 
                tsFormular = new ToolStrip();
                this.tsFormular.Dock = System.Windows.Forms.DockStyle.Right;
                this.tsFormular.Location = new System.Drawing.Point(771, 3);
                this.tsFormular.Name = "tsFormular";
                this.tsFormular.Size = new System.Drawing.Size(24, 125);
                this.tsFormular.TabIndex = 1;
                this.tsFormular.Text = "tsFormular";

                ConLoc.Controls.Add(tsFormular);
            }

        }

        public Formularing(Control ConFormul)
        {
            ListFormular(ConFormul);
            Formular = new ListView();
            Formular.Items.Clear();
            Formular.Columns.Clear();

            FormularToolStrip();

            // 
            // Formular
            // 
            this.columnHeaderName = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderPage = new System.Windows.Forms.ColumnHeader();
            this.columnHeaderNote = new System.Windows.Forms.ColumnHeader();
            this.Formular.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeaderName,
            this.columnHeaderPage,
            this.columnHeaderNote});
            this.Formular.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Formular.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.Formular.LabelEdit = true;
            this.Formular.Location = new System.Drawing.Point(0, 0);
            this.Formular.MultiSelect = false;
            this.Formular.Name = "Formular";
            this.Formular.ShowGroups = false;
            this.Formular.Size = new System.Drawing.Size(630, 492);
            this.Formular.TabIndex = 0;
            this.Formular.UseCompatibleStateImageBehavior = false;
            this.Formular.View = System.Windows.Forms.View.Details;
            // 
            // columnHeaderName
            // 
            this.columnHeaderName.Text = "Наименование приложения";
            this.columnHeaderName.Width = 174;
            // 
            // columnHeaderPage
            // 
            this.columnHeaderPage.Text = "Листов";
            this.columnHeaderPage.Width = 64;
            // 
            // columnHeaderNote
            // 
            this.columnHeaderNote.Text = "Описание";
            this.columnHeaderNote.Width = 376;

            DocumentID = 0;
            this.contextMenuStripFormular = new System.Windows.Forms.ContextMenuStrip();
            this.добавитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.редактироватьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.удалитьЗаписьToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contextMenuStripFormular.SuspendLayout();
            Formular.ContextMenuStrip = this.contextMenuStripFormular;
            try { FormularPicture.Visible = false; }
            catch { }

            Formular.Controls.Add(FormularPicture);
            // 
            // contextMenuStripFormular
            // 
            this.contextMenuStripFormular.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.добавитьЗаписьToolStripMenuItem,
                this.редактироватьЗаписьToolStripMenuItem,
                this.удалитьЗаписьToolStripMenuItem});
            this.contextMenuStripFormular.Name = "contextMenuStrip1";
            this.contextMenuStripFormular.Size = new System.Drawing.Size(173, 70);
            // 
            // добавитьЗаписьToolStripMenuItem
            // 
            this.добавитьЗаписьToolStripMenuItem.Name = "добавитьЗаписьToolStripMenuItem";
            this.добавитьЗаписьToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.добавитьЗаписьToolStripMenuItem.Text = "Добавить запись";
            // 
            // редактироватьЗаписьToolStripMenuItem
            // 
            this.редактироватьЗаписьToolStripMenuItem.Name = "редактироватьЗаписьToolStripMenuItem";
            this.редактироватьЗаписьToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.редактироватьЗаписьToolStripMenuItem.Text = "Редактировать запись";
            // 
            // удалитьЗаписьToolStripMenuItem
            // 
            this.удалитьЗаписьToolStripMenuItem.Name = "удалитьЗаписьToolStripMenuItem";
            this.удалитьЗаписьToolStripMenuItem.Size = new System.Drawing.Size(172, 22);
            this.удалитьЗаписьToolStripMenuItem.Text = "Удалить запись";

            this.contextMenuStripFormular.ResumeLayout(false);

            TextFormular = new TextBox[3];
            FormularPicture = new PictureBox();
            TextFormular[0] = new TextBox();
            TextFormular[1] = new TextBox();
            TextFormular[2] = new TextBox();
            TextFormular[0].BackColor = Color.LightSeaGreen;
            TextFormular[1].BackColor = Color.LightSeaGreen;
            TextFormular[2].BackColor = Color.LightSeaGreen;
            FormularPicture.Controls.Add(TextFormular[0]);
            FormularPicture.Controls.Add(TextFormular[1]);
            FormularPicture.Controls.Add(TextFormular[2]);
            FormularPicture.Visible = false;
            this.Formular.Controls.Add(FormularPicture);

            this.добавитьЗаписьToolStripMenuItem.Click += new System.EventHandler(this.добавитьЗаписьToolStripMenuItem_Click);
            this.удалитьЗаписьToolStripMenuItem.Click += new System.EventHandler(this.удалитьЗаписьToolStripMenuItem_Click);
            this.редактироватьЗаписьToolStripMenuItem.Click += new System.EventHandler(this.редактироватьЗаписьToolStripMenuItem_Click);
            TextFormular[0].MouseDoubleClick += new MouseEventHandler(Formularing_MouseDoubleClick);
            TextFormular[1].MouseDoubleClick += new MouseEventHandler(Formularing_MouseDoubleClick);
            TextFormular[2].MouseDoubleClick += new MouseEventHandler(Formularing_MouseDoubleClick);
            TextFormular[0].KeyPress += new KeyPressEventHandler(Formularing_KeyPressN0);
            TextFormular[1].KeyPress += new KeyPressEventHandler(Formularing_KeyPressN1);
            TextFormular[2].KeyPress += new KeyPressEventHandler(Formularing_KeyPress);
        }

        private void Draw_Controls()
        {
        }

        private void Formularing_KeyPressN0(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                TextFormular[1].Focus();
        }

        private void Formularing_KeyPressN1(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                TextFormular[2].Focus();
        }

        private void Formularing_KeyPress(Object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
                if (TextFormular[0].Text.Trim().Length > 0)
                {
                    Write_to_Formular();
                    AddRecord_toFormular();
                }
                else
                {
                    if (ItmFmr != null) ItmFmr.Remove();
                    FormularPicture.Visible = false;
                }
        }

        private void Write_to_Formular()
        {
            try
            {
                if (ItmFmr != null)
                {
                    ItmFmr.SubItems[0].Text = TextFormular[0].Text.Trim();
                    ItmFmr.SubItems[1].Text = TextFormular[1].Text.Trim();
                    ItmFmr.SubItems[2].Text = TextFormular[2].Text.Trim();
                }
            }
            catch { }
            FormularPicture.Visible = false;
        }

        private void Formularing_MouseDoubleClick(Object sender, MouseEventArgs e)
        {
            Write_to_Formular();
        }

        private void добавитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Write_to_Formular();
            AddRecord_toFormular();
        }

        private void AddRecord_toFormular()
        {
            ListViewItem ItemF = Formular.Items.Add("Новое приложение");
            ItemF.SubItems.Add("1");
            ItemF.SubItems.Add("-");
            TextFormular[0].Text = "";
            TextFormular[1].Text = "";
            TextFormular[2].Text = "";

            Forpic_resize(ItemF);
        }

        private void ЗабратьФормулярToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (ListViewItem FItem in Formular.SelectedItems)
            {
                string Who_have = AMASCommand.Move_formular((int)Convert.ToInt32(FItem.Name.Substring(1)), "");
                if (Who_have.Trim().Length > 0) FItem.SubItems[2].Text = Who_have;
            }
        }

        private void tsiSaveFormular_Click(object sender, EventArgs e)
        {
            Write_to_Formular();
        }

        private void удалитьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RemoveRecords();
        }

        private void RemoveRecords()
        {
            foreach (ListViewItem ItemF in Formular.SelectedItems)
                ItemF.Remove();
        }

        private void редактироватьЗаписьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                TextFormular[0].Text = Formular.SelectedItems[0].Text;
                TextFormular[1].Text = Formular.SelectedItems[0].SubItems[1].Text;
                TextFormular[2].Text = Formular.SelectedItems[0].SubItems[2].Text;
                Forpic_resize(Formular.SelectedItems[0]);
            }
            catch { }
        }

        private void Forpic_resize(ListViewItem ItmF)
        {
            if (ItmF != null)
                if (DocumentID < 1)
                {
                    ItmFmr = ItmF;
                    FormularPicture.Left = 0;
                    FormularPicture.Top = ItmF.Position.Y;
                    FormularPicture.Width = Formular.Width;
                    FormularPicture.Height = ItmF.ListView.Height;
                    TextFormular[0].Left = 0;
                    TextFormular[0].Width = Formular.Columns[0].Width;
                    TextFormular[1].Left = TextFormular[0].Width + 2;
                    TextFormular[1].Width = Formular.Columns[1].Width;
                    TextFormular[2].Left = TextFormular[1].Width + TextFormular[0].Width + 4;
                    TextFormular[2].Width = FormularPicture.Width - TextFormular[2].Left - 6;
                    FormularPicture.Height = 50;
                    TextFormular[0].Height = FormularPicture.Height;
                    TextFormular[1].Height = FormularPicture.Height;
                    TextFormular[2].Height = FormularPicture.Height;
                    TextFormular[0].Top = 0;
                    TextFormular[1].Top = 0;
                    TextFormular[2].Top = 0;
                    FormularPicture.Visible = true;
                    FormularPicture.BringToFront();
                }
        }

        public void FormularList()
        {
            if (AMASacc != null)
            {
                string key = "";
                int num;
                Formular.Clear();
                if (AMASacc.Set_table("TPat3", AMAS_Query.Class_AMAS_Query.MetadataFormular(DocumentID), null))
                {
                    for (int i = 0; i < AMASacc.Rows_count; i++)
                        try
                        {
                            AMASacc.Get_row(i);
                            num = (int)AMASacc.Find_Field("id");
                            key = "F" + num.ToString();
                            Formular.Items.Add(key, (string)AMASacc.Find_Field("docname"));
                            num = (int)AMASacc.Find_Field("lists");
                            Formular.Items[key].SubItems[1].Text = num.ToString();
                            Formular.Items[key].SubItems[2].Text = (string)AMASacc.Find_Field("denote");
                            Formular.Items[key].SubItems[3].Text = (string)AMASacc.Find_Field("fio");
                        }
                        catch (Exception ex)
                        {
                            AMASacc.EBBLP.AddError(ex.Message, "Pattern - 7", ex.StackTrace);
                        }
                    AMASacc.ReturnTable();
                }
            }
        }

        public void Save_formular(int DocId)
        {
            Array formularrray = Array.CreateInstance(typeof(string), Formular.Items.Count, 3);

            for (int i = 0; i < Formular.Items.Count; i++)
            {

                formularrray.SetValue(Formular.Items[i].Text, i, 0);
                formularrray.SetValue(Formular.Items[i].SubItems[1].Text, i, 1);
                formularrray.SetValue(Formular.Items[i].SubItems[2].Text, i, 2);
            }
            AMAS_DBI.AMASCommand.Append_formular(DocId, formularrray);

        }
    }

    public class XMLDocument
    {
        string XMLfile = "";

        public XMLDocument(string aFile)
        {
            XMLfile = aFile;
        }

        public int Kind()
        {
            int ret = -1;
            DataTable XMLTable = new DataTable();
            try
            {
                //XmlReader Xred= 
                XMLTable.ReadXml(XMLfile);
            }
            catch (Exception ex) { XMLTable = null; string res = ex.Message; }
            if (XMLTable != null)
                if (XMLTable.Rows.Count > 0)
                    try
                    {
                        DataRow XR = XMLTable.Rows[0];
                        ret = (int)Convert.ToInt32(XR["DocumentKind"].ToString());
                    }
                    catch { ret = -1; }
            return ret;
        }

        public int Tema()
        {
            DataTable XMLTable = new DataTable();
            try
            {
                XMLTable.ReadXml(XMLfile);
            }
            catch { XMLTable = null; }
            {
                int ret = -1;
                if (XMLTable != null)
                    if (XMLTable.Rows.Count > 0)
                        try
                        {
                            DataRow XR = XMLTable.Rows[0];
                            ret = (int)Convert.ToInt32(XR["DocumentTema"].ToString());
                        }
                        catch { ret = -1; }
                return ret;
            }
        }
    }

}
