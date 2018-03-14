using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;

namespace Solution.UI.Web
{
    public partial class Unsubscribe : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (!IsPostBack)
            {
                unsubscribe();
                ltbrTitle.Text = "Unsubscribe Newsletter";
                ltTitle.Text = "Unsubscribe Newsletter";
            }
        }

        /// <summary>
        /// This Method takes Email from Query string and
        /// unsubscribe that customer
        /// </summary>
        private void unsubscribe()
        {
            try
            {
                NewsSubscribtionComponent objNews = new NewsSubscribtionComponent();
                if (Request.QueryString["EMail"] != null && Request.QueryString["EMail"].ToString() != "")
                {
                    String Email = Server.HtmlDecode(Request.QueryString["EMail"]).ToString();
                    Email = SecurityComponent.Decrypt(Email);

                    DataSet DSNews = new DataSet();
                    DSNews = objNews.GetNewsSubscribtionList(Email, Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()));

                    if (DSNews != null && DSNews.Tables.Count > 0 && DSNews.Tables[0].Rows.Count > 0)
                    {
                        if (DSNews.Tables[0].Rows[0]["NewsSubscriptionID"] != null)
                        {
                            int Id = Convert.ToInt32(DSNews.Tables[0].Rows[0]["NewsSubscriptionID"].ToString());
                            objNews.delNews(Id);
                            lblMsg.Text = "You have been Unsubscribed Successfully...";
                            lblMsg.Visible = true;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "You have been already Unsubscribed...";
                        lblMsg.Visible = true;
                    }
                }
            }
            catch (Exception ex)
            {
                //COmm.WriteLog("\r\n Page Name :Unsubscribe \r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->unsubscribe() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }

    }
}