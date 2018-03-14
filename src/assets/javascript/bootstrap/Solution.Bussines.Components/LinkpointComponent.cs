using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;
using LinkPointTransaction;
using System.Data.Objects;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Linkpoint Component Class Contains Credticard Process related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class LinkpointComponent
    {
        #region  Key Function

        /// <summary>
        /// Capture Order By Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>Return String</returns>
        public string CaptureOrder(Int32 OrderNumber)
        {
            String result = "LINKPOINT COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH LINKPOINT CODE TURNED ON";
            DataSet dsOrder = new DataSet();
            OrderComponent objDsorder = new OrderComponent();
            dsOrder = objDsorder.GetOrderDetailsByOrderID(OrderNumber);
            bool useLiveTransactions = Convert.ToBoolean(AppLogic.AppConfigs("UseLiveTransactions"));
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {

                String TransID = Convert.ToString(dsOrder.Tables[0].Rows[0]["AuthorizationPNREF"].ToString());
                Decimal OrderTotal = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString());
                String CardNumber = SecurityComponent.Decrypt(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString());
                String CardExpirationMonth = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                String CardExpirationYear = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString());

                if (CardExpirationYear.Length > 2)
                {
                    CardExpirationYear = CardExpirationYear.Substring(2, 2);
                }

                if (TransID.Length == 0 || TransID == "0")
                {
                    result = "Invalid or Empty Transaction ID";
                }
                else
                {
                    LPOrderPart objLporderPart = LPOrderFactory.createOrderPart("order");
                    LPOrderPart ObjectOrderpart = LPOrderFactory.createOrderPart();

                    ObjectOrderpart.put("ordertype", "POSTAUTH");
                    objLporderPart.addPart("orderoptions", ObjectOrderpart);

                    ObjectOrderpart.clear();
                    ObjectOrderpart.put("configfile", AppLogic.AppConfigs("LINKPOINT_CONFIGFILE"));
                    objLporderPart.addPart("merchantinfo", ObjectOrderpart);

                    // Build creditcard
                    ObjectOrderpart.clear();
                    ObjectOrderpart.put("cardnumber", CardNumber);
                    ObjectOrderpart.put("cardexpmonth", CardExpirationMonth);
                    ObjectOrderpart.put("cardexpyear", CardExpirationYear);
                    // add creditcard to order
                    objLporderPart.addPart(Convert.ToString(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()).ToLower(), ObjectOrderpart);

                    // Build payment
                    ObjectOrderpart.clear();
                    ObjectOrderpart.put("chargetotal", OrderTotal.ToString("f2"));
                    // add payment to order
                    objLporderPart.addPart("payment", ObjectOrderpart);

                    // Add oid
                    ObjectOrderpart.clear();
                    ObjectOrderpart.put("oid", TransID);
                    // add transactiondetails to order
                    objLporderPart.addPart("transactiondetails", ObjectOrderpart);

                    LinkPointTxn LPTxn = new LinkPointTxn();

                    // get outgoing Xml from the 'order' object
                    string outXml = objLporderPart.toXML();
                    outXml = outXml.Replace("CREDITCARD", "creditcard");

                    String KeyFile = AppLogic.AppConfigs("LINKPOINT_KEYFILE");
                    if (!System.IO.File.Exists(KeyFile))
                    {
                        KeyFile = HttpContext.Current.Request.MapPath(KeyFile);
                    }

                    //objOrder.CaptureTxCommand = outXml;

                    // Call LPTxn
                    String LinkPoint_Host = "";
                    if (Convert.ToBoolean(AppLogic.AppConfigs("UseLiveTransactions")))
                    {
                        LinkPoint_Host = AppLogic.AppConfigs("LINKPOINT_LIVE_SERVER").ToString();
                    }
                    else
                    {
                        LinkPoint_Host = AppLogic.AppConfigs("LINKPOINT_TEST_SERVER").ToString();
                    }

                    int Port = 0;
                    Int32.TryParse(AppLogic.AppConfigs("LINKPOINT_PORT"), out Port);
                    string rawResponseString = LPTxn.send(KeyFile, LinkPoint_Host, Port, outXml);

                    SQLAccess objExecute = new SQLAccess();
                    objExecute.ExecuteNonQuery("update tb_Order set CaptureTxCommand='" + outXml.Replace("'", "''") + "', CaptureTXResult='" + rawResponseString.Replace("'", "''") + "' where OrderNumber=" + OrderNumber + "");


                    // objOrder.CaptureTXResult = rawResponseString;
                    // response holders
                    String R_Approved = ParseTag("r_approved", rawResponseString);
                    String R_Code = ParseTag("r_code", rawResponseString);
                    String R_Error = ParseTag("r_error", rawResponseString);
                    String R_Message = ParseTag("r_message", rawResponseString);

                    if (R_Approved == "APPROVED")
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = R_Error;
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Order Process Card
        /// </summary>
        /// <param name="OrderNumber">Int32 orderNumber</param>
        /// <param name="TransactionMode">string Transaction Mode</param>
        /// <param name="CardExtraCode">string Extra code</param>
        /// <param name="CAVV">string CAVV</param>
        /// <param name="ECI">string ECI</param>
        /// <param name="XID">string XID</param>
        /// <param name="AVSResult">string AVSResult</param>
        /// <param name="AuthorizationResult">string AuthorizationResult</param>
        /// <param name="AuthorizationCode">string AuthorizationCode</param>
        /// <param name="AuthorizationTransID">string AuthorizationTransID</param>
        /// <param name="TransactionCommandOut">string TransactionCommandOut</param>
        /// <param name="TransactionResponse">string TransactionResponse</param>
        /// <returns>Returns String Result data</returns>
        public String ProcessCard(int OrderNumber, String TransactionMode, String CardExtraCode, String CAVV, String ECI, String XID, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut, out String TransactionResponse)
        {
            String result = "LINKPOINT COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH LINKPOINT CODE TURNED ON";
            AuthorizationCode = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;
            AVSResult = String.Empty;
            TransactionCommandOut = String.Empty;
            TransactionResponse = String.Empty;

            CountryComponent objCountry = new CountryComponent();
            // create order
            LPOrderPart objLporderpart = LPOrderFactory.createOrderPart("order");
            // create a part we will use to build the order
            LPOrderPart objOrderpart = LPOrderFactory.createOrderPart();
            OrderComponent objDsorder = new OrderComponent();
            DataSet dsOrder = new DataSet();
            dsOrder = objDsorder.GetOrderDetailsByOrderID(OrderNumber);
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                // Build 'orderoptions'
                objOrderpart.put("ordertype", TransactionMode.ToUpper());
                // add 'orderoptions to order
                objLporderpart.addPart("orderoptions", objOrderpart);

                // Build 'merchantinfo'
                objOrderpart.clear();
                objOrderpart.put("configfile", AppLogic.AppConfigs("LINKPOINT_CONFIGFILE"));
                // add 'merchantinfo to order
                objLporderpart.addPart("merchantinfo", objOrderpart);

                // Build 'billing'
                // Required for AVS. If not provided, 
                // transactions will downgrade.
                objOrderpart.clear();
                objOrderpart.put("name", Convert.ToString(dsOrder.Tables[0].Rows[0]["CardName"].ToString()));
                objOrderpart.put("company", Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString()));
                objOrderpart.put("address1", Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()));
                objOrderpart.put("address2", Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()));
                objOrderpart.put("city", Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCity"].ToString()));
                objOrderpart.put("state", Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingState"].ToString()));
                // Required for AVS. If not provided, 
                // transactions will downgrade.	
                String addrnum = String.Empty;
                int ix = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()).IndexOf(" ");
                if (ix > 0)
                {
                    addrnum = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()).Substring(0, ix);
                }
                if (AppLogic.AppConfigBool("LINKPOINT_Verify_Addresses") && addrnum.Length != 0)
                {
                    objOrderpart.put("zip", Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingZip"].ToString()));
                    objOrderpart.put("addrnum", addrnum);
                }
                objOrderpart.put("country", objCountry.GetCountryCodeByName(Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString())));
                objOrderpart.put("phone", Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString()));
                objOrderpart.put("email", Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingEmail"].ToString()));
                // add 'billing to order
                objLporderpart.addPart("billing", objOrderpart);

                objOrderpart.clear();
                objOrderpart.put("name", Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString()));
                objOrderpart.put("address1", Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString()));
                objOrderpart.put("address2", Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()));
                objOrderpart.put("city", Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString()));
                objOrderpart.put("state", Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingState"].ToString()));
                objOrderpart.put("zip", Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString()));

                objOrderpart.put("country", objCountry.GetCountryCodeByName(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString())));
                objLporderpart.addPart("shipping", objOrderpart);

                // Build 'creditcard'
                objOrderpart.clear();
                objOrderpart.put("cardnumber", SecurityComponent.Decrypt(Convert.ToString(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString())));
                objOrderpart.put("cardexpmonth", Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString()));
                String ExpYear = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString());
                if (Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString()).Length > 2)
                {
                    ExpYear = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString()).Substring(2, 2);
                }
                objOrderpart.put("cardexpyear", ExpYear);
                if (CardExtraCode.Trim().Length != 0)
                {
                    objOrderpart.put("cvmvalue", CardExtraCode.PadLeft(3, '0'));
                }
                if (CardExtraCode.Length != 0)
                {
                    objOrderpart.put("cvmindicator", "provided");
                }
                else
                {
                    objOrderpart.put("cvmindicator", "not_provided");
                }

                // add 'creditcard to order
                objLporderpart.addPart(Convert.ToString(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()).ToLower(), objOrderpart);

                // Build 'payment'
                objOrderpart.clear();
                objOrderpart.put("chargetotal", Convert.ToDouble(Convert.ToString(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString())).ToString("f2"));
                objOrderpart.put("shipping", Convert.ToDouble(dsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()).ToString("f2"));
                objOrderpart.put("tax", Convert.ToDouble(dsOrder.Tables[0].Rows[0]["OrderTax"].ToString()).ToString("f2"));
                objOrderpart.put("subtotal", Convert.ToDouble(dsOrder.Tables[0].Rows[0]["OrderSubtotal"].ToString()).ToString("f2"));
                // add 'payment to order
                objLporderpart.addPart("payment", objOrderpart);

                // Add oid
                objOrderpart.clear();
                objOrderpart.put("oid", AppLogic.AppConfigs("StoreName") + "-" + OrderNumber.ToString());
                // add transactiondetails to order
                objLporderpart.addPart("transactiondetails", objOrderpart);

                // add notes 	
                objOrderpart.clear();
                objOrderpart.put("comments", "CustomerID=" + Convert.ToString(dsOrder.Tables[0].Rows[0]["CustomerID"].ToString()) + ", OrderNumber=" + OrderNumber.ToString());
                objLporderpart.addPart("notes", objOrderpart);

                // create transaction object	
                LinkPointTxn LPTxn = new LinkPointTxn();

                // get outgoing Xml from the 'order' object
                string outXml = objLporderpart.toXML().Replace("CREDITCARD", "creditcard");
                outXml = outXml.Replace("CREDITCARD", "creditcard");

                String KeyFile = AppLogic.AppConfigs("LINKPOINT_KEYFILE");
                if (!System.IO.File.Exists(KeyFile))
                {
                    KeyFile = HttpContext.Current.Request.MapPath(KeyFile);
                }
                // Call LPTxn
                String LinkPoint_Host = "";
                if (Convert.ToBoolean(AppLogic.AppConfigs("UseLiveTransactions")))
                {
                    LinkPoint_Host = AppLogic.AppConfigs("LINKPOINT_LIVE_SERVER").ToString();
                }
                else
                {
                    LinkPoint_Host = AppLogic.AppConfigs("LINKPOINT_TEST_SERVER").ToString();
                }
                int Port = 0;
                Int32.TryParse(AppLogic.AppConfigs("LINKPOINT_PORT"), out Port);
                string rawResponseString = LPTxn.send(KeyFile, LinkPoint_Host, Port, outXml);

                // response holders
                String R_Time = ParseTag("r_time", rawResponseString);
                String R_Ref = ParseTag("r_ref", rawResponseString);
                String R_Approved = ParseTag("r_approved", rawResponseString);
                String R_Code = ParseTag("r_code", rawResponseString);
                String R_Authresr = ParseTag("r_authresronse", rawResponseString);
                String R_Error = ParseTag("r_error", rawResponseString);
                String R_OrderNum = ParseTag("r_ordernum", rawResponseString);
                String R_Message = ParseTag("r_message", rawResponseString);
                String R_Score = ParseTag("r_score", rawResponseString);
                String R_TDate = ParseTag("r_tdate", rawResponseString);
                String R_AVS = ParseTag("r_avs", rawResponseString);
                String R_Tax = ParseTag("r_tax", rawResponseString);
                String R_Shipping = ParseTag("r_shipping", rawResponseString);
                String R_FraudCode = ParseTag("r_fraudCode", rawResponseString);
                String R_ESD = ParseTag("esd", rawResponseString);

                String sql = String.Empty;

                AuthorizationCode = R_Code;
                AuthorizationResult = rawResponseString;
                AuthorizationTransID = R_OrderNum;
                AVSResult = R_AVS;

                TransactionCommandOut = outXml.ToString();
                TransactionResponse = rawResponseString;

                if (R_Approved.Equals("APPROVED", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = "OK";
                }
                else
                {
                    result = R_Error;
                    if (result.Length == 0)
                    {
                        result = "Unspecified Error";
                    }
                    result = result.Replace("account", "card");
                    result = result.Replace("Account", "Card");
                    result = result.Replace("ACCOUNT", "CARD");
                    result = result.Trim();
                    if (result.ToLower().StartsWith("sgs-000001"))
                        result = "The transaction was declined by the gateway.";
                    else if (result.ToLower().StartsWith("sgs-002000"))
                        result = "1. The transaction was declined by the gateway due to insufficient funds.<br/>2. Unsupported credit card type.<br/>3. General Processor Error.";
                    else if (result.ToLower().StartsWith("sgs-002100"))
                        result = "The server encountered a network error communicating with the gateway. Please retry.";
                    else if (result.ToLower().StartsWith("sgs-002302"))
                        result = "1. Invalid credit card expiration month.<br/>2. Invalid credit card expiration year.";
                    else if (result.ToLower().StartsWith("sgs-002303"))
                        result = "Invalid credit card number.";
                    else if (result.ToLower().StartsWith("sgs-002304"))
                        result = "Credit card is expired.";
                    else if (result.ToLower().StartsWith("sgs-005004"))
                        result = "This is duplicate order. Please try after some-time.";
                }
            }

            return result;
        }
        /// <summary>
        /// Void Order By order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>Returns String result data</returns>
        public String VoidOrder(int OrderNumber)
        {
            String result = "LINKPOINT COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH LINKPOINT CODE TURNED ON";

            SQLAccess objExecute = new SQLAccess();
            objExecute.ExecuteNonQuery("update tb_Order set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
            bool useLiveTransactions = AppLogic.AppConfigBool("UseLiveTransactions");
            String TransID = String.Empty;
            String CardNumber = String.Empty;
            String CardExpirationMonth = String.Empty;
            String CardExpirationYear = String.Empty;
            Decimal OrderTotal = System.Decimal.Zero;
            OrderComponent objDsorder = new OrderComponent();
            DataSet dsOrder = new DataSet();
            dsOrder = objDsorder.GetOrderDetailsByOrderID(OrderNumber);
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                TransID = Convert.ToString(dsOrder.Tables[0].Rows[0]["AuthorizationPNREF"].ToString());
                CardNumber = SecurityComponent.Decrypt(Convert.ToString(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString()));
                CardExpirationMonth = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                CardExpirationYear = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString());
                OrderTotal = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString());

                if (CardExpirationYear.Length > 2)
                {
                    CardExpirationYear = CardExpirationYear.Substring(2, 2);
                }

                if (TransID.Length == 0 || TransID == "0")
                {
                    result = "Invalid or Empty Transaction ID";
                }
                else
                {
                    LPOrderPart objLPorderpart = LPOrderFactory.createOrderPart("order");
                    LPOrderPart objOrderpart = LPOrderFactory.createOrderPart();

                    objOrderpart.put("ordertype", "VOID");
                    objLPorderpart.addPart("orderoptions", objOrderpart);

                    objOrderpart.clear();
                    objOrderpart.put("configfile", AppLogic.AppConfigs("LINKPOINT_CONFIGFILE"));
                    objLPorderpart.addPart("merchantinfo", objOrderpart);

                    // Build creditcard
                    objOrderpart.clear();
                    objOrderpart.put("cardnumber", CardNumber);
                    objOrderpart.put("cardexpmonth", CardExpirationMonth);
                    objOrderpart.put("cardexpyear", CardExpirationYear);
                    // add creditcard to order
                    objLPorderpart.addPart(Convert.ToString(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()).ToLower(), objOrderpart);

                    // Build payment
                    objOrderpart.clear();
                    objOrderpart.put("chargetotal", OrderTotal.ToString("f2"));
                    // add payment to order
                    objLPorderpart.addPart("payment", objOrderpart);

                    // Add oid
                    objOrderpart.clear();
                    objOrderpart.put("oid", TransID);
                    // add transactiondetails to order
                    objLPorderpart.addPart("transactiondetails", objOrderpart);

                    LinkPointTxn LPTxn = new LinkPointTxn();

                    // get outgoing Xml from the 'order' object
                    string outXml = objLPorderpart.toXML();
                    outXml = outXml.Replace("CREDITCARD", "creditcard");

                    String KeyFile = AppLogic.AppConfigs("LINKPOINT_KEYFILE");
                    if (!System.IO.File.Exists(KeyFile))
                    {
                        KeyFile = HttpContext.Current.Request.MapPath(KeyFile);
                    }
                    //objExecute.ExecuteNonQuery("update tb_Order set VoidTXCommand='" + outXml.ToString().Replace("'", "''") + "' where OrderNumber=" + OrderNumber.ToString());

                    // Call LPTxn
                    String LinkPoint_Host = "";
                    if (Convert.ToBoolean(AppLogic.AppConfigs("UseLiveTransactions")))
                    {
                        LinkPoint_Host = AppLogic.AppConfigs("LINKPOINT_LIVE_SERVER").ToString();
                    }
                    else
                    {
                        LinkPoint_Host = AppLogic.AppConfigs("LINKPOINT_TEST_SERVER").ToString();
                    }
                    int Port = 0;
                    Int32.TryParse(AppLogic.AppConfigs("LINKPOINT_PORT"), out Port);
                    string rawResponseString = LPTxn.send(KeyFile, LinkPoint_Host, Port, outXml);
                    objExecute.ExecuteNonQuery("UPDATE tb_Order set VoidTXResult=" + rawResponseString.Replace("'", "''") + ",VoidTXCommand='" + outXml.ToString().Replace("'", "''") + "' where OrderNumber=" + OrderNumber.ToString());

                    // response holders
                    String R_Approved = ParseTag("r_approved", rawResponseString);
                    String R_Code = ParseTag("r_code", rawResponseString);
                    String R_Error = ParseTag("r_error", rawResponseString);
                    String R_Message = ParseTag("r_message", rawResponseString);

                    if (R_Approved == "APPROVED")
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = R_Error;
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Refund Order By order Number
        /// </summary>
        /// <param name="OriginalOrderNumber">int OriginalOrderNumber</param>
        /// <param name="NewOrderNumber">int NewOrderNumber</param>
        /// <param name="RefundAmount">decimal RefundAmount</param>
        /// <param name="RefundReason">String RefundReason</param>
        /// <returns>Returns String result data</returns>
        public String RefundOrder(int OriginalOrderNumber, int NewOrderNumber, decimal RefundAmount, String RefundReason)
        {
            String result = "LINKPOINT COM COMPONENTS NOT INSTALLED ON SERVER OR STOREFRONT NOT COMPILED WITH LINKPOINT CODE TURNED ON";

            SQLAccess objExecute = new SQLAccess();
            objExecute.ExecuteNonQuery("update tb_Order set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber=" + OriginalOrderNumber.ToString());
            bool useLiveTransactions = Convert.ToBoolean(AppLogic.AppConfigs("UseLiveTransactions"));
            String TransID = String.Empty;
            String CardNumber = String.Empty;
            String CardExpirationMonth = String.Empty;
            String CardExpirationYear = String.Empty;
            Decimal OrderTotal = System.Decimal.Zero;
            OrderComponent objDsorder = new OrderComponent();
            DataSet dsOrder = new DataSet();
            dsOrder = objDsorder.GetOrderDetailsByOrderID(OriginalOrderNumber);
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                TransID = Convert.ToString(dsOrder.Tables[0].Rows[0]["AuthorizationPNREF"].ToString());
                CardNumber = SecurityComponent.Decrypt(Convert.ToString(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString())).ToString();
                CardExpirationMonth = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                CardExpirationYear = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString());
                OrderTotal = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString());

                if (CardExpirationYear.Length > 2)
                {
                    CardExpirationYear = CardExpirationYear.Substring(2, 2);
                }

                if (TransID.Length == 0 || TransID == "0")
                {
                    result = "Invalid or Empty Transaction ID";
                }
                else if (CardNumber.Length == 0)
                {
                    result = "Credit Card Number Not Found or Empty";
                }
                else
                {
                    LPOrderPart objLPorderpart = LPOrderFactory.createOrderPart("order");
                    LPOrderPart objOrderpart = LPOrderFactory.createOrderPart();

                    objOrderpart.put("ordertype", "CREDIT");
                    objLPorderpart.addPart("orderoptions", objOrderpart);

                    objOrderpart.clear();
                    objOrderpart.put("configfile", AppLogic.AppConfigs("LINKPOINT_CONFIGFILE"));
                    objLPorderpart.addPart("merchantinfo", objOrderpart);

                    // Build creditcard
                    objOrderpart.clear();
                    objOrderpart.put("cardnumber", CardNumber);
                    objOrderpart.put("cardexpmonth", CardExpirationMonth);
                    objOrderpart.put("cardexpyear", CardExpirationYear);
                    // add creditcard to order
                    objLPorderpart.addPart(Convert.ToString(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()).ToLower(), objOrderpart);

                    // Build payment
                    objOrderpart.clear();
                    if (RefundAmount == System.Decimal.Zero)
                    {
                        objOrderpart.put("chargetotal", OrderTotal.ToString("f2"));
                    }
                    else
                    {
                        objOrderpart.put("chargetotal", RefundAmount.ToString("f2"));
                    }
                    // add payment to order
                    objLPorderpart.addPart("payment", objOrderpart);

                    // Add oid
                    objOrderpart.clear();
                    objOrderpart.put("oid", TransID);
                    // add transactiondetails to order
                    objLPorderpart.addPart("transactiondetails", objOrderpart);

                    LinkPointTxn LPTxn = new LinkPointTxn();

                    // get outgoing Xml from the 'order' object
                    string outXml = objLPorderpart.toXML();
                    outXml = outXml.Replace("CREDITCARD", "creditcard");

                    String KeyFile = AppLogic.AppConfigs("LINKPOINT_KEYFILE");
                    if (!System.IO.File.Exists(KeyFile))
                    {
                        KeyFile = HttpContext.Current.Request.MapPath(KeyFile);
                    }
                    String CardToken = String.Format("<cardnumber>{0}</cardnumber>", CardNumber);
                    String CardTokenReplacement = String.Format("<cardnumber>{0}</cardnumber>", CardNumber);


                    // Call LPTxn
                    String LinkPoint_Host = "";
                    if (Convert.ToBoolean(AppLogic.AppConfigs("UseLiveTransactions")))
                    {
                        LinkPoint_Host = AppLogic.AppConfigs("LINKPOINT_LIVE_SERVER").ToString();
                    }
                    else
                    {
                        LinkPoint_Host = AppLogic.AppConfigs("LINKPOINT_TEST_SERVER").ToString();
                    }
                    int Port = 0;
                    Int32.TryParse(AppLogic.AppConfigs("LINKPOINT_PORT"), out Port);
                    string rawResponseString = LPTxn.send(KeyFile, LinkPoint_Host, Port, outXml);
                    objExecute.ExecuteNonQuery("update tb_Order set RefundTXCommand=" + outXml.Replace(CardToken, CardTokenReplacement).ToString().Replace("'", "''") + ", RefundTXResult=" + rawResponseString.ToString().Replace("'", "''") + " where OrderNumber=" + OriginalOrderNumber.ToString());

                    // response holders
                    String R_Approved = ParseTag("r_approved", rawResponseString);
                    String R_Code = ParseTag("r_code", rawResponseString);
                    String R_Error = ParseTag("r_error", rawResponseString);
                    String R_Message = ParseTag("r_message", rawResponseString);

                    if (R_Approved == "APPROVED")
                    {
                        result = "OK";
                    }
                    else
                    {
                        result = R_Error;
                    }
                }
            }

            return result;
        }
        /// <summary>
        /// Split string Data
        /// </summary>
        /// <param name="tag">string tag</param>
        /// <param name="rsp">string rsp</param>
        /// <returns>Returns string result data</returns>
        private string ParseTag(string tag, string rsp)
        {
            StringBuilder sb = new StringBuilder(256);
            sb.AppendFormat("<{0}>", tag);
            int len = sb.Length;
            int idxSt = -1;
            int idxEnd = -1;
            idxSt = rsp.IndexOf(sb.ToString());
            if (idxSt == -1)
            {
                return "";
            }
            idxSt += len;
            sb.Remove(0, len);
            sb.AppendFormat("</{0}>", tag);
            idxEnd = rsp.IndexOf(sb.ToString(), idxSt);
            if (idxEnd == -1)
            {
                return "";
            }
            return rsp.Substring(idxSt, idxEnd - idxSt);
        }
        #endregion
    }
}
