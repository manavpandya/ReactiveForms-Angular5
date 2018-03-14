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
    public partial class PopUpProduct1 : BasePage
    {
        public DataTable dt;
        PagedDataSource pgsource = new PagedDataSource();
        int findex, lindex;



        DataColumn dcMembershipDiscountID;
        DataColumn dcProductId;
        DataColumn dcProductSku;
        DataColumn dcDiscount;
        DataColumn dcProductname;


        #region Page Load
        /// <summary>
        /// Page Load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                btnGo.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/search.gif";
                btnShowAll.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/showall.png";
                btnAddToSelectionlist.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";
                btnAddSelectedItems.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save-changes.png";
                TempTable();
                BindSearchData();
                if (Request.QueryString["StoreID"] != null && Request.QueryString["clientid"].ToString() != null && !string.IsNullOrEmpty(Request.QueryString["clientid"].ToString()))
                {
                    string IDs = "";
                    if (Request.QueryString["IDs"] != null && !string.IsNullOrEmpty(Request.QueryString["IDs"].ToString()))
                    {
                        IDs = Request.QueryString["IDs"].ToString();
                    }
                    if (!string.IsNullOrEmpty(IDs))
                    {
                        ViewState["SelectedProdIds"] = IDs;
                    }
                    else
                        ViewState["SelectedProdIds"] = "";
                }
                else
                {
                    //Page.ClientQueryString. = "Cookies is not Enable in this computer please Enable cookies to work properly.";
                    ViewState["SelectedProdIds"] = "";
                }
                Fillgrid();
                txtSearch.Text = "";
                txtSearch.Focus();
            }
        }

        #endregion

        #region Re bind All Skus

        private void addallarryitem(string SKUs)
        {
            try
            {
                string tmpStr = "";
                if (!string.IsNullOrEmpty(SKUs))
                {
                    string[] tmp = SKUs.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int cnt = 0; cnt < tmp.Length; cnt++)
                    {
                        tmpStr += tmp[cnt] + ",";
                    }

                }
                ViewState["SelectedProdIds"] = tmpStr;

            }
            catch { }
        }
        #endregion

        #region Search Data

        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
            {

                BindSearchData();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Please enter search keyword');", true);
            }
        }
        #endregion

        #region Bind Search Data

        protected void BindSearchData()
        {
            DataSet dsSearch = new DataSet();
            string WhrClus = "";
            if (ddlSearchby.SelectedValue == "Category")
            {
                dsSearch = ProductComponent.GetSearchProductVal(Convert.ToInt32(Request.QueryString["StoreId"]), WhrClus);
            }
            else
            {
                if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
                {
                    WhrClus += " Where " + ddlSearchby.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Trim() + "%' And ISNULL(tb_Product.Deleted,0) != 1 And ISNULL(tb_Product.Active,0)=1 and (tb_Product.StoreId = " + Request.QueryString["StoreId"] + ")";
                    //WhrClus += " Where " + ddlSearchby.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Trim() + "%' And ISNULL(tb_Product.Deleted,0) != 1 And ISNULL(tb_Product.Active,0)=1 and (tb_Product.StoreId = " + Request.QueryString["StoreId"] + ") and ProductID not in (Select DiscountObjectID from tb_ResellerDiscount where CustomerID =" + Request.QueryString["CustId"].ToString() + " and DiscountType = 'product')";
                    //    else if (Request.QueryString["CustomerLevelID"] != null)
                    //        WhrClus += " Where " + ddlSearchby.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Trim() + "%' And ISNULL(tb_Product.Deleted,0) != 1 And ISNULL(tb_Product.Active,0)=1 and (tb_Product.StoreId = " + Request.QueryString["StoreId"] + ") and ProductID not in (Select DiscountObjectID from tb_MembershipDiscount where CustomerLevelID =" + Request.QueryString["CustomerLevelID"].ToString() + " and DiscountType = 'product')";
                }
                else
                {
                    WhrClus = " where ISNULL(Deleted,0) != 1 And ISNULL(Active,0)=1 and (StoreId = " + Request.QueryString["StoreId"] + ")";
                    //if (Request.QueryString["CustID"] != null)
                    //    WhrClus = " where ISNULL(Deleted,0) != 1 And ISNULL(Active,0)=1 and (StoreId = " + Request.QueryString["StoreId"] + ") and ProductID not in (Select DiscountObjectID from tb_ResellerDiscount where CustomerID =" + Request.QueryString["CustId"].ToString() + " and DiscountType = 'product')";
                    //else if (Request.QueryString["CustomerLevelID"] != null)
                    // WhrClus = " where ISNULL(Deleted,0) != 1 And ISNULL(Active,0)=1 and (StoreId = " + Request.QueryString["StoreId"] + ") and ProductID not in (Select DiscountObjectID from tb_MembershipDiscount where CustomerLevelID =" + Request.QueryString["CustomerLevelID"].ToString() + " and DiscountType = 'product')";
                }

                dsSearch = ProductComponent.GetSearchProductVal(Convert.ToInt32(Request.QueryString["StoreId"]), WhrClus);
            }
            if (dsSearch != null && dsSearch.Tables.Count > 0 && dsSearch.Tables[0].Rows.Count > 0)
            {
                grdProduct.DataSource = dsSearch;
                grdProduct.DataBind();
                trCheckClearAll.Visible = true;
                btnAddToSelectionlist.Visible = true;
                // lblTotProducts.InnerText = "Product(s) Found : " + dsSearch.Tables[0].Rows.Count.ToString();
            }
            else
            {
                grdProduct.DataSource = null;
                grdProduct.DataBind();
                trCheckClearAll.Visible = false;
                btnAddToSelectionlist.Visible = false;
                //lblTotProducts.InnerText = "";
            }
        }

        #endregion

        /// <summary>
        /// Paging Data
        /// </summary>





        private void TempTable()
        {
            dt = new DataTable();
            System.Data.DataColumn RoColm = new System.Data.DataColumn("RowNumber", typeof(int));
            dt.Columns.Add(RoColm);
            dcMembershipDiscountID = new System.Data.DataColumn("MembershipDiscountID", typeof(String));
            dt.Columns.Add(dcMembershipDiscountID);
            dcProductId = new System.Data.DataColumn("ProductId", typeof(String));
            dt.Columns.Add(dcProductId);
            dcProductSku = new System.Data.DataColumn("Sku", typeof(String));
            dt.Columns.Add(dcProductSku);

            dcDiscount = new System.Data.DataColumn("ProductDiscount", typeof(String));
            dt.Columns.Add(dcDiscount);
            dcProductname = new System.Data.DataColumn("Name", typeof(String));
            dt.Columns.Add(dcProductname);
        }
        private void Fillgrid()
        {
            DataTable dtGrid = new DataTable();
            if (Session["TempDsProdDiscount"] != null)
            {
                dtGrid = (DataTable)Session["TempDsProdDiscount"];
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
                Session["TempDsProdDiscount"] = null;
                grdSelected.DataSource = null;
                grdSelected.DataBind();
                trAddSeletedItems.Visible = false;
                btnAddSelectedItems.Visible = false;
                
            }
        }
        protected void btnAddToSelectionlist_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TempTable();
                Session["TempDsProdDiscount"] = null;
                if (grdProduct.Rows.Count > 0)
                {
                    int MembershipDiscountID = 1;

                    decimal discount_out = 0;
                    if (ViewState["dtTemp"] != null)
                    {
                        dt = null;
                        dt = (DataTable)ViewState["dtTemp"];
                    }
                    int cnt = 0;
                    foreach (GridViewRow gvr in grdProduct.Rows)
                    {
                        Label lblProductID = (Label)gvr.FindControl("lblProductID");
                        Label lblProductName = (Label)gvr.FindControl("lblProductName");
                        Label lblSKU = (Label)gvr.FindControl("lblSKU");

                        String Ids = Convert.ToString(((Label)gvr.FindControl("lblProductID")).Text.ToString());
                        Boolean chkSelect = Convert.ToBoolean(((CheckBox)gvr.FindControl("chkSelect")).Checked);
                        if (chkSelect)
                        {
                            cnt++;
                            if (decimal.TryParse((((TextBox)gvr.FindControl("txtDiscount")).Text.ToString()), out discount_out))
                            {
                                AddtoList(Ids);
                                DataRow[] drlength = dt.Select("ProductID=" + lblProductID.Text.ToString() + "");
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
                                    dr["MembershipDiscountID"] = MembershipDiscountID;
                                    dr["ProductID"] = lblProductID.Text;
                                    dr["Name"] = lblProductName.Text;
                                    dr["Sku"] = lblSKU.Text;
                                    dr["ProductDiscount"] = discount_out;
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
                                            if (lblProductID.Text.ToString() == dt.Rows[k]["ProductId"].ToString())
                                            {
                                                dt.Rows[k]["ProductDiscount"] = discount_out;
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
                    Session["TempDsProdDiscount"] = dt;
                    Fillgrid();
                    Page.ClientScript.RegisterClientScriptBlock(btnAddToSelectionlist.GetType(), "@closemsg6", "window.opener.document.getElementById('ContentPlaceHolder1_btnProdDiscountDetailid').click();", true);
                }
            }
            catch { }

        }

        /// <summary>
        ///  Add Tto Selection Item Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddSelectedItems_Click(object sender, ImageClickEventArgs e)
        {
            if (grdSelected.Rows.Count > 0)
            {
                try
                {
                    Page.ClientScript.RegisterClientScriptBlock(btnAddSelectedItems.GetType(), "@closemsg1", "window.close();window.opener.document.getElementById('ContentPlaceHolder1_btnProdDiscountDetailid').click();", true);
                }
                catch { }
            }
        }

        private void AddtoList(string lblSKU)
        {
            try
            {
                string list = "";
                if (ViewState["SelectedProdIds"] != null)
                {
                    list = ViewState["SelectedProdIds"].ToString();
                    if (!string.IsNullOrEmpty(lblSKU) && !list.Contains(lblSKU + ","))
                    {
                        list += lblSKU + ",";
                    }
                }
                else ViewState["SelectedProdIds"] = "";

                ViewState["SelectedProdIds"] = list;
            }
            catch { }
        }

        protected void btnShowAll_Click(object sender, ImageClickEventArgs e)
        {
            BindSearchData();
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
                if (Session["TempDsProdDiscount"] != null)
                {
                    dtGrid = (DataTable)Session["TempDsProdDiscount"];
                    DataRow[] dr = dtGrid.Select("RowNumber=" + RowInd + "");
                    dtGrid.Rows.Remove(dr[0]);
                    dtGrid.AcceptChanges();
                }
                Session["TempDsProdDiscount"] = dtGrid;
                Fillgrid();
                Page.ClientScript.RegisterClientScriptBlock(grdSelected.GetType(), "@closemsg5", "window.opener.document.getElementById('ContentPlaceHolder1_btnProdDiscountDetailid').click();", true);
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

        protected void grdProduct_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            Decimal discount_out;
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (Session["TempDsProdDiscount"] != null)
                {
                    dt = (DataTable)Session["TempDsProdDiscount"];
                    Label lblCategoryID = (Label)e.Row.FindControl("lblProductID");
                    CheckBox chkSelect = (CheckBox)e.Row.FindControl("chkSelect");
                    TextBox txtDiscount = (TextBox)e.Row.FindControl("txtDiscount");

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (lblCategoryID != null)
                        {
                            if (lblCategoryID.Text == dt.Rows[i]["ProductId"].ToString())
                            {
                                //chkSelect.Checked = true;
                                decimal.TryParse(dt.Rows[i]["ProductDiscount"].ToString(), out discount_out);
                                //txtDiscount.Text = Math.Round(discount_out, 2).ToString();
                            }
                        }
                    }
                }
            }
        }
    }
}