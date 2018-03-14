/*******************************************************************************
 * Copyright 2009-2016 Amazon Services. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 *
 * You may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 *******************************************************************************
 * MWS Merchant Fulfillment Service
 * API Version: 2015-06-01
 * Library Version: 2016-03-30
 * Generated: Fri Nov 11 06:01:15 PST 2016
 */

using MWSMerchantFulfillmentService.Model;
using Solution.Bussines.Components.AdminCommon;
using System;
using System.Collections.Generic;
using System.Data;

namespace MWSMerchantFulfillmentService
{

    /// <summary>
    /// Runnable sample code to demonstrate usage of the C# client.
    ///
    /// To use, import the client source as a console application,
    /// and mark this class as the startup object. Then, replace
    /// parameters below with sensible values and run.
    /// </summary>
    public class MWSMerchantFulfillmentServiceSample
    {
          DataSet dsAllShiping = new DataSet();
        public static void Main(string[] args)
        {
            //// TODO: Set the below configuration variables before attempting to run
            
            //// Developer AWS access key
            //string accessKey = "replaceWithAccessKey";

            //// Developer AWS secret key
            //string secretKey = "replaceWithSecretKey";

            //// The client application name
            //string appName = "Curtainsonbudget Amazon";

            //// The client application version
            //string appVersion = "1.0";

            //// The endpoint for region service and version (see developer guide)
            //// ex: https://mws.amazonservices.com
            //string serviceURL = "https://mws.amazonservices.com";

            //// Create a configuration object
            //MWSMerchantFulfillmentServiceConfig config = new MWSMerchantFulfillmentServiceConfig();
            //config.ServiceURL = serviceURL;
            //// Set other client connection configurations here if needed
            //// Create the client itself
            //MWSMerchantFulfillmentService client = new MWSMerchantFulfillmentServiceClient(accessKey, secretKey, appName, appVersion, config);

            //MWSMerchantFulfillmentServiceSample sample = new MWSMerchantFulfillmentServiceSample(client);

            //// Uncomment the operation you'd like to test here
            //// TODO: Modify the request created in the Invoke method to be valid

            //try
            //{
            //    IMWSResponse response = null;
            //    // response = sample.InvokeCancelShipment();
            //    // response = sample.InvokeCreateShipment();
            //    // response = sample.InvokeGetEligibleShippingServices();
            //    // response = sample.InvokeGetShipment();
            //    // response = sample.InvokeGetServiceStatus();
            //    // Console.WriteLine("Response:");
            //    ResponseHeaderMetadata rhmd = response.ResponseHeaderMetadata;
            //    // We recommend logging the request id and timestamp of every call.
            //    //  Console.WriteLine("RequestId: " + rhmd.RequestId);
            //    // Console.WriteLine("Timestamp: " + rhmd.Timestamp);
            //    string responseXml = response.ToXML();
            //    //  Console.WriteLine(responseXml);
            //}
            //catch (MWSMerchantFulfillmentServiceException ex)
            //{
            //    // Exception properties are important for diagnostics.
            //    ResponseHeaderMetadata rhmd = ex.ResponseHeaderMetadata;
            //    //   Console.WriteLine("Service Exception:");
            //    if (rhmd != null)
            //    {
            //        //  Console.WriteLine("RequestId: " + rhmd.RequestId);
            //        // Console.WriteLine("Timestamp: " + rhmd.Timestamp);
            //    }
            //    //Console.WriteLine("Message: " + ex.Message);
            //    //Console.WriteLine("StatusCode: " + ex.StatusCode);
            //    //Console.WriteLine("ErrorCode: " + ex.ErrorCode);
            //    //Console.WriteLine("ErrorType: " + ex.ErrorType);
            //    //throw ex;
            //}

        }

        private readonly MWSMerchantFulfillmentService client;

        public MWSMerchantFulfillmentServiceSample(MWSMerchantFulfillmentService client)
        {
            this.client = client;
        }
        //public GetEligibleShippingServicesResponse GetAllEligibleShippingServices(string sellerId, string mwsAuthToken, Decimal Length, Decimal Width, Decimal Height, Decimal weight, DataTable dtorder)
        //{
        //    GetEligibleShippingServicesResponse res = new GetEligibleShippingServicesResponse();
        //    res = InvokeGetEligibleShippingServices(sellerId, mwsAuthToken,Length,   Width,   Height,   weight,   dtorder);
        //    return res;
        //}
        public CancelShipmentResponse InvokeCancelShipment(string sellerId, string shipmentId)
        {
            // Create a request.
            CancelShipmentRequest request = new CancelShipmentRequest();
            request.SellerId = sellerId;
            string mwsAuthToken = "";
            request.MWSAuthToken = mwsAuthToken;
            request.ShipmentId = shipmentId;
            
            return this.client.CancelShipment(request);
        }

        public CreateShipmentResponse InvokeCreateShipment(string AmazonOrderId, string sellerId, string mwsAuthToken, string shippingServiceId, string shippingServiceOfferId, Decimal Length, Decimal Width, Decimal Height,Decimal weight,DataTable dtorder)
        {
            try
            {
                dsAllShiping = Solution.Bussines.Components.CommonComponent.GetCommonDataSet("SELECT ConfigValue,ConfigName FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname in ('Shipping.CompanyName','Shipping.OriginAddress','Shipping.OriginAddress2','Shipping.OriginCity','Shipping.OriginState','Shipping.OriginZip','Shipping.OriginCountry','Shipping.OriginEmail','Shipping.OriginPhone','Shipping.DeliveryExperience','Shipping.DeclaredValue','Shipping.LabelFormat') ");
                // Create a request.
                CreateShipmentRequest request = new CreateShipmentRequest();
                // string sellerId = "example";
                request.SellerId = sellerId;
                // string mwsAuthToken = "example";
                request.MWSAuthToken = mwsAuthToken;


                ShipmentRequestDetails shipmentRequestDetails = new ShipmentRequestDetails();
                request.ShipmentRequestDetails = shipmentRequestDetails;
                //string shippingServiceId = "example";
                
                request.ShippingServiceId = shippingServiceId;
                // string shippingServiceOfferId = "example";
                request.ShippingServiceOfferId = shippingServiceOfferId;
                string hazmatType = "None";
                request.HazmatType = hazmatType;
                shipmentRequestDetails.AmazonOrderId = AmazonOrderId;
                shipmentRequestDetails.SellerOrderId = "";
                //  shipmentRequestDetails.MustArriveByDate = Convert.ToDateTime(String.Format(@"{0:yyyy-MM-dd\THH:mm:ssZ}", Convert.ToDateTime(DateTime.Now.Date.ToString())));
                PackageDimensions objPackageDimensions = new PackageDimensions();

                objPackageDimensions.Length = Convert.ToDecimal(Length);
                objPackageDimensions.Width = Convert.ToDecimal(Width);
                objPackageDimensions.Height = Convert.ToDecimal(Height);
                objPackageDimensions.Unit = "inches";
                objPackageDimensions.PredefinedPackageDimensions = "";
                shipmentRequestDetails.PackageDimensions = objPackageDimensions;

                Weight objWeight = new Weight();
                objWeight.Value = Convert.ToDecimal(weight) * Convert.ToDecimal(16);
                objWeight.Unit = "ounces";
                shipmentRequestDetails.Weight = objWeight;

                Address onjShipFromAddress = new Address();
                //   shipmentRequestDetails.ShipDate = Convert.ToDateTime(String.Format(@"{0:yyyy-MM-dd\THH:mm:ssZ}", Convert.ToDateTime(DateTime.Now.Date.ToString())));
                //onjShipFromAddress.Name = AppLogic.AppConfigs("Shipping.CompanyName");
                //onjShipFromAddress.AddressLine1 = AppLogic.AppConfigs("Shipping.OriginAddress");
                //onjShipFromAddress.AddressLine2 = AppLogic.AppConfigs("Shipping.OriginAddress2");

                //onjShipFromAddress.City = AppLogic.AppConfigs("Shipping.OriginCity");
                //onjShipFromAddress.StateOrProvinceCode = AppLogic.AppConfigs("Shipping.OriginState");
                //onjShipFromAddress.PostalCode = AppLogic.AppConfigs("Shipping.OriginZip");
                //onjShipFromAddress.CountryCode = AppLogic.AppConfigs("Shipping.OriginCountry");
                //onjShipFromAddress.Email = AppLogic.AppConfigs("Shipping.OriginEmail"); //"hasanikl@yahoo.com"; new
                //onjShipFromAddress.Phone = AppLogic.AppConfigs("Shipping.OriginPhone");
                //shipmentRequestDetails.ShipFromAddress = onjShipFromAddress;
                //ShippingServiceOptions objShippingServiceOptions = new ShippingServiceOptions();
                //objShippingServiceOptions.DeliveryExperience = AppLogic.AppConfigs("Shipping.DeliveryExperience");// "DeliveryConfirmationWithoutSignature"; New
                //objShippingServiceOptions.CarrierWillPickUp = false;
                //CurrencyAmount objCurrencyAmount = new CurrencyAmount();
                //objCurrencyAmount.CurrencyCode = "USD";
                //objCurrencyAmount.Amount = Convert.ToDecimal(AppLogic.AppConfigs("Shipping.DeclaredValue").ToString()); //new
                //objShippingServiceOptions.DeclaredValue = objCurrencyAmount;
                //objShippingServiceOptions.LabelFormat = AppLogic.AppConfigs("Shipping.LabelFormat");// "ShippingServiceDefault"; New



                onjShipFromAddress.Name = "Half Price Drapes";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.CompanyName'")[0]["ConfigValue"].ToString();
                onjShipFromAddress.AddressLine1 = "600 Wharton Circle SW";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginAddress'")[0]["ConfigValue"].ToString();// AppLogic.AppConfigs("Shipping.OriginAddress");
                onjShipFromAddress.AddressLine2 = "";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginAddress2'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginAddress2");

                onjShipFromAddress.City = "Atlanta";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginCity'")[0]["ConfigValue"].ToString();// AppLogic.AppConfigs("Shipping.OriginCity");
                onjShipFromAddress.StateOrProvinceCode = "GA";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginState'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginState");
                onjShipFromAddress.PostalCode = "30336";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginZip'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginZip");
                onjShipFromAddress.CountryCode = dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginCountry'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginCountry");
                onjShipFromAddress.Email = dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginEmail'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginEmail"); //"hasanikl@yahoo.com";
                onjShipFromAddress.Phone = dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginPhone'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginPhone");
                shipmentRequestDetails.ShipFromAddress = onjShipFromAddress;
                ShippingServiceOptions objShippingServiceOptions = new ShippingServiceOptions();
                objShippingServiceOptions.DeliveryExperience = dsAllShiping.Tables[0].Select("ConfigName='Shipping.DeliveryExperience'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.DeliveryExperience");
                objShippingServiceOptions.CarrierWillPickUp = false;
                CurrencyAmount objCurrencyAmount = new CurrencyAmount();
                objCurrencyAmount.CurrencyCode = "USD";
                objCurrencyAmount.Amount = Convert.ToDecimal(dsAllShiping.Tables[0].Select("ConfigName='Shipping.DeclaredValue'")[0]["ConfigValue"].ToString()); ;//AppLogic.AppConfigs("Shipping.DeclaredValue").ToString()
                objShippingServiceOptions.DeclaredValue = objCurrencyAmount;

                objShippingServiceOptions.LabelFormat = dsAllShiping.Tables[0].Select("ConfigName='Shipping.LabelFormat'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.LabelFormat");

                // objShippingServiceOptions.LabelFormat = "ShippingServiceDefault";
                shipmentRequestDetails.ShippingServiceOptions = objShippingServiceOptions;
                if(dtorder != null && dtorder.Rows.Count > 0)
                {
                    for (int i = 0; i < dtorder.Rows.Count; i++)
                    {
                        Item itm = new Item();
                        itm.OrderItemId = dtorder.Rows[i]["AmazonItemId"].ToString();
                        itm.Quantity = Convert.ToInt32(dtorder.Rows[i]["Qty"].ToString());
                        shipmentRequestDetails.ItemList.Add(itm);
                    }
                }
                
                //itm = new Item();
                //itm.OrderItemId = "63097082177842";
                //itm.Quantity = 3;
                //shipmentRequestDetails.ItemList.Add(itm);



                LabelCustomization objLabelCustomization = new LabelCustomization();
                objLabelCustomization.CustomTextForLabel = "";
                objLabelCustomization.StandardIdForLabel = "";
               // objLabelCustomization.StandardIdForLabel = "";
                shipmentRequestDetails.LabelCustomization = objLabelCustomization;
                request.ShipmentRequestDetails = shipmentRequestDetails;
                return this.client.CreateShipment(request);


                //           &HazmatType=None
                //&ShippingServiceId=FEDEX_PTP_PRIORITY_OVERNIGHT
                //&ShipmentRequestDetails.AmazonOrderId=903-1713775-3598252
                //&ShipmentRequestDetails.LabelCustomization.CustomTextForLabel=ABC123
                //&ShipmentRequestDetails.LabelCustomization.StandardIdForLabel=AmazonOrderId
                //&ShipmentRequestDetails.MustArriveByDate=2015-09-25T07%3A00%3A00Z
                //&ShipmentRequestDetails.PackageDimensions.Length=5
                //&ShipmentRequestDetails.PackageDimensions.Width=5
                //&ShipmentRequestDetails.PackageDimensions.Height=5
                //&ShipmentRequestDetails.PackageDimensions.Unit=inches
                //&ShipmentRequestDetails.Weight.Value=10
                //&ShipmentRequestDetails.Weight.Unit=ounces
                //&ShipmentRequestDetails.ShipDate=2015-09-23T20%3A10%3A56.829Z
                //&ShipmentRequestDetails.ShipFromAddress.Name=John%20Doe
                //&ShipmentRequestDetails.ShipFromAddress.AddressLine1=1234%20Westlake%20Ave
                //&ShipmentRequestDetails.ShipFromAddress.City=Seattle
                //&ShipmentRequestDetails.ShipFromAddress.StateOrProvinceCode=WA
                //&ShipmentRequestDetails.ShipFromAddress.PostalCode=98121
                //&ShipmentRequestDetails.ShipFromAddress.CountryCode=US
                //&ShipmentRequestDetails.ShipFromAddress.Email=example%40example.com
                //&ShipmentRequestDetails.ShipFromAddress.Phone=2061234567
                //&ShipmentRequestDetails.ShippingServiceOptions.DeliveryExperience=DeliveryConfirmationWithoutSignature
                //&ShipmentRequestDetails.ShippingServiceOptions.CarrierWillPickUp=false
                //&ShipmentRequestDetails.ShippingServiceOptions.DeclaredValue.CurrencyCode=USD
                //&ShipmentRequestDetails.ShippingServiceOptions.DeclaredValue.Amount=10.00
                //&ShipmentRequestDetails.ShippingServiceOptions.LabelFormat=ZPL203
                //&ShipmentRequestDetails.ItemList.Item.1.OrderItemId=40525960574974
                //&ShipmentRequestDetails.ItemList.Item.1.Quantity=1
            }
            catch (MWSMerchantFulfillmentServiceException ex)
            {
                ResponseHeaderMetadata rhmd = ex.ResponseHeaderMetadata;
            }
            return null;
        }

        public GetEligibleShippingServicesResponse InvokeGetEligibleShippingServices(string sellerId, string mwsAuthToken, Decimal Length, Decimal Width, Decimal Height,Decimal weight,DataTable dtorder)
        {
            try
            {
                dsAllShiping = Solution.Bussines.Components.CommonComponent.GetCommonDataSet("SELECT ConfigValue,ConfigName FROM tb_AppConfig WHERE StoreID=3 AND ISNULL(Deleted,0)=0 and Configname in ('Shipping.CompanyName','Shipping.OriginAddress','Shipping.OriginAddress2','Shipping.OriginCity','Shipping.OriginState','Shipping.OriginZip','Shipping.OriginCountry','Shipping.OriginEmail','Shipping.OriginPhone','Shipping.DeliveryExperience','Shipping.DeclaredValue','Shipping.LabelFormat') ");
                // Create a request.
                GetEligibleShippingServicesRequest request = new GetEligibleShippingServicesRequest();
                //string sellerId = "example";
                request.SellerId = sellerId;
                // string mwsAuthToken = "example";
                request.MWSAuthToken = mwsAuthToken;
                ShipmentRequestDetails shipmentRequestDetails = new ShipmentRequestDetails();

                shipmentRequestDetails.AmazonOrderId = dtorder.Rows[0]["RefOrderId"].ToString();
                shipmentRequestDetails.SellerOrderId = "";
                //  shipmentRequestDetails.MustArriveByDate = Convert.ToDateTime(String.Format(@"{0:yyyy-MM-dd\THH:mm:ss}", Convert.ToDateTime(DateTime.Now.Date.AddDays(3).ToString())));
                PackageDimensions objPackageDimensions = new PackageDimensions();

                objPackageDimensions.Length = Convert.ToDecimal(Length);
                objPackageDimensions.Width = Convert.ToDecimal(Width);
                objPackageDimensions.Height = Convert.ToDecimal(Height);
                objPackageDimensions.Unit = "inches";
                objPackageDimensions.PredefinedPackageDimensions = "";
                shipmentRequestDetails.PackageDimensions = objPackageDimensions;

                Weight objWeight = new Weight();
                objWeight.Value = Convert.ToDecimal(weight) * Convert.ToDecimal(16);
                objWeight.Unit = "ounces";
                shipmentRequestDetails.Weight = objWeight;

                Address onjShipFromAddress = new Address();
                // shipmentRequestDetails.ShipDate = Convert.ToDateTime(String.Format(@"{0:yyyy-MM-dd\THH:mm:ss}", Convert.ToDateTime(DateTime.Now.Date.AddDays(3).ToString())));
                onjShipFromAddress.Name = "Half Price Drapes";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.CompanyName'")[0]["ConfigValue"].ToString();
                onjShipFromAddress.AddressLine1 = "600 Wharton Circle SW";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginAddress'")[0]["ConfigValue"].ToString();// AppLogic.AppConfigs("Shipping.OriginAddress");
                onjShipFromAddress.AddressLine2 = "";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginAddress2'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginAddress2");

                onjShipFromAddress.City = "Atlanta";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginCity'")[0]["ConfigValue"].ToString();// AppLogic.AppConfigs("Shipping.OriginCity");
                onjShipFromAddress.StateOrProvinceCode = "GA";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginState'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginState");
                onjShipFromAddress.PostalCode = "30336";// dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginZip'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginZip");
                onjShipFromAddress.CountryCode = dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginCountry'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginCountry");
                onjShipFromAddress.Email = dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginEmail'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginEmail"); //"hasanikl@yahoo.com";
                onjShipFromAddress.Phone = dsAllShiping.Tables[0].Select("ConfigName='Shipping.OriginPhone'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.OriginPhone");
                shipmentRequestDetails.ShipFromAddress = onjShipFromAddress;
                ShippingServiceOptions objShippingServiceOptions = new ShippingServiceOptions();
                objShippingServiceOptions.DeliveryExperience = dsAllShiping.Tables[0].Select("ConfigName='Shipping.DeliveryExperience'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.DeliveryExperience");
                objShippingServiceOptions.CarrierWillPickUp = false;
                CurrencyAmount objCurrencyAmount = new CurrencyAmount();
                objCurrencyAmount.CurrencyCode = "USD";
                objCurrencyAmount.Amount = Convert.ToDecimal(dsAllShiping.Tables[0].Select("ConfigName='Shipping.DeclaredValue'")[0]["ConfigValue"].ToString()); ;//AppLogic.AppConfigs("Shipping.DeclaredValue").ToString()
                objShippingServiceOptions.DeclaredValue = objCurrencyAmount;

                objShippingServiceOptions.LabelFormat = dsAllShiping.Tables[0].Select("ConfigName='Shipping.LabelFormat'")[0]["ConfigValue"].ToString();//AppLogic.AppConfigs("Shipping.LabelFormat");
                //ShippingService objss = new ShippingService();

                //objss.CarrierName = "UPS";
                //objss.ShippingServiceName = "Standard";

                
                shipmentRequestDetails.ShippingServiceOptions = objShippingServiceOptions;
                //Item itm = new Item();
                //itm.OrderItemId = "27320911048362";
                //itm.Quantity = 3;
                //shipmentRequestDetails.ItemList.Add(itm);
                //itm = new Item();
                //itm.OrderItemId = "63097082177842";
                //itm.Quantity = 3;
                //shipmentRequestDetails.ItemList.Add(itm);

                if (dtorder != null && dtorder.Rows.Count > 0)
                {
                    for (int i = 0; i < dtorder.Rows.Count; i++)
                    {
                        Item itm = new Item();
                        itm.OrderItemId = dtorder.Rows[i]["AmazonItemId"].ToString();
                        itm.Quantity = Convert.ToInt32(dtorder.Rows[i]["Qty"].ToString());
                        shipmentRequestDetails.ItemList.Add(itm);
                    }
                }

                LabelCustomization objLabelCustomization = new LabelCustomization();
                objLabelCustomization.CustomTextForLabel = "";
                objLabelCustomization.StandardIdForLabel = "";
                shipmentRequestDetails.LabelCustomization = objLabelCustomization;
                request.ShipmentRequestDetails = shipmentRequestDetails;
                return this.client.GetEligibleShippingServices(request);
            }
            catch (MWSMerchantFulfillmentServiceException ex)
            {
                ResponseHeaderMetadata rhmd = ex.ResponseHeaderMetadata;
            }
            return null;
        }

        public GetShipmentResponse InvokeGetShipment(string sellerId, string shipmentId)
        {
            // Create a request.
            GetShipmentRequest request = new GetShipmentRequest();
            
            request.SellerId = sellerId;
            string mwsAuthToken = "";
            request.MWSAuthToken = mwsAuthToken;
             
            request.ShipmentId = shipmentId;
            return this.client.GetShipment(request);
        }

        public GetServiceStatusResponse InvokeGetServiceStatus()
        {
            // Create a request.
            GetServiceStatusRequest request = new GetServiceStatusRequest();
            string sellerId = "";
            request.SellerId = sellerId;
            string mwsAuthToken = "";
            request.MWSAuthToken = mwsAuthToken;
            return this.client.GetServiceStatus(request);
        }




    }
}
