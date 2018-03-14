using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;


namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class BulkImageReport : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["datarecord"] = null;
                BindStore();
                FillProductsColorbysale();

            }

            btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; padding-right:0px; height: 23px; border:none;cursor:pointer;");

        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
            if (!String.IsNullOrEmpty(Request.QueryString["storeid"]))
            {
                ddlStore.SelectedValue = Convert.ToString(Request.QueryString["storeid"]);

            }
        }

        /// <summary>
        /// Fills the product by sale
        /// </summary>
        private void FillProductsColorbysale()
        {
            DataSet dsCustomer = new DataSet();
            //  dsCustomer = DashboardComponent.GetProductColorBySales(Convert.ToInt32(ddlStore.SelectedValue));
            if (ddlStore.SelectedIndex > 0)
            {
                dsCustomer = CommonComponent.GetCommonDataSet("SELECT A.*,case when storeId=1 then  dbo.Producthamming_Scalar_TEMP(A.UPC,A.SKU,storeId) else dbo.Producthamming_Scalar(A.UPC,A.SKU,A.storeId) end as Inventory1 FROM (select *,  ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID,TotalRows=COUNT(*) OVER()  from  tb_Product where isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreID=" + Convert.ToInt32(ddlStore.SelectedValue) + " and (SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR name like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR ProductID in (select ProductID from tb_ProductVariantValue where SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' or UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' ))) AS A WHERE A.RowID>=0 and A.RowID <=20");
            }
            else
            {
                dsCustomer = CommonComponent.GetCommonDataSet("SELECT A.*,case when storeId=1 then  dbo.Producthamming_Scalar_TEMP(A.UPC,A.SKU,storeId) else dbo.Producthamming_Scalar(A.UPC,A.SKU,A.storeId) end as Inventory1 FROM (select *,  ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID,TotalRows=COUNT(*) OVER()  from  tb_Product where isnull(Active,0)=1 and isnull(Deleted,0)=0 and (SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR name like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR ProductID in (select ProductID from tb_ProductVariantValue where SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' or UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' ))) AS A WHERE A.RowID>=0 and A.RowID <=20");
                //dsCustomer = CommonComponent.GetCommonDataSet("select *, case when storeId=1 then  dbo.Producthamming_Scalar_TEMP(UPC,SKU,storeId) else dbo.Producthamming_Scalar(UPC,SKU,storeId) end as Inventory1 from  tb_Product where isnull(Active,0)=1 and isnull(Deleted,0)=0 and (SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR name like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR ProductID in (select ProductID from tb_ProductVariantValue where SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' )  ) ");
            }

            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {

                Int32 PageCount = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["TotalRows"].ToString());
                for (int i = 20; i < PageCount; i++)
                {
                    DataRow dr = dsCustomer.Tables[0].NewRow();
                    dsCustomer.Tables[0].Rows.Add(dr);
                }
                dsCustomer.Tables[0].AcceptChanges();
                grdProductColorSalesReport.DataSource = dsCustomer;

                grdProductColorSalesReport.DataBind();
            }
            else
            {
                Session["datarecord"] = null;
                grdProductColorSalesReport.DataSource = null;
                grdProductColorSalesReport.DataBind();
            }
        }
        private void FillProductsColorbysalePaging()
        {
            DataSet dsCustomer = new DataSet();
            //  dsCustomer = DashboardComponent.GetProductColorBySales(Convert.ToInt32(ddlStore.SelectedValue));
            Int32 i = grdProductColorSalesReport.PageIndex;
            Int32 startRowId = 0;
            Int32 endRowId = 0;
            startRowId = i * 20;
            endRowId = startRowId + 20 + 1;
            if (ddlStore.SelectedIndex > 0)
            {
                dsCustomer = CommonComponent.GetCommonDataSet("SELECT A.*,case when storeId=1 then  dbo.Producthamming_Scalar_TEMP(A.UPC,A.SKU,storeId) else dbo.Producthamming_Scalar(A.UPC,A.SKU,A.storeId) end as Inventory1 FROM (select *,  ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID,TotalRows=COUNT(*) OVER()  from  tb_Product where isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreID=" + Convert.ToInt32(ddlStore.SelectedValue) + " and (SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR name like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR ProductID in (select ProductID from tb_ProductVariantValue where SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' or UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' ))) AS A WHERE A.RowID >" + startRowId + " and A.RowID < " + endRowId + "");
            }
            else
            {
                dsCustomer = CommonComponent.GetCommonDataSet("SELECT A.*,case when storeId=1 then  dbo.Producthamming_Scalar_TEMP(A.UPC,A.SKU,storeId) else dbo.Producthamming_Scalar(A.UPC,A.SKU,A.storeId) end as Inventory1 FROM (select *,  ROW_NUMBER() OVER (ORDER BY SKu desc) As RowID,TotalRows=COUNT(*) OVER()  from  tb_Product where isnull(Active,0)=1 and isnull(Deleted,0)=0 and (SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR name like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR ProductID in (select ProductID from tb_ProductVariantValue where SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' or UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' ))) AS A WHERE A.RowID >" + startRowId + " and A.RowID < " + endRowId + "");
                //dsCustomer = CommonComponent.GetCommonDataSet("select *, case when storeId=1 then  dbo.Producthamming_Scalar_TEMP(UPC,SKU,storeId) else dbo.Producthamming_Scalar(UPC,SKU,storeId) end as Inventory1 from  tb_Product where isnull(Active,0)=1 and isnull(Deleted,0)=0 and (SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR name like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR ProductID in (select ProductID from tb_ProductVariantValue where SKU like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' OR UPC like '%" + txtSearch.Text.Trim().Replace("'", "''") + "%' )  ) ");
            }


            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                Int32 PageCount = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["TotalRows"].ToString());
                if( i > 0)
                {
                    for (int j = 0; j < startRowId; j++)
                    {
                        DataRow dr = dsCustomer.Tables[0].NewRow();
                        dsCustomer.Tables[0].Rows.InsertAt(dr, j);
                    }
                    dsCustomer.Tables[0].AcceptChanges();
                   
                    for (int j = endRowId - 1; j < PageCount; j++)
                    {
                        DataRow dr = dsCustomer.Tables[0].NewRow();
                        dsCustomer.Tables[0].Rows.InsertAt(dr, j);
                    }
                    dsCustomer.Tables[0].AcceptChanges();
                }
                else
                {
                    for (int j = 20; j < PageCount; j++)
                    {
                        DataRow dr = dsCustomer.Tables[0].NewRow();
                        dsCustomer.Tables[0].Rows.Add(dr);
                    }
                    dsCustomer.Tables[0].AcceptChanges();
                }
                
              
                grdProductColorSalesReport.DataSource = dsCustomer;

                grdProductColorSalesReport.DataBind();
            }
            else
            {
                grdProductColorSalesReport.DataSource = null;
                grdProductColorSalesReport.DataBind();
            }



        }


        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Icon Product Image Full Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Micro/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
        }


        public String GetIconImageCount(String ProductID, String ImageName)
        {
            int count = 0;
            DataSet ds = (CommonComponent.GetCommonDataSet("select isnull(Imagename,'') from tb_ProductImgDesc where isnull(Imagename,'')<>'' and ProductId=" + ProductID + ""));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                String imagepath = String.Empty;
                count = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    imagepath = AppLogic.AppConfigs("ImagePathProductBulk") + "Micro/" + ds.Tables[0].Rows[i][0].ToString();
                    if (File.Exists(Server.MapPath(imagepath)))
                    {
                        count++;
                    }
                }
            }

            String imagepath1 = AppLogic.AppConfigs("ImagePathProductBulk") + "Micro/" + ImageName;
            if (File.Exists(Server.MapPath(imagepath1)))
            {
                count = count + 1;
            }


            //if(count>0)
            //{
            //    count = count + 1;
            //}
            //else
            //{
            //    count = 0;
            //}
            return count.ToString();
        }



        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillProductsColorbysale();
        }

        /// <summary>
        ///  Product Sale Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdProductColorSalesReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdProductColorSalesReport.PageIndex = e.NewPageIndex;
            FillProductsColorbysalePaging();
        }

        protected void grdProductColorSalesReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                //ImageButton btnedit = (ImageButton)e.Row.FindControl("btnedit");
                //btnedit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";

                Label lblSKU = (Label)e.Row.FindControl("lblSKU");
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Label lblstoreid = (Label)e.Row.FindControl("lblstoreid");
                Label lblInventory = (Label)e.Row.FindControl("lblInventory");


                System.Web.UI.HtmlControls.HtmlAnchor asaleorderid = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("asaleorderid");
                asaleorderid.HRef = "PhoneOrder.aspx?searchlinksku=" + lblSKU.Text; asaleorderid.Target = "_blank";

                System.Web.UI.HtmlControls.HtmlAnchor aquoteid = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("aquoteid");
                aquoteid.HRef = "CustomerQuote.aspx?searchlinksku=" + lblSKU.Text; aquoteid.Target = "_blank";

                GridView grdVariant = (GridView)e.Row.FindControl("grdVariant");
                DataSet dsn = new DataSet();
                if (lblstoreid.Text.ToString().Trim() == "1")
                {
                    dsn = CommonComponent.GetCommonDataSet("select VariantValue,SKU,dbo.Producthamming_Scalar_TEMP(UPC,SKU," + lblstoreid.Text.ToString() + ") as Inventory,ProductID from tb_ProductVariantValue where productid=" + lblProductID.Text.ToString() + " and isnull(sku,'')<>''");
                }
                else
                {
                    dsn = CommonComponent.GetCommonDataSet("select VariantValue,SKU,dbo.Producthamming_Scalar(UPC,SKU," + lblstoreid.Text.ToString() + ") as Inventory,ProductID from tb_ProductVariantValue where productid=" + lblProductID.Text.ToString() + " and isnull(sku,'')<>''");
                }

                if (dsn != null && dsn.Tables.Count > 0 && dsn.Tables[0].Rows.Count > 0)
                {
                    grdVariant.DataSource = dsn.Tables[0];
                    grdVariant.DataBind();
                    lblInventory.Text = "";
                }
                else
                {
                    grdVariant.DataSource = null;
                    grdVariant.DataBind();
                }


            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            FillProductsColorbysale();
        }

        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            FillProductsColorbysale();
        }
    }
}