using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using System.Data;
using System.Drawing;
using Solution.Bussines.Components;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Solution.Bussines.Components.AdminCommon;
namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductBestBuy : BasePage
    {
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
        ConfigurationComponent objConfiguration = new ConfigurationComponent();

        int CurrentStoreID = 1;
        string storeID = "1";

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
                SetManufacture();
                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");

                if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null)
                {
                    BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                    BindDataProductBuy(Convert.ToInt32(Request.QueryString["ID"]));
                    lblHeader.Text = "Edit Product";
                }
                else
                {
                    ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                }
            }
            if (Request.QueryString["StoreID"] != null)
                AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            BindSize();
        }

        /// <summary>
        /// Bind Sizes
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
        /// Bind the Manufactures into Drop down
        /// </summary>
        private void SetManufacture()
        {
            DataSet dsManufacture = ManufactureComponent.GetAllManufactureDetail();
            ddlManufacture.DataSource = dsManufacture.Tables[0];
            ddlManufacture.DataTextField = "Name";
            ddlManufacture.DataValueField = "ManufactureID";
            ddlManufacture.DataBind();
            ListItem li = new ListItem("Select Manufacturer", "-1");
            ddlManufacture.Items.Insert(0, li);
            ListItem li1 = new ListItem("Others", "0");
            int i = ddlManufacture.Items.Count;

            ddlManufacture.Items.Insert(i, li1);
        }

        /// <summary>
        /// Generates Randoms the UPC Number
        /// </summary>
        /// <param name="StoreId">int StoreId</param>
        protected void RandomUPCNumber(int StoreId)
        {
            int Flag = 0;
            Random ranUpcNo = new Random();
            if (Flag == 0)
            {
                do
                {
                    int UpcNo = ranUpcNo.Next(1000000000);
                    //SQLAccess dbAccess = new SQLAccess();
                    DataSet ds = (CommonComponent.GetCommonDataSet("select ISNULL(UPC,0) as UPC from tb_product where StoreID=" + StoreId.ToString() + " And UPC='" + UpcNo.ToString() + "'"));
                    if (ds != null && ds.Tables[0].Rows.Count > 0)
                    {
                        Int64 ChjkUPC = Convert.ToInt64(ds.Tables[0].Rows[0][0].ToString());
                        if (ChjkUPC > 0 && ChjkUPC != null)
                        {
                            Flag = 0;
                        }
                        else
                        {
                            Flag = 1;
                            txtGtin.Text = UpcNo.ToString("D10");
                        }
                    }
                    else
                    {
                        Flag = 1;
                        txtGtin.Text = UpcNo.ToString("D10");
                    }
                }
                while (Flag != 1);
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
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
        /// Binds the Data of Best Buy Product
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="StoreID">int StoreID</param>
        private void BindData(int ID, int StoreID)
        {
            if (Session["StoreID"] != null)
            {
                storeID = Session["StoreID"].ToString();
            }
            DataSet DsProduct = new DataSet();
            DsProduct = ProductComponent.GetProductDetailByID(ID, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            GetValue(DsProduct, StoreID);

        }

        /// <summary>
        /// Binds the Data Product of Best Buy
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        private void BindDataProductBuy(int ProductID)
        {
            ProductBuyComponent objProductBuyComp = new ProductBuyComponent();
            DataSet DsProductBuy = new DataSet();
            DsProductBuy = objProductBuyComp.GetProductBuyData(ProductID);
            GetBuyProductValue(DsProductBuy);
        }

        /// <summary>
        /// Gets the Best Buy Product Value
        /// </summary>
        /// <param name="DsbuyProduct">Dataset DsbuyProduct</param>
        private void GetBuyProductValue(DataSet DsbuyProduct)
        {
            if (DsbuyProduct != null && DsbuyProduct.Tables[0].Rows.Count > 0)
            {
                if (string.IsNullOrEmpty(DsbuyProduct.Tables[0].Rows[0]["ShippingRateExpedited"].ToString().Trim()))
                {
                    txtShippingRateExpedited.Text = "0.00";
                }
                else
                {
                    txtShippingRateExpedited.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsbuyProduct.Tables[0].Rows[0]["ShippingRateExpedited"].ToString().Trim()), 2));
                }

                if (string.IsNullOrEmpty(DsbuyProduct.Tables[0].Rows[0]["ShippingRateStandard"].ToString().Trim()))
                {
                    txtShippingRateStandard.Text = "0.00";
                }
                else
                {
                    txtShippingRateStandard.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsbuyProduct.Tables[0].Rows[0]["ShippingRateStandard"].ToString().Trim()), 2));
                }

                if (string.IsNullOrEmpty(DsbuyProduct.Tables[0].Rows[0]["OfferExpeditedShipping"].ToString().Trim()))
                {
                    txtOfferExpeditedShipping.Text = "";
                }
                else
                {
                    txtOfferExpeditedShipping.Text = Convert.ToString(DsbuyProduct.Tables[0].Rows[0]["OfferExpeditedShipping"].ToString().Trim());
                }


                txtAssemblyLevel.Text = DsbuyProduct.Tables[0].Rows[0]["Ass_Type"].ToString().Trim();
                txtBoardType.Text = DsbuyProduct.Tables[0].Rows[0]["Board_Type"].ToString().Trim();
                txtBrightness.Text = DsbuyProduct.Tables[0].Rows[0]["Brightness"].ToString().Trim();
                txtCardType.Text = DsbuyProduct.Tables[0].Rows[0]["Card_Type"].ToString().Trim();
                txtClosure.Text = DsbuyProduct.Tables[0].Rows[0]["Closure"].ToString().Trim();
                txtDispenser.Text = DsbuyProduct.Tables[0].Rows[0]["Dispenser_Type"].ToString().Trim();
                txtEaselType.Text = DsbuyProduct.Tables[0].Rows[0]["Easel_Type"].ToString().Trim();
            }
        }

        /// <summary>
        /// Gets the value
        /// </summary>
        /// <param name="Ds">Dataset ds</param>
        /// <param name="StoreID">int StoreID</param>
        private void GetValue(DataSet Ds, int StoreID)
        {
            if (Ds != null && Ds.Tables[0].Rows.Count > 0)
            {
                txtGtin.Text = Ds.Tables[0].Rows[0]["UPC"].ToString().Trim();

                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Isbn"].ToString()))
                {
                    txtISBN.Text = Ds.Tables[0].Rows[0]["Isbn"].ToString().Trim();
                }

                ddlManufacture.SelectedValue = Ds.Tables[0].Rows[0]["ManufactureID"].ToString();

                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ManufacturePartNo"].ToString()))
                {
                    txtManufactrPartNo.Text = Ds.Tables[0].Rows[0]["ManufacturePartNo"].ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Asin"].ToString()))
                {
                    txtAsin.Text = Ds.Tables[0].Rows[0]["Asin"].ToString().Trim();
                }

                txtSKU.Text = Ds.Tables[0].Rows[0]["SKU"].ToString();
                txtSerialNo.Text = Ds.Tables[0].Rows[0]["Srno"].ToString();
                txtTitle.Text = Ds.Tables[0].Rows[0]["Name"].ToString();
                txtWeight.Text = Ds.Tables[0].Rows[0]["Weight"].ToString();
                txtMsrp.Text = Ds.Tables[0].Rows[0]["Price"].ToString();

                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Saleprice"].ToString()))
                    txtListingPrice.Text = Ds.Tables[0].Rows[0]["SalePrice"].ToString();

                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ProductSetID"].ToString()))
                {
                    txtProductSetID.Text = Ds.Tables[0].Rows[0]["ProductSetID"].ToString().Trim();
                }

                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Avail"].ToString()))
                    txtAvailability.Text = Ds.Tables[0].Rows[0]["Avail"].ToString().Trim();

                txtInventory.Text = Ds.Tables[0].Rows[0]["Inventory"].ToString().Trim();
                int ChkLowInventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(*) from tb_Product where ProductID = " + Request.QueryString["ID"].ToString() + " and (ISNULL( tb_Product.Inventory,0) <  ISNULL( tb_Product.LowInventory,0))"));
                if (ChkLowInventory > 0)
                {
                    ImgToggelInventory.Visible = true;
                }
                chkPublished.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["Active"].ToString().Trim());
                //For discontinue radiobutton
                bool Limited = false;

                Boolean.TryParse(Ds.Tables[0].Rows[0]["Discontinue"].ToString().Trim(), out Limited);
                if (Limited == true)
                {
                    RBtnContiYes.Checked = true;
                    RBtnContNo.Checked = false;
                }
                else
                {
                    RBtnContNo.Checked = true;
                    RBtnContiYes.Checked = false;
                }

                txtDisplayOrder.Text = Ds.Tables[0].Rows[0]["DisplayOrder"].ToString().Trim();
                txtwidth.Text = Convert.ToString(Ds.Tables[0].Rows[0]["width"].ToString());
                txtheight.Text = Convert.ToString(Ds.Tables[0].Rows[0]["height"].ToString());
                txtlength.Text = Convert.ToString(Ds.Tables[0].Rows[0]["length"].ToString());

                //Description tab region
                txtDescription.Text = Server.HtmlDecode(Ds.Tables[0].Rows[0]["Description"].ToString().Trim());
                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Features"].ToString()))
                {
                    txtFeatures.Text = Server.HtmlDecode(Ds.Tables[0].Rows[0]["Features"].ToString().Trim());
                }
                txtWarranty.Text = Ds.Tables[0].Rows[0]["Extended-warranty"].ToString().Trim();

                //SEO tab region
                txtSETitle.Text = Ds.Tables[0].Rows[0]["SETitle"].ToString().Trim();
                txtSEOImageTooltip.Text = Ds.Tables[0].Rows[0]["ToolTip"].ToString().Trim();
                txtSEKeywords.Text = Ds.Tables[0].Rows[0]["SEKeywords"].ToString().Trim();
                txtSEDesc.Text = Ds.Tables[0].Rows[0]["SEDescription"].ToString().Trim();

                //Image region
                #region Get Image
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                    Directory.CreateDirectory(Server.MapPath(ProductMediumPath));
                string Imagename = Ds.Tables[0].Rows[0]["Imagename"].ToString();
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

                //category region
                txtMainCategory.Text = Ds.Tables[0].Rows[0]["MainCategory"].ToString().Trim();
                ddlCategory.SelectedValue = Ds.Tables[0].Rows[0]["BuyCategoryID"].ToString().Trim();
                //Fields region
                txtLocation.Text = Convert.ToString(Ds.Tables[0].Rows[0]["Location"]).Replace("~", ",");
                txtTagName.Text = Ds.Tables[0].Rows[0]["TagName"].ToString().Trim();
                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Condition"].ToString()))
                {
                    if (Ds.Tables[0].Rows[0]["Condition"].ToString().Trim() == "Refurbished") { ddlCondition.SelectedValue = "Refurbished"; }
                    else { ddlCondition.SelectedValue = "New"; }
                }
                else
                {
                    ddlCondition.SelectedValue = "New";
                }
                if (Ds.Tables[0].Rows[0]["IsShipSeparately"].ToString().Trim() == "" || Ds.Tables[0].Rows[0]["IsShipSeparately"].ToString().Trim() == null)
                    chkIsShipSeparate.Checked = false;
                else
                    chkIsShipSeparate.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["IsShipSeparately"].ToString().Trim()); ;

            }
        }

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
        /// Sets the value
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns the tb_Product Object</returns>
        private tb_Product SetValue(tb_Product product)
        {
            if (Request.QueryString["StoreID"] != null)
                product.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(Request.QueryString["StoreID"].ToString()));
            //Set Manufacture ID foreign key value
            product.tb_ManufactureReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Manufacture", "ManufactureID", Convert.ToInt32(ddlManufacture.SelectedValue));
            //tb_Product.tb_ManufactureReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Manufacture", "ManufactureID", Convert.ToInt32(ddlManufacture.SelectedValue));
            product.SellerID = Convert.ToInt32(txtSellerID.Text.Trim());
            product.UPC = txtGtin.Text.Trim().ToString();
            product.Isbn = txtISBN.Text.Trim().ToString();
            product.ManufacturePartNo = txtManufactrPartNo.Text.Trim().ToString();
            product.ASIN = txtAsin.Text.Trim().ToString();
            product.SKU = txtSKU.Text.Trim().ToString();
            product.Srno = Convert.ToInt32(txtSerialNo.Text.Trim().ToString());
            product.Name = txtTitle.Text.Trim().ToString();
            product.Weight = Convert.ToDecimal(txtWeight.Text.Trim().ToString());
            product.Price = Convert.ToDecimal(txtMsrp.Text.Trim().ToString());

            if (!string.IsNullOrEmpty(txtListingPrice.Text))
                product.SalePrice = Convert.ToDecimal(txtListingPrice.Text.Trim());

            //product.buyKeywords = txtBuyKeywords.Text.Trim();

            product.ProductSetID = (txtProductSetID.Text.Trim());
            product.Avail = txtAvailability.Text.Trim().ToString();
            product.Inventory = Convert.ToInt32(txtInventory.Text.Trim().ToString());
            product.Active = chkPublished.Checked;
            if (RBtnContiYes.Checked == true)
            {
                product.Discontinue = true;
            }
            else
            {
                product.Discontinue = false;
            }
            if (!string.IsNullOrEmpty(txtDisplayOrder.Text))
                product.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim().ToString());

            if (!string.IsNullOrEmpty(txtwidth.Text))
                product.Width = txtwidth.Text.Trim().ToString();

            if (!string.IsNullOrEmpty(txtDisplayOrder.Text))
                product.Height = txtheight.Text.Trim().ToString();

            if (!string.IsNullOrEmpty(txtlength.Text))
                product.Length = txtlength.Text.Trim().ToString();

            if (!string.IsNullOrEmpty(txtDescription.Text))
                product.Description = txtDescription.Text.Trim().ToString();

            if (!string.IsNullOrEmpty(txtFeatures.Text))
                product.Features = txtFeatures.Text.Trim().ToString();

            if (!string.IsNullOrEmpty(txtWarranty.Text))
                product.Extended_warranty = txtWarranty.Text.Trim().ToString();

            product.SETitle = txtSETitle.Text.Trim().ToString();
            product.SEDescription = txtSEDesc.Text.Trim().ToString();
            product.SEKeywords = txtSEKeywords.Text.Trim().ToString();
            product.ToolTip = txtSEOImageTooltip.Text.Trim().ToString();

            product.MainCategory = txtMainCategory.Text.Trim().ToString();
            product.BuyCategoryID = Convert.ToInt32(ddlCategory.SelectedValue);

            product.Location = txtLocation.Text.Trim().ToString();
            product.TagName = txtTagName.Text.Trim().ToString();
            product.Condition = ddlCondition.SelectedValue.ToString();
            product.IsShipSeparately = chkIsShipSeparate.Checked;

            product.CreatedOn = DateTime.Now;
            product.CreatedBy = Convert.ToInt32(Session["AdminID"].ToString());
            product.Deleted = false;

            return product;
        }

        /// <summary>
        /// Sets the Product Buy Value
        /// </summary>
        /// <param name="ProductId">int ProductId  </param>
        /// <returns>Returns the tb_ProductBuy object</returns>
        private tb_ProductBuy SetProductBuyValue(Int32 ProductId)
        {
            tb_ProductBuy productBuy = new tb_ProductBuy();
            productBuy.ProductID = ProductId;

            if (!string.IsNullOrEmpty(txtShippingRateExpedited.Text))
                productBuy.ShippingRateExpedited = Convert.ToDecimal(txtShippingRateExpedited.Text.Trim().ToString());
            else
                productBuy.ShippingRateExpedited = decimal.Zero;
            if (!string.IsNullOrEmpty(txtShippingRateStandard.Text))
                productBuy.ShippingRateStandard = Convert.ToDecimal(txtShippingRateStandard.Text.Trim().ToString());
            else
                productBuy.ShippingRateStandard = decimal.Zero;
            productBuy.OfferExpeditedShipping = txtOfferExpeditedShipping.Text.Trim().ToString();
            productBuy.Ass_Type = txtAssemblyLevel.Text.Trim();
            productBuy.Board_Type = txtBoardType.Text.Trim();
            productBuy.Brightness = txtBrightness.Text.Trim();
            productBuy.Card_Type = txtCardType.Text.Trim();
            productBuy.Closure = txtClosure.Text.Trim();
            productBuy.Dispenser_Type = txtDispenser.Text.Trim().ToString();
            productBuy.Easel_Type = txtEaselType.Text.Trim();
            return productBuy;
        }

        /// <summary>
        /// Inserts the Product
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        private void InsertProduct(int StoreID)
        {
            tb_Product = new tb_Product();
            tb_Product = SetValue(tb_Product);
            ProductComponent objProduct = new ProductComponent();

            Int32 ProductID = Convert.ToInt32(ProductComponent.InsertProduct(tb_Product));
            tb_ProductBuy objProductBuy = new tb_ProductBuy();
            if (ProductID > 0)
            {

                objProductBuy = SetProductBuyValue(ProductID);

                if (objProductBuy != null)
                {
                    int BuyProductId = ProductBuyComponent.InsertProductBuy(objProductBuy);
                }

                string strImageName = "";

                if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                {
                    strImageName = txtSKU.Text.ToString() + "_" + ProductID + ".jpg";
                    SaveImage(strImageName);
                }

                tb_Product = new tb_Product();
                tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);
                if (string.IsNullOrEmpty(ImgLarge.Src.ToString()) || ImgLarge.Src.ToString().ToLower().Contains("image_not_available"))
                    tb_Product.ImageName = "";
                else
                    tb_Product.ImageName = strImageName;
                ProductComponent.UpdateProduct(tb_Product);

                Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
            }
        }

        /// <summary>
        /// Updates the Product
        /// </summary>
        /// <param name="ProdID">int ProdID</param>
        /// <param name="StoreID">int StoreID</param>
        private void UpdateProduct(int ProdID, int StoreID)
        {
            ProductComponent objProduct = new ProductComponent();
            tb_Product = new tb_Product();

            tb_Product = objProduct.GetAllProductDetailsbyProductID(ProdID);

            tb_Product = SetValue(tb_Product);
            tb_Product.UpdatedBy = Convert.ToInt32(Session["AdminID"].ToString());
            tb_Product.UpdatedOn = DateTime.Now;
            ProductComponent.UpdateProduct(tb_Product);
            tb_ProductBuy objProductBuy = new tb_ProductBuy();
            ProductBuyComponent objProductBuyComp = new ProductBuyComponent();
            DataSet ds = objProductBuyComp.GetProductBuyData(ProdID);

            objProductBuy = SetProductBuyValue(ProdID);
            objProductBuy.ProductBuyID = Convert.ToInt32(ds.Tables[0].Rows[0]["ProductBuyID"].ToString());
            ProductBuyComponent.UpdateProduct(objProductBuy);

            //if (Convert.ToInt32(ProductComponent.UpdateProduct(tb_Product)) > 0)
            {
                //ProductComponent.UpdateProduct(tb_Product);
                string strImageName = "";

                if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                {
                    strImageName = txtSKU.Text.ToString() + "_" + ProdID + ".jpg";
                    SaveImage(strImageName);
                }

                tb_Product = new tb_Product();
                tb_Product = objProduct.GetAllProductDetailsbyProductID(ProdID);
                if (string.IsNullOrEmpty(ImgLarge.Src.ToString()) || ImgLarge.Src.ToString().ToLower().Contains("image_not_available"))
                    tb_Product.ImageName = "";
                else
                    tb_Product.ImageName = strImageName;
                int count = ProductComponent.UpdateProduct(tb_Product);
                if (count > 0)
                {
                    Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                }
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
        ///  Random Number Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkRandomNo_Click(object sender, EventArgs e)
        {
            int UPCStoreId = 0;
            if (Request.QueryString["StoreID"] != null)
            {
                UPCStoreId = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
            }
            else
            {
                UPCStoreId = CurrentStoreID;
            }
            RandomUPCNumber(UPCStoreId);
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
    }
}