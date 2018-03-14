using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Collections;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Solution.Data;
namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductAmazon : BasePage
    {
        private int StoreID = 0;

        tb_Product tb_Product = null;
        tb_ProductAmazon tb_ProductAmazon = null;
        tb_ProductCategory tbProductCategory = null;

        public static string ProductTempPath = string.Empty;
        public static string ProductIconPath = string.Empty;
        public static string ProductMediumPath = string.Empty;
        public static string ProductLargePath = string.Empty;
        public static string ProductMicroPath = string.Empty;

        static public int productID = 0;
        static int finHeight;
        static int finWidth;
        static Size thumbNailSizeLarge = Size.Empty;
        static Size thumbNailSizeMediam = Size.Empty;
        static Size thumbNailSizeIcon = Size.Empty;
        static Size thumbNailSizeMicro = Size.Empty;
        ConfigurationComponent objConfiguration = new ConfigurationComponent();
        int InventoryTotal = 0;
        DataSet dsWarehouse = new DataSet();
        int pProductID = 0;
        int CurrentStoreID = 0;
        int OldStoreID = 0;
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
            if (!IsPostBack)
            {
                txtInventory.Attributes.Add("readonly", "true");
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
                        Response.Redirect("ProductAmazon.aspx?Storeid=" + Convert.ToInt32(Request.QueryString["StoreID"]) + "&Mode=Edit&ID=" + NewProductID.ToString() + "");
                    }
                }
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["StoreID"] != null)
                {
                    if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                    { }
                    else
                    {
                        string StrUPC = Convert.ToString(CommonComponent.GetScalarCommonData("select TOP 1 UPC from tb_upcmaster where sku is null ORDER BY NEWID() "));
                        if (StrUPC != null && StrUPC.Length > 0)
                        {
                            txtStandardProductID.Text = StrUPC;
                        }
                    }
                }
            }

            if (!IsPostBack)
            {
                //StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                BindAllCombo(CurrentStoreID);
                BindCategory(CurrentStoreID);
                BindManufacture(CurrentStoreID);
                BindProductType(CurrentStoreID);

                //mode 2: to bind warehouse grid with inventory of respective warehouses
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
            }
            if (!IsPostBack)
            {
                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");

                if (Request.QueryString["StoreID"] != null)
                {
                    if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null && Request.QueryString["CloneID"] == null)
                    {
                        divDescription.Visible = true;
                        BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                        tduploadMoreImages.Visible = true;
                        msgid.Visible = false;
                    }
                    if (Request.QueryString["Mode"] == null && Request.QueryString["ID"] != null && Request.QueryString["CloneID"] != null)
                    {
                        divDescription.Visible = true;
                        BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["CloneID"]));
                        tduploadMoreImages.Visible = false;
                        msgid.Visible = true;

                    }
                    //else
                    //{
                    //    ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                    //}

                    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                }

                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";


            }
            else
            {
                if (chkIsHamming.Checked == true)
                {
                    tdHamminglbl.Attributes.Add("style", "display:''");
                    tdHammingQty.Attributes.Add("style", "display:''");
                }
                else
                {
                    tdHamminglbl.Attributes.Add("style", "display:none");
                    tdHammingQty.Attributes.Add("style", "display:none");
                }
                Int32 invtCount = 0;
                foreach (GridViewRow gr in grdWarehouse.Rows)
                {
                    TextBox txtin = (TextBox)gr.FindControl("txtInventory");
                    Label lblTotal = (Label)grdWarehouse.FooterRow.FindControl("lblTotal");

                    if (txtin.Text.ToString().Trim() != "")
                    {
                        invtCount += Convert.ToInt32(txtin.Text.ToString());
                    }
                    lblTotal.Text = invtCount.ToString();
                    txtInventory.Text = invtCount.ToString();
                }
            }
            Page.MaintainScrollPositionOnPostBack = true;
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
        /// Creates the folder at Specified path
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


        private void BindAllCombo(Int32 StoreID)
        {
            BindProductType(StoreID);
            BindProductTypeDelivery(StoreID);
        }

        /// <summary>
        /// Bind Product Data
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="StoreID">int StoreID</param>
        public void BindData(Int32 ID, Int32 StoreID)
        {
            DataSet DsProduct = new DataSet();
            DsProduct = ProductComponent.GetProductByProductID(ID);

            DataSet DsProductAmazon = new DataSet();
            DsProductAmazon = ProductComponent.GetProductByAmazonRefID(ID);
            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
                txtStandardProductID.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["UPC"]);
                //if (txtStandardProductID.Text != null && txtStandardProductID.Text != "")
                //    txtStandardProductID.Attributes.Add("ReadOnly", "true");


                try
                {

                    ddlproductidtype.Items.FindByText("UPC").Selected = true;

                    if (ddlproductidtype.SelectedItem.Text.ToString().Trim().ToLower() == "upc" && txtStandardProductID.Text.ToString().Trim() != "")
                    {
                        GenerateBarcode(txtStandardProductID.Text.ToString().Trim());
                    }
                }
                catch { }
            }
            if (DsProductAmazon != null && DsProductAmazon.Tables.Count > 0 && DsProductAmazon.Tables[0].Rows.Count > 0)
            {

                if (Request.QueryString["CloneID"] == null)
                {
                    ddlproductidtype.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ProductIDType"]);
                }
                try
                {
                    if (Request.QueryString["CloneID"] == null)
                    {
                        ddlManufacture.SelectedItem.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["Manufacturer"]);
                    }
                }
                catch
                {
                    ddlManufacture.SelectedIndex = 0;
                }
                //txtManufacturer.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["Manufacturer"]);
                txtMFRPartNumber.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["MfrPartNumber"]);
                if (Request.QueryString["CloneID"] == null)
                {
                    ddlProductType.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ProductType"]);
                }


                try
                {
                    txtItemType.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ItemType"]);
                }
                catch
                {
                }
                ddlFulfillment.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["FulfillmentCenterID"]);
                txtCurrency.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["Currency"]);
                txtBrand.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["Brand"]);
                txtModelName.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ModelName"]);
                txtModelNumber.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ModelNumber"]);
                txtMerchantCatalogNumber.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["MerchantCatalogNumber"]);
                txtBulletPoint1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["BulletPoint1"]);
                txtBulletPoint2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["BulletPoint2"]);
                txtBulletPoint3.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["BulletPoint3"]);
                txtBulletPoint4.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["BulletPoint4"]);
                txtBulletPoint5.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["BulletPoint5"]);
                txtProductDescription.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ProductDescription"]);
                txtKeyword1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["Keyword1"]);
                txtKeyword2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["Keyword2"]);
                txtKeyword3.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["Keyword3"]);
                txtMainImageURL.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["MainImageURL"]);
                ddlConditionType.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ConditionType"]);
                txtConditionNote.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ConditionNote"]);
                ddlShippingWeightUnitOfMeasure.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["shipping_weight_unit_of_measure"]);
                txtOtherImageUrl1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["OtherImageURL1"]);
                txtOtherImageUrl2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["OtherImageURL2"]);
                txtOtherImageUrl3.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["OtherImageURL3"]);
                txtOtherImageUrl4.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["OtherImageURL4"]);
                txtOtherImageUrl5.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["OtherImageURL5"]);
                txtOtherImageUrl6.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["OtherImageURL6"]);
                txtOtherImageUrl7.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["OtherImageURL7"]);
                txtOtherImageUrl8.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["OtherImageURL8"]);
                txtRelationshipType.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RelationshipType"]);

                chkProp65.Checked = Convert.ToBoolean(DsProductAmazon.Tables[0].Rows[0]["Prop65"].ToString());
                ddlCPSIAWarning1.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["CPSIAWarning1"]);
                ddlCPSIAWarning2.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["CPSIAWarning2"]);
                ddlCPSIAWarning3.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["CPSIAWarning3"]);
                ddlCPSIAWarning4.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["CPSIAWarning4"]);
                txtCPSIAWarningDescription.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["CPSIAWarningDescription"]);

                txtPlatinumKeywords1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["PlatinumKeywords1"]);
                txtPlatinumKeywords2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["PlatinumKeywords2"]);
                txtPlatinumKeywords3.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["PlatinumKeywords3"]);
                txtPlatinumKeywords4.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["PlatinumKeywords4"]);
                txtPlatinumKeywords5.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["PlatinumKeywords5"]);

                txtParentSKU.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ParentSKU"]);
                txtProductTaxCode.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ProductTaxCode"]);
                txtLaunchDate.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["LaunchDate"]);
                txtReleaseDate.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ReleaseDate"]);
                txtMAP.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["MAP"]);
                txtMSRP.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["MSRP"]);
                txtSaleStartDate.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SaleStartDate"]);
                txtSaleEndDate.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SaleEndDate"]);
                txtRebateStartDate1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RebateStartDate1"]);
                txtRebateEndDate1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RebateEndDate1"]);
                txtRebateMessage1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RebateMessage1"]);
                txtRebateName1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RebateName1"]);
                txtRebateStartDate2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RebateStartDate2"]);
                txtRebateEndDate2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RebateEndDate2"]);
                txtRebateMessage2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RebateMessage2"]);
                txtRebateName2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RebateName2"]);
                txtMaxOrderQuantity.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["MaxOrderQuantity"]);
                txtLeadtimeToShip.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["LeadtimeToShip"]);
                txtRestockDate.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RestockDate"]);
                txtItemsIncluded.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["ItemsIncluded"]);
                txtMaxAggregateShipQuantity.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["MaxAggregateShipQuantity"]);
                chkIsGiftWrapAvailable.Checked = Convert.ToBoolean(DsProductAmazon.Tables[0].Rows[0]["IsGiftWrapAvailable"]);
                chkIsGiftMessageAvailable.Checked = Convert.ToBoolean(DsProductAmazon.Tables[0].Rows[0]["IsGiftMessageAvailable"]);
                chkIsDiscontinuedByManufacturer.Checked = Convert.ToBoolean(DsProductAmazon.Tables[0].Rows[0]["IsDiscontinuedByManufacturer"]);
                ddlRegisteredParameter.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["RegisteredParameter"]);
                ddlUpdateDelete.SelectedValue = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["UpdateDelete"]);
                txtPoSForUseWith.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["PoS_ForUseWith"]);
                txtPoSBatteryChemicalType.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["PoS_BatteryChemicalType"]);
                txtPoSCameraPowerSupplyType.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["PoS_CameraPowerSupplyType"]);
                txtSSSurveillanceSystemType.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SS_SurveillanceSystemType"]);
                txtSSCameraType.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SS_CameraType"]);
                txtSSFeatures1.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SS_Features1"]);
                txtSSFeatures2.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SS_Features2"]);
                txtSSFeatures3.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SS_Features3"]);
                txtSSFeatures4.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SS_Features4"]);
                txtSSFeatures5.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SS_Features5"]);
                txtSSCameraAccessories.Text = Convert.ToString(DsProductAmazon.Tables[0].Rows[0]["SS_CameraAccessories"]);

            }
            #region set value for product

            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
                txtTitle.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Name"]);
                txtSKU.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SKU"]);

                string ckupcexist = Convert.ToString(CommonComponent.GetScalarCommonData("select SKU from tb_upcmasterreal where sku ='" + txtSKU.Text + "'"));
                string ckupcno = Convert.ToString(CommonComponent.GetScalarCommonData("select UPC from tb_upcmasterreal where sku ='" + txtSKU.Text + "'"));

                if (ckupcexist != null && ckupcexist.Length > 0)
                {
                    //txtStandardProductID.Text = ckupcno;
                    lbtngetUPC.Visible = false;
                }
                else
                {
                    lbtngetUPC.Visible = true;
                }


                txtWeight.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["weight"].ToString());
                txtInventory.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Inventory"].ToString());
                int ChkLowInventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(*) from tb_Product where ProductID = " + Request.QueryString["ID"].ToString() + " and (ISNULL( tb_Product.Inventory,0) <  ISNULL( tb_Product.LowInventory,0))"));
                if (ChkLowInventory > 0)
                {
                    ImgToggelInventory.Visible = true;
                }

                txtTitleDesc.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["TabTitle1"].ToString());
                txtDescription.Text = Server.HtmlDecode(DsProduct.Tables[0].Rows[0]["Description"].ToString());

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString().Trim()))
                    txtSalesPrice.Text = "0.00";
                else
                    txtSalesPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Price"].ToString().Trim()))
                    txtItemPrice.Text = "0.00";
                else
                    txtItemPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["Price"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["OurPrice"].ToString().Trim()))
                    txtOurPrice.Text = "0.00";
                else
                    txtOurPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["OurPrice"].ToString().Trim()), 2));

                txtInventory.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Inventory"].ToString());

                chkPublished.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Active"].ToString());
                chkIsDiscontinue.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Discontinue"].ToString());
                chkIsTabbingDisplay.Checked = true;
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Color"].ToString().Trim()))
                    ddlColormap.SelectedValue = DsProduct.Tables[0].Rows[0]["Color"].ToString();
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Size"].ToString().Trim()))
                    ddlDesign.SelectedValue = DsProduct.Tables[0].Rows[0]["Size"].ToString();

                chkIsHamming.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsHamming"].ToString());
                if (chkIsHamming.Checked)
                {
                    tdHamminglbl.Attributes.Add("style", "display:''");
                    tdHammingQty.Attributes.Add("style", "display:''");
                    txtHammingQty.Text = DsProduct.Tables[0].Rows[0]["HammingSafetyQty"].ToString();
                }

                try
                {
                    if (DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"] != null && DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"].ToString() != "")
                    {
                        //ddlProductTypeDelivery.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"]);
                        string name = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_ProductTypeDelivery where ProductTypeDeliveryID=" + DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"] + ""));
                        ddlProductTypeDelivery.ClearSelection();
                        ddlProductTypeDelivery.Items.FindByText(name).Selected = true;
                        ddlProductTypeDelivery_SelectedIndexChanged(null, null);
                    }
                    if (DsProduct.Tables[0].Rows[0]["ProductTypeID"] != null && DsProduct.Tables[0].Rows[0]["ProductTypeID"].ToString() != "")
                    {
                        //  ddlProductType.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductTypeID"]);

                        string name = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_ProductType where ProductTypeID=" + DsProduct.Tables[0].Rows[0]["ProductTypeID"] + ""));
                        ddlProductType.ClearSelection();
                        ddlProductType.Items.FindByText(name).Selected = true;
                        ddlProductType_SelectedIndexChanged(null, null);
                    }
                    if (DsProduct.Tables[0].Rows[0]["VendorID"] != null && DsProduct.Tables[0].Rows[0]["VendorID"].ToString() != "")
                    {
                        ddlvendor.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["VendorID"]);

                        //string name = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_Vendor where VendorID=" + DsProduct.Tables[0].Rows[0]["VendorID"] + ""));
                        //ddlvendor.ClearSelection();
                        //ddlvendor.Items.FindByText(name).Selected = true;
                    }
                }
                catch { }

                #region Get Image
                string Imagename = DsProduct.Tables[0].Rows[0]["Imagename"].ToString();
                String strImageName = RemoveSpecialCharacter(Convert.ToString(DsProduct.Tables[0].Rows[0]["SKU"]).ToCharArray()) + "_" + ID.ToString() + ".jpg";

                if (Imagename.ToString().Trim().ToLower().IndexOf("http") > -1)
                {
                    System.Net.WebClient objClient = new System.Net.WebClient();
                    String strSavedImgPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/") + strImageName.ToString();
                    objClient.DownloadFile(Imagename.ToString(), Server.MapPath(strSavedImgPath));
                    if (File.Exists(Server.MapPath(strSavedImgPath)))
                    {
                        ImgLarge.Src = strSavedImgPath.ToString();
                        ViewState["File"] = strImageName.ToString();
                    }
                    else
                    {
                        ImgLarge.Src = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/image_not_available.jpg");
                    }
                }
                else
                {
                    ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                    if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                        Directory.CreateDirectory(Server.MapPath(ProductMediumPath));

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
                }
                #endregion


                #region  Bind Category and Main Category Data

                if (DsProduct.Tables[0].Rows[0]["MainCategory"] != null && DsProduct.Tables[0].Rows[0]["MainCategory"].ToString() != "")
                {
                    txtMaincategory.Text = DsProduct.Tables[0].Rows[0]["MainCategory"].ToString().Trim();
                }

                DataSet dsCategory = new DataSet();
                CategoryComponent catc = new CategoryComponent();
                dsCategory = catc.getcategorydetailsbyProductID(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                {
                    string Category = ",";
                    for (int j = 0; j < dsCategory.Tables[0].Rows.Count; j++)
                    {
                        Category += dsCategory.Tables[0].Rows[j]["CategoryID"].ToString().Trim() + ",";
                    }
                    BindTreeData(Category);
                }
                #endregion


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
            #endregion

            }
        }

        /// <summary>
        /// Category Tree Binding
        /// </summary>
        /// <param name="Category">string Category</param>
        public void BindTreeData(string Category)
        {
            string CategoryList = Category;
            StringArrayConverter Categoryconvertor = new StringArrayConverter();
            Array CategoryArray = (Array)Categoryconvertor.ConvertFrom(CategoryList);
            if (CategoryArray.Length != 0)
            {
                foreach (TreeNode tn in trvCategories.Nodes)
                {
                    for (int j = 0; j < CategoryArray.Length; j++)
                    {
                        if (Category.Contains("," + tn.Value.ToString() + ","))
                        {
                            tn.Checked = true;
                            tn.Expanded = true;
                            if (tn.Parent != null)
                                tn.Parent.Expanded = true;
                        }
                        EditTree(Category, tn);
                    }
                }
            }
        }

        /// <summary>
        /// Edits the Tree view
        /// </summary>
        /// <param name="Name">string Name</param>
        /// <param name="tn">TreeNode tn</param>
        public void EditTree(string Name, TreeNode tn)
        {
            foreach (TreeNode tnchild in tn.ChildNodes)
            {
                if (Name.Contains("," + tnchild.Value.ToString() + ","))
                {
                    while (tn.Parent != null)
                    {
                        tn.Parent.Expanded = true;
                        tn = tn.Parent;
                    }
                    tnchild.Checked = true;
                    tnchild.Parent.Expanded = true;
                }
                EditTree(Name, tnchild);
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            float SalesPrice = float.Parse(txtSalesPrice.Text.Trim());
            float ItemPrice = float.Parse(txtItemPrice.Text.Trim());
            if (SalesPrice > ItemPrice)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Sales Price Should be less than or equal to Item Price', 'Message','ContentPlaceHolder1_txtItemPrice');});", true);
                txtSalesPrice.Focus();
                return;
            }
            //if (Request.QueryString["StoreID"] != null)
            //{
            //    if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "edit")
            //    {
            //        UpdateProduct(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
            //    }
            //    else
            //    {
            //        InsertProduct(Convert.ToInt32(Request.QueryString["StoreID"]));
            //    }
            //}
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().Trim().ToLower() == "edit")
            {
                UpdateProduct(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
            }
            else
            {
                int ChkClone = 1;
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString().Trim().ToLower() == "")
                {
                    ChkClone = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) TotCnt from tb_Product where StoreID=" + Request.QueryString["StoreID"].ToString() + " and SKU " +
                                   " in(Select SKU from tb_Product where ProductID =" + Request.QueryString["ID"].ToString() + ") and isnull(deleted,0)=0"));
                }
                if (ChkClone == 0)
                {
                    InsertCloneProduct(Convert.ToInt32(Request.QueryString["StoreID"]));
                }
                else
                {
                    InsertProduct(Convert.ToInt32(Request.QueryString["StoreID"]));
                }

            }
        }

        /// <summary>
        /// Function for Insert Product
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        public void InsertProduct(int StoreID)
        {
            tb_Product = new tb_Product();
            tb_Product = SetValue(tb_Product);

            tb_ProductAmazon = new tb_ProductAmazon();

            tb_ProductAmazon = SetValueAmazon(tb_ProductAmazon);
            tb_Product.TabTitle1 = "Product Description";
            tb_Product.CreatedBy = Convert.ToInt32(Session["AdminID"]);
            tb_Product.CreatedOn = System.DateTime.Now;
            tb_Product.Deleted = false;
            string CategoryValue = "";
            CategoryValue = SetCategory(StoreID);

            //if (string.IsNullOrEmpty(CategoryValue.ToString()))
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcateInsert", "$(document).ready( function() {jAlert('You must select at least one category!', 'Message');});", true);
            //    return;
            //}

            Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect ISNULL(count(sku),0) as TotCnt From tb_product Where sku='" + txtSKU.Text.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1"));
            if (ChkSKu > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU with same name already exists specify another sku', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                txtSKU.Focus();
                return;
            }
            else
            {
                Int32 ProductID = Convert.ToInt32(ProductComponent.InsertProduct(tb_Product));


                try
                {
                    if (ddlproductidtype.SelectedItem.Text.ToString().Trim().ToLower() == "upc" && txtStandardProductID.Text.ToString().Trim() != "")
                    {
                        GenerateBarcode(txtStandardProductID.Text.ToString().Trim());
                    }
                }
                catch { }



                tb_ProductAmazon.AmazonRefID = ProductID;
                Int32 ProductAmazonID = Convert.ToInt32(ProductComponent.InsertProductAmazon(tb_ProductAmazon));

                if (ProductID > 0)
                {
                    AddCategory(CategoryValue, ProductID);
                    string strImageName = "";
                    if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                    {
                        strImageName = RemoveSpecialCharacter(txtSKU.Text.ToString().ToCharArray()) + "_" + ProductID + ".jpg";
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

                    if (chkIsDiscontinue.Checked)
                        CommonComponent.ExecuteCommonData("Update tb_Product set DiscontinuedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");
                    CommonComponent.ExecuteCommonData("Update tb_Product set InventoryUpdatedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");

                    CommonComponent.ExecuteCommonData("Update tb_UPCMasterReal set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtStandardProductID.Text.Trim() + "'");
                    CommonComponent.ExecuteCommonData("Update tb_UPCMaster set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtStandardProductID.Text.Trim() + "'");

                    #region Save Inventory in warehouse

                    SaveInventoryInWarehouse(ProductID, 1);

                    #endregion

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

                    //#region new upc logic

                    //int cntSKU = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(SKU), sku from tb_Product where SKU='" + txtSKU.Text + "' group by SKU having COUNT(SKU) > 1"));

                    //if (cntSKU != null && cntSKU > 1)
                    //{

                    //    string existinUPC = Convert.ToString(CommonComponent.GetScalarCommonData("select UPC from tb_Product where storeid ! =" + Convert.ToInt32(Request.QueryString["StoreID"]) + " and sku='" + txtSKU.Text + "'"));
                    //    if (existinUPC != null && existinUPC.Length > 0)
                    //        CommonComponent.ExecuteCommonData("Update tb_product set UPC='" + existinUPC + "' where SKU='" + txtSKU.Text + "'");
                    //}
                    //#endregion

                    int cntUPC = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(upc) from tb_Product where SKU='" + txtSKU.Text + "' group by UPC having COUNT(upc) > 1 "));
                    if (cntUPC != 0 && cntUPC > 1)
                        chkUPC = true;
                    else
                        chkUPC = false;


                    //if (chkUPC == false)
                    //{
                    Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    //}
                    //else
                    //{
                    //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Another UPC with same SKU already exists in another Store, UPC for this SKU will be updated with that UPC!');window.location ='ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : "") + "';", true);
                    //}
                }
            }
        }

        /// <summary>
        /// Function for Update Product
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="StoreId">int StoreId</param>
        public void UpdateProduct(int ProductID, int StoreId)
        {

            ProductComponent objProduct = new ProductComponent();
            tb_Product = new tb_Product();
            tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);

            tb_Product = SetValue(tb_Product);


            tb_Product.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
            tb_Product.UpdatedOn = System.DateTime.Now;

            tb_ProductAmazon = objProduct.GetAllProductDetailsbyAmazonRefID(ProductID);
            tb_ProductAmazon = SetValueAmazon(tb_ProductAmazon);
            Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ISNULL(count(sku),0) as TotCnt From tb_product Where sku='" + txtSKU.Text.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " and Productid <>" + ProductID + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1"));
            if (ChkSKu > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU with same name already exists specify another sku', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                txtSKU.Focus();
                return;
            }
            else
            {

                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit" && Request.QueryString["ID"] != null)
                {
                    DeleteCategory(Convert.ToInt32(Request.QueryString["ID"].ToString()));
                }
                string CategoryValue = "";
                CategoryValue = SetCategory(StoreId);
                //if (string.IsNullOrEmpty(CategoryValue.ToString()))
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgCategory", "$(document).ready( function() {jAlert('You must select at least one category!', 'Message');});", true);
                //    return;
                //}

                if (Convert.ToInt32(ProductComponent.UpdateProduct(tb_Product)) > 0)
                {
                    try
                    {
                        if (ddlproductidtype.SelectedItem.Text.ToString().Trim().ToLower() == "upc" && txtStandardProductID.Text.ToString().Trim() != "")
                        {
                            GenerateBarcode(txtStandardProductID.Text.ToString().Trim());
                        }
                    }
                    catch { }

                    AddCategory(CategoryValue, ProductID);

                    #region Save and Update Image
                    string strImageName = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                        {
                            strImageName = RemoveSpecialCharacter(txtSKU.Text.ToString().ToCharArray()) + "_" + ProductID + ".jpg";
                            string Imgname = "";
                            if (!string.IsNullOrEmpty(tb_Product.ImageName))
                            {
                                Imgname = tb_Product.ImageName.ToString();
                            }
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

                    bool chkdisconti = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(Discontinue,0) from tb_Product where ProductID=" + ProductID + ""));
                    if ((chkdisconti == false && chkIsDiscontinue.Checked == true))
                        CommonComponent.ExecuteCommonData("Update tb_Product set DiscontinuedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");

                    Int32 inventoryUpdated = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select isnull(Inventory,0) from tb_Product where ProductID=" + ProductID + ""));

                    CommonComponent.ExecuteCommonData("Update tb_UPCMasterReal set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtStandardProductID.Text.Trim() + "'");
                    CommonComponent.ExecuteCommonData("Update tb_UPCMaster set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtStandardProductID.Text.Trim() + "'");

                    #region Update Inventory in warehouse

                    SaveInventoryInWarehouse(Convert.ToInt32(Request.QueryString["ID"].ToString()), 2);

                    #endregion

                    int VendorID = 0;
                    if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() != "vendor")
                        VendorID = 0;
                    else VendorID = Convert.ToInt32(ddlvendor.SelectedItem.Value);

                    CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET ProductTypeDeliveryID=" + Convert.ToInt32(ddlProductTypeDelivery.SelectedItem.Value) + ",ProductTypeID=" + Convert.ToInt32(ddlProductType.SelectedItem.Value) + ",VendorID=" + VendorID + " WHERE ProductID=" + ProductID + "");

                    #region dropship data and product data

                    if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                    {
                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "dropship" && dtAssembler != null && dtAssembler.Rows.Count > 0)
                        {
                            DeleteVendorData(ProductID);
                            FillData(ProductID);
                        }

                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "vendor" && dt != null && dt.Rows.Count > 0)
                        {
                            DeleteDropShipData(ProductID);
                            FillProductData(ProductID);
                        }
                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "dropship" && (dtAssembler == null || dtAssembler.ToString() == ""))
                        {

                            FillData(ProductID);
                        }
                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() == "vendor" && (dt == null || dt.ToString() == ""))
                        {

                            FillProductData(ProductID);
                        }
                        // FillData(ProductID);
                        // FillProductData(ProductID);
                    }
                    #endregion

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

                            if (inventoryUpdated != Convert.ToInt32(updatedQuantity))
                                CommonComponent.ExecuteCommonData("Update tb_Product set InventoryUpdatedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");
                        }
                    }
                    else
                    {
                        if (inventoryUpdated != Convert.ToInt32(txtInventory.Text))
                            CommonComponent.ExecuteCommonData("Update tb_Product set InventoryUpdatedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");
                    }

                    #endregion

                    //int cntSKU = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(SKU), sku from tb_Product where SKU='" + txtSKU.Text + "' group by SKU having COUNT(SKU) > 1"));

                    //if (cntSKU != null && cntSKU > 1)
                    //{
                    //    CommonComponent.ExecuteCommonData("Update tb_product set UPC='" + txtStandardProductID.Text + "' where SKU='" + txtSKU.Text + "'");
                    //}


                    //int cntUPC = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(upc) from tb_Product where SKU='" + txtSKU.Text + "' group by UPC having COUNT(upc) > 1 "));
                    //if (cntUPC != 0 && cntUPC > 1)
                    //    chkUPC = true;
                    //else
                    //    chkUPC = false;


                    //if (chkUPC == false)
                    //{
                    Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    //}
                    //else
                    //{
                    //    Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    //    // System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Another UPC with same SKU already exists in another Store, UPC for this SKU will be updated with that UPC!');window.location ='ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : "") + "';", true);
                    //}
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

        /// <summary>
        /// Delete Already Existing Categories
        /// </summary>
        /// <param name="product">int product</param>
        public void DeleteCategory(int product)
        {
            CategoryComponent.DeleteProductCategory(product);
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {

            ViewState["DelImage"] = null;
            ViewState["File"] = null;
            Response.Redirect("ProductList.aspx?cancel=1&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
        }

        /// <summary>
        /// Description Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlDescription_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        /// <summary>
        /// Set Category
        /// </summary>
        /// <returns>Returns Categories as a String Value</returns>
        public string SetCategory(Int32 StoreId)
        {
            Int32 CatIDforMainCat = 0;
            ArrayList Category = new ArrayList();
            Category = AddProductCategory();
            Category.Sort();
            string CategoryValue = ",";
            for (int i = 0; i < Category.Count; i++)
            {
                if (i == 0)
                {
                    CatIDforMainCat = Convert.ToInt32(Category[i].ToString().Trim());
                }

                if (i == Category.Count)
                    CategoryValue += Category[i].ToString().Trim();
                else
                    CategoryValue += Category[i].ToString().Trim() + ",";
            }

            if (Category.Count == 0)
            {
                return "";
            }

            if (string.IsNullOrEmpty(txtMaincategory.Text) || !string.IsNullOrEmpty(CatIDforMainCat.ToString()))
            {
                CategoryComponent catc = new CategoryComponent();
                DataSet dsMainCat = catc.getCatdetailbycatid(CatIDforMainCat, StoreId);
                String CatNameforMainCat = "";
                if (dsMainCat != null && dsMainCat.Tables.Count > 0 && dsMainCat.Tables[0].Rows.Count > 0)
                {
                    CatNameforMainCat = Convert.ToString(dsMainCat.Tables[0].Rows[0]["Name"]);
                }
                txtMaincategory.Text = CatNameforMainCat;
            }


            return CategoryValue;
        }

        /// <summary>
        /// Set Value into Product Table
        /// </summary>
        /// <param name="tb_Product">tb_Product tb_Product</param>
        /// <returns>Returns the tb_Product Table Object</returns>
        public tb_Product SetValue(tb_Product tb_Product)
        {
            if (Request.QueryString["StoreID"] != null)
                tb_Product.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(Request.QueryString["StoreID"].ToString()));

            tb_Product.Name = Convert.ToString(txtTitle.Text.Trim());

            tb_Product.SKU = txtSKU.Text.Trim();
            // tb_Product.UPC = txtStandardProductID.Text.ToString();
            //DataSet dsUPC = null;

            //if (lbtngetUPC.Visible == false)
            //    dsUPC = CommonComponent.GetCommonDataSet("select SKU,UPC from tb_upcmasterreal where sku is not null ");
            //else
            //    dsUPC = CommonComponent.GetCommonDataSet("select SKU,UPC from tb_upcmaster where sku is not null ");

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
                        //txtStandardProductID.Text = dsUPC.Tables[0].Rows[i]["UPC"].ToString();
                        tb_Product.UPC = dsUPC.Tables[0].Rows[i]["UPC"].ToString();
                        break;
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkUPC", "$(document).ready( function() {jAlert('UPC with same SKU already exists in another Store', 'Message','ContentPlaceHolder1_txtUPC');});", true);

                    }
                    else
                    {

                        tb_Product.UPC = txtStandardProductID.Text.Trim();
                    }
                }
            }
            else
            {
                tb_Product.UPC = txtStandardProductID.Text.Trim();
            }


            decimal Price = decimal.Zero;
            decimal.TryParse(txtItemPrice.Text.Trim(), out Price);
            tb_Product.Price = Convert.ToDecimal(Price);

            decimal SalesPrice = decimal.Zero;
            decimal.TryParse(txtSalesPrice.Text.Trim(), out SalesPrice);
            tb_Product.SalePrice = Convert.ToDecimal(SalesPrice);

            decimal OurPrice = decimal.Zero;
            decimal.TryParse(txtOurPrice.Text.Trim(), out OurPrice);
            tb_Product.OurPrice = Convert.ToDecimal(OurPrice);

            if (txtInventory.Text.Trim().Length == 0)
                txtInventory.Text = "0";
            tb_Product.Inventory = Convert.ToInt32(txtInventory.Text.Trim());

            decimal Weight = decimal.Zero;
            decimal.TryParse(txtWeight.Text.Trim(), out Weight);
            tb_Product.Weight = Convert.ToDecimal(Weight);
            //tb_Product.tb_Manufacture.ManufactureID = Convert.ToInt32(ddlManufacture.SelectedValue);
            tb_Product.tb_ManufactureReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Manufacture", "ManufactureID", Convert.ToInt32(ddlManufacture.SelectedValue));
            tb_Product.Active = chkPublished.Checked;
            tb_Product.Discontinue = chkIsDiscontinue.Checked;
            tb_Product.Color = ddlColormap.SelectedValue.ToString();
            tb_Product.Size = ddlDesign.SelectedValue.ToString();

            tb_Product.IsHamming = chkIsHamming.Checked;
            if (chkIsHamming.Checked)
            {
                Int32 HammingQty = 0;
                Int32.TryParse(txtHammingQty.Text.ToString(), out HammingQty);
                Int32 InvQty = 0;
                Int32.TryParse(txtInventory.Text.ToString(), out InvQty);
                if (InvQty >= HammingQty)
                {
                    tb_Product.HammingSafetyQty = HammingQty;
                }
            }
            else
            {
                tb_Product.HammingSafetyQty = Convert.ToInt16(0);
            }

            //tb_Product.tb_ProductType.ProductTypeID = Convert.ToInt32(ddlProductType.SelectedValue);
            tb_Product.tb_ProductTypeReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_ProductType", "ProductTypeID", Convert.ToInt32(2));

            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit" && Request.QueryString["ID"] != null)
            {
                if (ddlDescription.SelectedValue == "1")
                {
                    tb_Product.Description = Convert.ToString((txtDescription.Text.Trim().Replace("&nbsp;", " ")));
                }
                else
                    tb_Product.Description = Convert.ToString((tb_Product.Description)).Replace("&nbsp;", " ");
            }
            else
                tb_Product.Description = Convert.ToString((txtDescription.Text.Trim().Replace("&nbsp;"," ")));

            return tb_Product;
        }

        /// <summary>
        /// Sets the value for Amazon Product
        /// </summary>
        /// <param name="tb_ProductAmazon">tb_ProductAmazon tb_ProductAmazon</param>
        /// <returns>Returns the tb_ProductAmazon Table Object</returns>
        public tb_ProductAmazon SetValueAmazon(tb_ProductAmazon tb_ProductAmazon)
        {
            if (ddlproductidtype.SelectedValue != "0")
            {
                tb_ProductAmazon.ProductIDType = ddlproductidtype.SelectedItem.Text;
            }
            else
            {
                tb_ProductAmazon.ProductIDType = null;
            }
            tb_ProductAmazon.StandardProductID = txtStandardProductID.Text.Trim();
            tb_ProductAmazon.Manufacturer = ddlManufacture.SelectedItem.Text.ToString();
            tb_ProductAmazon.MfrPartNumber = txtMFRPartNumber.Text.Trim();
            tb_ProductAmazon.ItemType = txtItemType.SelectedValue.Trim();


            if (ddlFulfillment.SelectedValue != "0")
            {
                tb_ProductAmazon.FulfillmentCenterID = ddlFulfillment.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.FulfillmentCenterID = null;
            }
            tb_ProductAmazon.Currency = txtCurrency.Text.Trim();
            if (ddlProductType.SelectedValue != "0")
            {
                tb_ProductAmazon.ProductType = ddlProductType.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.ProductType = null;
            }

            tb_ProductAmazon.Brand = txtBrand.Text.Trim();
            tb_ProductAmazon.ModelName = txtModelName.Text.Trim();
            tb_ProductAmazon.ModelNumber = txtModelNumber.Text.Trim();
            tb_ProductAmazon.MerchantCatalogNumber = txtMerchantCatalogNumber.Text.Trim();
            tb_ProductAmazon.BulletPoint1 = txtBulletPoint1.Text.Trim();
            tb_ProductAmazon.BulletPoint2 = txtBulletPoint2.Text.Trim();
            tb_ProductAmazon.BulletPoint3 = txtBulletPoint3.Text.Trim();
            tb_ProductAmazon.BulletPoint4 = txtBulletPoint4.Text.Trim();
            tb_ProductAmazon.BulletPoint5 = txtBulletPoint5.Text.Trim();
            tb_ProductAmazon.ProductDescription = txtProductDescription.Text.Trim();
            tb_ProductAmazon.Keyword1 = txtKeyword1.Text.Trim();
            tb_ProductAmazon.Keyword2 = txtKeyword2.Text.Trim();
            tb_ProductAmazon.Keyword3 = txtKeyword3.Text.Trim();

            tb_ProductAmazon.MainImageURL = txtMainImageURL.Text.Trim();
            if (ddlConditionType.SelectedValue != "0")
            {
                tb_ProductAmazon.ConditionType = ddlConditionType.SelectedValue;
            }
            else
                tb_ProductAmazon.ConditionType = null;

            tb_ProductAmazon.ConditionNote = txtConditionNote.Text.Trim();
            if (ddlShippingWeightUnitOfMeasure.SelectedValue != "0")
            {
                tb_ProductAmazon.shipping_weight_unit_of_measure = ddlShippingWeightUnitOfMeasure.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.shipping_weight_unit_of_measure = null;
            }
            tb_ProductAmazon.Currency = txtCurrency.Text.Trim();

            tb_ProductAmazon.OtherImageURL1 = txtOtherImageUrl1.Text.Trim();
            tb_ProductAmazon.OtherImageURL2 = txtOtherImageUrl2.Text.Trim();
            tb_ProductAmazon.OtherImageURL3 = txtOtherImageUrl3.Text.Trim();
            tb_ProductAmazon.OtherImageURL4 = txtOtherImageUrl4.Text.Trim();
            tb_ProductAmazon.OtherImageURL5 = txtOtherImageUrl5.Text.Trim();
            tb_ProductAmazon.OtherImageURL6 = txtOtherImageUrl6.Text.Trim();
            tb_ProductAmazon.OtherImageURL7 = txtOtherImageUrl7.Text.Trim();
            tb_ProductAmazon.OtherImageURL8 = txtOtherImageUrl8.Text.Trim();
            tb_ProductAmazon.RelationshipType = txtRelationshipType.Text.Trim();
            tb_ProductAmazon.Prop65 = chkProp65.Checked;

            if (ddlCPSIAWarning1.SelectedValue != "0")
            {
                tb_ProductAmazon.CPSIAWarning1 = ddlCPSIAWarning1.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.CPSIAWarning1 = null;
            }
            if (ddlCPSIAWarning2.SelectedValue != "0")
            {
                tb_ProductAmazon.CPSIAWarning2 = ddlCPSIAWarning2.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.CPSIAWarning2 = null;
            }
            if (ddlCPSIAWarning3.SelectedValue != "0")
            {
                tb_ProductAmazon.CPSIAWarning3 = ddlCPSIAWarning3.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.CPSIAWarning3 = null;
            }
            if (ddlCPSIAWarning4.SelectedValue != "0")
            {
                tb_ProductAmazon.CPSIAWarning4 = ddlCPSIAWarning4.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.CPSIAWarning4 = null;
            }

            tb_ProductAmazon.CPSIAWarningDescription = txtCPSIAWarningDescription.Text.Trim();
            tb_ProductAmazon.PlatinumKeywords1 = txtPlatinumKeywords1.Text.Trim();
            tb_ProductAmazon.PlatinumKeywords2 = txtPlatinumKeywords2.Text.Trim();
            tb_ProductAmazon.PlatinumKeywords3 = txtPlatinumKeywords3.Text.Trim();
            tb_ProductAmazon.PlatinumKeywords4 = txtPlatinumKeywords4.Text.Trim();
            tb_ProductAmazon.PlatinumKeywords5 = txtPlatinumKeywords5.Text.Trim();
            tb_ProductAmazon.ParentSKU = txtParentSKU.Text.Trim();
            tb_ProductAmazon.ProductTaxCode = txtProductTaxCode.Text.Trim();
            if (txtLaunchDate.Text != "")
            {
                tb_ProductAmazon.LaunchDate = Convert.ToDateTime(txtLaunchDate.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.LaunchDate = null;
            }
            if (txtReleaseDate.Text != "")
            {
                tb_ProductAmazon.ReleaseDate = Convert.ToDateTime(txtReleaseDate.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.ReleaseDate = null;
            }
            if (txtMAP.Text != "")
            {
                tb_ProductAmazon.MAP = Convert.ToDecimal(txtMAP.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.MAP = null;
            }
            if (txtMSRP.Text != "")
            {
                tb_ProductAmazon.MSRP = Convert.ToDecimal(txtMSRP.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.MSRP = null;
            }
            if (txtSaleStartDate.Text != "")
            {
                tb_ProductAmazon.SaleStartDate = Convert.ToDateTime(txtSaleStartDate.Text.Trim());
            }

            else
            {
                tb_ProductAmazon.SaleStartDate = null;
            }

            if (txtSaleEndDate.Text != "")
            {
                tb_ProductAmazon.SaleEndDate = Convert.ToDateTime(txtSaleEndDate.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.SaleEndDate = null;
            }

            if (txtRebateStartDate1.Text != "")
            {
                tb_ProductAmazon.RebateStartDate1 = Convert.ToDateTime(txtRebateStartDate1.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.RebateStartDate1 = null;
            }

            if (txtRebateEndDate1.Text != "")
            {
                tb_ProductAmazon.RebateEndDate1 = Convert.ToDateTime(txtRebateEndDate1.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.RebateEndDate1 = null;
            }

            tb_ProductAmazon.RebateMessage1 = txtRebateMessage1.Text.Trim();
            tb_ProductAmazon.RebateName1 = txtRebateName1.Text.Trim();
            if (txtRebateStartDate2.Text != "")
            {
                tb_ProductAmazon.RebateStartDate2 = Convert.ToDateTime(txtRebateStartDate2.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.RebateStartDate2 = null;
            }

            if (txtRebateEndDate2.Text != "")
            {
                tb_ProductAmazon.RebateEndDate2 = Convert.ToDateTime(txtRebateEndDate2.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.RebateEndDate2 = null;
            }

            tb_ProductAmazon.RebateMessage2 = txtRebateMessage2.Text.Trim();
            tb_ProductAmazon.RebateName2 = txtRebateName2.Text.Trim();
            if (txtMaxOrderQuantity.Text.Trim().Length != 0)
            {
                tb_ProductAmazon.MaxOrderQuantity = Convert.ToDecimal(txtMaxOrderQuantity.Text.Trim());
            }
            else
                tb_ProductAmazon.MaxOrderQuantity = Convert.ToDecimal(0);

            if (txtLeadtimeToShip.Text.Trim().Length != 0)
            {
                tb_ProductAmazon.LeadtimeToShip = Convert.ToDecimal(txtLeadtimeToShip.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.LeadtimeToShip = Convert.ToDecimal(0);
            }
            if (txtRestockDate.Text != "")
            {
                tb_ProductAmazon.RestockDate = Convert.ToDateTime(txtRestockDate.Text.Trim());
            }
            else
            {
                tb_ProductAmazon.RestockDate = null;
            }

            tb_ProductAmazon.ItemsIncluded = txtItemsIncluded.Text.Trim();
            if (txtMaxAggregateShipQuantity.Text.Trim().Length != 0)
            {
                tb_ProductAmazon.MaxAggregateShipQuantity = Convert.ToDecimal(txtMaxAggregateShipQuantity.Text.Trim());
            }
            else
                tb_ProductAmazon.MaxAggregateShipQuantity = Convert.ToDecimal(0);

            tb_ProductAmazon.IsGiftWrapAvailable = chkIsGiftWrapAvailable.Checked;
            tb_ProductAmazon.IsGiftWrapAvailable = chkIsGiftMessageAvailable.Checked;

            if (ddlRegisteredParameter.SelectedValue != "0")
            {
                tb_ProductAmazon.RegisteredParameter = ddlRegisteredParameter.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.RegisteredParameter = null;
            }

            if (ddlUpdateDelete.SelectedValue != "0")
            {
                tb_ProductAmazon.UpdateDelete = ddlUpdateDelete.SelectedValue;
            }
            else
            {
                tb_ProductAmazon.UpdateDelete = null;
            }

            tb_ProductAmazon.PoS_ForUseWith = txtPoSForUseWith.Text.Trim();
            tb_ProductAmazon.PoS_BatteryChemicalType = txtPoSBatteryChemicalType.Text.Trim();
            tb_ProductAmazon.PoS_CameraPowerSupplyType = txtPoSBatteryChemicalType.Text.Trim();
            tb_ProductAmazon.SS_SurveillanceSystemType = txtSSSurveillanceSystemType.Text.Trim();
            tb_ProductAmazon.SS_CameraType = txtSSCameraType.Text.Trim();
            tb_ProductAmazon.SS_Features1 = txtSSFeatures1.Text.Trim();
            tb_ProductAmazon.SS_Features2 = txtSSFeatures2.Text.Trim();
            tb_ProductAmazon.SS_Features3 = txtSSFeatures3.Text.Trim();
            tb_ProductAmazon.SS_Features4 = txtSSFeatures4.Text.Trim();
            tb_ProductAmazon.SS_Features5 = txtSSFeatures5.Text.Trim();
            tb_ProductAmazon.SS_CameraAccessories = txtSSCameraAccessories.Text.Trim();

            return tb_ProductAmazon;
        }

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
                CommonComponent.ErrorLog("ProductAmazon.aspx", ex.Message, ex.StackTrace);
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

                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductLargePath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "medium":
                        strFilePath = Server.MapPath(ProductMediumPath + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductMediumPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "icon":
                        strFilePath = Server.MapPath(ProductIconPath + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductIconPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "micro":
                        strFilePath = Server.MapPath(ProductMicroPath + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
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
        /// Resize Photo
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="Size">string Size</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizePhoto(string strFile, string Size, string strFilePath)
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
        /// Function for Remove Special Characters
        /// </summary>
        /// <param name="charr">char[] charr</param>
        /// <returns>Returns String value after Remove Special Character</returns>
        public static string RemoveSpecialCharacter(char[] charr)
        {
            string res = "";
            string value = new string(charr);
            value = value.Replace('~', '-');
            value = value.Replace('!', '-');
            value = value.Replace('@', '-');
            value = value.Replace('#', '-');
            value = value.Replace('$', '-');
            value = value.Replace('%', '-');
            value = value.Replace('^', '-');
            value = value.Replace('&', '-');
            value = value.Replace('*', '-');
            value = value.Replace('(', '-');
            value = value.Replace(')', '-');
            value = value.Replace('_', '-');
            value = value.Replace('+', '-');
            value = value.Replace('|', '-');
            value = value.Replace('\\', '-');
            value = value.Replace('/', '-');
            value = value.Replace('?', '-');
            value = value.Replace('\'', '-');
            value = value.Replace('"', '-');
            value = value.Replace(' ', '-');
            value = value.Replace('>', '-');
            value = value.Replace('<', '-');
            value = value.Replace('.', '-');
            value = value.Replace(',', '-');
            value = value.Replace(':', '-');
            value = value.Replace("'", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            res = value;
            return res;
        }

        /// <summary>
        /// Bind Category
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        protected void BindCategory(Int32 StoreID)
        {
            DataSet dsCategory = new DataSet();
            dsCategory = CategoryComponent.GetCategoryByStoreID(StoreID, 3);
            LoadCategoryTree(StoreID.ToString(), dsCategory);
        }

        /// <summary>
        /// Bind Category in TreeView
        /// </summary>
        /// <param name="StoreID">string StoreID</param>
        /// <param name="dsCategory">Dataset dsCategory</param>
        public void LoadCategoryTree(string StoreID, DataSet dsCategory)
        {
            string CatName = "";
            trvCategories.Nodes.Clear();
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0 && dsCategory.Tables[0].Select("ParentCategoryID=0").Length > 0)
            {
                foreach (DataRow dr in dsCategory.Tables[0].Select("ParentCategoryID=0"))
                {
                    CatName = dr["Name"].ToString();
                    TreeNode myNode = new TreeNode();
                    myNode.Text = "<span onclick=\"this.parentNode.parentNode.firstChild.checked=!this.parentNode.parentNode.firstChild.checked;return false;\">" + CatName + "</span>";
                    myNode.Value = dr["CategoryID"].ToString();
                    myNode.ShowCheckBox = true;
                    myNode.CollapseAll();
                    trvCategories.Nodes.Add(myNode);
                    LoadChildNode(Convert.ToInt32(dr["CategoryID"].ToString()), myNode, dsCategory);
                }
            }
        }

        /// <summary>
        /// Bind Child Category in TreeView
        /// </summary>
        /// <param name="id">int id</param>
        /// <param name="tn">TreeNode tn</param>
        /// <param name="dsCategory">Dataset dsCategory</param>
        public void LoadChildNode(int id, TreeNode tn, DataSet dsCategory)
        {
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0 && dsCategory.Tables[0].Select("ParentCategoryID=" + id).Length > 0)
            {
                foreach (DataRow dr in dsCategory.Tables[0].Select("ParentCategoryID=" + id))
                {
                    string ChildCatName = dr["Name"].ToString();
                    TreeNode tnChild = new TreeNode();
                    tnChild.Text = "<span onclick=\"this.parentNode.parentNode.firstChild.checked=!this.parentNode.parentNode.firstChild.checked;return false;\">" + ChildCatName + "</span>";
                    tnChild.Value = dr["CategoryID"].ToString();
                    tnChild.ShowCheckBox = true;
                    tnChild.CollapseAll();
                    tn.ChildNodes.Add(tnChild);
                    LoadChildNode(Convert.ToInt32(dr["CategoryID"].ToString()), tnChild, dsCategory);
                }
            }
        }

        /// <summary>
        /// Function for Add Product Category
        /// </summary>
        /// <returns>Returns the Arralist of Product Category</returns>
        public ArrayList AddProductCategory()
        {
            ArrayList CategoryArray = new ArrayList();
            foreach (TreeNode tn in trvCategories.Nodes)
            {
                if (tn.Checked == true)
                {
                    CategoryArray.Add(tn.Value);
                }
                CategoryArray = GetCategoryIDList(CategoryArray, tn);
            }
            return CategoryArray;
        }

        /// <summary>
        /// Get Category using Id
        /// </summary>
        /// <param name="array">ArrayList array</param>
        /// <param name="tn">TreeNode tn</param>
        /// <returns>Returns the Arralist of CategoryIDs</returns>
        public ArrayList GetCategoryIDList(ArrayList array, TreeNode tn)
        {
            foreach (TreeNode tchild in tn.ChildNodes)
            {
                if (tchild.Checked == true)
                {
                    array.Add(tchild.Value);
                }
                GetCategoryIDList(array, tchild);
            }
            return array;
        }

        /// <summary>
        /// Add Product Category function
        /// </summary>
        /// <param name="CategoryID">String CategoryID</param>
        /// <param name="ProductID">int ProductID</param>
        public void AddCategory(string CategoryID, int ProductID)
        {
            Int32 CatID = 0;
            StringArrayConverter Catconvertor = new StringArrayConverter();
            Array CatArray = (Array)Catconvertor.ConvertFrom(CategoryID);
            for (int i = 0; i < CatArray.Length; i++)
            {
                if (CatArray.GetValue(i).ToString() != "")
                {
                    tbProductCategory = new tb_ProductCategory();
                    CatID = Convert.ToInt32(CatArray.GetValue(i).ToString());
                    tbProductCategory.CategoryID = CatID;
                    tbProductCategory.ProductID = ProductID;
                    DataSet dsDisplay = ProductComponent.GetDisplayOrderByProductIDAndCategoryID(Convert.ToInt32(ProductID), Convert.ToInt32(CatID));
                    if (dsDisplay != null && dsDisplay.Tables.Count > 0 && dsDisplay.Tables[0].Rows.Count > 0)
                    {
                        tbProductCategory.DisplayOrder = Convert.ToInt32(dsDisplay.Tables[0].Rows[0]["DisplayOrder"].ToString().Trim());
                    }
                    else
                    {
                        tbProductCategory.DisplayOrder = 0;
                    }
                    ProductComponent.InsertProductCategory(tbProductCategory);
                }
            }
        }


        /// <summary>
        /// Product Type Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {

            //if (ddlProductType.SelectedItem.Text.ToLower() != "assembly product")
            //{
            //    grdAssembler.Visible = false;
            //}
            //else
            //{
            //    grdAssembler.Visible = true;
            //}

            //if (ddlProductType.SelectedIndex == 0)
            //{
            //    grdAssembler.Visible = false;
            //    grdDropShip.Visible = false;
            //}
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
        /// Delete Iamge
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
        ///  Upload Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click1(object sender, ImageClickEventArgs e)
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
        /// Bind Product Type Dropdown
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        //protected void BindProductType(Int32 StoreID)
        //{
        //    DataSet dsProductType = new DataSet();
        //    dsProductType = ProductTypeComponent.GetProductTypeByStoreID(StoreID);
        //    if (dsProductType != null && dsProductType.Tables.Count > 0 && dsProductType.Tables[0].Rows.Count > 0)
        //    {
        //        ddlProductType.DataSource = dsProductType;
        //        ddlProductType.DataTextField = "Name";
        //        ddlProductType.DataValueField = "ProductTypeID";
        //    }
        //    else
        //    {
        //        ddlProductType.DataSource = null;
        //    }
        //    ddlProductType.DataBind();
        //    ddlProductType.Items.Insert(0, new ListItem("Select Product Type", "0"));
        //    if (dsProductType != null && dsProductType.Tables.Count > 0 && dsProductType.Tables[0].Rows.Count > 0)
        //        ddlProductType.SelectedIndex = 1;
        //}


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
            // if (dsProductTypeDelivery != null && dsProductTypeDelivery.Tables.Count > 0 && dsProductTypeDelivery.Tables[0].Rows.Count > 0)
            // ddlProductTypeDelivery.SelectedIndex = 1;
            if (ddlProductTypeDelivery.SelectedIndex == 0)
            {
                grdAssembler.Visible = false;
                grdDropShip.Visible = false;
            }
        }

        #region Code for Ware House

        /// <summary>
        /// Gets the Warehouse List
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Mode">int Mode</param>_
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
                Label lblPreferredLocation = (Label)e.Row.FindControl("lblPreferredLocation");
                RadioButton rdowarehouse = (RadioButton)e.Row.FindControl("rdowarehouse");
                TextBox txtWarehouseInventory = (TextBox)e.Row.FindControl("txtInventory");

                try
                {
                    if (lblPreferredLocation != null && !string.IsNullOrEmpty(lblPreferredLocation.Text.ToString().Trim()) && lblPreferredLocation.Text.ToString() == "1")
                    {
                        rdowarehouse.Checked = true;
                    }
                }
                catch { }

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
                RadioButton rdowarehouse = (RadioButton)row.FindControl("rdowarehouse");
                TextBox txtInventory = (row.FindControl("txtInventory") as TextBox);

                bool PreferredLocation = false;
                if (rdowarehouse.Checked)
                {
                    PreferredLocation = true;
                }
                if (txtInventory != null)
                {
                    if (int.TryParse(txtInventory.Text.Trim(), out Inventory))
                    {

                        objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, Inventory, mode, PreferredLocation);
                        InventoryTotal += Inventory;
                    }
                    else
                    {
                        objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, 0, mode, PreferredLocation);
                    }
                }
                else
                {
                    objProductComp.InsertUpdateWarehouse(Convert.ToInt32(lblWarehouse.Text), ProductID, 0, mode, PreferredLocation);
                }

            }

            objProductComp.UpdateProductinventory(ProductID, InventoryTotal, Convert.ToInt32(Request.QueryString["StoreID"]));
        }

        #endregion

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
                    url = "ProductAmazon.aspx?StoreID=" + CurrentStoreID.ToString() + "&ID=" + Request.QueryString["oldid"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString();
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

        protected void lbtngetUPC_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["StoreID"] != null)
            {
                if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                {
                    string checkrealupc = Convert.ToString(CommonComponent.GetScalarCommonData("Select SKU from tb_UPCMasterReal where UPC='" + txtStandardProductID.Text.Trim() + "' and SKU ='" + txtSKU.Text.Trim() + "'"));
                    if (checkrealupc != null && checkrealupc.Length > 0)
                    {
                        lbtngetUPC.Visible = false;
                    }
                    else
                    {
                        StrUPCReal = Convert.ToString(CommonComponent.GetScalarCommonData("select TOP 1 UPC from tb_UPCMasterReal where sku is null ORDER BY NEWID() "));
                        if (StrUPCReal != null && StrUPCReal.Length > 0)
                        {
                            txtStandardProductID.Text = StrUPCReal;
                            lbtngetUPC.Visible = false;
                            StrUPC = null;
                        }
                    }
                }
                else
                {
                    StrUPCReal = Convert.ToString(CommonComponent.GetScalarCommonData("select TOP 1 UPC from tb_UPCMasterReal where sku is null ORDER BY NEWID() "));
                    if (StrUPCReal != null && StrUPCReal.Length > 0)
                    {
                        txtStandardProductID.Text = StrUPCReal;
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
                        //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Enter Valid Quantity!');", true);
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


        /// <summary>
        /// insert Product When its Clone Product
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        public void InsertCloneProduct(int StoreID)
        {
            tb_Product = new tb_Product();
            tb_Product = SetValue(tb_Product);
            tb_Product.TabTitle1 = "Product Description";
            tb_Product.CreatedBy = Convert.ToInt32(Session["AdminID"]);
            tb_Product.CreatedOn = System.DateTime.Now;
            tb_Product.Deleted = false;

            string CategoryValue = "";
            CategoryValue = SetCategory(StoreID);

            Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect ISNULL(count(sku),0) as TotCnt From tb_product Where sku='" + txtSKU.Text.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1"));
            if (ChkSKu > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU with same name already exists specify another sku', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                txtSKU.Focus();
                return;
            }
            else
            {
                Int32 ProductID = Convert.ToInt32(ProductComponent.InsertProduct(tb_Product));

                if (ProductID > 0)
                {
                    AddCategory(CategoryValue, ProductID);

                    string strImageName = "";
                    if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                    {
                        strImageName = RemoveSpecialCharacter(txtSKU.Text.ToString().ToCharArray()) + "_" + ProductID + ".jpg";
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

                    // .... Add Product Variant ...
                    Int32 Result = 0;// Convert.ToInt32(ProductComponent.SaveCloneProductVariant(Convert.ToInt32(Request.QueryString["StoreId"]), Convert.ToInt32(Request.QueryString["Id"]), ProductID));
                    Result = ProductID;
                    if (Result > 0)
                    {
                        try
                        {
                            DataSet DsProduct = new DataSet();
                            DsProduct = CommonComponent.GetCommonDataSet("Select * From tb_Product Where ProductId=" + Convert.ToInt32(Request.QueryString["Id"]) + " and isnull(deleted,0)=0");
                            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
                            {
                                // Old Product File Path
                                Int32 OldStoreId = Convert.ToInt32(DsProduct.Tables[0].Rows[0]["StoreId"].ToString());
                                if (OldStoreId > 0)
                                    AppConfig.StoreID = Convert.ToInt32(OldStoreId);
                                string OldProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                                string OldProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                                string OldProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                                string OldProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");
                                string ProductPdfPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "PDF/");
                                string Sourcetemppath = AppLogic.AppConfigs("VideoPath").ToString();

                                // New Product File Path
                                AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");
                                string DestinationPdfPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "PDF/");
                                string targettemppath = AppLogic.AppConfigs("VideoPath").ToString();

                                if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                                    Directory.CreateDirectory(Server.MapPath(ProductMediumPath));
                                string Imagename = Convert.ToString(DsProduct.Tables[0].Rows[0]["Imagename"].ToString());

                                #region Save Product Images

                                string Sku = Convert.ToString(DsProduct.Tables[0].Rows[0]["sku"].ToString());

                                string strFilePath = Server.MapPath(OldProductMediumPath + Imagename);
                                //if (File.Exists(strFilePath) && !string.IsNullOrEmpty(Imagename.ToString()))
                                //{
                                //    File.Copy(strFilePath, Server.MapPath(ProductMediumPath + Sku + "_" + Result.ToString() + ".jpg"));
                                //}

                                //string strFileLargePath = Server.MapPath(OldProductLargePath + Imagename);
                                //if (File.Exists(strFileLargePath) && !string.IsNullOrEmpty(Imagename.ToString()))
                                //{
                                //    File.Copy(strFileLargePath, Server.MapPath(ProductLargePath + Sku + "_" + Result.ToString() + ".jpg"));
                                //}

                                //string strFileIconPath = Server.MapPath(OldProductIconPath + Imagename);
                                //if (File.Exists(strFileIconPath) && !string.IsNullOrEmpty(Imagename.ToString()))
                                //{
                                //    File.Copy(strFileIconPath, Server.MapPath(ProductIconPath + Sku + "_" + Result.ToString() + ".jpg"));
                                //}

                                //string strFileMicroPath = Server.MapPath(OldProductMicroPath + Imagename);
                                //if (File.Exists(strFileMicroPath) && !string.IsNullOrEmpty(Imagename.ToString()))
                                //{
                                //    File.Copy(strFileMicroPath, Server.MapPath(ProductMicroPath + Sku + "_" + Result.ToString() + ".jpg"));
                                //}
                                //update
                                //CommonComponent.ExecuteCommonData("Update tb_product set Imagename='" + Sku + "_" + Result.ToString() + ".jpg' Where ProductId=" + Result + " And storeId=" + Convert.ToInt32(Request.QueryString["StoreId"]) + "");

                                string[] ArrFileName = Imagename.Split('.');
                                string filename = ArrFileName[0].ToString();
                                // Copy more Image for Product
                                try
                                {
                                    for (int i = 1; i < 26; i++)
                                    {
                                        if (File.Exists(Server.MapPath(OldProductLargePath + filename + "_" + i.ToString() + ".jpg")) == true)
                                        {
                                            File.Copy(Server.MapPath(OldProductLargePath + filename + "_" + i.ToString() + ".jpg"), Server.MapPath(ProductLargePath + Sku + "_" + Result.ToString() + "_" + i.ToString() + ".jpg"), true);
                                        }
                                        if (File.Exists(Server.MapPath(OldProductMediumPath + filename + "_" + i.ToString() + ".jpg")) == true)
                                        {
                                            File.Copy(Server.MapPath(OldProductMediumPath + filename + "_" + i.ToString() + ".jpg"), Server.MapPath(ProductMediumPath + Sku + "_" + Result.ToString() + "_" + i.ToString() + ".jpg"), true);
                                        }
                                        if (File.Exists(Server.MapPath(OldProductIconPath + filename + "_" + i.ToString() + ".jpg")) == true)
                                        {
                                            File.Copy(Server.MapPath(OldProductIconPath + filename + "_" + i.ToString() + ".jpg"), Server.MapPath(ProductIconPath + Sku + "_" + Result.ToString() + "_" + i.ToString() + ".jpg"), true);
                                        }
                                        if (File.Exists(Server.MapPath(OldProductMicroPath + filename + "_" + i.ToString() + ".jpg")) == true)
                                        {
                                            File.Copy(Server.MapPath(OldProductMicroPath + filename + "_" + i.ToString() + ".jpg"), Server.MapPath(ProductMicroPath + Sku + "_" + Result.ToString() + "_" + i.ToString() + ".jpg"), true);
                                        }
                                    }
                                }
                                catch { }

                                #endregion

                                //update Pdf file
                                #region Save .Pdf File
                                if (!string.IsNullOrEmpty(ProductPdfPath.ToString()))
                                {
                                    string Pdffile = Server.MapPath(ProductPdfPath + Convert.ToString(DsProduct.Tables[0].Rows[0]["PDFName"].ToString()));
                                    string ext = System.IO.Path.GetExtension(Pdffile);
                                    if (ext == ".pdf")
                                    {
                                        if (!Directory.Exists(Server.MapPath(DestinationPdfPath)))
                                            Directory.CreateDirectory(Server.MapPath(DestinationPdfPath));
                                        if (File.Exists(Pdffile))
                                        {
                                            File.Copy(Pdffile, Server.MapPath(DestinationPdfPath + Convert.ToString(Sku + "_" + Result.ToString() + ".pdf")), true);
                                            CommonComponent.ExecuteCommonData("Update tb_product set PDFName='" + Sku + "_" + Result.ToString() + ".pdf' Where ProductId=" + Result + " And storeId=" + Convert.ToInt32(Request.QueryString["StoreId"]) + "");
                                        }
                                    }
                                }
                                #endregion

                                #region save and Upload Video

                                string sourcePath = @Server.MapPath(Sourcetemppath);
                                string targetPath = @Server.MapPath(targettemppath);
                                string srcfileName = Request.QueryString["Id"].ToString() + ".flv";
                                string destfileName = ProductID.ToString() + ".flv";
                                string sourceFile = System.IO.Path.Combine(sourcePath, srcfileName);
                                string destFile = System.IO.Path.Combine(targetPath, destfileName);
                                if (!System.IO.Directory.Exists(targetPath))
                                {
                                    System.IO.Directory.CreateDirectory(targetPath);
                                }
                                if (File.Exists(sourceFile))
                                {
                                    System.IO.File.Copy(sourceFile, destFile, true);
                                }
                                #endregion
                            }
                        }
                        catch { }
                    }
                    Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                }
            }
        }
    }
}