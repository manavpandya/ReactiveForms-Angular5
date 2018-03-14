using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using Solution.Bussines.Entities;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductEBay : BasePage
    {
        private int StoreID = 7;
        tb_Product tb_Product = null;
        DataSet dseBaycategory = new DataSet();
        DataSet dseBayStoreCategory = new DataSet();
        public static string ProductTempPath = string.Empty;
        public static string ProductIconPath = string.Empty;
        public static string ProductMediumPath = string.Empty;
        public static string ProductLargePath = string.Empty;
        public static string ProductMicroPath = string.Empty;

        static int finHeight;
        static int finWidth;
        static Size thumbNailSizeLarge = Size.Empty;
        static Size thumbNailSizeMediam = Size.Empty;
        static Size thumbNailSizeIcon = Size.Empty;
        static Size thumbNailSizeMicro = Size.Empty;

        int InventoryTotal = 0;
        DataSet dsWarehouse = new DataSet();
        ConfigurationComponent objConfiguration = new ConfigurationComponent();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
            btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
            btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            if (!IsPostBack)
            {
                txtInventory.Attributes.Add("readonly", "true");
                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");

                lblebayExpireDate.Text = "---";
                lblebayLastUpdated.Text = "---";
                lblebayProductID.Text = "---";
                BindebayCategory();
                BindeBayStoreCategory();

                if (Request.QueryString["ID"] != null)
                {
                    GetWarehouseList(Convert.ToInt32(Request.QueryString["ID"]), 2);
                }
                else if (Request.QueryString["OLDID"] != null)
                {
                    GetWarehouseList(Convert.ToInt32(Request.QueryString["OLDID"]), 2);
                }
                else
                {
                    GetWarehouseList(0, 1);
                }

                if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null)
                {

                    BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                    tduploadMoreImages.Visible = true;
                    trProductVariant.Visible = true;
                    ifrmProductVariant.Attributes.Add("src", "ProductVariant.aspx?StoreId=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ID=" + Convert.ToString(Request.QueryString["ID"]) + "&HType=no");
                    lblHeader.Text = "Update Product";
                    trProductOption.Attributes.Add("style", "display:''");
                    trProductOption.Attributes.Add("style", "display:block");
                }
                else
                {
                    trProductVariant.Visible = false;
                    ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                }
            }

            //if (Request.QueryString["StoreID"] != null)
            //    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            if (!IsPostBack)
            {
                if (Request.QueryString["StoreID"] != null)
                    StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                if (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
                {
                    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                }
                else
                {
                    if (!string.IsNullOrEmpty(AppLogic.AppConfigs("StoreID")))
                        AppConfig.StoreID = Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString());
                    else
                        AppConfig.StoreID = 7;
                }
                if (AppConfig.StoreID < 1)
                    AppConfig.StoreID = 7;

            }
            BindSize();
        }

        /// <summary>
        /// Binds the size for Create Image
        /// </summary>
        private void BindSize()
        {
            DataSet dsIconWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductIconWidth");
            DataSet dsIconHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductIconHeight");
            if ((dsIconWidth != null && dsIconWidth.Tables.Count > 0 && dsIconWidth.Tables[0].Rows.Count > 0) && (dsIconHeight != null && dsIconHeight.Tables.Count > 0 && dsIconHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeIcon = new Size(Convert.ToInt32(dsIconWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsIconHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsLargeWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductLargeWidth");
            DataSet dsLargeHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductLargeHeight");
            if ((dsLargeWidth != null && dsLargeWidth.Tables.Count > 0 && dsLargeWidth.Tables[0].Rows.Count > 0) && (dsLargeHeight != null && dsLargeHeight.Tables.Count > 0 && dsLargeHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeLarge = new Size(Convert.ToInt32(dsLargeWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsLargeHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsMediumWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductMediumWidth");
            DataSet dsMediumHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductMediumHeight");
            if ((dsMediumWidth != null && dsMediumWidth.Tables.Count > 0 && dsMediumWidth.Tables[0].Rows.Count > 0) && (dsMediumHeight != null && dsMediumHeight.Tables.Count > 0 && dsMediumHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeMediam = new Size(Convert.ToInt32(dsMediumWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMediumHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsMicroWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductMicroWidth");
            DataSet dsMicroHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductMicroHeight");
            if ((dsMicroWidth != null && dsMicroWidth.Tables.Count > 0 && dsMicroWidth.Tables[0].Rows.Count > 0) && (dsMicroHeight != null && dsMicroHeight.Tables.Count > 0 && dsMicroHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeMicro = new Size(Convert.ToInt32(dsMicroWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMicroHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

        }

        /// <summary>
        /// Binds the Data for Ebay Store
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="StoreID">int StoreID</param>
        public void BindData(Int32 ID, Int32 StoreID)
        {
            DataSet DsProduct = new DataSet();
            DsProduct = ProductComponent.GetProductByProductID(ID);

            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
                txtProductName.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Name"]);
                txtSKU.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SKU"]);

                txtWeight.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["weight"].ToString());
                txtInventory.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Inventory"].ToString());
                txtDescription.Text = Server.HtmlDecode(DsProduct.Tables[0].Rows[0]["Description"].ToString());
                txtFeatures.Text = Server.HtmlDecode(DsProduct.Tables[0].Rows[0]["Features"].ToString());

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString().Trim()))
                    txtSalePrice.Text = "0.00";
                else
                    txtSalePrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Price"].ToString().Trim()))
                    txtPrice.Text = "0.00";
                else
                    txtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["Price"].ToString().Trim()), 2));

                txtInventory.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Inventory"].ToString());

                chkPublished.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Active"].ToString());

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["eBayCategoryID"].ToString()))
                {
                    ddlebayCategory.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["eBayCategoryID"]);
                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["eBayStoreCategoryID"].ToString()))
                {
                    ddlebayStorecategory.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["eBayStoreCategoryID"]);
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["isFreeshipping"].ToString()))
                {
                    chkfreeshipping.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["isFreeshipping"].ToString());
                }
                else
                {
                    chkfreeshipping.Checked = false;

                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Brand"].ToString()))
                {
                    txtBrand.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Brand"].ToString());
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Materials"].ToString()))
                {
                    txtmaterial.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Materials"].ToString());
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["ManufacturePartNo"].ToString()))
                {
                    txtModel.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["ManufacturePartNo"].ToString());
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["ProductSummary"].ToString()))
                {
                    txtType.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductSummary"].ToString());
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Colors"].ToString()))
                {
                    ddlColormap.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["Colors"].ToString());
                }
                
                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["eBayListingType"].ToString()))
                {
                    RBtnStore.Checked = true;
                }
                else
                {
                    if (Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["eBayListingType"]) != true)
                    {
                        RBtnStore.Checked = true;
                        RBtnAuction.Checked = false;
                    }
                    else
                    {
                        RBtnStore.Checked = false;
                        RBtnAuction.Checked = true;
                    }
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["eBayListingDay"].ToString()))
                    TxtebayListingDays.Text = (DsProduct.Tables[0].Rows[0]["eBayListingDay"].ToString());
                else
                    TxtebayListingDays.Text = "30";

                lblebayProductID.Text = string.IsNullOrEmpty((DsProduct.Tables[0].Rows[0]["eBayProductID"].ToString())) ? "---" : (DsProduct.Tables[0].Rows[0]["eBayProductID"].ToString());
                //[eBayLastUpdated]
                lblebayLastUpdated.Text = (DsProduct.Tables[0].Rows[0]["eBayLastUpdated"].ToString());

                DateTime dteBayLastUpload = DateTime.MinValue;
                DateTime.TryParse(DsProduct.Tables[0].Rows[0]["ebayLastUpdated"].ToString(), out dteBayLastUpload);
                if (dteBayLastUpload != DateTime.MinValue)
                {
                    lblebayLastUpdated.Text = dteBayLastUpload.ToString();
                    lblebayExpireDate.Text = dteBayLastUpload.AddDays(Convert.ToInt32(DsProduct.Tables[0].Rows[0]["EbayListingDay"].ToString().Trim())).ToString();
                    if (lblebayExpireDate.Text == lblebayLastUpdated.Text)
                        lblebayExpireDate.Text = "---";
                }

                #region Get Image
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                    Directory.CreateDirectory(Server.MapPath(ProductMediumPath));
                string Imagename = DsProduct.Tables[0].Rows[0]["Imagename"].ToString();
                string strFilePath = Server.MapPath(ProductMediumPath + Imagename);

                btnDelete.Visible = false;
                if (File.Exists(strFilePath))
                {
                    ViewState["DelImage"] = Imagename;
                    btnDelete.Visible = true;
                    ImgLarge.Src = ProductMediumPath + Imagename;
                }
                else
                {
                    ViewState["DelImage"] = null;
                    btnDelete.Visible = false;
                    ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                }
                #endregion
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSalePrice.Text))
            {
                if (Convert.ToDecimal(txtPrice.Text.Trim()) < Convert.ToDecimal(txtSalePrice.Text.Trim()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Sale Price cannot be greater than Price.', 'Message','txtSalePrice')", true);
                    return;
                }
            }
            if (Request.QueryString["StoreID"] != null)
            {
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
                {
                    UpdateProduct(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                else
                {
                    InsertProduct(Convert.ToInt32(Request.QueryString["StoreID"]));
                }
            }
        }

        /// <summary>
        /// Inserts the Product Data into Product Table and Buy Product Table
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        private void InsertProduct(int StoreID)
        {
            tb_Product = new tb_Product();
            tb_Product = SetValue(tb_Product);

            Int32 ProductID = Convert.ToInt32(ProductComponent.InsertProduct(tb_Product));

            if (ProductID > 0)
            {
                string strImageName = "";

                if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                {
                    strImageName = txtSKU.Text.ToString() + "_" + ProductID + ".jpg";
                    SaveImage(strImageName);
                }
                ProductComponent objProduct = new ProductComponent();
                tb_Product = new tb_Product();
                tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);
                if (string.IsNullOrEmpty(ImgLarge.Src.ToString()) || ImgLarge.Src.ToString().ToLower().Contains("image_not_available"))
                    tb_Product.ImageName = "";
                else
                    tb_Product.ImageName = strImageName;
                ProductComponent.UpdateProduct(tb_Product);

                #region Save Inventory in warehouse

                SaveInventoryInWarehouse(ProductID, 1);

                #endregion
                Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
            }
        }

        /// <summary>
        /// Sets the value of Product into Controls
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns tb_Product Table Object</returns>
        private tb_Product SetValue(tb_Product tb_Product)
        {
            if (Request.QueryString["StoreID"] != null)
                tb_Product.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(Request.QueryString["StoreID"].ToString()));

            tb_Product.Name = Convert.ToString(txtProductName.Text.Trim());
            tb_Product.SEName = CommonOperations.RemoveSpecialCharacter(txtProductName.Text.Trim().ToCharArray());
            tb_Product.SKU = txtSKU.Text.Trim();
            tb_Product.Weight = Convert.ToDecimal(txtWeight.Text.Trim());
            tb_Product.Price = Convert.ToDecimal(txtPrice.Text.Trim());
            if (!string.IsNullOrEmpty(txtSalePrice.Text))
                tb_Product.SalePrice = Convert.ToDecimal(txtSalePrice.Text.Trim());
            tb_Product.Inventory = Convert.ToInt32(txtInventory.Text.Trim());
            tb_Product.Active = chkPublished.Checked;
            tb_Product.Description = txtDescription.Text.Trim();
            tb_Product.Features = txtFeatures.Text.Trim();
            tb_Product.eBayCategoryID = Convert.ToInt64(ddlebayCategory.SelectedValue.ToString());
            tb_Product.eBayStoreCategoryID = Convert.ToInt64(ddlebayStorecategory.SelectedValue.ToString());
            if (RBtnStore.Checked == true)
                tb_Product.eBayListingType = false;
            else
                tb_Product.eBayListingType = true;
            if (!string.IsNullOrEmpty(TxtebayListingDays.Text))
                tb_Product.eBayListingDay = Convert.ToInt32(TxtebayListingDays.Text.Trim());
            tb_Product.CreatedOn = DateTime.Now;
            tb_Product.CreatedBy = Convert.ToInt32(Session["AdminID"].ToString());
            tb_Product.eBayLastUpdated = DateTime.Now;
            tb_Product.Deleted = false;
            tb_Product.IsFreeShipping = Convert.ToBoolean(chkfreeshipping.Checked);
            tb_Product.Colors = Convert.ToString(ddlColormap.SelectedValue.ToString());
            tb_Product.Brand = Convert.ToString(txtBrand.Text.ToString());
            tb_Product.Materials = Convert.ToString(txtmaterial.Text.ToString());
            tb_Product.ManufacturePartNo = Convert.ToString(txtModel.Text.ToString());
            tb_Product.ProductSummary = Convert.ToString(txtType.Text.ToString());
            return tb_Product;
        }

        /// <summary>
        /// Update the Product Data into Product Table and Buy Product Table
        /// </summary>
        /// <param name="ProdID">int ProductID</param>
        /// <param name="StoreID">int StoreID</param>
        private void UpdateProduct(int ProductID, int StoreId)
        {
            ProductComponent objProduct = new ProductComponent();
            tb_Product = new tb_Product();
            tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);
            tb_Product = SetValue(tb_Product);
            tb_Product.UpdatedBy = Convert.ToInt32(Session["AdminID"].ToString());
            tb_Product.eBayLastUpdated = DateTime.Now;
            // ProductComponent.UpdateProduct(tb_Product);
            //Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));

            if (Convert.ToInt32(ProductComponent.UpdateProduct(tb_Product)) > 0)
            {


                #region Save and Update Image
                string strImageName = "";
                try
                {
                    if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                    {
                        strImageName = txtSKU.Text.ToString() + "_" + ProductID + ".jpg";
                        string Imgname = tb_Product.ImageName.ToString();
                        if (ImgLarge.Src.Contains(ProductTempPath))
                        {
                            SaveImage(strImageName);
                        }
                        else if (!string.IsNullOrEmpty(Imgname.ToString()))
                        {
                            if (File.Exists(Server.MapPath(ProductLargePath + Imgname)))
                                File.Move(Server.MapPath(ProductLargePath + Imgname), Server.MapPath(ProductLargePath + strImageName));

                            if (File.Exists(Server.MapPath(ProductMediumPath + Imgname)))
                                File.Move(Server.MapPath(ProductMediumPath + Imgname), Server.MapPath(ProductMediumPath + strImageName));

                            if (File.Exists(Server.MapPath(ProductMicroPath + Imgname)))
                                File.Move(Server.MapPath(ProductMicroPath + Imgname), Server.MapPath(ProductMicroPath + strImageName));

                            if (File.Exists(Server.MapPath(ProductIconPath + Imgname)))
                                File.Move(Server.MapPath(ProductIconPath + Imgname), Server.MapPath(ProductIconPath + strImageName));
                        }
                    }

                    tb_Product = new tb_Product();
                    tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);

                    if (string.IsNullOrEmpty(ImgLarge.Src.ToString()) || ImgLarge.Src.ToString().ToLower().Contains("image_not_available"))
                        tb_Product.ImageName = "";
                    else
                        tb_Product.ImageName = strImageName;
                }
                catch { }

                #endregion

                ProductComponent.UpdateProduct(tb_Product);

                #region Update Inventory in warehouse
                SaveInventoryInWarehouse(Convert.ToInt32(Request.QueryString["ID"].ToString()), 2);
                #endregion
                Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
            }
        }

        #region Save Image

        /// <summary>
        /// Save Image
        /// </summary>
        /// <param name="FileName">string FileName</param>
        protected void SaveImage(string FileName)
        {
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
            ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
            ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
            ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
            ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");

            //create icon folder 
            if (!Directory.Exists(Server.MapPath(ProductIconPath)))
                Directory.CreateDirectory(Server.MapPath(ProductIconPath));

            //create Medium folder 
            if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                Directory.CreateDirectory(Server.MapPath(ProductMediumPath));

            //create Large folder 
            if (!Directory.Exists(Server.MapPath(ProductLargePath)))
                Directory.CreateDirectory(Server.MapPath(ProductLargePath));

            //create Micro folder 
            if (!Directory.Exists(Server.MapPath(ProductMicroPath)))
                Directory.CreateDirectory(Server.MapPath(ProductMicroPath));

            CommonOperations.SaveOnContentServer(Server.MapPath(ProductIconPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductMediumPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductLargePath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductMicroPath));

            if (ImgLarge.Src.Contains(ProductTempPath))
            {
                try
                {
                    CreateImage("Medium", FileName);
                    CreateImage("Icon", FileName);
                    CreateImage("Micro", FileName);
                    CreateImage("Large", FileName);
                }
                catch (Exception ex)
                {
                    lblMsg.Text += "<br />" + ex.Message;
                }
                finally
                {
                    DeleteTempFile("icon");
                }
            }
        }


        /// <summary>
        ///  Create Image
        /// </summary>
        /// <param name="Size">string Size</param>
        /// <param name="FileName">string FileName</param>
        protected void CreateImage(string Size, string FileName)
        {
            try
            {
                string strFile = null;
                String strPath = "";
                if (ImgLarge.Src.ToString().IndexOf("?") > -1)
                {
                    strPath = ImgLarge.Src.Split('?')[0];
                }
                else
                {
                    strPath = ImgLarge.Src.ToString();
                }
                strFile = Server.MapPath(strPath);
                string strFilePath = "";
                Size = Size.ToLower();
                switch (Size)
                {
                    case "large":
                        strFilePath = Server.MapPath(ProductLargePath + FileName);

                        if (Request.QueryString["Mode"] == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductLargePath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "medium":
                        strFilePath = Server.MapPath(ProductMediumPath + FileName);
                        if (Request.QueryString["Mode"] == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductMediumPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "icon":
                        strFilePath = Server.MapPath(ProductIconPath + FileName);
                        if (Request.QueryString["Mode"] == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductIconPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "micro":
                        strFilePath = Server.MapPath(ProductMicroPath + FileName);
                        if (Request.QueryString["Mode"] == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductMicroPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                }
                ResizePhoto(strFile, Size, strFilePath);
            }
            catch (Exception ex)
            {
                if (ex.Source == "System.Drawing")
                    lblMsg.Text = "<br />Error Saving " + Size + " Image..Please check that Directory exists..";
                else
                    lblMsg.Text += "<br />" + ex.Message;
                CommonComponent.ErrorLog("Product.aspx", ex.Message, ex.StackTrace);
            }

        }

        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImage(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(ImageName)))
                    File.Delete(Server.MapPath(ImageName));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ImageName));
            }
            catch (Exception ex)
            {
                lblMsg.Text += "<br />" + ex.Message;
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }

        /// <summary>
        /// Resize Photo
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="Size">string Size</param>
        /// <param name="strFilePath">string strFilePath</param>
        private void ResizePhoto(string strFile, string Size, string strFilePath)
        {
            switch (Size)
            {
                case "medium":
                    finHeight = thumbNailSizeMediam.Height;
                    finWidth = thumbNailSizeMediam.Width;
                    break;
                case "icon":
                    finHeight = thumbNailSizeIcon.Height;
                    finWidth = thumbNailSizeIcon.Width;
                    break;
                case "micro":
                    finHeight = thumbNailSizeMicro.Height;
                    finWidth = thumbNailSizeMicro.Width;
                    break;

            }
            if (Size == "large")
            {
                File.Copy(strFile, strFilePath, true);
                CommonOperations.SaveOnContentServer(strFilePath);
            }
            else
                ResizeImage(strFile, finWidth, finHeight, strFilePath);
        }

        /// <summary>
        /// Resize Images
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="FinWidth">int FinWidth</param>
        /// <param name="FinHeight">int FinHeight</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizeImage(string strFile, int FinWidth, int FinHeight, string strFilePath)
        {
            System.Drawing.Image imgVisol = System.Drawing.Image.FromFile(strFile);
            float resizePercent = 0;
            int resizedHeight = imgVisol.Height;
            int resizedWidth = imgVisol.Width;

            if (imgVisol.Height >= FinHeight && imgVisol.Width >= FinWidth)
            {
                float resizePercentHeight = 0;
                float resizePercentWidth = 0;
                resizePercentHeight = (FinHeight * 100) / imgVisol.Height;
                resizePercentWidth = (FinWidth * 100) / imgVisol.Width;
                if (resizePercentHeight < resizePercentWidth)
                {
                    resizedHeight = FinHeight;
                    resizedWidth = (int)Math.Round(resizePercentHeight * imgVisol.Width / 100.0);
                }
                if (resizePercentHeight >= resizePercentWidth)
                {
                    resizedWidth = FinWidth;
                    resizedHeight = (int)Math.Round(resizePercentWidth * imgVisol.Height / 100.0);
                }
            }
            else if (imgVisol.Width >= FinWidth && imgVisol.Height <= FinHeight)
            {
                resizedWidth = FinWidth;
                resizePercent = (FinWidth * 100) / imgVisol.Width;
                resizedHeight = (int)Math.Round((imgVisol.Height * resizePercent) / 100.0);
            }

            else if (imgVisol.Width <= FinWidth && imgVisol.Height >= FinHeight)
            {
                resizePercent = (FinHeight * 100) / imgVisol.Height;
                resizedHeight = FinHeight;
                resizedWidth = (int)Math.Round(resizePercent * imgVisol.Width / 100.0);
            }

            Bitmap resizedPhoto = new Bitmap(resizedWidth, resizedHeight, PixelFormat.Format24bppRgb);
            Graphics grPhoto = Graphics.FromImage(resizedPhoto);

            int destWidth = resizedWidth;
            int destHeight = resizedHeight;
            int sourceWidth = imgVisol.Width;
            int sourceHeight = imgVisol.Height;

            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle DestRect = new Rectangle(0, 0, destWidth, destHeight);
            Rectangle srcRect = new Rectangle(0, 0, sourceWidth, sourceHeight);
            grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grPhoto.DrawImage(imgVisol, DestRect, srcRect, GraphicsUnit.Pixel);

            GenerateImage(resizedPhoto, strFilePath, FinWidth, FinHeight);

            resizedPhoto.Dispose();
            grPhoto.Dispose();
            imgVisol.Dispose();

        }

        /// <summary>
        /// Generate Image
        /// </summary>
        /// <param name="extBMP">Bitmap extBMP</param>
        /// <param name="DestFileName">string DestFileName</param>
        /// <param name="DefWidth">int DefWidth</param>
        /// <param name="DefHeight">int DefHeight</param>
        private void GenerateImage(Bitmap extBMP, string DestFileName, int DefWidth, int DefHeight)
        {
            Encoder Enc = Encoder.SaveFlag;
            EncoderParameters EncParms = new EncoderParameters(1);
            EncoderParameter EncParm;
            ImageCodecInfo CodecInfo = GetEncoderInfo("image/jpeg");
            EncParm = new EncoderParameter(Encoder.Quality, (long)600);
            EncParms.Param[0] = new EncoderParameter(Encoder.Quality, (long)600);

            if (extBMP != null && extBMP.Width < (DefWidth) && extBMP.Height < (DefHeight))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, startX, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null && extBMP.Width < (DefWidth))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                g.DrawImage(extBMP, startX, 0);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null && extBMP.Height < (DefHeight))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, 0, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null)
            {
                extBMP.Save(DestFileName, CodecInfo, EncParms);
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
        }

        /// <summary>
        /// Get Encoder Information
        /// </summary>
        /// <param name="resizeMimeType">string resizeMimeType</param>
        /// <returns>Returns the ImageCodecInfo Object</returns>
        private static ImageCodecInfo GetEncoderInfo(string resizeMimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == resizeMimeType)
                    return codecs[i];
            return null;
        }

        /// <summary>
        /// Delete Temp files
        /// </summary>
        /// <param name="strsize">string strsize</param>
        protected void DeleteTempFile(string strsize)
        {
            try
            {
                if (strsize == "icon")
                {
                    string path = string.Empty;
                    if (ViewState["File"] != null && ViewState["File"].ToString().Trim().Length > 0)
                    {
                        path = Server.MapPath(ProductTempPath + ViewState["File"].ToString());
                    }

                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("Product.aspx", ex.Message, ex.StackTrace);
            }
        }
        #endregion

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ProductList.aspx?cancel=1&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
        }

        /// <summary>
        ///  Upload Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (fuProductIcon.FileName.Length > 0 && Path.GetExtension(fuProductIcon.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fuProductIcon.FileName.Length > 0)
                {
                    try
                    {
                        FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + fuProductIcon.FileName);
                        if (flinfo.Exists)
                        {
                            flinfo.Delete();
                        }
                    }
                    catch { }
                    ViewState["File"] = fuProductIcon.FileName.ToString();
                    fuProductIcon.SaveAs(Server.MapPath(ProductTempPath) + fuProductIcon.FileName);
                    ImgLarge.Src = ProductTempPath + fuProductIcon.FileName;
                    lblMsg.Text = "";
                }
                else
                {
                    ViewState["File"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_fuProductIcon');});", true);
            }
        }

        /// <summary>
        /// Get eBay Category List
        /// </summary>
        private void BindebayCategory()
        {
            ProductComponent objProductComp = new ProductComponent();
            dseBaycategory = objProductComp.GeteBayCategory();

            DataRow[] drCatagories = null;
            ListItem LT2 = new ListItem();
            if (dseBaycategory != null && dseBaycategory.Tables[0].Rows.Count > 0)
            {
                drCatagories = dseBaycategory.Tables[0].Select("ParentCategoryID=0");
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = selDR["Name"].ToString();
                    LT2.Value = selDR["ebaycategoryid"].ToString();
                    ddlebayCategory.Items.Add(LT2);
                    if (dseBaycategory.Tables[0].Select("ParentCategoryID=" + selDR["EbayCategoryID"]).Length > 0)
                        SetChildCategoryeBay(Convert.ToInt32(selDR["EbayCategoryID"].ToString()), 0, selDR["Name"].ToString());
                }
            }
        }

        /// <summary>
        /// Set Child category for eBay
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="Number">int Number</param>
        /// <param name="strPrevName">string strPrevName</param>
        private void SetChildCategoryeBay(int ID, int Number, string strPrevName)
        {
            if (Number >= 10)
                return;
            DataRow[] drCatagories = dseBaycategory.Tables[0].Select("ParentCategoryID=" + ID.ToString());
            ListItem LT2;
            if (drCatagories.Length > 0)
            {
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = strPrevName + " > " + selDR["Name"].ToString();
                    LT2.Value = selDR["ebaycategoryid"].ToString();
                    ddlebayCategory.Items.Add(LT2);
                    if (dseBaycategory.Tables[0].Select("ParentCategoryID=" + selDR["ebaycategoryid"]).Length > 0)
                    {
                        SetChildCategoryeBay(Convert.ToInt32(selDR["ebaycategoryid"].ToString()), Number + 1, strPrevName + " > " + selDR["Name"].ToString());
                    }
                }
            }
        }

        /// <summary>
        /// Binds the eBay Store Category
        /// </summary>
        private void BindeBayStoreCategory()
        {
            ProductComponent objProdComp = new ProductComponent();
            dseBayStoreCategory = objProdComp.GetebayStoreCategory();
            DataRow[] drCatagories = null;
            ListItem LT2 = new ListItem();
            ddlebayStorecategory.Items.Clear();
            int count = 1;
            if (dseBayStoreCategory != null && dseBayStoreCategory.Tables[0].Rows.Count > 0)
            {
                drCatagories = dseBayStoreCategory.Tables[0].Select("ParentstorecategoryID=0");
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = selDR["Name"].ToString();
                    LT2.Value = selDR["ebaystorecategoryid"].ToString();

                    ddlebayStorecategory.Items.Add(LT2);
                    if (dseBayStoreCategory.Tables[0].Select("ParentstorecategoryID=" + selDR["ID"]).Length > 0)
                        SetChildStoreCategory(Convert.ToInt64(selDR["ID"].ToString()), count, selDR["Name"].ToString());
                }
            }
        }

        /// <summary>
        /// Sets the Child Store Category
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="Number">int Number</param>
        /// <param name="strPrevName">string strPrevName</param>
        private void SetChildStoreCategory(Int64 ID, int Number, string strPrevName)
        {
            int count = Number;
            if (count >= 15)
                return;
            DataRow[] drCatagories = dseBayStoreCategory.Tables[0].Select("ParentstorecategoryID=" + ID.ToString());
            ListItem LT2;
            int innercount = 0;
            if (drCatagories.Length > 0)
            {
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = strPrevName + " > " + selDR["Name"].ToString();
                    LT2.Value = selDR["ebaystorecategoryid"].ToString();
                    ddlebayStorecategory.Items.Add(LT2);
                    if (dseBayStoreCategory.Tables[0].Select("ParentstorecategoryID=" + selDR["ebaystorecategoryid"]).Length > 0)
                    {
                        innercount++;
                        SetChildStoreCategory(Convert.ToInt64(selDR["ebaystorecategoryid"].ToString()), innercount + Number, strPrevName + " > " + selDR["Name"].ToString());
                    }
                }
            }
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            DeleteImage(ProductLargePath + ViewState["DelImage"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage"].ToString());
            ViewState["DelImage"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
            btnDelete.Visible = false;
        }


        #region Code for Ware House

        /// <summary>
        /// Gets the Warehouse List
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Mode">int Mode</param>
        private void GetWarehouseList(int ProductID, int Mode)
        {
            ProductComponent objProdComp = new ProductComponent();
            dsWarehouse = objProdComp.GetWarehouseList(Mode, ProductID);
            if (dsWarehouse != null && dsWarehouse.Tables[0].Rows.Count > 0)
            {
                grdWarehouse.DataSource = dsWarehouse;
                grdWarehouse.DataBind();
            }
            else
            {
                grdWarehouse.DataSource = null;
                grdWarehouse.DataBind();
            }
        }

        /// <summary>
        /// WareHouse Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdWarehouse_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TextBox txtWarehouseInventory = (TextBox)e.Row.FindControl("txtInventory");
                if (txtWarehouseInventory != null)
                    InventoryTotal += int.TryParse(txtWarehouseInventory.Text.Trim().ToString(), out InventoryTotal) ? InventoryTotal : 0;
            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblTotal = (Label)e.Row.FindControl("lblTotal");
                if (lblTotal != null)
                    lblTotal.Text = InventoryTotal.ToString();
            }
        }

        /// <summary>
        /// Saves the Inventory in Warehouse
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="mode">int ProductID</param>
        protected void SaveInventoryInWarehouse(int ProductID, int mode)
        {

            ProductComponent objProductComp = new ProductComponent();
            int InventoryTotal = 0;
            foreach (GridViewRow row in grdWarehouse.Rows)
            {
                int Inventory = 0;
                Label lblWarehouse = (Label)row.FindControl("lblWarehouseID");
                TextBox txtInventory = (row.FindControl("txtInventory") as TextBox);
                if (txtInventory != null)
                {
                    if (int.TryParse(txtInventory.Text.Trim(), out Inventory))
                    {
                        objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, Inventory, mode, false);
                        InventoryTotal += Inventory;
                    }
                    else
                    {
                        objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, 0, mode, false);
                    }
                }
                else
                {
                    objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, 0, mode, false);
                }
            }
            objProductComp.UpdateProductinventory(ProductID, InventoryTotal, Convert.ToInt32(Request.QueryString["StoreID"]));
        }
        #endregion
    }
}