using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class SalesRepresentiveReport : BasePage
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
                imgSearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/search.gif";
                imgShowAll.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/showall.png";

                BindStore();
            }

        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            Getrecord(1);
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
            ddlStore.SelectedIndex = 0;
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param> 
        protected void imgShowAll_Click(object sender, ImageClickEventArgs e)
        {
            Getrecord(0);
        }

        /// <summary>
        /// Get Record specified by Flag
        /// </summary>
        /// <param name="Flag">int Flag</param>
        private void Getrecord(Int32 Flag)
        {
            DataSet dsRecord = new DataSet();

            if (Flag == 0)
            {
                if (ddlStore.SelectedIndex > 0)
                {
                    dsRecord = CommonComponent.GetCommonDataSet("select DISTINCT count(OrderNumber) over(partition by SalesRepName) as OrderNumber,SalesRepName,tb_Store.StoreName,ISNULL(sum(OrderTotal) over(partition by SalesRepName),0) as OrderTotal from tb_Order INNER JOIN tb_store on tb_store.Storeid=tb_Order.StoreId where ISNULL(SalesRepName,'') <> '' and tb_Order.StoreID=" + ddlStore.SelectedValue + "");
                }
                else
                {
                    dsRecord = CommonComponent.GetCommonDataSet("select DISTINCT count(OrderNumber) over(partition by SalesRepName) as OrderNumber,SalesRepName,tb_Store.StoreName,ISNULL(sum(OrderTotal) over(partition by SalesRepName),0) as OrderTotal from tb_Order INNER JOIN tb_store on tb_store.Storeid=tb_Order.StoreId where ISNULL(SalesRepName,'') <> ''");
                }
            }
            else
            {
                if (ddlStore.SelectedIndex > 0)
                {
                    dsRecord = CommonComponent.GetCommonDataSet("select DISTINCT count(OrderNumber) over(partition by SalesRepName) as OrderNumber,SalesRepName,tb_Store.StoreName,ISNULL(sum(OrderTotal) over(partition by SalesRepName),0) as OrderTotal from tb_Order INNER JOIN tb_store on tb_store.Storeid=tb_Order.StoreId where ISNULL(SalesRepName,'') like '%" + txtname.Text.ToString().Replace("'", "''") + "%' AND ISNULL(SalesRepName,'') <> '' and tb_Order.StoreID=" + ddlStore.SelectedValue + "");
                }
                else
                {
                    dsRecord = CommonComponent.GetCommonDataSet("select DISTINCT count(OrderNumber) over(partition by SalesRepName) as OrderNumber,SalesRepName,tb_Store.StoreName,ISNULL(sum(OrderTotal) over(partition by SalesRepName),0) as OrderTotal from tb_Order INNER JOIN tb_store on tb_store.Storeid=tb_Order.StoreId where ISNULL(SalesRepName,'') like '%" + txtname.Text.ToString().Replace("'", "''") + "%' AND ISNULL(SalesRepName,'') <> ''");
                }
            }

            if (dsRecord != null && dsRecord.Tables.Count > 0 && dsRecord.Tables[0].Rows.Count > 0)
            {
                grvSalesrepReport.DataSource = dsRecord;
                grvSalesrepReport.DataBind();
            }
            else
            {
                grvSalesrepReport.DataSource = null;
                grvSalesrepReport.DataBind();

            }
        }
    }
}