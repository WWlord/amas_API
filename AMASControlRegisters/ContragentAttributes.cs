using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;

namespace AMASControlRegisters
{
    public partial class ContragentAttributes : UserControl
    {
        private AMAS_DBI.Class_syb_acc ACC;

        private DataTable Phones;
        private DataTable Emails;
        private int agent;

        public ContragentAttributes()
        {
            InitializeComponent();

            dataGridViewPhone.VirtualMode = true;
            dataGridViewPhone.Enabled = true;
            dataGridViewPhone.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewPhone.AllowUserToAddRows = true;
            dataGridViewPhone.AllowUserToDeleteRows = true;

            dataGridViewEmail.VirtualMode = true;
            dataGridViewEmail.Enabled = true;
            dataGridViewEmail.EditMode = DataGridViewEditMode.EditOnEnter;
            dataGridViewEmail.AllowUserToAddRows = true;
            dataGridViewEmail.AllowUserToDeleteRows = true;
        }

        public void Conect(AMAS_DBI.Class_syb_acc AACC)
        {
            ACC = AACC;
        }

        public void EstablePost(int agentId)
        {
            agent = agentId;
            if (ACC.Set_table("Catt1", "select * from dbo.age_phone where agent=" + agentId.ToString(), null))
            {
                Phones = ACC.Current_table;
                ACC.ReturnTable();
                dataGridViewPhone.DataSource = Phones;
                try
                {
                    dataGridViewPhone.Columns["Id"].Visible = false;
                    dataGridViewPhone.Columns["agent"].Visible = false;
                    dataGridViewPhone.Columns["userId"].Visible = false;
                    dataGridViewPhone.Columns["date_change"].Visible = false;
                    dataGridViewPhone.Columns["Phone"].HeaderText = "Телефон";
                    dataGridViewPhone.Columns["wdo"].HeaderText = "Описание";
                }
                catch (Exception ex)
                {
                    ACC.EBBLP.AddError(ex.Message, "Contragent Attr - 1", ex.StackTrace);
                }
                Phones.TableNewRow += new DataTableNewRowEventHandler(Phones_TableNewRow);
                Phones.RowDeleting += new DataRowChangeEventHandler(Phones_RowDeleting);
                Phones.RowChanged += new DataRowChangeEventHandler(Phones_RowChanged);
            }

            if (ACC.Set_table("Catt2", "select * from dbo.age_email where agent=" + agentId.ToString(), null))
            {
                Emails = ACC.Current_table;
                ACC.ReturnTable();
                dataGridViewEmail.DataSource = Emails;
                try
                {
                    dataGridViewEmail.Columns["Id"].Visible = false;
                    dataGridViewEmail.Columns["agent"].Visible = false;
                    dataGridViewEmail.Columns["userId"].Visible = false;
                    dataGridViewEmail.Columns["dat_change"].Visible = false;
                    dataGridViewEmail.Columns["Email"].HeaderText = "Почтовый ящик";
                    dataGridViewEmail.Columns["wdo"].HeaderText = "Описание";
                }
                catch (Exception ex)
                {
                    ACC.EBBLP.AddError(ex.Message, "Contragent Attr - 2", ex.StackTrace);
                }
                Emails.TableNewRow += new DataTableNewRowEventHandler(Emails_TableNewRow);
                Emails.RowDeleting += new DataRowChangeEventHandler(Emails_RowDeleting);
                Emails.RowChanged += new DataRowChangeEventHandler(Emails_RowChanged);
            }
        }

        private void Phones_RowDeleting(Object sender, DataRowChangeEventArgs e)
        {
            AMASCommand.agent_phone_erise((int)e.Row["Id"]);
        }

        private void Emails_RowDeleting(Object sender, DataRowChangeEventArgs e)
        {
            AMASCommand.agent_email_erise((int)e.Row["Id"]);
        }

        private void Phones_RowChanged(Object sender, DataRowChangeEventArgs e)
        {
            string wdo = "";
            string phone = "";
            try
            {
                wdo = (string)e.Row["wdo"];
            }
            catch { }
            try
            {
                phone = (string)e.Row["phone"];
            }
            catch { }
            AMASCommand.agent_phone_update((int)e.Row["Id"], phone, wdo);
        }

        private void Emails_RowChanged(Object sender, DataRowChangeEventArgs e)
        {
            string wdo = "";
            string Emails = "";
            try
            {
                wdo = (string)e.Row["wdo"];
            }
            catch { }
            try
            {
                Emails = (string)e.Row["Email"];
            }
            catch { }
            AMASCommand.agent_email_update((int)e.Row["Id"], Emails, wdo);
        }

        private void Phones_TableNewRow(Object sender, DataTableNewRowEventArgs e)
        {
            e.Row["agent"] = agent;
            string wdo = "";
            string phone = "";
            try
            {
                wdo = (string)e.Row["wdo"];
            }
            catch { }
            try
            {
                phone = (string)e.Row["phone"];
            }
            catch { }
            e.Row["id"] = AMASCommand.agent_phone_append(agent, phone, wdo);
        }

        private void Emails_TableNewRow(Object sender, DataTableNewRowEventArgs e)
        {
            e.Row["agent"] = agent;
            string wdo = "";
            string Emails = "";
            try
            {
                wdo = (string)e.Row["wdo"];
            }
            catch { }
            try
            {
                Emails = (string)e.Row["Email"];
            }
            catch { }
            e.Row["id"] = AMASCommand.agent_email_append(agent, Emails, wdo);
        }

        private void удалитьТелефонToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            try
            {
                Phones.Rows[dataGridViewPhone.CurrentRow.Index].Delete();
                dataGridViewPhone.Refresh();
            }
            catch
            {
            }
        }

        private void toolStripMenuItem1_Click_1(object sender, EventArgs e)
        {
            Emails.Rows[dataGridViewEmail.CurrentRow.Index].Delete();
            dataGridViewEmail.Refresh();
        }

    }
}
