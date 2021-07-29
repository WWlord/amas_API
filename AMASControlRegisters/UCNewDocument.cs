using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;
using ClassPattern;

namespace AMASControlRegisters
{
    public partial class UCNewDocument : UserControl
    {

        private int a_width;
        private int a_heiht;

        //private System.Windows.Forms.GroupBox gbCommom; 
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbTemy;
        private System.Windows.Forms.ComboBox cbKinds;

        private Address_ids KindBox;
        private Address_ids TemaBox;

        private AMAS_DBI.Class_syb_acc SYB_acc;
        private AMASControlRegisters.Document_Viewer document_New;
        private int parentDoc;
        bool eriseDoc = false;
        public int DocId=0;

        public UCNewDocument(Class_syb_acc Acc, Document_Viewer docum,int parentId, bool killDoc)
        {
            InitializeComponent();

            document_New = docum;
            SYB_acc = Acc;
            parentDoc = parentId;
            eriseDoc = killDoc;
            a_width=this.Width;
            a_heiht = this.Height;

            // 
            // label2
            // 
            this.label2 = new Label();
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 60);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "Тема";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label1
            // 
            this.label1 = new Label();
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(5, 27);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 17);
            this.label1.TabIndex = 6;
            this.label1.Text = "Документ";
            this.label1.TextAlign = System.Drawing.ContentAlignment.TopRight;

            // 
            // cbTemy
            // 
            this.cbTemy = new ComboBox();
            this.cbTemy.FormattingEnabled = true;
            this.cbTemy.Location = new System.Drawing.Point(96, 57);
            this.cbTemy.Margin = new System.Windows.Forms.Padding(4);
            this.cbTemy.Name = "cbTemy";
            this.cbTemy.Size = new System.Drawing.Size(213, 24);
            this.cbTemy.TabIndex = 5;
            // 
            // cbKinds
            // 
            this.cbKinds = new ComboBox();
            this.cbKinds.FormattingEnabled = true;
            this.cbKinds.Location = new System.Drawing.Point(96, 23);
            this.cbKinds.Margin = new System.Windows.Forms.Padding(4);
            this.cbKinds.Name = "cbKinds";
            this.cbKinds.Size = new System.Drawing.Size(213, 24);
            this.cbKinds.TabIndex = 4;

           this.panelKIndTema.Controls.Add(this.label2);
            this.panelKIndTema.Controls.Add(this.label1);
            this.panelKIndTema.Controls.Add(this.cbTemy);
            this.panelKIndTema.Controls.Add(this.cbKinds);

            KindBox = new Address_ids(cbKinds);
            TemaBox = new Address_ids(cbTemy);
            KindBox.connect(SYB_acc);
            TemaBox.connect(SYB_acc);
            KindBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_kinds(), "kind", "kod");
            TemaBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_temy(KindBox.get_ident()), "description_", "tema");
            cbKinds.SelectedIndexChanged += new EventHandler(cbKinds_SelectedIndexChanged);
            this.Resize += new EventHandler(UCNewDocument_Resize);
        }

        void cbKinds_SelectedIndexChanged(object sender, EventArgs e)
        {
            TemaBox.Select_Subject(AMAS_Query.Class_AMAS_Query.Wflow_temy(KindBox.get_ident()), "description_", "tema");
        }

        void UCNewDocument_Resize(object sender, EventArgs e)
        {
            if (this.Width < a_width) this.Width = a_width;
            if (this.Height < a_heiht) this.Height = a_heiht;
        }

        public void answer(string cod)
        {
            tbxmetadata.Text = cod;
        }

        private void btnPattern_Click(object sender, EventArgs e)
        {
            DocumentProcessing DoPr = new DocumentProcessing(SYB_acc, document_New);
            DoPr.AddDot(KindBox.get_ident(), TemaBox.get_ident());
        }

        private void buaatonAddFile_Click(object sender, EventArgs e)
        {
            document_New.File_Append();
        }

        private void btnRemoveFile_Click(object sender, EventArgs e)
        {
            document_New.File_Delete();
        }

        private void btnEditor_Click(object sender, EventArgs e)
        {
            document_New.Editor_append();
        }

        private void btnGoBack_Click(object sender, EventArgs e)
        {            
            this.SendToBack();
            this.Visible = false;
            if (eriseDoc)
            {
                document_New.Parent.Controls.Remove(document_New);
                document_New.Visible = false;
            }
            else if (DocId > 0)
            {
                document_New.Edit_document = false;
                document_New.New_document = false;
                document_New.Doc_ID = DocId;
            }
            this.Parent.Controls.Remove(this);
        }

        private void btnSaveDocument_Click(object sender, EventArgs e)
        {
           if (document_New != null)
            {
                int document = AMAS_DBI.AMASCommand.Append_Indoor_document(KindBox.get_ident(), TemaBox.get_ident(), document_New.Annotation, parentDoc);
                if (document > 0)
                {
                    document_New.SaveDocument(document);
                    AMASCommand.AnswerDocument(document, document_New.Sender);
                }
                document_New.New_document = true;
                document_New.Doc_ID = 0;
            }

        }
    }
}
