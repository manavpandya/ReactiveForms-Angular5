using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductSearch : BasePage
    {

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnGo.ImageUrl = "/App_Themes/" + Page.Theme + "/images/search.gif";
                btnAddOptionProduct.ImageUrl = "/App_Themes/" + Page.Theme + "/images/add-option-product.png";
                btnAddOptionProduct.OnClientClick = "return ShowVariantdiv();";
                btnSelectGrid.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                if (Request.QueryString["StoreId"] != null)
                {
                    BindManufacture(Convert.ToInt32(Request.QueryString["StoreId"]));
                    BindCategory();
                    if (Request.QueryString["VariValId"] != null)
                    {
                        btnAddOptionProduct.Visible = true;
                        lblProductSku.Text = "RSC-" + Request.QueryString["VariValId"].ToString();
                    }
                    else
                    {
                        btnAddOptionProduct.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Binds the Manufacture for Store
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        protected void BindManufacture(Int32 StoreID)
        {
            DataSet dsManufacture = new DataSet();
            dsManufacture = ManufactureComponent.GetManufactureByStoreID(StoreID);
            if (dsManufacture != null && dsManufacture.Tables.Count > 0 && dsManufacture.Tables[0].Rows.Count > 0)
            {
                ddlManufacture.DataSource = dsManufacture;
                ddlManufacture.DataTextField = "Name";
                ddlManufacture.DataValueField = "ManufactureID";
                ddlManufacture.DataBind();
            }
            else
            {
                ddlManufacture.DataSource = null;
                ddlManufacture.DataBind();
            }
            ddlManufacture.DataBind();
            ddlManufacture.Items.Insert(0, new ListItem("Select Manufacturer", "0"));
        }

        /// <summary>
        /// Binds the Category for Store
        /// </summary>
        private void BindCategory()
        {
            ddlCategory.Items.Clear();
            DataSet dsCategory = new DataSet();
            dsCategory = CategoryComponent.GetCategoryByStoreID(Convert.ToInt32(Request.QueryString["StoreId"]));
            int count = 1;
            ListItem LT2 = new ListItem();
            DataRow[] drCatagories = null;
            if (Convert.ToInt32(Request.QueryString["StoreId"]) > 0)
            {
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=0 and StoreID=" + Convert.ToInt32(Request.QueryString["StoreId"]));
            }
            else
            {
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=0");
            }

            if (dsCategory != null && drCatagories.Length > 0)
            {
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = "...|" + count + "|" + selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), count, Convert.ToInt32(Request.QueryString["StoreId"]), dsCategory);
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("Root Category", "0"));
        }

        /// <summary>
        /// Sets the Child Category
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="Number">int Number</param>
        /// <param name="storeID">int StoreID</param>
        /// <param name="dsCategory">Dataset dsCategory</param>
        private void SetChildCategory(int ID, int Number, Int32 storeID, DataSet dsCategory)
        {
            int count = Number;
            string st = "...";
            for (int i = 0; i < count; i++)
            {
                st += st;
            }
            DataRow[] drCatagories = null;
            if (storeID == 0)
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + ID.ToString());
            else
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + ID.ToString() + " and StoreID=" + storeID);
            ListItem LT2;
            int innercount = 0;
            if (drCatagories.Length > 0)
            {
                innercount++;
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = st + "|" + (count + 1) + "|" + selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), innercount + Number, storeID, dsCategory);
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            DataSet dsSearch = new DataSet();
            ProductComponent procon = new ProductComponent();
            string WhrClus = "";
            //string WhrClus01 = "";
            if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
            {
                WhrClus += "Where " + ddlSearchby.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Trim() + "%'";

                dsSearch = ProductComponent.GetSearchProductVal(Convert.ToInt32(Request.QueryString["StoreId"]), WhrClus);

                if (dsSearch != null && dsSearch.Tables.Count > 0 && dsSearch.Tables[0].Rows.Count > 0)
                {
                    grdProduct.DataSource = dsSearch;
                    grdProduct.DataBind();
                    btnSave.Visible = true;
                }
                else
                {
                    grdProduct.DataSource = null;
                    grdProduct.DataBind();
                    btnSave.Visible = false;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Please enter search keyword');", true);
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (grdProduct.Rows.Count > 0)
            {
                for (int i = 0; i < grdProduct.Rows.Count; i++)
                {
                    HtmlInputRadioButton rdcheckid = (HtmlInputRadioButton)grdProduct.Rows[i].FindControl("rdcheckid");
                    Label lblProductID = (Label)grdProduct.Rows[i].FindControl("lblProductID");
                    Label lblSKU = (Label)grdProduct.Rows[i].FindControl("lblSKU");

                    Int32 VariantId = Convert.ToInt32(Request.QueryString["VId"]);
                    Int32 VariValId = Convert.ToInt32(Request.QueryString["VariValId"]);
                    Int32 ProductId = Convert.ToInt32(Request.QueryString["Id"]);
                    Int32 VariantProductID = Convert.ToInt32(lblProductID.Text.ToString());
                    if (rdcheckid.Checked)
                    {
                        Int32 Result = 0;// Convert.ToInt32(ProductComponent.SaveVariantProductID(Convert.ToInt32(Request.QueryString["StoreId"]), VariValId, VariantId, ProductId, VariantProductID));
                        if (Result > 0 && Request.QueryString["RId"] != null)
                        {
                            Int32 RowId = Convert.ToInt32(Request.QueryString["RId"]);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "set", "window.opener.document.getElementById('ContentPlaceHolder1_grdVariantValue_lblOptionProductSKU_" + RowId + "').innerHTML='" + lblSKU.Text + "'; window.close(); ", true);
                            return;
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "set", "window.opener.document.getElementById('ContentPlaceHolder1_lblProductSku').innerHTML='" + lblSKU.Text + "'; window.opener.document.getElementById('ContentPlaceHolder1_lblProductID').innerHTML='" + lblProductID.Text + "'; window.close(); ", true);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        ///  Select Grid Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSelectGrid_Click(object sender, ImageClickEventArgs e)
        {
            if (validation())
            {
                string strQry = "Insert into tb_product(StoreID,Name,Sku,Description ,Price ,SalePrice ,Weight ,Inventory ,CreatedOn ,Active ,Deleted) VALUES  (" + Request.QueryString["StoreID"].ToString() + ",'" + txtProductName.Text.Trim() + "','RSC-'+ CONVERT(NVARCHAR(100)," + Request.QueryString["VariValId"].ToString() + ") ,''," + Convert.ToDecimal(txtPrice.Text.Trim()) + "," + Convert.ToDecimal(txtPrice.Text.Trim()) + ",1,999,getdate(),1,1); Select scope_identity()";
                try
                {
                    Int32 NewProductId = Convert.ToInt32(CommonComponent.GetScalarCommonData(strQry));
                    #region Insert Variant Product Details

                    Int32 VariantId = Convert.ToInt32(Request.QueryString["VId"]);
                    Int32 VariValId = Convert.ToInt32(Request.QueryString["VariValId"]);
                    Int32 ProductId = Convert.ToInt32(Request.QueryString["Id"]);
                    Int32 VariantProductID = Convert.ToInt32(NewProductId.ToString());

                    if (!string.IsNullOrEmpty(lblProductSku.Text.ToString()))
                    {
                        Int32 Result = 0;// Convert.ToInt32(ProductComponent.SaveVariantProductID(Convert.ToInt32(Request.QueryString["StoreId"]), VariValId, VariantId, ProductId, VariantProductID));
                        if (Result > 0 && Request.QueryString["RId"] != null)
                        {
                            Int32 RowId = Convert.ToInt32(Request.QueryString["RId"]);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "set", "alert('Product Inserted Successfully...');window.opener.document.getElementById('ContentPlaceHolder1_grdVariantValue_lblOptionProductSKU_" + RowId + "').innerHTML='" + lblProductSku.Text + "'; window.close(); ", true);
                            return;
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "set", "window.opener.document.getElementById('ContentPlaceHolder1_lblProductSku').innerHTML='" + lblProductSku.Text + "'; window.opener.document.getElementById('ContentPlaceHolder1_lblProductID').innerHTML='" + NewProductId + "'; window.close(); ", true);
                            return;
                        }
                    }
                    #endregion
                }
                catch { }
            }
            else
            {
                if (!string.IsNullOrEmpty(lblProductSku.Text.ToString()) && Request.QueryString["RId"] != null)
                {
                    Int32 NewProductId = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 ProductId from tb_product where sku='RSC-" + Request.QueryString["VariValId"].ToString() + "' and deleted=1"));
                    if (NewProductId > 0)
                    {
                        Int32 VariantId = Convert.ToInt32(Request.QueryString["VId"]);
                        Int32 VariValId = Convert.ToInt32(Request.QueryString["VariValId"]);
                        Int32 ProductId = Convert.ToInt32(Request.QueryString["Id"]);
                        Int32 VariantProductID = Convert.ToInt32(NewProductId.ToString());
                        Int32 Result = 0;// Convert.ToInt32(ProductComponent.SaveVariantProductID(Convert.ToInt32(Request.QueryString["StoreId"]), VariValId, VariantId, ProductId, VariantProductID));
                        if (Result > 0 && Request.QueryString["RId"] != null)
                        {
                            Int32 RowId = Convert.ToInt32(Request.QueryString["RId"]);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "set", "alert('Product Already Exists...');window.opener.document.getElementById('ContentPlaceHolder1_grdVariantValue_lblOptionProductSKU_" + RowId + "').innerHTML='" + lblProductSku.Text + "'; window.close(); ", true);
                            return;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks All Validation Before Insert the Data into Database
        /// </summary>
        /// <returns>true if Validate proper, false otherwise</returns>
        private bool validation()
        {
            bool flag = true;
            DataSet dspro = new DataSet();
            dspro = CommonComponent.GetCommonDataSet("select name,price from tb_product where sku='RSC-" + Request.QueryString["VariValId"].ToString() + "' and deleted=1");

            if (dspro != null && dspro.Tables[0].Rows.Count > 0)
            {
                flag = false;
            }
            return flag;
        }
    }
}