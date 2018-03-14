using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.Common;
using System.IO;
using System.Web.UI.HtmlControls;
namespace Solution.UI.Web
{
    public partial class SearchOld : System.Web.UI.Page
    {
        DataSet dsCategory = new DataSet();
        public static string ProductIconPath = string.Empty;
        PagedDataSource pgsource = new PagedDataSource();
        int findex, lindex;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            ltbrTitle.Text = "Search";
            ltTitle.Text = "Search";
            if (!IsPostBack)
            {
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");

                if (Request.QueryString["SearchTerm"] != null)
                {
                    txtSearch.Focus();
                    txtSearch.Text = Request.QueryString["SearchTerm"].ToString().Trim();
                    FillCategoryDropDown();
                    BindSearching();
                }
                else
                {
                    grdProduct.DataSource = null;
                    grdProduct.DataBind();
                    DivTop.Visible = false;
                    lblItems.Text = "";
                    trItemsCount.Visible = false;
                    DivBottom.Visible = false;
                }
            }
            if (hdnsearch.Value.ToString() == "1")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@ShowHideDivs", "document.getElementById('divImage').style.display='block';document.getElementById('ContentPlaceHolder1_lblAdvancedSearch').innerHTML = 'Search Home';", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@ShowHideDivs", "document.getElementById('divImage').style.display='none'; document.getElementById('ContentPlaceHolder1_lblAdvancedSearch').innerHTML = 'Advanced Search';", true);
            }
        }

        /// <summary>
        /// Bind Category Drop down
        /// </summary>
        private void FillCategoryDropDown()
        {
            ddlCategory.Items.Clear();
            Int32 storeID = Convert.ToInt32(AppLogic.AppConfigs("StoreId"));
            if (storeID == 0)
            {
                dsCategory = CategoryComponent.GetCategoryByStoreID(1, 3);
            }
            else
            {
                dsCategory = CategoryComponent.GetCategoryByStoreID(Convert.ToInt32(storeID), 3);
            }

            int count = 1;
            ListItem LT2 = new ListItem();
            DataRow[] drCatagories = null;
            if (storeID > 0)
            {
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=0 and StoreID=" + Convert.ToInt32(storeID));
            }
            else
            {
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=0");
            }

            if (dsCategory != null && drCatagories.Length > 0)
            {
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = "...|" + count + "|" + selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), count, storeID);
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("Select Category", "0"));
        }

        /// <summary>
        /// Sets the Child Category
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="Number">int Number</param>
        /// <param name="storeID">int StoreID</param>
        private void SetChildCategory(int ID, int Number, Int32 storeID)
        {
            int count = Number;
            string st = "...";
            for (int i = 0; i < count; i++)
            {
                st += st;
            }
            DataRow[] drCatagories = null;
            if (storeID == 0)
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + ID.ToString());
            else
                drCatagories = dsCategory.Tables[0].Select("ParentCategoryID=" + ID.ToString() + " and StoreID=" + storeID);
            ListItem LT2;
            int innercount = 0;
            if (drCatagories.Length > 0)
            {
                innercount++;
                foreach (DataRow selDR in drCatagories)
                {
                    LT2 = new ListItem();
                    LT2.Text = st + "|" + (count + 1) + "|" + selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), innercount + Number, storeID);
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgButtonSearch_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                CurrentPage = 0;
                BindSearching();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Msg01", "alert('Please enter something to Search');", true);
            }
        }

        /// <summary>
        /// Binds the Searching Details with Insert Search Log
        /// </summary>
        public void BindSearching()
        {
            AddSearchLog();

            DataSet DsSearchValue = new DataSet();
            String CategoryId = "0";
            if (ddlCategory.SelectedIndex > 0)
                CategoryId = ddlCategory.SelectedValue.Trim();
            else
                CategoryId = "0";

            DataSet DsCategory = new DataSet();
            CategoryComponent ctx = new CategoryComponent();
            string StrCategory = "";
            if (ddlCategory.SelectedIndex > 0)
            {
                StrCategory = Convert.ToString(GetCategoryValue(Convert.ToInt32(CategoryId), Convert.ToInt32(1)));
            }
            bool SearchInDescription = false;
            if (rdoDescription.SelectedValue == "true")
                SearchInDescription = true;
            else
                SearchInDescription = false;

            if (rdoPics.SelectedIndex == 1)
                grdProduct.Columns[1].Visible = false;
            else
                grdProduct.Columns[1].Visible = true;

            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                DsSearchValue = ProductComponent.GetSearchDataForProduct(txtSearch.Text.Trim().Replace("'", "''"), txtSalePriceFrm.Text.Trim(), txtSalePriceTo.Text.Trim(), StrCategory, SearchInDescription, 1);
                if (DsSearchValue != null && DsSearchValue.Tables.Count > 0 && DsSearchValue.Tables[0].Rows.Count > 0)
                {
                    if (DsSearchValue.Tables[0].Rows.Count == 1 && (DsCategory == null || DsCategory.Tables.Count == 0 || DsCategory.Tables[0].Rows.Count == 0))
                    {
                        try
                        {
                            // Response.Redirect("/" + DsSearchValue.Tables[0].Rows[0]["MainCategory"].ToString() + "/" + (DsSearchValue.Tables[0].Rows[0]["SEName"].ToString()) + "-" + DsSearchValue.Tables[0].Rows[0]["Productid"].ToString() + ".aspx");
                            Response.Redirect("/" + DsSearchValue.Tables[0].Rows[0]["ProductUrl"].ToString() + "");
                        }
                        catch { }
                    }

                    #region Paging Code
                    pgsource.DataSource = DsSearchValue.Tables[0].DefaultView;
                    pgsource.AllowPaging = true;
                    pgsource.PageSize = 18;
                    pgsource.CurrentPageIndex = CurrentPage;

                    //Store it Total pages value in View state
                    ViewState["totpage"] = pgsource.PageCount;

                    this.lnkbottomprevious.Visible = !pgsource.IsFirstPage;
                    this.lnkbottomNext.Visible = !pgsource.IsLastPage;
                    this.lnkTopPrevious.Visible = !pgsource.IsFirstPage;
                    this.lnkTopNext.Visible = !pgsource.IsLastPage;

                    grdProduct.DataSource = pgsource;
                    grdProduct.DataBind();
                    doPaging();
                    RepeaterPaging.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    dtlTopPaging.ItemStyle.HorizontalAlign = HorizontalAlign.Center;
                    #endregion

                    lblItems.Text = "Item(s) Found : " + DsSearchValue.Tables[0].Rows.Count;
                    trItemsCount.Visible = true;
                    DivTop.Visible = true;
                    DivBottom.Visible = true;

                }
                else
                {
                    grdProduct.DataSource = null;
                    grdProduct.DataBind();
                    DivTop.Visible = false;
                    lblItems.Text = "";
                    trItemsCount.Visible = false;
                    DivBottom.Visible = false;
                }

                #region Serch Category Bind

                try
                {
                    if (ddlCategory.SelectedIndex > 0)
                    {
                        StrCategory = Convert.ToString(GetCategoryValue(Convert.ToInt32(CategoryId), 1));
                    }
                    DsCategory = ctx.GetCategoryForSearch(txtSearch.Text.Trim().Replace("'", "''"), StrCategory, SearchInDescription, 1);
                    if (DsCategory != null && DsCategory.Tables.Count > 0 && DsCategory.Tables[0].Rows.Count > 0)
                    {
                        DataSet DsTemp = new DataSet();
                        DsTemp = DsCategory.Copy();
                        string strName = "";
                        for (Int32 i = 0; i < DsCategory.Tables[0].Rows.Count; i++)
                        {
                            if (strName != DsCategory.Tables[0].Rows[i]["Name"].ToString())
                            {
                            }
                            else
                            {
                                DsCategory.Tables[0].Rows.RemoveAt(i);
                                i--;
                            }
                            strName = DsCategory.Tables[0].Rows[i]["Name"].ToString();
                            DsCategory.Tables[0].AcceptChanges();
                        }

                        rptCategory.DataSource = DsCategory;
                        rptCategory.DataBind();
                        divCategoryList.Visible = true;

                        lblCategoryTotal.Text = "Item(s) Found : " + DsCategory.Tables[0].Rows.Count.ToString();
                    }
                    else
                    {
                        divCategoryList.Visible = false;
                        lblCategoryTotal.Text = "";
                    }
                }
                catch { }
                #endregion

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@Msg01", "alert('Please enter something to Search');", true);
            }
        }

        /// <summary>
        /// Gets the Category Value
        /// </summary>
        /// <param name="CateId">int CateId</param>
        /// <param name="StoreId">int CateId</param>
        /// <returns>Returns the Category Values as a String</returns>
        public string GetCategoryValue(Int32 CateId, Int32 StoreId)
        {
            DataSet DsCate = new DataSet();
            CategoryComponent ctx = new CategoryComponent();
            DsCate = CategoryComponent.GetCategoryByStoreID(StoreId);
            //DsCate = ctx.GetCategoryByStoreID(CateId, StoreId);
            string CateChildList = CateId + ",";
            string StrChildCate = "";
            if (DsCate != null && DsCate.Tables.Count > 0 && DsCate.Tables[0].Rows.Count > 0)
            {
                if (DsCate != null && DsCate.Tables.Count > 0 && DsCate.Tables[0].Rows.Count > 0 && DsCate.Tables[0].Select("ParentCategoryID=" + CateId + "").Length > 0)
                {
                    foreach (DataRow dr in DsCate.Tables[0].Select("ParentCategoryID=" + CateId + ""))
                    {
                        CateChildList += dr["CategoryID"].ToString() + ",";
                        StrChildCate += GetChildCategoryId(Convert.ToInt32(dr["CategoryID"].ToString()), DsCate);
                    }
                }
            }
            CateChildList = CateChildList.Substring(0, CateChildList.Length - 1);
            if (StrChildCate.Length > 0)
            {
                StrChildCate = StrChildCate.Substring(0, StrChildCate.Length - 1);
                return CateChildList = CateChildList + "," + StrChildCate;
            }
            else
            {
                return CateChildList.ToString();
            }
        }

        /// <summary>
        /// Gets the Child Category Id
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <param name="dsCategory">Dataset dsCategory</param>
        /// <returns>Returns the Child CategoryIDs as a String</returns>
        public string GetChildCategoryId(Int32 Id, DataSet dsCategory)
        {
            string StrCateChild = "";
            if (dsCategory != null && dsCategory.Tables.Count > 0 && dsCategory.Tables[0].Rows.Count > 0 && dsCategory.Tables[0].Select("ParentCategoryID=" + Id + "").Length > 0)
            {
                foreach (DataRow dr in dsCategory.Tables[0].Select("ParentCategoryID=" + Id + ""))
                {
                    StrCateChild += dr["CategoryID"].ToString() + ",";
                    GetChildCategoryId(Convert.ToInt32(dr["CategoryID"].ToString()), dsCategory);
                }
            }
            return StrCateChild.ToString();
        }

        /// <summary>
        /// Add '...', if String length is more than 20 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 20 Length String </returns>
        public String SetSKU(String Name)
        {
            if (Name.Length > 20)
                Name = Name.Substring(0, 20) + "...";
            return Name;
        }

        /// <summary>
        /// Add '...', if String length is more than 60 characters
        /// </summary>
        /// <param name="Name">String Name</param>
        /// <returns>Return Max. 60 Length String </returns>
        public String SetName(String Name)
        {
            if (Name.Length > 60)
                Name = Name.Substring(0, 60) + "...";
            return Name;
        }

        /// <summary>
        /// Gets the Micro Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns Micro Image Path</returns>
        public String GetMicroImage(String img)
        {
            String imagepath = String.Empty;
            if (string.IsNullOrEmpty(ProductIconPath))
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
            String Temp = imagepath = ProductIconPath + img;
            imagepath = Temp;
            if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path")+(imagepath)))
            {
                return AppLogic.AppConfigs("Live_Contant_Server")+imagepath;
            }
            else
            {
                return AppLogic.AppConfigs("Live_Contant_Server")+ProductIconPath + "image_not_available.jpg";
            }
        }

        /// <summary>
        /// Paging Data
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

            dtlTopPaging.DataSource = dt;
            dtlTopPaging.DataBind();

        }

        /// <summary>
        /// Get Current Paging
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
                //Set View statevalue when page is changed through Paging "RepeaterPaging" DataList
                ViewState["CurrentPage"] = value;
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
                //Assign CurrentPage number when user click on the page number in the Paging "RepeaterPaging" DataList
                CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
                //Refresh "Repeater1" control Data once user change page                
                BindSearching();
            }
        }

        /// <summary>
        /// Paging Repeater Item DAta Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DataListItemEventArgs e</param>
        protected void RepeaterPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            //Enabled False for current selected Page index
            LinkButton lnkPage = (LinkButton)e.Item.FindControl("Pagingbtn");
            if (lnkPage.CommandArgument.ToString() == CurrentPage.ToString())
            {
                lnkPage.Enabled = false;
                lnkPage.ForeColor = System.Drawing.Color.FromName("#EF6501");
            }
            else
            {
                lnkPage.ForeColor = System.Drawing.Color.FromName("#2A2A2A");
            }

        }

        #region First Last Previous Next

        /// <summary>
        /// If user click Previous Link button assign current index as -1 it reduce existing page index
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            //If user click Previous Link button assign current index as -1 it reduce existing page index.
            CurrentPage -= 1;
            //refresh "Repeater1" Data
            BindSearching();
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
            //refresh "Repeater1" Data
            BindSearching();
        }
        #endregion

        /// <summary>
        /// Gets the Parent Category Details from specified CategoryID
        /// </summary>
        /// <param name="categoryid">int categoryid</param>
        /// <param name="strlink">string strlink</param>
        /// <returns>Returns the Link of Parent Category if Exists as a String</returns>
        private string GetParent(Int32 categoryid, string strlink)
        {
            string str = strlink;

            categoryid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect top 1 ParentCategoryid from tb_CategoryMapping WHERE CategoryID=" + categoryid + ""));
            string SeName = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ISNULL(SEName,'') as SeName FROM tb_Category WHERE Categoryid =" + categoryid + ""));
            if (categoryid > 0)
            {
                str = SeName.ToString() + "/" + str;
                str = GetParent(categoryid, str);
            }
            else
            {
                str = strlink.ToString();
            }
            return str;
        }

        /// <summary>
        /// Category repeater Item Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">RepeaterItemEventArgs e</param>
        protected void rptCategory_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                Label lblCategory = (Label)e.Item.FindControl("lblCategory");
                HtmlAnchor tagsename = (HtmlAnchor)e.Item.FindControl("tagsename");
                Label lblSeName = (Label)e.Item.FindControl("lblSeName");

                string SeName = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ISNULL(SEName,'') as SeName FROM tb_Category WHERE Categoryid in (SElect ParentCategoryid from tb_CategoryMapping WHERE CategoryID=" + lblCategory.Text.Trim() + " And StoreId=" + AppLogic.AppConfigs("StoreId") + ")"));
                Int32 Categoryid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect top 1 ParentCategoryid from tb_CategoryMapping WHERE CategoryID=" + lblCategory.Text.Trim() + ""));
                string StrCate = "";
                if (Categoryid > 0)
                {
                    StrCate = GetParent(Categoryid, SeName);
                    tagsename.HRef = "/" + lblSeName.Text.ToString() + ".html";
                }
                else
                {
                    tagsename.HRef = "/" + lblSeName.Text.ToString() + ".html";
                }
            }
        }

        /// <summary>
        /// insert Search Log
        /// </summary>
        /// <returns>Returns Inserted SearchLogID</returns>
        public void AddSearchLog()
        {
            SearchLogComponent objSearchLog = new SearchLogComponent();
            tb_SearchLog tb_SearchLog = new tb_SearchLog();
            tb_SearchLog.SearchTerm = txtSearch.Text.Trim().Replace("'", "''");
            tb_SearchLog.StoreID = Convert.ToInt32(AppLogic.AppConfigs("StoreID"));
            tb_SearchLog.CreatedOn = DateTime.Now;
            int intSearchLogID = objSearchLog.InsertSearchLog(tb_SearchLog);
            //return intSearchLogID;
        }

        /// <summary>
        /// Product Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSalePrice = (Label)e.Row.FindControl("lblSalePrice");
                Label lblPrice = (Label)e.Row.FindControl("lblPrice");
                Label lblImageName = (Label)e.Row.FindControl("lblImageName");
                Label lblTagName = (Label)e.Row.FindControl("lblTagName");
                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Literal lblTagImage = (Literal)e.Row.FindControl("lblTagImage");
                if (!string.IsNullOrEmpty(lblImageName.Text) && !lblImageName.Text.ToString().ToLower().Contains("image_not_available"))
                {
                    if (lblTagName != null && !string.IsNullOrEmpty(lblTagName.Text.ToString().Trim()) && lblTagName.Text.ToString().ToLower().IndexOf("bestseller") > -1)
                    {
                        //string StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + lblProductID.Text.ToString() + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 ";
                        //Int32 Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                        //if (Intcnt > 0)
                        //{
                        lblTagImage.Text = "<img title='Best Seller' src=\"/images/BestSeller_new.png\" alt=\"Best Seller\" class='bestseller' />";
                        //}
                    }
                    else if (lblTagName != null && !string.IsNullOrEmpty(lblTagName.Text.ToString().Trim()))
                    {
                        lblTagImage.Text = "<img  title='" + lblTagName.Text.ToString().Trim() + "' src=\"/images/" + lblTagName.Text.ToString().Trim() + ".jpg\" alt=\"" + lblTagName.Text.ToString().Trim() + "\" class='" + lblTagName.Text.ToString().ToLower() + "'   />";
                    }
                    else
                    {


                        string StrQry = "Select COUNT(VariantValueID) from tb_ProductVariantValue Where productid=" + lblProductID.Text.ToString() + " and (cast(OnSaleFromdate as date) <=  cast(GETDATE() as date) and cast(OnSaleTodate as date) >=cast(GETDATE() as date)) and ISNULL(OnSale,0)=1 ";
                        Int32 Intcnt = Convert.ToInt32(CommonComponent.GetScalarCommonData(StrQry.ToString()));
                        if (Intcnt > 0)
                        {
                            lblTagImage.Text = "<img title='Sale' src=\"/images/bestseller.png\" alt=\"Sale\" class='bestseller' />";
                        }
                    }
                }

                
                    
                if (Convert.ToDecimal(lblSalePrice.Text) > decimal.Zero && Convert.ToDecimal(lblPrice.Text) > Convert.ToDecimal(lblSalePrice.Text))
                {
                    lblSalePrice.Text = string.Format("{0:0.00}", Convert.ToDecimal(lblSalePrice.Text));
                }
                else
                {
                    lblSalePrice.Text = string.Format("{0:0.00}", Convert.ToDecimal(lblPrice.Text));
                }
            }
        }
    }
}