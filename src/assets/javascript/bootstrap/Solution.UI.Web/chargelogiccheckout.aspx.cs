using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text.RegularExpressions;


namespace Solution.UI.Web
{
    public partial class chargelogiccheckout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonOperations.RedirectWithSSL(true);

            //   clsCustomer ThisCustomer;
            ///iPrincipal1 = ((EcommPrincipal)Context.User);
            ///

            int custid = 0;
            int Orderid = 0;
            int CustomerQuoteID = 0;
            string HostedPaymentID = "";
            string ConfirmationID = "";
            string Token = "";
            if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Convert.ToInt32(Session["CustID"].ToString()) > 0)
            {
                custid = Convert.ToInt32(Session["CustID"].ToString());
            }
            if (Session["ONo"] != null && Session["ONo"].ToString() != "" && Convert.ToInt32(Session["ONo"].ToString()) > 0)
            {
                Orderid = Convert.ToInt32(Session["ONo"].ToString());
            }
            if (Session["CustomerQuoteID"] != null && Session["CustomerQuoteID"].ToString() != "" && Convert.ToInt32(Session["CustomerQuoteID"].ToString()) > 0)
            {
                CustomerQuoteID = Convert.ToInt32(Session["CustomerQuoteID"].ToString());
            }


            if (Request.QueryString["Action"] != null && Request.QueryString["Action"].ToString().ToLower() == "approve")
            {
                if (Request.QueryString["HostedPaymentID"] != null)
                {
                    HostedPaymentID = Request.QueryString["HostedPaymentID"].ToString();
                }
                if (Request.QueryString["ConfirmationID"] != null)
                {
                    ConfirmationID = Request.QueryString["ConfirmationID"].ToString();
                }
                if (Request.QueryString["Token"] != null)
                {
                    Token = Request.QueryString["Token"].ToString();
                }

            }

            if (custid != 0 && Orderid != 0)
            {

                {
                    String sql2 = "update tb_order set " +
                     "PaymentGateway='CREDITCARD',Deleted=0,TransactionStatus='CAPTURED',ChargeHostedPaymentID='" + HostedPaymentID + "',ChargeConfirmationID='" + ConfirmationID + "',ChargeToken='" + Token + "',CapturedOn=getdate() ," +
                     "AuthorizationResult='', " +
                     "AuthorizationCode='', " +
                     "AuthorizationPNREF='', " +
                     "TransactionCommand='', " +
                     "CaptureTxCommand='', " +
                     "CaptureTXResult='' where OrderNumber=" + Orderid.ToString();
                    CommonComponent.ExecuteCommonData(sql2);


                    CommonComponent.ExecuteCommonData("DELETE FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE CustomerId=" + custid + ")");
                    CommonComponent.ExecuteCommonData("DELETE FROM  tb_ShoppingCart WHERE CustomerId=" + custid + "");




                    #region Send Invoice Email to Customer

                    try
                    {
                        if (Session["Isphone"] == null)
                        {
                            if (Session["RepCartItems"] != null)
                            {
                                DataSet dsrep = new DataSet();
                                dsrep = (DataSet)Session["RepCartItems"];

                                if (dsrep != null && dsrep.Tables.Count > 0 && dsrep.Tables[0].Rows.Count > 0)
                                {
                                    int totalItemCount = dsrep.Tables[0].Rows.Count;
                                    for (int jj = 0; jj < totalItemCount; jj++)
                                    {

                                        //HiddenField hdnProductId = (HiddenField)RptCartItems.Items[i].FindControl("hdnProductId");
                                        //Label lblPrice = (Label)RptCartItems.Items[i].FindControl("lblPrice");
                                        //Label lblDiscountprice = (Label)RptCartItems.Items[i].FindControl("lblDiscountprice");
                                        //TextBox txtQty = (TextBox)RptCartItems.Items[i].FindControl("txtQty");
                                        //Literal ltrSubTotal = (Literal)RptCartItems.Items[i].FindControl("ltrSubTotal");
                                        Decimal Price = 0;
                                        Decimal.TryParse(dsrep.Tables[0].Rows[jj]["Price"].ToString(), out Price);


                                        Decimal DisPrice = 0;
                                        Decimal.TryParse(dsrep.Tables[0].Rows[jj]["DiscountPrice"].ToString(), out DisPrice);

                                        Int32 Productid = 0;
                                        Int32.TryParse(dsrep.Tables[0].Rows[jj]["Productid"].ToString(), out Productid);

                                        Decimal Subtotal = 0;
                                        Decimal.TryParse(dsrep.Tables[0].Rows[jj]["subtotal"].ToString(), out Subtotal);

                                        Int32 Qty = 0;
                                        Int32.TryParse(dsrep.Tables[0].Rows[jj]["Qty"].ToString(), out Qty);

                                        Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + Productid + " and ItemType='Swatch'"));
                                        if (Isorderswatch == 1)
                                        {
                                            try
                                            {
                                                //Decimal strpp = Convert.ToDecimal(lblPrice.Text.ToString().Replace("$", "").Trim());
                                                //Decimal strdiscount = Convert.ToDecimal(lblDiscountprice.Text.ToString().Replace("$", "").Trim());
                                                //Decimal decSubTotal = Convert.ToDecimal(ltrSubTotal.Text.ToString().Replace("$", "").Trim());
                                                if (Subtotal > Decimal.Zero)
                                                {
                                                    if (Subtotal == Price)
                                                    {
                                                        Price = Price / Convert.ToDecimal(Qty);
                                                    }
                                                }
                                                if (Subtotal > Decimal.Zero)
                                                {
                                                    if (Subtotal == DisPrice)
                                                    {
                                                        DisPrice = DisPrice / Convert.ToDecimal(Qty);
                                                    }
                                                }
                                                CommonComponent.ExecuteCommonData("UPDATE tb_OrderedShoppingCartItems SET Price='" + string.Format("{0:0.00}", Price) + "',DiscountPrice='" + string.Format("{0:0.00}", DisPrice) + "' WHERE RefProductid=" + Productid + " AND OrderedShoppingCartID in (SELECT ShoppingCardID FROM tb_order WHERE OrderNumber=" + Orderid + ")");
                                            }
                                            catch { }
                                        }
                                    }

                                }

                            }
                        }
                    }
                    catch { }

                    SendMail(Orderid);

                    #endregion


                    if (CustomerQuoteID > 0)
                    {

                        CommonComponent.ExecuteCommonData("update tb_customerquote set OrderNumber=" + Orderid + " where CustomerQuoteID="
                            + Convert.ToInt32(CustomerQuoteID));
                    }


                    if (Session["UserCreated"] != null && Session["UserCreated"].ToString() != "" && Session["UserCreated"].ToString().Trim() == "1")
                    {
                        CreateCustomerAndSendMail(Convert.ToInt32(Session["CustID"].ToString()), Orderid);
                    }


                    Session["PaymentGateway"] = null;
                    Session["PaymentGatewayStatus"] = null;
                    Session["SalesTax"] = null;
                    Session["CouponCode"] = null;
                    Session["Discount"] = null;
                    Session["PaymentMethod"] = null;
                    Session["ONo"] = Orderid.ToString();
                    Session["NoOfCartItems"] = null;
                    Session["SelectedCountry"] = null;
                    Session["SelectedZipCode"] = null;
                    Session["SelectedShippingMethod"] = null;

                    Session["CustomerDiscount"] = null;
                    Session["CustomerQuoteID"] = null;
                    Session["CouponCodebycustomer"] = null;
                    Session["CouponCodeDiscountPrice"] = null;
                    Session["Emailforshippingmethod"] = null;
                    Session["GiftCertificateDiscountCode"] = null;
                    Session["GiftCertificateDiscount"] = null;
                    Session["GiftCertificateRemaningBalance"] = null;
                    Session["RepCartItems"] = null;
                    int i = 0;
                    if (Session["UserCreated"] != null && Session["UserCreated"].ToString().Trim() == "1")
                    {
                        i = 1;
                    }
                    Session["UserCreated"] = null;
                    if (Session["Isphone"] != null && Session["Isphone"].ToString() == "1")
                    {
                        Session["Isphone"] = null;
                        Response.Redirect("/Admin/Orders/OrderList.aspx?Storeid=1");
                    }
                    else if (i == 1)
                    {
                        Response.Redirect("/orderreceived.aspx?UserCreated=true");
                    }
                    else
                    {
                        Response.Redirect("/orderreceived.aspx");
                    }
                }




            }
        }


        /// <summary>
        /// Send mail to Register with Password Customer
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        public void CreateCustomerAndSendMail(Int32 CustomerID, Int32 OrderNumber)
        {
            CustomerComponent objCustomer = new CustomerComponent();
            Int32 IsAdded = Convert.ToInt32(objCustomer.AddCustomerAfterorderPlaced(Convert.ToInt32(OrderNumber.ToString()), Session["user"].ToString(), SecurityComponent.Encrypt(Session["pass"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreId")), Convert.ToInt32(CustomerID)));
            if (IsAdded > 0)
            {
                objCustomer = new CustomerComponent();
                DataSet dsCreateAccount = new DataSet();
                dsCreateAccount = objCustomer.GetEmailTamplate("CreateAccount", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                //olution.Bussines.Entities.tb_Customer objCustData = objCustomer.GetCustomerDataByID(Convert.ToInt32(Session["CustID"]));
                string StrName = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(FirstName,'') + ' ' + ISNULL(LastName,'') as Name from tb_Customer where CustomerID =" + Convert.ToInt32(Session["CustID"]) + ""));
                if (dsCreateAccount != null && dsCreateAccount.Tables.Count > 0 && dsCreateAccount.Tables[0].Rows.Count > 0)
                {
                    String strBody = "";
                    String strSubject = "";
                    strBody = dsCreateAccount.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsCreateAccount.Tables[0].Rows[0]["Subject"].ToString();

                    if (strSubject.Contains("###LIVE_SERVER###"))
                    {
                        strSubject = Regex.Replace(strSubject, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                    }
                    strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                    if (strBody.Contains("###LIVE_SERVER###"))
                    {
                        strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###USERNAME###"))
                    {
                        strBody = Regex.Replace(strBody, "###USERNAME###", Session["user"].ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###PASSWORD###"))
                    {
                        strBody = Regex.Replace(strBody, "###PASSWORD###", Session["pass"].ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###YEAR###"))
                    {
                        strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###STOREPATH###"))
                    {
                        strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###FIRSTNAME###"))
                    {
                        //strBody = Regex.Replace(strBody, "###FIRSTNAME###", objCustData.FirstName.ToString() + " " + objCustData.LastName.ToString(), RegexOptions.IgnoreCase);
                        strBody = Regex.Replace(strBody, "###FIRSTNAME###", StrName.ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###STORENAME###"))
                    {
                        strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);
                    }
                    if (strBody.Contains("###StoreID###"))
                    {
                        strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                    }

                    try
                    {
                        AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                        CommonOperations.SendMail(Session["user"].ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                    }
                    catch { }
                }

                Response.Cookies.Add(new System.Web.HttpCookie("ecommcustomer", null));
                Session["user"] = "";
                Session["pass"] = "";
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Account has been created successfully.');", true);
            }
        }

        public String SQuote(String s)
        {
            int len = s.Length + 25;
            System.Text.StringBuilder tmpS = new System.Text.StringBuilder(len); // hopefully only one alloc
            tmpS.Append("N'");
            tmpS.Append(s.Replace("'", "''"));
            tmpS.Append("'");
            return tmpS.ToString();
        }

        /// <summary>
        /// Order Receipt Send To Customer & Admin
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNUmber</param>
        //public void SendMail(Int32 OrderNumber)
        //{
        //    try
        //    {
        //        string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
        //        string Body = "";
        //        //      string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + "invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
        //        string url = AppLogic.AppConfigs("LIVE_SERVER") + "/" + "Invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
        //        WebRequest NewWebReq = WebRequest.Create(url);
        //        WebResponse newWebRes = NewWebReq.GetResponse();
        //        string format = newWebRes.ContentType;
        //        Stream ftprespstrm = newWebRes.GetResponseStream();
        //        StreamReader reader;
        //        reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
        //        Body = reader.ReadToEnd().ToString();
        //        Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");

        //        Body = Body.Replace("title=\"Facebook\"", "title=\"Facebook\" style=\"display:none;\"");
        //        Body = Body.Replace("title=\"Twitter\"", "title=\"Twitter\" style=\"display:none;\"");
        //        Body = Body.Replace("title=\"Pinterest\"", "title=\"Pinterest\" style=\"display:none;\"");
        //        Body = Body.Replace("title=\"Google Plus\"", "title=\"Google Plus\" style=\"display:none;\"");
        //        Body = Body.Replace("id=\"trHeaderMenu\"", "id=\"trHeaderMenu\" style=\"display:none;\"");
        //        Body = Body.Replace("id=\"trStoreBanner\"", "id=\"trStoreBanner\" style=\"display:none;\"");


        //        AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

        //        try
        //        {
        //            CommonOperations.SendMail(ToID, "New Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonComponent.ExecuteCommonData("INSERT INTO tb_MailLog(Body,Storeid,CustomerId) VALUES ('" + ex.Message.ToString().Replace("'", "''") + " " + ex.StackTrace.ToString().Replace("'", "''") + "',1,113096)");
        //        }
        //        try
        //        {
        //            string StrCustEmail = Convert.ToString(CommonComponent.GetScalarCommonData("Select Email from tb_Order where OrderNumber='" + OrderNumber + "'"));


        //            if (StrCustEmail != null && StrCustEmail != "" && StrCustEmail.Length > 0)
        //            {
        //                CommonOperations.SendMail(StrCustEmail, "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
        //            }
        //            //  else
        //            //  {
        //            //  CommonOperations.SendMail(StrCustEmail, "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
        //            // }
        //        }
        //        catch (Exception ex)
        //        {
        //            CommonComponent.ExecuteCommonData("INSERT INTO tb_MailLog(Body,Storeid,CustomerId) VALUES ('" + ex.Message.ToString().Replace("'", "''") + " " + ex.StackTrace.ToString().Replace("'", "''") + "',1,113096)");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        CommonComponent.ExecuteCommonData("INSERT INTO tb_MailLog(Body,Storeid,CustomerId) VALUES ('" + ex.Message.ToString().Replace("'", "''") + " " + ex.StackTrace.ToString().Replace("'", "''") + "',1,113096)");
        //        //CommonComponent.ErrorLog(Request.Url.ToString(), ex.Message.ToString(), ex.StackTrace.ToString());
        //    }
        //}
        public void SendMail(Int32 OrderNumber)
        {
            try
            {
                string ToID = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(EmailID,'') FROM tb_ContactEmail WHERE Subject='Order Confirmation'"));
                if (string.IsNullOrEmpty(ToID))
                {
                    ToID = AppLogic.AppConfigs("MailMe_ToAddress");
                }


                string Body = "";
                //      string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + "invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                string url = AppLogic.AppConfigs("LIVE_SERVER") + "/" + "Invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
                WebRequest NewWebReq = WebRequest.Create(url);
                WebResponse newWebRes = NewWebReq.GetResponse();
                string format = newWebRes.ContentType;
                Stream ftprespstrm = newWebRes.GetResponseStream();
                StreamReader reader;
                reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                Body = reader.ReadToEnd().ToString();
                Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");

                Body = Body.Replace("title=\"Facebook\"", "title=\"Facebook\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Twitter\"", "title=\"Twitter\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Pinterest\"", "title=\"Pinterest\" style=\"display:none;\"");
                Body = Body.Replace("title=\"Google Plus\"", "title=\"Google Plus\" style=\"display:none;\"");
                Body = Body.Replace("id=\"trHeaderMenu\"", "id=\"trHeaderMenu\" style=\"display:none;\"");
                Body = Body.Replace("id=\"trStoreBanner\"", "id=\"trStoreBanner\" style=\"display:none;\"");


                AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

                try
                {
                    if (Session["Emailforshippingmethod"] != null && Session["Emailforshippingmethod"].ToString() != "")
                    {
                        ToID = ToID + ";" + Session["Emailforshippingmethod"].ToString();

                        CommonOperations.SendMail(ToID, "New Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                    }
                    else
                    {
                        //CommonOperations.SendMail(ToID, "New Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                    }


                }
                catch { }
                try
                {
                    string StrCustEmail = Convert.ToString(CommonComponent.GetScalarCommonData("Select Email from tb_Order where OrderNumber='" + OrderNumber + "'"));


                    if (StrCustEmail != null && StrCustEmail != "" && StrCustEmail.Length > 0)
                    {
                        //if (ViewState["BillEmail"] != null)
                        //{
                        //    CommonOperations.SendMail(ViewState["BillEmail"].ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                        //}
                        //else
                        //{
                        CommonOperations.SendMail(StrCustEmail.ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                    }
                }
                catch { }
            }
            catch
            {
            }
        }


    }
}