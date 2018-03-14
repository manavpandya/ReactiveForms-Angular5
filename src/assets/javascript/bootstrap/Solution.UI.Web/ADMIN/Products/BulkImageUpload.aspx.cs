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

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class BulkImageUpload : BasePage
    {

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStore();
                FillProductsColorbysale();
                //ViewState["ImagePathProduct"] = 
              
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
                dsCustomer = CommonComponent.GetCommonDataSet("select * from  tb_Product where isnull(Active,0)=1 and isnull(Deleted,0)=0 and StoreID=" + Convert.ToInt32(ddlStore.SelectedValue) + " and (SKU like '%" + txtSearch.Text.Trim() + "%' OR name like '%" + txtSearch.Text.Trim() + "%' OR UPC like '%" + txtSearch.Text.Trim() + "%'  )");
            }
            else
            {
                dsCustomer = CommonComponent.GetCommonDataSet("select * from  tb_Product where isnull(Active,0)=1 and isnull(Deleted,0)=0 and (SKU like '%" + txtSearch.Text.Trim() + "%' OR name like '%" + txtSearch.Text.Trim() + "%' OR UPC like '%" + txtSearch.Text.Trim() + "%'  ) ");
            }
            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
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
            imagepath = AppLogic.AppConfigs("ImagePathProductBulk") + "Micro/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProductBulk") + "Micro/image_not_available.jpg");
        }
     

        public String GetIconImageCount(String ProductID,String ImageName)
        {
            int count = 0;
            DataSet ds = (CommonComponent.GetCommonDataSet("select isnull(Imagename,'') from tb_ProductImgDesc where isnull(Imagename,'')<>'' and ProductId=" + ProductID + ""));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            { String imagepath = String.Empty;
                count=0;
                for(int i=0;i<ds.Tables[0].Rows.Count;i++)
                {
                imagepath = AppLogic.AppConfigs("ImagePathProductBulk") + "Micro/" + ds.Tables[0].Rows[i][0].ToString();
                if(File.Exists(Server.MapPath(imagepath)))
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
           return count.ToString() ;
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
            FillProductsColorbysale();
        }

        protected void grdProductColorSalesReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                //ImageButton btnedit = (ImageButton)e.Row.FindControl("btnedit");
                //btnedit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
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