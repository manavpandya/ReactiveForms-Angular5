using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ListBulkPrinting : BasePage
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
                txtFromDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtToDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                Bindstore();
                BindBlukPrint();
            }
            btnprintslip.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-packing-slip.gif";
            ImageButton1.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-packing-slip.gif";
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            grvBulkPrintingReport.PageIndex = 0;
            BindBlukPrint();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            grvBulkPrintingReport.PageIndex = 0;
            BindBlukPrint();
        }

        /// <summary>
        ///  Search All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            grvBulkPrintingReport.PageIndex = 0;
            BindBlukPrint();
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void Bindstore()
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
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            try
            {
                if (Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"]) > 0)
                {
                    ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"]);
                }
                else
                {
                    ddlStore.SelectedIndex = 0;
                }
            }
            catch
            {
                ddlStore.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Binds the bulk print.
        /// </summary>
        private void BindBlukPrint()
        {
            DataSet dsOrder = new DataSet();
            string strWhereClus = "";
            if (string.IsNullOrEmpty(txtFromDate.Text.ToString()))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter From Date.', 'Message');", true);
                return;
            }
            if (txtFromDate.Text.ToString() != "" && txtToDate.Text.ToString() != "")
            {
                if (Convert.ToDateTime(txtToDate.Text.ToString()) >= Convert.ToDateTime(txtFromDate.Text.ToString()))
                { }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtFromDate.Text.ToString() == "" && txtToDate.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtToDate.Text.ToString() == "" && txtFromDate.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            if (txtFromDate.Text.ToString() != "")
            {
                strWhereClus += " AND cast(OrderDate as date) >= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtFromDate.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
            }
            if (txtToDate.Text.ToString() != "")
            {
                strWhereClus += " AND cast(OrderDate as date) <= cast('" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtToDate.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "' as date)";
            }
            if (ddlIsPrinted.SelectedValue.ToLower().ToString() == "not printed")
            {
                strWhereClus += " AND ISNULL(IsPrinted,0)=0";
            }
            else if (ddlIsPrinted.SelectedValue.ToLower().ToString() == "printed")
            {
                strWhereClus += " AND ISNULL(IsPrinted,0)=1";
            }

            if (ddlStore.SelectedIndex != 0)
            {
                strWhereClus += " and tb_Order.StoreID=" + ddlStore.SelectedValue.ToString() + "";
            }

            if (!string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
            {
                strWhereClus += " AND tb_Order.OrderNumber =" + txtSearch.Text.ToString().Replace("'", "''") + "";
            }

            dsOrder = OrderComponent.GetorderListForMultiCapture(strWhereClus.ToString(), 2);
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                grvBulkPrintingReport.DataSource = dsOrder;
                grvBulkPrintingReport.DataBind();
                trBottom.Visible = true;
                trtop.Visible = true;
            }
            else
            {
                grvBulkPrintingReport.DataSource = null;
                grvBulkPrintingReport.DataBind();
                trBottom.Visible = false;
                trtop.Visible = false;
            }
        }

        /// <summary>
        /// Bulk Printing Report Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvBulkPrintingReport_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView dr = (DataRowView)e.Row.DataItem;
                int OrderNumber = Convert.ToInt32(dr["OrderNumber"].ToString());
                Literal ltrBulkOrderNnumber = (Literal)e.Row.FindControl("ltrBulkOrderNnumber");

                ltrBulkOrderNnumber.Text = "<a class=\"order-no\" onclick=\"chkHeight();\" href=\"/Admin/Orders/Orders.aspx?id=" + ltrBulkOrderNnumber.Text.ToString() + "\">" + ltrBulkOrderNnumber.Text.ToString() + "</a><br />";

                Label lbship = (Label)e.Row.FindControl("lblIsShipped");
                if (lbship.Text != "")
                {
                    if (Convert.ToDateTime(lbship.Text) == System.DateTime.MinValue)
                        lbship.Text = "No";
                }
                Label lbItems = (Label)e.Row.FindControl("lblItems");
                Label lbSID = (Label)e.Row.FindControl("lblShoppingCartID");

                string st = null;
                DataSet ds = new DataSet();
                string StrQuery = "SELECT tb_Product.Name ProductName, tb_OrderedShoppingCartItems.Quantity ProductQuantity " +
                                  " FROM tb_OrderedShoppingCartItems INNER JOIN tb_OrderedShoppingCart ON  " +
                                  " tb_OrderedShoppingCartItems.OrderedShoppingCartID = tb_OrderedShoppingCart.OrderedShoppingCartID INNER JOIN  " +
                                  " tb_Product ON tb_OrderedShoppingCartItems.RefProductID = tb_Product.ProductID INNER JOIN  " +
                                  " tb_Order ON tb_OrderedShoppingCart.OrderedShoppingCartID = tb_Order.ShoppingCardID  " +
                                  " WHERE tb_Order.OrderNumber = " + OrderNumber + " AND tb_Order.ShoppingCardID =" + Convert.ToInt32(lbSID.Text.ToString()) + "";
                ds = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                if (ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr1 in ds.Tables[0].Rows)
                    {
                        st = st + "(" + dr1["ProductQuantity"].ToString() + ")" + "    " + "<strong>" + dr1["ProductName"].ToString() + "</strong>";
                        st += "<BR>";
                    }
                    lbItems.Text = st.ToString();
                }
                else
                    lbItems.Text = "<strong>Order Catalog</strong>";

                //StringBuilder items = new StringBuilder();
                //bool first = true;
                //foreach (CartItem c in ObjOrder.CartItems)
                //{
                //    if (!first)
                //    {
                //        items.Append("<br/><br/>");
                //    }
                //    items.Append("(" + c.m_Quantity.ToString() + ") ");
                //    items.Append(ObjOrder.GetLineItemDescription(c));
                //    first = false;
                //}
            }
        }

        /// <summary>
        ///  Print Slip Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnprintslip_Click(object sender, ImageClickEventArgs e)
        {
            Session["PrintOrderId"] = "";
            int i = 0;
            foreach (GridViewRow r in grvBulkPrintingReport.Rows)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)r.FindControl("chkPrint");
                Label lb = (Label)r.FindControl("OrderId");
                if (chk.Checked)
                {
                    if (i == 0)
                        Session["PrintOrderId"] += lb.Text.ToString();
                    else
                        Session["PrintOrderId"] += "," + lb.Text.ToString();
                    i++;
                }
            }
            if (!string.IsNullOrEmpty(Session["PrintOrderId"].ToString()))
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "subscribescript", "window.open('PrintMultipleSlip.aspx', '','height=750,width=850,scrollbars=1');", true);
            }
            else
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select atleast one order to print.','Message');", true);
        }

        /// <summary>
        ///  Bulk Printing Report Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvBulkPrintingReport_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvBulkPrintingReport.PageIndex = e.NewPageIndex;
            BindBlukPrint();
        }

        /// <summary>
        ///  Image Multi Cap Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ImgMultiCap_Click(object sender, ImageClickEventArgs e)
        {
            Session["PrintOrderId"] = "";
            int i = 0;
            foreach (GridViewRow r in grvBulkPrintingReport.Rows)
            {
                HtmlInputCheckBox chk = (HtmlInputCheckBox)r.FindControl("chkPrint");
                Label lb = (Label)r.FindControl("OrderId");
                if (chk.Checked)
                {
                    if (i == 0)
                        Session["PrintOrderId"] += lb.Text.ToString();
                    else
                        Session["PrintOrderId"] += "," + lb.Text.ToString();
                    i++;
                }
            }
            if (!string.IsNullOrEmpty(Session["PrintOrderId"].ToString()))
            {
                Page.ClientScript.RegisterClientScriptBlock(Page.GetType(), "subscribescript", "window.open('PrintMultipleSlip.aspx', '','height=750,width=850,scrollbars=1');", true);
            }
            else
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select atleast one order to print.','Message');", true);
        }

        /// <summary>
        /// Printing Option Selection
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlIsPrinted_SelectedIndexChanged(object sender, EventArgs e)
        {
            grvBulkPrintingReport.PageIndex = 0;
            BindBlukPrint();
        }
    }
}