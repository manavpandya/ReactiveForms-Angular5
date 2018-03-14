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
    public partial class QuickView : System.Web.UI.Page
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


            if (!IsPostBack)
            {


                if (Request.QueryString["PID"] != null)
                {
                    Session["HeaderCatid"] = null;
                    Session["HeaderSubCatid"] = null;
                    btnAddTocartSwatch.OnClientClick = "InsertProductSwatchQuick(" + Request.QueryString["PID"].ToString() + ",'" + btnAddTocartSwatch.ClientID.ToString() + "'); return false;";
                    btnAddTocartMade.OnClientClick = "InsertProductCustomQuick(" + Request.QueryString["PID"].ToString() + ",'" + btnAddTocartMade.ClientID.ToString() + "'); return false;";

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


                        }
                        try
                        {
                            ProductComponent objRecer = new ProductComponent();
                            objRecer.UpdateProductForView(Productid);
                            GetProductDetails(Productid);
                            GetnextPreviousProduct();
                            BindCartDetails();

                            FillQuantityDiscountTable(Productid.ToString(), Convert.ToString(Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));

                            BindProductProperty();
                            BindProductQuote();

                            GetMakeOrderWidth();
                            GetMakeOrderStyle();
                            GetMakeOrderLength();
                            GetMakeOrderOptions();
                            GetMakeOrderQuantity();
                        }
                        catch
                        {
                        }
                    }
                    else
                    {
                        Response.Redirect("/index.aspx");
                    }

                    if (Session["HeaderCatid"] == null || Session["HeaderCatid"].ToString() == "" || Session["HeaderCatid"].ToString() == "0")
                    { //GetparentCategory();
                    }

                }
                else
                {
                    Response.Redirect("/index.aspx");
                }
                //breadcrumbs();
                ShowHidePortions();


            }
            else
            {


                if (ViewState["VariantNameId"] != null && ViewState["VariantNameId"].ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "dropdwnselect", "SelectVariantBypostback('" + ViewState["VariantNameId"].ToString() + "','" + ViewState["VariantValueId"].ToString() + "');", true);

                }
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tadisplay", "window.parent.document.getElementById('prepagequick').style.display = 'none';", true);

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
        }
        private void GetOption()
        {
            DataSet dsstyle = new DataSet();
            dsstyle = CommonComponent.GetCommonDataSet(@"SELECT case when isnull(tb_ProductOptionsPrice.AdditionalPrice,0)=0 then tb_ProductOptionsPrice.Options else tb_ProductOptionsPrice.Options+'($'+cast(Round(tb_ProductOptionsPrice.AdditionalPrice,2) as varchar(10))+')'  end as name, tb_ProductOptionsPrice.ProductId, tb_ProductSearchType.DisplayOrder
FROM dbo.tb_ProductOptionsPrice INNER JOIN dbo.tb_ProductSearchType ON dbo.tb_ProductOptionsPrice.Options = dbo.tb_ProductSearchType.SearchValue WHERE tb_ProductOptionsPrice.ProductId=" + Request.QueryString["PID"].ToString() + @" Order By tb_ProductSearchType.DisplayOrder");

            if (dsstyle != null && dsstyle.Tables.Count > 0 && dsstyle.Tables[0].Rows.Count > 0)
            {
                ddlcustomoptin.Items.Clear();
                ddlcustomoptin.DataSource = dsstyle;
                ddlcustomoptin.DataTextField = "name";
                ddlcustomoptin.DataValueField = "name";

                ddlcustomoptin.DataBind();
                ddlcustomoptin.Items.Insert(0, new ListItem("Options", "0"));
            }
            else
            {
                //ddlcustomoptin.DataSource = null;
                //ddlcustomoptin.DataBind();
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


            #region Hide More Images
            if (Convert.ToString(AppLogic.AppConfigs("SwitchItemViewMoreImages")).Trim().ToLower() == "false")
            {
                hideMoreImages.Visible = false;
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
                        ltBreadcrmbs.Text = ConfigurationComponent.GetBreadCrum(Convert.ToInt32(dsCommon.Tables[0].Rows[0]["CategoryID"].ToString()), Convert.ToInt32(dsCommon.Tables[0].Rows[0]["ParentCategoryID"].ToString()), 1, "", 3, 0);
                    }
                }
            }


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
                StrSEName = CommonOperations.RemoveSpecialCharacter(StrReplace.Trim().ToLower().Replace("&#32;", " ").ToCharArray());
                ltBreadcrmbs.Text = StartPath + " <a href=" + strSeNameJoin + "/" + StrSEName + "' title='" + StrReplace + "'>" + StrReplace + "</a>";
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
                bool IsCustomProductId = false;
                bool.TryParse(dsproduct.Tables[0].Rows[0]["iscustom"].ToString(), out IsCustomProductId);

                BindFabricSwatchBanner(Convert.ToString(dsproduct.Tables[0].Rows[0]["SKU"]));
                licustom.Attributes.Remove("onclick");
                licustom.Attributes.Add("onclick", "javascript:Clearsess('/" + dsproduct.Tables[0].Rows[0]["productUrl"].ToString() + "'," + dsproduct.Tables[0].Rows[0]["ProductID"].ToString() + ");");
                Int32 Swatchid = 0;
                bool isSwatch = Int32.TryParse(dsproduct.Tables[0].Rows[0]["ProductSwatchid"].ToString(), out Swatchid);
                if (Swatchid <= 0)
                {
                    divswatch.Visible = false;
                    liswatch.Visible = false;
                }
                else
                {
                    if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Ismadetoorderswatch"].ToString()) && Convert.ToBoolean(dsproduct.Tables[0].Rows[0]["Ismadetoorderswatch"].ToString()))
                    {

                        liswatch.Visible = true;
                    }
                    else
                    {
                        liswatch.Visible = false;
                    }
                    DataSet DataSwatch = new DataSet();
                    Solution.Data.SQLAccess objSql = new Solution.Data.SQLAccess();
                    DataSwatch = objSql.GetDs("SELECT ISNULL(SalePriceTag,'') as SalePriceTag,Sename, isnull(salePrice,0) as salePrice,isnull(Price,0) as Price,name,Sename,ImageName,SwatchDescription,MainCategory,isnull(Inventory,0) as Inventory,isnull(ProductUrl,'') as ProductUrl FROM tb_product WHERE productId=" + Swatchid + "");

                    if (DataSwatch != null && DataSwatch.Tables.Count > 0 && DataSwatch.Tables[0].Rows.Count > 0)
                    {
                        btnAddTocartSwatch.OnClientClick = "InsertProductSwatchQuick(" + Swatchid.ToString() + ",'" + btnAddTocartSwatch.ClientID.ToString() + "'); return false;";
                        liswatch.Attributes.Remove("onclick");
                        liswatch.Attributes.Add("onclick", "InsertProductSwatchQuick(" + Swatchid.ToString() + ",'" + btnAddTocartSwatch.ClientID.ToString() + "');");

                        if (Convert.ToInt32(DataSwatch.Tables[0].Rows[0]["Inventory"].ToString()) > 0)
                        {
                            Decimal swatchprice = Convert.ToDecimal(DataSwatch.Tables[0].Rows[0]["Price"]);
                            Decimal swatchsaleprice = Convert.ToDecimal(DataSwatch.Tables[0].Rows[0]["salePrice"]);
                            ltOrderswatch.Text = Convert.ToString(DataSwatch.Tables[0].Rows[0]["SwatchDescription"].ToString());
                            //aswatchurl.HRef = "/" + Convert.ToString(DataSwatch.Tables[0].Rows[0]["ProductUrl"].ToString());
                            aswatchurl.HRef = "javascript:void(0);";
                            aswatchurl.Attributes.Add("onclick", "javascript:window.parent.location.href='/" + Convert.ToString(DataSwatch.Tables[0].Rows[0]["ProductUrl"].ToString()) + "';");

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
                                    if (File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "micro/" + DataSwatch.Tables[0].Rows[0]["Imagename"].ToString())))
                                    {
                                        imgswatchproduct.Src = AppLogic.AppConfigs("ImagePathProduct") + "micro/" + DataSwatch.Tables[0].Rows[0]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgswatchproduct.Src = AppLogic.AppConfigs("ImagePathProduct") + "micro/image_not_available.jpg?" + rd1.Next(10000).ToString();
                                    }
                                }
                                else
                                {
                                    imgswatchproduct.Src = AppLogic.AppConfigs("ImagePathProduct") + "micro/image_not_available.jpg?" + rd1.Next(10000).ToString();
                                }

                            }
                        }
                        else
                        {
                            divswatch.Visible = false;
                            liswatch.Visible = false;
                        }
                    }
                }

                ltrProductName.Text = Convert.ToString(dsproduct.Tables[0].Rows[0]["name"].ToString());

                try
                {
                    GetOptinalProductDetails(Convert.ToString(dsproduct.Tables[0].Rows[0]["OptionalAccessories"]));
                }
                catch { }

                // Start - For Fill Secondary Color Option

                string SalePriceTag = "";
                if (!string.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["SalePriceTag"].ToString()))
                {
                    SalePriceTag = Convert.ToString(dsproduct.Tables[0].Rows[0]["SalePriceTag"].ToString());
                    hdnSalePriceTag.Value = "(" + Convert.ToString(dsproduct.Tables[0].Rows[0]["SalePriceTag"].ToString()) + ")";
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
                            //strResult += "<li><a href=\"javascript:void(0);\"><img src=\"" + strImagePath.Trim() + "\" alt=\"\" title=\"" + Convert.ToString(dsColor.Tables[0].Rows[i]["ColorName"]) + "\" style=\"border;solid 1px #dddddd;\"></a></li>";

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
                                            strhref = "\" onclick=\"javascript:window.parent.location.href='/" + strhref + "';";
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
                        divColorOption.Visible = true;
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
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                        else
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>";
                        hdncustomprice.Value = price.ToString();
                        hdnpricecustomcart.Value = price.ToString();
                    }
                    else if (price > salePrice)
                    {
                        //ltRegularPrice.Text = "<div id=\"spnRegularPrice\" class='item-right-pt1-right' style=\"Color:#484848;font-size: 12px;float:left; font-weight: normal;\">" + " : " + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</div>";
                        ltRegularPrice.Text = "<tt>MSRP :</tt> <span>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</span>";
                        if (!string.IsNullOrEmpty(hdnSalePriceTag.Value.ToString()) && hdnSalePriceTag.Value.ToString() != "")
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + salePrice.ToString("C") + "</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                        else
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + salePrice.ToString("C") + "</strong>";
                        hdncustomprice.Value = salePrice.ToString();
                        hdnpricecustomcart.Value = salePrice.ToString();
                    }

                    else
                    {
                        //ltRegularPrice.Text = "<div id=\"spnRegularPrice\" class='item-right-pt1-right' style=\"Color:#484848;font-size: 12px;float:left; font-weight: normal;\">" + " : " + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</div>";
                        ltRegularPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>";
                        if (!string.IsNullOrEmpty(hdnSalePriceTag.Value.ToString()) && hdnSalePriceTag.Value.ToString() != "")
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong> <span>" + hdnSalePriceTag.Value.ToString() + "</span>";
                        else
                            ltcustomPrice.Text = " <tt>Your Price :</tt> <strong>" + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["price"].ToString()).ToString("C") + "</strong>";
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
                            ltYourPrice.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C");
                            ltYourPriceforshiopop.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C");
                            //ltYourPrice.Text = "<div class='item-right-pt1-right' id=\"spnYourPrice\">" + "<span style='color:#484848; padding-left:0px;'> : <span style='padding-left:0px;'> " + Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C") + "</div>";
                        }
                        else
                        {
                            ltYourPrice.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C");
                            ltYourPriceforshiopop.Text = Convert.ToDecimal(dsproduct.Tables[0].Rows[0]["saleprice"].ToString()).ToString("C");
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
                        divYourPrice.Visible = false;
                        divYouSave.Visible = false;
                    }

                }
                else
                {
                    hdnsaleprice.Value = "0";
                    divYourPrice.Visible = false;
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
                btnAddToCart.OnClientClick = "InsertProductQuickview(" + Request.QueryString["PID"].ToString() + ",'btnAddToCart'); return false;";
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

                if (!String.IsNullOrEmpty(dsproduct.Tables[0].Rows[0]["Imagename"].ToString()))
                {
                    if (File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsproduct.Tables[0].Rows[0]["Imagename"].ToString())))
                    {
                        imgMain.Src = AppLogic.AppConfigs("ImagePathProduct") + "icon/" + dsproduct.Tables[0].Rows[0]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                        //ltrMainImage.Text = "<img src=\"" + AppLogic.AppConfigs("ImagePathProduct") + "medium/" + dsproduct.Tables[0].Rows[0]["Imagename"].ToString() + "?" + rd.Next(10000).ToString() + "\" alt=\"\" id=\"imgMain\"  title=\"\"  style=\"vertical-align: middle;width: 322px; height: 312px;\" />";

                        //if (!string.IsNullOrEmpty(StrTagName))
                        //{

                        //    lblTag.Text += "<img style='float:right;' title='" + StrTagName.ToString().Trim() + "' src=\"/images/" + StrTagName.ToString().Trim() + ".png\" alt=\"" + StrTagName.ToString().Trim() + "\" class='" + StrTagName.ToString().ToLower() + "' />";
                        //    ImgNext.Attributes.Add("style", "color:#5e5e5e;");

                        //    INextImage.Attributes.Add("style", "float:right;");
                        //    //IPreImage.Attributes.Add("style", "width:60px !important;");

                        //}
                        //if (dsproduct.Tables[0].Rows[0]["IsFreeEngraving"].ToString() == "True")
                        //{
                        //    lblFreeEngravingImage.Text = "<img title='Free Engraving' src=\"/images/FreeEngraving.png\" alt=\"Free Engraving\" style='position: relative;float:left; top: 0px; left: 0px;'/>";
                        //}


                    }
                    else
                    {
                        imgMain.Src = AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(10000).ToString();
                        //ltrMainImage.Text = "<img src=\"" + AppLogic.AppConfigs("ImagePathProduct") + "medium/image_not_available.jpg?" + rd.Next(10000).ToString() + "\" alt=\"\" title=\"\" style=\"vertical-align: middle;width: 322px; height: 312px;\" />";
                    }
                }
                else
                {
                    imgMain.Src = AppLogic.AppConfigs("ImagePathProduct") + "icon/image_not_available.jpg?" + rd.Next(10000).ToString();
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

                    string Imagename = GetMediumImage(dsproduct.Tables[0].Rows[0]["Imagename"].ToString());

                    string Description = dsproduct.Tables[0].Rows[0]["Name"].ToString().Replace("'", "''") + " - SKU: " + dsproduct.Tables[0].Rows[0]["Sku"].ToString() + " at " + AppLogic.AppConfigs("Live_server").Replace("www.", "");
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
                }
                catch { }
                GetTabs(dsproduct);
                GetCommnetByCustomer(productID);


                BindMoreImage(dsproduct.Tables[0].Rows[0]["ImageName"].ToString(), "jpg");

                // Bind Variant Engraving Line
                string StrEngraving = string.Empty;
                DataSet dsVariant = new DataSet();
                dsVariant = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + productID + " AND isnull(ParentId,0)=0 Order By DisplayOrder");
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
                    //strvarinatcustom += "<div id=\"divVariantcustom\">";
                    bool checkmade = false;
                    bool isCustom = false;
                    bool isChild = false;
                    Int32 Sid = 0;
                    Int32 CntReadymade = 0;
                    Int32 iNumb = 0;
                    if (areadymade.Visible == true)
                    {
                        CntReadymade = 1;
                    }
                    for (int i = 0; i < dsVariant.Tables[0].Rows.Count; i++)
                    {
                        iNumb++;
                        string strvarinatchild = "";
                        string radiobutton = "";
                        checkmade = false;
                        strvarinat += "<div class=\"readymade-detail-pt1\" >";
                        if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("header design") > -1)
                        {
                            strvarinat += "<div class=\"readymade-detail-pt1-pro\"  id=\"divcolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">";
                        }
                        else
                        {
                            strvarinat += "<div class=\"readymade-detail-pt1-pro\"  id=\"divcolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" onclick=\"varianttabhideshow(" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + ");\">";
                        }
                        if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("header design") > -1)
                        {
                            strvarinat += "<span id=\"spancolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + iNumb.ToString() + "</span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().Replace("select ", "").Replace("select", "") + "###val###";
                        }
                        else
                        {
                            strvarinat += "<span id=\"spancolspan-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + iNumb.ToString() + "</span>" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "###val###";
                        }
                        strvarinat += "<div id=\"divselvalue-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"float:right;line-height:25px;padding-right:2px;\"></div>";
                        strvarinat += "</div>";

                        strvarinat += "<div class=\"readymade-detail-left\" style=\"width:30%;display:none;\" id=\"divvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";
                        DataSet dsVariantvalue = new DataSet();
                        dsVariantvalue = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariantValue WHERE ProductID=" + productID + " AND VariantID=" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + " Order By DisplayOrder");
                        if (dsVariantvalue != null && dsVariantvalue.Tables.Count > 0 && dsVariantvalue.Tables[0].Rows.Count > 0)
                        {
                            strvarinat += "  <div class=\"readymade-detail-right-pro\" style='width:68%;display:none;' id='divSelectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'>";
                            strvarinat += "<select ####onchange#### name=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"width: auto !important;\" class=\"option1\" id=\"Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >";
                            if (dsVariant.Tables[0].Rows[i]["VariantName"].ToString().ToLower().IndexOf("header") > -1)
                            {
                                strvarinat += "<option value=\"0\" style=\"display:none;\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                                strvarinat = strvarinat.Replace("###val###", "###vv###");
                            }
                            else
                            {
                                strvarinat += "<option value=\"0\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                                strvarinat = strvarinat.Replace("###val###", "");
                            }

                            for (int j = 0; j < dsVariantvalue.Tables[0].Rows.Count; j++)
                            {
                                string StrVarValueId = Convert.ToString(dsVariantvalue.Tables[0].Rows[j]["VariantValueId"]);
                                string StrQry = "";
                                Int32 Intcnt = 0;
                                string StrBuy1onsale = "";
                                bool IsOnSale = false;
                                decimal OnsalePrice = 0;

                                if (CntReadymade == 1)
                                {
                                    StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + productID + " and (Convert(char(10),Buy1Fromdate,101) <=  Convert(char(10),GETDATE(),101) and Convert(char(10),Buy1Todate,101) >=convert(char(10),GETDATE(),101)) and ISNULL(Buy1Get1,0)=1 and VariantValueID=" + StrVarValueId + "";
                                    Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                    if (Intcnt > 0)
                                    {
                                        //StrBuy1onsale = "<span class=\"buy-get-free\">(Buy 1 Get 1 Free)</span>";

                                        StrBuy1onsale = "(Buy 1 Get 1 Free)";
                                    }

                                    StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + productID + " and (Convert(char(10),OnSaleFromdate,101) <=  Convert(char(10),GETDATE(),101) and Convert(char(10),OnSaleTodate,101) >=convert(char(10),GETDATE(),101)) and ISNULL(OnSale,0)=1 and VariantValueID=" + StrVarValueId + "";
                                    Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                    if (Intcnt > 0)
                                    {
                                        IsOnSale = true;
                                        OnsalePrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(OnSalePrice,0) as OnSalePrice from tb_ProductVariantValue Where productid=" + productID + " and VariantValueID=" + StrVarValueId + " and ISNULL(OnSale,0)=1"));
                                        //StrBuy1onsale += "<span class=\"buy-get-free\">(On Sale)</span>";
                                        StrBuy1onsale += "(On Sale)";
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
                                        strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvalue.Tables[0].Rows[j]["VariantPrice"].ToString())) + ")";
                                    }
                                }
                                if (dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString().ToLower().IndexOf("custom") > -1)
                                {
                                    checkmade = true;
                                    isCustom = true;
                                }
                                else
                                {
                                    //strvarinat += "<div style=\"float:left;width:100%;margin-bottom:5px;\"><div id=\"divflat-radio-1545-7379\" onclick=\"selecteddropdownvalue(1545,7379);####selectded####;\" class=\"iradio_flat-red\"></div><div style=\"margin-top: 2px;\">&nbsp;25Wx108L Pole Pocket</div></div>";
                                    strvarinat = strvarinat.Replace("###vv###", ":" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "");
                                    strvarinat += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "\" ####selectded####>" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "</option>";
                                }
                                DataSet dsVariantparent = new DataSet();
                                dsVariantparent = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariant WHERE ProductID=" + productID + " AND isnull(ParentId,0)=" + dsVariantvalue.Tables[0].Rows[j]["VariantValueId"].ToString() + "");
                                if (isChild == false && dsVariantparent.Tables.Count > 0 && dsVariantparent.Tables[0].Rows.Count > 0)
                                {
                                    iNumb++;
                                    radiobutton = "";
                                    strvarinat = strvarinat.Replace("####selectded####", " selected");
                                    strvarinat = strvarinat.Replace("####onchange####", " onchange=\"sendData('Selectvariant-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "','divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "');\"");
                                    Sid = Convert.ToInt32(dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString());
                                    strvarinatchild = "";
                                    strvarinatchild += "<div class=\"readymade-detail-pt1\" >";
                                    strvarinatchild += "<div class=\"readymade-detail-pt1-pro\" onclick=\"varianttabhideshow(" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + ");\" id=\"divcolspan-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\">";
                                    strvarinatchild += "<span id=\"spancolspan-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\">" + iNumb.ToString() + "</span>" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString();
                                    strvarinatchild += "<div id=\"divselvalue-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" style=\"float:right;line-height:25px;padding-right:2px;\"></div>";
                                    strvarinatchild += "</div>";

                                    strvarinatchild += "<div class=\"readymade-detail-left\" style=\"width:30%;display:none;\" id=\"divvariantname-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "</div><input type=\"hidden\" name=\"hdnvariantname-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" value=\"" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "\" />";
                                    strvarinatchild += "  <div class=\"readymade-detail-right-pro\" style='width:68%;display:none;' id='divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'>";
                                    //strvarinatchild += "  <div class=\"\" style='width:74%;display:none;' id='divSelectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "'>";
                                    strvarinatchild += "<select onchange=\"PriceChangeondropdown();\" name=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" style=\"width: auto !important;display:none;\" class=\"option1\" id=\"Selectvariant-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "\" >";
                                    strvarinatchild += "<option value=\"0\">" + dsVariantparent.Tables[0].Rows[0]["VariantName"].ToString() + "</option>";
                                    DataSet dsVariantvaluechild = new DataSet();
                                    dsVariantvaluechild = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariantValue WHERE ProductID=" + productID + " AND VariantID=" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "");
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
                                                StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + productID + " and (Convert(char(10),Buy1Fromdate,101) <=  Convert(char(10),GETDATE(),101) and Convert(char(10),Buy1Todate,101) >=convert(char(10),GETDATE(),101)) and ISNULL(Buy1Get1,0)=1 and VariantValueID=" + StrVarValueId + "";
                                                Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                                if (Intcnt > 0)
                                                {
                                                    //StrBuy1onsale = "<span class=\"buy-get-free\">(Buy 1 Get 1 Free)</span>";
                                                    StrBuy1onsale = "(Buy 1 Get 1 Free)";
                                                }

                                                StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + productID + " and (Convert(char(10),OnSaleFromdate,101) <=  Convert(char(10),GETDATE(),101) and Convert(char(10),OnSaleTodate,101) >=convert(char(10),GETDATE(),101)) and ISNULL(OnSale,0)=1 and VariantValueID=" + StrVarValueId + "";
                                                Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                                                if (Intcnt > 0)
                                                {
                                                    IsOnSale = true;
                                                    OnsalePrice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(OnSalePrice,0) as OnSalePrice from tb_ProductVariantValue Where productid=" + productID + " and VariantValueID=" + StrVarValueId + " and ISNULL(OnSale,0)=1"));
                                                    //StrBuy1onsale += "<span class=\"buy-get-free\">(On Sale)</span>";
                                                    StrBuy1onsale += "(On Sale)";
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
                                                    strPrice = "($" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvaluechild.Tables[0].Rows[k]["VariantPrice"].ToString())) + ")";
                                                }
                                            }
                                            if (dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString().ToLower().IndexOf("custom") > -1)
                                            {
                                                checkmade = true;
                                                isCustom = true;
                                            }
                                            else
                                            {
                                                radiobutton += "<div style=\"float:left;width:100%;margin-bottom:5px;\"><div class=\"iradio_flat-red\" onclick=\"selecteddropdownvalue(" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "," + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + ");PriceChangeondropdown();\" id=\"divflat-radio-" + dsVariantparent.Tables[0].Rows[0]["VariantID"].ToString() + "-" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\"></div><div style=\"margin-top: 2px;\">&nbsp;" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "</div></div>";
                                                strvarinatchild += "<option value=\"" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValueId"].ToString() + "\">" + dsVariantvaluechild.Tables[0].Rows[k]["VariantValue"].ToString() + StrBuy1onsale + strPrice + "</option>";
                                            }
                                        }
                                    }
                                    strvarinatchild += "</select>" + radiobutton + "</div>";

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
                            strvarinat += "</select></div>";

                        }
                        strvarinat = strvarinat.Replace("###val###", "");
                        strvarinat += "</div>";

                        strvarinat += strvarinatchild;

                        if (checkmade == true && isChild == false)
                        {
                        }
                        else
                        {

                            //strvarinatcustom += "<div class=\"readymade-detail-pt1\" >";
                            //strvarinatcustom += "<div class=\"readymade-detail-left\" id=\"divvariantnamecustom-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</div><input type=\"hidden\" name=\"hdnvariantnamecustom-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" value=\"" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "\" />";

                            //if (dsVariantvalue != null && dsVariantvalue.Tables.Count > 0 && dsVariantvalue.Tables[0].Rows.Count > 0)
                            //{
                            //    strvarinatcustom += "  <div class=\"readymade-detail-right\" style='width:74%;' id='divSelectvariantcustom-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "'>";
                            //    strvarinatcustom += "<select  name=\"Selectvariantcustom-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" style=\"width: auto !important;\" class=\"option1\" id=\"Selectvariantcustom-" + dsVariant.Tables[0].Rows[i]["VariantID"].ToString() + "\" >";
                            //    strvarinatcustom += "<option value=\"0\">" + dsVariant.Tables[0].Rows[i]["VariantName"].ToString() + "</option>";
                            //    for (int j = 0; j < dsVariantvalue.Tables[0].Rows.Count; j++)
                            //    {
                            //        string strPrice = "";
                            //        if (Convert.ToDecimal(dsVariantvalue.Tables[0].Rows[j]["VariantPrice"].ToString()) > Decimal.Zero)
                            //        {
                            //            strPrice = "(+$" + String.Format("{0:0.00}", Convert.ToDecimal(dsVariantvalue.Tables[0].Rows[j]["VariantPrice"].ToString())) + ")";
                            //        }

                            //        strvarinatcustom += "<option value=\"" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + "\">" + dsVariantvalue.Tables[0].Rows[j]["VariantValue"].ToString() + strPrice + "</option>";

                            //    }
                            //    strvarinatcustom += "</select></div>";
                            //}
                            //strvarinatcustom += "</div>"; 


                        }


                    }
                    //strvarinatcustom += "</div>";
                    strvarinat += "</div>";
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
                    divcustom.Visible = false;
                    ltmadevariant.Visible = false;
                    licustom.Visible = false;
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
            //DataSet dsOA = new DataSet();
            //dsOA = ProductComponent.GetOptinalProductDetailByID(SKUs, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            //if (dsOA != null && dsOA.Tables.Count > 0 && dsOA.Tables[0].Rows.Count > 0)
            //{
            //    gvOptionalAcc.DataSource = dsOA.Tables[0];
            //    gvOptionalAcc.DataBind();
            //    // divOptionalAccessories.Visible = true;
            //}
            //else
            //{
            //    gvOptionalAcc.DataSource = null;
            //    gvOptionalAcc.DataBind();
            //    divOptionalAccessories.Visible = false;
            //}
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



        /// <summary>
        /// Get Comment from Customer By ProductID
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        private void GetCommnetByCustomer(int ProductID)
        {
            DataSet dsComment = new DataSet();
            dsComment = ProductComponent.GetProductRating(ProductID);
            if (dsComment != null && dsComment.Tables.Count > 0 && dsComment.Tables[0].Rows.Count > 0)
            {

                ltRating.Text = "";
                decimal dd = Math.Truncate(Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]));
                decimal ddnew = Convert.ToDecimal(dsComment.Tables[0].Rows[0]["AvgRating"]) - dd;

                if (dd == Convert.ToDecimal(1))
                {
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltRating.Text += "<img  src=\"/images/25-star.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/rating-half.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/75-star.jpg\">";
                    }
                    else
                    {
                        ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    }
                    ltRating.Text += "<img src=\"/images/star-form1.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    ltRating.Text += " (" + dsComment.Tables[0].Rows[0]["AvgRating"].ToString() + "/5 Ratings)";
                }
                else if (dd == Convert.ToDecimal(2))
                {

                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltRating.Text += "<img  src=\"/images/25-star.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/rating-half.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/75-star.jpg\">";
                    }
                    else
                    {
                        ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    }

                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    ltRating.Text += " (" + dsComment.Tables[0].Rows[0]["AvgRating"].ToString() + "/5 Ratings)";
                }
                else if (dd == Convert.ToDecimal(3))
                {


                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltRating.Text += "<img  src=\"/images/25-star.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/rating-half.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/75-star.jpg\">";
                    }
                    else
                    {
                        ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    }


                    ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    ltRating.Text += " (" + dsComment.Tables[0].Rows[0]["AvgRating"].ToString() + "/5 Ratings)";
                }
                else if (dd == Convert.ToDecimal(4))
                {

                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    if (ddnew > Convert.ToDecimal(0) && ddnew <= Convert.ToDecimal(0.25))
                    {
                        ltRating.Text += "<img  src=\"/images/25-star.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.25) && ddnew <= Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/rating-half.jpg\">";
                    }
                    else if (ddnew > Convert.ToDecimal(0.50))
                    {
                        ltRating.Text += "<img  src=\"/images/75-star.jpg\">";
                    }
                    else
                    {
                        ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                    }
                    ltRating.Text += " (" + dsComment.Tables[0].Rows[0]["AvgRating"].ToString() + "/5 Ratings)";
                }
                else if (dd == Convert.ToDecimal(5))
                {

                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += "<img  src=\"/images/star-form.jpg\">";
                    ltRating.Text += " (5/5 Ratings)";
                }
            }
            else
            {
                divCustomerRating.Visible = false;

                ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                ltRating.Text += "<img  src=\"/images/star-form1.jpg\">";
                ltRating.Text += " (0/5 Ratings)";

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

                }
                else
                {
                    Session["NoOfCartItems"] = null;
                    lttotalitems.Text = "0 Item";

                    ltsubtotal.Text = Convert.ToDecimal(0).ToString("C");
                }
            }
            else
            {

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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg1234322", "jAlert('Please enter valid inventory.', 'Message','txtQty');", true);
                            return;

                        }
                        else if (Qty <= 0)
                        {
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg1234322", "jAlert('Please enter valid inventory.', 'Message','txtQty');", true);
                            return;
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
                                string strAll = "function CheckTimer(){" + strAll1 + " document.getElementById('lblTime').innerHTML=(hours < 10 ? '0' : '' ) +hours +' Hrs : ' + (minutes < 10 ? '0' : '' ) + minutes  + ' Mins : ' +  (seconds < 10 ? '0' : '' ) + seconds + ' Sec';}function Settime(){setInterval('CheckTimer();', 1000);} Settime();";
                                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message','txtQty');", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message','txtQty');", true);
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
                                                string strAll = "function CheckTimer(){" + strAll1 + " document.getElementById('lblTime').innerHTML=(hours < 10 ? '0' : '' ) +hours +' Hrs : ' + (minutes < 10 ? '0' : '' ) + minutes  + ' Mins : ' +  (seconds < 10 ? '0' : '' ) + seconds + ' Sec';}function Settime(){setInterval('CheckTimer();', 1000);} Settime();";
                                                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", strAll + " jAlert('" + strResult.ToString() + "', 'Message','txtQty');", true);
                                            }
                                            else
                                            {
                                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "', 'Message','txtQty');", true);
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
                            ScriptManager.RegisterStartupScript(Page, Page.GetType(), "msg12", "jAlert('Not Sufficient Inventory.', 'Message','txtQty');document.getElementById('prepage').style.display = 'none';", true);
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
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg1234322", "jAlert('Please enter valid inventory.', 'Message','txtQty');", true);
                    return;

                }
                else if (Qty <= 0)
                {
                    if (ViewState["VariantNameId"] != null && ViewState["VariantNameId"].ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "dropdwnselect", "SelectVariantBypostback('" + ViewState["VariantNameId"].ToString() + "','" + ViewState["VariantValueId"].ToString() + "');", true);

                    }
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg1234322", "jAlert('Please enter valid inventory.', 'Message','txtQty');", true);
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
                if (File.Exists(Server.MapPath(imagepath)))
                {
                    return imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Medium/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Medium/image_not_available.jpg");
        }

        /// <summary>
        /// Get Next Previous Product
        /// </summary>
        private void GetnextPreviousProduct()
        {

            //

            //
            Int32 pagecount = -2;
            if (Request.QueryString["next"] != null && Request.QueryString["prev"] != null)
            {
                if (Request.QueryString["pcnt"] != null)
                {
                    pagecount = Convert.ToInt32(Request.QueryString["pcnt"].ToString());
                }
                if (Request.QueryString["next"].ToString() == "0" && Request.QueryString["prev"].ToString() == "0")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "mspreviousdisabled1", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){return false;};", true);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnextdisable1", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){return false;};", true);
                }
                else if (Request.QueryString["next"].ToString() != "0" && Request.QueryString["prev"].ToString() == "0")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnext", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-product';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){window.parent.ShowModelQuick(" + Request.QueryString["next"].ToString() + "," + (pagecount + 1).ToString() + ",0);};", true);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "mspreviousdisabled1", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){return false;};", true);
                }
                else if (Request.QueryString["next"].ToString() == "0" && Request.QueryString["prev"].ToString() != "0")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msprevious", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-product';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){window.parent.ShowModelQuick(" + Request.QueryString["prev"].ToString() + "," + (pagecount - 1).ToString() + ",0);};", true);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnextdisable1", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){return false;};", true);
                }
                else if (Request.QueryString["next"].ToString() != "0" && Request.QueryString["prev"].ToString() != "0")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msprevious", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-product';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){window.parent.document.getElementById('prepagequick').style.display = '';window.parent.ShowModelQuick(" + Request.QueryString["prev"].ToString() + "," + (pagecount - 1).ToString() + ",0);};", true);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnext", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-product';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){window.parent.document.getElementById('prepagequick').style.display = '';window.parent.ShowModelQuick(" + Request.QueryString["next"].ToString() + "," + (pagecount + 1).ToString() + ",0);};", true);
                }
            }

            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "mspreviousdisabled1", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){return false;};", true);
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnextdisable1", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){return false;};", true);
            //try
            //{
            //    DataSet dsNext = new DataSet();
            //    if ((Session["ProductIds"] == null || (Session["ProductIds"] != null && !Session["ProductIds"].ToString().Contains(Request.QueryString["PID"].ToString()))))
            //    {
            //        dsNext = CommonComponent.GetCommonDataSet("EXEC usp_Product_GetnextpreviousProduct " + Request.QueryString["PID"].ToString() + "," + AppLogic.AppConfigs("StoreId") + "");
            //        if (dsNext != null && dsNext.Tables.Count > 0 && dsNext.Tables[0].Rows.Count > 0)
            //        {
            //            int cntItem = dsNext.Tables[0].Rows.Count;
            //            string PIDList = string.Empty;
            //            for (int cntPID = 0; cntPID < cntItem; cntPID++)
            //                PIDList += dsNext.Tables[0].Rows[cntPID]["ProductID"].ToString() + ",";
            //            if (PIDList != "")
            //                PIDList = PIDList.Substring(0, PIDList.Length - 1);
            //            Session["ProductIds"] = PIDList;
            //        }
            //    }
            //    if (Session["ProductIds"] != null)
            //    {

            //        string ProductIDList = Session["ProductIds"].ToString();
            //        string CurrPID = Request.QueryString["PID"].ToString();
            //        string TempStr = string.Empty;
            //        int KeyIndex = 0;
            //        int StIndex = 0;
            //        int LastIndex = 0;
            //        string pPID = "";
            //        string nPID = "";
            //        if (ProductIDList != "" && (ProductIDList.IndexOf("," + CurrPID) != -1 || ProductIDList.IndexOf(CurrPID + ",") != -1 || ProductIDList.IndexOf("," + CurrPID + ",") != -1))
            //        {
            //            if (ProductIDList.IndexOf("," + CurrPID + ",") != -1)
            //            {
            //                KeyIndex = ProductIDList.IndexOf("," + CurrPID + ",");
            //                if (ProductIDList.Substring(0, KeyIndex).Length > 8)
            //                    StIndex = KeyIndex - 8;
            //                else
            //                    StIndex = 0;
            //                if (ProductIDList.Substring(KeyIndex, ProductIDList.Length - KeyIndex).Length > 16)
            //                    LastIndex = KeyIndex + 16;
            //                else
            //                    LastIndex = ProductIDList.Length;
            //            }
            //            else if (ProductIDList.IndexOf(CurrPID + ",") != -1 && ProductIDList.IndexOf("," + CurrPID) == -1)
            //            {
            //                KeyIndex = ProductIDList.IndexOf(CurrPID + ",");
            //                StIndex = 0;
            //                if (ProductIDList.Length > 16)
            //                    LastIndex = 16;
            //                else
            //                    LastIndex = ProductIDList.Length;
            //            }
            //            else if (ProductIDList.IndexOf("," + CurrPID) != -1 && ProductIDList.IndexOf(CurrPID + ",") == -1)
            //            {
            //                KeyIndex = ProductIDList.IndexOf("," + CurrPID);
            //                if (ProductIDList.Length > 16)
            //                    StIndex = KeyIndex - 8;
            //                else
            //                    StIndex = 0;
            //                LastIndex = ProductIDList.Length;
            //            }
            //            else if (ProductIDList.IndexOf(CurrPID + ",") == 0)
            //            {
            //                KeyIndex = ProductIDList.IndexOf(CurrPID + ",");
            //                StIndex = 0;
            //                if (ProductIDList.Length > 16)
            //                    LastIndex = 16;
            //                else
            //                    LastIndex = ProductIDList.Length;
            //            }


            //            TempStr = ProductIDList.Substring(StIndex, LastIndex - StIndex);
            //            string[] StrArrPID = TempStr.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

            //            if (StrArrPID.Length > 0)
            //            {
            //                if (StrArrPID[0] == CurrPID)
            //                    nPID = StrArrPID[1];
            //                else if (StrArrPID[StrArrPID.Length - 1] == CurrPID)
            //                    pPID = StrArrPID[StrArrPID.Length - 2];
            //                else if (StrArrPID.Length > 2 && TempStr.Contains(CurrPID))
            //                    for (int i = 0; i < StrArrPID.Length; i++)
            //                    {
            //                        try
            //                        {
            //                            pPID = StrArrPID[i].ToString();
            //                            if (StrArrPID[i + 1].ToString() == CurrPID)
            //                            {
            //                                nPID = StrArrPID[i + 2].ToString();
            //                                break;
            //                            }
            //                        }
            //                        catch { }
            //                    }
            //            }
            //        }




            //        string sqtParam = "";
            //        if (pPID != "" && nPID == "")
            //            sqtParam = pPID;
            //        else if (pPID == "" && nPID != "")
            //            sqtParam = nPID;
            //        else if (pPID != "" && nPID != "")
            //            sqtParam = pPID + "," + nPID;

            //        if (sqtParam != "")
            //        {
            //            DataSet dsPreproduct = new DataSet();
            //            dsPreproduct = CommonComponent.GetCommonDataSet("SELECT imagename,MainCategory,SEName,productid,isnull(tb_Product.ProductURL,'') as ProductURL FROM tb_Product WHERE productid in (" + sqtParam + ")");

            //            if (dsPreproduct != null && dsPreproduct.Tables.Count > 0 && dsPreproduct.Tables[0].Rows.Count == 2)
            //            {
            //                if (pPID != "")
            //                {

            //                    if (pPID != null)
            //                    {
            //                        DataRow[] dr = dsPreproduct.Tables[0].Select("productid=" + pPID.ToString() + "");
            //                        foreach (DataRow dr1 in dr)
            //                        {
            //                            //ImgPrevious.HRef = "/" + dr1["MainCategory"].ToString() + "/" + dr1["SEName"].ToString() + "-" + pPID + ".aspx";
            //                            ImgPrevious.HRef = "/" + dr1["ProductURL"].ToString().ToLower();
            //                            imgPrev.Src = GetMicroImage(dr1["ImageName"].ToString());
            //                            break;
            //                        }
            //                    }
            //                    // IPreImage.Visible = true;
            //                    ImgPrevious.Visible = true;
            //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msprevious", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-product';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){window.parent.document.getElementById('prepagequick').style.display = '';window.parent.document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=" + pPID + "';};", true);

            //                    imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 60%; margin-top: 2%;");

            //                }
            //                else
            //                {
            //                    //IPreImage.Visible = false;
            //                    ImgPrevious.Visible = false;
            //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "mspreviousdisabled", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){return false;};", true);
            //                    imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 72%; margin-top: 2%;");
            //                }
            //                if (nPID != "")
            //                {
            //                    DataRow[] dr = dsPreproduct.Tables[0].Select("productid=" + nPID.ToString() + "");
            //                    foreach (DataRow dr1 in dr)
            //                    {
            //                        //ImgNext.HRef = "/" + dr1["MainCategory"].ToString() + "/" + dr1["SEName"].ToString() + "-" + nPID + ".aspx";
            //                        ImgNext.HRef = "/" + dr1["ProductURL"].ToString().ToLower();
            //                        imgNextImage.Src = GetMicroImage(dr1["ImageName"].ToString());
            //                        break;
            //                    }
            //                    //INextImage.Visible = true;
            //                    ImgNext.Visible = true;
            //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnext", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-product';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){window.parent.document.getElementById('prepagequick').style.display = '';window.parent.document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=" + nPID + "';};", true);
            //                    if (ImgPrevious.Visible == true)
            //                        imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 60%; margin-top: 2%;");
            //                    else
            //                        imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 72%; margin-top: 2%;");

            //                }
            //                else
            //                {
            //                    //INextImage.Visible = false;
            //                    ImgNext.Visible = false;
            //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnextdisable", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){return false;};", true);
            //                }

            //            }
            //            else if (dsPreproduct != null && dsPreproduct.Tables.Count > 0 && dsPreproduct.Tables[0].Rows.Count == 1)
            //            {
            //                if (pPID != "")
            //                {
            //                    DataRow[] dr = dsPreproduct.Tables[0].Select("productid=" + pPID.ToString() + "");
            //                    foreach (DataRow dr1 in dr)
            //                    {
            //                        //ImgPrevious.HRef = "/" + dr1["MainCategory"].ToString() + "/" + dr1["SEName"].ToString() + "-" + pPID + ".aspx";
            //                        ImgPrevious.HRef = "/" + dr1["ProductURL"].ToString().ToLower();
            //                        imgPrev.Src = GetMicroImage(dr1["ImageName"].ToString());
            //                        break;
            //                    }
            //                    //IPreImage.Visible = true;
            //                    ImgPrevious.Visible = true;
            //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msprevious", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-product';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){window.parent.document.getElementById('prepagequick').style.display = '';window.parent.document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=" + pPID + "';};", true);
            //                    imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 60%; margin-top: 2%;");
            //                }
            //                else
            //                {
            //                    // IPreImage.Visible = false;
            //                    ImgPrevious.Visible = false;
            //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "mspreviousdisabled", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){return false;};", true);
            //                    imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 72%; margin-top: 2%;");
            //                }
            //                if (nPID != "")
            //                {
            //                    DataRow[] dr = dsPreproduct.Tables[0].Select("productid=" + nPID.ToString() + "");
            //                    foreach (DataRow dr1 in dr)
            //                    {
            //                        //ImgNext.HRef = "/" + dr1["MainCategory"].ToString() + "/" + dr1["SEName"].ToString() + "-" + nPID + ".aspx";
            //                        ImgNext.HRef = "/" + dr1["ProductURL"].ToString().ToLower();
            //                        imgNextImage.Src = GetMicroImage(dr1["ImageName"].ToString());
            //                        break;
            //                    }
            //                    //INextImage.Visible = true;

            //                    ImgNext.Visible = true;
            //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnext", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-product';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){window.parent.document.getElementById('prepagequick').style.display = '';window.parent.document.getElementById('frmdisplayquick').src = '/QuickView.aspx?PID=" + nPID + "';};", true);
            //                    ImgNext.Visible = true;
            //                    if (ImgPrevious.Visible == true)
            //                        imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 60%; margin-top: 2%;");
            //                    else
            //                        imgNextImage.Attributes.Add("style", "border:1px solid #eee; width: 100px; height: 120px; position: absolute;display: none; margin-left: 72%; margin-top: 2%;");
            //                }
            //                else
            //                {
            //                    //INextImage.Visible = false;
            //                    ImgNext.Visible = false;
            //                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnextdisable", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){return false;};", true);
            //                }

            //            }
            //        }
            //        else
            //        {
            //            //INextImage.Visible = false;
            //            //IPreImage.Visible = false;
            //            ImgPrevious.Visible = false;
            //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "mspreviousdisabled", "window.parent.document.getElementById('ContentPlaceHolder1_subpre').className='pre-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subpre').onclick=function(){return false;};", true);
            //            ImgNext.Visible = false;
            //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msnextdisable", "window.parent.document.getElementById('ContentPlaceHolder1_subnext').className='next-productdisabled';window.parent.document.getElementById('ContentPlaceHolder1_subnext').onclick=function(){return false;};", true);
            //        }
            //    }


            //}
            //catch
            //{

            //}
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

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }
            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Icon/image_not_available.jpg");
        }



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
                ltmoreimages.Text = "";
                strImagePath = GetMicroImage(strImageName + "." + strImageExt);
                strLImagePath = GetMoreMediumImage(strImageName + "." + strImageExt);
                if (strImagePath != AppLogic.AppConfigs("ImagePathProduct") + "micro/image_not_available.jpg")
                {
                    strMoreImg.Append("<li><a  href=\"javascript:void(0);\"><img id='first' onmouseover=\"document.getElementById('imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click(); $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" onclick=\"document.getElementById('imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click(); $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" src=\"" + strImagePath + "\"   width=\"58\" height=\"78\" /></a></li>");
                    ltmoreimages.Text += "<li><a href=\"javascript:void(0);\"><img width=\"44px\" height=\"40px\" onmouseover=\"document.getElementById('imgMain').src='" + strLImagePath.ToLower().Replace("/large/", "/icon/") + "'; $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" onclick=\"document.getElementById('imgMain').src='" + strLImagePath.ToLower().Replace("/medium/", "/icon/") + "'; $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" src=\"" + strImagePath.ToLower().Replace("/medium/", "/icon/") + "\" ></a></li>";
                }

                int CountMoreImage = 0;
                divtempimage.InnerHtml = "";

                DataSet dsImgdesc = new DataSet();
                dsImgdesc = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductImgDesc WHERE ProductId=" + Request.QueryString["PID"].ToString() + " Order By ImageNo");
                if (dsImgdesc != null && dsImgdesc.Tables.Count > 0 && dsImgdesc.Tables[0].Rows.Count > 0)
                {
                    Random rd = new Random();
                    for (int d = 0; d < dsImgdesc.Tables[0].Rows.Count; d++)
                    {
                        if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()))
                        {
                            strImagePath = GetMicroImage(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                            strLImagePath = GetMoreMediumImage(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                            if (strImagePath != AppLogic.AppConfigs("ImagePathProduct") + "micro/" + "image_not_available.jpg")
                            {

                                strMoreImg.Append("<li><a  href=\"javascript:void(0);\"><img  onmouseover=\"document.getElementById('imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click(); $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" onclick=\"document.getElementById('imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click(); $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" src=\"" + strImagePath + "\"   width=\"58\" height=\"78\" /></a></li>");
                                CountMoreImage++;
                                divtempimage.InnerHtml += "<img src=\"" + strLImagePath + "\" />";
                                ltmoreimages.Text += "<li><a href=\"javascript:void(0);\"><img width=\"44px\" height=\"40px\" onmouseover=\"document.getElementById('imgMain').src='" + strLImagePath.ToLower().Replace("/large/", "/icon/") + "'; $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" onclick=\"document.getElementById('imgMain').src='" + strLImagePath.ToLower().Replace("/medium /", "/icon/") + "'; $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" src=\"" + strImagePath.ToLower().Replace("/medium/", "/icon/") + "\" ></a></li>";


                            }
                        }
                    }
                }
                else
                {
                    for (int cnt = 1; cnt < 25; cnt++)
                    {
                        if (CountMoreImage > 7)
                        {
                            break;
                        }

                        strImagePath = GetMicroImage(strImageName + "_" + cnt.ToString() + "." + strImageExt);
                        strLImagePath = GetMoreMediumImage(strImageName + "_" + cnt.ToString() + "." + strImageExt);
                        if (strImagePath != AppLogic.AppConfigs("ImagePathProduct") + "micro/" + "image_not_available.jpg")
                        {

                            strMoreImg.Append("<li><a  href=\"javascript:void(0);\"><img  onmouseover=\"document.getElementById('imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click(); $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" onclick=\"document.getElementById('imgMain').src='" + strLImagePath + "';document.getElementById('Button2').click(); $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" src=\"" + strImagePath + "\"   width=\"58\" height=\"78\" /></a></li>");

                            CountMoreImage++;
                            divtempimage.InnerHtml += "<img src=\"" + strLImagePath + "\" />";
                            ltmoreimages.Text += "<li><a href=\"javascript:void(0);\"><img width=\"44px\" height=\"40px\" onmouseover=\"document.getElementById('imgMain').src='" + strLImagePath.ToLower().Replace("/large/", "/icon/") + "'; $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" onclick=\"document.getElementById('imgMain').src='" + strLImagePath.ToLower().Replace("/medium /", "/icon/") + "'; $('#loader_img1').show(); $('#imgMain').hide(); $('#imgMain').load(function () { $('#loader_img1').hide(); $('#imgMain').show(); }).each(function () { if (this.complete) $(this).load(); });\" src=\"" + strImagePath.ToLower().Replace("/medium/", "/icon/") + "\" ></a></li>";


                        }
                    }
                }

                //ltBindMoreImage.Text = strMoreImg.ToString();
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
            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }

            return AppLogic.AppConfigs("ImagePathProduct") + "micro/" + "image_not_available.jpg";
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

            if (File.Exists(Server.MapPath(imagepath)))
            {
                return imagepath;
            }

            return AppLogic.AppConfigs("ImagePathProduct") + "medium/" + "image_not_available.jpg";
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
                if (System.IO.File.Exists(Server.MapPath(imagepath)))
                { }
                else
                {
                    imagepath = "" + AppLogic.AppConfigs("ImagePathProduct") + "Medium/" + "image_not_available.gif";
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
                divPriceQuote.Visible = false;
                //DataSet dsProductQuote = new DataSet();
                //dsProductQuote = CommonComponent.GetCommonDataSet("select IsPriceQuote,ProductID from tb_Product where ProductID=" + Convert.ToString(Request.QueryString["PID"]) + "");
                //if (dsProductQuote != null && dsProductQuote.Tables.Count > 0 && dsProductQuote.Tables[0].Rows.Count > 0)
                //{
                //    if (Convert.ToBoolean(dsProductQuote.Tables[0].Rows[0]["IsPriceQuote"].ToString()) == true)
                //        divPriceQuote.Visible = true;
                //    else
                //        divPriceQuote.Visible = false;
                //}
                //else divPriceQuote.Visible = false;
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
    }
}