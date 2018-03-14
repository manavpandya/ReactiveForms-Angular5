using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class PopupCategory : BasePage
    {
        public DataTable dt;
        DataSet dsCategory = new DataSet();
        PagedDataSource pgsource = new PagedDataSource();
        int findex, lindex;
        DataColumn dcMembershipDiscountID;
        DataColumn dcCategoryId;
        DataColumn dcDiscount;
        DataColumn dcProductname;

        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            btnGo.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/search.gif";
            btnShowAll.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/showall.png";
            btnAddToSelectionlist.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";
            btnAddSelectedItems.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save-changes.png";

            if (!IsPostBack)
            {
                // FillCategoryDropDown();

                BindSearchData();
                TempTable();

                if (Request.QueryString["StoreID"] != null && Request.QueryString["clientid"].ToString() != null && !string.IsNullOrEmpty(Request.QueryString["clientid"].ToString()))
                {
                    string IDs = "";
                    if (Request.QueryString["IDs"] != null && !string.IsNullOrEmpty(Request.QueryString["IDs"].ToString()))
                    {
                        IDs = Request.QueryString["IDs"].ToString();
                    }
                    if (!string.IsNullOrEmpty(IDs))
                    {
                        ViewState["SelectedCatIds"] = IDs;
                    }
                    else
                        ViewState["SelectedCatIds"] = "";
                }
                else
                {
                    //Page.ClientQueryString. = "Cookies is not Enable in this computer please Enable cookies to work properly.";
                    ViewState["SelectedCatIds"] = "";
                }
                Fillgrid();
            }

        }

        /// <summary>
        /// Initial Table
        /// </summary>
        private void TempTable()
        {
            dt = new DataTable();
            System.Data.DataColumn RoColm = new System.Data.DataColumn("RowNumber", typeof(int));
            dt.Columns.Add(RoColm);
            dcMembershipDiscountID = new System.Data.DataColumn("MembershipDiscountID", typeof(String));
            dt.Columns.Add(dcMembershipDiscountID);
            dcCategoryId = new System.Data.DataColumn("CategoryId", typeof(String));
            dt.Columns.Add(dcCategoryId);
            dcDiscount = new System.Data.DataColumn("CategoryDiscount", typeof(String));
            dt.Columns.Add(dcDiscount);
            dcProductname = new System.Data.DataColumn("Productname", typeof(String));
            dt.Columns.Add(dcProductname);
        }

        /// <summary>
        /// function for Bind Category Dropdown
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

                    if (count == 1)
                    {
                        LT2.Text = "|" + count + "|" + selDR["Name"].ToString();
                    }
                    else
                        LT2.Text = "...|" + count + "|" + selDR["Name"].ToString();
                    LT2.Value = selDR["CategoryID"].ToString();
                    ddlCategory.Items.Add(LT2);
                    SetChildCategory(Convert.ToInt32(selDR["CategoryID"].ToString()), count, storeID);
                }
            }
            ddlCategory.Items.Insert(0, new ListItem("Select Category", "0"));
        }

        /// <summary>
        /// Function for Set Child Category
        /// </summary>
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
        /// function for Get Category Value
        /// </summary>
        public string GetCategoryValue(Int32 CateId, Int32 StoreId)
        {
            DataSet DsCate = new DataSet();
            CategoryComponent ctx = new CategoryComponent();
            DsCate = CategoryComponent.GetCategoryByStoreID(StoreId);
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
        /// Function for Get Child Category ID
        /// </summary>
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
        /// Function for Bind Search Data
        /// </summary>
        protected void BindSearchData()
        {
            string StrCategory = "";
            String CategoryId = "0";
            //if (ddlCategory.SelectedIndex > 0)
            //    CategoryId = ddlCategory.SelectedValue.Trim();
            //else
            //    CategoryId = "0";

            //if (ddlCategory.SelectedIndex > 0)
            //{
            //    StrCategory = Convert.ToString(GetCategoryValue(Convert.ToInt32(CategoryId), Convert.ToInt32(AppConfig.StoreID)));
            //}

            CategoryComponent objCatComp = new CategoryComponent();
            DataSet dsSearch = new DataSet();
            string WhrClus = "";
            if(txtSearch.Text.ToString()!="")
            {
                WhrClus = " and name like '%" + txtSearch.Text.ToString().Replace("'", "''") + "%'";
            }
            if (Request.QueryString["mode"].ToString() == "2")
            {
                lblHeader.Text = "Search Category(s)";
                if (StrCategory != "")
                {
                    // if (Request.QueryString["StoreId"] != null)
                    //  WhrClus += " Where " + " CategoryID  in (" + StrCategory + ") and  CategoryID in (SELECT CategoryID FROM dbo.tb_CategoryMapping WHERE CategoryID in(" + StrCategory + ") and  isnull(ParentCategoryID,0)<>0 ) and  isnull(deleted,0)=0 and isnull(active,0)=1 and StoreID =" + Request.QueryString["StoreID"].ToString() + " and DiscountType = 'category')";
                    // if (Request.QueryString["CustID"] != null)
                    //  WhrClus += " Where " + " CategoryID  in (" + StrCategory + ") and  CategoryID in (SELECT CategoryID FROM dbo.tb_CategoryMapping WHERE CategoryID in(" + StrCategory + ") and  isnull(ParentCategoryID,0)<>0 ) and  isnull(deleted,0)=0 and isnull(active,0)=1 and StoreID =" + Request.QueryString["StoreID"].ToString() + "  and CategoryID not in (Select DiscountObjectID from tb_ResellerDiscount where CustomerID =" + Request.QueryString["CustId"].ToString() + " and DiscountType = 'category')";
                    // else if (Request.QueryString["CustomerLevelID"] != null)
                    //   WhrClus += " Where " + " CategoryID  in (" + StrCategory + ") and  CategoryID in (SELECT CategoryID FROM dbo.tb_CategoryMapping WHERE CategoryID in(" + StrCategory + ") and  isnull(ParentCategoryID,0)<>0 ) and  isnull(deleted,0)=0 and isnull(active,0)=1 and StoreID =" + Request.QueryString["StoreID"].ToString() + "  and CategoryID not in (Select DiscountObjectID from tb_MembershipDiscount where CustomerLevelID =" + Request.QueryString["CustomerLevelID"].ToString() + " and DiscountType = 'category')";
                }
                else
                {
                    // if (Request.QueryString["StoreId"] != null)
                    //   WhrClus = " where  isnull(deleted,0)=0 and isnull(active,0)=1 and CategoryID in (SELECT CategoryID FROM dbo.tb_CategoryMapping WHERE ParentCategoryID=0 ) and  DiscountType = 'category')";


                    //if (Request.QueryString["CustID"] != null)
                    //    WhrClus = " where  isnull(deleted,0)=0 and isnull(active,0)=1 and CategoryID in (SELECT CategoryID FROM dbo.tb_CategoryMapping WHERE ParentCategoryID=0 ) and CategoryID not in (Select DiscountObjectID from tb_ResellerDiscount where CustomerID =" + Request.QueryString["CustId"].ToString() + " and DiscountType = 'category')";
                    //else if (Request.QueryString["CustomerLevelID"] != null)
                    //    WhrClus = " where  isnull(deleted,0)=0 and isnull(active,0)=1 and CategoryID in (SELECT CategoryID FROM dbo.tb_CategoryMapping WHERE ParentCategoryID=0 ) and CategoryID not in (Select DiscountObjectID from tb_MembershipDiscount where CustomerLevelID =" + Request.QueryString["CustomerLevelID"].ToString() + " and DiscountType = 'category')";
                }
                dsSearch = objCatComp.GetSearchCategoryValue(Convert.ToInt32(Request.QueryString["StoreId"]), WhrClus, 2);

            }

            if (dsSearch != null && dsSearch.Tables.Count > 0 && dsSearch.Tables[0].Rows.Count > 0)
            {
                grdProduct.DataSource = dsSearch;
                grdProduct.DataBind();

                trCheckClearAll.Visible = true;
                btnAddToSelectionlist.Visible = true;
                // lblTotProducts.InnerText = "Category(s) Found : " + dsSearch.Tables[0].Rows.Count.ToString();
            }
            else
            {
                grdProduct.DataSource = null;
                grdProduct.DataBind();
                trCheckClearAll.Visible = false;
                btnAddToSelectionlist.Visible = false;
                // lblTotProducts.InnerText = "";
            }
        }

        /// <summary>
        /// Add to list
        /// </summary>
        /// <param name="lblSKU">string lblSKU</param>
        private void AddtoList(string Ids)
        {
            try
            {
                string list = "";
                if (ViewState["SelectedCatIds"] != null)
                {
                    list = ViewState["SelectedCatIds"].ToString();
                    if (!string.IsNullOrEmpty(Ids) && !list.Contains(Ids + ","))
                    {
                        list += Ids + ",";
                    }
                }
                else ViewState["SelectedCatIds"] = "";

                ViewState["SelectedCatIds"] = list;
            }
            catch { }
        }

        /// <summary>
        /// Click Event for Go Button
        /// </summary>
        /// 
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            if (txtSearch.Text.ToString() != "")
            {

                BindSearchData();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Please Select Category');", true);
            }
        }

        /// <summary>
        /// Click Event for Show All button
        /// </summary>
        protected void btnShowAll_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
           // ddlCategory.SelectedIndex = 0;
            BindSearchData();
        }

        /// <summary>
        /// Click Event for Add To Selection List Button
        /// </summary>
        protected void btnAddToSelectionlist_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TempTable();
                Session["TempDsCatDiscount"] = null;
                if (grdProduct.Rows.Count > 0)
                {
                    int MembershipDiscountID = 1;
                    if (ViewState["dtTemp"] != null)
                    {
                        dt = null;
                        dt = (DataTable)ViewState["dtTemp"];
                    }
                    int cnt = 0;
                    decimal discount_out = 0;
                    foreach (GridViewRow gvr in grdProduct.Rows)
                    {
                        Label lblCategoryid = (Label)gvr.FindControl("lblCategoryID");
                        Label lblProductName = (Label)gvr.FindControl("lblProductName");
                        String Ids = Convert.ToString(((Label)gvr.FindControl("lblCategoryID")).Text.ToString());
                        Boolean chkSelect = Convert.ToBoolean(((CheckBox)gvr.FindControl("chkSelect")).Checked);
                        if (chkSelect)
                        {
                            cnt++;
                            if (decimal.TryParse((((TextBox)gvr.FindControl("txtDiscount")).Text.ToString()), out discount_out))
                            {
                                AddtoList(Ids);
                                DataRow[] drlength = dt.Select("CategoryId=" + lblCategoryid.Text.ToString() + "");
                                if (dt != null && drlength.Length == 0)
                                {
                                    DataRow dr;
                                    Int32 icount = 1;
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        icount = dt.Rows.Count + 1;
                                    }
                                    dr = dt.NewRow();
                                    dr["RowNumber"] = icount;
                                    dr["CategoryId"] = lblCategoryid.Text;
                                    dr["Productname"] = lblProductName.Text;
                                    dr["CategoryDiscount"] = discount_out;
                                    dt.Rows.Add(dr);
                                    dt.AcceptChanges();
                                    MembershipDiscountID++;
                                }
                                else
                                {
                                    if (drlength.Length > 0)
                                    {
                                        for (int k = 0; k < dt.Rows.Count; k++)
                                        {
                                            if (lblCategoryid.Text.ToString() == dt.Rows[k]["CategoryId"].ToString())
                                            {
                                                dt.Rows[k]["CategoryDiscount"] = discount_out;
                                            }
                                        }
                                        dt.AcceptChanges();
                                    }
                                }
                            }
                            CheckBox chks = (CheckBox)gvr.FindControl("chkSelect");
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@sdsdf" + cnt.ToString(), "document.getElementById('" + chks.ClientID.ToString() + "').checked=false;document.getElementById('" + chks.ClientID.ToString().Replace("chkSelect", "txtDiscount") + "').value='0.00';", true);
                        }
                    }
                    ViewState["dtTemp"] = dt;
                    Session["TempDsCatDiscount"] = dt;
                    string CatIds = ViewState["SelectedCatIds"].ToString();
                    Fillgrid();
                    Page.ClientScript.RegisterClientScriptBlock(btnAddToSelectionlist.GetType(), "@closemsg1", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + CatIds + "';window.opener.document.getElementById('ContentPlaceHolder1_btnCustDiscountDetailid').click();", true);
                }
            }
            catch { }
        }

        /// <summary>
        /// grdProduct_RowDataBound
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            decimal discount_out = 0;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {


                if (Session["TempDsCatDiscount"] != null)
                {
                    dt = (DataTable)Session["TempDsCatDiscount"];

                    Label lblCategoryID = (Label)e.Row.FindControl("lblCategoryID");
                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    TextBox txtDiscount = (TextBox)e.Row.FindControl("txtDiscount");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (lblCategoryID != null)
                        {
                            if (lblCategoryID.Text == dt.Rows[i]["Categoryid"].ToString())
                            {
                                // chkSelect.Checked = true;
                                decimal.TryParse(dt.Rows[i]["CategoryDiscount"].ToString(), out discount_out);
                                //  txtDiscount.Text = Math.Round(discount_out, 2).ToString();
                            }
                        }
                    }
                }

            }
        }

        /// <summary>
        /// Bind Selected Category List
        /// </summary>
        private void Fillgrid()
        {
            DataTable dtGrid = new DataTable();
            if (Session["TempDsCatDiscount"] != null)
            {
                dtGrid = (DataTable)Session["TempDsCatDiscount"];
                ViewState["dtTemp"] = dtGrid;
                grdSelected.DataSource = dtGrid;
                grdSelected.DataBind();
                if (dtGrid != null && dtGrid.Rows.Count > 0)
                {
                    trAddSeletedItems.Visible = true;
                    btnAddSelectedItems.Visible = true;
                }
                else
                {
                    trAddSeletedItems.Visible = false;
                    btnAddSelectedItems.Visible = false;
                }
            }
            else
            {
                ViewState["dtTemp"] = null;
                Session["TempDsCatDiscount"] = null;
                grdSelected.DataSource = null;
                grdSelected.DataBind();
                trAddSeletedItems.Visible = false;
                btnAddSelectedItems.Visible = false;
            }
        }

        /// <summary>
        /// Selected Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdSelected_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "delMe")
            {
                Int32 RowInd = Convert.ToInt32(e.CommandArgument.ToString());
                DataTable dtGrid = new DataTable();
                if (Session["TempDsCatDiscount"] != null)
                {
                    dtGrid = (DataTable)Session["TempDsCatDiscount"];
                    DataRow[] dr = dtGrid.Select("RowNumber=" + RowInd + "");
                    dtGrid.Rows.Remove(dr[0]);
                    dtGrid.AcceptChanges();
                }
                Session["TempDsCatDiscount"] = dtGrid;
                Fillgrid();
                if (ViewState["SelectedCatIds"] != null)
                {
                    string CatIds = Convert.ToString(ViewState["SelectedCatIds"]);
                    Page.ClientScript.RegisterClientScriptBlock(grdSelected.GetType(), "@closemsg3", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + CatIds + "';window.opener.document.getElementById('ContentPlaceHolder1_btnCustDiscountDetailid').click();", true);
                }
            }
        }

        /// <summary>
        /// Selected Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdSelected_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ImageButton btndel = (ImageButton)e.Row.FindControl("btndel");
                btndel.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
            }
        }

        /// <summary>
        ///  Add to Selection Item Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddSelectedItems_Click(object sender, ImageClickEventArgs e)
        {
            if (grdSelected.Rows.Count > 0)
            {
                try
                {
                    if (ViewState["SelectedCatIds"] != null)
                    {
                        string CatIds = Convert.ToString(ViewState["SelectedCatIds"]);
                        Page.ClientScript.RegisterClientScriptBlock(btnAddSelectedItems.GetType(), "@closemsg5", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + CatIds + "';window.close();window.opener.document.getElementById('ContentPlaceHolder1_btnCustDiscountDetailid').click();", true);
                    }
                }
                catch { }
            }
        }
    }
}


