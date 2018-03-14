using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class StoreCreditDetails : Solution.UI.Web.BasePage
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
                GetAmount();
            }
        }

        /// <summary>
        /// Gets the Amount for the Store Credit
        /// </summary>
        private void GetAmount()
        {
            DataSet dsAmt = new DataSet();
            trCredit.Visible = false;
            grdRMARequestList.DataSource = null;
            grdRMARequestList.DataBind();
            dsAmt = CommonComponent.GetCommonDataSet("SELECT dbo.tb_GiftCard.SerialNumber AS CouponsCode, dbo.tb_GiftCard.InitialAmount, dbo.tb_GiftCard.Balance, " +
                      " dbo.tb_GiftCard.ExpirationDate, dbo.tb_Customer.FirstName + '  ' + dbo.tb_Customer.LastName AS CustName,  " +
                      " dbo.tb_GiftCard.RMAnumber AS ReturnItemID, ISNULL(dbo.tb_GiftCardUsage.Amount, 0) AS Amount,  " +
                      " dbo.tb_GiftCardUsage.CreatedOn AS usedDate " +
                      " FROM dbo.tb_Customer INNER JOIN " +
                      " dbo.tb_GiftCard ON dbo.tb_Customer.CustomerID = dbo.tb_GiftCard.CustomerID LEFT OUTER JOIN " +
                      " dbo.tb_GiftCardUsage ON dbo.tb_GiftCard.GiftCardID = dbo.tb_GiftCardUsage.GiftCardID  where tb_GiftCard.RMANumber=" + Convert.ToInt32(Request.QueryString["RMA"]) + "");
            if (dsAmt != null && dsAmt.Tables.Count > 0 && dsAmt.Tables[0].Rows.Count > 0)
            {
                trCredit.Visible = true;
                grdRMARequestList.DataSource = dsAmt;
                grdRMARequestList.DataBind();
            }
            else
            {
                trCredit.Visible = false;
                grdRMARequestList.DataSource = null;
                grdRMARequestList.DataBind();
            }
        }

        /// <summary>
        /// RMA Request List Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdRMARequestList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblAmt = (Label)e.Row.FindControl("lbltotalamt");
                Label lblReason = (Label)e.Row.FindControl("lblReason");
                Label lblbalance = (Label)e.Row.FindControl("lblbalance");
                lbltotal.Text = "$" + Math.Round(Convert.ToDecimal(lblAmt.Text.ToString()), 2).ToString();
                lblremaining.Text = "$" + Math.Round(Convert.ToDecimal(lblbalance.Text.ToString()), 2).ToString();
            }
        }
    }
}