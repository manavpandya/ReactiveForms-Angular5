using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using Solution.Data;
using System.Text;
using System.Net;
using System.Xml;
using eBay.Service.Call;
using eBay.Service.Core.Sdk;
using eBay.Service.Core.Soap;
using LumenWorks.Framework.IO.Csv;
using System.Net.Security;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductList : Solution.UI.Web.BasePage
    {


        #region Declaration
        DataSet dsCategory = new DataSet();
        DataSet dsProduct = new DataSet();
        public static bool isDescendProductName = false;
        public static bool isDescendSKU = false;
        public static bool isDescendInventory = false;
        public static bool isDescendPrice = false;
        public static bool isDescendSalePrice = false;
        static string AccessToken = "";

        int storeID = 1;
        String Live_Server = "";
        String Image_Path = "";
        private string StrFileName
        {
            get
            {
                if (ViewState["FileName"] == null)
                {
                    return "";
                }
                else
                {
                    return (ViewState["FileName"].ToString());
                }
            }
            set
            {
                ViewState["FileName"] = value;
            }
        }
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                lblMsg.Text = "";
                if (Request.QueryString["Insert"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product inserted successfully.', 'Message');});", true);
                }
                else if (Request.QueryString["Update"] != null)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product updated successfully.', 'Message');});", true);
                }
                else if (Request.QueryString["cancel"] != null)
                {

                }
                bindstore();
                AppConfig.StoreID = 1;

                if (ddlStore.SelectedIndex == 0)
                {
                    AppConfig.StoreID = 1;
                    storeID = 1;

                }
                else
                {
                    AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                    storeID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                }
                clsvariables.LoadAllPath(); // By Girish

                FillCategoryDropDown(Convert.ToInt32(Request.QueryString["StoreID"]));
                FillProductTypeDropDown();
                FillProductTypeDeliveryDropDown();

                if (Request.QueryString["Insert"] != null || Request.QueryString["Update"] != null || Request.QueryString["cancel"] != null)
                {
                    if (Session["SIDSearch"] != null && Session["StockSearch"] != null && Session["ProdctTypeSearch"] != null && Session["CategorySearch"] != null && Session["StatusSearch"] != null && Session["SearchBy"] != null && Session["SearchText"] != null && Session["SearchGridpage"] != null)
                    {
                        try
                        {
                            ddlStore.SelectedValue = Session["SIDSearch"].ToString();
                            ddlProductTypeDelivery.SelectedValue = Session["StockSearch"].ToString();
                            ddlProductType.SelectedValue = Session["ProdctTypeSearch"].ToString();
                            ddlCategory.SelectedValue = Session["CategorySearch"].ToString();
                            ddlStatus.SelectedValue = Session["StatusSearch"].ToString();
                            ddlSearch.SelectedValue = Session["SearchBy"].ToString();
                            txtSearch.Text = Session["SearchText"].ToString();
                            grdProduct.PageIndex = Convert.ToInt32(Session["SearchGridpage"].ToString());
                            grdProduct.DataBind();
                        }
                        catch
                        {
                            Session["SIDSearch"] = null;
                            Session["StockSearch"] = null;
                            Session["ProdctTypeSearch"] = null;
                            Session["CategorySearch"] = null;
                            Session["StatusSearch"] = null;
                            Session["SearchBy"] = null;
                            Session["SearchText"] = null;
                            Session["SearchGridpage"] = "0";

                            grdProduct.PageIndex = 0;
                            grdProduct.DataBind();
                        }
                    }
                    else
                    {
                        Session["SIDSearch"] = null;
                        Session["StockSearch"] = null;
                        Session["ProdctTypeSearch"] = null;
                        Session["CategorySearch"] = null;
                        Session["StatusSearch"] = null;
                        Session["SearchBy"] = null;
                        Session["SearchText"] = null;
                        Session["SearchGridpage"] = "0";

                        grdProduct.PageIndex = 0;
                        grdProduct.DataBind();
                    }

                }
                else
                {
                    Session["SIDSearch"] = null;
                    Session["StockSearch"] = null;
                    Session["ProdctTypeSearch"] = null;
                    Session["CategorySearch"] = null;
                    Session["StatusSearch"] = null;
                    Session["SearchBy"] = null;
                    Session["SearchText"] = null;
                    Session["SearchGridpage"] = "0";

                    grdProduct.PageIndex = 0;
                    grdProduct.DataBind();
                }




            }





            if (ddlStore.SelectedIndex == -1 || ddlStore.SelectedIndex == 1)
                ahAddProduct.HRef = "Product.aspx?StoreID=1";
            else if (ddlStore.SelectedIndex == 2)
                ahAddProduct.HRef = "ProductYahoo.aspx?StoreID=2";
            else if (ddlStore.SelectedValue == "3")
                ahAddProduct.HRef = "ProductAmazon.aspx?StoreID=3";
            else if (ddlStore.SelectedValue == "4")
                ahAddProduct.HRef = "ProductOverStock.aspx?StoreID=4";
            else if (ddlStore.SelectedValue == "5")
                ahAddProduct.HRef = "ProductYahoo.aspx?StoreID=5";
            else if (ddlStore.SelectedValue == "6")
                ahAddProduct.HRef = "ProductYahoo.aspx?StoreID=6";
            else if (ddlStore.SelectedValue == "7")
                ahAddProduct.HRef = "ProductEBay.aspx?StoreID=7";
            else if (ddlStore.SelectedValue == "8")
                ahAddProduct.HRef = "ProductSears.aspx?StoreID=8";
            else if (ddlStore.SelectedValue == "9")
                ahAddProduct.HRef = "ProductBuy.aspx?StoreID=9";

            if (Convert.ToInt32(ddlStore.SelectedValue.ToString()) == 1)
            {
                btnExportPrice.Visible = true;
                btnecommexportprice.Visible = true;

                uploadCSV.Visible = true;
                btnImport.Visible = true;
                lblMessage.Visible = true;
                lblImportCSv.Visible = true;
            }
            else
            {
                btnExportPrice.Visible = false;
                btnecommexportprice.Visible = false;

                uploadCSV.Visible = false;
                btnImport.Visible = false;
                lblMessage.Visible = false;
                lblImportCSv.Visible = false;
            }

            btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export-all-product-inventory.png) no-repeat transparent; width: 197px; height: 23px; border:none;cursor:pointer;");
            btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; padding-right:0px; height: 23px; border:none;cursor:pointer;");
            btnUpdateMultiple.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/update.png) no-repeat transparent; width: 70px;padding-right:0px; height: 23px; border:none;cursor:pointer;");
            btnEditMultiplePrice.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/edit-multiple-price.png) no-repeat transparent; width:140px; height: 23px; border:none;cursor:pointer;");
            btnEditMultipleProduct.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/edit-multiple-product.png) no-repeat transparent; width: 160px; height: 23px; border:none;cursor:pointer;");
            btnMultipleDelete.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
            btnSaveMultiple.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/save.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
            btnCancelMultiple.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
            btnApprove.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/approveitems.png";
            btnExportPrice.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnCheckAll.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/show-all-inventory-products.gif) no-repeat; width: 190px; height: 22px; border:0;cursor: pointer; font-size: 9px;");

            btnecommexportprice.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/exportprice.gif) no-repeat transparent;height: 23px; border:none;cursor:pointer;");
            #region Commented Code for amazon Upload Buttons - Do not Delete

            btnAmazonProduct.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/uploadinamazon.png) no-repeat;width: 125px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");

            btnAmazonImage.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/uploadamazonimage.png) no-repeat;width: 99px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");


            btnAmozonUpdate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/updateinventoryinamazon.png) no-repeat; width: 180px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");
            btnAmazonPrice.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/updatepriceinamazon.png) no-repeat; width: 150px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");

            btneBayProduct.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/upload-in-ebay.png) no-repeat;width: 106px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");
            btneBayUpdate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/updateinventoryinebay.png) no-repeat; width: 167px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");

            btnSearsProduct.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/uploadinsears.png) no-repeat;width: 112px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");
            btnSearsUpdate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/updateinventoryinsears.png) no-repeat; width: 174px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");
            btnSearsPrice.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/updatepriceinsears.png) no-repeat; width: 145px; height: 22px; border: 0; cursor: pointer; font-size: 9px;");

            btnOverStockUpdate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/uploadinoverstock.png) no-repeat;width: 202px; height: 23px; border: 0; cursor: pointer; font-size: 9px;");
            btnOverStockAllUpdate.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/uploadallinoverstock.png) no-repeat;width: 225px; height: 23px; border: 0; cursor: pointer; font-size: 9px;");
            btnOverStockProduct.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/uploadproductinoverstock.png) no-repeat;width: 144px; height: 23px; border: 0; cursor: pointer; font-size: 9px;");
            btnPrintBarcode.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-barcode.gif";
            #endregion
        }

        private void SetSession()
        {
            Session["SIDSearch"] = ddlStore.SelectedValue.ToString();
            Session["StockSearch"] = ddlProductTypeDelivery.SelectedValue.ToString();
            Session["ProdctTypeSearch"] = ddlProductType.SelectedValue.ToString();
            Session["CategorySearch"] = ddlCategory.SelectedValue.ToString();
            Session["StatusSearch"] = ddlStatus.SelectedValue.ToString();
            Session["SearchBy"] = ddlSearch.SelectedValue.ToString();
            Session["SearchText"] = txtSearch.Text.ToString();
            Session["SearchGridpage"] = grdProduct.PageIndex.ToString();

        }
        /// <summary>
        /// Fill Category Drop down
        /// </summary>
        private void FillCategoryDropDown(int StoreID)
        {
            ddlCategory.Items.Clear();
            if (ddlStore.SelectedIndex == 0)
            {
                return;
            }
            else
            {
                dsCategory = CategoryComponent.GetCategoryByStoreID(StoreID, 3); //Option 3: To display category order by Display order
            }

            int count = 1;
            ListItem LT2 = new ListItem();
            DataRow[] drCatagories = null;
            if (ddlStore.SelectedIndex > 0)
            {
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=0 and StoreID=" + Convert.ToInt32(ddlStore.SelectedValue.ToString()), "DisplayOrder");
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
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), count);
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("Root Category", "0"));
        }

        /// <summary>
        /// Set Child category
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="Number">int Number</param>
        private void SetChildCategory(int ID, int Number)
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
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), innercount + Number);
                }
            }
        }

        /// <summary>
        /// Bind Product Type Drop down
        /// </summary>
        private void FillProductTypeDropDown()
        {
            DataSet dsProductType = new DataSet();
            if (ddlStore.SelectedIndex != 0)
                dsProductType = ProductTypeComponent.GetProductTypeByStoreID(Convert.ToInt32(ddlStore.SelectedValue));
            else
                dsProductType = ProductTypeComponent.GetProductTypeByStoreID(1);
            if (dsProductType != null && dsProductType.Tables.Count > 0 && dsProductType.Tables[0].Rows.Count > 0)
            {
                ddlProductType.DataSource = dsProductType;
                ddlProductType.DataTextField = "Name";
                ddlProductType.DataValueField = "ProductTypeID";
            }
            else
            {
                ddlProductType.DataSource = null;
            }
            ddlProductType.DataBind();
            ddlProductType.Items.Insert(0, new ListItem("All Products", "0"));
            ddlProductType.SelectedIndex = 0;
        }

        /// <summary>
        /// Bind Product Type Delivery Drop down
        /// </summary>
        private void FillProductTypeDeliveryDropDown()
        {
            DataSet dsProductTypeDelivery = new DataSet();
            if (ddlStore.SelectedIndex != 0)
                dsProductTypeDelivery = ProductTypeComponent.GetProductTypeDeliveryByStoreID(Convert.ToInt32(ddlStore.SelectedValue));
            else
                dsProductTypeDelivery = ProductTypeComponent.GetProductTypeDeliveryByStoreID(1);
            // dsProductTypeDelivery = ProductTypeComponent.GetProductTypeDeliveryByStoreID(StoreID);
            if (dsProductTypeDelivery != null && dsProductTypeDelivery.Tables.Count > 0 && dsProductTypeDelivery.Tables[0].Rows.Count > 0)
            {
                ddlProductTypeDelivery.DataSource = dsProductTypeDelivery;
                ddlProductTypeDelivery.DataTextField = "Name";
                ddlProductTypeDelivery.DataValueField = "ProductTypeDeliveryID";
            }
            else
            {
                ddlProductTypeDelivery.DataSource = null;
            }
            ddlProductTypeDelivery.DataBind();
            // ddlProductTypeDelivery.Items.Insert(0, new ListItem("Select Product Type Delivery", "0"));         
            ddlProductTypeDelivery.Items.Insert(0, new ListItem("All Products", "0"));
            ddlProductTypeDelivery.SelectedIndex = 0;
        }

        /// <summary>
        /// Sort Column in ASC or DESC Order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton btn = (ImageButton)sender;
            if (btn != null)
            {
                if (btn.CommandArgument == "ASC")
                {
                    grdProduct.Sort(btn.CommandName.ToString(), SortDirection.Ascending);
                    btn.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (btn.ID == "btnProductName")
                    {
                        isDescendProductName = false;
                    }
                    else if (btn.ID == "btnSKU")
                    {
                        isDescendSKU = false;
                    }
                    else if (btn.ID == "btnInventory")
                    {
                        isDescendInventory = false;
                    }
                    else if (btn.ID == "btnPrice")
                    {
                        isDescendPrice = false;
                    }
                    else if (btn.ID == "btnSalePrice")
                    {
                        isDescendSalePrice = false;
                    }
                    btn.AlternateText = "Descending Order";
                    btn.ToolTip = "Descending Order";
                    btn.CommandArgument = "DESC";
                }
                else if (btn.CommandArgument == "DESC")
                {
                    grdProduct.Sort(btn.CommandName.ToString(), SortDirection.Descending);
                    btn.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (btn.ID == "btnProductName")
                    {
                        isDescendProductName = true;
                    }
                    else if (btn.ID == "btnSKU")
                    {
                        isDescendSKU = true;
                    }
                    else if (btn.ID == "btnInventory")
                    {
                        isDescendInventory = true;
                    }
                    else if (btn.ID == "btnPrice")
                    {
                        isDescendPrice = true;
                    }
                    else if (btn.ID == "btnSalePrice")
                    {
                        isDescendSalePrice = true;
                    }
                    btn.AlternateText = "Ascending Order";
                    btn.ToolTip = "Ascending Order";
                    btn.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event for Get Product List
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdProduct.PageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GridPageSize"].ToString());
            lblMsg.Text = "";
            if (ddlStore.SelectedIndex > 0)
            {

                btnCheckAll.Visible = true;
                ddlCategory.Enabled = true;
            }
            else
            {
                btnCheckAll.Visible = false;
                ddlCategory.Enabled = false;
            }


            #region Commented Code for amazon Upload Buttons - Do not Delete
            if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("wayfailr") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("lnt") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("bellacor") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("kohl") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("houzz") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("atg") > -1 || (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1))
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }

            if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("amazon") > -1)
            {
                btnAmazonPrice.Visible = true;
                btnAmazonProduct.Visible = true;
                btnAmozonUpdate.Visible = true;
                btnAmazonImage.Visible = true;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;

                btnExport.Visible = false;
            }
            else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("ebay") > -1)
            {

                btneBayProduct.Visible = true;
                btneBayUpdate.Visible = true;

                btnAmazonImage.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
            }
            else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("sears") > -1)
            {

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnAmazonImage.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;

                btnSearsProduct.Visible = true;
                btnSearsUpdate.Visible = true;
                btnSearsPrice.Visible = true;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
            }
            else if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1)
            {
                btnOverStockUpdate.Visible = true;
                btnOverStockAllUpdate.Visible = true;
                btnOverStockProduct.Visible = true;

                btnAmazonImage.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;


                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;
            }
            else
            {
                btnAmazonImage.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;


                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
            }
            #endregion

            grdProduct.SelectedIndex = 0;
            grdProduct.DataBind();
            FillCategoryDropDown(Convert.ToInt32(ddlStore.SelectedItem.Value));
            FillProductTypeDeliveryDropDown();
            FillProductTypeDropDown();
            if (grdProduct.Rows.Count == 0)
            {
                trBottom.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;
                btnAmazonImage.Visible = false;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
                //btnExport.Visible = false;
            }

            if (ddlStore.SelectedItem.Value == "1" || ddlStore.SelectedItem.Value == "-1")
            {
                ahAddProduct.HRef = "Product.aspx?StoreID=1";
                HideButtons();
            }
            else if (ddlStore.SelectedItem.Value == "2")
            {
                ahAddProduct.HRef = "ProductYahoo.aspx?StoreID=2";
            }

            else if (ddlStore.SelectedItem.Value == "3")
            {
                ahAddProduct.HRef = "ProductAmazon.aspx?StoreID=3";
            }
            else if (ddlStore.SelectedItem.Value == "4")
            {
                ahAddProduct.HRef = "Product.aspx?StoreID=4";

            }
            else if (ddlStore.SelectedItem.Value == "5")
            {
                ahAddProduct.HRef = "Product.aspx?StoreID=5";

            }
            else if (ddlStore.SelectedItem.Value == "6")
            {
                ahAddProduct.HRef = "Product.aspx?StoreID=6";
            }
            else if (ddlStore.SelectedItem.Value == "7")
            {
                ahAddProduct.HRef = "ProductEBay.aspx?StoreID=7";
            }
            else if (ddlStore.SelectedItem.Value == "8")
            {
                ahAddProduct.HRef = "ProductSears.aspx?StoreID=8";
            }
            else if (ddlStore.SelectedItem.Value == "9")
            {
                ahAddProduct.HRef = "ProductBuy.aspx?StoreID=9";
            }
            else
            {
                ahAddProduct.HRef = "Product.aspx?StoreID=" + ddlStore.SelectedItem.Value + "";

            }

            if (ddlStore.SelectedValue != "")
                storeID = Convert.ToInt32(ddlStore.SelectedValue);
            AppConfig.StoreID = storeID;
            clsvariables.LoadAllPath(); // By Girish
            SetSession();
        }

        /// <summary>
        /// Product Type Drop Down Selected Index Changed Event for Get Product List
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdProduct.PageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GridPageSize"].ToString());
            //Filter data based on selected store
            grdProduct.SelectedIndex = 0;
            grdProduct.DataBind();
            if (grdProduct.Rows.Count == 0)
            {
                btnAmazonImage.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;


                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
                //btnExport.Visible = false;
                trBottom.Visible = false;
            }
            SetSession();
        }

        /// <summary>
        /// Product Type Delivery Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlProductTypeDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlProductTypeDelivery.SelectedIndex == 0)
            {
                FillProductTypeDropDown();
            }
            if (ddlProductTypeDelivery.SelectedIndex != 0)
            {

                ddlProductType.Items.Clear();
                DataSet dssupportedProduct = new DataSet();
                VendorComponent objVendor = new VendorComponent();
                dssupportedProduct = objVendor.GetProductTypeDeliveryByID(Convert.ToInt32(ddlProductTypeDelivery.SelectedValue.ToString()));

                if (dssupportedProduct != null && dssupportedProduct.Tables.Count > 0 && dssupportedProduct.Tables[0].Rows.Count > 0)
                {
                    string databaseValue = dssupportedProduct.Tables[0].Rows[0]["SupportedProductType"].ToString();
                    string[] arr = databaseValue.Split(',');
                    ProductTypeComponent objProductType = new ProductTypeComponent();
                    foreach (string value in arr)
                    {
                        DataSet dsType = objProductType.GetProductTypeByName(Convert.ToString(value), Convert.ToInt32(Request.QueryString["StoreID"]));
                        if (dsType != null && dsType.Tables.Count > 0 && dsType.Tables[0].Rows.Count > 0)
                        {
                            this.ddlProductType.Items.Add(new ListItem(Convert.ToString(dsType.Tables[0].Rows[0]["Name"]), Convert.ToString(dsType.Tables[0].Rows[0]["ProductTypeID"])));
                        }
                    }
                    ddlProductType.Items.Insert(0, new ListItem("All Products", "0"));
                }
                else
                {
                    ddlProductType.DataSource = null;
                    ddlProductType.DataBind();
                    ddlProductType.SelectedIndex = 0;
                }
            }

            grdProduct.PageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GridPageSize"].ToString());
            grdProduct.SelectedIndex = 0;
            grdProduct.DataBind();
            if (grdProduct.Rows.Count == 0)
            {
                trBottom.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;
                btnAmazonImage.Visible = false;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
                //btnExport.Visible = false;
            }
            SetSession();
        }

        /// <summary>
        /// Category Drop Down Selected Index Changed Event for Get Product List
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Filter data based on selected store
            grdProduct.PageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GridPageSize"].ToString());
            storeID = Convert.ToInt32(ddlCategory.SelectedItem.Value);
            grdProduct.SelectedIndex = 0;
            grdProduct.DataBind();
            if (grdProduct.Rows.Count == 0)
            {
                trBottom.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;
                btnAmazonImage.Visible = false;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
                //btnExport.Visible = false;
            }
            SetSession();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grdProduct.PageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GridPageSize"].ToString());
            grdProduct.PageIndex = 0;
            grdProduct.DataBind();
            if (grdProduct.Rows.Count == 0)
            {
                trBottom.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;
                btnAmazonImage.Visible = false;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
                //btnExport.Visible = false;
            }
            btnApprove.Visible = false;
            if (grdProduct.Rows.Count > 0 && ddlStatus.SelectedItem.Text.ToString().ToLower() == "dataverify")
            {
                btnApprove.Visible = true;
            }
            SetSession();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {

            trBottom.Visible = true;
            ddlStore.SelectedIndex = 0;
            ddlProductType.SelectedIndex = 0;
            ddlCategory.Items.Clear();
            ddlCategory.Enabled = false;
            ddlStatus.SelectedIndex = 0;
            txtSearch.Text = "";
            txtValue.Text = "";
            grdProduct.PageSize = Convert.ToInt32(System.Configuration.ConfigurationSettings.AppSettings["GridPageSize"].ToString());
            grdProduct.SelectedIndex = 0;
            grdProduct.DataBind();
            btnEditMultiplePrice.Visible = true;
            btnSaveMultiple.Visible = false;
            btnCancelMultiple.Visible = false;
            ddlFieldName.SelectedIndex = 0;
            ddlOperation.SelectedIndex = 0;
            ddlValue.SelectedIndex = 0;
            FillProductTypeDropDown();
            SetSession();
        }

        /// <summary>
        /// Product Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            btnApprove.Visible = false;
            if (grdProduct.Rows.Count > 0)
            {
                trBottom.Visible = true;
                if (ddlStatus.SelectedItem.Text.ToString().ToLower() == "dataverify")
                {
                    btnApprove.Visible = true;
                }
            }
            else
            {
                trBottom.Visible = false;
                ViewState["ExportProductIds"] = null;
            }
            #region set property for sorting
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendProductName == false)
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnProductName");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnProductName.AlternateText = "Ascending Order";
                    btnProductName.ToolTip = "Ascending Order";
                    btnProductName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnProductName");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnProductName.AlternateText = "Descending Order";
                    btnProductName.ToolTip = "Descending Order";
                    btnProductName.CommandArgument = "ASC";
                }
                if (isDescendSKU == false)
                {
                    ImageButton btnSKU = (ImageButton)e.Row.FindControl("btnSKU");
                    btnSKU.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnSKU.AlternateText = "Ascending Order";
                    btnSKU.ToolTip = "Ascending Order";
                    btnSKU.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnSKU = (ImageButton)e.Row.FindControl("btnSKU");
                    btnSKU.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnSKU.AlternateText = "Descending Order";
                    btnSKU.ToolTip = "Descending Order";
                    btnSKU.CommandArgument = "ASC";
                }
                if (isDescendInventory == false)
                {
                    ImageButton btnInventory = (ImageButton)e.Row.FindControl("btnInventory");
                    btnInventory.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnInventory.AlternateText = "Ascending Order";
                    btnInventory.ToolTip = "Ascending Order";
                    btnInventory.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnInventory = (ImageButton)e.Row.FindControl("btnInventory");
                    btnInventory.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnInventory.AlternateText = "Descending Order";
                    btnInventory.ToolTip = "Descending Order";
                    btnInventory.CommandArgument = "ASC";
                }
                if (isDescendPrice == false)
                {
                    ImageButton btnPrice = (ImageButton)e.Row.FindControl("btnPrice");
                    btnPrice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnPrice.AlternateText = "Ascending Order";
                    btnPrice.ToolTip = "Ascending Order";
                    btnPrice.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnPrice = (ImageButton)e.Row.FindControl("btnPrice");
                    btnPrice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnPrice.AlternateText = "Descending Order";
                    btnPrice.ToolTip = "Descending Order";
                    btnPrice.CommandArgument = "ASC";
                }
                if (isDescendSalePrice == false)
                {
                    ImageButton btnSalePrice = (ImageButton)e.Row.FindControl("btnSalePrice");
                    btnSalePrice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnSalePrice.AlternateText = "Ascending Order";
                    btnSalePrice.ToolTip = "Ascending Order";
                    btnSalePrice.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnSalePrice = (ImageButton)e.Row.FindControl("btnSalePrice");
                    btnSalePrice.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnSalePrice.AlternateText = "Descending Order";
                    btnSalePrice.ToolTip = "Descending Order";
                    btnSalePrice.CommandArgument = "ASC";
                }
            }
            #endregion

            #region Set navigation URl on ProductName and EditButton
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.HyperLink HlEdit = (HyperLink)e.Row.FindControl("hlEdit");
                Literal lblName = (Literal)e.Row.FindControl("lblProductName");
                Label lblStoreID = (Label)e.Row.FindControl("lblStoreID");
                Literal ltclone = (Literal)e.Row.FindControl("ltclone");
                HiddenField hdnProductID = (HiddenField)e.Row.FindControl("hdnProductid");
                HiddenField hdnUPC = (HiddenField)e.Row.FindControl("hdnUPC");
                Label lblhemming = (Label)e.Row.FindControl("lblhemming");
                Label lblSKU = (Label)e.Row.FindControl("lblSKU");
                Literal ltotherstore = (Literal)e.Row.FindControl("ltotherstore");
                DataSet dsstore = new DataSet();
                if (ddlStatus.SelectedValue.ToString().ToLower() == "active")
                {
                    dsstore = CommonComponent.GetCommonDataSet("select StoreID from tb_product where sku = '" + lblSKU.Text + "' and ISnull(Active,0) =1 and Isnull(Deleted,0)=0 and Storeid not in (" + lblStoreID.Text + ")");
                }
                else if (ddlStatus.SelectedValue.ToString().ToLower() == "inactive")
                {
                    dsstore = CommonComponent.GetCommonDataSet("select StoreID from tb_product where sku = '" + lblSKU.Text + "' and ISnull(Active,0) =0 and Isnull(Deleted,0)=0 and Storeid not in (" + lblStoreID.Text + ")");
                }
                else if (ddlStatus.SelectedValue.ToString().ToLower() == "adminonly")
                {
                    dsstore = CommonComponent.GetCommonDataSet("select StoreID from tb_product where sku = '" + lblSKU.Text + "' and ISnull(Active,0) =0 and isnull(AdminActive,0)=1 and Isnull(Deleted,0)=0 and Storeid not in (" + lblStoreID.Text + ")");
                }
                else
                {
                    dsstore = CommonComponent.GetCommonDataSet("select StoreID from tb_product where sku = '" + lblSKU.Text + "' and  Isnull(Deleted,0)=0 and Storeid not in (" + lblStoreID.Text + ")");
                }

                string astoreidhref = string.Empty;
                if (dsstore != null && dsstore.Tables.Count > 0 && dsstore.Tables[0].Rows.Count > 0)
                {
                    for (int j = 0; j < dsstore.Tables[0].Rows.Count; j++)
                    {
                        switch (Convert.ToInt32(dsstore.Tables[0].Rows[j]["StoreID"].ToString()))
                        {
                            case 1:
                                astoreidhref = "/admin/products/Product.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                break;
                            case 2:
                                astoreidhref = "/admin/products/ProductYahoo.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "'  style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                break;

                            case 3:
                                astoreidhref = "/admin/products/ProductAmazon.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "'  style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";


                                break;

                            case 4:
                                astoreidhref = "/admin/products/ProductOverStock.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";


                                break;

                            case 5:
                                //astoreidhref = "/admin/products/ProductNewEgg.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                //ltotherstore.Text += "<a href='" + astoreidhref + "'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                astoreidhref = "javascript:void(0);";

                                break;

                            case 6:
                                astoreidhref = "javascript:void(0);";
                                //astoreidhref = "/admin/products/ProductSears.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                //ltotherstore.Text += "<a href='" + astoreidhref + "'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";


                                break;
                            case 7:
                                astoreidhref = "/admin/products/ProductEBay.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";


                                break;
                            case 8:
                                astoreidhref = "/admin/products/ProductSears.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";



                                break;

                            case 9:
                                astoreidhref = "/admin/products/Product.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                break;

                            case 10:
                                astoreidhref = "/admin/products/Product.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                break;
                            case 11:
                                astoreidhref = "/admin/products/Product.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                break;
                            case 12:
                                astoreidhref = "/admin/products/Product.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                break;
                            case 13:
                                astoreidhref = "/admin/products/Product.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                break;
                            case 14:
                                astoreidhref = "/admin/products/Product.aspx?StoreID=" + dsstore.Tables[0].Rows[j]["StoreID"].ToString().Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                                ltotherstore.Text += "<a href='" + astoreidhref + "' style='text-decoration:none;'><img border=\"0\" src=\"/Admin/images/" + dsstore.Tables[0].Rows[j]["StoreID"].ToString() + ".png\"> </a>";
                                break;




                        }
                    }
                }

                if (lblStoreID.Text.ToString() == "1")
                {
                    try
                    {
                        Int32 inv = 0;
                        DataSet ds = new DataSet();
                        ds = CommonComponent.GetCommonDataSet("SELECT * FROM tb_productvariantvalue WHERE Productid=" + hdnProductID.Value.ToString() + " and isnull(upc,'')<>''");
                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                        {
                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                Int32 ii = 0;
                                string strff = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + ds.Tables[0].Rows[i]["upc"].ToString() + "','" + ds.Tables[0].Rows[i]["SKU"].ToString() + "'," + lblStoreID.Text.ToString() + ")"));
                                Int32.TryParse(strff, out ii);
                                inv = inv + ii;

                            }
                            if (inv >= 0)
                            {
                                lblhemming.Text = inv.ToString();
                            }
                            else
                            {
                                lblhemming.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + hdnUPC.Value.ToString() + "','" + lblSKU.Text.ToString() + "'," + lblStoreID.Text.ToString() + ")"));
                            }
                        }
                        else
                        {
                            lblhemming.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + hdnUPC.Value.ToString() + "','" + lblSKU.Text.ToString() + "'," + lblStoreID.Text.ToString() + ")"));
                        }
                    }
                    catch
                    {
                        lblhemming.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + hdnUPC.Value.ToString() + "','" + lblSKU.Text.ToString() + "'," + lblStoreID.Text.ToString() + ")"));
                    }


                }
                else
                {
                    lblhemming.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + hdnUPC.Value.ToString() + "','" + lblSKU.Text.ToString() + "'," + lblStoreID.Text.ToString() + ")"));
                }



                if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("wayfailr") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("lnt") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("bellacor") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("kohl") > -1 || (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1))
                {
                    if (ViewState["ExportProductIds"] != null)
                    {
                        ViewState["ExportProductIds"] = ViewState["ExportProductIds"] + hdnProductID.Value.ToString() + ",";
                    }
                    else
                    {
                        ViewState["ExportProductIds"] = hdnProductID.Value.ToString() + ",";
                    }
                }
                else
                {
                    ViewState["ExportProductIds"] = null;
                }

                if (lblStoreID.Text.ToString() == "1")
                {
                    HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                }
                else if (lblStoreID.Text.ToString() == "2")
                {
                    HlEdit.NavigateUrl = "/admin/products/ProductYahoo.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                }
                else if (lblStoreID.Text.ToString() == "3")
                {
                    HlEdit.NavigateUrl = "/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                }
                else if (lblStoreID.Text.ToString() == "4")
                {
                    HlEdit.NavigateUrl = "/admin/products/ProductOverStock.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "5")
                {
                    // HlEdit.NavigateUrl = "/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "6")
                {
                    //HlEdit.NavigateUrl = "/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "7")
                {
                    HlEdit.NavigateUrl = "/admin/products/ProductEBay.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "8")
                {
                    HlEdit.NavigateUrl = "/admin/products/ProductSears.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "9")
                {
                    HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "10")
                {
                    HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "11")
                {
                    HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "12")
                {
                    HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "13")
                {
                    HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                else if (lblStoreID.Text.ToString() == "14")
                {
                    HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                    //HlEdit.NavigateUrl = "javascript:void(0);";
                }
                // ImageButton ImgToggelInventory = (ImageButton)e.Row.FindControl("ImgToggelInventory");
                System.Web.UI.HtmlControls.HtmlContainerControl ImgToggelInventory = (System.Web.UI.HtmlControls.HtmlContainerControl)e.Row.FindControl("ImgToggelInventory");

                int ChkLowInventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(*) from tb_Product where ProductID = " + hdnProductID.Value + " and (ISNULL( tb_Product.Inventory,0) <  ISNULL( tb_Product.LowInventory,0))"));
                if (ChkLowInventory > 0)
                {

                    ImgToggelInventory.Visible = true;
                }

                HlEdit.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                //lblName.Text = "<a href='/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                //Set image dynamically
                ImageButton btnEditPrice = (ImageButton)e.Row.FindControl("btnEditPrice");
                ImageButton btnSave = (ImageButton)e.Row.FindControl("btnSave");
                ImageButton btnCancel = (ImageButton)e.Row.FindControl("btnCancel");
                btnEditPrice.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/edit-price.gif";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/save.png";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/CloseIcon.png";
                try
                {
                    if (ltclone != null)
                    {
                        ltclone.Text = "<a id=\"button\" name='button' href='javascript:void(0);' style='cursor:pointer;' onclick=\"OpenClonePopup(" + lblStoreID.Text.Trim() + "," + hdnProductID.Value + ");\">Clone</a>";
                    }
                }
                catch { }

                if ((lblStoreID != null) && (ltclone != null) && (HlEdit != null))
                {
                    switch (Convert.ToInt32(lblStoreID.Text.ToString()))
                    {

                        case 1:
                            HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;

                        case 2:
                            HlEdit.NavigateUrl = "/admin/products/ProductYahoo.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/ProductYahoo.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        case 3:
                            HlEdit.NavigateUrl = "/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        case 4:
                            HlEdit.NavigateUrl = "/admin/products/ProductOverStock.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/ProductOverStock.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        //case 5:
                        //    HlEdit.NavigateUrl = "/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                        //    lblName.Text = "<a href='/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                        //    break;
                        //case 6:
                        //    HlEdit.NavigateUrl = "/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                        //    lblName.Text = "<a href='/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                        //    break;
                        case 7:
                            HlEdit.NavigateUrl = "/admin/products/ProductEBay.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/ProductEBay.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        case 8:
                            HlEdit.NavigateUrl = "/admin/products/ProductSears.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/ProductSears.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        case 9:
                            HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        case 10:
                            HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        case 11:
                            HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        case 12:
                            HlEdit.NavigateUrl = "/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                            lblName.Text = "<a href='/admin/products/Product.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                            break;
                        //case 13:
                        //    HlEdit.NavigateUrl = "/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit";
                        //    lblName.Text = "<a href='/admin/products/ProductAmazon.aspx?StoreID=" + lblStoreID.Text.Trim() + "&ID=" + hdnProductID.Value + "&Mode=edit'>" + lblName.Text + "</a>";
                        //    break;
                    }
                }

                Literal ltStatusColor = (Literal)e.Row.FindControl("ltStatusColor");
                HiddenField hdnProductTypeID = (HiddenField)e.Row.FindControl("hdnProductTypeID");
                HiddenField hdnProductTypeDeliveryID = (HiddenField)e.Row.FindControl("hdnProductTypeDeliveryID");

                ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:green'></div>&nbsp;";

                Int32 ProductDataCount = 0;
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("select count(*) from tb_Product where len(description)>0 and len(SETitle)>0 and len(SEDescription)>0 and len(SEKeywords)>0 and len(ToolTip)>0 and ProductID=" + hdnProductID.Value)), out ProductDataCount);
                if (ProductDataCount == 0)
                {
                    ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:yellow'></div>&nbsp;";
                }
                String strProductType = "", strProductTypeDelivery = "";
                if (hdnProductTypeID.Value != null && hdnProductTypeID.Value != "")
                {
                    strProductType = Convert.ToString(CommonComponent.GetScalarCommonData("select name from tb_ProductType where ProductTypeID=" + hdnProductTypeID.Value));
                    if (!String.IsNullOrEmpty(strProductType))
                    {
                        Int32 AssemblyItemCount = 0;
                        if (strProductType.ToLower() == "assembly product")
                        {
                            Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("select COUNT(*) from tb_ProductAssembly where RefProductID="
                                + hdnProductID.Value)), out AssemblyItemCount);
                            if (AssemblyItemCount == 0)
                            {
                                ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:red'></div>&nbsp;";
                            }
                        }
                    }
                }
                if (hdnProductTypeDeliveryID.Value != null && hdnProductTypeDeliveryID.Value != "")
                {
                    strProductTypeDelivery = Convert.ToString(CommonComponent.GetScalarCommonData("select name from tb_ProductTypeDelivery where ProductTypeDeliveryID=" + hdnProductTypeDeliveryID.Value));
                    if (!String.IsNullOrEmpty(strProductTypeDelivery))
                    {
                        Int32 VendorSKUCount = 0;
                        if (strProductTypeDelivery.ToLower() == "dropship")
                        {
                            Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("select COUNT(*) from tb_ProductVendorSKU where ProductID="
                                    + hdnProductID.Value)), out VendorSKUCount);
                            if (VendorSKUCount == 0)
                            {
                                ltStatusColor.Text = "<div style='float:left; width:10px; height:10px; background-color:red'></div>&nbsp;";
                            }
                        }
                    }
                }

            }

            #endregion

            #region eBay Field Listing
            bool ebaygridbind = false;
            if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("ebay") > -1)
            {
                ebaygridbind = true;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (ebaygridbind == true)
                {
                    e.Row.Cells[7].Visible = e.Row.Cells[8].Visible = e.Row.Cells[9].Visible = true;
                    e.Row.Cells[10].Visible = e.Row.Cells[11].Visible = false;
                }
                else
                {
                    e.Row.Cells[7].Visible = e.Row.Cells[8].Visible = e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = e.Row.Cells[11].Visible = true;

                }
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (ebaygridbind == true)
                {
                    e.Row.Cells[7].Visible = e.Row.Cells[8].Visible = e.Row.Cells[9].Visible = true;
                    e.Row.Cells[10].Visible = e.Row.Cells[11].Visible = false;
                }
                else
                {
                    e.Row.Cells[7].Visible = e.Row.Cells[8].Visible = e.Row.Cells[9].Visible = false;
                    e.Row.Cells[10].Visible = e.Row.Cells[11].Visible = true;

                }
            }

            #endregion
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            // int StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            //Store is selected dynamically from menu
            if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]))
                ddlStore.SelectedValue = Request.QueryString["StoreID"].ToString();
            else
                ddlStore.SelectedIndex = 0;

            #region Commented Code for amazon Upload Buttons - Do not Delete
            if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("wayfailr") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("lnt") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("bellacor") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("kohl") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("houzz") > -1 || ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("atg") > -1 || (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1))
            {
                btnExport.Visible = true;
            }
            else
            {
                btnExport.Visible = false;
            }

            if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("amazon") > -1)
            {
                btnAmazonPrice.Visible = true;
                btnAmazonProduct.Visible = true;
                btnAmozonUpdate.Visible = true;
                btnAmazonImage.Visible = true;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
            }
            else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("ebay") > -1)
            {

                btneBayProduct.Visible = true;
                btneBayUpdate.Visible = true;

                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;
                btnAmazonImage.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
            }
            else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("sears") > -1)
            {

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnAmazonImage.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;

                btnSearsProduct.Visible = true;
                btnSearsUpdate.Visible = true;
                btnSearsPrice.Visible = true;

                btnOverStockUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
            }
            else if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1)
            {
                btnOverStockUpdate.Visible = true;
                btnOverStockAllUpdate.Visible = true;
                btnOverStockProduct.Visible = true;

                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;
                btnAmazonImage.Visible = false;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;
            }
            else
            {
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;
                btnAmazonImage.Visible = false;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockAllUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
            }
            #endregion


        }

        /// <summary>
        /// Set value for Update
        /// </summary>
        private void SetValueForUpdate()
        {
            foreach (GridViewRow row in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                if (chk.Checked)
                {
                    HiddenField hdnProductID = (HiddenField)row.FindControl("hdnProductid");
                    Label lblStoreID = (Label)row.FindControl("lblStoreID");
                    Label lblSalePrice = (Label)row.FindControl("lblSalePrice");
                    Label lblPrice = (Label)row.FindControl("lblPrice");
                    Label lblInventory = (Label)row.FindControl("lblInventory");

                    if (lblSalePrice.Text.Trim() == "")
                        lblSalePrice.Text = "0";
                    decimal Value = 0;
                    if (decimal.TryParse(txtValue.Text.Trim(), out Value))
                    {
                        if (DoValidation(Convert.ToDecimal(lblSalePrice.Text.Trim().ToString()), Convert.ToDecimal(lblPrice.Text.Trim().ToString()), Convert.ToDecimal(txtValue.Text.Trim().ToString())))
                        {
                            ProductComponent.UpdateMultiplePriceForProduct(ddlFieldName.SelectedValue, Convert.ToDecimal(txtValue.Text.Trim()), Convert.ToInt32(hdnProductID.Value), Convert.ToInt32(lblStoreID.Text), ddlValue.SelectedValue, ddlOperation.SelectedValue);
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Please Enter valid Value";
                        trMsg.Attributes.Add("style", "display:'';");
                    }
                }
            }
        }

        /// <summary>
        /// Check entered Value is valid or not For Price and Sale price
        /// </summary>
        /// <param name="SalePrice">Decimal SalePrice</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="txtValue">Decimal txtValue</param>
        /// <returns>True if succeed, else false</returns>
        private bool DoValidation(decimal SalePrice, decimal Price, decimal txtValue)
        {
            if (ddlValue.SelectedValue == "Value")
            {
                if (ddlFieldName.SelectedValue == "SalePrice" && ddlOperation.SelectedValue.ToString() == "Add" && ((SalePrice + txtValue) > Price))
                {
                    lblMsg.Text = "SalePrice must be less than  or equal to Price...";
                    trMsg.Attributes.Add("style", "display:'';");

                    return false;
                }
                else if (ddlFieldName.SelectedValue == "Price" && ddlOperation.SelectedValue.ToString() == "Add" && ((Price + txtValue) < SalePrice))
                {
                    lblMsg.Text = "SalePrice must be less than  or equal to Price...";
                    lblMsg.Visible = true;
                    return false;
                }

                else if (ddlFieldName.SelectedValue == "Price" && (Price + txtValue <= 0))
                {
                    lblMsg.Text = "Price can not be zero or less than zero...";
                    lblMsg.Visible = true;
                    return false;

                }
                else if (ddlFieldName.SelectedValue == "SalePrice" && ddlOperation.SelectedValue.ToString() == "Exact" && (SalePrice + txtValue < 0))
                {
                    lblMsg.Text = "Sale Price can not be less than zero...";
                    lblMsg.Visible = true;
                    return false;
                }
                return true;
            }
            else
            {

                if (ddlFieldName.SelectedValue == "SalePrice" && ddlOperation.SelectedValue.ToString() == "Add" && (SalePrice + ((txtValue * SalePrice) / 100)) > Price)
                {
                    lblMsg.Text = "SalePrice must be less than  or equal to Price...";
                    lblMsg.Visible = true;
                    trMsg.Attributes.Add("style", "display:block;");
                    return false;
                }
                else if (ddlFieldName.SelectedValue == "Price" && ddlOperation.SelectedValue.ToString().ToLower() == "Add" && (Price + ((txtValue * Price) / 100)) < SalePrice)
                {
                    lblMsg.Text = "SalePrice Must be less than  or equal to Price...";
                    lblMsg.Visible = true;
                    trMsg.Attributes.Add("style", "display:'';");
                    return false;
                }

                else if (ddlFieldName.SelectedValue == "Price" && (Price + (txtValue * Price) / 100) <= 0)
                {
                    lblMsg.Text = "Price can not be zero or less than zero...";
                    lblMsg.Visible = true;
                    trMsg.Attributes.Add("style", "display:'';");
                    return false;
                }
                else if (ddlFieldName.SelectedValue == "SalePrice" && (SalePrice + (txtValue * SalePrice) / 100) < 0)
                {
                    lblMsg.Text = "Sale Price can not be less than zero...";
                    trMsg.Attributes.Add("style", "display:'';");
                    lblMsg.Visible = true;
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// Performing Update on Price and Sale Price Column for Individual Row
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdProduct_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            trMsg.Attributes.Add("style", "display:none;");
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                Label lblSalePrice = (Label)row.FindControl("lblSalePrice");
                Label lblPrice = (Label)row.FindControl("lblPrice");
                Label lblInventory = row.FindControl("lblInventory") as Label;
                TextBox tbSalePrice = (TextBox)row.FindControl("txtSalePrice");
                TextBox tbPrice = (TextBox)row.FindControl("txtPrice");
                TextBox tbInventory = row.FindControl("txtInventory") as TextBox;
                ImageButton btnSave = row.FindControl("btnSave") as ImageButton;
                ImageButton btnCancel = row.FindControl("btnCancel") as ImageButton;
                ImageButton btnEditPrice = row.FindControl("btnEditPrice") as ImageButton;
                HiddenField hdnProductID = row.FindControl("hdnProductid") as HiddenField;
                CheckBox chkSelect = row.FindControl("chkSelect") as CheckBox;
                if (e.CommandName == "EditPrice")
                {
                    // tbInventory.Visible = true;
                    tbSalePrice.Visible = true;
                    tbPrice.Visible = true;
                    btnSave.Visible = true;
                    btnCancel.Visible = true;
                    lblSalePrice.Visible = false;
                    lblPrice.Visible = false;
                    //lblInventory.Visible = false;
                    btnEditPrice.Visible = false;
                }
                else if (e.CommandName == "Save")
                {
                    int Inventory = 0;
                    decimal Price = 0, SalePrice = 0;
                    ProductComponent objProductComponent = new ProductComponent();
                    tb_Product product = new tb_Product();
                    product = objProductComponent.GetAllProductDetailsbyProductID(Convert.ToInt32(hdnProductID.Value));
                    if (decimal.TryParse(tbPrice.Text.Trim(), out Price))
                        product.Price = Convert.ToDecimal(tbPrice.Text.Trim());

                    else
                        tbPrice.Text = lblPrice.Text.Trim();
                    if (decimal.TryParse(tbSalePrice.Text.Trim(), out SalePrice))
                        product.SalePrice = Convert.ToDecimal(tbSalePrice.Text.Trim());
                    else
                        tbSalePrice.Text = lblSalePrice.Text.Trim();
                    //if (int.TryParse(tbInventory.Text.Trim(), out Inventory))
                    //    product.Inventory = Convert.ToInt32(tbInventory.Text.Trim());
                    //else
                    //    tbInventory.Text = lblInventory.Text;
                    product.Inventory = Convert.ToInt32(lblInventory.Text.Trim());
                    bool intl = int.TryParse(lblInventory.Text.Trim(), out Inventory);
                    if (SalePrice > Price)
                    {
                        lblMsg.Text = "SalePrice must be less than  or equal to Price...";
                        trMsg.Attributes.Add("style", "display:'';");
                        //tbSalePrice.Visible = false;
                        //tbPrice.Visible = false;
                        //tbInventory.Visible = false;
                        //btnEditPrice.Visible = true;
                        //btnCancel.Visible = false;
                        //btnSave.Visible = false;
                        //lblPrice.Visible = true;
                        //lblSalePrice.Visible = true;
                        //lblInventory.Visible = true;
                        return;
                    }
                    product.UpdatedOn = DateTime.Now;
                    product.UpdatedBy = Convert.ToInt32(Session["AdminID"].ToString());
                    int RowsAffected = 0;
                    if ((Price > 0 || SalePrice > 0))
                    {
                        RowsAffected = ProductComponent.UpdateProduct(product);
                    }
                    else
                    {
                        lblMsg.Text = "Enter value for Inventory , Price and Sale Price...";
                        trMsg.Attributes.Add("style", "display:'';");
                        return;
                    }
                    if (RowsAffected > 0)
                    {
                        grdProduct.DataBind();
                        lblMsg.Text = "Product updated successfully..";
                        trMsg.Attributes.Add("style", "display:'';");
                    }
                }
                else if (e.CommandName == "Cancel")
                {
                    btnEditPrice.Visible = false;
                    btnCancel.Visible = false;
                    btnSave.Visible = true;
                    lblPrice.Visible = true;
                    lblSalePrice.Visible = true;
                    tbSalePrice.Visible = false;
                    tbPrice.Visible = false;
                    chkSelect.Checked = false;
                }
            }
        }

        /// <summary>
        ///  Update Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            SetValueForUpdate();
            grdProduct.DataBind();
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                if (chk.Checked)
                {
                    HiddenField hdnProductID = (HiddenField)row.FindControl("hdnProductid");

                    tb_Product product = new tb_Product();
                    ProductComponent objProductComp = new ProductComponent();
                    product = objProductComp.GetAllProductDetailsbyProductID(Convert.ToInt32(hdnProductID.Value));
                    product.Deleted = true;
                    int RowsAffected = ProductComponent.UpdateProduct(product);

                    //string StrSKU = Convert.ToString(CommonComponent.GetScalarCommonData("Select SKU from tb_Product where ProductID=" + Convert.ToInt32(hdnProductID.Value) + ""));
                    //string StrUPC = Convert.ToString(CommonComponent.GetScalarCommonData("Select UPC from tb_Product where ProductID=" + Convert.ToInt32(hdnProductID.Value) + ""));
                    //if (StrUPC != null && StrUPC != "")
                    //{
                    //    CommonComponent.ExecuteCommonData("Update tb_UPCMaster set SKU=null where SKU='" + StrSKU + "' and UPC='" + StrUPC + "'");
                    //}

                }
            }
            grdProduct.DataBind();
        }

        /// <summary>
        ///  Edit Multiple Price Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnEditMultiplePrice_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in grdProduct.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    Label lblPrice = (Label)row.FindControl("lblPrice");
                    TextBox txtPrice = (TextBox)row.FindControl("txtPrice");
                    Label lblSalePrice = (Label)row.FindControl("lblSalePrice");
                    TextBox txtSalePrice = (TextBox)row.FindControl("txtSalePrice");
                    lblPrice.Visible = false;
                    txtPrice.Visible = true;
                    lblSalePrice.Visible = false;
                    txtSalePrice.Visible = true;
                    btnSaveMultiple.Visible = true;
                    btnCancelMultiple.Visible = true;
                    (sender as Button).Visible = false;
                }
            }
        }

        /// <summary>
        ///  Save Multiple Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSaveMultiple_Click(object sender, EventArgs e)
        {
            decimal Price = 0, SalePrice = 0;
            foreach (GridViewRow row in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                if (chk.Checked)
                {
                    HiddenField hdnProductID = (HiddenField)row.FindControl("hdnProductid");
                    TextBox txtPrice = (TextBox)row.FindControl("txtPrice");
                    TextBox txtSalePrice = (TextBox)row.FindControl("txtSalePrice");
                    Label lblPrice = (Label)row.FindControl("lblPrice");
                    Label lblSalePrice = (Label)row.FindControl("lblSalePrice");
                    tb_Product product = new tb_Product();
                    ProductComponent objProductComp = new ProductComponent();
                    if (!string.IsNullOrEmpty(txtPrice.Text.ToString()) && !string.IsNullOrEmpty(txtSalePrice.Text.ToString()))
                    {
                        Price = Convert.ToDecimal(txtPrice.Text.ToString());
                        SalePrice = Convert.ToDecimal(txtSalePrice.Text.ToString());

                        if (Price >= SalePrice)
                        {
                            product = objProductComp.GetAllProductDetailsbyProductID(Convert.ToInt32(hdnProductID.Value));
                            product.Price = Convert.ToDecimal(txtPrice.Text.Trim());
                            product.SalePrice = Convert.ToDecimal(txtSalePrice.Text.Trim());
                            ProductComponent.UpdateProduct(product);
                            lblMsg.Text = "Product(s) updated successfully...";
                            trMsg.Attributes.Add("style", "display:'';");
                            txtSalePrice.Visible = false;
                            txtPrice.Visible = false;
                            lblPrice.Visible = true;
                            lblSalePrice.Visible = true;
                            chk.Checked = false;
                        }
                        else
                        {
                            lblMsg.Text = "Sale Price must be less than  or equal to Price...";
                            trMsg.Attributes.Add("style", "display:'';");
                            return;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Enter value for Price and Sale Price.";
                        trMsg.Attributes.Add("style", "display:'';");
                        return;
                    }
                    // int RowsAffected = ProductComponent.UpdateProduct(product);
                }
            }
            grdProduct.DataBind();
            btnSaveMultiple.Visible = false;
            btnCancelMultiple.Visible = false;
            btnEditMultiplePrice.Visible = true;
        }

        /// <summary>
        ///  Cancel Multiple Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancelMultiple_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            foreach (GridViewRow row in grdProduct.Rows)
            {
                CheckBox chkSelect = (CheckBox)row.FindControl("chkSelect");
                if (chkSelect != null && chkSelect.Checked)
                {
                    Label lblPrice = (Label)row.FindControl("lblPrice");
                    TextBox txtPrice = (TextBox)row.FindControl("txtPrice");
                    Label lblSalePrice = (Label)row.FindControl("lblSalePrice");
                    TextBox txtSalePrice = (TextBox)row.FindControl("txtSalePrice");
                    lblPrice.Visible = true;
                    txtPrice.Visible = false;
                    lblSalePrice.Visible = true;
                    txtSalePrice.Visible = false;
                    chkSelect.Checked = false;
                }
            }
            btnSaveMultiple.Visible = false;
            btnCancelMultiple.Visible = false;
            btnEditMultiplePrice.Visible = true;
        }

        /// <summary>
        ///  Edit Multiple Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnEditMultipleProduct_Click(object sender, EventArgs e)
        {
            String Productids = "";
            foreach (GridViewRow row in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)row.FindControl("chkSelect");
                HiddenField hdnProductID = (HiddenField)row.FindControl("hdnProductid");
                int ID = Convert.ToInt32(hdnProductID.Value);
                if (chk.Checked)
                {
                    if (Productids != "")
                    {
                        Productids = Productids + "," + ID;
                    }
                    else
                    {
                        Productids = ID.ToString();
                    }
                }
            }
            Session["ProductIDs"] = Productids;
            Response.Redirect("MultipleProduct.aspx");
        }

        /// <summary>
        ///  Upload in eBay Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUploadIneBay_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product cannot be upload because of eBay store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Update in eBay Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateIneBay_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Inventory cannot be Update because of eBay store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUploadInSears_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product cannot be uploaded because Sears store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Update Inventory in Sears Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateInventorySears_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Inventory cannot be Updated because Sears store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Update Price in Sears Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdatePriceSears_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Price cannot be Updated because Sears store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Update in Buy Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateInBuy_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Inventory cannot be Updated because Buy.com store Configuration is pending!', 'Message');});", true);

        }

        /// <summary>
        ///  Update in Best Buy Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateInBestBuy_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Inventory cannot be Updated because BestBuy.com store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Update in New Egg Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateInNewEgg_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Inventory cannot be Updated because NewEgg store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Upload Image in New Egg Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUploadInNewEgg_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product cannot be Updated because NewEgg store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Upload in Amazon Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUploadInamazon_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Product cannot be Updated because Amazon store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Update Inventory Amazon Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateInventoryamazon_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Inventory cannot be Updated because Amazon store Configuration is pending!', 'Message');});", true);
        }

        /// <summary>
        ///  Update Price Amazon Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdatePriceamazon_Click(object sender, EventArgs e)
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Price cannot be Updated because Amazon store Configuration is pending!', 'Message');});", true);
        }

        public void HideButtons()
        {

        }

        /// <summary>
        ///  Amazon Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAmazonProduct_Click(object sender, EventArgs e)
        {
            SendProductFile();
            SendInventoryFile();
            SendPriceFile();
            SendProductImageFile();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgamazonproduct", "jAlert('Product Uploaded Successfully.','Success');", true);
            //GetErrorList("7362093018");
            //GetErrorList(txtSearch.Text.ToString());

        }
        protected void btnAmazonImage_Click(object sender, EventArgs e)
        {
            SendProductImageFile();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgamazonproduct", "jAlert('Image Uploaded Successfully.','Success');", true);
        }


        /// <summary>
        ///  Amazon Price Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAmazonPrice_Click(object sender, EventArgs e)
        {
            SendPriceFile();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgamazonproduct", "jAlert('Price Updated Successfully.','Success');", true);
        }

        /// <summary>
        ///  Update1 Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdate1_Click(object sender, EventArgs e)
        {
            //SendProductFile();
            SendInventoryFile();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgamazonproduct", "jAlert('Inventory Updated Successfully.','Success');", true);
            //SendProductImageFile();
            //GetErrorList(txtSearch.Text.ToString());

        }

        #region "Amazon Product Data Insert/Update"

        /// <summary>
        /// Create Product Message
        /// </summary>
        /// <param name="ProductID">string ProductID</param>
        /// <param name="inventory">int inventory</param>
        /// <param name="MessageID">int MessageID</param>
        /// <param name="ParentDoc">MemoryStream ParentDoc</param>
        private void CreateProductMessage(string ProductID, int inventory, int MessageID, MemoryStream ParentDoc)
        {
            string myString;
            SQLAccess objProduct = new SQLAccess();
            DataSet dsProduct = new DataSet();


            //            dsProduct = objProduct.GetDs(@"SELECT  dbo.tb_AmazonProduct.item_weight_UOM, dbo.tb_AmazonProduct.offering_condition, dbo.tb_Product.Weight,  dbo.tb_Product.ProductID, dbo.tb_Product.AmazonProductId, 
            //                      '' as BulletPoint1, '' as BulletPoint2, dbo.tb_AmazonProduct.product_type as ProductType,isnull(dbo.tb_AmazonProduct.product_tax_code,'') as  product_tax_code,
            //                      dbo.tb_AmazonProduct.brand_name as MFRPartNumber, dbo.tb_Manufacture.Name as MName,dbo.tb_Product.UPC, dbo.tb_Product.Description, dbo.tb_Product.SKU, 
            //                      dbo.tb_Product.Name AS pname,dbo.tb_Product.Inventory, dbo.tb_Product.StoreID, case when isnull(dbo.tb_Product.SalePrice,0)=0 then  isnull(dbo.tb_Product.Price,0) else dbo.tb_Product.SalePrice end as msrp
            //FROM dbo.tb_Product LEFT OUTER JOIN
            //                      dbo.tb_Manufacture ON dbo.tb_Product.ManufactureID = dbo.tb_Manufacture.ManufactureID LEFT OUTER JOIN
            //                      dbo.tb_AmazonProduct ON dbo.tb_Product.ProductID = dbo.tb_AmazonProduct.ProductId WHERE   dbo.tb_Product.StoreID=" + ddlStore.SelectedValue.ToString() + @" AND dbo.tb_Product.ProductID in (" + ProductID.ToString() + @")");

            dsProduct = objProduct.GetDs(@"SELECT    dbo.tb_Product.[Weight],  dbo.tb_Product.ProductID, dbo.tb_Product.AmazonProductId, 
                                             ISNULL(dbo.tb_ProductType.Name,'') as ProductType,'' as  product_tax_code,
                                            dbo.tb_Manufacture.Name as MName,dbo.tb_Product.UPC, dbo.tb_Product.Description, dbo.tb_Product.SKU, 
                                            dbo.tb_Product.Name AS pname,dbo.tb_Product.Inventory, dbo.tb_Product.StoreID, case when isnull(dbo.tb_Product.SalePrice,0)=0 then  isnull(dbo.tb_Product.Price,0) else dbo.tb_Product.SalePrice end as msrp,isnull(tb_ProductAmazon.ItemType,'') as ItemType,isnull(tb_ProductAmazon.Brand,'') as Brand,tb_ProductAmazon.BulletPoint1,tb_ProductAmazon.BulletPoint2,tb_ProductAmazon.BulletPoint3,tb_ProductAmazon.BulletPoint4,tb_ProductAmazon.BulletPoint5,
                                            tb_ProductAmazon.PlatinumKeywords1,tb_ProductAmazon.PlatinumKeywords2,tb_ProductAmazon.PlatinumKeywords3,tb_ProductAmazon.PlatinumKeywords4,tb_ProductAmazon.PlatinumKeywords5,ISNULL(dbo.tb_Product.Color,'') as Color,ISNULL(dbo.tb_Product.Size,'') as Size  FROM dbo.tb_Product LEFT OUTER JOIN
                                            dbo.tb_Manufacture ON dbo.tb_Product.ManufactureID = dbo.tb_Manufacture.ManufactureID  LEFT OUTER JOIN
                                            dbo.tb_ProductAmazon ON dbo.tb_Product.ProductID = dbo.tb_ProductAmazon.AmazonRefID LEFT OUTER JOIN dbo.tb_ProductType ON CAST(dbo.tb_ProductAmazon.ProductType AS INT) = dbo.tb_ProductType.ProductTypeID WHERE   dbo.tb_Product.StoreID=" + ddlStore.SelectedValue.ToString() + @" AND dbo.tb_Product.ProductID in (" + ProductID.ToString() + @")");
            string strDefaultcategory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT configvalue FROM tb_AppConfig WHERE Storeid=3 And configname='AmazonitemType' and isnull(Deleted,0)=0"));
            if (string.IsNullOrEmpty(strDefaultcategory))
            {
                strDefaultcategory = "HOME_FURNITURE_AND_DECOR";
            }

            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                int iCount = 0;
                for (int i = 0; i < dsProduct.Tables[0].Rows.Count; i++)
                {
                    iCount += 1;
                    myString = "<Message>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<MessageID>" + iCount.ToString() +
                        "</MessageID>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["AmazonProductId"].ToString()))
                    {

                        myString = "<OperationType>Update</OperationType>";
                    }
                    else
                    {
                        myString = "<OperationType>Update</OperationType>";
                    }
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<Product>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    ////////////Data///////////

                    myString = "<SKU>" + dsProduct.Tables[0].Rows[i]["SKU"].ToString() + "</SKU>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["AmazonProductId"].ToString()))
                    {
                        myString = "<StandardProductID>";
                        this.AddStringToStream(ref myString, ParentDoc);
                        myString = "<Type>ASIN</Type>";
                        this.AddStringToStream(ref myString, ParentDoc);
                        myString = "<Value>" + dsProduct.Tables[0].Rows[i]["AmazonProductId"].ToString() + "</Value>";
                        this.AddStringToStream(ref myString, ParentDoc);
                        myString = "</StandardProductID>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    else if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["UPC"].ToString()))
                    {
                        myString = "<StandardProductID>";
                        this.AddStringToStream(ref myString, ParentDoc);
                        myString = "<Type>UPC</Type>";
                        this.AddStringToStream(ref myString, ParentDoc);
                        myString = "<Value>" + dsProduct.Tables[0].Rows[i]["UPC"].ToString() + "</Value>"; //" + dsProduct.Tables[0].Rows[i]["UPC"].ToString() + " //B0009F4YR6
                        this.AddStringToStream(ref myString, ParentDoc);
                        myString = "</StandardProductID>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["product_tax_code"].ToString()))
                    {
                        myString = "<ProductTaxCode>" + dsProduct.Tables[0].Rows[i]["product_tax_code"].ToString() + "</ProductTaxCode>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    myString = "<Condition>";
                    this.AddStringToStream(ref myString, ParentDoc);
                    //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["offering_condition"].ToString()))
                    //{
                    //    myString = "<ConditionType>" + dsProduct.Tables[0].Rows[i]["offering_condition"].ToString() + "</ConditionType>";
                    //}
                    //else
                    //{
                    myString = "<ConditionType>New</ConditionType>";
                    // }
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "</Condition>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<NumberOfItems>" + dsProduct.Tables[0].Rows[i]["Inventory"].ToString() + "</NumberOfItems>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<DescriptionData>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<Title>" + dsProduct.Tables[0].Rows[i]["pname"].ToString() + "</Title>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<Brand>" + dsProduct.Tables[0].Rows[i]["Brand"].ToString() + "</Brand>";//" + dsProduct.Tables[0].Rows[i]["MFRPartNumber"].ToString() + "
                    this.AddStringToStream(ref myString, ParentDoc);
                    //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["Color"].ToString()))
                    //{
                    //    myString = "<Color>" + dsProduct.Tables[0].Rows[i]["Color"].ToString() + "</Color>";//" + dsProduct.Tables[0].Rows[i]["MFRPartNumber"].ToString() + "
                    //    this.AddStringToStream(ref myString, ParentDoc);
                    //}


                    myString = "<Description>" + System.Text.RegularExpressions.Regex.Replace(dsProduct.Tables[0].Rows[i]["Description"].ToString().Replace("&nbsp;", " "), @"<[^>]*>", string.Empty) + "</Description>";
                    this.AddStringToStream(ref myString, ParentDoc);
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["BulletPoint1"].ToString()))
                    {
                        myString = "<BulletPoint>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["BulletPoint1"].ToString()) + "</BulletPoint>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }

                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["BulletPoint2"].ToString()))
                    {
                        myString = "<BulletPoint>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["BulletPoint2"].ToString()) + "</BulletPoint>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["BulletPoint3"].ToString()))
                    {
                        myString = "<BulletPoint>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["BulletPoint3"].ToString()) + "</BulletPoint>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["BulletPoint4"].ToString()))
                    {
                        myString = "<BulletPoint>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["BulletPoint4"].ToString()) + "</BulletPoint>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["BulletPoint5"].ToString()))
                    {
                        myString = "<BulletPoint>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["BulletPoint5"].ToString()) + "</BulletPoint>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }

                    myString = "<ShippingWeight unitOfMeasure=\"LB\">" + string.Format("{0:0.00}", Convert.ToDecimal(dsProduct.Tables[0].Rows[i]["Weight"].ToString())) + "</ShippingWeight>";
                    this.AddStringToStream(ref myString, ParentDoc);



                    myString = "<MSRP currency=\"USD\">" + string.Format("{0:0.00}", Convert.ToDecimal(dsProduct.Tables[0].Rows[i]["msrp"].ToString())) + "</MSRP>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<Manufacturer>" + dsProduct.Tables[0].Rows[i]["mname"].ToString() + "</Manufacturer>";//" + dsProduct.Tables[0].Rows[i]["mname"].ToString() + "
                    this.AddStringToStream(ref myString, ParentDoc);

                    //myString = "<MfrPartNumber>" + dsProduct.Tables[0].Rows[i]["SKU"].ToString() + "</MfrPartNumber>";
                    //this.AddStringToStream(ref myString, ParentDoc);


                    myString = "<ItemType>" + strDefaultcategory.ToString() + "</ItemType>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["PlatinumKeywords1"].ToString()))
                    {
                        myString = "<SearchTerms>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["PlatinumKeywords1"].ToString()) + "</SearchTerms>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }

                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["PlatinumKeywords2"].ToString()))
                    {
                        myString = "<SearchTerms>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["PlatinumKeywords2"].ToString()) + "</SearchTerms>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["PlatinumKeywords3"].ToString()))
                    {
                        myString = "<SearchTerms>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["PlatinumKeywords3"].ToString()) + "</SearchTerms>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["PlatinumKeywords4"].ToString()))
                    {
                        myString = "<SearchTerms>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["PlatinumKeywords4"].ToString()) + "</SearchTerms>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["PlatinumKeywords5"].ToString()))
                    {
                        myString = "<SearchTerms>" + Convert.ToString(dsProduct.Tables[0].Rows[i]["PlatinumKeywords5"].ToString()) + "</SearchTerms>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }

                    myString = "</DescriptionData>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<ProductData>";
                    this.AddStringToStream(ref myString, ParentDoc);


                    myString = "<Home>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<ProductType>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<FurnitureAndDecor>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    //myString = "<Wattage>1</Wattage>";
                    //this.AddStringToStream(ref myString, ParentDoc);

                    //myString = "<Directions>Example Directions</Directions>";
                    //this.AddStringToStream(ref myString, ParentDoc);
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["Color"].ToString()))
                    {
                        myString = "<ColorMap>" + dsProduct.Tables[0].Rows[i]["Color"].ToString() + "</ColorMap>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["Size"].ToString()))
                    {
                        //myString = "<Design>" + dsProduct.Tables[0].Rows[i]["Size"].ToString() + "</Design>";
                        //this.AddStringToStream(ref myString, ParentDoc);
                    }
                    if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[i]["ItemType"].ToString()))
                    {
                        myString = "<Material>" + dsProduct.Tables[0].Rows[i]["ItemType"].ToString().Replace("&", "&amp;") + "</Material>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    //myString = "<Material>Cotton</Material>";
                    //this.AddStringToStream(ref myString, ParentDoc);

                    myString = "</FurnitureAndDecor>";
                    this.AddStringToStream(ref myString, ParentDoc);
                    myString = "</ProductType>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<Parentage>base-product</Parentage>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "</Home>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    //myString = "</Home>";
                    //this.AddStringToStream(ref myString, ParentDoc);

                    myString = "</ProductData>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    //myString = "<LaunchDate>" + DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ssCST") + "</LaunchDate>";
                    //this.AddStringToStream(ref myString, ParentDoc);

                    myString = "</Product>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "</Message>";
                    this.AddStringToStream(ref myString, ParentDoc);
                }
            }
        }

        /// <summary>
        /// Get Product Feed for Amazon Store
        /// </summary>
        /// <param name="id">string ID</param>
        /// <returns>Returns the Product Feed as a Memory Stream</returns>
        public System.IO.MemoryStream GetProductFeed(string id)
        {
            System.IO.MemoryStream myDocument = new System.IO.MemoryStream();
            string myString;

            //Add the document header.
            myString = "<?xml version=\"1.0\" encoding=\"iso-8859-1\" ?>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<AmazonEnvelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"amzn-envelope.xsd\">";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<DocumentVersion>1.01</DocumentVersion>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MerchantIdentifier>" +
                id +
                "</MerchantIdentifier>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "</Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MessageType>Product</MessageType>";
            this.AddStringToStream(ref myString, myDocument);
            myString = "<PurgeAndReplace>false</PurgeAndReplace>";
            this.AddStringToStream(ref myString, myDocument);
            string ids = "0";
            //Add the individual messages to the document.    for (int i = 1; i <= SKUs.Count; i++)
            foreach (GridViewRow dr in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                HiddenField lblProductId = (HiddenField)dr.FindControl("hdnProductid");
                Label lblInventory = (Label)dr.FindControl("lblInventory");


                int iCount = 0;
                if (chk.Checked == true)
                {
                    ids += "," + lblProductId.Value.ToString();

                    //this.CreateInventoryMessage("0Z-2WTI-WY7H", Convert.ToInt32(2), 1, myDocument);
                }

            }
            if (ids != "0")
            {
                this.CreateProductMessage(ids.ToString(), Convert.ToInt32(0), 0, myDocument);
            }

            myString = "</AmazonEnvelope>";
            this.AddStringToStream(ref myString, myDocument);

            return myDocument;
        }

        /// <summary>
        /// Get Error List for Amazon Store
        /// </summary>
        /// <param name="str">string str</param>
        private void GetErrorList(string str)
        {
            try
            {
                MarketplaceWebService.Model.GetFeedSubmissionResultRequest fd = new MarketplaceWebService.Model.GetFeedSubmissionResultRequest();
                fd.FeedSubmissionId = str.ToString();
                // MarketplaceWebService.Model.
                String strAmazonFeedPath = Convert.ToString(AppLogic.AppConfigs("AmazonProductFeed"));
                if (!System.IO.Directory.Exists(Server.MapPath(strAmazonFeedPath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(strAmazonFeedPath));
                }
                fd.FeedSubmissionResult = File.Open(Server.MapPath(strAmazonFeedPath.ToString() + "AmazonError_Test" + str + "_ImageError.xml"), FileMode.OpenOrCreate, FileAccess.ReadWrite);
                SQLAccess objdb = new SQLAccess();
                String accessKeyId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
                String secretAccessKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));
                string applicationName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));
                string merchantId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
                string marketplaceId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));

                MarketplaceWebService.MarketplaceWebServiceConfig mwsConfig2 = new MarketplaceWebService.MarketplaceWebServiceConfig();
                string ServiceURLAmz = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig where configname='AmazonServiceURL' AND Storeid=3"));
                mwsConfig2.ServiceURL = ServiceURLAmz.ToString();
                //mwsConfig2.ServiceURL = AmazonEndpointUrl;
                //mwsConfig2.SetUserAgentHeader("AMService", "2009-01-01", "C#", new string[] { });
                fd.Merchant = merchantId;
                fd.Marketplace = marketplaceId;
                MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, applicationName.ToString(), "1.01", mwsConfig2);
                MarketplaceWebService.Model.GetFeedSubmissionResultResponse submitres = mwsclient.GetFeedSubmissionResult(fd);
                //MarketplaceWebService.Model.GetFeedSubmissionResultResult tt = new MarketplaceWebService.Model.GetFeedSubmissionResultResult();

                // tt = submitres.GetFeedSubmissionResultResult;




            }
            catch
            {
            }
        }

        /// <summary>
        /// Send Product File for Amazon Store
        /// </summary>
        public void SendProductFile()
        {
            System.IO.MemoryStream objDocument = new System.IO.MemoryStream();
            try
            {

                SQLAccess objdb = new SQLAccess();
                String accessKeyId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
                String secretAccessKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));
                string applicationName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));
                string merchantId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
                string marketplaceId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));



                MarketplaceWebService.MarketplaceWebServiceConfig mwsConfig2 = new MarketplaceWebService.MarketplaceWebServiceConfig();
                string ServiceURLAmz = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig where configname='AmazonServiceURL' AND Storeid=3"));
                mwsConfig2.ServiceURL = ServiceURLAmz.ToString();
                //mwsConfig2.ServiceURL = AmazonEndpointUrl;
                //mwsConfig2.SetUserAgentHeader("AMService", "2009-01-01", "C#", new string[] { });

                MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, applicationName.ToString(), "1.01", mwsConfig2);
                // MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient("0PB842ExampleN4ZTR2", "SvSExamplefZpSignaturex2cs%3D", "test", "v1.0", mwsConfig2);

                MarketplaceWebService.Model.SubmitFeedRequest sfrequest = new MarketplaceWebService.Model.SubmitFeedRequest();

                sfrequest.Merchant = merchantId;
                sfrequest.Marketplace = marketplaceId;

                objDocument = GetProductFeed(merchantId);

                System.IO.MemoryStream stre = objDocument;// GetProductFeed(merchantId);

                sfrequest.FeedContent = stre;

                //   sfrequest.WithContentMD5(MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent));
                sfrequest.FeedContent.Position = 0;


                sfrequest.FeedType = "_POST_PRODUCT_DATA_";
                sfrequest.ContentMD5 = MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent);
                MarketplaceWebService.Model.SubmitFeedResponse submitres = mwsclient.SubmitFeed(sfrequest);
                String strAmazonFeedPath = Convert.ToString(AppLogic.AppConfigs("AmazonProductFeed"));
                if (!System.IO.Directory.Exists(Server.MapPath(strAmazonFeedPath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(strAmazonFeedPath));
                }
                StreamWriter objWrite = new StreamWriter(Server.MapPath(strAmazonFeedPath + "Product_feedSubmissionResult_" + DateTime.Now.Date.Ticks.ToString() + ".xml"), false);
                objWrite.Write(submitres.ToXML());
                objWrite.Close();
                objWrite.Dispose();
                //string strFeedId = submitres.SubmitFeedResult.FeedSubmissionInfo.FeedSubmissionId.ToString();
                // GetErrorList(strFeedId);
                //SendProductFileWebStore(objDocument);
                //InvokeSubmitFeed(mwsclient, sfrequest);



                //mwsclient.signQuer(url, AWSSecretAccessKey);

            }
            catch (Exception ex)
            {
                //if (ex.Message.ToString().ToLower().IndexOf("content-md5 we calculated for your feed") > -1)
                //{
                //    string[] str = ex.Message.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //    SendProductFile(str[str.Length - 1].ToString().Replace(")", ""), objDocument);
                //}
                //lblMsg.Text = ex.Message.ToString();
            }

        }

        /// <summary>
        /// Send Product File Web Store for Amazon Store
        /// </summary>
        /// <param name="productDoc">MemoryStream productDoc</param>
        public void SendProductFileWebStore(System.IO.MemoryStream productDoc)
        {
            System.IO.MemoryStream objDocument = new System.IO.MemoryStream();
            try
            {

                SQLAccess objdb = new SQLAccess();
                String accessKeyId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
                String secretAccessKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));
                string applicationName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));
                string merchantId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
                string marketplaceId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));



                MarketplaceWebService.MarketplaceWebServiceConfig mwsConfig2 = new MarketplaceWebService.MarketplaceWebServiceConfig();
                string ServiceURLAmz = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig where configname='AmazonServiceURL' AND Storeid=3"));
                mwsConfig2.ServiceURL = ServiceURLAmz.ToString();
                //mwsConfig2.ServiceURL = AmazonEndpointUrl;
                //mwsConfig2.SetUserAgentHeader("AMService", "2009-01-01", "C#", new string[] { });

                MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, applicationName.ToString(), "1.01", mwsConfig2);
                // MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient("0PB842ExampleN4ZTR2", "SvSExamplefZpSignaturex2cs%3D", "test", "v1.0", mwsConfig2);

                MarketplaceWebService.Model.SubmitFeedRequest sfrequest = new MarketplaceWebService.Model.SubmitFeedRequest();

                sfrequest.Merchant = merchantId;
                sfrequest.Marketplace = marketplaceId;

                objDocument = productDoc;//GetProductFeed(merchantId);

                System.IO.MemoryStream stre = GetProductFeed(merchantId); //objDocument;

                sfrequest.FeedContent = stre;

                //   sfrequest.WithContentMD5(MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent));
                sfrequest.FeedContent.Position = 0;


                sfrequest.FeedType = "_POST_WEBSTORE_ITEM_DATA_";
                sfrequest.ContentMD5 = MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent);
                MarketplaceWebService.Model.SubmitFeedResponse submitres = mwsclient.SubmitFeed(sfrequest);
                String strAmazonFeedPath = Convert.ToString(AppLogic.AppConfigs("AmazonProductFeed"));
                if (!System.IO.Directory.Exists(Server.MapPath(strAmazonFeedPath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(strAmazonFeedPath));
                }
                StreamWriter objWrite = new StreamWriter(Server.MapPath(strAmazonFeedPath.ToString() + "ProductWebStore_feedSubmissionResult_" + DateTime.Now.Date.Ticks.ToString() + ".xml"), false);
                objWrite.Write(submitres.ToXML());
                objWrite.Close();
                objWrite.Dispose();

                //InvokeSubmitFeed(mwsclient, sfrequest);



                //mwsclient.signQuer(url, AWSSecretAccessKey);

            }
            catch (Exception ex)
            {
                //if (ex.Message.ToString().ToLower().IndexOf("content-md5 we calculated for your feed") > -1)
                //{
                //    string[] str = ex.Message.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //    SendProductFile(str[str.Length - 1].ToString().Replace(")", ""), objDocument);
                //}
                //lblMsg.Text = ex.Message.ToString();
            }

        }

        #endregion

        #region "Amazon Product Image Data Insert/Update"

        /// <summary>
        /// Create Product Image Message for Amazon Store
        /// </summary>
        /// <param name="ProductID">string ProductID</param>
        /// <param name="inventory">int inventory</param>
        /// <param name="MessageID">int MessageID</param>
        /// <param name="ParentDoc">MemoryStream ParentDoc</param>
        private void CreateProductImageMessage(string ProductID, int inventory, int MessageID, MemoryStream ParentDoc)
        {
            string myString;
            SQLAccess objProduct = new SQLAccess();
            DataSet dsProduct = new DataSet();


            dsProduct = objProduct.GetDs("SELECT ImageName,SKU FROM tb_Product WHERE StoreID=" + ddlStore.SelectedValue.ToString() + " AND ProductID in (" + ProductID.ToString() + ")");

            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {
                int iCount = 0;
                for (int i = 0; i < dsProduct.Tables[0].Rows.Count; i++)
                {
                    iCount += 1;
                    myString = "<Message>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<MessageID>" + iCount.ToString() +
                        "</MessageID>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    //myString = "<OperationType>Update</OperationType>";
                    //this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<ProductImage>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<SKU>" + dsProduct.Tables[0].Rows[i]["SKU"].ToString() + "</SKU>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    myString = "<ImageType>Main</ImageType>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    StringArrayConverter Storeconvertor = new StringArrayConverter();
                    Array StoreArray = (Array)Storeconvertor.ConvertFrom(AppLogic.AppConfigs("AllowedExtensions"));
                    bool fl = false;
                    //for (int j = 0; j < StoreArray.Length; j++)
                    //{
                    if (File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "large/" + dsProduct.Tables[0].Rows[i]["ImageName"].ToString())))
                    {
                        fl = true;
                        //myString = "<ImageLocation>" + AppLogic.AppConfig("Live_Server_product") + AppLogic.AppConfig("Imagespath").Replace("/images/", "images/") + "Product/medium/" + dsProduct.Tables[0].Rows[i]["ImageName"].ToString() + "</ImageLocation>";
                        myString = "<ImageLocation>" + AppLogic.AppConfigs("LIVE_SERVER") + AppLogic.AppConfigs("ImagePathProduct") + "large/" + dsProduct.Tables[0].Rows[i]["ImageName"].ToString() + "</ImageLocation>";
                        //myString = "<ImageLocation>http://ep.yimg.com/ca/I/yhst-75126210552718_2261_2021459598</ImageLocation>";

                        this.AddStringToStream(ref myString, ParentDoc);

                        //break;
                    }
                    //}
                    if (fl == false)
                    {
                        myString = "<ImageLocation>" + AppLogic.AppConfigs("LIVE_SERVER") + "/" + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.gif</ImageLocation>";
                        this.AddStringToStream(ref myString, ParentDoc);
                    }
                    myString = "</ProductImage>";
                    this.AddStringToStream(ref myString, ParentDoc);
                    myString = "</Message>";
                    this.AddStringToStream(ref myString, ParentDoc);

                    //More Image////
                    FileInfo flnew = new FileInfo(dsProduct.Tables[0].Rows[i]["ImageName"].ToString());
                    Int32 icount = 0;
                    for (int it = 1; it < 26; it++)
                    {
                        for (int j = 0; j < StoreArray.Length; j++)
                        {
                            if (File.Exists(Server.MapPath("/" + AppLogic.AppConfigs("ImagePathProduct").ToString() + "large/" + dsProduct.Tables[0].Rows[i]["ImageName"].ToString().Replace(flnew.Extension.ToString(), "") + "_" + it.ToString() + StoreArray.GetValue(j))))
                            {
                                icount++;
                                iCount++;
                                myString = "<Message>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<MessageID>" + iCount.ToString() +
                                    "</MessageID>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<OperationType>Update</OperationType>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<ProductImage>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<SKU>" + dsProduct.Tables[0].Rows[i]["SKU"].ToString() + "</SKU>";
                                this.AddStringToStream(ref myString, ParentDoc);
                                //myString = "<ProductImage>";
                                //this.AddStringToStream(ref myString, ParentDoc);

                                //myString = "<SKU>" + dsProduct.Tables[0].Rows[i]["SKU"].ToString() + "</SKU>";
                                //this.AddStringToStream(ref myString, ParentDoc);

                                myString = "<ImageType>PT" + icount.ToString() + "</ImageType>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                //myString = "<ImageLocation>http://ep.yimg.com/ca/I/yhst-75126210552718_2261_2021568871</ImageLocation>";
                                myString = "<ImageLocation>" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/" + AppLogic.AppConfigs("ImagePathProduct").ToString() + "large/" + dsProduct.Tables[0].Rows[i]["ImageName"].ToString().Replace(flnew.Extension.ToString(), "") + "_" + it.ToString() + StoreArray.GetValue(j) + "</ImageLocation>";
                                this.AddStringToStream(ref myString, ParentDoc);

                                myString = "</ProductImage>";
                                this.AddStringToStream(ref myString, ParentDoc);
                                myString = "</Message>";
                                this.AddStringToStream(ref myString, ParentDoc);

                            }
                        }
                    }



                    //More Image////


                }
            }
        }

        /// <summary>
        /// Get Product Image Feed for Amazon Store
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>Returns the Product Image Feed as a Memory Stream</returns>
        public System.IO.MemoryStream GetProductImageFeed(string id)
        {
            System.IO.MemoryStream myDocument = new System.IO.MemoryStream();
            string myString;

            //Add the document header.
            myString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<AmazonEnvelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"amzn-envelope.xsd\">";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<DocumentVersion>1.01</DocumentVersion>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MerchantIdentifier>" +
                id +
                "</MerchantIdentifier>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "</Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MessageType>ProductImage</MessageType>";
            this.AddStringToStream(ref myString, myDocument);

            string ids = "0";

            //Add the individual messages to the document.    for (int i = 1; i <= SKUs.Count; i++)
            foreach (GridViewRow dr in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                HiddenField lblProductId = (HiddenField)dr.FindControl("hdnProductid");
                Label lblInventory = (Label)dr.FindControl("lblInventory");


                int iCount = 0;
                if (chk.Checked == true)
                {
                    ids += "," + lblProductId.Value.ToString();
                }

            }
            if (ids != "0")
            {
                this.CreateProductImageMessage(ids.ToString(), Convert.ToInt32(0), 0, myDocument);
            }

            myString = "</AmazonEnvelope>";
            this.AddStringToStream(ref myString, myDocument);

            return myDocument;
        }

        /// <summary>
        /// Send Product Image File for Amazon Store
        /// </summary>
        public void SendProductImageFile()
        {
            System.IO.MemoryStream objDocument = new System.IO.MemoryStream();
            try
            {

                SQLAccess objdb = new SQLAccess();
                String accessKeyId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
                String secretAccessKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));
                string applicationName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));
                string merchantId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
                string marketplaceId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));



                MarketplaceWebService.MarketplaceWebServiceConfig mwsConfig2 = new MarketplaceWebService.MarketplaceWebServiceConfig();
                string ServiceURLAmz = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig where configname='AmazonServiceURL' AND Storeid=3"));
                mwsConfig2.ServiceURL = ServiceURLAmz.ToString();
                //mwsConfig2.ServiceURL = AmazonEndpointUrl;
                //mwsConfig2.SetUserAgentHeader("AMService", "2009-01-01", "C#", new string[] { });

                MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, applicationName.ToString(), "1.01", mwsConfig2);
                // MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient("0PB842ExampleN4ZTR2", "SvSExamplefZpSignaturex2cs%3D", "test", "v1.0", mwsConfig2);

                MarketplaceWebService.Model.SubmitFeedRequest sfrequest = new MarketplaceWebService.Model.SubmitFeedRequest();

                sfrequest.Merchant = merchantId;
                sfrequest.Marketplace = marketplaceId;


                objDocument = GetProductImageFeed(merchantId);
                System.IO.MemoryStream stre = objDocument;// GetProductImageFeed(merchantId);
                sfrequest.FeedContent = stre;

                //   sfrequest.WithContentMD5(MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent));
                sfrequest.FeedContent.Position = 0;
                sfrequest.FeedType = "_POST_PRODUCT_IMAGE_DATA_";
                sfrequest.ContentMD5 = MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent);
                MarketplaceWebService.Model.SubmitFeedResponse submitres = mwsclient.SubmitFeed(sfrequest);
                String strAmazonFeedPath = Convert.ToString(AppLogic.AppConfigs("AmazonProductFeed"));
                if (!System.IO.Directory.Exists(Server.MapPath(strAmazonFeedPath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(strAmazonFeedPath));
                }
                StreamWriter objWrite = new StreamWriter(Server.MapPath(strAmazonFeedPath.ToString() + "Image_feedSubmissionResult_" + DateTime.Now.Date.Ticks.ToString() + ".xml"), false);
                objWrite.Write(submitres.ToXML());
                objWrite.Close();
                objWrite.Dispose();
                //InvokeSubmitFeed(mwsclient, sfrequest);

            }
            catch (Exception ex)
            {
                //if (ex.Message.ToString().ToLower().IndexOf("content-md5 we calculated for your feed") > -1)
                //{
                //    string[] str = ex.Message.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //    SendProductImageFile(str[str.Length - 1].ToString().Replace(")", ""), objDocument);
                //}
                //lblMsg.Text = ex.Message.ToString();
            }

        }

        #endregion

        #region "Amazon Product Inventory Data Insert/Update"

        /// <summary>
        /// Add String to Stream for Amazon Product
        /// </summary>
        /// <param name="StringToAdd">string StringToAdd</param>
        /// <param name="StreamToAddTo">MemorySteam StreamToAddTo</param>
        private void AddStringToStream(ref string StringToAdd, MemoryStream StreamToAddTo)
        {
            //Convert the string into a byte array and add
            //it to the stream.
            System.Text.UTF8Encoding myEncoding =
            new System.Text.UTF8Encoding();
            byte[] myBuffer = myEncoding.GetBytes(StringToAdd);
            StreamToAddTo.Write(myBuffer, 0, myBuffer.Length);
        }

        /// <summary>
        /// Get Invenory Feed for Amazon Store
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>Returns the Inventory Feed as a Memory Stream</returns>
        public System.IO.MemoryStream GetInventoryFeed(string id)
        {
            System.IO.MemoryStream myDocument = new System.IO.MemoryStream();
            string myString;

            //Add the document header.
            myString = "<?xml version=\"1.0\" ?>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<AmazonEnvelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"amzn-envelope.xsd\">";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<DocumentVersion>1.01</DocumentVersion>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MerchantIdentifier>" +
                id +
                "</MerchantIdentifier>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "</Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MessageType>Inventory</MessageType>";
            this.AddStringToStream(ref myString, myDocument);
            //Add the individual messages to the document.    for (int i = 1; i <= SKUs.Count; i++)
            int iCount = 0;
            foreach (GridViewRow dr in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                Label lblSKU = (Label)dr.FindControl("lblSKU");
                Label lblInventory = (Label)dr.FindControl("lblInventory");
                HiddenField hdnUPC = (HiddenField)dr.FindControl("hdnUPC");


                if (chk.Checked == true)
                {
                    iCount = iCount + 1;

                    Int32 inv = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + hdnUPC.Value.ToString() + "','" + lblSKU.Text.ToString() + "'," + ddlStore.SelectedValue.ToString() + ")"));
                    this.CreateInventoryMessage(lblSKU.Text.ToString(), Convert.ToInt32(inv.ToString().Trim()), iCount, myDocument);
                    // this.CreateInventoryMessage("0Z-2WTI-WY7H", Convert.ToInt32(1), 1, myDocument);
                }
            }

            myString = "</AmazonEnvelope>";
            this.AddStringToStream(ref myString, myDocument);

            return myDocument;
        }

        /// <summary>
        /// Create Inventory Message
        /// </summary>
        /// <param name="SKU">string SKU</param>
        /// <param name="inventory">int inventory</param>
        /// <param name="MessageID">int MessageID</param>
        /// <param name="ParentDoc">MemoryStream ParentDoc</param>
        private void CreateInventoryMessage(string SKU, int inventory, int MessageID, MemoryStream ParentDoc)
        {
            string myString;

            myString = "<Message>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "<MessageID>" + MessageID.ToString() +
                "</MessageID>";
            this.AddStringToStream(ref myString, ParentDoc);

            //myString = "<OperationType>Update</OperationType>";
            //this.AddStringToStream(ref myString, ParentDoc);

            myString = "<Inventory>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "<SKU>" + SKU + "</SKU>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "<Quantity>" +
                inventory.ToString() + "</Quantity>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "</Inventory>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "</Message>";
            this.AddStringToStream(ref myString, ParentDoc);
        }

        /// <summary>
        /// Send Inventory File for Amazon Store
        /// </summary>
        public void SendInventoryFile()
        {
            System.IO.MemoryStream objDocument = new System.IO.MemoryStream();
            try
            {

                SQLAccess objdb = new SQLAccess();
                String accessKeyId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
                String secretAccessKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));
                string applicationName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));
                string merchantId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
                string marketplaceId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));



                MarketplaceWebService.MarketplaceWebServiceConfig mwsConfig2 = new MarketplaceWebService.MarketplaceWebServiceConfig();
                string ServiceURLAmz = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig where configname='AmazonServiceURL' AND Storeid=3"));
                mwsConfig2.ServiceURL = ServiceURLAmz.ToString();
                //mwsConfig2.ServiceURL = AmazonEndpointUrl;
                //mwsConfig2.SetUserAgentHeader("AMService", "2009-01-01", "C#", new string[] { });

                MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, applicationName.ToString(), "1.01", mwsConfig2);
                // MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient("0PB842ExampleN4ZTR2", "SvSExamplefZpSignaturex2cs%3D", "test", "v1.0", mwsConfig2);

                MarketplaceWebService.Model.SubmitFeedRequest sfrequest = new MarketplaceWebService.Model.SubmitFeedRequest();

                sfrequest.Merchant = merchantId;
                sfrequest.Marketplace = marketplaceId;


                objDocument = GetInventoryFeed(merchantId);
                System.IO.MemoryStream stre = objDocument;// GetInventoryFeed(merchantId);
                sfrequest.FeedContent = stre;

                //   sfrequest.WithContentMD5(MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent));
                sfrequest.FeedContent.Position = 0;
                sfrequest.FeedType = "_POST_INVENTORY_AVAILABILITY_DATA_";
                sfrequest.ContentMD5 = MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent);
                MarketplaceWebService.Model.SubmitFeedResponse submitres = mwsclient.SubmitFeed(sfrequest);
                String strAmazonFeedPath = Convert.ToString(AppLogic.AppConfigs("AmazonProductFeed"));
                if (!System.IO.Directory.Exists(Server.MapPath(strAmazonFeedPath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(strAmazonFeedPath));
                }
                StreamWriter objWrite = new StreamWriter(Server.MapPath(strAmazonFeedPath + "Inventory_feedSubmissionResult_" + DateTime.Now.Date.Ticks.ToString() + ".xml"), false);
                objWrite.Write(submitres.ToXML());
                objWrite.Close();
                objWrite.Dispose();
                //InvokeSubmitFeed(mwsclient, sfrequest);

            }
            catch (Exception ex)
            {
                //if (ex.Message.ToString().ToLower().IndexOf("content-md5 we calculated for your feed") > -1)
                //{
                //    string[] str = ex.Message.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //    SendInventoryFile(str[str.Length - 1].ToString().Replace(")", ""), objDocument);

                //}
                //lblMsg.Text = ex.Message.ToString();
            }

        }

        /// <summary>
        /// Invoke Submit Feed for Amazon Store
        /// </summary>
        /// <param name="service">MarketplaceWebServiceClient service</param>
        /// <param name="request">SubmitFeedRequest request</param>
        public void InvokeSubmitFeed(MarketplaceWebService.MarketplaceWebServiceClient service, MarketplaceWebService.Model.SubmitFeedRequest request)
        {
            MarketplaceWebService.Model.SubmitFeedResponse submitres = service.SubmitFeed(request);

        }
        #endregion

        #region "Amazon Product Price Data Insert/Update"

        /// <summary>
        /// Get Price Feed for Amazon Store
        /// </summary>
        /// <param name="id">string id</param>
        /// <returns>Returns the Price Feed as a Memory Stream</returns>
        public System.IO.MemoryStream GetPriceFeed(string id)
        {
            System.IO.MemoryStream myDocument = new System.IO.MemoryStream();
            string myString;

            //Add the document header.
            myString = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<AmazonEnvelope xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"amzn-envelope.xsd\">";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<DocumentVersion>1.01</DocumentVersion>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MerchantIdentifier>" +
                id +
                "</MerchantIdentifier>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "</Header>";
            this.AddStringToStream(ref myString, myDocument);

            myString = "<MessageType>Price</MessageType>";
            this.AddStringToStream(ref myString, myDocument);
            //Add the individual messages to the document.    for (int i = 1; i <= SKUs.Count; i++)
            int iCount = 0;
            foreach (GridViewRow dr in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                Label lblSKU = (Label)dr.FindControl("lblSKU");
                Label lblPrice = (Label)dr.FindControl("lblPrice");
                Label lblSalePrice = (Label)dr.FindControl("lblSalePrice");


                if (chk.Checked == true)
                {
                    if (Convert.ToDecimal(lblSalePrice.Text.ToString()) > 0 && Convert.ToDecimal(lblPrice.Text) > Convert.ToDecimal(lblSalePrice.Text))
                    {
                        iCount = iCount + 1;
                        this.CreatePriceMessage(lblSKU.Text.ToString(), Convert.ToString(lblPrice.Text.ToString().Trim()), Convert.ToString(lblSalePrice.Text.ToString().Trim()), iCount, myDocument);
                    }
                    else
                    {
                        iCount = iCount + 1;
                        this.CreatePriceMessage(lblSKU.Text.ToString(), Convert.ToString(lblPrice.Text.ToString().Trim()), Convert.ToString(lblPrice.Text.ToString().Trim()), iCount, myDocument);
                    }

                }
            }

            myString = "</AmazonEnvelope>";
            this.AddStringToStream(ref myString, myDocument);

            return myDocument;
        }

        private void GetItemLookUp()
        {
            //if (!(string.IsNullOrEmpty(ISBN) && string.IsNullOrEmpty(ASIN)))
            //{
            //    AWSECommerceService service = new AWSECommerceService();
            //    ItemLookup lookup = new ItemLookup();
            //    ItemLookupRequest request = new ItemLookupRequest();

            //    lookup.AssociateTag = ConfigurationManager.AppSettings["AssociatesTag"];
            //    lookup.AWSAccessKeyId = ConfigurationManager.AppSettings["AWSAccessKey"];
            //    if (string.IsNullOrEmpty(ASIN))
            //    {
            //        request.IdType = ItemLookupRequestIdType.ISBN;
            //        request.ItemId = new string[] { ISBN.Replace("-", "") };
            //    }
            //    else
            //    {
            //        request.IdType = ItemLookupRequestIdType.ASIN;
            //        request.ItemId = new string[] { ASIN };
            //    }
            //    request.ResponseGroup = ConfigurationManager.AppSettings["AWSResponseGroups"].Split(new char[] { ' ', ',', ';' }, StringSplitOptions.RemoveEmptyEntries);

            //    lookup.Request = new ItemLookupRequest[] { request };
            //    ItemLookupResponse response = service.ItemLookup(lookup);

            //    if (response.Items.Length > 0 && response.Items[0].Item.Length > 0)
            //    {
            //        Item item = response.Items[0].Item[0];
            //        if (item.MediumImage == null)
            //        {
            //            bookImageHyperlink.Visible = false;
            //        }
            //        else
            //        {
            //            bookImageHyperlink.ImageUrl = item.MediumImage.URL;
            //        }
            //        bookImageHyperlink.NavigateUrl = item.DetailPageURL;
            //        bookTitleHyperlink.Text = item.ItemAttributes.Title;
            //        bookTitleHyperlink.NavigateUrl = item.DetailPageURL;
            //        if (item.OfferSummary.LowestNewPrice == null)
            //        {
            //            if (item.OfferSummary.LowestUsedPrice == null)
            //            {
            //                priceHyperlink.Visible = false;
            //            }
            //            else
            //            {
            //                priceHyperlink.Text = string.Format("Buy used {0}", item.OfferSummary.LowestUsedPrice.FormattedPrice);
            //                priceHyperlink.NavigateUrl = item.DetailPageURL;
            //            }
            //        }
            //        else
            //        {
            //            priceHyperlink.Text = string.Format("Buy new {0}", item.OfferSummary.LowestNewPrice.FormattedPrice);
            //            priceHyperlink.NavigateUrl = item.DetailPageURL;
            //        }
            //        if (item.ItemAttributes.Author != null)
            //        {
            //            authorLabel.Text = string.Format("By {0}", string.Join(", ", item.ItemAttributes.Author));
            //        }
            //        else
            //        {
            //            authorLabel.Text = string.Format("By {0}", string.Join(", ", item.ItemAttributes.Creator.Select(c => c.Value).ToArray()));
            //        }
            //        ItemLink link = item.ItemLinks.Where(i => i.Description.Contains("Wishlist")).FirstOrDefault();
            //        if (link == null)
            //        {
            //            wishListHyperlink.Visible = false;
            //        }
            //        else
            //        {
            //            wishListHyperlink.NavigateUrl = link.URL;
            //        }
            //    }
            //}
        }

        /// <summary>
        /// Create Price Message
        /// </summary>
        /// <param name="SKU">string SKU</param>
        /// <param name="inventory">int inventory</param>
        /// <param name="MessageID">int MessageID</param>
        /// <param name="ParentDoc">MemoryStream ParentDoc</param>
        private void CreatePriceMessage(string SKU, string Price, string saleprice, int MessageID, MemoryStream ParentDoc)
        {
            string myString;

            myString = "<Message>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "<MessageID>" + MessageID.ToString() +
                "</MessageID>";
            this.AddStringToStream(ref myString, ParentDoc);

            //myString = "<OperationType>Update</OperationType>";
            //this.AddStringToStream(ref myString, ParentDoc);

            myString = "<Price>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "<SKU>" + SKU + "</SKU>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "<StandardPrice currency=\"USD\">" +
                string.Format("{0:0.00}", Convert.ToDecimal(saleprice)) + "</StandardPrice>";
            //string.Format("{0:0.00}", Convert.ToDecimal(Price)) + "</StandardPrice>";
            this.AddStringToStream(ref myString, ParentDoc);

            //myString = "<Sale>";
            //this.AddStringToStream(ref myString, ParentDoc);

            //myString = "<StartDate>" + DateTime.UtcNow.AddDays(-1).ToString("s") + "Z</StartDate>";
            //this.AddStringToStream(ref myString, ParentDoc);

            //myString = "<EndDate>" + DateTime.UtcNow.AddYears(1).ToString("s") + "Z</EndDate>";
            //this.AddStringToStream(ref myString, ParentDoc);


            //myString = "<SalePrice currency=\"USD\">" + string.Format("{0:0.00}", Convert.ToDecimal(saleprice)) + "</SalePrice>";
            //this.AddStringToStream(ref myString, ParentDoc);

            //myString = "</Sale>";
            //this.AddStringToStream(ref myString, ParentDoc);

            myString = "</Price>";
            this.AddStringToStream(ref myString, ParentDoc);

            myString = "</Message>";
            this.AddStringToStream(ref myString, ParentDoc);
        }

        /// <summary>
        /// Send Price File for Amazon Store
        /// </summary>
        public void SendPriceFile()
        {
            System.IO.MemoryStream objDocument = new System.IO.MemoryStream();
            try
            {

                SQLAccess objdb = new SQLAccess();
                String accessKeyId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonAccessKey' AND Storeid=3"));
                String secretAccessKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonSecretKey' AND Storeid=3"));
                string applicationName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonApplicationName' AND Storeid=3"));
                string merchantId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantID' AND Storeid=3"));
                string marketplaceId = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AmazonMerchantPlaceID' AND Storeid=3"));



                MarketplaceWebService.MarketplaceWebServiceConfig mwsConfig2 = new MarketplaceWebService.MarketplaceWebServiceConfig();
                string ServiceURLAmz = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig where configname='AmazonServiceURL' AND Storeid=3"));
                mwsConfig2.ServiceURL = ServiceURLAmz.ToString();
                //mwsConfig2.ServiceURL = AmazonEndpointUrl;
                //mwsConfig2.SetUserAgentHeader("AMService", "2009-01-01", "C#", new string[] { });

                MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient(accessKeyId, secretAccessKey, applicationName.ToString(), "1.01", mwsConfig2);
                // MarketplaceWebService.MarketplaceWebServiceClient mwsclient = new MarketplaceWebService.MarketplaceWebServiceClient("0PB842ExampleN4ZTR2", "SvSExamplefZpSignaturex2cs%3D", "test", "v1.0", mwsConfig2);

                MarketplaceWebService.Model.SubmitFeedRequest sfrequest = new MarketplaceWebService.Model.SubmitFeedRequest();

                sfrequest.Merchant = merchantId;
                sfrequest.Marketplace = marketplaceId;

                objDocument = GetPriceFeed(merchantId);

                System.IO.MemoryStream stre = objDocument;// GetPriceFeed(merchantId);
                sfrequest.FeedContent = stre;

                //   sfrequest.WithContentMD5(MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent));
                sfrequest.FeedContent.Position = 0;
                sfrequest.FeedType = "_POST_PRODUCT_PRICING_DATA_";
                sfrequest.ContentMD5 = MarketplaceWebService.MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent);
                MarketplaceWebService.Model.SubmitFeedResponse submitres = mwsclient.SubmitFeed(sfrequest);
                String strAmazonFeedPath = Convert.ToString(AppLogic.AppConfigs("AmazonProductFeed"));
                if (!System.IO.Directory.Exists(Server.MapPath(strAmazonFeedPath)))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(strAmazonFeedPath));
                }
                StreamWriter objWrite = new StreamWriter(Server.MapPath(strAmazonFeedPath + "Price_feedSubmissionResult_" + DateTime.Now.Date.Ticks.ToString() + ".xml"), false);
                objWrite.Write(submitres.ToXML());
                objWrite.Close();
                objWrite.Dispose();
                //InvokeSubmitFeed(mwsclient, sfrequest);

            }
            catch (Exception ex)
            {
                //if (ex.Message.ToString().ToLower().IndexOf("content-md5 we calculated for your feed") > -1)
                //{
                //    string[] str = ex.Message.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                //    SendPriceFile(str[str.Length - 1].ToString().Replace(")", ""), objDocument);

                //}
                //lblMsg.Text = ex.Message.ToString();
            }

        }

        #endregion

        #region Code for eBay Upload

        #region Code for Update Inventory in eBay

        /// <summary>
        ///  eBay Inventory Update Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btneBayUpdate_Click(object sender, EventArgs e)
        {
            eBayInventoryUpdate();
        }

        /// <summary>
        /// Get Data for Update Inventory of eBay Product
        /// </summary>
        public void eBayInventoryUpdate()
        {
            string ProductString = string.Empty;
            foreach (GridViewRow r in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                HiddenField lb = (HiddenField)r.FindControl("hdnProductid");
                int ID = Convert.ToInt32(lb.Value.ToString());
                if (chk.Checked)
                {
                    ProductString += ID + ",";
                }
            }
            if (ProductString.Length > 0)
                ProductString = ProductString.Substring(0, ProductString.Length - 1);

            if (!string.IsNullOrEmpty(ProductString.Trim()))
            {
                if (ProductString != null)
                {
                    DataSet ds = new DataSet();
                    DataView dv;

                    string Query = "";
                    if (ddlStore.SelectedValue == "")
                    {
                        // Query = "select isnull(SKU,'') as SKU,isnull(EbayProductID,'') as EbayProductID,isnull(Inventory,0) as Inventory,round((case when(isnull(price,0)=0) then saleprice else price end) ,2)" +
                        // "as price,round(( case when(isnull(SalePrice,0)=0) then price else saleprice end ),2)" +
                        // "as SalePrice from dbo.tb_Ecomm_Product  " +
                        //"where  ProductID in (select items from dbo.Split('" + ProductString + "',','))";

                        Query = @"select isnull(SKU,'') as SKU,isnull(EbayProductID,'') as EbayProductID,isnull(Inventory,0) as Inventory,round((case when(isnull(price,0)=0) then saleprice else price end) ,2)
                                    as price,round(( case when(isnull(SalePrice,0)=0) then price else saleprice end ),2)
                                    as SalePrice,ProductID, isnull(UPC,'') as UPC from dbo.tb_Product  
                                    where  ProductID in (select items from dbo.Split('" + ProductString + "',','))";
                    }
                    else
                    {
                        // Query = "select isnull(SKU,'') as SKU,isnull(EbayProductID,'') as EbayProductID,isnull(Inventory,0) as Inventory,round((case when(isnull(price,0)=0) then saleprice else price end) ,2)" +
                        // "as price,round(( case when(isnull(SalePrice,0)=0) then price else saleprice end ),2)" +
                        // "as SalePrice from dbo.tb_Ecomm_Product  " +
                        //"where storeid=" + ddlStore.SelectedValue.ToString() + " and ProductID in (select items from dbo.Split('" + ProductString + "',','))";

                        Query = @"select isnull(SKU,'') as SKU,isnull(EbayProductID,'') as EbayProductID,isnull(Inventory,0) as Inventory,round((case when(isnull(price,0)=0) then saleprice else price end) ,2)
                                    as price,round(( case when(isnull(SalePrice,0)=0) then price else saleprice end ),2)
                                    as SalePrice,ProductID, isnull(UPC,'') as UPC from dbo.tb_Product  
                                    where storeid=" + ddlStore.SelectedValue.ToString() + " and ProductID in (select items from dbo.Split('" + ProductString + "',','))";
                    }
                    SQLAccess objAccess = new SQLAccess();
                    ds = objAccess.GetDs(Query);
                    if (ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        DataTable distinctTable = ds.Tables[0];
                        reviceInventoryStatusinEbay(distinctTable);

                    }
                }
            }
        }

        /// <summary>
        /// Update Inventory in eBay Store
        /// </summary>
        /// <param name="dt">DataTable dt</param>
        public void reviceInventoryStatusinEbay(DataTable dt)
        {
            //Image_Path = AppLogic.AppConfigs("AdminImagesPath"); // Commented by Girish due to not used
            storeID = AppConfig.StoreID;
            Int32 revicecnt = 0;
            InventoryStatusTypeCollection itemCollection = new InventoryStatusTypeCollection();

            foreach (DataRow dr in dt.Rows)
            {
                //InventoryStatusType item = new InventoryStatusType();
                //item.ItemID = dr["EbayProductID"].ToString().Trim();
                //item.Quantity = Convert.ToInt32(dr["Inventory"].ToString().Trim());
                //item.QuantitySpecified = true;
                //item.SKU = dr["SKU"].ToString().Trim();
                //itemCollection.Add(item);
                //revicecnt++;

                InventoryStatusType item = new InventoryStatusType();

                item.ItemID = dr["EbayProductID"].ToString().Trim(); //110091219077 "110091220802";//


                Int32 Inventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dr["UPC"].ToString() + "','" + dr["SKU"].ToString() + "'," + ddlStore.SelectedValue.ToString() + ")"));
                if (Inventory > 10)
                {
                    Inventory = 10;
                }
                // item.Quantity = Convert.ToInt32(dr["Inventory"].ToString().Trim());
                item.Quantity = Convert.ToInt32(Inventory.ToString().Trim());
                item.QuantitySpecified = true;
                // item.SKU = dr["SKU"].ToString().Trim(); //SB61-SQY006-B105

                ItemType itemUp = new ItemType();
                itemUp.InventoryTrackingMethod = InventoryTrackingMethodCodeType.ItemID;
                itemUp.ListingType = ListingTypeCodeType.FixedPriceItem;
                DataSet dsEbayVar = new DataSet();
                DataSet dsEbayVariant = new DataSet();

                dsEbayVariant = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + dr["ProductID"].ToString() + " Order By VariantName");
                if (dsEbayVariant != null)
                {
                    bool flag = false;
                    int Totalqty = 0;
                    if (dsEbayVariant.Tables[0].Rows.Count > 0)
                    {

                        VariationTypeCollection VarCol = new VariationTypeCollection();
                        NameValueListTypeCollection objColl = new NameValueListTypeCollection();
                        VariationType var1 = new VariationType();

                        for (int l = 0; l < dsEbayVariant.Tables[0].Rows.Count; l++)
                        {
                            //objname.Name = dsEbayVariant.Tables[0].Rows[l]["VariantName"].ToString();
                            dsEbayVar = CommonComponent.GetCommonDataSet("SELECT * FROM Get_Variant_Details WHERE ProductID=" + dr["ProductID"].ToString() + " AND VariantID=" + dsEbayVariant.Tables[0].Rows[l]["VariantID"].ToString() + " Order By VariantName");
                            if (dsEbayVar.Tables[0].Rows.Count > 0)
                            {
                                for (int j = 0; j < dsEbayVar.Tables[0].Rows.Count; j++)
                                {
                                    if (l == 0)
                                    {

                                        NameValueListType objname = new NameValueListType();
                                        objname.Name = dsEbayVariant.Tables[0].Rows[l]["VariantName"].ToString();
                                        StringCollection objnamevalue = new StringCollection();
                                        objnamevalue.Add(dsEbayVar.Tables[0].Rows[j]["VariantValue"].ToString());
                                        objname.Value = objnamevalue;
                                        var1.SKU = dr["SKU"].ToString() + "-" + dsEbayVar.Tables[0].Rows[j]["VariantValue"].ToString();
                                        var1.Quantity = Convert.ToInt32(dsEbayVar.Tables[0].Rows[j]["Inventory"].ToString());
                                        double totalprice = Convert.ToDouble(dr["SalePrice"].ToString());
                                        totalprice = totalprice + Convert.ToDouble(dsEbayVar.Tables[0].Rows[j]["VariantPrice"].ToString());
                                        Totalqty += Convert.ToInt32(dsEbayVar.Tables[0].Rows[j]["Inventory"].ToString());
                                        var1.StartPrice = new AmountType();
                                        var1.StartPrice.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                                        var1.StartPrice.Value = Convert.ToDouble(totalprice.ToString());
                                        var1.VariationSpecifics = new NameValueListTypeCollection();
                                        var1.VariationSpecifics.Add(objname);
                                        objColl.Add(objname);
                                    }
                                    else
                                    {
                                        NameValueListType objname = new NameValueListType();
                                        objname.Name = dsEbayVariant.Tables[0].Rows[l]["VariantName"].ToString();
                                        StringCollection objnamevalue = new StringCollection();
                                        objnamevalue.Add(dsEbayVar.Tables[0].Rows[j]["VariantValue"].ToString());
                                        objname.Value = objnamevalue;
                                        var1.VariationSpecifics.Add(objname);
                                        // var1.VariationSpecifics.Add(objname1);
                                        //VarCol.Add(var1);

                                        // VarCol.Add(var1);

                                        objColl.Add(objname);
                                    }
                                }

                            }
                        }
                        VarCol.Add(var1);
                        itemUp.Variations = new VariationsType();
                        itemUp.Variations.VariationSpecificsSet = objColl;
                        itemUp.Variations.Variation = VarCol;
                        itemUp.Quantity = Totalqty;




                        /////////////NEW//////////////////
                        //itemUp.Variations = new VariationsType();

                        //itemUp.Variations.VariationSpecificsSet = new NameValueListTypeCollection();
                        //for (int l = 0; l < dsEbayVariant.Tables[0].Rows.Count; l++)
                        //{



                        //    NameValueListType NVListVS1 = new NameValueListType();
                        //    NVListVS1.Name = dsEbayVariant.Tables[0].Rows[l]["VariantName"].ToString();
                        //    StringCollection VSvaluecollection1 = new StringCollection();
                        //    string sttvalue = "";
                        //    dsEbayVar = CommonComponent.GetCommonDataSet("SELECT * FROM Get_Variant_Details WHERE ProductID=" + dr["ProductID"].ToString() + " AND VariantID=" + dsEbayVariant.Tables[0].Rows[l]["VariantID"].ToString() + " Order By VariantName");
                        //    if (dsEbayVar.Tables[0].Rows.Count > 0)
                        //    {
                        //        for (int j = 0; j < dsEbayVar.Tables[0].Rows.Count; j++)
                        //        {
                        //            sttvalue += "\"" + dsEbayVar.Tables[0].Rows[j]["VariantValue"].ToString() + "\",";  
                        //        }
                        //    }
                        //    String[] Size = { sttvalue.ToString().Substring(0, sttvalue.ToString().Length - 1) };
                        //    VSvaluecollection1.AddRange(Size);
                        //    NVListVS1.Value = VSvaluecollection1;
                        //    itemUp.Variations.VariationSpecificsSet.Add(NVListVS1);
                        //}

                        //itemUp.Variations.Variation = VarCol; 

                        //NVListVS1.Value = VSvaluecollection1;
                        //itemUp.Variations.VariationSpecificsSet.Add(NVListVS1);

                        //NameValueListType NVListVS2 = new NameValueListType();
                        //NVListVS2.Name = "Colour";
                        //StringCollection VSvaluecollection2 = new StringCollection();
                        //String[] Colour = { "Black", "Blue" };
                        //VSvaluecollection2.AddRange(Colour);

                        //NVListVS2.Value = VSvaluecollection2;
                        //itemUp.Variations.VariationSpecificsSet.Add(NVListVS2); 



                        revicecnt++;
                        ReviseFixedPriceItemCall reviceitem = new ReviseFixedPriceItemCall(GetContext());

                        itemUp.ItemID = dr["EbayProductID"].ToString().Trim();  //"110091220802";
                        reviceitem.Item = itemUp;
                        reviceitem.Execute();

                    }
                    else
                    {
                        itemCollection.Add(item);
                        revicecnt++;
                    }


                }
                else
                {
                    itemCollection.Add(item);
                    revicecnt++;
                }

            }

            try
            {
                if (itemCollection.Count > 0)
                {
                    ReviseInventoryStatusRequestType reviceItemInventory = new ReviseInventoryStatusRequestType();
                    reviceItemInventory.InventoryStatus = itemCollection;
                    ReviseInventoryStatusCall objRequest = new ReviseInventoryStatusCall(GetContext());
                    objRequest.ApiRequest = reviceItemInventory;
                    objRequest.ExecuteRequest(reviceItemInventory);
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message.ToString();
                lblMsg.Visible = true;
                trMsg.Attributes.Add("style", "display:'';");
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('" + ex.Message.ToString() + "'); ", true);
                return;
            }
            lblMsg.Text = revicecnt + " Number of Products Inventory Staus Revised.";
            Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('" + revicecnt + " Number of Products Inventory Staus Revised." + "');", true);
            lblMsg.Visible = true;
            trMsg.Attributes.Add("style", "display:'';");

        }
        #endregion

        #region Code for Upload Product in eBay

        /// <summary>
        ///  eBay Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btneBayProduct_Click(object sender, EventArgs e)
        {
            eBayProductUpload();
        }

        /// <summary>
        /// Get Data for Update Product in eBay 
        /// </summary>
        public void eBayProductUpload()
        {
            string ProductString = string.Empty;
            foreach (GridViewRow r in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                HiddenField lb = (HiddenField)r.FindControl("hdnProductid");
                int ID = Convert.ToInt32(lb.Value.ToString());
                if (chk.Checked)
                {
                    ProductString += ID + ",";
                }
            }
            //ProductString = NewProductID.ToString();
            if (ProductString.Length > 0)
                ProductString = ProductString.Substring(0, ProductString.Length - 1);

            if (ProductString != null)
            {
                Live_Server = AppLogic.AppConfigs("Live_Server");

                if (ProductString != null)
                {
                    DataSet ds = new DataSet();
                    DataView dv;

                    string Query = "";
                    if (ddlStore.SelectedValue == "-1")
                    {
                        // Query = "select *,round((case when(isnull(price,0)=0) then saleprice else price end) ,2)" +
                        // "as price1,round(( case when(isnull(SalePrice,0)=0) then price else saleprice end ),2)" +
                        // "as SalePrice1 from dbo.tb_Ecomm_Product  " +
                        //"where  ProductID in (select items from dbo.Split('" + ProductString + "',','))";

                        Query = @"select *,round((case when(isnull(price,0)=0) then saleprice else price end) ,2)
                                as price1,round(( case when(isnull(SalePrice,0)=0) then price else saleprice end ),2)
                                as SalePrice1 from dbo.tb_Product 
                                where  ProductID in (select items from dbo.Split('" + ProductString + "',','))";
                    }
                    else
                    {
                        // Query = "select *,round((case when(isnull(price,0)=0) then saleprice else price end) ,2)" +
                        // "as price1,round(( case when(isnull(SalePrice,0)=0) then price else saleprice end ),2)" +
                        // "as SalePrice1 from dbo.tb_Ecomm_Product  " +
                        //"where storeid=" + ddlStore.SelectedValue.ToString() + " and ProductID in (select items from dbo.Split('" + ProductString + "',','))";

                        Query = @"select *,round((case when(isnull(price,0)=0) then saleprice else price end) ,2)
                            as price1,round(( case when(isnull(SalePrice,0)=0) then price else saleprice end ),2)
                            as SalePrice1 from dbo.tb_Product 
                            where storeid=" + ddlStore.SelectedValue.ToString() + " and ProductID in (select items from dbo.Split('" + ProductString + "',','))";
                    }
                    SQLAccess objAccess = new SQLAccess();
                    ds = objAccess.GetDs(Query);
                    dv = ds.Tables[0].DefaultView;
                    if (dv.Count > 0)
                    {
                        DataTable dt = dv.Table;
                        DataTable distinctTable = dt.DefaultView.ToTable( /*distinct*/ true);
                        createorupdateebayproducts(distinctTable);

                    }
                }
            }
        }

        /// <summary>
        /// Create or Update eBay Products
        /// </summary>
        /// <param name="dt">DataTable dt</param>
        public void createorupdateebayproducts(DataTable dt)
        {
            SQLAccess objAccess = new SQLAccess();

            string strHtml = string.Empty;
            string strProductPath = string.Empty;
            if (ddlStore.SelectedValue != null)
            {
                strHtml = "ProductHtml-" + ddlStore.SelectedValue.ToString() + ".htm";
                Image_Path = "/ebay/images/";

                strProductPath = AppLogic.AppConfigs("ImagePathProduct").Trim().ToLower().Replace("product/", "");
            }

            Live_Server = AppLogic.AppConfigs("Live_Server_Product");

            // storeID = AdminEcomm.Admin.Client.AppConfigs.StoreId; // Commented By Girish
            storeID = AppConfig.StoreID;


            SQLAccess dbAccess = new SQLAccess();
            string strResponse = "";
            string strResponseoriginal = "";

            System.Net.WebClient Client = new System.Net.WebClient();


            Stream strm = Client.OpenRead(Server.MapPath(strHtml)); //Server.MapPath("ProductHtml.htm")
            StreamReader sr = new StreamReader(strm);
            string strResponse1 = "";
            while ((strResponse1 = sr.ReadLine()) != null)
            {
                strResponse += strResponse1;
            }
            strResponseoriginal = strResponse.ToString();
            sr.Close();

            Int32 insertcnt = 0, revicecnt = 0, relistcnt = 0;

            string PrivacyPolicy = "";
            string ShippingPolicy = "";
            string AboutUs = "";

            string ReturnPolicy = "";
            string ReturnProcess = "";
            string RefundTimeFrame = "";
            string MailThePackageTo = "";
            string ChangesToThePolicy = "";



            PrivacyPolicy = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='PrivacyPolicyEbay'"));
            ShippingPolicy = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='ShippingEbay'"));
            AboutUs = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='AboutUsEbay'"));

            ReturnProcess = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='ReturnProcess'"));
            RefundTimeFrame = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='RefundTimeFrame'"));
            MailThePackageTo = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='MailThePackageTo'"));
            ReturnPolicy = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='EasyReturnsEbay'"));
            ChangesToThePolicy = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='ChangesToThePolicy'"));


            if (string.IsNullOrEmpty(PrivacyPolicy))
                PrivacyPolicy = "Coming Soon";
            if (string.IsNullOrEmpty(ShippingPolicy))
                ShippingPolicy = "Coming Soon";
            if (string.IsNullOrEmpty(AboutUs))
                AboutUs = "Coming Soon";
            if (string.IsNullOrEmpty(ReturnPolicy))
                ReturnPolicy = "Coming Soon";

            if (string.IsNullOrEmpty(ReturnProcess))
                ReturnProcess = "Coming Soon";
            if (string.IsNullOrEmpty(RefundTimeFrame))
                RefundTimeFrame = "Coming Soon";
            if (string.IsNullOrEmpty(MailThePackageTo))
                MailThePackageTo = "Coming Soon";
            if (string.IsNullOrEmpty(ChangesToThePolicy))
                ChangesToThePolicy = "Coming Soon";

            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToBoolean(dr["EbayListingType"]))
                {
                    DataTable dtAuction = new DataTable();
                    dtAuction = dt.Clone();
                    object[] dr1 = dr.ItemArray;
                    dtAuction.Rows.Add(dr1);
                    createorupdateebayproductsAuction(dtAuction);
                }
                else
                {
                    String Category1 = "";
                    String Category2 = "";
                    String StoreCategory1 = "";
                    String StoreCategory2 = "";

                    strResponse = strResponseoriginal;

                    strResponse = strResponse.Replace("/ebay/", Live_Server + "/ebay/");


                    //strResponse = strResponse.Replace("/ebay/", Live_Server + "/");

                    string strReplace = "";
                    if (dr["Description"] != null)
                        strReplace = dr["Description"].ToString();

                    strReplace = strReplace.Replace(strProductPath + "product/", Live_Server + strProductPath + "product/"); //dough full 

                    strResponse = strResponse.ToString().Replace("###shippingpolicy###", ShippingPolicy.ToString());
                    strResponse = strResponse.ToString().Replace("###privacypolicy###", PrivacyPolicy.ToString());
                    strResponse = strResponse.ToString().Replace("###aboutus###", AboutUs.ToString());
                    strResponse = strResponse.ToString().Replace("###returnpolicy###", ReturnPolicy.ToString());

                    strResponse = strResponse.ToString().Replace("###returnprocess###", ReturnProcess.ToString());
                    strResponse = strResponse.ToString().Replace("###refundtimeframe###", RefundTimeFrame.ToString());
                    strResponse = strResponse.ToString().Replace("###mailthepackageto###", MailThePackageTo.ToString());
                    strResponse = strResponse.ToString().Replace("###changestothepolicy###", ChangesToThePolicy.ToString());

                    string Availability = "InStock";
                    if (dr["Avail"] != null && dr["Avail"].ToString().Trim() != "")
                        Availability = dr["Avail"].ToString();
                    strResponse = strResponse.ToString().Replace("###avilability###", Availability.ToString());

                    strResponse = strResponse.ToString().Replace("###Description###", System.Text.RegularExpressions.Regex.Replace(strReplace.ToString(), @"<[^>]*>", string.Empty));
                    strResponse = strResponse.ToString().Replace("###Features###", Convert.ToString(dr["Features"]));
                    // strResponse = strResponse.ToString().Replace(Live_Server + "/Client/css/stylenew.css", Live_Server + "/ebay/css/style-ebay.css"); // Commented - By Girish
                    string warranty = "";
                    if (dr["extended-warranty"] != null)
                        warranty = dr["extended-warranty"].ToString();

                    strResponse = strResponse.ToString().Replace("###warranty###", warranty.Replace(strProductPath + "product", Live_Server + strProductPath + "product"));
                    strResponse = strResponse.Replace("###name###", dr["Name"].ToString());
                    strResponse = strResponse.Replace("###id###", dr["productID"].ToString());
                    if ((dr["SKU"].ToString() != null) && (dr["SKU"].ToString() != ""))
                    {
                        strResponse = strResponse.Replace("###code###", dr["SKU"].ToString());
                    }
                    else
                    {
                        strResponse = strResponse.Replace("###code###", "-");
                    }
                    strResponse = strResponse.Replace("###price###", Convert.ToString(Convert.ToDouble(dr["Price1"].ToString()).ToString("N2")));
                    strResponse = strResponse.Replace("###saleprice###", Convert.ToString(Convert.ToDouble(dr["SalePrice1"].ToString()).ToString("N2")));

                    strResponse = strResponse.Replace("###yousave###", (Convert.ToDouble(Convert.ToDouble(dr["Price1"].ToString()) - Convert.ToDouble(dr["SalePrice1"].ToString())).ToString("f2")).ToString() + " (" + String.Format("{0:0}", (((Convert.ToDouble(dr["Price1"].ToString()) - Convert.ToDouble(dr["SalePrice1"].ToString())) / Convert.ToDouble(dr["Price1"].ToString())) * 100)) + "%)");

                    DataTable dtRating = new DataTable();
                    Decimal AvgRating = 0;
                    Int32 TotalReviews = 0;
                    dtRating = dbAccess.GetDs("select isnull(avg(isnull(rating,0)),0) as AvgRating,isnull(Count(1),0) as TotalReviews from tb_Rating where ProductID=" + dr["ProductID"].ToString()).Tables[0];
                    AvgRating = Convert.ToDecimal(dtRating.Rows[0]["AvgRating"].ToString());
                    TotalReviews = Convert.ToInt32(dtRating.Rows[0]["TotalReviews"].ToString());
                    if (AvgRating == 0)
                        strResponse = strResponse.Replace("###rating###", "");
                    else if (TotalReviews == 1)
                        strResponse = strResponse.Replace("###rating###", "<div class=\"item-right-row\"><p>Product Rating </p>:<span>" + BindStarsImage(AvgRating) + "(" + TotalReviews + " Review)</span> </div>");
                    else
                        strResponse = strResponse.Replace("###rating###", "<div class=\"item-right-row\"><p>Product Rating </p>:<span>" + BindStarsImage(AvgRating) + "(" + TotalReviews + " Reviews)</span> </div>");


                    string ImageServer = string.Empty;

                    if (ddlStore.SelectedValue.ToString() == "7") // here 7 is ebay storeid
                    {
                        ImageServer = strProductPath;
                    }


                    strResponse = strResponse.Replace("###img1###", Live_Server + GetMediumImage(ImageServer + "product/Medium/" + dr["ImageName"].ToString()) + "");
                    string strImages = "";

                    //Getting icon and medium images for image scrolller
                    strImages = strImages + "<li title='" + dr["Name"].ToString() + "'>";


                    string strMoreImageName = string.Empty;
                    strMoreImageName = dr["ImageName"].ToString().Replace(".jpeg", "").Replace(".jpg", "");

                    strImages = strImages + "<img oncontextmenu='return false;' src='" + Live_Server + GetMicroImage(ImageServer + "product/Micro/" + dr["ImageName"].ToString()) + "' alt='" + dr["Name"].ToString() + "' title='" + dr["Name"].ToString() + "' onmouseover='javascript:document.getElementById(&quot;ProductPic" + dr["productID"].ToString() + "&quot;).src=&quot;" + Live_Server + GetMediumImage(ImageServer + "product/Medium/" + dr["ImageName"].ToString()) + "&quot;;currentID=this.parentNode.id;'></li>";
                    for (int i = 1; i < 26; i++)
                    {

                        if (System.IO.File.Exists(Server.MapPath(ImageServer + "Product/Micro/" + strMoreImageName + "_" + i.ToString() + "_.jpg")) == true)
                        {
                            strImages = strImages + "<li  title='" + dr["Name"].ToString() + "' style='float:left;'>";
                            strImages = strImages + "<img oncontextmenu='return false;' src='" + Live_Server + ImageServer + "product/Micro/" + strMoreImageName + "_" + i.ToString() + ".jpg' alt='" + dr["Name"].ToString() + "' title='" + dr["Name"].ToString() + "' onmouseover='javascript:document.getElementById(&quot;ProductPic" + dr["productID"].ToString() + "&quot;).src=&quot;" + Live_Server + GetMediumImage(ImageServer + "product/Medium/" + strMoreImageName.ToString() + "_" + i.ToString() + ".jpg") + "&quot;;currentID=this.parentNode.id;'></li>";
                        }
                    }

                    strResponse = strResponse.Replace("###images###", strImages.ToString());
                    strResponse = strResponse.Replace("\t", "");
                    strResponse = strResponse.Replace("\" />", "\"/>");
                    strResponse = strResponse.Replace("\" >", "\">");

                    string Description = strResponse.ToString().Replace("\r\n", "");


                    ItemType item = new ItemType();

                    item.Currency = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    item.Country = eBay.Service.Core.Soap.CountryCodeType.US;
                    item.Site = SiteCodeType.US;

                    string pname = dr["Name"].ToString();
                    if (dr["Name"].ToString().Length > 55)
                        pname = dr["Name"].ToString().Substring(0, 51) + "...";

                    item.Title = pname;

                    item.Description = Description;

                    string returnpolicy = "";

                    if (dr["extended-warranty"] != null)
                        returnpolicy = dr["extended-warranty"].ToString().Replace(strProductPath + "product", Live_Server + strProductPath + "product"); // Need to changes by Girish


                    ReturnPolicyType po = new ReturnPolicyType();
                    po.WarrantyOffered = returnpolicy;
                    po.ReturnsAcceptedOption = "ReturnsAccepted";
                    po.RefundOption = "MoneyBackOrExchange";
                    po.ReturnsWithinOption = "Days_30";
                    po.Description = " ";
                    po.ShippingCostPaidByOption = "Buyer";
                    item.ReturnPolicy = po;
                    NameValueListType objSpecific = new NameValueListType();
                    NameValueListTypeCollection objCollmaterial = new NameValueListTypeCollection();
                    NameValueListType objnameSpecific = new NameValueListType();
                    if (!string.IsNullOrEmpty(dr["ProductSummary"].ToString()))
                    {
                        objSpecific = new NameValueListType();
                        objnameSpecific = new NameValueListType();
                        objnameSpecific.Name = "Type";
                        StringCollection objnamevalue = new StringCollection();
                        objnamevalue.Add(dr["ProductSummary"].ToString());
                        objnameSpecific.Value = objnamevalue;
                        objCollmaterial.Add(objnameSpecific);
                    }
                    if (!string.IsNullOrEmpty(dr["Materials"].ToString()))
                    {
                        objSpecific = new NameValueListType();
                        objnameSpecific = new NameValueListType();
                        objnameSpecific.Name = "Material";
                        StringCollection objnamevalue = new StringCollection();
                        objnamevalue.Add(dr["Materials"].ToString());
                        objnameSpecific.Value = objnamevalue;
                        objCollmaterial.Add(objnameSpecific);
                    }
                    if (!string.IsNullOrEmpty(dr["Brand"].ToString()))
                    {
                        objSpecific = new NameValueListType();
                        objnameSpecific = new NameValueListType();
                        objnameSpecific.Name = "Brand";
                        StringCollection objnamevalue = new StringCollection();
                        objnamevalue.Add(dr["Brand"].ToString());
                        objnameSpecific.Value = objnamevalue;
                        objCollmaterial.Add(objnameSpecific);
                    }
                    if (!string.IsNullOrEmpty(dr["ManufacturePartNo"].ToString()))
                    {
                        objSpecific = new NameValueListType();
                        objnameSpecific = new NameValueListType();
                        objnameSpecific.Name = "Model";
                        StringCollection objnamevalue = new StringCollection();
                        objnamevalue.Add(dr["ManufacturePartNo"].ToString());
                        objnameSpecific.Value = objnamevalue;
                        objCollmaterial.Add(objnameSpecific);
                    }
                    if (!string.IsNullOrEmpty(dr["Colors"].ToString()))
                    {
                        objSpecific = new NameValueListType();
                        objnameSpecific = new NameValueListType();
                        objnameSpecific.Name = "Color";
                        StringCollection objnamevalue = new StringCollection();
                        objnamevalue.Add(dr["Colors"].ToString());
                        objnameSpecific.Value = objnamevalue;
                        objCollmaterial.Add(objnameSpecific);
                    }



                    //objnameSpecific.Name = "Material";
                    //StringCollection objnamevalue = new StringCollection();
                    //objnamevalue.Add("100% Linen");
                    //objnameSpecific.Value = objnamevalue;

                    //
                    //objCollmaterial.Add(objnameSpecific);
                    if (objnameSpecific != null)
                    {
                        item.ItemSpecifics = objCollmaterial;
                    }

                    item.ItemID = dr["ProductID"].ToString();
                    item.SKU = dr["SKU"].ToString();

                    #region  Price and SalePrice and Bid Start Price

                    item.StartPrice = new AmountType();   //bid start price
                    item.StartPrice.Value = Convert.ToDouble(dr["saleprice1"].ToString());
                    item.StartPrice.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;

                    AmountType price = new AmountType();

                    price.Value = Convert.ToDouble(dr["saleprice1"].ToString());
                    price.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;

                    item.BuyItNowPrice = price;

                    #endregion

                    item.ConditionDisplayName = "New";
                    item.ConditionID = 1000;


                    #region Set Category

                    if (dr["EbayStoreCategoryID"] != null && dr["EbayStoreCategoryID"].ToString().Trim() != "")
                    {
                        string[] tmp = dr["EbayStoreCategoryID"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (tmp.Length > 0)
                        {

                            if (Convert.ToBoolean(Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString())) || Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString()) == 1)
                            {
                                StoreCategory1 = "3475288015";
                            }
                            else
                            {
                                StoreCategory1 = tmp[0].ToString();
                            }
                            //   StoreCategory1 = tmp[0].ToString();
                        }
                        if (tmp.Length > 1)
                            StoreCategory2 = tmp[1].ToString();
                    }

                    if (!string.IsNullOrEmpty(StoreCategory1))
                    {
                        item.Storefront = new StorefrontType();
                        item.Storefront.StoreCategoryID = Convert.ToInt64(StoreCategory1);
                        if (!string.IsNullOrEmpty(StoreCategory2))
                            item.Storefront.StoreCategory2ID = Convert.ToInt64(StoreCategory2);
                    }

                    //set category

                    if (dr["EbayCategoryID"] != null && dr["EbayCategoryID"].ToString().Trim() != "")
                    {
                        string[] tmp = dr["EbayCategoryID"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (tmp.Length > 0)
                        {
                            if (Convert.ToBoolean(Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString())) || Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString()) == 1)
                            {
                                Category1 = "155193";
                            }
                            else
                            {
                                Category1 = tmp[0].ToString();
                            }
                        }
                        if (tmp.Length > 1)
                            Category2 = tmp[1].ToString();
                    }

                    CategoryType ebayprimaycategory = new CategoryType();
                    ebayprimaycategory.CategoryID = Category1;
                    item.PrimaryCategory = ebayprimaycategory;

                    if (!string.IsNullOrEmpty(Category2))
                    {
                        CategoryType ebaysecondarycategory = new CategoryType();
                        ebaysecondarycategory.CategoryID = Category2;
                        item.PrimaryCategory = ebaysecondarycategory;
                    }

                    #endregion

                    Int32 Inventory = 0;
                    Int32.TryParse(dr["Inventory"].ToString(), out Inventory);

                    Inventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dr["UPC"].ToString() + "','" + dr["SKU"].ToString() + "'," + ddlStore.SelectedValue.ToString() + ")"));
                    if (Inventory > 10)
                    {
                        Inventory = 10;
                    }
                    item.Quantity = Inventory;
                    item.InventoryTrackingMethod = InventoryTrackingMethodCodeType.ItemID;

                    Int32 listingdays = 0;
                    if (dr["EbayListingDay"] != null && dr["EbayListingDay"].ToString().Trim() != "")
                    {
                        listingdays = Convert.ToInt32(dr["EbayListingDay"].ToString().Trim());
                    }


                    //item.LotSize = 0;
                    item.PrivateListing = false;
                    item.BuyerResponsibleForShipping = true;

                    #region Payment Options
                    item.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection();
                    item.PaymentMethods.AddRange(new BuyerPaymentMethodCodeType[] { 
                BuyerPaymentMethodCodeType.PayPal});
                    item.PayPalEmailAddress = AppLogic.AppConfigs("EBayPayPalEmailAddress");


                    #endregion

                    #region Insurance
                    item.ShippingDetails = new ShippingDetailsType();
                    //InsuranceDetailsType objinsurance = new InsuranceDetailsType();
                    //AmountType insuranceamt = new AmountType();
                    //insuranceamt.currencyID = CurrencyCodeType.USD;
                    //insuranceamt.Value = 234;
                    //objinsurance.InsuranceFee = insuranceamt;
                    //objinsurance.InsuranceOptionSpecified = true;
                    //item.ShippingDetails.InsuranceWanted = true;
                    //item.ShippingDetails.InsuranceDetails = objinsurance;

                    #endregion

                    #region Sale Tax

                    #endregion

                    item.Country = eBay.Service.Core.Soap.CountryCodeType.US;
                    item.GiftIcon = 0;

                    item.ListingDesigner = new ListingDesignerType();
                    item.ListingDesigner.ThemeID = 10;
                    item.ListingDesigner.LayoutID = 10000;

                    item.BestOfferDetails = new BestOfferDetailsType();
                    item.BestOfferDetails.BestOfferEnabled = false;
                    item.DispatchTimeMaxSpecified = false;

                    #region  Need Charity

                    #endregion


                    Decimal weight = 0;
                    Decimal.TryParse(dr["Weight"].ToString(), out weight);
                    if (weight == 0)
                        weight = 1;

                    #region Shipping Policy

                    Double saleprice = Convert.ToDouble(dr["saleprice1"].ToString());
                    item.ShippingDetails = new ShippingDetailsType();
                    item.ShippingDetails.ShippingServiceOptions = new ShippingServiceOptionsTypeCollection();
                    item.ShippingDetails.InternationalShippingServiceOption = new InternationalShippingServiceOptionsTypeCollection();

                    Int32 ShippingTimeMax = Convert.ToInt32(objAccess.ExecuteScalarQuery("SELECT Top 1 ConfigValue FROM tb_AppConfig WHERE Configname='ShippingTimeMax' AND StoreId=7 ANd isnull(deleted,0)=0"));
                    Int32 ShippingTimeMin = Convert.ToInt32(objAccess.ExecuteScalarQuery("SELECT Top 1 ConfigValue FROM tb_AppConfig WHERE Configname='ShippingTimeMin' AND StoreId=7 ANd isnull(deleted,0)=0"));


                    if (saleprice > 58.99 && saleprice < 9999999)
                    {
                        ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[1];
                        opt[0] = new ShippingServiceOptionsType();
                        opt[0].ShippingServiceCost = new AmountType();
                        opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[0].ShippingServiceCost.Value = 0;
                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[0].ShippingService = "UPSGround";
                        opt[0].ShippingTimeMax = ShippingTimeMax;

                        opt[0].ShippingTimeMin = ShippingTimeMin;
                        opt[0].ShippingTimeMaxSpecified = true;
                        opt[0].ShippingTimeMinSpecified = true;
                        if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                        {
                            opt[0].FreeShipping = true;
                            opt[0].FreeShippingSpecified = true;
                        }
                        opt[0].ShippingServiceAdditionalCost = new AmountType();
                        opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[0].ShippingServiceAdditionalCost.Value = 0;

                        item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);

                    }
                    if (saleprice < 59)
                    {
                        ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[4];
                        opt[0] = new ShippingServiceOptionsType();
                        opt[0].ShippingServiceCost = new AmountType();
                        opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;

                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[0].ShippingService = "UPSGround";
                        opt[0].ShippingTimeMax = ShippingTimeMax;
                        opt[0].ShippingTimeMin = ShippingTimeMin;
                        opt[0].ShippingTimeMaxSpecified = true;
                        opt[0].ShippingTimeMinSpecified = true;
                        if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                        {
                            opt[0].ShippingServiceCost.Value = 0;//6.95;
                            opt[0].FreeShipping = true;
                            opt[0].FreeShippingSpecified = true;
                        }
                        else
                        {
                            opt[0].ShippingServiceCost.Value = 3.95;//6.95;
                        }
                        opt[0].ShippingServiceAdditionalCost = new AmountType();
                        opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[0].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);


                        opt[1] = new ShippingServiceOptionsType();
                        opt[1].ShippingServiceCost = new AmountType();
                        opt[1].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[1].ShippingServiceCost.Value = 15.95;

                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[1].ShippingService = "UPS3rdDay"; //3 day select
                        opt[1].ShippingTimeMax = 3;
                        opt[1].ShippingTimeMin = 1;
                        opt[1].ShippingServiceAdditionalCost = new AmountType();
                        opt[1].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[1].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[1]);

                        opt[2] = new ShippingServiceOptionsType();
                        opt[2].ShippingServiceCost = new AmountType();
                        opt[2].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[2].ShippingServiceCost.Value = 20.95;

                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[2].ShippingService = "UPS2ndDay"; //3 day select
                        opt[2].ShippingTimeMax = 2;
                        opt[2].ShippingTimeMin = 1;
                        opt[2].ShippingServiceAdditionalCost = new AmountType();
                        opt[2].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[2].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[2]);



                        opt[3] = new ShippingServiceOptionsType();
                        opt[3].ShippingServiceCost = new AmountType();
                        opt[3].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[3].ShippingServiceCost.Value = 30.95;

                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[3].ShippingService = "UPSNextDay"; //3 day select
                        opt[3].ShippingTimeMax = 1;
                        opt[3].ShippingTimeMin = 1;
                        opt[3].ShippingServiceAdditionalCost = new AmountType();
                        opt[3].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[3].ShippingServiceAdditionalCost.Value = 0;

                        //opt[3].ShippingServicePriority = 4;
                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[3]);


                    }
                    else if (saleprice < 149)
                    {
                        ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[2];
                        opt[0] = new ShippingServiceOptionsType();
                        opt[0].ShippingServiceCost = new AmountType();
                        opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;


                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[0].ShippingService = "UPS3rdDay"; //3 day select
                        opt[0].ShippingTimeMax = 3;
                        opt[0].ShippingTimeMin = 1;
                        if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                        {
                            opt[0].FreeShipping = true;
                            opt[0].ShippingServiceCost.Value = 0;
                        }
                        else
                        {
                            opt[0].ShippingServiceCost.Value = 20.95;
                        }
                        opt[0].ShippingServiceAdditionalCost = new AmountType();
                        opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[0].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);

                        opt[1] = new ShippingServiceOptionsType();
                        opt[1].ShippingServiceCost = new AmountType();
                        opt[1].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[1].ShippingServiceCost.Value = 30.95;

                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[1].ShippingService = "UPS2ndDay"; //3 day select
                        opt[1].ShippingTimeMax = 2;
                        opt[1].ShippingTimeMin = 1;
                        opt[1].ShippingServiceAdditionalCost = new AmountType();
                        opt[1].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[1].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[1]);
                    }
                    else if (saleprice < 350)
                    {
                        ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[2];
                        opt[0] = new ShippingServiceOptionsType();
                        opt[0].ShippingServiceCost = new AmountType();
                        opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;


                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[0].ShippingService = "UPS3rdDay"; //3 day select
                        opt[0].ShippingTimeMax = 3;
                        opt[0].ShippingTimeMin = 1;
                        if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                        {
                            opt[0].FreeShipping = true;
                            opt[0].ShippingServiceCost.Value = 0;
                        }
                        else
                        {
                            opt[0].ShippingServiceCost.Value = 25.95;
                        }
                        opt[0].ShippingServiceAdditionalCost = new AmountType();
                        opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[0].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);

                        opt[1] = new ShippingServiceOptionsType();
                        opt[1].ShippingServiceCost = new AmountType();
                        opt[1].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[1].ShippingServiceCost.Value = 35.95;

                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[1].ShippingService = "UPS2ndDay"; //3 day select
                        opt[1].ShippingTimeMax = 2;
                        opt[1].ShippingTimeMin = 1;
                        opt[1].ShippingServiceAdditionalCost = new AmountType();
                        opt[1].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[1].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[1]);
                    }
                    else
                    {
                        ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[2];
                        opt[0] = new ShippingServiceOptionsType();
                        opt[0].ShippingServiceCost = new AmountType();
                        opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;


                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[0].ShippingService = "UPS3rdDay"; //3 day select
                        opt[0].ShippingTimeMax = 3;
                        opt[0].ShippingTimeMin = 1;
                        if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                        {
                            opt[0].FreeShipping = true;
                            opt[0].ShippingServiceCost.Value = 0;
                        }
                        else
                        {
                            opt[0].ShippingServiceCost.Value = 29.95;
                        }
                        opt[0].ShippingServiceAdditionalCost = new AmountType();
                        opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[0].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);

                        opt[1] = new ShippingServiceOptionsType();
                        opt[1].ShippingServiceCost = new AmountType();
                        opt[1].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[1].ShippingServiceCost.Value = 39.95;

                        // ShippingService is now a string
                        //Make a call to GeteBayDetails to find out the valid Shipping Service values
                        opt[1].ShippingService = "UPS2ndDay"; //3 day select
                        opt[1].ShippingTimeMax = 2;
                        opt[1].ShippingTimeMin = 1;
                        opt[1].ShippingServiceAdditionalCost = new AmountType();
                        opt[1].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                        opt[1].ShippingServiceAdditionalCost.Value = 0;

                        if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                            item.ShippingDetails.ShippingServiceOptions.Add(opt[1]);
                    }

                    item.ShippingDetails.ShippingType = ShippingTypeCodeType.Flat;

                    #endregion

                    item.Location = "US";

                    item.UUID = System.Guid.NewGuid().ToString().Replace("-", "");

                    #region Variant


                    DataSet dsEbayVar = new DataSet();
                    DataSet dsEbayVariant = new DataSet();

                    dsEbayVariant = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + dr["ProductID"].ToString() + " Order By Displayorder");

                    bool flag = false;
                    int Totalqty = 0;
                    if (dsEbayVariant.Tables[0].Rows.Count > 0)
                    {
                        item.Variations = new VariationsType();
                        item.Variations.VariationSpecificsSet = new NameValueListTypeCollection();
                        Int32 i1 = 0;
                        Int32 i2 = 0;
                        string strvariant = "10,";
                        for (int l = 0; l < dsEbayVariant.Tables[0].Rows.Count; l++)
                        {
                            dsEbayVar = CommonComponent.GetCommonDataSet("SELECT * FROM Get_Variant_Details WHERE ProductID=" + dr["ProductID"].ToString() + " AND VariantID=" + dsEbayVariant.Tables[0].Rows[l]["VariantID"].ToString() + " Order By VariantName");
                            if (dsEbayVar.Tables[0].Rows.Count > 0)
                            {
                                NameValueListType NVListVS2 = new NameValueListType();
                                NVListVS2.Name = dsEbayVariant.Tables[0].Rows[l]["VariantName"].ToString();
                                StringCollection VSvaluecollection2 = new StringCollection();
                                string strvalue = "";
                                String[] Colour = new String[dsEbayVar.Tables[0].Rows.Count];
                                for (int j = 0; j < dsEbayVar.Tables[0].Rows.Count; j++)
                                {
                                    //strvalue += "" + dsEbayVar.Tables[0].Rows[j]["VariantValue"].ToString() + "",";
                                    Colour[j] = dsEbayVar.Tables[0].Rows[j]["VariantValue"].ToString();
                                }
                                if (dsEbayVar.Tables[0].Rows.Count > i1)
                                {
                                    i1 = Convert.ToInt32(dsEbayVar.Tables[0].Rows.Count);
                                    i2 = Convert.ToInt32(dsEbayVariant.Tables[0].Rows[l]["VariantID"].ToString());
                                }
                                // strvalue = strvalue.Substring(0, strvalue.Length - 1);

                                VSvaluecollection2.AddRange(Colour);
                                NVListVS2.Value = VSvaluecollection2;
                                item.Variations.VariationSpecificsSet.Add(NVListVS2);
                                strvariant += dsEbayVariant.Tables[0].Rows[l]["VariantID"].ToString() + ",";
                            }
                        }
                        if (strvariant.Length > 0)
                        {
                            strvariant = strvariant.Substring(0, strvariant.Length - 1);
                        }
                        else
                        {
                            strvariant = "0";
                        }

                        VariationTypeCollection VarCol = new VariationTypeCollection();
                        NameValueListTypeCollection objColl = new NameValueListTypeCollection();


                        DataSet dsEbayVarmain = new DataSet();
                        dsEbayVarmain = CommonComponent.GetCommonDataSet("SELECT * FROM Get_Variant_Details WHERE ProductID=" + dr["ProductID"].ToString() + " AND VariantID=" + i2.ToString() + " Order By VariantName");
                        if (dsEbayVarmain.Tables[0].Rows.Count > 0)
                        {
                            DataSet dsEbayVarmainchild = new DataSet();
                            dsEbayVarmainchild = CommonComponent.GetCommonDataSet("SELECT * FROM Get_Variant_Details WHERE ProductID=" + dr["ProductID"].ToString() + " AND VariantID <> " + i2.ToString() + " AND  isnull(VariantID,0) in (" + strvariant + ") Order By VariantName");
                            if (dsEbayVarmainchild.Tables[0].Rows.Count > 0)
                            {
                                for (int k = 0; k < dsEbayVarmainchild.Tables[0].Rows.Count; k++)
                                {
                                    for (int j = 0; j < dsEbayVarmain.Tables[0].Rows.Count; j++)
                                    {

                                        VariationType var1 = new VariationType();
                                        NameValueListType Var1Spec2 = new NameValueListType();
                                        StringCollection Var1Spec2Valuecoll = new StringCollection();
                                        Var1Spec2.Name = dsEbayVarmain.Tables[0].Rows[j]["VariantName"].ToString();

                                        Var1Spec2.SourceSpecified = true;
                                        //var1.Delete = true;

                                        if (!string.IsNullOrEmpty(dsEbayVarmain.Tables[0].Rows[j]["SKU"].ToString()))
                                        {
                                            var1.SKU = dsEbayVarmain.Tables[0].Rows[j]["SKU"].ToString();//dr["SKU"].ToString() + "-" + dsEbayVarmain.Tables[0].Rows[j]["VariantValue"].ToString().Replace("\"", "").Replace("'", "").Replace(" ", "-") + "-" + dsEbayVarmainchild.Tables[0].Rows[k]["VariantValue"].ToString();
                                        }
                                        else
                                        {
                                            var1.SKU = dr["SKU"].ToString() + "-" + dsEbayVarmain.Tables[0].Rows[j]["VariantValue"].ToString().Replace("\"", "").Replace("'", "").Replace(" ", "-") + "-" + dsEbayVarmainchild.Tables[0].Rows[k]["VariantValue"].ToString();
                                        }
                                        var1.Quantity = Convert.ToInt32(dsEbayVarmain.Tables[0].Rows[j]["Inventory"].ToString());
                                        Totalqty += Convert.ToInt32(dsEbayVarmain.Tables[0].Rows[j]["Inventory"].ToString());
                                        var1.StartPrice = new AmountType();
                                        var1.StartPrice.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                                        double totalprice = Convert.ToDouble(dr["saleprice1"].ToString());
                                        totalprice = totalprice + Convert.ToDouble(dsEbayVarmain.Tables[0].Rows[j]["VariantPrice"].ToString());
                                        var1.StartPrice.Value = totalprice;
                                        var1.VariationSpecifics = new NameValueListTypeCollection();

                                        Var1Spec2Valuecoll.Add(dsEbayVarmain.Tables[0].Rows[j]["VariantValue"].ToString());
                                        Var1Spec2.Value = Var1Spec2Valuecoll;

                                        var1.VariationSpecifics.Add(Var1Spec2);

                                        NameValueListType Var2Spec1 = new NameValueListType();
                                        StringCollection Var2Spec1Valuecoll = new StringCollection();

                                        Var2Spec1.Name = dsEbayVarmainchild.Tables[0].Rows[k]["VariantName"].ToString();
                                        Var2Spec1Valuecoll.Add(dsEbayVarmainchild.Tables[0].Rows[k]["VariantValue"].ToString());
                                        Var2Spec1.Value = Var2Spec1Valuecoll;
                                        var1.VariationSpecifics.Add(Var2Spec1);
                                        VarCol.Add(var1);

                                    }

                                }
                            }
                        }

                        //item.Variations.VariationSpecificsSet = objColl;
                        item.Variations.Variation = VarCol;
                        item.Quantity = Totalqty;
                    }

                    #endregion


                    #region Listing images

                    #region add images from live
                    item.PictureDetails = new PictureDetailsType();
                    item.PictureDetails.PhotoDisplaySpecified = true;
                    item.PictureDetails.PhotoDisplay = PhotoDisplayCodeType.SuperSize;
                    item.PictureDetails.PictureURL = new StringCollection();

                    item.PictureDetails.PictureURL.Add(Live_Server + GetMediumImage(ImageServer + "product/large/" + dr["ImageName"].ToString()));
                    item.PictureDetails.GalleryType = GalleryTypeCodeType.Gallery;

                    ApiCall obj = new ApiCall();

                    for (int i = 1; i < 26; i++)
                        if (System.IO.File.Exists(Server.MapPath(ImageServer + "Product/large/" + dr["ImageName"].ToString().Replace(".jpeg", "").Replace(".jpg", "") + "_" + i.ToString() + ".jpg")) == true)
                        {
                            //Response.Write(Live_Server.ToString() + ImageServer + "product/Medium/" + dr["ImageName"].ToString().Replace(".jpeg", "").Replace(".jpg", "") + "_" + i.ToString() + ".jpg");
                            item.PictureDetails.PictureURL.Add(Live_Server + GetMediumImage(ImageServer + "product/large/" + dr["ImageName"].ToString().Replace(".jpeg", "").Replace(".jpg", "") + "_" + i.ToString() + ".jpg"));
                        }
                    #endregion


                    #endregion


                    item.ListingDetails = new ListingDetailsType();

                    if (listingdays == 0)
                    {
                        item.ListingDuration = "GTC";
                    }
                    else
                        item.ListingDuration = "GTC";


                    item.DispatchTimeMax = 1;
                    ReviseFixedPriceItemCall reviceitem = null;
                    item.ListingType = ListingTypeCodeType.FixedPriceItem;
                    DataSet dsebay = new DataSet();
                    dsebay = dbAccess.GetDs("select ebayLastUpdated,EbayProductID from tb_Product where ProductID=" + dr["ProductID"].ToString());
                    try
                    {
                        if (string.IsNullOrEmpty(dsebay.Tables[0].Rows[0]["EbayProductID"].ToString().Trim()))
                        {
                            AddFixedPriceItemCall fixedpriceitem = new AddFixedPriceItemCall(GetContext());

                            FeeTypeCollection fees = fixedpriceitem.AddFixedPriceItem(item);
                            double decTotalFee = 0;
                            foreach (FeeType fee in fees)
                                decTotalFee += fee.Fee.Value;
                            dbAccess.ExecuteNonQuery("update tb_Product set EbayProductID='" + item.ItemID + "',EbayCreated='" + DateTime.Now + "',EbayLastUpdated='" + DateTime.Now + "',EbayListingFee=" + decTotalFee + " where productid=" + dr["ProductID"].ToString());
                            insertcnt++;

                        }
                        else
                        {

                            item.ItemID = dsebay.Tables[0].Rows[0]["EbayProductID"].ToString().Trim();

                            DateTime dtLastUpdated = Convert.ToDateTime(dsebay.Tables[0].Rows[0]["EbayLastUpdated"].ToString());
                            if (dtLastUpdated.AddDays(listingdays) < DateTime.Today && listingdays > 0)
                            {
                                RelistFixedPriceItemCall relistitem = new RelistFixedPriceItemCall(GetContext());

                                FeeTypeCollection fees = relistitem.RelistFixedPriceItem(item, null);

                                double decTotalFee = 0;
                                foreach (FeeType fee in fees)
                                    decTotalFee += fee.Fee.Value;
                                dbAccess.ExecuteNonQuery("update tb_Product set EbayProductID='" + item.ItemID + "',EbayListingFee=EbayListingFee+" + decTotalFee + ",EbayLastUpdated='" + DateTime.Now + "',EbayCreated='" + DateTime.Now + "' where productid=" + dr["ProductID"].ToString());
                                relistcnt++;
                            }
                            else
                            {

                                reviceitem = new ReviseFixedPriceItemCall(GetContext());
                                reviceitem.Item = item;
                                reviceitem.Execute();

                                FeeTypeCollection fees = reviceitem.FeeList;
                                //double decTotalFee = 0;
                                //foreach (FeeType fee in fees)
                                //    decTotalFee += fee.Fee.Value;
                                dbAccess.ExecuteNonQuery("update tb_Product set EbayLastUpdated='" + DateTime.Now + "' where productid=" + dr["ProductID"].ToString());
                                //revise your product in ebay...
                                revicecnt++;
                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text = ex.Message.ToString();
                        lblMsg.Visible = true;
                        trMsg.Attributes.Add("style", "display:'';");
                        return;
                    }
                }
            }
            if (lblMsg.Text.ToString() == "")
            {
                lblMsg.Text = insertcnt + " Number of Products Listed. <br/>" + revicecnt + " Number of Products Revised. <br/>" + relistcnt + " Number of Products Re-Listed.";
                lblMsg.Visible = true;
                trMsg.Attributes.Add("style", "display:'';");
            }
        }

        /// <summary>
        /// Create or Update eBay Products as a Auction
        /// </summary>
        /// <param name="dt">DataTable dt</param>
        public void createorupdateebayproductsAuction(DataTable dt)
        {
            SQLAccess objAccess = new SQLAccess();

            string strHtml = string.Empty;
            string strProductPath = string.Empty;
            if (ddlStore.SelectedValue != null)
            {
                strHtml = "ProductHtml-" + ddlStore.SelectedValue.ToString() + ".htm";
                Image_Path = "/ebay/images/";
                strProductPath = AppLogic.AppConfigs("ImagePathProduct").Trim().ToLower().Replace("product/", "");
            }

            Live_Server = AppLogic.AppConfigs("Live_Server_Product");
            storeID = AppConfig.StoreID;

            SQLAccess dbAccess = new SQLAccess();
            string strResponse = "";
            string strResponseoriginal = "";

            System.Net.WebClient Client = new System.Net.WebClient();


            Stream strm = Client.OpenRead(Server.MapPath(strHtml)); //Server.MapPath("ProductHtml.htm")
            StreamReader sr = new StreamReader(strm);
            string strResponse1 = "";
            while ((strResponse1 = sr.ReadLine()) != null)
            {
                strResponse += strResponse1;
            }
            strResponseoriginal = strResponse.ToString();
            sr.Close();

            Int32 insertcnt = 0, revicecnt = 0, relistcnt = 0;

            string PrivacyPolicy = "";
            string ShippingPolicy = "";
            string AboutUs = "";
            string ReturnPolicy = "";

            string ReturnProcess = "";
            string RefundTimeFrame = "";
            string MailThePackageTo = "";
            string ChangesToThePolicy = "";

            PrivacyPolicy = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND  TopicName='PrivacyPolicyEbay'"));
            ShippingPolicy = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND  TopicName='ShippingEbay'"));
            AboutUs = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND  TopicName='AboutUsEbay'"));
            ReturnPolicy = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from tb_Topic  where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='EasyReturnsEbay'"));

            ReturnProcess = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='ReturnProcess'"));
            RefundTimeFrame = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='RefundTimeFrame'"));
            MailThePackageTo = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='MailThePackageTo'"));
            ChangesToThePolicy = Convert.ToString(objAccess.ExecuteScalarQuery("Select Top 1 Description from dbo.tb_Topic where Storeid=" + ddlStore.SelectedValue.ToString() + " AND TopicName='ChangesToThePolicy'"));


            if (string.IsNullOrEmpty(PrivacyPolicy))
                PrivacyPolicy = "Coming Soon";
            if (string.IsNullOrEmpty(ShippingPolicy))
                ShippingPolicy = "Coming Soon";
            if (string.IsNullOrEmpty(AboutUs))
                AboutUs = "Coming Soon";
            if (string.IsNullOrEmpty(ReturnPolicy))
                ReturnPolicy = "Coming Soon";

            if (string.IsNullOrEmpty(ReturnProcess))
                ReturnProcess = "Coming Soon";
            if (string.IsNullOrEmpty(RefundTimeFrame))
                RefundTimeFrame = "Coming Soon";
            if (string.IsNullOrEmpty(MailThePackageTo))
                MailThePackageTo = "Coming Soon";
            if (string.IsNullOrEmpty(ChangesToThePolicy))
                ChangesToThePolicy = "Coming Soon";

            foreach (DataRow dr in dt.Rows)
            {
                String Category1 = "";
                String Category2 = "";
                String StoreCategory1 = "";
                String StoreCategory2 = "";

                strResponse = strResponseoriginal;

                strResponse = strResponse.Replace("/ebay/", Live_Server + "/ebay/");
                //strResponse = strResponse.Replace("/ebay/", Live_Server + "/");

                string strReplace = "";
                if (dr["Description"] != null)
                    strReplace = dr["Description"].ToString();

                strReplace = strReplace.Replace(strProductPath + "product/", Live_Server + strProductPath + "product/");

                strResponse = strResponse.ToString().Replace("###shippingpolicy###", ShippingPolicy.ToString());
                strResponse = strResponse.ToString().Replace("###privacypolicy###", PrivacyPolicy.ToString());
                strResponse = strResponse.ToString().Replace("###aboutus###", AboutUs.ToString());
                strResponse = strResponse.ToString().Replace("###returnpolicy###", ReturnPolicy.ToString());

                strResponse = strResponse.ToString().Replace("###returnprocess###", ReturnProcess.ToString());
                strResponse = strResponse.ToString().Replace("###refundtimeframe###", RefundTimeFrame.ToString());
                strResponse = strResponse.ToString().Replace("###mailthepackageto###", MailThePackageTo.ToString());
                strResponse = strResponse.ToString().Replace("###changestothepolicy###", ChangesToThePolicy.ToString());


                string Availability = "InStock";
                if (dr["Avail"] != null && dr["Avail"].ToString().Trim() != "")
                    Availability = dr["Avail"].ToString();
                strResponse = strResponse.ToString().Replace("###avilability###", Availability.ToString());

                strResponse = strResponse.ToString().Replace("###Description###", System.Text.RegularExpressions.Regex.Replace(strReplace.ToString(), @"<[^>]*>", string.Empty));
                strResponse = strResponse.ToString().Replace("###Features###", Convert.ToString(dr["Features"]));
                //strResponse = strResponse.ToString().Replace(Live_Server + "/Client/css/stylenew.css", Live_Server + "/ebay/css/style-ebay.css"); // Commented - By Girish
                string warranty = "";
                if (dr["extended-warranty"] != null)
                    warranty = dr["extended-warranty"].ToString();

                strResponse = strResponse.ToString().Replace("###warranty###", warranty.Replace(strProductPath + "product", Live_Server + strProductPath + "product"));
                strResponse = strResponse.Replace("###name###", dr["Name"].ToString());
                strResponse = strResponse.Replace("###id###", dr["productID"].ToString());
                if ((dr["SKU"].ToString() != null) && (dr["SKU"].ToString() != ""))
                {
                    strResponse = strResponse.Replace("###code###", dr["SKU"].ToString());
                }
                else
                {
                    strResponse = strResponse.Replace("###code###", "-");
                }
                strResponse = strResponse.Replace("###price###", Convert.ToString(Convert.ToDouble(dr["Price1"].ToString()).ToString("N2")));
                strResponse = strResponse.Replace("###saleprice###", Convert.ToString(Convert.ToDouble(dr["SalePrice1"].ToString()).ToString("N2")));

                strResponse = strResponse.Replace("###yousave###", (Convert.ToDouble(Convert.ToDouble(dr["Price1"].ToString()) - Convert.ToDouble(dr["SalePrice1"].ToString())).ToString("f2")).ToString() + " (" + String.Format("{0:0}", (((Convert.ToDouble(dr["Price1"].ToString()) - Convert.ToDouble(dr["SalePrice1"].ToString())) / Convert.ToDouble(dr["Price1"].ToString())) * 100)) + "%)");

                DataTable dtRating = new DataTable();
                Decimal AvgRating = 0;
                Int32 TotalReviews = 0;
                dtRating = dbAccess.GetDs("select isnull(avg(isnull(rating,0)),0) as AvgRating,isnull(Count(1),0) as TotalReviews from tb_Rating where ProductID=" + dr["ProductID"].ToString()).Tables[0];
                AvgRating = Convert.ToDecimal(dtRating.Rows[0]["AvgRating"].ToString());
                TotalReviews = Convert.ToInt32(dtRating.Rows[0]["TotalReviews"].ToString());
                if (AvgRating == 0)
                    strResponse = strResponse.Replace("###rating###", "");
                else if (TotalReviews == 1)
                    strResponse = strResponse.Replace("###rating###", "<div class=\"item-right-row\"><p>Product Rating </p>:<span>" + BindStarsImage(AvgRating) + "(" + TotalReviews + " Review)</span> </div>");
                else
                    strResponse = strResponse.Replace("###rating###", "<div class=\"item-right-row\"><p>Product Rating </p>:<span>" + BindStarsImage(AvgRating) + "(" + TotalReviews + " Reviews)</span> </div>");

                string ImageServer = string.Empty;

                if (ddlStore.SelectedValue.ToString() == "7") // here 7 is ebay storeid
                {
                    ImageServer = strProductPath;
                }


                strResponse = strResponse.Replace("###img1###", Live_Server + GetMediumImage(ImageServer + "product/Medium/" + dr["ImageName"].ToString()) + "");
                string strImages = "";

                //Getting icon and medium images for image scrolller
                strImages = strImages + "<li title='" + dr["Name"].ToString() + "'>";


                string strMoreImageName = string.Empty;
                strMoreImageName = dr["ImageName"].ToString().Replace(".jpeg", "").Replace(".jpg", "");

                strImages = strImages + "<img oncontextmenu='return false;' src='" + Live_Server + GetMicroImage(ImageServer + "product/Micro/" + dr["ImageName"].ToString()) + "' alt='" + dr["Name"].ToString() + "' title='" + dr["Name"].ToString() + "' onmouseover='javascript:document.getElementById(&quot;ProductPic" + dr["productID"].ToString() + "&quot;).src=&quot;" + Live_Server + GetMediumImage(ImageServer + "product/Medium/" + dr["ImageName"].ToString()) + "&quot;;currentID=this.parentNode.id;'></li>";
                for (int i = 1; i < 26; i++)
                {

                    if (System.IO.File.Exists(Server.MapPath(ImageServer + "Product/Micro/" + strMoreImageName + "_" + i.ToString() + "_.jpg")) == true)
                    {
                        strImages = strImages + "<li  title='" + dr["Name"].ToString() + "' style='float:left;'>";
                        strImages = strImages + "<img oncontextmenu='return false;' src='" + Live_Server + ImageServer + "product/Micro/" + strMoreImageName + "_" + i.ToString() + ".jpg' alt='" + dr["Name"].ToString() + "' title='" + dr["Name"].ToString() + "' onmouseover='javascript:document.getElementById(&quot;ProductPic" + dr["productID"].ToString() + "&quot;).src=&quot;" + Live_Server + GetMediumImage(ImageServer + "product/Medium/" + strMoreImageName.ToString() + "_" + i.ToString() + ".jpg") + "&quot;;currentID=this.parentNode.id;'></li>";
                    }
                }

                strResponse = strResponse.Replace("###images###", strImages.ToString());
                strResponse = strResponse.Replace("\t", "");
                strResponse = strResponse.Replace("\" />", "\"/>");
                strResponse = strResponse.Replace("\" >", "\">");

                string Description = strResponse.ToString().Replace("\r\n", "");


                ItemType item = new ItemType();

                item.Currency = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                item.Country = eBay.Service.Core.Soap.CountryCodeType.US;
                item.Site = SiteCodeType.US;

                string pname = dr["Name"].ToString();
                if (dr["Name"].ToString().Length > 55)
                    pname = dr["Name"].ToString().Substring(0, 51) + "...";

                item.Title = pname;

                //if (dr["Name"].ToString().Length > 55)
                //    item.SubTitle = dr["Name"].ToString();

                item.Description = Description;

                string returnpolicy = "";

                if (dr["extended-warranty"] != null)
                    returnpolicy = dr["extended-warranty"].ToString().Replace(strProductPath + "product", Live_Server + ImageServer + "product");


                ReturnPolicyType po = new ReturnPolicyType();
                po.WarrantyOffered = returnpolicy;
                po.ReturnsAcceptedOption = "ReturnsAccepted";
                po.RefundOption = "MoneyBackOrExchange"; //MoneybackOr
                po.ReturnsWithinOption = "Days_30";
                po.Description = " ";
                po.ShippingCostPaidByOption = "Buyer";
                item.ReturnPolicy = po;

                NameValueListType objSpecific = new NameValueListType();
                NameValueListTypeCollection objCollmaterial = new NameValueListTypeCollection();
                NameValueListType objnameSpecific = new NameValueListType();
                if (!string.IsNullOrEmpty(dr["ProductSummary"].ToString()))
                {
                    objSpecific = new NameValueListType();
                    objnameSpecific = new NameValueListType();
                    objnameSpecific.Name = "Type";
                    StringCollection objnamevalue = new StringCollection();
                    objnamevalue.Add(dr["ProductSummary"].ToString());
                    objnameSpecific.Value = objnamevalue;
                    objCollmaterial.Add(objnameSpecific);
                }
                if (!string.IsNullOrEmpty(dr["Materials"].ToString()))
                {
                    objSpecific = new NameValueListType();
                    objnameSpecific = new NameValueListType();
                    objnameSpecific.Name = "Material";
                    StringCollection objnamevalue = new StringCollection();
                    objnamevalue.Add(dr["Materials"].ToString());
                    objnameSpecific.Value = objnamevalue;
                    objCollmaterial.Add(objnameSpecific);
                }
                if (!string.IsNullOrEmpty(dr["Brand"].ToString()))
                {
                    objSpecific = new NameValueListType();
                    objnameSpecific = new NameValueListType();
                    objnameSpecific.Name = "Brand";
                    StringCollection objnamevalue = new StringCollection();
                    objnamevalue.Add(dr["Brand"].ToString());
                    objnameSpecific.Value = objnamevalue;
                    objCollmaterial.Add(objnameSpecific);
                }
                if (!string.IsNullOrEmpty(dr["ManufacturePartNo"].ToString()))
                {
                    objSpecific = new NameValueListType();
                    objnameSpecific = new NameValueListType();
                    objnameSpecific.Name = "Model";
                    StringCollection objnamevalue = new StringCollection();
                    objnamevalue.Add(dr["ManufacturePartNo"].ToString());
                    objnameSpecific.Value = objnamevalue;
                    objCollmaterial.Add(objnameSpecific);
                }
                if (!string.IsNullOrEmpty(dr["Colors"].ToString()))
                {
                    objSpecific = new NameValueListType();
                    objnameSpecific = new NameValueListType();
                    objnameSpecific.Name = "Color";
                    StringCollection objnamevalue = new StringCollection();
                    objnamevalue.Add(dr["Colors"].ToString());
                    objnameSpecific.Value = objnamevalue;
                    objCollmaterial.Add(objnameSpecific);
                }


                if (objnameSpecific != null)
                {
                    item.ItemSpecifics = objCollmaterial;
                }

                item.ItemID = dr["ProductID"].ToString();
                item.SKU = dr["SKU"].ToString();

                #region  Price and SalePrice and Bid Start Price

                item.StartPrice = new AmountType();   //bid start price
                item.StartPrice.Value = Convert.ToDouble(dr["saleprice1"].ToString());
                item.StartPrice.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;

                AmountType price = new AmountType();
                price.Value = Convert.ToDouble(dr["price"].ToString());
                price.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                item.BuyItNowPrice = price;

                #endregion

                item.ConditionDisplayName = "New";
                item.ConditionID = 1000;


                #region Set Category

                if (dr["EbayStoreCategoryID"] != null && dr["EbayStoreCategoryID"].ToString().Trim() != "")
                {
                    string[] tmp = dr["EbayStoreCategoryID"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length > 0)
                    {
                        if (Convert.ToBoolean(Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString())) || Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString()) == 1)
                        {
                            StoreCategory1 = "3475288015";
                        }
                        else
                        {
                            StoreCategory1 = tmp[0].ToString();
                        }
                    }
                    if (tmp.Length > 1)
                        StoreCategory2 = tmp[1].ToString();
                }

                if (!string.IsNullOrEmpty(StoreCategory1))
                {
                    item.Storefront = new StorefrontType();
                    item.Storefront.StoreCategoryID = Convert.ToInt64(StoreCategory1);
                    if (!string.IsNullOrEmpty(StoreCategory2))
                        item.Storefront.StoreCategory2ID = Convert.ToInt64(StoreCategory2);
                }


                //set category

                if (dr["EbayCategoryID"] != null && dr["EbayCategoryID"].ToString().Trim() != "")
                {
                    string[] tmp = dr["EbayCategoryID"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (tmp.Length > 0)
                    {
                        if (Convert.ToBoolean(Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString())) || Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString()) == 1)
                        {
                            Category1 = "155193";
                        }
                        else
                        {
                            Category1 = tmp[0].ToString();
                        }
                    }

                    if (tmp.Length > 1)
                        Category2 = tmp[1].ToString();
                }

                CategoryType ebayprimaycategory = new CategoryType();
                ebayprimaycategory.CategoryID = Category1;
                item.PrimaryCategory = ebayprimaycategory;

                if (!string.IsNullOrEmpty(Category2))
                {
                    CategoryType ebaysecondarycategory = new CategoryType();
                    ebaysecondarycategory.CategoryID = Category2;
                    item.PrimaryCategory = ebaysecondarycategory;
                }

                #endregion

                Int32 Inventory = 0;
                Int32.TryParse(dr["Inventory"].ToString(), out Inventory);
                item.Quantity = Inventory;
                item.InventoryTrackingMethod = InventoryTrackingMethodCodeType.ItemID;

                item.PrivateListing = false;
                item.BuyerResponsibleForShipping = true;

                #region Payment Options
                item.PaymentMethods = new BuyerPaymentMethodCodeTypeCollection();
                item.PaymentMethods.AddRange(new BuyerPaymentMethodCodeType[] { 
                BuyerPaymentMethodCodeType.PayPal});
                item.PayPalEmailAddress = AppLogic.AppConfigs("EBayPayPalEmailAddress");


                #endregion

                #region Insurance
                item.ShippingDetails = new ShippingDetailsType();

                #endregion

                #region Sale Tax

                #endregion

                item.Country = eBay.Service.Core.Soap.CountryCodeType.US;
                item.GiftIcon = 0;

                item.ListingDesigner = new ListingDesignerType();
                item.ListingDesigner.ThemeID = 10;
                item.ListingDesigner.LayoutID = 10000;

                item.BestOfferDetails = new BestOfferDetailsType();
                item.BestOfferDetails.BestOfferEnabled = false;
                item.DispatchTimeMaxSpecified = false;

                #region  Need Charity

                #endregion


                Decimal weight = 0;
                Decimal.TryParse(dr["Weight"].ToString(), out weight);
                if (weight == 0)
                    weight = 1;

                #region Shipping Policy


                Double saleprice = Convert.ToDouble(dr["saleprice1"].ToString());
                item.ShippingDetails = new ShippingDetailsType();
                item.ShippingDetails.ShippingServiceOptions = new ShippingServiceOptionsTypeCollection();
                item.ShippingDetails.InternationalShippingServiceOption = new InternationalShippingServiceOptionsTypeCollection();

                if (saleprice > 58.99 && saleprice < 9999999)
                {
                    ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[1];
                    opt[0] = new ShippingServiceOptionsType();
                    opt[0].ShippingServiceCost = new AmountType();
                    opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[0].ShippingServiceCost.Value = 0;
                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[0].ShippingService = "UPSGround";
                    opt[0].ShippingTimeMax = 5;
                    opt[0].ShippingTimeMin = 2;
                    if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                    {
                        opt[0].FreeShipping = true;
                        opt[0].FreeShippingSpecified = true;
                    }
                    opt[0].ShippingServiceAdditionalCost = new AmountType();
                    opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[0].ShippingServiceAdditionalCost.Value = 0;

                    // opt[0].ShippingServicePriority = 1;

                    item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);

                }
                if (saleprice < 59)
                {
                    ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[4];
                    opt[0] = new ShippingServiceOptionsType();
                    opt[0].ShippingServiceCost = new AmountType();
                    opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[0].ShippingService = "UPSGround";
                    opt[0].ShippingTimeMax = 5;
                    opt[0].ShippingTimeMin = 2;
                    if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                    {
                        opt[0].FreeShipping = true;
                        opt[0].FreeShippingSpecified = true;
                        opt[0].ShippingServiceCost.Value = 0;//6.95;
                    }
                    else
                    {
                        opt[0].ShippingServiceCost.Value = 3.95;//6.95;
                    }
                    opt[0].ShippingServiceAdditionalCost = new AmountType();
                    opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[0].ShippingServiceAdditionalCost.Value = 0;

                    // opt[0].ShippingServicePriority = 1;
                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);


                    opt[1] = new ShippingServiceOptionsType();
                    opt[1].ShippingServiceCost = new AmountType();
                    opt[1].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[1].ShippingServiceCost.Value = 15.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[1].ShippingService = "UPS3rdDay"; //3 day select
                    opt[1].ShippingTimeMax = 3;
                    opt[1].ShippingTimeMin = 1;
                    opt[1].ShippingServiceAdditionalCost = new AmountType();
                    opt[1].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[1].ShippingServiceAdditionalCost.Value = 0;

                    // opt[1].ShippingServicePriority = 2;
                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[1]);

                    opt[2] = new ShippingServiceOptionsType();
                    opt[2].ShippingServiceCost = new AmountType();
                    opt[2].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[2].ShippingServiceCost.Value = 20.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[2].ShippingService = "UPS2ndDay"; //3 day select
                    opt[2].ShippingTimeMax = 2;
                    opt[2].ShippingTimeMin = 1;

                    opt[2].ShippingServiceAdditionalCost = new AmountType();
                    opt[2].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[2].ShippingServiceAdditionalCost.Value = 0;

                    //opt[2].ShippingServicePriority = 3;
                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[2]);



                    opt[3] = new ShippingServiceOptionsType();
                    opt[3].ShippingServiceCost = new AmountType();
                    opt[3].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[3].ShippingServiceCost.Value = 30.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[3].ShippingService = "UPSNextDay"; //3 day select
                    opt[3].ShippingTimeMax = 1;
                    opt[3].ShippingTimeMin = 1;

                    opt[3].ShippingServiceAdditionalCost = new AmountType();
                    opt[3].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[3].ShippingServiceAdditionalCost.Value = 0;

                    //opt[3].ShippingServicePriority = 4;
                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[3]);


                }
                else if (saleprice < 149)
                {
                    ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[2];
                    opt[0] = new ShippingServiceOptionsType();
                    opt[0].ShippingServiceCost = new AmountType();
                    opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    //opt[0].ShippingServiceCost.Value = 20.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[0].ShippingService = "UPS3rdDay"; //3 day select
                    opt[0].ShippingTimeMax = 3;
                    opt[0].ShippingTimeMin = 1;
                    if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                    {
                        opt[0].FreeShipping = true;
                        opt[0].FreeShippingSpecified = true;
                        opt[0].ShippingServiceCost.Value = 0;
                    }
                    else
                    {
                        opt[0].ShippingServiceCost.Value = 20.95;
                    }
                    opt[0].ShippingServiceAdditionalCost = new AmountType();
                    opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[0].ShippingServiceAdditionalCost.Value = 0;

                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);

                    opt[1] = new ShippingServiceOptionsType();
                    opt[1].ShippingServiceCost = new AmountType();
                    opt[1].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[1].ShippingServiceCost.Value = 30.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[1].ShippingService = "UPS2ndDay"; //3 day select
                    opt[1].ShippingTimeMax = 2;
                    opt[1].ShippingTimeMin = 1;
                    opt[1].ShippingServiceAdditionalCost = new AmountType();
                    opt[1].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[1].ShippingServiceAdditionalCost.Value = 0;

                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[1]);
                }
                else if (saleprice < 350)
                {
                    ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[2];
                    opt[0] = new ShippingServiceOptionsType();
                    opt[0].ShippingServiceCost = new AmountType();
                    opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    // opt[0].ShippingServiceCost.Value = 25.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[0].ShippingService = "UPS3rdDay"; //3 day select
                    opt[0].ShippingTimeMax = 3;
                    opt[0].ShippingTimeMin = 1;
                    if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                    {
                        opt[0].FreeShipping = true;
                        opt[0].FreeShippingSpecified = true;
                        opt[0].ShippingServiceCost.Value = 0;
                    }
                    else
                    {
                        opt[0].ShippingServiceCost.Value = 25.95;
                    }
                    opt[0].ShippingServiceAdditionalCost = new AmountType();
                    opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[0].ShippingServiceAdditionalCost.Value = 0;

                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);

                    opt[1] = new ShippingServiceOptionsType();
                    opt[1].ShippingServiceCost = new AmountType();
                    opt[1].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[1].ShippingServiceCost.Value = 35.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[1].ShippingService = "UPS2ndDay"; //3 day select
                    opt[1].ShippingTimeMax = 2;
                    opt[1].ShippingTimeMin = 1;
                    opt[1].ShippingServiceAdditionalCost = new AmountType();
                    opt[1].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[1].ShippingServiceAdditionalCost.Value = 0;

                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[1]);
                }
                else
                {
                    ShippingServiceOptionsType[] opt = new ShippingServiceOptionsType[2];
                    opt[0] = new ShippingServiceOptionsType();
                    opt[0].ShippingServiceCost = new AmountType();
                    opt[0].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    //opt[0].ShippingServiceCost.Value = 29.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[0].ShippingService = "UPS3rdDay"; //3 day select
                    opt[0].ShippingTimeMax = 3;
                    opt[0].ShippingTimeMin = 1;
                    if (Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "true" || Convert.ToString(dr["IsfreeShipping"].ToString()).ToLower() == "1")
                    {
                        opt[0].FreeShipping = true;
                        opt[0].FreeShippingSpecified = true;
                        opt[0].ShippingServiceCost.Value = 0;
                    }
                    else
                    {
                        opt[0].ShippingServiceCost.Value = 29.95;
                    }
                    opt[0].ShippingServiceAdditionalCost = new AmountType();
                    opt[0].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[0].ShippingServiceAdditionalCost.Value = 0;

                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[0]);

                    opt[1] = new ShippingServiceOptionsType();
                    opt[1].ShippingServiceCost = new AmountType();
                    opt[1].ShippingServiceCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[1].ShippingServiceCost.Value = 39.95;

                    // ShippingService is now a string
                    //Make a call to GeteBayDetails to find out the valid Shipping Service values
                    opt[1].ShippingService = "UPS2ndDay"; //3 day select
                    opt[1].ShippingTimeMax = 2;
                    opt[1].ShippingTimeMin = 1;
                    opt[1].ShippingServiceAdditionalCost = new AmountType();
                    opt[1].ShippingServiceAdditionalCost.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                    opt[1].ShippingServiceAdditionalCost.Value = 0;

                    if (item.ShippingDetails.ShippingServiceOptions.Count < 3)
                        item.ShippingDetails.ShippingServiceOptions.Add(opt[1]);
                }

                item.ShippingDetails.ShippingType = ShippingTypeCodeType.Flat;
                #endregion



                item.Location = "US";

                item.UUID = System.Guid.NewGuid().ToString().Replace("-", "");

                #region Variant


                DataSet dsEbayVar = new DataSet();
                DataSet dsEbayVariant = new DataSet();

                dsEbayVariant = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + dr["ProductID"].ToString() + " Order By Displayorder");

                bool flag = false;
                int Totalqty = 0;
                if (dsEbayVariant.Tables[0].Rows.Count > 0)
                {
                    item.Variations = new VariationsType();
                    item.Variations.VariationSpecificsSet = new NameValueListTypeCollection();
                    Int32 i1 = 0;
                    Int32 i2 = 0;
                    string strvariant = "10,";
                    for (int l = 0; l < dsEbayVariant.Tables[0].Rows.Count; l++)
                    {
                        dsEbayVar = CommonComponent.GetCommonDataSet("SELECT * FROM Get_Variant_Details WHERE ProductID=" + dr["ProductID"].ToString() + " AND VariantID=" + dsEbayVariant.Tables[0].Rows[l]["VariantID"].ToString() + " Order By VariantName");
                        if (dsEbayVar.Tables[0].Rows.Count > 0)
                        {
                            NameValueListType NVListVS2 = new NameValueListType();
                            NVListVS2.Name = dsEbayVariant.Tables[0].Rows[l]["VariantName"].ToString();
                            StringCollection VSvaluecollection2 = new StringCollection();
                            string strvalue = "";
                            String[] Colour = new String[dsEbayVar.Tables[0].Rows.Count];
                            for (int j = 0; j < dsEbayVar.Tables[0].Rows.Count; j++)
                            {
                                //strvalue += "" + dsEbayVar.Tables[0].Rows[j]["VariantValue"].ToString() + "",";
                                Colour[j] = dsEbayVar.Tables[0].Rows[j]["VariantValue"].ToString();
                            }
                            if (dsEbayVar.Tables[0].Rows.Count > i1)
                            {
                                i1 = Convert.ToInt32(dsEbayVar.Tables[0].Rows.Count);
                                i2 = Convert.ToInt32(dsEbayVariant.Tables[0].Rows[l]["VariantID"].ToString());
                            }
                            // strvalue = strvalue.Substring(0, strvalue.Length - 1);

                            VSvaluecollection2.AddRange(Colour);
                            NVListVS2.Value = VSvaluecollection2;
                            item.Variations.VariationSpecificsSet.Add(NVListVS2);
                            strvariant += dsEbayVariant.Tables[0].Rows[l]["VariantID"].ToString() + ",";
                        }
                    }
                    if (strvariant.Length > 0)
                    {
                        strvariant = strvariant.Substring(0, strvariant.Length - 1);
                    }
                    else
                    {
                        strvariant = "0";
                    }

                    VariationTypeCollection VarCol = new VariationTypeCollection();
                    NameValueListTypeCollection objColl = new NameValueListTypeCollection();


                    DataSet dsEbayVarmain = new DataSet();
                    dsEbayVarmain = CommonComponent.GetCommonDataSet("SELECT * FROM Get_Variant_Details WHERE ProductID=" + dr["ProductID"].ToString() + " AND VariantID=" + i2.ToString() + " Order By VariantName");
                    if (dsEbayVarmain.Tables[0].Rows.Count > 0)
                    {
                        DataSet dsEbayVarmainchild = new DataSet();
                        dsEbayVarmainchild = CommonComponent.GetCommonDataSet("SELECT * FROM Get_Variant_Details WHERE ProductID=" + dr["ProductID"].ToString() + " AND VariantID <> " + i2.ToString() + " AND  isnull(VariantID,0) in (" + strvariant + ") Order By VariantName");
                        if (dsEbayVarmainchild.Tables[0].Rows.Count > 0)
                        {
                            for (int k = 0; k < dsEbayVarmainchild.Tables[0].Rows.Count; k++)
                            {
                                for (int j = 0; j < dsEbayVarmain.Tables[0].Rows.Count; j++)
                                {

                                    VariationType var1 = new VariationType();
                                    NameValueListType Var1Spec2 = new NameValueListType();
                                    StringCollection Var1Spec2Valuecoll = new StringCollection();
                                    Var1Spec2.Name = dsEbayVarmain.Tables[0].Rows[j]["VariantName"].ToString();

                                    Var1Spec2.SourceSpecified = true;
                                    if (!string.IsNullOrEmpty(dsEbayVarmain.Tables[0].Rows[j]["SKU"].ToString()))
                                    {
                                        var1.SKU = dsEbayVarmain.Tables[0].Rows[j]["SKU"].ToString();//dr["SKU"].ToString() + "-" + dsEbayVarmain.Tables[0].Rows[j]["VariantValue"].ToString().Replace("\"", "").Replace("'", "").Replace(" ", "-") + "-" + dsEbayVarmainchild.Tables[0].Rows[k]["VariantValue"].ToString();
                                    }
                                    else
                                    {
                                        var1.SKU = dr["SKU"].ToString() + "-" + dsEbayVarmain.Tables[0].Rows[j]["VariantValue"].ToString().Replace("\"", "").Replace("'", "").Replace(" ", "-") + "-" + dsEbayVarmainchild.Tables[0].Rows[k]["VariantValue"].ToString();
                                    }
                                    var1.Quantity = Convert.ToInt32(dsEbayVarmain.Tables[0].Rows[j]["Inventory"].ToString());
                                    Totalqty += Convert.ToInt32(dsEbayVarmain.Tables[0].Rows[j]["Inventory"].ToString());
                                    var1.StartPrice = new AmountType();
                                    var1.StartPrice.currencyID = eBay.Service.Core.Soap.CurrencyCodeType.USD;
                                    double totalprice = Convert.ToDouble(dr["saleprice1"].ToString());
                                    totalprice = totalprice + Convert.ToDouble(dsEbayVarmain.Tables[0].Rows[j]["VariantPrice"].ToString());
                                    var1.StartPrice.Value = totalprice;
                                    var1.VariationSpecifics = new NameValueListTypeCollection();

                                    Var1Spec2Valuecoll.Add(dsEbayVarmain.Tables[0].Rows[j]["VariantValue"].ToString());
                                    Var1Spec2.Value = Var1Spec2Valuecoll;

                                    var1.VariationSpecifics.Add(Var1Spec2);

                                    NameValueListType Var2Spec1 = new NameValueListType();
                                    StringCollection Var2Spec1Valuecoll = new StringCollection();

                                    Var2Spec1.Name = dsEbayVarmainchild.Tables[0].Rows[k]["VariantName"].ToString();
                                    Var2Spec1Valuecoll.Add(dsEbayVarmainchild.Tables[0].Rows[k]["VariantValue"].ToString());
                                    Var2Spec1.Value = Var2Spec1Valuecoll;
                                    var1.VariationSpecifics.Add(Var2Spec1);
                                    VarCol.Add(var1);
                                }

                            }
                        }
                    }

                    //item.Variations.VariationSpecificsSet = objColl;
                    item.Variations.Variation = VarCol;
                    item.Quantity = Totalqty;
                }

                #endregion

                #region Listing images

                #region Test sample images

                #endregion


                #region add images from live
                item.PictureDetails = new PictureDetailsType();
                item.PictureDetails.PictureURL = new StringCollection();

                item.PictureDetails.PictureURL.Add(Live_Server + GetMediumImage(ImageServer + "product/medium/" + dr["ImageName"].ToString()));
                item.PictureDetails.GalleryType = GalleryTypeCodeType.Gallery;

                for (int i = 1; i < 26; i++)
                    if (System.IO.File.Exists(Server.MapPath(ImageServer + "Product/Medium/" + dr["ImageName"].ToString().Replace(".jpeg", "").Replace(".jpg", "") + "_" + i.ToString() + ".jpg")) == true)
                    {

                        item.PictureDetails.PictureURL.Add(Live_Server + GetMediumImage(ImageServer + "product/Medium/" + dr["ImageName"].ToString().ToLower().Replace(".jpeg", "").Replace(".jpg", "") + "_" + i.ToString() + ".jpg"));
                    }
                #endregion
                item.PictureDetails.PhotoDisplaySpecified = true;
                item.PictureDetails.PhotoDisplay = PhotoDisplayCodeType.SuperSizePictureShow;


                #endregion

                item.ListingDetails = new ListingDetailsType();
                Int32 listingdays = Convert.ToInt32(AppLogic.AppConfigs("EbayListingDuration").ToString());
                item.ListingDuration = "Days_5";
                item.DispatchTimeMax = 1;
                item.ListingType = ListingTypeCodeType.Chinese;

                DataSet dsebay = new DataSet();
                dsebay = dbAccess.GetDs("select ebayLastUpdated,EbayProductID from tb_Product where ProductID=" + dr["ProductID"].ToString());
                try
                {
                    if (string.IsNullOrEmpty(dsebay.Tables[0].Rows[0]["EbayProductID"].ToString().Trim()))
                    {
                        AddItemCall fixedpriceitem = new AddItemCall(GetContext());
                        FeeTypeCollection fees = fixedpriceitem.AddItem(item);
                        double decTotalFee = 0;
                        foreach (FeeType fee in fees)
                            decTotalFee += fee.Fee.Value;
                        dbAccess.ExecuteNonQuery("update tb_Product set EbayProductID='" + item.ItemID + "',EbayCreated='" + DateTime.Now + "',EbayLastUpdated='" + DateTime.Now + "',EbayListingFee=" + decTotalFee + " where productid=" + dr["ProductID"].ToString());
                        insertcnt++;
                    }
                    else
                    {
                        DateTime dtLastUpdated = Convert.ToDateTime(dsebay.Tables[0].Rows[0]["EbayLastUpdated"].ToString());

                        if (dtLastUpdated.AddDays(listingdays) < DateTime.Today && listingdays > 0)
                        {

                            RelistFixedPriceItemCall relistitem = new RelistFixedPriceItemCall(GetContext());
                            FeeTypeCollection fees = relistitem.RelistFixedPriceItem(item, null);
                            double decTotalFee = 0;
                            foreach (FeeType fee in fees)
                                decTotalFee += fee.Fee.Value;
                            // dbAccess.ExecuteNonQuery("update tb_ecomm_product set EbayProductID='" + item.ItemID + "',EbayListingFee=EbayListingFee+" + decTotalFee + ",EbayLastUpdated='" + DateTime.Now + "',EbayCreated='" + DateTime.Now + "' where productid=" + dr["ProductID"].ToString());
                            relistcnt++;
                        }
                        else
                        {
                            lblMsg.Text = "Auction Item can not be Revise.";
                            lblMsg.Visible = true;
                            trMsg.Attributes.Add("style", "display:block;");
                            return;
                        }
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message.ToString();
                    lblMsg.Visible = true;
                    trMsg.Attributes.Add("style", "display:'';");
                    return;
                }

            }
            lblMsg.Text = insertcnt + " Number of Products Listed. <br/>" + revicecnt + " Number of Products Revised. <br/>" + relistcnt + " Number of Products Re-Listed.";
            lblMsg.Visible = true;
            trMsg.Attributes.Add("style", "display:'';");
        }

        /// <summary>
        /// Bind Star Images
        /// </summary>
        /// <param name="d">Decimal d</param>
        /// <returns>Returns the string of Images HTML</returns>
        public String BindStarsImage(Decimal d)
        {
            Live_Server = AppLogic.AppConfigs("Live_Server");

            String s = String.Empty;
            if (d == decimal.Zero)
            {
                return string.Empty;
            }
            else if (d < 0.12M)
            {
                //s = "<img style='text-align:center;vertical-align:middle;padding-right: 1px;'  src=\"/ebay/images/rating_inactive.gif\"    /><img style='text-align:center;vertical-align:middle;padding-right: 1px;' src=\"/ebay/images/rating_inactive.gif\"  /><img style='text-align:center;vertical-align:middle;padding-right: 1px;' src=\"/ebay/images/rating_inactive.gif\"  /><img style='text-align:center;vertical-align:middle;padding-right: 1px;' src=\"/ebay/images/rating_inactive.gif\"  /><img style='text-align:center;vertical-align:middle;padding-right: 1px;' src=\"/ebay/images/rating_inactive.gif\"  />";
            }
            else if (d >= 0.12M && d < 0.37M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_1_4.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  />";
            }
            else if (d >= 0.37M && d < 0.62M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/starh.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  />";
            }
            else if (d >= 0.62M && d < 0.87M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_3_4.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  />";
            }
            else if (d >= 0.87M && d < 1.12M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\" />";
            }
            else if (d >= 1.12M && d < 1.37M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_1_4.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 1.37M && d < 1.62M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/starh.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 1.62M && d < 1.87M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_3_4.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 1.87M && d < 2.12M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"   /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 2.12M && d < 2.37M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_1_4.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 2.37M && d < 2.62M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/starh.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 2.62M && d < 2.87M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_3_4.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 2.87M && d < 3.12M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"   /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 3.12M && d < 3.37M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\"  width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_1_4.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 3.37M && d < 3.62M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right:1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/starh.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 3.62M && d < 3.87M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_3_4.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 3.87M && d < 4.12M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_inactive.gif\"   />";
            }
            else if (d >= 4.12M && d < 4.37M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_1_4.gif\"   />";
            }
            else if (d >= 4.37M && d < 4.62M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"    /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/starh.gif\"   />";
            }
            else if (d >= 4.62M && d < 4.87M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"   /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_3_4.gif\"   />";
            }
            else if (d >= 4.87M)
            {
                s = "<img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"   /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"  /><img style=\"text-align:center;vertical-align:middle;padding-right: 1px;\" width=\"14px\" height=\"14px\" src=\"/ebay/images/rating_active.gif\"   />";
            }
            return s.Replace("/ebay/images", Live_Server + "/ebay/images");
        }

        /// <summary>
        /// Get Medium Image of a Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Path for Medium Image</returns>
        public String GetMediumImage(String img)
        {
            String imagepath = String.Empty;
            String Temp = img;
            imagepath = Temp;
            string strPth = Server.MapPath(imagepath).ToString();
            if (System.IO.File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/image_not_available.jpg");
        }

        /// <summary>
        /// Get Micro Image of a Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Path for Micro Image</returns>
        public String GetMicroImage(String img)
        {
            String imagepath = String.Empty;
            String Temp = img;
            imagepath = Temp;
            if (System.IO.File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/image_not_available.jpg");
        }

        /// <summary>
        /// Get the Credentials for eBay Connections
        /// </summary>
        /// <returns>Returns the ApiContext object</returns>
        public ApiContext GetContext()
        {

            ApiContext contexttemp = new ApiContext();

            if (Convert.ToBoolean(Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString())) || Convert.ToInt32(AppLogic.AppConfigs("UseEbaySandBox").ToString()) == 1)
            //if (Convert.ToBoolean(true))
            {
                // credentials for the call
                contexttemp.ApiCredential.ApiAccount.Developer = "330ff77c-5092-473a-908c-f16c03d169e5"; //AppLogic.AppConfigs("ebaySandboxDevKey");
                contexttemp.ApiCredential.ApiAccount.Application = "Kaushala-b604-4f83-bf0c-ebc59b93cfdf";// AppLogic.AppConfigs("ebaySandboxAppKey");
                contexttemp.ApiCredential.ApiAccount.Certificate = "999022da-5ac7-4b11-8c61-d7b7a568d13a";// AppLogic.AppConfigs("ebaySandboxCertiKey");
                contexttemp.ApiCredential.eBayToken = "AgAAAA**AQAAAA**aAAAAA**rHU9UQ**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4CoAZaGoQWdj6x9nY+seQ**eZMBAA**AAMAAA**M2bGqoyFyDop9bYmsEvtXyx+q/7zICvZeKGMtpt1hq3hRM7tHojVZvIZsRpn2Cql5c1pb2C5a0ZFCO3g1P75i5qpVUks2dbT8pJV6m/kpGy6RIuiywz7i1wRS1bYiRap+z4TOfuvtm7Uk4T2T1phSUnHZLg2rX0P2swIGeYMXHoz13sfSczgXPLHcF7tvyKwEXrt/t1uwKLYT2INyHiyCeUG99Nwu3YEAVO5J9mDC4xYfXpTAXIOa5NQ7MqbprZN8VZZXLIyznOu34xkKMDVaqx47Ir3G6Ei+b1sGsiN8oSfa33pbuPv3WLXpvKyrIvUXUD3uUuCmcARmJMyocl5Rfym25R/tqXUrh+1KjsFRxWGiKE/U75nX2esAYfCb1H0JaAyqBumRRwqk0hgoq+C33QJ1xWOd2dn1Yf05gJWgJM2vgv3xdz2mpDjLynNKDsu609ezl4Zj14ehiaMtqQHlPrtSkF2JNllE89rSdU43fI1UlZ9ROPKfMhKHFPDzWLnCRZMnJUjn8Fs6CJUqTDsKbH6fQDr6g6Xkx/plcej54SJQVGqvL8jNMl9oYxAl+wN6F+ID/PSitGGnDEhlmDnq2pK8+cG7CFhrFmLoiV1AK//93pMkoxrqodqFbT510Ad8tdkG3gppm81m6lrYgjjtwlr8pvgzzBPC1mvMO+bRV81bE1Ys78x8+SyycpqcAKVq7ScQ9InDIZF2gccQ5UAhXqpxoc5EoqJCPx9Mk7FqOv4omFnW1IKmXd8Q/vUEQ52";// AppLogic.AppConfigs("ebaySandboxToken");
                // set the url
                contexttemp.SoapApiServerUrl = "https://api.sandbox.ebay.com/wsapi";// AppLogic.AppConfigs("ebaySandboxServerUrl");
                //contexttemp.Version = "485";
                //contexttemp.ApiCredential.eBayAccount.UserName = "TESTUSER_Kaushalam";
                //contexttemp.ApiCredential.eBayAccount.Password = "n$r7xRu";
            }
            else
            {
                ////set the productin keys and token
                contexttemp.ApiCredential.ApiAccount.Developer = AppLogic.AppConfigs("ebayDevKey"); ;
                contexttemp.ApiCredential.ApiAccount.Application = AppLogic.AppConfigs("ebayAppKey");
                contexttemp.ApiCredential.ApiAccount.Certificate = AppLogic.AppConfigs("ebayCertiKey");
                contexttemp.ApiCredential.eBayToken = AppLogic.AppConfigs("ebayToken");
                //set the server url
                contexttemp.SoapApiServerUrl = AppLogic.AppConfigs("ebayServerUrl");



                //contexttemp.ApiCredential.ApiAccount.Developer = "330ff77c-5092-473a-908c-f16c03d169e5"; //AppLogic.AppConfigs("ebaySandboxDevKey");
                //contexttemp.ApiCredential.ApiAccount.Application = "Kaushala-b604-4f83-bf0c-ebc59b93cfdf";// AppLogic.AppConfigs("ebaySandboxAppKey");
                //contexttemp.ApiCredential.ApiAccount.Certificate = "999022da-5ac7-4b11-8c61-d7b7a568d13a";// AppLogic.AppConfigs("ebaySandboxCertiKey");
                //contexttemp.ApiCredential.eBayToken = "AgAAAA**AQAAAA**aAAAAA**rHU9UQ**nY+sHZ2PrBmdj6wVnY+sEZ2PrA2dj6wFk4CoAZaGoQWdj6x9nY+seQ**eZMBAA**AAMAAA**M2bGqoyFyDop9bYmsEvtXyx+q/7zICvZeKGMtpt1hq3hRM7tHojVZvIZsRpn2Cql5c1pb2C5a0ZFCO3g1P75i5qpVUks2dbT8pJV6m/kpGy6RIuiywz7i1wRS1bYiRap+z4TOfuvtm7Uk4T2T1phSUnHZLg2rX0P2swIGeYMXHoz13sfSczgXPLHcF7tvyKwEXrt/t1uwKLYT2INyHiyCeUG99Nwu3YEAVO5J9mDC4xYfXpTAXIOa5NQ7MqbprZN8VZZXLIyznOu34xkKMDVaqx47Ir3G6Ei+b1sGsiN8oSfa33pbuPv3WLXpvKyrIvUXUD3uUuCmcARmJMyocl5Rfym25R/tqXUrh+1KjsFRxWGiKE/U75nX2esAYfCb1H0JaAyqBumRRwqk0hgoq+C33QJ1xWOd2dn1Yf05gJWgJM2vgv3xdz2mpDjLynNKDsu609ezl4Zj14ehiaMtqQHlPrtSkF2JNllE89rSdU43fI1UlZ9ROPKfMhKHFPDzWLnCRZMnJUjn8Fs6CJUqTDsKbH6fQDr6g6Xkx/plcej54SJQVGqvL8jNMl9oYxAl+wN6F+ID/PSitGGnDEhlmDnq2pK8+cG7CFhrFmLoiV1AK//93pMkoxrqodqFbT510Ad8tdkG3gppm81m6lrYgjjtwlr8pvgzzBPC1mvMO+bRV81bE1Ys78x8+SyycpqcAKVq7ScQ9InDIZF2gccQ5UAhXqpxoc5EoqJCPx9Mk7FqOv4omFnW1IKmXd8Q/vUEQ52";// AppLogic.AppConfigs("ebaySandboxToken");
                //// set the url
                //contexttemp.SoapApiServerUrl = "https://api.sandbox.ebay.com/wsapi";// AppLogic.AppConfigs("ebaySandboxServerUrl");


            }
            return contexttemp;
        }
        #endregion

        #endregion

        #region Code for Sears Upload

        #region For Update Inventory
        /// <summary>
        ///  Sears Inventory Update Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearsUpdate_Click(object sender, EventArgs e)
        {
            InventoryItemSears();
        }

        /// <summary>
        /// Update inventory for Sears
        /// </summary>
        private void InventoryItemSears()
        {
            AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            String strProductPath = AppLogic.AppConfigs("ImagePathProduct").Trim().Replace("product/", "");
            StreamReader strXml = File.OpenText(Server.MapPath(strProductPath.Trim()) + "/ProductXML/inventory-xml-feed-v1-example.xml");
            string xmlFinal = strXml.ReadToEnd();

            SQLAccess objSql = new SQLAccess();
            DataSet dsData = new DataSet();
            string strIds = "0";
            foreach (GridViewRow gr in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)gr.FindControl("chkSelect");
                HiddenField lblid = (HiddenField)gr.FindControl("hdnProductid");
                if (chk.Checked)
                {
                    strIds += "," + lblid.Value.ToString();
                }
            }

            dsData = objSql.GetDs("SELECT Inventory,SKU FROM tb_Product WHERE ProductId in (" + strIds + ")");
            string strStringXMl = "";

            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                int j = 0;
                for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
                {
                    j = j + 1;
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["SKU"].ToString()))
                    {
                        strStringXMl += "<item item-id=\"" + dsData.Tables[0].Rows[i]["SKU"].ToString().Trim() + "\">\r\n";
                    }
                    //if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Inventory"].ToString()))
                    //{
                    //    // strStringXMl += "<quantity>" + dsData.Tables[0].Rows[i]["Inventory"].ToString() + "</quantity>\r\n";
                    //}

                    strStringXMl += "<locations>";
                    strStringXMl += "<location location-id=\"" + AppLogic.AppConfigs("Searslocation").ToString() + "\">\r\n";
                    strStringXMl += "<quantity>" + dsData.Tables[0].Rows[i]["Inventory"].ToString() + "</quantity>\r\n";
                    strStringXMl += "<pick-up-now-eligible>false</pick-up-now-eligible>\r\n";
                    strStringXMl += "</location>\r\n";
                    strStringXMl += "</locations>\r\n";
                    strStringXMl += "</item>\r\n";
                }
            }
            xmlFinal = xmlFinal.Replace("###Inventory####", strStringXMl.ToString());
            StreamWriter writer = null;
            string strfilename = "";

            try
            {
                strfilename = Server.MapPath(strProductPath.Trim()) + "/ProductXML/Sears" + DateTime.Now.Ticks.ToString() + ".xml";
                writer = new StreamWriter(strfilename, true);
                writer.Write(xmlFinal);
            }
            catch { }
            finally
            {
                if (writer != null)
                    writer.Close();
            }

            try
            {
                StreamReader str = File.OpenText(strfilename);
                string xml = str.ReadToEnd();
                byte[] arr = System.Text.Encoding.ASCII.GetBytes(xml);

                // Prepare web request...
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://seller.marketplace.sears.com/SellerPortal/api/inventory/fbm-lmp/v6?email=" + AppLogic.AppConfigs("Searsusername") + "&password=" + AppLogic.AppConfigs("SearsPassword") + "");
                myRequest.Method = "PUT";
                myRequest.ContentType = "application/xml";
                myRequest.ContentLength = arr.Length;
                Stream newStream = myRequest.GetRequestStream();
                // Send the data.
                newStream.Write(arr, 0, arr.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
                Encoding enc = System.Text.Encoding.GetEncoding(1252);
                StreamReader loResponseStream = new StreamReader(response.GetResponseStream(), enc);
                try
                {
                    string Response = loResponseStream.ReadToEnd();
                    loResponseStream.Close();
                    response.Close();
                    lblMsg.Text = "File uploaded Successfully.";
                }
                catch
                {

                    lblMsg.Text = "File uploaded Successfully.";
                    loResponseStream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message.ToString();
            }
        }
        #endregion

        #region For Upload Product
        /// <summary>
        ///  Sears Product Upload Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearsProduct_Click(object sender, EventArgs e)
        {
            UploadItemSears();
        }

        /// <summary>
        /// Upload Product in Sears
        /// </summary>
        private void UploadItemSears()
        {
            AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            String strProductPath = AppLogic.AppConfigs("ImagePathProduct").Trim().Replace("product/", "");
            StreamReader strXml = File.OpenText(Server.MapPath(strProductPath) + "/ProductXML/lmp-item-v4-nonvariation1.xml");
            string xmlFinal = strXml.ReadToEnd();

            SQLAccess objSql = new SQLAccess();
            DataSet dsData = new DataSet();
            string strIds = "0";
            foreach (GridViewRow gr in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)gr.FindControl("chkSelect");
                HiddenField lblid = (HiddenField)gr.FindControl("hdnProductid");

                if (chk.Checked)
                {
                    strIds += "," + lblid.Value.ToString();
                }
            }

            dsData = objSql.GetDs("SELECT dbo.tb_Product.*, dbo.tb_Manufacture.Name AS Manufacturename FROM dbo.tb_Manufacture INNER JOIN dbo.tb_Product ON dbo.tb_Manufacture.ManufactureID = dbo.tb_Product.ManufactureID WHERE dbo.tb_Product.ProductId IN (" + strIds + ")");
            string strStringXMl = "";

            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["SKU"].ToString()))
                    {
                        strStringXMl += "<item item-id=\"" + dsData.Tables[0].Rows[i]["SKU"].ToString().Trim() + "\">\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["name"].ToString()))
                    {
                        strStringXMl += "<title>" + dsData.Tables[0].Rows[i]["name"].ToString() + "</title>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Summary"].ToString()))
                    {
                        strStringXMl += "<short-desc>" + dsData.Tables[0].Rows[i]["Summary"].ToString() + "</short-desc>\r\n";
                    }
                    else
                    {
                        strStringXMl += "<short-desc></short-desc>\r\n";
                    }
                    strStringXMl += "<mature-content>false</mature-content>\r\n";
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["UPC"].ToString()))
                    {
                        strStringXMl += "<upc>" + dsData.Tables[0].Rows[i]["UPC"].ToString() + "</upc>\r\n";
                    }
                    else
                    {
                        strStringXMl += "<upc></upc>\r\n";
                    }

                    objSql = new SQLAccess();
                    DataSet dsnew = new DataSet();
                    //                    dsnew = objSql.GetDs(@"SELECT     dbo.tb_Ecomm_Category.Name, dbo.tb_Ecomm_Category.CategoryID, isnull(dbo.tb_Ecomm_Category.SearsCategoryID,0) as SearsCategoryID 
                    //                      FROM         dbo.tb_Ecomm_Category INNER JOIN
                    //                      dbo.tb_Ecomm_ProductCategory ON dbo.tb_Ecomm_Category.CategoryID = dbo.tb_Ecomm_ProductCategory.CategoryID INNER JOIN
                    //                      dbo.tb_Ecomm_Product ON dbo.tb_Ecomm_ProductCategory.ProductID = dbo.tb_Ecomm_Product.ProductID WHERE dbo.tb_Ecomm_Product.ProductID=" + dsData.Tables[0].Rows[i]["ProductId"].ToString() + @"");

                    dsnew = objSql.GetDs(@"SELECT dbo.tb_Category.Name, dbo.tb_Category.CategoryID, isnull(dbo.tb_Category.SearsCategoryID,0) as SearsCategoryID 
                                            FROM dbo.tb_Category INNER JOIN
                                            dbo.tb_ProductCategory ON dbo.tb_Category.CategoryID = dbo.tb_ProductCategory.CategoryID INNER JOIN
                                            dbo.tb_Product ON dbo.tb_ProductCategory.ProductID = dbo.tb_Product.ProductID WHERE dbo.tb_Product.ProductID=" + dsData.Tables[0].Rows[i]["ProductId"].ToString());


                    if (dsnew != null && dsnew.Tables.Count > 0 && dsnew.Tables[0].Rows.Count > 0)
                    {
                        for (int icat = 0; icat < dsnew.Tables[0].Rows.Count; icat++)
                        {
                            strStringXMl += "<item-class id=\"" + dsnew.Tables[0].Rows[icat]["SearsCategoryID"].ToString() + "\">\r\n";
                            strStringXMl += "<name>" + dsnew.Tables[0].Rows[icat]["Name"].ToString() + "</name>\r\n";
                            strStringXMl += "</item-class>\r\n";
                            strStringXMl += "<your-categorization></your-categorization>\r\n";
                        }
                    }

                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Description"].ToString()))
                    {
                        strStringXMl += "<long-desc><![CDATA[" + dsData.Tables[0].Rows[i]["Description"].ToString() + "]]></long-desc>\r\n";
                    }
                    else
                    {
                        strStringXMl += "<long-desc></long-desc>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["ManufacturePartNo"].ToString()))
                    {
                        strStringXMl += "<model-number>" + dsData.Tables[0].Rows[i]["ManufacturePartNo"].ToString() + "</model-number>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Price"].ToString()))
                    {
                        strStringXMl += "<standard-price>" + string.Format("{0:0.00}", Convert.ToDecimal(dsData.Tables[0].Rows[i]["Price"].ToString())) + "</standard-price>\r\n";
                    }
                    strStringXMl += "<sale>\r\n";
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["SalePrice"].ToString()))
                    {
                        if (Convert.ToDecimal(dsData.Tables[0].Rows[i]["SalePrice"].ToString()) > Decimal.Zero)
                        {
                            strStringXMl += "<sale-price>" + string.Format("{0:0.00}", Convert.ToDecimal(dsData.Tables[0].Rows[i]["SalePrice"].ToString())) + "</sale-price>\r\n";
                        }
                        else
                        {
                            strStringXMl += "<sale-price></sale-price>\r\n";
                        }
                    }
                    else
                    {
                        strStringXMl += "<sale-price></sale-price>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["AvaliableStartDate"].ToString()))
                    {
                        strStringXMl += "<sale-start-date>" + string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dsData.Tables[0].Rows[i]["AvaliableStartDate"].ToString())) + "</sale-start-date>\r\n";
                    }
                    else
                    {
                        strStringXMl += "<sale-start-date></sale-start-date>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["AvailableEndDate"].ToString()))
                    {
                        strStringXMl += "<sale-end-date>" + string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dsData.Tables[0].Rows[i]["AvailableEndDate"].ToString())) + "</sale-end-date>\r\n";
                    }
                    else
                    {
                        strStringXMl += "<sale-end-date></sale-end-date>\r\n";
                    }
                    strStringXMl += "<promotional-text></promotional-text>\r\n";
                    strStringXMl += "</sale>\r\n";

                    strStringXMl += "<shipping-override>\r\n";
                    strStringXMl += "<shipping-method-ground status=\"enabled\">\r\n";
                    strStringXMl += "</shipping-method-ground>\r\n";
                    strStringXMl += "<shipping-method-expedited status=\"enabled\">\r\n";
                    strStringXMl += "</shipping-method-expedited>\r\n";
                    strStringXMl += "<shipping-method-premium status=\"enabled\">\r\n";
                    strStringXMl += "</shipping-method-premium>\r\n";
                    strStringXMl += "</shipping-override>\r\n";

                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Surcharge"].ToString()))
                    {
                        //strStringXMl += "<handling-fee>" + string.Format("{0:0}", Convert.ToDecimal(dsData.Tables[0].Rows[i]["Surcharge"].ToString())) + "</handling-fee>\r\n";
                    }
                    else
                    {
                        //strStringXMl += "<handling-fee></handling-fee>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["MapPriceIndicator"].ToString()))
                    {
                        strStringXMl += "<map-price-indicator>" + dsData.Tables[0].Rows[i]["MapPriceIndicator"].ToString().ToLower() + "</map-price-indicator>\r\n";
                    }
                    else
                    {
                        strStringXMl += "<map-price-indicator></map-price-indicator>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Manufacturename"].ToString()))
                    {
                        strStringXMl += "<brand>" + dsData.Tables[0].Rows[i]["Manufacturename"].ToString() + "</brand>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Width"].ToString()))
                    {
                        strStringXMl += "<shipping-length>" + dsData.Tables[0].Rows[i]["Width"].ToString() + "</shipping-length>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["width"].ToString()))
                    {
                        strStringXMl += "<shipping-width>" + dsData.Tables[0].Rows[i]["width"].ToString() + "</shipping-width>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Height"].ToString()))
                    {
                        strStringXMl += "<shipping-height>" + dsData.Tables[0].Rows[i]["Height"].ToString() + "</shipping-height>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["weight"].ToString()))
                    {
                        strStringXMl += "<shipping-weight>" + string.Format("{0:0.00}", Convert.ToDecimal(dsData.Tables[0].Rows[i]["weight"].ToString())) + "</shipping-weight>\r\n";
                    }

                    strStringXMl += "<local-marketplace-flags>\r\n";
                    strStringXMl += "<is-restricted>" + Convert.ToBoolean(dsData.Tables[0].Rows[i]["IsRestricted"].ToString()).ToString().ToLower() + "</is-restricted>\r\n";
                    strStringXMl += "<perishable>" + Convert.ToBoolean(dsData.Tables[0].Rows[i]["Perishable"].ToString()).ToString().ToLower() + "</perishable>\r\n";
                    strStringXMl += "<requires-refrigeration>false</requires-refrigeration>\r\n";
                    strStringXMl += "<requires-freezing>false</requires-freezing>\r\n";
                    strStringXMl += "<contains-alcohol>false</contains-alcohol>\r\n";
                    strStringXMl += "<contains-tobacco>false</contains-tobacco>\r\n";
                    strStringXMl += "</local-marketplace-flags>\r\n";
                    string strSoucefilelarge = strProductPath.Trim() + "Product/Large/";
                    if (File.Exists(Server.MapPath(strSoucefilelarge) + dsData.Tables[0].Rows[i]["Imagename"].ToString()))
                    {
                        //strStringXMl += "<image-url><url>" + AppLogic.AppConfig("Live_Server_Product") + strSoucefilelarge + dsData.Tables[0].Rows[i]["Imagename"].ToString() + "</url></image-url>\r\n";
                        strStringXMl += "<image-url><url>" + AppLogic.AppConfigs("Live_Server_Product") + strSoucefilelarge + dsData.Tables[0].Rows[i]["Imagename"].ToString() + "</url></image-url>\r\n";
                        StringArrayConverter Storeconvertor = new StringArrayConverter();
                        Array StoreArray = (Array)Storeconvertor.ConvertFrom(AppLogic.AppConfigs("AllowedExtensions"));
                        int k = 0;
                        try
                        {
                            for (int iIm = 1; iIm < 26; iIm++)
                            {
                                for (int j = 0; j < StoreArray.Length; j++)
                                {
                                    FileInfo fl = new FileInfo(dsData.Tables[0].Rows[i]["Imagename"].ToString());
                                    string strimagenme = fl.Name.ToString().Replace(fl.Extension.ToString(), "");
                                    strimagenme = strimagenme.Replace(StoreArray.GetValue(j).ToString(), "");
                                    if (File.Exists(Server.MapPath(strSoucefilelarge + strimagenme.ToString() + "_" + iIm.ToString() + StoreArray.GetValue(j).ToString())) == true)
                                    {
                                        k += 1;
                                        if (k == 1)
                                        {
                                            strStringXMl += "<feature-image-url>\r\n<url>" + AppLogic.AppConfigs("Live_Server_Product") + strSoucefilelarge + strimagenme.ToString() + "_" + iIm.ToString() + StoreArray.GetValue(j).ToString() + "</url>\r\n</feature-image-url>\r\n";
                                            //strStringXMl += "<swatch-image-url><url>" + AppLogic.AppConfig("Live_Server_Product") + strSoucefilelarge + dsData.Tables[0].Rows[i]["ProductId"].ToString() + "_" + iIm.ToString() + "_" + StoreArray.GetValue(j).ToString() + "</url></swatch-image-url>\r\n";
                                        }
                                        if (k == 2)
                                        {
                                            strStringXMl += "<feature-image-url>\r\n<url>" + AppLogic.AppConfigs("Live_Server_Product") + strSoucefilelarge + strimagenme.ToString() + "_" + iIm.ToString() + StoreArray.GetValue(j).ToString() + "</url>\r\n</feature-image-url>\r\n";
                                        }
                                        if (k == 3)
                                        {
                                            strStringXMl += "<feature-image-url>\r\n<url>" + AppLogic.AppConfigs("Live_Server_Product") + strSoucefilelarge + strimagenme.ToString() + "_" + iIm.ToString() + StoreArray.GetValue(j).ToString() + "</url>\r\n</feature-image-url>\r\n";
                                        }
                                    }
                                }
                                if (k >= 3)
                                {
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }
                    }

                    strStringXMl += "<attributes>\r\n";
                    //if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Sizes"].ToString()))
                    //{
                    //strStringXMl += "<attribute>";
                    //strStringXMl += "<attribute-id></attribute-id>";
                    //strStringXMl += "<attribute-value-id id=\"\">";
                    //strStringXMl += "<name><![CDATA[" + dsData.Tables[0].Rows[i]["Sizes"].ToString() + "]]></name>";
                    //strStringXMl += "</attribute-value-id>";
                    //strStringXMl += "</attribute>";
                    //}

                    //if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["Colors"].ToString()))
                    //{
                    //strStringXMl += "<attribute>";
                    //strStringXMl += "<attribute-id></attribute-id>";
                    //strStringXMl += "<attribute-value-id id=\"\">";
                    //strStringXMl += "<name><![CDATA[" + dsData.Tables[0].Rows[i]["Colors"].ToString() + "]]></name>";
                    //strStringXMl += "</attribute-value-id>";
                    //strStringXMl += "</attribute>";

                    //}
                    strStringXMl += "</attributes>\r\n";
                    strStringXMl += "</item>\r\n";


                }
                xmlFinal = xmlFinal.Replace("###items###", strStringXMl.ToString());
                StreamWriter writer = null;
                string strfilename = "";
                try
                {
                    strfilename = Server.MapPath(strProductPath.Trim()) + "/ProductXML/Sears" + DateTime.Now.Ticks.ToString() + ".xml";
                    writer = new StreamWriter(strfilename, true);
                    writer.Write(xmlFinal);
                }
                catch { }
                finally
                {
                    if (writer != null)
                        writer.Close();
                }

                try
                {
                    StreamReader str = File.OpenText(strfilename);
                    string xml = str.ReadToEnd();
                    byte[] arr = System.Text.Encoding.ASCII.GetBytes(xml);

                    // Prepare web request...
                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://seller.marketplace.sears.com/SellerPortal/api/catalog/fbm/v10?email=" + Server.UrlEncode(AppLogic.AppConfigs("Searsusername").ToString()) + "&password=" + Server.UrlEncode(AppLogic.AppConfigs("SearsPassword").ToString()) + "");
                    myRequest.Method = "PUT";
                    myRequest.ContentType = "application/xml";
                    myRequest.ContentLength = arr.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(arr, 0, arr.Length);
                    newStream.Close();
                    HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();

                    Encoding enc = System.Text.Encoding.GetEncoding(1252);
                    StreamReader loResponseStream = new StreamReader(response.GetResponseStream(), enc);
                    try
                    {
                        string ResponseNew = loResponseStream.ReadToEnd();
                        loResponseStream.Close();
                        response.Close();

                        lblMsg.Text = "File uploaded Successfully.";
                    }
                    catch
                    {
                        lblMsg.ForeColor = System.Drawing.Color.Green;
                        lblMsg.Text = "File uploaded Successfully.";
                        //lblMsg.Text = ex.Message.ToString();
                        loResponseStream.Close();
                        response.Close();
                    }
                }
                catch (Exception ex)
                {
                    lblMsg.Text = ex.Message.ToString();
                }
            }
        }
        #endregion

        #region For Update Price
        /// <summary>
        ///  Sears Product Price Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearsPrice_Click(object sender, EventArgs e)
        {
            ItemPriceSears();
        }

        /// <summary>
        /// Update Price in Sears
        /// </summary>
        private void ItemPriceSears()
        {
            AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            String strProductPath = AppLogic.AppConfigs("ImagePathProduct").Trim().Replace("product/", "");
            StreamReader strXml = File.OpenText(Server.MapPath(strProductPath.Trim()) + "/ProductXML/pricing.xml");
            string xmlFinal = strXml.ReadToEnd();

            SQLAccess objSql = new SQLAccess();
            DataSet dsData = new DataSet();
            string strIds = "0";
            foreach (GridViewRow gr in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)gr.FindControl("chkSelect");
                HiddenField lblid = (HiddenField)gr.FindControl("hdnProductid");
                if (chk.Checked)
                {
                    strIds += "," + lblid.Value.ToString();
                }
            }

            dsData = objSql.GetDs("SELECT SKU,isnull(price,0) as price, case when isnull(saleprice,0)=0 then isnull(price,0) else saleprice end as Saleprice,isnull(MapPriceIndicator,'strict') as MapPriceIndicator,AvaliableStartDate,AvailableEndDate,isnull(SurCharge,0) as SurCharge FROM tb_Product WHERE ProductId in (" + strIds + ")");
            string strStringXMl = "";

            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["SKU"].ToString()))
                    {
                        strStringXMl += "<item item-id=\"" + dsData.Tables[0].Rows[i]["SKU"].ToString().Trim() + "\">\r\n";
                    }

                    strStringXMl += "<standard-price>" + string.Format("{0:0.00}", Convert.ToDecimal(dsData.Tables[0].Rows[i]["price"].ToString().Trim())) + "</standard-price>\r\n";
                    strStringXMl += "<sale>\r\n";
                    strStringXMl += "<sale-price>" + string.Format("{0:0.00}", Convert.ToDecimal(dsData.Tables[0].Rows[i]["Saleprice"].ToString().Trim())) + "</sale-price>\r\n";

                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["AvaliableStartDate"].ToString()))
                    {
                        strStringXMl += "<sale-start-date>" + string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dsData.Tables[0].Rows[i]["AvaliableStartDate"].ToString())) + "</sale-start-date>\r\n";
                    }
                    else
                    {
                        strStringXMl += "<sale-start-date></sale-start-date>\r\n";
                    }
                    if (!string.IsNullOrEmpty(dsData.Tables[0].Rows[i]["AvailableEndDate"].ToString()))
                    {
                        strStringXMl += "<sale-end-date>" + string.Format("{0:yyyy-MM-dd}", Convert.ToDateTime(dsData.Tables[0].Rows[i]["AvailableEndDate"].ToString())) + "</sale-end-date>\r\n";
                    }
                    else
                    {
                        strStringXMl += "<sale-end-date></sale-end-date>\r\n";
                    }

                    strStringXMl += "</sale>\r\n";
                    //strStringXMl += "<handling-fee>" + string.Format("{0:0}", Convert.ToDecimal(dsData.Tables[0].Rows[i]["SurCharge"].ToString().Trim())) + "</handling-fee>\r\n";
                    strStringXMl += "<map-price-indicator>" + dsData.Tables[0].Rows[i]["MapPriceIndicator"].ToString().ToLower() + "</map-price-indicator>\r\n";
                    strStringXMl += "</item>\r\n";
                }
            }
            xmlFinal = xmlFinal.Replace("###Price####", strStringXMl.ToString());
            StreamWriter writer = null;
            string strfilename = "";

            try
            {
                strfilename = Server.MapPath(strProductPath.Trim()) + "/ProductXML/SearsPrice" + DateTime.Now.Ticks.ToString() + ".xml";
                writer = new StreamWriter(strfilename, true);
                writer.Write(xmlFinal);
            }
            catch { }
            finally
            {
                if (writer != null)
                    writer.Close();
            }


            try
            {
                StreamReader str = File.OpenText(strfilename);
                string xml = str.ReadToEnd();
                byte[] arr = System.Text.Encoding.ASCII.GetBytes(xml);

                // Prepare web request...
                HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://seller.marketplace.sears.com/SellerPortal/api/pricing/fbm/v4?email=" + AppLogic.AppConfigs("Searsusername") + "&password=" + AppLogic.AppConfigs("SearsPassword") + "");
                myRequest.Method = "PUT";
                myRequest.ContentType = "application/xml";
                myRequest.ContentLength = arr.Length;
                Stream newStream = myRequest.GetRequestStream();
                // Send the data.
                newStream.Write(arr, 0, arr.Length);
                newStream.Close();
                HttpWebResponse response = (HttpWebResponse)myRequest.GetResponse();
                Encoding enc = System.Text.Encoding.GetEncoding(1252);
                StreamReader loResponseStream = new StreamReader(response.GetResponseStream(), enc);
                try
                {
                    string Response = loResponseStream.ReadToEnd();
                    loResponseStream.Close();
                    response.Close();
                    lblMsg.Text = "File uploaded Successfully.";
                }
                catch
                {

                    lblMsg.Text = "File uploaded Successfully.";
                    loResponseStream.Close();
                    response.Close();
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message.ToString();
            }

        }
        #endregion

        #endregion

        #region Over Stock Functions

        #region For Update Inventory
        protected void btnOverStockUpdate_Click(object sender, EventArgs e)
        {
            UpdaeoverStockQuantity();
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "success", "jAlert('Inventory Updated Successfully.','Success');", true);
        }
        protected void btnOverStockAllUpdate_Click(object sender, EventArgs e)
        {
            UpdaeoverStockAllQuantity();
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "success", "jAlert('Inventory Updated Successfully.','Success');", true);
        }
        private void UpdaeoverStockQuantity()
        {
            ServicePointManager.ServerCertificateValidationCallback = new
             RemoteCertificateValidationCallback
(
 delegate { return true; }
);

            Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
            string OverstockUserName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockUserName' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));
            string OverstockPassword = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockPassword' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));
            // String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            transactionCommand.Append("<supplierInventoryMessage xmlns=\"api.supplieroasis.com\">");



            foreach (GridViewRow gr in grdProduct.Rows)
            {
                Label lblSku = (Label)gr.FindControl("lblSKU");
                Label lblInventory = (Label)gr.FindControl("lblInventory");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
                Label lblOptionSku = (Label)gr.FindControl("lblOptionSku");
                HiddenField hdnUPC = (HiddenField)gr.FindControl("hdnUPC");

                if (chkSelect.Checked == true)
                {
                    transactionCommand.Append("<supplierInventory>");
                    string hemmInventory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + hdnUPC.Value.ToString() + "','" + lblSku.Text.ToString() + "'," + ddlStore.SelectedValue.ToString() + ")"));
                    transactionCommand.Append("<partnerSku>" + lblSku.Text.ToString() + "</partnerSku>");
                    transactionCommand.Append("<supplierWarehouseQuantity>");
                    transactionCommand.Append("<warehouseName><code>Exclusive</code></warehouseName>");
                    transactionCommand.Append("<quantity>" + hemmInventory.ToString() + "</quantity>");
                    transactionCommand.Append("<timestamp>" + string.Format("{0:yyyy-MM-ddThh:mm:ss}", Convert.ToDateTime(DateTime.Now)) + "</timestamp>");
                    transactionCommand.Append("<barcode>" + lblSku.Text.ToString() + "</barcode>");
                    transactionCommand.Append("</supplierWarehouseQuantity>");
                    transactionCommand.Append("</supplierInventory>");
                }
            }


            transactionCommand.Append("</supplierInventoryMessage>");
            //System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            //byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());


            //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            //myRequest.Method = "POST";
            //myRequest.Timeout = 300000;
            //myRequest.Headers.Add("SapiMethodName", "UpdateQuantity");
            //// myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
            //myRequest.ContentType = "application/xml";
            //myRequest.ContentLength = data.Length;
            //Stream newStream = myRequest.GetRequestStream();
            //// Send the data.
            //newStream.Write(data, 0, data.Length);
            //newStream.Close();
            //// get the response
            //WebResponse myResponse;
            //String rawResponseString = String.Empty;
            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

            //String AuthServer = "https://sapiqa.overstock.com/api";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://api.supplieroasis.com/inventory?jobName=");
            myRequest.Method = "POST";

            myRequest.Headers.Add("Authorization", string.Format("Basic {0}", GetAuthorization(OverstockUserName, OverstockPassword)));
            myRequest.ContentType = "application/xml; charset=UTF-8";

            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/Updateproduct-" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch (Exception Ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = Ex.Message.ToString() + " " + Ex.StackTrace.ToString();
                }
                myResponse.Close();
            }
            catch (Exception Ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = Ex.Message.ToString() + " " + Ex.StackTrace.ToString();
            }
        }
        private static string GetAuthorization(string User, string Password)
        {
            UTF8Encoding utf8encoder = new UTF8Encoding(false, true);

            return Convert.ToBase64String(utf8encoder.GetBytes(string.Format("{0}:{1}", User, Password)));
        }
        private void UpdaeoverStockAllQuantity()
        {
            ServicePointManager.ServerCertificateValidationCallback = new
             RemoteCertificateValidationCallback
(
 delegate { return true; }
);
            Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
            string OverstockUserName = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockUserName' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));
            string OverstockPassword = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='OverstockPassword' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));
            // String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));

            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"UTF-8\"?>");
            transactionCommand.Append("<supplierInventoryMessage xmlns=\"api.supplieroasis.com\">");

            DataSet dsallProduct = new DataSet();
            dsallProduct = CommonComponent.GetCommonDataSet("SELECT SKU,UPC,isnull(OptionSku,'') as OptionSku FROM tb_product WHERE Isnull(Active,0)=1 and isnull(Deleted,0)=0 and isnull(UPC,'') <>'' and storeId=" + ddlStore.SelectedValue.ToString() + "");
            //foreach (GridViewRow gr in grdProduct.Rows)
            //{
            //Label lblSku = (Label)gr.FindControl("lblSKU");
            //Label lblInventory = (Label)gr.FindControl("lblInventory");
            //CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");
            //Label lblOptionSku = (Label)gr.FindControl("lblOptionSku");
            //HiddenField hdnUPC = (HiddenField)gr.FindControl("hdnUPC");

            //if (chkSelect.Checked == true)
            //{
            if (dsallProduct != null && dsallProduct.Tables.Count > 0 && dsallProduct.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsallProduct.Tables[0].Rows.Count; i++)
                {

                    if (!string.IsNullOrEmpty(dsallProduct.Tables[0].Rows[i]["OptionSku"].ToString().Trim()))
                    {

                        transactionCommand.Append("<supplierInventory>");
                        string hemmInventory = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dsallProduct.Tables[0].Rows[i]["UPC"].ToString() + "','" + dsallProduct.Tables[0].Rows[i]["SKU"].ToString() + "'," + ddlStore.SelectedValue.ToString() + ")"));
                        transactionCommand.Append("<partnerSku>" + dsallProduct.Tables[0].Rows[i]["SKU"].ToString() + "</partnerSku>");
                        transactionCommand.Append("<supplierWarehouseQuantity>");
                        transactionCommand.Append("<warehouseName><code>Exclusive</code></warehouseName>");
                        transactionCommand.Append("<quantity>" + hemmInventory.ToString() + "</quantity>");
                        transactionCommand.Append("<timestamp>" + string.Format("{0:yyyy-MM-ddThh:mm:ss}", Convert.ToDateTime(DateTime.Now)) + "</timestamp>");
                        transactionCommand.Append("<barcode>" + dsallProduct.Tables[0].Rows[i]["SKU"].ToString() + "</barcode>");
                        transactionCommand.Append("</supplierWarehouseQuantity>");
                        transactionCommand.Append("</supplierInventory>");


                    }
                }

            }
            transactionCommand.Append("</supplierInventoryMessage>");


            //System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            //byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());


            //HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
            //myRequest.Method = "POST";
            //myRequest.Timeout = 300000;
            //myRequest.Headers.Add("SapiMethodName", "UpdateQuantity");
            //// myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
            //myRequest.ContentType = "application/xml";
            //myRequest.ContentLength = data.Length;
            //Stream newStream = myRequest.GetRequestStream();
            //// Send the data.
            //newStream.Write(data, 0, data.Length);
            //newStream.Close();
            //// get the response
            //WebResponse myResponse;
            //String rawResponseString = String.Empty;
            System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
            byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());

            //String AuthServer = "https://sapiqa.overstock.com/api";
            HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create("https://api.supplieroasis.com/inventory?jobName=");
            myRequest.Method = "POST";

            myRequest.Headers.Add("Authorization", string.Format("Basic {0}", GetAuthorization(OverstockUserName, OverstockPassword)));
            myRequest.ContentType = "application/xml; charset=UTF-8";

            myRequest.ContentLength = data.Length;
            Stream newStream = myRequest.GetRequestStream();
            // Send the data.
            newStream.Write(data, 0, data.Length);
            newStream.Close();
            // get the response
            WebResponse myResponse;
            String rawResponseString = String.Empty;
            try
            {
                myResponse = myRequest.GetResponse();
                using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                {
                    rawResponseString = sr.ReadToEnd();
                    // Close and clean up the StreamReader
                    sr.Close();
                }
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(rawResponseString);
                ds.ReadXml(new XmlNodeReader(xDoc));
                try
                {
                    if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                    {
                        Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                    }
                    ds.WriteXml(Server.MapPath("/OverstockOrder/Updateproduct-" + DateTime.Now.Ticks.ToString() + ".xml"));
                }
                catch (Exception Ex)
                {
                    lblMsg.Visible = true;
                    lblMsg.Text = Ex.Message.ToString() + " " + Ex.StackTrace.ToString();
                }
                myResponse.Close();
            }
            catch (Exception Ex)
            {
                lblMsg.Visible = true;
                lblMsg.Text = Ex.Message.ToString() + " " + Ex.StackTrace.ToString();
            }
        }
        #endregion

        #region  For Upload Product


        protected void btnOverStockProduct_Click(object sender, EventArgs e)
        {
            UploadOverStockProduct();
            Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "success", "jAlert('Product Uploaded Successfully.','Success');", true);
        }

        private void UploadOverStockProduct()
        {
            Solution.Data.SQLAccess objdb = new Solution.Data.SQLAccess();
            DataSet dsUploadProducts = new DataSet();
            string MerchantKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='MerchantKey' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));
            string AuthenticationKey = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthenticationKey' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));
            String AuthServer = Convert.ToString(objdb.ExecuteScalarQuery("Select configvalue from tb_AppConfig  where configname='AuthServer' AND Storeid=" + ddlStore.SelectedValue.ToString() + ""));

            String strProductPath = Convert.ToString(AppLogic.AppConfigs("ImagePathProduct"));
            ASCIIEncoding encoding = new ASCIIEncoding();
            StringBuilder transactionCommand = new StringBuilder(4096);
            DataSet ds = new DataSet();
            transactionCommand.Append("<?xml version=\"1.0\" encoding=\"ISO-8859-1\"?>");
            transactionCommand.Append("<Request xmlns=\"http://www.overstock.com/shoppingApi\">");
            transactionCommand.Append("<MerchantKey>" + MerchantKey + "</MerchantKey>");
            transactionCommand.Append("<AuthenticationKey>" + AuthenticationKey + "</AuthenticationKey>");
            transactionCommand.Append("<SubmitNewProduct>");

            String strProductIDs = "";
            foreach (GridViewRow gr in grdProduct.Rows)
            {
                HiddenField lblProductID = (HiddenField)gr.FindControl("hdnProductid");
                CheckBox chkSelect = (CheckBox)gr.FindControl("chkSelect");

                if (chkSelect.Checked == true)
                {
                    strProductIDs += lblProductID.Value.Trim() + ",";
                }
            }

            if (strProductIDs != "")
            {
                dsUploadProducts = CommonComponent.GetCommonDataSet(@"SELECT ISNULL(dbo.tb_Category.SearsCategoryID,0) AS SearsCategoryID,dbo.tb_Product.Name,dbo.tb_Product.ShortName,dbo.tb_Product.Summary,dbo.tb_Product.[Description],ISNULL(Price,0) AS Price,
                                    CASE WHEN ISNULL(SalePrice,0)=0 THEN ISNULL(Price,0) ELSE ISNULL(SalePrice,0) END AS SalePrice,ISNULL([Length],0) AS [Length],ISNULL([Weight],0) AS [Weight],ISNULL(Height,0)AS Height,
                                    SourceZipCode,ProductCondition ,SKU,ISNULL(dbo.tb_Product.UPC,'') AS UPC,ISNULL(Inventory,0) AS Inventory,ISNULL([Weight],0) AS [Weight],dbo.tb_Manufacture.Name AS ManufatureName,PartNumber,Brand,WarrantyProvider,WarrantyDescription,WarrantyContactPhoneNumber,
                                    Materials,ISNULL(dbo.tb_Product.ImageName,'') AS ImageName 
                                    FROM dbo.tb_Product LEFT OUTER JOIN dbo.tb_ProductCategory ON dbo.tb_Product.ProductID = dbo.tb_ProductCategory.ProductID LEFT OUTER JOIN dbo.tb_Category ON dbo.tb_ProductCategory.CategoryID = dbo.tb_Category.CategoryID
                                    LEFT OUTER JOIN dbo.tb_Manufacture ON dbo.tb_Manufacture.ManufactureID=dbo.tb_Product.ManufactureID
                                    WHERE dbo.tb_Product.ProductID IN (" + strProductIDs.Trim(',') + ")");

                if (dsUploadProducts != null && dsUploadProducts.Tables.Count > 0 && dsUploadProducts.Tables[0].Rows.Count > 0)
                {
                    for (int p = 0; p < dsUploadProducts.Tables[0].Rows.Count; p++)
                    {
                        transactionCommand.Append("<Product>");
                        transactionCommand.Append("<Name>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Name"]) + "</Name>");
                        transactionCommand.Append("<ShortName>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["ShortName"]) + "</ShortName>");
                        transactionCommand.Append("<Summary>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Summary"]) + "</Summary>");
                        //transactionCommand.Append("<Description><![CDATA[" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Description"]) + "]]></Description>");
                        transactionCommand.Append("<Description>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Name"]) + "</Description>");
                        transactionCommand.Append("<CategoryId>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["SearsCategoryID"]) + "</CategoryId>");
                        transactionCommand.Append("<Pricing>");
                        transactionCommand.Append("<MSRP>" + Convert.ToDecimal(dsUploadProducts.Tables[0].Rows[p]["Price"]).ToString("0.00") + "</MSRP>");
                        //transactionCommand.Append("<MSRPSalesLocation></MSRPSalesLocation>");
                        transactionCommand.Append("<StreetPrice>" + Convert.ToDecimal(dsUploadProducts.Tables[0].Rows[p]["Price"]).ToString("0.00") + "</StreetPrice>");
                        transactionCommand.Append("<SuggestedSellingPrice>" + Convert.ToDecimal(dsUploadProducts.Tables[0].Rows[p]["SalePrice"]).ToString("0.00") + "</SuggestedSellingPrice>");
                        transactionCommand.Append("</Pricing>");
                        transactionCommand.Append("<PackageDimensions>");
                        transactionCommand.Append("<Length>1</Length>");
                        transactionCommand.Append("<Width>1</Width>");
                        transactionCommand.Append("<Height>1</Height>");
                        transactionCommand.Append("</PackageDimensions>");
                        //transactionCommand.Append("<SourceZipCode>23456</SourceZipCode>");
                        //transactionCommand.Append("<SourceZipCode/>");" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["SourceZipCode"]) + "
                        // transactionCommand.Append("<LTL>FALSE</LTL>");
                        // transactionCommand.Append("<ShipAlone>false</ShipAlone>");
                        //transactionCommand.Append("<CountryOfOrigin>USA</CountryOfOrigin>");
                        transactionCommand.Append("<Condition>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["ProductCondition"]) + "</Condition>");
                        transactionCommand.Append("<Options>");
                        transactionCommand.Append("<Option>");
                        transactionCommand.Append("<VendorSku>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["SKU"]) + "</VendorSku>");
                        //transactionCommand.Append("<UPC>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["UPC"]) + "</UPC>");
                        //transactionCommand.Append("<Description><![CDATA[" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Description"]) + "]]></Description>");
                        transactionCommand.Append("<Description>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Name"]) + "</Description>");
                        transactionCommand.Append("<Cost>" + Convert.ToDecimal(dsUploadProducts.Tables[0].Rows[p]["Price"]).ToString("0.00") + "</Cost>");
                        transactionCommand.Append("<Quantity>" + Convert.ToString(CommonComponent.GetScalarCommonData("SELECT dbo.Producthamming_Scalar('" + dsUploadProducts.Tables[0].Rows[p]["UPC"].ToString() + "','" + dsUploadProducts.Tables[0].Rows[p]["SKU"].ToString() + "'," + ddlStore.SelectedValue.ToString() + ")")) + "</Quantity>");
                        transactionCommand.Append("<Weight>" + Convert.ToDecimal(dsUploadProducts.Tables[0].Rows[p]["Weight"]).ToString("0.0") + "</Weight>");
                        transactionCommand.Append("</Option>");
                        transactionCommand.Append("</Options>");

                        transactionCommand.Append("<Manufacturer>");
                        transactionCommand.Append("<Name>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["ManufatureName"]) + "</Name>");
                        transactionCommand.Append("<PartNumber>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["PartNumber"]) + "</PartNumber>");
                        transactionCommand.Append("</Manufacturer>");

                        transactionCommand.Append("<Brand>0</Brand>");

                        transactionCommand.Append("<Warranty>");
                        transactionCommand.Append("<Provider>N</Provider>");
                        transactionCommand.Append("<Description>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["WarrantyDescription"]) + "</Description>");
                        transactionCommand.Append("<ContactPhoneNumber>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["WarrantyContactPhoneNumber"]) + "</ContactPhoneNumber>");
                        transactionCommand.Append("</Warranty>");


                        transactionCommand.Append("<Specifications>");
                        transactionCommand.Append("<Materials>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Materials"]) + "</Materials>");
                        transactionCommand.Append("</Specifications>");

                        transactionCommand.Append("<ImagesURLS>");

                        String strImageName = Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["ImageName"]);
                        String strThumbnail = strProductPath + "Icon/" + strImageName.Trim();
                        String strMain = strProductPath + "Medium/" + strImageName.Trim();
                        String strLarge = strProductPath + "Large/" + strImageName.Trim();

                        if (File.Exists(Server.MapPath(strThumbnail)))
                        {
                            transactionCommand.Append("<Thumbnail>" + AppLogic.AppConfigs("LIVE_SERVER") + strThumbnail + "</Thumbnail>");
                        }
                        else
                        {
                            transactionCommand.Append("<Thumbnail></Thumbnail>");
                        }
                        if (File.Exists(Server.MapPath(strMain)))
                        {
                            transactionCommand.Append("<Main>" + AppLogic.AppConfigs("LIVE_SERVER") + strMain + "</Main>");
                        }
                        else
                        {
                            transactionCommand.Append("<Main></Main>");
                        }
                        if (File.Exists(Server.MapPath(strLarge)))
                        {
                            transactionCommand.Append("<Large>" + AppLogic.AppConfigs("LIVE_SERVER") + strLarge + "</Large>");
                        }
                        else
                        {
                            transactionCommand.Append("<Large></Large>");
                        }
                        transactionCommand.Append("</ImagesURLS>");

                        transactionCommand.Append("</Product>");


                        // transactionCommand.Append("<Product>");
                        // transactionCommand.Append("<Name>Three Dots Bandeau Tunic Sides</Name>");
                        // transactionCommand.Append("<ShortName>Three Dots Bandeau Tunic Sides</ShortName>");
                        // transactionCommand.Append("<Summary>Bandeau Tunic W/Shirred Sides Visco</Summary>");
                        // //transactionCommand.Append("<Description><![CDATA[" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Description"]) + "]]></Description>");
                        // transactionCommand.Append("<Description>Color: Orange. Bandeau Tunic with shired sides.</Description>");
                        // transactionCommand.Append("<CategoryId>12844</CategoryId>");
                        // transactionCommand.Append("<Pricing>");
                        // transactionCommand.Append("<MSRP>74.0000</MSRP>");
                        //  transactionCommand.Append("<MSRPSalesLocation>Finnish Line</MSRPSalesLocation>");
                        // transactionCommand.Append("<StreetPrice>74</StreetPrice>");
                        // transactionCommand.Append("<SuggestedSellingPrice>37</SuggestedSellingPrice>");
                        // transactionCommand.Append("</Pricing>");
                        // transactionCommand.Append("<PackageDimensions>");
                        // transactionCommand.Append("<Length>24</Length>");
                        // transactionCommand.Append("<Width>19</Width>");
                        // transactionCommand.Append("<Height>1</Height>");
                        // transactionCommand.Append("</PackageDimensions>");
                        //transactionCommand.Append("<SourceZipCode>30168</SourceZipCode>");
                        // //transactionCommand.Append("<SourceZipCode/>");" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["SourceZipCode"]) + "
                        // // transactionCommand.Append("<LTL>FALSE</LTL>");
                        // // transactionCommand.Append("<ShipAlone>false</ShipAlone>");
                        //  transactionCommand.Append("<CountryOfOrigin>US</CountryOfOrigin>");
                        // transactionCommand.Append("<Condition>NEW</Condition>");
                        // transactionCommand.Append("<Options>");
                        // transactionCommand.Append("<Option>");
                        // transactionCommand.Append("<VendorSku>EM0W-028:ORG</VendorSku>");
                        // //transactionCommand.Append("<UPC>" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["UPC"]) + "</UPC>");
                        // //transactionCommand.Append("<Description><![CDATA[" + Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["Description"]) + "]]></Description>");
                        // transactionCommand.Append("<Description>Parent</Description>");
                        // transactionCommand.Append("<Cost>74.0</Cost>");
                        // transactionCommand.Append("<Quantity>5</Quantity>");
                        // transactionCommand.Append("<Weight>1</Weight>");
                        // transactionCommand.Append("</Option>");
                        // transactionCommand.Append("</Options>");

                        // transactionCommand.Append("<Manufacturer>");
                        // transactionCommand.Append("<Name>Three Dots</Name>");
                        // transactionCommand.Append("<PartNumber>EM0W-028</PartNumber>");
                        // transactionCommand.Append("</Manufacturer>");

                        // transactionCommand.Append("<Brand>4985</Brand>");

                        // transactionCommand.Append("<Warranty>");
                        // transactionCommand.Append("<Provider>P</Provider>");
                        // transactionCommand.Append("<Description>No Warranty</Description>");
                        // transactionCommand.Append("<ContactPhoneNumber>.</ContactPhoneNumber>");
                        // transactionCommand.Append("</Warranty>");


                        // transactionCommand.Append("<Specifications>");
                        // transactionCommand.Append("<Materials>Cotton</Materials>");
                        // transactionCommand.Append("</Specifications>");

                        // transactionCommand.Append("<ImagesURLS>");

                        // String strImageName = Convert.ToString(dsUploadProducts.Tables[0].Rows[p]["ImageName"]);
                        // String strThumbnail = strProductPath + "Icon/" + strImageName.Trim();
                        // String strMain = strProductPath + "Medium/" + strImageName.Trim();
                        // String strLarge = strProductPath + "Large/" + strImageName.Trim();

                        // if (File.Exists(Server.MapPath(strThumbnail)))
                        // {
                        //     transactionCommand.Append("<Thumbnail>" + AppLogic.AppConfigs("LIVE_SERVER") + strThumbnail + "</Thumbnail>");
                        // }
                        // else
                        // {
                        //     transactionCommand.Append("<Thumbnail>http://images.overstock.com/examples/Images/4/EM0W028_ORG_Thumb.jpg</Thumbnail>");
                        // }
                        // if (File.Exists(Server.MapPath(strMain)))
                        // {
                        //     transactionCommand.Append("<Main>" + AppLogic.AppConfigs("LIVE_SERVER") + strMain + "</Main>");
                        // }
                        // else
                        // {
                        //     transactionCommand.Append("<Main>http://images.overstock.com/examples/Images/4/EM0W028_ORG_Main.jpg</Main>");
                        // }
                        // if (File.Exists(Server.MapPath(strLarge)))
                        // {
                        //     transactionCommand.Append("<Large>" + AppLogic.AppConfigs("LIVE_SERVER") + strLarge + "</Large>");
                        // }
                        // else
                        // {
                        //     transactionCommand.Append("<Large>http://images.overstock.com/examples/Images/4/EM0W028_ORG_Large.jpg</Large>");
                        // }
                        // transactionCommand.Append("</ImagesURLS>");

                        // transactionCommand.Append("</Product>");
                    }

                    transactionCommand.Append("</SubmitNewProduct>");
                    transactionCommand.Append("</Request>");

                    System.Text.Encoding iso_8859_1 = System.Text.Encoding.GetEncoding("iso-8859-1");
                    byte[] data = iso_8859_1.GetBytes(transactionCommand.ToString());


                    HttpWebRequest myRequest = (HttpWebRequest)WebRequest.Create(AuthServer);
                    myRequest.Method = "POST";
                    myRequest.Timeout = 300000;
                    myRequest.Headers.Add("SapiMethodName", "SubmitNewProduct");
                    // myRequest.Credentials = new NetworkCredential("EXCLUSIVE FABRICS", "password");
                    myRequest.ContentType = "application/xml";
                    myRequest.ContentLength = data.Length;
                    Stream newStream = myRequest.GetRequestStream();
                    // Send the data.
                    newStream.Write(data, 0, data.Length);
                    newStream.Close();
                    // get the response
                    WebResponse myResponse;
                    String rawResponseString = String.Empty;
                    try
                    {
                        myResponse = myRequest.GetResponse();
                        using (StreamReader sr = new StreamReader(myResponse.GetResponseStream()))
                        {
                            rawResponseString = sr.ReadToEnd();
                            // Close and clean up the StreamReader
                            sr.Close();
                        }
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(rawResponseString);
                        ds.ReadXml(new XmlNodeReader(xDoc));
                        try
                        {
                            if (!Directory.Exists(Server.MapPath("/OverstockOrder")))
                            {
                                Directory.CreateDirectory(Server.MapPath("/OverstockOrder"));
                            }
                            ds.WriteXml(Server.MapPath("/OverstockOrder/SubmitNewProduct-" + DateTime.Now.Ticks.ToString() + ".xml"));
                        }
                        catch { }
                        myResponse.Close();
                    }
                    catch
                    {
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "NoProductAvailableForUpload", "jAlert('No Product Available For Upload','Message')", true);
                    return;
                }
            }
        }
        #endregion

        #region For Update Price
        protected void btnOverStockPrice_Click(object sender, EventArgs e)
        {
            UpdateOverStockPrice();
        }

        private void UpdateOverStockPrice()
        {
        }
        #endregion

        #endregion

        /// <summary>
        /// Button click to Approve Data Verification
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnApprove_Click(object sender, ImageClickEventArgs e)
        {
            if (grdProduct.Rows.Count > 0 && ddlStatus.SelectedItem.Text.ToString().ToLower() == "dataverify")
            {
                Int32 AdminId = 0;
                if (Session["AdminID"] != null)
                {
                    AdminId = Convert.ToInt32(Session["AdminID"].ToString());
                }
                int Cnt = 0;
                for (int i = 0; i < grdProduct.Rows.Count; i++)
                {
                    HiddenField hdnProductid = (HiddenField)grdProduct.Rows[i].FindControl("hdnProductid");
                    CheckBox chkSelect = (CheckBox)grdProduct.Rows[i].FindControl("chkSelect");
                    if (chkSelect.Checked)
                    {
                        Cnt++;
                        CommonComponent.ExecuteCommonData("Update tb_Product set IsDataVerify=1,DataVerifyBy=" + AdminId + ",IsDataVerifyOn=getdate() where ProductID= " + hdnProductid.Value.ToString() + "");
                    }
                }
                if (Cnt > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + Cnt + " Item(s) Approved Successfully.', 'Message');", true);
                    btnSearch_Click(null, null);
                }
                else { Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Item(s) Approved Successfully.', 'Message');", true); }
            }
        }

        protected void btnApproveTemp_Click(object sender, EventArgs e)
        {
            btnApprove_Click(null, null);
        }

        //Export Product

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

        protected void btnExport_Click(object sender, EventArgs e)
        {
            //if (ViewState["ExportProductIds"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
            //    return;
            //}

            ViewState["LastExportFileName"] = null;
            if (ddlStore.SelectedItem.Text.ToString().Trim() != "")
            {
                if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("wayfailr") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("lnt") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("bellacor") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("kohl") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("houzz") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("atg") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else if ((ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1))
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore", "jAlert('Please Select Store','Message');", true);
                return;
            }
        }

        private void GenerateWayfailrProducts(string StoreName)
        {
            string StrProductIds = "";
            if (ViewState["ExportProductIds"] != null)
            {
                StrProductIds = Convert.ToString(ViewState["ExportProductIds"]);
                StrProductIds = StrProductIds.Substring(0, StrProductIds.Length - 1);
            }

            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();
            SecurityComponent objsec = new SecurityComponent();
            dsorder = CommonComponent.GetCommonDataSet("EXEC [usp_PartnerProductExport] '" + StoreName.ToString().Trim().ToLower() + "','" + StrProductIds + "'");
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
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                return;
            }

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    string StrStorename = "";
                    if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("wayfailr") > -1)
                    {
                        StrStorename = "wayfailr";
                    }
                    else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("lnt") > -1)
                    {
                        StrStorename = "lnt";
                    }
                    else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("bellacor") > -1)
                    {
                        StrStorename = "bellacor";
                    }
                    else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("kohl") > -1)
                    {
                        StrStorename = "kohl";
                    }
                    else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("houzz") > -1)
                    {
                        StrStorename = "Houzz";
                    }
                    else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("atg") > -1)
                    {
                        StrStorename = "ATG";
                    }
                    else if ((ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1))
                    {
                        StrStorename = "OverStock";
                    }
                    String FileName = StrStorename.ToString() + "_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                    ViewState["LastExportFileName"] = null;
                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);

                    ViewState["LastExportFileName"] = FileName.ToString();
                    WriteFile(sb.ToString(), FilePath);
                }
                if (ViewState["LastExportFileName"] != null)
                {
                    DownloadProductExport();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Data Exported Successfully.','Message');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                    return;
                }
            }
        }

        protected void DownloadProductExport()
        {
            if (ViewState["LastExportFileName"] != null)
            {
                String FilePath = Server.MapPath("~/Admin/Files/" + ViewState["LastExportFileName"].ToString());
                if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                if (File.Exists(FilePath))
                {
                    Response.Clear();
                    Response.ClearContent();
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["LastExportFileName"].ToString());
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('File not found.','Message');", true);
                return;
            }
        }

        protected void UploadProductExported()
        {
            // Code for upload at ftp
            if (ViewState["LastExportFileName"] != null)
            {
                String FilePath = Server.MapPath("~/Admin/Files/" + ViewState["LastExportFileName"].ToString());
                if (File.Exists(FilePath) && ddlStore.SelectedValue.ToString() != "")
                {
                    string StrFTPFolderpath = "";
                    string ftphost = "";
                    string ftpusername = "";
                    string ftppassword = "";

                    ftphost = Convert.ToString(CommonComponent.GetScalarCommonData("SElect top 1 ISNULL(ConfigValue,'') as ConfigValue from tb_appconfig Where StoreID=" + ddlStore.SelectedValue.ToString() + " and ConfigName ='FTPHost'"));
                    ftpusername = Convert.ToString(CommonComponent.GetScalarCommonData("SElect top 1 ISNULL(ConfigValue,'') as ConfigValue from tb_appconfig Where StoreID=" + ddlStore.SelectedValue.ToString() + " and ConfigName ='FTPusername'"));
                    ftppassword = Convert.ToString(CommonComponent.GetScalarCommonData("SElect top 1 ISNULL(ConfigValue,'') as ConfigValue from tb_appconfig Where StoreID=" + ddlStore.SelectedValue.ToString() + " and ConfigName ='FTPpassword'"));
                    StrFTPFolderpath = Convert.ToString(CommonComponent.GetScalarCommonData("SElect top 1 ISNULL(ConfigValue,'') as ConfigValue from tb_appconfig Where StoreID=" + ddlStore.SelectedValue.ToString() + " and ConfigName ='FTPfoldername'"));

                    if (!string.IsNullOrEmpty(ftphost.ToString().Trim()) && !string.IsNullOrEmpty(ftpusername.ToString().Trim()) && !string.IsNullOrEmpty(ftppassword.ToString().Trim()))
                    {
                        UplaodFileOnftp(FilePath.ToString(), ftphost, ftpusername, ftppassword, StrFTPFolderpath.ToString());
                    }
                }
            }
        }

        private void UplaodFileOnftp(string filefullpathname, string ftphost, string ftpusername, string ftppassword, string ftpuploadpath)
        {

            string CompleteDPath = "ftp://" + ftphost + "/" + ftpuploadpath + "/";
            string FileName = filefullpathname;
            string ftpServerIP = ftphost;
            string ftpUserID = ftpusername;
            string ftpPassword = ftppassword;

            FileInfo fileInf = new FileInfo(FileName);

            string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;

            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided

            //reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/httpdocs/client/images/BuyOrderFiles/" + fileInf.Name));
            if (ftpuploadpath == "")
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileInf.Name));
            }
            else
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + ftpuploadpath + "/" + fileInf.Name));
            }


            // Provide the WebPermission Credintials

            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);



            // By default KeepAlive is true, where the control connection is not closed

            // after a command is executed.

            reqFTP.KeepAlive = false;

            reqFTP.UsePassive = false;

            // Specify the command to be executed.

            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;



            // Specify the data transfer type.

            reqFTP.UseBinary = true;



            // Notify the server about the size of the uploaded file

            reqFTP.ContentLength = fileInf.Length;



            // The buffer size is set to 2kb

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;



            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded

            FileStream fs = fileInf.OpenRead();



            try
            {

                // Stream to which the file to be upload is written

                Stream strm = reqFTP.GetRequestStream();



                // Read from the file stream 2kb at a time

                contentLen = fs.Read(buff, 0, buffLength);



                // Till Stream content ends

                while (contentLen != 0)
                {

                    // Write Content from the file stream to the FTP Upload Stream

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }



                // Close the file stream and the Request Stream

                strm.Close();

                fs.Close();

            }

            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message, "Upload Error");

            }
        }

        //protected void btnExportPrice_Click(object sender, EventArgs e)
        //{


        //    CommonComponent clsCommon = new CommonComponent();
        //    DataView dvCust = new DataView();
        //    //  dvCust = clsCommon.GetCustomerExport(txtSearch.Text.Trim(), ddlStore.SelectedValue).Tables[0].DefaultView;
        //    //  DataSet ds = CommonComponent.GetCommonDataSet("select tb_Category.CategoryID,Name as CategoryName,isnull(DisplayOrder,0) as DisplayOrders,tb_CategoryMapping.ParentCategoryID,isnull((select isnull(Name,'') as ParentName from tb_Category where categoryID =tb_CategoryMapping.ParentCategoryID),'') as ParentName from tb_Category inner join tb_CategoryMapping on tb_Category.CategoryID=tb_CategoryMapping.CategoryID where isnull(tb_Category.Deleted,0)=0 and isnull(tb_Category.Active,0)=1");
        //    DataSet ds = new DataSet();
        //    //ds = CategoryComponent.GetAllCategoriesWithsearch(Convert.ToInt32(ddlStore.SelectedValue), Convert.ToString(ddlSearch.SelectedValue), txtSearch.Text.Trim(), "Active");

        //    if (ddlStatus.SelectedValue.ToString().ToLower().Trim() == "active")
        //    {
        //        ds = CommonComponent.GetCommonDataSet("select SKU,VariantPrice as SalePrice,BaseCustomPrice,DisplayOrder,isnull([Weight],0) as [Weight] from tb_ProductVariantValue where isnull(SKU,'')<>'' and ProductID in(select ProductID from tb_Product where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and ProductID  in (select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))) union select SKU,SalePrice as SalePrice,0 as BaseCustomPrice,DisplayOrder,isnull([Weight],0) as [Weight] from tb_Product where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and ProductID  in (select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))");
        //    }
        //    else if (ddlStatus.SelectedValue.ToString().ToLower().Trim() == "inactive")
        //    {
        //        ds = CommonComponent.GetCommonDataSet("select SKU,VariantPrice as SalePrice,BaseCustomPrice,DisplayOrder,isnull([Weight],0) as [Weight] from tb_ProductVariantValue where isnull(SKU,'')<>'' and ProductID in(select ProductID from tb_Product where StoreID=1 and isnull(Active,0)=0 and isnull(Deleted,0)=0 and ProductID  in (select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))) union select SKU,SalePrice as SalePrice,0 as BaseCustomPrice,DisplayOrder,isnull([Weight],0) as [Weight] from tb_Product where StoreID=1 and isnull(Active,0)=0 and isnull(Deleted,0)=0 and ProductID  in (select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))");
        //    }
        //    else if (ddlStatus.SelectedValue.ToString().ToLower().Trim() == "deleted")
        //    {
        //        ds = CommonComponent.GetCommonDataSet("select SKU,VariantPrice as SalePrice,BaseCustomPrice,DisplayOrder,isnull([Weight],0) as [Weight] from tb_ProductVariantValue where isnull(SKU,'')<>'' and ProductID in(select ProductID from tb_Product where StoreID=1 and  isnull(Deleted,0)= 1 and ProductID  in (select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))) union select SKU,SalePrice as SalePrice,0 as BaseCustomPrice,DisplayOrder,isnull([Weight],0) as [Weight] from tb_Product where StoreID=1 and isnull(Deleted,0)=1 and ProductID  in (select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))");
        //    }
        //    else
        //    {
        //        ds = CommonComponent.GetCommonDataSet("select SKU,VariantPrice as SalePrice,BaseCustomPrice,DisplayOrder,isnull([Weight],0) as [Weight] from tb_ProductVariantValue where isnull(SKU,'')<>'' and ProductID in(select ProductID from tb_Product where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and ProductID  in (select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))) union select SKU,SalePrice as SalePrice,0 as BaseCustomPrice,DisplayOrder,isnull([Weight],0) as [Weight] from tb_Product where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and ProductID  in (select ProductID from tb_ProductCategory where ProductID in (Select ProductID from tb_ProductCategory where CategoryID in (select CategoryID from tb_Category where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 )))");
        //    }

        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        dvCust = ds.Tables[0].DefaultView;
        //        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //        if (dvCust != null)
        //        {
        //            for (int i = 0; i < dvCust.Table.Rows.Count; i++)
        //            {
        //                object[] args = new object[5];
        //                args[0] = Convert.ToString(dvCust.Table.Rows[i]["SKU"]);
        //                args[1] = Convert.ToString(dvCust.Table.Rows[i]["SalePrice"].ToString());
        //                args[2] = Convert.ToString(dvCust.Table.Rows[i]["BaseCustomPrice"]);
        //                args[3] = Convert.ToString(dvCust.Table.Rows[i]["DisplayOrder"]);
        //                args[4] = Convert.ToString(dvCust.Table.Rows[i]["Weight"]);
        //                sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\"", args));
        //            }
        //        }

        //        if (!String.IsNullOrEmpty(sb.ToString()))
        //        {

        //            DateTime dt = DateTime.Now;
        //            String FileName = "ProductList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
        //            string FullString = sb.ToString();
        //            sb.Remove(0, sb.Length);
        //            sb.AppendLine("SKU,SalePrice,BaseCustomPrice,DisplayOrder,Weight");
        //            sb.AppendLine(FullString);

        //            if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
        //                Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

        //            String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
        //            WriteFile(sb.ToString(), FilePath);
        //            Response.ContentType = "text/csv";
        //            Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
        //            Response.TransmitFile(FilePath);
        //            Response.End();
        //        }
        //    }

        //}
        protected void btnExportPrice_Click(object sender, EventArgs e)
        {

            CommonComponent clsCommon = new CommonComponent();
            DataView dvCust = new DataView();

            DataSet ds = new DataSet();


            int storeid = 1;
            if (ddlStore.SelectedValue.ToString() == "-1")
            {
                storeid = 0;
            }
            else
            {
                storeid = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            ds = CommonComponent.GetCommonDataSet("Exec usp_Product_ExportProductDetails 'tb_product.ProductID desc'," + ddlProductType.SelectedValue.ToString() + "," + ddlProductTypeDelivery.SelectedValue.ToString() + "," + ddlCategory.SelectedValue.ToString() + "," + storeid + ",'" + ddlSearch.SelectedValue.ToString() + "','" + txtSearch.Text.ToString() + "','" + ddlStatus.SelectedValue.ToString() + "'");


            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dvCust = ds.Tables[0].DefaultView;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (dvCust != null)
                {
                    for (int i = 0; i < dvCust.Table.Rows.Count; i++)
                    {
                        object[] args = new object[5];
                        args[0] = Convert.ToString(dvCust.Table.Rows[i]["SKU"]);
                        args[1] = Convert.ToString(dvCust.Table.Rows[i]["SalePrice"].ToString());
                        args[2] = Convert.ToString(dvCust.Table.Rows[i]["BaseCustomPrice"]);
                        args[3] = Convert.ToString(dvCust.Table.Rows[i]["DisplayOrder"]);
                        args[4] = Convert.ToString(dvCust.Table.Rows[i]["Weight"]);
                        sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\"", args));
                    }
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "ProductList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine("SKU,SalePrice,BaseCustomPrice,DisplayOrder,Weight");
                    sb.AppendLine(FullString);

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }

        }
        /// <summary>
        /// Import button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadCSV.HasFile && Path.GetExtension(uploadCSV.FileName).ToLower() == ".csv")
                {
                    lblMessage.Text = "";
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/")))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/"));
                    StrFileName = uploadCSV.FileName;
                    DeleteDocument(StrFileName);
                    uploadCSV.SaveAs(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName);
                    //StrFileName = Server.MapPath(AppLogic.AppConfigs("CategoryImportPath") + "CategoryCSV/ImportCSV/") + StrFileName;
                    FillMapping(uploadCSV.FileName);
                }
                else
                {
                    //lblMessage.Text = "Please upload appropriate file.";
                    //lblMessage.Style.Add("color", "#FF0000");
                    //lblMessage.Style.Add("font-weight", "normal");

                }
                if (!string.IsNullOrEmpty(StrFileName))
                {

                    DataTable dtCSV = LoadCSV(StrFileName);
                    if (InsertDataInDataBase(dtCSV) && lblMessage.Text == "")
                    {
                        // contVerify.Visible = false;
                        lblMessage.Text = "Product Imported Successfully";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                        lblMessage.Visible = true;
                        return;

                    }


                }
                else
                {
                    lblMessage.Text += "Sorry file not found. Please retry uploading.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                    lblMessage.Visible = true;
                }
            }
            catch { }
        }


        /// <summary>
        /// Display CSV File data in Grid
        /// </summary>
        /// <param name="FileName">FileName</param>
        /// <returns>DataTable</returns>
        private DataTable LoadCSV(string FileName)
        {
            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string[] FieldNames = csv.GetFieldHeaders();
                DataTable dtCSV = new DataTable();
                DataColumn columnID = new DataColumn();
                columnID.Caption = "Number";
                columnID.ColumnName = "Number";
                columnID.AllowDBNull = false;
                columnID.AutoIncrement = true;
                columnID.AutoIncrementSeed = 1;
                columnID.AutoIncrementStep = 1;
                dtCSV.Columns.Add(columnID);
                foreach (string FieldName in FieldNames)
                    dtCSV.Columns.Add(FieldName);
                while (csv.ReadNextRecord())
                {
                    DataRow dr = dtCSV.NewRow();
                    for (int i = 0; i < FieldCount; i++)
                    {
                        string FieldName = FieldNames[i];
                        if (!dr.Table.Columns.Contains(FieldName))
                        { continue; }

                        dr[FieldName] = csv[i];
                    }
                    dtCSV.Rows.Add(dr);
                }
                dtCSV.AcceptChanges();
                return dtCSV;
            }
        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string StrFileName)
        {
            try
            {
                string docPath = Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }

        private bool InsertDataInDataBase(DataTable dt)
        {



            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (String.IsNullOrEmpty(dt.Rows[i]["SalePrice"].ToString()))
                    {
                        dt.Rows[i]["SalePrice"] = 0;
                        dt.AcceptChanges();
                    }
                    if (String.IsNullOrEmpty(dt.Rows[i]["BaseCustomPrice"].ToString()))
                    {
                        dt.Rows[i]["BaseCustomPrice"] = 0;
                        dt.AcceptChanges();
                    }
                    if (String.IsNullOrEmpty(dt.Rows[i]["DisplayOrder"].ToString()))
                    {
                        dt.Rows[i]["DisplayOrder"] = 0;
                        dt.AcceptChanges();
                    }
                    if (String.IsNullOrEmpty(dt.Rows[i]["Weight"].ToString()))
                    {
                        dt.Rows[i]["Weight"] = 0;
                        dt.AcceptChanges();
                    }
                    try
                    {
                        CommonComponent.ExecuteCommonData("update tb_product set SalePrice=" + dt.Rows[i]["SalePrice"].ToString() + ",DisplayOrder=" + dt.Rows[i]["DisplayOrder"].ToString() + ",[Weight]=" + dt.Rows[i]["Weight"].ToString() + " where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and SKU = '" + dt.Rows[i]["SKU"].ToString() + "'");
                        CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set VariantPrice=" + dt.Rows[i]["SalePrice"].ToString() + ", BaseCustomPrice=" + dt.Rows[i]["BaseCustomPrice"].ToString() + ",DisplayOrder=" + dt.Rows[i]["DisplayOrder"].ToString() + ",[Weight]=" + dt.Rows[i]["Weight"].ToString() + " where  SKU = '" + dt.Rows[i]["SKU"].ToString() + "' and ProductID in (select ProductID from tb_Product where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0)");
                    }
                    catch { }


                }
            }
            else
            {
                return false;

                StrFileName = "";
            }


            return true;

        }

        /// <summary>
        /// Bind Check box based on columns specified in CSV File
        /// </summary>
        /// <param name="FileName">FileName</param>
        private void FillMapping(string FileName)
        {

            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string FieldStrike = ",";

                if (FieldCount > 0)
                {
                    string[] FieldNames = csv.GetFieldHeaders();

                    foreach (string FieldName in FieldNames)
                    {
                        string tempFieldName = FieldName.ToLower();
                        if (tempFieldName == "sku" || tempFieldName == "saleprice" || tempFieldName == "displayorder" || tempFieldName == "basecustomprice" || tempFieldName == "weight")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",sku,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",displayorder,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",saleprice,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",basecustomprice,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",weight,") > -1)
                        {

                        }
                        else
                        {
                            lblMessage.Text = "File Does not contain all columns";
                            lblMessage.Style.Add("color", "#FF0000");
                            lblMessage.Style.Add("font-weight", "normal");
                        }
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {

                        //if (FieldStrike.ToLower().Contains("price"))
                        //    chkFields.Items.Add("Price");
                        //if (FieldStrike.ToLower().Contains("saleprice"))
                        //    chkFields.Items.Add("Sale Price");
                        //if (FieldStrike.ToLower().Contains("inventory"))
                        //    chkFields.Items.Add("Inventory");
                        //if (FieldStrike.ToLower().Contains("weight"))
                        //    chkFields.Items.Add("Weight");
                        BindData();
                        //contVerify.Visible = true;
                        //DataTable dtCSV = LoadCSV(FileName);
                    }
                    else
                    {
                        lblMessage.Text = "Please Specify SKU,SalePrice,BaseCustomPrice,DisplayOrder,Weight in file.";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    lblMessage.Text = "Please Specify SKU,SalePrice,BaseCustomPrice,DisplayOrder,Weight in file.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
        }

        /// <summary>
        /// Bind Data with Gridview
        /// </summary>
        private void BindData()
        {
            DataTable dtCSV = LoadCSV(StrFileName);
            if (dtCSV.Rows.Count > 0)
            {

            }
            else
                lblMessage.Text = "No data exists in file.";
            lblMessage.Style.Add("color", "#FF0000");
            lblMessage.Style.Add("font-weight", "normal");
        }
        //protected void btnRefresh_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        String ContentPath = "";
        //        ContentPath = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName like '%ContentServerPhysicalPath%' and storeid=1"));
        //        if (!String.IsNullOrEmpty(ContentPath))
        //        {
        //            if (!Directory.Exists((ContentPath + "/InventoryFiles/")))
        //                Directory.CreateDirectory((ContentPath + "/InventoryFiles/"));
        //            int loginid = 0;
        //            loginid = Convert.ToInt32(Session["AdminID"].ToString());
        //            if (loginid > 0)
        //            {



        //                string ProductID = "";
        //                for (int i = 0; i < grdProduct.Rows.Count; i++)
        //                {
        //                    HiddenField hdnProductID = (HiddenField)grdProduct.Rows[i].FindControl("hdnProductid");
        //                    CheckBox chkSelect = (CheckBox)grdProduct.Rows[i].FindControl("chkSelect");

        //                    if (chkSelect.Checked)
        //                    {


        //                        if (String.IsNullOrEmpty(ProductID))
        //                        {
        //                            ProductID = hdnProductID.Value.ToString();
        //                        }
        //                        else
        //                        {
        //                            ProductID = ProductID + "," + hdnProductID.Value.ToString();
        //                        }

        //                    }
        //                }

        //                if (!String.IsNullOrEmpty(ProductID))
        //                {
        //                    CommonComponent.ExecuteCommonData("Exec usp_activate_updateInventory '" + ProductID + "' ");

        //                    String Docpath = ContentPath + "/InventoryFiles/" + loginid + ".xml";
        //                    if (File.Exists((Docpath)))
        //                    {
        //                        DataSet dsxml = new DataSet();

        //                        DataTable dt = new DataTable("InventoryMaster");
        //                        dt.Columns.Add(new DataColumn("IsInventory", typeof(string)));
        //                        DataRow dr = dt.NewRow();
        //                        dr["IsInventory"] = "1";
        //                        dt.Rows.Add(dr);
        //                        dsxml.Tables.Add(dt);

        //                        dsxml.WriteXml(Docpath);
        //                    }
        //                    else
        //                    {

        //                        String FileName = loginid + ".xml";

        //                        DataSet dsxml = new DataSet();

        //                        DataTable dt = new DataTable("InventoryMaster");
        //                        dt.Columns.Add(new DataColumn("IsInventory", typeof(string)));
        //                        DataRow dr = dt.NewRow();
        //                        dr["IsInventory"] = "1";
        //                        dt.Rows.Add(dr);
        //                        dsxml.Tables.Add(dt);

        //                        dsxml.WriteXml(Docpath);


        //                    }
        //                }


        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                String ContentPath = "";
                ContentPath = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName like '%ContentServerPhysicalPath%' and storeid=1"));
                if (!String.IsNullOrEmpty(ContentPath))
                {
                    if (!Directory.Exists((ContentPath + "/InventoryFiles/")))
                        Directory.CreateDirectory((ContentPath + "/InventoryFiles/"));
                    int loginid = 0;
                    loginid = Convert.ToInt32(Session["AdminID"].ToString());
                    if (loginid > 0)
                    {



                        string ProductID = "";
                        for (int i = 0; i < grdProduct.Rows.Count; i++)
                        {
                            HiddenField hdnProductID = (HiddenField)grdProduct.Rows[i].FindControl("hdnProductid");
                            CheckBox chkSelect = (CheckBox)grdProduct.Rows[i].FindControl("chkSelect");

                            if (chkSelect.Checked)
                            {


                                if (String.IsNullOrEmpty(ProductID))
                                {
                                    ProductID = hdnProductID.Value.ToString();
                                }
                                else
                                {
                                    ProductID = ProductID + "," + hdnProductID.Value.ToString();
                                }

                            }
                        }

                        if (!String.IsNullOrEmpty(ProductID))
                        {
                            CommonComponent.ExecuteCommonData("Exec usp_activate_updateInventory '" + ProductID + "' ");

                            String Docpath = ContentPath + "/InventoryFiles/" + loginid + ".xml";
                            if (File.Exists((Docpath)))
                            {
                                DataSet dsxml = new DataSet();

                                DataTable dt = new DataTable("InventoryMaster");
                                dt.Columns.Add(new DataColumn("IsInventory", typeof(string)));
                                DataRow dr = dt.NewRow();
                                dr["IsInventory"] = "1";
                                dt.Rows.Add(dr);
                                dsxml.Tables.Add(dt);

                                dsxml.WriteXml(Docpath);
                            }
                            else
                            {

                                String FileName = loginid + ".xml";

                                DataSet dsxml = new DataSet();

                                DataTable dt = new DataTable("InventoryMaster");
                                dt.Columns.Add(new DataColumn("IsInventory", typeof(string)));
                                DataRow dr = dt.NewRow();
                                dr["IsInventory"] = "1";
                                dt.Rows.Add(dr);
                                dsxml.Tables.Add(dt);

                                dsxml.WriteXml(Docpath);


                            }
                        }


                    }

                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void btnCheckAll_Click(object sender, EventArgs e)
        {
            grdProduct.PageSize = 10000;
            txtSearch.Text = "";
            grdProduct.PageIndex = 0;

            grdProduct.DataBind();
            if (grdProduct.Rows.Count == 0)
            {
                trBottom.Visible = false;
                btnAmazonPrice.Visible = false;
                btnAmazonProduct.Visible = false;
                btnAmozonUpdate.Visible = false;
                btnAmazonImage.Visible = false;

                btneBayProduct.Visible = false;
                btneBayUpdate.Visible = false;

                btnSearsProduct.Visible = false;
                btnSearsUpdate.Visible = false;
                btnSearsPrice.Visible = false;

                btnOverStockUpdate.Visible = false;
                btnOverStockProduct.Visible = false;
                //btnExport.Visible = false;
            }
            btnApprove.Visible = false;
            if (grdProduct.Rows.Count > 0 && ddlStatus.SelectedItem.Text.ToString().ToLower() == "dataverify")
            {
                btnApprove.Visible = true;
            }
            SetSession();
        }

        protected void btnecommexportprice_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();
            SecurityComponent objsec = new SecurityComponent();
            dsorder = CommonComponent.GetCommonDataSet("EXEC [usp_product_exportproductprice]");
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
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                return;
            }

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    string StrStorename = "HPDEcomm";

                    String FileName = StrStorename.ToString() + "_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                    ViewState["LastExportFileName1"] = null;
                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);

                    ViewState["LastExportFileName1"] = FileName.ToString();
                    WriteFile(sb.ToString(), FilePath);
                }
                if (ViewState["LastExportFileName1"] != null)
                {
                    DownloadProductExport1();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Data Exported Successfully.','Message');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                    return;
                }
            }

        }

        protected void DownloadProductExport1()
        {
            if (ViewState["LastExportFileName1"] != null)
            {
                String FilePath = Server.MapPath("~/Admin/Files/" + ViewState["LastExportFileName1"].ToString());
                if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                if (File.Exists(FilePath))
                {
                    Response.Clear();
                    Response.ClearContent();
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["LastExportFileName1"].ToString());
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('File not found.','Message');", true);
                return;
            }
        }

        #region "Jet Product"
        public static string GetNumber(int length)
        {
            return string.Concat(RandomDigits().Take(length));
        }
        public static IEnumerable<int> RandomDigits()
        {
            var rng = new Random(System.Environment.TickCount);
            while (true) yield return rng.Next(10);
        }
        public async void Put_merchantSKU()
        {
            try
            {
                //Task<string> task = GenerateToken();
                //GenerateToken();

                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Convert.ToString(AppLogic.AppConfigs("JetProductToken")) + "");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken + "");
                // Request headers
                //client.DefaultRequestHeaders.Add("Content-Type", "application/json");

                HttpResponseMessage response;

                string productdetails = "";
                foreach (GridViewRow dr in grdProduct.Rows)
                {
                    CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                    Label lblSKU = (Label)dr.FindControl("lblSKU");
                    Label lblPrice = (Label)dr.FindControl("lblPrice");
                    Label lblSalePrice = (Label)dr.FindControl("lblSalePrice");
                    HiddenField hdnProductid = (HiddenField)dr.FindControl("hdnProductid");
                    // " + hdnProductid.Value + @",
                    // """ + dsjetProduct.Tables[0].Rows[0]["AvaliableStartDate"].ToString() + @""",
                    if (chk.Checked == true)
                    {
                        string staticnumber = GetNumber(12);


                        //DataSet dsjetProduct = CommonComponent.GetCommonDataSet("select isnull(UPC,'') as UPC,ASIN,Brand,Manufature,isnull(eBayCategoryID,'') as eBayCategoryID,ImageName,Name,Description,ManufacturePartNo,AvaliableStartDate,Bulletpoint1,Bulletpoint2,Length,Height,Width,Weight,Price,SalePrice,OurPrice,SKU,Inventory from tb_Product where ProductID=" + hdnProductid.Value + "");
                        DataSet dsjetProduct = CommonComponent.GetCommonDataSet("exec GetProductForJet " + hdnProductid.Value+",1");
                        var uri = "https://merchant-api.jet.com/api/merchant-skus/" + dsjetProduct.Tables[0].Rows[0]["SKU"].ToString();

                        if (dsjetProduct.Tables[0].Rows.Count > 0)
                        {
                            decimal width = 1;
                            decimal height = 1;
                            decimal length = 1;
                            decimal weight = 1;
                            if (!string.IsNullOrEmpty(dsjetProduct.Tables[0].Rows[0]["Width"].ToString()) && Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Width"].ToString()), 2) > 0)
                            {
                                width = Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Width"].ToString()), 2);
                            }
                            if (!string.IsNullOrEmpty(dsjetProduct.Tables[0].Rows[0]["Height"].ToString()) && Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Height"].ToString()), 2) > 0)
                            {
                                height = Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Height"].ToString()), 2);
                            }
                            if (!string.IsNullOrEmpty(dsjetProduct.Tables[0].Rows[0]["Length"].ToString()) && Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Length"].ToString()), 2) > 0)
                            {
                                length = Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Length"].ToString()), 2);
                            }
                            if (!string.IsNullOrEmpty(dsjetProduct.Tables[0].Rows[0]["Weight"].ToString()) && Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Weight"].ToString()), 2) > 0)
                            {
                                weight = Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Weight"].ToString()), 2);
                            }
                            string strImg = "";
                            if (File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsjetProduct.Tables[0].Rows[0]["ImageName"].ToString())))
                            {
                                strImg = "http://www.acedepot.com" + AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsjetProduct.Tables[0].Rows[0]["ImageName"].ToString();
                            }
                            else
                            {
                                strImg = "http://www.acedepot.com" + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg";
                            }

                            //Int32 icount = 0;
                            string moreimage = "";
                            for (int it = 1; it < 26; it++)
                            {
                                if (File.Exists(Server.MapPath("/" + AppLogic.AppConfigs("ImagePathProduct").ToString() + "medium/" + dsjetProduct.Tables[0].Rows[0]["ImageName"].ToString().Replace(".jpg", "") + "_" + it.ToString() + ".jpg")))
                                {
                                    moreimage += "\"image_slot_id\" : " + it.ToString() + ",\"image_url\" : \"http://www.acedepot.com/" + AppLogic.AppConfigs("ImagePathProduct") + "/medium/" + dsjetProduct.Tables[0].Rows[0]["ImageName"].ToString().Replace(".jpg", "") + @"_" + it.ToString() + @".jpg"",},";
                                }
                            }
                            if (moreimage.Length > 0)
                            {
                                moreimage = moreimage.Substring(0, moreimage.Length - 1);
                                moreimage = "\"alternate_images\" : [" + moreimage + "]";
                            }

                            string brand = "";
                            string manufacturer = "";
                            string part_number = "";
                            if (!string.IsNullOrEmpty(dsjetProduct.Tables[0].Rows[0]["Brand"].ToString()))
                            {
                                brand = SetBrand(dsjetProduct.Tables[0].Rows[0]["Brand"].ToString());
                            }
                            else
                            {
                                brand = "";
                            }
                            if (!string.IsNullOrEmpty(dsjetProduct.Tables[0].Rows[0]["Manufature"].ToString()))
                            {
                                manufacturer = SetBrand(dsjetProduct.Tables[0].Rows[0]["Manufature"].ToString());
                            }
                            else
                            {
                                manufacturer = "";
                            }
                            if (!string.IsNullOrEmpty(dsjetProduct.Tables[0].Rows[0]["ManufacturePartNo"].ToString()))
                            {
                                part_number = SetBrand(dsjetProduct.Tables[0].Rows[0]["ManufacturePartNo"].ToString());
                            }
                            else
                            {
                                part_number = dsjetProduct.Tables[0].Rows[0]["SKU"].ToString();
                            }


                            productdetails = @"{
  
                              ""product_title"":""" + SetTitle(dsjetProduct.Tables[0].Rows[0]["Name"].ToString().Trim()) + @""",
                              ""jet_browse_node_id"": " + dsjetProduct.Tables[0].Rows[0]["eBayCategoryID"].ToString() + @", 
                              ""ASIN"": """ + dsjetProduct.Tables[0].Rows[0]["ASIN"].ToString() + @""",
                              ""standard_product_codes"" : [
		                        {	
			                        ""standard_product_code"": """ + dsjetProduct.Tables[0].Rows[0]["UPC"].ToString() + @""",
			                        ""standard_product_code_type"": ""UPC""
		                        }
	                          ],
                            ""multipack_quantity"": 1,
	                        ""brand"": """ + brand.Replace("'", "") + @""",
	                        ""manufacturer"": """ + manufacturer.Replace("'", "") + @""",
	                        ""mfr_part_number"":""" + part_number + @""",
	                        ""product_description"": """ + SetDescription(Regex.Replace(dsjetProduct.Tables[0].Rows[0]["Description"].ToString(), @"<[^>]*>", String.Empty)) + @""",
                            ""bullets"": [
			                            ""SOLD PER PANEL"",
			                            ""100% Cotton"",
			                            ""3\"" Pole Pocket with Hook Belt & Back Tabs"",
			                            ""Blackout Lined"",
			                            ""Dry Clean Only""
			                            ],
                            ""number_units_for_price_per_unit"":1,
                            ""type_of_unit_for_price_per_unit"":""each"";
                            ""shipping_weight_pounds"": " + weight + @",
	                        ""package_length_inches"": " + length + @",
	                        ""package_width_inches"":  " + width + @",
	                        ""package_height_inches"": " + height + @",
	                        ""display_length_inches"":" + length + @",
	                        ""display_width_inches"": " + width + @",
	                        ""display_height_inches"": " + height + @",
                            ""prop65"": true,
                            ""legal_disclaimer_description"": ""Legal stuff goes here"",
                              ""cpsia_cautionary_statements"": [
				                              ""choking hazard balloon"",
				                              ""choking hazard small parts""
								                              ],
                            ""country_of_origin"": ""CHINA"",
                             ""safety_warning"": ""full safety"",
                            ""msrp"": " + Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["Price"].ToString()), 2) + @",
                            ""map_price"": " + Math.Round(Convert.ToDecimal(dsjetProduct.Tables[0].Rows[0]["SalePrice"].ToString()), 2) + @",
                            ""map_implementation"": 101,
                            ""product_tax_code"": ""General Clothing"",";

                            productdetails += @"""attributes_node_specific"":[";

                            productdetails += @"{
			                              ""attribute_id"": 50,
			                              ""attribute_value"": ""50 x 120"",
			                              },
			                              {
			                              ""attribute_id"": 119,
			                              ""attribute_value"": ""Hazelwood Beige""
			                              }],";

                            productdetails += @"main_image_url"" :  """ + strImg + @""",
	                        ""swatch_image_url"" :  """ + strImg + @""",
	                        ""alternate_images"" : [
		                        {
			                        ""image_slot_id"" : 1,
			                        ""image_url"" : """ + strImg + @"""
		                        },
		                        {
			                        ""image_slot_id"" : 2,
			                        ""image_url"" :  """ + strImg + @"""
		                        }]";

                            productdetails += "}";
                        }


                        byte[] byteData = Encoding.UTF8.GetBytes(productdetails);

                        using (var content = new ByteArrayContent(byteData))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            response = await client.PutAsync(uri, content);
                            using (HttpContent content1 = response.Content)
                            {
                                if (response.StatusCode.ToString() == "201" || response.StatusCode.ToString() == "204" || response.StatusCode.ToString() == "200")
                                {
                                    CommonComponent.ExecuteCommonData("UPDATE tb_product SET isjetuploaded=1 WHERE ProductId=" + hdnProductid.Value.ToString() + "");
                                }
                                // ... Read the string.
                                string result = await content1.ReadAsStringAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async void UpdateProductPrice()
        {
            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Convert.ToString(AppLogic.AppConfigs("JetProductToken")) + "");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken + "");
                // Request headers

                HttpResponseMessage response;

                foreach (GridViewRow dr in grdProduct.Rows)
                {
                    CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                    Label lblSKU = (Label)dr.FindControl("lblSKU");
                    Label lblPrice = (Label)dr.FindControl("lblPrice");
                    Label lblSalePrice = (Label)dr.FindControl("lblSalePrice");
                    HiddenField hdnProductid = (HiddenField)dr.FindControl("hdnProductid");

                    if (chk.Checked == true)
                    {
                        var uri = "https://merchant-api.jet.com/api/merchant-skus/" + lblSKU.Text.ToString().Trim() + "/price";
                        //var uri = "https://merchant-api.jet.com/api/merchant-skus/123456789012/price";
                        // var uri = "https://merchant-api.jet.com/api/merchant-skus/{"+hdnProductid.Value+"}/price";
                        //DataSet dssd = CommonComponent.GetCommonDataSet("select Price from tb_Product where ProductID="+hdnProductid.Value+"");
                        decimal price = 0;
                        if (Convert.ToDouble(lblPrice.Text.ToString()) >= Convert.ToDouble(lblSalePrice.Text.ToString()) && Convert.ToDouble(lblSalePrice.Text.ToString()) > Convert.ToDouble(0))
                        {
                            price = Convert.ToDecimal(lblSalePrice.Text.ToString());
                        }
                        else
                        { price = Convert.ToDecimal(lblPrice.Text.ToString()); }

                        //if (lblSalePrice.Text == "0")
                        //{
                        //    price = Convert.ToDecimal(lblPrice.Text.ToString());
                        //}

                        string node = Convert.ToString(AppLogic.AppConfigs("fulfillment_node_id"));
                        string Inventory = @"{
                                              ""price"":" + price + @",
                                              ""fulfillment_nodes"": [
                                                {
                                                  ""fulfillment_node_id"": """ + Convert.ToString(AppLogic.AppConfigs("fulfillment_node_id")) + @""",
                                                  ""fulfillment_node_price"": " + price + @"
                                                }
                                              ]
                                            }";
                        // Request body
                        byte[] byteData = Encoding.UTF8.GetBytes(Inventory);

                        using (var content = new ByteArrayContent(byteData))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            response = await client.PutAsync(uri, content);
                            using (HttpContent content1 = response.Content)
                            {
                                // ... Read the string.

                                string result = await content1.ReadAsStringAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //  Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());
            }
        }
        public async void deleteproduct()
        {
            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                // client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Convert.ToString(AppLogic.AppConfigs("JetProductToken")) + "");
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken + "");
                // Request headers

                HttpResponseMessage response;

                foreach (GridViewRow dr in grdProduct.Rows)
                {
                    CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                    Label lblSKU = (Label)dr.FindControl("lblSKU");
                    // Label lblPrice = (Label)dr.FindControl("lblPrice");
                    // Label lblSalePrice = (Label)dr.FindControl("lblSalePrice");
                    HiddenField hdnProductid = (HiddenField)dr.FindControl("hdnProductid");

                    if (chk.Checked == true)
                    {
                        //  var uri = "https://merchant-api.jet.com/api/merchant-skus/" + lblSKU.Text.ToString().Trim() + "/price";
                        var uri = " https://merchant-api.jet.com/api/merchant-skus/" + lblSKU.Text.ToString().Trim() + "/status/archive"; //DW-U4NB-AYKA
                        //var uri = "https://merchant-api.jet.com/api/merchant-skus/123456789012/price";
                        // var uri = "https://merchant-api.jet.com/api/merchant-skus/{"+hdnProductid.Value+"}/price";
                        //DataSet dssd = CommonComponent.GetCommonDataSet("select Price from tb_Product where ProductID="+hdnProductid.Value+"");
                        //decimal price = 0;
                        //if (Convert.ToDouble(lblPrice.Text.ToString()) >= Convert.ToDouble(lblSalePrice.Text.ToString()) && Convert.ToDouble(lblSalePrice.Text.ToString()) > Convert.ToDouble(0))
                        //{
                        //    price = Convert.ToDecimal(lblSalePrice.Text.ToString());
                        //}
                        //else
                        //{ price = Convert.ToDecimal(lblPrice.Text.ToString()); }

                        //if (lblSalePrice.Text == "0")
                        //{
                        //    price = Convert.ToDecimal(lblPrice.Text.ToString());
                        //}

                        string node = Convert.ToString(AppLogic.AppConfigs("fulfillment_node_id"));
                        string Inventory = @"{ ""is_archived"": true }";
                        // Request body
                        byte[] byteData = Encoding.UTF8.GetBytes(Inventory);

                        using (var content = new ByteArrayContent(byteData))
                        {
                            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            response = await client.PutAsync(uri, content);
                            using (HttpContent content1 = response.Content)
                            {
                                // ... Read the string.

                                string result = await content1.ReadAsStringAsync();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());
            }
        }
        public async void UpdateProductInventory()
        {
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);
            //  client.DefaultRequestHeaders.Add("Authorization", "Bearer " + Convert.ToString(AppLogic.AppConfigs("JetProductToken")) + "");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken + "");
            // Request headers

            HttpResponseMessage response;

            foreach (GridViewRow dr in grdProduct.Rows)
            {
                CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                Label lblSKU = (Label)dr.FindControl("lblSKU");

                Label lblInventory = (Label)dr.FindControl("lblInventory");
                HiddenField hdnProductid = (HiddenField)dr.FindControl("hdnProductid");

                if (chk.Checked == true)
                {
                    // var uri = "https://merchant-api.jet.com/api/merchant-skus/{" + hdnProductid.Value + "}/inventory";
                    //var uri = "https://merchant-api.jet.com/api/merchant-skus/123456789012/inventory";
                    var uri = "https://merchant-api.jet.com/api/merchant-skus/" + lblSKU.Text.ToString().Trim() + "/inventory";
                    //DataSet dssd = CommonComponent.GetCommonDataSet("select Brand,Manufature,Name,Description,Bulletpoint1,Bulletpoint1,Length,Height,Weight,Price,SalePrice,OurPrice,ima from tb_Product");
                    //  ""quantity"":" + Convert.ToInt32(lblInventory.Text.ToString()) + @",

                    // ""fulfillment_node_id"": ""69c9ac37a5b64e808d9f70e8b9dd02fb"",
                    string Inventory = @"{
                                       ""fulfillment_nodes"": [
                                        {
                                          ""fulfillment_node_id"": """ + Convert.ToString(AppLogic.AppConfigs("fulfillment_node_id")) + @""",
                                          ""quantity"": " + Convert.ToInt32(lblInventory.Text.ToString()) + @"
                                        }
                                      ]
                                    }";

                    //                ,
                    //{
                    //  ""fulfillment_node_id"": ""AKSDKDJIJDISJFIDFJIDSIFFISI"",
                    //  ""quantity"": 20
                    //}
                    // Request body
                    byte[] byteData = Encoding.UTF8.GetBytes(Inventory);

                    using (var content = new ByteArrayContent(byteData))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        response = await client.PutAsync(uri, content);
                        using (HttpContent content1 = response.Content)
                        {
                            // ... Read the string.
                            string result = await content1.ReadAsStringAsync();
                        }
                    }
                }
            }
        }
        public async void GenerateToken()
        {
            try
            {


                // string filename = System.Reflection.Assembly.GetExecutingAssembly().Location.ToString().Replace("JetService.exe", "") + "/Resources/Token";
                string filename = "/Resources/Token";
                if (!System.IO.Directory.Exists(Server.MapPath(filename)))   // if (!Directory.Exists(filename))
                {
                    Directory.CreateDirectory(filename);
                }
                filename = filename + "/Token_data.xml";

                if (File.Exists(Server.MapPath(filename)))
                {
                    DataSet dsExist = new DataSet();
                    dsExist.ReadXml(Server.MapPath(filename));
                    if (dsExist != null && dsExist.Tables.Count > 0 && dsExist.Tables[0].Rows.Count > 0)
                    {
                        DateTime datExpire = Convert.ToDateTime(dsExist.Tables[0].Rows[0]["expires_on"].ToString());
                        TimeSpan diff = datExpire - DateTime.Now;
                        double hours = diff.TotalHours;
                        if (hours > Convert.ToDouble(1))
                        {
                            AccessToken = dsExist.Tables[0].Rows[0]["id_token"].ToString();
                            //GetOrderNumbers();
                            // return AccessToken;
                            return;
                        }
                    }
                }

                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);


                var uri = "https://merchant-api.jet.com/api/token";

                HttpResponseMessage response;

                // Request body
                string username = Convert.ToString(AppLogic.AppConfigs("JetProductUsername"));
                string password = Convert.ToString(AppLogic.AppConfigs("JetProductPassword"));
                //  byte[] byteData = Encoding.UTF8.GetBytes("{\"user\":\"88463AB4C4D1C670C2B324F8A9393A05AF59B95B\",\"pass\":\"X2cRp+O39Jw6ZQUScJRqeZZgrpM5MSZLfEjE5PwJyA/V\"}");
                byte[] byteData = Encoding.UTF8.GetBytes("{\"user\":\"" + username + "\",\"pass\":\"" + password + "\"}");

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    // response = await client.GetAsync(uri, content);
                    //response = await client.PostAsync(uri, content);
                    response = client.PostAsync(uri, content).Result;
                    //System.Threading.Thread.Sleep(100000);
                    using (HttpContent content1 = response.Content)
                    {
                        // ... Read the string.
                        DataSet DsToken = new DataSet();
                        string result = await content1.ReadAsStringAsync();
                        DataTable dt = new DataTable();

                        dt = JsonStringToDataTable(result); //(DataTable)JsonConvert.DeserializeObject(result, (typeof(DataTable)));
                        DsToken.Tables.Add(dt);

                        if (DsToken != null && DsToken.Tables.Count > 0 && DsToken.Tables[0].Rows.Count > 0)
                        {
                            try
                            {
                                if (File.Exists(Server.MapPath(filename)))
                                {
                                    File.Delete(Server.MapPath(filename));
                                }
                                DsToken.WriteXml(Server.MapPath(filename));
                            }
                            catch { }
                            AccessToken = DsToken.Tables[0].Rows[0]["id_token"].ToString();
                            //Put_merchantSKU();
                            //return AccessToken;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());
            }

            //return "";
        }
        static DataTable JsonStringToDataTable(string jsonString)
        {
            DataTable dt = new DataTable();
            string[] jsonStringArray = Regex.Split(jsonString.Replace("[", "").Replace("]", ""), "},{");
            List<string> ColumnsName = new List<string>();
            foreach (string jSA in jsonStringArray)
            {
                string[] jsonStringData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                foreach (string ColumnsNameData in jsonStringData)
                {
                    try
                    {
                        int idx = ColumnsNameData.IndexOf(":");
                        string ColumnsNameString = ColumnsNameData.Substring(0, idx - 1).Replace("\"", "").Replace(System.Environment.NewLine, "").Replace("\\r\\n ", "").Trim();
                        if (!ColumnsName.Contains(ColumnsNameString))
                        {
                            ColumnsName.Add(ColumnsNameString);
                        }
                    }
                    catch (Exception ex)
                    {
                    }
                }
                break;
            }
            foreach (string AddColumnName in ColumnsName)
            {
                dt.Columns.Add(AddColumnName);
            }
            foreach (string jSA in jsonStringArray)
            {
                string[] RowData = Regex.Split(jSA.Replace("{", "").Replace("}", ""), ",");
                DataRow nr = dt.NewRow();
                foreach (string rowData in RowData)
                {
                    try
                    {
                        int idx = rowData.IndexOf(":");
                        string RowColumns = rowData.Substring(0, idx - 1).Replace("\"", "").Replace(System.Environment.NewLine, "").Trim();
                        string RowDataString = rowData.Substring(idx + 1).Replace("\"", "").Trim();
                        nr[RowColumns] = RowDataString;
                    }
                    catch (Exception ex)
                    {
                        continue;
                    }
                }
                dt.Rows.Add(nr);
            }
            return dt;
        }
        public static async void GetOrderNumbers()
        {
            // GenerateToken();
            var client = new HttpClient();
            var queryString = HttpUtility.ParseQueryString(string.Empty);

            // Request headers
            // client.DefaultRequestHeaders.Add("Content-Type", "application/json");
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken + "");
            //  var uri = "https://merchant-api.jet.com/api/orders/ready";
            //var uri = "https://merchant-api.jet.com/api/orders/created";
            //var uri = "https://merchant-api.jet.com/api/orders/acknowledged";
            //var uri = "https://merchant-api.jet.com/api/orders/inprogress";
            var uri = "https://merchant-api.jet.com/api/orders/complete";
            HttpResponseMessage response;
            response = await client.GetAsync(uri);
            using (HttpContent content1 = response.Content)
            {
                // ... Read the string.
                string result = await content1.ReadAsStringAsync();
                // ... Display the result.
            }

        }
        public String SetTitle(String Name)
        {
            string name = string.Empty;

            try
            {
                if (Name.Length < 5)
                    Name = Name + ".....";
                if (Name.Length > 500)
                    Name = Name.Substring(0, 488) + "..";
                name = Server.HtmlEncode(Name);
            }
            catch (Exception ex)
            {

            }
            return name;
        }
        public String SetBrand(String Name)
        {
            string name = string.Empty;

            try
            {
                if (Name.Length > 50)
                    Name = Name.Substring(0, 48) + "..";
                name = Server.HtmlEncode(Name);
            }
            catch (Exception ex)
            {
            }
            return name;
        }
        public String SetDescription(String Name)
        {
            string name = string.Empty;
            if (string.IsNullOrEmpty(name))
            {
                name = "Testttt Product";
            }
            try
            {
                if (Name.Length > 2000)
                    Name = Name.Substring(0, 1998) + "..";
                name = Server.HtmlEncode(Name);
            }
            catch (Exception ex)
            {
            }
            return name;
        }
        public async void VariationUpload(string ProductId)
        {
            try
            {
                var client = new HttpClient();
                var queryString = HttpUtility.ParseQueryString(string.Empty);
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + AccessToken + "");

                HttpResponseMessage response;

                string productdetails = "";
                // foreach (GridViewRow dr in grdProduct.Rows)
                // {
                //CheckBox chk = (CheckBox)dr.FindControl("chkSelect");
                //Label lblSKU = (Label)dr.FindControl("lblSKU");
                //Label lblPrice = (Label)dr.FindControl("lblPrice");
                //Label lblSalePrice = (Label)dr.FindControl("lblSalePrice");
                //HiddenField hdnProductid = (HiddenField)dr.FindControl("hdnProductid");

                //if (chk.Checked == true)
                // {
                string staticnumber = GetNumber(12);

                DataSet dsjetProduct = CommonComponent.GetCommonDataSet("exec GetProductForJet " + ProductId + ",2");
                //var uri = "https://merchant-api.jet.com/api/merchant-skus/" + dsjetProduct.Tables[0].Rows[0]["SKU"].ToString();
                var uri = "https://merchant-api.jet.com/api/merchant-skus/" + dsjetProduct.Tables[0].Rows[0]["SKU"].ToString() + "/variation";


                if (dsjetProduct.Tables[0].Rows.Count > 0)
                {
                    string childSkus = "";
                    string variantIds = "";

                    dsjetProduct.Tables[0].DefaultView.ToTable(true, "chilSku");

                    for (int i = 0; i < dsjetProduct.Tables[0].Rows.Count; i++)
                    {
                        childSkus += "\"" + dsjetProduct.Tables[0].Rows[i]["chilSku"].ToString() + "\"" + ",";
                    }

                    dsjetProduct.Tables[0].DefaultView.ToTable(true, "VariantName");

                    for (int i = 0; i < dsjetProduct.Tables[0].Rows.Count; i++)
                    {
                        if (dsjetProduct.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("size") > -1)
                        {
                            variantIds = "50,";
                            break;
                        }
                    }
                    variantIds += "119,";


                    productdetails += @"{""relationship"": ""Variation"",""variation_refinements"": [" + variantIds.Remove(variantIds.Length - 1) + "],";
                    productdetails += @"""children_skus"":[" + childSkus.Remove(childSkus.Length - 1) + "]}";

                }


                byte[] byteData = Encoding.UTF8.GetBytes(productdetails);

                using (var content = new ByteArrayContent(byteData))
                {
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    response = await client.PutAsync(uri, content);
                    using (HttpContent content1 = response.Content)
                    {
                        //if (response.StatusCode.ToString() == "201" || response.StatusCode.ToString() == "204" || response.StatusCode.ToString() == "200")
                        //{
                        //    CommonComponent.ExecuteCommonData("UPDATE tb_product SET isjetuploaded=1 WHERE ProductId=" + hdnProductid.Value.ToString() + "");
                        //}
                        string result = await content1.ReadAsStringAsync();
                    }
                }
                //}
                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}