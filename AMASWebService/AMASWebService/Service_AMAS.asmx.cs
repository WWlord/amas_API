using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Xml.Linq;
using AMAS_DBI;
using ClassStructure;

namespace AMASWebService
{
    /// <summary>
    /// Сводное описание для Service_AMAS
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из сценария с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    // [System.Web.Script.Services.ScriptService]
    public class Service_AMAS : System.Web.Services.WebService
    {
        Structure EntStructure = null;
        AMAS_DBI.Class_syb_acc AmasAcc=null;

        [WebMethod]
        public bool Connect(string select_ServerDB, string userName, string Password)
        {
            AmasAcc = new Class_syb_acc((int)AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL, select_ServerDB, Password, userName);
            if (AmasAcc != null) return true; else return false;
        }

        [WebMethod]
        public bool TakeStructure(string select_ServerDB, string userName, string Password)
        {
            AmasAcc = new Class_syb_acc((int)AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL, select_ServerDB, Password, userName);
            if (AmasAcc != null) EntStructure = new Structure(AmasAcc); else EntStructure = null;
            if (EntStructure == null) return false;
            else return true;
        }

        [WebMethod]
        public string[] GetDepts(string select_ServerDB, string userName, string Password)
        {
            AmasAcc = new Class_syb_acc((int)AMAS_DBI.Class_syb_acc.AMAS_connections.MSSQL, select_ServerDB, Password, userName);
            if (AmasAcc != null) EntStructure = new Structure(AmasAcc);
            if (EntStructure != null) return EntStructure.draw_WebSshema_enterprice(); else return null;           
        }

    }
}
