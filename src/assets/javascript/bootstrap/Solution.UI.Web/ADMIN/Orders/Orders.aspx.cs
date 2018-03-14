using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System.Net;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Data.SqlClient;

namespace Solution.UI.Web.ADMIN.Orders
{
    /// <summary>
    /// Order For Order related Information By Order #     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public partial class Orders : BasePage
    {
        DataSet dsOrder = null;
        public Boolean IsRefund = false;
        public string PaymentMethod = "";

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Id"] != null)
            {
                Int32 StoreID = 0;
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + Convert.ToInt32(Request.QueryString["ID"]))), out StoreID);
                AppConfig.StoreID = StoreID;
                creditcarddetails.Visible = false;

                PaymentMethod = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(paymentmethod,'') as paymentmethod from tb_order where ordernumber=" + Convert.ToInt32(Request.QueryString["ID"]).ToString() + ""));
                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")) && PaymentMethod.ToString() != "" && PaymentMethod.ToString().ToLower().IndexOf("paypal") <= -1)
                {
                    btnCapture.Visible = false;
                    btnRefund.Visible = false;
                    //  btnUploadOrder.Visible = false;
                    btnCancelOrder.Visible = false;
                }
            }

            if (!IsPostBack)
            {
                if (ViewState["urlreffer"] == null && Request.UrlReferrer != null)
                {
                    ViewState["urlreffer"] = Request.UrlReferrer.ToString();
                }

                frmUpdateOrder.Attributes.Add("src", "ApproveOrder.aspx?ONO=" + Convert.ToInt32(Request.QueryString["ID"]).ToString());

                FillStatusColor();
                Page.MaintainScrollPositionOnPostBack = true;
                ButtonImages();
                GetOrderDocuments();
                frmInvoice.Attributes.Add("onload", "iframeAutoheight(this);");
                frmInvoice.Attributes.Add("src", "Invoice.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");
                frmfullInvoice.Attributes.Add("onload", "iframeAutoheight(this);");
                //frmfullInvoice.Attributes.Add("src", "InvoiceFull.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");
                frmfullInvoice.Attributes.Add("src", "Invoice_Sendmail.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                //frmPickingSlip.Attributes.Add("onload", "iframeAutoheight(this);");
                //frmPickingSlip.Attributes.Add("src", "BulkPickingSlip.aspx?Ono=" + Convert.ToInt32(Request.QueryString["ID"]).ToString());

                btnReset.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgPrevious.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/icon/previous.png";
                imgNext.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/icon/next.png";
                popupContactClose.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel-icon.png";
                btnUploadOrder.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/upload-orders-to-nav.gif";

                btnUploadCancelOrder.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/upload-cancel-order.gif";
                btnaAllowIP.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/allow-this-ip.png";
                btnBlockIP.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/block-this-ip.png";

                if (Request.QueryString["Id"] != null)
                {
                    string Str = Request.QueryString["Id"].ToString().Trim();
                    int Num;
                    bool isNum = int.TryParse(Str, out Num);
                    if (isNum)
                    {
                        Int32 POStoreID = 0;
                        Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + Convert.ToInt32(Request.QueryString["ID"]))), out POStoreID);

                        string StoreName = "";
                        StoreName = Convert.ToString(CommonComponent.GetScalarCommonData("select StoreName from tb_Store where StoreID in (select StoreID from tb_order where OrderNumber=" + Convert.ToInt32(Request.QueryString["ID"]) + ")"));
                        if (!string.IsNullOrEmpty(StoreName) && StoreName != null && StoreName.Length > 0)
                            ltStoreName.Text = "Store: " + StoreName;

                        if (Request.QueryString["PO"] != null)
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "loadPO", "Tabdisplay(2);", true);

                        frmPackingSlip.Attributes.Add("onload", "iframeAutoheight(this);");
                        if (POStoreID == 4)
                        {
                            frmPackingSlip.Attributes.Add("src", "PackingSlipMultiwarehouse.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");
                        }
                        else
                        {
                            frmPackingSlip.Attributes.Add("src", "PackingSlipMultiwarehouse.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");
                        }
                        frmorderLog.Attributes.Add("onload", "iframeAutoheight(this);");
                        frmorderLog.Attributes.Add("src", "OrderLog.aspx?Ono=" + Request.QueryString["Id"].ToString() + "");

                        if (POStoreID == 3 || POStoreID == 4 || POStoreID == 1)
                        {
                            DataSet dsorder = new DataSet();
                            dsorder = CommonComponent.GetCommonDataSet("Select ISNULL(IsBackEnd,0) as IsBackEnd,ISNULL(BackEndGUID,'') as BackEndGUID,isnull(isNAVInserted,0) as isNAVInserted,isnull(isnavcompleted,0) as isnavcompleted from tb_order where ordernumber=" + Request.QueryString["Id"].ToString() + "");
                            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
                            {


                                if (dsorder.Tables[0].Rows[0]["IsBackEnd"].ToString() == "1" && string.IsNullOrEmpty(dsorder.Tables[0].Rows[0]["BackEndGUID"].ToString().Trim()))
                                {
                                    btnUploadOrder.Visible = true;
                                    btnUploadCancelOrder.Visible = false;
                                }
                                else if (dsorder.Tables[0].Rows[0]["IsBackEnd"].ToString() == "0" && string.IsNullOrEmpty(dsorder.Tables[0].Rows[0]["BackEndGUID"].ToString().Trim()))
                                {
                                    btnUploadCancelOrder.Visible = true;
                                    btnUploadOrder.Visible = false;
                                }


                                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")))
                                {
                                    if ((dsorder.Tables[0].Rows[0]["isNAVInserted"].ToString() == "1" || dsorder.Tables[0].Rows[0]["isNAVInserted"].ToString().ToLower() == "true") && dsorder.Tables[0].Rows[0]["isnavcompleted"].ToString() == "0" || dsorder.Tables[0].Rows[0]["isNAVInserted"].ToString().ToLower() == "false")
                                    {
                                        btnUploadOrder.Visible = true;

                                    }
                                    else
                                    {
                                        btnUploadOrder.Visible = false;
                                    }


                                    btnUploadCancelOrder.Visible = false;
                                }
                            }




                        }

                        if (Request.QueryString["PS"] != null)
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "loadPackingSlip", "Tabdisplay(3);", true);

                        GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["Id"].ToString()));


                        //Bellow line must be placed after GetOrderDetailsByOrderNumber(<OrderID>);
                        //frmOrderMail.Attributes.Add("src", "OrderEmail.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "&OrderEmail=" + Convert.ToString(hdnOrderEmail.Value) + "");
                        frmShippingLabel.Attributes.Add("src", "ShippingLabel.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                        frmChangeOrder.Attributes.Add("onload", "iframeAutoheight(this);");
                        frmChangeOrder.Attributes.Add("src", "ChangeOrder.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                        frmRMAReguest.Attributes.Add("onload", "iframeAutoheight(this);");
                        frmRMAReguest.Attributes.Add("src", "RMARequestList.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                        frmReguest.Attributes.Add("onload", "iframeAutoheight(this);");
                        frmReguest.Attributes.Add("src", "ReturnItem.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                        frmPurchaseOrder.Attributes.Add("onload", "iframeAutoheight(this);");
                        frmPurchaseOrder.Attributes.Add("src", "POOrder.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                        frmRefund.Attributes.Add("onload", "iframeAutoheight(this);");
                        frmRefund.Attributes.Add("src", "RMARefund.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                        frmShipping.Attributes.Add("onload", "iframeAutoheight(this);");
                        frmShipping.Attributes.Add("src", "Shipping.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                        frmHaming.Attributes.Add("onload", "iframeAutoheight(this);");
                        frmHaming.Attributes.Add("src", "OrderHaming.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(Request.QueryString["Id"].ToString())) + "");

                        // Check If Order is Refund
                        //string ReturnType = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ReturnType,'') as ReturnType from tb_ReturnItem where OrderedNumber=" + Request.QueryString["Id"].ToString().Trim() + ""));
                        //if (!string.IsNullOrEmpty(ReturnType.ToString().Trim()))
                        //{
                        //    if (ReturnType == "RR")
                        //        IsRefund = true;
                        //}
                        string ReturnType = Convert.ToString(CommonComponent.GetScalarCommonData("Select top 1 ISNULL(returnid,0) as returnid from tb_return where OrderedNumber=" + Request.QueryString["Id"].ToString().Trim() + ""));
                        if (!string.IsNullOrEmpty(ReturnType.ToString().Trim()))
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tabclick", "document.getElementById('li9').style.display = '';", true);
                        }
                        FillComment();
                        GetCreditCardType(POStoreID);

                        #region PackProduct

                        btnAdjustCapture.ImageUrl = "/App_Themes/" + Page.Theme + "/button/capture.png";
                        btnRefund_UpdateOrder.ImageUrl = "/App_Themes/" + Page.Theme + "/images/refund.png";
                        BindProducts();

                        #endregion PackProduct

                        if (Request.QueryString["refund"] != null)
                        {
                            IsRefund = true;
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tabclick", "Tabdisplay(10);chkHeight();iframereload('ContentPlaceHolder1_frmRefund');document.getElementById('prepage').style.display = 'none';", true);
                        }
                        if (Request.QueryString["return"] != null)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tabclick", "Tabdisplay(8);chkHeight();iframereload('ContentPlaceHolder1_frmReguest');document.getElementById('prepage').style.display = 'none';", true);
                        }
                        else if (Request.QueryString["PurchaseOrder"] != null) // After PO Completed
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "tabclick", "Tabdisplay(11);chkHeight();iframereload('ContentPlaceHolder1_frmPurchaseOrder');document.getElementById('prepage').style.display = 'none';", true);
                        }
                    }
                    else
                    {
                        EmptyDetails();
                    }
                }
                Session["vriantData"] = null;
                Session["StrtempOrderedCustomCartID"] = null;
                Session["vriantDataQty"] = null;

                ChkHamingValueMakeTabVisible(Convert.ToInt32(Request.QueryString["ID"]));
            }
            else
            {
                if (hdnHamingTab.Value == "1")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hamingtabvisi", "document.getElementById('li15').style.display = '';", true);
                    String strltrPrivate = ltrPrivate.Text.ToString().Replace("- Hemming Required", "");
                    strltrPrivate = strltrPrivate + "- Hemming Required";
                    ltrPrivate.Text = strltrPrivate;
                    CommonComponent.ExecuteCommonData("UPDATE tb_Order SET Notes='" + strltrPrivate.Replace("'", "''") + "' WHERE OrderNumber=" + Convert.ToInt32(Request.QueryString["ID"]) + "");
                }
            }
        }

        /// <summary>
        /// Button Image Theme  Wise
        /// </summary>
        private void ButtonImages()
        {
            btnShareComment.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/submit-comment.png) no-repeat transparent; width: 103px; height: 23px; border:none;cursor:pointer;");
            btnUpdateState.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/Update-Status.png) no-repeat transparent; width: 94px; height: 23px; border:none;cursor:pointer;");

            btnCapture.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/capture.png) no-repeat transparent; width: 74px; height: 23px; border:none;cursor:pointer;");
            btnRefund.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/refund.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
            btnRefund.OnClientClick = "OpenCenterWindow('RefundOrder.aspx?oNo=" + Request.QueryString["Id"] + "',600,300); return false;";

            btnForceRefund.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/force-refund.png) no-repeat transparent; width: 103px; height: 23px; border:none;cursor:pointer;");
            btnVoid.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/void.png) no-repeat transparent; width: 55px; height: 23px; border:none;cursor:pointer;");
            btnCancelOrder.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/cancel-order.png) no-repeat transparent; width: 102px; height: 23px; border:none;cursor:pointer;");
            btnSendEmail.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/send-email.png) no-repeat transparent; width: 85px; height: 23px; border:none;cursor:pointer;");
            //btnHold.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/button/hold.png) no-repeat transparent; width: 103px; height: 23px; border:none;cursor:pointer;");
        }

        /// <summary>
        /// Get Order Details By Order Number
        /// </summary>
        /// <param name="OrderNumber">Int Order Number</param>
        private void GetOrderDetailsByOrderNumber(Int32 OrderNumber)
        {

            dsOrder = new DataSet();
            dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["Id"].ToString()));
            DataSet dsNext = new DataSet();
            OrderComponent objNext = new OrderComponent();
            dsNext = objNext.GetnextpreviousOrder(Convert.ToInt32(Request.QueryString["Id"].ToString()));
            if (dsNext != null && dsNext.Tables.Count > 0 && dsNext.Tables[0].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsNext.Tables[0].Rows[0]["nextOrder"].ToString()) > 0)
                {
                    imgNext.OnClientClick = "javascript:chkHeight();window.location.href='/Admin/Orders/Orders.aspx?id=" + dsNext.Tables[0].Rows[0]["nextOrder"].ToString() + "'; return false;";
                    tdNext.Visible = true;
                }
                else
                {
                    tdNext.Visible = false;
                }
                if (Convert.ToInt32(dsNext.Tables[0].Rows[0]["previousOrder"].ToString()) > 0)
                {
                    imgPrevious.OnClientClick = "javascript:chkHeight();window.location.href='/Admin/Orders/Orders.aspx?id=" + dsNext.Tables[0].Rows[0]["previousOrder"].ToString() + "'; return false;";
                    tdPrevious.Visible = true;
                }
                else
                {
                    tdPrevious.Visible = false;
                }
            }
            else
            {
                tdNext.Visible = false;
                tdPrevious.Visible = false;
            }
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                /*********Order Status**********/
                ltrOrderNumber.Text = "<font style=\"font-size:20px;\">" + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + "</font>, ";
                ltrDate.Text = string.Format("{0:MMMMMMMMMM dd, yyyy hh:mm:ss ttt}", Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString()));

                string SalesAgentName = "";
                SalesAgentName = Convert.ToString(CommonComponent.GetScalarCommonData("SElect ISNULL(FirstName,'') +' '+ ISNULL(LastName,'') as SalesAgentName  from tb_Admin where ISNULL(Active,0)=1 and ISNULL(Deleted,0)=0 " +
                                        " and  adminid in (Select ISNULL(SalesAgentID,0) as SId from tb_Order where ISNULL(IsPhoneOrder,0)=1 and OrderNumber=" + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + ")"));

                ltrRefOrderNumber.Text = "";
                if (dsOrder.Tables[0].Rows[0]["RefOrderID"] != DBNull.Value && dsOrder.Tables[0].Rows[0]["RefOrderID"] != null)
                {
                    if (dsOrder.Tables[0].Rows[0]["RefOrderID"].ToString() != "" && dsOrder.Tables[0].Rows[0]["RefOrderID"].ToString() != "0")
                        ltrRefOrderNumber.Text = "Ref. Order # <font style=\"font-size:14px;\">" + Convert.ToString(dsOrder.Tables[0].Rows[0]["RefOrderID"]) + "</font>, ";
                }
                if (!string.IsNullOrEmpty(SalesAgentName.ToString()))
                {
                    ltrRefOrderNumber.Text += "&nbsp;(Phone Order by " + SalesAgentName.ToString() + " ), ";
                }

                Decimal OrderAdjTotal = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["orderTotal"]) + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"]) - Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["RefundedAmount"]);
                ltrOrderTotal.Text = OrderAdjTotal.ToString("C");
                hdnAmount.Value = (Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["orderTotal"]) + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"])).ToString();
                hdnrefundAmount.Value = dsOrder.Tables[0].Rows[0]["RefundedAmount"].ToString();
                aeditBill.Attributes.Add("onclick", "ShowAddressModel('id=" + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + "&type=bill');");
                aeditShip.Attributes.Add("onclick", "ShowAddressModel('id=" + dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString() + "&type=ship');");

                try
                {
                    if (Convert.ToString(dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString()).ToLower() == "hold")
                    {
                        ahold.Visible = false;
                    }
                    else
                    {
                        ahold.Visible = true;
                    }
                    ddlStatus.Items.FindByText(Convert.ToString(dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString())).Selected = true;

                    ddlStatus_SelectedIndexChanged(null, null);

                }
                catch
                {
                    ddlStatus.SelectedIndex = 0;
                    ddlStatus_SelectedIndexChanged(null, null);
                }

                if (dsOrder.Tables[0].Rows[0]["LastIPAddress"] != null)
                {
                    bool IPAddress = objNext.GetIPAddressDetail(dsOrder.Tables[0].Rows[0]["LastIPAddress"].ToString());
                    if (IPAddress == true)
                    {
                        btnaAllowIP.Visible = true;
                        btnBlockIP.Visible = false;
                    }
                    else
                    {
                        btnaAllowIP.Visible = false;
                        btnBlockIP.Visible = true;
                    }
                }

                if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() == "pending")
                {
                    ltrorderTranStatus.Text = "<b style=\"color:#D3321C;\">" + dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() + "</b>";
                    btnCapture.Visible = true;
                    btnVoid.Visible = true;
                    btnCancelOrder.Visible = true;
                    trrefund.Visible = true;
                    trcapture.Visible = true;




                    // trvoid.Visible = true;
                }
                else if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() == "authorized")
                {
                    ltrorderTranStatus.Text = "<b style=\"color:#FF7F00;\">" + dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() + "</b>";
                    btnCapture.Visible = true;
                    btnVoid.Visible = true;
                    btnCancelOrder.Visible = true;
                    trcapture.Visible = true;
                    //  trvoid.Visible = true;
                    trrefund.Visible = true;



                }
                else if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() == "captured")
                {
                    ltrorderTranStatus.Text = "<b style=\"color:#348934;\">" + dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() + "</b>";
                    if (Convert.ToString(dsOrder.Tables[0].Rows[0]["AuthorizationPNREF"]) != "" && Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"]) > decimal.Zero)
                    {
                        btnRefund.Visible = true;
                        trcapture.Visible = true;
                    }
                    else
                    {
                        btnRefund.Visible = false;
                        trcapture.Visible = false;
                        trrefund.Visible = true;
                        btnForceRefund.Visible = false;
                        btnCancelOrder.Visible = true;
                    }





                    // btnForceRefund.Visible = true;

                    // trrefund.Visible = true;
                }
                else if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() == "voided")
                {
                    ltrorderTranStatus.Text = "<b style=\"color:#1A1AC4;\">" + dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() + "</b>";
                }
                else if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() == "refunded")
                {
                    ltrorderTranStatus.Text = "<b style=\"color:#00AAFF;\">" + dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() + "</b>";
                    trrefund.Visible = false;
                    btnCancelOrder.Visible = true;
                    trcapture.Visible = false;
                }
                else if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() == "partially refunded")
                {
                    ltrorderTranStatus.Text = "<b style=\"color:#00AAFF;\">" + dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() + "</b>";
                    btnRefund.Visible = true;
                    trcapture.Visible = true;
                }
                else if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() == "canceled")
                {
                    ltrorderTranStatus.Text = "<b style=\"color:#ff0000;\">" + dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() + "</b>";
                    trrefund.Visible = false;
                    btnCancelOrder.Visible = false;
                }


                if (AppLogic.AppConfigs("IschargelogicAdmin") != null && AppLogic.AppConfigs("IschargelogicAdmin").ToString() != "" && Convert.ToBoolean(AppLogic.AppConfigs("IschargelogicAdmin")) && PaymentMethod.ToString() != "" && PaymentMethod.ToString().ToLower().IndexOf("paypal") <= -1)
                {
                    btnCapture.Visible = false;
                    btnRefund.Visible = false;
                    btnCancelOrder.Visible = false;
                }


                ltrProcessingStatus.Text = dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString();
                string salesagentname = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT FirstName+' '+LastName AS Name FROM dbo.tb_Admin WHERE AdminID IN( SELECT SalesAgentID FROM dbo.tb_Order WHERE OrderNumber='" + OrderNumber + "')"));
                if (salesagentname == "" || salesagentname == null)
                {
                    salesAgent.Text = "N/A";
                }
                else
                {
                    salesAgent.Text = salesagentname;
                }
                /**********Customer Contact Details****/
                ahrefname.InnerHtml = dsOrder.Tables[0].Rows[0]["FirstName"].ToString() + " " + dsOrder.Tables[0].Rows[0]["LastName"].ToString();
                ahrefname.HRef = "mailto:" + dsOrder.Tables[0].Rows[0]["Email"].ToString();
                ahrefMail.InnerHtml = dsOrder.Tables[0].Rows[0]["Email"].ToString();
                ahrefMail.HRef = "mailto:" + dsOrder.Tables[0].Rows[0]["Email"].ToString();
                hdnOrderEmail.Value = Convert.ToString(dsOrder.Tables[0].Rows[0]["Email"]);
                ltrShipping.Text = dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString() + " " + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()).ToString("C");
                if (dsOrder.Tables[0].Rows[0]["LastIPAddress"] != null && dsOrder.Tables[0].Rows[0]["LastIPAddress"].ToString() != "")
                {
                    ltrIP.Text = dsOrder.Tables[0].Rows[0]["LastIPAddress"].ToString();
                    IPAddressRow.Visible = true;
                }
                else
                    IPAddressRow.Visible = false;

                /********NOTES******************/
                ltrOrderNotes.Text = dsOrder.Tables[0].Rows[0]["OrderNotes"].ToString();
                ltrPrivate.Text = dsOrder.Tables[0].Rows[0]["Notes"].ToString();
                //if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["GiftPackNote"].ToString()))
                //{
                //    //ltrGiftnote.Text = dsOrder.Tables[0].Rows[0]["GiftPackNote"].ToString();
                //}
                //else
                //{
                //    ltrGiftnote.Text = "N/A";
                //}


                /*********Billing Address*******/

                ltrBillingAddress.Text = "";
                ltrBillingAddress.Text += dsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString() + " " + dsOrder.Tables[0].Rows[0]["BillingLastName"].ToString() + "<br />";
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString()))
                {
                    ltrBillingAddress.Text += dsOrder.Tables[0].Rows[0]["BillingCompany"].ToString() + "<br />";
                }

                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()))
                {
                    ltrBillingAddress.Text += dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                {
                    ltrBillingAddress.Text += dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                {
                    ltrBillingAddress.Text += dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString() + "<br />";
                }

                ltrBillingAddress.Text += dsOrder.Tables[0].Rows[0]["BillingCity"].ToString() + "," + dsOrder.Tables[0].Rows[0]["BillingState"].ToString() + " " + dsOrder.Tables[0].Rows[0]["BillingZip"].ToString() + "<br />";
                ltrBillingAddress.Text += dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString() + "<br />";
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                {
                    ltrBillingAddress.Text += dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["BillingEmail"].ToString()))
                {
                    ltrBillingAddress.Text += "Email: " + dsOrder.Tables[0].Rows[0]["BillingEmail"].ToString();
                }

                /******************Shipping Address************/

                ltrShippingAddress.Text = "";
                ltrShippingAddress.Text += dsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + dsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString() + "<br />";

                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                {
                    ltrShippingAddress.Text += dsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br />";
                }

                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                {
                    ltrShippingAddress.Text += dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                {
                    ltrShippingAddress.Text += dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                {
                    ltrShippingAddress.Text += dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br />";
                }

                ltrShippingAddress.Text += dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString() + "," + dsOrder.Tables[0].Rows[0]["ShippingState"].ToString() + " " + dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />";

                ltrShippingAddress.Text += dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString() + "<br />";

                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                {
                    ltrShippingAddress.Text += dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString() + "<br />";
                }
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingEmail"].ToString()))
                {
                    ltrShippingAddress.Text += "Email: " + dsOrder.Tables[0].Rows[0]["ShippingEmail"].ToString();
                }

                /***********payment Method ************/
                ltrPayment.Text = "Payment Method : " + dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString();
                ViewState["PaymentMethod"] = dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString();
                if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()) && (dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString().ToLower() == "creditcard" || dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString().ToLower() == "paypal"))
                {
                    ltrPayment.Text += "<br />Card Type : " + dsOrder.Tables[0].Rows[0]["CardType"].ToString();
                    ViewState["CardType"] = dsOrder.Tables[0].Rows[0]["CardType"].ToString();
                    ltrPayment.Text += "<br />Card Holder's Name : " + dsOrder.Tables[0].Rows[0]["CardName"].ToString();
                    ViewState["CardName"] = dsOrder.Tables[0].Rows[0]["CardName"].ToString();
                    string strNum = "";
                    if (ViewState["CardType"] != null && !string.IsNullOrEmpty(ViewState["CardType"].ToString()) && ViewState["CardType"].ToString().Trim().ToLower() == "american express")
                    {
                        ViewState["CardType"] = "AMEX";
                    }
                    else if (ViewState["CardType"] != null && !string.IsNullOrEmpty(ViewState["CardType"].ToString()) && ViewState["CardType"].ToString().Trim().ToLower() == "visa")
                    {
                        ViewState["CardType"] = "VISA";
                    }

                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString()))
                    {
                        string CardNumber = SecurityComponent.Decrypt(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString());

                        if (CardNumber.Length > 4)
                        {
                            for (int i = 0; i < CardNumber.Length - 4; i++)
                            {
                                strNum += "*";
                            }
                            strNum += CardNumber.ToString().Substring(CardNumber.Length - 4);
                            ViewState["CardNumber"] = strNum;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["last4"].ToString()))
                        {
                            strNum += "***********" + dsOrder.Tables[0].Rows[0]["last4"].ToString();

                        }
                    }
                    string strAVSResult = "";
                    strAVSResult = Convert.ToString(dsOrder.Tables[0].Rows[0]["AVSResult"]);
                    ltrPayment.Text += "<br />Card Number : " + strNum.ToString();
                    acreditcard.Visible = true;
                    if (strAVSResult.ToString() != "")
                    {
                        if (strAVSResult.ToString().Trim().ToLower() == "n" || strAVSResult.ToString().Trim().ToLower() == "a" || strAVSResult.ToString().Trim().ToLower() == "z" || strAVSResult.ToString().Trim().ToLower() == "w" || strAVSResult.ToString().Trim().ToLower() == "mismatch")
                        {
                            ltrPayment.Text += "<br />AVS Result : <label style='color:Red;'>" + strAVSResult + "</label> ";
                        }
                        else
                        {
                            ltrPayment.Text += "<br />AVS Result : " + strAVSResult.ToString();
                        }


                    }
                    if (dsOrder.Tables[0].Rows[0]["Transactionstatus"].ToString().ToLower().Trim() == "pending")
                    {
                        if (Request.QueryString["Id"].ToString() != null)
                        {
                            Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Count(TransactionID) FROM tb_FailedTransaction WHERE OrderNumber=" + Request.QueryString["Id"].ToString() + ""));
                            if (Count > 0) ;
                            {
                                lttransactionresult.Text = "<br />Transaction Result : CC Declined";
                            }
                        }
                    }

                    ViewState["CardMonth"] = dsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString();
                    ViewState["CardYear"] = dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString();
                    ViewState["CardVarificationCode"] = dsOrder.Tables[0].Rows[0]["CardVarificationCode"].ToString();

                    creditcarddetails.Visible = true;
                }
                else if (dsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString().ToLower() == "paypalexpress")
                {
                    //   ltrPayment.Text += "<br />Reference Detail : PAYPALEXPRESS";
                    creditcarddetails.Visible = false;
                }
                else
                {
                    ltrPayment.Text += "<br />Reference Detail : " + dsOrder.Tables[0].Rows[0]["Referrer"].ToString();
                    creditcarddetails.Visible = false;
                }

                //ltrPayment.Text += "<br />Order was placed using USD";

                /***************Product List*************/

                if (dsOrder != null && dsOrder.Tables.Count > 1 && dsOrder.Tables[1].Rows.Count > 0)
                {
                    ltrProductlist.Text = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" class=\"order-table\">";
                    ltrProductlist.Text += " <tbody>";
                    ltrProductlist.Text += "<tr>";
                    ltrProductlist.Text += " <th align=\"left\" width=\"79%\" valign=\"middle\">";
                    ltrProductlist.Text += "Products";
                    ltrProductlist.Text += "</th>";
                    ltrProductlist.Text += "<th align=\"right\" width=\"6%\" valign=\"middle\">";
                    ltrProductlist.Text += "Price";
                    ltrProductlist.Text += "</th>";
                    ltrProductlist.Text += "<th align=\"right\" width=\"8%\" valign=\"middle\">";
                    ltrProductlist.Text += "Qty";
                    ltrProductlist.Text += "</th>";
                    ltrProductlist.Text += "<th align=\"right\" width=\"7%\" valign=\"middle\" style=\"border-right: medium none;\">";
                    ltrProductlist.Text += "Sub Total";
                    ltrProductlist.Text += "</th>";
                    ltrProductlist.Text += "</tr>";
                    int row = 1;

                    for (int i = 0; i < dsOrder.Tables[1].Rows.Count; i++)
                    {
                        if (row % 2 == 0 && i != 0)
                        {
                            ltrProductlist.Text += "<tr class=\"oddrow\">";
                        }
                        else
                        {
                            ltrProductlist.Text += "<tr class=\"altrow\">";
                        }

                        ltrProductlist.Text += "<td align=\"left\" valign=\"top\">";
                        ltrProductlist.Text += "<a title=\"" + Server.HtmlEncode(dsOrder.Tables[1].Rows[i]["ProductName"].ToString()) + "\" href=\"/Admin/products/Product.aspx?StoreID=" + dsOrder.Tables[0].Rows[0]["storeId"].ToString() + "&ID=" + dsOrder.Tables[1].Rows[i]["RefProductId"].ToString() + "&Mode=edit\">" + dsOrder.Tables[1].Rows[i]["ProductName"].ToString() + "</a><br />";

                        string[] variantName = dsOrder.Tables[1].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantValue = dsOrder.Tables[1].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < variantValue.Length; j++)
                        {
                            if (variantName.Length > j)
                            {
                                ltrProductlist.Text += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                            }
                        }

                        ltrProductlist.Text += "SKU: " + dsOrder.Tables[1].Rows[i]["SKU"].ToString() + "";
                        ltrProductlist.Text += "</td>";

                        ltrProductlist.Text += "<td align=\"right\" valign=\"top\">";
                        ltrProductlist.Text += "<strong>" + Convert.ToDecimal(dsOrder.Tables[1].Rows[i]["price"].ToString()).ToString("C") + "</strong>";
                        ltrProductlist.Text += "</td>";

                        ltrProductlist.Text += "<td align=\"right\" valign=\"top\">";
                        ltrProductlist.Text += "Ordered " + dsOrder.Tables[1].Rows[i]["Quantity"].ToString() + "";
                        ltrProductlist.Text += "</td>";

                        ltrProductlist.Text += "<td align=\"right\" valign=\"top\" style=\"border-right: medium none;\">";
                        ltrProductlist.Text += Convert.ToDecimal(dsOrder.Tables[1].Rows[i]["productSubTotal"].ToString()).ToString("C");
                        ltrProductlist.Text += "</td>";
                        ltrProductlist.Text += "</tr>";
                        row++;
                    }

                    ltrProductlist.Text += "</tbody>";
                    ltrProductlist.Text += "</table>";
                }
                else
                {
                    ltrProductlist.Text = "Shopping Cart is Empty.";
                }
                ltrProductlist.Text = "";

                ltrorderTotals.Text = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" class=\"order-totals-table\" width='100%'>";
                ltrorderTotals.Text += "<tbody>";

                //ltrorderTotals.Text += "<tr>";
                //ltrorderTotals.Text += "<td align=\"right\"  valign=\"middle\">";
                //ltrorderTotals.Text += "Sub Total :";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "<td align=\"right\"   valign=\"middle\">";
                //ltrorderTotals.Text += "<span>" + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["orderSubTotal"].ToString()).ToString("C") + "</span>";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "</tr>";

                //ltrorderTotals.Text += "<tr>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //ltrorderTotals.Text += "Discount :";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()))
                //    ltrorderTotals.Text += "<span>" + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()).ToString("C") + "</span>";
                //else
                //    ltrorderTotals.Text += "<span>$0.00</span>";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "</tr>";

                //ltrorderTotals.Text += "<tr>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //ltrorderTotals.Text += "Customer Level Discount :";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()))
                //    ltrorderTotals.Text += "<span>" + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()).ToString("C") + "</span>";
                //else
                //    ltrorderTotals.Text += "<span>$0.00</span>";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "</tr>";

                //ltrorderTotals.Text += "<tr>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //ltrorderTotals.Text += "Quantity Discount :";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()))
                //    ltrorderTotals.Text += "<span>" + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()).ToString("C") + "</span>";
                //else
                //    ltrorderTotals.Text += "<span>$0.00</span>";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "</tr>";

                //ltrorderTotals.Text += "<tr>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //ltrorderTotals.Text += "Total Taxes :";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";

                //if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["OrderTax"].ToString()))
                //    ltrorderTotals.Text += "<span>" + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTax"].ToString()).ToString("C") + "</span>";
                //else
                //    ltrorderTotals.Text += "<span>$0.00</span>";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "</tr>";

                //ltrorderTotals.Text += "<tr>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //ltrorderTotals.Text += "Shipping &amp; Handling :";
                //ltrorderTotals.Text += " </td>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()))
                //    ltrorderTotals.Text += "<span>" + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()).ToString("C") + "</span>";
                //else
                //    ltrorderTotals.Text += "<span>$0.00</span>";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "</tr>";

                //ltrorderTotals.Text += "<tr>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //ltrorderTotals.Text += "<strong>Grand Total :</strong>";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                //if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["orderTotal"].ToString()))
                //    ltrorderTotals.Text += "<span>" + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["orderTotal"].ToString()).ToString("C") + "</span>";
                //else
                //    ltrorderTotals.Text += "<span>$0.00</span>";
                //ltrorderTotals.Text += "</td>";
                //ltrorderTotals.Text += "</tr>";


                ltrorderTotals.Text += "<tr>";
                ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\" width='80%'>";
                ltrorderTotals.Text += "<strong>Total Paid :</strong>";
                ltrorderTotals.Text += "</td>";
                ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                Decimal OrderAdust = 0;
                Decimal.TryParse(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"].ToString(), out OrderAdust);

                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString()) && (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() != "authorized" || dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() != "pending"))
                    ltrorderTotals.Text += "<span>" + Convert.ToDecimal(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["orderTotal"].ToString()) + OrderAdust).ToString("C") + "</span>";
                else
                    ltrorderTotals.Text += "<span>$0.00</span>";
                ltrorderTotals.Text += "</td>";
                ltrorderTotals.Text += "</tr>";

                ltrorderTotals.Text += "<tr>";
                ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                ltrorderTotals.Text += "<strong>Total Refunded :</strong>";
                ltrorderTotals.Text += "</td>";
                ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["RefundedAmount"].ToString()))
                    ltrorderTotals.Text += "<span>" + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["RefundedAmount"].ToString()).ToString("C") + "</span>";
                else
                    ltrorderTotals.Text += "<span>$0.00</span>";
                ltrorderTotals.Text += "</td>";
                ltrorderTotals.Text += "</tr>";

                ltrorderTotals.Text += "<tr>";
                ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";
                ltrorderTotals.Text += "<strong>Total Due :</strong>";
                ltrorderTotals.Text += "</td>";
                ltrorderTotals.Text += "<td align=\"right\" valign=\"middle\">";

                if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString()) && (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString().ToLower() != "pending"))
                {
                    ltrorderTotals.Text += "<span>$0.00</span>";
                }
                else
                {
                    Decimal Totaldue = Decimal.Zero;

                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["RefundedAmount"].ToString()))
                    {
                        Totaldue = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["ordertotal"].ToString());
                        Totaldue = Totaldue - Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["RefundedAmount"].ToString());
                        ltrorderTotals.Text += "<span>" + Convert.ToDecimal(Totaldue.ToString()).ToString("C") + "</span>";
                    }
                    else
                    {
                        Totaldue = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["ordertotal"].ToString());
                        ltrorderTotals.Text += "<span>" + Convert.ToDecimal(Totaldue.ToString()).ToString("C") + "</span>";
                    }
                }

                ltrorderTotals.Text += "</td>";
                ltrorderTotals.Text += "</tr>";
                ltrorderTotals.Text += "</tbody>";
                ltrorderTotals.Text += "</table>";

            }
            else
            {
                EmptyDetails();
            }



        }

        /// <summary>
        ///AllowIP Button Click Event
        /// </summary> 
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnaAllowIP_Click(object sender, ImageClickEventArgs e)
        {
            dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["Id"].ToString()));
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                OrderComponent objOrder = new OrderComponent();
                objOrder.DeleteIPAddressDetail(dsOrder.Tables[0].Rows[0]["LastIPAddress"].ToString());
                btnBlockIP.Visible = true;
                btnaAllowIP.Visible = false;
            }
        }

        /// <summary>
        /// BlockIP Button Click Event
        /// </summary>    
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnBlockIP_Click(object sender, ImageClickEventArgs e)
        {
            dsOrder = OrderComponent.GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["Id"].ToString()));
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                OrderComponent objOrder = new OrderComponent();
                tb_RestrictedIP tb_restricted = new tb_RestrictedIP();
                tb_restricted.IPAddress = dsOrder.Tables[0].Rows[0]["LastIPAddress"].ToString();
                int CustomerID = Convert.ToInt32(dsOrder.Tables[0].Rows[0]["CustomerID"].ToString());
                tb_restricted.tb_CustomerReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Customer", "CustomerID", CustomerID);
                tb_restricted.StoreID = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                objOrder.InsertIPAddressDetail(tb_restricted);
                btnBlockIP.Visible = false;
                btnaAllowIP.Visible = true;
            }
        }

        /// <summary>
        /// Fill Status Color
        /// </summary>
        private void FillStatusColor()
        {
            DataSet dsColor = new DataSet();
            dsColor = CommonComponent.GetCommonDataSet("SELECT StatusName,ColorName FROM tb_OrderStatusColor WHERE Isnull(Active,0)=1");
            if (dsColor != null && dsColor.Tables.Count > 0 && dsColor.Tables[0].Rows.Count > 0)
            {
                ddlStatus.DataSource = dsColor;
                ddlStatus.DataTextField = "StatusName";
                ddlStatus.DataValueField = "ColorName";
                ddlStatus.DataBind();
                //ddlColor.DataSource = dsColor;
                //ddlColor.DataTextField = "ColorName";
                //ddlColor.DataValueField = "ColorName";
                //ddlColor.DataBind();
            }
            else
            {
                ddlStatus.DataSource = null;

                ddlStatus.Items.Insert(0, new ListItem("None", "0"));
                ddlColor.Items.Insert(0, new ListItem("None", "0"));
            }

        }

        /// <summary>
        /// When No Data Fill up.
        /// </summary>
        private void EmptyDetails()
        {
            ltrOrderNumber.Text = "N/A";

            ltrOrderNotes.Text = "N/A";
            ltrOrderTotal.Text = "N/A";
            ltrorderTotals.Text = "N/A";
            ltrorderTranStatus.Text = "N/A";
            ltrPayment.Text = "N/A";
            ltrPrivate.Text = "N/A";
            //ltrProcessingStatus.Text = "N/A";
            ltrProductlist.Text = "N/A";
            ltrShipping.Text = "N/A";
            ltrShippingAddress.Text = "N/A";
            ltrIP.Text = "N/A";
            //ltrGiftnote.Text = "N/A";
            ltrDate.Text = "N/A";
            ltrCommentAll.Text = "N/A";
            ltrBillingAddress.Text = "N/A";
            ahrefname.InnerHtml = "N/A";
            ahrefMail.InnerHtml = "N/A";
            trComments.Visible = false;
        }

        /// <summary>
        ///  Share Comment Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShareComment_Click(object sender, EventArgs e)
        {
            int notify = 0;
            OrderComponent objCommnet = new OrderComponent();
            tb_OrderComment tb_OrderComment = new tb_OrderComment();
            tb_OrderComment.Comment = txtComment.Text.ToString();
            tb_OrderComment.Status = ddlStatus.SelectedItem.Text.ToString();
            if (chkNotify.Checked)
            {
                tb_OrderComment.NotifyCustomer = true;
                notify = 1;
            }
            else
            {
                tb_OrderComment.NotifyCustomer = false;
                notify = 0;
            }
            if (chkfrontend.Checked)
            {
                tb_OrderComment.IsVisibleFrontEnd = true;
            }
            else
            {
                tb_OrderComment.IsVisibleFrontEnd = false;
            }
            tb_OrderComment.OrderNumber = Convert.ToInt32(Request.QueryString["Id"].ToString());
            tb_OrderComment.CreatedBy = Convert.ToInt32(Session["AdminID"]);
            tb_OrderComment.CreatedOn = DateTime.Now;
            tb_OrderComment.ColorName = ddlColor.SelectedValue.ToString();
            //tb_Order ObjOrder = new tb_Order();
            //ObjOrder.OrderStatus = ddlStatus.SelectedValue.ToString();

            Int32 isadded = objCommnet.InsertOrderComments(tb_OrderComment); //Create State Line


            if (isadded > 0)
            {
                FillComment();
                if (notify == 1)
                {
                    DataSet ds = CommonComponent.GetCommonDataSet("select isnull(Firstname,'')+' '+isnull(Lastname,'')  as fname,isnull(Email,'') as Email,storeid from tb_order where ordernumber=" + Convert.ToInt32(Request.QueryString["Id"].ToString()) + "");
                    if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                    {
                        string toaddress = ds.Tables[0].Rows[0]["Email"].ToString();
                        string name = ds.Tables[0].Rows[0]["fname"].ToString();
                        int storeid = Convert.ToInt32(ds.Tables[0].Rows[0]["storeid"].ToString());
                        SendMail(toaddress, storeid, txtComment.Text.ToString(), Convert.ToInt32(Request.QueryString["Id"].ToString()), name);
                    }

                }
                //objCommnet.UpdateOrderStatus(Convert.ToInt32(Request.QueryString["Id"].ToString()), ddlStatus.SelectedItem.Text.ToString(), txtComment.Text.ToString());

                ltrOrderNotes.Text = txtComment.Text.ToString();
                txtComment.Text = "";
                // ltrProcessingStatus.Text = ddlStatus.SelectedItem.Text.ToString();
            }
            else
            {

            }



        }
        private void SendMail(string ToAddress, int StoreID, string Comments, Int32 OrderNumber, string Customername)
        {

            try
            {
                CustomerComponent objCustomer = null;
                objCustomer = new CustomerComponent();
                DataSet dsMailTemplate = new DataSet();
                //dsMailTemplate = objCustomer.GetEmailTamplate("AbandonedShoppingCartEmail", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                dsMailTemplate = objCustomer.GetEmailTamplate("CommentNotifyCustomer", StoreID);
                string strSTOREPATH = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(ConfigValue,'') FROM tb_Appconfig WHERE ConfigName='STOREPATH' and StoreId=" + StoreID + " and isnull(Deleted,0)=0"));
                string strSTORENAME = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(ConfigValue,'') FROM tb_Appconfig WHERE ConfigName='STORENAME' and StoreId=" + StoreID + " and isnull(Deleted,0)=0"));
                string strLIVE_SERVER = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(ConfigValue,'') FROM tb_Appconfig WHERE ConfigName='LIVE_SERVER' and StoreId=" + StoreID + " and isnull(Deleted,0)=0"));
                if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
                {
                    StringBuilder sw = new StringBuilder(2000);

                    sw.Append("<br/>");
                    sw.Append(Comments);

                    String strBody = "";
                    String strSubject = "";
                    strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                    strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();
                    strSubject = Regex.Replace(strSubject, "###STORENAME###", strSTORENAME.ToString(), RegexOptions.IgnoreCase);
                    strSubject = Regex.Replace(strSubject, "###ORDERNUMBER###", OrderNumber.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###ORDERNUMBER###", OrderNumber.ToString(), RegexOptions.IgnoreCase);
                    strBody = strBody.Replace("/images/Store_1.png", "http://www.halfpricedrapes.com/images/Store_" + StoreID.ToString() + ".png");
                    strBody = Regex.Replace(strBody, "###STOREPATH###", strSTOREPATH.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###STORENAME###", strSTORENAME.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", strLIVE_SERVER.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###Comment###", sw.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###StoreID###", StoreID.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###CustomerName###", Customername.ToString(), RegexOptions.IgnoreCase);
                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody, null, "text/html");


                    CommonOperations.SendMail(ToAddress, strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);


                }
            }
            catch { }
        }
        /// <summary>
        /// Fill Comment by Order Number
        /// </summary>
        private void FillComment()
        {
            OrderComponent objCommnet = new OrderComponent();
            List<tb_OrderComment> objCommnetList = objCommnet.GetOrderCommentsByID(Convert.ToInt32(Request.QueryString["id"].ToString()));
            ltrCommentAll.Text = "";
            if (objCommnetList.Count > 0)
            {

                for (int i = 0; i < objCommnetList.Count; i++)
                {

                    ltrCommentAll.Text += "<tr>";
                    ltrCommentAll.Text += "<td align=\"left\" valign=\"top\" colspan=\"3\">";
                    ltrCommentAll.Text += "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" style=\"border-top: 1px dashed rgb(182, 182, 182);\">";
                    ltrCommentAll.Text += "<tbody>";
                    ltrCommentAll.Text += "<tr>";
                    ltrCommentAll.Text += "<td align=\"left\" width=\"6%\" valign=\"top\"><img title=\"processing\" alt=\"processing\" src=\"/App_Themes/" + Page.Theme.ToString() + "/icon/processing-cion.png\"></td>";
                    ltrCommentAll.Text += "<td align=\"left\" width=\"94%\" valign=\"top\">" + string.Format("{0:MMMMMMMMMM dd,yyyy hh:mm:ss ttt}", Convert.ToDateTime(objCommnetList[i].CreatedOn)) + "  | <strong>" + objCommnetList[i].Status + "</strong><br />";
                    try
                    {

                        if (Convert.ToBoolean(objCommnetList[i].NotifyCustomer))
                        {
                            ltrCommentAll.Text += "Customer <span>Notified</span><br />";
                        }
                        else
                        {
                            ltrCommentAll.Text += "Customer <span>Not Notified</span><br />";
                        }
                        if (!string.IsNullOrEmpty(objCommnetList[i].ColorName.ToString()))
                        {
                            ltrCommentAll.Text += "<span style=\"color:" + objCommnetList[i].ColorName.ToString() + "\">" + objCommnetList[i].Comment.ToString() + "</span>";
                        }
                        else
                        {
                            ltrCommentAll.Text += objCommnetList[i].Comment.ToString();
                        }
                    }
                    catch
                    {

                    }
                    ltrCommentAll.Text += "</td>";
                    ltrCommentAll.Text += "</tr>";
                    ltrCommentAll.Text += "</tbody></table>";
                    ltrCommentAll.Text += "</td>";
                    ltrCommentAll.Text += "</tr>";
                }




                //string str = objCommnetList[0].Comment 
            }
            //<table cellspacing="0" cellpadding="0" border="0" width="100%" style="border-top: 1px dashed rgb(182, 182, 182);">
            //                                      <tbody><tr>
            //                                        <td align="left" width="6%" valign="top"><img title="processing" alt="processing" src="theme/gray/icon/processing-cion.png"></td>
            //                                        <td align="left" width="94%" valign="top">March 30, 2012 6:42:25 PM  | <strong>Processing</strong><br>
            //                                          Customer <span>Not Notified</span></td>
            //                                      </tr>
            //                                    </tbody></table>
        }

        /// <summary>
        ///  Send Email Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendEmail_Click(object sender, EventArgs e)
        {
            SendMail(Convert.ToInt32(Request.QueryString["id"].ToString()));
            OrderComponent objOrderlog = new OrderComponent();
            objOrderlog.InsertOrderlog(13, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
        }
        protected void btnExport_Click(object sender, EventArgs e)
        {

            GenerateCSVOrder();

            //DataSet DsOrder = CommonComponent.GetCommonDataSet("SElect * from tb_Order Where OrderNumber=" + Request.QueryString["id"].ToString() + "");
            //DataSet DsOrderedShippingCart = CommonComponent.GetCommonDataSet("Select * from tb_OrderedShoppingCartItems Where OrderedShoppingCartID in (Select ShoppingCardID from tb_Order Where OrderNumber=" + Request.QueryString["id"].ToString() + ")");
            ////Select * from tb_OrderedShoppingCart Where OrderedShoppingCartID in (Select ShoppingCardID from tb_Order Where OrderNumber=1104)
            //decimal totDiscountAmt = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select Sum(ISNULL(CustomDiscount,0)+ISNULL(GiftCertificateDiscountAmount,0)+ISNULL(CouponDiscountAmount,0)+ISNULL(QuantityDiscountAmount,0)+ISNULL(LevelDiscountAmount,0)) from tb_order Where OrderNumber=" + Request.QueryString["id"].ToString() + ""));

            //if (DsOrder != null && DsOrder.Tables.Count > 0 && DsOrder.Tables[0].Rows.Count > 0)
            //{
            //    if (Convert.ToInt32(ExecuteScalarQuery("Select COUNT(OrderNumber) as TotCnt from tborders Where OrderNumber = " + Request.QueryString["id"].ToString() + "")) > 0)
            //    {
            //        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@altmsg", "jAlert('Order already exported.','Message');", true);
            //        return;
            //    }
            //    else
            //    {

            //        string StrEmail = Convert.ToString(DsOrder.Tables[0].Rows[0]["email"].ToString());
            //        string StrCustomerName = Convert.ToString(DsOrder.Tables[0].Rows[0]["FirstName"].ToString() + " " + DsOrder.Tables[0].Rows[0]["LastName"].ToString());
            //        if (StrCustomerName.Length > 50)
            //            StrCustomerName = StrCustomerName.Substring(0, 49);


            //        StrCustomerName = StrCustomerName.Replace("'", "''");
            //        string StrBillingName = Convert.ToString(DsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString() + " " + DsOrder.Tables[0].Rows[0]["BillingLastName"].ToString());
            //        if (StrBillingName.Length > 50)
            //            StrBillingName = StrBillingName.Substring(0, 49);
            //        StrBillingName = StrBillingName.Replace("'", "''");

            //        string StrShippingName = Convert.ToString(DsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + DsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString());
            //        if (StrShippingName.Length > 50)
            //            StrShippingName = StrShippingName.Substring(0, 49);

            //        StrShippingName = StrShippingName.Replace("'", "''");

            //        string StrCardExpirationMonth = "";
            //        string StrCardExpirationYear = "";
            //        string CCExpDate = "";
            //        if (!string.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString()))
            //            StrCardExpirationMonth = DsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString();

            //        if (!string.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString()))
            //            StrCardExpirationYear = DsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString();
            //        if (StrCardExpirationYear.Length > 3)
            //            StrCardExpirationYear = StrCardExpirationYear.Substring(2, 2);
            //        CCExpDate = StrCardExpirationMonth.ToString() + "/" + StrCardExpirationYear.ToString();

            //        string strCardName = Convert.ToString(DsOrder.Tables[0].Rows[0]["CardName"].ToString());
            //        if (strCardName.Length > 0 && strCardName.Length > 41)
            //            strCardName = strCardName.Substring(0, 40);

            //        strCardName = strCardName.Replace("'", "''");

            //        string strBillingAddress1 = Convert.ToString(DsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString());
            //        if (strBillingAddress1.Length > 41)
            //            strBillingAddress1 = strBillingAddress1.Substring(0, 40);

            //        strBillingAddress1 = strBillingAddress1.Replace("'", "''");

            //        string strBillingAddress2 = Convert.ToString(DsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString());
            //        if (strBillingAddress2.Length > 41)
            //            strBillingAddress2 = strBillingAddress2.Substring(0, 40);

            //        strBillingAddress2 = strBillingAddress2.Replace("'", "''");


            //        string strShippingAddress1 = Convert.ToString(DsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString());
            //        if (strShippingAddress1.Length > 41)
            //            strShippingAddress1 = strShippingAddress1.Substring(0, 40);

            //        strShippingAddress1 = strShippingAddress1.Replace("'", "''");

            //        string strShippingAddress2 = Convert.ToString(DsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString());
            //        if (strShippingAddress2.Length > 41)
            //            strShippingAddress2 = strShippingAddress2.Substring(0, 40);

            //        strShippingAddress2 = strShippingAddress2.Replace("'", "''");

            //        string strBillingCompany = Convert.ToString(DsOrder.Tables[0].Rows[0]["BillingCompany"].ToString());
            //        if (strBillingCompany.Length > 41)
            //            strBillingCompany = strBillingCompany.Substring(0, 40);

            //        strBillingCompany = strBillingCompany.Replace("'", "''");


            //        string IsPrinted = "0";
            //        if (!string.IsNullOrEmpty(Convert.ToString(DsOrder.Tables[0].Rows[0]["IsPrinted"])) && Convert.ToBoolean(Convert.ToString(DsOrder.Tables[0].Rows[0]["IsPrinted"])))
            //            IsPrinted = "1";
            //        //string strMethodOfPayment = Convert.ToString(DsOrder.Tables[0].Rows[0]["PaymentMethod"]) + " " + Convert.ToString(DsOrder.Tables[0].Rows[0]["CardType"]);
            //        string strMethodOfPayment = "1";
            //        string StrCustomerId = Convert.ToString(DsOrder.Tables[0].Rows[0]["CustomerID"]);
            //        string StrGUIDLocation = Convert.ToString(ExecuteScalarQuery("SElect NewId() as GUIDLocation"));
            //        string StrGUIDTaxCode = Convert.ToString(ExecuteScalarQuery("SElect NewId() as GUIDTaxCode"));
            //        string StrShippingMethod = Convert.ToString(DsOrder.Tables[0].Rows[0]["ShippingMethod"]);
            //        if (StrShippingMethod.Length > 25)
            //            StrShippingMethod = StrShippingMethod.Substring(0, 24);
            //        string StrGUIDOrder = Convert.ToString(ExecuteScalarQuery("SElect NewId() as GUIDOrder"));
            //        bool ResultOrder = false;
            //        if (!string.IsNullOrEmpty(StrCustomerId))
            //        {
            //            bool Result = false;
            //            string StrGUIDCustomer = Convert.ToString(ExecuteScalarQuery("SElect  TOP 1 ISNULL(GUIDCustomer,'') from tbcustomer where Email='" + StrEmail + "'"));
            //            if (string.IsNullOrEmpty(StrGUIDCustomer.ToString()))
            //            {
            //                StrGUIDCustomer = Convert.ToString(ExecuteScalarQuery("SElect NewId() as GUIDCustomer"));
            //                // insert into tbcustomer  table in HPDAcctivate DB
            //                string StrCustomerQuery = "Insert into tbcustomer (GUIDCustomer,CustId,CompanyName,Name,FirstName,LastName,Address,Address2,City,State,Zip,Country, " +
            //                    " Phone,Email, GUIDSalesperson, GUIDTaxCode, TaxIncluded, " +
            //                    " GUIDTerms,CreditHold,Status,PopupNotes, CreatedBy,CreatedDate,UpdatedBy,UpdatedDate) " +
            //                    " Values('" + StrGUIDCustomer.ToString() + "','" + StrCustomerName.ToString() + "','" + strBillingCompany.ToString() + "' ,'" + StrCustomerName.ToString() + "','" + DsOrder.Tables[0].Rows[0]["FirstName"].ToString() + "','" + DsOrder.Tables[0].Rows[0]["LastName"].ToString() + "', " +
            //                    " '" + strBillingAddress1.ToString() + "','" + strBillingAddress2.ToString() + "','" + DsOrder.Tables[0].Rows[0]["BillingCity"].ToString().Replace("'", "''") + "','" + DsOrder.Tables[0].Rows[0]["BillingState"].ToString().Replace("'", "''") + "','" + DsOrder.Tables[0].Rows[0]["BillingZip"].ToString() + "','" + DsOrder.Tables[0].Rows[0]["BillingCountry"].ToString() + "','" + DsOrder.Tables[0].Rows[0]["BillingPhone"].ToString() + "','" + DsOrder.Tables[0].Rows[0]["Email"].ToString() + "', newid(),newid(),0,newid(),0, " +
            //                    " 1,0,'EKL','" + DsOrder.Tables[0].Rows[0]["OrderDate"].ToString() + "','EKL',getdate())";

            //                Result = Convert.ToBoolean(ExecuteNonQuery(StrCustomerQuery.ToString()));
            //            }
            //            else
            //            {
            //                Result = true;
            //            }


            //            if (Result)
            //            {
            //                // insert into tborder table

            //                string StrOrderQuery = "insert into tborders ( " +
            //                    " GUIDOrder,OrderNumber,Type,GUIDBranch,OrderDate,EntryDate,EnteredBy,OrderStatus,StatusDate,StatusChangedBy, " +
            //                    " Completed,Printed,ReadyToPrint,PickTicketPrinted,PickTicketReadyToPrint,ShippingDocumentPrinted,ShippingDocumentReadyToPrint, " +
            //                     " ShipVia,RequestedShipDate,QuotedDaysToShip,ManualHold,GUIDCustomer, " +
            //                    " SoldToOverride,SoldToName,SoldToAddress1,SoldToAddress2,SoldToCity,SoldToState,SoldToZip,SoldToCountry,GUIDLocation, " +
            //                    " ShipToOverride,ShipToAttn,ShipToAddress1,ShipToCity,ShipToState,ShipToZip,ShipToCountry, " +
            //                    " Taxable,GUIDTaxCode,TaxPct,FrtTaxPct,TaxPercentText,SchedSubTotal,SchedDiscountAmount,SchedSalesTax,SchedShippingCharge,SchedTotalAmount,SubTotal, " +
            //                    " DiscountType,InvoiceDiscountPct,DiscountAmount,SalesTax,TotalAmount,Reference,GUIDTerms,TermsDescription,SchedTermsDiscountAvailable,DeliveryMiles,Reference2, " +
            //                    " ContactName,ContactPhoneNumber,ContactEMailAddress,AmtPaid,CCExpDate,CCName,CCPostalCode,DiscAmt, " +
            //                    " MethodOfPayment,ReadyToInvoice,InvoicingError,Carrier,CarrierService,NextShipmentNumber, " +
            //                    " GUIDSalesperson,GUIDOrderWorkFlowStatus,TaxIncluded,OriginType,OriginID,GUIDTemplate, " +
            //                    " WebOrderNumber,UpdatedBy,UpdatedDate,ExchangeRate,WebOrderID) " +

            //                    " Values( '" + StrGUIDOrder.ToString() + "'," + Request.QueryString["id"].ToString() + ",'O',NewId(), '" + DsOrder.Tables[0].Rows[0]["OrderDate"].ToString() + "', '" + DsOrder.Tables[0].Rows[0]["OrderDate"].ToString() + "','EKL','S',GETDATE(),'EKL', " +
            //                    " 0," + IsPrinted.ToString() + ",0,0,0,0,0,  " +
            //                    " '" + DsOrder.Tables[0].Rows[0]["ShippedVIA"].ToString() + "','" + DsOrder.Tables[0].Rows[0]["ShippedOn"].ToString() + "',7,0,'" + StrGUIDCustomer.ToString() + "',  " +
            //                    " 0,'" + StrBillingName.ToString() + "','" + strBillingAddress1 + "','" + strBillingAddress2.ToString() + "','" + DsOrder.Tables[0].Rows[0]["BillingCity"].ToString().Replace("'", "''") + "','" + DsOrder.Tables[0].Rows[0]["BillingState"].ToString().Replace("'", "''") + "','" + DsOrder.Tables[0].Rows[0]["BillingZip"].ToString() + "','" + DsOrder.Tables[0].Rows[0]["BillingCountry"].ToString() + "','" + StrGUIDLocation.ToString() + "', " +
            //                    " 0,'" + StrShippingName.ToString() + "','" + strShippingAddress1.ToString() + "','" + DsOrder.Tables[0].Rows[0]["ShippingCity"].ToString().Replace("'", "''") + "','" + DsOrder.Tables[0].Rows[0]["ShippingState"].ToString().Replace("'", "''") + "','" + DsOrder.Tables[0].Rows[0]["ShippingZip"].ToString() + "','" + DsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString() + "', " +

            //                    " 1,'" + StrGUIDTaxCode.ToString() + "',0.0,0.0,'0.00',0,0,0,0,0," + DsOrder.Tables[0].Rows[0]["OrderSubtotal"].ToString() + ", " +
            //                    " '$',0, " + totDiscountAmt + ",0," + DsOrder.Tables[0].Rows[0]["OrderTotal"].ToString() + ",'999777',NewId(),'Due on receipt',0,0,'999000',  " +

            //                    " '" + StrBillingName.ToString() + "','" + DsOrder.Tables[0].Rows[0]["BillingPhone"].ToString() + "','" + DsOrder.Tables[0].Rows[0]["Email"].ToString() + "'," + DsOrder.Tables[0].Rows[0]["OrderTotal"].ToString() + ",'" + CCExpDate + "','" + strCardName.ToString() + "','" + DsOrder.Tables[0].Rows[0]["ShippingZip"].ToString() + "'," + DsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString() + ", " +
            //                    "" + strMethodOfPayment.ToString() + " ,0,0,'" + DsOrder.Tables[0].Rows[0]["ShippedVIA"].ToString() + "','" + StrShippingMethod.ToString() + "',1,  " +
            //                    " NewId(),NewId() ,0,'I','custom_test',NEWID(), " +
            //                    " '999777','EKL',getdate(),1,'999000') ";
            //                ResultOrder = Convert.ToBoolean(ExecuteNonQuery(StrOrderQuery.ToString()));
            //            }

            //            if (ResultOrder)
            //            {
            //                // Insert into tbOrderDetail
            //                if (DsOrderedShippingCart != null && DsOrderedShippingCart.Tables.Count > 0 && DsOrderedShippingCart.Tables[0].Rows.Count > 0)
            //                {
            //                    decimal TotInvoiceDiscount = 0, TotQty = 0;
            //                    for (int i = 0; i < DsOrderedShippingCart.Tables[0].Rows.Count; i++)
            //                    {
            //                        decimal DiscountPri = 0;
            //                        decimal.TryParse(DsOrderedShippingCart.Tables[0].Rows[i]["DiscountPrice"].ToString(), out DiscountPri);
            //                        decimal Price = 0;
            //                        decimal.TryParse(DsOrderedShippingCart.Tables[0].Rows[i]["Price"].ToString(), out Price);
            //                        Int32 Qty = 0;
            //                        int.TryParse(DsOrderedShippingCart.Tables[0].Rows[i]["Quantity"].ToString(), out Qty);
            //                        Int32 ShippedQty = 0;
            //                        int.TryParse(DsOrderedShippingCart.Tables[0].Rows[i]["ShippedQty"].ToString(), out ShippedQty);

            //                        decimal TotAmt = Convert.ToDecimal(Price * Qty);
            //                        string strProductName = Convert.ToString(DsOrderedShippingCart.Tables[0].Rows[i]["ProductName"]) + " " + Convert.ToString(DsOrderedShippingCart.Tables[0].Rows[i]["VariantValues"]);
            //                        strProductName = strProductName.Replace("'", "''");
            //                        decimal InvoiceDiscount = 0;
            //                        if (Price > 0 && DiscountPri > 0)
            //                        {
            //                            InvoiceDiscount = (Convert.ToDecimal(Price - DiscountPri) * Convert.ToDecimal(Qty));
            //                            if (InvoiceDiscount <= 0)
            //                                InvoiceDiscount = 0;
            //                        }

            //                        TotInvoiceDiscount += InvoiceDiscount;
            //                        TotQty += Qty;

            //                        string StrGUIDOrderDetail = Convert.ToString(ExecuteScalarQuery("SElect NewId() as GUIDOrderDetail"));
            //                        string StrGUIDProduct = Convert.ToString(ExecuteScalarQuery("SElect NewId() as GUIDProduct"));

            //                        string StrOrderDetailsQuery = " Insert into tbOrderDetail ( " +
            //                                                    " GUIDOrderDetail,GUIDOrder,LineNumber,SubLineNumber,ComponentLevel,LineType," +
            //                                                    " GUIDProduct,ProductID,GUIDWarehouse,Description," +
            //                                                    " QtyOrdered,QtyShipped,QtyPicked,QtyScheduled,QtyInvoiced,QtyBackordered,Completed,LineCancelled,Unit,DisplayUnit," +
            //                                                    " DisplayUnitFactor,PriceCode,Price,DisplayPrice,LineTaxPrice,PriceUnit,PriceUnitFactor,PriceUnitFactorType," +
            //                                                    " Freight,ProductTaxPct,LineDiscountPct,Amount,DisplayAmount,InvoiceDiscountAmount,LineTaxAmount," +
            //                                                    " SchedAmount,SchedInvoiceDiscountAmount,SchedLineTaxAmount,Discountable,Taxable," +
            //                                                    " GUIDTaxCode,GUIDProductClass,Length,Weight,VariableLength,VariableWeight,SpecialInstructions,InventoryControlType," +
            //                                                    " QtyLotSerial,CreatePO,Reference,ToBeBilled,Exported940)" +
            //                                                    " values" +
            //                                                    " ('" + StrGUIDOrderDetail.ToString() + "','" + StrGUIDOrder.ToString() + "',2,0,0,'P'," +
            //                                                    " '" + StrGUIDProduct.ToString() + "'," + DsOrderedShippingCart.Tables[0].Rows[i]["RefProductID"].ToString() + ",NEWID(),'" + strProductName.ToString() + "'," +
            //                                                    " " + Qty + "," + ShippedQty + ",0," + Qty + "," + Qty + ",0,0,0,'Ea','Ea'," +
            //                                                    " 1,'-'," + Price + "," + Price + ",0,'Ea',1,'S'," +
            //                                                    " 0,0,0," + TotAmt + "," + TotAmt + "," + InvoiceDiscount + ",0," +
            //                                                    " " + TotAmt + "," + DiscountPri + ",0,0,1," +
            //                                                    " '" + StrGUIDTaxCode.ToString() + "',NEWID(),0,0,0,0,'Test Order','S'," +
            //                                                    " 0,0,'999777',0,0)";

            //                        bool ResultOrderDetails = Convert.ToBoolean(ExecuteNonQuery(StrOrderDetailsQuery.ToString()));
            //                    }

            //                    decimal OrderShippingCosts = 0;
            //                    decimal.TryParse(DsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString(), out OrderShippingCosts);
            //                    if (OrderShippingCosts > 0)
            //                    {

            //                        string StrOrderDetailsShippingQuery = " insert into tbOrderDetail ( " +
            //                                                                " GUIDOrderDetail,GUIDOrder,LineNumber,SubLineNumber,ComponentLevel,LineType, " +
            //                                                                " GUIDProduct,ProductID,GUIDWarehouse,Description, " +
            //                                                                " QtyOrdered,QtyShipped,QtyPicked,QtyScheduled,QtyInvoiced,QtyBackordered,Completed,LineCancelled,Unit,DisplayUnit, " +
            //                                                                " DisplayUnitFactor,PriceCode,Price,DisplayPrice,LineTaxPrice,PriceUnit,PriceUnitFactor,PriceUnitFactorType, " +
            //                                                                " Freight,ProductTaxPct,LineDiscountPct,Amount,DisplayAmount,InvoiceDiscountAmount,LineTaxAmount, " +
            //                                                                " SchedAmount,SchedInvoiceDiscountAmount,SchedLineTaxAmount,Discountable,Taxable, " +
            //                                                                " GUIDTaxCode,GUIDProductClass,Length,Weight,VariableLength,VariableWeight,SpecialInstructions,InventoryControlType, " +
            //                                                                " QtyLotSerial,CreatePO,Reference,ToBeBilled,Exported940) " +
            //                                                                " values( " +
            //                                                                " NewId(),'" + StrGUIDOrder.ToString() + "',1,0,0,'C', " +
            //                                                                " NEWID(),'Shipping & Handling',NULL,'Shipping & Handling', " +
            //                                                                " 1,0,0,1,1,0,0,0,'Ea','Ea', " +
            //                                                                " 1,'-'," + OrderShippingCosts + "," + OrderShippingCosts + ",0,'Ea',1,'S', " +
            //                                                                " 0,0,0," + OrderShippingCosts + "," + OrderShippingCosts + "," + TotInvoiceDiscount + ",0, " +
            //                                                                " " + OrderShippingCosts + "," + TotInvoiceDiscount + ",0,0,1, " +
            //                                                                " '" + StrGUIDTaxCode.ToString() + "',NEWID(),0,0,0,0,'','S', " +
            //                                                                " 0,0,'999777',0,0 ) ";

            //                        bool ResultOrderDetails = Convert.ToBoolean(ExecuteNonQuery(StrOrderDetailsShippingQuery.ToString())); // " " + TotQty + ",0,0," + TotQty + "," + TotQty + ",0,0,0,'Ea','Ea', " +
            //                    }
            //                }
            //            }
            //        }
            //        if (ResultOrder)
            //        {
            //            Page.ClientScript.RegisterStartupScript(Page.GetType(), "@altmsg", "jAlert('Your order exported Successfully.','Message');", true);
            //        }
            //    }
            //}
        }



        /// <summary>
        ///  Queries for New Database
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>

        public bool ExecuteNonQuery(string Query)
        {
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["HPDAcctivate_DBEntities"]);
            SqlCommand cmd;
            bool result = false;
            cmd = new SqlCommand(Query, conn);
            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                int index = cmd.ExecuteNonQuery();
                if (index != 1)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }

            catch (Exception ex)
            {
                result = false;
                HttpContext context = HttpContext.Current;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@alterror", "jAlert('" + ex.Message.ToString() + "','Error');", true);
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
                cmd.Dispose();
            }
            return result;
        }

        public Object ExecuteScalarQuery(string Query)
        {
            SqlCommand cmd = new SqlCommand();
            Object Obj = new Object();
            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = Query;
                Obj = ExecuteScalarQuery(cmd);
            }
            catch (Exception ex)
            {
                Obj = null;
                HttpContext context = HttpContext.Current;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@alterror", "jAlert('" + ex.Message.ToString() + "','Error');", true);
            }
            finally
            {
                cmd.Dispose();
            }
            return Obj;
        }

        public Object ExecuteScalarQuery(SqlCommand cmd)
        {
            SqlConnection connnew = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["HPDAcctivate_DBEntities"]);
            Object Obj = new Object();
            try
            {
                cmd.Connection = connnew;
                if (connnew.State == ConnectionState.Closed)
                    connnew.Open();
                Obj = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Obj = null;
                HttpContext context = HttpContext.Current;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@alterror", "jAlert('" + ex.Message.ToString() + "','Error');", true);

            }
            finally
            {
                if (connnew != null)
                    if (connnew.State == ConnectionState.Open) connnew.Close();
                cmd.Dispose();
            }
            return Obj;
        }

        protected void btnExportproduct_Click(object sender, EventArgs e)
        {
            GenerateCSVProduct();

        }
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            sFieldValueToEscape = sFieldValueToEscape.Replace("\\r\\n", System.Environment.NewLine);
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
            }
        }
        private void GenerateCSVOrder()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=OrderDetail_Export.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder sb = new StringBuilder();

            DataSet dsorder = new DataSet();
            SecurityComponent objsec = new SecurityComponent();
            bool falg = false;

            dsorder = CommonComponent.GetCommonDataSet("EXEC usp_OrderDetailAcctivate " + Request.QueryString["id"].ToString() + "");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {
                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }

            }
            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {

                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower().Trim() == "credit card number")
                        {
                            if (string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString().Trim()))
                            {
                                args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                            }
                            else
                            {
                                args[c] = _EscapeCsvField(SecurityComponent.Decrypt(dsorder.Tables[0].Rows[i][c].ToString().Trim()));
                            }

                        }
                        else
                        {
                            args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                        }
                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
                CommonComponent.ExecuteCommonData("Update tb_order set ExportedDate=GETDATE() Where OrderNumber=" + Convert.ToInt32(Request.QueryString["id"].ToString()) + "");
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();

        }
        private void GenerateCSVProduct()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition",
             "attachment;filename=ProductDetail_Export.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder sb = new StringBuilder();

            DataSet dsorder = new DataSet();

            bool falg = false;

            dsorder = CommonComponent.GetCommonDataSet("EXEC usp_OrderproductDetailAcctivate " + Request.QueryString["id"].ToString() + "");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {
                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }

            }
            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {

                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }

                CommonComponent.ExecuteCommonData("Update tb_order set ExportedDate=GETDATE() Where OrderNumber=" + Convert.ToInt32(Request.QueryString["id"].ToString()) + "");
            }

            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
        }
        /// <summary>
        /// Order Receipt Send To Customer by Email 
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNUmber</param>
        public void SendMail(Int32 OrderNumber)
        {


            string Body = "";
            string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + "/Admin/Orders/Invoicemail.aspx?ONo=" + OrderNumber.ToString();
            WebRequest NewWebReq = WebRequest.Create(url);
            WebResponse newWebRes = NewWebReq.GetResponse();
            string format = newWebRes.ContentType;
            Stream ftprespstrm = newWebRes.GetResponseStream();
            StreamReader reader;
            reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
            Body = reader.ReadToEnd().ToString();
            Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"display:none;\"");
            AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
            try
            {

                Solution.Bussines.Components.AdminCommon.CommonOperations.SendMail(ahrefMail.InnerHtml.ToString().Trim(), "Receipt for Order #" + OrderNumber.ToString() + " from " + Solution.Bussines.Components.AdminCommon.AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Mail has been sent successfully.');Tabdisplay(" + hdnTabid.Value.ToString() + ");", true);
            }
            catch { Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Mail Sending Problem.');Tabdisplay(" + hdnTabid.Value.ToString() + ");", true); }

        }

        /// <summary>
        ///  Capture Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCapture_Click(object sender, EventArgs e)
        {
            string strResult = "OK";
            tb_Order objOrder = new tb_Order();
            objOrder.CaptureTxCommand = "";
            objOrder.CaptureTXResult = "";
            objOrder.AuthorizationPNREF = "";
            tb_Order objorderCapture = new tb_Order();
            DataSet dsAutho = new DataSet();
            dsAutho = CommonComponent.GetCommonDataSet("SELECT isnull(AuthorizationPNREF,'') as AuthorizationPNREF,isnull(PaymentGateway,'') as PaymentGateway,isnull(OrderTotal,0) as OrderTotal,isnull(TransactionStatus,'') as TransactionStatus FROM tb_order where OrderNumber=" + Request.QueryString["id"].ToString() + "");
            string PaymentGateway = "";
            string TransactionStatus = "";
            if (dsAutho != null && dsAutho.Tables.Count > 0 && dsAutho.Tables[0].Rows.Count > 0)
            {
                objOrder.AuthorizationPNREF = dsAutho.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                PaymentGateway = dsAutho.Tables[0].Rows[0]["PaymentGateway"].ToString();
                objOrder.OrderTotal = Convert.ToDecimal(dsAutho.Tables[0].Rows[0]["OrderTotal"].ToString());
                objOrder.OrderNumber = Convert.ToInt32(Request.QueryString["id"].ToString());
                TransactionStatus = dsAutho.Tables[0].Rows[0]["TransactionStatus"].ToString();
            }
            if (PaymentGateway.ToString().ToLower().Trim() == "paypal")
            {
                PayPalComponent objPay = new PayPalComponent();
                strResult = objPay.CaptureOrder(objOrder);
            }
            if (PaymentGateway.ToString().ToLower().Trim() == "paypalexpress")
            {
                PayPalComponent objPay = new PayPalComponent();
                strResult = objPay.CaptureOrder(objOrder);
            }
            else if (PaymentGateway.ToString().ToLower().Trim() == "authorizenet")
            {
                if (TransactionStatus.ToLower().Trim() == "pending")
                {

                    OrderComponent objCapture = new OrderComponent();
                    objorderCapture = objCapture.GetOrderByOrderNumber(Convert.ToInt32(Request.QueryString["id"].ToString()));
                    String AVSResult = String.Empty;
                    String AuthorizationResult = String.Empty;
                    String AuthorizationCode = String.Empty;
                    String AuthorizationTransID = String.Empty;
                    String TransactionCommand = String.Empty;
                    String TransactionResponse = String.Empty;

                    DataSet dsPayment = new DataSet();
                    dsPayment = objCapture.GetPaymentGateway(objorderCapture.PaymentMethod.ToString(), Convert.ToInt32(objorderCapture.tb_StoreReference.EntityKey.EntityKeyValues[0].Value.ToString()));
                    string PaymentGatewayStatus = "";
                    if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                    {
                        PaymentGatewayStatus = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString());
                    }
                    strResult = AuthorizeNetComponent.ProcessCardForYahooorderAdmin(Convert.ToInt32(Request.QueryString["id"].ToString()), Convert.ToInt32(objorderCapture.CustomerID), Convert.ToDecimal(objorderCapture.OrderTotal.ToString()), AppLogic.AppConfigBool("UseLiveTransactions"), PaymentGatewayStatus.ToString(), objorderCapture, objorderCapture, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    //   Response.Write(strResult.ToString());
                    objorderCapture.AuthorizationCode = AuthorizationCode;
                    objorderCapture.AuthorizationPNREF = AuthorizationTransID;
                    objorderCapture.AuthorizationResult = AuthorizationResult;
                    objorderCapture.TransactionCommand = TransactionCommand;
                    objorderCapture.AVSResult = AVSResult;

                }
                else
                {
                    OrderComponent objCapture = new OrderComponent();
                    objorderCapture = objCapture.GetOrderByOrderNumber(Convert.ToInt32(Request.QueryString["id"].ToString()));
                    strResult = AuthorizeNetComponent.CaptureOrderFirst(objOrder);

                    Decimal AuthorizedAmount1 = Convert.ToDecimal(objorderCapture.AuthorizedAmount.ToString());
                    Decimal AuthorizedAmount = Convert.ToDecimal(objorderCapture.OrderTotal) - AuthorizedAmount1;
                    if (Convert.ToDecimal(AuthorizedAmount) > Convert.ToDecimal(0) && Convert.ToDecimal(AuthorizedAmount1) > Convert.ToDecimal(0) && strResult == "OK")
                    {
                        String AVSResult = String.Empty;
                        String AuthorizationResult = String.Empty;
                        String AuthorizationCode = String.Empty;
                        String AuthorizationTransID = String.Empty;
                        String TransactionCommand = String.Empty;
                        String TransactionResponse = String.Empty;
                        string strResult1 = AuthorizeNetComponent.ProcessCardForYahooorderAdmin(Convert.ToInt32(Request.QueryString["id"].ToString()), Convert.ToInt32(objorderCapture.CustomerID), Convert.ToDecimal(AuthorizedAmount.ToString()), AppLogic.AppConfigBool("UseLiveTransactions"), "auth_capture", objorderCapture, objorderCapture, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
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
                if (TransactionStatus.ToLower().Trim() == "pending")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_order SET AuthorizedAmount='" + objorderCapture.OrderTotal.ToString() + "', AVSResult='" + objorderCapture.AVSResult.ToString().Replace("'", "''") + "',AuthorizationResult='" + objorderCapture.AuthorizationResult.ToString().Replace("'", "''") + "',AuthorizationCode='" + objorderCapture.AuthorizationCode.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + objorderCapture.AuthorizationPNREF.ToString().Replace("'", "''") + "',TransactionCommand='" + objorderCapture.TransactionCommand.ToString().Replace("'", "''") + "',AuthorizedOn=dateadd(hour,-2,getdate()), TransactionStatus='AUTHORIZED' WHERE orderNumber=" + Request.QueryString["id"].ToString() + "");
                }
                else
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_order SET CaptureTXCommand='" + objOrder.CaptureTxCommand.ToString().Replace("'", "''") + "',CaptureTXResult='" + objOrder.CaptureTXResult.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + objOrder.AuthorizationPNREF.ToString().Replace("'", "''") + "', CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='CAPTURED' WHERE orderNumber=" + Request.QueryString["id"].ToString() + "");
                }

                #region Commented Code by Girish - Do not Delete
                //try
                //{
                //    CommonComponent.ExecuteCommonData("UPDATE dbo.tb_GiftCard SET IsActive=1 WHERE OrderNumber=" + Request.QueryString["id"].ToString() + "");
                //}
                //catch { }

                #endregion
                CommonComponent.ExecuteCommonData("EXEC usp_Product_AdjustInventory " + Request.QueryString["id"].ToString() + ",-1, 0");

                if (TransactionStatus.ToLower().Trim() == "pending")
                {
                    lttransactionresult.Text = "";
                    ltrorderTranStatus.Text = "<b style=\"color:#348934;\">AUTHORIZED</b>";
                    OrderComponent objOrderlog = new OrderComponent();
                    objOrderlog.InsertOrderlog(1, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Your Transaction State has been AUTHORIZED.','Success');", true);

                }
                else
                {
                    ltrorderTranStatus.Text = "<b style=\"color:#348934;\">CAPTURED</b>";
                    btnCapture.Visible = false;
                    btnRefund.Visible = true;
                    trrefund.Visible = false;
                    trvoid.Visible = false;
                    OrderComponent objOrderlog = new OrderComponent();
                    objOrderlog.InsertOrderlog(2, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Your Transaction State has been CAPTURED.','Success');", true);
                }


            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "','Failed');", true);
            }
        }

        /// <summary>
        ///  Refund Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnRefund_Click(object sender, EventArgs e)
        {
            CommonComponent.ExecuteCommonData("UPDATE tb_order SET TransactionStatus='REFUNDED' WHERE orderNumber=" + Request.QueryString["id"] + "");
            ltrorderTranStatus.Text = "<b style=\"color:#00AAFF;\">REFUNDED</b>";
            btnRefund.Visible = false;
            trcapture.Visible = false;

        }

        /// <summary>
        ///  Force Refund Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnForceRefund_Click(object sender, EventArgs e)
        {
            CommonComponent.ExecuteCommonData("UPDATE  tb_order SET TransactionStatus='FORCE REFUND' WHERE orderNumber=" + Request.QueryString["id"] + "");
            ltrorderTranStatus.Text = "FORCE REFUND";
            OrderComponent objOrderlog = new OrderComponent();
            objOrderlog.InsertOrderlog(6, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));
            btnForceRefund.Visible = false;
            trrefund.Visible = false;
        }

        /// <summary>
        ///  Cancel Order Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            string strResult = "OK";
            DataSet dsAutho = new DataSet();
            dsAutho = CommonComponent.GetCommonDataSet("SELECT isnull(AuthorizationPNREF,'') as AuthorizationPNREF,isnull(PaymentGateway,'') as PaymentGateway,isnull(OrderTotal,0) as OrderTotal,isnull(TransactionStatus,'') as TransactionStatus,storeId FROM tb_order where OrderNumber=" + Request.QueryString["id"].ToString() + "");
            string PaymentGAteWay = "";
            string TransactionStatus = "";
            string AuthorizationPNREF = "";
            Int32 ssid = 0;
            if (dsAutho != null && dsAutho.Tables.Count > 0 && dsAutho.Tables[0].Rows.Count > 0)
            {
                AuthorizationPNREF = dsAutho.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                PaymentGAteWay = dsAutho.Tables[0].Rows[0]["PaymentGateway"].ToString();
                TransactionStatus = dsAutho.Tables[0].Rows[0]["TransactionStatus"].ToString();
                ssid = Convert.ToInt32(dsAutho.Tables[0].Rows[0]["storeId"].ToString());
            }
            /**********PayPal***************/
            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypal")
            {
                if (TransactionStatus.ToLower() == "authorized")
                {
                    PayPalComponent objPay = new PayPalComponent();
                    strResult = objPay.VoidOrder(Convert.ToInt32(Request.QueryString["id"].ToString()), AuthorizationPNREF.ToString());
                }
            }


            if (PaymentGAteWay.ToString().ToLower().Trim() == "paypalexpress")
            {
                if (TransactionStatus.ToLower() == "authorized")
                {
                    PayPalComponent objPay = new PayPalComponent();
                    strResult = objPay.VoidOrder(Convert.ToInt32(Request.QueryString["id"].ToString()), AuthorizationPNREF.ToString());
                }
            }
            else if (PaymentGAteWay.ToString().ToLower().Trim() == "authorizenet")
            {
                if (TransactionStatus.ToLower() == "authorized")
                {
                    strResult = AuthorizeNetComponent.VoidOrder(Convert.ToInt32(Request.QueryString["id"].ToString()), AuthorizationPNREF.ToString());
                }
            }
            if (strResult == "OK")
            {
                OrderComponent objOrderlog = new OrderComponent();
                objOrderlog.InsertOrderlog(21, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));

                CommonComponent.ExecuteCommonData("UPDATE  tb_order SET TransactionStatus='CANCELED',orderStatus='CANCELED' WHERE orderNumber=" + Request.QueryString["id"] + "");
                OrderComponent objOrder = new OrderComponent();
                objOrder.UpdateInventoryByOrderNumber(Convert.ToInt32(Request.QueryString["id"].ToString()), 1);
                try
                {
                    objOrderlog.RefundGiftCardAmount(Convert.ToInt32(Request.QueryString["id"].ToString()));
                }
                catch { }

                ltrorderTranStatus.Text = "<b style=\"color:#ff0000;\">CANCELED</b>";
                btnCancelOrder.Visible = false;
                ltrProcessingStatus.Text = "CANCELED";
                trvoid.Visible = false;
                trcapture.Visible = false;
                trrefund.Visible = false;
                if (ssid != 11)
                {
                    SendOrderCancelMail(Request.QueryString["id"].ToString());
                }

            }
            else
            {
                if (strResult != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('" + strResult.ToString() + "','Failed');", true);
                }

            }
        }

        /// <summary>
        ///  Void Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnVoid_Click(object sender, EventArgs e)
        {
            CommonComponent.ExecuteCommonData("UPDATE  tb_order SET TransactionStatus='CANCELED' WHERE orderNumber=" + Request.QueryString["id"] + "");
            ltrorderTranStatus.Text = "<b style=\"color:#1A1AC4;\">VOIDED</b>";
            btnVoid.Visible = false;
            trvoid.Visible = false;
            trcapture.Visible = false;
            trrefund.Visible = false;
            OrderComponent objOrder = new OrderComponent();
            objOrder.UpdateInventoryByOrderNumber(Convert.ToInt32(Request.QueryString["id"].ToString()), 1);
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtNameOnCard.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Name of Card.','Required Information','ContentPlaceHolder1_txtNameOnCard');", true);
                txtNameOnCard.Focus();
                return;
            }
            else if (ddlCardType.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Card Type.','Required Information','ContentPlaceHolder1_ddlCardType');", true);
                ddlCardType.Focus();
                return;
            }
            else if (txtCardNumber.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Card Number.','Required Information','ContentPlaceHolder1_txtCardNumber');", true);
                txtCardNumber.Focus();
                return;
            }
            //else if (txtCSC.Text.ToString().Trim() == "")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Card Verification Code.','Required Information','ContentPlaceHolder1_txtCSC');", true);
            //    txtCSC.Focus();
            //    return;
            //}
            if (txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().IndexOf("*") < -1)
            {
                bool chkflg = false;
                long crdNumber = 0;
                chkflg = long.TryParse(txtCardNumber.Text.ToString(), out crdNumber);
                if (!chkflg)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter valid Numeric Card Number','Required Information''ContentPlaceHolder1_txtCardNumber');", true);
                    txtCardNumber.Focus();
                    return;
                }
            }
            if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().Length < 16)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Credit Card Number must be 16 digit long.','Required Information','ContentPlaceHolder1_txtCardNumber');", true);
                txtCardNumber.Focus();
                return;
            }
            if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCardNumber.Text.ToString().Trim() != "" && (txtCardNumber.Text.ToString().Trim().Length < 15 || txtCardNumber.Text.ToString().Trim().Length > 15))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Credit Card Number must be 15 digit long.','Required Information','ContentPlaceHolder1_txtCardNumber');", true);
                txtCardNumber.Focus();
                return;
            }

            if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCSC.Text.ToString().Trim() != "" && (txtCSC.Text.ToString().Trim().Length < 3 || txtCSC.Text.ToString().Trim().Length > 3))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Card Verification Code must be 4 digit long.','Required Information','ContentPlaceHolder1_txtCSC');", true);
                txtCSC.Focus();
                return;
            }
            if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCSC.Text.ToString().Trim() != "" && txtCSC.Text.ToString().Trim().Length < 4)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Card Verification Code must be 3 digit long.','Required Information','ContentPlaceHolder1_txtCSC');", true);
                txtCSC.Focus();
                return;
            }
            if (ddlMonth.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Month.','Required Information','ContentPlaceHolder1_ddlMonth');", true);
                ddlMonth.Focus();
                return;
            }
            else if (ddlYear.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Select Year.','Required Information','ContentPlaceHolder1_ddlYear');", true);
                ddlYear.Focus();
                return;
            }
            else if ((Convert.ToInt32(ddlYear.SelectedValue) > DateTime.Now.Year) || (Convert.ToInt32(ddlYear.SelectedValue) == DateTime.Now.Year && Convert.ToInt32(ddlMonth.SelectedValue) >= DateTime.Now.Month))
            {

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "jAlert('Please Enter Valid Expiration Date.','Required Information','ContentPlaceHolder1_ddlYear');", true);
                return;
            }
            string cardNumber = "";
            string strNumber = "";
            if (txtCardNumber.Text.ToString().IndexOf("*") > -1)
            {
                if (ViewState["CardNumber"] != null)
                {
                    cardNumber = ViewState["CardNumber"].ToString();
                    strNumber = ViewState["CardNumber"].ToString();
                }
                Int32 IntUpdate = Convert.ToInt32(CommonComponent.ExecuteCommonData("UPDATE tb_order SET CardVarificationCode='" + txtCSC.Text.ToString() + "',CardType='" + ddlCardType.Text.ToString() + "',CardName='" + txtNameOnCard.Text.ToString().Replace("'", "''") + "',CardExpirationMonth='" + ddlMonth.SelectedValue.ToString() + "',CardExpirationYear='" + ddlYear.SelectedValue.ToString() + "',Last4='" + strNumber.Substring(strNumber.Length - 4) + "' WHERE OrderNumber=" + Request.QueryString["ID"].ToString() + ""));
            }
            else
            {
                strNumber = txtCardNumber.Text.ToString();
                cardNumber = SecurityComponent.Encrypt(txtCardNumber.Text.ToString());
                Int32 IntUpdate = Convert.ToInt32(CommonComponent.ExecuteCommonData("UPDATE tb_order SET CardVarificationCode='" + txtCSC.Text.ToString() + "',CardNumber='" + cardNumber + "',CardType='" + ddlCardType.Text.ToString() + "',CardName='" + txtNameOnCard.Text.ToString().Replace("'", "''") + "',CardExpirationMonth='" + ddlMonth.SelectedValue.ToString() + "',CardExpirationYear='" + ddlYear.SelectedValue.ToString() + "',Last4='" + strNumber.Substring(strNumber.Length - 4) + "' WHERE OrderNumber=" + Request.QueryString["ID"].ToString() + ""));
            }
            OrderComponent objOrderlog = new OrderComponent();
            objOrderlog.InsertOrderlog(20, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));

            ViewState["CardType"] = ddlCardType.SelectedItem.Text.ToString();
            ViewState["CardName"] = txtNameOnCard.Text.ToString();
            ViewState["CardYear"] = ddlYear.SelectedValue.ToString();
            ViewState["CardMonth"] = ddlYear.SelectedValue.ToString();
            ltrPayment.Text = "";
            if (ViewState["PaymentMethod"] != null)
            {
                ltrPayment.Text = "Payment Method : " + ViewState["PaymentMethod"].ToString();
            }
            if (ViewState["CardType"] != null && !string.IsNullOrEmpty(ViewState["CardType"].ToString()) && ViewState["CardType"].ToString().Trim().ToLower() == "american express")
            {
                ViewState["CardType"] = "AMEX";
            }
            else if (ViewState["CardType"] != null && !string.IsNullOrEmpty(ViewState["CardType"].ToString()) && ViewState["CardType"].ToString().Trim().ToLower() == "visa")
            {
                ViewState["CardType"] = "VISA";
            }
            ltrPayment.Text += "<br />Card Type : " + ViewState["CardType"].ToString();
            ltrPayment.Text += "<br />Card Holder's Name : " + ViewState["CardName"].ToString();
            string strNum = "";
            if (!string.IsNullOrEmpty(strNumber.ToString()))
            {

                if (strNumber.Length > 4 && strNumber.IndexOf("*") <= -1)
                {
                    for (int i = 0; i < strNumber.Length - 4; i++)
                    {
                        strNum += "*";
                    }
                    strNum += strNumber.ToString().Substring(strNumber.Length - 4);
                    ViewState["CardNumber"] = strNum;
                }
            }
            ViewState["CardVarificationCode"] = txtCSC.Text.ToString();
            //if (ViewState["CardNumber"] != null)
            //{
            //    ltrPayment.Text += "<br />Card Number : " + ViewState["CardNumber"].ToString();
            //}
            if (strNumber != "" && strNumber.Length > 4)
            {
                ltrPayment.Text += "<br />Card Number : ***********" + strNumber.ToString().Substring(strNumber.ToString().Length - 4);
            }

        }


        /// <summary>
        /// Get Credit Card Type By StoreID
        /// </summary>
        private void GetCreditCardType(Int32 StoreId)
        {
            CreditCardComponent objCreditcard = new CreditCardComponent();
            DataSet dsCard = new DataSet();
            ddlCardType.Items.Clear();
            dsCard = objCreditcard.GetAllCarTypeByStoreID(Convert.ToInt32(StoreId));
            if (dsCard != null && dsCard.Tables.Count > 0 && dsCard.Tables[0].Rows.Count > 0)
            {
                ddlCardType.DataSource = dsCard;
                ddlCardType.DataTextField = "CardType";
                ddlCardType.DataValueField = "CardType";
                ddlCardType.DataBind();
            }
            else
            {
                ddlCardType.DataSource = null;
                ddlCardType.DataBind();
            }
            ddlCardType.Items.Insert(0, new ListItem("Select Card Type", "0"));
            ddlCardType.SelectedIndex = 0;
            for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 10; i++)
                ddlYear.Items.Add(i.ToString());
        }

        /// <summary>
        /// Status Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlColor.Items.Clear();
            ddlColor.Items.Insert(0, new ListItem(ddlStatus.SelectedValue.ToString(), ddlStatus.SelectedValue.ToString()));
        }

        /// <summary>
        ///  Update Status Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateState_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                if (ddlStatus.SelectedItem.Text.ToString().ToLower() == "shipped")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_Order SET ShippedOn=getdate(),ShippedVIA='UPS',ShippingTrackingNumber='0', OrderStatus='" + ddlStatus.SelectedItem.Text.ToString().Replace("'", "''") + "' WHERE OrderNumber=" + Request.QueryString["id"].ToString() + "");
                }
                else if (ddlStatus.SelectedItem.Text.ToString().ToLower() == "canceled")
                {
                    try
                    {
                        OrderComponent objOrderlog = new OrderComponent();
                        objOrderlog.InsertOrderlog(21, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));

                        CommonComponent.ExecuteCommonData("UPDATE  tb_order SET orderStatus='CANCELED' WHERE orderNumber=" + Request.QueryString["id"] + "");
                        OrderComponent objOrder = new OrderComponent();
                        ltrProcessingStatus.Text = "CANCELED";
                        SendOrderCancelMail(Request.QueryString["id"].ToString());
                    }
                    catch
                    {

                    }

                }
                else
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_Order SET OrderStatus='" + ddlStatus.SelectedItem.Text.ToString().Replace("'", "''") + "' WHERE OrderNumber=" + Request.QueryString["id"].ToString() + "");
                }
                ltrProcessingStatus.Text = ddlStatus.SelectedItem.Text.ToString();
                if (ddlStatus.SelectedItem.Text.ToString().ToLower() != "hold")
                {
                    ahold.Visible = true;
                }
            }
        }

        /// <summary>
        ///  Hold Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnHold_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {

                CommonComponent.ExecuteCommonData("UPDATE tb_Order SET OrderStatus='Hold' WHERE OrderNumber=" + Request.QueryString["id"].ToString() + "");
                ltrProcessingStatus.Text = "Hold";
                ahold.Visible = false;
                OrderComponent objOrderlog = new OrderComponent();
                objOrderlog.InsertOrderlog(22, Convert.ToInt32(Request.QueryString["id"].ToString()), "", Convert.ToInt32(Session["AdminID"].ToString()));

            }
        }

        #region Bind and Update Order

        /// <summary>
        /// Function for Bind Order Products
        /// </summary>
        public void BindProducts()
        {
            GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["Id"].ToString()));
            dtPackProduct = new DataTable();
            dtPackProduct.Columns.AddRange(
            new DataColumn[] 
                        { 
                            new DataColumn("ProductID"), 
                            new DataColumn("ProductName"), 
                            new DataColumn("SKU"), 
                            new DataColumn("Items"), 
                            new DataColumn("ItemSKU"), 
                            new DataColumn("Quantity"), 
                            new DataColumn("NewQuantity"), 
                            new DataColumn("Supplier"), 
                            new DataColumn("Price"),
                            new DataColumn("OrderedPackageCartID"),
                            new DataColumn("UpgradeProductID"),
                            new DataColumn("UpgradeSKU"),
                            new DataColumn("UpgradeQuantity"),
                            new DataColumn("UpgradePrice"),
                            new DataColumn("IsPack"),
                            new DataColumn("NewPrice"),
                            new DataColumn("OrderedCustomCartID"),
                            new DataColumn("VariantNames"),
                            new DataColumn("VariantValues")

                        });
            dtPackProduct.Columns["ProductID"].DataType = typeof(Int32);

            ViewState["OrderTotal"] = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"]);

            #region Totals

            lblSubTotal.Text = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderSubTotal"]).ToString("f2");

            Decimal custDist = 0;
            Decimal.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["CustomDiscount"]), out custDist);
            Decimal Discount = custDist + Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"]) + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"]) + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["QuantityDiscountAmount"]) + Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"]), 2);

            ViewState["LevelDiscountAmount"] = dsOrder.Tables[0].Rows[0]["LevelDiscountAmount"];
            ViewState["CouponDiscountAmount"] = dsOrder.Tables[0].Rows[0]["CouponDiscountAmount"];
            ViewState["QuantityDiscountAmount"] = dsOrder.Tables[0].Rows[0]["QuantityDiscountAmount"];

            lblDiscount.Text = Math.Round(Discount, 2).ToString();

            decimal ShipCost = Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()), 2);
            lblShoppingCost.Text = ShipCost.ToString();

            decimal OrderTax = Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTax"].ToString()), 2);
            lblOrderTax.Text = OrderTax.ToString();

            decimal total = Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()), 2);
            lblTotal.Text = total.ToString();

            decimal AdjustmentAmount = Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"].ToString()), 2);
            lblAdjustmentAmount.Text = AdjustmentAmount.ToString();

            decimal FinalTotal = total + AdjustmentAmount;
            lblFinalTotal.Text = FinalTotal.ToString();

            #endregion Totals

            Int32.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["ShoppingCardID"]), out OrderedShoppingCartID);
            Int32.TryParse(Convert.ToString(dsOrder.Tables[0].Rows[0]["StoreID"]), out StoreID);

            decimal decAdjustmentAmount = decimal.Zero;
            decimal.TryParse(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"].ToString(), out decAdjustmentAmount);

            //if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() == AppLogic.ro_TXStateAuthorized)
            //    btnAdjustCapture.Visible = true;

            //if (decAdjustmentAmount > 0 && !string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["AdjustmentCapturedOn"].ToString()) && dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() == AppLogic.ro_TXStateCaptured)
            //{
            //    btnAdjustCapture.Visible = false;
            //}
            //else if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString()) && decAdjustmentAmount > 0 && string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["AdjustmentCapturedOn"].ToString()))
            //{
            //    btnAdjustCapture.Visible = true;
            //    btnAdjustCapture.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/capture-adjustments.png";
            //}

            //if (dsOrder.Tables[0].Rows[0]["TransactionStatus"].ToString() == AppLogic.ro_TXStateCaptured)
            //    btnRefund_UpdateOrder.Visible = true;

            OrderComponent objOrder = new OrderComponent();
            dtPackProduct = objOrder.GetInvoiceProducts(OrderedShoppingCartID).Tables[0];
            grdProducts.DataSource = dtPackProduct;
            grdProducts.DataBind();

        }

        DataTable dtPackProduct = null;
        Int32 StoreID = 0, OrderedShoppingCartID = 0;
        Decimal TotalPrice = 0, TotalUpgradePrice = 0, AdjustmentAmount = 0, Discount = 0, ShippingCost = 0, OrderTax = 0;

        /// <summary>
        /// Product Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSKU = (Label)e.Row.FindControl("lblSKU");

                Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                Label lblQty = (Label)e.Row.FindControl("lblQty");


                Label lblPrice = (Label)e.Row.FindControl("lblPrice");

                Label lblVariantNames = (Label)e.Row.FindControl("lblVariantNames");
                Label lblVariantValues = (Label)e.Row.FindControl("lblVariantValues");
                Label lblName = (Label)e.Row.FindControl("lblName");

                Label lblOrderedCustomCartID = (Label)e.Row.FindControl("lblOrderedCustomCartID");


                if (lblName != null)
                {
                    //if (lblProductID != null && lblOrderedCustomCartID != null)
                    //{
                    //    string strSql = "Select isnull(isManual,0) as isManual from tb_OrderedShoppingCartItems where RefProductID=" + lblProductID.Text + " and OrderedCustomCartID=" + lblOrderedCustomCartID.Text;
                    //    Boolean isManual = false;
                    //    isManual = Convert.ToBoolean(CommonComponent.GetScalarCommonData(strSql));
                    //    if (isManual)
                    //        btnRemove.Visible = true;
                    //    else btnRemove.Visible = false;
                    //}

                    System.Text.StringBuilder Table = new System.Text.StringBuilder();
                    Table.Append(lblName.Text.Trim());
                    string[] Names = lblVariantNames.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] Values = lblVariantValues.Text.Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    int iLoopValues = 0;
                    if (Names.Length == Values.Length)
                    {
                        for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;&nbsp;&nbsp;" + Names[iLoopValues] + " : " + Values[iLoopValues]);
                        }
                    }
                    else if (Values.Length > 0)
                    {
                        for (iLoopValues = 0; iLoopValues < Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;&nbsp;&nbsp;- " + Values[iLoopValues]);
                        }
                    }
                    else if (Names.Length > 0)
                    {
                        for (iLoopValues = 0; iLoopValues < Names.Length; iLoopValues++)
                        {
                            Table.Append("<br/>&nbsp;&nbsp;&nbsp;- " + Names[iLoopValues]);
                        }
                    }
                    lblName.Text = Table.ToString();
                    TotalPrice += Convert.ToDecimal(lblPrice.Text);
                }

            }
            else if (e.Row.RowType == DataControlRowType.Footer)
            {
                //  Label lblTotalPrice = (Label)e.Row.FindControl("lblTotalPrice");
                //  lblTotalPrice.Text = TotalPrice.ToString();
            }
        }

        /// <summary>
        /// Product Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Stop")
            {
                if (e.CommandSource is ImageButton)
                {
                    Int32 OrderedCustomCartID = Convert.ToInt32(e.CommandArgument);
                    Label lblProductID = (e.CommandSource as ImageButton).Parent.FindControl("lblProductID") as Label;

                    string strSql = "delete from tb_OrderedShoppingCartItems where OrderedCustomCartID=" + OrderedCustomCartID + " and RefProductID=" + lblProductID.Text;
                    CommonComponent.ExecuteCommonData(strSql);
                    strSql = "delete from tb_OrderedPackageShoppingCartItems where OrderedCustomCartID=" + OrderedCustomCartID + " and ProductID=" + lblProductID.Text;
                    CommonComponent.ExecuteCommonData(strSql);
                    BindProducts();
                }
            }
        }



        /// <summary>
        ///  Adjust Capture Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAdjustCapture_Click(object sender, EventArgs e)
        {
            int OrderNumber = 0;
            int.TryParse(Request.QueryString["ID"].ToString(), out OrderNumber);
            GetOrderDetailsByOrderNumber(Convert.ToInt32(Request.QueryString["Id"].ToString()));

            String AVSResult = String.Empty;
            String AuthorizationResult = String.Empty;
            String AuthorizationCode = String.Empty;
            String AuthorizationTransID = String.Empty;
            String TransactionCommand = String.Empty;
            String TransactionResponse = String.Empty;
            String Status1 = string.Empty;
            String Status2 = string.Empty;
            String strAlert = string.Empty;

            //Capture First Authorized Data. Original Order Capture.

            tb_Order objOrder = new tb_Order();
            string PaymentGateway = "";
            PaymentGateway = dsOrder.Tables[0].Rows[0]["PaymentGateway"].ToString();
            if (Convert.ToString(dsOrder.Tables[0].Rows[0]["TransactionStatus"]) == AppLogic.ro_TXStateAuthorized)
            {
                objOrder.CaptureTxCommand = "";
                objOrder.CaptureTXResult = "";
                objOrder.AuthorizationPNREF = "";

                objOrder.AuthorizationPNREF = dsOrder.Tables[0].Rows[0]["AuthorizationPNREF"].ToString();
                objOrder.OrderTotal = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString());
                if (PaymentGateway.ToString().ToLower().Trim() == "paypal")
                {
                    PayPalComponent objPayPal = new PayPalComponent();
                    Status2 = objPayPal.CaptureOrder(objOrder);
                }
                else if (PaymentGateway.ToString().ToLower().Trim() == "paypalexpress")
                {
                    PayPalComponent objPayPal = new PayPalComponent();
                    Status2 = objPayPal.CaptureOrder(objOrder);

                }

            }

            if (!string.IsNullOrEmpty(Status2))
            {
                if (Status2.Trim() == "OK")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_order SET CaptureTXCommand='" + objOrder.CaptureTxCommand.ToString().Replace("'", "''") + "',CaptureTXResult='" + objOrder.CaptureTXResult.ToString().Replace("'", "''") + "',AuthorizationPNREF='" + objOrder.AuthorizationPNREF.ToString().Replace("'", "''") + "', CapturedOn=dateadd(hour,-2,getdate()), TransactionStatus='CAPTURED' WHERE orderNumber=" + Request.QueryString["id"].ToString() + "");
                    CommonComponent.ExecuteCommonData("EXEC usp_Product_AdjustInventory " + OrderNumber + ",-1, 0");
                    OrderComponent objOrderlog = new OrderComponent();
                    objOrderlog.InsertOrderlog(2, OrderNumber, "", Convert.ToInt32(Session["AdminID"]));
                    Status2 = "Your Order Transaction has been CAPTURED.";
                }
                strAlert = Status2;
            }

            #region AdjustmentAmount

            if (Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"]) > 0)
            {
                decimal decl = Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"]);
                decl = decimal.Round(decl, 2);

                if (PaymentGateway.ToString().ToLower().Trim() == "paypal")
                {
                    PayPalComponent objPayPal = new PayPalComponent();
                    Object objorder1 = new Object();
                    objOrder = GetOrderDetailsForPayment(OrderNumber);
                    Status1 = objPayPal.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(objOrder.CustomerID), Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["AdjustmentAmount"]), AppLogic.AppConfigBool("UseLiveTransactions"), AppLogic.ro_TXStateCaptured, objOrder, String.Empty, objOrder, String.Empty, String.Empty, String.Empty, out  AVSResult, out  AuthorizationResult, out  AuthorizationCode, out  AuthorizationTransID, out  TransactionCommand, out TransactionResponse);
                    //Status1 = "OK";
                }
                else if (PaymentGateway.ToString().ToLower().Trim() == "paypalexpress")
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "msgReload", " alert('Paypal Express not Supported');", true);
                    return;
                }

                if (Status1 == AppLogic.ro_OK)
                {
                    String TransCMD = TransactionCommand;
                    String TransRES = AuthorizationResult;
                    String strSql = "update tb_Order set AdjustmentCapturedon=GetDate()," +
                        "AVSResult=isnull(convert(VarChar(max),AVSResult),'')+'-'+" + SQuote(AVSResult) + ", " +
                        "AuthorizationResult=isnull(convert(VarChar(max),AuthorizationResult),'')+'-'+" + SQuote(TransRES) + ", " +
                        "TransactionCommand=isnull(convert(VarChar(max),TransactionCommand),'')+'-'+" + SQuote(TransCMD) + ", " +
                        "AuthorizationPNREF=isnull(AuthorizationPNREF,'')+'" + AppLogic.AppConfigs("AUTHORIZENET_X_DELIM_CHAR") + AuthorizationTransID + "', " +
                        "CaptureTxCommand='" + TransactionCommand.Replace("'", "''") + "', " +
                        "CaptureTxResult='" + AuthorizationResult.Replace("'", "''") + "' " +
                        "where OrderNumber=" + Convert.ToString(dsOrder.Tables[0].Rows[0]["OrderNumber"]);
                    CommonComponent.ExecuteCommonData(strSql);
                }
            }
            if (!string.IsNullOrEmpty(Status1))
            {
                if (Status1.Trim() == "OK")
                {
                    Status1 = "Your Adjustment Transaction has been CAPTURED.";
                }
                strAlert = Status1;
            }

            string url = "Orders.aspx?ID=" + Request.QueryString["ID"];
            Page.ClientScript.RegisterStartupScript(this.GetType(), "msgReload", " alert('" + strAlert + "'); window.parent.parent.location.href='Orders.aspx?ID=" + Convert.ToString(Request.QueryString["ID"]) + "';", true);

            #endregion
        }

        /// <summary>
        /// Gets the Order Details for Payment
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns tb_Order Table Object</returns>
        private tb_Order GetOrderDetailsForPayment(Int32 OrderNumber)
        {
            DataSet dsOrder = new DataSet();
            dsOrder = CommonComponent.GetCommonDataSet("select * from tb_order where OrderNumber=" + OrderNumber);
            tb_Order objOrderData = new tb_Order();
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                objOrderData.FirstName = Convert.ToString(dsOrder.Tables[0].Rows[0]["FirstName"].ToString());
                objOrderData.LastName = Convert.ToString(dsOrder.Tables[0].Rows[0]["LastName"].ToString());
                objOrderData.Email = Convert.ToString(dsOrder.Tables[0].Rows[0]["Email"].ToString());

                //Billing Address
                objOrderData.BillingFirstName = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString());
                objOrderData.BillingLastName = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingLastName"].ToString());
                objOrderData.BillingAddress1 = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString());
                objOrderData.BillingAddress2 = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString());
                objOrderData.BillingSuite = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingSuite"].ToString());
                objOrderData.BillingCity = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCity"].ToString());
                objOrderData.BillingState = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingState"].ToString());
                objOrderData.BillingZip = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingZip"].ToString());
                objOrderData.BillingCountry = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingCountry"].ToString());
                objOrderData.BillingPhone = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingPhone"].ToString());
                objOrderData.BillingEmail = Convert.ToString(dsOrder.Tables[0].Rows[0]["BillingEmail"].ToString());

                // Credit Card Details
                objOrderData.CardName = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardName"].ToString());
                objOrderData.CardType = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardType"].ToString());
                objOrderData.CardVarificationCode = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardVarificationCode"].ToString());
                objOrderData.CardNumber = SecurityComponent.Decrypt(dsOrder.Tables[0].Rows[0]["CardNumber"].ToString());
                objOrderData.CardExpirationMonth = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                objOrderData.CardExpirationYear = Convert.ToString(dsOrder.Tables[0].Rows[0]["CardExpirationYear"].ToString());

                //Shipping Address
                objOrderData.ShippingFirstName = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString());
                objOrderData.ShippingLastName = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString());
                objOrderData.ShippingAddress1 = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString());
                objOrderData.ShippingAddress2 = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString());
                objOrderData.ShippingSuite = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString());
                objOrderData.ShippingCity = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCity"].ToString());
                objOrderData.ShippingState = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingState"].ToString());
                objOrderData.ShippingZip = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingZip"].ToString());
                objOrderData.ShippingCountry = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString());
                objOrderData.ShippingPhone = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString());
                objOrderData.ShippingEmail = Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingEmail"].ToString());
            }
            return objOrderData;
        }

        /// <summary>
        /// Button Click Event for Refund Updated Order
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnRefund_UpdateOrder_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            int OrderNumber = 0;
            int.TryParse(Request.QueryString["ID"].ToString(), out OrderNumber);
            System.Text.StringBuilder sw = new System.Text.StringBuilder();
            sw.Append("<script language='javascript' type='text/javascript'>");
            sw.Append("window.open('refundorder.aspx?ONo=" + OrderNumber + "','RefundOrder" + r.Next(1, 100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,copyhistory=no,width=600,height=500,left=0,top=0');");
            sw.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", sw.ToString());
        }

        /// <summary>
        /// Change the string,Replace the single Quote to Space
        /// </summary>
        /// <param name="s">string s</param>
        /// <returns>Change the string, Replace the single Quote to Space</returns>
        public String SQuote(String s)
        {
            int len = s.Length + 25;
            System.Text.StringBuilder tmpS = new System.Text.StringBuilder(len);
            tmpS.Append("N'");
            tmpS.Append(s.Replace("'", "''"));
            tmpS.Append("'");
            return tmpS.ToString();
        }

        #endregion

        /// <summary>
        /// Sends the order cancel mail.
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        private void SendOrderCancelMail(string OrderNumber)
        {
            try
            {
                string ToID = "";
                ToID = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(EmailID,'') FROM tb_ContactEmail WHERE Subject='Cancel Notice'"));
                if (string.IsNullOrEmpty(ToID))
                {
                    ToID = AppLogic.AppConfigs("ContactMail_ToAddress");
                }
                if (Session["AdminID"] != null)
                {
                    String cancelemail = Convert.ToString(CommonComponent.GetScalarCommonData("select EmailID from tb_Admin where AdminID=" + Convert.ToInt32(Session["AdminID"].ToString()) + ""));
                    if (!String.IsNullOrEmpty(cancelemail))
                    {
                        ToID = ToID + ";" + cancelemail;

                    }
                }

                StringBuilder sw = new StringBuilder(5000);
                CustomerComponent objProInq = new CustomerComponent();
                DataSet dsproductInquireies = new DataSet();
                Int32 SoreId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT Storeid FROM tb_order WHERE OrderNumber=" + OrderNumber + ""));
                string strSTOREPATH = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(ConfigValue,'') FROM tb_Appconfig WHERE ConfigName='STOREPATH' and StoreId=" + SoreId + " and isnull(Deleted,0)=0"));
                string strSTORENAME = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(ConfigValue,'') FROM tb_Appconfig WHERE ConfigName='STORENAME' and StoreId=" + SoreId + " and isnull(Deleted,0)=0"));
                string strLIVE_SERVER = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(ConfigValue,'') FROM tb_Appconfig WHERE ConfigName='LIVE_SERVER' and StoreId=" + SoreId + " and isnull(Deleted,0)=0"));
                dsproductInquireies = objProInq.GetEmailTamplate("CancelOrder", Convert.ToInt32(1));
                if (dsproductInquireies != null && dsproductInquireies.Tables.Count > 0 && dsproductInquireies.Tables[0].Rows.Count > 0)
                {
                    String strBody = "";
                    String strSubject = "";

                    strBody = dsproductInquireies.Tables[0].Rows[0]["EmailBody"].ToString();
                    strBody = strBody.Replace("/images/Store_1.png", "http://www.halfpricedrapes.com/images/Store_" + SoreId.ToString() + ".png");
                    strSubject = dsproductInquireies.Tables[0].Rows[0]["Subject"].ToString();
                    strBody = Regex.Replace(strBody, "###STOREPATH###", strSTOREPATH.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###LIVE_SERVER###", strLIVE_SERVER.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###STORENAME###", strSTORENAME.ToString(), RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                    strBody = Regex.Replace(strBody, "###OrderNumber###", OrderNumber, RegexOptions.IgnoreCase);
                    strBody = Regex.Replace(strBody, "###StoreID###", SoreId.ToString(), RegexOptions.IgnoreCase);
                    //  strSubject = AppLogic.AppConfigs("STORENAME").ToString() + " Order Cancelled - ONo: " + OrderNumber.ToString();
                    strSubject = dsproductInquireies.Tables[0].Rows[0]["Subject"].ToString().Replace("###OrderNumber###", OrderNumber.ToString());

                    AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                    if (SoreId != 11)
                    {
                        CommonOperations.SendMail(ToID + ";" + ahrefMail.InnerHtml.ToString().Trim(), strSubject, strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                    }
                    
                }
            }
            catch (Exception ex)
            {
            }
        }

        #region Order document
        /// <summary>
        ///  Upload Document Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            string CustDocPathTemp = AppLogic.AppConfigs("Order.DocumentPath") + Request.QueryString["Id"];
            if (!Directory.Exists(Server.MapPath(CustDocPathTemp)))
                Directory.CreateDirectory(Server.MapPath(CustDocPathTemp));
            if (fuUplodDoc.FileName.Length > 0)
            {
                ViewState["DocumentFileName"] = fuUplodDoc.FileName.ToString();
                fuUplodDoc.SaveAs(Server.MapPath(CustDocPathTemp) + "/" + fuUplodDoc.FileName);
                trDelete.Attributes.Add("style", "display:''; vertical-align: middle");
            }
            else
            {
                //ViewState["DocumentFileName"] = null;
                string Msg = "Please select file to Upload";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "UplMsg", "$(document).ready( function() {jAlert('" + Msg.ToString() + "', 'Message','ContentPlaceHolder1_fuUplodDoc');});", true);
            }
            GetOrderDocuments();
            SetUploadDocumentActive();


        }

        /// <summary>
        /// Sets the upload document Tab Active Setting.
        /// </summary>
        private void SetUploadDocumentActive()
        {
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgdocumnetUpload", " document.getElementById('ordernotes').removeAttribute('class');" +
               "document.getElementById('yearly').className='active';" +
               "document.getElementById('divOrderNote').style.display = 'none';" +
               "document.getElementById('divCustomerNote').style.display = 'none';" +
               "document.getElementById('divUploadDoc').style.display = '';", true);
        }

        /// <summary>
        /// Deletes the document od Customer.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string CustomerDoc)
        {
            try
            {
                string docPath = AppLogic.AppConfigs("Order.DocumentPath").ToString() + Request.QueryString["Id"] + "/" + CustomerDoc;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Gets the order documents.
        /// </summary>
        private void GetOrderDocuments()
        {
            string docPath = AppLogic.AppConfigs("Order.DocumentPath").ToString();
            if (Directory.Exists(Server.MapPath(docPath + Request.QueryString["Id"])))
            {
                DirectoryInfo di = new DirectoryInfo(Server.MapPath(docPath + Request.QueryString["Id"]));
                FileInfo[] rgFiles = di.GetFiles("*.*");

                if (rgFiles != null && rgFiles.Count() > 0)
                {
                    grdOrderDoc.DataSource = rgFiles;
                    grdOrderDoc.DataBind();
                }
                else
                {
                    grdOrderDoc.DataSource = null;
                    grdOrderDoc.DataBind();
                }
            }
        }

        /// <summary>
        /// Order Document Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdOrderDoc_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DeleteDocument(e.CommandArgument.ToString());
            GetOrderDocuments();
            SetUploadDocumentActive();
        }
        #endregion

        /// <summary>
        /// Check Haming Value and make Tab Visible
        /// </summary>
        protected void ChkHamingValueMakeTabVisible(Int32 OrderNumber)
        {
            DataSet dsProductVarinat = new DataSet();
            string StrOrderedCustomCartID = "";
            if (StoreID.ToString() == "1")
            {
                dsProductVarinat = CommonComponent.GetCommonDataSet("SElect * from tb_OrderedShoppingCartItems Where OrderedShoppingCartID in (Select ShoppingCardID from tb_order Where OrderNumber=" + OrderNumber + ") and (VariantNames like '%Header Design%' or VariantNames like '%Select Size%')");
            }
            else
            {
                dsProductVarinat = CommonComponent.GetCommonDataSet("SElect * from tb_OrderedShoppingCartItems Where OrderedShoppingCartID in (Select ShoppingCardID from tb_order Where OrderNumber=" + OrderNumber + ")");
            }

            if (dsProductVarinat != null && dsProductVarinat.Tables.Count > 0 && dsProductVarinat.Tables[0].Rows.Count > 0)
            {
                Boolean IsHamingProduct = false;
                for (int i = 0; i < dsProductVarinat.Tables[0].Rows.Count; i++)
                {
                    string StrVarintName = Convert.ToString(dsProductVarinat.Tables[0].Rows[i]["VariantNames"]);
                    string StrVariantValues = Convert.ToString(dsProductVarinat.Tables[0].Rows[i]["VariantValues"]);
                    string StrRefProductID = Convert.ToString(dsProductVarinat.Tables[0].Rows[i]["RefProductID"]);
                    string StrtempOrderedCustomCartID = Convert.ToString(dsProductVarinat.Tables[0].Rows[i]["OrderedCustomCartID"]);
                    string strSKu = Convert.ToString(dsProductVarinat.Tables[0].Rows[i]["SKU"]);
                    string productType = Convert.ToString(dsProductVarinat.Tables[0].Rows[i]["IsProductType"]);
                    if (productType == "1")
                    {
                        string Ismadetoorder = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(Ismadetoorder,0) FROM tb_product WHERE ProductId=" + StrRefProductID.ToString() + ""));
                        if (!string.IsNullOrEmpty(Ismadetoorder) && Convert.ToBoolean(Ismadetoorder))
                        {
                            continue;
                        }
                    }

                    string strUPC = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 UPC FROM tb_product WHERE ProductId=" + StrRefProductID.ToString() + ""));
                    Int32 Quantity = 0;
                    Int32.TryParse(Convert.ToString(dsProductVarinat.Tables[0].Rows[i]["Quantity"]), out Quantity);
                    //if (!string.IsNullOrEmpty(StrVarintName.ToString().Trim()) && !string.IsNullOrEmpty(StrVariantValues.ToString().Trim()))
                    //{
                    //    string[] StrVname = StrVarintName.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //    string[] StrVvalue = StrVariantValues.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //    string StrTempVariv = "";
                    //    string StrTempProductId = "";
                    //    Int32 VariQty = 0;
                    //    Int32 VariantValueID = 0;
                    //    DataSet dsvaraint = new DataSet();
                    //    if (StrVname.Length > 0 && StrVvalue.Length > 0)
                    //    {
                    //        bool nam = false;
                    //        bool val = false;
                    //        string strval = "";
                    //        for (int k = 0; k < StrVvalue.Length; k++)
                    //        {
                    //            VariQty = 0;

                    //            if (!string.IsNullOrEmpty(StrVvalue[k].ToString()))
                    //            {
                    //                if (StrVname[k].ToString().ToLower().IndexOf("header design") > -1)
                    //                {
                    //                    val = true;
                    //                    VariantValueID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT VariantValueID FROM tb_ProductVariantValue WHERE VariantValue = '" + StrVvalue[k].ToString() + "' AND ProductId in (SELECT ProductId FROM tb_product WHERE isnull(IsHamming,0)=1 AND StoreId=1 AND ProductId=" + StrRefProductID.ToString() + ")"));
                    //                    dsvaraint = CommonComponent.GetCommonDataSet("SELECT tb_ProductVariantValue.VariantValue,tb_ProductVariantValue.Inventory,isnull(tb_ProductVariantValue.AddiHemingQty,0) as AddiHemingQty FROM tb_ProductVariantValue   WHERE tb_ProductVariantValue.variantid in (SELECT variantid FROM tb_ProductVariant WHERE ParentId=" + VariantValueID + ")");

                    //                }
                    //                else if (StrVname[k].ToString().ToLower().IndexOf("select size") > -1 || StrVname[k].ToString().ToLower().IndexOf("selectsize") > -1)
                    //                {
                    //                    nam = true;
                    //                    strval = StrVvalue[k].ToString();
                    //                    if (strval.ToLower().IndexOf("wx") > -1)
                    //                    {
                    //                        strval = strval.Substring(strval.ToLower().IndexOf("wx") + 2, strval.ToLower().Length - strval.ToLower().IndexOf("wx") - 2);
                    //                        strval = strval.Substring(0, strval.ToLower().IndexOf("l"));
                    //                        strval = strval.Replace(" ", "");
                    //                    }
                    //                    else if (strval.ToLower().IndexOf("w x") > -1)
                    //                    {
                    //                        strval = strval.Substring(strval.ToLower().IndexOf("w x") + 3, strval.ToLower().Length - strval.ToLower().IndexOf("w x") - 3);
                    //                        strval = strval.Substring(0, strval.ToLower().IndexOf("l"));
                    //                        strval = strval.Replace(" ", "");
                    //                    }

                    //                }
                    //                if (nam == true && val == true && dsvaraint != null && dsvaraint.Tables.Count > 0 && dsvaraint.Tables[0].Rows.Count > 0)
                    //                {
                    //                    DataRow[] dr = dsvaraint.Tables[0].Select("VariantValue like '%" + strval + "l%'");
                    //                    bool chk = false;
                    //                    if (dr.Length > 0)
                    //                    {
                    //                        foreach (DataRow dr1 in dr)
                    //                        {

                    //                            if (Convert.ToInt32(dr1["Inventory"].ToString()) - Convert.ToInt32(dr1["AddiHemingQty"].ToString()) >= Quantity)
                    //                            {
                    //                                chk = true;

                    //                            }
                    //                            else
                    //                            {
                    //                                //Quantity = Quantity - Convert.ToInt32(dr1["Inventory"].ToString());
                    //                            }

                    //                        }

                    //                        if (chk == false)
                    //                        {
                    //                            for (int j = 0; j < dsvaraint.Tables[0].Rows.Count; j++)
                    //                            {
                    //                                string strlength = dsvaraint.Tables[0].Rows[j]["VariantValue"].ToString();
                    //                                if (strlength.ToLower().IndexOf("wx") > -1)
                    //                                {
                    //                                    strlength = strlength.Substring(strlength.ToLower().IndexOf("wx") + 2, strlength.ToLower().Length - strlength.ToLower().IndexOf("wx") - 2);
                    //                                    strlength = strlength.Substring(0, strlength.ToLower().IndexOf("l"));
                    //                                    strlength = strlength.Replace(" ", "");
                    //                                    try
                    //                                    {
                    //                                        if (Convert.ToInt32(strlength) > Convert.ToInt32(strval))
                    //                                        {
                    //                                            Int32 TQty = Convert.ToInt32(dsvaraint.Tables[0].Rows[j]["Inventory"].ToString()) - Convert.ToInt32(dsvaraint.Tables[0].Rows[j]["AddiHemingQty"].ToString());
                    //                                            if (Convert.ToInt32(TQty.ToString()) >= Quantity)
                    //                                            {
                    //                                                if (Session["vriantData"] == null)
                    //                                                {
                    //                                                    Session["vriantData"] = strlength + "-" + TQty.ToString() + ",";
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    Session["vriantData"] = Session["vriantData"] + strlength + "-" + TQty.ToString() + ",";
                    //                                                }
                    //                                                if (Session["StrtempOrderedCustomCartID"] == null)
                    //                                                {
                    //                                                    Session["StrtempOrderedCustomCartID"] = StrtempOrderedCustomCartID.ToString() + ",";
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    Session["StrtempOrderedCustomCartID"] = Session["StrtempOrderedCustomCartID"] + StrtempOrderedCustomCartID.ToString() + ",";
                    //                                                }
                    //                                                if (Session["vriantDataQty"] == null)
                    //                                                {
                    //                                                    Session["vriantDataQty"] = Quantity.ToString() + ",";
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    Session["vriantDataQty"] = Session["vriantDataQty"] + Quantity.ToString() + ",";
                    //                                                }
                    //                                                nam = false; val = false;
                    //                                            }
                    //                                        }

                    //                                    }
                    //                                    catch
                    //                                    {
                    //                                    }
                    //                                }
                    //                                else if (strlength.ToLower().IndexOf("w x") > -1)
                    //                                {
                    //                                    strlength = strlength.Substring(strlength.ToLower().IndexOf("w x") + 3, strlength.ToLower().Length - strlength.ToLower().IndexOf("w x") - 3);
                    //                                    strlength = strlength.Substring(0, strlength.ToLower().IndexOf("l"));
                    //                                    strlength = strlength.Replace(" ", "");
                    //                                    try
                    //                                    {
                    //                                        if (Convert.ToInt32(strlength) > Convert.ToInt32(strval))
                    //                                        {
                    //                                            Int32 TQty = Convert.ToInt32(dsvaraint.Tables[0].Rows[j]["Inventory"].ToString()) - Convert.ToInt32(dsvaraint.Tables[0].Rows[j]["AddiHemingQty"].ToString());
                    //                                            if (Convert.ToInt32(TQty) >= Quantity)
                    //                                            {
                    //                                                if (Session["vriantData"] == null)
                    //                                                {
                    //                                                    Session["vriantData"] = strlength + "-" + TQty.ToString() + ",";
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    Session["vriantData"] = Session["vriantData"] + strlength + "-" + TQty.ToString() + ",";
                    //                                                }
                    //                                                if (Session["StrtempOrderedCustomCartID"] == null)
                    //                                                {
                    //                                                    Session["StrtempOrderedCustomCartID"] = StrtempOrderedCustomCartID.ToString() + ",";
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    Session["StrtempOrderedCustomCartID"] = Session["StrtempOrderedCustomCartID"] + StrtempOrderedCustomCartID.ToString() + ",";
                    //                                                }
                    //                                                if (Session["vriantDataQty"] == null)
                    //                                                {
                    //                                                    Session["vriantDataQty"] = Quantity.ToString() + ",";
                    //                                                }
                    //                                                else
                    //                                                {
                    //                                                    Session["vriantDataQty"] = Session["vriantDataQty"] + Quantity.ToString() + ",";
                    //                                                }

                    //                                            }
                    //                                        }

                    //                                    }
                    //                                    catch
                    //                                    {

                    //                                    }
                    //                                }

                    //                            }
                    //                        }
                    //                    }
                    //                    nam = false; val = false;
                    //                }
                    //                //StrTempVariv = StrVvalue[k].ToString();

                    //            }
                    //        }

                    //        //if (VariantValueID > 0)
                    //        //{
                    //        //    string StrFinalVariValue = "";
                    //        //    if (StrTempVariv.ToString().IndexOf("(+$") > -1)
                    //        //    {
                    //        //        string[] StrTemp = StrTempVariv.ToString().Split("(+$".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //        //        if (StrTemp.Length > 0)
                    //        //        {
                    //        //            StrFinalVariValue = StrTemp[0].ToString();
                    //        //        }
                    //        //    }
                    //        //    else if (StrTempVariv.ToString().IndexOf("($") > -1)
                    //        //    {
                    //        //        string[] StrTemp = StrTempVariv.ToString().Split("($".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //        //        if (StrTemp.Length > 0)
                    //        //        {
                    //        //            StrFinalVariValue = StrTemp[0].ToString();
                    //        //        }
                    //        //    }
                    //        //    else { StrFinalVariValue = StrTempVariv.ToString(); }

                    //        //    if (!string.IsNullOrEmpty(StrFinalVariValue.ToString()))
                    //        //    {
                    //        //        VariQty = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT ISNULL(Inventory,0) as Inventory FROM tb_ProductVariantValue WHERE VariantID in (Select VariantID from tb_ProductVariant Where ProductID=" + StrRefProductID.ToString() + " and ParentId =" + VariantValueID + " and VariantValue='" + StrFinalVariValue.ToString() + "')"));
                    //        //    }
                    //        //    if ((Quantity > 0 && VariQty > 0) && Quantity > VariQty)
                    //        //    {
                    //        //        IsHamingProduct = true;
                    //        //        StrOrderedCustomCartID += StrtempOrderedCustomCartID + ",";
                    //        //    }

                    //        //    //DataSet dsVariantValue = new DataSet();
                    //        //    //dsVariantValue = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductVariantValue WHERE VariantID in (Select VariantID from tb_ProductVariant Where ProductID=" + StrRefProductID + " and ParentId =" + VariantValueID + ")");
                    //        //    //if (dsVariantValue != null && dsVariantValue.Tables.Count > 0 && dsVariantValue.Tables[0].Rows.Count > 0)
                    //        //    //{

                    //        //    //}
                    //        //}
                    //    }
                    //}
                    //else
                    {
                        bool chkall = false;


                        Int32 PhammingID = 0;
                        if (StoreID.ToString() == "1")
                        {
                            PhammingID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 ProductID FROM  tb_product WHERE isnull(StoreID,0)=1 AND isnull(IsHamming,0)=1 AND isnull(active,0)=1 and isnull(Deleted,0)=0 AND ProductId=" + StrRefProductID + ""));
                        }
                        else
                        {
                            PhammingID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 ProductID FROM  tb_product WHERE isnull(StoreID,0)=1 AND isnull(IsHamming,0)=1 AND isnull(active,0)=1 and isnull(Deleted,0)=0 AND UPC IN (SELECT UPC FROM tb_product WHERE isnull(active,0)=1 and isnull(deleted,0)=0 AND storeid=" + StoreID + " AND SKU='" + strSKu.Replace("'", "''") + "')"));
                            if (PhammingID == 0)
                            {
                                PhammingID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 ProductID FROM  tb_product WHERE isnull(StoreID,0)=1 AND isnull(IsHamming,0)=1 AND isnull(active,0)=1 and isnull(Deleted,0)=0 AND ProductId in (SELECT ProductID FROM tb_ProductVariantValue WHERE UPC IN (SELECT UPC FROM tb_product WHERE isnull(active,0)=1 and isnull(deleted,0)=0 AND storeid=" + StoreID + " AND SKU='" + strSKu.Replace("'", "''") + "'))"));
                            }
                        }
                        Int32 strAllowHeming = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 isnull(ConfigValue,'0') FROM tb_Appconfig WHERE ConfigName='IshemmingActive' AND isnull(Deleted,0)=0 and StoreId=1"));
                        if (strAllowHeming == 0)
                        {
                            PhammingID = 0;
                        }
                        //if (strSKu.IndexOf("-120") <= -1 && PhammingID > 0)
                        //{
                        double hamingpercantage = Convert.ToDouble(AppLogic.AppConfigs("HemmingGlobalsafety").ToString());
                        string strsku1 = "";

                        //if (strSKu.IndexOf("-84") > -1)
                        //{
                        //    strsku1 = "'" + strSKu.Replace("'", "''") + "','" + strSKu.Replace("-84", "-96").Replace("'", "''") + "','" + strSKu.Replace("-84", "-108").Replace("'", "''") + "','" + strSKu.Replace("-84", "-120").Replace("'", "''") + "'";
                        //}
                        //else if (strSKu.IndexOf("-96") > -1)
                        //{
                        //    strsku1 = "'" + strSKu.Replace("'", "''") + "','" + strSKu.Replace("-96", "-108").Replace("'", "''") + "','" + strSKu.Replace("-96", "-120").Replace("'", "''") + "'";
                        //}
                        //else if (strSKu.IndexOf("-108") > -1)
                        //{
                        //    strsku1 = "'" + strSKu.Replace("'", "''") + "','" + strSKu.Replace("-108", "-120").Replace("'", "''") + "'";
                        //    //strp = strp.Substring(0, strp.LastIndexOf("-") + 1);
                        //}
                        DataSet ds = new DataSet();
                        DataSet dsSKU = new DataSet();
                        if (StoreID.ToString() == "1")
                        {
                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU,isnull(Inventory,0) as Inventory,isnull(AddiHemingQty,0) as AddiHemingQty,UPC FROM tb_ProductVariantValue WHERE ProductId=" + PhammingID + " AND isnull(UPC,'') <> ''");
                            dsSKU = CommonComponent.GetCommonDataSet("SELECT DISTINCT TOP 1  SKU,isnull(Inventory,0) as Inventory,isnull(AddiHemingQty,0) as AddiHemingQty,UPC FROM tb_ProductVariantValue WHERE ProductId=" + PhammingID + " AND SKU='" + strSKu.Replace("'", "''") + "'");
                        }
                        else
                        {
                            ds = CommonComponent.GetCommonDataSet("SELECT DISTINCT SKU,isnull(Inventory,0) as Inventory,isnull(AddiHemingQty,0) as AddiHemingQty,UPC FROM tb_ProductVariantValue WHERE ProductId=" + PhammingID + " AND isnull(UPC,'') <> '' ");
                            dsSKU = CommonComponent.GetCommonDataSet("SELECT DISTINCT TOP 1  SKU,isnull(Inventory,0) as Inventory,isnull(AddiHemingQty,0) as AddiHemingQty,UPC FROM tb_ProductVariantValue WHERE ProductId=" + PhammingID + " AND UPC in (SELECT UPC FROM tb_product WHERE isnull(active,0)=1 and isnull(deleted,0)=0 AND storeid=" + StoreID + " AND SKU='" + strSKu.Replace("'", "''") + "')");
                        }
                        string strlength = "";
                        string strlength96 = "";
                        string strlength108 = "";
                        string strlength120 = "";

                        string strlengthUPC = "";
                        string strlength96UPC = "";
                        string strlength108UPC = "";
                        string strlength120UPC = "";

                        Int32 Qty84 = 0;
                        Int32 Qty96 = 0;
                        Int32 Qty108 = 0;
                        Int32 Qty120 = 0;

                        Int32 Qty84F = 0;
                        Int32 Qty96F = 0;
                        Int32 Qty108F = 0;
                        Int32 Qty120F = 0;

                        if (dsSKU != null && dsSKU.Tables.Count > 0 && dsSKU.Tables[0].Rows.Count > 0)
                        {
                            Int32 Qty = 0;
                            Qty = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(hamingpercantage) * Convert.ToDecimal(dsSKU.Tables[0].Rows[0]["Inventory"].ToString())) / Convert.ToDecimal(100))));
                            //hamingpercantage
                            //Qty = Convert.ToInt32(ds.Tables[0].Rows[0]["Inventory"].ToString()) 
                            if (strAllowHeming == 1 && PhammingID > 0)
                            {
                                Qty = Qty - Convert.ToInt32(dsSKU.Tables[0].Rows[0]["AddiHemingQty"].ToString());
                            }

                            //else
                            //{
                            //    Qty = Qty;
                            //}
                            if (Qty < 0)
                            {
                                Qty = 0;
                            }

                            if (strSKu.IndexOf("-84") > -1)
                            {
                                Qty84F = Qty;

                            }
                            else if (strSKu.IndexOf("-96") > -1)
                            {
                                Qty96F = Qty;
                                //Qty84 = Qty84
                            }
                            else if (strSKu.IndexOf("-108") > -1)
                            {
                                Qty108F = Qty;
                            }
                            else if (strSKu.IndexOf("-120") > -1)
                            {
                                Qty120F = Qty;
                            }

                            if (Convert.ToInt32(Qty) >= Quantity)
                            {
                                chkall = true;
                            }
                        }

                        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0 && chkall == false)
                        {
                            Int32 tempQty84 = 0;
                            Int32 tempQty96 = 0;
                            Int32 tempQty108 = 0;
                            Int32 tempQty120 = 0;
                            Int32 StoreQty = 0;
                            for (int ids = 0; ids < ds.Tables[0].Rows.Count; ids++)
                            {
                                //double Qty = Convert.ToDouble(); 
                                Int32 Qty = 0;
                                Qty = Convert.ToInt32(string.Format("{0:0}", Math.Floor(Convert.ToDecimal(Convert.ToDecimal(hamingpercantage) * Convert.ToDecimal(ds.Tables[0].Rows[ids]["Inventory"].ToString())) / Convert.ToDecimal(100))));
                                StoreQty = Qty;
                                if (strAllowHeming == 1 && PhammingID > 0)
                                {
                                    Qty = Qty - Convert.ToInt32(ds.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                }
                                else
                                {
                                    Qty = Qty;

                                }
                                if (Qty < 0)
                                {
                                    Qty = 0;
                                }

                                //if (Convert.ToInt32(Qty) >= Quantity)
                                //{

                                if (ds.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-84") > -1)
                                {
                                    if (strAllowHeming == 1 && PhammingID > 0)
                                    {
                                        tempQty84 = StoreQty - Convert.ToInt32(ds.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                    }
                                    else
                                    {
                                        tempQty84 = StoreQty;
                                    }
                                    strlength = ds.Tables[0].Rows[ids]["SKU"].ToString();
                                    strlengthUPC = ds.Tables[0].Rows[ids]["UPC"].ToString();
                                    Qty84 = Qty + Qty96;
                                    Qty84F = Qty;

                                }
                                else if (ds.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-96") > -1)
                                {
                                    if (strAllowHeming == 1 && PhammingID > 0)
                                    {
                                        tempQty96 = StoreQty - Convert.ToInt32(ds.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                    }
                                    else
                                    {
                                        tempQty96 = StoreQty;
                                    }
                                    strlength96 = ds.Tables[0].Rows[ids]["SKU"].ToString();
                                    strlength96UPC = ds.Tables[0].Rows[ids]["UPC"].ToString();
                                    Qty96 = Qty + Qty108;
                                    Qty96F = Qty;
                                    Qty84 = Qty84F + Qty96;
                                    //Qty84 = Qty84
                                }
                                else if (ds.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-108") > -1)
                                {
                                    if (strAllowHeming == 1 && PhammingID > 0)
                                    {
                                        tempQty108 = StoreQty - Convert.ToInt32(ds.Tables[0].Rows[ids]["AddiHemingQty"].ToString());
                                    }
                                    else
                                    {
                                        tempQty108 = StoreQty;
                                    }
                                    strlength108 = ds.Tables[0].Rows[ids]["SKU"].ToString();
                                    strlength108UPC = ds.Tables[0].Rows[ids]["UPC"].ToString();
                                    Qty108 = Qty + Qty120;
                                    Qty108F = Qty;
                                    Qty96 = Qty96F + Qty108;
                                    Qty84 = Qty84F + Qty96;
                                }
                                else if (ds.Tables[0].Rows[ids]["SKU"].ToString().IndexOf("-120") > -1)
                                {
                                    strlength120 = ds.Tables[0].Rows[ids]["SKU"].ToString();
                                    strlength120UPC = ds.Tables[0].Rows[ids]["UPC"].ToString();
                                    Qty120 = Qty;
                                    Qty120F = Qty;
                                    Qty108 = Qty108F + Qty120;
                                    Qty96 = Qty96F + Qty108;
                                    Qty84 = Qty84F + Qty96;
                                }
                                if (Session["StrtempOrderedCustomCartID"] == null)
                                {
                                    Session["StrtempOrderedCustomCartID"] = StrtempOrderedCustomCartID.ToString() + ",";
                                }
                                else
                                {
                                    Session["StrtempOrderedCustomCartID"] = Session["StrtempOrderedCustomCartID"] + StrtempOrderedCustomCartID.ToString() + ",";
                                }
                                if (Session["vriantDataQty"] == null)
                                {
                                    Session["vriantDataQty"] = Quantity.ToString() + ",";
                                }
                                else
                                {
                                    Session["vriantDataQty"] = Session["vriantDataQty"] + Quantity.ToString() + ",";
                                }

                                //}
                            }

                            //if (strSKu.Trim() != Convert.ToString(ds.Tables[0].Rows[ids]["SKU"].ToString()))
                            //{
                            //if (Session["vriantData"] == null)
                            //{
                            //    Session["vriantData"] = strlength + "-" + Qty.ToString() + ",";
                            //}
                            //else
                            //{
                            if (strUPC.ToString() == strlengthUPC.ToString())
                            {
                                Session["vriantData"] = strlength96 + "-" + Qty96F.ToString() + "," + strlength108 + "-" + Qty108F.ToString() + "," + strlength120 + "-" + Qty120F.ToString() + ",";
                            }
                            else if (strUPC.ToString() == strlength96UPC.ToString())
                            {
                                Session["vriantData"] = strlength108 + "-" + Qty108F.ToString() + "," + strlength120 + "-" + Qty120F.ToString() + ",";
                            }
                            else if (strUPC.ToString() == strlength108UPC.ToString())
                            {
                                Session["vriantData"] = strlength120 + "-" + Qty120F.ToString() + ",";
                            }
                            else if (strUPC.ToString() == strlength120UPC.ToString())
                            {
                                Session["vriantData"] = strlength120 + "-" + Qty120F.ToString() + ",";
                            }
                            else
                            {
                                Session["vriantData"] = strlength + "-" + Qty84F.ToString() + "," + strlength96 + "-" + Qty96F.ToString() + "," + strlength108 + "-" + Qty108F.ToString() + "," + strlength120 + "-" + Qty120F.ToString() + ",";
                            }

                            //Session["vriantData"] = Session["vriantData"] + strlength + "-" + Qty.ToString() + ",";

                            //}

                            //}


                        }

                        //}


                    }
                }
                if (Session["StrtempOrderedCustomCartID"] != null)
                {
                    //if (!string.IsNullOrEmpty(StrOrderedCustomCartID.ToString()))
                    //{
                    //    StrOrderedCustomCartID = StrOrderedCustomCartID.Substring(0, StrOrderedCustomCartID.Length - 1);
                    //    Session["HamingProduct"] = StrOrderedCustomCartID;
                    //}
                    hdnHamingTab.Value = "1";
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hamingtabvisi", "document.getElementById('li15').style.display = '';", true);
                    String strltrPrivate = ltrPrivate.Text.ToString().Replace("- Hemming Required", "");
                    strltrPrivate = strltrPrivate + "- Hemming Required";
                    ltrPrivate.Text = strltrPrivate;
                    CommonComponent.ExecuteCommonData("UPDATE tb_Order SET Notes='" + strltrPrivate.Replace("'", "''") + "' WHERE OrderNumber=" + OrderNumber.ToString() + "");

                }
            }

        }

        protected void btnUploadOrder_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["Id"] != null)
            {
                //CommonComponent.ExecuteCommonData("Update tb_order set IsBackEnd=0,IsuploadCancel=0 where ordernumber=" + Request.QueryString["Id"].ToString() + "");
                //btnUploadOrder.Visible = false;
                //btnUploadCancelOrder.Visible = true;

                CommonComponent.ExecuteCommonData("UPDATE tb_order SET isNAVInserted=0,isnavcompleted=1,IsNavError=0,NAVError='' WHERE OrderNumber=" + Request.QueryString["Id"].ToString() + "");
                btnUploadOrder.Visible = false;
                btnUploadCancelOrder.Visible = false;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorder", "jAlert('Order uploaded successfully, Please check in NAV after 5 minutes.','Message');", true);
            }
        }
        protected void btnUploadCancelOrder_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["Id"] != null)
            {
                CommonComponent.ExecuteCommonData("Update tb_order set IsBackEnd=1,IsuploadCancel=1 where ordernumber=" + Request.QueryString["Id"].ToString() + " and isnull(BackEndGUID,'')='' ");
                btnUploadCancelOrder.Visible = false;
                btnUploadOrder.Visible = true;
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@uploorder", "jAlert('Order upload Cancel successfully.','Message');", true);
            }
        }

        protected void btnuploadorderinnnav_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}
