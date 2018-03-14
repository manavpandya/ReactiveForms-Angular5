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

namespace Solution.UI.Web
{
    public partial class Paypalexpressok : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {



            //   clsCustomer ThisCustomer;
            ///iPrincipal1 = ((EcommPrincipal)Context.User);
            int custid = 0;
            int Orderid = 0;
            int CustomerQuoteID = 0;
            //string AppKeyName = "CustIDKey" + Convert.ToString(HttpContext.Current.Request.UserHostAddress).Replace('.', '-');
            string SessionKeyName = "CustIDKey" + Convert.ToString(HttpContext.Current.Request.UserHostAddress).Replace('.', '-');
            //if (Cache.Get("CacheCustomerID") != null)


            //   CommonComponent.(Query);


            DataSet DsGetCust = CommonComponent.GetCommonDataSet("select RecCustID,OrderNumber,CustomerQuoteID from tb_CustomerStatus where RecUniqueID='" + SessionKeyName + "'");



            string strTranscationMode = "auth";
            object TranscationMode = CommonComponent.GetScalarCommonData("select InitialPaymentStatus from tb_PaymentServices  where PaymentService like '%PAYPALEXPRESS%'");
            if (TranscationMode != null)
            {
                strTranscationMode = Convert.ToString(TranscationMode);
            }


            if (DsGetCust.Tables[0].Rows.Count > 0)
            {
                custid = Convert.ToInt32(DsGetCust.Tables[0].Rows[0]["RecCustID"].ToString());
                Orderid = Convert.ToInt32(DsGetCust.Tables[0].Rows[0]["OrderNumber"].ToString());

                if (DsGetCust.Tables[0].Rows[0]["CustomerQuoteID"] != DBNull.Value)
                {
                    if (Convert.ToString(DsGetCust.Tables[0].Rows[0]["CustomerQuoteID"]) != "")
                        CustomerQuoteID = Convert.ToInt32(DsGetCust.Tables[0].Rows[0]["CustomerQuoteID"].ToString());
                }
            }
            if (custid != 0 && Orderid != 0)
            {
                Request.Cookies.Remove("CustIDPaypal");
                //   ThisCustomer = new clsCustomer(custid);

                String PayPalToken = String.Empty;

                if (HttpContext.Current.Request.QueryString["token"] != null)
                {
                    try
                    {
                        PayPalToken = HttpContext.Current.Request.QueryString["token"].ToString();
                    }
                    catch (Exception ex)
                    {
                        PayPalToken = String.Empty;
                        CommonComponent.ErrorLog("Error Description Paypal: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->Page_Load() \r\n Date: " + System.DateTime.Now + "\r\n", "", "");
                    }
                }

                if (PayPalToken.ToLowerInvariant().Replace(" ", "").IndexOf("<script") != -1)
                {
                    throw new ArgumentException("SECURITY EXCEPTION");
                }

                PayPalComponent gw = new PayPalComponent();
                String PayerID = gw.GetECDetails(PayPalToken, custid);

                Session["PayPalExpressToken"] = SecurityComponent.Encrypt(PayPalToken);
                Session["PayPalExpressPayerID"] = SecurityComponent.Encrypt(PayerID);
                if (HttpContext.Current.Request.QueryString["st"] != null)
                {
                    Session["PayPalstatus"] = HttpContext.Current.Request.QueryString["st"].ToString();
                }

                String AuthorizationResult = String.Empty;
                String AuthorizationCode = String.Empty;
                String AuthorizationTransID = String.Empty;
                String TransactionCommand = String.Empty;
                String TransactionResponse = String.Empty;

                Decimal OrderTotals = 0;

                Object OrderTotal = CommonComponent.GetScalarCommonData(" select OrderTotal  from tb_Order  where OrderNumber ='" + Orderid + "'");

                Decimal.TryParse(Convert.ToString(OrderTotal), out OrderTotals);

                // OrderTotals = 1;

                PayPalComponent gw1 = new PayPalComponent();

                string status = gw1.ProcessEC(OrderTotals, Orderid, PayPalToken, PayerID, strTranscationMode, out AuthorizationResult, out AuthorizationTransID);
                if (status == AppLogic.ro_OK)
                {
                    CommonComponent.ExecuteCommonData("update tb_order set TransactionStatus='" + AppLogic.ro_TXStateAuthorized + "', AuthorizedOn=dateadd(hour,-2,getdate()) where OrderNumber='" + Orderid + "'");
                    String TransCMD = TransactionCommand;
                    String TransRES = AuthorizationResult;



                    if (strTranscationMode.ToLower() == "sale")
                    {
                        String sql2 = "update tb_order set " +
                         "PaymentGateway='PAYPALEXPRESS',Deleted=0,TransactionStatus='CAPTURED',CapturedOn=getdate() ," +
                         "AuthorizationResult=" + SQuote(TransRES) + ", " +
                         "AuthorizationCode=" + SQuote(AuthorizationCode) + ", " +
                         "AuthorizationPNREF=" + SQuote(AuthorizationTransID) + ", " +
                         "TransactionCommand=" + SQuote(TransCMD) + ", " +
                         "CaptureTxCommand=" + SQuote(TransCMD) + ", " +
                         "CaptureTXResult=" + SQuote(TransRES) +
                     " where OrderNumber=" + Orderid.ToString();
                        CommonComponent.ExecuteCommonData(sql2);
                    }
                    else
                    {

                        String sql2 = "update tb_order set " +
                            "PaymentGateway='PAYPALEXPRESS',Deleted=0,TransactionStatus='AUTHORIZED',OrderStatus='AUTHORIZED', " +
                            "AuthorizationResult=" + SQuote(TransRES) + ", " +
                            "AuthorizationCode=" + SQuote(AuthorizationCode) + ", " +
                            "AuthorizationPNREF=" + SQuote(AuthorizationTransID) + ", " +
                            "TransactionCommand=" + SQuote(TransCMD) +
                        " where OrderNumber=" + Orderid.ToString();
                        CommonComponent.ExecuteCommonData(sql2);

                    }
                    CommonComponent.ExecuteCommonData("DELETE FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE CustomerId=" + custid + ")");
                    CommonComponent.ExecuteCommonData("DELETE FROM  tb_ShoppingCart WHERE CustomerId=" + custid + "");


                    Session["ONo"] = Orderid;
                    Session["CustID"] = custid;
                    CommonComponent.ExecuteCommonData("delete from tb_CustomerStatus where RecCustID='" + custid + "'");


                    #region Send Invoice Email to Customer

                    SendMail(Orderid);

                    #endregion


                    if (CustomerQuoteID > 0)
                    {

                        CommonComponent.ExecuteCommonData("update tb_customerquote set OrderNumber=" + Orderid + " where CustomerQuoteID="
                            + Convert.ToInt32(CustomerQuoteID));
                    }

                    Response.Redirect("/orderreceived.aspx");
                }
                else
                {
                    Session["CustID"] = custid;
                    OrderComponent objDsorder = new OrderComponent();
                    tb_FailedTransaction objFailed = new tb_FailedTransaction();
                    objFailed.OrderNumber = Convert.ToInt32(Orderid);
                    objFailed.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
                    objFailed.PaymentGateway = Convert.ToString(Session["PaymentGateway"].ToString());
                    objFailed.Paymentmethod = Convert.ToString(Session["PaymentMethod"].ToString());
                    objFailed.TransactionCommand = Convert.ToString(TransactionCommand);
                    objFailed.TransactionResult = Convert.ToString(TransactionResponse);
                    objFailed.OrderDate = DateTime.Now;
                    objFailed.IPAddress = Request.UserHostAddress.ToString();
                    Int32 FaileId = Convert.ToInt32(objDsorder.AddOrderFailedTransaction(objFailed, Convert.ToInt32(1)));
                    SendMailForFailedTransaction(Orderid);

                    Object ObCustomerQuoteID = CommonComponent.GetScalarCommonData(" select CustomerQuoteID  from tb_CustomerQuote   where OrderNumber ='" + Orderid + "' ");

                    if (ObCustomerQuoteID != null && Convert.ToString(ObCustomerQuoteID) != "" && Convert.ToString(ObCustomerQuoteID) != "0")
                    {
                        if (CustomerQuoteID > 0)
                        {
                            string QuoteID = SecurityComponent.Encrypt(Convert.ToString(CustomerQuoteID));
                            Session["QuoteIDCompare"] = QuoteID.ToString();
                            Response.Redirect("/CustomerquoteCheckout.aspx?custquoteid='" + QuoteID + "' &error=" + status + "");

                            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "pageredirect", "window.location.href='/checkoutcustomerquote.aspx?custquoteid=" + QuoteID + "&error=" + status + "';", true);

                        }
                    }
                    else
                    {
                        if (Request.QueryString["stype"] != null)
                        {

                            if (Convert.ToString(Request.QueryString["stype"]) != "")
                            {
                                //  String sURL = gw.StartEC(strTranscationMode, "/checkoutcommon.aspx?resetlinkback=1&stype=" + Convert.ToString(Request.QueryString["stype"]) + "");

                                Response.Redirect("/checkoutcommon.aspx?error=" + status + "&stype=" + Convert.ToString(Request.QueryString["stype"]) + "");
                            }
                        }
                        else
                            Response.Redirect("/checkoutcommon.aspx?error=" + status + "");
                    }
                }


                //  String next = "order.aspx?paymentmethod=" + Server.UrlEncode(AppLogic.ro_PMPayPalExpress);

                //    next += "&useraction=commit&CustID=" + custid;


                //Response.Redirect(next);
                //}
                //else
                //{
                //    Response.Redirect("checkoutcommon.aspx");
                //}
            }
        }


        /// <summary>
        /// Sends the Mail for Failed Transaction.
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        public void SendMailForFailedTransaction(Int32 OrderNumber)
        {
            string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
            string Body = "";
            string url = AppLogic.AppConfigs("LIVE_SERVER") + "/" + "FailedTransactionEmail.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
            WebRequest NewWebReq = WebRequest.Create(url);
            WebResponse newWebRes = NewWebReq.GetResponse();
            string format = newWebRes.ContentType;
            Stream ftprespstrm = newWebRes.GetResponseStream();
            StreamReader reader;
            reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
            Body = reader.ReadToEnd().ToString();
            Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");
            AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
            try
            {
                CommonOperations.SendMail(ToID, "Failed Order Transaction for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
            }
            catch { }
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
        public void SendMail(Int32 OrderNumber)
        {
            try
            {
                //string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
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
                    string Emailforshippingmethod = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 ShippingMethod FROM tb_order WHERE OrderNumber=" + OrderNumber + " and ShippingMethod <> 'Ground'"));
                    if (!string.IsNullOrEmpty(Emailforshippingmethod))
                    {
                        //ToID = ToID + ";" + Session["Emailforshippingmethod"].ToString();

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
                        CommonOperations.SendMail(StrCustEmail, "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                    }
                    //  else
                    //  {
                    //  CommonOperations.SendMail(StrCustEmail, "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                    // }
                }
                catch { }
            }
            catch
            {
            }
        }
    }
}