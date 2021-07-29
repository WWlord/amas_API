using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;
using CommonValues;

namespace ClassPattern
{
    public partial class PictersLibrary : UserControl
    {
        const int HeadExt = 24;
        const int HeadLenght = 12;

        public delegate void ScanDoc(string Filename);
        public event ScanDoc Scanned;

        public delegate void Closing();
        public event Closing ScannerClose;

        private bool AddRemSavePicters = true;

        private ClassPattern.baseLayer.PicLibSettings PLSettings = new ClassPattern.baseLayer.PicLibSettings();

        private bool SaveToFile = false;

        private bool Widthing=false;
        private bool Heighting=false;

        public bool SizeHeight
        {
            get { return Heighting; }
            set { Heighting = value; if (Heighting) Widthing = false; PicterResize(); }
        }
        
        public bool SizeWidth
        {
            get { return Widthing; }
            set { Widthing = value; if (Widthing) Heighting = false; PicterResize(); }
        }

        public bool Saved
        {
            get { return SaveToFile; }
            set { SaveToFile = value; }
        }

        public bool Editable
        {
            get { return AddRemSavePicters; }
            set
            {
                AddRemSavePicters = value;
                if (AddRemSavePicters)
                {
                    for (int i = 0; i < toolStrip1.Items.Count; i++)
                        if (toolStrip1.Items[i].Name.Contains("ed"))
                            toolStrip1.Items[i].Visible = true;
                        else
                            toolStrip1.Items[i].Visible = false;
                }
                else
                {
                    for (int i = 0; i < toolStrip1.Items.Count; i++)
                        if (toolStrip1.Items[i].Name.Contains("Wk"))
                            toolStrip1.Items[i].Visible = true;
                        else
                            toolStrip1.Items[i].Visible = false;
                }
            }
        }

        public PictersLibrary()
        {
            InitializeComponent();

            pbPicter.SizeMode = PictureBoxSizeMode.Zoom;
            pbPicter.SendToBack();

            trackBarPicter.TickFrequency = 10;
            trackBarPicter.Minimum = 10;
            trackBarPicter.Maximum = 500;
            trackBarPicter.ValueChanged += new EventHandler(trackBarPicter_ValueChanged);
            edWktsComboBox.SelectedIndexChanged += new EventHandler(edWktsComboBox_SelectedIndexChanged);
            edWktsComboBox.TextChanged += new EventHandler(edWktsComboBox_TextChanged);
            trackBarPicter.MouseLeave += new EventHandler(trackBarPicter_MouseLeave);
            openFilePicters.InitialDirectory = Path.GetPathRoot(Path.GetTempPath());

            if (panelpicters.Dock == DockStyle.Bottom)
                panelpicters.Height = 100;
            else panelpicters.Width = 100;

            panelPicter.Resize += new EventHandler(panelPicter_Resize);

            GetSetting();
        }

        void GetSetting()
        {
                PLSettings.SettingsKey = "PicLibrary";
                edWktsComboBox.Text = PLSettings.Zoom.ToString();
                RollDown = PLSettings.RollDown;
        }

        void SaveSetting()
        {
            try
            {
                PLSettings.SettingsKey = "PicLibrary";
                PLSettings.Zoom = (int)Convert.ToInt32(edWktsComboBox.Text);
                PLSettings.RollDown = RollDown;
                PLSettings.Save();
            }
            catch { }
        }
        
        void panelPicter_Resize(object sender, EventArgs e)
        {
            PicterResize();
        }

        public void PicterResize()
        {
            int shift=0;
            if (Widthing)
            {
                try
                {
                    pbPicter.Top = toolStrip1.Height + 1;
                    pbPicter.Left = 1;
                    if (pbPicter.Height < pbPicter.Image.Size.Height) shift = 10;
                    pbPicter.Width = panelPicter.Width-2-shift;
                    pbPicter.Height = pbPicter.Width * pbPicter.Image.Size.Height / pbPicter.Image.Size.Width;
                    if (pbPicter.Width < panelPicter.Width-shift)
                        pbPicter.Left = (panelPicter.Width - pbPicter.Width-shift) / 2;
                    else pbPicter.Left = 1;
                }
                catch { }
            }
            else if (Heighting)
            {
                try
                {
                    pbPicter.Left = 1;
                    pbPicter.Top = toolStrip1.Height + 1;
                    if (pbPicter.Width < pbPicter.Image.Size.Width) shift = 10;
                    pbPicter.Height = panelPicter.Height - toolStrip1.Height - 2 - shift;
                    pbPicter.Width = pbPicter.Height * pbPicter.Image.Size.Width / pbPicter.Image.Size.Height;
                    if (pbPicter.Width < panelPicter.Width-shift)
                        pbPicter.Left = (panelPicter.Width - pbPicter.Width-shift) / 2;
                    else pbPicter.Left = 1;
                }
                catch { }
            }
            else
                ZoomPicter();
        }

        public void Close()
        {
            SaveLP(); 
        }

        void trackBarPicter_MouseLeave(object sender, EventArgs e)
        {
            trackBarPicter.Visible = false;
        }

        void edWktsComboBox_TextChanged(object sender, EventArgs e)
        {
            Widthing = false;
            Heighting = false;
            ZoomPicter();
        }

        void edWktsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Widthing = false;
            Heighting = false;
            ZoomPicter();
            SaveSetting();
        }

        private void ZoomPicter()
        {
            int z;
            try
            {
                z = Convert.ToInt32(edWktsComboBox.Text);
            }
            catch { z = 0; }
            if (z > 0)
            {
                if (z > 1000) z = 1000;
                if (z < 10) z = 10;
                Zoom = z;
                if (trackBarPicter.Value != z) trackBarPicter.Value = z;
            }
            else trackBarPicter.Visible = true;
        }

        void trackBarPicter_ValueChanged(object sender, EventArgs e)
        {
            edWktsComboBox.Text = trackBarPicter.Value.ToString();
        }

        PicterLeaf[] pic = null;

        private int zooming=100;

        public int Zoom
        {
            get { return zooming; }
            set
            {
                zooming = value;
                PLSettings.Zoom = zooming;
                PLSettings.Save();
                Widthing = false;
                Heighting = false;
                try
                {
                    pbPicter.Height = pbPicter.Image.Size.Height * zooming / 100;
                    pbPicter.Width = pbPicter.Image.Size.Width * zooming / 100;
                    if (pbPicter.Width < panelPicter.Width)
                        pbPicter.Left = (panelPicter.Width - pbPicter.Width) / 2;
                    else pbPicter.Left = 1;
                    pbPicter.Top = toolStrip1.Height + 1;
                }
                catch { }
            }
        }

        private class PicterLeaf : PictureBox
        {
            int Index;
            string Fname = "";
            public int PicIndex
            {
                get { return Index; }
                set { Index = value; }
            }

            public string FileName
            {
                get { return Fname; }
            }

            public PicterLeaf(int inx, string FN)
            {
                Index = inx;
                Fname = FN;
            }

        }

        private void tsAdd_Click(object sender, EventArgs e)
        {
            openFilePicters.Filter = "Библиотека|*.AMASplb";
            openFilePicters.Multiselect = false;
            openFilePicters.Title = "Открыть библиотеку";
            openFilePicters.CheckFileExists = true;
            openFilePicters.DefaultExt = "AMASplb";
            if (openFilePicters.ShowDialog() == DialogResult.OK)
            {
                ClearPicters();
                ShowPicters(loadLibrary(openFilePicters.FileName));
            }
        }

        public void OpenLibrary(string FileName)
        {
            ClearPicters();
            ShowPicters(loadLibrary(FileName));
        }

        private void ClearPicters()
        {
            pbPicter.Image = null;
            if (pic != null)
                for (int i = 0; i < pic.Length; i++)
                    panelpicters.Controls.Remove(pic[i]);
            pic = null;
        }

        private string[] loadLibrary(string FileName)
        {
            string[] Picters=null;
            FileStream fLibrary=null;
            try
            {
                fLibrary = new FileStream(FileName, FileMode.Open, FileAccess.Read);
                string ToFile;
                byte[] buff;
                int poslib = 0;
                string fileExt;
                string fileLenght;
                int picnumber = 1;
                while (poslib < fLibrary.Length - HeadExt - HeadLenght)
                {
                    try
                    {
                        buff = new byte[HeadExt];
                        fLibrary.Read(buff, 0, HeadExt);
                        poslib += HeadExt;
                        fileExt = "";
                        for (int i = 0; i < buff.Length; i++)
                            fileExt += Convert.ToString((char)buff[i]);
                        fileExt = fileExt.Trim();

                        buff = new byte[HeadLenght];
                        fLibrary.Read(buff, 0, HeadLenght);
                        poslib += HeadLenght;
                        fileLenght = "";
                        for (int i = 0; i < buff.Length; i++)
                            fileLenght += Convert.ToString((char)buff[i]);

                        ToFile = Path.GetTempFileName();
                        if (File.Exists(ToFile))
                            File.Delete(ToFile);
                        FileStream fPicters = new FileStream(ToFile, FileMode.CreateNew);
                        buff = new byte[Convert.ToInt32(fileLenght)];
                        fLibrary.Read(buff, 0, Convert.ToInt32(fileLenght));
                        poslib += Convert.ToInt32(fileLenght);

                        fPicters.Write(buff, 0, Convert.ToInt32(fileLenght));
                        fPicters.Close();

                        if (Picters == null)
                        {
                            Picters = new string[1];
                            Picters[0] = ToFile;
                        }
                        else
                        {
                            string[] tempic = new string[Picters.Length + 1];
                            Picters.CopyTo(tempic, 0);
                            tempic[Picters.Length] = ToFile;
                            Picters = tempic;
                        }
                        picnumber++;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("картинку № " + picnumber.ToString() + " невозможно загрузить. /n" + ex.Message);
                        poslib = (int)fLibrary.Length;
                    }
                }
            }
            catch
            {
                MessageBox.Show("Библиотеку невозможно загрузить");
            }
            finally
            {
                if (fLibrary!=null) fLibrary.Close();
            }

            return Picters;
        }

        private void edWktsComboBox_Click(object sender, EventArgs e)
        {
            
        }

        private void trackBarPicter_Scroll(object sender, EventArgs e)
        {
           
        }

        private void tsSave_Click(object sender, EventArgs e)
        {
            SaveLP();
        }

        private void SaveLP()
        {
            if (pic != null)
            {
                string FN = "";
                if (SaveToFile)
                {
                    openFilePicters.Filter = "Библиотека|*.AMASplb";
                    openFilePicters.Multiselect = false;
                    openFilePicters.Title = "Сохранить картинки";
                    openFilePicters.CheckFileExists = false;
                    openFilePicters.DefaultExt = "AMASplb";
                    if (openFilePicters.ShowDialog() == DialogResult.OK)
                    {
                        FN = openFilePicters.SafeFileName;
                    }
                }
                else
                {
                    FN = CommonValues.CommonClass.TempDirectory.Trim();
                    if (FN.Substring(FN.Length - 1, 1).CompareTo("\\") != 0) FN += "\\";

                    int ii = 1;
                    while (ii > 0)
                    {
                        FileInfo ff = new FileInfo(FN + "PICLIB" + ii.ToString() + ".AMASplb");
                        if (ff.Exists)
                            try
                            {
                                ff.Delete();

                            }
                            catch
                            {
                            }
                        if (ff.Exists)
                            ii++;
                        else
                        {
                            FN += "PICLIB" + ii.ToString() + ".AMASplb";
                            ii = -1;
                        }
                    }
                    if (FN.Length > 0)
                    {
                        string ext = CommonValues.CommonClass.getfileExtention(FN);
                        if (ext.Length > 0)
                            FN = FN.Substring(0, FN.Length - ext.Length) + "AMASplb";
                        else
                            FN += ".AMASplb";
                        SavePicterLibrary(FN);
                        Scanned(FN);
                    }
                }
                ClearPicters();
            }
        }


        public void SavePicterLibrary(string ToFile)
        {
            FileStream fPicters = null ;
            try
            {
                if (File.Exists(ToFile))
                    File.Delete(ToFile);
                fPicters = new FileStream(ToFile, FileMode.CreateNew);
                byte[] buff;
                for (int i = 0; i < pic.Length; i++)
                {
                    try
                    {
                        char[] extchar = CommonValues.CommonClass.getfileExtention(ToFile).PadRight(HeadExt, ' ').ToCharArray();
                        for (int l = 0; l < HeadExt; l++)
                            fPicters.WriteByte((byte)extchar[l]);

                        buff = File.ReadAllBytes(pic[i].FileName);
                        extchar = buff.LongLength.ToString().PadLeft(HeadLenght, '0').ToCharArray();
                        for (int l = 0; l < HeadLenght; l++)
                            fPicters.WriteByte((byte)extchar[l]);
                        fPicters.Write(buff, 0, (int)buff.LongLength);
                    }
                    catch
                    {
                        MessageBox.Show("файл " + " не сохранён");
                    }
                }
            }
            catch
            {
                MessageBox.Show("Библиотека не сформирована");
            }
            finally
            {
                if (fPicters!=null) fPicters.Close();
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        private void tsLeft_Click(object sender, EventArgs e)
        {
            if (PicterNumber > 0)
            {
                PicterNumber--;
                pic[PicterNumber].Select();
                PISel(pic[PicterNumber]);
            }
        }

        private void tsRight_Click(object sender, EventArgs e)
        {
            if (PicterNumber < pic.Length-1)
            {
                PicterNumber++;
                pic[PicterNumber].Select();
                PISel(pic[PicterNumber]);
            }
        }

        private void depot(string ToFile)
        {
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/jpeg");
            System.Drawing.Imaging.Encoder myEncoder;
            EncoderParameter myEncoderParameter;
            EncoderParameters myEncoderParameters;
            myEncoderParameters = new EncoderParameters(0);

            //Set Quality
            myEncoder = System.Drawing.Imaging.Encoder.Quality;
            myEncoderParameter = new EncoderParameter(
                        myEncoder,
                        75L);
            //myEncoderParameters.Param[0] = myEncoderParameter;

            //Set Color
            myEncoder = System.Drawing.Imaging.Encoder.ColorDepth;
            myEncoderParameter = new EncoderParameter(
                        myEncoder,
                        24L);
            //myEncoderParameters.Param[1] = myEncoderParameter;

            //Set Compression
            myEncoder = System.Drawing.Imaging.Encoder.Compression;
            myEncoderParameter = new EncoderParameter(
                        myEncoder,
                        (long)EncoderValue.CompressionNone);
            //myEncoderParameters.Param[2] = myEncoderParameter;

            if (File.Exists(ToFile))
                File.Delete(ToFile);
            Stream qq = new MemoryStream();
            Stream fPicters = new FileStream(ToFile, FileMode.Create);
            for (int i = 0; i < pic.Length; i++)
            {
                if (i == 0)
                    try
                    {
                        switch (CommonClass.getfileExtention(pic[i].FileName.ToLower()))
                        {
                            case "jpg":
                            case "jpeg":
                                myImageCodecInfo = GetEncoderInfo("image/jpeg");
                                myEncoderParameters = new EncoderParameters(1);

                                //Set Quality
                                myEncoder = System.Drawing.Imaging.Encoder.Quality;
                                myEncoderParameter = new EncoderParameter(
                                            myEncoder,
                                            75L);
                                myEncoderParameters.Param[0] = myEncoderParameter;
                                break;
                            case "gif":
                            case "giff":
                                myImageCodecInfo = GetEncoderInfo("image/gif");
                                break;
                            case "png":
                                myImageCodecInfo = GetEncoderInfo("image/png");
                                break;
                        }
                        pic[i].Image.Save(pic[i].FileName, myImageCodecInfo, myEncoderParameters);
                        //pic[i].Image.Save (ToFile, myImageCodecInfo, myEncoderParameters);
                    }
                    catch //(Exception ex)
                    {
                    }
                else
                    try
                    {
                        pic[i].Image.SaveAdd(myEncoderParameters);
                    }
                    catch //(Exception ex)
                    {
                    }
            }
            fPicters.Flush();
            fPicters.Close();

        }

        private void edtsAddList_Click(object sender, EventArgs e)
        {
            openFilePicters.Filter = "Графика|*.jpg;*.jpeg;*.gif;*.giff;*.png";
            openFilePicters.Multiselect = true;
            openFilePicters.Title = "Выбор картинок";
            openFilePicters.CheckFileExists = true;
            if (openFilePicters.ShowDialog() == DialogResult.OK)
            {
                ShowPicters(openFilePicters.FileNames);
            }
        }

        public void ShowPicters(string[] Picters)
        {
            if (Picters != null)
            {
                int shift = 0;
                if (pic == null)
                {
                    pic = new PicterLeaf[Picters.Length];
                    shift = 0;
                }
                else
                {
                    shift = pic.Length;
                    PicterLeaf[] NewPic = new PicterLeaf[pic.Length + Picters.Length];
                    pic.CopyTo(NewPic, 0);
                    pic = NewPic;
                }
                int i = 0;
                int ii = shift;
                while (i < Picters.Length)
                {
                    try
                    {
                        pic[ii] = new PicterLeaf(ii, Picters[i]);
                        panelpicters.Controls.Add(pic[ii]);
                        pic[ii].Load(Picters[i]);
                        pic[ii].Click += new EventHandler(PictersLibrary_Click);
                        if (panelpicters.Dock == DockStyle.Bottom)
                        {
                            pic[ii].Height = panelpicters.Height - 10;
                            pic[ii].Width = pic[ii].Height * pic[ii].Image.Width / pic[ii].Image.Height;//pic[ii].Height * 20 / 29;
                            if (ii == 0) pic[ii].Left = 1;
                            else pic[ii].Left = pic[ii - 1].Left + pic[ii - 1].Width + 2;
                            pic[ii].Top = 2;
                        }
                        else
                        {
                            pic[ii].Width = panelpicters.Width - 10;
                            pic[ii].Height = pic[ii].Width * pic[ii].Image.Height / pic[ii].Image.Width;  // pic[ii].Width * 29 / 20;
                            if (ii == 0) pic[ii].Top = 2;
                            else pic[ii].Top = pic[ii - 1].Top + pic[ii - 1].Height + 2;
                            pic[ii].Left = 2;
                        }
                        pic[ii].SizeMode = PictureBoxSizeMode.Zoom;
                        ii++;
                    }
                    catch
                    {
                        MessageBox.Show("Невозможно загрузить файл " + Picters[i]);
                        panelpicters.Controls.Remove(pic[ii]);
                        if (pic.Length > shift + 1)
                        {
                            pic[ii] = null;
                            PicterLeaf[] tmPic = new PicterLeaf[pic.Length - 1];
                            for (int l = 0; l < ii; l++) tmPic[l] = pic[l];
                            pic = tmPic;
                        }
                        else
                        {
                            pic = null;
                            break;
                        }
                    }
                    i++;
                }
            }
        }

        DirectoryInfo ScanDir = null;
        private DateTime ScanStart = DateTime.MinValue;
        public DateTime TimeStart
        {
            get { return ScanStart; }
            set
            {
                if (ScanDir != null)
                {
                    ScanStart = DateTime.Now;
                    timer1.Interval = 1000;
                    timer1.Enabled = true;
                    timer1.Start();
                }
                else
                {
                    timer1.Stop();
                    timer1.Enabled = false;
                }
            }
        }

        public void setScannerDirectory(string scanpath)
        {
            if (scanpath != null)
            {
                ScanDir = new DirectoryInfo(scanpath);
                timer1.Tick += new EventHandler(timer1_Tick);
                TimeStart = DateTime.Now;
            }
        }

        private int PicterNumber = 0;

        void PictersLibrary_Click(object sender, EventArgs e)
        {
            PISel((PicterLeaf)sender);
        }

        void timer1_Tick(object sender, EventArgs e)
        {
            if (ScanDir != null)
                try
                {
                    FileInfo[] FilesPic = ScanDir.GetFiles("*.JPG");
                    string[] SelectedFiles = null;
                    foreach (FileInfo PicFile in FilesPic)
                        if (PicFile.LastAccessTime > ScanStart)
                        {
                            if (SelectedFiles == null)
                            {
                                SelectedFiles = new string[1];
                                SelectedFiles[0] = PicFile.FullName;
                            }
                            else
                            {
                                string[] tmpSFiles = new string[SelectedFiles.Length + 1];
                                SelectedFiles.CopyTo(tmpSFiles, 0);
                                tmpSFiles[SelectedFiles.Length] = PicFile.FullName;
                                SelectedFiles = tmpSFiles;
                            }
                        }
                    if (SelectedFiles != null) ShowPicters(SelectedFiles);
                    ScanStart = DateTime.Now;
                }
                catch
                {
                }
        }

        private void PISel(PicterLeaf pic)
        {
            pbPicter.Image = pic.Image;
            PicterNumber = pic.PicIndex;
            PicterResize();
        }

        private void edtsRemovelist_Click(object sender, EventArgs e)
        {
            try
            {
                if (pic != null)
                    if (pic.Length > 0)
                    {
                        PicterLeaf[] NewPic = new PicterLeaf[pic.Length - 1];
                        int l = 0;
                        for (int i = 0; i < pic.Length; i++)
                        {
                            if (i != PicterNumber)
                            {
                                NewPic[l] = pic[i];
                                l++;
                            }
                        }
                        for (int i =pic.Length-1; i > PicterNumber ; i--)
                            pic[i].Left = pic[i-1].Left;
                        panelpicters.Controls.Remove(pic[PicterNumber]);
                        pic = NewPic;
                        for (int i = 0; i < pic.Length; i++)
                            pic[i].PicIndex = panelpicters.Controls.IndexOf(pic[i]);
                        pbPicter.Image = null;
                    }
            }
            catch
            {
            }
        }

        private bool Set_cls = false;
        public bool SetClose
        {
            get { return Set_cls; }
            set
            {
                Set_cls = value;
                if (Set_cls)
                    try
                    {
                        edtsClose.Visible = true;
                        edtsClose.Click += new EventHandler(tsClose_Click);
                    }
                    catch { }
                else
                    try
                    {
                        edtsClose.Visible = false;
                        edtsClose.Click -= new EventHandler(tsClose_Click);
                    }
                    catch { }
            }
        }

        void tsClose_Click(object sender, EventArgs e)
        {
            Close();
            ScannerClose();
        }

        private void edtsKill_Click(object sender, EventArgs e)
        {
            ClearPicters();
        }

        public int RollDown
        {
            get { return (int) panelpicters.Dock; }
            set 
            { 
                panelpicters.Dock = (DockStyle) value;
                PLSettings.RollDown = (int)panelpicters.Dock;
                PLSettings.Save();
                if (panelpicters.Dock == DockStyle.Right)
                {
                    panelpicters.Width = 100;
                    PictureBox lpb = null;
                    foreach (PictureBox pic in panelpicters.Controls)
                    {
                        if (lpb == null) pic.Top = 2;
                        else pic.Top = lpb.Top + lpb.Height + 2;
                        pic.Width = panelpicters.Width - 10;
                        pic.Height = pic.Width * pic.Image.Height / pic.Image.Width;//29 / 20;
                        pic.Left = 2;
                        lpb = pic;
                    }
                }
                else
                {
                    panelpicters.Dock = DockStyle.Bottom;
                    panelpicters.Height = 100;
                    PictureBox lpb = null;
                    foreach (PictureBox pic in panelpicters.Controls)
                    {
                        if (lpb == null) pic.Left = 1;
                        else pic.Left = lpb.Left + lpb.Width + 2;
                        pic.Height = panelpicters.Height - 10;
                        pic.Width = pic.Height * pic.Image.Width / pic.Image.Height;//20 / 29;
                        pic.Top = 2;
                        lpb = pic;
                    }
                }
            }
        }

        private void edtsPicRollSit_Click(object sender, EventArgs e)
        {
            if (panelpicters.Dock == DockStyle.Bottom)
                RollDown =(int) DockStyle.Right;
            else
                RollDown = (int) DockStyle.Bottom;

            SaveSetting();
        }

        private void tsWidth_Click(object sender, EventArgs e)
        {
            SizeWidth = true;
        }

        private void tsHeight_Click(object sender, EventArgs e)
        {
            SizeHeight = true;
        }
    }
}
