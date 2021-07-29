using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using ClassStructure;

namespace Chief
{
    public partial class FormSeekEmDep : Form
    {
        Structure InStrucrure = null;
        Groups InGroups = null;
        TreeNode LastNode = null;

        public FormSeekEmDep(Structure InStr)
        {
            InitializeComponent();

            InStrucrure = InStr;
            InGroups = InStrucrure.UsedGroups;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FNDDepEmpl(false);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FNDDepEmpl(true);
        }

        private void FNDDepEmpl(bool TF)
        {
            if (checkedDepEmp.SelectedItem!=null)
            if (InStrucrure != null)
                if (checkedDepEmp.SelectedItem.Equals( checkedDepEmp.Items[0]))
                {
                    TreeNode nod = InStrucrure.Find_Department(textBoxED.Text.Trim(), LastNode, TF);
                    if (nod != null)
                    {
                        nod.TreeView.BringToFront();
                        nod.EnsureVisible();                           
                        nod.TreeView.SelectedNode = nod;
                        LastNode = nod;
                    }
                }
                else
                {
                    if (LastNode != null)
                    {
                        if (LastNode.TreeView.Name.CompareTo(InStrucrure.TreeV.Name) == 0)
                        {
                            TreeNode nod = InStrucrure.Find_Employee(textBoxED.Text.Trim(), LastNode, TF);
                            if (nod != null)
                            {
                                nod.TreeView.BringToFront();
                                nod.EnsureVisible();
                                nod.TreeView.SelectedNode = nod;
                                LastNode = nod;
                            }
                            else if (InGroups != null)
                            {
                                nod = InGroups.Find_Employee(textBoxED.Text.Trim(), null, TF);
                                if (nod != null)
                                {
                                    nod.TreeView.BringToFront();
                                    nod.EnsureVisible();
                                    nod.TreeView.SelectedNode = nod;
                                    LastNode = nod;
                                }
                            }
                        }
                        else if (InGroups != null)
                            if (LastNode.TreeView.Name.CompareTo(InGroups.GroupsTree.Name) == 0)
                            {
                                TreeNode nod = InGroups.Find_Employee(textBoxED.Text.Trim(), null, TF);
                                if (nod != null)
                                {
                                    nod.TreeView.BringToFront();
                                    nod.EnsureVisible();
                                    nod.TreeView.SelectedNode = nod;
                                    LastNode = nod;
                                }
                            }
                    }
                    else
                    {
                        TreeNode nod = InStrucrure.Find_Employee(textBoxED.Text.Trim(), LastNode, TF);
                        if (nod != null)
                        {
                            nod.TreeView.BringToFront();
                            nod.EnsureVisible();
                            nod.TreeView.SelectedNode = nod;
                            LastNode = nod;
                        }
                        else if (InGroups != null)
                        {
                             nod = InGroups.Find_Employee(textBoxED.Text.Trim(), LastNode, TF);
                            if (nod != null)
                            {
                                nod.TreeView.BringToFront();
                                nod.EnsureVisible();
                                nod.TreeView.SelectedNode = nod;
                                LastNode = nod;
                            }
                        }
                    }
                }
            else if (InGroups != null && checkedDepEmp.SelectedItem.Equals(checkedDepEmp.Items[0]))
            {
                TreeNode nod = InGroups.Find_Employee(textBoxED.Text.Trim(), LastNode, TF);
                if (nod != null)
                {
                    nod.TreeView.BringToFront();
                    nod.EnsureVisible();
                    nod.TreeView.SelectedNode = nod;
                    LastNode = nod;
                }
            }
        }
    }
}