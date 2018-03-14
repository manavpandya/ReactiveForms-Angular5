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
    public partial class TellAFriend : System.Web.UI.Page
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
                txtname.Focus();
                txtMessage.Text = "I thought you would be interested in this item at " + AppLogic.AppConfigs("StoreName").ToString() + " Enjoy!";
                if (Request.Headers["referer"] != null)
                {
                    rURL = Convert.ToString(Request.Headers["referer"]).Trim();
                }
                ViewState["rURL"] = rURL.ToString();
                FillProductImage();
                txtname.Focus();
                Page.MaintainScrollPositionOnPostBack = true;
            }
        }

        /// <summary>
        /// Display Product Image and Name
        /// </summary>
        private void FillProductImage()
        {
            if (Request.QueryString["ProductID"] != null)
            {

                DataSet dsProductImage = new DataSet();

                dsProductImage = ProductComponent.GetproductImagename(Convert.ToInt32(Request.QueryString["ProductID"]));

                if (dsProductImage != null && dsProductImage.Tables.Count > 0 && dsProductImage.Tables[0].Rows.Count > 0)
                {
                    lblProductName.Text = Convert.ToString(dsProductImage.Tables[0].Rows[0]["Name"]);
                    if (Convert.ToString(dsProductImage.Tables[0].Rows[0]["ImageName"]) != "")
                    {
                        imgProduct.ImageUrl = AppLogic.AppConfigs("ImagePathProduct") + "icon/" + Convert.ToString(dsProductImage.Tables[0].Rows[0]["ImageName"]);
                    }
                    else
                    {
                        imgProduct.ImageUrl = AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg";
                    }
                }
            }
        }

        /// <summary>
        /// Clears the Fields
        /// </summary>
        private void ClearFields()
        {
            txtname.Text = "";
            txtEmail.Text = "";
            txtReEmail.Text = "";
            txtEmail1.Text = "";
            txtEmail2.Text = "";
            txtEmail3.Text = "";
            txtMessage.Text = "";
        }

        /// <summary>
        ///  Send Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSend_Click(object sender, ImageClickEventArgs e)
        {
            Regex reValidEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (txtname.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your name.');", true);
                txtname.Focus();
                return;
            }
            else if (txtEmail.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your email.');", true);
                txtEmail.Focus();
                return;
            }
            else if (txtEmail.Text != null && !reValidEmail.IsMatch(txtEmail.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid email.');", true);
                txtEmail.Focus();
                return;
            }
            else if (txtReEmail.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter re-enter email.');", true);
                txtReEmail.Focus();
                return;
            }
            else if (txtReEmail.Text != "" && !reValidEmail.IsMatch(txtReEmail.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid re-enter email.');", true);
                txtReEmail.Focus();
                return;
            }
            else if (txtEmail.Text.ToString().ToLower().Trim() != txtReEmail.Text.ToString().ToLower().Trim())
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Re-enter email must be same with your email.');", true);
                txtReEmail.Focus();
                return;
            }
            else if (txtEmail1.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Email1.');", true);
                txtEmail1.Focus();
                return;
            }
            else if (txtEmail1.Text != "" && !reValidEmail.IsMatch(txtEmail1.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid Email1.');", true);
                txtEmail1.Focus();
                return;
            }
            else if (txtEmail.Text.ToString().Trim() == txtEmail1.Text.ToString().Trim())
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
            ClearFields();
            txtname.Focus();
        }

        /// <summary>
        /// Send Email to Friends
        /// </summary>
        /// <param name="ToAddress">String ToAddress</param>
        private void SendEmail(String ToAddress)
        {
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();
            dsMailTemplate = objCustomer.GetEmailTamplate("TellAFriendProduct", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strSubject = Regex.Replace(strSubject, "###YOURNAME###", txtname.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                if (Request.QueryString["ProductID"] != null)
                {
                    strBody = Regex.Replace(strBody, "###YOURNAME###", txtname.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###OPTION###", " : " + txtMessage.Text.ToString().Trim() + "", RegexOptions.IgnoreCase);
                    if (ViewState["RURL"] != null)
                    {
                        rURL = ViewState["RURL"].ToString().Trim();
                    }
                }
                else
                {
                    strBody = Regex.Replace(strBody, "###YOURNAME###", "Your Friend " + txtname.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###OPTION###", "thinks you may like our products", RegexOptions.IgnoreCase);
                    if (!(rURL.ToLower().ToString().Contains("/c-")) && !(rURL.ToLower().ToString().Contains("/s-")) && !(rURL.ToLower().ToString().Contains("/p-")) && !(rURL.ToLower().ToString().Contains("/index.")) && !(rURL.ToLower().ToString().Contains("/sitemap")))
                        rURL = AppLogic.AppConfigs("LIVE_SERVER");
                }

                if (Session["CustID"] != null)
                {
                    strBody = Regex.Replace(strBody, "###CUSTID_LINK###", "" + rURL + "", RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###RURL###", "" + AppLogic.AppConfigs("STOREPATH") + "", RegexOptions.IgnoreCase);
                }
                else
                {
                    strBody = Regex.Replace(strBody, "###CUSTID_LINK###", "" + rURL + "", RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###RURL###", "" + AppLogic.AppConfigs("STOREPATH") + "", RegexOptions.IgnoreCase);
                }

                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }
    }
}