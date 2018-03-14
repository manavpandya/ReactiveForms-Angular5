using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using System.Data;
using Solution.Bussines.Components;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Solution.Bussines.Components.AdminCommon;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductBuy : BasePage
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

        int pProductID = 0;
        int CurrentStoreID = 0;
        int OldStoreID = 0;

        int NewCurrentStoreID = 1;
        string storeID = "1";

        public bool chkUPC = false;
        string StrUPC = string.Empty;
        string StrUPCReal = string.Empty;

        public static string Vendorsku = "";
        public static string Productsku = "";

        public static DataTable dt;
        DataColumn dcDopshipper;
        DataColumn dcSKU;
        DataColumn dcPriority;
        DataColumn dcVendorSKUID;
        DataColumn dcVendorID;
        DataColumn dcDopshipperName;

        public static DataTable dtAssembler;
        DataColumn dcProductName;
        DataColumn dcProductSKU;
        DataColumn dcQuantity;
        DataColumn dcProductID;


        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["StoreID"] != null && Request.QueryString["ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ID"].ToString()) && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
            {
                pProductID = Convert.ToInt32(Request.QueryString["ID"]);
                CurrentStoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            }
            if (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
            {
                CurrentStoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            }
            if (Request.QueryString["CloneID"] != null && !string.IsNullOrEmpty(Request.QueryString["CloneID"].ToString()))
            {
                OldStoreID = Convert.ToInt32(Request.QueryString["CloneID"]);
            }
            BindSize();
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
            btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
            btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            if (!IsPostBack)
            {
                TempTable();
                AssemblerTempTable();
                BindVendorSKU();
                BindProductSKU();


                if (Request.QueryString["StoreID"] != null && Request.QueryString["OLDID"] != null && !string.IsNullOrEmpty(Request.QueryString["OLDID"].ToString()) && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
                {
                    string NewProductID = IsProductAvailable(Request.QueryString["OLDID"].ToString(), Request.QueryString["StoreId"].ToString());
                    if (string.IsNullOrEmpty(NewProductID))
                    {
                        pronotavailmsg.Text = "This Product is not available in this Store. Do you want to add the same ?";
                        trAddNewProduct.Visible = true;
                        tableproduct.Visible = false;
                    }
                    else
                    {
                        trAddNewProduct.Visible = false;
                        tableproduct.Visible = true;
                        Response.Redirect("ProductBuy.aspx?Storeid=" + Convert.ToInt32(Request.QueryString["StoreID"]) + "&Mode=Edit&ID=" + NewProductID.ToString() + "");
                    }
                }
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["StoreID"] != null)
                {
                    if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                    {

                    }
                    else
                    {
                        StrUPC = Convert.ToString(CommonComponent.GetScalarCommonData("select TOP 1 UPC from tb_upcmaster where sku is null ORDER BY NEWID() "));
                        if (StrUPC != null && StrUPC.Length > 0)
                        {
                            txtGtin.Text = StrUPC;
                        }
                    }
                }
            }
            if (!IsPostBack)
            {
                BindManufacture(CurrentStoreID);
                BindAllCombo(CurrentStoreID);
                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");

                if (Request.QueryString["StoreID"] != null)
                {
                    if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null && Request.QueryString["CloneID"] == null)
                    {
                        BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                        BindDataProductBuy(Convert.ToInt32(Request.QueryString["ID"]));
                        lblHeader.Text = "Edit Product";
                        msgid.Visible = false;
                    }
                    if (Request.QueryString["Mode"] == null && Request.QueryString["ID"] != null && Request.QueryString["CloneID"] != null)
                    {
                        BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["CloneID"]));
                        msgid.Visible = true;
                    }
                    if (Request.QueryString["Mode"] == null && Request.QueryString["ID"] == null && Request.QueryString["CloneID"] == null)
                    {
                        ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                    }
                    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                }
            }
            //else
            //{
            //    Int32 invtCount = 0;
            //    foreach (GridViewRow gr in grdWarehouse.Rows)
            //    {
            //        TextBox txtin = (TextBox)gr.FindControl("txtInventory");
            //        Label lblTotal = (Label)grdWarehouse.FooterRow.FindControl("lblTotal");

            //        if (txtin.Text.ToString().Trim() != "")
            //        {
            //            invtCount += Convert.ToInt32(txtin.Text.ToString());
            //        }
            //        lblTotal.Text = invtCount.ToString();
            //        txtInventory.Text = invtCount.ToString();
            //    }
            //}
            if (Request.QueryString["StoreID"] != null)
                AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
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
        /// Bind Manufacture Dropdown
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
            }
            else
            {
                ddlManufacture.DataSource = null;
            }
            ddlManufacture.DataBind();
            ddlManufacture.Items.Insert(0, new ListItem("Select Manufacturer", "0"));
            if (dsManufacture != null && dsManufacture.Tables.Count > 0 && dsManufacture.Tables[0].Rows.Count > 0)
                ddlManufacture.SelectedIndex = 1;
        }

        /// <summary>
        /// Generates Randoms the UPC number
        /// </summary>
        /// <param name="StoreId">int StoreID</param>
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

        private void BindAllCombo(Int32 StoreID)
        {
            BindProductType(StoreID);
            BindProductTypeDelivery(StoreID);
        }

        #region Generate Barcode From OrderNo. By Girish

        /// <summary>
        /// Generates the Barcode
        /// </summary>
        /// <param name="UPCCode">string UPCCode</param>
        private void GenerateBarcode(String UPCCode)
        {

            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
            CreateFolder(FPath.ToString());
            if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png")))
            {
                DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                bCodeControl.BarCode = UPCCode.Trim();
                bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                bCodeControl.BarCodeHeight = 70;
                bCodeControl.ShowHeader = false;
                bCodeControl.ShowFooter = true;
                bCodeControl.FooterText = "UPC-" + UPCCode.Trim();
                bCodeControl.Size = new System.Drawing.Size(250, 100);
                bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"));
            }
            trBarcode.Visible = true;
            imgOrderBarcode.Src = FPath + "/UPC-" + UPCCode.Trim() + ".png";
        }

        /// <summary>
        /// Creates the Folder at Specified Path
        /// </summary>
        /// <param name="FPath">string FPath</param>
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }

        #endregion

        /// <summary>
        /// Binds the Data
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="StoreID">int StoreID</param>
        private void BindData(int ID, int StoreID)
        {
            //if (Session["StoreID"] != null)
            //{
            //    storeID = Session["StoreID"].ToString();
            //}
            DataSet DsProduct = new DataSet();
            DsProduct = ProductComponent.GetProductDetailByID(ID, Convert.ToInt32(StoreID));
            GetValue(DsProduct, StoreID);
        }

        /// <summary>
        /// Binds the Data Products of Buy
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
        /// Bind Product Type Dropdown
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        protected void BindProductType(Int32 StoreID)
        {
            DataSet dsProductType = new DataSet();
            dsProductType = ProductTypeComponent.GetProductTypeByStoreID(StoreID);
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
            ddlProductType.Items.Insert(0, new ListItem("Select Product Type", "0"));
            // if (dsProductType != null && dsProductType.Tables.Count > 0 && dsProductType.Tables[0].Rows.Count > 0)
            //     ddlProductType.SelectedIndex = 1;
        }

        protected void BindProductTypeDelivery(Int32 StoreID)
        {
            DataSet dsProductTypeDelivery = new DataSet();
            dsProductTypeDelivery = ProductTypeComponent.GetProductTypeDeliveryByStoreID(StoreID);
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
            ddlProductTypeDelivery.Items.Insert(0, new ListItem("Select Product Type Delivery", "0"));

            if (ddlProductTypeDelivery.SelectedIndex == 0)
            {
                grdAssembler.Visible = false;
                grdDropShip.Visible = false;
            }

            // if (dsProductTypeDelivery != null && dsProductTypeDelivery.Tables.Count > 0 && dsProductTypeDelivery.Tables[0].Rows.Count > 0)
            // ddlProductTypeDelivery.SelectedIndex = 1;
        }

        /// <summary>
        /// Gets the Buy Product Value
        /// </summary>
        /// <param name="DsbuyProduct">DataSet DsbuyProduct</param>
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
        /// Gets the Value
        /// </summary>
        /// <param name="Ds">Dataset Ds</param>
        /// <param name="StoreID">int StoreID</param>
        private void GetValue(DataSet Ds, int StoreID)
        {
            if (Ds != null && Ds.Tables[0].Rows.Count > 0)
            {
                txtGtin.Text = Ds.Tables[0].Rows[0]["UPC"].ToString().Trim();
                try
                {
                    if (Convert.ToString(Ds.Tables[0].Rows[0]["UPC"]) != "")
                    {

                        String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                        if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + Convert.ToString(Ds.Tables[0].Rows[0]["UPC"]).Trim() + ".png")))
                        {
                            trBarcode.Visible = true;
                            imgOrderBarcode.Src = FPath + "/UPC-" + Convert.ToString(Ds.Tables[0].Rows[0]["UPC"]).Trim() + ".png";
                        }
                        else
                        {
                            GenerateBarcode(Convert.ToString(Ds.Tables[0].Rows[0]["UPC"]).Trim());
                        }
                    }
                }
                catch { }

                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Isbn"].ToString()))
                {
                    txtISBN.Text = Ds.Tables[0].Rows[0]["Isbn"].ToString().Trim();
                }
                try
                {
                    if (Request.QueryString["CloneID"] == null)
                    {
                        ddlManufacture.SelectedValue = Ds.Tables[0].Rows[0]["ManufactureID"].ToString();
                    }
                }
                catch
                {
                    ddlManufacture.SelectedIndex = 0;
                }
                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["ManufacturePartNo"].ToString()))
                {
                    txtManufactrPartNo.Text = Ds.Tables[0].Rows[0]["ManufacturePartNo"].ToString().Trim();
                }
                if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Asin"].ToString()))
                {
                    txtAsin.Text = Ds.Tables[0].Rows[0]["Asin"].ToString().Trim();
                }

                txtSKU.Text = Ds.Tables[0].Rows[0]["SKU"].ToString();

                if (txtGtin.Text != null && txtGtin.Text != "")
                    txtGtin.Attributes.Add("ReadOnly", "true");

                string ckupcexist = Convert.ToString(CommonComponent.GetScalarCommonData("select SKU from tb_upcmasterreal where sku ='" + txtSKU.Text + "'"));
                string ckupcno = Convert.ToString(CommonComponent.GetScalarCommonData("select UPC from tb_upcmasterreal where sku ='" + txtSKU.Text + "'"));
                if (ckupcexist != null && ckupcexist.Length > 0)
                {
                    // txtUPC.Text = ckupcno;
                    lbtngetUPC.Visible = false;
                }
                else
                {
                    lbtngetUPC.Visible = true;
                }


                txtSerialNo.Text = Ds.Tables[0].Rows[0]["Srno"].ToString();
                txtTitle.Text = Ds.Tables[0].Rows[0]["Name"].ToString();

                txtWeight.Text = Ds.Tables[0].Rows[0]["Weight"].ToString();

                if (string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["Price"].ToString().Trim()))
                    txtMsrp.Text = "0.00";
                else
                    txtMsrp.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Ds.Tables[0].Rows[0]["Price"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["SalePrice"].ToString().Trim()))
                    txtListingPrice.Text = "0.00";
                else
                    txtListingPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Ds.Tables[0].Rows[0]["SalePrice"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(Ds.Tables[0].Rows[0]["OurPrice"].ToString().Trim()))
                    txtOurPrice.Text = "0.00";
                else
                    txtOurPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(Ds.Tables[0].Rows[0]["OurPrice"].ToString().Trim()), 2));

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
                chkIsDiscontinue.Checked = Convert.ToBoolean(Ds.Tables[0].Rows[0]["Discontinue"].ToString());
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


                try
                {
                    if (Ds.Tables[0].Rows[0]["ProductTypeDeliveryID"] != null && Ds.Tables[0].Rows[0]["ProductTypeDeliveryID"].ToString() != "")
                    {
                        //ddlProductTypeDelivery.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"]);
                        string name = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_ProductTypeDelivery where ProductTypeDeliveryID=" + Ds.Tables[0].Rows[0]["ProductTypeDeliveryID"] + ""));
                        ddlProductTypeDelivery.ClearSelection();
                        ddlProductTypeDelivery.Items.FindByText(name).Selected = true;
                        ddlProductTypeDelivery_SelectedIndexChanged(null, null);
                    }
                    if (Ds.Tables[0].Rows[0]["ProductTypeID"] != null && Ds.Tables[0].Rows[0]["ProductTypeID"].ToString() != "")
                    {
                        //  ddlProductType.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductTypeID"]);

                        string name = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_ProductType where ProductTypeID=" + Ds.Tables[0].Rows[0]["ProductTypeID"] + ""));
                        ddlProductType.ClearSelection();
                        ddlProductType.Items.FindByText(name).Selected = true;
                        ddlProductType_SelectedIndexChanged(null, null);
                    }
                    if (Ds.Tables[0].Rows[0]["VendorID"] != null && Ds.Tables[0].Rows[0]["VendorID"].ToString() != "")
                    {
                        ddlvendor.SelectedValue = Convert.ToString(Ds.Tables[0].Rows[0]["VendorID"]);

                        //string name = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_Vendor where VendorID=" + DsProduct.Tables[0].Rows[0]["VendorID"] + ""));
                        //ddlvendor.ClearSelection();
                        //ddlvendor.Items.FindByText(name).Selected = true;
                    }
                }
                catch { }

                //Image region
                #region Get Image
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                    Directory.CreateDirectory(Server.MapPath(ProductMediumPath));
                string Imagename = Ds.Tables[0].Rows[0]["Imagename"].ToString();
                string strFilePath = Server.MapPath(ProductMediumPath + Imagename);

                btnDelete.Visible = false;
                if (Request.QueryString["CloneID"] == null)
                {
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
                }
                else
                {
                    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                    Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                    string strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/") + Imagename);
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct"))))
                    {
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct")));
                    }
                    if (File.Exists(strFilePathOld.Replace("/Medium/", "/Large/")))
                    {
                        FileInfo flOld = new FileInfo(strFilePathOld.ToString().Replace("/Medium/", "/Large/"));

                        FileInfo fl = new FileInfo(strFilePath.ToString().Replace("/Medium/", "/Large/"));
                        ViewState["File"] = fl.Name.ToString();
                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreId"].ToString());
                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                        if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "/Temp")))
                        {
                            Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "/Temp"));
                        }
                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString()), true);
                        ImgLarge.Src = AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString();
                        lblMsg.Text = "";
                    }
                    else
                    {
                        ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                        ViewState["File"] = null;
                    }
                }
                #endregion

                //category region
                if (Request.QueryString["CloneID"] == null)
                {
                    txtMainCategory.Text = Ds.Tables[0].Rows[0]["MainCategory"].ToString().Trim();
                    ddlCategory.SelectedValue = Ds.Tables[0].Rows[0]["BuyCategoryID"].ToString().Trim();
                }
                else
                {
                    txtMainCategory.Text = "";
                    ddlCategory.SelectedIndex = 0;
                }
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

                Int32 ID = 0;
                if (Request.QueryString["StoreID"] != null && Request.QueryString["ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ID"].ToString()) && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
                {
                    ID = Convert.ToInt32(Request.QueryString["ID"]);
                }

                #region GetVendorDropship

                VendorComponent objVendor = new VendorComponent();

                DataSet dssku = new DataSet();
                dssku = objVendor.GetDropShipperListByProductID(ID);
                if (dssku != null && dssku.Tables.Count > 0 && dssku.Tables[0].Rows.Count > 0)
                {
                    hdnvendorAllSku.Value = dssku.Tables[0].Rows[0]["VendorSKU"].ToString();
                    if (hdnvendorAllSku.Value != null && hdnvendorAllSku.Value != "")
                    { GetVendorSKU(ID); }
                }
                #endregion

                #region GetVendorDropship



                DataSet dsProductSKU = new DataSet();
                dsProductSKU = objVendor.GetAllAssemblerProductSKUByProductID(ID);
                if (dsProductSKU != null && dsProductSKU.Tables.Count > 0 && dsProductSKU.Tables[0].Rows.Count > 0)
                {
                    hdnProductALLSku.Value = dsProductSKU.Tables[0].Rows[0]["SKU"].ToString();
                    if (hdnProductALLSku.Value != null && hdnProductALLSku.Value != "")
                    { GetProductSKU(ID, StoreID); }
                }
                #endregion


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
        /// Sets the value of Product into Controls
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns tb_Product Table Object</returns>
        private tb_Product SetValue(tb_Product product)
        {
            if (Request.QueryString["StoreID"] != null)
                product.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(Request.QueryString["StoreID"].ToString()));
            //Set Manufacture ID foreign key value
            product.tb_ManufactureReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Manufacture", "ManufactureID", Convert.ToInt32(ddlManufacture.SelectedValue));
            //tb_Product.tb_ManufactureReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Manufacture", "ManufactureID", Convert.ToInt32(ddlManufacture.SelectedValue));
            product.SellerID = Convert.ToInt32(txtSellerID.Text.Trim());
            // product.UPC = txtGtin.Text.Trim().ToString();

            DataSet dsUPC = null;

            bool UPCchrck = false;

            if (Request.QueryString["StoreID"] != null && Request.QueryString["ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ID"].ToString()) && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
            {
                UPCchrck = true;
            }

            if (UPCchrck)
            {
                if (lbtngetUPC.Visible == false)
                    dsUPC = CommonComponent.GetCommonDataSet("select SKU,UPC from tb_upcmasterreal where sku is not null ");
                else
                    dsUPC = CommonComponent.GetCommonDataSet("select SKU,UPC from tb_upcmaster where sku is not null ");

            }
            else
            {
                dsUPC = CommonComponent.GetCommonDataSet("select SKU,UPC from tb_upcmaster where sku is not null ");
            }
            if (dsUPC != null && dsUPC.Tables.Count > 0 && dsUPC.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsUPC.Tables[0].Rows.Count; i++)
                {
                    if (txtSKU.Text == dsUPC.Tables[0].Rows[i]["SKU"].ToString())
                    {
                        //chkUPC = true;
                        // txtUPC.Text = dsUPC.Tables[0].Rows[i]["UPC"].ToString();
                        tb_Product.UPC = dsUPC.Tables[0].Rows[i]["UPC"].ToString();
                        break;
                    }
                    else
                    {
                        tb_Product.UPC = txtGtin.Text.Trim();
                    }
                }
            }
            else
            {
                tb_Product.UPC = txtGtin.Text.Trim();
            }


            product.Isbn = txtISBN.Text.Trim().ToString();
            product.ManufacturePartNo = txtManufactrPartNo.Text.Trim().ToString();
            product.ASIN = txtAsin.Text.Trim().ToString();
            product.SKU = txtSKU.Text.Trim().ToString();
            product.Srno = Convert.ToInt32(txtSerialNo.Text.Trim().ToString());
            product.Name = txtTitle.Text.Trim().ToString();
            decimal Weight = decimal.Zero;
            decimal.TryParse(txtWeight.Text.Trim(), out Weight);
            product.Weight = Convert.ToDecimal(Weight);

            decimal Width = decimal.Zero;
            decimal.TryParse(txtwidth.Text.Trim(), out Width);
            product.Width = Convert.ToString(Width);

            decimal Height = decimal.Zero;
            decimal.TryParse(txtheight.Text.Trim(), out Height);
            product.Height = Convert.ToString(Height);

            decimal Length = decimal.Zero;
            decimal.TryParse(txtlength.Text.Trim(), out Length);
            product.Length = Convert.ToString(Length);

            decimal Price = decimal.Zero;
            decimal.TryParse(txtMsrp.Text.Trim(), out Price);
            tb_Product.Price = Convert.ToDecimal(Price);

            decimal SalePrice = decimal.Zero;
            decimal.TryParse(txtListingPrice.Text.Trim(), out SalePrice);
            product.SalePrice = Convert.ToDecimal(SalePrice);

            decimal OurPrice = decimal.Zero;
            decimal.TryParse(txtOurPrice.Text.Trim(), out OurPrice);
            product.OurPrice = Convert.ToDecimal(OurPrice);

            //product.buyKeywords = txtBuyKeywords.Text.Trim();

            product.ProductSetID = (txtProductSetID.Text.Trim());
            product.Avail = txtAvailability.Text.Trim().ToString();
            if (txtInventory.Text.Trim().Length == 0)
                txtInventory.Text = "0";
            product.Inventory = Convert.ToInt32(txtInventory.Text.Trim().ToString());
            product.Active = chkPublished.Checked;
            product.Discontinue = chkIsDiscontinue.Checked;
            if (RBtnContiYes.Checked == true)
            {
                product.Discontinue = true;
            }
            else
            {
                product.Discontinue = false;
            }
            if (string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()) && txtDisplayOrder.Text.Trim() == "")
                product.DisplayOrder = 0;
            else
                product.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim().ToString());

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
        /// Sets the Product of Buy Store Value
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        /// <returns>Returns tb_ProductBuy Table Object</returns>
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
        /// Inserts the Product Data into Product Table and Buy Product Table
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        private void InsertProduct(int StoreID)
        {
            tb_Product = new tb_Product();
            tb_Product = SetValue(tb_Product);

            Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect ISNULL(count(sku),0) as TotCnt From tb_product Where sku='" + txtSKU.Text.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1"));
            if (ChkSKu > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU with same name already exists specify another sku', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                txtSKU.Focus();
                return;
            }
            else
            {

                ProductComponent objProduct = new ProductComponent();
                Int32 ProductID = Convert.ToInt32(ProductComponent.InsertProduct(tb_Product));
                tb_ProductBuy objProductBuy = new tb_ProductBuy();
                if (ProductID > 0)
                {
                    if (txtGtin.Text.ToString().Trim() != "")
                    {
                        try
                        {
                            GenerateBarcode(txtGtin.Text.ToString().Trim());
                        }
                        catch { }
                    }
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

                    if (chkIsDiscontinue.Checked)
                        CommonComponent.ExecuteCommonData("Update tb_Product set DiscontinuedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");
                    CommonComponent.ExecuteCommonData("Update tb_Product set InventoryUpdatedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");

                    CommonComponent.ExecuteCommonData("Update tb_UPCMasterReal set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtGtin.Text.Trim() + "'");
                    CommonComponent.ExecuteCommonData("Update tb_UPCMaster set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtGtin.Text.Trim() + "'");

                    int VendorID = 0;
                    if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() != "vendor")
                        VendorID = 0;
                    else VendorID = Convert.ToInt32(ddlvendor.SelectedItem.Value);

                    CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET ProductTypeDeliveryID=" + Convert.ToInt32(ddlProductTypeDelivery.SelectedItem.Value) + ",ProductTypeID=" + Convert.ToInt32(ddlProductType.SelectedItem.Value) + ",VendorID=" + VendorID + " WHERE ProductID=" + ProductID + "");

                    FillData(ProductID);
                    FillProductData(ProductID);

                    #region New Inventory Logic

                    if (ddlProductType.SelectedItem.Text.ToLower() == "assembly product")
                    {
                        DataSet dsProductQunt = CommonComponent.GetCommonDataSet("select top 1 ProductID,Quantity from tb_ProductAssembly where RefProductID=" + ProductID + " order by Quantity desc");

                        if (dsProductQunt != null && dsProductQunt.Tables.Count > 0 && dsProductQunt.Tables[0].Rows.Count > 0)
                        {
                            int tpproductID = Convert.ToInt32(dsProductQunt.Tables[0].Rows[0]["ProductID"].ToString());
                            int Quantity = Convert.ToInt32(dsProductQunt.Tables[0].Rows[0]["Quantity"].ToString());

                            int inv = Convert.ToInt32(CommonComponent.GetScalarCommonData("select inventory from tb_Product where ProductID=" + tpproductID + ""));
                            int updatedQuantity = (Convert.ToInt32(inv / Quantity));
                            //updatedQuantity = Math.Floor(updatedQuantity);
                            CommonComponent.ExecuteCommonData("delete from tb_WareHouseProductInventory where productid=" + ProductID + "");

                            int warehouseid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 warehouseid from tb_WareHouse"));

                            CommonComponent.ExecuteCommonData("INSERT INTO dbo.tb_WareHouseProductInventory( WareHouseID, ProductID, Inventory )VALUES (" + warehouseid + "," + ProductID + "," + updatedQuantity + ")");

                            CommonComponent.ExecuteCommonData("update tb_product set inventory=" + updatedQuantity + " where productid=" + ProductID + "");

                        }
                    }
                    #endregion

                    #region new upc logic

                    int cntSKU = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(SKU), sku from tb_Product where SKU='" + txtSKU.Text + "' group by SKU having COUNT(SKU) > 1"));

                    if (cntSKU != null && cntSKU > 1)
                    {

                        string existinUPC = Convert.ToString(CommonComponent.GetScalarCommonData("select UPC from tb_Product where storeid ! =" + Convert.ToInt32(Request.QueryString["StoreID"]) + " and sku='" + txtSKU.Text + "'"));
                        if (existinUPC != null && existinUPC.Length > 0)
                            CommonComponent.ExecuteCommonData("Update tb_product set UPC='" + existinUPC + "' where SKU='" + txtSKU.Text + "'");
                    }
                    #endregion


                    int cntUPC = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(upc) from tb_Product where SKU='" + txtSKU.Text + "' group by UPC having COUNT(upc) > 1 "));
                    if (cntUPC != 0 && cntUPC > 1)
                        chkUPC = true;
                    else
                        chkUPC = false;

                    if (chkUPC == false)
                    {
                        Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    }
                    else
                    {
                        Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                       // System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Another UPC with same SKU already exists in another Store, UPC for this SKU will be updated with that UPC!');window.location ='ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : "") + "';", true);
                    }



                    //Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                }
            }
        }

        /// <summary>
        /// Update the Product Data into Product Table and Buy Product Table
        /// </summary>
        /// <param name="ProdID">int ProdID</param>
        /// <param name="StoreID">int StoreID</param>
        private void UpdateProduct(int ProdID, int StoreID)
        {
            ProductComponent objProduct = new ProductComponent();
            tb_Product = new tb_Product();

            tb_Product = objProduct.GetAllProductDetailsbyProductID(ProdID);

            tb_Product = SetValue(tb_Product);
            Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ISNULL(count(sku),0) as TotCnt From tb_product Where sku='" + txtSKU.Text.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " and Productid <>" + ProdID + " AND ISNULL(Deleted,0)=0  AND ISNULL(Active,0)=1"));
            if (ChkSKu > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU with same name already exists specify another sku', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                txtSKU.Focus();
                return;
            }
            else
            {
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
                    if (txtGtin.Text.ToString().Trim() != "")
                    {
                        try
                        {
                            GenerateBarcode(txtGtin.Text.ToString().Trim());
                        }
                        catch { }
                    }
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

                    bool chkdisconti = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(Discontinue,0) from tb_Product where ProductID=" + ProdID + ""));
                    if ((chkdisconti == false && chkIsDiscontinue.Checked == true))
                        CommonComponent.ExecuteCommonData("Update tb_Product set DiscontinuedOn='" + DateTime.Now + "' where ProductID=" + ProdID + "");

                    Int32 inventoryUpdated = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select isnull(Inventory,0) from tb_Product where ProductID=" + ProdID + ""));

                    int count = ProductComponent.UpdateProduct(tb_Product);

                    CommonComponent.ExecuteCommonData("Update tb_UPCMaster set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtGtin.Text.Trim() + "'");
                    CommonComponent.ExecuteCommonData("Update tb_UPCMasterReal set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtGtin.Text.Trim() + "'");

                    //Update vendor and ProductType and Stock/Dropship
                    int VendorID = 0;
                    if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() != "vendor")
                        VendorID = 0;
                    else VendorID = Convert.ToInt32(ddlvendor.SelectedItem.Value);

                    CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET ProductTypeDeliveryID=" + Convert.ToInt32(ddlProductTypeDelivery.SelectedItem.Value) + ",ProductTypeID=" + Convert.ToInt32(ddlProductType.SelectedItem.Value) + ",VendorID=" + VendorID + " WHERE ProductID=" + ProdID + "");

                    #region dropship data and product data

                    if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                    {
                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "dropship" && dtAssembler != null && dtAssembler.Rows.Count > 0)
                        {
                            DeleteVendorData(ProdID);
                            FillData(ProdID);
                        }

                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "vendor" && dt != null && dt.Rows.Count > 0)
                        {
                            DeleteDropShipData(ProdID);
                            FillProductData(ProdID);
                        }
                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "dropship" && (dtAssembler == null || dtAssembler.ToString() == ""))
                        {

                            FillData(ProdID);
                        }
                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "vendor" && (dt == null || dt.ToString() == ""))
                        {

                            FillProductData(ProdID);
                        }
                        // FillData(ProductID);
                        // FillProductData(ProductID);
                    }
                    #endregion

                    #region New Inventory Logic

                    if (ddlProductType.SelectedItem.Text.ToLower() == "assembly product")
                    {
                        DataSet dsProductQunt = CommonComponent.GetCommonDataSet("select top 1 ProductID,Quantity from tb_ProductAssembly where RefProductID=" + ProdID + " order by Quantity desc");

                        if (dsProductQunt != null && dsProductQunt.Tables.Count > 0 && dsProductQunt.Tables[0].Rows.Count > 0)
                        {
                            int tpproductID = Convert.ToInt32(dsProductQunt.Tables[0].Rows[0]["ProductID"].ToString());
                            int Quantity = Convert.ToInt32(dsProductQunt.Tables[0].Rows[0]["Quantity"].ToString());

                            int inv = Convert.ToInt32(CommonComponent.GetScalarCommonData("select inventory from tb_Product where ProductID=" + tpproductID + ""));
                            int updatedQuantity = (Convert.ToInt32(inv / Quantity));
                            //updatedQuantity = Math.Floor(updatedQuantity);
                            CommonComponent.ExecuteCommonData("delete from tb_WareHouseProductInventory where productid=" + ProdID + "");

                            int warehouseid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 warehouseid from tb_WareHouse"));

                            CommonComponent.ExecuteCommonData("INSERT INTO dbo.tb_WareHouseProductInventory( WareHouseID, ProductID, Inventory )VALUES (" + warehouseid + "," + ProdID + "," + updatedQuantity + ")");
                            CommonComponent.ExecuteCommonData("update tb_product set inventory=" + updatedQuantity + " where productid=" + ProdID + "");

                            if (inventoryUpdated != Convert.ToInt32(updatedQuantity))
                                CommonComponent.ExecuteCommonData("Update tb_Product set InventoryUpdatedOn='" + DateTime.Now + "' where ProductID=" + ProdID + "");
                        }
                    }
                    else
                    {
                        if (inventoryUpdated != Convert.ToInt32(txtInventory.Text))
                            CommonComponent.ExecuteCommonData("Update tb_Product set InventoryUpdatedOn='" + DateTime.Now + "' where ProductID=" + ProdID + "");
                    }

                    #endregion

                    int cntSKU = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(SKU), sku from tb_Product where SKU='" + txtSKU.Text + "' group by SKU having COUNT(SKU) > 1"));

                    if (cntSKU != null && cntSKU > 1)
                    {
                        CommonComponent.ExecuteCommonData("Update tb_product set UPC='" + txtGtin.Text + "' where SKU='" + txtSKU.Text + "'");
                    }

                    int cntUPC = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(upc) from tb_Product where SKU='" + txtSKU.Text + "' group by UPC having COUNT(upc) > 1 "));
                    if (cntUPC > 1)
                        chkUPC = true;
                    else
                        chkUPC = false;

                    if (chkUPC == false)
                    {
                        Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    }
                    else
                    {
                        Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Another UPC with same SKU already exists in another Store, UPC for this SKU will be updated with that UPC!');window.location ='ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : "") + "';", true);
                    }


                    //if (count > 0)
                    //{
                    //    Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    //}
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
            if (Request.QueryString["CloneID"] == null && (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString())))
            {
                AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
            }
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
                UPCStoreId = NewCurrentStoreID;
            }
            RandomUPCNumber(UPCStoreId);
        }


        /// <summary>
        /// Product Type Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (ddlProductType.SelectedItem.Text.ToLower() == "assembly product" && ddlProductTypeDelivery.SelectedItem.Text.ToLower() != "select product type delivery")
            {
                grdAssembler.Visible = true;
            }
            else
            {
                grdAssembler.Visible = false;
            }

            if (ddlProductType.SelectedIndex == 0)
            {
                grdAssembler.Visible = false;
                grdDropShip.Visible = false;
            }
            if (ddlProductType.SelectedIndex == 1 && ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "dropship")
            {
                grdDropShip.Visible = true;
            }

            if (ddlProductType.SelectedIndex == 1 && ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "vendor")
            {
                grdAssembler.Visible = false;
            }
        }

        /// <summary>
        /// Product Type Delivery Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlProductTypeDelivery_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlProductTypeDelivery.SelectedIndex != 0)
            {

                ddlProductType.Items.Clear();
                ddlvendor.Items.Clear();
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
                    ddlProductType.Items.Insert(0, new ListItem("Select Product Type", "0"));
                }
                else
                {
                    ddlProductType.DataSource = null;
                    ddlProductType.DataBind();
                    ddlProductType.SelectedIndex = 0;
                }

                if (ddlProductTypeDelivery.SelectedIndex == 1)
                {
                    VendorDAC objVendorDAC = new VendorDAC();
                    DataSet dsdropshipsku = objVendorDAC.GetVendorList(1);
                    if (dsdropshipsku != null && dsdropshipsku.Tables.Count > 0 && dsdropshipsku.Tables[0].Rows.Count > 0)
                    {
                        if (txtInventory.Text == null || txtInventory.Text == "")
                        {
                            txtInventory.Text = "0";
                        }

                        ddlvendor.DataSource = dsdropshipsku.Tables[0];
                        ddlvendor.DataTextField = "Name";
                        ddlvendor.DataValueField = "VendorID";
                        ddlvendor.DataBind();
                    }
                    else
                    {
                        ddlvendor.DataSource = null;
                        ddlvendor.DataBind();
                    }
                    ddlvendor.Items.Insert(0, new ListItem("Select Drop Shipper", "0"));



                }
                if (ddlProductTypeDelivery.SelectedIndex == 2)
                {

                    VendorDAC objVendorDAC = new VendorDAC();
                    DataSet dsVendor = objVendorDAC.GetVendorList(0);
                    if (dsVendor != null && dsVendor.Tables.Count > 0 && dsVendor.Tables[0].Rows.Count > 0)
                    {
                        ddlvendor.DataSource = dsVendor.Tables[0];
                        ddlvendor.DataTextField = "Name";
                        ddlvendor.DataValueField = "VendorID";
                        ddlvendor.DataBind();
                    }
                    else
                    {
                        ddlvendor.DataSource = null;
                        ddlvendor.DataBind();
                    }
                    ddlvendor.Items.Insert(0, new ListItem("Select Vendor", "0"));
                }
            }

            if (ddlProductTypeDelivery.SelectedIndex == 0)
            {
                ddlProductType.SelectedIndex = 0;
                if (txtInventory.Text == "0")
                {
                    txtInventory.Text = "";
                }
            }

            if (ddlProductTypeDelivery.SelectedIndex == 1)
            {
                ddlProductType.SelectedIndex = 1;
                grdDropShip.Visible = true;
            }
            else
            {
                grdDropShip.Visible = false;
            }
            if (ddlProductTypeDelivery.SelectedIndex == 2)
            {
                ddlProductType.SelectedIndex = 1;
                if (txtInventory.Text == "0")
                {
                    txtInventory.Text = "";
                }
                //grdAssembler.Visible = true;
            }
            else
            {
                grdAssembler.Visible = false;
            }

            if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() != "vendor" && ddlProductTypeDelivery.SelectedItem.Text.ToLower() != "dropship")
            {
                divvendor.Visible = false;
                ddlvendor.Visible = false;
                ddlvendor.DataSource = null;
                ddlvendor.DataBind();
            }
            else
            {
                divvendor.Visible = true;
                ddlvendor.Visible = true;
            }

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
        ///  Clone New Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCloneNewProduct_Click(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString["oldid"] != null && !string.IsNullOrEmpty(Request.QueryString["oldid"].ToString()) && Request.QueryString["storeid"] != null && !string.IsNullOrEmpty(Request.QueryString["storeid"].ToString()))
                {
                    string url = string.Empty;
                    url = "ProductBuy.aspx?StoreID=" + CurrentStoreID.ToString() + "&ID=" + Request.QueryString["oldid"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString();
                    Response.Redirect(url);
                }
            }
            catch { }

        }

        /// <summary>
        /// Determines whether  product is available for the specified ProductID.
        /// </summary>
        /// <param name="pid">string pid</param>
        /// <param name="currStore">string currStore</param>
        private string IsProductAvailable(string pid, string currStore)
        {
            try
            {
                SQLAccess dbAccess = new SQLAccess();
                return Convert.ToString(dbAccess.ExecuteScalarQuery("select p1.ProductID from  tb_Product p1,tb_Product p2 where p1.sku=p2.sku and isnull(p1.Deleted,0) !=1 ANd isnull(p1.Active,0)=1 and p2.productid=" + pid + " AND p1.Storeid=" + currStore.ToString() + ""));
            }
            catch { return ""; }
        }

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

        protected void lbtngetUPC_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["StoreID"] != null)
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                {
                    string checkrealupc = Convert.ToString(CommonComponent.GetScalarCommonData("Select SKU from tb_UPCMasterReal where UPC='" + txtGtin.Text.Trim() + "' and SKU ='" + txtSKU.Text.Trim() + "'"));
                    if (checkrealupc != null && checkrealupc.Length > 0)
                    {
                        lbtngetUPC.Visible = false;
                    }
                    else
                    {
                        StrUPCReal = Convert.ToString(CommonComponent.GetScalarCommonData("select TOP 1 UPC from tb_UPCMasterReal where sku is null ORDER BY NEWID() "));
                        if (StrUPCReal != null && StrUPCReal.Length > 0)
                        {
                            txtGtin.Text = StrUPCReal;
                            lbtngetUPC.Visible = false;
                            StrUPC = null;
                        }
                    }
                }
                else
                {
                    StrUPCReal = Convert.ToString(CommonComponent.GetScalarCommonData("select TOP 1 UPC from tb_UPCMasterReal where SKU is null ORDER BY NEWID() "));
                    if (StrUPCReal != null && StrUPCReal.Length > 0)
                    {
                        txtGtin.Text = StrUPCReal;
                        lbtngetUPC.Visible = false;
                        StrUPC = null;
                    }
                }
            }
        }

        //---New code--//

        #region Dropship

        private void BindVendorSKUfromtable()
        {
            grdDropShip.DataSource = null;
            grdDropShip.DataBind();

            if (dt != null & dt.Rows.Count > 0)
            {
                grdDropShip.DataSource = dt;
                grdDropShip.DataBind();

            }
            else
            {
                grdDropShip.DataSource = null;
                grdDropShip.DataBind();
                dt = null;
            }
        }
        private void BindVendorSKU()
        {
            grdDropShip.DataSource = null;
            grdDropShip.DataBind();
            DataSet dsvendors = CommonComponent.GetCommonDataSet("SELECT tb_VendorSKU.VendorSKUID,tb_VendorSKU.VendorID,tb_VendorSKU.VendorSKU,tb_VendorSKU.ProductName,'1' as Priority, Name FROM dbo.tb_VendorSKU inner join tb_Vendor on " +
                                                                 "tb_VendorSKU.VendorID = tb_Vendor.VendorID  WHERE VendorSKU IN('" + hdnvendorAllSku.Value.Replace(",", "','") + "')");
            if (dsvendors != null & dsvendors.Tables.Count > 0 && dsvendors.Tables[0].Rows.Count > 0)
            {
                if (dt == null && dt.Rows.Count == 0)
                {
                    dt = dsvendors.Tables[0];
                    grdDropShip.DataSource = dt;
                    grdDropShip.DataBind();
                }
                else
                {
                    for (int i = 0; i <= dsvendors.Tables[0].Rows.Count - 1; i++)
                    {
                        DataRow drPackage = null;
                        DataRow[] drr = dt.Select("VendorSKUID=" + dsvendors.Tables[0].Rows[i]["VendorSKUID"] + "");
                        if (drr.Length > 0)
                        {
                        }
                        else
                        {
                            drPackage = dt.NewRow();
                            drPackage["VendorID"] = dsvendors.Tables[0].Rows[i]["VendorID"];
                            drPackage["VendorSKUID"] = dsvendors.Tables[0].Rows[i]["VendorSKUID"];
                            drPackage["ProductName"] = dsvendors.Tables[0].Rows[i]["ProductName"];
                            drPackage["VendorSKU"] = dsvendors.Tables[0].Rows[i]["VendorSKU"];
                            drPackage["Priority"] = dsvendors.Tables[0].Rows[i]["Priority"];
                            drPackage["Name"] = dsvendors.Tables[0].Rows[i]["Name"];
                            dt.Rows.Add(drPackage);
                        }
                    }
                    grdDropShip.DataSource = dt;
                    grdDropShip.DataBind();
                }
            }
            else
            {
                grdDropShip.DataSource = null;
                grdDropShip.DataBind();
            }
        }
        protected void btnvendorlist_click(object sender, EventArgs e)
        {
            if (hdnvendorAllSku.Value != null && hdnvendorAllSku.Value != "")
            { BindVendorSKU(); }
            //else
            if (hdnProductALLSku.Value != null && hdnProductALLSku.Value != "")
            { BindProductSKU(); }
        }
        protected void grdDropShip_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                //((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {

                ((ImageButton)e.Row.FindControl("ibtnFeaturecategory")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/add-more.png";
            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {

                ((ImageButton)e.Row.FindControl("ibtnFeaturecategory")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/add-more.png";
            }
        }
        protected void grdDropShip_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {

            }

            if (e.CommandName.ToLower() == "save")
            {
                GridViewRow gvrow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;

                int id = Convert.ToInt32(e.CommandArgument.ToString());

                DataRow[] drr = dt.Select("VendorSKUID= " + id + "  ");
                for (int i = 0; i < drr.Length; i++)
                {
                    TextBox txtPriority = (TextBox)gvrow.FindControl("txtPriority");
                    drr[i]["Priority"] = txtPriority.Text;

                }

                dt.AcceptChanges();
                grdDropShip.EditIndex = -1;
                BindVendorSKUfromtable();
            }
            else if (e.CommandName == "del")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                DataRow[] drr = dt.Select("VendorSKUID=" + id + "");
                for (int i = 0; i < drr.Length; i++)
                {
                    Vendorsku = Vendorsku + drr[i]["VendorSKUID"] + ",";
                    drr[i].Delete();
                }
                dt.AcceptChanges();
                hdnvendorAllSku.Value = "";
                for (int k = 0; k <= dt.Rows.Count - 1; k++)
                {
                    hdnvendorAllSku.Value = hdnvendorAllSku.Value + dt.Rows[k]["VendorSKU"].ToString() + ",";
                }

                if (hdnvendorAllSku.Value.Length > 0)
                {
                    hdnvendorAllSku.Value = hdnvendorAllSku.Value.Substring(0, hdnvendorAllSku.Value.Length - 1);
                }

                BindVendorSKU();

            }
        }
        protected void editRecord(object sender, GridViewEditEventArgs e)
        {
            grdDropShip.EditIndex = e.NewEditIndex;
            BindVendorSKU();
        }

        protected void cancelRecord(object sender, GridViewCancelEditEventArgs e)
        {
            grdDropShip.EditIndex = -1;
            BindVendorSKU();
        }
        private void TempTable()
        {
            dt = new DataTable();

            dcVendorID = new System.Data.DataColumn("VendorID", typeof(int));
            dt.Columns.Add(dcVendorID);
            dcVendorSKUID = new System.Data.DataColumn("VendorSKUID", typeof(int));
            dt.Columns.Add(dcVendorSKUID);
            dcDopshipper = new System.Data.DataColumn("ProductName", typeof(String));
            dt.Columns.Add(dcDopshipper);
            dcSKU = new System.Data.DataColumn("VendorSKU", typeof(String));
            dt.Columns.Add(dcSKU);
            dcPriority = new System.Data.DataColumn("Priority", typeof(String));
            dt.Columns.Add(dcPriority);
            dcDopshipperName = new System.Data.DataColumn("Name", typeof(String));
            dt.Columns.Add(dcDopshipperName);
        }


        public void FillData(Int32 ProductID)
        {

            if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null)
            {
                if (Vendorsku != "")
                {
                    Vendorsku = Vendorsku.Substring(0, Vendorsku.Length - 1);
                    CommonComponent.ExecuteCommonData("DELETE from dbo.tb_ProductVendorSKU  WHERE ProductID=" + Convert.ToInt32(Request.QueryString["ID"]) + " AND VendorSKUID IN(" + Vendorsku + ")");
                    Vendorsku = "";
                }
            }


            foreach (GridViewRow row in grdDropShip.Rows)
            {
                Label lblVendorID = (Label)row.FindControl("lblVendorID");
                Label lblVendorSKU = (Label)row.FindControl("lblVendorSKU");
                Label lblPriority = (Label)row.FindControl("lblPriority");
                Label lblVendorSKUID = (Label)row.FindControl("lblVendorSKUID");

                if (dt != null && dt.Rows.Count > 0)
                {
                    VendorComponent objVendor = new VendorComponent();
                    objVendor.InsertDropshipProduct(ProductID, Convert.ToInt32(lblVendorID.Text), Convert.ToInt32(lblVendorSKUID.Text), lblVendorSKU.Text, lblPriority.Text);
                }

            }
        }
        public void GetVendorSKU(Int32 ProductID)
        {
            //DataSet dsgetvendors = CommonComponent.GetCommonDataSet("SELECT VendorSKUID,VendorID,VendorSKU,ProductName, Priority FROM dbo.tb_VendorSKU WHERE VendorSKU IN('" + hdnvendorAllSku.Value.Replace(",", "','") + "')");

            DataSet dsgetvendors = CommonComponent.GetCommonDataSet("SELECT tb_VendorSKU.VendorSKUID,tb_VendorSKU.VendorID,tb_VendorSKU.VendorSKU,tb_VendorSKU.ProductName" +
            ", tb_ProductVendorSKU.Priority, Name FROM dbo.tb_VendorSKU INNER JOIN  dbo.tb_ProductVendorSKU ON " +
            "dbo.tb_VendorSKU.VendorSKU = dbo.tb_ProductVendorSKU.VendorSKU inner join tb_Vendor on  tb_VendorSKU.VendorID = tb_Vendor.VendorID WHERE tb_VendorSKU.VendorSKU IN('" + hdnvendorAllSku.Value.Replace(",", "','") + "') and ProductID=" + ProductID + "");
            if (dsgetvendors != null & dsgetvendors.Tables.Count > 0 && dsgetvendors.Tables[0].Rows.Count > 0)
            {
                grdDropShip.DataSource = dsgetvendors;
                grdDropShip.DataBind();
                dt = dsgetvendors.Tables[0];
            }
            else
            {
                grdDropShip.DataSource = null;
                grdDropShip.DataBind();
                dt = null;
            }
        }


        #endregion

        #region Assembler Product
        private void AssemblerTempTable()
        {
            dtAssembler = new DataTable();

            dcProductID = new System.Data.DataColumn("ProductID", typeof(int));
            dtAssembler.Columns.Add(dcProductID);
            dcProductName = new System.Data.DataColumn("Name", typeof(String));
            dtAssembler.Columns.Add(dcProductName);
            dcProductSKU = new System.Data.DataColumn("SKU", typeof(String));
            dtAssembler.Columns.Add(dcProductSKU);
            dcQuantity = new System.Data.DataColumn("Quantity", typeof(String));
            dtAssembler.Columns.Add(dcQuantity);
        }

        private void BindProductSKU()
        {

            grdAssembler.DataSource = null;
            grdAssembler.DataBind();
            // dtAssembler = null;

            DataSet dsproductSKU = CommonComponent.GetCommonDataSet("SELECT ProductID,SKU,Name,'1' as Quantity FROM dbo.tb_Product WHERE Isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND StoreID=" + Convert.ToInt32(Request.QueryString["StoreID"]) + " and SKU IN('" + hdnProductALLSku.Value.Replace(",", "','") + "')");
            if (dsproductSKU != null && dsproductSKU.Tables.Count > 0 && dsproductSKU.Tables[0].Rows.Count > 0)
            {
                //grdAssembler.DataSource = dsproductSKU;
                //grdAssembler.DataBind();

                if (dtAssembler == null && dtAssembler.Rows.Count < 0)
                {
                    dtAssembler = dsproductSKU.Tables[0];
                    grdAssembler.DataSource = dtAssembler;
                    grdAssembler.DataBind();
                }
                else
                {
                    for (int i = 0; i <= dsproductSKU.Tables[0].Rows.Count - 1; i++)
                    {
                        //   dsvendors.Tables[0].Rows[i][""]
                        DataRow drPackage = null;
                        DataRow[] drr = dtAssembler.Select("ProductID=" + dsproductSKU.Tables[0].Rows[i]["ProductID"] + "");
                        if (drr.Length > 0)
                        {
                        }
                        else
                        {
                            drPackage = dtAssembler.NewRow();
                            // drPackage["ShippingCartID"] = strShoppingCartID;
                            drPackage["ProductID"] = dsproductSKU.Tables[0].Rows[i]["ProductID"];
                            drPackage["Name"] = dsproductSKU.Tables[0].Rows[i]["Name"];
                            drPackage["SKU"] = dsproductSKU.Tables[0].Rows[i]["SKU"];

                            drPackage["Quantity"] = dsproductSKU.Tables[0].Rows[i]["Quantity"];
                            //  drPackage["ProductId"] = strProductId;
                            dtAssembler.Rows.Add(drPackage);
                        }

                    }

                    grdAssembler.DataSource = dtAssembler;
                    grdAssembler.DataBind();
                }



            }
            else
            {
                grdAssembler.DataSource = null;
                grdAssembler.DataBind();
            }
        }

        protected void grdAssembler_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                //((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    TextBox txtQuantity = (TextBox)e.Row.FindControl("txtQuantity");
                    if (txtQuantity.Text != null)
                        ((ImageButton)e.Row.FindControl("btnSave")).Attributes.Add("onclick", "javascript:btnCheck('" + ((TextBox)e.Row.FindControl("txtQuantity")).ClientID + "','" + ((ImageButton)e.Row.FindControl("btnSave")).ClientID + "')");

                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {

                ((ImageButton)e.Row.FindControl("ibtnFeatureProduct")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/add-more.png";
            }
            else if (e.Row.RowType == DataControlRowType.EmptyDataRow)
            {

                ((ImageButton)e.Row.FindControl("ibtnFeatureProduct")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/add-more.png";
            }
        }

        protected void editProduct(object sender, GridViewEditEventArgs e)
        {
            grdAssembler.EditIndex = e.NewEditIndex;
            BindProductSKU();
        }

        protected void cancelProduct(object sender, GridViewCancelEditEventArgs e)
        {
            grdAssembler.EditIndex = -1;
            BindProductSKU();
        }

        protected void grdAssembler_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.ToLower() == "save")
            {
                GridViewRow gvrow = (GridViewRow)((ImageButton)e.CommandSource).NamingContainer;
                Int32 Quantity = 0;
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                TextBox txtQuantity = (TextBox)gvrow.FindControl("txtQuantity");
                if (txtQuantity.Text != null)
                {
                    if (Convert.ToInt32(txtQuantity.Text.Trim()) > 0)
                    {
                        //if (Request.QueryString["CloneID"] != null)
                        Quantity = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Inventory from tb_product where ProductId=" + id + " and StoreId=" + Request.QueryString["StoreID"] + ""));

                        DataRow[] drr = dtAssembler.Select("ProductID= " + id + "  ");

                        if (Quantity >= Convert.ToInt32(txtQuantity.Text))
                        {
                            for (int i = 0; i < drr.Length; i++)
                            {
                                //TextBox txtQuantity = (TextBox)gvrow.FindControl("txtQuantity");
                                drr[i]["Quantity"] = txtQuantity.Text;

                            }
                            dtAssembler.AcceptChanges();
                            //grdAssembler.EditIndex = -1;
                            //BindProductSKUfromtable();//
                        }
                        else
                        {
                            System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Not Enough Inventory Available For this Product.');", true);
                            //return;
                        }

                        //dtAssembler.AcceptChanges();
                        //grdAssembler.EditIndex = -1;
                        // BindProductSKUfromtable();//
                    }
                    else
                    {
                       // System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Enter Valid Quantity!');", true);
                    }
                    grdAssembler.EditIndex = -1;
                    BindProductSKUfromtable();//
                }
            }
            else if (e.CommandName == "del")
            {
                int id = Convert.ToInt32(e.CommandArgument.ToString());
                DataRow[] drr = dtAssembler.Select("ProductID=" + id + "");
                for (int i = 0; i < drr.Length; i++)
                {
                    Productsku = Productsku + drr[i]["ProductID"] + ",";
                    drr[i].Delete();
                }
                dtAssembler.AcceptChanges();
                hdnProductALLSku.Value = "";
                for (int k = 0; k <= dtAssembler.Rows.Count - 1; k++)
                {
                    hdnProductALLSku.Value = hdnProductALLSku.Value + dtAssembler.Rows[k]["SKU"].ToString() + ",";
                }

                if (hdnProductALLSku.Value.Length > 0)
                {
                    hdnProductALLSku.Value = hdnProductALLSku.Value.Substring(0, hdnProductALLSku.Value.Length - 1);
                }

                BindProductSKU();

            }
        }

        private void BindProductSKUfromtable()
        {
            grdAssembler.DataSource = null;
            grdAssembler.DataBind();

            if (dtAssembler != null & dtAssembler.Rows.Count > 0)
            {
                grdAssembler.DataSource = dtAssembler;
                grdAssembler.DataBind();

            }
            else
            {
                grdAssembler.DataSource = null;
                grdAssembler.DataBind();
                dtAssembler = null;
            }
        }

        public void FillProductData(Int32 ProductID)
        {

            if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null)
            {
                if (Productsku != "")
                {
                    Productsku = Productsku.Substring(0, Productsku.Length - 1);
                    CommonComponent.ExecuteCommonData("DELETE from dbo.tb_ProductAssembly WHERE RefProductID=" + Convert.ToInt32(Request.QueryString["ID"]) + " AND ProductID IN(" + Productsku + ")");
                    Productsku = "";
                }
            }
            foreach (GridViewRow row in grdAssembler.Rows)
            {

                Label lblQuantity = (Label)row.FindControl("lblQuantity");
                Label lblProductID = (Label)row.FindControl("lblProductID");

                if (dtAssembler != null && dtAssembler.Rows.Count > 0)
                {
                    VendorComponent objVendor = new VendorComponent();
                    //objVendor.InsertAssemblerProduct(ProductID, Convert.ToInt32(lblProductID.Text), Convert.ToInt32(lblQuantity.Text));
                    objVendor.InsertAssemblerProduct(ProductID, Convert.ToInt32(lblProductID.Text), Convert.ToInt32(lblQuantity.Text), Convert.ToInt32(Session["AdminID"]), Convert.ToInt32(Session["AdminID"]));
                }

            }
        }

        public void GetProductSKU(Int32 ProductID, Int32 StoreID)
        {
            //DataSet dsgetvendors = CommonComponent.GetCommonDataSet("SELECT VendorSKUID,VendorID,VendorSKU,ProductName, Priority FROM dbo.tb_VendorSKU WHERE VendorSKU IN('" + hdnvendorAllSku.Value.Replace(",", "','") + "')");

            //DataSet dsgetsku = CommonComponent.GetCommonDataSet("SELECT tb_ProductAssembly.ProductAssemblyID, tb_ProductAssembly.RefProductID, tb_ProductAssembly.ProductID," +
            //                                                        "tb_ProductAssembly.Quantity  FROM tb_ProductAssembly INNER JOIN dbo.tb_Product ON dbo.tb_ProductAssembly.ProductID = dbo.tb_Product.ProductID" +
            //                                                        "WHERE tb_ProductAssembly.ProductID=@ProductID AND dbo.tb_ProductAssembly.RefProductID=@RefProductID AND dbo.tb_Product.SKU IN('" + hdnvendorAllSku.Value.Replace(",", "','") + "')");


            if (Request.QueryString["CloneID"] != null)
            {
                DataTable dtAssembly = new DataTable();
                dtAssembly.Columns.AddRange(
                new DataColumn[] 
                { 
                    new DataColumn("RefProductID"), 
                    new DataColumn("ProductID"), 
                    new DataColumn("Quantity"), 
                    new DataColumn("SKU"), 
                    new DataColumn("Name")
                });
                dtAssembly.Columns["RefProductID"].DataType = typeof(Int32);
                dtAssembly.Columns["ProductID"].DataType = typeof(Int32);
                dtAssembly.Columns["Quantity"].DataType = typeof(Int32);

                if (hdnProductALLSku != null && hdnProductALLSku.Value != "" && hdnProductALLSku.Value.Length > 0)
                {
                    string[] strSKU = hdnProductALLSku.Value.Split(',');
                    for (int i = 0; i < strSKU.Length; i++)
                    {
                        Int32 Quantity = 0;
                        Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Quantity,0) as Quantity from tb_ProductAssembly where RefProductID="
                            + Request.QueryString["ID"] + " and ProductID in (select ProductID from tb_Product where StoreID="
                            + Request.QueryString["CloneID"] + " and SKU='" + strSKU[i] + "')")), out Quantity);
                        if (Quantity > 0)
                        {
                            DataRow dr = dtAssembly.NewRow();
                            dr["Quantity"] = Quantity;

                            DataSet dsProd = new DataSet();
                            dsProd = CommonComponent.GetCommonDataSet("select '' as RefProductID, ProductID,SKU,isnull(Name,'') as Name from tb_Product where StoreID="
                                + Request.QueryString["StoreID"] + " and SKU='" + strSKU[i] + "' and Inventory >= " + Quantity + "");
                            if (dsProd != null && dsProd.Tables.Count > 0 && dsProd.Tables[0].Rows.Count > 0)
                            {
                                dr["ProductID"] = Convert.ToString(dsProd.Tables[0].Rows[0]["ProductID"]);
                                dr["SKU"] = Convert.ToString(dsProd.Tables[0].Rows[0]["SKU"]);
                                dr["Name"] = Convert.ToString(dsProd.Tables[0].Rows[0]["Name"]);
                                dtAssembly.Rows.Add(dr);
                            }
                        }
                    }
                }

                if (dtAssembly != null && dtAssembly.Rows.Count > 0)
                {
                    grdAssembler.DataSource = dtAssembly.DefaultView;
                    grdAssembler.DataBind();
                    dtAssembler = dtAssembly;
                }
                else
                {
                    grdAssembler.DataSource = null;
                    grdAssembler.DataBind();
                    dtAssembler = null;
                }
            }
            else
            {
                VendorDAC obj = new VendorDAC();
                DataSet dsgetsku = obj.GetProductSKUListByProductID(ProductID, StoreID);

                if (dsgetsku != null & dsgetsku.Tables.Count > 0 && dsgetsku.Tables[0].Rows.Count > 0)
                {
                    grdAssembler.DataSource = dsgetsku;
                    grdAssembler.DataBind();
                    dtAssembler = dsgetsku.Tables[0];
                }
                else
                {
                    grdAssembler.DataSource = null;
                    grdAssembler.DataBind();
                    dtAssembler = null;
                }
            }

        }

        public void DeleteVendorData(Int32 ProductID)
        {
            if (dtAssembler != null && dtAssembler.Rows.Count > 0 && dt != null && dt.Rows.Count > 0)
            {
                CommonComponent.ExecuteCommonData("Delete from tb_ProductAssembly where RefProductID=" + ProductID + "");
            }
        }
        public void DeleteDropShipData(Int32 ProductID)
        {
            if (dt != null && dt.Rows.Count > 0 && dtAssembler != null && dtAssembler.Rows.Count > 0)
            {
                CommonComponent.ExecuteCommonData("Delete from tb_ProductVendorSKU where ProductID=" + ProductID + "");
            }
        }
        #endregion


    }
}