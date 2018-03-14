using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Collections;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class OldPurchaseOrder : BasePage
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
                btnSubmit.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/search.gif";
                btnShowAll.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/showall.png";
                if (Request.QueryString["ONo"] != null)
                {
                    lblOrderNumber.Text = "Order Number:" + Request.QueryString["ONo"].ToString();
                    GeneratePO.HRef = "PurchaseOrder.aspx?Ono=" + Request.QueryString["ONo"];
                    GeneratePO.Visible = true;
                    btnClose.Visible = false;
                    datetr.Visible = false;
                    //BindCart(Convert.ToInt32(Request.QueryString["ONo"])); // Pending it update code when its needed from Sulu [Eol Parts] admin Panel....By Pallavi 31 Oct 2012
                }
                else if (Request.QueryString["VendorID"] != "")
                {
                    GeneratePO.Visible = false;
                    btnClose.Visible = true;
                    datetr.Visible = true;
                    BindVendorCart(Request.QueryString["VendorID"].ToString().Trim(), 1);
                }
            }
        }


        /// <summary>
        /// Binds the vendor cart.
        /// </summary>
        /// <param name="VendorID">int VendorID</param>
        /// <param name="type">int type</param>
        private void BindVendorCart(string VendorID, int type)
        {
            DataSet DsOldPOrder = new DataSet();
            string WheClus = "";
            if (type == 0) // Date Searching
            {
                WheClus = " and po.podate between '" + txtFromDate.Text.ToString() + "' and '" + txtToDate.Text.ToString() + "' ";
            }

            DsOldPOrder = CommonComponent.GetCommonDataSet("select convert(varchar(max),round(isnull(po.Adjustments,0),2)) as Adjustments,convert(varchar(max),round(isnull(po.Tax,0),2)) as Tax, " +
                                          " convert(varchar(max),round(isnull(po.Shipping,0),2)) as Shipping,po.ordernumber, " +
                                          " convert(varchar(max),round(isnull(po.additionalcost,0),2)) as additionalcost, " +
                                          " convert(varchar(max),round(isnull(po.poamount,0),2)) as poamount,po.PONumber,po.VendorID,po.PODate,po.OrderNumber,v.Name   " +
                                          " from tb_PurchaseOrder po INNER JOIN tb_Order o ON po.OrderNumber = o.OrderNumber  " +
                                          " INNER JOIN tb_Vendor v ON po.VendorID = v.VendorID  where po.VendorID = " + VendorID + "" + WheClus + "order by po.ordernumber desc,po.ponumber desc");

            if (DsOldPOrder != null && DsOldPOrder.Tables.Count > 0 && DsOldPOrder.Tables[0].Rows.Count > 0)
            {
                ltDetails.Text = "";
                ltDetails.Text += "<br/>";
                ltDetails.Text += " <b>Purchase Order for : " + DsOldPOrder.Tables[0].Rows[0]["Name"].ToString() + "</b> ";
                gvOldVendorPO.DataSource = DsOldPOrder;
                gvOldVendorPO.DataBind();
                gvOldVendorPO.Visible = true;
                trTotal.Visible = true;
                printPOid.Visible = true;
                //gvOldPOrder.Visible = false;

                Decimal TotalPOAmount = 0;
                Decimal TotalClearPOAmount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select isnull(sum(isnull(paidamount,0)),0) from tb_vendorpayment where vendorid=" + Convert.ToInt32(VendorID) + ""));
                Decimal TotalUnClearPOAmount = 0;
                foreach (DataRow dr in DsOldPOrder.Tables[0].Rows)
                {
                    TotalPOAmount += Convert.ToDecimal(dr["POAmount"].ToString());
                }
                TotalUnClearPOAmount = TotalPOAmount - TotalClearPOAmount;
                ltClearPOAmt.Text = "<b>Clear Amount </b>: $" + TotalClearPOAmount.ToString("f2");
                ltUnClearPOAmt.Text = "<b>UnClear Amount </b>: $" + TotalUnClearPOAmount.ToString("f2");
                ltTotalPOAmt.Text = "<b>Total PO Amount </b>: $" + TotalPOAmount.ToString("f2");
            }
            else
            {
                gvOldVendorPO.DataSource = null;
                gvOldVendorPO.DataBind();
                gvOldVendorPO.Visible = true;
                trTotal.Visible = false;
                printPOid.Visible = false;
                //gvOldPOrder.Visible = false;
            }
        }

        /// <summary>
        /// Makes the positive.
        /// </summary>
        /// <param name="Adjustments">string Adjustments</param>
        /// <returns>Returns the Positive Value.</returns>
        public String MakePositive(string Adjustments)
        {
            try
            {
                Decimal Adjust = Convert.ToDecimal(Adjustments);
                if (Adjust < 0)
                    return "$" + (-Adjust) + "(-)";
                else if (Adjust == 0)
                    return "$0.00";
                else
                    return "$" + (Adjust) + "(+)";
            }
            catch { return "$0.00"; }
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            DateTime from = new DateTime();
            DateTime to = new DateTime();
            try
            {
                from = Convert.ToDateTime(txtFromDate.Text.ToString());
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('Please enter valid From Date.')", true);
                return;
            }
            try
            {
                to = Convert.ToDateTime(txtToDate.Text.ToString());
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('Please enter valid To Date.')", true);
                return;
            }
            if (from > to)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "alert('Please enter valid Date Range.')", true);
                return;
            }

            if (Request.QueryString["ONo"] != null)
            {
                //BindCart(Convert.ToInt32(Request.QueryString["ONo"]), from, to);
            }
            else if (Request.QueryString["VendorID"] != "")
            {
                BindVendorCart(Request.QueryString["VendorID"].ToString().Trim(), 0);
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowAll_Click(object sender, ImageClickEventArgs e)
        {
            BindVendorCart(Request.QueryString["VendorID"].ToString().Trim(), 1);
        }

        /// <summary>
        /// Old Vendor PO Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void gvOldVendorPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.HtmlControls.HtmlAnchor lnkPo = (System.Web.UI.HtmlControls.HtmlAnchor)e.Row.FindControl("lnkPo");
                Label lblPOName = (Label)e.Row.FindControl("lblPOName");
                Label lblOrdernumber = (Label)e.Row.FindControl("lblOrdernumber");
                if (lblPOName != null)
                {
                    lnkPo.Attributes.Add("onclick", "OpenCenterWindow('OldPurchaseOrderCart.aspx?Ono=" + lblOrdernumber.Text.ToString() + "&Type=VendorPO&PONo=" + lblPOName.Text.ToString() + "',886,600)");
                }
            }
        }
    }
}