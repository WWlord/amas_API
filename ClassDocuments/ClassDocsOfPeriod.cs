using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using ClassInterfases;
using AMAS_DBI;
using AMAS_Query;
using AMASDocuments;
using System.Drawing;
using CommonValues;

namespace DocumentsByPeriod 
{
    public class DocsOfPeriod 
    {
        public class Body
        {
            public System.Windows.Forms.Panel Paneling;
            public System.Windows.Forms.TreeView treeDocsView;
            private System.Windows.Forms.DateTimePicker From;
            private System.Windows.Forms.DateTimePicker To;
            private System.Windows.Forms.Label CheckFrom;
            private System.Windows.Forms.Label CheckTo;
            private System.Windows.Forms.Button Executing;
            private AMAS_Query.DOCUM Seek_Docs;
            private System.Windows.Forms.Button Cansel;
            private int box_checus;
            private System.Windows.Forms.ToolStripProgressBar ProgBar;

            private CheckBox[] CheckBoxes;
            //private System.Windows.Forms.ImageList imagelib;
            private Class_syb_acc SYB_acc;
            private AMAS_Query.TreeGrow TreG;

            public Body( System.Windows.Forms.ImageList IMG, Class_syb_acc Acc, System.Windows.Forms.Panel Panel, AMAS_Query.DOCUM Seek_D, System.Windows.Forms.ToolStripProgressBar PrBr)
            {
                Seek_Docs = Seek_D;
                //imagelib = IMG;
                SYB_acc = Acc;
                TreG = new TreeGrow(20);
                Paneling = Panel;
                ProgBar = PrBr;
                Paneling.Controls.Clear();
                From = new DateTimePicker();
                Panel.Controls.Add(From);
                To = new DateTimePicker();
                Panel.Controls.Add(To);
                CheckFrom = new Label();
                CheckFrom.Text = "От";
                CheckFrom.TextAlign = System.Drawing.ContentAlignment.TopLeft;
                Panel.Controls.Add(CheckFrom);
                CheckTo = new Label();
                CheckTo.Text = "До";
                CheckTo.TextAlign = System.Drawing.ContentAlignment.TopLeft;
                Panel.Controls.Add(CheckTo);
                CheckBoxes = new CheckBox[Seek_Docs.count_seek];
                for (int i = 0; i < Seek_Docs.count_seek; i++)
                {
                    CheckBoxes[i] = new CheckBox();
                    CheckBoxes[i].Text = Seek_Docs.SEEK_doc[i].desc;
                    CheckBoxes[i].Click += new EventHandler(All_Docs_CheckedChanged);
                    Panel.Controls.Add(CheckBoxes[i]);
                }
                box_checus = 0;
                CheckBoxes[0].Checked = true;
                Executing = new Button();
                Executing.Text="Выполнить!";
                Executing.Click+=new EventHandler(Executing_Click);
                Panel.Controls.Add(Executing);
                treeDocsView = new TreeView();
                treeDocsView.Visible = false;
                treeDocsView.Dock = DockStyle.Fill;
                Panel.Controls.Add(treeDocsView);
                Panel.Resize += new EventHandler(Resize_Me);
                treeDocsView.ImageList = IMG;
                treeDocsView.SelectedImageIndex = AMASDocuments.ClassDocsItem.DocAsOpen;
                Cansel = new Button();
                treeDocsView.Controls.Add(Cansel);
                Cansel.Width = 20;
                Cansel.Height = 10;
                Cansel.BackColor = Color.Maroon;
                Cansel.Click += new EventHandler(Cansel_Click);
                Resize();
            }

            public delegate void PickedHandler(int DocId,TreeNode Nod);
            public event PickedHandler NodePicked;

            private void Select_document_NodePicked(int DocId,TreeNode Nod)
            {
                NodePicked(DocId, Nod);
            }

            private AMASDocuments.SelectDocuments.ClassSelectDocuments Select_document=null;

            public AMASDocuments.ClassDocsItem ItDocument { get { return Select_document.Selected_document; } }

            public void CurrentItemRefresh()
            {
                if (Select_document != null) Select_document.CurrentNodeRefresh();
            }

            private void Cansel_Click(object sender, EventArgs e)
            {
                foreach (Control c in Paneling.Controls)
                    c.Visible = true;
                treeDocsView.Visible = false;
                treeDocsView.Nodes.Clear();
                Resize();
            }

            private void Executing_Click(object sender, EventArgs e)
            {
                ExecSteps(null);
            }

            public void ExecSteps(CommonValues.FindProperty FndPr)
            {
                for (int i = 0; i < Seek_Docs.count_seek; i++)
                {
                    if (CheckBoxes[i].Checked) AMAS_Query.Class_AMAS_Query.FiltrIndex = Seek_Docs.SEEK_doc[i].val;
                }
                DateTime fd;
                DateTime ft;

                try { fd = From.Value.Date; }
                catch { fd = DateTime.MinValue.Date; }
                try { ft = To.Value.Date.AddDays(1); }
                catch { ft = System.DateTime.Today.Date.AddDays(1); }
                AMAS_Query.Class_AMAS_Query.DocsListofPeriod(fd, ft);
                Select_document = new AMASDocuments.SelectDocuments.ClassSelectDocuments(SYB_acc, treeDocsView, TreG, ProgBar, FndPr);
                foreach (Control ctl in Paneling.Controls)
                    ctl.Visible = false;
                treeDocsView.Visible = true;
                Resize();
                Select_document.NodePicked += new AMASDocuments.SelectDocuments.ClassSelectDocuments.PickedHandler(Select_document_NodePicked);
            }

            public AMASDocuments.ClassDocsItem selectedDoc 
            { 
                get 
                {
                    try
                    {
                        Select_document.Selected_document.Picked();
                        return Select_document.Selected_document;
                    }
                    catch { return null; }
                }
                //set
                //{
                //    Select_document.Selected_document
                //}
            }

            private void All_Docs_CheckedChanged(object sender, EventArgs e)
            {
                for (int i = 0; i < Seek_Docs.count_seek; i++)
                {
                    int c=0;
                    int last_box = box_checus;
                    if (CheckBoxes[i].Checked) 
                    {
                        c+=1;
                        if (i != box_checus)
                            { box_checus = i; }
                    }
                    else CheckBoxes[i].Checked = false;
                    if (last_box != box_checus) { CheckBoxes[last_box].Checked = false; }
                    else { CheckBoxes[last_box].Checked = true; }
                }
            }

            private void Resize_Me(object sender, EventArgs e)
            {
                Resize();
            }

            public void Resize ()
            {
                int iWidth = Paneling.Width;
                int iHeight = Paneling.Height;

                CheckFrom.Top = 10;
                CheckFrom.Left = 0;
                CheckFrom.Width = (iWidth - 10) / 5;
                CheckFrom.Height = (iHeight - 20) / 9;
                CheckTo.Top = CheckFrom.Top + CheckFrom.Height;
                CheckTo.Left = CheckFrom.Left;
                CheckTo.Width = CheckFrom.Width;
                CheckTo.Height = CheckFrom.Height;
                From.Top = CheckFrom.Top;
                From.Left = CheckFrom.Left + CheckFrom.Width;
                From.Width = iWidth - CheckFrom.Width - CheckFrom.Left-20;
                From.Height = CheckFrom.Height;
                To.Top = CheckTo.Top;
                To.Left = CheckTo.Left + CheckTo.Width ;
                To.Width = From.Width;
                To.Height = From.Height;
                Executing.Left = To.Left;
                Executing.Width = To.Width;
                Executing.Height = To.Height;
                Executing.Top = iHeight - Executing.Height - 10;
                for (int i = 0; i < Seek_Docs.count_seek; i++)
                {
                    CheckBoxes[i].Height = (Executing.Top - (To.Top + To.Height + 10)  - 10) / (Seek_Docs.count_seek + 1); ;
                    CheckBoxes[i].Top = To.Top + To.Height + 10 + CheckBoxes[i].Height * i;
                    CheckBoxes[i].Left = To.Left;
                    CheckBoxes[i].Width = To.Width;
                }
                Cansel.Top = 0;
                Cansel.Left = treeDocsView.Width - Cansel.Width ;
                if (treeDocsView.Scrollable) Cansel.Left -= 20;
            }
        }

        public Body DocsGroup;

        public DocsOfPeriod(AMAS_Query.DOCUM Seek_Docs,Panel panul, System.Windows.Forms.Form DG)
        {
            ClassInterfases.FormShowCon Theatre;
            Theatre = DG as ClassInterfases.FormShowCon;
            if (Theatre != null)
            {
                DocsGroup = new Body(Theatre.imagelib(), Theatre.DB_acc(), panul, Seek_Docs, Theatre.FuelBar());
                DocsGroup.NodePicked += new Body.PickedHandler(DocsGroup_NodePicked);
            }
            DocsGroup.Resize();
        }


        public delegate void PickedHandler(int DocId,TreeNode Nod);
        public event PickedHandler NodePicked;

        private void DocsGroup_NodePicked(int DocId,TreeNode Nod)
        {
            //DocsGroup.selectedDoc = DocId;
            NodePicked(DocId,Nod);
        }
    }
}
