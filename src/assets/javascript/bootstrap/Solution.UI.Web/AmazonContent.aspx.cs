using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web
{
    public partial class AmazonContent : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (Request.QueryString["StaticPage"] != null && Request.QueryString["StaticPage"].ToString() != "")
            {
                BindStaticData();
            }
        }

        /// <summary>
        /// Binds the Static Data
        /// </summary>
        private void BindStaticData()
        {
            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList(Request.QueryString["StaticPage"].ToString(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            //#region SEO
            //try
            //{
            //    String SETitle = "";
            //    String SEKeywords = "";
            //    String SEDescription = "";
            //    if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["SETitle"].ToString()))
            //    {
            //        SETitle = dsTopic.Tables[0].Rows[0]["SETitle"].ToString();
            //    }
            //    else
            //    {
            //        SETitle = AppLogic.AppConfigs("SiteSETitle").ToString();
            //    }

            //    if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["SEKeywords"].ToString()))
            //    {
            //        SEKeywords = dsTopic.Tables[0].Rows[0]["SEKeywords"].ToString();
            //    }
            //    else
            //    {
            //        SEKeywords = AppLogic.AppConfigs("SiteSEKeywords").ToString();
            //    }


            //    if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["SEDescription"].ToString()))
            //    {
            //        SEDescription = dsTopic.Tables[0].Rows[0]["SEDescription"].ToString();
            //    }
            //    else
            //    {
            //        SEDescription = AppLogic.AppConfigs("SiteSEDescription").ToString();
            //    }
            //    //Master.HeadTitle(SETitle, SEKeywords, SEDescription);
            //}
            //catch { }
            //#endregion

            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                if (!String.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["Title"].ToString()))
                {
                    ltTitle.Text = Convert.ToString(dsTopic.Tables[0].Rows[0]["Title"].ToString().Replace("<span>", " ").Replace("</span>", " "));
                }
                if (!String.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["Description"].ToString()))
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