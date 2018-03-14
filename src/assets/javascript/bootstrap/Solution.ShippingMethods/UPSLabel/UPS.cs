/*
 * UPS Code Version 1.0
 * Author Nhat Ho Vdap.com
 * This is open source and free. If you improve it or any recomandation please sent it to support@vdap.com. 
 */

using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Collections.Specialized;
using System.Net;
using System.Net.Security;

namespace ShippingUPSL
{
    public abstract class UPSBase
    {
        protected static readonly string UPS_SERVICE_URL = "https://{0}.ups.com/ups.app/xml/{1}";
        public static readonly string XML_CONNECT = @"<?xml version='1.0'?>
<AccessRequest xml:lang='en-US'>
 <AccessLicenseNumber>{0}</AccessLicenseNumber>
   <UserId>{1}</UserId>
   <Password>{2}</Password>
</AccessRequest>";
        public static readonly StringDictionary SHIP_METHODS = new StringDictionary();
        public static readonly StringDictionary PICKUP_METHODS = new StringDictionary();
        public static readonly StringDictionary PAKAGE_TYPE = new StringDictionary();
        public static readonly StringDictionary REF_TYPE = new StringDictionary();
        protected bool testMode = false;

        static UPSBase()
        {
            SHIP_METHODS.Add("00", "Free shipping");
            SHIP_METHODS.Add("14", "Next Day Air Early AM");
            SHIP_METHODS.Add("13", "Next Day Air Saver");
            SHIP_METHODS.Add("01", "Next Day Air");
            SHIP_METHODS.Add("03", "Ground");
            SHIP_METHODS.Add("07", "Wordwide Express");
            SHIP_METHODS.Add("08", "Wordwide Expedited");
            SHIP_METHODS.Add("11", "Standard");
            SHIP_METHODS.Add("12", "3 Day Selected");
            //SHIP_METHODS.Add("13", "Saver Canada");
            //SHIP_METHODS.Add("14", "Express Early AM");
            SHIP_METHODS.Add("54", "Wordwide Express Plus");
            SHIP_METHODS.Add("59", "Second Day Air AM");
            SHIP_METHODS.Add("65", "Saver");
            SHIP_METHODS.Add("82", "Today Standard");
            SHIP_METHODS.Add("83", "Today Dedicate Courrier");
            SHIP_METHODS.Add("84", "Today Intercity");
            SHIP_METHODS.Add("85", "Today Express");
            SHIP_METHODS.Add("86", "Today Express Saver");
            PICKUP_METHODS.Add("01", "Daily Pickup");
            PICKUP_METHODS.Add("03", "Customer Counter");
            PICKUP_METHODS.Add("06", "One Time Pickup");
            PICKUP_METHODS.Add("07", "On Call Air");
            PICKUP_METHODS.Add("11", "Sugested Retail Rates");
            PICKUP_METHODS.Add("19", "Letter Center");
            PICKUP_METHODS.Add("20", "Air Service Center");
            PAKAGE_TYPE.Add("01", "UPS Letter/UPS Express Envelop");
            PAKAGE_TYPE.Add("02", "Package");
            PAKAGE_TYPE.Add("03", "UPS Tube");
            PAKAGE_TYPE.Add("04", "UPS Pak");
            PAKAGE_TYPE.Add("21", "UPS Express Box");
            PAKAGE_TYPE.Add("24", "UPS 25kg Box");
            PAKAGE_TYPE.Add("25", "UPS 10kg Box");
            PAKAGE_TYPE.Add("30", "Pallet");
            REF_TYPE.Add("AJ", "Cusomter Account");
            REF_TYPE.Add("AT", "Approciation number");
            REF_TYPE.Add("BM", "Bill of ladding number");
            REF_TYPE.Add("9V", "COD number");
            REF_TYPE.Add("ON", "Dealer order number");
            REF_TYPE.Add("DP", "Department number");
            REF_TYPE.Add("3Q", "FDA product code");
            REF_TYPE.Add("IK", "Invoice number");
            REF_TYPE.Add("MK", "Manifest key number");
            REF_TYPE.Add("MJ", "Model number");
            REF_TYPE.Add("PM", "Part number");
            REF_TYPE.Add("PC", "Production code");
            REF_TYPE.Add("PO", "Purchase order number");
            REF_TYPE.Add("RQ", "Purchase request number");
            REF_TYPE.Add("RZ", "Return authorization number");
            REF_TYPE.Add("SA", "Sale person number");
            REF_TYPE.Add("SE", "Serial number");
            REF_TYPE.Add("ST", "Store number");
            REF_TYPE.Add("TN", "Transaction ref. number");



        }

        public UPSBase()
        {

        }



    };

    public struct RateRecord : System.IComparable
    {
        public readonly string Code;
        public readonly string Name;
        public readonly double Cost;
        public RateRecord(string code, string name, double cost)
        {
            Code = code;
            Name = name;
            Cost = cost;
        }
        public int CompareTo(object obj)
        {
            return Cost.CompareTo(((RateRecord)obj).Cost);
        }

    }

    public class RateResults
    {
        public readonly bool Success;
        public readonly string StatusMessage;
        RateRecord[] mathMethods;
        public RateResults(RateRecord[] math, bool success, string message)
        {
            mathMethods = math;
            Success = success;
            StatusMessage = message;
        }

        public RateResults(System.Xml.XmlDocument xml)
        {
            Success = false;
            XmlNode m = xml.SelectSingleNode("Response/ResponseStatusCode");
            if (m != null)
                Success = (int.Parse(m.InnerText) == 1);
            m = xml.SelectSingleNode("Response/ResponseStatusDescription");
            if (m != null)
                StatusMessage = m.InnerText;

            XmlNodeList nodes = xml.GetElementsByTagName("RatedShipment");
            mathMethods = new RateRecord[nodes.Count];
            int i = 0;
            foreach (XmlNode n in nodes)
            {
                XmlNode sv = n.SelectSingleNode("Service").SelectSingleNode("Code");
                XmlNode mn = n.SelectSingleNode("TotalCharges").SelectSingleNode("MonetaryValue");
                mathMethods[i++] = new RateRecord(sv.InnerText, UPSBase.SHIP_METHODS[sv.InnerText], double.Parse(mn.InnerText));
            }



        }
        public RateRecord[] Mathes
        {
            get { return mathMethods; }
        }
        public void Report()
        {
            Console.WriteLine(Success);

            foreach (RateRecord rc in mathMethods)
                Console.WriteLine(rc.Code + ": " + rc.Name + ": " + string.Format("{0:c}", rc.Cost));
        }



    };

    public class ShippingAcceptPackage
    {
        public readonly string TrackingNumber;
        private readonly byte[] imageBytes;
        public byte[] LabelImageInBytes
        {
            get { return imageBytes; }
        }

        public void  SaveLabelInGifFile(string fileName)
        {
            System.IO.MemoryStream ms = new System.IO.MemoryStream(imageBytes);
            System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
            img.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipNone);
            img.Save(fileName);
            ms.Close();
            ms.Dispose();
        }

        public ShippingAcceptPackage(string trackingNumber, string Encoding64)
        {
            TrackingNumber = trackingNumber;
            imageBytes = Convert.FromBase64String(Encoding64);
        }
    };

    public class ShippingAcceptResult
    {
        public readonly bool Success;
        public readonly string StatusMessage;
        public readonly string ShipmentIdentificationNumber;
        public readonly ShippingAcceptPackage[] Packages;
        public ShippingAcceptResult(System.Xml.XmlDocument xml)
        {
            Success = false;
            XmlNode m = xml.SelectSingleNode("ShipmentAcceptResponse/Response/ResponseStatusCode");

            if (m != null)
            {
                Success = (int.Parse(m.InnerText) == 1);
                if (!Success)
                {
                    m = xml.SelectSingleNode("ShipmentAcceptResponse/Response/ResponseStatusDescription");
                    if (m != null)
                        StatusMessage = m.InnerText;
                }
            }

            m = xml.SelectSingleNode("ShipmentAcceptResponse/ShipmentResults/ShipmentIdentificationNumber");
            this.ShipmentIdentificationNumber = (m != null) ? m.InnerText : "";

            XmlNodeList nodes = xml.SelectNodes("ShipmentAcceptResponse/ShipmentResults/PackageResults");
            Console.WriteLine(nodes.Count);
            Packages = new ShippingAcceptPackage[nodes.Count];
            for (int i = 0; i < nodes.Count; ++i)
            {
                m = nodes[i].SelectSingleNode("TrackingNumber");
                XmlNode m2 = nodes[i].SelectSingleNode("LabelImage/GraphicImage");
                Packages[i] = new ShippingAcceptPackage(m.InnerText, m2.InnerText);
            }

        }
    };

    public class ShippingConfirmResult
    {
        public readonly bool Success;
        public readonly string StatusMessage;
        public readonly string ShipmentDigest;
        public readonly string ShipmentIdentificationNumber;
        public readonly double Total;
        public readonly string Currency;

        public ShippingConfirmResult(System.Xml.XmlDocument xml)
        {

            Success = false;
            XmlNode m = xml.SelectSingleNode("ShipmentConfirmResponse/Response/ResponseStatusCode");

            if (m != null)
            {
                Success = (int.Parse(m.InnerText) == 1);
                if (!Success)
                {
                    m = xml.SelectSingleNode("ShipmentConfirmResponse/Response/ResponseStatusDescription");
                    if (m != null)
                        StatusMessage = m.InnerText;
                }
            }
            m = xml.SelectSingleNode("ShipmentConfirmResponse/ShipmentCharges/TotalCharges/CurrencyCode");
            if (m != null)
            {

                this.Currency = (m != null) ? m.InnerText : "USD";

            }
            m = xml.SelectSingleNode("ShipmentConfirmResponse/ShipmentCharges/TotalCharges/MonetaryValue");
            if (m != null)
            {
                this.Total = (m != null) ? double.Parse(m.InnerText) : 0.0;
            }

            m = xml.SelectSingleNode("ShipmentConfirmResponse/ShipmentIdentificationNumber");
            this.ShipmentIdentificationNumber = (m != null) ? m.InnerText : "";
            m = xml.SelectSingleNode("ShipmentConfirmResponse/ShipmentDigest");
            this.ShipmentDigest = (m != null) ? m.InnerText : "";

        }

        public void Print()
        {
            Console.WriteLine(Success);
            Console.WriteLine(StatusMessage);
            Console.WriteLine(Total);
            Console.WriteLine(Currency);
            Console.WriteLine(ShipmentDigest);
            Console.WriteLine(ShipmentIdentificationNumber);

        }
    }
    ;
    public class UPSShipping : UPSBase
    {

        //static readonly string UPS_URL = "https://{0}.ups.com/ups.app/xml/{1}";
        string sAccessCode;
        string sUserId;
        string sPassword;
        XmlDocument xmlRequest;
        public XmlDocument xmlRespone;
        XmlNode lastNode;

        enum ShipRequestTypes { ShipConfirm, ShipAccept, Void };
        ShipRequestTypes requestType = ShipRequestTypes.ShipConfirm;
        public bool TestMode
        {
            set { testMode = value; }
            get { return testMode; }
        }


        public UPSShipping(string accessCode, string userId, string password)
        {

            this.sAccessCode = accessCode;
            this.sUserId = userId;
            this.sPassword = password;
            xmlRequest = new System.Xml.XmlDocument();
            xmlRequest.LoadXml(@"<?xml version=""1.0""?>
<ShipmentConfirmRequest xml:lang=""en-US"">
   <Request>
      <TransactionReference>
         <CustomerContext>ShipConfirmUS</CustomerContext>
         <XpciVersion>1.0001</XpciVersion>
      </TransactionReference>
      <RequestAction>ShipConfirm</RequestAction>
      <RequestOption>nonvalidate</RequestOption>
   </Request>
   <LabelSpecification>
      <LabelPrintMethod>
         <Code>GIF</Code>
      </LabelPrintMethod>
      <LabelImageFormat>
         <Code>GIF</Code>
      </LabelImageFormat>
      <LabelStockSize>
         <Height>4</Height>
         <Width>6</Width>
      </LabelStockSize>
   </LabelSpecification>
   <Shipment></Shipment>
</ShipmentConfirmRequest>
");
            lastNode = xmlRequest.GetElementsByTagName("Shipment").Item(0);

        }

        public ShippingAcceptResult ShippingAccept(string acceptDigestCode)
        {
            requestType = ShipRequestTypes.ShipAccept;

            xmlRequest = new System.Xml.XmlDocument();
            xmlRequest.LoadXml(@"<?xml version=""1.0""?>
             <ShipmentAcceptRequest>
               <Request>
                  <TransactionReference>
                     <CustomerContext>TR01</CustomerContext>
                     <XpciVersion>1.0001</XpciVersion>
                  </TransactionReference>
                  <RequestAction>ShipAccept</RequestAction>
                  <RequestOption>01</RequestOption>
               </Request>
               <ShipmentDigest>" + acceptDigestCode + "</ShipmentDigest></ShipmentAcceptRequest>");

            byte[] bb = new System.Text.ASCIIEncoding().GetBytes(string.Format(XML_CONNECT, sAccessCode.Trim(), sUserId.Trim(), sPassword.Trim()));

            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(bb, 0, bb.Length);
            xmlRequest.Save(ms);
            bb = ms.ToArray();
            System.Net.WebClient wc = new System.Net.WebClient();
            xmlRespone = new XmlDocument();
            //System.Web.HttpContext.Current.Response.Write(System.Text.ASCIIEncoding.ASCII.GetString(wc.UploadData(UPS_URL, "POST", bb)));
            string serverName = (testMode) ? "wwwcie" : "www";

            serverName = string.Format(UPS_SERVICE_URL, serverName, ShipRequestTypes.ShipAccept);
            xmlRespone.LoadXml(System.Text.ASCIIEncoding.ASCII.GetString(wc.UploadData(serverName, "POST", bb)));
            // xmlRespone.Save("shippingresponseAccept.xml");
            return new ShippingAcceptResult(xmlRespone);


        }

        public void ShippingVoid(string voidIdentificationNumber)
        {
            requestType = ShipRequestTypes.Void;

            xmlRequest = new System.Xml.XmlDocument();
            xmlRequest.LoadXml(@"<?xml version=""1.0""?>
<VoidShipmentRequest>
   <Request>
      <TransactionReference>
         <CustomerContext>Void</CustomerContext>
         <XpciVersion>1.0001</XpciVersion>
      </TransactionReference>
      <RequestAction>Void</RequestAction>
      <RequestOption>1</RequestOption>
   </Request>
   <ShipmentIdentificationNumber>" + voidIdentificationNumber + "</ShipmentIdentificationNumber></VoidShipmentRequest>");


        }


        public ShippingConfirmResult ProcessShipping()
        {
            byte[] bb = new System.Text.ASCIIEncoding().GetBytes(string.Format(XML_CONNECT, sAccessCode.Trim(), sUserId.Trim(), sPassword.Trim()));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(bb, 0, bb.Length);
            xmlRequest.Save(ms);
            bb = ms.ToArray();
            System.Net.WebClient wc = new System.Net.WebClient();
            xmlRespone = new XmlDocument();
            //System.Web.HttpContext.Current.Response.Write(System.Text.ASCIIEncoding.ASCII.GetString(wc.UploadData(UPS_URL, "POST", bb)));
            string serverName = (testMode) ? "wwwcie" : "www";

            serverName = string.Format(UPS_SERVICE_URL, serverName, ShipRequestTypes.ShipConfirm);

            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;

            xmlRespone.LoadXml(System.Text.ASCIIEncoding.ASCII.GetString(wc.UploadData(serverName, "POST", bb)));
            //xmlRespone.Save("shippingresponse.xml");
            return new ShippingConfirmResult(xmlRespone);
        }

        public void ShipperInfo(string shipperName, string attentionName, string phoneNumber, string accountNumber,
            string addressLine1, string addressLine2, string city, string stateProvinceCode, string postalCode, string countryCode)
        {
            XmlNode shipper = xmlRequest.CreateNode(XmlNodeType.Element, "Shipper", null);
            shipper = lastNode.AppendChild(shipper);

            XmlNode temp = xmlRequest.CreateNode(XmlNodeType.Element, "Name", null);
            temp.InnerText = shipperName;
            shipper.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "AttentionName", null);
            temp.InnerText = attentionName;
            shipper.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "PhoneNumber", null);
            temp.InnerText = phoneNumber;
            shipper.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "ShipperNumber", null);
            temp.InnerText = accountNumber;
            shipper.AppendChild(temp);

            XmlNode address = xmlRequest.CreateNode(XmlNodeType.Element, "Address", null);
            address = shipper.AppendChild(address);


            temp = xmlRequest.CreateNode(XmlNodeType.Element, "AddressLine1", null);
            if (addressLine2 != null)
            {
                temp.InnerText = addressLine1 + ", " + addressLine2;
            }
            else
            {
                temp.InnerText = addressLine1;
            }

            address.AppendChild(temp);
            if (addressLine2.Length > 0)
            {
                temp = xmlRequest.CreateNode(XmlNodeType.Element, "AddressLine2", null);
                temp.InnerText = addressLine2;
                address.AppendChild(temp);
            }

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "City", null);
            temp.InnerText = city;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "StateProvinceCode", null);
            temp.InnerText = stateProvinceCode;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "PostalCode", null);
            temp.InnerText = postalCode;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "CountryCode", null);
            temp.InnerText = countryCode;
            address.AppendChild(temp);

        }

        public void ShipTo(string companyName, string attentionName, string phoneNumber, string addressLine1, string addressLine2, string city, string stateProvinceCode, string postalCode, string countryCode)
        {
            XmlNode shipto = xmlRequest.CreateNode(XmlNodeType.Element, "ShipTo", null);
            shipto = lastNode.AppendChild(shipto);

            XmlNode temp = xmlRequest.CreateNode(XmlNodeType.Element, "CompanyName", null);
            temp.InnerText = companyName;
            shipto.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "AttentionName", null);
            temp.InnerText = attentionName;
            shipto.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "PhoneNumber", null);
            temp.InnerText = phoneNumber;
            shipto.AppendChild(temp);



            XmlNode address = xmlRequest.CreateNode(XmlNodeType.Element, "Address", null);
            address = shipto.AppendChild(address);


            temp = xmlRequest.CreateNode(XmlNodeType.Element, "AddressLine1", null);
            temp.InnerText = addressLine1;
            address.AppendChild(temp);
            if (addressLine2.Length > 0)
            {
                temp = xmlRequest.CreateNode(XmlNodeType.Element, "AddressLine2", null);
                temp.InnerText = addressLine2;
                address.AppendChild(temp);
            }

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "City", null);
            temp.InnerText = city;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "StateProvinceCode", null);
            temp.InnerText = stateProvinceCode;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "PostalCode", null);
            temp.InnerText = postalCode;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "CountryCode", null);
            temp.InnerText = countryCode;
            address.AppendChild(temp);




        }

        public void ShipFrom(string companyName, string attentionName, string phoneNumber, string addressLine1, string addressLine2, string city, string stateProvinceCode, string postalCode, string countryCode)
        {
            XmlNode shipFrom = xmlRequest.CreateNode(XmlNodeType.Element, "ShipFrom", null);
            shipFrom = lastNode.AppendChild(shipFrom);

            XmlNode temp = xmlRequest.CreateNode(XmlNodeType.Element, "CompanyName", null);
            temp.InnerText = companyName;
            shipFrom.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "AttentionName", null);
            temp.InnerText = attentionName;
            shipFrom.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "PhoneNumber", null);
            temp.InnerText = phoneNumber;
            shipFrom.AppendChild(temp);



            XmlNode address = xmlRequest.CreateNode(XmlNodeType.Element, "Address", null);
            address = shipFrom.AppendChild(address);


            temp = xmlRequest.CreateNode(XmlNodeType.Element, "AddressLine1", null);
            temp.InnerText = addressLine1;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "AddressLine2", null);
            temp.InnerText = addressLine2;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "City", null);
            temp.InnerText = city;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "StateProvinceCode", null);
            temp.InnerText = stateProvinceCode;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "PostalCode", null);
            temp.InnerText = postalCode;
            address.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "CountryCode", null);
            temp.InnerText = countryCode;
            address.AppendChild(temp);




        }

        public void PaymentInformation(string accountNumber, string shipMethod)
        {

            XmlNode mainNode = xmlRequest.CreateNode(XmlNodeType.Element, "PaymentInformation", null);
            mainNode = lastNode.AppendChild(mainNode);
            mainNode = mainNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Prepaid", null));
            mainNode = mainNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "BillShipper", null));
            mainNode = mainNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "AccountNumber", null));
            mainNode.InnerText = accountNumber;

            mainNode = lastNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Service", null));
            XmlNode temp = xmlRequest.CreateNode(XmlNodeType.Element, "Code", null);
            temp.InnerText = shipMethod;
            mainNode.AppendChild(temp);

            temp = xmlRequest.CreateNode(XmlNodeType.Element, "Description", null);
            temp.InnerText = UPSBase.SHIP_METHODS[shipMethod];
            mainNode.AppendChild(temp);

        }

        public bool AddEmailNotification(string email, string memo)
        {
            XmlNode temp = lastNode.SelectSingleNode("ShipmentServiceOptions");
            if (temp == null)
            {
                temp = lastNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "ShipmentServiceOptions", null));
                temp = lastNode.AppendChild(temp);
            }


            int count = temp.SelectNodes("ShipmentNotification").Count;
            if (count >= 5)
                return false;

            temp = temp.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "ShipmentNotification", null));
            temp = temp.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "EMailMessage", null));

            XmlNode val = xmlRequest.CreateNode(XmlNodeType.Element, "EMailAddress", null);
            val.InnerText = email;
            temp.AppendChild(val);

            val = xmlRequest.CreateNode(XmlNodeType.Element, "Memo", null);
            val.InnerText = memo;
            temp.AppendChild(val);
            return true;


        }

        public void AddPackage(string description, string packageType, double weight, bool isLBS
            , double value, string currencyCode, string refCode, string refNumber, bool deliveryConfirmation, bool isInsured)
        {

            XmlNode package = lastNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Package", null));
            XmlNode temp1 = null;
            XmlNode temp2 = null;
            if (description.Length > 0)
            {
                temp1 = xmlRequest.CreateNode(XmlNodeType.Element, "Description", null);
                temp1.InnerText = description;
                package.AppendChild(temp1);
            }

            temp1 = xmlRequest.CreateNode(XmlNodeType.Element, "PackagingType", null);
            temp1 = package.AppendChild(temp1);

            temp2 = xmlRequest.CreateNode(XmlNodeType.Element, "Code", null);
            temp2.InnerText = packageType;
            temp1.AppendChild(temp2);

            temp1 = xmlRequest.CreateNode(XmlNodeType.Element, "ReferenceNumber", null);
            temp1 = package.AppendChild(temp1);
            temp2 = xmlRequest.CreateNode(XmlNodeType.Element, "Code", null);
            temp2.InnerText = refCode;
            temp1.AppendChild(temp2);

            temp2 = xmlRequest.CreateNode(XmlNodeType.Element, "Value", null);
            temp2.InnerText = refNumber;
            temp1.AppendChild(temp2);


            temp1 = xmlRequest.CreateNode(XmlNodeType.Element, "PackageWeight", null);
            temp1 = package.AppendChild(temp1);
            temp2 = xmlRequest.CreateNode(XmlNodeType.Element, "UnitOfMeasurement", null);
            temp2 = temp1.AppendChild(temp2);
            temp2 = temp2.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "UnitOfMeasurement", null));
            temp2.InnerText = (isLBS) ? "LBS" : "KGS";
            temp2 = xmlRequest.CreateNode(XmlNodeType.Element, "Weight", null);
            temp2.InnerText = Convert.ToInt32(Math.Round(weight)).ToString();
            temp1.AppendChild(temp2);

            temp1 = null;
            if (isInsured)
            {
                temp1 = xmlRequest.CreateNode(XmlNodeType.Element, "PackageServiceOptions", null);
                package.AppendChild(temp1);

                temp2 = temp1.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "InsuredValue", null));

                XmlNode temp3 = temp2.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "CurrencyCode", null));
                temp3.InnerText = currencyCode;
                temp2.AppendChild(temp3);

                temp3 = temp2.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "MonetaryValue", null));
                temp3.InnerText = Convert.ToInt32(Math.Round(value)).ToString();
                temp2.AppendChild(temp3);
            }
            if (deliveryConfirmation)
            {
                if (temp1 == null)
                {
                    temp1 = xmlRequest.CreateNode(XmlNodeType.Element, "PackageServiceOptions", null);
                    package.AppendChild(temp1);
                }
                temp2 = temp1.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "DeliveryConfirmation", null));
                temp2 = temp2.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "DCISType", null));
                temp2.InnerText = "2";
            }
        }



        public void SaveXMLRequest()
        {
            xmlRequest.Save("shippingRequest.xml");
        }

    };

    public class Rate : UPSBase
    {

        //static readonly string UPS_URL = "https://www.ups.com/ups.app/xml/Rate";
        string sAccessCode;
        string sUserId;
        string sPassword;
        XmlDocument xmlRequest;
        XmlDocument xmlRespone;
        XmlNode lastNode;
        private double sWeight = 0.0;
        public bool TestMode
        {
            set { testMode = value; }
            get { return testMode; }
        }
        public Rate(string accessCode, string userId, string password)
        {


            this.sAccessCode = accessCode;
            this.sUserId = userId;
            this.sPassword = password;
            xmlRequest = new System.Xml.XmlDocument();
            xmlRequest.LoadXml(@"<?xml version='1.0'?>
<RatingServiceSelectionRequest xml:lang='en-US'>
  <Request>
    <TransactionReference>
      <CustomerContext>Rating and Service</CustomerContext>
      <XpciVersion>1.0001</XpciVersion>
    </TransactionReference>
    <RequestAction>Rate</RequestAction>
    <RequestOption>shop</RequestOption>
  </Request>
</RatingServiceSelectionRequest>");
            lastNode = xmlRequest.GetElementsByTagName("Request").Item(0);

        }

        public void SaveXml()
        {
            byte[] bb = new System.Text.ASCIIEncoding().GetBytes(string.Format(XML_CONNECT, sAccessCode, sUserId, sPassword));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(bb, 0, bb.Length);
            xmlRequest.Save(ms);
            bb = ms.ToArray();
            Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(bb));


        }


        public RateResults ProcessRating()
        {
            if (sWeight <= 0.0)// free shipping
            {
                RateRecord[] rs = new RateRecord[1];
                rs[0] = new RateRecord("00", UPSBase.SHIP_METHODS["00"], 0.0);
                return new RateResults(rs, true, "");
            }

            //Uri uri = new Uri(UPS_URL);
            byte[] bb = new System.Text.ASCIIEncoding().GetBytes(string.Format(XML_CONNECT, sAccessCode.Trim(), sUserId.Trim(), sPassword.Trim()));
            System.IO.MemoryStream ms = new System.IO.MemoryStream();
            ms.Write(bb, 0, bb.Length);
            xmlRequest.Save(ms);
            bb = ms.ToArray();
            System.Net.WebClient wc = new System.Net.WebClient();
            xmlRespone = new XmlDocument();

            string serverName = (testMode) ? "wwwcie" : "www";
            serverName = string.Format(UPS_SERVICE_URL, serverName, "Rate");
            xmlRespone.LoadXml(System.Text.ASCIIEncoding.ASCII.GetString(wc.UploadData(serverName, "POST", bb)));
            return new RateResults(xmlRespone);

        }

        public void PackageInfo(double weight)
        {
            //  <Package>
            //  <PackagingType>
            //    <Code>02</Code>
            //    <Description>Package</Description>
            //  </PackagingType>
            //  <Description>Rate Shopping</Description>
            //  <PackageWeight>
            //    <Weight>33</Weight>
            //  </PackageWeight>
            //</Package>

            sWeight = weight;
            XmlNode shipmentPackage = lastNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Package", null));
            XmlNode shipmentPackageType = shipmentPackage.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "PackagingType", null));
            XmlNode node = xmlRequest.CreateNode(XmlNodeType.Element, "Code", null);
            node.InnerText = "02";
            shipmentPackageType.AppendChild(node);

            XmlNode wackageWeight = shipmentPackage.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "PackageWeight", null));
            node = shipmentPackage.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Weight", null));
            node.InnerText = string.Format("{0:0.0}", weight);
            wackageWeight.AppendChild(node);


        }

        public void PackageInfo(string packageType, double weight, string kgsORlbs, int length, int width, int height, string inORcm)
        {

            sWeight = weight;
            XmlNode shipmentWeight = lastNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "ShipmentWeight", null));
            XmlNode unitOfMeasurement = shipmentWeight.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "UnitOfMeasurement", null));
            XmlNode node = xmlRequest.CreateNode(XmlNodeType.Element, "Code", null);
            node.InnerText = kgsORlbs.ToUpper();
            unitOfMeasurement.AppendChild(node);
            node = xmlRequest.CreateNode(XmlNodeType.Element, "Weight", null);
            node.InnerText = string.Format("{0:0.0}", weight);
            unitOfMeasurement.AppendChild(node);

            XmlNode shipmentPackage = lastNode.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Package", null));
            XmlNode shipmentPackageType = shipmentPackage.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "PackagingType", null));
            node = xmlRequest.CreateNode(XmlNodeType.Element, "Code", null);
            node.InnerText = packageType;

            XmlNode PackageDimensions = shipmentPackage.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Dimensions", null));
            XmlNode unit = shipmentPackage.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "UnitOfMeasurement", null));
            node = xmlRequest.CreateNode(XmlNodeType.Element, "Code", null);
            node.InnerText = inORcm.ToUpper();

            node = xmlRequest.CreateNode(XmlNodeType.Element, "Length", null);
            node.InnerText = string.Format("{0:0.0}", length);
            PackageDimensions.AppendChild(node);

            node = xmlRequest.CreateNode(XmlNodeType.Element, "Width", null);
            node.InnerText = string.Format("{0:0.0}", width);
            PackageDimensions.AppendChild(node);

            node = xmlRequest.CreateNode(XmlNodeType.Element, "Height", null);
            node.InnerText = string.Format("{0:0.0}", height);
            PackageDimensions.AppendChild(node);

        }

        public void FromAddress(string pickupCode, string city, string state, string zipcode, string country)
        {

            XmlNode node = xmlRequest.CreateNode(XmlNodeType.Element, "PickupType", null);
            lastNode = lastNode.ParentNode.AppendChild(node);
            node = xmlRequest.CreateNode(XmlNodeType.Element, "Code", null);
            node.InnerText = pickupCode;
            lastNode.AppendChild(node);

            node = xmlRequest.CreateNode(XmlNodeType.Element, "Shipment", null);
            lastNode = lastNode.ParentNode.AppendChild(node);
            XmlNode shipper = xmlRequest.CreateNode(XmlNodeType.Element, "Shipper", null);
            shipper = lastNode.AppendChild(shipper);
            XmlNode address = shipper.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Address", null));
            node = xmlRequest.CreateNode(XmlNodeType.Element, "City", null);
            node.InnerText = city;
            address.AppendChild(node);
            if (country != "US")
            {
                node = xmlRequest.CreateNode(XmlNodeType.Element, "StateProvinceCode", null);
                node.InnerText = state;
                address.AppendChild(node);
            }

            node = xmlRequest.CreateNode(XmlNodeType.Element, "PostalCode", null);
            node.InnerText = zipcode;
            address.AppendChild(node);

            node = xmlRequest.CreateNode(XmlNodeType.Element, "CountryCode", null);
            node.InnerText = country;
            address.AppendChild(node);




        }
        public void ToAddress(string city, string state, string zipcode, string country, bool isResidentAddress)
        {
            XmlNode node = xmlRequest.CreateNode(XmlNodeType.Element, "ShipTo", null);
            XmlNode shipto = lastNode.AppendChild(node);
            XmlNode address = shipto.AppendChild(xmlRequest.CreateNode(XmlNodeType.Element, "Address", null));

            node = xmlRequest.CreateNode(XmlNodeType.Element, "City", null);
            node.InnerText = city;
            address.AppendChild(node);

            node = xmlRequest.CreateNode(XmlNodeType.Element, "StateProvinceCode", null);
            node.InnerText = state;
            address.AppendChild(node);


            node = xmlRequest.CreateNode(XmlNodeType.Element, "PostalCode", null);
            node.InnerText = zipcode;
            address.AppendChild(node);

            node = xmlRequest.CreateNode(XmlNodeType.Element, "CountryCode", null);
            node.InnerText = country;
            address.AppendChild(node);
            if (isResidentAddress)
            {
                node = xmlRequest.CreateNode(XmlNodeType.Element, "ResidentialAddressIndicator", null);
                node.InnerText = "1";
                address.AppendChild(node);
            }

        }

    };

    public class UPSTracking : UPSBase
    {

        public static readonly string XML_TRACK = @"<?xml version=""1.0"" ?> 
 <TrackRequest xml:lang=""en-US"">
 <Request>
 <TransactionReference>
  <CustomerContext>{0}</CustomerContext> 
  <XpciVersion>1.0001</XpciVersion> 
  </TransactionReference>
  <RequestAction>Track</RequestAction> 
  <RequestOption>none</RequestOption> 
  </Request>
 <ReferenceNumber>
  <Value>{1}</Value> 
  </ReferenceNumber>
  </TrackRequest>";
        //static readonly string UPS_URL = "https://www.ups.com/ups.app/xml/Track";
        string sAccessCode;
        string sUserId;
        string sPassword;

        XmlDocument xmlRespone;

        public bool TestMode
        {
            set { testMode = value; }
            get { return testMode; }
        }
        public UPSTracking(string accessCode, string userId, string password)
        {
            this.sAccessCode = accessCode;
            this.sUserId = userId;
            this.sPassword = password;



        }

        public void Save()
        {
            xmlRespone.Save("track.xml");
        }
        public bool Tracking(string refNumber, string description, out string trackNumber, out string StatusMessage)
        {
            trackNumber = "";
            StatusMessage = "";
            //Uri uri = new Uri(UPS_URL);
            byte[] bb = new System.Text.ASCIIEncoding().GetBytes(string.Format(XML_CONNECT, sAccessCode, sUserId, sPassword)
                + string.Format(XML_TRACK, description, refNumber));

            System.Net.WebClient wc = new System.Net.WebClient();
            xmlRespone = new XmlDocument();
            string serverName = (testMode) ? "wwwcie" : "www";
            serverName = string.Format(UPS_SERVICE_URL, serverName, "Track");
            xmlRespone.LoadXml(System.Text.ASCIIEncoding.ASCII.GetString(wc.UploadData(serverName, "POST", bb)));


            bool success = false;

            XmlNode m = xmlRespone.SelectSingleNode("TrackResponse/Response/ResponseStatusCode");
            if (m != null)
                success = (int.Parse(m.InnerText) == 1);


            m = xmlRespone.SelectSingleNode("TrackResponse/Response/ResponseStatusDescription");

            if (m != null)
                StatusMessage = m.InnerText;
            if (!success)
                return success;

            m = xmlRespone.SelectSingleNode("TrackResponse/Response/Shipment/Package/trackNumber ");
            if (m != null)
                trackNumber = m.InnerText;
            return true;



        }
    };

    public class USAddressValidate : UPSBase
    {

        public static readonly string XML_ADDRESS = @"<?xml version=""1.0"" ?> 
  <AddressValidationRequest xml:lang=""en-US"">
  <Request>
    <TransactionReference>
     <CustomerContext>Customer Data</CustomerContext> 
     <XpciVersion>1.0001</XpciVersion> 
    </TransactionReference>
    <RequestAction>AV</RequestAction> 
   </Request>
    <Address>
      <City>{0}</City> 
      <StateProvinceCode>{1}</StateProvinceCode> 
      <PostalCode>{2}</PostalCode> 
  </Address>
  </AddressValidationRequest>";
        //static readonly string UPS_URL = "https://www.ups.com/ups.app/xml/AV";
        string sAccessCode;
        string sUserId;
        string sPassword;
        string StatusMessage;
        XmlDocument xmlRespone;

        public bool TestMode
        {
            set { testMode = value; }
            get { return testMode; }
        }
        public USAddressValidate(string accessCode, string userId, string password)
        {
            this.sAccessCode = accessCode;
            this.sUserId = userId;
            this.sPassword = password;
        }

        public bool IsValid(string city, string stateAB, string zipcode)
        {

            StatusMessage = "";
            // Uri uri = new Uri(UPS_URL);
            byte[] bb = new System.Text.ASCIIEncoding().GetBytes(string.Format(UPSBase.XML_CONNECT, sAccessCode, sUserId, sPassword)
                + string.Format(XML_ADDRESS, city, stateAB, zipcode));

            System.Net.WebClient wc = new System.Net.WebClient();
            xmlRespone = new XmlDocument();
            string serverName = (testMode) ? "wwwcie" : "www";
            serverName = string.Format(UPS_SERVICE_URL, serverName, "AV");
            xmlRespone.LoadXml(System.Text.ASCIIEncoding.ASCII.GetString(wc.UploadData(serverName, "POST", bb)));

            bool success = false;

            XmlNode m = xmlRespone.SelectSingleNode("AddressValidationResponse/Response/ResponseStatusCode");
            if (m != null)
                success = (int.Parse(m.InnerText) == 1);


            m = xmlRespone.SelectSingleNode("AddressValidationResponse/Response/ResponseStatusDescription");

            if (m != null)
                StatusMessage = m.InnerText;

            if (!success)
                return success;

            XmlNode m2 = xmlRespone.SelectSingleNode("AddressValidationResponse/AddressValidationResult/Address/StateProvinceCode");
            if (m2 == null)
                return false;

            return string.Compare(stateAB.Trim(), m2.InnerText.Trim(), true) == 0;



        }
    }


}
