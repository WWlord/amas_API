using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using AMAS_DBI;
using AMAS_Query;

namespace ClassDocuments.DocsTree
{
    public class ClassDocsTree
    {
        private int MasterDocumentId;
        private System.Windows.Forms.TreeView DoscTreeView;
        private ArrayList DocsAL;
        private AMAS_DBI.Class_syb_acc AMASacc;

        private class Document
        {
            public int Ident;
            public Document Master;
            public Document Parent;
            public TreeNode Node;

            public Document(int DocId,  Document ParentDoc, TreeNode N)
            {
                Parent = ParentDoc;
                if (Parent == null)
                    Master = this;
                else
                    Master = Parent.Master;
                Ident = DocId;
                Node = N;
            }
        }

        public ClassDocsTree( System.Windows.Forms.TreeView TV, AMAS_DBI.Class_syb_acc ACC)
        {
            DoscTreeView = TV;
            AMASacc = ACC;
            DocsAL = new ArrayList();
            DoscTreeView.AfterSelect+=new TreeViewEventHandler(DoscTreeView_AfterSelect);
        }

        public void Refresh(int DocId, string DocNum)
        {
            bool b = true;
            foreach (Document doc in DocsAL)
            {
                if (doc.Ident == DocId)
                {
                    b = false;
                    doc.Node.TreeView.SelectedNode = doc.Node;
                    break;
                }
            }
            if (b)
            {
                MasterDocumentId = AMAS_DBI.AMASCommand.GetMasterDocument(DocId);
                if (AMASacc.Set_table("", "select find_cod from dbo.RKK_flow_document where kod=" + MasterDocumentId.ToString(), null))
                {
                    DoscTreeView.Nodes.Clear();
                    DocsAL.Clear();
                    AddDocument(MasterDocumentId,(string) AMASacc.Find_Field("find_cod"), null);
                    AMASacc.ReturnTable();
                }
            }
        }

        private TreeNode AddDocument(int DocId, string DocNum, TreeNode SelectedNode)
        {
            bool b = true;
            TreeNode Nod = null;

                foreach (Document doc in DocsAL)
                {
                    if (doc.Ident == DocId)
                    {
                        b = false;
                        break;
                    }
                }
                if (b)
                    try
                    {
                        if (SelectedNode == null)
                            Nod = DoscTreeView.Nodes.Add("D" + DocId.ToString().Trim(), DocNum);
                        else
                            Nod = SelectedNode.Nodes.Add("D" + DocId.ToString().Trim(), DocNum);

                        Document ddd = null;
                        foreach (Document doc in DocsAL)
                        {
                            if (SelectedNode.Name.CompareTo(doc.Node.Name) == 0)
                            {
                                ddd = doc;
                                break;
                            }
                        }
                        Document d = new Document(DocId, ddd, Nod);
                                DocsAL.Add(d);
                                SubDocsList(DocId, Nod);
                    }
                    catch (Exception ex) { Console.WriteLine(ex.Message); }
            return Nod;
        }

        public delegate void PickedDocument(int DocId);
        public event PickedDocument DocPicked;

        private void DoscTreeView_AfterSelect(Object sender, TreeViewEventArgs e)
        {
            int DocID = Convert.ToInt32(e.Node.Name.Substring(1));
            DocPicked(DocID);
        }

        private bool SubDocsList(int DocID, TreeNode SelectedNode)
        {
            bool b = true;
            try
            {
                try
                {
                    if (AMASacc.Set_table("TCDTree1", AMAS_Query.Class_AMAS_Query.SubDocuments(DocID), null))
                    {
                        for (int i = 0; i < AMASacc.Rows_count; i++)
                        {
                            AMASacc.Get_row(i);
                            AddDocument((int)AMASacc.Find_Field("kod"), (string)AMASacc.Find_Field("find_cod"), SelectedNode);
                        }
                        AMASacc.ReturnTable();
                    }
                }
                catch { b = false; }
            }
            catch { b = false; }
            return b;
        }
    }
}
