using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Web.UI.HtmlControls;
using Solution.Bussines.Components.AdminCommon;
using System.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class CustomerPhoneOrder : BasePage
    {
        #region Declaration

        CustomerComponent objCustomer = null;
        tb_Customer tb_Customer = null;
        public static bool isDescendEmail = false;
        public static bool isDescendCustomerName = false;
        public static bool isDescendStoreName = false;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["storeId"] != null)
            {
                if (Session["AdminID"] == null || Session["AdminID"].ToString() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter4", "window.parent.location.href='/Admin/Login.aspx';", true);
                    return;
                }
                else
                {
                    String strScript = @"$(function () {
                                    $('#header').css('display', 'none');
                                    $('#footer').css('display', 'none');
                                    $('.content-row1').css('display', 'none');
                                    $('body').css('background', 'none');
                                    });";

                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "HideHeaderFooter", strScript.Trim(), true);
                }
            }
            if (!IsPostBack)
            {

                Session["PhoneCustomers"] = null;
                isDescendEmail = false;
                isDescendCustomerName = false;
                isDescendStoreName = false;
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                bindstore();
                bindCustomer("");
                string strScript = "if(window.parent.document.getElementById('ContentPlaceHolder1_TxtEmail') != null && window.parent.document.getElementById('ContentPlaceHolder1_TxtEmail').value != ''){document.getElementById('ContentPlaceHolder1_txtSearch').value=window.parent.document.getElementById('ContentPlaceHolder1_TxtEmail').value;document.getElementById('ContentPlaceHolder1_btnSearch').click();}";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "SeachEmail", strScript.Trim(), true);
               
            }
        }

        /// <summary>
        /// Binds the customers for Phone order.
        /// </summary>
        /// <param name="SearchVal">string SearchVal</param>
        private void bindCustomer(String SearchVal)
        {
            Int32 StoreId = 0;
            DataSet dsCustomer = new DataSet();
            CustomerComponent objcust = new CustomerComponent();
            if (ddlStore.SelectedIndex == 0)
                StoreId = Convert.ToInt32(0);
            else StoreId = Convert.ToInt32(ddlStore.SelectedValue.ToString());

            DataView dv = new DataView();
            if (Session["PhoneCustomers"] != null)
            {
                dv = ((DataTable)Session["PhoneCustomers"]).DefaultView;
            }
            else
            {
                dsCustomer = objcust.GetCustomerforPhoneOrder(StoreId, SearchVal.ToString());
                dv = new DataView(dsCustomer.Tables[0]);
            }

            if (dv != null)
            {
                if (ViewState["SortFieldName"] != null && ViewState["SortFieldValue"] != null)
                {
                    dv.Sort = ViewState["SortFieldName"].ToString() + " " + ViewState["SortFieldValue"].ToString();
                }
                if (dv != null && dv.Table != null && dv.ToTable().Rows.Count > 0)
                {
                    grdCustomer.DataSource = dv;
                    grdCustomer.DataBind();
                    lblTotcount.Text = "Total Record : " + dv.ToTable().Rows.Count.ToString();
                    Session["PhoneCustomers"] = dv.ToTable();
                }
                else
                {
                    grdCustomer.DataSource = null;
                    grdCustomer.DataBind();
                    lblTotcount.Text = "";
                    Session["PhoneCustomers"] = null;
                }
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            //var storeDetail = objStorecomponent.GetStore();
            DataSet dsStore = new DataSet();

            //Only Selected Stores will Display not all ----------------------

            dsStore = CommonComponent.GetCommonDataSet("Select * from tb_store where ISNULL(deleted,0)=0");
            if (dsStore != null && dsStore.Tables.Count > 0 && dsStore.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStore.Tables[0];
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            try
            {
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                {
                    if (Request.QueryString["storeId"] != null)
                    {
                        ddlStore.SelectedValue = Convert.ToString(Request.QueryString["storeId"].ToString());
                    }
                    else
                    {
                        ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                    }
                }
                else
                {
                    if (Request.QueryString["storeId"] != null)
                    {
                        ddlStore.SelectedValue = Convert.ToString(Request.QueryString["storeId"].ToString());
                    }
                    else
                    {
                        ddlStore.SelectedIndex = 0;
                    }
                }
            }
            catch
            {
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Filter record based on selected field and search value
            if (!string.IsNullOrEmpty(txtSearch.Text.ToString()))
            {
                Session["PhoneCustomers"] = null;
                ViewState["SortFieldName"] = null;
                ViewState["SortFieldValue"] = null;
                string SearchValue = txtSearch.Text.ToString().Trim().Replace("'", "''");
                bindCustomer(SearchValue.ToString());
            }
            grdCustomer.PageIndex = 0;
            grdCustomer.DataBind();
        }

        /// <summary>
        /// Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            Session["PhoneCustomers"] = null;
            ViewState["SortFieldName"] = null;
            ViewState["SortFieldValue"] = null;
            txtSearch.Text = "";
            ddlStore.SelectedIndex = 0;
            bindCustomer("");
        }

        /// <summary>
        /// Store Selected index change event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["PhoneCustomers"] = null;
            ViewState["SortFieldName"] = null;
            ViewState["SortFieldValue"] = null;
            if (!string.IsNullOrEmpty(txtSearch.Text.ToString()))
            {
                string SearchValue = txtSearch.Text.ToString().Trim().Replace("'", "''");
                bindCustomer(SearchValue.ToString());
            }
            else
            {
                bindCustomer("");
            }
        }

        /// <summary>
        /// Customer Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdCustomer_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        /// <summary>
        /// Customer Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdCustomer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblCustID = (Label)e.Row.FindControl("lblCustID");
                HtmlAnchor tagCustomer = (HtmlAnchor)e.Row.FindControl("tagCustomer");
                if (ddlStore.SelectedIndex > 0)
                {
                    if (Request.QueryString["Quote"] != null)
                    {
                        tagCustomer.HRef = "/Admin/Orders/CustomerQuote.aspx?CustId=" + lblCustID.Text.ToString() + "&StoreId=" + ddlStore.SelectedValue.ToString();
                    }
                    else
                    {
                        tagCustomer.HRef = "PhoneOrder.aspx?CustId=" + lblCustID.Text.ToString() + "&StoreId=" + ddlStore.SelectedValue.ToString();
                    }
                }
                else
                {
                    if (Request.QueryString["Quote"] != null)
                    {
                        tagCustomer.HRef = "/Admin/Orders/CustomerQuote.aspx?CustId=" + lblCustID.Text.ToString();
                    }
                    else
                    {
                        tagCustomer.HRef = "PhoneOrder.aspx?CustId=" + lblCustID.Text.ToString();
                    }
                }
            }

            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendEmail == false)
                {
                    ImageButton imgSortEmail = (ImageButton)e.Row.FindControl("imgSortEmail");
                    imgSortEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    imgSortEmail.AlternateText = "Ascending Order";
                    imgSortEmail.ToolTip = "Ascending Order";
                    imgSortEmail.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton imgSortEmail = (ImageButton)e.Row.FindControl("imgSortEmail");
                    imgSortEmail.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    imgSortEmail.AlternateText = "Descending Order";
                    imgSortEmail.ToolTip = "Descending Order";
                    imgSortEmail.CommandArgument = "ASC";
                }
                if (isDescendCustomerName == false)
                {
                    ImageButton imgSortCustomerName = (ImageButton)e.Row.FindControl("imgSortCustomerName");
                    imgSortCustomerName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    imgSortCustomerName.AlternateText = "Ascending Order";
                    imgSortCustomerName.ToolTip = "Ascending Order";
                    imgSortCustomerName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton imgSortCustomerName = (ImageButton)e.Row.FindControl("imgSortCustomerName");
                    imgSortCustomerName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    imgSortCustomerName.AlternateText = "Descending Order";
                    imgSortCustomerName.ToolTip = "Descending Order";
                    imgSortCustomerName.CommandArgument = "ASC";
                }
                if (isDescendStoreName == false)
                {
                    ImageButton imgSortStoreName = (ImageButton)e.Row.FindControl("imgSortStoreName");
                    imgSortStoreName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    imgSortStoreName.AlternateText = "Ascending Order";
                    imgSortStoreName.ToolTip = "Ascending Order";
                    imgSortStoreName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton imgSortStoreName = (ImageButton)e.Row.FindControl("imgSortStoreName");
                    imgSortStoreName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    imgSortStoreName.AlternateText = "Descending Order";
                    imgSortStoreName.ToolTip = "Descending Order";
                    imgSortStoreName.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Sorting Grid View Ascending or Descending
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            DataView dv = new DataView();
            if (Session["PhoneCustomers"] != null)
            {
                dv = ((DataTable)Session["PhoneCustomers"]).DefaultView;
            }
            ViewState["SortFieldName"] = null;
            ViewState["SortFieldValue"] = null;
            if (lb != null && dv.Count > 0)
            {
                if (lb.CommandArgument == "ASC")
                {
                    dv.Sort = lb.CommandName + " " + lb.CommandArgument;
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "imgSortEmail")
                    {
                        isDescendEmail = false;
                    }
                    else if (lb.ID == "imgSortCustomerName")
                    {
                        isDescendCustomerName = false;
                    }
                    else
                    {
                        isDescendStoreName = false;
                    }
                    ViewState["SortFieldValue"] = lb.CommandArgument.ToString();
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    dv.Sort = lb.CommandName + " " + lb.CommandArgument;
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "imgSortEmail")
                    {
                        isDescendEmail = true;
                    }
                    else if (lb.ID == "imgSortCustomerName")
                    {
                        isDescendCustomerName = true;
                    }
                    else
                    {
                        isDescendStoreName = true;
                    }
                    ViewState["SortFieldValue"] = lb.CommandArgument.ToString();
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
                ViewState["SortFieldName"] = lb.CommandName.ToString();

                grdCustomer.DataSource = dv;
                grdCustomer.DataBind();
            }
        }

        /// <summary>
        ///  Hidden Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnhdnDelete_Click(object sender, EventArgs e)
        {
            int gCustomerID = Convert.ToInt32(hdnCustDelete.Value);
            objCustomer = new CustomerComponent();
            tb_Customer = new tb_Customer();
            int IsDeleted = 0;
            tb_Customer = objCustomer.GetCustomerDataByID(gCustomerID);
            tb_Customer.Deleted = true;
            IsDeleted = objCustomer.UpdateCustomerList(tb_Customer);
            if (IsDeleted > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record Deleted Successfully.', 'Message');});", true);
                grdCustomer.DataBind();
                hdnCustDelete.Value = "0";
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while deleting record.', 'Message');});", true);
                return;
            }
        }

        /// <summary>
        ///  Customer Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdCustomer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomer.PageIndex = e.NewPageIndex;
            bindCustomer(txtSearch.Text.ToString().Trim().Replace("'", "''"));
        }

        protected void grdCustomer_Sorting(object sender, GridViewSortEventArgs e)
        {

        }

        private void RemoveCart(Int32 CustId)
        {
            CommonComponent.ExecuteCommonData("DELETE FROM tb_ShoppingCartItems WHERE ShoppingCartID in (SELECT ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + CustId + ")");
        }
        protected void grdCustomer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Int32 Id = Convert.ToInt32(grdCustomer.DataKeys[e.NewEditIndex].Value.ToString());

            RemoveCart(Id);
            if (Request.QueryString["CustId"] != null && Request.QueryString["CustId"].ToString().Trim() != "" && Request.QueryString["CustId"].ToString() != "0")
            {
                if (Request.QueryString["Quote"] != null)
                {
                    if (Request.QueryString["CustId"].ToString() != Id.ToString())
                    {
                        CommonComponent.ExecuteCommonData("Delete from tb_CustomerQuoteItems Where CustomerId=" + Id.ToString() + " and CustomerQuoteID=0");
                        CommonComponent.ExecuteCommonData("Update tb_CustomerQuoteItems set CustomerId=" + Id.ToString() + " Where CustomerId=" + Request.QueryString["CustId"].ToString() + " and CustomerQuoteID=0");
                    }
                }
                else
                {
                    RegisterCart(Convert.ToInt32(Request.QueryString["CustId"].ToString()), Convert.ToInt32(Id));
                }
            }
            string strUrl = "";
            if (Request.QueryString["Storeid"] != null)
            {
                if (Request.QueryString["Quote"] != null)
                {
                    strUrl = "/Admin/Orders/CustomerQuote.aspx?CustId=" + Id.ToString() + "&StoreId=" + Request.QueryString["Storeid"].ToString() + "&saleorder=1";
                }
                else
                {
                    strUrl = "/Admin/orders/PhoneOrder.aspx?CustId=" + Id.ToString() + "&StoreId=" + Request.QueryString["Storeid"].ToString() + "&saleorder=1";
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "pageredirect", "javascript:window.parent.location.href='" + strUrl + "';", true);
            }
            else
            {
                if (Request.QueryString["Quote"] != null)
                {
                    strUrl = "/Admin/Orders/CustomerQuote.aspx?CustId=" + Id.ToString() + "&saleorder=1";
                }
                else { strUrl = "/Admin/orders/PhoneOrder.aspx?CustId=" + Id.ToString() + "&saleorder=1"; }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "pageredirect", "javascript:window.parent.location.href='" + strUrl + "';", true);
            }
        }
        public void RegisterCart(Int32 OldCustID, Int32 NewId)
        {

            CustomerComponent objCustomer = new CustomerComponent();
            ShoppingCartComponent objShoppingCart = new ShoppingCartComponent();

            if (OldCustID.ToString() != NewId.ToString())
            {
                objShoppingCart.DeleteCartItems(NewId);
                objShoppingCart.UpdateCustomerForCart(NewId, OldCustID);
            }

        }
    }
}