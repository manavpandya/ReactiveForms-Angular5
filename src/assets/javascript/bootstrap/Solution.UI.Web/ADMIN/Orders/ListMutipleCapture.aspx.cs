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

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class ListMutipleCapture : BasePage
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
                BindMultiCapture();
            }
            btnCaptureMultiTop.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/multi-capture.gif";
            ImageButton1.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/multi-capture.gif";
        }

        /// <summary>
        /// Binds the multi capture Data function.
        /// </summary>
        private void BindMultiCapture()
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
                strWhereClus += " AND Convert(char(10),OrderDate,101) >= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtFromDate.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }
            if (txtToDate.Text.ToString() != "")
            {

                strWhereClus += " AND Convert(char(10),OrderDate,101) <= Convert(char(10),'" + String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(txtToDate.Text.ToString().Replace(" 00:00:00", "").Replace(" ", ""))) + "',101)";
            }

            //if (ddlStore.SelectedIndex != 0)
            //{
            strWhereClus += "and tb_Order.StoreID=" + ddlStore.SelectedValue.ToString() + "";
            //}

            if (ddlSearch.SelectedIndex > 0 && !string.IsNullOrEmpty(txtSearch.Text.ToString().Trim()))
            {
                strWhereClus += "AND tb_Order.OrderNumber =" + txtSearch.Text.ToString().Replace("'", "''") + "";
            }

            dsOrder = OrderComponent.GetorderListForMultiCapture(strWhereClus.ToString(), 1);
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                grvMultiCapture.DataSource = dsOrder;
                grvMultiCapture.DataBind();
                trBottom.Visible = true;
                trtop.Visible = true;
            }
            else
            {
                grvMultiCapture.DataSource = null;
                grvMultiCapture.DataBind();
                trBottom.Visible = false;
                trtop.Visible = false;
            }
        }

        /// <summary>
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            grvMultiCapture.PageIndex = 0;
            BindMultiCapture();
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGo_Click(object sender, ImageClickEventArgs e)
        {
            grvMultiCapture.PageIndex = 0;
            BindMultiCapture();
        }

        /// <summary>
        ///  Search All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearchall_Click(object sender, ImageClickEventArgs e)
        {
            txtSearch.Text = "";
            grvMultiCapture.PageIndex = 0;
            BindMultiCapture();
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
            //ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
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
        /// Multi Capture Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grvMultiCapture_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Literal ltrMultiCapOrderNnumber = (Literal)e.Row.FindControl("ltrMultiCapOrderNnumber");
                Int32 OrderNumber = Convert.ToInt32(ltrMultiCapOrderNnumber.Text.Trim());
                ltrMultiCapOrderNnumber.Text = "<a class=\"order-no\" onclick=\"chkHeight();\" href=\"/Admin/Orders/Orders.aspx?id=" + ltrMultiCapOrderNnumber.Text.ToString() + "\">" + ltrMultiCapOrderNnumber.Text.ToString() + "</a><br />";
            }
        }

        /// <summary>
        ///  Multi Capture Top Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnCaptureMultiTop_Click(object sender, ImageClickEventArgs e)
        {
            if (grvMultiCapture.Rows.Count > 0)
            {
                string strResult = "OK";
                string StrPrintMsg = "";
                for (int i = 0; i < grvMultiCapture.Rows.Count; i++)
                {
                    CheckBox ChkSelect = (CheckBox)grvMultiCapture.Rows[i].FindControl("chkSelectCaptureMultiOrder");
                    if (ChkSelect.Checked)
                    {
                        Label lblMultiCapOrderNnumber = (Label)grvMultiCapture.Rows[i].FindControl("lblMultiCapOrderNnumber");
                        if (lblMultiCapOrderNnumber != null)
                        {
                            Int32 OrderNum = Convert.ToInt32(lblMultiCapOrderNnumber.Text.ToString());

                            strResult = "";
                            tb_Order objOrder = new tb_Order();
                            DataSet dsAutho = new DataSet();
                            objOrder.CaptureTxCommand = "";
                            objOrder.CaptureTXResult = "";
                            objOrder.AuthorizationPNREF = "";
                            dsAutho = CommonComponent.GetCommonDataSet("SELECT isnull(AuthorizationPNREF,'') as AuthorizationPNREF,isnull(PaymentGateway,'') as PaymentGateway,isnull(OrderTotal,0) as OrderTotal,isnull(TransactionStatus,'') as TransactionStatus FROM tb_order where OrderNumber=" + OrderNum.ToString() + "");
                            string PaymentGAteWay = "";
                            string Transactionstatus = "";
                            if (dsAutho != null && dsAutho.Tables.Count > 0 && dsAutho.Tables[0].Rows.Count > 0)
                            {
                                objOrder.AuthorizationPNREF = dsAutho.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                                PaymentGAteWay = dsAutho.Tables[0].Rows[0]["PaymentGateway"].ToString();
                                objOrder.OrderTotal = Convert.ToDecimal(dsAutho.Tables[0].Rows[0]["OrderTotal"].ToString());
                                Transactionstatus = Convert.ToString(dsAutho.Tables[0].Rows[0]["TransactionStatus"].ToString());
                                objOrder.OrderNumber = Convert.ToInt32(OrderNum.ToString());
                            }
                            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypal")
                            {
                                PayPalComponent objPay = new PayPalComponent();
                                strResult = objPay.CaptureOrder(objOrder);
                            }
                            else if (PaymentGAteWay.ToString().ToLower().Trim() == "authorizenet")
                            {
                                if (Transactionstatus.ToLower().Trim() == "authorized")
                                {

                                    tb_Order objorderCapture = new tb_Order();
                                    OrderComponent objCapture = new OrderComponent();
                                    objorderCapture = objCapture.GetOrderByOrderNumber(Convert.ToInt32(OrderNum.ToString()));
                                    strResult = AuthorizeNetComponent.CaptureOrderFirst(objOrder);
                                    Decimal AuthorizedAmount1 = Convert.ToDecimal(objorderCapture.AuthorizedAmount.ToString());
                                    Decimal AuthorizedAmount = Convert.ToDecimal(objorderCapture.OrderTotal) - AuthorizedAmount1;
                                    if (AuthorizedAmount > Convert.ToDecimal(0) && AuthorizedAmount1 > Convert.ToDecimal(0) && strResult == "OK")
                                    {

                                        String AVSResult = String.Empty;
                                        String AuthorizationResult = String.Empty;
                                        String AuthorizationCode = String.Empty;
                                        String AuthorizationTransID = String.Empty;
                                        String TransactionCommand = String.Empty;
                                        String TransactionResponse = String.Empty;
                                        string strResult1 = AuthorizeNetComponent.ProcessCardForYahooorderAdmin(Convert.ToInt32(OrderNum.ToString()), Convert.ToInt32(objorderCapture.CustomerID), Convert.ToDecimal(AuthorizedAmount.ToString()), AppLogic.AppConfigBool("UseLiveTransactions"), "auth_capture", objorderCapture, objorderCapture, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                                        if (strResult1 == "OK")
                                        {
                                            try
                                            {
                                                objOrder.AuthorizationCode += "," + AuthorizationCode.ToString();
                                                objOrder.AuthorizationPNREF += "|CAPTURE=" + AuthorizationTransID.ToString();
                                                objOrder.CaptureTXResult += "," + AuthorizationResult.ToString();
                                                objOrder.CaptureTxCommand += "," + TransactionCommand.ToString();
                                                objOrder.AVSResult += "," + AVSResult;
                                            }
                                            catch
                                            {

                                            }
                                            //CommonComponent.ExecuteCommonData("UPDATE tb_order SET AuthorizedAmount=AuthorizedAmount + " + AuthorizedAmount.ToString() + " WHERE orderNumber=" + Request.QueryString["id"].ToString() + "");

                                        }
                                    }

                                }

                            }
                            if (strResult == "OK")
                            {
                                OrderComponent objOrderlog = new OrderComponent();
                                objOrderlog.InsertOrderlog(2, Convert.ToInt32(OrderNum.ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                                CommonComponent.ExecuteCommonData("UPDATE tb_order SET CaptureTXCommand='" + objOrder.CaptureTxCommand.ToString().Replace("'", "''") + "',CaptureTXResult='" + objOrder.CaptureTXResult.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + objOrder.AuthorizationPNREF.ToString().Replace("'", "''") + "', CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='CAPTURED' WHERE orderNumber=" + OrderNum.ToString() + "");
                                StrPrintMsg += OrderNum + @" : Your Transaction State has been CAPTURED.<br />";
                            }
                            else
                            {
                                StrPrintMsg += OrderNum + @" : Your Transaction has been Failed.<br />";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(StrPrintMsg.ToString()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + StrPrintMsg.ToString() + "','Message');", true);
                }
                BindMultiCapture();
            }
        }

        /// <summary>
        ///  Multi Capture Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void ImgMultiCap_Click(object sender, ImageClickEventArgs e)
        {
            if (grvMultiCapture.Rows.Count > 0)
            {
                string strResult = "OK";
                string StrPrintMsg = "";
                for (int i = 0; i < grvMultiCapture.Rows.Count; i++)
                {
                    CheckBox ChkSelect = (CheckBox)grvMultiCapture.Rows[i].FindControl("chkSelectCaptureMultiOrder");
                    if (ChkSelect.Checked)
                    {
                        Label lblMultiCapOrderNnumber = (Label)grvMultiCapture.Rows[i].FindControl("lblMultiCapOrderNnumber");
                        if (lblMultiCapOrderNnumber != null)
                        {
                            Int32 OrderNum = Convert.ToInt32(lblMultiCapOrderNnumber.Text.ToString());

                            strResult = "";
                            tb_Order objOrder = new tb_Order();
                            DataSet dsAutho = new DataSet();
                            objOrder.CaptureTxCommand = "";
                            objOrder.CaptureTXResult = "";
                            objOrder.AuthorizationPNREF = "";
                            string Transactionstatus = "";
                            dsAutho = CommonComponent.GetCommonDataSet("SELECT isnull(AuthorizationPNREF,'') as AuthorizationPNREF,isnull(PaymentGateway,'') as PaymentGateway,isnull(OrderTotal,0) as OrderTotal,isnull(TransactionStatus,'') as TransactionStatus FROM tb_order where OrderNumber=" + OrderNum.ToString() + "");
                            string PaymentGAteWay = "";
                            if (dsAutho != null && dsAutho.Tables.Count > 0 && dsAutho.Tables[0].Rows.Count > 0)
                            {
                                objOrder.AuthorizationPNREF = dsAutho.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                                PaymentGAteWay = dsAutho.Tables[0].Rows[0]["PaymentGateway"].ToString();
                                objOrder.OrderTotal = Convert.ToDecimal(dsAutho.Tables[0].Rows[0]["OrderTotal"].ToString());
                                Transactionstatus = Convert.ToString(dsAutho.Tables[0].Rows[0]["TransactionStatus"].ToString());
                                objOrder.OrderNumber = Convert.ToInt32(OrderNum.ToString());
                            }
                            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypal")
                            {
                                PayPalComponent objPay = new PayPalComponent();
                                strResult = objPay.CaptureOrder(objOrder);
                            }
                            else if (PaymentGAteWay.ToString().ToLower().Trim() == "authorizenet")
                            {
                                if (Transactionstatus.ToLower().Trim() == "authorized")
                                {
                                    //strResult = AuthorizeNetComponent.CaptureOrder(objOrder);

                                    tb_Order objorderCapture = new tb_Order();
                                    OrderComponent objCapture = new OrderComponent();
                                    objorderCapture = objCapture.GetOrderByOrderNumber(Convert.ToInt32(OrderNum.ToString()));
                                    strResult = AuthorizeNetComponent.CaptureOrderFirst(objOrder);
                                    Decimal AuthorizedAmount1 = Convert.ToDecimal(objorderCapture.AuthorizedAmount.ToString());
                                    Decimal AuthorizedAmount = Convert.ToDecimal(objorderCapture.OrderTotal) - AuthorizedAmount1;
                                    if (AuthorizedAmount > Convert.ToDecimal(0) && AuthorizedAmount1 > Convert.ToDecimal(0) && strResult == "OK")
                                    {

                                        String AVSResult = String.Empty;
                                        String AuthorizationResult = String.Empty;
                                        String AuthorizationCode = String.Empty;
                                        String AuthorizationTransID = String.Empty;
                                        String TransactionCommand = String.Empty;
                                        String TransactionResponse = String.Empty;
                                        string strResult1 = AuthorizeNetComponent.ProcessCardForYahooorderAdmin(Convert.ToInt32(OrderNum.ToString()), Convert.ToInt32(objorderCapture.CustomerID), Convert.ToDecimal(AuthorizedAmount.ToString()), AppLogic.AppConfigBool("UseLiveTransactions"), "auth_capture", objorderCapture, objorderCapture, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                                        if (strResult1 == "OK")
                                        {
                                            try
                                            {
                                                objOrder.AuthorizationCode += "," + AuthorizationCode.ToString();
                                                objOrder.AuthorizationPNREF += "|CAPTURE=" + AuthorizationTransID.ToString();
                                                objOrder.CaptureTXResult += "," + AuthorizationResult.ToString();
                                                objOrder.CaptureTxCommand += "," + TransactionCommand.ToString();
                                                objOrder.AVSResult += "," + AVSResult;
                                            }
                                            catch
                                            {

                                            }
                                            //CommonComponent.ExecuteCommonData("UPDATE tb_order SET AuthorizedAmount=AuthorizedAmount + " + AuthorizedAmount.ToString() + " WHERE orderNumber=" + Request.QueryString["id"].ToString() + "");

                                        }
                                    }
                                }

                            }
                            if (strResult == "OK")
                            {
                                OrderComponent objOrderlog = new OrderComponent();
                                objOrderlog.InsertOrderlog(2, Convert.ToInt32(OrderNum.ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                                CommonComponent.ExecuteCommonData("UPDATE tb_order SET CaptureTXCommand='" + objOrder.CaptureTxCommand.ToString().Replace("'", "''") + "',CaptureTXResult='" + objOrder.CaptureTXResult.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + objOrder.AuthorizationPNREF.ToString().Replace("'", "''") + "', CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='CAPTURED' WHERE orderNumber=" + OrderNum.ToString() + "");
                                StrPrintMsg += OrderNum + @" : Your Transaction State has been CAPTURED.<br />";
                            }
                            else
                            {
                                StrPrintMsg += OrderNum + @" : Your Transaction has been Failed.<br />";
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(StrPrintMsg.ToString()))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + StrPrintMsg.ToString() + "','Message');", true);
                }
                BindMultiCapture();
            }
        }

        /// <summary>
        ///  Multi Capture Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grvMultiCapture_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grvMultiCapture.PageIndex = e.NewPageIndex;
            BindMultiCapture();
        }
    }
}