using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Web.UI.HtmlControls;
using StringBuilder = System.Text.StringBuilder;
using System.Text.RegularExpressions;

namespace Solution.UI.Web
{
    public partial class StaticPage : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (Request.QueryString["StaticPage"] != null && Request.QueryString["StaticPage"].ToString() != "")
            {
                BindStaticData();
                if (Request.QueryString["StaticPage"].ToString().ToLower() == "freeshipping")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgheadertop", "tabheaderhide('lifreeshipp','litradepartner');", true);
                }
                else if (Request.QueryString["StaticPage"].ToString().ToLower() == "tradepartnerprogram")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgheadertop", "tabheaderhide('litradepartner','lifreeshipp');", true);
                }
            }

        }

        /// <summary>
        /// Binds the Static Data
        /// </summary>
        private void BindStaticData()
        {
            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList(Request.QueryString["StaticPage"].ToString(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            try
            {

                String SETitle = "";
                String SEKeywords = "";
                String SEDescription = "";
                if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["SETitle"].ToString()))
                {
                    SETitle = dsTopic.Tables[0].Rows[0]["SETitle"].ToString();
                }
                else
                {
                    SETitle = AppLogic.AppConfigs("SiteSETitle").ToString();
                }

                if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["SEKeywords"].ToString()))
                {
                    SEKeywords = dsTopic.Tables[0].Rows[0]["SEKeywords"].ToString();
                }
                else
                {
                    SEKeywords = AppLogic.AppConfigs("SiteSEKeywords").ToString();
                }


                if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["SEDescription"].ToString()))
                {
                    SEDescription = dsTopic.Tables[0].Rows[0]["SEDescription"].ToString();
                }
                else
                {
                    SEDescription = AppLogic.AppConfigs("SiteSEDescription").ToString();
                }
                Master.HeadTitle(SETitle, SEKeywords, SEDescription);
            }
            catch { }
            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                litBreadTitle.Text = Convert.ToString(dsTopic.Tables[0].Rows[0]["Title"].ToString().Replace("<span>", " ").Replace("</span>", " "));
                ltTitle.Text = Convert.ToString(dsTopic.Tables[0].Rows[0]["Title"].ToString().Replace("<span>", " ").Replace("</span>"," "));
                if (dsTopic.Tables[0].Rows[0]["Description"].ToString() == "")
                {
                    ltPage.Text = "<p style='padding-left:20px;'>Coming Soon...</p>";
                }
                else
                {
                    ltPage.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                }

                dsTopic.Dispose();
            }

        }
    }
}