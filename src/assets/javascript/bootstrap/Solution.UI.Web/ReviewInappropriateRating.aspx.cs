using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Solution.Bussines.Components.Common;
using System.Net.Mail;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;

namespace Solution.UI.Web
{
    public partial class ReviewInappropriateRating : System.Web.UI.Page
    {
        #region Local Variables

        int RatingID = 0;

        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (Request.QueryString["RatingID"] != null && Convert.ToString(Request.QueryString["RatingID"]) != "")
            {
                RatingID = Convert.ToInt32(Request.QueryString["RatingID"]);
            }
            if (!IsPostBack)
            {

                txtName.Focus();
            }

            Page.Form.DefaultButton = btnSend.UniqueID;

        }

        protected void btnSend_Click(object sender, ImageClickEventArgs e)
        {

            Regex reValidEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (txtName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Name.');", true);
                txtName.Focus();
                return;
            }
            else if (txtEmail.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your email.');", true);
                txtEmail.Focus();
                return;
            }
            else if (!reValidEmail.IsMatch(txtEmail.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter valid email.');", true);
                txtEmail.Focus();
                return;
            }
            else if (txtLink.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Content Link.');", true);
                txtLink.Focus();
                return;
            }
            else if (txtMessage.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your messsage.');", true);
                txtMessage.Focus();
                return;
            }



            int isAdded = Convert.ToInt32(CommonComponent.GetScalarCommonData("INSERT INTO [dbo].[tb_ReviewInappropriateRaring] ([Name],[StoreID],[Email],[LinkContent],[Message],[CreatedOn]) " +
                                                         " VALUES('" + txtName.Text.Trim().Replace("'", "''") + "'," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + ",'" + txtEmail.Text.Trim().Replace("'", "''") + "','" + txtLink.Text.Trim().Replace("'", "''") + "','" + txtMessage.Text.Trim().Replace("'", "''") + "',getdate()); SELECT SCOPE_IDENTITY();"));

            if (isAdded > 0)
            {
                string MailAddress = AppLogic.AppConfigs("ContactMail_ToAddress");

                if (MailAddress != string.Empty)
                {
                    Int32 ReturnMailid = SendEmail(MailAddress);
                    if (ReturnMailid > 0)
                    {
                        txtName.Text = "";
                        txtEmail.Text = "";
                        txtLink.Text = "";
                        txtMessage.Text = "";
                        txtName.Focus();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('E-Mail has been sent successfully.');window.parent.disablePopup();", true);
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem while sending Inappropriate Rating Report.');", true);
                return;
            }
        }


        private Int32 SendEmail(String ToAddress)
        {
            try
            {
                int MailID = 0;
                CustomerComponent objCustomer = new CustomerComponent();
                ProductComponent objProduct = new ProductComponent();
                DataSet dsMailTemplate = new DataSet();
                dsMailTemplate = objCustomer.GetEmailTamplate("ReviewInappropriateRating", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {
                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();


                    strSubject = Regex.Replace(strSubject, "###NAME###", txtName.Text.ToString(), RegexOptions.IgnoreCase);
                    strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###NAME###", txtName.Text.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###EMAIL###", txtEmail.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###LINK###", txtLink.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###MESSAGE###", txtMessage.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                    MailID = CommonOperations.SendMailWithReplyTo(txtEmail.Text, ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                }
                return MailID;
            }
            catch
            {

            }
            return 1;

        }
    }
}