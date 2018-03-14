using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Components.Common;
using Solution.Data;
using System.Data;
using System.Web;
using System.Text.RegularExpressions;
using Solution.Bussines.Entities;
using System.Globalization;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// PayPal Component Class Contains PayPal related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class PayPalComponent
    {

        //public const String BN = "Kaushalam" + "Digital" + "_Cart"; // Do not change this line or your PayPal website calls may not work!
        //private const String API_VER = "3.0";
        //private PayPalAPISoapBinding IPayPalRefund;
        //private PayPalAPIAASoapBinding IPayPal;
        private CultureInfo USCulture = new CultureInfo("en-US");
        private Regex regInteger = new Regex(@"^-?\d+$");
        ////Express checkout
        //public BasicAmountType ECOrderTotal = new BasicAmountType();
        //public AddressType ECShippingAddress = new AddressType();

        //// DoDirectPaymentRequest
        //public DoDirectPaymentRequestType PaymentRequest;
        //public DoDirectPaymentReq DDPReq;
        //public TransactionSearchReq request;


        //public const String BN = "AspDotNet" + "Storefront" + "_Cart"; // Do not change this line or your paypal website calls may not work!
        public const String BN = "Kaushalam" + "Digital" + "_Cart"; // Do not change this line or your paypal website calls may not work!
        private const String API_VER = "3.0";
        private PayPalAPISoapBinding IPayPalRefund;
        private PayPalAPIAASoapBinding IPayPal;

        //Express checkout
        public BasicAmountType ECOrderTotal = new BasicAmountType();
        public AddressType ECShippingAddress = new AddressType();

        // DoDirectPaymentRequest
        public DoDirectPaymentRequestType PaymentRequest;
        public DoDirectPaymentReq DDPReq;
        public TransactionSearchReq request;



        public PayPalComponent()
        {
            //IPayPal = new PayPalAPIAASoapBinding();
            //IPayPalRefund = new PayPalAPISoapBinding();

            //if (AppLogic.AppConfigBool("UseLiveTransactions"))
            //{
            //    IPayPal.Url = AppLogic.AppConfigs("PayPal.API.LiveURL");
            //}
            //else
            //{
            //    IPayPal.Url = AppLogic.AppConfigs("PayPal.API.TestURL");
            //}
            //IPayPalRefund.Url = IPayPal.Url;

            //IPayPal.UserAgent = HttpContext.Current.Request.UserAgent;
            //IPayPalRefund.UserAgent = IPayPal.UserAgent;

            //UserIdPasswordType PayPalUser = new UserIdPasswordType();
            //PayPalUser.Username = AppLogic.AppConfigs("PayPal.API.Username");
            //PayPalUser.Password = AppLogic.AppConfigs("PayPal.API.Password");
            //PayPalUser.Signature = AppLogic.AppConfigs("PayPal.API.Signature");

            ////Subject should be the Sellers e-mail address (if you are using 3-part API calls) with the correct account permissions. You also have 
            ////set up permissions for this e-mail address for the "type" of transaction you want to allow.
            ////This access changes are made in the Sandbox.
            ////The name of the entity on behalf of which this profile is issuing calls
            ////This is for Third-Party access
            //// You have to set up Virtual Terminals and complete the Billing Agreement in the Sandbox before you can make Direct Payments
            //PayPalUser.Subject = AppLogic.AppConfigs("PayPal.API.MerchantEMailAddress");

            //CustomSecurityHeaderType CSecHeaderType = new CustomSecurityHeaderType();
            //CSecHeaderType.Credentials = PayPalUser;
            //CSecHeaderType.MustUnderstand = true;

            //IPayPal.RequesterCredentials = CSecHeaderType;
            //IPayPalRefund.RequesterCredentials = CSecHeaderType;




            IPayPal = new PayPalAPIAASoapBinding();
            IPayPalRefund = new PayPalAPISoapBinding();

            if (AppLogic.AppConfigBool("UseLiveTransactions"))
            {
                IPayPal.Url = AppLogic.AppConfigs("PayPal.API.LiveURL");
            }
            else
            {
                IPayPal.Url = AppLogic.AppConfigs("PayPal.API.TestURL");
            }
            IPayPalRefund.Url = IPayPal.Url;

            IPayPal.UserAgent = HttpContext.Current.Request.UserAgent;
            IPayPalRefund.UserAgent = IPayPal.UserAgent;

            UserIdPasswordType PayPalUser = new UserIdPasswordType();
            PayPalUser.Username = AppLogic.AppConfigs("PayPal.API.Username");
            PayPalUser.Password = AppLogic.AppConfigs("PayPal.API.Password");
            PayPalUser.Signature = AppLogic.AppConfigs("PayPal.API.Signature");

            //Subject should be the Sellers e-mail address (if you are using 3-part API calls) with the correct account permissions. You also have 
            //set up permissions for this e-mail address for the "type" of transaction you want to allow.
            //This access changes are made in the Sandbox.
            //The name of the entity on behalf of which this profile is issuing calls
            //This is for Third-Party access
            // You have to set up Virtual Terminals and complete the Billing Agreement in the Sandbox before you can make Direct Payments
            PayPalUser.Subject = AppLogic.AppConfigs("PayPal.API.MerchantEMailAddress");

            CustomSecurityHeaderType CSecHeaderType = new CustomSecurityHeaderType();
            CSecHeaderType.Credentials = PayPalUser;
            CSecHeaderType.MustUnderstand = true;

            IPayPal.RequesterCredentials = CSecHeaderType;
            IPayPalRefund.RequesterCredentials = CSecHeaderType;


        }

        /// <summary>
        /// Capture Order for PayPal
        /// </summary>
        /// <param name="o">tb_Order o</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String CaptureOrder(tb_Order o)
        {
            String result = String.Empty;

            o.CaptureTxCommand = "";
            o.CaptureTXResult = "";

            // check for ReauthorizationID first, if doesn't exist, use original AuthorizationID
            String TransID = Regex.Match(o.AuthorizationPNREF, "(?<=REAU=)[0-9A-Z]+").ToString();
            if (TransID.Length == 0)
            {
                TransID = Regex.Match(o.AuthorizationPNREF, "(?<=AUTH=)[0-9A-Z]+").ToString();
            }

            Decimal OrderTotal = Convert.ToDecimal(o.OrderTotal.ToString());

            if (TransID.Length == 0 || TransID == "0")
            {
                result = "Invalid or Empty Transaction ID";
            }
            else
            {
                try
                {
                    DoCaptureReq CaptureReq = new DoCaptureReq();
                    DoCaptureRequestType CaptureRequestType = new DoCaptureRequestType();
                    DoCaptureResponseType CaptureResponse;

                    BasicAmountType totalAmount = new BasicAmountType();
                    totalAmount.Value = String.Format("{0:0.00}", OrderTotal);
                    totalAmount.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), AppLogic.AppConfigs("Localization.StoreCurrency"), true);

                    CaptureRequestType.Amount = totalAmount;
                    CaptureRequestType.AuthorizationID = TransID;

                    CaptureRequestType.CompleteType = CompleteCodeType.Complete;
                    CaptureRequestType.Version = API_VER;

                    CaptureReq.DoCaptureRequest = CaptureRequestType;

                    o.CaptureTxCommand = XmlCommonComponent.SerializeObject(CaptureReq, CaptureReq.GetType()); //"Not Available For PayPal";

                    CaptureResponse = (DoCaptureResponseType)IPayPal.DoCapture(CaptureReq);

                    o.CaptureTXResult = XmlCommonComponent.SerializeObject(CaptureResponse, CaptureResponse.GetType());

                    if (CaptureResponse.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = AppLogic.ro_OK;
                        String CaptureTransID = CaptureResponse.DoCaptureResponseDetails.PaymentInfo.TransactionID;
                        o.AuthorizationPNREF = o.AuthorizationPNREF + "|CAPTURE=" + CaptureTransID;
                    }
                    else
                    {
                        if (CaptureResponse.Errors != null)
                        {
                            bool first = true;
                            for (int ix = 0; ix < CaptureResponse.Errors.Length; ix++)
                            {
                                if (!first)
                                {
                                    result += ", ";
                                }
                                result += "Error: [" + CaptureResponse.Errors[ix].ErrorCode + "] " + CaptureResponse.Errors[ix].LongMessage;
                                first = false;
                            }
                        }
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
        /// Void Order for PayPal
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="AuthorizationPNREF">string AuthorizationPNREF</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String VoidOrder(int OrderNumber, string AuthorizationPNREF)
        {
            String result = String.Empty;



            CommonComponent.ExecuteCommonData("update tb_order set VoidTXCommand=NULL, VoidTXResult=NULL where OrderNumber=" + OrderNumber.ToString());
            String TransID = String.Empty;
            TransID = AuthorizationPNREF;

            // use the ID from the original authorization, and not the reauthorization.
            TransID = Regex.Match(TransID, "(?<=AUTH=)[0-9A-Z]+").ToString();
            if (TransID.Length == 0 || TransID == "0")
            {
                result = "Invalid or Empty Transaction ID";
            }
            else
            {
                try
                {

                    DoVoidReq VoidReq = new DoVoidReq();
                    DoVoidRequestType VoidRequestType = new DoVoidRequestType();
                    DoVoidResponseType VoidResponse;

                    VoidRequestType.AuthorizationID = TransID;
                    VoidRequestType.Version = API_VER;

                    VoidReq.DoVoidRequest = VoidRequestType;

                    VoidResponse = (DoVoidResponseType)IPayPal.DoVoid(VoidReq);

                    CommonComponent.ExecuteCommonData("update tb_order set VoidTXCommand=" +
                         SQuote(XmlCommonComponent.SerializeObject(VoidReq, VoidReq.GetType())
                        )
                        + ", VoidTXResult=" + SQuote(XmlCommonComponent.SerializeObject(VoidResponse, VoidResponse.GetType())) + " where OrderNumber=" + OrderNumber.ToString());

                    if (VoidResponse.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = AppLogic.ro_OK;
                    }
                    else
                    {
                        if (VoidResponse.Errors != null)
                        {
                            bool first = true;
                            for (int ix = 0; ix < VoidResponse.Errors.Length; ix++)
                            {
                                if (!first)
                                {
                                    result += ", ";
                                }
                                result += "Error: [" + VoidResponse.Errors[ix].ErrorCode + "] " + VoidResponse.Errors[ix].LongMessage;
                                first = false;
                            }
                        }
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
        /// Refund Order for PayPal
        /// </summary>
        /// <param name="OriginalOrderNumber">int OriginalOrderNumber</param>
        /// <param name="RefundAmount">decimal RefundAmount</param>
        /// <param name="RefundReason">decimal RefundReason</param>
        /// <param name="TransactionID">string TransactionID</param>
        /// <param name="OrderTotalAmount">decimal OrderTotalAmount</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String RefundOrder(int OriginalOrderNumber, decimal RefundAmount, String RefundReason, string TransactionID, decimal OrderTotalAmount)
        {
            // if RefundAmount == 0.0M, then then ENTIRE order amount will be refunded!


            String result = String.Empty;
            // CommonComponent.ExecuteCommonData("update tb_order set RefundTXCommand=NULL, RefundTXResult=NULL where OrderNumber=" + OriginalOrderNumber.ToString());
            String TransID = TransactionID;
            Decimal OrderTotal = OrderTotalAmount;
            DataSet dsorder = new DataSet();
            TransID = Regex.Match(Convert.ToString(TransID), "(?<=CAPTURE=)[0-9A-Z]+").ToString();
            if (TransID.Length == 0 || TransID == "0")
            {
                result = "Invalid or Empty Transaction ID";
            }
            else
            {
                try
                {

                    RefundTransactionReq RefundReq = new RefundTransactionReq();
                    RefundTransactionRequestType RefundRequestType = new RefundTransactionRequestType();
                    RefundTransactionResponseType RefundResponse;
                    BasicAmountType BasicAmount = new BasicAmountType();

                    RefundRequestType.TransactionID = TransID;
                    RefundRequestType.Version = API_VER;

                    BasicAmount.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), AppLogic.AppConfigs("Localization.StoreCurrency"), true);

                    //If partial refund set value ( like 1.95). If FULL Refund leave it empty. The transactionID will take care of the amount
                    if (OrderTotal == RefundAmount || RefundAmount == 0.0M)
                    {
                        RefundRequestType.RefundType = RefundType.Full;
                    }
                    else
                    {
                        BasicAmount.Value = Convert.ToString(RefundAmount);
                        RefundRequestType.RefundType = RefundType.Partial;
                    }
                    RefundRequestType.Amount = BasicAmount;
                    RefundRequestType.RefundTypeSpecified = true;

                    if (!String.IsNullOrEmpty(RefundReason))
                    {
                        RefundRequestType.Memo = RefundReason;
                    }

                    RefundReq.RefundTransactionRequest = RefundRequestType;

                    CommonComponent.ExecuteCommonData("update tb_order set RefundTXCommand=isnull(RefundTXCommand,'')+" + SQuote(XmlCommonComponent.SerializeObject(RefundRequestType, RefundRequestType.GetType())) + " where OrderNumber=" + OriginalOrderNumber.ToString());

                    RefundResponse = (RefundTransactionResponseType)IPayPalRefund.RefundTransaction(RefundReq);

                    CommonComponent.ExecuteCommonData("update tb_order set RefundTXCommand=isnull(RefundTXCommand,'')+" + SQuote(XmlCommonComponent.SerializeObject(RefundReq, RefundReq.GetType()))
                        + ", RefundTXResult=isnull(RefundTXResult,'')+" + SQuote(XmlCommonComponent.SerializeObject(RefundResponse, RefundResponse.GetType())) + " where OrderNumber=" + OriginalOrderNumber.ToString());

                    String RefundTXResult = String.Empty;
                    if (RefundResponse.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = AppLogic.ro_OK;
                        String RefundTransID = RefundResponse.RefundTransactionID;
                        CommonComponent.ExecuteCommonData("update tb_order set AuthorizationPNREF=AuthorizationPNREF+'|REFUND=" + RefundTransID + "' where OrderNumber=" + OriginalOrderNumber.ToString());
                    }
                    else
                    {
                        if (RefundResponse.Errors != null)
                        {
                            bool first = true;
                            for (int ix = 0; ix < RefundResponse.Errors.Length; ix++)
                            {
                                if (!first)
                                {
                                    result += ", ";
                                }
                                result += "Error: [" + RefundResponse.Errors[ix].ErrorCode + "] " + RefundResponse.Errors[ix].LongMessage;
                                first = false;
                            }
                        }
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
        /// ParseUSInt
        /// </summary>
        /// <param name="s">String s</param>
        /// <returns>Returns Integer</returns>
        private int ParseUSInt(String s)
        {
            try
            {
                if (regInteger.IsMatch(s))
                {
                    return System.Int32.Parse(s, USCulture);
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// Process Card For Client Side
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="OrderTotal">decimal OrderTotal</param>
        /// <param name="useLiveTransactions">bool useLiveTransactions</param>
        /// <param name="TransactionMode">string TransactionMode</param>
        /// <param name="UseBillingAddress">tb_Order UseBillingAddress</param>
        /// <param name="CardExtraCode">string CardExtraCode</param>
        /// <param name="UseShippingAddress">tb_Order UseShippingAddress</param>
        /// <param name="CAVV">String CAVV</param>
        /// <param name="ECI">string ECI</param>
        /// <param name="XID">string XID</param>
        /// <param name="AVSResult">string AVSResult</param>
        /// <param name="AuthorizationResult">string AuthorizationResult</param>
        /// <param name="AuthorizationCode">string AuthorizationCode</param>
        /// <param name="AuthorizationTransID">string AuthorizationTransID</param>
        /// <param name="TransactionCommandOut">string TransactionCommandOut</param>
        /// <param name="TransactionResponse">string TransactionResponse</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String ProcessCardForClientSide(int OrderNumber, int CustomerID, Decimal OrderTotal, bool useLiveTransactions, String TransactionMode, tb_Order UseBillingAddress, String CardExtraCode, tb_Order UseShippingAddress, String CAVV, String ECI, String XID, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut, out String TransactionResponse)
        {
            String result = AppLogic.ro_OK;
            AuthorizationCode = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;
            AVSResult = String.Empty;
            TransactionCommandOut = String.Empty;
            TransactionResponse = String.Empty;
            try
            {
                // the request details object contains all payment details 
                DoDirectPaymentRequestDetailsType RequestDetails = new DoDirectPaymentRequestDetailsType();

                // define the payment action to 'Sale'
                // (another option is 'Authorization', which would be followed later with a DoCapture API call)
                RequestDetails.PaymentAction = (PaymentActionCodeType)(TransactionMode.ToLower().IndexOf("auth") > -1 ? (int)PaymentActionCodeType.Authorization : (int)PaymentActionCodeType.Sale);

                // define the total amount and currency for the transaction
                PaymentDetailsType PaymentDetails = new PaymentDetailsType();

                BasicAmountType totalAmount = new BasicAmountType();
                totalAmount.Value = string.Format("{0:0.00}", OrderTotal);
                totalAmount.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), AppLogic.AppConfigs("Localization.StoreCurrency"), true);
                PaymentDetails.OrderTotal = totalAmount;
                PaymentDetails.InvoiceID = OrderNumber.ToString();
                PaymentDetails.ButtonSource = PayPalComponent.BN + "_DP_US";
                PaymentDetails.OrderDescription = AppLogic.AppConfigs("StoreName");

                // define the credit card to be used
                CreditCardDetailsType creditCard = new CreditCardDetailsType();
                creditCard.CreditCardNumber = UseBillingAddress.CardNumber;
                creditCard.ExpMonth = ParseUSInt(Convert.ToString(UseBillingAddress.CardExpirationMonth));
                creditCard.ExpYear = ParseUSInt(Convert.ToString(UseBillingAddress.CardExpirationYear));
                if (CardExtraCode != "")
                    creditCard.CVV2 = CardExtraCode;
                else
                    creditCard.CVV2 = UseBillingAddress.CardVarificationCode;

                string Cardtype = UseBillingAddress.CardType.ToString();
                if (Cardtype.ToLower() == "master card")
                    Cardtype = "MasterCard";

                creditCard.CreditCardType = (CreditCardTypeType)Enum.Parse(typeof(CreditCardTypeType), Cardtype, true);
                PayerInfoType cardHolder = new PayerInfoType();
                PersonNameType oPersonNameType = new PersonNameType();
                oPersonNameType.FirstName = UseBillingAddress.BillingFirstName;
                oPersonNameType.LastName = UseBillingAddress.BillingLastName;
                oPersonNameType.MiddleName = String.Empty;
                oPersonNameType.Salutation = String.Empty;
                oPersonNameType.Suffix = String.Empty;
                cardHolder.PayerName = oPersonNameType;

                AddressType PayerAddress = new AddressType();
                PayerAddress.Street1 = UseBillingAddress.BillingAddress1;
                PayerAddress.CityName = UseBillingAddress.BillingCity;
                PayerAddress.StateOrProvince = UseBillingAddress.BillingState;
                PayerAddress.PostalCode = UseBillingAddress.BillingZip;

                string strCountry = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 TwoLetterISOCode FROM tb_Country WHERE Name='" + UseBillingAddress.BillingCountry.ToString() + "'"));
                PayerAddress.Country = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), strCountry, true);
                PayerAddress.CountrySpecified = true;

                if (UseShippingAddress != null)
                {
                    AddressType shippingAddress = new AddressType();
                    shippingAddress.Name = (UseShippingAddress.FirstName + " " + UseShippingAddress.LastName).Trim();
                    shippingAddress.Street1 = UseShippingAddress.ShippingAddress1;
                    shippingAddress.Street2 = UseShippingAddress.ShippingAddress2 + (UseShippingAddress.ShippingSuite != "" ? " Ste " + UseShippingAddress.ShippingSuite : "");
                    shippingAddress.CityName = UseShippingAddress.ShippingCity;
                    shippingAddress.StateOrProvince = UseShippingAddress.ShippingState;
                    shippingAddress.PostalCode = UseShippingAddress.ShippingZip;
                    string strCountryShipp = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 TwoLetterISOCode FROM tb_Country WHERE Name='" + UseShippingAddress.ShippingCountry.ToString() + "'"));
                    shippingAddress.Country = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), strCountryShipp, true);
                    shippingAddress.CountrySpecified = true;
                    PaymentDetails.ShipToAddress = shippingAddress;
                }

                cardHolder.Address = PayerAddress;
                creditCard.CardOwner = cardHolder;

                RequestDetails.CreditCard = creditCard;
                RequestDetails.PaymentDetails = PaymentDetails;
                RequestDetails.IPAddress = "120.72.91.26";//HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; // cart.ThisCustomer.LastIPAddress;"120.72.91.26";//
                RequestDetails.PaymentAction = (PaymentActionCodeType)(TransactionMode.ToLower().IndexOf("auth") > -1 ? (int)PaymentActionCodeType.Authorization : (int)PaymentActionCodeType.Sale);

                // instantiate the actual request object
                PaymentRequest = new DoDirectPaymentRequestType();
                PaymentRequest.Version = API_VER;
                PaymentRequest.DoDirectPaymentRequestDetails = RequestDetails;
                DDPReq = new DoDirectPaymentReq();
                DDPReq.DoDirectPaymentRequest = PaymentRequest;

                DoDirectPaymentResponseType responseDetails = (DoDirectPaymentResponseType)IPayPal.DoDirectPayment(DDPReq);

                if (responseDetails.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
                {
                    AuthorizationTransID = (TransactionMode.ToLower().IndexOf("auth") > -1 ? "AUTH=" : "CAPTURE=") + responseDetails.TransactionID.ToString();
                    AuthorizationCode = responseDetails.CorrelationID;
                    AVSResult = responseDetails.AVSCode;
                    result = AppLogic.ro_OK;
                    AuthorizationResult = responseDetails.Ack.ToString() + "|AVSCode=" + responseDetails.AVSCode.ToString() + "|CVV2Code=" + responseDetails.CVV2Code.ToString();
                }
                else
                {
                    if (responseDetails.Errors != null)
                    {
                        String Separator = String.Empty;
                        for (int ix = 0; ix < responseDetails.Errors.Length; ix++)
                        {
                            AuthorizationResult += Separator;
                            AuthorizationResult += responseDetails.Errors[ix].LongMessage;// record failed TX
                            TransactionResponse += Separator;
                            try
                            {
                                TransactionResponse += String.Format("|{0},{1},{2}|", responseDetails.Errors[ix].ShortMessage, responseDetails.Errors[ix].ErrorCode, responseDetails.Errors[ix].LongMessage); // record failed TX
                            }
                            catch { }
                            Separator = ", ";
                        }
                    }
                    result = AuthorizationResult;
                    // just store something here, as there is no other way to get data out of this gateway about the failure for logging in failed transaction table
                }
            }
            catch (Exception ex)
            {
                result = ex.Message.ToString();
            }
            return result;
        }


        /// <summary>
        /// Process PayPal
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="OrderTotal">decimal OrderTotal</param>
        /// <param name="useLiveTransactions">bool useLiveTransactions</param>
        /// <param name="TransactionMode">string TransactionMode</param>
        /// <param name="UseBillingAddress">tb_Order UseBillingAddress</param>
        /// <param name="UseShippingAddress">tb_Order UseShippingAddress</param>
        /// <param name="CAVV">String CAVV</param>
        /// <param name="ECI">string ECI</param>
        /// <param name="XID">string XID</param>
        /// <param name="AVSResult">string AVSResult</param>
        /// <param name="AuthorizationResult">string AuthorizationResult</param>
        /// <param name="AuthorizationCode">string AuthorizationCode</param>
        /// <param name="AuthorizationTransID">string AuthorizationTransID</param>
        /// <param name="TransactionCommandOut">string TransactionCommandOut</param>
        /// <param name="TransactionResponse">string TransactionResponse</param>
        /// <returns>Returns the result as a String according to execution</returns>
        static public String ProcessPaypal(int OrderNumber, int CustomerID, Decimal OrderTotal, bool useLiveTransactions, String TransactionMode, tb_Order UseBillingAddress, tb_Order UseShippingAddress, String CAVV, String ECI, String XID, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut, out String TransactionResponse)
        {
            // ProcessPaypal() is used for Express Checkout and PayPal payments.
            // Credit Card processing via Website Payments Pro is handled by ProcessCard(),
            // just like other credit card gateways.

            String result = AppLogic.ro_OK;

            AuthorizationCode = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;
            if (!String.IsNullOrEmpty(XID))
            {
                AuthorizationTransID = (TransactionMode.ToLower().IndexOf("auth") > -1 ? "AUTH=" : "CAPTURE=") + XID;
            }
            AVSResult = String.Empty;
            TransactionCommandOut = String.Empty;
            TransactionResponse = String.Empty;
            return result;
        }



        /// <summary>
        /// Process Card for PayPal
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="OrderTotal">decimal OrderTotal</param>
        /// <param name="useLiveTransactions">bool useLiveTransactions</param>
        /// <param name="TransactionMode">string TransactionMode</param>
        /// <param name="UseBillingAddress">tb_Order UseBillingAddress</param>
        /// <param name="CardExtraCode">string CardExtraCode</param>
        /// <param name="UseShippingAddress">tb_Order UseShippingAddress</param>
        /// <param name="CAVV">String CAVV</param>
        /// <param name="ECI">string ECI</param>
        /// <param name="XID">string XID</param>
        /// <param name="AVSResult">string AVSResult</param>
        /// <param name="AuthorizationResult">string AuthorizationResult</param>
        /// <param name="AuthorizationCode">string AuthorizationCode</param>
        /// <param name="AuthorizationTransID">string AuthorizationTransID</param>
        /// <param name="TransactionCommandOut">string TransactionCommandOut</param>
        /// <param name="TransactionResponse">string TransactionResponse</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String ProcessCard(int OrderNumber, int CustomerID, Decimal OrderTotal, bool useLiveTransactions, String TransactionMode, tb_Order UseBillingAddress, String CardExtraCode, tb_Order UseShippingAddress, String CAVV, String ECI, String XID, out String AVSResult, out String AuthorizationResult, out String AuthorizationCode, out String AuthorizationTransID, out String TransactionCommandOut, out String TransactionResponse)
        {
            // ProcessCard() is used for Credit Card processing via Website Payments Pro,
            // just like other credit card gateways.
            // ProcessPaypal() is used for Express Checkout and PayPal payments.

            String result = AppLogic.ro_OK;

            AuthorizationCode = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;
            AVSResult = String.Empty;
            TransactionCommandOut = String.Empty;
            TransactionResponse = String.Empty;

            // the request details object contains all payment details 
            DoDirectPaymentRequestDetailsType RequestDetails = new DoDirectPaymentRequestDetailsType();

            // define the payment action to 'Sale'
            // (another option is 'Authorization', which would be followed later with a DoCapture API call)
            RequestDetails.PaymentAction = (PaymentActionCodeType)(TransactionMode.ToLower().IndexOf("auth") > -1 ? (int)PaymentActionCodeType.Authorization : (int)PaymentActionCodeType.Sale);

            // define the total amount and currency for the transaction
            PaymentDetailsType PaymentDetails = new PaymentDetailsType();

            BasicAmountType totalAmount = new BasicAmountType();
            totalAmount.Value = Convert.ToString(OrderTotal);
            totalAmount.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), AppLogic.AppConfigs("Localization.StoreCurrency"), true);
            PaymentDetails.OrderTotal = totalAmount;
            PaymentDetails.InvoiceID = OrderNumber.ToString();
            PaymentDetails.ButtonSource = PayPalComponent.BN + "_DP_US";
            PaymentDetails.OrderDescription = AppLogic.AppConfigs("StoreName");

            // define the credit card to be used
            CreditCardDetailsType creditCard = new CreditCardDetailsType();
            creditCard.CreditCardNumber = UseBillingAddress.CardNumber;
            creditCard.ExpMonth = ParseUSInt(Convert.ToString(UseBillingAddress.CardExpirationMonth));
            creditCard.ExpYear = ParseUSInt(Convert.ToString(UseBillingAddress.CardExpirationYear));
            creditCard.CVV2 = CardExtraCode;
            creditCard.CreditCardType = (CreditCardTypeType)Enum.Parse(typeof(CreditCardTypeType), UseBillingAddress.CardType, true);
            PayerInfoType cardHolder = new PayerInfoType();
            PersonNameType oPersonNameType = new PersonNameType();
            oPersonNameType.FirstName = UseBillingAddress.BillingFirstName;
            oPersonNameType.LastName = UseBillingAddress.BillingLastName;
            oPersonNameType.MiddleName = String.Empty;
            oPersonNameType.Salutation = String.Empty;
            oPersonNameType.Suffix = String.Empty;
            cardHolder.PayerName = oPersonNameType;

            AddressType PayerAddress = new AddressType();
            PayerAddress.Street1 = UseBillingAddress.BillingAddress1;
            PayerAddress.CityName = UseBillingAddress.BillingCity;
            PayerAddress.StateOrProvince = UseBillingAddress.BillingState;
            PayerAddress.PostalCode = UseBillingAddress.BillingZip;
            CountryComponent objCountry = new CountryComponent();
            PayerAddress.Country = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), objCountry.GetCountryCodeByName(UseBillingAddress.BillingCountry), true);
            PayerAddress.CountrySpecified = true;

            if (UseShippingAddress != null)
            {
                AddressType shippingAddress = new AddressType();
                shippingAddress.Name = (UseShippingAddress.ShippingFirstName + " " + UseShippingAddress.ShippingLastName).Trim();
                shippingAddress.Street1 = UseShippingAddress.ShippingAddress1;
                shippingAddress.Street2 = UseShippingAddress.ShippingAddress2 + (UseShippingAddress.ShippingSuite != "" ? " Ste " + UseShippingAddress.ShippingSuite : "");
                shippingAddress.CityName = UseShippingAddress.ShippingCity;
                shippingAddress.StateOrProvince = UseShippingAddress.ShippingState;
                shippingAddress.PostalCode = UseShippingAddress.ShippingZip;
                shippingAddress.Country = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), objCountry.GetCountryCodeByName(UseShippingAddress.ShippingCountry), true);
                shippingAddress.CountrySpecified = true;
                PaymentDetails.ShipToAddress = shippingAddress;
            }

            cardHolder.Address = PayerAddress;
            creditCard.CardOwner = cardHolder;

            RequestDetails.CreditCard = creditCard;
            RequestDetails.PaymentDetails = PaymentDetails;
            RequestDetails.IPAddress = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]; // cart.ThisCustomer.LastIPAddress;

            // instantiate the actual request object
            PaymentRequest = new DoDirectPaymentRequestType();
            PaymentRequest.Version = API_VER;
            PaymentRequest.DoDirectPaymentRequestDetails = RequestDetails;
            DDPReq = new DoDirectPaymentReq();
            DDPReq.DoDirectPaymentRequest = PaymentRequest;

            DoDirectPaymentResponseType responseDetails = (DoDirectPaymentResponseType)IPayPal.DoDirectPayment(DDPReq);

            if (responseDetails.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
            {
                AuthorizationTransID = (TransactionMode.ToLower().IndexOf("auth") > -1 ? "AUTH=" : "CAPTURE=") + responseDetails.TransactionID.ToString();
                AuthorizationCode = responseDetails.CorrelationID;
                AVSResult = responseDetails.AVSCode;
                result = AppLogic.ro_OK;
                AuthorizationResult = responseDetails.Ack.ToString() + "|AVSCode=" + responseDetails.AVSCode.ToString() + "|CVV2Code=" + responseDetails.CVV2Code.ToString();
            }
            else
            {
                if (responseDetails.Errors != null)
                {
                    String Separator = String.Empty;
                    for (int ix = 0; ix < responseDetails.Errors.Length; ix++)
                    {
                        AuthorizationResult += Separator;
                        AuthorizationResult += responseDetails.Errors[ix].LongMessage;// record failed TX
                        TransactionResponse += Separator;
                        try
                        {
                            TransactionResponse += String.Format("|{0},{1},{2}|", responseDetails.Errors[ix].ShortMessage, responseDetails.Errors[ix].ErrorCode, responseDetails.Errors[ix].LongMessage); // record failed TX
                        }
                        catch { }
                        Separator = ", ";
                    }
                }
                result = AuthorizationResult;
                // just store something here, as there is no other way to get data out of this gateway about the failure for logging in failed transaction table
            }
            return result;
        }

        /// <summary>
        /// Re-Authorize Order
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String ReAuthorizeOrder(int OrderNumber)
        {
            // Once the PayPal honor period (3 days) is over, PayPal no longer ensures that 100%
            // of the funds will be available. A ReAuthorize will start a new settle period.

            String result = String.Empty;

            String PNREF = String.Empty;
            String TransID = String.Empty;

            SQLAccess DB = new SQLAccess();

            Decimal OrderTotal = 0;

            DataSet dsorder = new DataSet();
            dsorder = CommonComponent.GetCommonDataSet("select AuthorizationPNREF,OrderTotal from tb_order where OrderNumber=" + OrderNumber.ToString());
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                PNREF = Convert.ToString(dsorder.Tables[0].Rows[0]["AuthorizationPNREF"].ToString());
                TransID = Regex.Match(PNREF, "(?<=AUTH=)[0-9A-Z]+").ToString();
                OrderTotal = Convert.ToDecimal(dsorder.Tables[0].Rows[0]["OrderTotal"].ToString());
            }


            if (TransID.Length == 0 || TransID == "0")
            {
                result = "Invalid or Empty Transaction ID";
            }
            else
            {
                try
                {
                    BasicAmountType totalAmount = new BasicAmountType();
                    totalAmount.Value = Convert.ToString(OrderTotal);
                    totalAmount.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), AppLogic.AppConfigs("Localization.StoreCurrency"), true);

                    DoReauthorizationRequestType Reauth = new DoReauthorizationRequestType();
                    Reauth.Amount = totalAmount;
                    Reauth.AuthorizationID = TransID;
                    Reauth.Version = API_VER;

                    DoReauthorizationReq ReauthReq = new DoReauthorizationReq();

                    ReauthReq.DoReauthorizationRequest = Reauth;

                    DoReauthorizationResponseType ReauthResponse;
                    ReauthResponse = (DoReauthorizationResponseType)IPayPal.DoReauthorization(ReauthReq);

                    if (ReauthResponse.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
                    {
                        result = AppLogic.ro_OK;
                        DB.ExecuteNonQuery("update tb_order set AuthorizedOn=getdate(), AuthorizationPNREF=AuthorizationPNREF+'|REAU=" + ReauthResponse.AuthorizationID + "' where OrderNumber=" + OrderNumber.ToString());
                    }
                    else
                    {
                        if (ReauthResponse.Errors != null)
                        {
                            bool first = true;
                            for (int ix = 0; ix < ReauthResponse.Errors.Length; ix++)
                            {
                                if (!first)
                                {
                                    result += ", ";
                                }
                                result += "Error: [" + ReauthResponse.Errors[ix].ErrorCode + "] " + ReauthResponse.Errors[ix].LongMessage;
                                first = false;
                            }
                        }
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
        /// Start EC
        /// </summary>
        /// <param name="TransactionMode">string TransactionMode</param>
        /// <returns>Returns the result as a String according to execution</returns>
        //  public String StartEC(string TransactionMode)
        //  { // default is we show the customer the review page
        // return StartEC(false, TransactionMode);
        //  }

        public String StartEC(string NewTransactionMode, string CancelPage)
        { // default is we show the customer the review page
            return StartEC(false, NewTransactionMode, CancelPage);
        }



        public String StartEC(bool boolBypassOrderReview, string NewTransactionMode, string CancelPage)
        {
            SetExpressCheckoutReq ECRequest = new SetExpressCheckoutReq();
            SetExpressCheckoutRequestType varECRequest = new SetExpressCheckoutRequestType();
            SetExpressCheckoutRequestDetailsType varECRequestDetails = new SetExpressCheckoutRequestDetailsType();
            SetExpressCheckoutResponseType ECResponse = new SetExpressCheckoutResponseType();

            ECRequest.SetExpressCheckoutRequest = varECRequest;
            varECRequest.SetExpressCheckoutRequestDetails = varECRequestDetails;

            ECOrderTotal.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), AppLogic.AppConfigs("Localization.StoreCurrency"), true);
            varECRequestDetails.OrderTotal = ECOrderTotal;

            if (AppLogic.AppConfigBool("PayPal.RequireConfirmedAddress"))
            {
                varECRequestDetails.ReqConfirmShipping = "1";
            }
            else
            {
                varECRequestDetails.ReqConfirmShipping = "0";
                if (!String.IsNullOrEmpty(ECShippingAddress.CityName + ECShippingAddress.StateOrProvince + ECShippingAddress.PostalCode))
                { // if shipping address defined (not Anonymous)
                    varECRequestDetails.Address = ECShippingAddress;
                    varECRequestDetails.AddressOverride = "1";
                }
            }

            varECRequestDetails.ReturnURL = AppLogic.AppConfigs("PAYPAL_LIVE_SERVER") + AppLogic.AppConfigs("PayPal.Express.ReturnURL");
            if (boolBypassOrderReview)
            {
                varECRequestDetails.ReturnURL += "?BypassOrderReview=true";
            }

            varECRequestDetails.CancelURL = AppLogic.AppConfigs("PAYPAL_LIVE_SERVER").ToString().Replace("Index.aspx/", "") + CancelPage;
            varECRequestDetails.LocaleCode = AppLogic.AppConfigs("PayPal.DefaultLocaleCode");

            //if (AppLogic.TransactionModeIsAuthOnly())
            //{
            //    varECRequestDetails.PaymentAction = PaymentActionCodeType.Authorization;
            //}
            //else
            //{
            //    varECRequestDetails.PaymentAction = PaymentActionCodeType.Sale;
            //}



            if (NewTransactionMode.ToLower().IndexOf("auth") > -1)
            {
                varECRequestDetails.PaymentAction = PaymentActionCodeType.Authorization;
            }
            else
            {
                varECRequestDetails.PaymentAction = PaymentActionCodeType.Sale;
            }


            varECRequestDetails.PaymentActionSpecified = true;
            varECRequest.Version = "1.0";

            if (AppLogic.AppConfigs("PayPal.Express.PageStyle").Trim() != "")
            {
                varECRequestDetails.PageStyle = AppLogic.AppConfigs("PayPal.Express.PageStyle").Trim();
            }

            if (AppLogic.AppConfigs("PayPal.Express.HeaderImage").Trim() != "")
            {
                varECRequestDetails.cppheaderimage = AppLogic.AppConfigs("PayPal.Express.HeaderImage").Trim();
            }

            if (AppLogic.AppConfigs("PayPal.Express.HeaderBackColor").Trim() != "")
            {
                varECRequestDetails.cppheaderbackcolor = AppLogic.AppConfigs("PayPal.Express.HeaderBackColor").Trim();
            }

            if (AppLogic.AppConfigs("PayPal.Express.HeaderBorderColor").Trim() != "")
            {
                varECRequestDetails.cppheaderbordercolor = AppLogic.AppConfigs("PayPal.Express.HeaderBorderColor").Trim();
            }

            if (AppLogic.AppConfigs("PayPal.Express.PayFlowColor").Trim() != "")
            {
                varECRequestDetails.cpppayflowcolor = AppLogic.AppConfigs("PayPal.Express.PayFlowColor").Trim();
            }

            String result = String.Empty;
            try
            {
                ECResponse = IPayPal.SetExpressCheckout(ECRequest);

                if (ECResponse.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = AppLogic.ro_OK;
                }
                else
                {
                    if (ECResponse.Errors != null)
                    {
                        bool first = true;
                        for (int ix = 0; ix < ECResponse.Errors.Length; ix++)
                        {
                            if (!first)
                            {
                                result += ", ";
                            }
                            result += "Error: [" + ECResponse.Errors[ix].ErrorCode + "] " + ECResponse.Errors[ix].LongMessage;
                            first = false;
                        }
                    }
                }

            }
            catch
            {
                result = "Failed to start PayPal Express Checkout! Please try another payment method.";
            }

            String sURL = String.Empty;
            if (result == AppLogic.ro_OK)
            {
                if (AppLogic.AppConfigBool("UseLiveTransactions") == true)
                {
                    sURL = AppLogic.AppConfigs("PayPal.Express.LiveURL");
                }
                else
                {
                    sURL = AppLogic.AppConfigs("PayPal.Express.SandboxURL");
                }
                sURL += "?cmd=_express-checkout&token=" + ECResponse.Token;
                if (boolBypassOrderReview)
                {
                    sURL += "&useraction=commit";
                }
            }
            else
            {
                sURL = CancelPage + "&error=" + HttpContext.Current.Server.UrlEncode(result);
            }
            return sURL;
        }

        /// <summary>
        /// Start EC
        /// </summary>
        /// <param name="boolBypassOrderReview">bool boolBypassOrderReview</param>
        /// <param name="TransactionMode">String TransactionMode</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String StartEC(bool boolBypassOrderReview, String TransactionMode, string mode, string CancelPage)
        {
            SetExpressCheckoutReq ECRequest = new SetExpressCheckoutReq();
            SetExpressCheckoutRequestType varECRequest = new SetExpressCheckoutRequestType();
            SetExpressCheckoutRequestDetailsType varECRequestDetails = new SetExpressCheckoutRequestDetailsType();
            SetExpressCheckoutResponseType ECResponse = new SetExpressCheckoutResponseType();

            ECRequest.SetExpressCheckoutRequest = varECRequest;
            varECRequest.SetExpressCheckoutRequestDetails = varECRequestDetails;

            ECOrderTotal.currencyID = (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), AppLogic.AppConfigs("Localization.StoreCurrency"), true);
            varECRequestDetails.OrderTotal = ECOrderTotal;

            if (AppLogic.AppConfigBool("PayPal.RequireConfirmedAddress"))
            {
                varECRequestDetails.ReqConfirmShipping = "1";
            }
            else
            {
                varECRequestDetails.ReqConfirmShipping = "0";
                if (!String.IsNullOrEmpty(ECShippingAddress.CityName + ECShippingAddress.StateOrProvince + ECShippingAddress.PostalCode))
                { // if shipping address defined (not Anonymous)
                    varECRequestDetails.Address = ECShippingAddress;
                    varECRequestDetails.AddressOverride = "1";
                }
            }

            varECRequestDetails.ReturnURL = AppLogic.AppConfigs("PAYPAL_LIVE_SERVER") + AppLogic.AppConfigs("PayPal.Express.ReturnURL");
            if (boolBypassOrderReview)
            {
                varECRequestDetails.ReturnURL += "?BypassOrderReview=true";
            }

            varECRequestDetails.CancelURL = AppLogic.AppConfigs("PAYPAL_LIVE_SERVER").ToString().Replace("Index.aspx/", "") + CancelPage;
            varECRequestDetails.LocaleCode = AppLogic.AppConfigs("PayPal.DefaultLocaleCode");

            if (TransactionMode.ToLower().IndexOf("auth") > -1)
            {
                varECRequestDetails.PaymentAction = PaymentActionCodeType.Authorization;
            }
            else
            {
                varECRequestDetails.PaymentAction = PaymentActionCodeType.Sale;
            }

            varECRequestDetails.PaymentActionSpecified = true;
            varECRequest.Version = "1.0";

            if (AppLogic.AppConfigs("PayPal.Express.PageStyle").Trim() != "")
            {
                varECRequestDetails.PageStyle = AppLogic.AppConfigs("PayPal.Express.PageStyle").Trim();
            }

            if (AppLogic.AppConfigs("PayPal.Express.HeaderImage").Trim() != "")
            {
                varECRequestDetails.cppheaderimage = AppLogic.AppConfigs("PayPal.Express.HeaderImage").Trim();
            }

            if (AppLogic.AppConfigs("PayPal.Express.HeaderBackColor").Trim() != "")
            {
                varECRequestDetails.cppheaderbackcolor = AppLogic.AppConfigs("PayPal.Express.HeaderBackColor").Trim();
            }

            if (AppLogic.AppConfigs("PayPal.Express.HeaderBorderColor").Trim() != "")
            {
                varECRequestDetails.cppheaderbordercolor = AppLogic.AppConfigs("PayPal.Express.HeaderBorderColor").Trim();
            }

            if (AppLogic.AppConfigs("PayPal.Express.PayFlowColor").Trim() != "")
            {
                varECRequestDetails.cpppayflowcolor = AppLogic.AppConfigs("PayPal.Express.PayFlowColor").Trim();
            }

            String result = String.Empty;
            try
            {
                ECResponse = IPayPal.SetExpressCheckout(ECRequest);

                if (ECResponse.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
                {
                    result = AppLogic.ro_OK;
                }
                else
                {
                    if (ECResponse.Errors != null)
                    {
                        bool first = true;
                        for (int ix = 0; ix < ECResponse.Errors.Length; ix++)
                        {
                            if (!first)
                            {
                                result += ", ";
                            }
                            result += "Error: [" + ECResponse.Errors[ix].ErrorCode + "] " + ECResponse.Errors[ix].LongMessage;
                            first = false;
                        }
                    }
                }

            }
            catch
            {
                result = "Failed to start PayPal Express Checkout! Please try another payment method.";
            }

            String sURL = String.Empty;
            if (result == AppLogic.ro_OK)
            {
                if (AppLogic.AppConfigBool("UseLiveTransactions") == true)
                {
                    sURL = AppLogic.AppConfigs("PayPal.Express.LiveURL");
                }
                else
                {
                    sURL = AppLogic.AppConfigs("PayPal.Express.SandboxURL");
                }
                sURL += "?cmd=_express-checkout&token=" + ECResponse.Token;
                if (boolBypassOrderReview)
                {
                    sURL += "&useraction=commit";
                }
            }
            else
            {
                sURL = "addtocart.aspx?resetlinkback=1&errormsg=" + HttpContext.Current.Server.UrlEncode(result);
            }
            return sURL;
        }

        /// <summary>
        /// Set EC Shipping Country
        /// </summary>
        /// <param name="Country">String Country</param>
        public void SetECShippingCountry(String Country)
        {
            CountryDAC objCountry = new CountryDAC();
            ECShippingAddress.Country = (CountryCodeType)Enum.Parse(typeof(CountryCodeType), objCountry.GetTwoCountryCodeByName(Country), true);
            ECShippingAddress.CountrySpecified = true;
        }

        /// <summary>
        /// Currency String For Gateway Without Exchange Rate
        /// </summary>
        /// <param name="amt">decimal amt</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String CurrencyStringForGatewayWithoutExchangeRate(decimal amt)
        {
            String s = amt.ToString("#.00", USCulture);
            if (s == ".00")
            {
                s = "0.00";
            }
            return s;
        }

        /// <summary>
        /// Get EC Details
        /// </summary>
        /// <param name="PayPalToken">String PayPalToken</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public String GetECDetails(String PayPalToken, int CustomerID)
        {
            GetExpressCheckoutDetailsReq ECRequest = new GetExpressCheckoutDetailsReq();
            GetExpressCheckoutDetailsRequestType varECRequest = new GetExpressCheckoutDetailsRequestType();
            GetExpressCheckoutDetailsResponseType ECResponse = new GetExpressCheckoutDetailsResponseType();
            GetExpressCheckoutDetailsResponseDetailsType varECResponse = new GetExpressCheckoutDetailsResponseDetailsType();

            ECRequest.GetExpressCheckoutDetailsRequest = varECRequest;
            ECResponse.GetExpressCheckoutDetailsResponseDetails = varECResponse;

            varECRequest.Token = PayPalToken;
            varECRequest.Version = "3.0";

            ECResponse = IPayPal.GetExpressCheckoutDetails(ECRequest);

            PayerInfoType PayerInfo = ECResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo;

            //clsCustomer ThisCustomer = new clsCustomer(CustomerID);
            //if (!ThisCustomer.IsRegistered)
            //{
            //    ThisCustomer.FirstName = PayerInfo.PayerName.FirstName;
            //    ThisCustomer.LastName = PayerInfo.PayerName.LastName;

            //    if (PayerInfo.Address.Phone != null)
            //    {
            //        ThisCustomer.Phone = PayerInfo.Address.Phone;
            //    }

            //    ThisCustomer.UpdateCustomer(CustomerID, ThisCustomer.Email);

            //    //ThisCustomer.UpdateCustomer(
            //    //    /*CustomerLevelID*/ null,
            //    //    /*EMail*/ PayerInfo.Payer,
            //    //    /*SaltedAndHashedPassword*/ null,
            //    //    /*SaltKey*/ null,
            //    //    /*DateOfBirth*/ null,
            //    //    /*Gender*/ null,
            //    //    /*FirstName*/ PayerInfo.PayerName.FirstName,
            //    //    /*LastName*/ PayerInfo.PayerName.LastName,
            //    //    /*Notes*/ null,
            //    //    /*SkinID*/ null,
            //    //    /*Phone*/ AppLogic.IIF(PayerInfo.Address.Phone != null, PayerInfo.Address.Phone, ""),
            //    //    /*AffiliateID*/ null,
            //    //    /*Referrer*/ null,
            //    //    /*CouponCode*/ null,
            //    //    /*OkToEmail*/ 0,
            //    //    /*IsAdmin*/ null,
            //    //    /*BillingEqualsShipping*/ null,
            //    //    /*LastIPAddress*/ null,
            //    //    /*OrderNotes*/ null,
            //    //    /*SubscriptionExpiresOn*/ null,
            //    //    /*RTShipRequest*/ null,
            //    //    /*RTShipResponse*/ null,
            //    //    /*OrderOptions*/ null,
            //    //    /*LocaleSetting*/ null,
            //    //    /*MicroPayBalance*/ null,
            //    //    /*RecurringShippingMethodID*/ null,
            //    //    /*RecurringShippingMethod*/ null,
            //    //    /*BillingAddressID*/ null,
            //    //    /*ShippingAddressID*/ null,
            //    //    /*GiftRegistryGUID*/ null,
            //    //    /*GiftRegistryIsAnonymous*/ null,
            //    //    /*GiftRegistryAllowSearchByOthers*/ null,
            //    //    /*GiftRegistryNickName*/ null,
            //    //    /*GiftRegistryHideShippingAddresses*/ null,
            //    //    /*CODCompanyCheckAllowed*/ null,
            //    //    /*CODNet30Allowed*/ null,
            //    //    /*ExtensionData*/ null,
            //    //    /*FinalizationData*/ null,
            //    //    /*Deleted*/ null,
            //    //    /*Over13Checked*/ null,
            //    //    /*CurrencySetting*/ null,
            //    //    /*VATSetting*/ null,
            //    //    /*VATRegistrationID*/ null,
            //    //    /*StoreCCInDB*/ null,
            //    //    /*IsRegistered*/ null,
            //    //    /*LockedUntil*/ null,
            //    //    /*AdminCanViewCC*/ null,
            //    //    /*BadLogin*/ null,
            //    //    /*Active*/ null,
            //    //    /*PwdChangeRequired*/ null,
            //    //    /*RegisterDate*/ null,
            //    //    null,
            //    //    1
            //    //);

            //    String PM = AppLogic.ro_PMPayPalExpress;

            //    String BillingPhone = String.Empty;

            //    //if (ThisCustomer.PrimaryBillingAddressID > 0)
            //    //{

            //    clsAddress UseBillingAddress = new clsAddress(ThisCustomer.BillingAddressID);

            //    //UseBillingAddress.LoadByCustomer(ThisCustomer.CustomerID, ThisCustomer.PrimaryBillingAddressID, AddressTypes.Billing);

            //    if (UseBillingAddress.PaymentMethodIDLastUsed != AppLogic.ro_PMPayPalExpress
            //        && UseBillingAddress.PaymentMethodIDLastUsed != AppLogic.ro_PMPayPalExpressMark)
            //    {
            //        PM = AppLogic.ro_PMPayPalExpress;
            //    }
            //    else
            //    {
            //        PM = UseBillingAddress.PaymentMethodIDLastUsed;
            //    }
            //    BillingPhone = UseBillingAddress.Phone;

            //    //}

            //     //Anonymous Paypal Express Checkout order, use Paypal's address
            //    clsAddress ShippingAddress = new clsAddress();
            //    ShippingAddress.CustomerID = CustomerID;
            //    ShippingAddress.PaymentMethodIDLastUsed = PM;
            //    String[] NameArray = PayerInfo.Address.Name.Split(new string[1] { " " }, 2, StringSplitOptions.RemoveEmptyEntries);
            //    String FirstName = String.Empty;
            //    String LastName = String.Empty;
            //    if (NameArray.Length > 1)
            //    {
            //        FirstName = NameArray[0];
            //        LastName = NameArray[1];
            //    }
            //    else
            //    {
            //        LastName = PayerInfo.Address.Name;
            //    }
            //    ShippingAddress.FirstName = FirstName;
            //    ShippingAddress.LastName = LastName;
            //    ShippingAddress.Address1 = PayerInfo.Address.Street1;
            //    ShippingAddress.Address2 = PayerInfo.Address.Street2;
            //    ShippingAddress.Phone = AppLogic.IIF(PayerInfo.Address.Phone != null, PayerInfo.Address.Phone, BillingPhone);
            //    ShippingAddress.City = PayerInfo.Address.CityName;
            //    ShippingAddress.State = PayerInfo.Address.StateOrProvince;
            //    ShippingAddress.ZipCode = PayerInfo.Address.PostalCode;
            //    ShippingAddress.Country = PayerInfo.Address.CountryName;

            //    ShippingAddress.AddAddress();

            //    //ShippingAddress.MakeCustomersPrimaryAddress(AddressTypes.Shipping);

            //    clsAddress BillingAddress = new clsAddress();
            //    BillingAddress.CustomerID = CustomerID;
            //    BillingAddress.PaymentMethodIDLastUsed = PM;
            //    BillingAddress.FirstName = PayerInfo.PayerName.FirstName;
            //    BillingAddress.LastName = PayerInfo.PayerName.LastName;
            //    BillingAddress.Address1 = PayerInfo.Address.Street1;
            //    BillingAddress.Address2 = PayerInfo.Address.Street2;
            //    BillingAddress.Phone = AppLogic.IIF(PayerInfo.Address.Phone != null, PayerInfo.Address.Phone, "");
            //    BillingAddress.City = PayerInfo.Address.CityName;
            //    BillingAddress.State = PayerInfo.Address.StateOrProvince;
            //    BillingAddress.ZipCode = PayerInfo.Address.PostalCode;
            //    BillingAddress.Country = PayerInfo.Address.CountryName;
            //    BillingAddress.AddAddress();

            //    //BillingAddress.MakeCustomersPrimaryAddress(AddressTypes.Billing);

            //}
            //else
            //{ // A registered and logged in Customer

            //    clsAddress UseBillingAddress = new clsAddress(ThisCustomer.BillingAddressID);
            //    //UseBillingAddress.LoadByCustomer(ThisCustomer.CustomerID, ThisCustomer.PrimaryBillingAddressID, AddressTypes.Billing);
            //    UseBillingAddress.ClearCCInfo();
            //    if (UseBillingAddress.PaymentMethodIDLastUsed != AppLogic.ro_PMPayPalExpress
            //        && UseBillingAddress.PaymentMethodIDLastUsed != AppLogic.ro_PMPayPalExpressMark)
            //    {
            //        UseBillingAddress.PaymentMethodIDLastUsed = AppLogic.ro_PMPayPalExpress;
            //    }


            //    string sql = String.Format("select top 1 AddressID as N from tb_Address where Address1={0} and Address2={1} and City={2} and State={3} and ZipCode={4} and Country={5} and CustomerID={6}",
            //        SQuote(PayerInfo.Address.Street1), SQuote(PayerInfo.Address.Street2), SQuote(PayerInfo.Address.CityName), SQuote(AppLogic.GetStateName(PayerInfo.Address.StateOrProvince)),
            //        SQuote(PayerInfo.Address.PostalCode), UseBillingAddress.GetIDByCountryName(PayerInfo.Address.CountryName), CustomerID);

            //    int ExistingAddressID = GetSqlN(sql);

            //    clsAddress ShippingAddress ;

            //    if (ExistingAddressID == 0)
            //    { // Does not exist

            //        ShippingAddress = new clsAddress();

            //        ShippingAddress.CustomerID = CustomerID;
            //        ShippingAddress.PaymentMethodIDLastUsed = AppLogic.ro_PMPayPalExpress;
            //        String[] NameArray = PayerInfo.Address.Name.Split(new string[1] { " " }, 2, StringSplitOptions.RemoveEmptyEntries);
            //        String FirstName = String.Empty;
            //        String LastName = String.Empty;
            //        if (NameArray.Length > 1)
            //        {
            //            FirstName = NameArray[0];
            //            LastName = NameArray[1];
            //        }
            //        else
            //        {
            //            LastName = PayerInfo.Address.Name;
            //        }
            //        ShippingAddress.FirstName = FirstName;
            //        ShippingAddress.LastName = LastName;
            //        ShippingAddress.Address1 = PayerInfo.Address.Street1;
            //        ShippingAddress.Address2 = PayerInfo.Address.Street2;
            //        ShippingAddress.Phone = AppLogic.IIF(PayerInfo.Address.Phone != null, PayerInfo.Address.Phone, UseBillingAddress.Phone);
            //        ShippingAddress.City = PayerInfo.Address.CityName;
            //        ShippingAddress.State = PayerInfo.Address.StateOrProvince;
            //        ShippingAddress.ZipCode = PayerInfo.Address.PostalCode;
            //        ShippingAddress.Country = PayerInfo.Address.CountryName;
            //        int CustomerShipAddID=ShippingAddress.AddAddress();
            //        clsCustomer objeCust = new clsCustomer();
            //        objeCust.UpdateCustAddress("ShippingAddressID", CustomerShipAddID, CustomerID);
            //       // ShippingAddress.MakeCustomersPrimaryAddress(AddressTypes.Shipping);
            //    }
            //    else
            //    { // Exists already
            //        ShippingAddress = new clsAddress(ExistingAddressID);
            //        //ShippingAddress.MakeCustomersPrimaryAddress(AddressTypes.Shipping);
            //    }
            //}

            return ECResponse.GetExpressCheckoutDetailsResponseDetails.PayerInfo.PayerID;
        }

        /// <summary>
        /// Process EC for PayPal
        /// </summary>
        /// <param name="OrderTotal">decimal OrderTotal</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="PayPalToken">string PayPalToken</param>
        /// <param name="PayerID">String PayerID</param>
        /// <param name="TransactionMode">string TransactionMode</param>
        /// <param name="AuthorizationResult">string AuthorizationResult</param>
        /// <param name="AuthorizationTransID">string AuthorizationTransID</param>
        /// <returns>Returns the result as a String according to execution</returns>>
        public String ProcessEC(decimal OrderTotal, int OrderNumber, String PayPalToken, String PayerID, String TransactionMode, out String AuthorizationResult, out String AuthorizationTransID)
        {
            String result = String.Empty;
            AuthorizationResult = String.Empty;
            AuthorizationTransID = String.Empty;

            DoExpressCheckoutPaymentReq ECRequest = new DoExpressCheckoutPaymentReq();
            DoExpressCheckoutPaymentRequestType varECRequest = new DoExpressCheckoutPaymentRequestType();
            DoExpressCheckoutPaymentRequestDetailsType varECRequestDetails = new DoExpressCheckoutPaymentRequestDetailsType();
            DoExpressCheckoutPaymentResponseType ECResponse = new DoExpressCheckoutPaymentResponseType();
            DoExpressCheckoutPaymentResponseDetailsType varECResponse = new DoExpressCheckoutPaymentResponseDetailsType();

            ECRequest.DoExpressCheckoutPaymentRequest = varECRequest;
            varECRequest.DoExpressCheckoutPaymentRequestDetails = varECRequestDetails;
            ECResponse.DoExpressCheckoutPaymentResponseDetails = varECResponse;

            varECRequestDetails.Token = PayPalToken;
            varECRequestDetails.PayerID = PayerID;

            if (TransactionMode.ToLower().IndexOf("auth") > -1)
            {
                varECRequestDetails.PaymentAction = PaymentActionCodeType.Authorization;
            }
            else
            {
                varECRequestDetails.PaymentAction = PaymentActionCodeType.Sale;
            }

            PaymentDetailsType ECPaymentDetails = new PaymentDetailsType();
            BasicAmountType ECPaymentOrderTotal = new BasicAmountType();
            //ECPaymentOrderTotal.Value = Localization.CurrencyStringForGatewayWithoutExchangeRate(OrderTotal);
            ECPaymentOrderTotal.Value = CurrencyStringForGatewayWithoutExchangeRate(OrderTotal);
            ECPaymentOrderTotal.currencyID =
                (CurrencyCodeType)Enum.Parse(typeof(CurrencyCodeType), AppLogic.AppConfigs("Localization.StoreCurrency"), true);
            ECPaymentDetails.OrderTotal = ECPaymentOrderTotal;
            ECPaymentDetails.InvoiceID = OrderNumber.ToString();
            ECPaymentDetails.ButtonSource = PayPalComponent.BN + "_EC_US";

            varECRequestDetails.PaymentDetails = ECPaymentDetails;

            varECRequest.Version = "1.0";
            try
            {
                ECResponse = IPayPal.DoExpressCheckoutPayment(ECRequest);
            }
            catch { }

            if (ECResponse.Ack.ToString().StartsWith("success", StringComparison.InvariantCultureIgnoreCase))
            {
                AuthorizationTransID = (TransactionMode.ToLower().IndexOf("auth") > -1 ? "AUTH=" : "CAPTURE=") + ECResponse.DoExpressCheckoutPaymentResponseDetails.PaymentInfo.TransactionID;
                result = AppLogic.ro_OK;
                AuthorizationResult = ECResponse.Ack.ToString();
            }
            else
            {
                if (ECResponse.Errors != null)
                {
                    bool first = true;
                    for (int ix = 0; ix < ECResponse.Errors.Length; ix++)
                    {
                        if (!first)
                        {
                            AuthorizationResult += ", ";
                        }
                        AuthorizationResult += ECResponse.Errors[ix].LongMessage;
                        first = false;
                    }
                }
                result = AuthorizationResult;
            }
            return result;
        }

        /// <summary>
        /// Get Transaction State for PayPal Process
        /// </summary>
        /// <param name="PaymentStatus">String PaymentStatus</param>
        /// <param name="PendingReason">String PendingReason</param>
        /// <returns>Returns the result as a String according to execution</returns>
        static public String GetTransactionState(String PaymentStatus, String PendingReason)
        {
            String result = String.Empty;

            switch (PaymentStatus.ToLowerInvariant())
            {
                case "pending":
                    switch (PendingReason.ToLowerInvariant())
                    {
                        case "authorization":
                            result = AppLogic.ro_TXStateAuthorized;
                            break;
                        default:
                            result = AppLogic.ro_TXStatePending;
                            break;
                    }
                    break;
                case "processed":
                case "completed":
                case "canceled_reversal":
                    result = AppLogic.ro_TXStateCaptured;
                    break;
                case "denied":
                case "expired":
                case "failed":
                case "voided":
                    result = AppLogic.ro_TXStateVoided;
                    break;
                case "refunded":
                case "reversed":
                    result = AppLogic.ro_TXStateRefunded;
                    break;
                default:
                    result = AppLogic.ro_TXStateUnknown;
                    break;
            }
            return result;
        }

        /// <summary>
        /// Replace function for single quote to double quote and Add N as Prefix
        /// </summary>
        /// <param name="s">String s</param>
        /// <returns>Returns String</returns>
        public String SQuote(String s)
        {
            return "N'" + s.Replace("'", "''") + "'";
        }

    }
}
