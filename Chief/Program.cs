using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using AMAS_DBI;
using CommonValues;

namespace Chief
{
    class MyApplicationContext : ApplicationContext
    {
        private int tryEnter = 0;
        private FormConnect form1;
        private Form form2;

        public MyApplicationContext()
        {
            form1 = new FormConnect();
            form1.Closing += new CancelEventHandler(OnFormClosing);
            form1.Show();

        }

        private void OnFormClosing(object sender, CancelEventArgs e)
        {
            // When a form is closing, remember the form position so it
            // can be saved in the user data file.
            if (sender is FormConnect)
            {
                AMAS_DBI.Class_syb_acc SYB_acc = Dbase();
                if (SYB_acc.Connected)
                {
                    form2 = new Master(SYB_acc);
                    form2.Closing += new CancelEventHandler(OnFormClosing);
                    form2.Show();
                }
                else
                {
                    if (tryEnter == 3)
                    {
                        MessageBox.Show("Количество попыток входа в систему исчерпано.");
                        Application.Exit();
                    }
                    else
                    {
                        MessageBox.Show("Неверно заданы параметры входа в систему. Повторите ввод параметров.");
                        form1 = new FormConnect();
                        form1.Closing += new CancelEventHandler(OnFormClosing);
                        form1.Show();
                    }
                     tryEnter++;
               }
            }
            if (sender is Master)
                Application.Exit();

        }

        private AMAS_DBI.Class_syb_acc Dbase()
        {
            AMAS_DBI.Class_syb_acc SYB_acc = new Class_syb_acc(form1.conn_select_DB, form1.conn_ODBC, form1.PWD, form1.UID);
            return SYB_acc;
        }

    }
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>


        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //Application.Run(new Chef());

            MyApplicationContext context = new MyApplicationContext();
            Application.Run(context);
        }
    }
}