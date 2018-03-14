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
using LumenWorks.Framework.IO.Csv;
using Solution.UI.Web.ADMIN.Settings;

namespace Solution.UI.Web.ADMIN.Products
{
    /// <summary>
    /// Edit Product Page
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public partial class Product : BasePage
    {
        private int StoreID = 1;

        tb_Product tb_Product = null;
        tb_ProductCategory tbProductCategory = null;

        public static string ProductTempPath = string.Empty;
        public static string ProductIconPath = string.Empty;
        public static string ProductMediumPath = string.Empty;
        public static string ProductLargePath = string.Empty;
        public static string ProductMicroPath = string.Empty;
        public string Funname = "";
        static public int productID = 0;
        static int finHeight;
        static int finWidth;
        static Size thumbNailSizeLarge = Size.Empty;
        static Size thumbNailSizeMediam = Size.Empty;
        static Size thumbNailSizeIcon = Size.Empty;
        static Size thumbNailSizeMicro = Size.Empty;
        ConfigurationComponent objConfiguration = new ConfigurationComponent();
        String StrVendorIDs = String.Empty;
        String StrVendorPriority = String.Empty;
        DataSet dsVendorids = new DataSet();
        public static DataSet dsVendor;
        int InventoryTotal = 0;
        DataSet dsWarehouse = new DataSet();
        //int pProductID = 0;
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
        DataTable TempCSV = new DataTable();
        public static DataTable dtAssembler;
        DataColumn dcProductName;
        DataColumn dcProductSKU;
        DataColumn dcQuantity;
        DataColumn dcProductID;
        Int32 Totalsku = 0;
        DataTable dterror = new DataTable();
        DataTable dtwaring = new DataTable();

        double ReplacementAddDays = Convert.ToDouble(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='ReplacementAddDays' and storeid=1"));
        int ReplacementAddDaysValidation = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='ReplacementAddDaysvalidation' and storeid=1"));
        int ReplacementAddDaysValidationETA = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='ReplacementAddDaysValidationETA' and storeid=1"));

        Int32 replnishedsku = 0;
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

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblmessageoption.Text = "";
            if (Request.QueryString["StoreID"] != null && Request.QueryString["ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ID"].ToString()) && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
            {
                // pProductID = Convert.ToInt32(Request.QueryString["ID"]);
                CurrentStoreID = Convert.ToInt32(Request.QueryString["StoreID"]);

            }
            if (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
            {
                CurrentStoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                {

                }
                else
                {
                    txtPricePerInch.Text = AppLogic.AppConfigs("PerInchPrice");
                    txtAdditionalCharge.Text = AppLogic.AppConfigs("AdditionalCharge");
                    txtInchHeaderHeme.Text = AppLogic.AppConfigs("YardHeaderandhem");
                    txtFabric.Text = AppLogic.AppConfigs("FabricInch");
                }
            }
            if (Request.QueryString["CloneID"] != null && !string.IsNullOrEmpty(Request.QueryString["CloneID"].ToString()))
            {
                OldStoreID = Convert.ToInt32(Request.QueryString["CloneID"]);
            }
            BindSize();

            if (!IsPostBack)
            {
                BindRepenishableData();


                GetDetailsofShade();
                BindNavItemCategory();
                if (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
                {
                    ViewState["ImagePathProduct"] = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT Top 1 ConfigValue FROM tb_AppConfig WHERE Configname='ImagePathProduct' and isnull(Deleted,0)=0 and StoreId=" + Request.QueryString["StoreID"].ToString() + ""));
                }
                else
                {
                    ViewState["ImagePathProduct"] = AppLogic.AppConfigs("ImagePathProduct").ToString();
                }
                txtInventory.Attributes.Add("readonly", "true");
                TempTable();
                AssemblerTempTable();
                FillFabricType();
                FillProductfeature();
                FillFabricVendor();
                if (Request.QueryString["ID"] != null)
                {
                    DisplayShadeCalc();
                }
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
                        Response.Redirect("Product.aspx?Storeid=" + Convert.ToInt32(Request.QueryString["StoreID"]) + "&Mode=Edit&ID=" + NewProductID.ToString() + "");
                    }
                }
                hdnTabid.Value = "5";
            }

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
                        AppConfig.StoreID = 1;
                }
                if (AppConfig.StoreID < 1)
                    AppConfig.StoreID = 1;

            }

            if (!IsPostBack)
            {
                if (Request.QueryString["StoreID"] != null)
                {
                    if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                    {
                        skuedit.Visible = true;


                    }
                    else
                    {
                        //StrUPC = Convert.ToString(CommonComponent.GetScalarCommonData("select TOP 1 UPC from tb_upcmaster where sku is null ORDER BY NEWID() "));
                        //if (StrUPC != null && StrUPC.Length > 0)
                        //{
                        //    txtUPC.Text = StrUPC;
                        //}
                    }
                }
            }
            if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
            {
                if (hdnskuedit.Value.ToString() == "1")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "skureadonly", "$('#ContentPlaceHolder1_txtSKU').attr('readonly','false');", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "skureadonly", "$('#ContentPlaceHolder1_txtSKU').attr('readonly','true');", true);
                }
            }

            if (!IsPostBack)
            {
                BindVendorSKU();
                BindProductSKU();
                BindRomanShadeYardage();
                BindSearchDropdowns();
                ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
                ProductIconPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Icon/");
                ProductMediumPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Medium/");
                ProductLargePath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Large/");
                ProductMicroPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Micro/");
                imgbtnAlt1.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                imgbtnAlt2.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                imgbtnAlt3.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                imgbtnAlt4.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                imgbtnAlt5.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnImportReplenishment.Attributes.Add("style", "background: url(/REPLENISHMENTMANAGEMENT/Images/import-replenishment-file.gif)no-repeat transparent;border:none;cursor:pointer;width:200px;height: 23px;");
                imgbtnAlt1del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                imgbtnAlt2del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                imgbtnAlt3del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                imgbtnAlt4del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                imgbtnAlt5del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                if (Request.QueryString["StoreID"] != null)
                {
                    StoreID = CurrentStoreID;
                    BindAllCombo(CurrentStoreID);
                    BindCategory(CurrentStoreID);
                    if (StoreID == 3)
                    {
                        Isspecialtd.Visible = true;
                    }
                    else
                        Isspecialtd.Visible = false;
                    if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null && Request.QueryString["CloneID"] == null)
                    {

                        string Sourcetemppath = AppLogic.AppConfigs("VideoPath").ToString();
                        //string checkfile = Sourcetemppath + Request.QueryString["ID"].ToString() + ".flv";
                        //if (File.Exists(Server.MapPath(checkfile)))
                        //{

                        //    ViewState["VideoName"] = Request.QueryString["ID"].ToString();
                        //    lblVideoName.Text = Request.QueryString["ID"].ToString() + ".flv";
                        //    trvideodelete.Visible = false;
                        //    btndeleteVideo.Visible = true;
                        //    lblVideoName.Visible = true;

                        //    string targettemppath = AppLogic.AppConfigs("VideoTempPath").ToString();
                        //    string sourcePath = @Server.MapPath(Sourcetemppath);
                        //    string targetPath = @Server.MapPath(targettemppath);
                        //    string srcfileName = Request.QueryString["ID"].ToString() + ".flv";
                        //    string destfileName = Request.QueryString["ID"].ToString() + ".flv";

                        //    string sourceFile = System.IO.Path.Combine(sourcePath, srcfileName);
                        //    string destFile = System.IO.Path.Combine(targetPath, destfileName);

                        //    //if (!System.IO.Directory.Exists(targetPath))
                        //    //{
                        //    //    System.IO.Directory.CreateDirectory(targetPath);
                        //    //}
                        //    //if (File.Exists(sourceFile))
                        //    //    System.IO.File.Copy(sourceFile, destFile, true);

                        //}

                        string checkfile = Sourcetemppath + Request.QueryString["ID"].ToString() + ".mp4";
                        if (File.Exists(Server.MapPath(checkfile)))
                        {

                            ViewState["VideoName"] = Request.QueryString["ID"].ToString();
                            lblVideoName.Text = Request.QueryString["ID"].ToString() + ".mp4";
                            trvideodelete.Visible = false;
                            btndeleteVideo.Visible = true;
                            lblVideoName.Visible = true;

                            string targettemppath = AppLogic.AppConfigs("VideoTempPath").ToString();
                            string sourcePath = @Server.MapPath(Sourcetemppath);
                            string targetPath = @Server.MapPath(targettemppath);
                            string srcfileName = Request.QueryString["ID"].ToString() + ".mp4";
                            string destfileName = Request.QueryString["ID"].ToString() + ".mp4";

                            string sourceFile = System.IO.Path.Combine(sourcePath, srcfileName);
                            string destFile = System.IO.Path.Combine(targetPath, destfileName);

                            //if (!System.IO.Directory.Exists(targetPath))
                            //{
                            //    System.IO.Directory.CreateDirectory(targetPath);
                            //}
                            //if (File.Exists(sourceFile))
                            //    System.IO.File.Copy(sourceFile, destFile, true);

                        }
                        checkfile = Sourcetemppath + Request.QueryString["ID"].ToString() + "_1.mp4";
                        if (File.Exists(Server.MapPath(checkfile)))
                        {

                            ViewState["VideoName1"] = Request.QueryString["ID"].ToString() + "_1";
                            lblVideoName1.Text = Request.QueryString["ID"].ToString() + "_1.mp4";
                            trvideodelete1.Visible = false;
                            btndeleteVideo1.Visible = true;
                            lblVideoName1.Visible = true;

                            string targettemppath = AppLogic.AppConfigs("VideoTempPath").ToString();
                            string sourcePath = @Server.MapPath(Sourcetemppath);
                            string targetPath = @Server.MapPath(targettemppath);
                            string srcfileName = Request.QueryString["ID"].ToString() + "_1.mp4";
                            string destfileName = Request.QueryString["ID"].ToString() + "_1.mp4";

                            string sourceFile = System.IO.Path.Combine(sourcePath, srcfileName);
                            string destFile = System.IO.Path.Combine(targetPath, destfileName);

                            //if (!System.IO.Directory.Exists(targetPath))
                            //{
                            //    System.IO.Directory.CreateDirectory(targetPath);
                            //}
                            //if (File.Exists(sourceFile))
                            //    System.IO.File.Copy(sourceFile, destFile, true);

                        }

                        checkfile = Sourcetemppath + Request.QueryString["ID"].ToString() + "_2.mp4";
                        if (File.Exists(Server.MapPath(checkfile)))
                        {

                            ViewState["VideoName2"] = Request.QueryString["ID"].ToString() + "_2";
                            lblVideoName2.Text = Request.QueryString["ID"].ToString() + "_2.mp4";
                            trvideodelete2.Visible = false;
                            btndeleteVideo2.Visible = true;
                            lblVideoName2.Visible = true;

                            string targettemppath = AppLogic.AppConfigs("VideoTempPath").ToString();
                            string sourcePath = @Server.MapPath(Sourcetemppath);
                            string targetPath = @Server.MapPath(targettemppath);
                            string srcfileName = Request.QueryString["ID"].ToString() + "_2.mp4";
                            string destfileName = Request.QueryString["ID"].ToString() + "_2.mp4";

                            string sourceFile = System.IO.Path.Combine(sourcePath, srcfileName);
                            string destFile = System.IO.Path.Combine(targetPath, destfileName);

                            //if (!System.IO.Directory.Exists(targetPath))
                            //{
                            //    System.IO.Directory.CreateDirectory(targetPath);
                            //}
                            //if (File.Exists(sourceFile))
                            //    System.IO.File.Copy(sourceFile, destFile, true);

                        }

                        tduploadPdf.Visible = true;
                        trProductOption.Attributes.Add("style", "display:''");
                        trProductOption.Attributes.Add("style", "display:block");
                        divDescription.Visible = true;

                        BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                        BindProductStylePrice("edit");
                        BindProductOptionsPrice("edit");

                        tduploadMoreImages.Visible = false;
                        //tduploadMoreImages.Visible = true;

                        btnUploadvideo.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                        btndeleteVideo.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delete.gif";



                        btnApprove.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/approveitems.png";

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

                        // Product Variant Page

                        trProductVariant.Visible = true;
                        // ifrmProductVariant.Attributes.Add("onload", "iframeAutoheight(this);");
                        ifrmProductVariant.Attributes.Add("src", "ProductVariant.aspx?StoreId=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ID=" + Convert.ToString(Request.QueryString["ID"]) + "&HType=no");
                        Manufacturelink.HRef = "Manufacturer.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "";
                        QuantityDiscountLink.HRef = "../Promotions/QuantityDiscount.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "";
                        //ColorLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchColor=Color";
                        ColorLink.HRef = "AddProductColor.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        if (Request.QueryString["SearchColor"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        //PatternLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchPattern=Pattern";
                        PatternLink.HRef = "AddProductPattern.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);

                        if (Request.QueryString["SearchPattern"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        //FabricLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchFabric=Fabric";
                        if (Request.QueryString["SearchFabric"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        FabricLink.HRef = "AddProductFabric.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        HeaderLink.HRef = "AddProductHeader.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        roomLink.HRef = "AddProductRoom.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        StyleLink.HRef = "AddProductFeature.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);

                        //StyleLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchFeature=Feature";
                        if (Request.QueryString["SearchStyle"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }
                        newStyleLink.HRef = "AddProductStyle.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        //newStyleLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchStyle=Style"; newStyleLink.HRef = "AddProductStyle.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);                       
                        if (Request.QueryString["FeatureStyle"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        FabricLink.HRef = "AddProductFabric.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);

                        HeaderLink.HRef = "AddProductHeader.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        //HeaderLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchHeader=Header";
                        if (Request.QueryString["SearchHeader"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        CustomCalculatorLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchCustomCalculator=Style";
                        if (Request.QueryString["SearchCustomCalculator"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(8);", true);
                        }
                        OptionCalculatorLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchOptionCalculator=Option";
                        if (Request.QueryString["SearchOptionCalculator"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(8);", true);
                        }


                        FabricTypeLink.HRef = "ProductFabricSettings.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchFabricType=FabricType";
                        if (Request.QueryString["SearchFabricType"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(5);", true);
                        }

                        FabricCodeLink.HRef = "ProductFabricSettings.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchFabricCode=FabricCode";
                        if (Request.QueryString["SearchFabricCode"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(5);", true);
                        }

                        SecondaryColorLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&SearchSecondaryColorStyle=color";
                        if (Request.QueryString["SearchSecondaryColorStyle"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        aromanadd.HRef = "/Admin/Products/ProductRomanShadeYardage.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"].ToString()) + "&ProductID=" + Convert.ToString(Request.QueryString["ID"]) + "&romanproductType=1";

                    }

                    else
                    {

                        Manufacturelink.HRef = "Manufacturer.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "";
                        QuantityDiscountLink.HRef = "../PromotionsQuantityDiscount.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "";
                        //ColorLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchColor=Color";
                        ColorLink.HRef = "AddProductColor.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        if (Request.QueryString["SearchColor"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }
                        PatternLink.HRef = "AddProductPattern.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchPattern=Pattern";
                        //PatternLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchPattern=Pattern";
                        //PatternLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchFabric=Fabric";

                        if (Request.QueryString["SearchPattern"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        if (Request.QueryString["SearchFabric"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        //newStyleLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchStyle=Style";
                        newStyleLink.HRef = "AddProductStyle.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);

                        HeaderLink.HRef = "AddProductHeader.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        roomLink.HRef = "AddProductRoom.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);

                        if (Request.QueryString["SearchStyle"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        //StyleLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchFeature=Feature";
                        StyleLink.HRef = "AddProductFeature.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);
                        if (Request.QueryString["SearchFeature"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        //HeaderLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchHeader=Header";
                        HeaderLink.HRef = "AddProductHeader.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]);

                        if (Request.QueryString["SearchHeader"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        CustomCalculatorLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchCustomCalculator=CustomStyle";
                        if (Request.QueryString["SearchCustomCalculator"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(8);", true);
                        }
                        OptionCalculatorLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchOptionCalculator=Options";
                        if (Request.QueryString["SearchOptionCalculator"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(8);", true);
                        }
                        FabricTypeLink.HRef = "ProductFabricSettings.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchFabricType=FabricType";
                        if (Request.QueryString["SearchFabricType"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(5);", true);
                        }

                        FabricCodeLink.HRef = "ProductFabricSettings.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchFabricCode=FabricCode";
                        if (Request.QueryString["SearchFabricCode"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(5);", true);
                        }

                        SecondaryColorLink.HRef = "SearchProductType.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"]) + "&SearchSecondaryColorStyle=color";
                        if (Request.QueryString["SearchSecondaryColorStyle"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselcted", "Tabdisplay(7);", true);
                        }

                        aromanadd.HRef = "/Admin/Products/ProductRomanShadeYardage.aspx?ProductStoreID=" + Convert.ToString(Request.QueryString["StoreID"].ToString()) + "&romanproductType=1";
                        trProductVariant.Visible = false;

                        if (Request.QueryString["CloneID"] != null)
                        {
                            BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["CloneID"]));
                            GetWarehouseList(Convert.ToInt32(Request.QueryString["ID"]), 2);
                            trProductOption.Attributes.Add("style", "display:none;");
                            msgid.Visible = true;
                            if (Request.QueryString["ID"] != null && Request.QueryString["CloneID"] != null)
                            {
                                BindProductStylePrice("edit");
                                BindProductOptionsPrice("edit");
                            }
                        }
                        else
                        {
                            trProductOption.Attributes.Add("style", "display:none;");
                            ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";

                            ImgAlt1.Src = ProductMediumPath + "image_not_available.jpg";
                            ImgAlt2.Src = ProductMediumPath + "image_not_available.jpg";
                            ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg";
                            ImgAlt4.Src = ProductMediumPath + "image_not_available.jpg";
                            ImgAlt5.Src = ProductMediumPath + "image_not_available.jpg";
                            //mode 1: to bind warehouse grid without inventory 
                            GetWarehouseList(0, 1);
                            BindProductStylePrice("insert");
                            BindProductOptionsPrice("insert");
                            msgid.Visible = false;
                        }

                    }
                }

                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnSaveandexit.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save-exit.gif";

                btnSaveStylePrice.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnSaveOptionPrice.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnSaveSecondaryColor.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                btnUploadvideo.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btndeleteVideo.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delete.gif";
                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
                btnUploadPdfFile.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnRepleshmentsave1.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                if (ViewState["VideoName"] != null)
                {
                    trvideodelete.Visible = true;
                    btndeleteVideo.Visible = true;
                    lblVideoName.Visible = true;
                }
                if (ViewState["VideoName1"] != null)
                {
                    trvideodelete1.Visible = true;
                    btndeleteVideo1.Visible = true;
                    lblVideoName1.Visible = true;
                }
                if (ViewState["VideoName2"] != null)
                {
                    trvideodelete2.Visible = true;
                    btndeleteVideo2.Visible = true;
                    lblVideoName2.Visible = true;
                }
            }
            else
            {
                //if (chkIsHamming.Checked == true)
                //{
                //    tdHamminglbl.Attributes.Add("style", "display:''");
                //    tdHammingQty.Attributes.Add("style", "display:''");
                //}
                //else
                //{
                //    tdHamminglbl.Attributes.Add("style", "display:none");
                //    tdHammingQty.Attributes.Add("style", "display:none");
                //}

                if (chkismadetoswatch.Checked == true)
                {
                    divProductSwatch.Attributes.Add("style", "display:block");
                }
                else
                {
                    divProductSwatch.Attributes.Add("style", "display:none");
                    txtProductSwatchId.Text = "";
                }
                if (chkIsProperty.Checked)
                {
                    divproperty.Attributes.Add("style", "display = none");
                }
                else { divproperty.Attributes.Add("style", "display = ''"); }

                if (ddlItemType.SelectedValue.ToString().ToLower().Trim() == "roman")
                {
                    chkIsRoman.Checked = true;
                    //divRomanShadeYard.Attributes.Add("style", "display:''");
                    divRomanShadeYard.Attributes.Add("style", "display:none");
                }
                else
                {
                    chkIsRoman.Checked = false;
                    divRomanShadeYard.Attributes.Add("style", "display:none");
                }

                if (ddlItemType.SelectedValue.ToString().ToLower().Trim() == "swatch")
                {
                    chkForAll.Visible = true;
                    pchkForAll.Visible = true;
                }
                else
                {
                    chkForAll.Visible = false;
                    pchkForAll.Visible = false;
                }
                //if (chkIsRoman.Checked)
                //{
                //    divRomanShadeYard.Attributes.Add("style", "display = none");
                //}
                //else { divRomanShadeYard.Attributes.Add("style", "display = ''"); }

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

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "tbselctedGeneral", "Tabdisplay(" + hdnTabid.Value.ToString() + ");", true);
            }
            if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "lidisabled", "document.getElementById('liMenu10').style.display = '';", true);
            }
            if (Request.QueryString["StoreId"] != null && Request.QueryString["StoreId"].ToString() != "1")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "lidisabledTab", "document.getElementById('liMenu6').style.display = 'none';document.getElementById('liMenu7').style.display = 'none';document.getElementById('liMenu8').style.display = 'none';document.getElementById('liMenu9').style.display = 'none';document.getElementById('liMenu10').style.display = 'none';", true);
            }
        }

        private void FillFabricVendor()
        {
            VendorDAC objVendorDAC = new VendorDAC();
            DataSet dsdropshipsku = objVendorDAC.GetVendorList(0);
            chkFabricvendor.Items.Clear();
            if (dsdropshipsku != null && dsdropshipsku.Tables.Count > 0 && dsdropshipsku.Tables[0].Rows.Count > 0)
            {
                chkFabricvendor.DataSource = dsdropshipsku.Tables[0];
                chkFabricvendor.DataTextField = "Name";
                chkFabricvendor.DataValueField = "VendorID";
                chkFabricvendor.DataBind();

                //ddlFabricvendor.DataSource = dsdropshipsku.Tables[0];
                //ddlFabricvendor.DataTextField = "Name";
                //ddlFabricvendor.DataValueField = "VendorID";
                //ddlFabricvendor.DataBind();
            }
            else
            {
                chkFabricvendor.DataSource = null;
                chkFabricvendor.DataBind();
            }
            ddlFabricvendor.Items.Insert(0, new ListItem("Select Fabric Vendor", "0"));
        }
        /// <summary>
        /// Bind Roman Shade Yardage
        /// </summary>
        private void BindRomanShadeYardage()
        {
            DataSet DsRomanShade = new DataSet();
            DsRomanShade = CommonComponent.GetCommonDataSet("Select RomanShadeId,ISNULL(ShadeName,'') as ShadeName from tb_ProductRomanShadeYardage where ISNULL(Active,0)=1");
            if (DsRomanShade != null && DsRomanShade.Tables.Count > 0 && DsRomanShade.Tables[0].Rows.Count > 0)
            {
                ddlRomanShadeYardage.DataSource = DsRomanShade;
                ddlRomanShadeYardage.DataValueField = "RomanShadeId";
                ddlRomanShadeYardage.DataTextField = "ShadeName";
                ddlRomanShadeYardage.DataBind();
            }
            else
            {
                ddlRomanShadeYardage.DataSource = null;
                ddlRomanShadeYardage.DataBind();
            }
            ddlRomanShadeYardage.Items.Insert(0, new ListItem("Select Roman Shade", "0"));
        }

        /// <summary>
        /// Bind Fabric Type
        /// </summary>
        private void FillFabricType()
        {
            DataSet DsFabircType = new DataSet();
            ProductComponent objProduct = new ProductComponent();
            //     DsFabircType = CommonComponent.GetCommonDataSet("SELECT FabricTypeID,FabricTypename FROM dbo.tb_ProductFabricType WHERE ISNULL(Active,0)=1 AND ISNULL(FabricTypename,'')<>'' ORDER BY ISNULL(DisplayOrder,999) ASC"); //objProduct.GetProductFabricDetails(0, 1);
            DsFabircType = CommonComponent.GetCommonDataSet("SELECT FabricTypeID,FabricTypename FROM dbo.tb_ProductFabricType WHERE  ISNULL(FabricTypename,'')<>'' ORDER BY ISNULL(DisplayOrder,999) ASC"); //objProduct.GetProductFabricDetails(0, 1);
            if (DsFabircType != null && DsFabircType.Tables.Count > 0 && DsFabircType.Tables[0].Rows.Count > 0)
            {
                ddlFabricType.DataSource = DsFabircType;
                ddlFabricType.DataValueField = "FabricTypeID";
                ddlFabricType.DataTextField = "FabricTypename";
                ddlFabricType.DataBind();
            }
            else
            {
                ddlFabricType.DataSource = null;
                ddlFabricType.DataBind();
            }
            ddlFabricType.Items.Insert(0, new ListItem("Select Fabric Type", "0"));
            ddlFabricType_SelectedIndexChanged(null, null);
        }

        protected void ddlFabricType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlFabricCode.Items.Clear();
            ProductComponent objProduct = new ProductComponent();

            DataSet DsFabricCode = new DataSet();
            DsFabricCode = objProduct.GetProductFabricDetails(Convert.ToInt32(ddlFabricType.SelectedValue), 2);
            if (DsFabricCode != null && DsFabricCode.Tables.Count > 0 && DsFabricCode.Tables[0].Rows.Count > 0)
            {
                ddlFabricCode.DataSource = DsFabricCode;
                ddlFabricCode.DataValueField = "FabricCodeId";
                ddlFabricCode.DataTextField = "Code";
                ddlFabricCode.DataBind();
            }
            else
            {
                ddlFabricCode.DataSource = null;
                ddlFabricCode.DataBind();
            }
            ddlFabricCode.Items.Insert(0, new ListItem("Select Fabric Code", "0"));
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

        #region Bind All Dropdowns

        /// <summary>
        /// Binds all Drop down
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        private void BindAllCombo(Int32 StoreID)
        {
            BindTaxClass(StoreID);
            BindProductType(StoreID);
            BindProductTypeDelivery(StoreID);
            BindManufacture(StoreID);
            BindQuantityDiscount(StoreID);
        }

        /// <summary>
        /// Bind Tax Class Dropdown
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        protected void BindTaxClass(Int32 StoreID)
        {
            DataSet dsTaxClass = new DataSet();
            dsTaxClass = TaxClassComponent.GetTaxClassByStoreID(StoreID);
            if (dsTaxClass != null && dsTaxClass.Tables.Count > 0 && dsTaxClass.Tables[0].Rows.Count > 0)
            {
                ddlTaxClass.DataSource = dsTaxClass;
                ddlTaxClass.DataTextField = "TaxName";
                ddlTaxClass.DataValueField = "TaxClassID";
            }
            else
            {
                ddlTaxClass.DataSource = null;
            }
            ddlTaxClass.DataBind();
            ddlTaxClass.Items.Insert(0, new ListItem("Select Tax Class", "0"));
            if (dsTaxClass != null && dsTaxClass.Tables.Count > 0 && dsTaxClass.Tables[0].Rows.Count > 0)
                ddlTaxClass.SelectedIndex = 1;
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
                ddlProductTypeDelivery.DataBind();
                ddlProductTypeDelivery.Items.Insert(0, new ListItem("Select Product Type Delivery", "0"));

                ddlProductTypeDelivery.Items.FindByText("Vendor").Selected = true;
                ddlProductTypeDelivery_SelectedIndexChanged(null, null);
                if (ddlvendor.Items.Count > 0)
                {
                    ddlvendor.SelectedIndex = 1;
                }
            }
            else
            {
                ddlProductTypeDelivery.DataSource = null;
                ddlProductTypeDelivery.DataBind();
                ddlProductTypeDelivery.Items.Insert(0, new ListItem("Select Product Type Delivery", "0"));
            }


            if (ddlProductTypeDelivery.SelectedIndex == 0)
            {
                grdAssembler.Visible = false;
                grdDropShip.Visible = false;
            }

            // if (dsProductTypeDelivery != null && dsProductTypeDelivery.Tables.Count > 0 && dsProductTypeDelivery.Tables[0].Rows.Count > 0)
            // ddlProductTypeDelivery.SelectedIndex = 1;
        }

        /// <summary>
        /// Bind Quantity Discount Dropdown
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        protected void BindQuantityDiscount(Int32 StoreID)
        {
            DataSet dsQtyDis = new DataSet();
            dsQtyDis = DiscountComponent.GetQuantityDiscountByStoreID(StoreID);
            if (dsQtyDis != null && dsQtyDis.Tables.Count > 0 && dsQtyDis.Tables[0].Rows.Count > 0)
            {
                ddlQuantityDiscount.DataSource = dsQtyDis;
                ddlQuantityDiscount.DataTextField = "Name";
                ddlQuantityDiscount.DataValueField = "QuantityDiscountID";
            }
            else
            {
                ddlQuantityDiscount.DataSource = null;
            }
            ddlQuantityDiscount.DataBind();
            ddlQuantityDiscount.Items.Insert(0, new ListItem("Select Quantity Discount", "0"));
        }

        #endregion

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
        /// <param name="StoreID">int StoreID</param>
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
        /// Save button Click event for Add or Update Product
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {


            if (Request.QueryString["StoreID"] != null)
            {
                if (chkIsFreeShipping.Checked || chkNewArrival.Checked || chkFeatured.Checked || chkIsBestSeller.Checked)
                {
                    if (txtNewArrivalFromDate.Text.ToString() != "" && txtNewArrivalToDate.Text.ToString() != "")
                    {
                        if (Convert.ToDateTime(txtNewArrivalToDate.Text.ToString()) >= Convert.ToDateTime(txtNewArrivalFromDate.Text.ToString()))
                        {

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid new arrival date.', 'Message');", true);
                            return;
                        }
                    }
                    else if (txtIsFeaturedFromDate.Text.ToString() != "" && txtIsFeaturedToDate.Text.ToString() != "")
                    {
                        if (Convert.ToDateTime(txtIsFeaturedToDate.Text.ToString()) >= Convert.ToDateTime(txtIsFeaturedFromDate.Text.ToString()))
                        {

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid featured date.', 'Message');", true);
                            return;
                        }
                    }
                    else if (txtBestSellerFromDate.Text.ToString() != "" && txtBestSellerToDate.Text.ToString() != "")
                    {
                        if (Convert.ToDateTime(txtBestSellerToDate.Text.ToString()) >= Convert.ToDateTime(txtBestSellerFromDate.Text.ToString()))
                        {

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid Best Seller date.', 'Message');", true);
                            return;
                        }
                    }
                    else if (txtFreeShippingFromDate.Text.ToString() != "" && txtFreeShippingToDate.Text.ToString() != "")
                    {
                        if (Convert.ToDateTime(txtFreeShippingToDate.Text.ToString()) >= Convert.ToDateTime(txtFreeShippingFromDate.Text.ToString()))
                        {

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid Free Shipping date.', 'Message');", true);
                            return;
                        }
                    }
                    else
                    {
                        if (txtNewArrivalFromDate.Text.ToString() == "" && txtNewArrivalToDate.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid new arrival from date.', 'Message');", true);
                            return;
                        }
                        else if (txtNewArrivalToDate.Text.ToString() == "" && txtNewArrivalFromDate.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid new arrival to  date.', 'Message');", true);
                            return;
                        }


                        if (txtIsFeaturedFromDate.Text.ToString() == "" && txtIsFeaturedToDate.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid featured from date.', 'Message');", true);
                            return;
                        }
                        else if (txtIsFeaturedToDate.Text.ToString() == "" && txtIsFeaturedFromDate.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid featured to  date.', 'Message');", true);
                            return;
                        }

                        if (txtBestSellerFromDate.Text.ToString() == "" && txtBestSellerToDate.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid best seller from date.', 'Message');", true);
                            return;
                        }
                        else if (txtBestSellerToDate.Text.ToString() == "" && txtBestSellerFromDate.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid best seller to  date.', 'Message');", true);
                            return;
                        }

                        if (txtFreeShippingFromDate.Text.ToString() == "" && txtFreeShippingToDate.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid free shipping from date.', 'Message');", true);
                            return;
                        }
                        else if (txtFreeShippingToDate.Text.ToString() == "" && txtFreeShippingFromDate.Text.ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid free shipping to  date.', 'Message');", true);
                            return;
                        }

                    }
                }

                if (chkisordermade.Checked || chkismadetomeasure.Checked)
                {

                    if (ddlFabricType.SelectedIndex > 0)
                    {
                        Int32 fabriccatid = Convert.ToInt32(ddlFabricType.SelectedValue.ToString());
                        bool flag = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT isnull(active,0) FROM dbo.tb_ProductFabricType WHERE  FabricTypeID=" + fabriccatid + ""));
                        if (flag == false)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Active Fabric Category Or Remove Made To Order/Made To Measure.', 'Message');", true);
                            return;
                        }
                        else
                        {
                            if (ddlFabricCode.SelectedIndex > 0)
                            {
                                Int32 fabrictypeid = Convert.ToInt32(ddlFabricCode.SelectedValue.ToString());
                                flag = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT isnull(active,0) FROM dbo.tb_ProductFabricCode WHERE  FabricCodeId=" + fabrictypeid + ""));
                                if (flag == false)
                                {
                                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Active Fabric Code Or Remove Made To Order/Made To Measure.', 'Message');", true);
                                    return;
                                }
                            }

                        }

                    }
                }



                if (chkismadetomeasure.Checked && ddlItemType.SelectedItem.Text.ToString().ToLower() != "fabric" && ddlItemType.SelectedItem.Text.ToString().ToLower() != "roman")
                {
                    if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "")
                    {
                        Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(ProductID) from tb_ProductVariantValue where SKU like '%-cus%' and ISNULL(VarActive,0)=1 and ProductID=" + Request.QueryString["ID"].ToString() + ""));
                        if (Count <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Custom Size Product Option not Found,Please insert Custom Option First.', 'Message');", true);
                            return;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Custom Size Product Option not Found,Please insert Custom Option First.', 'Message');", true);
                        return;
                    }
                }


                if (chkIsShowBuy1Get1.Checked)
                {
                    try
                    {
                        if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "")
                        {
                            Int32 Vpid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select top 1 tb_ProductVariantValue.Productid from tb_ProductVariantValue inner join tb_product on tb_product.productid=tb_ProductVariantValue.productid Where isnull(VarActive,0)=1 AND  (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and ISNULL(Buy1Get1,0)=1 and tb_ProductVariantValue.ProductId=" + Request.QueryString["ID"].ToString() + ""));
                            if (Vpid > 0)
                            {

                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Unselect Is Show Buy1Get1 Option', 'Message');", true);
                                return;

                            }
                        }
                    }
                    catch { }

                }


                if (chkPublished.Checked == false && !String.IsNullOrEmpty(txtpageredirecturl.Text.Trim()))
                {
                    txtpageredirecturl.Text = txtpageredirecturl.Text.Replace("/", "").Trim();
                    string OldUrl = "/" + txtpageredirecturl.Text.Trim();
                    Int32 count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(RedirectID) from tb_PageRedirect where StoreID=1 and OldUrl='" + OldUrl + "'"));
                    if (count > 0)
                    {


                        string sename = OldUrl.Replace(".html", "").Replace("/", "").Trim();
                        string productsenam = sename + ".html";
                        Int32 pcount = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(ProductID) from tb_Product where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and producturl='" + productsenam.ToLower() + "'"));
                        if (pcount <= 0)
                        {
                            pcount = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(CategoryID) from tb_Category where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and SEName='" + sename.ToString().ToLower() + "'"));
                            if (pcount <= 0)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "redirecturl('ContentPlaceHolder1_chkPublished','ContentPlaceHolder1_chkpublishedadmin');jAlert('Redirect URL already Exist,Please enter another Redirect URL', 'Message');", true);
                                return;
                            }
                        }





                    }

                    string sename1 = OldUrl.Replace(".html", "").Replace("/", "").Trim();
                    string productsenam1 = sename1 + ".html";
                    Int32 pcount1 = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(ProductID) from tb_Product where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and producturl='" + productsenam1.ToLower() + "'"));
                    if (pcount1 <= 0)
                    {
                        pcount1 = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(CategoryID) from tb_Category where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and SEName='" + sename1.ToString().ToLower() + "'"));
                        if (pcount1 <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "redirecturl('ContentPlaceHolder1_chkPublished','ContentPlaceHolder1_chkpublishedadmin');jAlert('Invalid Redirect URL,Please enter Active Product Or Category URL', 'Message');", true);
                            return;
                        }
                    }
                }

                //if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "edit")
                //{
                //    int ChkClone = 0;

                //    ChkClone = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) TotCnt from tb_Product where StoreID=" + Request.QueryString["StoreID"].ToString() + " and SKU " +
                //                   " in(Select SKU from tb_Product where ProductID =" + Request.QueryString["ID"].ToString() + ") and isnull(deleted,0)=0"));
                //    if (ChkClone == 0)
                //    {
                //        InsertCloneProduct(Convert.ToInt32(Request.QueryString["StoreID"]));
                //    }
                //    else
                //    {
                //        UpdateProduct(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                //    }
                //}
                //else
                //{
                //    InsertProduct(Convert.ToInt32(Request.QueryString["StoreID"]));
                //}

                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().Trim().ToLower() == "edit")
                {
                    btnSaveStylePrice_Click(null, null);
                    btnSaveOptionPrice_Click(null, null);
                    try
                    {
                        if (!String.IsNullOrEmpty(txtSKU.Text) && !String.IsNullOrEmpty(txtUPC.Text))
                        {

                            String WareHouseLocation = "";
                            foreach (GridViewRow row in grdWarehouse.Rows)
                            {

                                RadioButton rdowarehouse = (RadioButton)row.FindControl("rdowarehouse");
                                Label lblWarehouse = (Label)row.FindControl("lblWarehouseID");
                                TextBox txtInventory = (row.FindControl("txtInventory") as TextBox);


                                if (rdowarehouse.Checked)
                                {

                                    WareHouseLocation = lblWarehouse.Text;
                                    String WareHouseCode = Convert.ToString(CommonComponent.GetScalarCommonData("Select Code from tb_WareHouse where WareHouseID=" + Convert.ToInt32(WareHouseLocation.ToString()) + ""));
                                    CommonComponent.ExecuteCommonData("exec usp_Warehouse_CheckWarehouse '" + txtSKU.Text.ToString().Trim() + "','" + txtUPC.Text.ToString().Trim() + "','" + WareHouseCode.ToString().Trim() + "'");

                                }
                            }

                        }
                    }
                    catch { }
                    UpdateProduct(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));


                }
                else
                {
                    int ChkClone = 1;
                    if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString().Trim().ToLower() == "")
                    {
                        ChkClone = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) TotCnt from tb_Product where StoreID=" + Request.QueryString["StoreID"].ToString() + " and SKU " +
                                       " in (Select SKU from tb_Product where ProductID =" + Request.QueryString["ID"].ToString() + ") and isnull(deleted,0)=0"));
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(txtSKU.Text) && !String.IsNullOrEmpty(txtUPC.Text))
                        {

                            String WareHouseLocation = "";
                            foreach (GridViewRow row in grdWarehouse.Rows)
                            {

                                RadioButton rdowarehouse = (RadioButton)row.FindControl("rdowarehouse");
                                Label lblWarehouse = (Label)row.FindControl("lblWarehouseID");
                                TextBox txtInventory = (row.FindControl("txtInventory") as TextBox);


                                if (rdowarehouse.Checked)
                                {

                                    WareHouseLocation = lblWarehouse.Text;
                                    String WareHouseCode = Convert.ToString(CommonComponent.GetScalarCommonData("Select Code from tb_WareHouse where WareHouseID=" + Convert.ToInt32(WareHouseLocation.ToString()) + ""));
                                    CommonComponent.ExecuteCommonData("exec usp_Warehouse_CheckWarehouse '" + txtSKU.Text.ToString().Trim() + "','" + txtUPC.Text.ToString().Trim() + "','" + WareHouseCode.ToString().Trim() + "'");

                                }
                            }

                        }
                    }
                    catch { }

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
        }

        /// <summary>
        /// Cancel button Click event for Redirect to Product List Page
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ImgLarge.Src.Trim() != "" && ImgLarge.Src.Trim().ToLower().IndexOf("/product/temp/") > -1)
                {
                    FileInfo flinfo = new FileInfo(Server.MapPath(ImgLarge.Src.Trim()));
                    if (flinfo.Exists)
                    {
                        flinfo.Delete();
                    }
                }
            }
            catch { }
            ViewState["DelImage"] = null;
            ViewState["File"] = null;
            ViewState["PdfFile"] = null;
            Response.Redirect("ProductList.aspx?cancel=1&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
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

            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
                string productid = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(YoutubeVideoURL,'') as  YoutubeVideoURL from tb_product where productid=" + ID + ""));
                if (!String.IsNullOrEmpty(productid))
                    txtyoutubevideo.Text = productid;
                txtProductName.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Name"]);
                txtSKU.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SKU"]);
                txtUPC.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["UPC"]);
                //if (txtUPC.Text != null && txtUPC.Text != "")
                //    txtUPC.Attributes.Add("ReadOnly", "true");


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

                Int32 IsDataVerify = 0;
                btnApprove.Visible = false;
                if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
                {
                    if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsDataVerify"].ToString()))
                    {
                        Int32.TryParse(DsProduct.Tables[0].Rows[0]["IsDataVerify"].ToString(), out IsDataVerify);
                        if (IsDataVerify == 0)
                        {
                            btnApprove.Visible = true;
                        }
                    }
                }

                if (Request.QueryString["CloneID"] == null)
                {

                    ddlProductType.SelectedValue = DsProduct.Tables[0].Rows[0]["ProductTypeID"].ToString();

                    ddlManufacture.SelectedValue = DsProduct.Tables[0].Rows[0]["ManufactureID"].ToString();
                }
                txtWidth.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["width"].ToString());
                txtHeight.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["height"].ToString());
                txtLength.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["length"].ToString());
                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["weight"].ToString()))
                {
                    txtWeight.Text = "0";
                }
                else
                {
                    txtWeight.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["weight"].ToString());
                }
                txtInventory.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Inventory"].ToString());
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["OptionSku"].ToString()))
                    txtoptionSKU.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["OptionSku"].ToString());


                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["LowInventory"].ToString()))
                    txtLowInventory.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["LowInventory"].ToString());

                int ChkLowInventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(*) from tb_Product where ProductID = " + Request.QueryString["ID"].ToString() + " and (ISNULL( tb_Product.Inventory,0) <  ISNULL( tb_Product.LowInventory,0))"));

                if (ChkLowInventory > 0)
                {
                    ImgToggelInventory.Visible = true;
                }

                chkIsFreeEngraving.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsFreeEngraving"]);

                txtTitleDesc.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["TabTitle1"].ToString());
                txtDescription.Text = Server.HtmlDecode(DsProduct.Tables[0].Rows[0]["Description"].ToString());
                txtFeatures.Text = Server.HtmlDecode(DsProduct.Tables[0].Rows[0]["Features"].ToString());
                txtShippingTime.Text = Server.HtmlDecode(DsProduct.Tables[0].Rows[0]["ShippingTime"].ToString());
                txtmarry.Text = Server.HtmlDecode(DsProduct.Tables[0].Rows[0]["MarryProducts"].ToString());
                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString().Trim()))
                    txtSalePrice.Text = "0.00";
                else
                    txtSalePrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Price"].ToString().Trim()))
                    txtPrice.Text = "0.00";
                else
                    txtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["Price"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["OurPrice"].ToString().Trim()))
                    txtOurPrice.Text = "0.00";
                else
                    txtOurPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["OurPrice"].ToString().Trim()), 2));

                txtInventory.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Inventory"].ToString());

                if (DsProduct.Tables[0].Rows[0]["TaxClassID"] != null && DsProduct.Tables[0].Rows[0]["TaxClassID"].ToString() != "")
                    ddlTaxClass.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["TaxClassID"]);

                if (DsProduct.Tables[0].Rows[0]["ItemType"] != null && DsProduct.Tables[0].Rows[0]["ItemType"].ToString() != "")
                    ddlItemType.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ItemType"]);

                if (ddlItemType.SelectedValue.ToString().ToLower().Trim() == "roman")
                {
                    chkIsRoman.Checked = true;
                    if (chkIsRoman.Checked)
                    {
                        //divRomanShadeYard.Attributes.Add("style", "display:''");
                        divRomanShadeYard.Attributes.Add("style", "display:none");
                    }

                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["ShowOnEffProduct"].ToString()))
                {
                    if (Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["ShowOnEffProduct"].ToString()))
                        rdoShowOnEffProduct.Items.FindByValue(DsProduct.Tables[0].Rows[0]["ShowOnEffProduct"].ToString()).Selected = true;
                    else
                        rdoShowOnEffProduct.Items.FindByValue("False").Selected = true;
                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Productdoublewideid"].ToString()))
                {
                    //select SKU+','  from tb_Product where ProductID in (SELECT Items FROM dbo.Split((select  Productdoublewideid from tb_Product where StoreID = 1 and ProductID = 36770),',') WHERE  isnull(Items,'')<>'') FOR XML PATH('')
                    txtProductDoublewide.Text = Convert.ToString(CommonComponent.GetScalarCommonData("select SKU+','  from tb_Product where ProductID in (SELECT Items FROM dbo.Split((select  Productdoublewideid from tb_Product where StoreID =" + StoreID + " and ProductID = " + ID + "),',') WHERE  isnull(Items,'')<>'') FOR XML PATH('')"));
                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["ProductIDHardware"].ToString()))
                {
                    //select SKU+','  from tb_Product where ProductID in (SELECT Items FROM dbo.Split((select  Productdoublewideid from tb_Product where StoreID = 1 and ProductID = 36770),',') WHERE  isnull(Items,'')<>'') FOR XML PATH('')
                    txtProductPair.Text = Convert.ToString(CommonComponent.GetScalarCommonData("select SKU+','  from tb_Product where ProductID in (SELECT Items FROM dbo.Split((select  ProductIDHardware from tb_Product where StoreID =" + StoreID + " and ProductID = " + ID + "),',') WHERE  isnull(Items,'')<>'') FOR XML PATH('')"));
                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["VideoTitle"].ToString()))
                    txtVideotitle.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["VideoTitle"].ToString());

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Videodetail"].ToString()))
                    txtVideoDetail.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Videodetail"].ToString());

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Whywelove"].ToString()))
                    txtWhyWeLove.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Whywelove"].ToString());

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Stylist"].ToString()))
                    txtStylistList.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Stylist"].ToString());


                chkIsTabbingDisplay.Checked = true;

                if (DsProduct.Tables[0].Rows[0]["IsSaleclearance"].ToString().Trim() == "" || DsProduct.Tables[0].Rows[0]["IsSaleclearance"].ToString().Trim() == null)
                    chkSaleclearance.Checked = false;
                else
                {
                    chkSaleclearance.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsSaleclearance"].ToString().Trim());

                    if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Saleprice"].ToString().Trim()))
                        txtSaleClearance.Text = "0.00";
                    else
                        txtSaleClearance.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["Saleprice"].ToString().Trim()), 2));
                }
                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["FeatureID"].ToString().Trim()))
                    ddlproductfeature.SelectedValue = "0";
                else
                    ddlproductfeature.SelectedValue = DsProduct.Tables[0].Rows[0]["FeatureID"].ToString();


                ///for image

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["ImageDescription"].ToString().Trim()))
                    txtImgDesc.Text = "";
                else
                    txtImgDesc.Text = DsProduct.Tables[0].Rows[0]["ImageDescription"].ToString();

                if (Convert.ToString(DsProduct.Tables[0].Rows[0]["RomanShadeId"].ToString().Trim()) != "")
                {
                    try
                    {
                        ddlRomanShadeYardage.ClearSelection();
                        ddlRomanShadeYardage.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["RomanShadeId"]);
                    }
                    catch { ddlRomanShadeYardage.SelectedIndex = 0; }
                }
                else
                {
                    ddlRomanShadeYardage.SelectedIndex = 0;
                }

                if (Convert.ToString(DsProduct.Tables[0].Rows[0]["FabricType"].ToString().Trim()) != "")
                {
                    try
                    {
                        ddlFabricType.ClearSelection();
                        ddlFabricType.Items.FindByText(Convert.ToString(DsProduct.Tables[0].Rows[0]["FabricType"])).Selected = true;
                    }
                    catch { ddlFabricType.SelectedIndex = 0; }
                }
                else
                {
                    ddlFabricType.SelectedIndex = 0;
                }
                ddlFabricType_SelectedIndexChanged(null, null);
                if (Convert.ToString(DsProduct.Tables[0].Rows[0]["FabricCode"].ToString().Trim()) != "")
                {
                    try
                    {
                        ddlFabricCode.ClearSelection();
                        ddlFabricCode.Items.FindByText(Convert.ToString(DsProduct.Tables[0].Rows[0]["FabricCode"])).Selected = true;
                    }
                    catch { ddlFabricCode.SelectedIndex = 0; }
                }
                else
                {
                    ddlFabricCode.SelectedIndex = 0;
                }

                if (chkSaleclearance.Checked)
                    txtSaleClearance.Attributes.Add("style", "display:'';");
                else
                    txtSaleClearance.Attributes.Add("style", "display:none;");

                txtAvailability.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Avail"].ToString().Trim());
                if (Request.QueryString["CloneID"] == null)
                {
                    ddlQuantityDiscount.SelectedValue = DsProduct.Tables[0].Rows[0]["QuantityDiscountID"].ToString();
                }

                if (Request.QueryString["CloneID"] == null)
                {
                    if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["CountryOfOrigin"].ToString()))
                    {
                        ddlcountryoforigin.SelectedValue = DsProduct.Tables[0].Rows[0]["CountryOfOrigin"].ToString().Trim();
                    }
                    else
                    {
                        ddlcountryoforigin.SelectedIndex = 0;
                    }
                }


                if (Request.QueryString["CloneID"] == null)
                {
                    if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["ProductPostingGroup"].ToString()))
                    {
                        ddlProductPostingGroup.SelectedValue = DsProduct.Tables[0].Rows[0]["ProductPostingGroup"].ToString().Trim();
                    }
                    else
                    {
                        ddlProductPostingGroup.SelectedIndex = 0;
                    }
                }


                if (Request.QueryString["CloneID"] == null)
                {
                    if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["InventoryPostingGroup"].ToString()))
                    {
                        ddlInventoryPostingGroup.SelectedValue = DsProduct.Tables[0].Rows[0]["InventoryPostingGroup"].ToString().Trim();
                    }
                    else
                    {
                        ddlInventoryPostingGroup.SelectedIndex = 0;
                    }
                }

                if (Request.QueryString["CloneID"] == null)
                {
                    if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Weighted"].ToString()))
                    {
                        ddlweighted.SelectedValue = DsProduct.Tables[0].Rows[0]["Weighted"].ToString().Trim();
                    }
                    else
                    {
                        ddlweighted.SelectedIndex = 0;
                    }
                }


                if (Request.QueryString["CloneID"] == null)
                {
                    if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["NavHeader"].ToString()))
                    {
                        ddlnavheader.SelectedValue = DsProduct.Tables[0].Rows[0]["NavHeader"].ToString().Trim();
                    }
                    else
                    {
                        ddlnavheader.SelectedIndex = 0;
                    }
                }

                if (Request.QueryString["CloneID"] == null)
                {
                    if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["NavItemCategory"].ToString()))
                    {
                        ddlnavitemcategory.SelectedValue = DsProduct.Tables[0].Rows[0]["NavItemCategory"].ToString().Trim();
                        ddlnavitemcategory_SelectedIndexChanged(null, null);
                    }
                    else
                    {
                        ddlnavitemcategory.ClearSelection();
                        ddlnavitemcategory.SelectedIndex = 0;
                    }
                }
                if (Request.QueryString["CloneID"] == null)
                {
                    if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["NavProductGroup"].ToString()))
                    {
                        if (ddlProductGroupCode.Items.Count > 0)
                        {
                            ddlProductGroupCode.SelectedValue = DsProduct.Tables[0].Rows[0]["NavProductGroup"].ToString().Trim();
                        }

                    }
                    else
                    {
                        ddlProductGroupCode.ClearSelection();
                        ddlProductGroupCode.SelectedIndex = 0;
                    }
                }
                if (Request.QueryString["CloneID"] == null)
                {
                    if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["NoOfWidth"].ToString()))
                    {

                        txtnoofwidth.Text = DsProduct.Tables[0].Rows[0]["NoOfWidth"].ToString().Trim();


                    }

                }


                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["SurCharge"].ToString().Trim()))
                    txtSurCharge.Text = "0.00";
                else
                    txtSurCharge.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["SurCharge"].ToString().Trim()), 2));

                chkIsGrommet.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsGrommet"].ToString());

                chkOverRide.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsOverRideOption"].ToString());

                chkPublished.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Active"].ToString());


                if (chkPublished.Checked)
                {
                    chkpublishedadmin.Checked = false;
                }
                else
                {
                    chkpublishedadmin.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["AdminActive"].ToString());
                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsdropshipProduct"].ToString().Trim()))
                {
                    chkdropshipproduct.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsdropshipProduct"].ToString());
                }
                else
                {
                    chkdropshipproduct.Checked = false;
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Isnodisplaycpage"].ToString().Trim()))
                {
                    chkIsnodisplaycpage.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Isnodisplaycpage"].ToString());
                }
                else
                {
                    chkIsnodisplaycpage.Checked = false;
                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["FabricVendor"].ToString().Trim()))
                {
                    ddlFabricvendor.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["FabricVendor"].ToString());
                }
                else
                {
                    ddlFabricvendor.SelectedValue = "0";
                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["FabricVendorIds"].ToString().Trim()))
                {
                    string StrFabricVendor = "," + Convert.ToString(DsProduct.Tables[0].Rows[0]["FabricVendorIds"].ToString().Trim()) + ",";
                    if (StrFabricVendor.Length > 0 && StrFabricVendor.ToString().ToLower().IndexOf(",") > -1)
                    {
                        //string[] splitstrFabric = StrFabricVendor.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        //for (int k = 0; k < splitstrFabric.Length; k++)
                        //{
                        for (int i = 0; i < chkFabricvendor.Items.Count; i++)
                        {
                            if (StrFabricVendor.ToString().IndexOf("," + chkFabricvendor.Items[i].Value.ToString() + ",") > -1)
                            {
                                chkFabricvendor.Items[i].Selected = true;
                            }
                        }
                        //}
                    }
                }

                chkIsDiscontinue.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Discontinue"].ToString());
                chkCallusforPrice.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["CallUsForPrice"].ToString());
                chkIsFreeShipping.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsFreeShipping"].ToString());
                if (chkIsFreeShipping.Checked)
                {
                    DiveFreeShipping.Attributes.Add("style", "display:''");
                }
                chkkidscollection.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IskidsCollection"].ToString());
                chkIsShowBuy1Get1.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsShowBuy1Get1"].ToString());
                chkIsHamming.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsHamming"].ToString());
                //if (chkIsHamming.Checked)
                //{
                //tdHamminglbl.Attributes.Add("style", "display:''");
                //tdHammingQty.Attributes.Add("style", "display:''");
                txtHammingQty.Text = DsProduct.Tables[0].Rows[0]["HammingSafetyper"].ToString();
                //}

                txtDisplayOrder.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["DisplayOrder"].ToString().Trim());
                chkNewArrival.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsNewArrival"].ToString());
                if (chkNewArrival.Checked)
                {
                    DivNewArrival.Attributes.Add("style", "display:''");
                }

                chkFeatured.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsFeatured"].ToString());
                if (chkFeatured.Checked)
                {
                    DivFeatured.Attributes.Add("style", "display:''");
                }
                chkGiftWrap.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["GiftWrap"].ToString());
                chkIsAuthorizeRefund.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsAuthorizeRefund"].ToString());

                txtProductURL.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductURL"].ToString().Trim());

                if (Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Active"].ToString()))
                {
                    txtpageredirecturl.Text = "";
                    txtpageredirecturl.Attributes.Add("readonly", "true");
                    tdpageredirect.Attributes.Add("style", "display:none");
                }
                else
                {
                    string OldUrl = "/" + DsProduct.Tables[0].Rows[0]["ProductURL"].ToString().Trim();
                    string NewUrl = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 NewUrl from tb_PageRedirect where StoreID=1 and OldUrl='" + OldUrl + "' order by RedirectID desc"));
                    txtpageredirecturl.Text = NewUrl.Replace("/", "").Trim();
                    if (String.IsNullOrEmpty(NewUrl))
                    {
                        NewUrl = Convert.ToString(CommonComponent.GetScalarCommonData("select Top 1 isnull(tb_category.Sename,'')  from tb_Product INNER JOIN tb_ProductCategory on tb_ProductCategory.Productid=tb_Product.ProductId INNER JOIN tb_category on tb_category.categoryId=tb_ProductCategory.categoryId where tb_Product.ProductID=" + ID + " and tb_Product.StoreID=1 and isnull(tb_category.Active,0)=1 and tb_category.StoreID=1 and isnull(tb_category.Name,'') not like '%Shop All%'"));
                        if (!String.IsNullOrEmpty(NewUrl))
                        {
                            NewUrl = NewUrl + ".html";
                            txtpageredirecturl.Text = NewUrl.Replace("/", "").Trim();
                        }
                    }

                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Ismadetoready"].ToString()))
                {
                    chkisreadymade.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Ismadetoready"].ToString());
                    if (chkisreadymade.Checked == true)
                        chkisordermade.Checked = false;
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Ismadetoorder"].ToString()))
                {
                    chkisordermade.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Ismadetoorder"].ToString());
                    if (chkisordermade.Checked == true)
                        chkisreadymade.Checked = false;
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Ismadetomeasure"].ToString()))
                {
                    chkismadetomeasure.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Ismadetomeasure"].ToString());

                }


                DataSet dsReturn = new DataSet();
                dsReturn = CommonComponent.GetCommonDataSet("select isnull(IsReadtMadeReturn,0) as IsReadtMadeReturn,isnull(IsMadeToOrderReturn,0) as IsMadeToOrderReturn,isnull(IsCustomReturn,0) as IsCustomReturn from tb_product where productid=" + ID + "  ");

                if (dsReturn != null && dsReturn.Tables.Count > 0 && dsReturn.Tables[0].Rows.Count > 0)
                {


                    if (!string.IsNullOrEmpty(dsReturn.Tables[0].Rows[0]["IsReadtMadeReturn"].ToString()))
                    {
                        chkReadyMadeReturn.Checked = Convert.ToBoolean(dsReturn.Tables[0].Rows[0]["IsReadtMadeReturn"].ToString());
                        //if (chkReadyMadeReturn.Checked == true)
                        //    chkMadeToOrderReturn.Checked = false;
                    }
                    if (!string.IsNullOrEmpty(dsReturn.Tables[0].Rows[0]["IsMadeToOrderReturn"].ToString()))
                    {
                        chkMadeToOrderReturn.Checked = Convert.ToBoolean(dsReturn.Tables[0].Rows[0]["IsMadeToOrderReturn"].ToString());
                        //if (chkMadeToOrderReturn.Checked == true)
                        //    chkReadyMadeReturn.Checked = false;
                    }
                    if (!string.IsNullOrEmpty(dsReturn.Tables[0].Rows[0]["IsCustomReturn"].ToString()))
                    {
                        chkMadeToMeasureReturn.Checked = Convert.ToBoolean(dsReturn.Tables[0].Rows[0]["IsCustomReturn"].ToString());

                    }
                }

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Ismadetoorderswatch"].ToString()))
                {
                    chkismadetoswatch.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Ismadetoorderswatch"].ToString());
                    if (chkismadetoswatch.Checked == true)
                    {
                        divProductSwatch.Attributes.Add("style", "display:block");
                    }
                    else
                    {
                        divProductSwatch.Attributes.Add("style", "display:none");
                        txtProductSwatchId.Text = "";
                    }
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Isseocanonical"].ToString()))
                {
                    chkcanonical.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Isseocanonical"].ToString());
                }
                if (chkcanonical.Checked == false)
                {
                    txtcanonical.Attributes.Add("readonly", "true");
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["CanonicalUrl"].ToString()))
                {
                    txtcanonical.Text = DsProduct.Tables[0].Rows[0]["CanonicalUrl"].ToString();
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsBestSeller"].ToString()))
                    chkIsBestSeller.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsBestSeller"].ToString());
                else chkIsBestSeller.Checked = false;
                if (chkIsBestSeller.Checked)
                {
                    DivBestSeller.Attributes.Add("style", "display:''");
                }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsSpecials"].ToString()))
                    chkIsSpecial.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsSpecials"].ToString());
                else chkIsSpecial.Checked = false;

                chkIsfreefabricswatch.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Isfreefabricswatch"].ToString());
                chkIsCustom.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsCustom"].ToString());

                chkIspriceQuote.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IspriceQuote"].ToString());
                chkIsProperty.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Isproperty"].ToString());
                if (chkIsProperty.Checked)
                {
                    divproperty.Attributes.Add("style", "display:''");
                    ddlLightControl.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["LightControl"]);
                    ddlPrivacy.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["Privacy"]);
                    ddlEfficiency.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["Efficiency"]);
                }
                if (Convert.ToInt32(DsProduct.Tables[0].Rows[0]["ProductSwatchid"]) > 0)
                {
                    Int32 ProductSwatchid = Convert.ToInt32(DsProduct.Tables[0].Rows[0]["ProductSwatchid"]);
                    if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["StoreId"])))
                        txtProductSwatchId.Text = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(SKU,'') from tb_Product Where (ISNULL(tb_Product.Deleted,0) != 1) and ProductID=" + ProductSwatchid + " AND (tb_Product.StoreId = " + Convert.ToString(Request.QueryString["StoreId"]) + ")"));
                }

                //if (ddlColors.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Colors"].ToString()) != null)
                //    ddlColors.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Colors"].ToString()).Selected = true;
                //else
                //    ddlColors.SelectedIndex = 0;

                #region Get All Product Mapping Data
                DataSet dsProductMapping = CommonComponent.GetCommonDataSet("exec GetAllProductMappingData " + ID);
                if (dsProductMapping != null && dsProductMapping.Tables.Count > 0)
                {
                    //For Colors START
                    if (dsProductMapping.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dsProductMapping.Tables[0].Rows)
                        {
                            for (int l = 0; l < ddlColors.Items.Count; l++)
                            {
                                if (ddlColors.Items[l].Value.ToString().ToLower().Contains("~"))
                                {
                                    if (drRow["ColorID"].ToString() == ddlColors.Items[l].Value.ToString().ToLower().Split('~')[0])
                                    {
                                        ddlColors.Items[l].Selected = true;
                                    }
                                }
                                else
                                {
                                    if (drRow["ColorID"].ToString() == ddlColors.Items[l].Value.ToString().ToLower())
                                    {
                                        ddlColors.Items[l].Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    //For Colors END

                    //For FABRIC START
                    if (dsProductMapping.Tables[1].Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dsProductMapping.Tables[1].Rows)
                        {
                            for (int l = 0; l < ddlFab.Items.Count; l++)
                            {
                                if (ddlFab.Items[l].Value.ToString().ToLower().Contains("~"))
                                {
                                    if (drRow["FabricID"].ToString() == ddlFab.Items[l].Value.ToString().ToLower().Split('~')[0])
                                    {
                                        ddlFab.Items[l].Selected = true;
                                    }
                                }
                                else
                                {
                                    if (drRow["FabricID"].ToString() == ddlFab.Items[l].Value.ToString().ToLower())
                                    {
                                        ddlFab.Items[l].Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    //For FABRIC END

                    //For PATTERN START
                    if (dsProductMapping.Tables[2].Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dsProductMapping.Tables[2].Rows)
                        {
                            for (int l = 0; l < ddlPatt.Items.Count; l++)
                            {
                                if (ddlPatt.Items[l].Value.ToString().ToLower().Contains("~"))
                                {
                                    if (drRow["PatternID"].ToString() == ddlPatt.Items[l].Value.ToString().ToLower().Split('~')[0])
                                    {
                                        ddlPatt.Items[l].Selected = true;
                                    }
                                }
                                else
                                {
                                    if (drRow["PatternID"].ToString() == ddlPatt.Items[l].Value.ToString().ToLower())
                                    {
                                        ddlPatt.Items[l].Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    //For PATTERN END

                    //For FEATURE START
                    if (dsProductMapping.Tables[3].Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dsProductMapping.Tables[3].Rows)
                        {
                            for (int l = 0; l < ddlStyle.Items.Count; l++)
                            {
                                if (ddlStyle.Items[l].Value.ToString().ToLower().Contains("~"))
                                {
                                    if (drRow["FeatureID"].ToString() == ddlStyle.Items[l].Value.ToString().ToLower().Split('~')[0])
                                    {
                                        ddlStyle.Items[l].Selected = true;
                                    }
                                }
                                else
                                {
                                    if (drRow["FeatureID"].ToString() == ddlStyle.Items[l].Value.ToString().ToLower())
                                    {
                                        ddlStyle.Items[l].Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    //For FEATURE END

                    //For FEATURE START
                    if (dsProductMapping.Tables[4].Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dsProductMapping.Tables[4].Rows)
                        {
                            for (int l = 0; l < ddlNewStyle.Items.Count; l++)
                            {
                                if (ddlNewStyle.Items[l].Value.ToString().ToLower().Contains("~"))
                                {
                                    if (drRow["StyleID"].ToString() == ddlNewStyle.Items[l].Value.ToString().ToLower().Split('~')[0])
                                    {
                                        ddlNewStyle.Items[l].Selected = true;
                                    }
                                }
                                else
                                {
                                    if (drRow["StyleID"].ToString() == ddlNewStyle.Items[l].Value.ToString().ToLower())
                                    {
                                        ddlNewStyle.Items[l].Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    //For FEATURE END

                    //For Room START
                    if (dsProductMapping.Tables[5].Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dsProductMapping.Tables[5].Rows)
                        {
                            for (int l = 0; l < ddlRoom.Items.Count; l++)
                            {
                                if (ddlRoom.Items[l].Value.ToString().ToLower().Contains("~"))
                                {
                                    if (drRow["RoomID"].ToString() == ddlRoom.Items[l].Value.ToString().ToLower().Split('~')[0])
                                    {
                                        ddlRoom.Items[l].Selected = true;
                                    }
                                }
                                else
                                {
                                    if (drRow["RoomID"].ToString() == ddlNewStyle.Items[l].Value.ToString().ToLower())
                                    {
                                        ddlRoom.Items[l].Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    //For Room END

                    //For Room START
                    if (dsProductMapping.Tables[6].Rows.Count > 0)
                    {
                        foreach (DataRow drRow in dsProductMapping.Tables[6].Rows)
                        {
                            for (int l = 0; l < chkHeader.Items.Count; l++)
                            {
                                if (chkHeader.Items[l].Value.ToString().ToLower().Contains("~"))
                                {
                                    if (drRow["HeaderID"].ToString() == chkHeader.Items[l].Value.ToString().ToLower().Split('~')[0])
                                    {
                                        chkHeader.Items[l].Selected = true;
                                    }
                                }
                                else
                                {
                                    if (drRow["HeaderID"].ToString() == chkHeader.Items[l].Value.ToString().ToLower())
                                    {
                                        chkHeader.Items[l].Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    //if (dsProductMapping.Tables[7].Rows.Count > 0)
                    //{
                    //    foreach (DataRow drRow in dsProductMapping.Tables[7].Rows)
                    //    {
                    //        for (int l = 0; l < chkmaterial.Items.Count; l++)
                    //        {
                    //            if (chkmaterial.Items[l].Value.ToString().ToLower().Contains("~"))
                    //            {
                    //                if (drRow["MaterialID"].ToString() == chkmaterial.Items[l].Value.ToString().ToLower().Split('~')[0])
                    //                {
                    //                    chkmaterial.Items[l].Selected = true;
                    //                }
                    //            }
                    //            else
                    //            {
                    //                if (drRow["MaterialID"].ToString() == chkmaterial.Items[l].Value.ToString().ToLower())
                    //                {
                    //                    chkmaterial.Items[l].Selected = true;
                    //                }
                    //            }
                    //        }
                    //    }
                    //}
                    //For Room END

                }
                #endregion
                /*
                if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Colors"].ToString()))
                {
                    string StrColor = "," + DsProduct.Tables[0].Rows[0]["Colors"].ToString();
                    if (StrColor.IndexOf(",") > -1)
                    {
                        for (int l = 0; l < ddlColors.Items.Count; l++)
                        {
                            if (StrColor.ToLower().IndexOf("," + ddlColors.Items[l].Text.ToString().ToLower() + ",") > -1)
                            {
                                ddlColors.Items[l].Selected = true;
                            }
                        }
                    }
                    else
                    {
                        ddlColors.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Colors"].ToString()).Selected = true;
                    }
                }
                */
                string StrHeader = Convert.ToString(DsProduct.Tables[0].Rows[0]["Header"].ToString());
                if (!string.IsNullOrEmpty(StrHeader))
                {
                    string[] StrSplit = StrHeader.Split(',');
                    if (StrSplit.Length > 0)
                    {
                        for (int i = 0; i < StrSplit.Length; i++)
                        {
                            string StrHeaderVal = Convert.ToString(StrSplit[i]);
                            if (!string.IsNullOrEmpty(StrHeaderVal) && chkHeader.Items.Count > 0)
                            {
                                for (int k = 0; k < chkHeader.Items.Count; k++)
                                {
                                    if (chkHeader.Items[k].Value.ToString().Contains("~"))
                                    {
                                        if (chkHeader.Items[k].Value.ToString().Split('~')[1] == StrHeaderVal)
                                        {
                                            chkHeader.Items[k].Selected = true;
                                        }
                                    }
                                    else
                                    {
                                        if (chkHeader.Items[k].Text.ToString() == StrHeaderVal)
                                        {
                                            chkHeader.Items[k].Selected = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (ddlPattern.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Pattern"].ToString()) != null)
                    ddlPattern.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Pattern"].ToString()).Selected = true;
                else
                    ddlPattern.SelectedIndex = 0;

                if (ddlFabric.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Fabric"].ToString()) != null)
                    ddlFabric.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Fabric"].ToString()).Selected = true;
                else
                    ddlFabric.SelectedIndex = 0;

                if (ddlSalePriceTag.Items.FindByValue(DsProduct.Tables[0].Rows[0]["SalePriceTag"].ToString()) != null)
                {
                    ddlSalePriceTag.Items.FindByValue(DsProduct.Tables[0].Rows[0]["SalePriceTag"].ToString()).Selected = true;
                }
                else
                    ddlSalePriceTag.SelectedIndex = 0;

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsNewArrivalFromDate"].ToString()))
                    txtNewArrivalFromDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DsProduct.Tables[0].Rows[0]["IsNewArrivalFromDate"]));
                else
                    txtNewArrivalFromDate.Text = "";

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsNewArrivalToDate"].ToString()))
                    txtNewArrivalToDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DsProduct.Tables[0].Rows[0]["IsNewArrivalToDate"]));
                else
                    txtNewArrivalToDate.Text = "";


                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsFeaturedFromDate"].ToString()))
                    txtIsFeaturedFromDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DsProduct.Tables[0].Rows[0]["IsFeaturedFromDate"]));
                else
                    txtIsFeaturedFromDate.Text = "";

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsFeaturedToDate"].ToString()))
                    txtIsFeaturedToDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DsProduct.Tables[0].Rows[0]["IsFeaturedToDate"]));
                else
                    txtIsFeaturedToDate.Text = "";


                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsBestSellerFromDate"].ToString()))
                    txtBestSellerFromDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DsProduct.Tables[0].Rows[0]["IsBestSellerFromDate"]));
                else
                    txtBestSellerFromDate.Text = "";

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsBestSellerToDate"].ToString()))
                    txtBestSellerToDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DsProduct.Tables[0].Rows[0]["IsBestSellerToDate"]));
                else
                    txtBestSellerToDate.Text = "";


                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsFreeShippingFromDate"].ToString()))
                    txtFreeShippingFromDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DsProduct.Tables[0].Rows[0]["IsFreeShippingFromDate"]));
                else
                    txtFreeShippingFromDate.Text = "";


                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["IsFreeShippingToDate"].ToString()))
                    txtFreeShippingToDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DsProduct.Tables[0].Rows[0]["IsFreeShippingToDate"]));
                else
                    txtFreeShippingToDate.Text = "";

                if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Style"].ToString()))
                {
                    string strStyle = "," + DsProduct.Tables[0].Rows[0]["Style"].ToString();
                    if (strStyle.IndexOf(",") > -1)
                    {
                        for (int l = 0; l < ddlStyle.Items.Count; l++)
                        {
                            if (strStyle.ToLower().IndexOf("," + ddlStyle.Items[l].Text.ToString().ToLower() + ",") > -1)
                            {
                                ddlStyle.Items[l].Selected = true;
                            }
                        }
                    }
                    else
                    {
                        ddlStyle.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Style"].ToString()).Selected = true;
                    }
                }
                if (!String.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["NewStyle"].ToString()))
                {
                    string strStyle = "," + DsProduct.Tables[0].Rows[0]["NewStyle"].ToString();
                    if (strStyle.IndexOf(",") > -1)
                    {
                        for (int l = 0; l < ddlNewStyle.Items.Count; l++)
                        {
                            if (strStyle.ToLower().IndexOf("," + ddlNewStyle.Items[l].Text.ToString().ToLower() + ",") > -1)
                            {
                                ddlNewStyle.Items[l].Selected = true;
                            }
                        }
                    }
                    else
                    {
                        ddlNewStyle.Items.FindByValue(DsProduct.Tables[0].Rows[0]["NewStyle"].ToString()).Selected = true;
                    }
                }

                //if (ddlStyle.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Style"].ToString()) != null)
                //    ddlStyle.Items.FindByValue(DsProduct.Tables[0].Rows[0]["Style"].ToString()).Selected = true;



                txtSETitle.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                txtSEKeyword.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SEKeywords"].ToString());
                txtSEDescription.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SEDescription"].ToString());
                txtToolTip.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["ToolTip"].ToString());

                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Coloroption"].ToString()))
                { txtcoloroptions.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Coloroption"].ToString()); }
                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Coloroptintitle"].ToString()))
                { txtcolorTitle.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Coloroptintitle"].ToString()); }



                if (!string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["RelatedProduct"].ToString()))
                    txtRelProducts.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["RelatedProduct"].ToString());

                if (ddlTagName.Items.FindByValue(DsProduct.Tables[0].Rows[0]["TagName"].ToString()) != null)
                {
                    ddlTagName.Items.FindByValue(DsProduct.Tables[0].Rows[0]["TagName"].ToString()).Selected = true;
                }
                else
                    ddlTagName.SelectedIndex = 0;

                if (ddlMainColor.Items.FindByValue(DsProduct.Tables[0].Rows[0]["MainColorId"].ToString()) != null)
                {
                    ddlMainColor.Items.FindByValue(DsProduct.Tables[0].Rows[0]["MainColorId"].ToString()).Selected = true;
                }
                else
                    ddlMainColor.SelectedIndex = 0;

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["PerInchPrice"].ToString().Trim()))
                    txtPricePerInch.Text = AppLogic.AppConfigs("PerInchPrice");
                else
                    txtPricePerInch.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["PerInchPrice"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["AdditionalCharge"].ToString().Trim()))
                    txtAdditionalCharge.Text = AppLogic.AppConfigs("AdditionalCharge");
                else
                    txtAdditionalCharge.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["AdditionalCharge"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["YardHeaderandhem"].ToString().Trim()))
                    txtInchHeaderHeme.Text = AppLogic.AppConfigs("YardHeaderandhem");
                else
                    txtInchHeaderHeme.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["YardHeaderandhem"].ToString().Trim());

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["FabricInch"].ToString().Trim()))
                    txtFabric.Text = AppLogic.AppConfigs("FabricInch");
                else
                    txtFabric.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["FabricInch"].ToString().Trim()), 2));


                chkSatisfactionGuaranteed.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["isSatisfactionGuaranteed"].ToString());
                txtOptionalAccesories.Text = DsProduct.Tables[0].Rows[0]["OptionalAccessories"].ToString();
                try
                {
                    if (Convert.ToString(DsProduct.Tables[0].Rows[0]["UPC"]) != "")
                    {

                        String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                        if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + Convert.ToString(DsProduct.Tables[0].Rows[0]["UPC"]).Trim() + ".png")))
                        {
                            trBarcode.Visible = true;
                            imgOrderBarcode.Src = FPath + "/UPC-" + Convert.ToString(DsProduct.Tables[0].Rows[0]["UPC"]).Trim() + ".png";
                        }
                        else
                        {
                            GenerateBarcode(Convert.ToString(DsProduct.Tables[0].Rows[0]["UPC"]).Trim());
                        }
                    }
                }
                catch { }

                //if (DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"] != null && DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"].ToString() != "")
                //{
                //    ddlProductTypeDelivery.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"]);
                //    ddlProductTypeDelivery_SelectedIndexChanged(null, null);
                //}
                //if (DsProduct.Tables[0].Rows[0]["ProductTypeID"] != null && DsProduct.Tables[0].Rows[0]["ProductTypeID"].ToString() != "")
                //{
                //    ddlProductType.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductTypeID"]);
                //    ddlProductType_SelectedIndexChanged(null, null);
                //}
                //if (DsProduct.Tables[0].Rows[0]["VendorID"] != null && DsProduct.Tables[0].Rows[0]["VendorID"].ToString() != "")
                //{
                //    ddlvendor.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["VendorID"]);
                //}

                try
                {
                    if (DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"] != null && DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"].ToString() != "")
                    {
                        //ddlProductTypeDelivery.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"]);
                        string name = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_ProductTypeDelivery where ProductTypeDeliveryID=" + DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"] + ""));
                        ddlProductTypeDelivery.ClearSelection();
                        ddlProductTypeDelivery.SelectedValue = DsProduct.Tables[0].Rows[0]["ProductTypeDeliveryID"].ToString();
                        //ddlProductTypeDelivery.Items.FindByText(name).Selected = true;
                        ddlProductTypeDelivery_SelectedIndexChanged(null, null);
                    }
                    if (DsProduct.Tables[0].Rows[0]["ProductTypeID"] != null && DsProduct.Tables[0].Rows[0]["ProductTypeID"].ToString() != "")
                    {
                        //  ddlProductType.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ProductTypeID"]);

                        string name = Convert.ToString(CommonComponent.GetScalarCommonData("Select Name from tb_ProductType where ProductTypeID=" + DsProduct.Tables[0].Rows[0]["ProductTypeID"] + ""));
                        ddlProductType.ClearSelection();
                        //ddlProductType.Items.FindByText(name).Selected = true;
                        ddlProductType.SelectedValue = DsProduct.Tables[0].Rows[0]["ProductTypeID"].ToString();
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
                    String strSavedImgPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/") + strImageName.ToString();
                    objClient.DownloadFile(Imagename.ToString(), Server.MapPath(strSavedImgPath));
                    if (File.Exists(Server.MapPath(strSavedImgPath)))
                    {
                        ImgLarge.Src = strSavedImgPath.ToString();
                        System.Drawing.Image objimg = System.Drawing.Image.FromFile(Server.MapPath(strSavedImgPath));
                        txtIconHeigth.Text = objimg.Height.ToString();
                        txtIconWidth.Text = objimg.Width.ToString();
                    }
                    else
                    {
                        ImgLarge.Src = string.Concat(ViewState["ImagePathProduct"].ToString(), "Large/image_not_available.jpg");
                    }
                    ViewState["File"] = strImageName.ToString();
                }
                else
                {
                    ProductMediumPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Medium/");
                    if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                        Directory.CreateDirectory(Server.MapPath(ProductMediumPath));

                    string strFilePath = Server.MapPath(ProductMediumPath + Imagename);
                    string strIconPath = Server.MapPath(ProductMediumPath.Replace("Medium", "Icon") + Imagename);

                    btnDelete.Visible = false;
                    if (Request.QueryString["CloneID"] == null)
                    {
                        if (File.Exists(strFilePath))
                        {
                            ViewState["DelImage"] = Imagename;
                            btnDelete.Visible = true;
                            ImgLarge.Src = ProductMediumPath + Imagename;
                            if (File.Exists(strIconPath.Trim()))
                            {
                                System.Drawing.Image objimg = System.Drawing.Image.FromFile(strIconPath.Trim());
                                txtIconHeigth.Text = objimg.Height.ToString();
                                txtIconWidth.Text = objimg.Width.ToString();
                            }
                        }
                        else
                        {
                            ViewState["DelImage"] = null;
                            btnDelete.Visible = false;
                            ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                        }

                        DataSet dsImgdesc = new DataSet();
                        dsImgdesc = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductImgDesc WHERE ProductId=" + DsProduct.Tables[0].Rows[0]["ProductId"].ToString() + "");
                        if (dsImgdesc != null && dsImgdesc.Tables.Count > 0 && dsImgdesc.Tables[0].Rows.Count > 0)
                        {
                            Random rd = new Random();
                            for (int d = 0; d < dsImgdesc.Tables[0].Rows.Count; d++)
                            {
                                if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "1")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt1.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt1del.Visible = true;
                                        ViewState["DelImage1"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt1del.Visible = false;
                                        ImgAlt1.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt1.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }

                                }

                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "2")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt2.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt2del.Visible = true;
                                        ViewState["DelImage2"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt2del.Visible = false;
                                        ImgAlt2.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt2.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "3")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt3.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt3del.Visible = true;
                                        ViewState["DelImage3"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt3del.Visible = false;
                                        ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt3.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "4")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt4.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt4del.Visible = true;
                                        ViewState["DelImage4"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt4del.Visible = false;
                                        ImgAlt4.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt4.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "5")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt5.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt5del.Visible = true;
                                        ViewState["DelImage5"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt5del.Visible = false;
                                        ImgAlt5.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt5.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                            }

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
                        DataSet dsImgdesc = new DataSet();
                        dsImgdesc = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductImgDesc WHERE ProductId=" + DsProduct.Tables[0].Rows[0]["ProductId"].ToString() + "");
                        if (dsImgdesc != null && dsImgdesc.Tables.Count > 0 && dsImgdesc.Tables[0].Rows.Count > 0)
                        {
                            Random rd = new Random();
                            for (int d = 0; d < dsImgdesc.Tables[0].Rows.Count; d++)
                            {
                                if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "1")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath.Replace("/Medium/", "/Large/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreId"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt1.Src = AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString();
                                        ViewState["File1"] = fl.Name.ToString();
                                    }
                                    else
                                    {

                                        ImgAlt1.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt1.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }

                                }

                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "2")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreId"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt2.Src = AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString();
                                        ViewState["File2"] = fl.Name.ToString();
                                    }
                                    else
                                    {

                                        ImgAlt2.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt2.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "3")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreId"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt3.Src = AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString();
                                        ViewState["File3"] = fl.Name.ToString();

                                    }
                                    else
                                    {

                                        ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt3.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();
                                        ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "4")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreId"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt4.Src = AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString();
                                        ViewState["File4"] = fl.Name.ToString();

                                    }
                                    else
                                    {

                                        ImgAlt4.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt4.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "5")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreId"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt5.Src = AppLogic.AppConfigs("ImagePathProduct") + "Temp/" + fl.Name.ToString();
                                        ViewState["File5"] = fl.Name.ToString();

                                    }
                                    else
                                    {

                                        ImgAlt5.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt5.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                            }

                        }
                    }
                }
                #endregion

                #region Get PDF File if Exists
                string ProductPdfPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "PDF/");
                if (!string.IsNullOrEmpty(ProductPdfPath.ToString()))
                {
                    string Pdffile = Server.MapPath(ProductPdfPath + Convert.ToString(DsProduct.Tables[0].Rows[0]["PDFName"].ToString()));
                    string ext = System.IO.Path.GetExtension(Pdffile);
                    if (ext == ".pdf")
                    {
                        if (File.Exists(Pdffile))
                        {
                            btnDownloadPDF.Visible = true;
                            btnDownloadPDF.HRef = ProductPdfPath + Convert.ToString(DsProduct.Tables[0].Rows[0]["PDFName"].ToString());
                        }
                        else
                        {
                            btnDownloadPDF.Visible = false;
                        }
                    }
                }

                #endregion

                #region  Bind Category and Main Category Data
                if (Request.QueryString["CloneID"] == null)
                {
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
            }
        }

        #region Category Tree Binding

        /// <summary>
        /// Category Tree Binding
        /// </summary>
        /// <param name="Category">String Category</param>
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
        /// Edits the TreeView
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
        #endregion

        /// <summary>
        /// Function for Insert Product
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        public void InsertProduct(int StoreID)
        {
            try
            {
                ProductComponent objProduct = new ProductComponent();
                tb_Product = new tb_Product();
                tb_Product = SetValue(tb_Product);
                tb_Product.TabTitle1 = "Product Description";
                tb_Product.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tb_Product.CreatedOn = System.DateTime.Now;
                tb_Product.Deleted = false;
                tb_Product.MarryProducts = txtmarry.Text.ToString();

                string CategoryValue = "";
                CategoryValue = SetCategory(StoreID);
                if (CategoryValue != null && CategoryValue != "" && txtFeatures.Text == "")
                {

                    String UpdatedCategoryvalue = "";
                    //Trimstart = CategoryValue.TrimStart(',');
                    //TrimEnd = Trimstart.TrimEnd(',');
                    UpdatedCategoryvalue = CategoryValue;
                    UpdatedCategoryvalue = UpdatedCategoryvalue.Substring(1);
                    UpdatedCategoryvalue = UpdatedCategoryvalue.Substring(0, UpdatedCategoryvalue.Length - 1);
                    int FeatureId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 ISNULL(Featureid,0) AS Featureid FROM dbo.tb_Category WHERE CategoryID IN (" + UpdatedCategoryvalue + ")").ToString());
                    String DescDetails = "";
                    DataSet dsFeatureDesc = new DataSet();
                    StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                    dsFeatureDesc = objProduct.GetproductFeature(FeatureId, StoreID, 2);
                    if (dsFeatureDesc != null && dsFeatureDesc.Tables.Count > 0 && dsFeatureDesc.Tables[0].Rows.Count > 0)
                    {
                        DescDetails = Server.HtmlDecode(dsFeatureDesc.Tables[0].Rows[0]["Body"].ToString().Trim());
                        ddlproductfeature.SelectedValue = FeatureId.ToString();
                        txtFeatures.Text = DescDetails;
                        tb_Product.Features = txtFeatures.Text;
                        tb_Product.FeatureID = FeatureId;
                    }
                }
                //if (string.IsNullOrEmpty(CategoryValue.ToString()))
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcateInsert", "$(document).ready( function() {jAlert('You must select at least one category!', 'Message');});", true);
                //    return;
                //}


                Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect ISNULL(count(sku),0) as TotCnt From tb_product Where sku='" + txtSKU.Text.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1 "));
                if (ChkSKu > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU with same name already exists specify another sku', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                    txtSKU.Focus();
                    return;
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtProductURL.Text.ToString().Trim()))
                    {
                        string ProductURL = txtProductURL.Text.ToString();
                        Int32 ChkProductURL = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect ISNULL(count(ProductURL),0) as TotCnt From tb_product Where ProductURL='" + ProductURL.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1"));
                        if (ChkProductURL > 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkPrtoductURL", "$(document).ready( function() {jAlert('Product with same URL already exists specify another ProductURL', 'Message','ContentPlaceHolder1_txtProductURL');});", true);
                            txtProductURL.Focus();
                            return;
                        }
                    }

                    Int32 ProductID = Convert.ToInt32(ProductComponent.InsertProduct(tb_Product));
                    try
                    {
                        int VendorID = 0;
                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() != "vendor")
                            VendorID = 0;
                        else VendorID = Convert.ToInt32(ddlvendor.SelectedItem.Value);

                        CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET  Videodetail='" + txtVideoDetail.Text.ToString().Trim().Replace("'", "''") + "', VideoTitle='" + txtVideotitle.Text.ToString().Trim().Replace("'", "''") + "', ProductTypeDeliveryID=" + Convert.ToInt32(ddlProductTypeDelivery.SelectedItem.Value) + ",ProductTypeID=" + Convert.ToInt32(ddlProductType.SelectedItem.Value) + ",VendorID=" + VendorID + " WHERE ProductID=" + ProductID + "");
                    }
                    catch
                    {

                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(txtyoutubevideo.Text))
                        {
                            CommonComponent.ExecuteCommonData("update tb_product set YoutubeVideoURL='" + txtyoutubevideo.Text.ToString().Trim().Replace("'", "''") + "' where productid=" + ProductID);
                        }
                    }
                    catch
                    {

                    }
                    if (!string.IsNullOrEmpty(txtcoloroptions.Text))
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Coloroption=  '" + txtcoloroptions.Text.ToString().Replace("'", "''") + "'  WHERE ProductID=" + ProductID); }
                     


                    if (!string.IsNullOrEmpty(txtProductDoublewide.Text))
                    {
                        string strQWide = string.Empty;
                        if (txtProductDoublewide.Text.Trim().Contains(","))
                        {
                            string strSKUs = string.Empty;
                            string[] strArr = txtProductDoublewide.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strArr.Length > 0)
                            {
                                for (int i = 0; i < strArr.Length; i++)
                                {
                                    if (i == strArr.Length - 1)
                                        strSKUs += "'" + strArr[i].ToString() + "'";
                                    else
                                        strSKUs += "'" + strArr[i].ToString() + "',";
                                }
                            }
                            if (!string.IsNullOrEmpty(strSKUs))
                            {
                                strQWide = "update tb_Product set Productdoublewideid = (select cast(ProductID as varchar(20))+ ',' from tb_Product where   isnull(Deleted,0)=0 and isnull(Active,0)=1 and SKU in (" + strSKUs + ") and StoreID =  " + StoreID + " for xml path('')) where ProductID = " + ProductID;
                            }
                        }
                        else
                        {
                            strQWide = "update tb_Product set Productdoublewideid = (select Top 1 ProductID from tb_Product where SKU = '" + txtProductDoublewide.Text.Trim() + "' and StoreID = " + StoreID + " and isnull(Deleted,0)=0 and isnull(Active,0)=1))  where ProductID =" + ProductID;
                        }
                        if (!string.IsNullOrEmpty(strQWide))
                        { CommonComponent.ExecuteCommonData(strQWide); }

                    }
                    else
                    {
                        //CommonComponent.ExecuteCommonData("update tb_Product set Productdoublewideid ='' WHERE  ProductID =" + ProductID + "");
                    }


                    if (!string.IsNullOrEmpty(txtProductPair.Text))
                    {
                        string strQPair = string.Empty;
                        if (txtProductPair.Text.Trim().Contains(","))
                        {
                            string strSKUs = string.Empty;
                            string[] strArr = txtProductPair.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strArr.Length > 0)
                            {
                                for (int i = 0; i < strArr.Length; i++)
                                {
                                    if (i == strArr.Length - 1)
                                        strSKUs += "'" + strArr[i].ToString() + "'";
                                    else
                                        strSKUs += "'" + strArr[i].ToString() + "',";
                                }
                            }
                            if (!string.IsNullOrEmpty(strSKUs))
                            {
                                strQPair = "update tb_Product set ProductIDHardware = (select cast(ProductID as varchar(20))+ ',' from tb_Product where SKU in (" + strSKUs + ") and StoreID =  " + StoreID + " and isnull(Deleted,0)=0 and isnull(Active,0)=1 for xml path('')) where ProductID = " + ProductID;
                            }
                        }
                        else
                        {
                            strQPair = "update tb_Product set ProductIDHardware = (select Top 1 ProductID from tb_Product where SKU = '" + txtProductPair.Text.Trim() + "' and StoreID = " + StoreID + " and isnull(Deleted,0)=0 and isnull(Active,0)=1)  where ProductID =" + ProductID;
                        }
                        if (!string.IsNullOrEmpty(strQPair))
                        { CommonComponent.ExecuteCommonData(strQPair); }

                    }

                    if (chkIsGrommet.Checked)
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET IsGrommet= 1 WHERE ProductID=" + ProductID); }
                    else
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET IsGrommet= 0 WHERE ProductID=" + ProductID); }

                    if (chkOverRide.Checked)
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET IsOverRideOption= 1 WHERE ProductID=" + ProductID); }
                    else
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET IsOverRideOption= 0 WHERE ProductID=" + ProductID); }

                    if (!string.IsNullOrEmpty(txtWhyWeLove.Text))
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Whywelove=  '" + txtWhyWeLove.Text.ToString().Replace("'", "''") + "'  WHERE ProductID=" + ProductID); }

                    if (!string.IsNullOrEmpty(txtStylistList.Text))
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Stylist=  '" + txtStylistList.Text.ToString().Replace("'", "''") + "'  WHERE ProductID=" + ProductID); }


                    string stylenmnew = "";
                    for (int l = 0; l < ddlNewStyle.Items.Count; l++)
                    {
                        if (ddlNewStyle.Items[l].Selected == true)
                        {
                            stylenmnew += ddlNewStyle.Items[l].Text.ToString() + ",";
                        }
                    }
                    Int32 chkIsno = 0;
                    if (chkIsnodisplaycpage.Checked)
                    {
                        chkIsno = 1;
                    }
                    CommonComponent.ExecuteCommonData("update tb_product set ShowOnEffProduct = '" + Convert.ToBoolean(rdoShowOnEffProduct.SelectedValue) + "',NewStyle='" + stylenmnew.Replace("'", "''") + "', Isnodisplaycpage=" + chkIsno + ", IsReadtMadeReturn='" + chkReadyMadeReturn.Checked + "',IsMadeToOrderReturn='" + chkMadeToOrderReturn.Checked + "',IsCustomReturn='" + chkMadeToMeasureReturn.Checked + "' where ProductID=" + ProductID + "");

                    int MainColorID = 0;
                    if (ddlMainColor.SelectedIndex > 0)
                    {
                        MainColorID = Convert.ToInt32(ddlMainColor.SelectedValue.ToString().Trim());
                    }
                    CommonComponent.ExecuteCommonData("update tb_product set MainColorId = '" + MainColorID + "' where ProductID=" + ProductID + "");

                    decimal NoofWidth = decimal.Zero;
                    decimal.TryParse(txtnoofwidth.Text.Trim(), out NoofWidth);
                    NoofWidth = Convert.ToDecimal(NoofWidth);

                    string CountryofOrigin = "";
                    if (ddlcountryoforigin.SelectedIndex > 0)
                    {
                        CountryofOrigin = ddlcountryoforigin.SelectedValue.ToString().Trim();
                    }
                    string ProductPostingGroup = "";
                    if (ddlProductPostingGroup.SelectedIndex > 0)
                    {
                        ProductPostingGroup = ddlProductPostingGroup.SelectedValue.ToString().Trim();
                    }
                    string InventoryPostingGroup = "";
                    if (ddlInventoryPostingGroup.SelectedIndex > 0)
                    {
                        InventoryPostingGroup = ddlInventoryPostingGroup.SelectedValue.ToString().Trim();
                    }
                    string Weighted = "";
                    if (ddlweighted.SelectedIndex > 0)
                    {
                        Weighted = ddlweighted.SelectedValue.ToString().Trim();
                    }

                    string NavHeader = "";
                    if (ddlnavheader.SelectedIndex > 0)
                    {
                        NavHeader = ddlnavheader.SelectedValue.ToString().Trim();
                    }

                    string NavItemCategory = "";
                    if (ddlnavitemcategory.SelectedIndex > 0)
                    {
                        NavItemCategory = ddlnavitemcategory.SelectedValue.ToString().Trim();
                    }

                    string NavProductGroup = "";
                    if (ddlProductGroupCode.Items.Count > 0)
                    {
                        if (ddlProductGroupCode.SelectedIndex > 0)
                        {
                            NavProductGroup = ddlProductGroupCode.SelectedValue.ToString().Trim();
                        }
                    }
                    Int32 SeoUrl = 0;
                    if (chkcanonical.Checked)
                    {
                        SeoUrl = 1;
                    }

                    Int32 adminActive = 0;
                    if (chkpublishedadmin.Checked)
                    {
                        adminActive = 1;
                    }

                    CommonComponent.ExecuteCommonData("UPDATE tb_Product SET AdminActive=" + adminActive + ", Isseocanonical=" + SeoUrl + ", CanonicalUrl='" + txtcanonical.Text.ToString().Replace("'", "''") + "', CountryOfOrigin=  '" + CountryofOrigin + "',ProductPostingGroup='" + ProductPostingGroup + "',InventoryPostingGroup='" + InventoryPostingGroup + "',NoOfWidth=" + NoofWidth + ",Weighted='" + Weighted + "',NavHeader='" + NavHeader + "',NavItemCategory='" + NavItemCategory + "',NavProductGroup='" + NavProductGroup + "'  WHERE ProductID=" + ProductID);



                    if (ProductID > 0)
                    {
                        if (txtUPC.Text.ToString().Trim() != "")
                        {
                            try
                            {
                                GenerateBarcode(txtUPC.Text.ToString().Trim());
                            }
                            catch { }
                        }
                        AddCategory(CategoryValue, ProductID);
                        SaveProductPrice(ProductID);
                        SaveProductOptionsPrice(ProductID);
                        string strImageName = "";
                        string strImageNameNew = "";
                        string strdelImage = "";
                        if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                        {
                            strImageName = RemoveSpecialCharacter(txtSKU.Text.ToString().ToCharArray()) + "_" + ProductID + ".jpg";
                            strImageNameNew = RemoveSpecialCharacter(txtSKU.Text.ToString().ToCharArray()) + "_" + ProductID;
                            SaveImage(strImageName);

                            if (ViewState["File1"] != null)
                            {
                                if (ViewState["DelImage1"] != null)
                                {
                                    strdelImage = ViewState["DelImage1"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_1.jpg", ViewState["File1"].ToString(), ImgAlt1, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=1 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt1.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_1.jpg' Where ImageNo=1 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt1.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",1,'" + txtalt1.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_1.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",1,'','" + strImageNameNew + "_1.jpg')");
                                }
                            }
                            if (ViewState["File2"] != null)
                            {
                                if (ViewState["DelImage2"] != null)
                                {
                                    strdelImage = ViewState["DelImage2"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_2.jpg", ViewState["File2"].ToString(), ImgAlt2, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=2 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt2.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_2.jpg' Where ImageNo=2 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt2.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",2,'" + txtalt2.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_2.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",2,'','" + strImageNameNew + "_2.jpg')");
                                }
                            }
                            if (ViewState["File3"] != null)
                            {
                                if (ViewState["DelImage3"] != null)
                                {
                                    strdelImage = ViewState["DelImage3"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_3.jpg", ViewState["File3"].ToString(), ImgAlt3, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=3 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt3.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_3.jpg' Where ImageNo=3 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt3.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",3,'" + txtalt3.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_3.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",3,'','" + strImageNameNew + "_3.jpg')");
                                }

                            }
                            if (ViewState["File4"] != null)
                            {
                                if (ViewState["DelImage4"] != null)
                                {
                                    strdelImage = ViewState["DelImage4"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_4.jpg", ViewState["File4"].ToString(), ImgAlt4, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=4 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt4.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_4.jpg' Where ImageNo=4 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt4.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",4,'" + txtalt4.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_4.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",4,'','" + strImageNameNew + "_4.jpg')");
                                }

                            }
                            if (ViewState["File5"] != null)
                            {
                                if (ViewState["DelImage5"] != null)
                                {
                                    strdelImage = ViewState["DelImage5"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_5.jpg", ViewState["File5"].ToString(), ImgAlt5, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=5 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt5.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_5.jpg' Where ImageNo=5 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt5.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",5,'" + txtalt5.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_5.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",5,'','" + strImageNameNew + "_5.jpg')");
                                }
                            }

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

                        CommonComponent.ExecuteCommonData("Update tb_UPCMasterReal set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtUPC.Text.Trim() + "'");
                        CommonComponent.ExecuteCommonData("Update tb_UPCMaster set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtUPC.Text.Trim() + "'");

                        #region save and Upload Video


                        string Sourcetemppath = AppLogic.AppConfigs("VideoTempPath").ToString();
                        string targettemppath = AppLogic.AppConfigs("VideoPath").ToString();
                        string sourcePath = @Server.MapPath(Sourcetemppath);
                        string targetPath = @Server.MapPath(targettemppath);

                        string srcfileName = "";
                        if (ViewState["VideoName"] != null)
                        {
                            srcfileName = ViewState["VideoName"].ToString();
                        }

                        string destfileName = ProductID.ToString() + ".mp4";

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

                        srcfileName = "";
                        if (ViewState["VideoName1"] != null)
                        {
                            srcfileName = ViewState["VideoName1"].ToString();
                        }

                        destfileName = ProductID.ToString() + "_1.mp4";

                        sourceFile = System.IO.Path.Combine(sourcePath, srcfileName);
                        destFile = System.IO.Path.Combine(targetPath, destfileName);


                        if (!System.IO.Directory.Exists(targetPath))
                        {
                            System.IO.Directory.CreateDirectory(targetPath);
                        }

                        if (File.Exists(sourceFile))
                        {
                            System.IO.File.Copy(sourceFile, destFile, true);
                        }


                        srcfileName = "";
                        if (ViewState["VideoName2"] != null)
                        {
                            srcfileName = ViewState["VideoName2"].ToString();
                        }

                        destfileName = ProductID.ToString() + "_2.mp4";

                        sourceFile = System.IO.Path.Combine(sourcePath, srcfileName);
                        destFile = System.IO.Path.Combine(targetPath, destfileName);


                        if (!System.IO.Directory.Exists(targetPath))
                        {
                            System.IO.Directory.CreateDirectory(targetPath);
                        }

                        if (File.Exists(sourceFile))
                        {
                            System.IO.File.Copy(sourceFile, destFile, true);
                        }
                        #endregion

                        #region Save Inventory in warehouse

                        SaveInventoryInWarehouse(ProductID, 1);

                        #endregion


                        SaveSecondaryColor(Convert.ToInt32(ProductID));

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
                                ////updatedQuantity = Math.Floor(updatedQuantity);
                                //CommonComponent.ExecuteCommonData("delete from tb_WareHouseProductInventory where productid=" + ProductID + "");

                                //int warehouseid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 warehouseid from tb_WareHouse"));

                                //CommonComponent.ExecuteCommonData("INSERT INTO dbo.tb_WareHouseProductInventory( WareHouseID, ProductID, Inventory )VALUES (" + warehouseid + "," + ProductID + "," + updatedQuantity + ")");

                                //CommonComponent.ExecuteCommonData("update tb_product set inventory=" + updatedQuantity + " where productid=" + ProductID + "");

                            }
                        }
                        #endregion

                        #region new upc logic

                        //int cntSKU = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(SKU), sku from tb_Product where SKU='" + txtSKU.Text + "' group by SKU having COUNT(SKU) > 1"));

                        //if (cntSKU != null && cntSKU > 1)
                        //{

                        //    string existinUPC = Convert.ToString(CommonComponent.GetScalarCommonData("select UPC from tb_Product where storeid ! =" + Convert.ToInt32(Request.QueryString["StoreID"]) + " and sku='" + txtSKU.Text + "'"));
                        //    if (existinUPC != null && existinUPC.Length > 0)
                        //        CommonComponent.ExecuteCommonData("Update tb_product set UPC='" + existinUPC + "' where SKU='" + txtSKU.Text + "'");
                        //}
                        #endregion

                        bool kidscollection = false;
                        if (chkkidscollection.Checked)
                        {
                            kidscollection = true;

                        }
                        else
                        {
                            kidscollection = false;
                        }


                        bool IsShowBuy1Get1 = false;
                        if (chkIsShowBuy1Get1.Checked)
                        {
                            IsShowBuy1Get1 = true;

                        }
                        else
                        {
                            IsShowBuy1Get1 = false;
                        }



                        //return details
                        CommonComponent.ExecuteCommonData("update tb_product set IsKidsCollection='" + kidscollection + "',IsShowBuy1Get1='" + IsShowBuy1Get1 + "' where ProductID=" + ProductID + "");

                        int cntUPC = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(upc) from tb_Product where SKU='" + txtSKU.Text + "' group by UPC having COUNT(upc) > 1 "));
                        if (cntUPC != 0 && cntUPC > 1)
                            chkUPC = true;
                        else
                            chkUPC = false;

                        InsertUpdateStyleData(ProductID);

                        //if (chkUPC == false)
                        //{
                        try
                        {
                            Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIn(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + ProductID.ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                            if (sprice > Decimal.Zero)
                            {
                                CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + ProductID.ToString() + "");

                            }
                        }
                        catch
                        {

                        }
                        CommonComponent.ExecuteCommonData("EXEC GuiSetcoloroption " + ProductID + "");

                        if (hdnexit.Value.ToString() == "1")
                        {
                            Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "ValidaCnt", "jAlert('Record Added Successfully','Message');window.location.href='Product.aspx?StoreID=" + Request.QueryString["StoreId"].ToString() + "&ID=" + ProductID.ToString() + "&Mode=edit';", true);
                        }

                        //}
                        //else
                        //{
                        //    System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Another UPC with same SKU already exists in another Store, UPC for this SKU will be updated with that UPC!');window.location ='ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : "") + "';", true);
                        //}
                    }
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());
            }
        }

        protected void InsertUpdateStyleData(Int32 ProductNewId)
        {
            if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(*) as Tot from tb_ProductStylePrice where productId=" + ProductNewId + "")) > 0)
            {
                CommonComponent.ExecuteCommonData("Delete from tb_ProductStylePrice where productId=" + ProductNewId + "");
            }

            for (int i = 0; i < grdProductStyleType.Rows.Count; i++)
            {
                decimal AddiPrice = 0, PerInch = 0, YardHeaderandhem = 0, FabricInch = 0, Fabricwidth = 0, Dividedwidth = 0;
                CheckBox ChkActive = (CheckBox)grdProductStyleType.Rows[i].FindControl("chkActive");
                Label lblStylename = (Label)grdProductStyleType.Rows[i].FindControl("lblStylename");
                Label ProductStyleId = (Label)grdProductStyleType.Rows[i].FindControl("ProductStyleId");
                TextBox txtAdditionalPrice = (TextBox)grdProductStyleType.Rows[i].FindControl("txtAdditionalPrice");
                TextBox txtPerInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtPerInch");

                TextBox txtYardHeaderandhem = (TextBox)grdProductStyleType.Rows[i].FindControl("txtYardHeaderandhem");
                TextBox txtFabricInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtFabricInch");

                TextBox txtFabricwidth = (TextBox)grdProductStyleType.Rows[i].FindControl("txtFabricwidth");
                TextBox txtdividedwidth = (TextBox)grdProductStyleType.Rows[i].FindControl("txtdividedwidth");
                Int32 Activestr = 0;
                if (ChkActive.Checked)
                {
                    Activestr = 1;
                }
                else
                {
                    Activestr = 0;
                }
                if (!string.IsNullOrEmpty(txtAdditionalPrice.Text.ToString()))
                    AddiPrice = Convert.ToDecimal(txtAdditionalPrice.Text.ToString());
                if (!string.IsNullOrEmpty(txtPerInch.Text.ToString()))
                    PerInch = Convert.ToDecimal(txtPerInch.Text.ToString());

                if (!string.IsNullOrEmpty(txtYardHeaderandhem.Text.ToString()))
                    YardHeaderandhem = Convert.ToDecimal(txtYardHeaderandhem.Text.ToString());
                if (!string.IsNullOrEmpty(txtFabricInch.Text.ToString()))
                    FabricInch = Convert.ToDecimal(txtFabricInch.Text.ToString());
                if (!string.IsNullOrEmpty(txtFabricwidth.Text.ToString()))
                    Fabricwidth = Convert.ToDecimal(txtFabricwidth.Text.ToString());

                if (!string.IsNullOrEmpty(txtdividedwidth.Text.ToString()))
                    Dividedwidth = Convert.ToDecimal(txtdividedwidth.Text.ToString());

                CommonComponent.ExecuteCommonData("Insert into tb_ProductStylePrice (ProductId,Style,AdditionalPrice,PerInch,Active,Createdon,YardHeaderandhem,FabricInch,FabricWidth,DividedWidth) " +
                                                  " values(" + ProductNewId + ",'" + lblStylename.Text.ToString().Replace("'", "''") + "'," + AddiPrice + "," + PerInch + "," + Activestr + ",GETDATE()," + YardHeaderandhem + "," + FabricInch + "," + Fabricwidth + "," + Dividedwidth + ")");


            }
        }
        protected void BindNavItemCategory()
        {
            DataSet dsCat = new DataSet();
            dsCat = CommonComponent.GetCommonDataSet("select Code,Description from tb_NavItemCategory where isnull(active,0)=1");
            if (dsCat != null && dsCat.Tables.Count > 0 && dsCat.Tables[0].Rows.Count > 0)
            {
                ddlnavitemcategory.DataSource = dsCat;
                ddlnavitemcategory.DataTextField = "Description";
                ddlnavitemcategory.DataValueField = "Code";
            }
            else
            {
                ddlnavitemcategory.DataSource = null;
            }
            ddlnavitemcategory.DataBind();
            ddlnavitemcategory.Items.Insert(0, new ListItem("Select Item Category", ""));
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
        /// Function for Update Product
        /// </summary>
        /// <param name="ProductID">in ProductID</param>
        /// <param name="StoreId">int StoreId</param>
        public void UpdateProduct(int ProductID, int StoreId)
        {
            ProductComponent objProduct = new ProductComponent();
            tb_Product = new tb_Product();
            tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);
            tb_Product = SetValue(tb_Product);

            Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ISNULL(count(sku),0) as TotCnt From tb_product Where sku='" + txtSKU.Text.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " and Productid <>" + ProductID + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1 "));
            if (ChkSKu > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU with same name already exists specify another sku', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                txtSKU.Focus();
                return;
            }
            else
            {
                if (!string.IsNullOrEmpty(txtProductURL.Text.ToString().Trim()))
                {
                    string ProductURL = txtProductURL.Text.ToString();
                    Int32 ChkProductURL = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect ISNULL(count(ProductURL),0) as TotCnt From tb_product Where ProductURL='" + ProductURL.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " and Productid <>" + ProductID + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1"));
                    if (ChkProductURL > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkPrtoductURL", "$(document).ready( function() {jAlert('Product with same URL already exists specify another ProductURL', 'Message','ContentPlaceHolder1_txtProductURL');});", true);
                        txtProductURL.Focus();
                        return;
                    }
                }



                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit" && Request.QueryString["ID"] != null)
                {
                    DataSet dscat = new DataSet();
                    dscat = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductCategory WHERE  ProductID=" + Request.QueryString["ID"].ToString() + "");
                    ViewState["getcategorylist"] = dscat;

                    DeleteCategory(Convert.ToInt32(Request.QueryString["ID"].ToString()));
                }
                string CategoryValue = "";
                CategoryValue = SetCategory(StoreId);

                if (CategoryValue != null && CategoryValue != "" && txtFeatures.Text == "")
                {

                    String UpdatedCategoryvalue = "";
                    //Trimstart = CategoryValue.TrimStart(',');
                    //TrimEnd = Trimstart.TrimEnd(',');
                    UpdatedCategoryvalue = CategoryValue;
                    UpdatedCategoryvalue = UpdatedCategoryvalue.Substring(1);
                    UpdatedCategoryvalue = UpdatedCategoryvalue.Substring(0, UpdatedCategoryvalue.Length - 1);
                    int FeatureId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 ISNULL(Featureid,0) AS Featureid FROM dbo.tb_Category WHERE CategoryID IN (" + UpdatedCategoryvalue + ")").ToString());
                    String DescDetails = "";
                    DataSet dsFeatureDesc = new DataSet();
                    StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                    dsFeatureDesc = objProduct.GetproductFeature(FeatureId, StoreID, 2);
                    if (dsFeatureDesc != null && dsFeatureDesc.Tables.Count > 0 && dsFeatureDesc.Tables[0].Rows.Count > 0)
                    {
                        DescDetails = Server.HtmlDecode(dsFeatureDesc.Tables[0].Rows[0]["Body"].ToString().Trim());
                        ddlproductfeature.SelectedValue = FeatureId.ToString();
                        txtFeatures.Text = DescDetails;
                        tb_Product.Features = txtFeatures.Text;
                        tb_Product.FeatureID = FeatureId;
                    }
                }

                //if (string.IsNullOrEmpty(CategoryValue.ToString()))
                //{
                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgCategory", "$(document).ready( function() {jAlert('You must select at least one category!', 'Message');});", true);
                //    return;
                //}

                bool chkdisconti = Convert.ToBoolean(CommonComponent.GetScalarCommonData("Select isnull(Discontinue,0) from tb_Product where ProductID=" + ProductID + ""));
                if ((chkdisconti == false && chkIsDiscontinue.Checked == true))
                {


                    CommonComponent.ExecuteCommonData("Update tb_Product set DiscontinuedOn='" + DateTime.Now + "' where ProductID=" + ProductID + "");
                    CommonComponent.ExecuteCommonData("Exec GuiInsertDiscontinueLog '" + txtSKU.Text.ToString().Trim() + "','Manual'," + Session["AdminID"].ToString() + ",1,'" + txtUPC.Text.ToString().Trim() + "',0");
                }
                else if ((chkdisconti == true && chkIsDiscontinue.Checked == false))
                {
                    CommonComponent.ExecuteCommonData("Exec GuiInsertDiscontinueLog '" + txtSKU.Text.ToString().Trim() + "','Manual'," + Session["AdminID"].ToString() + ",0,'" + txtUPC.Text.ToString().Trim() + "',1");
                }


                try
                {
                    DataSet Dscompare = new DataSet();
                    Dscompare = CommonComponent.GetCommonDataSet("select isnull(IsSaleclearance,0) as IsSaleclearance,isnull(saleprice,0) as saleprice,isnull(sku,'') as sku,isnull(upc,'') as upc from tb_Product where ProductID=" + ProductID + "");
                    if (Dscompare != null && Dscompare.Tables.Count > 0 && Dscompare.Tables[0].Rows.Count > 0)
                    {
                        bool beforeonsale = false;
                        Boolean.TryParse(Dscompare.Tables[0].Rows[0]["IsSaleclearance"].ToString(), out beforeonsale);
                        Decimal beforeonsaleprice = decimal.Zero;
                        Decimal.TryParse(Dscompare.Tables[0].Rows[0]["IsSaleclearance"].ToString(), out beforeonsaleprice);


                        bool onsale = false;
                        Decimal onsaleprice = decimal.Zero;
                        if (chkSaleclearance.Checked)
                        {
                            onsale = true;
                        }

                        Decimal.TryParse(txtSaleClearance.Text.Trim(), out onsaleprice);
                        if (beforeonsale != onsale)
                        {
                            CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + Dscompare.Tables[0].Rows[0]["sku"].ToString().Trim().Replace("'", "''") + "','" + Dscompare.Tables[0].Rows[0]["upc"].ToString().Trim().Replace("'", "''") + "'," + onsale + ",'',''," + beforeonsale + ",'','','Manual'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                        }
                        else if (beforeonsale == onsale && beforeonsaleprice != onsaleprice && onsale == true)
                        {
                            CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + Dscompare.Tables[0].Rows[0]["sku"].ToString().Trim().Replace("'", "''") + "','" + Dscompare.Tables[0].Rows[0]["upc"].ToString().Trim().Replace("'", "''") + "'," + onsale + ",'',''," + beforeonsale + ",'','','Manual'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                        }
                    }
                }
                catch { }


                try
                {
                    DataSet Dsnewarrcompare = new DataSet();
                    Dsnewarrcompare = CommonComponent.GetCommonDataSet("select isnull(IsNewArrival,0) as IsNewArrival,isnull(IsNewArrivalFromDate,'') as IsNewArrivalFromDate,isnull(IsNewArrivalToDate,'') as IsNewArrivalToDate,isnull(TagName,'') as TagName,isnull(sku,'') as sku,isnull(upc,'') as upc from tb_Product  where ProductID=" + ProductID + "");
                    if (Dsnewarrcompare != null && Dsnewarrcompare.Tables.Count > 0 && Dsnewarrcompare.Tables[0].Rows.Count > 0)
                    {
                        bool IsNewArrival = false;
                        bool BeforeIsNewArrival = false;
                        DateTime IsNewArrivalFromdate;
                        DateTime IsNewArrivalTodate;
                        DateTime BeforeIsNewArrivalfromdate;
                        DateTime beforeIsNewArrivaltodate;
                        Boolean.TryParse(Dsnewarrcompare.Tables[0].Rows[0]["IsNewArrival"].ToString(), out BeforeIsNewArrival);
                        DateTime.TryParse(Dsnewarrcompare.Tables[0].Rows[0]["IsNewArrivalFromDate"].ToString(), out BeforeIsNewArrivalfromdate);
                        DateTime.TryParse(Dsnewarrcompare.Tables[0].Rows[0]["IsNewArrivalToDate"].ToString(), out beforeIsNewArrivaltodate);

                        if (chkNewArrival.Checked)
                        {
                            IsNewArrival = true;

                        }
                        DateTime.TryParse(txtNewArrivalFromDate.Text, out IsNewArrivalFromdate);
                        DateTime.TryParse(txtNewArrivalToDate.Text, out IsNewArrivalTodate);

                        // if (IsNewArrival == true)
                        // {
                        if (BeforeIsNewArrival == IsNewArrival)
                        {


                            if (BeforeIsNewArrivalfromdate.Date == IsNewArrivalFromdate.Date && beforeIsNewArrivaltodate.Date == IsNewArrivalTodate.Date)
                            {

                            }
                            else
                            {
                                if (IsNewArrival == true)
                                {
                                    CommonComponent.ExecuteCommonData("Exec GuiInsertNewArrivalLog '" + Dsnewarrcompare.Tables[0].Rows[0]["sku"].ToString().Trim().Replace("'", "''") + "','" + Dsnewarrcompare.Tables[0].Rows[0]["upc"].ToString().Trim().Replace("'", "''") + "'," + IsNewArrival + ",'" + IsNewArrivalFromdate + "','" + IsNewArrivalTodate + "'," + BeforeIsNewArrival + ",'" + BeforeIsNewArrivalfromdate + "','" + beforeIsNewArrivaltodate + "','Manual'," + Session["AdminID"].ToString() + "");
                                }


                            }
                        }
                        else if (BeforeIsNewArrival != IsNewArrival)
                        {
                            CommonComponent.ExecuteCommonData("Exec GuiInsertNewArrivalLog '" + Dsnewarrcompare.Tables[0].Rows[0]["sku"].ToString().Trim().Replace("'", "''") + "','" + Dsnewarrcompare.Tables[0].Rows[0]["upc"].ToString().Trim().Replace("'", "''") + "'," + IsNewArrival + ",'" + IsNewArrivalFromdate + "','" + IsNewArrivalTodate + "'," + BeforeIsNewArrival + ",'" + BeforeIsNewArrivalfromdate + "','" + beforeIsNewArrivaltodate + "','Manual'," + Session["AdminID"].ToString() + "");
                        }
                        //}

                    }
                }
                catch { }


                Int32 inventoryUpdated = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select isnull(Inventory,0) from tb_Product where ProductID=" + ProductID + ""));



                decimal NoofWidth = decimal.Zero;
                decimal.TryParse(txtnoofwidth.Text.Trim(), out NoofWidth);
                NoofWidth = Convert.ToDecimal(NoofWidth);

                string CountryofOrigin = "";
                if (ddlcountryoforigin.SelectedIndex > 0)
                {
                    CountryofOrigin = ddlcountryoforigin.SelectedValue.ToString().Trim();
                }
                string ProductPostingGroup = "";
                if (ddlProductPostingGroup.SelectedIndex > 0)
                {
                    ProductPostingGroup = ddlProductPostingGroup.SelectedValue.ToString().Trim();
                }
                string InventoryPostingGroup = "";
                if (ddlInventoryPostingGroup.SelectedIndex > 0)
                {
                    InventoryPostingGroup = ddlInventoryPostingGroup.SelectedValue.ToString().Trim();
                }
                string Weighted = "";
                if (ddlweighted.SelectedIndex > 0)
                {
                    Weighted = ddlweighted.SelectedValue.ToString().Trim();
                }
                string NavHeader = "";
                if (ddlnavheader.SelectedIndex > 0)
                {
                    NavHeader = ddlnavheader.SelectedValue.ToString().Trim();
                }
                string NavItemCategory = "";
                if (ddlnavitemcategory.SelectedIndex > 0)
                {
                    NavItemCategory = ddlnavitemcategory.SelectedValue.ToString().Trim();
                }

                string NavProductGroup = "";
                if (ddlProductGroupCode.Items.Count > 0)
                {
                    if (ddlProductGroupCode.SelectedIndex > 0)
                    {
                        NavProductGroup = ddlProductGroupCode.SelectedValue.ToString().Trim();
                    }
                }
                Int32 SeoUrl = 0;
                if (chkcanonical.Checked)
                {
                    SeoUrl = 1;
                }
                Int32 adminActive = 0;
                if (chkpublishedadmin.Checked)
                {
                    adminActive = 1;
                }
                #region StyleNew

                string stylenmnew = "";
                for (int l = 0; l < ddlNewStyle.Items.Count; l++)
                {
                    if (ddlNewStyle.Items[l].Selected == true)
                    {
                        if (ddlNewStyle.Items[l].Value.ToString().Contains("~"))
                            stylenmnew += ddlNewStyle.Items[l].Value.ToString().Split('~')[1].ToString() + ",";
                        else
                            stylenmnew += ddlNewStyle.Items[l].Value.ToString() + ",";
                    }
                }
                //for (int l = 0; l < ddlNewStyle.Items.Count; l++)
                //{
                //    if (ddlNewStyle.Items[l].Selected == true)
                //    {
                //        stylenmnew += ddlNewStyle.Items[l].Text.ToString() + ",";
                //    }
                //}
                #endregion

                int MainColorID = 0;
                if (ddlMainColor.SelectedIndex > 0)
                {
                    MainColorID = Convert.ToInt32(ddlMainColor.SelectedValue.ToString().Trim());
                }
                CommonComponent.ExecuteCommonData("update tb_product set MainColorId = '" + MainColorID + "' where ProductID=" + ProductID + "");

                if (chkIsGrommet.Checked)
                { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET IsGrommet= 1 WHERE ProductID=" + ProductID); }
                else
                { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET IsGrommet= 0 WHERE ProductID=" + ProductID); }

                if (chkOverRide.Checked)
                { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET IsOverRideOption= 1 WHERE ProductID=" + ProductID); }
                else
                { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET IsOverRideOption= 0 WHERE ProductID=" + ProductID); }

                Int32 chkIsno = 0;
                if (chkIsnodisplaycpage.Checked)
                {
                    chkIsno = 1;
                }
                CommonComponent.ExecuteCommonData("UPDATE tb_Product SET ShowOnEffProduct = '" + Convert.ToBoolean(rdoShowOnEffProduct.SelectedValue) + "',NewStyle='" + stylenmnew.Replace("'", "''") + "', Isnodisplaycpage=" + chkIsno + ", AdminActive=" + adminActive + ", Videodetail='" + txtVideoDetail.Text.ToString().Trim().Replace("'", "''") + "', VideoTitle='" + txtVideotitle.Text.ToString().Trim().Replace("'", "''") + "', Isseocanonical=" + SeoUrl + ", CanonicalUrl='" + txtcanonical.Text.ToString().Replace("'", "''") + "', CountryOfOrigin=  '" + CountryofOrigin + "',ProductPostingGroup='" + ProductPostingGroup + "',InventoryPostingGroup='" + InventoryPostingGroup + "',NoOfWidth=" + NoofWidth + ",Weighted='" + Weighted + "',NavHeader='" + NavHeader + "',NavItemCategory='" + NavItemCategory + "',NavProductGroup='" + NavProductGroup + "'  WHERE ProductID=" + ProductID);

                CommonComponent.ExecuteCommonData("update tb_product set YoutubeVideoURL='" + txtyoutubevideo.Text.ToString().Trim().Replace("'", "''") + "' where productid=" + ProductID);
                try
                {
                    if (!string.IsNullOrEmpty(txtProductURL.Text.ToString().Trim()) && chkPublished.Checked == true)
                    {
                        CommonComponent.ExecuteCommonData("EXEC Guisetchangeproductsename " + ProductID + ",'" + txtProductURL.Text.ToString().Trim() + "'");
                    }

                }
                catch
                {

                }

                #region pageredirect
                try
                {
                    //if (!String.IsNullOrEmpty(txtpageredirecturl.Text))
                    //{
                    string NewUrl = txtpageredirecturl.Text.ToString();
                    NewUrl = NewUrl.Replace("/", "").Trim();

                    String OldUrl = "/" + txtProductURL.Text.ToString().Trim();
                    Int32 RedirectID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 RedirectID from tb_PageRedirect where StoreID=1 and isnull(OldUrl,'')='" + OldUrl + "' order by RedirectID desc"));
                    if (RedirectID > 0)
                    {
                        // Int32 RedirectID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 RedirectID from tb_PageRedirect where StoreID=1 and OldUrl='" + OldUrl + "' order by RedirectID desc"));
                        CommonComponent.ExecuteCommonData("update tb_PageRedirect set NewUrl='" + NewUrl.Replace("'", "''") + "' where RedirectID=" + RedirectID + "");
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(txtpageredirecturl.Text.ToString()))
                        {
                            CommonComponent.ExecuteCommonData("insert into tb_PageRedirect(storeid,OldUrl,NewUrl) values (1,'" + OldUrl.Replace("'", "''") + "','" + NewUrl.Replace("'", "''") + "')");
                            //Response.Write("insert into tb_PageRedirect(storeid,OldUrl,NewUrl) values (1,'" + OldUrl.Replace("'", "''") + "','" + NewUrl.Replace("'", "''") + "')");
                        }
                    }

                    // }
                }
                catch (Exception ex)
                {

                    //Response.Write(ex.Message.ToString()+" "+ ex.StackTrace.ToString());
                }
                #endregion

                if (Convert.ToInt32(ProductComponent.UpdateProduct(tb_Product)) > 0)
                {
                    #region Insert Update Search Option Colors
                    string Strcolors = string.Empty;
                    for (int l = 0; l < ddlColors.Items.Count; l++)
                    {
                        if (ddlColors.Items[l].Selected == true)
                        {
                            if (ddlColors.Items[l].Value.ToString().Contains("~"))
                                Strcolors += ddlColors.Items[l].Value.ToString().Split('~')[0].ToString() + ",";
                            else
                                Strcolors += ddlColors.Items[l].Value.ToString() + ",";
                        }
                    }
                    //if (!string.IsNullOrEmpty(Strcolors))
                    {
                        CommonComponent.ExecuteCommonData("Exec UpdateProductMappings '" + Strcolors.ToString() + "'," + Convert.ToInt32(ProductID) + ",'color'");

                    }
                    //CommonComponent.ExecuteCommonData("update tb_product SET Colors='" + Strcolors + "' WHERE ProductID=" + ProductID + "");
                    #endregion

                    #region Insert Update Search Option Features
                    string StrFeatures = string.Empty;
                    for (int l = 0; l < ddlStyle.Items.Count; l++)
                    {
                        if (ddlStyle.Items[l].Selected == true)
                        {
                            if (ddlStyle.Items[l].Value.ToString().Contains("~"))
                                StrFeatures += ddlStyle.Items[l].Value.ToString().Split('~')[0].ToString() + ",";
                            else
                                StrFeatures += ddlStyle.Items[l].Value.ToString() + ",";
                        }
                    }
                    // if (!string.IsNullOrEmpty(StrFeatures))
                    {
                        CommonComponent.ExecuteCommonData("Exec UpdateProductMappings '" + StrFeatures.ToString() + "'," + Convert.ToInt32(ProductID) + ",'feature'");
                    }
                    #endregion

                    #region Insert Update Search Option Style
                    string strStyle = string.Empty;
                    for (int l = 0; l < ddlNewStyle.Items.Count; l++)
                    {
                        if (ddlNewStyle.Items[l].Selected == true)
                        {
                            if (ddlNewStyle.Items[l].Value.ToString().Contains("~"))
                                strStyle += ddlNewStyle.Items[l].Value.ToString().Split('~')[0].ToString() + ",";
                            else
                                strStyle += ddlNewStyle.Items[l].Value.ToString() + ",";
                        }
                    }
                    //if (!string.IsNullOrEmpty(StrFeatures))
                    {
                        CommonComponent.ExecuteCommonData("Exec UpdateProductMappings '" + strStyle.ToString() + "'," + Convert.ToInt32(ProductID) + ",'style'");
                    }
                    #endregion

                    #region Insert Update Search Option Rooms
                    string strRooms = string.Empty;
                    for (int l = 0; l < ddlRoom.Items.Count; l++)
                    {
                        if (ddlRoom.Items[l].Selected == true)
                        {
                            if (ddlNewStyle.Items[l].Value.ToString().Contains("~"))
                                strRooms += ddlRoom.Items[l].Value.ToString().Split('~')[0].ToString() + ",";
                            else
                                strRooms += ddlRoom.Items[l].Value.ToString() + ",";
                        }
                    }
                    //  if (!string.IsNullOrEmpty(StrFeatures))
                    {
                        CommonComponent.ExecuteCommonData("Exec UpdateProductMappings '" + strRooms.ToString() + "'," + Convert.ToInt32(ProductID) + ",'room'");
                    }
                    #endregion

                    #region Insert Update Search Option Fabric
                    string strFabric = string.Empty;
                    for (int l = 0; l < ddlFab.Items.Count; l++)
                    {
                        if (ddlFab.Items[l].Selected == true)
                        {
                            if (ddlFab.Items[l].Value.ToString().Contains("~"))
                                strFabric += ddlFab.Items[l].Value.ToString().Split('~')[0].ToString() + ",";
                            else
                                strFabric += ddlFab.Items[l].Value.ToString() + ",";
                        }
                    }
                    // if (!string.IsNullOrEmpty(StrFeatures))
                    {
                        CommonComponent.ExecuteCommonData("Exec UpdateProductMappings '" + strFabric.ToString() + "'," + Convert.ToInt32(ProductID) + ",'fabric'");
                    }
                    #endregion

                    #region Insert Update Search Option Pattern
                    string strPattern = string.Empty;
                    for (int l = 0; l < ddlPatt.Items.Count; l++)
                    {
                        if (ddlPatt.Items[l].Selected == true)
                        {
                            if (ddlPatt.Items[l].Value.ToString().Contains("~"))
                                strPattern += ddlPatt.Items[l].Value.ToString().Split('~')[0].ToString() + ",";
                            else
                                strPattern += ddlPatt.Items[l].Value.ToString() + ",";
                        }
                    }
                    //   if (!string.IsNullOrEmpty(StrFeatures))
                    {
                        CommonComponent.ExecuteCommonData("Exec UpdateProductMappings '" + strPattern.ToString() + "'," + Convert.ToInt32(ProductID) + ",'pattern'");
                    }
                    #endregion

                    #region Insert Update Search Option Headers
                    string strHeader = string.Empty;
                    for (int l = 0; l < chkHeader.Items.Count; l++)
                    {
                        if (chkHeader.Items[l].Selected == true)
                        {
                            if (chkHeader.Items[l].Value.ToString().Contains("~"))
                                strHeader += chkHeader.Items[l].Value.ToString().Split('~')[0].ToString() + ",";
                            else
                                strHeader += chkHeader.Items[l].Value.ToString() + ",";
                        }
                    }
                    // if (!string.IsNullOrEmpty(StrFeatures))
                    {
                        CommonComponent.ExecuteCommonData("Exec UpdateProductMappings '" + strHeader.ToString() + "'," + Convert.ToInt32(ProductID) + ",'header'");
                    }
                    #endregion

                    //Update Content for Swach
                    if (ddlItemType.SelectedItem.Text.ToString().ToLower() == "swatch" && chkForAll.Checked)
                    {
                        CommonComponent.ExecuteCommonData("EXEC GuiSetalloptiontoparent " + ProductID + "");
                    }

                    //string strMaterial = string.Empty;
                    //for (int l = 0; l < chkmaterial.Items.Count; l++)
                    //{
                    //    if (chkmaterial.Items[l].Selected == true)
                    //    {
                    //        if (chkmaterial.Items[l].Value.ToString().Contains("~"))
                    //            strMaterial += chkmaterial.Items[l].Value.ToString().Split('~')[0].ToString() + ",";
                    //        else
                    //            strMaterial += chkmaterial.Items[l].Value.ToString() + ",";
                    //    }
                    //}
                    //CommonComponent.ExecuteCommonData("Exec UpdateProductMappings '" + strMaterial.ToString() + "'," + Convert.ToInt32(ProductID) + ",'material'");

                    if (txtUPC.Text.ToString().Trim() != "")
                    {
                        try
                        {
                            GenerateBarcode(txtUPC.Text.ToString().Trim());
                        }
                        catch { }
                    }
                    AddCategory(CategoryValue, ProductID);
                    try
                    {
                        int VendorID = 0;
                        if (ddlProductTypeDelivery.SelectedItem.Text.ToLower() != "vendor")
                            VendorID = 0;
                        else VendorID = Convert.ToInt32(ddlvendor.SelectedItem.Value);

                        CommonComponent.ExecuteCommonData("UPDATE dbo.tb_Product SET ProductTypeDeliveryID=" + Convert.ToInt32(ddlProductTypeDelivery.SelectedItem.Value) + ",ProductTypeID=" + Convert.ToInt32(ddlProductType.SelectedItem.Value) + ",VendorID=" + VendorID + " WHERE ProductID=" + ProductID + "");
                    }
                    catch { }

                    if (!string.IsNullOrEmpty(txtProductDoublewide.Text))
                    {
                        string strQWide = string.Empty;
                        if (txtProductDoublewide.Text.Trim().Contains(","))
                        {
                            string strSKUs = string.Empty;
                            string[] strArr = txtProductDoublewide.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strArr.Length > 0)
                            {
                                for (int i = 0; i < strArr.Length; i++)
                                {
                                    if (i == strArr.Length - 1)
                                        strSKUs += "'" + strArr[i].ToString() + "'";
                                    else
                                        strSKUs += "'" + strArr[i].ToString() + "',";
                                }
                            }
                            if (!string.IsNullOrEmpty(strSKUs))
                            {
                                strQWide = "update tb_Product set Productdoublewideid = (select cast(ProductID as varchar(20))+ ',' from tb_Product where   isnull(Deleted,0)=0 and isnull(Active,0)=1 and SKU in (" + strSKUs + ") and StoreID =  " + StoreID + " for xml path('')) where ProductID = " + ProductID;
                            }
                        }
                        else
                        {
                            strQWide = "update tb_Product set Productdoublewideid = (select ProductID from tb_Product where   isnull(Deleted,0)=0 and isnull(Active,0)=1 and SKU ='" + txtProductDoublewide.Text.Trim() + "' and StoreID = " + StoreId + ")  where ProductID =" + ProductID;
                        }
                        if (!string.IsNullOrEmpty(strQWide))
                        { CommonComponent.ExecuteCommonData(strQWide); }

                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("update tb_Product set Productdoublewideid ='' WHERE  ProductID =" + ProductID + "");
                    }

                    if (!string.IsNullOrEmpty(txtProductPair.Text))
                    {
                        string strQPair = string.Empty;
                        if (txtProductPair.Text.Trim().Contains(","))
                        {
                            string strSKUs = string.Empty;
                            string[] strArr = txtProductPair.Text.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strArr.Length > 0)
                            {
                                for (int i = 0; i < strArr.Length; i++)
                                {
                                    if (i == strArr.Length - 1)
                                        strSKUs += "'" + strArr[i].ToString() + "'";
                                    else
                                        strSKUs += "'" + strArr[i].ToString() + "',";
                                }
                            }
                            if (!string.IsNullOrEmpty(strSKUs))
                            {
                                strQPair = "update tb_Product set ProductIDHardware = (select cast(ProductID as varchar(20))+ ',' from tb_Product where SKU in (" + strSKUs + ") and isnull(Deleted,0)=0 and isnull(Active,0)=1 and StoreID =  " + StoreID + " for xml path('')) where ProductID = " + ProductID;
                            }
                        }
                        else
                        {
                            strQPair = "update tb_Product set ProductIDHardware = (select top 1 ProductID from tb_Product where SKU ='" + txtProductPair.Text.Trim() + "' and StoreID = " + StoreID + "  and isnull(Deleted,0)=0 and isnull(Active,0)=1)   where ProductID =" + ProductID;
                        }
                        if (!string.IsNullOrEmpty(strQPair))
                        { CommonComponent.ExecuteCommonData(strQPair); }

                    }
                    else
                    {
                        CommonComponent.ExecuteCommonData("update tb_Product set ProductIDHardware ='' WHERE  ProductID =" + ProductID + "");
                    }


                    if (!string.IsNullOrEmpty(txtWhyWeLove.Text))
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Whywelove=  '" + txtWhyWeLove.Text.ToString().Replace("'", "''") + "'  WHERE ProductID=" + ProductID); }
                    else
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Whywelove='' WHERE ProductID=" + ProductID);
                    }

                    if (!string.IsNullOrEmpty(txtStylistList.Text))
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Stylist= '" + txtStylistList.Text.ToString().Replace("'", "''") + "'  WHERE ProductID=" + ProductID); }
                    else
                    {
                        CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Stylist=''  WHERE ProductID=" + ProductID);
                    }

                    SaveProductOptionsPrice(ProductID);

                    #region Save and Update Image
                    string strImageName = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(txtSKU.Text.ToString().Trim()))
                        {

                            string Imgname = "";

                            if (!string.IsNullOrEmpty(tb_Product.ImageName))
                            {
                                Imgname = Convert.ToString(tb_Product.ImageName.ToString());
                                strImageName = Imgname;
                            }
                            else
                            {
                                strImageName = RemoveSpecialCharacter(txtSKU.Text.ToString().ToCharArray()) + "_" + ProductID + ".jpg";
                            }
                            if (ImgLarge.Src.ToLower().IndexOf("/temp/") > -1)
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


                            string strdelImage = "";
                            string strImageNameNew = strImageName.Replace(".jpg", "");
                            if (ImgAlt1.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File1"] != null)
                            {
                                if (ViewState["DelImage1"] != null)
                                {
                                    strdelImage = ViewState["DelImage1"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_1.jpg", ViewState["File1"].ToString(), ImgAlt1, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=1 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt1.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_1.jpg' Where ImageNo=1 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt1.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",1,'" + txtalt1.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_1.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",1,'','" + strImageNameNew + "_1.jpg')");
                                }
                            }
                            if (ImgAlt2.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File2"] != null)
                            {
                                if (ViewState["DelImage2"] != null)
                                {
                                    strdelImage = ViewState["DelImage2"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_2.jpg", ViewState["File2"].ToString(), ImgAlt2, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=2 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt2.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_2.jpg' Where ImageNo=2 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt2.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",2,'" + txtalt2.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_2.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",2,'','" + strImageNameNew + "_2.jpg')");
                                }
                            }
                            if (ImgAlt3.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File3"] != null)
                            {
                                if (ViewState["DelImage3"] != null)
                                {
                                    strdelImage = ViewState["DelImage3"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_3.jpg", ViewState["File3"].ToString(), ImgAlt3, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=3 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt3.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_3.jpg' Where ImageNo=3 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt3.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",3,'" + txtalt3.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_3.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",3,'','" + strImageNameNew + "_3.jpg')");
                                }
                            }
                            if (ImgAlt4.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File4"] != null)
                            {
                                if (ViewState["DelImage4"] != null)
                                {
                                    strdelImage = ViewState["DelImage4"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_4.jpg", ViewState["File4"].ToString(), ImgAlt4, strdelImage);


                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=4 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt4.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_4.jpg' Where ImageNo= 4 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt4.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",4,'" + txtalt4.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_4.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",4,'','" + strImageNameNew + "_4.jpg')");
                                }

                            }
                            if (ImgAlt5.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File5"] != null)
                            {
                                if (ViewState["DelImage5"] != null)
                                {
                                    strdelImage = ViewState["DelImage5"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_5.jpg", ViewState["File5"].ToString(), ImgAlt5, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=5 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt5.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_5.jpg' Where ImageNo=5 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt5.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",5,'" + txtalt5.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_5.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",5,'','" + strImageNameNew + "_5.jpg')");
                                }
                            }
                        }

                        tb_Product = new tb_Product();
                        tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);

                        if (string.IsNullOrEmpty(ImgLarge.Src.ToString()) || ImgLarge.Src.ToString().ToLower().IndexOf("image_not_available") > -1)
                        {
                            //tb_Product.ImageName = "";
                        }
                        else
                        {
                            if (ViewState["File"] != null && ImgLarge.Src.ToLower().IndexOf("/temp/") <= -1)
                            {
                                tb_Product.ImageName = ViewState["File"].ToString();
                            }
                            else
                            {
                                tb_Product.ImageName = strImageName;
                            }
                        }

                    }
                    catch { }

                    #endregion

                    #region Save PDF File
                    try
                    {
                        bool Flag = false;
                        ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
                        string ProductPDFPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "PDF/");
                        if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                            Directory.CreateDirectory(Server.MapPath(ProductTempPath));

                        if (ViewState["PdfFile"] != null)
                        {
                            if (ViewState["PdfFile"].ToString().Length > 0 && Path.GetExtension(ViewState["PdfFile"].ToString().ToLower()) == ".pdf".ToString().ToLower())
                                Flag = true;

                            if (Flag)
                            {
                                if (ViewState["PdfFile"].ToString().Length > 0)
                                {
                                    String ChkPdfFile = Convert.ToString(CommonComponent.GetScalarCommonData("SElect ISNULL(PDFName,'') as PDFName From tb_product Where Productid=" + ProductID + " And StoreId=" + Request.QueryString["StoreId"].ToString() + ""));
                                    if (!string.IsNullOrEmpty(ChkPdfFile.ToString()))
                                    {
                                        if (File.Exists(Server.MapPath(ProductPDFPath) + ChkPdfFile))
                                        {
                                            File.Delete(Server.MapPath(ProductPDFPath) + ChkPdfFile);
                                        }
                                    }
                                    File.Copy(Server.MapPath(ProductTempPath) + ViewState["PdfFile"], Server.MapPath(ProductPDFPath) + RemoveSpecialCharacter(txtSKU.Text.ToString().ToCharArray()) + "_" + ProductID + ".pdf");
                                    tb_Product.PDFName = RemoveSpecialCharacter(txtSKU.Text.ToString().ToCharArray()) + "_" + ProductID + ".pdf";
                                }
                            }
                        }
                    }
                    catch { }
                    #endregion

                    ProductComponent.UpdateProduct(tb_Product);


                    bool kidscollection = false;
                    if (chkkidscollection.Checked)
                    {
                        kidscollection = true;

                    }
                    else
                    {
                        kidscollection = false;
                    }
                    bool IsShowBuy1Get1 = false;
                    if (chkIsShowBuy1Get1.Checked)
                    {
                        IsShowBuy1Get1 = true;
                    }
                    else
                    {
                        IsShowBuy1Get1 = false;
                    }
                    //return details
                    CommonComponent.ExecuteCommonData("update tb_product set IsKidsCollection='" + kidscollection + "',IsShowBuy1Get1='" + IsShowBuy1Get1 + "' where ProductID=" + ProductID + "");
                    CommonComponent.ExecuteCommonData("update tb_product set IsReadtMadeReturn='" + chkReadyMadeReturn.Checked + "',IsMadeToOrderReturn='" + chkMadeToOrderReturn.Checked + "',IsCustomReturn='" + chkMadeToMeasureReturn.Checked + "' where ProductID=" + ProductID + "");
                    if (!string.IsNullOrEmpty(txtcoloroptions.Text))
                    { CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Coloroption= '" + txtcoloroptions.Text.ToString().Replace("'", "''") + "'  WHERE ProductID=" + ProductID); }
                    else {
                        CommonComponent.ExecuteCommonData("UPDATE tb_Product SET Coloroption= ''  WHERE ProductID=" + ProductID);
                    }
                    CommonComponent.ExecuteCommonData("Update tb_UPCMaster set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtUPC.Text.Trim() + "'");
                    CommonComponent.ExecuteCommonData("Update tb_UPCMasterReal set SKU='" + txtSKU.Text.Trim() + "' where UPC ='" + txtUPC.Text.Trim() + "'");

                    CommonComponent.ExecuteCommonData("EXEC GuiSetcoloroption " + ProductID + "");
                    //#region pageredirect
                    //try
                    //{
                    //    //if (!String.IsNullOrEmpty(txtpageredirecturl.Text))
                    //    //{
                    //    string NewUrl = txtpageredirecturl.Text.ToString();
                    //    NewUrl = NewUrl.Replace("/", "").Trim();

                    //    String OldUrl = "/" + txtProductURL.Text.ToString().Trim();
                    //    Int32 RedirectID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 RedirectID from tb_PageRedirect where StoreID=1 and OldUrl='" + OldUrl + "' order by RedirectID desc"));
                    //    if (RedirectID > 0)
                    //    {
                    //        // Int32 RedirectID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 RedirectID from tb_PageRedirect where StoreID=1 and OldUrl='" + OldUrl + "' order by RedirectID desc"));

                    //        CommonComponent.ExecuteCommonData("update tb_PageRedirect set NewUrl='" + NewUrl.Replace("'", "''") + "' where RedirectID=" + RedirectID + "");

                    //    }
                    //    else
                    //    {
                    //        if (!String.IsNullOrEmpty(txtpageredirecturl.Text.ToString()))
                    //        {
                    //            CommonComponent.ExecuteCommonData("insert into tb_PageRedirect(storeid,OldUrl,NewUrl) values (1,'" + OldUrl.Replace("'", "''") + "','" + NewUrl.Replace("'", "''") + "')");
                    //            //Response.Write("insert into tb_PageRedirect(storeid,OldUrl,NewUrl) values (1,'" + OldUrl.Replace("'", "''") + "','" + NewUrl.Replace("'", "''") + "')");
                    //        }
                    //    }

                    //    // }
                    //}
                    //catch(Exception ex) 
                    //{

                    //    //Response.Write(ex.Message.ToString()+" "+ ex.StackTrace.ToString());
                    //}
                    //#endregion

                    #region Save and upload Video
                    //new for video
                    string targettemppath = AppLogic.AppConfigs("VideoPath").ToString();
                    string CheckFile = targettemppath + ProductID.ToString() + ".mp4";
                    if (ViewState["VideoName"] != null)
                    {
                        string Sourcetemppath = AppLogic.AppConfigs("VideoTempPath").ToString();
                        string sourcePath = @Server.MapPath(Sourcetemppath);
                        string targetPath = @Server.MapPath(targettemppath);
                        string srcfileName = ViewState["VideoName"].ToString();
                        string destfileName = ProductID.ToString() + ".mp4";

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
                    }
                    else
                    {
                        if (File.Exists(Server.MapPath(CheckFile)))
                        {
                            File.Delete(Server.MapPath(CheckFile));
                        }
                    }
                    CheckFile = targettemppath + ProductID.ToString() + "_1.mp4";
                    if (ViewState["VideoName1"] != null)
                    {
                        string Sourcetemppath = AppLogic.AppConfigs("VideoTempPath").ToString();
                        string sourcePath = @Server.MapPath(Sourcetemppath);
                        string targetPath = @Server.MapPath(targettemppath);
                        string srcfileName = ViewState["VideoName1"].ToString();
                        string destfileName = ProductID.ToString() + "_1.mp4";

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
                    }
                    else
                    {
                        if (File.Exists(Server.MapPath(CheckFile)))
                        {
                            File.Delete(Server.MapPath(CheckFile));
                        }
                    }


                    CheckFile = targettemppath + ProductID.ToString() + "_2.mp4";
                    if (ViewState["VideoName2"] != null)
                    {
                        string Sourcetemppath = AppLogic.AppConfigs("VideoTempPath").ToString();
                        string sourcePath = @Server.MapPath(Sourcetemppath);
                        string targetPath = @Server.MapPath(targettemppath);
                        string srcfileName = ViewState["VideoName2"].ToString();
                        string destfileName = ProductID.ToString() + "_2.mp4";

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
                    }
                    else
                    {
                        if (File.Exists(Server.MapPath(CheckFile)))
                        {
                            File.Delete(Server.MapPath(CheckFile));
                        }
                    }
                    #endregion

                    #region Update Inventory in warehouse

                    SaveInventoryInWarehouse(Convert.ToInt32(Request.QueryString["ID"].ToString()), 2);

                    #endregion

                    //Update vendor and ProductType and Stock/Dropship


                    SaveSecondaryColor(Convert.ToInt32(ProductID));

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
                            //CommonComponent.ExecuteCommonData("delete from tb_WareHouseProductInventory where productid=" + ProductID + "");

                            //int warehouseid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 warehouseid from tb_WareHouse"));

                            //CommonComponent.ExecuteCommonData("INSERT INTO dbo.tb_WareHouseProductInventory( WareHouseID, ProductID, Inventory,PreferredLocation )VALUES (" + warehouseid + "," + ProductID + "," + updatedQuantity + ")");

                            //CommonComponent.ExecuteCommonData("update tb_product set inventory=" + updatedQuantity + " where productid=" + ProductID + "");

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
                    //    CommonComponent.ExecuteCommonData("Update tb_product set UPC='" + txtUPC.Text + "' where SKU='" + txtSKU.Text + "'");
                    //}

                    //int cntUPC = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(upc) from tb_Product where SKU='" + txtSKU.Text + "' group by UPC having COUNT(upc) > 1 "));
                    //if (cntUPC > 1)
                    //    chkUPC = true;
                    //else
                    //    chkUPC = false;

                    //if (chkUPC == false)
                    //{
                    try
                    {
                        Int32 cnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(ProductID) from tb_ProductVariantValue  Where isnull(VarActive,0)=1 and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(Buy1Get1,0)=1 and isnull(Buy1Price,0)<>0 and ProductID=" + ProductID.ToString() + ""));
                        if (cnt > 0)
                        {
                            Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select min(A.price) from (select isnull(Buy1Price,0) as Price from tb_ProductVariantValue  Where isnull(VarActive,0)=1 and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(Buy1Get1,0)=1 and isnull(Buy1Price,0)<>0 and ProductID=" + ProductID.ToString() + " union all SELECT (isnull(Variantprice,0)) as Price FROM tb_ProductVariantValue WHERE   isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1 and isnull(Buy1Price,0)<=0 and isnull(VarActive,0)=1 and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(Buy1Get1,0)=1 and ProductID=" + ProductID.ToString() + ") as A"));
                            if (sprice > Decimal.Zero)
                            {
                                CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + ProductID.ToString() + "");

                            }
                        }
                        else
                        {

                            Int32 cntt = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(ProductID) from tb_ProductVariantValue  Where isnull(VarActive,0)=1 and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(OnSale,0)=1 and isnull(OnSalePrice,0)<>0 and ProductID=" + ProductID.ToString() + ""));

                            if (cntt > 0)
                            {
                                Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select min(A.price) from (select isnull(OnSalePrice,0) as Price from tb_ProductVariantValue  Where isnull(VarActive,0)=1 and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(OnSale,0)=1 and isnull(OnSalePrice,0)<>0 and ProductID=" + ProductID.ToString() + " union all SELECT (isnull(Variantprice,0)) as Price FROM tb_ProductVariantValue WHERE   isnull(Variantprice,0) > 0  and isnull(OnSalePrice,0)<=0 and isnull(VarActive,0)=1 and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(OnSale,0)=1 and ProductID=" + ProductID.ToString() + ") as A"));
                                if (sprice > Decimal.Zero)
                                {
                                    CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + ProductID.ToString() + "");

                                }
                            }
                            else
                            {
                                Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIn(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + ProductID.ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                                if (sprice > Decimal.Zero)
                                {
                                    CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + ProductID.ToString() + "");

                                }
                            }

                        }




                    }
                    catch
                    {

                    }

                    if (hdnexit.Value.ToString() == "1")
                    {
                        Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "ValidaCnt", "jAlert('Record Updated Successfully.','Message');window.location.href=window.location.href;", true);
                    }
                    //}
                    //else
                    //{
                    //    Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    //    //System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Another UPC with same SKU already exists in another Store, UPC for this SKU will be updated with that UPC!');window.location ='ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : "") + "';", true);
                    //}
                }
            }
        }

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
            //if (!string.IsNullOrEmpty(txtOurPrice.Text.ToString()) && Convert.ToDecimal(txtOurPrice.Text.ToString()) > 0 && Convert.ToDecimal(txtSalePrice.Text.ToString()) > 0)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcateInsert", "$(document).ready( function() {jAlert('You must enter Our Cost less than or equal to Sale price!', 'Message');});", true);
            //    return;
            //}
            //else if (!string.IsNullOrEmpty(txtOurPrice.Text.ToString()) && Convert.ToDecimal(txtOurPrice.Text.ToString()) > 0 && Convert.ToDecimal(txtPrice.Text.ToString()) > 0)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgcateInsert", "$(document).ready( function() {jAlert('You must enter Our Cost less than or equal to price!', 'Message');});", true);
            //    return;
            //}
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
                    Int32 Result = Convert.ToInt32(ProductComponent.SaveCloneProductVariant(Convert.ToInt32(Request.QueryString["StoreId"]), Convert.ToInt32(Request.QueryString["Id"]), ProductID));
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
                                //string srcfileName = Request.QueryString["Id"].ToString() + ".flv";
                                //string destfileName = ProductID.ToString() + ".flv";
                                string srcfileName = Request.QueryString["Id"].ToString() + ".mp4";
                                string destfileName = ProductID.ToString() + ".mp4";
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
                    if (hdnexit.Value.ToString() == "1")
                    {
                        Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "ValidaCnt", "jAlert('Record Added Successfully','Message');window.location.href='Product.aspx?StoreID=" + Request.QueryString["StoreId"].ToString() + "&ID=" + ProductID.ToString() + "&Mode=edit';", true);
                    }
                }
            }
        }

        /// <summary>
        /// Set Category
        /// </summary>
        /// <param name="StoreId">int StoreId</param>
        /// <returns>Returns the Category as a String</returns>
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

            if (!string.IsNullOrEmpty(txtMaincategory.Text))
            {
                tb_Product.MainCategory = RemoveSpecialCharacter(txtMaincategory.Text.Trim().ToLower().ToCharArray());
            }
            return CategoryValue;
        }

        /// <summary>
        /// Set Value into Product Table
        /// </summary>
        /// <param name="tb_Product">tb_Product tb_Product</param>
        /// <returns>Returns the tb_Product Object</returns>
        public tb_Product SetValue(tb_Product tb_Product)
        {
            if (Request.QueryString["StoreID"] != null)
                tb_Product.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(Request.QueryString["StoreID"].ToString()));

            tb_Product.Name = Convert.ToString(txtProductName.Text.Trim());
            tb_Product.SEName = RemoveSpecialCharacter(txtProductName.Text.Trim().ToLower().ToCharArray());

            tb_Product.SKU = txtSKU.Text.Trim();
            tb_Product.OptionSku = txtoptionSKU.Text.ToString();
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

            if (ddlFabricType.SelectedIndex != 0)
            {
                tb_Product.FabricType = Convert.ToString(ddlFabricType.SelectedItem.Text);
            }

            if (ddlRomanShadeYardage.SelectedIndex != 0 && ddlItemType.SelectedValue.ToString().ToLower().Trim() == "roman")
            {
                tb_Product.RomanShadeId = Convert.ToInt32(ddlRomanShadeYardage.SelectedValue);
            }
            else { tb_Product.RomanShadeId = 0; }

            if (ddlFabricCode.SelectedIndex != 0)
            {
                tb_Product.FabricCode = Convert.ToString(ddlFabricCode.SelectedItem.Text);
            }

            if (Convert.ToInt32(ddlproductfeature.SelectedValue) > 0)
            {
                tb_Product.FeatureID = Convert.ToInt32(ddlproductfeature.SelectedValue);
                tb_Product.Features = txtFeatures.Text.Trim();
            }
            tb_Product.ImageDescription = txtImgDesc.Text.ToString();
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
                        tb_Product.UPC = txtUPC.Text.Trim();
                    }
                }
            }
            else
            {
                tb_Product.UPC = txtUPC.Text.Trim();
            }

            if (ddlTaxClass.SelectedValue != "0")
            {
                tb_Product.tb_TaxClassReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_TaxClass", "TaxClassID", Convert.ToInt32(ddlTaxClass.SelectedValue));
            }

            if (ddlProductType.SelectedValue != "0")
            {
                tb_Product.tb_ProductTypeReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_ProductType", "ProductTypeID", Convert.ToInt32(ddlProductType.SelectedValue));
            }

            if (!string.IsNullOrEmpty(txtProductURL.Text.ToString().Trim()))
            {
                string ProductURL = txtProductURL.Text.ToString();
                tb_Product.ProductURL = ProductURL.ToString();
            }
            else { tb_Product.ProductURL = ""; }

            if (ddlManufacture.SelectedValue != "0")
            {
                tb_Product.tb_ManufactureReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Manufacture", "ManufactureID", Convert.ToInt32(ddlManufacture.SelectedValue));
            }
            tb_Product.Avail = txtAvailability.Text.ToString();

            tb_Product.ItemType = ddlItemType.SelectedValue.ToString();


            if (txtInventory.Text.Trim().Length == 0)
                txtInventory.Text = "0";
            tb_Product.Inventory = Convert.ToInt32(txtInventory.Text.Trim());

            if (txtLowInventory.Text.Trim().Length == 0)
                txtLowInventory.Text = "0";
            tb_Product.LowInventory = Convert.ToInt32(txtLowInventory.Text.Trim());

            if (ddlQuantityDiscount.SelectedValue != "0")
                tb_Product.tb_QauntityDiscountReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_QauntityDiscount", "QuantityDiscountID", Convert.ToInt32(ddlQuantityDiscount.SelectedValue));
            else
                tb_Product.tb_QauntityDiscountReference.EntityKey = null;

            decimal Weight = decimal.Zero;
            decimal.TryParse(txtWeight.Text.Trim(), out Weight);
            tb_Product.Weight = Convert.ToDecimal(Weight);

            decimal Width = decimal.Zero;
            decimal.TryParse(txtWidth.Text.Trim(), out Width);
            tb_Product.Width = Convert.ToString(Width);

            decimal Height = decimal.Zero;
            decimal.TryParse(txtHeight.Text.Trim(), out Height);
            tb_Product.Height = Convert.ToString(Height);

            decimal Length = decimal.Zero;
            decimal.TryParse(txtLength.Text.Trim(), out Length);
            tb_Product.Length = Convert.ToString(Length);

            decimal Price = decimal.Zero;
            decimal.TryParse(txtPrice.Text.Trim(), out Price);
            tb_Product.Price = Convert.ToDecimal(Price);

            decimal OurPrice = decimal.Zero;
            decimal.TryParse(txtOurPrice.Text.Trim(), out OurPrice);
            tb_Product.OurPrice = Convert.ToDecimal(OurPrice);

            tb_Product.IsSaleclearance = chkSaleclearance.Checked;
            if (chkSaleclearance.Checked)
            {
                if (txtSalePrice.Text.Trim().Length == 0)
                    txtSalePrice.Text = "0";

                if (txtSaleClearance.Text != "")
                    tb_Product.SalePrice = Convert.ToDecimal(txtSaleClearance.Text.Trim());
                else tb_Product.SalePrice = Convert.ToDecimal(0);
            }
            else
            {
                if (txtSalePrice.Text.Trim().Length == 0)
                    txtSalePrice.Text = "0";

                if (txtSalePrice.Text.Trim() != "")
                    tb_Product.SalePrice = Convert.ToDecimal(txtSalePrice.Text.Trim());
                else tb_Product.SalePrice = Convert.ToDecimal(0);
            }

            tb_Product.Features = txtFeatures.Text.Trim();
            tb_Product.ShippingTime = txtShippingTime.Text.Trim();

            tb_Product.IsFreeEngraving = Convert.ToBoolean(chkIsFreeEngraving.Checked);
            tb_Product.Coloroptintitle = Convert.ToString(txtcolorTitle.Text.ToString());
            tb_Product.EngravingSize = Convert.ToInt32(0);

            tb_Product.SurCharge = Convert.ToDecimal((txtSurCharge.Text.Trim() == "") ? "0.00" : txtSurCharge.Text.Trim());
            tb_Product.Active = chkPublished.Checked;
            tb_Product.IsdropshipProduct = chkdropshipproduct.Checked;
            tb_Product.CallUsForPrice = chkCallusforPrice.Checked;
            tb_Product.IsFreeShipping = chkIsFreeShipping.Checked;
            tb_Product.IsHamming = chkIsHamming.Checked;
            //if (chkIsHamming.Checked)
            //{
            decimal HammingQty = 0;
            decimal.TryParse(txtHammingQty.Text.ToString(), out HammingQty);
            tb_Product.HammingSafetyper = Convert.ToDecimal(HammingQty);
            //tb_Product.FabricVendor = Convert.ToInt32(ddlFabricvendor.SelectedValue.ToString());
            tb_Product.FabricVendor = Convert.ToInt32(0);
            string StrFabricVendor = "";
            //foreach (ListItem li in chkFabricvendor.Items)
            //{
            //    if (li.Selected)
            //    {
            //        StrFabricVendor += li.Value + ",";
            //    }
            //}
            //if (StrFabricVendor.Length > 1)
            //{
            //    StrFabricVendor = StrFabricVendor.Substring(0, StrFabricVendor.Length - 1);
            //}
            StrFabricVendor = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(FabricVendorIds,'') as FabricVendorIds FROM tb_ProductFabricCode WHERE Code='" + ddlFabricCode.SelectedItem.Text.ToString() + "' AND FabricTypeID=" + ddlFabricType.SelectedValue.ToString() + ""));
            tb_Product.FabricVendorIds = StrFabricVendor.ToString();
            //}

            //tb_Product.Isfreefabricswatch = chkIsfreefabricswatch.Checked;
            if (ddlItemType.SelectedValue.ToString().ToLower() == "swatch")
            {
                tb_Product.Isfreefabricswatch = true;
            }
            else
            {
                tb_Product.Isfreefabricswatch = false;
            }
            //tb_Product.IsCustom = chkIsCustom.Checked;
            tb_Product.IsCustom = chkismadetomeasure.Checked;
            if (ddlItemType.SelectedValue.ToString().ToLower().Trim() == "roman")
            {
                tb_Product.IsRoman = true;
            }
            else { tb_Product.IsRoman = false; }
            if (ddlItemType.SelectedValue.ToString().ToLower().Trim() == "roman")
            {
                //divRomanShadeYard.Attributes.Add("style", "display:''");
                divRomanShadeYard.Attributes.Add("style", "display:none");
            }
            tb_Product.IspriceQuote = chkIspriceQuote.Checked;
            Int32 ProductSwatchid = 0;
            if (!string.IsNullOrEmpty(txtProductSwatchId.Text.Trim().ToString()))
            {
                string ProSku = Convert.ToString(txtProductSwatchId.Text.Trim());
                if (!string.IsNullOrEmpty(Convert.ToString(Request.QueryString["StoreId"])))
                    ProductSwatchid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ProductID from tb_Product Where (ISNULL(tb_Product.Deleted,0) != 1) and SKU='" + ProSku + "' AND (tb_Product.StoreId = " + Convert.ToString(Request.QueryString["StoreId"]) + ")"));
            }
            tb_Product.ProductSwatchid = ProductSwatchid;
            tb_Product.Isproperty = chkIsProperty.Checked;
            if (chkIsProperty.Checked)
            {
                tb_Product.LightControl = Convert.ToInt32(ddlLightControl.SelectedValue);
                tb_Product.Privacy = Convert.ToInt32(ddlPrivacy.SelectedValue);
                tb_Product.Efficiency = Convert.ToInt32(ddlEfficiency.SelectedValue);
            }
            else
            {
                tb_Product.LightControl = 0;
                tb_Product.Privacy = 0;
                tb_Product.Efficiency = 0;
            }

            #region Color
            string Strcolor = "";
            for (int l = 0; l < ddlColors.Items.Count; l++)
            {
                if (ddlColors.Items[l].Selected == true)
                {
                    if (ddlColors.Items[l].Value.ToString().Contains("~"))
                        Strcolor += ddlColors.Items[l].Value.ToString().Split('~')[1].ToString() + ",";
                    else
                        Strcolor += ddlColors.Items[l].Value.ToString() + ",";
                }
            }
            tb_Product.Colors = Strcolor;
            #endregion

            #region Pattern

            //if (ddlPattern.SelectedIndex > 0)
            //    tb_Product.Pattern = ddlPattern.SelectedValue.ToString();
            //else
            //    tb_Product.Pattern = "";
            string StrPattern = "";
            if (ddlPatt.Items.Count > 0)
            {
                for (int i = 0; i < ddlPatt.Items.Count; i++)
                {
                    if (ddlPatt.Items[i].Selected)
                    {
                        if (string.IsNullOrEmpty(StrPattern))
                        {
                            if (ddlPatt.Items[i].Value.ToString().Contains("~"))
                                StrPattern += ddlPatt.Items[i].Value.ToString().Split('~')[1].ToString() + ",";
                            else
                                StrPattern = StrPattern + ddlPatt.Items[i].Text.ToString() + ",";
                        }
                    }
                }
            }
            tb_Product.Pattern = StrPattern.ToString();
            #endregion

            #region Fabric
            //if (ddlFabric.SelectedIndex > 0)
            //    tb_Product.Fabric = ddlFabric.SelectedValue.ToString();
            //else
            //    tb_Product.Fabric = "";
            string StrFabric = "";
            if (ddlFab.Items.Count > 0)
            {
                for (int i = 0; i < chkHeader.Items.Count; i++)
                {
                    if (ddlFab.Items[i].Selected)
                    {
                        if (string.IsNullOrEmpty(StrFabric))
                        {
                            if (ddlFab.Items[i].Value.ToString().Contains("~"))
                                StrFabric += ddlFab.Items[i].Value.ToString().Split('~')[1].ToString() + ",";
                            else
                                StrFabric = StrFabric + ddlFab.Items[i].Text.ToString() + ",";
                        }
                    }
                }
            }
            tb_Product.Fabric = StrFabric.ToString();
            #endregion

            #region Header
            string StrHeader = "";
            if (chkHeader.Items.Count > 0)
            {
                for (int i = 0; i < chkHeader.Items.Count; i++)
                {
                    if (chkHeader.Items[i].Selected)
                    {
                        if (chkHeader.Items[i].Value.ToString().Contains("~"))
                            StrHeader += chkHeader.Items[i].Value.ToString().Split('~')[1].ToString() + ",";
                        else
                            StrHeader = StrHeader + chkHeader.Items[i].Text.ToString() + ",";
                    }
                }
            }
            tb_Product.Header = StrHeader.ToString();
            #endregion

            #region Feature
            string stylenm = "";
            if (ddlStyle.Items.Count > 0)
            {
                for (int l = 0; l < ddlStyle.Items.Count; l++)
                {
                    if (ddlStyle.Items[l].Selected == true)
                    {
                        if (ddlStyle.Items[l].Value.ToString().Contains("~"))
                            stylenm += ddlStyle.Items[l].Value.ToString().Split('~')[1].ToString() + ",";
                        else
                            stylenm += ddlStyle.Items[l].Text.ToString() + ",";
                    }
                }
            }
            tb_Product.Style = stylenm;
            #endregion

            if (string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()) && txtDisplayOrder.Text.Trim() == "")
                tb_Product.DisplayOrder = 0;
            else tb_Product.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
            tb_Product.IsNewArrival = chkNewArrival.Checked;

            tb_Product.IsFeatured = chkFeatured.Checked;
            tb_Product.GiftWrap = chkGiftWrap.Checked;
            tb_Product.IsAuthorizeRefund = chkIsAuthorizeRefund.Checked;
            tb_Product.IsBestSeller = chkIsBestSeller.Checked;
            tb_Product.IsSpecials = chkIsSpecial.Checked;
            tb_Product.Discontinue = chkIsDiscontinue.Checked;
            tb_Product.Ismadetoready = chkisreadymade.Checked;
            tb_Product.Ismadetoorder = chkisordermade.Checked;
            tb_Product.Ismadetomeasure = chkismadetomeasure.Checked;
            tb_Product.Ismadetoorderswatch = chkismadetoswatch.Checked;

            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit" && Request.QueryString["ID"] != null)
            {
                if (ddlDescription.SelectedValue == "1")
                {
                    tb_Product.Description = Convert.ToString((txtDescription.Text.Trim()));
                }
                // else
                //    tb_Product.Description = Convert.ToString((tb_Product.Description));
            }
            else
                tb_Product.Description = Convert.ToString((txtDescription.Text.Trim()));

            tb_Product.SETitle = txtSETitle.Text.Trim();
            tb_Product.SEKeywords = txtSEKeyword.Text.ToString().Trim();
            tb_Product.SEDescription = txtSEDescription.Text.Trim();
            tb_Product.ToolTip = txtToolTip.Text.Trim();


            if (!string.IsNullOrEmpty(txtRelProducts.Text.ToString()))
                tb_Product.RelatedProduct = txtRelProducts.Text.ToString();
            else tb_Product.RelatedProduct = "";

            tb_Product.TagName = ddlTagName.SelectedValue.ToString();
            tb_Product.SalePriceTag = ddlSalePriceTag.SelectedValue.ToString();

            if (!string.IsNullOrEmpty(txtNewArrivalFromDate.Text.ToString()))
                tb_Product.IsNewArrivalFromDate = Convert.ToDateTime(txtNewArrivalFromDate.Text.ToString());
            if (!string.IsNullOrEmpty(txtNewArrivalToDate.Text.ToString()))
                tb_Product.IsNewArrivalToDate = Convert.ToDateTime(txtNewArrivalToDate.Text.ToString());

            if (!string.IsNullOrEmpty(txtIsFeaturedFromDate.Text.ToString()))
                tb_Product.IsFeaturedFromDate = Convert.ToDateTime(txtIsFeaturedFromDate.Text.ToString());
            if (!string.IsNullOrEmpty(txtIsFeaturedToDate.Text.ToString()))
                tb_Product.IsFeaturedToDate = Convert.ToDateTime(txtIsFeaturedToDate.Text.ToString());

            if (!string.IsNullOrEmpty(txtBestSellerFromDate.Text.ToString()))
                tb_Product.IsBestSellerFromDate = Convert.ToDateTime(txtBestSellerFromDate.Text.ToString());
            if (!string.IsNullOrEmpty(txtBestSellerToDate.Text.ToString()))
                tb_Product.IsBestSellerToDate = Convert.ToDateTime(txtBestSellerToDate.Text.ToString());

            if (!string.IsNullOrEmpty(txtFreeShippingFromDate.Text.ToString()))
                tb_Product.IsFreeShippingFromDate = Convert.ToDateTime(txtFreeShippingFromDate.Text.ToString());
            if (!string.IsNullOrEmpty(txtFreeShippingToDate.Text.ToString()))
                tb_Product.IsFreeShippingToDate = Convert.ToDateTime(txtFreeShippingToDate.Text.ToString());



            tb_Product.OptionalAccessories = txtOptionalAccesories.Text.Trim().ToString();
            //tb_Product.isFree19InchLCD = chkFree19Inch.Checked;
            //tb_Product.isFree22InchLCD = chkFree22Inch.Checked;
            //tb_Product.isFreeDVRUpgrade = chkFreeDVRUpgrade.Checked;
            //tb_Product.isFreeHDUpgrade = chkFreeHDUpgrade.Checked;
            //tb_Product.isFreeDVDBurner = chkFreeDVDBurner.Checked;
            //tb_Product.isFreeTechSupport = chkFreeTechSupport.Checked;
            //tb_Product.is3YearDVRWarranty = chk3YearDVRWarranty.Checked;
            tb_Product.isSatisfactionGuaranteed = chkSatisfactionGuaranteed.Checked;
            tb_Product.isTabbingDisplay = true;
            tb_Product.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
            tb_Product.UpdatedOn = System.DateTime.Now;
            tb_Product.MarryProducts = txtmarry.Text.ToString();

            if (txtPricePerInch.Text.Trim().Length == 0)
                txtPricePerInch.Text = AppLogic.AppConfigs("PerInchPrice");

            if (txtPricePerInch.Text.Trim() != "")
                tb_Product.PerInchPrice = Convert.ToDecimal(txtPricePerInch.Text.Trim());
            else txtPricePerInch.Text = AppLogic.AppConfigs("PerInchPrice");

            if (txtAdditionalCharge.Text.Trim().Length == 0)
                txtAdditionalCharge.Text = AppLogic.AppConfigs("AdditionalCharge");

            if (txtAdditionalCharge.Text.Trim() != "")
                tb_Product.AdditionalCharge = Convert.ToDecimal(txtAdditionalCharge.Text.Trim());
            else txtAdditionalCharge.Text = AppLogic.AppConfigs("AdditionalCharge");

            if (txtInchHeaderHeme.Text.Trim().Length == 0)
                txtInchHeaderHeme.Text = AppLogic.AppConfigs("YardHeaderandhem");

            if (txtInchHeaderHeme.Text.Trim() != "")
                tb_Product.YardHeaderandhem = Convert.ToInt32(txtInchHeaderHeme.Text.Trim());
            else txtInchHeaderHeme.Text = AppLogic.AppConfigs("YardHeaderandhem");

            if (txtFabric.Text.Trim().Length == 0)
                txtFabric.Text = AppLogic.AppConfigs("FabricInch");

            if (txtFabric.Text.Trim() != "")
                tb_Product.FabricInch = Convert.ToInt32(txtFabric.Text.Trim());
            else txtFabric.Text = AppLogic.AppConfigs("FabricInch");


            return tb_Product;
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

            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            ProductIconPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Icon/");
            ProductMediumPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Medium/");
            ProductLargePath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Large/");
            ProductMicroPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Micro/");

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
                    CreateImage("Fabricimage", FileName);
                    CreateImage("Large", FileName);
                    if (ddlItemType.SelectedItem.Text.ToString().ToLower().Trim() == "swatch")
                    {
                        CreateImage("color/icon", FileName);
                    }


                }
                catch (Exception ex)
                {
                    lblMsg.Text += "<br />" + ex.Message;
                }
                finally
                {

                    //  string FileName2 = ViewState["File"].ToString();
                    if ((ViewState["File1"] != null && ViewState["File"].ToString().Equals(ViewState["File1"].ToString())) || (ViewState["File2"] != null && ViewState["File"].ToString().Equals(ViewState["File2"].ToString())) || (ViewState["File3"] != null && ViewState["File"].ToString().Equals(ViewState["File3"].ToString())) || (ViewState["File4"] != null && ViewState["File"].ToString().Equals(ViewState["File4"].ToString())) || (ViewState["File5"] != null && ViewState["File"].ToString().Equals(ViewState["File5"].ToString())))
                    {

                    }
                    else
                    {
                        DeleteTempFile("icon");
                    }
                }
            }
        }
        protected void SaveImageAlt(string FileName, string tempFile, System.Web.UI.HtmlControls.HtmlImage img1option, string delfile)
        {
            if (Request.QueryString["CloneID"] == null && (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString())))
            {
                AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
            }

            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            ProductIconPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Icon/");
            ProductMediumPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Medium/");
            ProductLargePath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Large/");
            ProductMicroPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Micro/");

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

            if (img1option.Src.Contains(ProductTempPath))
            {
                try
                {
                    CreateImageAlt("Medium", FileName, img1option, delfile);
                    CreateImageAlt("Icon", FileName, img1option, delfile);
                    CreateImageAlt("Micro", FileName, img1option, delfile);
                    CreateImageAlt("Large", FileName, img1option, delfile);
                }
                catch (Exception ex)
                {
                    lblMsg.Text += "<br />" + ex.Message;
                }
                finally
                {


                    if (FileName.Contains("_1.jpg"))
                    {

                        if ((ViewState["File2"] != null && tempFile.ToString().Equals(ViewState["File2"].ToString())) || (ViewState["File3"] != null && tempFile.ToString().Equals(ViewState["File3"].ToString())) || (ViewState["File4"] != null && tempFile.ToString().Equals(ViewState["File4"].ToString())) || (ViewState["File5"] != null && tempFile.ToString().Equals(ViewState["File5"].ToString())))
                        {

                        }
                        else
                        {
                            DeleteTempFileAlt("icon", tempFile);
                        }
                    }
                    else if (FileName.Contains("_2.jpg"))
                    {
                        if ((ViewState["File3"] != null && tempFile.ToString().Equals(ViewState["File3"].ToString())) || (ViewState["File4"] != null && tempFile.ToString().Equals(ViewState["File4"].ToString())) || (ViewState["File5"] != null && tempFile.ToString().Equals(ViewState["File5"].ToString())))
                        {

                        }
                        else
                        {
                            DeleteTempFileAlt("icon", tempFile);
                        }
                    }
                    else if (FileName.Contains("_3.jpg"))
                    {
                        if ((ViewState["File4"] != null && tempFile.ToString().Equals(ViewState["File4"].ToString())) || (ViewState["File5"] != null && tempFile.ToString().Equals(ViewState["File5"].ToString())))
                        {

                        }
                        else
                        {
                            DeleteTempFileAlt("icon", tempFile);
                        }

                    }
                    else if (FileName.Contains("_4.jpg"))
                    {
                        if ((ViewState["File5"] != null && tempFile.ToString().Equals(ViewState["File5"].ToString())))
                        {

                        }
                        else
                        {
                            DeleteTempFileAlt("icon", tempFile);
                        }
                    }
                    else if (FileName.Contains("_5.jpg"))
                    {
                        DeleteTempFileAlt("icon", tempFile);
                    }

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
                CommonComponent.ErrorLog("Product.aspx", ex.Message, ex.StackTrace);
            }
        }
        protected void DeleteTempFileAlt(string strsize, string tempfile)
        {
            try
            {
                if (strsize == "icon")
                {
                    string path = string.Empty;
                    if (tempfile != null && tempfile.ToString().Trim().Length > 0)
                    {
                        path = Server.MapPath(ProductTempPath + tempfile.ToString());
                    }

                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("Product.aspx", ex.Message, ex.StackTrace);
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
                    case "fabricimage":
                        strFilePath = Server.MapPath(ProductIconPath.ToLower().Replace("/icon/", "/fabricimage/") + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductIconPath.Replace("/icon/", "/fabricimage/") + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "color/icon":
                        strFilePath = Server.MapPath(ProductIconPath.ToLower().Replace("/icon/", "/color/icon/") + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductIconPath.Replace("/icon/", "/color/icon/") + ViewState["DelImage"].ToString());
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
        protected void CreateImageAlt(string Size, string FileName, System.Web.UI.HtmlControls.HtmlImage img1option, string delfile)
        {
            try
            {
                string strFile = null;
                String strPath = "";
                if (img1option.Src.ToString().IndexOf("?") > -1)
                {
                    strPath = img1option.Src.Split('?')[0];
                }
                else
                {
                    strPath = img1option.Src.ToString();
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
                            if (delfile != null && delfile.ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductLargePath + delfile.ToString());
                            }
                        }
                        break;
                    case "medium":
                        strFilePath = Server.MapPath(ProductMediumPath + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (delfile != null && delfile.ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductMediumPath + delfile.ToString());
                            }
                        }
                        break;
                    case "icon":
                        strFilePath = Server.MapPath(ProductIconPath + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (delfile != null && delfile.ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductIconPath + delfile.ToString());
                            }
                        }
                        break;
                    case "micro":
                        strFilePath = Server.MapPath(ProductMicroPath + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (delfile != null && delfile.ToString().Trim().Length > 0)
                            {
                                // DeleteImage(ProductMicroPath + ViewState["DelImage"].ToString());
                                DeleteImage(ProductMicroPath + delfile.ToString());
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
                    if (txtIconWidth.Text.Trim() != "" && txtIconHeigth.Text.Trim() != "")
                    {
                        Size CustomIconSize = new Size(Convert.ToInt32(txtIconWidth.Text.Trim()), Convert.ToInt32(txtIconHeigth.Text.Trim()));
                        finHeight = CustomIconSize.Height;
                        finWidth = CustomIconSize.Width;

                    }
                    else
                    {
                        finHeight = thumbNailSizeIcon.Height;
                        finWidth = thumbNailSizeIcon.Width;
                    }

                    break;
                case "micro":
                    finHeight = thumbNailSizeMicro.Height;
                    finWidth = thumbNailSizeMicro.Width;
                    break;
                case "color/icon":
                    finHeight = 100;
                    finWidth = 100;
                    break;
                case "fabricimage":
                    finHeight = 100;
                    finWidth = 100;
                    break;
                case "large":
                    try
                    {
                        System.Drawing.Image objlarge = System.Drawing.Image.FromFile(strFile);
                        finHeight = objlarge.Height;
                        finWidth = objlarge.Width;
                        break;
                    }
                    catch
                    {

                    }
                    break;

            }
            if (Size.ToLower().Trim() == "large" || Size.ToLower().Trim() == "medium")
            {
                File.Copy(strFile, strFilePath, true);
                //try
                //{
                //    CompressimagePanda objcompress = new CompressimagePanda();
                //    objcompress.compressimage(strFilePath);
                //}
                //catch
                //{

                //}
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
            int ss = 0;
            int se = 0;
            if (imgVisol.Height >= FinHeight && imgVisol.Width >= FinWidth)
            {
                float resizePercentHeight = 0;
                float resizePercentWidth = 0;
                resizePercentHeight = (FinHeight * 100) / imgVisol.Height;
                resizePercentWidth = (FinWidth * 100) / imgVisol.Width;
                resizedWidth = FinWidth;
                resizedHeight = FinHeight;
                //if (resizePercentHeight < resizePercentWidth)
                //{
                //    resizedHeight = FinHeight;
                //    resizedWidth = (int)Math.Round(resizePercentHeight * imgVisol.Width / 100.0);
                //}
                //if (resizePercentHeight >= resizePercentWidth)
                //{
                //    resizedWidth = FinWidth;
                //    resizedHeight = (int)Math.Round(resizePercentWidth * imgVisol.Height / 100.0);
                //}
                ss = 8;
                se = 8;
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
            Rectangle srcRect = new Rectangle(ss, se, sourceWidth, sourceHeight);
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
                //try
                //{
                //    CompressimagePanda objcompress = new CompressimagePanda();
                //    objcompress.compressimage(DestFileName);
                //}
                //catch
                //{

                //}
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
                //try
                //{
                //    CompressimagePanda objcompress = new CompressimagePanda();
                //    objcompress.compressimage(DestFileName);
                //}
                //catch
                //{

                //}
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
                //try
                //{
                //    CompressimagePanda objcompress = new CompressimagePanda();
                //    objcompress.compressimage(DestFileName);
                //}
                //catch
                //{

                //}
                CommonOperations.SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null)
            {
                extBMP.Save(DestFileName, CodecInfo, EncParms);
                extBMP.Dispose();
                //try
                //{
                //    CompressimagePanda objcompress = new CompressimagePanda();
                //    objcompress.compressimage(DestFileName);
                //}
                //catch
                //{

                //}
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
        /// Function for Add Product Category
        /// </summary>
        /// <returns>Returns the ArrayList</returns>
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
        /// <returns>Returns the ArrayList</returns>
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
        /// <param name="CategoryID">string CategoryID</param>
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

                    if (ViewState["getcategorylist"] != null)
                    {
                        DataSet dslist = new DataSet();
                        dslist = (DataSet)ViewState["getcategorylist"];
                        if (dslist != null && dslist.Tables.Count > 0 && dslist.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] dr = dslist.Tables[0].Select("ProductId='" + ProductID + "' and CategoryId=" + CatID + " ");
                            if (dr.Length > 0)
                            {
                                Int32 dddd = 999;
                                try
                                {
                                    if (!string.IsNullOrEmpty(dr[0]["DisplayOrder"].ToString()))
                                    {
                                        Int32.TryParse(dr[0]["DisplayOrder"].ToString().Trim(), out dddd);
                                    }
                                }
                                catch
                                {

                                }



                                tbProductCategory.DisplayOrder = dddd;
                            }
                            else
                            {
                                tbProductCategory.DisplayOrder = 999;
                            }
                        }
                        else
                        {
                            tbProductCategory.DisplayOrder = 999;
                        }
                    }
                    else
                    {
                        DataSet dsDisplay = ProductComponent.GetDisplayOrderByProductIDAndCategoryID(Convert.ToInt32(ProductID), Convert.ToInt32(CatID));
                        if (dsDisplay != null && dsDisplay.Tables.Count > 0 && dsDisplay.Tables[0].Rows.Count > 0)
                        {

                            Int32 dddd = 999;
                            try
                            {


                                if (!string.IsNullOrEmpty(dsDisplay.Tables[0].Rows[0]["DisplayOrder"].ToString()))
                                {
                                    Int32.TryParse(dsDisplay.Tables[0].Rows[0]["DisplayOrder"].ToString().Trim(), out dddd);
                                }

                            }
                            catch
                            {

                            }
                            tbProductCategory.DisplayOrder = dddd;
                        }
                        else
                        {
                            tbProductCategory.DisplayOrder = 999;
                        }
                    }


                    ProductComponent.InsertProductCategory(tbProductCategory);
                }
            }
        }

        /// <summary>
        ///  Upload Image Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (fuProductIcon.FileName.Length > 0 && Path.GetExtension(fuProductIcon.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fuProductIcon.FileName.Length > 0)
                {
                    bool checksize = false;
                    checksize = CheckImageSize(fuProductIcon, 1500, 1500);
                    if (!checksize)
                    {
                        ViewState["File"] = null;
                        lblMsg.Text = "Product image upload size must be width: 1500px and height: 1500px.";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_fuProductIcon');});", true);
                    }
                    else
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
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
        /// Description Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlDescription_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FieldNm = "";
            string Descri = "";
            DataSet dsDesc = new DataSet();

            if (ddlDescription.SelectedValue.ToString() != "")
            {
                if (Request.QueryString["Mode"] != null && !string.IsNullOrEmpty(Request.QueryString["Mode"].ToString()) && Request.QueryString["ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ID"].ToString()) && Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
                {
                    tb_Product tb_Product = new tb_Product();
                    dsDesc = ProductComponent.GetProductByProductID(Convert.ToInt32(Request.QueryString["ID"].ToString()));

                    string DescDetails = "";
                    string TitleNm = "";
                    if (dsDesc != null && dsDesc.Tables.Count > 0 && dsDesc.Tables[0].Rows.Count > 0)
                    {
                        if (ddlDescription.SelectedValue.ToString() == "1")
                        {
                            DescDetails = Server.HtmlDecode(dsDesc.Tables[0].Rows[0]["Description"].ToString().Trim());
                            TitleNm = dsDesc.Tables[0].Rows[0]["TabTitle1"].ToString().Trim();
                        }
                        if (ddlDescription.SelectedValue.ToString() == "2")
                        {
                            DescDetails = Server.HtmlDecode(dsDesc.Tables[0].Rows[0]["TabDesc2"].ToString().Trim());
                            TitleNm = dsDesc.Tables[0].Rows[0]["TabTitle2"].ToString().Trim();
                        }
                        if (ddlDescription.SelectedValue.ToString() == "3")
                        {
                            DescDetails = Server.HtmlDecode(dsDesc.Tables[0].Rows[0]["TabDesc3"].ToString().Trim());
                            TitleNm = dsDesc.Tables[0].Rows[0]["TabTitle3"].ToString().Trim();
                        }
                        if (ddlDescription.SelectedValue.ToString() == "4")
                        {
                            DescDetails = Server.HtmlDecode(dsDesc.Tables[0].Rows[0]["TabDesc4"].ToString().Trim());
                            TitleNm = dsDesc.Tables[0].Rows[0]["TabTitle4"].ToString().Trim();
                        }
                        if (ddlDescription.SelectedValue.ToString() == "5")
                        {
                            DescDetails = Server.HtmlDecode(dsDesc.Tables[0].Rows[0]["TabDesc5"].ToString().Trim());
                            TitleNm = dsDesc.Tables[0].Rows[0]["TabTitle5"].ToString().Trim();
                        }

                        txtDescription.Text = DescDetails;
                        txtTitleDesc.Text = TitleNm;
                    }
                    else
                    {
                        txtDescription.Text = "";
                        txtTitleDesc.Text = "";
                    }
                }
            }
        }

        /// <summary>
        ///  Save Other Description Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkSaveDesc_Click1(object sender, EventArgs e)
        {
            tb_Product tb_Product = new tb_Product();
            ProductComponent objProduct = new ProductComponent();

            tb_Product = objProduct.GetAllProductDetailsbyProductID(Convert.ToInt32(Request.QueryString["ID"]));

            if (ddlDescription.SelectedValue.ToString().ToLower() == "1")
            {
                tb_Product.Description = txtDescription.Text.ToString();
                tb_Product.TabTitle1 = txtTitleDesc.Text.ToString().ToString();
            }
            else if (ddlDescription.SelectedValue.ToString().ToLower() == "2")
            {
                tb_Product.TabDesc2 = txtDescription.Text.ToString();
                tb_Product.TabTitle2 = txtTitleDesc.Text.ToString().ToString();
            }
            else if (ddlDescription.SelectedValue.ToString().ToLower() == "3")
            {
                tb_Product.TabDesc3 = txtDescription.Text.ToString();
                tb_Product.TabTitle3 = txtTitleDesc.Text.ToString().ToString();
            }
            else if (ddlDescription.SelectedValue.ToString().ToLower() == "4")
            {
                tb_Product.TabDesc4 = txtDescription.Text.ToString();
                tb_Product.TabTitle4 = txtTitleDesc.Text.ToString().ToString();
            }
            else if (ddlDescription.SelectedValue.ToString().ToLower() == "5")
            {
                tb_Product.TabDesc5 = txtDescription.Text.ToString();
                tb_Product.TabTitle5 = txtTitleDesc.Text.ToString().ToString();
            }
            if (Convert.ToInt32(ProductComponent.UpdateProduct(tb_Product)) > 0)
            {

            }
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
        ///  Upload PDF File Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUploadPdfFile_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            if (FileUploadPdf.FileName.Length > 0 && Path.GetExtension(FileUploadPdf.FileName.ToString().ToLower()) == ".pdf".ToString().ToLower())
                Flag = true;

            if (Flag)
            {
                if (FileUploadPdf.FileName.Length > 0)
                {
                    ViewState["PdfFile"] = FileUploadPdf.FileName.ToString();
                    FileUploadPdf.SaveAs(Server.MapPath(ProductTempPath) + FileUploadPdf.FileName);
                }
            }
            if (FileUploadPdf.FileName.Length <= 0)
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg1", "$(document).ready( function() {jAlert('Upload valid [.Pdf] file', 'Message','ContentPlaceHolder1_FileUploadPdf');});", true);

            if (Path.GetExtension(FileUploadPdf.FileName.ToString().ToLower()) != ".pdf".ToString().ToLower())
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('Only .Pdf files are allowed', 'Message','ContentPlaceHolder1_FileUploadPdf');});", true);
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
                    if (txtInventory.ReadOnly)
                    {
                        if (string.IsNullOrEmpty(txtInventory.Text.ToString()) || txtInventory.Text.Trim() == "0")
                        {
                            txtInventory.Attributes.Add("readonly", "false");
                            txtInventory.Text = "0";
                            txtInventory.Attributes.Add("readonly", "true");
                        }

                    }
                    else
                    {
                        if (string.IsNullOrEmpty(txtInventory.Text.ToString()) || txtInventory.Text.Trim() == "0")
                        {
                            txtInventory.Text = "0";
                        }
                    }




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

        ///// <summary>
        /////  Upload Video Button Click Event
        ///// </summary>
        ///// <param name="sender">object sender</param>
        ///// <param name="e">EventArgs e</param>
        //protected void btnUploadvideo_Click(object sender, ImageClickEventArgs e)
        //{
        //    if (string.IsNullOrEmpty(fuProductVideo.FileName.Trim()))
        //    {

        //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Select Video.', 'Message');});", true);
        //        return;
        //    }

        //    else if (!string.IsNullOrEmpty(fuProductVideo.FileName.Trim()))
        //    {
        //        if (Path.GetExtension(fuProductVideo.FileName.Trim()).ToLower() != ".flv")
        //        {
        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please upload flv Video file.', 'Message');});", true);
        //            return;
        //        }
        //    }

        //    try
        //    {
        //        string videopathtemp = "";
        //        string productid = "";
        //        FileInfo VideoName = new FileInfo(fuProductVideo.FileName);
        //        productid = System.IO.Path.GetFileNameWithoutExtension(VideoName.Name.ToString());
        //        videopathtemp = AppLogic.AppConfigs("VideoTempPath").ToString();
        //        string file = videopathtemp + productid + ".flv";
        //        if (!Directory.Exists(Server.MapPath(videopathtemp)))
        //            Directory.CreateDirectory(Server.MapPath(videopathtemp));

        //        if (File.Exists(Server.MapPath(file)))
        //            File.Delete(Server.MapPath(file));

        //        lblVideoName.Text = fuProductVideo.FileName;
        //        fuProductVideo.SaveAs(Server.MapPath(file));
        //        ViewState["VideoName"] = productid.ToString() + ".flv";

        //        trvideodelete.Visible = true;
        //        btndeleteVideo.Visible = true;
        //        lblVideoName.Visible = true;
        //    }
        //    catch { }
        //}
        /// <summary>
        ///  Upload Video Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUploadvideo_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(fuProductVideo.FileName.Trim()))
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Select Video.', 'Message');});", true);
                return;
            }

            else if (!string.IsNullOrEmpty(fuProductVideo.FileName.Trim()))
            {
                if (Path.GetExtension(fuProductVideo.FileName.Trim()).ToLower() != ".mp4")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please upload mp4 Video file.', 'Message');});", true);
                    return;
                }
            }

            try
            {
                string videopathtemp = "";
                string productid = "";
                FileInfo VideoName = new FileInfo(fuProductVideo.FileName);
                productid = System.IO.Path.GetFileNameWithoutExtension(VideoName.Name.ToString());
                videopathtemp = AppLogic.AppConfigs("VideoTempPath").ToString();
                string file = videopathtemp + productid + ".mp4";
                if (!Directory.Exists(Server.MapPath(videopathtemp)))
                    Directory.CreateDirectory(Server.MapPath(videopathtemp));

                if (File.Exists(Server.MapPath(file)))
                    File.Delete(Server.MapPath(file));

                lblVideoName.Text = fuProductVideo.FileName;
                fuProductVideo.SaveAs(Server.MapPath(file));
                ViewState["VideoName"] = productid.ToString() + ".mp4";

                trvideodelete.Visible = true;
                btndeleteVideo.Visible = true;
                lblVideoName.Visible = true;
            }
            catch { }
        }
        /// <summary>
        ///  Delete Video Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btndeleteVideo_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["VideoName"] = null;
            trvideodelete.Visible = false;
            btndeleteVideo.Visible = false;
            lblVideoName.Visible = false;
        }

        /// <summary>
        /// Get the Ware House List
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
        /// Ware House Gridview Row Data Bound Event
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
        /// Saves the Inventory in Warehouse.
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="mode">int mode</param>
        protected void SaveInventoryInWarehouse(int ProductID, int mode)
        {
            try
            {
                ProductComponent objProductComp = new ProductComponent();
                int InventoryTotal = 0;
                foreach (GridViewRow row in grdWarehouse.Rows)
                {
                    int Inventory = 0;
                    RadioButton rdowarehouse = (RadioButton)row.FindControl("rdowarehouse");
                    Label lblWarehouse = (Label)row.FindControl("lblWarehouseID");
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
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString() + " " + ex.StackTrace.ToString());

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
                    url = "Product.aspx?StoreID=" + CurrentStoreID.ToString() + "&ID=" + Request.QueryString["oldid"].ToString() + "&CloneID=" + Request.QueryString["CloneID"].ToString();
                    Response.Redirect(url);
                }
            }
            catch { }

        }

        /// <summary>
        /// Determines Whether is Product Available the Specified Pid
        /// </summary>
        /// <param name="pid">int pid</param>
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
                    string checkrealupc = Convert.ToString(CommonComponent.GetScalarCommonData("Select SKU from tb_UPCMasterReal where UPC='" + txtUPC.Text.Trim() + "' and SKU ='" + txtSKU.Text.Trim() + "'"));
                    if (checkrealupc != null && checkrealupc.Length > 0)
                    {
                        lbtngetUPC.Visible = false;
                    }
                    else
                    {
                        StrUPCReal = Convert.ToString(CommonComponent.GetScalarCommonData("select TOP 1 UPC from tb_UPCMasterReal where sku is null ORDER BY NEWID() "));
                        if (StrUPCReal != null && StrUPCReal.Length > 0)
                        {
                            txtUPC.Text = StrUPCReal;
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
                        txtUPC.Text = StrUPCReal;
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
            try
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
            catch { }
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
            DataSet dsproductSKU = new DataSet();

            if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "")
            {
                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_ProductAssembly Where RefProductID=" + Request.QueryString["ID"].ToString() + "")) > 0)
                {
                    string StrQuery = "Select tb_ProductAssembly.ProductID,tb_Product.SKU as SKU,tb_Product.Name as Name,Quantity from tb_ProductAssembly Inner join tb_Product on tb_Product.ProductID=tb_ProductAssembly.ProductID Where RefProductID=" + Request.QueryString["ID"].ToString() + " " +
                                      " Union All" +
                                      " Select ProductID,SKU,Name,'1' as Quantity FROM dbo.tb_Product WHERE Isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND StoreID=" + Convert.ToInt32(Request.QueryString["StoreID"]) + " and SKU IN('" + hdnProductALLSku.Value.Replace(",", "','") + "')";
                    dsproductSKU = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                }
            }
            else
            {
                dsproductSKU = CommonComponent.GetCommonDataSet("SELECT ProductID,SKU,Name,'1' as Quantity FROM dbo.tb_Product WHERE Isnull(Active,0)=1 AND Isnull(Deleted,0)=0 AND StoreID=" + Convert.ToInt32(Request.QueryString["StoreID"]) + " and SKU IN('" + hdnProductALLSku.Value.Replace(",", "','") + "')");
            }
            if (dsproductSKU != null && dsproductSKU.Tables.Count > 0 && dsproductSKU.Tables[0].Rows.Count > 0)
            {
                //grdAssembler.DataSource = dsproductSKU;
                //grdAssembler.DataBind();
                string[] StrSku = hdnProductALLSku.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                string[] StrQty = hdnProductALLQty.Value.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

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
                            if (StrSku.Length > 0)
                            {
                                for (int k = 0; k < StrSku.Length; k++)
                                {
                                    if (StrSku[k].ToString().ToLower() == dsproductSKU.Tables[0].Rows[i]["SKU"].ToString().ToLower())
                                    {
                                        if (StrQty.Length > 0 && StrQty[k].Length > 0)
                                        {
                                            foreach (DataRow dr in drr)
                                            {
                                                if (dr["SKu"].ToString().ToLower() == StrSku[k].ToString().ToLower())
                                                {
                                                    dr["Quantity"] = StrQty[k].ToString();
                                                    dtAssembler.AcceptChanges();
                                                    break;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            drPackage = dtAssembler.NewRow();
                            // drPackage["ShippingCartID"] = strShoppingCartID;
                            drPackage["ProductID"] = dsproductSKU.Tables[0].Rows[i]["ProductID"];
                            drPackage["Name"] = dsproductSKU.Tables[0].Rows[i]["Name"];
                            drPackage["SKU"] = dsproductSKU.Tables[0].Rows[i]["SKU"];

                            if (StrSku.Length > 0)
                            {
                                for (int k = 0; k < StrSku.Length; k++)
                                {
                                    if (StrSku[k].ToString().ToLower() == dsproductSKU.Tables[0].Rows[i]["SKU"].ToString().ToLower())
                                    {
                                        drPackage["Quantity"] = StrQty[k].ToString();
                                        break;
                                    }
                                }
                            }
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
                        //  System.Web.UI.ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "alert('Enter Valid Quantity!');", true);
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
                    if (Request.QueryString["ID"] != null)
                    {
                        CommonComponent.ExecuteCommonData("Delete from tb_ProductAssembly where RefProductID=" + Request.QueryString["ID"].ToString() + " and Productid=" + drr[i]["ProductID"].ToString() + "");
                    }
                    drr[i].Delete();
                }
                dtAssembler.AcceptChanges();
                hdnProductALLSku.Value = "";
                hdnProductALLQty.Value = "";
                for (int k = 0; k <= dtAssembler.Rows.Count - 1; k++)
                {
                    hdnProductALLSku.Value = hdnProductALLSku.Value + dtAssembler.Rows[k]["SKU"].ToString() + ",";
                    hdnProductALLQty.Value = hdnProductALLQty.Value + dtAssembler.Rows[k]["Quantity"].ToString() + ",";
                }

                if (hdnProductALLSku.Value.Length > 0)
                {
                    hdnProductALLSku.Value = hdnProductALLSku.Value.Substring(0, hdnProductALLSku.Value.Length - 1);
                    hdnProductALLQty.Value = hdnProductALLQty.Value.Substring(0, hdnProductALLQty.Value.Length - 1);
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

            try
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
            catch { }
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
            if (dtAssembler != null && dtAssembler.Rows.Count > 0)
            {
                CommonComponent.ExecuteCommonData("Delete from tb_ProductAssembly where RefProductID=" + ProductID + "");
            }
        }
        public void DeleteDropShipData(Int32 ProductID)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                CommonComponent.ExecuteCommonData("Delete from tb_ProductVendorSKU where ProductID=" + ProductID + "");
            }
        }
        #endregion

        #region Bind Pattern,color,Style related Details
        protected void BindProductStylePrice(string Mode)
        {
            DataSet dtSearch = new DataSet();
            if (Request.QueryString["Id"] != null && Request.QueryString["CloneID"] == null)
            {
                dtSearch = CommonComponent.GetCommonDataSet(@"SELECT * FROM (Select ISNULL(ProductStyleId,0) ProductStyleId,ISNULL(SearchValue,'') as StyleName,ISNULL(tb_ProductStylePrice.AdditionalPrice,0) as AdditionalPrice,ISNULL(tb_ProductStylePrice.Active,0) Active,ISNULL(tb_ProductStylePrice.PerInch,0) as PerInch,isnull(tb_ProductSearchType.DisplayOrder,999) as DisplayOrder,ISNULL(tb_ProductStylePrice.YardHeaderandhem,0) as YardHeaderandhem,ISNULL(tb_ProductStylePrice.FabricInch,0) as FabricInch,ISNULL(dbo.tb_ProductStylePrice.FabricWidth,0)  AS FabricWidth ,ISNULL(dbo.tb_ProductStylePrice.DividedWidth,0)  AS DividedWidth from tb_ProductSearchType 
                                                         INNER Join tb_ProductStylePrice on tb_ProductStylePrice.Style=tb_ProductSearchType.SearchValue 
                                                         where SearchType = 6 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0 AND tb_ProductStylePrice.ProductId =" + Request.QueryString["Id"].ToString() + @"
                                                         
                                                UNION         
                                                       Select 0 as ProductStyleId,ISNULL(SearchValue,'') as StyleName,0 as AdditionalPrice,0 as Active,0 as PerInch,isnull(tb_ProductSearchType.DisplayOrder,9999) as DisplayOrder,0 as YardHeaderandhem,0 as FabricInch,0  AS FabricWidth ,0 AS DividedWidth from tb_ProductSearchType 
                                                      where SearchType = 6 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0  AND tb_ProductSearchType.SearchValue not in 
                                                      (SELECT Style FROM  tb_ProductStylePrice WHERE tb_ProductStylePrice.ProductId =" + Request.QueryString["Id"].ToString() + @")) as A

                                                           
                                                          Order by ISNULL(A.DisplayOrder,999)");
            }
            else if (Request.QueryString["Id"] != null && Request.QueryString["CloneID"] != null)
            {
                dtSearch = CommonComponent.GetCommonDataSet(@"SELECT * FROM (Select 0 as ProductStyleId,ISNULL(SearchValue,'') as StyleName,ISNULL(tb_ProductStylePrice.AdditionalPrice,0) as AdditionalPrice,ISNULL(tb_ProductStylePrice.Active,0) Active,ISNULL(tb_ProductStylePrice.PerInch,0) as PerInch,isnull(tb_ProductSearchType.DisplayOrder,999) as DisplayOrder,ISNULL(tb_ProductStylePrice.YardHeaderandhem,0) as YardHeaderandhem,ISNULL(tb_ProductStylePrice.FabricInch,0) as FabricInch,ISNULL(dbo.tb_ProductStylePrice.FabricWidth,0)  AS FabricWidth ,ISNULL(dbo.tb_ProductStylePrice.DividedWidth,0)  AS DividedWidth from tb_ProductSearchType 
                                                         INNER Join tb_ProductStylePrice on tb_ProductStylePrice.Style=tb_ProductSearchType.SearchValue 
                                                         where SearchType = 6 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0 AND tb_ProductStylePrice.ProductId =" + Request.QueryString["Id"].ToString() + @"
                                                         
                                               UNION         
                                                       Select 0 as ProductStyleId,ISNULL(SearchValue,'') as StyleName,0 as AdditionalPrice,0 as Active,0 as PerInch,isnull(tb_ProductSearchType.DisplayOrder,9999) as DisplayOrder,0 as YardHeaderandhem,0 as FabricInch,0  AS FabricWidth ,0 AS DividedWidth from tb_ProductSearchType 
                                                      where SearchType = 6 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0  AND tb_ProductSearchType.SearchValue not in 
                                                      (SELECT Style FROM  tb_ProductStylePrice WHERE tb_ProductStylePrice.ProductId =" + Request.QueryString["Id"].ToString() + @")) as A

                                                           
                                                          Order by ISNULL(A.DisplayOrder,999)");
            }
            else
            {
                dtSearch = CommonComponent.GetCommonDataSet(@"Select 0 as ProductStyleId,ISNULL(SearchValue,'') as StyleName,0 as AdditionalPrice,0 as Active,0 as PerInch,isnull(tb_ProductSearchType.DisplayOrder,9999) as DisplayOrder,0 as YardHeaderandhem,0 as FabricInch,0  AS FabricWidth ,0 AS DividedWidth from tb_ProductSearchType 
                                                      where SearchType = 6 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            }
            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                grdProductStyleType.DataSource = dtSearch.Tables[0];
                grdProductStyleType.DataBind();
                if (Mode == "edit")
                {
                    grdProductStyleType.Columns[6].Visible = true;
                    btnSaveStylePrice.Visible = false;
                }
                else
                {
                    //grdProductStyleType.Columns[6].Visible = false;
                    //btnSaveStylePrice.Visible = false;
                    btnSaveStylePrice.Visible = false;
                }
            }
            else
            {
                grdProductStyleType.DataSource = null;
                grdProductStyleType.DataBind();
            }
        }
        protected void BindProductOptionsPrice(string Mode)
        {
            DataSet dtSearch = new DataSet();
            if (Request.QueryString["Id"] != null && Request.QueryString["CloneID"] == null)
            {
                dtSearch = CommonComponent.GetCommonDataSet(@"SELECT * FROM (Select ISNULL(ProductOptionsId,0) ProductOptionsId,ISNULL(SearchValue,'') as StyleName,ISNULL(tb_ProductOptionsPrice.AdditionalPrice,0) as AdditionalPrice,tb_ProductSearchType.DisplayOrder,ISNULL(tb_ProductOptionsPrice.AdditionalPricePercentage,0) as AdditionalPricePercentage ,isnull(tb_ProductOptionsPrice.Active,0) as Active from tb_ProductSearchType 
                                                         INNER Join tb_ProductOptionsPrice on tb_ProductOptionsPrice.Options=tb_ProductSearchType.SearchValue 
                                                         where SearchType = 7 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0 AND tb_ProductOptionsPrice.ProductId =" + Request.QueryString["Id"].ToString() + @"
                                                         
                                                UNION         
                                                       Select 0 as ProductOptionsId,ISNULL(SearchValue,'') as StyleName,0 as AdditionalPrice,tb_ProductSearchType.DisplayOrder,0 as AdditionalPricePercentage,0 as Active from tb_ProductSearchType 
                                                      where SearchType = 7 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0  AND tb_ProductSearchType.SearchValue not in 
                                                      (SELECT Options FROM  tb_ProductOptionsPrice WHERE tb_ProductOptionsPrice.ProductId =" + Request.QueryString["Id"].ToString() + @")) as A

                                                           
                                                          Order by ISNULL(A.DisplayOrder,999)");
            }
            else if (Request.QueryString["Id"] != null && Request.QueryString["CloneID"] != null)
            {
                dtSearch = CommonComponent.GetCommonDataSet(@"SELECT * FROM (Select 0 as ProductOptionsId,ISNULL(SearchValue,'') as StyleName,ISNULL(tb_ProductOptionsPrice.AdditionalPrice,0) as AdditionalPrice,tb_ProductSearchType.DisplayOrder,ISNULL(tb_ProductOptionsPrice.AdditionalPricePercentage,0) as AdditionalPricePercentage,isnull(tb_ProductOptionsPrice.Active,0) as Active from tb_ProductSearchType 
                                                         INNER Join tb_ProductOptionsPrice on tb_ProductOptionsPrice.Options=tb_ProductSearchType.SearchValue 
                                                         where SearchType = 7 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0 AND tb_ProductOptionsPrice.ProductId =" + Request.QueryString["Id"].ToString() + @"
                                                         
                                                UNION         
                                                       Select 0 as ProductOptionsId,ISNULL(SearchValue,'') as StyleName,0 as AdditionalPrice,tb_ProductSearchType.DisplayOrder,0 as AdditionalPricePercentage,0 as Active from tb_ProductSearchType 
                                                      where SearchType = 7 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0  AND tb_ProductSearchType.SearchValue not in 
                                                      (SELECT Options FROM  tb_ProductOptionsPrice WHERE tb_ProductOptionsPrice.ProductId =" + Request.QueryString["Id"].ToString() + @")) as A 
                                                          Order by ISNULL(A.DisplayOrder,999)");
            }
            else
            {
                dtSearch = CommonComponent.GetCommonDataSet(@"Select 0 as ProductOptionsId,ISNULL(SearchValue,'') as StyleName,0 as AdditionalPrice,0 as Active,0 as PerInch,isnull(tb_ProductSearchType.DisplayOrder,9999) as DisplayOrder,0 as YardHeaderandhem,0 as FabricInch,0 as AdditionalPricePercentage from tb_ProductSearchType 
                                                      where SearchType = 7 and ISNULL(tb_ProductSearchType.Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            }
            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                grdoptions.DataSource = dtSearch.Tables[0];
                grdoptions.DataBind();
                if (Mode == "edit")
                {
                    // grdoptions.Columns[6].Visible = true;
                    btnSaveOptionPrice.Visible = true;
                }
                else
                {
                    // grdoptions.Columns[6].Visible = false;
                    btnSaveOptionPrice.Visible = false;
                }
            }
            else
            {
                grdoptions.DataSource = null;
                grdoptions.DataBind();
            }
        }
        protected void BindSearchDropdowns()
        {
            DataSet dtSearch = new DataSet();
            string strQuery = string.Empty;
            //strQuery = "Select ISNULL(Searchvalue,'') as Searchvalue,SearchID,'' as ColorSku,0 as Active from tb_ProductSearchType where SearchType=1 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue";

            strQuery = "select ColorID as SearchID,ColorName,'' as ColorSku,ImgName,Sename,DisplayOrderBy,ISNULL(Active,0) as Active,ISNULL(Deleted,0) as Deleted, ColorName + '~'+isnull(ImgName,'')  as SearchValue,Convert(varchar(10),ColorID) + '~'+ColorName as SearchValueID from tb_Color where ISNULL(Active,0)  = 1 and isnull(deleted,0) = 0 order by DisplayOrderBy ASC";
            dtSearch = CommonComponent.GetCommonDataSet(strQuery);
            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                ddlColors.DataSource = dtSearch.Tables[0];
                ddlColors.DataTextField = "SearchValue";
                ddlColors.DataValueField = "SearchValueID";
                ddlColors.DataBind();

                grdSecondaryColor.DataSource = dtSearch.Tables[0];
                grdSecondaryColor.DataBind();
                // trSecondaryColor.Visible = true;
                trSecondaryColor.Visible = false;
                ddlMainColor.DataSource = dtSearch.Tables[0];
                ddlMainColor.DataTextField = "ColorName";
                ddlMainColor.DataValueField = "SearchID";
                ddlMainColor.DataBind();
                ddlMainColor.Items.Insert(0, new ListItem("Select Color", "0"));
            }
            else
            {
                ddlMainColor.Items.Insert(0, new ListItem("Select Color", "0"));
                grdSecondaryColor.DataSource = null;
                grdSecondaryColor.DataBind();

                ddlColors.DataSource = null;
                ddlColors.DataBind();
                trSecondaryColor.Visible = false;
            }
            //ddlColors.Items.Insert(0, new ListItem("Select Color", "0"));

            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit" && Request.QueryString["ID"] != null)
            {
                btnSaveSecondaryColor.Visible = true;
            }
            else
            {
                btnSaveSecondaryColor.Visible = false;
            }

            dtSearch = new DataSet(); // Bind Patterns
            //dtSearch = CommonComponent.GetCommonDataSet("Select ISNULL(Searchvalue,'') as Searchvalue,SearchID from tb_ProductSearchType where SearchType=2 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue");
            dtSearch = CommonComponent.GetCommonDataSet("select PatternID as SearchID,PatternName,ImgName,Sename,DisplayOrderBy,ISNULL(Active,0) as Active,ISNULL(Deleted,0) as Deleted, PatternName + '~'+isnull(ImgName,'')  as SearchValue,Convert(varchar(10),PatternID) + '~'+PatternName as SearchValueID from tb_Pattern where ISNULL(Active,0)  = 1 and isnull(deleted,0) = 0 order by DisplayOrderBy ASC");
            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                ddlPattern.DataSource = dtSearch.Tables[0];
                ddlPattern.DataTextField = "Searchvalue";
                ddlPattern.DataValueField = "Searchvalue";
                ddlPattern.DataBind();
                ddlPattern.Style.Add("display", "none");
                ddlPatt.DataSource = dtSearch.Tables[0];
                ddlPatt.DataTextField = "SearchValue";
                ddlPatt.DataValueField = "SearchValueID";
                ddlPatt.DataBind();
            }
            else { ddlPattern.DataSource = null; ddlPattern.DataBind(); }
            ddlPattern.Items.Insert(0, new ListItem("Select Pattern", "0"));

            dtSearch = new DataSet(); // Bind Fabric
            //dtSearch = CommonComponent.GetCommonDataSet("Select ISNULL(Searchvalue,'') as Searchvalue,SearchID from tb_ProductSearchType where SearchType=3 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue");
            dtSearch = CommonComponent.GetCommonDataSet("select FabricID as SearchID,FabricName,ImgName,Sename,DisplayOrderBy,ISNULL(Active,0) as Active,ISNULL(Deleted,0) as Deleted, FabricName + '~'+isnull(ImgName,'')  as SearchValue,Convert(varchar(10),FabricID) + '~'+FabricName as SearchValueID from tb_Fabric where ISNULL(Active,0)  = 1 and isnull(deleted,0) = 0 order by DisplayOrderBy ASC");
            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                ddlFabric.DataSource = dtSearch.Tables[0];
                ddlFabric.DataTextField = "Searchvalue";
                ddlFabric.DataValueField = "SearchID";
                ddlFabric.DataBind();
                ddlFabric.Style.Add("display", "none");

                ddlFab.DataSource = dtSearch.Tables[0];
                ddlFab.DataTextField = "SearchValue";
                ddlFab.DataValueField = "SearchValueID";
                ddlFab.DataBind();
            }
            else { ddlFabric.DataSource = null; ddlFabric.DataBind(); }
            ddlFabric.Items.Insert(0, new ListItem("Select Fabric", "0"));

            dtSearch = new DataSet(); // Bind Feature
            //dtSearch = CommonComponent.GetCommonDataSet("Select ISNULL(Searchvalue,'') as Searchvalue,SearchID from tb_ProductSearchType where SearchType=4 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue");
            dtSearch = CommonComponent.GetCommonDataSet("select FeatureID as SearchID,FeatureName ,ImgName,Sename,DisplayOrderBy,ISNULL(Active,0) as Active,ISNULL(Deleted,0) as Deleted, FeatureName + '~'+isnull(ImgName,'') as SearchValue,Convert(varchar(10),FeatureID) + '~'+FeatureName as SearchValueID  from tb_Feature where ISNULL(Active,0)  = 1 and isnull(deleted,0) = 0 order by DisplayOrderBy ASC");
            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                ddlStyle.DataSource = dtSearch.Tables[0];
                ddlStyle.DataTextField = "SearchValue";
                ddlStyle.DataValueField = "SearchValueID";
                ddlStyle.DataBind();
            }
            else { ddlStyle.DataSource = null; ddlStyle.DataBind(); }

            dtSearch = new DataSet(); // Bind Feature
            //dtSearch = CommonComponent.GetCommonDataSet("Select ISNULL(Searchvalue,'') as Searchvalue,SearchID from tb_ProductSearchType where SearchType=4 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue");
            dtSearch = CommonComponent.GetCommonDataSet("select RoomID as SearchID,RoomName ,ImgName,Sename,DisplayOrderBy,ISNULL(Active,0) as Active,ISNULL(Deleted,0) as Deleted, RoomName + '~'+isnull(ImgName,'') as SearchValue,Convert(varchar(10),RoomID) + '~'+RoomName  as SearchValueID from tb_Room where ISNULL(Active,0)  = 1 and isnull(deleted,0) = 0 order by DisplayOrderBy ASC");
            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                ddlRoom.DataSource = dtSearch.Tables[0];
                ddlRoom.DataTextField = "SearchValue";
                ddlRoom.DataValueField = "SearchValueID";
                ddlRoom.DataBind();
            }
            else { ddlRoom.DataSource = null; ddlRoom.DataBind(); }

            //dtSearch = new DataSet(); // Bind Style
            //dtSearch = CommonComponent.GetCommonDataSet("Select ISNULL(Searchvalue,'') as Searchvalue,SearchID from tb_ProductSearchType where SearchType=4 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue");
            //if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            //{
            //    ddlStyle.DataSource = dtSearch.Tables[0];
            //    ddlStyle.DataTextField = "Searchvalue";
            //    ddlStyle.DataValueField = "Searchvalue";
            //    ddlStyle.DataBind();
            //}
            //else { ddlStyle.DataSource = null; ddlStyle.DataBind(); }

            //ddlStyle.Items.Insert(0, new ListItem("Select Style", "0"));

            dtSearch = new DataSet(); //Bind Style
            //dtSearch = CommonComponent.GetCommonDataSet("Select ISNULL(Searchvalue,'') as Searchvalue,SearchID from tb_ProductSearchType where SearchType=8 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue");
            dtSearch = CommonComponent.GetCommonDataSet("select StyleID as SearchID,StyleName ,ImgName,Sename,DisplayOrderBy,ISNULL(Active,0) as Active,ISNULL(Deleted,0) as Deleted, StyleName + '~'+isnull(ImgName,'') as SearchValue,Convert(varchar(10),StyleID) + '~'+StyleName  as SearchValueID  from tb_Style where ISNULL(Active,0)  = 1 and isnull(deleted,0) = 0 order by DisplayOrderBy ASC");

            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                ddlNewStyle.DataSource = dtSearch.Tables[0];
                ddlNewStyle.DataTextField = "SearchValue";
                ddlNewStyle.DataValueField = "SearchValueID";
                ddlNewStyle.DataBind();
            }
            else
            {
                ddlNewStyle.DataSource = null;
                ddlNewStyle.DataBind();
            }
            dtSearch = new DataSet();
            //dtSearch = CommonComponent.GetCommonDataSet("Select ISNULL(Searchvalue,'') as Searchvalue,SearchID from tb_ProductSearchType where SearchType=5 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue");
            dtSearch = CommonComponent.GetCommonDataSet("select HeaderID as SearchID,HeaderName ,ImageName,Sename,DisplayOrderBy,ISNULL(Active,0) as Active,ISNULL(Deleted,0) as Deleted, HeaderName + '~'+isnull(ImageName,'') as SearchValue,Convert(varchar(10),HeaderID) + '~'+HeaderName  as SearchValueID   from tb_Header where ISNULL(Active,0)  = 1 and isnull(deleted,0) = 0 order by DisplayOrderBy ASC");
            if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            {
                chkHeader.DataSource = dtSearch.Tables[0];
                chkHeader.DataTextField = "SearchValue";
                chkHeader.DataValueField = "SearchValueID";
                chkHeader.DataBind();
            }
            else
            {
                chkHeader.DataSource = null;
                chkHeader.DataBind();
            }
            dtSearch = CommonComponent.GetCommonDataSet("select  * from tb_Material where ISNULL(Active,0)  = 1 and isnull(deleted,0) = 0 order by DisplayOrderBy ASC");
            //if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
            //{
                

            //    chkmaterial.DataSource = dtSearch.Tables[0];
            //    chkmaterial.DataTextField = "MaterialName";
            //    chkmaterial.DataValueField = "MaterialID";
            //    chkmaterial.DataBind();
            //}
            //else { chkmaterial.DataSource = null; chkmaterial.DataBind(); }

        }

        #endregion

        protected void grdProductStyleType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[3].Attributes.Add("style", "display:none");
            }

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                e.Row.Cells[3].Attributes.Add("style", "display:none");
                //Label lblSearchType = (Label)e.Row.FindControl("lblSearchType");
                //Label lblSearchTypeId = (Label)e.Row.FindControl("lblSearchTypeId");
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";

                ((ImageButton)e.Row.FindControl("btnSave")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.png";
                ((ImageButton)e.Row.FindControl("btnCancel")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/CloseIcon.png";

                TextBox txtdividedwidth = (TextBox)e.Row.FindControl("txtdividedwidth");
                txtdividedwidth.Attributes.Add("readonly", "true");


                //if (!string.IsNullOrEmpty(lblSearchTypeId.Text.Trim().ToString()))
                //{
                //    if (lblSearchTypeId.Text.ToString() == "1")
                //        lblSearchType.Text = "Color";
                //    if (lblSearchTypeId.Text.ToString() == "2")
                //        lblSearchType.Text = "Pattern";
                //    if (lblSearchTypeId.Text.ToString() == "3")
                //        lblSearchType.Text = "Fabric";
                //    if (lblSearchTypeId.Text.ToString() == "4")
                //        lblSearchType.Text = "Style";
                //}
            }
        }

        protected void grdoptions_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ////Label lblOptionActive = (Label)e.Row.FindControl("lblOptionActive");
                ////CheckBox chkOptionActive = (CheckBox)e.Row.FindControl("chkOptionActive");
                ////if(lblOptionActive.Text.ToString().ToLower()=="true" || lblOptionActive.Text=="1")
                ////{
                ////    chkOptionActive.Checked = true;
                ////}
            }
        }

        /// <summary>
        /// Admin Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdProductStyleType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandSource.GetType() != typeof(GridView))
            {
                GridViewRow row = ((GridViewRow)((System.Web.UI.WebControls.ImageButton)(e.CommandSource)).NamingContainer);
                Label lblStylename = (Label)row.FindControl("lblStylename");
                Label lblActive = (Label)row.FindControl("lblActive");
                CheckBox ChkActive = (CheckBox)row.FindControl("chkActive");
                TextBox txtAdditionalPrice = (TextBox)row.FindControl("txtAdditionalPrice");
                TextBox txtPerInch = (TextBox)row.FindControl("txtPerInch");
                ImageButton btnSave = row.FindControl("btnSave") as ImageButton;
                ImageButton btnCancel = row.FindControl("btnCancel") as ImageButton;
                ImageButton btnEditPrice = row.FindControl("_editLinkButton") as ImageButton;
                TextBox txtYardHeaderandhem = (TextBox)row.FindControl("txtYardHeaderandhem");
                TextBox txtFabricInch = (TextBox)row.FindControl("txtFabricInch");
                Label lblPerInch = (Label)row.FindControl("lblPerInch");
                Label lblPrice = (Label)row.FindControl("lblPrice");
                Label lbldividedwidth = (Label)row.FindControl("lbldividedwidth");
                Label lblFabricwidth = (Label)row.FindControl("lblFabricwidth");
                Label lblFabricInch = (Label)row.FindControl("lblFabricInch");
                Label lblYardHeaderandhem = (Label)row.FindControl("lblYardHeaderandhem");



                TextBox txtFabricwidth = (TextBox)row.FindControl("txtFabricwidth");
                TextBox txtdividedwidth = (TextBox)row.FindControl("txtdividedwidth");

                if (e.CommandName == "DeleteAdmin")
                {
                    try
                    {
                        int SearchID = Convert.ToInt32(e.CommandArgument);
                        CommonComponent.ExecuteCommonData("Delete from tb_ProductStylePrice where ProductStyleId=" + SearchID + "");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecDelete", "alert('Record Deleted Successfully');", true);
                        BindProductStylePrice("edit");
                        BindProductOptionsPrice("edit");
                    }
                    catch (Exception ex)
                    { throw ex; }
                }
                else if (e.CommandName == "edit")
                {
                    try
                    {
                        btnSave.Visible = true;
                        btnCancel.Visible = true;
                        btnEditPrice.Visible = false;
                        txtAdditionalPrice.Visible = true;
                        lblPrice.Visible = false;
                        txtPerInch.Visible = true;
                        lblPerInch.Visible = false;
                        txtYardHeaderandhem.Visible = true;
                        lblYardHeaderandhem.Visible = false;
                        txtFabricInch.Visible = true;
                        lblFabricInch.Visible = false;
                        txtFabricwidth.Visible = true;
                        lblFabricwidth.Visible = false;
                        txtdividedwidth.Visible = true;
                        lbldividedwidth.Visible = false;
                    }
                    catch
                    { }
                }
                else if (e.CommandName == "Save")
                {
                    decimal AddiPrice = 0, PerInch = 0, YardHeaderandhem = 0, FabricInch = 0, Fabricwidth = 0, Dividedwidth = 0;

                    int SearchID = Convert.ToInt32(e.CommandArgument);
                    if (!string.IsNullOrEmpty(txtAdditionalPrice.Text.ToString()))
                        AddiPrice = Convert.ToDecimal(txtAdditionalPrice.Text.ToString());
                    if (!string.IsNullOrEmpty(txtPerInch.Text.ToString()))
                        PerInch = Convert.ToDecimal(txtPerInch.Text.ToString());

                    if (!string.IsNullOrEmpty(txtYardHeaderandhem.Text.ToString()))
                        YardHeaderandhem = Convert.ToDecimal(txtYardHeaderandhem.Text.ToString());
                    if (!string.IsNullOrEmpty(txtFabricInch.Text.ToString()))
                        FabricInch = Convert.ToDecimal(txtFabricInch.Text.ToString());


                    if (!string.IsNullOrEmpty(txtFabricwidth.Text.ToString()))
                        Fabricwidth = Convert.ToDecimal(txtFabricwidth.Text.ToString());

                    if (!string.IsNullOrEmpty(txtdividedwidth.Text.ToString()))
                        Dividedwidth = Convert.ToDecimal(txtdividedwidth.Text.ToString());

                    Int16 Active = 0;
                    if (ChkActive.Checked)
                        Active = 1;
                    //if (!ChkActive.Checked)
                    //{
                    //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecUpdate", "alert('Please Select Style');", true);
                    //    return;
                    //}

                    if (SearchID > 0)
                    {
                        //CommonComponent.ExecuteCommonData("Update tb_ProductStylePrice set Style='" + lblStylename.Text.ToString().Replace("'", "''") + "',AdditionalPrice=" + AddiPrice + ",PerInch=" + PerInch + ",Active=" + Active + ",YardHeaderandhem=" + YardHeaderandhem + ",FabricInch=" + FabricInch + " where ProductStyleId=" + SearchID + "");

                        CommonComponent.ExecuteCommonData("Update tb_ProductStylePrice set Style='" + lblStylename.Text.ToString().Replace("'", "''") + "',AdditionalPrice=" + AddiPrice + ",PerInch=" + PerInch + ",Active=" + Active + ",YardHeaderandhem=" + YardHeaderandhem + ",FabricInch=" + FabricInch + " ,FabricWidth=" + Fabricwidth + " ,DividedWidth=" + Dividedwidth + "  where ProductStyleId=" + SearchID + "");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecUpdate", "alert('Record Updated Successfully');", true);
                    }
                    else
                    {
                        //if (ChkActive.Checked)
                        //{
                        //CommonComponent.ExecuteCommonData("Insert into tb_ProductStylePrice (ProductId,Style,AdditionalPrice,PerInch,Active,Createdon,YardHeaderandhem,FabricInch) " +
                        //                            " values(" + Request.QueryString["ID"].ToString() + ",'" + lblStylename.Text.ToString().Replace("'", "''") + "'," + AddiPrice + "," + PerInch + ",1,GETDATE()," + YardHeaderandhem + "," + FabricInch + ")");

                        CommonComponent.ExecuteCommonData("Insert into tb_ProductStylePrice (ProductId,Style,AdditionalPrice,PerInch,Active,Createdon,YardHeaderandhem,FabricInch,FabricWidth,DividedWidth) " +
                                                      " values(" + Request.QueryString["ID"] + ",'" + lblStylename.Text.ToString().Replace("'", "''") + "'," + AddiPrice + "," + PerInch + "," + Active + ",GETDATE()," + YardHeaderandhem + "," + FabricInch + "," + Fabricwidth + "," + Dividedwidth + ")");

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "RecUpdate", "alert('Record Updated Successfully');", true);
                        //}
                    }

                    BindProductStylePrice("edit");
                    BindProductOptionsPrice("edit");
                }
                else if (e.CommandName == "Cancel")
                {
                    btnEditPrice.Visible = true;
                    btnCancel.Visible = false;
                    btnSave.Visible = false;
                    if (lblActive.Text.ToLower() == "true")
                        ChkActive.Checked = true;
                    else
                        ChkActive.Checked = false;

                    txtAdditionalPrice.Text = lblPrice.Text.ToString();
                    txtPerInch.Text = lblPerInch.Text.ToString();
                    txtYardHeaderandhem.Text = lblYardHeaderandhem.Text.ToString();
                    txtFabricInch.Text = lblFabricInch.Text.ToString();
                    txtFabricwidth.Text = lblFabricwidth.Text.ToString();
                    txtdividedwidth.Text = lbldividedwidth.Text.ToString();
                    txtAdditionalPrice.Visible = false;
                    lblPrice.Visible = true;
                    txtPerInch.Visible = false;
                    lblPerInch.Visible = true;
                    txtYardHeaderandhem.Visible = false;
                    lblYardHeaderandhem.Visible = true;
                    txtFabricInch.Visible = false;
                    lblFabricInch.Visible = true;
                    txtFabricwidth.Visible = false;
                    lblFabricwidth.Visible = true;
                    txtdividedwidth.Visible = false;
                    lbldividedwidth.Visible = true;
                }
            }
        }

        protected void btnSaveStylePrice_Click(object sender, ImageClickEventArgs e)
        {
            if (grdProductStyleType.Rows.Count > 0 && Request.QueryString["ID"] != null)
            {

                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(*) as Tot from tb_ProductStylePrice where productId=" + Request.QueryString["ID"].ToString() + "")) > 0)
                {
                    CommonComponent.ExecuteCommonData("Delete from tb_ProductStylePrice where productId=" + Request.QueryString["ID"].ToString() + "");
                }

                for (int i = 0; i < grdProductStyleType.Rows.Count; i++)
                {
                    decimal AddiPrice = 0, PerInch = 0, YardHeaderandhem = 0, FabricInch = 0, Fabricwidth = 0, Dividedwidth = 0;
                    CheckBox ChkActive = (CheckBox)grdProductStyleType.Rows[i].FindControl("chkActive");
                    Label lblStylename = (Label)grdProductStyleType.Rows[i].FindControl("lblStylename");
                    Label ProductStyleId = (Label)grdProductStyleType.Rows[i].FindControl("ProductStyleId");
                    TextBox txtAdditionalPrice = (TextBox)grdProductStyleType.Rows[i].FindControl("txtAdditionalPrice");
                    TextBox txtPerInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtPerInch");

                    TextBox txtYardHeaderandhem = (TextBox)grdProductStyleType.Rows[i].FindControl("txtYardHeaderandhem");
                    TextBox txtFabricInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtFabricInch");

                    TextBox txtFabricwidth = (TextBox)grdProductStyleType.Rows[i].FindControl("txtFabricwidth");
                    TextBox txtdividedwidth = (TextBox)grdProductStyleType.Rows[i].FindControl("txtdividedwidth");


                    if (ChkActive.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtAdditionalPrice.Text.ToString()))
                            AddiPrice = Convert.ToDecimal(txtAdditionalPrice.Text.ToString());
                        if (!string.IsNullOrEmpty(txtPerInch.Text.ToString()))
                            PerInch = Convert.ToDecimal(txtPerInch.Text.ToString());

                        if (!string.IsNullOrEmpty(txtYardHeaderandhem.Text.ToString()))
                            YardHeaderandhem = Convert.ToDecimal(txtYardHeaderandhem.Text.ToString());
                        if (!string.IsNullOrEmpty(txtFabricInch.Text.ToString()))
                            FabricInch = Convert.ToDecimal(txtFabricInch.Text.ToString());
                        if (!string.IsNullOrEmpty(txtFabricwidth.Text.ToString()))
                            Fabricwidth = Convert.ToDecimal(txtFabricwidth.Text.ToString());

                        if (!string.IsNullOrEmpty(txtdividedwidth.Text.ToString()))
                            Dividedwidth = Convert.ToDecimal(txtdividedwidth.Text.ToString());
                        //CommonComponent.ExecuteCommonData("Insert into tb_ProductStylePrice (ProductId,Style,AdditionalPrice,PerInch,Active,Createdon,YardHeaderandhem,FabricInch) " +
                        // " values(" + Request.QueryString["ID"] + ",'" + lblStylename.Text.ToString().Replace("'", "''") + "'," + AddiPrice + "," + PerInch + ",1,GETDATE()," + YardHeaderandhem + "," + FabricInch + ")");

                        CommonComponent.ExecuteCommonData("Insert into tb_ProductStylePrice (ProductId,Style,AdditionalPrice,PerInch,Active,Createdon,YardHeaderandhem,FabricInch,FabricWidth,DividedWidth) " +
                                                          " values(" + Request.QueryString["ID"] + ",'" + lblStylename.Text.ToString().Replace("'", "''") + "'," + AddiPrice + "," + PerInch + ",1,GETDATE()," + YardHeaderandhem + "," + FabricInch + "," + Fabricwidth + "," + Dividedwidth + ")");

                    }
                }
                BindProductStylePrice("edit");
            }
        }
        protected void btnSaveOptionPrice_Click(object sender, ImageClickEventArgs e)
        {
            if (grdProductStyleType.Rows.Count > 0 && Request.QueryString["ID"] != null)
            {

                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(*) as Tot from tb_ProductOptionsPrice where productId=" + Request.QueryString["ID"].ToString() + "")) > 0)
                {
                    CommonComponent.ExecuteCommonData("Delete from tb_ProductOptionsPrice where productId=" + Request.QueryString["ID"].ToString() + "");
                }

                for (int i = 0; i < grdoptions.Rows.Count; i++)
                {
                    decimal AddiPrice = 0, PerInch = 0, YardHeaderandhem = 0, FabricInch = 0, PricePercentage = 0;
                    // CheckBox ChkActive = (CheckBox)grdProductStyleType.Rows[i].FindControl("chkActive");
                    Label lblStylename = (Label)grdoptions.Rows[i].FindControl("lblStylename");
                    Label ProductStyleId = (Label)grdoptions.Rows[i].FindControl("ProductStyleId");
                    TextBox txtAdditionalPrice = (TextBox)grdoptions.Rows[i].FindControl("txtAdditionalPrice");
                    CheckBox chkOptionActive = (CheckBox)grdoptions.Rows[i].FindControl("chkOptionActive");
                    int Active = 0;
                    if (chkOptionActive.Checked)
                    {
                        Active = 1;

                    }
                    else
                    {
                        Active = 0;
                    }
                    TextBox txtPricePercentage = (TextBox)grdoptions.Rows[i].FindControl("txtPricePercentage");
                    // TextBox txtPerInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtPerInch");

                    // TextBox txtYardHeaderandhem = (TextBox)grdProductStyleType.Rows[i].FindControl("txtYardHeaderandhem");
                    // TextBox txtFabricInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtFabricInch");

                    //if (ChkActive.Checked)
                    //{
                    if (!string.IsNullOrEmpty(txtAdditionalPrice.Text.ToString()))
                        AddiPrice = Convert.ToDecimal(txtAdditionalPrice.Text.ToString());
                    //if (!string.IsNullOrEmpty(txtPerInch.Text.ToString()))
                    //    PerInch = Convert.ToDecimal(txtPerInch.Text.ToString());

                    //if (!string.IsNullOrEmpty(txtYardHeaderandhem.Text.ToString()))
                    //    YardHeaderandhem = Convert.ToDecimal(txtYardHeaderandhem.Text.ToString());
                    //if (!string.IsNullOrEmpty(txtFabricInch.Text.ToString()))
                    //    FabricInch = Convert.ToDecimal(txtFabricInch.Text.ToString());
                    if (!string.IsNullOrEmpty(txtPricePercentage.Text.ToString()))
                        PricePercentage = Convert.ToDecimal(txtPricePercentage.Text.ToString());
                    CommonComponent.ExecuteCommonData("Insert into tb_ProductOptionsPrice (ProductId,Options,AdditionalPrice,Createdon,AdditionalPricePercentage,Active) " +
                                                     " values(" + Request.QueryString["ID"] + ",'" + lblStylename.Text.ToString().Replace("'", "''") + "'," + AddiPrice + ",GETDATE()," + PricePercentage.ToString() + "," + Active + ")");

                    //}
                }
                lblmessageoption.Text = "Record Updated Successfully.";
                BindProductOptionsPrice("edit");
            }
        }
        protected void SaveProductOptionsPrice(Int32 ProductId)
        {
            if (grdoptions.Rows.Count > 0)
            {
                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(*) as Tot from tb_ProductOptionsPrice where productId=" + ProductId + "")) > 0)
                {
                    CommonComponent.ExecuteCommonData("Delete from tb_ProductOptionsPrice where productId=" + ProductId + "");
                }
                for (int i = 0; i < grdoptions.Rows.Count; i++)
                {
                    decimal AddiPrice = 0, PerInch = 0, YardHeaderandhem = 0, FabricInch = 0, PricePercentage = 0;

                    //CheckBox ChkActive = (CheckBox)grdProductStyleType.Rows[i].FindControl("chkActive");
                    Label lblStylename = (Label)grdoptions.Rows[i].FindControl("lblStylename");
                    Label ProductStyleId = (Label)grdoptions.Rows[i].FindControl("ProductStyleId");
                    TextBox txtAdditionalPrice = (TextBox)grdoptions.Rows[i].FindControl("txtAdditionalPrice");
                    //TextBox txtPerInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtPerInch");
                    TextBox txtPricePercentage = (TextBox)grdoptions.Rows[i].FindControl("txtPricePercentage");
                    CheckBox chkOptionActive = (CheckBox)grdoptions.Rows[i].FindControl("chkOptionActive");
                    int Active = 0;
                    if (chkOptionActive.Checked)
                    {
                        Active = 1;

                    }
                    else
                    {
                        Active = 0;
                    }
                    //TextBox txtYardHeaderandhem = (TextBox)grdProductStyleType.Rows[i].FindControl("txtYardHeaderandhem");
                    //TextBox txtFabricInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtFabricInch");

                    //if (ChkActive.Checked)
                    //{
                    if (!string.IsNullOrEmpty(txtAdditionalPrice.Text.ToString()))
                        AddiPrice = Convert.ToDecimal(txtAdditionalPrice.Text.ToString());


                    if (!string.IsNullOrEmpty(txtPricePercentage.Text.ToString()))
                        PricePercentage = Convert.ToDecimal(txtPricePercentage.Text.ToString());

                    //if (!string.IsNullOrEmpty(txtPerInch.Text.ToString()))
                    //    PerInch = Convert.ToDecimal(txtPerInch.Text.ToString());

                    //if (!string.IsNullOrEmpty(txtYardHeaderandhem.Text.ToString()))
                    //    YardHeaderandhem = Convert.ToDecimal(txtYardHeaderandhem.Text.ToString());
                    //if (!string.IsNullOrEmpty(txtFabricInch.Text.ToString()))
                    //    FabricInch = Convert.ToDecimal(txtFabricInch.Text.ToString());


                    CommonComponent.ExecuteCommonData("Insert into tb_ProductOptionsPrice (ProductId,Options,AdditionalPrice,Createdon,AdditionalPricePercentage,Active) " +
                                                     " values(" + ProductId + ",'" + lblStylename.Text.ToString().Replace("'", "''") + "'," + AddiPrice + ",GETDATE()," + PricePercentage + "," + Active + ")");
                    //}
                }
            }
        }
        protected void SaveProductPrice(Int32 ProductId)
        {
            if (grdProductStyleType.Rows.Count > 0)
            {
                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(*) as Tot from tb_ProductStylePrice where productId=" + ProductId + "")) > 0)
                {
                    CommonComponent.ExecuteCommonData("Delete from tb_ProductStylePrice where productId=" + ProductId + "");
                }
                for (int i = 0; i < grdProductStyleType.Rows.Count; i++)
                {
                    decimal AddiPrice = 0, PerInch = 0, YardHeaderandhem = 0, FabricInch = 0;

                    CheckBox ChkActive = (CheckBox)grdProductStyleType.Rows[i].FindControl("chkActive");
                    Label lblStylename = (Label)grdProductStyleType.Rows[i].FindControl("lblStylename");
                    Label ProductStyleId = (Label)grdProductStyleType.Rows[i].FindControl("ProductStyleId");
                    TextBox txtAdditionalPrice = (TextBox)grdProductStyleType.Rows[i].FindControl("txtAdditionalPrice");
                    TextBox txtPerInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtPerInch");
                    TextBox txtFabricwidth = (TextBox)grdProductStyleType.Rows[i].FindControl("txtFabricwidth");
                    TextBox txtdividedwidth = (TextBox)grdProductStyleType.Rows[i].FindControl("txtdividedwidth");

                    TextBox txtYardHeaderandhem = (TextBox)grdProductStyleType.Rows[i].FindControl("txtYardHeaderandhem");
                    TextBox txtFabricInch = (TextBox)grdProductStyleType.Rows[i].FindControl("txtFabricInch");

                    if (ChkActive.Checked)
                    {
                        if (!string.IsNullOrEmpty(txtAdditionalPrice.Text.ToString()))
                            AddiPrice = Convert.ToDecimal(txtAdditionalPrice.Text.ToString());
                        if (!string.IsNullOrEmpty(txtPerInch.Text.ToString()))
                            PerInch = Convert.ToDecimal(txtPerInch.Text.ToString());

                        if (!string.IsNullOrEmpty(txtYardHeaderandhem.Text.ToString()))
                            YardHeaderandhem = Convert.ToDecimal(txtYardHeaderandhem.Text.ToString());
                        if (!string.IsNullOrEmpty(txtFabricInch.Text.ToString()))
                            FabricInch = Convert.ToDecimal(txtFabricInch.Text.ToString());

                        decimal Fabricwidth = 0, dividedwidth = 0;
                        decimal.TryParse(txtFabricwidth.Text.ToString(), out Fabricwidth);
                        decimal.TryParse(txtdividedwidth.Text.ToString(), out dividedwidth);

                        CommonComponent.ExecuteCommonData("Insert into tb_ProductStylePrice (ProductId,Style,AdditionalPrice,PerInch,Active,Createdon,YardHeaderandhem,FabricInch,FabricWidth,DividedWidth) " +
                                                         " values(" + ProductId + ",'" + lblStylename.Text.ToString().Replace("'", "''") + "'," + AddiPrice + "," + PerInch + ",1,GETDATE()," + YardHeaderandhem + "," + FabricInch + "," + Fabricwidth + "," + dividedwidth + ")");
                    }
                }
            }
        }

        protected void grdProductStyleType_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void grdProductStyleType_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void btnSaveSecondaryColor_Click(object sender, ImageClickEventArgs e)
        {
            if (grdSecondaryColor.Rows.Count > 0 && Request.QueryString["ID"] != null)
            {
                SaveSecondaryColor(Convert.ToInt32(Request.QueryString["ID"].ToString()));
            }
        }

        protected void SaveSecondaryColor(int ProductId)
        {
            try
            {
                if (grdSecondaryColor.Rows.Count > 0)
                {
                    int cnt = 0;
                    string StrSecondarycolor = "";
                    StrSecondarycolor = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(SecondaryColor,'') as SecondaryColor from tb_product  where ProductId=" + ProductId + ""));

                    if (!string.IsNullOrEmpty(StrSecondarycolor.Trim().ToString()))
                    {
                        CommonComponent.ExecuteCommonData("Update tb_product set SecondaryColor='',ColorSku='' where productid=" + ProductId + "");
                    }

                    string StrSecondaryColorValue = "", strColorSkuValue = "";

                    for (int i = 0; i < grdSecondaryColor.Rows.Count; i++)
                    {
                        CheckBox chkSecondaryActive = (CheckBox)grdSecondaryColor.Rows[i].FindControl("chkSecondaryActive");
                        Label lblSearchvalue = (Label)grdSecondaryColor.Rows[i].FindControl("lblSearchvalue");
                        TextBox txtColorSku = (TextBox)grdSecondaryColor.Rows[i].FindControl("txtColorSku");

                        if (chkSecondaryActive.Checked)
                        {
                            string StrColorName = "", strColorSku = "";

                            if (!string.IsNullOrEmpty(lblSearchvalue.Text.ToString()))
                                StrColorName = Convert.ToString(lblSearchvalue.Text.ToString());
                            if (!string.IsNullOrEmpty(txtColorSku.Text.ToString()))
                                strColorSku = Convert.ToString(txtColorSku.Text.ToString());

                            StrSecondaryColorValue = StrSecondaryColorValue + StrColorName + ",";

                            if (!string.IsNullOrEmpty(strColorSku.ToString().Trim()))
                            {
                                strColorSkuValue = strColorSkuValue + strColorSku + ",";
                            }
                            else
                            {
                                strColorSkuValue = strColorSkuValue + "~,";
                            }
                            cnt++;
                        }
                    }

                    CommonComponent.ExecuteCommonData("Update tb_product set SecondaryColor='" + StrSecondaryColorValue + "',ColorSku='" + strColorSkuValue + "' where productid=" + ProductId + "");


                    DataSet dtSearch = new DataSet();
                    dtSearch = CommonComponent.GetCommonDataSet("Select ISNULL(Searchvalue,'') as Searchvalue,SearchID,'' as ColorSku,0 as Active from tb_ProductSearchType where SearchType=1 and ISNULL(deleted,1)=0 and ISNULL(Active,0)=1 and ISNULL(Searchvalue,'')<>'' Order by Searchvalue");
                    if (dtSearch != null && dtSearch.Tables.Count > 0 && dtSearch.Tables[0].Rows.Count > 0)
                    {
                        grdSecondaryColor.DataSource = dtSearch.Tables[0];
                        grdSecondaryColor.DataBind();
                        trSecondaryColor.Visible = false;
                    }
                    else
                    {
                        grdSecondaryColor.DataSource = null;
                        grdSecondaryColor.DataBind();
                        trSecondaryColor.Visible = false;
                    }
                    if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit" && Request.QueryString["ID"] != null)
                    {
                        btnSaveSecondaryColor.Visible = true;
                    }
                    else
                    {
                        btnSaveSecondaryColor.Visible = false;
                    }
                }
            }
            catch { }
        }

        protected void grdSecondaryColor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSearchvalue = (Label)e.Row.FindControl("lblSearchvalue");
                TextBox txtColorSku = (TextBox)e.Row.FindControl("txtColorSku");
                CheckBox chkSecondaryActive = (CheckBox)e.Row.FindControl("chkSecondaryActive");
                Literal ltrSelectSku = (Literal)e.Row.FindControl("ltrSelectSku");

                ltrSelectSku.Text = "<a id=\"atagSwatchProduct\" name=\"aOptAcc\" onclick=\"openCenteredCrossSaleWindowforProductSwatch('" + txtColorSku.ClientID + "');\" style=\"margin-right: 15px; font-weight: bold; cursor: pointer;\">Select SKU(s)</a>";

                if (Request.QueryString["ID"] != null)
                {
                    DataSet DsSecondaryColor = CommonComponent.GetCommonDataSet("Select ISNULL(SecondaryColor,'') as SecondaryColor,ISNULL(ColorSku,'') as ColorSku from tb_product  where ProductId=" + Request.QueryString["ID"].ToString() + "");
                    if (DsSecondaryColor != null && DsSecondaryColor.Tables.Count > 0 && DsSecondaryColor.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(DsSecondaryColor.Tables[0].Rows[0]["SecondaryColor"].ToString()))
                        {
                            string StrSTemp = DsSecondaryColor.Tables[0].Rows[0]["SecondaryColor"].ToString();
                            string StrCSkuTemp = Convert.ToString(DsSecondaryColor.Tables[0].Rows[0]["ColorSku"].ToString());

                            if (StrSTemp.ToString().Trim().IndexOf(",") > -1)
                            {
                                string[] StrSecondarycolor = StrSTemp.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                string[] StrColorSku = StrCSkuTemp.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                                if (StrSecondarycolor.Length > 0)
                                {
                                    for (int i = 0; i < StrSecondarycolor.Length; i++)
                                    {
                                        string StrValue = Convert.ToString(StrSecondarycolor[i]);

                                        if (StrValue.ToString().Trim().ToLower() == lblSearchvalue.Text.ToString().Trim().ToLower())
                                        {
                                            chkSecondaryActive.Checked = true;
                                            string StrSku = "";
                                            //if (StrColorSku.Length >= i && !string.IsNullOrEmpty(StrColorSku[i].ToString()))
                                            if (StrColorSku.Length > 0 && StrColorSku.Length >= i && !string.IsNullOrEmpty(StrColorSku[i].ToString()))
                                            {
                                                StrSku = Convert.ToString(StrColorSku[i]);
                                            }
                                            if (!string.IsNullOrEmpty(StrSku) && StrSku.ToString() != "~")
                                                txtColorSku.Text = StrSku.ToString();
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        private void FillProductfeature()
        {
            DataSet DsProductfeature = new DataSet();
            ProductComponent objProduct = new ProductComponent();
            if (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
            {
                StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                DsProductfeature = objProduct.GetproductFeature(0, StoreID, 1);
                if (DsProductfeature != null && DsProductfeature.Tables.Count > 0 && DsProductfeature.Tables[0].Rows.Count > 0)
                {
                    ddlproductfeature.DataSource = DsProductfeature;
                    ddlproductfeature.DataValueField = "FeatureId";
                    ddlproductfeature.DataTextField = "Name";
                    ddlproductfeature.DataBind();
                }
                else
                {
                    ddlproductfeature.DataSource = null;
                    ddlproductfeature.DataBind();
                }
                ddlproductfeature.Items.Insert(0, new ListItem("Select Feature", "0"));
            }
        }

        protected void ddlproductfeature_SelectedIndexChanged(object sender, EventArgs e)
        {
            String DescDetails = "";
            if (ddlproductfeature.SelectedValue.ToString() != "0")
            {
                if (Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString()))
                {
                    int FeatureId = Convert.ToInt32(ddlproductfeature.SelectedValue);
                    ProductComponent objProduct = new ProductComponent();
                    DataSet dsFeatureDesc = new DataSet();
                    StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                    dsFeatureDesc = objProduct.GetproductFeature(FeatureId, StoreID, 2);
                    DescDetails = Server.HtmlDecode(dsFeatureDesc.Tables[0].Rows[0]["Body"].ToString().Trim());
                    txtFeatures.Text = DescDetails;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "featureRedi", "TabdisplayProduct(2);", true);
                }
            }
            else { txtFeatures.Text = ""; }
        }

        protected void btnApprove_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["ID"] != null && Request.QueryString["Mode"] != null)
            {
                Int32 AdminId = 0;
                if (Session["AdminID"] != null)
                {
                    AdminId = Convert.ToInt32(Session["AdminID"].ToString());
                }
                CommonComponent.ExecuteCommonData("Update tb_Product set IsDataVerify=1,DataVerifyBy=" + AdminId + ",IsDataVerifyOn=getdate() where ProductID= " + Request.QueryString["ID"].ToString() + "");
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Item Approved Successfully.', 'Message');", true);
                btnApprove.Visible = false;
            }
        }
        protected void imgbtnAlt1del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage1"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage1"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage1"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage1"].ToString());
            ViewState["DelImage1"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt1.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt1del.Visible = false;
        }
        protected void imgbtnAlt2del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage2"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage2"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage2"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage2"].ToString());
            ViewState["DelImage2"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt2.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt2del.Visible = false;
        }
        protected void imgbtnAlt3del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage3"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage3"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage3"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage3"].ToString());
            ViewState["DelImage3"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt3del.Visible = false;
        }
        protected void imgbtnAlt4del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage4"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage4"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage4"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage4"].ToString());
            ViewState["DelImage4"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt4.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt4del.Visible = false;
        }
        protected void imgbtnAlt5del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage5"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage5"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage5"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage5"].ToString());
            ViewState["DelImage5"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt5.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt5del.Visible = false;
        }

        protected void imgbtnAlt1_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt1.FileName.Length > 0 && Path.GetExtension(flalt1.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt1.FileName.Length > 0)
                {
                    bool checksize = false;
                    checksize = CheckImageSize(flalt1, 1500, 1500);
                    if (!checksize)
                    {
                        ViewState["File1"] = null;
                        lblMsg.Text = "Product image upload size must be width: 1500px and height: 1500px.";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt1');});", true);
                    }
                    else
                    {
                        try
                        {
                            FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt1.FileName);
                            if (flinfo.Exists)
                            {
                                flinfo.Delete();
                            }
                        }
                        catch { }
                        ViewState["File1"] = flalt1.FileName.ToString();
                        flalt1.SaveAs(Server.MapPath(ProductTempPath) + flalt1.FileName);
                        ImgAlt1.Src = ProductTempPath + flalt1.FileName;
                        lblMsg.Text = "";
                    }
                }
                else
                {
                    ViewState["File1"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt1');});", true);
            }

        }
        protected void imgbtnAlt2_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt2.FileName.Length > 0 && Path.GetExtension(flalt2.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt2.FileName.Length > 0)
                {
                    bool checksize = false;
                    checksize = CheckImageSize(flalt2, 1500, 1500);
                    if (!checksize)
                    {
                        ViewState["File2"] = null;
                        lblMsg.Text = "Product image upload size must be width: 1500px and height: 1500px.";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt2');});", true);
                    }
                    else
                    {
                        try
                        {
                            FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt2.FileName);
                            if (flinfo.Exists)
                            {
                                flinfo.Delete();
                            }
                        }
                        catch { }
                        ViewState["File2"] = flalt2.FileName.ToString();
                        flalt2.SaveAs(Server.MapPath(ProductTempPath) + flalt2.FileName);
                        ImgAlt2.Src = ProductTempPath + flalt2.FileName;
                        lblMsg.Text = "";
                    }
                }
                else
                {
                    ViewState["File2"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt2');});", true);
            }

        }
        protected void imgbtnAlt3_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt3.FileName.Length > 0 && Path.GetExtension(flalt3.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt3.FileName.Length > 0)
                {
                    bool checksize = false;
                    checksize = CheckImageSize(flalt3, 1500, 1500);
                    if (!checksize)
                    {
                        ViewState["File3"] = null;
                        lblMsg.Text = "Product image upload size must be width: 1500px and height: 1500px.";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt3');});", true);
                    }
                    else
                    {
                        try
                        {
                            FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt3.FileName);
                            if (flinfo.Exists)
                            {
                                flinfo.Delete();
                            }
                        }
                        catch { }
                        ViewState["File3"] = flalt3.FileName.ToString();
                        flalt3.SaveAs(Server.MapPath(ProductTempPath) + flalt3.FileName);
                        ImgAlt3.Src = ProductTempPath + flalt3.FileName;
                        lblMsg.Text = "";
                    }
                }
                else
                {
                    ViewState["File3"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt3');});", true);
            }

        }
        protected void imgbtnAlt4_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt4.FileName.Length > 0 && Path.GetExtension(flalt4.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt4.FileName.Length > 0)
                {
                    bool checksize = false;
                    checksize = CheckImageSize(flalt4, 1500, 1500);
                    if (!checksize)
                    {
                        ViewState["File4"] = null;
                        lblMsg.Text = "Product image upload size must be width: 1500px and height: 1500px.";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt4');});", true);
                    }
                    else
                    {
                        try
                        {
                            FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt4.FileName);
                            if (flinfo.Exists)
                            {
                                flinfo.Delete();
                            }
                        }
                        catch { }
                        ViewState["File4"] = flalt4.FileName.ToString();
                        flalt4.SaveAs(Server.MapPath(ProductTempPath) + flalt4.FileName);
                        ImgAlt4.Src = ProductTempPath + flalt4.FileName;
                        lblMsg.Text = "";
                    }
                }
                else
                {
                    ViewState["File4"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt4');});", true);
            }

        }
        protected void imgbtnAlt5_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(ViewState["ImagePathProduct"].ToString(), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt5.FileName.Length > 0 && Path.GetExtension(flalt5.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt5.FileName.Length > 0)
                {
                    bool checksize = false;
                    checksize = CheckImageSize(flalt5, 1500, 1500);
                    if (!checksize)
                    {
                        ViewState["File5"] = null;
                        lblMsg.Text = "Product image upload size must be width: 1500px and height: 1500px.";
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt5');});", true);
                    }
                    else
                    {
                        try
                        {
                            FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt5.FileName);
                            if (flinfo.Exists)
                            {
                                flinfo.Delete();
                            }
                        }
                        catch { }
                        ViewState["File5"] = flalt5.FileName.ToString();
                        flalt5.SaveAs(Server.MapPath(ProductTempPath) + flalt5.FileName);
                        ImgAlt5.Src = ProductTempPath + flalt5.FileName;
                        lblMsg.Text = "";
                    }
                }
                else
                {
                    ViewState["File5"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt5');});", true);
            }

        }

        private void GetDetailsofShade()
        {
            txtSuggestedRetail.Text = Convert.ToString(CommonComponent.GetScalarCommonData("select Configvalue from tb_appconfig where configname='ShadeSuggestedRetail' and  storeid=1"));
            txtMarkup.Text = Convert.ToString(CommonComponent.GetScalarCommonData("select Configvalue from tb_appconfig where configname='ShadeMarkup' and  storeid=1"));
            DataSet DsShadeData = new DataSet();
            string productid = "";
            if (Request.QueryString["oldid"] != null)
            {
                if (Request.QueryString["ID"] == null)
                {
                    //  Request.QueryString["ID"] = Request.QueryString["OLDID"].ToString();
                    productid = Request.QueryString["oldid"].ToString();
                }
                else
                {
                    productid = Request.QueryString["ID"].ToString();
                }

            }
            else
            {
                if (Request.QueryString["ID"] != null)
                {
                    productid = Request.QueryString["ID"].ToString();
                }
            }


            //if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "" )
            //{
            //DsShadeData = CommonComponent.GetCommonDataSet("select isnull(MinLength,0) as MinLength,isnull(MaxLength,0) as MaxLength,isnull(MinWidth,0) as MinWidth,isnull(MaxWidth,0) as MaxWidth  from tb_CellularShadeMeasurement where ProductID=" + Request.QueryString["ID"].ToString() + " and isnull(active,0)=1");
            DsShadeData = CommonComponent.GetCommonDataSet("select isnull(MinLength,0) as MinLength,isnull(MaxLength,0) as MaxLength,isnull(MinWidth,0) as MinWidth,isnull(MaxWidth,0) as MaxWidth  from tb_CellularShadeMeasurement where ProductID=" + productid + " and isnull(active,0)=1");
            if (DsShadeData != null && DsShadeData.Tables.Count > 0 && DsShadeData.Tables[0].Rows.Count > 0)
            {

                txtminwidth.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsShadeData.Tables[0].Rows[0]["MinWidth"].ToString().Trim()), 0));
                txtmaxwidth.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsShadeData.Tables[0].Rows[0]["MaxWidth"].ToString().Trim()), 0));
                txtminlength.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsShadeData.Tables[0].Rows[0]["MinLength"].ToString().Trim()), 0));
                txtmaxlength.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsShadeData.Tables[0].Rows[0]["MaxLength"].ToString().Trim()), 0));

            }
            else
            {

            }
            // }

        }
        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadCSV.HasFile && Path.GetExtension(uploadCSV.FileName).ToLower() == ".csv")
                {
                    lblMessage.Text = "";
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ShadeVendorCalc") + "Calc/")))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ShadeVendorCalc") + "Calc/"));
                    StrFileName = uploadCSV.FileName;
                    DeleteDocument(StrFileName);
                    uploadCSV.SaveAs(Server.MapPath(AppLogic.AppConfigs("ShadeVendorCalc") + "Calc/") + StrFileName);
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
                    if (InsertShadeData(dtCSV) && lblMessage.Text == "")
                    {
                        // contVerify.Visible = false;
                        lblMessage.Text = "Shade Vendor Calculator Imported Successfully";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                        lblMessage.Visible = true;
                        DisplayShadeCalc();
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


        private bool InsertShadeData(DataTable dtShadeData)
        {
            if (dtShadeData != null && dtShadeData.Rows.Count > 0)
            {


                int count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(productid) from tb_CellularShadeMeasurement where productid=" + Request.QueryString["ID"].ToString() + ""));
                if (count > 0)
                {
                    CommonComponent.ExecuteCommonData("update tb_CellularShadeMeasurement set MinWidth=" + Convert.ToDecimal(txtminwidth.Text) + ",MaxWidth=" + Convert.ToDecimal(txtmaxwidth.Text) + ",MinLength=" + Convert.ToDecimal(txtminlength.Text) + ",MaxLength=" + Convert.ToDecimal(txtmaxlength.Text) + " where ProductID=" + Request.QueryString["ID"].ToString() + " and isnull(active,0)=1");
                }
                else
                {
                    CommonComponent.ExecuteCommonData("insert into tb_CellularShadeMeasurement (SKU,MinLength,MaxLength,MinWidth,MaxWidth,Active,ProductID) values ('" + txtSKU.Text + "'," + Convert.ToDecimal(txtminlength.Text) + "," + Convert.ToDecimal(txtmaxlength.Text) + "," + Convert.ToDecimal(txtminwidth.Text) + "," + Convert.ToDecimal(txtmaxwidth.Text) + ",1," + Request.QueryString["ID"].ToString() + ")");
                }




                CommonComponent.ExecuteCommonData("EXEC usp_DeleteShadeInfo " + Request.QueryString["ID"].ToString() + "");
                string[] array = new string[dtShadeData.Columns.Count - 1];
                Decimal MinWidth = 0;
                Decimal MaxWidth = 0;
                Decimal MinLen = 0;
                Decimal MaxLen = 0;

                //DataSet DsShadeData = new DataSet();
                //DsShadeData = CommonComponent.GetCommonDataSet("select isnull(MinLength,0) as MinLength,isnull(MaxLength,0) as MaxLength,isnull(MinWidth,0) as MinWidth,isnull(MaxWidth,0) as MaxWidth  from tb_CellularShadeMeasurement where ProductID=" + Request.QueryString["ID"].ToString() + " and isnull(active,0)=1");
                //if (DsShadeData != null && DsShadeData.Tables.Count > 0 && DsShadeData.Tables[0].Rows.Count > 0)
                //{

                //MinWidth = Convert.ToDecimal(DsShadeData.Tables[0].Rows[0]["MinWidth"].ToString());
                //MaxWidth = Convert.ToDecimal(DsShadeData.Tables[0].Rows[0]["MaxWidth"].ToString());
                //MinLen = Convert.ToDecimal(DsShadeData.Tables[0].Rows[0]["MinLength"].ToString());
                //MaxLen = Convert.ToDecimal(DsShadeData.Tables[0].Rows[0]["MaxLength"].ToString());

                // }

                MinWidth = Convert.ToDecimal(txtminwidth.Text);
                MaxWidth = Convert.ToDecimal(txtmaxwidth.Text);
                MinLen = Convert.ToDecimal(txtminlength.Text);
                MaxLen = Convert.ToDecimal(txtmaxlength.Text);

                Int32 Totalwidth = 0;
                Int32 Totalwidth1 = 0;

                for (int j = 1; j < dtShadeData.Columns.Count; j++)
                {

                    if (dtShadeData.Columns[j].ColumnName.ToString().IndexOf(" ") > -1)
                    {
                        dtShadeData.Columns[j].ColumnName = dtShadeData.Columns[j].ColumnName.ToString().Substring(0, dtShadeData.Columns[j].ColumnName.ToString().IndexOf(" "));
                        dtShadeData.AcceptChanges();
                    }
                    if (j == 1)
                    {
                        for (int u = Convert.ToInt32(dtShadeData.Columns[j].ColumnName.ToString()) - 6; u <= Convert.ToInt32(dtShadeData.Columns[j].ColumnName.ToString()); u++)
                        {
                            Totalwidth = u;
                            if (MinWidth <= Totalwidth && MaxWidth >= Totalwidth)
                            {
                                Int32 WidthIdColid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  top 1 ShadeWidthID from  tb_Shadewidth where Value=" + u.ToString() + " and ProductID=" + Request.QueryString["ID"].ToString() + ""));
                                if (WidthIdColid == 0)
                                {
                                    CommonComponent.ExecuteCommonData("insert into tb_Shadewidth (Value,ProductID) values(" + u.ToString() + "," + Request.QueryString["ID"].ToString() + ")");
                                }
                            }
                        }
                    }
                    else
                    {
                        Totalwidth = Totalwidth + 1;
                        for (int u = Totalwidth; u <= Convert.ToInt32(dtShadeData.Columns[j].ColumnName.ToString()); u++)
                        {
                            Totalwidth1 = u;
                            if (MinWidth <= Totalwidth1 && MaxWidth >= Totalwidth1)
                            {
                                Int32 WidthIdColid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  top 1 ShadeWidthID from  tb_Shadewidth where Value=" + u.ToString() + " and ProductID=" + Request.QueryString["ID"].ToString() + ""));
                                if (WidthIdColid == 0)
                                {
                                    CommonComponent.ExecuteCommonData("insert into tb_Shadewidth (Value,ProductID) values(" + u.ToString() + "," + Request.QueryString["ID"].ToString() + ")");
                                }
                            }

                        }
                        Totalwidth = Totalwidth1;
                    }


                }


                Int32 StartWidth = Convert.ToInt32(dtShadeData.Columns[1].ColumnName.ToString());
                if (MinWidth < StartWidth)
                {
                    for (int u = Convert.ToInt32(MinWidth); u < Convert.ToInt32(StartWidth); u++)
                    {
                        Int32 WidthIdColid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  top 1 ShadeWidthID from  tb_Shadewidth where Value=" + u.ToString() + " and ProductID=" + Request.QueryString["ID"].ToString() + ""));
                        if (WidthIdColid == 0)
                        {
                            CommonComponent.ExecuteCommonData("insert into tb_Shadewidth (Value,ProductID) values(" + u.ToString() + "," + Request.QueryString["ID"].ToString() + ")");
                        }
                    }

                }


                Int32 StartLength = Convert.ToInt32(dtShadeData.Rows[1][0].ToString());
                if (MinLen < StartLength)
                {
                    for (int u = Convert.ToInt32(MinLen); u < Convert.ToInt32(StartLength); u++)
                    {
                        Int32 Colid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  top 1 ShadeLengthID from  tb_Shadelength where Value=" + u.ToString() + " and ProductID=" + Request.QueryString["ID"].ToString() + ""));
                        if (Colid == 0)
                        {

                            CommonComponent.GetScalarCommonData("insert into tb_Shadelength (Value,ProductID) values(" + u.ToString() + "," + Request.QueryString["ID"].ToString() + ")");


                        }
                    }
                }



                Int32 EndWidth = Convert.ToInt32(dtShadeData.Columns[dtShadeData.Columns.Count - 1].ColumnName.ToString());
                if (MaxWidth > EndWidth)
                {
                    for (int u = EndWidth + 1; u <= Convert.ToInt32(MaxWidth); u++)
                    {
                        Int32 WidthIdColid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  top 1 ShadeWidthID from  tb_Shadewidth where Value=" + u.ToString() + " and ProductID=" + Request.QueryString["ID"].ToString() + ""));
                        if (WidthIdColid == 0)
                        {
                            CommonComponent.ExecuteCommonData("insert into tb_Shadewidth (Value,ProductID) values(" + u.ToString() + "," + Request.QueryString["ID"].ToString() + ")");
                        }
                    }

                }



                Int32 EndLength = Convert.ToInt32(dtShadeData.Rows[dtShadeData.Rows.Count - 1][0].ToString());
                if (MaxLen > EndLength)
                {
                    for (int u = Convert.ToInt32(EndLength + 1); u <= Convert.ToInt32(MaxLen); u++)
                    {
                        Int32 Colid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  top 1 ShadeLengthID from  tb_Shadelength where Value=" + u.ToString() + " and ProductID=" + Request.QueryString["ID"].ToString() + ""));
                        if (Colid == 0)
                        {

                            CommonComponent.GetScalarCommonData("insert into tb_Shadelength (Value,ProductID) values(" + u.ToString() + "," + Request.QueryString["ID"].ToString() + ")");

                        }
                    }
                }




                DataSet dsrow = new DataSet();
                dsrow = CommonComponent.GetCommonDataSet("select * from tb_Shadewidth WHERE ProductID=" + Request.QueryString["ID"].ToString() + " order by value");


                Int32 rwIdcnt = 1;
                bool rowchange = false;
                for (int j = 0; j < dsrow.Tables[0].Rows.Count; j++)
                {
                    if (rowchange)
                    {
                        rwIdcnt = rwIdcnt + 1;
                        rowchange = false;
                    }
                    for (int k = 1; k < dtShadeData.Columns.Count; k++)
                    {

                        if (dtShadeData.Columns[k].ColumnName.ToString().IndexOf(" ") > -1)
                        {
                            dtShadeData.Columns[k].ColumnName = dtShadeData.Columns[k].ColumnName.ToString().Substring(0, dtShadeData.Columns[k].ColumnName.ToString().IndexOf(" "));
                            dtShadeData.AcceptChanges();
                        }
                        if (Convert.ToDecimal(dtShadeData.Columns[k].ColumnName.ToString()) == Convert.ToDecimal(dsrow.Tables[0].Rows[j]["Value"].ToString()))
                        {
                            rowchange = true;
                        }
                    }


                    //if (Convert.ToInt32(dsrow.Tables[0].Rows[j]["ShadeWidthID"].ToString()) > 

                    for (int i = 1; i < dtShadeData.Rows.Count; i++)
                    {
                        //if (Convert.TOIntdtShadeData.Rows[i][rwIdcnt].ToString() > )
                        //{
                        //    dtShadeData.Columns[j].ColumnName = dtShadeData.Columns[j].ColumnName.ToString().Substring(0, dtShadeData.Columns[j].ColumnName.ToString().IndexOf(" "));
                        //    dtShadeData.AcceptChanges();
                        //}

                        int Colid = 0;
                        for (int u = Convert.ToInt32(dtShadeData.Rows[i][0].ToString()) - 6; u <= Convert.ToInt32(dtShadeData.Rows[i][0].ToString()); u++)
                        {
                            if (MinLen <= u && MaxLen >= u)
                            {
                                Colid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  top 1 ShadeLengthID from  tb_Shadelength where Value=" + u.ToString() + " and ProductID=" + Request.QueryString["ID"].ToString() + ""));
                                if (Colid == 0)
                                {
                                    if (j == 0)
                                    {
                                        Colid = Convert.ToInt32(CommonComponent.GetScalarCommonData("insert into tb_Shadelength (Value,ProductID) values(" + u.ToString() + "," + Request.QueryString["ID"].ToString() + ");select SCOPE_IDENTITY()"));
                                    }

                                }

                            }

                        }




                    }


                }
                dtShadeData.Rows.RemoveAt(0);
                dtShadeData.AcceptChanges();
                DataSet dslength = new DataSet();
                dslength = CommonComponent.GetCommonDataSet("select * from tb_Shadelength WHERE ProductID=" + Request.QueryString["ID"].ToString() + " order by value");

                for (int j = 0; j < dsrow.Tables[0].Rows.Count; j++)
                {
                    Int32 rowId = -1;
                    for (int l = 1; l < dtShadeData.Columns.Count; l++)
                    {
                        if (Convert.ToInt32(dtShadeData.Columns[l].ColumnName.ToString()) >= Convert.ToInt32(string.Format("{0:0}", Convert.ToDecimal(dsrow.Tables[0].Rows[j]["Value"].ToString()))))
                        {
                            rowId = l;
                            break;
                        }

                    }
                    for (int k = 0; k < dslength.Tables[0].Rows.Count; k++)
                    {


                        Int32 detailtid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select  top 1 ShadeLengthID from  tb_ShadeDetail where ShadeWidthID=" + dsrow.Tables[0].Rows[j]["ShadeWidthID"].ToString() + " AND ShadeLengthID=" + dslength.Tables[0].Rows[k]["ShadeLengthID"].ToString() + ""));

                        if (detailtid == 0)
                        {
                            if (rowId == -1)
                            {
                                rowId = dtShadeData.Columns.Count - 1;
                            }
                            DataRow[] drall = dtShadeData.Select("width >=" + string.Format("{0:0}", Convert.ToDecimal(dslength.Tables[0].Rows[k]["Value"].ToString())) + "");
                            if (drall.Length > 0)
                            {
                                CommonComponent.ExecuteCommonData("insert into tb_ShadeDetail (ShadeLengthID,ShadeWidthID,Value) values(" + dslength.Tables[0].Rows[k]["ShadeLengthID"].ToString() + "," + dsrow.Tables[0].Rows[j]["ShadeWidthID"].ToString() + "," + drall[0][rowId].ToString() + ")");
                            }
                            else
                            {
                                //drall = dtShadeData.Select("width >=" + string.Format("{0:0}", Convert.ToDecimal(dtShadeData.Columns[dtShadeData.Rows.ToString())) + "");

                                CommonComponent.ExecuteCommonData("insert into tb_ShadeDetail (ShadeLengthID,ShadeWidthID,Value) values(" + dslength.Tables[0].Rows[k]["ShadeLengthID"].ToString() + "," + dsrow.Tables[0].Rows[j]["ShadeWidthID"].ToString() + "," + dtShadeData.Rows[dtShadeData.Rows.Count - 1][rowId].ToString() + ")");
                            }


                        }
                    }


                }


            }
            else
            {
                return false;
            }


            return true;
        }


        private void DisplayShadeCalc()
        {
            DataSet dsWidth = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Shadewidth WHERE ProductId=" + Request.QueryString["ID"] + " order by value");

            DataSet dsLength = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ShadeLength  WHERE ProductId=" + Request.QueryString["ID"] + " order by value");

            DataSet dsDetails = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ShadeDetail");

            Double shadesug = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeSuggestedRetail' and isnull(deleted,0)=0 and Storeid=1"));

            Double shademarkup = Convert.ToDouble(CommonComponent.GetScalarCommonData("SELECT Top 1 Configvalue FROM tb_AppConfig WHERE ConfigName ='ShadeMarkup' and isnull(deleted,0)=0 and Storeid=1"));



            string strHTMl = "";

            string strHTMLLength = "";

            string strLength = "";

            if (dsWidth != null && dsWidth.Tables.Count > 0 && dsWidth.Tables[0].Rows.Count > 0)
            {



                strHTMl += "<div><table>";

                strHTMl += "<tr><td class=\"headcol\">WIDTH ►</td>";

                strHTMLLength += "<tr><td class=\"headcol\">LENGTH ▼</td>";



                for (int i = 0; i < dsWidth.Tables[0].Rows.Count; i++)
                {

                    strHTMl += "<td class=\"long\"  style=\"padding-left: 25px;\">" + dsWidth.Tables[0].Rows[i]["value"].ToString() + "</td><td class=\"long\">Suggested</td><td class=\"long\">Our Price</td>";

                    strHTMLLength += "<td class=\"long\"></td><td class=\"long\">Retail</td><td class=\"long\">$</td>";

                    if (i == (dsWidth.Tables[0].Rows.Count - 1))
                    {

                        strHTMl += "</tr>";

                        strHTMLLength += "</tr>";

                    }





                }

                strHTMl = strHTMl + strHTMLLength;

                if (dsLength != null && dsLength.Tables.Count > 0 && dsLength.Tables[0].Rows.Count > 0)
                {

                    for (int i = 0; i < dsLength.Tables[0].Rows.Count; i++)
                    {

                        strHTMl += "<tr><td class=\"headcol\">" + dsLength.Tables[0].Rows[i]["value"].ToString() + "</td>";

                        for (int j = 0; j < dsWidth.Tables[0].Rows.Count; j++)
                        {

                            DataRow[] dr = dsDetails.Tables[0].Select("ShadeWidthID=" + dsWidth.Tables[0].Rows[j]["ShadeWidthID"].ToString() + " and ShadeLengthID=" + dsLength.Tables[0].Rows[i]["ShadeLengthID"].ToString() + "");

                            if (dr != null && dr.Length > 0)
                            {

                                foreach (DataRow dr1 in dr)
                                {

                                    strHTMl += "<td  style=\"padding-left: 25px;\">" + dr1["value"].ToString() + "</td>";

                                    strHTMl += "<td  style=\"padding-left: 25px;\">" + string.Format("{0:0.00}", Convert.ToDouble(Convert.ToDouble(dr1["value"].ToString()) * shadesug)) + "</td>";

                                    strHTMl += "<td  style=\"padding-left: 25px;\">" + string.Format("{0:0.00}", Convert.ToDouble(Convert.ToDouble(dr1["value"].ToString()) * shademarkup)) + "</td>";

                                }



                            }



                        }

                        strHTMl += "</tr>";

                    }

                }

                strHTMl += "</table></div>";

                //<td class=\"long\">24</td><td class="long">Suggested</td><td class="long">Our Price</td><td class="long">24</td><td class="long">Suggested</td><td class="long">Our Price</td><td class="long">24</td><td class="long">Suggested</td><td class="long">Our Price</td><td class="long">24</td><td class="long">Suggested</td><td class="long">Our Price</td><td class="long">24</td><td class="long">Suggested</td><td class="long">Our Price</td></tr>



            }

            divshadecalculator.InnerHtml = strHTMl;

        }

        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string StrFileName)
        {
            try
            {
                string docPath = Server.MapPath(AppLogic.AppConfigs("ShadeVendorCalc") + "Calc/") + StrFileName;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }
        /// <summary>
        /// Bind Check box based on columns specified in CSV File
        /// </summary>
        /// <param name="FileName">FileName</param>
        private void FillMapping(string FileName)
        {

            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ShadeVendorCalc") + "Calc/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            // BindDataShade();
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
                        if (tempFieldName == "width")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",width,") > -1)
                        {

                        }
                        else
                        {
                            lblMessage.Text = "File Does not contain all columns";
                            lblMessage.Style.Add("color", "#FF0000");
                            lblMessage.Style.Add("font-weight", "normal");
                        }
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 0)
                    {

                        //if (FieldStrike.ToLower().Contains("price"))
                        //    chkFields.Items.Add("Price");
                        //if (FieldStrike.ToLower().Contains("saleprice"))
                        //    chkFields.Items.Add("Sale Price");
                        //if (FieldStrike.ToLower().Contains("inventory"))
                        //    chkFields.Items.Add("Inventory");
                        //if (FieldStrike.ToLower().Contains("weight"))
                        //    chkFields.Items.Add("Weight");
                        BindDataShade();
                        //contVerify.Visible = true;
                        //DataTable dtCSV = LoadCSV(FileName);
                    }
                    else
                    {
                        lblMessage.Text = "Please Specify width Column in file.";
                        lblMessage.Style.Add("color", "#FF0000");
                        lblMessage.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    lblMessage.Text = "Please Specify width Column in file.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
        }

        /// <summary>
        /// Bind Data with Gridview
        /// </summary>
        private void BindDataShade()
        {
            DataTable dtCSV = LoadCSV(StrFileName);
            if (dtCSV.Rows.Count > 0)
            {
                //if(dtCSV.Rows[0][0].ToString().ToLower()=="width")
                //{
                if (dtCSV.Rows[0][0].ToString().ToLower() == "length")
                {

                }
                else
                {
                    lblMessage.Text = "Please Specify length in file.";
                    lblMessage.Style.Add("color", "#FF0000");
                    lblMessage.Style.Add("font-weight", "normal");
                }
                //}
                //else
                //{
                //    lblMessage.Text = "Please Specify width in file.";
                //    lblMessage.Style.Add("color", "#FF0000");
                //    lblMessage.Style.Add("font-weight", "normal");
                //}


            }
            else
                lblMessage.Text = "No data exists in file.";
            lblMessage.Style.Add("color", "#FF0000");
            lblMessage.Style.Add("font-weight", "normal");
        }

        private DataTable LoadCSV(string FileName)
        {
            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ShadeVendorCalc") + "Calc/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();

            DataSet dtRecord = new DataSet();

            //try
            //{

            //    OleDbConnection con = new OleDbConnection(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + Server.MapPath(AppLogic.AppConfigs("ShadeVendorCalc") + @"\Calc\") + @"\" + FileName.ToString() + ";Extended Properties=Excel 8.0");

            //    con.Open();

            //     DataTable dt = con.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            //   if(dt!=null && dt.Rows.Count>0)
            //   {
            //       OleDbDataAdapter da = new OleDbDataAdapter("select * from ["+dt.Rows[0][0].ToString()+"]", con);

            //       da.Fill(dtRecord);
            //   }
            //   else
            //   {
            //       lblMessage.Text = "No data exists in file.";
            //       lblMessage.Style.Add("color", "#FF0000");
            //       lblMessage.Style.Add("font-weight", "normal");
            //       return dtRecord.Tables[0];
            //   }


            //}

            //catch
            //{

            //}
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string[] FieldNames = csv.GetFieldHeaders();
                DataTable dtCSV = new DataTable();
                //DataColumn columnID = new DataColumn();
                //columnID.Caption = "Number";
                //columnID.ColumnName = "Number";
                //columnID.AllowDBNull = false;
                //columnID.AutoIncrement = true;
                //columnID.AutoIncrementSeed = 1;
                //columnID.AutoIncrementStep = 1;
                //dtCSV.Columns.Add(columnID);
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
        protected void ddlnavitemcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlnavitemcategory.SelectedIndex > 0)
            {
                ddlProductGroupCode.Items.Clear();
                DataSet dsProductGroup = new DataSet();
                dsProductGroup = CommonComponent.GetCommonDataSet("select ProductGroupCode,Description from tb_NavProductGroupCode where CatCode='" + ddlnavitemcategory.SelectedValue.ToString().Trim() + "' and isnull(active,0)=1");
                if (dsProductGroup != null && dsProductGroup.Tables.Count > 0 && dsProductGroup.Tables[0].Rows.Count > 0)
                {

                    ddlProductGroupCode.DataSource = dsProductGroup;
                    ddlProductGroupCode.DataTextField = "Description";
                    ddlProductGroupCode.DataValueField = "ProductGroupCode";
                    tditemgroup.Visible = true;
                    tditemgroup1.Visible = true;
                    ddlProductGroupCode.DataBind();
                }
                else
                {
                    ddlProductGroupCode.DataSource = null;
                    ddlProductGroupCode.DataBind();
                }

                ddlProductGroupCode.Items.Insert(0, new ListItem("Select Product Group Code", ""));

                tditemgroup.Visible = true;
                tditemgroup1.Visible = true;
            }
            else
            {
                tditemgroup.Visible = false;
                tditemgroup1.Visible = false;
            }
        }
        protected void btndeleteVideo2_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["VideoName2"] = null;
            trvideodelete2.Visible = false;
            btndeleteVideo2.Visible = false;
            lblVideoName2.Visible = false;
        }

        protected void btndeleteVideoTemp2_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void btnUploadvideo2_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(fuProductVideo2.FileName.Trim()))
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Select Video.', 'Message');});", true);
                return;
            }

            else if (!string.IsNullOrEmpty(fuProductVideo2.FileName.Trim()))
            {
                if (Path.GetExtension(fuProductVideo2.FileName.Trim()).ToLower() != ".mp4")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please upload mp4 Video file.', 'Message');});", true);
                    return;
                }
            }

            try
            {
                string videopathtemp = "";
                string productid = "";
                FileInfo VideoName = new FileInfo(fuProductVideo2.FileName);
                productid = System.IO.Path.GetFileNameWithoutExtension(VideoName.Name.ToString());
                videopathtemp = AppLogic.AppConfigs("VideoTempPath").ToString();
                string file = videopathtemp + productid + "_2.mp4";
                if (!Directory.Exists(Server.MapPath(videopathtemp)))
                    Directory.CreateDirectory(Server.MapPath(videopathtemp));

                if (File.Exists(Server.MapPath(file)))
                    File.Delete(Server.MapPath(file));

                lblVideoName2.Text = fuProductVideo2.FileName;
                fuProductVideo2.SaveAs(Server.MapPath(file));
                ViewState["VideoName2"] = productid.ToString() + "_2.mp4";

                trvideodelete2.Visible = true;
                btndeleteVideo2.Visible = true;
                lblVideoName2.Visible = true;
            }
            catch { }
        }
        protected void btnUploadvideo1_Click(object sender, ImageClickEventArgs e)
        {
            if (string.IsNullOrEmpty(fuProductVideo1.FileName.Trim()))
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Select Video.', 'Message');});", true);
                return;
            }

            else if (!string.IsNullOrEmpty(fuProductVideo1.FileName.Trim()))
            {
                if (Path.GetExtension(fuProductVideo1.FileName.Trim()).ToLower() != ".mp4")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please upload mp4 Video file.', 'Message');});", true);
                    return;
                }
            }

            try
            {
                string videopathtemp = "";
                string productid = "";
                FileInfo VideoName = new FileInfo(fuProductVideo1.FileName);
                productid = System.IO.Path.GetFileNameWithoutExtension(VideoName.Name.ToString());
                videopathtemp = AppLogic.AppConfigs("VideoTempPath").ToString();
                string file = videopathtemp + productid + "_1.mp4";
                if (!Directory.Exists(Server.MapPath(videopathtemp)))
                    Directory.CreateDirectory(Server.MapPath(videopathtemp));

                if (File.Exists(Server.MapPath(file)))
                    File.Delete(Server.MapPath(file));

                lblVideoName1.Text = fuProductVideo1.FileName;
                fuProductVideo1.SaveAs(Server.MapPath(file));
                ViewState["VideoName1"] = productid.ToString() + "_1.mp4";

                trvideodelete1.Visible = true;
                btndeleteVideo1.Visible = true;
                lblVideoName1.Visible = true;
            }
            catch { }
        }
        protected void btndeleteVideo1_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["VideoName1"] = null;
            trvideodelete1.Visible = false;
            btndeleteVideo1.Visible = false;
            lblVideoName1.Visible = false;
        }



        //protected void btnImportReplenishment_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        int pid=0;
        //        if(Request.QueryString["ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ID"].ToString()))
        //        {
        //            pid=Convert.ToInt32(Request.QueryString["ID"].ToString());
        //        }

        //       Response.Redirect("/REPLENISHMENTMANAGEMENT/ImportReplenishmentData.aspx?ID="+pid+"");
        //    }
        //    catch { }
        //}


        //private void BindRepenishableData()
        //{

        //    int pid = 0;
        //    if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "")
        //    {
        //        pid = Convert.ToInt32(Request.QueryString["ID"].ToString());
        //    }

        //    string query = "SELECT '' as SKU,'PO#' as PO1,'Qty On Order' as qty1,'ETA (Date)' as Etadate1,'PO#' as PO2,'Qty On Order' as qty2,'ETA (Date)' as Etadate2,'PO#' as PO3,'Qty On Order' as qty3,'ETA (Date)' as Etadate3,'PO#' as PO4,'Qty On Order' as qty4,'ETA (Date)' as Etadate4,-1 as DisplayOrder,0 as VariantValueID,0 as IsDiscontinued,0 as IsRepenishable,0 as IsBackorderable,'' as LeadTime Union all ";

        //    string query1 = " SELECT isnull(tb_Product.SKU,'') as SKU,cast(isnull(PO1,0) as varchar) as PO1,cast(isnull(qty1,0) as varchar) as qty1,cast(isnull(Etadate1,'')as varchar) as Etadate1,cast(isnull(PO2,0) as varchar) as PO2,cast(isnull(qty2,0)as varchar) as qty2,cast(isnull(Etadate2,'')as varchar) as Etadate2,cast(isnull(PO3,0)as varchar) as PO3,cast(isnull(qty3,0)as varchar) as qty3,cast(isnull(Etadate3,'')as varchar) as Etadate3,cast(isnull(PO4,0)as varchar) as PO4,cast(isnull(qty4,0)as varchar) as qty4,cast(isnull(Etadate4,'')as varchar) as Etadate4,isnull(DisplayOrder,0) as DisplayOrder,0 as VariantValueID,0 as IsDiscontinued,0 as IsRepenishable,0 as IsBackorderable,isnull(LeadTime,'') as LeadTime  from   tb_Product  left outer join  tb_Replenishment on tb_Product.ProductID = tb_Replenishment.ProductID where tb_Replenishment.productid=" + pid + " and  isnull(tb_Replenishment.VariantValueID,0)=0  union all ";

        //    string query2 = "SELECT isnull(tb_ProductVariantValue.SKU,'') as SKU,cast(isnull(PO1,0) as varchar) as PO1,cast(isnull(qty1,0) as varchar) as qty1,cast(isnull(Etadate1,'')as varchar) as Etadate1,cast(isnull(PO2,0) as varchar) as PO2,cast(isnull(qty2,0)as varchar) as qty2,cast(isnull(Etadate2,'')as varchar) as Etadate2,cast(isnull(PO3,0)as varchar) as PO3,cast(isnull(qty3,0)as varchar) as qty3,cast(isnull(Etadate3,'')as varchar) as Etadate3,cast(isnull(PO4,0)as varchar) as PO4,cast(isnull(qty4,0)as varchar) as qty4,cast(isnull(Etadate4,'')as varchar) as Etadate4,isnull(DisplayOrder,0) as DisplayOrder,tb_ProductVariantValue.VariantValueID,0 as IsDiscontinued,0 as IsRepenishable,0 as IsBackorderable,isnull(LeadTime,'') as LeadTime  from   tb_ProductVariantValue  left outer join  tb_Replenishment on tb_ProductVariantValue.VariantValueID = tb_Replenishment.VariantValueID where tb_ProductVariantValue.productid="+pid+"  and  isnull(tb_ProductVariantValue.sku,'')<>'' and  tb_Replenishment.sku not like '%-cus%'  ";
        //    query = query + query1 + query2;
        //    DataSet dsData = new DataSet();
        //    dsData = CommonComponent.GetCommonDataSet(query);
        //    if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
        //    {
        //        grdREPLENISHMENT1.DataSource = dsData.Tables[0];
        //        grdREPLENISHMENT1.DataBind();
        //        if (dsData.Tables[0].Rows.Count > 1)
        //        {


        //            btnRepleshmentsave1.Visible = true;
        //        }
        //        else
        //        {
        //            btnRepleshmentsave1.Visible = false;
        //        }



        //    }
        //    else
        //    {
        //        grdREPLENISHMENT1.DataSource = null;
        //        grdREPLENISHMENT1.DataBind();
        //        btnRepleshmentsave1.Visible = false;
        //    }

        //}


        //protected void grdREPLENISHMENT1_RowDataBound(object sender, GridViewRowEventArgs e)
        //{
        //    if (e.Row.RowType == DataControlRowType.DataRow)
        //    {

        //        TextBox txtPO1 = (TextBox)e.Row.FindControl("txtPO1");
        //        TextBox txtPO2 = (TextBox)e.Row.FindControl("txtPO2");
        //        TextBox txtPO3 = (TextBox)e.Row.FindControl("txtPO3");
        //        TextBox txtPO4 = (TextBox)e.Row.FindControl("txtPO4");

        //        TextBox txtqty1 = (TextBox)e.Row.FindControl("txtqty1");
        //        TextBox txtqty2 = (TextBox)e.Row.FindControl("txtqty2");
        //        TextBox txtqty3 = (TextBox)e.Row.FindControl("txtqty3");
        //        TextBox txtqty4 = (TextBox)e.Row.FindControl("txtqty4");

        //        TextBox txtEtadate1 = (TextBox)e.Row.FindControl("txtEtadate1");
        //        TextBox txtEtadate2 = (TextBox)e.Row.FindControl("txtEtadate2");
        //        TextBox txtEtadate3 = (TextBox)e.Row.FindControl("txtEtadate3");
        //        TextBox txtEtadate4 = (TextBox)e.Row.FindControl("txtEtadate4");




        //        Label lblPO1 = (Label)e.Row.FindControl("lblPO1");
        //        Label lblPO2 = (Label)e.Row.FindControl("lblPO2");
        //        Label lblPO3 = (Label)e.Row.FindControl("lblPO3");
        //        Label lblPO4 = (Label)e.Row.FindControl("lblPO4");

        //        Label lblqty1 = (Label)e.Row.FindControl("lblqty1");
        //        Label lblqty2 = (Label)e.Row.FindControl("lblqty2");
        //        Label lblqty3 = (Label)e.Row.FindControl("lblqty3");
        //        Label lblqty4 = (Label)e.Row.FindControl("lblqty4");

        //        Label lblEtadate1 = (Label)e.Row.FindControl("lblEtadate1");
        //        Label lblEtadate2 = (Label)e.Row.FindControl("lblEtadate2");
        //        Label lblEtadate3 = (Label)e.Row.FindControl("lblEtadate3");
        //        Label lblEtadate4 = (Label)e.Row.FindControl("lblEtadate4");

        //        Label lblvariantsku = (Label)e.Row.FindControl("lblvariantsku");
        //        TextBox txtvariantsku = (TextBox)e.Row.FindControl("txtvariantsku");



        //        if (txtPO1.Text.ToString().Trim()=="0")
        //        {
        //            txtPO1.Text = "";

        //        }
        //        if (txtPO2.Text.ToString().Trim() == "0")
        //        {
        //            txtPO2.Text = "";

        //        }
        //        if (txtPO3.Text.ToString().Trim() == "0")
        //        {
        //            txtPO3.Text = "";

        //        }
        //        if (txtPO4.Text.ToString().Trim() == "0")
        //        {
        //            txtPO4.Text = "";

        //        }

        //        if (txtqty1.Text.ToString().Trim() == "0")
        //        {
        //            txtqty1.Text = "";

        //        }
        //        if (txtqty2.Text.ToString().Trim() == "0")
        //        {
        //            txtqty2.Text = "";

        //        }
        //        if (txtqty3.Text.ToString().Trim() == "0")
        //        {
        //            txtqty3.Text = "";

        //        }
        //        if (txtqty4.Text.ToString().Trim() == "0")
        //        {
        //            txtqty4.Text = "";

        //        }

        //        if (String.IsNullOrEmpty(txtEtadate1.Text) || txtEtadate1.Text.ToString().ToLower().IndexOf("1900") > -1)
        //        {
        //            txtEtadate1.Text = "";
        //        }
        //        if (String.IsNullOrEmpty(txtEtadate2.Text) || txtEtadate2.Text.ToString().ToLower().IndexOf("1900") > -1)
        //        {
        //            txtEtadate2.Text = "";
        //        }

        //        if (String.IsNullOrEmpty(txtEtadate3.Text) || txtEtadate3.Text.ToString().ToLower().IndexOf("1900") > -1)
        //        {
        //            txtEtadate3.Text = "";
        //        }

        //        if (String.IsNullOrEmpty(txtEtadate4.Text) || txtEtadate4.Text.ToString().ToLower().IndexOf("1900") > -1)
        //        {
        //            txtEtadate4.Text = "";
        //        }

        //        DateTime Etadate1;
        //        DateTime.TryParse(txtEtadate1.Text.ToString(), out Etadate1);
        //        DateTime Etadate2;
        //        DateTime.TryParse(txtEtadate2.Text.ToString(), out Etadate2);
        //        DateTime Etadate3;
        //        DateTime.TryParse(txtEtadate3.Text.ToString(), out Etadate3);
        //        DateTime Etadate4;
        //        DateTime.TryParse(txtEtadate4.Text.ToString(), out Etadate4);


        //        //if(!String.IsNullOrEmpty(txtEtadate1.Text))
        //        //{
        //        //    txtEtadate1.Text = Etadate1.AddDays(-ReplacementAddDays).ToString("MM/dd/yyyy");

        //        //}
        //        //if (!String.IsNullOrEmpty(txtEtadate2.Text))
        //        //{

        //        //    txtEtadate2.Text = Etadate2.AddDays(-ReplacementAddDays).ToString("MM/dd/yyyy");
        //        //}
        //        //if (!String.IsNullOrEmpty(txtEtadate3.Text))
        //        //{
        //        //    txtEtadate3.Text = Etadate3.AddDays(-ReplacementAddDays).ToString("MM/dd/yyyy");

        //        //}
        //        //if (!String.IsNullOrEmpty(txtEtadate4.Text))
        //        //{

        //        //    txtEtadate4.Text = Etadate4.AddDays(-ReplacementAddDays).ToString("MM/dd/yyyy");
        //        //}

        //        //    Funname += "<script type=\"text/javascript\">";
        //        Funname += "$j('#" + txtEtadate1.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";

        //        Funname += " $j('#" + txtEtadate2.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
        //        Funname += " $j('#" + txtEtadate3.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
        //        Funname += " $j('#" + txtEtadate4.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
        //        // Funname += "</script>";

        //        if (e.Row.RowIndex == 0)
        //        {
        //            txtPO1.Visible = false;
        //            txtPO2.Visible = false;
        //            txtPO3.Visible = false;
        //            txtPO4.Visible = false;

        //            txtqty1.Visible = false;
        //            txtqty2.Visible = false;
        //            txtqty3.Visible = false;
        //            txtqty4.Visible = false;

        //            txtEtadate1.Visible = false;
        //            txtEtadate2.Visible = false;
        //            txtEtadate3.Visible = false;
        //            txtEtadate4.Visible = false;

        //            txtvariantsku.Visible = false;
        //        }
        //        else
        //        {
        //            lblPO1.Visible = false;
        //            lblPO2.Visible = false;
        //            lblPO3.Visible = false;
        //            lblPO4.Visible = false;

        //            lblqty1.Visible = false;
        //            lblqty2.Visible = false;
        //            lblqty3.Visible = false;
        //            lblqty4.Visible = false;

        //            lblEtadate1.Visible = false;
        //            lblEtadate2.Visible = false;
        //            lblEtadate3.Visible = false;
        //            lblEtadate4.Visible = false;
        //            txtvariantsku.Visible = false;
        //        }




        //    }
        //    if (e.Row.RowType == DataControlRowType.Footer)
        //    {
        //        // Funname = " var $j = jQuery.noConflict(); $j(function () {" + Funname.ToString() + "});";
        //        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Funname", Funname, true);
        //    }
        //}

        //protected void btnRepleshmentsave1_Click(object sender, ImageClickEventArgs e)
        //{
        //    try
        //    {
        //        if (grdREPLENISHMENT1.Rows.Count > 0)
        //        {

        //            DataTable Dt = new DataTable();
        //            Dt.Columns.Add("Number", typeof(int));
        //            Dt.Columns.Add("sku", typeof(string));
        //            Dt.Columns.Add("name", typeof(string));
        //            Dt.Columns.Add("PO#1", typeof(string));
        //            Dt.Columns.Add("PO#2", typeof(string));
        //            Dt.Columns.Add("PO#3", typeof(string));
        //            Dt.Columns.Add("PO#4", typeof(string));
        //            Dt.Columns.Add("Quantity Ordered1", typeof(string));
        //            Dt.Columns.Add("Quantity Ordered2", typeof(string));
        //            Dt.Columns.Add("Quantity Ordered3", typeof(string));
        //            Dt.Columns.Add("Quantity Ordered4", typeof(string));
        //            Dt.Columns.Add("Shipping Date1", typeof(string));
        //            Dt.Columns.Add("Shipping Date2", typeof(string));
        //            Dt.Columns.Add("Shipping Date3", typeof(string));
        //            Dt.Columns.Add("Shipping Date4", typeof(string));
        //            Dt.AcceptChanges();

        //            for (int i = 1; i < grdREPLENISHMENT1.Rows.Count; i++)
        //            {
        //                TextBox txtPO1 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtPO1");
        //                TextBox txtPO2 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtPO2");
        //                TextBox txtPO3 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtPO3");
        //                TextBox txtPO4 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtPO4");

        //                TextBox txtqty1 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtqty1");
        //                TextBox txtqty2 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtqty2");
        //                TextBox txtqty3 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtqty3");
        //                TextBox txtqty4 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtqty4");

        //                TextBox txtEtadate1 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtEtadate1");
        //                TextBox txtEtadate2 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtEtadate2");
        //                TextBox txtEtadate3 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtEtadate3");
        //                TextBox txtEtadate4 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtEtadate4");

        //                Label lblPO1 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblPO1");
        //                Label lblPO2 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblPO2");
        //                Label lblPO3 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblPO3");
        //                Label lblPO4 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblPO4");

        //                Label lblqty1 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblqty1");
        //                Label lblqty2 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblqty2");
        //                Label lblqty3 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblqty3");
        //                Label lblqty4 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblqty4");

        //                Label lblEtadate1 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblEtadate1");
        //                Label lblEtadate2 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblEtadate2");
        //                Label lblEtadate3 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblEtadate3");
        //                Label lblEtadate4 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblEtadate4");

        //                Label lblvariantsku = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblvariantsku");
        //                TextBox txtvariantsku = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtvariantsku");
        //                DataRow dr = Dt.NewRow();
        //                dr["Number"] = i - 1;
        //                dr["sku"] = lblvariantsku.Text.ToString().Trim();
        //                dr["name"] = txtProductName.Text.ToString().Replace("'", "''");
        //                dr["PO#1"] = txtPO1.Text.ToString();
        //                dr["PO#2"] = txtPO2.Text.ToString();
        //                dr["PO#3"] = txtPO3.Text.ToString();
        //                dr["PO#4"] = txtPO4.Text.ToString();
        //                dr["Quantity Ordered1"] = txtqty1.Text.ToString();
        //                dr["Quantity Ordered2"] = txtqty2.Text.ToString();
        //                dr["Quantity Ordered3"] = txtqty3.Text.ToString();
        //                dr["Quantity Ordered4"] = txtqty4.Text.ToString();
        //                dr["Shipping Date1"] = txtEtadate1.Text.ToString();
        //                dr["Shipping Date2"] = txtEtadate2.Text.ToString();
        //                dr["Shipping Date3"] = txtEtadate3.Text.ToString();
        //                dr["Shipping Date4"] = txtEtadate4.Text.ToString();
        //                Dt.Rows.Add(dr);
        //                Dt.AcceptChanges();

        //            }


        //            if (Dt.Rows.Count > 0)
        //            {
        //                validatefiledata(Dt);
        //            }


        //        }
        //    }
        //    catch { }

        //}




        //private void validatefiledata(DataTable Dt)
        //{
        //    Int32 po1 = 0;
        //    Int32 po2 = 0;
        //    Int32 po3 = 0;
        //    Int32 po4 = 0;
        //    Int32 qty1 = 0;
        //    Int32 qty2 = 0;
        //    Int32 qty3 = 0;
        //    Int32 qty4 = 0;
        //    DateTime shipping1;
        //    DateTime shipping2;
        //    DateTime shipping3;
        //    DateTime shipping4;
        //    String sku = "";
        //    String Name = "";
        //    Int32 error = 0;
        //    Int32 error2 = 0;
        //    int flag = 0;



        //    //lblskucount.Text = Dt.Rows.Count.ToString();
        //    //Totalsku = Dt.Rows.Count;
        //    lblerrorsku.Text = ",";
        //    for (int i = 0; i < Dt.Rows.Count; i++)
        //    {
        //        error = 0;
        //        error2 = 0;

        //        try
        //        {
        //            if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
        //            {
        //                sku = Dt.Rows[i]["sku"].ToString();



        //                if (!CheckSkuExist(sku, Dt))
        //                {
        //                    if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                        lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    error2++;
        //                }

        //                if (!CheckDuplicateSku(sku, Dt))
        //                {
        //                    if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                        lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    error2++;
        //                }


        //            }
        //            else
        //            {
        //                String ErrorType = "Missing SKU";
        //                String ExcelRowNumber = Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Number"].ToString()) + 1);
        //                String ProductSKU = "";
        //                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                InsertErrorinTemp("No Replenishment Details Found", "0", "");
        //                error++;
        //            }
        //        }
        //        catch
        //        {
        //            if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //            error++;

        //        }

        //        if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
        //        {



        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["name"].ToString()))
        //                {
        //                    Name = Dt.Rows[i]["name"].ToString();
        //                }
        //                else
        //                {
        //                    //error++;
        //                }
        //            }
        //            catch
        //            {
        //                error++;
        //            }


        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#1"].ToString()))
        //                {
        //                    po1 = Convert.ToInt32(Dt.Rows[i]["PO#1"].ToString());
        //                }
        //                else
        //                {
        //                    //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    //error++;
        //                }
        //            }
        //            catch
        //            {
        //                //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                //error++;
        //            }

        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#2"].ToString()))
        //                {
        //                    po2 = Convert.ToInt32(Dt.Rows[i]["PO#2"].ToString());
        //                }
        //                else
        //                {
        //                    //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    //error++;
        //                }
        //            }
        //            catch
        //            {
        //                //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                //error++;
        //            }
        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#3"].ToString()))
        //                {
        //                    po3 = Convert.ToInt32(Dt.Rows[i]["PO#3"].ToString());
        //                }
        //                else
        //                {
        //                    //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    //error++;
        //                }
        //            }
        //            catch
        //            {
        //                //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                //error++;
        //            }

        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#4"].ToString()))
        //                {
        //                    po4 = Convert.ToInt32(Dt.Rows[i]["PO#4"].ToString());
        //                }
        //                else
        //                {
        //                    //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    //error++;
        //                }
        //            }
        //            catch
        //            {
        //                //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                //error++;
        //            }



        //            //if (!CheckPOUnique(po1, po2, po3, po4, sku, Convert.ToInt32(Dt.Rows[i]["Number"].ToString())))
        //            //{
        //            //    error++;
        //            //}


        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered1"].ToString()))
        //                {
        //                    qty1 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered1"].ToString());
        //                }
        //                else
        //                {
        //                    //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    //error++;
        //                }
        //            }
        //            catch
        //            {
        //                //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                //error++;
        //            }

        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered2"].ToString()))
        //                {
        //                    qty2 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered2"].ToString());
        //                }
        //                else
        //                {
        //                    //    if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    //        lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    //    error++;
        //                }
        //            }
        //            catch
        //            {
        //                //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                //error++;
        //            }
        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered3"].ToString()))
        //                {
        //                    qty3 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered3"].ToString());
        //                }
        //                else
        //                {
        //                    //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    //error++;
        //                }
        //            }
        //            catch
        //            {
        //                //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                //error++;
        //            }

        //            try
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered4"].ToString()))
        //                {
        //                    qty4 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered4"].ToString());
        //                }
        //                else
        //                {
        //                    //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                    //error++;
        //                }
        //            }
        //            catch
        //            {
        //                //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                //error++;
        //            }


        //            if (!CheckDeliveryDateinsequence(sku, Convert.ToString(Dt.Rows[i]["PO#1"]), Convert.ToString(Dt.Rows[i]["PO#2"]), Convert.ToString(Dt.Rows[i]["PO#3"]), Convert.ToString(Dt.Rows[i]["PO#4"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered1"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered2"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered3"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered4"]), Convert.ToString(Dt.Rows[i]["Shipping Date1"]), Convert.ToString(Dt.Rows[i]["Shipping Date2"]), Convert.ToString(Dt.Rows[i]["Shipping Date3"]), Convert.ToString(Dt.Rows[i]["Shipping Date4"]), Convert.ToInt32(Dt.Rows[i]["Number"].ToString()) + 1))
        //            {
        //                if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
        //                    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
        //                error++;
        //            }




        //        }

        //        // Int32 c = dterror.Rows.Count;

        //        if (error == 0 && error2 == 0)
        //        {
        //            replnishedsku = replnishedsku + 1;
        //        }
        //        else
        //        {
        //            //if (lblMsg.Text.ToString().IndexOf("Data does not match field data format") <= -1)
        //            //    lblMsg.Text += "Data does not match field data format";
        //        }

        //    }


        //    String strErrors = "";
        //    if (dterror.Rows.Count > 0)
        //    {
        //        strErrors += "<table class=\"table table-bordered table-striped table-condensed cf\">";
        //        strErrors += "<thead><tr class=\"cf\">";
        //        strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">#</th>";
        //        strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">SKU</th>";
        //        strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\"> Row #s</th>";
        //        strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">Error Description</th>";
        //        strErrors += "</tr></thead>";
        //        for (int k = 0; k < dterror.Rows.Count; k++)
        //        {

        //            if (dterror.Rows[k][1].ToString().Replace("'", "''") == "0")
        //            {
        //                strErrors += "<tbody>";
        //                strErrors += "<tr>";
        //                strErrors += "<td align=\"center\" colspan=\"4\" style=\"color:red;\">";
        //                strErrors += "Warning Message: " + dterror.Rows[k][0].ToString().Replace("'", "''");
        //                strErrors += "</td>";
        //                strErrors += "</tr>";
        //                strErrors += "</tbody>";
        //            }
        //            else
        //            {
        //                strErrors += "<tbody>";
        //                strErrors += "<tr>";
        //                strErrors += "<td align=\"left\" style=\"color:blue;\">";
        //                strErrors += k + 1;
        //                strErrors += "</td>";
        //                strErrors += "<td align=\"left\" style=\"color:blue;\">";
        //                strErrors += dterror.Rows[k][2].ToString().Replace("'", "''");
        //                strErrors += "</td>";
        //                strErrors += "<td align=\"left\" style=\"color:blue;\">";
        //                strErrors += dterror.Rows[k][1].ToString().Replace("'", "''");
        //                strErrors += "</td>";
        //                strErrors += "<td align=\"left\" style=\"color:blue;\">";
        //                strErrors += dterror.Rows[k][0].ToString().Replace("'", "''");
        //                strErrors += "</td>";
        //                strErrors += "</tr>";
        //                strErrors += "</tbody>";
        //            }


        //        }
        //        strErrors += "</table>";
        //    }
        //    ltrErrors.Text = "";
        //    ltrErrors.Text = strErrors;
        //    if (String.IsNullOrEmpty(strErrors.ToString()))
        //    {
        //        // diverror.Visible = false;



        //        SaveReplenishmentData(Dt);


        //        //lblMsg.Text = "Successfull";
        //    }

        //    lblreplenishedskucount.Text = replnishedsku.ToString();
        //    try
        //    {
        //        lblerrorsku.Text = lblerrorsku.Text.ToString().Remove(lblerrorsku.Text.ToString().LastIndexOf(","));
        //    }
        //    catch { }
        //    if (!String.IsNullOrEmpty(lblerrorsku.Text) && lblerrorsku.Text.ToString().Length > 2)
        //    {
        //        lblerrorsku.Visible = false;

        //    }
        //    else
        //    {
        //        lblerrorsku.Visible = false;

        //    }




        //    for(int y=0;y<grdREPLENISHMENT1.Rows.Count;y++)
        //    {

        //        try
        //        {

        //            TextBox txtEtadate1 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtEtadate1");
        //            TextBox txtEtadate2 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtEtadate2");
        //            TextBox txtEtadate3 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtEtadate3");
        //            TextBox txtEtadate4 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtEtadate4");


        //            DateTime Etadate1;
        //            DateTime.TryParse(txtEtadate1.Text.ToString(), out Etadate1);
        //            DateTime Etadate2;
        //            DateTime.TryParse(txtEtadate2.Text.ToString(), out Etadate2);
        //            DateTime Etadate3;
        //            DateTime.TryParse(txtEtadate3.Text.ToString(), out Etadate3);
        //            DateTime Etadate4;
        //            DateTime.TryParse(txtEtadate4.Text.ToString(), out Etadate4);




        //            //    Funname += "<script type=\"text/javascript\">";
        //            Funname += "$j('#" + txtEtadate1.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";

        //            Funname += " $j('#" + txtEtadate2.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
        //            Funname += " $j('#" + txtEtadate3.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
        //            Funname += " $j('#" + txtEtadate4.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
        //        }
        //        catch { }

        //    }






        //}



        //private void SaveReplenishmentData(DataTable Dt)
        //{

        //        if (Dt != null && Dt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < Dt.Rows.Count; i++)
        //            {
        //                if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
        //                {

        //                    Int32 po1 = 0;
        //                    Int32 po2 = 0;
        //                    Int32 po3 = 0;
        //                    Int32 po4 = 0;
        //                    Int32 qty1 = 0;
        //                    Int32 qty2 = 0;
        //                    Int32 qty3 = 0;
        //                    Int32 qty4 = 0;

        //                    String sku = "";
        //                    String Name = "";
        //                    String PID = "";
        //                    String VariantValueID = "";



        //                    sku = Dt.Rows[i]["sku"].ToString();
        //                    if (lblerrorsku.Text.ToString().IndexOf("," + sku + ",") <= -1)
        //                    {


        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
        //                            {
        //                                sku = Dt.Rows[i]["sku"].ToString();
        //                            }

        //                        }
        //                        catch
        //                        {

        //                        }

        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["name"].ToString()))
        //                            {
        //                                Name = Dt.Rows[i]["name"].ToString();
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }


        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#1"].ToString()))
        //                            {
        //                                po1 = Convert.ToInt32(Dt.Rows[i]["PO#1"].ToString());
        //                            }
        //                            else
        //                            {
        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }

        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#2"].ToString()))
        //                            {
        //                                po2 = Convert.ToInt32(Dt.Rows[i]["PO#2"].ToString());
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }
        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#3"].ToString()))
        //                            {
        //                                po3 = Convert.ToInt32(Dt.Rows[i]["PO#3"].ToString());
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }

        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#4"].ToString()))
        //                            {
        //                                po4 = Convert.ToInt32(Dt.Rows[i]["PO#4"].ToString());
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }



        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered1"].ToString()))
        //                            {
        //                                qty1 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered1"].ToString());
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }

        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered2"].ToString()))
        //                            {
        //                                qty2 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered2"].ToString());
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }
        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered3"].ToString()))
        //                            {
        //                                qty3 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered3"].ToString());
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }

        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered4"].ToString()))
        //                            {
        //                                qty4 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered4"].ToString());
        //                            }
        //                            else
        //                            {

        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }
        //                        TextBox txtEtadate1 = new TextBox();
        //                        TextBox txtEtadate2 = new TextBox();
        //                        TextBox txtEtadate3 = new TextBox();
        //                        TextBox txtEtadate4 = new TextBox();
        //                        try
        //                        {
        //                            DateTime Etadate1;
        //                            DateTime.TryParse(Dt.Rows[i]["Shipping Date1"].ToString(), out Etadate1);
        //                            DateTime Etadate2;
        //                            DateTime.TryParse(Dt.Rows[i]["Shipping Date2"].ToString(), out Etadate2);
        //                            DateTime Etadate3;
        //                            DateTime.TryParse(Dt.Rows[i]["Shipping Date3"].ToString(), out Etadate3);
        //                            DateTime Etadate4;
        //                            DateTime.TryParse(Dt.Rows[i]["Shipping Date4"].ToString(), out Etadate4);



        //                            if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
        //                            {
        //                                // txtEtadate1.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
        //                            }
        //                            else
        //                            {

        //                               txtEtadate1.Text = Etadate1.ToString("MM/dd/yyyy");
        //                            }
        //                            if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
        //                            {
        //                                //  txtEtadate2.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
        //                            }
        //                            else
        //                            {
        //                                txtEtadate2.Text = Etadate2.ToString("MM/dd/yyyy");
        //                            }
        //                            if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
        //                            {
        //                                // txtEtadate3.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
        //                            }
        //                            else
        //                            {
        //                                txtEtadate3.Text = Etadate3.ToString("MM/dd/yyyy");
        //                            }
        //                            if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
        //                            {
        //                                //  txtEtadate4.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
        //                            }
        //                            else
        //                            {
        //                                txtEtadate4.Text = Etadate4.ToString("MM/dd/yyyy");
        //                            }

        //                        }
        //                        catch { }


        //                        try
        //                        {

        //                            PID = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 productid from tb_product where isnull(sku,'')='" + sku + "' and isnull(deleted,0)=0 and storeid=1 order by productid Desc"));
        //                        }
        //                        catch { }
        //                        try
        //                        {
        //                            if (String.IsNullOrEmpty(PID))
        //                            {
        //                                DataSet dsproduct = new DataSet();
        //                                dsproduct = (CommonComponent.GetCommonDataSet("select top 1 VariantValueID,productid from tb_ProductVariantValue where isnull(sku,'')='" + sku + "' and productid in (select productid from tb_product where isnull(deleted,0)=0 and storeid=1) order by VariantValueID Desc "));
        //                                if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
        //                                {
        //                                    PID = dsproduct.Tables[0].Rows[0]["productid"].ToString();
        //                                    VariantValueID = dsproduct.Tables[0].Rows[0]["VariantValueID"].ToString();
        //                                }

        //                            }
        //                            else
        //                            {
        //                                VariantValueID = "0";
        //                            }
        //                        }
        //                        catch
        //                        {

        //                        }

        //                        try
        //                        {
        //                            if (!String.IsNullOrEmpty(PID) && !String.IsNullOrEmpty(VariantValueID))
        //                            {
        //                                Int32 count = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(productid) from tb_Replenishment where ProductID=" + PID + " and VariantValueID=" + VariantValueID + ""));
        //                                if (count > 0)
        //                                {
        //                                    //update

        //                                    CommonComponent.ExecuteCommonData("update [dbo].[tb_Replenishment] set [PO1]=" + po1 + ",[qty1]=" + qty1 + ",[Etadate1]='" + txtEtadate1.Text + "',[PO2]=" + po2 + ",[qty2]=" + qty2 + ",[Etadate2]='" + txtEtadate2.Text + "',[PO3]=" + po3 + ",[qty3]=" + qty3 + ",[Etadate3]='" + txtEtadate3.Text + "',[PO4]=" + po4 + ",[qty4]=" + qty4 + ",[Etadate4]='" + txtEtadate4.Text + "',sku='" + sku + "',name='" + Name + "',FileID=0,UpdatedBy=" + Session["AdminID"].ToString() + ",UpdatedOn=getdate() where VariantValueID=" + VariantValueID + " and Productid=" + PID + " ");

        //                                }
        //                                else
        //                                {
        //                                    //insert
        //                                    CommonComponent.ExecuteCommonData("insert into [dbo].[tb_Replenishment] ([ProductID],[VariantValueID],[PO1],[qty1],[Etadate1],[PO2],[qty2],[Etadate2],[PO3],[qty3],[Etadate3],[PO4],[qty4],[Etadate4],[CreatedOn],[sku],[Name],[CreatedBy],[FileID]) values (" + PID + "," + VariantValueID + "," + po1 + "," + qty1 + ",'" + txtEtadate1.Text + "'," + po2 + "," + qty2 + ",'" + txtEtadate2.Text + "'," + po3 + "," + qty3 + ",'" + txtEtadate3.Text + "'," + po4 + "," + qty4 + ",'" + txtEtadate4.Text + "',getdate(),'" + sku + "','" + Name + "'," + Session["AdminID"].ToString() + ",0)");

        //                                }


        //                            }
        //                        }
        //                        catch { }



        //                    }
        //                }
        //            }



        //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "mess", "alert('Replenishment Data Uploaded Successfully')", true);

        //            BindRepenishableData();

        //        }
        //}

        //private void AddColumnsErrortable()
        //{
        //    try
        //    {
        //        DataColumn col1 = new DataColumn("Error Type", typeof(string));
        //        dterror.Columns.Add(col1);
        //        DataColumn col2 = new DataColumn("Excel Row Number", typeof(string));
        //        dterror.Columns.Add(col2);
        //        DataColumn col3 = new DataColumn("Product SKU", typeof(string));
        //        dterror.Columns.Add(col3);

        //        dterror.AcceptChanges();
        //    }
        //    catch { }

        //}

        //private void InsertErrorinTemp(String ErrorType, String ExcelRowNumber, String ProductSKU)
        //{
        //    try
        //    {
        //        DataRow dr = null;
        //        if (dterror.Columns.Count > 0)
        //        {
        //            dr = dterror.NewRow();
        //            dr["Error Type"] = ErrorType;
        //            dr["Excel Row Number"] = ExcelRowNumber;
        //            dr["Product SKU"] = ProductSKU;
        //            dterror.Rows.Add(dr);
        //            dterror.AcceptChanges();
        //        }
        //        else
        //        {
        //            AddColumnsErrortable();
        //            dr = dterror.NewRow();
        //            dr["Error Type"] = ErrorType;
        //            dr["Excel Row Number"] = ExcelRowNumber;
        //            dr["Product SKU"] = ProductSKU;
        //            dterror.Rows.Add(dr);
        //            dterror.AcceptChanges();

        //        }
        //    }
        //    catch { }
        //}

        //private bool CheckDuplicateSku(String Sku, DataTable Dt)
        //{

        //    String ErrorType = "Duplicate SKUs";
        //    String ExcelRowNumber = "";
        //    String ProductSKU = "";
        //    DataRow[] drerror = Dt.Select("sku ='" + Sku + "'");
        //    if (drerror.Length > 1)
        //    {
        //        for (int i = 0; i < drerror.Length; i++)
        //        {
        //            ExcelRowNumber += Convert.ToInt32(drerror[i]["Number"].ToString()) + 1 + ",";
        //            ProductSKU = drerror[i]["sku"].ToString();
        //        }


        //        if (ExcelRowNumber.ToString().Length > 2)
        //        {
        //            ExcelRowNumber = ExcelRowNumber.ToString().Remove(ExcelRowNumber.ToString().LastIndexOf(","));
        //        }

        //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //        return false;

        //    }
        //    else
        //    {
        //        return true;
        //    }

        //}





        //private bool CheckSkuExist(String Sku, DataTable Dt)
        //{
        //    String ErrorType = "Invalid SKU";
        //    String ExcelRowNumber = "";
        //    String ProductSKU = "";
        //    string checksku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_product where sku='" + Sku + "' and storeid=1 and  isnull(deleted,0)=0"));
        //    if (String.IsNullOrEmpty(checksku))
        //    {
        //        string checksubsku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_ProductVariantValue where sku='" + Sku + "' and productid in (select productid from tb_product where storeid=1  and isnull(deleted,0)=0)"));
        //        if (String.IsNullOrEmpty(checksubsku))
        //        {
        //            DataRow[] drerror = Dt.Select("sku ='" + Sku + "'");
        //            if (drerror.Length > 0)
        //            {
        //                for (int i = 0; i < drerror.Length; i++)
        //                {
        //                    ExcelRowNumber += Convert.ToInt32(drerror[i]["Number"].ToString()) + 1 + ",";
        //                    ProductSKU = drerror[i]["sku"].ToString();
        //                }


        //                if (ExcelRowNumber.ToString().Length > 0)
        //                {
        //                    ExcelRowNumber = ExcelRowNumber.ToString().Remove(ExcelRowNumber.ToString().LastIndexOf(","));
        //                }

        //                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                return false;

        //            }
        //            else
        //            {
        //                return false;
        //            }


        //        }
        //        else
        //        {
        //            checksubsku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_ProductVariantValue where sku='" + Sku + "' and isnull(varactive,0)=1 and productid in (select productid from tb_product where storeid=1 and isnull(active,0)=1 and isnull(deleted,0)=0)"));
        //            if (String.IsNullOrEmpty(checksubsku))
        //            {
        //                InsertErrorinTemp("Product is Inactive  SKU : " + Sku, "0", "");
        //            }
        //            return true;
        //        }

        //    }
        //    else
        //    {


        //        checksku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_product where sku='" + Sku + "' and storeid=1 and isnull(active,0)=1 and  isnull(deleted,0)=0"));
        //        if (String.IsNullOrEmpty(checksku))
        //        {
        //            InsertErrorinTemp("Product is Inactive  SKU : " + Sku, "0", "");
        //        }
        //        return true;
        //    }

        //}


        //private bool CheckPOUnique(Int32 po1, Int32 po2, Int32 po3, Int32 po4, String Sku, Int32 RowNumber, Int32 POBlocks)
        //{
        //    try
        //    {
        //        String ErrorType = "Invalid PO # Value";
        //        String ExcelRowNumber = "";
        //        String ProductSKU = "";

        //        DataTable tb = new DataTable();
        //        DataColumn col1 = new DataColumn("PO", typeof(string));
        //        tb.Columns.Add(col1);
        //        DataColumn col2 = new DataColumn("num", typeof(string));
        //        tb.Columns.Add(col2);
        //        tb.AcceptChanges();
        //        Int32 er = 0;

        //        if (POBlocks >= 1)
        //        {
        //            if (po1 > 0)
        //            {
        //                DataRow dd = tb.NewRow();
        //                dd["PO"] = po1;
        //                dd["num"] = "1";
        //                tb.Rows.Add(dd);
        //                tb.AcceptChanges();
        //            }
        //        }
        //        if (POBlocks >= 2)
        //        {
        //            if (po2 > 0)
        //            {
        //                DataRow dd = tb.NewRow();
        //                dd["PO"] = po2;
        //                dd["num"] = "2";
        //                tb.Rows.Add(dd);
        //                tb.AcceptChanges();
        //            }
        //        }
        //        if (POBlocks >= 3)
        //        {
        //            if (po3 > 0)
        //            {
        //                DataRow dd = tb.NewRow();
        //                dd["PO"] = po3;
        //                dd["num"] = "3";
        //                tb.Rows.Add(dd);
        //                tb.AcceptChanges();
        //            }
        //        }
        //        if (POBlocks >= 4)
        //        {
        //            if (po4 > 0)
        //            {
        //                DataRow dd = tb.NewRow();
        //                dd["PO"] = po4;
        //                dd["num"] = "4";
        //                tb.Rows.Add(dd);
        //                tb.AcceptChanges();
        //            }
        //        }

        //        if (tb.Rows.Count > 0)
        //        {
        //            for (int j = 0; j < tb.Rows.Count; j++)
        //            {
        //                int q = Convert.ToInt32(tb.Rows[j][0].ToString());
        //                for (int k = j + 1; k < tb.Rows.Count; k++)
        //                {
        //                    int yy = Convert.ToInt32(tb.Rows[k][0].ToString());
        //                    if (q == yy)
        //                    {
        //                        string pp = Convert.ToString(tb.Rows[j][1].ToString());
        //                        ErrorType = "Invalid PO " + pp + " Value";
        //                        ExcelRowNumber = RowNumber.ToString();
        //                        ProductSKU = Sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                        er++;
        //                    }
        //                }


        //            }


        //            if (er > 0)
        //            {
        //                //ExcelRowNumber = RowNumber.ToString();
        //                //ProductSKU = Sku;
        //                //InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            return true;
        //        }


        //        return true;
        //    }
        //    catch (Exception ex)
        //    {
        //        return false;
        //    }



        //}


        //private bool CheckDeliveryDateinsequence(string sku, string po1, string po2, string po3, string po4, string qty1, string qty2, string qty3, string qty4, string shipping1, string shipping2, string shipping3, string shipping4, Int32 RowNumber)
        //{
        //    DateTime today = DateTime.Now;

        //    Int32 checkpocounter = 0;
        //    Int32 ErrorPOCounter = 0;
        //    Int32 Error = 0;


        //    if (!String.IsNullOrEmpty(sku))
        //    {

        //        if (String.IsNullOrEmpty(po1) && String.IsNullOrEmpty(qty1) && String.IsNullOrEmpty(shipping1))
        //        {
        //            ErrorPOCounter++;

        //        }
        //        else
        //        {
        //            if (ErrorPOCounter == 0)
        //                checkpocounter = 1;
        //        }

        //        if (String.IsNullOrEmpty(po2) && String.IsNullOrEmpty(qty2) && String.IsNullOrEmpty(shipping2) && ErrorPOCounter == 0)
        //        {
        //            ErrorPOCounter++;

        //        }
        //        else
        //        {
        //            if (ErrorPOCounter == 0)
        //                checkpocounter = 2;
        //        }

        //        if (String.IsNullOrEmpty(po3) && String.IsNullOrEmpty(qty3) && String.IsNullOrEmpty(shipping3) && ErrorPOCounter == 0)
        //        {
        //            ErrorPOCounter++;

        //        }
        //        else
        //        {
        //            if (ErrorPOCounter == 0)
        //                checkpocounter = 3;
        //        }

        //        if (String.IsNullOrEmpty(po4) && String.IsNullOrEmpty(qty4) && String.IsNullOrEmpty(shipping4) && ErrorPOCounter == 0)
        //        {
        //            ErrorPOCounter++;

        //        }
        //        else
        //        {
        //            if (ErrorPOCounter == 0)
        //                checkpocounter = 4;

        //        }

        //        //////////check Order Quantity validation

        //        for (int i = 1; i <= checkpocounter; i++)
        //        {

        //            if (i == 1)
        //            {
        //                if (!String.IsNullOrEmpty(qty1))
        //                {
        //                    Int32 q1 = 0;
        //                    Int32.TryParse(qty1, out q1);
        //                    if (q1 > 0)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Invalid Quantity 1 Value";
        //                        String ExcelRowNumber = Convert.ToString(RowNumber);
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                        Error++;
        //                    }
        //                }
        //                else
        //                {
        //                    String ErrorType = "Missing Quantity 1 Value";
        //                    String ExcelRowNumber = Convert.ToString(RowNumber);
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                    Error++;
        //                }

        //            }


        //            if (i == 2)
        //            {
        //                if (!String.IsNullOrEmpty(qty2))
        //                {
        //                    Int32 q2 = 0;
        //                    Int32.TryParse(qty2, out q2);
        //                    if (q2 > 0)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Invalid Quantity 2 Value";
        //                        String ExcelRowNumber = Convert.ToString(RowNumber);
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                        Error++;
        //                    }
        //                }
        //                else
        //                {
        //                    String ErrorType = "Missing Quantity 2 Value";
        //                    String ExcelRowNumber = Convert.ToString(RowNumber);
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                    Error++;
        //                }

        //            }


        //            if (i == 3)
        //            {
        //                if (!String.IsNullOrEmpty(qty3))
        //                {
        //                    Int32 q3 = 0;
        //                    Int32.TryParse(qty3, out q3);
        //                    if (q3 > 0)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Invalid Quantity 3 Value";
        //                        String ExcelRowNumber = Convert.ToString(RowNumber);
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                        Error++;
        //                    }
        //                }
        //                else
        //                {
        //                    String ErrorType = "Missing Quantity 3 Value";
        //                    String ExcelRowNumber = Convert.ToString(RowNumber);
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                    Error++;
        //                }

        //            }



        //            if (i == 4)
        //            {
        //                if (!String.IsNullOrEmpty(qty4))
        //                {
        //                    Int32 q4 = 0;
        //                    Int32.TryParse(qty4, out q4);
        //                    if (q4 > 0)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Invalid Quantity 4 Value";
        //                        String ExcelRowNumber = Convert.ToString(RowNumber);
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                        Error++;
        //                    }
        //                }
        //                else
        //                {
        //                    String ErrorType = "Missing Quantity 4 Value";
        //                    String ExcelRowNumber = Convert.ToString(RowNumber);
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //                    Error++;
        //                }

        //            }




        //        }

        //        ////////////////// check PONumber validation

        //        Int32 PO1 = 0;
        //        Int32 PO2 = 0;
        //        Int32 PO3 = 0;
        //        Int32 PO4 = 0;
        //        Int32.TryParse(po1, out PO1);
        //        Int32.TryParse(po2, out PO2);
        //        Int32.TryParse(po3, out PO3);
        //        Int32.TryParse(po4, out PO4);


        //        if (checkpocounter >= 1)
        //        {
        //            if (!String.IsNullOrEmpty(po1))
        //            {



        //                if (PO1 > 0)
        //                {

        //                }
        //                else
        //                {
        //                    String ErrorType = "Invalid Value for 'PO1'";
        //                    String ExcelRowNumber = RowNumber.ToString();
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                    Error++;

        //                }
        //            }
        //            else
        //            {
        //                String ErrorType = "Missing PO 1 Value";
        //                String ExcelRowNumber = RowNumber.ToString();
        //                String ProductSKU = sku;
        //                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                Error++;

        //            }

        //        }
        //        if (checkpocounter >= 2)
        //        {
        //            if (!String.IsNullOrEmpty(po2))
        //            {
        //                if (PO2 > 0)
        //                {

        //                }
        //                else
        //                {
        //                    String ErrorType = "Invalid Value for 'PO2'";
        //                    String ExcelRowNumber = RowNumber.ToString();
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                    Error++;

        //                }
        //            }
        //            else
        //            {
        //                String ErrorType = "Missing PO 2 Value";
        //                String ExcelRowNumber = RowNumber.ToString();
        //                String ProductSKU = sku;
        //                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                Error++;

        //            }

        //        }
        //        if (checkpocounter >= 3)
        //        {
        //            if (!String.IsNullOrEmpty(po3))
        //            {
        //                if (PO3 > 0)
        //                {

        //                }
        //                else
        //                {
        //                    String ErrorType = "Invalid Value for 'PO3'";
        //                    String ExcelRowNumber = RowNumber.ToString();
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                    Error++;

        //                }
        //            }
        //            else
        //            {
        //                String ErrorType = "Missing PO 3 Value";
        //                String ExcelRowNumber = RowNumber.ToString();
        //                String ProductSKU = sku;
        //                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                Error++;

        //            }

        //        }

        //        if (checkpocounter >= 4)
        //        {
        //            if (!String.IsNullOrEmpty(po3))
        //            {
        //                if (PO4 > 0)
        //                {

        //                }
        //                else
        //                {
        //                    String ErrorType = "Invalid Value for 'PO4'";
        //                    String ExcelRowNumber = RowNumber.ToString();
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                    Error++;

        //                }
        //            }
        //            else
        //            {
        //                String ErrorType = "Missing PO 4 Value";
        //                String ExcelRowNumber = RowNumber.ToString();
        //                String ProductSKU = sku;
        //                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                Error++;

        //            }

        //        }

        //        if (!CheckPOUnique(PO1, PO2, PO3, PO4, sku, RowNumber, checkpocounter))
        //        {
        //            Error++;
        //        }



        //        //////////////check shipping date validation
        //        DateTime shippingdate1;
        //        DateTime shippingdate2;
        //        DateTime shippingdate3;
        //        DateTime shippingdate4;
        //        for (int i = 1; i <= checkpocounter; i++)
        //        {

        //            if (i == 1)
        //            {
        //                try
        //                {
        //                    if (!String.IsNullOrEmpty(shipping1))
        //                    {
        //                        shippingdate1 = Convert.ToDateTime(shipping1);
        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Missing Date 1 Value";
        //                        String ExcelRowNumber = RowNumber.ToString();
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                        Error++;
        //                    }
        //                }
        //                catch
        //                {


        //                    String ErrorType = "Invalid Date 1 Value";
        //                    String ExcelRowNumber = RowNumber.ToString();
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                    Error++;
        //                }

        //            }


        //            if (i == 2)
        //            {
        //                try
        //                {
        //                    if (!String.IsNullOrEmpty(shipping2))
        //                    {
        //                        shippingdate2 = Convert.ToDateTime(shipping2);
        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Missing Date 2 Value";
        //                        String ExcelRowNumber = RowNumber.ToString();
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                        Error++;
        //                    }
        //                }
        //                catch
        //                {


        //                    String ErrorType = "Invalid Date 3 Value";
        //                    String ExcelRowNumber = RowNumber.ToString();
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                    Error++;
        //                }

        //            }


        //            if (i == 3)
        //            {
        //                try
        //                {
        //                    if (!String.IsNullOrEmpty(shipping3))
        //                    {
        //                        shippingdate3 = Convert.ToDateTime(shipping3);
        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Missing Date 3 Value";
        //                        String ExcelRowNumber = RowNumber.ToString();
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                        Error++;
        //                    }
        //                }
        //                catch
        //                {


        //                    String ErrorType = "Invalid Date 3 Value";
        //                    String ExcelRowNumber = RowNumber.ToString();
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                    Error++;
        //                }

        //            }

        //            if (i == 4)
        //            {
        //                try
        //                {
        //                    if (!String.IsNullOrEmpty(shipping4))
        //                    {
        //                        shippingdate4 = Convert.ToDateTime(shipping4);
        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Missing Date 4 Value";
        //                        String ExcelRowNumber = RowNumber.ToString();
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                        Error++;
        //                    }
        //                }
        //                catch
        //                {


        //                    String ErrorType = "Invalid Date 4 Value";
        //                    String ExcelRowNumber = RowNumber.ToString();
        //                    String ProductSKU = sku;
        //                    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                    Error++;
        //                }

        //            }



        //        }




        //        ////////////////////validation for Date 1 < Date 2 < Date 3 < Date 4
        //        DateTime Etadate1;
        //        DateTime.TryParse(shipping1, out Etadate1);
        //        DateTime Etadate2;
        //        DateTime.TryParse(shipping2, out Etadate2);
        //        DateTime Etadate3;
        //        DateTime.TryParse(shipping3, out Etadate3);
        //        DateTime Etadate4;
        //        DateTime.TryParse(shipping4, out Etadate4);



        //        if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
        //        {
        //            // Etadate1 = DateTime.Now;
        //        }
        //        if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
        //        {
        //            // Etadate2 = DateTime.Now;
        //        }
        //        if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
        //        {
        //            //Etadate3 = DateTime.Now;
        //        }
        //        if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
        //        {
        //            // Etadate4 = DateTime.Now;
        //        }
        //        //if ((Etadate1.Date < Etadate2.Date) && (Etadate2.Date < Etadate3.Date) && (Etadate3.Date < Etadate4.Date))
        //        //{

        //        //}
        //        //else
        //        //{

        //        //}
        //        if (checkpocounter >= 1)
        //        {
        //            try
        //            {


        //                if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
        //                {
        //                    Error++;

        //                }
        //                else
        //                {
        //                    if (today.Date < Etadate1.Date)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Wrong Date 1 Value";
        //                        String ExcelRowNumber = RowNumber.ToString();
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                        Error++;
        //                    }
        //                }

        //            }
        //            catch
        //            {

        //            }

        //        }
        //        if (checkpocounter >= 2)
        //        {
        //            try
        //            {


        //                if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
        //                {
        //                    Error++;

        //                }
        //                else
        //                {
        //                    if (Etadate1.Date < Etadate2.Date)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Wrong Date 2 Value";
        //                        String ExcelRowNumber = RowNumber.ToString();
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                        Error++;
        //                    }
        //                }

        //            }
        //            catch
        //            {

        //            }
        //        }


        //        if (checkpocounter >= 3)
        //        {
        //            try
        //            {


        //                if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
        //                {
        //                    Error++;

        //                }
        //                else
        //                {
        //                    if (Etadate2.Date < Etadate3.Date)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Wrong Date 3 Value";
        //                        String ExcelRowNumber = RowNumber.ToString();
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                        Error++;
        //                    }
        //                }

        //            }
        //            catch
        //            {

        //            }
        //        }

        //        if (checkpocounter >= 4)
        //        {
        //            try
        //            {


        //                if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
        //                {
        //                    Error++;

        //                }
        //                else
        //                {
        //                    if (Etadate3.Date < Etadate4.Date)
        //                    {

        //                    }
        //                    else
        //                    {
        //                        String ErrorType = "Wrong Date 4 Value";
        //                        String ExcelRowNumber = RowNumber.ToString();
        //                        String ProductSKU = sku;
        //                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

        //                        Error++;
        //                    }
        //                }

        //            }
        //            catch
        //            {

        //            }
        //        }

        //    }
        //    else
        //    {
        //        String ErrorType = "Sku Should not be Blank";
        //        String ExcelRowNumber = Convert.ToString(RowNumber);
        //        String ProductSKU = sku;
        //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //        Error++;
        //    }

        //    if (checkpocounter == 0)
        //    {
        //        String ErrorType = "Remove sku or add PO Details in File";
        //        String ExcelRowNumber = Convert.ToString(RowNumber);
        //        String ProductSKU = sku;
        //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
        //        InsertErrorinTemp("No Replenishment Details Found", "0", "");
        //        Error++;
        //    }
        //    if (Error > 0)
        //    {
        //        return false;

        //    }
        //    else
        //    {
        //        return true;

        //    }



        //    //  return true;
        //}
        #region replenishment
        protected void btnImportReplenishment_Click(object sender, EventArgs e)
        {
            try
            {
                int pid = 0;
                if (Request.QueryString["ID"] != null && !string.IsNullOrEmpty(Request.QueryString["ID"].ToString()))
                {
                    pid = Convert.ToInt32(Request.QueryString["ID"].ToString());
                }

                Response.Redirect("/REPLENISHMENTMANAGEMENT/ImportReplenishmentData.aspx?ID=" + pid + "");
            }
            catch { }
        }


        private void BindRepenishableData()
        {

            int pid = 0;
            if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "")
            {
                pid = Convert.ToInt32(Request.QueryString["ID"].ToString());
            }

            string query = "SELECT '' as SKU,'PO#' as PO1,'Qty On Order' as qty1,'Shipping (Date)' as Shipping1,'ETA (Date)' as Etadate1,'PO#' as PO2,'Qty On Order' as qty2,'Shipping (Date)' as Shipping2,'ETA (Date)' as Etadate2,'PO#' as PO3,'Qty On Order' as qty3,'Shipping (Date)' as Shipping3,'ETA (Date)' as Etadate3,'PO#' as PO4,'Qty On Order' as qty4,'Shipping (Date)' as Shipping4,'ETA (Date)' as Etadate4,-1 as DisplayOrder,0 as VariantValueID,0 as IsDiscontinued,0 as IsRepenishable,0 as IsBackorderable,'' as LeadTime Union all ";

            string query1 = " SELECT isnull(tb_Product.SKU,'') as SKU,cast(isnull(PO1,0) as varchar) as PO1,cast(isnull(qty1,0) as varchar) as qty1,cast(isnull(Shipping1,'')as varchar) as Shipping1,cast(isnull(Etadate1,'')as varchar) as Etadate1,cast(isnull(PO2,0) as varchar) as PO2,cast(isnull(qty2,0)as varchar) as qty2,cast(isnull(Shipping2,'')as varchar) as Shipping2,cast(isnull(Etadate2,'')as varchar) as Etadate2,cast(isnull(PO3,0)as varchar) as PO3,cast(isnull(qty3,0)as varchar) as qty3,cast(isnull(Shipping3,'')as varchar) as Shipping3,cast(isnull(Etadate3,'')as varchar) as Etadate3,cast(isnull(PO4,0)as varchar) as PO4,cast(isnull(qty4,0)as varchar) as qty4,cast(isnull(Shipping4,'')as varchar) as Shipping4,cast(isnull(Etadate4,'')as varchar) as Etadate4,isnull(DisplayOrder,0) as DisplayOrder,0 as VariantValueID,0 as IsDiscontinued,0 as IsRepenishable,0 as IsBackorderable,isnull(LeadTime,'') as LeadTime  from   tb_Product  left outer join  tb_Replenishment on tb_Product.ProductID = tb_Replenishment.ProductID where tb_Replenishment.productid=" + pid + " and  isnull(tb_Replenishment.VariantValueID,0)=0  union all ";

            string query2 = "SELECT isnull(tb_ProductVariantValue.SKU,'') as SKU,cast(isnull(PO1,0) as varchar) as PO1,cast(isnull(qty1,0) as varchar) as qty1,cast(isnull(Shipping1,'')as varchar) as Shipping1,cast(isnull(Etadate1,'')as varchar) as Etadate1,cast(isnull(PO2,0) as varchar) as PO2,cast(isnull(qty2,0)as varchar) as qty2,cast(isnull(Shipping2,'')as varchar) as Shipping2,cast(isnull(Etadate2,'')as varchar) as Etadate2,cast(isnull(PO3,0)as varchar) as PO3,cast(isnull(qty3,0)as varchar) as qty3,cast(isnull(Shipping3,'')as varchar) as Shipping3,cast(isnull(Etadate3,'')as varchar) as Etadate3,cast(isnull(PO4,0)as varchar) as PO4,cast(isnull(qty4,0)as varchar) as qty4,cast(isnull(Shipping4,'')as varchar) as Shipping4,cast(isnull(Etadate4,'')as varchar) as Etadate4,isnull(DisplayOrder,0) as DisplayOrder,tb_ProductVariantValue.VariantValueID,0 as IsDiscontinued,0 as IsRepenishable,0 as IsBackorderable,isnull(LeadTime,'') as LeadTime  from   tb_ProductVariantValue  left outer join  tb_Replenishment on tb_ProductVariantValue.VariantValueID = tb_Replenishment.VariantValueID where tb_ProductVariantValue.productid=" + pid + "  and  isnull(tb_ProductVariantValue.sku,'')<>'' and  tb_Replenishment.sku not like '%-cus%'  ";
            query = query + query1 + query2;
            DataSet dsData = new DataSet();
            dsData = CommonComponent.GetCommonDataSet(query);
            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                grdREPLENISHMENT1.DataSource = dsData.Tables[0];
                grdREPLENISHMENT1.DataBind();
                if (dsData.Tables[0].Rows.Count > 1)
                {


                    btnRepleshmentsave1.Visible = true;
                }
                else
                {
                    btnRepleshmentsave1.Visible = false;
                }



            }
            else
            {
                grdREPLENISHMENT1.DataSource = null;
                grdREPLENISHMENT1.DataBind();
                btnRepleshmentsave1.Visible = false;
            }

        }


        protected void grdREPLENISHMENT1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                Funname = "";
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                TextBox txtPO1 = (TextBox)e.Row.FindControl("txtPO1");
                TextBox txtPO2 = (TextBox)e.Row.FindControl("txtPO2");
                TextBox txtPO3 = (TextBox)e.Row.FindControl("txtPO3");
                TextBox txtPO4 = (TextBox)e.Row.FindControl("txtPO4");

                TextBox txtqty1 = (TextBox)e.Row.FindControl("txtqty1");
                TextBox txtqty2 = (TextBox)e.Row.FindControl("txtqty2");
                TextBox txtqty3 = (TextBox)e.Row.FindControl("txtqty3");
                TextBox txtqty4 = (TextBox)e.Row.FindControl("txtqty4");


                TextBox txtshipping1 = (TextBox)e.Row.FindControl("txtshipping1");
                TextBox txtshipping2 = (TextBox)e.Row.FindControl("txtshipping2");
                TextBox txtshipping3 = (TextBox)e.Row.FindControl("txtshipping3");
                TextBox txtshipping4 = (TextBox)e.Row.FindControl("txtshipping4");



                TextBox txtEtadate1 = (TextBox)e.Row.FindControl("txtEtadate1");
                TextBox txtEtadate2 = (TextBox)e.Row.FindControl("txtEtadate2");
                TextBox txtEtadate3 = (TextBox)e.Row.FindControl("txtEtadate3");
                TextBox txtEtadate4 = (TextBox)e.Row.FindControl("txtEtadate4");




                Label lblPO1 = (Label)e.Row.FindControl("lblPO1");
                Label lblPO2 = (Label)e.Row.FindControl("lblPO2");
                Label lblPO3 = (Label)e.Row.FindControl("lblPO3");
                Label lblPO4 = (Label)e.Row.FindControl("lblPO4");

                Label lblqty1 = (Label)e.Row.FindControl("lblqty1");
                Label lblqty2 = (Label)e.Row.FindControl("lblqty2");
                Label lblqty3 = (Label)e.Row.FindControl("lblqty3");
                Label lblqty4 = (Label)e.Row.FindControl("lblqty4");

                Label lblshipping1 = (Label)e.Row.FindControl("lblshipping1");
                Label lblshipping2 = (Label)e.Row.FindControl("lblshipping2");
                Label lblshipping3 = (Label)e.Row.FindControl("lblshipping3");
                Label lblshipping4 = (Label)e.Row.FindControl("lblshipping4");

                Label lblEtadate1 = (Label)e.Row.FindControl("lblEtadate1");
                Label lblEtadate2 = (Label)e.Row.FindControl("lblEtadate2");
                Label lblEtadate3 = (Label)e.Row.FindControl("lblEtadate3");
                Label lblEtadate4 = (Label)e.Row.FindControl("lblEtadate4");

                Label lblvariantsku = (Label)e.Row.FindControl("lblvariantsku");
                TextBox txtvariantsku = (TextBox)e.Row.FindControl("txtvariantsku");



                if (txtPO1.Text.ToString().Trim() == "0")
                {
                    txtPO1.Text = "";

                }
                if (txtPO2.Text.ToString().Trim() == "0")
                {
                    txtPO2.Text = "";

                }
                if (txtPO3.Text.ToString().Trim() == "0")
                {
                    txtPO3.Text = "";

                }
                if (txtPO4.Text.ToString().Trim() == "0")
                {
                    txtPO4.Text = "";

                }

                if (txtqty1.Text.ToString().Trim() == "0")
                {
                    txtqty1.Text = "";

                }
                if (txtqty2.Text.ToString().Trim() == "0")
                {
                    txtqty2.Text = "";

                }
                if (txtqty3.Text.ToString().Trim() == "0")
                {
                    txtqty3.Text = "";

                }
                if (txtqty4.Text.ToString().Trim() == "0")
                {
                    txtqty4.Text = "";

                }



                if (String.IsNullOrEmpty(txtshipping1.Text) || txtshipping1.Text.ToString().ToLower().IndexOf("1900") > -1)
                {
                    txtshipping1.Text = "";
                }
                if (String.IsNullOrEmpty(txtshipping2.Text) || txtshipping2.Text.ToString().ToLower().IndexOf("1900") > -1)
                {
                    txtshipping2.Text = "";
                }

                if (String.IsNullOrEmpty(txtshipping3.Text) || txtshipping3.Text.ToString().ToLower().IndexOf("1900") > -1)
                {
                    txtshipping3.Text = "";
                }

                if (String.IsNullOrEmpty(txtshipping4.Text) || txtshipping4.Text.ToString().ToLower().IndexOf("1900") > -1)
                {
                    txtshipping4.Text = "";
                }


                if (String.IsNullOrEmpty(txtEtadate1.Text) || txtEtadate1.Text.ToString().ToLower().IndexOf("1900") > -1)
                {
                    txtEtadate1.Text = "";
                }
                if (String.IsNullOrEmpty(txtEtadate2.Text) || txtEtadate2.Text.ToString().ToLower().IndexOf("1900") > -1)
                {
                    txtEtadate2.Text = "";
                }

                if (String.IsNullOrEmpty(txtEtadate3.Text) || txtEtadate3.Text.ToString().ToLower().IndexOf("1900") > -1)
                {
                    txtEtadate3.Text = "";
                }

                if (String.IsNullOrEmpty(txtEtadate4.Text) || txtEtadate4.Text.ToString().ToLower().IndexOf("1900") > -1)
                {
                    txtEtadate4.Text = "";
                }

                DateTime Etadate1;
                DateTime.TryParse(txtEtadate1.Text.ToString(), out Etadate1);
                DateTime Etadate2;
                DateTime.TryParse(txtEtadate2.Text.ToString(), out Etadate2);
                DateTime Etadate3;
                DateTime.TryParse(txtEtadate3.Text.ToString(), out Etadate3);
                DateTime Etadate4;
                DateTime.TryParse(txtEtadate4.Text.ToString(), out Etadate4);


                //if(!String.IsNullOrEmpty(txtEtadate1.Text))
                //{
                //    txtEtadate1.Text = Etadate1.AddDays(-ReplacementAddDays).ToString("MM/dd/yyyy");

                //}
                //if (!String.IsNullOrEmpty(txtEtadate2.Text))
                //{

                //    txtEtadate2.Text = Etadate2.AddDays(-ReplacementAddDays).ToString("MM/dd/yyyy");
                //}
                //if (!String.IsNullOrEmpty(txtEtadate3.Text))
                //{
                //    txtEtadate3.Text = Etadate3.AddDays(-ReplacementAddDays).ToString("MM/dd/yyyy");

                //}
                //if (!String.IsNullOrEmpty(txtEtadate4.Text))
                //{

                //    txtEtadate4.Text = Etadate4.AddDays(-ReplacementAddDays).ToString("MM/dd/yyyy");
                //}

                //    Funname += "<script type=\"text/javascript\">";

                Funname += "$j('#" + txtshipping1.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";

                Funname += " $j('#" + txtshipping2.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                Funname += " $j('#" + txtshipping3.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                Funname += " $j('#" + txtshipping4.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";

                Funname += "$j('#" + txtEtadate1.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";

                Funname += " $j('#" + txtEtadate2.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                Funname += " $j('#" + txtEtadate3.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                Funname += " $j('#" + txtEtadate4.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                // Funname += "</script>";

                if (e.Row.RowIndex == 0)
                {
                    txtPO1.Visible = false;
                    txtPO2.Visible = false;
                    txtPO3.Visible = false;
                    txtPO4.Visible = false;

                    txtqty1.Visible = false;
                    txtqty2.Visible = false;
                    txtqty3.Visible = false;
                    txtqty4.Visible = false;


                    txtshipping1.Visible = false;
                    txtshipping2.Visible = false;
                    txtshipping3.Visible = false;
                    txtshipping4.Visible = false;

                    txtEtadate1.Visible = false;
                    txtEtadate2.Visible = false;
                    txtEtadate3.Visible = false;
                    txtEtadate4.Visible = false;

                    txtvariantsku.Visible = false;
                }
                else
                {
                    lblPO1.Visible = false;
                    lblPO2.Visible = false;
                    lblPO3.Visible = false;
                    lblPO4.Visible = false;

                    lblqty1.Visible = false;
                    lblqty2.Visible = false;
                    lblqty3.Visible = false;
                    lblqty4.Visible = false;

                    lblEtadate1.Visible = false;
                    lblEtadate2.Visible = false;
                    lblEtadate3.Visible = false;
                    lblEtadate4.Visible = false;

                    lblshipping1.Visible = false;
                    lblshipping2.Visible = false;
                    lblshipping3.Visible = false;
                    lblshipping4.Visible = false;

                    txtvariantsku.Visible = false;
                }




            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                // Funname = " var $j = jQuery.noConflict(); $j(function () {" + Funname.ToString() + "});";
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Funname", Funname, true);
            }
        }

        protected void btnRepleshmentsave1_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (grdREPLENISHMENT1.Rows.Count > 0)
                {

                    DataTable Dt = new DataTable();
                    Dt.Columns.Add("Number", typeof(int));
                    Dt.Columns.Add("sku", typeof(string));
                    Dt.Columns.Add("name", typeof(string));
                    Dt.Columns.Add("PO#1", typeof(string));
                    Dt.Columns.Add("PO#2", typeof(string));
                    Dt.Columns.Add("PO#3", typeof(string));
                    Dt.Columns.Add("PO#4", typeof(string));
                    Dt.Columns.Add("Quantity Ordered1", typeof(string));
                    Dt.Columns.Add("Quantity Ordered2", typeof(string));
                    Dt.Columns.Add("Quantity Ordered3", typeof(string));
                    Dt.Columns.Add("Quantity Ordered4", typeof(string));
                    Dt.Columns.Add("Shipping Date1", typeof(string));
                    Dt.Columns.Add("Shipping Date2", typeof(string));
                    Dt.Columns.Add("Shipping Date3", typeof(string));
                    Dt.Columns.Add("Shipping Date4", typeof(string));
                    Dt.Columns.Add("ETA ATL1", typeof(string));
                    Dt.Columns.Add("ETA ATL2", typeof(string));
                    Dt.Columns.Add("ETA ATL3", typeof(string));
                    Dt.Columns.Add("ETA ATL4", typeof(string));
                    Dt.AcceptChanges();

                    for (int i = 1; i < grdREPLENISHMENT1.Rows.Count; i++)
                    {
                        TextBox txtPO1 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtPO1");
                        TextBox txtPO2 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtPO2");
                        TextBox txtPO3 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtPO3");
                        TextBox txtPO4 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtPO4");

                        TextBox txtqty1 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtqty1");
                        TextBox txtqty2 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtqty2");
                        TextBox txtqty3 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtqty3");
                        TextBox txtqty4 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtqty4");


                        TextBox txtshipping1 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtshipping1");
                        TextBox txtshipping2 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtshipping2");
                        TextBox txtshipping3 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtshipping3");
                        TextBox txtshipping4 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtshipping4");

                        TextBox txtEtadate1 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtEtadate1");
                        TextBox txtEtadate2 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtEtadate2");
                        TextBox txtEtadate3 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtEtadate3");
                        TextBox txtEtadate4 = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtEtadate4");

                        Label lblPO1 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblPO1");
                        Label lblPO2 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblPO2");
                        Label lblPO3 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblPO3");
                        Label lblPO4 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblPO4");

                        Label lblqty1 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblqty1");
                        Label lblqty2 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblqty2");
                        Label lblqty3 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblqty3");
                        Label lblqty4 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblqty4");

                        Label lblEtadate1 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblEtadate1");
                        Label lblEtadate2 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblEtadate2");
                        Label lblEtadate3 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblEtadate3");
                        Label lblEtadate4 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblEtadate4");


                        Label lblshipping1 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblshipping1");
                        Label lblshipping2 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblshipping2");
                        Label lblshipping3 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblshipping3");
                        Label lblshipping4 = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblshipping4");

                        Label lblvariantsku = (Label)grdREPLENISHMENT1.Rows[i].FindControl("lblvariantsku");
                        TextBox txtvariantsku = (TextBox)grdREPLENISHMENT1.Rows[i].FindControl("txtvariantsku");
                        DataRow dr = Dt.NewRow();
                        dr["Number"] = i - 1;
                        dr["sku"] = lblvariantsku.Text.ToString().Trim();
                        dr["name"] = txtProductName.Text.ToString().Replace("'", "''");
                        dr["PO#1"] = txtPO1.Text.ToString();
                        dr["PO#2"] = txtPO2.Text.ToString();
                        dr["PO#3"] = txtPO3.Text.ToString();
                        dr["PO#4"] = txtPO4.Text.ToString();
                        dr["Quantity Ordered1"] = txtqty1.Text.ToString();
                        dr["Quantity Ordered2"] = txtqty2.Text.ToString();
                        dr["Quantity Ordered3"] = txtqty3.Text.ToString();
                        dr["Quantity Ordered4"] = txtqty4.Text.ToString();
                        dr["Shipping Date1"] = txtshipping1.Text.ToString();
                        dr["Shipping Date2"] = txtshipping2.Text.ToString();
                        dr["Shipping Date3"] = txtshipping3.Text.ToString();
                        dr["Shipping Date4"] = txtshipping4.Text.ToString();
                        dr["ETA ATL1"] = txtEtadate1.Text.ToString();
                        dr["ETA ATL2"] = txtEtadate2.Text.ToString();
                        dr["ETA ATL3"] = txtEtadate3.Text.ToString();
                        dr["ETA ATL4"] = txtEtadate4.Text.ToString();
                        Dt.Rows.Add(dr);
                        Dt.AcceptChanges();

                    }


                    if (Dt.Rows.Count > 0)
                    {
                        validatefiledata(Dt);
                    }


                }
            }
            catch { }

        }

        private void AddTemptable(DataTable dt)
        {
            TempCSV.Clear();
            TempCSV = dt.Copy();
            if (TempCSV != null && TempCSV.Rows.Count > 0)
            {

                DataColumn col1 = new DataColumn("Flag", typeof(string));
                TempCSV.Columns.Add(col1);
            }

        }

        private void validatefiledata(DataTable Dt)
        {
            Int32 po1 = 0;
            Int32 po2 = 0;
            Int32 po3 = 0;
            Int32 po4 = 0;
            Int32 qty1 = 0;
            Int32 qty2 = 0;
            Int32 qty3 = 0;
            Int32 qty4 = 0;
            DateTime shipping1;
            DateTime shipping2;
            DateTime shipping3;
            DateTime shipping4;
            String sku = "";
            String Name = "";
            Int32 error = 0;
            Int32 error2 = 0;
            int flag = 0;

            AddTemptable(Dt);

            //lblskucount.Text = Dt.Rows.Count.ToString();
            //Totalsku = Dt.Rows.Count;
            lblerrorsku.Text = ",";
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                error = 0;
                error2 = 0;

                try
                {
                    if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
                    {
                        sku = Dt.Rows[i]["sku"].ToString();

                        //string checksku = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(sku,'') from tb_product where sku='" + sku + "' and storeid=1 and isnull(active,0)=1 and isnull(deleted,0)=0"));
                        //if(String.IsNullOrEmpty(checksku))
                        //{
                        //    string checksubsku = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(sku,'') from tb_ProductVariantValue where sku='" + sku + "' and productid in (select productid from tb_product where storeid=1 and isnull(active,0)=1 and isnull(deleted,0)=0)"));
                        //    if (String.IsNullOrEmpty(checksubsku))
                        //    {
                        //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //        error2++;
                        //    }

                        //}

                        if (!CheckSkuExist(sku, Dt))
                        {
                            if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                                lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            error2++;
                        }

                        if (!CheckDuplicateSku(sku, Dt))
                        {
                            if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                                lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            error2++;
                        }


                    }
                    else
                    {
                        String ErrorType = "Missing SKU";
                        String ExcelRowNumber = Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Number"].ToString()) + 1);
                        String ProductSKU = "";
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        InsertErrorinTemp("No Replenishment Details Found", "0", "");
                        UpdateFlag("0", ExcelRowNumber);
                        error++;
                    }
                }
                catch
                {
                    if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    error++;

                }

                if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
                {



                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["name"].ToString()))
                        {
                            Name = Dt.Rows[i]["name"].ToString();
                        }
                        else
                        {
                            //error++;
                        }
                    }
                    catch
                    {
                        error++;
                    }


                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#1"].ToString()))
                        {
                            // po1 = Convert.ToInt32(Dt.Rows[i]["PO#1"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#2"].ToString()))
                        {
                            // po2 = Convert.ToInt32(Dt.Rows[i]["PO#2"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#3"].ToString()))
                        {
                            // po3 = Convert.ToInt32(Dt.Rows[i]["PO#3"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#4"].ToString()))
                        {
                            //po4 = Convert.ToInt32(Dt.Rows[i]["PO#4"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }



                    //if (!CheckPOUnique(po1, po2, po3, po4, sku, Convert.ToInt32(Dt.Rows[i]["Number"].ToString())))
                    //{
                    //    error++;
                    //}


                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered1"].ToString()))
                        {
                            qty1 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered1"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered2"].ToString()))
                        {
                            qty2 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered2"].ToString());
                        }
                        else
                        {
                            //    if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //        lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //    error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered3"].ToString()))
                        {
                            qty3 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered3"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered4"].ToString()))
                        {
                            qty4 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered4"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }


                    if (!CheckDeliveryDateinsequence(sku, Convert.ToString(Dt.Rows[i]["PO#1"]), Convert.ToString(Dt.Rows[i]["PO#2"]), Convert.ToString(Dt.Rows[i]["PO#3"]), Convert.ToString(Dt.Rows[i]["PO#4"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered1"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered2"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered3"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered4"]), Convert.ToString(Dt.Rows[i]["Shipping Date1"]), Convert.ToString(Dt.Rows[i]["Shipping Date2"]), Convert.ToString(Dt.Rows[i]["Shipping Date3"]), Convert.ToString(Dt.Rows[i]["Shipping Date4"]), Convert.ToInt32(Dt.Rows[i]["Number"].ToString()) + 1, Convert.ToString(Dt.Rows[i]["ETA ATL1"]), Convert.ToString(Dt.Rows[i]["ETA ATL2"]), Convert.ToString(Dt.Rows[i]["ETA ATL3"]), Convert.ToString(Dt.Rows[i]["ETA ATL4"])))
                    {
                        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        error++;
                    }


                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(Dt.Rows[i]["Shipping Date1"].ToString()))
                    //        {
                    //            shipping1 = Convert.ToDateTime(Dt.Rows[i]["Shipping Date1"].ToString());
                    //        }
                    //        else
                    //        {
                    //            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //            //error++;
                    //        }
                    //    }
                    //    catch
                    //    {


                    //        String ErrorType = "Invalid Date Format for 'Date 1'";
                    //        String ExcelRowNumber = Dt.Rows[i]["Number"].ToString();
                    //        String ProductSKU = sku;
                    //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                    //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //        error++;
                    //    }


                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(Dt.Rows[i]["Shipping Date2"].ToString()))
                    //        {
                    //            shipping2 = Convert.ToDateTime(Dt.Rows[i]["Shipping Date2"].ToString());
                    //        }
                    //        else
                    //        {
                    //            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //            //error++;
                    //        }
                    //    }
                    //    catch
                    //    {
                    //        String ErrorType = "Invalid Date Format for 'Date 2'";
                    //        String ExcelRowNumber = Dt.Rows[i]["Number"].ToString();
                    //        String ProductSKU = sku;
                    //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                    //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //        error++;



                    //    }

                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(Dt.Rows[i]["Shipping Date3"].ToString()))
                    //        {
                    //            shipping3 = Convert.ToDateTime(Dt.Rows[i]["Shipping Date3"].ToString());
                    //        }
                    //        else
                    //        {
                    //            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //            //error++;
                    //        }
                    //    }
                    //    catch
                    //    {
                    //        String ErrorType = "Invalid Date Format for 'Date 3'";
                    //        String ExcelRowNumber = Dt.Rows[i]["Number"].ToString();
                    //        String ProductSKU = sku;
                    //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                    //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //        error++;
                    //    }


                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(Dt.Rows[i]["Shipping Date4"].ToString()))
                    //        {
                    //            shipping4 = Convert.ToDateTime(Dt.Rows[i]["Shipping Date4"].ToString());
                    //        }
                    //        else
                    //        {
                    //            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //            //error++;
                    //        }
                    //    }
                    //    catch
                    //    {
                    //        String ErrorType = "Invalid Date Format for 'Date 4'";
                    //        String ExcelRowNumber = Dt.Rows[i]["Number"].ToString();
                    //        String ProductSKU = sku;
                    //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                    //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //        error++;
                    //    }

                }

                // Int32 c = dterror.Rows.Count;

                if (error == 0 && error2 == 0)
                {
                    replnishedsku = replnishedsku + 1;
                    String ExcelRowNumber = Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Number"].ToString()) + 1);
                    UpdateFlag("4", ExcelRowNumber);
                }
                else
                {
                    //if (lblMsg.Text.ToString().IndexOf("Data does not match field data format") <= -1)
                    //    lblMsg.Text += "Data does not match field data format";
                }

            }


            String strErrors = "";
            if (dterror.Rows.Count > 0)
            {
                strErrors += "<table class=\"table table-bordered table-striped table-condensed cf\">";
                strErrors += "<thead><tr class=\"cf\">";
                strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">#</th>";
                strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">SKU</th>";
                strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\"> Row #s</th>";
                strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">Error Description</th>";
                strErrors += "</tr></thead>";
                for (int k = 0; k < dterror.Rows.Count; k++)
                {

                    if (dterror.Rows[k][1].ToString().Replace("'", "''") == "0")
                    {
                        strErrors += "<tbody>";
                        strErrors += "<tr>";
                        strErrors += "<td align=\"center\" colspan=\"4\" style=\"color:red;\">";
                        strErrors += "Warning Message: " + dterror.Rows[k][0].ToString().Replace("'", "''");
                        strErrors += "</td>";
                        strErrors += "</tr>";
                        strErrors += "</tbody>";
                    }
                    else
                    {
                        strErrors += "<tbody>";
                        strErrors += "<tr>";
                        strErrors += "<td align=\"left\" style=\"color:blue;\">";
                        strErrors += k + 1;
                        strErrors += "</td>";
                        strErrors += "<td align=\"left\" style=\"color:blue;\">";
                        strErrors += dterror.Rows[k][2].ToString().Replace("'", "''");
                        strErrors += "</td>";
                        strErrors += "<td align=\"left\" style=\"color:blue;\">";
                        strErrors += dterror.Rows[k][1].ToString().Replace("'", "''");
                        strErrors += "</td>";
                        strErrors += "<td align=\"left\" style=\"color:blue;\">";
                        strErrors += dterror.Rows[k][0].ToString().Replace("'", "''");
                        strErrors += "</td>";
                        strErrors += "</tr>";
                        strErrors += "</tbody>";
                    }


                }
                strErrors += "</table>";
            }
            ltrErrors.Text = "";
            ltrErrors.Text = strErrors;
            ViewState["temptable"] = TempCSV;

            if (TempCSV != null && TempCSV.Rows.Count > 0)
            {
                DataRow[] ddd = TempCSV.Select("isnull(Flag,'') <>'0'");
                if (ddd.Length > 0)
                {
                    lblreplenishedskucount.Text = ddd.Length.ToString();
                    SaveReplenishmentData(Dt);
                }
            }


            if (String.IsNullOrEmpty(strErrors.ToString()))
            {
                // diverror.Visible = false;



                //  SaveReplenishmentData(Dt);


                //lblMsg.Text = "Successfull";
            }


            try
            {
                lblerrorsku.Text = lblerrorsku.Text.ToString().Remove(lblerrorsku.Text.ToString().LastIndexOf(","));
            }
            catch { }
            if (!String.IsNullOrEmpty(lblerrorsku.Text) && lblerrorsku.Text.ToString().Length > 2)
            {
                lblerrorsku.Visible = false;

            }
            else
            {
                lblerrorsku.Visible = false;

            }




            for (int y = 0; y < grdREPLENISHMENT1.Rows.Count; y++)
            {

                try
                {

                    TextBox txtEtadate1 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtEtadate1");
                    TextBox txtEtadate2 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtEtadate2");
                    TextBox txtEtadate3 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtEtadate3");
                    TextBox txtEtadate4 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtEtadate4");
                    TextBox txtshipping1 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtshipping1");
                    TextBox txtshipping2 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtshipping2");
                    TextBox txtshipping3 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtshipping3");
                    TextBox txtshipping4 = (TextBox)grdREPLENISHMENT1.Rows[y].FindControl("txtshipping4");

                    DateTime Etadate1;
                    DateTime.TryParse(txtEtadate1.Text.ToString(), out Etadate1);
                    DateTime Etadate2;
                    DateTime.TryParse(txtEtadate2.Text.ToString(), out Etadate2);
                    DateTime Etadate3;
                    DateTime.TryParse(txtEtadate3.Text.ToString(), out Etadate3);
                    DateTime Etadate4;
                    DateTime.TryParse(txtEtadate4.Text.ToString(), out Etadate4);




                    //    Funname += "<script type=\"text/javascript\">";

                    Funname += "$j('#" + txtshipping1.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";

                    Funname += " $j('#" + txtshipping2.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                    Funname += " $j('#" + txtshipping3.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                    Funname += " $j('#" + txtshipping4.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                    Funname += "$j('#" + txtEtadate1.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";

                    Funname += " $j('#" + txtEtadate2.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                    Funname += " $j('#" + txtEtadate3.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                    Funname += " $j('#" + txtEtadate4.ClientID.ToString() + "').datetimepicker({showButtonPanel: true, ampm: false,showHour: false, showMinute: false, showSecond: false, showTime: false, showOn: \"button\",buttonImage: \"/App_Themes/" + Page.Theme + "/images/date-icon.png\", buttonImageOnly: true}); ";
                }
                catch { }

            }






        }

        private void UpdateFlag(string flag, string ExcelRowNumber)
        {

            ExcelRowNumber = (Convert.ToInt32(ExcelRowNumber) - 1).ToString();
            if (TempCSV != null && TempCSV.Rows.Count > 0)
            {
                DataRow[] drtemp = TempCSV.Select("Number='" + ExcelRowNumber + "'");

                if (drtemp.Length > 0)
                {
                    if (!String.IsNullOrEmpty(drtemp[0]["Flag"].ToString()))
                    {
                        int f = 0;
                        Int32.TryParse(drtemp[0]["Flag"].ToString(), out f);
                        int fout = 0;
                        Int32.TryParse(flag.ToString(), out fout);
                        if (fout < f)
                        {
                            drtemp[0]["Flag"] = flag.ToString().Trim();
                            TempCSV.AcceptChanges();
                        }

                    }
                    else
                    {
                        drtemp[0]["Flag"] = flag.ToString().Trim();
                        TempCSV.AcceptChanges();
                    }




                }
            }
        }

        private void SaveReplenishmentData(DataTable Dtt)
        {
            int counter = 0;
            DataTable Dt = new DataTable();


            if (ViewState["temptable"] != null)
            {
                Dt = (DataTable)ViewState["temptable"];
            }
            else
            {
                Dt = Dtt.Copy();
            }
            if (Dt != null && Dt.Rows.Count > 0)
            {
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()) && Dt.Rows[i]["flag"].ToString() != "0")
                    {
                        int checkflag = 0;
                        Int32.TryParse(Dt.Rows[i]["flag"].ToString(), out checkflag);
                        string po1 = "";
                        string po2 = "";
                        string po3 = "";
                        string po4 = "";
                        Int32 qty1 = 0;
                        Int32 qty2 = 0;
                        Int32 qty3 = 0;
                        Int32 qty4 = 0;
                        TextBox txtShippingdate1 = new TextBox();
                        TextBox txtShippingdate2 = new TextBox();
                        TextBox txtShippingdate3 = new TextBox();
                        TextBox txtShippingdate4 = new TextBox();
                        TextBox txtEtadate1 = new TextBox();
                        TextBox txtEtadate2 = new TextBox();
                        TextBox txtEtadate3 = new TextBox();
                        TextBox txtEtadate4 = new TextBox();

                        String sku = "";
                        String Name = "";
                        String PID = "";
                        String VariantValueID = "";



                        sku = Dt.Rows[i]["sku"].ToString();
                        //if (lblerrorsku.Text.ToString().IndexOf("," + sku + ",") <= -1)
                        //{


                        try
                        {
                            if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
                            {
                                sku = Dt.Rows[i]["sku"].ToString();
                            }

                        }
                        catch
                        {

                        }

                        try
                        {
                            if (!String.IsNullOrEmpty(Dt.Rows[i]["name"].ToString()))
                            {
                                Name = Dt.Rows[i]["name"].ToString();
                            }
                            else
                            {

                            }
                        }
                        catch
                        {

                        }



                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#1"].ToString()))
                        //    {
                        //        po1 = Convert.ToString(Dt.Rows[i]["PO#1"].ToString());
                        //    }
                        //    else
                        //    {
                        //    }
                        //}
                        //catch
                        //{

                        //}

                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#2"].ToString()))
                        //    {
                        //        po2 = Convert.ToString(Dt.Rows[i]["PO#2"].ToString());
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        //catch
                        //{

                        //}
                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#3"].ToString()))
                        //    {
                        //        po3 = Convert.ToString(Dt.Rows[i]["PO#3"].ToString());
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        //catch
                        //{

                        //}

                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#4"].ToString()))
                        //    {
                        //        po4 = Convert.ToString(Dt.Rows[i]["PO#4"].ToString());
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        //catch
                        //{

                        //}



                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered1"].ToString()))
                        //    {
                        //        qty1 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered1"].ToString());
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        //catch
                        //{

                        //}

                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered2"].ToString()))
                        //    {
                        //        qty2 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered2"].ToString());
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        //catch
                        //{

                        //}
                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered3"].ToString()))
                        //    {
                        //        qty3 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered3"].ToString());
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        //catch
                        //{

                        //}

                        //try
                        //{
                        //    if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered4"].ToString()))
                        //    {
                        //        qty4 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered4"].ToString());
                        //    }
                        //    else
                        //    {

                        //    }
                        //}
                        //catch
                        //{

                        //}

                        //TextBox txtshipping1 = new TextBox();
                        //TextBox txtshipping2 = new TextBox();
                        //TextBox txtshipping3 = new TextBox();
                        //TextBox txtshipping4 = new TextBox();
                        //try
                        //{
                        //    DateTime Shipping1;
                        //    DateTime.TryParse(Dt.Rows[i]["Shipping Date1"].ToString(), out Shipping1);
                        //    DateTime Shipping2;
                        //    DateTime.TryParse(Dt.Rows[i]["Shipping Date2"].ToString(), out Shipping2);
                        //    DateTime Shipping3;
                        //    DateTime.TryParse(Dt.Rows[i]["Shipping Date3"].ToString(), out Shipping3);
                        //    DateTime Shipping4;
                        //    DateTime.TryParse(Dt.Rows[i]["Shipping Date4"].ToString(), out Shipping4);



                        //    if (Shipping1.ToString() == "" || Shipping1.ToString("MM/dd/yyyy") == "01/01/0001")
                        //    {
                        //        // txtEtadate1.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                        //    }
                        //    else
                        //    {

                        //        txtshipping1.Text = Shipping1.ToString("MM/dd/yyyy");
                        //    }
                        //    if (Shipping2.ToString() == "" || Shipping2.ToString("MM/dd/yyyy") == "01/01/0001")
                        //    {
                        //        //  txtEtadate2.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                        //    }
                        //    else
                        //    {
                        //        txtshipping2.Text = Shipping2.ToString("MM/dd/yyyy");
                        //    }
                        //    if (Shipping3.ToString() == "" || Shipping3.ToString("MM/dd/yyyy") == "01/01/0001")
                        //    {
                        //        // txtEtadate3.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                        //    }
                        //    else
                        //    {
                        //        txtshipping3.Text = Shipping3.ToString("MM/dd/yyyy");
                        //    }
                        //    if (Shipping4.ToString() == "" || Shipping4.ToString("MM/dd/yyyy") == "01/01/0001")
                        //    {
                        //        //  txtEtadate4.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                        //    }
                        //    else
                        //    {
                        //        txtshipping4.Text = Shipping4.ToString("MM/dd/yyyy");
                        //    }

                        //}
                        //catch { }



                        //TextBox txtEtadate1 = new TextBox();
                        //TextBox txtEtadate2 = new TextBox();
                        //TextBox txtEtadate3 = new TextBox();
                        //TextBox txtEtadate4 = new TextBox();
                        //try
                        //{
                        //    DateTime Etadate1;
                        //    DateTime.TryParse(Dt.Rows[i]["ETA ATL1"].ToString(), out Etadate1);
                        //    DateTime Etadate2;
                        //    DateTime.TryParse(Dt.Rows[i]["ETA ATL2"].ToString(), out Etadate2);
                        //    DateTime Etadate3;
                        //    DateTime.TryParse(Dt.Rows[i]["ETA ATL3"].ToString(), out Etadate3);
                        //    DateTime Etadate4;
                        //    DateTime.TryParse(Dt.Rows[i]["ETA ATL4"].ToString(), out Etadate4);



                        //    if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                        //    {
                        //        // txtEtadate1.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                        //    }
                        //    else
                        //    {

                        //       txtEtadate1.Text = Etadate1.ToString("MM/dd/yyyy");
                        //    }
                        //    if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                        //    {
                        //        //  txtEtadate2.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                        //    }
                        //    else
                        //    {
                        //        txtEtadate2.Text = Etadate2.ToString("MM/dd/yyyy");
                        //    }
                        //    if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                        //    {
                        //        // txtEtadate3.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                        //    }
                        //    else
                        //    {
                        //        txtEtadate3.Text = Etadate3.ToString("MM/dd/yyyy");
                        //    }
                        //    if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                        //    {
                        //        //  txtEtadate4.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                        //    }
                        //    else
                        //    {
                        //        txtEtadate4.Text = Etadate4.ToString("MM/dd/yyyy");
                        //    }

                        //}
                        //catch { }
                        if (checkflag >= 1)
                        {

                            try
                            {
                                if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#1"].ToString()))
                                {
                                    po1 = Convert.ToString(Dt.Rows[i]["PO#1"].ToString());
                                }
                                else
                                {
                                }
                            }
                            catch
                            {

                            }


                            try
                            {
                                if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered1"].ToString()))
                                {
                                    qty1 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered1"].ToString());
                                }
                                else
                                {

                                }
                            }
                            catch
                            {

                            }

                            try
                            {
                                DateTime shipppingdate1;
                                DateTime.TryParse(Dt.Rows[i]["Shipping Date1"].ToString(), out shipppingdate1);
                                if (shipppingdate1.ToString() == "" || shipppingdate1.ToString("MM/dd/yyyy") == "01/01/0001")
                                {
                                    // txtEtadate1.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                }
                                else
                                {

                                    //txtEtadate1.Text = Etadate1.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                    txtShippingdate1.Text = shipppingdate1.ToString("MM/dd/yyyy");
                                }
                            }
                            catch { }

                            try
                            {
                                DateTime Etadate1;
                                DateTime.TryParse(Dt.Rows[i]["ETA ATL1"].ToString(), out Etadate1);

                                if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                                {
                                    // txtEtadate1.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                }
                                else
                                {

                                    //txtEtadate1.Text = Etadate1.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                    txtEtadate1.Text = Etadate1.ToString("MM/dd/yyyy");
                                }
                            }
                            catch { }

                        }






                        if (checkflag >= 2)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#2"].ToString()))
                                {
                                    po2 = Convert.ToString(Dt.Rows[i]["PO#2"].ToString());
                                }
                                else
                                {

                                }
                            }
                            catch
                            {

                            }

                            try
                            {
                                if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered2"].ToString()))
                                {
                                    qty2 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered2"].ToString());
                                }
                                else
                                {

                                }
                            }
                            catch
                            {

                            }


                            try
                            {
                                DateTime shipppingdate2;
                                DateTime.TryParse(Dt.Rows[i]["Shipping Date2"].ToString(), out shipppingdate2);
                                if (shipppingdate2.ToString() == "" || shipppingdate2.ToString("MM/dd/yyyy") == "01/01/0001")
                                {
                                    //  txtEtadate2.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                }
                                else
                                {
                                    //txtEtadate2.Text = Etadate2.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                    txtShippingdate2.Text = shipppingdate2.ToString("MM/dd/yyyy");
                                }
                            }
                            catch { }

                            try
                            {
                                DateTime Etadate2;
                                DateTime.TryParse(Dt.Rows[i]["ETA ATL2"].ToString(), out Etadate2);
                                if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                                {
                                    //  txtEtadate2.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                }
                                else
                                {
                                    //txtEtadate2.Text = Etadate2.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                    txtEtadate2.Text = Etadate2.ToString("MM/dd/yyyy");
                                }
                            }
                            catch { }

                        }




                        if (checkflag >= 3)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#3"].ToString()))
                                {
                                    po3 = Convert.ToString(Dt.Rows[i]["PO#3"].ToString());
                                }
                                else
                                {

                                }
                            }
                            catch
                            {

                            }

                            try
                            {
                                if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered3"].ToString()))
                                {
                                    qty3 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered3"].ToString());
                                }
                                else
                                {

                                }
                            }
                            catch
                            {

                            }


                            try
                            {
                                DateTime shipppingdate3;
                                DateTime.TryParse(Dt.Rows[i]["Shipping Date3"].ToString(), out shipppingdate3);
                                if (shipppingdate3.ToString() == "" || shipppingdate3.ToString("MM/dd/yyyy") == "01/01/0001")
                                {
                                    // txtEtadate3.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                }
                                else
                                {
                                    //txtEtadate3.Text = Etadate3.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                    txtShippingdate3.Text = shipppingdate3.ToString("MM/dd/yyyy");
                                }
                            }
                            catch { }


                            try
                            {
                                DateTime Etadate3;
                                DateTime.TryParse(Dt.Rows[i]["ETA ATL3"].ToString(), out Etadate3);
                                if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                                {
                                    // txtEtadate3.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                }
                                else
                                {
                                    //txtEtadate3.Text = Etadate3.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                    txtEtadate3.Text = Etadate3.ToString("MM/dd/yyyy");
                                }
                            }
                            catch { }
                        }



                        if (checkflag >= 4)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#4"].ToString()))
                                {
                                    po4 = Convert.ToString(Dt.Rows[i]["PO#4"].ToString());
                                }
                                else
                                {

                                }
                            }
                            catch
                            {

                            }

                            try
                            {
                                if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered4"].ToString()))
                                {
                                    qty4 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered4"].ToString());
                                }
                                else
                                {

                                }
                            }
                            catch
                            {

                            }
                            try
                            {



                                DateTime shipppingdate4;
                                DateTime.TryParse(Dt.Rows[i]["Shipping Date4"].ToString(), out shipppingdate4);

                                if (shipppingdate4.ToString() == "" || shipppingdate4.ToString("MM/dd/yyyy") == "01/01/0001")
                                {
                                    //  txtEtadate4.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                }
                                else
                                {
                                    //txtEtadate4.Text = Etadate4.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                    txtShippingdate4.Text = shipppingdate4.ToString("MM/dd/yyyy");
                                }
                            }
                            catch
                            {

                            }

                            try
                            {



                                DateTime Etadate4;
                                DateTime.TryParse(Dt.Rows[i]["ETA ATL4"].ToString(), out Etadate4);

                                if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                                {
                                    //  txtEtadate4.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                }
                                else
                                {
                                    //txtEtadate4.Text = Etadate4.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                    txtEtadate4.Text = Etadate4.ToString("MM/dd/yyyy");
                                }

                            }
                            catch { }
                        }

                        try
                        {

                            PID = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 productid from tb_product where isnull(sku,'')='" + sku + "' and isnull(deleted,0)=0 and storeid=1 order by productid Desc"));
                        }
                        catch { }
                        try
                        {
                            if (String.IsNullOrEmpty(PID))
                            {
                                DataSet dsproduct = new DataSet();
                                dsproduct = (CommonComponent.GetCommonDataSet("select top 1 VariantValueID,productid from tb_ProductVariantValue where isnull(sku,'')='" + sku + "' and productid in (select productid from tb_product where isnull(deleted,0)=0 and storeid=1) order by VariantValueID Desc "));
                                if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                                {
                                    PID = dsproduct.Tables[0].Rows[0]["productid"].ToString();
                                    VariantValueID = dsproduct.Tables[0].Rows[0]["VariantValueID"].ToString();
                                }

                            }
                            else
                            {
                                VariantValueID = "0";
                            }
                        }
                        catch
                        {

                        }

                        try
                        {
                            if (!String.IsNullOrEmpty(PID) && !String.IsNullOrEmpty(VariantValueID))
                            {
                                Int32 count = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(productid) from tb_Replenishment where ProductID=" + PID + " and VariantValueID=" + VariantValueID + ""));
                                if (count > 0)
                                {
                                    //update

                                    CommonComponent.ExecuteCommonData("update [dbo].[tb_Replenishment] set [PO1]='" + po1 + "',[qty1]=" + qty1 + ",[Shipping1]='" + txtShippingdate1.Text + "',[Etadate1]='" + txtEtadate1.Text + "',[PO2]='" + po2 + "',[qty2]=" + qty2 + ",[Shipping2]='" + txtShippingdate2.Text + "',[Etadate2]='" + txtEtadate2.Text + "',[PO3]='" + po3 + "',[qty3]=" + qty3 + ",[Shipping3]='" + txtShippingdate3.Text + "',[Etadate3]='" + txtEtadate3.Text + "',[PO4]='" + po4 + "',[qty4]=" + qty4 + ",[Shipping4]='" + txtShippingdate4.Text + "',[Etadate4]='" + txtEtadate4.Text + "',sku='" + sku + "',name='" + Name + "',FileID=0,UpdatedBy=" + Session["AdminID"].ToString() + ",UpdatedOn=getdate() where VariantValueID=" + VariantValueID + " and Productid=" + PID + " ");

                                }
                                else
                                {
                                    //insert
                                    CommonComponent.ExecuteCommonData("insert into [dbo].[tb_Replenishment] ([ProductID],[VariantValueID],[PO1],[qty1],[Shipping1],[Etadate1],[PO2],[qty2],[Shipping2],[Etadate2],[PO3],[qty3],[Shipping3],[Etadate3],[PO4],[qty4],[Shipping4],[Etadate4],[CreatedOn],[sku],[Name],[CreatedBy],[FileID]) values (" + PID + "," + VariantValueID + ",'" + po1 + "'," + qty1 + ",'" + txtShippingdate1.Text + "','" + txtEtadate1.Text + "','" + po2 + "'," + qty2 + ",'" + txtShippingdate2.Text + "','" + txtEtadate2.Text + "','" + po3 + "'," + qty3 + ",'" + txtShippingdate3.Text + "','" + txtEtadate3.Text + "','" + po4 + "'," + qty4 + ",'" + txtShippingdate4.Text + "','" + txtEtadate4.Text + "',getdate(),'" + sku + "','" + Name + "'," + Session["AdminID"].ToString() + ",0)");

                                }

                                counter++;
                            }
                        }
                        catch { }



                        //  }
                    }
                }


                Page.ClientScript.RegisterStartupScript(Page.GetType(), "mess", "alert('Replenishment Data Uploaded Successfully')", true);

                BindRepenishableData();

            }
        }

        private void AddColumnsErrortable()
        {
            try
            {
                DataColumn col1 = new DataColumn("Error Type", typeof(string));
                dterror.Columns.Add(col1);
                DataColumn col2 = new DataColumn("Excel Row Number", typeof(string));
                dterror.Columns.Add(col2);
                DataColumn col3 = new DataColumn("Product SKU", typeof(string));
                dterror.Columns.Add(col3);

                dterror.AcceptChanges();
            }
            catch { }

        }

        private void InsertErrorinTemp(String ErrorType, String ExcelRowNumber, String ProductSKU)
        {
            try
            {
                DataRow dr = null;
                if (dterror.Columns.Count > 0)
                {
                    dr = dterror.NewRow();
                    dr["Error Type"] = ErrorType;
                    dr["Excel Row Number"] = ExcelRowNumber;
                    dr["Product SKU"] = ProductSKU;
                    dterror.Rows.Add(dr);
                    dterror.AcceptChanges();
                }
                else
                {
                    AddColumnsErrortable();
                    dr = dterror.NewRow();
                    dr["Error Type"] = ErrorType;
                    dr["Excel Row Number"] = ExcelRowNumber;
                    dr["Product SKU"] = ProductSKU;
                    dterror.Rows.Add(dr);
                    dterror.AcceptChanges();

                }
            }
            catch { }
        }

        private bool CheckDuplicateSku(String Sku, DataTable Dt)
        {

            String ErrorType = "Duplicate SKUs";
            String ExcelRowNumber = "";
            String ProductSKU = "";
            DataRow[] drerror = Dt.Select("sku ='" + Sku + "'");
            if (drerror.Length > 1)
            {
                for (int i = 0; i < drerror.Length; i++)
                {
                    ExcelRowNumber += Convert.ToInt32(drerror[i]["Number"].ToString()) + 1 + ",";
                    ProductSKU = drerror[i]["sku"].ToString();
                }


                if (ExcelRowNumber.ToString().Length > 2)
                {
                    ExcelRowNumber = ExcelRowNumber.ToString().Remove(ExcelRowNumber.ToString().LastIndexOf(","));
                }

                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                UpdateFlag("0", ExcelRowNumber);
                return false;

            }
            else
            {
                return true;
            }

        }
        private bool CheckSkuExist(String Sku, DataTable Dt)
        {
            String ErrorType = "Invalid SKU";
            String ExcelRowNumber = "";
            String ProductSKU = "";
            string checksku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_product where sku='" + Sku + "' and storeid=1 and  isnull(deleted,0)=0"));
            if (String.IsNullOrEmpty(checksku))
            {
                string checksubsku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_ProductVariantValue where sku='" + Sku + "' and productid in (select productid from tb_product where storeid=1  and isnull(deleted,0)=0)"));
                if (String.IsNullOrEmpty(checksubsku))
                {
                    DataRow[] drerror = Dt.Select("sku ='" + Sku + "'");
                    if (drerror.Length > 0)
                    {
                        for (int i = 0; i < drerror.Length; i++)
                        {
                            ExcelRowNumber += Convert.ToInt32(drerror[i]["Number"].ToString()) + 1 + ",";
                            ProductSKU = drerror[i]["sku"].ToString();
                        }


                        if (ExcelRowNumber.ToString().Length > 0)
                        {
                            ExcelRowNumber = ExcelRowNumber.ToString().Remove(ExcelRowNumber.ToString().LastIndexOf(","));
                        }
                        UpdateFlag("0", ExcelRowNumber);
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        return false;

                    }
                    else
                    {
                        return false;
                    }


                }
                else
                {
                    checksubsku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_ProductVariantValue where sku='" + Sku + "' and isnull(varactive,0)=1 and productid in (select productid from tb_product where storeid=1 and isnull(active,0)=1 and isnull(deleted,0)=0)"));
                    if (String.IsNullOrEmpty(checksubsku))
                    {
                        InsertErrorinTemp("Product is Inactive  SKU : " + Sku, "0", "");
                    }
                    return true;
                }

            }
            else
            {


                checksku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_product where sku='" + Sku + "' and storeid=1 and isnull(active,0)=1 and  isnull(deleted,0)=0"));
                if (String.IsNullOrEmpty(checksku))
                {
                    InsertErrorinTemp("Product is Inactive  SKU : " + Sku, "0", "");
                }
                return true;
            }

        }

        private bool CheckPOUnique(string po1, string po2, string po3, string po4, String Sku, Int32 RowNumber, Int32 POBlocks)
        {
            try
            {
                String ErrorType = "Invalid PO # Value";
                String ExcelRowNumber = "";
                String ProductSKU = "";

                DataTable tb = new DataTable();
                DataColumn col1 = new DataColumn("PO", typeof(string));
                tb.Columns.Add(col1);
                DataColumn col2 = new DataColumn("num", typeof(string));
                tb.Columns.Add(col2);
                tb.AcceptChanges();
                Int32 er = 0;

                if (POBlocks >= 1)
                {
                    if (!String.IsNullOrEmpty(po1))
                    {
                        DataRow dd = tb.NewRow();
                        dd["PO"] = po1;
                        dd["num"] = "1";
                        tb.Rows.Add(dd);
                        tb.AcceptChanges();
                    }
                }
                if (POBlocks >= 2)
                {
                    if (!String.IsNullOrEmpty(po2))
                    {
                        DataRow dd = tb.NewRow();
                        dd["PO"] = po2;
                        dd["num"] = "2";
                        tb.Rows.Add(dd);
                        tb.AcceptChanges();
                    }
                }
                if (POBlocks >= 3)
                {
                    if (!String.IsNullOrEmpty(po2))
                    {
                        DataRow dd = tb.NewRow();
                        dd["PO"] = po3;
                        dd["num"] = "3";
                        tb.Rows.Add(dd);
                        tb.AcceptChanges();
                    }
                }
                if (POBlocks >= 4)
                {
                    if (!String.IsNullOrEmpty(po2))
                    {
                        DataRow dd = tb.NewRow();
                        dd["PO"] = po4;
                        dd["num"] = "4";
                        tb.Rows.Add(dd);
                        tb.AcceptChanges();
                    }
                }

                if (tb.Rows.Count > 0)
                {
                    for (int j = 0; j < tb.Rows.Count; j++)
                    {
                        string q = Convert.ToString(tb.Rows[j][0].ToString());
                        for (int k = j + 1; k < tb.Rows.Count; k++)
                        {
                            string yy = Convert.ToString(tb.Rows[k][0].ToString());
                            if (q.ToString().ToLower() == yy.ToString().ToLower())
                            {
                                // string pp = Convert.ToString(tb.Rows[j][1].ToString());
                                string pp = Convert.ToString(tb.Rows[k][1].ToString());
                                ErrorType = "Invalid PO " + pp + " Value";
                                ExcelRowNumber = RowNumber.ToString();
                                ProductSKU = Sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag((Convert.ToInt32(pp) - 1).ToString(), ExcelRowNumber);
                                er++;
                            }
                        }


                    }


                    if (er > 0)
                    {
                        //ExcelRowNumber = RowNumber.ToString();
                        //ProductSKU = Sku;
                        //InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        return false;
                    }
                }
                else
                {
                    return true;
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }



        }

        private bool CheckDeliveryDateinsequence(string sku, string po1, string po2, string po3, string po4, string qty1, string qty2, string qty3, string qty4, string shipping1, string shipping2, string shipping3, string shipping4, Int32 RowNumber, string eta1, string eta2, string eta3, string eta4)
        {
            DateTime today = DateTime.Now;

            Int32 checkpocounter = 0;
            Int32 ErrorPOCounter = 0;
            Int32 Error = 0;


            if (!String.IsNullOrEmpty(sku))
            {

                if (String.IsNullOrEmpty(po1) && String.IsNullOrEmpty(qty1) && String.IsNullOrEmpty(shipping1) && String.IsNullOrEmpty(eta1))
                {
                    ErrorPOCounter++;

                }
                else
                {
                    if (ErrorPOCounter == 0)
                        checkpocounter = 1;
                }

                if (String.IsNullOrEmpty(po2) && String.IsNullOrEmpty(qty2) && String.IsNullOrEmpty(shipping2) && String.IsNullOrEmpty(eta2) && ErrorPOCounter == 0)
                {
                    ErrorPOCounter++;

                }
                else
                {
                    if (ErrorPOCounter == 0)
                        checkpocounter = 2;
                }

                if (String.IsNullOrEmpty(po3) && String.IsNullOrEmpty(qty3) && String.IsNullOrEmpty(shipping3) && String.IsNullOrEmpty(eta3) && ErrorPOCounter == 0)
                {
                    ErrorPOCounter++;

                }
                else
                {
                    if (ErrorPOCounter == 0)
                        checkpocounter = 3;
                }

                if (String.IsNullOrEmpty(po4) && String.IsNullOrEmpty(qty4) && String.IsNullOrEmpty(shipping4) && String.IsNullOrEmpty(eta4) && ErrorPOCounter == 0)
                {
                    ErrorPOCounter++;

                }
                else
                {
                    if (ErrorPOCounter == 0)
                        checkpocounter = 4;

                }

                //////////check Order Quantity validation

                for (int i = 1; i <= checkpocounter; i++)
                {

                    if (i == 1)
                    {
                        if (!String.IsNullOrEmpty(qty1))
                        {
                            Int32 q1 = 0;
                            Int32.TryParse(qty1, out q1);
                            if (q1 > 0)
                            {

                            }
                            else
                            {
                                String ErrorType = "Invalid Quantity 1 Value";
                                String ExcelRowNumber = Convert.ToString(RowNumber);
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }
                        else
                        {
                            String ErrorType = "Missing Quantity 1 Value";
                            String ExcelRowNumber = Convert.ToString(RowNumber);
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("0", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 2)
                    {
                        if (!String.IsNullOrEmpty(qty2))
                        {
                            Int32 q2 = 0;
                            Int32.TryParse(qty2, out q2);
                            if (q2 > 0)
                            {

                            }
                            else
                            {
                                String ErrorType = "Invalid Quantity 2 Value";
                                String ExcelRowNumber = Convert.ToString(RowNumber);
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }
                        else
                        {
                            String ErrorType = "Missing Quantity 2 Value";
                            String ExcelRowNumber = Convert.ToString(RowNumber);
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 3)
                    {
                        if (!String.IsNullOrEmpty(qty3))
                        {
                            Int32 q3 = 0;
                            Int32.TryParse(qty3, out q3);
                            if (q3 > 0)
                            {

                            }
                            else
                            {
                                String ErrorType = "Invalid Quantity 3 Value";
                                String ExcelRowNumber = Convert.ToString(RowNumber);
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }
                        else
                        {
                            String ErrorType = "Missing Quantity 3 Value";
                            String ExcelRowNumber = Convert.ToString(RowNumber);
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;
                        }

                    }



                    if (i == 4)
                    {
                        if (!String.IsNullOrEmpty(qty4))
                        {
                            Int32 q4 = 0;
                            Int32.TryParse(qty4, out q4);
                            if (q4 > 0)
                            {

                            }
                            else
                            {
                                String ErrorType = "Invalid Quantity 4 Value";
                                String ExcelRowNumber = Convert.ToString(RowNumber);
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }
                        else
                        {
                            String ErrorType = "Missing Quantity 4 Value";
                            String ExcelRowNumber = Convert.ToString(RowNumber);
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;
                        }

                    }




                }

                ////////////////// check PONumber validation

                //Int32 PO1 = 0;
                //Int32 PO2 = 0;
                //Int32 PO3 = 0;
                //Int32 PO4 = 0;
                //Int32.TryParse(po1, out PO1);
                //Int32.TryParse(po2, out PO2);
                //Int32.TryParse(po3, out PO3);
                //Int32.TryParse(po4, out PO4);


                if (checkpocounter >= 1)
                {
                    if (!String.IsNullOrEmpty(po1))
                    {



                        //if (PO1 > 0)
                        //{

                        //}
                        //else
                        //{
                        //    String ErrorType = "Invalid Value for 'PO1'";
                        //    String ExcelRowNumber = RowNumber.ToString();
                        //    String ProductSKU = sku;
                        //    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                        //    Error++;

                        //}
                    }
                    else
                    {
                        String ErrorType = "Missing PO 1 Value";
                        String ExcelRowNumber = RowNumber.ToString();
                        String ProductSKU = sku;
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        UpdateFlag("0", ExcelRowNumber);

                        Error++;

                    }

                }
                if (checkpocounter >= 2)
                {
                    if (!String.IsNullOrEmpty(po2))
                    {
                        //if (PO2 > 0)
                        //{

                        //}
                        //else
                        //{
                        //    String ErrorType = "Invalid Value for 'PO2'";
                        //    String ExcelRowNumber = RowNumber.ToString();
                        //    String ProductSKU = sku;
                        //    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                        //    Error++;

                        //}
                    }
                    else
                    {
                        String ErrorType = "Missing PO 2 Value";
                        String ExcelRowNumber = RowNumber.ToString();
                        String ProductSKU = sku;
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        UpdateFlag("1", ExcelRowNumber);

                        Error++;

                    }

                }
                if (checkpocounter >= 3)
                {
                    if (!String.IsNullOrEmpty(po3))
                    {
                        //if (PO3 > 0)
                        //{

                        //}
                        //else
                        //{
                        //    String ErrorType = "Invalid Value for 'PO3'";
                        //    String ExcelRowNumber = RowNumber.ToString();
                        //    String ProductSKU = sku;
                        //    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                        //    Error++;

                        //}
                    }
                    else
                    {
                        String ErrorType = "Missing PO 3 Value";
                        String ExcelRowNumber = RowNumber.ToString();
                        String ProductSKU = sku;
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        UpdateFlag("2", ExcelRowNumber);
                        Error++;

                    }

                }

                if (checkpocounter >= 4)
                {
                    if (!String.IsNullOrEmpty(po3))
                    {
                        //if (PO4 > 0)
                        //{

                        //}
                        //else
                        //{
                        //    String ErrorType = "Invalid Value for 'PO4'";
                        //    String ExcelRowNumber = RowNumber.ToString();
                        //    String ProductSKU = sku;
                        //    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                        //    Error++;

                        //}
                    }
                    else
                    {
                        String ErrorType = "Missing PO 4 Value";
                        String ExcelRowNumber = RowNumber.ToString();
                        String ProductSKU = sku;
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        UpdateFlag("3", ExcelRowNumber);
                        Error++;

                    }

                }

                if (!CheckPOUnique(po1, po2, po3, po4, sku, RowNumber, checkpocounter))
                {
                    Error++;
                }



                //////////////check shipping date validation
                DateTime shippingdate1;
                DateTime shippingdate2;
                DateTime shippingdate3;
                DateTime shippingdate4;
                for (int i = 1; i <= checkpocounter; i++)
                {

                    if (i == 1)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(shipping1))
                            {
                                shippingdate1 = Convert.ToDateTime(shipping1);
                            }
                            else
                            {
                                String ErrorType = "Missing Date 1 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid Date 1 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("0", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 2)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(shipping2))
                            {
                                shippingdate2 = Convert.ToDateTime(shipping2);
                            }
                            else
                            {
                                String ErrorType = "Missing Date 2 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid Date 3 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 3)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(shipping3))
                            {
                                shippingdate3 = Convert.ToDateTime(shipping3);
                            }
                            else
                            {
                                String ErrorType = "Missing Date 3 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid Date 3 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;
                        }

                    }

                    if (i == 4)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(shipping4))
                            {
                                shippingdate4 = Convert.ToDateTime(shipping4);
                            }
                            else
                            {
                                String ErrorType = "Missing Date 4 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid Date 4 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;
                        }

                    }



                }




                ////////////////////validation for Date 1 < Date 2 < Date 3 < Date 4
                DateTime Etadate1;
                DateTime.TryParse(shipping1, out Etadate1);
                DateTime Etadate2;
                DateTime.TryParse(shipping2, out Etadate2);
                DateTime Etadate3;
                DateTime.TryParse(shipping3, out Etadate3);
                DateTime Etadate4;
                DateTime.TryParse(shipping4, out Etadate4);



                if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate1 = DateTime.Now;
                }
                if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate2 = DateTime.Now;
                }
                if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    //Etadate3 = DateTime.Now;
                }
                if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate4 = DateTime.Now;
                }
                //if ((Etadate1.Date < Etadate2.Date) && (Etadate2.Date < Etadate3.Date) && (Etadate3.Date < Etadate4.Date))
                //{

                //}
                //else
                //{

                //}
                if (checkpocounter >= 1)
                {
                    try
                    {


                        if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            Error++;
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("0", ExcelRowNumber);
                        }
                        else
                        {
                            if (today.Date.AddDays(-ReplacementAddDaysValidation) <= Etadate1.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong Date 1 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }

                }
                if (checkpocounter >= 2)
                {
                    try
                    {


                        if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etadate1.Date <= Etadate2.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong Date 2 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }


                if (checkpocounter >= 3)
                {
                    try
                    {


                        if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etadate2.Date <= Etadate3.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong Date 3 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }

                if (checkpocounter >= 4)
                {
                    try
                    {


                        if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etadate3.Date <= Etadate4.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong Date 4 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }












                //////////////check eta date validation




                DateTime etaatl1;
                DateTime etaatl2;
                DateTime etaatl3;
                DateTime etaatl4;
                for (int i = 1; i <= checkpocounter; i++)
                {

                    if (i == 1)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(eta1))
                            {
                                etaatl1 = Convert.ToDateTime(eta1);
                            }
                            else
                            {
                                String ErrorType = "Missing ETA ATL1 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid ETA ATL1 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("0", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 2)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(eta2))
                            {
                                etaatl2 = Convert.ToDateTime(eta2);
                            }
                            else
                            {
                                String ErrorType = "Missing ETA ATL2 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid ETA ATL3 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 3)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(eta3))
                            {
                                etaatl3 = Convert.ToDateTime(eta3);
                            }
                            else
                            {
                                String ErrorType = "Missing ETA ATL3 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid ETA ATL3 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;
                        }

                    }

                    if (i == 4)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(eta4))
                            {
                                etaatl4 = Convert.ToDateTime(eta4);
                            }
                            else
                            {
                                String ErrorType = "Missing ETA ATL4 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid ETA ATL4 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;
                        }

                    }



                }




                ////////////////////validation for Date 1 < Date 2 < Date 3 < Date 4
                DateTime Etaatldate1;
                DateTime.TryParse(eta1, out Etaatldate1);
                DateTime Etaatldate2;
                DateTime.TryParse(eta2, out Etaatldate2);
                DateTime Etaatldate3;
                DateTime.TryParse(eta3, out Etaatldate3);
                DateTime Etaatldate4;
                DateTime.TryParse(eta4, out Etaatldate4);



                if (Etaatldate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate1 = DateTime.Now;
                }
                if (Etaatldate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate2 = DateTime.Now;
                }
                if (Etaatldate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    //Etadate3 = DateTime.Now;
                }
                if (Etaatldate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate4 = DateTime.Now;
                }
                //if ((Etadate1.Date < Etadate2.Date) && (Etadate2.Date < Etadate3.Date) && (Etadate3.Date < Etadate4.Date))
                //{

                //}
                //else
                //{

                //}
                if (checkpocounter >= 1)
                {
                    try
                    {


                        if (Etaatldate1.ToString() == "" || Etaatldate1.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            Error++;
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("0", ExcelRowNumber);
                        }
                        else
                        {
                            if (today.Date.AddDays(-ReplacementAddDaysValidationETA) <= Etaatldate1.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong ETA ATL1 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }

                }
                if (checkpocounter >= 2)
                {
                    try
                    {


                        if (Etaatldate2.ToString() == "" || Etaatldate2.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etaatldate1.Date <= Etaatldate2.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong ETA ATL2 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }


                if (checkpocounter >= 3)
                {
                    try
                    {


                        if (Etaatldate3.ToString() == "" || Etaatldate3.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etaatldate2.Date <= Etaatldate3.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong ETA ATL3 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }

                if (checkpocounter >= 4)
                {
                    try
                    {


                        if (Etaatldate4.ToString() == "" || Etaatldate4.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etaatldate3.Date <= Etaatldate4.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong ETA ATL4 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }


            }
            else
            {
                String ErrorType = "Sku Should not be Blank";
                String ExcelRowNumber = Convert.ToString(RowNumber);
                String ProductSKU = sku;
                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                UpdateFlag("0", ExcelRowNumber);
                Error++;
            }

            if (checkpocounter == 0)
            {

                String ErrorType = "Remove sku or add PO Details in File";
                String ExcelRowNumber = Convert.ToString(RowNumber);
                String ProductSKU = sku;
                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                InsertErrorinTemp("No Replenishment Details Found", "0", "");
                UpdateFlag("0", ExcelRowNumber);
                Error++;
            }
            if (Error > 0)
            {
                return false;

            }
            else
            {
                return true;

            }



            //  return true;
        }

        #endregion

        protected void ddlColors_DataBound(object sender, EventArgs e)
        {
            string strImagePath = string.Empty;
            foreach (ListItem item in ddlColors.Items)
            {
                if (item.Text.ToString().Contains("~"))
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "color/large/" + item.Text.ToString().Split('~')[1].ToString(), "color");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text.ToString().Split('~')[0] + "'/>" + "<br/><span>" + item.Text.ToString().Split('~')[0] + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
                else
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "color/large/" + item.Text.ToString(), "color");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text + "'/>" + "<br/><span>" + item.Text + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
            }
        }

        protected void ddlStyle_DataBound(object sender, EventArgs e)
        {
            string strImagePath = string.Empty;
            foreach (ListItem item in ddlStyle.Items)
            {
                if (item.Text.ToString().Contains("~"))
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "Feature/large/" + item.Text.ToString().Split('~')[1].ToString(), "Feature");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text.ToString().Split('~')[0] + "'/>" + "<br/><span>" + item.Text.ToString().Split('~')[0] + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
                else
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "Feature/large/" + item.Text.ToString(), "Feature");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text + "'/>" + "<br/><span>" + item.Text + "</span>",
                    string.Format("Feature/large/" + "{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
            }
        }

        protected void ddlNewStyle_DataBound(object sender, EventArgs e)
        {
            string strImagePath = string.Empty;
            foreach (ListItem item in ddlNewStyle.Items)
            {
                if (item.Text.ToString().Contains("~"))
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "style/large/" + item.Text.ToString().Split('~')[1].ToString(), "style");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text.ToString().Split('~')[0] + "'/>" + "<br/><span>" + item.Text.ToString().Split('~')[0] + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
                else
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "style/large/" + item.Text.ToString(), "style");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text + "'/>" + "<br/><span>" + item.Text + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
            }
        }

        protected void chkHeader_DataBound(object sender, EventArgs e)
        {
            string strImagePath = string.Empty;
            foreach (ListItem item in chkHeader.Items)
            {
                if (item.Text.ToString().Contains("~"))
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "header/large/" + item.Text.ToString().Split('~')[1].ToString(), "header");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text.ToString().Split('~')[0] + "'/>" + "<br/><span>" + item.Text.ToString().Split('~')[0] + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
                else
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "header/large/" + item.Text.ToString(), "header");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text + "'/>" + "<br/><span>" + item.Text + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
            }
        }

        protected void ddlRoom_DataBound(object sender, EventArgs e)
        {
            string strImagePath = string.Empty;
            foreach (ListItem item in ddlRoom.Items)
            {
                if (item.Text.ToString().Contains("~"))
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "room/large/" + item.Text.ToString().Split('~')[1].ToString(), "room");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text.ToString().Split('~')[0] + "'/>" + "<br/><span>" + item.Text.ToString().Split('~')[0] + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
                else
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "room/large/" + item.Text.ToString(), "room");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text + "'/>" + "<br/><span>" + item.Text + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
            }
        }

        protected void ddlFab_DataBound(object sender, EventArgs e)
        {
            string strImagePath = string.Empty;
            foreach (ListItem item in ddlFab.Items)
            {
                if (item.Text.ToString().Contains("~"))
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "fabric/large/" + item.Text.ToString().Split('~')[1].ToString(), "fabric");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text.ToString().Split('~')[0] + "'/>" + "<br/><span>" + item.Text.ToString().Split('~')[0] + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
                else
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "fabric/large/" + item.Text.ToString(), "fabric");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text + "'/>" + "<br/><span>" + item.Text + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
            }
        }

        protected void ddlPatt_DataBound(object sender, EventArgs e)
        {
            string strImagePath = string.Empty;
            foreach (ListItem item in ddlPatt.Items)
            {
                if (item.Text.ToString().Contains("~"))
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "pattern/large/" + item.Text.ToString().Split('~')[1].ToString(), "pattern");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text.ToString().Split('~')[0] + "'/>" + "<br/><span>" + item.Text.ToString().Split('~')[0] + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
                else
                {
                    strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "pattern/large/" + item.Text.ToString(), "pattern");
                    item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text + "'/>" + "<br/><span>" + item.Text + "</span>",
                    string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
                }
            }
        }

        public String GetImagePath(string img, string Folder)
        {
            String imagepath = String.Empty;
            String Temp = img;
            imagepath = Temp;
            if (System.IO.File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct"), Folder + "/Micro/image_not_available.jpg");
        }


        private Boolean CheckImageSize(FileUpload ff, int width, int height)
        {
            try
            {
                if (!Directory.Exists(Server.MapPath("/Resources/temp")))
                {
                    Directory.CreateDirectory(Server.MapPath("/Resources/temp"));

                }
                FileInfo file = new FileInfo(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                if (file.Exists)
                {
                    file.Delete();
                }

                ff.SaveAs(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                if (img.Width == width && img.Height == height)
                {
                    img.Dispose();
                    return true;

                }
                else
                {
                    img.Dispose();
                    return false;
                }
            }
            catch
            {

            }

            return false;
        }

        //protected void chkmaterial_DataBound(object sender, EventArgs e)
        //{
        //    string strImagePath = string.Empty;
        //    foreach (ListItem item in chkmaterial.Items)
        //    {
        //        if (item.Text.ToString().Contains("~"))
        //        {
        //            strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "material/large/" + item.Text.ToString().Split('~')[1].ToString(), "fabric");
        //            item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text.ToString().Split('~')[0] + "'/>" + "<br/><span>" + item.Text.ToString().Split('~')[0] + "</span>",
        //            string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
        //        }
        //        else
        //        {
        //            strImagePath = GetImagePath(AppLogic.AppConfigs("ImagePathProduct") + "material/large/" + item.Text.ToString(), "fabric");
        //            item.Text = string.Format("<img style='width:50px; height:50px;border:1px solid #dadada;border-radius:100%;' src= \"{0}\" title='" + item.Text + "'/>" + "<br/><span>" + item.Text + "</span>",
        //            string.Format("{0}", AppLogic.AppConfigs("Live_Contant_Server") + strImagePath), null);
        //        }
        //    }
        //}

    }
}
