using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
namespace Solution.UI.Web.ADMIN.Products
{
    public partial class Products : Solution.UI.Web.BasePage
    {
        ProductComponent objProductComponent = new ProductComponent();
        tb_Product objProductctx = new tb_Product();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                bindProductDetails();

            ibtnUpdate.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save-changes.png";

        }

        /// <summary>
        /// Method to Bind Product Detail By Category ID
        /// </summary>
        private void bindProductDetails()
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["StoreID"] != null)
            {
                if (Request.QueryString["mode"] == "product")
                    ibtnUpdate.Visible = false;
                else
                    ibtnUpdate.Visible = true;
                DataSet dsProduct = ProductComponent.GetProductDetailByCategoryID(Convert.ToInt16(Request.QueryString["ID"]), Convert.ToInt16(Request.QueryString["StoreID"]));
                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    gvProduct.DataSource = dsProduct;
                    gvProduct.DataBind();
                }
                else
                {
                    gvProduct.DataSource = null;
                    gvProduct.DataBind();
                    ibtnUpdate.Visible = false;
                }
            }
        }

        /// <summary>
        /// Product Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtDisplay = (TextBox)e.Row.FindControl("txtDisplay");
                TextBox txtInventory = (TextBox)e.Row.FindControl("txtInventory");
                TextBox txtPrice = (TextBox)e.Row.FindControl("txtPrice");
                TextBox txtSaleprice = (TextBox)e.Row.FindControl("txtSaleprice");
                HyperLink hplEdit = (HyperLink)e.Row.FindControl("hplEdit");

                hplEdit.ImageUrl = "/App_Themes/" + Page.Theme + "/images/Edit.gif";

                if (txtDisplay != null)
                    txtDisplay.Attributes.Add("onkeypress", "return isNumberKeyDisplay(event)");

                if (txtInventory != null)
                    txtInventory.Attributes.Add("onkeypress", "return isNumberKeyDisplay(event)");

                if (txtPrice != null)
                    txtPrice.Attributes.Add("onkeypress", "return isNumberKey(event)");

                if (txtSaleprice != null)
                    txtSaleprice.Attributes.Add("onkeypress", "return isNumberKey(event)");
            }

            #region based on mode visible & invisible column

            //Help for Columns
            //Columns[0] = Name
            //Columns[1] = SKU
            //Columns[2] = DisplayOrder
            //Columns[3] = Inventory
            //Columns[4] = Price
            //Columns[5] = SalePrice
            //Columns[6] = UpdatedOn
            //Columns[7] = UpdatedBy
            //Columns[8] = Edit

            if (Request.QueryString["mode"] == "DO")
            {
                this.gvProduct.Columns[0].ItemStyle.Width = 300;
                this.gvProduct.Columns[1].ItemStyle.Width = 20;
                this.gvProduct.Columns[2].ItemStyle.Width = 20;
                this.gvProduct.Columns[3].Visible = false;
                this.gvProduct.Columns[4].Visible = false;
                this.gvProduct.Columns[5].Visible = false;
                this.gvProduct.Columns[6].Visible = false;
                this.gvProduct.Columns[7].Visible = false;
                this.gvProduct.Columns[8].ItemStyle.Width = 20;
            }
            if (Request.QueryString["mode"] == "inventory")
            {
                this.gvProduct.Columns[0].ItemStyle.Width = 250;
                this.gvProduct.Columns[1].ItemStyle.Width = 20;
                this.gvProduct.Columns[2].Visible = false;
                this.gvProduct.Columns[3].ItemStyle.Width = 20;
                this.gvProduct.Columns[4].Visible = false;
                this.gvProduct.Columns[5].Visible = false;
                this.gvProduct.Columns[6].Visible = false;
                this.gvProduct.Columns[7].Visible = false;
                this.gvProduct.Columns[8].ItemStyle.Width = 20;
            }
            if (Request.QueryString["mode"] == "price")
            {
                this.gvProduct.Columns[0].ItemStyle.Width = 300;
                this.gvProduct.Columns[1].ItemStyle.Width = 20;
                this.gvProduct.Columns[2].Visible = false;
                this.gvProduct.Columns[3].Visible = false;
                this.gvProduct.Columns[4].ItemStyle.Width = 15;
                this.gvProduct.Columns[5].ItemStyle.Width = 15;
                this.gvProduct.Columns[6].Visible = false;
                this.gvProduct.Columns[7].Visible = false;
                this.gvProduct.Columns[8].ItemStyle.Width = 20;
            }
            if (Request.QueryString["mode"] == "product")
            {
                this.gvProduct.Columns[0].ItemStyle.Width = 300;
                this.gvProduct.Columns[1].ItemStyle.Width = 15;
                this.gvProduct.Columns[2].Visible = false;
                this.gvProduct.Columns[3].Visible = false;
                this.gvProduct.Columns[4].Visible = false;
                this.gvProduct.Columns[5].Visible = false;
                this.gvProduct.Columns[6].ItemStyle.Width = 15;
                this.gvProduct.Columns[7].ItemStyle.Width = 15;
                this.gvProduct.Columns[8].ItemStyle.Width = 15;
            }

            #endregion
        }

        /// <summary>
        /// Update Button Click Event to Update Product by DisplayOrder,Inventory and Price
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnUpdate_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["ID"] != null)
            {
                int totalRecordCount = gvProduct.Rows.Count;
                for (int i = 0; i < totalRecordCount; i++)
                {
                    HiddenField hdnProductId = (HiddenField)gvProduct.Rows[i].FindControl("hdnProductId");

                    string productId = hdnProductId.Value;
                    string display = string.Empty;
                    string salePrice = "0";
                    if (Request.QueryString["mode"] == "DO")
                    {
                        TextBox txtDisplay = (TextBox)gvProduct.Rows[i].FindControl("txtDisplay");
                        if (hdnProductId != null && txtDisplay.Text != "" && txtDisplay.Text.Length > 0)
                        {
                            display = txtDisplay.Text.Trim();
                        }
                        else
                        {
                            display = "0";
                        }

                        //conside saleprice variable to store categoryid
                        if (Request.QueryString["ID"] != null)
                            salePrice = Request.QueryString["ID"];
                    }
                    if (Request.QueryString["mode"] == "inventory")
                    {
                        TextBox txtInventory = (TextBox)gvProduct.Rows[i].FindControl("txtInventory");
                        if (hdnProductId != null && txtInventory.Text != "" && txtInventory.Text.Length > 0)
                        {
                            display = txtInventory.Text.Trim();
                        }
                        else
                        {
                            display = "0";
                        }
                    }
                    if (Request.QueryString["mode"] == "price")
                    {

                        TextBox txtPrice = (TextBox)gvProduct.Rows[i].FindControl("txtPrice");
                        TextBox txtSaleprice = (TextBox)gvProduct.Rows[i].FindControl("txtSaleprice");
                        if (hdnProductId != null)
                        {
                            if (txtPrice.Text != "" && txtPrice.Text.Length > 0)
                                display = Math.Round(Convert.ToDecimal(txtPrice.Text.Trim()), 2).ToString();
                            else
                                display = "0";
                            if (txtSaleprice.Text != "" && txtSaleprice.Text.Length > 0)
                                salePrice = Math.Round(Convert.ToDecimal(txtSaleprice.Text.Trim()), 2).ToString();
                            else
                                salePrice = "0";
                        }
                    }


                    ProductComponent.UpdateProductByDisplayOrderPriceInventory(Request.QueryString["mode"], Convert.ToInt32(productId), display, salePrice);
                }
            }
            bindProductDetails();
        }

        /// <summary>
        /// Product Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvProduct_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProduct.PageIndex = e.NewPageIndex;
            bindProductDetails();
        }

        public string geturl(String sid, String productid)
        {
            if (productid != null && sid != null)
            {
                var storeid =Convert.ToInt32(sid);// document.getElementById('ContentPlaceHolder1_storeid').value;

                if (storeid == 1)
                {
                    return "/Admin/products/product.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                }

                else if (storeid == 3)
                {
                    return "/Admin/products/ProductAmazon.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                }
                else if (storeid == 4)
                {
                    return "/Admin/products/ProductOverStock.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                }
                else if (storeid == 7)
                {
                    return "/Admin/products/ProductEBay.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                }

                else if (storeid == 8)
                {
                    return "/Admin/products/ProductSears.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                }

                else
                {

                    return "/Admin/products/product.aspx?StoreID=" + storeid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
                }





            }


            return "/Admin/products/product.aspx?StoreID=" + sid + "&ID=" + productid + "&Mode=edit&LowInvt=2";
        }
    }
}
