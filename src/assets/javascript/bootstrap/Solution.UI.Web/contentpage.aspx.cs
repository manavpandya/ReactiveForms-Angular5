using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web
{
    public partial class contentpage : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if(Request.QueryString["id"] != null)
                {
                    GetPleatGuideData();
                }
               
            }
        }
        private void GetPleatGuideData()
        {
            DataSet ds = new DataSet();
            lttopic.Text = "";
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString().IndexOf(",") > -1)
            {
                string[] strtopic = Request.QueryString["id"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (strtopic.Length > 0)
                {
                    for (int i = 0; i < strtopic.Length; i++)
                    {
                        ds = TopicComponent.GetTopicAccordingStoreID(strtopic[i].ToString(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            lttopic.Text += "<div class=\"static-title\"><span style=\"padding-left: 10px;\">" + ds.Tables[0].Rows[0]["Title"].ToString() + "</span></div>" + ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
                        }
                    }
                   
                     
                }
                
            }
            else
            {
                ds = TopicComponent.GetTopicAccordingStoreID(Request.QueryString["id"].ToString(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    lttopic.Text += "<div class=\"static-title\"><span style=\"padding-left: 10px;\">" + ds.Tables[0].Rows[0]["Title"].ToString() + "</span></div>" + ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
                }
                else
                {
                    lttopic.Text = "Comming Soon.....";
                }
            }
           
        }
    }
}