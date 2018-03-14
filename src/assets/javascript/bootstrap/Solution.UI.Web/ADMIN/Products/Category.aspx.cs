using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Collections;
namespace Solution.UI.Web.ADMIN.Settings
{
    /// <summary>
    /// Category Page For user to maintain data by UI
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 
    public partial class category : Solution.UI.Web.BasePage
    {
        #region Declaration

        #region components

        RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();
        StoreComponent objStorecomponent = new StoreComponent();
        CategoryComponent objCategorycomponent = new CategoryComponent();
        ConfigurationComponent objConfiguration = new ConfigurationComponent();

        #endregion

        #region entities

        tb_Category ctxCategory = null;
        tb_CategoryMapping ctxCategorymapping = null;

        #endregion

        #region Variables

        public static string CategoryTempPath = string.Empty;
        public static string CategoryIconPath = string.Empty;
        public static string CategoryMediumPath = string.Empty;
        public static string CategoryLargePath = string.Empty;
        public static string CategoryMicroPath = string.Empty;
        public static string CategoryBannerPath = string.Empty;
        public static string CategoryBannerTempPath = string.Empty;

        static Size thumbNailSizeIcon = Size.Empty;
        static Size thumbNailSizeMicro = Size.Empty;
        static Size thumbNailSizeBanner = Size.Empty;
        public static System.Text.StringBuilder sbCatMap = null;
        public static System.Text.StringBuilder sb = null;
        static string[] cmid;
        static string[] parentValue;
        int finHeight;
        int finWidth;

        public String strSearchedCategory = ",";
        public String strNodeExpand = ",";
        public String strParentNodeExpand = ",";
        public ArrayList arrSearchedCategory = new ArrayList();
        Int32 nodecount = -1;
        Int32 nodecount1 = 0;
        int StoreID = 1;
        #endregion

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            clsvariables.LoadAllPath();
            if (!Page.IsPostBack)
            {
                if (Request.QueryString["mode"] != null)
                {
                    if (Request.QueryString["mode"].ToString().Equals("Inserted"))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Category Inserted Successfully.', 'Message');});", true);
                    }
                    else if (Request.QueryString["mode"].ToString().Equals("Updated"))
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Category Updated Successfully.', 'Message');});", true);
                    }
                }

                bindstore();

                if (Request.QueryString["StoreID"] != null)
                {
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(Request.QueryString["StoreID"].ToString().Trim()));
                    AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue);
                    StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString().Trim());
                }
                else
                {
                    StoreID = 1;
                    AppConfig.StoreID = 1;
                }

                if (Request.QueryString["Search"] != null)
                {
                    ViewState["SearchEnabled"] = "1";
                    txtSearch.Text = Request.QueryString["Search"].ToString();
                }
                if (Request.QueryString["ActiveStatus"] != null)
                {
                    rdlCategoryStatus.SelectedIndex = Convert.ToInt32(Request.QueryString["ActiveStatus"]);
                }
                FillProductfeature();
                if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null && Request.QueryString["StoreID"] != null && Request.QueryString["Mode"] == "edit")
                {
                    lblTitle.Text = "Update Category";
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(Request.QueryString["StoreID"].ToString().Trim()));
                    BindData(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                    li2.Visible = true;
                    li3.Visible = true;
                    // li4.Visible = true;
                    li5.Visible = true;
                }
                else
                {
                    lblTitle.Text = "Add Category";
                    rblIsfeaturedNo.Checked = true;
                    BindImage("");
                }
                bindTreeviewwithcategory();
                FillMasterCategorylist();



            }
            imgSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
            imgCancle.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
            ibtnBanner2.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            ibtnBanner3.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            ibtnBanner.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            ibtnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            ibtngo.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif";
            BindSizes();
            Page.MaintainScrollPositionOnPostBack = true;
        }

        /// <summary>
        /// Bind the Size of the Images
        /// </summary>
        private void BindSizes()
        {
            try
            {
                DataSet dsIconWidth = objConfiguration.GetImageSizeByType(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "CategoryIconWidth");
                DataSet dsIconHeight = objConfiguration.GetImageSizeByType(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "CategoryIconHeight");

                DataSet dsMicroWidth = objConfiguration.GetImageSizeByType(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "CategoryMicroWidth");
                DataSet dsMicroHeight = objConfiguration.GetImageSizeByType(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "CategoryMicroHeight");
                if ((dsIconWidth != null && dsIconWidth.Tables.Count > 0 && dsIconWidth.Tables[0].Rows.Count > 0) && (dsIconHeight != null && dsIconHeight.Tables.Count > 0 && dsIconHeight.Tables[0].Rows.Count > 0))
                {
                    thumbNailSizeIcon = new Size(Convert.ToInt32(dsIconWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsIconHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
                    //thumbNailSizeMicro = new Size(100, 130);

                }
                if ((dsMicroWidth != null && dsMicroWidth.Tables.Count > 0 && dsMicroWidth.Tables[0].Rows.Count > 0) && (dsMicroHeight != null && dsMicroHeight.Tables.Count > 0 && dsMicroHeight.Tables[0].Rows.Count > 0))
                {
                    thumbNailSizeMicro = new Size(Convert.ToInt32(dsMicroWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMicroHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
                }
                DataSet dsBannerWidth = objConfiguration.GetImageSizeByType(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "CategoryBannerWidth");
                DataSet dsBannerHeight = objConfiguration.GetImageSizeByType(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "CategoryBannerHeight");
                if ((dsBannerWidth != null && dsBannerWidth.Tables.Count > 0 && dsBannerWidth.Tables[0].Rows.Count > 0) && (dsBannerHeight != null && dsBannerHeight.Tables.Count > 0 && dsBannerHeight.Tables[0].Rows.Count > 0))
                {
                    thumbNailSizeBanner = new Size(Convert.ToInt32(dsBannerWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsBannerHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));

                }
            }
            catch { }
        }

        /// <summary>
        /// Method for Bind Existing Category Details
        /// </summary>
        /// <param name="catId">int catId</param>
        /// <param name="storeid">int storeid</param>
        private void BindData(int catId, int storeid)
        {
            DataSet ds = objCategorycomponent.getCatdetailbycatid(catId, storeid);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                txtname.Text = ds.Tables[0].Rows[0]["Name"].ToString();
                if (ds.Tables[0].Rows[0]["DisplayOrder"] != null)
                    txtdisplayorder.Text = ds.Tables[0].Rows[0]["DisplayOrder"].ToString();
                else
                    txtdisplayorder.Text = "0";
                if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("sears") > -1)
                {
                    lblSearsCategoryID.Text = "Sears Category ID";
                    trSearsCatID.Visible = true;
                    if (ds.Tables[0].Rows[0]["SearsCategoryID"] != null)
                        txtSearsCategoryID.Text = ds.Tables[0].Rows[0]["SearsCategoryID"].ToString();
                    else
                        txtSearsCategoryID.Text = "0";
                }
                else if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1)
                {
                    lblSearsCategoryID.Text = "Over Stock Category ID";
                    trSearsCatID.Visible = true;
                    if (ds.Tables[0].Rows[0]["SearsCategoryID"] != null)
                        txtSearsCategoryID.Text = ds.Tables[0].Rows[0]["SearsCategoryID"].ToString();
                    else
                        txtSearsCategoryID.Text = "0";
                }
                else
                {
                    lblSearsCategoryID.Text = "";
                    trSearsCatID.Visible = false;
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["FeatureID"].ToString()))
                {
                    ddlproductfeature.SelectedValue = ds.Tables[0].Rows[0]["FeatureID"].ToString();
                }
                else { ddlproductfeature.SelectedValue = "0"; }
                txtSummary.Text = Server.HtmlDecode(ds.Tables[0].Rows[0]["Summary"].ToString());

                txtBestSellerSKUs.Text = ds.Tables[0].Rows[0]["BestSellerSKUs"].ToString();

                txtDescription.Text = Server.HtmlDecode(ds.Tables[0].Rows[0]["Description"].ToString());
                //txttooltip.Text = ds.Tables[0].Rows[0]["ToolTip"].ToString();
                txtseodescription.Text = ds.Tables[0].Rows[0]["SEDescription"].ToString();
                txtsetitle.Text = ds.Tables[0].Rows[0]["SETitle"].ToString();
                txtsekeyword.Text = ds.Tables[0].Rows[0]["SEKeywords"].ToString();
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShowOnEff"].ToString()))
                {
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowOnEff"].ToString()))
                        rdoShowOnEff.Items.FindByValue(ds.Tables[0].Rows[0]["ShowOnEff"].ToString()).Selected = true;
                    else
                        rdoShowOnEff.Items.FindByValue("False").Selected = true;
                }
                txtbannertext.Text = Server.HtmlDecode(ds.Tables[0].Rows[0]["BannerText"].ToString());
                txteffname.Text = ds.Tables[0].Rows[0]["EFFName"].ToString();
                txteffurl.Text = ds.Tables[0].Rows[0]["EFFSEName"].ToString();
                txtEFFDescription.Text = Server.HtmlDecode(ds.Tables[0].Rows[0]["EFFDescription"].ToString());
                txtEFFseodescription.Text = ds.Tables[0].Rows[0]["EFFSEDescription"].ToString();
                txtEFFsetitle.Text = ds.Tables[0].Rows[0]["EFFSETitle"].ToString();
                txtEFFsekeyword.Text = ds.Tables[0].Rows[0]["EFFSEKeywords"].ToString();
                if (rblPublished.Items.FindByValue(ds.Tables[0].Rows[0]["Active"].ToString()) != null)
                    rblPublished.Items.FindByValue(ds.Tables[0].Rows[0]["Active"].ToString()).Selected = true;
                else rblPublished.Items.FindByValue("False").Selected = true;

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["IsFeatured"].ToString()))
                {
                    if (Convert.ToBoolean(ds.Tables[0].Rows[0]["IsFeatured"].ToString()))
                    { rblIsfeatured.Checked = true; }
                    else
                        rblIsfeaturedNo.Checked = true;
                }

                txtHeaderText.Text = Convert.ToString(ds.Tables[0].Rows[0]["HeaderText"].ToString());
                txtShortTitle.Text = Convert.ToString(ds.Tables[0].Rows[0]["ShortName"].ToString());

                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Isseocanonical"].ToString()))
                {
                    chkcanonical.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["Isseocanonical"].ToString());
                }
                if (chkcanonical.Checked == false)
                {
                    txtcanonical.Attributes.Add("readonly", "true");
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["CanonicalUrl"].ToString()))
                {
                    txtcanonical.Text = ds.Tables[0].Rows[0]["CanonicalUrl"].ToString();
                }
                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["Isnotfaceted"].ToString()) && Convert.ToBoolean(ds.Tables[0].Rows[0]["Isnotfaceted"].ToString()))
                {
                    chkfaceted.Checked = true;
                
                }

                BindImage(ds.Tables[0].Rows[0]["ImageName"].ToString());
                ViewState["ImageName"] = Convert.ToString(ds.Tables[0].Rows[0]["ImageName"]);
                BindBanner(ds.Tables[0].Rows[0]["BannerImageName"].ToString());
                BindBanner1(ds.Tables[0].Rows[0]["BannerImageName1"].ToString());

                BindBanner2(ds.Tables[0].Rows[0]["BannerImageName2"].ToString());
                //set bottom navigate url Products,DisplayOrder,Price,Inventory
                if (Request.QueryString["ID"] != null && Request.QueryString["storeid"] != null)
                {
                    //hlProducts.Target = "frmProducts";
                    frmProducts.Attributes.Add("onload", "iframeAutoheight(this);");
                    // hlProducts.NavigateUrl = "Products.aspx?ID=" + Request.QueryString["ID"] + "&StoreID=" + Request.QueryString["storeid"] + "&mode=product";
                    //hlProducts.Attributes.Add("onclick", "vsble();");

                    frmProducts.Attributes.Add("src", "Products.aspx?ID=" + Request.QueryString["ID"] + "&StoreID=" + Request.QueryString["storeid"] + "&mode=product");
                    // hlDisplayOrder.Target = "frmDisplayOrder";
                    frmDisplayOrder.Attributes.Add("onload", "iframeAutoheight(this);");
                    //hlDisplayOrder.NavigateUrl = "Products.aspx?ID=" + Request.QueryString["ID"] + "&StoreID=" + Request.QueryString["storeid"] + "&mode=DO";
                    //hlDisplayOrder.Attributes.Add("onclick", "vsbleDisplayOrder();");
                    frmDisplayOrder.Attributes.Add("src", "Products.aspx?ID=" + Request.QueryString["ID"] + "&StoreID=" + Request.QueryString["storeid"] + "&mode=DO");
                    //hlInventory.Target = "frminventory"; 
                    frminventory.Attributes.Add("onload", "iframeAutoheight(this);");
                    //hlInventory.NavigateUrl = "Products.aspx?ID=" + Request.QueryString["ID"] + "&StoreID=" + Request.QueryString["storeid"] + "&mode=inventory";
                    //hlInventory.Attributes.Add("onclick", "vsblefrminventory();");
                    frminventory.Attributes.Add("src", "Products.aspx?ID=" + Request.QueryString["ID"] + "&StoreID=" + Request.QueryString["storeid"] + "&mode=inventory");
                    //hlPrice.Target = "frmprice";
                    frmprice.Attributes.Add("onload", "iframeAutoheight(this);");
                    frmprice.Attributes.Add("src", "Products.aspx?ID=" + Request.QueryString["ID"] + "&StoreID=" + Request.QueryString["storeid"] + "&mode=price");
                    //hlPrice.NavigateUrl = "Products.aspx?ID=" + Request.QueryString["ID"] + "&StoreID=" + Request.QueryString["storeid"] + "&mode=price";
                    // hlPrice.Attributes.Add("onclick", "vsblefrmprice();");

                    var resultVar = ctxRedtag.tb_Category.Join(ctxRedtag.tb_CategoryMapping, tb_Category => tb_Category.CategoryID, tb_CategoryMapping => tb_CategoryMapping.CategoryID, (tb_Category, tb_CategoryMapping) => new { tb_Category.CategoryID, tb_CategoryMapping.ParentCategoryID, tb_Category.Name }).Where(tb_CategoryMapping => tb_CategoryMapping.ParentCategoryID == 0).ToList();

                    trShowonHeader.Attributes.Add("style", "display:none;");
                    if (resultVar.Count > 0)
                    {
                        foreach (var catName in resultVar)
                        {
                            if (Request.QueryString["ID"] != null && catName.CategoryID == Convert.ToInt32(Request.QueryString["ID"]))
                            {
                                trShowonHeader.Attributes.Add("style", "display:'';");
                                if (!string.IsNullOrEmpty(ds.Tables[0].Rows[0]["ShowOnHeader"].ToString()) && Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowOnHeader"].ToString()))
                                    chkShowOnHeader.Checked = Convert.ToBoolean(ds.Tables[0].Rows[0]["ShowOnHeader"].ToString());
                            }
                        }
                    }
                }
            }
            else
            {
                rblIsfeaturedNo.Checked = true;
            }
        }

        /// <summary>
        /// Display the Image Function
        /// </summary>
        /// <param name="imageName">string imageName</param>
        private void BindImage(string imageName)
        {
            imageName = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Icon/") + imageName;
            if (File.Exists(Server.MapPath(imageName)))
            {
                imgIcon.Src = imageName;

            }
            else
            {
                imgIcon.Src = string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");

            }
        }

        /// <summary>
        /// Binds the Banner for Display
        /// </summary>
        /// <param name="imageName">string imageName</param>
        private void BindBanner(string imageName)
        {
            imageName = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Banner/") + imageName;
            if (File.Exists(Server.MapPath(imageName)))
            {
                imgBanner.Src = imageName;
                imgdelete.Visible = true;

            }
            else
            {
                imgBanner.Src = string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
                imgdelete.Visible = false;
            }
        }
        /// <summary>
        /// Binds the Banner for Display
        /// </summary>
        /// <param name="imageName">string imageName</param>
        private void BindBanner1(string imageName)
        {
            imageName = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Banner/") + imageName;
            if (File.Exists(Server.MapPath(imageName)))
            {
                imgBanner2.Src = imageName;
                imgdelete2.Visible = true;

            }
            else
            {
                imgBanner2.Src = string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
                imgdelete2.Visible = false;
            }
        }

        /// <summary>
        /// Binds the Banner for Display
        /// </summary>
        /// <param name="imageName">string imageName</param>
        private void BindBanner2(string imageName)
        {
            imageName = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Banner/") + imageName;
            if (File.Exists(Server.MapPath(imageName)))
            {
                imgBanner3.Src = imageName;
                imgdelete3.Visible = true;

            }
            else
            {
                imgBanner3.Src = string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
                imgdelete3.Visible = false;
            }
        }
        //// <summary>
        //// Method for Bind Treeview with Category Details
        //// </summary>
        private void bindTreeviewwithcategory()
        {
            if (Request.QueryString["ID"] != null)
            {
                bindParentValue();
            }

            tvCategory.Nodes.Clear();
            TreeNode rootnode = new TreeNode();
            rootnode.Text = "Root Category";
            rootnode.Value = "0";
            rootnode.NavigateUrl = "javascript:void(0);";
            rootnode.ShowCheckBox = true;
            tvCategory.Nodes.Add(rootnode);
            int StoreID = Convert.ToInt32(ddlStore.SelectedValue);
            var resultVar = ctxRedtag.tb_Category.Join(ctxRedtag.tb_CategoryMapping, tb_Category => tb_Category.CategoryID, tb_CategoryMapping => tb_CategoryMapping.CategoryID, (tb_Category, tb_CategoryMapping) => new { tb_Category.CategoryID, tb_CategoryMapping.ParentCategoryID, tb_Category.Name, tb_Category.Active, tb_Category.Deleted, tb_Category.tb_Store.StoreID }).Where(tb_CategoryMapping => tb_CategoryMapping.ParentCategoryID == 0).Where(de => ((System.Boolean?)de.Deleted ?? false) == false && de.StoreID == StoreID).ToList();

            if (resultVar.Count > 0)
            {
                foreach (var catName in resultVar)
                {
                    if (Request.QueryString["ID"] != null && catName.CategoryID != Convert.ToInt32(Request.QueryString["ID"]))
                    {
                        TreeNode node = new TreeNode(catName.Name.ToString(), catName.CategoryID.ToString());
                        node.NavigateUrl = "javascript:void(0);";
                        node.ShowCheckBox = true;

                        if (strNodeExpand.Contains("," + catName.CategoryID.ToString() + ","))
                        {
                            if (strParentNodeExpand.Contains("," + catName.CategoryID.ToString() + ","))
                            {
                                node.Checked = true;
                            }
                            node.Expanded = true;
                        }
                        else
                        {
                            node.Checked = false;
                            node.Collapse();
                        }

                        //node.Collapse();
                        tvCategory.Nodes.Add(node);
                        childnode(Convert.ToInt32(catName.CategoryID.ToString().Trim()), node);
                    }
                    else if (Request.QueryString["ID"] == null)
                    {
                        TreeNode node = new TreeNode(catName.Name.ToString(), catName.CategoryID.ToString());
                        node.NavigateUrl = "javascript:void(0);";
                        node.ShowCheckBox = true;
                        node.Collapse();
                        tvCategory.Nodes.Add(node);
                        childnode(Convert.ToInt32(catName.CategoryID.ToString().Trim()), node);
                    }
                }
            }

            if (Request.QueryString["Root"] != null && Convert.ToString(Request.QueryString["Root"]) == "1")
            {
                tvCategory.Nodes[0].Checked = true;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "ChkRootNode();", true);
            }
            if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null && Request.QueryString["StoreID"] != null && Request.QueryString["Mode"] == "edit")
            {
                if (strNodeExpand.Length == 1)
                {
                    tvCategory.Nodes[0].Checked = true;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "ChkRootNode();", true);
                }
            }
        }

        /// <summary>
        /// Gets the Parent Category for Search
        /// </summary>
        /// <param name="CatName">string CatName</param>
        private void GetParentCategoryForSearch(String CatName)
        {
            DataSet dsSearchCategory = new DataSet();
            dsSearchCategory = CategoryComponent.SearchCategory(CatName, 3, Convert.ToInt32(ddlStore.SelectedValue));
            if (dsSearchCategory != null && dsSearchCategory.Tables.Count > 0 && dsSearchCategory.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsSearchCategory.Tables[0].Rows.Count; i++)
                {
                    if (!strSearchedCategory.Contains("," + dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ","))
                    {
                        strSearchedCategory += dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ",";
                    }
                    if (Convert.ToInt32(dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"]) != 0)
                    {
                        if (!strSearchedCategory.Contains("," + dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString() + ","))
                        {
                            strSearchedCategory += dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString() + ",";
                        }
                        GetParentCategoryForSearch(Convert.ToString(dsSearchCategory.Tables[0].Rows[i]["ParentcategoyName"]));
                    }
                }
            }
        }

        /// <summary>
        /// Function for Count the Product of a Category for Display in Tree view
        /// </summary>
        /// <param name="CategoryId">int CategoryId</param>
        /// <returns>Returns the counted the product of a category for display in Tree view</returns>
        public Int32 ProductCount(Int32 CategoryId)
        { 
            DataSet dsCategory = new DataSet();
            dsCategory = CommonComponent.GetCommonDataSet("EXEC usp_CategoryProductCount_list " + CategoryId + ", 0");
            Int32 PCount = 0;
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                PCount = Convert.ToInt32(dsCategory.Tables[0].Rows[0]["Total"].ToString());
            }

            /*
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                DataSet dsParent = new DataSet();

                dsParent = CommonComponent.GetCommonDataSet("SELECT count(Productid) as tt FROM dbo.tb_Product WHERE ProductID IN(SELECT ProductID FROM dbo.tb_ProductCategory WHERE CategoryID=" + CategoryId + ") AND ISNULL(Active ,0)=1 AND ISNULL(Deleted,0)=0");
                if (dsParent != null && dsParent.Tables.Count > 0 && dsParent.Tables[0].Rows.Count > 0)
                {
                    PCount = Convert.ToInt32(dsParent.Tables[0].Rows[0]["tt"].ToString());
                }
                Int32 catid = 0;
                Int32 parentcategid = 0;
                for (int i = 0; i < dsCategory.Tables[0].Rows.Count; i++)
                {
                    if (catid != Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()) || parentcategid != Convert.ToInt32(dsCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString()))
                    {

                        PCount = LoadCategoryChild(Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()), PCount);

                    }
                    catid = Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString());
                    parentcategid = Convert.ToInt32(dsCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString());
                }
            }
            else
            {
                dsCategory = CommonComponent.GetCommonDataSet("SELECT count(Productid) as tt FROM dbo.tb_Product WHERE ProductID IN(SELECT ProductID FROM dbo.tb_ProductCategory WHERE CategoryID=" + CategoryId + ") AND ISNULL(Active ,0)=1 AND ISNULL(Deleted,0)=0");
                if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                {
                    PCount = Convert.ToInt32(dsCategory.Tables[0].Rows[0]["tt"].ToString());
                }
            }*/
            return PCount;

        }

        /// <summary>
        /// Loads the Child Category
        /// </summary>
        /// <param name="CategoryId">int CategoryId</param>
        /// <param name="count">int count</param>
        /// <returns>Returns the Category count</returns>
        public Int32 LoadCategoryChild(Int32 CategoryId, Int32 count)
        {

            DataSet dsCategory = new DataSet();
            dsCategory = CommonComponent.GetCommonDataSet("EXEC usp_CategoryProductCount " + CategoryId + ", 0");

            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                DataSet dsParent = new DataSet();
                dsParent = CommonComponent.GetCommonDataSet("SELECT count(Productid) as tt FROM dbo.tb_Product WHERE ProductID IN(SELECT ProductID FROM dbo.tb_ProductCategory WHERE CategoryID=" + CategoryId + ") AND ISNULL(Active ,0)=1 AND ISNULL(Deleted,0)=0");
                if (dsParent != null && dsParent.Tables.Count > 0 && dsParent.Tables[0].Rows.Count > 0)
                {
                    count += Convert.ToInt32(dsParent.Tables[0].Rows[0]["tt"].ToString());
                }
                Int32 catid = 0;
                Int32 parentcategid = 0;
                for (int i = 0; i < dsCategory.Tables[0].Rows.Count; i++)
                {
                    if (catid != Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()) || parentcategid != Convert.ToInt32(dsCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString()))
                    {


                        count = LoadCategoryChild(Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString()), count);

                    }
                    catid = Convert.ToInt32(dsCategory.Tables[0].Rows[i]["CategoryID"].ToString());
                    parentcategid = Convert.ToInt32(dsCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString());
                }

            }
            else
            {
                dsCategory = CommonComponent.GetCommonDataSet("SELECT count(Productid) as tt FROM dbo.tb_Product WHERE ProductID IN(SELECT ProductID FROM dbo.tb_ProductCategory WHERE CategoryID=" + CategoryId + ") AND ISNULL(Active ,0)=1 AND ISNULL(Deleted,0)=0");
                if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
                {
                    count += Convert.ToInt32(dsCategory.Tables[0].Rows[0]["tt"].ToString());
                }

            }
            return count;
        }

        /// <summary>
        /// Binds the Parent Value
        /// </summary>
        private void bindParentValue()
        {
            sbCatMap = new System.Text.StringBuilder();

            strNodeExpand = ",";
            strParentNodeExpand = ",";
            DataSet dsSearchCategory = new DataSet();
            dsSearchCategory = CategoryComponent.ExpandedCategory(Convert.ToInt32(Request.QueryString["ID"]), 2, Convert.ToInt32(ddlStore.SelectedValue));

            if (dsSearchCategory != null && dsSearchCategory.Tables.Count > 0 && dsSearchCategory.Tables[0].Rows.Count > 0)
            {
                for (int j = 0; j < dsSearchCategory.Tables[0].Rows.Count; j++)
                {
                    if (!strParentNodeExpand.Contains("," + dsSearchCategory.Tables[0].Rows[j]["ParentCategoryID"].ToString() + ","))
                    {
                        strParentNodeExpand += dsSearchCategory.Tables[0].Rows[j]["ParentCategoryID"].ToString() + ",";

                    }
                }

                for (int i = 0; i < dsSearchCategory.Tables[0].Rows.Count; i++)
                {
                    if (Convert.ToInt32(dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"]) != 0)
                    {
                        if (!strNodeExpand.Contains("," + dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString() + ","))
                        {
                            strNodeExpand += dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString() + ",";
                        }
                        parsenode(Convert.ToInt32(dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"]));
                    }
                }
            }

            if (Request.QueryString["ID"] != null)
            {
                DataSet dsParentCategory = CategoryComponent.GetCategoryDetailBycategoryIdWithParentID(Convert.ToInt32(Request.QueryString["ID"]));
                if (dsParentCategory != null && dsParentCategory.Tables.Count > 0 && dsParentCategory.Tables[0].Rows.Count > 0)
                {
                    int pCount = dsParentCategory.Tables[0].Rows.Count;
                    for (int i = 0; i < pCount; i++)
                    {
                        sbCatMap.Append(dsParentCategory.Tables[0].Rows[i]["CategoryMappingID"].ToString() + ",");
                    }
                }
            }
        }

        /// <summary>
        /// Parses the node by CategoryID
        /// </summary>
        /// <param name="CatID">The cat ID.</param>
        private void parsenode(Int32 CatID)
        {
            DataSet dsSearchCategory = new DataSet();
            dsSearchCategory = CategoryComponent.ExpandedCategory(CatID, 2, Convert.ToInt32(ddlStore.SelectedValue));
            if (dsSearchCategory != null && dsSearchCategory.Tables.Count > 0 && dsSearchCategory.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsSearchCategory.Tables[0].Rows.Count; i++)
                {
                    if (!strNodeExpand.Contains("," + dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ","))
                    {
                        strNodeExpand += dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ",";
                    }
                    if (Convert.ToInt32(dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"]) != 0)
                    {
                        if (!strNodeExpand.Contains("," + dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString() + ","))
                        {
                            strNodeExpand += dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString() + ",";
                        }
                        parsenode(Convert.ToInt32(dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"]));
                    }
                }
            }
        }

        /// <summary>
        /// Method for bind child node in treeview by parent node
        /// </summary>
        /// <param name="catId">int catId</param>
        /// <param name="node">TreeNode node</param>
        private void childnode(int catId, TreeNode node)
        {
            int StoreID = Convert.ToInt32(ddlStore.SelectedValue);
            var resultVar = ctxRedtag.tb_Category.Join(ctxRedtag.tb_CategoryMapping, tb_Category => tb_Category.CategoryID, tb_CategoryMapping => tb_CategoryMapping.CategoryID, (tb_Category, tb_CategoryMapping) => new { tb_Category.CategoryID, tb_CategoryMapping.ParentCategoryID, tb_Category.Name, tb_Category.Active, tb_Category.Deleted, tb_Category.tb_Store.StoreID }).Where(tb_CategoryMapping => tb_CategoryMapping.ParentCategoryID == catId).Where(de => ((System.Boolean?)de.Deleted ?? false) == false && de.StoreID == StoreID).ToList();
            if (resultVar.Count > 0)
            {
                foreach (var subCatname in resultVar)
                {
                    if (Request.QueryString["ID"] != null && subCatname.CategoryID != Convert.ToInt32(Request.QueryString["ID"]))
                    {
                        TreeNode tnchild = new TreeNode();
                        tnchild.NavigateUrl = "javascript:void(0);";
                        tnchild.Value = subCatname.CategoryID.ToString();
                        tnchild.ShowCheckBox = true;
                        tnchild.Text = subCatname.Name.ToString();
                        tnchild.ToolTip = subCatname.Name.ToString();

                        if (strNodeExpand.Contains("," + subCatname.CategoryID.ToString() + ","))
                        {
                            if (strParentNodeExpand.Contains("," + subCatname.CategoryID.ToString() + ","))
                            {
                                tnchild.Checked = true;
                            }
                            tnchild.Expanded = true;
                        }
                        else
                        {
                            tnchild.Checked = false;
                            tnchild.Collapse();
                        }

                        node.ChildNodes.Add(tnchild);
                        childnode(Convert.ToInt32(subCatname.CategoryID.ToString().Trim()), tnchild);
                    }
                    else if (Request.QueryString["ID"] == null)
                    {
                        TreeNode tnchild = new TreeNode();
                        tnchild.NavigateUrl = "javascript:void(0);";
                        tnchild.Value = subCatname.CategoryID.ToString();
                        tnchild.ShowCheckBox = true;
                        tnchild.Text = subCatname.Name.ToString();
                        tnchild.ToolTip = subCatname.Name.ToString();
                        node.ChildNodes.Add(tnchild);
                        childnode(Convert.ToInt32(subCatname.CategoryID.ToString().Trim()), tnchild);
                    }
                }
            }
        }

        /// <summary>
        /// Fill Master Category list (Bind 'tvMasterCategoryList' Treeview with Category and subcategory Details.)
        /// </summary>
        private void FillMasterCategorylist()
        {
            tvMasterCategoryList.Nodes.Clear();
            //TreeNode rootnode = new TreeNode();
            //rootnode.Text = "Root Category";
            //rootnode.Value = "0";


            //tvMasterCategoryList.Nodes.Add(rootnode);


            #region For Search Category

            if (ViewState["SearchEnabled"] != null && Convert.ToString(ViewState["SearchEnabled"]) == "1" && txtSearch.Text.Trim() != "")
            {
                DataSet dsSearchCategory = new DataSet();
                if (ddlSearch.SelectedIndex == 1)
                {
                    dsSearchCategory = CategoryComponent.SearchCategory(txtSearch.Text.Trim(), 1, Convert.ToInt32(ddlStore.SelectedValue));
                    if (dsSearchCategory != null && dsSearchCategory.Tables.Count > 0 && dsSearchCategory.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsSearchCategory.Tables[0].Rows.Count; i++)
                        {
                            //strSearchedCategory += dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ",";
                            if (!strSearchedCategory.Contains("," + dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ","))
                            {
                                strSearchedCategory += dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ",";
                            }
                        }
                    }
                }
                else if (ddlSearch.SelectedIndex == 0)
                {
                    dsSearchCategory = CategoryComponent.SearchCategory(txtSearch.Text.Trim(), 2, Convert.ToInt32(ddlStore.SelectedValue));

                    if (dsSearchCategory != null && dsSearchCategory.Tables.Count > 0 && dsSearchCategory.Tables[0].Rows.Count > 0)
                    {
                        for (int i = 0; i < dsSearchCategory.Tables[0].Rows.Count; i++)
                        {
                            //strSearchedCategory += dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ",";
                            if (!strSearchedCategory.Contains("," + dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ","))
                            {
                                strSearchedCategory += dsSearchCategory.Tables[0].Rows[i]["CategoryID"].ToString() + ",";
                            }

                            if (Convert.ToInt32(dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"]) != 0)
                            {
                                if (!strSearchedCategory.Contains("," + dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString() + ","))
                                {
                                    strSearchedCategory += dsSearchCategory.Tables[0].Rows[i]["ParentCategoryID"].ToString() + ",";
                                }
                                GetParentCategoryForSearch(Convert.ToString(dsSearchCategory.Tables[0].Rows[i]["ParentcategoyName"]));
                            }
                        }
                    }
                }
            }

            #endregion



            //String rdlStatus = "";
            //for (int rd = 0; rd < rdlCategoryStatus.Items.Count; rd++)
            //{
            //    if (rdlCategoryStatus.Items[rd].Selected)
            //    {
            //        rdlStatus = rdlCategoryStatus.Items[rd].Value.Trim();
            //    }
            //}

            //if (rdlStatus == "InActive")
            //{
            //    IsActive = false;
            //}

            int StoreID = Convert.ToInt32(ddlStore.SelectedValue);
            var resultVar = ctxRedtag.tb_Category.Join(ctxRedtag.tb_CategoryMapping, tb_Category => tb_Category.CategoryID, tb_CategoryMapping => tb_CategoryMapping.CategoryID, (tb_Category, tb_CategoryMapping) => new { tb_Category.CategoryID, tb_CategoryMapping.ParentCategoryID, tb_Category.tb_Store.StoreID, tb_Category.Name, tb_Category.Active, tb_Category.Deleted, tb_Category.DisplayOrder }).Where(tb_CategoryMapping => tb_CategoryMapping.ParentCategoryID == 0).Where(de => ((System.Boolean?)de.Deleted ?? false) == false && de.StoreID == StoreID).OrderBy(a => a.DisplayOrder).ToList();

            if (rdlCategoryStatus.SelectedIndex != 0)
            {
                if (rdlCategoryStatus.SelectedIndex == 1)
                {
                    resultVar = resultVar.Where(ac => ((System.Boolean?)ac.Active ?? false) == true).ToList();
                }
                else
                {
                    resultVar = resultVar.Where(ac => ((System.Boolean?)ac.Active ?? false) == false).ToList();
                }

            }

            if (resultVar.Count > 0)
            {
                if (ViewState["SearchEnabled"] != null && Convert.ToString(ViewState["SearchEnabled"]) == "1" && txtSearch.Text.Trim() != "")
                {
                    foreach (var catName in resultVar)
                    {
                        if (strSearchedCategory.Contains("," + catName.CategoryID.ToString() + ","))
                        {
                            nodecount++;
                            string Catename = "";
                            Int32 Pc = ProductCount(catName.CategoryID);
                            if (Pc > 0)
                            {
                                Catename = catName.Name.ToString() + "(" + Pc + ")";
                            }
                            else
                            {
                                Catename = catName.Name.ToString() + "(" + Pc + ")";
                                //Catename = catName.Name.ToString();
                            }
                            TreeNode node = new TreeNode(Catename, catName.CategoryID.ToString());
                            node.ToolTip = catName.Name.ToString();
                            if (txtSearch.Text.ToString().Trim() != "")
                            {
                                node.NavigateUrl = "/Admin/Products/Category.aspx?Node=" + nodecount.ToString() + "&ActiveStatus=" + rdlCategoryStatus.SelectedIndex.ToString() + "&Search=" + Server.UrlEncode(txtSearch.Text.ToString().Trim()) + "&Mode=edit&ID=" + catName.CategoryID.ToString() + "&storeid=" + catName.StoreID;
                            }
                            else
                            {
                                node.NavigateUrl = "/Admin/Products/Category.aspx?Node=" + nodecount.ToString() + "&ActiveStatus=" + rdlCategoryStatus.SelectedIndex.ToString() + "&Mode=edit&ID=" + catName.CategoryID.ToString() + "&storeid=" + catName.StoreID;
                            }
                            node.ImageUrl = "/Admin/Images/tree-folder.gif";

                            if (strNodeExpand.Contains("," + catName.CategoryID.ToString() + ","))
                            {
                                node.Expanded = true;
                                node.Selected = true;
                                node.SelectAction = TreeNodeSelectAction.SelectExpand;

                            }
                            else
                            {
                                node.Checked = false;
                                node.Selected = false;
                                node.Collapse();
                            }

                            //node.Collapse();

                            tvMasterCategoryList.Nodes.Add(node);
                            BindchildnodeForMaster(Convert.ToInt32(catName.CategoryID.ToString().Trim()), node);
                        }
                    }
                }
                else
                {
                    foreach (var catName in resultVar)
                    {
                        nodecount++;
                        string Catename = "";
                        Int32 Pc = ProductCount(catName.CategoryID);
                        if (Pc > 0)
                        {
                            Catename = catName.Name.ToString() + "(" + Pc + ")";
                        }
                        else
                        {
                            Catename = catName.Name.ToString() + "(" + Pc + ")";
                            //Catename = catName.Name.ToString();
                        }

                        TreeNode node = new TreeNode(Catename, catName.CategoryID.ToString());
                        node.ToolTip = catName.Name.ToString();
                        if (txtSearch.Text.ToString().Trim() != "")
                        {
                            node.NavigateUrl = "/Admin/Products/Category.aspx?Node=" + nodecount.ToString() + "&ActiveStatus=" + rdlCategoryStatus.SelectedIndex.ToString() + "&Search=" + Server.UrlEncode(txtSearch.Text.ToString().Trim()) + "&Mode=edit&ID=" + catName.CategoryID.ToString() + "&storeid=" + catName.StoreID;
                        }
                        else
                        {
                            node.NavigateUrl = "/Admin/Products/Category.aspx?Node=" + nodecount.ToString() + "&ActiveStatus=" + rdlCategoryStatus.SelectedIndex.ToString() + "&Mode=edit&ID=" + catName.CategoryID.ToString() + "&storeid=" + catName.StoreID;
                        }
                        node.ImageUrl = "/Admin/Images/tree-folder.gif";

                        if (strNodeExpand.Contains("," + catName.CategoryID.ToString() + ","))
                        {
                            node.Expanded = true;
                            node.Selected = true;
                            node.SelectAction = TreeNodeSelectAction.SelectExpand;
                        }
                        else
                        {
                            node.Checked = false;
                            node.Selected = false;
                            node.Collapse();
                        }
                        //node.Collapse();

                        tvMasterCategoryList.Nodes.Add(node);
                        BindchildnodeForMaster(Convert.ToInt32(catName.CategoryID.ToString().Trim()), node);
                    }
                }
            }

            if (tvMasterCategoryList.Nodes.Count == 0)
            {
                trCollexpand.Visible = false;
            }
            else
            {
                trCollexpand.Visible = true;
            }
        }

        /// <summary>
        /// Bind child node For Master
        /// </summary>
        /// <param name="catId">int catId</param>
        /// <param name="node">TreeNode node</param>
        private void BindchildnodeForMaster(int catId, TreeNode node)
        {


            int StoreID = Convert.ToInt32(ddlStore.SelectedValue);
            var resultVar = ctxRedtag.tb_Category.Join(ctxRedtag.tb_CategoryMapping, tb_Category => tb_Category.CategoryID, tb_CategoryMapping => tb_CategoryMapping.CategoryID, (tb_Category, tb_CategoryMapping) => new { tb_Category.CategoryID, tb_CategoryMapping.ParentCategoryID, tb_Category.tb_Store.StoreID, tb_Category.Name, tb_Category.Active, tb_Category.Deleted }).Where(tb_CategoryMapping => tb_CategoryMapping.ParentCategoryID == catId).Where(de => ((System.Boolean?)de.Deleted ?? false) == false && de.StoreID == StoreID).ToList();

            //if (rdlCategoryStatus.SelectedIndex != 0)
            //{
            //    if (rdlCategoryStatus.SelectedIndex == 1)
            //    {
            //        resultVar = resultVar.Where(ac => ((System.Boolean?)ac.Active ?? false) == true).ToList();
            //    }
            //    else
            //    {
            //        resultVar = resultVar.Where(ac => ((System.Boolean?)ac.Active ?? false) == false).ToList();
            //    }

            //}

            if (resultVar.Count > 0)
            {
                if (ViewState["SearchEnabled"] != null && Convert.ToString(ViewState["SearchEnabled"]) == "1" && txtSearch.Text.Trim() != "" && ddlSearch.SelectedIndex == 0)
                {
                    foreach (var subCatname in resultVar)
                    {
                        if (strSearchedCategory.Contains("," + subCatname.CategoryID + ","))
                        {
                            nodecount++;
                            TreeNode tnchild = new TreeNode();
                            if (txtSearch.Text.ToString().Trim() != "")
                            {
                                tnchild.NavigateUrl = "/Admin/Products/Category.aspx?Node=" + nodecount.ToString() + "&ActiveStatus=" + rdlCategoryStatus.SelectedIndex.ToString() + "&Search=" + Server.UrlEncode(txtSearch.Text.ToString().Trim()) + "&Mode=edit&ID=" + subCatname.CategoryID.ToString() + "&storeid=" + subCatname.StoreID;
                            }
                            else
                            {

                                tnchild.NavigateUrl = "/Admin/Products/Category.aspx?Node=" + nodecount.ToString() + "&ActiveStatus=" + rdlCategoryStatus.SelectedIndex.ToString() + "&Mode=edit&ID=" + subCatname.CategoryID.ToString() + "&storeid=" + subCatname.StoreID;
                            }
                            tnchild.Value = subCatname.CategoryID.ToString();

                            string Catename = "";
                            Int32 Pc = ProductCount(subCatname.CategoryID);
                            if (Pc > 0)
                            {
                                Catename = subCatname.Name.ToString() + "(" + Pc + ")";
                            }
                            else
                            {
                                //Catename = subCatname.Name.ToString();
                                Catename = subCatname.Name.ToString() + "(" + Pc + ")";
                            }

                            tnchild.Text = Catename;
                            tnchild.ToolTip = subCatname.Name.ToString();
                            tnchild.ImageUrl = "/Admin/Images/tree-folder.gif";

                            if (strNodeExpand.Contains("," + subCatname.CategoryID.ToString() + ","))
                            {
                                tnchild.Expanded = true;
                                tnchild.Selected = true;
                                tnchild.SelectAction = TreeNodeSelectAction.SelectExpand;
                            }
                            else
                            {
                                tnchild.Checked = false;
                                tnchild.Selected = false;
                                tnchild.Collapse();
                            }

                            //tnchild.Collapse();

                            node.ChildNodes.Add(tnchild);
                            BindchildnodeForMaster(Convert.ToInt32(subCatname.CategoryID.ToString().Trim()), tnchild);
                        }
                    }
                }
                else
                {
                    foreach (var subCatname in resultVar)
                    {
                        nodecount++;
                        TreeNode tnchild = new TreeNode();
                        if (txtSearch.Text.ToString().Trim() != "")
                        {
                            tnchild.NavigateUrl = "/Admin/Products/Category.aspx?Node=" + nodecount.ToString() + "&ActiveStatus=" + rdlCategoryStatus.SelectedIndex.ToString() + "&Search=" + Server.UrlEncode(txtSearch.Text.ToString().Trim()) + "&Mode=edit&ID=" + subCatname.CategoryID.ToString() + "&storeid=" + subCatname.StoreID;
                        }
                        else
                        {
                            tnchild.NavigateUrl = "/Admin/Products/Category.aspx?Node=" + nodecount.ToString() + "&ActiveStatus=" + rdlCategoryStatus.SelectedIndex.ToString() + "&Mode=edit&ID=" + subCatname.CategoryID.ToString() + "&storeid=" + subCatname.StoreID;
                        }
                        tnchild.Value = subCatname.CategoryID.ToString();
                        string Catename = "";
                        Int32 Pc = ProductCount(subCatname.CategoryID);
                        if (Pc > 0)
                        {
                            Catename = subCatname.Name.ToString() + "(" + Pc + ")";
                        }
                        else
                        {
                            Catename = subCatname.Name.ToString() + "(" + Pc + ")";
                            //Catename = subCatname.Name.ToString();
                        }

                        tnchild.Text = Catename;
                        tnchild.ToolTip = subCatname.Name.ToString();
                        tnchild.ImageUrl = "/Admin/Images/tree-folder.gif";
                        if (strNodeExpand.Contains("," + subCatname.CategoryID.ToString() + ","))
                        {
                            tnchild.Expanded = true;
                            tnchild.Selected = true;
                            tnchild.SelectAction = TreeNodeSelectAction.SelectExpand;

                        }
                        else
                        {
                            tnchild.Checked = false;
                            tnchild.Selected = false;
                            tnchild.Collapse();
                        }

                        //tnchild.Collapse();

                        //if(Request.QueryString["Id"] != null)
                        //{
                        //if(subCatname.CategoryID == 
                        //}
                        node.ChildNodes.Add(tnchild);
                        BindchildnodeForMaster(Convert.ToInt32(subCatname.CategoryID.ToString().Trim()), tnchild);
                    }
                }

            }
        }

        /// <summary>
        /// Bind Store Details with dropdown
        /// </summary>
        private void bindstore()
        {
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            // ddlStore.Items.Insert(0, "- Select Store -");

            try
            {
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                {
                    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                }
            }
            catch
            {
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Get All the Categoryid which are Selected by the User
        /// </summary>
        /// <returns>string</returns>
        public string GetSelectedParents()
        {
            String SelectedParents = string.Empty;
            //if (("," + AppLogic.AppConfig("RootCategoryIDs") + ",").Contains("," + categoryID + ","))
            //    return ",0";
            //Int32.TryParse(ddlStore.SelectedValue, out storeID);
            foreach (TreeNode tn in tvCategory.Nodes)
            {
                if (tn.Checked == true)
                {
                    SelectedParents += "," + tn.Value;
                }
                SelectedParents = GetCategoryIDList(SelectedParents, tn);
            }

            if (SelectedParents.Length > 0 && SelectedParents.ToString().Substring(0, 1) == ",")
                SelectedParents = SelectedParents.Substring(1);

            return SelectedParents;
        }

        /// <summary>
        /// Get all the subcategoryid which are selected by the user under the parent category
        /// </summary>
        /// <param name="SelectedParents">SeltedCatetgoryId string</param>
        /// <param name="tn">Tree node for which the we are looking the subcategory</param>
        /// <returns>Returns the CategoryIDs as a String</returns>
        public String GetCategoryIDList(String SelectedParents, TreeNode tn)
        {
            foreach (TreeNode tchild in tn.ChildNodes)
            {
                if (tchild.Checked == true)
                {
                    SelectedParents += "," + tchild.Value;
                }
                SelectedParents = GetCategoryIDList(SelectedParents, tchild);
            }
            return SelectedParents;
        }

        /// <summary>
        /// Method for Save Data into category
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgbtnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (chkShowOnHeader.Checked && string.IsNullOrEmpty(txtShortTitle.Text.ToString().Trim()))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter Short Title.', 'Message','ContentPlaceHolder1_txtShortTitle');});", true);
                return;
            }

            if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null && Request.QueryString["StoreID"] != null)
            {
                updateCategory();
                //Response.Redirect("categoryList.aspx?mode=edit");
                Response.Redirect("/Admin/Products/Category.aspx?mode=Updated");
            }
            else
            {
                #region Check Name is Exist or Not
                if (txtname.Text != "")
                {
                    DataSet dsCatname = objCategorycomponent.getcategorydetailsbyname(txtname.Text.Trim(), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                    if (dsCatname != null && dsCatname.Tables.Count > 0 && dsCatname.Tables[0].Rows.Count > 0)
                    {
                        lblMsg.Text = "This Category Name Already Exists!";
                    }
                    else
                    {
                        insertCategory();

                        Response.Redirect("/Admin/Products/Category.aspx?mode=Inserted");
                    }
                }
                #endregion
            }
        }

        /// <summary>
        /// Method to update Exists Category Details
        /// </summary>
        private void updateCategory()
        {
            ctxCategory = new tb_Category();

            ctxCategory.CategoryID = Convert.ToInt32(Request.QueryString["ID"]);
            ctxCategory.Name = txtname.Text.Trim();
            string sename = CommonOperations.RemoveSpecialCharacter(txtname.Text.Trim().ToLower().ToCharArray());
            ctxCategory.SEName = sename;
            ctxCategory.SEDescription = txtseodescription.Text.Trim();
            ctxCategory.SEKeywords = txtsekeyword.Text.Trim();
            ctxCategory.CreatedBy = Convert.ToInt32(Session["AdminID"]);
            ctxCategory.CreatedOn = System.DateTime.Now;
            ctxCategory.SETitle = txtsetitle.Text.Trim();
            ctxCategory.Summary = txtSummary.Text.ToString();
            ctxCategory.Description = txtDescription.Text.ToString();
            ctxCategory.Active = Convert.ToBoolean(rblPublished.SelectedValue);
            ctxCategory.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(ddlStore.SelectedValue));
            ctxCategory.ToolTip = "";//txttooltip.Text.Trim();
            ctxCategory.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
            ctxCategory.UpdatedOn = System.DateTime.Now;
            if (Convert.ToInt32(ddlproductfeature.SelectedValue) > 0)
            {
                ctxCategory.FeatureID = Convert.ToInt32(ddlproductfeature.SelectedValue);
            }
            else { ctxCategory.FeatureID = 0; }
            if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("sears") > -1)
            {
                lblSearsCategoryID.Text = "Sears Category ID";
                trSearsCatID.Visible = true;
                if (txtSearsCategoryID.Text != null && txtSearsCategoryID.Text.ToString().Trim() != "")
                    ctxCategory.SearsCategoryID = Convert.ToInt32(txtSearsCategoryID.Text);
                else
                    ctxCategory.SearsCategoryID = 0;
            }
            else if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1)
            {
                lblSearsCategoryID.Text = "Over Stock Category ID";
                trSearsCatID.Visible = true;
                if (txtSearsCategoryID.Text != null && txtSearsCategoryID.Text.ToString().Trim() != "")
                    ctxCategory.SearsCategoryID = Convert.ToInt32(txtSearsCategoryID.Text);
                else
                    ctxCategory.SearsCategoryID = 0;
            }


            if (ViewState["File"] != null)
            {
                //SaveImage(ViewState["File"].ToString(), "");
                //ctxCategory.ImageName = ViewState["File"].ToString();
                ctxCategory.ImageName = sename + "_" + Request.QueryString["ID"].ToString() + ".jpg";//ViewState["File"].ToString();
                SaveImage(ctxCategory.ImageName, "");
            }
            else
            {
                if (!imgIcon.Src.ToString().Contains("image_not_available"))
                {
                    //string iconImageName = sename + "_" + Request.QueryString["ID"].ToString() + ".jpg";
                    //SaveImage(iconImageName, "");
                    //ctxCategory.ImageName = iconImageName;
                    if (ViewState["ImageName"] != null)
                        ctxCategory.ImageName = Convert.ToString(ViewState["ImageName"]);
                }
            }
            if (ViewState["BannerFile"] != null)
            {
                //SaveImage(ViewState["BannerFile"].ToString(), "banner");
                try
                {
                    //File.Copy(Server.MapPath(imgBanner.Src.ToString()), Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + ViewState["BannerFile"].ToString(), true);
                    //CommonOperations.SaveOnContentServer(Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + ViewState["BannerFile"].ToString());
                    //File.Delete(Server.MapPath(imgBanner.Src.ToString()));
                    File.Copy(Server.MapPath(imgBanner.Src.ToString()), Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + Request.QueryString["ID"].ToString() + "_1.jpg", true);
                    CommonOperations.SaveOnContentServer(Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + Request.QueryString["ID"].ToString() + "_1.jpg");
                    File.Delete(Server.MapPath(imgBanner.Src.ToString()));
                    ctxCategory.BannerImageName = Request.QueryString["ID"].ToString() + "_1.jpg";//ViewState["BannerFile"].ToString();
                    //  SaveImage(ctxCategory.BannerImageName, "banner");

                }
                catch
                {

                }

            }
            else
            {
                if (!imgBanner.Src.ToString().Contains("image_not_available"))
                {
                    string bannerImageName = Request.QueryString["ID"].ToString() + "_1.jpg";
                    //SaveImage(bannerImageName, "banner");
                    ctxCategory.BannerImageName = bannerImageName;
                }
            }

            //bannerimage2

            if (ViewState["BannerFile1"] != null)
            {
                //SaveImage(ViewState["BannerFile"].ToString(), "banner");
                try
                {
                    //File.Copy(Server.MapPath(imgBanner.Src.ToString()), Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + ViewState["BannerFile"].ToString(), true);
                    //CommonOperations.SaveOnContentServer(Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + ViewState["BannerFile"].ToString());
                    //File.Delete(Server.MapPath(imgBanner.Src.ToString()));
                    File.Copy(Server.MapPath(imgBanner2.Src.ToString()), Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + Request.QueryString["ID"].ToString() + "_2.jpg", true);
                    CommonOperations.SaveOnContentServer(Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + Request.QueryString["ID"].ToString() + "_2.jpg");
                    File.Delete(Server.MapPath(imgBanner2.Src.ToString()));

                    CommonComponent.ExecuteCommonData("update tb_Category set BannerImageName1='" + Request.QueryString["ID"].ToString() + "_2.jpg" + "' where categoryid=" + Request.QueryString["ID"].ToString() + "");

                    //   ctxCategory.BannerImageName = Request.QueryString["ID"].ToString() + "_1.jpg";//ViewState["BannerFile"].ToString();
                    //  SaveImage(ctxCategory.BannerImageName, "banner");

                }
                catch
                {

                }

            }
            else
            {
                if (!imgBanner2.Src.ToString().Contains("image_not_available"))
                {
                    string bannerImageName = Request.QueryString["ID"].ToString() + "_2.jpg";
                    //SaveImage(bannerImageName, "banner");
                    CommonComponent.ExecuteCommonData("update tb_Category set BannerImageName1='" + Request.QueryString["ID"].ToString() + "_2.jpg" + "' where categoryid=" + Request.QueryString["ID"].ToString() + "");
                    // ctxCategory.BannerImageName = bannerImageName;
                }
            }

            //
            if (ViewState["BannerFile2"] != null)
            {
                //SaveImage(ViewState["BannerFile"].ToString(), "banner");
                try
                {
                    //File.Copy(Server.MapPath(imgBanner.Src.ToString()), Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + ViewState["BannerFile"].ToString(), true);
                    //CommonOperations.SaveOnContentServer(Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + ViewState["BannerFile"].ToString());
                    //File.Delete(Server.MapPath(imgBanner.Src.ToString()));
                    File.Copy(Server.MapPath(imgBanner3.Src.ToString()), Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + Request.QueryString["ID"].ToString() + "_3.jpg", true);
                    CommonOperations.SaveOnContentServer(Server.MapPath(AppLogic.AppConfigs("ImagePathCategory") + "banner/") + Request.QueryString["ID"].ToString() + "_3.jpg");
                    File.Delete(Server.MapPath(imgBanner3.Src.ToString()));

                    CommonComponent.ExecuteCommonData("update tb_Category set BannerImageName2='" + Request.QueryString["ID"].ToString() + "_3.jpg" + "' where categoryid=" + Request.QueryString["ID"].ToString() + "");

                    //   ctxCategory.BannerImageName = Request.QueryString["ID"].ToString() + "_1.jpg";//ViewState["BannerFile"].ToString();
                    //  SaveImage(ctxCategory.BannerImageName, "banner");

                }
                catch
                {

                }

            }
            else
            {
                if (!imgBanner3.Src.ToString().Contains("image_not_available"))
                {
                    string bannerImageName = Request.QueryString["ID"].ToString() + "_3.jpg";
                    //SaveImage(bannerImageName, "banner");
                    CommonComponent.ExecuteCommonData("update tb_Category set BannerImageName2='" + Request.QueryString["ID"].ToString() + "_3.jpg" + "' where categoryid=" + Request.QueryString["ID"].ToString() + "");
                    // ctxCategory.BannerImageName = bannerImageName;
                }
            }


            string effsename = "";
            if (!string.IsNullOrEmpty(txteffurl.Text.Trim()))
            {
                effsename = CommonOperations.RemoveSpecialCharacter(txteffurl.Text.Trim().ToLower().ToCharArray());

            }
            else
            {
                effsename = CommonOperations.RemoveSpecialCharacter(txteffname.Text.Trim().ToLower().ToCharArray());

            }

            Int32 SeoUrl = 0;
            if (chkcanonical.Checked)
            {
                SeoUrl = 1;
            }
             Int32 faceted = 0;
            if (chkfaceted.Checked)
            {
                faceted = 1;
            }

            CommonComponent.ExecuteCommonData("update tb_Category set Isnotfaceted=" + faceted + ",  Isseocanonical=" + SeoUrl + ", CanonicalUrl='" + txtcanonical.Text.ToString().Replace("'", "''") + "', BannerText='" + txtbannertext.Text.ToString().Replace("'", "''").Trim() + "',ShowOnEff = '" + Convert.ToBoolean(rdoShowOnEff.SelectedValue) + "',EFFName='" + txteffname.Text.ToString().Trim().Replace("'", "''") + "',EFFSEName='" + effsename + "',EFFDescription='" + txtEFFDescription.Text.ToString().Trim().Replace("'", "''") + "',EFFSEDescription='" + txtEFFseodescription.Text.ToString().Trim().Replace("'", "''") + "',EFFSETitle='" + txtEFFsetitle.Text.ToString().Trim().Replace("'", "''") + "',EFFSEKeywords='" + txtEFFsekeyword.Text.ToString().Trim().Replace("'", "''") + "' where categoryid=" + Request.QueryString["ID"].ToString() + "");


            if (txtdisplayorder.Text != null && txtdisplayorder.Text != "")
                ctxCategory.DisplayOrder = Convert.ToInt32(txtdisplayorder.Text.Trim());
            ctxCategory.Deleted = false;

            if (rblIsfeatured.Checked == true)
                ctxCategory.IsFeatured = Convert.ToBoolean(rblIsfeatured.Checked);
            else ctxCategory.IsFeatured = false;

            ctxCategory.HeaderText = txtHeaderText.Text.ToString().Trim().Replace("'", "''");
            ctxCategory.ShortName = txtShortTitle.Text.ToString().Trim().Replace("'", "''");

            if (tvCategory.Nodes[0].Checked == true)
            {
                ctxCategory.ShowOnHeader = chkShowOnHeader.Checked;
            }

            try
            {
                CommonComponent.ExecuteCommonData("EXEC Guisetchangecategorysename " + ctxCategory.CategoryID + ",'" + ctxCategory.SEName.ToString() + "'");

            }
            catch
            {

            }

            objCategorycomponent.updateCategory(ctxCategory);

            //Update data for Best Seller SKUS
            if (txtBestSellerSKUs.Text.Trim() != "")
            {
                CommonComponent.ExecuteCommonData("exec UpdateBestSellerCategory '" + txtBestSellerSKUs.Text.Trim().Replace("'", "''") + "'," + Request.QueryString["ID"].ToString() + "," + Request.QueryString["StoreID"].ToString());
            }

            string ChkParent = GetSelectedParents();
            int j = 0;

            //Delete
            ConfigurationComponent.GetBreadCrum(Convert.ToInt32(Request.QueryString["ID"]), 0, Convert.ToInt32(ddlStore.SelectedValue), "Category", 2, 0);

            String[] arrParent = ChkParent.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            foreach (String strParent in arrParent)
            {
                ctxCategorymapping = new tb_CategoryMapping();
                parentValue = sbCatMap.ToString().Split(',');

                string[] pv = new string[parentValue.Length - 1];
                Array.Copy(parentValue, pv, parentValue.Length - 1);
                int count = pv.Count();
                int p = pv.Count();

                #region same mapping count
                if (arrParent.Count() == count)
                {
                    for (int k = j; j < arrParent.Count(); j++)
                    {
                        objCategorycomponent.Update(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(strParent), Convert.ToInt32(pv[j].ToString()));
                        ConfigurationComponent.GetBreadCrum(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(strParent), Convert.ToInt32(ddlStore.SelectedValue), "Category", 1, 1);
                        j += 1;
                        break;
                    }
                }
                #endregion

                #region if selected node greater than mapping count
                else if (arrParent.Count() > count)
                {
                    for (int k = j; j < arrParent.Count(); j++)
                    {
                        if (j < count)
                        {
                            objCategorycomponent.Update(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(strParent), Convert.ToInt32(pv[j].ToString()));
                            ConfigurationComponent.GetBreadCrum(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(strParent), Convert.ToInt32(ddlStore.SelectedValue), "Category", 1, 1);
                            j = k + 1;
                            break;
                        }
                        else
                        {
                            ctxCategorymapping.ParentCategoryID = Convert.ToInt32(strParent);
                            ctxCategorymapping.CategoryID = Convert.ToInt32(Request.QueryString["ID"]);
                            objCategorycomponent.CreateCategorymapping(ctxCategorymapping);
                            ConfigurationComponent.GetBreadCrum(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(strParent), Convert.ToInt32(ddlStore.SelectedValue), "Category", 1, 1);
                            break;
                        }
                    }

                }
                #endregion
                else if (arrParent.Count() < count)
                {
                    for (int k = j; j <= pv.Count(); j++)
                    {

                        if (j < arrParent.Count())
                        {
                            objCategorycomponent.Update(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(strParent), Convert.ToInt32(pv[j].ToString()));
                            ConfigurationComponent.GetBreadCrum(Convert.ToInt32(Request.QueryString["ID"]), Convert.ToInt32(strParent), Convert.ToInt32(ddlStore.SelectedValue), "Category", 1, 1);
                            j = k + 1;
                            if (j != arrParent.Count())
                                break;
                        }
                        else
                        {
                            objCategorycomponent.delete(Convert.ToInt32(pv[j - 1].ToString()));

                        }
                    }
                }


            }

        }

        /// <summary>
        /// Method for Save Category Data With  Mapping Details
        /// </summary>
        private void insertCategory()
        {
            //Save data of Category Detail
            ctxCategory = new tb_Category();
            String imagePath = string.Empty;
            String bannerPath = string.Empty;

            ctxCategory.Name = txtname.Text.Trim();
            string sename = CommonOperations.RemoveSpecialCharacter(txtname.Text.Trim().ToLower().ToCharArray());
            ctxCategory.SEName = sename;
            ctxCategory.SEDescription = txtseodescription.Text.Trim();
            ctxCategory.SEKeywords = txtsekeyword.Text.Trim();

            ctxCategory.CreatedOn = DateTime.Now;
            ctxCategory.SETitle = txtsetitle.Text.Trim();
            ctxCategory.Summary = txtSummary.Text.ToString();
            ctxCategory.Description = txtDescription.Text.ToString();
            ctxCategory.Active = Convert.ToBoolean(rblPublished.SelectedValue);
            if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("sears") > -1)
            {
                lblSearsCategoryID.Text = "Sears Category ID";
                trSearsCatID.Visible = true;
                if (txtSearsCategoryID.Text != null && txtSearsCategoryID.Text.ToString().Trim() != "")
                    ctxCategory.SearsCategoryID = Convert.ToInt32(txtSearsCategoryID.Text);
                else
                    ctxCategory.SearsCategoryID = 0;
            }
            else if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1)
            {
                lblSearsCategoryID.Text = "Over Stock Category ID";
                trSearsCatID.Visible = true;
                if (txtSearsCategoryID.Text != null && txtSearsCategoryID.Text.ToString().Trim() != "")
                    ctxCategory.SearsCategoryID = Convert.ToInt32(txtSearsCategoryID.Text);
                else
                    ctxCategory.SearsCategoryID = 0;
            }
            else
            {
                lblSearsCategoryID.Text = "";
                trSearsCatID.Visible = false;
            }

            if (Convert.ToInt32(ddlproductfeature.SelectedValue) > 0)
            {
                ctxCategory.FeatureID = Convert.ToInt32(ddlproductfeature.SelectedValue);
            }
            else { ctxCategory.FeatureID = 0; }
            ctxCategory.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(ddlStore.SelectedValue));
            ctxCategory.ToolTip = "";//txttooltip.Text.Trim();
            //if (ViewState["File"] != null)
            //{
            //    ctxCategory.ImageName = ViewState["File"].ToString();
            //    SaveImage(ViewState["File"].ToString(), "");
            //}
            if (ViewState["BannerFile"] != null)
            {
                ctxCategory.BannerImageName = ViewState["BannerFile"].ToString();
                SaveImage(ViewState["BannerFile"].ToString(), "banner");
            }
            if (txtdisplayorder.Text != null && txtdisplayorder.Text != "")
                ctxCategory.DisplayOrder = Convert.ToInt32(txtdisplayorder.Text.Trim());
            ctxCategory.Deleted = false;

            if (rblIsfeatured.Checked == true)
                ctxCategory.IsFeatured = Convert.ToBoolean(rblIsfeatured.Checked);
            else ctxCategory.IsFeatured = false;

            ctxCategory.HeaderText = txtHeaderText.Text.ToString().Trim().Replace("'", "''");
            ctxCategory.ShortName = txtShortTitle.Text.ToString().Trim().Replace("'", "''");

            if (tvCategory.Nodes[0].Checked == true)
            {
                ctxCategory.ShowOnHeader = chkShowOnHeader.Checked;
            }
            int NewCategoryID = 0;
            NewCategoryID = objCategorycomponent.CreateCategory(ctxCategory);

            //SaveImage(ViewState["File"].ToString());
            //SaveImage(ViewState["BannerFile"].ToString());

            //Save Data of Category Mapping

            //RedTag_CCTV_Ecomm_DBEntities redTagcontext = new RedTag_CCTV_Ecomm_DBEntities();
            //var result = from c in redTagcontext.tb_Category where c.Name == txtname.Text.Trim() select c.CategoryID;

            string catid = NewCategoryID.ToString();// result.First().ToString();

            if (ViewState["File"] != null)
            {
                ctxCategory.ImageName = sename + "_" + catid + ".jpg";
                SaveImage(ctxCategory.ImageName, "");
                CommonComponent.ExecuteCommonData("update tb_category set imagename='" + ctxCategory.ImageName + "' where categoryid=" + catid);
            }

            string effsename = "";
            if (!string.IsNullOrEmpty(txteffurl.Text.Trim()))
            {
                effsename = CommonOperations.RemoveSpecialCharacter(txteffurl.Text.Trim().ToLower().ToCharArray());

            }
            else
            {
                effsename = CommonOperations.RemoveSpecialCharacter(txteffname.Text.Trim().ToLower().ToCharArray());

            }
            Int32 SeoUrl = 0;
            if (chkcanonical.Checked)
            {
                SeoUrl = 1;
            }
             Int32 faceted = 0;
            if (chkfaceted.Checked)
            {
                faceted = 1;
            }


            CommonComponent.ExecuteCommonData("update tb_Category set Isnotfaceted=" + faceted + ", Isseocanonical=" + SeoUrl + ", CanonicalUrl='" + txtcanonical.Text.ToString().Replace("'", "''") + "', BannerText='" + txtbannertext.Text.ToString().Replace("'", "''").Trim() + "',ShowOnEff = '" + Convert.ToBoolean(rdoShowOnEff.SelectedValue) + "',EFFName='" + txteffname.Text.ToString().Trim().Replace("'", "''") + "',EFFSEName='" + effsename + "',EFFDescription='" + txtEFFDescription.Text.ToString().Trim().Replace("'", "''") + "',EFFSEDescription='" + txtEFFseodescription.Text.ToString().Trim().Replace("'", "''") + "',EFFSETitle='" + txtEFFsetitle.Text.ToString().Trim().Replace("'", "''") + "',EFFSEKeywords='" + txtEFFsekeyword.Text.ToString().Trim().Replace("'", "''") + "' where categoryid=" + catid + "");

            int parentId = 0;

            ConfigurationComponent.GetBreadCrum(Convert.ToInt32(Request.QueryString["ID"]), 0, Convert.ToInt32(ddlStore.SelectedValue), "Category", 2, 0);
            string ChkParent = GetSelectedParents();
            if (ChkParent == "")
            {
                if (catid != null)
                {
                    ctxCategorymapping = new tb_CategoryMapping();
                    ctxCategorymapping.CategoryID = Convert.ToInt32(catid);
                    ctxCategorymapping.ParentCategoryID = parentId;
                    objCategorycomponent.CreateCategorymapping(ctxCategorymapping);
                    ConfigurationComponent.GetBreadCrum(Convert.ToInt32(catid), parentId, Convert.ToInt32(ddlStore.SelectedValue), "Category", 1, 1);
                }
            }
            else
            {
                String[] arrParent = ChkParent.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                foreach (String strParent in arrParent)
                {
                    ctxCategorymapping = new tb_CategoryMapping();
                    ctxCategorymapping.CategoryID = Convert.ToInt32(catid);
                    ctxCategorymapping.ParentCategoryID = Convert.ToInt32(strParent);
                    objCategorycomponent.CreateCategorymapping(ctxCategorymapping);
                    ConfigurationComponent.GetBreadCrum(Convert.ToInt32(catid), parentId, Convert.ToInt32(ddlStore.SelectedValue), "Category", 1, 1);
                }
            }
            clearselection();
        }

        /// <summary>
        /// Saves the Category image.
        /// </summary>
        /// <param name="FileName">string FileName</param>
        /// <param name="type">string type.</param>
        protected void SaveImage(string FileName, string type)
        {
            CategoryTempPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Temp/");
            CategoryIconPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Icon/");
            CategoryMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Medium/");
            CategoryLargePath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Large/");
            CategoryMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Micro/");
            CategoryBannerPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Banner/");
            CategoryBannerTempPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Temp/Banner/");
            //create icon folder 
            //clsvariables.LoadAllPath();
            if (!Directory.Exists(Server.MapPath(CategoryIconPath)))
                Directory.CreateDirectory(Server.MapPath(CategoryIconPath));

            //create Medium folder 
            if (!Directory.Exists(Server.MapPath(CategoryMediumPath)))
                Directory.CreateDirectory(Server.MapPath(CategoryMediumPath));

            //create Large folder 
            if (!Directory.Exists(Server.MapPath(CategoryLargePath)))
                Directory.CreateDirectory(Server.MapPath(CategoryLargePath));

            //create Banner folder 
            if (!Directory.Exists(Server.MapPath(CategoryBannerPath)))
                Directory.CreateDirectory(Server.MapPath(CategoryBannerPath));

            //create Micro folder 
            if (!Directory.Exists(Server.MapPath(CategoryMicroPath)))
                Directory.CreateDirectory(Server.MapPath(CategoryMicroPath));

            //CommonOperations.CreateFolder(Server.MapPath(clsvariables.PathCIconImage().ToString()));
            //CommonOperations.CreateFolder(Server.MapPath(clsvariables.PathCMediumImage().ToString()));
            //CommonOperations.CreateFolder(Server.MapPath(clsvariables.PathCLargeImage().ToString()));
            //CommonOperations.CreateFolder(Server.MapPath(clsvariables.PathCMicroImage().ToString()));
            //CommonOperations.CreateFolder(Server.MapPath(clsvariables.PathCTempBanner().ToString()));
            CommonOperations.SaveOnContentServer(Server.MapPath(CategoryIconPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(CategoryMediumPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(CategoryLargePath));
            CommonOperations.SaveOnContentServer(Server.MapPath(CategoryMicroPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(CategoryBannerPath));

            //CommonOperations.CreateFolder(Server.MapPath(clsvariables.PathCBannerImage().ToString()));
            //CommonOperations.SaveOnContentServer(Server.MapPath(clsvariables.PathCBannerImage().ToString()));
            if (type != "banner" && type == "")
            {
                if (imgIcon.Src.Contains(CategoryTempPath))
                {
                    try
                    {
                        CreateImage("Icon", FileName);
                        CreateImage("Micro", FileName);
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text += "<br />" + ex.Message;
                    }
                    finally
                    {
                        DeleteTempFile("icon");
                        DeleteTempFile("micro");
                    }
                }
            }
            if (type == "banner")
            {
                if (imgBanner.Src.Contains(CategoryBannerTempPath))
                {
                    try
                    {
                        CreateImage("banner", FileName);
                    }
                    catch (Exception ex)
                    {
                        lblMsg.Text += "<br />" + ex.Message;
                    }
                    finally
                    {
                        DeleteTempFile("banner");
                    }
                }
            }
        }

        /// <summary>
        /// Deletes the Temporary File
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
                        path = Server.MapPath(CategoryTempPath + ViewState["File"].ToString());
                    }

                    File.Delete(path);
                }
                if (strsize == "micro")
                {
                    string path = string.Empty;
                    if (ViewState["File"] != null && ViewState["File"].ToString().Trim().Length > 0)
                    {
                        path = Server.MapPath(CategoryTempPath + ViewState["File"].ToString());
                    }

                    File.Delete(path);
                }
                else if (strsize == "banner")
                {
                    string path = string.Empty;
                    if (ViewState["BannerFile"] != null && ViewState["BannerFile"].ToString().Trim().Length > 0)
                    {
                        path = Server.MapPath(CategoryBannerTempPath + ViewState["BannerFile"].ToString());
                    }

                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("Category.aspx - Admin", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Deletes the Category image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImage(string ImageName)
        {
            try
            {
                if (File.Exists((ImageName)))
                    File.Delete((ImageName));
                CommonOperations.DeleteFileFromContentServer((ImageName));

                //if (File.Exists(Server.MapPath(ImageName)))
                // File.Delete(Server.MapPath(ImageName));
                //            CommonOperation.DeleteFileFromContentServer(Server.MapPath(ImageName));
            }
            catch (Exception ex)
            {
                lblMsg.Text += "<br />" + ex.Message;
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }

        /// <summary>
        /// Creates the Category image
        /// </summary>
        /// <param name="Size">string Size</param>
        /// <param name="FileName">string FileName</param>
        protected void CreateImage(string Size, string FileName)
        {
            try
            {
                string strFile = "";
                String strPath = "";
                string strFileBanner = "";
                String strPathBanner = "";
                Size = Size.ToLower();

                if (imgIcon.Src.ToString().IndexOf("?") > -1)
                {
                    strPath = imgIcon.Src.Split('?')[0];
                }
                else
                {
                    strPath = imgIcon.Src.ToString();
                }

                if (Size.ToLower() == "banner")
                {
                    if (imgBanner.Src.ToString().IndexOf("?") > -1)
                    {
                        strPathBanner = imgBanner.Src.Split('?')[0];
                    }
                    else
                    {
                        strPathBanner = imgBanner.Src.ToString();
                    }
                }

                strFile = Server.MapPath(strPath);
                strFileBanner = Server.MapPath(strPathBanner);
                string Path = "";
                switch (Size)
                {
                    case "icon":
                        Path = Server.MapPath(CategoryIconPath + FileName);
                        if (Request.QueryString["Mode"] == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(Server.MapPath(CategoryIconPath + ViewState["DelImage"].ToString()));
                            }
                        }
                        break;
                    case "micro":
                        Path = Server.MapPath(CategoryMicroPath + FileName);
                        if (Request.QueryString["Mode"] == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(Server.MapPath(CategoryMicroPath + ViewState["DelImage"].ToString()));
                            }
                        }
                        break;
                    case "banner":
                        Path = Server.MapPath(CategoryBannerPath + FileName);
                        if (Request.QueryString["Mode"] == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(Server.MapPath(CategoryBannerPath + ViewState["DelImage"].ToString()));
                            }
                        }
                        break;
                }
                if (Size == "icon")
                    ResizePhoto(strFile, Size, Path);
                if (Size == "micro")
                    ResizePhoto(strFile, Size, Path);
                if (Size == "banner")
                    ResizePhoto(strFileBanner, Size, Path);
            }
            catch (Exception ex)
            {
                if (ex.Source == "System.Drawing")
                    lblMsg.Text = "<br />Error Saving " + Size + " Image..Please check that Directory exists..";
                else
                    lblMsg.Text += "<br />" + ex.Message;
                CommonComponent.ErrorLog("Category.aspx - Admin", ex.Message, ex.StackTrace);
            }
        }

        /// <summary>
        /// Resizes the photo
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="Size">string Size</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizePhoto(string strFile, string Size, string strFilePath)
        {
            switch (Size)
            {
                case "icon":
                    finHeight = thumbNailSizeIcon.Height;
                    finWidth = thumbNailSizeIcon.Width;
                    break;
                case "micro":
                    finHeight = thumbNailSizeMicro.Height;
                    finWidth = thumbNailSizeMicro.Width;
                    break;
                case "banner":
                    finHeight = thumbNailSizeBanner.Height;
                    finWidth = thumbNailSizeBanner.Width;
                    break;
            }
            ResizeImage(strFile, finWidth, finHeight, strFilePath);
        }

        /// <summary>
        /// Resizes the image.
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
        /// Generates the Category Image.
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
        /// Gets the Encoder Information.
        /// </summary>
        /// <param name="resizeMimeType">string resizeMimeType</param>
        /// <returns>Returns the ImageCodecInfo Object.</returns>
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
        /// Method for Clear All Field if not Want Add or Update Data
        /// </summary>
        private void clearselection()
        {
            ddlStore.ClearSelection();
            txtdisplayorder.Text = string.Empty;
            txtname.Text = string.Empty;
            txtsekeyword.Text = string.Empty;
            txtseodescription.Text = string.Empty;
            txtsetitle.Text = string.Empty;
            //txttooltip.Text = string.Empty;
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            clearselection();
            Response.Redirect("/Admin/Products/Category.aspx");
        }

        /// <summary>
        ///  Upload Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnUpload_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            string CategoryTempPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Temp/");
            //clsvariables.LoadAllPath();
            if (!Directory.Exists(Server.MapPath(CategoryTempPath)))
                Directory.CreateDirectory(Server.MapPath(CategoryTempPath));


            for (int j = 0; j < StoreArray.Length; j++)
                if (fuIcon.FileName.Length > 0 && Path.GetExtension(fuIcon.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fuIcon.FileName.Length > 0)
                {
                    ViewState["File"] = fuIcon.FileName.ToString();
                    fuIcon.SaveAs(Server.MapPath(CategoryTempPath) + fuIcon.FileName);
                    imgIcon.Src = CategoryTempPath + fuIcon.FileName;
                    lblMsg.Text = "";
                }
            }
            else
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";


        }

        /// <summary>
        ///  Banner Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnBanner_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            //clsvariables.LoadAllPath();
            string CategoryBannerTempPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Temp/Banner/");
            if (!Directory.Exists(Server.MapPath(CategoryBannerTempPath)))
                Directory.CreateDirectory(Server.MapPath(CategoryBannerTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (fucategorybanner.FileName.Length > 0 && Path.GetExtension(fucategorybanner.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fucategorybanner.FileName.Length > 0)
                {
                    ViewState["BannerFile"] = fucategorybanner.FileName.ToString();
                    fucategorybanner.SaveAs(Server.MapPath(CategoryBannerTempPath) + fucategorybanner.FileName);

                    imgBanner.Src = CategoryBannerTempPath + fucategorybanner.FileName;
                    lblMsg.Text = "";

                }
            }
            else
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
        }

        protected void imgdelete_Click(object sender, ImageClickEventArgs e)
        {
            string strImg = "";
            strImg = imgBanner.Src.ToString();
            if (File.Exists(Server.MapPath(strImg)))
            {
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(strImg));
                File.Delete(Server.MapPath(strImg));
                imgBanner.Src = string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
            }
            imgdelete.Visible = false;


        }

        /// <summary>
        /// Initializes the <see cref="T:System.Web.UI.HtmlTextWriter" /> object and calls on the child controls of the <see cref="T:System.Web.UI.Page" /> to render.
        /// </summary>
        /// <param name="writer">The <see cref="T:System.Web.UI.HtmlTextWriter" /> that receives the page content.</param>
        protected override void Render(System.Web.UI.HtmlTextWriter writer)
        {
            try
            {
                System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder();
                System.IO.StringWriter stringWriter = new System.IO.StringWriter(stringBuilder);
                System.Web.UI.HtmlTextWriter htmlWriter = new System.Web.UI.HtmlTextWriter(stringWriter);
                base.Render(htmlWriter);
                string yourHtml = stringBuilder.ToString();//.Replace(stringBuilder.ToString().IndexOf("<input type=\"hidden\" name=\"__VIEWSTATE\" id=\"__VIEWSTATE\" value=") + ,""); // ***** Parse and Modify This *****
                yourHtml = yourHtml.Replace("id=\"ContentPlaceHolder1_tvCategoryn0CheckBox\"", "id=\"ContentPlaceHolder1_tvCategoryn0CheckBox\" onclick=\"ChkRootNode();\" onchange=\"ChkRootNode();\"");
                yourHtml = yourHtml.Replace("id=&#34;ContentPlaceHolder1_tvMasterCategoryListt", "onclick=\"chkHeight();\" id=&#34;ContentPlaceHolder1_tvMasterCategoryListt");
                yourHtml = yourHtml.Replace("id=\"ContentPlaceHolder1_tvMasterCategoryListt", "onclick=\"chkHeight();\" id=\"ContentPlaceHolder1_tvMasterCategoryListt");
                yourHtml = yourHtml.Replace("style=\"white-space:nowrap;\"", "");

                if (Request.QueryString["Node"] != null)
                {
                    if (Request.QueryString["Search"] != null && txtSearch.Text.ToString().Trim() == "")
                    {

                    }
                    else
                    {
                        yourHtml = yourHtml.Replace("id=\"ContentPlaceHolder1_tvMasterCategoryListt" + Request.QueryString["Node"].ToString() + "\"", " style=\"Color:#000 !important;\" id=\"ContentPlaceHolder1_tvMasterCategoryListt" + Request.QueryString["Node"].ToString() + "\"");
                    }
                }
                writer.Write(yourHtml);
            }
            catch
            {
            }
        }

        /// <summary>
        ///  Collapse All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkCollapseAll_Click(object sender, EventArgs e)
        {
            tvMasterCategoryList.CollapseAll();
        }

        /// <summary>
        ///  Expand All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkExpandAll_Click(object sender, EventArgs e)
        {
            tvMasterCategoryList.ExpandAll();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtngo_Click(object sender, ImageClickEventArgs e)
        {

            strSearchedCategory = ",";
            ViewState["SearchEnabled"] = "1";
            ViewState["SearchText"] = txtSearch.Text.Trim();
            FillMasterCategorylist();
            if (txtSearch.Text.ToString().Trim() == "")
            {
                nodecount1 = 0;
                nodecount = -1;
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("sears") > -1)
            {
                lblSearsCategoryID.Text = "Sears Category ID";
                trSearsCatID.Visible = true;
            }
            else if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("over stock") > -1 || ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("overstock") > -1)
            {
                lblSearsCategoryID.Text = "Over Stock Category ID";
                trSearsCatID.Visible = true;
            }
            else
            {
                trSearsCatID.Visible = false;
            }

            bindTreeviewwithcategory();
            FillMasterCategorylist();
            FillProductfeature();
        }

        /// <summary>
        /// Category Status Radio Button Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rdlCategoryStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ibtngo_Click(null, null);
        }

        private void FillProductfeature()
        {
            DataSet DsProductfeature = new DataSet();
            ProductComponent objProduct = new ProductComponent();

            if (ddlStore.SelectedValue.ToString() != null && ddlStore.SelectedValue.ToString() != "")
            {
                StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else { StoreID = 1; }

            DsProductfeature = objProduct.GetproductFeature(0, StoreID, 1);
            ddlproductfeature.Items.Clear();
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

        protected void ibtnBanner2_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            //clsvariables.LoadAllPath();
            string CategoryBannerTempPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Temp/Banner/");
            if (!Directory.Exists(Server.MapPath(CategoryBannerTempPath)))
                Directory.CreateDirectory(Server.MapPath(CategoryBannerTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (fucategorybanner2.FileName.Length > 0 && Path.GetExtension(fucategorybanner2.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fucategorybanner2.FileName.Length > 0)
                {
                    ViewState["BannerFile1"] = fucategorybanner2.FileName.ToString();
                    fucategorybanner2.SaveAs(Server.MapPath(CategoryBannerTempPath) + fucategorybanner2.FileName);

                    imgBanner2.Src = CategoryBannerTempPath + fucategorybanner2.FileName;
                    lblMsg.Text = "";

                }
            }
            else
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
        }

        protected void imgdelete2_Click(object sender, ImageClickEventArgs e)
        {
            string strImg = "";
            strImg = imgBanner2.Src.ToString();
            if (File.Exists(Server.MapPath(strImg)))
            {
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(strImg));
                File.Delete(Server.MapPath(strImg));
                imgBanner2.Src = string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
            }
            imgdelete2.Visible = false;

        }

        protected void ibtnBanner3_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            //clsvariables.LoadAllPath();
            string CategoryBannerTempPath = string.Concat(AppLogic.AppConfigs("ImagePathCategory"), "Temp/Banner/");
            if (!Directory.Exists(Server.MapPath(CategoryBannerTempPath)))
                Directory.CreateDirectory(Server.MapPath(CategoryBannerTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (fucategorybanner3.FileName.Length > 0 && Path.GetExtension(fucategorybanner3.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fucategorybanner3.FileName.Length > 0)
                {
                    ViewState["BannerFile2"] = fucategorybanner3.FileName.ToString();
                    fucategorybanner3.SaveAs(Server.MapPath(CategoryBannerTempPath) + fucategorybanner3.FileName);

                    imgBanner3.Src = CategoryBannerTempPath + fucategorybanner3.FileName;
                    lblMsg.Text = "";

                }
            }
            else
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
        }

        protected void imgdelete3_Click(object sender, ImageClickEventArgs e)
        {
            string strImg = "";
            strImg = imgBanner3.Src.ToString();
            if (File.Exists(Server.MapPath(strImg)))
            {
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(strImg));
                File.Delete(Server.MapPath(strImg));
                imgBanner3.Src = string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
            }
            imgdelete3.Visible = false;
        }
    }
}