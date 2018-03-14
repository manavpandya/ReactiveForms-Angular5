using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Solution.UI.Web
{
    public class BasePage : System.Web.UI.Page
    {
        protected override void OnPreInit(EventArgs e)
        {
            base.OnPreInit(e);
            if (Session["MyTheme"] == null)
            {
                if (Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) != "")
                {
                    string strTheme = Solution.Bussines.Components.AdminComponent.GetTheme(Convert.ToInt32(Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"])));
                    if (strTheme != "")
                    {
                        Session["MyTheme"] = strTheme;
                    }
                    else
                    {
                        Session["MyTheme"] = "Gray";
                    }
                }
                else
                {
                    Session["MyTheme"] = "Gray";
                }
                Page.Theme = ((string)Session["MyTheme"]);
            }
            else
            {
                Page.Theme = ((string)Session["MyTheme"]);
            }
        }
    }
}