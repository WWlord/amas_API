using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AMASControlRegisters;
using System.Windows.Forms;

namespace AMASControlRegisters
{
    public class CreateDocument
    {
        AMAS_DBI.Class_syb_acc AMAS_access;
        public TabControl TabDocuments;

        public CreateDocument(AMAS_DBI.Class_syb_acc ACC)
        {
            AMAS_access = ACC;
        }

        AMASControlRegisters.Document_Viewer document_New;

        public AMASControlRegisters.Document_Viewer FillDocuments(string FileName,  int kind, int tema, string name)
        {
            if (document_New == null)
            {
                document_New = new AMASControlRegisters.Document_Viewer(AMAS_access, null);

                // 
                // Новый документ
                // 
                this.document_New.Dock = System.Windows.Forms.DockStyle.Fill;
                this.document_New.Location = new System.Drawing.Point(0, 0);
                this.document_New.Name = "documentXML_Show";
                this.document_New.Sender = 0;
                this.document_New.TabIndex = 14;
            }

            document_New.New_document = true;
            document_New.Doc_ID = 0;
            document_New.SelectedFile_Append(FileName);

            int document = AMAS_DBI.AMASCommand.Append_Indoor_document(kind, tema, document_New.Annotation, 0);
            if (document > 0)
            {
                document_New.SaveDocument(document);
                document_New.New_document = false;
                document_New.Doc_ID = document;
                document_New.Refresh();
            }
            return document_New;
        }
    }
}
