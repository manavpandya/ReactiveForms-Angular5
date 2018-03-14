using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Data;

namespace Solution.UI.Web
{
    public partial class MiniCartCall : System.Web.UI.Page
    {
        bool outofstock = false;
        bool IsRestrictedAll = false;
        Int32 TotalQuantityValue = 0;
        string StrContact = "";
        int Yardqty = 0;
        double actualYard = 0;
        string strDatenew = "";
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            StrContact = "1-866-413-7273";
            if (!string.IsNullOrEmpty(AppLogic.AppConfigs("StorePhoneNumber").ToString()))
            {
                StrContact = AppLogic.AppConfigs("StorePhoneNumber").ToString();
            }
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "Insert")
            {
                AddTocart();
            }
            else if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "InsertMulti")
            {
                AddTocartMulti();
            }
            else if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "InsertMultiSwatch")
            {
                AddToCartMultiSwatch();
            }
            else if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "Update" && Session["CustId"] != null)
            {
                UpdateShoppingCart(Request.QueryString["Products"].ToString(), Session["CustId"].ToString());
            }
            else if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "Delete")
            {
                DeleteCart(Convert.ToInt32(Request.QueryString["Products"].ToString()));
            }
        }

        /// <summary>
        /// Checks the Inventory.
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="CustomerId">int CustomerId</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>Returns true if sufficient Product Available, false otherwise</returns>
        private bool CheckInventory(Int32 ProductID, Int32 CustomerId, Int32 Qty)
        {
            strDatenew = "";
            if (Request.QueryString["VariantValues"] != null && Request.QueryString["VariantValues"].ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
            {
                Qty = Qty * 2;
            }

            Int32 AssemblyProduct = 0, ReturnQty = 0;
            AssemblyProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect COUNT(*) From tb_product Where ProductId=" + ProductID + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and ProductTypeID in (Select ProductTypeID From tb_ProductType where Name='Assembly Product')"));
            //if (AssemblyProduct > 0)
            //{
            //    ReturnQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_Check_ProductAssemblyInventory " + ProductID + "," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + "," + CustomerId + ",1"));
            //    TotalQuantityValue = ReturnQty;
            //    if (ReturnQty <= 0)
            //    {
            //        TotalQuantityValue = 0;
            //        return false;
            //    }
            //    else if (Qty > ReturnQty)
            //    {
            //        return false;
            //    }
            //    else
            //    { return true; }
            //}
            //else
            {
                Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
                                                     " WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + CustomerId + ") " +
                                                     " AND ProductID=" + ProductID + " AND VariantNames like '" + Request.QueryString["VariantNames"].ToString().Replace("'", "''").Replace("~hpd~", "-") + "%' AND VariantValues like '" + Request.QueryString["VariantValues"].ToString().Replace("'", "''").Replace("~hpd~", "-") + "%'"));
                Qty = Qty + ShoppingCartQty;

                DataSet dscount = new DataSet();
                Int32 alinv = 0;
                if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"].ToString() == "1")
                {
                    string[] strNmyard = Request.QueryString["VariantNames"].ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] strValyeard = Request.QueryString["VariantValues"].ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //if (strValyeard.Length > 0)
                    //{
                    //    if (strValyeard.Length == strNmyard.Length)
                    //    {
                    //        for (int j = 0; j < strNmyard.Length; j++)
                    //        {
                    //            if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                    //            {
                    //                if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                    //                {
                    //                    string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                    //                    dscount = CommonComponent.GetCommonDataSet("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + " AND AllowQuantity >= " + Qty + "");
                    //                    alinv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                    //                }
                    //                else
                    //                {
                    //                    string strvalue = strValyeard[j].ToString().Trim();
                    //                    dscount = CommonComponent.GetCommonDataSet("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + " AND AllowQuantity >= " + Qty + "");
                    //                    alinv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                    //                }
                    //            }

                    //        }
                    //    }
                    //}

                }
                else
                {
                    dscount = CommonComponent.GetCommonDataSet("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + " AND Inventory >= " + Qty + "");
                }
                if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
                {
                   // return true;

                    #region swatch-hemming
                    if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"].ToString() == "0")
                    {

                        Int32 IsHemming = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,0) as ConfigValue from tb_AppConfig where ConfigName='IshemmingActive'"));
                        if (IsHemming == 1)
                        {
                            int SwatchHemmInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(HammingSafetyQty,0) as HammingSafetyQty FROM tb_product WHERE isnull(IsHamming,0)=1 and ProductId=" + ProductID + ""));
                            int CntInv = Convert.ToInt32(dscount.Tables[0].Rows[0][0].ToString()) - SwatchHemmInv;
                            if (CntInv >= Qty)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return true;
                        }
                    }
                    else
                    {
                        return true;
                    }

                    #endregion

                }
                else
                {
                    if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"].ToString() == "1")
                    {
                        //CommonComponent.ExecuteCommonData("INSERT INTO temp_Qty(IDname,Qty) values ('" + Request.QueryString["VariantValues"].ToString().Replace("'", "''") + "','" + TotalQuantityValue.ToString() + "')");
                        string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + ProductID.ToString() + ""));
                        if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder))
                        {

                            string[] strNmyard = Request.QueryString["VariantNames"].ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] strValyeard = Request.QueryString["VariantValues"].ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strValyeard.Length > 0)
                            {
                                if (strValyeard.Length == strNmyard.Length)
                                {
                                    for (int j = 0; j < strNmyard.Length; j++)
                                    {
                                        if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                                        {
                                            if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                            {
                                                string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                                strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));

                                                int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                                                //TotalQuantityValue = Convert.ToInt32(CntInv);

                                                DataSet dsUPC = new DataSet();
                                                dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue + "' AND ProductId=" + ProductID + "");
                                                string upc = "";
                                                string Skuoption = "";
                                                if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                {
                                                    upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                    Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                    if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                    {
                                                        string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                        if (!string.IsNullOrEmpty(strQty))
                                                        {
                                                            try
                                                            {
                                                                TotalQuantityValue = Convert.ToInt32(strQty.ToString());
                                                                //CommonComponent.ExecuteCommonData("INSERT INTO temp_Qty(IDname,Qty) values ('" + strvalue.Replace("'", "''") + "','" + TotalQuantityValue.ToString() + "')");
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                }


                                                if (TotalQuantityValue <= 0)
                                                {
                                                    TotalQuantityValue = 0;
                                                }
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
                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductID + ",50," + Length.ToString() + "," + Qty.ToString() + ",'Pole Pocket','Lined'");
                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                {
                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                }
                                                Int32 OrderQty = Qty;
                                                if (resp != "")
                                                {
                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                }
                                                Yardqty = OrderQty;
                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProductID.ToString() + " "));
                                                if (!string.IsNullOrEmpty(strDatenew))
                                                {
                                                    ViewState["AvailableDate"] = strDatenew;
                                                    return true;
                                                }
                                                else if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                {

                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return false;
                                                }
                                                else
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return true;
                                                }
                                            }
                                            else
                                            {
                                                string strvalue = strValyeard[j].ToString().Trim();
                                                strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                                                //TotalQuantityValue = Convert.ToInt32(CntInv);
                                                strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProductID + ""));
                                                DataSet dsUPC = new DataSet();
                                                dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue  + "' AND  ProductId=" + ProductID + "");
                                                string upc = "";
                                                string Skuoption = "";
                                                if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                {
                                                    upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                    Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                    if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                    {
                                                        string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                        if (!string.IsNullOrEmpty(strQty))
                                                        {
                                                            try
                                                            {
                                                                TotalQuantityValue = Convert.ToInt32(strQty.ToString());
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                }

                                                if (TotalQuantityValue <= 0)
                                                {
                                                    TotalQuantityValue = 0;
                                                }
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
                                                dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProductID + ",50," + Length.ToString() + "," + Qty.ToString() + ",'Pole Pocket','Lined'");
                                                if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                {
                                                    resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                    actualYard = Convert.ToDouble(resp.ToString());
                                                }
                                                Int32 OrderQty = Qty;
                                                if (resp != "")
                                                {
                                                    OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                }
                                                Yardqty = OrderQty;
                                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProductID.ToString() + " "));
                                                if (!string.IsNullOrEmpty(strDatenew))
                                                {
                                                    ViewState["AvailableDate"] = strDatenew;
                                                    return true;
                                                }
                                                else if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return false;
                                                }
                                                else
                                                {
                                                    ViewState["AvailableDate"] = StrVendor;
                                                    return true;
                                                }
                                            }
                                        }

                                    }
                                }
                            }
                        }
                        else
                        {

                            string[] strNmyard = Request.QueryString["VariantNames"].ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] strValyeard = Request.QueryString["VariantValues"].ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strValyeard.Length > 0)
                            {
                                int warehouseId = 0;
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
                                            strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                            strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue + "' AND  ProductId=" + ProductID + ""));
                                            DataSet dsUPC = new DataSet();
                                            dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU,VariantValueID FROM tb_ProductVariantValue WHERE VariantValue='" + strvalue  + "' AND  ProductId=" + ProductID + "");
                                            string upc = "";
                                            string Skuoption = "";
                                            string Variantvalueid = "";
                                            if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                            {
                                                upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                Variantvalueid = Convert.ToString(dsUPC.Tables[0].Rows[0]["VariantValueID"].ToString());

                                                warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductVariantInventory inner join tb_WareHouse on tb_WareHouseProductVariantInventory.WareHouseID=tb_WareHouse.WareHouseID where VariantValueID=" + Convert.ToInt32(Variantvalueid) + " and tb_WareHouseProductVariantInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                if (warehouseId == 0)
                                                {
                                                    warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID in (" + ProductID + ") and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                }

                                                if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                {

                                                    string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                    if (!string.IsNullOrEmpty(strQty))
                                                    {
                                                        try
                                                        {
                                                            TotalQuantityValue = Convert.ToInt32(strQty.ToString());
                                                            // CommonComponent.ExecuteCommonData("INSERT INTO temp_Qty(IDname,Qty) values ('" + strvalue.Replace("'", "''") + "','" + TotalQuantityValue.ToString() + "')");
                                                        }
                                                        catch { }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    if (strNmyard.Length == 1 && strNmyard[0].ToString().ToLower().IndexOf("estimated") > -1)
                                    {
                                        int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + ""));
                                        TotalQuantityValue = Convert.ToInt32(CntInv);
                                        warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ProductID + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                        if (warehouseId == 17)
                                        {
                                            TotalQuantityValue = 9999;
                                        }

                                    }
                                    if (warehouseId == 17)
                                    {
                                        TotalQuantityValue = 9999;
                                    }
                                }
                            }
                            else
                            {
                                int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + ""));
                                TotalQuantityValue = Convert.ToInt32(CntInv);
                                int warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ProductID + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                if (warehouseId == 17)
                                {
                                    TotalQuantityValue = 9999;
                                }
                            }

                            if (TotalQuantityValue <= 0)
                            {
                                TotalQuantityValue = 0;
                            }
                            if (!string.IsNullOrEmpty(strDatenew))
                            {
                                ViewState["AvailableDate"] = strDatenew;
                                return true;
                            }
                            else if (TotalQuantityValue >= Qty)
                            {
                                // CommonComponent.ExecuteCommonData("INSERT INTO temp_Qty(IDname,Qty) values ('Qty:" + Qty.ToString().Replace("'", "''") + "','" + TotalQuantityValue.ToString() + "')");
                                return true;
                            }

                            //TotalQuantityValue = alinv;
                            return false;
                        }
                    }
                    else
                    {
                        int CntInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) as Inventory FROM tb_product WHERE ProductId=" + ProductID + ""));

                        #region swatch-hemming
                        if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"].ToString() == "0")
                        {

                            Int32 IsHemming = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,0) as ConfigValue from tb_AppConfig where ConfigName='IshemmingActive'"));
                            if (IsHemming == 1)
                            {
                                int SwatchHemmInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(HammingSafetyQty,0) as HammingSafetyQty FROM tb_product WHERE isnull(IsHamming,0)=1 and  ProductId=" + ProductID + ""));
                                CntInv = CntInv - SwatchHemmInv;
                            }
                        }

                        #endregion

                        TotalQuantityValue = Convert.ToInt32(CntInv);
                        if (TotalQuantityValue <= 0)
                        {
                            TotalQuantityValue = 0;
                        }
                        if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"].ToString() == "3")
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// Adds the customer Temporary
        /// </summary>
        private void AddCustomer()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Customer objCust = new tb_Customer();
            Int32 CustID = -1;
            CustID = objCustomer.InsertCustomer(objCust, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            Session["CustID"] = CustID.ToString();
            System.Web.HttpCookie custCookie = new System.Web.HttpCookie("ecommcustomer", CustID.ToString());
            custCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(custCookie);

        }

        /// <summary>
        /// Deletes the Cart By CustomCartID
        /// </summary>
        /// <param name="CustomCartid">int CustomCartid</param>
        private void DeleteCart(Int32 CustomCartid)
        {
            ShoppingCartComponent objCart = new ShoppingCartComponent();

            objCart.DeleteCartItemByCustomCartID(Convert.ToInt32(CustomCartid));

            clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(Session["CustID"]));
            Response.Clear();
            Response.Write(objMiniCart.GetMiniCart());
            Response.End();



        }

        /// <summary>
        /// Adds Selected Product into the Cart
        /// </summary>
        private void AddTocart()
        {
            ViewState["AvailableDate"] = null;
            bool IsRestricted = true;
            bool isDropshipProduct = false;
            Int32 Isorderswatch1 = 0;
            IsRestricted = CheckCustomerIsRestricted();
            if (!IsRestricted)
            {
                if (Session["CustID"] == null || Session["CustID"].ToString() == "")
                {
                    AddCustomer();
                }

                string VariantValueId = "";
                string VariantNameId = "";
                string VariantQty = "";
                bool check = false;
                string[] strPIds = { "" };
                string[] strPrices = { "" };
                string[] strQuantitys = { "" };
                Yardqty = 0;
                string VariantValueIdtemp = "";
                actualYard = 0;

                if (Request.QueryString["ProdID"] != null && Request.QueryString["Price"] != null && Request.QueryString["Quantity"] != null)
                {
                    strPIds = Request.QueryString["ProdID"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    strPrices = Request.QueryString["Price"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    strQuantitys = Request.QueryString["Quantity"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    return;
                }



                for (int k = 0; k < strPIds.Length; k++)
                {
                    if (Request.QueryString["ProductType"] == null)
                    {
                        String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustId"].ToString() + " Order By ShoppingCartID DESC) "));
                        //if (!string.IsNullOrEmpty(strswatchQtyy) && (Convert.ToInt32(strswatchQtyy) + Convert.ToInt32(strQuantitys[k].ToString())) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()))
                        //{
                        strPrices[k] = "0.00";
                        //}
                        //else
                        //{
                        //strPrices[k] = "0.00";
                        //}
                    }
                    else
                    {
                        Isorderswatch1 = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[k].ToString() + " and ItemType='Swatch'"));
                        if (Isorderswatch1 == 1)
                        {
                            strPrices[k] = "0.00";
                        }
                    }
                    isDropshipProduct = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(IsdropshipProduct,0) FROM tb_product WHERE productid=" + Convert.ToInt32(strPIds[k].ToString()).ToString() + ""));

                    if (Request.QueryString["ProductType"] != null && (Request.QueryString["ProductType"] == "2")) // Made to Measure
                    {
                        DataSet DSFabricDetails = CommonComponent.GetCommonDataSet("Select FabricCode,FabricType  from tb_product where Productid=" + Convert.ToInt32(strPIds[k].ToString()) + " and StoreId=" + AppConfig.StoreID + "");
                        if (DSFabricDetails != null && DSFabricDetails.Tables.Count > 0 && DSFabricDetails.Tables[0].Rows.Count > 0)
                        {
                            string FabricCode = Convert.ToString(DSFabricDetails.Tables[0].Rows[0]["FabricCode"]);
                            string FabricType = Convert.ToString(DSFabricDetails.Tables[0].Rows[0]["FabricType"]);
                            Int32 FabricTypeID = 0;
                            if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
                            {
                                FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
                                if (FabricTypeID > 0)
                                {
                                    DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
                                    Int32 QtyOnHand = 0, NextOrderQty = 0, TotalQty = 0;
                                    Int32 OrderQty = Convert.ToInt32(strQuantitys[k].ToString());

                                    if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
                                            QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);

                                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
                                            NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
                                    }
                                    Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
                                                " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + Convert.ToInt32(Session["CustID"]) + " Order By ShoppingCartID) " +
                                                " AND ProductID=" + Convert.ToInt32(strPIds[k].ToString()) + " AND VariantNames like '" + Request.QueryString["VariantNames"].ToString().Replace("'", "''").Replace("~hpd~", "-") + "%' AND VariantValues like '" + Request.QueryString["VariantValues"].ToString().Replace("'", "''") + "%'"));
                                    OrderQty = OrderQty + ShoppingCartQty;
                                    TotalQty = QtyOnHand + NextOrderQty;
                                    try
                                    {
                                        string Style = "";
                                        double Width = 0;
                                        double Length = 0;
                                        string Options = "";
                                        string[] strNmyard = Request.QueryString["VariantNames"].ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string[] strValyeard = Request.QueryString["VariantValues"].ToString().Replace("'", "''").Replace("~hpd~", "-").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        if (strNmyard.Length > 0)
                                        {
                                            if (strValyeard.Length == strNmyard.Length)
                                            {
                                                for (int j = 0; j < strNmyard.Length; j++)
                                                {
                                                    if (strNmyard[j].ToString().ToLower() == "width")
                                                    {
                                                        Width = Convert.ToDouble(strValyeard[j].ToString());
                                                    }
                                                    if (strNmyard[j].ToString().ToLower() == "length")
                                                    {
                                                        Length = Convert.ToDouble(strValyeard[j].ToString());
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
                                        if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "")
                                        {

                                            DataSet dsYard = new DataSet();
                                            dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + strPIds[k].ToString() + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
                                            if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                            {
                                                //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                                                resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                actualYard = Convert.ToDouble(resp.ToString());
                                            }
                                        }
                                        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        //{
                                        //    //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                                        //    resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0][1].ToString()));
                                        //}
                                        if (resp != "")
                                        {
                                            OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));

                                        }
                                    }
                                    catch
                                    {
                                    }
                                    Yardqty = OrderQty;
                                    if (Request.QueryString["VariantValues"] != null && Request.QueryString["VariantValues"].ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                                    {
                                        OrderQty = OrderQty * 2;
                                    }

                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + strPIds[k].ToString() + " "));

                                    if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                                    {
                                        if (isDropshipProduct == false)
                                        {
                                            outofstock = true;
                                        }
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                    else
                                    {
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (CheckInventory(Convert.ToInt32(strPIds[k].ToString()), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strQuantitys[k].ToString())))
                        {

                        }
                        else
                        {
                            if (isDropshipProduct == false)
                            {
                                outofstock = true;
                            }
                        }
                    }
                }
                if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"] == "1" && ViewState["AvailableDate"] == null) // Made to Measure
                {
                    DateTime dtnew = DateTime.Now.Date.AddDays(12);
                    ViewState["AvailableDate"] = Convert.ToString(dtnew);
                    //for (int k = 0; k < strPIds.Length; k++)
                    //{
                    //    Int32 OrderQty = Convert.ToInt32(strQuantitys[k].ToString());
                    //    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + strPIds[k].ToString() + " "));
                    //    if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                    //    {
                    //        ViewState["AvailableDate"] = StrVendor;
                    //    }
                    //    else
                    //    {
                    //        ViewState["AvailableDate"] = StrVendor;
                    //    }
                    //}
                }
                if (outofstock)
                {
                    Response.Clear();
                    string StrRetValue = "";
                    StrRetValue += "<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                    StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                    StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                    StrRetValue += "<div class=\"description_box_border\">";
                    StrRetValue += "<br>";
                    StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                    StrRetValue += "<tbody>";
                    StrRetValue += "<tr>";
                    StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                    StrRetValue += "<div>This product is out of stock.</div>";
                    StrRetValue += "</td>";
                    StrRetValue += "</tr>";
                    StrRetValue += "<tr>";
                    StrRetValue += "<td valign=\"middle\" align=\"center\">";
                    StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                    StrRetValue += "</td>";
                    StrRetValue += "</tr>";
                    StrRetValue += "<tr>";
                    StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                    StrRetValue += "</tr>";
                    StrRetValue += "</tbody></table>";
                    StrRetValue += "</div>";
                    StrRetValue += "</div>";

                    Response.Write("<div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                    Response.End();
                }
                if (Request.QueryString["Quantity"].ToString().Trim() != "" && outofstock == false)
                {
                    decimal LengthStdAllow = 0;
                    for (int j = 0; j < strPIds.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(strPIds[j].ToString()) && Session["CustID"] != null)
                        {
                            VariantNameId = "";
                            VariantValueId = "";
                            ShoppingCartComponent objShopping = new ShoppingCartComponent();
                            decimal price = Convert.ToDecimal(strPrices[j].ToString());
                            string avail = "";
                            decimal fbyardcost = 0;
                            int Qty = Convert.ToInt32(strQuantitys[j].ToString());
                            if (j == 0)
                            {
                                if (Request.QueryString["VariantNames"] != null && !string.IsNullOrEmpty(Request.QueryString["VariantNames"].ToString()))
                                {
                                    string[] strNm = Request.QueryString["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    string[] strVal = Request.QueryString["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    if (strNm.Length > 0)
                                    {
                                        if (strVal.Length == strNm.Length)
                                        {

                                            int RomanShadeId = 0; //Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(RomanShadeId,0) as RomanShadeId from tb_product where ProductId=" + Convert.ToInt32(strPIds[j].ToString()) + " and StoreId=" + AppConfig.StoreID + ""));
                                            for (int pp = 0; pp < strNm.Length; pp++)
                                            {
                                                if (strNm[pp].ToString().ToLower().IndexOf("roman shade design") > -1)
                                                {
                                                    RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select TOp 1 ISNULL(RomanShadeId,0) as RomanShadeId from tb_ProductRomanShadeYardage where ShadeName='" + strVal[pp].ToString().Trim() + "' AND isnull(Active,0)=1"));
                                                    //break;
                                                }
                                                if (strNm[pp].ToString().ToLower().IndexOf("color") > -1)
                                                {
                                                    fbyardcost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT isnull(VariantPrice,0) FROM tb_ProductVariantValue WHERE VariantValue='" + strVal[pp].ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(strPIds[j].ToString()) + ""));
                                                }


                                            }

                                            decimal VariValue = 0, WidthStdAllow = 0;
                                            bool optiontrue = false;
                                            for (int k = 0; k < strNm.Length; k++)
                                            {
                                                if (VariantNameId.ToString().ToLower().IndexOf(strNm[k].ToString().ToLower()) > -1)
                                                {
                                                    string[] strNmtemp = VariantNameId.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                    string[] strValtemp = VariantValueId.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                    VariantNameId = "";
                                                    VariantValueId = "";
                                                    for (int p = 0; p < strNmtemp.Length; p++)
                                                    {
                                                        if (strNmtemp[p].ToString().ToLower().IndexOf(strNm[k].ToString().ToLower()) > -1)
                                                        {
                                                            VariantNameId += strNmtemp[p].ToString() + ",";
                                                            VariantValueId += strValtemp[p].ToString() + " " + strVal[k].ToString() + ",";
                                                        }
                                                        else
                                                        {
                                                            VariantNameId += strNmtemp[p].ToString() + ",";
                                                            VariantValueId += strValtemp[p].ToString() + ",";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    VariantNameId = VariantNameId + strNm[k].ToString() + ",";
                                                    VariantValueId = VariantValueId + strVal[k].ToString() + ",";
                                                }


                                                // Roman Yardage ---------------------------------

                                                if (RomanShadeId > 0 || Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"] == "3")
                                                {
                                                    if (strNm[k].ToString().ToLower().Trim().IndexOf("width") > -1)
                                                    {
                                                        if (strVal[k].ToString().IndexOf("/") > -1)
                                                        {
                                                            string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                            strwidth = strwidth.Replace("/", "");
                                                            decimal tt = Convert.ToDecimal(strwidth);
                                                            strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                            tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                            decimal WidthStdAllow1 = tt;
                                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;
                                                        }
                                                        else
                                                        {
                                                            decimal WidthStdAllow1 = 0;
                                                            decimal.TryParse(strVal[k].ToString(), out WidthStdAllow1);
                                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;

                                                        }
                                                        //WidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + VariValue + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                    }
                                                    if (strNm[k].ToString().ToLower().Trim().IndexOf("length") > -1)
                                                    {
                                                        if (VariValue > Convert.ToDecimal(1))
                                                        {
                                                            if (strVal[k].ToString().IndexOf("/") > -1)
                                                            {
                                                                string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                                strwidth = strwidth.Replace("/", "");
                                                                decimal tt = Convert.ToDecimal(strwidth);
                                                                strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                                decimal VariValue1 = tt;
                                                                VariValue = VariValue + VariValue1;
                                                            }
                                                            else
                                                            {
                                                                decimal VariValue1 = 0;
                                                                decimal.TryParse(strVal[k].ToString(), out VariValue1);
                                                                VariValue = VariValue + VariValue1;

                                                            }

                                                            //decimal.TryParse(strVal[k].ToString(), out VariValue);
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
                                                                LengthStdAllow = Math.Round(LengthStdAllow, 2);

                                                                for (int p = 0; p < strNm.Length; p++)
                                                                {
                                                                    if (strNm[p].ToString().ToLower().Trim().IndexOf("option") > -1)
                                                                    {
                                                                        optiontrue = true;
                                                                        DataSet dsRoman = new DataSet();
                                                                        dsRoman = CommonComponent.GetCommonDataSet("Select isnull(FabricPerYardCost,0) as FabricPerYardCost,isnull(ManufuacturingCost,0) as ManufuacturingCost from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1");

                                                                        if (dsRoman != null && dsRoman.Tables.Count > 0 && dsRoman.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            decimal yardprice = Convert.ToDecimal(fbyardcost) * Convert.ToDecimal(LengthStdAllow);
                                                                            yardprice = yardprice + Convert.ToDecimal(dsRoman.Tables[0].Rows[0]["ManufuacturingCost"]);
                                                                            decimal rangecost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Cost,0) as Cost from tb_Roman_Shipping where Fromwidth <= " + WidthStdAllow + " And Towidth >=" + WidthStdAllow + " and ISNULL(Active,0)=1"));
                                                                            yardprice = yardprice + rangecost;
                                                                            decimal ProductOptionsPrice = 0;
                                                                            decimal Duties = 0;
                                                                            if (strVal[p].ToString().ToLower() == "lined")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else if (strVal[p].ToString().ToLower() == "lined & interlined" || strVal[p].ToString().ToLower() == "lined &amp; interlined")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  (Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' Or Options='" + strVal[p].ToString().ToLower().Replace("'", "''").Replace("&amp;", "&") + "') and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else if (strVal[p].ToString().ToLower() == "blackout lining")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            Duties = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Duties,0) as Duties from tb_ProductRomanShadeYardage where  RomanShadeId=" + RomanShadeId + ""));
                                                                            //
                                                                            if (Duties > decimal.Zero)
                                                                            {
                                                                                //ProductOptionsPrice = ProductOptionsPrice / Convert.ToDecimal(100);
                                                                                //decimal liningoption = (yardprice * ProductOptionsPrice) + yardprice;

                                                                                ////ProductOptionsPrice = Convert.ToDecimal(15) / Convert.ToDecimal(100);

                                                                                ////liningoption = liningoption * ProductOptionsPrice;
                                                                                ////liningoption = liningoption + yardprice;
                                                                                //price = liningoption;

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
                                                                        }
                                                                        break;
                                                                    }
                                                                }
                                                                if (optiontrue == false && VariValue == Decimal.Zero && WidthStdAllow == Decimal.Zero)
                                                                {
                                                                    DataSet dsRoman = new DataSet();
                                                                    dsRoman = CommonComponent.GetCommonDataSet("Select isnull(FabricPerYardCost,0) as FabricPerYardCost,isnull(ManufuacturingCost,0) as ManufuacturingCost from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1");

                                                                    if (dsRoman != null && dsRoman.Tables.Count > 0 && dsRoman.Tables[0].Rows.Count > 0)
                                                                    {

                                                                        decimal yardprice = Convert.ToDecimal(fbyardcost) * Convert.ToDecimal(LengthStdAllow);
                                                                        yardprice = yardprice + Convert.ToDecimal(dsRoman.Tables[0].Rows[0]["ManufuacturingCost"]);
                                                                        decimal rangecost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Cost,0) as Cost from tb_Roman_Shipping where Fromwidth <= " + WidthStdAllow + " And Towidth >=" + WidthStdAllow + " and ISNULL(Active,0)=1"));
                                                                        yardprice = yardprice + rangecost;

                                                                        decimal ProductOptionsPrice = 0;// Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0) as AdditionalPricePercentage from tb_ProductOptionsPrice where  ProductId=" + strPIds[j].ToString() + " and Options='" + strVal[p].ToString() + "'"));

                                                                        decimal liningoption = yardprice;
                                                                        ProductOptionsPrice = Convert.ToDecimal(15) / Convert.ToDecimal(100);
                                                                        liningoption = liningoption * ProductOptionsPrice;
                                                                        liningoption = liningoption + yardprice;
                                                                        price = liningoption;

                                                                    }
                                                                    optiontrue = true;
                                                                }
                                                                else
                                                                {
                                                                    if (optiontrue == false)
                                                                    {


                                                                        Decimal shademarkup = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));

                                                                        Decimal Pricepp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 value FROM tb_ShadeDetail WHERE ShadeWidthID in (SELECT ShadeWidthID FROM tb_Shadewidth WHERE ProductId=" + strPIds[j].ToString() + " and value=floor('" + WidthStdAllow.ToString() + "')) and ShadeLengthID in(SELECT ShadeLengthID FROM tb_ShadeLength WHERE ProductId=" + strPIds[j].ToString() + " and value=floor('" + VariValue.ToString() + "')) "));

                                                                        Decimal yourprice = Pricepp * shademarkup;
                                                                        price = yourprice;
                                                                        optiontrue = true;
                                                                    }
                                                                }
                                                                //price = Select Options



                                                            }
                                                            else
                                                            {
                                                                Decimal shademarkup = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));

                                                                Decimal Pricepp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 value FROM tb_ShadeDetail WHERE ShadeWidthID in (SELECT ShadeWidthID FROM tb_Shadewidth WHERE ProductId=" + strPIds[j].ToString() + " and value=floor('" + WidthStdAllow.ToString() + "')) and ShadeLengthID in(SELECT ShadeLengthID FROM tb_ShadeLength WHERE ProductId=" + strPIds[j].ToString() + " and value=floor('" + VariValue.ToString() + "')) "));

                                                                Decimal yourprice = Pricepp * shademarkup;
                                                                price = yourprice;
                                                                optiontrue = true;

                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (strVal[k].ToString().IndexOf("/") > -1)
                                                            {
                                                                string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                                strwidth = strwidth.Replace("/", "");
                                                                decimal tt = Convert.ToDecimal(strwidth);
                                                                strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                                decimal VariValue1 = tt;
                                                                VariValue = VariValue + VariValue1;
                                                            }
                                                            else
                                                            {
                                                                decimal VariValue1 = 0;
                                                                decimal.TryParse(strVal[k].ToString(), out VariValue1);
                                                                VariValue = VariValue + VariValue1;

                                                            }
                                                        }
                                                    }
                                                }
                                                if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "" && Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"].ToString() == "2")
                                                {
                                                    avail = Convert.ToString(ViewState["AvailableDate"]);
                                                    try
                                                    {
                                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                                if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"] == "1")
                                                {
                                                    if (avail == "")
                                                    {
                                                        avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + strPIds[j].ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + Qty + ""));
                                                        if (avail == "")
                                                        {
                                                            avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + strPIds[j].ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + Qty + ""));
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                    }
                                }
                                VariantValueIdtemp = VariantValueId;
                                if (LengthStdAllow > 0)
                                {
                                    Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(LengthStdAllow)));
                                    actualYard = Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(LengthStdAllow.ToString())));
                                    //// VariantNameId = VariantNameId + "Yardage Required,";
                                    //// VariantValueId = VariantValueId + LengthStdAllow.ToString() + ",";
                                    Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
                                                " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + Convert.ToInt32(Session["CustID"]) + " Order By ShoppingCartID) " +
                                                " AND ProductID=" + Convert.ToInt32(strPIds[j].ToString()) + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND replace(VariantValues,cast((Quantity * " + LengthStdAllow + ") as nvarchar(100)),'" + LengthStdAllow + "')='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'"));


                                    DataSet dsvariant = new DataSet();
                                    dsvariant = CommonComponent.GetCommonDataSet("SELECT VariantValues FROM tb_ShoppingCartItems  WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Convert.ToInt32(Session["CustID"].ToString()) + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND replace(VariantValues,cast((Quantity * " + LengthStdAllow + ") as nvarchar(100)),'" + LengthStdAllow + "')='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    if (dsvariant != null && dsvariant.Tables.Count > 0 && dsvariant.Tables[0].Rows.Count > 0)
                                    {

                                        VariantValueId = Convert.ToString(dsvariant.Tables[0].Rows[0]["VariantValues"].ToString());
                                    }
                                    Int32 OrderQty = Qty + ShoppingCartQty;
                                    Yardqty = Yardqty * OrderQty;
                                    LengthStdAllow = LengthStdAllow * Convert.ToDecimal(OrderQty);
                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + strPIds[j].ToString() + ",'" + Request.QueryString["VariantNames"].ToString().Replace("~hpd~", "-") + "','" + Request.QueryString["VariantValues"].ToString().Replace("~hpd~", "-") + "'"));

                                    if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                    {
                                        if (isDropshipProduct == false)
                                        {

                                            Response.Clear();
                                            string StrRetValue = "";
                                            StrRetValue += "<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                                            StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                                            StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                                            StrRetValue += "<div class=\"description_box_border\">";
                                            StrRetValue += "<br>";
                                            StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                                            StrRetValue += "<tbody>";
                                            //StrRetValue += "<tr>";
                                            //StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                                            //StrRetValue += "<div>Available in our Warehouse : <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span></div>";
                                            //StrRetValue += "</td>";
                                            //StrRetValue += "</tr>";
                                            //StrRetValue += "<tr>";
                                            //StrRetValue += "<td valign=\"middle\" align=\"center\">";
                                            //StrRetValue += "<div>Call us at <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> if you are looking for more than <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span> Quantity(s)</div>";
                                            //StrRetValue += "</td>";
                                            //StrRetValue += "</tr>";

                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                                            StrRetValue += "<div>This product is out of stock.</div>";
                                            StrRetValue += "</td>";
                                            StrRetValue += "</tr>";
                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\">";
                                            StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                                            StrRetValue += "</td>";
                                            StrRetValue += "</tr>";

                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                                            StrRetValue += "</tr>";
                                            StrRetValue += "</tbody></table>";
                                            StrRetValue += "</div>";
                                            StrRetValue += "</div>";

                                            Response.Write("<div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                                            Response.End();
                                            return;
                                        }
                                        ViewState["AvailableDate"] = StrVendor;

                                    }
                                    else
                                    {
                                        ViewState["AvailableDate"] = StrVendor;
                                    }



                                    //Yardqty = LengthStdAllow * 
                                    //VariantNameId = VariantNameId + "Yardage Required,";
                                    //// VariantValueIdtemp = VariantValueIdtemp + LengthStdAllow.ToString() + ",";
                                    if (ShoppingCartQty == 0)
                                    {
                                        VariantValueId = VariantValueIdtemp;
                                    }

                                }
                                else
                                {
                                    if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"] == "3")
                                    {
                                        string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Qty + "," + strPIds[j].ToString() + ",'" + Request.QueryString["VariantNames"].ToString().Replace("~hpd~", "-") + "','" + Request.QueryString["VariantValues"].ToString().Replace("~hpd~", "-") + "'"));
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                }
                                if (ViewState["AvailableDate"] != null)
                                {
                                    try
                                    {
                                        avail = Convert.ToString(ViewState["AvailableDate"]);
                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                    }
                                    catch { }
                                }
                                Int32 IsSwatchproduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[j].ToString() + " and ItemType='Swatch'"));


                                if (avail != "" && IsSwatchproduct != 1)
                                {
                                    VariantNameId = VariantNameId + "Estimated Delivery,";
                                    VariantValueId = VariantValueId + avail.ToString() + ",";
                                }
                                if (!string.IsNullOrEmpty(strDatenew))
                                {
                                    VariantNameId = VariantNameId + "Back Order,";
                                    VariantValueId = VariantValueId + "Yes,";
                                }

                                string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strPIds[j].ToString()), Qty, price, "", "", VariantNameId, VariantValueId, 0);

                                string strSKU = Convert.ToString(CommonComponent.GetScalarCommonData("select RelatedProduct from tb_Product where productid=" + Request.QueryString["ProdID"].ToString() + " and StoreId=-1"));
                                DataSet ds = new DataSet();
                                if (Request.QueryString["ProductType"] != null)
                                {
                                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[j].ToString() + " and ItemType='Swatch'"));
                                    if (Isorderswatch == 1)
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=0 WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                    else if (Request.QueryString["ProductType"].ToString() == "3")
                                    {
                                        //CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', VariantValues='" + VariantValueIdtemp.ToString().Replace("'", "''").Replace("~hpd~", "-") + "', IsProductType=" + Request.QueryString["ProductType"].ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");

                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "',  IsProductType=" + Request.QueryString["ProductType"].ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                    else
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=" + Request.QueryString["ProductType"].ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                }
                                else
                                {

                                    CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=0 WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");

                                }

                                ds = CommonComponent.GetCommonDataSet("select SKU,ProductId from tb_Product where isnull(active,0)=1 AND  isnull(deleted,0)=0 AND Storeid=" + AppLogic.AppConfigs("Storeid").ToString() + " and isnull(sku,'') <>'' and  SKU in (select items from dbo.Split ('" + strSKU.ToString() + "',','))");
                                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                    {
                                        string strResult1 = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(ds.Tables[0].Rows[k]["ProductId"].ToString()), Qty, 0, "", "", "", "", Convert.ToInt32(strPIds[j].ToString()));

                                    }
                                }

                                check = true;
                            }
                        }
                        if (check)
                        {
                            clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(Session["CustID"]));
                            Response.Clear();
                            Response.Write(objMiniCart.GetMiniCart());
                            Response.End();
                        }

                    }

                }
            }
            else
            {
                Response.Clear();
                Response.Write("<div style='color:cc0000;display:none;'>Customer is restricted.</div>");
                Response.End();
            }
        }
        private void AddToCartMultiSwatch()
        {

            string strsku = ",";
            string strskulist = ",";
            int outofstockpid = 0;
            ViewState["AvailableDate"] = null;
            bool IsRestricted = true;
            bool isDropshipProduct = false;
            Int32 Isorderswatch1 = 0;
            IsRestricted = CheckCustomerIsRestricted();
            if (!IsRestricted)
            {
                if (Session["CustID"] == null || Session["CustID"].ToString() == "")
                {
                    AddCustomer();
                }

                string VariantValueId = "";
                string VariantNameId = "";
                string VariantQty = "";
                bool check = false;
                string[] strPIds = { "" };
                string[] strPrices = { "" };
                string[] strQuantitys = { "" };
                Yardqty = 0;
                string VariantValueIdtemp = "";
                actualYard = 0;

                if (Request.QueryString["ProdID"] != null && Request.QueryString["Price"] != null && Request.QueryString["Quantity"] != null)
                {
                    strPIds = Request.QueryString["ProdID"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    strPrices = Request.QueryString["Price"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    strQuantitys = Request.QueryString["Quantity"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                }
                else
                {
                    return;
                }



                for (int k = 0; k < strPIds.Length; k++)
                {
                    if (Request.QueryString["ProductType"] == null)
                    {
                        String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustId"].ToString() + " Order By ShoppingCartID DESC) "));
                        //if (!string.IsNullOrEmpty(strswatchQtyy) && (Convert.ToInt32(strswatchQtyy) + Convert.ToInt32(strQuantitys[k].ToString())) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()))
                        //{
                        strPrices[k] = "0.00";
                        //}
                        //else
                        //{
                        //strPrices[k] = "0.00";
                        //}
                    }
                    else
                    {
                        Isorderswatch1 = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[k].ToString() + " and ItemType='Swatch'"));
                        if (Isorderswatch1 == 1)
                        {
                            strPrices[k] = "0.00";
                        }
                    }
                    isDropshipProduct = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(IsdropshipProduct,0) FROM tb_product WHERE productid=" + Convert.ToInt32(strPIds[k].ToString()).ToString() + ""));



                    if (CheckInventory(Convert.ToInt32(strPIds[k].ToString()), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strQuantitys[k].ToString())))
                    {

                    }
                    else
                    {
                        if (isDropshipProduct == false)
                        {

                            strsku = strsku + strPIds[k].ToString() + ",";
                            string s = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(sku,'') from tb_product where productid=" + strPIds[k].ToString() + ""));
                            strskulist = strskulist + s + ",";
                            outofstock = true;
                            outofstockpid = outofstockpid + 1;
                        }
                    }

                }
                if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"] == "1" && ViewState["AvailableDate"] == null) // Made to Measure
                {
                    DateTime dtnew = DateTime.Now.Date.AddDays(12);
                    ViewState["AvailableDate"] = Convert.ToString(dtnew);
                    //for (int k = 0; k < strPIds.Length; k++)
                    //{
                    //    Int32 OrderQty = Convert.ToInt32(strQuantitys[k].ToString());
                    //    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + strPIds[k].ToString() + " "));
                    //    if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                    //    {
                    //        ViewState["AvailableDate"] = StrVendor;
                    //    }
                    //    else
                    //    {
                    //        ViewState["AvailableDate"] = StrVendor;
                    //    }
                    //}
                }

                if (Request.QueryString["Quantity"].ToString().Trim() != "" && strPIds.Length >= outofstockpid)
                {
                    decimal LengthStdAllow = 0;
                    for (int j = 0; j < strPIds.Length; j++)
                    {
                        if (!string.IsNullOrEmpty(strPIds[j].ToString()) && Session["CustID"] != null && strsku.ToString().IndexOf("," + strPIds[j].ToString() + ",") <= -1)
                        {
                            VariantNameId = "";
                            VariantValueId = "";
                            ShoppingCartComponent objShopping = new ShoppingCartComponent();
                            decimal price = Convert.ToDecimal(strPrices[j].ToString());
                            string avail = "";
                            decimal fbyardcost = 0;
                            int Qty = Convert.ToInt32(strQuantitys[j].ToString());
                            //if (j == 0)
                            {
                                if (Request.QueryString["VariantNames"] != null && !string.IsNullOrEmpty(Request.QueryString["VariantNames"].ToString()))
                                {
                                    string[] strNm = Request.QueryString["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    string[] strVal = Request.QueryString["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                    if (strNm.Length > 0)
                                    {
                                        if (strVal.Length == strNm.Length)
                                        {

                                            int RomanShadeId = 0; //Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(RomanShadeId,0) as RomanShadeId from tb_product where ProductId=" + Convert.ToInt32(strPIds[j].ToString()) + " and StoreId=" + AppConfig.StoreID + ""));
                                            for (int pp = 0; pp < strNm.Length; pp++)
                                            {
                                                if (strNm[pp].ToString().ToLower().IndexOf("roman shade design") > -1)
                                                {
                                                    RomanShadeId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select TOp 1 ISNULL(RomanShadeId,0) as RomanShadeId from tb_ProductRomanShadeYardage where ShadeName='" + strVal[pp].ToString().Trim() + "' AND isnull(Active,0)=1"));
                                                    //break;
                                                }
                                                if (strNm[pp].ToString().ToLower().IndexOf("color") > -1)
                                                {
                                                    fbyardcost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT isnull(VariantPrice,0) FROM tb_ProductVariantValue WHERE VariantValue='" + strVal[pp].ToString().Trim().Replace("'", "''") + "' and ProductId=" + Convert.ToInt32(strPIds[j].ToString()) + ""));
                                                }


                                            }

                                            decimal VariValue = 0, WidthStdAllow = 0;
                                            bool optiontrue = false;
                                            for (int k = 0; k < strNm.Length; k++)
                                            {
                                                if (VariantNameId.ToString().ToLower().IndexOf(strNm[k].ToString().ToLower()) > -1)
                                                {
                                                    string[] strNmtemp = VariantNameId.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                    string[] strValtemp = VariantValueId.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                                    VariantNameId = "";
                                                    VariantValueId = "";
                                                    for (int p = 0; p < strNmtemp.Length; p++)
                                                    {
                                                        if (strNmtemp[p].ToString().ToLower().IndexOf(strNm[k].ToString().ToLower()) > -1)
                                                        {
                                                            VariantNameId += strNmtemp[p].ToString() + ",";
                                                            VariantValueId += strValtemp[p].ToString() + " " + strVal[k].ToString() + ",";
                                                        }
                                                        else
                                                        {
                                                            VariantNameId += strNmtemp[p].ToString() + ",";
                                                            VariantValueId += strValtemp[p].ToString() + ",";
                                                        }
                                                    }
                                                }
                                                else
                                                {
                                                    VariantNameId = VariantNameId + strNm[k].ToString() + ",";
                                                    VariantValueId = VariantValueId + strVal[k].ToString() + ",";
                                                }


                                                // Roman Yardage ---------------------------------

                                                if (RomanShadeId > 0 || Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"] == "3")
                                                {
                                                    if (strNm[k].ToString().ToLower().Trim().IndexOf("width") > -1)
                                                    {
                                                        if (strVal[k].ToString().IndexOf("/") > -1)
                                                        {
                                                            string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                            strwidth = strwidth.Replace("/", "");
                                                            decimal tt = Convert.ToDecimal(strwidth);
                                                            strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                            tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                            decimal WidthStdAllow1 = tt;
                                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;
                                                        }
                                                        else
                                                        {
                                                            decimal WidthStdAllow1 = 0;
                                                            decimal.TryParse(strVal[k].ToString(), out WidthStdAllow1);
                                                            WidthStdAllow = WidthStdAllow + WidthStdAllow1;

                                                        }
                                                        //WidthStdAllow = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(Sum(ISNULL(WidthStandardAllowance,0)+" + VariValue + ")/54,0) as WidthStdAllow from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1"));
                                                    }
                                                    if (strNm[k].ToString().ToLower().Trim().IndexOf("length") > -1)
                                                    {
                                                        if (VariValue > Convert.ToDecimal(1))
                                                        {
                                                            if (strVal[k].ToString().IndexOf("/") > -1)
                                                            {
                                                                string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                                strwidth = strwidth.Replace("/", "");
                                                                decimal tt = Convert.ToDecimal(strwidth);
                                                                strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                                decimal VariValue1 = tt;
                                                                VariValue = VariValue + VariValue1;
                                                            }
                                                            else
                                                            {
                                                                decimal VariValue1 = 0;
                                                                decimal.TryParse(strVal[k].ToString(), out VariValue1);
                                                                VariValue = VariValue + VariValue1;

                                                            }

                                                            //decimal.TryParse(strVal[k].ToString(), out VariValue);
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
                                                                LengthStdAllow = Math.Round(LengthStdAllow, 2);

                                                                for (int p = 0; p < strNm.Length; p++)
                                                                {
                                                                    if (strNm[p].ToString().ToLower().Trim().IndexOf("option") > -1)
                                                                    {
                                                                        optiontrue = true;
                                                                        DataSet dsRoman = new DataSet();
                                                                        dsRoman = CommonComponent.GetCommonDataSet("Select isnull(FabricPerYardCost,0) as FabricPerYardCost,isnull(ManufuacturingCost,0) as ManufuacturingCost from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1");

                                                                        if (dsRoman != null && dsRoman.Tables.Count > 0 && dsRoman.Tables[0].Rows.Count > 0)
                                                                        {
                                                                            decimal yardprice = Convert.ToDecimal(fbyardcost) * Convert.ToDecimal(LengthStdAllow);
                                                                            yardprice = yardprice + Convert.ToDecimal(dsRoman.Tables[0].Rows[0]["ManufuacturingCost"]);
                                                                            decimal rangecost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Cost,0) as Cost from tb_Roman_Shipping where Fromwidth <= " + WidthStdAllow + " And Towidth >=" + WidthStdAllow + " and ISNULL(Active,0)=1"));
                                                                            yardprice = yardprice + rangecost;
                                                                            decimal ProductOptionsPrice = 0;
                                                                            decimal Duties = 0;
                                                                            if (strVal[p].ToString().ToLower() == "lined")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else if (strVal[p].ToString().ToLower() == "lined & interlined" || strVal[p].ToString().ToLower() == "lined &amp; interlined")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  (Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' Or Options='" + strVal[p].ToString().ToLower().Replace("'", "''").Replace("&amp;", "&") + "') and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else if (strVal[p].ToString().ToLower() == "blackout lining")
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            else
                                                                            {
                                                                                ProductOptionsPrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0)  from tb_ProductOptionsPrice where  Options='" + strVal[p].ToString().ToLower().Replace("'", "''") + "' and ProductId=" + strPIds[j].ToString() + ""));
                                                                            }
                                                                            Duties = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Duties,0) as Duties from tb_ProductRomanShadeYardage where  RomanShadeId=" + RomanShadeId + ""));
                                                                            //
                                                                            if (Duties > decimal.Zero)
                                                                            {
                                                                                //ProductOptionsPrice = ProductOptionsPrice / Convert.ToDecimal(100);
                                                                                //decimal liningoption = (yardprice * ProductOptionsPrice) + yardprice;

                                                                                ////ProductOptionsPrice = Convert.ToDecimal(15) / Convert.ToDecimal(100);

                                                                                ////liningoption = liningoption * ProductOptionsPrice;
                                                                                ////liningoption = liningoption + yardprice;
                                                                                //price = liningoption;

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
                                                                        }
                                                                        break;
                                                                    }

                                                                }
                                                                if (optiontrue == false && VariValue == Decimal.Zero && WidthStdAllow == Decimal.Zero)
                                                                {
                                                                    DataSet dsRoman = new DataSet();
                                                                    dsRoman = CommonComponent.GetCommonDataSet("Select isnull(FabricPerYardCost,0) as FabricPerYardCost,isnull(ManufuacturingCost,0) as ManufuacturingCost from tb_ProductRomanShadeYardage where RomanShadeId=" + RomanShadeId + " and ISNULL(Active,0)=1");

                                                                    if (dsRoman != null && dsRoman.Tables.Count > 0 && dsRoman.Tables[0].Rows.Count > 0)
                                                                    {

                                                                        decimal yardprice = Convert.ToDecimal(fbyardcost) * Convert.ToDecimal(LengthStdAllow);
                                                                        yardprice = yardprice + Convert.ToDecimal(dsRoman.Tables[0].Rows[0]["ManufuacturingCost"]);
                                                                        decimal rangecost = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(Cost,0) as Cost from tb_Roman_Shipping where Fromwidth <= " + WidthStdAllow + " And Towidth >=" + WidthStdAllow + " and ISNULL(Active,0)=1"));
                                                                        yardprice = yardprice + rangecost;

                                                                        decimal ProductOptionsPrice = 0;// Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select top 1 isnull(AdditionalPricePercentage,0) as AdditionalPricePercentage from tb_ProductOptionsPrice where  ProductId=" + strPIds[j].ToString() + " and Options='" + strVal[p].ToString() + "'"));

                                                                        decimal liningoption = yardprice;
                                                                        ProductOptionsPrice = Convert.ToDecimal(15) / Convert.ToDecimal(100);
                                                                        liningoption = liningoption * ProductOptionsPrice;
                                                                        liningoption = liningoption + yardprice;
                                                                        price = liningoption;

                                                                    }
                                                                    optiontrue = true;
                                                                }
                                                                else
                                                                {
                                                                    if (optiontrue == false)
                                                                    {


                                                                        Decimal shademarkup = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));

                                                                        Decimal Pricepp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 value FROM tb_ShadeDetail WHERE ShadeWidthID in (SELECT ShadeWidthID FROM tb_Shadewidth WHERE ProductId=" + strPIds[j].ToString() + " and value=floor('" + WidthStdAllow.ToString() + "')) and ShadeLengthID in(SELECT ShadeLengthID FROM tb_ShadeLength WHERE ProductId=" + strPIds[j].ToString() + " and value=floor('" + VariValue.ToString() + "')) "));

                                                                        Decimal yourprice = Pricepp * shademarkup;
                                                                        price = yourprice;
                                                                        optiontrue = true;
                                                                    }
                                                                }
                                                                //price = Select Options



                                                            }
                                                            else
                                                            {
                                                                Decimal shademarkup = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));

                                                                Decimal Pricepp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT Top 1 value FROM tb_ShadeDetail WHERE ShadeWidthID in (SELECT ShadeWidthID FROM tb_Shadewidth WHERE ProductId=" + strPIds[j].ToString() + " and value=floor('" + WidthStdAllow.ToString() + "')) and ShadeLengthID in(SELECT ShadeLengthID FROM tb_ShadeLength WHERE ProductId=" + strPIds[j].ToString() + " and value=floor('" + VariValue.ToString() + "')) "));

                                                                Decimal yourprice = Pricepp * shademarkup;
                                                                price = yourprice;
                                                                optiontrue = true;

                                                            }
                                                        }
                                                        else
                                                        {
                                                            if (strVal[k].ToString().IndexOf("/") > -1)
                                                            {
                                                                string strwidth = strVal[k].ToString().Substring(0, strVal[k].ToString().IndexOf("/"));
                                                                strwidth = strwidth.Replace("/", "");
                                                                decimal tt = Convert.ToDecimal(strwidth);
                                                                strwidth = Convert.ToString(strVal[k].ToString()).Replace(strwidth + "/", "");
                                                                tt = Convert.ToDecimal(tt) / Convert.ToDecimal(strwidth);
                                                                decimal VariValue1 = tt;
                                                                VariValue = VariValue + VariValue1;
                                                            }
                                                            else
                                                            {
                                                                decimal VariValue1 = 0;
                                                                decimal.TryParse(strVal[k].ToString(), out VariValue1);
                                                                VariValue = VariValue + VariValue1;

                                                            }
                                                        }
                                                    }
                                                }
                                                if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "" && Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"].ToString() == "2")
                                                {
                                                    avail = Convert.ToString(ViewState["AvailableDate"]);
                                                    try
                                                    {
                                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                                if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"] == "1")
                                                {
                                                    if (avail == "")
                                                    {
                                                        avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + strPIds[j].ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + Qty + ""));
                                                        if (avail == "")
                                                        {
                                                            avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + strPIds[j].ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + Qty + ""));
                                                        }
                                                    }
                                                }


                                            }
                                        }
                                    }
                                }
                                VariantValueIdtemp = VariantValueId;
                                if (LengthStdAllow > 0)
                                {
                                    Yardqty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(LengthStdAllow)));
                                    actualYard = Convert.ToDouble(string.Format("{0:0.00}", Convert.ToDecimal(LengthStdAllow.ToString())));
                                    //// VariantNameId = VariantNameId + "Yardage Required,";
                                    //// VariantValueId = VariantValueId + LengthStdAllow.ToString() + ",";
                                    Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Sum(ISNULL(Quantity,0)),0) FROM tb_ShoppingCartItems " +
                                                " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + Convert.ToInt32(Session["CustID"]) + " Order By ShoppingCartID) " +
                                                " AND ProductID=" + Convert.ToInt32(strPIds[j].ToString()) + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND replace(VariantValues,cast((Quantity * " + LengthStdAllow + ") as nvarchar(100)),'" + LengthStdAllow + "')='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'"));


                                    DataSet dsvariant = new DataSet();
                                    dsvariant = CommonComponent.GetCommonDataSet("SELECT VariantValues FROM tb_ShoppingCartItems  WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Convert.ToInt32(Session["CustID"].ToString()) + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND replace(VariantValues,cast((Quantity * " + LengthStdAllow + ") as nvarchar(100)),'" + LengthStdAllow + "')='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    if (dsvariant != null && dsvariant.Tables.Count > 0 && dsvariant.Tables[0].Rows.Count > 0)
                                    {

                                        VariantValueId = Convert.ToString(dsvariant.Tables[0].Rows[0]["VariantValues"].ToString());
                                    }
                                    Int32 OrderQty = Qty + ShoppingCartQty;
                                    Yardqty = Yardqty * OrderQty;
                                    LengthStdAllow = LengthStdAllow * Convert.ToDecimal(OrderQty);
                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + strPIds[j].ToString() + ",'" + Request.QueryString["VariantNames"].ToString().Replace("~hpd~", "-") + "','" + Request.QueryString["VariantValues"].ToString().Replace("~hpd~", "-") + "'"));

                                    if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor == "" || StrVendor == "n/a"))
                                    {
                                        if (isDropshipProduct == false)
                                        {

                                            Response.Clear();
                                            string StrRetValue = "";
                                            StrRetValue += "<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                                            StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                                            StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                                            StrRetValue += "<div class=\"description_box_border\">";
                                            StrRetValue += "<br>";
                                            StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                                            StrRetValue += "<tbody>";
                                            //StrRetValue += "<tr>";
                                            //StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                                            //StrRetValue += "<div>Available in our Warehouse : <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span></div>";
                                            //StrRetValue += "</td>";
                                            //StrRetValue += "</tr>";
                                            //StrRetValue += "<tr>";
                                            //StrRetValue += "<td valign=\"middle\" align=\"center\">";
                                            //StrRetValue += "<div>Call us at <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> if you are looking for more than <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span> Quantity(s)</div>";
                                            //StrRetValue += "</td>";
                                            //StrRetValue += "</tr>";

                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                                            StrRetValue += "<div>This product is out of stock.</div>";
                                            StrRetValue += "</td>";
                                            StrRetValue += "</tr>";
                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\">";
                                            StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                                            StrRetValue += "</td>";
                                            StrRetValue += "</tr>";

                                            StrRetValue += "<tr>";
                                            StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                                            StrRetValue += "</tr>";
                                            StrRetValue += "</tbody></table>";
                                            StrRetValue += "</div>";
                                            StrRetValue += "</div>";

                                            Response.Write("<div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                                            Response.End();
                                            return;
                                        }
                                        ViewState["AvailableDate"] = StrVendor;

                                    }
                                    else
                                    {
                                        ViewState["AvailableDate"] = StrVendor;
                                    }



                                    //Yardqty = LengthStdAllow * 
                                    //VariantNameId = VariantNameId + "Yardage Required,";
                                    //// VariantValueIdtemp = VariantValueIdtemp + LengthStdAllow.ToString() + ",";
                                    if (ShoppingCartQty == 0)
                                    {
                                        VariantValueId = VariantValueIdtemp;
                                    }

                                }
                                else
                                {
                                    if (Request.QueryString["ProductType"] != null && Request.QueryString["ProductType"] == "3")
                                    {
                                        string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Qty + "," + strPIds[j].ToString() + ",'" + Request.QueryString["VariantNames"].ToString().Replace("~hpd~", "-") + "','" + Request.QueryString["VariantValues"].ToString().Replace("~hpd~", "-") + "'"));
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                }
                                if (ViewState["AvailableDate"] != null)
                                {
                                    try
                                    {
                                        avail = Convert.ToString(ViewState["AvailableDate"]);
                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                    }
                                    catch { }
                                }
                                Int32 IsSwatchproduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[j].ToString() + " and ItemType='Swatch'"));


                                if (avail != "" && IsSwatchproduct != 1)
                                {
                                    VariantNameId = VariantNameId + "Estimated Delivery,";
                                    VariantValueId = VariantValueId + avail.ToString() + ",";
                                }
                                if (!string.IsNullOrEmpty(strDatenew))
                                {
                                    VariantNameId = VariantNameId + "Back Order,";
                                    VariantValueId = VariantValueId + "Yes,";
                                }

                                string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strPIds[j].ToString()), Qty, price, "", "", VariantNameId, VariantValueId, 0);

                                string strSKU = Convert.ToString(CommonComponent.GetScalarCommonData("select RelatedProduct from tb_Product where productid=" + Request.QueryString["ProdID"].ToString() + " and StoreId=-1"));
                                DataSet ds = new DataSet();
                                if (Request.QueryString["ProductType"] != null)
                                {
                                    Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + strPIds[j].ToString() + " and ItemType='Swatch'"));
                                    if (Isorderswatch == 1)
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=0 WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                    else if (Request.QueryString["ProductType"].ToString() == "3")
                                    {
                                        //CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', VariantValues='" + VariantValueIdtemp.ToString().Replace("'", "''").Replace("~hpd~", "-") + "', IsProductType=" + Request.QueryString["ProductType"].ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");

                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "',  IsProductType=" + Request.QueryString["ProductType"].ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                    else
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=" + Request.QueryString["ProductType"].ToString() + " WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");
                                    }
                                }
                                else
                                {

                                    CommonComponent.ExecuteCommonData("UPDATE tb_ShoppingCartItems SET YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "', IsProductType=0 WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM  tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " order By ShoppingCartID DESC) AND ProductID=" + strPIds[j].ToString() + " AND VariantNames='" + VariantNameId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND VariantValues='" + VariantValueId.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'");

                                }

                                ds = CommonComponent.GetCommonDataSet("select SKU,ProductId from tb_Product where isnull(active,0)=1 AND  isnull(deleted,0)=0 AND Storeid=" + AppLogic.AppConfigs("Storeid").ToString() + " and isnull(sku,'') <>'' and  SKU in (select items from dbo.Split ('" + strSKU.ToString() + "',','))");
                                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < ds.Tables[0].Rows.Count; k++)
                                    {
                                        string strResult1 = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(ds.Tables[0].Rows[k]["ProductId"].ToString()), Qty, 0, "", "", "", "", Convert.ToInt32(strPIds[j].ToString()));

                                    }
                                }

                                check = true;
                            }
                        }




                        if (check && outofstockpid == 0 && j == strPIds.Length - 1)
                        {
                            clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(Session["CustID"]));
                            Response.Clear();
                            Response.Write(objMiniCart.GetMiniCart());
                            Response.End();
                        }

                    }

                    if (outofstockpid > 0 && outofstockpid != strPIds.Length)
                    {
                        clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(Session["CustID"]));
                        Response.Clear();
                        string StrRetValue = "";
                        StrRetValue += "###inv###<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                        StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                        StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                        StrRetValue += "<div class=\"description_box_border\">";
                        StrRetValue += "<br>";
                        StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                        StrRetValue += "<tbody>";
                        StrRetValue += "<tr>";
                        StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                        if (strskulist.Length > 2)
                        {
                            strskulist = strskulist.Substring(1, strskulist.Length - 1);
                        }
                        StrRetValue += "<div>" + strskulist + " This products are out of stock.</div>";
                        StrRetValue += "</td>";
                        StrRetValue += "</tr>";
                        StrRetValue += "<tr>";
                        StrRetValue += "<td valign=\"middle\" align=\"center\">";
                        StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                        StrRetValue += "</td>";
                        StrRetValue += "</tr>";
                        StrRetValue += "<tr>";
                        StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                        StrRetValue += "</tr>";
                        StrRetValue += "</tbody></table>";
                        StrRetValue += "</div>";
                        StrRetValue += "</div></div>###inv1###";
                        if (strPIds.Length != outofstockpid)
                        {

                            Response.Write(StrRetValue + "###cart###" + objMiniCart.GetMiniCart() + "###cart1###<div style='color:cc0000;display:none;'>shraddha</div><div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>");
                        }
                        else
                        {
                            Response.Write(StrRetValue + "###cart###" + objMiniCart.GetMiniCart() + "###cart1###<<div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>");
                        }

                        Response.End();
                    }
                    else if (outofstockpid > 0)
                    {
                        Response.Clear();
                        string StrRetValue = "";
                        StrRetValue += "<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                        StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                        StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                        StrRetValue += "<div class=\"description_box_border\">";
                        StrRetValue += "<br>";
                        StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                        StrRetValue += "<tbody>";
                        StrRetValue += "<tr>";
                        StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                        if (strskulist.Length > 2)
                        {
                            strskulist = strskulist.Substring(1, strskulist.Length - 1);
                        }
                        StrRetValue += "<div>" + strskulist + " This products are out of stock.</div>";
                        StrRetValue += "</td>";
                        StrRetValue += "</tr>";
                        StrRetValue += "<tr>";
                        StrRetValue += "<td valign=\"middle\" align=\"center\">";
                        StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                        StrRetValue += "</td>";
                        StrRetValue += "</tr>";
                        StrRetValue += "<tr>";
                        StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                        StrRetValue += "</tr>";
                        StrRetValue += "</tbody></table>";
                        StrRetValue += "</div>";
                        StrRetValue += "</div>";
                        if (strPIds.Length != outofstockpid)
                        {

                            Response.Write("<div style='color:cc0000;display:none;'>shraddha</div><div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                        }
                        else
                        {
                            Response.Write("<div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                        }

                        Response.End();
                    }

                }
            }
            else
            {
                Response.Clear();
                Response.Write("<div style='color:cc0000;display:none;'>Customer is restricted.</div>");
                Response.End();
            }

        }

        /// <summary>
        /// Adds Multiple Selected Product into the Cart
        /// </summary>
        private void AddTocartMulti()
        {
            bool IsRestricted = true;
            IsRestricted = CheckCustomerIsRestricted();
            if (!IsRestricted)
            {
                if (Session["CustID"] == null || Session["CustID"].ToString() == "")
                {
                    AddCustomer();
                }

                string VariantValueId = "";
                string VariantNameId = "";
                string VariantQty = "";
                bool check = false;
                string[] strPIds = { "" };
                string[] strPrices = { "" };
                string[] strQuantitys = { "" };
                if (Request.QueryString["ProdID"] != null && Request.QueryString["Price"] != null && Request.QueryString["Quantity"] != null)
                {
                    strPIds = Request.QueryString["ProdID"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    strPrices = Request.QueryString["Price"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    strQuantitys = Request.QueryString["Quantity"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                }
                else
                {
                    return;
                }


                if (Request.QueryString["Quantity"].ToString().Trim() != "")
                {

                    for (int j = 0; j < strPIds.Length; j++)
                    {
                        if (CheckInventory(Convert.ToInt32(strPIds[j].ToString()), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strQuantitys[j].ToString())))
                        {
                            if (!string.IsNullOrEmpty(strPIds[j].ToString()) && Session["CustID"] != null)
                            {
                                VariantNameId = "";
                                VariantValueId = "";
                                ShoppingCartComponent objShopping = new ShoppingCartComponent();
                                decimal price = Convert.ToDecimal(strPrices[j].ToString());
                                int Qty = Convert.ToInt32(strQuantitys[j].ToString());


                                string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strPIds[j].ToString()), Qty, price, "", "", "", "", 0);
                                check = true;
                            }
                        }
                        else
                        {
                            outofstock = true;
                        }

                    }
                    if (check)
                    {
                        clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(Session["CustID"]));
                        Response.Clear();
                        Response.Write(objMiniCart.GetMiniCart());
                        Response.End();
                    }
                    else
                    {
                        if (outofstock)
                        {
                            string StrRetValue = "";
                            StrRetValue += "<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                            StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                            StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                            StrRetValue += "<div class=\"description_box_border\">";
                            StrRetValue += "<br>";
                            StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                            StrRetValue += "<tbody>";
                            //StrRetValue += "<tr>";
                            //StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                            //StrRetValue += "<div>Available in our Warehouse : <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span></div>";
                            //StrRetValue += "</td>";
                            //StrRetValue += "</tr>";
                            //StrRetValue += "<tr>";
                            //StrRetValue += "<td valign=\"middle\" align=\"center\">";
                            //StrRetValue += "<div>Call us at <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> if you are looking for more than <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span> Quantity(s)</div>";
                            //StrRetValue += "</td>";
                            //StrRetValue += "</tr>";

                            StrRetValue += "<tr>";
                            StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                            StrRetValue += "<div>This product is out of stock.</div>";
                            StrRetValue += "</td>";
                            StrRetValue += "</tr>";
                            StrRetValue += "<tr>";
                            StrRetValue += "<td valign=\"middle\" align=\"center\">";
                            StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                            StrRetValue += "</td>";
                            StrRetValue += "</tr>";

                            StrRetValue += "<tr>";
                            StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                            StrRetValue += "</tr>";
                            StrRetValue += "</tbody></table>";
                            StrRetValue += "</div>";
                            StrRetValue += "</div>";

                            Response.Clear();
                            Response.Write("<div style='color:cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                            Response.End();

                        }
                    }

                }

            }
            else
            {
                Response.Clear();
                Response.Write("<div style='color:cc0000;display:none;'>Customer is restricted.</div>");
                Response.End();
            }

        }

        /// <summary>
        /// Updates the Cart
        /// </summary>
        /// <param name="ProdID">int ProdID</param>
        /// <param name="CustID">int CustID</param>
        /// <param name="Quantity">int Quantity</param>
        /// <param name="Price">decimal Price</param>
        /// <param name="strVname">string strVname</param>
        /// <param name="strVval">string strVval</param>
        private void UpdateCart(Int32 ProdID, Int32 CustID, Int32 Quantity, decimal Price, string strVname, string strVval)
        {
            bool IsRestricted = true;
            ViewState["AvailableDate"] = null;
            IsRestricted = CheckCustomerIsRestricted();
            bool isDropshipProduct = false;

            if (strVval != null && strVval.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
            {
                Quantity = Quantity * 2;
            }
            isDropshipProduct = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(IsdropshipProduct,0) FROM tb_product WHERE productid=" + ProdID.ToString() + ""));
            strDatenew = "";
            if (!IsRestricted)
            {
                if (Session["CustID"] == null || Session["CustID"].ToString() == "")
                {
                    AddCustomer();
                }

                string VariantValueId = strVval;
                string VariantNameId = strVname;
                string VariantQty = "";
                bool check = false;
                bool checkvendordate = false;

                Decimal qtyyrd = Decimal.Zero;
                Int32 OrderQty = 0;
                string StrProductType = "";
                Int32 Strproduct = 0;
                if (Convert.ToInt32(Quantity) > 0)
                {
                    DataSet ds = ProductComponent.GetProductDetailByID(Convert.ToInt32(ProdID), Convert.ToInt32(1));
                    Int32 pInventory = 0;
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        //StrProductType = Convert.ToString(CommonComponent.GetScalarCommonData("SElect case when ISNULL(Ismadetoready,0)=1 then 1 When ISNULL(Ismadetomeasure,0)=1 then 2 when ISNULL(IsRoman,0)=1 then 3 else 0 end from tb_product where Productid=" + ProdID + ""));


                        Strproduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(IsProductType,1) FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC) AND  productId=" + ProdID.ToString() + "  AND VariantNames='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND   VariantValues='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'"));
                        StrProductType = Strproduct.ToString();

                        if (!string.IsNullOrEmpty(StrProductType) && (StrProductType.ToString() == "2")) // Made to Measure
                        {
                            string FabricCode = Convert.ToString(ds.Tables[0].Rows[0]["FabricCode"]);
                            string FabricType = Convert.ToString(ds.Tables[0].Rows[0]["FabricType"]);
                            Int32 FabricTypeID = 0;
                            if (!string.IsNullOrEmpty(FabricCode) && !string.IsNullOrEmpty(FabricType))
                            {
                                FabricTypeID = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(FabricTypeID,0) as FabricTypeID from tb_ProductFabricType where FabricTypename ='" + FabricType + "'"));
                                if (FabricTypeID > 0)
                                {
                                    DataSet dsFabricWidth = CommonComponent.GetCommonDataSet("Select top 1 * from tb_ProductFabricWidth where FabricCodeID in (Select ISNULL(FabricCodeID,0) from tb_ProductFabricCode Where FabricTypeID=" + FabricTypeID + " and Code='" + FabricCode + "')");
                                    Int32 QtyOnHand = 0, NextOrderQty = 0, TotalQty = 0;
                                    OrderQty = Convert.ToInt32(Quantity);



                                    if (dsFabricWidth != null && dsFabricWidth.Tables.Count > 0 && dsFabricWidth.Tables[0].Rows.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"].ToString()))
                                            QtyOnHand = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["QtyOnHand"]);

                                        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"].ToString()))
                                            NextOrderQty = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NextOrderQty"]);
                                    }
                                    pInventory = QtyOnHand + NextOrderQty;
                                    try
                                    {
                                        string Style = "";
                                        double Width = 0;
                                        double Length = 0;
                                        string Options = "";
                                        string[] strNmyard = strVname.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        string[] strValyeard = strVval.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                        if (strNmyard.Length > 0)
                                        {
                                            if (strValyeard.Length == strNmyard.Length)
                                            {
                                                for (int k = 0; k < strNmyard.Length; k++)
                                                {
                                                    if (strNmyard[k].ToString().ToLower() == "width")
                                                    {
                                                        Width = Convert.ToDouble(strValyeard[k].ToString());
                                                    }
                                                    if (strNmyard[k].ToString().ToLower() == "length")
                                                    {
                                                        Length = Convert.ToDouble(strValyeard[k].ToString());
                                                    }
                                                    if (strNmyard[k].ToString().ToLower() == "options")
                                                    {
                                                        Options = Convert.ToString(strValyeard[k].ToString());
                                                    }
                                                    if (strNmyard[k].ToString().ToLower() == "header")
                                                    {
                                                        Style = Convert.ToString(strValyeard[k].ToString());
                                                    }
                                                }
                                            }
                                        }
                                        string resp = "";
                                        if (Width > Convert.ToDouble(0) && Length > Convert.ToDouble(0) && Style != "")
                                        {
                                            DataSet dsYard = new DataSet();
                                            dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProdID + "," + Width.ToString() + "," + Length.ToString() + "," + OrderQty.ToString() + ",'" + Style + "','" + Options + "'");
                                            if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                            {
                                                //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                                                resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                actualYard = Convert.ToDouble(resp.ToString());
                                            }
                                        }
                                        //if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                                        //{
                                        //    //resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
                                        //    resp = string.Format("{0:0.00}", Convert.ToDecimal(ds.Tables[0].Rows[0][1].ToString()));
                                        //}
                                        if (resp != "")
                                        {
                                            OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                        }
                                    }
                                    catch
                                    {
                                    }
                                    Yardqty = OrderQty;

                                    if (Request.QueryString["VariantValues"] != null && Request.QueryString["VariantValues"].ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                                    {
                                        OrderQty = OrderQty * 2;
                                    }

                                    //if (pInventory > 0 && OrderQty > pInventory && isDropshipProduct == false)
                                    //{
                                    //    TotalQuantityValue = pInventory;
                                    //    outofstock = true;
                                    //}
                                    //else
                                    //{
                                    //    if (OrderQty > QtyOnHand)
                                    //    {
                                    //        if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()) && !dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString().ToLower().Contains("1900"))
                                    //        {
                                    //            Int32 TotalDays = 0;
                                    //            if (!string.IsNullOrEmpty(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString()))
                                    //            {
                                    //                TotalDays = Convert.ToInt32(dsFabricWidth.Tables[0].Rows[0]["NoOfDays"].ToString());
                                    //            }
                                    //            DateTime dtnew = Convert.ToDateTime(dsFabricWidth.Tables[0].Rows[0]["AvailableDate"].ToString()).AddDays(TotalDays);
                                    //            ViewState["AvailableDate"] = Convert.ToString(dtnew);


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
                                    //        ViewState["AvailableDate"] = Convert.ToString(dtnew);
                                    //    }
                                    //}

                                    if (pInventory > 0 && OrderQty > pInventory && isDropshipProduct == false)
                                    {
                                        TotalQuantityValue = pInventory;
                                    }

                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProdID + " "));

                                    if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                                    {
                                        if (isDropshipProduct == false)
                                        {
                                            outofstock = true;
                                        }
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                    else
                                    {
                                        checkvendordate = true;
                                        ViewState["AvailableDate"] = StrVendor;
                                    }
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                {
                                    pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                }
                            }
                        }
                        else if (Strproduct == 3)
                        {
                            //qtyyrd = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT (" + Quantity.ToString() + " * Actualyard)  FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC) AND  productId=" + ProdID.ToString() + "  AND VariantNames='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND   VariantValues='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'"));
                            //Yardqty = Convert.ToInt32(Math.Ceiling(qtyyrd));
                            //actualYard = Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDecimal(qtyyrd / Convert.ToDecimal(Quantity.ToString()))));
                            //string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + ProdID + ",'" + strVname + "','" + strVval + "'"));

                            //if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                            //{
                            //    if (isDropshipProduct == false)
                            //    {
                            //        outofstock = true;
                            //    }
                            //    ViewState["AvailableDate"] = StrVendor;
                            //    //checkvendordate = true;
                            //}
                            //else
                            //{
                            //    checkvendordate = true;
                            //    ViewState["AvailableDate"] = StrVendor;
                            //}
                            qtyyrd = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT (" + Quantity.ToString() + " * Actualyard)  FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC) AND  productId=" + ProdID.ToString() + "  AND VariantNames='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND   VariantValues='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "'"));
                            Yardqty = Convert.ToInt32(Math.Ceiling(qtyyrd));
                            actualYard = Convert.ToDouble(String.Format("{0:0.00}", Convert.ToDecimal(qtyyrd / Convert.ToDecimal(Quantity.ToString()))));
                            string StrVendor = "";
                            if (Yardqty == 0)
                            {
                                StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Quantity + "," + ProdID + ",'" + strVname + "','" + strVval + "'"));
                               // ViewState["AvailableDate"] = StrVendor;
                                if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                {
                                    if (isDropshipProduct == false)
                                    {
                                        outofstock = true;
                                    }
                                    ViewState["AvailableDate"] = StrVendor;
                                    //checkvendordate = true;
                                }
                                else
                                {
                                    checkvendordate = true;
                                    ViewState["AvailableDate"] = StrVendor;
                                }
                            }
                            else
                            {
                                StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation_Roman] " + Yardqty + "," + ProdID + ",'" + strVname + "','" + strVval + "'"));
                                if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                {
                                    if (isDropshipProduct == false)
                                    {
                                        outofstock = true;
                                    }
                                    ViewState["AvailableDate"] = StrVendor;
                                    //checkvendordate = true;
                                }
                                else
                                {
                                    checkvendordate = true;
                                    ViewState["AvailableDate"] = StrVendor;
                                }
                            }
                        }
                        else if (Strproduct == 1)
                        {
                            string[] strNmyard = strVname.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] strValyeard = strVval.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                            if (strValyeard.Length > 0)
                            {
                                int warehouseId = 0;
                                if (strValyeard.Length == strNmyard.Length)
                                {
                                    for (int j = 0; j < strNmyard.Length; j++)
                                    {
                                        if (strNmyard[j].ToString().Trim().ToLower().IndexOf("select size") > -1)
                                        {
                                            if (strValyeard[j].ToString().Trim().IndexOf("(+$") > -1 || strValyeard[j].ToString().Trim().IndexOf("($") > -1)
                                            {
                                                string strvalue = strValyeard[j].ToString().Trim().Substring(0, strValyeard[j].ToString().Trim().LastIndexOf("("));
                                                strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProdID + ""));
                                                int CntInv = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProdID + ""));
                                                DataSet dsUPC = new DataSet();
                                                dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU,Variantvalueid FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND ProductId=" + ProdID + "");
                                                string upc = "";
                                                string Skuoption = "";
                                                string Variantvalueid = "";
                                                if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                {
                                                    upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                    Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                    Variantvalueid = Convert.ToString(dsUPC.Tables[0].Rows[0]["VariantValueID"].ToString());
                                                    warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductVariantInventory inner join tb_WareHouse on tb_WareHouseProductVariantInventory.WareHouseID=tb_WareHouse.WareHouseID where VariantValueID=" + Convert.ToInt32(Variantvalueid) + " and tb_WareHouseProductVariantInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                    if (warehouseId == 0)
                                                    {
                                                        warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID in (" + ProdID + " ) and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                    }
                                                    if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                    {
                                                        string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                        if (!string.IsNullOrEmpty(strQty))
                                                        {
                                                            try
                                                            {
                                                                TotalQuantityValue = Convert.ToInt32(strQty.ToString());
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                }


                                                if (TotalQuantityValue <= 0)
                                                {
                                                    TotalQuantityValue = 0;
                                                }

                                                //TotalQuantityValue = Convert.ToInt32(CntInv);
                                                pInventory = TotalQuantityValue;

                                                string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + ProdID.ToString() + ""));
                                                if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) // && TotalQuantityValue < Convert.ToInt32(Quantity)
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
                                                    dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProdID + ",50," + Length.ToString() + "," + Quantity.ToString() + ",'Pole Pocket','Lined'");
                                                    if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                    {
                                                        resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                        actualYard = Convert.ToDouble(resp.ToString());
                                                    }
                                                    OrderQty = Quantity;
                                                    if (resp != "")
                                                    {
                                                        OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));
                                                    }
                                                    Yardqty = OrderQty;
                                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProdID.ToString() + " "));
                                                    pInventory = TotalQuantityValue;
                                                    if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                    {
                                                        ViewState["AvailableDate"] = StrVendor;
                                                        checkvendordate = false;
                                                    }
                                                    else
                                                    {
                                                        ViewState["AvailableDate"] = StrVendor;
                                                        checkvendordate = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (warehouseId == 17)
                                                    {
                                                        pInventory = 9999;
                                                        TotalQuantityValue = 9999;


                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(strDatenew))
                                                {
                                                    ViewState["AvailableDate"] = strDatenew;
                                                    checkvendordate = true;
                                                }
                                            }
                                            else
                                            {
                                                string strvalue = strValyeard[j].ToString().Trim();
                                                strvalue = strvalue.Replace("(Buy 1 Get 1 Free)", "").Replace("(On Sale)", "");
                                                strDatenew = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT convert(char(10),BackOrderdate,101) FROM tb_ProductVariantValue WHERE  cast(BackOrderdate as date) >=cast(GETDATE() as date) and VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProdID + ""));
                                                int CntInv = 0; //Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(AllowQuantity,0) as Inventory FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND  ProductId=" + ProdID + ""));
                                                //TotalQuantityValue = Convert.ToInt32(CntInv);
                                                DataSet dsUPC = new DataSet();
                                                dsUPC = CommonComponent.GetCommonDataSet("SELECT ISNULL(UPC,'') as UPC,SKU,Variantvalueid FROM tb_ProductVariantValue WHERE  VariantValue='" + strvalue.Replace("'", "''") + "' AND ProductId=" + ProdID + "");
                                                string upc = "";
                                                string Skuoption = "";
                                                string Variantvalueid = "";
                                                if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
                                                {
                                                    upc = Convert.ToString(dsUPC.Tables[0].Rows[0]["UPC"].ToString());
                                                    Skuoption = Convert.ToString(dsUPC.Tables[0].Rows[0]["SKU"].ToString());
                                                    Variantvalueid = Convert.ToString(dsUPC.Tables[0].Rows[0]["VariantValueID"].ToString());
                                                    warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductVariantInventory inner join tb_WareHouse on tb_WareHouseProductVariantInventory.WareHouseID=tb_WareHouse.WareHouseID where VariantValueID=" + Convert.ToInt32(Variantvalueid) + " and tb_WareHouseProductVariantInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                    if (warehouseId == 0)
                                                    {
                                                        warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID in (" + ProdID + " ) and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                                    }
                                                    if (!string.IsNullOrEmpty(upc) && !string.IsNullOrEmpty(Skuoption))
                                                    {
                                                        string strQty = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar_TEMP('" + upc + "','" + Skuoption + "',1) "));
                                                        if (!string.IsNullOrEmpty(strQty))
                                                        {
                                                            try
                                                            {
                                                                TotalQuantityValue = Convert.ToInt32(strQty.ToString());
                                                            }
                                                            catch { }
                                                        }
                                                    }
                                                }

                                                pInventory = TotalQuantityValue;
                                                string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + ProdID.ToString() + ""));
                                                if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder)) //  && TotalQuantityValue < Convert.ToInt32(Quantity)
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
                                                    dsYard = CommonComponent.GetCommonDataSet("EXEC usp_Product_Pricecalculator " + ProdID + ",50," + Length.ToString() + "," + Quantity.ToString() + ",'Pole Pocket','Lined'");
                                                    if (dsYard != null && dsYard.Tables.Count > 0 && dsYard.Tables[0].Rows.Count > 0)
                                                    {
                                                        resp = string.Format("{0:0.00}", Convert.ToDecimal(dsYard.Tables[0].Rows[0][1].ToString()));
                                                        actualYard = Convert.ToDouble(resp.ToString());
                                                    }
                                                    OrderQty = Quantity;
                                                    if (resp != "")
                                                    {
                                                        OrderQty = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(resp)));

                                                    }
                                                    Yardqty = OrderQty;
                                                    string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + Quantity + "," + ProdID.ToString() + " "));

                                                    if (!string.IsNullOrEmpty(StrVendor) && (StrVendor == "-" || StrVendor.ToString().ToLower() == "n/a" || StrVendor == ""))
                                                    {
                                                        ViewState["AvailableDate"] = StrVendor;
                                                        checkvendordate = false;
                                                    }
                                                    else
                                                    {
                                                        ViewState["AvailableDate"] = StrVendor;
                                                        checkvendordate = true;
                                                    }
                                                }
                                                else
                                                {
                                                    if (warehouseId == 17)
                                                    {
                                                        TotalQuantityValue = 9999;
                                                        pInventory = 9999;
                                                    }
                                                }
                                                if (!string.IsNullOrEmpty(strDatenew))
                                                {
                                                    ViewState["AvailableDate"] = strDatenew;
                                                    checkvendordate = true;
                                                }
                                            }
                                        }

                                    }
                                }
                                if (strNmyard.Length == 1 && strNmyard[0].ToString().ToLower().IndexOf("estimated") > -1)
                                {
                                    if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                    {
                                        pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                    }
                                    if (ds.Tables[0].Rows[0]["ProductID"].ToString().Trim() != "")
                                    {

                                        warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ds.Tables[0].Rows[0]["ProductID"].ToString() + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                        if (warehouseId == 17)
                                        {
                                            pInventory = 9999;

                                        }
                                    }
                                }
                            }
                            else
                            {
                                if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                                {
                                    pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                }
                                if (ds.Tables[0].Rows[0]["ProductID"].ToString().Trim() != "")
                                {

                                    int warehouseId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_WareHouse.WareHouseID,0) as warehouseid from tb_WareHouseProductInventory inner join tb_WareHouse on tb_WareHouseProductInventory.WareHouseID=tb_WareHouse.WareHouseID where ProductID =" + ds.Tables[0].Rows[0]["ProductID"].ToString() + " and tb_WareHouseProductInventory.WareHouseID=17 and isnull(PreferredLocation,0)=1"));
                                    if (warehouseId == 17)
                                    {
                                        pInventory = 9999;

                                    }
                                }
                            }

                        }
                        else
                        {
                            if (ds.Tables[0].Rows[0]["Inventory"].ToString().Trim() != "")
                            {
                                pInventory = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString());
                                #region swatch-hemming
                                if (Strproduct == 0)
                                {

                                    Int32 IsHemming = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,0) as ConfigValue from tb_AppConfig where ConfigName='IshemmingActive'"));
                                    if (IsHemming == 1)
                                    {
                                        int SwatchHemmInv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(HammingSafetyQty,0) as HammingSafetyQty FROM tb_product WHERE isnull(IsHamming,0)=1 and ProductId=" + ds.Tables[0].Rows[0]["ProductID"].ToString() + ""));
                                        pInventory = pInventory - SwatchHemmInv;
                                    }
                                }

                                #endregion
                            }
                        }
                    }

                    if (Strproduct.ToString() == "1" && ViewState["AvailableDate"] == null) // Made to Measure
                    {
                        try
                        {
                            DateTime dtnew = DateTime.Now.Date.AddDays(12);
                            ViewState["AvailableDate"] = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(dtnew.ToString()));
                        }
                        catch
                        {

                        }

                        //string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + Quantity + "," + ProdID + " "));
                        //if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                        //{
                        //    ViewState["AvailableDate"] = StrVendor;
                        //}
                        //else
                        //{
                        //    ViewState["AvailableDate"] = StrVendor;
                        //}
                    }
                    if (pInventory >= Quantity || isDropshipProduct == true || checkvendordate == true)
                    {

                        if (Session["CustID"] != null)
                        {
                            ShoppingCartComponent objShopping = new ShoppingCartComponent();
                            decimal price = Convert.ToDecimal(Price);


                            int Qty = Convert.ToInt32(Quantity.ToString());
                            Solution.Data.SQLAccess objsql = new Data.SQLAccess();

                            decimal pp = Convert.ToDecimal(objsql.ExecuteScalarQuery("SELECT isnull(Price,0) FROM tb_ShoppingCartItems WHERE ProductID=" + ProdID.ToString() + "  AND isnull(VariantNames,'')='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(VariantValues,'')='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)"));
                            // string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(ProdID), Quantity, pp, "", "", VariantNameId, VariantValueId,0);

                            ///// Put Here Assembly////

                            Int32 AssemblyProduct = 0, ReturnQty = 0;
                            AssemblyProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect COUNT(*) From tb_product Where Productid=" + ProdID + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and ProductTypeID in (Select ProductTypeID From tb_ProductType where Name='Assembly Product')"));
                            if (AssemblyProduct > 0)
                            {
                                ReturnQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_Check_ProductAssemblyInventory " + ProdID + "," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + "," + Session["CustID"].ToString() + ",2"));
                                if (ReturnQty <= 0 && isDropshipProduct == false)
                                {
                                    TotalQuantityValue = ReturnQty;
                                    outofstock = true;
                                    return;
                                }
                                else if (Qty > ReturnQty && isDropshipProduct == false)
                                {
                                    TotalQuantityValue = ReturnQty;
                                    outofstock = true;
                                    return;
                                }
                                else
                                {
                                    // UpdateCart  Query
                                }
                            }


                            if (strVval != null && strVval.ToString().ToLower().IndexOf("(buy 1 get 1 free)") > -1)
                            {
                                Quantity = Quantity / 2;
                            }

                            objsql.ExecuteNonQuery("UPDATE  tb_ShoppingCartItems SET Quantity =" + Quantity + ", YardQuantity=" + Yardqty + ",Actualyard='" + actualYard + "' WHERE ProductID=" + ProdID.ToString() + " AND isnull(VariantNames,'')='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(VariantValues,'')='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)");
                            //objsql.ExecuteNonQuery("UPDATE  tb_ShoppingCartItems SET Quantity =" + Quantity + " WHERE RelatedproductID=" + ProdID.ToString() + " AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)");
                            Int32 RelatedproductID = 0, ParentQty = 0;
                            RelatedproductID = Convert.ToInt32(objsql.ExecuteScalarQuery("SELECT isnull(RelatedproductID,0) FROM tb_ShoppingCartItems WHERE RelatedproductID=" + ProdID.ToString() + "  AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)"));
                            if (RelatedproductID > 0)
                            {
                                ParentQty = Convert.ToInt32(objsql.ExecuteScalarQuery("SELECT isnull(Quantity,0) FROM tb_ShoppingCartItems WHERE ProductID=" + RelatedproductID.ToString() + "  AND isnull(VariantNames,'')='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(VariantValues,'')='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)"));
                                DataSet dsAseeembly = new DataSet();
                                dsAseeembly = objsql.GetDs("SElect ISNULL(Quantity,0),ProductId from tb_ProductAssembly Where RefProductId=" + RelatedproductID.ToString() + "");
                                if (dsAseeembly != null && dsAseeembly.Tables.Count > 0 && dsAseeembly.Tables[0].Rows.Count > 0)
                                {
                                    for (int i = 0; i < dsAseeembly.Tables[0].Rows.Count; i++)
                                    {
                                        Int32 AssemblyQty = Convert.ToInt32(dsAseeembly.Tables[0].Rows[i][0].ToString());
                                        AssemblyQty = (AssemblyQty * ParentQty);

                                        objsql.ExecuteNonQuery("UPDATE  tb_ShoppingCartItems SET Quantity =" + AssemblyQty + " WHERE ProductID=" + dsAseeembly.Tables[0].Rows[i]["ProductId"].ToString() + " AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)");
                                    }

                                }

                            }

                            try
                            {
                                string[] strNm = strVname.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] strVal = strVval.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string avail = "";
                                VariantNameId = "";
                                VariantValueId = "";

                                if (strNm.Length > 0)
                                {
                                    if (strVal.Length == strNm.Length)
                                    {
                                        for (int k = 0; k < strNm.Length; k++)
                                        {
                                            if (Strproduct != 3 && strNm[k].ToString().ToLower().IndexOf("estimated delivery") <= -1)
                                            {
                                                VariantNameId = VariantNameId + strNm[k].ToString() + ",";

                                                VariantValueId = VariantValueId + strVal[k].ToString() + ",";

                                                if (ViewState["AvailableDate"] != null && ViewState["AvailableDate"].ToString() != "" && Strproduct.ToString() == "2")
                                                {
                                                    avail = Convert.ToString(ViewState["AvailableDate"]);
                                                    try
                                                    {
                                                        avail = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(avail.ToString()));
                                                    }
                                                    catch
                                                    {
                                                    }
                                                }
                                                if (avail == "" && Strproduct == 1)
                                                {
                                                    avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 LockQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProdID.ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(LockQuantity,0) >=" + Qty + ""));
                                                    if (avail == "")
                                                    {
                                                        avail = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 AllowQuantityAvail FROM tb_ProductVariantValue WHERE productId=" + ProdID.ToString() + "  AND VariantValue='" + strVal[k].ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(AllowQuantity,0) >=" + Qty + ""));

                                                    }
                                                }
                                            }
                                            else if (Strproduct == 3 && strNm[k].ToString().ToLower().IndexOf("yardage required") <= -1 && strNm[k].ToString().ToLower().IndexOf("estimated delivery") <= -1)
                                            {
                                                VariantNameId = VariantNameId + strNm[k].ToString() + ",";

                                                VariantValueId = VariantValueId + strVal[k].ToString() + ",";


                                            }
                                        }
                                        avail = Convert.ToString(ViewState["AvailableDate"]);
                                        if (Strproduct != 3 && avail != "" && Strproduct != 0)
                                        {
                                            VariantNameId = VariantNameId + "Estimated Delivery,";
                                            VariantValueId = VariantValueId + avail.ToString() + ",";
                                            objsql.ExecuteNonQuery("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE ProductID=" + ProdID.ToString() + " AND isnull(VariantNames,'')='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(VariantValues,'')='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)");
                                        }
                                        else if (Strproduct == 3)
                                        {
                                            ////VariantNameId = VariantNameId + "Yardage Required,";
                                            ////VariantValueId = VariantValueId + qtyyrd.ToString() + ",";
                                            if (avail != "")
                                            {
                                                VariantNameId = VariantNameId + "Estimated Delivery,";
                                                VariantValueId = VariantValueId + avail.ToString() + ",";
                                            }
                                            objsql.ExecuteNonQuery("UPDATE  tb_ShoppingCartItems SET YardQuantity=" + Yardqty.ToString() + ", VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE ProductID=" + ProdID.ToString() + " AND isnull(VariantNames,'')='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(VariantValues,'')='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)");
                                        }
                                        else
                                        {
                                            if (Strproduct != 3)
                                            {
                                                objsql.ExecuteNonQuery("UPDATE  tb_ShoppingCartItems SET VariantNames ='" + VariantNameId.ToString().Replace("'", "''") + "',VariantValues='" + VariantValueId.Replace("'", "''") + "' WHERE ProductID=" + ProdID.ToString() + " AND isnull(VariantNames,'')='" + strVname.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND isnull(VariantValues,'')='" + strVval.ToString().Replace("'", "''").Replace("~hpd~", "-") + "' AND ShoppingCartID in (SELECT TOP 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By ShoppingCartID DESC)");
                                            }
                                        }

                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    else
                    {
                        if (isDropshipProduct == false)
                        {

                            TotalQuantityValue = pInventory;
                            if (StrProductType.ToString() == "2")
                            {
                                string StrVendor = Convert.ToString(CommonComponent.GetScalarCommonData("Exec [usp_VendorPortalQuantityCalculation] " + OrderQty + "," + ProdID + " "));
                                if (!string.IsNullOrEmpty(StrVendor) && StrVendor == "-")
                                {
                                    outofstock = true;
                                }
                            }
                            else
                            {
                                outofstock = true;
                            }
                        }
                    }
                }
            }
            else
            {
                IsRestrictedAll = true;
            }
        }

        /// <summary>
        /// Binds the Mini Cart
        /// </summary>
        public void BindMiniCart()
        {
            if (outofstock == true)
            {
                string StrRetValue = "";
                //StrRetValue += "<div style=\"height: 120px; position: fixed;display: block;width:100%;\" class=\"description_box\">";
                //StrRetValue += "<div style=\"text-align: center;background: none repeat scroll 0 0 #641114;color: #FFFFFF;font-weight: bold;height: 20px;padding: 5px;\" class=\"title\">";
                //StrRetValue += "<span style=\"font-size:16px;\">Stock Info</span>";
                //StrRetValue += "<div class=\"description_box_border\">";
                //StrRetValue += "<br>";
                //StrRetValue += "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\">";
                //StrRetValue += "<tbody>";
                //StrRetValue += "<tr>";
                //StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                //StrRetValue += "<div>Available in our Warehouse : <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span></div>";
                //StrRetValue += "</td>";
                //StrRetValue += "</tr>";
                //StrRetValue += "<tr>";
                //StrRetValue += "<td valign=\"middle\" align=\"center\">";
                //StrRetValue += "<div>Call us at <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> if you are looking for more than <span style=\"color:#641114 !important; font-weight:bold;\">" + TotalQuantityValue.ToString() + "</span> Quantity(s)</div>";
                //StrRetValue += "</td>";
                //StrRetValue += "</tr>";
                //StrRetValue += "<tr>";
                //StrRetValue += "<td valign=\"middle\" align=\"center\">&nbsp;</td>";
                //StrRetValue += "</tr>";
                //StrRetValue += "</tbody></table>";
                //StrRetValue += "</div>";
                //StrRetValue += "</div>";

                StrRetValue += "<div style=\"height: 130px; display: block; width: 100%; background: #fff;\">";
                StrRetValue += "<div style=\"text-align: center; background: none repeat scroll 0 0 #641114; color: #FFFFFF;";
                StrRetValue += "font-weight: bold; height: 20px; padding: 5px;\" class=\"title\">";
                StrRetValue += "<span style=\"font-size: 16px;\">Stock Info</span><div style=\"color: #393939;\">";
                StrRetValue += "<br>";
                StrRetValue += "<table align=\"center\" border=\"0\" cellpadding=\"0\" cellspacing=\"0\" width=\"100%\">";
                StrRetValue += "<tbody>";
                //StrRetValue += "<tr>";
                //StrRetValue += "<td style=\"height: 30px;\" align=\"center\" valign=\"middle\">";
                //StrRetValue += "<div>";
                //StrRetValue += "Available in our Warehouse : <span style=\"color: #641114 !important; font-weight: bold;\">" + TotalQuantityValue.ToString() + "</span></div>";
                //StrRetValue += "</td>";
                //StrRetValue += "</tr>";
                //StrRetValue += "<tr>";
                //StrRetValue += "<td align=\"center\" valign=\"middle\">";
                //StrRetValue += "<div>";
                //StrRetValue += "Call us at <span style=\"color: #641114 !important; font-weight: bold;\">" + StrContact.ToString() + "</span> if you are looking for more than <span style=\"color: #641114 !important; font-weight: bold;\">" + TotalQuantityValue.ToString() + "</span> Quantity(s)</div>";
                //StrRetValue += "</td>";
                //StrRetValue += "</tr>";

                StrRetValue += "<tr>";
                StrRetValue += "<td valign=\"middle\" align=\"center\" style=\"height:30px;\">";
                StrRetValue += "<div>This product is out of stock.</div>";
                StrRetValue += "</td>";
                StrRetValue += "</tr>";
                StrRetValue += "<tr>";
                StrRetValue += "<td valign=\"middle\" align=\"center\">";
                StrRetValue += "<div>Please call <span style=\"color:#641114 !important; font-weight:bold;\">" + StrContact.ToString() + "</span> for additional assistance.</div>";
                StrRetValue += "</td>";
                StrRetValue += "</tr>";


                StrRetValue += "<tr>";
                StrRetValue += "<td align=\"center\" valign=\"middle\">&nbsp;</td>";
                StrRetValue += "</tr>";
                StrRetValue += "</tbody>";
                StrRetValue += "</table>";
                StrRetValue += "</div>";
                StrRetValue += "</div>";
                StrRetValue += "</div>";


                string StrNew = "<div style=\"float: left; background-color: transparent; left: -15px; top: -18px;position: absolute;\">";
                StrNew += "<a title=\"\" onclick=\"javascript:document.getElementById('divInventoryCart').style.display = 'none';\" href=\"javascript:void(0);\"><img alt=\"\" src=\"/images/popupclose.png\"></a>";
                StrNew += "</div>";

                Response.Clear();
                Response.Write(StrNew.ToString() + "<div style='color:#cc0000;display:none;'>Not Sufficient Inventory</div>" + StrRetValue.ToString());
                Response.End();
            }
            else if (IsRestrictedAll == true)
            {
                Response.Clear();
                Response.Write("<div style='color:#cc0000;display:none;'>Customer is restricted.</div>");
                Response.End();
            }
            else
            {
                clsMiniCartComponent objMiniCart = new clsMiniCartComponent(Convert.ToInt32(Session["CustID"]));
                Response.Clear();
                Response.Write(objMiniCart.GetMiniCart());
                Response.End();
            }
        }

        /// <summary>
        /// Updates the Shopping Cart into Database
        /// </summary>
        /// <param name="Products">String Products</param>
        /// <param name="CustID">String CustID</param>
        public void UpdateShoppingCart(String Products, String CustID)
        {
            Int32 Cid = 0;
            if (CustID == "" || CustID == "0")
                Cid = Convert.ToInt32(System.Web.HttpContext.Current.Session["CustID"]);
            else
                Cid = Convert.ToInt32(CustID);

            string[] ProductArray;
            ProductArray = Products.Split('*');

            for (int i = 0; i < ProductArray.Length; i++)
            {
                string[] Temp;
                if (ProductArray[i] != null && ProductArray[i].ToString() != "")
                {
                    Temp = ProductArray[i].Split('-');
                    Yardqty = 0;
                    actualYard = 0;
                    UpdateCart(Convert.ToInt32(Temp[1].ToString().Replace("~hpd~", "-")), 0, Convert.ToInt32(Temp[4].ToString().Replace("~hpd~", "-")), Convert.ToDecimal(Temp[5].ToString().Replace("~hpd~", "-")), Temp[2].ToString().Replace("~hpd~", "-"), Temp[3].ToString().Replace("~hpd~", "-"));
                }
            }
            BindMiniCart();
        }

        /// <summary>
        /// Checks the customer that is he restricted?
        /// </summary>
        /// <returns>Returns true if Restricted, false otherwise</returns>
        private bool CheckCustomerIsRestricted()
        {
            bool IsRestrictedCust = false;
            IsRestrictedCust = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT RestrictedIPID FROM tb_RestrictedIP WHERE IPAddress='" + Request.UserHostAddress.ToString() + "'"));
            return IsRestrictedCust;
        }
    }
}