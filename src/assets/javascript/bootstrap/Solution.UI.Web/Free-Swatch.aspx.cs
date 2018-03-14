using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.IO;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web
{
    public partial class Free_Swatch : System.Web.UI.Page
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
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// 
        string strView = "";
        Int32 itemCount = 6;
        Int32 TotalCount = 6;
        protected void Page_Load(object sender, EventArgs e)
        {


            //  gridlisttop.Visible = false; listbottom.Visible = false;
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

                    string[] strGetfiles = System.IO.Directory.GetFiles(AppLogic.AppConfigs("Live_Contant_Server_Path") + "/images/FreeSwatchBanner/");
                    if (strGetfiles.Length > 0)
                    {
                        foreach (string strfl in strGetfiles)
                        {
                            if (File.Exists(strfl))
                            {
                                FileInfo fl = new FileInfo(strfl);
                                imgBanner.Src = AppLogic.AppConfigs("Live_Contant_Server") + "/images/FreeSwatchBanner/" + fl.Name.ToString();
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
                if (Request.QueryString["CatID"] != null)
                {
                    Session["HeaderSubCatid"] = Request.QueryString["CatID"].ToString();
                }
                else
                {
                    Session["HeaderSubCatid"] = null;
                }
                BindCategory();
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgload", "getReminingproductDefault('/ProductCompare.aspx');", true);
                BindSubCategoryNameDescription();
                BindSubCategoryofMainCategory();
                BindProductOfSubCategory();
                breadcrumbs();

                if (Session["CmpProductID"] != null && Session["CmpProductID"].ToString() != "")
                    Getdata(Session["CmpProductID"].ToString());
                GetCompareProduct();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msggcompare", "compare(0, 0);", true);
                //BindPattern();
                //BindFabric();
                //BindStyle();
                //BindColors();
                //BindHeader();
                if (ViewState["strFeaturedimg"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloader", "$(document).ready(function () {" + ViewState["strFeaturedimg"].ToString() + "});", true);
                }
                if (ViewState["strFeaturedimglist"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderlist", "$(document).ready(function () {" + ViewState["strFeaturedimglist"].ToString() + "});", true);

                }
                //if (Convert.ToInt32(hdnallpages.Value.ToString()) > 0)
                //{
                //    btntempclick_Click(null, null);
                //}
                if (hdnallpages.Value.ToString() == "1")
                {
                    btnIndexPriceGo1_Click(null, null);
                }
                else if (hdnallpages.Value.ToString() == "2")
                {
                    btnIndexPriceGo_Click(null, null);
                }
                else if (hdnallpages.Value.ToString() == "3")
                {
                    ddlTopPrice_SelectedIndexChanged1(null, null);
                }
                else if (hdnallpages.Value.ToString() == "4")
                {
                    lnkViewAll_Click(null, null);
                }
                else if (hdnallpages.Value.ToString() == "5")
                {
                    lnkViewAllPages_Click(null, null);
                }
                else if (hdnallpages.Value.ToString() == "6")
                {
                    grid_bottom_Click(null, null);
                }
                else if (hdnallpages.Value.ToString() == "7")
                {
                    list_bottom_Click(null, null);
                }
                else if (hdnallpages.Value.ToString() == "8")
                {
                    itemCount = 6;
                    if (hdnpagenumber.Value.ToString().Trim() != "")
                    {
                        CurrentPage = Convert.ToInt32(hdnpagenumber.Value.ToString().Trim());
                        BindProductOfSubCategory();
                    }
                }
                else if (hdnallpages.Value.ToString() == "9")
                {
                    if (hdnpagenumber.Value.ToString().Trim() != "")
                    {
                        CurrentPage = Convert.ToInt32(hdnpagenumber.Value.ToString().Trim());
                        CurrentPage += 1;
                        lnkNext_Click(null, null);
                    }
                }
                else if (hdnallpages.Value.ToString() == "10")
                {
                    if (hdnpagenumber.Value.ToString().Trim() != "")
                    {
                        CurrentPage = Convert.ToInt32(hdnpagenumber.Value.ToString().Trim());
                        CurrentPage--;
                        lnkPrevious_Click(null, null);
                    }
                }
                else if (hdnallpages.Value.ToString() == "11")
                {
                    ddltopcategory_SelectedIndexChanged(null, null);
                }
                hdnallpages.Value = "0";
                hdnquickview.Value = "0";

                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("SwatchMaxlength")) && Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength")) > 0)
                {
                    hdnswatchmaxlength.Value = Convert.ToString(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                }
            }
            else
            {
                hdnallpages.Value = "0";
                if (ViewState["strFeaturedimgpostback"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
                }
                if (ViewState["strFeaturedimglistpostback"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
                }
            }
            if (Session["CmpProductID"] != null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "comparetable", "compare(0, 0);", true);
            }

        }


        /// <summary>
        /// Get BreadCrumbs
        /// </summary>
        private void breadcrumbs()
        {
            ltbreadcrmbs.Text = "<a href=\"/\" title=\"Home\">Home </a><img src=\"/images/breadcrumbs-bullet.png\" alt=\"\" title=\"\" class=\"breadcrumbs-bullet\"><span> Free Swatch </span>";
            //ConfigurationComponent.GetBreadCrum(Convert.ToInt32(Request.QueryString["CatID"]), Convert.ToInt32(Request.QueryString["CatPID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID").ToString()), "", 3, 0);
        }

        /// <summary>
        /// Bind Product of Subcategory
        /// </summary>
        private void BindProductOfSubCategory()
        {
            DataSet dsProduct = null;
            string StrCategoryQuery = "";
            if (ddltopcategory.SelectedValue.ToString() == "0")
            {
                StrCategoryQuery = Convert.ToString(CommonComponent.GetScalarCommonData("Select CAST(CategoryID as varchar(max)) +','  from tb_category Where Name like '%swatch%' and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and isnull(Active,0) = 1 and isnull(Deleted,0) = 0 for xml path('') "));
            }
            else
            {
                StrCategoryQuery = ddltopcategory.SelectedValue.ToString(); //Convert.ToString(CommonComponent.GetScalarCommonData("Select CAST(CategoryID as varchar(max)) +','  from tb_category Where categoryid =" + ddltopcategory.SelectedValue.ToString() + " and StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + " and isnull(Active,0) = 1 and isnull(Deleted,0) = 0 for xml path('') "));
            }
            if (!string.IsNullOrEmpty(StrCategoryQuery) && StrCategoryQuery.Length > 0)
            {
                if (StrCategoryQuery.IndexOf(",") > -1)
                {
                    StrCategoryQuery = StrCategoryQuery.Substring(0, StrCategoryQuery.Length - 1);
                }
                dsProduct = ProductComponent.GetFreeSwatchProductDetailsByOrderPrice(StrCategoryQuery.ToString(), Convert.ToInt32(1), ddlTopPrice.SelectedValue);

                //dsProduct = ProductComponent.GetProductDetailsByOrderPrice(Convert.ToInt32(Request.QueryString["CatID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID")), ddlTopPrice.SelectedValue);
                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    //string product_count = CommonComponent.GetScalarCommonData("select   count(*) as productcount	from tb_Product p inner join tb_ProductCategory pc on p.ProductID=pc.ProductID " +
                    //          " where p.StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")) + "" +
                    //          " and pc.CategoryID in (" + StrCategoryQuery.ToString() + ")" +
                    //          " and isnull(Active,0) = 1 and isnull(Isfreefabricswatch,0) = 1  and isnull(Deleted,0) = 0").ToString();

                    string product_count = dsProduct.Tables[0].Rows.Count.ToString();
                    productcount.Value = product_count;
                    if (Convert.ToInt32(product_count) == 1) { ltrtopproductcount.Text = product_count + " " + "Item"; ltrbottomcount.Text = ltrtopproductcount.Text; }
                    else { ltrtopproductcount.Text = product_count + " " + "Item(s)"; ltrbottomcount.Text = ltrtopproductcount.Text; }
                    ////dsProduct.Tables[0].Rows.Count.ToString();
                    dvMessage.Visible = false;
                    topNumber.Visible = true;
                    topMiddle.Visible = true;
                   // topBottom.Visible = true;
                    divtopitemcount.Visible = true;
                    divbottomitemcount.Visible = true;
                    ////RepProduct.DataSource = dsProduct;
                    ////RepProduct.DataBind();
                    ////Repeaterlistview.DataSource = dsProduct;
                    ////Repeaterlistview.DataBind();
                    #region Paging Code

                    pgsource.DataSource = dsProduct.Tables[0].DefaultView;
                    pgsource.AllowPaging = true;
                    if (ViewState["All"] == null)
                    {
                        lnkViewAllPages.Visible = false;
                        divViewAllPages.Visible = false;
                        lnkBottomViewAllPages.Visible = false;
                        divViewAllPagesBottom.Visible = false;
                        pgsource.PageSize = 15;
                        divTopPaging.Visible = true;
                        divBottomPaging.Visible = true;
                    }
                    else
                    {
                        lnkViewAllPages.Visible = true;
                       // divViewAllPages.Visible = true;
                        lnkBottomViewAllPages.Visible = true;
                        //divViewAllPagesBottom.Visible = true;
                        CurrentPage = 0;
                        divTopPaging.Visible = false;
                        divBottomPaging.Visible = false;
                        pgsource.PageSize = dsProduct.Tables[0].Rows.Count;
                    }


                    if (CurrentPage == pgsource.PageCount && CurrentPage != 0)
                    {
                        CurrentPage--;
                    }
                    pgsource.CurrentPageIndex = CurrentPage;

                    //Store it Total pages value in View state
                    ViewState["totpage"] = pgsource.PageCount;

                    this.lnkPrevious.Visible = !pgsource.IsFirstPage;
                    this.lnkNext.Visible = !pgsource.IsLastPage;
                    this.lnkTopprevious.Visible = !pgsource.IsFirstPage;
                    this.lnktopNext.Visible = !pgsource.IsLastPage;

                    if (CurrentPage == 0 && CurrentPage == pgsource.PageCount - 1)
                    {
                        lnkViewAll.Visible = false;
                        lnkBottomViewAll.Visible = false;
                    }
                    else
                    {
                        lnkViewAll.Visible = true;
                        lnkBottomViewAll.Visible = true;
                    }


                    RecordCount = pgsource.Count;
                    itemCount = 6;
                    TotalCount = Convert.ToInt32(dsProduct.Tables[0].Rows.Count.ToString());
                    RepProduct.DataSource = pgsource;
                    RepProduct.DataBind();
                    Repeaterlistview.DataSource = pgsource;
                    Repeaterlistview.DataBind();
                    doPaging();
                    RepeaterPaging.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    dlToppaging.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                    #endregion
                    divTopPaging.Visible = true;
                    divBottomPaging.Visible = true;
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
                   // topNumber.Visible = false;
                     
                    divTopPaging.Visible = false;
                    divBottomPaging.Visible = false;
                    topMiddle.Visible = false;
                   // topBottom.Visible = false;
                    divtopitemcount.Visible = false;
                    divbottomitemcount.Visible = false;
                }
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
                topNumber.Visible = false;
                topMiddle.Visible = false;
                topBottom.Visible = false;
                divtopitemcount.Visible = false;
                divbottomitemcount.Visible = false;
            }
        }

        /// <summary>
        /// Paging Data
        /// </summary>
        private void doPaging()
        {
            if (RepProduct.Visible == true)
            {
                grid_view.CssClass = "grid-click";
                list_view.CssClass = "list-view";
                grid_bottom.CssClass = "grid-click";
                list_bottom.CssClass = "list-view";
            }
            else if (Repeaterlistview.Visible == true)
            {
                grid_view.CssClass = "grid-view";
                list_view.CssClass = "list-click";
                list_bottom.CssClass = "list-click";
                grid_bottom.CssClass = "grid-view";

            }
            DataTable dt = new DataTable();

            //First Column store page index default it start from "0"
            //Second Column store page index default it start from "1"
            dt.Columns.Add("PageIndex");
            dt.Columns.Add("PageText");

            //Assign First Index starts from which number in paging data list
            findex = CurrentPage - 5;

            //Set Last index value if current page less than 5 then last index added "5" values to the Current page else it set "10" for last page number
            if ((CurrentPage >= 5) && (CurrentPage % 5 == 0))
            {
                lindex = CurrentPage + 5;
                ViewState["lindex"] = lindex;

                findex = CurrentPage;
                ViewState["findex"] = findex;
            }
            else if ((CurrentPage > 5) && (CurrentPage % 5 != 0))
            {
                if (ViewState["lindex"] != null && ViewState["findex"] != null)
                {
                    lindex = Convert.ToInt32(ViewState["lindex"]);
                    findex = Convert.ToInt32(ViewState["findex"]);
                    if (lindex > CurrentPage && findex < CurrentPage)
                    {
                    }
                    else
                    {
                        lindex = CurrentPage + 5;
                        ViewState["lindex"] = lindex;

                        findex = CurrentPage;
                        ViewState["findex"] = findex;
                    }
                }
            }
            else
            {
                lindex = 5;
            }

            //Check last page is greater than total page then reduced it to total no. of page is last index
            if (lindex > Convert.ToInt32(ViewState["totpage"]))
            {
                lindex = Convert.ToInt32(ViewState["totpage"]);
                findex = lindex - 5;
                ViewState["lindex"] = lindex;
                ViewState["findex"] = findex;
            }

            if (findex < 0)
            {
                findex = 0;
            }
            if (CurrentPage != 0 && (CurrentPage % 5) == 0)
            {

                for (int i = findex; i < lindex; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = i;
                    dr[1] = i + 1;
                    dt.Rows.Add(dr);
                }
            }
            else
            {
                //Now creating page number based on above first and last page index
                for (int i = findex; i < lindex; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = i;
                    dr[1] = i + 1;
                    dt.Rows.Add(dr);
                }
            }

            //Finally bind it page numbers in to the Paging DataList "RepeaterPaging"
            RepeaterPaging.DataSource = dt;
            RepeaterPaging.DataBind();

            dlToppaging.DataSource = dt;
            dlToppaging.DataBind();

        }

        /// <summary>
        /// Get Current Paging
        /// </summary>
        public int CurrentPage
        {
            get
            {   //Check view state is null if null then return current index value as "0" else return specific page viewstate value
                if (ViewState["CurrentPage"] == null)
                {
                    return 0;
                }
                else
                {
                    return ((int)ViewState["CurrentPage"]);
                }
            }
            set
            {
                //Set View statevalue when page is changed through Paging "RepeaterPaging" DataList
                ViewState["CurrentPage"] = value;
            }
        }

        /// <summary>
        /// Bind Subcategory from Main Category
        /// </summary>
        private void BindSubCategoryofMainCategory()
        {
            DataSet dsSubcategory = CategoryComponent.GetSubCategoryByCategoryId(Convert.ToInt32(Request.QueryString["CatID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsSubcategory != null && dsSubcategory.Tables.Count > 0 && dsSubcategory.Tables[0].Rows.Count > 0)
            {
                dscatchk = dsSubcategory;
                //RepSubCategory.DataSource = dsSubcategory;
                RepSubCategory.DataSource = null;
                dvSubcategory.Visible = false;
            }
            else
            {
                dvSubcategory.Visible = false;
                RepSubCategory.DataSource = null;
            }
            RepSubCategory.DataBind();
        }

        /// <summary>
        /// Bind SubCategory Name
        /// </summary>


        private void BindCategory()
        {
            DataSet dsCategory = new DataSet();
            dsCategory = CommonComponent.GetCommonDataSet("SELECT * FROM tb_Category WHERE StoreId=1 and Isnull(Active,0)=1 and isnull(deleted,0)=0 and name like '%swatch%'");
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0)
            {
                ddltopcategory.DataSource = dsCategory;
                ddltopcategory.DataTextField = "Name";
                ddltopcategory.DataValueField = "CategoryID";
                ddltopcategory.DataBind();

                ddlbottomcategory.DataSource = dsCategory;
                ddlbottomcategory.DataTextField = "Name";
                ddlbottomcategory.DataValueField = "CategoryID";
                ddlbottomcategory.DataBind();
            }
            else
            {
                ddlbottomcategory.DataSource = null;
                ddlbottomcategory.DataBind();

                ddlbottomcategory.DataSource = null;
                ddlbottomcategory.DataBind();
            }
            ddltopcategory.Items.Insert(0, new ListItem("All", "0"));
            ddlbottomcategory.Items.Insert(0, new ListItem("All", "0"));


        }
        private void BindSubCategoryNameDescription()
        {
            DataSet dsSubcategorydetail = objCategory.getCatdetailbycatid(Convert.ToInt32(Request.QueryString["CatID"]), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            try
            {

                String SETitle = "";
                String SEKeywords = "";
                String SEDescription = "";
                if (!string.IsNullOrEmpty(dsSubcategorydetail.Tables[0].Rows[0]["SETitle"].ToString()))
                {
                    SETitle = dsSubcategorydetail.Tables[0].Rows[0]["SETitle"].ToString();
                }
                else
                {
                    SETitle = AppLogic.AppConfigs("SiteSETitle").ToString();
                }

                if (!string.IsNullOrEmpty(dsSubcategorydetail.Tables[0].Rows[0]["SEKeywords"].ToString()))
                {
                    SEKeywords = dsSubcategorydetail.Tables[0].Rows[0]["SEKeywords"].ToString();
                }
                else
                {
                    SEKeywords = AppLogic.AppConfigs("SiteSEKeywords").ToString();
                }


                if (!string.IsNullOrEmpty(dsSubcategorydetail.Tables[0].Rows[0]["SEDescription"].ToString()))
                {
                    SEDescription = dsSubcategorydetail.Tables[0].Rows[0]["SEDescription"].ToString();
                }
                else
                {
                    SEDescription = AppLogic.AppConfigs("SiteSEDescription").ToString();
                }
                Master.HeadTitle(SETitle, SEKeywords, SEDescription);
            }
            catch { }
            if (dsSubcategorydetail != null && dsSubcategorydetail.Tables.Count > 0 && dsSubcategorydetail.Tables[0].Rows.Count > 0)
            {
                ltTitle.Text = Convert.ToString(dsSubcategorydetail.Tables[0].Rows[0]["Name"]);
                RptCategoryDescription.DataSource = dsSubcategorydetail;

                if (Convert.ToString(dsSubcategorydetail.Tables[0].Rows[0]["Description"]).Length > 0)
                    divCatBanner.Visible = true;
                else divCatBanner.Visible = false;
            }
            else
            {
                RptCategoryDescription.DataSource = null;
            }
            RptCategoryDescription.DataBind();
        }

        #region Get Image Name

        /// <summary>
        /// Get Category Image with Full Path
        /// </summary>
        /// <param name="img">Image Name</param>
        /// <returns>return Image with Full Path </returns>
        public String GetIconImageCategory(String img)
        {
            try
            {

                String imagepath = String.Empty;
                imagepath = AppLogic.AppConfigs("ImagePathCategory") + "Icon/" + img;

                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") + imagepath))
                {
                    return AppLogic.AppConfigs("Live_Contant_Server") + imagepath;
                }

                return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathCategory") + "Icon/image_not_available.jpg");
            }
            catch
            {
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

            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_path") +imagepath))
            {
                return AppLogic.AppConfigs("Live_Contant_Server") +  imagepath;
            }

            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }

        #endregion

        #region Subcategory, Product, Price dropdown, Paging Events

        /// <summary>
        /// SubCategory ItemDatabound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void RepSubCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            //{
            //    HtmlGenericControl Catbox = (HtmlGenericControl)e.Item.FindControl("Catbox");
            //    HtmlGenericControl catDisplay = (HtmlGenericControl)e.Item.FindControl("catDisplay");
            //    Literal litControl = (Literal)e.Item.FindControl("ltTop");
            //    if (e.Item.ItemIndex % columnCount == 0)
            //    {
            //        if (e.Item.ItemIndex != 0)
            //        {
            //            litControl.Text = "</div><div class=\"fp-bg\">";
            //        }
            //    }
            //    if ((counter % columnCount) == 0)
            //    {
            //        //CatDisplay.Attributes.Add("class", "cat-display-none");
            //        Catbox.Attributes.Add("class", "fp-pro-1");
            //    }
            //    counter += 1;
            //}
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Catbox = (HtmlGenericControl)e.Item.FindControl("Catbox");
                HtmlGenericControl catDisplay = (HtmlGenericControl)e.Item.FindControl("catDisplay");
                Literal litControl = (Literal)e.Item.FindControl("ltTop");

                if ((e.Item.ItemIndex + 1) % 6 == 0 && e.Item.ItemIndex != 0)
                {
                    //Catbox.Attributes.Add("style", "margin-right:0px;");
                    litControl.Text = "</ul><ul>";
                }
            }
        }

        /// <summary>
        /// Product Item Databound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void RepProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Probox = (HtmlGenericControl)e.Item.FindControl("Probox");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");

                Label lblTName = (Label)e.Item.FindControl("lblTName");
                Literal lblTag = (Literal)e.Item.FindControl("lblTag");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");
                HtmlInputHidden hdnItemIndex = (HtmlInputHidden)e.Item.FindControl("hdnItemIndex");
                HtmlInputHidden hdnswatchprice = (HtmlInputHidden)e.Item.FindControl("hdnswatchprice");

                Literal litControl = (Literal)e.Item.FindControl("ltTop");
                HtmlImage outofStockDiv = (HtmlImage)e.Item.FindControl("outofStockDiv");
                HtmlImage orderswatch = (HtmlImage)e.Item.FindControl("orderswatch");
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

                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                        {
                            // lblTag.Text = "<img width='41' height='42'  title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='new' />";
                            lblTag.Text = "<img title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='" + lblTName.Text.ToString().ToLower() + "' />";
                        }
                    }
                }
                if (ViewState["strFeaturedimg"] != null)
                {
                    //ViewState["strFeaturedimg"] = ViewState["strFeaturedimg"].ToString() + "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#" + imgName.ClientID.ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
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
                    // ViewState["strFeaturedimgpostback"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
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
                    //if (RecordCount == itemCount)
                    //{
                    //    litControl.Text = "</ul><ul><li style='width:251px !important;'>";
                    //}
                    //else
                    //{
                    //litControl.Text = "</ul><ul><li>";
                    //}
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

                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");

                HtmlAnchor aProductlink = (HtmlAnchor)e.Item.FindControl("aProductlink");
                HtmlAnchor aorderswatch = (HtmlAnchor)e.Item.FindControl("aorderswatch");
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
                    ltrRegularPrice.Text += "Swatch Price: <span>" + Price.ToString("C") + "</span>";// "<p>Regular Price: " + Price.ToString("C") + "</p>";
                    hdnprice = Price;
                }
                else ltrRegularPrice.Text += "<span>&nbsp;</span>";
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "Swatch Price: <span>" + SalePrice.ToString("C") + "</span>";// "<p>Your Price: <strong>" + SalePrice.ToString("C") + "</strong></p>";
                    hdnprice = SalePrice;
                }
                else ltrYourPrice.Text += "Swatch Price: <span>" + Price.ToString("C") + "</span>";

                //if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                //{

                //Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(dbo.tb_ProductVariant.VariantID) FROM  dbo.tb_ProductVariant INNER JOIN dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID WHERE dbo.tb_ProductVariant.Productid=" + ltrproductid.Text.ToString() + " "));
                //if (Count == 0)
                //{
                //    aProductlink.Attributes.Add("onclick", "adtocart('" + hdnprice.ToString() + "'," + ltrproductid.Text.ToString() + ");");
                //}
                //else
                //{
                //    aProductlink.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ");");
                //}

                string SwatchProductId = "";//Convert.ToString(CommonComponent.GetScalarCommonData("SElect ISNULL(ProductSwatchid,0) as ProductSwatchid from tb_Product where ProductID=" + ltrproductid.Text.ToString() + ""));
                if (!string.IsNullOrEmpty(SwatchProductId) && Convert.ToInt32(SwatchProductId) > 0)
                {
                    decimal Swatchprice = 0;
                    DataSet dsSwatchDetails = new DataSet();
                    dsSwatchDetails = CommonComponent.GetCommonDataSet("Select ISNULL(price,0)as Price,ISNULL(saleprice,0) saleprice from tb_Product where ProductID=" + SwatchProductId + "");
                    if (dsSwatchDetails != null && dsSwatchDetails.Tables.Count > 0 && dsSwatchDetails.Tables[0].Rows.Count > 0)
                    {
                        Decimal swatchprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["Price"]);
                        Decimal swatchsaleprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["salePrice"]);
                        if (swatchprice > decimal.Zero)
                        {
                            if (swatchsaleprice == decimal.Zero)
                            {
                                Swatchprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["price"].ToString());
                            }
                            else if (swatchprice > swatchsaleprice)
                            {
                                Swatchprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["saleprice"].ToString());
                            }
                            else { Swatchprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["price"].ToString()); }
                        }
                    }

                    hdnswatchprice.Value = Convert.ToString(Swatchprice.ToString());

                    //aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                    //aProductlink.Attributes.Remove("onclick");
                    //aProductlink.Visible = false;

                    aProductlink.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + SwatchProductId.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                    aProductlink.HRef = "javascript:void(0);";

                    innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                    imgAddToCart.Visible = true;
                    outofStockDiv.Visible = false;

                    aorderswatch.Visible = true;
                    aorderswatch.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + SwatchProductId.ToString() + ",'" + aorderswatch.ClientID + "'); return false;");
                    orderswatch.Visible = true;
                    //  imgAddToCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + "," + hdnItemIndex.Value.ToString() + "," + TotalCount.ToString() + ");");
                    imgAddToCart.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + SwatchProductId.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                }
                else
                {

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
                        //  innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        // imgAddToCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + "," + hdnItemIndex.Value.ToString() + "," + TotalCount.ToString() + ");");

                        imgAddToCart.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");

                        // innerAddtoCart.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                       // aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                        aProductlink.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                        aProductlink.HRef = "javascript:void(0);";
                        innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                    }
                    else
                    {
                        aProductlink.HRef = "javascript:void(0);";
                        //aProductlink.Attributes.Add("onclick", "InsertProductSubcategory(" + ltrproductid.Text.ToString() + ",'" + aProductlink.ClientID.ToString() + "');");
                        // innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        //innerAddtoCart.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";// "javascript:void(0);";
                        innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        // imgAddToCart.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + "," + hdnItemIndex.Value.ToString() + "," + TotalCount.ToString() + ");");
                        imgAddToCart.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                        aProductlink.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                        //aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                    }
                }
                //}
                //else
                //{

                //    //// aProductlink.HRef = "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //    //aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //    aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                //    aProductlink.Attributes.Remove("onclick");
                //    aProductlink.Visible = false;
                //    innerAddtoCart.HRef = "javascript:void(0);";
                //    //innerAddtoCart.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //    innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                //    imgAddToCart.Visible = false;
                //    outofStockDiv.Visible = true;

                //}

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

        /// <summary>
        /// Price dropdown(Bottom) Selected Index
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlbottomprice_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RepProduct.Visible == true)
            {
                grid_view.CssClass = "grid-click";
                list_view.CssClass = "list-view";
                grid_bottom.CssClass = "grid-click";
                list_bottom.CssClass = "list-view";
            }
            else if (Repeaterlistview.Visible == true)
            {
                grid_view.CssClass = "grid-view";
                list_view.CssClass = "list-click";
                list_bottom.CssClass = "list-click";
                grid_bottom.CssClass = "grid-view";

            }
            ddlTopPrice.SelectedValue = ddlbottomprice.SelectedValue;
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
            }
            if (ViewState["strFeaturedimglistpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
            }
        }

        /// <summary>
        /// Price dropdown(Top) Selected Index
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddltopcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RepProduct.Visible == true)
            {
                grid_view.CssClass = "grid-click";
                list_view.CssClass = "list-view";
                grid_bottom.CssClass = "grid-click";
                list_bottom.CssClass = "list-view";
            }
            else if (Repeaterlistview.Visible == true)
            {
                grid_view.CssClass = "grid-view";
                list_view.CssClass = "list-click";
                list_bottom.CssClass = "list-click";
                grid_bottom.CssClass = "grid-view";

            }
            ddlbottomcategory.SelectedValue = ddltopcategory.SelectedValue;
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
            }
            if (ViewState["strFeaturedimglistpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
            }
        }
        protected void ddlbottomcategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RepProduct.Visible == true)
            {
                grid_view.CssClass = "grid-click";
                list_view.CssClass = "list-view";
                grid_bottom.CssClass = "grid-click";
                list_bottom.CssClass = "list-view";
            }
            else if (Repeaterlistview.Visible == true)
            {
                grid_view.CssClass = "grid-view";
                list_view.CssClass = "list-click";
                list_bottom.CssClass = "list-click";
                grid_bottom.CssClass = "grid-view";

            }
            ddltopcategory.SelectedValue = ddlbottomcategory.SelectedValue;
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
            }
            if (ViewState["strFeaturedimglistpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
            }
        }


        protected void ddlTopPrice_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (RepProduct.Visible == true)
            {
                grid_view.CssClass = "grid-click";
                list_view.CssClass = "list-view";
                grid_bottom.CssClass = "grid-click";
                list_bottom.CssClass = "list-view";
            }
            else if (Repeaterlistview.Visible == true)
            {
                grid_view.CssClass = "grid-view";
                list_view.CssClass = "list-click";
                list_bottom.CssClass = "list-click";
                grid_bottom.CssClass = "grid-view";

            }
            ddlbottomprice.SelectedValue = ddlTopPrice.SelectedValue;
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
            }
            if (ViewState["strFeaturedimglistpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
            }
        }

        /// <summary>
        /// Paging Item Command
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DataListCommandEventArgs e</param>
        protected void RepeaterPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("newpage"))
            {
                itemCount = 6;
                //Assign CurrentPage number when user click on the page number in the Paging "RepeaterPaging" DataList
                CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
                //Refresh "Repeater1" control Data once user change page
                BindProductOfSubCategory();

            }
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
            }
            if (ViewState["strFeaturedimglist"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);
            }

        }

        /// <summary>
        /// If user Click First Link button assign current index as Zero "0" then refresh "Repeater1" Data.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            //If user click First Link button assign current index as Zero "0" then refresh "Repeater1" Data.
            CurrentPage = 0;
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
            }
            if (ViewState["strFeaturedimglist"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);
            }
        }

        /// <summary>
        /// If user Click Last Link button assign current index as totalpage then refresh "Repeater1" Data.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkLast_Click(object sender, EventArgs e)
        {
            //If user click Last Link button assign current index as totalpage then refresh "Repeater1" Data.
            CurrentPage = (Convert.ToInt32(ViewState["totpage"]) - 1);
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
            }
            if (ViewState["strFeaturedimglist"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);
            }
        }

        /// <summary>
        /// If user click Previous Link button assign current index as -1 it reduce existing page index.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            //If user click Previous Link button assign current index as -1 it reduce existing page index.

            CurrentPage -= 1;
            //refresh "Repeater1" Data
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
            }
            if (ViewState["strFeaturedimglist"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);
            }

        }

        /// <summary>
        /// If user click Next Link button assign current index as +1 it add one value to existing page index.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            //If user click Next Link button assign current index as +1 it add one value to existing page index.

            CurrentPage += 1;
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
            }
            if (ViewState["strFeaturedimglist"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);
            }


        }

        /// <summary>
        /// Repeater Paging
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void RepeaterPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //Enabled False for current selected Page index
            LinkButton lnkPage = (LinkButton)e.Item.FindControl("Pagingbtn");
            if (lnkPage.CommandArgument.ToString() == CurrentPage.ToString())
            {
                lnkPage.Enabled = false;
                //lnkPage.Attributes.Add("style", "background: none repeat scroll 0 0 #7D7D7D;color: #FFFFFF;");
                lnkPage.Attributes.Add("style", "color: #B92127;");
                lnkPage.OnClientClick = "return true;";
            }
            else
            {
                // lnkPage.Attributes.Add("style", "background: none repeat scroll 0 0 #FFFFFF;color: #5E5E5E;");
                lnkPage.OnClientClick = "pagingSelection(" + lnkPage.CommandArgument.ToString() + ");chkHeight();";
            }
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
            }
            if (ViewState["strFeaturedimglist"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);
            }
        }

        #endregion

        /// <summary>
        /// Set Name of Product or category
        /// </summary>
        /// <param name="Name"></param>
        /// <returns>Return Max. 62 Length of string</returns>
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

        #region Add Customer Temparory
        /// <summary>
        /// Add Customer for Cart item
        /// </summary>
        private void AddCustomer()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            Solution.Bussines.Entities.tb_Customer objCust = new Solution.Bussines.Entities.tb_Customer();
            Int32 CustID = -1;
            CustID = objCustomer.InsertCustomer(objCust, Convert.ToInt32(1));
            Session["CustID"] = CustID.ToString();
            System.Web.HttpCookie custCookie = new System.Web.HttpCookie("ecommcustomer", CustID.ToString());
            custCookie.Expires = DateTime.Now.AddYears(1);
            Response.Cookies.Add(custCookie);
        }
        #endregion

        /// <summary>
        /// AddToCart Button click event
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
                    hiddenCustID.Value = Session["CustID"].ToString();
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
        /// <returns>True or False</returns>
        private bool CheckCustomerIsRestricted()
        {
            bool IsRestrictedCust = false;
            IsRestrictedCust = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT RestrictedIPID FROM tb_RestrictedIP WHERE IPAddress='" + Request.UserHostAddress.ToString() + "'"));
            return IsRestrictedCust;
        }

        /// <summary>
        /// View All product button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkViewAll_Click(object sender, EventArgs e)
        {
            if (RepProduct.Visible == true)
            {
                grid_view.CssClass = "grid-click";
                list_view.CssClass = "list-view";
                grid_bottom.CssClass = "grid-click";
                list_bottom.CssClass = "list-view";
            }
            else if (Repeaterlistview.Visible == true)
            {
                grid_view.CssClass = "grid-view";
                list_view.CssClass = "list-click";
                list_bottom.CssClass = "list-click";
                grid_bottom.CssClass = "grid-view";

            }
            ViewState["All"] = "All";
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
            }
            if (ViewState["strFeaturedimglist"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);
            }
        }

        /// <summary>
        /// View page button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkViewAllPages_Click(object sender, EventArgs e)
        {
            if (RepProduct.Visible == true)
            {
                grid_view.CssClass = "grid-click";
                list_view.CssClass = "list-view";
                grid_bottom.CssClass = "grid-click";
                list_bottom.CssClass = "list-view";
            }
            else if (Repeaterlistview.Visible == true)
            {
                grid_view.CssClass = "grid-view";
                list_view.CssClass = "list-click";
                list_bottom.CssClass = "list-click";
                grid_bottom.CssClass = "grid-view";

            }
            ViewState["All"] = null;
            BindProductOfSubCategory();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", ViewState["strFeaturedimg"].ToString(), true);
            }
            if (ViewState["strFeaturedimglist"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlist", ViewState["strFeaturedimglist"].ToString(), true);
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

        public void GetCompareProduct()
        {
            if (!string.IsNullOrEmpty(Comparestr))
            {
                ltrProduct.Text = Comparestr;
            }
            else
            {
                ltrProduct.Text = "";
            }
        }

        private void AddProductData(string ProIDs)
        {
            if (Session["CmpProductID"] != null)
            {
                if (!Session["CmpProductID"].ToString().Contains(ProIDs.ToString().ToString() + ","))
                {
                    Session["CmpProductID"] += ProIDs.ToString() + ",";
                }
            }
            else
            {
                Session["CmpProductID"] += ProIDs.ToString() + ",";
            }
        }

        protected void rdoCreditCard_CheckedChanged(object sender, EventArgs e)
        {
            foreach (RepeaterItem row in RepProduct.Items)
            {
                CheckBox chk = (CheckBox)row.FindControl("add_to_compare");
                Literal hdnProductID = (Literal)row.FindControl("ltrproductid");
                if (chk.Checked)
                {
                    AddProductData(hdnProductID.Text);
                }
                else
                {
                    DeleteProductData(hdnProductID.Text);
                }
            }
            Getdata(Session["CmpProductID"].ToString());
            GetCompareProduct();
        }

        private void DeleteProductData(string ProIDs)
        {
            if (Session["CmpProductID"] != null)
            {
                if (Session["CmpProductID"].ToString().Contains(ProIDs.ToString().ToString() + ","))
                {
                    string newvar = "";
                    string proid = Session["CmpProductID"].ToString().TrimEnd(',');
                    string[] arr = proid.Split(',');
                    if (ProIDs != "")
                    {
                        for (int i = 0; i < arr.Length; i++)
                        {
                            if (arr[i] != ProIDs.ToString())
                            {
                                newvar += arr[i].ToString() + ",";
                            }
                        }
                        Session["CmpProductID"] = "";
                        Session["CmpProductID"] = newvar;
                        foreach (RepeaterItem row in RepProduct.Items)
                        {
                            CheckBox chk = (CheckBox)row.FindControl("add_to_compare");
                            Literal hdnProductID = (Literal)row.FindControl("ltrproductid");
                            if (ProIDs.ToString() == hdnProductID.Text.ToString())
                            {
                                chk.Checked = false;
                            }
                        }
                    }
                }
            }
        }

        public void Getdata(string ProIDs)
        {
            string proid = ProIDs.TrimEnd(',');
            string[] arr = proid.Split(',');
            DataSet dsproduct = new DataSet();
            if (ProIDs != "")
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].ToString() != "")
                    {
                        dsproduct = ProductComponent.GetProductDetailByID(Convert.ToInt32(arr[i].ToString()));
                        if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                        {
                            Comparestr += @"<div id='cb-" + (i + 1).ToString() + "' class='comparison-box'>";
                            Comparestr += "<div class='comparison-img'>";
                            Comparestr += "<img width='50' height='50' title='" + (dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "' alt='" + (dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "' src='" + GetIconImageProduct(dsproduct.Tables[0].Rows[0]["ImageName"].ToString()) + "'></div>";
                            Comparestr += "<div class='comparison-right'><h4>" + SetNameForCompare(dsproduct.Tables[0].Rows[0]["Name"].ToString()) + "</h4></div>";
                            Comparestr += "<div style='float:left; position:absolute; bottom:3px; right:3px; cursor:pointer;' onclick='DeleteProductData(" + (i + 1).ToString() + "," + arr[i].ToString() + ");' id='delete_compare'>";
                            Comparestr += "<img width='16' height='16' src='/images/delete_ico.png'>";
                            Comparestr += "</div>";
                            Comparestr += "</div>";
                        }
                    }
                }
                if (arr.Length >= 1)
                {
                    if (arr.Length > 1)
                    {
                        //btncompare.Visible = true;
                    }
                    else
                    {
                        //btncompare.Visible = false;
                    }
                    comare_div.Visible = true;
                }
                else
                {
                    comare_div.Visible = false;
                }
            }
            else
            {
                comare_div.Visible = false;
            }
        }

        protected void btnCompareID_Click(object sender, ImageClickEventArgs e)
        {
            DeleteProductData(hdnCompare.Value);
            Getdata(Session["CmpProductID"].ToString());
            GetCompareProduct();
        }

        public String SetNameForCompare(String Name)
        {
            if (Name.Length > 45)
                Name = Name.Substring(0, 42) + "...";
            return Server.HtmlEncode(Name);
        }

        public void gridchange(string ProIDs)
        {

            string proid = ProIDs.TrimEnd(',');
            string[] arr = proid.Split(',');
            DataSet dsproduct = new DataSet();
            if (ProIDs != "")
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].ToString() != "")
                    {
                        dsproduct = ProductComponent.GetProductDetailByID(Convert.ToInt32(arr[i].ToString()));
                        if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                        {

                            if (Session["CmpProductID"] != null)
                            {
                                if (Session["CmpProductID"].ToString().Contains(dsproduct.Tables[0].Rows[0]["ProductId"].ToString() + ","))
                                {
                                    strView = "if(document.getElementById('add_to_compare" + dsproduct.Tables[0].Rows[0]["ProductId"].ToString() + "') != null){document.getElementById('add_to_compare" + dsproduct.Tables[0].Rows[0]["ProductId"].ToString() + "').checked = true;}";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloadchecked" + i, strView, true);

                                }
                            }

                        }
                    }
                }
            }
        }
        public void listchange(string ProIDs)
        {
            string proid = ProIDs.TrimEnd(',');
            string[] arr = proid.Split(',');
            DataSet dsproduct = new DataSet();
            if (ProIDs != "")
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i].ToString() != "")
                    {
                        dsproduct = ProductComponent.GetProductDetailByID(Convert.ToInt32(arr[i].ToString()));
                        if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                        {

                            if (Session["CmpProductID"] != null)
                            {
                                if (Session["CmpProductID"].ToString().Contains(dsproduct.Tables[0].Rows[0]["ProductId"].ToString() + ","))
                                {
                                    strView = "if(document.getElementById('add_to_compare_list" + dsproduct.Tables[0].Rows[0]["ProductId"].ToString() + "') != null){document.getElementById('add_to_compare_list" + dsproduct.Tables[0].Rows[0]["ProductId"].ToString() + "').checked = true;}";
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloadchecked" + i + i, strView, true);

                                }
                            }
                        }
                    }
                }
            }
        }
        protected void grid_view_Click(object sender, EventArgs e)
        {

            grid_view.CssClass = "grid-click";
            list_view.CssClass = "list-view";
            grid_bottom.CssClass = "grid-click";
            list_bottom.CssClass = "list-view";
            RepProduct.Visible = true;
            Repeaterlistview.Visible = false;

            ScriptManager.RegisterStartupScript(this, this.GetType(), "msggcompare1", "compare(0,0);", true);
            if (Session["CmpProductID"] != null && Session["CmpProductID"].ToString() != "")
                gridchange(Session["CmpProductID"].ToString());
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
            }
            if (ViewState["strFeaturedimglistpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
            }
        }

        protected void list_view_Click(object sender, EventArgs e)
        {

            grid_view.CssClass = "grid-view";
            list_view.CssClass = "list-click";
            list_bottom.CssClass = "list-click";
            grid_bottom.CssClass = "grid-view";
            RepProduct.Visible = false;
            Repeaterlistview.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msggcompare2", "compare(0,0);", true);
            if (Session["CmpProductID"] != null && Session["CmpProductID"].ToString() != "")
                listchange(Session["CmpProductID"].ToString());

            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
            }
            if (ViewState["strFeaturedimglistpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
            }

        }

        protected void grid_bottom_Click(object sender, EventArgs e)
        {

            grid_bottom.CssClass = "grid-click";
            list_bottom.CssClass = "list-view";
            grid_view.CssClass = "grid-click";
            list_view.CssClass = "list-view";
            RepProduct.Visible = true;
            Repeaterlistview.Visible = false; Page.ClientScript.RegisterStartupScript(Page.GetType(), "msggcompare", "compare(0, 0);", true);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msggcompare3", "compare(0,0);", true);
            if (Session["CmpProductID"] != null && Session["CmpProductID"].ToString() != "")
                gridchange(Session["CmpProductID"].ToString());

            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
            }
            if (ViewState["strFeaturedimglistpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
            }
        }

        protected void list_bottom_Click(object sender, EventArgs e)
        {

            grid_bottom.CssClass = "grid-view";
            list_bottom.CssClass = "list-click";
            list_view.CssClass = "list-click";
            grid_view.CssClass = "grid-view";
            RepProduct.Visible = false;
            Repeaterlistview.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "msggcompare4", "compare(0,0);", true);
            if (Session["CmpProductID"] != null && Session["CmpProductID"].ToString() != "")
                listchange(Session["CmpProductID"].ToString());

            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString() + "});", true);
            }
            if (ViewState["strFeaturedimglistpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderlistpost", "$(document).ready(function () {" + ViewState["strFeaturedimglistpostback"].ToString() + "});", true);
            }
        }

        protected void RepProduct_List_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Probox = (HtmlGenericControl)e.Item.FindControl("Probox");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");
                HtmlInputHidden hdnswatchprice = (HtmlInputHidden)e.Item.FindControl("hdnswatchprice");
                HtmlImage orderswatch = (HtmlImage)e.Item.FindControl("orderswatch");
                Label lblTName = (Label)e.Item.FindControl("lblTName");
                Literal lblTag = (Literal)e.Item.FindControl("lblTag");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");
                HtmlInputHidden hdnItemIndex = (HtmlInputHidden)e.Item.FindControl("hdnItemIndex");
                Literal litControl = (Literal)e.Item.FindControl("ltTop");
                HtmlAnchor aorderswatch = (HtmlAnchor)e.Item.FindControl("aorderswatch");
                HtmlAnchor innerAddtoCart = (HtmlAnchor)e.Item.FindControl("innerAddtoCart");
                HtmlImage imgAddToCart = (HtmlImage)e.Item.FindControl("imgAddToCart");
                Image imgName = (Image)e.Item.FindControl("imgName");
                #region Bind rating
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                DataSet dsComment = new DataSet();
                dsComment = ProductComponent.GetProductRating(Convert.ToInt32(ltrproductid.Text));
                Literal ltreviewDetail = (Literal)e.Item.FindControl("ltrating");
                Decimal rating = 0;
                HtmlGenericControl DivratingNew = (HtmlGenericControl)e.Item.FindControl("Divratinglist");
                Label ltrRatingCount = (Label)e.Item.FindControl("ltrRatingCount");


                decimal dd = 0;
                decimal ddnew = 0;

                if (dsComment != null && dsComment.Tables[0].Rows.Count > 0)
                {

                    rating = Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]);
                    ltrRatingCount.Text = dsComment.Tables[0].Rows[0]["AvgRating"].ToString();
                    DivratingNew.Visible = true;

                    dd = Math.Truncate(Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]));
                    ddnew = Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]) - dd;
                }
                else
                {
                    DivratingNew.Visible = false;

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

                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                        {
                            // lblTag.Text = "<img width='41' height='42'  title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='new'  style='padding-top: 10px;'/>";
                            lblTag.Text = "<img title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='" + lblTName.Text.ToString().ToLower() + "' />";
                        }
                    }
                }
                if (e.Item.ItemIndex % 5 == 0)
                {
                    if (e.Item.ItemIndex != 0)
                    {
                        litControl.Text = "</div><div class=\"fp-list-bg \">";
                    }
                }
                if (ViewState["strFeaturedimglist"] != null)
                {
                    ViewState["strFeaturedimglist"] = ViewState["strFeaturedimglist"].ToString() + "$('#loader_imglist" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#" + imgName.ClientID.ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').load(function () {$('#loader_imglist" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                }
                else
                {
                    ViewState["strFeaturedimglist"] = "$('#loader_imglist" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#" + imgName.ClientID.ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').load(function () {$('#loader_imglist" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#" + imgName.ClientID.ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                }
                if (ViewState["strFeaturedimglistpostback"] != null)
                {
                    ViewState["strFeaturedimglistpostback"] = ViewState["strFeaturedimglistpostback"].ToString() + "$('#loader_imglist" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                else
                {
                    ViewState["strFeaturedimglistpostback"] = "$('#loader_imglist" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                if ((e.Item.ItemIndex + 1) % 5 == 0 && e.Item.ItemIndex != 0)
                {
                    Probox.Attributes.Add("class", "fp-box-list");
                    //proDisplay.Attributes.Add("class", "pro-display-none");
                }

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrlink1");
                HtmlInputHidden ltrProductURL = (HtmlInputHidden)e.Item.FindControl("ltrProductURL");

                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");
                //Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                HtmlAnchor aProductlink = (HtmlAnchor)e.Item.FindControl("aProductlink");
                // HtmlAnchor aoutofstock = (HtmlAnchor)e.Item.FindControl("aoutofstock");
                HtmlImage aoutofstock = (HtmlImage)e.Item.FindControl("aoutofstock");
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
                //        ltrRegularPrice.Text += " <p>Regular Price:<del>" + Price.ToString("C") + "</del></p>";
                //        hdnprice = Price;
                //    }
                //    else
                //    {
                //        ltrRegularPrice.Text += "<p style='width:98px;line-height:18px'>Price:<span>" + Price.ToString("C") + "</span></p>";
                //        hdnprice = Price;
                //    }
                //}
                //else ltrRegularPrice.Text += "<p>&nbsp;</p>";
                //if (SalePrice > decimal.Zero && Price > SalePrice)
                //{
                //    ltrYourPrice.Text += "<p>Sale Price:<span>" + SalePrice.ToString("C") + "</span></p>";
                //    hdnprice = SalePrice;
                //}
                //else ltrYourPrice.Text += "<p><span>&nbsp;</span></p>";

                if (Price > decimal.Zero)
                {
                    ltrRegularPrice.Text += "Swatch Price: <span>" + Price.ToString("C") + "</span>";// "<p>Regular Price: " + Price.ToString("C") + "</p>";
                    hdnprice = Price;
                }
                else ltrRegularPrice.Text += "<span>&nbsp;</span>";
                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text += "Swatch Price: <span>" + SalePrice.ToString("C") + "</span>";// "<p>Your Price: <strong>" + SalePrice.ToString("C") + "</strong></p>";
                    hdnprice = SalePrice;
                }
                else ltrYourPrice.Text += "Swatch Price: <span>" + Price.ToString("C") + "</span>";


                //if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                //{
                string SwatchProductId = "";// Convert.ToString(CommonComponent.GetScalarCommonData("SElect ISNULL(ProductSwatchid,0) as ProductSwatchid from tb_Product where ProductID=" + ltrproductid.Text.ToString() + ""));
                if (!string.IsNullOrEmpty(SwatchProductId) && Convert.ToInt32(SwatchProductId) > 0)
                {
                    decimal Swatchprice = 0;
                    DataSet dsSwatchDetails = new DataSet();
                    dsSwatchDetails = CommonComponent.GetCommonDataSet("Select ISNULL(price,0)as Price,ISNULL(saleprice,0) saleprice from tb_Product where ProductID=" + SwatchProductId + "");
                    if (dsSwatchDetails != null && dsSwatchDetails.Tables.Count > 0 && dsSwatchDetails.Tables[0].Rows.Count > 0)
                    {
                        Decimal swatchprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["Price"]);
                        Decimal swatchsaleprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["salePrice"]);
                        if (swatchprice > decimal.Zero)
                        {
                            if (swatchsaleprice == decimal.Zero)
                            {
                                Swatchprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["price"].ToString());
                            }
                            else if (swatchprice > swatchsaleprice)
                            {
                                Swatchprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["saleprice"].ToString());
                            }
                            else { Swatchprice = Convert.ToDecimal(dsSwatchDetails.Tables[0].Rows[0]["price"].ToString()); }
                        }
                    }

                    hdnswatchprice.Value = Convert.ToString(Swatchprice.ToString());

                  //  aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                  //  aProductlink.Attributes.Remove("onclick");
                   // aProductlink.Visible = false;
                    aProductlink.HRef = "javascript:void(0);";
                    aProductlink.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                    innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();

                    imgAddToCart.Visible = true;
                    aoutofstock.Visible = false;

                    aorderswatch.Visible = true;
                    aorderswatch.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + SwatchProductId.ToString() + ",'" + aorderswatch.ClientID + "'); return false;");
                    orderswatch.Visible = true;
                    imgAddToCart.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                }
                else
                {
                    DataSet dsVariant = new DataSet();
                    dsVariant = CommonComponent.GetCommonDataSet("SELECT VariantID FROM tb_ProductVariant WHERE ProductID=" + ltrproductid.Text.ToString() + "");
                    if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                    {
                        ////aProductlink.Attributes.Remove("onclick");
                        ////// aProductlink.HRef = "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                        ////aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                        //aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                        innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        imgAddToCart.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                        //aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                        aProductlink.HRef = "javascript:void(0);";
                        aProductlink.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                        //innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();

                    }
                    else
                    {
                        ////aProductlink.HRef = "javascript:void(0);";
                        ////aProductlink.Attributes.Add("onclick", "InsertProductSubcategory(" + ltrproductid.Text.ToString() + ",'" + aProductlink.ClientID.ToString() + "');");
                        //aProductlink.HRef = "javascript:void(0);";
                        //aProductlink.Attributes.Add("onclick", "InsertProductSubcategory(" + ltrproductid.Text.ToString() + ",'" + aProductlink.ClientID.ToString() + "');");
                        innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                        imgAddToCart.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                        //aProductlink.HRef = "javascript:void(0);";
                        // aProductlink.Attributes.Add("onclick", "InsertProductSubcategory(" + ltrproductid.Text.ToString() + "," + hdnItemIndex.Value.ToString() + "," + TotalCount.ToString() + ");");
                        //innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                       // aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                        aProductlink.HRef = "javascript:void(0);";
                        aProductlink.Attributes.Add("onclick", "InsertProductOrderSwatchAddtocartfree(" + ltrproductid.Text.ToString() + ",'" + imgAddToCart.ClientID + "'); return false;");
                    }
                }
                //}
                //else
                //{

                //    ////// aProductlink.HRef = "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //    ////aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //    ////aProductlink.Attributes.Remove("onclick");
                //    ////aProductlink.Visible = false;
                //    //aProductlink.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //    aProductlink.HRef = "/" + ltrProductURL.Value.ToString();
                //    aProductlink.Attributes.Remove("onclick");
                //    aProductlink.Visible = false;
                //    innerAddtoCart.HRef = "javascript:void(0);";
                //    //innerAddtoCart.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                //    innerAddtoCart.HRef = "/" + ltrProductURL.Value.ToString();
                //    imgAddToCart.Visible = false;
                //    aoutofstock.Visible = true;


                //}
                #region compare
                if (Session["CmpProductID"] != null)
                {
                    if (Session["CmpProductID"].ToString().Contains(ltrproductid.Text.ToString() + ","))
                    {
                        strView = "if(document.getElementById('add_to_compare_grid" + ltrproductid.Text.ToString() + "') != null){document.getElementById('add_to_compare_grid" + ltrproductid.Text.ToString() + "').checked = true;}";
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloadchecked" + e.Item.ItemIndex.ToString(), strView, true);
                        //chkcompare.Checked = true;
                    }
                }

                #endregion
            }
        }

        //private void BindPattern()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType=2 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order By isnull(Displayorder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel3\" class=\"jcarousel-skin-tango2\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro\">";
        //            }
        //            if (i % 8 == 0 && i > 7)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro\">";
        //                }
        //            }

        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            StrPattern += "<li class=\"pattern-pro-box\">";
        //            StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkPattern_" + SearchId + "\">";
        //            StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrPattern.Text = StrPattern.ToString();
        //}

        //private void BindFabric()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 3 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order By isnull(Displayorder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel4\" class=\"jcarousel-skin-tango2\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro\">";
        //            }
        //            if (i % 8 == 0 && i > 7)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro\">";
        //                }
        //            }

        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            StrPattern += "<li class=\"pattern-pro-box\">";
        //            StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkFabric_" + SearchId + "\">";
        //            StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrFabric.Text = StrPattern.ToString();
        //}

        //private void BindStyle()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 4 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order By isnull(Displayorder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel5\" class=\"jcarousel-skin-tango2\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            if (i == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro\">";
        //            }
        //            if (i % 8 == 0 && i > 7)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro\">";
        //                }
        //            }

        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            StrPattern += "<li class=\"pattern-pro-box\">";
        //            StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkStyle_" + SearchId + "\">";
        //            StrPattern += "<span>" + SearchValue.ToString() + "</span></li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrStyle.Text = StrPattern.ToString();
        //}

        //private void BindHeader()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 5 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel7\" class=\"jcarousel-skin-tango2\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        Int32 icheck = 0;
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

        //            string strImageName = "";
        //            String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
        //            if (!Directory.Exists(Server.MapPath(SearchProductPath)))
        //                Directory.CreateDirectory(Server.MapPath(SearchProductPath));
        //            string strFilePath = Server.MapPath(SearchProductPath + Imagename);
        //            Random rnd = new Random();
        //            if (File.Exists(strFilePath))
        //            {
        //                strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
        //            }
        //            else
        //            {
        //                dsPattern.Tables[0].Rows.RemoveAt(i);
        //                i--;
        //                dsPattern.Tables[0].AcceptChanges();
        //                continue;
        //            }
        //            if (icheck == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro\">";
        //            }
        //            if (icheck % 3 == 0 && icheck > 2)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro\">";
        //                }
        //            }
        //            icheck++;
        //            StrPattern += "<li class=\"header-pro-box\">";
        //            StrPattern += "<input type=\"checkbox\" class=\"checkbox\" onclick=\"CheckSelection();\" value=\"" + SearchValue.ToString() + "\" name=\"chkHeader_" + SearchId + "\">";
        //            StrPattern += "<img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString() + "\"><span style=\"padding-left: 16px;\">" + SearchValue.ToString() + "</span></li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrHeader.Text = StrPattern.ToString();
        //}

        //private void BindColors()
        //{
        //    string StrPattern = "";
        //    DataSet dsPattern = new DataSet();
        //    dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType =1 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0  Order by ISNULL(DisplayOrder,999)");
        //    StrPattern = "<div class=\"toggle1\">";
        //    StrPattern += "<ul id=\"mycarousel2\" class=\"jcarousel-skin-tango0\">";
        //    if (dsPattern != null && dsPattern.Tables.Count > 0 && dsPattern.Tables[0].Rows.Count > 0)
        //    {
        //        Int32 icheck = 0;
        //        for (int i = 0; i < dsPattern.Tables[0].Rows.Count; i++)
        //        {
        //            string SearchValue = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchValue"]);
        //            string SearchId = Convert.ToString(dsPattern.Tables[0].Rows[i]["SearchId"]);
        //            string Imagename = Convert.ToString(dsPattern.Tables[0].Rows[i]["Imagename"]);

        //            string strImageName = "";
        //            String SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
        //            if (!Directory.Exists(Server.MapPath(SearchProductPath)))
        //                Directory.CreateDirectory(Server.MapPath(SearchProductPath));
        //            string strFilePath = Server.MapPath(SearchProductPath + Imagename);
        //            Random rnd = new Random();
        //            if (File.Exists(strFilePath))
        //            {
        //                strImageName = SearchProductPath + Imagename + "?" + rnd.Next(10000);
        //            }
        //            else
        //            {
        //                dsPattern.Tables[0].Rows.RemoveAt(i);
        //                i--;
        //                dsPattern.Tables[0].AcceptChanges();
        //                continue;
        //            }
        //            if (icheck == 0)
        //            {
        //                StrPattern += "<li><ul class=\"option-pro-color\" style=\"width:92px !important;\">";
        //            }
        //            if (icheck % 10 == 0 && icheck > 9)
        //            {
        //                if (dsPattern.Tables[0].Rows.Count - 1 > i)
        //                {
        //                    StrPattern += "</ul></li><li><ul class=\"option-pro-color\" style=\"width:92px !important;\">";
        //                }
        //            }

        //            icheck++;
        //            StrPattern += "<li class=\"option-pro-box\" style=\"padding-bottom:4px !important;\">";
        //            StrPattern += "<a href=\"javascript:void(0);\" onclick=\"ColorSelection('" + SearchValue.ToString() + "');\"><img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString() + "\"></a> </li>";
        //        }
        //        StrPattern += "</ul></li>";
        //    }
        //    StrPattern += "</ul>";
        //    StrPattern += "</div>";
        //    ltrColor.Text = StrPattern.ToString();
        //}

        protected void btnIndexPriceGo_Click(object sender, ImageClickEventArgs e)
        {
            //Session["IndexPriceValue"] = null;
            //Session["IndexFabricValue"] = null;
            //Session["IndexPatternValue"] = null;
            //Session["IndexStyleValue"] = null;
            //Session["IndexColorValue"] = null;
            //Session["IndexHeaderValue"] = null;
            //Session["IndexCustomValue"] = null;

            //if (!string.IsNullOrEmpty(txtFrom.Text.ToString().Trim()) || !string.IsNullOrEmpty(txtTo.Text.ToString().Trim()))
            //{
            //    if (string.IsNullOrEmpty(txtFrom.Text.ToString().Trim()))
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Please Enter Valid Price.');document.getElementById('ContentPlaceHolder1_txtFrom').focus();", true);
            //        return;
            //    }
            //    if (string.IsNullOrEmpty(txtTo.Text.ToString().Trim()))
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Tovalcal", "alert('Please Enter Valid Price.');document.getElementById('ContentPlaceHolder1_txtTo').focus();", true);
            //        return;
            //    }
            //    decimal FromVal = Convert.ToDecimal(txtFrom.Text.Trim());
            //    decimal ToVal = Convert.ToDecimal(txtTo.Text.Trim());
            //    if (FromVal > ToVal)
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Low Price should be Less than High Price.');document.getElementById('ContentPlaceHolder1_txtFrom').focus();", true);
            //        return;
            //    }
            //    Session["IndexPriceValue"] = FromVal.ToString() + "-" + ToVal.ToString();
            //}

            //string[] strkey = Request.Form.AllKeys;

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

            //if (!string.IsNullOrEmpty(hdnColorSelection.Value) && hdnColorSelection.Value != "")
            //{
            //    Session["IndexColorValue"] = hdnColorSelection.Value.ToString();
            //}
            //hdnColorSelection.Value = "";

            //if (Session["IndexPriceValue"] == null && Session["IndexFabricValue"] == null && Session["IndexPatternValue"] == null && Session["IndexStyleValue"] == null && Session["IndexColorValue"] == null && Session["IndexHeaderValue"] == null && Session["IndexCustomValue"] == null)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "fromvalcal", "alert('Please Select Search Criteria.');", true);
            //    return;
            //}
            //else
            //{
            //    if (Request.QueryString["CatID"] != null && Convert.ToString(Request.QueryString["CatID"]).Trim() != "" && Convert.ToString(Request.QueryString["CatID"]).Trim() != "0")
            //    {
            //        if (Request.QueryString["CatPID"] != null && Convert.ToString(Request.QueryString["CatPID"]).Trim() != "")
            //        {
            //            Response.Redirect("/ProductSearchList.aspx?CatID=" + Request.QueryString["CatID"].ToString().Trim() + "&CatPID=" + Request.QueryString["CatPID"].ToString() + "");
            //        }
            //        else
            //        {
            //            Response.Redirect("/ProductSearchList.aspx?CatID=" + Request.QueryString["CatID"].ToString().Trim() + "");
            //        }
            //    }
            //    else
            //    {
            //        Response.Redirect("/ProductSearchList.aspx");
            //    }
            //}
        }
        protected void btnIndexPriceGo1_Click(object sender, ImageClickEventArgs e)
        {
            //Session["IndexPriceValue"] = null;
            //Session["IndexFabricValue"] = null;
            //Session["IndexPatternValue"] = null;
            //Session["IndexStyleValue"] = null;
            //Session["IndexColorValue"] = null;
            //Session["IndexHeaderValue"] = null;
            //Session["IndexCustomValue"] = null;

            //string[] strkey = Request.Form.AllKeys;

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

            //if (!string.IsNullOrEmpty(hdnColorSelection.Value) && hdnColorSelection.Value != "")
            //{
            //    Session["IndexColorValue"] = hdnColorSelection.Value.ToString();
            //}
            //hdnColorSelection.Value = "";

            //if (Session["IndexPriceValue"] == null && Session["IndexFabricValue"] == null && Session["IndexPatternValue"] == null && Session["IndexStyleValue"] == null && Session["IndexColorValue"] == null && Session["IndexHeaderValue"] == null && Session["IndexCustomValue"] == null)
            //{
            //    Response.Redirect("/ProductSearchList.aspx");
            //}
            //else
            //{
            //    if (Request.QueryString["CatID"] != null && Convert.ToString(Request.QueryString["CatID"]).Trim() != "" && Convert.ToString(Request.QueryString["CatID"]).Trim() != "0")
            //    {
            //        if (Request.QueryString["CatPID"] != null && Convert.ToString(Request.QueryString["CatPID"]).Trim() != "")
            //        {
            //            Response.Redirect("/ProductSearchList.aspx?CatID=" + Request.QueryString["CatID"].ToString().Trim() + "&CatPID=" + Request.QueryString["CatPID"].ToString() + "");
            //        }
            //        else
            //        {
            //            Response.Redirect("/ProductSearchList.aspx?CatID=" + Request.QueryString["CatID"].ToString().Trim() + "");
            //        }
            //    }
            //    else
            //    {
            //        Response.Redirect("/ProductSearchList.aspx");
            //    }
            //}
        }
        protected void btntempclick_Click(object sender, EventArgs e)
        {

        }
    }
}