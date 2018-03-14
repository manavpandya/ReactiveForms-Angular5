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
using System.Text;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Solution.UI.Web
{
    public partial class Facebook : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = "";
            oAuthFacebookComponent oAuth = new oAuthFacebookComponent();

            String RquestPageName = "index.aspx";
            if (Request.QueryString["RquestPageName"] != null && Request.QueryString["RquestPageName"].ToString() != "" && !string.IsNullOrEmpty(Request.QueryString["RquestPageName"].ToString()) && Request.QueryString["RquestPageName"].ToString().Length > 0)
                RquestPageName = Request.QueryString["RquestPageName"].ToString();
            else
                RquestPageName = "index.aspx";

            if (RquestPageName.ToLower().Contains("facebook.aspx"))
            {
                RquestPageName = "index.aspx";
            }
            else if (RquestPageName.ToLower().Contains("login.aspx"))
            {
                RquestPageName = "CreateAccount.aspx";
            }
            if (Request["code"] == null)
            {
                //Redirect the user to Twitter for authorization.
                //Using oauth_callback for local testing.
                Response.Redirect(oAuth.AuthorizationLinkGet(AppLogic.AppConfigs("LIVE_SERVER") + "/Facebook.aspx?RquestPageName=" + Server.UrlEncode(RquestPageName.ToString())));
            }
            else
            {
                //Get the access token and secret.
                oAuth.AccessTokenGet(Request["code"], AppLogic.AppConfigs("LIVE_SERVER") + "/Facebook.aspx?RquestPageName=" + Server.UrlEncode(RquestPageName.ToString()));
                if (oAuth.Token.Length > 0)
                {
                    //We now have the credentials, so we can start making API calls
                    url = "https://graph.facebook.com/me?access_token=" + oAuth.Token;
                    string json = oAuth.WebRequest(oAuthFacebookComponent.Method.GET, url, String.Empty);

                    string[] StrProfile = json.Split(',');

                    if (StrProfile.Length > 0)
                    {
                        String first_name = "";
                        String last_name = "";
                        String _EMail = "";
                        for (int i = 0; i <= StrProfile.Length - 1; i++)
                        {
                            string[] strheader = StrProfile[i].ToString().Split(':');

                            if (strheader[0].ToString().Replace("\":", "").Replace("\"", "").ToLower() == "first_name")
                            {
                                first_name = strheader[1].ToString().Replace("\"", "");
                            }
                            if (strheader[0].ToString().Replace("\":", "").Replace("\"", "").ToLower() == "last_name")
                            {
                                last_name = strheader[1].ToString().Replace("\"", "");
                            }
                            if (strheader[0].ToString().Replace("\":", "").Replace("\"", "").ToLower() == "email")
                            {
                                _EMail = strheader[1].ToString().Replace("\"", "").Replace(@"\u0040", "@");
                            }
                        }
                        String strSql = "";
                        DataSet DsCustomer = new DataSet();
                        CustomerComponent ObjCustomer = new CustomerComponent();
                        DsCustomer = ObjCustomer.GetCustomerforFacebookByCustID(_EMail.ToString().Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreId")));

                        if (DsCustomer != null && DsCustomer.Tables.Count > 0 && DsCustomer.Tables[0].Rows.Count > 0)
                        {
                            bool flag = CommonOperations.RegisterCart(Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]), false);
                            Session["UserName"] = Convert.ToString(DsCustomer.Tables[0].Rows[0]["Email"]);
                            Session["FirstName"] = Convert.ToString(DsCustomer.Tables[0].Rows[0]["FirstName"]);
                            Session["CustID"] = Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]);

                            // Get Total Quantity Customer wise
                            DataSet dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
                            int Total = 0;
                            if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                            {
                                for (int i = 0; i < dsShoppingCart.Tables[0].Rows.Count; i++)
                                {
                                    Total += Convert.ToInt32(dsShoppingCart.Tables[0].Rows[i]["Qty"]);
                                }
                            }
                            Session["NoOfCartItems"] = Total;
                            CustomerComponent objcust = new CustomerComponent();
                            if (objcust.GetBlankDetailsofCustomer(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreId"))) == 0)
                                Response.Redirect("/Createaccount.aspx?type=Isfbuser&Mode=edit");
                            else
                            {
                                if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                                    Response.Redirect("/AddToCart.aspx");
                                else
                                    Response.Redirect("index.aspx");
                            }
                        }
                        else
                        {
                            String strPassword = GetPassword();

                            strSql = "insert into tb_customer (firstname,lastname,email,password,createdon,StoreID,active,deleted,isFBUser,IsRegistered) values ('"
                                + first_name.ToString().Trim() + "','" + last_name.ToString().Trim() + "','" + _EMail.ToString().Trim() + "','" + SecurityComponent.Encrypt(strPassword) + "',getdate()," + AppLogic.AppConfigs("StoreID") + ",1,0,1,1)";
                            CommonComponent.ExecuteCommonData(strSql);

                            DsCustomer = new DataSet();
                            DsCustomer = ObjCustomer.GetCustomerforFacebookByCustID(_EMail.ToString().Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
                            if (DsCustomer != null && DsCustomer.Tables.Count > 0 && DsCustomer.Tables[0].Rows.Count > 0)
                            {
                                Int32 AddressID = 0;
                                strSql = @"insert into tb_address (customerid,firstname,lastname,email,createdon,deleted,addresstype,StoreID) values 
                                                       (" + Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]) + ",'" + Convert.ToString(DsCustomer.Tables[0].Rows[0]["FirstName"]) + "','" + Convert.ToString(DsCustomer.Tables[0].Rows[0]["LastName"]) + "','" + Convert.ToString(DsCustomer.Tables[0].Rows[0]["Email"]) + "',getdate(),0,0," + AppLogic.AppConfigs("StoreID") + "); select @@identity";
                                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData(strSql)), out AddressID);

                                strSql = "update tb_customer set billingaddressid=" + AddressID + " where customerid=" + Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]);
                                CommonComponent.ExecuteCommonData(strSql);

                                strSql = @"insert into tb_address (customerid,firstname,lastname,email,createdon,deleted,addresstype,StoreID) values 
                                                       (" + Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]) + ",'" + Convert.ToString(DsCustomer.Tables[0].Rows[0]["FirstName"]) + "','" + Convert.ToString(DsCustomer.Tables[0].Rows[0]["LastName"]) + "','" + Convert.ToString(DsCustomer.Tables[0].Rows[0]["Email"]) + "',getdate(),0,1," + AppLogic.AppConfigs("StoreID") + "); select @@identity";
                                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData(strSql)), out AddressID);

                                strSql = "update tb_customer set BillingEqualShippingbit=0, shippingaddressid=" + AddressID + " where customerid=" + Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]);
                                CommonComponent.ExecuteCommonData(strSql);

                                bool flag = CommonOperations.RegisterCart(Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]), false);
                                Session["UserName"] = Convert.ToString(DsCustomer.Tables[0].Rows[0]["Email"]);
                                Session["FirstName"] = Convert.ToString(DsCustomer.Tables[0].Rows[0]["FirstName"]);
                                Session["CustID"] = Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]);

                                // Get Total Quentity Customer wise
                                DataSet dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
                                int Total = 0;
                                if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsShoppingCart.Tables[0].Rows.Count; i++)
                                    {
                                        Total += Convert.ToInt32(dsShoppingCart.Tables[0].Rows[i]["Qty"]);
                                    }
                                }

                                Session["NoOfCartItems"] = Total;
                                if (Convert.ToBoolean(AppLogic.AppConfigs("SendCustomerRegistrationMail")))
                                {
                                    SendMail(first_name.ToString().Trim(), last_name.ToString().Trim(), strPassword, _EMail.ToString());
                                }
                                CustomerComponent objcust = new CustomerComponent();
                                if (objcust.GetBlankDetailsofCustomer(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreId"))) == 0)
                                    Response.Redirect("/Createaccount.aspx?type=Isfbuser&Mode=edit");
                                else
                                {
                                    if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                                        Response.Redirect("/AddToCart.aspx");
                                    else
                                        Response.Redirect("index.aspx");
                                }
                            }
                        }
                    }
                }
            }

        }

        #region Generate Random Password

        /// <summary>
        /// Gets the Password from Generating Random
        /// </summary>
        /// <returns>Returns Randomly Generated Password</returns>
        public string GetPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(4, true));
            builder.Append(RandomNumber(1000, 9999));
            builder.Append(RandomString(2, false));
            return builder.ToString();
        }

        /// <summary>
        /// Generate Random Number
        /// </summary>
        /// <param name="min">int min</param>
        /// <param name="max">int max</param>
        /// <returns>Returns Random Number</returns>
        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        /// <summary>
        /// Generate Random String
        /// </summary>
        /// <param name="size">int size</param>
        /// <param name="lowerCase">bool lowerCase</param>
        /// <returns>Returns Randomly generated String</returns>
        private string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }

        #endregion

        /// <summary>
        ///  Send Mail after Creating Customer
        /// </summary>
        /// <param name="FName">String FName</param>
        /// <param name="LName">String LName</param>
        /// <param name="Password">String Password</param>
        /// <param name="UserEmail">String UserEmail</param>
        private void SendMail(String FName, String LName, String Password, String UserEmail)
        {
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsCreateAccount = new DataSet();
            dsCreateAccount = objCustomer.GetEmailTamplate("CreateAccount", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsCreateAccount != null && dsCreateAccount.Tables.Count > 0 && dsCreateAccount.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                String strSubject = "";
                strBody = dsCreateAccount.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsCreateAccount.Tables[0].Rows[0]["Subject"].ToString();

                strSubject = Regex.Replace(strSubject, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###FIRSTNAME###", FName.ToString() + ' ' + LName.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###USERNAME###", UserEmail.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PASSWORD###", Password.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(UserEmail.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }
    }
}