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
    public partial class PopUpProduct : BasePage
    {
        PagedDataSource pgsource = new PagedDataSource();
        int findex, lindex;


        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnGo.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/search.gif";
                btnShowAll.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/showall.png";
                btnAddToSelectionlist.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.gif";
                btnAddSelectedItems.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/save-changes.png";

                txtSearch.Text = "";
                txtSearch.Focus();

                if (Request.QueryString["StoreID"] != null)
                {
                    HttpCookie myCookie = new HttpCookie("prskus");
                    myCookie = Request.Cookies["prskus"];
                }
            }
        }

        /// <summary>
        /// Add all array items the specified SKUs
        /// </summary>
        /// <param name="SKUs">string SKUs</param>
        private void addallarryitem(string SKUs)
        {
            try
            {
                string tmpStr = "";
                if (!string.IsNullOrEmpty(SKUs))
                {
                    string[] tmp = SKUs.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int cnt = 0; cnt < tmp.Length; cnt++)
                        tmpStr += tmp[cnt] + ",";
                    if (tmp.Length > 0)
                        BindSelectedData();
                }
                ViewState["SelectedSKUs"] = tmpStr;
                BindSelectedData();
            }
            catch { }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
            {
                CurrentPage = 0;
                BindSearchData();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg01", "alert('Please enter search keyword');", true);
            }
        }

        /// <summary>
        /// Binds the search data.
        /// </summary>
        protected void BindSearchData()
        {
            DataSet dsSearch = new DataSet();
            string WhrClus = "";
            if (ddlSearchby.SelectedValue == "Category")
            {
                WhrClus += " where c.Name like '%" + txtSearch.Text.ToString().Trim() + "%'" + " and  (ISNULL(p.Deleted,0) != 1) And (ISNULL(p.Active,1) = 1) and (p.StoreId = " + Request.QueryString["StoreId"] + ") and (ISNULL(p.Price,0) >0 or ISNULL(p.SalePrice,0) >0)";
                dsSearch = ProductComponent.GetSearchProductValforCategory(Convert.ToInt32(Request.QueryString["StoreId"]), WhrClus, 2);
            }
            else
            {
                if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
                {
                    WhrClus += " Where " + ddlSearchby.SelectedValue.ToString() + " like '%" + txtSearch.Text.ToString().Trim() + "%' And (ISNULL(tb_Product.Deleted,0) != 1) And (ISNULL(tb_Product.Active,1) = 1) and (tb_Product.StoreId = " + Request.QueryString["StoreId"] + ") and (ISNULL(Price,0) >0 or ISNULL(SalePrice,0) >0) ";
                }
                else
                    WhrClus = " where  ISNULL(Deleted,0) != 1 And ISNULL(Active,1) = 1 and (StoreId = " + Request.QueryString["StoreId"] + ") and (ISNULL(Price,0) >0 or ISNULL(SalePrice,0) >0) ";
                dsSearch = ProductComponent.GetSearchProductValforCategory(Convert.ToInt32(Request.QueryString["StoreId"]), WhrClus, 1);
            }
            if (dsSearch != null && dsSearch.Tables.Count > 0 && dsSearch.Tables[0].Rows.Count > 0)
            {
                #region Paging Code

                pgsource.DataSource = dsSearch.Tables[0].DefaultView;
                pgsource.AllowPaging = true;
                if (ViewState["All"] == null)
                {
                    pgsource.PageSize = 15;
                    divTopPaging.Visible = true;
                }
                else
                {
                    CurrentPage = 0;
                    divTopPaging.Visible = false;
                    pgsource.PageSize = dsSearch.Tables[0].Rows.Count;
                }
                pgsource.CurrentPageIndex = CurrentPage;
                ViewState["totpage"] = pgsource.PageCount;
                this.lnkTopprevious.Visible = !pgsource.IsFirstPage;
                this.lnktopNext.Visible = !pgsource.IsLastPage;
                grdProduct.DataSource = pgsource;
                grdProduct.DataBind();
                doPaging();
                RepeaterPaging.ItemStyle.HorizontalAlign = HorizontalAlign.Center;

                #endregion

                trCheckClearAll.Visible = true;
                trPaging.Visible = true;
                btnAddToSelectionlist.Visible = true;

                lblTotProducts.InnerText = "Product(s) Found : " + dsSearch.Tables[0].Rows.Count.ToString();
            }
            else
            {
                grdProduct.DataSource = null;
                grdProduct.DataBind();
                trCheckClearAll.Visible = false;
                trPaging.Visible = false;
                btnAddToSelectionlist.Visible = false;
                lblTotProducts.InnerText = "";
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
                for (int i = findex; i < lindex; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr[0] = i;
                    dr[1] = i + 1;
                    dt.Rows.Add(dr);
                }
            }
            RepeaterPaging.DataSource = dt;
            RepeaterPaging.DataBind();
        }

        /// <summary>
        /// Get Current Paging
        /// </summary>
        private int CurrentPage
        {
            get
            {
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
                ViewState["CurrentPage"] = value;
            }
        }

        /// <summary>
        /// Paging Repeater Item Command
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">DataListCommandEventArgs e</param>
        protected void RepeaterPaging_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("newpage"))
            {
                CurrentPage = Convert.ToInt32(e.CommandArgument.ToString());
                BindSearchData();
            }
        }

        /// <summary>
        /// If user click Previous Link button assign current index as -1 it reduce existing page index.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkPrevious_Click(object sender, EventArgs e)
        {
            CurrentPage -= 1;
            BindSearchData();
        }

        /// <summary>
        /// If user click Next Link button assign current index as +1 it add one value to existing page index.
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void lnkNext_Click(object sender, EventArgs e)
        {
            CurrentPage += 1;
            BindSearchData();
        }

        /// <summary>
        /// Paging Repeater Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void RepeaterPaging_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            LinkButton lnkPage = (LinkButton)e.Item.FindControl("Pagingbtn");
            if (lnkPage.CommandArgument.ToString() == CurrentPage.ToString())
            {
                lnkPage.Enabled = false;
                lnkPage.ForeColor = System.Drawing.Color.FromName("#FF0000");
            }
            else
            {
                lnkPage.ForeColor = System.Drawing.Color.FromName("#2A2A2A");
            }
        }

        /// <summary>
        ///  Add to Selection list Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddToSelectionlist_Click(object sender, ImageClickEventArgs e)
        {
            if (grdProduct.Rows.Count > 0)
            {
                bool res = false;
                for (int i = 0; i < grdProduct.Rows.Count; i++)
                {
                    Boolean chkSelect = Convert.ToBoolean(((CheckBox)grdProduct.Rows[i].FindControl("chkSelect")).Checked);
                    int Quantity_out = 0;
                    CustomerComponent objCustComp = new CustomerComponent();
                    if (chkSelect)
                    {
                        if (int.TryParse((((TextBox)grdProduct.Rows[i].FindControl("txtQuantity")).Text.ToString()), out Quantity_out))
                        {
                            if (!string.IsNullOrEmpty(((TextBox)grdProduct.Rows[i].FindControl("txtQuantity")).Text.ToString()) && Quantity_out > 0)
                            {
                                int ProductId = Convert.ToInt32(((Label)grdProduct.Rows[i].FindControl("lblProductID")).Text.ToString());
                                int Availnventory = Convert.ToInt32(((Label)grdProduct.Rows[i].FindControl("lblInventory")).Text.ToString());
                                decimal Price = Convert.ToDecimal(((Label)grdProduct.Rows[i].FindControl("lblPrice")).Text.ToString());
                                string Name = Convert.ToString(((Label)grdProduct.Rows[i].FindControl("lblProductName")).Text.ToString());
                                string Sku = Convert.ToString(((Label)grdProduct.Rows[i].FindControl("lblSKU")).Text.ToString());

                                Int32 CountQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect ISNULL(SUM(Inventory),0) as TotQty from tb_WareHouseProduct where ProductID=" + ProductId + " and StoreID=" + AppLogic.AppConfigs("StoreID") + ""));
                                if (CountQty > 0)
                                {
                                    Quantity_out = Quantity_out + CountQty;
                                }
                                //if (Quantity_out > Availnventory)
                                //{
                                //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgQty", "alert('Entered Quantity is Grater than Available Quantity " + Availnventory + ".');", true);
                                //    return;
                                //}
                                Int32 Chkduplicate = Convert.ToInt32(CommonComponent.GetScalarCommonData("SElect Count(*) from tb_WareHouseProduct where ProductID=" + ProductId + " and StoreID=" + AppLogic.AppConfigs("StoreID") + ""));
                                if (Chkduplicate > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_WareHouseProduct set Inventory=" + Quantity_out + " where ProductID=" + ProductId + " and StoreID=" + AppLogic.AppConfigs("StoreID") + "");
                                    res = true;
                                }
                                else
                                {
                                    CommonComponent.ExecuteCommonData("Insert into tb_WareHouseproduct(Productid,Inventory,Price,SKU,Name,CreatedOn,StoreId)values(" + ProductId + "," + Quantity_out + "," + Price + ",'" + Sku.ToString() + "','" + Name.ToString().Replace("'", "''") + "',getdate()," + Request.QueryString["StoreId"].ToString() + ")");
                                    res = true;
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgQty", "alert('Please Enter Valid Quantity');", true);
                                return;
                            }
                        }
                    }
                }
                if (res)
                {
                    Page.ClientScript.RegisterClientScriptBlock(btnAddSelectedItems.GetType(), "@closemsg", "window.opener.location.href = window.opener.location.href;alert('Product(s) added Successfully');window.close();", true);
                }
            }
        }

        /// <summary>
        /// Add to the list.
        /// </summary>
        /// <param name="lblSKU">string lblSKU</param>
        private void AddtoList(string lblSKU)
        {
            try
            {
                string list = "";
                if (ViewState["SelectedSKUs"] != null)
                {
                    list = ViewState["SelectedSKUs"].ToString();
                    if (!string.IsNullOrEmpty(lblSKU) && !list.Contains(lblSKU + ","))
                    {
                        list += lblSKU + ",";
                    }
                }
                else ViewState["SelectedSKUs"] = "";

                ViewState["SelectedSKUs"] = list;
            }
            catch { }
        }

        /// <summary>
        /// Binds the selected data from Selected SKUs.
        /// </summary>
        private void BindSelectedData()
        {
            string SKUs = ViewState["SelectedSKUs"].ToString();
            if (!string.IsNullOrEmpty(SKUs))
            {
                SKUs = SKUs.Replace("'", "''").TrimEnd(",".ToCharArray()).TrimStart(",".ToCharArray());
                DataSet ds = new DataSet();
                string sql = "exec [usp_GetProductsUsingSkus] " + AppLogic.AppConfigs("StoreId") + ", '" + SKUs + "'";
                ds = CommonComponent.GetCommonDataSet(sql.ToString());
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    grdSelected.DataSource = ds.Tables[0];
                    grdSelected.DataBind();
                    trSelectedData.Visible = true;
                    trAddSeletedItems.Visible = true;
                }
                else
                {
                    grdSelected.DataSource = null;
                    grdSelected.DataBind();
                    trSelectedData.Visible = false;
                    trAddSeletedItems.Visible = false;
                }
            }
            else
            {
                grdSelected.DataSource = null;
                grdSelected.DataBind();
                trSelectedData.Visible = false;
                trAddSeletedItems.Visible = false;
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowAll_Click(object sender, ImageClickEventArgs e)
        {
            CurrentPage = 0;
            BindSearchData();
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
                Label lblCat = (Label)e.Row.FindControl("lblCat");
                ImageButton btndel = (ImageButton)e.Row.FindControl("btndel");
                btndel.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/cancel-icon.png";
                if (lblCat != null && lblCat != null && !string.IsNullOrEmpty(lblCat.Text))
                {
                    btndel.Visible = false;
                }
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
                Removedata(e.CommandArgument.ToString());
                BindSelectedData();
            }
        }

        /// <summary>
        /// Remove data from the SKUs Viw State.
        /// </summary>
        /// <param name="lblSKU">String lblSKU</param>
        private void Removedata(string lblSKU)
        {
            try
            {
                string list = ViewState["SelectedSKUs"].ToString();
                if (!string.IsNullOrEmpty(list) && list.Contains(lblSKU + ","))
                {
                    list = list.Replace(lblSKU + ",", "");
                }
                ViewState["SelectedSKUs"] = list;
            }
            catch { }
        }

        /// <summary>
        ///  Add Selected Items Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddSelectedItems_Click(object sender, ImageClickEventArgs e)
        {
            if (grdSelected.Rows.Count > 0)
            {
                try
                {
                    if (!string.IsNullOrEmpty(Request.QueryString["clientid"].ToString()))
                    {
                        string skus = ViewState["SelectedSKUs"].ToString();
                        if (skus.Length > 1)
                            skus = skus.TrimEnd(",".ToCharArray());
                        Page.ClientScript.RegisterClientScriptBlock(btnAddSelectedItems.GetType(), "@closemsg", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').value = '" + skus + "';alert('Product(s) added Successfully');", true);
                    }
                }
                catch { }
            }
        }

    }
}