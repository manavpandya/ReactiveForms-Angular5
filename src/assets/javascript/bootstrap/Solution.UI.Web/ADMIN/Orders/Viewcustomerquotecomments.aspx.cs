using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class Viewcustomerquotecomments : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Request.QueryString["storeId"] != null)
            {
                if (Session["AdminID"] == null || Session["AdminID"].ToString() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter4", "window.parent.location.href='/Admin/Login.aspx';", true);
                    return;
                }
                else
                {
                    String strScript = @"$(function () {
                                    $('#header').css('display', 'none');
                                    $('#footer').css('display', 'none');
                                    $('.content-row1').css('display', 'none');
                                    $('body').css('background', 'none');
                                    });";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter", strScript.Trim(), true);
                }
            }
            if (!IsPostBack)
            {
               
                imgsendmail.ImageUrl = "/App_Themes/" + Page.Theme + "/button/send-email.png";
              //  popupContactClose.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                BindCustomerComent();
            
            }

        }

        private void BindCustomerComent()
        {
            if (Request.QueryString["custquoteid"] != null && Request.QueryString["custquoteid"].ToString() != "")
            {
                String custquoteid =Request.QueryString["custquoteid"].ToString();
                String Storeid = Request.QueryString["StoreId"].ToString();

                DataSet dsCustDetails = CommonComponent.GetCommonDataSet("select * from tb_contactus where  QuoteID=" + custquoteid + "");

                    if (dsCustDetails != null && dsCustDetails.Tables.Count > 0)
                    {

                        if (dsCustDetails.Tables[0].Rows[0]["name"] != null && dsCustDetails.Tables[0].Rows[0]["name"].ToString() != "")
                            lblname.Text = dsCustDetails.Tables[0].Rows[0]["name"].ToString();
                        txtsubject.Text = "RE: " + dsCustDetails.Tables[0].Rows[0]["Subject"].ToString();

                        if (dsCustDetails.Tables[0].Rows[0]["Email"] != null && dsCustDetails.Tables[0].Rows[0]["Email"].ToString() != "")
                            lblEmail.Text = dsCustDetails.Tables[0].Rows[0]["Email"].ToString();

                        if (dsCustDetails.Tables[0].Rows[0]["Address"] != null && dsCustDetails.Tables[0].Rows[0]["Address"].ToString() != "")
                            lblAddress.Text = dsCustDetails.Tables[0].Rows[0]["Address"].ToString();

                        if (dsCustDetails.Tables[0].Rows[0]["Message"] != null && dsCustDetails.Tables[0].Rows[0]["Message"].ToString() != "")
                            lblQuoteComment.Text = dsCustDetails.Tables[0].Rows[0]["Message"].ToString();

                    }
                   
                  
            }
           
        }

        protected void imgsendmail_Click(object sender, ImageClickEventArgs e)
        {
            string strBody = "";                 
            DataSet dsTemplate = new DataSet();
            dsTemplate = CommonComponent.GetCommonDataSet("select * from  dbo.tb_EmailTemplate where storeid=" + AppLogic.AppConfigs("Storeid").ToString() + " And Label ='CustomerCommentsReplyEmail'");
            if (dsTemplate != null && dsTemplate.Tables.Count > 0 && dsTemplate.Tables[0].Rows.Count > 0)
            {
                strBody = dsTemplate.Tables[0].Rows[0]["EmailBody"].ToString();

                strBody = Regex.Replace(strBody, "###Name###", lblname.Text.ToString(), RegexOptions.IgnoreCase);      
                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###Message###", txtmsgbody.Text.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                try
                {
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody, null, "text/html");
                    CommonOperations.SendMail(lblEmail.Text.ToString().Trim(), txtsubject.Text.ToString().Trim(), strBody, Request.UserHostAddress.ToString(), true, av);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Mail has been Sent Successfully.');", true);                   
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "closeWindow", "javascript:window.close();", true);
                }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Mail Sending problem.');", true);
                }
            }

         

        }
    }
}