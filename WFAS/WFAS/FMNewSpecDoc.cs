using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using AMASControlRegisters;
using AMAS_DBI;


namespace WFAS
{
    public partial class FMNewSpecDoc : Form
    {
        private AMAS_DBI.Class_syb_acc AMAS_access;    
        DataTable ResData;
            int id=0;
            DateTime data ;
            string name="";
            bool YN;
            AMASControlRegisters.CreateDocument NewDoc;

            string KindName = "Контракт";
            string TemaName = "техническое обслуживание";
        public FMNewSpecDoc(AMAS_DBI.Class_syb_acc Acc)
        {
            InitializeComponent();

            AMAS_access = Acc;
            NewDoc = new AMASControlRegisters.CreateDocument(AMAS_access);

            data = DateTime.Today;
            ResData = new DataTable("Results");
            ResData.Columns.Add("id", id.GetType());
            ResData.Columns.Add("data", data.GetType());
            ResData.Columns.Add("name", name.GetType());
            ResData.Columns.Add("YN", YN.GetType());
            ResData.Columns.Add("DocumentKind", getKindId().GetType());
            ResData.Columns.Add("DocumentTema", getTemaId().GetType());
            
            monthCalendar1.DateSelected += new DateRangeEventHandler(monthCalendar1_DateSelected);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ResData.Rows.Clear();

            name = textBox1.Text.Trim();
            try
            {
                id = (int)Convert.ToInt32(maskedTextBox1.Text);
            }
            catch(Exception ex)
            { id = 0; }
            if (checkBox1.Checked) YN = true; else YN = false;
            object[] rowVals = new object[6];
            rowVals[0] = id;
            rowVals[1] = data;
            rowVals[2] = name;
            rowVals[3] = YN;
            rowVals[4] = getKindId();
            rowVals[5] = getTemaId();
           
            ResData.Rows.Add(rowVals);
            string file = CommonValues.CommonClass.TempDirectory + Path.GetRandomFileName() + ".xml";
            if (File.Exists(file)) File.Delete(file);
            FileStream fst = new FileStream(file, FileMode.Create);
            ResData.WriteXml(fst, XmlWriteMode.WriteSchema);
            fst.Close();
            Document_Viewer DocSee = NewDoc.FillDocuments(file, getKindId(), getTemaId(), "Шаблон");
            this.Controls.Add(DocSee);
            DocSee.BringToFront();
        }

        void monthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            data=e.Start;
        }

        private int getKindId()
        {
            int ret = 0;
            try
            {
                string kindn = "";
                AMAS_access.Set_table("SpecDoc", AMAS_Query.Class_AMAS_Query.Get_Temy_Kind_Employee(2), null);
                for (int i = 0; i < AMAS_access.Rows_count; i++)
                {
                    AMAS_access.Get_row(i);
                    kindn=(string)AMAS_access.Find_Field("kind");
                    if (KindName.ToLower().CompareTo(kindn.ToLower().Trim()) == 0)
                    {
                        ret = (int)AMAS_access.Find_Field("kod");
                        break;
                    }
                }
                AMAS_access.ReturnTable();
            }
            catch { }
            return ret;
        }

        private int getTemaId()
        {
            int ret = 0;
            try
            {
                string teman = "";
                AMAS_access.Set_table("SpecDoc", AMAS_Query.Class_AMAS_Query.Get_Temy_Kind_Employee(1), null);
                for (int i = 0; i < AMAS_access.Rows_count; i++)
                {
                    AMAS_access.Get_row(i);
                    teman = (string)AMAS_access.Find_Field("description_");
                    if (TemaName.ToLower().CompareTo(teman.ToLower().Trim()) == 0)
                    {
                        ret = (int)AMAS_access.Find_Field("tema");
                        break;
                    }
                }
                AMAS_access.ReturnTable();
            }
            catch { }
            return ret;
        }

    }
}
