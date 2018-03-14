using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.Common;
using System.Data;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using Solution.Data;

namespace Solution.UI.Web
{
    public partial class Login : System.Web.UI.Page
    {
        #region Local Variable

        CustomerComponent objCustomer = null;
        public string storePath;

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonOperations.RedirectWithSSL(true);
            storePath = "<b>" + AppLogic.AppConfigs("STOREPATH") + "</b>";
            Session["PagaeName"] = "Login";

           
            if (!IsPostBack)
            {
                if (Session["PaymentMethod"] == null)
                {
                    GuestCheckOut.HRef = "/AddTocart.aspx";
                }

                System.Web.HttpBrowserCapabilities browser = Request.Browser;
                if (browser.Browser.ToString().ToLower() == "chrome")
                {
                    txtpassword.Font.Size = FontUnit.Parse("16");
                }

                AppLogic.ApplicationStart();
                ltbrTitle.Text = "Login";
                ltTitle.Text = "Login";
                txtusername.Focus();

                int NoOfItems = 0;
                if (Session["NoOfCartItems"] != null)
                {
                    Int32.TryParse(Convert.ToString(Session["NoOfCartItems"]), out NoOfItems);
                }

                if (NoOfItems == 0)
                {
                    pnlGuest.Visible = false;
                    tdCreateAcc.Width = "66%";
                }
                else
                {
                    if (Request.QueryString["wishlist"] != null)
                    {
                        pnlGuest.Visible = false;
                        tdCreateAcc.Width = "66%";
                    }
                    else
                    {
                        pnlGuest.Visible = true;
                        tdCreateAcc.Width = "33%";
                    }

                }
            }
        }

        /// <summary>
        /// Login Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnLogin_Click(object sender, ImageClickEventArgs e)
        {
            MatchUserName();
        }

        /// <summary>
        /// Check the Validation for Login Page and MatchUserName for login and According that Reliable Message Display
        /// </summary>
        public void MatchUserName()
        {
            if (txtusername.Text != "" && txtpassword.Text != "")
            {
                objCustomer = new CustomerComponent();
                int CustID = 0;

                int CurrentCustomerID = 0;
                try
                {
                    CurrentCustomerID = objCustomer.GetCustIDByUserName(txtusername.Text.Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                }
                catch { }


                if (CurrentCustomerID != 0)
                {
                    DataSet dsCustomer = objCustomer.GetCustomerLockOut(CurrentCustomerID, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                    int CustomerTry = 0;
                    if (dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptCount"] != null && dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptCount"].ToString() != "")
                    {
                        CustomerTry = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptCount"]);
                    }

                    Double TimeOutValue = Convert.ToDouble(AppLogic.AppConfigs("LockoutTime"));

                    DateTime date = new DateTime();
                    if (dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptDate"] != null && dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptDate"].ToString() != "")
                    {
                        date = Convert.ToDateTime(dsCustomer.Tables[0].Rows[0]["FailedPasswordAttemptDate"]);
                    }
                    DateTime datenew = date.AddMinutes(TimeOutValue);

                    Int32 isLock = 0;
                    if (dsCustomer.Tables[0].Rows[0]["IsLockedOut"] != null && dsCustomer.Tables[0].Rows[0]["IsLockedOut"].ToString() != "")
                        isLock = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["IsLockedOut"]);

                    Int32 AllowedTry = 5;

                    DataSet dsGetCustomerDetail = new DataSet();
                    dsGetCustomerDetail = objCustomer.GetCustIDByUserNamePassword(txtusername.Text.Trim(), SecurityComponent.Encrypt(txtpassword.Text.ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                    if (dsGetCustomerDetail != null && dsGetCustomerDetail.Tables.Count > 0 && dsGetCustomerDetail.Tables[0].Rows.Count > 0)
                    {
                        CustID = Convert.ToInt32(dsGetCustomerDetail.Tables[0].Rows[0]["CustomerID"].ToString());
                    }
                    if (Convert.ToBoolean(isLock) && DateTime.Now < datenew)
                    {
                        objCustomer.UpdateCustomerLockOut(CurrentCustomerID, isLock, 0, date, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Your Account is Locked for " + TimeOutValue + " Minutes , Please try Later...');", true);
                        txtusername.Text = "";
                        txtpassword.Text = "";
                        txtusername.Focus();
                        return;
                    }
                    else if (CustID == 0)
                    {
                        CustomerTry++;

                        if (CustomerTry >= AllowedTry)
                        {
                            isLock = 1;
                            objCustomer.UpdateCustomerLockOut(CurrentCustomerID, isLock, 0, DateTime.Now, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Your Account is Locked for " + TimeOutValue + " Minutes , Please try Later...');", true);
                            return;
                        }
                        else
                        {
                            objCustomer.UpdateCustomerLockOut(CurrentCustomerID, isLock, CustomerTry, DateTime.Now, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        }

                        int remaingtrial = AllowedTry - (CustomerTry);
                        string trial = "";
                        if (Convert.ToBoolean(isLock))
                        {
                            trial = "1 trial is";
                        }
                        else
                        {
                            if (remaingtrial > 1)
                                trial = remaingtrial + " trials are";
                            else
                                trial = remaingtrial + " trial is";
                        }
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert(' Invalid User Name or Password. " + trial + " remaining, then your account will be locked.');", true);
                        txtpassword.Text = "";
                        txtpassword.Focus();
                        return;
                    }

                    else
                    {
                        bool flag = CommonOperations.RegisterCart(CustID, false);
                        Session["UserName"] = txtusername.Text.ToString();
                        Session["CustID"] = CustID;
                        Session["IsAnonymous"] = "false";
                        Session["FirstName"] = Convert.ToString(dsGetCustomerDetail.Tables[0].Rows[0]["FirstName"]);
                        Response.Cookies.Add(new System.Web.HttpCookie("ecommcustomer", null));//remove anonymous cookies.

                        objCustomer = new CustomerComponent();
                        isLock = 0;

                        objCustomer.UpdateCustomerLockOut(CustID, isLock, 0, DateTime.Now, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                        if (Request.QueryString["ReturnURL"] != null)
                        {
                            Response.Redirect(Request.QueryString["ReturnURL"].ToString());
                        }
                        if (flag)
                        {
                            if (Session["NoOfCartItems"] != null)
                            {
                                if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
                                {
                                    InsertWishListItems();
                                    Session["CouponCode"] = null;
                                    Session["CouponCodebycustomer"] = null;
                                    Session["CouponCodeDiscountPrice"] = null;
                                    Session["CustomerDiscount"] = null;
                                }

                                else if (Request.QueryString["wishlist"] != null)
                                {
                                    ShoppingCartComponent objWishlist = new ShoppingCartComponent();
                                    objWishlist.AddToWishList(Convert.ToInt32(Session["CustID"].ToString()));
                                    Session["CouponCode"] = null;
                                    Session["CouponCodebycustomer"] = null;
                                    Session["CouponCodeDiscountPrice"] = null;
                                    Session["CustomerDiscount"] = null;
                                    Response.Redirect("/Wishlist.aspx", true);
                                }
                                else
                                {
                                    if (Session["PaymentMethod"] != null)
                                    {
                                        Response.Redirect("CheckOutCommon.aspx", true);
                                    }
                                    else
                                    {
                                        Response.Redirect("/Addtocart.aspx", true);
                                    }
                                }
                            }
                            else
                            {
                                if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
                                {
                                    InsertWishListItems();
                                }
                                Response.Redirect("/MyAccount.aspx", true);
                            }
                        }
                        else
                        {
                            if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
                            {
                                InsertWishListItems();
                            }
                            Response.Redirect("/MyAccount.aspx", true);
                        }
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('User does not exist...');", true);
                    txtusername.Text = "";
                    txtpassword.Text = "";
                    txtusername.Focus();
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please enter User Name and password');", true);
                return;
            }
        }

        /// <summary>
        /// Insert WishList Item into WishList Table
        /// </summary>
        private void InsertWishListItems()
        {
            int IsAdded = 0;
            if (Session["WishListProduct"] != null)
            {
                DataTable dtProduct = new DataTable();
                dtProduct = (DataTable)Session["WishListProduct"];

                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    WishListItemsComponent objWishList = new WishListItemsComponent();
                    for (int i = 0; i < dtProduct.Rows.Count; i++)
                    {
                        IsAdded = objWishList.AddWishListItems(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(dtProduct.Rows[i]["ProductID"]), Convert.ToInt32(dtProduct.Rows[i]["Quantity"]), Convert.ToDecimal(dtProduct.Rows[i]["Price"]), Convert.ToString(dtProduct.Rows[i]["VariantNameId"]), Convert.ToString(dtProduct.Rows[i]["VariantValueId"]));

                    }
                    if (IsAdded > 0)
                    {
                        Session["WishListProduct"] = null;
                        Response.Redirect("WishList.aspx", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while adding product into wishlist.', 'Message');});", true);
                        return;
                    }
                }
            }
        }


        /// <summary>
        /// Create Account Button Click Event For Go on Create Account Page
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCreateAccount_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
            {
                Response.Redirect("CreateAccount.aspx?wishlist=2", true);
            }
            else if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "1")
            {
                Response.Redirect("CreateAccount.aspx?wishlist=1", true);
            }
            else
            {
                Response.Redirect("CreateAccount.aspx", true);
            }


        }

        /// <summary>
        /// Forgot Password Button Click Event for Forgot password box show and Login panel will be hide
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lkbForgetpwd_Click(object sender, EventArgs e)
        {
            pnlLogin.Visible = false;
            pnlForgotPassword.Visible = true;
            ltTitle.Text = "Forgot Password";
            ltbrTitle.Text = "Forgot Password";
            txtEmail.Focus();
        }

        /// <summary>
        /// Cancel Button Click Event for Login panel show and Forgot password box will be hide
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            pnlLogin.Visible = true;
            pnlForgotPassword.Visible = false;
            ltTitle.Text = "Login";
            ltbrTitle.Text = "Login";
            txtusername.Focus();
        }
        private string GenerateRandomCode()
        {
            Random random = new Random();
            string s = "";
            for (int i = 0; i < 6; i++)
                s = String.Concat(s, random.Next(10).ToString());
            return s;
        }
        /// <summary>
        /// Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            objCustomer = new CustomerComponent();
            DataSet dsForgotPwd = new DataSet();
            DataSet dsMailTemplate = new DataSet();
            //dsForgotPwd = objCustomer.GetPasswordForForgotPassWord(txtEmail.Text.ToString().Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            dsForgotPwd = CommonComponent.GetCommonDataSet("Select * from tb_Customer Where Email='" + txtEmail.Text.ToString().Trim() + "' and Storeid =1 and isnull(Deleted,0)=0 and isnull(Active,0)=1 and Isnull(IsRegistered,0) =1 ");
            dsMailTemplate = objCustomer.GetEmailTamplate("Forgotpassword", Convert.ToInt32(1));
            if (dsForgotPwd != null && dsForgotPwd.Tables.Count > 0 && dsForgotPwd.Tables[0].Rows.Count > 0)
            {
                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {

                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                    strSubject = Regex.Replace(strSubject, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###FIRSTNAME###", Convert.ToString(dsForgotPwd.Tables[0].Rows[0]["FirstName"]), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###LASTNAME###", Convert.ToString(dsForgotPwd.Tables[0].Rows[0]["LastName"]), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###USERNAME###", Convert.ToString(dsForgotPwd.Tables[0].Rows[0]["Email"]), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);


                    if (!String.IsNullOrEmpty(dsForgotPwd.Tables[0].Rows[0]["Password"].ToString()))
                    {
                        strBody = Regex.Replace(strBody, "###PASSWORD###", SecurityComponent.Decrypt(dsForgotPwd.Tables[0].Rows[0]["Password"].ToString()), RegexOptions.IgnoreCase);
                    }
                    else
                    {
                        Int32 CustomerID = 0;

                        CustomerID = Convert.ToInt32(dsForgotPwd.Tables[0].Rows[0]["CustomerID"].ToString());
                        String NewPass = GenerateRandomCode();
                        CommonComponent.ExecuteCommonData("update tb_customer set password ='" + SecurityComponent.Encrypt(NewPass) + "' where CustomerId =" + CustomerID);
                        strBody = Regex.Replace(strBody, "###PASSWORD###", NewPass, RegexOptions.IgnoreCase);

                    }

                   // strBody = Regex.Replace(strBody, "###PASSWORD###", SecurityComponent.Decrypt(Convert.ToString(dsForgotPwd.Tables[0].Rows[0]["Password"])), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                    CommonOperations.SendMail(txtEmail.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Your Password has been sent to your E-Mail Address.');", true);

                    txtEmail.Text = "";
                    int NoOfItems = 0;
                    ltTitle.Text = "Login";
                    ltbrTitle.Text = "Login";
                    if (Session["NoOfCartItems"] != null)
                    {
                        Int32.TryParse(Convert.ToString(Session["NoOfCartItems"]), out NoOfItems);
                    }

                    if (NoOfItems == 0)
                    {
                        pnlForgotPassword.Visible = false;
                        pnlGuest.Visible = false;
                        pnlLogin.Visible = true;
                        tdCreateAcc.Width = "66%";
                        txtusername.Focus();
                    }
                    else
                    {
                        pnlForgotPassword.Visible = false;
                        pnlGuest.Visible = true;
                        pnlLogin.Visible = true;
                        tdCreateAcc.Width = "33%";
                        txtusername.Focus();
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Invalid E-Mail Address. Please verify it.');", true);
                    txtEmail.Focus();
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msga", "alert('This E-Mail Address is not registered with us.');", true);
                txtEmail.Focus();
                return;
            }
        }
    }
}
