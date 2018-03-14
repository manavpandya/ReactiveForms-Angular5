﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class CustomerProfitableReport : Solution.UI.Web.BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindStore();
                FillProfitableCustomer();
            }
        }

        /// <summary>
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
        }

        /// <summary>
        /// Get the Profitable Customers
        /// </summary>
        private void FillProfitableCustomer()
        {
            DataSet dsCustomer = new DataSet();
            dsCustomer = DashboardComponent.GetProfitableCustomer(Convert.ToInt32(ddlStore.SelectedValue));
            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                grdCustomerProfitableReport.DataSource = dsCustomer;
                grdCustomerProfitableReport.DataBind();
            }
            else
            {
                grdCustomerProfitableReport.DataSource = null;
                grdCustomerProfitableReport.DataBind();
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillProfitableCustomer();
        }

        /// <summary>
        ///  Customer Profitable Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdCustomerProfitableReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCustomerProfitableReport.PageIndex = e.NewPageIndex;
            FillProfitableCustomer();
        }

    }
}