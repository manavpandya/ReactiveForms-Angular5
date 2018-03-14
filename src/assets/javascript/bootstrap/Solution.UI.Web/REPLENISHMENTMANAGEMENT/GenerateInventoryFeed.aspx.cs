using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.IO;
using Solution.Bussines.Components.Common;
using System.Threading;
using LumenWorks.Framework.IO.Csv;
using System.Data;
using Solution.Bussines.Components;
using System.Text;
using ExcelLibrary;
using ExcelLibrary.SpreadSheet;
using System.Data.SqlClient;





namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class GenerateInventoryFeed : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("/Admin/login.aspx");
            }
            if (!IsPostBack)
            {
                BindStore();
            }
        }

        private void BindStore()
        {
            GenerateInventoryFeedComponent objInv = new GenerateInventoryFeedComponent();
            DataSet dsStore = new DataSet();
            dsStore = objInv.GetSalesPartnerList();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlstore.DataSource = dsStore.Tables[0];
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "RepStoreID";
                ddlstore.DataBind();
            }
            else
            {
                ddlstore.DataSource = null;
            }
            ddlstore.Items.Insert(0, new ListItem("Select Channel Partner", "0"));
        }

        //private void GetFieldTemplate()
        //{
        //    GenerateInventoryFeedComponent objInv = new GenerateInventoryFeedComponent();
        //    DataSet DsTemplate = new DataSet();
        //    DsTemplate = objInv.GetChannelPartnerFeedTemplate(Convert.ToInt32(ddlstore.SelectedValue.ToString()));
        //    if (DsTemplate != null && DsTemplate.Tables.Count > 0 && DsTemplate.Tables[0].Rows.Count > 0)
        //    {
        //        String strqueryProduct = "";
        //        String StrVariant = "";


        //        DataSet dsProductcol = new DataSet();
        //        dsProductcol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_Product'");
        //        if (dsProductcol != null && dsProductcol.Tables.Count > 0 && dsProductcol.Tables[0].Rows.Count > 0)
        //        {

        //            strqueryProduct = "select ";
        //            String Strpara = "";
        //            for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
        //            {
        //                Int32 IsRequired = 0;
        //                Int32 IsStatic = 0;
        //                string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
        //                if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
        //                {
        //                    IsRequired = 1;
        //                }
        //                else
        //                {
        //                    IsRequired = 0;
        //                }

        //                if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
        //                {
        //                    IsStatic = 1;
        //                }
        //                else
        //                {
        //                    IsStatic = 0;
        //                }

        //                String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
        //                String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

        //                if (!String.IsNullOrEmpty(MappingColumn))
        //                {
        //                    if (MappingColumn == "SKU")
        //                    {
        //                        MappingColumn = "SKU";
        //                    }
        //                    else if (MappingColumn == "GS1 ID")
        //                    {
        //                        MappingColumn = "productid";
        //                    }
        //                    else if (MappingColumn == "Product Name/Description")
        //                    {
        //                        MappingColumn = "Name";
        //                    }
        //                    else if (MappingColumn == "Price")
        //                    {
        //                        MappingColumn = "Price";
        //                    }
        //                    else if (MappingColumn == "Quantity On Order")
        //                    {
        //                        MappingColumn = "Inventory";
        //                    }
        //                    else if (MappingColumn == "Discontinued Status")
        //                    {
        //                        MappingColumn = "IsDiscontinued";
        //                    }
        //                    else if (MappingColumn == "Active or Inactive Status")
        //                    {
        //                        MappingColumn = "Active";
        //                    }
        //                    else if (MappingColumn == "Item Availability Date")
        //                    {
        //                        MappingColumn = "CreatedOn";
        //                    }

        //                    DataRow[] drr = dsProductcol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
        //                    if (drr.Length > 0)
        //                    {
        //                        Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
        //                    }
        //                    else
        //                    {
        //                        MappingColumn = "";
        //                        // Strpara += ' ' + " as " + FieldName + ",";
        //                        Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
        //                    }



        //                }
        //                else if (IsStatic == 1)
        //                {
        //                    Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
        //                }




        //            }

        //            Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
        //            strqueryProduct = strqueryProduct + Strpara + " from tb_product ";
        //            String Strwhr = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') and storeid=1";
        //            strqueryProduct = strqueryProduct + Strwhr;



        //        }



        //        DataSet dsVariantCol = new DataSet();
        //        dsVariantCol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_ProductVariantValue'");
        //        if (dsVariantCol != null && dsVariantCol.Tables.Count > 0 && dsVariantCol.Tables[0].Rows.Count > 0)
        //        {
        //            StrVariant = "select ";


        //            String Strpara = "";
        //            for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
        //            {
        //                Int32 IsRequired = 0;
        //                Int32 IsStatic = 0;
        //                string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
        //                if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
        //                {
        //                    IsRequired = 1;
        //                }
        //                else
        //                {
        //                    IsRequired = 0;
        //                }

        //                if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
        //                {
        //                    IsStatic = 1;
        //                }
        //                else
        //                {
        //                    IsStatic = 0;
        //                }

        //                String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
        //                String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

        //                if (!String.IsNullOrEmpty(MappingColumn))
        //                {
        //                    if (MappingColumn == "Channel Partner SKU")
        //                    {
        //                        MappingColumn = "SKU";
        //                    }
        //                    else if (MappingColumn == "GS1 ID")
        //                    {
        //                        MappingColumn = "productid";
        //                    }
        //                    else if (MappingColumn == "Price")
        //                    {
        //                        MappingColumn = "Price";
        //                    }
        //                    else if (MappingColumn == "Discontinued Status")
        //                    {
        //                        MappingColumn = "IsDiscontinued";
        //                    }
        //                    else if (MappingColumn == "Price")
        //                    {
        //                        MappingColumn = "VariantPrice";
        //                    }
        //                    else if (MappingColumn == "Active or Inactive Status")
        //                    {
        //                        MappingColumn = "VarActive";
        //                    }
        //                    else if (MappingColumn == "Quantity Available for Channel Partner")
        //                    {
        //                        MappingColumn = "Inventory";
        //                    }
        //                    else if (MappingColumn == "Item Availability Date")
        //                    {
        //                        MappingColumn = "BackOrderdate";
        //                    }

        //                    DataRow[] drr = dsVariantCol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
        //                    if (drr.Length > 0)
        //                    {
        //                        Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
        //                    }
        //                    else
        //                    {
        //                        MappingColumn = "";
        //                        Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
        //                    }



        //                }
        //                else if (IsStatic == 1)
        //                {
        //                    Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
        //                }




        //            }


        //            Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
        //            StrVariant = " Union all " + StrVariant + Strpara + " from tb_productvariantvalue ";
        //            string Strwhr2 = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') ";
        //            StrVariant = StrVariant + Strwhr2;


        //            //StrVariant = StrVariant + " Union all ";
        //            //string child = "";
        //            //child = "select ";
        //            //child = child + Strpara + " from tb_productvariantvalue ";
        //            //string Strwhr2 = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') ";
        //            //StrVariant = StrVariant + child + Strwhr2;

        //        }



        //        String strquery = "";
        //        strquery = strqueryProduct + StrVariant;
        //        DataSet dsEcommProducts = new DataSet();
        //        dsEcommProducts = CommonComponent.GetCommonDataSet(strquery);
        //        if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0 && dsEcommProducts.Tables[0].Rows.Count > 0)
        //        {
        //            if (radiocsv.Checked)
        //            {
        //                GenerateCSV(dsEcommProducts);
        //            }
        //            else if (radioExcel.Checked)
        //            {
        //                GenerateExcel(dsEcommProducts);
        //            }



        //        }
        //        else
        //        {
        //            if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0)
        //            {
        //                if (radiocsv.Checked)
        //                {
        //                    GenerateCSV(dsEcommProducts);
        //                }
        //                else if (radioExcel.Checked)
        //                {
        //                    GenerateExcel(dsEcommProducts);
        //                }



        //            }
        //        }


        //    }
        //    else
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore", "alert('Template Data not Found.');", true);
        //        return;
        //    }


        //}

        //private void GetFieldTemplate()
        //{
        //    GenerateInventoryFeedComponent objInv = new GenerateInventoryFeedComponent();
        //    DataSet DsTemplate = new DataSet();
        //    DsTemplate = objInv.GetChannelPartnerFeedTemplate(Convert.ToInt32(ddlstore.SelectedValue.ToString()));
        //    if (DsTemplate != null && DsTemplate.Tables.Count > 0 && DsTemplate.Tables[0].Rows.Count > 0)
        //    {
        //        String strqueryProduct = "";
        //        String StrVariant = "";


        //        //DataSet dsProductcol = new DataSet();
        //        //dsProductcol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_Product'");
        //        //if (dsProductcol != null && dsProductcol.Tables.Count > 0 && dsProductcol.Tables[0].Rows.Count > 0)
        //        //{

        //        strqueryProduct = "select ";
        //        String Strpara = "";
        //        for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
        //        {
        //            Int32 IsRequired = 0;
        //            Int32 IsStatic = 0;
        //            string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
        //            if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
        //            {
        //                IsRequired = 1;
        //            }
        //            else
        //            {
        //                IsRequired = 0;
        //            }

        //            if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
        //            {
        //                IsStatic = 1;
        //            }
        //            else
        //            {
        //                IsStatic = 0;
        //            }

        //            String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
        //            String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

        //            if (!String.IsNullOrEmpty(MappingColumn))
        //            {
        //                if (MappingColumn.ToString().ToLower().Trim() == "channel partner sku")
        //                {
        //                    MappingColumn = "SKU";

        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "sku")
        //                {
        //                    MappingColumn = "SKU";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "gs1 id")
        //                {
        //                    MappingColumn = "UPC";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "product name/description")
        //                {
        //                    MappingColumn = "(select top 1 isnull(name,'')  from tb_Product where StoreID=" + ddlstore.SelectedValue.ToString() + " and UPC=tb_Product.UPC)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "price")
        //                {
        //                    // MappingColumn = "cast(Price as nvarchar(max))";
        //                    MappingColumn = "(select top 1 cast(Price as nvarchar(max))  from tb_Product where StoreID=" + ddlstore.SelectedValue.ToString() + " and UPC=tb_Product.UPC)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order1")
        //                {
        //                    MappingColumn = "(select top 1 tb_Replenishment.qty1 from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "item availability date1")
        //                {
        //                    MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 1) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
        //                }


        //                else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order2")
        //                {
        //                    MappingColumn = "(select top 1 tb_Replenishment.qty2 from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "item availability date2")
        //                {
        //                    MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 1)  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
        //                }

        //                else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order3")
        //                {
        //                    MappingColumn = "(select top 1 tb_Replenishment.qty3 from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "item availability date3")
        //                {
        //                    MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 1)  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
        //                }

        //                else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order4")
        //                {
        //                    MappingColumn = "(select top 1 tb_Replenishment.qty4 from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "item availability date4")
        //                {
        //                    MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 1) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
        //                }

        //                else if (MappingColumn.ToString().ToLower().IndexOf("quantity available for channel partner") > -1)
        //                {
        //                    MappingColumn = "cast(Inventory as nvarchar(max))";

        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "discontinued status")
        //                {
        //                    // MappingColumn = "case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end";
        //                    MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max)) ='0' then 'false' else 'true' end from tb_Product where StoreID=" + ddlstore.SelectedValue.ToString() + " and upc=tb_Product.UPC)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status" && FieldName.ToString().ToLower() == "available")
        //                {
        //                    MappingColumn = " case when isnull(inventory,0) >0 then 'Yes' else 'No' end ";

        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status")
        //                {
        //                    MappingColumn = "Active";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "optionsku")
        //                {
        //                    MappingColumn = "(select top 1 isnull(optionsku,'')  from tb_Product where StoreID=" + ddlstore.SelectedValue.ToString() + " and UPC=tb_Product.UPC)";
        //                }

        //                //DataRow[] drr = dsProductcol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
        //                //if (drr.Length > 0)
        //                //{
        //                //    Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
        //                //}
        //                //else
        //                //{
        //                //    MappingColumn = "";
        //                //    // Strpara += ' ' + " as " + FieldName + ",";
        //                //    Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
        //                //}
        //                if (!String.IsNullOrEmpty(MappingColumn))
        //                    Strpara += MappingColumn + " as [" + FieldName + "],";
        //                else
        //                {
        //                    MappingColumn = "";
        //                    Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
        //                }



        //            }
        //            else if (IsStatic == 1)
        //            {
        //                if (StaticValue.ToLower().Trim() == "blank")
        //                {
        //                    StaticValue = "";
        //                }
        //                Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
        //            }




        //        }

        //        Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
        //        strqueryProduct = strqueryProduct + Strpara + " from tb_product ";
        //        String Strwhr = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') and storeid=1 and ProductID not in (select ProductID from tb_ProductVariantValue) and isnull(Deleted,0)<>1";
        //        strqueryProduct = strqueryProduct + Strwhr;



        //        //}



        //        //DataSet dsVariantCol = new DataSet();
        //        //dsVariantCol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_ProductVariantValue'");
        //        //if (dsVariantCol != null && dsVariantCol.Tables.Count > 0 && dsVariantCol.Tables[0].Rows.Count > 0)
        //        //{
        //        StrVariant = "select ";


        //        Strpara = "";
        //        for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
        //        {
        //            Int32 IsRequired = 0;
        //            Int32 IsStatic = 0;
        //            string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
        //            if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
        //            {
        //                IsRequired = 1;
        //            }
        //            else
        //            {
        //                IsRequired = 0;
        //            }

        //            if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
        //            {
        //                IsStatic = 1;
        //            }
        //            else
        //            {
        //                IsStatic = 0;
        //            }

        //            String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
        //            String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

        //            if (!String.IsNullOrEmpty(MappingColumn))
        //            {

        //                if (MappingColumn.ToString().ToLower().Trim() == "channel partner sku")
        //                {
        //                    MappingColumn = "SKU";

        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "gs1 id")
        //                {
        //                    MappingColumn = "UPC";

        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "sku")
        //                {
        //                    MappingColumn = "SKU";

        //                }

        //                else if (MappingColumn.ToString().ToLower().Trim() == "product name/description")
        //                {

        //                    //  MappingColumn = "(select tb_Product.Name from tb_product where ProductID=tb_productvariantvalue.ProductID)";
        //                    MappingColumn = "(select top 1 isnull(tb_Product.Name,'') from tb_product where StoreID=" + ddlstore.SelectedValue.ToString() + " and UPC=tb_productvariantvalue.UPC )";

        //                }

        //                else if (MappingColumn.ToString().ToLower().Trim() == "price")
        //                {
        //                    // MappingColumn = "cast(VariantPrice as nvarchar(max))";
        //                    MappingColumn = "(select top 1 cast(Price as nvarchar(max))  from tb_Product where StoreID=" + ddlstore.SelectedValue.ToString() + " and UPC=tb_productvariantvalue.UPC)";

        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status" && FieldName.ToString().ToLower() == "available")
        //                {
        //                    MappingColumn = " case when isnull(inventory,0) > 0 then 'Yes' else 'No' end ";

        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status")
        //                {
        //                    MappingColumn = "VarActive";

        //                }

        //                else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order1")
        //                {
        //                    MappingColumn = "(select top 1 tb_Replenishment.qty1 from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "item availability date1")
        //                {
        //                    MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 1) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";

        //                }

        //                else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order2")
        //                {
        //                    MappingColumn = "(select top 1 tb_Replenishment.qty2 from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "item availability date2")
        //                {
        //                    MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 1) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
        //                }

        //                else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order3")
        //                {
        //                    MappingColumn = "(select top 1 tb_Replenishment.qty3 from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "item availability date3")
        //                {
        //                    MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 1) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
        //                }

        //                else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order4")
        //                {
        //                    MappingColumn = "(select top 1 tb_Replenishment.qty4 from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "item availability date4")
        //                {
        //                    MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 1) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
        //                }

        //                else if (MappingColumn.ToString().ToLower().IndexOf("quantity available for channel partner") > -1)
        //                {
        //                    MappingColumn = "cast(Inventory as nvarchar(max))";

        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "discontinued status")
        //                {
        //                    // MappingColumn = "(select case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end from tb_product where ProductID=tb_productvariantvalue.ProductID)";
        //                    MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end from tb_product where StoreID=" + ddlstore.SelectedValue.ToString() + " and UPC=tb_productvariantvalue.UPC)";
        //                }
        //                else if (MappingColumn.ToString().ToLower().Trim() == "optionsku")
        //                {
        //                    MappingColumn = "(select top 1 isnull(optionsku,'')  from tb_Product where StoreID=" + ddlstore.SelectedValue.ToString() + " and UPC=tb_productvariantvalue.UPC)";
        //                }

        //                if (!String.IsNullOrEmpty(MappingColumn))
        //                    Strpara += MappingColumn + " as [" + FieldName + "],";
        //                else
        //                {
        //                    MappingColumn = "";
        //                    Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
        //                }
        //                //DataRow[] drr = dsVariantCol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
        //                //if (drr.Length > 0)
        //                //{
        //                //    Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
        //                //}
        //                //else
        //                //{
        //                //    MappingColumn = "";
        //                //    Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
        //                //}



        //            }
        //            else if (IsStatic == 1)
        //            {
        //                if (StaticValue.ToLower().Trim() == "blank")
        //                {
        //                    StaticValue = "";
        //                }
        //                Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
        //            }




        //        }


        //        Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
        //        StrVariant = " Union all " + StrVariant + Strpara + " from tb_productvariantvalue ";
        //        string Strwhr2 = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') and productid  in (select productid from tb_product where isnull(deleted,0)=0 and storeid=1) ";
        //        StrVariant = StrVariant + Strwhr2;


        //        //StrVariant = StrVariant + " Union all ";
        //        //string child = "";
        //        //child = "select ";
        //        //child = child + Strpara + " from tb_productvariantvalue ";
        //        //string Strwhr2 = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') ";
        //        //StrVariant = StrVariant + child + Strwhr2;

        //        //  }

        //        string kohlquery = "";
        //        if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
        //        {
        //            kohlquery = "select 'Always IN' as [FILE TYPE],'your sku #' as [VENDOR SKU],'Yes,No, or Guaranteed' as [AVAILABLE],'Quantity On Hand' as [QTY],'Quantity Arriving Next' as [NEXT AVAILABLE QTY],'Format: MM/DD/YYYY' as [NEXT AVAILABLE DATE],'Product Manufacturer' as [MANUFACTURER],'Manufacturers SKU' as [MANUFACTURER SKU],'Enter Product Description' as [DESCRIPTION],'Enter Cost to Merchant in Dollars' as [UNIT COST],'Enter Unit Price in Dollars' as [UNIT COST 2],' n/a' as [UNIT COST 3],' n/a' as [UNIT COST 4],'Discontinued? 1\' as [DISCONTINUED],'Department Number of Product' as [MERCH. DEPT.],'Usually \"EA\"' as [UNIT OF MEASURE],'SKU Assigned by Merchant' as [MERCHANT SKU],'Enter Merchant Name' as  [MERCHANT],'Enter UPC or EAN #' as [GS1 ID] union all SELECT  [FILE TYPE], [VENDOR SKU],  [AVAILABLE],  cast([QTY] as nvarchar(100)) as [QTY],  cast([NEXT AVAILABLE QTY] as nvarchar(100)) as [NEXT AVAILABLE QTY],  [NEXT AVAILABLE DATE],  [MANUFACTURER],  [MANUFACTURER SKU],  [DESCRIPTION],  [UNIT COST],  [UNIT COST 2],  [UNIT COST 3], [UNIT COST 4], [DISCONTINUED], [MERCH. DEPT.],   [UNIT OF MEASURE],  [MERCHANT SKU],  [MERCHANT],  [GS1 ID] FROM ( ";
        //            //kohlquery = "select 'Always \"IN\"' as [FILE TYPE],'your sku #' as [VENDOR SKU],'Yes,\"No\", or \"Guaranteed\"' as [AVAILABLE],'Quantity On Hand' as [QTY],'Quantity Arriving Next' as [NEXT AVAILABLE QTY],'Format: MM/DD/YYYY' as [NEXT AVAILABLE DATE],'Product Manufacturer' as [MANUFACTURER],'Manufacturers SKU' as [MANUFACTURER SKU],'Enter Product Description' as [DESCRIPTION],'Enter Cost to Merchant in Dollars' as [UNIT COST],'Enter Unit Price in Dollars' as [UNIT COST 2],' n/a' as [UNIT COST 3],' n/a' as [UNIT COST 4],'Discontinued? \"1\"' as [DISCONTINUED],'Department Number of Product' as [MERCH. DEPT.],'Usually \"EA\"' as [UNIT OF MEASURE],'SKU Assigned by Merchant' as [MERCHANT SKU],'Enter Merchant Name' as  [MERCHANT],'Enter UPC or EAN #' as [GS1 ID] union all ";      
        //        }



        //        String strquery = "";
        //        if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
        //        {
        //            strquery = kohlquery + strqueryProduct + StrVariant + " ) as A";
        //        }
        //        else
        //        {
        //            strquery = strqueryProduct + StrVariant;
        //        }

        //        Response.Write("SELECT DISTINCT C.* FROM (" + strquery + ") as C");
        //        DataSet dsEcommProducts = new DataSet();
        //        dsEcommProducts = CommonComponent.GetCommonDataSet("SELECT DISTINCT C.* FROM (" + strquery + ") as C");
        //        if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0 && dsEcommProducts.Tables[0].Rows.Count > 0)
        //        {
        //            if (radiocsv.Checked)
        //            {
        //                GenerateCSV(dsEcommProducts);
        //            }
        //            else if (radioExcel.Checked)
        //            {
        //                GenerateExcel(dsEcommProducts);
        //            }



        //        }
        //        else
        //        {
        //            if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0)
        //            {
        //                if (radiocsv.Checked)
        //                {
        //                    GenerateCSV(dsEcommProducts);
        //                }
        //                else if (radioExcel.Checked)
        //                {
        //                    GenerateExcel(dsEcommProducts);
        //                }



        //            }
        //        }


        //    }
        //    else
        //    {
        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore", "alert('Template Data not Found.');", true);
        //        return;
        //    }


        //}

        private void GetFieldTemplate()
        {
            int storeid = 0;
            storeid = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            GenerateInventoryFeedComponent objInv = new GenerateInventoryFeedComponent();
            DataSet DsTemplate = new DataSet();
            DsTemplate = objInv.GetChannelPartnerFeedTemplate(Convert.ToInt32(ddlstore.SelectedValue.ToString()));


            if (DsTemplate != null && DsTemplate.Tables.Count > 0 && DsTemplate.Tables[0].Rows.Count > 0)
            {
                bool kohldis = false;
                bool atgdis = false;
                bool hozzdis = false;

                kohldis = Convert.ToBoolean(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='IsKohlDis' and storeid=1"));
                atgdis = Convert.ToBoolean(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='IsAtgDis' and storeid=1"));
                hozzdis = Convert.ToBoolean(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='IsHozzDis' and storeid=1"));

                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    storeid = 4;
                }
                String strqueryProduct = "";
                String StrVariant = "";


                //DataSet dsProductcol = new DataSet();
                //dsProductcol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_Product'");
                //if (dsProductcol != null && dsProductcol.Tables.Count > 0 && dsProductcol.Tables[0].Rows.Count > 0)
                //{

                strqueryProduct = "select ";
                String Strpara = "";
                for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
                {
                    Int32 IsRequired = 0;
                    Int32 IsStatic = 0;
                    string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
                    if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
                    {
                        IsRequired = 1;
                    }
                    else
                    {
                        IsRequired = 0;
                    }

                    if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
                    {
                        IsStatic = 1;
                    }
                    else
                    {
                        IsStatic = 0;
                    }

                    String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
                    String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

                    if (!String.IsNullOrEmpty(MappingColumn))
                    {
                        if (MappingColumn.ToString().ToLower().Trim() == "channel partner sku")
                        {
                            MappingColumn = "SKU";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "sku")
                        {
                            MappingColumn = "SKU";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "gs1 id")
                        {
                            //if (storeid == 12)
                            //{
                            //    MappingColumn = "''''+cast(UPC as nvarchar(max))";
                            //}
                            //else
                            //{
                                MappingColumn = "UPC";
                           // }

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "product name/description")
                        {
                            MappingColumn = "(select top 1 isnull(q.name,'')  from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "price")
                        {
                            // MappingColumn = "cast(Price as nvarchar(max))";
                            MappingColumn = "(select top 1 cast(q.Price as nvarchar(max))  from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order1")
                        {
                            MappingColumn = "(select top 1 case when cast(isnull(tb_Replenishment.qty1,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty1,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date1")
                        {
                            //if (storeid == 14 || ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep" || storeid == 12 || storeid == 9)
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 101) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                           // }

                        }


                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order2")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty2,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty2,0) as nvarchar(max)) else '' end  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date2")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 1) as nvarchar(max))  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 101)  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order3")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty3,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty3,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date3")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 1) as nvarchar(max))  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 101)  from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                           // }
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order4")
                        {
                            MappingColumn = "(select top 1 case when cast(isnull(tb_Replenishment.qty4,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty4,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date4")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 101) from tb_Replenishment where tb_Replenishment.ProductID=tb_product.ProductID)";
                           // }
                        }

                        else if (MappingColumn.ToString().ToLower().IndexOf("quantity available for channel partner") > -1)
                        {
                            MappingColumn = " (dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")) ";//cast(Inventory as nvarchar(max))

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "discontinued status")
                        {
                            // MappingColumn = "case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end";
                            if (storeid == 14 && atgdis == true) //atg
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(q.Discontinue,0) as nvarchar(max)) ='0' then 'false' else case when (dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")) >0 then 'false' else  'true' end end from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                            }
                            else if (storeid == 13 && hozzdis == true) //Houzz
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(q.Discontinue,0) as nvarchar(max)) ='0' then 'false' else case when (dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")) >0 then 'false' else  'true' end end from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                            }
                            else
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(q.Discontinue,0) as nvarchar(max)) ='0' then 'false' else 'true' end from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                            }

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status" && FieldName.ToString().ToLower() == "available")
                        {
                            if (storeid == 12 && kohldis == true) //kohl
                            {
                                MappingColumn = "case when (select top 1  cast(isnull(q.Discontinue,0) as nvarchar(max)) from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)='0' then 'Yes' else case when (select top 1  cast(isnull(q.Discontinue,0) as nvarchar(max)) from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)='1' and isnull((dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end end ";
                            }
                            else
                            {
                                MappingColumn = "case when isnull((dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end ";
                            }


                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status")
                        {
                            MappingColumn = "Active";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "optionsku")
                        {
                            MappingColumn = "(select top 1 isnull(q.optionsku,'')  from tb_Product as q where q.StoreID=" + storeid + " and q.sku=tb_Product.sku)";
                        }

                        //DataRow[] drr = dsProductcol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
                        //if (drr.Length > 0)
                        //{
                        //    Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
                        //}
                        //else
                        //{
                        //    MappingColumn = "";
                        //    // Strpara += ' ' + " as " + FieldName + ",";
                        //    Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
                        //}
                        if (!String.IsNullOrEmpty(MappingColumn))
                            Strpara += MappingColumn + " as [" + FieldName + "],";
                        else
                        {
                            MappingColumn = "";
                            Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
                        }



                    }
                    else if (IsStatic == 1)
                    {
                        if (StaticValue.ToLower().Trim() == "blank")
                        {
                            StaticValue = "";
                        }
                        Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
                    }




                }

                Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
                strqueryProduct = strqueryProduct + Strpara + " from tb_product ";
                String Strwhr = "";

                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    Strwhr = "  where sku in (select isnull(sku,'') as sku from tb_product where storeid=" + storeid + " and isnull(deleted,0)=0 and isnull(sku,'')<>'' and isnull(IsOverStockRep,0)=1 and isnull(IsDisplayOnFeed,0)=1) and storeid=1 and ProductID not in (select ProductID from tb_ProductVariantValue) and isnull(Deleted,0)<>1";
                }
                else
                {
                    Strwhr = "  where sku in (select isnull(sku,'') as sku from tb_product where storeid=" + storeid + " and isnull(deleted,0)=0 and isnull(sku,'')<>'' and isnull(IsDisplayOnFeed,0)=1) and storeid=1 and ProductID not in (select ProductID from tb_ProductVariantValue) and isnull(Deleted,0)<>1";

                }

                strqueryProduct = strqueryProduct + Strwhr;



                //}



                //DataSet dsVariantCol = new DataSet();
                //dsVariantCol = CommonComponent.GetCommonDataSet("select Column_name  from Information_schema.columns  where Table_name like 'tb_ProductVariantValue'");
                //if (dsVariantCol != null && dsVariantCol.Tables.Count > 0 && dsVariantCol.Tables[0].Rows.Count > 0)
                //{
                StrVariant = "select ";


                Strpara = "";
                for (int y = 0; y < DsTemplate.Tables[0].Rows.Count; y++)
                {
                    Int32 IsRequired = 0;
                    Int32 IsStatic = 0;
                    string FieldName = DsTemplate.Tables[0].Rows[y]["FieldName"].ToString();
                    if (DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsRequired"].ToString().ToLower().Trim() == "1")
                    {
                        IsRequired = 1;
                    }
                    else
                    {
                        IsRequired = 0;
                    }

                    if (DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "true" || DsTemplate.Tables[0].Rows[y]["IsStatic"].ToString().ToLower().Trim() == "1")
                    {
                        IsStatic = 1;
                    }
                    else
                    {
                        IsStatic = 0;
                    }

                    String StaticValue = DsTemplate.Tables[0].Rows[y]["StaticValue"].ToString();
                    String MappingColumn = DsTemplate.Tables[0].Rows[y]["MappingColumn"].ToString();

                    if (!String.IsNullOrEmpty(MappingColumn))
                    {

                        if (MappingColumn.ToString().ToLower().Trim() == "channel partner sku")
                        {
                            MappingColumn = "SKU";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "gs1 id")
                        {
                            //if (storeid == 12)
                            //{
                            //    MappingColumn = "''''+cast(UPC as nvarchar(max))";
                            //}
                            //else
                            //{
                                MappingColumn = "UPC";
                           // }



                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "sku")
                        {
                            MappingColumn = "SKU";

                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "product name/description")
                        {

                            //  MappingColumn = "(select tb_Product.Name from tb_product where ProductID=tb_productvariantvalue.ProductID)";
                            MappingColumn = "(select top 1 isnull(tb_Product.Name,'') from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku )";

                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "price")
                        {
                            // MappingColumn = "cast(VariantPrice as nvarchar(max))";
                            MappingColumn = "(select top 1 cast(Price as nvarchar(max))  from tb_Product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status" && FieldName.ToString().ToLower() == "available")
                        {
                            if (storeid == 12 && kohldis == true)
                            {
                                //  MappingColumn = "case when isnull((dbo.Producthamming_Scalar(tb_Product.upc,tb_Product.sku," + storeid + ")),0) > 0 then 'Yes' else case when  (select top 1  cast(isnull(tb_product.Discontinue,0) as nvarchar(max)) from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)='1' then 'Yes' else 'No' end end ";
                                MappingColumn = "case when (select top 1  cast(isnull(tb_product.Discontinue,0) as nvarchar(max)) from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)='0' then 'Yes' else case when  (select top 1  cast(isnull(tb_product.Discontinue,0) as nvarchar(max)) from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)='1' and isnull((dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end end ";


                            }
                            else
                            {
                                MappingColumn = "case when isnull((dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")),0) > 0 then 'Yes' else 'No' end ";
                            }


                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "active or inactive status")
                        {
                            MappingColumn = "VarActive";



                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order1")
                        {
                            MappingColumn = "(select top 1 case when cast(isnull(tb_Replenishment.qty1,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty1,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date1")
                        {
                            //if (storeid == 14 || ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep" || storeid == 12 || storeid == 9)
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate1 AS DATE), 101) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}

                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order2")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty2,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty2,0) as nvarchar(max)) else '' end  from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date2")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate2 AS DATE), 101) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order3")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty3,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty3,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date3")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate3 AS DATE), 101) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                           // }
                        }

                        else if (MappingColumn.ToString().ToLower().Trim() == "quantity on order4")
                        {
                            MappingColumn = "(select top 1  case when cast(isnull(tb_Replenishment.qty4,0) as nvarchar(max))!='0' then cast(isnull(tb_Replenishment.qty4,0) as nvarchar(max)) else '' end from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "item availability date4")
                        {
                            //if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                            //{
                            //    MappingColumn = "(select top 1 ''''+cast(CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 1) as nvarchar(max)) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                            //else
                            //{
                                MappingColumn = "(select top 1 CONVERT(VARCHAR(10), CAST(tb_Replenishment.Etadate4 AS DATE), 101) from tb_Replenishment where tb_Replenishment.VariantValueID=tb_ProductVariantValue.VariantValueID)";
                            //}
                        }

                        else if (MappingColumn.ToString().ToLower().IndexOf("quantity available for channel partner") > -1)
                        {
                            // MappingColumn = "cast(Inventory as nvarchar(max))";
                            MappingColumn = " (dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")) ";

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "discontinued status")
                        {
                            // MappingColumn = "(select case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end from tb_product where ProductID=tb_productvariantvalue.ProductID)";
                            if (storeid == 14 && atgdis == true) //atg
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else case when (dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")) >0 then 'false' else 'true' end end from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                            }
                            else if (storeid == 13 && hozzdis == true) //Houzz
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else case when (dbo.Producthamming_Scalar(tb_productvariantvalue.upc,tb_productvariantvalue.sku," + storeid + ")) >0 then 'false' else 'true' end end from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                            }

                            else
                            {
                                MappingColumn = "(select top 1 case when cast(isnull(tb_product.Discontinue,0) as nvarchar(max))='0' then 'false' else 'true' end from tb_product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                            }

                        }
                        else if (MappingColumn.ToString().ToLower().Trim() == "optionsku")
                        {
                            MappingColumn = "(select top 1 isnull(optionsku,'')  from tb_Product where StoreID=" + storeid + " and sku=tb_productvariantvalue.sku)";
                        }

                        if (!String.IsNullOrEmpty(MappingColumn))
                            Strpara += MappingColumn + " as [" + FieldName + "],";
                        else
                        {
                            MappingColumn = "";
                            Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
                        }
                        //DataRow[] drr = dsVariantCol.Tables[0].Select("Column_name = '" + MappingColumn + "'");
                        //if (drr.Length > 0)
                        //{
                        //    Strpara += "[" + MappingColumn + "] as [" + FieldName + "],";
                        //}
                        //else
                        //{
                        //    MappingColumn = "";
                        //    Strpara += "'" + MappingColumn + "'   as [" + FieldName + "],";
                        //}



                    }
                    else if (IsStatic == 1)
                    {
                        if (StaticValue.ToLower().Trim() == "blank")
                        {
                            StaticValue = "";
                        }
                        Strpara += " '" + StaticValue + "' as [" + FieldName + "]" + ",";
                    }




                }


                Strpara = Strpara.ToString().Remove(Strpara.ToString().LastIndexOf(","));
                StrVariant = " Union all " + StrVariant + Strpara + " from tb_productvariantvalue ";
                string Strwhr2 = "";
                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    Strwhr2 = "  where sku in (select isnull(sku,'') as sku from tb_product where storeid=" + storeid + " and isnull(deleted,0)=0 and isnull(sku,'')<>'' and isnull(IsOverStockRep,0)=1 and isnull(IsDisplayOnFeed,0)=1) and productid  in (select productid from tb_product where isnull(deleted,0)=0 and storeid=1) ";
                }
                else
                {
                    Strwhr2 = "  where sku in (select isnull(sku,'') as sku from tb_product where storeid=" + storeid + " and isnull(deleted,0)=0 and isnull(sku,'')<>'' and isnull(IsDisplayOnFeed,0)=1) and productid  in (select productid from tb_product where isnull(deleted,0)=0 and storeid=1) ";
                }

                StrVariant = StrVariant + Strwhr2;


                //StrVariant = StrVariant + " Union all ";
                //string child = "";
                //child = "select ";
                //child = child + Strpara + " from tb_productvariantvalue ";
                //string Strwhr2 = "  where upc in (select isnull(upc,'') as upc from tb_product where storeid=" + ddlstore.SelectedValue.ToString() + " and isnull(deleted,0)=0 and isnull(upc,'')<>'') ";
                //StrVariant = StrVariant + child + Strwhr2;

                //  }

                string kohlquery = "";
                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
                {
                    kohlquery = "select 'Always IN' as [FILE TYPE],'your sku #' as [VENDOR SKU],'Yes,No, or Guaranteed' as [AVAILABLE],'Quantity On Hand' as [QTY],'Quantity Arriving Next' as [NEXT AVAILABLE QTY],'Format: MM/DD/YYYY' as [NEXT AVAILABLE DATE],'Product Manufacturer' as [MANUFACTURER],'Manufacturers SKU' as [MANUFACTURER SKU],'Enter Product Description' as [DESCRIPTION],'Enter Cost to Merchant in Dollars' as [UNIT COST],'Enter Unit Price in Dollars' as [UNIT COST 2],' n/a' as [UNIT COST 3],' n/a' as [UNIT COST 4],'Discontinued? 1\' as [DISCONTINUED],'Department Number of Product' as [MERCH. DEPT.],'Usually \"EA\"' as [UNIT OF MEASURE],'SKU Assigned by Merchant' as [MERCHANT SKU],'Enter Merchant Name' as  [MERCHANT],'Enter UPC or EAN #' as [GS1 ID] union all SELECT  [FILE TYPE], [VENDOR SKU],  [AVAILABLE],  cast([QTY] as nvarchar(100)) as [QTY],  cast([NEXT AVAILABLE QTY] as nvarchar(100)) as [NEXT AVAILABLE QTY],  [NEXT AVAILABLE DATE],  [MANUFACTURER],  [MANUFACTURER SKU],  [DESCRIPTION],  [UNIT COST],  [UNIT COST 2],  [UNIT COST 3], [UNIT COST 4], [DISCONTINUED], [MERCH. DEPT.],   [UNIT OF MEASURE],  [MERCHANT SKU],  [MERCHANT],  [GS1 ID] FROM ( ";
                    //kohlquery = "select 'Always \"IN\"' as [FILE TYPE],'your sku #' as [VENDOR SKU],'Yes,\"No\", or \"Guaranteed\"' as [AVAILABLE],'Quantity On Hand' as [QTY],'Quantity Arriving Next' as [NEXT AVAILABLE QTY],'Format: MM/DD/YYYY' as [NEXT AVAILABLE DATE],'Product Manufacturer' as [MANUFACTURER],'Manufacturers SKU' as [MANUFACTURER SKU],'Enter Product Description' as [DESCRIPTION],'Enter Cost to Merchant in Dollars' as [UNIT COST],'Enter Unit Price in Dollars' as [UNIT COST 2],' n/a' as [UNIT COST 3],' n/a' as [UNIT COST 4],'Discontinued? \"1\"' as [DISCONTINUED],'Department Number of Product' as [MERCH. DEPT.],'Usually \"EA\"' as [UNIT OF MEASURE],'SKU Assigned by Merchant' as [MERCHANT SKU],'Enter Merchant Name' as  [MERCHANT],'Enter UPC or EAN #' as [GS1 ID] union all ";      
                }



                String strquery = "";
                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
                {
                    strquery = kohlquery + strqueryProduct + StrVariant + " ) as A";
                }
                else
                {
                    strquery = strqueryProduct + StrVariant;
                }

                //    Response.Write("SELECT DISTINCT C.* FROM (" + strquery + ") as C");
               // Response.Write("SELECT DISTINCT C.* FROM (" + strquery + ") as C");

                DataSet dsEcommProducts = new DataSet();
                //dsEcommProducts = CommonComponent.GetCommonDataSet("SELECT DISTINCT C.* FROM ("+strquery+") as C");

                SqlDataAdapter Adpt = new SqlDataAdapter();
                SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString());
                SqlCommand cmd = new SqlCommand();
                try
                {
                    dsEcommProducts = new DataSet();
                    if (conn.State == ConnectionState.Closed)
                        conn.Open();

                    cmd.CommandType = CommandType.Text;
                    cmd.CommandText = "SELECT DISTINCT C.* FROM (" + strquery + ") as C";
                    cmd.CommandTimeout = 600;
                    cmd.Connection = conn;

                    Adpt.SelectCommand = cmd;
                    Adpt.Fill(dsEcommProducts);
                }
                catch (Exception ex)
                {
                    dsEcommProducts = null;

                    if (conn.State == ConnectionState.Open)
                        conn.Close();
                }
                finally
                {
                    if (conn != null)
                        if (conn.State == ConnectionState.Open) conn.Close();
                    cmd.Dispose();
                    Adpt.Dispose();
                }


                //Response.Write("SELECT DISTINCT C.* FROM (" + strquery + ") as C");
                if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0 && dsEcommProducts.Tables[0].Rows.Count > 0)
                {
                    if (radiocsv.Checked)
                    {
                        GenerateCSV(dsEcommProducts);
                    }
                    else if (radioExcel.Checked)
                    {
                        GenerateExcel(dsEcommProducts);
                    }



                }
                else
                {
                    if (dsEcommProducts != null && dsEcommProducts.Tables.Count > 0)
                    {
                        if (radiocsv.Checked)
                        {
                            GenerateCSV(dsEcommProducts);
                        }
                        else if (radioExcel.Checked)
                        {
                            GenerateExcel(dsEcommProducts);
                        }



                    }
                }


            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore", "alert('Template Data not Found.');", true);
                return;
            }


        }
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            sFieldValueToEscape = sFieldValueToEscape.Replace("\\r\\n", System.Environment.NewLine);
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
            }
        }


        private void GenerateExcel(DataSet Dstt)
        {
            DataSet Ds = new DataSet();
            string overrepemail = "";
            overrepemail = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,'') from tb_AppConfig where ConfigName='OverStockRepEmailID' and storeid=1"));

            DataTable dttemp = Dstt.Tables[0].Clone();

            for (int i = 0; i < dttemp.Columns.Count; i++)
            {
                dttemp.Columns[i].DataType = typeof(string);
            }
            foreach (DataRow row in Dstt.Tables[0].Rows)
            {
                dttemp.ImportRow(row);
            }


            Ds.Tables.Add(dttemp);
            try
            {

                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    DataTable dt1 = new DataTable();
                    DataColumn col1 = new DataColumn("ColumnName", typeof(string));
                    dt1.Columns.Add(col1);
                    DataColumn col2 = new DataColumn("ColumnNo", typeof(int));
                    dt1.Columns.Add(col2);
                    DataColumn col3 = new DataColumn("flag", typeof(int));
                    dt1.Columns.Add(col3);
                    DataSet dsdefaultdata = new DataSet();
                    for (int j = 0; j < Ds.Tables[0].Columns.Count; j++)
                    {
                        dsdefaultdata = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID in (SELECT ReplenishmentFieldID FROM tb_Replenishment_FeedtemplateDetail WHERE FieldName='" + Ds.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''") + "' and InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + "))");
                        if (dsdefaultdata != null && dsdefaultdata.Tables.Count > 0 && dsdefaultdata.Tables[0].Rows.Count > 0)
                        {
                            DataRow drr = dt1.NewRow();

                            drr["ColumnName"] = Ds.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''");//dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString();
                            drr["ColumnNo"] = j;
                            drr["flag"] = 1;
                            dt1.Rows.Add(drr);
                        }
                        else
                        {
                            DataRow drr = dt1.NewRow();
                            drr["ColumnName"] = Ds.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''");//dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString();
                            drr["ColumnNo"] = j;
                            drr["flag"] = 0;
                            dt1.Rows.Add(drr);
                        }
                    }



                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        for (int c = 0; c < dt1.Rows.Count; c++)
                        {


                            dsdefaultdata = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID in (SELECT ReplenishmentFieldID FROM tb_Replenishment_FeedtemplateDetail WHERE FieldName='" + Ds.Tables[0].Columns[c].ColumnName.ToString().Replace("'", "''") + "' and InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + "))");
                            if (dsdefaultdata != null && dsdefaultdata.Tables.Count > 0 && dsdefaultdata.Tables[0].Rows.Count > 0)
                            {


                                for (int kk = 0; kk < dsdefaultdata.Tables[0].Rows.Count; kk++)
                                {
                                    try
                                    {
                                        if (!string.IsNullOrEmpty(dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString()) && dt1.Rows[c]["flag"].ToString() == "1" && !string.IsNullOrEmpty(Ds.Tables[0].Rows[i][dt1.Rows[c]["ColumnName"].ToString()].ToString()) && Convert.ToBoolean(dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString()) == Convert.ToBoolean(Ds.Tables[0].Rows[i][dt1.Rows[c]["ColumnName"].ToString()].ToString()))
                                        {
                                            Ds.Tables[0].Rows[i][c] = (dsdefaultdata.Tables[0].Rows[kk]["Assignedvalue"].ToString());
                                            Ds.Tables[0].AcceptChanges();


                                        }

                                    }
                                    catch
                                    {

                                    }

                                    //if (chk == false)
                                    //{
                                    //    Ds.Tables[0].Rows[i][c] = Ds.Tables[0].Rows[i][c].ToString().Trim();
                                    //}

                                }



                            }
                            else
                            {
                                if (!String.IsNullOrEmpty(Ds.Tables[0].Rows[i][c].ToString().Trim()))
                                {
                                    Ds.Tables[0].Rows[i][c] = (Ds.Tables[0].Rows[i][c].ToString().Trim().Replace("01/01/00", "").Replace("01/01/1900", "").Replace("1/1/1900", "").Replace("1/1/00", ""));
                                }
                                else
                                {
                                    Ds.Tables[0].Rows[i][c] = "";
                                }
                                // Ds.Tables[0].Rows[i][c] = (Ds.Tables[0].Rows[i][c].ToString().Trim());
                            }

                        }

                    }
                }


                DateTime dt = DateTime.Now;
                string StrStorename = "";
                //    StrStorename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Filename FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + ""));// ddlstore.SelectedItem.ToString().Replace(" ", "_");
                String FileName = "";
                if (txtstorefile.Text.ToString().Trim() != "")
                {
                    StrStorename = txtstorefile.Text.ToString().Trim();
                    FileName = StrStorename.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".xls";
                }
                else
                {
                    //  FileName = StrStorename.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + "_" + dt.Day + "_" + dt.Year + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second + ".xls";
                    FileName = ddlstore.SelectedItem.Text.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".xls";


                }

                if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName + "")))
                {
                    for (int i = 1; i < 100; i++)
                    {
                        if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + i + ".xls")))
                        {


                        }
                        else
                        {
                            FileName = FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + i.ToString() + ".xls";
                            break;
                        }
                    }
                }

                if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));
                //xlWorkBook.SaveAs(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName), Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal, misValue, misValue, misValue, misValue, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlExclusive, misValue, misValue, misValue, misValue, misValue);
                //xlWorkBook.Close(true, misValue, misValue);
                //xlApp.Quit();
                //releaseObject(xlWorkSheet);
                //releaseObject(xlWorkBook);
                //releaseObject(xlApp);
                int[] iColumns = new int[Ds.Tables[0].Columns.Count];

                string overstockheader = "";

                overstockheader = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,'') from tb_AppConfig where ConfigName='ShowOverStockheader' and storeid=1"));
                Boolean ShowOverStockheader = false;
                Boolean.TryParse(overstockheader, out ShowOverStockheader);

                for (int i = 1; i < Ds.Tables[0].Columns.Count; i++)
                {
                    iColumns[i] = i;

                }
                if ((ddlstore.SelectedItem.Text.ToString().ToLower() == "over" || ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk" || ddlstore.SelectedItem.Text.ToString().ToLower() == "over stock" || ddlstore.SelectedItem.Text.ToString().ToLower() == "overstock" || ddlstore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1))
                {
                    if ((ShowOverStockheader == false))
                    {
                        for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                        {
                            string clo = " ";
                            for (int o = 0; o < i; o++)
                            {
                                clo = clo + " ";
                            }
                            Ds.Tables[0].Columns[i].ColumnName = clo;
                            Ds.Tables[0].AcceptChanges();
                        }
                    }
                }
                else if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
                {

                    DataRow dr = Ds.Tables[0].NewRow();
                    //for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    //{
                    //    if (i == 0)
                    //    {
                    //        dr[i] = "MANDATORY COLUMNS ARE MARKED IN ORANGE, RECOMMENDED COLUMNS ARE MARKED IN BLUE.  ** Reference the Supplier Guide for details on Best Practice for your Inventory Updates.";
                    //    }
                    //    else
                    //    {
                    //        dr[i] = " ";
                    //    }

                    //}
                    //Ds.Tables[0].Rows.InsertAt(dr, 0);
                    //dr = Ds.Tables[0].NewRow();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            dr[i] = "When you are done click \"File\" and then select \"Save As\".";
                        }
                        else if (i == 1)
                        {
                            dr[i] = "**Reference the Supplier Guide for details on Best Practice for your Inventory Updates.";
                        }
                        else
                        {
                            dr[i] = " ";
                        }

                    }
                    Ds.Tables[0].Rows.InsertAt(dr, 0);
                    dr = Ds.Tables[0].NewRow();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            dr[i] = "At the Bottom of the Save window make sure the \"Save As Type\" dropdown menu has Excel 97-2003 workbook (*.xls) or Excel workbook (*.xlsx) selected.";
                        }
                        else
                        {
                            dr[i] = " ";
                        }

                    }
                    Ds.Tables[0].Rows.InsertAt(dr, 1);
                    dr = Ds.Tables[0].NewRow();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        dr[i] = Ds.Tables[0].Columns[i].ColumnName.ToString();
                    }
                    Ds.Tables[0].Rows.InsertAt(dr, 2);

                    Ds.Tables[0].AcceptChanges();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            Ds.Tables[0].Columns[i].ColumnName = "MANDATORY COLUMNS ARE MARKED IN ORANGE.RECOMMENDED COLUMNS ARE MARKED IN BLUE. ";
                        }

                        else
                        {
                            string clo = " ";
                            for (int o = 0; o < i; o++)
                            {
                                clo = clo + " ";
                            }

                            Ds.Tables[0].Columns[i].ColumnName = clo;
                        }

                    }
                    Ds.Tables[0].AcceptChanges();
                }
                else if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    DataRow dr = Ds.Tables[0].NewRow();
                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        dr[i] = Ds.Tables[0].Columns[i].ColumnName.ToString();
                    }
                    Ds.Tables[0].Rows.InsertAt(dr, 0);
                    Ds.Tables[0].AcceptChanges();


                    for (int i = 0; i < Ds.Tables[0].Columns.Count; i++)
                    {
                        if (i == 0)
                        {
                            Ds.Tables[0].Columns[i].ColumnName = overrepemail;
                            //   Ds.Tables[0].Columns[i].ColumnName = "artmoyatheeff.com";
                        }
                        else if (i == 1)
                        {
                            Ds.Tables[0].Columns[i].ColumnName = "83290";
                        }
                        else if (i == 2)
                        {
                            Ds.Tables[0].Columns[i].ColumnName = "replenishment";
                        }

                        else
                        {
                            string clo = " ";
                            for (int o = 0; o < i; o++)
                            {
                                clo = clo + " ";
                            }

                            Ds.Tables[0].Columns[i].ColumnName = clo;
                        }

                    }


                    Ds.Tables[0].AcceptChanges();

                }



                try
                {
                    RKLib.ExportData.Export objExport = new RKLib.ExportData.Export("Win");

                    objExport.ExportDetails(Ds.Tables[0], iColumns, RKLib.ExportData.Export.ExportFormat.Excel, Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName));
                }
                catch { }

                // ExcelLibrary.DataSetHelper.CreateWorkbook(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName), Ds);
                ViewState["LastGeneratedFeedFileName"] = null;
                String FilePath = Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName);

                ViewState["LastGeneratedFeedFileName"] = FileName.ToString();


                if (ViewState["LastGeneratedFeedFileName"] != null)
                {
                    GenerateInventoryFeedComponent objinv = new GenerateInventoryFeedComponent();
                    if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
                    {
                        objinv.InsertFeedLog(ViewState["LastGeneratedFeedFileName"].ToString(), Convert.ToInt32(Session["AdminID"].ToString()), Convert.ToInt32(ddlstore.SelectedValue.ToString()));
                    }
                    // DownloadProductExportExcel();
                    btndownloadnow.Visible = true;

                    // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Data Exported Successfully.');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Record(s) not Found.');", true);
                    return;
                }


            }
            catch (Exception ex)
            {
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('" + ex.Message.ToString() + "');", true);
                Response.Write(ex.Message.ToString());
            }


        }
        private void releaseObject(object obj)
        {
            try
            {
                System.Runtime.InteropServices.Marshal.ReleaseComObject(obj);

                obj = null;
            }
            catch (Exception ex)
            {
                obj = null;
            }
            finally
            {
                GC.Collect();
            }
        }
        private void GenerateCSV(DataSet Ds)
        {


            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();
            dsorder = Ds;
            SecurityComponent objsec = new SecurityComponent();

            string column = "";
            string columnnom = "";
            string overstockheader = "";
            string overrepemail = "";
            overrepemail = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,'') from tb_AppConfig where ConfigName='OverStockRepEmailID' and storeid=1"));
            overstockheader = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(ConfigValue,'') from tb_AppConfig where ConfigName='ShowOverStockheader' and storeid=1"));
            Boolean ShowOverStockheader = false;
            Boolean.TryParse(overstockheader, out ShowOverStockheader);

            if (ddlstore.SelectedItem.Text.ToString().ToLower() != "over" && ddlstore.SelectedItem.Text.ToString().ToLower() != "ostk" && ddlstore.SelectedItem.Text.ToString().ToLower() != "over stock" && ddlstore.SelectedItem.Text.ToString().ToLower() != "overstock" && ddlstore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") <= -1)
            {
                if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Columns.Count > 0)
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
            }
            else
            {
                if (ShowOverStockheader == false)
                {
                    if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Columns.Count > 0)
                    {
                        for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                        {
                            if (dsorder.Tables[0].Columns.Count - 1 == i)
                            {
                                // column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                                columnnom += "{" + i.ToString() + "}";
                            }
                            else
                            {

                                //  column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                                columnnom += "{" + i.ToString() + "},";
                            }
                        }
                    }
                }
                else
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
            }



            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                DataSet dsdefaultdata = new DataSet();
                DataTable dt = new DataTable();
                DataColumn col1 = new DataColumn("ColumnName", typeof(string));
                dt.Columns.Add(col1);
                DataColumn col2 = new DataColumn("ColumnNo", typeof(int));
                dt.Columns.Add(col2);
                DataColumn col3 = new DataColumn("flag", typeof(int));
                dt.Columns.Add(col3);
                //   DataRow dr = null;

                for (int j = 0; j < dsorder.Tables[0].Columns.Count; j++)
                {

                    dsdefaultdata = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID in (SELECT ReplenishmentFieldID FROM tb_Replenishment_FeedtemplateDetail WHERE FieldName='" + dsorder.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''") + "' and InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + "))");
                    if (dsdefaultdata != null && dsdefaultdata.Tables.Count > 0 && dsdefaultdata.Tables[0].Rows.Count > 0)
                    {
                        DataRow drr = dt.NewRow();

                        drr["ColumnName"] = dsorder.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''");//dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString();
                        drr["ColumnNo"] = j;
                        drr["flag"] = 1;
                        dt.Rows.Add(drr);
                    }
                    else
                    {
                        DataRow drr = dt.NewRow();
                        drr["ColumnName"] = dsorder.Tables[0].Columns[j].ColumnName.ToString().Replace("'", "''");//dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString();
                        drr["ColumnNo"] = j;
                        drr["flag"] = 0;
                        dt.Rows.Add(drr);
                    }


                }

                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dt.Rows.Count; c++)
                    {


                        //DataSet dsdefaultdata = new DataSet();
                        dsdefaultdata = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Replenishment_configure WHERE FieldID in (SELECT ReplenishmentFieldID FROM tb_Replenishment_FeedtemplateDetail WHERE FieldName='" + Ds.Tables[0].Columns[c].ColumnName.ToString().Replace("'", "''") + "' and InventoryFeedTemplateID in (SELECT InventoryFeedTemplateID FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + "))");
                        if (dsdefaultdata != null && dsdefaultdata.Tables.Count > 0 && dsdefaultdata.Tables[0].Rows.Count > 0)
                        {
                            bool chk = false;

                            for (int kk = 0; kk < dsdefaultdata.Tables[0].Rows.Count; kk++)
                            {
                                try
                                {
                                    if (!string.IsNullOrEmpty(dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString()) && !string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][dt.Rows[c]["ColumnName"].ToString()].ToString()) && dt.Rows[c]["flag"].ToString() == "1" && Convert.ToBoolean(dsdefaultdata.Tables[0].Rows[kk]["Dbvalue"].ToString()) == Convert.ToBoolean(dsorder.Tables[0].Rows[i][dt.Rows[c]["ColumnName"].ToString()].ToString()))
                                    {
                                        chk = true;
                                        args[c] = _EscapeCsvField(dsdefaultdata.Tables[0].Rows[kk]["Assignedvalue"].ToString());
                                    }
                                }
                                catch
                                {

                                }




                                if (chk == false)
                                {
                                    // args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                                    if (!String.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString()))
                                    {
                                        args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                                    }
                                    else
                                    {
                                        args[c] = "";
                                    }
                                }
                            }

                        }
                        else
                        {
                            //args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                            if (!String.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString()))
                            {
                                args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim().Replace("01/01/00", "").Replace("01/01/1900", "").Replace("1/1/1900", "").Replace("1/1/00", ""));
                            }
                            else
                            {
                                args[c] = "";
                            }
                        }




                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
            }
            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                string FullString = sb.ToString();
                sb.Remove(0, sb.Length);
                if (ddlstore.SelectedItem.Text.ToString().ToLower() == "kohl" || ddlstore.SelectedItem.Text.ToString().ToLower() == "kohls")
                {
                    sb.AppendLine("MANDATORY COLUMNS ARE MARKED IN ORANGE, RECOMMENDED COLUMNS ARE MARKED IN BLUE.  ** Reference the Supplier Guide for details on Best Practice for your Inventory Updates.");
                    sb.AppendLine("When you are done click \"File\" and then select \"Save As\".");
                    sb.AppendLine("At the Bottom of the Save window make sure the \"Save As Type\" dropdown menu has Excel 97-2003 workbook (*.xls) or Excel workbook (*.xlsx) selected.");
                }
                else if (ddlstore.SelectedItem.Text.ToString().ToLower() == "ostk rep")
                {
                    object[] argsover = new object[3];
                    argsover[0] = overrepemail;
                    argsover[1] = "83290";
                    argsover[2] = "replenishment";
                    sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\"", argsover));
                }
                sb.AppendLine(FullString);

                DateTime dt = DateTime.Now;
                string StrStorename = "";
                // StrStorename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Filename FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + ""));// ddlstore.SelectedItem.ToString().Replace(" ", "_");
                String FileName = "";


                if (txtstorefile.Text.ToString().Trim() != "")
                {
                    StrStorename = txtstorefile.Text.ToString().Trim();
                    // suggestedfilename = StrStorename.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".csv";
                    FileName = StrStorename.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".csv";
                }
                else
                {
                    //  FileName = StrStorename.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + "_" + dt.Day + "_" + dt.Year + "_" + dt.Hour + "_" + dt.Minute + "_" + dt.Second + ".csv";
                    FileName = ddlstore.SelectedItem.Text.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + ".csv";



                }




                if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName + "")))
                {
                    //FileName = FileName + "_" + dt.Month + dt.Day + dt.Year + ".csv";
                    //if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName + "")))
                    //{
                    for (int i = 1; i < 100; i++)
                    {

                        if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + i + ".csv")))
                        {


                        }
                        else
                        {
                            FileName = FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + i.ToString() + ".csv";
                            break;
                        }
                    }
                    //  }
                }
                //else
                //{
                //    FileName = FileName + "_" + dt.Month + dt.Day + dt.Year + ".csv";
                //    if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName + "")))
                //    {
                //        for (int i = 1; i < 100; i++)
                //        {
                //            if (File.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName + "")))
                //            {
                //                FileName = FileName.ToString().Replace(".xls", "").Replace(".csv", "").Replace(".xlsx", "") + "_" + dt.Month + dt.Day + dt.Year + "_" + i + ".csv";
                //            }
                //            else
                //            {
                //               // FileName = FileName + "_" + dt.Month + dt.Day + dt.Year + ".csv";
                //                break;
                //            }
                //        }
                //    }
                //}




                if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));

                ViewState["LastGeneratedFeedFileName"] = null;
                String FilePath = Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + FileName);

                ViewState["LastGeneratedFeedFileName"] = FileName.ToString();
                WriteFile(sb.ToString(), FilePath);
            }
            if (ViewState["LastGeneratedFeedFileName"] != null)
            {
                GenerateInventoryFeedComponent objinv = new GenerateInventoryFeedComponent();
                if (Session["AdminID"] != null && Session["AdminID"].ToString() != "")
                {
                    objinv.InsertFeedLog(ViewState["LastGeneratedFeedFileName"].ToString(), Convert.ToInt32(Session["AdminID"].ToString()), Convert.ToInt32(ddlstore.SelectedValue.ToString()));
                }

                btndownloadnow.Visible = true;


                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Data Exported Successfully.');", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('Record(s) not Found.');", true);
                return;
            }


        }

        protected void DownloadProductExportExcel()
        {
            if (ViewState["LastGeneratedFeedFileName"] != null)
            {
                String FilePath = Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + ViewState["LastGeneratedFeedFileName"].ToString());
                if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));

                if (File.Exists(FilePath))
                {



                    Response.Clear();
                    Response.ClearContent();
                    Response.ContentType = "application/ms-excel";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["LastGeneratedFeedFileName"].ToString());
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('File not found.');", true);
                return;
            }
        }


        protected void DownloadProductExportCSV()
        {
            if (ViewState["LastGeneratedFeedFileName"] != null)
            {
                String FilePath = Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/" + ViewState["LastGeneratedFeedFileName"].ToString());
                if (!Directory.Exists(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/REPLENISHMENTMANAGEMENT/Files/"));

                if (File.Exists(FilePath))
                {
                    Response.Clear();
                    Response.ClearContent();
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["LastGeneratedFeedFileName"].ToString());
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "alert('File not found.');", true);
                return;
            }
        }
        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text">String Text</param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }

        protected void btngeneratefeed_Click(object sender, EventArgs e)
        {
            btndownloadnow.Visible = false;
            GetFieldTemplate();
        }

        protected void btndownloadnow_Click(object sender, EventArgs e)
        {
            if (ViewState["LastGeneratedFeedFileName"] != null)
            {
                if (ViewState["LastGeneratedFeedFileName"].ToString().ToLower().IndexOf(".csv") > -1)
                {
                    DownloadProductExportCSV();
                }
                else
                {
                    DownloadProductExportExcel();
                }

            }
        }

        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {

            btndownloadnow.Visible = false;
            txtstorefile.Text = "";
            radiocsv.Checked = true;
            radioExcel.Checked = false;
            if (ddlstore.SelectedValue.ToString() != "0")
            {
                string filename = "";
                filename = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT replace(replace(isnull(Filename,''),'.csv',''),'.xls','') as Filename FROM tb_Replenishment_Feedtemplate WHERE StoreId=" + ddlstore.SelectedValue.ToString() + ""));
                if (!String.IsNullOrEmpty(filename))
                {
                    txtstorefile.Text = filename;
                }
            }

        }
    }
}