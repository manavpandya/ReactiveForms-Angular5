using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Path = System.IO.Path;
using System.Collections;
using Solution.Bussines.Components.AdminCommon;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.IO;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductNewEgg : BasePage
    {

        tb_Product tb_Product = null;
        tb_ProductCategory tbProductCategory = null;
        StoreComponent stac = new StoreComponent();
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

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");
                //int StoreID = Convert.ToInt32(AppLogic.AppConfigs("StoreID"));
                if (Request.QueryString["StoreID"] != null)
                {
                    int StoreID;
                    StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                    BindStore();
                    ddlStore.SelectedValue = StoreID.ToString();
                    ddlStore.Enabled = false;
                    BindManufacture(StoreID);
                    BindCategory(StoreID);
                    if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null)
                    {
                        BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                    }
                    else
                    {
                        ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                    }
                    AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
                }
                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            }
        }

        /// <summary>
        /// Binds the Manufacture
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
        /// Bind All Stores in Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlStore.DataSource = Storelist;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
            }
            else
            {
                ddlStore.DataSource = null;
            }
            ddlStore.DataBind();
            ddlStore.Items.Insert(0, new ListItem("All Store", "0"));

        }

        /// <summary>
        /// Binds the Product Category
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        protected void BindCategory(Int32 StoreID)
        {
            DataSet dsCategory = new DataSet();
            dsCategory = CategoryComponent.GetCategoryByStoreID(StoreID, 3);
            LoadCategoryTree(StoreID.ToString(), dsCategory);
        }

        /// <summary>
        /// Loads the Category Tree
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="dsCategory">Datset dsCategory</param>
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
        /// Loads the Child Node for Category Tree
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
                if (fuImage.FileName.Length > 0 && Path.GetExtension(fuImage.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fuImage.FileName.Length > 0)
                {
                    try
                    {
                        FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + fuImage.FileName);
                        if (flinfo.Exists)
                        {
                            flinfo.Delete();
                        }
                    }
                    catch { }
                    ViewState["File"] = fuImage.FileName.ToString();
                    fuImage.SaveAs(Server.MapPath(ProductTempPath) + fuImage.FileName);
                    ImgLarge.Src = ProductTempPath + fuImage.FileName;

                }
                else
                {
                    ViewState["File"] = null;
                }
            }
            else
            {
                string s = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + s + "', 'Message','ContentPlaceHolder1_fuImage');});", true);
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
        /// Removes the Special Character for String
        /// </summary>
        /// <param name="charr">char[] charr</param>
        /// <returns>Returns String with  Removed Special Character</returns>
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
        /// Adds the Product Category
        /// </summary>
        /// <returns>Returns an ArrayList of Product Category</returns>
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
        /// Gets the CategoryIDs List
        /// </summary>
        /// <param name="array">ArrayList array</param>
        /// <param name="tn">TreeNode tn</param>
        /// <returns>Returns the Array List of CategoryIDs</returns>
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
        /// Sets the value of Product into Controls
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns tb_Product Table Object</returns>
        public tb_Product SetValue(tb_Product tb_Product)
        {
            if (Request.QueryString["StoreID"] != null)
                tb_Product.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(Request.QueryString["StoreID"].ToString()));

            tb_Product.Name = Convert.ToString(txtproductname.Text.Trim());
            tb_Product.SKU = txtSKU.Text.Trim();
            tb_Product.SellerPartNo = txtsellerpartnumber.Text.Trim();
            if (ddlManufacture.SelectedValue != "0")
            {
                tb_Product.tb_ManufactureReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Manufacture", "ManufactureID", Convert.ToInt32(ddlManufacture.SelectedValue));
            }
            tb_Product.Isbn = txtisbn.Text.Trim();
            tb_Product.UPC = txtUPC.Text.Trim();
            tb_Product.MainCategory = txtMaincategory.Text.ToString().Trim();
            tb_Product.ManuItemURL = txtManufactureItemURL.Text.ToString().Trim();
            tb_Product.RelatedProduct = txtRelatedSellerPartno.Text.ToString().Trim();
            tb_Product.WebSiteShortTitle = txtWebsiteShortTitle.Text.ToString().Trim();
            tb_Product.WebSiteLongTitle = txtWebsiteLongTitle.Text.ToString().Trim();
            tb_Product.Description = Convert.ToString((txtProductDescription.Text.Trim()));

            decimal Length = decimal.Zero;
            decimal.TryParse(txtitemlength.Text.Trim(), out Length);
            tb_Product.Length = Convert.ToString(Length);

            decimal Width = decimal.Zero;
            decimal.TryParse(txtitemwidth.Text.Trim(), out Width);
            tb_Product.Width = Convert.ToString(Width);


            decimal Height = decimal.Zero;
            decimal.TryParse(txtitemheight.Text.Trim(), out Height);
            tb_Product.Height = Convert.ToString(Height);

            decimal Weight = decimal.Zero;
            decimal.TryParse(txtItemWeight.Text.Trim(), out Weight);
            tb_Product.Weight = Convert.ToDecimal(Weight);

            tb_Product.ItemPackage = ddlitempackage.SelectedValue;
            tb_Product.IsRestricted = chkShippingRestiction.Checked;

            decimal Price = decimal.Zero;
            decimal.TryParse(txtPrice.Text.Trim(), out Price);
            tb_Product.Price = Convert.ToDecimal(Price);

            if (txtsellingprice.Text.Trim().Length == 0)
                txtsellingprice.Text = "0";
            if (txtsellingprice.Text.Trim() != "")
                tb_Product.SalePrice = Convert.ToDecimal(txtsellingprice.Text.Trim());
            else tb_Product.SalePrice = Convert.ToDecimal(0);

            tb_Product.Shipping = ddlshipping.SelectedValue;
            if (txtInventory.Text.Trim().Length == 0)
                txtInventory.Text = "0";
            tb_Product.Inventory = Convert.ToInt32(txtInventory.Text.Trim());

            tb_Product.ActivationMark = chkActivationMark.Checked;
            // tb_Product.SurCharge = Convert.ToDecimal((txtSurCharge.Text.Trim() == "") ? "0.00" : txtSurCharge.Text.Trim());

            tb_Product.Prop65 = chkProp65.Checked;
            tb_Product.Pro65Motherboard = chkProp65Motherboard.Checked;
            tb_Product.Age18Verification = chkAgeVerification.Checked;

            if (ddlchoking1.SelectedValue != "0")
            {
                tb_Product.ChokingHazard1 = ddlchoking1.SelectedValue;
            }
            else
                tb_Product.ChokingHazard1 = null;
            if (ddlchoking2.SelectedValue != "0")
            {
                tb_Product.ChokingHazard2 = ddlchoking2.SelectedValue;
            }
            else
                tb_Product.ChokingHazard2 = null;
            if (ddlchoking3.SelectedValue != "0")
            {
                tb_Product.ChokingHazard3 = ddlchoking3.SelectedValue;
            }
            else
                tb_Product.ChokingHazard3 = null;
            if (ddlchoking4.SelectedValue != "0")
            {
                tb_Product.ChokingHazard4 = ddlchoking4.SelectedValue;
            }
            else
                tb_Product.ChokingHazard4 = null;

            tb_Product.SETitle = txtSETitle.Text.Trim();
            tb_Product.SEKeywords = txtSEKeyword.Text.ToString().Trim();
            tb_Product.SEDescription = txtSEDescription.Text.Trim();

            if (string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()) && txtDisplayOrder.Text.Trim() == "")
                tb_Product.DisplayOrder = 0;
            else tb_Product.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());
            tb_Product.Active = chkPublished.Checked;

            tb_Product.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
            tb_Product.UpdatedOn = System.DateTime.Now;
            return tb_Product;
        }

        /// <summary>
        /// Sets the Category
        /// </summary>
        /// <param name="StoreId">int StoreId</param>
        /// <returns>Returns the String of CategoryIDs</returns>
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
        /// Adds the Category into Database
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
                CommonComponent.ErrorLog("Productsears.aspx", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Inserts the Product Data into Product Table and Buy Product Table
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        public void InsertProduct(int StoreID)
        {
            tb_Product = new tb_Product();
            tb_Product = SetValue(tb_Product);
            tb_Product.TabTitle1 = "Product Description";
            tb_Product.CreatedBy = Convert.ToInt32(Session["AdminID"]);
            tb_Product.CreatedOn = System.DateTime.Now;

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

                    Response.Redirect("ProductList.aspx?Insert=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
                }
            }
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
        /// Binds the Data of New Egg Product
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="StoreID">int StoreID</param>
        public void BindData(Int32 ID, Int32 StoreID)
        {
            DataSet DsProduct = new DataSet();
            DsProduct = ProductComponent.GetProductByProductID(ID);

            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
                ddlStore.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["StoreID"]);
                txtproductname.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Name"]);
                txtSKU.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SKU"]);
                txtsellerpartnumber.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SellerPartNo"].ToString());
                ddlManufacture.SelectedValue = DsProduct.Tables[0].Rows[0]["ManufactureID"].ToString();
                txtisbn.Text = DsProduct.Tables[0].Rows[0]["Isbn"].ToString();
                txtUPC.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["UPC"]);
                txtManufactureItemURL.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["ManuItemURL"].ToString());
                txtRelatedSellerPartno.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["RelatedProduct"].ToString());
                txtWebsiteShortTitle.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["WebSiteShortTitle"].ToString());
                txtWebsiteLongTitle.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["WebSiteLongTitle"].ToString());
                txtProductDescription.Text = Server.HtmlDecode(DsProduct.Tables[0].Rows[0]["Description"].ToString());
                txtitemlength.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["length"].ToString());
                txtitemwidth.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["width"].ToString());
                txtitemheight.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["height"].ToString());
                txtItemWeight.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["weight"].ToString());
                ddlitempackage.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ItemPackage"].ToString());
                chkShippingRestiction.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["IsRestricted"].ToString());

                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["Price"].ToString().Trim()))
                    txtPrice.Text = "0.00";
                else
                    txtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["Price"].ToString().Trim()), 2));


                if (string.IsNullOrEmpty(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString().Trim()))
                    txtsellingprice.Text = "0.00";
                else
                    txtsellingprice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(DsProduct.Tables[0].Rows[0]["SalePrice"].ToString().Trim()), 2));

                ddlshipping.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["Shipping"].ToString());
                txtInventory.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["Inventory"].ToString());
                int ChkLowInventory = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(*) from tb_Product where ProductID = " + Request.QueryString["ID"].ToString() + " and (ISNULL( tb_Product.Inventory,0) <  ISNULL( tb_Product.LowInventory,0))"));

                if (ChkLowInventory > 0)
                {
                    ImgToggelInventory.Visible = true;
                }


                chkActivationMark.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["ActivationMark"].ToString());
                chkProp65.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Prop65"].ToString());
                chkProp65Motherboard.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Pro65Motherboard"].ToString());
                chkAgeVerification.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Age18Verification"].ToString());
                ddlchoking1.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ChokingHazard1"].ToString());
                ddlchoking2.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ChokingHazard2"].ToString());
                ddlchoking3.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ChokingHazard3"].ToString());
                ddlchoking4.SelectedValue = Convert.ToString(DsProduct.Tables[0].Rows[0]["ChokingHazard4"].ToString());
                txtSETitle.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SETitle"].ToString());
                txtSEKeyword.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SEKeywords"].ToString());
                txtSEDescription.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["SEDescription"].ToString());
                chkPublished.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Active"].ToString());
                txtDisplayOrder.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["DisplayOrder"].ToString().Trim());

                #region Get Image
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                    Directory.CreateDirectory(Server.MapPath(ProductMediumPath));
                string Imagename = DsProduct.Tables[0].Rows[0]["ImageName"].ToString();
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
            }
        }

        /// <summary>
        /// Binds the Tree Data for Category
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
        /// Edits the tree for Category
        /// </summary>
        /// <param name="Name">String Name</param>
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
        /// Deletes the Category for Specific Product from Database
        /// </summary>
        /// <param name="product">int product</param>
        public void DeleteCategory(int product)
        {
            CategoryComponent.DeleteProductCategory(product);
        }

        /// <summary>
        /// Update the Product Data into Product Table and Buy Product Table
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="StoreID">int StoreID</param>
        public void UpdateProduct(int ProductID, int StoreId)
        {
            ProductComponent objProduct = new ProductComponent();
            tb_Product = new tb_Product();
            tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);
            tb_Product = SetValue(tb_Product);

            Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ISNULL(count(sku),0) as TotCnt From tb_product Where sku='" + txtSKU.Text.ToString().Trim() + "' And StoreId=" + Request.QueryString["StoreID"].ToString() + " and Productid <>" + ProductID + " AND ISNULL(Deleted,0)=0 AND ISNULL(Active,0)=1"));
            if (ChkSKu > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('SKU with same name already exists specify another sku', 'Message','ContentPlaceHolder1_txtSKU');});", true);
                txtSKU.Focus();
                return;
            }
            else
            {

                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"] == "edit" && Request.QueryString["ID"] != null)
                {
                    DeleteCategory(Convert.ToInt32(Request.QueryString["ID"].ToString()));
                }
                string CategoryValue = "";
                CategoryValue = SetCategory(StoreId);

                if (Convert.ToInt32(ProductComponent.UpdateProduct(tb_Product)) > 0)
                {
                    AddCategory(CategoryValue, ProductID);

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
                    Response.Redirect("ProductList.aspx?Update=true&StoreID=" + ((Request.QueryString["StoreID"] != null) ? Convert.ToString(Request.QueryString["StoreID"]) : ""));
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
    }
}