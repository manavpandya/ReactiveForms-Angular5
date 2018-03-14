using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Xml;
using System.Globalization;
using System.Data.SqlClient;

using Solution.Bussines.Entities;
using System.Web;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// AuthorizeNet Component Class Contains AuthorizeNet Related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class AuthorizeNetComponent
    {
        public AuthorizeNetComponent() { }

        /// <summary>
        /// Get Card Process For Client Side Verification
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="OrderTotal">Decimal OrderTotal</param>
        /// <param name="useLiveTransactions">bool useLiveTransactions</param>
        /// <param name="TransactionMode">String TransactionMode</param>
        /// <param name="UseBillingAddress">tb_Order UseBillingAddress</param>
        /// <param name="UseShippingAddress">tb_Order UseShippingAddress</param>
        /// <param name="CAVV">String CAVV</param>
        /// <param name="ECI">String ECI</param>
        /// <param name="XID">String XID</param>        /// 
        /// <param name="AVSResult">String AVSResult</param>
        /// <param name="AuthorizationResult">String AuthorizationResult</param>
        /// <param name="AuthorizationCode">String AuthorizationCode</param>
        /// <param name="AuthorizationTransID">String AuthorizationTransID</param>
        /// <param name="TransactionCommandOut">String TransactionCommandOut</param>
        /// <param name="TransactionResponse">String TransactionResponse</param>
        /// <returns>Returns Card Verifications Details</returns>
        static public String ProcessCardForClientSide(int OrderNumber, int CustomerID, Decimal OrderTotal, bool useLiveTransactions, String TransactionMode, tb_Order UseBillingAddress, tb_Order UseShippingAddress, String CAVV, String ECI, String XID, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut, out String TransactionResponse)
        {
            String result = Solution.Bussines.Components.Common.AppLogic.ro_OK;
            AuthorizationCode = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;
            AVSResult = String.Empty;
            TransactionCommandOut = String.Empty;
            TransactionResponse = String.Empty;

            String X_Login = Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_LOGIN");
            if (X_Login.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(Solution.Bussines.Components.Common.AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_Login = reg.Read("AUTHORIZENET_X_LOGIN");
                reg = null;
            }

            String X_TranKey = Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_TRAN_KEY");
            if (X_TranKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(Solution.Bussines.Components.Common.AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_TranKey = reg.Read("AUTHORIZENET_X_TRAN_KEY");
                reg = null;
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);

            //transactionCommand.Append("x_type=" + CommonLogic.IIF(TransactionMode == Solution.Bussines.Components.Common.AppLogic.ro_TXModeAuthOnly, "AUTH_ONLY", "AUTH_CAPTURE"));
            if (TransactionMode.ToLower().IndexOf("auth_only") > -1)
            {
                transactionCommand.Append("x_type=AUTH_ONLY");
            }
            else
            {
                transactionCommand.Append("x_type=AUTH_CAPTURE");
            }
            transactionCommand.Append("&x_login=" + X_Login);
            transactionCommand.Append("&x_tran_key=" + X_TranKey);
            transactionCommand.Append("&x_version=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_VERSION"));
            transactionCommand.Append("&x_test_request=FALSE");// + CommonLogic.IIF(useLiveTransactions, "FALSE", "TRUE"));
            transactionCommand.Append("&x_merchant_email=" + HttpContext.Current.Server.UrlEncode(Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_Email")));
            transactionCommand.Append("&x_description=" + HttpContext.Current.Server.UrlEncode(Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreName") + " Order " + OrderNumber.ToString()));

            transactionCommand.Append("&x_method=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_METHOD"));

            transactionCommand.Append("&x_delim_Data=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_DATA"));
            transactionCommand.Append("&x_delim_Char=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR"));
            transactionCommand.Append("&x_encap_char=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_ENCAP_CHAR"));
            transactionCommand.Append("&x_relay_response=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_RELAY_RESPONSE"));

            transactionCommand.Append("&x_email_customer=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_Email_CUSTOMER"));
            transactionCommand.Append("&x_recurring_billing=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_RECURRING_BILLING"));

            transactionCommand.Append("&x_amount=" + OrderTotal);
            transactionCommand.Append("&x_card_num=" + UseBillingAddress.CardNumber.ToString());
            transactionCommand.Append("&x_card_code=" + UseBillingAddress.CardVarificationCode.Trim());

            transactionCommand.Append("&x_exp_date=" + UseBillingAddress.CardExpirationMonth.ToString().PadLeft(2, '0') + UseBillingAddress.CardExpirationYear);
            transactionCommand.Append("&x_phone=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingPhone));
            transactionCommand.Append("&x_fax=");
            transactionCommand.Append("&x_customer_tax_id=");
            transactionCommand.Append("&x_cust_id=" + CustomerID.ToString());
            transactionCommand.Append("&x_invoice_num=" + OrderNumber.ToString());
            transactionCommand.Append("&x_email=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.Email));
            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            transactionCommand.Append("&x_first_name=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingFirstName));
            transactionCommand.Append("&x_last_name=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingLastName));
            transactionCommand.Append("&x_company=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCompany));
            transactionCommand.Append("&x_address=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingAddress1));
            transactionCommand.Append("&x_city=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCity));
            transactionCommand.Append("&x_state=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingState));
            transactionCommand.Append("&x_zip=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingZip));
            transactionCommand.Append("&x_country=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCountry));

            if (UseShippingAddress != null)
            {
                transactionCommand.Append("&x_ship_to_first_name=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingFirstName));
                transactionCommand.Append("&x_ship_to_last_name=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingLastName));
                transactionCommand.Append("&x_ship_to_company=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCompany));
                transactionCommand.Append("&x_ship_to_address=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingAddress1));
                transactionCommand.Append("&x_ship_to_city=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCity));
                transactionCommand.Append("&x_ship_to_state=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingState));
                transactionCommand.Append("&x_ship_to_zip=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingZip));
                transactionCommand.Append("&x_ship_to_country=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCountry));
            }

            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            if (CAVV.Length != 0 || ECI.Length != 0)
            {
                transactionCommand.Append("&x_authentication_indicator=" + ECI);
                transactionCommand.Append("&x_cardholder_authentication_value=" + CAVV);
            }

            byte[] data = encoding.GetBytes(transactionCommand.ToString());
            //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>TranscationCommand: " + transactionCommand.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
            // Prepare web request...
            try
            {

                String AuthServer = Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_LIVE_SERVER");

                if (!useLiveTransactions)
                    AuthServer = Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_TEST_SERVER");

                String rawResponseString = String.Empty;

                int MaxTries = Convert.ToInt32(Solution.Bussines.Components.Common.AppLogic.AppConfigs("GatewayRetries")) + 1;
                int CurrentTry = 0;
                bool CallSuccessful = false;
                do
                {
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    myRequest.Method = "POST";
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                    myRequest.ContentLength = data.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    // get the response
                    WebResponse myResponse;

                    CurrentTry++;
                    try
                    {
                        //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>Making Request..." + CurrentTry + " Time<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                        myResponse = myRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                        {
                            rawResponseString = sr.ReadToEnd();
                            sr.Close();
                        }
                        myResponse.Close();
                        CallSuccessful = true;
                        //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>Request successfully executed..." + CurrentTry + " Time<br/>Response: " + rawResponseString + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                    }
                    catch (Exception ex)
                    {
                        CallSuccessful = false;
                        //CommonOperation.TraceProcess("OrderProcessError", "Order " + OrderNumber + "<br/>Error while creating Request to AuthorizeNet." + CurrentTry + " Time<br/>Error Description: " + ex.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                        rawResponseString = "0|||Error Calling Authorize.Net Payment Gateway||||||||";
                    }
                }
                while (!CallSuccessful && CurrentTry < MaxTries);


                // rawResponseString now has gateway response
                TransactionResponse = rawResponseString;
                String[] statusArray = rawResponseString.Split(Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR").ToCharArray());
                // this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
                for (int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
                {
                    statusArray[i] = statusArray[i].Trim('\"');
                }

                String sql = String.Empty;
                String replyCode = statusArray[0].Replace(":", "");
                String responseCode = statusArray[2];
                String approvalCode = statusArray[4];
                String authResponse = statusArray[3];
                String TransID = statusArray[6];

                AuthorizationCode = statusArray[4];
                AuthorizationResult = rawResponseString;
                AuthorizationTransID = statusArray[6];
                AVSResult = statusArray[5];
                TransactionCommandOut = transactionCommand.ToString().Replace(X_TranKey, "*".PadLeft(X_TranKey.Length));

                if (replyCode == "1")
                {
                    result = Solution.Bussines.Components.Common.AppLogic.ro_OK;
                    CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + OrderNumber + "");
                }
                else
                {
                    result = authResponse;
                    if (result.Length == 0)
                    {
                        result = "Unspecified Error";
                    }
                    else
                    {
                        result = result.Replace("account", "card");
                        result = result.Replace("Account", "Card");
                        result = result.Replace("ACCOUNT", "CARD");
                    }
                }
                //CommonOperation.TraceProcess("OrderProcessResult", "Order " + OrderNumber + "<br/>Return Value: " + authResponse.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
            }
            catch (Exception ex)
            {
                //CommonOperation.TraceProcess("OrderProcessResultError", "Order " + OrderNumber + "<br/>Error while processing result: " + ex.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                result = "Error calling Authorize.net gateway. Please retry your order in a few minutes or select another checkout payment option.";
            }
            return result;
        }
        static public String ProcessCardForYahooorder(int OrderNumber, int CustomerID, Decimal OrderTotal, bool useLiveTransactions, String TransactionMode, tb_Order UseBillingAddress, tb_Order UseShippingAddress, String CAVV, String ECI, String XID, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut, out String TransactionResponse)
        {
            String result = Solution.Bussines.Components.Common.AppLogic.ro_OK;
            AuthorizationCode = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;
            AVSResult = String.Empty;
            TransactionCommandOut = String.Empty;
            TransactionResponse = String.Empty;

            String X_Login = Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_LOGIN");
            if (X_Login.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(Solution.Bussines.Components.Common.AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_Login = reg.Read("AUTHORIZENET_X_LOGIN");
                reg = null;
            }

            String X_TranKey = Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_TRAN_KEY");
            if (X_TranKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(Solution.Bussines.Components.Common.AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_TranKey = reg.Read("AUTHORIZENET_X_TRAN_KEY");
                reg = null;
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);

            //transactionCommand.Append("x_type=" + CommonLogic.IIF(TransactionMode == Solution.Bussines.Components.Common.AppLogic.ro_TXModeAuthOnly, "AUTH_ONLY", "AUTH_CAPTURE"));
            if (TransactionMode.ToLower().IndexOf("auth_only") > -1)
            {
                transactionCommand.Append("x_type=AUTH_ONLY");
            }
            else
            {
                transactionCommand.Append("x_type=AUTH_CAPTURE");
            }
            transactionCommand.Append("&x_login=" + X_Login);
            transactionCommand.Append("&x_tran_key=" + X_TranKey);
            transactionCommand.Append("&x_version=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_VERSION"));
            transactionCommand.Append("&x_test_request=FALSE");// + CommonLogic.IIF(useLiveTransactions, "FALSE", "TRUE"));
            transactionCommand.Append("&x_merchant_email=" + HttpContext.Current.Server.UrlEncode(Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_Email")));
            transactionCommand.Append("&x_description=" + HttpContext.Current.Server.UrlEncode(Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreName") + " Order " + OrderNumber.ToString()));

            transactionCommand.Append("&x_method=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_METHOD"));

            transactionCommand.Append("&x_delim_Data=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_DATA"));
            transactionCommand.Append("&x_delim_Char=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR"));
            transactionCommand.Append("&x_encap_char=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_ENCAP_CHAR"));
            transactionCommand.Append("&x_relay_response=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_RELAY_RESPONSE"));

            transactionCommand.Append("&x_email_customer=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_Email_CUSTOMER"));
            transactionCommand.Append("&x_recurring_billing=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_RECURRING_BILLING"));

            transactionCommand.Append("&x_amount=" + OrderTotal);
            transactionCommand.Append("&x_card_num=" + SecurityComponent.Decrypt(UseBillingAddress.CardNumber.ToString()));
            //transactionCommand.Append("&x_card_code=" + UseBillingAddress.CardVarificationCode.Trim());
            transactionCommand.Append("&x_card_code=");

            transactionCommand.Append("&x_exp_date=" + UseBillingAddress.CardExpirationMonth.ToString().PadLeft(2, '0') + UseBillingAddress.CardExpirationYear);
            transactionCommand.Append("&x_phone=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingPhone));
            transactionCommand.Append("&x_fax=");
            transactionCommand.Append("&x_customer_tax_id=");
            transactionCommand.Append("&x_cust_id=" + CustomerID.ToString());
            transactionCommand.Append("&x_invoice_num=" + OrderNumber.ToString());
            transactionCommand.Append("&x_email=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.Email));
            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            transactionCommand.Append("&x_first_name=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingFirstName));
            transactionCommand.Append("&x_last_name=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingLastName));
            transactionCommand.Append("&x_company=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCompany));
            transactionCommand.Append("&x_address=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingAddress1));
            transactionCommand.Append("&x_city=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCity));
            transactionCommand.Append("&x_state=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingState));
            transactionCommand.Append("&x_zip=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingZip));
            transactionCommand.Append("&x_country=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCountry));

            if (UseShippingAddress != null)
            {
                transactionCommand.Append("&x_ship_to_first_name=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingFirstName));
                transactionCommand.Append("&x_ship_to_last_name=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingLastName));
                transactionCommand.Append("&x_ship_to_company=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCompany));
                transactionCommand.Append("&x_ship_to_address=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingAddress1));
                transactionCommand.Append("&x_ship_to_city=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCity));
                transactionCommand.Append("&x_ship_to_state=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingState));
                transactionCommand.Append("&x_ship_to_zip=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingZip));
                transactionCommand.Append("&x_ship_to_country=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCountry));
            }

            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            if (CAVV.Length != 0 || ECI.Length != 0)
            {
                transactionCommand.Append("&x_authentication_indicator=" + ECI);
                transactionCommand.Append("&x_cardholder_authentication_value=" + CAVV);
            }

            byte[] data = encoding.GetBytes(transactionCommand.ToString());
            //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>TranscationCommand: " + transactionCommand.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
            // Prepare web request...
            try
            {

                String AuthServer = Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_LIVE_SERVER");

                if (!useLiveTransactions)
                    AuthServer = Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_TEST_SERVER");

                String rawResponseString = String.Empty;

                int MaxTries = Convert.ToInt32(Solution.Bussines.Components.Common.AppLogic.AppConfigs("GatewayRetries")) + 1;
                int CurrentTry = 0;
                bool CallSuccessful = false;
                do
                {
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    myRequest.Method = "POST";
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                    myRequest.ContentLength = data.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    // get the response
                    WebResponse myResponse;

                    CurrentTry++;
                    try
                    {
                        //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>Making Request..." + CurrentTry + " Time<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                        myResponse = myRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                        {
                            rawResponseString = sr.ReadToEnd();
                            sr.Close();
                        }
                        myResponse.Close();
                        CallSuccessful = true;
                        //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>Request successfully executed..." + CurrentTry + " Time<br/>Response: " + rawResponseString + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                    }
                    catch (Exception ex)
                    {
                        CallSuccessful = false;
                        //CommonOperation.TraceProcess("OrderProcessError", "Order " + OrderNumber + "<br/>Error while creating Request to AuthorizeNet." + CurrentTry + " Time<br/>Error Description: " + ex.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                        rawResponseString = "0|||Error Calling Authorize.Net Payment Gateway||||||||";
                    }
                }
                while (!CallSuccessful && CurrentTry < MaxTries);


                // rawResponseString now has gateway response
                TransactionResponse = rawResponseString;
                String[] statusArray = rawResponseString.Split(Solution.Bussines.Components.Common.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR").ToCharArray());
                // this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
                for (int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
                {
                    statusArray[i] = statusArray[i].Trim('\"');
                }

                String sql = String.Empty;
                String replyCode = statusArray[0].Replace(":", "");
                String responseCode = statusArray[2];
                String approvalCode = statusArray[4];
                String authResponse = statusArray[3];
                String TransID = statusArray[6];

                AuthorizationCode = statusArray[4];
                AuthorizationResult = rawResponseString;
                AuthorizationTransID = statusArray[6];
                AVSResult = statusArray[5];
                TransactionCommandOut = transactionCommand.ToString().Replace(X_TranKey, "*".PadLeft(X_TranKey.Length));

                if (replyCode == "1")
                {
                    result = Solution.Bussines.Components.Common.AppLogic.ro_OK;
                     CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + OrderNumber + "");
                }
                else
                {
                    result = authResponse;
                    if (result.Length == 0)
                    {
                        result = "Unspecified Error";
                    }
                    else
                    {
                        result = result.Replace("account", "card");
                        result = result.Replace("Account", "Card");
                        result = result.Replace("ACCOUNT", "CARD");
                    }
                }
                //CommonOperation.TraceProcess("OrderProcessResult", "Order " + OrderNumber + "<br/>Return Value: " + authResponse.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
            }
            catch (Exception ex)
            {
                //CommonOperation.TraceProcess("OrderProcessResultError", "Order " + OrderNumber + "<br/>Error while processing result: " + ex.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                result = "Error calling Authorize.net gateway. Please retry your order in a few minutes or select another checkout payment option.";
            }
            return result;
        }
        static public String ProcessCardForYahooorderAdmin(int OrderNumber, int CustomerID, Decimal OrderTotal, bool useLiveTransactions, String TransactionMode, tb_Order UseBillingAddress, tb_Order UseShippingAddress, String CAVV, String ECI, String XID, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut, out String TransactionResponse)
        {
            String result = Solution.Bussines.Components.AdminCommon.AppLogic.ro_OK;
            AuthorizationCode = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;
            AVSResult = String.Empty;
            TransactionCommandOut = String.Empty;
            TransactionResponse = String.Empty;

            String X_Login = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_LOGIN");
            if (X_Login.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_Login = reg.Read("AUTHORIZENET_X_LOGIN");
                reg = null;
            }

            String X_TranKey = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_TRAN_KEY");
            if (X_TranKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_TranKey = reg.Read("AUTHORIZENET_X_TRAN_KEY");
                reg = null;
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);

            //transactionCommand.Append("x_type=" + CommonLogic.IIF(TransactionMode == Solution.Bussines.Components.AdminCommon.AppLogic.ro_TXModeAuthOnly, "AUTH_ONLY", "AUTH_CAPTURE"));
            if (TransactionMode.ToLower().IndexOf("auth_only") > -1)
            {
                transactionCommand.Append("x_type=AUTH_ONLY");
            }
            else
            {
                transactionCommand.Append("x_type=AUTH_CAPTURE");
            }
            transactionCommand.Append("&x_login=" + X_Login);
            transactionCommand.Append("&x_tran_key=" + X_TranKey);
            transactionCommand.Append("&x_version=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_VERSION"));
            transactionCommand.Append("&x_test_request=FALSE");// + CommonLogic.IIF(useLiveTransactions, "FALSE", "TRUE"));
            transactionCommand.Append("&x_merchant_email=" + HttpContext.Current.Server.UrlEncode(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_Email")));
            transactionCommand.Append("&x_description=" + HttpContext.Current.Server.UrlEncode(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreName") + " Order " + OrderNumber.ToString()));

            transactionCommand.Append("&x_method=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_METHOD"));

            transactionCommand.Append("&x_delim_Data=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_DATA"));
            transactionCommand.Append("&x_delim_Char=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR"));
            transactionCommand.Append("&x_encap_char=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_ENCAP_CHAR"));
            transactionCommand.Append("&x_relay_response=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_RELAY_RESPONSE"));

            transactionCommand.Append("&x_email_customer=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_Email_CUSTOMER"));
            transactionCommand.Append("&x_recurring_billing=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_RECURRING_BILLING"));

            transactionCommand.Append("&x_amount=" + OrderTotal);
            transactionCommand.Append("&x_card_num=" + SecurityComponent.Decrypt(UseBillingAddress.CardNumber.ToString()));
            //transactionCommand.Append("&x_card_code=" + UseBillingAddress.CardVarificationCode.Trim());
            transactionCommand.Append("&x_card_code=");

            transactionCommand.Append("&x_exp_date=" + UseBillingAddress.CardExpirationMonth.ToString().PadLeft(2, '0') + UseBillingAddress.CardExpirationYear);
            transactionCommand.Append("&x_phone=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingPhone));
            transactionCommand.Append("&x_fax=");
            transactionCommand.Append("&x_customer_tax_id=");
            transactionCommand.Append("&x_cust_id=" + CustomerID.ToString());
            transactionCommand.Append("&x_invoice_num=" + OrderNumber.ToString());
            transactionCommand.Append("&x_email=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.Email));
            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            transactionCommand.Append("&x_first_name=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingFirstName));
            transactionCommand.Append("&x_last_name=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingLastName));
            transactionCommand.Append("&x_company=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCompany));
            transactionCommand.Append("&x_address=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingAddress1));
            transactionCommand.Append("&x_city=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCity));
            transactionCommand.Append("&x_state=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingState));
            transactionCommand.Append("&x_zip=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingZip));
            transactionCommand.Append("&x_country=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCountry));

            if (UseShippingAddress != null)
            {
                transactionCommand.Append("&x_ship_to_first_name=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingFirstName));
                transactionCommand.Append("&x_ship_to_last_name=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingLastName));
                transactionCommand.Append("&x_ship_to_company=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCompany));
                transactionCommand.Append("&x_ship_to_address=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingAddress1));
                transactionCommand.Append("&x_ship_to_city=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCity));
                transactionCommand.Append("&x_ship_to_state=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingState));
                transactionCommand.Append("&x_ship_to_zip=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingZip));
                transactionCommand.Append("&x_ship_to_country=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCountry));
            }

            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            if (CAVV.Length != 0 || ECI.Length != 0)
            {
                transactionCommand.Append("&x_authentication_indicator=" + ECI);
                transactionCommand.Append("&x_cardholder_authentication_value=" + CAVV);
            }

            byte[] data = encoding.GetBytes(transactionCommand.ToString());
            //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>TranscationCommand: " + transactionCommand.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
            // Prepare web request...
            try
            {

                String AuthServer = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_LIVE_SERVER");

                if (!useLiveTransactions)
                    AuthServer = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_TEST_SERVER");

                String rawResponseString = String.Empty;

                int MaxTries = Convert.ToInt32(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("GatewayRetries")) + 1;
                int CurrentTry = 0;
                bool CallSuccessful = false;
                do
                {
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    myRequest.Method = "POST";
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                    myRequest.ContentLength = data.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    // get the response
                    WebResponse myResponse;

                    CurrentTry++;
                    try
                    {
                        //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>Making Request..." + CurrentTry + " Time<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                        myResponse = myRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                        {
                            rawResponseString = sr.ReadToEnd();
                            sr.Close();
                        }
                        myResponse.Close();
                        CallSuccessful = true;
                        //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>Request successfully executed..." + CurrentTry + " Time<br/>Response: " + rawResponseString + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                    }
                    catch (Exception ex)
                    {
                        CallSuccessful = false;
                        //CommonOperation.TraceProcess("OrderProcessError", "Order " + OrderNumber + "<br/>Error while creating Request to AuthorizeNet." + CurrentTry + " Time<br/>Error Description: " + ex.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                        rawResponseString = "0|||Error Calling Authorize.Net Payment Gateway||||||||";
                    }
                }
                while (!CallSuccessful && CurrentTry < MaxTries);


                // rawResponseString now has gateway response
                TransactionResponse = rawResponseString;
                String[] statusArray = rawResponseString.Split(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR").ToCharArray());
                // this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
                for (int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
                {
                    statusArray[i] = statusArray[i].Trim('\"');
                }

                String sql = String.Empty;
                String replyCode = statusArray[0].Replace(":", "");
                String responseCode = statusArray[2];
                String approvalCode = statusArray[4];
                String authResponse = statusArray[3];
                String TransID = statusArray[6];

                AuthorizationCode = statusArray[4];
                AuthorizationResult = rawResponseString;
                AuthorizationTransID = statusArray[6];
                AVSResult = statusArray[5];
                TransactionCommandOut = transactionCommand.ToString().Replace(X_TranKey, "*".PadLeft(X_TranKey.Length));

                if (replyCode == "1")
                {
                    result = Solution.Bussines.Components.AdminCommon.AppLogic.ro_OK;
                  CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + OrderNumber + "");
                }
                else
                {
                    result = authResponse;
                    if (result.Length == 0)
                    {
                        result = "Unspecified Error";
                    }
                    else
                    {
                        result = result.Replace("account", "card");
                        result = result.Replace("Account", "Card");
                        result = result.Replace("ACCOUNT", "CARD");
                    }
                }
                //CommonOperation.TraceProcess("OrderProcessResult", "Order " + OrderNumber + "<br/>Return Value: " + authResponse.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
            }
            catch (Exception ex)
            {
                //CommonOperation.TraceProcess("OrderProcessResultError", "Order " + OrderNumber + "<br/>Error while processing result: " + ex.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                result = "Error calling Authorize.net gateway. Please retry your order in a few minutes or select another checkout payment option.";
            }
            return result;
        }

        /// <summary>
        /// Get Card Process For Admin Side Verification
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="OrderTotal">Decimal OrderTotal</param>
        /// <param name="useLiveTransactions">bool useLiveTransactions</param>
        /// <param name="TransactionMode">String TransactionMode</param>
        /// <param name="UseBillingAddress">tb_Order UseBillingAddress</param>
        /// <param name="UseShippingAddress">tb_Order UseShippingAddress</param>
        /// <param name="CAVV">String CAVV</param>
        /// <param name="ECI">String ECI</param>
        /// <param name="XID">String XID</param>
        /// <param name="AVSResult">String AVSResult</param>
        /// <param name="AuthorizationResult">String AuthorizationResult</param>
        /// <param name="AuthorizationCode">String AuthorizationCode</param>
        /// <param name="AuthorizationTransID">String AuthorizationTransID</param>
        /// <param name="TransactionCommandOut">String TransactionCommandOut</param>
        /// <param name="TransactionResponse">String TransactionResponse</param>
        /// <returns>Returns Card Verifications Details</returns>
        static public String ProcessCardForAdminSide(int OrderNumber, int CustomerID, Decimal OrderTotal, bool useLiveTransactions, String TransactionMode, tb_Order UseBillingAddress, tb_Order UseShippingAddress, String CAVV, String ECI, String XID, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut, out String TransactionResponse)
        {
            String result = AppLogic.ro_OK;
            AuthorizationCode = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;
            AVSResult = String.Empty;
            TransactionCommandOut = String.Empty;
            TransactionResponse = String.Empty;

            String X_Login = AppLogic.AppConfigs("AUTHORIZENET_X_LOGIN");
            if (X_Login.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_Login = reg.Read("AUTHORIZENET_X_LOGIN");
                reg = null;
            }

            String X_TranKey = AppLogic.AppConfigs("AUTHORIZENET_X_TRAN_KEY");
            if (X_TranKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_TranKey = reg.Read("AUTHORIZENET_X_TRAN_KEY");
                reg = null;
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);

            //transactionCommand.Append("x_type=" + CommonLogic.IIF(TransactionMode == AppLogic.ro_TXModeAuthOnly, "AUTH_ONLY", "AUTH_CAPTURE"));
            if (TransactionMode.ToLower().IndexOf("auth_only") > -1)
            {
                transactionCommand.Append("x_type=AUTH_ONLY");
            }
            else
            {
                transactionCommand.Append("x_type=AUTH_CAPTURE");
            }
            transactionCommand.Append("&x_login=" + X_Login);
            transactionCommand.Append("&x_tran_key=" + X_TranKey);
            transactionCommand.Append("&x_version=" + AppLogic.AppConfigs("AUTHORIZENET_X_VERSION"));
            transactionCommand.Append("&x_test_request=FALSE");// + CommonLogic.IIF(useLiveTransactions, "FALSE", "TRUE"));
            transactionCommand.Append("&x_merchant_email=" + HttpContext.Current.Server.UrlEncode(AppLogic.AppConfigs("AUTHORIZENET_X_Email")));
            transactionCommand.Append("&x_description=" + HttpContext.Current.Server.UrlEncode(AppLogic.AppConfigs("StoreName") + " Order " + OrderNumber.ToString()));

            transactionCommand.Append("&x_method=" + AppLogic.AppConfigs("AUTHORIZENET_X_METHOD"));

            transactionCommand.Append("&x_delim_Data=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_DATA"));
            transactionCommand.Append("&x_delim_Char=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR"));
            transactionCommand.Append("&x_encap_char=" + AppLogic.AppConfigs("AUTHORIZENET_X_ENCAP_CHAR"));
            transactionCommand.Append("&x_relay_response=" + AppLogic.AppConfigs("AUTHORIZENET_X_RELAY_RESPONSE"));

            transactionCommand.Append("&x_email_customer=" + AppLogic.AppConfigs("AUTHORIZENET_X_Email_CUSTOMER"));
            transactionCommand.Append("&x_recurring_billing=" + AppLogic.AppConfigs("AUTHORIZENET_X_RECURRING_BILLING"));

            transactionCommand.Append("&x_amount=" + OrderTotal);
            transactionCommand.Append("&x_card_num=" + UseBillingAddress.CardNumber.ToString());
            transactionCommand.Append("&x_card_code=" + UseBillingAddress.CardVarificationCode.Trim());

            transactionCommand.Append("&x_exp_date=" + UseBillingAddress.CardExpirationMonth.ToString().PadLeft(2, '0') + UseBillingAddress.CardExpirationYear);
            transactionCommand.Append("&x_phone=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingPhone));
            transactionCommand.Append("&x_fax=");
            transactionCommand.Append("&x_customer_tax_id=");
            transactionCommand.Append("&x_cust_id=" + CustomerID.ToString());
            transactionCommand.Append("&x_invoice_num=" + OrderNumber.ToString());
            transactionCommand.Append("&x_email=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.Email));
            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            transactionCommand.Append("&x_first_name=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingFirstName));
            transactionCommand.Append("&x_last_name=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingLastName));
            transactionCommand.Append("&x_company=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCompany));
            transactionCommand.Append("&x_address=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingAddress1));
            transactionCommand.Append("&x_city=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCity));
            transactionCommand.Append("&x_state=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingState));
            transactionCommand.Append("&x_zip=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingZip));
            transactionCommand.Append("&x_country=" + HttpContext.Current.Server.UrlEncode(UseBillingAddress.BillingCountry));

            if (UseShippingAddress != null)
            {
                transactionCommand.Append("&x_ship_to_first_name=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingFirstName));
                transactionCommand.Append("&x_ship_to_last_name=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingLastName));
                transactionCommand.Append("&x_ship_to_company=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCompany));
                transactionCommand.Append("&x_ship_to_address=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingAddress1));
                transactionCommand.Append("&x_ship_to_city=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCity));
                transactionCommand.Append("&x_ship_to_state=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingState));
                transactionCommand.Append("&x_ship_to_zip=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingZip));
                transactionCommand.Append("&x_ship_to_country=" + HttpContext.Current.Server.UrlEncode(UseShippingAddress.ShippingCountry));
            }

            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);

            if (CAVV.Length != 0 || ECI.Length != 0)
            {
                transactionCommand.Append("&x_authentication_indicator=" + ECI);
                transactionCommand.Append("&x_cardholder_authentication_value=" + CAVV);
            }

            byte[] data = encoding.GetBytes(transactionCommand.ToString());
            //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>TranscationCommand: " + transactionCommand.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
            // Prepare web request...
            try
            {

                String AuthServer = AppLogic.AppConfigs("AUTHORIZENET_LIVE_SERVER");

                if (!useLiveTransactions)
                    AuthServer = AppLogic.AppConfigs("AUTHORIZENET_TEST_SERVER");

                String rawResponseString = String.Empty;

                int MaxTries = Convert.ToInt32(AppLogic.AppConfigs("GatewayRetries")) + 1;
                int CurrentTry = 0;
                bool CallSuccessful = false;
                do
                {
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    myRequest.Method = "POST";
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                    myRequest.ContentLength = data.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    // get the response
                    WebResponse myResponse;

                    CurrentTry++;
                    try
                    {
                        //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>Making Request..." + CurrentTry + " Time<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                        myResponse = myRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                        {
                            rawResponseString = sr.ReadToEnd();
                            sr.Close();
                        }
                        myResponse.Close();
                        CallSuccessful = true;
                        //CommonOperation.TraceProcess("OrderProcess", "Order " + OrderNumber + "<br/>Request successfully executed..." + CurrentTry + " Time<br/>Response: " + rawResponseString + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                    }
                    catch (Exception ex)
                    {
                        CallSuccessful = false;
                        //CommonOperation.TraceProcess("OrderProcessError", "Order " + OrderNumber + "<br/>Error while creating Request to AuthorizeNet." + CurrentTry + " Time<br/>Error Description: " + ex.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                        rawResponseString = "0|||Error Calling Authorize.Net Payment Gateway||||||||";
                    }
                }
                while (!CallSuccessful && CurrentTry < MaxTries);


                // rawResponseString now has gateway response
                TransactionResponse = rawResponseString;
                String[] statusArray = rawResponseString.Split(AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR").ToCharArray());
                // this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
                for (int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
                {
                    statusArray[i] = statusArray[i].Trim('\"');
                }

                String sql = String.Empty;
                String replyCode = statusArray[0].Replace(":", "");
                String responseCode = statusArray[2];
                String approvalCode = statusArray[4];
                String authResponse = statusArray[3];
                String TransID = statusArray[6];

                AuthorizationCode = statusArray[4];
                AuthorizationResult = rawResponseString;
                AuthorizationTransID = statusArray[6];
                AVSResult = statusArray[5];
                TransactionCommandOut = transactionCommand.ToString().Replace(X_TranKey, "*".PadLeft(X_TranKey.Length));

                if (replyCode == "1")
                {
                    result = AppLogic.ro_OK;
                    CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + OrderNumber + "");
                }
                else
                {
                    result = authResponse;
                    if (result.Length == 0)
                    {
                        result = "Unspecified Error";
                    }
                    else
                    {
                        result = result.Replace("account", "card");
                        result = result.Replace("Account", "Card");
                        result = result.Replace("ACCOUNT", "CARD");
                    }
                }
                //CommonOperation.TraceProcess("OrderProcessResult", "Order " + OrderNumber + "<br/>Return Value: " + authResponse.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
            }
            catch (Exception ex)
            {
                //CommonOperation.TraceProcess("OrderProcessResultError", "Order " + OrderNumber + "<br/>Error while processing result: " + ex.ToString() + "<br/>Time: " + DateTime.Now.ToLongTimeString(), "AuthorizeNet");
                result = "Error calling Authorize.net gateway. Please retry your order in a few minutes or select another checkout payment option.";
            }
            return result;
        }

        /// <summary>
        ///  Get Status of Transaction where it is Capture Order or not
        /// </summary>
        /// <param name="o">tb_Order o</param>
        /// <returns>Returns Transaction status where it is Capture Order or not</returns>
        static public String CaptureOrder(tb_Order o)
        {
            String result = AppLogic.ro_OK;

            o.CaptureTxCommand = "";
            o.CaptureTXResult = "";
            bool useLiveTransactions = AppLogic.AppConfigBool("UseLiveTransactions");
            String TransID = o.AuthorizationPNREF;
            Decimal OrderTotal = Convert.ToDecimal(o.OrderTotal);


            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            transactionCommand.Append("x_type=PRIOR_AUTH_CAPTURE");

            String X_Login = AppLogic.AppConfigs("AUTHORIZENET_X_LOGIN");
            if (X_Login.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_Login = reg.Read("AUTHORIZENET_X_LOGIN");
                reg = null;
            }

            String X_TranKey = AppLogic.AppConfigs("AUTHORIZENET_X_TRAN_KEY");
            if (X_TranKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_TranKey = reg.Read("AUTHORIZENET_X_TRAN_KEY");
                reg = null;
            }

            transactionCommand.Append("&x_login=" + X_Login);
            transactionCommand.Append("&x_tran_key=" + X_TranKey);
            transactionCommand.Append("&x_version=" + AppLogic.AppConfigs("AUTHORIZENET_X_VERSION"));
            transactionCommand.Append("&x_test_request=FALSE");
            transactionCommand.Append("&x_method=" + AppLogic.AppConfigs("AUTHORIZENET_X_METHOD"));
            transactionCommand.Append("&x_delim_Data=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_DATA"));
            transactionCommand.Append("&x_delim_Char=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR"));
            transactionCommand.Append("&x_encap_char=" + AppLogic.AppConfigs("AUTHORIZENET_X_ENCAP_CHAR"));
            transactionCommand.Append("&x_relay_response=" + AppLogic.AppConfigs("AUTHORIZENET_X_RELAY_RESPONSE"));
            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            transactionCommand.Append("&x_trans_id=" + TransID);
            if (OrderTotal != System.Decimal.Zero)
            {
                // amount could have changed by admin user, so capture the current Order Total from the db:
                transactionCommand.Append("&x_amount=" + OrderTotal);
            }
            o.CaptureTxCommand = transactionCommand.ToString().Replace(X_TranKey, "*".PadLeft(X_TranKey.Length));

            try
            {
                byte[] data = encoding.GetBytes(transactionCommand.ToString());

                // Prepare web request...
                String AuthServer = AppLogic.AppConfigs("AUTHORIZENET_LIVE_SERVER");

                if (!useLiveTransactions)
                    AuthServer = AppLogic.AppConfigs("AUTHORIZENET_TEST_SERVER");

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                Stream newStream = myRequest.GetRequestStream();
                // Send the data.
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // get the response
                WebResponse myResponse;
                String rawResponseString = String.Empty;
                try
                {
                    myResponse = myRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                    {
                        rawResponseString = sr.ReadToEnd();
                        // Close and clean up the StreamReader
                        sr.Close();
                    }
                    myResponse.Close();
                }
                catch
                {
                    rawResponseString = "0|||Error Calling AuthorizeNet Payment Gateway||||||||";
                }

                // rawResponseString now has gateway response
                String[] statusArray = rawResponseString.Split(AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR").ToCharArray());
                // this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
                for (int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
                {
                    statusArray[i] = statusArray[i].Trim('\"');
                }

                String sql = String.Empty;
                String replyCode = statusArray[0];

                o.CaptureTXResult = rawResponseString;
                if (replyCode == "1")
                {
                    result = AppLogic.ro_OK;
                    //CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + o.OrderNumber + "");
                }
                else
                {
                    result = statusArray[3];
                }
            }
            catch
            {
                result = "NO RESPONSE FROM GATEWAY!";
            }
            return result;
        }
        static public String CaptureOrderFirst(tb_Order o)
        {
            String result = AppLogic.ro_OK;

            o.CaptureTxCommand = "";
            o.CaptureTXResult = "";
            bool useLiveTransactions = AppLogic.AppConfigBool("UseLiveTransactions");
            String TransID = o.AuthorizationPNREF;
            Decimal OrderTotal = Convert.ToDecimal(o.OrderTotal);

            Decimal AuthoRizeAmt = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT isnull(AuthorizedAmount,0) as AuthorizedAmount FROM tb_Order WHERE orderNumber=" + o.OrderNumber.ToString() + ""));

            if (AuthoRizeAmt > Convert.ToDecimal(0) && OrderTotal > AuthoRizeAmt)
            {
                OrderTotal = AuthoRizeAmt;
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            transactionCommand.Append("x_type=PRIOR_AUTH_CAPTURE");

            String X_Login = AppLogic.AppConfigs("AUTHORIZENET_X_LOGIN");
            if (X_Login.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_Login = reg.Read("AUTHORIZENET_X_LOGIN");
                reg = null;
            }

            String X_TranKey = AppLogic.AppConfigs("AUTHORIZENET_X_TRAN_KEY");
            if (X_TranKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_TranKey = reg.Read("AUTHORIZENET_X_TRAN_KEY");
                reg = null;
            }

            transactionCommand.Append("&x_login=" + X_Login);
            transactionCommand.Append("&x_tran_key=" + X_TranKey);
            transactionCommand.Append("&x_version=" + AppLogic.AppConfigs("AUTHORIZENET_X_VERSION"));
            transactionCommand.Append("&x_test_request=FALSE");
            transactionCommand.Append("&x_method=" + AppLogic.AppConfigs("AUTHORIZENET_X_METHOD"));
            transactionCommand.Append("&x_delim_Data=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_DATA"));
            transactionCommand.Append("&x_delim_Char=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR"));
            transactionCommand.Append("&x_encap_char=" + AppLogic.AppConfigs("AUTHORIZENET_X_ENCAP_CHAR"));
            transactionCommand.Append("&x_relay_response=" + AppLogic.AppConfigs("AUTHORIZENET_X_RELAY_RESPONSE"));
            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            transactionCommand.Append("&x_trans_id=" + TransID);
            if (OrderTotal != System.Decimal.Zero)
            {
                // amount could have changed by admin user, so capture the current Order Total from the db:
                transactionCommand.Append("&x_amount=" + OrderTotal);
            }
            o.CaptureTxCommand = transactionCommand.ToString().Replace(X_TranKey, "*".PadLeft(X_TranKey.Length));

            try
            {
                byte[] data = encoding.GetBytes(transactionCommand.ToString());

                // Prepare web request...
                String AuthServer = AppLogic.AppConfigs("AUTHORIZENET_LIVE_SERVER");

                if (!useLiveTransactions)
                    AuthServer = AppLogic.AppConfigs("AUTHORIZENET_TEST_SERVER");

                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                myRequest.Method = "POST";
                myRequest.ContentType = "application/x-www-form-urlencoded";
                myRequest.ContentLength = data.Length;
                Stream newStream = myRequest.GetRequestStream();
                // Send the data.
                newStream.Write(data, 0, data.Length);
                newStream.Close();
                // get the response
                WebResponse myResponse;
                String rawResponseString = String.Empty;
                try
                {
                    myResponse = myRequest.GetResponse();
                    using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                    {
                        rawResponseString = sr.ReadToEnd();
                        // Close and clean up the StreamReader
                        sr.Close();
                    }
                    myResponse.Close();
                }
                catch
                {
                    rawResponseString = "0|||Error Calling AuthorizeNet Payment Gateway||||||||";
                }

                // rawResponseString now has gateway response
                String[] statusArray = rawResponseString.Split(AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR").ToCharArray());
                // this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
                for (int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
                {
                    statusArray[i] = statusArray[i].Trim('\"');
                }

                String sql = String.Empty;
                String replyCode = statusArray[0];

                o.CaptureTXResult = rawResponseString;
                if (replyCode == "1")
                {
                    result = AppLogic.ro_OK;
                    //CommonComponent.ExecuteCommonData("Update tb_Order set CardVarificationCode='',CardNumber='' where OrderNumber=" + o.OrderNumber + "");
                }
                else
                {
                    result = statusArray[3];
                }
            }
            catch
            {
                result = "NO RESPONSE FROM GATEWAY!";
            }
            return result;
        }

        /// <summary>
        /// Get Void Order Details
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="AuthorizationPNREF">string AuthorizationPNREF</param>
        /// <returns>Returns Void Order</returns>
        static public String VoidOrder(int OrderNumber, string AuthorizationPNREF)
        {
            String result = AppLogic.ro_OK;
            CommonComponent.ExecuteCommonData("update tb_order set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber=" + OrderNumber.ToString());

            bool useLiveTransactions = AppLogic.AppConfigBool("UseLiveTransactions");
            String TransID = String.Empty;
            TransID = AuthorizationPNREF;

            String X_Login = AppLogic.AppConfigs("AUTHORIZENET_X_LOGIN");
            if (X_Login.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_Login = reg.Read("AUTHORIZENET_X_LOGIN");
                reg = null;
            }

            String X_TranKey = AppLogic.AppConfigs("AUTHORIZENET_X_TRAN_KEY");
            if (X_TranKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_TranKey = reg.Read("AUTHORIZENET_X_TRAN_KEY");
                reg = null;
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            transactionCommand.Append("x_type=VOID");
            transactionCommand.Append("&x_login=" + X_Login);
            transactionCommand.Append("&x_tran_key=" + X_TranKey);
            transactionCommand.Append("&x_version=" + AppLogic.AppConfigs("AUTHORIZENET_X_VERSION"));
            transactionCommand.Append("&x_test_request=FALSE");
            transactionCommand.Append("&x_method=" + AppLogic.AppConfigs("AUTHORIZENET_X_METHOD"));
            transactionCommand.Append("&x_delim_Data=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_DATA"));
            transactionCommand.Append("&x_delim_Char=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR"));
            transactionCommand.Append("&x_encap_char=" + AppLogic.AppConfigs("AUTHORIZENET_X_ENCAP_CHAR"));
            transactionCommand.Append("&x_relay_response=" + AppLogic.AppConfigs("AUTHORIZENET_X_RELAY_RESPONSE"));
            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            transactionCommand.Append("&x_trans_id=" + TransID);

            CommonComponent.ExecuteCommonData("update tb_order set VoidTXCommand=" + transactionCommand.ToString().Replace(X_TranKey, "*".PadLeft(X_TranKey.Length)) + " where OrderNumber=" + OrderNumber.ToString());

            if (TransID.Length == 0 || TransID == "0")
            {
                result = "Invalid or Empty Transaction ID";
            }
            else
            {
                try
                {
                    byte[] data = encoding.GetBytes(transactionCommand.ToString());

                    // Prepare web request...
                    //String AuthServer = CommonLogic.IIF(useLiveTransactions, AppLogic.AppConfig("AUTHORIZENET_LIVE_SERVER"), AppLogic.AppConfig("AUTHORIZENET_TEST_SERVER"));
                    String AuthServer = AppLogic.AppConfigs("AUTHORIZENET_LIVE_SERVER");

                    if (!useLiveTransactions)
                        AuthServer = AppLogic.AppConfigs("AUTHORIZENET_TEST_SERVER");

                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    myRequest.Method = "POST";
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                    myRequest.ContentLength = data.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    // get the response
                    WebResponse myResponse;
                    String rawResponseString = String.Empty;
                    try
                    {
                        myResponse = myRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                        {
                            rawResponseString = sr.ReadToEnd();
                            // Close and clean up the StreamReader
                            sr.Close();
                        }
                        myResponse.Close();
                    }
                    catch
                    {
                        rawResponseString = "0|||Error Calling Authorize.Net Payment Gateway||||||||";
                    }

                    // rawResponseString now has gateway response
                    String[] statusArray = rawResponseString.Split(AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR").ToCharArray());
                    // this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
                    for (int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
                    {
                        statusArray[i] = statusArray[i].Trim('\"');
                    }

                    String sql = String.Empty;
                    String replyCode = statusArray[0].Replace(":", "");

                    CommonComponent.ExecuteCommonData("update tb_order set VoidTXResult=" + rawResponseString + " where OrderNumber=" + OrderNumber.ToString());
                    if (replyCode == "1")
                    {
                        result = AppLogic.ro_OK;
                    }
                    else
                    {
                        result = statusArray[3];
                    }
                }
                catch
                {
                    result = "NO RESPONSE FROM GATEWAY!";
                }
            }
            return result;
        }

        /// <summary>
        /// Get Refund Order Status 
        /// if RefundAmount == 0.0M, then then ENTIRE order amount will be refunded!
        /// </summary>
        /// <param name="OriginalOrderNumber">int OriginalOrderNumber</param>
        /// <param name="RefundAmount">decimal RefundAmount</param>
        /// <param name="RefundReason">String RefundReason</param>
        /// <returns>Returns Order Status for Refund Order</returns>
        static public String RefundOrder(int OriginalOrderNumber, decimal RefundAmount, String RefundReason)
        {
            String result = AppLogic.ro_OK;
            CommonComponent.ExecuteCommonData("update tb_order set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber=" + OriginalOrderNumber.ToString());
            bool useLiveTransactions = AppLogic.AppConfigBool("UseLiveTransactions");
            String TransID = String.Empty;
            String Last4 = String.Empty;
            String CardNumber = String.Empty;
            int CustomerID = 0;
            Decimal OrderTotal = System.Decimal.Zero;
            String BillingLastName = String.Empty;
            String BillingFirstName = String.Empty;
            String BillingCompany = String.Empty;
            String BillingAddress1 = String.Empty;
            String BillingAddress2 = String.Empty;
            String BillingSuite = String.Empty;
            String BillingCity = String.Empty;
            String BillingState = String.Empty;
            String BillingZip = String.Empty;
            String BillingCountry = String.Empty;
            String BillingPhone = String.Empty;
            String BillingEMail = String.Empty;

            System.Data.DataSet ds = new System.Data.DataSet();
            ds = CommonComponent.GetCommonDataSet("select * from tb_order where OrderNumber=" + OriginalOrderNumber.ToString());
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToString(ds.Tables[0].Rows[0]["AuthorizationPNREF"].ToString()).ToUpper().IndexOf("CAPTURE") > -1)
                {
                    TransID = System.Text.RegularExpressions.Regex.Match(Convert.ToString(ds.Tables[0].Rows[0]["AuthorizationPNREF"].ToString()), "(?<=CAPTURE=)[0-9A-Z]+").ToString(); //rs["AuthorizationPNREF"].ToString().Replace("|");
                }
                else
                {
                    TransID = ds.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                }

                //TransID = ds.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Last4"].ToString()))
                {
                    Last4 = ds.Tables[0].Rows[0]["Last4"].ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CardNumber"].ToString()))
                    {
                        CardNumber = SecurityComponent.Decrypt(ds.Tables[0].Rows[0]["CardNumber"].ToString());
                        Last4 = CardNumber.Substring(CardNumber.Length - 4);
                    }
                }
                OrderTotal = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrderTotal"].ToString());
                CustomerID = Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerID"].ToString());
                BillingLastName = ds.Tables[0].Rows[0]["BillingLastName"].ToString();
                BillingFirstName = ds.Tables[0].Rows[0]["BillingFirstName"].ToString();
                BillingCompany = ds.Tables[0].Rows[0]["BillingCompany"].ToString();
                BillingAddress1 = ds.Tables[0].Rows[0]["BillingAddress1"].ToString();
                BillingAddress2 = ds.Tables[0].Rows[0]["BillingAddress2"].ToString();
                BillingSuite = ds.Tables[0].Rows[0]["BillingSuite"].ToString();
                BillingCity = ds.Tables[0].Rows[0]["BillingCity"].ToString();
                BillingState = ds.Tables[0].Rows[0]["BillingState"].ToString();
                BillingZip = ds.Tables[0].Rows[0]["BillingZip"].ToString();
                BillingCountry = ds.Tables[0].Rows[0]["BillingCountry"].ToString();
                BillingPhone = ds.Tables[0].Rows[0]["BillingPhone"].ToString();
                BillingEMail = ds.Tables[0].Rows[0]["EMail"].ToString();
            }

            String X_Login = AppLogic.AppConfigs("AUTHORIZENET_X_LOGIN");
            if (X_Login.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_Login = reg.Read("AUTHORIZENET_X_LOGIN");
                reg = null;
            }

            String X_TranKey = AppLogic.AppConfigs("AUTHORIZENET_X_TRAN_KEY");
            if (X_TranKey.Trim().ToUpperInvariant() == "REGISTRY")
            {
                WindowsRegistry reg = new WindowsRegistry(AppLogic.AppConfigs("EncryptKey.RegistryLocation"));
                X_TranKey = reg.Read("AUTHORIZENET_X_TRAN_KEY");
                reg = null;
            }

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            transactionCommand.Append("x_type=CREDIT");
            transactionCommand.Append("&x_login=" + X_Login);
            transactionCommand.Append("&x_tran_key=" + X_TranKey);
            transactionCommand.Append("&x_version=" + AppLogic.AppConfigs("AUTHORIZENET_X_VERSION"));
            transactionCommand.Append("&x_test_request=FALSE");// + CommonLogic.IIF(useLiveTransactions, "FALSE", "TRUE"));
            transactionCommand.Append("&x_method=" + AppLogic.AppConfigs("AUTHORIZENET_X_METHOD"));
            transactionCommand.Append("&x_delim_Data=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_DATA"));
            transactionCommand.Append("&x_delim_Char=" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR"));
            transactionCommand.Append("&x_encap_char=" + AppLogic.AppConfigs("AUTHORIZENET_X_ENCAP_CHAR"));
            transactionCommand.Append("&x_relay_response=" + AppLogic.AppConfigs("AUTHORIZENET_X_RELAY_RESPONSE"));
            transactionCommand.Append("&x_trans_id=" + TransID);
            if (RefundAmount == System.Decimal.Zero)
            {
                transactionCommand.Append("&x_amount=" + OrderTotal);
            }
            else
            {
                transactionCommand.Append("&x_amount=" + RefundAmount);
            }
            transactionCommand.Append("&x_cust_id=" + CustomerID.ToString());
            transactionCommand.Append("&x_invoice_num=" + OriginalOrderNumber.ToString());
            transactionCommand.Append("&x_email=" + HttpContext.Current.Server.UrlEncode(BillingEMail));
            transactionCommand.Append("&x_email_customer=false");
            transactionCommand.Append("&x_customer_ip=" + HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]);
            transactionCommand.Append("&x_card_num=" + Last4);

            transactionCommand.Append("&x_description=" + HttpContext.Current.Server.UrlEncode(RefundReason));
            transactionCommand.Append("&x_first_name=" + HttpContext.Current.Server.UrlEncode(BillingFirstName));
            transactionCommand.Append("&x_last_name=" + HttpContext.Current.Server.UrlEncode(BillingLastName));
            transactionCommand.Append("&x_company=" + HttpContext.Current.Server.UrlEncode(BillingCompany));
            transactionCommand.Append("&x_address=" + HttpContext.Current.Server.UrlEncode(BillingAddress1));
            transactionCommand.Append("&x_city=" + HttpContext.Current.Server.UrlEncode(BillingCity));
            transactionCommand.Append("&x_state=" + HttpContext.Current.Server.UrlEncode(BillingState));
            transactionCommand.Append("&x_zip=" + HttpContext.Current.Server.UrlEncode(BillingZip));
            transactionCommand.Append("&x_country=" + HttpContext.Current.Server.UrlEncode(BillingCountry));
            transactionCommand.Append("&x_phone=" + HttpContext.Current.Server.UrlEncode(BillingPhone));

            CommonComponent.ExecuteCommonData("update tb_order set RefundTXCommand='" + transactionCommand.ToString().Replace(X_TranKey, "*".PadLeft(X_TranKey.Length)).Replace("'", "''") + "' where OrderNumber=" + OriginalOrderNumber.ToString());

            if (TransID.Length == 0 || TransID == "0")
            {
                result = "Invalid or Empty Transaction ID";
            }
            else if (Last4.Length == 0)
            {
                result = "Credit Card Number (Last4) Not Found or Empty";
            }
            else
            {
                try
                {

                    byte[] data = encoding.GetBytes(transactionCommand.ToString());

                    // Prepare web request...
                    String AuthServer = AppLogic.AppConfigs("AUTHORIZENET_LIVE_SERVER");

                    if (!useLiveTransactions)
                        AuthServer = AppLogic.AppConfigs("AUTHORIZENET_TEST_SERVER");

                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    myRequest.Method = "POST";
                    myRequest.ContentType = "application/x-www-form-urlencoded";
                    myRequest.ContentLength = data.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    // get the response
                    WebResponse myResponse;
                    String rawResponseString = String.Empty;
                    try
                    {
                        myResponse = myRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                        {
                            rawResponseString = sr.ReadToEnd();
                            // Close and clean up the StreamReader
                            sr.Close();
                        }
                        myResponse.Close();
                    }
                    catch
                    {
                        rawResponseString = "0|||Error Calling Authorize.Net Payment Gateway||||||||";
                    }

                    // rawResponseString now has gateway response
                    String[] statusArray = rawResponseString.Split(AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR").ToCharArray());
                    // this seems to be a new item where auth.net is returing quotes around each parameter, so strip them out:
                    for (int i = statusArray.GetLowerBound(0); i <= statusArray.GetUpperBound(0); i++)
                    {
                        statusArray[i] = statusArray[i].Trim('\"');
                    }

                    String sql = String.Empty;
                    String replyCode = statusArray[0].Replace(":", "");
                    CommonComponent.ExecuteCommonData("update tb_order set RefundTXResult=" + rawResponseString + " where OrderNumber=" + OriginalOrderNumber.ToString());
                    if (replyCode == "1")
                    {
                        result = AppLogic.ro_OK;
                    }
                    else
                    {
                        result = statusArray[3];
                    }
                }
                catch
                {
                    result = "NO RESPONSE FROM GATEWAY!";
                }
            }
            return result;
        }
    }
}
