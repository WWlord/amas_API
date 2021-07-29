using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace WebApplicationAMAS
{
    public partial class WebFormAccount : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            WebFormInner wf = new WebFormInner();
            wf.Visible = true;
        }
        string PWD;
        string UName;
        string Quest;
        string Answer;
        string eMail;

        protected void CreateUserWizard1_CreatedUser(object sender, EventArgs e)
        {
            
        }

        protected void CreateUserWizard1_CreatedUser1(object sender, EventArgs e)
        {
            PWD=CreateUserWizard1.Password;
            Quest=CreateUserWizard1.Question;
            UName=CreateUserWizard1.UserName;
            Answer=CreateUserWizard1.Answer;
            eMail=CreateUserWizard1.Email;
            CreateUserWizard1.LoginCreatedUser = true;
        }
    }
}
