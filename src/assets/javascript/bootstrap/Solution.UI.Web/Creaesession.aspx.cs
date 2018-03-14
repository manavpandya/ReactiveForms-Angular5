using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web
{
    public partial class Creaesession : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["pnm"] != null && Request.QueryString["pid"] != null)
            {
                Session["madeorder"] = Request.QueryString["pid"].ToString();
                //Response.Redirect(Request.QueryString["pnm"].ToString());
            }
        }
    }
}