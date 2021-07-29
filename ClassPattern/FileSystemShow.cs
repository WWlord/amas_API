using System;
using System.Collections.Generic;
using System.Text;
using AMAS_DBI;
using System.Collections;
using System.Drawing;
using CommonValues;

namespace ClassPattern
{
    public class FIleSystemShow
    {
        public enum ExecModule {Resourse=1, Master}

        private ExecModule ExeMod = ExecModule.Master;
        private AMAS_DBI.Class_syb_acc AMASacc;
        private System.Windows.Forms.TreeView KTExplorer;
        private System.Windows.Forms.WebBrowser www;
        System.Windows.Forms.TreeNode KTNod = null;
        FileDirExplorer PickFile;
        private System.Windows.Forms.ContextMenuStrip Assign;
        private System.Windows.Forms.ToolStripMenuItem íàçíà÷èòüToolStripMenuItem;

        public FIleSystemShow(System.Windows.Forms.TreeView FE, System.Windows.Forms.WebBrowser web, System.Windows.Forms.TreeView KT, Class_syb_acc Acc, ExecModule EM)
        {
            ExeMod = EM;
            AMASacc = Acc;
            PickFile = new FileDirExplorer(FE, "*.doc");
            KTExplorer = KT;
            www = web;
            K_list();
            KTExplorer.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(KTExplorer_AfterSelect);

            this.Assign = new System.Windows.Forms.ContextMenuStrip();
            KTExplorer.ContextMenuStrip = this.Assign;
            this.íàçíà÷èòüToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.Assign.SuspendLayout();
            // 
            // Assign
            // 
            this.Assign.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
                this.íàçíà÷èòüToolStripMenuItem});
            this.Assign.Name = "Assign";
            this.Assign.Size = new System.Drawing.Size(153, 48);
            this.Assign.Text = "Íàçíà÷èòü";
            // 
            // íàçíà÷èòüToolStripMenuItem
            // 
            this.íàçíà÷èòüToolStripMenuItem.Name = "íàçíà÷èòüToolStripMenuItem";
            this.íàçíà÷èòüToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.íàçíà÷èòüToolStripMenuItem.Text = "Íàçíà÷èòü";
            this.íàçíà÷èòüToolStripMenuItem.Click += new System.EventHandler(this.íàçíà÷èòüToolStripMenuItem_Click);
            this.Assign.ResumeLayout(false);
            KTExplorer.Refresh();
            PickFile.FilePicked += new FileDirExplorer.FilePathHandler(PickFile_FilePicked);
        }

        private void PickFile_FilePicked(string fileloc)
        {
            www.Stop();
            www.Navigate(fileloc);
        }

        private void íàçíà÷èòüToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PickFile.IS_File)
                if (KTNod != null)
                {
                    int tema = 0;
                    int kind = 0;
                    if (KTNod.Name.Substring(0, 1).CompareTo("t") == 0)
                    {
                        tema = (int)Convert.ToInt32(KTNod.Name.Substring(1));
                        kind = (int)Convert.ToInt32(KTNod.Parent.Name.Substring(1));
                    }
                    else kind = (int)Convert.ToInt32(KTNod.Name.Substring(1));
                    if (ExeMod == ExecModule.Resourse)
                    {
                        if (AMASCommand.AddtoDOTLibrary(kind, tema, PickFile.FileImage))
                            KTNod.ForeColor = Color.Violet;
                    }
                    else
                    {
                        if (AMASCommand.AddtoMYDOTLibrary(kind, tema, PickFile.FileImage))
                            KTNod.ForeColor = Color.Violet;
                    }
                }
        }

        ArrayList KT_ROWS;

        private void K_list()
        {
            int r = (int)CommonValues.CommonClass.Lists.Kind;
            KTExplorer.Nodes.Clear();
            try
            {
                if (ExeMod == ExecModule.Resourse)
                    KT_ROWS = AMAS_DBI.AMASCommand.A_resolutions_list(r);
                else KT_ROWS = AMAS_DBI.AMASCommand.MyKinds_list();

                foreach (CommonClass.Arraysheet t in KT_ROWS)
                    KTExplorer.Nodes.Add("k" + t.Id, t.Name);
            }
            catch (Exception e)
            {
                AMASacc.EBBLP.AddError(e.Message, "FSS - 3", e.StackTrace);
            }
        }

        private void T_list(System.Windows.Forms.TreeNode Nod)
        {
            System.Windows.Forms.TreeNode nnn;
            if (Nod != null)
                if (Nod.Name.Substring(0, 1).CompareTo("k") == 0)
                    if (Nod.Nodes.Count == 0)
                    {
                        Nod.Nodes.Clear();
                        try
                        {
                            if (ExeMod == ExecModule.Resourse)
                                KT_ROWS = AMAS_DBI.AMASCommand.SubKindTema_list((int)Convert.ToInt32(Nod.Name.Substring(1)));
                            else
                                KT_ROWS = AMAS_DBI.AMASCommand.MySubKindTema_list((int)Convert.ToInt32(Nod.Name.Substring(1)));

                            foreach (CommonClass.ArrayThree t in KT_ROWS)
                            {
                                nnn = Nod.Nodes.Add("t" + t.Id, t.Name);
                                if (t.FId> 0)
                                    nnn.ForeColor = Color.Violet;
                                else
                                    nnn.ForeColor = Color.DarkCyan;
                            }
                        }
                        catch (Exception e)
                        {
                            AMASacc.EBBLP.AddError(e.Message, "FSS - 4", e.StackTrace);
                        }
                    }
        }

        private void KTExplorer_AfterSelect(Object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (KTNod != e.Node)
            {
                KTNod = e.Node;
                T_list(KTNod);
            }
        }

    }

}
