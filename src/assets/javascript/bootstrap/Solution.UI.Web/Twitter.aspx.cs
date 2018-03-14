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
using System.Xml;
using System.IO;

namespace Solution.UI.Web
{
    public partial class Twitter : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string url = "";
            string xml = "";
            oAuthTwitterComponent oAuth = new oAuthTwitterComponent();
            String _Name = "";
            string _EMail = "";

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

            if (Request["oauth_token"] == null)
            {
                oAuth.CallBackUrl = AppLogic.AppConfigs("LIVE_SERVER") + "/Twitter.aspx?RquestPageName=" + RquestPageName.ToString();
                Response.Redirect(oAuth.AuthorizationLinkGet());
            }
            else
            {
                oAuth.AccessTokenGet(Request["oauth_token"], Request["oauth_verifier"]);

                if (oAuth.TokenSecret.Length > 0)
                {
                    //url = "https://twitter.com/account/verify_credentials.xml";
                    url = "https://api.twitter.com/1/account/verify_credentials.xml";
                    xml = oAuth.oAuthWebRequest(oAuthTwitterComponent.Method.GET, url, String.Empty);
                    string Email = "";
                    string Name = "";
                    string id = "";

                    if (xml != null && xml != "")
                    {
                        DataSet ds = new DataSet();
                        XmlReader reader = XmlReader.Create(new StringReader(xml));
                        ds.ReadXml(reader);

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            if (ds.Tables[0].Rows[0]["screen_name"] != null)
                            {
                                Email = ds.Tables[0].Rows[0]["screen_name"].ToString();
                            }
                            if (ds.Tables[0].Rows[0]["screen_name"] != null)
                            {
                                Name = ds.Tables[0].Rows[0]["name"].ToString();
                            }
                            if (ds.Tables[0].Rows[0]["id"] != null)
                            {
                                id = ds.Tables[0].Rows[0]["id"].ToString();
                            }
                        }
                        _Name = Name;
                        _EMail = Email;

                        bool IsTwitt = false;

                        DataSet dsTwitter = new DataSet();
                        string CheckTwitter = "SELECT top 1 * FROM dbo.tb_Customer WHERE TwitterID='" + id + "'  AND TwitterScreen='" + Email + "'";
                        dsTwitter = CommonComponent.GetCommonDataSet(CheckTwitter);
                        if (dsTwitter != null && dsTwitter.Tables.Count > 0 && dsTwitter.Tables[0].Rows.Count > 0)
                        {
                            if (dsTwitter.Tables[0].Rows[0]["Email"] != null)
                            {
                                _EMail = dsTwitter.Tables[0].Rows[0]["Email"].ToString();
                                IsTwitt = true;
                            }
                        }

                        #region Common Code

                        String strSql = "";
                        DataSet DsCustomer = new DataSet();
                        CustomerComponent ObjCustomer = new CustomerComponent();
                        if (IsTwitt == true)
                        {
                            strSql = "select CustomerID,FirstName,LastName,Email from tb_Customer where TwitterID='" + id + "'  AND TwitterScreen='" + Email + "'";
                        }
                        else
                        {
                            strSql = "select CustomerID,FirstName,LastName,Email from tb_Customer where email like '" + _EMail.ToString().Trim() + "@%'";
                        }
                        DsCustomer = CommonComponent.GetCommonDataSet(strSql);

                        if (DsCustomer != null && DsCustomer.Tables.Count > 0 && DsCustomer.Tables[0].Rows.Count > 0)
                        {
                            bool flag = CommonOperations.RegisterCart(Convert.ToInt32(DsCustomer.Tables[0].Rows[0]["CustomerID"]), false);
                            Session["UserName"] = Convert.ToString(DsCustomer.Tables[0].Rows[0]["Email"]);
                            Session["FirstName"] = Convert.ToString(DsCustomer.Tables[0].Rows[0]["FirstName"]);
                            Session["LastName"] = Convert.ToString(DsCustomer.Tables[0].Rows[0]["lastName"]);
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
                                Response.Redirect("/Createaccount.aspx?Mode=edit");
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

                            strSql = "insert into tb_customer (firstname,email,password,createdon,StoreID,active,deleted,isFBUser,IsRegistered,TwitterID,TwitterScreen) values ('"
                                + _Name.ToString().Trim() + "','" + _EMail.ToString().Trim() + "','" + SecurityComponent.Encrypt(strPassword) + "',getdate()," + AppLogic.AppConfigs("StoreID") + ",1,0,1,1, '" + id + "','" + Email + "')";
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
                                Session["LastName"] = Convert.ToString(DsCustomer.Tables[0].Rows[0]["lastName"]);
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
                                    //SendMail(first_name.ToString().Trim(), last_name.ToString().Trim(), strPassword, _EMail.ToString());
                                }
                                CustomerComponent objcust = new CustomerComponent();
                                if (objcust.GetBlankDetailsofCustomer(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreId"))) == 0)
                                    Response.Redirect("/Createaccount.aspx?Mode=edit");
                                else
                                {
                                    if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                                        Response.Redirect("/AddToCart.aspx");
                                    else
                                        Response.Redirect("index.aspx");
                                }
                            }
                        }
                        #endregion
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