using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class Dashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("/Admin/login.aspx");
            }
            if(!IsPostBack)
            {
                LoadUserControls();
            }
        }

        private void LoadUserControls()
        {
            Literal ltrCommon = null;
            String strControlPathPrefix = "/REPLENISHMENTMANAGEMENT/Controls/";
            ltrCommon = new Literal();

            ltrCommon.Text = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\">";
            divleftControls.Controls.Add(ltrCommon);
            ltrCommon.Dispose();

            if (File.Exists(Server.MapPath(strControlPathPrefix+"/InventoryFeedGenerateLog.ascx")))
            {
                strControlPathPrefix = strControlPathPrefix + "/InventoryFeedGenerateLog.ascx";
                    ltrCommon = new Literal();
                    ltrCommon.Text = "<tr><td>";
                    divleftControls.Controls.Add(ltrCommon);
                    ltrCommon.Dispose();
                    UserControl usrcntrl = (UserControl)Page.LoadControl(strControlPathPrefix.ToString());
                    usrcntrl.EnableViewState = false;
                    divleftControls.Controls.Add(usrcntrl);
                    ltrCommon = new Literal();
                    ltrCommon.Text = "</td></tr>";
                    divleftControls.Controls.Add(ltrCommon);
                    ltrCommon.Dispose();
                }
            
            ltrCommon = new Literal();
            ltrCommon.Text = "</table>";
            divleftControls.Controls.Add(ltrCommon);
            ltrCommon.Dispose();
        }
    }
}