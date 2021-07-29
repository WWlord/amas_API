using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPFAMASControlRegisters
{
    /// <summary>
    /// Логика взаимодействия для WPFAddress.xaml
    /// </summary>
    public partial class WPFAddress : UserControl
    {
            private AMAS_DBI.Class_syb_acc AMASacc;

            private Address_ids state_ids;

        public WPFAddress(AMAS_DBI.Class_syb_acc Acc)
        {
            InitializeComponent();

            AMASacc = Acc;

            state_ids = new Address_ids(cbState);
        }
    }

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
        ComboBox AddressBox;

        public Address_ids(ComboBox CBox)
        {
            AddressBox = CBox;
            this.AddressBox.ManipulationStarting += new EventHandler<ManipulationStartingEventArgs>(AddressBox_ManipulationStarting); 
            this.AddressBox.TextInput+=new TextCompositionEventHandler(AddressBox_TextInput); //.TextChanged += new EventHandler(this_AddressBox_TextChanged);
            this.AddressBox.LostFocus +=new RoutedEventHandler(AddressBox_LostFocus); 
            this.AddressBox.SelectionChanged+=new SelectionChangedEventHandler(AddressBox_SelectionChanged); //.SelectedIndexChanged += new EventHandler(this_SelectedIndexChanged);
            this.AddressBox.KeyDown+=new KeyEventHandler(AddressBox_KeyDown); 
            this.AddressBox.KeyUp+=new KeyEventHandler(AddressBox_KeyUp); //+= new KeyEventHandler(this_AddressBox_KeyUp);
        }


        void AddressBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Child != null) Child.clear();
        }

        void AddressBox_LostFocus(object sender, RoutedEventArgs e)
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

        void AddressBox_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (new_Address)
            {
                AddressBox.Text = textBuffer + backtextBuffer;
                //AddressBox. .SelectionStart = textBuffer.Length;
                //AddressBox.SelectionLength = backtextBuffer.Length;
            }
 
        }

        void AddressBox_ManipulationStarting(object sender, ManipulationStartingEventArgs e)
        {
            cleanNewAddress();
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

        void AddressBox_KeyUp(object sender, KeyEventArgs e)
        {
//            throw new NotImplementedException();
//        }

 //       private void this_AddressBox_KeyUp(object sender, System.Windows.Forms.KeyEventArgs e)
//        {
            switch ((char)e.Key)
            {
                case (char)Key.Left:
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
                case (char)Key.Right:
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
                case (char)Key.Delete:
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

        void AddressBox_KeyDown(object sender, KeyEventArgs e)
        {
//            throw new NotImplementedException();
//        }
//
//        private void this_KeyPress(object sender, KeyPressEventArgs e)
//        {
            switch ((char)e.Key) // .KeyChar)
            {
                case (char)Key.Back:
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
                case (char)Key.End:
                    if (new_Address)
                    {
                        textBuffer += backtextBuffer;
                        backtextBuffer = "";
                    }
                    break;
                case (char)Key.Home:
                    if (new_Address)
                    {
                        backtextBuffer = textBuffer + backtextBuffer;
                        textBuffer = "";
                    }
                    break;
                case (char)Key.Escape:
                    backtextBuffer = "";
                    textBuffer = "";
                    AddressBox.Text = "";
                    if (Child != null)
                        Child.clear();
                    break;
                default:
                    if (((char)e.Key >= " ".ToCharArray()[0] && (char)e.Key <= "z".ToCharArray()[0]) || ((char)e.Key >= "А".ToCharArray()[0] && (char)e.Key <= "я".ToCharArray()[0]))
                    {
                        if (new_Address)
                        {
                            textBuffer += Convert.ToString(e.Key);
                            int number = get_number_by_text(textBuffer);
                            if (number >= 0) backtextBuffer = AddressName[number].Substring(textBuffer.Length);
                            else backtextBuffer = "";
                        }
                        else
                        {
                            new_Address = true;
                            textBuffer = AddressBox.Text + Convert.ToString(e.Key); ;
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
                AddressBox.Items.Refresh();
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
                AMASacc.EBBLP.AddError(err.Message, "WPFAddress - 81", err.StackTrace);
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
                AMASacc.EBBLP.AddError(err.Message, "WPFAddress - 82", err.StackTrace);
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
                AMASacc.EBBLP.AddError(err.Message, "WPFAddress - 83", err.StackTrace);
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
                    AMASacc.EBBLP.AddError(err.Message, "WPFAddress - 84", err.StackTrace);
                    ResultErr = err.Message;
                }
                AMASacc.ReturnTable();
            }
            if (array_dimention > 0)
            {
                AddressBox.SelectedIndex = 0;
                AddressBox.Items.Refresh();
            }
            else
            {
                current_number = -1;
                AddressBox.SelectedIndex = -1;
                AddressBox.Text = "";
                AddressBox.Items.Refresh();
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

}
