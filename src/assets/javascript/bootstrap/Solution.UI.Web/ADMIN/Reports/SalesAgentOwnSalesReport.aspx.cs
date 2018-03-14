using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class SalesAgentOwnSalesReport :BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStore();
                FillSaleAgentOrdersSale();
            }
        }

        /// Bind All Stores in Drop down
        /// </summary>
        private void BindStore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, new ListItem("All Stores", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
            }
            if (!String.IsNullOrEmpty(Request.QueryString["storeid"]))
            {
                ddlStore.SelectedValue = Convert.ToString(Request.QueryString["storeid"]);

            }
            if(!String.IsNullOrEmpty(Request.QueryString["storeid"]))
            {
                 ddlStore.SelectedValue=Convert.ToString(Request.QueryString["storeid"]);
            }
        }

        
        private void FillSaleAgentOrdersSale()
        {
            DataSet dsCustomer = new DataSet();
            int loginid = Convert.ToInt32(Session["AdminID"]);
            dsCustomer = DashboardComponent.GetSalesAgentOwnSalesReport(Convert.ToInt32(ddlStore.SelectedValue), loginid);
            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                grdSalesAgentOrders.DataSource = dsCustomer;
                grdSalesAgentOrders.DataBind();
            }
            else
            {
                grdSalesAgentOrders.DataSource = null;
                grdSalesAgentOrders.DataBind();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillSaleAgentOrdersSale();
        }

        /// <summary>
        ///  Product Sale Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdSalesAgentOrders_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdSalesAgentOrders.PageIndex = e.NewPageIndex;
            FillSaleAgentOrdersSale();
        }

        protected void grdSalesAgentOrders_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblOrderStatus = (Label)e.Row.FindControl("lblOrderStatus");

                if (lblOrderStatus != null)
                {
                    if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("pending") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#D3321C;font-size:11px;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("authorized") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#FF7F00;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("captured") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#348934;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("void") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#1A1AC4;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("refunded") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#00AAFF;");
                    }
                    else if (lblOrderStatus.Text.ToString().Trim().ToLower().IndexOf("canceled") > -1)
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#FF0000;");
                    }
                    else
                    {
                        lblOrderStatus.Attributes.Add("style", "text-decoration:none;color:#000;");
                    }
                }
            }
        }
    }
}