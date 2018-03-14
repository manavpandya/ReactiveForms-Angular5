using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.Common;
using System.Text;
using System.Net.Mail;
using System.Data;

namespace Solution.UI.Web
{
    public partial class ReferAFriendAmazonProduct : System.Web.UI.Page
    {
        /// <summary>
        /// Tell A Friend Page is used to tell about 
        /// our site to your friends.
        /// <author>
        /// Kaushalam Team © Kaushalam Inc. 2012.
        /// </author>
        /// Version 1.0
        /// </summary>

        #region Local Variables
        static string rURL = AppLogic.AppConfigs("LIVE_SERVER");
        #endregion

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
                txtYourname.Focus();
                if (Request.Headers["referer"] != null)
                {
                    rURL = Convert.ToString(Request.Headers["referer"]).Trim();
                }
            }
        }

        /// <summary>
        ///  This Event Send E-Mail to your friend
        /// and also make some Validation
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnSend_Click(object sender, ImageClickEventArgs e)
        {
            Regex reValidEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (txtYourname.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your name.');", true);
                txtYourname.Focus();
                return;
            }
            else if (txtYourEmail.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your email.');", true);
                txtYourEmail.Focus();
                return;
            }
            else if (!reValidEmail.IsMatch(txtYourEmail.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid email.');", true);
                txtYourEmail.Focus();
                return;
            }
            else if (txtReEnterEmail.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter re-enter email.');", true);
                txtReEnterEmail.Focus();
                return;
            }
            else if (!reValidEmail.IsMatch(txtReEnterEmail.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid re-enter email.');", true);
                txtReEnterEmail.Focus();
                return;
            }
            else if (txtYourEmail.Text.ToString().ToLower().Trim() != txtReEnterEmail.Text.ToString().ToLower().Trim())
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Re-enter email must be same with your email.');", true);
                txtReEnterEmail.Focus();
                return;
            }
            else if (txtEmail1.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Email1.');", true);
                txtEmail1.Focus();
                return;
            }
            else if (!reValidEmail.IsMatch(txtEmail1.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid Email1.');", true);
                txtEmail1.Focus();
                return;
            }
            else if (txtYourEmail.Text.ToString().Trim() == txtEmail1.Text.ToString().Trim())
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your email and Email1 can not be same.');", true);
                txtEmail1.Focus();
                return;
            }
            else if (txtEmail2.Text != "" && !reValidEmail.IsMatch(txtEmail2.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid Email2.');", true);
                txtEmail2.Focus();
                return;
            }
            else if (txtEmail3.Text != "" && !reValidEmail.IsMatch(txtEmail3.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid Email3.');", true);
                txtEmail3.Focus();
                return;
            }

            SendEmail(txtEmail1.Text.ToString().Trim());
            if (txtEmail2.Text != "")
            {
                SendEmail(txtEmail2.Text.ToString().Trim());
            }
            if (txtEmail3.Text != "")
            {
                SendEmail(txtEmail3.Text.ToString().Trim());
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('E-Mail has been sent successfully.');", true);
            txtYourname.Text = "";
            txtYourEmail.Text = "";
            txtReEnterEmail.Text = "";
            txtEmail1.Text = "";
            txtEmail2.Text = "";
            txtEmail3.Text = "";
            txtYourname.Focus();
        }

        /// <summary>
        /// Send Email function to Friends
        /// </summary>
        /// <param name="ToAddress">String ToAddress</param>
        private void SendEmail(String ToAddress)
        {
            CustomerComponent objCustomer = new CustomerComponent();

            DataSet dsMailTemplate = new DataSet();
            dsMailTemplate = objCustomer.GetEmailTamplate("TellAFriend", Convert.ToInt32(3));
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

                strSubject = Regex.Replace(strSubject, "###YOURNAME###", txtYourname.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", LIVE_SERVER.ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###StoreID###", "3", RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STOREPATH###", STOREPATH.ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", STORENAME.ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###YOURNAME###", txtYourname.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                if (Request.QueryString["ProductID"] != null)
                {
                    strBody = Regex.Replace(strBody, "###OPTION###", "thought you may like this product", RegexOptions.IgnoreCase);
                    if (ViewState["RURL"] != null)
                        rURL = ViewState["RURL"].ToString().Trim();
                }
                else
                {
                    strBody = Regex.Replace(strBody, "###OPTION###", "thinks you may like our products", RegexOptions.IgnoreCase);
                    if (!(rURL.ToLower().ToString().Contains("/c-")) && !(rURL.ToLower().ToString().Contains("/s-")) && !(rURL.ToLower().ToString().Contains("/p-")) && !(rURL.ToLower().ToString().Contains("/index.")) && !(rURL.ToLower().ToString().Contains("/sitemap")))
                        rURL = AppLogic.AppConfigs("LIVE_SERVER");
                }

                //if (Session["CustID"] != null)
                //{
                //    strBody = Regex.Replace(strBody, "###CUSTID_LINK###", "" + AppLogic.AppConfigs("LIVE_SERVER") + "/Redirect.aspx?Custid=" + Server.UrlEncode(SecurityComponent.Encrypt((Session["CustID"].ToString()))) + "", RegexOptions.IgnoreCase);
                //    strBody = Regex.Replace(strBody, "###RURL###", "" + AppLogic.AppConfigs("LIVE_SERVER") + "", RegexOptions.IgnoreCase);
                //    //sbEmail.Append("<a href='" + AppLogic.AppConfigs("LIVE_SERVER") + "/Redirect.aspx?Custid=" + Server.UrlEncode(SecurityComponent.Encrypt((Session["CustID"].ToString()))) + "'>" + AppLogic.AppConfigs("LIVE_SERVER") + "</a>");
                //}
                //else
                //{
                strBody = Regex.Replace(strBody, "###CUSTID_LINK###", "" + rURL + "", RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###RURL###", "" + AppLogic.AppConfigs("STOREPATH") + "", RegexOptions.IgnoreCase);
                //sbEmail.Append("<a href='" + rURL + "'>" + rURL + "</a>");
                //}

                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }
    }
}