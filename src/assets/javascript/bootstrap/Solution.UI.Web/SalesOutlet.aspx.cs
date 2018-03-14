using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Data;
using Solution.Bussines.Components.Common;
using System.IO;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web
{
    public partial class SalesOutlet : System.Web.UI.Page
    {
        #region Declaration
        CategoryComponent objCategory = new CategoryComponent();
        ProductComponent objProduct = new ProductComponent();
        protected int counter = 1;
        protected int columnCount = 4;
        protected int pcounter = 1;
        protected int pcolumnCount = 3;
        private int RecordCount = 0;
        public string fullPath = string.Empty;
        PagedDataSource pgsource = new PagedDataSource();
        int findex, lindex;
        DataSet dscatchk = new DataSet();
        string Comparestr = "";
        string strView = "";
        Int32 itemCount = 6;
        Int32 TotalCount = 6;
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            fullPath = Server.UrlDecode(Request.RawUrl.ToLower());
            string AppPath = Request.Url.Scheme + "://" + Request.Url.Authority;
            if (AppLogic.AppConfigBool("UseLiveRewritePath"))
                AppPath += ":" + Request.Url.Port + Request.ApplicationPath.ToLower();
            else
                AppPath += Request.ApplicationPath.ToLower();

            if (fullPath.Contains(AppPath))
            {
                int IndexPath = fullPath.LastIndexOf(AppPath);
                fullPath = fullPath.Substring(IndexPath + AppPath.Length);
            }

            fullPath = "/" + fullPath.Trim("/".ToCharArray());

            if (fullPath.Contains("*")) //hr
                fullPath = fullPath.Substring(0, fullPath.IndexOf("*"));

            if (!IsPostBack)
            {
                try
                {

                    string[] strGetfiles = System.IO.Directory.GetFiles(AppLogic.AppConfigs("Live_Contant_Server_Path") + "/images/SaleClearanceBanner/");
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {
                            if (File.Exists(strfl))
                            {
                                FileInfo fl = new FileInfo(strfl);
                                imgBanner.Src = AppLogic.AppConfigs("Live_Contant_Server") + "/images/SaleClearanceBanner/" + fl.Name.ToString();
                                break;
                            }
                        }
                    }
                }
                catch { }

                if (Request.QueryString["CatID"] != null)
                {
                    Session["CategoryId"] = Convert.ToString(Request.QueryString["CatID"]);
                }
                if (Request.QueryString["CatPID"] != null)
                {
                    Session["ParentCategoryId"] = Convert.ToString(Request.QueryString["CatPID"]);
                    Session["HeaderCatid"] = Request.QueryString["CatPID"].ToString();
                }

                if (Session["HeaderCatid"] != null && Convert.ToInt32(Session["HeaderCatid"]) == 0)
                {
                    if (Request.QueryString["CatID"] != null)
                    {
                        Session["HeaderCatid"] = Request.QueryString["CatID"].ToString();
                    }
                }
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgload", "getReminingproductDefault('/ProductCompare.aspx');", true);

                BindProductOfSubCategory();





                if (ViewState["strFeaturedimg"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
                }
                if (ViewState["strFeaturedimglist"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);

                }
            }
            else
            {
                if (ViewState["strFeaturedimgpostback"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderpost", ViewState["strFeaturedimgpostback"].ToString(), true);
                }
                if (ViewState["strFeaturedimglistpostback"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderlistpost", ViewState["strFeaturedimglistpostback"].ToString(), true);
                }
            }

        }
        /// <summary>
        /// Replace the Space which is comes in ProductName to "-"
        /// </summary>
        /// <param name="Name">ProductName</param>
        /// <returns>return the ProductName with Replace the Space which is comes in ProductName to "-" </returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace("'", "&#39;").Replace('"', '-').Replace('\'', '-').ToString();
            //return Name.Replace('"', '-').Replace('\'', '-').Replace("'", "&#39;").ToString();
        }
        /// <summary>
        /// Get Category Image with Full Path
        /// </summary>
        /// <param name="img">Image Name</param>
        /// <returns>return Image with Full Path </returns>
        public String GetIconImageCategory(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathCategory") + "Icon/" + img;

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
            }

            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Get Product Image With Full Path
        /// </summary>
        /// <param name="img">Image Name</param>
        /// <returns>return Image with Full Path </returns>

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

        public String SetName(String Name)
        {
            if (Name.Length > 65)
                Name = Name.Substring(0, 62) + "...";
            return Server.HtmlEncode(Name);
        }


        public String SetDescription(String Name)
        {
            if (Name.Length > 565)
                Name = Name.Substring(0, 450) + "...";
            return (Name);
        }



        /// <summary>
        /// Bind Product of Subcategory
        /// </summary>
        private void BindProductOfSubCategory()
        {
            DataSet dsProduct = null;

            dsProduct = ProductComponent.GetProductDetailsSalesOutlet(0, Convert.ToInt32(AppLogic.AppConfigs("StoreID")), "Price Range");
            //dsProduct = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Product WHERE ProductId in (SELECT ProductId FROM tb_ProductVariantValue WHERE isnull(Buy1Get1,0) = 1) and isnull(active,0)=1 and isnull(deleted,0)=0 and Storeid=1");
            if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
            {

                dvMessage.Visible = false;
                topMiddle.Visible = true;

                TotalCount = Convert.ToInt32(dsProduct.Tables[0].Rows.Count);
                RepProduct.DataSource = dsProduct;
                RepProduct.DataBind();



                //divTopPaging.Visible = false;
                //divBottomPaging.Visible = false;
            }
            else
            {
                if (dscatchk != null && dscatchk.Tables.Count > 0 && dscatchk.Tables[0].Rows.Count > 0)
                {
                    dvMessage.Visible = false;
                }
                else
                {
                    dvMessage.Visible = true;
                }

                topMiddle.Visible = false;

            }
        }

        protected void RepProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Probox = (HtmlGenericControl)e.Item.FindControl("Probox");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");

                Label lblTName = (Label)e.Item.FindControl("lblTName");
                Literal lblTag = (Literal)e.Item.FindControl("lblTag");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");
                Literal litControl = (Literal)e.Item.FindControl("ltTop");
                HtmlImage outofStockDiv = (HtmlImage)e.Item.FindControl("outofStockDiv");
                HtmlAnchor innerAddtoCart = (HtmlAnchor)e.Item.FindControl("innerAddtoCart");
                HtmlImage imgAddToCart = (HtmlImage)e.Item.FindControl("imgAddToCart");
                Image imgName = (Image)e.Item.FindControl("imgName");

                Literal ltbottom = (Literal)e.Item.FindControl("ltbottom");

                #region Bind rating
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                DataSet dsComment = new DataSet();
                dsComment = ProductComponent.GetProductRating(Convert.ToInt32(ltrproductid.Text));
                Literal ltreviewDetail = (Literal)e.Item.FindControl("ltrating");
                Label ltrRatingCount = (Label)e.Item.FindControl("ltrRatingCount");

                Decimal rating = 0;
                HtmlGenericControl DivratingNew = (HtmlGenericControl)e.Item.FindControl("Divratinglist");
                HtmlGenericControl divSpace = (HtmlGenericControl)e.Item.FindControl("divSpace");


                decimal dd = 0;
                decimal ddnew = 0;

                if (dsComment != null && dsComment.Tables[0].Rows.Count > 0)
                {

                    rating = Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]);
                    ltrRatingCount.Text = dsComment.Tables[0].Rows[0]["AvgRating"].ToString();
                    DivratingNew.Visible = true;
                    divSpace.Visible = false;
                    dd = Math.Truncate(Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]));
                    ddnew = Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]) - dd;
                }
                else
                {
                    DivratingNew.Visible = false;
                    divSpace.Visible = true;
                }


                if (dd == Convert.ToDecimal(1))
                {
                    //ltreviewDetail.Text += "<img height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/25-star.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/rating-half.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/75-star.jpg\" >";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                    }
                    ltreviewDetail.Text += "<img src=\"/images/star-form1.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                }
                else if (dd == Convert.ToDecimal(2))
                {
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/25-star.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/rating-half.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/75-star.jpg\" >";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                    }

                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\">";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                }
                else if (dd == Convert.ToDecimal(3))
                {
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/25-star.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/rating-half.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/75-star.jpg\" >";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\">";
                    }


                    ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                }
                else if (dd == Convert.ToDecimal(4))
                {
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    //ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    ltreviewDetail.Text += "<img  src=\"/images/star-form.jpg\" >";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/25-star.jpg\" >";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/rating-half.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/75-star.jpg\" >";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img  src=\"/images/star-form1.jpg\" >";
                    }
                }
                else if (dd == Convert.ToDecimal(5))
                {
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form.png\">";
                }
                else
                {
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                    ltreviewDetail.Text += "<img  height=\"15\" width=\"15\" title=\"\" alt=\"\" src=\"/images/star-form1.png\">";
                }
                ltreviewDetail.Text += "";


                #endregion

                //if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                //{
                //    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                //    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                //    {
                //        if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                //        {
                // lblTag.Text = "<img width='41' height='42'  title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='new' />";
                lblTag.Text = "<img title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/bogof.png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='byu1get1list' />";
                //        }
                //    }
                //}
                if (ViewState["strFeaturedimg"] != null)
                {
                    // ViewState["strFeaturedimg"] = ViewState["strFeaturedimg"].ToString() + "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#" + imgName.ClientID.ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                }
                else
                {
                    // ViewState["strFeaturedimg"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#" + imgName.ClientID.ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                }
                if (ViewState["strFeaturedimgpostback"] != null)
                {
                    // ViewState["strFeaturedimgpostback"] = ViewState["strFeaturedimgpostback"].ToString() + "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                else
                {
                    //  ViewState["strFeaturedimgpostback"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                //if (e.Item.ItemIndex % 4 == 0)
                //{
                //    if (e.Item.ItemIndex != 0)
                //    {
                //        litControl.Text = "</div><div class=\"fp-bg\">";
                //    }
                //}

                //if ((e.Item.ItemIndex + 1) % 4 == 0 && e.Item.ItemIndex != 0)
                //{
                //    Probox.Attributes.Add("class", "fp-pro-1");
                //    //proDisplay.Attributes.Add("class", "pro-display-none");
                //}
                if ((e.Item.ItemIndex + 1) % itemCount == 0 && e.Item.ItemIndex != 0)
                {
                    //Probox.Attributes.Add("style", "margin-right:0px;");
                    ////proDisplay.Attributes.Add("class", "pro-display-none");
                    //litControl.Text = "</ul><ul><li>";
                    litControl.Text = "<li>";
                    ltbottom.Text = "</li>";
                    itemCount = itemCount + 5;
                }
                else
                {
                    litControl.Text = "<li>";
                    ltbottom.Text = "</li>";
                }

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");




                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrlink1");
                HtmlInputHidden ltrProductURL = (HtmlInputHidden)e.Item.FindControl("ltrProductURL");
                HtmlInputHidden hdnItemIndex = (HtmlInputHidden)e.Item.FindControl("hdnItemIndex");
                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");

                HtmlAnchor aProductlink = (HtmlAnchor)e.Item.FindControl("aProductlink");
                Decimal hdnprice = 0;
                Decimal SalePrice = 0;
                Decimal Price = 0;

                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);
                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);

                //if (Price > decimal.Zero)
                //{
                //    if (SalePrice > decimal.Zero && Price > SalePrice)
                //    {
                //        ltrRegularPrice.Text += " <p><del>" + Price.ToString("C") + "</del></p>";
                //        hdnprice = Price;
                //    }
                //    else
                //    {
                //        ltrRegularPrice.Text += "<div class=\"fp-sale-price\">" + Price.ToString("C") + "</div>";
                //        hdnprice = Price;
                //    }
                //}
                //else ltrRegularPrice.Text += "<p>&nbsp;</p>";
                //if (SalePrice > decimal.Zero && Price > SalePrice)
                //{
                //    ltrYourPrice.Text += "<div class=\"fp-sale-price\">" + SalePrice.ToString("C") + "</div>";
                //    hdnprice = SalePrice;
                //}
                ////else ltrYourPrice.Text += "<p>&nbsp;<strong>&nbsp;</strong></p>";

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

                if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                {

                    //Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(dbo.tb_ProductVariant.VariantID) FROM  dbo.tb_ProductVariant INNER JOIN dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID WHERE dbo.tb_ProductVariant.Productid=" + ltrproductid.Text.ToString() + " "));
                    //if (Count == 0)
                    //{
                    //    aProductlink.Attributes.Add("onclick", "adtocart('" + hdnprice.ToString() + "'," + ltrproductid.Text.ToString() + ");");
                    //}
                    //else
                    //{
                    //    aProductlink.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ");");
                    //}
                    DataSet dsVariant = new DataSet();
                    dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantID FROM tb_ProductVariant WHERE ProductID=" + ltrproductid.Text.ToString() + "");
                    if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                    {
                        //////aProductlink.Attributes.Remove("onclick");
                        //////aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                        ////aProductlink.HRef = "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                        //////aProductlink.Attributes.Remove("onclick");
                        ////aProductlink.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ");");
                        //aProductlink.HRef = "javascript:void(0);";
                        //aProductlink.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ",'" + aProductlink.ClientID.ToString() + "');");
                        ////aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";

                        //aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                        //innerAddtoCart.HRef = "javascript:void(0);";
                        //innerAddtoCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ",'" + aProductlink.ClientID.ToString() + "');");
                        //innerAddtoCart.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                        aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                        innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        imgAddToCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + "," + hdnItemIndex.Value.ToString() + "," + TotalCount.ToString() + ");");
                    }
                    else
                    {
                        // aProductlink.HRef = "javascript:void(0);";
                        // aProductlink.Attributes.Add("onclick", "InsertProductSubcategory(" + ltrproductid.Text.ToString() + ",'" + aProductlink.ClientID.ToString() + "');");
                        innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                        //innerAddtoCart.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";// "javascript:void(0);";
                        //innerAddtoCart.HRef = "javascript:void(0);";
                        //innerAddtoCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ",'" + aProductlink.ClientID.ToString() + "');");
                        imgAddToCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + "," + hdnItemIndex.Value.ToString() + "," + TotalCount.ToString() + ");");
                    }
                }
                else
                {

                    //// aProductlink.HRef = "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                    //aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                    aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                    aProductlink.Attributes.Remove("onclick");
                    aProductlink.Visible = true;
                    innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                    //innerAddtoCart.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                    innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                    imgAddToCart.Visible = false;
                    outofStockDiv.Visible = false;

                }

                #region Compare

                //CheckBox chkcompare = (CheckBox)e.Item.FindControl("add_to_compare");

                if (Session["CmpProductID"] != null)
                {
                    if (Session["CmpProductID"].ToString().Contains(ltrproductid.Text.ToString() + ","))
                    {
                        strView = "if(document.getElementById('add_to_compare" + ltrproductid.Text.ToString() + "') != null){document.getElementById('add_to_compare" + ltrproductid.Text.ToString() + "').checked = true;}";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloadchecked" + e.Item.ItemIndex.ToString(), strView, true);
                        //chkcompare.Checked = true;
                    }
                }

                #endregion
            }
        }
        #region Remove ViewState From page
        /// <summary>
        /// Remove ViewState From page.
        /// </summary>
        /// <returns></returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (Session[Session.SessionID] != null)
                return (new LosFormatter().Deserialize((string)Session[Session.SessionID]));
            return null;
        }

        /// <summary>
        /// Save Page State To Persistence Medium
        /// </summary>
        /// <param name="state">object state</param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            LosFormatter los = new LosFormatter();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            los.Serialize(sw, state);
            string vs = sw.ToString();
            Session[Session.SessionID] = vs;
        }
        #endregion

    }
}