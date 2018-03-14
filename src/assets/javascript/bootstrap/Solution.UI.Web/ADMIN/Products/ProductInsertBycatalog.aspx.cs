using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Solution.Data;
using Solution.Bussines.Components;
using System.Data;
using System.Xml;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductInsertBycatalog : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Response.Write("PTCH-JTSP003-FB".Substring(0, "PTCH-JTSP003-FB".ToString().IndexOf("-")));
        }
        protected void btnAddProduct1_Click(object sender, EventArgs e)
        {
            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            // dsAll = objSql.GetDs("SELECT DISTINCT [Product Name],[Item Categogy Global Dimension] FROM MASTERData_new WHERE [Main Category] in ('Designer Silk Fabric Curtains','Designer Faux Silk Fabric Curtains','Solid Silk Fabric Curtains') AND [Product URLS] like '%http://%' and [Product URLS] not in (SELECT 'http://www.halfpricedrapes.com/'+ ProductUrl FROM tb_product WHERE Isnull(Active,0)=1 AND isnull(Deleted,0)=0 AND Storeid=1)   Order By [Product Name]");
            dsAll = objSql.GetDs("SELECT DISTINCT [Product Name],[Item Categogy Global Dimension] FROM MASTERData_new WHERE [Main Category] in ('Designer Silk Fabric Curtains','Designer Faux Silk Fabric Curtains','Solid Silk Fabric Curtains') AND [Product URLS] like '%http://%' and [Product URLS] not in (SELECT 'http://www.halfpricedrapes.com/'+ ProductUrl FROM tb_product WHERE Isnull(Active,0)=1 AND isnull(Deleted,0)=0 AND Storeid=1)   Order By [Product Name]");

            //            dsAll = objSql.GetDs(@"SELECT DISTINCT [Product Name],isnull([Item Categogy Global Dimension],'Swatch') as [Item Categogy Global Dimension] FROM MASTERData_New WHERE [Product SKU] in (
            // SELECT Code FROM HPDMaster_20Dec WHERE Code NOT IN (
            // SELECT  SKU FROM tb_product INNER JOIN tb_productCAtegory ON tb_product.ProductID=tb_productCAtegory.ProductID
            // WHERE tb_product.SKU in (SELECT Code FROM HPDMaster_20Dec) and Isnull(active,0)=1 ANd isnull(Deleted,0)=0 and StoreId=1))");

            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {
                Response.Write(dsAll.Tables[0].Rows.Count.ToString());
                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    //if (string.IsNullOrEmpty(dsAll.Tables[0].Rows[i]["Item Categogy Global Dimension"].ToString()))
                    //{
                    //    dsAll.Tables[0].Rows[i]["Item Categogy Global Dimension"] = "Swatch";
                    //}
                    string strtag = "";
                    string strInsert = "0";
                    if (dsAll.Tables[0].Rows[i]["Item Categogy Global Dimension"].ToString().ToLower() == "drape")
                    {

                        DataSet dsAllproduct = new DataSet();

                        dsAllproduct = objSql.GetDs("SELECT * FROM MASTERData_new WHERE [Item Categogy Global Dimension]='" + dsAll.Tables[0].Rows[i]["Item Categogy Global Dimension"].ToString().Replace("'", "''") + "' AND [Product Name]='" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "'");
                        if (dsAllproduct != null && dsAllproduct.Tables[0].Rows.Count > 0)
                        {
                            if (dsAllproduct.Tables[0].Rows[0]["UOM"].ToString().ToLower().IndexOf("each") > -1)
                            {
                                strtag = "Per Panel";
                            }
                            else if (dsAllproduct.Tables[0].Rows[0]["UOM"].ToString().ToLower().IndexOf("pair") > -1)
                            {
                                strtag = "Per Pair";
                            }
                            string sku = Convert.ToString(dsAllproduct.Tables[0].Rows[0]["Product SKU"].ToString());
                            sku = sku.Replace("-84-", "-").Replace("-96-", "-").Replace("-108-", "-").Replace("-120-", "-");
                            sku = sku.Replace("-84", "").Replace("-96", "").Replace("-108", "").Replace("-120", "");

                            string ydprivacy = "0", ydlightcontrol = "0", ydefficiency = "0";
                            ydprivacy = Convert.ToString(dsAllproduct.Tables[0].Rows[0]["ydprivacy"].ToString());
                            ydlightcontrol = Convert.ToString(dsAllproduct.Tables[0].Rows[0]["ydlightcontrol"].ToString());
                            ydefficiency = Convert.ToString(dsAllproduct.Tables[0].Rows[0]["ydefficiency"].ToString());
                            Int32 isproperty = 0;
                            if (ydprivacy.ToString() != "0" && ydprivacy.ToString() != "")
                            {
                                isproperty = 1;
                            }



                            //strInsert = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOp 1 Productid FROM tb_product WHERE isnull(Active,0)=1 AND isnull(deleted,0)=0 AND UPC='" + dsAllproduct.Tables[0].Rows[0]["UPC"].ToString() + "' AND Storeid=1"));
                            if (dsAllproduct.Tables[0].Rows[0]["Main Category"].ToString().ToLower().IndexOf("roman ") > -1)
                            {
                                //objSql.ExecuteNonQuery("UPDATE tb_product SET   price='" + dsAllproduct.Tables[0].Rows[0]["price"].ToString().Replace("'", "''") + "',Saleprice='" + dsAllproduct.Tables[0].Rows[0]["Saleprice"].ToString().Replace("'", "''") + "',Weight=" + dsAllproduct.Tables[0].Rows[0]["shipweight"].ToString().Replace("'", "''") + ",ShippingTime='" + dsAllproduct.Tables[0].Rows[0]["productshippingtime"].ToString().Replace("'", "''") + "',Description='" + dsAllproduct.Tables[0].Rows[0]["Description"].ToString().Replace("'", "''") + "',Features='" + dsAllproduct.Tables[0].Rows[0]["ydproductfeatures"].ToString().Replace("'", "''") + "', Isproperty=" + isproperty + ",Privacy=" + ydprivacy + ",Efficiency=" + ydefficiency + ",LightControl=" + ydlightcontrol + " WHERE isnull(Active,0)=1 AND isnull(deleted,0)=0 AND UPC='" + dsAllproduct.Tables[0].Rows[0]["UPC"].ToString() + "' AND Storeid=1");
                                strInsert = Convert.ToString(objSql.ExecuteScalarQuery("INSERT INTO  tb_product (storeid,Active,Deleted,CreatedOn,Name,ShortName,SKU,Inventory,Weight,OptionSku,UPC,price,Saleprice,Description,Features,ManufactureID,Condition,ProductCondition,IsCustom,Header,Ismadetoorder,Pattern,Colors,ShippingTime,FabricType,FabricCode,Isproperty,Privacy,Efficiency,LightControl,ProductURL,IsRoman,SalePriceTag) " +
                              " VALUES(1,1,0,getdate(),'" + dsAllproduct.Tables[0].Rows[0]["Product Name"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Product Name"].ToString().Replace("'", "''") + "','" + sku.ToString() + "',10," + dsAllproduct.Tables[0].Rows[0]["shipweight"].ToString().Replace("'", "''") + ",'','" + dsAllproduct.Tables[0].Rows[0]["UPC"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["price"].ToString() + "','" + dsAllproduct.Tables[0].Rows[0]["saleprice"].ToString() + "','" + dsAllproduct.Tables[0].Rows[0]["Description"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["ydproductfeatures"].ToString().Replace("'", "''") + "',1,'New','New',1,'" + dsAllproduct.Tables[0].Rows[0]["Header"].ToString().Replace("'", "''") + "',1,'" + dsAllproduct.Tables[0].Rows[0]["Pattern"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Color"].ToString().Replace("'", "''") + ",','" + dsAllproduct.Tables[0].Rows[0]["productshippingtime"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Fabric Care"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Fabric Care"].ToString().Replace("'", "''") + "'," + isproperty + "," + ydprivacy + "," + ydefficiency + "," + ydlightcontrol + ",'" + dsAllproduct.Tables[0].Rows[0]["Product URLS"].ToString().Replace("'", "''").Replace("http://www.halfpricedrapes.com/", "") + "',1,'" + strtag + "'); SELECT SCOPE_IDENTITY();"));
                            }
                            else
                            {
                                //objSql.ExecuteNonQuery("UPDATE tb_product SET  ispriceQuote=1, price='" + dsAllproduct.Tables[0].Rows[0]["price"].ToString().Replace("'", "''") + "',Saleprice='" + dsAllproduct.Tables[0].Rows[0]["Saleprice"].ToString().Replace("'", "''") + "',Weight=" + dsAllproduct.Tables[0].Rows[0]["shipweight"].ToString().Replace("'", "''") + ",ShippingTime='" + dsAllproduct.Tables[0].Rows[0]["productshippingtime"].ToString().Replace("'", "''") + "',Description='" + dsAllproduct.Tables[0].Rows[0]["Description"].ToString().Replace("'", "''") + "',Features='" + dsAllproduct.Tables[0].Rows[0]["ydproductfeatures"].ToString().Replace("'", "''") + "', Isproperty=" + isproperty + ",Privacy=" + ydprivacy + ",Efficiency=" + ydefficiency + ",LightControl=" + ydlightcontrol + " WHERE isnull(Active,0)=1 AND isnull(deleted,0)=0 AND UPC='" + dsAllproduct.Tables[0].Rows[0]["UPC"].ToString() + "' AND Storeid=1");
                                //objSql.ExecuteNonQuery("UPDATE tb_product SET price='" + dsAllproduct.Tables[0].Rows[0]["price"].ToString().Replace("'", "''") + "',Saleprice='" + dsAllproduct.Tables[0].Rows[0]["Saleprice"].ToString().Replace("'", "''") + "',Description='" + dsAllproduct.Tables[0].Rows[0]["Description"].ToString().Replace("'", "''") + "',Features='" + dsAllproduct.Tables[0].Rows[0]["ydproductfeatures"].ToString().Replace("'", "''") + "', Isproperty=" + isproperty + ",Privacy=" + ydprivacy + ",Efficiency=" + ydefficiency + ",LightControl=" + ydlightcontrol + " WHERE isnull(Active,0)=1 AND isnull(deleted,0)=0 AND UPC='" + dsAllproduct.Tables[0].Rows[0]["UPC"].ToString() + "' AND Storeid=1");
                                strInsert = Convert.ToString(objSql.ExecuteScalarQuery("INSERT INTO  tb_product (storeid,Active,Deleted,CreatedOn,Name,ShortName,SKU,Inventory,Weight,OptionSku,UPC,price,Saleprice,Description,Features,ManufactureID,Condition,ProductCondition,IsCustom,Header,Ismadetoorder,Ismadetomeasure,Pattern,Colors,ShippingTime,FabricType,FabricCode,Isproperty,Privacy,Efficiency,LightControl,ProductURL,SalePriceTag) " +
                                " VALUES(1,1,0,getdate(),'" + dsAllproduct.Tables[0].Rows[0]["Product Name"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Product Name"].ToString().Replace("'", "''") + "','" + sku.ToString() + "',10," + dsAllproduct.Tables[0].Rows[0]["shipweight"].ToString().Replace("'", "''") + ",'','" + dsAllproduct.Tables[0].Rows[0]["UPC"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["price"].ToString() + "','" + dsAllproduct.Tables[0].Rows[0]["saleprice"].ToString() + "','" + dsAllproduct.Tables[0].Rows[0]["Description"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["ydproductfeatures"].ToString().Replace("'", "''") + "',1,'New','New',1,'" + dsAllproduct.Tables[0].Rows[0]["Header"].ToString().Replace("'", "''") + "',1,1,'" + dsAllproduct.Tables[0].Rows[0]["Pattern"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Color"].ToString().Replace("'", "''") + ",','" + dsAllproduct.Tables[0].Rows[0]["productshippingtime"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Fabric Care"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Fabric Care"].ToString().Replace("'", "''") + "'," + isproperty + "," + ydprivacy + "," + ydefficiency + "," + ydlightcontrol + ",'" + dsAllproduct.Tables[0].Rows[0]["Product URLS"].ToString().Replace("'", "''").Replace("http://www.halfpricedrapes.com/", "") + "','" + strtag + "'); SELECT SCOPE_IDENTITY();"));
                            }
                            objSql.ExecuteNonQuery("DELETE FROM tb_WareHouseProductInventory WHERE ProductId=" + strInsert + "");
                            if (dsAllproduct.Tables[0].Rows[0]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                            {
                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (1," + strInsert + ",10,1)");
                            }
                            else
                            {
                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (15," + strInsert + ",10,1)");
                            }
                            strInsert = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 ProductId FROM tb_product WHERE SKu='" + sku.ToString().Replace("'", "''") + "' AND Storeid=1 order By ProductId DESC"));
                            objSql.ExecuteNonQuery("DELETE FROM tb_ProductCategory WHERE ProductId=" + strInsert + "");
                            Int32 Categoryid = 0;
                            if (dsAllproduct.Tables[0].Rows[0]["Sub Category"].ToString().ToLower().IndexOf("signature sheer curtains") > -1)
                            {
                                Categoryid = 1697;
                            }
                            else
                            {
                                Categoryid = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT TOP 1 CategoryID FROM tb_Category WHERE Storeid=1 AND isnull(Active,0)=1 AND isnull(Deleted,0)=0 AND Name like '%" + dsAllproduct.Tables[0].Rows[0]["Sub Category"].ToString().ToLower().Replace("'", "''").Replace(" fabric", "") + "%'"));
                            }
                            objSql.ExecuteNonQuery("INSERT INTO tb_ProductCategory(CategoryID,ProductID) VALUES (" + Categoryid + "," + strInsert + ")");
                            objSql.ExecuteNonQuery("Update tb_Product SET Active=1,deleted=0 WHERE productId=" + strInsert + "");
                        }
                        //dsAllproduct = objSql.GetDs("SELECT * FROM MASTERData WHERE [Item Categogy Global Dimension]='" + dsAll.Tables[0].Rows[i]["Item Categogy Global Dimension"].ToString().Replace("'", "''") + "' AND isnull(Size,'') <> '' AND [Product Name]='" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "'");
                        if (dsAllproduct != null && dsAllproduct.Tables[0].Rows.Count > 1 && strInsert != "0")
                        {
                            string str1 = "";
                            string str2 = "";
                            string str3 = "";
                            string str4 = "";
                            objSql.ExecuteNonQuery("DELETE FROM tb_WareHouseProductVariantInventory WHERE Productid =" + strInsert + "");
                            objSql.ExecuteNonQuery("DELETE FROM tb_ProductVariantValue WHERE Productid =" + strInsert + "");
                            objSql.ExecuteNonQuery("DELETE FROM tb_ProductVariant WHERE Productid =" + strInsert + "");
                            if (dsAllproduct != null && dsAllproduct.Tables[0].Rows.Count == 4)
                            {

                                Int32 pvariantId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariant(ProductID, VariantName,IsParent, ParentId, DisplayOrder) VALUES (" + strInsert + ",'Select Top Header Design',1,0,1);  SELECT SCOPE_IDENTITY();"));


                                Int32 PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                     " values (" + pvariantId + ",'Pole Pocket','0',1," + strInsert + ",'','','',0,0,0,0,0,0); SELECT SCOPE_IDENTITY();"));

                                pvariantId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariant(ProductID, VariantName,IsParent, ParentId, DisplayOrder) VALUES (" + strInsert + ",'Select Size',0," + PvalueId.ToString() + ",1);  SELECT SCOPE_IDENTITY();"));

                                for (int j = 0; j < dsAllproduct.Tables[0].Rows.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x") > -1)
                                    {
                                        str1 = "50Wx84L Pole Pocket";
                                        str2 = "50Wx96L Pole Pocket";
                                        str3 = "50Wx108L Pole Pocket";
                                        str4 = "50Wx120L Pole Pocket";
                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 108") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                               " values (" + pvariantId + ",'" + str3 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',3," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 120") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str4 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',4," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }


                                    }
                                    else if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x") > -1)
                                    {
                                        str1 = "25Wx84L Pole Pocket";
                                        str2 = "25Wx96L Pole Pocket";
                                        str3 = "25Wx108L Pole Pocket";
                                        str4 = "25Wx120L Pole Pocket";
                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 108") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                               " values (" + pvariantId + ",'" + str3 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',3," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 120") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str4 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',4," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }


                                    }
                                    else if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x") > -1)
                                    {
                                        str1 = "100Wx84L Pole Pocket";
                                        str2 = "100Wx96L Pole Pocket";
                                        str3 = "100Wx108L Pole Pocket";
                                        str4 = "100Wx120L Pole Pocket";
                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 108") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str3 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',3," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 120") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str4 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',4," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }


                                    }
                                }
                                PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'Custom Size','0',5," + strInsert + ",'','','',0,0,0,0,0,0); SELECT SCOPE_IDENTITY();"));




                            }
                            else if (dsAllproduct != null && dsAllproduct.Tables[0].Rows.Count == 3)
                            {
                                Int32 pvariantId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariant(ProductID, VariantName,IsParent, ParentId, DisplayOrder) VALUES (" + strInsert + ",'Select Top Header Design',1,0,1);  SELECT SCOPE_IDENTITY();"));


                                Int32 PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                     " values (" + pvariantId + ",'Pole Pocket','0',1," + strInsert + ",'','','',0,0,0,0,0,0); SELECT SCOPE_IDENTITY();"));

                                pvariantId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariant(ProductID, VariantName,IsParent, ParentId, DisplayOrder) VALUES (" + strInsert + ",'Select Size',0," + PvalueId.ToString() + ",1); SELECT SCOPE_IDENTITY();"));

                                for (int j = 0; j < dsAllproduct.Tables[0].Rows.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x") > -1)
                                    {
                                        str1 = "50Wx84L Pole Pocket";
                                        str2 = "50Wx96L Pole Pocket";
                                        str3 = "50Wx108L Pole Pocket";

                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 108") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                               " values (" + pvariantId + ",'" + str3 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',3," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }


                                    }
                                    else if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x") > -1)
                                    {
                                        str1 = "25Wx84L Pole Pocket";
                                        str2 = "25Wx96L Pole Pocket";
                                        str3 = "25Wx108L Pole Pocket";
                                        str4 = "25Wx120L Pole Pocket";
                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 108") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                               " values (" + pvariantId + ",'" + str3 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',3," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }



                                    }
                                    else if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x") > -1)
                                    {
                                        str1 = "100Wx84L Pole Pocket";
                                        str2 = "100Wx96L Pole Pocket";
                                        str3 = "100Wx108L Pole Pocket";

                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));
                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));
                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 108") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str3 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',3," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }

                                    }
                                }
                                PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'Custom Size','0',4," + strInsert + ",'','','',0,0,0,0,0,0); SELECT SCOPE_IDENTITY();"));

                            }
                            else if (dsAllproduct != null && dsAllproduct.Tables[0].Rows.Count == 2)
                            {
                                Int32 pvariantId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariant(ProductID, VariantName,IsParent, ParentId, DisplayOrder) VALUES (" + strInsert + ",'Select Top Header Design',1,0,1);  SELECT SCOPE_IDENTITY();"));


                                Int32 PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                     " values (" + pvariantId + ",'Pole Pocket','0',1," + strInsert + ",'','','',0,0,0,0,0,0); SELECT SCOPE_IDENTITY();"));

                                pvariantId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariant(ProductID, VariantName,IsParent, ParentId, DisplayOrder) VALUES (" + strInsert + ",'Select Size',0," + PvalueId.ToString() + ",1)"));

                                for (int j = 0; j < dsAllproduct.Tables[0].Rows.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x") > -1)
                                    {
                                        str1 = "50Wx84L Pole Pocket";
                                        str2 = "50Wx96L Pole Pocket";

                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("50 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }



                                    }
                                    else if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x") > -1)
                                    {
                                        str1 = "25Wx84L Pole Pocket";
                                        str2 = "25Wx96L Pole Pocket";
                                        str3 = "25Wx108L Pole Pocket";
                                        str4 = "25Wx120L Pole Pocket";
                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));


                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("25 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }

                                    }
                                    else if (!string.IsNullOrEmpty(dsAllproduct.Tables[0].Rows[j]["Size"].ToString()) && dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x") > -1)
                                    {
                                        str1 = "100Wx84L Pole Pocket";
                                        str2 = "100Wx96L Pole Pocket";

                                        if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 84") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str1 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',1," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }
                                        else if (dsAllproduct.Tables[0].Rows[j]["Size"].ToString().ToLower().IndexOf("100 x 96") > -1)
                                        {
                                            PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                             " values (" + pvariantId + ",'" + str2 + "','" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "',2," + strInsert + ",'" + dsAllproduct.Tables[0].Rows[j]["Product SKU"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["UPC"].ToString() + "','" + dsAllproduct.Tables[0].Rows[j]["Header"].ToString() + "',10,0,10,0,0,'" + dsAllproduct.Tables[0].Rows[j]["Saleprice"].ToString() + "'); SELECT SCOPE_IDENTITY();"));

                                            if (dsAllproduct.Tables[0].Rows[j]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                                            {

                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }
                                            else
                                            {
                                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (15," + strInsert + "," + pvariantId + "," + PvalueId + ",10,1)");
                                            }

                                        }


                                    }
                                }
                                PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                               " values (" + pvariantId + ",'Custom Size','0',3," + strInsert + ",'','','',0,0,0,0,0,0); SELECT SCOPE_IDENTITY();"));
                            }
                        }


                    }
                    else if (dsAll.Tables[0].Rows[i]["Item Categogy Global Dimension"].ToString().ToLower() == "swatch")
                    {
                        DataSet dsAllproduct = new DataSet();

                        dsAllproduct = objSql.GetDs("SELECT * FROM MASTERData_New WHERE [Item Categogy Global Dimension]='" + dsAll.Tables[0].Rows[i]["Item Categogy Global Dimension"].ToString().Replace("'", "''") + "' AND [Product Name]='" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "'");
                        if (dsAllproduct != null && dsAllproduct.Tables[0].Rows.Count > 0)
                        {
                            if (dsAllproduct.Tables[0].Rows[0]["UOM"].ToString().ToLower().IndexOf("each") > -1)
                            {
                                strtag = "Per Panel";
                            }
                            else if (dsAllproduct.Tables[0].Rows[0]["UOM"].ToString().ToLower().IndexOf("pair") > -1)
                            {
                                strtag = "Per Pair";
                            }
                            string sku = Convert.ToString(dsAllproduct.Tables[0].Rows[0]["Product SKU"].ToString());
                            sku = sku.Replace("-84-", "-").Replace("-96-", "-").Replace("-108-", "-").Replace("-120-", "-");
                            sku = sku.Replace("-84", "").Replace("-96", "").Replace("-108", "").Replace("-120", "");
                            strInsert = "0";
                            string ydprivacy = "0", ydlightcontrol = "0", ydefficiency = "0";
                            ydprivacy = Convert.ToString(dsAllproduct.Tables[0].Rows[0]["ydprivacy"].ToString());
                            ydlightcontrol = Convert.ToString(dsAllproduct.Tables[0].Rows[0]["ydlightcontrol"].ToString());
                            ydefficiency = Convert.ToString(dsAllproduct.Tables[0].Rows[0]["ydefficiency"].ToString());
                            if (ydprivacy == "")
                            {
                                ydprivacy = "0";
                            }
                            if (ydlightcontrol == "")
                            {
                                ydlightcontrol = "0";
                            }
                            if (ydefficiency == "")
                            {
                                ydefficiency = "0";
                            }
                            Int32 isproperty = 0;
                            if (ydprivacy.ToString() != "0" && ydprivacy.ToString() != "")
                            {
                                isproperty = 1;
                            }

                            //objSql.ExecuteNonQuery("UPDATE tb_product SET   price='" + dsAllproduct.Tables[0].Rows[0]["price"].ToString().Replace("'", "''") + "',Saleprice='" + dsAllproduct.Tables[0].Rows[0]["Saleprice"].ToString().Replace("'", "''") + "',Weight=" + dsAllproduct.Tables[0].Rows[0]["shipweight"].ToString().Replace("'", "''") + ",ShippingTime='" + dsAllproduct.Tables[0].Rows[0]["productshippingtime"].ToString().Replace("'", "''") + "',Description='" + dsAllproduct.Tables[0].Rows[0]["Description"].ToString().Replace("'", "''") + "',Features='" + dsAllproduct.Tables[0].Rows[0]["ydproductfeatures"].ToString().Replace("'", "''") + "', Isproperty=" + isproperty + ",Privacy=" + ydprivacy + ",Efficiency=" + ydefficiency + ",LightControl=" + ydlightcontrol + " WHERE isnull(Active,0)=1 AND isnull(deleted,0)=0 AND UPC='" + dsAllproduct.Tables[0].Rows[0]["UPC"].ToString() + "' AND Storeid=1");
                            strInsert = Convert.ToString(objSql.ExecuteScalarQuery("INSERT INTO  tb_product (storeid,Active,Deleted,CreatedOn,Name,ShortName,SKU,Inventory,Weight,OptionSku,UPC,price,Saleprice,Description,Features,ManufactureID,Condition,ProductCondition,IsCustom,Header,Ismadetoorder,Pattern,Colors,ShippingTime,FabricType,FabricCode,Isproperty,Privacy,Efficiency,LightControl,ProductURL,Isfreefabricswatch,SalePriceTag) " +
                                " VALUES(1,1,0,getdate(),'" + dsAllproduct.Tables[0].Rows[0]["Product Name"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Product Name"].ToString().Replace("'", "''") + "','" + sku.ToString() + "',10," + dsAllproduct.Tables[0].Rows[0]["shipweight"].ToString().Replace("'", "''") + ",'','" + dsAllproduct.Tables[0].Rows[0]["UPC"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["price"].ToString() + "','" + dsAllproduct.Tables[0].Rows[0]["saleprice"].ToString() + "','" + dsAllproduct.Tables[0].Rows[0]["Description"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["ydproductfeatures"].ToString().Replace("'", "''") + "',1,'New','New',0,'" + dsAllproduct.Tables[0].Rows[0]["Header"].ToString().Replace("'", "''") + "',0,'" + dsAllproduct.Tables[0].Rows[0]["Pattern"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Color"].ToString().Replace("'", "''") + ",','" + dsAllproduct.Tables[0].Rows[0]["productshippingtime"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Fabric Care"].ToString().Replace("'", "''") + "','" + dsAllproduct.Tables[0].Rows[0]["Fabric Care"].ToString().Replace("'", "''") + "'," + isproperty + "," + ydprivacy + "," + ydefficiency + "," + ydlightcontrol + ",'" + dsAllproduct.Tables[0].Rows[0]["Product URLS"].ToString().Replace("'", "''").Replace("http://www.halfpricedrapes.com/", "") + "',1,'" + strtag + "'); SELECT SCOPE_IDENTITY();"));


                            objSql.ExecuteNonQuery("DELETE FROM tb_WareHouseProductInventory WHERE ProductId=" + strInsert + "");
                            if (dsAllproduct.Tables[0].Rows[0]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                            {
                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (1," + strInsert + ",10,1)");
                            }
                            else
                            {
                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (15," + strInsert + ",10,1)");
                            }

                            strInsert = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 ProductId FROM tb_product WHERE SKU='" + sku.ToString().Replace("'", "''") + "' AND Storeid=1  order By ProductId DESC"));

                            objSql.ExecuteNonQuery("DELETE FROM tb_ProductCategory WHERE ProductId=" + strInsert + "");

                            Int32 Categoryid = 0;
                            if (dsAllproduct.Tables[0].Rows[0]["Sub Category"].ToString().ToLower().IndexOf("signature sheer curtains") > -1)
                            {
                                Categoryid = 1697;
                            }
                            else
                            {
                                Categoryid = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT TOP 1 CategoryID FROM tb_Category WHERE Storeid=1 AND isnull(Active,0)=1 AND isnull(Deleted,0)=0 AND Name like '%" + dsAllproduct.Tables[0].Rows[0]["Sub Category"].ToString().ToLower().Replace("'", "''").Replace(" fabric", "") + "%'"));
                            }
                            objSql.ExecuteNonQuery("INSERT INTO tb_ProductCategory(CategoryID,ProductID) VALUES (" + Categoryid + "," + strInsert + ")");
                            objSql.ExecuteNonQuery("Update tb_Product SET Active=1,deleted=0 WHERE productId=" + strInsert + "");

                            sku = sku.Replace("-SW-", "-");
                            sku = sku.Replace("-SW", "");

                            Int32 PID = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT TOP 1 ProductID FROM tb_product WHERE Storeid=1 And isnull(Active,0)=1 AND isnull(Deleted,0)=0 AND SKU='" + sku + "' order By ProductID DESC"));
                            objSql.ExecuteNonQuery("UPDATE tb_product SET ProductSwatchid=" + strInsert + " WHERE ProductID=" + PID + "");
                            //ProductSwatchid

                        }
                    }
                    else
                    {

                    }



                }
            }
        }
        protected void btnAddProduct0_Click(object sender, EventArgs e)
        {
            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            // dsAll = objSql.GetDs("SELECT * FROM OverStocksku_new Order By SKU");
            dsAll = objSql.GetDs("SELECT  * FROM OverStockFinalData");
            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    string strInsert = Convert.ToString(objSql.ExecuteScalarQuery("SELECT ProductId FROM tb_product WHERE Storeid=4 AND SKU='" + dsAll.Tables[0].Rows[i]["Partner SKU"].ToString() + "'"));
                    if (!string.IsNullOrEmpty(strInsert))
                    {


                        string srrnew = "";// Convert.ToString(objSql.ExecuteScalarQuery("SELECT NewSKu FROM overtockSKU WHERE  SKU='" + dsAll.Tables[0].Rows[i]["sku"].ToString() + "'"));
                        //if (!string.IsNullOrEmpty(srrnew))
                        //{

                        //}
                        //else
                        {
                            srrnew = dsAll.Tables[0].Rows[i]["Partner SKU"].ToString();
                        }
                        objSql.ExecuteNonQuery("UPDATE tb_product SET Name='" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "',ShortName='" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "', SKU='" + srrnew.ToString() + "',OptionSku='" + dsAll.Tables[0].Rows[i]["SKU"].ToString() + "',UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + "',price='" + dsAll.Tables[0].Rows[i]["cost"].ToString().Replace("$", "") + "',Saleprice='" + dsAll.Tables[0].Rows[i]["cost"].ToString().Replace("$", "") + "' WHERE ProductId=" + strInsert + "");
                        //objSql.ExecuteNonQuery("DELETE FROM tb_WareHouseProductInventory WHERE ProductId=" + strInsert + "");
                        //if (dsmaster.Tables[0].Rows[0]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                        //{
                        //    objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (1," + strInsert + ",10,1)");
                        //}
                        //else
                        //{
                        //    objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (15," + strInsert + ",10,1)");
                        //}
                        //objSql.ExecuteNonQuery("DELETE FROM tb_ProductCategory WHERE ProductId=" + strInsert + "");

                        //objSql.ExecuteNonQuery("INSERT INTO tb_ProductCategory(CategoryID,ProductID) VALUES (1721," + strInsert + ",10)");


                    }
                    else
                    {
                        //DataSet dsmaster = new DataSet();
                        //dsmaster = objSql.GetDs("SELECT * FROM MASTERData WHERE [Product SKU]='" + dsAll.Tables[0].Rows[i]["sku"].ToString() + "'");
                        //if (dsmaster != null && dsmaster.Tables[0].Rows.Count > 0)
                        //{

                        string srrnew = ""; //Convert.ToString(objSql.ExecuteScalarQuery("SELECT NewSKu FROM overtockSKU WHERE  SKU='" + dsAll.Tables[0].Rows[i]["sku"].ToString() + "'"));
                        //if (!string.IsNullOrEmpty(srrnew))
                        //{

                        //}
                        //else
                        {
                            srrnew = dsAll.Tables[0].Rows[i]["Partner SKU"].ToString();
                        }
                        strInsert = Convert.ToString(objSql.ExecuteScalarQuery("INSERT INTO  tb_product (storeid,Active,Deleted,CreatedOn,Name,ShortName,SKU,Inventory,Weight,OptionSku,UPC,price,Saleprice,Description,Features,ManufactureID,Condition,ProductCondition) VALUES(4,1,0,getdate(),'" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "','" + srrnew.ToString() + "',0,0,'" + dsAll.Tables[0].Rows[i]["SKU"].ToString() + "','" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["cost"].ToString().Replace("$", "") + "','" + dsAll.Tables[0].Rows[i]["cost"].ToString().Replace("$", "") + "','','',6,'New','New'); SELECT SCOPE_IDENTITY();"));

                        // objSql.ExecuteNonQuery("UPDATE tb_product SET Imagename='" + srrnew.ToString() + "_" + strInsert.ToString() + ".jpg' WHERE ProductId=" + strInsert + "");

                        objSql.ExecuteNonQuery("DELETE FROM tb_WareHouseProductInventory WHERE ProductId=" + strInsert + "");
                        //if (dsmaster.Tables[0].Rows[0]["Defualt Warehouse Location"].ToString().ToLower() == "ca")
                        //{
                        objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory) VALUES (1," + strInsert + ",0)");
                        //}
                        //else
                        //{
                        //    objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory) VALUES (15," + strInsert + ",0)");
                        //}
                        objSql.ExecuteNonQuery("DELETE FROM tb_ProductCategory WHERE ProductId=" + strInsert + "");

                        objSql.ExecuteNonQuery("INSERT INTO tb_ProductCategory(CategoryID,ProductID) VALUES (1721," + strInsert + ",10)");

                        //}


                    }
                }
            }

        }
        public static string RemoveSpecialCharacter(char[] charr)
        {
            string res = "";
            string value = new string(charr);
            value = value.Replace('~', '-');
            value = value.Replace('!', '-');
            value = value.Replace('@', '-');
            value = value.Replace('#', '-');
            value = value.Replace('$', '-');
            value = value.Replace('%', '-');
            value = value.Replace('^', '-');
            value = value.Replace('&', '-');
            value = value.Replace('*', '-');
            value = value.Replace('(', '-');
            value = value.Replace(')', '-');
            value = value.Replace('_', '-');
            value = value.Replace('+', '-');
            value = value.Replace('|', '-');
            value = value.Replace('\\', '-');
            value = value.Replace('/', '-');
            value = value.Replace('?', '-');
            value = value.Replace('\'', '-');
            value = value.Replace('"', '-');
            value = value.Replace(' ', '-');
            value = value.Replace('>', '-');
            value = value.Replace('<', '-');
            value = value.Replace('.', '-');
            value = value.Replace(',', '-');
            value = value.Replace(':', '-');
            value = value.Replace("'", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            res = value;
            return res;
        }
        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            if (File.Exists(Server.MapPath("/catalog.xml")))
            {
                DataSet dsXMl = new DataSet();
                //dsXMl.ReadXml(Server.MapPath("/catalog.xml"));
                XmlDocument _xmlDocument = new XmlDocument();
                _xmlDocument.Load(Server.MapPath("/catalog.xml"));

                //Select the element with in the xml you wish to extract;
                XmlNodeList _nodeList = _xmlDocument.SelectNodes("Catalog/Item");
                SQLAccess objSql = new SQLAccess();
                foreach (XmlNode _node in _nodeList)
                {
                    string strproductyahooid = "";
                    strproductyahooid = _node.Attributes[0].Value.ToString();
                    // if (strproductyahooid == "flanders-multi-jacquard-curtain")
                    //if (strproductyahooid == "cappuccino-faux-stripe-taffeta-curtains-drapes")
                    //{

                    //}
                    //else
                    //{
                    //    continue;
                    //}
                    //Int32 strurl = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count([Product Name]) FROM MASTERData WHERE [Product URLS] like '%" + strproductyahooid + ".html%'"));
                    Int32 strurl = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT  Count(productId) FROM tb_Product WHERE  ProductId in (SELECT ProductID FROM tb_ProductCategory WHERE CategoryID in (SELECT CategoryID FROM tb_CategoryMapping WHERE ParentCategoryID="+ txtCatId.Text.ToString() +")) and  Storeid=1 and isnull(active,0)=1 AND isnull(Deleted,0)=0  and ProductURL like '%" + strproductyahooid + ".html%'"));

                    // Int32 strurl = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count(ProductURL) FROM tb_TempImagerename_New WHERE ProductURL like '%" + strproductyahooid + ".html%'"));
                    // string strcode = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 MASTERData_New.[Product SKU] FROM MASTERData_New INNER JOIN MASTERData ON  MASTERData_New.[Old Product URLS]=MASTERData.[Product URLS]  WHERE  MASTERData_New.[Product URLS] like '%/" + strproductyahooid + ".html%' AND  MASTERData_New.[Main Category] in ('Designer Silk Fabric Curtains','Designer Faux Silk Fabric Curtains','Solid Silk Fabric Curtains') "));
                    if (strurl > 0)
                    //if (strcode != "")
                    {
                        XmlNodeList list = _node.ChildNodes;
                        // string strcode = "";
                        string strstartprice = "";
                        string strprice = "0";
                        string strsaleprice = "0";
                        string strdescription = "";
                        string strydproductfeatures = "";
                        string strproductshippingtime = "";
                        string strprice1 = "";
                        string strprice2 = "";
                        string strprice3 = "";
                        string strprice4 = "";


                        string strimg = "";
                        string strimg1 = "";
                        string strimg2 = "";
                        string strimg3 = "";
                        string strimg4 = "";


                        string ydprivacy = "";
                        string ydlightcontrol = "";
                        string ydefficiency = "";
                        string shipweight = "";
                        Decimal strprice11 = 0;
                        Decimal strprice22 = 0;
                        Decimal strprice33 = 0;
                        Decimal strprice44 = 0;

                        Decimal strsaleprice11 = 0;
                        Decimal strsaleprice22 = 0;
                        Decimal strsaleprice33 = 0;
                        Decimal strsaleprice44 = 0;

                        string strimage = "";
                        for (int j = 0; j < _node.ChildNodes.Count; j++)
                        {

                            if (list[j].Name == "ItemField")
                            {
                                //list[j].Attributes[0].Value.ToString();
                                // Response.Write(list[j].Attributes[0].Name.ToString()+" "+list[j].Attributes[0].Value.ToString());
                                //Response.Write(list[j].Attributes[1].Name.ToString()+" "+list[j].Attributes[1].Value.ToString());
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "code")
                                {
                                    //strcode = list[j].Attributes[1].Value.ToString();
                                }
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "image")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "more-image-1")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg1 = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "more-image-2")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg2 = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "more-image-3")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg3 = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "more-image-4")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg4 = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "price")
                                //{
                                //    strprice = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "start-price")
                                //{
                                //    strstartprice = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "sale-price")
                                //{
                                //    strsaleprice = list[j].Attributes[1].Value.ToString();
                                //}
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-searchable-content")
                                {
                                    strdescription = list[j].Attributes[1].Value.ToString();
                                }
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-privacy")
                                //{
                                //    ydprivacy = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-light-control")
                                //{
                                //    ydlightcontrol = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-efficiency")
                                //{
                                //    ydefficiency = list[j].Attributes[1].Value.ToString();
                                //}
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-product-features")
                                {
                                    strydproductfeatures = list[j].Attributes[1].Value.ToString();
                                }
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "product-shipping-time")
                                //{
                                //    strproductshippingtime = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "ship-weight")
                                //{
                                //    shipweight = list[j].Attributes[1].Value.ToString();
                                //}

                            }
                            if (list[j].Name == "ItemFieldOptions")
                            {
                                XmlNodeList _nodeList1 = list[j].ChildNodes;
                                for (int k = 0; k < list[j].ChildNodes.Count; j++)
                                {
                                    XmlNodeList _nodeList2 = _nodeList1[k].ChildNodes;
                                    for (int p = 0; p < _nodeList1[k].ChildNodes.Count; p++)
                                    {


                                        //if (_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") > -1)
                                        //{
                                        //    Response.Write(_nodeList2[p].Attributes[0].Value.ToString());
                                        //    if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51w") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51w") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51w") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51w") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }





                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51x") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51x") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51x") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51x") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }


                                        //}

                                    }
                                }
                            }


                        }
                        //if (strstartprice != "")
                        //{
                        //    strprice = strstartprice;
                        //}
                        //else if (strsaleprice != "" && strprice == "")
                        //{
                        //    strprice = strsaleprice;
                        //}
                        //if (strsaleprice == "")
                        //{
                        //    strsaleprice = strprice;
                        //}
                        //try
                        //{
                        //    if (strprice1 == "")
                        //    {
                        //        strprice1 = strprice;
                        //        strprice11 = Convert.ToDecimal(strprice1);
                        //        strsaleprice11 = Convert.ToDecimal(strsaleprice);
                        //    }
                        //    else
                        //    {
                        //        strprice11 = Convert.ToDecimal(strprice1) + Convert.ToDecimal(strprice);
                        //        strsaleprice11 = Convert.ToDecimal(strsaleprice) + Convert.ToDecimal(strprice1);
                        //        //strprice11 = Convert.ToDecimal(strprice);
                        //        //strsaleprice11 = Convert.ToDecimal(strprice1);
                        //    }
                        //    if (strprice2 == "")
                        //    {
                        //        strprice2 = strprice;
                        //        strprice22 = Convert.ToDecimal(strprice2);
                        //        strsaleprice22 = Convert.ToDecimal(strsaleprice);
                        //    }
                        //    else
                        //    {
                        //        strprice22 = Convert.ToDecimal(strprice2) + Convert.ToDecimal(strprice);
                        //        strsaleprice22 = Convert.ToDecimal(strsaleprice) + Convert.ToDecimal(strprice2);

                        //        //strprice22 = Convert.ToDecimal(strprice);
                        //        //strsaleprice22 = Convert.ToDecimal(strprice2);
                        //    }
                        //    if (strprice3 == "")
                        //    {
                        //        strprice3 = strprice;
                        //        strprice33 = Convert.ToDecimal(strprice3);
                        //        strsaleprice33 = Convert.ToDecimal(strsaleprice);
                        //    }
                        //    else
                        //    {
                        //        strprice33 = Convert.ToDecimal(strprice3) + Convert.ToDecimal(strprice);
                        //        strsaleprice33 = Convert.ToDecimal(strsaleprice) + Convert.ToDecimal(strprice3);

                        //        //strprice33 = Convert.ToDecimal(strprice);
                        //        //strsaleprice33 = Convert.ToDecimal(strprice3);
                        //    }
                        //    if (strprice4 == "")
                        //    {
                        //        strprice4 = strprice;
                        //        strprice44 = Convert.ToDecimal(strprice4);
                        //        strsaleprice44 = Convert.ToDecimal(strsaleprice);
                        //    }
                        //    else
                        //    {
                        //        strprice44 = Convert.ToDecimal(strprice4) + Convert.ToDecimal(strprice);
                        //        strsaleprice44 = Convert.ToDecimal(strsaleprice) + Convert.ToDecimal(strprice4);

                        //        //strprice44 = Convert.ToDecimal(strprice);
                        //        //strsaleprice44 = Convert.ToDecimal(strprice4);
                        //    }

                        //}
                        //catch
                        //{
                        //}
                        //if ("PDCH-KBS-16-SW" == strcode.ToString())
                        //{

                        //}
                        //objSql.ExecuteNonQuery("UPDATE tb_TempImagerename_New SET Image='" + strimg + "', Image1='" + strimg1 + "', Image2='" + strimg2 + "', Image3='" + strimg3 + "',Image4='" + strimg4 + "'  WHERE ProductURL like '%" + strproductyahooid + ".html%'");

                        objSql.ExecuteNonQuery("UPDATE tb_Product SET Description='" + strdescription.Replace("'", "''") + "',Features='" + strydproductfeatures.Replace("'","''") + "'  WHERE ProductURL like '%" + strproductyahooid + ".html%'");

                        //if (strcode != "")
                        //{

                        //    //strcode = strcode.Replace("-SLDW", "");
                        //    //strcode = strcode.Replace("-GR-BO", "");
                        //    //strcode = strcode.Replace("-GR", "");
                        //    strcode = strcode.Replace("-84-PR", "").Replace("-96-PR", "").Replace("-108-PR", "").Replace("-120-PR", "");
                        //    strcode = strcode.Replace("-84-GRBO", "").Replace("-96-GRBO", "").Replace("-108-GRBO", "").Replace("-120-GRBO", "");

                        //    strcode = strcode.Replace("-84-GR", "").Replace("-96-GR", "").Replace("-108-GR", "").Replace("-120-GR", "");
                        //    strcode = strcode.Replace("-84-SLDW", "").Replace("-96-SLDW", "").Replace("-108-SLDW", "").Replace("-120-SLDW", "");
                        //    strcode = strcode.Replace("-84-DLSW", "").Replace("-96-DLSW", "").Replace("-108-DLSW", "").Replace("-120-DLSW", "");

                        //    strcode = strcode.Replace("-84-RU", "").Replace("-96-RU", "").Replace("-108-RU", "").Replace("-120-RU", "");
                        //    strcode = strcode.Replace("-DL-CUS", "");
                        //    strcode = strcode.Replace("-CUS", "");
                        //    strcode = strcode.Replace("-84", "").Replace("-96", "").Replace("-108", "").Replace("-120", "");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-DL-CUS'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-CUS'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "'");



                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-DLSW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-DLSW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-DLSW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-DLSW'");

                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-SLDW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-SLDW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-SLDW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-SLDW'");


                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-GR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-GR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-GR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-GR'");


                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-GRBO'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-GRBO'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-GRBO'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-GRBO'");


                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-PR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-PR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-PR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-PR'");


                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-RU'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-RU'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-RU'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-RU'");

                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "'Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-DL-CUS'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-CUS'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "'");



                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-DLSW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-DLSW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-DLSW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-DLSW'");

                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-SLDW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-SLDW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-SLDW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-SLDW'");


                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-GR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-GR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-GR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-GR'");


                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-GRBO'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-GRBO'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-GRBO'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-GRBO'");


                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-PR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-PR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-PR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-PR'");


                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-RU'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-RU'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-RU'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-RU'");




                        //}



                    }
                }



            }
        }

        protected void btnAddProduct2_Click(object sender, EventArgs e)
        {
            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            dsAll = objSql.GetDs("SELECT DISTINCT [Preferred Vendor],[Product URLS],[Item Categogy Global Dimension] FROM MASTERData WHERE [Product URLS] like '%http://%' and [Product URLS] in (SELECT 'http://www.halfpricedrapes.com/'+ ProductUrl FROM tb_product WHERE Isnull(Active,0)=1 AND isnull(Deleted,0)=0 AND Storeid=1) AND isnull([Item Categogy Global Dimension],'') <> ''  Order By [Product URLS]");
            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {
                Response.Write(dsAll.Tables[0].Rows.Count.ToString());
                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    string strvendor = Convert.ToString(dsAll.Tables[0].Rows[i]["Preferred Vendor"].ToString());
                    if (!string.IsNullOrEmpty(strvendor))
                    {
                        Int32 vendorid = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT VendorID FROM tb_Vendor WHERE Name='" + strvendor.Replace("'", "''") + "' AND isnull(Active,0)=1 AND isnull(Deleted,0)=0"));
                        if (vendorid == 0)
                        {
                            vendorid = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_Vendor(Name,Active,Deleted,IsDropshipper) VALUES ('" + strvendor.Replace("'", "''") + "',1,0,0); SELECT SCOPE_IDENTITY();"));
                        }

                        CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET ItemType='" + dsAll.Tables[0].Rows[i]["Item Categogy Global Dimension"].ToString().Replace("'", "''") + "', ProductTypeDeliveryID=" + Convert.ToInt32(14) + ",VendorID=" + vendorid + " WHERE ProductURL='" + dsAll.Tables[0].Rows[i]["Product URLS"].ToString().Replace("http://www.halfpricedrapes.com/", "").Replace("'", "''") + "' AND StoreId=1 AND Isnull(Active,0)=1 AND isnull(Deleted,0)=0");
                    }
                }
            }



        }

        protected void Button1_Click(object sender, EventArgs e)
        {

            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            dsAll = objSql.GetDs("SELECT SKU,ISNULL(Discontinued,0) as Discontinued FROM " + txttablename.Text.ToString() + "");
            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    DataSet dsMaster = new DataSet();
                    dsMaster = objSql.GetDs("SELECT TOP 1 * FROM MasterData WHERE [Product SKU] ='" + Convert.ToString(dsAll.Tables[0].Rows[i]["SKU"].ToString()) + "'");
                    if (dsMaster != null && dsMaster.Tables[0].Rows.Count > 0)
                    {
                        // Int32 strProductId = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT productId FROM tb_product WHERE SKu='" + dsAll.Tables[0].Rows[i]["SKU"].ToString() + "' AND StoreId=1 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0"));
                        //if (strProductId > 0)
                        //{

                        //}
                        //else
                        //{
                        Int32 newP = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_product(Name,SKU,UPC,Price,SalePrice,ProductURL,Active,Deleted,StoreId,Discontinue,Inventory) VALUES ('" + dsMaster.Tables[0].Rows[0]["Product Name"].ToString().Replace("'", "''") + "','" + dsMaster.Tables[0].Rows[0]["Product SKU"].ToString().Replace("'", "''") + "','" + dsMaster.Tables[0].Rows[0]["UPC"].ToString() + "','" + dsMaster.Tables[0].Rows[0]["Price"].ToString() + "','" + dsMaster.Tables[0].Rows[0]["Saleprice"].ToString() + "','" + dsMaster.Tables[0].Rows[0]["Product URLS"].ToString().Replace("http://www.halfpricedrapes.com/", "") + "',1,0," + txtastoreid.Text.ToString() + "," + dsAll.Tables[0].Rows[i]["Discontinued"].ToString() + ",0); SELECT SCOPE_IDENTITY();"));
                        //}

                    }
                    else
                    {
                        dsMaster = objSql.GetDs("SELECT TOP 1 * FROM Acctivate_UPC WHERE ProductID ='" + Convert.ToString(dsAll.Tables[0].Rows[i]["SKU"].ToString()) + "'");
                        if (dsMaster != null && dsMaster.Tables[0].Rows.Count > 0)
                        {
                            //Int32 strProductId = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT productId FROM tb_product WHERE SKu='" + dsAll.Tables[0].Rows[i]["SKU"].ToString() + "' AND StoreId=1 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0"));
                            //if (strProductId > 0)
                            //{

                            //}
                            //else
                            //{
                            Int32 newP = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_product(Name,SKU,UPC,Price,SalePrice,ProductURL,Active,Deleted,StoreId,Discontinue,Inventory) VALUES ('','" + dsMaster.Tables[0].Rows[0]["ProductID"].ToString() + "','" + dsMaster.Tables[0].Rows[0]["UPC"].ToString() + "','0.00','0.00','',1,0," + txtastoreid.Text.ToString() + "," + dsAll.Tables[0].Rows[i]["Discontinued"].ToString() + ",0); SELECT SCOPE_IDENTITY();"));
                            //}
                        }

                    }
                }
            }


        }
        protected void Button5_Click(object sender, EventArgs e)
        {

            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            dsAll = objSql.GetDs("SELECT SKU,Name,Discontinued FROM " + txttablename.Text.ToString() + "");
            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {

                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    DataSet dsMaster = new DataSet();
                    dsMaster = objSql.GetDs("SELECT TOP 1 * FROM MasterData_New WHERE [Product SKU] ='" + Convert.ToString(dsAll.Tables[0].Rows[i]["SKU"].ToString()) + "'");
                    if (dsMaster != null && dsMaster.Tables[0].Rows.Count > 0)
                    {
                        // Int32 strProductId = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT productId FROM tb_product WHERE SKu='" + dsAll.Tables[0].Rows[i]["SKU"].ToString() + "' AND StoreId=1 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0"));
                        //if (strProductId > 0)
                        //{

                        //}
                        //else
                        //{
                        Int32 newP = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_product(Name,SKU,UPC,Price,SalePrice,ProductURL,Active,Deleted,StoreId,Discontinue,Inventory) VALUES ('" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "','" + dsMaster.Tables[0].Rows[0]["UPC"].ToString() + "','" + dsMaster.Tables[0].Rows[0]["Price"].ToString() + "','" + dsMaster.Tables[0].Rows[0]["Saleprice"].ToString() + "','" + dsMaster.Tables[0].Rows[0]["Product URLS"].ToString().Replace("http://www.halfpricedrapes.com/", "") + "',1,0," + txtastoreid.Text.ToString() + "," + dsAll.Tables[0].Rows[i]["Discontinued"].ToString() + ",0); SELECT SCOPE_IDENTITY();"));
                        //}

                    }
                    else
                    {
                        dsMaster = objSql.GetDs("SELECT TOP 1 * FROM Acctivate_UPC WHERE ProductID ='" + Convert.ToString(dsAll.Tables[0].Rows[i]["SKU"].ToString()) + "'");
                        if (dsMaster != null && dsMaster.Tables[0].Rows.Count > 0)
                        {
                            //Int32 strProductId = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT productId FROM tb_product WHERE SKu='" + dsAll.Tables[0].Rows[i]["SKU"].ToString() + "' AND StoreId=1 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0"));
                            //if (strProductId > 0)
                            //{

                            //}
                            //else
                            //{
                            Int32 newP = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_product(Name,SKU,UPC,Price,SalePrice,ProductURL,Active,Deleted,StoreId,Discontinue,Inventory) VALUES ('" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["SKU"].ToString() + "','" + dsMaster.Tables[0].Rows[0]["UPC"].ToString() + "','0.00','0.00','',1,0," + txtastoreid.Text.ToString() + "," + dsAll.Tables[0].Rows[i]["Discontinued"].ToString() + ",0); SELECT SCOPE_IDENTITY();"));
                            //}
                        }

                    }
                }
            }


        }

        protected void Button2_Click(object sender, EventArgs e)
        {
            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            //dsAll = objSql.GetDs("SELECT Top 20 * FROM AmazonAllProduct WHERE isnull(UPC,'') <> '' AND isnull(UPC,'') NOT in (SELECT isnull(UPC,'') FROM tb_product WHERE Storeid=3) AND Isnull(ImageURl,'') <>''");

            //string strSqlname = "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT tb_Product.UPC FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + ") UNION ";
            //strSqlname += "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT isnull(UPC,'') tb_ProductVariantValue FROM tb_ProductVariantValue  WHERE isnull(upc,'') <> '' AND productId in (SELECT tb_Product.ProductId FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + "))";

            string strSqlname = "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT Isnull(UPC,'') FROM tb_Product WHERE StoreId=4 AND " + txttablename.Text.ToString() + " AND isnull(Active,0)=1 And isnull(Deleted,0)=0) ";
            //strSqlname += "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT isnull(UPC,'') tb_ProductVariantValue FROM tb_ProductVariantValue  WHERE isnull(upc,'') <> '' AND productId in (SELECT tb_Product.ProductId FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + "))";




            //dsAll = objSql.GetDs(@"SELECT isnull(UPC,'') FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT tb_Product.UPC FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + ")");
            dsAll = objSql.GetDs(strSqlname);
            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {


                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    Int32 PIds = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 ProductId FROM tb_product WHERE StoreId=3 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'"));
                    if (PIds > 0)
                    {
                        //DataSet dsnew = new DataSet();

                        //dsnew = CommonComponent.GetCommonDataSet("SELECT tb_product.Price,tb_ProductVariantValue.BasecustomPrice,tb_product.ProductId,isnull(tb_product.Imagename,'') as Imagename FROM tb_product INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.ProductID=tb_product.ProductID WHERE tb_product.StoreId=1 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND  tb_ProductVariantValue.UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");

                        //if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                        //{

                        //    string imagename = "";
                        //    if (!string.IsNullOrEmpty(dsnew.Tables[0].Rows[0]["Imagename"].ToString()))
                        //    {
                        //        if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString())))
                        //        {
                        //            try
                        //            {
                        //                imagename = dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "_" + PIds.ToString() + ".jpg";

                        //                for (int imgnew = 1; imgnew < 10; imgnew++)
                        //                {
                        //                    if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg")))
                        //                    {
                        //                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                        //                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/Medium/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                        //                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/icon/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                        //                    }
                        //                }
                        //            }
                        //            catch
                        //            {

                        //            }
                        //        }
                        //    }

                        //   /// CommonComponent.ExecuteCommonData("UPDATE tb_product SET Imagename='" + imagename + "' WHERE  tb_product.StoreId=3 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");
                        //}
                        //                        string B1 = txtb1.Text.ToString();
                        //                        string B2 = txtb2.Text.ToString();
                        //                        string B3 = txtb3.Text.ToString();
                        //                        string B4 = txtb4.Text.ToString();
                        //                        string B5 = txtb5.Text.ToString();

                        //                        CommonComponent.ExecuteCommonData(@"INSERT INTO  tb_ProductAmazon (AmazonRefID, StandardProductID, ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                        //                                                 BulletPoint1, BulletPoint2, BulletPoint3, BulletPoint4, BulletPoint5, ProductDescription, ProductType, ItemType, Prop65, CPSIAWarning1, CPSIAWarning2, 
                        //                                                 CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                        //                                                 PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                        //                                                 OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                        //                                                 FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                        //                                                 RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                        //                                                 [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                        //                                                 RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                        //                                                 SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType) 
                        //                                                 SELECT        " + PIds.ToString() + @", '" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + @"', ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                        //                                                 '" + B1 + @"', '" + B2 + @"', '" + B3 + @"', '" + B4 + @"', '" + B5 + @"', ProductDescription, ProductType, ItemType, Prop65, CPSIAWarning1, CPSIAWarning2, 
                        //                                                 CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                        //                                                 PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                        //                                                 OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                        //                                                 FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                        //                                                 RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                        //                                                 [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                        //                                                 RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                        //                                                 SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType
                        //                                                 FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");
                        //FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");

                        continue;
                    }
                    DataSet dsOverStockData = new DataSet();
                    dsOverStockData = CommonComponent.GetCommonDataSet("SELECT ProductId FROM tb_product WHERE StoreId=4 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");
                    string B1 = txtb1.Text.ToString();
                    string B2 = txtb2.Text.ToString();
                    string B3 = txtb3.Text.ToString();
                    string B4 = txtb4.Text.ToString();
                    string B5 = txtb5.Text.ToString();

                    if (dsOverStockData != null && dsOverStockData.Tables.Count > 0 && dsOverStockData.Tables[0].Rows.Count > 0)
                    {
                        Int32 PI = 0;
                        PI = Convert.ToInt32(CommonComponent.GetScalarCommonData(@"INSERT INTO tb_Product( StoreID, ManufactureID, DistributorID, TaxClassID, ProductTypeID, ProductTypeDeliveryID, QuantityDiscountID, VendorID, Name, 
                         SEName, SKU, Price, SalePrice, OurPrice, SurCharge, Inventory, Weight, Size, Color, Description, UPC, Height, Width, Length, Avail, AvaliableStartDate,
                         AvailableEndDate, ImageName, PDFName, Discontinue, MainCategory, IsKit, IsPack, IsFeatured, IsFreeShipping, IsNewArrival, IsSaleclearance, 
                         IsAuthorizeRefund, IsBestSeller, OptionalAccessories, OutOfStockMessage, SEKeywords, SEDescription, SETitle, ToolTip, DisplayOrder, GiftWrap, 
                         CallUsForPrice, isSatisfactionGuaranteed, isTabbingDisplay, TabTitle1, TabTitle2, TabDesc2, TabTitle3, TabDesc3, TabTitle4, TabDesc4, TabTitle5, 
                         TabDesc5, ViewCount, LastViewDate, Active, Deleted, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, RelatedProduct, TagName, eBayListingType, 
                         eBayListingDay, eBayCreatedOn, eBayLastUpdated, eBayListingFee, eBayProductID, eBayCategoryID, eBayStoreCategoryID, IsRestricted, Perishable, 
                         RequiresRefrigeration, RequiresFreezing, ContainsAlcohol, MapPriceIndicator, ShippingStartDate, ShippingEndDate, ManufacturePartNo, Summary, SellerID, 
                         Isbn, ASIN, Srno, ProductSetID, BuyListingID, Features, [Extended-warranty], BuyCategoryID, Location, Condition, IsShipSeparately, ManuItemURL, 
                         WebSiteShortTitle, WebSiteLongTitle, ItemPackage, ActivationMark, Prop65, Pro65Motherboard, Age18Verification, ChokingHazard1, ChokingHazard2, 
                         ChokingHazard3, ChokingHazard4, SellerPartNo, Shipping, BuyKeywords, Options, IsFreeEngraving, EngravingSize, OldProductID, IsSpecials, ReturnPolicy, 
                         amazonproductid, ManufactureCatalogNo, Brand, Bulletpoint1, Bulletpoint2, Bulletpoint3, Bulletpoint4, Bulletpoint5, ProductURL, YahooID, Dimension, 
                         Taxable, Ypath, LowInventory, MarryProducts, InventoryUpdatedOn, DiscontinuedOn, ImageOrg, Visualproperties, Image1, Image2, Image3, Image4, Image5, 
                         overstockproductid, OptionSku, EbayCreated, ContainsTobacco, Sizes, ShortName, PartNumber, Materials, WarrantyProvider, WarrantyDescription, 
                         WarrantyContactPhoneNumber, ProductSummary, ProductCondition, SourceZipCode, NeweggItemNumber, BuyReferenceID, PerInchPrice, AdditionalCharge, 
                         YardHeaderandhem, FabricInch, Isfreefabricswatch, IspriceQuote, ProductSwatchid, Isproperty, LightControl, Privacy, Efficiency, Pattern, Colors, Fabric, Style, 
                         SecondaryColor, SwatchDescription, ShippingTime, FabricType, FabricCode, IsCustom, Header, PageURL, ColorSku, Ismadetoorder, Ismadetomeasure, 
                         IsRoman, RomanShadeId, SalePriceTag, IsNewArrivalFromDate, IsNewArrivalToDate, IsBestSellerFromDate, IsBestSellerToDate, IsFreeShippingFromDate, 
                         IsFreeShippingToDate, IsFeaturedFromDate, IsFeaturedToDate, ImageDescription, FeatureID, Ismadetoready, Ismadetoorderswatch, IsdropshipProduct, 
                         IsDataVerify, DataVerifyBy, IsDataVerifyOn, ItemType, IsHamming, HammingSafetyQty, HammingSafetyper) 
                         SELECT 3, 3,  NULL, NULL, 2, ProductTypeDeliveryID, QuantityDiscountID, VendorID,  '" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("'", "''") + @"', 
                         '" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("'", "''").Replace(" ", "-").ToLower() + @"', '" + dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + @"',Price, SalePrice, OurPrice, SurCharge, Inventory, case when isnull(Weight,0)=0 then 1 else Weight end as Weight, Size, '" + dsAll.Tables[0].Rows[i]["Color"].ToString().Replace("'", "''") + "', Description, '" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + @"', Height, Width, Length, Avail, AvaliableStartDate, 
                         AvailableEndDate, '', '', Discontinue, '', IsKit, IsPack, IsFeatured, IsFreeShipping, IsNewArrival, IsSaleclearance, 
                         IsAuthorizeRefund, IsBestSeller, OptionalAccessories, OutOfStockMessage, SEKeywords, SEDescription, '" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("'", "''") + @"', '" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + @"', DisplayOrder, GiftWrap, 
                         CallUsForPrice, isSatisfactionGuaranteed, isTabbingDisplay, TabTitle1, TabTitle2, TabDesc2, TabTitle3, TabDesc3, TabTitle4, TabDesc4, TabTitle5, 
                         TabDesc5, ViewCount, LastViewDate, Active, Deleted, CreatedBy, CreatedOn, UpdatedBy, getdate(), RelatedProduct, TagName, eBayListingType, 
                         eBayListingDay, eBayCreatedOn, eBayLastUpdated, eBayListingFee, eBayProductID, eBayCategoryID, eBayStoreCategoryID, IsRestricted, Perishable, 
                         RequiresRefrigeration, RequiresFreezing, ContainsAlcohol, MapPriceIndicator, ShippingStartDate, ShippingEndDate, ManufacturePartNo, Summary, SellerID, 
                         Isbn, ASIN, Srno, ProductSetID, BuyListingID, Features, [Extended-warranty], BuyCategoryID, Location, Condition, IsShipSeparately, ManuItemURL, 
                         WebSiteShortTitle, WebSiteLongTitle, ItemPackage, ActivationMark, Prop65, Pro65Motherboard, Age18Verification, ChokingHazard1, ChokingHazard2, 
                         ChokingHazard3, ChokingHazard4, SellerPartNo, Shipping, BuyKeywords, Options, IsFreeEngraving, EngravingSize, OldProductID, IsSpecials, ReturnPolicy, 
                         '', ManufactureCatalogNo, Brand, Bulletpoint1, Bulletpoint2, Bulletpoint3, Bulletpoint4, Bulletpoint5, ProductURL, YahooID, Dimension, 
                         Taxable, Ypath, LowInventory, MarryProducts, InventoryUpdatedOn, DiscontinuedOn, ImageOrg, Visualproperties, Image1, Image2, Image3, Image4, Image5, 
                         '', '', EbayCreated, ContainsTobacco, Sizes, ShortName, PartNumber, Materials, WarrantyProvider, WarrantyDescription, 
                         WarrantyContactPhoneNumber, ProductSummary, ProductCondition, SourceZipCode, NeweggItemNumber, BuyReferenceID, PerInchPrice, AdditionalCharge, 
                         YardHeaderandhem, FabricInch, Isfreefabricswatch, IspriceQuote, ProductSwatchid, Isproperty, LightControl, Privacy, Efficiency, Pattern, Colors, Fabric, Style, 
                         SecondaryColor, SwatchDescription, ShippingTime, FabricType, FabricCode, IsCustom, Header, PageURL, ColorSku, Ismadetoorder, Ismadetomeasure, 
                         IsRoman, RomanShadeId, SalePriceTag, IsNewArrivalFromDate, IsNewArrivalToDate, IsBestSellerFromDate, IsBestSellerToDate, IsFreeShippingFromDate, 
                         IsFreeShippingToDate, IsFeaturedFromDate, IsFeaturedToDate, ImageDescription, FeatureID, Ismadetoready, Ismadetoorderswatch, IsdropshipProduct, 
                         IsDataVerify, DataVerifyBy, IsDataVerifyOn, ItemType, IsHamming, HammingSafetyQty, HammingSafetyper FROM tb_Product WHERE ProductId=" + dsOverStockData.Tables[0].Rows[0]["ProductId"] + @"; SELECT SCOPE_IDENTITY(); "));
                        CommonComponent.ExecuteCommonData(@"INSERT INTO  tb_WareHouseProductInventory(WareHouseID, ProductID, Inventory, PreferredLocation) SELECT WareHouseID, " + PI.ToString() + @", Inventory, PreferredLocation FROM tb_WareHouseProductInventory WHERE ProductId=" + dsOverStockData.Tables[0].Rows[0]["ProductId"].ToString() + @"");


                        CommonComponent.ExecuteCommonData(@"INSERT INTO  tb_ProductAmazon (AmazonRefID, StandardProductID, ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                         BulletPoint1, BulletPoint2, BulletPoint3, BulletPoint4, BulletPoint5, ProductDescription, ProductType, ItemType, Prop65, CPSIAWarning1, CPSIAWarning2, 
                         CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                         PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                         OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                         FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                         RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                         [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                         RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                         SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType) 
                         SELECT        " + PI + @", '" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + @"', ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                         '" + B1 + @"', '" + B2 + @"', '" + B3 + @"', '" + B4 + @"', '" + B5 + @"', ProductDescription, ProductType, '" + B2.ToString() + @"', Prop65, CPSIAWarning1, CPSIAWarning2, 
                         CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                         PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                         OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                         FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                         RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                         [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                         RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                         SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType
                         FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");
                        //FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");
                        DataSet dsnew = new DataSet();

                        dsnew = CommonComponent.GetCommonDataSet("SELECT tb_product.Price,tb_ProductVariantValue.BasecustomPrice,tb_product.ProductId,isnull(tb_product.Imagename,'') as Imagename FROM tb_product INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.ProductID=tb_product.ProductID WHERE tb_product.StoreId=1 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND  tb_ProductVariantValue.UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");

                        if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                        {
                            string imagename = "";
                            if (!string.IsNullOrEmpty(dsnew.Tables[0].Rows[0]["Imagename"].ToString()))
                            {
                                try
                                {
                                    if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString())))
                                    {
                                        imagename = dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "_" + PI.ToString() + ".jpg";
                                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + imagename.ToString()), true);
                                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/Medium/" + imagename.ToString()), true);
                                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/icon/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + imagename.ToString()), true);
                                        for (int imgnew = 1; imgnew < 10; imgnew++)
                                        {
                                            if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg")))
                                            {
                                                File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                                File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/Medium/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                                File.Copy(Server.MapPath("/Resources/halfpricedraps/product/icon/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                            }
                                        }
                                    }
                                }
                                catch
                                {
                                }
                            }

                            CommonComponent.ExecuteCommonData("UPDATE tb_product SET Imagename='" + imagename + "', Price=" + dsnew.Tables[0].Rows[0]["Price"].ToString() + ",SalePrice=" + dsnew.Tables[0].Rows[0]["BasecustomPrice"].ToString() + " WHERE  tb_product.StoreId=3 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");
                        }
                    }
                    else
                    {
                        dsOverStockData = CommonComponent.GetCommonDataSet("SELECT DISTINCT ProductId FROM tb_product WHERE StoreId=1 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND ProductID IN (SELECT ProductID FROM tb_ProductVariantValue WHERE UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "')");
                        if (dsOverStockData != null && dsOverStockData.Tables.Count > 0 && dsOverStockData.Tables[0].Rows.Count > 0)
                        {
                            Int32 PI = 0;
                            PI = Convert.ToInt32(CommonComponent.GetScalarCommonData(@"INSERT INTO tb_Product( StoreID, ManufactureID, DistributorID, TaxClassID, ProductTypeID, ProductTypeDeliveryID, QuantityDiscountID, VendorID, Name, 
                         SEName, SKU, Price, SalePrice, OurPrice, SurCharge, Inventory, Weight, Size, Color, Description, UPC, Height, Width, Length, Avail, AvaliableStartDate,
                         AvailableEndDate, ImageName, PDFName, Discontinue, MainCategory, IsKit, IsPack, IsFeatured, IsFreeShipping, IsNewArrival, IsSaleclearance, 
                         IsAuthorizeRefund, IsBestSeller, OptionalAccessories, OutOfStockMessage, SEKeywords, SEDescription, SETitle, ToolTip, DisplayOrder, GiftWrap, 
                         CallUsForPrice, isSatisfactionGuaranteed, isTabbingDisplay, TabTitle1, TabTitle2, TabDesc2, TabTitle3, TabDesc3, TabTitle4, TabDesc4, TabTitle5, 
                         TabDesc5, ViewCount, LastViewDate, Active, Deleted, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, RelatedProduct, TagName, eBayListingType, 
                         eBayListingDay, eBayCreatedOn, eBayLastUpdated, eBayListingFee, eBayProductID, eBayCategoryID, eBayStoreCategoryID, IsRestricted, Perishable, 
                         RequiresRefrigeration, RequiresFreezing, ContainsAlcohol, MapPriceIndicator, ShippingStartDate, ShippingEndDate, ManufacturePartNo, Summary, SellerID, 
                         Isbn, ASIN, Srno, ProductSetID, BuyListingID, Features, [Extended-warranty], BuyCategoryID, Location, Condition, IsShipSeparately, ManuItemURL, 
                         WebSiteShortTitle, WebSiteLongTitle, ItemPackage, ActivationMark, Prop65, Pro65Motherboard, Age18Verification, ChokingHazard1, ChokingHazard2, 
                         ChokingHazard3, ChokingHazard4, SellerPartNo, Shipping, BuyKeywords, Options, IsFreeEngraving, EngravingSize, OldProductID, IsSpecials, ReturnPolicy, 
                         amazonproductid, ManufactureCatalogNo, Brand, Bulletpoint1, Bulletpoint2, Bulletpoint3, Bulletpoint4, Bulletpoint5, ProductURL, YahooID, Dimension, 
                         Taxable, Ypath, LowInventory, MarryProducts, InventoryUpdatedOn, DiscontinuedOn, ImageOrg, Visualproperties, Image1, Image2, Image3, Image4, Image5, 
                         overstockproductid, OptionSku, EbayCreated, ContainsTobacco, Sizes, ShortName, PartNumber, Materials, WarrantyProvider, WarrantyDescription, 
                         WarrantyContactPhoneNumber, ProductSummary, ProductCondition, SourceZipCode, NeweggItemNumber, BuyReferenceID, PerInchPrice, AdditionalCharge, 
                         YardHeaderandhem, FabricInch, Isfreefabricswatch, IspriceQuote, ProductSwatchid, Isproperty, LightControl, Privacy, Efficiency, Pattern, Colors, Fabric, Style, 
                         SecondaryColor, SwatchDescription, ShippingTime, FabricType, FabricCode, IsCustom, Header, PageURL, ColorSku, Ismadetoorder, Ismadetomeasure, 
                         IsRoman, RomanShadeId, SalePriceTag, IsNewArrivalFromDate, IsNewArrivalToDate, IsBestSellerFromDate, IsBestSellerToDate, IsFreeShippingFromDate, 
                         IsFreeShippingToDate, IsFeaturedFromDate, IsFeaturedToDate, ImageDescription, FeatureID, Ismadetoready, Ismadetoorderswatch, IsdropshipProduct, 
                         IsDataVerify, DataVerifyBy, IsDataVerifyOn, ItemType, IsHamming, HammingSafetyQty, HammingSafetyper) 
                         SELECT 3, 3, NULL, NULL, 2, ProductTypeDeliveryID, QuantityDiscountID, VendorID,  '" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("'", "''") + @"', 
                         '" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("'", "''").Replace(" ", "-").ToLower() + @"', '" + dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + @"', '" + dsAll.Tables[0].Rows[i]["Price"].ToString().Replace("'", "''") + @"', '" + dsAll.Tables[0].Rows[i]["Price"].ToString().Replace("'", "''") + @"', OurPrice, SurCharge, Inventory, case when isnull(Weight,0)=0 then 1 else Weight end as Weight, Size, '" + dsAll.Tables[0].Rows[i]["Color"].ToString().Replace("'", "''") + "', Description, '" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + @"', Height, Width, Length, Avail, AvaliableStartDate, 
                         AvailableEndDate, '', '', Discontinue, '', IsKit, IsPack, IsFeatured, IsFreeShipping, IsNewArrival, IsSaleclearance, 
                         IsAuthorizeRefund, IsBestSeller, OptionalAccessories, OutOfStockMessage, SEKeywords, SEDescription, '" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("'", "''") + @"', '" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + @"', DisplayOrder, GiftWrap, 
                         CallUsForPrice, isSatisfactionGuaranteed, isTabbingDisplay, TabTitle1, TabTitle2, TabDesc2, TabTitle3, TabDesc3, TabTitle4, TabDesc4, TabTitle5, 
                         TabDesc5, ViewCount, LastViewDate, Active, Deleted, CreatedBy, CreatedOn, UpdatedBy, getdate(), RelatedProduct, TagName, eBayListingType, 
                         eBayListingDay, eBayCreatedOn, eBayLastUpdated, eBayListingFee, eBayProductID, eBayCategoryID, eBayStoreCategoryID, IsRestricted, Perishable, 
                         RequiresRefrigeration, RequiresFreezing, ContainsAlcohol, MapPriceIndicator, ShippingStartDate, ShippingEndDate, ManufacturePartNo, Summary, SellerID, 
                         Isbn, ASIN, Srno, ProductSetID, BuyListingID, Features, [Extended-warranty], BuyCategoryID, Location, Condition, IsShipSeparately, ManuItemURL, 
                         WebSiteShortTitle, WebSiteLongTitle, ItemPackage, ActivationMark, Prop65, Pro65Motherboard, Age18Verification, ChokingHazard1, ChokingHazard2, 
                         ChokingHazard3, ChokingHazard4, SellerPartNo, Shipping, BuyKeywords, Options, IsFreeEngraving, EngravingSize, OldProductID, IsSpecials, ReturnPolicy, 
                         '', ManufactureCatalogNo, Brand, Bulletpoint1, Bulletpoint2, Bulletpoint3, Bulletpoint4, Bulletpoint5, ProductURL, YahooID, Dimension, 
                         Taxable, Ypath, LowInventory, MarryProducts, InventoryUpdatedOn, DiscontinuedOn, ImageOrg, Visualproperties, Image1, Image2, Image3, Image4, Image5, 
                         '', '', EbayCreated, ContainsTobacco, Sizes, ShortName, PartNumber, Materials, WarrantyProvider, WarrantyDescription, 
                         WarrantyContactPhoneNumber, ProductSummary, ProductCondition, SourceZipCode, NeweggItemNumber, BuyReferenceID, PerInchPrice, AdditionalCharge, 
                         YardHeaderandhem, FabricInch, Isfreefabricswatch, IspriceQuote, ProductSwatchid, Isproperty, LightControl, Privacy, Efficiency, Pattern, Colors, Fabric, Style, 
                         SecondaryColor, SwatchDescription, ShippingTime, FabricType, FabricCode, IsCustom, Header, PageURL, ColorSku, Ismadetoorder, Ismadetomeasure, 
                         IsRoman, RomanShadeId, SalePriceTag, IsNewArrivalFromDate, IsNewArrivalToDate, IsBestSellerFromDate, IsBestSellerToDate, IsFreeShippingFromDate, 
                         IsFreeShippingToDate, IsFeaturedFromDate, IsFeaturedToDate, ImageDescription, FeatureID, Ismadetoready, Ismadetoorderswatch, IsdropshipProduct, 
                         IsDataVerify, DataVerifyBy, IsDataVerifyOn, ItemType, IsHamming, HammingSafetyQty, HammingSafetyper FROM tb_Product WHERE ProductId=" + dsOverStockData.Tables[0].Rows[0]["ProductId"] + @"; SELECT SCOPE_IDENTITY(); "));
                            CommonComponent.ExecuteCommonData(@"INSERT INTO  tb_WareHouseProductInventory(WareHouseID, ProductID, Inventory, PreferredLocation) SELECT WareHouseID, " + PI.ToString() + @", Inventory, PreferredLocation FROM tb_WareHouseProductInventory WHERE ProductId=" + dsOverStockData.Tables[0].Rows[0]["ProductId"].ToString() + @"");


                            CommonComponent.ExecuteCommonData(@"INSERT INTO  tb_ProductAmazon (AmazonRefID, StandardProductID, ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                     BulletPoint1, BulletPoint2, BulletPoint3, BulletPoint4, BulletPoint5, ProductDescription, ProductType, ItemType, Prop65, CPSIAWarning1, CPSIAWarning2, 
                         CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                         PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                         OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                         FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                         RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                         [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                         RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                         SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType) 
                         SELECT        " + PI + @", '" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + @"', ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                         '" + B1 + @"', '" + B2 + @"', '" + B3 + @"', '" + B4 + @"','" + B5 + @"', ProductDescription, ProductType, '" + B2.ToString() + @"', Prop65, CPSIAWarning1, CPSIAWarning2, 
                         CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                         PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                         OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                         FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                         RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                         [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                         RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                         SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType
                         FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");
                            DataSet dsnew = new DataSet();

                            dsnew = CommonComponent.GetCommonDataSet("SELECT tb_product.Price,tb_ProductVariantValue.BasecustomPrice,tb_product.ProductId,isnull(tb_product.Imagename,'') as Imagename FROM tb_product INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.ProductID=tb_product.ProductID WHERE tb_product.StoreId=1 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND  tb_ProductVariantValue.UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");

                            if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                            {

                                string imagename = "";
                                if (!string.IsNullOrEmpty(dsnew.Tables[0].Rows[0]["Imagename"].ToString()))
                                {
                                    if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString())))
                                    {
                                        try
                                        {
                                            imagename = dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "_" + PI.ToString() + ".jpg";
                                            File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + imagename.ToString()), true);
                                            File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/Medium/" + imagename.ToString()), true);
                                            File.Copy(Server.MapPath("/Resources/halfpricedraps/product/icon/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + imagename.ToString()), true);
                                            for (int imgnew = 1; imgnew < 10; imgnew++)
                                            {
                                                if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg")))
                                                {
                                                    File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                                    File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/Medium/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                                    File.Copy(Server.MapPath("/Resources/halfpricedraps/product/icon/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                                }
                                            }
                                        }
                                        catch
                                        {

                                        }
                                    }
                                }

                                CommonComponent.ExecuteCommonData("UPDATE tb_product SET Imagename='" + imagename + "', Price=" + dsnew.Tables[0].Rows[0]["Price"].ToString() + ",SalePrice=" + dsnew.Tables[0].Rows[0]["BasecustomPrice"].ToString() + " WHERE  tb_product.StoreId=3 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");
                            }
                            //FROM  tb_ProductAmazon  WHERE AmazonRefID=5815");
                        }
                    }

                }
            }
        }
        protected void btnAmazon_Click(object sender, EventArgs e)
        {
            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            //dsAll = objSql.GetDs("SELECT Top 20 * FROM AmazonAllProduct WHERE isnull(UPC,'') <> '' AND isnull(UPC,'') NOT in (SELECT isnull(UPC,'') FROM tb_product WHERE Storeid=3) AND Isnull(ImageURl,'') <>''");

            string strSqlname = "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT tb_Product.UPC FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + ") UNION ";
            strSqlname += "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT isnull(UPC,'') tb_ProductVariantValue FROM tb_ProductVariantValue  WHERE isnull(upc,'') <> '' AND productId in (SELECT tb_Product.ProductId FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + "))";
            //dsAll = objSql.GetDs(@"SELECT isnull(UPC,'') FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT tb_Product.UPC FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + ")");
            dsAll = objSql.GetDs(strSqlname);
            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {


                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    Int32 PIds = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 ProductId FROM tb_product WHERE StoreId=3 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'"));
                    if (PIds > 0)
                    {
                        DataSet dsnew = new DataSet();

                        dsnew = CommonComponent.GetCommonDataSet("SELECT tb_product.Price,tb_ProductVariantValue.BasecustomPrice,tb_product.ProductId,isnull(tb_product.Imagename,'') as Imagename FROM tb_product INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.ProductID=tb_product.ProductID WHERE tb_product.StoreId=1 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND  tb_ProductVariantValue.UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");

                        if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                        {

                            string imagename = "";
                            if (!string.IsNullOrEmpty(dsnew.Tables[0].Rows[0]["Imagename"].ToString()))
                            {
                                if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString())))
                                {
                                    try
                                    {
                                        imagename = dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "_" + PIds.ToString() + ".jpg";

                                        for (int imgnew = 1; imgnew < 10; imgnew++)
                                        {
                                            if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg")))
                                            {
                                                File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                                File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/Medium/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                                File.Copy(Server.MapPath("/Resources/halfpricedraps/product/icon/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                            }
                                        }
                                    }
                                    catch
                                    {

                                    }
                                }
                            }



                            continue;
                        }
                    }
                }
            }
        }

        protected void Button3_Click(object sender, EventArgs e)
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_TempImagerename_New");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    CommonComponent.ExecuteCommonData("Update tb_TempImagerename_New SET  ProductURL= '" + RemoveSpecialCharacter(Convert.ToString(ds.Tables[0].Rows[i]["ProductURL"]).ToCharArray()) + ".html' WHERE ImageName='" + ds.Tables[0].Rows[i]["ImageName"].ToString() + "'");
                }
            }
        }

        protected void Button4_Click(object sender, EventArgs e)
        {

            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            //dsAll = objSql.GetDs("SELECT Top 20 * FROM AmazonAllProduct WHERE isnull(UPC,'') <> '' AND isnull(UPC,'') NOT in (SELECT isnull(UPC,'') FROM tb_product WHERE Storeid=3) AND Isnull(ImageURl,'') <>''");

            string strSqlname = "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT tb_Product.UPC FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + ") UNION ";
            strSqlname += "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT isnull(UPC,'') tb_ProductVariantValue FROM tb_ProductVariantValue  WHERE isnull(upc,'') <> '' AND productId in (SELECT tb_Product.ProductId FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + "))";

            //string strSqlname = "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT Isnull(UPC,'') FROM tb_Product WHERE StoreId=4 AND " + txttablename.Text.ToString() + " AND isnull(Active,0)=1 And isnull(Deleted,0)=0) ";
            //strSqlname += "SELECT * FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT isnull(UPC,'') tb_ProductVariantValue FROM tb_ProductVariantValue  WHERE isnull(upc,'') <> '' AND productId in (SELECT tb_Product.ProductId FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + "))";




            //dsAll = objSql.GetDs(@"SELECT isnull(UPC,'') FROM AmazonAllProduct WHERE isnull(UPC,'') IN (SELECT tb_Product.UPC FROM dbo.tb_ProductCategory INNER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID INNER JOIN dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_ProductCategory.CategoryID=" + txtCatId.Text.ToString() + ")");
            dsAll = objSql.GetDs(strSqlname);
            if (dsAll != null && dsAll.Tables[0].Rows.Count > 0)
            {


                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    Int32 PIds = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 ProductId FROM tb_product WHERE StoreId=7 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'"));
                    if (PIds > 0)
                    {
                        //DataSet dsnew = new DataSet();

                        //dsnew = CommonComponent.GetCommonDataSet("SELECT tb_product.Price,tb_ProductVariantValue.BasecustomPrice,tb_product.ProductId,isnull(tb_product.Imagename,'') as Imagename FROM tb_product INNER JOIN tb_ProductVariantValue ON tb_ProductVariantValue.ProductID=tb_product.ProductID WHERE tb_product.StoreId=1 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND  tb_ProductVariantValue.UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");

                        //if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                        //{

                        //    string imagename = "";
                        //    if (!string.IsNullOrEmpty(dsnew.Tables[0].Rows[0]["Imagename"].ToString()))
                        //    {
                        //        if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString())))
                        //        {
                        //            try
                        //            {
                        //                imagename = dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "_" + PIds.ToString() + ".jpg";

                        //                for (int imgnew = 1; imgnew < 10; imgnew++)
                        //                {
                        //                    if (File.Exists(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg")))
                        //                    {
                        //                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                        //                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/large/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/Medium/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                        //                        File.Copy(Server.MapPath("/Resources/halfpricedraps/product/icon/" + dsnew.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                        //                    }
                        //                }
                        //            }
                        //            catch
                        //            {

                        //            }
                        //        }
                        //    }

                        //   /// CommonComponent.ExecuteCommonData("UPDATE tb_product SET Imagename='" + imagename + "' WHERE  tb_product.StoreId=3 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");
                        //}
                        //                        string B1 = txtb1.Text.ToString();
                        //                        string B2 = txtb2.Text.ToString();
                        //                        string B3 = txtb3.Text.ToString();
                        //                        string B4 = txtb4.Text.ToString();
                        //                        string B5 = txtb5.Text.ToString();

                        //                        CommonComponent.ExecuteCommonData(@"INSERT INTO  tb_ProductAmazon (AmazonRefID, StandardProductID, ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                        //                                                 BulletPoint1, BulletPoint2, BulletPoint3, BulletPoint4, BulletPoint5, ProductDescription, ProductType, ItemType, Prop65, CPSIAWarning1, CPSIAWarning2, 
                        //                                                 CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                        //                                                 PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                        //                                                 OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                        //                                                 FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                        //                                                 RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                        //                                                 [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                        //                                                 RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                        //                                                 SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType) 
                        //                                                 SELECT        " + PIds.ToString() + @", '" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + @"', ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                        //                                                 '" + B1 + @"', '" + B2 + @"', '" + B3 + @"', '" + B4 + @"', '" + B5 + @"', ProductDescription, ProductType, ItemType, Prop65, CPSIAWarning1, CPSIAWarning2, 
                        //                                                 CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                        //                                                 PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                        //                                                 OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                        //                                                 FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                        //                                                 RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                        //                                                 [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                        //                                                 RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                        //                                                 SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType
                        //                                                 FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");
                        //FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");

                        continue;
                    }
                    DataSet dsOverStockData = new DataSet();
                    dsOverStockData = CommonComponent.GetCommonDataSet("SELECT ProductId,isnull(Imagename,'') as Imagename FROM tb_product WHERE StoreId=3 AND isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");
                    string B1 = txtb1.Text.ToString();
                    string B2 = txtb2.Text.ToString();
                    string B3 = txtb3.Text.ToString();
                    string B4 = txtb4.Text.ToString();
                    string B5 = txtb5.Text.ToString();

                    if (dsOverStockData != null && dsOverStockData.Tables.Count > 0 && dsOverStockData.Tables[0].Rows.Count > 0)
                    {
                        Int32 PI = 0;
                        PI = Convert.ToInt32(CommonComponent.GetScalarCommonData(@"INSERT INTO tb_Product( StoreID, ManufactureID, DistributorID, TaxClassID, ProductTypeID, ProductTypeDeliveryID, QuantityDiscountID, VendorID, Name, 
                         SEName, SKU, Price, SalePrice, OurPrice, SurCharge, Inventory, Weight, Size, Color, Description, UPC, Height, Width, Length, Avail, AvaliableStartDate,
                         AvailableEndDate, ImageName, PDFName, Discontinue, MainCategory, IsKit, IsPack, IsFeatured, IsFreeShipping, IsNewArrival, IsSaleclearance, 
                         IsAuthorizeRefund, IsBestSeller, OptionalAccessories, OutOfStockMessage, SEKeywords, SEDescription, SETitle, ToolTip, DisplayOrder, GiftWrap, 
                         CallUsForPrice, isSatisfactionGuaranteed, isTabbingDisplay, TabTitle1, TabTitle2, TabDesc2, TabTitle3, TabDesc3, TabTitle4, TabDesc4, TabTitle5, 
                         TabDesc5, ViewCount, LastViewDate, Active, Deleted, CreatedBy, CreatedOn, UpdatedBy, UpdatedOn, RelatedProduct, TagName, eBayListingType, 
                         eBayListingDay, eBayCreatedOn, eBayLastUpdated, eBayListingFee, eBayProductID, eBayCategoryID, eBayStoreCategoryID, IsRestricted, Perishable, 
                         RequiresRefrigeration, RequiresFreezing, ContainsAlcohol, MapPriceIndicator, ShippingStartDate, ShippingEndDate, ManufacturePartNo, Summary, SellerID, 
                         Isbn, ASIN, Srno, ProductSetID, BuyListingID, Features, [Extended-warranty], BuyCategoryID, Location, Condition, IsShipSeparately, ManuItemURL, 
                         WebSiteShortTitle, WebSiteLongTitle, ItemPackage, ActivationMark, Prop65, Pro65Motherboard, Age18Verification, ChokingHazard1, ChokingHazard2, 
                         ChokingHazard3, ChokingHazard4, SellerPartNo, Shipping, BuyKeywords, Options, IsFreeEngraving, EngravingSize, OldProductID, IsSpecials, ReturnPolicy, 
                         amazonproductid, ManufactureCatalogNo, Brand, Bulletpoint1, Bulletpoint2, Bulletpoint3, Bulletpoint4, Bulletpoint5, ProductURL, YahooID, Dimension, 
                         Taxable, Ypath, LowInventory, MarryProducts, InventoryUpdatedOn, DiscontinuedOn, ImageOrg, Visualproperties, Image1, Image2, Image3, Image4, Image5, 
                         overstockproductid, OptionSku, EbayCreated, ContainsTobacco, Sizes, ShortName, PartNumber, Materials, WarrantyProvider, WarrantyDescription, 
                         WarrantyContactPhoneNumber, ProductSummary, ProductCondition, SourceZipCode, NeweggItemNumber, BuyReferenceID, PerInchPrice, AdditionalCharge, 
                         YardHeaderandhem, FabricInch, Isfreefabricswatch, IspriceQuote, ProductSwatchid, Isproperty, LightControl, Privacy, Efficiency, Pattern, Colors, Fabric, Style, 
                         SecondaryColor, SwatchDescription, ShippingTime, FabricType, FabricCode, IsCustom, Header, PageURL, ColorSku, Ismadetoorder, Ismadetomeasure, 
                         IsRoman, RomanShadeId, SalePriceTag, IsNewArrivalFromDate, IsNewArrivalToDate, IsBestSellerFromDate, IsBestSellerToDate, IsFreeShippingFromDate, 
                         IsFreeShippingToDate, IsFeaturedFromDate, IsFeaturedToDate, ImageDescription, FeatureID, Ismadetoready, Ismadetoorderswatch, IsdropshipProduct, 
                         IsDataVerify, DataVerifyBy, IsDataVerifyOn, ItemType, IsHamming, HammingSafetyQty, HammingSafetyper) 
                         SELECT 7, NULL,  NULL, NULL, NULL, ProductTypeDeliveryID, QuantityDiscountID, VendorID,  Name, 
                         '" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("'", "''").Replace(" ", "-").ToLower() + @"', '" + dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + @"',Price, SalePrice, OurPrice, SurCharge, Inventory, case when isnull(Weight,0)=0 then 1 else Weight end as Weight, Size, '" + dsAll.Tables[0].Rows[i]["Color"].ToString().Replace("'", "''") + "', Description, '" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + @"', Height, Width, Length, Avail, AvaliableStartDate, 
                         AvailableEndDate, '', '', Discontinue, '', IsKit, IsPack, IsFeatured, 0, IsNewArrival, IsSaleclearance, 
                         IsAuthorizeRefund, IsBestSeller, OptionalAccessories, OutOfStockMessage, SEKeywords, SEDescription, '" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("'", "''") + @"', '" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + @"', DisplayOrder, GiftWrap, 
                         CallUsForPrice, isSatisfactionGuaranteed, isTabbingDisplay, TabTitle1, TabTitle2, TabDesc2, TabTitle3, TabDesc3, TabTitle4, TabDesc4, TabTitle5, 
                         TabDesc5, ViewCount, LastViewDate, Active, Deleted, CreatedBy, CreatedOn, UpdatedBy, getdate(), RelatedProduct, TagName, 0, 
                         30, getdate(), eBayLastUpdated, 0.00, eBayProductID, 45515, 3451471014, IsRestricted, Perishable, 
                         RequiresRefrigeration, RequiresFreezing, ContainsAlcohol, MapPriceIndicator, ShippingStartDate, ShippingEndDate,'" + dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + @"', Summary, SellerID, 
                         Isbn, ASIN, Srno, ProductSetID, BuyListingID, Features, [Extended-warranty], BuyCategoryID, Location, Condition, IsShipSeparately, ManuItemURL, 
                         WebSiteShortTitle, WebSiteLongTitle, ItemPackage, ActivationMark, Prop65, Pro65Motherboard, Age18Verification, ChokingHazard1, ChokingHazard2, 
                         ChokingHazard3, ChokingHazard4, SellerPartNo, Shipping, BuyKeywords, Options, IsFreeEngraving, EngravingSize, OldProductID, IsSpecials, ReturnPolicy, 
                         '', ManufactureCatalogNo, 'Curtains on Budget', Bulletpoint1, Bulletpoint2, Bulletpoint3, Bulletpoint4, Bulletpoint5, ProductURL, YahooID, Dimension, 
                         Taxable, Ypath, LowInventory, MarryProducts, InventoryUpdatedOn, DiscontinuedOn, ImageOrg, Visualproperties, Image1, Image2, Image3, Image4, Image5, 
                         '', '', EbayCreated, ContainsTobacco, Sizes, ShortName, PartNumber, Materials, WarrantyProvider, WarrantyDescription, 
                         WarrantyContactPhoneNumber, ProductSummary, ProductCondition, SourceZipCode, NeweggItemNumber, BuyReferenceID, PerInchPrice, AdditionalCharge, 
                         YardHeaderandhem, FabricInch, Isfreefabricswatch, IspriceQuote, ProductSwatchid, Isproperty, LightControl, Privacy, Efficiency, Pattern, '" + dsAll.Tables[0].Rows[i]["Color"].ToString().Replace("'", "''") + @"', Fabric, Style, 
                         SecondaryColor, SwatchDescription, ShippingTime, FabricType, FabricCode, IsCustom, Header, PageURL, ColorSku, Ismadetoorder, Ismadetomeasure, 
                         IsRoman, RomanShadeId, SalePriceTag, IsNewArrivalFromDate, IsNewArrivalToDate, IsBestSellerFromDate, IsBestSellerToDate, IsFreeShippingFromDate, 
                         IsFreeShippingToDate, IsFeaturedFromDate, IsFeaturedToDate, ImageDescription, FeatureID, Ismadetoready, Ismadetoorderswatch, IsdropshipProduct, 
                         IsDataVerify, DataVerifyBy, IsDataVerifyOn, ItemType, IsHamming, HammingSafetyQty, HammingSafetyper FROM tb_Product WHERE ProductId=" + dsOverStockData.Tables[0].Rows[0]["ProductId"] + @"; SELECT SCOPE_IDENTITY(); "));
                        CommonComponent.ExecuteCommonData(@"INSERT INTO  tb_WareHouseProductInventory(WareHouseID, ProductID, Inventory, PreferredLocation) SELECT WareHouseID, " + PI.ToString() + @", Inventory, PreferredLocation FROM tb_WareHouseProductInventory WHERE ProductId=" + dsOverStockData.Tables[0].Rows[0]["ProductId"].ToString() + @"");


                        //                        CommonComponent.ExecuteCommonData(@"INSERT INTO  tb_ProductAmazon (AmazonRefID, StandardProductID, ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                        //                         BulletPoint1, BulletPoint2, BulletPoint3, BulletPoint4, BulletPoint5, ProductDescription, ProductType, ItemType, Prop65, CPSIAWarning1, CPSIAWarning2, 
                        //                         CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                        //                         PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                        //                         OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                        //                         FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                        //                         RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                        //                         [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                        //                         RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                        //                         SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType) 
                        //                         SELECT        " + PI + @", '" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + @"', ProductIDType, Brand, Manufacturer, ModelName, ModelNumber, MfrPartNumber, MerchantCatalogNumber, 
                        //                         '" + B1 + @"', '" + B2 + @"', '" + B3 + @"', '" + B4 + @"', '" + B5 + @"', ProductDescription, ProductType, '" + B2.ToString() + @"', Prop65, CPSIAWarning1, CPSIAWarning2, 
                        //                         CPSIAWarning3, CPSIAWarning4, CPSIAWarningDescription, Keyword1, Keyword2, Keyword3, PlatinumKeywords1, PlatinumKeywords2, PlatinumKeywords3, 
                        //                         PlatinumKeywords4, PlatinumKeywords5, MainImageURL, OtherImageURL1, OtherImageURL2, OtherImageURL3, OtherImageURL4, OtherImageURL5, 
                        //                         OtherImageURL6, OtherImageURL7, OtherImageURL8, ParentSKU, RelationshipType, ProductTaxCode, LaunchDate, ReleaseDate, MAP, MSRP, Currency, 
                        //                         FulfillmentCenterID, SaleStartDate, SaleEndDate, RebateStartDate1, RebateStartDate2, RebateEndDate1, RebateEndDate2, RebateMessage1, 
                        //                         RebateMessage2, RebateName1, RebateName2, MaxOrderQuantity, LeadtimeToShip, RestockDate, ItemsIncluded, shipping_weight_unit_of_measure, 
                        //                         [shipping-weight], ConditionType, ConditionNote, MaxAggregateShipQuantity, IsGiftWrapAvailable, IsGiftMessageAvailable, IsDiscontinuedByManufacturer, 
                        //                         RegisteredParameter, UpdateDelete, SS_SurveillanceSystemType, SS_CameraType, SS_Features1, SS_Features2, SS_Features3, SS_Features4, 
                        //                         SS_Features5, SS_CameraAccessories, PoS_ForUseWith, PoS_BatteryChemicalType, PoS_CameraPowerSupplyType
                        //                         FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");
                        //FROM  tb_ProductAmazon  WHERE AmazonRefID=2896");

                        string imagename = "";
                        if (!string.IsNullOrEmpty(dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString()))
                        {
                            try
                            {
                                if (File.Exists(Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString())))
                                {
                                    imagename = dsAll.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "_" + PI.ToString() + ".jpg";
                                    File.Copy(Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/halfpricedraps/product/large/" + imagename.ToString()), true);
                                    File.Copy(Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/halfpricedraps/product/Medium/" + imagename.ToString()), true);
                                    File.Copy(Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString()), Server.MapPath("/Resources/halfpricedraps/product/icon/" + imagename.ToString()), true);
                                    for (int imgnew = 1; imgnew < 10; imgnew++)
                                    {
                                        if (File.Exists(Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg")))
                                        {
                                            File.Copy(Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/halfpricedraps/product/large/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                            File.Copy(Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/large/" + dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/halfpricedraps/product/Medium/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                            File.Copy(Server.MapPath("/Resources/CurtainsonbudgetAmazon/product/icon/" + dsOverStockData.Tables[0].Rows[0]["Imagename"].ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), Server.MapPath("/Resources/halfpricedraps/product/icon/" + imagename.ToString().Replace(".jpg", "").Replace(".jpeg", "") + "_" + imgnew + ".jpg"), true);
                                        }
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }

                        CommonComponent.ExecuteCommonData("UPDATE tb_product SET Imagename='" + imagename + "' WHERE  tb_product.StoreId=7 AND isnull(tb_product.Active,0)=1 AND Isnull(tb_product.Deleted,0)=0 AND UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString() + "'");

                    }


                }
            }

        }

        protected void btnfabricvendor_Click(object sender, EventArgs e)
        {
            SQLAccess objSql = new SQLAccess();

            //DataSet dsAll = new DataSet();
            //string strSqlname = "SELECT * FROM MASTERData WHERE [Product SKu] like '%-FB'";
            //dsAll = objSql.GetDs(strSqlname);
            //if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
            //{
            //    for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
            //    {
            //        string strPid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 productId FROM tb_Product WHERE Storeid=1 AND isnull(active,0)=1 AND isnull(Deleted,0)=0 AND SKU like '%" + dsAll.Tables[0].Rows[i]["Product SKU"].ToString().Replace("-FB", "-") + "%' "));
            //        if (!string.IsNullOrEmpty(strPid))
            //        {
            //            string strFabricTypeid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 FabricTypeID FROM tb_ProductFabricType WHERE  isnull(Active,0)=1 AND  FabricTypename ='" + dsAll.Tables[0].Rows[i]["Prodcut Class/Group"].ToString() + "'"));
            //            if (string.IsNullOrEmpty(strFabricTypeid))
            //            {
            //                strFabricTypeid = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_ProductFabricType(Active,FabricTypename,DisplayOrder) VALUES (1,'" + dsAll.Tables[0].Rows[i]["Prodcut Class/Group"].ToString().Replace("'", "''") + "',0); SELECT SCOPE_IDENTITY();"));
            //            }

            //            string strFabricCodeid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 FabricCodeId FROM tb_ProductFabricCode WHERE  isnull(Active,0)=1 AND FabricTypeID=" + strFabricTypeid + " AND  Code ='" + dsAll.Tables[0].Rows[i]["Product SKU"].ToString().Replace("'", "''") + "'"));
            //            if (string.IsNullOrEmpty(strFabricCodeid))
            //            {
            //                strFabricCodeid = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_ProductFabricCode(Active,FabricTypeID,Name,Code,CreatedOn,DisplayOrder) VALUES (1," + strFabricTypeid + ",'" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["Product SKU"].ToString().Replace("'", "''") + "',getdate(),0); SELECT SCOPE_IDENTITY();"));
            //            }

            //            string strvendorids = "0";
            //            if (dsAll.Tables[0].Rows[i]["Preferred vendor"].ToString().IndexOf("/") > -1)
            //            {
            //                string[] strvndor = dsAll.Tables[0].Rows[i]["Preferred vendor"].ToString().Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            //                if (strvndor.Length > 0)
            //                {
            //                    foreach (string vv in strvndor)
            //                    {
            //                        strvendorids += "," + Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorID FROM tb_Vendor WHERE Name='" + vv.Replace("'", "''") + "'"));
            //                    }
            //                }
            //            }
            //            else
            //            {
            //                strvendorids = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorID FROM tb_Vendor WHERE Name='" + dsAll.Tables[0].Rows[i]["Preferred vendor"].ToString().Replace("'", "''") + "'"));
            //            }

            //            CommonComponent.ExecuteCommonData("UPDATE tb_Product SET FabricType='" + dsAll.Tables[0].Rows[i]["Prodcut Class/Group"].ToString().Replace("'", "''") + "',FabricCode='" + dsAll.Tables[0].Rows[i]["Product SKU"].ToString().Replace("'", "''") + "',FabricVendorIds='" + strvendorids + "' WHERE productId=" + strPid + "");
            //        }
            //    }
            //}


            DataSet dsAll = new DataSet();
            string strSqlname = "SELECT * FROM TempVendorSKU";
            dsAll = objSql.GetDs(strSqlname);
            if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    string strPid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 productId FROM tb_Product WHERE ProductId in (SELECT ProductID FROM tb_ProductCategory WHERE CategoryID in (SELECT CategoryID FROM tb_CategoryMapping WHERE " + txtCatId.Text.ToString() + ")) and  Storeid=1 and isnull(FabricCode,'') = '' and isnull(active,0)=1 AND isnull(Deleted,0)=0 AND SKU like '%" + dsAll.Tables[0].Rows[i]["Code"].ToString().Replace("-FB", "") + "%' "));
                    if (!string.IsNullOrEmpty(strPid))
                    {
                        string strFabricTypeid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 FabricTypeID FROM tb_ProductFabricType WHERE  isnull(Active,0)=1 AND  FabricTypename ='" + dsAll.Tables[0].Rows[i]["Category"].ToString().Replace("'", "''") + "'"));
                        if (string.IsNullOrEmpty(strFabricTypeid))
                        {
                            strFabricTypeid = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_ProductFabricType(Active,FabricTypename,DisplayOrder) VALUES (1,'" + dsAll.Tables[0].Rows[i]["Category"].ToString().Replace("'", "''") + "',0); SELECT SCOPE_IDENTITY();"));
                        }

                        string strFabricCodeid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 FabricCodeId FROM tb_ProductFabricCode WHERE  isnull(Active,0)=1 AND FabricTypeID=" + strFabricTypeid + " AND  Code ='" + dsAll.Tables[0].Rows[i]["Code"].ToString().Replace("'", "''") + "'"));
                        if (string.IsNullOrEmpty(strFabricCodeid))
                        {
                            strFabricCodeid = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_ProductFabricCode(Active,FabricTypeID,Name,Code,CreatedOn,DisplayOrder) VALUES (1," + strFabricTypeid + ",'" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["Code"].ToString().Replace("'", "''") + "',getdate(),0); SELECT SCOPE_IDENTITY();"));
                        }
                        else
                        {
                            CommonComponent.ExecuteCommonData("UPDATE tb_ProductFabricCode SET Name='" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + "' WHERE FabricCodeId=" + strFabricCodeid + "");
                        }


                        string strvendorids = "0";
                        if (!string.IsNullOrEmpty(dsAll.Tables[0].Rows[i]["VendorName"].ToString()) && dsAll.Tables[0].Rows[i]["VendorName"].ToString().IndexOf("/") > -1)
                        {
                            string[] strvndor = dsAll.Tables[0].Rows[i]["VendorName"].ToString().Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strvndor.Length > 0)
                            {
                                foreach (string vv in strvndor)
                                {
                                    strvendorids += "," + Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorID FROM tb_Vendor WHERE Name='" + vv.Replace("'", "''") + "'"));
                                }
                            }
                        }
                        else
                        {
                            if (!string.IsNullOrEmpty(dsAll.Tables[0].Rows[i]["VendorName"].ToString()))
                            {
                                strvendorids = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorID FROM tb_Vendor WHERE Name='" + dsAll.Tables[0].Rows[i]["VendorName"].ToString().Replace("'", "''") + "'"));
                            }
                        }

                        CommonComponent.ExecuteCommonData("UPDATE tb_Product SET FabricType='" + dsAll.Tables[0].Rows[i]["Category"].ToString().Replace("'", "''") + "',FabricCode='" + dsAll.Tables[0].Rows[i]["Code"].ToString().Replace("'", "''") + "',FabricVendorIds='" + strvendorids + "' WHERE productId=" + strPid + "");
                        //CommonComponent.ExecuteCommonData("UPDATE tb_ProductFabricCode SET FabricVendorIds='" + strvendorids + "' WHERE FabricCodeId=" + strFabricCodeid + "");
                    }
                    else
                    {
                        //string strFabricTypeid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 FabricTypeID FROM tb_ProductFabricType WHERE  isnull(Active,0)=1 AND  FabricTypename ='" + dsAll.Tables[0].Rows[i]["Category"].ToString().Replace("'", "''") + "'"));
                        //if (string.IsNullOrEmpty(strFabricTypeid))
                        //{
                        //    strFabricTypeid = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_ProductFabricType(Active,FabricTypename,DisplayOrder) VALUES (1,'" + dsAll.Tables[0].Rows[i]["Category"].ToString().Replace("'", "''") + "',0); SELECT SCOPE_IDENTITY();"));
                        //}

                        //string strFabricCodeid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 FabricCodeId FROM tb_ProductFabricCode WHERE  isnull(Active,0)=1 AND FabricTypeID=" + strFabricTypeid + " AND  Code ='" + dsAll.Tables[0].Rows[i]["Code"].ToString().Replace("'", "''") + "'"));
                        //if (string.IsNullOrEmpty(strFabricCodeid))
                        //{
                        //    strFabricCodeid = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_ProductFabricCode(Active,FabricTypeID,Name,Code,CreatedOn,DisplayOrder) VALUES (1," + strFabricTypeid + ",'" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["Code"].ToString().Replace("'", "''") + "',getdate(),0); SELECT SCOPE_IDENTITY();"));
                        //}
                        //else
                        //{
                        //    CommonComponent.ExecuteCommonData("UPDATE tb_ProductFabricCode SET Name='" + dsAll.Tables[0].Rows[i]["name"].ToString().Replace("'", "''") + "' WHERE FabricCodeId=" + strFabricCodeid + "");
                        //}


                        //string strvendorids = "0";
                        //if (dsAll.Tables[0].Rows[i]["VendorName"].ToString().IndexOf("/") > -1)
                        //{
                        //    string[] strvndor = dsAll.Tables[0].Rows[i]["VendorName"].ToString().Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //    if (strvndor.Length > 0)
                        //    {
                        //        foreach (string vv in strvndor)
                        //        {
                        //            strvendorids += "," + Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorID FROM tb_Vendor WHERE Name='" + vv.Replace("'", "''") + "'"));
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    strvendorids = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorID FROM tb_Vendor WHERE Name='" + dsAll.Tables[0].Rows[i]["VendorName"].ToString().Replace("'", "''") + "'"));
                        //}

                        //CommonComponent.ExecuteCommonData("UPDATE tb_ProductFabricCode SET FabricVendorIds='" + strvendorids + "' WHERE FabricCodeId=" + strFabricCodeid + "");
                    }
                }
            }

        }
        protected void btnCustomSKu_Click(object sender, EventArgs e)
        {
            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            string strSqlname = "SELECT * FROM MASTERData_new WHERE [Product SKu] like '%-CUS' and [Product URLS] like '%http://www.halfprice%'";
            dsAll = objSql.GetDs(strSqlname);
            if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    string strPid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 productId FROM tb_Product WHERE ProductID  ProductId in (SELECT ProductID FROM tb_ProductCategory WHERE CategoryID in (SELECT CategoryID FROM tb_CategoryMapping WHERE ParentCategoryID=" + txtCatId.Text.ToString() + "))  AND Storeid=1 AND isnull(active,0)=1 AND isnull(Deleted,0)=0 AND ProductUrl like '%" + dsAll.Tables[0].Rows[i]["Product URLS"].ToString().Replace("http://www.halfpricedrapes.com/", "") + "%' "));
                    if (!string.IsNullOrEmpty(strPid))
                    {
                        //string strFabricTypeid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 FabricTypeID FROM tb_ProductFabricType WHERE  isnull(Active,0)=1 AND  FabricTypename ='" + dsAll.Tables[0].Rows[i]["Prodcut Class/Group"].ToString() + "'"));
                        //if (string.IsNullOrEmpty(strFabricTypeid))
                        //{
                        //    strFabricTypeid = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_ProductFabricType(Active,FabricTypename,DisplayOrder) VALUES (1,'" + dsAll.Tables[0].Rows[i]["Prodcut Class/Group"].ToString().Replace("'", "''") + "',0); SELECT SCOPE_IDENTITY();"));
                        //}

                        //string strFabricCodeid = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 FabricCodeId FROM tb_ProductFabricCode WHERE  isnull(Active,0)=1 AND FabricTypeID=" + strFabricTypeid + " AND  Code ='" + dsAll.Tables[0].Rows[i]["Product SKU"].ToString().Replace("'", "''") + "'"));
                        //if (string.IsNullOrEmpty(strFabricCodeid))
                        //{
                        //    strFabricCodeid = Convert.ToString(CommonComponent.GetScalarCommonData("INSERT INTO tb_ProductFabricCode(Active,FabricTypeID,Name,Code,CreatedOn,DisplayOrder) VALUES (1," + strFabricTypeid + ",'" + dsAll.Tables[0].Rows[i]["Product Name"].ToString().Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["Product SKU"].ToString().Replace("'", "''") + "',getdate(),0); SELECT SCOPE_IDENTITY();"));
                        //}

                        //string strvendorids = "0";
                        //if (dsAll.Tables[0].Rows[i]["Preferred vendor"].ToString().IndexOf("/") > -1)
                        //{
                        //    string[] strvndor = dsAll.Tables[0].Rows[i]["Preferred vendor"].ToString().Split("/".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //    if (strvndor.Length > 0)
                        //    {
                        //        foreach (string vv in strvndor)
                        //        {
                        //            strvendorids += "," + Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorID FROM tb_Vendor WHERE Name='" + vv.Replace("'", "''") + "'"));
                        //        }
                        //    }
                        //}
                        //else
                        //{
                        //    strvendorids = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VendorID FROM tb_Vendor WHERE Name='" + dsAll.Tables[0].Rows[i]["Preferred vendor"].ToString().Replace("'", "''") + "'"));
                        //}

                        CommonComponent.ExecuteCommonData("UPDATE tb_ProductVariantValue SET SKU='" + dsAll.Tables[0].Rows[i]["Product SKU"].ToString().Replace("'", "''") + "',UPC='" + dsAll.Tables[0].Rows[i]["UPC"].ToString().Replace("'", "''") + "' WHERE VariantValue='Custom Size' AND productId=" + strPid + "");
                    }
                }
            }
        }
        protected void btnRoman_Click(object sender, EventArgs e)
        {
            SQLAccess objSql = new SQLAccess();

            DataSet dsAll = new DataSet();
            string strSqlname = "SELECT * FROM HPDMaster_20Dec_Roman";
            dsAll = objSql.GetDs(strSqlname);
            string strCode = "";
            if (dsAll != null && dsAll.Tables.Count > 0 && dsAll.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsAll.Tables[0].Rows.Count; i++)
                {
                    if (strCode != dsAll.Tables[0].Rows[i]["MasterCode"].ToString())
                    {
                        Int32 newP = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_product(Name,SKU,UPC,Price,SalePrice,ProductURL,Active,Deleted,StoreId,Discontinue,Inventory,IsRoman,RomanShadeId) VALUES ('" + dsAll.Tables[0].Rows[i]["Name"].ToString().Replace("Casual ", "").Replace(" Soft Fold", "").Replace("Front Slat ", "").Replace(" Relaxed", "").Replace("'", "''") + "','" + dsAll.Tables[0].Rows[i]["MasterCode"].ToString() + "','','0.00','0.00','',1,0,1,0,0,1,0); SELECT SCOPE_IDENTITY();"));
                        objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductInventory(WareHouseID,ProductID,Inventory,PreferredLocation) VALUES (1," + newP + ", 0,1)");
                        Int32 pvariantId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariant(ProductID, VariantName,IsParent, ParentId, DisplayOrder) VALUES (" + newP + ",'Roman Shade Design',0,0,1);  SELECT SCOPE_IDENTITY();"));

                        Int32 PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'Casual','0',1," + newP + ",'" + dsAll.Tables[0].Rows[i]["MasterCode"].ToString() + "-CAS','','',10,0,10,0,0,'0.00'); SELECT SCOPE_IDENTITY();"));
                        objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + newP + "," + pvariantId + "," + PvalueId + ",10,1)");

                        PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                                " values (" + pvariantId + ",'Soft Fold','0',2," + newP + ",'" + dsAll.Tables[0].Rows[i]["MasterCode"].ToString() + "-SFL','','',10,0,10,0,0,'0.00'); SELECT SCOPE_IDENTITY();"));
                        objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + newP + "," + pvariantId + "," + PvalueId + ",10,1)");

                        PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                               " values (" + pvariantId + ",'Front Slat','0',3," + newP + ",'" + dsAll.Tables[0].Rows[i]["MasterCode"].ToString() + "-FSL','','',10,0,10,0,0,'0.00'); SELECT SCOPE_IDENTITY();"));
                        objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + newP + "," + pvariantId + "," + PvalueId + ",10,1)");

                        PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                              " values (" + pvariantId + ",'Relaxed','0',4," + newP + ",'" + dsAll.Tables[0].Rows[i]["MasterCode"].ToString() + "-RLX','','',10,0,10,0,0,'0.00'); SELECT SCOPE_IDENTITY();"));
                        objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + newP + "," + pvariantId + "," + PvalueId + ",10,1)");

                        pvariantId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariant(ProductID, VariantName,IsParent, ParentId, DisplayOrder) VALUES (" + newP + ",'Select Fabric Color',0,0,2);  SELECT SCOPE_IDENTITY();"));

                        string strode = dsAll.Tables[0].Rows[i]["MasterCode"].ToString().Substring(0, dsAll.Tables[0].Rows[i]["MasterCode"].ToString().IndexOf("-"));
                        DataSet dsAllCode = new DataSet();
                        strSqlname = "SELECT * FROM RomanFabric_Code WHERE isnull(fabricCode,'') like '" + strode + "-%'";
                        dsAllCode = objSql.GetDs(strSqlname);
                        if (dsAllCode != null && dsAllCode.Tables.Count > 0 && dsAllCode.Tables[0].Rows.Count > 0)
                        {
                            for (int j = 0; j < dsAllCode.Tables[0].Rows.Count; j++)
                            {

                                string FabricType = Convert.ToString(objSql.ExecuteScalarQuery("SELECT top 1 dbo.tb_ProductFabricType.FabricTypename FROM dbo.tb_ProductFabricType INNER JOIN dbo.tb_ProductFabricCode ON dbo.tb_ProductFabricType.FabricTypeID = dbo.tb_ProductFabricCode.FabricTypeID WHERE dbo.tb_ProductFabricCode.Code='" + dsAllCode.Tables[0].Rows[j]["FabricCode"].ToString().Replace("'", "''") + "'"));

                                PvalueId = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_ProductVariantValue (FabricType,FabricCode,VariantID, VariantValue, VariantPrice, DisplayOrder, ProductID, SKU, UPC, Header, Inventory, RelatedProductid, AllowQuantity, LockQuantity, AddiHemingQty, BasecustomPrice)" +
                            " values ('" + FabricType.ToString() + "','" + dsAllCode.Tables[0].Rows[j]["FabricCode"].ToString() + "'," + pvariantId + ",'" + dsAllCode.Tables[0].Rows[j]["FabricName"].ToString() + "','0'," + (j + 1).ToString() + "," + newP + ",'','','',10,0,10,0,0,'0.00'); SELECT SCOPE_IDENTITY();"));
                                objSql.ExecuteNonQuery("INSERT INTO tb_WareHouseProductVariantInventory(WareHouseID, ProductID, VariantID, VariantValueID, Inventory,PreferredLocation) VALUES (1," + newP + "," + pvariantId + "," + PvalueId + ",10,1)");
                            }
                        }
                    }
                    strCode = dsAll.Tables[0].Rows[i]["MasterCode"].ToString();
                }
            }
        }
        protected void btnSearchpattern_Click(object sender, EventArgs e)
        {
            if (File.Exists(Server.MapPath("/catalog.xml")))
            {
                DataSet dsXMl = new DataSet();
                //dsXMl.ReadXml(Server.MapPath("/catalog.xml"));
                XmlDocument _xmlDocument = new XmlDocument();
                _xmlDocument.Load(Server.MapPath("/catalog.xml"));

                //Select the element with in the xml you wish to extract;
                XmlNodeList _nodeList = _xmlDocument.SelectNodes("Catalog/Item");
                SQLAccess objSql = new SQLAccess();
                foreach (XmlNode _node in _nodeList)
                {
                    string strproductyahooid = "";
                    strproductyahooid = _node.Attributes[0].Value.ToString();
                    // if (strproductyahooid == "flanders-multi-jacquard-curtain")
                    //if (strproductyahooid == "cappuccino-faux-stripe-taffeta-curtains-drapes")
                    //{

                    //}
                    //else
                    //{
                    //    continue;
                    //}
                    //Int32 strurl = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count([Product Name]) FROM MASTERData WHERE [Product URLS] like '%" + strproductyahooid + ".html%'"));
                    Int32 strurl = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT  Count(productId) FROM tb_Product WHERE  ProductId in (SELECT ProductID FROM tb_ProductCategory WHERE CategoryID in (SELECT CategoryID FROM tb_CategoryMapping WHERE ParentCategoryID " + txtCatId.Text.ToString() + ")) and  Storeid=1 and isnull(active,0)=1 AND isnull(Deleted,0)=0  and BackupProductUrl like '%" + strproductyahooid + ".html%'"));

                    // Int32 strurl = Convert.ToInt32(objSql.ExecuteScalarQuery("SELECT Count(ProductURL) FROM tb_TempImagerename_New WHERE ProductURL like '%" + strproductyahooid + ".html%'"));
                    // string strcode = Convert.ToString(objSql.ExecuteScalarQuery("SELECT TOP 1 MASTERData_New.[Product SKU] FROM MASTERData_New INNER JOIN MASTERData ON  MASTERData_New.[Old Product URLS]=MASTERData.[Product URLS]  WHERE  MASTERData_New.[Product URLS] like '%/" + strproductyahooid + ".html%' AND  MASTERData_New.[Main Category] in ('Designer Silk Fabric Curtains','Designer Faux Silk Fabric Curtains','Solid Silk Fabric Curtains') "));
                    if (strurl > 0)
                    //if (strcode != "")
                    {
                        XmlNodeList list = _node.ChildNodes;
                        // string strcode = "";
                        string strstartprice = "";
                        string strprice = "0";
                        string strsaleprice = "0";
                        string strdescription = "";
                        string strydproductfeatures = "";

                        string searchpattern = "";
                        string style = "";
                        
                        string searchfabric = "";
                        string primaryheader = "";
                        string searchcolor = "";
                        

                        string strproductshippingtime = "";
                        string strprice1 = "";
                        string strprice2 = "";
                        string strprice3 = "";
                        string strprice4 = "";


                        string strimg = "";
                        string strimg1 = "";
                        string strimg2 = "";
                        string strimg3 = "";
                        string strimg4 = "";


                        string ydprivacy = "";
                        string ydlightcontrol = "";
                        string ydefficiency = "";
                        string shipweight = "";
                        Decimal strprice11 = 0;
                        Decimal strprice22 = 0;
                        Decimal strprice33 = 0;
                        Decimal strprice44 = 0;

                        Decimal strsaleprice11 = 0;
                        Decimal strsaleprice22 = 0;
                        Decimal strsaleprice33 = 0;
                        Decimal strsaleprice44 = 0;

                        string strimage = "";
                        for (int j = 0; j < _node.ChildNodes.Count; j++)
                        {

                            if (list[j].Name == "ItemField")
                            {
                                //list[j].Attributes[0].Value.ToString();
                                // Response.Write(list[j].Attributes[0].Name.ToString()+" "+list[j].Attributes[0].Value.ToString());
                                //Response.Write(list[j].Attributes[1].Name.ToString()+" "+list[j].Attributes[1].Value.ToString());
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "code")
                                {
                                    //strcode = list[j].Attributes[1].Value.ToString();
                                }
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "image")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "more-image-1")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg1 = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "more-image-2")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg2 = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "more-image-3")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg3 = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "more-image-4")
                                //{
                                //    if (!string.IsNullOrEmpty(list[j].Attributes[1].Value.ToString()))
                                //    {
                                //        strimg4 = list[j].Attributes[1].Value.ToString().Substring(list[j].Attributes[1].Value.ToString().IndexOf("src=") + 4, list[j].Attributes[1].Value.ToString().IndexOf(">") - list[j].Attributes[1].Value.ToString().IndexOf("src=") - 4);
                                //    }
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "price")
                                //{
                                //    strprice = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "start-price")
                                //{
                                //    strstartprice = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "sale-price")
                                //{
                                //    strsaleprice = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-searchable-content")
                                //{
                                //    strdescription = list[j].Attributes[1].Value.ToString();
                                //}
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "search-pattern")
                                {
                                    searchpattern = list[j].Attributes[1].Value.ToString();
                                }
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "style")
                                {
                                    style = list[j].Attributes[1].Value.ToString();
                                }
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "search-fabric")
                                {
                                    searchfabric = list[j].Attributes[1].Value.ToString();
                                }
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "primary-header")
                                {
                                    primaryheader = list[j].Attributes[1].Value.ToString();
                                }
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "search-color")
                                {
                                    searchcolor = list[j].Attributes[1].Value.ToString();
                                }
                                
                                
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-privacy")
                                {
                                    ydprivacy = list[j].Attributes[1].Value.ToString();
                                }
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-light-control")
                                {
                                    ydlightcontrol = list[j].Attributes[1].Value.ToString();
                                }
                                if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-efficiency")
                                {
                                    ydefficiency = list[j].Attributes[1].Value.ToString();
                                }
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "yd-product-features")
                                //{
                                //    strydproductfeatures = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "product-shipping-time")
                                //{
                                //    strproductshippingtime = list[j].Attributes[1].Value.ToString();
                                //}
                                //if (list[j].Attributes[0].Value.ToString().ToLower() == "ship-weight")
                                //{
                                //    shipweight = list[j].Attributes[1].Value.ToString();
                                //}

                            }
                            if (list[j].Name == "ItemFieldOptions")
                            {
                                XmlNodeList _nodeList1 = list[j].ChildNodes;
                                for (int k = 0; k < list[j].ChildNodes.Count; j++)
                                {
                                    XmlNodeList _nodeList2 = _nodeList1[k].ChildNodes;
                                    for (int p = 0; p < _nodeList1[k].ChildNodes.Count; p++)
                                    {


                                        //if (_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") > -1)
                                        //{
                                        //    Response.Write(_nodeList2[p].Attributes[0].Value.ToString());
                                        //    if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51w") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51w") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51w") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51w") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100w") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 w") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25w") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }





                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51x") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51x") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51x") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("50x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("51x") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("100x") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x84") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 84") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1))
                                        //    {
                                        //        strprice1 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x96") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 96") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1))
                                        //    {
                                        //        strprice2 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x108") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 108") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1))
                                        //    {
                                        //        strprice3 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }
                                        //    else if ((_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x120") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("x 120") > -1) && (_nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25 x") > -1 || _nodeList2[p].Attributes[0].Value.ToString().ToLower().IndexOf("25x") > -1))
                                        //    {
                                        //        strprice4 = _nodeList2[p].Attributes[0].Value.ToString().Substring(_nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") + 3, _nodeList2[p].Attributes[0].Value.ToString().LastIndexOf(")") - _nodeList2[p].Attributes[0].Value.ToString().IndexOf("(+$") - 3);
                                        //    }


                                        //}

                                    }
                                }
                            }


                        }
                        //if (strstartprice != "")
                        //{
                        //    strprice = strstartprice;
                        //}
                        //else if (strsaleprice != "" && strprice == "")
                        //{
                        //    strprice = strsaleprice;
                        //}
                        //if (strsaleprice == "")
                        //{
                        //    strsaleprice = strprice;
                        //}
                        //try
                        //{
                        //    if (strprice1 == "")
                        //    {
                        //        strprice1 = strprice;
                        //        strprice11 = Convert.ToDecimal(strprice1);
                        //        strsaleprice11 = Convert.ToDecimal(strsaleprice);
                        //    }
                        //    else
                        //    {
                        //        strprice11 = Convert.ToDecimal(strprice1) + Convert.ToDecimal(strprice);
                        //        strsaleprice11 = Convert.ToDecimal(strsaleprice) + Convert.ToDecimal(strprice1);
                        //        //strprice11 = Convert.ToDecimal(strprice);
                        //        //strsaleprice11 = Convert.ToDecimal(strprice1);
                        //    }
                        //    if (strprice2 == "")
                        //    {
                        //        strprice2 = strprice;
                        //        strprice22 = Convert.ToDecimal(strprice2);
                        //        strsaleprice22 = Convert.ToDecimal(strsaleprice);
                        //    }
                        //    else
                        //    {
                        //        strprice22 = Convert.ToDecimal(strprice2) + Convert.ToDecimal(strprice);
                        //        strsaleprice22 = Convert.ToDecimal(strsaleprice) + Convert.ToDecimal(strprice2);

                        //        //strprice22 = Convert.ToDecimal(strprice);
                        //        //strsaleprice22 = Convert.ToDecimal(strprice2);
                        //    }
                        //    if (strprice3 == "")
                        //    {
                        //        strprice3 = strprice;
                        //        strprice33 = Convert.ToDecimal(strprice3);
                        //        strsaleprice33 = Convert.ToDecimal(strsaleprice);
                        //    }
                        //    else
                        //    {
                        //        strprice33 = Convert.ToDecimal(strprice3) + Convert.ToDecimal(strprice);
                        //        strsaleprice33 = Convert.ToDecimal(strsaleprice) + Convert.ToDecimal(strprice3);

                        //        //strprice33 = Convert.ToDecimal(strprice);
                        //        //strsaleprice33 = Convert.ToDecimal(strprice3);
                        //    }
                        //    if (strprice4 == "")
                        //    {
                        //        strprice4 = strprice;
                        //        strprice44 = Convert.ToDecimal(strprice4);
                        //        strsaleprice44 = Convert.ToDecimal(strsaleprice);
                        //    }
                        //    else
                        //    {
                        //        strprice44 = Convert.ToDecimal(strprice4) + Convert.ToDecimal(strprice);
                        //        strsaleprice44 = Convert.ToDecimal(strsaleprice) + Convert.ToDecimal(strprice4);

                        //        //strprice44 = Convert.ToDecimal(strprice);
                        //        //strsaleprice44 = Convert.ToDecimal(strprice4);
                        //    }

                        //}
                        //catch
                        //{
                        //}
                        //if ("PDCH-KBS-16-SW" == strcode.ToString())
                        //{

                        //}
                        //objSql.ExecuteNonQuery("UPDATE tb_TempImagerename_New SET Image='" + strimg + "', Image1='" + strimg1 + "', Image2='" + strimg2 + "', Image3='" + strimg3 + "',Image4='" + strimg4 + "'  WHERE ProductURL like '%" + strproductyahooid + ".html%'");
                        if(primaryheader.ToString().ToLower() =="pole pocket" || primaryheader.ToString().ToLower() == "rod pocket")
                        {
                            primaryheader = "Pole Pocket/Rod Pocket";
                        }
                        Int32 Isproperty=0;
                        if(string.IsNullOrEmpty(ydefficiency))
                        {
                            ydefficiency = "0";
                        }
                        if (string.IsNullOrEmpty(ydlightcontrol))
                        {
                            ydlightcontrol = "0";
                        }
                        if (string.IsNullOrEmpty(ydprivacy))
                        {
                            ydprivacy = "0";
                        }
                        if(ydefficiency.ToString() == "0")
                        {
                            Isproperty = 0;
                        }
                        else
                        {
                            Isproperty = 1;

                        }
                        objSql.ExecuteNonQuery("UPDATE tb_Product SET Isproperty=" + Isproperty + ",Efficiency=" + ydefficiency + ",Privacy=" + ydprivacy + ",LightControl=" + ydlightcontrol + ", Colors='" + searchcolor.Replace("'", "''") + "',Fabric='" + searchfabric.Replace("'", "''") + "',Pattern='" + searchpattern.Replace("'", "''") + "',Style='" + style + "',Header='" + primaryheader + "'  WHERE BackupProductUrl like '%" + strproductyahooid + ".html%'");

                        //if (strcode != "")
                        //{

                        //    //strcode = strcode.Replace("-SLDW", "");
                        //    //strcode = strcode.Replace("-GR-BO", "");
                        //    //strcode = strcode.Replace("-GR", "");
                        //    strcode = strcode.Replace("-84-PR", "").Replace("-96-PR", "").Replace("-108-PR", "").Replace("-120-PR", "");
                        //    strcode = strcode.Replace("-84-GRBO", "").Replace("-96-GRBO", "").Replace("-108-GRBO", "").Replace("-120-GRBO", "");

                        //    strcode = strcode.Replace("-84-GR", "").Replace("-96-GR", "").Replace("-108-GR", "").Replace("-120-GR", "");
                        //    strcode = strcode.Replace("-84-SLDW", "").Replace("-96-SLDW", "").Replace("-108-SLDW", "").Replace("-120-SLDW", "");
                        //    strcode = strcode.Replace("-84-DLSW", "").Replace("-96-DLSW", "").Replace("-108-DLSW", "").Replace("-120-DLSW", "");

                        //    strcode = strcode.Replace("-84-RU", "").Replace("-96-RU", "").Replace("-108-RU", "").Replace("-120-RU", "");
                        //    strcode = strcode.Replace("-DL-CUS", "");
                        //    strcode = strcode.Replace("-CUS", "");
                        //    strcode = strcode.Replace("-84", "").Replace("-96", "").Replace("-108", "").Replace("-120", "");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-DL-CUS'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-CUS'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "'");



                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-DLSW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-DLSW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-DLSW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-DLSW'");

                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-SLDW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-SLDW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-SLDW'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-SLDW'");


                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-GR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-GR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-GR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-GR'");


                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-GRBO'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-GRBO'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-GRBO'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-GRBO'");


                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-PR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-PR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-PR'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-PR'");


                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice11.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice11.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-RU'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice22.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice22.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-RU'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice33.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice33.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-RU'");
                        //    //objSql.ExecuteNonQuery("UPDATE MASTERData SET code='" + strcode.Replace("'", "''") + "',price='" + strprice44.ToString().Replace("'", "''") + "',saleprice='" + strsaleprice44.ToString().Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-RU'");

                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "'Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-DL-CUS'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-CUS'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "'");



                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-DLSW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-DLSW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-DLSW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-DLSW'");

                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-SLDW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-SLDW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-SLDW'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-SLDW'");


                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-GR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-GR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-GR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-GR'");


                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-GRBO'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-GRBO'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-GRBO'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-GRBO'");


                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-PR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-PR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-PR'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-PR'");


                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-84-RU'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-96-RU'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-108-RU'");
                        //    objSql.ExecuteNonQuery("UPDATE MASTERData_New SET code='" + strcode.Replace("'", "''") + "',Description='" + strdescription.Replace("'", "''") + "',ydproductfeatures='" + strydproductfeatures.Replace("'", "''") + "',productshippingtime='" + strproductshippingtime.Replace("'", "''") + "',yhaooid='" + strproductyahooid.Replace("'", "''") + "',ydprivacy='" + ydprivacy + "',ydlightcontrol='" + ydlightcontrol + "',ydefficiency='" + ydefficiency + "',shipweight='" + shipweight + "' WHERE [Product SKU]='" + strcode + "-120-RU'");




                        //}



                    }
                }



            }
        }

    }
}