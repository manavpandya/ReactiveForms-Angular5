using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Net.Mail;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web
{
    public partial class Requestacall : System.Web.UI.Page
    {
        CustomerComponent objCustomer = null;
        protected void Page_Load(object sender, EventArgs e)
        {
            
                ContactusData();
           
        }

        private void ContactusData()
        {
            tb_ContactUs tb_ContactUs = new tb_ContactUs();
            tb_ContactUs.Name = Request.Form["firstname"].ToString() + " " + Request.Form["lastname"].ToString();
            tb_ContactUs.Email = Request.Form["email"].ToString();
            tb_ContactUs.City = "";
            tb_ContactUs.Message = Request.Form["information"].ToString();
            tb_ContactUs.Address = Request.Form["message"].ToString();
            //tb_ContactUs.Country = Request.Form["Country"].ToString();
            ContactUsComponent objContactus = new ContactUsComponent();
            int isadded = objContactus.InsertContactUsDetail(tb_ContactUs);
            if (isadded == 0)
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding contact us details, please try again...');", true);
            else
            {
                string MailAddress = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='ContactMail_ToAddress'"));
                //string MailAddress = "asdas@asa.as";
                if (MailAddress != string.Empty)
                {
                    SendMail(MailAddress);
                    SendMailToCustomer(Request.Form["email"].ToString(), isadded);
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgsend", "alert('E-Mail has been sent successfully.');window.opener.location.href='http://www.curtainsonbudget.com/info/ThankYou';", true);
            }
        }

        #region send Mail

        /// <summary>
        /// Sends the Mail.
        /// </summary>
        /// <param name="TOAddress">string TOAddress</param>
        private void SendMail(string TOAddress)
        {
            objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();

            dsMailTemplate = objCustomer.GetEmailTamplate("RequesacallForAdmin", Convert.ToInt32(3));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {

                StringBuilder sw = new StringBuilder(4000);
                sw.Append("<table width='100%' class='popup_cantain'></tr>");
                sw.Append("<tr><td  style='margin-top:7px;width:30%;'>");
                sw.Append("<b style='color:#000000;'>Name :</b></td><td  style='margin-top:10px;width:70%;'>" + Request.Form["email"].ToString() + "</td></tr>");
                sw.Append("<tr><td ><b style='color:#000000;'>E-Mail address :</b></td><td>  <a href='mailto:" + Request.Form["Email"].ToString() + "' style='color:#000000;'>" + Request.Form["email"].ToString() + "</a></td></tr>");


                if (Request.Form["message"].ToString() != "")
                    sw.Append("<tr><td><b style='color:#000000;'>Address :</b></td><td > " + Request.Form["message"].ToString() + "</td></tr>");
                //if (Request.Form["txtCity"].ToString() != "")
                //  sw.Append("<tr><td><b style='color:#000000;'>City :</b></td><td > " + Request.Form["txtCity"].ToString() + "</td></tr>");
                //if (ddlcountry.SelectedIndex != -11)
                //    sw.Append("<tr><td><b style='color:#000000;'>Country :</b></td><td > " + ddlcountry.SelectedItem.ToString() + "</td></tr>");
                //if (ddlstate.SelectedIndex != -11 && ddlstate.SelectedItem.ToString() != "Select State/Province" && ddlstate.SelectedItem.ToString() != "Other")
                //    sw.Append("<tr><td><b style='color:#000000;'>State :</b> </td><td >" + ddlstate.SelectedItem.ToString() + "</td></tr>");
                //else if (txtother.Text.Trim() != "")
                //    sw.Append("<tr><td><b style='color:#000000;'>State :</b></td><td > " + txtother.Text.Trim() + "</td></tr>");
                //if (txtzipcode.Text.Trim() != "")
                //    sw.Append("<tr><td><b style='color:#000000;'>Zip :</b></td><td > " + txtzipcode.Text.Trim() + "</td></tr>");
                //if (txtPhone.Text.Trim() != "")
                //    sw.Append("<tr><td><b style='color:#000000;'>Phone :</b></td><td > " + txtPhone.Text.Trim() + "</td></tr>");
                //if (txtinformation.Text.Trim() != "")
                //    sw.Append("<tr><td><b style='color:#000000;'>Specific Information :</b></td><td style='padding-top:5px;padding-bottom:5px' > " + txtinformation.Text.ToString() + "</td></tr>");
                //sw.Append("</table>");


                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                string STORENAME = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='STORENAME'"));
                string LIVE_SERVER = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='Live_Server'"));
                string STOREPATH = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='STOREPATH'"));
                strSubject = Regex.Replace(strSubject, "###STORENAME###", STORENAME.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", LIVE_SERVER.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STOREPATH###", STOREPATH.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", STORENAME.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###ContactUsDetails###", sw.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", "3", RegexOptions.IgnoreCase);
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(TOAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }

        /// <summary>
        /// Sends the Mail to Customer
        /// </summary>
        /// <param name="TOAddress">string TOAddress</param>
        /// <param name="ContactID">int ContactID</param>
        private void SendMailToCustomer(string TOAddress, Int32 ContactID)
        {
            objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();
            dsMailTemplate = objCustomer.GetEmailTamplate("RequesacallForAdmin", Convert.ToInt32(3));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                string STORENAME = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='STORENAME'"));
                string LIVE_SERVER = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='Live_Server'"));
                string STOREPATH = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Storeid=3 AND Configname='STOREPATH'"));

                strSubject = Regex.Replace(strSubject, "###STOREPATH###", STOREPATH.ToString(), RegexOptions.IgnoreCase);
                strSubject = Regex.Replace(strSubject, "###YOURNAME###", Request.Form["firstname"].ToString() + " " + Request.Form["lastname"].ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STOREPATH###", STOREPATH.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", STORENAME.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", LIVE_SERVER.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", "3", RegexOptions.IgnoreCase);
                CommonComponent.ExecuteCommonData("UPDATE tb_ContactUs SET Subject='" + strSubject.ToString().Replace("'", "''") + "' WHERE ContactUsID=" + ContactID + "");
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(TOAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }

        #endregion

    }
}