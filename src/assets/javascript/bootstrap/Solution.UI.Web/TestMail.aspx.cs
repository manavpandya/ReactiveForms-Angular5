using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Net.Mail;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.Common;
using System.Net;
using System.IO;
using LumenWorks.Framework.IO.Csv;
using System.Web.UI.WebControls;
using Newtonsoft.Json;

namespace Solution.UI.Web
{
    public partial class TestMail : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //AppLogic.ApplicationStart();
            //lblMesage.Text = "";
            //lblError.Text = "";
            //CommonOperations.RedirectWithSSL(true);
            //txtEmailid.Text = "brijeshs@kaushalam.com";

            //Response.Write(AppLogic.AppConfigs("LIVE_SERVER") + "/" + "invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt("1078")));

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //string strBody = "";
            //CustomerComponent objCustomer = new CustomerComponent();
            //DataSet dsMailTemplate = new DataSet();
            //dsMailTemplate = objCustomer.GetEmailTamplate("NewsSubscription");

            //if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            //{
            //    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
            //}

            //AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
            //    SendMail(txtEmailid.Text, strBody, av);
            // SendMail(1078);
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            DataSet dsproduct = new DataSet();

        }


        [System.Web.Services.WebMethod]
        public static string GetData(Int32 ProductId, Int32 Width, Int32 Length, Int32 Qty, string style, string options)
        {
            string resp = string.Empty;
            DataSet ds = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            ds = objSql.GetDs("EXEC usp_Product_Pricecalculator " + ProductId + "," + Width.ToString() + "," + Length.ToString() + "," + Qty.ToString() + ",'" + style + "','" + options + "'");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0][0].ToString()));
            }

            return resp;
        }
        [System.Web.Services.WebMethod]
        public static string GetDataAdmin(Int32 ProductId, Int32 Width, Int32 Length, Int32 Qty, string style, string options, Int32 ProductType)
        {
            string resp = string.Empty;
            DataSet ds = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            ds = objSql.GetDs("EXEC usp_Product_Pricecalculator " + ProductId + "," + Width.ToString() + "," + Length.ToString() + "," + Qty.ToString() + ",'" + style + "','" + options + "'");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0][0].ToString()));
            }

            return resp;
        }
        [System.Web.Services.WebMethod]
        public static string GetDataRomanprice(Int32 ProductId, string Width, string Width2, Int32 Qty, string options, string Length, string Length2, string fabricnameoptin, string colorvalueid, string lift)
        {
            decimal VariValue = 0, WidthStdAllow = 0;
            decimal price = 0;
            if (Length != "")
            {
                VariValue = Convert.ToDecimal(Length);
            }
            if (Width != "")
            {
                WidthStdAllow = Convert.ToDecimal(Width);
            }
            decimal LengthStdAllow = 0;
            if (Length2.ToString().IndexOf("/") > -1)
            {
                string strwidth = Length2.ToString().Substring(0, Length2.ToString().IndexOf("/"));
                strwidth = strwidth.Replace("/", "");
                decimal tt = Convert.ToDecimal(strwidth);
                strwidth = Convert.ToString(Length2.ToString()).Replace(strwidth + "/", "");
                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                decimal VariValue1 = tt;
                VariValue = VariValue + VariValue1;
            }
            else
            {
                decimal VariValue1 = 0;
                decimal.TryParse(Length2.ToString(), out VariValue1);
                VariValue = VariValue + VariValue1;

            }
            if (Width2.ToString().IndexOf("/") > -1)
            {
                string strwidth = Width2.ToString().Substring(0, Width2.ToString().IndexOf("/"));
                strwidth = strwidth.Replace("/", "");
                decimal tt = Convert.ToDecimal(strwidth);
                strwidth = Convert.ToString(Width2.ToString()).Replace(strwidth + "/", "");
                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                decimal WidthStdAllow1 = tt;
                WidthStdAllow = WidthStdAllow + WidthStdAllow1;
            }
            else
            {
                decimal WidthStdAllow1 = 0;
                decimal.TryParse(Width2.ToString(), out WidthStdAllow1);
                WidthStdAllow = WidthStdAllow + WidthStdAllow1;

            }
            ///int RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(RomanShadeId,0) as RomanShadeId from tb_product where ProductId=" + Convert.ToInt32(ProductId.ToString()) + " and StoreId=1"));
            int RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select TOp 1 ISNULL(RomanShadeId,0) as RomanShadeId from tb_ProductRomanShadeYardage where ShadeName='" + fabricnameoptin.ToString().Trim() + "' AND isnull(Active,0)=1"));




            DataSet dsRoman = new DataSet();
            dsRoman = CommonComponent.GetCommonDataSet("Select isnull(FabricPerYardCost,0) as FabricPerYardCost,isnull(ManufuacturingCost,0) as ManufuacturingCost from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1");
            decimal tempWidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + WidthStdAllow + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));

            string strFabricname = fabricnameoptin;//Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 ShadeName from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + ""));
            if (strFabricname.ToString().ToLower().Trim() == "casual")
            {
                LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
            }
            else if (strFabricname.ToString().ToLower().Trim() == "relaxed")
            {
                LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(2 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                //LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
            }
            else if (strFabricname.ToString().ToLower().Trim() == "soft fold")
            {
                LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(2 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
            }
            else if (strFabricname.ToString().ToLower().Trim() == "front slat")
            {
                LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(1.75 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
            }
            else
            {
                LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
            }

            Decimal fbyardcost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT isnull(VariantPrice,0) FROM tb_ProductVariantValue WHERE VariantValueID=" + colorvalueid + ""));

            LengthStdAllow = Math.Round(LengthStdAllow, 2);
            if (dsRoman != null && dsRoman.Tables.Count > 0 && dsRoman.Tables[0].Rows.Count > 0)
            {
                decimal yardprice = Convert.ToDecimal(fbyardcost) * Convert.ToDecimal(LengthStdAllow);
                yardprice = yardprice + Convert.ToDecimal(dsRoman.Tables[0].Rows[0]["ManufuacturingCost"]);
                decimal rangecost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Cost,0) as Cost from tb_Roman_Shipping where Fromwidth <= " + WidthStdAllow + " And Towidth >=" + WidthStdAllow + " and ISNULL(Active,0)=1"));
                yardprice = yardprice + rangecost;
                decimal ProductOptionsPrice = 0;
                decimal Duties = 0;
                if (options.ToString().ToLower() == "lined")
                {
                    ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0) from tb_ProductOptionsPrice where  Options='" + options.ToString().ToLower().Replace("'", "''") + "' and ProductId=" + ProductId + ""));
                }
                else if (options.ToString().ToLower() == "lined & interlined" || options.ToString().ToLower() == "lined &amp; interlined")
                {
                    ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  (Options='" + options.ToString().ToLower().Replace("'", "''") + "' Or Options='" + options.ToString().ToLower().Replace("'", "''").Replace("&amp;", "&") + "') and ProductId=" + ProductId + ""));
                }
                else if (options.ToString().ToLower() == "blackout lining")
                {
                    ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + options.ToString().ToLower().Replace("'", "''") + "' and ProductId=" + ProductId + ""));
                }
                else
                {
                    ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + options.ToString().ToLower().Replace("'", "''") + "' and ProductId=" + ProductId + ""));
                }
                Duties = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Duties,0) as Duties from tb_ProductRomanShadeYardage where  RomanShadeId=" + RomanShadeId + ""));
                //
                if (Duties > decimal.Zero)
                {
                    decimal ProductOptionsPrice1 = Duties / Convert.ToDecimal(100);
                    // decimal liningoption = (yardprice * ProductOptionsPrice) + yardprice;

                    //// ProductOptionsPrice = Convert.ToDecimal(15) / Convert.ToDecimal(100);
                    // //liningoption = liningoption * ProductOptionsPrice;
                    // //liningoption = liningoption + yardprice;

                    decimal liningoption = yardprice;
                    ProductOptionsPrice = Convert.ToDecimal(ProductOptionsPrice) / Convert.ToDecimal(100);
                    liningoption = liningoption * ProductOptionsPrice;
                    liningoption = liningoption + yardprice;
                    liningoption = (liningoption * ProductOptionsPrice1);
                    liningoption = liningoption + yardprice;
                    price = liningoption;
                }
                else
                {

                    decimal liningoption = yardprice;

                    ProductOptionsPrice = Convert.ToDecimal(ProductOptionsPrice) / Convert.ToDecimal(100);

                    liningoption = liningoption * ProductOptionsPrice;
                    liningoption = liningoption + yardprice;
                    price = liningoption;
                }
                Decimal orgPrice = 0;
                //objmini.Mode = 6;
                //objmini.VariantValue = fabricnameoptin.ToString().Trim();
                //Decimal.TryParse(objmini.GetProductvalues(), out orgPrice);
                orgPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT isnull(VariantPrice,0) FROM tb_ProductVariantValue WHERE VariantValue='" + fabricnameoptin.ToString().Trim() + "' and ProductId=" + ProductId + ""));
                Decimal dd = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Price,0) FROM tb_RomanPricewidthcalculation WHERE isnull(TypeName,'')='" + strFabricname.ToString() + "' and  isnull(Deleted,0)=0 and WidthTo >='" + WidthStdAllow + "' and WidthFrom <='" + WidthStdAllow + "' and isnull(FabricCode,'')<>'' and  isnull(FabricCode,'') in (SELECT isnull(FabricCode,'') FROM tb_ProductVariantValue WHERE VariantValueID=" + colorvalueid + ")"));
                orgPrice = orgPrice + dd;
                Decimal dd1 = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Price,0) FROM tb_RomanPricewidthcalculation WHERE isnull(TypeName,'')='" + strFabricname.ToString() + "' and  isnull(Deleted,0)=0 and WidthTo >=0 and WidthFrom <=0 and isnull(FabricCode,'')<>'' and  isnull(FabricCode,'') in (SELECT isnull(FabricCode,'') FROM tb_ProductVariantValue WHERE VariantValueID=" + colorvalueid + ") Order By RomanWidhId"));
                Decimal dd2 = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(Price,0) FROM tb_RomanPriceLengthcalculation WHERE isnull(TypeName,'')='" + strFabricname.ToString() + "' and  isnull(Deleted,0)=0 and LengthTo >='" + VariValue + "' and LengthFrom <='" + VariValue + "' and isnull(FabricCode,'')<>'' and  isnull(FabricCode,'') in (SELECT isnull(FabricCode,'') FROM tb_ProductVariantValue WHERE VariantValueID=" + colorvalueid + ")"));
                dd1 = dd2 - dd1;
                orgPrice = dd + dd1;
                if (ProductOptionsPrice > Decimal.Zero)
                {
                    ProductOptionsPrice = (ProductOptionsPrice * 100) * orgPrice / Convert.ToDecimal(100);
                    price = orgPrice + ProductOptionsPrice;
                }
                else
                {
                    price = orgPrice;
                }
                if (lift.ToString().ToLower().IndexOf("continuous") > -1 && AppLogic.AppConfigs("RomanLiftdefaultPrice") != null)
                {
                    price = price + Convert.ToDecimal(AppLogic.AppConfigs("RomanLiftdefaultPrice").ToString());
                }
                price = price;
            }
            return string.Format("{0:0.00}", price);
        }

        [System.Web.Services.WebMethod]
        public static string GetDataAdminMessage(Int32 ProductId, Int32 ProductType, Int32 Qty, string vValueid, string vNameid)
        {
            string resp1 = string.Empty;
            int pInventory = 0;
            bool outofstock = false;
            string StrErrorMsg = "";
            int Yardqty = 0;
            double actualYard = 0;
            String strVariantNames = String.Empty;
            String strVariantValues = String.Empty;
            Decimal price = Decimal.Zero;
            Int32 finalQty = Qty;
            string srrmessage = "";
            if (vValueid != null && vValueid.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
            {
                Qty = Qty * 2;
            }

            string AvailableDate = "";
            string[] Names2 = System.Text.RegularExpressions.Regex.Split(vNameid.ToString().Trim(), "divvariantname=");
            string[] Values2 = System.Text.RegularExpressions.Regex.Split(vValueid.ToString().Trim(), "divvariantname=");
            for (int i = 0; i < Names2.Length; i++)
            {
                if (Names2.Length > 0 && !string.IsNullOrEmpty(Names2[i].ToString().Trim()))
                {
                    strVariantNames += Names2[i].ToString().Trim();
                }
                if (Values2.Length > 0 && !string.IsNullOrEmpty(Values2[i].ToString().Trim()))
                {
                    strVariantValues += Values2[i].ToString().Trim();
                }
            }

            #region Variant


            #endregion

            //string ProductType = Convert.ToString(CommonComponent.GetScalarCommonData("Select (case when ISNULL(IsCustom,0) = 1 then 2 when ISNULL(IsRoman,0)=1 then 3 else 1 end) As ProductType from tb_Product where Productid=" + ProductID + " and StoreID=" + AppLogic.AppConfigs("Storeid").ToString() + ""));

            DataSet ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_product WHERE Productid=" + ProductId.ToString() + "");// ProductComponent.GetProductDetailByID(Convert.ToInt32(ProductID), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "2") // Made to Measure
                {
                    string FabricCode = Convert.ToString(ds.Tables[0].Rows[0]["FabricCode"]);
                    string FabricType = Convert.ToString(ds.Tables[0].Rows[0]["FabricType"]);
                    Int32 FabricTypeID = 0; if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
                    {
                        FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
                        if (FabricTypeID > 0)
                        {
                            DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
                            Int32 QtyOnHand = 0, NextOrderQty = 0, TotalQty = 0;
                            Int32 OrderQty = Convert.ToInt32(Qty);

                            if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
                                    QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);

                                if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
                                    NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
                            }
                            Int32 ShoppingCartQty = 0;
                            if (HttpContext.Current.Session["CustID"] != null)
                            {

                                ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Quantity,0) FROM tb_ShoppingCartItems " +
                                          " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + Convert.ToInt32(HttpContext.Current.Session["CustID"].ToString()) + " Order By ShoppingCartID) " +
                                          " AND ProductID=" + Convert.ToInt32(ProductId) + " AND VariantNames like '" + strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-") + "%' AND VariantValues like '" + strVariantValues.ToString().Replace("'", "''") + "%'"));
                            }
                            OrderQty = OrderQty + ShoppingCartQty;

                            TotalQty = QtyOnHand + NextOrderQty;

                            try
                            {
                                string Style = "";
                                double Width = 0;
                                double Length = 0;
                                string Options = "";
                                string[] strNmyard = strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] strValyeard = strVariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (strNmyard.Length > 0)
                                {
                                    if (strValyeard.Length == strNmyard.Length)
                                    {
                                        for (int j = 0; j < strNmyard.Length; j++)
                                        {
                                            if (strNmyard[j].ToString().ToLower() == "width")
                                            {
                                                if (strValyeard[j].ToString().ToLower().IndexOf("$") > -1)
                                                {
                                                    double.TryParse(strValyeard[j].ToString().Substring(0, strValyeard[j].ToString().IndexOf("(")), out Width);
                                                }
                                                else
                                                {
                                                    Width = Convert.ToDouble(strValyeard[j].ToString());
                                                }
                                            }
                                            if (strNmyard[j].ToString().ToLower() == "length")
                                            {
                                                if (strValyeard[j].ToString().ToLower().IndexOf("$") > -1)
                                                {
                                                    double.TryParse(strValyeard[j].ToString().Substring(0, strValyeard[j].ToString().IndexOf("(")), out Length);
                                                }
                                                else { Length = Convert.ToDouble(strValyeard[j].ToString()); }
                                            }
                                            if (strNmyard[j].ToString().ToLower() == "options")
                                            {
                                                Options = Convert.ToString(strValyeard[j].ToString());
                                            }
                                            if (strNmyard[j].ToString().ToLower() == "header")
                                            {
                                                Style = Convert.ToString(strValyeard[j].ToString());
                                            }
                                        }
                                    }
                                }
                                string resp = "";
                                //  if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "" && Options != "")
                                if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "")
                                {
                                    DataSet dsYard = new DataSet();
                                    dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductId + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
                                    if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                    {
                                        resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                        actualYard = Convert.ToDouble(resp.ToString());
                                    }
                                }
                                if (resp != "")
                                {
                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                }
                            }
                            catch
                            {
                            }
                            Yardqty = OrderQty;
                            //if (TotalQty > 0 && OrderQty > TotalQty)
                            //{
                            //    outofstock = true;
                            //}
                            //else
                            //{
                            //    if (OrderQty > QtyOnHand)
                            //    {
                            //        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()) && dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString().ToLower().IndexOf("1900") <= -1)
                            //        {
                            //            Int32 TotalDays = 0;
                            //            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
                            //            {
                            //                TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
                            //            }
                            //            DateTime dtnew = Convert.ToDateTime(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()).AddDays(TotalDays);
                            //            AvailableDate = Convert.ToString(dtnew);
                            //        }

                            //    }
                            //    else
                            //    {
                            //        Int32 TotalDays = 0;
                            //        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
                            //        {
                            //            TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
                            //        }
                            //        DateTime dtnew = DateTime.Now.Date.AddDays(TotalDays);
                            //        AvailableDate = Convert.ToString(dtnew);
                            //    }
                            //}

                            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + Yardqty + "," + ProductId + " "));
                            srrmessage = Yardqty.ToString() + " " + ProductId.ToString();
                            if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                            {
                                //outofstock = true;
                                AvailableDate = StrVendor;
                            }
                            else
                            {
                                AvailableDate = StrVendor;
                            }
                        }
                    }
                }
                else
                {
                    Int32 ShoppingCartQty = 0;
                    if (HttpContext.Current.Session["CustID"] != null)
                    {
                        ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
                                                   " WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + HttpContext.Current.Session["CustID"].ToString() + ") " +
                                                   " AND ProductID=" + ProductId + ""));
                    }
                    Qty = Qty + ShoppingCartQty;
                    DataSet dscount = new DataSet();
                    dscount = CommonComponent.GetCommonDataSet("SELECT 1 FROM tb_product WHERE ProductId=" + ProductId + " AND Inventory >= " + Qty + "");
                    if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
                    {
                    }
                    else
                    {
                        ///   outofstock = true;
                    }
                    //if (CheckInventory(Convert.ToInt32(ProductId), Convert.ToInt32(HdnCustID.Value.ToString()), Convert.ToInt32(Qty)))
                    //{ }
                    //else
                    //{
                    //    outofstock = true;
                    //}
                }
            }

            if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "1" && AvailableDate == "") // Made to Measure
            {
                DateTime dtnew = DateTime.Now.Date.AddDays(12);
                AvailableDate = Convert.ToString(dtnew);

                //string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + Qty + "," + ProductId + " "));
                //if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                //{
                //    AvailableDate = StrVendor;
                //}
                //else
                //{
                //    AvailableDate = StrVendor;
                //}
                string strvalue = "";
                string[] strNmyard = strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] strValyeard = strVariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (strNmyard.Length > 0)
                {
                    if (strValyeard.Length == strNmyard.Length)
                    {
                        for (int j = 0; j < strNmyard.Length; j++)
                        {
                            if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                            {
                                if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                {
                                    strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                }
                                else
                                {
                                    strvalue = strValyeard[j].ToString();
                                }
                                break;
                            }
                        }
                    }
                }

                int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''").Replace("(Buy 1 Get 1 Free)", "").Replace("(Final Sales)", "") + "' AND  ProductId=" + ProductId + ""));
                string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + ProductId.ToString() + ""));
                string strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue.Replace("'", "''").Replace("(Buy 1 Get 1 Free)", "").Replace("(Final Sales)", "") + "' AND ProductId=" + ProductId + ""));
                if (CntInv < Qty)
                {
                    if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder))
                    {
                        string resp = "";
                        DataSet dsYard = new DataSet();
                        string Length = "";
                        if (strvalue.ToLower().IndexOf("x84l") > -1 || strvalue.ToLower().IndexOf("x 84l") > -1 || strvalue.ToLower().IndexOf("x84 l") > -1 || strvalue.ToLower().IndexOf("x 84 l") > -1)
                        {
                            Length = "84";
                        }
                        else if (strvalue.ToLower().IndexOf("x96l") > -1 || strvalue.ToLower().IndexOf("x 96l") > -1 || strvalue.ToLower().IndexOf("x96 l") > -1 || strvalue.ToLower().IndexOf("x 96 l") > -1)
                        {
                            Length = "96";
                        }
                        else if (strvalue.ToLower().IndexOf("x108l") > -1 || strvalue.ToLower().IndexOf("x 108l") > -1 || strvalue.ToLower().IndexOf("x108 l") > -1 || strvalue.ToLower().IndexOf("x 108 l") > -1)
                        {
                            Length = "108";
                        }
                        else if (strvalue.ToLower().IndexOf("x120") > -1 || strvalue.ToLower().IndexOf("x 120l") > -1 || strvalue.ToLower().IndexOf("x120 l") > -1 || strvalue.ToLower().IndexOf("x 120 l") > -1)
                        {
                            Length = "120";
                        }
                        dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductId + ",50," + Length.ToString() + "," + Qty.ToString() + ",'Pole Pocket','Lined'");
                        if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                        {
                            resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                        }
                        Int32 OrderQty = Qty;
                        if (resp != "")
                        {
                            OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                        }
                        string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProductId.ToString() + " "));

                        if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                        {
                            AvailableDate = StrVendor.ToString();
                        }
                        else
                        {
                            AvailableDate = StrVendor.ToString();
                        }
                    }
                    else
                    {
                        //outofstock = true;
                    }
                }
                if (!string.IsNullOrEmpty(strDatenew))
                {
                    AvailableDate = strDatenew.ToString();
                }

            }
            if (outofstock)
            {
                StrErrorMsg = "Not Sufficient Inventory";
            }


            if (outofstock == false)
            {
                decimal LengthStdAllow = 0;

                String[] Names = strVariantNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                String[] Values = strVariantValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                string VariantNameId = "";
                string VariantValueId = "";
                string avail = "";
                if (strVariantNames != null && !string.IsNullOrEmpty(strVariantNames.ToString()))
                {
                    if (Names.Length > 0)
                    {
                        if (Values.Length == Names.Length)
                        {
                            //int RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(RomanShadeId,0) as RomanShadeId from tb_product where ProductId=" + Convert.ToInt32(ProductId) + " and StoreId=1"));
                            int RomanShadeId = 0;
                            for (int pp = 0; pp < Names.Length; pp++)
                            {
                                if (Names[pp].ToString().ToLower().IndexOf("roman shade design") > -1)
                                {
                                    RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select TOp 1 ISNULL(RomanShadeId,0) as RomanShadeId from tb_ProductRomanShadeYardage where ShadeName='" + Values[pp].ToString().Trim() + "' AND isnull(Active,0)=1"));
                                    break;
                                }
                            }
                            decimal VariValue = 0, WidthStdAllow = 0;
                            for (int k = 0; k < Names.Length; k++)
                            {
                                VariantNameId = VariantNameId + Names[k].ToString() + ",";
                                VariantValueId = VariantValueId + Values[k].ToString() + ",";

                                // Roman Yardage ---------------------------------
                                if (RomanShadeId > 0 && !string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "3")
                                {
                                    if (Names[k].ToString().ToLower().Trim().IndexOf("width") > -1)
                                    {
                                        //if (Values[k].ToString().ToLower().IndexOf("$") > -1)
                                        //{
                                        //    decimal.TryParse(Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("(")), out WidthStdAllow);
                                        //}
                                        //else
                                        //{
                                        //    decimal.TryParse(Values[k].ToString(), out WidthStdAllow);
                                        //}
                                        if (Values[k].ToString().IndexOf("/") > -1)
                                        {
                                            string strwidth = Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("/"));
                                            strwidth = strwidth.Replace("/", "");
                                            decimal tt = Convert.ToDecimal(strwidth);
                                            strwidth = Convert.ToString(Values[k].ToString()).Replace(strwidth + "/", "");
                                            tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                            decimal WidthStdAllow1 = tt;
                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;
                                        }
                                        else
                                        {
                                            decimal WidthStdAllow1 = 0;
                                            decimal.TryParse(Values[k].ToString(), out WidthStdAllow1);
                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;

                                        }

                                    }
                                    if (Names[k].ToString().ToLower().Trim().IndexOf("length") > -1)
                                    {
                                        if (VariValue > Convert.ToDecimal(1))
                                        {
                                            if (Values[k].ToString().IndexOf("/") > -1)
                                            {
                                                string strwidth = Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("/"));
                                                strwidth = strwidth.Replace("/", "");
                                                decimal tt = Convert.ToDecimal(strwidth);
                                                strwidth = Convert.ToString(Values[k].ToString()).Replace(strwidth + "/", "");
                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                decimal VariValue1 = tt;
                                                VariValue = VariValue + VariValue1;
                                            }
                                            else
                                            {
                                                decimal VariValue1 = 0;
                                                decimal.TryParse(Values[k].ToString(), out VariValue1);
                                                VariValue = VariValue + VariValue1;

                                            }


                                            //if (Values[k].ToString().ToLower().IndexOf("$") > -1)
                                            //{
                                            //    decimal.TryParse(Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("(")), out VariValue);
                                            //}
                                            //else
                                            //{
                                            //    decimal.TryParse(Values[k].ToString(), out VariValue);
                                            //}
                                            decimal tempWidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + WidthStdAllow + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                            string strFabricname = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 ShadeName from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + ""));
                                            if (tempWidthStdAllow > 0)
                                            {
                                                if (strFabricname.ToString().ToLower().Trim() == "casual")
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                else if (strFabricname.ToString().ToLower().Trim() == "relaxed")
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(2 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                    //LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                else if (strFabricname.ToString().ToLower().Trim() == "soft fold")
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(2 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                else if (strFabricname.ToString().ToLower().Trim() == "front slat")
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+(1.75 * " + VariValue + "))*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                else
                                                {
                                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                }
                                                //LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                LengthStdAllow = Math.Round(LengthStdAllow, 2);
                                                Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(LengthStdAllow)));
                                                Yardqty = Yardqty * finalQty;

                                            }
                                            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + ProductId.ToString() + ",'" + strVariantNames.ToString().Replace("~hpd~", "-") + "','" + strVariantValues.ToString().Replace("~hpd~", "-") + "'"));

                                            if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                            {
                                                AvailableDate = StrVendor;
                                            }
                                            else
                                            {
                                                AvailableDate = StrVendor;
                                            }
                                        }
                                        else
                                        {
                                            decimal VariValue1 = 0;
                                            decimal.TryParse(Values[k].ToString(), out VariValue1);
                                            VariValue = VariValue + VariValue1;
                                        }
                                    }
                                }
                                if (AvailableDate.ToString() != "" && !string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "2")
                                {
                                    avail = Convert.ToString(AvailableDate);
                                    try
                                    {
                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                    }
                                    catch
                                    {
                                    }
                                }
                                if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "1")
                                {
                                    if (avail == "")
                                    {
                                        avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProductId + "  AND VariantValue='" + Values[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + Qty + ""));
                                        if (avail == "")
                                        {
                                            avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProductId + "  AND VariantValue='" + Values[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + Qty + ""));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                if (LengthStdAllow > 0)
                {
                    VariantNameId = VariantNameId + "Yardage Required,";
                    VariantValueId = VariantValueId + LengthStdAllow.ToString() + ",";
                }
                if (AvailableDate != "")
                {
                    avail = AvailableDate;
                    try
                    {
                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                    }
                    catch
                    {

                    }
                }
                if (avail != "")
                {
                    //  avail = "Approx. Delivery: " + avail.ToString();
                    avail = "Estimated Delivery: " + avail.ToString();

                }
                //if (LengthStdAllow > 0)
                //{
                //    resp1 = avail + " Yardage Required: " + LengthStdAllow.ToString();
                //}
                //else
                //{
                resp1 = avail;
                //}
            }
            else
            {
                //if (srrmessage != "")
                //{
                //    resp1 = srrmessage;
                //}
                //else
                //{
                resp1 = "";
                //}
            }


            return resp1;
        }

        //public static string GetDataAdminMessage(Int32 ProductId, Int32 ProductType, Int32 Qty, string vValueid, string vNameid)
        //{
        //    string resp1 = string.Empty;
        //    int pInventory = 0;
        //    bool outofstock = false;
        //    string StrErrorMsg = "";
        //    int Yardqty = 0;
        //    double actualYard = 0;
        //    String strVariantNames = String.Empty;
        //    String strVariantValues = String.Empty;
        //    Decimal price = Decimal.Zero;
        //    Int32 finalQty = Qty;

        //    if (vValueid != null && vValueid.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
        //    {
        //        Qty = Qty * 2;
        //    }

        //    string AvailableDate = "";
        //    string[] Names2 = System.Text.RegularExpressions.Regex.Split(vNameid.ToString().Trim(), "divvariantname=");
        //    string[] Values2 = System.Text.RegularExpressions.Regex.Split(vValueid.ToString().Trim(), "divvariantname=");
        //    for (int i = 0; i < Names2.Length; i++)
        //    {
        //        if (Names2.Length > 0 && !string.IsNullOrEmpty(Names2[i].ToString().Trim()))
        //        {
        //            strVariantNames += Names2[i].ToString().Trim();
        //        }
        //        if (Values2.Length > 0 && !string.IsNullOrEmpty(Values2[i].ToString().Trim()))
        //        {
        //            strVariantValues += Values2[i].ToString().Trim();
        //        }
        //    }

        //    #region Variant


        //    #endregion

        //    //string ProductType = Convert.ToString(CommonComponent.GetScalarCommonData("Select (case when ISNULL(IsCustom,0) = 1 then 2 when ISNULL(IsRoman,0)=1 then 3 else 1 end) As ProductType from tb_Product where Productid=" + ProductID + " and StoreID=" + AppLogic.AppConfigs("Storeid").ToString() + ""));

        //    DataSet ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_product WHERE Productid=" + ProductId.ToString() + "");// ProductComponent.GetProductDetailByID(Convert.ToInt32(ProductID), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "2") // Made to Measure
        //        {
        //            string FabricCode = Convert.ToString(ds.Tables[0].Rows[0]["FabricCode"]);
        //            string FabricType = Convert.ToString(ds.Tables[0].Rows[0]["FabricType"]);
        //            Int32 FabricTypeID = 0; if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
        //            {
        //                FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
        //                if (FabricTypeID > 0)
        //                {
        //                    DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
        //                    Int32 QtyOnHand = 0, NextOrderQty = 0, TotalQty = 0;
        //                    Int32 OrderQty = Convert.ToInt32(Qty);

        //                    if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
        //                    {
        //                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
        //                            QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);

        //                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
        //                            NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
        //                    }
        //                    Int32 ShoppingCartQty = 0;
        //                    if (HttpContext.Current.Session["CustID"] != null)
        //                    {

        //                        ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Quantity,0) FROM tb_ShoppingCartItems " +
        //                                  " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + Convert.ToInt32(HttpContext.Current.Session["CustID"].ToString()) + " Order By ShoppingCartID) " +
        //                                  " AND ProductID=" + Convert.ToInt32(ProductId) + " AND VariantNames like '" + strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-") + "%' AND VariantValues like '" + strVariantValues.ToString().Replace("'", "''") + "%'"));
        //                    }
        //                    OrderQty = OrderQty + ShoppingCartQty;

        //                    TotalQty = QtyOnHand + NextOrderQty;

        //                    try
        //                    {
        //                        string Style = "";
        //                        double Width = 0;
        //                        double Length = 0;
        //                        string Options = "";
        //                        string[] strNmyard = strVariantNames.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //                        string[] strValyeard = strVariantValues.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //                        if (strNmyard.Length > 0)
        //                        {
        //                            if (strValyeard.Length == strNmyard.Length)
        //                            {
        //                                for (int j = 0; j < strNmyard.Length; j++)
        //                                {
        //                                    if (strNmyard[j].ToString().ToLower() == "width")
        //                                    {
        //                                        if (strValyeard[j].ToString().ToLower().IndexOf("$") > -1)
        //                                        {
        //                                            double.TryParse(strValyeard[j].ToString().Substring(0, strValyeard[j].ToString().IndexOf("(")), out Width);
        //                                        }
        //                                        else
        //                                        {
        //                                            Width = Convert.ToDouble(strValyeard[j].ToString());
        //                                        }
        //                                    }
        //                                    if (strNmyard[j].ToString().ToLower() == "length")
        //                                    {
        //                                        if (strValyeard[j].ToString().ToLower().IndexOf("$") > -1)
        //                                        {
        //                                            double.TryParse(strValyeard[j].ToString().Substring(0, strValyeard[j].ToString().IndexOf("(")), out Length);
        //                                        }
        //                                        else { Length = Convert.ToDouble(strValyeard[j].ToString()); }
        //                                    }
        //                                    if (strNmyard[j].ToString().ToLower() == "options")
        //                                    {
        //                                        Options = Convert.ToString(strValyeard[j].ToString());
        //                                    }
        //                                    if (strNmyard[j].ToString().ToLower() == "header")
        //                                    {
        //                                        Style = Convert.ToString(strValyeard[j].ToString());
        //                                    }
        //                                }
        //                            }
        //                        }
        //                        string resp = "";
        //                        if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "" && Options != "")
        //                        {
        //                            DataSet dsYard = new DataSet();
        //                            dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductId + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
        //                            if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
        //                            {
        //                                resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
        //                                actualYard = Convert.ToDouble(resp.ToString());
        //                            }
        //                        }
        //                        if (resp != "")
        //                        {
        //                            OrderQty = Convert.ToInt32(Qty) * Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
        //                        }
        //                    }
        //                    catch
        //                    {
        //                    }
        //                    Yardqty = OrderQty;
        //                    if (TotalQty > 0 && OrderQty > TotalQty)
        //                    {
        //                        outofstock = true;
        //                    }
        //                    else
        //                    {
        //                        if (OrderQty > QtyOnHand)
        //                        {
        //                            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()) && dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString().ToLower().IndexOf("1900") <= -1)
        //                            {
        //                                Int32 TotalDays = 0;
        //                                if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
        //                                {
        //                                    TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
        //                                }
        //                                DateTime dtnew = Convert.ToDateTime(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()).AddDays(TotalDays);
        //                                AvailableDate = Convert.ToString(dtnew);
        //                            }

        //                        }
        //                        else
        //                        {
        //                            Int32 TotalDays = 0;
        //                            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
        //                            {
        //                                TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
        //                            }
        //                            DateTime dtnew = DateTime.Now.Date.AddDays(TotalDays);
        //                            AvailableDate = Convert.ToString(dtnew);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Int32 ShoppingCartQty = 0;
        //            if (HttpContext.Current.Session["CustID"] != null)
        //            {
        //                ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
        //                                           " WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + HttpContext.Current.Session["CustID"].ToString() + ") " +
        //                                           " AND ProductID=" + ProductId + ""));
        //            }
        //            Qty = Qty + ShoppingCartQty;
        //            DataSet dscount = new DataSet();
        //            dscount = CommonComponent.GetCommonDataSet("SELECT 1 FROM tb_product WHERE ProductId=" + ProductId + " AND Inventory >= " + Qty + "");
        //            if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
        //            {
        //            }
        //            else
        //            {
        //                outofstock = true;
        //            }
        //            //if (CheckInventory(Convert.ToInt32(ProductId), Convert.ToInt32(HdnCustID.Value.ToString()), Convert.ToInt32(Qty)))
        //            //{ }
        //            //else
        //            //{
        //            //    outofstock = true;
        //            //}
        //        }
        //    }

        //    if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "1" && AvailableDate == "") // Made to Measure
        //    {
        //        DateTime dtnew = DateTime.Now.Date.AddDays(12);
        //        AvailableDate = Convert.ToString(dtnew);
        //    }
        //    if (outofstock)
        //    {
        //        StrErrorMsg = "Not Sufficient Inventory";
        //    }


        //    if (outofstock == false)
        //    {
        //        decimal LengthStdAllow = 0;

        //        String[] Names = strVariantNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //        String[] Values = strVariantValues.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

        //        string VariantNameId = "";
        //        string VariantValueId = "";
        //        string avail = "";
        //        if (strVariantNames != null && !string.IsNullOrEmpty(strVariantNames.ToString()))
        //        {
        //            if (Names.Length > 0)
        //            {
        //                if (Values.Length == Names.Length)
        //                {
        //                    int RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(RomanShadeId,0) as RomanShadeId from tb_product where ProductId=" + Convert.ToInt32(ProductId) + " and StoreId=" + AppConfig.StoreID + ""));
        //                    decimal VariValue = 0, WidthStdAllow = 0;
        //                    for (int k = 0; k < Names.Length; k++)
        //                    {
        //                        VariantNameId = VariantNameId + Names[k].ToString() + ",";
        //                        VariantValueId = VariantValueId + Values[k].ToString() + ",";

        //                        // Roman Yardage ---------------------------------
        //                        if (RomanShadeId > 0 && !string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "3")
        //                        {
        //                            if (Names[k].ToString().ToLower().Trim().IndexOf("width") > -1)
        //                            {
        //                                if (Values[k].ToString().ToLower().IndexOf("$") > -1)
        //                                {
        //                                    decimal.TryParse(Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("(")), out WidthStdAllow);
        //                                }
        //                                else
        //                                {
        //                                    decimal.TryParse(Values[k].ToString(), out WidthStdAllow);
        //                                }
        //                            }
        //                            if (Names[k].ToString().ToLower().Trim().IndexOf("length") > -1)
        //                            {
        //                                if (Values[k].ToString().ToLower().IndexOf("$") > -1)
        //                                {
        //                                    decimal.TryParse(Values[k].ToString().Substring(0, Values[k].ToString().IndexOf("(")), out VariValue);
        //                                }
        //                                else
        //                                {
        //                                    decimal.TryParse(Values[k].ToString(), out VariValue);
        //                                }
        //                                decimal tempWidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + WidthStdAllow + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
        //                                if (tempWidthStdAllow > 0)
        //                                {
        //                                    LengthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select Round(ISNULL((Sum(ISNULL(LengthStandardAllowance,0)+" + VariValue + ")*(" + tempWidthStdAllow + "))/36,0),2) as LengthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
        //                                    LengthStdAllow = Math.Round(LengthStdAllow, 2);
        //                                }
        //                            }
        //                        }
        //                        if (AvailableDate.ToString() != "" && !string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "2")
        //                        {
        //                            avail = Convert.ToString(AvailableDate);
        //                            try
        //                            {
        //                                avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
        //                            }
        //                            catch
        //                            {
        //                            }
        //                        }
        //                        if (!string.IsNullOrEmpty(ProductType.ToString()) && ProductType.ToString() == "1")
        //                        {
        //                            if (avail == "")
        //                            {
        //                                avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProductId + "  AND VariantValue='" + Values[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + Qty + ""));
        //                                if (avail == "")
        //                                {
        //                                    avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProductId + "  AND VariantValue='" + Values[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + Qty + ""));
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        if (LengthStdAllow > 0)
        //        {
        //            VariantNameId = VariantNameId + "Yardage Required,";
        //            VariantValueId = VariantValueId + LengthStdAllow.ToString() + ",";
        //        }
        //        if (AvailableDate != "")
        //        {
        //            avail = AvailableDate;
        //            avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
        //        }
        //        if (avail != "")
        //        {
        //            avail = "Estimated Date: " + avail.ToString();

        //        }
        //        if (LengthStdAllow > 0)
        //        {
        //            resp1 = avail + " Yardage Required: " + LengthStdAllow.ToString();
        //        }
        //        else
        //        {
        //            resp1 = avail;
        //        }
        //    }
        //    else
        //    {
        //        resp1 = "";
        //    }


        //    return resp1;
        //}

        //[System.Web.Services.WebMethod]
        //public static string GetDataAvaildate(Int32 ProductId, Int32 Width, Int32 Length, Int32 Qty, string style, string options, Int32 ProductType)
        //{

        //}


        [System.Web.Services.WebMethod]
        public static string Getvariantdata(Int32 ProductId, Int32 variantvalueId)
        {
            string resp = string.Empty;
            DataSet ds = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            DataSet dsVariantparent = new DataSet();
            if (variantvalueId > 0)
            {
                dsVariantparent = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + ProductId + " AND isnull(ParentId,0)=" + variantvalueId.ToString() + " Order By DisplayOrder");
            }
            if (dsVariantparent != null && dsVariantparent.Tables.Count > 0 && dsVariantparent.Tables[0].Rows.Count > 0)
            {
                resp += "<select onchange=\"PriceChangeondropdown();\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" style=\"width: auto !important;\" class=\"option1\" id=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" >";
                resp += "<option value=\"0\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "</option>";
                DataSet dsVariantvaluechild = new DataSet();
                dsVariantvaluechild = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariantValue WHERE ProductID=" + ProductId + " AND VariantID=" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + " Order By DisplayOrder");
                if (dsVariantvaluechild != null && dsVariantvaluechild.Tables.Count > 0 && dsVariantvaluechild.Tables[0].Rows.Count > 0)
                {
                    for (int k = 0; k < dsVariantvaluechild.Tables[0].Rows.Count; k++)
                    {
                        if (dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString().ToLower().IndexOf("custom") > -1)
                        {

                        }
                        else
                        {
                            string strPrice = "";
                            if (Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString()) > Decimal.Zero)
                            {
                                strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString())) + ")";
                            }
                            resp += "<option value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + strPrice + "</option>";
                        }
                    }
                }
                resp += "</select>";
            }
            else
            {
                resp += "<select  name=\"Selectvariant-0\" style=\"width: auto !important;\" class=\"option1\" id=\"Selectvariant-0\" >";
                resp += "<option value=\"0\">None</option>";
                resp += "</select>";
            }

            //ds = objSql.GetDs("EXEC usp_Product_Pricecalculator " + ProductId + "," + Width.ToString() + "," + Length.ToString() + "," + Qty.ToString() + "");
            //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            //{
            //    //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
            //    resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0][0].ToString()));
            //}

            return resp;
        }

        [System.Web.Services.WebMethod]
        public static string GeLimitMessage(Int32 PId)
        {

            String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + System.Web.HttpContext.Current.Session["CustId"].ToString() + " Order By ShoppingCartID DESC) "));
            //String strswatchQtyy ="2";
            string strDi = "";
            //strDi += "<div style=\"float;left;width:100%;line-height:25px;\">Sample Added</div>";
            //strDi += "<div style=\"float;left;width:100%;\"><hr /></div>";
            //if (strswatchQtyy != "" && Convert.ToInt32(strswatchQtyy) > 1)
            //{
            //    strDi += "<div style=\"float;left;width:100%;line-height:25px;\">" + strswatchQtyy.ToString() + " Samples Selected. (" + AppLogic.AppConfigs("SwatchMaxlength").ToString() + " Max)</div>";
            //}
            //else
            //{
            //    strDi += "<div style=\"float;left;width:100%;line-height:25px;\">" + strswatchQtyy.ToString() + " Sample Selected. (" + AppLogic.AppConfigs("SwatchMaxlength").ToString() + " Max)</div>";
            //}
            //strDi += "<div style=\"float;left;width:100%;\"><hr /></div>";
            //strDi += "<div style=\"float;left;width:100%;\"><input tytpe=\"button\" value=\"Countinue\" />&nbsp;<input tytpe=\"button\" value=\"Order\" /></div>";
            //strDi += "</div>";

            strDi += "<div class=\"swatch-popup-main\">";
            strDi += "<div class=\"swatch-popup-box\">";
            strDi += "<span>Select up to $" + string.Format("{0:0}", Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString())) + " worth of swatches on us!</span>";
            if (strswatchQtyy != "" && Convert.ToInt32(strswatchQtyy) > 1)
            {
                // strDi += "<p>" + strswatchQtyy + " Samples Selected. (Up to $" + string.Format("{0:0.00}", Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString())) + " Free Samples)</p>";
            }
            else
            {
                // strDi += "<p>" + strswatchQtyy + " Sample Selected. (Up to $" + string.Format("{0:0.00}", Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString())) + " Free Samples)</p>";
            }
            strDi += "<p>Swatch prices vary. $" + string.Format("{0:0}", Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString())) + " cannot be applied toward shipping. All swatch orders will have a $5.99 shipping charge added to your total.</p>";
            strDi += "<div class=\"swatch-order-btn\">";
            strDi += "<a href=\"javascript:void(0);\" onclick=\"window.parent.disablePopup();\"><img src=\"/images/continue-shopping.png\" alt=\"Continue Shopping\" title=\"Continue Shopping\"></a>&nbsp;&nbsp;";
            strDi += "<a href=\"javascript:void(0);\" onclick=\"window.parent.location.href='/CheckoutCommon.aspx';\" ><img src=\"/images/compelet-my-order.png\" alt=\"Complete My Order\" title=\"Complete My Order\"></a>";
            strDi += "</div>";
            strDi += "</div>";
            strDi += "</div>";


            return strDi;
        }


        [System.Web.Services.WebMethod]
        public static string Getvariantdataforrdo(Int32 ProductId, Int32 variantvalueId, Int32 RowIndex, bool IsRoman)
        {
            Int32 CntReadymade = 1;
            string resp = string.Empty;
            DataSet ds = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            DataSet dsVariantparent = new DataSet();
            if (variantvalueId > 0)
            {
                dsVariantparent = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + ProductId + " AND isnull(ParentId,0)=" + variantvalueId.ToString() + " Order By DisplayOrder");
            }
            if (dsVariantparent != null && dsVariantparent.Tables.Count > 0 && dsVariantparent.Tables[0].Rows.Count > 0)
            {

                //resp += "<select onchange=\"PriceChangeondropdown();\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" style=\"width: auto !important;\" class=\"option1\" id=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" >";
                //resp += "<option value=\"0\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "</option>";

                DataSet dsVariantvaluechild = new DataSet();
                dsVariantvaluechild = CommonComponent.GetCommonDataSet("SELECT * ,isnull(AllowQuantity,0) as aQty,isnull(LockQuantity,0) as lQty  FROM tb_ProductVariantValue WHERE ProductID=" + ProductId + " AND VariantID=" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + " Order By DisplayOrder");
                if (dsVariantvaluechild != null && dsVariantvaluechild.Tables.Count > 0 && dsVariantvaluechild.Tables[0].Rows.Count > 0)
                {
                    Int32 ichk = 0;
                    for (int k = 0; k < dsVariantvaluechild.Tables[0].Rows.Count; k++)
                    {
                        string StrVarValueId = Convert.ToString(dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString());
                        string StrQry = "";
                        Int32 Intcnt = 0;
                        string StrBuy1onsale = "";
                        bool IsOnSale = false;
                        decimal OnsalePrice = 0;

                        if (CntReadymade == 1)
                        {
                            StrBuy1onsale = "";
                            StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ProductId + " and (Convert(char(10),Buy1Fromdate,101) <=  Convert(char(10),GETDATE(),101) and Convert(char(10),Buy1Todate,101) >=convert(char(10),GETDATE(),101)) and ISNULL(Buy1Get1,0)=1 and VariantValueID=" + StrVarValueId + "";
                            Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                            if (Intcnt > 0)
                            {
                                StrBuy1onsale = " (Buy 1 Get 1 Free)";
                            }

                            StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ProductId + " and (Convert(char(10),OnSaleFromdate,101) <=  Convert(char(10),GETDATE(),101) and Convert(char(10),OnSaleTodate,101) >=convert(char(10),GETDATE(),101)) and ISNULL(OnSale,0)=1 and VariantValueID=" + StrVarValueId + "";
                            Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                            if (Intcnt > 0)
                            {
                                IsOnSale = true;
                                OnsalePrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(OnSalePrice,0) as OnSalePrice from tb_ProductVariantValue Where productid=" + ProductId + " and VariantValueID=" + StrVarValueId + " and ISNULL(OnSale,0)=1"));
                                StrBuy1onsale += " (Final Sales)";
                            }
                        }
                        if (dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString().ToLower().IndexOf("custom") > -1)
                        {

                        }
                        else
                        {

                            int AllwQty = 0;
                            Int32.TryParse(dsVariantvaluechild.Tables[0].Rows[k]["aQty"].ToString().ToString(), out AllwQty);
                            int LockQty = 0;
                            Int32.TryParse(dsVariantvaluechild.Tables[0].Rows[k]["lQty"].ToString().ToString(), out LockQty);
                            string StrAlowLockqty = "";

                            if (AllwQty > 0 && LockQty > 0)
                                StrAlowLockqty = "Allow Quantity: <span style='color:red;'>" + dsVariantvaluechild.Tables[0].Rows[k]["aQty"].ToString().ToString() + "</span>   Lock Quantity: <span style='color:red;'>" + dsVariantvaluechild.Tables[0].Rows[k]["lQty"].ToString().ToString() + "</span>";
                            else if (AllwQty > 0)
                                StrAlowLockqty = "Allow Quantity: <span style='color:red;'>" + dsVariantvaluechild.Tables[0].Rows[k]["aQty"].ToString().ToString() + "</span>";
                            else if (LockQty > 0)
                                StrAlowLockqty = "Lock Quantity: <span style='color:red;'>" + dsVariantvaluechild.Tables[0].Rows[k]["lQty"].ToString().ToString() + "</span>";
                            else
                                StrAlowLockqty = "";

                            string strPrice = "";
                            if (IsOnSale == true && OnsalePrice > decimal.Zero)
                            {
                                strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(OnsalePrice.ToString())) + ")";
                            }
                            else
                            {
                                if (Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString()) > Decimal.Zero)
                                {
                                    strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString())) + ")";
                                }
                            }

                            if (ichk == 0)
                            {
                                if (IsRoman == true)
                                {
                                    resp += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\" checked=\"checked\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                }
                                else
                                {
                                    resp += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdown(" + RowIndex + ");\" checked=\"checked\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                }
                            }
                            else
                            {
                                if (IsRoman == true)
                                {
                                    resp += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdownforroman(" + RowIndex + ");\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                }
                                else
                                {
                                    resp += "<input type=\"radio\" title=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" onchange=\"PriceChangeondropdown(" + RowIndex + ");\" id=\"Selectvariant-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "<br /><input type=\"hidden\" id=\"hdnallowquantity-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\" value=\"" + StrAlowLockqty.ToString().Trim() + "\" />";
                                }
                            }
                            ichk++;
                        }
                    }
                }
            }
            else
            {
                resp += "<input type=\"radio\" value=\"0\">None<br />";
            }
            return resp;
        }

        public void SendMail(Int32 OrderNumber)
        {

            string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
            string Body = "";
            string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + "invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
            WebRequest NewWebReq = WebRequest.Create(url);
            WebResponse newWebRes = NewWebReq.GetResponse();
            string format = newWebRes.ContentType;
            Stream ftprespstrm = newWebRes.GetResponseStream();
            StreamReader reader;
            reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
            Body = reader.ReadToEnd().ToString();
            Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");

            AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");

            try
            {
                CommonOperations.SendMail(ToID, "New Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
            }
            catch { }
            try
            {
                if (ViewState["BillEmail"] != null)
                {
                    CommonOperations.SendMail(ViewState["BillEmail"].ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                }
                else
                {
                    CommonOperations.SendMail("brijeshs@kaushalam.com", "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                }
            }
            catch { }
        }

        public void SendMail(string MailTo, string body, AlternateView Attachment)
        {
            //String host = "198.143.131.34";
            //String username = "sales@redtagsecuritycameras.com";
            //String password = "SlE#4123AzU";
            //String FromID = "sales@redtagsecuritycameras.com";

            String host = AppLogic.AppConfigs("Host");
            String username = AppLogic.AppConfigs("MailUserName");
            String password = AppLogic.AppConfigs("MailPassword");
            String FromID = AppLogic.AppConfigs("MailFrom");

            lblUsername.Text = host + "- " + username + "- " + password + "- " + FromID;

            MailMessage Msg = new MailMessage();
            SmtpClient MailObj = new SmtpClient(host);
            MailObj.UseDefaultCredentials = false;
            MailObj.Credentials = new System.Net.NetworkCredential(username, password);


            MailObj.DeliveryMethod = SmtpDeliveryMethod.Network;
            Msg.From = new MailAddress(FromID, FromID);
            String[] MailID = MailTo.Split(';');
            for (Int32 i = 0; i < MailID.Length; i++)
                Msg.To.Add(new MailAddress(MailID[i].ToString()));
            Msg.Subject = "Test Mail";
            Msg.Body = body;
            Msg.IsBodyHtml = true;
            if (Attachment != null)
                Msg.AlternateViews.Add(Attachment);
            try
            {
                MailObj.Send(Msg);
                lblMesage.Text = "send successfully";
            }

            catch (Exception ex)
            {

                lblError.Text = ex.Message;

            }

        }
        [System.Web.Services.WebMethod]
        public static string ChangePhoneOrderPrice(Int32 CustomCartID, String price)
        {
            CommonComponent.ExecuteCommonData("update tb_ShoppingCartItems set Price='" + price + "' where CustomCartID=" + CustomCartID + "");
            return "";
        }
        [System.Web.Services.WebMethod]
        public static string ChangeBackorderdate(String VariantName, String VariantValue, Int32 ProductID)
        {
            string[] strNmyard = VariantName.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] strValyeard = VariantValue.ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string strDatenew = "";
            if (strValyeard.Length > 0)
            {
                if (strValyeard.Length == strNmyard.Length)
                {
                    for (int j = 0; j < strNmyard.Length; j++)
                    {
                        if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                        {
                            string strvalue = "";
                            if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                            {
                                strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                            }
                            else
                            {
                                strvalue = strValyeard[j].ToString().Trim();
                            }
                            strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(Final Sales)", "");
                            strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                            //if(strDatenew.ToString().Length > 0)
                            //{
                            //    strDatenew = "Back Order Date: " + strDatenew.ToString();
                            //}
                        }
                    }
                }
            }
            return strDatenew;
        }

        [System.Web.Services.WebMethod]
        public static string GetUrl(String Colorname, String Patternname)
        {
            Page obj = new Page();
            obj.Session["IndexPriceValue"] = null;
            obj.Session["IndexFabricValue"] = null;
            obj.Session["IndexPatternValue"] = null;
            obj.Session["IndexStyleValue"] = null;
            obj.Session["IndexColorValue"] = null;
            obj.Session["IndexHeaderValue"] = null;
            obj.Session["IndexCustomValue"] = null;
            string Url = "/ProductSearchList.aspx";

            if (!string.IsNullOrEmpty(Colorname) && Colorname != "")
            {
                obj.Session["IndexColorValue"] = Colorname.ToString();
            }


            string header = "";
            if (!string.IsNullOrEmpty(Patternname) && Patternname != "")
            {
                obj.Session["IndexPatternValue"] = Patternname.ToString() + ",";
                header = Patternname.ToString();
            }


            if (obj.Session["IndexPriceValue"] == null && obj.Session["IndexFabricValue"] == null && obj.Session["IndexPatternValue"] == null && obj.Session["IndexStyleValue"] == null && obj.Session["IndexColorValue"] == null && obj.Session["IndexHeaderValue"] == null && obj.Session["IndexCustomValue"] == null)
            {

            }
            else
            {
                string strUrl = "";
                if (obj.Session["IndexColorValue"] != null)
                {
                    strUrl = CommonOperations.RemoveSpecialCharacter(obj.Session["IndexColorValue"].ToString().Trim().ToCharArray()).ToLower() + ".html";
                    Url = strUrl;
                }
                else if (!string.IsNullOrEmpty(header))
                {
                    strUrl = CommonOperations.RemoveSpecialCharacter(header.ToString().Trim().ToCharArray()).ToLower() + ".html";

                    Url = strUrl;
                }
                else
                {

                }



            }
            return Url;

        }


        [System.Web.Services.WebMethod]
        public static string GetDataRomanshadeprice(Int32 ProductId, string Width, string Width2, Int32 Qty, string options, string Length, string Length2, string fabricnameoptin, string colorvalueid)
        {
            decimal VariValue = 0, WidthStdAllow = 0;
            decimal price = 0;
            if (Length != "")
            {
                VariValue = Convert.ToDecimal(Length);
            }
            if (Width != "")
            {
                WidthStdAllow = Convert.ToDecimal(Width);
            }
            //decimal LengthStdAllow = 0;
            //if (Length2.ToString().IndexOf("/") > -1)
            //{
            //    string strwidth = Length2.ToString().Substring(0, Length2.ToString().IndexOf("/"));
            //    strwidth = strwidth.Replace("/", "");
            //    decimal tt = Convert.ToDecimal(strwidth);
            //    strwidth = Convert.ToString(Length2.ToString()).Replace(strwidth + "/", "");
            //    tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
            //    decimal VariValue1 = tt;
            //    VariValue = VariValue + VariValue1;
            //}
            //else
            //{
            //    decimal VariValue1 = 0;
            //    decimal.TryParse(Length2.ToString(), out VariValue1);
            //    VariValue = VariValue + VariValue1;

            //}
            //if (Width2.ToString().IndexOf("/") > -1)
            //{
            //    string strwidth = Width2.ToString().Substring(0, Width2.ToString().IndexOf("/"));
            //    strwidth = strwidth.Replace("/", "");
            //    decimal tt = Convert.ToDecimal(strwidth);
            //    strwidth = Convert.ToString(Width2.ToString()).Replace(strwidth + "/", "");
            //    tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
            //    decimal WidthStdAllow1 = tt;
            //    WidthStdAllow = WidthStdAllow + WidthStdAllow1;
            //}
            //else
            //{
            //    decimal WidthStdAllow1 = 0;
            //    decimal.TryParse(Width2.ToString(), out WidthStdAllow1);
            //    WidthStdAllow = WidthStdAllow + WidthStdAllow1;

            //}

            Double shadesug = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeSuggestedRetail' and isnull(deleted,0)=0 and Storeid=1"));
            Double shademarkup = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));

            Double Price = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 value FROM tb_ShadeDetail WHERE ShadeWidthID in (SELECT ShadeWidthID FROM tb_Shadewidth WHERE value='" + Convert.ToDouble(Width.ToString()) + "' and ProductId=" + ProductId + ") and ShadeLengthID in(SELECT ShadeLengthID FROM tb_ShadeLength WHERE ProductId=" + ProductId + " and value='" + Convert.ToDouble(Length.ToString()) + "') "));
            Double actualprice = Price * shadesug;
            Double yourprice = Price * shademarkup;
            return "Your Price: $" + string.Format("{0:0.00}", yourprice) + "<br /><span>MSRP: <s>" + "$" + string.Format("{0:0.00}", actualprice) + "</s></span>";
        }
        [System.Web.Services.WebMethod]
        public static string GetDataRomanshadepriceAdmin(Int32 ProductId, string Width, string Width2, Int32 Qty, string options, string Length, string Length2, string fabricnameoptin, string colorvalueid)
        {
            decimal VariValue = 0, WidthStdAllow = 0;
            decimal price = 0;
            if (Length != "")
            {
                VariValue = Convert.ToDecimal(Length);
            }
            if (Width != "")
            {
                WidthStdAllow = Convert.ToDecimal(Width);
            }


            Double shadesug = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeSuggestedRetail' and isnull(deleted,0)=0 and Storeid=1"));
            Double shademarkup = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));

            Double Price = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 value FROM tb_ShadeDetail WHERE ShadeWidthID in (SELECT ShadeWidthID FROM tb_Shadewidth WHERE value='" + Convert.ToDouble(Width.ToString()) + "' and ProductId=" + ProductId + ") and ShadeLengthID in(SELECT ShadeLengthID FROM tb_ShadeLength WHERE value='" + Convert.ToDouble(Length.ToString()) + "' and ProductId=" + ProductId + ") "));
            Double actualprice = Price * shadesug;
            Double yourprice = Price * shademarkup;
            return string.Format("{0:0.00}", yourprice);
        }

        [System.Web.Services.WebMethod]
        public static string GetDataAdminMessageRomanShade(Int32 ProductId, Int32 ProductType, Int32 Qty, string vValueid, string vNameid)
        {
            string[] Names2 = System.Text.RegularExpressions.Regex.Split(vNameid.ToString().Trim(), "divvariantname=");
            string[] Values2 = System.Text.RegularExpressions.Regex.Split(vValueid.ToString().Trim(), "divvariantname=");
            string strVariantNames = "";
            string strVariantValues = "";
            for (int i = 0; i < Names2.Length; i++)
            {
                if (Names2.Length > 0 && !string.IsNullOrEmpty(Names2[i].ToString().Trim()))
                {
                    strVariantNames += Names2[i].ToString().Trim();
                }
                if (Values2.Length > 0 && !string.IsNullOrEmpty(Values2[i].ToString().Trim()))
                {
                    strVariantValues += Values2[i].ToString().Trim();
                }
            }

            string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Qty + "," + ProductId.ToString() + ",'" + strVariantNames.ToString().Replace("~hpd~", "-") + "','" + strVariantValues.ToString().Replace("~hpd~", "-") + "'"));
            string AvailableDate = "";
            if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
            {
                AvailableDate = StrVendor;
            }
            else
            {
                AvailableDate = StrVendor;
            }
            string avail = "";
            if (AvailableDate != "")
            {
                avail = AvailableDate;
                try
                {
                    avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                }
                catch
                {

                }
            }
            if (avail != "")
            {
                //  avail = "Approx. Delivery: " + avail.ToString();
                avail = "Estimated Delivery: " + avail.ToString();

            }
            return avail;
        }

        [System.Web.Services.WebMethod]
        public static string getCustompageimage(string strProductId)
        {
            DataSet dsproduct = new DataSet();
            string strimagename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Imagename FROM tb_Product WHERE ProductId=" + strProductId + ""));
            string strp = "";
            Random rd = new Random(1000);
            if (!String.IsNullOrEmpty(strimagename.ToString()))
            {

                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (AppLogic.AppConfigs("ImagePathProduct") + "medium/" + strimagename.ToString())))
                {
                    strp = AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/" + strimagename.ToString() + "?" + rd.Next(10000).ToString();

                }
                else
                {
                    strp = AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg?" + rd.Next(10000).ToString();
                }
            }
            else
            {
                strp = AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg?" + rd.Next(10000).ToString();
            }

            return strp;
        }
        [System.Web.Services.WebMethod]
        public static string getCustompage(string strProductId, string strFabriccategoryId, string fabric, string pattern, string color)
        {
            string strcustomstring = "<div id=\"divVariant\">";
            strcustomstring += "<div class=\"readymade-detail\">";
            string swatchid = "0";

            // Start


            strcustomstring += "<div class=\"readymade-detail-pt1\">";

            strcustomstring += "<div onclick=\"varianttabhideshowcustom(0);\" id=\"divcolspancustom-0\" class=\"readymade-detail-pt1-pro\">";
            strcustomstring += "<span id=\"spancolspancustom-0\">1</span>Select a Fabric:<strong style=\"font-weight:normal;color:#B92127; margin-left:5px;\" id=\"fabricnamestrg\">###dftfabric####</strong>";
            strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;display:none;\"><a style=\"color: #B92127\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
            strcustomstring += "</div>";

            strcustomstring += "<div style=\"display: none;\" id=\"divcolspancustomvalue-0\" class=\"custom-detail-row\">";
            strcustomstring += "<div class=\"custom-detail-row-1\">";

            strcustomstring += "<div class=\"custom-detail-row-1-detail\"> <span>Categories</span>";
            strcustomstring += "<div class=\"selector fixedWidth\"> <span id=\"ddlcustomcat\">" + strFabriccategoryId + "</span>";
            strcustomstring += "<select id=\"ContentPlaceHolder1_ddlfabriccategory\" class=\"option-2\" name=\"\" onchange=\"fabriccategorycomb();\">";
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                strcustomstring += "<option value=\"All\" selected>All</option>";
            }
            else
            {
                strcustomstring += "<option>All</option>";
            }


            DataSet dsgetall = new DataSet();
            dsgetall = CommonComponent.GetCommonDataSet("SELECT DISTINCT UPPER(FabricType) as FabricType  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'') in (SELECT FabricTypename FROM tb_ProductFabricType WHERE Isnull(Active,0)=1 and isnull(FabricTypename,'')<>'') Order BY UPPER(FabricType)");


            if (dsgetall != null && dsgetall.Tables.Count > 0 && dsgetall.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsgetall.Tables[0].Rows.Count; i++)
                {

                    if (string.IsNullOrEmpty(strFabriccategoryId))
                    {
                        strFabriccategoryId = dsgetall.Tables[0].Rows[i]["FabricType"].ToString();
                    }
                    //strcustomstring = strcustomstring.Replace("###dftfabric####", strFabriccategoryId);
                    if (strFabriccategoryId.ToString().ToLower().Trim() == dsgetall.Tables[0].Rows[i]["FabricType"].ToString().ToLower().Trim())
                    {
                        strcustomstring += "<option value=\"" + dsgetall.Tables[0].Rows[i]["FabricType"].ToString() + "\" selected>" + dsgetall.Tables[0].Rows[i]["FabricType"].ToString() + "</option>";
                    }
                    else
                    {
                        strcustomstring += "<option value=\"" + dsgetall.Tables[0].Rows[i]["FabricType"].ToString() + "\">" + dsgetall.Tables[0].Rows[i]["FabricType"].ToString() + "</option>";
                    }

                }

            }

            strcustomstring += "</select>";
            strcustomstring += "</div>";


            DataSet dsgetall1 = new DataSet();
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Fabric  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'') <>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Fabric,'')<>'' UNION  SELECT 'Other' as Fabric  UNION  SELECT 'Fabric' as Fabric) as A Order By case when  Fabric='Other' then 2 when  Fabric='Fabric' then 0 else 1 end");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Fabric  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'') <>'' and isnull(FabricType,'') ='" + strFabriccategoryId + "'  and isnull(deleted,0)=0 and StoreId=1 and isnull(Fabric,'')<>'' UNION  SELECT 'Other' as Fabric  UNION  SELECT 'Fabric' as Fabric) as A Order By case when  Fabric='Other' then 2 when  Fabric='Fabric' then 0 else 1 end");
                //dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Fabric  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and FabricType='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Fabric,'')<>'' UNION  SELECT 'Other' as Fabric  UNION  SELECT 'Fabric' as Fabric) as A Order By case when  Fabric='Other' then 2 when  Fabric='Fabric' then 0 else 1 end");
            }




            strcustomstring += "<div class=\"selector fixedWidth\"> <span id=\"ddlcustomfabri\">###dftfabricnew####</span>";
            strcustomstring += "<select id=\"ContentPlaceHolder1_ddlfabric\" class=\"option-2\" name=\"\" onchange=\"fabriccategory();\">";
            //strcustomstring += "<option>all</option>";



            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsgetall1.Tables[0].Rows.Count; i++)
                {

                    if (string.IsNullOrEmpty(fabric))
                    {
                        fabric = dsgetall1.Tables[0].Rows[i]["Fabric"].ToString();
                    }
                    strcustomstring = strcustomstring.Replace("###dftfabricnew####", fabric);
                    if (fabric.ToString().ToLower().Trim() == dsgetall1.Tables[0].Rows[i]["Fabric"].ToString().ToLower().Trim())
                    {
                        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + "\" selected>" + dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + "</option>";
                    }
                    else
                    {
                        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + "\">" + dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + "</option>";
                    }

                }

            }

            strcustomstring += "</select>";
            strcustomstring += "</div>";
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Pattern  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Pattern,'')<>'' UNION  SELECT 'Other' as Pattern  UNION  SELECT 'Pattern' as Pattern) as A Order By case when  Pattern='Other' then 2 when  Pattern='Pattern' then 0 else 1 end");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Pattern  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(FabricType,'') ='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Pattern,'')<>'' UNION  SELECT 'Other' as Pattern  UNION  SELECT 'Pattern' as Pattern) as A Order By case when  Pattern='Other' then 2 when  Pattern='Pattern' then 0 else 1 end");
                //dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Pattern  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Pattern,'')<>'' UNION  SELECT 'Other' as Pattern  UNION  SELECT 'Pattern' as Pattern) as A Order By case when  Pattern='Other' then 2 when  Pattern='Pattern' then 0 else 1 end");
            }




            strcustomstring += "<div class=\"selector fixedWidth\"> <span id=\"ddlcustompattern\">###ddlcustompattern####</span>";
            strcustomstring += "<select id=\"ContentPlaceHolder1_ddlcustompattern\" class=\"option-2\" name=\"\" onchange=\"fabriccategory();\">";
            //strcustomstring += "<option>all</option>";



            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsgetall1.Tables[0].Rows.Count; i++)
                {

                    if (string.IsNullOrEmpty(pattern))
                    {
                        pattern = dsgetall1.Tables[0].Rows[i]["Pattern"].ToString();
                    }
                    strcustomstring = strcustomstring.Replace("###ddlcustompattern####", pattern);
                    if (pattern.ToString().ToLower().Trim() == dsgetall1.Tables[0].Rows[i]["Pattern"].ToString().ToLower().Trim())
                    {
                        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + "\" selected>" + dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + "</option>";
                    }
                    else
                    {
                        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + "\">" + dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + "</option>";
                    }

                }

            }

            strcustomstring += "</select>";
            strcustomstring += "</div>";

            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT Colors+',' as SearchValue  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Colors,'')<>''");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT Colors+',' as SearchValue  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(FabricType,'') ='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Colors,'')<>''");
                //dsgetall1 = CommonComponent.GetCommonDataSet("SELECT Colors+',' as SearchValue  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Colors,'')<>''");
            }




            strcustomstring += "<div class=\"selector fixedWidth\"> <span id=\"ddlcustomcolor\">###ddlcustomcolor####</span>";
            strcustomstring += "<select id=\"ContentPlaceHolder1_ddlcustomcolor\" class=\"option-2\" name=\"\" onchange=\"fabriccategory();\">";
            //strcustomstring += "<option>all</option>";
            if (string.IsNullOrEmpty(color))
            {
                strcustomstring += "<option value=\"Color\" selected>Color</option>";
            }
            else
            {
                strcustomstring += "<option value=\"Color\">Color</option>";
            }

            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {
                string strtt = ",";
                for (int m = 0; m < dsgetall1.Tables[0].Rows.Count; m++)
                {



                    string[] strcolor = dsgetall1.Tables[0].Rows[m]["SearchValue"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (strcolor.Length > 0)
                    {
                        bool flg = false;
                        foreach (string stt in strcolor)
                        {
                            if (!string.IsNullOrEmpty(stt))
                            {


                                if (strtt.ToString().ToLower().Trim().IndexOf("," + stt.ToLower().Trim() + ",") <= -1)
                                {
                                    if (string.IsNullOrEmpty(color))
                                    {
                                        color = "Color";
                                        flg = true;
                                    }
                                    strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);
                                    if (color.ToString().ToLower().Trim() == stt.ToString().ToLower().Trim() && flg == false)
                                    {
                                        strcustomstring += "<option value=\"" + stt.ToString() + "\" selected>" + stt.ToString() + "</option>";
                                    }
                                    else
                                    {
                                        strcustomstring += "<option value=\"" + stt.ToString() + "\">" + stt.ToString() + "</option>";
                                    }
                                    strtt += stt.Trim() + ",";
                                }
                            }
                            //if (string.IsNullOrEmpty(color))
                            //{
                            //    color = dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString();
                            //}
                            //strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);
                            //if (color.ToString().ToLower().Trim() == dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim())
                            //{
                            //    strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "\" selected>" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                            //}
                            //else
                            //{
                            //    strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "\">" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                            //}
                        }
                    }
                    else
                    {

                    }
                }
                if (string.IsNullOrEmpty(color))
                {
                    color = "Color";
                }
                //color = "Color";
                strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);
                //for (int i = 0; i < dsgetall1.Tables[0].Rows.Count; i++)
                //{

                //    if (string.IsNullOrEmpty(color))
                //    {
                //        color = dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString();
                //    }
                //    strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);
                //    if (color.ToString().ToLower().Trim() == dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim())
                //    {
                //        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "\" selected>" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                //    }
                //    else
                //    {
                //        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "\">" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                //    }

                //}

            }
            else
            {


                color = "Color";
                strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);

            }
            strcustomstring += "<option value=\"Other\">Other</option>";
            strcustomstring += "</select>";
            strcustomstring += "</div>";



            strcustomstring += "</div>";
            strcustomstring += "</div>";



            strcustomstring += "<div class=\"custom-detail-row-2\">";


            strcustomstring += "<div class=\"select-febric-bg\" ><div style=\"float:left\"><a href=\"javascript:void(0);\" title=\"Previous\" class=\"nextfabric\"  ###displaypre### id=\"apreviousfabric\"><img title=\"Previous\" alt=\"Previous\" src=\"/images/arrow-1-20.png\"  style=\"width:20px;height:20px\" border=\"0\" /></a></div><div style=\"float:right\"><a href=\"javascript:void(0);\" title=\"Next\" class=\"previousfabric\" id=\"anextfabric\" ###displaynext###><img title=\"Next\" style=\"width:20px;height:20px\" alt=\"Next\" src=\"/images/arrow-2-20.png\" border=\"0\" /></a></div></div>";


            strcustomstring += "<div class=\"select-febric-bg\" style=\"height:450px;overflow-y:auto;\" id=\"divscroll\">";
            strcustomstring += "<ul id=\"ulfabricall1\">";

            DataSet dsswatch = new DataSet();
            Int32 num = 0;
            string strpricetag = "";

            string strwhereclause = "";

            if (!string.IsNullOrEmpty(fabric) && fabric.ToLower() != "fabric" && fabric.ToLower() != "other")
            {
                strwhereclause += " and Fabric='" + fabric + "'";
            }
            if (!string.IsNullOrEmpty(color) && color.ToLower() != "color" && color.ToLower() != "other")
            {
                strwhereclause += " and colors like '%" + color + "%'";
            }
            if (!string.IsNullOrEmpty(pattern) && pattern.ToLower() != "pattern" && pattern.ToLower() != "other")
            {
                strwhereclause += " and pattern ='" + pattern + "'";
            }
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsswatch = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID,SKu,ImageName,tb_product.Name,A.ProductID,A.ProductSwatchid,A.name as pname,SalePriceTag,TotalRows=COUNT(*) OVER() FROM tb_product INNER JOIN (SELECT  ProductSwatchid,ProductID,Name  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'') in (SELECT FabricTypename FROM tb_ProductFabricType WHERE Isnull(Active,0)=1 and isnull(FabricTypename,'')<>'') and isnull(FabricType,'')<>'' " + strwhereclause + ") AS A on tb_product.Productid=A.ProductSwatchid WHERE StoreId=1 and isnull(active,0)=1 and isnull(deleted,0)=0) AS A WHERE A.RowID>=0 and A.RowID <=20");
            }
            else
            {
                dsswatch = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID, SKu,ImageName,tb_product.Name,A.ProductID,A.ProductSwatchid,A.name as pname,SalePriceTag,TotalRows=COUNT(*) OVER()  FROM tb_product INNER JOIN (SELECT  ProductSwatchid,ProductID,Name  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'') in (SELECT FabricTypename FROM tb_ProductFabricType WHERE Isnull(Active,0)=1 and isnull(FabricTypename,'')<>'') and isnull(FabricType,'')='" + strFabriccategoryId.Replace("'", "''") + "' " + strwhereclause + ") AS A on tb_product.Productid=A.ProductSwatchid WHERE StoreId=1 and isnull(active,0)=1 and isnull(deleted,0)=0) AS A WHERE A.RowID>=0 and A.RowID <=20 ");
            }


            Int32 nextId = 0;
            if (dsswatch != null && dsswatch.Tables.Count > 0 && dsswatch.Tables[0].Rows.Count > 0)
            {
                Int32 RowId = Convert.ToInt32(dsswatch.Tables[0].Rows[dsswatch.Tables[0].Rows.Count - 1]["RowID"].ToString());
                Int32 Totalrows = Convert.ToInt32(dsswatch.Tables[0].Rows[dsswatch.Tables[0].Rows.Count - 1]["TotalRows"].ToString());
                if (Totalrows > RowId)
                {
                    nextId = RowId + 1;
                    strcustomstring = strcustomstring.Replace("###displaypre###", " style='display:none;'");
                    strcustomstring = strcustomstring.Replace("###displaynext###", " onclick='getallcolor(" + nextId + ");'");

                }
                else
                {
                    strcustomstring = strcustomstring.Replace("###displaypre###", " style='display:none;'");
                    strcustomstring = strcustomstring.Replace("###displaynext###", " style='display:none;'");
                }
                Int32 ic = 0;
                for (int i = 0; i < dsswatch.Tables[0].Rows.Count; i++)
                {
                    if (!String.IsNullOrEmpty(dsswatch.Tables[0].Rows[i]["Imagename"].ToString()))
                    {
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString())))
                        {
                            if (ic == 0 && strProductId == "0")
                            {


                                strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                //strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            else
                            {
                                if (strProductId == dsswatch.Tables[0].Rows[i]["ProductID"].ToString())
                                {
                                    strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                    swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                    strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                                    strcustomstring = strcustomstring.Replace("###dftfabric####", dsswatch.Tables[0].Rows[i]["Pname"].ToString());
                                }
                                else
                                {
                                    strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                }

                                //strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            ic++;
                            if (strProductId == "0")
                            {
                                strProductId = dsswatch.Tables[0].Rows[i]["ProductID"].ToString();
                                swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                                strcustomstring = strcustomstring.Replace("###dftfabric####", dsswatch.Tables[0].Rows[i]["Pname"].ToString());
                            }
                        }
                        else
                        {
                            if (ic == 0 && strProductId == "0")
                            {


                                strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                //strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            else
                            {
                                if (strProductId == dsswatch.Tables[0].Rows[i]["ProductID"].ToString())
                                {
                                    strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                    swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                    strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                                    strcustomstring = strcustomstring.Replace("###dftfabric####", dsswatch.Tables[0].Rows[i]["Pname"].ToString());
                                }
                                else
                                {
                                    strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                }

                                //strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            ic++;
                            if (strProductId == "0")
                            {
                                strProductId = dsswatch.Tables[0].Rows[i]["ProductID"].ToString();
                                swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                                strcustomstring = strcustomstring.Replace("###dftfabric####", dsswatch.Tables[0].Rows[i]["Pname"].ToString());
                            }
                        }
                    }
                }


            }
            if (string.IsNullOrEmpty(strpricetag))
            {
                strpricetag = "(Per Panel)";
            }

            strcustomstring = strcustomstring.Replace("###dftfabric####", "");


            strcustomstring += "</ul>";
            strcustomstring += "<div style=\"float:left:width:100%;text-align:center;display:none;\" id=\"divscrollloader\"><img src=\"/images/fabric_loader.gif\" width=\"24px\" />";
            strcustomstring += "</div>";

            strcustomstring += "</div>";
            if (!string.IsNullOrEmpty(swatchid) && swatchid.ToString() != "0")
            {
                strcustomstring += "<div class=\"next-step\"><img border=\"0\" onclick=\"varianttabhideshowcustom(1);\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" src=\"/images/next-option.png\"></div>";
            }
            strcustomstring += "</div>";

            strcustomstring += "</div>";




            strcustomstring += "</div>";










            ///END


            // Start

            strcustomstring += "<div id=\"otherproduct\">";
            if (!string.IsNullOrEmpty(swatchid) && swatchid.ToString() != "0")
            {


                strcustomstring += "<div class=\"readymade-detail-pt1\">";
                strcustomstring += "<div onclick=\"varianttabhideshowcustom(1);\" id=\"divcolspancustom-1\" class=\"readymade-detail-pt1-pro\">";
                strcustomstring += "<span id=\"spancolspancustom-1\">2</span>Select Header";
                strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" onclick=\"ShowModelForPleatGuide();\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
                strcustomstring += "</div>";


                strcustomstring += "<div style=\"display: none;padding-left:4% !important;padding-top:10px;width:95% !important\" id=\"divcolspancustomvalue-1\" class=\"readymade-detail-right-pro custom-detail-row\">";
                strcustomstring += "<select style=\"width: 200px !important; display: none;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomstyle\" name=\"ctl00$ContentPlaceHolder1$ddlcustomstyle\">";
                strcustomstring += "<option value=\"0\">Header</option>";


                DataSet dsstyle = new DataSet();
                dsstyle = CommonComponent.GetCommonDataSet("SELECT Style as SearchValue,tb_ProductSearchType.displayorder from tb_ProductStylePrice INNER JOIN tb_ProductSearchType ON tb_ProductSearchType.SearchValue=tb_ProductStylePrice.Style  where tb_ProductStylePrice.productId=" + strProductId.ToString() + " and isnull(tb_ProductStylePrice.Active,0)=1 AND isnull(tb_ProductSearchType.Deleted,0)=0 AND tb_ProductSearchType.SearchType=6 Order By tb_ProductSearchType.displayorder");


                strcustomstring += "###option###";

                //strcustomstring += "<option value=\"Pole Pocket\">Pole Pocket</option>";
                //strcustomstring += "<option value=\"French Pleat\">French Pleat</option>";
                //strcustomstring += "<option value=\"Inverted Pleat\">Inverted Pleat</option>";
                //strcustomstring += "<option value=\"Parisian Pleat\">Parisian Pleat</option>";
                //strcustomstring += "<option value=\"Goblet Pleat\">Goblet Pleat</option>";

                strcustomstring += "</select>";
                // strcustomstring += "<div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Pole Pocket','divcustomflat-radio-001-1','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-1\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Pole Pocket','divcustomflat-radio-001-1','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Pole Pocket</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','French Pleat','divcustomflat-radio-001-2','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-2\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','French Pleat','divcustomflat-radio-001-2','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;French Pleat</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Inverted Pleat','divcustomflat-radio-001-3','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-3\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Inverted Pleat','divcustomflat-radio-001-3','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Inverted Pleat</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Parisian Pleat','divcustomflat-radio-001-4','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-4\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Parisian Pleat','divcustomflat-radio-001-4','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Parisian Pleat</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Goblet Pleat','divcustomflat-radio-001-5','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-5\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Goblet Pleat','divcustomflat-radio-001-5','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Goblet Pleat</div></div>";

                string strnewoption = "";
                if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
                {





                    for (int i = 0; i < dsstyle.Tables[0].Rows.Count; i++)
                    {

                        strcustomstring += "<div style=\"float: left; width: 30%; margin-bottom: 5px;margin-left:5px;\">";
                        strnewoption += "<option value=\"" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "\">" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                        strcustomstring += "<div style=\"float:left;width:100%;\">";

                        //strcustomstring += "<img src=\"/images/roman_inside_mount.png\">";
                        if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "pole pocket")
                        {
                            strcustomstring += "<img src=\"/images/pole_pocket_header.jpg\" title=\"Pole Pocket\" alt=\"Pole Pocket\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "french pleat")
                        {
                            strcustomstring += "<img src=\"/images/French_Pleat.jpg\" title=\"French Pleat\" alt=\"French Pleat\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "inverted pleat")
                        {
                            strcustomstring += "<img src=\"/images/Inverted_Pleat.jpg\" title=\"Inverted Pleat\" alt=\"Inverted Pleat\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "parisian pleat")
                        {
                            strcustomstring += "<img src=\"/images/Parisian_Pleat.jpg\" title=\"Parisian Pleat\" alt=\"Parisian Pleat\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "goblet pleat")
                        {
                            strcustomstring += "<img src=\"/images/Goblet_Pleat.jpg\" title=\"Goblet Pleat\" alt=\"Goblet Pleat\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "grommet pleat")
                        {
                            strcustomstring += "<img src=\"/images/Grommet_Pleat.jpg\" title=\"Goblet Pleat\" alt=\"Goblet Pleat\" style=\"width:163px;\">";
                        }
                        else
                        {
                            strcustomstring += "<img src=\"/images/pole_pocket_header.jpg\" title=\"Pole Pocket\" alt=\"Pole Pocket\" style=\"width:163px;\">";
                        }

                        strcustomstring += "</div>";
                        strcustomstring += "<div style=\"margin-left:22%;\"><div style=\"float:left;width:100%;margin-bottom:5px;\" class='item-radio-display' onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "','divcustomflat-radio-001-" + i.ToString() + "','001');ChangeCustomprice();\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "','divcustomflat-radio-001-" + i.ToString() + "','001');ChangeCustomprice();\" id=\"divcustomflat-radio-001-" + i.ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "</div></div></div>";
                        strcustomstring += "</div>";
                    }


                }
                strcustomstring = strcustomstring.Replace("###option###", strnewoption);

                //strcustomstring += "<div style=\"float: right; width: 100%; margin-bottom: 5px; text-align: right;\">";
                //strcustomstring += "<img border=\"0\" onclick=\"if(document.getElementById('ContentPlaceHolder1_ddlcustomstyle') != null &amp;&amp; document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex == 0){jAlert('Please Select Header.','Message');}else{varianttabhideshowcustom(2);}\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" src=\"/images/next-option.png\">";
                //strcustomstring += "</div>";

                strcustomstring += "<div style=\"float: right; width: 95%;text-align: right;padding:5px 6px 5px 0;\"><img border=\"0\" onclick=\"if(document.getElementById('ContentPlaceHolder1_ddlcustomstyle') != null && document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex == 0){jAlert('Please Select Header.','Message');}else{varianttabhideshowcustom(2);}\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" src=\"/images/next-option.png\"></div>";
                strcustomstring += "</div>";
                strcustomstring += "<div id=\"spancolspancustomvalue-1\" style=\"margin-top: 10px; display: none;\">";
                strcustomstring += "</div>";
                strcustomstring += "</div>";






                ///END







                strcustomstring += "<div class=\"readymade-detail-pt1\">";
                strcustomstring += "<div onclick=\"varianttabhideshowcustom(2);\" id=\"divcolspancustom-2\" class=\"readymade-detail-pt1-pro\">";
                strcustomstring += "<span id=\"spancolspancustom-2\">3</span>Select size";
                strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" onclick=\"variantDetail('divMakeOrderWidth');\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
                strcustomstring += "</div>";

                strcustomstring += "<div style=\"display: none;padding-left:4% !important;width:95% !important;padding-top:10px !important\" id=\"divcolspancustomvalue-2\" class=\"readymade-detail-right-pro custom-detail-row\">";
                strcustomstring += "<div style=\"float: left; width: 30%; margin-bottom: 5px;\">";
                strcustomstring += "<div style=\"float:left;width:100%;\"><a onclick=\"variantDetail('divMakeOrderWidth');\" href=\"javascript:void(0);\" title=\"Size\"><img src=\"/images/curtain-size.png\" alt=\"Size\" title=\"Size\"></a></div>";
                strcustomstring += "</div>";

                strcustomstring += "<div class=\"selector fixedWidth\">";
                strcustomstring += "<span id=\"ddlcustomwidth-ddl\" style=\"-moz-user-select: none;\">Width</span>";
                strcustomstring += "<select style=\"width: 200px !important;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomwidth\" name=\"ctl00$ContentPlaceHolder1$ddlcustomwidth\">";
                strcustomstring += "<option value=\"Width\" selected=\"selected\">Width</option>";
                for (int i = 25; i < 201; i++)
                {
                    strcustomstring += "<option value=\"" + i.ToString() + "\">" + i.ToString() + "" + "\"" + "</option>";
                }

                strcustomstring += "</select>";
                strcustomstring += "</div>";

                //strcustomstring += "<div class=\"selector fixedWidth\"><span id=\"spanvariant-exact-0\" style=\"-moz-user-select: none;\">00</span><select id=\"Selectvariant-exact-0\" name=\"Selectvariant-exact-0\" onchange=\"PriceChangeondropdown();\" class=\"option1\"><option style=\"display: none;\" value=\"0\">Extra width</option><option selected=\"true\" value=\"00\">00</option><option value=\"1/8\">1/8</option><option value=\"1/4\">1/4</option><option value=\"3/8\">3/8</option><option value=\"1/2\">1/2</option><option value=\"5/8\">5/8</option><option value=\"3/4\">3/4</option><option value=\"7/8\">7/8</option></select></div>";

                strcustomstring += "<div class=\"selector fixedWidth\">";
                strcustomstring += "<span id=\"ddlcustomlength-ddl\" style=\"-moz-user-select: none;\">Length</span>";
                strcustomstring += "<select style=\"width: 200px !important;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomlength\" name=\"ctl00$ContentPlaceHolder1$ddlcustomlength\">";
                strcustomstring += "<option value=\"\" selected=\"selected\">Length</option>";
                for (int i = 45; i < 276; i++)
                {
                    strcustomstring += "<option value=\"" + i.ToString() + "\">" + i.ToString() + "" + "\"" + "</option>";
                }

                strcustomstring += "</select>";
                strcustomstring += "</div>";
                //strcustomstring += "<div class=\"selector fixedWidth\"><span id=\"spanvariant-exact-99999999\" style=\"-moz-user-select: none;\">00</span><select id=\"Selectvariant-exact-0\" name=\"Selectvariant-exact-0\"  class=\"option1\"><option style=\"display: none;\" value=\"0\">Extra width</option><option selected=\"true\" value=\"00\">00</option><option value=\"1/8\">1/8</option><option value=\"1/4\">1/4</option><option value=\"3/8\">3/8</option><option value=\"1/2\">1/2</option><option value=\"5/8\">5/8</option><option value=\"3/4\">3/4</option><option value=\"7/8\">7/8</option></select></div>";
                strcustomstring += "<div style=\"float: right; width: 100%; margin-bottom: 5px; text-align: right;\">";
                strcustomstring += "<img border=\"0\" onclick=\"if(document.getElementById('ContentPlaceHolder1_ddlcustomwidth') != null &amp;&amp; document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex == 0){jAlert('Please Select Width.','Message');}else if(document.getElementById('ContentPlaceHolder1_ddlcustomlength') != null &amp;&amp; document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex == 0){jAlert('Please Select Length.','Message');}else{varianttabhideshowcustom(3);}\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" id=\"ContentPlaceHolder1_imgnextoption\" src=\"/images/next-option.png\">";
                strcustomstring += "</div>";
                strcustomstring += "</div>";


                strcustomstring += "<div id=\"spancolspancustomvalue-222\" style=\"margin-top: 10px; display: none;\">";
                strcustomstring += "</div>";
                strcustomstring += "</div>";

                //strcustomstring += "</div>";


                //strcustomstring += "<div style=\"display: none;\" class=\"readymade-detail-pt1\">";

                //strcustomstring += "<div style=\"display: none;\" onclick=\"varianttabhideshowcustom(3);\" id=\"divcolspancustom-333\" class=\"readymade-detail-pt1-pro\">";
                //strcustomstring += "<span id=\"spancolspancustom-333\">4</span>Select Length";
                //strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" onclick=\"variantDetail('divMakeOrderLength');\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
                //strcustomstring += "</div>";

                //strcustomstring += "<div style=\"display: none;\" id=\"divcolspancustomvalue-333\" class=\"readymade-detail-right-pro \">";
                //strcustomstring += "</div>";
                //strcustomstring += "<div id=\"spancolspancustomvalue-333\" style=\"margin-top: 10px; display: none;\">";
                //strcustomstring += "</div>";

                // strcustomstring += "</div>";


                dsstyle = new DataSet();
                dsstyle = CommonComponent.GetCommonDataSet(@"SELECT case when isnull(tb_ProductOptionsPrice.AdditionalPrice,0)=0 then tb_ProductOptionsPrice.Options else tb_ProductOptionsPrice.Options+'($'+cast(Round(tb_ProductOptionsPrice.AdditionalPrice,2) as varchar(10))+')'  end as name, tb_ProductOptionsPrice.ProductId, tb_ProductSearchType.DisplayOrder
FROM dbo.tb_ProductOptionsPrice INNER JOIN dbo.tb_ProductSearchType ON dbo.tb_ProductOptionsPrice.Options = dbo.tb_ProductSearchType.SearchValue WHERE isnull(tb_ProductOptionsPrice.Active,0)=1 AND tb_ProductOptionsPrice.ProductId=" + strProductId.ToString() + @" Order By tb_ProductSearchType.DisplayOrder");
                strnewoption = "";
                Int32 inum = 4;
                if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
                {


                    strcustomstring += "<div class=\"readymade-detail-pt1\">";
                    strcustomstring += "<div onclick=\"varianttabhideshowcustom(3);\" id=\"divcolspancustom-3\" class=\"readymade-detail-pt1-pro custom-detail-row\">";
                    strcustomstring += "<span id=\"spancolspancustom-3\">4</span>Select Options";
                    strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" onclick=\"variantDetail('divMakeOrderOptions');\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
                    strcustomstring += "</div>";

                    strcustomstring += "<div id=\"divcolspancustomvalue-3\" class=\"readymade-detail-right-pro custom-detail-row\" style=\"padding-left:4% !important;width:95% !important;display:none;padding-top:10px !important\">";

                    strcustomstring += "<select style=\"width: 200px !important; display: none;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomoptin\" name=\"ctl00$ContentPlaceHolder1$ddlcustomoptin\">";
                    strcustomstring += "<option value=\"0\">Options</option>";
                    //strcustomstring += "<option value=\"Lined\">Lined</option>";
                    //strcustomstring += "<option value=\"Lined &amp; Interlined\">Lined &amp; Interlined</option>";
                    //strcustomstring += "<option value=\"Blackout Lining\">Blackout Lining</option>";
                    strcustomstring += "###option1###";
                    strcustomstring += "</select>";


                    for (int i = 0; i < dsstyle.Tables[0].Rows.Count; i++)
                    {
                        strnewoption += "<option value=\"" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "\">" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "</option>";
                        if (i == 0)
                        {
                            strcustomstring += "<div style=\"float:left;margin-bottom:5px;margin-left:5px;\" class='item-radio-display' onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\" id=\"divcustomflat-radio-004-" + i.ToString() + "\"></div><div style=\"margin-top: 2px;float:left;\" class='item-radio-display-text'>&nbsp;" + dsstyle.Tables[0].Rows[i]["name"].ToString().Replace(" ", "&nbsp;") + "</div></div>";
                        }
                        else
                        {
                            strcustomstring += "<div style=\"float:left;margin-bottom:5px;margin-left:50px;\" class='item-radio-display' onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\" id=\"divcustomflat-radio-004-" + i.ToString() + "\"></div><div style=\"margin-top: 2px;float:left;\" class='item-radio-display-text'>&nbsp;" + dsstyle.Tables[0].Rows[i]["name"].ToString().Replace(" ", "&nbsp;") + "</div></div>";
                        }

                    }
                    strcustomstring = strcustomstring.Replace("###option1###", strnewoption);
                    //strcustomstring += "<div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined','divcustomflat-radio-004-1','004');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-1\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined','divcustomflat-radio-004-1','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Lined</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined &amp; Interlined','divcustomflat-radio-004-2','004');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-2\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined &amp; Interlined','divcustomflat-radio-004-2','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Lined &amp; Interlined</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Blackout Lining','divcustomflat-radio-004-3','004');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-3\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Blackout Lining','divcustomflat-radio-004-3','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Blackout Lining</div></div>";
                    strcustomstring += "<div style=\"float: right; width: 100%; margin-bottom: 5px; text-align: right;display:none;\">";
                    strcustomstring += "<img border=\"0\" onclick=\"if(document.getElementById('ContentPlaceHolder1_ddlcustomoptin') != null &amp;&amp; document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex == 0){jAlert('Please Select Options.','Message');}else{varianttabhideshowcustom(4);}\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" src=\"/images/next-option.png\">";
                    strcustomstring += "</div>";
                    strcustomstring += "</div>";

                    strcustomstring += " <div id=\"spancolspancustomvalue-3\" style=\"margin-top: 10px;\">";
                    strcustomstring += "</div>";
                    strcustomstring += " </div>";
                    //strcustomstring += " </div>";
                    inum++;
                }
                strcustomstring += " </div>";
                //strcustomstring += "<div style=\"display:none;\" class=\"readymade-detail-pt1\">";
                //strcustomstring += "<div onclick=\"varianttabhideshowcustom(4);\" id=\"divcolspancustom-4\" class=\"readymade-detail-pt1-pro\">";
                //strcustomstring += "<span id=\"spancolspancustom-4\"></span>Quantity (Panels):1";
                //strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px;\" id=\"divselvaluecustom-4\"></div>";
                //strcustomstring += "</div>";

                //strcustomstring += "<div style=\"display: none;\" id=\"divcolspancustomvalue-4\" class=\"readymade-detail-right-pro\">";



                //strcustomstring += "</div>";
                //strcustomstring += "</div>";

                strcustomstring += " <div class=\"readymade-detail-pt1\" id=\"addtocartdivnew\">";

                strcustomstring += "<div   id=\"divcaddtocart\" class=\"readymade-detail-pt1-pro active\">";
                strcustomstring += "<span id=\"spanaddtocart\">" + inum.ToString() + "</span>ADD TO CART";
                strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"></div>";
                strcustomstring += "</div>";

                strcustomstring += "<div  class=\"readymade-detail-right-pro custom-detail-row\">";
                strcustomstring += "<div style=\"margin-left: 4%;\" class=\"price-detail-left\">";
                strcustomstring += "<div class=\"item-pricepro\">";
                strcustomstring += " <div class=\"item-pricepro-left\">";

                strcustomstring += "<div class=\"quantit-pro\" style=\"font-size:14px !important;\"> <span>Quantity :</span>";
                strcustomstring += "<div id=\"uniform-lift_pos_select_right\" class=\"selector fixedWidth\">";
                strcustomstring += " <span id=\"dlcustomqty-ddl\" style=\"-moz-user-select: none;\">1</span>";
                strcustomstring += " <select style=\"width: 200px !important;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_dlcustomqty\" name=\"ctl00$ContentPlaceHolder1$dlcustomqty\">";
                strcustomstring += "<option value=\"\">Quantity</option>";
                strcustomstring += "<option value=\"1\" selected=\"selected\">1</option>";
                strcustomstring += "<option value=\"2\">2</option>";
                strcustomstring += "<option value=\"3\">3</option>";
                strcustomstring += "<option value=\"4\">4</option>";
                strcustomstring += "<option value=\"5\">5</option>";
                strcustomstring += "<option value=\"6\">6</option>";
                strcustomstring += "<option value=\"7\">7</option>";
                strcustomstring += "<option value=\"8\">8</option>";
                strcustomstring += "<option value=\"9\">9</option>";
                strcustomstring += "<option value=\"10\">10</option>";
                strcustomstring += "<option value=\"11\">11</option>";
                strcustomstring += "<option value=\"12\">12</option>";
                strcustomstring += "<option value=\"13\">13</option>";
                strcustomstring += "<option value=\"14\">14</option>";
                strcustomstring += "<option value=\"15\">15</option>";
                strcustomstring += "<option value=\"16\">16</option>";
                strcustomstring += "<option value=\"17\">17</option>";
                strcustomstring += "<option value=\"18\">18</option>";
                strcustomstring += "<option value=\"19\">19</option>";
                strcustomstring += "<option value=\"20\">20</option>";

                strcustomstring += "</select>";
                strcustomstring += "</div>";
                strcustomstring += "</div>";
                strcustomstring += "<div class=\"quantit-pro\"  style=\"font-size:14px !important;\">";
                strcustomstring += "<div id=\"divcustomprice\" class=\"selector fixedWidth regularprice-pro\" style=\"background:none;\"><label style=\"float: left;\">$0.00</label><span style=\"float: left; background: none repeat scroll 0% 0% transparent;width:auto;\">" + strpricetag + "</span>";
                strcustomstring += "</div>";
                strcustomstring += "</div>";

                strcustomstring += "<div id=\"divmeadetomeasure\" style=\"display: none; margin-bottom: 5px;\" class=\"listedprice-pro\">";
                strcustomstring += "</div>";

                strcustomstring += "</div>";

                strcustomstring += "</div>";


                strcustomstring += " </div>";



                strcustomstring += "<div class=\"custom-cart-total\" style=\"display:none;\">";
                strcustomstring += "<div class=\"custom-cart-total-row\">";
                strcustomstring += "<div class=\"custom-cart-total-left\"> <span>TOTAL:</span></div>";
                strcustomstring += "<div class=\"custom-cart-total-right\"><div id=\"divtotalcustomprice\" class=\"total\">$0.00</div></div>";
                strcustomstring += "</div>";
                strcustomstring += "</div>";
                strcustomstring += "<div class=\"custom-detail-add-to-cart-button\">";
                strcustomstring += " <input type=\"image\" onclick=\"InsertProductCustom(" + strProductId + ",'ContentPlaceHolder1_btnAddTocartMade'); return false;\" alt=\"ADD TO CART\" src=\"/images/item-add-to-cart.jpg\" class=\"price-detail-right\" title=\"ADD TO CART\" id=\"ContentPlaceHolder1_btnAddTocartMade\" name=\"ctl00$ContentPlaceHolder1$btnAddTocartMade\">";
                strcustomstring += "</div>";


                strcustomstring += "</div>";
                strcustomstring += "</div>";




            }


            strcustomstring += "</div>";

            strcustomstring += "<div id=\"prepage\" style=\"position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px;  height: 100%; width: 100%; z-index: 1000; display: none;\"><table width=\"100%\" style=\"padding-top: 30px;\" align=\"center\"><tr><td align=\"center\" style=\"color: #fff;\" valign=\"middle\"><img alt=\"\" src=\"/images/option_loading.gif\" /><br /> <b>Loading ... ... Please wait!</b></td></tr></table></div>";

            strcustomstring += "</div>";
            strcustomstring += "</div>";
            strcustomstring += "<div style=\"display:none;\">~~h-" + strProductId.ToString() + "-h~~</div>";
            strcustomstring += "<div style=\"display:none;\">~~p-" + swatchid.ToString() + "-p~~</div>";
            strcustomstring = strcustomstring.Replace("###displaypre###", "  style='display:none;'");
            strcustomstring = strcustomstring.Replace("###displaynext###", " style='display:none;'");

            HttpContext.Current.Session["ptID"] = strProductId.ToString();
            return strcustomstring;

        }
        [System.Web.Services.WebMethod]
        public static string getCustompagecolorall(string strProductId, string strFabriccategoryId, string fabric, string pattern, string color, string nextId, string Previousid)
        {
            string strcustomstring = "";

            string swatchid = "0";

            DataSet dsswatch = new DataSet();
            Int32 num = 0;
            string strpricetag = "";

            string strwhereclause = "";

            if (!string.IsNullOrEmpty(fabric) && fabric.ToLower() != "fabric" && fabric.ToLower() != "other")
            {
                strwhereclause += " and Fabric='" + fabric + "'";
            }
            if (!string.IsNullOrEmpty(color) && color.ToLower() != "color" && color.ToLower() != "other")
            {
                strwhereclause += " and colors like '%" + color + "%'";
            }
            if (!string.IsNullOrEmpty(pattern) && pattern.ToLower() != "pattern" && pattern.ToLower() != "other")
            {
                strwhereclause += " and pattern ='" + pattern + "'";
            }
            //if (strFabriccategoryId.ToString().ToLower() == "all")
            //{
            //    dsswatch = CommonComponent.GetCommonDataSet("SELECT Top 20 SKu,ImageName,Name,A.ProductID,A.ProductSwatchid,SalePriceTag FROM tb_product INNER JOIN (SELECT  ProductSwatchid,ProductID  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'')<>'' " + strwhereclause + ") AS A on tb_product.Productid=A.ProductSwatchid WHERE StoreId=1 and isnull(active,0)=1 and isnull(deleted,0)=0");
            //}
            //else
            //{
            //    dsswatch = CommonComponent.GetCommonDataSet("SELECT Top 20 SKu,ImageName,Name,A.ProductID,A.ProductSwatchid,SalePriceTag FROM tb_product INNER JOIN (SELECT  ProductSwatchid,ProductID  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'')='" + strFabriccategoryId.Replace("'", "''") + "' " + strwhereclause + ") AS A on tb_product.Productid=A.ProductSwatchid WHERE StoreId=1 and isnull(active,0)=1 and isnull(deleted,0)=0");
            //}
            Int32 nextnewId = Convert.ToInt32(nextId);
            Int32 nextnewIdend = 0;
            Int32 PreviousId = Convert.ToInt32(Previousid);
            if (nextnewId == 0)
            {
                if (PreviousId > 20)
                {
                    nextnewIdend = PreviousId;

                    nextnewId = PreviousId - 20;
                }
                else
                {
                    nextnewIdend = 21;

                    nextnewId = 1;

                }


            }
            else if (nextnewId > 0)
            {
                nextnewIdend = nextnewId + 20;
                nextnewId = nextnewId;
            }

            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsswatch = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID,SKu,ImageName,tb_product.Name,A.name as pname,A.ProductID,A.ProductSwatchid,SalePriceTag,TotalRows=COUNT(*) OVER() FROM tb_product INNER JOIN (SELECT  ProductSwatchid,ProductID,Name  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'') in (SELECT FabricTypename FROM tb_ProductFabricType WHERE Isnull(Active,0)=1 and isnull(FabricTypename,'')<>'') and isnull(FabricType,'')<>'' " + strwhereclause + ") AS A on tb_product.Productid=A.ProductSwatchid WHERE StoreId=1 and isnull(active,0)=1 and isnull(deleted,0)=0) AS A WHERE A.RowID>=" + nextnewId + " and A.RowID <" + nextnewIdend + "");
            }
            else
            {
                dsswatch = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID, SKu,ImageName,tb_product.Name,A.name as pname,A.ProductID,A.ProductSwatchid,SalePriceTag,TotalRows=COUNT(*) OVER()  FROM tb_product INNER JOIN (SELECT  ProductSwatchid,ProductID,Name   FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'') in (SELECT FabricTypename FROM tb_ProductFabricType WHERE Isnull(Active,0)=1 and isnull(FabricTypename,'')<>'') and isnull(FabricType,'')='" + strFabriccategoryId.Replace("'", "''") + "' " + strwhereclause + ") AS A on tb_product.Productid=A.ProductSwatchid WHERE StoreId=1 and isnull(active,0)=1 and isnull(deleted,0)=0) AS A WHERE A.RowID>=" + nextnewId + " and A.RowID <" + nextnewIdend + " ");
            }

            Int32 RowId = 0;
            Int32 Totalrows = 0;

            if (dsswatch != null && dsswatch.Tables.Count > 0 && dsswatch.Tables[0].Rows.Count > 0)
            {
                //strcustomstring = "<ul id=\"ulfabricall" + nextnewId.ToString() + "\">";
                RowId = Convert.ToInt32(dsswatch.Tables[0].Rows[dsswatch.Tables[0].Rows.Count - 1]["RowID"].ToString());
                Totalrows = Convert.ToInt32(dsswatch.Tables[0].Rows[dsswatch.Tables[0].Rows.Count - 1]["TotalRows"].ToString());
                //RowId = nextnewIdend + 20;
                //Totalrows = Totalrows - RowId;
                Int32 dd = nextnewIdend;
                nextnewId = 0;
                nextnewIdend = 0;
                if (Totalrows > RowId)
                {
                    nextnewId = dd;
                }
                else if (dd > 0)
                {
                    nextnewIdend = dd - 20;
                }
                if (nextnewId > 0)
                {
                    nextnewIdend = nextnewId - 20;
                }


                //if (Totalrows > 0)
                //{
                //    nextnewIdend = nextnewIdend - Convert.ToInt32(nextId);
                //    nextnewId = RowId;

                //}
                //else if (Totalrows < 0 && Totalrows > -21)
                //{
                //    RowId = RowId - Convert.ToInt32(dsswatch.Tables[0].Rows[dsswatch.Tables[0].Rows.Count - 1]["TotalRows"].ToString());
                //    if (RowId > 0 && Convert.ToInt32(nextId) > 0)
                //    {
                //        nextnewId = Convert.ToInt32(nextId) + 20;
                //        nextnewIdend = 0;
                //    }
                //    else if (RowId > 0 && Convert.ToInt32(PreviousId) > 0)
                //    {
                //        nextnewId = Convert.ToInt32(PreviousId) + 20;
                //        nextnewIdend = 0;
                //    }
                //    else
                //    {
                //        nextnewId = 0;
                //        nextnewIdend = 0;
                //    }
                //}
                //else
                //{

                //    if (Convert.ToInt32(nextnewIdend) > 21)
                //    {
                //        nextnewIdend = nextnewId;
                //    }
                //    nextnewId = 0;

                //}

                //if (Convert.ToInt32(PreviousId) > 0)
                //{
                //    nextnewId = Convert.ToInt32(PreviousId);
                //}

                //if (Convert.ToInt32(nextnewId) > 21)
                //{
                //    nextnewIdend = nextnewId - 20;
                //}



                //if (Totalrows > Convert.ToInt32(nextId) && Convert.ToInt32(nextId) != 0)
                //{
                //    nextnewId = Convert.ToInt32(nextId) + 20;
                //    nextnewIdend = nextnewId - 20;
                //    if (nextnewId <= 21)
                //    {
                //        nextnewIdend = 0;
                //    }

                //}
                //else if (Totalrows > Convert.ToInt32(nextId) && Convert.ToInt32(nextId) == 0)
                //{
                //    nextnewId = Convert.ToInt32(PreviousId) + 1;
                //    nextnewIdend = nextnewId - 20;
                //    if (nextnewId <= 21)
                //    {
                //        nextnewIdend = 0;
                //    }
                //}
                //else
                //{
                //    nextnewIdend = nextnewId - 20;
                //    if (PreviousId <= 21 && PreviousId > 0)
                //    {
                //        nextnewId = 21;
                //    }

                //}
                //if (nextnewId <= 21)
                //{
                //    nextnewIdend = 0;
                //}
                //if (nextnewId - 20 == nextnewIdend)
                //{
                //    nextnewId = 0;
                //}

                Int32 ic = 0;
                for (int i = 0; i < dsswatch.Tables[0].Rows.Count; i++)
                {
                    if (!String.IsNullOrEmpty(dsswatch.Tables[0].Rows[i]["Imagename"].ToString()))
                    {
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString())))
                        {
                            //if (ic == 0 && strProductId == "0")
                            //{
                            //    strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            //    //strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            //}
                            //else
                            {
                                if (strProductId == dsswatch.Tables[0].Rows[i]["ProductID"].ToString())
                                {
                                    strcustomstring += "<li class=\"active\" id=\"liswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                    swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                    strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                                }
                                else
                                {
                                    strcustomstring += "<li id=\"liswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                }

                                //strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            ic++;

                        }
                        else
                        {
                            //if (ic == 0 && strProductId == "0")
                            //{


                            //    strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            //    //strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            //}
                            //else
                            {
                                if (strProductId == dsswatch.Tables[0].Rows[i]["ProductID"].ToString())
                                {
                                    strcustomstring += "<li class=\"active\" id=\"liswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                    swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                    strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                                }
                                else
                                {
                                    strcustomstring += "<li id=\"liswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "','" + HttpContext.Current.Server.HtmlEncode(dsswatch.Tables[0].Rows[i]["Pname"].ToString()) + "');\" id=\"aswatch-" + dsswatch.Tables[0].Rows[i]["RowID"].ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "fabricimage/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Pname"].ToString() + "</span></a></li>";
                                }

                                //strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            ic++;

                        }
                    }

                }

                //strcustomstring += "</ul>";
            }
            strcustomstring += "<div style=\"display:none;\">~~n-" + nextnewId.ToString() + "-n~~</div>";
            //if (PreviousId <= 21 && Convert.ToInt32(nextId) == 0)
            //{
            //    strcustomstring += "<div style=\"display:none;\">~~pr-0-pr~~</div>";
            //}
            //else
            //{
            strcustomstring += "<div style=\"display:none;\">~~pr-" + nextnewIdend.ToString() + "-pr~~</div>";
            //}

            return strcustomstring;

        }
        [System.Web.Services.WebMethod]
        public static string getCustompageswatch(string strProductId, string strFabriccategoryId, string fabric, string pattern, string color, string nextId, string Previousid)
        {

            Int32 nextnewId = Convert.ToInt32(nextId);
            Int32 nextnewIdend = 0;
            Int32 PreviousId = Convert.ToInt32(Previousid);
            //if (nextnewId == 0)
            //{
            //    if (PreviousId > 20)
            //    {
            //        nextnewIdend = PreviousId;

            //        nextnewId = PreviousId - 20;
            //    }
            //    else
            //    {
            //        nextnewIdend = 21;

            //        nextnewId = 1;

            //    }
            //    //nextnewIdend = PreviousId;

            //    //nextnewId = PreviousId - 20;

            //}
            //else if (nextnewId > 0)
            //{
            //    nextnewIdend = nextnewId + 20;
            //    nextnewId = nextnewId;
            //}
            if (nextnewId > 0)
            {
                nextnewIdend = nextnewId;
                nextnewId = nextnewId - 20;
            }

            string strcustomstring = "<div id=\"divVariant\">";
            strcustomstring += "<div class=\"readymade-detail\">";
            string swatchid = "0";

            // Start


            strcustomstring += "<div class=\"readymade-detail-pt1\">";

            strcustomstring += "<div onclick=\"varianttabhideshowcustom(0);\" id=\"divcolspancustom-0\" class=\"readymade-detail-pt1-pro\">";
            strcustomstring += "<span id=\"spancolspancustom-0\">1</span>Select a Fabric:<strong style=\"font-weight:normal;color:#B92127; margin-left:5px;\">###dftfabric####</strong>";
            strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
            strcustomstring += "</div>";

            strcustomstring += "<div style=\"display: none;\" id=\"divcolspancustomvalue-0\" class=\"custom-detail-row\">";
            strcustomstring += "<div class=\"custom-detail-row-1\">";

            strcustomstring += "<div class=\"custom-detail-row-1-detail\"> <span>Categories</span>";
            strcustomstring += "<div class=\"selector fixedWidth\"> <span id=\"ddlcustomcat\">" + strFabriccategoryId + "</span>";
            strcustomstring += "<select id=\"ContentPlaceHolder1_ddlfabriccategory\" class=\"option-2\" name=\"\" onchange=\"fabriccategorycomb();\">";
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                strcustomstring += "<option value=\"All\" selected>All</option>";
            }
            else
            {
                strcustomstring += "<option>All</option>";
            }


            DataSet dsgetall = new DataSet();
            dsgetall = CommonComponent.GetCommonDataSet("SELECT DISTINCT FabricType  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'')<>'' Order BY FabricType");


            if (dsgetall != null && dsgetall.Tables.Count > 0 && dsgetall.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsgetall.Tables[0].Rows.Count; i++)
                {

                    if (string.IsNullOrEmpty(strFabriccategoryId))
                    {
                        strFabriccategoryId = dsgetall.Tables[0].Rows[i]["FabricType"].ToString();
                    }
                    strcustomstring = strcustomstring.Replace("###dftfabric####", strFabriccategoryId);
                    if (strFabriccategoryId.ToString().ToLower().Trim() == dsgetall.Tables[0].Rows[i]["FabricType"].ToString().ToLower().Trim())
                    {
                        strcustomstring += "<option value=\"" + dsgetall.Tables[0].Rows[i]["FabricType"].ToString() + "\" selected>" + dsgetall.Tables[0].Rows[i]["FabricType"].ToString() + "</option>";
                    }
                    else
                    {
                        strcustomstring += "<option value=\"" + dsgetall.Tables[0].Rows[i]["FabricType"].ToString() + "\">" + dsgetall.Tables[0].Rows[i]["FabricType"].ToString() + "</option>";
                    }

                }

            }

            strcustomstring += "</select>";
            strcustomstring += "</div>";


            DataSet dsgetall1 = new DataSet();
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Fabric  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Fabric,'')<>'' UNION  SELECT 'Other' as Fabric  UNION  SELECT 'Fabric' as Fabric) as A Order By case when  Fabric='Other' then 2 when  Fabric='Fabric' then 0 else 1 end");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Fabric  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Fabric,'')<>'' UNION  SELECT 'Other' as Fabric  UNION  SELECT 'Fabric' as Fabric) as A Order By case when  Fabric='Other' then 2 when  Fabric='Fabric' then 0 else 1 end");
                //dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Fabric  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and FabricType='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Fabric,'')<>'' UNION  SELECT 'Other' as Fabric  UNION  SELECT 'Fabric' as Fabric) as A Order By case when  Fabric='Other' then 2 when  Fabric='Fabric' then 0 else 1 end");
            }




            strcustomstring += "<div class=\"selector fixedWidth\"> <span id=\"ddlcustomfabri\">###dftfabricnew####</span>";
            strcustomstring += "<select id=\"ContentPlaceHolder1_ddlfabric\" class=\"option-2\" name=\"\" onchange=\"fabriccategory();\">";
            //strcustomstring += "<option>all</option>";



            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsgetall1.Tables[0].Rows.Count; i++)
                {

                    if (string.IsNullOrEmpty(fabric))
                    {
                        fabric = dsgetall1.Tables[0].Rows[i]["Fabric"].ToString();
                    }
                    strcustomstring = strcustomstring.Replace("###dftfabricnew####", fabric);
                    if (fabric.ToString().ToLower().Trim() == dsgetall1.Tables[0].Rows[i]["Fabric"].ToString().ToLower().Trim())
                    {
                        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + "\" selected>" + dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + "</option>";
                    }
                    else
                    {
                        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + "\">" + dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + "</option>";
                    }

                }

            }

            strcustomstring += "</select>";
            strcustomstring += "</div>";
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Pattern  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Pattern,'')<>'' UNION  SELECT 'Other' as Pattern  UNION  SELECT 'Pattern' as Pattern) as A Order By case when  Pattern='Other' then 2 when  Pattern='Pattern' then 0 else 1 end");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Pattern  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Pattern,'')<>'' UNION  SELECT 'Other' as Pattern  UNION  SELECT 'Pattern' as Pattern) as A Order By case when  Pattern='Other' then 2 when  Pattern='Pattern' then 0 else 1 end");
                //dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Pattern  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Pattern,'')<>'' UNION  SELECT 'Other' as Pattern  UNION  SELECT 'Pattern' as Pattern) as A Order By case when  Pattern='Other' then 2 when  Pattern='Pattern' then 0 else 1 end");
            }




            strcustomstring += "<div class=\"selector fixedWidth\"> <span id=\"ddlcustompattern\">###ddlcustompattern####</span>";
            strcustomstring += "<select id=\"ContentPlaceHolder1_ddlcustompattern\" class=\"option-2\" name=\"\" onchange=\"fabriccategory();\">";
            //strcustomstring += "<option>all</option>";



            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsgetall1.Tables[0].Rows.Count; i++)
                {

                    if (string.IsNullOrEmpty(pattern))
                    {
                        pattern = dsgetall1.Tables[0].Rows[i]["Pattern"].ToString();
                    }
                    strcustomstring = strcustomstring.Replace("###ddlcustompattern####", pattern);
                    if (pattern.ToString().ToLower().Trim() == dsgetall1.Tables[0].Rows[i]["Pattern"].ToString().ToLower().Trim())
                    {
                        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + "\" selected>" + dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + "</option>";
                    }
                    else
                    {
                        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + "\">" + dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + "</option>";
                    }

                }

            }

            strcustomstring += "</select>";
            strcustomstring += "</div>";

            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT Colors+',' as SearchValue  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Colors,'')<>''");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT Colors+',' as SearchValue  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Colors,'')<>''");
                //dsgetall1 = CommonComponent.GetCommonDataSet("SELECT Colors+',' as SearchValue  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Colors,'')<>''");
            }




            strcustomstring += "<div class=\"selector fixedWidth\"> <span id=\"ddlcustomcolor\">###ddlcustomcolor####</span>";
            strcustomstring += "<select id=\"ContentPlaceHolder1_ddlcustomcolor\" class=\"option-2\" name=\"\" onchange=\"fabriccategory();\">";
            //strcustomstring += "<option>all</option>";
            if (string.IsNullOrEmpty(color))
            {
                strcustomstring += "<option value=\"Color\" selected>Color</option>";
            }
            else
            {
                strcustomstring += "<option value=\"Color\">Color</option>";
            }

            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {

                string[] strcolor = dsgetall1.Tables[0].Rows[0]["SearchValue"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (strcolor.Length > 0)
                {
                    string strtt = ",";
                    bool flg = false;
                    foreach (string stt in strcolor)
                    {
                        if (!string.IsNullOrEmpty(stt))
                        {


                            if (strtt.ToString().ToLower().IndexOf("," + stt.ToLower() + ",") <= -1)
                            {
                                if (string.IsNullOrEmpty(color))
                                {
                                    color = "Color";
                                    flg = true;
                                }
                                strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);
                                if (color.ToString().ToLower().Trim() == stt.ToString().ToLower().Trim() && flg == false)
                                {
                                    strcustomstring += "<option value=\"" + stt.ToString() + "\" selected>" + stt.ToString() + "</option>";
                                }
                                else
                                {
                                    strcustomstring += "<option value=\"" + stt.ToString() + "\">" + stt.ToString() + "</option>";
                                }
                                strtt += stt + ",";
                            }
                        }
                        //if (string.IsNullOrEmpty(color))
                        //{
                        //    color = dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString();
                        //}
                        //strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);
                        //if (color.ToString().ToLower().Trim() == dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim())
                        //{
                        //    strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "\" selected>" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                        //}
                        //else
                        //{
                        //    strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "\">" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                        //}
                    }
                }
                else
                {
                    color = "Color";
                    strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);
                }
                //for (int i = 0; i < dsgetall1.Tables[0].Rows.Count; i++)
                //{

                //    if (string.IsNullOrEmpty(color))
                //    {
                //        color = dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString();
                //    }
                //    strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);
                //    if (color.ToString().ToLower().Trim() == dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim())
                //    {
                //        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "\" selected>" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                //    }
                //    else
                //    {
                //        strcustomstring += "<option value=\"" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "\">" + dsgetall1.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                //    }

                //}

            }
            else
            {


                color = "Color";
                strcustomstring = strcustomstring.Replace("###ddlcustomcolor####", color);

            }
            strcustomstring += "<option value=\"Other\">Other</option>";
            strcustomstring += "</select>";
            strcustomstring += "</div>";



            strcustomstring += "</div>";
            strcustomstring += "</div>";



            strcustomstring += "<div class=\"custom-detail-row-2\">";


            strcustomstring += "<div class=\"select-febric-bg\"><div style=\"float:left\"><a href=\"javascript:void(0);\" title=\"Previous\" class=\"nextfabric\"  ###displaypre### id=\"apreviousfabric\"><img title=\"Previous\" alt=\"Previous\" src=\"/images/arrow-1-20.png\"  style=\"width:20px;height:20px\" border=\"0\" /></a></div><div style=\"float:right\"><a href=\"javascript:void(0);\" title=\"Next\" class=\"previousfabric\" id=\"anextfabric\" ###displaynext###><img title=\"Next\" style=\"width:20px;height:20px\" alt=\"Next\" src=\"/images/arrow-2-20.png\" border=\"0\" /></a></div></div>";


            strcustomstring += "<div class=\"select-febric-bg\">";
            strcustomstring += "<ul id=\"ulfabricall\">";

            DataSet dsswatch = new DataSet();
            Int32 num = 0;
            string strpricetag = "";

            string strwhereclause = "";

            if (!string.IsNullOrEmpty(fabric) && fabric.ToLower() != "fabric" && fabric.ToLower() != "other")
            {
                strwhereclause += " and Fabric='" + fabric + "'";
            }
            if (!string.IsNullOrEmpty(color) && color.ToLower() != "color" && color.ToLower() != "other")
            {
                strwhereclause += " and colors like '%" + color + "%'";
            }
            if (!string.IsNullOrEmpty(pattern) && pattern.ToLower() != "pattern" && pattern.ToLower() != "other")
            {
                strwhereclause += " and pattern ='" + pattern + "'";
            }
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsswatch = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID,SKu,ImageName,Name,A.ProductID,A.ProductSwatchid,SalePriceTag,TotalRows=COUNT(*) OVER() FROM tb_product INNER JOIN (SELECT  ProductSwatchid,ProductID  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'')<>'' " + strwhereclause + ") AS A on tb_product.Productid=A.ProductSwatchid WHERE StoreId=1 and isnull(active,0)=1 and isnull(deleted,0)=0) AS A WHERE A.RowID >=" + nextnewId + " and A.RowID < " + nextnewIdend + "");
            }
            else
            {
                dsswatch = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID, SKu,ImageName,Name,A.ProductID,A.ProductSwatchid,SalePriceTag,TotalRows=COUNT(*) OVER()  FROM tb_product INNER JOIN (SELECT  ProductSwatchid,ProductID  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and StoreId=1 and isnull(FabricType,'')='" + strFabriccategoryId.Replace("'", "''") + "' " + strwhereclause + ") AS A on tb_product.Productid=A.ProductSwatchid WHERE StoreId=1 and isnull(active,0)=1 and isnull(deleted,0)=0) AS A WHERE A.RowID >=" + nextnewId + " and A.RowID < " + nextnewIdend + " ");
            }

            Int32 RowId = 0;
            Int32 Totalrows = 0;




            if (dsswatch != null && dsswatch.Tables.Count > 0 && dsswatch.Tables[0].Rows.Count > 0)
            {


                Totalrows = Convert.ToInt32(dsswatch.Tables[0].Rows[dsswatch.Tables[0].Rows.Count - 1]["TotalRows"].ToString());
                RowId = Convert.ToInt32(dsswatch.Tables[0].Rows[dsswatch.Tables[0].Rows.Count - 1]["RowID"].ToString());
                Int32 dd = nextnewIdend;
                nextnewId = 0;
                nextnewIdend = 0;
                if (Totalrows > RowId)
                {
                    nextnewId = dd;
                }
                else if (dd > 0)
                {
                    nextnewIdend = dd - 20;
                }
                if (nextnewId > 0)
                {
                    nextnewIdend = nextnewId - 20;
                }
                //Totalrows = Totalrows - RowId;
                //if (Totalrows > 0)
                //{
                //    nextnewIdend = nextnewIdend - Convert.ToInt32(nextId);
                //    nextnewId = RowId;

                //}
                //else if (Totalrows < 0 && Totalrows > -21)
                //{
                //    RowId = RowId - Convert.ToInt32(dsswatch.Tables[0].Rows[dsswatch.Tables[0].Rows.Count - 1]["TotalRows"].ToString());
                //    if (RowId > 0 && Convert.ToInt32(nextId) > 0)
                //    {
                //        nextnewId = Convert.ToInt32(nextId) + 20;
                //        nextnewIdend = 0;
                //    }
                //    else if (RowId > 0 && Convert.ToInt32(PreviousId) > 0)
                //    {
                //        nextnewId = Convert.ToInt32(PreviousId) + 20;
                //        nextnewIdend = 0;
                //    }
                //    else
                //    {
                //        nextnewId = 0;
                //        nextnewIdend = 0;
                //    }
                //}
                //else
                //{

                //    if (Convert.ToInt32(nextnewIdend) > 21)
                //    {
                //        nextnewIdend = nextnewId;
                //    }
                //    nextnewId = 0;

                //}

                //if (Convert.ToInt32(PreviousId) > 0)
                //{
                //    nextnewId = Convert.ToInt32(PreviousId);
                //}

                //if (Convert.ToInt32(nextnewId) > 21)
                //{
                //    nextnewIdend = nextnewId - 20;
                //}
                Int32 ic = 0;
                for (int i = 0; i < dsswatch.Tables[0].Rows.Count; i++)
                {
                    if (!String.IsNullOrEmpty(dsswatch.Tables[0].Rows[i]["Imagename"].ToString()))
                    {
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString())))
                        {
                            if (ic == 0 && strProductId == "0")
                            {


                                strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                                //strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            else
                            {
                                if (strProductId == dsswatch.Tables[0].Rows[i]["ProductID"].ToString())
                                {
                                    strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                                    swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                    strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                                }
                                else
                                {
                                    strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsswatch.Tables[0].Rows[i]["Imagename"].ToString() + "\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                                }

                                //strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            ic++;
                            if (strProductId == "0")
                            {
                                strProductId = dsswatch.Tables[0].Rows[i]["ProductID"].ToString();
                                swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                            }
                        }
                        else
                        {
                            if (ic == 0 && strProductId == "0")
                            {


                                strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                                //strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            else
                            {
                                if (strProductId == dsswatch.Tables[0].Rows[i]["ProductID"].ToString())
                                {
                                    strcustomstring += "<li class=\"active\" id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                                    swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                    strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                                }
                                else
                                {
                                    strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                                }

                                //strcustomstring += "<li id=\"liswatch-" + i.ToString() + "\"><a onclick=\"javascript:selectswatchimage(" + dsswatch.Tables[0].Rows[i]["ProductID"].ToString() + ",'aswatch-" + i.ToString() + "');\" id=\"aswatch-" + i.ToString() + "\" href=\"javascript:void(0);\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "micro/test-sku-123_15866_2.jpg\" alt=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\" title=\"" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "\"><span>" + dsswatch.Tables[0].Rows[i]["Name"].ToString() + "</span></a></li>";
                            }
                            ic++;
                            if (strProductId == "0")
                            {
                                strProductId = dsswatch.Tables[0].Rows[i]["ProductID"].ToString();
                                swatchid = dsswatch.Tables[0].Rows[i]["ProductSwatchid"].ToString();
                                strpricetag = dsswatch.Tables[0].Rows[i]["SalePriceTag"].ToString();
                            }
                        }
                    }
                }


            }
            if (string.IsNullOrEmpty(strpricetag))
            {
                strpricetag = "(Per Panel)";
            }




            strcustomstring += "</ul>";

            if (!string.IsNullOrEmpty(swatchid) && swatchid.ToString() != "0")
            {
                strcustomstring += "<div class=\"next-step\"><img border=\"0\" onclick=\"varianttabhideshowcustom(1);\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" src=\"/images/next-option.png\"></div>";
            }
            strcustomstring += "</div>";
            strcustomstring += "</div>";

            strcustomstring += "</div>";




            strcustomstring += "</div>";










            ///END


            // Start

            strcustomstring = "";
            /// if (!string.IsNullOrEmpty(swatchid) && swatchid.ToString() != "0")
            {


                strcustomstring += "<div class=\"readymade-detail-pt1\">";
                strcustomstring += "<div onclick=\"varianttabhideshowcustom(1);\" id=\"divcolspancustom-1\" class=\"readymade-detail-pt1-pro\">";
                strcustomstring += "<span id=\"spancolspancustom-1\">2</span>Select Header";
                strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" onclick=\"ShowModelForPleatGuide();\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
                strcustomstring += "</div>";


                strcustomstring += "<div style=\"display: none;padding-left:4% !important;padding-top:10px;width:95% !important\" id=\"divcolspancustomvalue-1\" class=\"readymade-detail-right-pro custom-detail-row\">";
                strcustomstring += "<select style=\"width: 200px !important; display: none;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomstyle\" name=\"ctl00$ContentPlaceHolder1$ddlcustomstyle\">";
                strcustomstring += "<option value=\"0\">Header</option>";


                DataSet dsstyle = new DataSet();
                dsstyle = CommonComponent.GetCommonDataSet("SELECT Style as SearchValue,tb_ProductSearchType.displayorder from tb_ProductStylePrice INNER JOIN tb_ProductSearchType ON tb_ProductSearchType.SearchValue=tb_ProductStylePrice.Style  where tb_ProductStylePrice.productId=" + strProductId.ToString() + " and isnull(tb_ProductStylePrice.Active,0)=1 AND isnull(tb_ProductSearchType.Deleted,0)=0 AND tb_ProductSearchType.SearchType=6 Order By tb_ProductSearchType.displayorder");


                strcustomstring += "###option###";

                //strcustomstring += "<option value=\"Pole Pocket\">Pole Pocket</option>";
                //strcustomstring += "<option value=\"French Pleat\">French Pleat</option>";
                //strcustomstring += "<option value=\"Inverted Pleat\">Inverted Pleat</option>";
                //strcustomstring += "<option value=\"Parisian Pleat\">Parisian Pleat</option>";
                //strcustomstring += "<option value=\"Goblet Pleat\">Goblet Pleat</option>";

                strcustomstring += "</select>";
                // strcustomstring += "<div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Pole Pocket','divcustomflat-radio-001-1','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-1\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Pole Pocket','divcustomflat-radio-001-1','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Pole Pocket</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','French Pleat','divcustomflat-radio-001-2','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-2\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','French Pleat','divcustomflat-radio-001-2','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;French Pleat</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Inverted Pleat','divcustomflat-radio-001-3','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-3\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Inverted Pleat','divcustomflat-radio-001-3','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Inverted Pleat</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Parisian Pleat','divcustomflat-radio-001-4','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-4\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Parisian Pleat','divcustomflat-radio-001-4','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Parisian Pleat</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Goblet Pleat','divcustomflat-radio-001-5','001');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-001-5\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','Goblet Pleat','divcustomflat-radio-001-5','001');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Goblet Pleat</div></div>";

                string strnewoption = "";
                if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
                {





                    for (int i = 0; i < dsstyle.Tables[0].Rows.Count; i++)
                    {

                        strcustomstring += "<div style=\"float: left; width: 30%; margin-bottom: 5px;margin-left:5px;\">";
                        strnewoption += "<option value=\"" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "\">" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "</option>";
                        strcustomstring += "<div style=\"float:left;width:100%;\">";

                        //strcustomstring += "<img src=\"/images/roman_inside_mount.png\">";
                        if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "pole pocket")
                        {
                            strcustomstring += "<img src=\"/images/pole_pocket_header.jpg\" title=\"Pole Pocket\" alt=\"Pole Pocket\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "french pleat")
                        {
                            strcustomstring += "<img src=\"/images/French_Pleat.jpg\" title=\"French Pleat\" alt=\"French Pleat\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "inverted pleat")
                        {
                            strcustomstring += "<img src=\"/images/Inverted_Pleat.jpg\" title=\"Inverted Pleat\" alt=\"Inverted Pleat\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "parisian pleat")
                        {
                            strcustomstring += "<img src=\"/images/Parisian_Pleat.jpg\" title=\"Parisian Pleat\" alt=\"Parisian Pleat\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "goblet pleat")
                        {
                            strcustomstring += "<img src=\"/images/Goblet_Pleat.jpg\" title=\"Goblet Pleat\" alt=\"Goblet Pleat\" style=\"width:163px;\">";
                        }
                        else if (dsstyle.Tables[0].Rows[i]["SearchValue"].ToString().ToLower().Trim() == "grommet pleat")
                        {
                            strcustomstring += "<img src=\"/images/Grommet_Pleat.jpg\" title=\"Goblet Pleat\" alt=\"Goblet Pleat\" style=\"width:163px;\">";
                        }
                        else
                        {
                            strcustomstring += "<img src=\"/images/pole_pocket_header.jpg\" title=\"Pole Pocket\" alt=\"Pole Pocket\" style=\"width:163px;\">";
                        }

                        strcustomstring += "</div>";
                        strcustomstring += "<div style=\"margin-left:22%;\"><div style=\"float:left;width:100%;margin-bottom:5px;\" class='item-radio-display' onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "','divcustomflat-radio-001-" + i.ToString() + "','001');ChangeCustomprice();\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomstyle','" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "','divcustomflat-radio-001-" + i.ToString() + "','001');ChangeCustomprice();\" id=\"divcustomflat-radio-001-" + i.ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsstyle.Tables[0].Rows[i]["SearchValue"].ToString() + "</div></div></div>";
                        strcustomstring += "</div>";
                    }


                }
                strcustomstring = strcustomstring.Replace("###option###", strnewoption);

                //strcustomstring += "<div style=\"float: right; width: 100%; margin-bottom: 5px; text-align: right;\">";
                //strcustomstring += "<img border=\"0\" onclick=\"if(document.getElementById('ContentPlaceHolder1_ddlcustomstyle') != null &amp;&amp; document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex == 0){jAlert('Please Select Header.','Message');}else{varianttabhideshowcustom(2);}\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" src=\"/images/next-option.png\">";
                //strcustomstring += "</div>";

                strcustomstring += "<div style=\"float: right; width: 95%;text-align: right;padding:5px 6px 5px 0;\"><img border=\"0\" onclick=\"if(document.getElementById('ContentPlaceHolder1_ddlcustomstyle') != null && document.getElementById('ContentPlaceHolder1_ddlcustomstyle').selectedIndex == 0){jAlert('Please Select Header.','Message');}else{varianttabhideshowcustom(2);}\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" src=\"/images/next-option.png\"></div>";
                strcustomstring += "</div>";
                strcustomstring += "<div id=\"spancolspancustomvalue-1\" style=\"margin-top: 10px; display: none;\">";
                strcustomstring += "</div>";
                strcustomstring += "</div>";






                ///END







                strcustomstring += "<div class=\"readymade-detail-pt1\">";
                strcustomstring += "<div onclick=\"varianttabhideshowcustom(2);\" id=\"divcolspancustom-2\" class=\"readymade-detail-pt1-pro\">";
                strcustomstring += "<span id=\"spancolspancustom-2\">3</span>Select size";
                strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" onclick=\"variantDetail('divMakeOrderWidth');\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
                strcustomstring += "</div>";

                strcustomstring += "<div style=\"display: none;padding-left:4% !important;width:95% !important;padding-top:10px !important\" id=\"divcolspancustomvalue-2\" class=\"readymade-detail-right-pro custom-detail-row\">";
                strcustomstring += "<div style=\"float: left; width: 30%; margin-bottom: 5px;\">";
                strcustomstring += "<div style=\"float:left;width:100%;\"><a onclick=\"variantDetail('divMakeOrderWidth');\" href=\"javascript:void(0);\" title=\"Size\"><img src=\"/images/curtain-size.png\" alt=\"Size\" title=\"Size\"></a></div>";
                strcustomstring += "</div>";

                strcustomstring += "<div class=\"selector fixedWidth\">";
                strcustomstring += "<span id=\"ddlcustomwidth-ddl\" style=\"-moz-user-select: none;\">Width</span>";
                strcustomstring += "<select style=\"width: 200px !important;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomwidth\" name=\"ctl00$ContentPlaceHolder1$ddlcustomwidth\">";
                strcustomstring += "<option value=\"Width\" selected=\"selected\">Width</option>";
                for (int i = 25; i < 201; i++)
                {
                    strcustomstring += "<option value=\"" + i.ToString() + "\">" + i.ToString() + "" + "\"" + "</option>\"";
                }

                strcustomstring += "</select>";
                strcustomstring += "</div>";

                //strcustomstring += "<div class=\"selector fixedWidth\"><span id=\"spanvariant-exact-0\" style=\"-moz-user-select: none;\">00</span><select id=\"Selectvariant-exact-0\" name=\"Selectvariant-exact-0\" onchange=\"PriceChangeondropdown();\" class=\"option1\"><option style=\"display: none;\" value=\"0\">Extra width</option><option selected=\"true\" value=\"00\">00</option><option value=\"1/8\">1/8</option><option value=\"1/4\">1/4</option><option value=\"3/8\">3/8</option><option value=\"1/2\">1/2</option><option value=\"5/8\">5/8</option><option value=\"3/4\">3/4</option><option value=\"7/8\">7/8</option></select></div>";

                strcustomstring += "<div class=\"selector fixedWidth\">";
                strcustomstring += "<span id=\"ddlcustomlength-ddl\" style=\"-moz-user-select: none;\">Length</span>";
                strcustomstring += "<select style=\"width: 200px !important;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomlength\" name=\"ctl00$ContentPlaceHolder1$ddlcustomlength\">";
                strcustomstring += "<option value=\"\" selected=\"selected\">Length</option>";
                for (int i = 45; i < 276; i++)
                {
                    strcustomstring += "<option value=\"" + i.ToString() + "\">" + i.ToString() + "" + "\"" + "</option>\"";
                }

                strcustomstring += "</select>";
                strcustomstring += "</div>";
                //strcustomstring += "<div class=\"selector fixedWidth\"><span id=\"spanvariant-exact-99999999\" style=\"-moz-user-select: none;\">00</span><select id=\"Selectvariant-exact-0\" name=\"Selectvariant-exact-0\"  class=\"option1\"><option style=\"display: none;\" value=\"0\">Extra width</option><option selected=\"true\" value=\"00\">00</option><option value=\"1/8\">1/8</option><option value=\"1/4\">1/4</option><option value=\"3/8\">3/8</option><option value=\"1/2\">1/2</option><option value=\"5/8\">5/8</option><option value=\"3/4\">3/4</option><option value=\"7/8\">7/8</option></select></div>";
                strcustomstring += "<div style=\"float: right; width: 100%; margin-bottom: 5px; text-align: right;\">";
                strcustomstring += "<img border=\"0\" onclick=\"if(document.getElementById('ContentPlaceHolder1_ddlcustomwidth') != null &amp;&amp; document.getElementById('ContentPlaceHolder1_ddlcustomwidth').selectedIndex == 0){jAlert('Please Select Width.','Message');}else if(document.getElementById('ContentPlaceHolder1_ddlcustomlength') != null &amp;&amp; document.getElementById('ContentPlaceHolder1_ddlcustomlength').selectedIndex == 0){jAlert('Please Select Length.','Message');}else{varianttabhideshowcustom(3);}\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" id=\"ContentPlaceHolder1_imgnextoption\" src=\"/images/next-option.png\">";
                strcustomstring += "</div>";
                strcustomstring += "</div>";


                strcustomstring += "<div id=\"spancolspancustomvalue-222\" style=\"margin-top: 10px; display: none;\">";
                strcustomstring += "</div>";
                strcustomstring += "</div>";

                strcustomstring += "</div>";
                strcustomstring += "<div style=\"display: none;\" class=\"readymade-detail-pt1\">";

                strcustomstring += "<div style=\"display: none;\" onclick=\"varianttabhideshowcustom(3);\" id=\"divcolspancustom-333\" class=\"readymade-detail-pt1-pro\">";
                strcustomstring += "<span id=\"spancolspancustom-333\">4</span>Select Length";
                strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" onclick=\"variantDetail('divMakeOrderLength');\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
                strcustomstring += "</div>";

                strcustomstring += "<div style=\"display: none;\" id=\"divcolspancustomvalue-333\" class=\"readymade-detail-right-pro \">";
                strcustomstring += "</div>";
                strcustomstring += "<div id=\"spancolspancustomvalue-333\" style=\"margin-top: 10px; display: none;\">";
                strcustomstring += "</div>";

                strcustomstring += "</div>";


                dsstyle = new DataSet();
                dsstyle = CommonComponent.GetCommonDataSet(@"SELECT case when isnull(tb_ProductOptionsPrice.AdditionalPrice,0)=0 then tb_ProductOptionsPrice.Options else tb_ProductOptionsPrice.Options+'($'+cast(Round(tb_ProductOptionsPrice.AdditionalPrice,2) as varchar(10))+')'  end as name, tb_ProductOptionsPrice.ProductId, tb_ProductSearchType.DisplayOrder
FROM dbo.tb_ProductOptionsPrice INNER JOIN dbo.tb_ProductSearchType ON dbo.tb_ProductOptionsPrice.Options = dbo.tb_ProductSearchType.SearchValue WHERE isnull(tb_ProductOptionsPrice.Active,0)=1 AND tb_ProductOptionsPrice.ProductId=" + strProductId.ToString() + @" Order By tb_ProductSearchType.DisplayOrder");
                strnewoption = "";
                Int32 inum = 4;
                if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
                {


                    strcustomstring += "<div class=\"readymade-detail-pt1\">";
                    strcustomstring += "<div onclick=\"varianttabhideshowcustom(3);\" id=\"divcolspancustom-3\" class=\"readymade-detail-pt1-pro custom-detail-row\">";
                    strcustomstring += "<span id=\"spancolspancustom-3\">4</span>Select Options";
                    strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"><a style=\"color: #B92127\" onclick=\"variantDetail('divMakeOrderOptions');\" href=\"javascript:void(0);\" title=\"Learn More\">LEARN MORE</a></div>";
                    strcustomstring += "</div>";

                    strcustomstring += "<div id=\"divcolspancustomvalue-3\" class=\"readymade-detail-right-pro custom-detail-row\" style=\"padding-left:4% !important;width:95% !important;display:none;padding-top:10px !important\">";

                    strcustomstring += "<select style=\"width: 200px !important; display: none;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomoptin\" name=\"ctl00$ContentPlaceHolder1$ddlcustomoptin\">";
                    strcustomstring += "<option value=\"0\">Options</option>";
                    //strcustomstring += "<option value=\"Lined\">Lined</option>";
                    //strcustomstring += "<option value=\"Lined &amp; Interlined\">Lined &amp; Interlined</option>";
                    //strcustomstring += "<option value=\"Blackout Lining\">Blackout Lining</option>";
                    strcustomstring += "###option1###";
                    strcustomstring += "</select>";


                    for (int i = 0; i < dsstyle.Tables[0].Rows.Count; i++)
                    {
                        strnewoption += "<option value=\"" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "\">" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "</option>";
                        if (i == 0)
                        {
                            strcustomstring += "<div style=\"float:left;margin-bottom:5px;margin-left:5px;\" class='item-radio-display' onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\" id=\"divcustomflat-radio-004-" + i.ToString() + "\"></div><div style=\"margin-top: 2px;float:left;\" class='item-radio-display-text'>&nbsp;" + dsstyle.Tables[0].Rows[i]["name"].ToString().Replace(" ", "&nbsp;") + "</div></div>";
                        }
                        else
                        {
                            strcustomstring += "<div style=\"float:left;margin-bottom:5px;margin-left:50px;\" class='item-radio-display' onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','" + dsstyle.Tables[0].Rows[i]["name"].ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\" id=\"divcustomflat-radio-004-" + i.ToString() + "\"></div><div style=\"margin-top: 2px;float:left;\" class='item-radio-display-text'>&nbsp;" + dsstyle.Tables[0].Rows[i]["name"].ToString().Replace(" ", "&nbsp;") + "</div></div>";
                        }

                    }
                    strcustomstring = strcustomstring.Replace("###option1###", strnewoption);
                    //strcustomstring += "<div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined','divcustomflat-radio-004-1','004');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-1\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined','divcustomflat-radio-004-1','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Lined</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined &amp; Interlined','divcustomflat-radio-004-2','004');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-2\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined &amp; Interlined','divcustomflat-radio-004-2','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Lined &amp; Interlined</div></div><div onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Blackout Lining','divcustomflat-radio-004-3','004');ChangeCustomprice();\" class=\"item-radio-display\" style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-3\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Blackout Lining','divcustomflat-radio-004-3','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div class=\"item-radio-display-text\" style=\"margin-top: 2px;\">&nbsp;Blackout Lining</div></div>";
                    strcustomstring += "<div style=\"float: right; width: 100%; margin-bottom: 5px; text-align: right;display:none;\">";
                    strcustomstring += "<img border=\"0\" onclick=\"if(document.getElementById('ContentPlaceHolder1_ddlcustomoptin') != null &amp;&amp; document.getElementById('ContentPlaceHolder1_ddlcustomoptin').selectedIndex == 0){jAlert('Please Select Options.','Message');}else{varianttabhideshowcustom(4);}\" style=\"cursor: pointer;\" title=\"Next\" alt=\"Next\" src=\"/images/next-option.png\">";
                    strcustomstring += "</div>";
                    strcustomstring += "</div>";

                    strcustomstring += " <div id=\"spancolspancustomvalue-3\" style=\"margin-top: 10px;\">";
                    strcustomstring += "</div>";
                    strcustomstring += " </div>";
                    inum++;
                }

                //strcustomstring += "<div style=\"display:none;\" class=\"readymade-detail-pt1\">";
                //strcustomstring += "<div onclick=\"varianttabhideshowcustom(4);\" id=\"divcolspancustom-4\" class=\"readymade-detail-pt1-pro\">";
                //strcustomstring += "<span id=\"spancolspancustom-4\"></span>Quantity (Panels):1";
                //strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px;\" id=\"divselvaluecustom-4\"></div>";
                //strcustomstring += "</div>";

                //strcustomstring += "<div style=\"display: none;\" id=\"divcolspancustomvalue-4\" class=\"readymade-detail-right-pro\">";



                //strcustomstring += "</div>";
                //strcustomstring += "</div>";

                strcustomstring += " <div class=\"readymade-detail-pt1\">";

                strcustomstring += "<div   id=\"divcaddtocart\" class=\"readymade-detail-pt1-pro active\">";
                strcustomstring += "<span id=\"spanaddtocart\">" + inum.ToString() + "</span>ADD TO CART";
                strcustomstring += "<div style=\"float: right; line-height: 25px; padding-right: 2px; padding-top: 2px;\"></div>";
                strcustomstring += "</div>";

                strcustomstring += "<div  class=\"readymade-detail-right-pro custom-detail-row\">";
                strcustomstring += "<div style=\"margin-left: 4%;\" class=\"price-detail-left\">";
                strcustomstring += "<div class=\"item-pricepro\">";
                strcustomstring += " <div class=\"item-pricepro-left\">";

                strcustomstring += "<div class=\"quantit-pro\" style=\"font-size:14px !important;\"> <span>Quantity :</span>";
                strcustomstring += "<div id=\"uniform-lift_pos_select_right\" class=\"selector fixedWidth\">";
                strcustomstring += " <span id=\"dlcustomqty-ddl\" style=\"-moz-user-select: none;\">1</span>";
                strcustomstring += " <select style=\"width: 200px !important;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_dlcustomqty\" name=\"ctl00$ContentPlaceHolder1$dlcustomqty\">";
                strcustomstring += "<option value=\"\">Quantity</option>";
                strcustomstring += "<option value=\"1\" selected=\"selected\">1</option>";
                strcustomstring += "<option value=\"2\">2</option>";
                strcustomstring += "<option value=\"3\">3</option>";
                strcustomstring += "<option value=\"4\">4</option>";
                strcustomstring += "<option value=\"5\">5</option>";
                strcustomstring += "<option value=\"6\">6</option>";
                strcustomstring += "<option value=\"7\">7</option>";
                strcustomstring += "<option value=\"8\">8</option>";
                strcustomstring += "<option value=\"9\">9</option>";
                strcustomstring += "<option value=\"10\">10</option>";
                strcustomstring += "<option value=\"11\">11</option>";
                strcustomstring += "<option value=\"12\">12</option>";
                strcustomstring += "<option value=\"13\">13</option>";
                strcustomstring += "<option value=\"14\">14</option>";
                strcustomstring += "<option value=\"15\">15</option>";
                strcustomstring += "<option value=\"16\">16</option>";
                strcustomstring += "<option value=\"17\">17</option>";
                strcustomstring += "<option value=\"18\">18</option>";
                strcustomstring += "<option value=\"19\">19</option>";
                strcustomstring += "<option value=\"20\">20</option>";

                strcustomstring += "</select>";
                strcustomstring += "</div>";
                strcustomstring += "</div>";
                strcustomstring += "<div class=\"quantit-pro\"  style=\"font-size:14px !important;\">";
                strcustomstring += "<div id=\"divcustomprice\" class=\"selector fixedWidth regularprice-pro\" style=\"background:none;\"><label style=\"float: left;\">$0.00</label><span style=\"float: left; background: none repeat scroll 0% 0% transparent;width:auto;\">" + strpricetag + "</span>";
                strcustomstring += "</div>";
                strcustomstring += "</div>";

                strcustomstring += "<div id=\"divmeadetomeasure\" style=\"display: none; margin-bottom: 5px;\" class=\"listedprice-pro\">";
                strcustomstring += "</div>";

                strcustomstring += "</div>";

                strcustomstring += "</div>";


                strcustomstring += " </div>";



                strcustomstring += "<div class=\"custom-cart-total\"  style=\"display:none;\">";
                strcustomstring += "<div class=\"custom-cart-total-row\">";
                strcustomstring += "<div class=\"custom-cart-total-left\"> <span>TOTAL:</span></div>";
                strcustomstring += "<div class=\"custom-cart-total-right\"><div id=\"divtotalcustomprice\" class=\"total\">$0.00</div></div>";
                strcustomstring += "</div>";
                strcustomstring += "</div>";
                strcustomstring += "<div class=\"custom-detail-add-to-cart-button\">";
                strcustomstring += " <input type=\"image\" onclick=\"InsertProductCustom(" + strProductId + ",'ContentPlaceHolder1_btnAddTocartMade'); return false;\" alt=\"ADD TO CART\" src=\"/images/item-add-to-cart.jpg\" class=\"price-detail-right\" title=\"ADD TO CART\" id=\"ContentPlaceHolder1_btnAddTocartMade\" name=\"ctl00$ContentPlaceHolder1$btnAddTocartMade\">";
                strcustomstring += "</div>";


                strcustomstring += "</div>";
                strcustomstring += "</div>";




            }


            strcustomstring += "</div>";

            strcustomstring += "<div id=\"prepage\" style=\"position: absolute; font-family: arial; font-size: 16; left: 0px; top: 0px;  height: 100%; width: 100%; z-index: 1000; display: none;\"><table width=\"100%\" style=\"padding-top: 30px;\" align=\"center\"><tr><td align=\"center\" style=\"color: #fff;\" valign=\"middle\"><img alt=\"\" src=\"/images/option_loading.gif\" /><br /> <b>Loading ... ... Please wait!</b></td></tr></table></div>";

            strcustomstring += "</div>";
            strcustomstring += "<div style=\"display:none;\">~~h-" + strProductId.ToString() + "-h~~</div>";
            strcustomstring += "<div style=\"display:none;\">~~p-" + swatchid.ToString() + "-p~~</div>";
            strcustomstring += "<div style=\"display:none;\">~~n-" + nextnewId.ToString() + "-n~~</div>";
            //if (PreviousId - 21 <= 0)
            //{
            //    strcustomstring += "<div style=\"display:none;\">~~pr-0-pr~~</div>";
            //}
            //else
            {
                strcustomstring += "<div style=\"display:none;\">~~pr-" + nextnewIdend.ToString() + "-pr~~</div>";
            }

            strcustomstring = strcustomstring.Replace("###displaypre###", "  style='display:none;'");
            strcustomstring = strcustomstring.Replace("###displaynext###", " style='display:none;'");

            HttpContext.Current.Session["ptID"] = strProductId.ToString();
            return strcustomstring;

        }
        [System.Web.Services.WebMethod]
        public static string getCustomfabric(string strFabriccategoryId)
        {
            //ContentPlaceHolder1_ddlfabric

            string strcustomstring = "";
            DataSet dsgetall1 = new DataSet();
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Fabric  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'') <>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Fabric,'')<>'' UNION  SELECT 'Other' as Fabric  UNION  SELECT 'Fabric' as Fabric) as A Order By case when  Fabric='Other' then 2 when  Fabric='Fabric' then 0 else 1 end");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Fabric  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'') <>''  and isnull(deleted,0)=0 and StoreId=1 and isnull(Fabric,'')<>'' and isnull(FabricType,'')='" + strFabriccategoryId + "'  UNION  SELECT 'Other' as Fabric  UNION  SELECT 'Fabric' as Fabric) as A Order By case when  Fabric='Other' then 2 when  Fabric='Fabric' then 0 else 1 end");
            }
            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsgetall1.Tables[0].Rows.Count; i++)
                {
                    strcustomstring += dsgetall1.Tables[0].Rows[i]["Fabric"].ToString() + ",";
                }

            }

            return strcustomstring;
        }
        [System.Web.Services.WebMethod]
        public static string getCustompattern(string strFabriccategoryId)
        {
            //ContentPlaceHolder1_ddlfabric

            string strcustomstring = "";
            DataSet dsgetall1 = new DataSet();
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Pattern  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Pattern,'')<>'' UNION  SELECT 'Other' as Pattern  UNION  SELECT 'Pattern' as Pattern) as A Order By case when  Pattern='Other' then 2 when  Pattern='Pattern' then 0 else 1 end");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT A.* FROM (SELECT DISTINCT Pattern  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(FabricType,'')='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Pattern,'')<>'' UNION  SELECT 'Other' as Pattern  UNION  SELECT 'Pattern' as Pattern) as A Order By case when  Pattern='Other' then 2 when  Pattern='Pattern' then 0 else 1 end");
            }

            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsgetall1.Tables[0].Rows.Count; i++)
                {
                    strcustomstring += dsgetall1.Tables[0].Rows[i]["Pattern"].ToString() + ",";
                }

            }

            return strcustomstring;

        }
        [System.Web.Services.WebMethod]
        public static string getCustomcolor(string strFabriccategoryId)
        {
            //ContentPlaceHolder1_ddlfabric

            string strcustomstring = "Color,";
            DataSet dsgetall1 = new DataSet();
            if (strFabriccategoryId.ToString().ToLower() == "all")
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT Colors+',' as SearchValue  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(deleted,0)=0 and StoreId=1 and isnull(Colors,'')<>''");
            }
            else
            {
                dsgetall1 = CommonComponent.GetCommonDataSet("SELECT Colors+',' as SearchValue  FROM tb_Product WHERE isnull(Ismadetomeasure,0)=1 and isnull(active,0)=1 and isnull(FabricType,'')<>'' and isnull(FabricType,'')='" + strFabriccategoryId + "' and isnull(deleted,0)=0 and StoreId=1 and isnull(Colors,'')<>''");
            }

            if (dsgetall1 != null && dsgetall1.Tables.Count > 0 && dsgetall1.Tables[0].Rows.Count > 0)
            {
                string strtt = ",";
                for (int m = 0; m < dsgetall1.Tables[0].Rows.Count; m++)
                {



                    string[] strcolor = dsgetall1.Tables[0].Rows[m]["SearchValue"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (strcolor.Length > 0)
                    {
                        bool flg = false;
                        foreach (string stt in strcolor)
                        {
                            if (!string.IsNullOrEmpty(stt))
                            {


                                if (strtt.ToString().ToLower().Trim().IndexOf("," + stt.ToLower().Trim() + ",") <= -1)
                                {


                                    strcustomstring += stt.ToString() + ",";

                                    strtt += stt.Trim() + ",";
                                }
                            }

                        }
                    }

                }
            }


            strcustomstring += "Other";
            return strcustomstring;

        }
        [System.Web.Services.WebMethod]
        public static string ChangePhoneOrderCouponPrice(Int32 CustomCartID, String price)
        {
            CommonComponent.ExecuteCommonData("update tb_ShoppingCartItems set DiscountPrice=" + price + " where CustomCartID=" + CustomCartID + "");
            return "";
        }

        /// <summary>
        /// WebMethod for Update Product Display Order
        /// </summary>
        /// <param name="string">productID</param>
        /// <param name="string">categoryID</param>
        [System.Web.Services.WebMethod]
        public static string updateproduct(string ids, string catid, string prtid)
        {
            if (ids != "")
            {

                CategoryComponent objCategory2 = new CategoryComponent();
                CommonComponent.ExecuteCommonData("Exec GuiSetProductDisplayOrder " + catid + ",'" + ids + "'");
                try
                {
                    string hdncategoryName = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT  Name FROM tb_Category   where CategoryID=" + catid + ""));
                    string hdncategoryName1 = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT  Name FROM tb_Category   where CategoryID=" + prtid + ""));
                    string srliveserver = System.Configuration.ConfigurationSettings.AppSettings["catcheservername"].ToString();
                    if (hdncategoryName.ToString().ToLower().IndexOf(" fabric") > -1)
                    {
                        var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + catid.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    else
                    {
                        var myUri = new Uri(srliveserver + "/category/category?catid=" + catid.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    if (hdncategoryName1.ToString().ToLower().IndexOf(" fabric") > -1)
                    {
                        var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + prtid.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    else
                    {
                        var myUri = new Uri(srliveserver + "/category/category?catid=" + prtid.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                }
                catch
                {

                }
                //objCategory2.GuiSetProductDisplayOrder(ids, Convert.ToInt32(catid));
            }
            return "";
        }

        /// <summary>
        /// WebMethod for Update Product Display Order
        /// </summary>
        /// <param name="string">productID</param>
        /// <param name="string">categoryID</param>
        [System.Web.Services.WebMethod]
        public static string updateproductByManual(string ids, string catid, string ProductID, string prtid)
        {
            if (ids != "")
            {

                CategoryComponent objCategory2 = new CategoryComponent();
                CommonComponent.ExecuteCommonData("Exec GuiProductDisplayOrderByTextBox " + catid + "," + ids + "");
                //objCategory2.SetCategoryDisplayOrder(ids, Convert.ToInt32(catid));

                CommonComponent.ExecuteCommonData("update tb_ProductCategory set DisplayOrder=" + ids + " where CategoryID=" + catid + " and ProductID=" + ProductID + "");
                try
                {
                    string hdncategoryName =Convert.ToString(CommonComponent.GetScalarCommonData("SELECT  Name FROM  tb_Category   where CategoryID=" + catid + ""));
                    string hdncategoryName1 = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT  Name FROM tb_Category   where CategoryID=" + prtid + ""));
                    string srliveserver = System.Configuration.ConfigurationSettings.AppSettings["catcheservername"].ToString();
                    if (hdncategoryName.ToString().ToLower().IndexOf(" fabric") > -1)
                    {
                        var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + catid.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    else
                    {
                        var myUri = new Uri(srliveserver + "/category/category?catid=" + catid.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    if (hdncategoryName1.ToString().ToLower().IndexOf(" fabric") > -1)
                    {
                        var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + prtid.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    else
                    {
                        var myUri = new Uri(srliveserver + "/category/category?catid=" + prtid.ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                }
                catch
                {

                }
                

            }
            return "";
        }
        /// <summary>
        /// WebMethod for Update Category Display Order
        /// </summary>
        /// <param name="string">ids</param>
        /// <param name="string">Parentcatid</param>
        [System.Web.Services.WebMethod]
        public static string updatecategorydisplay(string ids, string Parentcatid, string ppid)
        {
            if (ids != "")
            {
                string hdncategoryName = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT  Name FROM tb_Category   where CategoryID=" + Parentcatid + ""));
                string hdncategoryName1 = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT  Name FROM tb_Category   where CategoryID=" + ppid + ""));
                
                CommonComponent.ExecuteCommonData("Exec GuiSetCategoryDisplayOrder '" + ids + "'");
                string[] id = ids.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string srliveserver = System.Configuration.ConfigurationSettings.AppSettings["catcheservername"].ToString();
                for (int i = 0; i < id.Length; i++)
                {
                    if (hdncategoryName.ToString().ToLower().IndexOf(" fabric") > -1)
                    {
                        var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + id[i].ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                    else
                    {
                        var myUri = new Uri(srliveserver + "/category/category?catid=" + id[i].ToString().Trim() + "&catchupdate=1");
                        // Create a 'HttpWebRequest' object for the specified url. 
                        var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                        // Set the user agent as if we were a web browser
                        myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                        var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                        var stream = myHttpWebResponse.GetResponseStream();

                        myHttpWebResponse.Close();
                    }
                }

                if (hdncategoryName1.ToString().ToLower().IndexOf(" fabric") > -1)
                {
                    var myUri = new Uri(srliveserver + "/fabriclist/category?catid=" + ppid.ToString().Trim() + "&catchupdate=1");
                    // Create a 'HttpWebRequest' object for the specified url. 
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                    // Set the user agent as if we were a web browser
                    myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                    var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    var stream = myHttpWebResponse.GetResponseStream();

                    myHttpWebResponse.Close();
                }
                else
                {
                    var myUri = new Uri(srliveserver + "/category/category?catid=" + ppid.ToString().Trim() + "&catchupdate=1");
                    // Create a 'HttpWebRequest' object for the specified url. 
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                    // Set the user agent as if we were a web browser
                    myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                    var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    var stream = myHttpWebResponse.GetResponseStream();

                    myHttpWebResponse.Close();
                }
                try
                {
                    var myUri = new Uri(srliveserver + "/home/index?publish=1");
                    // Create a 'HttpWebRequest' object for the specified url. 
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                    // Set the user agent as if we were a web browser
                    myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                    var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    var stream = myHttpWebResponse.GetResponseStream();

                    myHttpWebResponse.Close();
                }
                catch
                {

                }

                //CategoryComponent objCategory2 = new CategoryComponent();

                //objCategory2.GuiSetCategoryDisplayOrder(ids, Convert.ToInt32(Parentcatid));
            }
            return "";
        }
        [System.Web.Services.WebMethod]
        public static string updatecategorydisplaymenu(string ids, string disorer)
        {
            if (ids != "")
            {
             
               // CommonComponent.ExecuteCommonData("Exec GuiSetCategoryDisplayOrder '" + ids + "'");
                CommonComponent.ExecuteCommonData("update tb_Category SET DisplayOrder=" + disorer + " WHERE CategoryID=" + ids);
                string srliveserver = System.Configuration.ConfigurationSettings.AppSettings["catcheservername"].ToString();
                 
                try
                {
                    var myUri = new Uri(srliveserver + "/home/index?publish=1");
                    // Create a 'HttpWebRequest' object for the specified url. 
                    var myHttpWebRequest = (HttpWebRequest)WebRequest.Create(myUri);
                    // Set the user agent as if we were a web browser
                    myHttpWebRequest.UserAgent = @"(Windows; U; Windows NT 5.1; en-US; rv:1.8.0.4) Gecko/20060508 Firefox/1.5.0.4"; //Mozilla/5.0 

                    var myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                    var stream = myHttpWebResponse.GetResponseStream();

                    myHttpWebResponse.Close();
                }
                catch
                {

                }

                //CategoryComponent objCategory2 = new CategoryComponent();

                //objCategory2.GuiSetCategoryDisplayOrder(ids, Convert.ToInt32(Parentcatid));
            }
            return "";
        }
        [System.Web.Services.WebMethod]
        public static string updatesaleseventdisplay(string ids, string eventid)
        {
            if (ids != "")
            {


                CommonComponent.ExecuteCommonData("Exec GuiSetEventDisplayOrder " + eventid + ",'" + ids + "'");
                //objCategory2.GuiSetProductDisplayOrder(ids, Convert.ToInt32(catid));
            }
            return "";
        }
        [System.Web.Services.WebMethod]
        public static string updatecollectiondisplay(string ids, string eventid)
        {
            if (ids != "")
            {


                CommonComponent.ExecuteCommonData("Exec GuiSetcollectionDisplayOrder " + eventid + ",'" + ids + "'");
                //objCategory2.GuiSetProductDisplayOrder(ids, Convert.ToInt32(catid));
            }
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string updatenewarrivaldisplayorder(string ids)
        {
            if (ids != "")
            {


                CommonComponent.ExecuteCommonData("Exec [GuiSetNewarrivalDisplayOrder] '" + ids + "'");
                //objCategory2.GuiSetProductDisplayOrder(ids, Convert.ToInt32(catid));
            }
            return "";
        }
        [System.Web.Services.WebMethod]
        public static string updatesalesoutletdisplayorder(string ids)
        {
            if (ids != "")
            {


                CommonComponent.ExecuteCommonData("Exec [GuiSetsalesoutletDisplayOrder] '" + ids + "'");
                //objCategory2.GuiSetProductDisplayOrder(ids, Convert.ToInt32(catid));
            }
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string updatecustompagedisplayorder(string ids)
        {
            if (ids != "")
            {


                CommonComponent.ExecuteCommonData("Exec [GuiSetcustompageDisplayOrder] '" + ids + "'");
                //objCategory2.GuiSetProductDisplayOrder(ids, Convert.ToInt32(catid));
            }
            return "";
        }
        [System.Web.Services.WebMethod]
        public static string updateonsaledisplayorder(string ids)
        {
            if (ids != "")
            {


                CommonComponent.ExecuteCommonData("Exec [GuiSetonsaleDisplayOrder] '" + ids + "'");
                //objCategory2.GuiSetProductDisplayOrder(ids, Convert.ToInt32(catid));
            }
            return "";
        }

        [System.Web.Services.WebMethod]
        public static string setdisplayorderoption(string option, string pagename)
        {


            if (option.ToString().ToLower() == "name")
            {
                CommonComponent.ExecuteCommonData("update tb_custompagemaster set Orderbycustom=0,Orderbyname=1,orderbyprice=0 where pagename='" + pagename.ToString().ToLower() + "'");
            }
            else if (option.ToString().ToLower() == "price")
            {
                CommonComponent.ExecuteCommonData("update tb_custompagemaster set Orderbycustom=0,Orderbyname=0,orderbyprice=1 where pagename='" + pagename.ToString().ToLower() + "'");
            }
            else
            {
                CommonComponent.ExecuteCommonData("update tb_custompagemaster set Orderbycustom=1,Orderbyname=0,orderbyprice=0 where pagename='" + pagename.ToString().ToLower() + "'");
            }
            return "";
        }



        [System.Web.Services.WebMethod]
        public static string gettradereport(string fromdate, string todate, string search, string flag)
        {


            DataSet dsMail = new DataSet();
            if (flag == "0")
            {
                dsMail = CommonComponent.GetCommonDataSet("Exec GuiGetTradelistReport '" + Convert.ToDateTime(fromdate) + "','" + Convert.ToDateTime(todate) + "','" + search.Replace("'", "''") + "'");
            }
            else
            {
                dsMail = CommonComponent.GetCommonDataSet("Exec GuiGetTradelistReport '','',''");
            }

            //dsMail = CommonComponent.GetCommonDataSet("select top 10 Email,CustomerID from tb_Customer");
            //dsMail = CommonComponent.GetCommonDataSet("Exec GuiGetTradelistReport '','',''");
            if (dsMail != null && dsMail.Tables.Count > 0 && dsMail.Tables[0].Rows.Count > 0)
            {
                string JSONString = string.Empty;

                JSONString = JsonConvert.SerializeObject(dsMail.Tables[0]);
                return JSONString;
            }
            return "";
        }
        #region fabricyard
        [System.Web.Services.WebMethod]
        public static string GetDataAdminfabric(Int32 ProductId, Int32 Qty)
        {
            string resp = string.Empty;
            DataSet ds = new DataSet();
            Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
            ds = objSql.GetDs("EXEC GuiGetProductFabricDetailsForPID " + ProductId + "");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0]["yardprice"].ToString()));
                resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0]["yardprice"].ToString()) * Convert.ToDecimal(Qty));
            }

            return resp;
        }
        #endregion

    }
}