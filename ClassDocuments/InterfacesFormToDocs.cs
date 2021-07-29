using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using AMAS_DBI;

namespace ClassInterfases
{
    public interface FormShowCon
    {
        System.Windows.Forms.ImageList imagelib();
        System.Windows.Forms.Panel panel();
        AMAS_DBI.Class_syb_acc DB_acc();
        System.Windows.Forms.ToolStripProgressBar FuelBar();
    }

    public interface GSADR
    {
        int GSAddressID();
        string GSAddressString();
    }

    public interface GSORG
    {
        string GSOrgName();
        int GSOrgID();
    }
}
