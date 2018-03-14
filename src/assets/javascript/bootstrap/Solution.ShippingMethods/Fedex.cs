using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using IDictionaryEnumerator = System.Collections.IDictionaryEnumerator;
using Solution.ShippingMethods.FedEx;
using System.Collections;
using System.Globalization;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;
namespace Solution.ShippingMethods
{
    public enum ResultType	// Enum ResultType: The available return types of the shipment rating(s)
    {
        Unknown = 0,
        PlainText = 1,	// ResultType.PlainText: Specifies the resulting output to be plain text with &lt;BR&gt; tags to separate them
        SingleDropDownList = 2,	// ResultType.SingleDropDownList: Specifies the resulting output to be a single line drop down list
        MultiDropDownList = 3,	// ResultType.MultiDropDownList: Specifies the resulting output to be a multi-line combo-box
        RadioButtonList = 4,	// ResultType.RadioButtonList: Specifies the resulting output to be a list of radio buttons with labels
        RawDelimited = 5,	// ResultType.RawDelimited: Specifes the resulting output to be a delimited string. Rates are delimited with a pipe character (|), rate names &amp; prices are delimited with a comma (,)
        DropDownListControl = 6,	// ResultType.DropDownListControl: Specifes the resulting output to be a System.Web.UI.WebControls.DropDownList control.
        RadioButtonListControl = 7	// ResultType.RadioButtonListControl: Specifes the resulting output to be a System.Web.UI.WebControls.RadioButtonList control.
    }

    public class Fedex
    {
        #region Variables
        private string m_FedExServer;
        private string m_FedExLogin;
        private string m_FedExUsername;
        private string m_FedExPassword;
        private string m_FedExLicense;
        private string m_FedexShipCountry;
        private string m_FedExShipZipCode;
        private string m_FedExShipState;
        private decimal m_FedExShippWeight;
        private int m_FedExShipProdCount;
        private bool m_TestMode;
        private ArrayList ratesValues;
        private ArrayList ratesText;
        static public CultureInfo USCulture = new CultureInfo("en-US");
        private Decimal m_ShipmentWeight;
        private ResidenceTypes m_DestinationResidenceType;
        private int m_FedExCount;
        #endregion

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

        public class PickupTypes
        {
            #region Properties
            /// <summary>
            /// Specifies the pickup type as: Daily Pickup
            /// </summary>
            public static string UPSDailyPickup
            {
                get { return "01"; }
            }
            /// <summary>
            /// Specifies the pickup type as: Customer Counter
            /// </summary>
            public static string UPSCustomerCounter
            {
                get { return "03"; }
            }
            /// <summary>
            /// Specifies the pickup type as: One time pickup
            /// </summary>
            public static string UPSOneTimePickup
            {
                get { return "06"; }
            }
            /// <summary>
            /// Specifies the pickup type as: On Call Air
            /// </summary>
            public static string UPSOnCallAir
            {
                get { return "07"; }
            }
            /// <summary>
            /// Specifies the pickup type as: Suggested retail rates
            /// </summary>
            public static string UPSSuggestedRetailRates
            {
                get { return "11"; }
            }
            /// <summary>
            /// Specifies the pickup type as: Letter center
            /// </summary>
            public static string UPSLetterCenter
            {
                get { return "19"; }
            }
            /// <summary>
            /// Specifies the pickup type as: Air service center
            /// </summary>
            public static string UPSAirServiceCenter
            {
                get { return "20"; }
            }
            #endregion
        }


        #region Properties
        public string FedExLogin
        {
            get { return m_FedExLogin; }
            set
            {
                m_FedExLogin = value;
                string[] arrFedExLogin = m_FedExLogin.Split(',');
                try
                {
                    m_FedExUsername = arrFedExLogin[0].Trim();
                    m_FedExPassword = arrFedExLogin[1].Trim();
                    m_FedExLicense = arrFedExLogin[2].Trim();
                }
                catch { }
            }
        }
        public string FedExServer	// FEDEX To ups server, either test or live
        {
            get { return m_FedExServer; }
            set { m_FedExServer = value.Trim(); }
        }

        public string FedExUsername	// FEDEX To ups server, either test or live
        {
            get { return m_FedExUsername; }
            set { m_FedExUsername = value.Trim(); }
        }

        public string FedExPassword	// FEDEX To ups server, either test or live
        {
            get { return m_FedExPassword; }
            set { m_FedExPassword = value.Trim(); }
        }

        public string FedExLicense	// FEDEX To ups server, either test or live
        {
            get { return m_FedExLicense; }
            set { m_FedExLicense = value.Trim(); }
        }

        public ResidenceTypes DestinationResidenceType	// Shipment Destination ResidenceType
        {
            get { return m_DestinationResidenceType; }
            set { m_DestinationResidenceType = value; }
        }

        public Decimal ShipmentWeight	// Shipment shipmentWeight
        {
            get { return m_ShipmentWeight; }
            set { m_ShipmentWeight = value; }
        }

        #endregion

        public Fedex()
        {
            ratesValues = new ArrayList();
            ratesText = new ArrayList();
        }

        #region FedExGetRates
        private void FedExGetRates(RateRequest request, out string RTShipRequest, out string RTShipResponse, bool isclient)
        {

            string Showon = String.Empty;
            isclient = false;
            if (isclient == true)
            {
                Showon = " ShowOnClient=1 ";
            }
            else
            {
                Showon = " ShowOnAdmin=1 ";
            }

            string strError = "";
            RateRequest request2 = request;
            string RTShipRequest2;
            string RTShipResponse2;

            #region Data Declaration
            ResultType format = ResultType.RawDelimited;
            RTShipRequest = String.Empty;
            RTShipResponse = String.Empty;

            //MarkupPercent = Decimal.Zero;
            Decimal TotalPrice = 0;
            Decimal remainingItemsInsuranceValue = 0.0M;
            m_FedExCount += 1;
            StringBuilder tmpS = new StringBuilder(4096);
            string ErrCatch = "";
            int PackageID = 1;
            decimal decTotalWeight = 0;
            #endregion

            #region Get FedEx Rates
            //request = SetIndividualPackageLineItems(request);
            RateService service = new RateService(); // Initialize the service
            string retval = string.Empty;
            try
            {
                // This is the call to the web service passing in a RateRequest and returning a RateReply
                service.Url = service.Url.Replace("gateway","gatewaybeta");
                RateReply reply = service.getRates(request); // Service call
                //
                string temp = string.Empty;
                if (reply.HighestSeverity == NotificationSeverityType.SUCCESS || reply.HighestSeverity == NotificationSeverityType.NOTE || reply.HighestSeverity == NotificationSeverityType.WARNING) // check if the call was successful
                {
                    int cntfx = 0;
                    foreach (RateReplyDetail rateDetail in reply.RateReplyDetails)
                    {
                        retval += rateDetail.ServiceType;
                        temp += "<br>" + rateDetail.ServiceType;
                        Decimal netvalu = Decimal.Zero;
                        Decimal tempRate = Decimal.Zero;
                        Decimal surcharge = Decimal.Zero;
                        foreach (RatedShipmentDetail shipmentDetail in rateDetail.RatedShipmentDetails)
                        {
                            if (netvalu == Decimal.Zero)
                                netvalu = shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount;
                            //surcharge = cntfx;

                            temp += "<br>RateType : " + shipmentDetail.ShipmentRateDetail.RateType;
                            temp += "<br>Total Billing Weight : " + shipmentDetail.ShipmentRateDetail.TotalBillingWeight.Value;
                            temp += "<br>Total Base Charge : " + shipmentDetail.ShipmentRateDetail.TotalBaseCharge.Amount;
                            temp += "<br>Total Discount : " + shipmentDetail.ShipmentRateDetail.TotalFreightDiscounts.Amount;
                            temp += "<br>Total Surcharges : " + shipmentDetail.ShipmentRateDetail.TotalSurcharges.Amount;
                            temp += "<br>Net Charge : " + shipmentDetail.ShipmentRateDetail.TotalNetCharge.Amount;

                        }
                        cntfx++; ;

                        DataSet dsCommon = new DataSet();
                        string SelectQuery = " SELECT * FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                         " WHERE ShippingService='FEDEX' AND tb_ShippingMethods.Active=1 and isnull(isRTShipping,0)=1 AND tb_ShippingMethods.Deleted=0  AND Name='" + rateDetail.ServiceType.ToString().Replace("_", " ") + "' and " + Showon + "";

                        dsCommon = CommonComponent.GetCommonDataSet(SelectQuery);
                        if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0)
                        {

                            if (dsCommon.Tables[0].Rows[0]["Name"] != null)
                            {

                                if (Convert.ToBoolean(dsCommon.Tables[0].Rows[0]["isRTShipping"]))
                                {
                                    tempRate = netvalu;// Convert.ToDecimal(shipmentX["TotalCharges"]["MonetaryValue"].InnerText);
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


                            ratesText.Add(rateDetail.ServiceType.ToString().Replace("_", " ") + " " + CurrencyStringForDBWithoutExchangeRate(tempRate));
                            ratesValues.Add(rateDetail.ServiceType.ToString().Replace("_", " ") + "|" + CurrencyStringForDBWithoutExchangeRate(tempRate) + "|" + CurrencyStringForDBWithoutExchangeRate(0));
                            //////////////////////////////////////
                            retval += "|" + netvalu.ToString() + "|" + cntfx.ToString() + ",";
                            temp += "<br>**********************************************************<br>";
                        }
                    }
                }
                else
                {
                    strError = strError + reply.Notifications[0].Message.ToString() + ";";
                   // CommonComponent.ExecuteCommonData("INSERT INTO Bellacor_14Dec(Name) VALUES ('1: " + reply.Notifications[0].Message.ToString().Replace("'", "''") + "')");
                  //  CommonComponent.ExecuteCommonData("INSERT INTO Bellacor_14Dec(Name) VALUES ('2: " + reply.Notifications[0].LocalizedMessage.ToString().Replace("'", "''") + "')");
                  //  CommonComponent.ExecuteCommonData("INSERT INTO Bellacor_14Dec(Name) VALUES ('3: " + reply.Notifications[0].Source.ToString().Replace("'", "''") + "')");
                    try
                    {
                      //  CommonComponent.ExecuteCommonData("INSERT INTO Bellacor_14Dec(Name) VALUES ('1: " + reply.Notifications[1].Message.ToString().Replace("'", "''") + "')");
                      //  CommonComponent.ExecuteCommonData("INSERT INTO Bellacor_14Dec(Name) VALUES ('2: " + reply.Notifications[1].LocalizedMessage.ToString().Replace("'", "''") + "')");
                        
                    }
                    catch
                    {

                    }
                    if (m_FedExCount <= 3)
                    {
                        FedExGetRates(request2, out RTShipRequest2, out RTShipResponse2, isclient);
                    }
                }
            }
            catch (System.Web.Services.Protocols.SoapException e)
            {
                strError = strError + e.Message.ToString();
            }
            catch (Exception ex)
            { }
            #endregion

            if (retval.Length > 0)
                retval = "!" + retval.Substring(0, retval.Length - 1);

            if (retval != "")
                tmpS.Append(retval);
            else
                tmpS.Append(strError);

        }
        #endregion

        #region CurrencyStringForDBWithhourExchangeRate
        /// <summary>
        /// get the Currency String For DataBase With Hour ExchangeRate
        /// </summary>
        /// <param name="amt">Decimal Amount</param>
        /// <returns>return the Currency String For DataBase With Hour ExchangeRate</returns>
        public static String CurrencyStringForDBWithoutExchangeRate(decimal amt)
        {
            String tmpS = amt.ToString("C", USCulture);
            if (tmpS.StartsWith("("))
            {
                tmpS = "-" + tmpS.Replace("(", "").Replace(")", "");
            }
            return tmpS.Replace("$", "").Replace(",", "");
        }
        #endregion


        public Object FedexGetRates(decimal Weight, string Address, string Address2, string City, string State, string Zip, string Country, int CustomerID, bool isclient)
        {
            decimal TotalPrice = 0;
            Packages shipment = new Packages();

            ShipmentWeight = Convert.ToDecimal(AppLogic.AppConfigs("FedEx.MaxWeight"));
            shipment.DestinationZipPostalCode = Zip;
            shipment.DestinationCountryCode = Country;
            if (shipment.DestinationCountryCode == "US")
            {
                shipment.DestinationStateProvince = State;
            }

            shipment.DestinationResidenceType = ResidenceTypes.Residential;
            DestinationResidenceType = shipment.DestinationResidenceType;

            int PackageID = 1;

            Decimal FixedShipWeightRange = Decimal.Zero;
            Decimal FixedShipWeight = Decimal.Zero;

            if (FixedShipWeightRange == decimal.Zero)
                Decimal.TryParse(AppLogic.AppConfigs("FedEx.FixedShipWeightRange"), out FixedShipWeightRange);

            if (FixedShipWeight == decimal.Zero)
                Decimal.TryParse(AppLogic.AppConfigs("FedEx.FixedShippingWeight"), out FixedShipWeight);


            DataSet DsCartItems = CommonComponent.GetCommonDataSet("SELECT tb_Product.Name,tb_Product.SEName, " +
            " (Case tb_ShoppingCartItems.weight  when 0 then tb_Product.weight else tb_ShoppingCartItems.weight end) as Weight," +
            " isnull(tb_Product.isFreeShipping,0) as 'IsFreeShipping',tb_Product.SurCharge,tb_Product.SEName,tb_Product.SKU," +
            " tb_Product.Name + ISNull(Convert(nvarchar(max),SUBSTRING(tb_Product.Description,0,180)),'') as Description," +
            " tb_ShoppingCartItems.Price As SalePrice, tb_ShoppingCartItems.Quantity,tb_ShoppingCartItems.ProductID, " +
            " tb_ShoppingCartItems.ShoppingCartID, tb_Product.Name AS ProductName,tb_ShoppingCartItems.VariantNames," +
            " tb_ShoppingCartItems.VariantValues FROM tb_Product INNER JOIN tb_ShoppingCartItems ON " +
            " tb_Product.ProductID = tb_ShoppingCartItems.ProductID Where tb_Product.StoreID=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreID") + "" +
            " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustomerID + ")" +
            " And tb_Product.Weight < " + FixedShipWeightRange + " or tb_Product.Weight >" + FixedShipWeight + "");


            String Query = "SELECT sum(tb_ShoppingCartItems.Quantity) as TotalPackCount FROM tb_Product  " +
            " INNER JOIN tb_ShoppingCartItems ON tb_Product.ProductID = tb_ShoppingCartItems.ProductID   " +
            " Where tb_Product.StoreID=" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreID") + " And isnull(isFreeShipping,0) <> 1  " +
            " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustomerID + ")";

            Object objTotalPackCount = CommonComponent.GetScalarCommonData(Query);
            Int32 TotalPackCount = 0;
            if (objTotalPackCount != null)
                Int32.TryParse(objTotalPackCount.ToString(), out TotalPackCount);


            Decimal remainingItemsWeight = 0.0M;
            Decimal remainingItemsInsuranceValue = 0.0M;
            Decimal TotalWeight = 0;
            if (Weight != null)
                Decimal.TryParse(Weight.ToString(), out TotalWeight);
            int LoopCount = 1;

            FedExRate oFedEx = new FedExRate();

            //For Client
            oFedEx.oCity = AppLogic.AppConfigs("Shipping.OriginCity");
            oFedEx.oCountryCode = AppLogic.AppConfigs("Shipping.OriginCountry");
            oFedEx.oPostalCode = AppLogic.AppConfigs("Shipping.OriginZip");
            oFedEx.oStateOrProvinceCode = AppLogic.AppConfigs("Shipping.OriginState");
            oFedEx.oStreetLines = AppLogic.AppConfigs("Shipping.OriginAddress") + ", " + AppLogic.AppConfigs("Shipping.OriginAddress2");

            oFedEx.dCountryCode = Country;
            oFedEx.dPostalCode = Zip;
            if (Country.ToString().ToLower() != "us")
                oFedEx.dStateOrProvinceCode = "";
            else
                oFedEx.dStateOrProvinceCode = State;

            RateRequest request = oFedEx.Main();
            int pakcnt = 0;

            #region NewCode

            int LoopCountNew = 1;
            if (TotalWeight > ShipmentWeight)
            {
                LoopCountNew = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(TotalWeight / ShipmentWeight)));
                remainingItemsWeight = TotalWeight % ShipmentWeight;
            }
            else
            {
                remainingItemsWeight = 0.0M;
            }

            if (remainingItemsWeight > 0)
                TotalPackCount = LoopCountNew + 1;
            else TotalPackCount = LoopCount;

            #endregion


            if (TotalPackCount != 0)
                pakcnt = TotalPackCount;
            else
                pakcnt = DsCartItems.Tables[0].Rows.Count;

            request.RequestedShipment.PackageCount = pakcnt.ToString();
            request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[pakcnt];

            #region Make Packages
            PackageID = 1;
            int cnt = 0;

            foreach (DataRow gvr in DsCartItems.Tables[0].Rows)
            {

                if (gvr["isFreeShipping"].ToString() == "0")
                {

                    int i = 0;
                    int Quantity = 0;// Convert.ToInt32(gvr["Quantity"].ToString());
                    for (i = 0; i < Quantity; i++)
                    {
                        Decimal ProWeight = Convert.ToDecimal(gvr["Weight"].ToString());
                        Package p = new Package();
                        p.PackageId = PackageID;
                        PackageID += 1;


                        p.Weight = ProWeight;

                        remainingItemsInsuranceValue = (gvr["SalePrice"].ToString() != null && !string.IsNullOrEmpty(gvr["SalePrice"].ToString())) ? Convert.ToDecimal(gvr["SalePrice"].ToString()) : Decimal.Zero;
                        TotalPrice += remainingItemsInsuranceValue;

                        p.Insured = AppLogic.AppConfigBool("RTShipping.Insured");
                        p.InsuredValue = remainingItemsInsuranceValue;

                        #region FedEx Package
                        //request.RequestedShipment.RequestedPackageLineItems[cnt] = new RequestedPackageLineItem();
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].SequenceNumber = Convert.ToString(cnt + 1); // package sequence number
                        ////
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].Weight = new Weight(); // package weight
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Units = WeightUnits.LB;
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Value = p.Weight;
                        ////
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions = new Dimensions(); // package dimensions
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Length = p.Length.ToString();
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Width = p.Width.ToString();
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Height = p.Height.ToString();
                        //request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Units = LinearUnits.IN;
                        ////
                        //if (p.Insured)
                        //{
                        //    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue = new Money(); // insured value
                        //    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Amount = p.InsuredValue;
                        //    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Currency = currentytype;
                        //}
                        //cnt++;
                        #endregion
                        p = null;
                    }
                }
            }

            #region NewCode

            for (int iNew = 0; iNew < LoopCountNew; iNew++)
            {
                Package p = new Package();

                p.PackageId = PackageID;
                PackageID = PackageID + 1;
                if (TotalWeight > ShipmentWeight)
                {
                    p.Weight = ShipmentWeight;
                }
                else
                {
                    p.Weight = TotalWeight;
                }

                // Set insurance. Get from products db shipping values?
                p.Insured = AppLogic.AppConfigBool("RTShipping.Insured"); //false;
                p.InsuredValue = remainingItemsInsuranceValue;

                #region FedEx Package

                request.RequestedShipment.RequestedPackageLineItems[cnt] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[cnt].SequenceNumber = Convert.ToString(cnt + 1); // package sequence number
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight = new Weight(); // package weight
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Units = WeightUnits.LB;
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Value = p.Weight;
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions = new Dimensions(); // package dimensions
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Length = p.Length.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Width = p.Width.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Height = p.Height.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Units = LinearUnits.IN;
                if (p.Insured)
                {
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue = new Money(); // insured value
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Amount = p.InsuredValue;
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Currency = "USD";
                }
                cnt++;

                #endregion

                p = null;
            }

            if (remainingItemsWeight != 0.0M)
            {
                // Create package object for this item
                Package p = new Package();
                p.PackageId = PackageID;
                PackageID = PackageID + 1;
                p.Weight = remainingItemsWeight;

                // Set insurance. Get from products db shipping values?
                p.Insured = AppLogic.AppConfigBool("FedEx.Insured"); //false;
                p.InsuredValue = remainingItemsInsuranceValue;

                #region FedEx Package

                request.RequestedShipment.RequestedPackageLineItems[cnt] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[cnt].SequenceNumber = Convert.ToString(cnt + 1); // package sequence number
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight = new Weight(); // package weight
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Units = WeightUnits.LB;
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Value = p.Weight;
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions = new Dimensions(); // package dimensions
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Length = p.Length.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Width = p.Width.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Height = p.Height.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Units = LinearUnits.IN;
                if (p.Insured)
                {
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue = new Money(); // insured value
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Amount = p.InsuredValue;
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Currency = "USD";
                }
                cnt++;

                #endregion

                p = null;
            }

            #endregion


            #endregion

            StringBuilder tmpS = new StringBuilder(4096);
            String RTShipRequest = String.Empty;
            String RTShipResponse = String.Empty;
            object returnObject = null;

            tmpS.Append((string)GetRates(shipment, out RTShipRequest, out RTShipResponse, request, isclient));
            returnObject = (object)tmpS;

            return returnObject;
        }


        public Object FedexGetRatesAdmin(int WareHouseID, decimal Weight, string Address, string Address2, string City, string State, string Zip, string Country, int CustomerID, bool isclient)
        {
            Packages shipment = new Packages();
            StateComponent objstate = new StateComponent();
            State = objstate.GetStateCodeByName(State);
            ShipmentWeight = Convert.ToDecimal(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.MaxWeight"));
            shipment.DestinationZipPostalCode = Zip;
            shipment.DestinationCountryCode = Country;
            if (shipment.DestinationCountryCode == "US")
            {
                shipment.DestinationStateProvince = State;
            }

            shipment.DestinationResidenceType = ResidenceTypes.Residential;
            DestinationResidenceType = shipment.DestinationResidenceType;

            int PackageID = 1;

            Decimal FixedShipWeightRange = Decimal.Zero;
            Decimal FixedShipWeight = Decimal.Zero;

            if (FixedShipWeightRange == decimal.Zero)
                Decimal.TryParse(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.FixedShipWeightRange"), out FixedShipWeightRange);

            if (FixedShipWeight == decimal.Zero)
                Decimal.TryParse(Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("FedEx.FixedShippingWeight"), out FixedShipWeight);

            DataSet DsCartItems = CommonComponent.GetCommonDataSet("SELECT tb_Product.Name,tb_Product.SEName, " +
            " (Case tb_ShoppingCartItems.weight  when 0 then tb_Product.weight else tb_ShoppingCartItems.weight end) as Weight," +
            " isnull(tb_Product.isFreeShipping,0) as 'IsFreeShipping',tb_Product.SurCharge,tb_Product.SEName,tb_Product.SKU," +
            " tb_Product.Name + ISNull(Convert(nvarchar(max),SUBSTRING(tb_Product.Description,0,180)),'') as Description," +
            " tb_ShoppingCartItems.Price As SalePrice, tb_ShoppingCartItems.Quantity,tb_ShoppingCartItems.ProductID, " +
            " tb_ShoppingCartItems.ShoppingCartID, tb_Product.Name AS ProductName,tb_ShoppingCartItems.VariantNames," +
            " tb_ShoppingCartItems.VariantValues FROM tb_Product INNER JOIN tb_ShoppingCartItems ON " +
            " tb_Product.ProductID = tb_ShoppingCartItems.ProductID Where tb_Product.StoreID=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + "" +
            " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustomerID + ")" +
            " And tb_Product.Weight < " + FixedShipWeightRange + " or tb_Product.Weight >" + FixedShipWeight + "");

            String Query = "SELECT sum(tb_ShoppingCartItems.Quantity) as TotalPackCount FROM tb_Product  " +
            " INNER JOIN tb_ShoppingCartItems ON tb_Product.ProductID = tb_ShoppingCartItems.ProductID   " +
            " Where tb_Product.StoreID=" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + " And isnull(isFreeShipping,0) <> 1  " +
            " And ShoppingCartID In (Select ShoppingCartID From tb_ShoppingCart Where CustomerID=" + CustomerID + ")";

            Object objTotalPackCount = CommonComponent.GetScalarCommonData(Query);
            Int32 TotalPackCount = 0;
            if (objTotalPackCount != null)
                Int32.TryParse(objTotalPackCount.ToString(), out TotalPackCount);

            //Weight
            Decimal remainingItemsWeight = 0.0M;
            Decimal remainingItemsInsuranceValue = 0.0M;
            Decimal TotalWeight = 0;
            if (Weight != null)
                Decimal.TryParse(Weight.ToString(), out TotalWeight);
            int LoopCount = 1;
            if (TotalWeight > ShipmentWeight)
            {
                LoopCount = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(TotalWeight / ShipmentWeight)));
                remainingItemsWeight = TotalWeight % ShipmentWeight;
            }
            else
            {
                remainingItemsWeight = 0.0M;
            }

            FedExRateAdmin oFedEx = new FedExRateAdmin();

            //For Client
            if (WareHouseID > 0)
            {
                string OrgShippingZip = "";
                string OrgCountry = "";
                string OrgAddress1 = "";
                string OrgAddress2 = "";
                string OrgCity = "";
                string OrgState = "";

                string sqlWareHouse = "SELECT Address1,Address2,City,State,ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse "
                    + " INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID;
                DataSet dsWareHouse = CommonComponent.GetCommonDataSet(sqlWareHouse);
                if (dsWareHouse != null && dsWareHouse.Tables.Count > 0 && dsWareHouse.Tables[0].Rows.Count > 0)
                {
                    OrgShippingZip = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["ZipCode"]);
                    OrgCountry = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["countryName"]);
                    OrgAddress1 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address1"]);
                    OrgAddress2 = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["Address2"]);
                    OrgCity = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["City"]);
                    OrgState = Convert.ToString(dsWareHouse.Tables[0].Rows[0]["State"]);
                }
                CountryComponent objCountry = new CountryComponent();
                OrgCountry = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(OrgCountry));
                StateComponent objState = new StateComponent();
                OrgState = Convert.ToString(objState.GetStateCodeByName(OrgState));

                oFedEx.oCity = OrgCity;
                oFedEx.oCountryCode = OrgCountry;
                oFedEx.oPostalCode = OrgShippingZip;
                oFedEx.oStateOrProvinceCode = OrgState;
                oFedEx.oStreetLines = OrgAddress1 + ", " + OrgAddress2;
            }
            else
            {
                oFedEx.oCity = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginCity");
                oFedEx.oCountryCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginCountry");
                oFedEx.oPostalCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginZip");
                oFedEx.oStateOrProvinceCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginState");
                oFedEx.oStreetLines = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginAddress") + ", " + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginAddress2");
            }

            oFedEx.dCountryCode = Country;
            oFedEx.dPostalCode = Zip;
            if (Country.ToString().ToLower() != "us")
                oFedEx.dStateOrProvinceCode = "";
            else
                oFedEx.dStateOrProvinceCode = State;


            RateRequest request = oFedEx.Main();
            int pakcnt = 0;

            #region NewCode

            int LoopCountNew = 1;
            if (TotalWeight > ShipmentWeight)
            {
                LoopCountNew = Convert.ToInt32(Math.Truncate(Convert.ToDecimal(TotalWeight / ShipmentWeight)));
                remainingItemsWeight = TotalWeight % ShipmentWeight;
            }
            else
            {
                remainingItemsWeight = 0.0M;
            }

            if (remainingItemsWeight > 0)
                TotalPackCount = LoopCount + 1;
            else TotalPackCount = LoopCount;

            #endregion

            if (TotalPackCount != 0)
                pakcnt = TotalPackCount;
            else
                pakcnt = DsCartItems.Tables[0].Rows.Count;

            request.RequestedShipment.PackageCount = pakcnt.ToString();
            request.RequestedShipment.RequestedPackageLineItems = new RequestedPackageLineItem[pakcnt];

            #region Make Packages

            PackageID = 1;
            int cnt = 0;

            #region NewCode

            for (int iNew = 0; iNew < LoopCountNew; iNew++)
            {
                Package p = new Package();
                p.PackageId = PackageID;
                PackageID = PackageID + 1;
                if (TotalWeight > ShipmentWeight)
                {
                    p.Weight = ShipmentWeight;
                }
                else
                {
                    p.Weight = TotalWeight;
                }

                // Set insurance. Get from products db shipping values?
                p.Insured = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigBool("RTShipping.Insured"); //false;
                p.InsuredValue = remainingItemsInsuranceValue;

                #region FedEx Package

                request.RequestedShipment.RequestedPackageLineItems[cnt] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[cnt].SequenceNumber = Convert.ToString(cnt + 1); // package sequence number
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight = new Weight(); // package weight
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Units = WeightUnits.LB;
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Value = p.Weight;
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions = new Dimensions(); // package dimensions
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Length = p.Length.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Width = p.Width.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Height = p.Height.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Units = LinearUnits.IN;
                if (p.Insured)
                {
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue = new Money(); // insured value
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Amount = p.InsuredValue;
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Currency = "USD";
                }
                cnt++;

                #endregion

                p = null;
            }

            if (remainingItemsWeight != 0.0M)
            {
                Package p = new Package();
                p.PackageId = PackageID;
                PackageID = PackageID + 1;
                p.Weight = remainingItemsWeight;

                // Set insurance. Get from products db shipping values?
                p.Insured = AppLogic.AppConfigBool("FedEx.Insured"); //false;
                p.InsuredValue = remainingItemsInsuranceValue;

                #region FedEx Package

                request.RequestedShipment.RequestedPackageLineItems[cnt] = new RequestedPackageLineItem();
                request.RequestedShipment.RequestedPackageLineItems[cnt].SequenceNumber = Convert.ToString(cnt + 1); // package sequence number
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight = new Weight(); // package weight
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Units = WeightUnits.LB;
                request.RequestedShipment.RequestedPackageLineItems[cnt].Weight.Value = p.Weight;
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions = new Dimensions(); // package dimensions
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Length = p.Length.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Width = p.Width.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Height = p.Height.ToString();
                request.RequestedShipment.RequestedPackageLineItems[cnt].Dimensions.Units = LinearUnits.IN;
                if (p.Insured)
                {
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue = new Money(); // insured value
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Amount = p.InsuredValue;
                    request.RequestedShipment.RequestedPackageLineItems[cnt].InsuredValue.Currency = "USD";
                }
                cnt++;

                #endregion
                p = null;
            }

            #endregion

            #endregion

            StringBuilder tmpS = new StringBuilder(4096);
            String RTShipRequest = String.Empty;
            String RTShipResponse = String.Empty;
            object returnObject = null;

            tmpS.Append((string)GetRates(shipment, out RTShipRequest, out RTShipResponse, request, isclient));
            returnObject = (object)tmpS;

            return returnObject;
        }


        public object GetRates(Packages Shipment, out string RTShipRequest, out string RTShipResponse, RateRequest request, bool isclient)
        {
            // Get all carriers to retrieve rates for
            RTShipRequest = String.Empty;
            RTShipResponse = String.Empty;
            try
            {
                FedExGetRates(request, out RTShipRequest, out RTShipResponse, isclient);
            }
            catch (Exception ex)
            {
                RTShipResponse = "CustomException: " + ex.Message;
            }
            // Check list format type, and setup appropriate 
            StringBuilder output = new StringBuilder(1024);
            object returnObject = null;
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
    }
}
