using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using Solution.Bussines.Components;
using System.Text;
using System.IO;
using System.Data;
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class GenerateTempXml : System.Web.UI.Page
    {
        StringBuilder sitemap = new StringBuilder();
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnGeneratexml_Click(object sender, EventArgs e)
        {
            sitemap.Append("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            sitemap.Append("<feed>");
            sitemap.Append("<lastModifiedDate>" + DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss") + "</lastModifiedDate> ");



            sitemap.Append("<data>");
            GetProducts();
            sitemap.Append("</data>");
            sitemap.Append("</feed>");


            //  sitemap = sitemap.Replace("&", "&amp;").Replace("<", "&lt;").Replace(">", "&gt;").Replace("\"", "&quot;").Replace("'", "&apos;");
            StreamWriter sw = new StreamWriter(System.IO.File.Open(Server.MapPath("/images/ProductDataFeed.xml"), System.IO.FileMode.Create));
            sw.Write(sitemap.ToString());
            sw.Close();
        }
        private void GetProducts()
        {
            DataSet dsProduct = new DataSet();

            string StrSqlQuery = "";
            // StrSqlQuery = "declare @Servername as nvarchar(100) declare @ImagePath as nvarchar(500) set @serverName =(select configvalue from tb_Ecomm_Appconfig where configname='Live_server') "
            //   + " set @ImagePath =(select configvalue from tb_Ecomm_Appconfig where configname='ImagesPath') "
            //   + " select (Case p.Inventory When 0 then 'No' Else 'Yes' END) as Availability, (CASe ISNULL(isfreeshipping,0) WHEN 0 then 'No' Else 'Yes' END ) as freeShipping,'' as deliveryinfo,'' as color,'' as size, p.ProductID as ID,p.Name,p.SKU,p.Description,'http://67.228.161.183/images/mrvacandmrssew/product/micro/'+p.imagename+'.jpg' as ImageLink,p.Price,p.SalePrice, "
            //   + " @Servername+'/'+p.sename+'-'+convert(Nvarchar(10),ProductID)+'.aspx' as URL, p.Weight,UPC,Inventory,m.Name as Brand,Condition, "
            //   + " (select count(1) from tb_Ecomm_Rating r where r.ProductID = p.ProductID) as 'product_review_count', "
            //   + " (select isnull(convert(Decimal(10,2),(Convert(Decimal(10,2),sum(Rating))/COunt(1))),0)  from tb_Ecomm_Rating r where r.ProductID = p.ProductID) as 'product_review_average' "
            //   + " ,isnull(Inventory,'') as Quantity,(Case p.Inventory When 0 then 'Out of Stock' else 'In Stock' end) as Availability, ProductTypeName,(select Top 1 Name  from tb_Ecomm_Category where SEName =p.MainCategory and StoreID=1) as MainCategory  from tb_Ecomm_Product p "
            //   + " left outer join tb_Ecomm_Manufacture m on m.ManufactureID = p.ManufactureID  where p.status=1 and p.deleted=0 and productid not in (select ProductID from tb_Ecomm_GiftCardProduct)";

            //dsProduct = ECommerceSite.DataBase.Common.SQLAccess.GetDs(StrSqlQuery);
            StrSqlQuery = @"select ProductID as id, ('http://www.halfpricedrapes.com/' + ProductURL) as url,Name as name, 
ImageName as ImageLink,isnull(Price,0) as Price,isnull(SalePrice,0) as SalePrice,Name as [shortDesc],Description as [desc],MainCategory as Category,SKU,Colors,isnull(Weight,0) as Weight,Pattern,Style,Fabric, isnull(ProductCondition,'New') as condition,case when isnull(Inventory,0) =0 then 'NO' else 'YES'  end as Availability from tb_Product   where StoreID=1 and isnull(Active,0)=1 and
isnull(deleted,0)=0 and ProductId in (SELECT ProductId FROM tb_Productcategory)";
            dsProduct = CommonComponent.GetCommonDataSet(StrSqlQuery);
            if (dsProduct.Tables[0].Rows.Count > 0 && dsProduct.Tables[0] != null)
            {
                foreach (DataRow dr in dsProduct.Tables[0].Rows)
                {



                    String strmaincaegory = "";
                    String strsubcaegory = "";

                    try
                    {

                        string Query1 = " SELECT Top 1  CategoryID FROM tb_ProductCategory WHERE ProductID=" + dr["ID"].ToString() + "";
                        Int32 Catid = Convert.ToInt32(CommonComponent.GetScalarCommonData(Query1));

                        Int32 ParentCatid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Top 1 isnull(ParentCategoryID,0) FROM tb_CategoryMapping WHERE CategoryID=" + Catid + " "));
                        strmaincaegory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Name FROM tb_Category WHERE CategoryID=" + ParentCatid + " "));
                        strsubcaegory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 Name FROM tb_Category WHERE CategoryID=" + Catid + " "));
                        //DataSet DsProcat = new DataSet();
                        //DsProcat = ECommerceSite.DataBase.Common.SQLAccess.GetDs(Query1);
                        //if (DsProcat.Tables[0].Rows.Count > 0 && DsProcat != null)
                        //{
                        //    strBreadcum = GetBreadCrumbPathAmazon(Convert.ToInt32(DsProcat.Tables[0].Rows[0]["CategoryID"].ToString()), true, 1);
                        //}


                    }
                    catch
                    {
                    }







                    sitemap.Append("<product>");

                    sitemap.Append("<id>");
                    sitemap.Append(Convert.ToString(dr["SKU"]).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;"));
                    sitemap.Append("</id>");

                    sitemap.Append("<url>");
                    sitemap.Append(Convert.ToString(dr["url"]).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;"));
                    sitemap.Append("</url>");

                    sitemap.Append("<name>");
                    sitemap.Append(Convert.ToString(dr["name"]).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;"));
                    sitemap.Append("</name>");

                    sitemap.Append("<image>");

                    if (String.IsNullOrEmpty(dr["ImageLink"].ToString()))
                    {
                        sitemap.Append("http://www.halfpricedrapes.us/Resources/halfpricedraps/product/Icon/image_not_available.jpg");
                    }
                    else
                    {
                        sitemap.Append("http://www.halfpricedrapes.us/Resources/halfpricedraps/product/Icon/" + Convert.ToString(dr["ImageLink"]));
                    }
                    sitemap.Append("</image>");

                    sitemap.Append("<price>");

                    if (dr["price"] != null && Convert.ToString(dr["price"]) != "")
                        sitemap.Append(string.Format("{0:0.00}", Convert.ToDecimal(dr["price"])));
                    else
                        sitemap.Append("0");

                    sitemap.Append("</price>");

                    sitemap.Append("<salePrice>");

                    if (dr["SalePrice"] != null && Convert.ToString(dr["SalePrice"]) != "")
                        sitemap.Append(string.Format("{0:0.00}", Convert.ToDecimal(dr["SalePrice"])));
                    else
                        sitemap.Append("0");
                    sitemap.Append("</salePrice>");

                    sitemap.Append("<currency>");
                    sitemap.Append("USD");
                    sitemap.Append("</currency>");

                    sitemap.Append("<shortDesc>");
                    sitemap.Append(Convert.ToString(dr["Name"]).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;"));
                    sitemap.Append("</shortDesc>");

                    sitemap.Append("<desc>");
                    sitemap.Append("<![CDATA[" + dr["desc"].ToString() + "]]>");
                    sitemap.Append("</desc>");

                    sitemap.Append("<category>");

                    sitemap.Append(Convert.ToString(strmaincaegory).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;"));
                    sitemap.Append("</category>");

                    sitemap.Append("<listCategory>");
                    sitemap.Append(Convert.ToString(strmaincaegory).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;") + ";" + Convert.ToString(strsubcaegory).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;"));
                    sitemap.Append("</listCategory>");

                    sitemap.Append("<condition>");
                    sitemap.Append(Convert.ToString(dr["condition"]));

                    sitemap.Append("</condition>");



                    sitemap.Append("<model>");
                    sitemap.Append(Convert.ToString(dr["SKU"]).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;"));

                    sitemap.Append("</model>");

                    sitemap.Append("<color>");
                    sitemap.Append(Convert.ToString(dr["colors"].ToString().Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace(",", ";")));
                    sitemap.Append("</color>");

                    sitemap.Append("<size>");

                    string strvaraintId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 VariantID FROM tb_ProductVariant WHERE ProductId=" + dr["ID"].ToString() + " and VariantName like '%select size%'"));
                    string strvariantvalue = "";

                    if (!string.IsNullOrEmpty(strvaraintId))
                    {
                        strvariantvalue = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VariantValue+';' FROM tb_ProductVariantValue WHERE ProductId=" + dr["ID"].ToString() + " and VariantValue not like '%custom%' and VariantID=" + strvaraintId + " FOR XML path('')"));
                    }

                    sitemap.Append(Convert.ToString(strvariantvalue.ToString().Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;")));
                    sitemap.Append("</size>");

                    sitemap.Append("<weight>");

                    if (dr["weight"] != null && Convert.ToString(dr["weight"]) != "")
                        sitemap.Append(Convert.ToString(dr["weight"]));
                    else
                        sitemap.Append("0");
                    sitemap.Append("</weight>");


                    sitemap.Append("<inStock>");
                    sitemap.Append(Convert.ToString(dr["Availability"]));
                    sitemap.Append("</inStock>");

                    sitemap.Append("<pattern>");
                    sitemap.Append(Convert.ToString(dr["Pattern"].ToString()).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace(",", ";"));
                    sitemap.Append("</pattern>");

                    sitemap.Append("<style>");
                    sitemap.Append(Convert.ToString(dr["Style"]).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace(",", ";"));
                    sitemap.Append("</style>");

                    sitemap.Append("<fabric>");
                    sitemap.Append(Convert.ToString(dr["Fabric"]).Replace("&", "&amp;").Replace("\"", "&quot;").Replace("'", "&apos;").Replace(",", ";"));
                    sitemap.Append("</fabric>");

                    sitemap.Append("</product>");



                }
            }
        }
        private string _EscapeCsvField(string sFieldValueToEscape)
        {

            sFieldValueToEscape = sFieldValueToEscape.Replace("®", "&reg;").Replace("©", "&copy;").Replace("™", "&#153;").Replace(",", "").Replace(" > ", ";");
            string pattern = @"<(.|\n)*?>";

            sFieldValueToEscape = System.Text.RegularExpressions.Regex.Replace(sFieldValueToEscape, pattern, string.Empty);

            return sFieldValueToEscape;

        }

        protected void GeneateFeed_Click(object sender, EventArgs e)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=ProductDetail_Export.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder sb = new StringBuilder();

            DataSet dsorder = new DataSet();

            bool falg = false;

            dsorder = CommonComponent.GetCommonDataSet("SELECT SKU as Id,Name,cast(Description as nvarchar(max)) as description,isnull(ItemType,'') as product_type,'http://www.halfpricedrapes.com/'+ProductURL as link,salePrice,Price, upc,isnull(Weight,0) as ShippingWeight,SKU,'HalfPriceDrapes.com' as brand,'New' as condition,'http://www.halfpricedrapes.com/Resources/halfpricedraps/product/icon/'+ImageName as image_link,'Home & Garden > Decor > Doors & Windows > Window Treatments > Curtains & Drapes' as product_type,'Home & Garden > Decor > Doors & Windows > Window Treatments > Curtains & Drapes' as google_product_category,SKu as mpn,case when isnull(inventory,0)=0 then 'out of stock' else 'in stock' end as availability  FROM tb_product WHERE  Storeid=1 and isnull(Active,0)=1 and isnull(deleted,0)=0");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {
                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }

            }
            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {

                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "description")
                        {
                            args[c] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i][c].ToString().Trim().Replace("&nbsp;", " ").Replace("<br>", "").Replace("<br/>", "").Replace("<br />", "").Replace("</br>", "").Replace("<p>", "").Replace("</p>", "").Replace("</ p>", "").Replace("<p >", ""), @"<[^>]*>", string.Empty));
                        }
                        else
                        {
                            args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                        }

                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }


            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
    }
}