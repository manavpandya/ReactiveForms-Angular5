using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.ShippingMethods.Endicia;
using Solution.Bussines.Components;

namespace Solution.ShippingMethods
{
    public class EndiciaService
    {
        public DataTable EndiciaGetRates(string tozipcode, string tocountry, double Weight, ref string Return_Response)
        {
            DataTable dt = new DataTable();

            // dt.Columns.Add("upsmethod", typeof(String));
            dt.Columns.Add("ShippingMethodName", typeof(String));
            dt.Columns.Add("Price", typeof(decimal));

            DataSet dsCommon = new DataSet();
            string SelectQuery = " SELECT Name,isnull(FixedPrice,0)+ ISNULL(AdditionalPrice,0) as FixedPrice FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
            " WHERE ShippingService='USPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0 and  tb_ShippingMethods.FixedPrice is not null and tb_ShippingMethods.ShowOnClient=1 and tb_ShippingMethods.isRTShipping=0 and tb_ShippingServices.StoreID='" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreID") + "'";

            dsCommon = CommonComponent.GetCommonDataSet(SelectQuery);
            if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dsCommon.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["ShippingMethodName"] = dsCommon.Tables[0].Rows[i]["Name"].ToString() + "($" + Convert.ToString(Math.Round(Convert.ToDecimal(dsCommon.Tables[0].Rows[i]["FixedPrice"]), 2)) + ")";
                    dr["Price"] = Math.Round(Convert.ToDecimal(dsCommon.Tables[0].Rows[i]["FixedPrice"]), 2);
                    dt.Rows.Add(dr);
                }
            }

            string[] zipcode = tozipcode.ToString().Split(' ');
            if (zipcode != null && zipcode.Length > 1)
            {
                tozipcode = zipcode[0].ToString();
            }
            EwsLabelService obj = new EwsLabelService();
            PostageRatesRequest objRateRequest = new PostageRatesRequest();

            objRateRequest.RequesterID = "LiveData";
            CertifiedIntermediary objCert = new CertifiedIntermediary();
            objCert.AccountID = Solution.Bussines.Components.Common.AppLogic.AppConfigs("USPS.UserName").ToString();
            objCert.PassPhrase = Solution.Bussines.Components.Common.AppLogic.AppConfigs("USPS.Password").ToString();
            objRateRequest.CertifiedIntermediary = objCert;

            //condition for international
            if (Solution.Bussines.Components.Common.AppLogic.AppConfigs("Shipping.OriginCountry").ToString().ToLower() != tocountry.ToLower())
                objRateRequest.MailClass = "International";

            else
                objRateRequest.MailClass = "Domestic";

            objRateRequest.WeightOz = Math.Round((Weight * 16), 1);
            objRateRequest.MailpieceShape = "Parcel";
            objRateRequest.Machinable = "True";

            objRateRequest.FromPostalCode = Solution.Bussines.Components.Common.AppLogic.AppConfigs("Shipping.OriginZip").ToString();
            objRateRequest.ToCountryCode = tocountry;
            objRateRequest.ToPostalCode = tozipcode;

            #region RESET Pass Phrase - Brijesh Shah - ONLY USE WHEN NEEDED
            //EndiciaLabel.ChangePassPhraseRequest objChangeReq = new EndiciaLabel.ChangePassPhraseRequest();
            //objChangeReq.NewPassPhrase = "mania15a";
            //objChangeReq.RequesterID = "abcd";
            //objChangeReq.RequestID = "abcd";

            //EndiciaLabel.CertifiedIntermediary objCI = new EndiciaLabel.CertifiedIntermediary();
            //objCI.AccountID = AppLogic.AppConfig("RTShipping.USPS.UserName").ToString();
            //objCI.PassPhrase = AppLogic.AppConfig("RTShipping.USPS.Password").ToString();
            //objChangeReq.CertifiedIntermediary = objCI;

            //EndiciaLabel.ChangePassPhraseRequestResponse ObjChangeRes = new EndiciaLabel.ChangePassPhraseRequestResponse();
            //ObjChangeRes = obj.ChangePassPhrase(objChangeReq);
            #endregion

            PostageRatesResponse objReponse = new PostageRatesResponse();
            objReponse = obj.CalculatePostageRates(objRateRequest);
            if (objReponse.Status == 0)
            {
                if (objReponse.PostagePrice.Length > 0)
                {

                    for (int i = 0; i <= objReponse.PostagePrice.Length - 1; i++)
                    {
                        DataSet dsShipping = new DataSet();
                        string shippingQuery = " SELECT * FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                         " WHERE ShippingService='USPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0 and tb_ShippingMethods.ShowOnClient=1 and  Name='" + "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "'";

                        dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                        if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                        {

                            if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0 && dsCommon.Tables[0].Select("Name='" + dsShipping.Tables[0].Rows[0]["Name"].ToString() + "'").Length > 0)
                            { }
                            else
                            {
                                DataRow dr = dt.NewRow();

                                dr["ShippingMethodName"] = "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "($" + Convert.ToString(Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2)) + ")";

                                if (dsShipping.Tables[0].Rows[0]["AdditionalPrice"] != null && dsShipping.Tables[0].Rows[0]["AdditionalPrice"] != DBNull.Value && Convert.ToDecimal(dsShipping.Tables[0].Rows[0]["AdditionalPrice"]) > 0)
                                {
                                    dr["Price"] = Math.Round(Convert.ToDecimal(dsShipping.Tables[0].Rows[0]["AdditionalPrice"].ToString()) + Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2);
                                }
                                else
                                    dr["Price"] = Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2);
                                dt.Rows.Add(dr);
                            }

                        }
                    }
                }
                return dt;
            }
            else
            {
                if (objReponse.ErrorMessage != "")
                {
                    string[] str = objReponse.ErrorMessage.Split('\n');
                    if (str.Length > 1)
                    {
                        Return_Response = "<br> USPS:- " + str[0].ToString().Replace("\n", "<br>");
                    }
                    else
                        Return_Response = "<br> USPS:- " + objReponse.ErrorMessage.Replace("\n", "<br>");
                }
                return null;
            }

        }


        public DataTable EndiciaGetRatesAdmin(string tozipcode, string tocountry, double Weight, ref string Return_Response)
        {
            DataTable dt = new DataTable();

            // dt.Columns.Add("upsmethod", typeof(String));
            dt.Columns.Add("ShippingMethodName", typeof(String));
            dt.Columns.Add("Price", typeof(decimal));

            DataSet dsCommon = new DataSet();
            string SelectQuery = " SELECT Name,isnull(FixedPrice,0)+ ISNULL(AdditionalPrice,0) as FixedPrice FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
            " WHERE ShippingService='USPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0 and  tb_ShippingMethods.FixedPrice is not null and tb_ShippingMethods.ShowOnClient=1 and tb_ShippingMethods.isRTShipping=0 and tb_ShippingServices.StoreID='" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + "'";

            dsCommon = CommonComponent.GetCommonDataSet(SelectQuery);
            if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dsCommon.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["ShippingMethodName"] = dsCommon.Tables[0].Rows[i]["Name"].ToString() + "($" + Convert.ToString(Math.Round(Convert.ToDecimal(dsCommon.Tables[0].Rows[i]["FixedPrice"]), 2)) + ")";
                    dr["Price"] = Math.Round(Convert.ToDecimal(dsCommon.Tables[0].Rows[i]["FixedPrice"]), 2);
                    dt.Rows.Add(dr);
                }
            }

            string[] zipcode = tozipcode.ToString().Split(' ');
            if (zipcode != null && zipcode.Length > 1)
            {
                tozipcode = zipcode[0].ToString();
            }
            EwsLabelService obj = new EwsLabelService();
            PostageRatesRequest objRateRequest = new PostageRatesRequest();

            objRateRequest.RequesterID = "LiveData";
            CertifiedIntermediary objCert = new CertifiedIntermediary();
            objCert.AccountID = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("USPS.UserName").ToString();
            objCert.PassPhrase = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("USPS.Password").ToString();
            objRateRequest.CertifiedIntermediary = objCert;

            //condition for international
            if (Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginCountry").ToString().ToLower() != tocountry.ToLower())
                objRateRequest.MailClass = "International";

            else
                objRateRequest.MailClass = "Domestic";

            objRateRequest.WeightOz = Math.Round((Weight * 16), 1);
            objRateRequest.MailpieceShape = "Parcel";
            objRateRequest.Machinable = "True";

            objRateRequest.FromPostalCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginZip").ToString();
            objRateRequest.ToCountryCode = tocountry;
            objRateRequest.ToPostalCode = tozipcode;

            #region RESET Pass Phrase - Brijesh Shah - ONLY USE WHEN NEEDED
            //EndiciaLabel.ChangePassPhraseRequest objChangeReq = new EndiciaLabel.ChangePassPhraseRequest();
            //objChangeReq.NewPassPhrase = "mania15a";
            //objChangeReq.RequesterID = "abcd";
            //objChangeReq.RequestID = "abcd";

            //EndiciaLabel.CertifiedIntermediary objCI = new EndiciaLabel.CertifiedIntermediary();
            //objCI.AccountID = AppLogic.AppConfig("RTShipping.USPS.UserName").ToString();
            //objCI.PassPhrase = AppLogic.AppConfig("RTShipping.USPS.Password").ToString();
            //objChangeReq.CertifiedIntermediary = objCI;

            //EndiciaLabel.ChangePassPhraseRequestResponse ObjChangeRes = new EndiciaLabel.ChangePassPhraseRequestResponse();
            //ObjChangeRes = obj.ChangePassPhrase(objChangeReq);
            #endregion

            PostageRatesResponse objReponse = new PostageRatesResponse();
            objReponse = obj.CalculatePostageRates(objRateRequest);
            if (objReponse.Status == 0)
            {
                if (objReponse.PostagePrice.Length > 0)
                {

                    for (int i = 0; i <= objReponse.PostagePrice.Length - 1; i++)
                    {
                        DataSet dsShipping = new DataSet();
                        string shippingQuery = " SELECT * FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                         " WHERE ShippingService='USPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0 and tb_ShippingMethods.ShowOnClient=1 and  Name='" + "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "'  and tb_ShippingServices.StoreID='" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + "'";

                        dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                        if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                        {

                            if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0 && dsCommon.Tables[0].Select("Name='" + dsShipping.Tables[0].Rows[0]["Name"].ToString() + "'").Length > 0)
                            { }
                            else
                            {
                                DataRow dr = dt.NewRow();

                                dr["ShippingMethodName"] = "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "($" + Convert.ToString(Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2)) + ")";

                                if (dsShipping.Tables[0].Rows[0]["AdditionalPrice"] != null && dsShipping.Tables[0].Rows[0]["AdditionalPrice"] != DBNull.Value && Convert.ToDecimal(dsShipping.Tables[0].Rows[0]["AdditionalPrice"]) > 0)
                                {
                                    dr["Price"] = Math.Round(Convert.ToDecimal(dsShipping.Tables[0].Rows[0]["AdditionalPrice"].ToString()) + Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2);
                                }
                                else
                                    dr["Price"] = Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2);
                                dt.Rows.Add(dr);
                            }

                        }
                    }
                }
                return dt;
            }
            else
            {
                if (objReponse.ErrorMessage != "")
                {
                    string[] str = objReponse.ErrorMessage.Split('\n');
                    if (str.Length > 1)
                    {
                        Return_Response = "<br> USPS:- " + str[0].ToString().Replace("\n", "<br>");
                    }
                    else
                        Return_Response = "<br> USPS:- " + objReponse.ErrorMessage.Replace("\n", "<br>");
                }
                return null;
            }

        }



        public DataTable EndiciaGetRatesEstimatedDays(string tozipcode, string tocountry, double Weight, ref string Return_Response)
        {
            DataTable dt = new DataTable();

            // dt.Columns.Add("upsmethod", typeof(String));
            dt.Columns.Add("ShippingMethodName", typeof(String));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("EstimatedDays", typeof(DateTime));

            DataSet dsCommon = new DataSet();
            string SelectQuery = " SELECT Name,isnull(FixedPrice,0)+ ISNULL(AdditionalPrice,0) as FixedPrice,DATEADD(DAY,isnull(EstimatedDays,0),GETDATE()) as EstimatedDays  FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
            " WHERE ShippingService='USPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0 and  tb_ShippingMethods.FixedPrice is not null and tb_ShippingMethods.ShowOnClient=1 and tb_ShippingMethods.isRTShipping=0 and tb_ShippingServices.StoreID='" + Solution.Bussines.Components.Common.AppLogic.AppConfigs("StoreID") + "'";

            dsCommon = CommonComponent.GetCommonDataSet(SelectQuery);
            if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i <= dsCommon.Tables[0].Rows.Count - 1; i++)
                {
                    DataRow dr = dt.NewRow();

                    dr["ShippingMethodName"] = dsCommon.Tables[0].Rows[i]["Name"].ToString() + "($" + Convert.ToString(Math.Round(Convert.ToDecimal(dsCommon.Tables[0].Rows[i]["FixedPrice"]), 2)) + ")";
                    dr["Price"] = Math.Round(Convert.ToDecimal(dsCommon.Tables[0].Rows[i]["FixedPrice"]), 2);
                    dr["EstimatedDays"] = Convert.ToDateTime(dsCommon.Tables[0].Rows[i]["EstimatedDays"].ToString()).ToShortDateString();
                    dt.Rows.Add(dr);
                }
            }

            string[] zipcode = tozipcode.ToString().Split(' ');
            if (zipcode != null && zipcode.Length > 1)
            {
                tozipcode = zipcode[0].ToString();
            }
            EwsLabelService obj = new EwsLabelService();
            PostageRatesRequest objRateRequest = new PostageRatesRequest();

            objRateRequest.RequesterID = "LiveData";
            CertifiedIntermediary objCert = new CertifiedIntermediary();
            objCert.AccountID = Solution.Bussines.Components.Common.AppLogic.AppConfigs("USPS.UserName").ToString();
            objCert.PassPhrase = Solution.Bussines.Components.Common.AppLogic.AppConfigs("USPS.Password").ToString();
            objRateRequest.CertifiedIntermediary = objCert;

            //condition for international
            if (Solution.Bussines.Components.Common.AppLogic.AppConfigs("Shipping.OriginCountry").ToString().ToLower() != tocountry.ToLower())
                objRateRequest.MailClass = "International";

            else
                objRateRequest.MailClass = "Domestic";

            objRateRequest.WeightOz = Math.Round((Weight * 16), 1);
            objRateRequest.MailpieceShape = "Parcel";
            objRateRequest.Machinable = "True";

            objRateRequest.FromPostalCode = Solution.Bussines.Components.Common.AppLogic.AppConfigs("Shipping.OriginZip").ToString();
            objRateRequest.ToCountryCode = tocountry;
            objRateRequest.ToPostalCode = tozipcode;

            #region RESET Pass Phrase - Brijesh Shah - ONLY USE WHEN NEEDED
            //EndiciaLabel.ChangePassPhraseRequest objChangeReq = new EndiciaLabel.ChangePassPhraseRequest();
            //objChangeReq.NewPassPhrase = "mania15a";
            //objChangeReq.RequesterID = "abcd";
            //objChangeReq.RequestID = "abcd";

            //EndiciaLabel.CertifiedIntermediary objCI = new EndiciaLabel.CertifiedIntermediary();
            //objCI.AccountID = AppLogic.AppConfig("RTShipping.USPS.UserName").ToString();
            //objCI.PassPhrase = AppLogic.AppConfig("RTShipping.USPS.Password").ToString();
            //objChangeReq.CertifiedIntermediary = objCI;

            //EndiciaLabel.ChangePassPhraseRequestResponse ObjChangeRes = new EndiciaLabel.ChangePassPhraseRequestResponse();
            //ObjChangeRes = obj.ChangePassPhrase(objChangeReq);
            #endregion

            PostageRatesResponse objReponse = new PostageRatesResponse();
            objReponse = obj.CalculatePostageRates(objRateRequest);
            if (objReponse.Status == 0)
            {
                if (objReponse.PostagePrice.Length > 0)
                {

                    for (int i = 0; i <= objReponse.PostagePrice.Length - 1; i++)
                    {
                        DataSet dsShipping = new DataSet();
                        string shippingQuery = " SELECT AdditionalPrice,DATEADD(DAY,isnull(EstimatedDays,0),GETDATE()) as EstimatedDays,tb_ShippingServices.ShippingServiceID,tb_ShippingMethods.Name FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                         " WHERE ShippingService='USPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0 and tb_ShippingMethods.ShowOnClient=1 and  Name='" + "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "'";

                        dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                        if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                        {

                            if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0 && dsCommon.Tables[0].Select("Name='" + dsShipping.Tables[0].Rows[0]["Name"].ToString() + "'").Length > 0)
                            { }
                            else
                            {
                                DataRow dr = dt.NewRow();

                                dr["ShippingMethodName"] = "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "($" + Convert.ToString(Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2)) + ")";

                                if (dsShipping.Tables[0].Rows[0]["AdditionalPrice"] != null && dsShipping.Tables[0].Rows[0]["AdditionalPrice"] != DBNull.Value && Convert.ToDecimal(dsShipping.Tables[0].Rows[0]["AdditionalPrice"]) > 0)
                                {
                                    dr["Price"] = Math.Round(Convert.ToDecimal(dsShipping.Tables[0].Rows[0]["AdditionalPrice"].ToString()) + Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2);
                                }
                                else
                                    dr["Price"] = Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2);

                                if (dsShipping.Tables[0].Rows[0]["EstimatedDays"] != null && dsShipping.Tables[0].Rows[0]["EstimatedDays"] != DBNull.Value)
                                    dr["EstimatedDays"] = Convert.ToDateTime(dsShipping.Tables[0].Rows[0]["EstimatedDays"].ToString()).ToShortDateString();
                                else
                                    dr["EstimatedDays"] = DBNull.Value;
                                dt.Rows.Add(dr);
                            }

                        }
                    }
                }
                return dt;
            }
            else
            {
                if (objReponse.ErrorMessage != "")
                {
                    string[] str = objReponse.ErrorMessage.Split('\n');
                    if (str.Length > 1)
                    {
                        Return_Response = "<br> USPS:- " + str[0].ToString().Replace("\n", "<br>");
                    }
                    else
                        Return_Response = "<br> USPS:- " + objReponse.ErrorMessage.Replace("\n", "<br>");
                }
                return null;
            }

        }


        public DataTable EndiciaGetRatesAdminShippingLabel(string tozipcode, string tocountry, double Weight, ref string Return_Response)
        {
            DataTable dt = new DataTable();

            // dt.Columns.Add("upsmethod", typeof(String));
            dt.Columns.Add("ShippingMethodName", typeof(String));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Shippingmethod", typeof(String));


            string[] zipcode = tozipcode.ToString().Split(' ');
            if (zipcode != null && zipcode.Length > 1)
            {
                tozipcode = zipcode[0].ToString();
            }
            EwsLabelService obj = new EwsLabelService();
            PostageRatesRequest objRateRequest = new PostageRatesRequest();

            objRateRequest.RequesterID = "LiveData";
            CertifiedIntermediary objCert = new CertifiedIntermediary();
            objCert.AccountID = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("USPS.UserName").ToString();
            objCert.PassPhrase = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("USPS.Password").ToString();
            objRateRequest.CertifiedIntermediary = objCert;

            //condition for international
            if (Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginCountry").ToString().ToLower() != tocountry.ToLower())
                objRateRequest.MailClass = "International";

            else
                objRateRequest.MailClass = "Domestic";

            objRateRequest.WeightOz = Math.Round((Weight * 16), 1);
            objRateRequest.MailpieceShape = "Parcel";
            objRateRequest.Machinable = "True";

            objRateRequest.FromPostalCode = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("Shipping.OriginZip").ToString();
            objRateRequest.ToCountryCode = tocountry;
            objRateRequest.ToPostalCode = tozipcode;

            #region RESET Pass Phrase - Brijesh Shah - ONLY USE WHEN NEEDED
            //EndiciaLabel.ChangePassPhraseRequest objChangeReq = new EndiciaLabel.ChangePassPhraseRequest();
            //objChangeReq.NewPassPhrase = "mania15a";
            //objChangeReq.RequesterID = "abcd";
            //objChangeReq.RequestID = "abcd";

            //EndiciaLabel.CertifiedIntermediary objCI = new EndiciaLabel.CertifiedIntermediary();
            //objCI.AccountID = AppLogic.AppConfig("RTShipping.USPS.UserName").ToString();
            //objCI.PassPhrase = AppLogic.AppConfig("RTShipping.USPS.Password").ToString();
            //objChangeReq.CertifiedIntermediary = objCI;

            //EndiciaLabel.ChangePassPhraseRequestResponse ObjChangeRes = new EndiciaLabel.ChangePassPhraseRequestResponse();
            //ObjChangeRes = obj.ChangePassPhrase(objChangeReq);
            #endregion

            PostageRatesResponse objReponse = new PostageRatesResponse();
            objReponse = obj.CalculatePostageRates(objRateRequest);
            if (objReponse.Status == 0)
            {
                if (objReponse.PostagePrice.Length > 0)
                {

                    for (int i = 0; i <= objReponse.PostagePrice.Length - 1; i++)
                    {
                        DataSet dsShipping = new DataSet();
                        string shippingQuery = " SELECT * FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                         " WHERE ShippingService='USPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0 and tb_ShippingMethods.ShowOnClient=1 and  Name='" + "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "'  and tb_ShippingServices.StoreID='" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + "'";

                        dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                        if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = dt.NewRow();

                            dr["ShippingMethodName"] = "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "($" + Convert.ToString(Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2)) + ")";
                            dr["Price"] = Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2);
                            dr["Shippingmethod"] = objReponse.PostagePrice[i].Postage.MailService;
                            dt.Rows.Add(dr);


                        }
                    }
                }
                return dt;
            }
            else
            {
                if (objReponse.ErrorMessage != "")
                {
                    string[] str = objReponse.ErrorMessage.Split('\n');
                    if (str.Length > 1)
                    {
                        Return_Response = "<br> USPS:- " + str[0].ToString().Replace("\n", "<br>");
                    }
                    else
                        Return_Response = "<br> USPS:- " + objReponse.ErrorMessage.Replace("\n", "<br>");
                }
                return null;
            }

        }



        public DataTable EndiciaGetRatesAdminWarehouseShippingLabel(string FromCountry, string FromZipCode, string tozipcode, string tocountry, double Weight, ref string Return_Response)
        {
            DataTable dt = new DataTable();

            // dt.Columns.Add("upsmethod", typeof(String));
            dt.Columns.Add("ShippingMethodName", typeof(String));
            dt.Columns.Add("Price", typeof(decimal));
            dt.Columns.Add("Shippingmethod", typeof(String));


            string[] zipcode = tozipcode.ToString().Split(' ');
            if (zipcode != null && zipcode.Length > 1)
            {
                tozipcode = zipcode[0].ToString();
            }
            EwsLabelService obj = new EwsLabelService();
            PostageRatesRequest objRateRequest = new PostageRatesRequest();

            objRateRequest.RequesterID = "LiveData";
            CertifiedIntermediary objCert = new CertifiedIntermediary();
            objCert.AccountID = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("USPS.UserName").ToString();
            objCert.PassPhrase = Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("USPS.Password").ToString();
            objRateRequest.CertifiedIntermediary = objCert;

            //condition for international
            if (FromCountry.ToLower() != tocountry.ToLower())
                objRateRequest.MailClass = "International";

            else
                objRateRequest.MailClass = "Domestic";

            objRateRequest.WeightOz = Math.Round((Weight * 16), 1);
            objRateRequest.MailpieceShape = "Parcel";
            objRateRequest.Machinable = "True";

            objRateRequest.FromPostalCode = FromZipCode;
            objRateRequest.ToCountryCode = tocountry;
            objRateRequest.ToPostalCode = tozipcode;

            #region RESET Pass Phrase - Brijesh Shah - ONLY USE WHEN NEEDED
            //EndiciaLabel.ChangePassPhraseRequest objChangeReq = new EndiciaLabel.ChangePassPhraseRequest();
            //objChangeReq.NewPassPhrase = "mania15a";
            //objChangeReq.RequesterID = "abcd";
            //objChangeReq.RequestID = "abcd";

            //EndiciaLabel.CertifiedIntermediary objCI = new EndiciaLabel.CertifiedIntermediary();
            //objCI.AccountID = AppLogic.AppConfig("RTShipping.USPS.UserName").ToString();
            //objCI.PassPhrase = AppLogic.AppConfig("RTShipping.USPS.Password").ToString();
            //objChangeReq.CertifiedIntermediary = objCI;

            //EndiciaLabel.ChangePassPhraseRequestResponse ObjChangeRes = new EndiciaLabel.ChangePassPhraseRequestResponse();
            //ObjChangeRes = obj.ChangePassPhrase(objChangeReq);
            #endregion

            PostageRatesResponse objReponse = new PostageRatesResponse();
            objReponse = obj.CalculatePostageRates(objRateRequest);
            if (objReponse.Status == 0)
            {
                if (objReponse.PostagePrice.Length > 0)
                {

                    for (int i = 0; i <= objReponse.PostagePrice.Length - 1; i++)
                    {
                        DataSet dsShipping = new DataSet();
                        string shippingQuery = " SELECT * FROM dbo.tb_ShippingMethods INNER JOIN dbo.tb_ShippingServices ON dbo.tb_ShippingMethods.ShippingServiceID = dbo.tb_ShippingServices.ShippingServiceID " +
                         " WHERE ShippingService='USPS' AND tb_ShippingMethods.Active=1 AND tb_ShippingMethods.Deleted=0 and tb_ShippingMethods.ShowOnClient=1 and  Name='" + "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "'  and tb_ShippingServices.StoreID='" + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreID") + "'";

                        dsShipping = CommonComponent.GetCommonDataSet(shippingQuery);
                        if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                        {
                            DataRow dr = dt.NewRow();

                            dr["ShippingMethodName"] = "USPS - " + objReponse.PostagePrice[i].Postage.MailService + "($" + Convert.ToString(Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2)) + ")";
                            dr["Price"] = Math.Round(Convert.ToDecimal(objReponse.PostagePrice[i].Postage.TotalAmount), 2);
                            dr["Shippingmethod"] = objReponse.PostagePrice[i].Postage.MailService;
                            dt.Rows.Add(dr);


                        }
                    }
                }
                return dt;
            }
            else
            {
                if (objReponse.ErrorMessage != "")
                {
                    string[] str = objReponse.ErrorMessage.Split('\n');
                    if (str.Length > 1)
                    {
                        // str[0] = "The maximum per package weight for the selected service from the selected country is 70 LBS.";
                        if (str[0].ToString().ToLower().Contains("one or more mail classes"))
                            str[0] = "The maximum per package weight for the selected service from the selected country is 70 LBS.";


                        Return_Response = "<br> USPS:- " + str[0].ToString().Replace("\n", "<br>");
                    }
                    else
                        Return_Response = "<br> USPS:- " + objReponse.ErrorMessage.Replace("\n", "<br>");
                }
                return null;
            }

        }


    }
}
