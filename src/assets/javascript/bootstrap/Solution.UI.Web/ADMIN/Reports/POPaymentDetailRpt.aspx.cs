using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Collections;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class POPaymentDetailRpt : BasePage
    {
        string[] strSelPurchaseOr = new string[100];
        decimal TotPOAmt = 0;
        decimal TotPaidAmt = 0;
        decimal TotRemainingAmt = 0;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtMailFrom.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtMailTo.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                BindStore();
                BindPOPayemntList();
                BindVendorName();
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
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
            {
                AppConfig.StoreID = 1;
                ddlStore.SelectedIndex = 0;
            }
        }


        /// <summary>
        /// Binds the PO Payment List
        /// </summary>
        private void BindPOPayemntList()
        {
            string strSql = "";
            DataSet dsPOPayment = new DataSet();

            if (txtMailFrom.Text.ToString() != "" && txtMailTo.Text.ToString() != "")
            {
                try { DateTime ds = Convert.ToDateTime(txtMailFrom.Text.ToString()); }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }

                try { DateTime ds = Convert.ToDateTime(txtMailTo.Text.ToString()); }
                catch
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }

                if (Convert.ToDateTime(txtMailTo.Text.ToString()) >= Convert.ToDateTime(txtMailFrom.Text.ToString()))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }
            else
            {
                if (txtMailFrom.Text.ToString() == "" && txtMailTo.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
                else if (txtMailTo.Text.ToString() == "" && txtMailFrom.Text.ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please select valid date.', 'Message');", true);
                    return;
                }
            }

            if (txtMailFrom.Text.ToString() != "")
            {
                strSql += " AND Convert(char(10),po.PODate,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailFrom.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            if (txtMailTo.Text.ToString() != "")
            {
                strSql += " AND Convert(char(10),po.PODate,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtMailTo.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }

            if (ddlVendor.SelectedIndex > 0)
            {
                strSql += " AND tb_Vendor.vendorid=" + ddlVendor.SelectedValue + "";
            }

            strSql = "select po.PONumber,po.PODate,tb_Vendor.Name as VendorName,ISNULL(po.AdditionalCost,0) as AdditionalCost, isnull(po.POAmount,0) as 'POAmount', " +
                            " sum(isnull(vpo.PaidAmount,0)) as PaidAmount,ISNULL(isnull(po.POAmount,0)-sum(isnull(vpo.PaidAmount,0)),0) as RemainingAmt " +
                            " ,po.VendorID, po.OrderNumber,isnull(po.PaymentComplete,0) as PaymentComplete " +
                            " from tb_PurchaseOrder po left outer join dbo.tb_vendorpaymentPurchaseOrder VPO on vpo.PONumber=po.PONumber  " +
                            " Inner Join tb_Vendor on po.VendorID=tb_Vendor.VendorID " +
                            " where isnull(tb_Vendor.Deleted,0)=0 and  isnull(tb_Vendor.Active,1)=1 " + strSql.ToString() + " " +
                            " group by po.PODate,tb_Vendor.Name,po.AdditionalCost,po.POAmount,po.PONumber,po.VendorID, po.OrderNumber,po.PaymentComplete Order by po.PODate desc,po.PONumber Desc";

            dsPOPayment = CommonComponent.GetCommonDataSet(strSql.ToString());
            if (dsPOPayment != null && dsPOPayment.Tables.Count > 0 && dsPOPayment.Tables[0].Rows.Count > 0)
            {
                grvPOPayemntList.DataSource = dsPOPayment;
                grvPOPayemntList.DataBind();

                decimal POAmt = 0, PaidAmt = 0, RemiAmt = 0;
                trTotalDisplay.Visible = true;
                for (int l = 0; l < dsPOPayment.Tables[0].Rows.Count; l++)
                {
                    if (!string.IsNullOrEmpty(dsPOPayment.Tables[0].Rows[l]["POAmount"].ToString()))
                        POAmt = POAmt + Convert.ToDecimal(dsPOPayment.Tables[0].Rows[l]["POAmount"].ToString());
                    if (!string.IsNullOrEmpty(dsPOPayment.Tables[0].Rows[l]["PaidAmount"].ToString()))
                        PaidAmt = PaidAmt + Convert.ToDecimal(dsPOPayment.Tables[0].Rows[l]["PaidAmount"].ToString());
                    if (!string.IsNullOrEmpty(dsPOPayment.Tables[0].Rows[l]["RemainingAmt"].ToString()))
                        RemiAmt = RemiAmt + Convert.ToDecimal(dsPOPayment.Tables[0].Rows[l]["RemainingAmt"].ToString());
                }
                lblTotPoAmt.Text = POAmt.ToString("c");
                lblTotPaidAmt.Text = PaidAmt.ToString("c");
                lblRemBalAmt.Text = RemiAmt.ToString("c");
            }
            else
            {
                grvPOPayemntList.DataSource = null;
                grvPOPayemntList.DataBind();
            }
        }

        /// <summary>
        /// Card Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvPOPayemntList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label lblPAmount = (Label)e.Row.FindControl("lblPAmount");
                    Label lblPaidAmount = (Label)e.Row.FindControl("lblPaidAmount");
                    Label lblRemaingAmount = (Label)e.Row.FindControl("lblRemaingAmount");
                    Label lblONumber = (Label)e.Row.FindControl("lblONumber");
                    Label lblPONumber = (Label)e.Row.FindControl("lblPONum");

                    if (lblONumber != null && !string.IsNullOrEmpty(lblONumber.Text.ToString()) && lblONumber.Text.ToString() == "0")
                    {
                        lblONumber.Text = "WH PO";
                    }

                    TotPOAmt = TotPOAmt + Convert.ToDecimal(lblPAmount.Text.Trim());
                    TotPaidAmt = TotPaidAmt + Convert.ToDecimal(lblPaidAmount.Text.Trim());
                    TotRemainingAmt = TotRemainingAmt + Convert.ToDecimal(lblRemaingAmount.Text.Trim());
                }
                if (e.Row.RowType == DataControlRowType.Footer)
                {
                    Label lblTPAmount = (Label)e.Row.FindControl("lblTPAmount");
                    Label lblTotalPAmount = (Label)e.Row.FindControl("lblTotalPaidAmount");
                    Label lblTotalRemAmount = (Label)e.Row.FindControl("lblTotalRemaining");

                    if (lblTPAmount != null)
                        lblTPAmount.Text = Convert.ToDecimal(TotPOAmt).ToString("f2");

                    if (lblTotalPAmount != null)
                        lblTotalPAmount.Text = Convert.ToDecimal(TotPaidAmt).ToString("f2");

                    if (lblTotalRemAmount != null)
                        lblTotalRemAmount.Text = Convert.ToDecimal(TotRemainingAmt).ToString("f2");
                }
            }
            catch (Exception ex)
            { }
        }

        /// <summary>
        /// Binds the Name of the Vendors
        /// </summary>
        private void BindVendorName()
        {
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("SElect VendorID,Name from tb_Vendor where ISNULL(Active,1)=1 and ISNULL(Deleted,0)=0");
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ddlVendor.DataSource = ds;
                ddlVendor.DataValueField = "VendorID";
                ddlVendor.DataTextField = "Name";
                ddlVendor.DataBind();
                ddlVendor.Items.Insert(0, new ListItem("All", "-1"));
            }
            else
            {
                ddlVendor.DataSource = null;
                ddlVendor.DataBind();
            }
        }

        /// <summary>
        ///  PO Payment List Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvPOPayemntList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvPOPayemntList.PageIndex = e.NewPageIndex;
            BindPOPayemntList();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            grvPOPayemntList.PageIndex = 0;
            BindPOPayemntList();
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            grvPOPayemntList.PageIndex = 0;
            ddlVendor.SelectedIndex = 0;
            BindPOPayemntList();
        }
    }
}