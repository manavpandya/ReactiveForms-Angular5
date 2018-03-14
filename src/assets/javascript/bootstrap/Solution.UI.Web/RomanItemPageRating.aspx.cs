using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using System.IO;
using System.Xml;
using Solution.Bussines.Components.Common;
using System.Web.UI.HtmlControls;
using StringBuilder = System.Text.StringBuilder;
using System.Text.RegularExpressions;

namespace Solution.UI.Web
{
    public partial class RomanItemPageRating : System.Web.UI.Page
    {
        public static int MaxLenEngraving = 0;

        #region Variable

        string StartPathParent = "";
        string EndPathParent = "";
        string strSeNameJoin = "'";
        string StartPath = "";
        string EndPath = "";
        string StrReplace = "";
        string StrSEName = "";
        Int32 itemCount = 8;
        public string strScriptVar = "";
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            string rURL = AppLogic.AppConfigs("LIVE_SERVER");
            CommonOperations.RedirectWithSSL(false);
            Page.MaintainScrollPositionOnPostBack = true;
            divPostComment.Visible = true;

            if (!IsPostBack)
            {
                if (Request.RawUrl != null)
                {
                    aback.HRef = Request.RawUrl.ToString().ToLower().Trim().Replace("-review", "");
                }
                if (Request.QueryString["PID"] != null)
                {
                    Session["HeaderCatid"] = null;
                    Session["HeaderSubCatid"] = null;
                    btnWishList.OnClientClick = "return InsertProductWishlist(" + Request.QueryString["PID"].ToString() + ",'" + btnWishList.ClientID + "');";
                    btnAddTocartSwatch.OnClientClick = "InsertProductSwatch(" + Request.QueryString["PID"].ToString() + ",'" + btnAddTocartSwatch.ClientID.ToString() + "'); return false;";
                    btnAddTocartMade.OnClientClick = "InsertProductCustom(" + Request.QueryString["PID"].ToString() + ",'" + btnAddTocartMade.ClientID.ToString() + "'); return false;";

                    string strProductid = Convert.ToString(Request.QueryString["PID"]);
                    int Productid = 0;
                    bool isNum = int.TryParse(strProductid, out Productid);
                    if (isNum)
                    {
                        ViewState["HotDealProduct"] = AppLogic.AppConfigs("HotDealProduct").ToString();
                        if (Convert.ToInt32(ViewState["HotDealProduct"]) == Productid)
                        {
                            divDeal.Visible = true;
                            divDeal1.Visible = true;
                            btnWishList.Visible = false;


                        }

                        ProductComponent objRecer = new ProductComponent();
                        objRecer.UpdateProductForView(Productid);
                        GetProductDetails(Productid);
                        GetnextPreviousProduct();
                        BindCartDetails();

                        FillQuantityDiscountTable(Productid.ToString(), Convert.ToString(Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
                        BindProducts();

                        BindProductProperty();
                        BindProductQuote();

                        GetColorOption();
                        //BindEngravingFonts();
                        string RecentViewProductID = AddRecentViewProduct(Request.QueryString["PID"].ToString());

                        if (RecentViewProductID != "")
                            GetRecentlyProduct(RecentViewProductID, Convert.ToInt32(Request.QueryString["PID"].ToString()));

                        GetMakeOrderWidth();
                        GetMakeOrderStyle();
                        GetMakeOrderLength();
                        GetMakeOrderOptions();
                        GetMakeOrderQuantity();
                        GetMeasuringData();
                        GetFaqData();
                        GetPleatGuideData();
                    }
                    else
                    {
                        Response.Redirect("/index.aspx");
                    }

                    if (Session["HeaderCatid"] == null || Session["HeaderCatid"].ToString() == "" || Session["HeaderCatid"].ToString() == "0")
                    { GetparentCategory(); }

                    string strscript = "";
                    if (ViewState["strRecentlyimg"] != null)
                    {
                        strscript = ViewState["strRecentlyimg"].ToString();
                    }
                    if (ViewState["strYoumayimg"] != null)
                    {
                        strscript += ViewState["strYoumayimg"].ToString();
                    }
                    if (ViewState["strcolroptionimg"] != null)
                    {
                        strscript += ViewState["strcolroptionimg"].ToString();
                    }

                    if (ViewState["strAccesoriesimg"] != null)
                    {
                        strscript += ViewState["strAccesoriesimg"].ToString();
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "tadisplayscript", "$(document).ready(function () {" + strscript.ToString() + "});", true);
                }
                else
                {
                    Response.Redirect("/index.aspx");
                }
                breadcrumbs();
                ShowHidePortions();


            }
            else
            {

                Master.BindHeaderLink();

                if (ViewState["VariantNameId"] != null && ViewState["VariantNameId"].ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "dropdwnselect", "SelectVariantBypostback('" + ViewState["VariantNameId"].ToString() + "','" + ViewState["VariantValueId"].ToString() + "');", true);

                }
                string strscript = "";
                if (ViewState["strRecentlyimgpostback"] != null)
                {
                    strscript = ViewState["strRecentlyimgpostback"].ToString();
                }
                if (ViewState["strYoumayimgpostback"] != null)
                {
                    strscript += ViewState["strYoumayimgpostback"].ToString();
                }
                if (ViewState["strcolroptionimgpostback"] != null)
                {
                    strscript += ViewState["strcolroptionimgpostback"].ToString();
                }
                if (ViewState["strAccesoriesimgpostback"] != null)
                {
                    strscript += ViewState["strAccesoriesimgpostback"].ToString();
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "tadisplayscript", "$(document).ready(function () {" + strscript.ToString() + "});", true);

            }
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidefooter", "$('#divhalfpricedrapes').css('display','none');$('#divhalfpricedrapes').html('');", true);

        }

        /// 
        /// <summary>
        /// Bind Style
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns the Recently view ProductIDs as a String </returns>
        /// 
        private void GetStyle()
        {
            DataSet dsstyle = new DataSet();
            dsstyle = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductSearchType WHERE isnull(Active,0)=1 AND isnull(Deleted,0)=0 AND SearchType=6 Order By displayorder");
            ddlcustomstyle.Items.Clear();
            ltstyleop.Text = "";
            if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
            {
                ddlcustomstyle.DataSource = dsstyle;
                ddlcustomstyle.DataTextField = "SearchValue";
                ddlcustomstyle.DataValueField = "SearchValue";

                ddlcustomstyle.DataBind();
                ddlcustomstyle.Items.Insert(0, new ListItem("Header", "0"));
            }
            else
            {
                ddlcustomstyle.DataSource = null;
                ddlcustomstyle.DataBind();
            }
            if (ddlcustomstyle.Items.Count > 0)
            {
                for (int i = 1; i < ddlcustomstyle.Items.Count; i++)
                {
                    ltstyleop.Text += "<div style=\"float:left;width:100%;margin-bottom:5px;\" class='item-radio-display'><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvaluecustom('" + ddlcustomstyle.ClientID.ToString() + "','" + ddlcustomstyle.Items[i].Value.ToString() + "','divcustomflat-radio-001-" + i.ToString() + "','001');ChangeCustomprice();\" id=\"divcustomflat-radio-001-" + i.ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + ddlcustomstyle.Items[i].Text.ToString() + "</div></div>";
                }
            }
        }
        private void GetOption()
        {
            DataSet dsstyle = new DataSet();
            dsstyle = CommonComponent.GetCommonDataSet(@"SELECT case when isnull(tb_ProductOptionsPrice.AdditionalPrice,0)=0 then tb_ProductOptionsPrice.Options else tb_ProductOptionsPrice.Options+'($'+cast(Round(tb_ProductOptionsPrice.AdditionalPrice,2) as varchar(10))+')'  end as name, tb_ProductOptionsPrice.ProductId, tb_ProductSearchType.DisplayOrder
FROM dbo.tb_ProductOptionsPrice INNER JOIN dbo.tb_ProductSearchType ON dbo.tb_ProductOptionsPrice.Options = dbo.tb_ProductSearchType.SearchValue WHERE tb_ProductOptionsPrice.ProductId=" + Request.QueryString["PID"].ToString() + @" Order By tb_ProductSearchType.DisplayOrder");
            ddlcustomoptin.Items.Clear();
            ltoptoins.Text = "";
            if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
            {
                ddlcustomoptin.DataSource = dsstyle;
                ddlcustomoptin.DataTextField = "name";
                ddlcustomoptin.DataValueField = "name";

                ddlcustomoptin.DataBind();
                ddlcustomoptin.Items.Insert(0, new ListItem("Options", "0"));
            }
            else
            {
                ddlcustomoptin.DataSource = null;
                ddlcustomoptin.DataBind();
            }
            if (ddlcustomoptin.Items.Count > 0)
            {
                for (int i = 1; i < ddlcustomoptin.Items.Count; i++)
                {
                    ltoptoins.Text += "<div style=\"float:left;width:100%;margin-bottom:5px;\" class='item-radio-display'><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvaluecustom('" + ddlcustomoptin.ClientID.ToString() + "','" + ddlcustomoptin.Items[i].Value.ToString() + "','divcustomflat-radio-004-" + i.ToString() + "','004');ChangeCustomprice();\" id=\"divcustomflat-radio-004-" + i.ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + ddlcustomoptin.Items[i].Text.ToString() + "</div></div>";
                }
            }
        }
        private string AddRecentViewProduct(string ProductID)
        {
            string RecentlyPid = "";
            if (Session["RecentViewProduct"] != null && Session["RecentViewProduct"].ToString() != "")
            {
                if (!Session["RecentViewProduct"].ToString().Contains(ProductID))
                {
                    Session["RecentViewProduct"] += ProductID + ",";
                }
            }
            else
            {
                Session["RecentViewProduct"] += ProductID + ",";
            }

            string[] strrecent = (Session["RecentViewProduct"].ToString()).TrimEnd(',').Split(',');

            if (strrecent.Count() > 1)
            {
                if (Session["RecentViewProduct"].ToString().Contains(ProductID))
                {
                    //find Position
                    int Postion = -1;
                    for (int i = 0; i < strrecent.Count() - 1; i++)
                    {
                        if (ProductID == strrecent[i].ToString())
                        {
                            Postion = i;
                        }
                    }
                    for (int i = 0; i < strrecent.Count(); i++)
                    {
                        if (Postion != i)
                        {
                            RecentlyPid += strrecent[i].ToString() + ",";
                        }
                    }

                    if (Postion > -1)
                        RecentlyPid += strrecent[Postion].ToString() + ",";

                    Session["RecentViewProduct"] = RecentlyPid;
                }
            }

            string[] strrecentFinal = (Session["RecentViewProduct"].ToString()).TrimEnd(',').Split(',');
            if (strrecentFinal.Count() > 4)
            {
                for (int i = 1; i < strrecentFinal.Count(); i++)
                {
                    RecentlyPid += strrecentFinal[i].ToString() + ",";
                }
                RecentlyPid = RecentlyPid.ToString().Substring(0, (RecentlyPid.ToString().Length - 1));
            }
            else
            {
                RecentlyPid = Session["RecentViewProduct"].ToString().Substring(0, (Session["RecentViewProduct"].ToString().Length - 1));
            }

            return RecentlyPid;
        }

        private void ShowHidePortions()
        {
            hdnIsShowImageZoomer.Value = Convert.ToString(AppLogic.AppConfigs("SwitchItemImageZoom")).Trim().ToLower();

            #region Here is BookMark,Ptirnt Page, ad To wishlist etc links

            String strPriceMatch = Convert.ToString(AppLogic.AppConfigs("SwitchItemPriceMatch"));
            String strPrintPage = Convert.ToString(AppLogic.AppConfigs("SwitchItemPrintThisPage"));
            String strWishList = Convert.ToString(AppLogic.AppConfigs("SwitchItemAddToWishlist"));
            String strEmailFriend = Convert.ToString(AppLogic.AppConfigs("SwitchItemTellAFriend"));
            String strBookMark = Convert.ToString(AppLogic.AppConfigs("SwitchItemBookMarkPage"));

            if (strPriceMatch.Trim().ToLower() == "false" && strPrintPage.Trim().ToLower() == "false" && strWishList.Trim().ToLower() == "false" && strEmailFriend.Trim().ToLower() == "false" && strBookMark.Trim().ToLower() == "false")
            {
                divBookmarkLinks.Visible = false;
            }
            else
            {
                divBookmarkLinks.Visible = true;
                if (strPriceMatch.Trim().ToLower() == "false")
                {
                    lnkPriceMatch.Visible = false;
                    lblafterpricematch.Visible = false;
                }
                else
                {
                    //lnkPriceMatch.Attributes.Add("onclick", "OpenCenterWindow('/Pricematch.aspx?ProductId=" + Convert.ToString(Request.QueryString["PID"]) + "',700,700);");
                    lnkPriceMatch.Attributes.Add("onclick", "ShowModelForPriceMatch('/Pricematch.aspx?ProductId=" + Request.QueryString["PID"].ToString() + "');");
                }

                if (strPrintPage.Trim().ToLower() == "false")
                {
                    lnkPrintThisPage.Visible = false;
                    lblafterprintpage.Visible = false;
                }
                else
                {
                    lnkPrintThisPage.Attributes.Add("onclick", "OpenCenterWindow('/itemprint.aspx?PID=" + Convert.ToString(Request.QueryString["PID"]) + "', 790,625);");
                }

                if (strWishList.Trim().ToLower() == "false")
                {
                    btnWishList.Visible = false;
                    lblafterwishlist.Visible = false;
                }

                if (strEmailFriend.Trim().ToLower() == "false")
                {
                    lnkTellFriend.Visible = false;
                    lblaftertellfriend.Visible = false;
                }
                else
                {
                    lnkTellFriend.HRef = "/TellAFriend.aspx?title=EMAILAFRIEND&ProductID=" + Convert.ToString(Request.QueryString["PID"]);
                }

                if (strBookMark.Trim().ToLower() == "false")
                {
                    lnkBookMark.Visible = false;
                    lblaftertellfriend.Visible = false;
                }
                else
                {
                    lnkBookMark.Attributes.Add("onclick", "return addthis_sendto();");
                    lnkBookMark.Attributes.Add("onmouseout", "addthis_close();");
                    lnkBookMark.Attributes.Add("onmouseover", "return addthis_open(this, '', '[URL]', '[TITLE]');");
                    lnkBookMark.HRef = "http://www.addthis.com/bookmark.php";
                }


            }
            #endregion

            #region Hide More Images
            if (Convert.ToString(AppLogic.AppConfigs("SwitchItemViewMoreImages")).Trim().ToLower() == "false")
            {
                hideMoreImages.Visible = false;
            }
            #endregion

            #region Hide Product Review
            if (Convert.ToString(AppLogic.AppConfigs("SwitchItemProductReview")).Trim().ToLower() == "false")
            {
                divProductReview.Visible = false;
            }
            #endregion

            #region Hide  Social links
            if (Convert.ToString(AppLogic.AppConfigs("SwitchItemSocialLink")).Trim().ToLower() == "false")
            {
                divSocialLinks.Visible = false;
            }
            #endregion

            #region Hide Related Products
            if (Convert.ToString(AppLogic.AppConfigs("SwitchItemRelatedProducts")).Trim().ToLower() == "false")
            {
                divYoumay.Visible = false;
            }
            #endregion

            #region Hide Related Products
            if (Convert.ToString(AppLogic.AppConfigs("SwitchItemRecentlyViewedProducts")).Trim().ToLower() == "false")
            {
                divrecently.Visible = false;
            }
            #endregion
        }

        /// <summary>
        /// Page Pre-render Event For Dynamic Variant Details
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_PreRender(object sender, EventArgs e)
        {

            //if (!IsPostBack)
            //{
            if (Request.QueryString["PID"] != null)
            {
                string strProductid = Convert.ToString(Request.QueryString["PID"]);
                int Productid = 0;
                bool isNum = int.TryParse(strProductid, out Productid);
                if (isNum)
                {
                    //GetVarinatByProductID(Productid);
                }
            }
            //}
        }

        /// <summary>
        /// Get breadcrumbs
        /// </summary>
        private void breadcrumbs()
        {
            try
            {
                string pc = "0";
                string cc = "0";
                if (Request.QueryString["PID"] != null)
                {
                    String SelectQuery = "";
                    SelectQuery = " SELECT  top 1 tb_ProductCategory.CategoryID,ParentCategoryID FROM tb_ProductCategory " +
                    " INNER JOIN tb_Product ON tb_ProductCategory.ProductID =tb_Product.ProductID  " +
                    " INNER JOIN tb_Category ON tb_ProductCategory.CategoryID = tb_Category.CategoryID  " +
                    " INNER join tb_CategoryMapping On tb_Category.CategoryID= tb_CategoryMapping.CategoryID WHERE  tb_Category.StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " And  tb_Product.StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " And tb_Category.Deleted=0 and  (tb_ProductCategory.ProductID = " + Request.QueryString["PID"] + ") ";

                    DataSet dsCommon = new DataSet();
                    dsCommon = CommonComponent.GetCommonDataSet(SelectQuery);

                    if (dsCommon != null && dsCommon.Tables.Count > 0 && dsCommon.Tables[0].Rows.Count > 0)
                    {
                        if (!string.IsNullOrEmpty(dsCommon.Tables[0].Rows[0]["CategoryID"].ToString()) && !string.IsNullOrEmpty(dsCommon.Tables[0].Rows[0]["ParentCategoryID"].ToString()))
                        {
                            pc = dsCommon.Tables[0].Rows[0]["ParentCategoryID"].ToString();
                            cc = dsCommon.Tables[0].Rows[0]["CategoryID"].ToString();
                            ltBreadcrmbs.Text = ConfigurationComponent.GetBreadCrum(Convert.ToInt32(dsCommon.Tables[0].Rows[0]["CategoryID"].ToString()), Convert.ToInt32(dsCommon.Tables[0].Rows[0]["ParentCategoryID"].ToString()), 1, "", 3, 0);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(ltBreadcrmbs.Text.ToString()))
                {

                    if (ltBreadcrmbs.Text.LastIndexOf("<a href") > -1)
                    {
                        StartPathParent = ltBreadcrmbs.Text.Substring(0, ltBreadcrmbs.Text.LastIndexOf("<a href") - 1);
                        EndPathParent = ltBreadcrmbs.Text.Substring(ltBreadcrmbs.Text.LastIndexOf("'/"));
                        strSeNameJoin = EndPathParent.Remove(EndPathParent.IndexOf("' title="));
                    }
                    StartPath = ltBreadcrmbs.Text.Substring(0, ltBreadcrmbs.Text.LastIndexOf("<span>") - 1);
                    EndPath = ltBreadcrmbs.Text.Substring(ltBreadcrmbs.Text.LastIndexOf("<span>"));
                    StrReplace = EndPath.Replace("<span> ", "").Replace(" </span>", "");
                    StrSEName = CommonOperations.RemoveSpecialCharacter(StrReplace.Trim().ToLower().Replace("&#32;", " ").Replace("&amp;", " ").ToCharArray());
                    ltBreadcrmbs.Text = StartPath + " <a href='/" + StrSEName + ".html' title='" + StrReplace + "'>" + StrReplace + "</a>";
                }
                else
                {
                    ltBreadcrmbs.Text = "<a href=\"/\" title=\"Home\">Home </a><img src=\"/images/breadcrumbs-bullet.png\" alt=\"\" title=\"\" class=\"breadcrumbs-bullet\">";

                    if (pc != "0")
                    {
                        DataSet pDs = new DataSet();
                        pDs = CommonComponent.GetCommonDataSet("SELECT name,sename FROM tb_Category WHERE Categoryid=" + pc + "");
                        if (pDs != null && pDs.Tables.Count > 0 && pDs.Tables[0].Rows.Count > 0)
                        {
                            ltBreadcrmbs.Text += "<a href=\"/" + pDs.Tables[0].Rows[0]["sename"].ToString() + ".html\" title=\"" + Server.HtmlEncode(pDs.Tables[0].Rows[0]["name"].ToString()) + "\">" + pDs.Tables[0].Rows[0]["name"].ToString() + "</a><img src=\"/images/breadcrumbs-bullet.png\" alt=\"\" title=\"\" class=\"breadcrumbs-bullet\">";
                        }
                    }
                    if (cc != "0")
                    {
                        DataSet pDs = new DataSet();
                        pDs = CommonComponent.GetCommonDataSet("SELECT name,sename FROM tb_Category WHERE Categoryid=" + cc + "");
                        if (pDs != null && pDs.Tables.Count > 0 && pDs.Tables[0].Rows.Count > 0)
                        {
                            ltBreadcrmbs.Text += "<a href=\"/" + pDs.Tables[0].Rows[0]["sename"].ToString() + ".html\" title=\"" + Server.HtmlEncode(pDs.Tables[0].Rows[0]["name"].ToString()) + "\">" + pDs.Tables[0].Rows[0]["name"].ToString() + "</a>";
                        }

                    }


                }
            }
            catch
            {
            }
        }

        #region Get Product Details By product ID

        /// <summary>
        /// Get Product Details By product ID
        /// </summary>
        /// <param name="productID">int product ID</param>
        private void GetProductDetails(int productID)
        {
            DataSet dsproduct = new DataSet();
            dsproduct = ProductComponent.GetProductDetailByID(productID, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
            {
                BindStaticData(Convert.ToString(dsproduct.Tables[0].Rows[0]["ShippingTime"]));
                BindFabricSwatchBanner(Convert.ToString(dsproduct.Tables[0].Rows[0]["SKU"]));

                bool IsCustomProductId = false;
                bool.TryParse(dsproduct.Tables[0].Rows[0]["iscustom"].ToString(), out IsCustomProductId);

                Int32 Swatchid = 0;
                bool isSwatch = Int32.TryParse(dsproduct.Tables[0].Rows[0]["ProductSwatchid"].ToString(), out Swatchid);
                if (Swatchid <= 0)
                {
                    divswatch.Visible = false;
                    liswatch.Visible = false;
                    btnAddTocartSwatch.Visible = false;
                }
                else
                {
                    btnAddTocartSwatch.Visible = true;
                    if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Ismadetoorderswatch"].ToString()) && Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["Ismadetoorderswatch"].ToString()))
                    {

                        liswatch.Visible = false;
                        //btnAddTocartSwatch.Visible = true;
                    }
                    else
                    {
                        liswatch.Visible = false;
                        //btnAddTocartSwatch.Visible = false;
                    }
                    DataSet DataSwatch = new DataSet();
                    Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
                    //DataSwatch = objSql.GetDs("SELECT Sename, isnull(salePrice,0) as salePrice,isnull(Price,0) as Price,name,Sename,ImageName,SwatchDescription,MainCategory,isnull(Inventory,0) as Inventory,isnull(ProductUrl,'') as ProductUrl FROM tb_product WHERE productId=" + Swatchid + "");
                    DataSwatch = objSql.GetDs("SELECT ISNULL(SalePriceTag,'') as SalePriceTag,Sename, isnull(salePrice,0) as salePrice,isnull(Price,0) as Price,name,Sename,ImageName,case when isnull(SwatchDescription,'')='' then  Description else SwatchDescription end as SwatchDescription,MainCategory,isnull(Inventory,0) as Inventory,isnull(ProductUrl,'') as ProductUrl FROM tb_product WHERE productId=" + Swatchid + "");

                    if (DataSwatch != null && DataSwatch.Tables.Count > 0 && DataSwatch.Tables[0].Rows.Count > 0)
                    {
                        //btnAddTocartSwatch.OnClientClick = "InsertProductSwatch(" + Swatchid.ToString() + ",'" + btnAddTocartSwatch.ClientID.ToString() + "'); return false;";
                        btnAddTocartSwatch.OnClientClick = "InsertProductSwatch(" + Swatchid.ToString() + ",'" + btnAddTocartSwatch.ClientID.ToString() + "'); return false;";
                        //if (Convert.ToInt32(DataSwatch.Tables[0].Rows[0]["Inventory"].ToString()) > 0)
                        //{
                        Decimal swatchprice = Convert.ToDecimal(DataSwatch.Tables[0].Rows[0]["Price"]);
                        Decimal swatchsaleprice = Convert.ToDecimal(DataSwatch.Tables[0].Rows[0]["salePrice"]);
                        ltOrderswatch.Text = Convert.ToString(DataSwatch.Tables[0].Rows[0]["SwatchDescription"].ToString());
                        //aswatchurl.HRef = "/" + Convert.ToString(DataSwatch.Tables[0].Rows[0]["MainCategory"].ToString()) + "/" + Convert.ToString(DataSwatch.Tables[0].Rows[0]["Sename"].ToString()) + "-" + Swatchid.ToString() + ".aspx";
                        aswatchurl.HRef = "/" + Convert.ToString(DataSwatch.Tables[0].Rows[0]["ProductUrl"].ToString());

                        string StrSalePriceTag = "";
                        if (!string.IsNullOrEmpty(DataSwatch.Tables[0].Rows[0]["SalePriceTag"].ToString()))
                            StrSalePriceTag = "<span style=\"padding-left: 2px;\">(" + Convert.ToString(DataSwatch.Tables[0].Rows[0]["SalePriceTag"]) + ")</span>";

                        if (swatchprice > decimal.Zero)
                        {
                            if (swatchsaleprice == decimal.Zero)
                            {
                                ltSwatchPrice.Text = " <span>Swatch Price :</span> <strong style='color: #B92127;font-size:16px;'>" + Convert.ToDecimal(DataSwatch.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>" + StrSalePriceTag;
                                hdnswatchprice.Value = Convert.ToString(DataSwatch.Tables[0].Rows[0]["price"].ToString());
                            }
                            else if (swatchprice > swatchsaleprice)
                            {

                                ltSwatchPrice.Text = " <span>Swatch Price :</span> <strong style='color: #B92127;font-size:16px;'>" + Convert.ToDecimal(swatchsaleprice).ToString("C") + "</strong>" + StrSalePriceTag;
                                hdnswatchprice.Value = Convert.ToString(swatchsaleprice);
                            }
                            else
                            {
                                ltSwatchPrice.Text = " <span>Swatch Price :</span> <strong style='color: #B92127;font-size:16px;'>" + Convert.ToDecimal(DataSwatch.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>" + StrSalePriceTag;
                                hdnswatchprice.Value = Convert.ToString(DataSwatch.Tables[0].Rows[0]["price"].ToString());
                            }
                            Random rd1 = new Random();
                            if (!String.IsNullOrEmpty(DataSwatch.Tables[0].Rows[0]["Imagename"].ToString()))
                            {
                                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(AppLogic.AppConfigs("ImagePathProduct") + "micro/" + DataSwatch.Tables[0].Rows[0]["Imagename"].ToString())))
                                {
                                    imgswatchproduct.Src =AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "micro/" + DataSwatch.Tables[0].Rows[0]["Imagename"].ToString();
                                }
                                else
                                {
                                    imgswatchproduct.Src =AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "micro/image_not_available.jpg?" + rd1.Next(10000).ToString();
                                }
                            }
                            else
                            {
                                imgswatchproduct.Src =AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "micro/image_not_available.jpg?" + rd1.Next(10000).ToString();
                            }

                        }
                        //}
                        //else
                        //{
                        //    divswatch.Visible = false;
                        //    liswatch.Visible = false;
                        //    btnAddTocartSwatch.Visible = false;
                        //}
                    }
                }

                ltrProductName.Text = Convert.ToString(dsproduct.Tables[0].Rows[0]["name"].ToString());

                try
                {
                    GetOptinalProductDetails(Convert.ToString(dsproduct.Tables[0].Rows[0]["OptionalAccessories"]));
                }
                catch { }

                try
                {
                    GetOptinalAccesories(Convert.ToString(dsproduct.Tables[0].Rows[0]["OptionalAccessories"]));
                }
                catch { }

                // Start - For Fill Secondary Color Option

                string SalePriceTag = "";
                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["SalePriceTag"].ToString()))
                {
                    SalePriceTag = Convert.ToString(dsproduct.Tables[0].Rows[0]["SalePriceTag"].ToString());
                    hdnSalePriceTag.Value = "(" + Convert.ToString(dsproduct.Tables[0].Rows[0]["SalePriceTag"].ToString()) + ")";
                }
                else
                {
                    hdnSalePriceTag.Value = "(Per Panel)";
                }

                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["SecondaryColor"].ToString()))
                {
                    String strSecondaryColorNames = Convert.ToString(dsproduct.Tables[0].Rows[0]["SecondaryColor"]);
                    Int32 FoundImages = 0;

                    String strColorSku = Convert.ToString(dsproduct.Tables[0].Rows[0]["colorsku"].ToString());
                    string[] splitstrColorSku = strColorSku.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] splitstrsecColor = strSecondaryColorNames.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    String strResult = "";
                    String strPath = AppLogic.AppConfigs("SecondaryColorImages");

                    DataSet dsColor = new DataSet();
                    dsColor = ProductComponent.GetSecondaryColorsImagePath(strSecondaryColorNames.Trim());
                    if (dsColor != null && dsColor.Tables.Count > 0 && dsColor.Tables[0].Rows.Count > 0)
                    {

                        strResult += "<ul class=\"item-color-option\">";
                        for (int i = 0; i < dsColor.Tables[0].Rows.Count; i++)
                        {
                            string strImagePath = strPath.Trim() + "/" + Convert.ToString(dsColor.Tables[0].Rows[i]["ImageName"]);
                            if (!File.Exists(Server.MapPath(strImagePath.Trim())))
                            {
                                continue;
                            }
                            FoundImages++;
                            //strResult += "<li><a href=\"javascript:void(0);\" style=\"cursor:default;\"><img  src=\"" + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dsColor.Tables[0].Rows[i]["ColorName"]) + "\" style=\"border;solid 1px #dddddd;\"></a></li>";
                            string strhref = "javascript:void(0);";
                            string strstyle = "default";
                            for (int k = 0; k < splitstrsecColor.Length; k++)
                            {
                                if (splitstrColorSku.Length > 0 && splitstrColorSku.Length >= k && splitstrsecColor[k] != null && splitstrsecColor[k].ToString().ToLower() == dsColor.Tables[0].Rows[i]["ColorName"].ToString().ToLower())
                                {
                                    if (splitstrColorSku[k] != null && splitstrColorSku[k].ToString() != "~")
                                    {

                                        strhref = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 ProductUrl FROM tb_product WHERE SKU='" + splitstrColorSku[k].ToString().Replace("'", "''") + "' AND Active=1 AND Deleted=0 AND StoreId=1"));
                                        if (!string.IsNullOrEmpty(strhref))
                                        {
                                            strhref = "/" + strhref;
                                            strstyle = "pointer";
                                        }
                                        else
                                        {
                                            strhref = "javascript:void(0);";
                                        }
                                    }
                                    break;
                                }
                            }
                            strResult += "<li><a href=\"" + strhref + "\" style=\"cursor:" + strstyle + ";\"><img  src=\"" + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dsColor.Tables[0].Rows[i]["ColorName"]) + "\" style=\"border;solid 1px #dddddd;\"></a></li>";

                        }
                        strResult += " </ul>";
                    }
                    if (strResult.Trim() != "" && FoundImages > 0)
                    {
                        ltrSecondoryColors.Text = strResult.ToString();
                        //divColorOption.Visible = true;
                    }
                    else
                    {
                        ltrSecondoryColors.Text = "";
                        divColorOption.Visible = false;
                    }
                }
                // Stop - For Fill Secondary Color Option

                decimal price = 0;
                decimal salePrice = 0;
                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["price"].ToString()))
                {
                    price = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString());
                }
                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()))
                {
                    salePrice = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString());
                }
                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["PDFName"].ToString()))
                {
                    ViewState["PdfPath"] = AppLogic.AppConfigs("ImagePathProduct") + "/PDF/" + Convert.ToString(dsproduct.Tables[0].Rows[0]["PDFName"].ToString());
                    if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(ViewState["PdfPath"].ToString())))
                    {
                        btnDownload.Visible = true;
                    }
                }
                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["EngravingSize"].ToString()))
                {
                    MaxLenEngraving = Convert.ToInt32(dsproduct.Tables[0].Rows[0]["EngravingSize"].ToString());
                }

                if (price > decimal.Zero)
                {
                    if (salePrice == decimal.Zero)
                    {
                        ltRegularPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>";
                        if (!string.IsNullOrEmpty(hdnSalePriceTag.Value.ToString()) && hdnSalePriceTag.Value.ToString() != "")
                        {
                            //ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>-.--</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                        }
                        else
                        {
                            // ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>";
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>-.--</strong>";
                        }
                        hdncustomprice.Value = price.ToString();
                        hdnpricecustomcart.Value = price.ToString();
                    }
                    else if (price > salePrice)
                    {
                        //ltRegularPrice.Text = "<div id=\"spnRegularPrice\" class='item-right-pt1-right' style=\"Color:#484848;font-size: 12px;float:left; font-weight: normal;\">" + " : " + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</div>";
                        //ltRegularPrice.Text = "<tt>MSRP :</tt> <span style=\"font-size:13px !important;\">" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</span>";
                        ltRegularPrice.Text = "";
                        if (!string.IsNullOrEmpty(hdnSalePriceTag.Value.ToString()) && hdnSalePriceTag.Value.ToString() != "")
                        {
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>-.--</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                            //ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + salePrice.ToString("C") + "</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                        }
                        else
                        {
                            //ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + salePrice.ToString("C") + "</strong>";
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>-.--</strong>";
                        }
                        hdncustomprice.Value = salePrice.ToString();
                        hdnpricecustomcart.Value = salePrice.ToString();
                    }

                    else
                    {
                        //ltRegularPrice.Text = "<div id=\"spnRegularPrice\" class='item-right-pt1-right' style=\"Color:#484848;font-size: 12px;float:left; font-weight: normal;\">" + " : " + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</div>";
                        //ltRegularPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>";
                        ltRegularPrice.Text = "";
                        if (!string.IsNullOrEmpty(hdnSalePriceTag.Value.ToString()) && hdnSalePriceTag.Value.ToString() != "")
                        {
                            //ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>-.--</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                        }
                        else
                        {
                            // ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>";
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>-.--</strong>";
                        }
                        hdncustomprice.Value = price.ToString();
                        hdnpricecustomcart.Value = price.ToString();
                    }
                    ltRegularPriceforShippop.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C");
                    hdnActual.Value = price.ToString();
                    hdnprice.Value = price.ToString();
                    hdnSaleActual.Value = price.ToString();
                }
                else
                {
                    hdnprice.Value = "0";
                    divRegularPrice.Visible = false;
                }

                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["IsFreeEngraving"].ToString()))
                {
                    if (Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["IsFreeEngraving"].ToString()))
                    {
                        divParent.Visible = true;
                    }
                    else { divParent.Visible = false; }
                }

                if (salePrice > decimal.Zero)
                {
                    if (price > salePrice)
                    {
                        if (ViewState["HotDealProduct"] != null && Convert.ToInt32(ViewState["HotDealProduct"]) == productID)
                        {
                            ltYourPrice.Text = "";
                            ltYourPriceforshiopop.Text = "";
                            //ltYourPrice.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C");
                            //ltYourPriceforshiopop.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C");
                            //ltYourPrice.Text = "<div class='item-right-pt1-right' id=\"spnYourPrice\">" + "<span style='color:#484848; padding-left:0px;'> : <span style='padding-left:0px;'> " + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C") + "</div>";
                        }
                        else
                        {
                            //ltYourPrice.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C");
                            //ltYourPriceforshiopop.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C");

                            ltYourPrice.Text = "";
                            ltYourPriceforshiopop.Text = "";

                            //ltYourPrice.Text = "<div class='item-right-pt1-right' id=\"spnYourPrice\">" + "<span style='color:#484848; padding-left:0px;'> : <span style='padding-left:0px;'> " + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C") + "</div>";
                        }
                        hdnsaleprice.Value = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString();
                        hdnprice.Value = salePrice.ToString();
                        decimal youSave = price - salePrice;
                        decimal Yousavepercentage = (Convert.ToDecimal(100) * youSave) / price;
                        ltYouSave.Text = "" + youSave.ToString("C") + "<span style='padding-left:0px; padding-left:0px;color:#B92127;'> (" + Math.Round(Yousavepercentage, 2) + "%)</span>";
                        //ltYouSave.Text = "<span>" + youSave.ToString("C") + "</span> (" + Math.Round(Yousavepercentage, 2) + "%)";
                        hdnYousave.Value = Yousavepercentage.ToString();
                        hdnSaleActual.Value = salePrice.ToString();
                    }
                    else
                    {
                        hdnsaleprice.Value = "0";
                        //divYourPrice.Visible = false;
                        divYouSave.Visible = false;
                    }

                }
                else
                {
                    hdnsaleprice.Value = "0";
                    // divYourPrice.Visible = false;
                    divYouSave.Visible = false;
                }
                if (ViewState["HotDealProduct"] != null && Convert.ToInt32(ViewState["HotDealProduct"]) == productID)
                {
                    divtodayonly.Visible = true;
                    hdnprice.Value = Convert.ToDecimal(AppLogic.AppConfigs("HotDealPrice")).ToString();
                    hdnsaleprice.Value = Convert.ToDecimal(AppLogic.AppConfigs("HotDealPrice")).ToString();
                    ltTodayPrice.Text = "<span style='padding-left:0px;font-weight:bold;color:#005395;'>" + Convert.ToDecimal(hdnsaleprice.Value).ToString("C") + "</span>";
                    if (price > Convert.ToDecimal(hdnsaleprice.Value))
                    {
                        decimal youSave = price - Convert.ToDecimal(hdnsaleprice.Value);
                        decimal Yousavepercentage = (Convert.ToDecimal(100) * youSave) / price;
                        ltYouSave.Text = "" + youSave.ToString("C") + "<span style='padding-left:0px;color:#B92127;'> (" + Math.Round(Yousavepercentage, 2) + "%)</span>";
                        //ltYouSave.Text = "<span>" + youSave.ToString("C") + "</span> (" + Math.Round(Yousavepercentage, 2) + "%)";
                        hdnYousave.Value = Yousavepercentage.ToString();
                        divYouSave.Visible = true;
                    }
                    else
                    {
                        divYouSave.Visible = false;
                    }
                }

                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Ismadetoready"].ToString()) && Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["Ismadetoready"].ToString()))
                {
                    //areadymade.InnerHtml = "MADE TO ORDER";
                    //areadymade.Title = "MADE TO ORDER";
                }
                else if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Ismadetoorder"].ToString()) && Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["Ismadetoorder"].ToString()))
                {
                    areadymade.InnerHtml = "MADE TO ORDER";
                    areadymade.Title = "MADE TO ORDER";
                }
                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Ismadetomeasure"].ToString()) && Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["Ismadetomeasure"].ToString()))
                {
                    amadetomeasure.InnerHtml = "MADE TO MEASURE";
                    amadetomeasure.Title = "MADE TO MEASURE";
                }
                litModelNumber.Text = Convert.ToString(dsproduct.Tables[0].Rows[0]["SKU"].ToString());
                ltfeature.Text = Convert.ToString(dsproduct.Tables[0].Rows[0]["Features"].ToString());
                //if (Convert.ToInt32(dsproduct.Tables[0].Rows[0]["Inventory"].ToString()) > 0)
                //{
                //btnoutofStock.Visible = false;
                //btnOutStock.Visible = false;
                divInStock.Visible = true;
                divOutStock.Visible = false;
                btnAddToCart.OnClientClick = "InsertProductRoman(" + Request.QueryString["PID"].ToString() + ",'ContentPlaceHolder1_btnAddToCart'); return false;";
                //btnInStock.Visible = true;
                //}
                //else
                //{
                //    //btnOutStock.Visible = true;
                //    //btnInStock.Visible = false;
                //    divInStock.Visible = false;
                //    divQuantity.Visible = false;
                //    btnAddToCart.Visible = false;
                //    btnoutofStock.Visible = true;
                //    divOutStock.Visible = true;
                //    lnkAvailNotification.Visible = true;
                //    //lnkAvailNotification.Attributes.Add("onclick", "OpenCenterWindow('/AvailabilityNotification.aspx?ProductId=" + Request.QueryString["PID"].ToString() + "',600,320);");
                //    lnkAvailNotification.Attributes.Add("onclick", "ShowModelForNotification('/AvailabilityNotification.aspx?ProductId=" + Request.QueryString["PID"].ToString() + "');");
                //    if (!String.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["OutOfStockMessage"].ToString()))
                //    {
                //        btnoutofStock.OnClientClick = "jAlert('" + dsproduct.Tables[0].Rows[0]["OutOfStockMessage"].ToString().Replace("'", @"\'") + "','Out of Stock Message');return false;";
                //    }
                //}
                Random rd = new Random();
                string StrTagName = Convert.ToString(dsproduct.Tables[0].Rows[0]["TagName"].ToString());
                imgMain.Attributes.Add("alt", Server.HtmlEncode(dsproduct.Tables[0].Rows[0]["Name"].ToString()));
                imgMain.Attributes.Add("title", Server.HtmlEncode(dsproduct.Tables[0].Rows[0]["Name"].ToString()));
                if (!String.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Imagename"].ToString()))
                {
                    if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsproduct.Tables[0].Rows[0]["Imagename"].ToString())))
                    {
                        imgMain.Src = AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsproduct.Tables[0].Rows[0]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                        //ltrMainImage.Text = "<img src=\"" + AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsproduct.Tables[0].Rows[0]["Imagename"].ToString() + "?" + rd.Next(10000).ToString() + "\" alt=\"\" id=\"imgMain\"  title=\"\"  style=\"vertical-align: middle;width: 322px; height: 312px;\" />";

                        if (!string.IsNullOrEmpty(StrTagName))
                        {

                            lblTag.Text += "<img style='width:60px;height:33px;' title='" + StrTagName.ToString().Trim() + "' src=\"/images/" + StrTagName.ToString().Trim() + ".png\" alt=\"" + StrTagName.ToString().Trim() + "\" class='" + StrTagName.ToString().ToLower() + "Item' />";
                            //    ImgNext.Attributes.Add("style", "color:#5e5e5e;");

                            //    INextImage.Attributes.Add("style", "float:right;");
                            //    //IPreImage.Attributes.Add("style", "width:60px !important;");

                        }
                        //if (dsproduct.Tables[0].Rows[0]["IsFreeEngraving"].ToString() == "True")
                        //{
                        //    lblFreeEngravingImage.Text = "<img title='Free Engraving' src=\"/images/FreeEngraving.png\" alt=\"Free Engraving\" style='position: relative;float:left; top: 0px; left: 0px;'/>";
                        //}


                    }
                    else
                    {
                        divzioom.Visible = false;
                        imgMain.Src =AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(10000).ToString();
                        //ltrMainImage.Text = "<img src=\"" + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg?" + rd.Next(10000).ToString() + "\" alt=\"\" title=\"\" style=\"vertical-align: middle;width: 322px; height: 312px;\" />";
                    }
                }
                else
                {
                    divzioom.Visible = false;
                    imgMain.Src =AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(10000).ToString();
                    //ltrMainImage.Text = "<img src=\"" + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg?" + rd.Next(10000).ToString() + "\" alt=\"\"  title=\"\" style=\"vertical-align: middle;width: 322px; height: 312px;\" />";
                }
                // Video by LPR
                string VideoPath = "" + AppLogic.AppConfigs("ImagePathProduct") + "Video/" + Request.QueryString["PID"].ToString() + ".flv";
                if (System.IO.File.Exists(Server.MapPath(VideoPath)))
                {
                    popUpDivnew.Visible = true;
                    ltvideo.Visible = false;
                    LoadVideo();

                }
                else
                {
                    popUpDivnew.Visible = false;
                    ltvideo.Visible = false;
                }
                //

                try
                {
                    string ProURL = AppLogic.AppConfigs("Live_server") + "/" + dsproduct.Tables[0].Rows[0]["MainCategory"].ToString() + "/" + dsproduct.Tables[0].Rows[0]["Sename"].ToString() + "-" + Request.QueryString["PID"].ToString() + ".aspx";
                    string imagepathforpinit = AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "Medium/" + dsproduct.Tables[0].Rows[0]["Imagename"].ToString();
                    string Imagename = GetMediumImage(dsproduct.Tables[0].Rows[0]["Imagename"].ToString());

                    string Description = dsproduct.Tables[0].Rows[0]["Name"].ToString().Replace("'", "''") + " - SKU: " + dsproduct.Tables[0].Rows[0]["Sku"].ToString() + " at " + AppLogic.AppConfigs("Live_server").Replace("www.", "");
                    ltrPinIt.Text = "<a  href=\"http://pinterest.com/pin/create/button/?url=" + ProURL + "&media=" + AppLogic.AppConfigs("Live_server") + "/" + Imagename + "&description=" + Description + "\" class=\"pin-it-button\" count-layout=\"horizontal\">Pin It</a>";
                }
                catch
                {
                }

                try
                {
                    String SETitle = "";
                    String SEKeywords = "";
                    String SEDescription = "";
                    if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["SETitle"].ToString()))
                    {
                        SETitle = dsproduct.Tables[0].Rows[0]["SETitle"].ToString();
                    }
                    else
                    {
                        SETitle = AppLogic.AppConfigs("SiteSETitle").ToString();
                    }

                    if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["SEKeywords"].ToString()))
                    {
                        SEKeywords = dsproduct.Tables[0].Rows[0]["SEKeywords"].ToString();
                    }
                    else
                    {
                        SEKeywords = AppLogic.AppConfigs("SiteSEKeywords").ToString();
                    }


                    if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["SEDescription"].ToString()))
                    {
                        SEDescription = dsproduct.Tables[0].Rows[0]["SEDescription"].ToString();
                    }
                    else
                    {
                        SEDescription = AppLogic.AppConfigs("SiteSEDescription").ToString();
                    }
                    Master.HeadTitle(SETitle, SEKeywords, SEDescription);
                }
                catch { }
                GetTabs(dsproduct);
                if (Swatchid > 0)
                {
                    divdiscription.Visible = true;
                }
                GetBoxes(dsproduct);
                GetCommnetByCustomer(productID);

                GetYoumayalsoLikeProduct(productID);
                BindMoreImage(dsproduct.Tables[0].Rows[0]["ImageName"].ToString(), "jpg");
                string strmeasurement = "";
                string stroptions = "";
                string strmount = "";
                // Bind Variant Engraving Line
                string StrEngraving = string.Empty;
                DataSet dsVariant = new DataSet();
                dsVariant = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + productID + " AND isnull(ParentId,0)=0  AND (ISNULL(VariantName,'') Not like '%Width%' AND ISNULL(VariantName,'') Not like '%length%') ORDER BY CASE WHEN ISNULL(VariantName,'') = 'Width' THEN 0 WHEN ISNULL(VariantName,'') = 'Length' THEN 0 ELSE 1 END, DisplayOrder");
                string strvarinat = "";

                string strvarinatcustom = "";
                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                {
                    ltvariant.Visible = true;
                    //strvarinat = "<div id=\"divVariant\">";
                    //for (int i = 0; i < dsVariant.Tables[0].Rows.Count; i++)
                    //{
                    //    strvarinat += "<div class=\"item-right-pt1\">";
                    //    strvarinat += "<div class=\"item-right-pt1-left\" id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";
                    //    DataSet dsVariantvalue = new DataSet();
                    //    dsVariantvalue = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariantValue WHERE ProductID=" + productID + " AND VariantID=" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "");
                    //    if (dsVariantvalue != null && dsVariantvalue.Tables.Count > 0 && dsVariantvalue.Tables[0].Rows.Count > 0)
                    //    {
                    //        strvarinat += "<div class=\"item-right-pt1-right\">:&nbsp;";
                    //        strvarinat += "<select name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"width: 100px;\" class=\"select-box\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >";
                    //        strvarinat += "<option value=\"0\">Select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                    //        for (int j = 0; j < dsVariantvalue.Tables[0].Rows.Count; j++)
                    //        {
                    //            strvarinat += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "\">" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "</option>";
                    //        }
                    //        strvarinat += "</select></div>";
                    //    }
                    //    strvarinat += "</div>";
                    //}
                    //strvarinat += "</div>";

                    strvarinat += "<div id=\"divVariant\">";


                    strmeasurement += "<div class=\"readymade-detail-pt1\">";
                    strmeasurement += "<div style=\"display: none;\" class=\"pro-properties-left\">";
                    strmeasurement += "SELECT MEASUREMENTS <span></span>";
                    strmeasurement += "</div>";
                    strmeasurement += "<input type=\"hidden\" value=\"Width\" name=\"hdnvariantname-0\" />";
                    strmeasurement += "<div onclick=\"varianttabhideshow(0);\" id=\"divcolspan-0\" class=\"readymade-detail-pt1-pro\">";
                    strmeasurement += "<span id=\"spancolspan-0\">3</span>SELECT MEASUREMENTS";
                    strmeasurement += "<div style=\"float: right; line-height: 25px; padding-right: 2px;\" id=\"divselvalue-0\">";
                    strmeasurement += "</div>";
                    strmeasurement += "</div>";
                    strmeasurement += "<div style=\"display: none;\" id=\"divSelectvariant-0\" class=\"readymade-detail-right-pro\">";
                    strmeasurement += " <div class=\"selector fixedWidth\">";
                    strmeasurement += " <span id=\"spanvariant-0\" style=\"-moz-user-select: none;\">Width</span>";
                    strmeasurement += "<div style=\"width: auto ! important;\" class=\"option-pt1\">";
                    strmeasurement += "<span id=\"divvariantname-0\" style=\"float: left; padding-right: 2px; display: none;\">";
                    strmeasurement += "Width</span>";
                    strmeasurement += " </div>";
                    strmeasurement += "<select id=\"Selectvariant-0\" class=\"option2\" name=\"Selectvariant-0\" onchange=\"PriceChangeondropdown();\">";
                    strmeasurement += "<option value=\"0\">Width</option>";
                    strmeasurement += "<option value=\"12\">12</option><option value=\"13\">13</option><option value=\"14\">14</option><option value=\"15\">15</option><option value=\"16\">16</option><option value=\"17\">17</option><option value=\"18\">18</option><option value=\"19\">19</option><option value=\"20\">20</option><option value=\"21\">21</option><option value=\"22\">22</option><option value=\"23\">23</option><option value=\"24\">24</option><option value=\"25\">25</option><option value=\"26\">26</option><option value=\"27\">27</option><option value=\"28\">28</option><option value=\"29\">29</option><option value=\"30\">30</option><option value=\"31\">31</option><option value=\"32\">32</option><option value=\"33\">33</option><option value=\"34\">34</option><option value=\"35\">35</option><option value=\"36\">36</option><option value=\"37\">37</option><option value=\"38\">38</option><option value=\"39\">39</option><option value=\"40\">40</option><option value=\"41\">41</option><option value=\"42\">42</option><option value=\"43\">43</option><option value=\"44\">44</option><option value=\"45\">45</option><option value=\"46\">46</option><option value=\"47\">47</option><option value=\"48\">48</option><option value=\"49\">49</option><option value=\"50\">50</option><option value=\"51\">51</option><option value=\"52\">52</option><option value=\"53\">53</option><option value=\"54\">54</option><option value=\"55\">55</option><option value=\"56\">56</option><option value=\"57\">57</option><option value=\"58\">58</option><option value=\"59\">59</option><option value=\"60\">60</option><option value=\"61\">61</option><option value=\"62\">62</option><option value=\"63\">63</option><option value=\"64\">64</option><option value=\"65\">65</option><option value=\"66\">66</option><option value=\"67\">67</option><option value=\"68\">68</option><option value=\"69\">69</option><option value=\"70\">70</option><option value=\"71\">71</option><option value=\"72\">72</option>";
                    strmeasurement += "</select></div>";


                    strmeasurement += "<div class=\"selector fixedWidth\">";
                    strmeasurement += "<span style=\"-moz-user-select: none;\" id=\"spanvariant-exact-0\">00</span><select";
                    strmeasurement += " class=\"option2\" onchange=\"PriceChangeondropdown();\" name=\"Selectvariant-exact-0\"";
                    strmeasurement += "id=\"Selectvariant-exact-0\"><option value=\"0\" style=\"display: none;\">Extra width</option>";
                    strmeasurement += "<option value=\"00\" selected=\"true\">00</option>";
                    strmeasurement += "<option value=\"1/8\">1/8</option>";
                    strmeasurement += "<option value=\"1/4\">1/4</option>";
                    strmeasurement += "<option value=\"3/8\">3/8</option>";
                    strmeasurement += "<option value=\"1/2\">1/2</option>";
                    strmeasurement += "<option value=\"5/8\">5/8</option>";
                    strmeasurement += "<option value=\"3/4\">3/4</option>";
                    strmeasurement += "<option value=\"7/8\">7/8</option>";
                    strmeasurement += "</select><span style=\"float: left; padding-right: 2px; display: none;\" id=\"divvariantname-exact-0\">Width</span></div>";


                    strmeasurement += "<div class=\"selector fixedWidth\">";
                    strmeasurement += "<span id=\"spanvariant-99999999\" style=\"-moz-user-select: none;\" id=\"spanvariant-99999999\">Length</span><div style=\"width: auto ! important;\"";
                    strmeasurement += " class=\"option-pt1\">";
                    strmeasurement += "<span id=\"divvariantname-99999999\" style=\"float: left; padding-right: 2px; display: none;\">";
                    strmeasurement += "Length</span></div>";
                    strmeasurement += "<select id=\"Selectvariant-99999999\" class=\"option2\" name=\"Selectvariant-99999999\" onchange=\"PriceChangeondropdown();\">";
                    strmeasurement += "<option value=\"0\">Length</option>";
                    strmeasurement += "<option value=\"12\">12</option><option value=\"13\">13</option><option value=\"14\">14</option><option value=\"15\">15</option><option value=\"16\">16</option><option value=\"17\">17</option><option value=\"18\">18</option><option value=\"19\">19</option><option value=\"20\">20</option><option value=\"21\">21</option><option value=\"22\">22</option><option value=\"23\">23</option><option value=\"24\">24</option><option value=\"25\">25</option><option value=\"26\">26</option><option value=\"27\">27</option><option value=\"28\">28</option><option value=\"29\">29</option><option value=\"30\">30</option><option value=\"31\">31</option><option value=\"32\">32</option><option value=\"33\">33</option><option value=\"34\">34</option><option value=\"35\">35</option><option value=\"36\">36</option><option value=\"37\">37</option><option value=\"38\">38</option><option value=\"39\">39</option><option value=\"40\">40</option><option value=\"41\">41</option><option value=\"42\">42</option><option value=\"43\">43</option><option value=\"44\">44</option><option value=\"45\">45</option><option value=\"46\">46</option><option value=\"47\">47</option><option value=\"48\">48</option><option value=\"49\">49</option><option value=\"50\">50</option><option value=\"51\">51</option><option value=\"52\">52</option><option value=\"53\">53</option><option value=\"54\">54</option><option value=\"55\">55</option><option value=\"56\">56</option><option value=\"57\">57</option><option value=\"58\">58</option><option value=\"59\">59</option><option value=\"60\">60</option><option value=\"61\">61</option><option value=\"62\">62</option><option value=\"63\">63</option><option value=\"64\">64</option><option value=\"65\">65</option><option value=\"66\">66</option><option value=\"67\">67</option><option value=\"68\">68</option><option value=\"69\">69</option><option value=\"70\">70</option><option value=\"71\">71</option><option value=\"72\">72</option>";
                    strmeasurement += "</select></div>";

                    strmeasurement += "<div class=\"selector fixedWidth\">";
                    strmeasurement += "<span style=\"-moz-user-select: none;\" id=\"spanvariant-exact-99999999\">00</span><select";
                    strmeasurement += " class=\"option2\" onchange=\"PriceChangeondropdown();\" name=\"Selectvariant-exact-99999999\"";
                    strmeasurement += "id=\"Selectvariant-exact-99999999\"><option value=\"0\" style=\"display: none;\">Extra length</option>";
                    strmeasurement += "<option value=\"00\" selected=\"true\">00</option>";
                    strmeasurement += "<option value=\"1/8\">1/8</option>";
                    strmeasurement += "<option value=\"1/4\">1/4</option>";
                    strmeasurement += "<option value=\"3/8\">3/8</option>";
                    strmeasurement += "<option value=\"1/2\">1/2</option>";
                    strmeasurement += "<option value=\"5/8\">5/8</option>";
                    strmeasurement += "<option value=\"3/4\">3/4</option>";
                    strmeasurement += "<option value=\"7/8\">7/8</option>";
                    strmeasurement += "</select><span style=\"float: left; padding-right: 2px; display: none;\" id=\"divvariantname-exact-99999999\">Length</span></div>";
                    strmeasurement += "<div style=\"float: right; width: 95%; text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-0') != null && document.getElementById('Selectvariant-0').selectedIndex == 0){jAlert('Please select width.','Message');}else if(document.getElementById('Selectvariant-99999999') != null && document.getElementById('Selectvariant-99999999').selectedIndex == 0){jAlert('Please select length.','Message');}else{varianttabhideshow(1000000);}\" /></div>";
                    strmeasurement += "</div>";


                    strmeasurement += "</div>";



                    //strvarinat += "<div class=\"readymade-detail-pt1\">";
                    //strvarinat += "<div onclick=\"varianttabhideshowcustom(4);\" id=\"divcolspancustom-4\" class=\"readymade-detail-pt1-pro\">";
                    //strvarinat += "<span id=\"spancolspancustom-4\">4</span>Select Options:";
                    //strvarinat += "<div style=\"float:right;line-height:25px;padding-right:2px;padding-top: 2px;\"><a onclick=\"variantDetail('divMakeOrderOptions');\" href=\"javascript:void(0);\" title=\"Learn More\"><img border=\"0\" src=\"/images/help-icon-more.png\"></a></div>";
                    //strvarinat += "</div>";
                    //strvarinat += "<div style=\"display:none;\" id=\"divcolspancustomvalue-4\" class=\"readymade-detail-right-pro\">";
                    //strvarinat += "<select style=\"width: 200px !important;display:none;\" onchange=\"ChangeCustomprice();\" class=\"option1\" id=\"ContentPlaceHolder1_ddlcustomoptin\" name=\"ctl00$ContentPlaceHolder1$ddlcustomoptin\">";
                    //strvarinat += "<option value=\"\" selected=\"selected\">Options</option>";
                    //strvarinat += "<option value=\"Lined\">Lined</option>";
                    //strvarinat += "<option value=\"Lined &amp; Interlined\">Lined &amp; Interlined</option>";
                    //strvarinat += "<option value=\"Blackout Lining\">Blackout Lining</option>";
                    //strvarinat += "</select>";
                    //strvarinat += "<div style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-1\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined','divcustomflat-radio-004-1','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div style=\"margin-top: 2px;\">&nbsp;Lined</div></div><div style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-2\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Lined &amp; Interlined','divcustomflat-radio-004-2','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div style=\"margin-top: 2px;\">&nbsp;Lined &amp; Interlined</div></div><div style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divcustomflat-radio-004-3\" onclick=\"selecteddropdownvaluecustom('ContentPlaceHolder1_ddlcustomoptin','Blackout Lining','divcustomflat-radio-004-3','004');ChangeCustomprice();\" class=\"iradio_flat-red\"></div><div style=\"margin-top: 2px;\">&nbsp;Blackout Lining</div></div>";
                    //strvarinat += "</div>";
                    //strvarinat += "<div id=\"spancolspancustomvalue-4\" style=\"margin-top:10px;display:none;\">";
                    //strvarinat += "</div>";
                    //strvarinat += "</div>";


                    stroptions += "<div class=\"readymade-detail-pt1\">";
                    stroptions += "<div id=\"divvariantname-1000000\" style=\"display: none;\" class=\"pro-properties-left\">Options</div>";
                    stroptions += "<input type=\"hidden\" value=\"Top Header Design\" name=\"hdnvariantname-1000000\"><div onclick=\"varianttabhideshow(1000000);\" id=\"divcolspan-1000000\" class=\"readymade-detail-pt1-pro\">";
                    stroptions += "<span id=\"spancolspan-1000000\">4</span>Options</div>";
                    stroptions += "<div style=\"display: none;\" id=\"divSelectvariant-1000000\" class=\"readymade-detail-right-pro\">";
                    stroptions += "<select id=\"Selectvariant-1000000\" class=\"option1\" name=\"Selectvariant-1000000\" style=\"display: none;\" onchange=\"PriceChangeondropdown();\">";
                    //strvarinat += "<option style=\"display: none;\" value=\"0\">Top Header Design</option>";
                    //strvarinat += "<option value=\"7405\">50Wx84L Pole Pocket BT (Ready Made)</option>";
                    //strvarinat += "<option value=\"7406\">50Wx96L Pole Pocket BT (Ready Made)(+$10.00)</option>";

                    stroptions += "<option value=\"\" selected=\"selected\">Options</option>";
                    stroptions += "<option value=\"1000001\">Lined</option>";
                    stroptions += "<option value=\"1000002\">Lined &amp; Interlined</option>";
                    stroptions += "<option value=\"1000003\">Blackout Lining</option>";
                    stroptions += "</select>";

                    stroptions += "<div style=\"float: left; width: 100%; margin-bottom: 5px;\">";
                    stroptions += "<div id=\"divflat-radio-1000000-1000001\" onclick=\"selecteddropdownvalue(1000000,1000001);PriceChangeondropdown();\" class=\"iradio_flat-red\">";
                    stroptions += "</div>";
                    stroptions += "<div style=\"margin-top: 2px;\" class='item-radio-display-text'> &nbsp;Lined</div>";
                    stroptions += "</div>";

                    stroptions += "<div style=\"float: left; width: 100%; margin-bottom: 5px;\">";
                    stroptions += "<div id=\"divflat-radio-1000000-1000002\" onclick=\"selecteddropdownvalue(1000000,1000002);PriceChangeondropdown();\" class=\"iradio_flat-red\">";
                    stroptions += "</div>";
                    stroptions += "<div style=\"margin-top: 2px;\" class='item-radio-display-text'> &nbsp;Lined &amp; Interlined</div>";
                    stroptions += "</div>";

                    stroptions += "<div style=\"float: left; width: 100%; margin-bottom: 5px;\">";
                    stroptions += "<div id=\"divflat-radio-1000000-1000003\" onclick=\"selecteddropdownvalue(1000000,1000003);PriceChangeondropdown();\" class=\"iradio_flat-red\">";
                    stroptions += "</div>";
                    stroptions += "<div style=\"margin-top: 2px;\" class='item-radio-display-text'> &nbsp;Blackout Lining</div>";
                    stroptions += "</div>";

                    stroptions += "<div style=\"float: right; width: 95%; text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-1000000') != null && document.getElementById('Selectvariant-1000000').selectedIndex == 0){jAlert('Please select options.','Message');}else{varianttabhideshow(2000000);}\" /></div>";

                    stroptions += "</div>";

                    stroptions += "</div>";



                    strmount += "<div class=\"readymade-detail-pt1\">";
                    strmount += "<div id=\"divvariantname-2000000\" style=\"display: none;\" class=\"pro-properties-left\">Mount</div>";
                    strmount += "<input type=\"hidden\" value=\"Mount\" name=\"hdnvariantname-2000000\"><div onclick=\"varianttabhideshow(2000000);\" id=\"divcolspan-2000000\" class=\"readymade-detail-pt1-pro\">";
                    strmount += "<span id=\"spancolspan-2000000\">5</span>Mount</div>";
                    strmount += "<div style=\"display: none;\" id=\"divSelectvariant-2000000\" class=\"readymade-detail-right-pro\">";
                    strmount += "<select id=\"Selectvariant-2000000\" class=\"option1\" name=\"Selectvariant-2000000\" style=\"display: none;\" onchange=\"PriceChangeondropdown();\">";

                    strmount += "<option value=\"\" selected=\"selected\">Mount</option>";
                    strmount += "<option value=\"2000001\">Inside</option>";
                    strmount += "<option value=\"2000002\">Outside</option>";
                    strmount += "</select>";

                    strmount += "<div style=\"float: left; width: 50%; margin-bottom: 5px;\">";
                    strmount += "<div style=\"float:left;width:100%;\"><img src=\"/images/roman_inside_mount.png\" /></div>";
                    strmount += "<div style=\"margin-left:16%;\"><div id=\"divflat-radio-2000000-2000001\" onclick=\"selecteddropdownvalue(2000000,2000001);PriceChangeondropdown();\" class=\"iradio_flat-red\">";
                    strmount += "</div>";
                    strmount += "<div style=\"margin-top: 2px;float:left;\"> &nbsp;Inside</div>";
                    strmount += "</div></div>";

                    strmount += "<div style=\"float: left; width: 50%; margin-bottom: 5px;\">";
                    strmount += "<div style=\"float:left;width:100%;\"><img src=\"/images/roman_outside_mount.png\" /></div>";
                    strmount += "<div style=\"margin-left:16%;\"><div id=\"divflat-radio-2000000-2000002\" onclick=\"selecteddropdownvalue(2000000,2000002);PriceChangeondropdown();\" class=\"iradio_flat-red\">";
                    strmount += "</div>";
                    strmount += "<div style=\"margin-top: 2px;float:left;\"> &nbsp;Outside</div>";
                    strmount += "</div></div>";

                    strmount += "<div style=\"float: right; width: 95%;text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-2000000') != null && document.getElementById('Selectvariant-2000000').selectedIndex == 0){jAlert('Please select options.','Message');}else{varianttabhideshow($$$$$$$$$$);}\" /></div>";

                    strmount += "</div>";
                    strmount += "</div>";

                    string strwidthlenght = "";
                    bool checkmade = false;
                    bool isCustom = false;
                    bool isChild = false;
                    bool IsWidth = false;
                    Int32 Sid = 0;
                    Int32 iNumb = 0;
                    Int32 CntReadymade = 0;
                    if (areadymade.Visible == true)
                    {
                        CntReadymade = 1;
                    }
                    string strtttCord = "";
                    string strtttCordvalue = "";
                    for (int i = 0; i < dsVariant.Tables[0].Rows.Count; i++)
                    {
                        iNumb++;
                        string strvarinatchild = "";
                        string radiobutton = "";
                        string strnextbutton = "";

                        //if (dsVariant.Tables[0].Rows.Count - 1 > i)
                        //{
                        if (dsVariant.Tables[0].Rows.Count - 1 == i)
                        {
                            strnextbutton = "<div style=\"float: right; width: 95%;text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "') != null && document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "').selectedIndex == 0){jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("select", "") + "','Message');}else{varianttabhideshow(0);}\" /></div>";
                        }
                        //else if (dsVariant.Tables[0].Rows.Count > 2 && i == 1)
                        //{
                        //    strnextbutton = "<div style=\"float: right; width: 95%;text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "') != null && document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "').selectedIndex == 0){jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("select", "") + "','Message');}else{varianttabhideshow(0);}\" /></div>";
                        //}
                        //else if (dsVariant.Tables[0].Rows.Count == 2 && i == 2)
                        //{
                        //    strnextbutton = "<div style=\"float: right; width: 95%;text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "') != null && document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "').selectedIndex == 0){jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("select", "") + "','Message');}else{varianttabhideshow(0);}\" /></div>";
                        //}
                        //else if (dsVariant.Tables[0].Rows.Count == 1 && i == 0)
                        //{
                        //    strnextbutton = "<div style=\"float: right; width: 95%;text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "') != null && document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "').selectedIndex == 0){jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("select", "") + "','Message');}else{varianttabhideshow(0);}\" /></div>";
                        //}
                        else if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("color") > -1)
                        {
                            strnextbutton = "<div style=\"float: right; width: 95%;text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "') != null && document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "').selectedIndex == 0){jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("select", "") + "','Message');}else{varianttabhideshow(0);}\" /></div>";
                        }
                        else
                        {
                            strnextbutton = "<div style=\"float: right; width: 95%;text-align: right;\"><img src=\"/images/next-option.png\" alt=\"Next\" title=\"Next\" border=\"0\" style=\"cursor:pointer;\" onclick=\"if(document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "') != null && document.getElementById('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "').selectedIndex == 0){jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("select", "") + "','Message');}else{varianttabhideshow(" + dsVariant.Tables[0].Rows[(i + 1)]["VariantID"].ToString() + ");}\" /></div>";
                        }
                        //}
                        checkmade = false;
                        bool IsVariColor = false;
                        DataSet dsVariantvalue = new DataSet();
                        dsVariantvalue = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariantValue WHERE ProductID=" + productID + " AND VariantID=" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + " Order By ISNULL(DisplayOrder,0)");
                        if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("color") > -1)
                        {
                            //if (dsVariant.Tables[0].Rows.Count > 2 && i == 2)
                            //{
                            //    strvarinat += strmeasurement + stroptions + strmount;
                            //    // iNumb++;
                            //    strvarinat = strvarinat.Replace("###3###", iNumb.ToString());
                            //    iNumb++;
                            //    strvarinat = strvarinat.Replace("###4###", iNumb.ToString());
                            //    iNumb++;
                            //    strvarinat = strvarinat.Replace("###5###", iNumb.ToString());
                            //    iNumb++;
                            //}
                            //else if (dsVariant.Tables[0].Rows.Count == 2 && i == 1)
                            //{
                            //    strvarinat += strmeasurement + stroptions + strmount;
                            //    // iNumb++;
                            //    strvarinat = strvarinat.Replace("###3###", iNumb.ToString());
                            //    iNumb++;
                            //    strvarinat = strvarinat.Replace("###4###", iNumb.ToString());
                            //    iNumb++;
                            //    strvarinat = strvarinat.Replace("###5###", iNumb.ToString());
                            //    iNumb++;
                            //}
                            //else if (dsVariant.Tables[0].Rows.Count == 1 && i == 0)
                            //{
                            //    strvarinat += strmeasurement + stroptions + strmount;

                            //    strvarinat = strvarinat.Replace("###3###", iNumb.ToString());
                            //    iNumb++;
                            //    strvarinat = strvarinat.Replace("###4###", iNumb.ToString());
                            //    iNumb++;
                            //    strvarinat = strvarinat.Replace("###5###", iNumb.ToString());
                            //    iNumb++;
                            //}
                            strvarinat = strvarinat.Replace("varianttabhideshow(888888888)", "varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ")");
                            strvarinat += "<div class=\"readymade-detail-pt1\" >";

                            strvarinat += "<div class=\"pro-properties-left\" style=\"display:none;\" id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";
                        }
                        else
                        {
                            if ((dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1 || dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1))
                            {
                                if (IsWidth == false)
                                {
                                    strvarinat += "<div class=\"readymade-detail-pt1\" >";
                                    //strvarinat += "<div class=\"pro-properties-left\">SELECT MEASUREMENTS <span><a href=\"#\" title=\"Learn More\">Learn More</a></span></div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";
                                    strvarinat += "<div class=\"pro-properties-left\" style=\"display:none;\">SELECT MEASUREMENTS <span></span></div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";
                                    strvarinat += "<div class=\"readymade-detail-pt1-pro\"  id=\"divcolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" onclick=\"varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ");\">";
                                    strvarinat += "<span id=\"spancolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + iNumb.ToString() + "</span>SELECT MEASUREMENTS ";
                                    strvarinat += "<div id=\"divselvalue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"float:right;line-height:25px;padding-right:2px;\"></div>";
                                    strvarinat += "</div>";
                                }
                            }
                            else
                            {
                                //if (dsVariant.Tables[0].Rows.Count > 2 && i == 2)
                                //{
                                //    strvarinat += strmeasurement + stroptions + strmount;

                                //    strvarinat = strvarinat.Replace("###3###", iNumb.ToString());
                                //    iNumb++;
                                //    strvarinat = strvarinat.Replace("###4###", iNumb.ToString());
                                //    iNumb++;
                                //    strvarinat = strvarinat.Replace("###5###", iNumb.ToString());
                                //    iNumb++;
                                //}
                                //else if (dsVariant.Tables[0].Rows.Count == 2 && i == 1)
                                //{
                                //    strvarinat += strmeasurement + stroptions + strmount;
                                //    //iNumb++;
                                //    strvarinat = strvarinat.Replace("###3###", iNumb.ToString());
                                //    iNumb++;
                                //    strvarinat = strvarinat.Replace("###4###", iNumb.ToString());
                                //    iNumb++;
                                //    strvarinat = strvarinat.Replace("###5###", iNumb.ToString());
                                //    iNumb++;
                                //}
                                //else if (dsVariant.Tables[0].Rows.Count == 1 && i == 0)
                                //{
                                //    strvarinat += strmeasurement + stroptions + strmount;
                                //    //iNumb++;
                                //    strvarinat = strvarinat.Replace("###3###", iNumb.ToString());
                                //    iNumb++;
                                //    strvarinat = strvarinat.Replace("###4###", iNumb.ToString());
                                //    iNumb++;
                                //    strvarinat = strvarinat.Replace("###5###", iNumb.ToString());
                                //    iNumb++;
                                //}
                                strvarinat = strvarinat.Replace("varianttabhideshow(888888888)", "varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ")");

                                if ((dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("cord") > -1))
                                {
                                    strtttCord += "<div class=\"readymade-detail-pt1\" >";
                                    strtttCord += "<div class=\"pro-properties-left\"  style=\"display:none;\" id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";

                                    strtttCord += "<div class=\"readymade-detail-pt1-pro\" id=\"divcolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" onclick=\"varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ");\">";

                                    strtttCord += "<span id=\"spancolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">6</span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString();
                                    strtttCord += "<div id=\"divselvalue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"float:right;line-height:25px;padding-right:2px;\"></div>";
                                    strtttCord += "</div>";
                                    strmount = strmount.Replace("varianttabhideshow($$$$$$$$$$);", "varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ");");

                                }
                                else
                                {
                                    strvarinat += "<div class=\"readymade-detail-pt1\" >";
                                    strvarinat += "<div class=\"pro-properties-left\"  style=\"display:none;\" id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";

                                    strvarinat += "<div class=\"readymade-detail-pt1-pro\" id=\"divcolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" onclick=\"varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ");\">";

                                    strvarinat += "<span id=\"spancolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + iNumb.ToString() + "</span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString();
                                    strvarinat += "<div id=\"divselvalue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"float:right;line-height:25px;padding-right:2px;\"></div>";
                                    strvarinat += "</div>";
                                }


                            }
                        }
                        if (dsVariantvalue != null && dsVariantvalue.Tables.Count > 0 && dsVariantvalue.Tables[0].Rows.Count > 0)
                        {
                            string StrColorVari = "";
                            if (!string.IsNullOrEmpty(dsVariant.Tables[0].Rows[i]["VariantName"].ToString()) && dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("color") > -1)
                            {
                                if (dsVariantvalue != null && dsVariantvalue.Tables.Count > 0 && dsVariantvalue.Tables[0].Rows.Count > 0)
                                {
                                    Random rnd = new Random();
                                    for (int j = 0; j < dsVariantvalue.Tables[0].Rows.Count; j++)
                                    {
                                        if (!string.IsNullOrEmpty(dsVariantvalue.Tables[0].Rows[j]["VariImageName"].ToString()))
                                        {
                                            string ColorImgName = Convert.ToString(dsVariantvalue.Tables[0].Rows[j]["VariImageName"].ToString()); //Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(Imagepath,'') as Imagepath from tb_Productcolor where ISNULL(Active,0)=1 and ColorName='" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "'"));
                                            if (!string.IsNullOrEmpty(ColorImgName))
                                            {
                                                String StrColorPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/");
                                                if (!Directory.Exists(Server.MapPath(StrColorPath)))
                                                    Directory.CreateDirectory(Server.MapPath(StrColorPath));
                                                string strcolorFilePath = Server.MapPath(StrColorPath + ColorImgName);
                                                if (File.Exists(strcolorFilePath))
                                                {
                                                    IsVariColor = true;
                                                    StrColorPath = StrColorPath + ColorImgName + "?" + rnd.Next(10000);
                                                    if (Session["VID"] != null && Convert.ToString(Session["VID"]) != "" && (dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString().ToLower() == Convert.ToString(Session["VID"]).ToLower()))
                                                    {
                                                        StrColorVari += "<li id=\"id_color_" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" style=\"width:auto !important;\" onclick=\"SelectColorli(" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");\" class=\"coloractiveli\"><a href=\"javascript:void(0);\"  onclick=\"SelectVariantByColor('" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "'," + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");\"><img id=\"img_color_" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" title=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "\" alt=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "\" src=\"" + StrColorPath + "\"></a>";
                                                    }
                                                    else
                                                    {
                                                        StrColorVari += "<li id=\"id_color_" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" style=\"width:auto !important;\" onclick=\"SelectColorli(" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");\"><a href=\"javascript:void(0);\" onclick=\"SelectVariantByColor('" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "'," + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");\" ><img id=\"img_color_" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" title=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "\" alt=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "\" src=\"" + StrColorPath + "\"></a>";
                                                    }
                                                    StrColorVari += "<input type=\"hidden\" value=\"1\" id=\"img_color_isavali_" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" /></li>";
                                                }
                                            }
                                        }
                                    }
                                }
                                if (IsVariColor)
                                {

                                    strvarinat += "<div class=\"readymade-detail-pt1-pro\" id=\"divcolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" onclick=\"varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ");\">";

                                    strvarinat += "<span id=\"spancolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >" + iNumb.ToString() + "</span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString();
                                    strvarinat += "<div id=\"divselvalue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"float:right;line-height:25px;padding-right:2px;\"></div>";
                                    strvarinat += "</div>";

                                    strvarinat += "<div class=\"pro-properties-pt1\" style=\"display:none;\"  id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\"><span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</span><input type=\"hidden\" name=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";
                                    strvarinat += "</div>";
                                    strvarinat += "<div class=\"readymade-detail-right-pro\" id=\"divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"display:none;\">";
                                    strvarinat += "<ul class=\"item-color-option\" id=\"color_ul_name\">";
                                    strvarinat += StrColorVari;
                                    strvarinat += "</ul>";
                                    strvarinat += strnextbutton.ToString();
                                    strvarinat += "</div>";
                                }
                            }

                            // ----------------------- If Width - Length Exists bind MEASUREMENTS Block 

                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("color") > -1)
                            {
                                //  strvarinat += "  <div class=\"pro-properties-right\" style=\"display:none;\" id='divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'>";
                                strvarinat += "  <div class=\"pro-properties-right\" style=\"display:none;\">";
                            }
                            else
                            {
                                if ((dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1) || (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1))
                                {

                                    if (strvarinat.ToString().ToLower().IndexOf("###width-length####") <= -1)
                                    {
                                        strvarinat += "  <div class=\"readymade-detail-right-pro\" id='divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "' style=\"display:none;\">###width-length####";
                                    }
                                }
                                else if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("cord") > -1)
                                {
                                    strtttCord += "  <div class=\"readymade-detail-right-pro\" id='divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "' style=\"display:none;\">";
                                }
                                else
                                {
                                    strvarinat += "  <div class=\"readymade-detail-right-pro\" id='divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "' style=\"display:none;\">";
                                }
                            }

                            if ((dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1) || (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1))
                            {
                                if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1)
                                {
                                    strwidthlenght += "<div class=\"selector fixedWidth\"><span style=\"-moz-user-select: none;\" id=\"spanvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">Length</span>";
                                    strwidthlenght += "<div class=\"option-pt1\" style=\"width:auto !important\"><span style=\"float:left;padding-right:2px;display:none;\" id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">Length</span></div>";
                                }
                                else
                                {
                                    strwidthlenght += "<div class=\"selector fixedWidth\"><span style=\"-moz-user-select: none;\" id=\"spanvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">Width</span>";
                                    strwidthlenght += "<div class=\"option-pt1\" style=\"width:auto !important\"><span style=\"float:left;padding-right:2px;display:none;\" id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">Width</span></div>";
                                }

                                strwidthlenght += "<select onchange=\"PriceChangeondropdown();\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" class=\"option2\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >";
                            }
                            else
                            {
                                // strvarinat += "<div class=\"selector fixedWidth\"><span style=\"-moz-user-select: none;\" id=\"spanvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</span>";

                                if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("cord") > -1)
                                {
                                    strtttCordvalue += "<select onchange=\"PriceChangeondropdown();\" style=\"display:none;\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" class=\"option1\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >";
                                }
                                else
                                {
                                    strvarinat += "<select onchange=\"PriceChangeondropdown();\" style=\"display:none;\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" class=\"option1\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >";
                                }
                            }

                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("header") > -1)
                            {
                                strvarinat += "<option value=\"0\" style=\"display:none;\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                            }
                            else
                            {
                                if ((dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1) || (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1))
                                {
                                    strwidthlenght += "<option value=\"0\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                                }
                                else
                                {
                                    if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("cord") > -1)
                                    {
                                        strtttCordvalue += "<option value=\"0\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                                    }
                                    else
                                    {
                                        strvarinat += "<option value=\"0\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                                    }
                                }
                            }


                            string strttt = "";
                            for (int j = 0; j < dsVariantvalue.Tables[0].Rows.Count; j++)
                            {
                                string StrVarValueId = Convert.ToString(dsVariantvalue.Tables[0].Rows[j]["VariantValueId"]);
                                string StrQry = "";
                                Int32 Intcnt = 0;
                                string StrBuy1onsale = "";
                                string StrbackOrderdate = "";
                                bool IsOnSale = false;
                                decimal OnsalePrice = 0;

                                if (CntReadymade == 1)
                                {
                                    StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + productID + " and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and ISNULL(Buy1Get1,0)=1 and VariantValueID=" + StrVarValueId + "";
                                    Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                    if (Intcnt > 0)
                                    {
                                        StrBuy1onsale = "<span class=\"buy-get-free\">(Buy 1 Get 1 Free)</span>";
                                    }

                                    StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + productID + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 and VariantValueID=" + StrVarValueId + "";
                                    Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                    if (Intcnt > 0)
                                    {
                                        IsOnSale = true;
                                        OnsalePrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(OnSalePrice,0) as OnSalePrice from tb_ProductVariantValue Where productid=" + productID + " and VariantValueID=" + StrVarValueId + " and ISNULL(OnSale,0)=1"));
                                        StrBuy1onsale += "<span class=\"buy-get-free\">(On Sale)</span>";
                                    }
                                    StrQry = "Select Convert(char(10),BackOrderdate,101) from tb_ProductVariantValue Where productid=" + productID + " and (cast(BackOrderdate as date) >=cast(GETDATE() as date)) and VariantValueID=" + StrVarValueId + "";
                                    StrbackOrderdate = Convert.ToString(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                    if (!String.IsNullOrEmpty(StrbackOrderdate))
                                    {
                                        StrbackOrderdate = " (Back Order Date: " + StrbackOrderdate + ")";
                                    }
                                }

                                string strPrice = "";
                                if (IsOnSale == true && OnsalePrice > decimal.Zero)
                                {
                                    strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(OnsalePrice.ToString())) + ")";
                                }
                                else
                                {
                                    if (Convert.ToDecimal(dsVariantvalue.Tables[0].Rows[j]["VariantPrice"].ToString()) > Decimal.Zero)
                                    {
                                        strPrice = "(+$" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvalue.Tables[0].Rows[j]["VariantPrice"].ToString())) + ")";
                                    }
                                }
                                if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("custom") > -1)
                                {
                                    checkmade = true;
                                    isCustom = true;
                                }
                                else
                                {
                                    if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("color") > -1)
                                    {
                                        if (Session["VID"] != null && Convert.ToString(Session["VID"]) != "" && (dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString().ToLower() == Convert.ToString(Session["VID"]).ToLower()))
                                        {
                                            strvarinat += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" selected=\"true\" ####selectded####>" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale.Replace("<span class=\"buy-get-free\">", "").Replace("</span>", "") + "</option>";
                                            Session["VID"] = null;
                                        }
                                        else { strvarinat += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####selectded####>" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale.Replace("<span class=\"buy-get-free\">", "").Replace("</span>", "") + "</option>"; }
                                        strttt += "<div style=\"float:left;width:100%;margin-bottom:5px;\" class='item-radio-display'><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + "</div></div>";
                                        //strvarinat += "<div style=\"float:left;width:100%;margin-bottom:5px;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "\"></div><div style=\"margin-top: 2px;\">&nbsp;" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</div></div>";

                                    }
                                    else if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("cord") > -1)
                                    {
                                        if ((dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1) || (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1))
                                        {
                                            strwidthlenght += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####selectded####>" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale.Replace("<span class=\"buy-get-free\">", "").Replace("</span>", "") + strPrice + "</option>";
                                        }
                                        else
                                        {

                                            strtttCordvalue += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####selectded####>" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale.Replace("<span class=\"buy-get-free\">", "").Replace("</span>", "") + strPrice + "</option>";
                                        }
                                        if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("left") > -1)
                                        {
                                            strtttCord += "<div style=\"float:left;width:50%;margin-bottom:5px;\" class='item-radio-display'><div style=\"float: left; width: 100%;\"><img src=\"/images/left-cord.png\" alt=\"Left\" title=\"Left\"></div><div style=\"margin-left: 17%;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;float: left;\">&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div></div>";
                                        }
                                        else if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("right") > -1)
                                        {
                                            strtttCord += "<div style=\"float:left;width:50%;margin-bottom:5px;\" class='item-radio-display'><div style=\"float: left; width: 100%;\"><img src=\"/images/right-cord.png\" alt=\"Right\" title=\"Right\"></div><div style=\"margin-left: 17%;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;float: left;\">&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div></div>";
                                        }
                                        else
                                        {
                                            strtttCord += "<div style=\"float:left;width:100%;margin-bottom:5px;\" class='item-radio-display'><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div>";
                                        }
                                    }
                                    else
                                    {
                                        if ((dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1) || (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1))
                                        {
                                            strwidthlenght += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####selectded####>" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale.Replace("<span class=\"buy-get-free\">", "").Replace("</span>", "") + strPrice + "</option>";
                                        }
                                        else
                                        {

                                            strvarinat += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####selectded####>" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale.Replace("<span class=\"buy-get-free\">", "").Replace("</span>", "") + strPrice + "</option>";
                                        }
                                        if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("casual") > -1)
                                        {
                                            strttt += "<div style=\"float:left;width:25%;margin-bottom:5px;\" class='item-radio-display'><div style=\"float: left; width: 100%;\"><img title=\"Casual\" alt=\"Casual\" src=\"/images/Casual.jpg\"></div><div style=\"margin-left: 30%;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div></div>";
                                        }
                                        else if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("soft fold") > -1)
                                        {
                                            strttt += "<div style=\"float:left;width:25%;margin-bottom:5px;\" class='item-radio-display'><div style=\"float: left; width: 100%;\"><img title=\"Soft Fold\" alt=\"Soft Fold\" src=\"/images/Soft_Fold.jpg\"></div><div style=\"margin-left: 30%;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div></div>";
                                        }
                                        else if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("front slat") > -1)
                                        {
                                            strttt += "<div style=\"float:left;width:25%;margin-bottom:5px;\" class='item-radio-display'><div style=\"float: left; width: 100%;\"><img title=\"Front Slat\" alt=\"Front Slat\" src=\"/images/Front_Slat.jpg\"></div><div style=\"margin-left: 30%;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div></div>";
                                        }
                                        else if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("relaxed") > -1)
                                        {
                                            strttt += "<div style=\"float:left;width:25%;margin-bottom:5px;\" class='item-radio-display'><div style=\"float: left; width: 100%;\"><img title=\"Relaxed\" alt=\"Relaxed\" src=\"/images/Relaxed.jpg\"></div><div style=\"margin-left: 30%;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div></div>";
                                        }
                                        else
                                        {
                                            strttt += "<div style=\"float:left;width:100%;margin-bottom:5px;\" class='item-radio-display'><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div>";
                                        }
                                        //strttt += "<div style=\"float:left;width:100%;margin-bottom:5px;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "," + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariant.Tables[0].Rows[i]["VariantId"].ToString() + "-" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\">&nbsp;" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div>";
                                    }
                                }
                                DataSet dsVariantparent = new DataSet();
                                dsVariantparent = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + productID + " AND isnull(ParentId,0)=" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "");
                                if (isChild == false && dsVariantparent.Tables.Count > 0 && dsVariantparent.Tables[0].Rows.Count > 0)
                                {
                                    //iNumb++;
                                    strvarinat = strvarinat.Replace("####selectded####", " selected");
                                    strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "','divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "');\"");
                                    Sid = Convert.ToInt32(dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString());
                                    strvarinatchild = "";
                                    radiobutton = "";
                                    strvarinatchild += "<div class=\"readymade-detail-pt1\" >";
                                    strvarinatchild += "<div class=\"pro-properties-left\" style=\"display:none;\" id=\"divvariantname-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "</div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "\" />";
                                    strvarinatchild += "<div class=\"readymade-detail-pt1-pro\" id=\"divcolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" onclick=\"varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ");\">";

                                    strvarinatchild += "<span id=\"spancolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + iNumb.ToString() + "</span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString();
                                    strvarinatchild += "<div id=\"divselvalue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"float:right;line-height:25px;padding-right:2px;\"></div>";
                                    strvarinatchild += "</div>";

                                    strvarinatchild += "<div class=\"readymade-detail-right-pro\" id='divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "' style=\"display:none;\">";
                                    strvarinatchild += "<select onchange=\"PriceChangeondropdown();\" style=\"display:none;\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" class=\"option1\" id=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" >";
                                    strvarinatchild += "<option value=\"0\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "</option>";
                                    DataSet dsVariantvaluechild = new DataSet();
                                    dsVariantvaluechild = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariantValue WHERE ProductID=" + productID + " AND VariantID=" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + " Order By ISNULL(DisplayOrder,0)");
                                    if (dsVariantvaluechild != null && dsVariantvaluechild.Tables.Count > 0 && dsVariantvaluechild.Tables[0].Rows.Count > 0)
                                    {
                                        for (int k = 0; k < dsVariantvaluechild.Tables[0].Rows.Count; k++)
                                        {
                                            IsOnSale = false;
                                            OnsalePrice = 0;
                                            if (CntReadymade == 1)
                                            {
                                                StrBuy1onsale = "";
                                                StrVarValueId = Convert.ToString(dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString());
                                                StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + productID + " and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and ISNULL(Buy1Get1,0)=1 and VariantValueID=" + StrVarValueId + "";
                                                Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                                if (Intcnt > 0)
                                                {
                                                    StrBuy1onsale = "<span class=\"buy-get-free\">(Buy 1 Get 1 Free)</span>";
                                                }

                                                StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + productID + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 and VariantValueID=" + StrVarValueId + "";
                                                Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                                if (Intcnt > 0)
                                                {
                                                    IsOnSale = true;
                                                    OnsalePrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(OnSalePrice,0) as OnSalePrice from tb_ProductVariantValue Where productid=" + productID + " and VariantValueID=" + StrVarValueId + " and ISNULL(OnSale,0)=1"));
                                                    StrBuy1onsale += "<span class=\"buy-get-free\">(On Sale)</span>";
                                                }
                                                StrQry = "Select Convert(char(10),BackOrderdate,101) from tb_ProductVariantValue Where productid=" + productID + " and (cast(BackOrderdate as date) >=cast(GETDATE() as date)) and VariantValueID=" + StrVarValueId + "";
                                                StrbackOrderdate = Convert.ToString(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                                if (!String.IsNullOrEmpty(StrbackOrderdate))
                                                {
                                                    StrbackOrderdate = " (Back Order Date: " + StrbackOrderdate + ")";
                                                }
                                            }

                                            strPrice = "";
                                            if (IsOnSale == true && OnsalePrice > decimal.Zero)
                                            {
                                                strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(OnsalePrice.ToString())) + ")";
                                            }
                                            else
                                            {
                                                if (Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString()) > Decimal.Zero)
                                                {
                                                    strPrice = "(+$" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString())) + ")";
                                                }
                                            }
                                            if (dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString().ToLower().IndexOf("custom") > -1)
                                            {
                                                checkmade = true;
                                                isCustom = true;
                                            }
                                            else
                                            {
                                                radiobutton += "<div style=\"float:left;width:100%;margin-bottom:5px;\" class='item-radio-display'><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "," + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\" class='item-radio-display-text'>&nbsp;" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice.Replace("(", " - ").Replace(")", "") + StrbackOrderdate + "</div></div>";
                                                strvarinatchild += "<option value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale.Replace("<span class=\"buy-get-free\">", "").Replace("</span>", "") + strPrice + "</option>";
                                            }
                                        }
                                    }
                                    //strvarinatchild += "</select><span><a title=\"Learn More\" href=\"javascript:void(0);\">Learn More</a></span></div>";
                                    strvarinatchild += "</select>" + radiobutton + strnextbutton + "</div>";
                                    strvarinatchild += "</div>";
                                    isChild = true;
                                }
                                else
                                {
                                    strvarinat = strvarinat.Replace("####selectded####", "");
                                    if (isChild)
                                    {
                                        if (Sid > 0)
                                        {
                                            strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "','divSelectvariant-" + Sid.ToString() + "');\"");
                                        }
                                        else
                                        {
                                            strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "','divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "');\"");
                                        }
                                    }
                                    else
                                    {
                                        strvarinat = strvarinat.Replace("####onchange####", "");
                                    }
                                }


                            }
                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1 || dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1)
                            {
                                strwidthlenght += "</select></div>";
                                //strvarinat += "<select ####onchange#### name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" class=\"option2\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >";
                                strwidthlenght += "<div class=\"selector fixedWidth\"><span style=\"-moz-user-select: none;\" id=\"spanvariant-exact-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</span>";
                                strwidthlenght += "<select class=\"option2\" onchange=\"PriceChangeondropdown();\" name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">";
                                strwidthlenght += "<option value=\"0\" style=\"display:none;\">Extra " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower() + "</option>";
                                strwidthlenght += "<option value=\"00\" selected=\"true\">00</option>";
                                strwidthlenght += "<option value=\"1/8\">1/8</option>";
                                strwidthlenght += "<option value=\"1/4\">1/4</option>";
                                strwidthlenght += "<option value=\"3/8\">3/8</option>";
                                strwidthlenght += "<option value=\"1/2\">1/2</option>";
                                strwidthlenght += "<option value=\"5/8\">5/8</option>";
                                strwidthlenght += "<option value=\"3/4\">3/4</option>";
                                strwidthlenght += "<option value=\"7/8\">7/8</option>";
                                strwidthlenght += "</select>";
                                strwidthlenght += "</div>";
                                strwidthlenght += "</div>";
                                IsWidth = true;


                            }
                            else if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("cord") > -1)
                            {
                                strtttCordvalue += "</select>";
                            }
                            //else { strvarinat += "</select><span><a title=\"Learn More\" href=\"javascript:void(0);\">Learn More</a></span></div>"; }
                            else { strvarinat += "</select>" + strttt + strnextbutton + "</div>"; }
                        }

                        //if ((i + 1) > 0)
                        //{
                        //    string StrVal = Convert.ToString(dsVariant.Tables[0].Rows[i + 1]["VariantName"].ToString());
                        //    if ((StrVal.ToString().ToLower().IndexOf("length") > -1) || (StrVal.ToString().ToLower().IndexOf("width") > -1))
                        //    { }
                        //    else
                        //        strvarinat += "</div>";
                        //}
                        //else
                        //{
                        //if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("width") > -1)
                        //{
                        //    strvarinat += "###wid-leg###";
                        //}
                        //else if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("length") > -1)
                        //{
                        //   // strvarinat += "</div>";
                        //}
                        //else
                        //{
                        if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("cord") <= -1)
                        {
                            strvarinat += "</div>";
                        }
                        //}
                        //}

                        strvarinat += strvarinatchild;

                        if (checkmade == true && isChild == false)
                        {
                        }
                        else
                        {
                        }

                    }

                    if (strvarinat.IndexOf("###width-length####") > -1)
                    {
                        strvarinat = strvarinat.Replace("###width-length####", strwidthlenght);
                    }

                    strvarinat += strmeasurement + stroptions + strmount + strtttCord + strtttCordvalue+ "</div></div>";
                    strvarinat += "</div>"; // End of divVariant
                    if (isCustom == false)
                    {
                        divcustom.Visible = false;
                        ltmadevariant.Visible = false;
                        licustom.Visible = false;
                    }
                    else
                    {
                        GetStyle();
                        GetOption();
                        ddlcustomlength.Attributes.Add("onchange", "ChangeCustomprice();");
                        ddlcustomoptin.Attributes.Add("onchange", "ChangeCustomprice();");
                        ddlcustomstyle.Attributes.Add("onchange", "ChangeCustomprice();");
                        ddlcustomwidth.Attributes.Add("onchange", "ChangeCustomprice();");
                        dlcustomqty.Attributes.Add("onchange", "ChangeCustomprice();");
                        //ChangeCustomprice
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "PriceChangeondropdown", "PriceChangeondropdown();", true);
                }
                else
                {
                    strvarinat += "<div id=\"divVariant\">";
                    strvarinat += "<div class=\"readymade-detail-pt1\">";
                    strvarinat += "<div style=\"display: none;\" class=\"pro-properties-left\">";
                    strvarinat += "SELECT MEASUREMENTS <span></span>";
                    strvarinat += "</div>";
                    strvarinat += "<input type=\"hidden\" value=\"Width\" name=\"hdnvariantname-0\" />";
                    strvarinat += "<div onclick=\"varianttabhideshow(0);\" id=\"divcolspan-0\" class=\"readymade-detail-pt1-pro active\">";
                    strvarinat += "<span id=\"spancolspan-0\">1</span>SELECT MEASUREMENTS";
                    strvarinat += "<div style=\"float: right; line-height: 25px; padding-right: 2px;\" id=\"divselvalue-0\">";
                    strvarinat += "</div>";
                    strvarinat += "</div>";
                    strvarinat += "<div style=\"display: block;\" id=\"divSelectvariant-0\" class=\"readymade-detail-right-pro\">";
                    strvarinat += " <div class=\"selector fixedWidth\">";
                    strvarinat += " <span id=\"spanvariant-0\" style=\"-moz-user-select: none;\">Width</span>";
                    strvarinat += "<div style=\"width: auto ! important;\" class=\"option-pt1\">";
                    strvarinat += "<span id=\"divvariantname-0\" style=\"float: left; padding-right: 2px; display: none;\">";
                    strvarinat += "Width</span>";
                    strvarinat += " </div>";
                    strvarinat += "<select id=\"Selectvariant-0\" class=\"option2\" name=\"Selectvariant-0\" onchange=\"PriceChangeondropdown();\">";
                    strvarinat += "<option value=\"0\">Width</option>";
                    strvarinat += "<option value=\"12\">12</option><option value=\"13\">13</option><option value=\"14\">14</option><option value=\"15\">15</option><option value=\"16\">16</option><option value=\"17\">17</option><option value=\"18\">18</option><option value=\"19\">19</option><option value=\"20\">20</option><option value=\"21\">21</option><option value=\"22\">22</option><option value=\"23\">23</option><option value=\"24\">24</option><option value=\"25\">25</option><option value=\"26\">26</option><option value=\"27\">27</option><option value=\"28\">28</option><option value=\"29\">29</option><option value=\"30\">30</option><option value=\"31\">31</option><option value=\"32\">32</option><option value=\"33\">33</option><option value=\"34\">34</option><option value=\"35\">35</option><option value=\"36\">36</option><option value=\"37\">37</option><option value=\"38\">38</option><option value=\"39\">39</option><option value=\"40\">40</option><option value=\"41\">41</option><option value=\"42\">42</option><option value=\"43\">43</option><option value=\"44\">44</option><option value=\"45\">45</option><option value=\"46\">46</option><option value=\"47\">47</option><option value=\"48\">48</option><option value=\"49\">49</option><option value=\"50\">50</option><option value=\"51\">51</option><option value=\"52\">52</option><option value=\"53\">53</option><option value=\"54\">54</option><option value=\"55\">55</option><option value=\"56\">56</option><option value=\"57\">57</option><option value=\"58\">58</option><option value=\"59\">59</option><option value=\"60\">60</option><option value=\"61\">61</option><option value=\"62\">62</option><option value=\"63\">63</option><option value=\"64\">64</option><option value=\"65\">65</option><option value=\"66\">66</option><option value=\"67\">67</option><option value=\"68\">68</option><option value=\"69\">69</option><option value=\"70\">70</option><option value=\"71\">71</option><option value=\"72\">72</option><option value=\"73\">73</option><option value=\"74\">74</option><option value=\"75\">75</option><option value=\"76\">76</option><option value=\"77\">77</option><option value=\"78\">78</option><option value=\"79\">79</option><option value=\"80\">80</option><option value=\"81\">81</option><option value=\"82\">82</option><option value=\"83\">83</option><option value=\"84\">84</option><option value=\"85\">85</option><option value=\"86\">86</option><option value=\"87\">87</option><option value=\"88\">88</option><option value=\"89\">89</option><option value=\"90\">90</option>";
                    strvarinat += "</select></div>";

                    strvarinat += "<div class=\"selector fixedWidth\">";
                    strvarinat += "<span id=\"spanvariant-99999999\" style=\"-moz-user-select: none;\" id=\"spanvariant-99999999\">Length</span><div style=\"width: auto ! important;\"";
                    strvarinat += " class=\"option-pt1\">";
                    strvarinat += "<span id=\"divvariantname-99999999\" style=\"float: left; padding-right: 2px; display: none;\">";
                    strvarinat += "Length</span></div>";
                    strvarinat += "<select id=\"Selectvariant-99999999\" class=\"option2\" name=\"Selectvariant-99999999\" onchange=\"PriceChangeondropdown();\">";
                    strvarinat += "<option value=\"0\">Length</option>";
                    strvarinat += "<option value=\"12\">12</option><option value=\"13\">13</option><option value=\"14\">14</option><option value=\"15\">15</option><option value=\"16\">16</option><option value=\"17\">17</option><option value=\"18\">18</option><option value=\"19\">19</option><option value=\"20\">20</option><option value=\"21\">21</option><option value=\"22\">22</option><option value=\"23\">23</option><option value=\"24\">24</option><option value=\"25\">25</option><option value=\"26\">26</option><option value=\"27\">27</option><option value=\"28\">28</option><option value=\"29\">29</option><option value=\"30\">30</option><option value=\"31\">31</option><option value=\"32\">32</option><option value=\"33\">33</option><option value=\"34\">34</option><option value=\"35\">35</option><option value=\"36\">36</option><option value=\"37\">37</option><option value=\"38\">38</option><option value=\"39\">39</option><option value=\"40\">40</option><option value=\"41\">41</option><option value=\"42\">42</option><option value=\"43\">43</option><option value=\"44\">44</option><option value=\"45\">45</option><option value=\"46\">46</option><option value=\"47\">47</option><option value=\"48\">48</option><option value=\"49\">49</option><option value=\"50\">50</option><option value=\"51\">51</option><option value=\"52\">52</option><option value=\"53\">53</option><option value=\"54\">54</option><option value=\"55\">55</option><option value=\"56\">56</option><option value=\"57\">57</option><option value=\"58\">58</option><option value=\"59\">59</option><option value=\"60\">60</option><option value=\"61\">61</option><option value=\"62\">62</option><option value=\"63\">63</option><option value=\"64\">64</option><option value=\"65\">65</option><option value=\"66\">66</option><option value=\"67\">67</option><option value=\"68\">68</option><option value=\"69\">69</option><option value=\"70\">70</option><option value=\"71\">71</option><option value=\"72\">72</option><option value=\"73\">73</option><option value=\"74\">74</option><option value=\"75\">75</option><option value=\"76\">76</option><option value=\"77\">77</option><option value=\"78\">78</option><option value=\"79\">79</option><option value=\"80\">80</option><option value=\"81\">81</option><option value=\"82\">82</option><option value=\"83\">83</option><option value=\"84\">84</option><option value=\"85\">85</option><option value=\"86\">86</option><option value=\"87\">87</option><option value=\"88\">88</option><option value=\"89\">89</option><option value=\"90\">90</option>";
                    strvarinat += "</select></div>";
                    strvarinat += "<div class=\"selector fixedWidth\">";
                    strvarinat += "<span style=\"-moz-user-select: none;\" id=\"spanvariant-exact-99999999\">00</span><select";
                    strvarinat += " class=\"option2\" onchange=\"PriceChangeondropdown();\" name=\"Selectvariant-exact-99999999\"";
                    strvarinat += "id=\"Selectvariant-exact-99999999\"><option value=\"0\" style=\"display: none;\">Extra length</option>";
                    strvarinat += "<option value=\"00\" selected=\"true\">00</option>";
                    strvarinat += "<option value=\"1/8\">1/8</option>";
                    strvarinat += "<option value=\"1/4\">1/4</option>";
                    strvarinat += "<option value=\"3/8\">3/8</option>";
                    strvarinat += "<option value=\"1/2\">1/2</option>";
                    strvarinat += "<option value=\"5/8\">5/8</option>";
                    strvarinat += "<option value=\"3/4\">3/4</option>";
                    strvarinat += "<option value=\"7/8\">7/8</option>";
                    strvarinat += "</select><span style=\"float: left; padding-right: 2px; display: none;\" id=\"divvariantname-exact-99999999\">Length</span></div>";
                    strvarinat += "</div>";
                    strvarinat += "</div>";
                    strvarinat += "</div>";
                    divcustom.Visible = false;
                    licustom.Visible = false;
                    ltmadevariant.Visible = false;
                }
                ltvariant.Text = strvarinat;
                ltmadevariant.Text = strvarinatcustom;
                if (IsCustomProductId == true)
                {
                    divcustom.Visible = true;
                    ltmadevariant.Visible = true;
                    licustom.Visible = true;
                    GetStyle();
                    GetOption();
                    ddlcustomlength.Attributes.Add("onchange", "ChangeCustomprice();");
                    ddlcustomoptin.Attributes.Add("onchange", "ChangeCustomprice();");
                    ddlcustomstyle.Attributes.Add("onchange", "ChangeCustomprice();");
                    ddlcustomwidth.Attributes.Add("onchange", "ChangeCustomprice();");
                    dlcustomqty.Attributes.Add("onchange", "ChangeCustomprice();");
                }
                else
                {
                    divcustom.Visible = false;
                    licustom.Visible = false;
                    ltmadevariant.Visible = false;
                }
            }
            else
            {
                Response.Redirect("/index.aspx");
            }

        }

        /// <summary>
        /// Gets the Optinal Product Details
        /// </summary>
        /// <param name="SKUs">String SKUs</param>
        private void GetOptinalProductDetails(String SKUs)
        {
            DataSet dsOA = new DataSet();
            dsOA = ProductComponent.GetOptinalProductDetailByID(SKUs, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsOA != null && dsOA.Tables.Count > 0 && dsOA.Tables[0].Rows.Count > 0)
            {
                gvOptionalAcc.DataSource = dsOA.Tables[0];
                gvOptionalAcc.DataBind();
                // divOptionalAccessories.Visible = true;
            }
            else
            {
                gvOptionalAcc.DataSource = null;
                gvOptionalAcc.DataBind();
                divOptionalAccessories.Visible = false;
            }
        }
        #endregion

        /// <summary>
        /// Get Variant Details By product ID
        /// </summary>
        /// <param name="productID">int product ID</param>
        private void GetVarinatByProductID(int productID)
        {
            strScriptVar = "";
            //strScriptVar = "function checkvariantvalidation(){" + System.Environment.NewLine;
            DataSet dsVariant = new DataSet();
            dsVariant = ProductComponent.GetProductVariantByproductID(productID);
            divAttributes.Controls.Clear();
            if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
            {
                string strVName = "";

                for (int i = 0; i < dsVariant.Tables[0].Rows.Count; i++)
                {
                    DataTable dtVariant = new DataTable();
                    dtVariant = dsVariant.Tables[0].Clone();
                    if (strVName != dsVariant.Tables[0].Rows[i]["VariantName"].ToString())
                    {
                        DataRow[] dr = dsVariant.Tables[0].Select("VariantName ='" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("'", "''") + "'");
                        foreach (DataRow dr1 in dr)
                        {
                            object[] drAll = dr1.ItemArray;
                            dtVariant.Rows.Add(drAll);
                        }
                        Literal ltrText;
                        ltrText = new Literal();
                        ltrText.ID = "ltr_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString();
                        ltrText.Text = "<div class=\"details-row\" style='color:#3D7453;Font-size:13px;'><div class=\"details-left\"><span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</span> :</div><div class=\"details-right\">";
                        divAttributes.Controls.Add(ltrText);

                        DropDownList ddlvaraint = new DropDownList();
                        ddlvaraint.ID = "ddlVariant_" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString();
                        ddlvaraint.AutoPostBack = false;
                        ddlvaraint.CssClass = "select-box";
                        ddlvaraint.Attributes.Add("style", "width:220px;");
                        ddlvaraint.DataSource = dtVariant;
                        ddlvaraint.DataTextField = "priceVariantValue";
                        ddlvaraint.DataValueField = "VariantValueID";
                        ddlvaraint.Attributes.Add("onchange", "ChangePrice();");


                        strScriptVar += "if(document.getElementById('ContentPlaceHolder1_" + ddlvaraint.ClientID.ToString() + "') != null &&  document.getElementById('ContentPlaceHolder1_" + ddlvaraint.ClientID.ToString() + "').selectedIndex==0)" + System.Environment.NewLine;
                        strScriptVar += "{" + System.Environment.NewLine;
                        strScriptVar += "jAlert('Please select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().Replace("'", @"\'") + "','Required Information','ContentPlaceHolder1_" + ddlvaraint.ClientID.ToString() + "');" + System.Environment.NewLine;
                        strScriptVar += "return false;" + System.Environment.NewLine;
                        strScriptVar += "}" + System.Environment.NewLine;

                        ddlvaraint.DataBind();
                        ddlvaraint.Items.Insert(0, new ListItem("Select " + dsVariant.Tables[0].Rows[i]["VariantName"].ToString(), "0"));
                        if (ViewState["VariantValueId"] != null)
                        {
                            for (int j = 0; j < ddlvaraint.Items.Count; j++)
                            {
                                if (ViewState["VariantValueId"].ToString().IndexOf("," + ddlvaraint.Items[j].Value.ToString() + ",") > -1)
                                {
                                    ddlvaraint.SelectedValue = ddlvaraint.Items[j].Value.ToString();
                                }
                            }
                        }
                        else
                        {
                            ddlvaraint.SelectedIndex = 0;
                        }
                        divAttributes.Controls.Add(ddlvaraint);
                        ltrText = new Literal();
                        ltrText.Text = "</div></div>";
                        divAttributes.Controls.Add(ltrText);
                    }
                    strVName = dsVariant.Tables[0].Rows[i]["VariantName"].ToString();
                }
            }
        }

        /// <summary>
        ///  Display tab
        /// </summary>
        /// <param name="dsProduct">DataSet dsProduct</param>
        private void GetTabs(DataSet dsProduct)
        {
            ltrTabs.Text = "";
            ltrTabs.Text += "<div class=\"tabbertab\">";
            if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["TabTitle1"].ToString()))
            {
                //ltrTabs.Text += "<h6><a title=\"" + Server.HtmlEncode(Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle1"])) + "\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle1"]) + "</a></h6>";
            }
            else
            {
                //ltrTabs.Text += "<h6><a title=\"Product Description\">Product Description</a></h6>";
            }
            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Description"].ToString()))
            //{
            //    divdescription.Visible = true;
            //}

            // String result = System.Text.RegularExpressions.Regex.Replace(dsProduct.Tables[0].Rows[0]["Description"].ToString().Trim(), @"<[^>]*>", String.Empty);
            //ltrTabs.Text += "<div class=\"item-tabing-text\">" + result.Trim().Replace("<p>", "").Replace("</p>", "").Replace("<br>", "").Replace("</br>", "").Replace("<br />", "");
            if (string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["Description"].ToString()))
            {
                divdiscription.Visible = false;
                divdiscription1.Visible = false;
            }
            else
            {
                ltrTabs.Text += "<div class=\"item-tabing-text\">" + dsProduct.Tables[0].Rows[0]["Description"].ToString();
                // ltrTabs.Text += "<div class=\"item-tabing-text\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["Description"]);
                ltrTabs.Text += "</div>";
            }
            ltrTabs.Text += "</div>";
            //bool tabdisplay = false;
            //bool check = bool.TryParse(dsProduct.Tables[0].Rows[0]["isTabbingDisplay"].ToString(), out tabdisplay);
            //if (tabdisplay)
            //{
            //    if (!String.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["TabTitle2"].ToString()))
            //    {
            //        ltrTabs.Text += "<div class=\"tabbertab\">";
            //        ltrTabs.Text += "<h6><a title=\"" + Server.HtmlEncode(Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle2"])) + "\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle2"]) + "</a></h6>";
            //        ltrTabs.Text += "<div class=\"item-tabing-text\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabDesc2"]); //Convert.ToString(dsProduct.Tables[0].Rows[0]["TabDesc2"]

            //        ltrTabs.Text += "</div>";
            //        ltrTabs.Text += "</div>";
            //        divdiscription.Visible = true;
            //        // divdescription.Visible = true;
            //    }
            //    if (!String.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["TabTitle3"].ToString()))
            //    {
            //        ltrTabs.Text += "<div class=\"tabbertab\">";
            //        ltrTabs.Text += "<h6><a title=\"" + Server.HtmlEncode(Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle3"])) + "\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle3"]) + "</a></h6>";
            //        ltrTabs.Text += "<div class=\"item-tabing-text\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabDesc3"]);
            //        //  ltrTabs.Text += "<div class=\"item-tabing-text\">";
            //        ltrTabs.Text += "</div>";
            //        ltrTabs.Text += "</div>";
            //        divdiscription.Visible = true;
            //        // divdescription.Visible = true;
            //    }
            //    if (!String.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["TabTitle4"].ToString()))
            //    {
            //        ltrTabs.Text += "<div class=\"tabbertab\">";
            //        ltrTabs.Text += "<h6><a title=\"" + Server.HtmlEncode(Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle4"])) + "\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle4"]) + "</a></h6>";
            //        ltrTabs.Text += "<div class=\"item-tabing-text\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabDesc4"]);
            //        // ltrTabs.Text += "<div class=\"item-tabing-text\">";
            //        ltrTabs.Text += "</div>";
            //        ltrTabs.Text += "</div>";
            //        divdiscription.Visible = true;
            //        // divdescription.Visible = true;
            //    }
            //    if (!String.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["TabTitle5"].ToString()))
            //    {
            //        ltrTabs.Text += "<div class=\"tabbertab\">";
            //        ltrTabs.Text += "<h6><a title=\"" + Server.HtmlEncode(Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle5"])) + "\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabTitle5"]) + "</a></h6>";
            //        ltrTabs.Text += "<div class=\"item-tabing-text\">" + Convert.ToString(dsProduct.Tables[0].Rows[0]["TabDesc5"]);
            //        //ltrTabs.Text += "<div class=\"item-tabing-text\">";
            //        ltrTabs.Text += "</div>";
            //        ltrTabs.Text += "</div>";
            //        divdiscription.Visible = true;
            //        //divdescription.Visible = true;
            //    }


            //}


        }

        /// Display Box in Right Side
        /// </summary>
        /// <param name="dsProduct">DataSet dsProduct</param>
        private void GetBoxes(DataSet dsProduct)
        {
            ltrBox.Text = "";

            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["isFree19InchLCD"].ToString()) && Convert.ToBoolean(dsProduct.Tables[0].Rows[0]["isFree19InchLCD"].ToString()))
            //{
            //    ltrBox.Text += "<div class=\"sub-section-banner\">";
            //    //ltrBox.Text += "<a title=\"\" href=\"javascript:void(0);\">";
            //    ltrBox.Text += "<img height=\"60\" width=\"212\" title=\"\" alt=\"\" src=\"/images/free-19-monitor.jpg\" />";
            //    //ltrBox.Text += "</a>";
            //    ltrBox.Text += " </div>";
            //}
            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["isFree22InchLCD"].ToString()) && Convert.ToBoolean(dsProduct.Tables[0].Rows[0]["isFree22InchLCD"].ToString()))
            //{
            //    ltrBox.Text += "<div class=\"sub-section-banner\">";
            //    //ltrBox.Text += "<a title=\"\" href=\"javascript:void(0);\">";
            //    ltrBox.Text += "<img height=\"60\" width=\"212\" title=\"\" alt=\"\" src=\"/images/free-22-monitor.jpg\" />";
            //    //ltrBox.Text += "</a>";
            //    ltrBox.Text += " </div>";
            //}
            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["isSatisfactionGuaranteed"].ToString()) && Convert.ToBoolean(dsProduct.Tables[0].Rows[0]["isSatisfactionGuaranteed"].ToString()))
            //{
            //    ltrBox.Text += "<div class=\"sub-section-banner\">";
            //    //ltrBox.Text += "<a title=\"\" href=\"javascript:void(0);\">";
            //    ltrBox.Text += "<img height=\"60\" width=\"212\" title=\"\" alt=\"\" src=\"/images/satisfaction.jpg\" />";
            //    //ltrBox.Text += "</a>";
            //    ltrBox.Text += " </div>";
            //}
            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["is3YearDVRWarranty"].ToString()) && Convert.ToBoolean(dsProduct.Tables[0].Rows[0]["is3YearDVRWarranty"].ToString()))
            //{
            //    ltrBox.Text += "<div class=\"sub-section-banner\">";
            //    //ltrBox.Text += "<a title=\"\" href=\"javascript:void(0);\">";
            //    ltrBox.Text += "<img height=\"60\" width=\"212\" title=\"\" alt=\"\" src=\"/images/3-years-dvr-warranty.jpg\" />";
            //    //ltrBox.Text += "</a>";
            //    ltrBox.Text += " </div>";
            //}
            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["isFreeDVRUpgrade"].ToString()) && Convert.ToBoolean(dsProduct.Tables[0].Rows[0]["isFreeDVRUpgrade"].ToString()))
            //{
            //    ltrBox.Text += "<div class=\"sub-section-banner\">";
            //    //ltrBox.Text += "<a title=\"\" href=\"javascript:void(0);\">";
            //    ltrBox.Text += "<img height=\"60\" width=\"212\" title=\"\" alt=\"\" src=\"/images/free-dvr-upgrade.jpg\" />";
            //    //ltrBox.Text += "</a>";
            //    ltrBox.Text += " </div>";
            //}
            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["isFreeHDUpgrade"].ToString()) && Convert.ToBoolean(dsProduct.Tables[0].Rows[0]["isFreeHDUpgrade"].ToString()))
            //{
            //    ltrBox.Text += "<div class=\"sub-section-banner\">";
            //    //ltrBox.Text += "<a title=\"\" href=\"javascript:void(0);\">";
            //    ltrBox.Text += "<img height=\"60\" width=\"212\" title=\"\" alt=\"\" src=\"/images/free-hdu-upgrade.jpg\" />";
            //    //ltrBox.Text += "</a>";
            //    ltrBox.Text += " </div>";
            //}
            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["isFreeDVDBurner"].ToString()) && Convert.ToBoolean(dsProduct.Tables[0].Rows[0]["isFreeDVDBurner"].ToString()))
            //{
            //    ltrBox.Text += "<div class=\"sub-section-banner\">";
            //    //ltrBox.Text += "<a title=\"\" href=\"javascript:void(0);\">";
            //    ltrBox.Text += "<img height=\"60\" width=\"212\" title=\"\" alt=\"\" src=\"/images/free-dvd-burner.jpg\" />";
            //    //ltrBox.Text += "</a>";
            //    ltrBox.Text += " </div>";
            //}
            //if (!string.IsNullOrEmpty(dsProduct.Tables[0].Rows[0]["isFreeTechSupport"].ToString()) && Convert.ToBoolean(dsProduct.Tables[0].Rows[0]["isFreeTechSupport"].ToString()))
            //{
            //    ltrBox.Text += "<div class=\"sub-section-banner\">";
            //    //ltrBox.Text += "<a title=\"\" href=\"javascript:void(0);\">";
            //    ltrBox.Text += "<img height=\"60\" width=\"212\" title=\"\" alt=\"\" src=\"/images/free-technical.jpg\" />";
            //    //ltrBox.Text += "</a>";
            //    ltrBox.Text += " </div>";
            //}
        }

        /// <summary>
        /// Get Comment from Customer By ProductID
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        private void GetCommnetByCustomer(int ProductID)
        {
            DataSet dsComment = new DataSet();
            dsComment = ProductComponent.GetProductRatingCount(ProductID, ddlSortReviewCnt.SelectedValue.ToString().ToLower());
            ltreviewDetail.Text = "";
            if (dsComment != null && dsComment.Tables.Count > 0 && dsComment.Tables[0].Rows.Count > 0)
            {
                divReviewSort.Visible = true;
                divReviewSort1.Visible = true;
                //ltreviewDetail.Text += "<div class='review-main'>";

                //ltreviewDetail.Text += "<div class='review-paging'>";
                //ltreviewDetail.Text += " <p> <strong>Showing 1-20 of 94 reviews for this product</strong></p>";
                //ltreviewDetail.Text += "</div>";

                //ltreviewDetail.Text += "<p class='write-review-link'><div class='comment-sort-by'><select class='option'><option>Helpfulness - High to Low</option></select> <span>Sort Reviews By :</span></div></p>";

                //ltreviewDetail.Text += "</div>";

                for (int i = 0; i < dsComment.Tables[0].Rows.Count; i++)
                {

                    ltreviewDetail.Text += "<div class=\"comment-detail\">";

                    ltreviewDetail.Text += "<div class=\"comment-detail-title\">";
                    ltreviewDetail.Text += "</div>";

                    ltreviewDetail.Text += "<div class=\"comment-detail-left\">";
                    ltreviewDetail.Text += "<p class='comment-rating'><span>Rating</span>";

                    if (Convert.ToInt32(dsComment.Tables[0].Rows[i]["Rating"]) == 1)
                    {
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img  class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img  class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                    }
                    else if (Convert.ToInt32(dsComment.Tables[0].Rows[i]["Rating"]) == 2)
                    {
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                    }
                    else if (Convert.ToInt32(dsComment.Tables[0].Rows[i]["Rating"]) == 3)
                    {
                        ltreviewDetail.Text += "<img class='img-left'   title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'   title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                    }
                    else if (Convert.ToInt32(dsComment.Tables[0].Rows[i]["Rating"]) == 4)
                    {
                        ltreviewDetail.Text += "<img class='img-left'   title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'   title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img  class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                    }
                    else if (Convert.ToInt32(dsComment.Tables[0].Rows[i]["Rating"]) == 5)
                    {
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'   title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'   title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form.jpg\">";
                    }
                    else
                    {
                        ltreviewDetail.Text += "<img class='img-left'   title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                        ltreviewDetail.Text += "<img class='img-left'  title=\"\" alt=\"\" src=\"/images/star-form1.jpg\">";
                    }

                    ltreviewDetail.Text += "</p>";

                    ltreviewDetail.Text += "<p><strong>" + Convert.ToString(dsComment.Tables[0].Rows[i]["FName"]) + "</strong>";
                    ltreviewDetail.Text += "</p>";

                    //ltreviewDetail.Text += "<p><span>" + String.Format("{0:ddddddddd, dd MMMMMMMMM yyyy}", Convert.ToDateTime(dsComment.Tables[0].Rows[i]["CreatedOn"])) + "</span>";
                    //ltreviewDetail.Text += "</p>";
                    ltreviewDetail.Text += "</div>";

                    ltreviewDetail.Text += "<div class='comment-detail-right'>";
                    ltreviewDetail.Text += "<p>" + Convert.ToString(dsComment.Tables[0].Rows[i]["Comments"]) + "</p>";


                    ltreviewDetail.Text += "<p><strong>Was This Review Helpful ?</strong></p>";
                    ltreviewDetail.Text += "<div class=\"comment-report\">";

                    int TotYes = 0, TotNo = 0;
                    DataSet dsReviewComment = new DataSet();
                    string IPAddress = Convert.ToString(HttpContext.Current.Request.UserHostAddress);
                    if (!string.IsNullOrEmpty(IPAddress.ToString().Trim()))
                    {
                        ltreviewDetail.Text += "<div class=\"comment-report-pt1\">";
                        dsReviewComment = CommonComponent.GetCommonDataSet("SELECT dbo.tb_Rating.RatingID,sum(ISNULL(tb_ReviewCount.yes,0)) as TotYes,sum(ISNULL(tb_ReviewCount.no,0)) as TotNo FROM dbo.tb_Rating LEFT OUTER JOIN dbo.tb_ReviewCount ON dbo.tb_Rating.RatingID = dbo.tb_ReviewCount.RatingID Where tb_Rating.RatingID =" + dsComment.Tables[0].Rows[i]["RatingID"].ToString() + " group by tb_Rating.RatingID");
                        if (dsReviewComment != null && dsReviewComment.Tables.Count > 0 && dsReviewComment.Tables[0].Rows.Count > 0)
                        {
                            TotYes = Convert.ToInt32(dsReviewComment.Tables[0].Rows[0]["TotYes"].ToString());
                            TotNo = Convert.ToInt32(dsReviewComment.Tables[0].Rows[0]["TotNo"].ToString());
                        }
                        ltreviewDetail.Text += "<span id=\"spn_Yes-" + dsComment.Tables[0].Rows[i]["RatingID"].ToString() + "\">" + TotYes + "</span>";
                        ltreviewDetail.Text += "<a id=\"Review_Yes\" onclick=\"ReviewCount(" + dsComment.Tables[0].Rows[i]["RatingID"].ToString() + ",'yes');\"><img src=\"images/yes-icon.jpg\" alt=\"\" title=\"\" class=\"img-left\"></a>";
                        ltreviewDetail.Text += "<span>Yes</span>";
                        ltreviewDetail.Text += "</div>";

                        ltreviewDetail.Text += "<div class=\"comment-report-pt1\">";
                        ltreviewDetail.Text += "<span id=\"spn_No-" + dsComment.Tables[0].Rows[i]["RatingID"].ToString() + "\">" + TotNo + "</span>";
                        ltreviewDetail.Text += "<a id=\"Review_No\" onclick=\"ReviewCount(" + dsComment.Tables[0].Rows[i]["RatingID"].ToString() + ",'no');\"><img src=\"images/no-icon.jpg\" alt=\"\" title=\"\" class=\"no-icon\"></a>";
                        ltreviewDetail.Text += "<span>No</span>";
                        ltreviewDetail.Text += "</div>";

                        ltreviewDetail.Text += "<div class=\"comment-report-pt1\">";
                        ltreviewDetail.Text += "<a style='color:#848383;' href=\"javascript:void(0);\" onclick=\"ShowModelForInappropriateRating('/ReviewInappropriateRating.aspx?RatingID=" + dsComment.Tables[0].Rows[i]["RatingID"].ToString() + "')\"><img src=\"images/flag-icon.jpg\" alt=\"\" title=\"\" class=\"img-left\">";
                        ltreviewDetail.Text += "<span>Report Inappropriate Content</a></span>";
                        ltreviewDetail.Text += "</div>";
                    }
                    ltreviewDetail.Text += "</div>";

                    ltreviewDetail.Text += "</div>";
                    ltreviewDetail.Text += "</div>";
                    ltreviewDetail.Text += "<div class=\"comment-detail\">";
                    ltreviewDetail.Text += "<p><strong></strong></p>";
                    ltreviewDetail.Text += "<p><span style='float:right;'>";
                    ltreviewDetail.Text += "</span></p>";
                    ltreviewDetail.Text += "<p></p>";
                    ltreviewDetail.Text += "</div>";
                }
                ltRating.Text = "";
                decimal dd = Math.Truncate(Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]));
                decimal ddnew = Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]) - dd;

                if (dd == Convert.ToDecimal(1))
                {
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltRating.Text += "<img  src=\"/images/25-star.jpg\" class='img-left'>";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/rating-half.jpg\" class='img-left'>";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/75-star.jpg\" class='img-left'>";
                    }
                    else
                    {
                        ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    }
                    ltRating.Text += "<img src=\"/images/star-form1.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    ltAvarageRate.Text += " <a style='cursor:default;'>Rating " + dsComment.Tables[0].Rows[0]["AvgRating"].ToString() + "</a>|<a style='cursor:pointer;' onclick=\"$('html, body').animate({ scrollTop: $('#divImage').offset().top }, 'slow'); \">" + dsComment.Tables[0].Rows.Count.ToString() + " Review</a>|";
                }
                else if (dd == Convert.ToDecimal(2))
                {

                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltRating.Text += "<img  src=\"/images/25-star.jpg\" class='img-left'>";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/rating-half.jpg\" class='img-left'>";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/75-star.jpg\" class='img-left'>";
                    }
                    else
                    {
                        ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    }

                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    ltAvarageRate.Text += " <a style='cursor:default;'>Rating " + dsComment.Tables[0].Rows[0]["AvgRating"].ToString() + "</a>|<a style='cursor:pointer;' onclick=\"$('html, body').animate({ scrollTop: $('#divImage').offset().top }, 'slow'); \">" + dsComment.Tables[0].Rows.Count.ToString() + " Review</a>|";
                }
                else if (dd == Convert.ToDecimal(3))
                {


                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltRating.Text += "<img  src=\"/images/25-star.jpg\" class='img-left'>";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/rating-half.jpg\" class='img-left'>";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/75-star.jpg\" class='img-left'>";
                    }
                    else
                    {
                        ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    }


                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    ltAvarageRate.Text += " <a style='cursor:default;'>Rating " + dsComment.Tables[0].Rows[0]["AvgRating"].ToString() + "</a>|<a style='cursor:pointer;' onclick=\"$('html, body').animate({ scrollTop: $('#divImage').offset().top }, 'slow'); \">" + dsComment.Tables[0].Rows.Count.ToString() + " Review</a>|";
                }
                else if (dd == Convert.ToDecimal(4))
                {

                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltRating.Text += "<img  src=\"/images/25-star.jpg\" class='img-left'>";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/rating-half.jpg\" class='img-left'>";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/75-star.jpg\" class='img-left'>";
                    }
                    else
                    {
                        ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                    }
                    ltAvarageRate.Text += "<a style='cursor:default;'>Rating " + dsComment.Tables[0].Rows[0]["AvgRating"].ToString() + "</a>|<a style='cursor:pointer;' onclick=\"$('html, body').animate({ scrollTop: $('#divImage').offset().top }, 'slow'); \">" + dsComment.Tables[0].Rows.Count.ToString() + " Review</a>|";
                }
                else if (dd == Convert.ToDecimal(5))
                {

                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\" class='img-left'>";
                    ltAvarageRate.Text += "<a style='cursor:default;' >Rating 5</a>|<a style='cursor:pointer;' onclick=\"$('html, body').animate({ scrollTop: $('#divImage').offset().top }, 'slow'); \">" + dsComment.Tables[0].Rows.Count.ToString() + " Review</a>|";
                }
            }
            else
            {
                divCustomerRating.Visible = false;
                divReviewSort.Visible = false;
                divReviewSort1.Visible = false;
                ltreviewDetail.Text += "<div class=\"comment-detail\"><p>";
                ltreviewDetail.Text += "<font style=' color: #B92127;font-size: 13px;'>No reviews are available for this product.</font>";
                ltreviewDetail.Text += "</p></div>";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\" class='img-left'>";
                ltAvarageRate.Text += "";

            }
        }

        /// <summary>
        /// Get Recently Product By product Id
        /// </summary>
        /// <param name="ProductID">int Product ID</param>
        private void GetRecentlyProduct(string ProductID, int OrgProductID)
        {
            DataSet dsRecently = new DataSet();
            ltrRecently.Text = "";

            dsRecently = ProductComponent.GetRecentlyProduct(ProductID, Convert.ToInt32(AppLogic.AppConfigs("StoreID")), OrgProductID);
            if (dsRecently != null && dsRecently.Tables.Count > 0 && dsRecently.Tables[0].Rows.Count > 0)
            {
                // ltrRecently.Text += "<div class=\"or-main\" >";
                for (Int32 i = 0; i < dsRecently.Tables[0].Rows.Count; i++)
                {
                    //if (i != dsRecently.Tables[0].Rows.Count - 1)
                    //{

                    //    ltrRecently.Text += "<div class='fp-box'>";
                    //    ltrRecently.Text += "<div class='fp-display' style='margin-top:10px;'>";

                    //}
                    //else
                    //{
                    ltrRecently.Text += " <li style=\"padding-right:1px !important;\">";
                    ltrRecently.Text += " <div class=\"bs-box\">";
                    ltrRecently.Text += "<div class=\"bs-display\">";
                    //ltrRecently.Text += "<div class='fp-box'>";
                    //ltrRecently.Text += "<div class='fp-display' style='margin-top:10px;'>";
                    //}

                    decimal price = 0;
                    decimal salePrice = 0;
                    if (!string.IsNullOrEmpty(dsRecently.Tables[0].Rows[i]["price"].ToString()))
                    {
                        price = Convert.ToDecimal(dsRecently.Tables[0].Rows[i]["price"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsRecently.Tables[0].Rows[i]["saleprice"].ToString()))
                    {
                        salePrice = Convert.ToDecimal(dsRecently.Tables[0].Rows[i]["saleprice"].ToString());
                    }
                    ltrRecently.Text += "<div class=\"bs-box-div\" style='position: relative;'> <div class='img-center'> ";
                    string StrTagName = Convert.ToString(dsRecently.Tables[0].Rows[i]["TagName"].ToString());


                    ltrRecently.Text += "<a   title=\"" + SetAttribute(dsRecently.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsRecently.Tables[0].Rows[i]["ProductURL"].ToString() + "\">";

                    Random rd = new Random();
                    if (!string.IsNullOrEmpty(dsRecently.Tables[0].Rows[i]["imagename"].ToString()))
                    {
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsRecently.Tables[0].Rows[i]["imagename"].ToString())))
                        {
                            ltrRecently.Text += "<img id=\"imgRecentlyProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsRecently.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsRecently.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsRecently.Tables[0].Rows[i]["imagename"].ToString() + "?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_img" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";

                        }
                        else
                        {
                            ltrRecently.Text += "<img id=\"imgRecentlyProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsRecently.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsRecently.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_img" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";
                        }
                    }
                    else
                    {
                        ltrRecently.Text += "<img id=\"imgRecentlyProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsRecently.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsRecently.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_img" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";
                    }

                    if (ViewState["strRecentlyimg"] != null)
                    {
                        ViewState["strRecentlyimg"] = ViewState["strRecentlyimg"].ToString() + "$('#loader_img" + 1.ToString() + "').show();$('#imgRecentlyProduct" + i.ToString() + "').hide();$('#imgRecentlyProduct" + i.ToString() + "').load(function () {$('#loader_img" + i.ToString() + "').hide();$('#imgRecentlyProduct" + i.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                        //ViewState["strFeaturedimg"] = ViewState["strFeaturedimg"].ToString() + "$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();});";
                    }
                    else
                    {
                        ViewState["strRecentlyimg"] = "$('#loader_img" + i.ToString() + "').show();$('#imgRecentlyProduct" + i.ToString() + "').hide();$('#imgRecentlyProduct" + i.ToString() + "').load(function () {$('#loader_img" + i.ToString() + "').hide();$('#imgRecentlyProduct" + i.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                        // ViewState["strFeaturedimg"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();});";
                    }
                    if (ViewState["strRecentlyimgpostback"] != null)
                    {
                        ViewState["strRecentlyimgpostback"] = ViewState["strRecentlyimgpostback"].ToString() + "$('#loader_img" + i.ToString() + "').hide();";
                    }
                    else
                    {
                        ViewState["strRecentlyimgpostback"] = "$('#loader_img" + i.ToString() + "').hide();";
                    }

                    ltrRecently.Text += " <img src='/images/view-detail.png' alt='View Detail' class='preview' width='150' height='30' title='View Detail'></a>";
                    // Bind TagName

                    if (!string.IsNullOrEmpty(StrTagName))
                    {
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsRecently.Tables[0].Rows[i]["imagename"].ToString())))
                        {
                            ltrRecently.Text += "<img title='" + StrTagName.ToString().Trim() + "' src=\"/images/" + StrTagName.ToString().Trim() + ".png\" alt=\"" + StrTagName.ToString().Trim() + "\" class='" + StrTagName.ToString().ToLower() + "' />";
                        }
                    }
                    ltrRecently.Text += "</div></div>";


                    // ltrRecently.Text += "<h2 class='fp-box-h2' style='height:37px;'><a title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsYouMay.Tables[0].Rows[i]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[i]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[i]["productId"].ToString() + ".aspx\">" + SetName(dsYouMay.Tables[0].Rows[i]["Sortname"].ToString()) + "</a></h2>";
                    ltrRecently.Text += "<h2 class='bs-box-h2' style='height:30px;'><a title=\"" + SetAttribute(dsRecently.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsRecently.Tables[0].Rows[i]["ProductURL"].ToString() + "\">" + SetName(dsRecently.Tables[0].Rows[i]["Sortname"].ToString()) + "</a></h2>";
                    ltrRecently.Text += "<p class='bs-box-p'>";
                    //if (price > decimal.Zero)
                    //{
                    //    if (salePrice > decimal.Zero && price > salePrice)
                    //    {
                    //        ltrRecently.Text += "Regular Price: " + price.ToString("C") + "";
                    //    }
                    //    else
                    //    {
                    //        ltrRecently.Text += "Price: " + price.ToString("C") + "";
                    //    }
                    //}
                    //else
                    //{
                    //    ltrYoumay.Text += "&nbsp;";
                    //}

                    if (salePrice > decimal.Zero && price > salePrice)
                    {
                        ltrRecently.Text += "Starting Price: <span>" + salePrice.ToString("C") + "</span>";
                        ltrRecently.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsRecently.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + salePrice.ToString() + "\" name=\"hdnmayprice-" + dsRecently.Tables[0].Rows[i]["ProductId"].ToString() + "\">";
                    }
                    else
                    {
                        ltrRecently.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsRecently.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + price.ToString() + "\" name=\"hdnmayprice-" + dsRecently.Tables[0].Rows[i]["ProductId"].ToString() + "\">";

                        ltrRecently.Text += "Starting Price: <span>" + price.ToString("C") + "</span>";

                    }
                    ltrRecently.Text += "</p>";
                    //ltrRecently.Text += "<div class='or-sellprice'>";
                    //if (salePrice > decimal.Zero && price > salePrice)
                    //{
                    //    ltrRecently.Text += "Sale Price: <span>" + salePrice.ToString("C") + "</span>";
                    //    ltrRecently.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + salePrice.ToString() + "\" name=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\">";
                    //}
                    //else
                    //{
                    //    ltrRecently.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + price.ToString() + "\" name=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\">";

                    //    ltrRecently.Text += "<span>&nbsp;</span>";

                    //}
                    //ltrRecently.Text += "</div>";

                    //ltrRecently.Text += "<div class=\"or-quantity\">";
                    //ltrRecently.Text += "<label>";
                    //ltrRecently.Text += "Quantity:</label>";
                    //ltrRecently.Text += "<input type=\"text\" maxlength=\"3\" onkeypress=\"return onKeyPressBlockNumbers1(event);\" id=\"txtmayQty-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\" class=\"quantitybox\" value=\"0\" name=\"txtmayQty-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\">";
                    //ltrRecently.Text += "</div>";


                    // ltrRecently.Text += "<span><a title=\"VIEW MORE\" href=\"/" + dsYouMay.Tables[0].Rows[i]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[i]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[i]["productId"].ToString() + ".aspx\"><img alt=\"VIEW MORE\" src=\"/images/view_more.jpg\"></a></span>";


                    ltrRecently.Text += "</div>";
                    ltrRecently.Text += "</div>";
                    ltrRecently.Text += "</li>";
                }

            }
            else
            {
                divrecently.Visible = false;
            }

        }

        /// <summary>
        /// Get You may also Like product
        /// </summary>
        /// <param name="ProductID">int Product ID</param>
        private void GetYoumayalsoLikeProduct(int ProductID)
        {
            DataSet dsYouMay = new DataSet();
            ltrYoumay.Text = "";
            ProductComponent objYoumay = new ProductComponent();
            dsYouMay = objYoumay.GetRelatedProduct(ProductID, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            Int32 iCount = 0;
            if (dsYouMay != null && dsYouMay.Tables.Count > 0 && dsYouMay.Tables[0].Rows.Count > 0)
            {
                iCount = dsYouMay.Tables[0].Rows.Count;
                iCount = 4 - iCount;
            }
            else
            {
                iCount = 4;
            }
            if (iCount > 0)
            {
                DataSet dsYouMay1 = new DataSet();
                dsYouMay1 = objYoumay.GetYoumayalsoLikeProduct(ProductID, iCount, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                foreach (DataRow dr in dsYouMay1.Tables[0].Rows)
                {
                    object[] dr1 = dr.ItemArray;
                    dsYouMay.Tables[0].Rows.Add(dr1);
                }
            }

            if (dsYouMay != null && dsYouMay.Tables.Count > 0 && dsYouMay.Tables[0].Rows.Count > 0)
            {
                // ltrYoumay.Text += "<div class=\"or-main\" id=\"divyouMayalso\">";
                for (Int32 i = 0; i < dsYouMay.Tables[0].Rows.Count; i++)
                {
                    //if (i != dsYouMay.Tables[0].Rows.Count - 1)
                    //{

                    //    ltrYoumay.Text += "<div class='fp-box'>";
                    //    ltrYoumay.Text += "<div class='fp-display' style='margin-top:10px;'>";

                    //}
                    //else
                    //{
                    ltrYoumay.Text += " <li>";
                    ltrYoumay.Text += " <div class=\"bs-box\">";
                    ltrYoumay.Text += "<div class=\"bs-display\">";
                    //ltrYoumay.Text += "<div class='fp-box'>";
                    //ltrYoumay.Text += "<div class='fp-display' style='margin-top:10px;'>";
                    //}

                    decimal price = 0;
                    decimal salePrice = 0;
                    if (!string.IsNullOrEmpty(dsYouMay.Tables[0].Rows[i]["price"].ToString()))
                    {
                        price = Convert.ToDecimal(dsYouMay.Tables[0].Rows[i]["price"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsYouMay.Tables[0].Rows[i]["saleprice"].ToString()))
                    {
                        salePrice = Convert.ToDecimal(dsYouMay.Tables[0].Rows[i]["saleprice"].ToString());
                    }
                    ltrYoumay.Text += "<div class=\"bs-box-div\" style='position: relative;'> <div class='img-center'> ";
                    string StrTagName = Convert.ToString(dsYouMay.Tables[0].Rows[i]["TagName"].ToString());

                    // ltrYoumay.Text += "<a   title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsYouMay.Tables[0].Rows[i]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[i]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[i]["productId"].ToString() + ".aspx\">";
                    ltrYoumay.Text += "<a   title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsYouMay.Tables[0].Rows[i]["ProductURL"].ToString() + "\">";

                    Random rd = new Random();
                    if (!string.IsNullOrEmpty(dsYouMay.Tables[0].Rows[i]["imagename"].ToString()))
                    {
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsYouMay.Tables[0].Rows[i]["imagename"].ToString())))
                        {
                            ltrYoumay.Text += "<img id=\"imgYoumayProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsYouMay.Tables[0].Rows[i]["imagename"].ToString() + "?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_imgYoumay" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";

                        }
                        else
                        {
                            ltrYoumay.Text += "<img id=\"imgYoumayProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_imgYoumay" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";
                        }
                    }
                    else
                    {
                        ltrYoumay.Text += "<img id=\"imgYoumayProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_imgYoumay" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";
                    }

                    if (ViewState["strYoumayimg"] != null)
                    {
                        ViewState["strYoumayimg"] = ViewState["strYoumayimg"].ToString() + "$('#loader_imgYoumay" + i.ToString() + "').show();$('#imgYoumayProduct" + i.ToString() + "').hide();$('#imgYoumayProduct" + i.ToString() + "').load(function () {$('#loader_imgYoumay" + i.ToString() + "').hide();$('#imgYoumayProduct" + i.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                        //ViewState["strFeaturedimg"] = ViewState["strFeaturedimg"].ToString() + "$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();});";
                    }
                    else
                    {
                        ViewState["strYoumayimg"] = "$('#loader_imgYoumay" + i.ToString() + "').show();$('#imgYoumayProduct" + i.ToString() + "').hide();$('#imgYoumayProduct" + i.ToString() + "').load(function () {$('#loader_imgYoumay" + i.ToString() + "').hide();$('#imgYoumayProduct" + i.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                        // ViewState["strFeaturedimg"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();});";
                    }
                    if (ViewState["strYoumayimgpostback"] != null)
                    {
                        ViewState["strYoumayimgpostback"] = ViewState["strYoumayimgpostback"].ToString() + "$('#loader_imgYoumay" + i.ToString() + "').hide();";
                    }
                    else
                    {
                        ViewState["strYoumayimgpostback"] = "$('#loader_imgYoumay" + i.ToString() + "').hide();";
                    }

                    ltrYoumay.Text += " <img src='/images/view-detail.png' alt='View Detail' class='preview' width='150' height='30' title='View Detail'></a>";
                    // Bind TagName

                    if (!string.IsNullOrEmpty(StrTagName))
                    {
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(AppLogic.AppConfigs("imagepathproduct") + "icon/" + dsYouMay.Tables[0].Rows[i]["imagename"].ToString())))
                        {
                            ltrYoumay.Text += "<img title='" + StrTagName.ToString().Trim() + "' src=\"/images/" + StrTagName.ToString().Trim() + ".png\" alt=\"" + StrTagName.ToString().Trim() + "\" class='" + StrTagName.ToString().ToLower() + "'  />";
                        }
                    }
                    ltrYoumay.Text += "</div></div>";


                    // ltrYoumay.Text += "<h2 class='fp-box-h2' style='height:37px;'><a title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsYouMay.Tables[0].Rows[i]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[i]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[i]["productId"].ToString() + ".aspx\">" + SetName(dsYouMay.Tables[0].Rows[i]["Sortname"].ToString()) + "</a></h2>";
                    ltrYoumay.Text += "<h2 class='bs-box-h2' style='height:30px;'><a title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsYouMay.Tables[0].Rows[i]["ProductURL"].ToString() + "\">" + SetName(dsYouMay.Tables[0].Rows[i]["Sortname"].ToString()) + "</a></h2>";
                    ltrYoumay.Text += "<p class='bs-box-p'>";
                    //if (price > decimal.Zero)
                    //{
                    //    if (salePrice > decimal.Zero && price > salePrice)
                    //    {
                    //        ltrYoumay.Text += "Regular Price: " + price.ToString("C") + "";
                    //    }
                    //    else
                    //    {
                    //        ltrYoumay.Text += "Price: " + price.ToString("C") + "";
                    //    }
                    //}
                    //else
                    //{
                    //    ltrYoumay.Text += "&nbsp;";
                    //}

                    if (salePrice > decimal.Zero && price > salePrice)
                    {
                        ltrYoumay.Text += "Starting Price: <span>" + salePrice.ToString("C") + "</span>";
                        ltrYoumay.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + salePrice.ToString() + "\" name=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\">";
                    }
                    else
                    {
                        ltrYoumay.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + price.ToString() + "\" name=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\">";

                        ltrYoumay.Text += "Starting Price: <span>" + price.ToString("C") + "</span>";

                    }
                    ltrYoumay.Text += "</p>";
                    //ltrYoumay.Text += "<div class='or-sellprice'>";
                    //if (salePrice > decimal.Zero && price > salePrice)
                    //{
                    //    ltrYoumay.Text += "Sale Price: <span>" + salePrice.ToString("C") + "</span>";
                    //    ltrYoumay.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + salePrice.ToString() + "\" name=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\">";
                    //}
                    //else
                    //{
                    //    ltrYoumay.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + price.ToString() + "\" name=\"hdnmayprice-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\">";

                    //    ltrYoumay.Text += "<span>&nbsp;</span>";

                    //}
                    //ltrYoumay.Text += "</div>";

                    //ltrYoumay.Text += "<div class=\"or-quantity\">";
                    //ltrYoumay.Text += "<label>";
                    //ltrYoumay.Text += "Quantity:</label>";
                    //ltrYoumay.Text += "<input type=\"text\" maxlength=\"3\" onkeypress=\"return onKeyPressBlockNumbers1(event);\" id=\"txtmayQty-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\" class=\"quantitybox\" value=\"0\" name=\"txtmayQty-" + dsYouMay.Tables[0].Rows[i]["ProductId"].ToString() + "\">";
                    //ltrYoumay.Text += "</div>";


                    // ltrYoumay.Text += "<span><a title=\"VIEW MORE\" href=\"/" + dsYouMay.Tables[0].Rows[i]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[i]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[i]["productId"].ToString() + ".aspx\"><img alt=\"VIEW MORE\" src=\"/images/view_more.jpg\"></a></span>";


                    ltrYoumay.Text += "</div>";
                    ltrYoumay.Text += "</div>";
                    ltrYoumay.Text += "</li>";

                }
                //ltrYoumay.Text += "<div class=\"add-to-cart1\">";
                ////ltrYoumay.Text += "<a title=\"Add To Cart\" href=\"javascript:void(0);\" onclick=\"javascript: document.getElementById('ContentPlaceHolder1_btnMultiPleAddtocart').click();\">";
                //ltrYoumay.Text += "<a id=\"productmultiAll\" title=\"Add To Cart\" href=\"javascript:void(0);\" onclick=\"javascript:return InsertProductMultiple('productmultiAll');\">";
                //ltrYoumay.Text += "<img title=\"Add To Cart\" alt=\"Add To Cart\" src=\"/images/add-to-cart.gif\"></a>";
                //ltrYoumay.Text += "</div>";
                //ltrYoumay.Text += "</div>";


            }
            else
            {
                divYoumay.Visible = false;
            }

        }

        private void GetOptinalAccesories(String SKUs)
        {
            DataSet dsOA = new DataSet();
            dsOA = ProductComponent.GetOptinalProductDetailByID(SKUs, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsOA != null && dsOA.Tables.Count > 0 && dsOA.Tables[0].Rows.Count > 0)
            {
                for (Int32 i = 0; i < dsOA.Tables[0].Rows.Count; i++)
                {
                    //if (i != dsOA.Tables[0].Rows.Count - 1)
                    //{

                    //    ltrOptionalAccesories.Text += "<div class='fp-box'>";
                    //    ltrOptionalAccesories.Text += "<div class='fp-display' style='margin-top:10px;'>";

                    //}
                    //else
                    //{
                    ltrOptionalAccesories.Text += " <li>";
                    ltrOptionalAccesories.Text += " <div class=\"bs-box\">";
                    ltrOptionalAccesories.Text += "<div class=\"bs-display\">";
                    //ltrOptionalAccesories.Text += "<div class='fp-box'>";
                    //ltrOptionalAccesories.Text += "<div class='fp-display' style='margin-top:10px;'>";
                    //}

                    decimal price = 0;
                    decimal salePrice = 0;
                    if (!string.IsNullOrEmpty(dsOA.Tables[0].Rows[i]["price"].ToString()))
                    {
                        price = Convert.ToDecimal(dsOA.Tables[0].Rows[i]["price"].ToString());
                    }
                    if (!string.IsNullOrEmpty(dsOA.Tables[0].Rows[i]["saleprice"].ToString()))
                    {
                        salePrice = Convert.ToDecimal(dsOA.Tables[0].Rows[i]["saleprice"].ToString());
                    }
                    ltrOptionalAccesories.Text += "<div class=\"bs-box-div\" style='position: relative;'> <div class='img-center'> ";
                    string StrTagName = Convert.ToString(dsOA.Tables[0].Rows[i]["TagName"].ToString());


                    ltrOptionalAccesories.Text += "<a   title=\"" + SetAttribute(dsOA.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsOA.Tables[0].Rows[i]["ProductURL"].ToString() + "\">";

                    Random rd = new Random();
                    if (!string.IsNullOrEmpty(dsOA.Tables[0].Rows[i]["imagename"].ToString()))
                    {
                        if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsOA.Tables[0].Rows[i]["imagename"].ToString())))
                        {
                            ltrOptionalAccesories.Text += "<img id=\"imgaccesoriesProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsOA.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsOA.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsOA.Tables[0].Rows[i]["imagename"].ToString() + "?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_imgAccesories" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";

                        }
                        else
                        {
                            ltrOptionalAccesories.Text += "<img id=\"imgaccesoriesProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsOA.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsOA.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_imgAccesories" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";
                        }
                    }
                    else
                    {
                        ltrOptionalAccesories.Text += "<img id=\"imgaccesoriesProduct" + i.ToString() + "\" style=\" width:230px; height:309px;\" title=\"" + SetAttribute(dsOA.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" alt=\"" + SetAttribute(dsOA.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" src=\"" +AppLogic.AppConfigs("Live_Contant_Server")+ AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(1000).ToString() + "\"><img src=\"/images/imgloader.gif\" alt=\"\" id=\"loader_imgAccesories" + i.ToString() + "\"style=\"text-align: center; vertical-align: middle;\" />";
                    }

                    if (ViewState["strAccesoriesimg"] != null)
                    {
                        ViewState["strAccesoriesimg"] = ViewState["strAccesoriesimg"].ToString() + "$('#loader_imgAccesories" + i.ToString() + "').show();$('#imgaccesoriesProduct" + i.ToString() + "').hide();$('#imgaccesoriesProduct" + i.ToString() + "').load(function () {$('#loader_imgAccesories" + i.ToString() + "').hide();$('#imgaccesoriesProduct" + i.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                        //ViewState["strFeaturedimg"] = ViewState["strFeaturedimg"].ToString() + "$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();});";
                    }
                    else
                    {
                        ViewState["strAccesoriesimg"] = "$('#loader_imgAccesories" + i.ToString() + "').show();$('#imgaccesoriesProduct" + i.ToString() + "').hide();$('#imgaccesoriesProduct" + i.ToString() + "').load(function () {$('#loader_imgAccesories" + i.ToString() + "').hide();$('#imgaccesoriesProduct" + i.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                        // ViewState["strFeaturedimg"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();});";
                    }
                    if (ViewState["strAccesoriesimgpostback"] != null)
                    {
                        ViewState["strAccesoriesimgpostback"] = ViewState["strAccesoriesimgpostback"].ToString() + "$('#loader_imgAccesories" + i.ToString() + "').hide();";
                    }
                    else
                    {
                        ViewState["strAccesoriesimgpostback"] = "$('#loader_imgAccesories" + i.ToString() + "').hide();";
                    }

                    ltrOptionalAccesories.Text += " <img src='/images/view-detail.png' alt='View Detail' class='preview' width='150' height='30' title='View Detail'></a>";
                    // Bind TagName

                    //if (!string.IsNullOrEmpty(StrTagName))
                    //{
                    //    if (File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsYouMay.Tables[0].Rows[i]["imagename"].ToString())))
                    //    {
                    //        ltrOptionalAccesories.Text += "<img title='" + StrTagName.ToString().Trim() + "' src=\"/images/" + StrTagName.ToString().Trim() + ".png\" alt=\"" + StrTagName.ToString().Trim() + "\" class='" + StrTagName.ToString().ToLower() + "' style='position: absolute; top: 110px; left: 4px;' />";
                    //    }
                    //}
                    ltrOptionalAccesories.Text += "</div></div>";


                    // ltrOptionalAccesories.Text += "<h2 class='fp-box-h2' style='height:37px;'><a title=\"" + SetAttribute(dsYouMay.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsYouMay.Tables[0].Rows[i]["MainCategory"].ToString() + "/" + dsYouMay.Tables[0].Rows[i]["sename"].ToString() + "-" + dsYouMay.Tables[0].Rows[i]["productId"].ToString() + ".aspx\">" + SetName(dsYouMay.Tables[0].Rows[i]["Sortname"].ToString()) + "</a></h2>";
                    ltrOptionalAccesories.Text += "<h2 class='bs-box-h2' style='height:30px;'><a title=\"" + SetAttribute(dsOA.Tables[0].Rows[i]["Tooltip"].ToString()) + "\" href=\"/" + dsOA.Tables[0].Rows[i]["ProductURL"].ToString() + "\">" + SetName(dsOA.Tables[0].Rows[i]["Name"].ToString()) + "</a></h2>";
                    ltrOptionalAccesories.Text += "<p class='bs-box-p'>";
                    //if (price > decimal.Zero)
                    //{
                    //    if (salePrice > decimal.Zero && price > salePrice)
                    //    {
                    //        ltrOptionalAccesories.Text += "Regular Price: " + price.ToString("C") + "";
                    //    }
                    //    else
                    //    {
                    //        ltrOptionalAccesories.Text += "Price: " + price.ToString("C") + "";
                    //    }
                    //}
                    //else
                    //{
                    //    ltrOptionalAccesories.Text += "&nbsp;";
                    //}

                    if (salePrice > decimal.Zero && price > salePrice)
                    {
                        ltrOptionalAccesories.Text += "Starting Price: <span>" + salePrice.ToString("C") + "</span>";
                        ltrOptionalAccesories.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsOA.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + salePrice.ToString() + "\" name=\"hdnmayprice-" + dsOA.Tables[0].Rows[i]["ProductId"].ToString() + "\">";
                    }
                    else
                    {
                        ltrOptionalAccesories.Text += "<input type=\"hidden\" id=\"hdnmayprice-" + dsOA.Tables[0].Rows[i]["ProductId"].ToString() + "\"  value=\"" + price.ToString() + "\" name=\"hdnmayprice-" + dsOA.Tables[0].Rows[i]["ProductId"].ToString() + "\">";

                        ltrOptionalAccesories.Text += "Starting Price: <span>" + price.ToString("C") + "</span>";

                    }
                    ltrOptionalAccesories.Text += "</p>";

                    ltrOptionalAccesories.Text += "</div>";
                    ltrOptionalAccesories.Text += "</div>";
                    ltrOptionalAccesories.Text += "</li>";

                }

            }
            else
            {
                divOptioanlAccesories.Visible = false;
            }
        }

        /// <summary>
        /// Add Customer for Cart item Temporary
        /// </summary>
        private void AddCustomer()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Customer objCust = new tb_Customer();
            Int32 CustID = -1;
            CustID = objCustomer.InsertCustomer(objCust, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            Session["CustID"] = CustID.ToString();
            System.Web.HttpCookie custCookie = new System.Web.HttpCookie("ecommcustomer", CustID.ToString());
            custCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(custCookie);

        }

        /// <summary>
        /// Bind Cart Details By Customer ID
        /// </summary>
        private void BindCartDetails()
        {
            DataSet dsCart = new DataSet();
            if (Session["CustID"] != null && Session["CustID"].ToString() != "")
            {
                dsCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
                if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
                {
                    int items = Convert.ToInt32(dsCart.Tables[0].Rows[0]["TotalItems"].ToString());
                    lttotalitems.Text = items > 1 ? items.ToString("D2") + " Items" : items.ToString("D2") + " Item";
                    decimal total = Convert.ToDecimal(dsCart.Tables[0].Rows[0]["SubTotal"].ToString());
                    //if (Session["Discount"] != null)
                    //{
                    //    total += Convert.ToDecimal(Session["Discount"].ToString());
                    //}
                    //if (Session["QtyDiscount"] != null)
                    //{
                    //    total += Convert.ToDecimal(Session["QtyDiscount"].ToString());
                    //}
                    ltsubtotal.Text = Convert.ToDecimal(total).ToString("C");
                    Session["NoOfCartItems"] = dsCart.Tables[0].Rows[0]["TotalItems"].ToString();
                    btnViewDetail.Visible = true;
                    btnCheckout.Visible = true;
                }
                else
                {
                    Session["NoOfCartItems"] = null;
                    lttotalitems.Text = "0 Item";
                    btnViewDetail.Visible = false;
                    btnCheckout.Visible = false;
                    ltsubtotal.Text = Convert.ToDecimal(0).ToString("C");
                }
            }
            else
            {
                btnViewDetail.Visible = false;
                btnCheckout.Visible = false;
                lttotalitems.Text = "0 Item";
                ltsubtotal.Text = Convert.ToDecimal(0).ToString("C");
            }
        }

        /// <summary>
        /// Check Customer is restricted or not.
        /// </summary>
        /// <returns>Returns True or False</returns>
        private bool CheckCustomerIsRestricted()
        {
            bool IsRestrictedCust = false;
            IsRestrictedCust = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT RestrictedIPID FROM tb_RestrictedIP WHERE IPAddress='" + Request.UserHostAddress.ToString() + "'"));
            return IsRestrictedCust;
        }

        #region Item Add To Cart

        /// <summary>
        /// Item Add or Update Into Cart Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddToCart_Click(object sender, ImageClickEventArgs e)
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
                string VariantQty = "";

                if (hdnEngravName.Value.ToString().Trim() == "")
                {
                    if (Request.QueryString["PID"] != null && Session["CustID"] != null)
                    {
                        ShoppingCartComponent objShopping = new ShoppingCartComponent();
                        decimal price = Convert.ToDecimal(hdnprice.Value);
                        Int32 Qty = 0;
                        bool flQty = Int32.TryParse(txtQty.Text.ToString(), out Qty);
                        if (flQty == false)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg1234322", "jAlert('Please enter valid inventory.', 'Message','ContentPlaceHolder1_txtQty');", true);
                            return;

                        }
                        else if (Qty <= 0)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg1234322", "jAlert('Please enter valid inventory.', 'Message','ContentPlaceHolder1_txtQty');", true);
                            return;
                        }
                        if (gvOptionalAcc.Rows.Count > 0)
                        {
                            for (int c = 0; c < gvOptionalAcc.Rows.Count; c++)
                            {
                                CheckBox ckhOASelect = (CheckBox)gvOptionalAcc.Rows[c].FindControl("ckhOASelect");
                                HiddenField hdnOAProductID = (HiddenField)gvOptionalAcc.Rows[c].FindControl("hdnOAProductID");
                                HiddenField hdnOASKU = (HiddenField)gvOptionalAcc.Rows[c].FindControl("hdnOASKU");
                                TextBox txtOAQty = (TextBox)gvOptionalAcc.Rows[c].FindControl("txtOAQty");
                                if (ckhOASelect.Checked)
                                {
                                    if (txtOAQty.Text.ToString().Trim() != "" && Convert.ToInt32(txtOAQty.Text.ToString()) > 0)
                                    {
                                        if (!CheckInventory(Convert.ToInt32(hdnOAProductID.Value.ToString()), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(txtOAQty.Text.ToString().Trim())))
                                        {
                                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg1234322", "jAlert('Not Sufficient Inventory.', 'Message','" + txtOAQty.ClientID + "');", true);
                                            return;
                                        }
                                    }
                                }
                            }
                        }

                        string[] strVariant = Request.Form.AllKeys;

                        foreach (string strKey in strVariant)
                        {

                            if (strKey.ToLower().IndexOf("selectvariant-") > -1)
                            {
                                if (Request.Form[strKey].ToString() == "0")
                                {
                                    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "variantAlert", "jAlert('Please Select " + Request.Form[strKey.Replace("Selectvariant", "hdnvariantname")].ToString() + " !!!', 'Message', '" + strKey + "');", true);
                                    return;
                                }
                                else
                                {
                                    VariantNameId = VariantNameId + Request.Form[strKey.Replace("Selectvariant", "hdnvariantname")].ToString().Replace(",", " ") + ",";
                                    VariantValueId = VariantValueId + Request.Form[strKey].ToString().Replace(",", " ") + ",";
                                }
                            }

                        }

                        string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(Request.QueryString["PID"]), Qty, price, "", "", VariantNameId, VariantValueId, 0);
                        int cntQty = 0;

                        if (strResult.ToLower() == "success")
                        {
                            cntQty = Qty;
                        }
                        #region code for Insert Optional Product in Add To Cart

                        if (gvOptionalAcc.Rows.Count > 0)
                        {
                            for (int g = 0; g < gvOptionalAcc.Rows.Count; g++)
                            {
                                CheckBox ckhOASelect = (CheckBox)gvOptionalAcc.Rows[g].FindControl("ckhOASelect");
                                HiddenField hdnOARegularPrice = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOARegularPrice");
                                HiddenField hdnOASalePrice = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOASalePrice");
                                HiddenField hdnOAProductID = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOAProductID");
                                HiddenField hdnOASKU = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOASKU");
                                TextBox txtOAQty = (TextBox)gvOptionalAcc.Rows[g].FindControl("txtOAQty");
                                if (ckhOASelect.Checked)
                                {
                                    if (txtOAQty.Text.ToString() == "0")
                                    {
                                        continue;
                                    }
                                    Decimal FinalPrice = 0;
                                    if (Convert.ToDecimal(hdnOASalePrice.Value) != 0)
                                    {
                                        FinalPrice = Convert.ToDecimal(hdnOASalePrice.Value);
                                    }
                                    else if (Convert.ToDecimal(hdnOARegularPrice.Value) != 0)
                                    {
                                        FinalPrice = Convert.ToDecimal(hdnOARegularPrice.Value);
                                    }

                                    string strOAResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(hdnOAProductID.Value), Convert.ToInt32(txtOAQty.Text), FinalPrice, "", "", "", "", 0);

                                    if (strOAResult.ToLower() == "success")
                                    {
                                        cntQty += Convert.ToInt32(txtOAQty.Text);
                                    }
                                }
                            }
                        }
                        #endregion

                        //if (strResult.ToLower() == "success")
                        if (cntQty > 0)
                        {
                            if (Session["NoOfCartItems"] == null)
                            {
                                Session["NoOfCartItems"] = cntQty.ToString();
                            }
                            Response.Redirect("/addtocart.aspx");
                        }
                        else
                        {
                            if (ViewState["HotDealProduct"] != null && Convert.ToInt32(ViewState["HotDealProduct"]) == Convert.ToInt32(Request.QueryString["PID"].ToString()))
                            {
                                divDeal.Visible = true;
                                divDeal1.Visible = true;
                                btnWishList.Visible = false;
                                string date = "";
                                if (Request.Browser.ToString().ToLower().IndexOf("firefox") > -1)
                                {
                                    date = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date.AddDays(1))) + " 24:00:00";
                                }
                                else
                                {
                                    date = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date.AddDays(1))) + " 12:00:00 AM";
                                }
                                string strAll1 = @"var date1 = new Date(); //small
                        var date2 = new Date('" + date + @"');
                        var sec = date2.getTime() - date1.getTime();
                        var second = 1000, minute = 60 * second, hour = 60 * minute, day = 24 * hour;
                        var days = Math.floor(sec / day);
                        sec -= days * day;
                        var hours = Math.floor(sec / hour);
                        sec -= hours * hour;
                        var minutes = Math.floor(sec / minute);
                        sec -= minutes * minute;
                        var seconds = Math.floor(sec / second);";
                                string strAll = "function CheckTimer(){" + strAll1 + " document.getElementById('ContentPlaceHolder1_lblTime').innerHTML=(hours < 10 ? '0' : '' ) +hours +' Hrs : ' + (minutes < 10 ? '0' : '' ) + minutes  + ' Mins : ' +  (seconds < 10 ? '0' : '' ) + seconds + ' Sec';}function Settime(){setInterval('CheckTimer();', 1000);} Settime();";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message','ContentPlaceHolder1_txtQty');", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message','ContentPlaceHolder1_txtQty');", true);
                            }

                        }
                    }
                }
                else
                {
                    bool check = false;
                    if (hdnQuantity.Value.ToString().Trim() != "" && Convert.ToInt32(hdnQuantity.Value.ToString()) > 0)
                    {
                        if (CheckInventory(Convert.ToInt32(Request.QueryString["PID"].ToString()), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(hdnQuantity.Value.ToString())))
                        {
                            string[] strname = Regex.Split(hdnEngravName.Value.ToString().Trim(), "divmain=");
                            string[] strvalue = Regex.Split(hdnEngravvalue.Value.ToString().Trim(), "divmain=");
                            string[] strQty = Regex.Split(hdnEngravQty.Value.ToString().Trim(), "divmain=");

                            for (int m = 0; m < strname.Length; m++)
                            {
                                if (strname[m].ToString().Trim() != "")
                                {
                                    VariantNameId = strname[m].Trim().Replace(System.Environment.NewLine, "").Replace(@"\r\n", "");
                                    VariantValueId = strvalue[m].Trim().Replace(System.Environment.NewLine, "").Replace(@"\r\n", "");
                                    VariantQty = strQty[m].Trim().Replace(System.Environment.NewLine, "").Replace(@"\r\n", "");

                                    string[] strprice = Regex.Split(VariantValueId.ToString().Trim(), ",");
                                    decimal vprice = 0;
                                    for (int n = 0; n < strprice.Length; n++)
                                    {
                                        if (strprice[n].ToString().Trim() != "" && strprice[n].ToString().IndexOf("$") > -1)
                                        {
                                            string str = strprice[n].Substring(strprice[n].IndexOf("$") + 1, strprice[n].Length - strprice[n].IndexOf("$") - 1);
                                            str = str.Replace("$", "").Replace("(", "").Replace(")", "").Replace("+", "").Replace("-", "");
                                            vprice += Convert.ToDecimal(str);
                                        }
                                    }
                                    if (Request.QueryString["PID"] != null && Session["CustID"] != null)
                                    {
                                        ShoppingCartComponent objShopping = new ShoppingCartComponent();
                                        decimal price = Convert.ToDecimal(hdnprice.Value) + vprice;

                                        string[] strQtyVal = Regex.Split(VariantQty.ToString().Trim(), ",");
                                        int Qty = Convert.ToInt32(strQtyVal[0]);

                                        string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(Request.QueryString["PID"]), Qty, price, "", "", VariantNameId, VariantValueId, 0);
                                        if (strResult.ToLower() == "success")
                                        {
                                            check = true;
                                        }
                                        else
                                        {
                                            if (ViewState["HotDealProduct"] != null && Convert.ToInt32(ViewState["HotDealProduct"]) == Convert.ToInt32(Request.QueryString["PID"].ToString()))
                                            {
                                                divDeal.Visible = true;
                                                divDeal1.Visible = true;
                                                btnWishList.Visible = false;
                                                string date = "";
                                                if (Request.Browser.ToString().ToLower().IndexOf("firefox") > -1)
                                                {
                                                    date = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date.AddDays(1))) + " 24:00:00";
                                                }
                                                else
                                                {
                                                    date = string.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.Date.AddDays(1))) + " 12:00:00 AM";
                                                }
                                                string strAll1 = @"var date1 = new Date(); //small
                        var date2 = new Date('" + date + @"');
                        var sec = date2.getTime() - date1.getTime();
                        var second = 1000, minute = 60 * second, hour = 60 * minute, day = 24 * hour;
                        var days = Math.floor(sec / day);
                        sec -= days * day;
                        var hours = Math.floor(sec / hour);
                        sec -= hours * hour;
                        var minutes = Math.floor(sec / minute);
                        sec -= minutes * minute;
                        var seconds = Math.floor(sec / second);";
                                                string strAll = "function CheckTimer(){" + strAll1 + " document.getElementById('ContentPlaceHolder1_lblTime').innerHTML=(hours < 10 ? '0' : '' ) +hours +' Hrs : ' + (minutes < 10 ? '0' : '' ) + minutes  + ' Mins : ' +  (seconds < 10 ? '0' : '' ) + seconds + ' Sec';}function Settime(){setInterval('CheckTimer();', 1000);} Settime();";
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", strAll + " jAlert('" + strResult.ToString() + "', 'Message','ContentPlaceHolder1_txtQty');", true);
                                            }
                                            else
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message','ContentPlaceHolder1_txtQty');", true);
                                            }

                                        }
                                    }

                                }
                            }
                            if (Session["NoOfCartItems"] == null)
                            {
                                Session["NoOfCartItems"] = "1";
                            }
                            if (check)
                            {
                                Response.Redirect("/addtocart.aspx");
                            }
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg12", "jAlert('Not Sufficient Inventory.', 'Message','ContentPlaceHolder1_txtQty');document.getElementById('prepage').style.display = 'none';", true);
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg123", "jAlert('Customer is restricted.', 'Message','');document.getElementById('prepage').style.display = 'none';", true);
            }
        }

        /// <summary>
        ///  Multiple Add to Cart Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnMultiPleAddtocart_Click(object sender, ImageClickEventArgs e)
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
                ShoppingCartComponent objShopping = new ShoppingCartComponent();


                bool check = false;
                bool checkInventory = false;

                string strInventory = "";
                string Invid = "";
                foreach (string strky in strkey)
                {
                    if (strky.ToLower().IndexOf("txtmayqty-") > -1)
                    {
                        if (Invid == "")
                        {
                            Invid = strky;
                        }
                    }
                    if (strky.ToLower().IndexOf("txtmayqty-") > -1 && Convert.ToInt32(Request.Form[strky].ToString()) > 0)
                    {

                        Int32 Qty = 0;
                        decimal price = Convert.ToDecimal(Request.Form[strky.Replace("txtmayQty", "hdnmayprice")].ToString());
                        bool flQty = Int32.TryParse(Request.Form[strky].ToString(), out Qty);
                        if (!CheckInventory(Convert.ToInt32(strky.ToString().ToLower().Replace("txtmayqty-", "")), Convert.ToInt32(Session["CustID"]), Convert.ToInt32(Request.Form[strky].ToString())))
                        {

                            if (checkInventory == false)
                            {
                                strInventory = "jAlert('Not Sufficient Inventory.', 'Message','" + strky + "');";
                            }
                            checkInventory = true;
                        }
                        else
                        {
                            string strResult = objShopping.AddItemIntoCart(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(strky.ToString().ToLower().Replace("txtmayqty-", "")), Qty, price, "", "", "", "", 0);
                            if (strResult.ToLower() == "success")
                            {
                                check = true;
                            }
                        }
                    }
                }
                if (check)
                {
                    Response.Redirect("/addtocart.aspx");
                }
                else
                {
                    if (checkInventory)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg123", "" + strInventory + " document.getElementById('prepage').style.display = 'none';", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg123", "jAlert('Please enter valid inventory.', 'Message','" + Invid + "'); document.getElementById('prepage').style.display = 'none';", true);
                    }
                }

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg123", "jAlert('Customer is restricted.', 'Message','');document.getElementById('prepage').style.display = 'none';", true);
            }
        }
        #endregion

        /// <summary>
        /// Checks the Inventory
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="CustomerId">int CustomerId</param>
        /// <param name="Qty">int Qty</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        private bool CheckInventory(Int32 ProductID, Int32 CustomerId, Int32 Qty)
        {
            Int32 ShoppingCartQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Quantity,0) FROM tb_ShoppingCartItems " +
                                                 " WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE Customerid=" + CustomerId + ") " +
                                                 " AND ProductID=" + ProductID + ""));
            Qty = Qty + ShoppingCartQty;
            DataSet dscount = new DataSet();
            dscount = CommonComponent.GetCommonDataSet("SELECT 1 FROM tb_product WHERE ProductId=" + ProductID + " AND Inventory >= " + Qty + "");
            if (dscount != null && dscount.Tables.Count > 0 && dscount.Tables[0].Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        #region Download PDF file

        /// <summary>
        /// Download Product Pdf File
        /// </summary>
        /// <param name="filepath">string filepath</param>
        private void DownloadFile(string filepath)
        {
            try
            {

                FileInfo file = new FileInfo(Server.MapPath(filepath));
                if (file.Exists)
                {
                    Response.ClearContent();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);

                    FileStream sourceFile = new FileStream(file.FullName, FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();
                    Response.BinaryWrite(getContent);

                }
            }
            catch { }
            Response.End();
        }

        /// <summary>
        ///  Download Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDownload_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["PdfPath"] != null)
            {
                string Filepath = ViewState["PdfPath"].ToString();
                DownloadFile(Filepath);
            }
        }
        #endregion

        /// <summary>
        /// Add product into WishList with Login User
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnWishList_Click(object sender, EventArgs e)
        {
            bool IsRestricted = true;
            IsRestricted = CheckCustomerIsRestricted();
            if (!IsRestricted)
            {
                //string[] strkey = Request.Form.AllKeys;
                string VariantValueId = "";
                string VariantNameId = "";
                string StrVaraintid = "";
                //foreach (string strkeynew in strkey)
                //{
                //    if (strkeynew.ToString().ToLower().IndexOf("ddlvariant") > -1)
                //    {
                //        VariantValueId += Request.Form[strkeynew].ToString() + ",";
                //        VariantNameId += strkeynew.ToString().Substring(strkeynew.ToString().LastIndexOf("_") + 1, strkeynew.ToString().Length - strkeynew.ToString().LastIndexOf("_") - 1) + ",";
                //    }
                //}
                //if (VariantValueId != "")
                //{
                //    ViewState["VariantValueId"] = "," + VariantValueId.ToString();
                //}
                //else
                //{
                //    ViewState["VariantValueId"] = null;
                //}
                //if (VariantNameId != "")
                //{
                //    ViewState["VariantNameId"] = "," + VariantNameId.ToString();
                //}
                //else
                //{
                //    ViewState["VariantNameId"] = null;
                //}

                decimal price = Convert.ToDecimal(hdnprice.Value);


                string[] strVariant = Request.Form.AllKeys;
                string strScript = "";
                foreach (string strKey in strVariant)
                {

                    if (strKey.ToLower().IndexOf("selectvariant-") > -1)
                    {
                        if (Request.Form[strKey].ToString() == "0")
                        {

                            if (strScript == "")
                            {
                                strScript = "jAlert('Please Select " + Request.Form[strKey.Replace("Selectvariant", "hdnvariantname")].ToString() + " !!!', 'Message', '" + strKey + "');";
                            }

                        }
                        else
                        {
                            StrVaraintid = StrVaraintid + strKey.ToString() + ",";
                            //VariantNameId = VariantNameId + Request.Form[strKey.Replace("Selectvariant", "hdnvariantname")].ToString().Replace(",", " ") + ",";
                            //VariantValueId = VariantValueId + Request.Form[strKey].ToString().Replace(",", " ") + ",";
                            VariantNameId = hdnVariantNameId.Value;
                            VariantValueId = hdnVariantValueId.Value;
                            if (VariantValueId != "")
                            {
                                ViewState["VariantValueId"] = VariantValueId.ToString();
                            }
                            else
                            {
                                ViewState["VariantValueId"] = null;
                            }
                            if (StrVaraintid != "")
                            {
                                ViewState["VariantNameId"] = StrVaraintid.ToString();
                            }
                            else
                            {
                                ViewState["VariantNameId"] = null;
                            }

                        }
                    }

                }
                if (strScript != "")
                {
                    if (ViewState["VariantNameId"] != null && ViewState["VariantNameId"].ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "dropdwnselect", "SelectVariantBypostback('" + ViewState["VariantNameId"].ToString() + "','" + ViewState["VariantValueId"].ToString() + "');", true);
                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "variantAlert", strScript, true);
                    return;
                }

                Int32 Qty = 0;
                bool flQty = Int32.TryParse(txtQty.Text.ToString(), out Qty);
                if (flQty == false)
                {
                    if (ViewState["VariantNameId"] != null && ViewState["VariantNameId"].ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "dropdwnselect", "SelectVariantBypostback('" + ViewState["VariantNameId"].ToString() + "','" + ViewState["VariantValueId"].ToString() + "');", true);

                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg1234322", "jAlert('Please enter valid inventory.', 'Message','ContentPlaceHolder1_txtQty');", true);
                    return;

                }
                else if (Qty <= 0)
                {
                    if (ViewState["VariantNameId"] != null && ViewState["VariantNameId"].ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "dropdwnselect", "SelectVariantBypostback('" + ViewState["VariantNameId"].ToString() + "','" + ViewState["VariantValueId"].ToString() + "');", true);

                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg1234322", "jAlert('Please enter valid inventory.', 'Message','ContentPlaceHolder1_txtQty');", true);
                    return;
                }

                if (hdnVariantPrice.Value != "0" && hdnVariantPrice.Value != "")
                {
                    decimal Variprice = Convert.ToDecimal(hdnVariantPrice.Value);
                    price = price + Variprice;
                }

                if (Session["CustID"] != null && Convert.ToString(Session["CustID"]) != "")
                {
                    WishListItemsComponent objWishList = new WishListItemsComponent();
                    int IsAdded = 0;
                    if (string.IsNullOrEmpty(VariantNameId.ToString()) || VariantNameId.ToString() == "0")
                        VariantNameId = "";
                    if (string.IsNullOrEmpty(VariantValueId.ToString()) || VariantValueId.ToString() == "0")
                        VariantValueId = "";
                    IsAdded = objWishList.AddWishListItems(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(Request.QueryString["PID"]), Qty, price, hdnVariantNameId.Value.ToString(), hdnVariantValueId.Value.ToString());


                    //if (gvOptionalAcc.Rows.Count > 0)
                    //{
                    //    for (int g = 0; g < gvOptionalAcc.Rows.Count; g++)
                    //    {
                    //        CheckBox ckhOASelect = (CheckBox)gvOptionalAcc.Rows[g].FindControl("ckhOASelect");
                    //        HiddenField hdnOARegularPrice = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOARegularPrice");
                    //        HiddenField hdnOASalePrice = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOASalePrice");
                    //        HiddenField hdnOAProductID = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOAProductID");
                    //        HiddenField hdnOASKU = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOASKU");
                    //        TextBox txtOAQty = (TextBox)gvOptionalAcc.Rows[g].FindControl("txtOAQty");
                    //        if (ckhOASelect.Checked)
                    //        {
                    //            if (txtOAQty.Text.ToString() == "0")
                    //            {
                    //                continue;
                    //            }
                    //            Decimal FinalPrice = 0;
                    //            if (Convert.ToDecimal(hdnOASalePrice.Value) != 0)
                    //            {
                    //                FinalPrice = Convert.ToDecimal(hdnOASalePrice.Value);
                    //            }
                    //            else if (Convert.ToDecimal(hdnOARegularPrice.Value) != 0)
                    //            {
                    //                FinalPrice = Convert.ToDecimal(hdnOARegularPrice.Value);
                    //            }

                    //            IsAdded = objWishList.AddWishListItems(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(hdnOAProductID.Value.ToString()), Convert.ToInt32(txtOAQty.Text.ToString()), FinalPrice, "", "");

                    //        }
                    //    }
                    //}

                    if (IsAdded > 0)
                    {
                        Response.Redirect("/WishList.aspx", true);
                    }
                    else
                    {
                        if (ViewState["VariantNameId"] != null && ViewState["VariantNameId"].ToString() != "")
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "dropdwnselect", "SelectVariantBypostback('" + ViewState["VariantNameId"].ToString() + "','" + ViewState["VariantValueId"].ToString() + "');", true);

                        }
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while adding product into wishlist.', 'Message');});", true);
                        return;
                    }
                }
                else
                {
                    DataTable dtProduct = new DataTable();
                    DataColumn dc;
                    dc = new DataColumn("ProductID");
                    dtProduct.Columns.Add(dc);
                    dc = new DataColumn("Quantity");
                    dtProduct.Columns.Add(dc);
                    dc = new DataColumn("Price");
                    dtProduct.Columns.Add(dc);
                    dc = new DataColumn("VariantNameId");
                    dtProduct.Columns.Add(dc);
                    dc = new DataColumn("VariantValueId");
                    dtProduct.Columns.Add(dc);
                    DataRow row = dtProduct.NewRow();
                    row["ProductID"] = Convert.ToInt32(Request.QueryString["PID"]);
                    row["Quantity"] = Qty;
                    row["Price"] = price;
                    row["VariantNameId"] = hdnVariantNameId.Value.ToString();
                    row["VariantValueId"] = hdnVariantValueId.Value.ToString();
                    dtProduct.Rows.Add(row);

                    if (gvOptionalAcc.Rows.Count > 0)
                    {
                        for (int g = 0; g < gvOptionalAcc.Rows.Count; g++)
                        {
                            CheckBox ckhOASelect = (CheckBox)gvOptionalAcc.Rows[g].FindControl("ckhOASelect");
                            HiddenField hdnOARegularPrice = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOARegularPrice");
                            HiddenField hdnOASalePrice = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOASalePrice");
                            HiddenField hdnOAProductID = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOAProductID");
                            HiddenField hdnOASKU = (HiddenField)gvOptionalAcc.Rows[g].FindControl("hdnOASKU");
                            TextBox txtOAQty = (TextBox)gvOptionalAcc.Rows[g].FindControl("txtOAQty");
                            if (ckhOASelect.Checked)
                            {
                                if (txtOAQty.Text.ToString() == "0")
                                {
                                    continue;
                                }
                                Decimal FinalPrice = 0;
                                if (Convert.ToDecimal(hdnOASalePrice.Value) != 0)
                                {
                                    FinalPrice = Convert.ToDecimal(hdnOASalePrice.Value);
                                }
                                else if (Convert.ToDecimal(hdnOARegularPrice.Value) != 0)
                                {
                                    FinalPrice = Convert.ToDecimal(hdnOARegularPrice.Value);
                                }
                                row = dtProduct.NewRow();
                                row["ProductID"] = Convert.ToInt32(hdnOAProductID.Value.ToString());
                                row["Quantity"] = Convert.ToInt32(txtOAQty.Text.ToString());
                                row["Price"] = FinalPrice;
                                row["VariantNameId"] = "";
                                row["VariantValueId"] = "";
                                dtProduct.Rows.Add(row);

                            }
                        }
                    }

                    if (Session["WishListProduct"] != null)
                    {
                        Session["WishListProduct"] = null;
                    }
                    Session["WishListProduct"] = dtProduct;
                    Response.Redirect("/Login.aspx?wishlist=2", true);
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Customer is restricted.', 'Message','');", true);
            }
        }

        /// <summary>
        /// Submit Comment & Rating
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["PID"] != null)
            {
                ProductComponent objRating = new ProductComponent();
                string strResult = "";
                if (Session["CustID"] != null)
                {
                    strResult = objRating.AddOrUpdateRating(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(Request.QueryString["PID"]), txtcomment.Text.ToString(), txtEmail.Text.ToString(), Convert.ToInt32(ddlrating.SelectedValue.ToString()), 1, txtname.Text.ToString());
                }
                else
                {
                    strResult = objRating.AddOrUpdateRating(Convert.ToInt32(0), Convert.ToInt32(Request.QueryString["PID"]), txtcomment.Text.ToString(), txtEmail.Text.ToString(), Convert.ToInt32(ddlrating.SelectedValue.ToString()), 1, txtname.Text.ToString());
                }
                if (strResult != "")
                {
                    txtcomment.Text = "";
                    txtname.Text = "";
                    txtEmail.Text = "";
                    ddlrating.SelectedIndex = 0;
                    if (divDeal1.Visible == true)
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message');clearcomment();if (document.getElementById('ContentPlaceHolder1_divDeal1') != null && document.getElementById('ContentPlaceHolder1_Dealofthedaylbl_Timer1') != null) {$find('ContentPlaceHolder1_Dealofthedaylbl_Timer1')._startTimer();}document.getElementById('prepage').style.display = 'none';", true);

                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message');clearcomment();document.getElementById('prepage').style.display = 'none';", true);

                    }
                }
            }
            else
            {
                Response.Redirect("/Login.aspx?ReturnUrl=" + Request.RawUrl.ToString());
            }
        }

        #region Go to Cart Details Page

        /// <summary>
        ///  View Details Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnViewDetail_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/addtocart.aspx");
        }

        /// <summary>
        ///  Check Out Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCheckout_Click(object sender, ImageClickEventArgs e)
        {
            if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
                if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString() != "")
                {
                    Response.Redirect("checkoutcommon.aspx");
                }
                else
                {
                    Response.Redirect("/addtocart.aspx");
                }
            }
            else
            {
                Response.Redirect("/addtocart.aspx");
            }
        }
        #endregion

        #region Remove ViewState From page


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
        /// Gets the Medium Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Medium Image Path</returns>
        public String GetMediumImage(String img)
        {
            clsvariables.LoadAllPath();
            String[] AllowedExtensions = AppLogic.AppConfigs("AllowedExtensions").Split(',');
            String imagepath = String.Empty;
            Random rd = new Random();
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Medium/" + img;
            if (img != "")
            {
                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(imagepath)))
                {
                    return AppLogic.AppConfigs("Live_Contant_Server")+imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "Medium/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "Medium/image_not_available.jpg");
        }

        /// <summary>
        /// Get Next Previous Product
        /// </summary>
        private void GetnextPreviousProduct()
        {
            try
            {
                DataSet dsNext = new DataSet();
                if ((Session["ProductIds"] == null || (Session["ProductIds"] != null && !Session["ProductIds"].ToString().Contains(Request.QueryString["PID"].ToString()))))
                {
                    dsNext = CommonComponent.GetCommonDataSet("EXEC usp_Product_GetnextpreviousProduct " + Request.QueryString["PID"].ToString() + "," + AppLogic.AppConfigs("StoreId") + "");
                    if (dsNext != null && dsNext.Tables.Count > 0 && dsNext.Tables[0].Rows.Count > 0)
                    {
                        int cntItem = dsNext.Tables[0].Rows.Count;
                        string PIDList = string.Empty;
                        for (int cntPID = 0; cntPID < cntItem; cntPID++)
                            PIDList += dsNext.Tables[0].Rows[cntPID]["ProductID"].ToString() + ",";
                        if (PIDList != "")
                            PIDList = PIDList.Substring(0, PIDList.Length - 1);
                        Session["ProductIds"] = PIDList;
                    }
                }
                if (Session["ProductIds"] != null)
                {

                    string ProductIDList = Session["ProductIds"].ToString();
                    string CurrPID = Request.QueryString["PID"].ToString();
                    string TempStr = string.Empty;
                    int KeyIndex = 0;
                    int StIndex = 0;
                    int LastIndex = 0;
                    string pPID = "";
                    string nPID = "";
                    if (ProductIDList != "" && (ProductIDList.IndexOf("," + CurrPID) != -1 || ProductIDList.IndexOf(CurrPID + ",") != -1 || ProductIDList.IndexOf("," + CurrPID + ",") != -1))
                    {
                        if (ProductIDList.IndexOf("," + CurrPID + ",") != -1)
                        {
                            KeyIndex = ProductIDList.IndexOf("," + CurrPID + ",");
                            if (ProductIDList.Substring(0, KeyIndex).Length > 8)
                                StIndex = KeyIndex - 8;
                            else
                                StIndex = 0;
                            if (ProductIDList.Substring(KeyIndex, ProductIDList.Length - KeyIndex).Length > 16)
                                LastIndex = KeyIndex + 16;
                            else
                                LastIndex = ProductIDList.Length;
                        }
                        else if (ProductIDList.IndexOf(CurrPID + ",") != -1 && ProductIDList.IndexOf("," + CurrPID) == -1)
                        {
                            KeyIndex = ProductIDList.IndexOf(CurrPID + ",");
                            StIndex = 0;
                            if (ProductIDList.Length > 16)
                                LastIndex = 16;
                            else
                                LastIndex = ProductIDList.Length;
                        }
                        else if (ProductIDList.IndexOf("," + CurrPID) != -1 && ProductIDList.IndexOf(CurrPID + ",") == -1)
                        {
                            KeyIndex = ProductIDList.IndexOf("," + CurrPID);
                            if (ProductIDList.Length > 16)
                                StIndex = KeyIndex - 8;
                            else
                                StIndex = 0;
                            LastIndex = ProductIDList.Length;
                        }
                        else if (ProductIDList.IndexOf(CurrPID + ",") == 0)
                        {
                            KeyIndex = ProductIDList.IndexOf(CurrPID + ",");
                            StIndex = 0;
                            if (ProductIDList.Length > 16)
                                LastIndex = 16;
                            else
                                LastIndex = ProductIDList.Length;
                        }


                        TempStr = ProductIDList.Substring(StIndex, LastIndex - StIndex);
                        string[] StrArrPID = TempStr.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        if (StrArrPID.Length > 0)
                        {
                            if (StrArrPID[0] == CurrPID)
                                nPID = StrArrPID[1];
                            else if (StrArrPID[StrArrPID.Length - 1] == CurrPID)
                                pPID = StrArrPID[StrArrPID.Length - 2];
                            else if (StrArrPID.Length > 2 && TempStr.Contains(CurrPID))
                                for (int i = 0; i < StrArrPID.Length; i++)
                                {
                                    try
                                    {
                                        pPID = StrArrPID[i].ToString();
                                        if (StrArrPID[i + 1].ToString() == CurrPID)
                                        {
                                            nPID = StrArrPID[i + 2].ToString();
                                            break;
                                        }
                                    }
                                    catch { }
                                }
                        }
                    }




                    string sqtParam = "";
                    if (pPID != "" && nPID == "")
                        sqtParam = pPID;
                    else if (pPID == "" && nPID != "")
                        sqtParam = nPID;
                    else if (pPID != "" && nPID != "")
                        sqtParam = pPID + "," + nPID;

                    if (sqtParam != "")
                    {
                        DataSet dsPreproduct = new DataSet();
                        dsPreproduct = CommonComponent.GetCommonDataSet("SELECT imagename,MainCategory,SEName,productid,isnull(tb_Product.ProductURL,'') as ProductURL FROM tb_Product WHERE productid in (" + sqtParam + ")");

                        if (dsPreproduct != null && dsPreproduct.Tables.Count > 0 && dsPreproduct.Tables[0].Rows.Count == 2)
                        {
                            if (pPID != "")
                            {

                                if (pPID != null)
                                {
                                    DataRow[] dr = dsPreproduct.Tables[0].Select("productid=" + pPID.ToString() + "");
                                    foreach (DataRow dr1 in dr)
                                    {
                                        //ImgPrevious.HRef = "/" + dr1["MainCategory"].ToString() + "/" + dr1["SEName"].ToString() + "-" + pPID + ".aspx";
                                        ImgPrevious.HRef = "/" + dr1["ProductURL"].ToString().ToLower();
                                        imgPrev.Src = GetMicroImage(dr1["ImageName"].ToString());
                                        break;
                                    }
                                }
                                // IPreImage.Visible = true;
                                ImgPrevious.Visible = true;
                                imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 60%; margin-top: 2%;right:0;");
                            }
                            else
                            {
                                //IPreImage.Visible = false;
                                ImgPrevious.Visible = false;
                                imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 72%; margin-top: 2%;right:0;");
                            }
                            if (nPID != "")
                            {
                                DataRow[] dr = dsPreproduct.Tables[0].Select("productid=" + nPID.ToString() + "");
                                foreach (DataRow dr1 in dr)
                                {
                                    //ImgNext.HRef = "/" + dr1["MainCategory"].ToString() + "/" + dr1["SEName"].ToString() + "-" + nPID + ".aspx";
                                    ImgNext.HRef = "/" + dr1["ProductURL"].ToString().ToLower();
                                    imgNextImage.Src = GetMicroImage(dr1["ImageName"].ToString());
                                    break;
                                }
                                //INextImage.Visible = true;
                                ImgNext.Visible = true;
                                if (ImgPrevious.Visible == true)
                                    imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 60%; margin-top: 2%;right:0;");
                                else
                                    imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 72%; margin-top: 2%;right:0;");

                            }
                            else
                            {
                                //INextImage.Visible = false;
                                ImgNext.Visible = false;
                            }

                        }
                        else if (dsPreproduct != null && dsPreproduct.Tables.Count > 0 && dsPreproduct.Tables[0].Rows.Count == 1)
                        {
                            if (pPID != "")
                            {
                                DataRow[] dr = dsPreproduct.Tables[0].Select("productid=" + pPID.ToString() + "");
                                foreach (DataRow dr1 in dr)
                                {
                                    //ImgPrevious.HRef = "/" + dr1["MainCategory"].ToString() + "/" + dr1["SEName"].ToString() + "-" + pPID + ".aspx";
                                    ImgPrevious.HRef = "/" + dr1["ProductURL"].ToString().ToLower();
                                    imgPrev.Src = GetMicroImage(dr1["ImageName"].ToString());
                                    break;
                                }
                                //IPreImage.Visible = true;
                                ImgPrevious.Visible = true;
                                imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 60%; margin-top: 2%;right:0;");
                            }
                            else
                            {
                                // IPreImage.Visible = false;
                                ImgPrevious.Visible = false;
                                imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 72%; margin-top: 2%;right:0;");
                            }
                            if (nPID != "")
                            {
                                DataRow[] dr = dsPreproduct.Tables[0].Select("productid=" + nPID.ToString() + "");
                                foreach (DataRow dr1 in dr)
                                {
                                    //ImgNext.HRef = "/" + dr1["MainCategory"].ToString() + "/" + dr1["SEName"].ToString() + "-" + nPID + ".aspx";
                                    ImgNext.HRef = "/" + dr1["ProductURL"].ToString().ToLower();
                                    imgNextImage.Src = GetMicroImage(dr1["ImageName"].ToString());
                                    break;
                                }
                                //INextImage.Visible = true;

                                ImgNext.Visible = true;
                                ImgNext.Visible = true;
                                if (ImgPrevious.Visible == true)
                                    imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 60%; margin-top: 2%;right:0;");
                                else
                                    imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 72%; margin-top: 2%;right:0;");
                            }
                            else
                            {
                                //INextImage.Visible = false;
                                ImgNext.Visible = false;
                            }

                        }
                    }
                    else
                    {
                        //INextImage.Visible = false;
                        //IPreImage.Visible = false;
                        ImgPrevious.Visible = false;
                        ImgNext.Visible = false;
                    }
                }


            }
            catch
            {

            }
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

        /// <summary>
        /// Best Seller Repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
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
                            lblTagImage.Text = "<img  title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".jpg\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "'   />";
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
                    ltrRegularPrice.Text += "<p>" + Price.ToString("C") + "</p>";//"<p>Regular Price: " + Price.ToString("C") + "</p>";
                    hdnprice = Price;
                }
                else
                {
                    ltrRegularPrice.Text += "<p>&nbsp;</p>";
                }
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "<p>" + SalePrice.ToString("C") + "</p>";//"<p>Your Price: <strong>" + SalePrice.ToString("C") + "</strong></p>";
                    hdnprice = SalePrice;
                }
                else
                {
                    ltrYourPrice.Text += "<p>" + Price.ToString("C") + "</p>";
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

        /// <summary>
        /// Add '...', if String length is more than 50 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 50 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 50)
                Name = Name.Substring(0, 42) + "...";
            return Server.HtmlEncode(Name);
        }

        /// <summary>
        /// Add '...', if String length is more than 50 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 50 Length String </returns>
        public String SetNameRec(String Name)
        {
            if (Name.Length > 50)
                Name = Name.Substring(0, 39) + "...";
            return Server.HtmlEncode(Name);
        }

        /// <summary>
        /// Gets the Icon Image Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the  the Icon Image Path</returns>
        public String GetIconImageProduct(String img)
        {
            String imagepath = String.Empty;
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Icon/" + img;

            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(imagepath)))
            {
                return AppLogic.AppConfigs("Live_Contant_Server")+imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        /// <summary>
        /// Binds the Products with Details
        /// </summary>
        private void BindProducts()
        {
            ///Bind Best Seller
            DataSet dsBestSeller = ProductComponent.DisplyProductByOption("IsBestSeller", Convert.ToInt32(AppLogic.AppConfigs("StoreID")), 4);
            if (dsBestSeller != null && dsBestSeller.Tables.Count > 0 && dsBestSeller.Tables[0].Rows.Count > 0)
            {
                rptBestSeller.DataSource = dsBestSeller;
                rptBestSeller.DataBind();
            }
        }

        /// <summary>
        /// Binds the Static Data
        /// </summary>
        //private void BindStaticData()
        //{
        //    DataSet dsTopic = new DataSet();
        //    dsTopic = TopicComponent.GetTopicList("Faqs", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
        //    if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
        //    {
        //        ltproductfaq.Text = Convert.ToString(dsTopic.Tables[0].Rows[0]["Title"]);
        //        if (dsTopic.Tables[0].Rows[0]["Description"].ToString() == "")
        //        {
        //            ltproductfaq.Text = "<p style='padding-left:20px;'>Coming Soon...</p>";
        //        }
        //        else
        //        {
        //            ltproductfaq.Text = "<p >" + dsTopic.Tables[0].Rows[0]["Description"].ToString() + "</p>";
        //        }
        //    }
        //    else
        //    {
        //        ltproductfaq.Text = "<p style='padding-left:20px;'>Coming Soon...</p>";
        //    }
        //}

        /// <summary>
        /// Fills the Quantity Discount Table.
        /// </summary>
        /// <param name="ProductID">String ProductID</param>
        /// <param name="StoreID">String StoreID</param>
        public void FillQuantityDiscountTable(string ProductID, string StoreID)
        {
            ProductComponent objQtyDiscount = new ProductComponent();
            DataSet dsQtyDiscount = new DataSet();
            dsQtyDiscount = objQtyDiscount.GetQuantityDiscountTableByProductID(ProductID, StoreID);

            StringBuilder sw = new StringBuilder(4000);
            Int32 iCount = 1;
            if (dsQtyDiscount != null && dsQtyDiscount.Tables.Count > 0 && dsQtyDiscount.Tables[0].Rows.Count > 0)
            {

                sw.Append("<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"0\" align=\"center\" class=\"datatable\">");
                sw.Append("<tr><th colspan='2' align='center'>QUANTITY DISCOUNT AVAILABLE!</th></tr>");
                ltQtyDiscountHideen.Text = "";
                for (int i = 0; i < dsQtyDiscount.Tables[0].Rows.Count; i++)
                {
                    if (iCount % 2 == 0)
                    {
                        sw.Append("<tr class='altrow'>");
                    }
                    else
                    {
                        sw.Append("<tr>");
                    }
                    sw.Append("<td>" + dsQtyDiscount.Tables[0].Rows[i]["QuantityRange"] + " </td>");
                    DataSet dsPrice = new DataSet();
                    dsPrice = objQtyDiscount.GetQuantityDiscountTableByItem(ProductID, StoreID);

                    decimal price = Convert.ToDecimal(dsPrice.Tables[0].Rows[0]["price"].ToString());
                    decimal saleprice = Convert.ToDecimal(dsPrice.Tables[0].Rows[0]["saleprice"].ToString());

                    decimal pp = 0;

                    if (price > saleprice)
                    {
                        if (saleprice > Convert.ToDecimal(0))
                        {
                            pp = saleprice;
                        }
                        else
                        {
                            pp = price;
                        }
                    }
                    else
                    {
                        pp = price;
                    }
                    decimal calPrice = pp;

                    calPrice = (calPrice * Convert.ToDecimal(dsQtyDiscount.Tables[0].Rows[i]["DiscountPercent"].ToString().Replace("%", "").Replace(" ", ""))) / 100;

                    calPrice = Convert.ToDecimal(pp) - Convert.ToDecimal(Math.Round(calPrice, 2));
                    sw.Append("<td id=\"spn" + i.ToString() + "\"><strong> $" + string.Format("{0:0.00}", calPrice) + " </strong></td>");

                    sw.Append("</tr>");
                    iCount++;
                }
                sw.Append("</table>");
                ltquantitydiscount.Text = sw.ToString();
            }
        }

        /// <summary>
        /// Binds  More Images
        /// </summary>
        /// <param name="ImageName">String ImageName</param>
        /// <param name="strtitle">String strtitle</param>
        private void BindMoreImage(string ImageName, string strtitle)
        {
            try
            {
                string strElementClickedID = "";
                string strImageName = "";
                string strImgName = "";
                string strImageExt = "";
                StringBuilder strMoreImg = new StringBuilder();
                string strImagePath = "", strLImagePath = "";
                strImgName = ImageName;
                string[] ar = new string[2];
                char[] splitter = { '.' };
                ar = strImgName.Split(splitter);
                strImageName = ar[ar.Length - 2].ToString();
                strImageExt = ar[ar.Length - 1].ToString();

                strImagePath = GetMicroImage(strImageName + "." + strImageExt);
                strLImagePath = GetMoreMediumImage(strImageName + "." + strImageExt);
                if (strImagePath != AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "micro/image_not_available.jpg")
                {
                    strMoreImg.Append("<li><a  href=\"javascript:void(0);\"><img id='first' onmouseover=\"document.getElementById('ContentPlaceHolder1_imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click();\" onclick=\"document.getElementById('ContentPlaceHolder1_imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click();\" src=\"" + strImagePath + "\" height=\"78\" /></a></li>");
                    try
                    {
                        string StrMainImgDesc = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImageDescription,'') as ImageDescription from tb_Product where productid=" + Request.QueryString["PID"].ToString() + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + ""));
                        if (!string.IsNullOrEmpty(StrMainImgDesc) && StrMainImgDesc.Length > 0)
                        {
                            string StrFullname = Server.HtmlEncode(StrMainImgDesc.ToString());
                            if (StrMainImgDesc.Length > 35)
                            {
                                StrMainImgDesc = StrMainImgDesc.Substring(0, 30) + " ...";
                            }
                            strMoreImg.Append("<div  title=\"" + StrFullname.ToString() + "\">" + Server.HtmlEncode(StrMainImgDesc.ToString()) + "</div>");
                        }
                    }
                    catch { }
                }

                int CountMoreImage = 0;
                divtempimage.InnerHtml = "";
                Int32 imgCount = 0;
                for (int cnt = 1; cnt < 25; cnt++)
                {
                    if (imgCount >= 7)
                    {
                        break;
                    }
                    strImagePath = GetMicroImage(strImageName + "_" + cnt.ToString() + "." + strImageExt);
                    strLImagePath = GetMoreMediumImage(strImageName + "_" + cnt.ToString() + "." + strImageExt);
                    if (strImagePath != AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "micro/" + "image_not_available.jpg")
                    {

                        strMoreImg.Append("<li><a  href=\"javascript:void(0);\"><img  onmouseover=\"document.getElementById('ContentPlaceHolder1_imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click();\" onclick=\"document.getElementById('ContentPlaceHolder1_imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click();\" src=\"" + strImagePath + "\" height=\"78\" /></a></li>");
                        try
                        {
                            string StrMainImgDesc = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(Description,'') as ImgDescription from tb_productimgdesc where productid=" + Request.QueryString["PID"].ToString() + " and ImageNo =" + cnt + ""));

                            if (!string.IsNullOrEmpty(StrMainImgDesc) && StrMainImgDesc.Length > 0)
                            {
                                string StrFullname = Server.HtmlEncode(StrMainImgDesc.ToString());
                                if (StrMainImgDesc.Length > 35)
                                {
                                    StrMainImgDesc = StrMainImgDesc.Substring(0, 30) + " ...";
                                }
                                strMoreImg.Append("<div title=\"" + StrFullname.ToString() + "\">" + Server.HtmlEncode(StrMainImgDesc.ToString()) + "</div>");
                            }
                        }
                        catch { }
                        CountMoreImage++;
                        divtempimage.InnerHtml += "<img src=\"" + strLImagePath + "\" />";
                        imgCount++;
                    }

                }

                ltBindMoreImage.Text = strMoreImg.ToString();
                hideMoreImages.Visible = true;

            }
            catch { }
        }

        /// <summary>
        /// Gets the Micro Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns  the Micro Image Path</returns>
        public String GetMicroImage(String img)
        {
            String imagepath = String.Empty;
            //String Temp = imagepath =clsvariables.clsVariables.PathPMicroImage + img;
            String Temp = AppLogic.AppConfigs("ImagePathProduct") + "micro/" + img;
            imagepath = Temp;
            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(imagepath)))
            {
                return AppLogic.AppConfigs("Live_Contant_Server")+imagepath;
            }

            return AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "micro/" + "image_not_available.jpg";
        }

        /// <summary>
        /// Gets the More Medium Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Medium Image Path</returns>
        private String GetMoreMediumImage(String img)
        {
            String imagepath = String.Empty;
            String Temp = imagepath = AppLogic.AppConfigs("ImagePathProduct") + "medium/" + img;

            imagepath = Temp;

            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(imagepath)))
            {
                return AppLogic.AppConfigs("Live_Contant_Server")+imagepath;
            }

            return AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "medium/" + "image_not_available.jpg";
        }

        /// <summary>
        /// Optional Accessory Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvOptionalAcc_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                HiddenField hdnOARegularPrice = (HiddenField)e.Row.FindControl("hdnOARegularPrice");
                HiddenField hdnOASalePrice = (HiddenField)e.Row.FindControl("hdnOASalePrice");
                Label lblOARegularPrice = (Label)e.Row.FindControl("lblOARegularPrice");
                Label lblOASalePrice = (Label)e.Row.FindControl("lblOASalePrice");
                Label lblSalePriceTitle = (Label)e.Row.FindControl("lblSalePriceTitle");
                Decimal Price = Convert.ToDecimal(hdnOARegularPrice.Value);
                Decimal SalePrice = Convert.ToDecimal(hdnOASalePrice.Value);
                if (SalePrice == 0)
                {
                    lblOASalePrice.Visible = false;
                    lblSalePriceTitle.Visible = false;
                }
                else
                {
                    lblOASalePrice.Visible = true;
                    lblSalePriceTitle.Visible = true;
                }

            }
        }

        /// <summary>
        /// Loads the Video
        /// </summary>
        private void LoadVideo()
        {
            try
            {
                Random rd = new Random();
                string VideoPath = "";
                string imagepath = "";
                if (Request.QueryString["PID"] != null && Request.QueryString["PID"].ToString() != "")
                {

                    imagepath = "" + AppLogic.AppConfigs("ImagePathProduct") + "Medium/" + Request.QueryString["PID"].ToString() + ".jpg";
                    VideoPath = "" + AppLogic.AppConfigs("ImagePathProduct") + "Video/" + Request.QueryString["PID"].ToString() + ".flv";
                }
                if (System.IO.File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(imagepath)))
                { }
                else
                {
                    imagepath = "" + AppLogic.AppConfigs("Live_Contant_Server")+AppLogic.AppConfigs("ImagePathProduct") + "Medium/" + "image_not_available.gif";
                }
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(Server.MapPath("ProductVideo.xml"));
                XmlNodeList nodeList = xmlDoc.SelectNodes("/flash_parameters/album/slide");
                nodeList[0].Attributes["v_URL"].Value = VideoPath + "?" + rd.Next(1000);
                xmlDoc.Save(Server.MapPath("ProductVideo.xml"));
                string Script = " <object classid='clsid:d27cdb6e-ae6d-11cf-96b8-444553540000' codebase='http://fpdownload.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=6,0,0,0' width='306' height='219' id='tech' align='middle'> " +
             " <param name='allowFullScreen' value='true'/> " +
             " <param name='transparent' value='true'/> " +
     " <param name='allowScriptAccess' value='sameDomain' /> " +
     " <param name='movie' value='ProductVideo.swf?xml_path=ProductVideo.xml" + "?" + rd.Next(1000) + "' /> " +
     " <param name='quality' value='high' /> " +
     " <embed src='ProductVideo.swf?xml_path=ProductVideo.xml' quality='high' width='306' z-index:5666666666; height='218' name='tech' align='middle' allowScriptAccess='sameDomain' allowFullScreen='true' type='application/x-shockwave-flash' pluginspage='http://www.macromedia.com/go/getflashplayer' /> " +
    " </object>";
                LtVedioParam.Text = Script;
            }
            catch { }
        }

        private void BindStaticData(String ShippingTime)
        {
            String strPolicy = "";
            String strIsShowShippingTime = Convert.ToString(AppLogic.AppConfigs("SwitchItemShippingTime"));
            String strIsShowReturnPolicy = Convert.ToString(AppLogic.AppConfigs("SwitchItemReturnPolicy"));
            Int32 cntShipping = 0;
            if (strIsShowShippingTime.Trim().ToLower() == "false" && strIsShowReturnPolicy.Trim().ToLower() == "false")
            {
                ltrShippingReturnPolicy.Text = "";
            }
            else
            {
                strPolicy += "<div class=\"item-left-row3\" id=\"divshippingtab\">";
                strPolicy += "<div id=\"item-tb-left\">";
                strPolicy += "<div class=\"item-tb-left\">";
                strPolicy += "<div class=\"item-tb-left-tab\">";
                strPolicy += "<div class=\"tabing\" id=\"tabber2-holder\">";
                strPolicy += "<ul class=\"tabbernav\"><li class=\"tabberactive\" id=\"lishippingtime\" onclick=\"tabdisplaycartpolicy('lishippingtime','divshippingtime');\"><a href=\"javascript:void(null);\" title=\"Shipping Time\" >Shipping Time</a></li><li id=\"lireturnpolicy\" onclick=\"tabdisplaycartpolicy('lireturnpolicy','divreturnpolicy');\"><a href=\"javascript:void(null);\" title=\"Return Policy\" >Return Policy</a></li></ul>";
                strPolicy += "</div>";
                strPolicy += "</div>";
                strPolicy += "<div id=\"content-right\" class=\"tabberlive\">";
                if (strIsShowShippingTime.Trim().ToLower() == "true")
                {
                    cntShipping++;
                    strPolicy += "<div class=\"tabbertab\" id=\"divshippingtime\">";

                    strPolicy += "<div>";
                    // strPolicy += ShippingTime.Trim() == "" ? "<p>Coming Soon...</p>" : Server.HtmlDecode(ShippingTime.Trim());

                    strPolicy += Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Description FROM tb_Topic WHERE TopicName='ShippingTime' AND Storeid=1 AND isnull(Deleted,0)=0"));

                    //strPolicy += "<table cellpadding=\"0\" cellspacing=\"0\" width=\"99%\" >";
                    //strPolicy += "<tr>";
                    //strPolicy += "<td colspan=\"4\" align=\"left\">Orders are processed within 72 hours, Mon-Fri (Holidays Excluded).<br /><br /></td>";

                    //strPolicy += "</tr>";
                    //strPolicy += "<tr>";
                    //strPolicy += "<td colspan=\"4\" align=\"left\">Items marked as:<br /></td>";

                    //strPolicy += "</tr>";
                    //strPolicy += "<tr>";
                    //strPolicy += "<th align=\"left\" style=\"border-collapse: collapse;border:solid 1px #ddd;width:25%padding:5px;\">Ready Made: *</th>";
                    //strPolicy += "<th align=\"left\" style=\"border-collapse: collapse;border:solid 1px #ddd;width:25%padding:5px;\">Made to Order: *</th>";
                    //strPolicy += "<th align=\"left\" style=\"border-collapse: collapse;border:solid 1px #ddd;width:25%padding:5px;\">Made to Measure: *</th>";
                    //strPolicy += "<th align=\"left\" style=\"border-collapse: collapse;border:solid 1px #ddd;width:25%;padding:5px;\">Swatches: *</th>";
                    //strPolicy += "</tr>";

                    //strPolicy += "<tr>";
                    //strPolicy += "<td align=\"left\" style=\"border-collapse: collapse;border:solid 1px #ddd;padding:5px;\">Delivery within 10-12 Business Days (Holidays Excluded)</td>";
                    //strPolicy += "<td align=\"left\" style=\"border-collapse: collapse;border:solid 1px #ddd;padding:5px;\">Delivery within 3-4 weeks. <br />NOTE: Embroidered Thai Silk Collection delivery within 6-8 weeks. <br />Collection delivery within 6-8 weeks. </td>";
                    //strPolicy += "<td align=\"left\" style=\"border-collapse: collapse;border:solid 1px #ddd;padding:5px;\">Delivery within 3-4 weeks. <br />NOTE: Embroidered Thai Silk Collection delivery within 6-8 weeks. <br />Collection delivery within 6-8 weeks. </td>";
                    //strPolicy += "<td align=\"left\" style=\"border-collapse: collapse;border:solid 1px #ddd;padding:5px;\">Swatches Sent Priority Mail. Please allow 10-12 Buiness days for delivery. </td>";
                    //strPolicy += "</tr>";
                    //strPolicy += "<tr>";
                    //strPolicy += "<td colspan=\"4\" align=\"left\"><br />* Items marked as BACKORDER are generally delivered in 3-4 weeks. Please see individual item pages for specific details.</td>";

                    //strPolicy += "</tr>";
                    //strPolicy += "</table>";



                    strPolicy += "</div>";
                    strPolicy += "</div>";
                }
                if (strIsShowReturnPolicy.Trim().ToLower() == "true")
                {
                    cntShipping++;
                    DataSet dsTopic = new DataSet();
                    dsTopic = TopicComponent.GetTopicList("returnpolicy", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                    String strReturnPolicy = "";
                    if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                    {
                        //if (dsTopic.Tables[0].Rows[0]["Description"].ToString() == "")
                        //{
                        //    strReturnPolicy = "<p>Coming Soon...</p>";
                        //}
                        //else
                        //{
                        //    strReturnPolicy = "<p>" + (dsTopic.Tables[0].Rows[0]["Description"].ToString()) + "</p>";
                        //}
                        titlereturn.InnerHtml = dsTopic.Tables[0].Rows[0]["title"].ToString();
                        ltrreturnpolicy.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                        strReturnPolicy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Description,Title FROM tb_Topic WHERE TopicName='productreturnPolicy' AND Storeid=1 AND isnull(Deleted,0)=0"));
                        dsTopic.Dispose();
                    }
                    strPolicy += "<div class=\"tabbertab\" id=\"divreturnpolicy\" style=\"display:none;\">";

                    strPolicy += "<div>";
                    strPolicy += strReturnPolicy.Trim();
                    strPolicy += "</div>";
                    strPolicy += "</div>";
                }
                strPolicy += "</div>";
                strPolicy += "</div>";
                strPolicy += "</div>";
                strPolicy += "</div>";
            }

            if (cntShipping > 0)
            {
                ltrShippingReturnPolicy.Text = strPolicy.ToString();
            }
        }


        //public static string GetData(Int32 ProductId, Int32 Width, Int32 Length, Int32 Qty)
        //{
        //    string resp = string.Empty;
        //    DataSet ds = new DataSet();
        //    Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
        //    ds = objSql.GetDs("EXEC usp_Product_Pricecalculator " + ProductId + "," + Width.ToString() + "," + Length.ToString() + "," + Qty.ToString() + "");
        //    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        //    {
        //        resp = " <tt>Your Price :</tt> <strong>$" + ds.Tables[0].Rows[0][0].ToString() + "</strong>";
        //    }

        //    return resp;
        //}

        public void BindProductProperty()
        {
            try
            {
                DataSet dsProductProperty = new DataSet();
                dsProductProperty = CommonComponent.GetCommonDataSet("select Isproperty,ProductID, isnull(LightControl,0) as LightControl,isnull(Privacy,0) as Privacy,isnull(Efficiency,0) as Efficiency from tb_Product where ProductID=" + Convert.ToString(Request.QueryString["PID"]) + "");
                if (dsProductProperty != null && dsProductProperty.Tables.Count > 0 && dsProductProperty.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["LightControl"].ToString()) > 0 || Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["Privacy"].ToString()) > 0 || Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["Efficiency"].ToString()) > 0)
                    {
                        if (Convert.ToBoolean(dsProductProperty.Tables[0].Rows[0]["Isproperty"].ToString()) == true)
                        {
                            divProductProperty.Visible = true;
                            if (Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["LightControl"].ToString()) > 0)
                            {
                                divLightControl.Visible = true;
                                imgLightControl.Src = "/images/light_" + Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["LightControl"].ToString()) + ".png";
                            }
                            if (Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["Privacy"].ToString()) > 0)
                            {
                                divPrivacy.Visible = true;
                                imgPrivacy.Src = "/images/privacy_" + Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["Privacy"].ToString()) + ".png";
                            }
                            if (Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["Efficiency"].ToString()) > 0)
                            {
                                divEfficiency.Visible = true;
                                imgEfficiency.Src = "/images/thermal_" + Convert.ToInt32(dsProductProperty.Tables[0].Rows[0]["Efficiency"].ToString()) + ".png";
                            }
                        }
                        else
                        {
                            divProductProperty.Visible = false;
                        }
                    }
                    else
                    {
                        divProductProperty.Visible = false;
                    }

                }
                else
                {
                    divProductProperty.Visible = false;
                }
            }
            catch { }
        }

        public void BindProductQuote()
        {
            try
            {
                DataSet dsProductQuote = new DataSet();
                dsProductQuote = CommonComponent.GetCommonDataSet("select isnull(IsPriceQuote,0) as IsPriceQuote,ProductID from tb_Product where ProductID=" + Convert.ToString(Request.QueryString["PID"]) + "");
                string strScript = "";
                if (dsProductQuote != null && dsProductQuote.Tables.Count > 0 && dsProductQuote.Tables[0].Rows.Count > 0)
                {
                    if (Convert.ToBoolean(dsProductQuote.Tables[0].Rows[0]["IsPriceQuote"].ToString()) == true)
                    {
                        divPriceQuote.InnerHtml = strScript.ToString() + divPriceQuote.InnerHtml.ToString();
                        divpricequtoteformadetomeasure.Visible = true;
                    }
                    else
                        divpricequtoteformadetomeasure.Visible = false;
                }
                else divpricequtoteformadetomeasure.Visible = false;
            }
            catch { }
        }
        public void BindFabricSwatchBanner(String SKU)
        {
            try
            {
                //DataSet dsFabricSwatchBanner = new DataSet();
                //dsFabricSwatchBanner = CommonComponent.GetCommonDataSet("select Isfreefabricswatch,ProductID from tb_Product where ProductID=" + Convert.ToString(Request.QueryString["PID"]) + "");
                //if (dsFabricSwatchBanner != null && dsFabricSwatchBanner.Tables.Count > 0 && dsFabricSwatchBanner.Tables[0].Rows.Count > 0)
                //{
                //    if (Convert.ToBoolean(dsFabricSwatchBanner.Tables[0].Rows[0]["Isfreefabricswatch"].ToString()) == true)
                //        divFreeFabricSwatchBanner.Visible = true;
                //    else
                //        divFreeFabricSwatchBanner.Visible = false;
                //}
                //else divFreeFabricSwatchBanner.Visible = false;


                DataSet dsFabricSwatchBanner = new DataSet();
                Boolean isImageAvail = false;
                dsFabricSwatchBanner = CommonComponent.GetCommonDataSet("select Isfreefabricswatch,ProductID from tb_Product where ProductID=" + Convert.ToString(Request.QueryString["PID"]) + "");
                if (dsFabricSwatchBanner != null && dsFabricSwatchBanner.Tables.Count > 0 && dsFabricSwatchBanner.Tables[0].Rows.Count > 0)
                {
                    DataSet dsSwatchBanner = new DataSet();
                    dsSwatchBanner = CommonComponent.GetCommonDataSet(@"SELECT ISNULL(BannerTitle,'') AS BannerTitle,ISNULL(BannerUrl,'') AS BannerUrl,ISNULL(BannerImage,'') AS BannerImage FROM dbo.tb_ItemBanners INNER JOIN dbo.tb_ItemBannerType ON dbo.tb_ItemBanners.ItemBannerTypeID = dbo.tb_ItemBannerType.ItemBannerTypeID
                                    WHERE ISNULL(','+Products+',','') like '%," + SKU + ",%' AND dbo.tb_ItemBanners.ItemBannerTypeID=1 AND ISNULL(dbo.tb_ItemBanners.Active,0)=1 AND convert(char(10),CAST(dbo.tb_ItemBanners.EndDate AS DATETIME),101)  >= convert(char(10),CAST('" + DateTime.Now.ToString() + "' AS DATETIME) ,101) AND  convert(char(10),CAST(dbo.tb_ItemBanners.StartDate AS DATETIME),101)  <= convert(char(10),CAST('" + DateTime.Now.ToString() + "' AS DATETIME) ,101)");
                    if (dsSwatchBanner != null && dsSwatchBanner.Tables.Count > 0 && dsSwatchBanner.Tables[0].Rows.Count > 0)
                    {
                        lnkFabricSwatchBanner.HRef = Convert.ToString(dsSwatchBanner.Tables[0].Rows[0]["BannerUrl"]) == "" ? "javascript:void(0);" : Convert.ToString(dsSwatchBanner.Tables[0].Rows[0]["BannerUrl"]);

                        String strImageName = Convert.ToString(dsSwatchBanner.Tables[0].Rows[0]["BannerImage"]);
                        if (strImageName.Trim() != "")
                        {
                            String strImagePath = AppLogic.AppConfigs("ImagePathBanner") + "Item";

                            if (File.Exists(Server.MapPath(strImagePath + "/" + strImageName.Trim())))
                            {
                                isImageAvail = true;
                                imgFabricSwatchBanner.Src = strImagePath + "/" + strImageName.Trim();
                                divFreeFabricSwatchBanner.Visible = true;
                            }
                        }
                    }
                }
                if (!isImageAvail)
                {
                    divFreeFabricSwatchBanner.Visible = false;
                }
                DataSet dsSmallBanner = new DataSet();
                dsSmallBanner = CommonComponent.GetCommonDataSet(@"SELECT ISNULL(BannerTitle,'') AS BannerTitle,ISNULL(BannerUrl,'') AS BannerUrl,ISNULL(BannerImage,'') AS BannerImage FROM dbo.tb_ItemBanners INNER JOIN dbo.tb_ItemBannerType ON dbo.tb_ItemBanners.ItemBannerTypeID = dbo.tb_ItemBannerType.ItemBannerTypeID
                                    WHERE ISNULL(','+Products+',','') like '%," + SKU + ",%' AND dbo.tb_ItemBanners.ItemBannerTypeID=2 AND ISNULL(dbo.tb_ItemBanners.Active,0)=1 AND convert(char(10),CAST(dbo.tb_ItemBanners.EndDate AS DATETIME),101)  >= convert(char(10),CAST('" + DateTime.Now.ToString() + "' AS DATETIME) ,101) AND  convert(char(10),CAST(dbo.tb_ItemBanners.StartDate AS DATETIME),101)  <= convert(char(10),CAST('" + DateTime.Now.ToString() + "' AS DATETIME) ,101)");
                if (dsSmallBanner != null && dsSmallBanner.Tables.Count > 0 && dsSmallBanner.Tables[0].Rows.Count > 0)
                {
                    //lnkFabricSwatchBanner.HRef = Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerUrl"]) == "" ? "javascript:void(0);" : Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerUrl"]);

                    String strImageName = Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerImage"]);
                    if (strImageName.Trim() != "")
                    {
                        String strImagePath = AppLogic.AppConfigs("ImagePathBanner") + "Item";

                        if (File.Exists(Server.MapPath(strImagePath + "/" + strImageName.Trim())))
                        {
                            if (!string.IsNullOrEmpty(dsSmallBanner.Tables[0].Rows[0]["BannerUrl"].ToString()))
                            {
                                ltmadeSmallbanner.Text = "<a href=\"" + dsSmallBanner.Tables[0].Rows[0]["BannerUrl"].ToString() + "\"><img class=\"banner_tag_made\" title=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\"  alt=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\" src=\"" + strImagePath + "/" + strImageName.Trim() + "\" /></a>";
                                ltswatchSmallbanner.Text = "<a href=\"" + dsSmallBanner.Tables[0].Rows[0]["BannerUrl"].ToString() + "\"><img class=\"banner_tag\" title=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\"  alt=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\" src=\"" + strImagePath + "/" + strImageName.Trim() + "\" /></a>";
                                ltradySmallbanner.Text = "<a href=\"" + dsSmallBanner.Tables[0].Rows[0]["BannerUrl"].ToString() + "\"><img class=\"banner_tag\" title=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\"  alt=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\" src=\"" + strImagePath + "/" + strImageName.Trim() + "\" /></a>";
                            }
                            else
                            {
                                ltmadeSmallbanner.Text = "<img class=\"banner_tag_made\" title=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\"  alt=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\" src=\"" + strImagePath + "/" + strImageName.Trim() + "\" />";
                                ltswatchSmallbanner.Text = "<img class=\"banner_tag\" title=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\"  alt=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\" src=\"" + strImagePath + "/" + strImageName.Trim() + "\" />";
                                ltradySmallbanner.Text = "<img class=\"banner_tag\" title=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\"  alt=\"" + Convert.ToString(dsSmallBanner.Tables[0].Rows[0]["BannerTitle"].ToString()) + "\" src=\"" + strImagePath + "/" + strImageName.Trim() + "\" />";
                            }


                        }
                    }



                }

            }
            catch { }
        }
        public void GetColorOption()
        {
//            DataSet dscolorOption = new DataSet();

//            dscolorOption = CommonComponent.GetCommonDataSet(@"Declare @temp nvarchar(max)
//                SET @temp=''
//                SET @temp=(select  isnull(RelatedProductId,'')+',' from  tb_ProductVariantValue where ProductID = " + Request.QueryString["PID"].ToString() + @" and isnull(RelatedProductId,'')<> '' FOR XML Path(''))
//                SET @temp = ''''+REPLACE(@temp,',',''',''')
//                SET @temp = SUBSTRING(@temp,0,LEN(@temp)-1)
//                EXEC ('select * from tb_product where active=1 and deleted=0 and isnull(SKU,'''') in ('+ @temp +')')");
//            if (dscolorOption != null && dscolorOption.Tables.Count > 0 && dscolorOption.Tables[0].Rows.Count > 0)
//            {
//                RepColorOption.DataSource = dscolorOption;
//                RepColorOption.DataBind();
//                divColorOptionForFabric.Visible = true;
//            }
//            else
//            {
                RepColorOption.DataSource = null;
                RepColorOption.DataBind();
                divColorOptionForFabric.Visible = false;
            //}

        }
        protected void RepColorOption_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Catbox = (HtmlGenericControl)e.Item.FindControl("Catbox");
                HtmlGenericControl CatDisplay = (HtmlGenericControl)e.Item.FindControl("CatDisplay");
                Literal litControl = (Literal)e.Item.FindControl("ltTop");
                Literal ltbottom = (Literal)e.Item.FindControl("ltBottom");
                Image imgName = (Image)e.Item.FindControl("imgName");
                //if ((e.Item.ItemIndex + 1) % 8 == 0 && e.Item.ItemIndex != 0)
                //{
                //    litControl.Text = "</ul><ul>";
                //}
                if (ViewState["strcolroptionimg"] != null)
                {
                    ViewState["strcolroptionimg"] = ViewState["strcolroptionimg"].ToString() + "$('#loader_imgcolor" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#" + imgName.ClientID.ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').load(function () {$('#loader_imgcolor" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";

                }
                else
                {
                    ViewState["strcolroptionimg"] = "$('#loader_imgcolor" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#" + imgName.ClientID.ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').load(function () {$('#loader_imgcolor" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";

                }
                if (ViewState["strcolroptionimgpostback"] != null)
                {
                    ViewState["strcolroptionimgpostback"] = ViewState["strcolroptionimgpostback"].ToString() + "$('#loader_imgcolor" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                else
                {
                    ViewState["strcolroptionimgpostback"] = "$('#loader_imgcolor" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                if ((e.Item.ItemIndex + 1) % itemCount == 0 && e.Item.ItemIndex != 0)
                {
                    //Probox.Attributes.Add("style", "margin-right:0px;");
                    ////proDisplay.Attributes.Add("class", "pro-display-none");
                    litControl.Text = "</ul><ul><li>";
                    ltbottom.Text = "</li>";
                    itemCount = itemCount + 7;
                }
                else
                {
                    litControl.Text = "<li>";
                    ltbottom.Text = "</li>";
                }
            }
        }

        public String SetNameForColor(String Name)
        {
            if (Name.Length > 35)
                Name = Name.Substring(0, 30) + "...";
            return Server.HtmlEncode(Name);
        }

        private void GetparentCategory()
        {
            Int32 Catid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 CategoryID FROM tb_ProductCategory WHERE ProductID=" + Request.QueryString["PID"].ToString() + ""));
            GetchildCategory(Catid.ToString());
        }
        private void GetchildCategory(string cid)
        {
            DataSet dsCategory = new DataSet();
            dsCategory = CommonComponent.GetCommonDataSet("SELECT * FROM tb_CategoryMapping WHERE CategoryID=" + cid.ToString() + "");
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsCategory.Tables[0].Rows[0]["ParentCategoryID"].ToString()) == 0)
                {
                    Session["HeaderCatid"] = dsCategory.Tables[0].Rows[0]["CategoryID"].ToString();
                }
                else
                {
                    GetchildCategory(dsCategory.Tables[0].Rows[0]["ParentCategoryID"].ToString());
                }
            }
        }

        private void GetMakeOrderStyle()
        {
            DataSet ds = new DataSet();
            ds = TopicComponent.GetTopicAccordingStoreID("makeorderstyle", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltMakeOrderStyle.Text = ds.Tables[0].Rows[0]["Description"].ToString();
            }
            else
            {
                ltMakeOrderStyle.Text = "Comming Soon.....";
            }
        }
        private void GetMakeOrderWidth()
        {
            DataSet ds = new DataSet();
            ds = TopicComponent.GetTopicAccordingStoreID("makeorderwidth", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltMakeOrderWidth.Text = ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
            }
            else
            {
                ltMakeOrderWidth.Text = "Comming Soon.....";
            }
        }
        private void GetMakeOrderLength()
        {
            DataSet ds = new DataSet();
            ds = TopicComponent.GetTopicAccordingStoreID("makeorderlength", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltMakeOrderLength.Text = ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
            }
            else
            {
                ltMakeOrderLength.Text = "Comming Soon.....";
            }
        }
        private void GetMakeOrderOptions()
        {
            DataSet ds = new DataSet();
            ds = TopicComponent.GetTopicAccordingStoreID("makeorderoptions", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltMakeOrderOptions.Text = ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
            }
            else
            {
                ltMakeOrderOptions.Text = "Comming Soon.....";
            }
        }
        private void GetMakeOrderQuantity()
        {
            DataSet ds = new DataSet();
            ds = TopicComponent.GetTopicAccordingStoreID("makeorderquantity", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltMakeOrderQuantity.Text = ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
            }
            else
            {
                ltMakeOrderQuantity.Text = "Comming Soon.....";
            }
        }
        private void GetMeasuringData()
        {
            DataSet ds = new DataSet();
            ds = TopicComponent.GetTopicAccordingStoreID("measuringguide", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltmeasuredata.Text = ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
            }
            else
            {
                ltmeasuredata.Text = "Comming Soon.....";
            }
        }
        private void GetFaqData()
        {
            DataSet ds = new DataSet();
            ds = TopicComponent.GetTopicAccordingStoreID("faq", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltrfaq.Text = ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
            }
            else
            {
                ltrfaq.Text = "Comming Soon.....";
            }
        }

        private void GetPleatGuideData()
        {
            DataSet ds = new DataSet();
            ds = TopicComponent.GetTopicAccordingStoreID("pleatguide", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                ltrPleatGuide.Text = "<div class=\"static-title\"><span style=\"padding-left: 10px;\">" + ds.Tables[0].Rows[0]["Title"].ToString() + "</span></div>" + ds.Tables[0].Rows[0]["Description"].ToString().Replace("<p>", "<p style='margin-top: 0px; margin-bottom: 0px;'>");
            }
            else
            {
                ltrPleatGuide.Text = "Comming Soon.....";
            }
        }
        protected void btnReviewCount_Click(object sender, EventArgs e)
        {
            int YesCnt = 0, NoCnt = 0;
            string IPAddress = Convert.ToString(HttpContext.Current.Request.UserHostAddress);

            if (!string.IsNullOrEmpty(hdnReviewId.Value.ToString().Trim()) && !string.IsNullOrEmpty(hdnReviewType.Value.ToString().Trim()))
            {
                if (hdnReviewType.Value.ToString().ToLower() == "yes")
                    YesCnt = 1;
                if (hdnReviewType.Value.ToString().ToLower() == "no")
                    NoCnt = 1;
                DataSet DsRatingCount = CommonComponent.GetCommonDataSet("Select RatingCountID,[RatingID],[StoreID],ISNULL(Yes,0) as TotYes,ISNULL(No,0) as TotNo,[IPAddress],[CreatedOn] from [tb_ReviewCount] where IPAddress='" + IPAddress + "' and RatingID=" + hdnReviewId.Value + "");
                if (DsRatingCount != null && DsRatingCount.Tables.Count > 0 && DsRatingCount.Tables[0].Rows.Count > 0)
                {
                    int RatingCountID = Convert.ToInt32(DsRatingCount.Tables[0].Rows[0]["RatingCountID"].ToString());
                    if (YesCnt == 1) { NoCnt = 0; }
                    if (NoCnt == 1) { YesCnt = 0; }

                    CommonComponent.ExecuteCommonData("Update tb_ReviewCount set yes=" + YesCnt + ",no=" + NoCnt + " where RatingCountID=" + RatingCountID + "");
                }
                else
                {
                    CommonComponent.ExecuteCommonData("INSERT INTO [dbo].[tb_ReviewCount] ([RatingID],[StoreID],[Yes],[No],[IPAddress],[CreatedOn]) " +
                                                         " VALUES(" + hdnReviewId.Value + "," + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + "," + YesCnt + "," + NoCnt + ",'" + IPAddress.ToString() + "',getdate())");
                }
                if (Request.QueryString["PID"] != null)
                {
                    string strProductid = Convert.ToString(Request.QueryString["PID"]);
                    GetCommnetByCustomer(Convert.ToInt32(strProductid));
                }
                ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msgreviewcnt", "jAlert('Thank you for review','Review');", true);
            }
        }
        protected void ddlSortReviewCnt_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSortReviewCnt1.SelectedValue = ddlSortReviewCnt.SelectedValue.ToString();

            if (Request.QueryString["PID"] != null)
            {
                string strProductid = Convert.ToString(Request.QueryString["PID"]);

                GetCommnetByCustomer(Convert.ToInt32(strProductid));
            }
        }
        protected void ddlSortReviewCnt1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSortReviewCnt.SelectedValue = ddlSortReviewCnt1.SelectedValue.ToString();

            if (Request.QueryString["PID"] != null)
            {
                string strProductid = Convert.ToString(Request.QueryString["PID"]);

                GetCommnetByCustomer(Convert.ToInt32(strProductid));
            }
        }
    }
}