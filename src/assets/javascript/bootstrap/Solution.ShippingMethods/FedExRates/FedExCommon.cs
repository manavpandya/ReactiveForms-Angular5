using System;
using System.Web.Services.Protocols;
using System.Data;
using Solution.ShippingMethods.FedExShipServiceWebReference;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;

/// <summary>
/// Summary description for FedExCommon
/// </summary>
public class FedExCommon
{
    public ProcessShipmentRequest LoginDetailfedEx(ProcessShipmentRequest request)
    {
        try
        {
            request.WebAuthenticationDetail = new WebAuthenticationDetail();
            request.WebAuthenticationDetail.UserCredential = new WebAuthenticationCredential();
            request.WebAuthenticationDetail.UserCredential.Key = AppLogic.AppConfigs("FedEx.Key"); // Replace "XXX" with the Key
            request.WebAuthenticationDetail.UserCredential.Password = AppLogic.AppConfigs("FedEx.Password"); // Replace "XXX" with the Password
            request.ClientDetail = new ClientDetail();
            request.ClientDetail.AccountNumber = AppLogic.AppConfigs("FedEx.AccountNumber"); // Replace "XXX" with client's account number
            request.ClientDetail.MeterNumber = AppLogic.AppConfigs("FedEx.Meter"); // Replace "XXX" with client's meter number

            //if (string.IsNullOrEmpty(request.WebAuthenticationDetail.UserCredential.Key)) request.WebAuthenticationDetail.UserCredential.Key = "KEfvbBjalsTF4KRG"; // Replace "XXX" with the Key
            //if (string.IsNullOrEmpty(request.WebAuthenticationDetail.UserCredential.Password)) request.WebAuthenticationDetail.UserCredential.Password = "129R6zSEMGjBocWOBxBdW8e8H";
            //if (string.IsNullOrEmpty(request.ClientDetail.AccountNumber)) request.ClientDetail.AccountNumber = "510087321";
            //if (string.IsNullOrEmpty(request.ClientDetail.MeterNumber)) request.ClientDetail.MeterNumber = "118512550";
            // Replace "XXX" with the Password
        }
        catch { }
        return request;

    }
    public ProcessShipmentRequest SetAddress(int WareHouseID, ProcessShipmentRequest request, DataSet ds)
    {
        try
        {
            CountryComponent objCountry = new CountryComponent();
            StateComponent objState = new StateComponent();
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

                OrgCountry = Convert.ToString(objCountry.GetCountryCodeByNameForShippingLabel(OrgCountry));
                OrgState = Convert.ToString(objState.GetStateCodeByName(OrgState));

                request.RequestedShipment.Shipper = new Party();
                request.RequestedShipment.Shipper.Contact = new Contact();
                request.RequestedShipment.Shipper.Contact.CompanyName = AppLogic.AppConfigs("Shipping.CompanyName");
                request.RequestedShipment.Shipper.Contact.PhoneNumber = AppLogic.AppConfigs("Shipping.OriginPhone");
                request.RequestedShipment.Shipper.Address = new Address();
                request.RequestedShipment.Shipper.Address.StreetLines = new string[2] { OrgAddress1, OrgAddress2 };
                request.RequestedShipment.Shipper.Address.City = OrgCity;
                request.RequestedShipment.Shipper.Address.StateOrProvinceCode = OrgState;
                request.RequestedShipment.Shipper.Address.PostalCode = OrgShippingZip;
                request.RequestedShipment.Shipper.Address.CountryCode = OrgCountry;
            }
            else
            {
                request.RequestedShipment.Shipper = new Party(); // Sender information
                request.RequestedShipment.Shipper.Contact = new Contact();
                //request.RequestedShipment.Shipper.Contact.PersonName = BussinessLogic.AppLogic.AppConfigs("Shipping.OriginContactName");
                request.RequestedShipment.Shipper.Contact.CompanyName = AppLogic.AppConfigs("Shipping.CompanyName");
                request.RequestedShipment.Shipper.Contact.PhoneNumber = AppLogic.AppConfigs("Shipping.OriginPhone");
                request.RequestedShipment.Shipper.Address = new Address();
                request.RequestedShipment.Shipper.Address.StreetLines = new string[2] { AppLogic.AppConfigs("Shipping.OriginAddress"), AppLogic.AppConfigs("Shipping.OriginAddress2") };
                request.RequestedShipment.Shipper.Address.City = AppLogic.AppConfigs("Shipping.OriginCity");
                request.RequestedShipment.Shipper.Address.StateOrProvinceCode = AppLogic.AppConfigs("Shipping.OriginState");
                request.RequestedShipment.Shipper.Address.PostalCode = AppLogic.AppConfigs("Shipping.OriginZip");
                request.RequestedShipment.Shipper.Address.CountryCode = AppLogic.AppConfigs("Shipping.OriginCountry");
            }

            objState = new StateComponent();
            string Abbreviation = objState.GetStateCodeByName(ds.Tables[0].Rows[0]["ShippingState"].ToString());
            objCountry = new CountryComponent();
            string TwoLetterISO = objCountry.GetCountryCodeByNameForShippingLabel(ds.Tables[0].Rows[0]["ShippingCountry"].ToString());

            request.RequestedShipment.Recipient = new Party(); // Recipient information
            request.RequestedShipment.Recipient.Contact = new Contact();
            request.RequestedShipment.Recipient.Contact.PersonName = ds.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + ds.Tables[0].Rows[0]["ShippingLastName"].ToString();
            request.RequestedShipment.Recipient.Contact.CompanyName = ds.Tables[0].Rows[0]["ShippingCompany"].ToString();
            request.RequestedShipment.Recipient.Contact.PhoneNumber = ds.Tables[0].Rows[0]["ShippingPhone"].ToString();
            //
            request.RequestedShipment.Recipient.Address = new Address();
            request.RequestedShipment.Recipient.Address.StreetLines = new string[2] { ds.Tables[0].Rows[0]["ShippingAddress1"].ToString(), Convert.ToString(ds.Tables[0].Rows[0]["ShippingAddress2"].ToString()) == "" ? Convert.ToString(ds.Tables[0].Rows[0]["ShippingSuite"].ToString()) : Convert.ToString(ds.Tables[0].Rows[0]["ShippingAddress2"].ToString()) };

            request.RequestedShipment.Recipient.Address.City = ds.Tables[0].Rows[0]["ShippingCity"].ToString();
            request.RequestedShipment.Recipient.Address.StateOrProvinceCode = string.IsNullOrEmpty(Abbreviation) ? "" : Abbreviation;
            request.RequestedShipment.Recipient.Address.PostalCode = ds.Tables[0].Rows[0]["ShippingZip"].ToString();
            request.RequestedShipment.Recipient.Address.CountryCode = string.IsNullOrEmpty(TwoLetterISO) ? "" : TwoLetterISO;
            request.RequestedShipment.Recipient.Address.Residential = false; //Convert.ToBoolean(Convert.ToInt32(Convert.ToString(ds.Tables[0].Rows[0]["shippingresidencetype"])));

            #region Demo

            //request.RequestedShipment.Shipper = new Party(); // Sender information
            //request.RequestedShipment.Shipper.Contact = new Contact();
            //request.RequestedShipment.Shipper.Contact.PersonName = "Sender Name";
            //request.RequestedShipment.Shipper.Contact.CompanyName = "Sender Company Name";
            //request.RequestedShipment.Shipper.Contact.PhoneNumber = "0805522713";
            //request.RequestedShipment.Shipper.Address = new Address();
            //request.RequestedShipment.Shipper.Address.StreetLines = new string[1] { "Address Line 1" };
            //request.RequestedShipment.Shipper.Address.City = "Memphis";
            //request.RequestedShipment.Shipper.Address.StateOrProvinceCode = "TN";
            //request.RequestedShipment.Shipper.Address.PostalCode = "38110";
            //request.RequestedShipment.Shipper.Address.CountryCode = "US";

            //request.RequestedShipment.Recipient = new Party(); // Recipient information
            //request.RequestedShipment.Recipient.Contact = new Contact();
            //request.RequestedShipment.Recipient.Contact.PersonName = "Recipient Name";
            //request.RequestedShipment.Recipient.Contact.CompanyName = "Recipient Company Name";
            //request.RequestedShipment.Recipient.Contact.PhoneNumber = "9012637906";
            ////
            //request.RequestedShipment.Recipient.Address = new Address();
            //request.RequestedShipment.Recipient.Address.StreetLines = new string[1] { "Address Line 1" };
            //request.RequestedShipment.Recipient.Address.City = "Richmond";
            //request.RequestedShipment.Recipient.Address.StateOrProvinceCode = "BC";
            //request.RequestedShipment.Recipient.Address.PostalCode = "V7C4V4";
            //request.RequestedShipment.Recipient.Address.CountryCode = "CA";
            //request.RequestedShipment.Recipient.Address.Residential = false;
            return request;
            #endregion
        }
        catch { }
        return request;

    }
    public ServiceType getservicetype(string rd)
    {
        ServiceType st = new ServiceType();
        if (rd.ToUpper().Replace(" ", "_").Contains("EUROPE_FIRST_INTERNATIONAL_PRIORITY"))
            st = ServiceType.EUROPE_FIRST_INTERNATIONAL_PRIORITY;
        else if (rd.ToUpper().Replace(" ", "_").Contains("FEDEX_1_DAY_FREIGHT"))
            st = ServiceType.FEDEX_1_DAY_FREIGHT;
        else if (rd.ToUpper().Replace(" ", "_").Contains("FEDEX_2_DAY"))
            st = ServiceType.FEDEX_2_DAY;
        else if (rd.ToUpper().Replace(" ", "_").Contains("FEDEX_2_DAY_FREIGHT"))
            st = ServiceType.FEDEX_2_DAY_FREIGHT;
        else if (rd.ToUpper().Replace(" ", "_").Contains("FEDEX_3_DAY_FREIGHT"))
            st = ServiceType.FEDEX_3_DAY_FREIGHT;
        else if (rd.ToUpper().Replace(" ", "_").Contains("FEDEX_GROUND"))
            st = ServiceType.FEDEX_GROUND;
        else if (rd.ToUpper().Replace(" ", "_").Contains("FIRST_OVERNIGHT"))
            st = ServiceType.FIRST_OVERNIGHT;
        else if (rd.ToUpper().Replace(" ", "_").Contains("GROUND_HOME_DELIVERY"))
            st = ServiceType.GROUND_HOME_DELIVERY;
        else if (rd.ToUpper().Replace(" ", "_").Contains("INTERNATIONAL_ECONOMY"))
            st = ServiceType.INTERNATIONAL_ECONOMY;
        else if (rd.ToUpper().Replace(" ", "_").Contains("INTERNATIONAL_ECONOMY_FREIGHT"))
            st = ServiceType.INTERNATIONAL_ECONOMY_FREIGHT;
        else if (rd.ToUpper().Replace(" ", "_").Contains("INTERNATIONAL_FIRST"))
            st = ServiceType.INTERNATIONAL_FIRST;
        else if (rd.ToUpper().Replace(" ", "_").Contains("INTERNATIONAL_GROUND"))
            st = ServiceType.INTERNATIONAL_GROUND;
        else if (rd.ToUpper().Replace(" ", "_").Contains("INTERNATIONAL_PRIORITY"))
            st = ServiceType.INTERNATIONAL_PRIORITY;
        else if (rd.ToUpper().Replace(" ", "_").Contains("INTERNATIONAL_PRIORITY_FREIGHT"))
            st = ServiceType.INTERNATIONAL_PRIORITY_FREIGHT;
        else if (rd.ToUpper().Replace(" ", "_").Contains("PRIORITY_OVERNIGHT"))
            st = ServiceType.PRIORITY_OVERNIGHT;
        else if (rd.ToUpper().Replace(" ", "_").Contains("SMART_POST"))
            st = ServiceType.SMART_POST;
        else if (rd.ToUpper().Replace(" ", "_").Contains("STANDARD_OVERNIGHT"))
            st = ServiceType.STANDARD_OVERNIGHT;
        else if (rd.ToUpper().Replace(" ", "_").Contains("FEDEX_EXPRESS_SAVER"))
            st = ServiceType.FEDEX_EXPRESS_SAVER;
        return st;
    }
}
