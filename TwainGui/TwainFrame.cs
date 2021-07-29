using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using TwainGui.baseLayer;
using TwainLib;
using ClassPattern;
using GdiPlusLib;

namespace TwainGui
{
public class TwainFrame : System.Windows.Forms.Form, IMessageFilter
	{
	private System.Windows.Forms.MdiClient mdiClient1;
	private System.Windows.Forms.MenuItem menuMainFile;
	private System.Windows.Forms.MenuItem menuItemScan;
	private System.Windows.Forms.MenuItem menuItemSelSrc;
	private System.Windows.Forms.MenuItem menuMainWindow;
	private System.Windows.Forms.MenuItem menuItemExit;
	private System.Windows.Forms.MenuItem menuItemSepr;
    private System.Windows.Forms.MainMenu mainFrameMenu;
    private ClassPattern.PictersLibrary LibPicters;
    private IContainer components;

    private TwainGuiSettings frmSettings1 = new TwainGuiSettings();

	public TwainFrame()
		{
		InitializeComponent();
		tw = new Twain();
		tw.Init( this.Handle );
        this.Load += new EventHandler(TwainFrame_Load);
        this.Disposed += new EventHandler(TwainFrame_Disposed);
        LibPicters.Scanned += new ClassPattern.PictersLibrary.ScanDoc(LibPicters_Scanned);
        LibPicters.SetClose = true;
        LibPicters.ScannerClose += new PictersLibrary.Closing(LibPicters_ScannerClose);
		}

    void LibPicters_ScannerClose()
    {
        //Close();
        this.Visible = false;
    }

    void LibPicters_Scanned(string Filename)
    {
        try
        {
            Scanned(Filename);
        }
        catch
        {
        }
    }

    public void SetScanDir(   string ScanDir    )
    {
         LibPicters.setScannerDirectory(ScanDir);
    }


    void TwainFrame_Disposed(object sender, EventArgs e)
    {
        frmSettings1.Save();
    }

    void TwainFrame_Load(object sender, EventArgs e)
    {
        frmSettings1.SettingsKey = "ScanForm";
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

	protected override void Dispose( bool disposing )
		{
		if( disposing )
			{
			tw.Finish();
			if (components != null) 
				{
				components.Dispose();
				}
			}
		base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.components = new System.ComponentModel.Container();
            this.menuMainFile = new System.Windows.Forms.MenuItem();
            this.menuItemSelSrc = new System.Windows.Forms.MenuItem();
            this.menuItemScan = new System.Windows.Forms.MenuItem();
            this.menuItemSepr = new System.Windows.Forms.MenuItem();
            this.menuItemExit = new System.Windows.Forms.MenuItem();
            this.mainFrameMenu = new System.Windows.Forms.MainMenu(this.components);
            this.menuMainWindow = new System.Windows.Forms.MenuItem();
            this.mdiClient1 = new System.Windows.Forms.MdiClient();
            this.LibPicters = new ClassPattern.PictersLibrary();
            this.SuspendLayout();
            // 
            // menuMainFile
            // 
            this.menuMainFile.Index = 0;
            this.menuMainFile.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuItemSelSrc,
            this.menuItemScan,
            this.menuItemSepr,
            this.menuItemExit});
            this.menuMainFile.MergeType = System.Windows.Forms.MenuMerge.MergeItems;
            this.menuMainFile.Text = "&Действия";
            // 
            // menuItemSelSrc
            // 
            this.menuItemSelSrc.Index = 0;
            this.menuItemSelSrc.MergeOrder = 11;
            this.menuItemSelSrc.Text = "&Выбрать сканер...";
            this.menuItemSelSrc.Click += new System.EventHandler(this.menuItemSelSrc_Click);
            // 
            // menuItemScan
            // 
            this.menuItemScan.Index = 1;
            this.menuItemScan.MergeOrder = 12;
            this.menuItemScan.Text = "&Сканировать...";
            this.menuItemScan.Click += new System.EventHandler(this.menuItemScan_Click);
            // 
            // menuItemSepr
            // 
            this.menuItemSepr.Index = 2;
            this.menuItemSepr.MergeOrder = 19;
            this.menuItemSepr.Text = "-";
            // 
            // menuItemExit
            // 
            this.menuItemExit.Index = 3;
            this.menuItemExit.MergeOrder = 21;
            this.menuItemExit.Text = "&Завершить";
            this.menuItemExit.Click += new System.EventHandler(this.menuItemExit_Click);
            // 
            // mainFrameMenu
            // 
            this.mainFrameMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
            this.menuMainFile,
            this.menuMainWindow});
            // 
            // menuMainWindow
            // 
            this.menuMainWindow.Index = 1;
            this.menuMainWindow.MdiList = true;
            this.menuMainWindow.Text = "&Листы";
            // 
            // mdiClient1
            // 
            this.mdiClient1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mdiClient1.Location = new System.Drawing.Point(0, 0);
            this.mdiClient1.Name = "mdiClient1";
            this.mdiClient1.Size = new System.Drawing.Size(600, 119);
            this.mdiClient1.TabIndex = 0;
            // 
            // LibPicters
            // 
            this.LibPicters.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.LibPicters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.LibPicters.Editable = true;
            this.LibPicters.Location = new System.Drawing.Point(0, 0);
            this.LibPicters.Name = "LibPicters";
            this.LibPicters.RollDown = 2;
            this.LibPicters.Saved = false;
            this.LibPicters.SetClose = false;
            this.LibPicters.Size = new System.Drawing.Size(600, 119);
            this.LibPicters.SizeHeight = false;
            this.LibPicters.SizeWidth = false;
            this.LibPicters.TabIndex = 1;
            this.LibPicters.TimeStart = new System.DateTime(((long)(0)));
            this.LibPicters.Zoom = 100;
            this.LibPicters.Load += new System.EventHandler(this.LibPicters_Load);
            // 
            // TwainFrame
            // 
            this.AutoScaleBaseSize = new System.Drawing.Size(5, 13);
            this.ClientSize = new System.Drawing.Size(600, 119);
            this.Controls.Add(this.LibPicters);
            this.Controls.Add(this.mdiClient1);
            this.IsMdiContainer = true;
            this.Menu = this.mainFrameMenu;
            this.Name = "TwainFrame";
            this.Text = "Сканирование документа";
            this.ResumeLayout(false);

		}
		#endregion



	private void menuItemExit_Click(object sender, System.EventArgs e)
    {
        LibPicters.Close();
        Close();
		}

	private void menuItemScan_Click(object sender, System.EventArgs e)
		{
		if( ! msgfilter )
			{
			this.Enabled = false;
			msgfilter = true;
			Application.AddMessageFilter( this );
			}
		tw.Acquire();
		}

	private void menuItemSelSrc_Click(object sender, System.EventArgs e)
		{
		tw.Select();
		}

    private void SaveScannedLeafs()
    {
        string[] Picters = new string[this.MdiChildren.Length];
        for (int i=0; i<this.MdiChildren.Length;i++)
        {
            PicForm Fm=(PicForm)this.MdiChildren[i];
            Picters[i]=Gdip.SaveDIBAsJpeg(Fm.Text, Fm.bmpptr, Fm.pixptr);
            Fm.Close();
            Fm.Dispose();
        }
        LibPicters.ShowPicters(Picters);
    }


	bool IMessageFilter.PreFilterMessage( ref Message m )
		{
		TwainCommand cmd = tw.PassMessage( ref m );
		if( cmd == TwainCommand.Not )
			return false;

        switch (cmd)
        {
            case TwainCommand.CloseRequest:
            case TwainCommand.CloseOk:
                EndingScan();
                tw.CloseSrc();
                SaveScannedLeafs();
                break;
            case TwainCommand.DeviceEvent:
                break;
            case TwainCommand.TransferReady:
                ArrayList pics = tw.TransferPictures();
                //EndingScan();
                //tw.CloseSrc();
                for (int i = 0; i < pics.Count; i++)
                {
                    IntPtr img = (IntPtr)pics[i];
                    PicForm newpic = new PicForm(img);
                    newpic.MdiParent = this;
                    picnumber++;
                    newpic.Text = "List" + picnumber.ToString();
                    newpic.Show();
                }
                break;
        }

		return true;
		}

    public delegate void ScanDoc(string Filename);
    public event ScanDoc Scanned;

    void newpic_ScanLeafed(ArrayList LeafList)
    {
       
    }

	private void EndingScan()
		{
		if( msgfilter )
			{
			Application.RemoveMessageFilter( this );
			msgfilter = false;
			this.Enabled = true;
			this.Activate();
			}
		}

    [STAThread]
    static void Main()
    {
        if (Twain.ScreenBitDepth < 15)
        {
            MessageBox.Show("Need high/true-color video mode!", "Screen Bit Depth", MessageBoxButtons.OK, MessageBoxIcon.Information);
            return;
        }

        TwainFrame mf = new TwainFrame();
        Application.Run(mf);
    }

	private bool	msgfilter;
	private Twain	tw;
	private int		picnumber = 0;

    private void LibPicters_Load(object sender, EventArgs e)
    {
    }

    private void menuItem1_Click(object sender, EventArgs e)
    {

    }

} // class TwainFrame

} // namespace TwainGui
