using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web
{
    public partial class inventorymessage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "loadsscript", "document.getElementById('divinventory').innerHTML=window.parent.document.getElementById('hdnhtmlall').value;", true);
        }
    }
}