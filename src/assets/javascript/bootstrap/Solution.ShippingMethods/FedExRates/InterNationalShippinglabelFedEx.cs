using System;
using System.IO;
using System.Web.Services.Protocols;

using System.Data;
using Solution.ShippingMethods.FedExShipServiceWebReference;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Drawing;
using System.Drawing.Drawing2D;
using PdfSharp.Drawing;
using System.Drawing.Imaging;

//
// Sample code to call the FedEx Ship Service - Ground International Shipment from United States to Canada
//

namespace Solution.ShippingMethods
{
    public class InterNationalShippinglabelFedEx
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
        #endregion

        public string ShipLableFedExInterna(int WareHouseID, DataSet ds, string wt, DataTable dt, string imgpath, string rdvalue)
        {
            string ErrStr = string.Empty;
            try
            {
                imagepath = imgpath;
                ProcessShipmentRequest request = new ProcessShipmentRequest();

                FedExCommon oFedExCommon = new FedExCommon();
                request = oFedExCommon.LoginDetailfedEx(request);
                request.TransactionDetail = new TransactionDetail();
                request.TransactionDetail.CustomerTransactionId = "***Ground International Shipment v8 Request using VC#***"; // The client will get the same value back in the response
                request.Version = new VersionId(); // WSDL version information, value is automatically set from wsdl
                decimal totalvalue = 0;
                for (int iprice = 0; iprice < dt.Rows.Count; iprice++)
                {
                    totalvalue += Convert.ToDecimal(dt.Rows[iprice]["ProductPrice"].ToString());
                }

                SetShipmentDetails(request, rdvalue, totalvalue);

                //SetShipmentDetails(request, rdvalue);
                request = oFedExCommon.SetAddress(WareHouseID, request, ds);
                SetPayment(request);
                SetLabelDetails(request);
                StateComponent objState = new StateComponent();
                string Abbreviation = objState.GetStateCodeByName(ds.Tables[0].Rows[0]["ShippingState"].ToString());
                CountryComponent objCountry = new CountryComponent();
                string TwoLetterISO = objCountry.GetCountryCodeByNameForShippingLabel(ds.Tables[0].Rows[0]["ShippingCountry"].ToString());
                Decimal OrWeight = 0; // Math.Round(Convert.ToDecimal(((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["OrderWeight"].ToString())) ? "0" : ds.Tables[0].Rows[0]["OrderWeight"].ToString())), 0);
                string Error = "";
                if (OrWeight == Decimal.Zero)
                    Decimal.TryParse(wt, out OrWeight);
                request = SetPackageLineItems(request, OrWeight, ds.Tables[0].Rows[0]["OrderNumber"].ToString(), dt);
                request = SetInternationalDetails(request, ds, TwoLetterISO, dt);
                FedExShipServiceWebReference.ShipService service = new FedExShipServiceWebReference.ShipService(); // Initialize the service
                service.Url = service.Url.ToString().Replace("gateway", "gatewaybeta");
                try
                {
                    ProcessShipmentReply reply = service.processShipment(request);
                    if ((reply.HighestSeverity != NotificationSeverityType.ERROR) && (reply.HighestSeverity != NotificationSeverityType.FAILURE)) // check if the call was successful
                    {
                        ErrStr = ShowShipmentReply(reply, ds.Tables[0].Rows[0]["OrderNumber"].ToString(), dt.Rows[0]["ShippingCartID"].ToString(), dt, WareHouseID);
                    }
                    else
                    {
                        ErrStr = reply.Notifications[0].Message;
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
            }
            catch { }
            return ErrStr;

        }

        private static void SetShipmentDetails(ProcessShipmentRequest request, string rdvalue, decimal totalInsuredval)
        {
            try
            {

                FedExCommon oFedExCommon = new FedExCommon();
                request.RequestedShipment = new RequestedShipment();
                request.RequestedShipment.ShipTimestamp = DateTime.Now; // Ship date and time
                request.RequestedShipment.DropoffType = DropoffType.REGULAR_PICKUP;
                request.RequestedShipment.ServiceType = oFedExCommon.getservicetype(rdvalue); // Service types are FEDEX_GROUND, ...
                request.RequestedShipment.PackagingType = PackagingType.YOUR_PACKAGING; // Packaging type YOUR_PACKAGING, ...
                request.RequestedShipment.RateRequestTypes = new RateRequestType[1] { RateRequestType.ACCOUNT }; // Rate types requested LIST, MULTIWEIGHT, ...
               
                //request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0] = ShipmentSpecialServiceType.RETURN_SHIPMENT;
               
                
                //request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType = ReturnType.PRINT_RETURN_LABEL;
                request.RequestedShipment.PackageCount = "1";
                request.RequestedShipment.PackageDetail = RequestedPackageDetailType.INDIVIDUAL_PACKAGES;
                request.RequestedShipment.PackageDetailSpecified = true;
                request.RequestedShipment.TotalInsuredValue = new Money();
                request.RequestedShipment.TotalInsuredValue.Amount = Convert.ToDecimal(totalInsuredval);
                request.RequestedShipment.TotalInsuredValue.Currency = "USD";
            }
            catch { }
        }

        private static void SetPayment(ProcessShipmentRequest request)
        {
            request.RequestedShipment.ShippingChargesPayment = new Payment();
            request.RequestedShipment.ShippingChargesPayment.PaymentType = PaymentType.SENDER;
            request.RequestedShipment.ShippingChargesPayment.Payor = new Payor();
            request.RequestedShipment.ShippingChargesPayment.Payor.AccountNumber = AppLogic.AppConfigs("FedEx.AccountNumber");  // Replace "XXX" with the payor account number
            request.RequestedShipment.ShippingChargesPayment.Payor.CountryCode = AppLogic.AppConfigs("Localization.StoreCurrency");

            request.RequestedShipment.SpecialServicesRequested = new ShipmentSpecialServicesRequested();
            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[1] { new ShipmentSpecialServiceType() };
            request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0] = ShipmentSpecialServiceType.RETURN_SHIPMENT;
            request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail = new ReturnShipmentDetail();
            request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType = ReturnType.PRINT_RETURN_LABEL;
            //request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[1];
            //request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0] = new ShipmentSpecialServiceType();
            //request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0] = ShipmentSpecialServiceType.RETURN_SHIPMENT;
           // request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType = ReturnType.FEDEX_TAG;
        }
        private static void SetReturn(ProcessShipmentRequest request)
        {
           
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
            //request.RequestedShipment.LabelSpecification.LabelPrintingOrientation = LabelPrintingOrientationType.TOP_EDGE_OF_TEXT_FIRST;
         //   request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes = new ShipmentSpecialServiceType[1] { ShipmentSpecialServiceType.RETURN_SHIPMENT };
            //request.RequestedShipment.SpecialServicesRequested.SpecialServiceTypes[0] = ShipmentSpecialServiceType.RETURN_SHIPMENT;
          //  request.RequestedShipment.SpecialServicesRequested.ReturnShipmentDetail.ReturnType = ReturnType.FEDEX_TAG;
            
            request.RequestedShipment.LabelSpecification.LabelStockType = LabelStockType.PAPER_4X6;
            request.RequestedShipment.LabelSpecification.LabelStockTypeSpecified = true;
            // request.RequestedShipment.LabelSpecification.LabelStockTypeSpecified = false;
        }

        private ProcessShipmentRequest SetPackageLineItems(ProcessShipmentRequest request, Decimal weight, string OrderNumber, DataTable dt)
        {
            try
            {
                //if (dt != null && dt.Rows.Count > 0)
                //{
                //    int totcnt = dt.Rows.Count;
                //    if (totcnt == 1) request.RequestedShipment.RequestedPackageLineItems[1] = new RequestedPackageLineItem[1] { new RequestedPackageLineItem() };
                //    if (totcnt == 2) request.RequestedShipment.RequestedPackageLineItems[2] = new RequestedPackageLineItem[2] { new RequestedPackageLineItem(), new RequestedPackageLineItem() };
                //    for (int i = 0; i < dt.Rows.Count; i++)
                //    {
                //        request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                //        request.RequestedShipment.RequestedPackageLineItems[i] = new RequestedPackageLineItem();
                //        request.RequestedShipment.RequestedPackageLineItems[i].SequenceNumber = Convert.ToString(i + 1);
                //        request.RequestedShipment.RequestedPackageLineItems[i].Weight = new Weight(); // Package weight information
                //        request.RequestedShipment.RequestedPackageLineItems[i].Weight.Value = Convert.ToDecimal(dt.Rows[i]["ProWeight"].ToString());
                //        request.RequestedShipment.RequestedPackageLineItems[i].Weight.Units = WeightUnits.LB;
                //        request.RequestedShipment.RequestedPackageLineItems[i].CustomerReferences = new CustomerReference[1] { new CustomerReference() }; // Reference details
                //        request.RequestedShipment.RequestedPackageLineItems[i].CustomerReferences[0].CustomerReferenceType = CustomerReferenceType.CUSTOMER_REFERENCE;
                //        request.RequestedShipment.RequestedPackageLineItems[i].CustomerReferences[0].Value = "No ref";
                //        request.RequestedShipment.RequestedPackageLineItems[i].Dimensions = new Dimensions();
                //        request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Length = dt.Rows[i]["Length"].ToString();
                //        request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Width = dt.Rows[i]["Width"].ToString();
                //        request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Height = dt.Rows[i]["Height"].ToString();
                //        request.RequestedShipment.RequestedPackageLineItems[i].Dimensions.Units = LinearUnits.IN;
                //    }
                //}

                request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[1];
                request.RequestedShipment.RequestedPackageLineItems[0] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[0].SequenceNumber = "1";
                request.RequestedShipment.RequestedPackageLineItems[0].Weight = new Weight(); // Package weight information
                request.RequestedShipment.RequestedPackageLineItems[0].Weight.Value = Convert.ToDecimal(dt.Rows[0]["ProWeight"].ToString());
                request.RequestedShipment.RequestedPackageLineItems[0].Weight.Units = WeightUnits.LB;
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences = new CustomerReference[1] { new CustomerReference() }; // Reference details
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[0].CustomerReferenceType = CustomerReferenceType.INVOICE_NUMBER;
                request.RequestedShipment.RequestedPackageLineItems[0].CustomerReferences[0].Value = OrderNumber.ToString();
                
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions = new Dimensions();
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Length = dt.Rows[0]["Length"].ToString();
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Width = dt.Rows[0]["Width"].ToString();
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Height = dt.Rows[0]["Height"].ToString();
                request.RequestedShipment.RequestedPackageLineItems[0].Dimensions.Units = LinearUnits.IN;
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue = new Money(); // insured value
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Amount = Convert.ToDecimal(dt.Rows[0]["ProductPrice"].ToString().Replace("$", ""));
                request.RequestedShipment.RequestedPackageLineItems[0].InsuredValue.Currency = "USD";



            }
            catch { }
            return request;
        }

        private ProcessShipmentRequest SetInternationalDetails(ProcessShipmentRequest request, DataSet ds, string countrycode, DataTable dt)
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
                request.RequestedShipment.InternationalDetail.CustomsValue.Amount = Convert.ToDecimal(ds.Tables[0].Rows[0]["OrderTotal"].ToString());
                request.RequestedShipment.InternationalDetail.CustomsValue.Currency = AppLogic.AppConfigs("Localization.StoreCurrency");

                // Elements for EEI/SED shipments
                request.RequestedShipment.InternationalDetail.ExportDetail = new ExportDetail();
                //request.RequestedShipment.InternationalDetail.ExportDetail.ExportComplianceStatement = "AESX20091127123456";
                request.RequestedShipment.InternationalDetail.DocumentContent = InternationalDocumentContentType.NON_DOCUMENTS;

                //
                SetCommodityDetails(request, ds.Tables[0].Rows[0]["OrderNumber"].ToString(), dt);
            }
            catch { }
            return request;
        }

        private static void SetCommodityDetails(ProcessShipmentRequest request, string OrderNumber, DataTable dt)
        {
            try
            {
                #region Demo
                //request.RequestedShipment.InternationalDetail.Commodities = new Commodity[1] { new Commodity() };
                //request.RequestedShipment.InternationalDetail.Commodities[0].NumberOfPieces = "1";
                //request.RequestedShipment.InternationalDetail.Commodities[0].Description = "Books";
                //request.RequestedShipment.InternationalDetail.Commodities[0].CountryOfManufacture = "US";
                //request.RequestedShipment.InternationalDetail.Commodities[0].Weight = new Weight();
                //request.RequestedShipment.InternationalDetail.Commodities[0].Weight.Value = 1.0M;
                //request.RequestedShipment.InternationalDetail.Commodities[0].Weight.Units = WeightUnits.LB;
                //request.RequestedShipment.InternationalDetail.Commodities[0].Quantity = "1";
                //request.RequestedShipment.InternationalDetail.Commodities[0].QuantityUnits = "EA";
                //request.RequestedShipment.InternationalDetail.Commodities[0].UnitPrice = new Money();
                //request.RequestedShipment.InternationalDetail.Commodities[0].UnitPrice.Amount = 1.000000M;
                //request.RequestedShipment.InternationalDetail.Commodities[0].UnitPrice.Currency = "USD";
                //request.RequestedShipment.InternationalDetail.Commodities[0].CustomsValue = new Money();
                //request.RequestedShipment.InternationalDetail.Commodities[0].CustomsValue.Amount = 100.000000M;
                //request.RequestedShipment.InternationalDetail.Commodities[0].CustomsValue.Currency = "USD";
                #endregion
                //clsOrderedShoppingCartItems ocart = new clsOrderedShoppingCartItems();
                //DataSet dsCart = ocart.GetOrderedShoppingCartItemsByOrderId(Convert.ToInt32(OrderNumber));

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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //request.RequestedShipment.InternationalDetail.Commodities[i+1] = new Commodity();
                        request.RequestedShipment.InternationalDetail.Commodities[i].NumberOfPieces = "1";
                        request.RequestedShipment.InternationalDetail.Commodities[i].Description = dt.Rows[i]["ProductName"].ToString();
                        request.RequestedShipment.InternationalDetail.Commodities[i].CountryOfManufacture = AppLogic.AppConfigs("Shipping.OriginCountry");
                        request.RequestedShipment.InternationalDetail.Commodities[i].Weight = new Weight();
                        request.RequestedShipment.InternationalDetail.Commodities[i].Weight.Value = Convert.ToDecimal(dt.Rows[i]["ProWeight"].ToString());
                        request.RequestedShipment.InternationalDetail.Commodities[i].Weight.Units = WeightUnits.LB;

                        if (dt.Rows[i]["Quantity"].ToString().Contains("~"))
                            request.RequestedShipment.InternationalDetail.Commodities[i].Quantity = "1";
                        else
                            request.RequestedShipment.InternationalDetail.Commodities[i].Quantity = dt.Rows[i]["Quantity"].ToString();

                        request.RequestedShipment.InternationalDetail.Commodities[i].QuantityUnits = "EA";
                        request.RequestedShipment.InternationalDetail.Commodities[i].UnitPrice = new Money();
                        request.RequestedShipment.InternationalDetail.Commodities[i].UnitPrice.Amount = Convert.ToDecimal(dt.Rows[i]["ProductPrice"].ToString());
                        request.RequestedShipment.InternationalDetail.Commodities[i].UnitPrice.Currency = AppLogic.AppConfigs("Localization.StoreCurrency");
                        request.RequestedShipment.InternationalDetail.Commodities[i].CustomsValue = new Money();
                        request.RequestedShipment.InternationalDetail.Commodities[i].CustomsValue.Amount = 0.00M;
                        request.RequestedShipment.InternationalDetail.Commodities[i].CustomsValue.Currency = AppLogic.AppConfigs("Localization.StoreCurrency");

                    }
                }
            }
            catch { }

        }
        public void ResizeFEDEXImage(string path, string fileName)
        {
            Int32 Resizeimage = 0;
            System.Drawing.Image img = null;
            try
            {
                if (File.Exists(path + "/" + fileName))
                {

                    using (img = System.Drawing.Image.FromFile(path + "/" + fileName))
                    {

                        Resizeimage++;
                        System.Drawing.Image thumbNail = new Bitmap(385, 578, PixelFormat.Format24bppRgb);
                        Bitmap newBMP = new Bitmap(1200, 1800);
                        newBMP.SetResolution(300, 300);
                        Graphics g = Graphics.FromImage(newBMP);
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(0, 0, 1200, 1800);
                        g.DrawImage(img, rect);
                        g.Dispose();
                        img.Dispose();
                        //this.Response.Clear();
                        //this.Response.ContentType = "image/jpeg";
                        //       thumbNail.Save(this.Response.OutputStream, ImageFormat.Jpeg);
                        //                    thumbNail.Save(path + "/" + fileName);
                        // newBMP.Save(path + "/" + fileName);
                        newBMP.Save(path + "/temp/" + fileName);
                        File.Copy(path + "/temp/" + fileName, path + "/" + fileName, true);
                        try
                        {
                            File.Delete(path + "/temp/" + fileName);
                        }
                        catch
                        {

                        }
                    }
                }
            }
            catch
            {
                img.Dispose();
                if (Resizeimage <= 2)
                {
                    ResizeFEDEXImage(path, fileName);
                }
            }
        }

        public void createpdffromimages(string path, string fromfileName)
        {
            PdfSharp.Pdf.PdfDocument doc = new PdfSharp.Pdf.PdfDocument();
            PdfSharp.Drawing.XImage img1 = null;
            Int32 pdfcounter = 0;
            try
            {


                pdfcounter++;
                PdfSharp.Pdf.PdfPage PFNEW = new PdfSharp.Pdf.PdfPage();
                PFNEW.Size = PdfSharp.PageSize.RA5;
                XUnit xy = new XUnit(435F);
                XUnit xy1 = new XUnit(290F);
                PFNEW.Height = xy;
                PFNEW.Width = xy1;
                doc.Pages.Add(PFNEW);
                XGraphics xgr = XGraphics.FromPdfPage(PFNEW);
                //XGraphics xgr = XGraphics.FromPdfPage(doc.Pages[0]);
                FileInfo fl = new FileInfo(path + "/" + fromfileName);
                string strname = fl.FullName.Replace(fl.Name.ToString(), "");
                img1 = XImage.FromFile(path + "/" + fromfileName);
                xgr.DrawImage(img1, 0, 0);
                doc.Save(fl.FullName.Replace(fl.Extension.ToString(), ".pdf"));
                img1.Dispose();
                doc.Close();
                //File.Move(fileName, System.Web.HttpContext.Current.Server.MapPath(AppLogic.AppConfigs("UsPS.LabelDeletePath") + fl.Name.ToString()));

            }
            catch
            {
                img1.Dispose();
                doc.Close();
                doc.Dispose();
                //if (pdfcounter <= 4)
                //{
                //    FileInfo fl = new FileInfo(path + "/" + fromfileName);

                //    fl.FullName.Replace(fl.Extension.ToString(), ".pdf");
                //    if (!fl.Exists)
                //    {
                //        createpdffromimages(path, fromfileName);
                //    }
                //}

            }
        }
        public string ShowShipmentReply(ProcessShipmentReply reply, string OrderNumber, string CartItemID, DataTable dtData, int WareHouseID)
        {
            string totalImg = string.Empty;
            try
            {
                string ImgName = string.Empty;
                int cnt = 1;
                int cntTa = 0;
                foreach (CompletedPackageDetail packageDetail in reply.CompletedShipmentDetail.CompletedPackageDetails)
                {
                    ShowTrackingDetails(packageDetail.TrackingIds);
                    ShowPackageRateDetails(packageDetail.PackageRating.PackageRateDetails);
                    if (null != packageDetail.Label.Parts[0].Image)
                    {
                        string fileName = packageDetail.TrackingIds[0].TrackingNumber;
                        fileName = "FedEx-Package" + dtData.Rows[0]["packageid"].ToString() + "_" + fileName + "_" + OrderNumber + "_" + ((string.IsNullOrEmpty(CartItemID)) ? "0" : CartItemID) + "@" +
                         DateTime.Now.Year.ToString() +
                         DateTime.Now.Month.ToString() +
                         DateTime.Now.Day.ToString() +
                         DateTime.Now.Hour.ToString() +
                         DateTime.Now.Minute.ToString() +
                         DateTime.Now.Second.ToString() + "-" + dtData.Rows[0]["packageid"].ToString() + ".png";
                        totalImg += fileName + "#";
                        SaveLabel(imagepath + fileName, packageDetail.Label.Parts[0].Image);
                        cnt++;
                        cntTa++;

                        try
                        {

                            ResizeFEDEXImage(imagepath, fileName);
                            createpdffromimages(imagepath, fileName);
                            ShippingComponent.UpdateorderedShoppingcartitems(packageDetail.TrackingIds[0].TrackingNumber, dtData.Rows[0]["ProductID"].ToString(), "FEDEX",Convert.ToInt32(dtData.Rows[0]["ShippingCartID"].ToString()), 1, WareHouseID, Convert.ToInt32(dtData.Rows[0]["packageid"].ToString()));
                        }
                        catch { }


                    }
                    //ShowBarcodeDetails(packageDetail.Barcodes);
                }
                //ShowPackageRouteDetails(reply.CompletedShipmentDetail.RoutingDetail);
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
        //public void SaveLabelInGifFile(string fileName)
        //{
        //    System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes);
        //    System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
        //    img.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);

        //    img.Save(fileName);
        //    System.IO.FileInfo objFile = new System.IO.FileInfo(fileName);
        //    img.Save(System.Environment.CurrentDirectory + "/Temp/" + objFile.Name.ToString());

        //    ms.Close();
        //    ms.Dispose();
        //}


        private static void DisplayLabel(string LabelFileName)
        {
            System.Diagnostics.ProcessStartInfo info = new System.Diagnostics.ProcessStartInfo(LabelFileName);
            info.UseShellExecute = true;
            info.Verb = "open";
            System.Diagnostics.Process.Start(info);
        }
    }
}