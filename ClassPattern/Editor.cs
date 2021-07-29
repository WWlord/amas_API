using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CommonValues;
using System.IO;

namespace ClassPattern
{
    public partial class Editor : UserControl
    {
        public bool Edited = false;

        public delegate void Unfocused();
        public event Unfocused SaveEdit;

        public string TEXT
        {
            get { return rtEditor.Text; }
        }

        public byte[] RTF
        {
            get
            {
                byte[] buf = new byte[rtEditor.Rtf.Length];
                char[] ECh = rtEditor.Rtf.ToCharArray();
                for (int i = 0; i < rtEditor.Rtf.Length; i++)
                    buf[i] = Convert.ToByte(ECh[i]);
                Edited = false;
                return buf;
            }
        }

        public Editor()
        {
            InitializeComponent();
            cbFonts.Items.Clear();
            foreach (FontFamily FF in FontFamily.Families)
            {
                cbFonts.Items.Add(FF.Name);
            }
            cbFonts.SelectedIndexChanged+=new EventHandler(cbFonts_SelectedIndexChanged);
            tsFontsize.SelectedIndexChanged+=new EventHandler(tsFontsize_SelectedIndexChanged);
            rtEditor.TextChanged += new EventHandler(rtEditor_TextChanged);
            rtEditor.LostFocus += new EventHandler(rtEditor_LostFocus);
            Edited = false;
        }

        void rtEditor_LostFocus(object sender, EventArgs e)
        {
            try
            {
                SaveEdit();
            }
            catch { }
        }

        void rtEditor_TextChanged(object sender, EventArgs e)
        {
            Edited = true;
        }

        private void tsbClear_Click(object sender, EventArgs e)
        {
            rtEditor.Clear();
            rtEditor.ClearUndo(); 
        }

        private void tsbFind_Click(object sender, EventArgs e)
        {
            rtEditor.Find(tbFind.Text.ToCharArray());// 
        }

        private void tsbCut_Click(object sender, EventArgs e)
        {
            rtEditor.Cut();
        }

        private void tsbPaste_Click(object sender, EventArgs e)
        {
            rtEditor.Paste(); 
        }

        private void tsbUndo_Click(object sender, EventArgs e)
        {
            rtEditor.Undo();
        }

        private void tsbRedo_Click(object sender, EventArgs e)
        {
            rtEditor.Redo();
        }

        private void cbFonts_SelectedIndexChanged(object sender, EventArgs e)
        {
            change_font();
        }

        private void tsFontsize_SelectedIndexChanged(object sender, EventArgs e)
        {
            change_font();
        }

        private void change_font()
        {
            try
            {
                FontStyle fs = FontStyle.Regular;
                if (tsfontBolt.Checked) fs = fs | FontStyle.Bold;
                if (tsfontitalic.Checked) fs = fs | FontStyle.Italic;
                if (tsfontunderline.Checked) fs = fs | FontStyle.Underline;
                FontFamily FF = new FontFamily(Convert.ToString(cbFonts.SelectedItem));
                Font f = new Font(FF, (float)Convert.ToInt16(tsFontsize.Text), fs);
                rtEditor.SelectionFont = f;
            }
            catch { }
        }

        private void tsfontBolt_Click(object sender, EventArgs e)
        {
            change_font();
        }

        private void tsfontitalic_Click(object sender, EventArgs e)
        {
            change_font();
        }

        private void tsfontunderline_Click(object sender, EventArgs e)
        {
            change_font();
        }

        public string SaveToFile()
        {
            string filename = CommonClass.TempDirectory + "editor.rtf";
            try { File.Delete(filename); }
            catch { }
            rtEditor.SaveFile(filename);
            Edited = false;
            return filename;
        }

        public string SaveToTextFile()
        {
            string filename = CommonClass.TempDirectory + "editor.txt";
            try { File.Delete(filename); }
            catch { }
            File.WriteAllText(filename, rtEditor.Text);
            Edited = false;
            return filename;
        }

        public void LoadFromFile(string Filename)
        {
            rtEditor.LoadFile(Filename);
            Edited = false;
        }

        public void LoadFromTextFile(string Filename)
        {
            byte[] buff=new byte[100];
            rtEditor.Clear();
            rtEditor.Text = File.ReadAllText(Filename);
            Edited = false;
        }

        private bool aEditable = true;
        public bool Editable
        {
            get 
            { 
                return aEditable; 
            }
            set 
            { 
                aEditable = value; 
                if (aEditable) 
                    tsCommands.Visible = true; 
                else tsCommands.Visible = false; 
            }
        }

        public void Set_color( Color Coler)
        {
            rtEditor.BackColor = Coler;
        }

        public void Clear()
        {
            rtEditor.Clear();
        }
    }
}
