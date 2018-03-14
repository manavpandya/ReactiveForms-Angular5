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
    public partial class AvailabilityNotification : System.Web.UI.Page
    {
        #region Local Variables

        int ProductID = 0;

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            if (Request.QueryString["ProductID"] != null && Convert.ToString(Request.QueryString["ProductID"]) != "")
            {
                ProductID = Convert.ToInt32(Request.QueryString["ProductID"]);
            }
            if (!IsPostBack)
            {

            }

            Page.Form.DefaultButton = btnSend.UniqueID;
        }

        /// <summary>
        /// Sending Availability Notification Email
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnSend_Click(object sender, ImageClickEventArgs e)
        {
            tb_AvailabilityNotification tb_Avalability = null;
            Regex reValidEmail = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");

            if (txtFirstName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter First Name.');", true);
                txtFirstName.Focus();
                return;
            }
            else if (txtLastName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your email.');", true);
                txtLastName.Focus();
                return;
            }
            else if (txttelephone.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter your email.');", true);
                txttelephone.Focus();
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

            tb_Avalability = new tb_AvailabilityNotification();
            MailLogComponent objMailLog = new MailLogComponent();
            tb_Avalability.FirstName = txtFirstName.Text;
            tb_Avalability.LastName = txtLastName.Text;
            tb_Avalability.Email = txtEmail.Text;
            tb_Avalability.Telephone = Convert.ToString(txttelephone.Text);
            int isAdded = objMailLog.InsertAvailabilityNotification(tb_Avalability, ProductID);

            if (isAdded > 0)
            {
                string MailAddress = AppLogic.AppConfigs("ContactMail_ToAddress");

                if (MailAddress != string.Empty)
                {
                    Int32 ReturnMailid = SendEmail(MailAddress);
                    if (ReturnMailid > 0)
                    {
                        tb_Avalability = new tb_AvailabilityNotification();
                        tb_Avalability.AvailabilityID = isAdded;
                        tb_Avalability.MailID = ReturnMailid;
                        tb_Avalability.MailSent = true;
                        objMailLog.UpdateAvailabilityNotification(tb_Avalability);
                        txtFirstName.Text = "";
                        txtLastName.Text = "";
                        txtEmail.Text = "";
                        txttelephone.Text = "";
                        txtFirstName.Focus();
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('E-Mail has been sent successfully. Our Customer Service Representative will respond to you by phone or E-Mail within 24 business hours.');", true);
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem while sending Availability Notification.');", true);
                return;
            }
        }

        /// <summary>
        /// Send Email function
        /// </summary>
        /// <param name="ToAddress">String ToAddress</param>
        private Int32 SendEmail(String ToAddress)
        {
            int MailID = 0;
            CustomerComponent objCustomer = new CustomerComponent();
            ProductComponent objProduct = new ProductComponent();
            DataSet dsMailTemplate = new DataSet();
            dsMailTemplate = objCustomer.GetEmailTamplate("AvailabilityNotification", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            tb_Product tb_product = new tb_Product();
            tb_product = objProduct.GetAllProductDetailsbyProductID(ProductID);
            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();


                strSubject = Regex.Replace(strSubject, "###FIRSTNAME###", txtFirstName.Text + " " + txtLastName.Text, RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###FIRSTNAME###", txtFirstName.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LASTNAME###", txtLastName.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###EMAIL###", txtEmail.Text.ToString().Trim(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###TELEPHONE###", txttelephone.Text.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PRODUCTLINK###", "/" + tb_product.MainCategory.ToString().Trim() + "/" + tb_product.SEName.ToString().Trim() + "-" + tb_product.ProductID.ToString().Trim() + ".aspx", RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PRODUCTNAME###", tb_product.Name.ToString().Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###SKU###", Convert.ToString(tb_product.SKU).Trim(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                MailID = CommonOperations.SendMailWithReplyTo(txtEmail.Text, ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }

            return MailID;
        }
    }
}