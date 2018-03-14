// This code was built using Visual Studio 2005
using System;
using System.Web.Services.Protocols;
using Solution.ShippingMethods.FedEx;
using Solution.Bussines.Components.Common;


//
// Sample code to call the FedEx Rate Available Web Service
//

namespace Solution.ShippingMethods
{
    public class FedExRate
    {

        #region LocalVarible

        
        private String _oStreetLines = string.Empty;
        private String _oCity = string.Empty;
        private String _oStateOrProvinceCode = string.Empty;
        private String _oPostalCode = string.Empty;
        private String _oCountryCode = string.Empty;

        public String oStreetLines
        {
            get { return _oStreetLines; }
            set { _oStreetLines = value; }
        }
        public String  oCity
        {
            get { return _oCity; }
            set { _oCity = value; }
        }
        public String oStateOrProvinceCode
        {
            get { return _oStateOrProvinceCode; }
            set { _oStateOrProvinceCode = value; }
        }
        public String oPostalCode
        {
            get { return _oPostalCode; }
            set { _oPostalCode = value; }
        }
        public String oCountryCode
        {
            get { return _oCountryCode; }
            set { _oCountryCode = value; }
        }
        private String _dStreetLines = string.Empty;
        private String _dCity = string.Empty;
        private String _dStateOrProvinceCode = string.Empty;
        private String _dPostalCode = string.Empty;
        private String _dCountryCode = string.Empty;

        public String dStreetLines
        {
            get { return _dStreetLines; }
            set { _dStreetLines = value; }
        }
        public String dCity
        {
            get { return _dCity; }
            set { _dCity = value; }
        }
        public String dStateOrProvinceCode
        {
            get { return _dStateOrProvinceCode; }
            set { _dStateOrProvinceCode = value; }
        }
        public String dPostalCode
        {
            get { return _dPostalCode; }
            set { _dPostalCode = value; }
        }
        public String dCountryCode
        {
            get { return _dCountryCode; }
            set { _dCountryCode = value; }
        }

        #endregion

        public RateRequest Main()
        {
            RateRequest request = CreateRateRequest();
            ////
            //RateService service = new RateService(); // Initialize the service
            //try
            //{
            //    // This is the call to the web service passing in a RateRequest and returning a RateReply
            //    RateReply reply = service.getRates(request); // Service call
            //    //
            //    if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING) // check if the call was successful
            //    {
            //        ShowRateReply(reply);
            //    }
            //    else
            //    {
            //        //Console.WriteLine(reply.Notifications[0].Message);
            //    }
            //}
            //catch (SoapException e)
            //{
            //    //Console.WriteLine(e.Detail.InnerText);
            //}
            //catch (Exception e)
            //{
            //    //Console.WriteLine(e.Message);
            //}
            ////Console.WriteLine("Press any key to quit!");
            ////Console.ReadKey();
            return request;
        }

        private static void ShowRateReply(RateReply reply)
        {
            Console.WriteLine("RateReply details:");
            Console.WriteLine("**********************************************************");
            foreach (RateReplyDetail rateDetail in reply.RateReplyDetails)
            {
                Console.WriteLine("ServiceType: " + rateDetail.ServiceType);
                foreach (RatedShipmentDetail shipmentDetail in rateDetail.RatedShipmentDetails)
                {
                    ShowPackageRateDetails(shipmentDetail);
                }
                ShowDeliveryDetails(rateDetail);
                Console.WriteLine("**********************************************************");
            }
        }

        private static void ShowPackageRateDetails(RatedShipmentDetail shipmentDetail)
        {
            Console.WriteLine("RateType : " + shipmentDetail.ShipmentRateDetail.RateType);
            Console.WriteLine("Total Billing Weight : " + shipmentDetail.ShipmentRateDetail.TotalBillingWeight.Value);
            Console.WriteLine("Total Base Charge : " + shipmentDetail.ShipmentRateDetail.TotalBaseCharge.Amount);
            Console.WriteLine("Total Discount : " + shipmentDetail.ShipmentRateDetail.TotalFreightDiscounts.Amount);
            Console.WriteLine("Total Surcharges : " + shipmentDetail.ShipmentRateDetail.TotalSurcharges.Amount);
            Console.WriteLine("Net Charge : " + shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount);
            Console.WriteLine("*********");
        }

        private static void ShowDeliveryDetails(RateReplyDetail rateDetail)
        {
            if (rateDetail.DeliveryTimestampSpecified)
            {
                Console.WriteLine("Delivery timestamp " + rateDetail.DeliveryTimestamp.ToString());
            }
            Console.WriteLine("Transit Time: " + rateDetail.TransitTime);
        }

        private  RateRequest CreateRateRequest()
        {
            // Build the RateRequest
            RateRequest request = new RateRequest();
            //
            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = AppLogic.AppConfigs("FedEx.Key"); // Replace "XXX" with the Key
            request.WebAuthenticationDetail.UserCredential.Password = AppLogic.AppConfigs("FedEx.Password"); // Replace "XXX" with the Password
            //
            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = AppLogic.AppConfigs("FedEx.AccountNumber"); // Replace "XXX" with client's account number
            request.ClientDetail.MeterNumber = AppLogic.AppConfigs("FedEx.Meter"); // Replace "XXX" with client's meter number
            
            //
            request.TransactionDetail = new TransactionDetail();
            request.TransactionDetail.CustomerTransactionId = "***Rate Available Services v8 Request using VC#***"; // This is a reference field for the customer.  Any value can be used and will be provided in the response.
            //
            request.Version = new VersionId(); // WSDL version information, value is automatically set from wsdl            
            // 
            request.ReturnTransitAndCommit = true;
            request.ReturnTransitAndCommitSpecified = true;
            request.CarrierCodes = new CarrierCodeType[2];
            // Insert the Carriers you would like to see the rates for
            request.CarrierCodes[0] = CarrierCodeType.FDXE;
            request.CarrierCodes[1] = CarrierCodeType.FDXG;
            
            
            //
            SetShipmentDetails(request);
            //
            request.RequestedShipment.Shipper = new Party();
            request.RequestedShipment.Shipper.Address = new Address();
            request.RequestedShipment.Shipper.Address.StreetLines = new string[1] { this.oStreetLines };
            request.RequestedShipment.Shipper.Address.City = this.oCity;
            request.RequestedShipment.Shipper.Address.StateOrProvinceCode = this.oStateOrProvinceCode;
            request.RequestedShipment.Shipper.Address.PostalCode = this.oPostalCode;
            request.RequestedShipment.Shipper.Address.CountryCode = this.oCountryCode;
            //
            request.RequestedShipment.Recipient = new Party();
            request.RequestedShipment.Recipient.Address = new Address();
            request.RequestedShipment.Recipient.Address.StreetLines = new string[1] { this.dStreetLines };
            request.RequestedShipment.Recipient.Address.City = this.dCity;
            request.RequestedShipment.Recipient.Address.StateOrProvinceCode = this.dStateOrProvinceCode;
            request.RequestedShipment.Recipient.Address.PostalCode = this.dPostalCode;
            request.RequestedShipment.Recipient.Address.CountryCode = this.dCountryCode;
            //
            SetPayment(request);
            //            
            SetIndividualPackageLineItems(request);
            //
            return request;
        }

        

        private static void SetShipmentDetails(RateRequest request)
        {
            request.RequestedShipment = new RequestedShipment();
            request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP; //Drop off types are BUSINESS_SERVICE_CENTER, DROP_BOX, REGULAR_PICKUP, REQUEST_COURIER, STATION
            request.RequestedShipment.TotalInsuredValue = new Money();
            request.RequestedShipment.TotalInsuredValue.Amount = 100;
            request.RequestedShipment.TotalInsuredValue.Currency = "USD";
            request.RequestedShipment.ShipTimestamp = DateTime.Now; // Shipping date and time
            request.RequestedShipment.ShipTimestampSpecified = true;
            request.RequestedShipment.RateRequestTypes = new RateRequestType[2];
            request.RequestedShipment.RateRequestTypes[0] = RateRequestType.ACCOUNT;
            request.RequestedShipment.RateRequestTypes[1] = RateRequestType.LIST;
            request.RequestedShipment.PackageDetail = RequestedPackageDetailType.INDIVIDUAL_PACKAGES;
            request.RequestedShipment.PackageDetailSpecified = true;
        }

        //public  void SetOrigin(RateRequest request)
        //{
        //    request.RequestedShipment.Shipper = new Party();
        //    request.RequestedShipment.Shipper.Address = new Address();
        //    request.RequestedShipment.Shipper.Address.StreetLines = new string[1] { this._oStreetLines };
        //    request.RequestedShipment.Shipper.Address.City = this.oCity ;
        //    request.RequestedShipment.Shipper.Address.StateOrProvinceCode = this.oStateOrProvinceCode;
        //    request.RequestedShipment.Shipper.Address.PostalCode = this.oPostalCode;
        //    request.RequestedShipment.Shipper.Address.CountryCode = this.oCountryCode;


        //}

        //private void SetDestination(RateRequest request)
        //{
        //    request.RequestedShipment.Recipient = new Party();
        //    request.RequestedShipment.Recipient.Address = new Address();
        //    request.RequestedShipment.Recipient.Address.StreetLines = new string[1] { "Recipient Address Line 1" };
        //    request.RequestedShipment.Recipient.Address.City = "Montreal";
        //    request.RequestedShipment.Recipient.Address.StateOrProvinceCode = "PQ";
        //    request.RequestedShipment.Recipient.Address.PostalCode = "H1E1A1";
        //    request.RequestedShipment.Recipient.Address.CountryCode = "CA";
        //}

        private static void SetPayment(RateRequest request)
        {
            request.RequestedShipment.ShippingChargesPayment = new Payment(); // Payment Information
            request.RequestedShipment.ShippingChargesPayment.PaymentType = PaymentType.SENDER; // Payment options are RECIPIENT, SENDER, THIRD_PARTY
            request.RequestedShipment.ShippingChargesPayment.PaymentTypeSpecified = true;
            request.RequestedShipment.ShippingChargesPayment.Payor = new Payor();
            request.RequestedShipment.ShippingChargesPayment.Payor.AccountNumber = AppLogic.AppConfigs("FedEx.AccountNumber"); ; // Replace "XXX" with client's account number
        }

        private static void SetIndividualPackageLineItems(RateRequest request)
        {
            // ------------------------------------------
            // Passing individual pieces rate request
            // ------------------------------------------
            request.RequestedShipment.PackageCount = "2";
            //                
            request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[2];
            request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            request.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber = "1"; // package sequence number
            //
            request.RequestedShipment.RequestedPackageLineItems[0].Weight = new Weight(); // package weight
            request.RequestedShipment.RequestedPackageLineItems[0].Weight.Units = WeightUnits.LB;
            request.RequestedShipment.RequestedPackageLineItems[0].Weight.Value = 15.0M;
            //
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions = new Dimensions(); // package dimensions
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Length = "10";
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Width = "13";
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Height = "4";
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Units = LinearUnits.IN;
            //
            request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue = new Money(); // insured value
            request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Amount = 100;
            request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency = "USD";

            //
            request.RequestedShipment.RequestedPackageLineItems[1] = new RequestedPackageLineItem();
            request.RequestedShipment.RequestedPackageLineItems[1].SequenceNumber = "2"; // package sequence number
            //
            request.RequestedShipment.RequestedPackageLineItems[1].Weight = new Weight(); // package weight
            request.RequestedShipment.RequestedPackageLineItems[1].Weight.Units = WeightUnits.LB;
            request.RequestedShipment.RequestedPackageLineItems[1].Weight.Value = 25.0M;
            //
            request.RequestedShipment.RequestedPackageLineItems[1].Dimensions = new Dimensions(); // package dimensions
            request.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Length = "20";
            request.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Width = "13";
            request.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Height = "4";
            request.RequestedShipment.RequestedPackageLineItems[1].Dimensions.Units = LinearUnits.IN;
            //
            request.RequestedShipment.RequestedPackageLineItems[1].InsuredValue = new Money(); // insured value
            request.RequestedShipment.RequestedPackageLineItems[1].InsuredValue.Amount = 500;
            request.RequestedShipment.RequestedPackageLineItems[1].InsuredValue.Currency = "USD";
        }
    }
}