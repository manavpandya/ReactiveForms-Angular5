using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class Setsession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Request.QueryString["Ids"] != null)
            {
                Session["pidevent"] = Request.QueryString["Ids"].ToString();
            }
            else if (Request.QueryString["cIds"] != null)
            {
                Session["pidcolection"] = Request.QueryString["cIds"].ToString();
            }
        }
    }
}