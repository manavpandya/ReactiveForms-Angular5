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
    public partial class SalesReport : BasePage
    {
        DataSet dsFooter = new DataSet();

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgSearch.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/searchgo.gif";
                BindStore();
                GetSalesReport();
            }
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSearch_Click(object sender, ImageClickEventArgs e)
        {
            GetSalesReport();
        }

        /// <summary>
        /// Get Sales Report Data
        /// </summary>
        private void GetSalesReport()
        {
            OrderComponent objReport = new OrderComponent();
            DataSet dsReport = new DataSet();

            DateTime dtStart = DateTime.Now;
            DateTime dtEnd = DateTime.Now;
            if (RadOrderByDays.SelectedValue.ToLower() == "customize")
            {
                if (txtOrderFrom.Text.ToString() != "" && txtOrderTo.Text.ToString() != "")
                {
                    if (Convert.ToDateTime(txtOrderTo.Text.ToString()) >= Convert.ToDateTime(txtOrderFrom.Text.ToString()))
                    {

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select valid date.');Datevisible();", true);
                        return;
                    }
                }
                else
                {
                    if (txtOrderFrom.Text.ToString() == "" && txtOrderTo.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select valid date.');Datevisible();", true);
                        return;
                    }
                    else if (txtOrderTo.Text.ToString() == "" && txtOrderFrom.Text.ToString() != "")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select valid date.');Datevisible();", true);
                        return;
                    }

                }
                dtStart = Convert.ToDateTime(txtOrderFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""));
                dtEnd = Convert.ToDateTime(txtOrderTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""));
            }
            dsReport = objReport.GetSaleReportRecord(RadOrderByDays.SelectedValue.ToString(), dtStart, dtEnd, Convert.ToInt32(ddlStore.SelectedValue.ToString()));
            if (dsReport != null && dsReport.Tables.Count > 0 && dsReport.Tables[0].Rows.Count > 1)
            {
                dsFooter = new DataSet();
                dsFooter = dsReport.Clone();

                DataRow dr = dsReport.Tables[0].Rows[dsReport.Tables[0].Rows.Count - 1];
                object[] dr1 = dr.ItemArray;
                dsFooter.Tables[0].Rows.Add(dr1);
                dsReport.Tables[0].Rows.RemoveAt(dsReport.Tables[0].Rows.Count - 1);
                dsReport.Tables[0].AcceptChanges();
                ViewState["TotalCount"] = dsReport.Tables[0].Rows.Count;
                ViewState["Count"] = "0";
                grvSalesReport.DataSource = dsReport;
                grvSalesReport.DataBind();

            }
            else
            {
                grvSalesReport.DataSource = null;
                grvSalesReport.DataBind();
            }
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "Datevisible();", true);

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
        /// Sale Report Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvSalesReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblpanddin = (Label)e.Row.FindControl("lblSubTotal");
                Label lblpanddinTot = (Label)e.Row.FindControl("lblSubTotalTot");
                Label lblOrderDate = (Label)e.Row.FindControl("lblOrderDate");
                LinkButton lnOrderSlip = (LinkButton)e.Row.FindControl("lnOrderSlip");
                if (lblpanddin != null && lblOrderDate != null)
                {
                    if (ViewState["Count"] == null)
                        ViewState["Count"] = "1";
                    else
                        ViewState["Count"] = Convert.ToInt16(ViewState["Count"]) + 1;

                    if (ViewState["TotalCount"] != null && Convert.ToInt16(ViewState["Count"]) == Convert.ToInt16(ViewState["TotalCount"]))
                    {
                        lblpanddin.ForeColor = System.Drawing.Color.Maroon;
                        lblpanddinTot.ForeColor = System.Drawing.Color.Maroon;
                        lnOrderSlip.Visible = false;
                        lblOrderDate.Visible = false;
                    }
                }

            }
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                Label lblpanddingOrderTotf = (Label)e.Row.FindControl("lblpanddingOrderTotf");
                Label lblCompleteOrderTotf = (Label)e.Row.FindControl("lblCompleteOrderTotf");
                Label lblCompleteOrderTotship = (Label)e.Row.FindControl("lblCompleteOrderTotship");
                Label lblDeclinedOrdersTotf = (Label)e.Row.FindControl("lblDeclinedOrdersTotf");
                Label lblCancelledOrdersTotf = (Label)e.Row.FindControl("lblCancelledOrdersTotf");
                Label lblAllDetailsTotf = (Label)e.Row.FindControl("lblAllDetailsTotf");

                e.Row.Cells[0].Attributes.Add("Style", "background-color:#707070");
                e.Row.Cells[1].Attributes.Add("Style", "background-color:#707070");
                e.Row.Cells[2].Attributes.Add("Style", "background-color:#707070");
                e.Row.Cells[3].Attributes.Add("Style", "background-color:#707070");
                e.Row.Cells[4].Attributes.Add("Style", "background-color:#707070");
                e.Row.Cells[5].Attributes.Add("Style", "background-color:#707070");
                e.Row.Cells[6].Attributes.Add("Style", "background-color:#707070");
                e.Row.Cells[7].Attributes.Add("Style", "background-color:#707070");
                e.Row.Cells[8].Attributes.Add("Style", "background-color:#707070");
                if (dsFooter != null && dsFooter.Tables.Count > 0 && dsFooter.Tables[0].Rows.Count > 0)
                {
                    lblpanddingOrderTotf.Text = dsFooter.Tables[0].Rows[0]["panddingOrder"].ToString() + "~" + Convert.ToDecimal(dsFooter.Tables[0].Rows[0]["panddingOrderTot"].ToString()).ToString("C");
                    lblCompleteOrderTotf.Text = dsFooter.Tables[0].Rows[0]["CompleteOrder"].ToString() + "~" + Convert.ToDecimal(dsFooter.Tables[0].Rows[0]["CompleteOrderTot"].ToString()).ToString("C");
                    lblCompleteOrderTotship.Text = dsFooter.Tables[0].Rows[0]["CompleteOrder"].ToString() + "~" + Convert.ToDecimal(dsFooter.Tables[0].Rows[0]["CompleteOrderTot"].ToString()).ToString("C");
                    lblDeclinedOrdersTotf.Text = dsFooter.Tables[0].Rows[0]["DeclinedOrders"].ToString() + "~" + Convert.ToDecimal(dsFooter.Tables[0].Rows[0]["DeclinedOrdersTot"].ToString()).ToString("C");
                    lblCancelledOrdersTotf.Text = dsFooter.Tables[0].Rows[0]["CancelledOrders"].ToString() + "~" + Convert.ToDecimal(dsFooter.Tables[0].Rows[0]["CancelledOrdersTot"].ToString()).ToString("C");
                    lblAllDetailsTotf.Text = dsFooter.Tables[0].Rows[0]["AllDetails"].ToString() + "~" + Convert.ToDecimal(dsFooter.Tables[0].Rows[0]["AllDetailsTot"].ToString()).ToString("C");
                }
            }


        }

        /// <summary>
        /// Sale Report Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grvSalesReport_RowCommand(object sender, GridViewCommandEventArgs e)
        {


        }

        /// <summary>
        /// Rad Order By Days Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void RadOrderByDays_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RadOrderByDays.SelectedValue.ToLower() == "customize")
            {
                datetd.Visible = true;
            }
            else
            {
                datetd.Visible = false;
            }
            if (RadOrderByDays.SelectedIndex == 3)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "if (document.getElementById('ContentPlaceHolder1_datetd') != null) {document.getElementById('ContentPlaceHolder1_datetd').style.display = '';}", true);
            }
        }

        /// <summary>
        ///  Sale Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvSalesReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvSalesReport.PageIndex = e.NewPageIndex;
            GetSalesReport();
        }
    }
}