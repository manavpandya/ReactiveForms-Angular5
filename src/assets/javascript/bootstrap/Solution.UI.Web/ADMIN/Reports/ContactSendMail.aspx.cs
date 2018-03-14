using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using System.Net.Mail;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class ContactSendMail : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSubmit.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/send-email.png";

                DataSet ds = new DataSet();
                ds = CommonComponent.GetCommonDataSet("SELECT Subject,Email FROM tb_ContactUs WHERE ContactUsID=" + Request.QueryString["MID"] + "");
                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    txtSubject.Text = "RE: " + ds.Tables[0].Rows[0]["Subject"].ToString().Trim();
                    ViewState["Email"] = ds.Tables[0].Rows[0]["Email"].ToString().Trim();
                }
            }
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubmit_onclick(object sender, ImageClickEventArgs e)
        {
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsCreateAccount = new DataSet();
            dsCreateAccount = objCustomer.GetEmailTamplate("ContactusReplyEmail", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsCreateAccount != null && dsCreateAccount.Tables.Count > 0 && dsCreateAccount.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                strBody = dsCreateAccount.Tables[0].Rows[0]["EmailBody"].ToString();
                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###Message###", txtmsgbody.Text.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                DataSet dsTopic = new DataSet();
                dsTopic = TopicComponent.GetTopicList("InvoiceSignature", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    strBody = Regex.Replace(strBody, "###Signature###", dsTopic.Tables[0].Rows[0]["Description"].ToString(), RegexOptions.IgnoreCase);
                    dsTopic.Dispose();
                }
                else
                {
                    strBody = Regex.Replace(strBody, "###Signature###", "Thank You, <br />" + AppLogic.AppConfigs("StoreName").ToString() + "", RegexOptions.IgnoreCase);
                }
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody, null, "text/html");
                try
                {
                    if (ViewState["Email"] != null)
                    {
                        CommonOperations.SendMail(ViewState["Email"].ToString(), txtSubject.Text.ToString(), strBody, Request.UserHostAddress.ToString(), true, av);
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Mail has been Sent Successfully.');", true);
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "AlertMessage", "jAlert('Mail has been Sent Successfully.', 'POMessage','');", true);
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "closeWindow", "javascript:window.close();",true);
                    }
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Mail Sending problem.');", true);
                }

            }
        }
    }
}