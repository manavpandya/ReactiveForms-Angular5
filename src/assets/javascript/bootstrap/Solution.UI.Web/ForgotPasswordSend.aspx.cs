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
    public partial class ForgotPasswordSend : System.Web.UI.Page
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
            CommonOperations.RedirectWithSSL(false);
            lblEmail.Focus();
            if (Request.QueryString["Email"] != null)
            {
               // FillOlduserData(Request.QueryString["Email"].ToString());
            }
            else
            {
                //if (Session["CustID"] == null)
                //{
                //    Response.Redirect("/Login.aspx");
                //    return;
                //}

                ltbrTitle.Text = "Forgot Password";
                ltTitle.Text = "Forgot Password";

                if (!IsPostBack)
                {
                    Session["PageName"] = "Forgot Password";

                    if (Session["CustID"] != null && Session["CustID"].ToString().Trim().Length > 0)
                    {
                      //  FillData(Convert.ToInt32(Session["CustID"].ToString()));
                    }
                   // txtshowcode.Focus();
                }
            }
        }


        private void FillOlduserData(String Email)
        {
            string EmailDecrypt = Server.UrlDecode(SecurityComponent.Decrypt(Email));
            lblEmail.Text = EmailDecrypt;

            DataSet dsCust = CommonComponent.GetCommonDataSet("Select * from tb_Customer Where Email='" + lblEmail.Text + "' and IsRegistered=1 and storeid =1");
            if (dsCust != null && dsCust.Tables.Count > 0 && dsCust.Tables[0].Rows.Count > 0)
            {
                lblEmail.Text = dsCust.Tables[0].Rows[0]["Email"].ToString();
                ViewState["FName"] = dsCust.Tables[0].Rows[0]["FirstName"].ToString();
                ViewState["LName"] = dsCust.Tables[0].Rows[0]["LastName"].ToString();

                if (dsCust.Tables[0].Rows[0]["Password"] != DBNull.Value && Convert.ToString(dsCust.Tables[0].Rows[0]["Password"]) != "")
                {
                    ViewState["Password"] = dsCust.Tables[0].Rows[0]["Password"].ToString();
                }
                else
                {
                    string newpassword = SecurityComponent.Encrypt(GenerateRandomCode().ToString());
                    ViewState["Password"] = newpassword;
                }
            }
        }

        /// <summary>
        /// Fill Email details and Store Password and First Name and Last Name in ViewState
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
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
                ViewState["Password"] = DsCustomerdata.Tables[0].Rows[0]["Password"].ToString();
            }
        }

        /// <summary>
        /// Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if (txtshowcode.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Shown Code.');", true);
                txtshowcode.Focus();
                return;
            }
            else if (txtshowcode.Text != Convert.ToString(Session["CaptchaImageText"]))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Correct Shown Code.');", true);
                txtshowcode.Focus();
                return;
            }
            //DataSet dsCust = CommonComponent.GetCommonDataSet("Select * from tb_Customer Where Email='" + lblEmail.Text + "'");
            //if (dsCust != null && dsCust.Tables.Count > 0 && dsCust.Tables[0].Rows.Count > 0)
            //{
            //    if (Request.QueryString["Email"] != null)
            //    {

            //        MatchOldUser(lblEmail.Text);
            //    }
            //}
            //else
            //{
            SendMail();
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert(Your Password has been sent to your E-Mail Address.');", true);
           // ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "checkEmailAd();", true);
            //  Response.Redirect("/MyAccount.aspx");
            // }
        }

        /// <summary>
        /// Send Mail to the Customer which Contains Password 
        /// </summary>
        private void SendMail()
        {
            objCustomer = new CustomerComponent();
            DataSet dsForgotPwd = new DataSet();
            DataSet dsMailTemplate = new DataSet();
            dsForgotPwd = CommonComponent.GetCommonDataSet("Select * from tb_Customer Where Email='" + lblEmail.Text + "' and Storeid =1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and Isnull(IsRegistered,0) =1 ");
                //objCustomer.GetPasswordForForgotPassWord(lblEmail.Text.ToString().Trim(),1);
            dsMailTemplate = objCustomer.GetEmailTamplate("Forgotpassword", 1);


            if (dsForgotPwd != null && dsForgotPwd.Tables.Count > 0 && dsForgotPwd.Tables[0].Rows.Count > 0)
            {

                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {
                    Int32 CustomerID = 0;

                    CustomerID = Convert.ToInt32(dsForgotPwd.Tables[0].Rows[0]["CustomerID"].ToString());

                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                    strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###FIRSTNAME###", dsForgotPwd.Tables[0].Rows[0]["FirstName"].ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LASTNAME###", dsForgotPwd.Tables[0].Rows[0]["LastName"].ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###USERNAME###", Convert.ToString(lblEmail.Text.ToString()), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                    if (!String.IsNullOrEmpty(dsForgotPwd.Tables[0].Rows[0]["Password"].ToString()))
                    {
                      strBody = Regex.Replace(strBody, "###PASSWORD###", SecurityComponent.Decrypt(dsForgotPwd.Tables[0].Rows[0]["Password"].ToString()), RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        String NewPass = GenerateRandomCode();
                        CommonComponent.ExecuteCommonData("update tb_customer set password ='" +SecurityComponent.Encrypt(NewPass) + "' where CustomerId =" + CustomerID);
                        strBody = Regex.Replace(strBody, "###PASSWORD###", NewPass, RegexOptions.IgnoreCase);

                    }
                    

                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                    CommonOperations.SendMail(lblEmail.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Your Password has been sent to your E-Mail Address.');window.location.href=window.location.href;", true);
                }
                else
                {

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Invalid E-Mail Address. Please verify it.');", true);
                    lblEmail.Focus();
                    return;

                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msga", "alert('This E-Mail Address is not registered with us.');", true);
                lblEmail.Focus();
                return;
            }
        }

        #region Olduser
        public void MatchOldUser(string Email)
        {
            DataSet dsCust = CommonComponent.GetCommonDataSet("Select * from tb_Customer_Temp Where Email='" + Email + "'");
            if (dsCust != null && dsCust.Tables.Count > 0 && dsCust.Tables[0].Rows.Count > 0)
            {
                #region Customer In

                int CustomerID = 0;
                bool IsCustomerAdded = false;
                CustomerComponent objCustomer = new CustomerComponent();
                tb_Customer tb_Customer = new tb_Customer();
                tb_Customer.Email = dsCust.Tables[0].Rows[0]["Email"].ToString();
                string newpassword = SecurityComponent.Encrypt(GenerateRandomCode().ToString());
                tb_Customer.Password = newpassword;
                ViewState["Password"] = newpassword;
                tb_Customer.FirstName = dsCust.Tables[0].Rows[0]["FirstName"].ToString();
                tb_Customer.LastName = dsCust.Tables[0].Rows[0]["LastName"].ToString();
                tb_Customer.IsRegistered = true;
                tb_Customer.IsLockedOut = false;
                tb_Customer.FailedPasswordAttemptCount = 0;
                tb_Customer.LastIPAddress = Request.UserHostAddress.ToString();
                tb_Customer.Active = true;
                tb_Customer.Deleted = false;
                tb_Customer.CreatedOn = Convert.ToDateTime(dsCust.Tables[0].Rows[0]["CreatedOn"].ToString());
                tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreId")));

                CustomerID = objCustomer.InsertCustomer(tb_Customer, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                //make yahoouser = 1
                if (CustomerID != -1)
                {
                    CommonComponent.ExecuteCommonData("update tb_Customer set YahooUser=1 where CustomerID=" + CustomerID + "");
                    System.Web.HttpCookie custCookie = new System.Web.HttpCookie("ecommcustomer", CustomerID.ToString());
                    custCookie.Expires = DateTime.Now.AddYears(1);
                    Response.Cookies.Add(custCookie);

                    if (InsertBillAddress(CustomerID, dsCust.Tables[0].Rows[0]["BillingAddressID"].ToString()) && InsertShippAddress(CustomerID, dsCust.Tables[0].Rows[0]["ShippingAddressID"].ToString()))
                    {
                        IsCustomerAdded = true;
                    }


                    //if (IsCustomerAdded)
                    //{
                    CommonComponent.ExecuteCommonData("delete from tb_Customer_Temp where Email='" + lblEmail.Text + "'");

                    //CommonOperations.RegisterCart(CustomerID, false);
                    //Session["CustID"] = CustomerID.ToString();
                    //Session["UserName"] = dsCust.Tables[0].Rows[0]["Email"].ToString();
                    //Session["IsAnonymous"] = "false";
                    //Session["FirstName"] = dsCust.Tables[0].Rows[0]["FirstName"].ToString();

                    SendMailforolduser();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "checkEmailAd();", true);

                    //ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "checkEmailAd();", true);


                    //}
                    //else
                    //{
                    //    //  Page.ClientScript.RegisterStartupScript(Page.GetType(), "Exists", "alert('Problem while Creating Account');", true);
                    //    return;
                    //}
                }
                else
                {
                    //  Page.ClientScript.RegisterStartupScript(Page.GetType(), "Exists", "alert('Customer already exists with same email address please select different email address.');", true);
                    return;
                }

                #endregion
            }
        }

        /// <summary>
        /// Insert Billing Address
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <returns>Returns Boolean True= Inserted and False=Not inserted</returns>
        public bool InsertBillAddress(Int32 CustID, string tempaddressid)
        {
            bool IsBillAddressInserted = false;

            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsAddress = CommonComponent.GetCommonDataSet("Select * from tb_Address_Temp Where AddressID=" + tempaddressid + "");
            if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[0].Rows.Count > 0)
            {

                tb_Address.CustomerID = CustID;
                tb_Address.FirstName = dsAddress.Tables[0].Rows[0]["FirstName"].ToString();
                tb_Address.LastName = dsAddress.Tables[0].Rows[0]["LastName"].ToString();

                tb_Address.Address1 = dsAddress.Tables[0].Rows[0]["Address1"].ToString();
                tb_Address.Address2 = dsAddress.Tables[0].Rows[0]["Address2"].ToString();
                tb_Address.City = dsAddress.Tables[0].Rows[0]["City"].ToString();
                tb_Address.Company = dsAddress.Tables[0].Rows[0]["Company"].ToString();

                tb_Address.State = dsAddress.Tables[0].Rows[0]["State"].ToString();

                tb_Address.Suite = dsAddress.Tables[0].Rows[0]["Suite"].ToString();
                tb_Address.ZipCode = dsAddress.Tables[0].Rows[0]["ZipCode"].ToString();
                if (dsAddress.Tables[0].Rows[0]["Country"].ToString() != "")
                {
                    tb_Address.Country = Convert.ToInt32(dsAddress.Tables[0].Rows[0]["Country"].ToString());
                }
                tb_Address.Phone = dsAddress.Tables[0].Rows[0]["Phone"].ToString();
                tb_Address.Fax = dsAddress.Tables[0].Rows[0]["Fax"].ToString();
                tb_Address.Email = dsAddress.Tables[0].Rows[0]["Email"].ToString();
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.AddressType = 0;
                tb_Address.CreatedOn = Convert.ToDateTime(dsAddress.Tables[0].Rows[0]["CreatedOn"].ToString());
                tb_Address.Deleted = false;
                int isadded = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (isadded > 0)
                {
                    IsBillAddressInserted = objCustomer.UpdateCustomerBillingAddress(CustID, isadded);
                    CommonComponent.ExecuteCommonData("delete from tb_Address_Temp Where AddressID=" + tempaddressid + "");
                }
                else
                {
                    //   Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding billing address, please try again...');", true);
                    IsBillAddressInserted = false;
                }

            }

            return IsBillAddressInserted;
        }

        /// <summary>
        /// Insert Shipping Address
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <returns>Returns Boolean True= Inserted and False=Not inserted</returns>
        public bool InsertShippAddress(Int32 CustID, string tempaddressid)
        {
            bool IsShippAddressInserted = false;

            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsAddress = CommonComponent.GetCommonDataSet("Select * from tb_Address_Temp Where AddressID=" + tempaddressid + "");
            if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[0].Rows.Count > 0)
            {
                tb_Address.CustomerID = CustID;
                tb_Address.FirstName = dsAddress.Tables[0].Rows[0]["FirstName"].ToString();
                tb_Address.LastName = dsAddress.Tables[0].Rows[0]["LastName"].ToString();
                tb_Address.Address1 = dsAddress.Tables[0].Rows[0]["Address1"].ToString();
                tb_Address.Address2 = dsAddress.Tables[0].Rows[0]["Address2"].ToString();
                tb_Address.City = dsAddress.Tables[0].Rows[0]["City"].ToString();
                tb_Address.Company = dsAddress.Tables[0].Rows[0]["Company"].ToString();
                tb_Address.State = dsAddress.Tables[0].Rows[0]["State"].ToString();
                tb_Address.Suite = dsAddress.Tables[0].Rows[0]["Suite"].ToString();
                tb_Address.ZipCode = dsAddress.Tables[0].Rows[0]["ZipCode"].ToString();
                if (dsAddress.Tables[0].Rows[0]["Country"].ToString() != "")
                {
                    tb_Address.Country = Convert.ToInt32(dsAddress.Tables[0].Rows[0]["Country"].ToString());
                }
                tb_Address.Phone = dsAddress.Tables[0].Rows[0]["Phone"].ToString();
                tb_Address.Fax = dsAddress.Tables[0].Rows[0]["Fax"].ToString();
                tb_Address.Email = dsAddress.Tables[0].Rows[0]["Email"].ToString();
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.AddressType = 0;
                tb_Address.CreatedOn = Convert.ToDateTime(dsAddress.Tables[0].Rows[0]["CreatedOn"].ToString());
                tb_Address.Deleted = false;
                int isadded = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (isadded > 0)
                {
                    IsShippAddressInserted = objCustomer.UpdateCustomerShippingAddress(CustID, isadded);
                    CommonComponent.ExecuteCommonData("delete from tb_Address_Temp Where AddressID=" + tempaddressid + "");
                }
                else
                {
                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding shipping address, please try again...');", true);
                    IsShippAddressInserted = false;
                }

            }
            return IsShippAddressInserted;
        }

        /// <summary>
        /// Send Mail function
        /// </summary>
        private void SendMailforolduser()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsMailTemplate = new DataSet();

            dsMailTemplate = objCustomer.GetEmailTamplate("Forgotpassword", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList("InvoiceSignature", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            DataSet dsCust = CommonComponent.GetCommonDataSet("Select * from tb_Customer Where Email='" + lblEmail.Text + "'");
            if (dsCust != null && dsCust.Tables.Count > 0 && dsCust.Tables[0].Rows.Count > 0)
            {

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

                    strBody = Regex.Replace(strBody, "###FIRSTNAME###", Convert.ToString(dsCust.Tables[0].Rows[0]["FirstName"].ToString()), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LASTNAME###", Convert.ToString(dsCust.Tables[0].Rows[0]["LastName"].ToString()), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###USERNAME###", Convert.ToString(lblEmail.Text), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###PASSWORD###", SecurityComponent.Decrypt(Convert.ToString(ViewState["Password"])), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                    CommonOperations.SendMail(lblEmail.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                }
            }
        }

        /// <summary>
        /// Generate random password
        /// </summary>
        /// <returns>Returns the Randomized Generated Password </returns>
        private string GenerateRandomCode()
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Login.aspx");
        }


        #endregion

        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            txtshowcode.Text = "";
            lblEmail.Text = "";
            lblEmail.Focus();
        }

    }
}