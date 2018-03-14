using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using ShipWSSample;
using System.IO;
using Solution.ShippingMethods.WebReference;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;

/// <summary>
/// Summary description for AccesslabelUPS
/// </summary>
public class AccesslabelUPS
{
    public AccesslabelUPS()
    {
        //
        // TODO: Add constructor logic here
        //
    }
    public static string Main(string[] args, string OrderNo, string ShippingMethod, bool IsTestMode, string ImgSavePath, DataTable myProductTable, bool deliveryNotification, int WareHouseID)
    {
        /* Register an account(it free) with UPS to abtain xmlAccessCode,username,password and accountnumber */
        string xmlAccessCode = args[0];
        string username = args[1];
        string password = args[2];
        string accountNumber = args[3];
        string ProductID = "";
        string RtnMessage = "";

        #region Comment code
        ///* Get Shipping rates*/
        //ShippingUPSL.Rate rate = new ShippingUPSL.Rate(xmlAccessCode, username, password);
        //rate.TestMode = true;
        //rate.FromAddress("03", "city", "CA", "92840", "US");
        //rate.ToAddress("Washington", "DC", "20559", "US", true);
        //rate.PackageInfo(1);
        //rate.ProcessRating().Report();
        //Console.ReadLine();
        ////return;

        ///* Valid date Address test*/
        //ShippingUPSL.USAddressValidate av = new ShippingUPSL.USAddressValidate(xmlAccessCode, username, password);
        //av.TestMode = true;
        //if (av.IsValid("Garden Grove", "CA", "92840"))
        //{
        //    int a = 0;
        //}
        ////return;

        ///*  Tracking Status
        // *  Tracking number needed to test
        // */

        //ShippingUPSL.UPSTracking tracking = new ShippingUPSL.UPSTracking(xmlAccessCode, username, password);
        //tracking.TestMode = true;
        //string trackingNumber = "", error = "";
        //if (tracking.Tracking("123442314", "", out trackingNumber, out error))
        //{
        //    Console.WriteLine("Good: " + trackingNumber);
        //}
        //else
        //    Console.WriteLine("Bad: " + error);
        //Console.ReadLine();
        //return;
        //us.ShipperInfo("vdap.com", "Nhat Ho", "714-636-8164", accountNumber, "12331 Ninth street", "", "Garden Grove", "CA", "92840", "US");
        //us.ShipFrom("vdap.com", "Nhat Ho", "714-636-8164", "12331 Ninth street", "", "Garden Grove", "CA", "92840", "US");
        //us.ShipTo("An mai", "An", "714-636-8164", "851 Redondo dr west", "", "Anahiem", "CA", "92801", "US");
        //us.PaymentInformation(accountNumber, "03");
        //us.AddEmailNotification("Nhatminhho@gmail.com", "test");
        //us.AddPackage("asdf", "02", 2, true, 130, "USD", "9V", "asdfads");
        //us.AddPackage("asdf2", "02", 4, true, 130, "USD", "9V", "asdfads");
        #endregion

        //Org Address

        string OrgShippingZip = "";
        string OrgCountry = "";
        string OrgAddress1 = "";
        string OrgAddress2 = "";
        string OrgCity = "";
        string OrgState = "";

        string sqlWareHouse = "SELECT Address1,Address2,City,State,ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID;
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




        /* Shipping confirm and Shipping Accept test*/

        string Result = "";
        string sql = string.Empty;
        DataSet ds = CommonComponent.GetCommonDataSet("select * from View_OrderDetails where OrderNumber=" + OrderNo.ToString());
        string totalImg = string.Empty;
        string ShoppingCartID = string.Empty;
        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            #region GetData
            string ErrString = string.Empty;
            string ShippingCompany = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingCompany"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingCompany"].ToString());
            string ShippingFirstName = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingFirstName"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingLastName"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingLastName"].ToString());
            string ShippingPhone = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingPhone"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingPhone"].ToString());
            string ShippingAddress1 = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingAddress1"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingAddress1"].ToString());
            string ShippingAddress2 = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingAddress2"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingAddress2"].ToString());
            string ShippingSuite = ds.Tables[0].Rows[0]["ShippingSuite"].ToString();
            string ShippingCity = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingCity"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingCity"].ToString());
            string ShippingState = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingState"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingState"].ToString());
            string ShippingZip = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingZip"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingZip"].ToString());
            string TwoLetterISOCode = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingCountry"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingCountry"].ToString());
            TwoLetterISOCode = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(TwoLetterISOCode));


            if (string.IsNullOrEmpty(ShippingFirstName))
                ErrString = "Name of the recipient is not available.";

            if (string.IsNullOrEmpty(ShippingCompany))
            {
                ShippingCompany = ShippingFirstName;
                ShippingFirstName = "";
            }

            if (string.IsNullOrEmpty(ShippingPhone))
                ErrString = "Phone Number of the recipient is not available or Invalid.";

            if (string.IsNullOrEmpty(ShippingAddress1))
                ErrString = "Shipping Address of the recipient is not available.";

            if (string.IsNullOrEmpty(ShippingCity))
                ErrString = "City name of the recipient is not available.";

            if (string.IsNullOrEmpty(ShippingState))
                ErrString = "State name of the recipient is not available.";

            if (string.IsNullOrEmpty(ShippingZip))
                ErrString = "Zip Code of the recipient is not available or Invalid";

            if (string.IsNullOrEmpty(TwoLetterISOCode))
                ErrString = "Country name is not Available.";

            if (!string.IsNullOrEmpty(ErrString))
            {
                ErrString = "Recipient address error: " + ErrString;
                return ErrString;

            }

            if (!string.IsNullOrEmpty(ShippingSuite))
            {
                if (!ShippingSuite.ToLower().StartsWith("suite"))
                {
                    ShippingSuite = "Suite #" + ShippingSuite.TrimStart('#');
                }
                ShippingAddress1 += "," + ShippingSuite;
            }

            #endregion
            ShippingUPSL.UPSShipping us = new ShippingUPSL.UPSShipping(xmlAccessCode, username, password);
            //return "";
            if (OrgCountry == TwoLetterISOCode)
            {
                us.TestMode = IsTestMode;
                us.ShipperInfo(AppLogic.AppConfigs("Shipping.CompanyName"), AppLogic.AppConfigs("Shipping.OriginContactName"), AppLogic.AppConfigs("Shipping.OriginPhone"), accountNumber, OrgAddress1, OrgAddress2, OrgCity, OrgState, OrgShippingZip, OrgCountry);
                //us.ShipperInfo("vdap.com", "Nhat Ho", "714-636-8164", accountNumber, "12331 Ninth street", "", "Garden Grove", "CA", "92840", "US");
                us.ShipFrom(AppLogic.AppConfigs("Shipping.CompanyName"), AppLogic.AppConfigs("Shipping.OriginContactName"), AppLogic.AppConfigs("Shipping.OriginPhone"), OrgAddress1, OrgAddress2, OrgCity, OrgState, OrgShippingZip, OrgCountry);
                //us.ShipFrom("vdap.com", "Nhat Ho", "714-636-8164", "12331 Ninth street", "", "Garden Grove", "CA", "92840", "US");

                if (ShippingCompany.Length > 35)
                    ShippingCompany = ShippingCompany.Substring(0, 35);
                if (ShippingFirstName.Length > 35)
                    ShippingFirstName = ShippingFirstName.Substring(0, 35);
                if (ShippingAddress1.Length > 35)
                    ShippingAddress1 = ShippingAddress1.Substring(0, 35);
                if (ShippingAddress2.Length > 35)
                    ShippingAddress2 = ShippingAddress2.Substring(0, 35);
                if (ShippingCity.Length > 30)
                    ShippingCity = ShippingCity.Substring(0, 30);


                // clsCountry objcountry = new clsCountry();

                //string Abbreviation = objcountry.GetAbbreviationByStateName(ShippingState);

                // StateComponent objState = new StateComponent();
                string Abbreviation = Convert.ToString(objState.GetStateCodeByName(ShippingState));


                //State = ((string.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippingState"].ToString())) ? "" : DsOrder.Tables[0].Rows[0]["ShippingState"].ToString());
                ShippingState = string.IsNullOrEmpty(Abbreviation) ? ShippingState : Abbreviation;

                us.ShipTo(ShippingCompany, ShippingFirstName, ShippingPhone, ShippingAddress1, ShippingAddress2, ShippingCity, ShippingState, ShippingZip, TwoLetterISOCode); //close
                //us.ShipTo("An mai", "An", "714-636-8164", "851 Redondo dr west", "", "Anahiem", "CA", "92801", "US"); //close
                us.PaymentInformation(accountNumber, ShippingMethod);
                //us.AddPackage("asdf", "02", 2, true, 130, "USD", "9V", "asdfads", deliveryNotification, true);
                //us.AddPackage("asdf2", "02", 4, true, 130, "USD", "9V", "asdfads", deliveryNotification, true);

                if (myProductTable != null && myProductTable.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < myProductTable.Rows.Count; cnt++)
                    {
                        string ProductName = myProductTable.Rows[cnt]["ProductName"].ToString();
                        string ProductWeight = myProductTable.Rows[cnt]["Weight"].ToString();
                        string ProductPrice = myProductTable.Rows[cnt]["Price"].ToString();
                        ShoppingCartID += myProductTable.Rows[cnt]["ShippingCartID"] + "|" + myProductTable.Rows[cnt]["PackageID"] + ",";

                        ProductID += myProductTable.Rows[cnt]["ProductID"] + "|";

                        Double ProductW = 1.00;
                        if (!string.IsNullOrEmpty(ProductWeight))
                            ProductW = Convert.ToDouble(ProductWeight);

                        Double ProductP = 1.00;
                        if (!string.IsNullOrEmpty(ProductPrice))
                            ProductP = Convert.ToDouble(ProductPrice);

                        bool isInsured = Convert.ToBoolean(myProductTable.Rows[cnt]["IsInsured"]);

                        us.AddPackage(ProductName, "02", ProductW, true, ProductP, "USD", "", "", deliveryNotification, isInsured);
                    }
                }

                #region test
                //us = new ShippingUPSL.UPSShipping(xmlAccessCode, username, password);
                //us.TestMode = IsTestMode;
                //if (IsTestMode)
                //{
                //    us.ShipperInfo("vdap.com", "Nhat Ho", "714-636-8164", accountNumber, "12331 Ninth street", "", "Garden Grove", "CA", "92840", "US");
                //    us.ShipFrom("vdap.com", "Nhat Ho", "714-636-8164", "12331 Ninth street", "", "Garden Grove", "CA", "92840", "US");
                //    us.ShipTo("An mai", "An", "714-636-8164", "851 Redondo dr west", "", "Anahiem", "CA", "92801", "US");
                //    us.PaymentInformation(accountNumber, "03");
                //    us.AddEmailNotification("Nhatminhho@gmail.com", "test");
                //    us.AddPackage("asdf", "02", 2, true, 130, "USD", "9V", "asdfads", deliveryNotification, true);
                //    us.AddPackage("asdf2", "02", 4, true, 130, "USD", "9V", "asdfads", deliveryNotification, true);
                //}
                #endregion

                ShippingUPSL.ShippingConfirmResult rs = us.ProcessShipping();
                Result += "##Success Login#" + rs.Success + "##Status#" + rs.StatusMessage;
                string sd = rs.ShipmentDigest;

                System.Xml.XmlNode resultNode = us.xmlRespone.GetElementsByTagName("ResponseStatusCode").Item(0);
                string Status = string.Empty;
                try
                {

                    string xml = us.xmlRespone.InnerXml;
                    if (xml.Contains("<Error>"))
                    {
                        int idx1 = xml.IndexOf("<ErrorDescription>") + 18;
                        int idx2 = xml.IndexOf("</ErrorDescription>");
                        int l = xml.Length;

                        Status = xml.Substring(idx1, idx2 - idx1);
                        //throw new USPSManagerException(errDesc);
                    }

                }
                catch { }
                if (!string.IsNullOrEmpty(Status) && resultNode.InnerText != "1")
                    return "XML Error response: " + Status;



                /*Pass the ShipmentDigest number to get label*/
                ShippingUPSL.ShippingAcceptResult scr = us.ShippingAccept(sd);
                int i = 0;
                Result += scr.Success + ", " + scr.StatusMessage + "," + scr.ShipmentIdentificationNumber + ", " + scr.Packages.Length;
                string ImgName = string.Empty;
                Result += "##LabelFetch Login#" + scr.Success + "##LabelFetch Status#" + scr.StatusMessage;

                string[] strParts = ShoppingCartID.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string[] strProductID = ProductID.Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (ShippingUPSL.ShippingAcceptPackage sp in scr.Packages)
                {

                    #region Create Image Name
                    string[] strIDs = { "" };
                    string strPIDs = "";
                    if (strParts.Length > i)
                        strIDs = strParts[i].Split('|');//shoppingcartRecID|PackageID
                    if (strProductID.Length > i)
                        strPIDs = strProductID[i].ToString();

                    i++;
                    string filename = "UPS-Package" + ((strIDs.Length > 1) ? strIDs[1] : i.ToString()) + "_" + sp.TrackingNumber + "_" + OrderNo + "_" + ((strIDs.Length > 0) ? strIDs[0] : i.ToString()) + "@" +
                              DateTime.Now.Year.ToString() +
                              DateTime.Now.Month.ToString() +
                              DateTime.Now.Day.ToString() +
                              DateTime.Now.Hour.ToString() +
                              DateTime.Now.Minute.ToString() +
                              DateTime.Now.Second.ToString() + "-" +
                              i.ToString() + ".gif";
                    ImgName = ImgSavePath + filename; // Path of the Label Image
                    #endregion

                    totalImg += filename + "#";
                    try
                    {
                        sp.SaveLabelInGifFile(ImgName);

                        ShippingComponent.UpdateorderedShoppingcartitems(sp.TrackingNumber, strPIDs, "UPS", Convert.ToInt32(strIDs[0]), 1, WareHouseID, ((strIDs.Length > 1) ? Convert.ToInt32(strIDs[1]) :Convert.ToInt32( i.ToString())));
                    }
                    catch { }

                }
                RtnMessage = Result + "||" + totalImg + "||" + ShoppingCartID;
            }
            else
            {

                RtnMessage = GenerateInternationalLabel(args, OrderNo, ShippingMethod, IsTestMode, ImgSavePath, myProductTable, deliveryNotification, WareHouseID);


            }
        }

        return RtnMessage; ;
    }

    public static bool UpdatCartItem(string TrackingNumber, string ProductId, string CourierName, int OrderedShoppingCartID, int ShippedQty,int WareHouseID,int Packid)
    {
        return Convert.ToBoolean(ShippingComponent.UpdateorderedShoppingcartitems(TrackingNumber, ProductId, CourierName, OrderedShoppingCartID, ShippedQty, WareHouseID, Packid));
    }
    public static string GenerateInternationalLabel(string[] args, string OrderNo, string ShippingMethod, bool IsTestMode, string ImgSavePath, DataTable myProductTable, bool deliveryNotification, int WareHouseID)
    {
        string ReturnMessage = "";
        string xmlAccessCode = args[0];
        string username = args[1];
        string password = args[2];
        string accountNumber = args[3];
        /* Shipping confirm and Shipping Accept test*/

        string Result = "";
        string sql = string.Empty;

        string OrgShippingZip = "";
        string OrgCountry = "";
        string OrgAddress1 = "";
        string OrgAddress2 = "";
        string OrgCity = "";
        string OrgState = "";

        string sqlWareHouse = "SELECT Address1,Address2,City,State,ZipCode,tb_Country.Name AS countryName FROM dbo.tb_WareHouse  INNER JOIN dbo.tb_Country ON tb_WareHouse.Country=tb_Country.CountryID WHERE tb_WareHouse.WareHouseID=" + WareHouseID;
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


        DataSet ds = CommonComponent.GetCommonDataSet("select * from View_OrderDetails where OrderNumber=" + OrderNo.ToString());

        string totalImg = string.Empty;
        string ShoppingCartID = string.Empty;

        try
        {

            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                #region GetData
                string ErrString = string.Empty;
                string ShippingCompany = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingCompany"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingCompany"].ToString());
                string ShippingFirstName = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingFirstName"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingLastName"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingLastName"].ToString());
                string ShippingPhone = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingPhone"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingPhone"].ToString());
                string ShippingAddress1 = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingAddress1"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingAddress1"].ToString());
                string ShippingAddress2 = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingAddress2"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingAddress2"].ToString());
                string ShippingSuite = ds.Tables[0].Rows[0]["ShippingSuite"].ToString();
                string ShippingCity = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingCity"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingCity"].ToString());
                string ShippingState = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingState"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingState"].ToString());
                string ShippingZip = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingZip"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingZip"].ToString());
                string TwoLetterISOCode = ((string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShippingCountry"].ToString())) ? "" : ds.Tables[0].Rows[0]["ShippingCountry"].ToString());
                TwoLetterISOCode = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(TwoLetterISOCode));


                if (string.IsNullOrEmpty(ShippingFirstName))
                    ErrString = "Name of the recipient is not available.";

                if (string.IsNullOrEmpty(ShippingCompany))
                {
                    ShippingCompany = ShippingFirstName;
                    ShippingFirstName = "";
                }

                if (string.IsNullOrEmpty(ShippingPhone))
                    ErrString = "Phone Number of the recipient is not available or Invalid.";

                if (string.IsNullOrEmpty(ShippingAddress1))
                    ErrString = "Shipping Address of the recipient is not available.";

                if (string.IsNullOrEmpty(ShippingCity))
                    ErrString = "City name of the recipient is not available.";

                if (string.IsNullOrEmpty(ShippingState))
                    ErrString = "State name of the recipient is not available.";

                if (string.IsNullOrEmpty(ShippingZip))
                    ErrString = "Zip Code of the recipient is not available or Invalid";

                if (string.IsNullOrEmpty(TwoLetterISOCode))
                    ErrString = "Country name is not Available.";

                if (!string.IsNullOrEmpty(ErrString))
                {
                    ErrString = "Recipient address error: " + ErrString;
                    return ErrString;

                }

                if (!string.IsNullOrEmpty(ShippingSuite))
                {
                    if (!ShippingSuite.ToLower().StartsWith("suite"))
                    {
                        ShippingSuite = "Suite #" + ShippingSuite.TrimStart('#');
                    }
                    ShippingAddress1 += "," + ShippingSuite;
                }


                if (ShippingCompany.Length > 35)
                    ShippingCompany = ShippingCompany.Substring(0, 35);
                if (ShippingFirstName.Length > 35)
                    ShippingFirstName = ShippingFirstName.Substring(0, 35);
                if (ShippingAddress1.Length > 35)
                    ShippingAddress1 = ShippingAddress1.Substring(0, 35);
                if (ShippingAddress2.Length > 35)
                    ShippingAddress2 = ShippingAddress2.Substring(0, 35);
                if (ShippingCity.Length > 30)
                    ShippingCity = ShippingCity.Substring(0, 30);

                #endregion
                ////Variable Declare
                ShipService shpSvc = new ShipService();
                ShipmentRequest shipmentRequest = new ShipmentRequest();
                UPSSecurity upss = new UPSSecurity();

                //User Information
                UPSSecurityServiceAccessToken upssSvcAccessToken = new UPSSecurityServiceAccessToken();
                upssSvcAccessToken.AccessLicenseNumber = xmlAccessCode;
                upss.ServiceAccessToken = upssSvcAccessToken;
                UPSSecurityUsernameToken upssUsrNameToken = new UPSSecurityUsernameToken();
                upssUsrNameToken.Username = username;
                upssUsrNameToken.Password = password;
                upss.UsernameToken = upssUsrNameToken;
                shpSvc.UPSSecurityValue = upss;
                ///////////////////////////////////////////////

                RequestType request = new RequestType();
                String[] requestOption = { "nonvalidate" };
                request.RequestOption = requestOption;
                shipmentRequest.Request = request;
                ShipmentType shipment = new ShipmentType();
                shipment.Description = AppLogic.AppConfigs("Shipping.OriginContactName").ToString();
                ShipperType shipper = new ShipperType();
                shipper.ShipperNumber = accountNumber;
                ///////////////////////////////////////////////////////////
                //************Payment Information**************************
                Solution.ShippingMethods.WebReference.PaymentInfoType paymentInfo = new Solution.ShippingMethods.WebReference.PaymentInfoType();
                ShipmentChargeType shpmentCharge = new ShipmentChargeType();
                BillShipperType billShipper = new BillShipperType();
                billShipper.AccountNumber = accountNumber;
                shpmentCharge.BillShipper = billShipper;
                shpmentCharge.Type = "01";
                ShipmentChargeType[] shpmentChargeArray = { shpmentCharge };
                paymentInfo.ShipmentCharge = shpmentChargeArray;
                shipment.PaymentInformation = paymentInfo;
                //********************************************************************

                //***************Shipper Address***************************************************************************************

                ShipAddressType shipperAddress = new ShipAddressType();
                String[] addressLine = { OrgAddress1 };
                shipperAddress.AddressLine = addressLine;
                shipperAddress.City = OrgCity;
                shipperAddress.PostalCode = OrgShippingZip;
                shipperAddress.StateProvinceCode = OrgState;
                shipperAddress.CountryCode = OrgCountry;
                shipperAddress.AddressLine = addressLine;
                shipper.Address = shipperAddress;


                //shipper.Name = AppLogic.AppConfigs("RTShipping.OriginContactName").ToString();
                //shipper.AttentionName = AppLogic.AppConfigs("RTShipping.CompanyName").ToString();


                if (AppLogic.AppConfigs("Shipping.CompanyName").ToString() != "")
                    shipper.Name = AppLogic.AppConfigs("Shipping.CompanyName").ToString();
                else
                    shipper.Name = AppLogic.AppConfigs("Shipping.OriginContactName").ToString();

                if (AppLogic.AppConfigs("Shipping.OriginContactName").ToString() != "")

                    shipper.AttentionName = AppLogic.AppConfigs("Shipping.OriginContactName").ToString();
                else
                    shipper.AttentionName = AppLogic.AppConfigs("Shipping.CompanyName").ToString();

                ShipPhoneType shipperPhone = new ShipPhoneType();
                shipperPhone.Number = AppLogic.AppConfigs("Shipping.OriginPhone").ToString();
                shipper.Phone = shipperPhone;
                shipment.Shipper = shipper;
                ShipFromType shipFrom = new ShipFromType();

                //*************************************************From Address*************************************************

                ShipAddressType shipFromAddress = new ShipAddressType();
                String[] shipFromAddressLine = { OrgAddress1 };
                shipFromAddress.AddressLine = addressLine;
                shipFromAddress.City = OrgCity;
                shipFromAddress.PostalCode = OrgShippingZip;
                shipFromAddress.StateProvinceCode = OrgState;
                shipFromAddress.CountryCode = OrgCountry;
                shipFrom.Address = shipFromAddress;


                if (AppLogic.AppConfigs("Shipping.CompanyName").ToString() != "")
                    shipFrom.AttentionName = AppLogic.AppConfigs("Shipping.CompanyName").ToString();
                else
                    shipFrom.AttentionName = AppLogic.AppConfigs("Shipping.OriginContactName").ToString();

                if (AppLogic.AppConfigs("Shipping.OriginContactName").ToString() != "")
                    shipFrom.Name = AppLogic.AppConfigs("Shipping.OriginContactName").ToString();
                else
                    shipFrom.Name = AppLogic.AppConfigs("Shipping.CompanyName").ToString();


                //shipFrom.AttentionName = AppLogic.AppConfigs("RTShipping.OriginContactName").ToString();
                //shipFrom.Name = AppLogic.AppConfigs("RTShipping.CompanyName").ToString();


                shipment.ShipFrom = shipFrom;

                //***************************************Ship To*****************************************************************

                //clsCountry objcountry = new clsCountry();
                //string Abbreviation = objcountry.GetAbbreviationByStateName(ShippingState);

                //   StateComponent objState = new StateComponent();
                string Abbreviation = Convert.ToString(objState.GetStateCodeByName(ShippingState));


                ShipToType shipTo = new ShipToType();
                ShipToAddressType shipToAddress = new ShipToAddressType();
                String[] addressLine1 = { ShippingAddress1 };
                shipToAddress.AddressLine = addressLine1;
                shipToAddress.City = ShippingCity;
                shipToAddress.PostalCode = ShippingZip;
                shipToAddress.StateProvinceCode = Abbreviation;
                shipToAddress.CountryCode = TwoLetterISOCode;
                shipTo.Address = shipToAddress;

                if (ShippingCompany != "")
                    shipTo.AttentionName = ShippingCompany;
                else
                    shipTo.AttentionName = ShippingFirstName;

                if (ShippingFirstName != "")
                    shipTo.Name = ShippingFirstName;
                else
                    shipTo.Name = ShippingCompany;

                ShipPhoneType shipToPhone = new ShipPhoneType();
                shipToPhone.Number = ShippingPhone;
                shipTo.Phone = shipToPhone;
                shipment.ShipTo = shipTo;

                //***********************************************Servies*****************************************************************
                ServiceType service = new ServiceType();
                // service.Code = "01";
                service.Code = ShippingMethod;// "08";
                shipment.Service = service;

                //**************************************************Package Type********************************************************************

                //PackageType package = new PackageType();
                //PackageWeightType packageWeight = new PackageWeightType();
                //packageWeight.Weight = "10";
                //ShipUnitOfMeasurementType uom = new ShipUnitOfMeasurementType();
                //uom.Code = "LBS";
                //packageWeight.UnitOfMeasurement = uom;
                //package.PackageWeight = packageWeight;
                //PackagingType packType = new PackagingType();
                //packType.Code = "02";
                //package.Packaging = packType;
                //PackageType[] pkgArray = { package };
                //shipment.Package = pkgArray;

                //int i = 0;
                //PackageType[] package = new PackageType[10];
                //for (i = 0; i <= 2; i++)
                //{
                //    package[i] = new PackageType();
                //    PackageWeightType packageWeight = new PackageWeightType();
                //    packageWeight.Weight = "10";
                //    ShipUnitOfMeasurementType uom = new ShipUnitOfMeasurementType();
                //    uom.Code = "LBS";
                //    packageWeight.UnitOfMeasurement = uom;
                //    package[i].PackageWeight = packageWeight;
                //    PackagingType packType = new PackagingType();
                //    packType.Code = "02";
                //    package[i].Packaging = packType;

                //}

                PackageType[] package = new PackageType[myProductTable.Rows.Count];
                if (myProductTable != null && myProductTable.Rows.Count > 0)
                {
                    for (int cnt = 0; cnt < myProductTable.Rows.Count; cnt++)
                    {
                        // string ProductName = myProductTable.Rows[cnt]["ProductName"].ToString();
                        // string ProductWeight = myProductTable.Rows[cnt]["Weight"].ToString();
                        // string ProductPrice = myProductTable.Rows[cnt]["Price"].ToString();
                        ShoppingCartID += myProductTable.Rows[cnt]["ShippingCartID"] + "|" + myProductTable.Rows[cnt]["PackageID"] + ",";
                        // Double ProductW = 1.00;
                        // if (!string.IsNullOrEmpty(ProductWeight))
                        //     ProductW = Convert.ToDouble(ProductWeight);

                        // Double ProductP = 1.00;
                        // if (!string.IsNullOrEmpty(ProductPrice))
                        //     ProductP = Convert.ToDouble(ProductPrice);

                        // bool isInsured = Convert.ToBoolean(myProductTable.Rows[cnt]["IsInsured"]);

                        // us.AddPackage(ProductName, "02", ProductW, true, ProductP, "USD", "", "", deliveryNotification, isInsured);
                        package[cnt] = new PackageType();
                        PackageWeightType packageWeight = new PackageWeightType();
                        packageWeight.Weight = myProductTable.Rows[cnt]["Weight"].ToString();
                        ShipUnitOfMeasurementType uom = new ShipUnitOfMeasurementType();
                        uom.Code = "LBS";
                        packageWeight.UnitOfMeasurement = uom;
                        package[cnt].PackageWeight = packageWeight;
                        PackagingType packType = new PackagingType();
                        packType.Code = "02";
                        package[cnt].Packaging = packType;
                    }
                }
                PackageType[] pkgArray = package;
                shipment.Package = pkgArray;
                //*******************************************************Lable Size*************************************************************************************
                LabelSpecificationType labelSpec = new LabelSpecificationType();
                LabelStockSizeType labelStockSize = new LabelStockSizeType();
                CurrencyMonetaryType obj = new CurrencyMonetaryType();
                obj.CurrencyCode = "USD";
                obj.MonetaryValue = "1";
                shipment.InvoiceLineTotal = obj;
                labelStockSize.Height = "6";
                labelStockSize.Width = "4";
                labelSpec.LabelStockSize = labelStockSize;
                LabelImageFormatType labelImageFormat = new LabelImageFormatType();
                labelImageFormat.Code = "Gif";
                labelSpec.LabelImageFormat = labelImageFormat;
                shipmentRequest.LabelSpecification = labelSpec;
                shipmentRequest.Shipment = shipment;
                System.Net.ServicePointManager.CertificatePolicy = new TrustAllCertificatePolicy();
                // Console.WriteLine(shipmentRequest);

                try
                {
                    ShipmentResponse shipmentResponse = shpSvc.ProcessShipment(shipmentRequest);

                    Result += "##Success Login#" + shipmentResponse.Response.ResponseStatus.Description + "##Status#" + "";
                    //************************Lable*************************************************

                    Result += shipmentResponse.Response.ResponseStatus.Description + ", " + "" + "," + shipmentResponse.ShipmentResults.ShipmentIdentificationNumber + ", " + shipmentResponse.ShipmentResults.PackageResults.Length;
                    string ImgName = string.Empty;
                    Result += "##LabelFetch Login#" + shipmentResponse.Response.ResponseStatus.Description + "##LabelFetch Status#" + shipmentResponse.Response.ResponseStatus.Description;

                    string[] strParts = ShoppingCartID.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);


                    for (int i = 0; i < shipmentResponse.ShipmentResults.PackageResults.Length; i++)
                    {
                        string[] strIDs = { "" };

                        if (strParts.Length > i)
                            strIDs = strParts[i].Split('|');



                        string filename = "UPS-Package" + ((strIDs.Length > 1) ? strIDs[1] : i.ToString()) + "_" + shipmentResponse.ShipmentResults.PackageResults[i].TrackingNumber.ToString() + "_" + OrderNo + "_" + ((strIDs.Length > 0) ? strIDs[0] : i.ToString()) + "@" +
                                  DateTime.Now.Year.ToString() +
                                  DateTime.Now.Month.ToString() +
                                  DateTime.Now.Day.ToString() +
                                  DateTime.Now.Hour.ToString() +
                                  DateTime.Now.Minute.ToString() +
                                  DateTime.Now.Second.ToString() + "-" +
                                  i.ToString() + ".gif";
                        ImgName = ImgSavePath + filename; // Path of the Label Image
                        totalImg += filename + "#";
                        try
                        {
                            //  sp.SaveLabelInGifFile(string Image ImgName);
                            Base64ToImage(shipmentResponse.ShipmentResults.PackageResults[0].ShippingLabel.GraphicImage, ImgName);
                        }
                        catch
                        {
                        }
                    }
                    ReturnMessage = Result + "||" + totalImg + "||" + ShoppingCartID;
                }
                catch (System.Web.Services.Protocols.SoapException ex)
                {
                    totalImg = "";
                    ShoppingCartID = "";
                    ReturnMessage = ex.Detail.LastChild.InnerText.ToString();
                }
            }
        }
        catch (System.Web.Services.Protocols.SoapException ex)
        {
            ReturnMessage = ex.Detail.LastChild.InnerText.ToString();
        }
        return ReturnMessage;

    }
    public static void Base64ToImage(string base64String, string FilePath)
    {
        // Convert Base64 String to byte[]

        try
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0,
              imageBytes.Length);

            // Convert byte[] to Image
            ms.Write(imageBytes, 0, imageBytes.Length);
            System.Drawing.Image image = System.Drawing.Image.FromStream(ms, true);
            image.Save(FilePath);
        }
        catch (Exception ex)
        {

        }
    }


}


