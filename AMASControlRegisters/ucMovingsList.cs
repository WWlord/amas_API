using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AMASControlRegisters
{
    public partial class UCMoviesList : UserControl
    {
        private AMAS_DBI.Class_syb_acc AMAS_access;
        int Answer_count;
        public int moving = 0;

        public UCMoviesList(int document, AMAS_DBI.Class_syb_acc ACC)
        {
            InitializeComponent();

            AMAS_access = ACC;
            listViewMovies.MultiSelect = false;
            listViewMovies.Items.Clear();

            if (AMAS_access.Set_table("ucMoving", "select rkk_moving.moving, rkk_moving.signing, emp_ent_employees.fio from dbo.rkk_moving join dbo.emp_ent_employees on rkk_moving.typist=emp_ent_employees.id where document= " + document.ToString() + " and for_  in (select cod from dbo.emp_dep_degrees where employee=dbo.user_ident() and executed is null)", null))
            {
                Answer_count = AMAS_access.Rows_count;
                for (int i = 0; i < AMAS_access.Rows_count; i++)
                {
                    AMAS_access.Find_Field("fio");
                    ListViewItem listItem = listViewMovies.Items.Add((string)AMAS_access.get_current_Field());
                    AMAS_access.Find_Field("moving");
                    listItem.Name = "mov"+(string)AMAS_access.get_current_Field();
                    AMAS_access.Find_Field("signing");
                    listItem.SubItems[1].Text = (string)AMAS_access.get_current_Field();
                }
                AMAS_access.ReturnTable();
            }


            listViewMovies.DoubleClick += new EventHandler(listViewMovies_DoubleClick);
        }

        void listViewMovies_DoubleClick(object sender, EventArgs e)
        {
        }

        private void listViewMovies_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        public int[] movlist()
        {
            int[] movs=null;
            int i = 0;
            if (listViewMovies.SelectedItems.Count > 0) movs = new int[listViewMovies.SelectedItems.Count];
            foreach (ListViewItem listItem in listViewMovies.SelectedItems)
                movs[i++] = (int)Convert.ToInt32(listItem.Name.Substring(3));
            return movs;
        }
    }
}
