using System;
using System.IO;
using System.Web.Services.Protocols;
using System.Data;
using Solution.ShippingMethods.FedExShipServiceWebReference;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
//
// Sample code to call the FedEx Ship Service - Ground International Shipment from United States to Canada
//

namespace Solution.ShippingMethods
{
    public class InterNationalShippinglabelFedExMPS
    {
        #region LocalVarible

        string imagepath = string.Empty;
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
        public String oCity
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
        ServiceType st = new ServiceType();
        string rdvalue = string.Empty;
        string totalImg = string.Empty;
        #endregion

        public string ShipLableFedExInterna(int WareHouseID, DataSet ds, string wt, DataTable dt, string imgpath, string rdval)
        {
            string ErrStr = string.Empty;

            string ShoppingCartID= string.Empty;
            try
            {
                rdvalue = rdval;
                string Ordernumber = ds.Tables[0].Rows[0]["OrderNumber"].ToString();
                ShoppingCartID += dt.Rows[0]["ShippingCartID"];
                imagepath = imgpath;
                ProcessShipmentRequest masterRequest = CreateMasterShipmentRequest(WareHouseID, ds, dt, wt, rdvalue);
                ShipService service = new ShipService();
                ProcessShipmentReply masterReply = service.processShipment(masterRequest);
                if ((masterReply.HighestSeverity != NotificationSeverityType.ERROR) && (masterReply.HighestSeverity != NotificationSeverityType.FAILURE))
                {
                    if (dt.Rows.Count > 1)
                    {
                        ShowShipmentReply(masterReply, Ordernumber, Convert.ToInt32(dt.Rows[0]["packageid"].ToString()), dt.Rows[0]["ShippingCartID"].ToString(), dt.Rows[0]["ProductID"].ToString(), "", WareHouseID);
                        for (int cnt = 1; cnt < dt.Rows.Count; cnt++)
                        {
                            ProcessShipmentRequest childRequest = CreateChildShipmentRequest(WareHouseID, masterReply, masterRequest, dt.Rows[cnt], Convert.ToString(cnt + 1), ds, dt, wt, rdval);
                            ProcessShipmentReply childReply = service.processShipment(childRequest);
                            if ((childReply.HighestSeverity != NotificationSeverityType.ERROR) && (childReply.HighestSeverity != NotificationSeverityType.FAILURE)) // check if the call was successful
                                ShowShipmentReply(childReply, Ordernumber, Convert.ToInt32(dt.Rows[cnt]["packageid"].ToString()), dt.Rows[cnt]["ShippingCartID"].ToString(), dt.Rows[cnt]["ProductID"].ToString(), "", WareHouseID);
                            else
                                ErrStr = childReply.Notifications[0].Message;
                        }
                    }
                }
            }
            catch (SoapException e)
            {
                ErrStr = e.Detail.InnerText;
            }
            catch (Exception e)
            {
                ErrStr = e.Message;
            }
            if (string.IsNullOrEmpty(totalImg)) totalImg = ErrStr;
            return totalImg;

        }

        private static ProcessShipmentRequest CreateChildShipmentRequest(int WareHouseID, ProcessShipmentReply masterReply, ProcessShipmentRequest masterRequest, DataRow dr, string seq, DataSet ds, DataTable dt, string wt, string rdvalue)
        {
            // Build the Child ShipmentRequest
            ProcessShipmentRequest childRequest = new ProcessShipmentRequest();
            //
            childRequest.WebAuthenticationDetail = masterRequest.WebAuthenticationDetail;
            childRequest.ClientDetail = masterRequest.ClientDetail;
            //
            childRequest.TransactionDetail = new TransactionDetail();
            childRequest.TransactionDetail.CustomerTransactionId = "***Ground International MPS Shipment v8 Request - Child using VC#***"; // The client will get the same value back in the response
            //
            childRequest.Version = masterRequest.Version;
            //
            SetShipmentDetails(WareHouseID, childRequest, rdvalue, wt, ds, dt);
            //
            childRequest.RequestedShipment.MasterTrackingId = new TrackingId(); // Master tracking number details
            childRequest.RequestedShipment.MasterTrackingId.TrackingNumber = masterReply.CompletedShipmentDetail.CompletedPackageDetails[0].TrackingIds[0].TrackingNumber;
            childRequest.RequestedShipment.MasterTrackingId.TrackingIdType = TrackingIdType.GROUND;
            childRequest.RequestedShipment.MasterTrackingId.TrackingIdTypeSpecified = true;
            //
            SetPackageLineItems(childRequest, seq, dr);
            //
            return childRequest;
        }

        private static void SetShipmentDetails(int WareHouseID, ProcessShipmentRequest request, string rdvalue, string wt, DataSet ds, DataTable dt)
        {
            request.RequestedShipment = new RequestedShipment();
            request.RequestedShipment.ShipTimestamp = DateTime.Now;
            FedExCommon oFedExCommon = new FedExCommon();
            request.RequestedShipment.ServiceType = oFedExCommon.getservicetype(rdvalue); // Service types are FEDEX_GROUND ...
            request.RequestedShipment.PackagingType = PackagingType.YOUR_PACKAGING; // Packaging type YOUR_PACKAGING, ...
            request.RequestedShipment.RateRequestTypes = new RateRequestType[1] { RateRequestType.ACCOUNT }; // Rate types requested LIST, MULTIWEIGHT, ...
            request.RequestedShipment.PackageCount = dt.Rows.Count.ToString(); // Package count 2
            request.RequestedShipment.PackageDetail = RequestedPackageDetailType.INDIVIDUAL_PACKAGES;
            request.RequestedShipment.PackageDetailSpecified = true;
            request.RequestedShipment.TotalInsuredValue = new Money();
            decimal totalvalue = 0;
            for (int iprice = 0; iprice < dt.Rows.Count; iprice++)
            {
                totalvalue += Convert.ToDecimal(dt.Rows[iprice]["ProductPrice"].ToString());
            }

            request.RequestedShipment.TotalInsuredValue.Amount = Convert.ToDecimal(totalvalue);
            request.RequestedShipment.TotalInsuredValue.Currency = "USD";
            request = oFedExCommon.SetAddress(WareHouseID, request, ds);
            SetPayment(request);
            SetLabelDetails(request);
            #region SetInternationalDetails

            CountryComponent objCountry = new CountryComponent();
            string TwoLetterISO = objCountry.GetCountryCodeByNameForShippingLabel(ds.Tables[0].Rows[0]["ShippingCountry"].ToString());
            request = SetInternationalDetails(request, ds.Tables[0].Rows[0]["OrderTotal"].ToString(), TwoLetterISO);

            #endregion
            request = SetCommodityDetails(request, ds.Tables[0].Rows[0]["OrderNumber"].ToString(), dt);

        }
        private static ProcessShipmentRequest CreateMasterShipmentRequest(int WareHouseID, DataSet ds, DataTable dt, string wt, string rdvalue)
        {
            // Build the Master ShipmentRequest
            ProcessShipmentRequest masterRequest = new ProcessShipmentRequest();
            //
            FedExCommon oFedExCommon = new FedExCommon();
            masterRequest = oFedExCommon.LoginDetailfedEx(masterRequest);

            masterRequest.TransactionDetail = new TransactionDetail();
            masterRequest.TransactionDetail.CustomerTransactionId = "***Ground International MPS Shipment v8 Request - Master using VC#***"; // The client will get the same value back in the response
            //
            masterRequest.Version = new VersionId(); // WSDL version information, value is automatically set from wsdl
            //
            SetShipmentDetails(WareHouseID, masterRequest, rdvalue, wt, ds, dt);

            SetPackageLineItems(masterRequest, "1", dt.Rows[0]);

            return masterRequest;
        }

        private static void SetPackageLineItems(ProcessShipmentRequest request, string sequenceNumber, DataRow dr)
        {
            decimal weightValue = Convert.ToDecimal(dr["ProWeight"]);
            string length = dr["Length"].ToString();
            string width = dr["Width"].ToString();
            string height = dr["Height"].ToString();
            request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
            request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
            request.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber = sequenceNumber;
            request.RequestedShipment.RequestedPackageLineItems[0].Weight = new Weight();
            request.RequestedShipment.RequestedPackageLineItems[0].Weight.Value = weightValue;
            request.RequestedShipment.RequestedPackageLineItems[0].Weight.Units = WeightUnits.LB;
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions = new Dimensions();
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Length = length;
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Width = width;
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Height = height;
            request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Units = LinearUnits.IN;
            request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue = new Money(); // insured value
            request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Amount = Convert.ToDecimal(dr["ProductPrice"].ToString().Replace("$", ""));
            request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency = "USD";

            // Customer references
            //request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = new CustomerReference[3] { new CustomerReference(), new CustomerReference(), new CustomerReference() };
            //request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[0].CustomerReferenceType = CustomerReferenceType.CUSTOMER_REFERENCE;
            //request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[0].Value = "GR4567892";
            //request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[1].CustomerReferenceType = CustomerReferenceType.INVOICE_NUMBER;
            //request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[1].Value = "INV4567892";
            //request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[2].CustomerReferenceType = CustomerReferenceType.P_O_NUMBER;
            //request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[2].Value = "PO4567892";
        }



        private static void SetPayment(ProcessShipmentRequest request)
        {
            request.RequestedShipment.ShippingChargesPayment = new Payment();
            request.RequestedShipment.ShippingChargesPayment.PaymentType = PaymentType.SENDER;
            request.RequestedShipment.ShippingChargesPayment.Payor = new Payor();
            request.RequestedShipment.ShippingChargesPayment.Payor.AccountNumber = AppLogic.AppConfigs("FedEx.AccountNumber");  // Replace "XXX" with the payor account number
            request.RequestedShipment.ShippingChargesPayment.Payor.CountryCode = AppLogic.AppConfigs("Localization.StoreCurrency");
        }

        private static void SetLabelDetails(ProcessShipmentRequest request)
        {
            request.RequestedShipment.LabelSpecification = new LabelSpecification(); // Label specification
            request.RequestedShipment.LabelSpecification.ImageType = ShippingDocumentImageType.PNG; // Image types PDF, PNG, DPL, ...
            request.RequestedShipment.LabelSpecification.ImageTypeSpecified = true;
            request.RequestedShipment.LabelSpecification.LabelFormatType = LabelFormatType.COMMON2D;
            //request.RequestedShipment.LabelSpecification.LabelPrintingOrientation = LabelPrintingOrientationType.;
            request.RequestedShipment.LabelSpecification.LabelStockType = LabelStockType.PAPER_4X6;
            request.RequestedShipment.LabelSpecification.LabelStockTypeSpecified = true;
        }



        private static ProcessShipmentRequest SetInternationalDetails(ProcessShipmentRequest request, string OT, string countrycode)
        {
            try
            {
                request.RequestedShipment.InternationalDetail = new InternationalDetail(); // International details
                request.RequestedShipment.InternationalDetail.DutiesPayment = new Payment();
                request.RequestedShipment.InternationalDetail.DutiesPayment.PaymentType = PaymentType.SENDER;
                request.RequestedShipment.InternationalDetail.DutiesPayment.Payor = new Payor();
                request.RequestedShipment.InternationalDetail.DutiesPayment.Payor.AccountNumber = AppLogic.AppConfigs("FedEx.AccountNumber");  // Replace "XXX" with the payor account number
                request.RequestedShipment.InternationalDetail.DutiesPayment.Payor.CountryCode = countrycode;
                request.RequestedShipment.InternationalDetail.DocumentContent = InternationalDocumentContentType.NON_DOCUMENTS;
                request.RequestedShipment.InternationalDetail.CustomsValue = new Money();
                request.RequestedShipment.InternationalDetail.CustomsValue.Amount = Convert.ToDecimal(OT);
                request.RequestedShipment.InternationalDetail.CustomsValue.Currency = AppLogic.AppConfigs("Localization.StoreCurrency");

                // Elements for EEI/SED shipments
                //request.RequestedShipment.InternationalDetail.ExportDetail = new ExportDetail();
                //request.RequestedShipment.InternationalDetail.ExportDetail.ExportComplianceStatement = "AESX20091127123456";
                request.RequestedShipment.InternationalDetail.DocumentContent = InternationalDocumentContentType.NON_DOCUMENTS;

                //

            }
            catch { }
            return request;
        }

        private static ProcessShipmentRequest SetCommodityDetails(ProcessShipmentRequest request, string OrderNumber, DataTable dt)
        {
            try
            {
                #region Demo
                if (dt != null && dt.Rows.Count > 0)
                {
                    int totcnt = dt.Rows.Count;
                    //request.RequestedShipment.InternationalDetail.Commodities = new Commodity[0] { };
                    if (totcnt == 1) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[1] { new Commodity() };
                    else if (totcnt == 2) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[2] { new Commodity(), new Commodity() };
                    else if (totcnt == 3) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[3] { new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 4) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[4] { new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 5) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[5] { new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 6) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[6] { new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 7) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[7] { new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 8) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[8] { new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 9) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[9] { new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 10) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[10] { new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 11) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[11] { new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    else if (totcnt == 12) request.RequestedShipment.InternationalDetail.Commodities = new Commodity[12] { new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity(), new Commodity() };
                    for (int cnt = 0; cnt < dt.Rows.Count; cnt++)
                    {
                        //request.RequestedShipment.InternationalDetail.Commodities = new Commodity[1] { new Commodity() };
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].NumberOfPieces = dt.Rows.Count.ToString();
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].Description = "e-Commerce Product Shipment.";
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].CountryOfManufacture = AppLogic.AppConfigs("Shipping.OriginCountry");
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].Weight = new Weight();
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].Weight.Value = Convert.ToDecimal(dt.Rows[cnt]["ProWeight"]);
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].Weight.Units = WeightUnits.LB;
                        if (dt.Rows[cnt]["Quantity"].ToString().Contains("~"))
                            request.RequestedShipment.InternationalDetail.Commodities[cnt].Quantity = "1";
                        else
                            request.RequestedShipment.InternationalDetail.Commodities[cnt].Quantity = dt.Rows[cnt]["Quantity"].ToString();
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].QuantityUnits = "EA";
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].UnitPrice = new Money();
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].UnitPrice.Amount = Convert.ToDecimal(dt.Rows[cnt]["ProductPrice"].ToString());
                        request.RequestedShipment.InternationalDetail.Commodities[cnt].UnitPrice.Currency = AppLogic.AppConfigs("Localization.StoreCurrency");
                    }
                }

                #endregion

            }
            catch { }
            return request;

        }

        public string ShowShipmentReply(ProcessShipmentReply reply, string OrderNumber, int num, string CartItemID, string productid, string ptype, int WareHouseID)
        {

            try
            {
                string ImgName = string.Empty;
                int cndat = 0;
                foreach (CompletedPackageDetail packageDetail in reply.CompletedShipmentDetail.CompletedPackageDetails)
                {
                    ShowTrackingDetails(packageDetail.TrackingIds);
                    ShowPackageRateDetails(packageDetail.PackageRating.PackageRateDetails);
                    if (null != packageDetail.Label.Parts[0].Image)
                    {
                        string fileName = packageDetail.TrackingIds[0].TrackingNumber;
                        fileName = "FedEx-Package" + num.ToString() + "_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(CartItemID)) ? "0" : CartItemID) + "@" +
                         DateTime.Now.Year.ToString() +
                         DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() +
                         DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() +
                         DateTime.Now.Second.ToString() + "-" + num.ToString() + ".png";
                        totalImg += fileName + "#";
                        SaveLabel(imagepath + fileName, packageDetail.Label.Parts[0].Image);
                    }
                    cndat++;

                    try
                    {


                        ShippingComponent.UpdateorderedShoppingcartitems(packageDetail.TrackingIds[0].TrackingNumber, productid, "FEDEX", Convert.ToInt32(CartItemID), 1, WareHouseID, num);

                       
                    }
                    catch { }
                }
            }
            catch { }
            return totalImg;
        }

        private static void ShowTrackingDetails(TrackingId[] TrackingIds)
        {
            // Tracking information for each package
            Console.WriteLine("Tracking details");
            if (TrackingIds != null)
            {
                for (int i = 0; i < TrackingIds.Length; i++)
                {
                    Console.WriteLine("Tracking # {0} Form ID {1}", TrackingIds[i].TrackingNumber, TrackingIds[i].FormId);
                }
            }
        }

        private static void ShowPackageRateDetails(PackageRateDetail[] PackageRateDetails)
        {
            Console.WriteLine("\nRate details");
            foreach (PackageRateDetail ratedPackage in PackageRateDetails)
            {
                if (ratedPackage.BillingWeight != null)
                    Console.WriteLine("Billing weight {0} {1}", ratedPackage.BillingWeight.Value, ratedPackage.BillingWeight.Units);
                if (ratedPackage.BaseCharge != null)
                    Console.WriteLine("Base charge {0} {1}", ratedPackage.BaseCharge.Amount, ratedPackage.BaseCharge.Currency);
                if (ratedPackage.NetCharge != null)
                    Console.WriteLine("Net charge {0} {1}", ratedPackage.NetCharge.Amount, ratedPackage.NetCharge.Currency);
                if (ratedPackage.Surcharges != null)
                {
                    foreach (Surcharge surcharge in ratedPackage.Surcharges) // Individual surcharge for each package
                        Console.WriteLine(" {0} surcharge {1} {2}", surcharge.SurchargeType, surcharge.Amount.Amount, surcharge.Amount.Currency);
                }
                if (ratedPackage.TotalSurcharges != null)
                    Console.WriteLine("Total surcharge {0} {1}", ratedPackage.TotalSurcharges.Amount, ratedPackage.TotalSurcharges.Currency);
            }
        }

        private static void ShowBarcodeDetails(PackageBarcodes Barcodes)
        {
            // Barcode information for each package
            Console.WriteLine("\nBarcode details");
            if (Barcodes != null)
            {
                if (Barcodes.StringBarcodes != null)
                {
                    for (int i = 0; i < Barcodes.StringBarcodes.Length; i++)
                    {
                        Console.WriteLine("String barcode {0} Type {1}", Barcodes.StringBarcodes[i].Value, Barcodes.StringBarcodes[i].Type);
                    }
                }

                if (Barcodes.BinaryBarcodes != null)
                {
                    for (int i = 0; i < Barcodes.BinaryBarcodes.Length; i++)
                    {
                        Console.WriteLine("Binary barcode Type {0}", Barcodes.BinaryBarcodes[i].Type);
                    }
                }
            }
        }

        private static void ShowPackageRouteDetails(ShipmentRoutingDetail RoutingDetail)
        {
            Console.WriteLine("\nRouting details");
            Console.WriteLine("URSA prefix {0} suffix {1}", RoutingDetail.UrsaPrefixCode, RoutingDetail.UrsaSuffixCode);
            Console.WriteLine("Service commitment {0} Airport ID {1}", RoutingDetail.DestinationLocationId, RoutingDetail.AirportId);

            if (RoutingDetail.DeliveryDaySpecified)
            {
                Console.WriteLine("Delivery day " + RoutingDetail.DeliveryDay);
            }
            if (RoutingDetail.DeliveryDateSpecified)
            {
                Console.WriteLine("Delivery date " + RoutingDetail.DeliveryDate.ToShortDateString());
            }
            Console.WriteLine("Transit time " + RoutingDetail.TransitTime);
        }

        private static void SaveLabel(string LabelFileName, byte[] LabelBuffer)
        {
            // Save label buffer to file
            FileStream LabelFile = new FileStream(LabelFileName, FileMode.Create);
            LabelFile.Write(LabelBuffer, 0, LabelBuffer.Length);
            LabelFile.Close();

            // Display label in Acrobat
            //DisplayLabel(LabelFileName);
        }

        private static void DisplayLabel(string LabelFileName)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(LabelFileName);
            info.UseShellExecute = true;
            info.Verb = "open";
            System.Diagnostics.Process.Start(info);
        }
    }
}