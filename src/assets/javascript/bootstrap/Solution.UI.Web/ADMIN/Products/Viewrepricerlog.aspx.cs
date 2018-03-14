﻿using Solution.Bussines.Components;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class Viewrepricerlog : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //  binddata();
            }

        }
        public void binddata()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("Exec usp_getrepricerlog '" + txtsearch.Text.ToString().Replace("'", "''") + "'");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                gvhemminglog.DataSource = ds;
                gvhemminglog.DataBind();
            }
            else
            {
                gvhemminglog.DataSource = null;
                gvhemminglog.DataBind();
            }
        }

        protected void gvhemminglog_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvhemminglog.PageIndex = e.NewPageIndex;
            binddata();
        }

        protected void btnsearch_Click(object sender, EventArgs e)
        {
            binddata();
        }

        protected void btnShowAll_Click(object sender, EventArgs e)
        {
            txtsearch.Text = "";
            binddata();
        }

        protected void gvhemminglog_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lbldes = (Label)e.Row.FindControl("lbldes");
                Label lbldes2 = (Label)e.Row.FindControl("lbldes2");

                if (!String.IsNullOrEmpty(lbldes2.Text.ToString()) && lbldes2.Text.ToString().ToLower().IndexOf("-formula") > -1)
                {
                    string Price = lbldes2.Text.ToString().Replace("-Formula", "").Replace("-formula", "");
                    lbldes.Text = "Manual Formula Price Update - " + Price;
                }
                else if (!String.IsNullOrEmpty(lbldes2.Text.ToString()) && lbldes2.Text.ToString().ToLower().IndexOf("-yourprice") > -1)
                {
                    string Price = lbldes2.Text.ToString().Replace("-YourPrice", "").Replace("-yourprice", "");
                    lbldes.Text = "Manual Price Update - " + Price;
                }
                else if (!String.IsNullOrEmpty(lbldes2.Text.ToString()) && lbldes2.Text.ToString().ToLower().IndexOf("-auto") > -1)
                {
                    string Price = lbldes2.Text.ToString().Replace("-Auto", "").Replace("-auto", "");
                    lbldes.Text = "Auto Price Update - " + Price;
                }

            }
        }

    }
}