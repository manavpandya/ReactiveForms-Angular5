using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Components;
using System.IO;
using System.Configuration;

namespace Solution.UI.Web
{
    public partial class Index : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        /// 

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            
            if (!IsPostBack)
            {

                AppLogic.ApplicationStart();
                AppConfig.StoreID = Convert.ToInt32(ConfigurationManager.AppSettings["GeneralStoreID"]);
                ltFeatureproductTitle.Text = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 Configvalue FROM tb_AppConfig WHERE configname='homepagefeatureproducttitle' and isnull(deleted,0)=0 and storeid=1"));
                BindFeaturedCategory();
                BindProducts();
                BindRightBanner();
                BindStaticData();
               // BindPattern();
                //BindFabric();
                BindColorOptions();
                BindPatternOptions();
                //BindStyle();
                //BindColors();
                //BindHeader();
                Session["IndexPriceValue"] = null;
                Session["IndexFabricValue"] = null;
                Session["IndexPatternValue"] = null;
                Session["IndexStyleValue"] = null;
                Session["IndexColorValue"] = null;
                Session["IndexHeaderValue"] = null;
                Session["IndexCustomValue"] = null;
                BindBanner();
                if (ViewState["strFeaturedimg"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloader", "var $j=jQuery.noConflict(); $j(document).ready(function () {" + ViewState["strFeaturedimg"].ToString() + "});", true);
                }
            }
            else
            {
                if (ViewState["strFeaturedimgpostback"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderpost", "var $j=jQuery.noConflict(); $j(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
                }

            }

        }

        #region Bind Right Banner, Products, FeaturedCategory
        /// <summary>
        /// Method for Bind  Colors
        /// </summary>
        private void BindColorOptions()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType =1 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");

            StrPattern = "<ul>";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                Int32 icheck = 0;
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

                    string strImageName = "";
                    String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages") + "HomePage/";
                    if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                        Directory.CreateDirectory(Server.MapPath(SearchProductPath));
                    string strFilePath = Server.MapPath(SearchProductPath + Imagename);
                    Random rnd = new Random();
                    if (File.Exists(strFilePath))
                    {
                        strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
                    }
                    else
                    {
                        dsPattern.Tables[0].Rows.RemoveAt(i);
                        i--;
                        dsPattern.Tables[0].AcceptChanges();
                        continue;
                    }



                    StrPattern += "<li>";
                    StrPattern += "<a href=\"javascript:void(0);\" onclick=\"ColorSelection('" + SearchValue.ToString() + "');\"><img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString().ToLower() +"\"></a>";
                    StrPattern += "</li>";
                    //StrPattern += "<span><a href=\"javascript:void(0);\" onclick=\"ColorSelection('" + SearchValue.ToString() + "');\">" + SearchValue.ToString() + "</a></span></li>";
                }

            }
            StrPattern += "</ul>";

            ltrColoroptions.Text = StrPattern.ToString();
        }

        private void BindPatternOptions()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType =2 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");

            StrPattern = "<ul>";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                Int32 icheck = 0;
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

                    string strImageName = "";
                    String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages") + "Medium/";
                    if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                        Directory.CreateDirectory(Server.MapPath(SearchProductPath));
                    string strFilePath = Server.MapPath(SearchProductPath + Imagename);
                    Random rnd = new Random();
                    if (File.Exists(strFilePath))
                    {
                        strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
                    }
                    else
                    {
                        dsPattern.Tables[0].Rows.RemoveAt(i);
                        i--;
                        dsPattern.Tables[0].AcceptChanges();
                        continue;
                    }



                    StrPattern += "<li>";
                    StrPattern += "<a href=\"javascript:void(0);\" onclick=\"CheckpatternSelection('" + SearchValue.ToString() + "');\"><img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString().ToLower() + "\"></a>";
                    StrPattern += "<span><a href=\"javascript:void(0);\" onclick=\"CheckpatternSelection('" + SearchValue.ToString() + "');\">" + SearchValue.ToString() + "</a></span></li>";
                }

            }
            StrPattern += "</ul>";

            ltrPatternoption.Text = StrPattern.ToString();
        }




        /// <summary>
        /// Method for Bind Banner on Page Header
        /// </summary>
        private void BindRightBanner()
        {
            //bind right side banner
            //DataSet dsRightbanner = TopicComponent.GetTopicList("Home-Right-Banner");
            //if (dsRightbanner != null && dsRightbanner.Tables.Count > 0 && dsRightbanner.Tables[0].Rows.Count > 0)
            //    dvBannerright.InnerHtml = Convert.ToString(dsRightbanner.Tables[0].Rows[0]["Description"]);

            Int32 ImgID = Convert.ToInt32(AppLogic.AppConfigs("HotDealProduct").ToString());
            string stritemurl = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT '/'+ MainCategory +'/'+ SeName FROM tb_Product WHERE Productid=" + ImgID + ""));
            //dvBannerright.InnerHtml = "<a href=\"" + stritemurl + "-" + ImgID.ToString() + ".aspx\"><img height='260' width='279' src=\"" + AppLogic.AppConfigs("ImagePathBanner") + "HotDeal/" + ImgID + ".jpg\" /></a>";
            dvBannerright.InnerHtml = "";

            //bind small banner
            DataSet dsSmallbanner = TopicComponent.GetTopicList("small-banner-row", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsSmallbanner != null && dsSmallbanner.Tables.Count > 0 && dsSmallbanner.Tables[0].Rows.Count > 0)
                dvSmallbannerrow.InnerHtml = Convert.ToString(dsSmallbanner.Tables[0].Rows[0]["Description"]);
        }

        /// <summary>
        /// Method for Bind Product by Different Criteria BestSeller, Featured
        /// </summary>
        private void BindProducts()
        {
            /////Bind Best Seller
            //DataSet dsBestSeller = ProductComponent.DisplyProductByOption("IsBestSeller", Convert.ToInt32(AppLogic.AppConfigs("StoreID")), 4);
            //if (dsBestSeller != null && dsBestSeller.Tables.Count > 0 && dsBestSeller.Tables[0].Rows.Count > 0)
            //{
            //    rptBestSeller.DataSource = dsBestSeller;
            //    rptBestSeller.DataBind();
            //}

            ////Bind Feature
            //DataSet dsFeatured = ProductComponent.DisplyProductByOption("IsFeatured", Convert.ToInt32(AppLogic.AppConfigs("StoreID")), 18);
            //if (dsFeatured != null && dsFeatured.Tables.Count > 0 && dsFeatured.Tables[0].Rows.Count > 0)
            //{
            //    rptFeaturedProduct.DataSource = dsFeatured;
            //    rptFeaturedProduct.DataBind();
            //}
        }

        /// <summary>
        /// Method for Bind Feature Category 
        /// </summary>
        private void BindFeaturedCategory()
        {
            //DataSet dsFeaturedCategory = CategoryComponent.GetFeaturedCategory(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            //if (dsFeaturedCategory != null && dsFeaturedCategory.Tables.Count > 0 && dsFeaturedCategory.Tables[0].Rows.Count > 0)
            //{
            //    rptFeaturedCategory.DataSource = dsFeaturedCategory;
            //    rptFeaturedCategory.DataBind();
            //}
        }

        #endregion

        /// <summary>
        /// Feature Category Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void rptFeaturedCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Catbox = (HtmlGenericControl)e.Item.FindControl("Catbox");
                HtmlGenericControl catDisplay = (HtmlGenericControl)e.Item.FindControl("catDisplay");

                if ((e.Item.ItemIndex + 1) % 3 == 0 && e.Item.ItemIndex != 0)
                {
                    //Catbox.Attributes.Add("class", "cat-box");
                    //Catbox.Attributes.Add("style", "margin-right:0;");
                    Catbox.Attributes.Add("class", "cat-box1");
                }
                //else
                //    Catbox.Attributes.Add("class", "cat-box");
            }
        }

        /// <summary>
        /// Featured Product Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void rptFeaturedProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Probox = (HtmlGenericControl)e.Item.FindControl("Probox");
                //HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");
                Label lblTagName = (Label)e.Item.FindControl("lblTagImageName");
                Literal lblTagImage = (Literal)e.Item.FindControl("lblTagImage");
                HtmlInputHidden lblFreeEngraving = (HtmlInputHidden)e.Item.FindControl("lblFreeEngraving");
                Literal lblFreeEngravingImage = (Literal)e.Item.FindControl("lblFreeEngravingImage");
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");


                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());
                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTagName != null && !string.IsNullOrEmpty(lblTagName.Text.ToString().Trim()) && lblTagName.Text.ToString().ToLower().IndexOf("bestseller") > -1)
                        {
                            //string StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ltrproductid.Text.ToString() + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 ";
                            //Int32 Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                            //if (Intcnt > 0)
                            //{
                            lblTagImage.Text = "<img title='Best Seller' src=\"/images/BestSeller_new.png\" alt=\"Best Seller\" class='bestseller' />";
                            //}
                        }
                        else if (lblTagName != null && !string.IsNullOrEmpty(lblTagName.Text.ToString().Trim()))
                        {
                            //lblTagImage.Text = "<img title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".jpg\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "' style='position: absolute; top: 110px; left: 98px;' width='62' height='17'  />";
                            lblTagImage.Text = "<img title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".png\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "' />";
                        }
                        else
                        {


                            string StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ltrproductid.Text.ToString() + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 ";
                            Int32 Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                            if (Intcnt > 0)
                            {
                                lblTagImage.Text = "<img title='Sale' src=\"/images/bestseller.png\" alt=\"Sale\" class='bestseller' />";
                            }
                        }

                    }

                }
                //else
                //{

                //    string StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + ltrproductid.Text.ToString() + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 ";
                //    Int32 Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                //    if (Intcnt > 0)
                //    {
                //        lblTagImage.Text = "<img title='Sale' src=\"/images/bestseller.png\" alt=\"Sale\" class='BestSeller' />";
                //    }
                //}

                if (ViewState["strFeaturedimg"] != null)
                {
                    // ViewState["strFeaturedimg"] = ViewState["strFeaturedimg"].ToString() + "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                    //ViewState["strFeaturedimg"] = ViewState["strFeaturedimg"].ToString() + "$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();});";
                }
                else
                {
                    //ViewState["strFeaturedimg"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                    // ViewState["strFeaturedimg"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();});";
                }
                if (ViewState["strFeaturedimgpostback"] != null)
                {
                    // ViewState["strFeaturedimgpostback"] = ViewState["strFeaturedimgpostback"].ToString() + "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                else
                {
                    // ViewState["strFeaturedimgpostback"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                //if ((e.Item.ItemIndex + 1) % 4 == 0 && e.Item.ItemIndex != 0)
                //{
                //    Probox.Attributes.Add("style", "padding-right: 0px;");
                //}

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrLink1");
                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");

                HtmlAnchor aFeaturedLink = (HtmlAnchor)e.Item.FindControl("aFeaturedLink");
                Decimal hdnprice = 0;
                Decimal SalePrice = 0;
                Decimal Price = 0;

                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);
                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);

                if (Price > decimal.Zero)
                {
                    ltrRegularPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";// "<p>Regular Price: " + Price.ToString("C") + "</p>";
                    hdnprice = Price;
                }
                else ltrRegularPrice.Text += "<span>&nbsp;</span>";
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "Starting Price: <span>" + SalePrice.ToString("C") + "</span>";// "<p>Your Price: <strong>" + SalePrice.ToString("C") + "</strong></p>";
                    hdnprice = SalePrice;
                }
                else ltrYourPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";


                if (lblFreeEngraving.Value == "True")
                {
                    lblFreeEngravingImage.Text = "<img title='Free Engraving' src=\"/images/FreeEngraving.jpg\" alt=\"Free Engraving\" style='position: absolute; top: 110px; left: 4px;'/>";
                }



                //if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                //{

                //    Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(dbo.tb_ProductVariant.VariantID) FROM  dbo.tb_ProductVariant INNER JOIN dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID WHERE dbo.tb_ProductVariant.Productid=" + ltrproductid.Text.ToString() + " "));
                //    if (Count == 0)
                //    {
                //        aFeaturedLink.Attributes.Add("onclick", "adtocart('" + hdnprice.ToString() + "'," + ltrproductid.Text.ToString() + ");");
                //    }
                //    else
                //    {
                //        aFeaturedLink.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ");");
                //    }
                //}
                //else
                //{
                aFeaturedLink.Attributes.Remove("onclick");
                aFeaturedLink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //}
            }
        }

        /// <summary>
        /// Best Seller Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void rptBestSeller_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl ProboxBestSeller = (HtmlGenericControl)e.Item.FindControl("ProboxBestSeller");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");
                Label lblTagName = (Label)e.Item.FindControl("lblTagImageName");
                Literal lblTagImage = (Literal)e.Item.FindControl("lblTagImage");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");

                if ((e.Item.ItemIndex + 1) % 4 == 0 && e.Item.ItemIndex != 0)
                {
                    ProboxBestSeller.Attributes.Add("style", "padding-right: 0px;");
                }
                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTagName != null && !string.IsNullOrEmpty(lblTagName.Text.ToString().Trim()))
                        {
                            lblTagImage.Text = "<img title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".jpg\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "'  />";
                        }
                    }
                }

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrlink1");

                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                HtmlAnchor abestLink = (HtmlAnchor)e.Item.FindControl("abestLink");
                Decimal hdnprice = 0;
                Decimal SalePrice = 0;
                Decimal Price = 0;

                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);
                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);

                if (Price > decimal.Zero)
                {
                    ltrRegularPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";//"<p>Regular Price: " + Price.ToString("C") + "</p>";
                    hdnprice = Price;
                }
                else
                {
                    ltrRegularPrice.Text += "<span>&nbsp;</span>";
                }
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "Starting Price: <span>" + SalePrice.ToString("C") + "</span>";//"<p>Your Price: <strong>" + SalePrice.ToString("C") + "</strong></p>";
                    hdnprice = SalePrice;
                }
                else
                {
                    ltrYourPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";
                }
                //if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                //{

                //    Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(dbo.tb_ProductVariant.VariantID) FROM  dbo.tb_ProductVariant INNER JOIN dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID WHERE dbo.tb_ProductVariant.Productid=" + ltrproductid.Text.ToString() + " "));
                //    if (Count == 0)
                //    {
                //        abestLink.Attributes.Add("onclick", "adtocart('" + hdnprice.ToString() + "'," + ltrproductid.Text.ToString() + ");");
                //    }
                //    else
                //    {
                //        abestLink.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ");");
                //    }
                //}
                //else
                //{
                abestLink.Attributes.Remove("onclick");
                abestLink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //}
            }
        }

        #region Get Icon Image

        /// <summary>
        /// Get Icon Image for Category
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Icon Category Image Full Path</returns>
        public String GetIconImageCategory(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathCategory") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Icon Product Image Full Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + imagepath))
            {
                return AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        #endregion

        #region SetName

        /// <summary>
        /// Add '...', if String length is more than 50 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 50 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 50)
                Name = Name.Substring(0, 47) + "...";
            return Server.HtmlEncode(Name);
        }

        /// <summary>
        /// Sets the Category Path
        /// </summary>
        /// <param name="SEName">String SEName</param>
        /// <param name="ParentSEName">String ParentSEName</param>
        /// <returns>Returns the Full Category path</returns>
        public String SetCategoryPath(String SEName, String ParentSEName)
        {
            string CategoryPath = "";
            if (ParentSEName.Length > 0)
                CategoryPath = "/" + ParentSEName + "/" + SEName;
            else CategoryPath = "/" + SEName;
            return CategoryPath;
        }

        #endregion

        #region Remove ViewState From Page


        /// <summary>
        /// Loads any saved view-state information to the <see cref="T:System.Web.UI.Page" /> object.
        /// </summary>
        /// <returns>The saved view state.</returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (Session[Session.SessionID] != null)
                return (new LosFormatter().Deserialize((string)Session[Session.SessionID]));
            return null;
        }

        /// <summary>
        /// Saves any view-state and control-state information for the page.
        /// </summary>
        /// <param name="state">The state.</param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            LosFormatter los = new LosFormatter();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            los.Serialize(sw, state);
            string vs = sw.ToString();
            Session[Session.SessionID] = vs;
        }

        #endregion

        /// <summary>
        /// Add Customer for Cart item Temporary
        /// </summary>
        private void AddCustomer()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            Solution.Bussines.Entities.tb_Customer objCust = new Solution.Bussines.Entities.tb_Customer();
            Int32 CustID = -1;
            CustID = objCustomer.InsertCustomer(objCust, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            Session["CustID"] = CustID.ToString();
            System.Web.HttpCookie custCookie = new System.Web.HttpCookie("ecommcustomer", CustID.ToString());
            custCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(custCookie);
        }

        /// <summary>
        ///  Add to Cart Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddToCart_Click(object sender, EventArgs e)
        {
            bool IsRestricted = true;
            IsRestricted = CheckCustomerIsRestricted();
            if (!IsRestricted)
            {
                if (Session["CustID"] == null || Session["CustID"].ToString() == "")
                {
                    AddCustomer();
                }

                string[] strkey = Request.Form.AllKeys;
                string VariantValueId = "";
                string VariantNameId = "";

                if (hdnproductId.Value.ToString() != "" && Session["CustID"] != null)
                {
                    ShoppingCartComponent objShopping = new ShoppingCartComponent();
                    decimal price = Convert.ToDecimal(hdnPrice.Value);
                    int Qty = Convert.ToInt32(1);
                    string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(hdnproductId.Value.ToString()), Qty, price, VariantNameId, VariantValueId, "", "", 0);
                    if (strResult.ToLower() == "success")
                    {
                        if (Session["NoOfCartItems"] == null)
                        {
                            Session["NoOfCartItems"] = Qty.ToString();
                        }
                        Response.Redirect("/addtoCart.aspx");
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message');", true);
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Customer is restricted.', 'Message','');", true);

            }
        }

        /// <summary>
        /// Check Customer is restricted or not.
        /// </summary>
        /// <returns> Returns True or False</returns>
        private bool CheckCustomerIsRestricted()
        {
            bool IsRestrictedCust = false;
            IsRestrictedCust = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT RestrictedIPID FROM tb_RestrictedIP WHERE IPAddress='" + Request.UserHostAddress.ToString() + "'"));
            return IsRestrictedCust;
        }

        /// <summary>
        /// Replace the Space which is comes in ProductName to "-"
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>return the ProductName with Replace the '"' and '\' which is comes in ProductName to "-" </returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace('"', '-').Replace('\'', '-').ToString();
        }


        public String SetStaticDescription(String Name)
        {
            if (Name.Length > 250)
                Name = Name.Substring(0, 247) + "...";
            else
                Name = Name + "...";
            return (Name.Replace("<p>", "").Replace("</p>", ""));
        }
        /// <summary>
        /// Binds the Static Data
        /// </summary>
        private void BindStaticData()
        {
            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList("designnotesblog", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            dsTopic = CommonComponent.GetCommonDataSet("SELECT * FROM tb_HomesmallBannerDetail WHERE isnull(active,0)=1 Order By DisplayOrder ASC");
            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsTopic.Tables[0].Rows.Count; i++)
                {
                    ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<div class=\"index-banner-main" + (i + 1).ToString() + "\">";
                    ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<div class=\"index-banner\">";
                    if (dsTopic.Tables[0].Rows[i]["Title"].ToString().ToLower().IndexOf("social media") > -1)
                    {
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<a class=\"index-houzz-icon\" href=\"https://www.houzz.com/pro/exclusivefabrics/half-price-drapes\" target=\"_blank\">";
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<img src=\"/images/index-houzz.jpg\" />";
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "</a>";

                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<a class=\"index-facebook-icon\" href=\"https://www.facebook.com/halfpricedrapes\" target=\"_blank\">";
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<img src=\"/images/index-facebook.jpg\" />";
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "</a>";

                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<a class=\"index-pinterest-icon\" href=\"https://pinterest.com/halfpricedrapes/\" target=\"_blank\">";
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<img src=\"/images/index-pinterest.jpg\" />";
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "</a>";

                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<a class=\"index-twitter-icon\" href=\"https://twitter.com/halfpricedrapes\" target=\"_blank\">";
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<img src=\"/images/index-twitter.jpg\" />";
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "</a>";

                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[i]["BannerURL"].ToString()))
                        {
                            ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<a href=\"" + dsTopic.Tables[0].Rows[i]["BannerURL"].ToString() + "\" traget=\"" + dsTopic.Tables[0].Rows[i]["Pagination"].ToString() + "\" title=\"" + Server.HtmlEncode(dsTopic.Tables[0].Rows[i]["Title"].ToString()) + "\">";
                            ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString() + AppLogic.AppConfigs("ImagePathSmallBanner").ToString() + dsTopic.Tables[0].Rows[i]["ImageName"].ToString() + "\" alt=\"" + Server.HtmlEncode(dsTopic.Tables[0].Rows[i]["Title"].ToString()) + "\" title=\"" + Server.HtmlEncode(dsTopic.Tables[0].Rows[i]["Title"].ToString()) + "\" class=\"img-left\" />";
                            ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "</a>";
                        }
                        else
                        {
                            ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString() + AppLogic.AppConfigs("ImagePathSmallBanner").ToString() + dsTopic.Tables[0].Rows[i]["ImageName"].ToString() + "\" alt=\"" + Server.HtmlEncode(dsTopic.Tables[0].Rows[i]["Title"].ToString()) + "\" title=\"" + Server.HtmlEncode(dsTopic.Tables[0].Rows[i]["Title"].ToString()) + "\" class=\"img-left\" />";
                        }
                    }
                    ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "</div>";
                    ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<div class=\"index-banner-bg\">";
                    string[] strSpace = dsTopic.Tables[0].Rows[i]["Title"].ToString().Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (strSpace.Length > 1)
                    {
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<h3>" + dsTopic.Tables[0].Rows[i]["Title"].ToString().Replace(strSpace[strSpace.Length - 1].ToString(), "") + "<span>" + strSpace[strSpace.Length - 1].ToString() + "</span></h3>";
                    }
                    else
                    {
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<h3>" + dsTopic.Tables[0].Rows[i]["Title"].ToString() + "</h3>";
                    }


                    ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<p>" + SetStaticDescription(dsTopic.Tables[0].Rows[i]["Description"].ToString()) + "..</p>";
                    if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[i]["BannerURL"].ToString()))
                    {
                        ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "<a href=\"" + dsTopic.Tables[0].Rows[i]["BannerURL"].ToString() + "\" target=\"" + dsTopic.Tables[0].Rows[i]["Pagination"].ToString() + "\" title=\"Read More\">Read More</a>";
                    }
                    ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "</div>";
                    ltrHeadCatalogContent.Text = ltrHeadCatalogContent.Text.ToString() + "</div>";

                }
            }

        }

        private void BindHeader()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 5 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel7\" class=\"jcarousel-skin-tango2\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                Int32 icheck = 0;
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

                    string strImageName = "";
                    String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
                    if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                        Directory.CreateDirectory(Server.MapPath(SearchProductPath));
                    string strFilePath = Server.MapPath(SearchProductPath + Imagename);
                    Random rnd = new Random();
                    if (File.Exists(strFilePath))
                    {
                        strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
                    }
                    else
                    {
                        dsPattern.Tables[0].Rows.RemoveAt(i);
                        i--;
                        dsPattern.Tables[0].AcceptChanges();
                        continue;
                    }
                    if (icheck == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro\">";
                    }
                    if (icheck % 3 == 0 && icheck > 2)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro\">";
                        }
                    }
                    icheck++;
                    StrPattern += "<li class=\"header-pro-box\">";
                    StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkHeader_" + SearchId + "\">";
                    StrPattern += "<img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString().ToLower() + "\"><span style=\"padding-left: 16px;\">" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrHeader.Text = StrPattern.ToString();
        }

        private void BindPattern()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType=2 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel3\" class=\"jcarousel-skin-tango2\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro\">";
                    }
                    if (i % 8 == 0 && i > 7)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro\">";
                        }
                    }

                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    StrPattern += "<li class=\"pattern-pro-box\">";
                    StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkPattern_" + SearchId + "\">";
                    StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrPattern.Text = StrPattern.ToString();
        }

        private void BindFabric()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 3 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel4\" class=\"jcarousel-skin-tango2\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro\">";
                    }
                    if (i % 8 == 0 && i > 7)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro\">";
                        }
                    }

                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    StrPattern += "<li class=\"pattern-pro-box\">";
                    StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkFabric_" + SearchId + "\">";
                    StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrFabric.Text = StrPattern.ToString();
        }

        private void BindStyle()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 4 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel5\" class=\"jcarousel-skin-tango2\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro\">";
                    }
                    if (i % 8 == 0 && i > 7)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro\">";
                        }
                    }

                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    StrPattern += "<li class=\"pattern-pro-box\">";
                    StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkStyle_" + SearchId + "\">";
                    StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrStyle.Text = StrPattern.ToString();
        }

        private void BindColors()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType =1 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
            StrPattern = "<div class=\"toggle1\">";
            StrPattern += "<ul id=\"mycarousel2\" class=\"jcarousel-skin-tango0\">";
            if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
            {
                Int32 icheck = 0;
                for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
                {
                    string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
                    string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
                    string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

                    string strImageName = "";
                    String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
                    if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                        Directory.CreateDirectory(Server.MapPath(SearchProductPath));
                    string strFilePath = Server.MapPath(SearchProductPath + Imagename);
                    Random rnd = new Random();
                    if (File.Exists(strFilePath))
                    {
                        strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
                    }
                    else
                    {
                        dsPattern.Tables[0].Rows.RemoveAt(i);
                        i--;
                        dsPattern.Tables[0].AcceptChanges();
                        continue;
                    }
                    if (icheck == 0)
                    {
                        StrPattern += "<li><ul class=\"option-pro-color\" style=\"width:92px !important;\">";
                    }
                    if (icheck % 10 == 0 && icheck > 9)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro-color\" style=\"width:92px !important;\">";
                        }
                    }

                    icheck++;
                    StrPattern += "<li class=\"option-pro-box\"  style=\"padding-bottom:4px !important;\">";
                    StrPattern += "<a href=\"javascript:void(0);\" onclick=\"ColorSelection('" + SearchValue.ToString() + "');\"><img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString().ToLower() + "\"></a> </li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrColor.Text = StrPattern.ToString();
        }

        protected void btnIndexPriceGo_Click(object sender, ImageClickEventArgs e)
        {
            Session["IndexPriceValue"] = null;
            Session["IndexFabricValue"] = null;
            Session["IndexPatternValue"] = null;
            Session["IndexStyleValue"] = null;
            Session["IndexColorValue"] = null;
            Session["IndexHeaderValue"] = null;
            Session["IndexCustomValue"] = null;

            if (!string.IsNullOrEmpty(txtFrom.Text.ToString().Trim()) || !string.IsNullOrEmpty(txtTo.Text.ToString().Trim()))
            {
                if (string.IsNullOrEmpty(txtFrom.Text.ToString().Trim()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Please Enter Valid Price.');document.getElementById('ContentPlaceHolder1_txtFrom').focus();", true);
                    return;
                }
                if (string.IsNullOrEmpty(txtTo.Text.ToString().Trim()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Tovalcal", "alert('Please Enter Valid Price.');document.getElementById('ContentPlaceHolder1_txtTo').focus();", true);
                    return;
                }
                decimal FromVal = Convert.ToDecimal(txtFrom.Text.Trim());
                decimal ToVal = Convert.ToDecimal(txtTo.Text.Trim());
                if (FromVal > ToVal)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Low Price should be Less than High Price.');document.getElementById('ContentPlaceHolder1_txtFrom').focus();", true);
                    return;
                }
                Session["IndexPriceValue"] = FromVal.ToString() + "-" + ToVal.ToString();
            }

            string[] strkey = Request.Form.AllKeys;

            foreach (string strkeynew in strkey)
            {
                if (strkeynew.ToString().ToLower().IndexOf("chkprice") > -1 && Session["IndexPriceValue"] == null)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexPriceValue"] += ChkValue[0];
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkpattern") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexPatternValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkfabric") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexFabricValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkstyle") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexStyleValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkheader") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexHeaderValue"] += ChkValue[0] + ",";
                }
                if (strkeynew.ToString().ToLower().IndexOf("chkcustom") > -1)
                {
                    string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
                    if (ChkValue.Length > 0)
                        Session["IndexCustomValue"] += ChkValue[0].ToString();
                }
            }

            if (!string.IsNullOrEmpty(hdnColorSelection.Value) && hdnColorSelection.Value != "")
            {
                Session["IndexColorValue"] = hdnColorSelection.Value.ToString();
            }
            hdnColorSelection.Value = "";

            if (Session["IndexPriceValue"] == null && Session["IndexFabricValue"] == null && Session["IndexPatternValue"] == null && Session["IndexStyleValue"] == null && Session["IndexColorValue"] == null && Session["IndexHeaderValue"] == null && Session["IndexCustomValue"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Please Select Search Criteria.');", true);
                return;
            }
            else
            {
                Response.Redirect("/ProductSearchList.aspx");
            }
        }
        protected void btnIndexPriceGo1_Click(object sender, ImageClickEventArgs e)
        {
            Session["IndexPriceValue"] = null;
            Session["IndexFabricValue"] = null;
            Session["IndexPatternValue"] = null;
            Session["IndexStyleValue"] = null;
            Session["IndexColorValue"] = null;
            Session["IndexHeaderValue"] = null;
            Session["IndexCustomValue"] = null;

            string[] strkey = Request.Form.AllKeys;

            //foreach (string strkeynew in strkey)
            //{
            //    if (strkeynew.ToString().ToLower().IndexOf("chkprice") > -1 && Session["IndexPriceValue"] == null)
            //    {
            //        string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
            //        if (ChkValue.Length > 0)
            //            Session["IndexPriceValue"] += ChkValue[0];
            //    }
            //    if (strkeynew.ToString().ToLower().IndexOf("chkpattern") > -1)
            //    {
            //        string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
            //        if (ChkValue.Length > 0)
            //            Session["IndexPatternValue"] += ChkValue[0] + ",";
            //    }
            //    if (strkeynew.ToString().ToLower().IndexOf("chkfabric") > -1)
            //    {
            //        string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
            //        if (ChkValue.Length > 0)
            //            Session["IndexFabricValue"] += ChkValue[0] + ",";
            //    }
            //    if (strkeynew.ToString().ToLower().IndexOf("chkstyle") > -1)
            //    {
            //        string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
            //        if (ChkValue.Length > 0)
            //            Session["IndexStyleValue"] += ChkValue[0] + ",";
            //    }
            //    if (strkeynew.ToString().ToLower().IndexOf("chkheader") > -1)
            //    {
            //        string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
            //        if (ChkValue.Length > 0)
            //            Session["IndexHeaderValue"] += ChkValue[0] + ",";
            //    }
            //    if (strkeynew.ToString().ToLower().IndexOf("chkcustom") > -1)
            //    {
            //        string[] ChkValue = Request.Form.GetValues(strkeynew.ToString());
            //        if (ChkValue.Length > 0)
            //            Session["IndexCustomValue"] += ChkValue[0].ToString();
            //    }
            //}

            if (!string.IsNullOrEmpty(hdnColorSelection.Value) && hdnColorSelection.Value != "")
            {
                Session["IndexColorValue"] = hdnColorSelection.Value.ToString();
            }
            hdnColorSelection.Value = "";

            string header = "";
            if (!string.IsNullOrEmpty(hdnpattern.Value) && hdnpattern.Value != "")
            {
                Session["IndexPatternValue"] = hdnpattern.Value.ToString() + ",";
                header = hdnpattern.Value.ToString();
            }
            hdnpattern.Value = "";

            if (Session["IndexPriceValue"] == null && Session["IndexFabricValue"] == null && Session["IndexPatternValue"] == null && Session["IndexStyleValue"] == null && Session["IndexColorValue"] == null && Session["IndexHeaderValue"] == null && Session["IndexCustomValue"] == null)
            {
                Response.Redirect("/ProductSearchList.aspx");
            }
            else
            {
                string strUrl = "";
                if (Session["IndexColorValue"] != null)
                {
                    strUrl = CommonOperations.RemoveSpecialCharacter(Session["IndexColorValue"].ToString().Trim().ToCharArray()).ToLower() + ".html";
                    Response.Redirect(strUrl);
                }
                else if (!string.IsNullOrEmpty(header))
                {
                    strUrl = CommonOperations.RemoveSpecialCharacter(header.ToString().Trim().ToCharArray()).ToLower() + ".html";
                    Response.Redirect(strUrl);
                }
                else
                {
                    Response.Redirect("/ProductSearchList.aspx");
                }



            }
        }
        private void BindBanner()
        {

            DataTable dtHomeBannerDetail = new DataTable();

            bool flgchk = false;
            bool flgleft = false;
            Int32 MainBannerCount = 1;


            dtHomeBannerDetail = CommonComponent.GetCommonDataSet("SELECT  * FROM tb_HomeSmallBottomBannerDetail  WHERE  StoreId=1 and isnull(Active,0)=1 and cast(StartDate as date) <= cast(getdate() as date) and cast(EndDate as date) >= cast(getdate() as date) Order By DisplayOrder").Tables[0];
            if (dtHomeBannerDetail != null && dtHomeBannerDetail.Rows.Count > 0)
            {
                hidesmallBannernew.Visible = true;
                hidesmallBannernew.InnerHtml = "<div id='small-banner-border'><div class=\"vector-line\"></div><div class=\"small-banner-main\">";

                Random rd = new Random();

                Int32 iCount = 1;
                for (int i = 0; i < dtHomeBannerDetail.Rows.Count; i++)
                {



                    if (iCount <= 3)
                    {



                        if (flgleft == false)
                        {
                            if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
                            {
                                hidesmallBannernew.InnerHtml += "<div class=\"small-banner-new-" + iCount.ToString() + "\"><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathSmallbottomBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></a></div>";
                            }
                            else
                            {
                                hidesmallBannernew.InnerHtml += "<div class=\"small-banner-new-" + iCount.ToString() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathSmallbottomBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></div>";
                            }
                            flgleft = true;
                            iCount++;
                        }
                        else
                        {
                            if (Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]) != "" && Convert.ToString(dtHomeBannerDetail.Rows[i]["StoreID"]) != "")
                            {
                                hidesmallBannernew.InnerHtml += "<div class=\"small-banner-new-" + iCount.ToString() + "\"><a href=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["BannerURL"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathSmallbottomBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></a></div>";
                            }
                            else
                            {
                                hidesmallBannernew.InnerHtml += "<div class=\"small-banner-new-" + iCount.ToString() + "\"><img src=\"" + AppLogic.AppConfigs("Live_Contant_Server").ToString().ToLower() + AppLogic.AppConfigs("ImagePathSmallbottomBanner").ToString().ToLower() + "" + dtHomeBannerDetail.Rows[i]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "\" alt=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\" title=\"" + Convert.ToString(dtHomeBannerDetail.Rows[i]["Title"]).Trim() + "\"></div>";
                            }
                            iCount++;
                        }
                    }

                }


                hidesmallBannernew.InnerHtml += "</div></div>";
            }





        }
    }
}