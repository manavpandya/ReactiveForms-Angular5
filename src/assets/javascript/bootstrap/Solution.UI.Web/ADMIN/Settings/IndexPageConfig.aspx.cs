using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class IndexPageConfig : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindStore();
                BindFeaturedCategory();
                BindFeaturedSystem();
                BindBestSeller();
                BindNewArrival();
                Int32 ID = Convert.ToInt32(AppLogic.AppConfigs("HotDealProduct").ToString());
                string title = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='OnSaleTitle' and StoreID=1"));
                if (!string.IsNullOrEmpty(title))
                {
                    txtonsaletitle.Text = title;
                }
                string title1 = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='KidsCollectionTitle' and StoreID=1"));
                if (!string.IsNullOrEmpty(title1))
                {
                    txtkidscollection.Text = title1;
                }
                string title2 = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='GrommetCurtainTitle' and StoreID=1"));
                if (!string.IsNullOrEmpty(title2))
                {
                    txtgrommetcurtains.Text = title2;
                }
                string title3 = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='NewArrivalCurtainTitle' and StoreID=1"));
                if (!string.IsNullOrEmpty(title3))
                {
                    txtnewarrivalcurtains.Text = title3;
                }
                string title4 = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='CustomCurtainTitle' and StoreID=1"));
                if (!string.IsNullOrEmpty(title4))
                {
                    txtcustomcurtains.Text = title4;
                }
                string title5 = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='FabricCurtainTitle' and StoreID=1"));
                if (!string.IsNullOrEmpty(title5))
                {
                    txtfabriccurtains.Text = title5;
                }
                SaleClearanceDescription.Text = Server.HtmlDecode(Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='Buy1Get1Desc' and StoreID=1")));
                onsaledescription.Text = Server.HtmlDecode(Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='OnsaleDesc' and StoreID=1")));
                NewarrivalCurtainsDescription.Text = Server.HtmlDecode(Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='NewArrivalDesc' and StoreID=1")));
                CustomItemPageDescription.Text = Server.HtmlDecode(Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='CustomItemPageDesc' and StoreID=1")));
                

                bindProduct();
                FillHotDealProduct();
                FillSaleClearanceBanner();

                FillRomanCategoryBanner();
                FillFreeSwatchBanner();
                FillCustomItemPageBanner();
                FillOnSaleBanner();
                FillKidsCollectionBanner();
                FillGrommetCurtainsBanner();
                FillNewArrivalCurtainsBanner();
                FillCustomCurtainsBanner();
                FillFabricCurtainsBanner();
              
            }
            btnFeatureCategory.Attributes.Add("onclick", "return testi();");
            ibtnFeaturecategory.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-featured-category.png";
            ibtnFeaturesystem.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-products.png";
            ibtnBestseller.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-best-seller.png";
            ibtnnewarrival.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/add-new-arrival.png";
            imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            imgSaveSaleClearance.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            imgSaveRomanCat.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            ImgSaveFreeSwatch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            ImgSaveCustomItemPage.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            imgsaveonsale.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            imgsavekidscollection.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            btndeletekidscollection.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            imgsavegrommetcurtains.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            btndeletegrommetcurtains.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            imgsavenewarrivalcurtains.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            btndeletenewarrivalcurtains.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            imgsavecustomcurtains.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
            btndeletecustomcurtains.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            btndeletefabriccurtains.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
            imgsavefabriccurtains.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";

        }

        #region Bind New Arrival Product


        /// <summary>
        /// Binds the New Arrival
        /// </summary>
        private void BindNewArrival()
        {
            DataSet dsNewArrival = ProductComponent.GetAllNewArrivalProductAdmin(Convert.ToInt32(ddlStore.SelectedValue));
            if (dsNewArrival != null && dsNewArrival.Tables.Count > 0 && dsNewArrival.Tables[0].Rows.Count > 0)
            {
                grdNewarrival.DataSource = dsNewArrival;
                grdNewarrival.DataBind();
            }
            else
            {
                grdNewarrival.DataSource = null;
                grdNewarrival.DataBind();
            }

        }

        /// <summary>
        /// New Arrival Gridview Row Canceling Edit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCancelEditEventArgs e</param>
        protected void grdNewarrival_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdNewarrival.EditIndex = -1;
            BindNewArrival();

        }

        /// <summary>
        /// New Arrival Gridview Row Updating Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewUpdateEventArgs e</param>
        protected void grdNewarrival_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)grdNewarrival.Rows[e.RowIndex];
            HiddenField hdnProductid = (HiddenField)row.FindControl("hdnProductid");
            TextBox txtDisplayorder = (TextBox)row.FindControl("txtDisplayorder");
            if (txtDisplayorder.Text != "")
            {
                ProductComponent.UpdateDisplayOrderForIndexPage("DO_IndexPage", Convert.ToInt32(hdnProductid.Value), txtDisplayorder.Text.Trim());
            }
            grdNewarrival.EditIndex = -1;
            BindNewArrival();
        }

        /// <summary>
        /// New Arrival Gridview Row Editing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewEditEventArgs e</param>
        protected void grdNewarrival_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdNewarrival.EditIndex = e.NewEditIndex;
            BindNewArrival();
        }

        #endregion

        #region Bind Best Seller

        /// <summary>
        /// Binds the Best Seller
        /// </summary>
        private void BindBestSeller()
        {
            DataSet dsBestSeller = ProductComponent.DisplyProductByOption("IsBestSeller", Convert.ToInt32(ddlStore.SelectedValue), 20);
            if (dsBestSeller != null && dsBestSeller.Tables.Count > 0 && dsBestSeller.Tables[0].Rows.Count > 0)
                grdBestSeller.DataSource = dsBestSeller;
            else grdBestSeller.DataSource = null;
            grdBestSeller.DataBind();
        }

        /// <summary>
        /// Best Seller Gridview Row Canceling Edit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCancelEditEventArgs e</param>
        protected void grdBestSeller_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdBestSeller.EditIndex = -1;
            BindBestSeller();
        }

        /// <summary>
        /// Best Seller Gridview Row Editing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewEditEventArgs e</param>
        protected void grdBestSeller_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdBestSeller.EditIndex = e.NewEditIndex;
            BindBestSeller();
        }

        /// <summary>
        ///  Best Seller Gridview Row Updating Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewUpdateEventArgs e</param>
        protected void grdBestSeller_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)grdBestSeller.Rows[e.RowIndex];
            HiddenField hdnProductid = (HiddenField)row.FindControl("hdnProductId");
            TextBox txtDisplayorder = (TextBox)row.FindControl("txtDisplayorder");
            if (txtDisplayorder.Text != "")
            {
                ProductComponent.UpdateDisplayOrderForIndexPage("DO_IndexPage", Convert.ToInt32(hdnProductid.Value), txtDisplayorder.Text.Trim());
            }
            grdBestSeller.EditIndex = -1;
            BindBestSeller();
        }

        #endregion

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
        {
            DataSet dsStore = StoreComponent.GetStoreList();
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStore;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            BindFeaturedCategory(); BindFeaturedSystem(); BindBestSeller(); BindNewArrival();
        }

        #region Bind Featured Category

        /// <summary>
        /// Binds the Featured Category
        /// </summary>
        private void BindFeaturedCategory()
        {
            DataSet dsFeaturedCategory = CategoryComponent.GetFeaturedCategory(Convert.ToInt32(ddlStore.SelectedValue));
            if (dsFeaturedCategory != null && dsFeaturedCategory.Tables.Count > 0 && dsFeaturedCategory.Tables[0].Rows.Count > 0)
                grdFeaturedcategory.DataSource = dsFeaturedCategory;
            else grdFeaturedcategory.DataSource = null;
            grdFeaturedcategory.DataBind();
        }

        /// <summary>
        ///  Featured Category Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnFeatureCategory_Click(object sender, EventArgs e)
        {
            BindFeaturedCategory(); BindFeaturedSystem(); BindBestSeller(); BindNewArrival();
        }

        /// <summary>
        /// Featured Category Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdFeaturedcategory_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnCatid = (HiddenField)e.Row.FindControl("hdnCategoryid");
                Label lblParent = (Label)e.Row.FindControl("lblPname");

                DataSet dsPname = CategoryComponent.GetParentCategoryNamebyCategoryID(Convert.ToInt16(hdnCatid.Value));
                if (dsPname != null && dsPname.Tables.Count > 0 && dsPname.Tables[0].Rows.Count > 0)
                {
                    int pCount = dsPname.Tables[0].Rows.Count;
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    for (int i = 0; i < pCount; i++)
                    {
                        sb.Append(dsPname.Tables[0].Rows[i]["Name"].ToString() + ", ");

                    }
                    int length = sb.ToString().Length;
                    string pName = sb.ToString().Remove(sb.ToString().LastIndexOf(","));
                    lblParent.Text = pName.ToString();
                }
            }
        }

        /// <summary>
        /// Featured Category Gridview Row Editing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewEditEventArgs e</param>
        protected void grdFeaturedcategory_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdFeaturedcategory.EditIndex = e.NewEditIndex;
            BindFeaturedCategory();
        }

        /// <summary>
        /// Featured Category Gridview Row Canceling Edit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCancelEditEventArgs e</param>
        protected void grdFeaturedcategory_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdFeaturedcategory.EditIndex = -1;
            BindFeaturedCategory();
        }

        /// <summary>
        /// Featured Category Gridview Row Updating Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewUpdateEventArgs e</param>
        protected void grdFeaturedcategory_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)grdFeaturedcategory.Rows[e.RowIndex];
            HiddenField hdnCategoryid = (HiddenField)row.FindControl("hdnCategoryid");
            TextBox txtDisplayorder = (TextBox)row.FindControl("txtDisplayorder");
            if (txtDisplayorder.Text != "")
            {
                CategoryComponent.UpdateCategoryDisplayOrder(Convert.ToInt32(hdnCategoryid.Value), Convert.ToInt32(ddlStore.SelectedValue), Convert.ToInt32(txtDisplayorder.Text.Trim()));
            }
            grdFeaturedcategory.EditIndex = -1;
            BindFeaturedCategory();
        }

        #endregion

        #region Bind Featured System

        /// <summary>
        /// Binds the Featured System
        /// </summary>
        private void BindFeaturedSystem()
        {
            DataSet dsFeature = ProductComponent.DisplyProductByOption("IsFeatured", Convert.ToInt32(ddlStore.SelectedValue), 20);
            if (dsFeature != null && dsFeature.Tables.Count > 0 && dsFeature.Tables[0].Rows.Count > 0)
            {
                grdFeaturedSystem.DataSource = dsFeature;
                grdFeaturedSystem.DataBind();
                hdnTotFeaturecnt.Value = dsFeature.Tables[0].Rows.Count.ToString();
            }
            else
            {
                grdFeaturedSystem.DataSource = null;
                grdFeaturedSystem.DataBind();
                hdnTotFeaturecnt.Value = "0";
            }
        }

        /// <summary>
        /// Featured System Gridview Row Canceling Edit Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCancelEditEventArgs e</param>
        protected void grdFeaturedSystem_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            grdFeaturedSystem.EditIndex = -1;
            BindFeaturedSystem();
        }

        /// <summary>
        /// Featured System Gridview Row Updating Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewUpdateEventArgs e</param>
        protected void grdFeaturedSystem_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            GridViewRow row = (GridViewRow)grdFeaturedSystem.Rows[e.RowIndex];
            HiddenField hdnProductid = (HiddenField)row.FindControl("hdnProductid");
            TextBox txtDisplayorder = (TextBox)row.FindControl("txtDisplayorder");
            if (txtDisplayorder.Text != "")
            {
                //ProductComponent.UpdateProductByDisplayOrderPriceInventory("DO", Convert.ToInt32(hdnProductid.Value), txtDisplayorder.Text.Trim(), null);
                ProductComponent.UpdateDisplayOrderForIndexPage("DO_IndexPage", Convert.ToInt32(hdnProductid.Value), txtDisplayorder.Text.Trim());

            }
            grdFeaturedSystem.EditIndex = -1;
            BindFeaturedSystem();
        }

        /// <summary>
        /// Featured System Gridview Row Editing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewEditEventArgs e</param>
        protected void grdFeaturedSystem_RowEditing(object sender, GridViewEditEventArgs e)
        {
            grdFeaturedSystem.EditIndex = e.NewEditIndex;
            BindFeaturedSystem();
        }

        #endregion

        #region Bind Deal of the Day Product

        /// <summary>
        /// Fills the Hot Deal Product
        /// </summary>
        private void FillHotDealProduct()
        {
            string ProductId = "0";
            string HotOfPrice = "0";
            ProductId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ConfigValue FROM dbo.tb_AppConfig WHERE  ConfigName ='HotdealProduct' AND StoreID=" + Convert.ToInt32(ddlStore.SelectedValue)));
            HotOfPrice = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT ConfigValue FROM dbo.tb_AppConfig WHERE  ConfigName ='HotdealPrice' AND StoreID=" + Convert.ToInt32(ddlStore.SelectedValue)));
            txtHotdealprice.Text = HotOfPrice;
            if (ProductId != "0")
                ViewState["ProductId"] = ProductId;
            if (System.IO.File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "HotDeal/" + ProductId + ".jpg")))
            {
                imgBanner.Src = AppLogic.AppConfigs("ImagePathBanner") + "HotDeal/" + ProductId + ".jpg";
                imgBanner.Visible = true;
            }
        }


        private void FillSaleClearanceBanner()
        {

            string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/SaleClearanceBanner/"));
            if (strGetfiles.Length > 0)
            {
                foreach (string strfl in strGetfiles)
                {
                    if (File.Exists(strfl))
                    {
                        FileInfo fl = new FileInfo(strfl);
                        imgClearanceBanner.Src = "/images/SaleClearanceBanner/" + fl.Name.ToString() + "";
                        imgClearanceBanner.Visible = true;
                        break;
                    }
                }
            }

            //if (System.IO.File.Exists(Server.MapPath("/images/SaleClearanceBanner/" + FileUploadClearanceBanner.FileName + "")))
            //{
            //    imgClearanceBanner.Src = "/images/SaleClearanceBanner/" + FileUploadClearanceBanner.FileName + "";
            //    imgClearanceBanner.Visible = true;
            //}
        }

        private void FillOnSaleBanner()
        {

            string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/OnSaleBanner/"));
            if (strGetfiles.Length > 0)
            {

                btnDelete.Visible = false;
                foreach (string strfl in strGetfiles)
                {
                    if (File.Exists(strfl))
                    {
                        FileInfo fl = new FileInfo(strfl);
                        imgonsalename.Value = fl.Name.ToString();
                        imgonsale.Src = "/images/OnSaleBanner/" + fl.Name.ToString() + "";
                        ViewState["DelImage"] = imgonsale.Src;
                        imgonsale.Visible = true;
                        btnDelete.Visible = true;
                        break;
                    }
                    else
                    {
                        ViewState["DelImage"] = null;
                        btnDelete.Visible = false;
                    }
                }
            }

            //if (System.IO.File.Exists(Server.MapPath("/images/SaleClearanceBanner/" + FileUploadClearanceBanner.FileName + "")))
            //{
            //    imgClearanceBanner.Src = "/images/SaleClearanceBanner/" + FileUploadClearanceBanner.FileName + "";
            //    imgClearanceBanner.Visible = true;
            //}
        }








        private void FillRomanCategoryBanner()
        {

            string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/RomanCategoryBanner/"));
            if (strGetfiles.Length > 0)
            {
                foreach (string strfl in strGetfiles)
                {
                    if (File.Exists(strfl))
                    {
                        FileInfo fl = new FileInfo(strfl);
                        imgRomanBanner.Src = "/images/RomanCategoryBanner/" + fl.Name.ToString() + "";
                        imgRomanBanner.Visible = true;
                        break;
                    }
                }
            }

        }

        /// <summary>
        /// Binds the Products into Drop Down
        /// </summary>
        private void bindProduct()
        {
            Int32 StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            DataSet DsProduct = new DataSet();
            DsProduct = CommonComponent.GetCommonDataSet("SELECT ProductID,SKU,Name +' - '+ SKU AS ProductName FROM dbo.tb_Product WHERE Deleted=0 and tb_Product.productid not in(select tb_Giftcardproduct.productid from tb_Giftcardproduct) AND  Active=1 and StoreID = " + StoreID + " order by Name ASC");
            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
                ddlProduct.DataSource = DsProduct;
                ddlProduct.DataBind();
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            String StrConfigValue1 = "0";
            Int32 StoID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            StrConfigValue1 = ddlProduct.SelectedValue.ToString();
            string strConfig = " Update tb_AppConfig  set ConfigValue=" + StrConfigValue1 + " where StoreID=" + StoID + " and ConfigName ='HotdealProduct' ";
            CommonComponent.ExecuteCommonData(strConfig);

            Decimal HotPrice = Convert.ToDecimal(txtHotdealprice.Text);
            string strConfigPrice = " Update tb_AppConfig  set ConfigValue=" + HotPrice + " where StoreID=" + StoID + " and ConfigName ='HotDealPrice' ";
            CommonComponent.ExecuteCommonData(strConfigPrice);

            Int32 isupdated = Convert.ToInt32(ddlProduct.SelectedValue.ToString());
            //if (ViewState["ProductId"] != null)
            //{
            //    try
            //    {
            //        File.Delete(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "HotDeal/" + ViewState["ProductId"].ToString() + ".jpg"));
            //    }

            //    catch { }
            //}
            if (!string.IsNullOrEmpty(ID) && Convert.ToString(ID) != "0")
            {

                if (FileUploadBanner.FileName.Length > 0)
                {

                    FileUploadBanner.PostedFile.SaveAs(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "HotDeal/" + isupdated + ".jpg"));
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "HotDeal/" + isupdated + ".jpg"));
                    }
                    catch
                    {

                    }

                }
                if (isupdated > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Successfully Updated...', 'Message');});", true);
                    FillHotDealProduct();
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record not found.', 'Message');});", true);
                return;
            }
        }



        protected void imgSaveSaleClearance_Click(object sender, ImageClickEventArgs e)
        {
            if (FileUploadClearanceBanner.HasFile && FileUploadClearanceBanner.FileName.Length > 0)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/SaleClearanceBanner/"));
                    imgClearanceBanner.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {
                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }
                if (FileUploadClearanceBanner.FileName.Length > 0)
                {

                    FileUploadClearanceBanner.PostedFile.SaveAs(Server.MapPath("/images/SaleClearanceBanner/" + FileUploadClearanceBanner.FileName + ""));
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/SaleClearanceBanner/" + FileUploadClearanceBanner.FileName));
                    }
                    catch
                    {

                    }
                }

                CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + SaleClearanceDescription.Text.Trim().Replace("'", "''") + "' where configname='Buy1Get1Desc' and storeid=1");


                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Successfully Updated...', 'Message');});", true);
                FillSaleClearanceBanner();

            }
            else
            {

                CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + SaleClearanceDescription.Text.Trim().Replace("'", "''") + "' where configname='Buy1Get1Desc' and storeid=1");

            }
            return;

        }


        protected void imgSaveRomanCat_Click(object sender, ImageClickEventArgs e)
        {
            try
            {

                string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/RomanCategoryBanner/"));
                if (strGetfiles.Length > 0)
                {
                    foreach (string strfl in strGetfiles)
                    {
                        if (File.Exists(strfl))
                        {
                            File.Delete(strfl);
                        }
                    }
                }
            }
            catch { }
            if (FileUploadRomanBanner.FileName.Length > 0)
            {

                FileUploadRomanBanner.PostedFile.SaveAs(Server.MapPath("/images/RomanCategoryBanner/" + FileUploadRomanBanner.FileName + ""));
                try
                {
                    CompressimagePanda objcompress = new CompressimagePanda();
                    objcompress.compressimage(Server.MapPath("/images/RomanCategoryBanner/" + FileUploadRomanBanner.FileName));
                }
                catch
                {

                }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Successfully Updated...', 'Message');});", true);
            FillRomanCategoryBanner();
            return;

        }


        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/Settings/IndexPageConfig.aspx");
        }

        #endregion

        /// <summary>
        /// Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            int CategoryID = Convert.ToInt32(hdnDelete.Value);
            CategoryComponent objCatComponent = new CategoryComponent();
            tb_Category category = objCatComponent.GetCategoryByCategoryID(CategoryID);
            category.IsFeatured = false;
            objCatComponent.updateCategory(category);
            BindFeaturedCategory();
        }

        /// <summary>
        ///  Hidden Featured Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnFeaturedProduct_Click(object sender, EventArgs e)
        {
            int ProductID = Convert.ToInt32(hdnFeaturedProduct.Value);
            ProductComponent ProductComponent = new ProductComponent();
            tb_Product product = ProductComponent.GetProductDetailByProductID(ProductID);
            product.IsFeatured = false;
            ProductComponent.UpdateProduct(product);
            BindFeaturedSystem();
        }

        /// <summary>
        ///  Hidden Best Seller Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnBestSeller_Click(object sender, EventArgs e)
        {
            int ProductID = Convert.ToInt32(hdnBestSeller.Value);
            ProductComponent ProductComponent = new ProductComponent();
            tb_Product product = ProductComponent.GetProductDetailByProductID(ProductID);
            product.IsBestSeller = false;
            ProductComponent.UpdateProduct(product);
            BindBestSeller();
        }

        /// <summary>
        ///  Hidden New Arrival Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnNewArrival_Click(object sender, EventArgs e)
        {
            int ProductID = Convert.ToInt32(hdnNewArrival.Value);
            ProductComponent ProductComponent = new ProductComponent();
            tb_Product product = ProductComponent.GetProductDetailByProductID(ProductID);
            product.IsNewArrival = false;
            ProductComponent.UpdateProduct(product);
            BindNewArrival();
        }

        protected void ImgSaveFreeSwatch_Click(object sender, ImageClickEventArgs e)
        {
            if (FileUploadFreeSwatch.FileName.Length > 0)
            {
                try
                {

                    string subPath = "/images/FreeSwatchBanner"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/FreeSwatchBanner/"));
                    imgFreeSwatch.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {
                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }
                if (FileUploadFreeSwatch.FileName.Length > 0)
                {

                    FileUploadFreeSwatch.PostedFile.SaveAs(Server.MapPath("/images/FreeSwatchBanner/" + FileUploadFreeSwatch.FileName + ""));
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/FreeSwatchBanner/" + FileUploadFreeSwatch.FileName));
                    }
                    catch
                    {

                    }
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Successfully Updated...', 'Message');});", true);
                FillFreeSwatchBanner();

            }
            return;
        }


        private void FillFreeSwatchBanner()
        {
            try
            {
                string subPath = "/images/FreeSwatchBanner"; // your code goes here

                bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subPath));
                string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/FreeSwatchBanner/"));
                if (strGetfiles.Length > 0)
                {
                    foreach (string strfl in strGetfiles)
                    {
                        if (File.Exists(strfl))
                        {
                            FileInfo fl = new FileInfo(strfl);
                            imgFreeSwatch.Src = "/images/FreeSwatchBanner/" + fl.Name.ToString() + "";
                            imgFreeSwatch.Visible = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }
       
        protected void ImgSaveCustomItemPage_Click(object sender, ImageClickEventArgs e)
        {
            if (FileUploadCustomItemPage.HasFile && FileUploadCustomItemPage.FileName.Length > 0)
            {
                try
                {

                    string subPath = "/images/CustomItemPageBanner"; // your code goes here

                    bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                    if (!exists)
                        System.IO.Directory.CreateDirectory(Server.MapPath(subPath));

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/CustomItemPageBanner/"));
                    imgCustomItemPage.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {
                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }
                        }
                    }
                }
                catch { }
                if (FileUploadCustomItemPage.FileName.Length > 0)
                {

                    FileUploadCustomItemPage.PostedFile.SaveAs(Server.MapPath("/images/CustomItemPageBanner/" + FileUploadCustomItemPage.FileName + ""));
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/CustomItemPageBanner/" + FileUploadCustomItemPage.FileName));
                    }
                    catch
                    {

                    }
                }


                CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + CustomItemPageDescription.Text.Trim().Replace("'", "''") + "' where configname='CustomItemPageDesc' and storeid=1");

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Successfully Updated...', 'Message');});", true);
                FillCustomItemPageBanner();

            }
            else
            {
                CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + CustomItemPageDescription.Text.Trim().Replace("'", "''") + "' where configname='CustomItemPageDesc' and storeid=1");
            }
            return;
        }

        private void FillCustomItemPageBanner()
        {
            try
            {
                string subPath = "/images/CustomItemPageBanner"; // your code goes here

                bool exists = System.IO.Directory.Exists(Server.MapPath(subPath));

                if (!exists)
                    System.IO.Directory.CreateDirectory(Server.MapPath(subPath));
                string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/CustomItemPageBanner/"));
                if (strGetfiles.Length > 0)
                {
                    foreach (string strfl in strGetfiles)
                    {
                        if (File.Exists(strfl))
                        {
                            FileInfo fl = new FileInfo(strfl);
                            imgCustomItemPage.Src = "/images/CustomItemPageBanner/" + fl.Name.ToString() + "";
                            imgCustomItemPage.Visible = true;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }

        }

        protected void imgsaveonsale_Click(object sender, ImageClickEventArgs e)
        {

            CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue='" + txtonsaletitle.Text.Trim() + "' where ConfigName='OnSaleTitle' and StoreID=1");

            if (fuonsale.HasFile && fuonsale.FileName.Length > 0)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/OnSaleBanner/"));
                    imgonsale.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {

                            FileInfo fl = new FileInfo(strfl);

                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }

                            try
                            {
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath("/images/OnSaleBanner/" + fl.Name.ToString() + ""));
                            }
                            catch { }
                        }
                    }
                }
                catch { }




                if (fuonsale.FileName.Length > 0)
                {

                    fuonsale.PostedFile.SaveAs(Server.MapPath("/images/OnSaleBanner/" + fuonsale.FileName + ""));
                    imgonsale.Visible = true;
                    btnDelete.Visible = true;
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/OnSaleBanner/" + fuonsale.FileName));
                    }
                    catch
                    {

                    }
                    CommonOperations.SaveOnContentServer(Server.MapPath("/images/OnSaleBanner/" + fuonsale.FileName + ""));
                }


                CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + onsaledescription.Text.Trim().Replace("'", "''") + "' where configname='OnsaleDesc' and storeid=1");

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Updated...', 'Message');});", true);
                FillOnSaleBanner();

            }
            else
            {

                CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + onsaledescription.Text.Trim().Replace("'", "''") + "' where configname='OnsaleDesc' and storeid=1");

            }
            return;
        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {

            DeleteImage(ViewState["DelImage"].ToString());
            ViewState["DelImage"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            // ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
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
                imgonsale.Visible = false;
                imgonsalename.Value = "";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Deleted...', 'Message');});", true);
            }
            catch (Exception ex)
            {
                lblMsg.Text += "<br />" + ex.Message;
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }


        private void FillKidsCollectionBanner()
        {

            string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/KidsCollectionBanner/"));
            if (strGetfiles.Length > 0)
            {

                btndeletekidscollection.Visible = false;
                foreach (string strfl in strGetfiles)
                {
                    if (File.Exists(strfl))
                    {
                        FileInfo fl = new FileInfo(strfl);
                        imgkidscollectionname.Value = fl.Name.ToString();
                        imgkidscollection.Src = "/images/KidsCollectionBanner/" + fl.Name.ToString() + "";
                        ViewState["DelImageKids"] = imgkidscollection.Src;
                        imgkidscollection.Visible = true;
                        btndeletekidscollection.Visible = true;
                        break;
                    }
                    else
                    {
                        ViewState["DelImageKids"] = null;
                        btndeletekidscollection.Visible = false;
                    }
                }
            }


        }

        protected void imgsavekidscollection_Click(object sender, ImageClickEventArgs e)
        {
            CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue='" + txtkidscollection.Text.Trim() + "' where ConfigName='KidsCollectionTitle' and StoreID=1");

            if (FileUploadkidscollection.FileName.Length > 0)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/KidsCollectionBanner/"));
                    imgkidscollection.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {

                            FileInfo fl = new FileInfo(strfl);

                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }

                            try
                            {
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath("/images/KidsCollectionBanner/" + fl.Name.ToString() + ""));
                            }
                            catch { }
                        }
                    }
                }
                catch { }




                if (FileUploadkidscollection.FileName.Length > 0)
                {

                    FileUploadkidscollection.PostedFile.SaveAs(Server.MapPath("/images/KidsCollectionBanner/" + FileUploadkidscollection.FileName + ""));
                    imgkidscollection.Visible = true;
                    btndeletekidscollection.Visible = true;
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/KidsCollectionBanner/" + FileUploadkidscollection.FileName));
                    }
                    catch
                    {

                    }
                    CommonOperations.SaveOnContentServer(Server.MapPath("/images/KidsCollectionBanner/" + FileUploadkidscollection.FileName + ""));
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Updated...', 'Message');});", true);
                FillKidsCollectionBanner();

            }
            return;

        }

        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImageKidsCollection(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(ImageName)))
                    File.Delete(Server.MapPath(ImageName));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ImageName));
                imgkidscollection.Visible = false;
                imgkidscollectionname.Value = "";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Deleted...', 'Message');});", true);
            }
            catch (Exception ex)
            {
                lblkidscollectionmsg.Text += "<br />" + ex.Message;
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }

        protected void btndeletekidscollection_Click(object sender, ImageClickEventArgs e)
        {
            DeleteImageKidsCollection(ViewState["DelImageKids"].ToString());
            ViewState["DelImageKids"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            // ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
            btndeletekidscollection.Visible = false;
        }


        private void FillGrommetCurtainsBanner()
        {

            string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/GrommetCurtainBanner/"));
            if (strGetfiles.Length > 0)
            {

                btndeletegrommetcurtains.Visible = false;
                foreach (string strfl in strGetfiles)
                {
                    if (File.Exists(strfl))
                    {
                        FileInfo fl = new FileInfo(strfl);
                        imggrommetcurtainsname.Value = fl.Name.ToString();
                        imggrommetcurtains.Src = "/images/GrommetCurtainBanner/" + fl.Name.ToString() + "";
                        ViewState["DelImageGrommet"] = imggrommetcurtains.Src;
                        imggrommetcurtains.Visible = true;
                        btndeletegrommetcurtains.Visible = true;
                        break;
                    }
                    else
                    {
                        ViewState["DelImageGrommet"] = null;
                        btndeletegrommetcurtains.Visible = false;
                    }
                }
            }


        }
        private void FillNewArrivalCurtainsBanner()
        {

            string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/NewArrivalBanner/"));
            if (strGetfiles.Length > 0)
            {

                btndeletenewarrivalcurtains.Visible = false;
                foreach (string strfl in strGetfiles)
                {
                    if (File.Exists(strfl))
                    {
                        FileInfo fl = new FileInfo(strfl);
                        imgnewarrivalcurtainsname.Value = fl.Name.ToString();
                        imgnewarrivalcurtains.Src = "/images/NewArrivalBanner/" + fl.Name.ToString() + "";
                        ViewState["DelImagenewarrival"] = imgnewarrivalcurtains.Src;
                        imgnewarrivalcurtains.Visible = true;
                        btndeletenewarrivalcurtains.Visible = true;
                        break;
                    }
                    else
                    {
                        ViewState["DelImagenewarrival"] = null;
                        btndeletenewarrivalcurtains.Visible = false;
                    }
                }
            }


        }

        protected void imgsavegrommetcurtains_Click(object sender, ImageClickEventArgs e)
        {
            CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue='" + txtgrommetcurtains.Text.Trim() + "' where ConfigName='GrommetCurtainTitle' and StoreID=1");

            if (FileUploadgrommetcurtains.FileName.Length > 0)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/GrommetCurtainBanner/"));
                    imggrommetcurtains.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {

                            FileInfo fl = new FileInfo(strfl);

                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }

                            try
                            {
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath("/images/GrommetCurtainBanner/" + fl.Name.ToString() + ""));
                            }
                            catch { }
                        }
                    }
                }
                catch { }




                if (FileUploadgrommetcurtains.FileName.Length > 0)
                {

                    FileUploadgrommetcurtains.PostedFile.SaveAs(Server.MapPath("/images/GrommetCurtainBanner/" + FileUploadgrommetcurtains.FileName + ""));
                    imggrommetcurtains.Visible = true;
                    btndeletegrommetcurtains.Visible = true;
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/GrommetCurtainBanner/" + FileUploadgrommetcurtains.FileName));
                    }
                    catch
                    {

                    }
                    CommonOperations.SaveOnContentServer(Server.MapPath("/images/GrommetCurtainBanner/" + FileUploadgrommetcurtains.FileName + ""));
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Updated...', 'Message');});", true);
                FillGrommetCurtainsBanner();

            }
            return;
        }

        protected void btndeletegrommetcurtains_Click(object sender, ImageClickEventArgs e)
        {
            DeleteImageGrommetCurtains(ViewState["DelImageGrommet"].ToString());
            ViewState["DelImageGrommet"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            // ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
            btndeletegrommetcurtains.Visible = false;
        }


        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImageGrommetCurtains(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(ImageName)))
                    File.Delete(Server.MapPath(ImageName));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ImageName));
                imggrommetcurtains.Visible = false;
                imggrommetcurtainsname.Value = "";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Deleted...', 'Message');});", true);
            }
            catch (Exception ex)
            {
                lblgrommetcurtainsmsg.Text += "<br />" + ex.Message;
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }

        protected void imgsavenewarrivalcurtains_Click(object sender, ImageClickEventArgs e)
        {
            CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue='" + txtnewarrivalcurtains.Text.Trim() + "' where ConfigName='NewArrivalCurtainTitle' and StoreID=1");

            if (FileUploadnewarrivalcurtains.HasFile && FileUploadnewarrivalcurtains.FileName.Length > 0)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/NewArrivalBanner/"));
                    imgnewarrivalcurtains.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {

                            FileInfo fl = new FileInfo(strfl);

                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }

                            try
                            {
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath("/images/NewArrivalBanner/" + fl.Name.ToString() + ""));
                            }
                            catch { }
                        }
                    }
                }
                catch { }




                if (FileUploadnewarrivalcurtains.FileName.Length > 0)
                {

                    FileUploadnewarrivalcurtains.PostedFile.SaveAs(Server.MapPath("/images/NewArrivalBanner/" + FileUploadnewarrivalcurtains.FileName + ""));
                    imgnewarrivalcurtains.Visible = true;
                    btndeletenewarrivalcurtains.Visible = true;
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/NewArrivalBanner/" + FileUploadnewarrivalcurtains.FileName));
                    }
                    catch
                    {

                    }
                    CommonOperations.SaveOnContentServer(Server.MapPath("/images/NewArrivalBanner/" + FileUploadnewarrivalcurtains.FileName + ""));
                }


                CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + NewarrivalCurtainsDescription.Text.Trim().Replace("'", "''") + "' where configname='NewArrivalDesc' and storeid=1");


                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Updated...', 'Message');});", true);
                FillNewArrivalCurtainsBanner();

            }
            else
            {
                CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + NewarrivalCurtainsDescription.Text.Trim().Replace("'", "''") + "' where configname='NewArrivalDesc' and storeid=1");
            }
            return;
        }

        protected void btndeletenewarrivalcurtains_Click(object sender, ImageClickEventArgs e)
        {
            DeleteImagenewarrivalCurtains(ViewState["DelImagenewarrival"].ToString());
            ViewState["DelImagenewarrival"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            // ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
            btndeletenewarrivalcurtains.Visible = false;
        }



        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImagenewarrivalCurtains(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(ImageName)))
                    File.Delete(Server.MapPath(ImageName));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ImageName));
                imgnewarrivalcurtains.Visible = false;
                imgnewarrivalcurtainsname.Value = "";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Deleted...', 'Message');});", true);
            }
            catch (Exception ex)
            {
                lblnewarrivalcurtainsmsg.Text += "<br />" + ex.Message;
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }


        private void FillCustomCurtainsBanner()
        {

            string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/customdrape/"));
            if (strGetfiles.Length > 0)
            {

                btndeletecustomcurtains.Visible = false;
                foreach (string strfl in strGetfiles)
                {
                    if (File.Exists(strfl))
                    {
                        FileInfo fl = new FileInfo(strfl);
                        imgcustomcurtainsname.Value = fl.Name.ToString();
                        imgcustomcurtains.Src = "/images/customdrape/" + fl.Name.ToString() + "";
                        ViewState["DelImagecustom"] = imgcustomcurtains.Src;
                        imgcustomcurtains.Visible = true;
                        btndeletecustomcurtains.Visible = true;
                        break;
                    }
                    else
                    {
                        ViewState["DelImagecustom"] = null;
                        btndeletecustomcurtains.Visible = false;
                    }
                }
            }


        }
        private void FillFabricCurtainsBanner()
        {

            string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/fabricdrape/"));
            if (strGetfiles.Length > 0)
            {

                btndeletefabriccurtains.Visible = false;
                foreach (string strfl in strGetfiles)
                {
                    if (File.Exists(strfl))
                    {
                        FileInfo fl = new FileInfo(strfl);
                        imgfabriccurtainsname.Value = fl.Name.ToString();
                        imgfabriccurtains.Src = "/images/fabricdrape/" + fl.Name.ToString() + "";
                        ViewState["DelImagefabric"] = imgfabriccurtains.Src;
                        imgfabriccurtains.Visible = true;
                        btndeletefabriccurtains.Visible = true;
                        break;
                    }
                    else
                    {
                        ViewState["DelImagefabric"] = null;
                        btndeletefabriccurtains.Visible = false;
                    }
                }
            }


        }

        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImagecustomCurtains(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(ImageName)))
                    File.Delete(Server.MapPath(ImageName));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ImageName));
                imgcustomcurtains.Visible = false;
                imgcustomcurtainsname.Value = "";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Deleted...', 'Message');});", true);
            }
            catch (Exception ex)
            {
                lblcustomcurtainsmsg.Text += "<br />" + ex.Message;
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }

        protected void DeleteImageFabricCurtains(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(ImageName)))
                    File.Delete(Server.MapPath(ImageName));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ImageName));
                imgfabriccurtains.Visible = false;
                imgfabriccurtainsname.Value = "";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Deleted...', 'Message');});", true);
            }
            catch (Exception ex)
            {
                lblfabriccurtainsmsg.Text += "<br />" + ex.Message;
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }
        protected void imgsavecustomcurtains_Click(object sender, ImageClickEventArgs e)
        {
            CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue='" + txtcustomcurtains.Text.Trim() + "' where ConfigName='CustomCurtainTitle' and StoreID=1");

            if (FileUploadcustomcurtains.FileName.Length > 0)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/customdrape/"));
                    imgcustomcurtains.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {

                            FileInfo fl = new FileInfo(strfl);

                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }

                            try
                            {
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath("/images/customdrape/" + fl.Name.ToString() + ""));
                            }
                            catch { }
                        }
                    }
                }
                catch { }




                if (FileUploadcustomcurtains.FileName.Length > 0)
                {

                    FileUploadcustomcurtains.PostedFile.SaveAs(Server.MapPath("/images/customdrape/" + FileUploadcustomcurtains.FileName + ""));
                    imgcustomcurtains.Visible = true;
                    btndeletecustomcurtains.Visible = true;
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/customdrape/" + FileUploadcustomcurtains.FileName));
                    }
                    catch
                    {

                    }
                    CommonOperations.SaveOnContentServer(Server.MapPath("/images/customdrape/" + FileUploadcustomcurtains.FileName + ""));
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Updated...', 'Message');});", true);
                FillCustomCurtainsBanner();

            }
            return;
        }

        protected void btndeletecustomcurtains_Click(object sender, ImageClickEventArgs e)
        {
            DeleteImagecustomCurtains(ViewState["DelImagecustom"].ToString());
            ViewState["DelImagecustom"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            // ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
            btndeletecustomcurtains.Visible = false;
        }

        protected void imgsavefabriccurtains_Click(object sender, ImageClickEventArgs e)
        {
            CommonComponent.ExecuteCommonData("update tb_AppConfig set ConfigValue='" + txtfabriccurtains.Text.Trim() + "' where ConfigName='FabricCurtainTitle' and StoreID=1");

            if (FileUploadfabriccurtains.FileName.Length > 0)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(Server.MapPath("/images/fabricdrape/"));
                    imgfabriccurtains.Src = "";
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {

                            FileInfo fl = new FileInfo(strfl);

                            try
                            {
                                if (File.Exists(strfl))
                                {
                                    File.Delete(strfl);
                                }
                            }
                            catch { }

                            try
                            {
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath("/images/fabricdrape/" + fl.Name.ToString() + ""));
                            }
                            catch { }
                        }
                    }
                }
                catch { }




                if (FileUploadfabriccurtains.FileName.Length > 0)
                {

                    FileUploadfabriccurtains.PostedFile.SaveAs(Server.MapPath("/images/fabricdrape/" + FileUploadfabriccurtains.FileName + ""));
                    imgfabriccurtains.Visible = true;
                    btndeletefabriccurtains.Visible = true;
                    try
                    {
                        CompressimagePanda objcompress = new CompressimagePanda();
                        objcompress.compressimage(Server.MapPath("/images/fabricdrape/" + FileUploadfabriccurtains.FileName));
                    }
                    catch
                    {

                    }
                    CommonOperations.SaveOnContentServer(Server.MapPath("/images/fabricdrape/" + FileUploadfabriccurtains.FileName + ""));
                }

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Banner Successfully Updated...', 'Message');});", true);
                FillFabricCurtainsBanner();

            }
            return;
        }

        protected void btndeletefabriccurtains_Click(object sender, ImageClickEventArgs e)
        {
            DeleteImageFabricCurtains(ViewState["DelImagefabric"].ToString());
            ViewState["DelImagefabric"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            // ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
            btndeletefabriccurtains.Visible = false;
        }

    }
}



