using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class AdminReplnishment : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(Session["AdminID"]==null)
            {
                Response.Redirect("/Admin/login.aspx");
            }
            if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("dashboard.aspx") > -1)
            {
                dashboardsubmenu.Attributes.Add("class", "active");
                liproduct.Attributes.Add("class", "dcjq-parent active");
               // lidashborad.Attributes.Add("class", "active");
            }
            else if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("importreplenishmentdata.aspx") > -1)
            {
                ImportReplenishmentData.Attributes.Add("class", "active");
                liproduct.Attributes.Add("class", "dcjq-parent active");
            }
            else if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("generateinventoryfeed.aspx") > -1)
            {
                GenerateInventoryFeed.Attributes.Add("class", "active");
                liproduct.Attributes.Add("class", "dcjq-parent active");
            }
            else if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("setupinventoryfeedschedular.aspx") > -1)
            {
                SetupInventoryFeedSchedular.Attributes.Add("class", "active");
                liproduct.Attributes.Add("class", "dcjq-parent active");
            }
            else if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("setupinventoryfeedtemplate.aspx") > -1)
            {
                SetupInventoryFeedtemplate.Attributes.Add("class", "active");
                liproduct.Attributes.Add("class", "dcjq-parent active");
            }
            else if (Request.Url != null && Request.Url.ToString().ToLower().IndexOf("setupinventoryfeeddata.aspx") > -1)
            {
                SetupInventoryFeeddata.Attributes.Add("class", "active");
                liproduct.Attributes.Add("class", "dcjq-parent active");
            }
             
            
        
        }
    }
}