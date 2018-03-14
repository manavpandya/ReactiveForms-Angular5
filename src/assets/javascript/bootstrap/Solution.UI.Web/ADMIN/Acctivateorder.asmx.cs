using System;
using System.Web;
using System.Collections;
using System.Web.Services;
using System.Web.Services.Protocols;
using Solution.Bussines.Components;
using Solution.ShippingMethods;
using System.Text;
using Solution.Bussines.Components.Common;
using System.Data;
using System.Security.Cryptography;
using Solution.Bussines.Entities;
using Solution.Data;
using System.IO;
using System.Drawing;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.Serialization.Formatters.Binary;

namespace Solution.UI.Web
{
    /// <summary>
    /// Summary description for Acctivateorder
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class Acctivateorder : System.Web.Services.WebService
    {
        public AuthHeader Authentication = new AuthHeader();
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get OrderDetails")]
        public DataSet GetOrderDetails()
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                dsorder = CommonComponent.GetCommonDataSet("SELECT DISTINCT *  FROM tb_order WHERE Isnull(IsBackEnd,0)=0 AND StoreId=4 AND isnull(Deleted,0)=0 AND ShoppingCardID in (SELECT OrderedShoppingCartID FROM tb_OrderedShoppingCartItems WHERE isnull(IsAccepted,0)=0) UNION ALL SELECT DISTINCT *  FROM tb_order WHERE Isnull(IsBackEnd,0)=0 AND StoreId in (1,3) AND isnull(Deleted,0)=0 ");
                return dsorder;

            }
            return dsorder;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Shoppingcart Details")]
        public DataSet GetShoppingcartDetails(Int32 StoreId, Int32 OrderNumber)
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                if (StoreId == 4)
                {
                    dsorder = CommonComponent.GetCommonDataSet("Select OrderedCustomCartID, OrderedShoppingCartID, RefProductID, ProductName,case when isnull(SKUupgrade,'')='' then SKU else SKUupgrade end as SKU, Quantity, Price, VariantNames, VariantValues, Weight, IsRefunded, RefundAmount, CreatedOn, TrackingNumber, ShippedVia, ShippedOn, ShippedQty, WareHouseID, InventoryUpdated, Tax, Shipping, LastTrackingNumber, LastShippedVia, PackId, YahooID, ProductURL, MarryProducts, MarryWithtotalQuantity, MarryproductQuantity, upgradeprice, IsManualShipped, OrderItemID, IsAccepted, Receipt_Item_ID, RelatedproductID, IsProductType, YardQuantity, Actualyard, Notes, DiscountPrice, SKUupgrade, isnull(Ismadetoroder,0) as Ismadetoroder  from tb_OrderedShoppingCartItems Where OrderedShoppingCartID in (Select ShoppingCardID from tb_Order Where OrderNumber=" + OrderNumber.ToString() + ") AND isnull(IsAccepted,0)=0");
                }
                else
                {
                    dsorder = CommonComponent.GetCommonDataSet("Select OrderedCustomCartID, OrderedShoppingCartID, RefProductID, ProductName,case when isnull(SKUupgrade,'')='' then SKU else SKUupgrade end as SKU, Quantity, Price, VariantNames, VariantValues, Weight, IsRefunded, RefundAmount, CreatedOn, TrackingNumber, ShippedVia, ShippedOn, ShippedQty, WareHouseID, InventoryUpdated, Tax, Shipping, LastTrackingNumber, LastShippedVia, PackId, YahooID, ProductURL, MarryProducts, MarryWithtotalQuantity, MarryproductQuantity, upgradeprice, IsManualShipped, OrderItemID, IsAccepted, Receipt_Item_ID, RelatedproductID, IsProductType, YardQuantity, Actualyard, Notes, DiscountPrice, SKUupgrade, isnull(Ismadetoroder,0) as Ismadetoroder from tb_OrderedShoppingCartItems Where OrderedShoppingCartID in (Select ShoppingCardID from tb_Order Where OrderNumber=" + OrderNumber.ToString() + ")");
                }
                if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            string strwarehouse = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 Warehouse FROM  tb_SKUWareouse WHERE SKU in (SELECT SKU FROM tb_Product WHERE ProductId=" + dsorder.Tables[0].Rows[i]["RefProductID"].ToString().Replace("'", "''") + ")"));
                            if (!string.IsNullOrEmpty(strwarehouse))
                            {


                                Int32 wid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT top 1 WareHouseID FROM  tb_WareHouse WHERE Name like '%" + strwarehouse.Replace("'", "''") + "%'"));
                                if (wid > 0)
                                {
                                    dsorder.Tables[0].Rows[i]["WareHouseID"] = wid;
                                }
                                else
                                {
                                    dsorder.Tables[0].Rows[i]["WareHouseID"] = 0;
                                }
                            }
                            else
                            {
                                Int32 wid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT top 1 WareHouseID FROM tb_WareHouseProductInventory WHERE isnull(PreferredLocation,0)=1 and ProductId=" + dsorder.Tables[0].Rows[i]["RefProductID"].ToString().Replace("'", "''") + ""));
                                if (wid > 0)
                                {
                                    dsorder.Tables[0].Rows[i]["WareHouseID"] = wid;
                                }
                                else
                                {
                                    dsorder.Tables[0].Rows[i]["WareHouseID"] = 0;
                                }

                            }
                        }
                        catch { }

                    }
                    dsorder.Tables[0].AcceptChanges();

                }
                return dsorder;

            }
            return dsorder;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Order Details By ID")]
        public DataSet GetOrderDetailsByID(Int32 OrderNumber)
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                dsorder = CommonComponent.GetCommonDataSet("SELECT  *  FROM tb_order WHERE OrderNumber=" + OrderNumber + "");
                return dsorder;
            }
            return dsorder;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Decrypt Text")]
        public string GetDecryptText(string EncryptString)
        {
            string descryptString = "";
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                descryptString = SecurityComponent.Decrypt(EncryptString);
                return descryptString;
            }
            return descryptString;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Decrypt Text")]
        public void Updateorderfiled(Int32 flagback, Int32 OrderNumber, string orderGuid)
        {
            string descryptString = "";
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                CommonComponent.ExecuteCommonData("UPDATE tb_order SET ExportedDate=getdate(), IsBackEnd=" + flagback + ",BackEndGUID='" + orderGuid + "' WHERE OrderNumber=" + OrderNumber + "");
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get SKU Text")]
        public string GetproductUPC(string SKU, string strStoreId)
        {
            string strUpc = "";
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                strUpc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 UPC FROM tb_product WHERE isnull(UPC,'') <>'' AND  SKU='" + SKU + "' AND Storeid=" + strStoreId + " ANd isnull(active,0)=1 AND isnull(Deleted,0)=0;"));
                if (strStoreId.ToString() == "3")
                {
                    strUpc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 UPC FROM tb_product WHERE isnull(UPC,'') <>'' AND  SKU='" + SKU + "' AND Storeid=1 AND isnull(active,0)=1 AND isnull(Deleted,0)=0;"));
                }
                if (strStoreId.ToString() == "1" && string.IsNullOrEmpty(strUpc))
                {
                    strUpc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT UPC FROM tb_ProductVariantValue WHERE SKU='" + SKU.ToString() + "' and Isnull(UPC,'')<>''"));
                }
                else if (strStoreId.ToString() == "3" && string.IsNullOrEmpty(strUpc))
                {
                    strUpc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT UPC FROM tb_ProductVariantValue WHERE SKU='" + SKU.ToString() + "' and Isnull(UPC,'')<>''"));
                }
                return strUpc;
            }
            return strUpc;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Sale Price Tag Amazon")]
        public string GetproductSaleTypeAmazon(string SKU)
        {
            string strUpc = "";
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                strUpc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 SalePriceTag FROM tb_product WHERE SKU='" + SKU + "' AND Storeid=1 ANd isnull(active,0)=1 AND isnull(Deleted,0)=0"));
                if (string.IsNullOrEmpty(strUpc))
                {
                    strUpc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 SalePriceTag FROM tb_product WHERE Storeid=1 ANd isnull(active,0)=1 AND isnull(Deleted,0)=0 and ProductID in (SELECT ProductID FROM tb_ProductVariantValue WHERE SKU='" + SKU.ToString() + "')"));
                }


                return strUpc;
            }
            return strUpc;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get product Sale Type")]
        public string GetproductSaleType(Int32 ProductId)
        {
            string strUpc = "";
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                strUpc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 SalePriceTag FROM tb_product WHERE ProductId=" + ProductId + ""));
                return strUpc;
            }
            return strUpc;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get SKU Text")]
        public string GetStateCode(string Statename)
        {
            string strUpc = "";
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                strUpc = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Abbreviation FROm tb_State WHERE Name='" + Statename.Replace("'", "''") + "'"));
                return strUpc;
            }
            return strUpc;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Order For Shipp")]
        public DataSet GetOrderDetailsForShipping()
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                //dsorder = CommonComponent.GetCommonDataSet("SELECT reforderid as BackEndGUID,OrderNumber,StoreId FROM tb_order WHERE Storeid=4 AND (datepart(MONTH,dbo.tb_Order.Orderdate) >=datepart(MONTH,getdate()) Or datepart(MONTH,dbo.tb_Order.Orderdate) >=11) AND datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,getdate()) AND isnull(Deleted,0)=0 AND Isnull(IsBackShip,0)=0 ");
                dsorder = CommonComponent.GetCommonDataSet("SELECT reforderid as BackEndGUID,OrderNumber,StoreId,OrderDate FROM tb_order WHERE Storeid in (4) AND ((datepart(MONTH,dbo.tb_Order.Orderdate) >=datepart(MONTH,getdate()) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate()))) or  (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year, Getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-2, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))) AND isnull(Deleted,0)=0 AND Isnull(IsBackShip,0)=0 UNION ALL SELECT cast(OrderNumber as nvarchar(20)) as BackEndGUID,OrderNumber,StoreId,OrderDate FROM tb_order WHERE Storeid in (1) AND ((datepart(MONTH,dbo.tb_Order.Orderdate) >=datepart(MONTH,getdate()) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))  OR (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year, Getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-2, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))) AND isnull(Deleted,0)=0 AND Isnull(IsBackShip,0)=0 UNION ALL SELECT refOrderId as BackEndGUID,OrderNumber,StoreId,OrderDate FROM tb_order WHERE Storeid in (3) AND ((datepart(MONTH,dbo.tb_Order.Orderdate) >=datepart(MONTH,getdate()) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))  OR (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year, Getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-2, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))) AND isnull(Deleted,0)=0 AND Isnull(IsBackShip,0)=0");
                return dsorder;

            }
            return dsorder;
        }



        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Import Shipping Details")]
        public void ImportShippingDetailsByOrderNumber(DataSet dsShipping, Int32 OrderNumber, Int32 StoreID)
        {
            string descryptString = "";
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {

                if (dsShipping != null && dsShipping.Tables.Count > 0 && dsShipping.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsShipping.Tables[0].Rows.Count; i++)
                    {
                        if (!string.IsNullOrEmpty(dsShipping.Tables[0].Rows[i]["TrackingNumber"].ToString()))
                        {
                            CommonComponent.ExecuteCommonData("UPDATE tb_order SET IsBackShip=1,OrderStatus='Shipped',ShippedVIA='" + dsShipping.Tables[0].Rows[i]["Carrier"].ToString() + "',ShippingTrackingNumber='" + dsShipping.Tables[0].Rows[i]["TrackingNumber"].ToString() + "',ShippedOn='" + dsShipping.Tables[0].Rows[i]["ShipmentDate"].ToString() + "' WHERE OrderNumber=" + OrderNumber + "");
                            Int32 Shippid = 0;

                            DataSet dsCardDetails = new DataSet();
                            if (StoreID == 4)
                            {
                                dsCardDetails = CommonComponent.GetCommonDataSet("Select * from tb_OrderedShoppingCartItems Where OrderedShoppingCartID in (Select ShoppingCardID from tb_Order Where OrderNumber=" + OrderNumber.ToString() + ") AND isnull(IsAccepted,0)=0");
                            }
                            else
                            {
                                dsCardDetails = CommonComponent.GetCommonDataSet("Select * from tb_OrderedShoppingCartItems Where OrderedShoppingCartID in (Select ShoppingCardID from tb_Order Where OrderNumber=" + OrderNumber.ToString() + ")");
                            }
                            //dsCardDetails = GetShoppingcartDetails(StoreID, OrderNumber);
                            if (dsCardDetails != null && dsCardDetails.Tables.Count > 0 && dsCardDetails.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsCardDetails.Tables[0].Rows.Count; j++)
                                {
                                    Shippid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(OrderNumber) FROM tb_OrderShippedItems WHERE RefProductID=" + dsCardDetails.Tables[0].Rows[j]["RefProductID"].ToString() + " AND OrderNumber=" + OrderNumber + ""));
                                    if (Shippid > 0)
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_OrderShippedItems SET TrackingNumber='" + dsShipping.Tables[0].Rows[i]["TrackingNumber"].ToString() + "',ShippedVia='" + dsShipping.Tables[0].Rows[i]["Carrier"].ToString() + "',Shipped=1,ShippedOn='" + dsShipping.Tables[0].Rows[i]["ShipmentDate"].ToString() + "',ShippedQty=" + dsCardDetails.Tables[0].Rows[j]["Quantity"].ToString() + " WHERE RefProductID=" + dsCardDetails.Tables[0].Rows[j]["RefProductID"].ToString() + " AND OrderNumber=" + OrderNumber + "");
                                    }
                                    else
                                    {
                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_OrderShippedItems(OrderNumber,RefProductID,TrackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty) VALUES (" + OrderNumber + "," + dsCardDetails.Tables[0].Rows[j]["RefProductID"].ToString() + ",'" + dsShipping.Tables[0].Rows[i]["TrackingNumber"].ToString() + "','" + dsShipping.Tables[0].Rows[i]["Carrier"].ToString() + "',1,'" + dsShipping.Tables[0].Rows[i]["ShipmentDate"].ToString() + "'," + dsCardDetails.Tables[0].Rows[j]["Quantity"].ToString() + ")");
                                    }
                                }
                            }
                        }
                    }



                }
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update Inventory")]
        public void UpdateInventory(DataSet DsInventory)
        {
            string descryptString = "";
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                if (DsInventory != null && DsInventory.Tables.Count > 0 && DsInventory.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < DsInventory.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            Int32 Invetory = Convert.ToInt32(string.Format("{0:0}", Convert.ToDecimal(DsInventory.Tables[0].Rows[i]["QuantityOnHand"].ToString())));
                            if (Invetory <= 0)
                            {
                                Invetory = 0;
                            }
                            //if (!string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) && (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("so") > -1  || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("special order") > -1))
                            //{
                            //    continue;
                            //}
                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + Invetory.ToString() + " WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' ");
                            DataSet dsProduct = new DataSet();

                            dsProduct = CommonComponent.GetCommonDataSet("SELECT ProductID FROM tb_Product WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' ");
                            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsProduct.Tables[0].Rows.Count; j++)
                                {


                                    Int32 WId = 0;
                                    string strCity = "";
                                    string stwnm = "";
                                    if (string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("main") > -1)
                                    {
                                        strCity = "Livermore";
                                        stwnm = "CA";

                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atl")
                                    {
                                        strCity = "Atlanta";
                                        stwnm = "ATL";

                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atlanta")
                                    {
                                        strCity = "Atlanta";
                                        stwnm = "ATL";
                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "so" || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "special order")
                                    {
                                        strCity = "Special Order";
                                        stwnm = "SO";
                                    }
                                    else
                                    {
                                        strCity = "Livermore";

                                    }

                                    WId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 WareHouseID FROM tb_WareHouse WHERE City like '%" + strCity.ToString() + "%'"));
                                    //////CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                    //CommonComponent.ErrorLog("Product Ware House", dsProduct.Tables[0].Rows[j]["ProductID"].ToString(), WId.ToString() + " " + DsInventory.Tables[0].Rows[i]["City"].ToString());



                                    Int32 PreferredLocation = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count([Main SKU]) FROM All_Ready_Made_Product WHERE isnull([Default Warehouse],'') <> '' and isnull([Default Warehouse],'') like '%" + stwnm + "%' and UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "'"));
                                    if (PreferredLocation > 0)
                                    {
                                        PreferredLocation = 1;
                                    }
                                    else
                                    {
                                        PreferredLocation = 0;
                                    }
                                    //StreamWriter writer = null;
                                    //try
                                    //{
                                    //    writer = new StreamWriter(Server.MapPath("AmazonErrolog.txt"), true);
                                    //    writer.WriteLine(DateTime.Now.ToString() + " : " + LogString);
                                    //}
                                    //catch { }
                                    //finally
                                    //{
                                    //    if (writer != null)
                                    //        writer.Close();
                                    //}

                                    Int32 InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId + ""));
                                    if (InvProducwarehouse > 0)
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + Invetory.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId.ToString() + "");
                                    }
                                    else
                                    {
                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "," + Invetory + "," + PreferredLocation + ")");
                                    }

                                    DataSet dsVariant = new DataSet();
                                    Int32 pid = 0;
                                    //dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreId=1) ");
                                    dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE  isnull(Deleted,0)=0 and StoreId=1) ");
                                    if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dsVariant.Tables[0].Rows.Count; k++)
                                        {
                                            pid = Convert.ToInt32(dsVariant.Tables[0].Rows[k]["ProductID"].ToString());
                                            /// CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                            /// 

                                            InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + ""));
                                            if (InvProducwarehouse > 0)
                                            {
                                                CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductVariantInventory SET Inventory=" + Invetory + " WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                            }
                                            else
                                            {

                                                CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID,VariantID,VariantValueID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + Invetory + ",0)");
                                            }
                                            Int32 TotalInvantory = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductVariantInventory WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " "));
                                            CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET Inventory=" + TotalInvantory + " WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "");

                                            DataSet dswarehouseproduct = new DataSet();
                                            dswarehouseproduct = CommonComponent.GetCommonDataSet("SELECT SUM(isnull(tb_WareHouseProductVariantInventory.Inventory,0)) as inventory,tb_WareHouseProductVariantInventory.WareHouseID FROM tb_WareHouseProductVariantInventory INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.VariantValueID=tb_WareHouseProductVariantInventory.VariantValueID WHERE tb_WareHouseProductVariantInventory.ProductID=" + pid.ToString() + " GROUP BY tb_WareHouseProductVariantInventory.WareHouseID");
                                            if (dswarehouseproduct != null && dswarehouseproduct.Tables.Count > 0 && dswarehouseproduct.Tables[0].Rows.Count > 0)
                                            {
                                                Int32 totalQty = 0;
                                                for (int m = 0; m < dswarehouseproduct.Tables[0].Rows.Count; m++)
                                                {
                                                    Int32 ProductInventoryID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ProductInventoryID,0) FROM tb_WareHouseProductInventory WHERE ProductID=" + pid.ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + ""));
                                                    totalQty = totalQty + Convert.ToInt32(dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString());
                                                    if (ProductInventoryID > 0)
                                                    {
                                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + " WHERE ProductID=" + pid.ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "");
                                                    }
                                                    else
                                                    {
                                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "," + pid.ToString() + "," + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + "," + PreferredLocation + ")");
                                                    }
                                                }
                                                CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + totalQty.ToString() + " WHERE ProductID=" + pid.ToString() + "");

                                            }
                                        }
                                        try
                                        {
                                            Int32 TotalqtyforProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + ""));
                                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + TotalqtyforProduct.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "");
                                        }
                                        catch
                                        {

                                        }


                                    }
                                    else
                                    {

                                        Int32 TotalqtyforProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + ""));

                                        CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + TotalqtyforProduct.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "");
                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET PreferredLocation=" + PreferredLocation + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " and WareHouseID=" + WId + "");
                                    }



                                    //tb_WareHouseProductVariantInventory
                                }
                            }
                            else
                            {
                                Int32 WId = 0;
                                string strCity = "";
                                string stwnm = "";
                                if (string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("main") > -1)
                                {
                                    strCity = "Livermore";
                                    stwnm = "CA";

                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atl")
                                {
                                    strCity = "Atlanta";
                                    stwnm = "ATL";
                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atlanta")
                                {
                                    strCity = "Atlanta";
                                    stwnm = "ATL";
                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "so" || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "special order")
                                {
                                    strCity = "Special Order";
                                    stwnm = "SO";
                                }
                                else
                                {
                                    strCity = "Livermore";// DsInventory.Tables[0].Rows[i]["City"].ToString();
                                    stwnm = "";

                                }
                                WId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 WareHouseID FROM tb_WareHouse WHERE City like '%" + strCity.ToString() + "%'"));


                                Int32 PreferredLocation = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count([Main SKU]) FROM All_Ready_Made_Product WHERE isnull([Default Warehouse],'') <> '' and isnull([Default Warehouse],'') like '%" + stwnm + "%' and UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "'"));
                                if (PreferredLocation > 0)
                                {
                                    PreferredLocation = 1;
                                }
                                else
                                {
                                    PreferredLocation = 0;
                                }

                                //CommonComponent.ErrorLog("Product Ware House name : ", DsInventory.Tables[0].Rows[i]["UPC"].ToString(), WId.ToString() + " " + DsInventory.Tables[0].Rows[i]["City"].ToString());
                                DataSet dsVariant = new DataSet();
                                //dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreId=1)");
                                dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Deleted,0)=0 and StoreId=1)");
                                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsVariant.Tables[0].Rows.Count; k++)
                                    {
                                        //CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + " ");
                                        Int32 InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + ""));
                                        if (InvProducwarehouse > 0)
                                        {
                                            CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductVariantInventory SET Inventory=" + Invetory + " WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + "");
                                        }
                                        else
                                        {
                                            CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID,VariantID,VariantValueID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + Invetory + "," + PreferredLocation + ")");
                                        }

                                        Int32 TotalInvantory = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductVariantInventory WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " "));
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET Inventory=" + TotalInvantory + " WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "");


                                        DataSet dswarehouseproduct = new DataSet();
                                        dswarehouseproduct = CommonComponent.GetCommonDataSet("SELECT SUM(isnull(tb_WareHouseProductVariantInventory.Inventory,0)) as inventory,tb_WareHouseProductVariantInventory.WareHouseID FROM tb_WareHouseProductVariantInventory INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.VariantValueID=tb_WareHouseProductVariantInventory.VariantValueID WHERE tb_WareHouseProductVariantInventory.ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " GROUP BY tb_WareHouseProductVariantInventory.WareHouseID");//SELECT SUM(isnull(Inventory,0)) as inventory,WareHouseID FROM tb_WareHouseProductVariantInventory WHERE ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " GROUP BY WareHouseID
                                        if (dswarehouseproduct != null && dswarehouseproduct.Tables.Count > 0 && dswarehouseproduct.Tables[0].Rows.Count > 0)
                                        {
                                            Int32 totalQty = 0;
                                            Int32 PId = Convert.ToInt32(dsVariant.Tables[0].Rows[k]["ProductID"].ToString());
                                            for (int m = 0; m < dswarehouseproduct.Tables[0].Rows.Count; m++)
                                            {
                                                Int32 ProductInventoryID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ProductInventoryID,0) FROM tb_WareHouseProductInventory WHERE ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + ""));
                                                totalQty = totalQty + Convert.ToInt32(dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString());

                                                if (ProductInventoryID > 0)
                                                {
                                                    CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + " WHERE ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "");
                                                }
                                                else
                                                {
                                                    CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + "," + PreferredLocation + ")");
                                                }
                                            }
                                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + totalQty.ToString() + " WHERE ProductID=" + PId.ToString() + "");

                                        }


                                    }



                                }
                            }
                        }
                        catch { }

                    }
                }
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update Inventory")]
        public string UpdateInventoryNew(DataSet DsInventory)
        {
            string descryptString = "";

            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                if (DsInventory != null && DsInventory.Tables.Count > 0 && DsInventory.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < DsInventory.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            Int32 Invetory = Convert.ToInt32(string.Format("{0:0}", Convert.ToDecimal(DsInventory.Tables[0].Rows[i]["QuantityOnHand"].ToString())));
                            if (Invetory <= 0)
                            {
                                Invetory = 0;
                            }
                            //if (!string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) && (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("so") > -1 || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("special order") > -1))
                            //{
                            //    continue;
                            //}
                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + Invetory.ToString() + " WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' ");
                            DataSet dsProduct = new DataSet();

                            dsProduct = CommonComponent.GetCommonDataSet("SELECT ProductID FROM tb_Product WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' ");
                            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsProduct.Tables[0].Rows.Count; j++)
                                {


                                    Int32 WId = 0;
                                    string strCity = "";
                                    string stwnm = "";
                                    if (string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("main") > -1)
                                    {
                                        strCity = "Livermore";
                                        stwnm = "CA";

                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atl")
                                    {
                                        strCity = "Atlanta";
                                        stwnm = "ATL";
                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atlanta")
                                    {
                                        strCity = "Atlanta";
                                        stwnm = "ATL";
                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "so" || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "special order")
                                    {
                                        strCity = "Special Order";
                                        stwnm = "SO";
                                    }
                                    else
                                    {
                                        strCity = "Livermore";

                                    }

                                    WId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 WareHouseID FROM tb_WareHouse WHERE City like '%" + strCity.ToString() + "%'"));
                                    //////CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                    //CommonComponent.ErrorLog("Product Ware House", dsProduct.Tables[0].Rows[j]["ProductID"].ToString(), WId.ToString() + " " + DsInventory.Tables[0].Rows[i]["City"].ToString());



                                    Int32 PreferredLocation = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count([Main SKU]) FROM All_Ready_Made_Product WHERE isnull([Default Warehouse],'') <> '' and isnull([Default Warehouse],'') like '%" + stwnm + "%' and UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "'"));
                                    if (PreferredLocation > 0)
                                    {
                                        PreferredLocation = 1;
                                    }
                                    else
                                    {
                                        PreferredLocation = 0;
                                    }
                                    //StreamWriter writer = null;
                                    //try
                                    //{
                                    //    writer = new StreamWriter(Server.MapPath("AmazonErrolog.txt"), true);
                                    //    writer.WriteLine(DateTime.Now.ToString() + " : " + LogString);
                                    //}
                                    //catch { }
                                    //finally
                                    //{
                                    //    if (writer != null)
                                    //        writer.Close();
                                    //}

                                    Int32 InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId + ""));
                                    if (InvProducwarehouse > 0)
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + Invetory.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId.ToString() + "");
                                    }
                                    else
                                    {
                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "," + Invetory + "," + PreferredLocation + ")");
                                    }

                                    DataSet dsVariant = new DataSet();
                                    Int32 pid = 0;
                                    //dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreId=1) ");
                                    dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Deleted,0)=0 and StoreId=1) ");
                                    if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dsVariant.Tables[0].Rows.Count; k++)
                                        {
                                            pid = Convert.ToInt32(dsVariant.Tables[0].Rows[k]["ProductID"].ToString());
                                            /// CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                            /// 

                                            InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + ""));
                                            if (InvProducwarehouse > 0)
                                            {
                                                CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductVariantInventory SET Inventory=" + Invetory + " WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                            }
                                            else
                                            {

                                                CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID,VariantID,VariantValueID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + Invetory + ",0)");
                                            }
                                            Int32 TotalInvantory = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductVariantInventory WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " "));
                                            CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET Inventory=" + TotalInvantory + " WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "");

                                            DataSet dswarehouseproduct = new DataSet();
                                            dswarehouseproduct = CommonComponent.GetCommonDataSet("SELECT SUM(isnull(tb_WareHouseProductVariantInventory.Inventory,0)) as inventory,tb_WareHouseProductVariantInventory.WareHouseID FROM tb_WareHouseProductVariantInventory INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.VariantValueID=tb_WareHouseProductVariantInventory.VariantValueID WHERE tb_WareHouseProductVariantInventory.ProductID=" + pid.ToString() + " GROUP BY tb_WareHouseProductVariantInventory.WareHouseID");//SELECT SUM(isnull(Inventory,0)) as inventory,WareHouseID FROM tb_WareHouseProductVariantInventory WHERE ProductID=" + pid.ToString() + " GROUP BY WareHouseID
                                            if (dswarehouseproduct != null && dswarehouseproduct.Tables.Count > 0 && dswarehouseproduct.Tables[0].Rows.Count > 0)
                                            {
                                                Int32 totalQty = 0;
                                                for (int m = 0; m < dswarehouseproduct.Tables[0].Rows.Count; m++)
                                                {
                                                    Int32 ProductInventoryID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ProductInventoryID,0) FROM tb_WareHouseProductInventory WHERE ProductID=" + pid.ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + ""));
                                                    totalQty = totalQty + Convert.ToInt32(dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString());
                                                    if (ProductInventoryID > 0)
                                                    {
                                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + " WHERE ProductID=" + pid.ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "");
                                                    }
                                                    else
                                                    {
                                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "," + pid.ToString() + "," + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + "," + PreferredLocation + ")");
                                                    }
                                                }
                                                CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + totalQty.ToString() + " WHERE ProductID=" + pid.ToString() + "");

                                            }
                                        }
                                        try
                                        {
                                            Int32 TotalqtyforProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + ""));
                                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + TotalqtyforProduct.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "");
                                        }
                                        catch
                                        {

                                        }

                                    }
                                    else
                                    {

                                        Int32 TotalqtyforProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + ""));

                                        CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + TotalqtyforProduct.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "");
                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET PreferredLocation=" + PreferredLocation + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " and WareHouseID=" + WId + "");

                                    }



                                    //tb_WareHouseProductVariantInventory
                                }
                            }
                            else
                            {
                                Int32 WId = 0;
                                string strCity = "";
                                string stwnm = "";
                                if (string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("main") > -1)
                                {
                                    strCity = "Livermore";
                                    stwnm = "CA";

                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atl")
                                {
                                    strCity = "Atlanta";
                                    stwnm = "ATL";
                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atlanta")
                                {
                                    strCity = "Atlanta";
                                    stwnm = "ATL";
                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "so" || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "special order")
                                {
                                    strCity = "Special Order";
                                    stwnm = "SO";
                                }
                                else
                                {
                                    strCity = "Livermore";// DsInventory.Tables[0].Rows[i]["City"].ToString();
                                    stwnm = "";

                                }
                                WId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 WareHouseID FROM tb_WareHouse WHERE City like '%" + strCity.ToString() + "%'"));


                                Int32 PreferredLocation = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count([Main SKU]) FROM All_Ready_Made_Product WHERE isnull([Default Warehouse],'') <> '' and isnull([Default Warehouse],'') like '%" + stwnm + "%' and UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "'"));
                                if (PreferredLocation > 0)
                                {
                                    PreferredLocation = 1;
                                }
                                else
                                {
                                    PreferredLocation = 0;
                                }

                                //CommonComponent.ErrorLog("Product Ware House name : ", DsInventory.Tables[0].Rows[i]["UPC"].ToString(), WId.ToString() + " " + DsInventory.Tables[0].Rows[i]["City"].ToString());
                                DataSet dsVariant = new DataSet();
                                //dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreId=1)");
                                dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE  isnull(Deleted,0)=0 and StoreId=1)");
                                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsVariant.Tables[0].Rows.Count; k++)
                                    {
                                        //CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + " ");
                                        Int32 InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + ""));
                                        if (InvProducwarehouse > 0)
                                        {
                                            CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductVariantInventory SET Inventory=" + Invetory + " WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + "");
                                        }
                                        else
                                        {
                                            CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID,VariantID,VariantValueID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + Invetory + "," + PreferredLocation + ")");
                                        }

                                        Int32 TotalInvantory = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductVariantInventory WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " "));
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET Inventory=" + TotalInvantory + " WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "");


                                        DataSet dswarehouseproduct = new DataSet();
                                        dswarehouseproduct = CommonComponent.GetCommonDataSet("SELECT SUM(isnull(tb_WareHouseProductVariantInventory.Inventory,0)) as inventory,tb_WareHouseProductVariantInventory.WareHouseID FROM tb_WareHouseProductVariantInventory INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.VariantValueID=tb_WareHouseProductVariantInventory.VariantValueID WHERE tb_WareHouseProductVariantInventory.ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " GROUP BY tb_WareHouseProductVariantInventory.WareHouseID"); //SELECT SUM(isnull(Inventory,0)) as inventory,WareHouseID FROM tb_WareHouseProductVariantInventory WHERE ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " GROUP BY WareHouseID
                                        if (dswarehouseproduct != null && dswarehouseproduct.Tables.Count > 0 && dswarehouseproduct.Tables[0].Rows.Count > 0)
                                        {
                                            Int32 totalQty = 0;
                                            Int32 PId = Convert.ToInt32(dsVariant.Tables[0].Rows[k]["ProductID"].ToString());
                                            for (int m = 0; m < dswarehouseproduct.Tables[0].Rows.Count; m++)
                                            {
                                                Int32 ProductInventoryID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ProductInventoryID,0) FROM tb_WareHouseProductInventory WHERE ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + ""));
                                                totalQty = totalQty + Convert.ToInt32(dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString());

                                                if (ProductInventoryID > 0)
                                                {
                                                    CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + " WHERE ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "");
                                                }
                                                else
                                                {
                                                    CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + "," + PreferredLocation + ")");
                                                }
                                            }
                                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + totalQty.ToString() + " WHERE ProductID=" + PId.ToString() + "");

                                        }


                                    }



                                }
                            }
                        }
                        catch { }

                    }
                }
            }
            return descryptString;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update Inventory")]
        public string UpdateInventoryNewbybyte(byte[] f)
        {
            DataSet DsInventory = new DataSet();

            string descryptString = "";

            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                BinaryFormatter bformatter = new BinaryFormatter();
                MemoryStream stream = new MemoryStream();
                stream = new MemoryStream(f);
                DsInventory = (DataSet)bformatter.Deserialize(stream);
                stream.Close();

                if (DsInventory != null && DsInventory.Tables.Count > 0 && DsInventory.Tables[0].Rows.Count > 0)
                {
                    CommonComponent.ErrorLog("UpdateInventoryNewbybyte", " Total count :", DsInventory.Tables[0].Rows.Count.ToString());
                    for (int i = 0; i < DsInventory.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            Int32 Invetory = Convert.ToInt32(string.Format("{0:0}", Convert.ToDecimal(DsInventory.Tables[0].Rows[i]["QuantityOnHand"].ToString())));
                            if (Invetory <= 0)
                            {
                                Invetory = 0;
                            }
                            //if (!string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) && (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("so") > -1 || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("special order") > -1))
                            //{
                            //    continue;
                            //}
                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + Invetory.ToString() + " WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' ");
                            DataSet dsProduct = new DataSet();

                            dsProduct = CommonComponent.GetCommonDataSet("SELECT ProductID FROM tb_Product WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' ");
                            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsProduct.Tables[0].Rows.Count; j++)
                                {


                                    Int32 WId = 0;
                                    string strCity = "";
                                    string stwnm = "";
                                    if (string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("main") > -1)
                                    {
                                        strCity = "Livermore";
                                        stwnm = "CA";

                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atl")
                                    {
                                        strCity = "Atlanta";
                                        stwnm = "ATL";
                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atlanta")
                                    {
                                        strCity = "Atlanta";
                                        stwnm = "ATL";
                                    }
                                    else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "so" || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "special order")
                                    {
                                        strCity = "Special Order";
                                        stwnm = "SO";
                                    }
                                    else
                                    {
                                        strCity = "Livermore";

                                    }

                                    WId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 WareHouseID FROM tb_WareHouse WHERE City like '%" + strCity.ToString() + "%'"));
                                    //////CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                    //CommonComponent.ErrorLog("Product Ware House", dsProduct.Tables[0].Rows[j]["ProductID"].ToString(), WId.ToString() + " " + DsInventory.Tables[0].Rows[i]["City"].ToString());



                                    Int32 PreferredLocation = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count([Main SKU]) FROM All_Ready_Made_Product WHERE isnull([Default Warehouse],'') <> '' and isnull([Default Warehouse],'') like '%" + stwnm + "%' and UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "'"));
                                    if (PreferredLocation > 0)
                                    {
                                        PreferredLocation = 1;
                                    }
                                    else
                                    {
                                        PreferredLocation = 0;
                                    }
                                    //StreamWriter writer = null;
                                    //try
                                    //{
                                    //    writer = new StreamWriter(Server.MapPath("AmazonErrolog.txt"), true);
                                    //    writer.WriteLine(DateTime.Now.ToString() + " : " + LogString);
                                    //}
                                    //catch { }
                                    //finally
                                    //{
                                    //    if (writer != null)
                                    //        writer.Close();
                                    //}

                                    Int32 InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId + ""));
                                    if (InvProducwarehouse > 0)
                                    {
                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + Invetory.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " AND WareHouseID=" + WId.ToString() + "");
                                    }
                                    else
                                    {
                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "," + Invetory + "," + PreferredLocation + ")");
                                    }

                                    DataSet dsVariant = new DataSet();
                                    Int32 pid = 0;
                                    //dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreId=1) ");
                                    dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Deleted,0)=0 and StoreId=1) ");
                                    if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dsVariant.Tables[0].Rows.Count; k++)
                                        {
                                            pid = Convert.ToInt32(dsVariant.Tables[0].Rows[k]["ProductID"].ToString());
                                            /// CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                            /// 

                                            InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + ""));
                                            if (InvProducwarehouse > 0)
                                            {
                                                CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductVariantInventory SET Inventory=" + Invetory + " WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + WId + "");
                                            }
                                            else
                                            {

                                                CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID,VariantID,VariantValueID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + Invetory + ",0)");
                                            }
                                            Int32 TotalInvantory = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductVariantInventory WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " "));
                                            CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET Inventory=" + TotalInvantory + " WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "");

                                            DataSet dswarehouseproduct = new DataSet();
                                            dswarehouseproduct = CommonComponent.GetCommonDataSet("SELECT SUM(isnull(tb_WareHouseProductVariantInventory.Inventory,0)) as inventory,tb_WareHouseProductVariantInventory.WareHouseID FROM tb_WareHouseProductVariantInventory INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.VariantValueID=tb_WareHouseProductVariantInventory.VariantValueID WHERE tb_WareHouseProductVariantInventory.ProductID=" + pid.ToString() + " GROUP BY tb_WareHouseProductVariantInventory.WareHouseID");
                                            if (dswarehouseproduct != null && dswarehouseproduct.Tables.Count > 0 && dswarehouseproduct.Tables[0].Rows.Count > 0)
                                            {
                                                Int32 totalQty = 0;
                                                for (int m = 0; m < dswarehouseproduct.Tables[0].Rows.Count; m++)
                                                {
                                                    Int32 ProductInventoryID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ProductInventoryID,0) FROM tb_WareHouseProductInventory WHERE ProductID=" + pid.ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + ""));
                                                    totalQty = totalQty + Convert.ToInt32(dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString());
                                                    if (ProductInventoryID > 0)
                                                    {
                                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + " WHERE ProductID=" + pid.ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "");
                                                    }
                                                    else
                                                    {
                                                        CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "," + pid.ToString() + "," + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + "," + PreferredLocation + ")");
                                                    }
                                                }
                                                CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + totalQty.ToString() + " WHERE ProductID=" + pid.ToString() + "");

                                            }
                                        }
                                        try
                                        {
                                            Int32 TotalqtyforProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + ""));
                                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + TotalqtyforProduct.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "");
                                        }
                                        catch
                                        {

                                        }

                                    }
                                    else
                                    {

                                        Int32 TotalqtyforProduct = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductInventory WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + ""));

                                        CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + TotalqtyforProduct.ToString() + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + "");
                                        CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET PreferredLocation=" + PreferredLocation + " WHERE ProductID=" + dsProduct.Tables[0].Rows[j]["ProductID"].ToString() + " and WareHouseID=" + WId + "");

                                    }



                                    //tb_WareHouseProductVariantInventory
                                }
                            }
                            else
                            {
                                Int32 WId = 0;
                                string strCity = "";
                                string stwnm = "";
                                if (string.IsNullOrEmpty(DsInventory.Tables[0].Rows[i]["City"].ToString()) || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().IndexOf("main") > -1)
                                {
                                    strCity = "Livermore";
                                    stwnm = "CA";

                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atl")
                                {
                                    strCity = "Atlanta";
                                    stwnm = "ATL";
                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "atlanta")
                                {
                                    strCity = "Atlanta";
                                    stwnm = "ATL";
                                }
                                else if (DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "so" || DsInventory.Tables[0].Rows[i]["City"].ToString().ToLower().Trim() == "special order")
                                {
                                    strCity = "Special Order";
                                    stwnm = "SO";
                                }
                                else
                                {
                                    strCity = "Livermore";// DsInventory.Tables[0].Rows[i]["City"].ToString();
                                    stwnm = "";

                                }
                                WId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 WareHouseID FROM tb_WareHouse WHERE City like '%" + strCity.ToString() + "%'"));


                                Int32 PreferredLocation = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count([Main SKU]) FROM All_Ready_Made_Product WHERE isnull([Default Warehouse],'') <> '' and isnull([Default Warehouse],'') like '%" + stwnm + "%' and UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString().Trim() + "'"));
                                if (PreferredLocation > 0)
                                {
                                    PreferredLocation = 1;
                                }
                                else
                                {
                                    PreferredLocation = 0;
                                }

                                //CommonComponent.ErrorLog("Product Ware House name : ", DsInventory.Tables[0].Rows[i]["UPC"].ToString(), WId.ToString() + " " + DsInventory.Tables[0].Rows[i]["City"].ToString());
                                DataSet dsVariant = new DataSet();
                                //dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreId=1)");
                                dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantValueID,ProductID,VariantID FROM tb_ProductVariantValue WHERE UPC='" + DsInventory.Tables[0].Rows[i]["UPC"].ToString() + "' and ProductId in (SELECT ProductId FROM tb_Product WHERE  isnull(Deleted,0)=0 and StoreId=1)");
                                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                                {
                                    for (int k = 0; k < dsVariant.Tables[0].Rows.Count; k++)
                                    {
                                        //CommonComponent.ExecuteCommonData("DELETE FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + " ");
                                        Int32 InvProducwarehouse = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 WareHouseID FROM tb_WareHouseProductVariantInventory WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + ""));
                                        if (InvProducwarehouse > 0)
                                        {
                                            CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductVariantInventory SET Inventory=" + Invetory + " WHERE VariantID=" + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + " AND VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "  AND WareHouseID=" + WId + "");
                                        }
                                        else
                                        {
                                            CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID,VariantID,VariantValueID,ProductID,Inventory,PreferredLocation) VALUES (" + WId.ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + Invetory + "," + PreferredLocation + ")");
                                        }

                                        Int32 TotalInvantory = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT SUM(isnull(Inventory,0)) as inventory FROM tb_WareHouseProductVariantInventory WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " "));
                                        CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET Inventory=" + TotalInvantory + " WHERE VariantValueID=" + dsVariant.Tables[0].Rows[k]["VariantValueID"].ToString() + " AND ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "");


                                        DataSet dswarehouseproduct = new DataSet();
                                        dswarehouseproduct = CommonComponent.GetCommonDataSet("SELECT SUM(isnull(tb_WareHouseProductVariantInventory.Inventory,0)) as inventory,tb_WareHouseProductVariantInventory.WareHouseID FROM tb_WareHouseProductVariantInventory INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.VariantValueID=tb_WareHouseProductVariantInventory.VariantValueID WHERE tb_WareHouseProductVariantInventory.ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " GROUP BY tb_WareHouseProductVariantInventory.WareHouseID");
                                        if (dswarehouseproduct != null && dswarehouseproduct.Tables.Count > 0 && dswarehouseproduct.Tables[0].Rows.Count > 0)
                                        {
                                            Int32 totalQty = 0;
                                            Int32 PId = Convert.ToInt32(dsVariant.Tables[0].Rows[k]["ProductID"].ToString());
                                            for (int m = 0; m < dswarehouseproduct.Tables[0].Rows.Count; m++)
                                            {
                                                Int32 ProductInventoryID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ProductInventoryID,0) FROM tb_WareHouseProductInventory WHERE ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + ""));
                                                totalQty = totalQty + Convert.ToInt32(dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString());

                                                if (ProductInventoryID > 0)
                                                {
                                                    CommonComponent.ExecuteCommonData("UPDATE tb_WareHouseProductInventory SET Inventory=" + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + " WHERE ProductID=" + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + " AND WareHouseID=" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "");
                                                }
                                                else
                                                {
                                                    CommonComponent.ExecuteCommonData("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (" + dswarehouseproduct.Tables[0].Rows[m]["WareHouseID"].ToString() + "," + dsVariant.Tables[0].Rows[k]["ProductID"].ToString() + "," + dswarehouseproduct.Tables[0].Rows[m]["inventory"].ToString() + "," + PreferredLocation + ")");
                                                }
                                            }
                                            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Inventory=" + totalQty.ToString() + " WHERE ProductID=" + PId.ToString() + "");

                                        }


                                    }



                                }
                            }
                        }
                        catch { }

                    }
                    CommonComponent.ErrorLog("UpdateInventoryNewbybyte", " End Total count :", DsInventory.Tables[0].Rows.Count.ToString());
                }
            }
            return descryptString;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update Partner Order Number")]
        public void UpdatePartnerOrderNumber(Int32 OrderNumber, Int32 StoreID, string strpartNum)
        {
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                CommonComponent.ExecuteCommonData("UPDATE tb_order SET PartnerNumber='" + strpartNum + "' WHERE OrderNumber=" + OrderNumber + "");
            }
        }
        //[SoapHeader("Authentication", Required = true)]
        //[WebMethod(Description = "Get Table information")]
        //public DataSet GetTableInformation(string TableName)
        //{
        //    DataSet dsorder = new DataSet();
        //    if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
        //    {
        //        dsorder = CommonComponent.GetCommonDataSet("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='" + TableName.Replace("'", "''") + "'");
        //        return dsorder;
        //    }
        //    return dsorder;
        //}
        //[SoapHeader("Authentication", Required = true)]
        //[WebMethod(Description = "Get Table Column information")]
        //public DataSet GetTablecolumnInformation(string TableName)
        //{
        //    DataSet dsorder = new DataSet();
        //    if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
        //    {
        //        dsorder = CommonComponent.GetCommonDataSet("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + TableName.Replace("'", "''") + "'");
        //        return dsorder;
        //    }
        //    return dsorder;
        //}
        //[SoapHeader("Authentication", Required = true)]
        //[WebMethod(Description = "Get Table Column information")]
        //public DataSet GetCustomerInformation(Int32 OrderNumber)
        //{
        //    DataSet dsorder = new DataSet();
        //    if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
        //    {
        //        //dsorder = CommonComponent.GetCommonDataSet("SELECT tb_Customer.CustomerID,tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Address.Company,tb_Address.Address1,tb_Address.Address2,tb_Address.City,tb_Address.State,tb_Address.Phone,tb_Address.ZipCode,tb_Address.Fax,tb_country.TwoLetterISOCode FROM tb_Customer inner join tb_address on tb_Customer.BillingAddressID=tb_Address.AddressId left outer join tb_country on tb_address.Country=tb_country.CountryID  WHERE tb_Customer.CustomerId=" + CustomerId + "");
        //        dsorder = CommonComponent.GetCommonDataSet("select *,BillCountry.TwoLetterISOCode  from tb_order left outer join tb_country as BillCountry on tb_order.BillingCountry=BillCountry.name  where ordernumber=" + OrderNumber + "");
        //        return dsorder;
        //    }
        //    return dsorder;
        //}
        //[SoapHeader("Authentication", Required = true)]
        //[WebMethod(Description = "Get Table Column information")]
        //public DataSet GetCustomerAddressInformation(Int32 AddressId)
        //{
        //    DataSet dsorder = new DataSet();
        //    if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
        //    {
        //        dsorder = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Address WHERE AddressId=" + AddressId + "");
        //        return dsorder;
        //    }
        //    return dsorder;
        //}
        //[SoapHeader("Authentication", Required = true)]
        //[WebMethod(Description = "Get Customer Ship Information")]
        //public DataSet GetCustomerShipInformation(Int32 OrderNumber)
        //{
        //    DataSet dsorder = new DataSet();
        //    if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
        //    {
        //        // dsorder = CommonComponent.GetCommonDataSet("SELECT tb_Customer.CustomerID,tb_Address.Email,tb_Address.FirstName,tb_Address.LastName,tb_Address.Company,tb_Address.Address1,tb_Address.Address2,tb_Address.City,tb_Address.State,tb_Address.Phone,tb_Address.ZipCode,tb_Address.Fax,tb_country.TwoLetterISOCode FROM tb_Customer inner join tb_address on tb_Customer.ShippingAddressID=tb_Address.AddressId left outer join tb_country on tb_address.Country=tb_country.CountryID  WHERE tb_Customer.CustomerId=" + CustomerId + "");
        //        dsorder = CommonComponent.GetCommonDataSet("select *,ShipCountry.TwoLetterISOCode  from tb_order left outer join tb_country as ShipCountry on tb_order.ShippingCountry=ShipCountry.name  where ordernumber=" + OrderNumber + "");
        //        return dsorder;
        //    }
        //    return dsorder;
        //}
        //[SoapHeader("Authentication", Required = true)]
        //[WebMethod(Description = "Insert microsoftNavLog")]
        //public void InsertMicrosoftNavLog(Int32 RefId, Int32 RefType, Int32 Status)
        //{

        //    if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
        //    {
        //        CommonComponent.ExecuteCommonData("insert into tb_MicrosoftNavLog(RefID,RefType,Status) values (" + RefId + "," + RefType + "," + Status + ") ");

        //    }

        //}

        #region "NAV"
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Table information")]
        public DataSet GetTableInformation(string TableName)
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                dsorder = CommonComponent.GetCommonDataSet("SELECT * FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME='" + TableName.Replace("'", "''") + "'");
                return dsorder;
            }
            return dsorder;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Table Column information")]
        public DataSet GetTablecolumnInformation(string TableName)
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                dsorder = CommonComponent.GetCommonDataSet("SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='" + TableName.Replace("'", "''") + "'");
                return dsorder;
            }
            return dsorder;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Customer information")]
        public DataSet GetCustomerInformation(Int32 OrderNumber)
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                //dsorder = CommonComponent.GetCommonDataSet("SELECT tb_Customer.CustomerID,tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Address.Company,tb_Address.Address1,tb_Address.Address2,tb_Address.City,tb_Address.State,tb_Address.Phone,tb_Address.ZipCode,tb_Address.Fax,tb_country.TwoLetterISOCode FROM tb_Customer inner join tb_address on tb_Customer.BillingAddressID=tb_Address.AddressId left outer join tb_country on tb_address.Country=tb_country.CountryID  WHERE tb_Customer.CustomerId=" + CustomerId + "");
                //  dsorder = CommonComponent.GetCommonDataSet("select *,BillCountry.TwoLetterISOCode  from tb_order left outer join tb_country as BillCountry on tb_order.BillingCountry=BillCountry.name  where ordernumber=" + OrderNumber + "");
                dsorder = CommonComponent.GetCommonDataSet("EXEC usp_MicrosoftNav_GetCustomerinformation " + OrderNumber + "");
                return dsorder;
            }
            return dsorder;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Customer Ship Information")]
        public DataSet GetCustomerShipInformation(Int32 OrderNumber)
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                // dsorder = CommonComponent.GetCommonDataSet("SELECT tb_Customer.CustomerID,tb_Address.Email,tb_Address.FirstName,tb_Address.LastName,tb_Address.Company,tb_Address.Address1,tb_Address.Address2,tb_Address.City,tb_Address.State,tb_Address.Phone,tb_Address.ZipCode,tb_Address.Fax,tb_country.TwoLetterISOCode FROM tb_Customer inner join tb_address on tb_Customer.ShippingAddressID=tb_Address.AddressId left outer join tb_country on tb_address.Country=tb_country.CountryID  WHERE tb_Customer.CustomerId=" + CustomerId + "");
                //  dsorder = CommonComponent.GetCommonDataSet("select *,ShipCountry.TwoLetterISOCode  from tb_order left outer join tb_country as ShipCountry on tb_order.ShippingCountry=ShipCountry.name  where ordernumber=" + OrderNumber + "");
                dsorder = CommonComponent.GetCommonDataSet("EXEC usp_MicrosoftNav_GetCustomerShipInformation " + OrderNumber + "");
                return dsorder;
            }
            return dsorder;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Insert microsoftNavLog")]
        public void InsertMicrosoftNavLog(Int32 RefId, Int32 RefType, Int32 Status)
        {

            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                CommonComponent.ExecuteCommonData("insert into tb_MicrosoftNavLog(RefID,RefType,Status) values (" + RefId + "," + RefType + "," + Status + ") ");

            }

        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Order information")]
        public DataSet GetOrderInformation(Int32 OrderNumber)
        {
            DataSet dsorder = new DataSet();
            DataSet dsCreditdetail = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                //dsorder = CommonComponent.GetCommonDataSet("SELECT tb_Customer.CustomerID,tb_Customer.Email,tb_Customer.FirstName,tb_Customer.LastName,tb_Address.Company,tb_Address.Address1,tb_Address.Address2,tb_Address.City,tb_Address.State,tb_Address.Phone,tb_Address.ZipCode,tb_Address.Fax,tb_country.TwoLetterISOCode FROM tb_Customer inner join tb_address on tb_Customer.BillingAddressID=tb_Address.AddressId left outer join tb_country on tb_address.Country=tb_country.CountryID  WHERE tb_Customer.CustomerId=" + CustomerId + "");
                // dsorder = CommonComponent.GetCommonDataSet("select CustomerID,OrderNumber,isnull(ShippingZip,'') as ShippingZip,isnull(ShippingFirstName,'') as ShippingFirstName ,isnull(ShippingLastName,'') as ShippingLastName,isnull(ShippingAddress1,'') as ShippingAddress1,isnull(ShippingAddress2,'') as ShippingAddress2 ,isnull(ShippingCity,'') as ShippingCity,OrderDate,case when isnull(OrderNotes,'')<>'' then 'TRUE' else 'FALSE' end as OrderNotes,isnull(OrderSubtotal,0) as OrderSubtotal ,sum(isnull(OrderTax,0)+OrderSubtotal) over() as OrderTax,isnull(BillingFirstName,'') as BillingFirstName,isnull(BillingLastName,'') as BillingLastName,isnull(BillingAddress1,'') as BillingAddress1,isnull(BillingAddress2,'') as BillingAddress2,isnull(BillingCity,'') as BillingCity,isnull(BillingZip,'') as BillingZip,isnull(BillingPhone,'') as BillingPhone,isnull(BillingState,'') as BillingState,isnull(ShippingState,'') as ShippingState,isnull(BillingEmail,'') as BillingEmail,isnull(CardName,'') as CardName ,isnull(RefOrderID,'') as RefOrderID,isnull(LastName,'') as LastName,isnull(FirstName,'') as FirstName,isnull(ShippedVIA,'') as ShippedVIA,isnull(CardNumber,'') as CardNumber,isnull(CardExpirationMonth,'') as CardExpirationMonth,case when len(isnull(CardExpirationYear,''))>=4 then substring(3,2) else isnull(CardExpirationYear,'') end  as CardExpirationYear,isnull(CardType,'') as CardType,isnull(CardVarificationCode,'') as CardVarificationCode,isnull(ShipCountry.TwoLetterISOCode,'') as ShippingCountry,isnull(BillCountry.TwoLetterISOCode,'') as  BillingCountry from tb_order left outer join tb_country as ShipCountry on tb_order.ShippingCountry=ShipCountry.name left outer join  tb_country as BillCountry on tb_order.BillingCountry=BillCountry.name where OrderNumber=" + OrderNumber + "");
                dsorder = CommonComponent.GetCommonDataSet("EXEC usp_MicrosoftNav_Orderinformation " + OrderNumber + "");
                if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
                {
                    dsCreditdetail = CommonComponent.GetCommonDataSet("select Top 1 CardNumber,CardVarificationCode from tb_EncryptcardDetail where OrderNumber=" + OrderNumber + " order by CrdDetailId Desc ");
                    if (dsCreditdetail != null && dsCreditdetail.Tables.Count > 0 && dsCreditdetail.Tables[0].Rows.Count > 0)
                    {
                        if (!String.IsNullOrEmpty(dsCreditdetail.Tables[0].Rows[0]["CardNumber"].ToString()))
                        {
                            dsorder.Tables[0].Rows[0]["CardNumber"] = SecurityComponent.Decrypt(dsCreditdetail.Tables[0].Rows[0]["CardNumber"].ToString());
                            dsorder.Tables[0].AcceptChanges();
                        }
                        else if (!String.IsNullOrEmpty(dsorder.Tables[0].Rows[0]["CardNumber"].ToString()))
                        {
                            dsorder.Tables[0].Rows[0]["CardNumber"] = SecurityComponent.Decrypt(dsorder.Tables[0].Rows[0]["CardNumber"].ToString());
                        }
                        if (!String.IsNullOrEmpty(dsCreditdetail.Tables[0].Rows[0]["CardVarificationCode"].ToString()))
                        {
                            dsorder.Tables[0].Rows[0]["CardVarificationCode"] = dsCreditdetail.Tables[0].Rows[0]["CardVarificationCode"].ToString();
                            dsorder.Tables[0].AcceptChanges();
                        }
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(dsorder.Tables[0].Rows[0]["CardNumber"].ToString()))
                        {
                            dsorder.Tables[0].Rows[0]["CardNumber"] = SecurityComponent.Decrypt(dsorder.Tables[0].Rows[0]["CardNumber"].ToString());
                        }
                    }
                }

                return dsorder;
            }
            return dsorder;
        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get ESalesLine information")]
        public DataSet GetESalesLineInformation(Int32 OrderNumber)
        {
            DataSet dsESalesLine = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                dsESalesLine = CommonComponent.GetCommonDataSet("EXEC usp_MicrosoftNAV_ESalesLine " + OrderNumber + "");
                return dsESalesLine;
            }
            return dsESalesLine;
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Order information")]
        public DataSet GetAllOrderstoInsertIntoNAV()
        {
            DataSet dsOrders = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                dsOrders = CommonComponent.GetCommonDataSet("select OrderNumber,CustomerID  from tb_Order where isnull(isnavinserted,0)=0 and isnull(IsNavError,0)=0 and ISNULL(Deleted,0)=0 and Year(OrderDate) >=2016");
                return dsOrders;
            }
            return dsOrders;

        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Order information")]
        public void UpdateNAVFlag(int OrderNumber)
        {
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                CommonComponent.ExecuteCommonData("update tb_order set isnavinserted=1,isnavcompleted=2,NAVError='',IsNavError=0 where ordernumber=" + OrderNumber + "");
                CommonComponent.ExecuteCommonData("EXEC usp_VendorPortal_Order_QuantityAdjustNAV " + OrderNumber + "");
            }

        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update Line Number information")]
        public void UpdateLineNo(int LineNo, int OrderedCustomCartID)
        {
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                CommonComponent.ExecuteCommonData("update tb_OrderedShoppingCartItems set [LineNo]=" + LineNo + " where OrderedCustomCartID=" + OrderedCustomCartID + "");
            }
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update Order Tracking Number information")]
        public void UpdateTrackingNumber(Int32 OrderNumber, String TrackingNumber, String Shippingvia, String ShipedOn, String SKu, Int32 Quantity)
        {
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                CommonComponent.ExecuteCommonData("EXEC usp_MicrosoftNav_PackageHeader " + OrderNumber + ",'" + TrackingNumber + "','" + Shippingvia + "','','" + ShipedOn.ToString() + "','" + SKu.Trim() + "'," + Quantity + "");

            }


        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update ProductInventory information")]
        public void UpdateProductInventoryFromNav(string SKU, int Inventory, int InventoryOnHand, Int32 ATLQty, Int32 LIVQty)
        {
            CommonComponent.ExecuteCommonData("EXEC usp_MicrosoftNav_InventoryList '" + SKU + "'," + Inventory + "," + InventoryOnHand + "," + ATLQty + "," + LIVQty + "");
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update ProductInventory information")]
        public void UpdateProductInventoryFromNavBulk(string SKU, int Inventory, int InventoryOnHand, Int32 ATLQty, Int32 LIVQty, Int32 ATLBULK)
        {
            CommonComponent.ExecuteCommonData("EXEC usp_MicrosoftNav_InventoryList '" + SKU + "'," + Inventory + "," + InventoryOnHand + "," + ATLQty + "," + LIVQty + "," + ATLBULK + "");
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Write Error Sku not found in NAV")]
        public void InsertErrorNAVSku(string SKU, int OrderNumber)
        {
            CommonComponent.ExecuteCommonData("update tb_order set IsNavError=1,NAVError='" + SKU + "',isNAVInserted=1,isnavcompleted=0 where ordernumber=" + OrderNumber + "");
        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Inventory Flag")]
        public DataSet NavGetInventoryFlag()
        {
            DataSet dsInventory = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                dsInventory = CommonComponent.GetCommonDataSet("SELECT * FROM tb_NavInventoryFlag");
                return dsInventory;
            }
            return dsInventory;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update Inventory Flag")]
        public void NavUpdateInventoryFlag(Int32 AdminId)
        {

            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                CommonComponent.ExecuteCommonData("update tb_NavInventoryFlag SET Flag=0");
                CommonComponent.ExecuteCommonData("INSERT INTO tb_NavInventoryDetail(UpdatedBy) VALUES (" + AdminId + ")");

            }

        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update Query")]
        public void NavUpdateQuery(string Query)
        {

            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                CommonComponent.ExecuteCommonData(Query);

            }

        }


        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get Order For Shipp")]
        public DataSet GetOrderDetailsForShippingNAV()
        {
            DataSet dsorder = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {
                //dsorder = CommonComponent.GetCommonDataSet("SELECT reforderid as BackEndGUID,OrderNumber,StoreId FROM tb_order WHERE Storeid=4 AND (datepart(MONTH,dbo.tb_Order.Orderdate) >=datepart(MONTH,getdate()) Or datepart(MONTH,dbo.tb_Order.Orderdate) >=11) AND datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,getdate()) AND isnull(Deleted,0)=0 AND Isnull(IsBackShip,0)=0 ");
                //dsorder = CommonComponent.GetCommonDataSet("SELECT reforderid as BackEndGUID,OrderNumber,StoreId,OrderDate FROM tb_order WHERE isnull(OrderStatus,'')<>'Shipped' and Storeid in (4) AND ((datepart(MONTH,dbo.tb_Order.Orderdate) >=datepart(MONTH,getdate()) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate()))) or  (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year, Getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-2, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))) and isnull(OrderStatus,'')<>'Shipped' AND isnull(Deleted,0)=0 AND Isnull(Isnavinserted,0)=1 and isnull(isnavcompleted,0)=2 UNION ALL SELECT cast(OrderNumber as nvarchar(20)) as BackEndGUID,OrderNumber,StoreId,OrderDate FROM tb_order WHERE Storeid in (1) AND ((datepart(MONTH,dbo.tb_Order.Orderdate) >=datepart(MONTH,getdate()) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))  OR (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year, Getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-2, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))) and isnull(OrderStatus,'')<>'Shipped' AND isnull(Deleted,0)=0 AND Isnull(Isnavinserted,0)=1 and isnull(isnavcompleted,0)=2 UNION ALL SELECT refOrderId as BackEndGUID,OrderNumber,StoreId,OrderDate FROM tb_order WHERE Storeid in (3) AND ((datepart(MONTH,dbo.tb_Order.Orderdate) >=datepart(MONTH,getdate()) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))  OR (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-1, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year, Getdate())) Or (datepart(MONTH,dbo.tb_Order.Orderdate) =datepart(MONTH,DATEADD(month,-2, Getdate())) and datepart(Year,dbo.tb_Order.Orderdate)=datepart(Year,DATEADD(year,-1, Getdate())))) and isnull(OrderStatus,'')<>'Shipped' AND isnull(Deleted,0)=0 AND Isnull(Isnavinserted,0)=1 and isnull(isnavcompleted,0)=2");
                dsorder = CommonComponent.GetCommonDataSet("Exec usp_MicrosoftNAV_GetOrderDetailsForShippingNAV");
                return dsorder;

            }
            return dsorder;
        }
        #endregion



        #region "Inventory Sync manual"
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Get XML Files")]
        public Int32 GetInventorySyncXMLFiles()
        {
            Int32 Isvalid = 0;
            try
            {
                string[] strFiles = { "" };
                if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
                {
                    strFiles = System.IO.Directory.GetFiles(Server.MapPath("/InventoryFiles/"));

                    foreach (string strfile in strFiles)
                    {
                        if (File.Exists(strfile))
                        {
                            DataSet dsXML = new DataSet();
                            dsXML.ReadXml(strfile);
                            if (dsXML != null && dsXML.Tables.Count > 0 && dsXML.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dsXML.Tables[0].Rows[0]["IsInventory"].ToString()) && dsXML.Tables[0].Rows[0]["IsInventory"].ToString() == "1")
                                {
                                    Isvalid = 1;
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }
            return Isvalid;

        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "Update XML Files")]
        public void UpdateInventorySyncXMLFiles()
        {
            try
            {
                string[] strFiles = { "" };
                if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
                {
                    strFiles = System.IO.Directory.GetFiles(Server.MapPath("/InventoryFiles/"));

                    foreach (string strfile in strFiles)
                    {
                        if (File.Exists(strfile))
                        {
                            DataSet dsXML = new DataSet();
                            dsXML.ReadXml(strfile);
                            if (dsXML != null && dsXML.Tables.Count > 0 && dsXML.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dsXML.Tables[0].Rows[0]["IsInventory"].ToString()) && dsXML.Tables[0].Rows[0]["IsInventory"].ToString() == "1")
                                {

                                    dsXML.Tables[0].Rows[0]["IsInventory"] = 0;
                                    dsXML.AcceptChanges();
                                    dsXML.WriteXml(strfile);
                                }
                            }
                        }
                    }
                }
            }
            catch
            {

            }


        }

        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "GET SKU")]
        public DataSet GetInventorySyncSKU()
        {
            DataSet dsInv = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {

                dsInv = CommonComponent.GetCommonDataSet("SELECT ''''+ UPC +''',' FROM tb_InventorySync WHERE isnull(UPC,'')<>'' and Isnull(IsActive,0)=0 FOR XML PATH('')");
                return dsInv;
            }
            return dsInv;
        }
        [SoapHeader("Authentication", Required = true)]
        [WebMethod(Description = "GET SKU")]
        public void UpdateInventorySyncSKU()
        {
            DataSet dsInv = new DataSet();
            if (Authentication.Username == "admin@hpd.com" && Authentication.Password == "HPD#2703")
            {

                CommonComponent.ExecuteCommonData("UPDATE tb_InventorySync SET IsActive=1");

            }

        }

        #endregion

        public class AuthHeader : SoapHeader
        {
            public string Username;
            public string Password;
        }
    }
}
