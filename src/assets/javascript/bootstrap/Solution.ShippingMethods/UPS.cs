using System;
using CollectionBase = System.Collections.CollectionBase;
using ArrayList = System.Collections.ArrayList;
using Hashtable = System.Collections.Hashtable;
using IDictionaryEnumerator = System.Collections.IDictionaryEnumerator;
using StreamReader = System.IO.StreamReader;
using Stream = System.IO.Stream;
using HttpWebRequest = System.Net.HttpWebRequest;
using WebRequest = System.Net.WebRequest;
using HttpWebResponse = System.Net.HttpWebResponse;
using WebResponse = System.Net.WebResponse;
using StringBuilder = System.Text.StringBuilder;
using ASCIIEncoding = System.Text.ASCIIEncoding;
using XmlDocument = System.Xml.XmlDocument;
using XmlNodeList = System.Xml.XmlNodeList;
using XmlNode = System.Xml.XmlNode;
using CultureInfo = System.Globalization.CultureInfo;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;
using System.Data;
using System.Xml;
using System.Net;
using System.Net.Security;

namespace Solution.ShippingMethods
{

    #region Enumeration
    public enum ResidenceTypes : int
    {
        Unknown = 0,
        Residential = 1,
        Commercial = 2
    }

    #endregion

    public class UPS
    {
        #region Variable and Declaration

        //User Details
        private string m_upsLogin;
        private string m_upsServer;
        private string m_upsUsername;
        private string m_upsPassword;
        private string m_upsLicense;
        private bool m_TestMode;
        private string m_upsTestServe;
        //

        //Original Client Details
        private string m_OriginAddress;
        private string m_OriginAddress2;
        private string m_OriginCity;
        private string m_OriginStateProvince;
        private string m_OriginZipPostalCode;
        private string m_OriginCountry;

        //Destination Details
        private string m_DestinationAddress;
        private string m_DestinationAddress2;
        private string m_DestinationCity;
        private string m_DestinationStateProvince;
        private string m_DestinationZipPostalCode;
        private string m_DestinationCountry;
        private ResidenceTypes m_DestinationResidenceType;
        //

        //Shipping Details
        private Decimal m_ShipmentWeight;
        private decimal m_ShipmentValue;
        private Decimal m_Length;	// Length of the package in inches
        private Decimal m_Width;	// Width of the package in inches
        private Decimal m_Height;	// Height of the package in inches
        private ArrayList ratesValues;
        private string m_upsLocalizationWeightUnits;
        private string m_upsMaxWeight;
        private string m_upsCallforShippingPromt;
        private string m_upsPickupType;
        private string m_upsWeightUnits;
        private string m_upsFirstClassShipWeight;
        private string m_upsSurcharge;
        private string m_upsMinimumPackageWeight;
        private bool m_Insured;
        private string m_PackagePickupType;
        //



        #endregion

        #region Propertiy

        public string UPSServer	// URL To ups server, either test or live
        {
            get { return m_upsServer; }
            set { m_upsServer = value.Trim(); }
        }

        public string UPSUsername	// URL To ups server, either test or live
        {
            get { return m_upsUsername; }
            set { m_upsUsername = value.Trim(); }
        }

        public string UPSPassword	// URL To ups server, either test or live
        {
            get { return m_upsPassword; }
            set { m_upsPassword = value.Trim(); }
        }

        public string UPSLicense	// URL To ups server, either test or live
        {
            get { return m_upsLicense; }
            set { m_upsLicense = value.Trim(); }
        }

        public bool TestMode	// Boolean value to set entire class into test mode. Only test servers will be used if applicable
        {
            get { return m_TestMode; }
            set { m_TestMode = value; }
        }

        public string upsTestServe
        {
            get { return m_upsTestServe; }
            set { m_upsTestServe = value; }
        }

        public string upsLocalizationWeightUnits
        {
            get { return m_upsLocalizationWeightUnits; }
            set { m_upsLocalizationWeightUnits = value; }
        }

        public string upsMaxWeight
        {
            get { return m_upsMaxWeight; }
            set { m_upsMaxWeight = value; }
        }

        public string upsCallforShippingPromt
        {
            get { return m_upsCallforShippingPromt; }
            set { m_upsCallforShippingPromt = value; }
        }

        public string upsWeightUnits
        {
            get { return m_upsWeightUnits; }
            set { m_upsWeightUnits = value; }
        }

        public string upsSurcharge
        {
            get { return m_upsSurcharge; }
            set { m_upsSurcharge = value; }
        }

        public string upsMinimumPackageWeight
        {
            get { return m_upsMinimumPackageWeight; }
            set { m_upsMinimumPackageWeight = value; }
        }


        public ResidenceTypes DestinationResidenceType	// Shipment Destination ResidenceType
        {
            get { return m_DestinationResidenceType; }
            set { m_DestinationResidenceType = value; }
        }

        public string OriginAddress	// Shipment origin street address
        {
            get { return m_OriginAddress; }
            set { m_OriginAddress = value; }
        }

        public string OriginAddress2	// Shipment origin street address continued
        {
            get { return m_OriginAddress2; }
            set { m_OriginAddress2 = value; }
        }

        public string OriginCity	// Shipment origin city
        {
            get { return m_OriginCity; }
            set { m_OriginCity = value; }
        }

        public string OriginStateProvince	// Shipment origin State or Province
        {
            get { return m_OriginStateProvince; }
            set { m_OriginStateProvince = value; }
        }

        public string OriginZipPostalCode	// Shipment Origin Zip or Postal Code
        {
            get { return m_OriginZipPostalCode; }
            set { m_OriginZipPostalCode = value; }
        }

        public string OriginCountry	// Shipment Origin Country
        {
            get { return m_OriginCountry; }
            set { m_OriginCountry = value; }
        }

        public Decimal ShipmentWeight	// Shipment shipmentWeight
        {
            get { return m_ShipmentWeight; }
            set { m_ShipmentWeight = value; }
        }

        public decimal ShipmentValue	//  Shipment value
        {
            get { return m_ShipmentValue; }
            set { m_ShipmentValue = value; }
        }

        public string UPSLogin	// UPS Login information, "Username,Password,License" Please note: The login information is case sensitive
        {
            get { return m_upsLogin; }
            set
            {
                m_upsLogin = value;
                string[] arrUpsLogin = m_upsLogin.Split(',');
                try
                {
                    m_upsUsername = arrUpsLogin[0].Trim();
                    m_upsPassword = arrUpsLogin[1].Trim();
                    m_upsLicense = arrUpsLogin[2].Trim();
                }
                catch { }
            }
        }


        public bool Insured
        {
            get { return m_Insured; }
            set { m_Insured = value; }
        }



        public string DestinationZipPostalCode
        {
            get { return m_DestinationZipPostalCode; }
            set { m_DestinationZipPostalCode = value; }
        }


        public string DestinationCountryCode
        {
            get { return m_DestinationCountry; }
            set { m_DestinationCountry = value; }
        }

        public string DestinationStateProvince
        {
            get { return m_DestinationStateProvince; }
            set { m_DestinationStateProvince = value; }
        }

        public string PackagePickupType
        {
            get { return m_PackagePickupType; }
            set { m_PackagePickupType = value; }
        }


        #endregion

        #region Package

        public class Packages : CollectionBase	// Data class which holds the multiples packages information
        {
            #region Variables
            private string m_Pickuptype;
            private string m_DestinationAddress1;
            private string m_DestinationAddress2;
            private string m_DestinationCity;
            private string m_DestinationStateProvince;
            private string m_DestinationZipPostalCode;
            private string m_DestinationCountryCode;
            private ResidenceTypes m_DestinationResidenceType;
            private Decimal m_Weight;
            private CultureInfo USCulture = new CultureInfo("en-US");
            #endregion



            #region Contructor
            public Packages()
            {
                m_Pickuptype = string.Empty;
                m_DestinationAddress1 = string.Empty;
                m_DestinationAddress2 = string.Empty;
                m_DestinationCity = string.Empty;
                m_DestinationStateProvince = string.Empty;
                m_DestinationZipPostalCode = string.Empty;
                m_DestinationCountryCode = string.Empty;
                m_Weight = 0.0M;
            }
            #endregion

            #region Properties
            public Decimal Weight
            {
                get
                {
                    for (int i = 0; i < this.List.Count; i++)
                    {
                        Package p = (Package)this.List[i];
                        this.m_Weight += p.Weight;
                        p = null;
                    }

                    return this.m_Weight;
                }
            }

            public string PickupType	// Shipment pickup type
            {
                get { return this.m_Pickuptype; }
                set { this.m_Pickuptype = value.Trim(); }
            }

            public string DestinationCity
            {
                get { return this.m_DestinationCity; }
                set { this.m_DestinationCity = value; }
            }

            public string DestinationAddress1
            {
                get { return this.m_DestinationAddress1; }
                set { this.m_DestinationAddress1 = value; }
            }

            public string DestinationZipPostalCode
            {
                get { return this.m_DestinationZipPostalCode; }
                set { this.m_DestinationZipPostalCode = value; }
            }

            public string DestinationAddress2
            {
                get { return this.m_DestinationAddress2; }
                set { this.m_DestinationAddress2 = value; }
            }

            public string DestinationStateProvince	// Shipment destination State or Province
            {
                get
                {
                    if (m_DestinationStateProvince == "-" || m_DestinationStateProvince == "--" || m_DestinationStateProvince == "ZZ")
                    {
                        return String.Empty;
                    }
                    else
                    {
                        return m_DestinationStateProvince;
                    }
                }
                set { m_DestinationStateProvince = value; }
            }

            public string DestinationCountryCode
            {
                get { return this.m_DestinationCountryCode; }
                set { this.m_DestinationCountryCode = value; }
            }

            public ResidenceTypes DestinationResidenceType
            {
                get { return this.m_DestinationResidenceType; }
                set { this.m_DestinationResidenceType = value; }
            }

            public Package this[int index]
            {
                get
                {
                    return (Package)this.List[index];
                }
            }

            #endregion

            #region AddPackage
            /// <summary>
            /// Add Package in List
            /// </summary>
            /// <param name="package"></param>
            public void AddPackage(Package package)
            {
                this.List.Add(package);
            }
            #endregion
        }


        public class Package // Data class which holds information about a single package 
        {
            #region Variables
            private Decimal m_Weight;
            private Decimal m_Height;
            private Decimal m_Length;
            private Decimal m_Width;
            private bool m_Insured;
            private Decimal m_InsuredValue;
            private int m_PackageId;
            #endregion

            #region Properties
            public Decimal InsuredValue
            {
                get { return m_InsuredValue; }
                set { m_InsuredValue = value; }
            }

            public int PackageId
            {
                get { return m_PackageId; }
                set { m_PackageId = value; }
            }

            public bool Insured
            {
                get { return m_Insured; }
                set { m_Insured = value; }
            }

            public Decimal Width
            {
                get { return m_Width; }
                set { m_Width = value; }
            }

            public Decimal Weight
            {
                get { return m_Weight; }
                set { m_Weight = value; }
            }

            public Decimal Height
            {
                get { return m_Height; }
                set { m_Height = value; }
            }

            public Decimal Length
            {
                get { return m_Length; }
                set { m_Length = value; }
            }
            #endregion

            #region Contructor
            public Package()
            {
            }
            #endregion

        }

        #endregion

        #region UPS Rate
        public void UPSGetRateDetials(Packages Shipment, decimal ExtraFee, Decimal TotalWeight, bool isclient)
        {
            string RTShipRequest = String.Empty;
            string RTShipResponse = String.Empty;
            string Showon = String.Empty;
            // check all required info
            if (m_upsLogin == string.Empty || m_upsUsername == string.Empty || m_upsPassword == string.Empty || m_upsLicense == string.Empty)
            {

                ratesValues.Add("Error: You must provide UPS login information");
                return;
            }

            if (Shipment.DestinationStateProvince == "AE")
            {

                ratesValues.Add("UPS Does not ship to APO Boxes");
                return;
            }

            // Check for test mode
            if (m_TestMode)
            {
                m_upsServer = m_upsTestServe; //AppLogic.AppConfig("RTShipping.UPS.TestServer");
            }

            // Check server setting
            if (m_upsServer == string.Empty)
            {

                ratesValues.Add("Error: You must provide the UPS server");
                return;
            }

            // Check for m_ShipmentWeight
            if (m_ShipmentWeight == 0.0M)
            {

                ratesValues.Add("Error: Shipment Weight must be greater than 0 " + m_upsLocalizationWeightUnits); //AppLogic.AppConfig("Localization.WeightUnits") + ".");
                return;
            }
            Decimal maxWeight = Convert.ToDecimal(m_upsMaxWeight);
            if (maxWeight == 0)
            {
                maxWeight = 150;
            }
            //*Brijesh Shah if (m_ShipmentWeight > maxWeight)
            //{
            //    ratesText.Add("UPS " + m_upsCallforShippingPromt); //AppLogic.AppConfig("RTShipping.CallForShippingPrompt"));
            //    ratesValues.Add("UPS " + m_upsCallforShippingPromt);  //AppLogic.AppConfig("RTShipping.CallForShippingPrompt"));
            //    return;
            //}


            if (isclient == true)
            {
                Showon = " ShowOnClient=1 ";
            }
            else
            {
                Showon = " ShowOnAdmin=1 ";
            }

            // Set the access request Xml
            String accessRequest = string.Format("<?xml version=\"1.0\"?><AccessRequest xml:lang=\"en-us\"><AccessLicenseNumber>{0}</AccessLicenseNumber><UserId>{1}</UserId><Password>{2}</Password></AccessRequest>", this.m_upsLicense, this.m_upsUsername, this.m_upsPassword);

            // Set the rate request Xml
            StringBuilder shipmentRequest = new StringBuilder(1024);
            shipmentRequest.Append("<?xml version=\"1.0\"?>");
            shipmentRequest.Append("<RatingServiceSelectionRequest xml:lang=\"en-US\">");
            shipmentRequest.Append("<Request>");
            shipmentRequest.Append("<RequestAction>Rate</RequestAction>");
            shipmentRequest.Append("<RequestOption>Shop</RequestOption>");
            shipmentRequest.Append("<TransactionReference>");
            shipmentRequest.Append("<CustomerContext>Rating and Service</CustomerContext>");
            shipmentRequest.Append("<XpciVersion>1.0001</XpciVersion>");
            shipmentRequest.Append("</TransactionReference>");
            shipmentRequest.Append("</Request>");
            shipmentRequest.Append("<PickupType>");
            shipmentRequest.Append("<Code>");
            shipmentRequest.Append(MapPickupType(Shipment.PickupType));
            shipmentRequest.Append("</Code>");
            shipmentRequest.Append("</PickupType>");
            //Add proper elements to support SuggestedRetailRates
            if (PackagePickupType.ToUpperInvariant() == "UPSSUGGESTEDRETAILRATES")
            {
                shipmentRequest.Append("<CustomerClassification>");
                shipmentRequest.Append("<Code>04</Code>");
                shipmentRequest.Append("</CustomerClassification>");
            }
            shipmentRequest.Append("<Shipment>");
            shipmentRequest.Append("<Shipper>");
            shipmentRequest.Append("<Address>");
            shipmentRequest.Append("<City>");
            shipmentRequest.Append(m_OriginCity.ToUpperInvariant());
            shipmentRequest.Append("</City>");
            shipmentRequest.Append("<StateProvinceCode>");
            shipmentRequest.Append(m_OriginStateProvince.ToUpperInvariant());
            shipmentRequest.Append("</StateProvinceCode>");
            shipmentRequest.Append("<PostalCode>");
            if (m_OriginZipPostalCode.Length > 5)
            {
                shipmentRequest.Append(m_OriginZipPostalCode.Substring(0, 5));
            }
            else
            {
                shipmentRequest.Append(m_OriginZipPostalCode);
            }
            shipmentRequest.Append("</PostalCode>");
            shipmentRequest.Append("<CountryCode>");
            shipmentRequest.Append(m_OriginCountry.ToUpperInvariant());
            shipmentRequest.Append("</CountryCode>");
            shipmentRequest.Append("</Address>");
            shipmentRequest.Append("</Shipper>");
            shipmentRequest.Append("<ShipTo>");
            shipmentRequest.Append("<Address>");
            shipmentRequest.Append("<City>");
            shipmentRequest.Append(Shipment.DestinationCity.ToUpperInvariant());
            shipmentRequest.Append("</City>");
            shipmentRequest.Append("<StateProvinceCode>");
            shipmentRequest.Append(Shipment.DestinationStateProvince.ToUpperInvariant());
            shipmentRequest.Append("</StateProvinceCode>");
            shipmentRequest.Append("<PostalCode>");
            if (Shipment.DestinationCountryCode.ToUpperInvariant() == "US" && Shipment.DestinationZipPostalCode.Length > 5)
            {
                shipmentRequest.Append(Shipment.DestinationZipPostalCode.Substring(0, 5));
            }
            else
            {
                shipmentRequest.Append(Shipment.DestinationZipPostalCode);
            }
            shipmentRequest.Append("</PostalCode>");
            shipmentRequest.Append("<CountryCode>");
            shipmentRequest.Append(Shipment.DestinationCountryCode.ToUpperInvariant());
            shipmentRequest.Append("</CountryCode>");
            if (this.m_DestinationResidenceType == ResidenceTypes.Commercial)
                shipmentRequest.Append("");
            else
                shipmentRequest.Append("<ResidentialAddressIndicator/>");
            shipmentRequest.Append("</Address>");
            shipmentRequest.Append("</ShipTo>");
            shipmentRequest.Append("<ShipmentWeight>");
            shipmentRequest.Append("<UnitOfMeasurement>");
            shipmentRequest.Append("<Code>");
            shipmentRequest.Append(m_upsWeightUnits); //AppLogic.AppConfig("RTShipping.WeightUnits").Trim().ToUpperInvariant());
            shipmentRequest.Append("</Code>");
            shipmentRequest.Append("</UnitOfMeasurement>");
            shipmentRequest.Append("<Weight>");
            shipmentRequest.Append(Convert.ToString(TotalWeight));
            shipmentRequest.Append("</Weight>");
            shipmentRequest.Append("</ShipmentWeight>");



            // loop through the packages
            foreach (Package pack in Shipment)
            {
                //Check for invalid weights and assign a new value if necessary
                if (pack.Weight < Convert.ToDecimal(m_upsMinimumPackageWeight))   //Convert.ToDecimal(AppLogic.AppConfig("UPS.MinimumPackageWeight")))
                {
                    pack.Weight = Convert.ToDecimal(m_upsMinimumPackageWeight);  //Convert.ToDecimal(AppLogic.AppConfig("UPS.MinimumPackageWeight"));
                }

                shipmentRequest.Append("<Package>");
                shipmentRequest.Append("<PackagingType>");
                shipmentRequest.Append("<Code>02</Code>");
                shipmentRequest.Append("</PackagingType>");
                shipmentRequest.Append("<Dimensions>");
                shipmentRequest.Append("<UnitOfMeasurement>");
                shipmentRequest.Append("<Code>IN</Code>");
                shipmentRequest.Append("</UnitOfMeasurement>");
                shipmentRequest.Append("<Length>");
                shipmentRequest.Append(pack.Length.ToString());
                shipmentRequest.Append("</Length>");
                shipmentRequest.Append("<Width>");
                shipmentRequest.Append(pack.Width.ToString());
                shipmentRequest.Append("</Width>");
                shipmentRequest.Append("<Height>");
                shipmentRequest.Append(pack.Height.ToString());
                shipmentRequest.Append("</Height>");
                shipmentRequest.Append("</Dimensions>");
                shipmentRequest.Append("<Description>");
                shipmentRequest.Append(pack.PackageId.ToString());
                shipmentRequest.Append("</Description>");
                shipmentRequest.Append("<PackageWeight>");
                shipmentRequest.Append("<UnitOfMeasure>");
                shipmentRequest.Append("<Code>");
                shipmentRequest.Append(m_upsWeightUnits.Trim().ToUpperInvariant());
                //shipmentRequest.Append(AppLogic.AppConfig("RTShipping.WeightUnits").Trim().ToUpperInvariant());
                shipmentRequest.Append("</Code>");
                shipmentRequest.Append("</UnitOfMeasure>");
                shipmentRequest.Append("<Weight>");
                shipmentRequest.Append(Convert.ToDecimal(pack.Weight));
                shipmentRequest.Append("</Weight>");
                shipmentRequest.Append("</PackageWeight>");
                shipmentRequest.Append("<OversizePackage />");

                if (pack.Insured && (pack.InsuredValue != 0))
                {
                    shipmentRequest.Append("<AdditionalHandling />");
                    shipmentRequest.Append("<PackageServiceOptions>");
                    shipmentRequest.Append("<InsuredValue>");
                    shipmentRequest.Append("<CurrencyCode>USD</CurrencyCode>");
                    shipmentRequest.Append("<MonetaryValue>");
                    shipmentRequest.Append(Convert.ToString(pack.InsuredValue));
                    shipmentRequest.Append("</MonetaryValue>");
                    shipmentRequest.Append("</InsuredValue>");
                    shipmentRequest.Append("</PackageServiceOptions>");
                }

                shipmentRequest.Append("</Package>");
            }

            shipmentRequest.Append("<ShipmentServiceOptions/></Shipment></RatingServiceSelectionRequest>");

            // Concat the requests
            String fullUPSRequest = accessRequest + shipmentRequest.ToString();

            RTShipRequest = fullUPSRequest;

            // Send request & capture response

            string result = POSTandReceiveData(fullUPSRequest, m_upsServer);

            RTShipResponse = result;

            // Load Xml into a XmlDocument object
            XmlDocument UPSResponse = new XmlDocument();
            try
            {
                UPSResponse.LoadXml(result);
            }
            catch
            {

                ratesValues.Add("Error: UPS Gateway Did Not Respond");
                return;
            }
            // Get Response code: 0 = Fail, 1 = Success
            XmlNodeList UPSResponseCode = UPSResponse.GetElementsByTagName("ResponseStatusCode");
            if (UPSResponseCode[0].InnerText == "1") // Success
            {
                // Loop through elements & get rates
                XmlNodeList ratedShipments = UPSResponse.GetElementsByTagName("RatedShipment");
                string tempService = string.Empty;
                Decimal tempRate = 0.0M;


                for (int i = 0; i < ratedShipments.Count; i++)
                {
                    XmlNode shipmentX = ratedShipments.Item(i);
                    tempService = UPSServiceCodeDescription(shipmentX["Service"]["Code"].InnerText);

                    DataSet dsCommon = new DataSet();
                    string SelectQuery = " SELECT * FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                     " WHERE ShippingService='UPS' AND tb_ShippingMethods.Active=1 and isnull(isRTShipping,0)=1 AND tb_ShippingMethods.Deleted=0  AND Name='" + tempService + "' and " + Showon + "";

                    dsCommon = CommonComponent.GetCommonDataSet(SelectQuery);
                    if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0)
                    {

                        if (dsCommon.Tables[0].Rows[0]["Name"] != null)
                        {

                            if (Convert.ToBoolean(dsCommon.Tables[0].Rows[0]["isRTShipping"]))
                            {
                                tempRate = Convert.ToDecimal(shipmentX["TotalCharges"]["MonetaryValue"].InnerText);
                            }
                            else
                            {
                                if (dsCommon.Tables[0].Rows[0]["FixedPrice"] != null && dsCommon.Tables[0].Rows[0]["FixedPrice"] != DBNull.Value)
                                {
                                    tempRate = Convert.ToDecimal(dsCommon.Tables[0].Rows[0]["FixedPrice"].ToString());
                                }
                                else
                                {
                                    tempRate = 0;
                                }
                            }

                            if (dsCommon.Tables[0].Rows[0]["AdditionalPrice"] != null && dsCommon.Tables[0].Rows[0]["AdditionalPrice"] != DBNull.Value)
                            {

                                tempRate += Convert.ToDecimal(dsCommon.Tables[0].Rows[0]["AdditionalPrice"]);

                            }

                        }



                        #region"percent"
                        //Brijesh Shah- Percentage Add Logic
                        //if (MarkupPercent != System.Decimal.Zero)
                        //{
                        //    tempRate = tempRate * (1.00M + (MarkupPercent / 100.0M));
                        //}
                        #endregion

                        tempRate += ExtraFee;
                        decimal UPSSurcharge = decimal.Zero;
                        UPSSurcharge = Convert.ToDecimal(m_upsSurcharge); //Convert.ToDecimal(AppLogic.AppConfig("UPSSurcharge").ToString());
                        //** Brijesh Shah decimal vat = Decimal.Round(tempRate * ShippingTaxRate, 2, MidpointRounding.AwayFromZero);
                        // ratesText.Add(tempService + " " + Convert.ToString(tempRate + UPSSurcharge));
                        ratesValues.Add(tempService + "|" + Convert.ToString(tempRate + UPSSurcharge));//** Brijesh Shah + "|" + Convert.ToString(vat));


                    }




                }
            }
            else // Error
            {
                XmlNodeList UPSError = UPSResponse.GetElementsByTagName("ErrorDescription");
                // ratesText.Add("UPS Error: " + UPSError[0].InnerText);
                ratesValues.Add("UPS Error: " + UPSError[0].InnerText);
                UPSError = null;
                return;
            }

            // Some clean up
            UPSResponseCode = null;
            UPSResponse = null;
        }


        public void UPSGetRateDetialsAdmin(Packages Shipment,  bool isclient)
        {
            string RTShipRequest = String.Empty;
            string RTShipResponse = String.Empty;
            string Showon = String.Empty;
            // check all required info
            if (m_upsLogin == string.Empty || m_upsUsername == string.Empty || m_upsPassword == string.Empty || m_upsLicense == string.Empty)
            {

                ratesValues.Add("Error: You must provide UPS login information");
                return;
            }

            if (Shipment.DestinationStateProvince == "AE")
            {

                ratesValues.Add("UPS Does not ship to APO Boxes");
                return;
            }

            // Check for test mode
            if (m_TestMode)
            {
                m_upsServer = m_upsTestServe; //AppLogic.AppConfig("RTShipping.UPS.TestServer");
            }

            // Check server setting
            if (m_upsServer == string.Empty)
            {

                ratesValues.Add("Error: You must provide the UPS server");
                return;
            }

            // Check for m_ShipmentWeight
            if (m_ShipmentWeight == 0.0M)
            {

                ratesValues.Add("Error: Shipment Weight must be greater than 0 " + m_upsLocalizationWeightUnits); //AppLogic.AppConfig("Localization.WeightUnits") + ".");
                return;
            }
            Decimal maxWeight = Convert.ToDecimal(m_upsMaxWeight);
            if (maxWeight == 0)
            {
                maxWeight = 150;
            }
            //*Brijesh Shah if (m_ShipmentWeight > maxWeight)
            //{
            //    ratesText.Add("UPS " + m_upsCallforShippingPromt); //AppLogic.AppConfig("RTShipping.CallForShippingPrompt"));
            //    ratesValues.Add("UPS " + m_upsCallforShippingPromt);  //AppLogic.AppConfig("RTShipping.CallForShippingPrompt"));
            //    return;
            //}


            if (isclient == true)
            {
                Showon = " ShowOnClient=1 ";
            }
            else
            {
                Showon = " ShowOnAdmin=1 ";
            }

            // Set the access request Xml
            String accessRequest = string.Format("<?xml version=\"1.0\"?><AccessRequest xml:lang=\"en-us\"><AccessLicenseNumber>{0}</AccessLicenseNumber><UserId>{1}</UserId><Password>{2}</Password></AccessRequest>", this.m_upsLicense, this.m_upsUsername, this.m_upsPassword);

            // Set the rate request Xml
            StringBuilder shipmentRequest = new StringBuilder(1024);
            shipmentRequest.Append("<?xml version=\"1.0\"?>");
            shipmentRequest.Append("<RatingServiceSelectionRequest xml:lang=\"en-US\">");
            shipmentRequest.Append("<Request>");
            shipmentRequest.Append("<RequestAction>Rate</RequestAction>");
            shipmentRequest.Append("<RequestOption>Shop</RequestOption>");
            shipmentRequest.Append("<TransactionReference>");
            shipmentRequest.Append("<CustomerContext>Rating and Service</CustomerContext>");
            shipmentRequest.Append("<XpciVersion>1.0001</XpciVersion>");
            shipmentRequest.Append("</TransactionReference>");
            shipmentRequest.Append("</Request>");
            shipmentRequest.Append("<PickupType>");
            shipmentRequest.Append("<Code>");
            shipmentRequest.Append(MapPickupType(Shipment.PickupType));
            shipmentRequest.Append("</Code>");
            shipmentRequest.Append("</PickupType>");
            //Add proper elements to support SuggestedRetailRates
            if (PackagePickupType.ToUpperInvariant() == "UPSSUGGESTEDRETAILRATES")
            {
                shipmentRequest.Append("<CustomerClassification>");
                shipmentRequest.Append("<Code>04</Code>");
                shipmentRequest.Append("</CustomerClassification>");
            }
            shipmentRequest.Append("<Shipment>");
            shipmentRequest.Append("<Shipper>");
            shipmentRequest.Append("<Address>");
            shipmentRequest.Append("<City>");
            shipmentRequest.Append(m_OriginCity.ToUpperInvariant());
            shipmentRequest.Append("</City>");
            shipmentRequest.Append("<StateProvinceCode>");
            shipmentRequest.Append(m_OriginStateProvince.ToUpperInvariant());
            shipmentRequest.Append("</StateProvinceCode>");
            shipmentRequest.Append("<PostalCode>");
            shipmentRequest.Append(m_OriginZipPostalCode.Substring(0, 5));
            shipmentRequest.Append("</PostalCode>");
            shipmentRequest.Append("<CountryCode>");
            shipmentRequest.Append(m_OriginCountry.ToUpperInvariant());
            shipmentRequest.Append("</CountryCode>");
            shipmentRequest.Append("</Address>");
            shipmentRequest.Append("</Shipper>");
            shipmentRequest.Append("<ShipTo>");
            shipmentRequest.Append("<Address>");
            shipmentRequest.Append("<City>");
            shipmentRequest.Append(Shipment.DestinationCity.ToUpperInvariant());
            shipmentRequest.Append("</City>");
            shipmentRequest.Append("<StateProvinceCode>");
            shipmentRequest.Append(Shipment.DestinationStateProvince.ToUpperInvariant());
            shipmentRequest.Append("</StateProvinceCode>");
            shipmentRequest.Append("<PostalCode>");
            if (Shipment.DestinationCountryCode.ToUpperInvariant() == "US" && Shipment.DestinationZipPostalCode.Length > 5)
            {
                shipmentRequest.Append(Shipment.DestinationZipPostalCode.Substring(0, 5));
            }
            else
            {
                shipmentRequest.Append(Shipment.DestinationZipPostalCode);
            }
            shipmentRequest.Append("</PostalCode>");
            shipmentRequest.Append("<CountryCode>");
            shipmentRequest.Append(Shipment.DestinationCountryCode.ToUpperInvariant());
            shipmentRequest.Append("</CountryCode>");
            if (this.m_DestinationResidenceType == ResidenceTypes.Commercial)
                shipmentRequest.Append("");
            else
                shipmentRequest.Append("<ResidentialAddressIndicator/>");
            shipmentRequest.Append("</Address>");
            shipmentRequest.Append("</ShipTo>");
            shipmentRequest.Append("<ShipmentWeight>");
            shipmentRequest.Append("<UnitOfMeasurement>");
            shipmentRequest.Append("<Code>");
            shipmentRequest.Append(m_upsWeightUnits); //AppLogic.AppConfig("RTShipping.WeightUnits").Trim().ToUpperInvariant());
            shipmentRequest.Append("</Code>");
            shipmentRequest.Append("</UnitOfMeasurement>");
            shipmentRequest.Append("<Weight>");
            shipmentRequest.Append(Convert.ToString(Shipment.Weight));
            shipmentRequest.Append("</Weight>");
            shipmentRequest.Append("</ShipmentWeight>");



            // loop through the packages
            foreach (Package pack in Shipment)
            {
                //Check for invalid weights and assign a new value if necessary
                if (pack.Weight < Convert.ToDecimal(m_upsMinimumPackageWeight))   //Convert.ToDecimal(AppLogic.AppConfig("UPS.MinimumPackageWeight")))
                {
                    pack.Weight = Convert.ToDecimal(m_upsMinimumPackageWeight);  //Convert.ToDecimal(AppLogic.AppConfig("UPS.MinimumPackageWeight"));
                }

                shipmentRequest.Append("<Package>");
                shipmentRequest.Append("<PackagingType>");
                shipmentRequest.Append("<Code>02</Code>");
                shipmentRequest.Append("</PackagingType>");
                shipmentRequest.Append("<Dimensions>");
                shipmentRequest.Append("<UnitOfMeasurement>");
                shipmentRequest.Append("<Code>IN</Code>");
                shipmentRequest.Append("</UnitOfMeasurement>");
                shipmentRequest.Append("<Length>");
                shipmentRequest.Append(pack.Length.ToString());
                shipmentRequest.Append("</Length>");
                shipmentRequest.Append("<Width>");
                shipmentRequest.Append(pack.Width.ToString());
                shipmentRequest.Append("</Width>");
                shipmentRequest.Append("<Height>");
                shipmentRequest.Append(pack.Height.ToString());
                shipmentRequest.Append("</Height>");
                shipmentRequest.Append("</Dimensions>");
                shipmentRequest.Append("<Description>");
                shipmentRequest.Append(pack.PackageId.ToString());
                shipmentRequest.Append("</Description>");
                shipmentRequest.Append("<PackageWeight>");
                shipmentRequest.Append("<UnitOfMeasure>");
                shipmentRequest.Append("<Code>");
                shipmentRequest.Append(m_upsWeightUnits.Trim().ToUpperInvariant());
                //shipmentRequest.Append(AppLogic.AppConfig("RTShipping.WeightUnits").Trim().ToUpperInvariant());
                shipmentRequest.Append("</Code>");
                shipmentRequest.Append("</UnitOfMeasure>");
                shipmentRequest.Append("<Weight>");
                shipmentRequest.Append(Convert.ToDecimal(pack.Weight));
                shipmentRequest.Append("</Weight>");
                shipmentRequest.Append("</PackageWeight>");
                shipmentRequest.Append("<OversizePackage />");

                if (pack.Insured && (pack.InsuredValue != 0))
                {
                    shipmentRequest.Append("<AdditionalHandling />");
                    shipmentRequest.Append("<PackageServiceOptions>");
                    shipmentRequest.Append("<InsuredValue>");
                    shipmentRequest.Append("<CurrencyCode>USD</CurrencyCode>");
                    shipmentRequest.Append("<MonetaryValue>");
                    shipmentRequest.Append(Convert.ToString(pack.InsuredValue));
                    shipmentRequest.Append("</MonetaryValue>");
                    shipmentRequest.Append("</InsuredValue>");
                    shipmentRequest.Append("</PackageServiceOptions>");
                }

                shipmentRequest.Append("</Package>");
            }

            shipmentRequest.Append("<ShipmentServiceOptions/></Shipment></RatingServiceSelectionRequest>");

            // Concat the requests
            String fullUPSRequest = accessRequest + shipmentRequest.ToString();

            RTShipRequest = fullUPSRequest;

            // Send request & capture response

            string result = POSTandReceiveData(fullUPSRequest, m_upsServer);

            RTShipResponse = result;

            // Load Xml into a XmlDocument object
            XmlDocument UPSResponse = new XmlDocument();
            try
            {
                UPSResponse.LoadXml(result);
            }
            catch
            {

                ratesValues.Add("Error: UPS Gateway Did Not Respond");
                return;
            }
            // Get Response code: 0 = Fail, 1 = Success
            XmlNodeList UPSResponseCode = UPSResponse.GetElementsByTagName("ResponseStatusCode");
            if (UPSResponseCode[0].InnerText == "1") // Success
            {
                // Loop through elements & get rates
                XmlNodeList ratedShipments = UPSResponse.GetElementsByTagName("RatedShipment");
                string tempService = string.Empty;
                Decimal tempRate = 0.0M;


                for (int i = 0; i < ratedShipments.Count; i++)
                {
                    XmlNode shipmentX = ratedShipments.Item(i);
                    tempService = UPSServiceCodeDescription(shipmentX["Service"]["Code"].InnerText);

                    DataSet dsCommon = new DataSet();
                    string SelectQuery = " SELECT * FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                     " WHERE ShippingService='UPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0  AND Name='" + tempService + "' and " + Showon + "";

                    dsCommon = CommonComponent.GetCommonDataSet(SelectQuery);
                    if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0)
                    {

                        if (dsCommon.Tables[0].Rows[0]["Name"] != null)
                        {

                            if (Convert.ToBoolean(dsCommon.Tables[0].Rows[0]["isRTShipping"]))
                            {
                                tempRate = Convert.ToDecimal(shipmentX["TotalCharges"]["MonetaryValue"].InnerText);
                            }
                            else
                            {
                                if (dsCommon.Tables[0].Rows[0]["FixedPrice"] != null && dsCommon.Tables[0].Rows[0]["FixedPrice"] != DBNull.Value)
                                {
                                    tempRate = Convert.ToDecimal(dsCommon.Tables[0].Rows[0]["FixedPrice"].ToString());
                                }
                                else
                                {
                                    tempRate = 0;
                                }
                            }

                            if (dsCommon.Tables[0].Rows[0]["AdditionalPrice"] != null && dsCommon.Tables[0].Rows[0]["AdditionalPrice"] != DBNull.Value)
                            {

                                tempRate += Convert.ToDecimal(dsCommon.Tables[0].Rows[0]["AdditionalPrice"]);

                            }

                        }



                        #region"percent"
                        //Brijesh Shah- Percentage Add Logic
                        //if (MarkupPercent != System.Decimal.Zero)
                        //{
                        //    tempRate = tempRate * (1.00M + (MarkupPercent / 100.0M));
                        //}
                        #endregion

                       // tempRate += ExtraFee;
                        decimal UPSSurcharge = decimal.Zero;
                        UPSSurcharge = Convert.ToDecimal(m_upsSurcharge); //Convert.ToDecimal(AppLogic.AppConfig("UPSSurcharge").ToString());
                        //** Brijesh Shah decimal vat = Decimal.Round(tempRate * ShippingTaxRate, 2, MidpointRounding.AwayFromZero);
                        // ratesText.Add(tempService + " " + Convert.ToString(tempRate + UPSSurcharge));
                        ratesValues.Add(tempService + "|" + Convert.ToString(tempRate + UPSSurcharge));//** Brijesh Shah + "|" + Convert.ToString(vat));


                    }




                }
            }
            else // Error
            {
                XmlNodeList UPSError = UPSResponse.GetElementsByTagName("ErrorDescription");
                // ratesText.Add("UPS Error: " + UPSError[0].InnerText);
                ratesValues.Add("UPS Error: " + UPSError[0].InnerText);
                UPSError = null;
                return;
            }

            // Some clean up
            UPSResponseCode = null;
            UPSResponse = null;
        }



        // public Object UPSGetRates(decimal TotalWeight, out string RTShipRequest, out string RTShipResponse, decimal ExtraFee, Decimal MarkupPercent, decimal ShippingTaxRate)
        public Object UPSGetRates(decimal TotalWeight, decimal ExtraFee, bool Isclient)
        {
            object returnObject = null;
            StringBuilder output = new StringBuilder(1024);
            Decimal remainingItemsWeight = 0.0M;
            Decimal remainingItemsInsuranceValue = 0.0M;
            int PackageID = 1;
            int LoopCount = 1;

            //Create Packages
            Packages UpsPackage = new Packages();
            UpsPackage.PickupType = PackagePickupType;
            UpsPackage.DestinationZipPostalCode = DestinationZipPostalCode;
            UpsPackage.DestinationCountryCode = DestinationCountryCode; ;//AppLogic.GetCountryTwoLetterISOCodeByID(Convert.ToInt32(Country));
            if (UpsPackage.DestinationCountryCode.ToLower() != "us")
            {
                UpsPackage.DestinationStateProvince = "";// AppLogic.IIF(String.IsNullOrEmpty(State), "", State);

            }
            else
            {
                UpsPackage.DestinationStateProvince = DestinationStateProvince;// AppLogic.IIF(String.IsNullOrEmpty(AppLogic.GetStateTwoLetterISOCode(State)), "", AppLogic.GetStateTwoLetterISOCode(State));
            }

            UpsPackage.DestinationResidenceType = ResidenceTypes.Residential;
            DestinationResidenceType = UpsPackage.DestinationResidenceType;
            if (TotalWeight > ShipmentWeight)
            {
                LoopCount = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(TotalWeight / ShipmentWeight)));
                remainingItemsWeight = TotalWeight % ShipmentWeight;
            }
            else
            {
                remainingItemsWeight = 0.0M;
            }

            for (int i = 0; i < LoopCount; i++)
            {
                Package pack = new Package();
                pack.PackageId = PackageID;

                if (TotalWeight > ShipmentWeight)
                {
                    pack.Weight = ShipmentWeight;
                }
                else
                {
                    pack.Weight = TotalWeight;
                }
                pack.Insured = Insured; //false;
                pack.InsuredValue = remainingItemsInsuranceValue;
                UpsPackage.AddPackage(pack);
                pack = null;
            }

            if (remainingItemsWeight != 0.0M)
            {
                // Create package object for this item
                Package pack = new Package();
                pack.PackageId = PackageID;
                PackageID = PackageID + 1;

                pack.Weight = remainingItemsWeight;

                // Set insurance. Get from products db shipping values?
                pack.Insured = Insured;
                pack.InsuredValue = remainingItemsInsuranceValue;

                // Add package to collection
                UpsPackage.AddPackage(pack);
                // shipmentUSPS.AddPackage(p);

                pack = null;
            }

            //Get Shipping Rate
            UPSGetRateDetials(UpsPackage, ExtraFee, TotalWeight, Isclient);

            String separator = String.Empty;
            for (int i = 0; i < ratesValues.Count; i++)
            {
                output.Append(separator);
                output.Append((string)ratesValues[i].ToString().Trim());
                separator = ",";
            }
            String tmpS = output.ToString();
            returnObject = (object)tmpS;


            return returnObject;
        }


        public Object UPSGetRatesPackage(Packages Shipment)
        {
            object returnObject = null;
            StringBuilder output = new StringBuilder(1024);
            UPSGetRateDetialsAdmin(Shipment,  false);

            String separator = String.Empty;
            for (int i = 0; i < ratesValues.Count; i++)
            {
                output.Append(separator);
                output.Append((string)ratesValues[i].ToString().Trim());
                separator = ",";
            }
            String tmpS = output.ToString();
            returnObject = (object)tmpS;
            return returnObject;
        }



        #endregion

        #region MapPickupType
        /// <summary>
        /// depends on s return Code
        /// </summary>
        /// <param name="s">s - String</param>
        /// <returns>s - String</returns>
        static private String MapPickupType(String s)
        {
            s = s.Trim().ToLowerInvariant();
            if (s == "upsdailypickup")
            {
                return "01";
            }
            if (s == "upscustomercounter")
            {
                return "03";
            }
            if (s == "upsonetimepickup")
            {
                return "06";
            }
            if (s == "upsoncallair")
            {
                return "07";
            }
            if (s == "upssuggestedretailrates")
            {
                return "11";
            }
            if (s == "upslettercenter")
            {
                return "19";
            }
            if (s == "upsairservicecenter")
            {
                return "20";
            }
            return "03"; // find some default
        }
        #endregion

        #region POSTandReceiveData
        /// <summary>
        /// Send and capture data using Post
        /// </summary>
        /// <param name="Request">The Xml Request to be sent</param>
        /// <param name="Server">The server the request should be sent to</param>
        /// <returns>String</returns>
        private string POSTandReceiveData(string Request, string Server)
        {
            //ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(delegate { return true; });

            //ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            // Set encoding & get content Length
            ASCIIEncoding encoding = new ASCIIEncoding();
            byte[] data = encoding.GetBytes(Request); // Request

            // Prepare post request
            HttpWebRequest shipRequest = (HttpWebRequest)WebRequest.Create(Server); // Server
            shipRequest.Method = "POST";
            shipRequest.ContentType = "application/x-www-form-urlencoded";
            shipRequest.ContentLength = data.Length;
            Stream requestStream = shipRequest.GetRequestStream();
            // Send the data
            requestStream.Write(data, 0, data.Length);
            requestStream.Close();
            // get the response
            WebResponse shipResponse = null;
            string response = String.Empty;
            try
            {
                shipResponse = shipRequest.GetResponse();
                using (StreamReader sr = new StreamReader(shipResponse.GetResponseStream()))
                {
                    response = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception exc)
            {
                response = exc.ToString();
            }
            finally
            {
                if (shipResponse != null) shipResponse.Close();
            }

            shipRequest = null;
            requestStream = null;
            shipResponse = null;

            return response;
        }
        #endregion

        #region UPSServiceCodeDescription
        /// <summary>
        /// Convert the input number to the textual description of the Service Code
        /// </summary>
        /// <param name="code">The Service Code number to be converted</param>
        /// <returns></returns>
        private string UPSServiceCodeDescription(string code)
        {
            string result = string.Empty;
            switch (code)
            {
                //case "01":
                //    result = "UPS Next Day Air";
                //    break;
                case "02":
                    result = "UPS 2nd Day Air";
                    break;
                case "03":
                    result = "UPS Ground";
                    break;
                case "07":
                    result = "UPS Worldwide Express";
                    break;
                case "08":
                    result = "UPS Worldwide Expedited";
                    break;
                case "11":
                    result = "UPS Standard";
                    break;
                case "12":
                    result = "UPS 3-Day Select";
                    break;
                case "13":
                    result = "UPS Next Day Air Saver";
                    break;
                case "14":
                    result = "UPS Next Day Air Early AM";
                    break;
                case "54":
                    result = "UPS Worldwide Express Plus";
                    break;
                case "59":
                    result = "UPS 2nd Day Air AM";
                    break;
                case "65":
                    result = "UPS Express Saver";
                    break;
            }

            return result;
        }
        #endregion

        #region Constructor

        public UPS(string Address, string Address2, string City, string State, string Zip, string Country)
        {
            UPSLogin = AppLogic.AppConfigs("UPS.UserName") + "," + AppLogic.AppConfigs("UPS.Password") + "," + AppLogic.AppConfigs("UPS.License");
            UPSServer = AppLogic.AppConfigs("UPS.Server");


            //Max Weight
            ShipmentWeight = Convert.ToDecimal(AppLogic.AppConfigs("UPS.MaxWeight"));


            //Client Address Details
            OriginAddress = Address;// AppLogic.AppConfigs("Shipping.OriginAddress");
            OriginAddress2 = Address2;// AppLogic.AppConfigs("Shipping.OriginAddress2");
            OriginCity = City;// AppLogic.AppConfigs("Shipping.OriginCity");
            OriginStateProvince = State;// AppLogic.AppConfigs("Shipping.OriginState");
            OriginZipPostalCode = Zip;// AppLogic.AppConfigs("Shipping.OriginZip");
            OriginCountry = Country;// AppLogic.AppConfigs("Shipping.OriginCountry");

            //
            upsTestServe = AppLogic.AppConfigs("UPS.TestServer");
            upsLocalizationWeightUnits = AppLogic.AppConfigs("Localization.WeightUnits"); //not available
            upsMaxWeight = AppLogic.AppConfigs("UPS.MaxWeight");
            upsCallforShippingPromt = AppLogic.AppConfigs("Shipping.CallForShippingPrompt");

            upsWeightUnits = AppLogic.AppConfigs("Shipping.WeightUnits");

            upsMinimumPackageWeight = AppLogic.AppConfigs("UPS.MinimumPackageWeight");
            upsSurcharge = AppLogic.AppConfigs("UPS.Surcharge");
            Insured = AppLogic.AppConfigBool("UPS.Insured");

            PackagePickupType = AppLogic.AppConfigs("UPS.PickupType");

            if (OriginCountry.ToUpperInvariant() == "US")
            {
                try
                {
                    OriginZipPostalCode = OriginZipPostalCode.Substring(0, 5);
                }
                catch
                {
                    throw new Exception("The RTShipping.OriginZip AppConfig parameter is invalid, please update this value.");
                }
            }

            m_DestinationAddress = string.Empty;
            m_DestinationAddress2 = string.Empty;
            m_DestinationCity = string.Empty;
            m_DestinationStateProvince = string.Empty;
            m_DestinationZipPostalCode = string.Empty;
            m_DestinationCountry = string.Empty;
            m_DestinationResidenceType = ResidenceTypes.Unknown;
            m_ShipmentValue = System.Decimal.Zero;
            m_TestMode = false;
            ratesValues = new ArrayList();




        }




        #endregion

     
    }

  
}
