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
    public partial class CustomerQuoteProducts : BasePage
    {
        DataSet dsProduct = new DataSet();
        public static bool isDescendProductName = false;
        public static bool isDescendSKU = false;
        CustomerQuoteComponent objCustomerQuote = new CustomerQuoteComponent();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnAddProducts.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/add-products.png) no-repeat transparent; width: 90px; height: 23px; border:none;cursor:pointer;");
                btnAddProducts2.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/add-products.png) no-repeat transparent; width: 90px; height: 23px; border:none;cursor:pointer;");
                btnBack.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/back.png) no-repeat transparent;width: 50px; height: 23px; border:none;cursor:pointer;");
                gvProducts.PageSize = 100;
                BindData();              
            }
        }


        /// <summary>
        /// Bind the Data into Gridview
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="SearchField">string SearchField</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="OrderField">string OrderField</param>
        /// <param name="Order">string Order</param>
        public void BindData(int Mode = 1, string SearchField = null, string SearchValue = null, string OrderField = null, string Order = "ASC")
        {
            if (Request.QueryString["StoreID"] != null)
            {
                dsProduct = objCustomerQuote.GetCustomerQuoteProducts(Convert.ToInt32(Request.QueryString["StoreID"]), Mode, SearchField, SearchValue, OrderField, Order);
                if (dsProduct != null && dsProduct.Tables.Count > 0 && dsProduct.Tables[0].Rows.Count > 0)
                {
                    gvProducts.DataSource = dsProduct.Tables[0];
                    gvProducts.DataBind();
                    trAddProducts.Visible = true;
                    trAddProducts1.Visible = true;
                }
                else
                {
                    gvProducts.DataSource = null;
                    gvProducts.DataBind();
                    trAddProducts.Visible = false;
                    trAddProducts1.Visible = false;

                    //lblMsg.Text = "No Record(s) Found.";
                }
            }
        }

        /// <summary>
        ///  Add Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddProductsTemp_Click(object sender, EventArgs e)
        {
            string PIDs = "";
            foreach (GridViewRow r in gvProducts.Rows)
            {
                CheckBox chk = (CheckBox)r.FindControl("chkSelect");
                Label lb = (Label)r.FindControl("lblProductID");
                int ID = Convert.ToInt32(lb.Text.ToString());
                if (chk.Checked)
                {
                    PIDs += ID + ",";
                }
            }
            if (!String.IsNullOrEmpty(PIDs))
            {
                Session["QuotedPids"] = PIDs;
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "revise")
                {
                    if (Request.QueryString["Quoted"] != null)
                        Response.Redirect("CustomerQuote.aspx?ID=" + Request.QueryString["Quoted"] + "&Mode=revise");
                    else
                        Response.Redirect("CustomerQuote.aspx?Mode=revise");
                }
                else
                {
                    if (Request.QueryString["Quoted"] != null)
                        Response.Redirect("CustomerQuote.aspx?ID=" + Request.QueryString["Quoted"]);
                    else
                        Response.Redirect("CustomerQuote.aspx");
                }
            }
            else
            {
                lblMsg.Text = "Please select Products";
            }
        }

        /// <summary>
        /// Products Card Gridview Page Index Changes Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void gvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvProducts.PageIndex = e.NewPageIndex;
            if (txtSearch.Text.Trim() != "")
            {
                BindData(2, ddlSearch.SelectedValue.ToString(), txtSearch.Text.Trim());
            }
            else
            {
                BindData();
            }
        }


        /// <summary>
        /// Products Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (txtSearch.Text.Trim() != "")
            {
                BindData(2, ddlSearch.SelectedValue.ToString(), txtSearch.Text.Trim(), e.CommandName.ToString(), e.CommandArgument.ToString());
            }
            else
            {
                BindData(1, null, null, e.CommandName.ToString(), e.CommandArgument.ToString());
            }
        }

        /// <summary>
        /// Products Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendProductName == false)
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnProductName");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnProductName.AlternateText = "Ascending Order";
                    btnProductName.ToolTip = "Ascending Order";
                    btnProductName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnProductName = (ImageButton)e.Row.FindControl("btnProductName");
                    btnProductName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnProductName.AlternateText = "Descending Order";
                    btnProductName.ToolTip = "Descending Order";
                    btnProductName.CommandArgument = "ASC";
                }
                if (isDescendSKU == false)
                {
                    ImageButton btnSKU = (ImageButton)e.Row.FindControl("btnSKU");
                    btnSKU.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnSKU.AlternateText = "Ascending Order";
                    btnSKU.ToolTip = "Ascending Order";
                    btnSKU.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnSKU = (ImageButton)e.Row.FindControl("btnSKU");
                    btnSKU.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnSKU.AlternateText = "Descending Order";
                    btnSKU.ToolTip = "Descending Order";
                    btnSKU.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSearch.Text.Trim()))
            {
                BindData(2, ddlSearch.SelectedValue.ToString(), txtSearch.Text.Trim());
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            isDescendProductName = false;
            isDescendSKU = false;
            BindData();
        }

        /// <summary>
        /// Sorting Grid View Ascending or Descending
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnProductName")
                    {
                        isDescendProductName = false;
                    }
                    else
                    {
                        isDescendSKU = false;
                    }
                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnProductName")
                    {
                        isDescendProductName = true;
                    }
                    else
                    {
                        isDescendSKU = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        /// Back to Customer Quote Page
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "revise")
            {
                if (Request.QueryString["Quoted"] != null)
                    Response.Redirect("CustomerQuote.aspx?ID=" + Request.QueryString["Quoted"] + "&Mode=revise");
                else
                    Response.Redirect("CustomerQuote.aspx?Mode=revise");
            }
            else
            {
                if (Request.QueryString["Quoted"] != null)
                    Response.Redirect("CustomerQuote.aspx?ID=" + Request.QueryString["Quoted"]);
                else
                    Response.Redirect("CustomerQuote.aspx");
            }
        }
    }
}