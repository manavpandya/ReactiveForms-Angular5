using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class FiledTransactionSummary : Solution.UI.Web.BasePage
    {
        StoreComponent stac = new StoreComponent();

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
                txtFromDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                txtToDate.Text = System.DateTime.Now.ToString("MM/dd/yyyy");
                BindStore();
                btnSubmit_Click(null, null);
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlstore.DataSource = Storelist;
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "StoreID";
            }
            else
            {
                ddlstore.DataSource = null;
            }
            ddlstore.DataBind();
            ddlstore.Items.Insert(0, new ListItem("All Store", "0"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlstore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue);
            }
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
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid From Date.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(txtToDate.Text.ToString()))
                    to = Convert.ToDateTime(txtToDate.Text.ToString());
            }
            catch
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid From Date.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                return;
            }
            if (!string.IsNullOrEmpty(txtToDate.Text.ToString()) && from > to)
            {
                Page.ClientScript.RegisterStartupScript(this.GetType(), "msg", "jAlert('Please enter valid Date Range.', 'Message', 'ContentPlaceHolder1_txtFromDate')", true);
                return;
            }

            string WheClus = "";
            DataSet DsFailTranOrder = new DataSet();
            if (ddlstore.SelectedIndex != 0)
            {
                if (!string.IsNullOrEmpty(txtFromDate.Text.ToString()) && !string.IsNullOrEmpty(txtToDate.Text.ToString()))
                {
                    WheClus = "Select * from tb_FailedTransaction where CONVERT(varchar(10),OrderDate,101) >= '" + txtFromDate.Text + "' and  " +
                              " '" + txtToDate.Text + "' >= CONVERT(varchar(10),OrderDate,101) and StoreID =" + ddlstore.SelectedValue + " ";
                }
                else
                {
                    WheClus = "Select CONVERT(varchar(10),OrderDate,101),* from tb_FailedTransaction where CONVERT(varchar(10),OrderDate,101) >= '" + txtFromDate.Text + "' and StoreID =" + ddlstore.SelectedValue + "";
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(txtFromDate.Text.ToString()) && !string.IsNullOrEmpty(txtToDate.Text.ToString()))
                {
                    WheClus = "Select * from tb_FailedTransaction where CONVERT(varchar(10),OrderDate,101) >= '" + txtFromDate.Text + "' and  " +
                              " '" + txtToDate.Text + "' >= CONVERT(varchar(10),OrderDate,101) ";
                }
                else
                {
                    WheClus = "Select CONVERT(varchar(10),OrderDate,101),* from tb_FailedTransaction where CONVERT(varchar(10),OrderDate,101) >= '" + txtFromDate.Text + "'";
                }
            }
            DsFailTranOrder = CommonComponent.GetCommonDataSet(WheClus.ToString());

            string StrOrderNum = "";
            string OrderSumm = "";
            decimal OrderTotal = 0;
            if (DsFailTranOrder != null && DsFailTranOrder.Tables.Count > 0 && DsFailTranOrder.Tables[0].Rows.Count > 0)
            {
                lblOrderCount.Text = DsFailTranOrder.Tables[0].Rows.Count.ToString();
                for (int i = 0; i < DsFailTranOrder.Tables[0].Rows.Count; i++)
                {
                    OrderSumm += DsFailTranOrder.Tables[0].Rows[i]["OrderNumber"].ToString() + ",";
                    StrOrderNum += "<a onclick=\"OpenCenterWindow('ViewFailedTransaction.aspx?ONo=" + DsFailTranOrder.Tables[0].Rows[i]["OrderNumber"].ToString() + "',900,600);\" href=\"javascript:void(0);\">" + DsFailTranOrder.Tables[0].Rows[i]["OrderNumber"].ToString() + "</a> ,";
                }
                if (StrOrderNum.Length > 0)
                {
                    StrOrderNum = StrOrderNum.Substring(0, StrOrderNum.Length - 1);
                    OrderSumm = OrderSumm.Substring(0, OrderSumm.Length - 1);
                    OrderTotal = Convert.ToDecimal(CommonComponent.GetScalarCommonData("Select ISNULL(SUM(ordertotal),0) from tb_Order where OrderNumber in (" + OrderSumm + ")"));
                }
                ltrOrderNumber.Text = StrOrderNum.ToString();
                lblOrderTotal.Text = OrderTotal.ToString("c");
            }
            else
            {
                ltrOrderNumber.Text = "---";
                lblOrderCount.Text = "0";
                lblOrderTotal.Text = OrderTotal.ToString("c");
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlstore_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSubmit_Click(null, null);
        }
    }
}