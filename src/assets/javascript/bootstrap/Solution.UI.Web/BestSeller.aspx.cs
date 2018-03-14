using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.IO;

namespace Solution.UI.Web
{
    public partial class BestSeller : System.Web.UI.Page
    {
        #region Declaration

        PagedDataSource pgsource = new PagedDataSource();
        int findex, lindex;
        Int32 itemCount = 6;
        #endregion


        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonOperations.RedirectWithSSL(false);
            ltbrTitle.Text = "Best Sellers";
            ltTitle.Text = "Best Sellers";
            if (!IsPostBack)
            {
                BindData();
                BindPattern();
                BindFabric();
                BindStyle();
                BindColors();
                BindHeader();
                if (ViewState["strFeaturedimg"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloader", "$(document).ready(function () {" + ViewState["strFeaturedimg"].ToString()+"});", true);
                }
            }
            else
            {
                if (ViewState["strFeaturedimgpostback"] != null)
                {
                    //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
                }

            }

        }

        /// <summary>
        /// Method to bind Repeater with New Arrival Product
        /// </summary>
        private void BindData()
        {
            DataSet dsNewArrival = ProductComponent.GetAllBestSellerProduct(ddlSortby.SelectedValue, AppLogic.AppConfigs("StoreID"));
            if (dsNewArrival != null && dsNewArrival.Tables.Count > 0 && dsNewArrival.Tables[0].Rows.Count > 0)
            {
                #region Paging Code
                pgsource.DataSource = dsNewArrival.Tables[0].DefaultView;
                //pgsource.AllowPaging = true;

                //if (ViewState["All"] == null)
                //{
                //    lnkViewAllPages.Visible = false;
                //    lnkBottomViewAllPages.Visible = false;
                //    divViewAllPagesBottom.Visible = false;
                //    pgsource.PageSize = 15;
                //    divTopPaging.Visible = true;
                //    divBottomPaging.Visible = true;
                //}
                //else
                //{
                //    lnkViewAllPages.Visible = true;
                //    lnkBottomViewAllPages.Visible = true;
                //    divViewAllPagesBottom.Visible = true;
                //    CurrentPage = 0;
                //    divTopPaging.Visible = false;
                //    divBottomPaging.Visible = false;
                   pgsource.PageSize = dsNewArrival.Tables[0].Rows.Count;
                //}
                //pgsource.CurrentPageIndex = CurrentPage;

                ////Store it Total pages value in View state
                //ViewState["totpage"] = pgsource.PageCount;

                //this.lnkPrevious.Visible = !pgsource.IsFirstPage;
                //this.lnkNext.Visible = !pgsource.IsLastPage;
                //this.lnkTopprevious.Visible = !pgsource.IsFirstPage;
                //this.lnktopNext.Visible = !pgsource.IsLastPage;

                //if (CurrentPage == 0 && CurrentPage == pgsource.PageCount - 1)
                //{
                //    lnkViewAll.Visible = false;
                //    lnkBottomViewAll.Visible = false;
                //}
                //else
                //{
                //    lnkViewAll.Visible = true;
                //    lnkBottomViewAll.Visible = true;
                //}
                itemCount = 6;
                RptNewArrivalProduct.DataSource = pgsource;
                RptNewArrivalProduct.DataBind();
                //doPaging();
                RepeaterPaging.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                dlToppaging.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                #endregion
            }
        }

        /// <summary>
        /// To Set Paging Index
        /// </summary>
        private void doPaging()
        {
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
        /// Get Value of Current Page
        /// </summary>
        private int CurrentPage
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
                //Set View state value when page is changed through Paging "RepeaterPaging" DataList
                ViewState["CurrentPage"] = value;
            }
        }

        /// <summary>
        /// Repeater Paging Item Command Event
        /// </summary>
        /// <param name="source">bject source</param>
        /// <param name="e">DataListCommandEventArgs e</param>
        protected void RepeaterPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("newpage"))
            {
                //Assign CurrentPage number when user click on the page number in the Paging "RepeaterPaging" DataList
                CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
                //Refresh "Repeater1" control Data once user change page
                BindData();
            }
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        /// Get First Record
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkFirst_Click(object sender, EventArgs e)
        {
            //If user click First Link button assign current index as Zero "0" then refresh "Repeater1" Data.
            CurrentPage = 0;
            BindData();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", "$(document).ready(function () {" + ViewState["strFeaturedimg"].ToString()+"});", true);
            }
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        /// Get Last Record
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkLast_Click(object sender, EventArgs e)
        {
            //If user click Last Link button assign current index as totalpage then refresh "Repeater1" Data.
            CurrentPage = (Convert.ToInt32(ViewState["totpage"]) - 1);
            BindData();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", "$(document).ready(function () {" + ViewState["strFeaturedimg"].ToString()+"});", true);
            }
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        /// Get Previous Record
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            //If user click Previous Link button assign current index as -1 it reduce existing page index.
            CurrentPage -= 1;
            //refresh "Repeater1" Data
            BindData();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", "$(document).ready(function () {" + ViewState["strFeaturedimg"].ToString()+"});", true);
            }
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        /// Get Next Record
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            //If user click Next Link button assign current index as +1 it add one value to existing page index.
            CurrentPage += 1;

            //refresh "Repeater1" Data
            BindData();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", "$(document).ready(function () {" + ViewState["strFeaturedimg"].ToString()+"});", true);
            }
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        /// Repeater Paging Item Data Bound
        /// </summary>
        /// <param name="sender">object senders</param>
        /// <param name="e">DataListItemEventArgs e</param>
        protected void RepeaterPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //Enabled False for current selected Page index
            LinkButton lnkPage = (LinkButton)e.Item.FindControl("Pagingbtn");
            if (lnkPage.CommandArgument.ToString() == CurrentPage.ToString())
            {
                lnkPage.Enabled = false;
               // lnkPage.Attributes.Add("style", "background: none repeat scroll 0 0 #7D7D7D;color: #FFFFFF;");
                lnkPage.Attributes.Add("style", "color: #B92127;");
            }
            else
            {
                //lnkPage.Attributes.Add("style", "background: none repeat scroll 0 0 #FFFFFF;color: #5E5E5E;");
            }
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        /// ItemDataBound Method to display 3 Record in each row
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RptNewArrivalProduct_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                HtmlGenericControl Probox = (HtmlGenericControl)e.Item.FindControl("Probox");
                HtmlGenericControl proDisplay = (HtmlGenericControl)e.Item.FindControl("proDisplay");
                Label lblTName = (Label)e.Item.FindControl("lblTName");
                Literal lblTag = (Literal)e.Item.FindControl("lblTag");
                HtmlInputHidden hdnImgName = (HtmlInputHidden)e.Item.FindControl("hdnImgName");
                Literal litControl = (Literal)e.Item.FindControl("ltTop");
                Literal ltbottom = (Literal)e.Item.FindControl("ltbottom");
                if (hdnImgName != null && !string.IsNullOrEmpty(hdnImgName.Value.ToString()))
                {
                    string ImgName = GetIconImageProduct(hdnImgName.Value.ToString());

                    if (!string.IsNullOrEmpty(ImgName) && !ImgName.ToString().ToLower().Contains("image_not_available"))
                    {
                        if (lblTName != null && !string.IsNullOrEmpty(lblTName.Text.ToString().Trim()))
                        {
                            lblTag.Text = "<img title='" + lblTName.Text.ToString().Trim() + "' src=\"/images/" + lblTName.Text.ToString().Trim() + ".png\" alt=\"" + lblTName.Text.ToString().Trim() + "\" class='" + lblTName.Text.ToString().ToLower() + "' />";
                        }
                    }
                }
                if (ViewState["strFeaturedimg"] != null)
                {
                  //  ViewState["strFeaturedimg"] = ViewState["strFeaturedimg"].ToString() + "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                }
                else
                {
                   // ViewState["strFeaturedimg"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').show();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').load(function () {$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();$('#imgFeaturedProduct" + (e.Item.ItemIndex + 1).ToString() + "').show();}).each(function() {if(this.complete) $(this).load();});";
                }
                if (ViewState["strFeaturedimgpostback"] != null)
                {
                   // ViewState["strFeaturedimgpostback"] = ViewState["strFeaturedimgpostback"].ToString() + "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                else
                {
                   // ViewState["strFeaturedimgpostback"] = "$('#loader_img" + (e.Item.ItemIndex + 1).ToString() + "').hide();";
                }
                if ((e.Item.ItemIndex + 1) % itemCount == 0 && e.Item.ItemIndex != 0)
                {
                    //Probox.Attributes.Add("style", "margin-right:0px;");
                    ////proDisplay.Attributes.Add("class", "pro-display-none");
                    litControl.Text = "</ul><ul><li>";
                    ltbottom.Text = "</li>";
                    itemCount = itemCount + 5;
                }
                else
                {
                    litControl.Text = "<li>";
                    ltbottom.Text = "</li>";
                }

                //else
                //    Probox.Attributes.Add("class", "cat-box");

                Label lblPrice = (Label)e.Item.FindControl("lblPrice");
                Label lblSalePrice = (Label)e.Item.FindControl("lblSalePrice");
                Literal ltrYourPrice = (Literal)e.Item.FindControl("ltrYourPrice");
                Literal ltrRegularPrice = (Literal)e.Item.FindControl("ltrRegularPrice");
                HtmlInputHidden ltrLink = (HtmlInputHidden)e.Item.FindControl("ltrLink");
                HtmlInputHidden ltrlink1 = (HtmlInputHidden)e.Item.FindControl("ltrLink1");
                Literal ltrInventory = (Literal)e.Item.FindControl("ltrInventory");
                Literal ltrproductid = (Literal)e.Item.FindControl("ltrproductid");
                HtmlAnchor anewArrival = (HtmlAnchor)e.Item.FindControl("anewArrival");
                Decimal hdnprice = 0;
                Decimal SalePrice = 0;
                Decimal Price = 0;

                if (lblPrice != null)
                    Price = Convert.ToDecimal(lblPrice.Text);
                if (lblSalePrice != null)
                    SalePrice = Convert.ToDecimal(lblSalePrice.Text);

                if (Price > decimal.Zero)
                {
                    ltrRegularPrice.Text += "Starting Price: <span>" + Price.ToString("C") + "</span>";
                    hdnprice = Price;
                }
                // else ltrRegularPrice.Text += "<p>&nbsp;</p>";

                if (SalePrice > decimal.Zero && Price > SalePrice)
                {
                    ltrYourPrice.Text = "Starting Price: <span>" + SalePrice.ToString("C") + "</span>";
                    hdnprice = SalePrice;
                }
                else ltrYourPrice.Text = "Starting Price: <span>" + Price.ToString("C") + "</span>";

                if (Convert.ToInt32(ltrInventory.Text.ToString()) > 0)
                {

                    Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(dbo.tb_ProductVariant.VariantID) FROM  dbo.tb_ProductVariant INNER JOIN dbo.tb_ProductVariantValue ON dbo.tb_ProductVariant.VariantID = dbo.tb_ProductVariantValue.VariantID WHERE dbo.tb_ProductVariant.Productid=" + ltrproductid.Text.ToString() + " "));
                    if (Count == 0)
                    {
                        anewArrival.Attributes.Add("onclick", "adtocart('" + hdnprice.ToString() + "'," + ltrproductid.Text.ToString() + ");");
                    }
                    else
                    {
                        anewArrival.Attributes.Add("onclick", "ShowModelQuick(" + ltrproductid.Text.ToString() + ");");
                    }
                }
                else
                {
                    anewArrival.Attributes.Remove("onclick");
                    anewArrival.HRef = "/" + ltrLink.Value.ToString() + "/" + ltrlink1.Value.ToString() + "-" + ltrproductid.Text.ToString() + ".aspx";
                }
            }
        }

        /// <summary>
        /// Get Icon Image for Product
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Image Path</returns>
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
        ///  Sort by Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlSortby_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlbottomprice.SelectedValue = ddlSortby.SelectedValue;
            BindData();
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        ///  Filter by Bottom Price Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlbottomprice_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlSortby.SelectedValue = ddlbottomprice.SelectedValue;
            BindData();
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        /// Add '...', if String length is more than 45 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 45 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 45)
                Name = Name.Substring(0, 42) + "...";
            return Server.HtmlEncode(Name);
        }

        /// <summary>
        ///  View All link Records
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkViewAll_Click(object sender, EventArgs e)
        {
            ViewState["All"] = "All";
            BindData();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", "$(document).ready(function () {" + ViewState["strFeaturedimg"].ToString()+"});", true);
            }
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        ///  View All link Pages
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkViewAllPages_Click(object sender, EventArgs e)
        {
            ViewState["All"] = null;
            BindData();
            if (ViewState["strFeaturedimg"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloader", "$(document).ready(function () {" + ViewState["strFeaturedimg"].ToString()+"});", true);
            }
            if (ViewState["strFeaturedimgpostback"] != null)
            {
                //ViewState["strFeaturedimg"] = "  + ViewState["strFeaturedimg"].ToString() + "});";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msgloaderpost", "$(document).ready(function () {" + ViewState["strFeaturedimgpostback"].ToString()+"});", true);
            }
        }

        /// <summary>
        /// Add Customer for Cart item
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
        /// Replace the '"' and '\' which is comes in ProductName to "-"
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>return the ProductName with Replace the '"' and '\' which is comes in ProductName to "-" </returns>
        public String SetAttribute(String Name)
        {
            return Name.Replace('"', '-').Replace('\'', '-').ToString();
        }

        private void BindPattern()
        {
            string StrPattern = "";
            DataSet dsPattern = new DataSet();
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType=2 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order By isnull(Displayorder,999)");
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
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 3 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order By isnull(Displayorder,999)");
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
            dsPattern = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(ImageName,'') as ImageName,ISNULL(Active,1) as Active From tb_ProductSearchType where SearchType = 4 and ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 Order By isnull(Displayorder,999)");
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
                    StrPattern += "<img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString() + "\"><span style=\"padding-left: 16px;\">" + SearchValue.ToString() + "</span></li>";
                }
                StrPattern += "</ul></li>";
            }
            StrPattern += "</ul>";
            StrPattern += "</div>";
            ltrHeader.Text = StrPattern.ToString();
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
                        StrPattern += "<li><ul class=\"option-pro-color\">";
                    }
                    if (icheck % 10 == 0 && icheck > 9)
                    {
                        if (dsPattern.Tables[0].Rows.Count - 1 > i)
                        {
                            StrPattern += "</ul></li><li><ul class=\"option-pro-color\">";
                        }
                    }

                    icheck++;
                    StrPattern += "<li class=\"option-pro-box\" style=\"padding-bottom:4px !important;\">";
                    StrPattern += "<a href=\"javascript:void(0);\" onclick=\"ColorSelection('" + SearchValue.ToString() + "');\"><img id=\"Img_" + SearchId + "\" title=\"" + SearchValue.ToString() + "\" alt=\"" + SearchValue.ToString() + "\" src=\"" + strImageName.ToString() + "\"></a> </li>";
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
                if (Request.QueryString["CatID"] != null && Convert.ToString(Request.QueryString["CatID"]).Trim() != "")
                {
                    if (Request.QueryString["CatPID"] != null && Convert.ToString(Request.QueryString["CatPID"]).Trim() != "")
                    {
                        Response.Redirect("/ProductSearchList.aspx?CatID=" + Request.QueryString["CatID"].ToString().Trim() + "&CatPID=" + Request.QueryString["CatPID"].ToString() + "");
                    }
                    else
                    {
                        Response.Redirect("/ProductSearchList.aspx?CatID=" + Request.QueryString["CatID"].ToString().Trim() + "");
                    }
                }
                else
                {
                    Response.Redirect("/ProductSearchList.aspx");
                }
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
                Response.Redirect("/ProductSearchList.aspx");
            }
            else
            {
                if (Request.QueryString["CatID"] != null && Convert.ToString(Request.QueryString["CatID"]).Trim() != "")
                {
                    if (Request.QueryString["CatPID"] != null && Convert.ToString(Request.QueryString["CatPID"]).Trim() != "")
                    {
                        Response.Redirect("/ProductSearchList.aspx?CatID=" + Request.QueryString["CatID"].ToString().Trim() + "&CatPID=" + Request.QueryString["CatPID"].ToString() + "");
                    }
                    else
                    {
                        Response.Redirect("/ProductSearchList.aspx?CatID=" + Request.QueryString["CatID"].ToString().Trim() + "");
                    }
                }
                else
                {
                    Response.Redirect("/ProductSearchList.aspx");
                }
            }
        }
        protected void btntempclick_Click(object sender, EventArgs e)
        {

        }
    }
}