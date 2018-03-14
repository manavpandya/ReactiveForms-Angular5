using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Solution.UI.Web
{
    public partial class changepassword : System.Web.UI.Page
    {
        #region Local Variables

        CustomerComponent objCustomer = null;

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CustID"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }
            CommonOperations.RedirectWithSSL(false);
            if (!IsPostBack)
            {
                Session["PageName"] = "Change Password";
                if (Session["CustID"] != null && Session["CustID"].ToString().Trim().Length > 0)
                {
                    FillData(Convert.ToInt32(Session["CustID"].ToString()));
                }
                txtOldPassword.Focus();
            }
            ltbrTitle.Text = "Change Password";
            ltTitle.Text = "Change Password";
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtOldPassword.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Old Password.');", true);
                txtOldPassword.Focus();
                return;
            }

            if (SecurityComponent.Encrypt(txtOldPassword.Text.ToString().Trim()) != Convert.ToString(ViewState["OldPasssword"]).Trim())
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter correct Old Password.');", true);
                txtOldPassword.Focus();
                return;
            }
            if (txtpassword.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Password.');", true);
                txtpassword.Focus();
                return;
            }
            else if (txtpassword.Text.Length < 6)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Password length must be at least 6 character long.');", true);
                txtpassword.Focus();
                return;
            }
            else if (txtconfirmpassword.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please enter Confirm Password.');", true);
                txtconfirmpassword.Focus();
                return;
            }
            else if (txtpassword.Text != txtconfirmpassword.Text)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "  alert('Confirm Password must be match with Password.');", true);
                txtconfirmpassword.Focus();
                return;
            }


            objCustomer = new CustomerComponent();
            bool isUpdate = false;
            tb_Customer tb_Customer = new tb_Customer();
            tb_Customer.Password = SecurityComponent.Encrypt(txtpassword.Text);
            tb_Customer.CustomerID = Convert.ToInt32(Session["CustID"].ToString().Trim());
            tb_Customer.LastIPAddress = Request.UserHostAddress.ToString();
            isUpdate = objCustomer.UpdatePasswordByCustomerID(tb_Customer);

            if (isUpdate)
            {
                SendMail();
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Password updated successfully and Your Password has been sent to your E-Mail Address.');", true);

                Response.Redirect("/MyAccount.aspx?ChangePasswordStatus=true");
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "  alert('Problem while updating Password.');", true);
                return;
            }

        }

        /// <summary>
        /// Fill Email Details and Store Password in ViewState
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        private void FillData(Int32 CustomerID)
        {
            objCustomer = new CustomerComponent();

            DataSet DsCustomerdata = new DataSet();
            DsCustomerdata = objCustomer.GetCustomerDetailByCustID(CustomerID);

            if (DsCustomerdata != null && DsCustomerdata.Tables.Count > 0 && DsCustomerdata.Tables[0].Rows.Count > 0)
            {
                lblEmail.Text = DsCustomerdata.Tables[0].Rows[0]["Email"].ToString();
                ViewState["FName"] = DsCustomerdata.Tables[0].Rows[0]["FirstName"].ToString();
                ViewState["LName"] = DsCustomerdata.Tables[0].Rows[0]["LastName"].ToString();
                ViewState["OldPasssword"] = DsCustomerdata.Tables[0].Rows[0]["Password"].ToString();
            }
        }

        /// <summary>
        /// Send Mail function
        /// </summary>
        private void SendMail()
        {
            objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();

            dsMailTemplate = objCustomer.GetEmailTamplate("ChangePassword", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList("InvoiceSignature", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {

                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###FIRSTNAME###", Convert.ToString(ViewState["FName"]), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LASTNAME###", Convert.ToString(ViewState["LName"]), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###USERNAME###", Convert.ToString(lblEmail.Text.ToString()), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###PASSWORD###", txtpassword.Text, RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    strBody = Regex.Replace(strBody, "###SIGNATURE###", Convert.ToString(dsTopic.Tables[0].Rows[0]["Description"]), RegexOptions.IgnoreCase);
                }
                else
                {
                    strBody = Regex.Replace(strBody, "###SIGNATURE###", "Thank You", RegexOptions.IgnoreCase);
                }
                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(lblEmail.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }

        /// <summary>
        /// Pre Render Event for Password
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtpassword_PreRender(object sender, EventArgs e)
        {
            txtpassword.Attributes.Add("value", txtpassword.Text);
        }

        /// <summary>
        /// Pre Render Event for Old Password
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtOldPassword_PreRender(object sender, EventArgs e)
        {
            txtOldPassword.Attributes.Add("value", txtOldPassword.Text);
        }

        /// <summary>
        /// Pre Render Event for Confirm Password
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtconfirmpassword_PreRender(object sender, EventArgs e)
        {
            txtconfirmpassword.Attributes.Add("value", txtconfirmpassword.Text);
        }
    }
}