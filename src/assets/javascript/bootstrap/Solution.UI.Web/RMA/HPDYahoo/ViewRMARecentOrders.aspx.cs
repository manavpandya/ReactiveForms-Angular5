using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components.Common;
using System.Net.Mail;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
namespace Solution.UI.Web.RMA.HPDYahoo
{
    public partial class ViewRMARecentOrders : System.Web.UI.Page
    {
        Int32 PageNo = 1;
        Int32 PageSize = 1;
        bool viewAll = false;
        Int32 PageCount = 0;
        System.Web.UI.WebControls.Literal ltrvartable = null;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(true);
            ltTitle.Text = "View Order";
            Session["PageOlderOrder"] = "false";
            UserControl userleft = (UserControl)Page.Master.FindControl("leftmenu");
            if (userleft != null)
            {
                System.Web.UI.HtmlControls.HtmlGenericControl hideleftmenu = (System.Web.UI.HtmlControls.HtmlGenericControl)userleft.FindControl("hideleftmenu");
                if (hideleftmenu != null)
                {
                    hideleftmenu.Visible = false;
                }
            }
            GetOrderDetails();
            //if (Request.QueryString["type"] != null)
            //{
            lnkBack.HRef = "/RMA/HPDYahoo/ReturnItem.aspx";
            //}
            //else { lnkBack.HRef = "/MyAccount.aspx"; }
        }

        /// <summary>
        /// Get Order Details
        /// </summary>
        private void GetOrderDetails()
        {
            DataSet DsOrder = new DataSet();
            Int32 custid = 0;
            Int32 ono = 0;
            if (Request.QueryString["type"] != null)
            {
                if (Request.QueryString["ono"] != null)
                {
                    Int32.TryParse(SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])), out ono);
                }
                else
                {

                }
            }
            else
            {
                if (Session["CID"] != null)
                {
                    Int32.TryParse(Session["CID"].ToString(), out custid);
                }
                else if (Session["CustID"] != null && Session["IsAnonymous"] != null && Session["IsAnonymous"].ToString().ToLower() == "false")
                {
                    Int32.TryParse(Convert.ToString(Session["CustID"]), out custid);
                }
                else if (Request.QueryString["ono"] != null)
                {
                    Int32.TryParse(SecurityComponent.Decrypt(Convert.ToString(Request.QueryString["ono"])), out ono);
                }
            }
            //for Paging

            OrderComponent objOrder = new OrderComponent();
            DataSet DsOrderPaging = new DataSet();
            if (custid > 0)
                DsOrderPaging = objOrder.GetViewOrderPaging(Convert.ToInt32(custid), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            // end for Paging 

            if (custid != 0)
                DsOrder = CommonComponent.GetCommonDataSet("select tb_order.OrderNumber from tb_order where CustomerID = " + custid + " and StoreId=" + AppLogic.AppConfigs("StoreID").ToString() + " and case when ShippingTrackingNumber is null then (select cast((count(*)) as nvarchar(max)) from tb_OrderShippedItems where tb_OrderShippedItems.ordernumber=tb_order.ordernumber) else ShippingTrackingNumber end  <>'0' order by ordernumber desc");
            else if (ono != 0)
                DsOrder = CommonComponent.GetCommonDataSet("select tb_order.OrderNumber from tb_order where OrderNumber = " + ono + " and case when ShippingTrackingNumber is null then (select cast((count(*)) as nvarchar(max)) from tb_OrderShippedItems where tb_OrderShippedItems.ordernumber=tb_order.ordernumber) else ShippingTrackingNumber end  <>'0' order by ordernumber desc");

            else
            {
                lblMsg.Text = " Sorry, Orders have been not Shipped yet. ";
                lblMsg.Visible = true;
                paging.Visible = false;
                return;
            }

            if (DsOrder != null && DsOrder.Tables.Count > 0 && DsOrder.Tables[0].Rows.Count > 0)
            {
                StringBuilder Table = new StringBuilder();
                DataRow[] dr = DsOrder.Tables[0].Select(); //("isnull(IsNew,0) = 0 AND isnull(ShippedVIA,0) = 1"); for old order
                if (dr != null && dr.Length > 0)
                {
                    bool isReturnitem = false;
                    bool isShipped = true;

                    foreach (DataRow drOrderNums in dr)
                    {
                        string OrderNumber = drOrderNums["OrderNumber"].ToString();
                        // dsOrder = objOrder.GetViewRecentOrderByOrderID(Convert.ToInt32(drOrderNums["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        DataSet dsOrder = new DataSet();
                        dsOrder = objOrder.GetViewRecentForRMAOrderByOrderID(Convert.ToInt32(drOrderNums["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                        if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
                        {
                            Table.Append("<table style='float:left;' width='100%' align='left' border='0' cellpadding='0' cellspacing='0' class='table-none'  >");
                            Table.Append("<tr>");
                            Table.Append("<td valign='top' style=\"border-right: 1px solid #DFDFDF;width:230px;align:left;\">");
                            Table.Append("<table width='100%' align='left' cellpadding='0' cellspacing='0' class='table-none'  style='border:0px;'>");
                            Table.Append("<tr>");
                            Table.Append("<td>Order Date");
                            Table.Append("<br/><p style=\"font-size:14px;padding-right:0px; font-weight:bold;width:230px;\">" + Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString()).ToLongDateString() + "</p>");
                            //Table.Append("<br/><br/><a href=\"/orderdetails.aspx?ono=" + Convert.ToString(drOrderNums["OrderNumber"].ToString()) + "\" >View Order Details</a> | <a href='invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(Convert.ToString(drOrderNums["OrderNumber"].ToString()))) + "' target=\"_blank\"\">View Invoice</a>  ");
                            Table.Append("<br/><br/><a href='/RMA/HPDYahoo/invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(Convert.ToString(drOrderNums["OrderNumber"].ToString()))) + "' target=\"_blank\"\">View Invoice</a>  ");
                            Table.Append("</td>");
                            Table.Append("</tr>");
                            Table.Append("<tr>");
                            Table.Append("<td>Order Number: " + Convert.ToString(drOrderNums["OrderNumber"].ToString() + ""));
                            Table.Append("</td>");
                            Table.Append("</tr>");
                            Table.Append("<tr>");
                            Table.Append("<td>Shipping Method: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString() + ""));
                            Table.Append("</td>");
                            Table.Append("</tr>");
                            Table.Append("<tr>");
                            Table.Append("<td>Recipient: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["LastName"].ToString() + ""));
                            Table.Append("</td>");
                            Table.Append("</tr>");
                            Table.Append("<tr>");
                            Table.Append("<td>Order Total: $" + Convert.ToString(Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()), 2)) + "");
                            Table.Append("</td>");
                            Table.Append("</tr>");

                            if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString()))
                            {
                                if (dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString().ToLower() == "new")
                                {
                                    Table.Append("<tr>");
                                    Table.Append("<td>Order Status: Pending");
                                    Table.Append("</td>");
                                    Table.Append("</tr>");
                                }
                                else
                                {
                                    Table.Append("<tr>");
                                    Table.Append("<td>Order Status: " + dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString() + "");
                                    Table.Append("</td>");
                                    Table.Append("</tr>");
                                }
                            }

                            Table.Append("</table>");
                            Table.Append("</td>");
                            Table.Append("<td style='padding-top:5px;width:98%;border-right: 1px solid #DFDFDF;' vAlign='top'>");

                            // bind Right Side Details with shipped and tracking details.
                            DataSet DsCartDetail = new DataSet();
                            DsCartDetail = objOrder.GetCartItemForViewRecentOrder(Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), Convert.ToInt32(dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                            if (DsCartDetail != null && DsCartDetail.Tables.Count > 0 && DsCartDetail.Tables[0].Rows.Count > 0)
                            {
                                Table.Append("<table width='100%' align='left' cellpadding='0' cellspacing='0' class='table_border_none' >");
                                Table.Append("<tr style='vAlign:top;'>");
                                Table.Append("<td colspan='2' vAlign='top' style='border-bottom:1px solid #DFDFDF;'>");
                                Table.Append("<p style='font-size:14px;font-weight:bold;width:83%;'>Ordered Items</p>");
                                Table.Append("</td>");
                                Table.Append("</tr>");

                                for (Int32 i = 0; i < DsCartDetail.Tables[0].Rows.Count; i++)
                                {
                                    string ProductId = Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString());
                                    Table.Append("<tr style='vAlign:top;'>");
                                    Table.Append("<td>&nbsp;</td>");
                                    Table.Append("<td vAlign='top'>");
                                    isReturnitem = false;
                                    int RMANo = 0;
                                    DateTime RMADate = new DateTime();
                                    DataSet dsReturnItem = new DataSet();
                                    dsReturnItem = CommonComponent.GetCommonDataSet("select * from dbo.tb_ReturnItem where OrderedNumber=" + OrderNumber + " and ProductID=" + ProductId + "");

                                    if (dsReturnItem != null && dsReturnItem.Tables.Count > 0 && dsReturnItem.Tables[0].Rows.Count > 0)
                                    {
                                        RMANo = Convert.ToInt32(dsReturnItem.Tables[0].Rows[0]["ReturnItemID"].ToString());
                                        RMADate = Convert.ToDateTime(dsReturnItem.Tables[0].Rows[0]["CreatedOn"].ToString());
                                        Int32 ttt = Convert.ToInt32(dsReturnItem.Tables[0].Rows[0]["Quantity"].ToString());
                                        if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["Quantity"].ToString()) > ttt)
                                        {
                                            isReturnitem = false;
                                        }
                                        else
                                        {
                                            isReturnitem = true;
                                        }
                                    }

                                    DataSet dsShippReturnItem = new DataSet();
                                    dsShippReturnItem = CommonComponent.GetCommonDataSet("select RefProductID from dbo.tb_OrderShippedItems where OrderNumber=" + OrderNumber + " and RefProductID=" + DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString() + " ");
                                    if ((dsShippReturnItem.Tables[0].Rows.Count > 0) && (isReturnitem != true))
                                    {
                                        isReturnitem = false;
                                    }
                                    else
                                    {
                                        isReturnitem = true;
                                    }
                                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString()))
                                    {
                                        DateTime dtShippedOn = Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["shippedon"].ToString());
                                        String strCurrentDate = dtShippedOn.ToShortDateString();
                                        String strCurrentTrackingNo = dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString();
                                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippedon"].ToString()))
                                        {
                                            Table.Append("Shipment 1 of " + (i + 1) + "<br/>");
                                            if (isReturnitem == true)
                                                Table.Append("<strong style='font-size:11px;color:#000;'>Shipped & Returned</strong><br/>");
                                            else
                                                Table.Append("<strong style='font-size:11px;color:#000;'>Shipped</strong><br/>");

                                            Table.Append("Shipped On:" + dsOrder.Tables[0].Rows[0]["shippedon"].ToString() + "<br/>");
                                            if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippingTrackingNumber"].ToString()))
                                                Table.Append("Tracking Number:" + dsOrder.Tables[0].Rows[0]["shippingTrackingNumber"].ToString() + "<br/>");
                                            if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippedvia"].ToString()))
                                                Table.Append("Shipsped Via:" + dsOrder.Tables[0].Rows[0]["shippedvia"].ToString() + "<br/>");

                                            if (isReturnitem == true) // coding for RMA
                                            {
                                                Table.Append("Returned On: <b>" + RMADate + "</b><br/>");
                                                Table.Append("RMA No: <b Style='font-size:12px;color:#B92127;'> RMA - " + RMANo.ToString() + "</b>  &nbsp;");
                                                Table.Append("for RMA slip <b Style='font-size:12px;color:#B92127;'><a onclick='javascript:window.open(\"ReturnPackageSlip.aspx?ID=" + RMANo.ToString() + "\", \"\",\"height=600,width=820,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");' style='text-decoration:none;cursor:pointer;'>click here</a></b><br/>");
                                            }
                                            Table.Append("</td>");
                                            Table.Append("</tr>");
                                        }
                                        else
                                        {
                                            // Set ParsalShipping 
                                            DataSet dtParsalShipping1 = new DataSet();
                                            dtParsalShipping1 = CommonComponent.GetCommonDataSet("select ISNULL(TrackingNumber,'') as TrackingNumber,ShippedVia,Shipped,ShippedOn  from tb_orderShippedItems where orderNumber=" + OrderNumber + " And RefProductID=" + ProductId + " and ISNULL(TrackingNumber,'') <> '' and ISNULL(ShippedVia,'') <> '' ");

                                            if (dtParsalShipping1 != null && dtParsalShipping1.Tables.Count > 0 && dtParsalShipping1.Tables[0].Rows.Count > 0)
                                            {
                                                Table.Append("<strong>Shipment 1 of 1</strong><br/>");
                                                if (isReturnitem == true)
                                                    Table.Append("<strong style='font-size:11px;color:#000;'>Shipped & Returned</strong><br/>");
                                                else
                                                    Table.Append("<strong style='font-size:11px;color:#000;'>Shipped</strong><br/>");

                                                Table.Append("Shipped On:" + dtParsalShipping1.Tables[0].Rows[0]["shippedon"].ToString() + "<br/>");
                                                if (!String.IsNullOrEmpty(dtParsalShipping1.Tables[0].Rows[0]["TrackingNumber"].ToString()))
                                                    Table.Append("Tracking Number:" + dtParsalShipping1.Tables[0].Rows[0]["TrackingNumber"].ToString() + "<br/>");
                                                if (!String.IsNullOrEmpty(dtParsalShipping1.Tables[0].Rows[0]["ShippedVia"].ToString()))
                                                    Table.Append("Shipped Via:" + dtParsalShipping1.Tables[0].Rows[0]["shippedvia"].ToString() + "<br/>");

                                                if (isReturnitem == true)
                                                {
                                                    Table.Append("Returned On: <b>" + RMADate + "</b><br/>");
                                                    Table.Append("RMA No: <b Style='font-size:12px;color:#B92127;'> RMA - " + RMANo.ToString() + "</b>  &nbsp;");
                                                    Table.Append("for RMA slip <b Style='font-size:12px;color:#B92127;'><a onclick='javascript:window.open(\"ReturnPackageSlip.aspx?ID=" + RMANo.ToString() + "\", \"\",\"height=600,width=820,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");' style='text-decoration:none;cursor:pointer;'>click here</a></b><br/>");
                                                }
                                                isShipped = true;
                                                Table.Append("</td>");
                                                Table.Append("</tr>");
                                            }
                                            else
                                            {
                                                isShipped = false;
                                                Table.Append("<strong style='font-size: 11px; color:#000;';>Not Shipped</strong><br/>");
                                            }
                                        }

                                        Table.Append("<tr style='valign:top;'>");

                                        if ((DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString() == null) || (DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString() == ""))
                                        {
                                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()))
                                            {
                                                BindVariantForProduct(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString(), DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString());
                                            }
                                            else
                                            {
                                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                                ltrvartable.Text = string.Empty;
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["YahooID"].ToString())))
                                            {
                                                string StrPath = AppLogic.AppConfigs("STOREPATH") + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["YahooID"].ToString()) + ".html";
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='http://" + StrPath.ToString() + "' target='_blank'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                                Table.Append("<td align='left' width='83%' valign='top'><a href='http://" + StrPath.ToString() + "' target='_blank'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='javascript:void(0);'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                                Table.Append("<td align='left' width='83%' valign='top'><label style='color:#B92127;'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</label><br />" + ltrvartable.Text + "");
                                            }
                                            Table.Append("</td>");
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()))
                                            {
                                                BindVariantForProduct(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()), Convert.ToString(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()));
                                            }
                                            else
                                            {
                                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                                ltrvartable.Text = string.Empty;
                                            }
                                            if (!string.IsNullOrEmpty(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["YahooID"].ToString())))
                                            {
                                                string StrPath = AppLogic.AppConfigs("STOREPATH") + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["YahooID"].ToString()) + ".html";
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='http://" + StrPath.ToString() + "' target='_blank'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                                Table.Append("<td align='left' width='83%' valign='top'><a href='http://" + StrPath.ToString() + "' target='_blank'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='javascript:void(0);'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                                Table.Append("<td align='left' width='83%' valign='top'><label style='color:#B92127;'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</label><br />" + ltrvartable.Text + "");
                                            }
                                            Table.Append("</td>");
                                        }
                                        Table.Append("</tr>");
                                        if (isReturnitem == true)
                                        { }
                                        else
                                        {
                                            int days = 0;
                                            //days = Convert.ToInt32(dbAccess.ExecuteScalerQuery(" select isnull(returndays,0) from tb_ecomm_product where productid=" + ProductID));
                                            if (days == 0)
                                            {
                                                if (AppConfig.StoreID > 0)
                                                {
                                                    try
                                                    {
                                                        Int32.TryParse(AppLogic.AppConfigs("ReturnDays").ToString(), out days);
                                                    }
                                                    catch { }
                                                }
                                            }
                                            if (isShipped)
                                            {
                                                if (days == 0)
                                                    days = 30;
                                                DateTime dt = Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString());
                                                Table.Append("<tr style='vAlign:top;'>");
                                                Table.Append("<td style='border-bottom:1px solid #e7e7e7;'>&nbsp;</td>");
                                                Table.Append("<td vAlign='top' align='right' style='border-bottom:1px solid #e7e7e7;'>");
                                                if (dt.AddDays(days) >= DateTime.Today)
                                                {
                                                    int IsItemalreadyretured = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_ReturnItem where OrderedNumber =" + OrderNumber.ToString() + " and ProductID=" + DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString() + ""));
                                                    if (IsItemalreadyretured == 0)
                                                        Table.Append("<a href=\"returnmerchandise.aspx?prodid=" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + "&ono=" + Server.UrlEncode(SecurityComponent.Encrypt(drOrderNums["OrderNumber"].ToString())) + "\"><img src=\"/images/return_item.png\" /></a>&nbsp;");
                                                }
                                                else if (dt.AddDays(days + 30) >= DateTime.Today)
                                                {
                                                    Table.Append("<a href=\"ReturnPackageSlip.aspx?prodid=" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + "&ono=" + Server.UrlEncode(SecurityComponent.Encrypt(drOrderNums["OrderNumber"].ToString())) + "&StoreCredit=1&Qty=" + DsCartDetail.Tables[0].Rows[i]["Quantity"].ToString() + "\"><img src=\"/images/store_credit.png\" /></a>&nbsp;");
                                                }
                                                else
                                                {
                                                    Table.Append("<a style='font-weight:bold;' href=\"mailto:" + AppLogic.AppConfigs("ContactMail_ToAddress").ToString() + "\"><img src=\"/images/send_e_Mail_for_request.png\" border=\"0\" /></a>&nbsp;");
                                                }
                                                Table.Append("</td>");
                                                Table.Append("</tr>");
                                            }
                                        }
                                    }
                                    // Tracking Else
                                    else
                                    {
                                        Table.Append("<tr>");
                                        Table.Append("<td colspan='3' align='left' style='border-bottom:1px solid #E5E5E5'>");

                                        if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippedon"].ToString()) && dsOrder.Tables[0].Rows[0]["shippedon"].ToString() != "Null")
                                        {
                                            Table.Append("Shipment 1 of " + (i + 1) + "<br/>");
                                            if (isReturnitem == true)
                                                Table.Append("<strong style='font-size:11px;color:#000;'>Shipped & Returned</strong><br/>");
                                            else
                                                Table.Append("<strong style='font-size:11px;color:#000;'>Shipped</strong><br/>");

                                            Table.Append("Shipped On:" + dsOrder.Tables[0].Rows[0]["shippedon"].ToString() + "<br/>");
                                            if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippingTrackingNumber"].ToString()))
                                                Table.Append("Tracking Number:" + dsOrder.Tables[0].Rows[0]["shippingTrackingNumber"].ToString() + "<br/>");
                                            if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippedvia"].ToString()))
                                                Table.Append("Shipsped Via:" + dsOrder.Tables[0].Rows[0]["shippedvia"].ToString() + "<br/>");

                                            if (isReturnitem == true)
                                            {
                                                Table.Append("Returned On: <b>" + RMADate + "</b><br/>");
                                                Table.Append("RMA No: <b Style='font-size:12px;color:#B92127;'> RMA - " + RMANo.ToString() + "</b>  &nbsp;");
                                                Table.Append("for RMA slip <b Style='font-size:12px;color:#B92127;'><a onclick='javascript:window.open(\"ReturnPackageSlip.aspx?ID=" + RMANo.ToString() + "\", \"\",\"height=600,width=820,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");' style='text-decoration:none;cursor:pointer;'>click here</a></b><br/>");
                                                Table.Append("</td>");
                                                Table.Append("</tr>");
                                            }
                                        }
                                        else
                                        {
                                            // Set ParsalShipping 
                                            DataSet dtParsalShipping1 = new DataSet();
                                            dtParsalShipping1 = CommonComponent.GetCommonDataSet("select ISNULL(TrackingNumber,'') as TrackingNumber,ShippedVia,Shipped,ShippedOn  from tb_orderShippedItems where orderNumber=" + OrderNumber + " And RefProductID=" + ProductId + "  and ISNULL(TrackingNumber,'') <> '' and ISNULL(ShippedVia,'') <> ''");

                                            if (dtParsalShipping1 != null && dtParsalShipping1.Tables.Count > 0 && dtParsalShipping1.Tables[0].Rows.Count > 0)
                                            {
                                                Table.Append("<strong>Shipment 1 of 1</strong><br/>");
                                                if (isReturnitem == true)
                                                    Table.Append("<strong style='font-size:11px;color:#000;'>Shipped & Returned</strong><br/>");
                                                else
                                                    Table.Append("<strong style='font-size:11px;color:#000;'>Shipped</strong><br/>");

                                                Table.Append("Shipped On:" + dtParsalShipping1.Tables[0].Rows[0]["shippedon"].ToString() + "<br/>");
                                                if (!string.IsNullOrEmpty(dtParsalShipping1.Tables[0].Rows[0]["TrackingNumber"].ToString()))
                                                    Table.Append("Tracking Number:" + dtParsalShipping1.Tables[0].Rows[0]["TrackingNumber"].ToString() + "<br/>");
                                                if (!string.IsNullOrEmpty(dtParsalShipping1.Tables[0].Rows[0]["shippedvia"].ToString()))
                                                    Table.Append("Shipped Via:" + dtParsalShipping1.Tables[0].Rows[0]["shippedvia"].ToString() + "<br/>");

                                                if (isReturnitem == true)
                                                {
                                                    Table.Append("Returned On: <b>" + RMADate + "</b><br/>");
                                                    Table.Append("RMA No: <b Style='font-size:12px;color:#B92127;'> RMA - " + RMANo.ToString() + "</b>  &nbsp;");
                                                    Table.Append("for RMA slip <b Style='font-size:12px;color:#B92127;'><a onclick='javascript:window.open(\"ReturnPackageSlip.aspx?ID=" + RMANo.ToString() + "\", \"\",\"height=600,width=820,scrollbars=1,left=50,top=50,toolbar=no,menubar=no\");' style='text-decoration:none;cursor:pointer;'>click here</a></b><br/>");
                                                }
                                                isShipped = true;
                                                Table.Append("</td>");
                                                Table.Append("</tr>");
                                            }
                                            else
                                            {
                                                isShipped = false;
                                                Table.Append("<strong style='font-size: 11px; color:#000;';>Not Shipped</strong><br/>");
                                            }

                                        }
                                        Table.Append("<tr style='valign:top;'>");

                                        if ((DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString() == null) || (DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString() == ""))
                                        {
                                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()))
                                            {
                                                BindVariantForProduct(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString(), DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString());
                                            }
                                            else
                                            {
                                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                                ltrvartable.Text = string.Empty;
                                            }
                                            //Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                            //Table.Append("</td>");
                                            //Table.Append("<td align='left' width='83%' valign='top'><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a></a><br />" + ltrvartable.Text + "");
                                            if (!string.IsNullOrEmpty(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["YahooID"].ToString())))
                                            {
                                                string StrPath = AppLogic.AppConfigs("STOREPATH") + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["YahooID"].ToString()) + ".html";
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='http://" + StrPath.ToString() + "' target='_blank'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                                Table.Append("<td align='left' width='83%' valign='top'><a href='http://" + StrPath.ToString() + "' target='_blank'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='javascript:void(0);'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                                Table.Append("<td align='left' width='83%' valign='top'><label style='color:#B92127;'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</label><br />" + ltrvartable.Text + "");
                                            }
                                            Table.Append("</td>");
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()))
                                            {
                                                BindVariantForProduct(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["VariantNames"].ToString()), Convert.ToString(DsCartDetail.Tables[0].Rows[i]["VariantValues"].ToString()));
                                            }
                                            else
                                            {
                                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                                ltrvartable.Text = string.Empty;
                                            }
                                            //Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString()) + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                            //Table.Append("</td>");
                                            //Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["MainCategory"].ToString()) + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                            if (!string.IsNullOrEmpty(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["YahooID"].ToString())))
                                            {
                                                string StrPath = AppLogic.AppConfigs("STOREPATH") + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["YahooID"].ToString()) + ".html";
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='http://" + StrPath.ToString() + "' target='_blank'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                                Table.Append("<td align='left' width='83%' valign='top'><a href='http://" + StrPath.ToString() + "' target='_blank'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='javascript:void(0);'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[i]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                                Table.Append("<td align='left' width='83%' valign='top'><label style='color:#B92127;'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["Name"].ToString()) + "</label><br />" + ltrvartable.Text + "");
                                            }
                                            Table.Append("</td>");
                                        }
                                        Table.Append("</tr>");

                                        //if (isauthorizeRefund == true && isReturnitem == true)
                                        if (isReturnitem == true)
                                        { }
                                        else
                                        {
                                            int days = 0;
                                            //days = Convert.ToInt32(dbAccess.ExecuteScalerQuery(" select isnull(returndays,0) from tb_ecomm_product where productid=" + ProductID));

                                            if (days == 0)
                                            {
                                                Int32.TryParse(AppLogic.AppConfigs("ReturnDays").ToString(), out days);
                                            }
                                            if (isShipped)
                                            {
                                                if (days == 0)
                                                    days = 30;
                                                DateTime dt = Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString());
                                                Table.Append("<tr style='vAlign:top;'>");
                                                Table.Append("<td>&nbsp;</td>");
                                                Table.Append("<td vAlign='top' align='right'>");
                                                if (dt.AddDays(days) >= DateTime.Today)
                                                {
                                                    Table.Append("<a href=\"returnmerchandise.aspx?prodid=" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + "&ono=" + Server.UrlEncode(SecurityComponent.Encrypt(drOrderNums["OrderNumber"].ToString())) + "\"><img src=\"/images/return_item.png\" /></a>&nbsp;");
                                                }
                                                else if (dt.AddDays(days + 30) >= DateTime.Today)
                                                {
                                                    // Table.Append("<a href=\"ReturnPackageSlip.aspx?prodid=" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + "&ono=" + Server.UrlEncode(SecurityComponent.Encrypt(drOrderNums["OrderNumber"].ToString())) + "\"><img src=\"/images/store_credit.gif\" /></a>&nbsp;");
                                                    Table.Append("<a href=\"ReturnPackageSlip.aspx?prodid=" + Convert.ToString(DsCartDetail.Tables[0].Rows[i]["RefProductID"].ToString()) + "&ono=" + Server.UrlEncode(SecurityComponent.Encrypt(drOrderNums["OrderNumber"].ToString())) + "&StoreCredit=1&Qty=" + DsCartDetail.Tables[0].Rows[i]["Quantity"].ToString() + "\"><img src=\"/images/store_credit.gif\" /></a>&nbsp;");
                                                }
                                                else
                                                {
                                                    Table.Append("<a style='font-weight:bold;' href=\"mailto:" + AppLogic.AppConfigs("ContactMail_ToAddress").ToString() + "\"><img src=\"/images/send_e_Mail_for_request.png\" border=\"0\" /></a>&nbsp;");
                                                }
                                                Table.Append("</td>");
                                                Table.Append("</tr>");
                                            }
                                        }
                                    }
                                }
                                // End right side block
                                Table.Append("</table>");
                                Table.Append("</td>");
                                Table.Append("</tr>");
                                Table.Append("</table>");
                            }
                            else
                            {
                                Table.Append("<span style='text-align: center; color: red; padding-left: 50%;'>No Record(s) Found!</span></tr>");
                                Table.Append("</table>");
                            }

                            lblTable.Text = Table.ToString();
                        }
                    }

                }
            }
            else
            {
                lblMsg.Text = "Sorry, Orders have been not Shipped yet.";
                paging.Visible = false;
            }
        }

        /// <summary>
        /// Get Micro Images
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns String</returns>
        public String GetMicroImage(String img)
        {
            clsvariables.LoadAllPath();
            String[] AllowedExtensions = AppLogic.AppConfigs("AllowedExtensions").Split(',');
            String imagepath = String.Empty;
            Random rd = new Random();
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Micro/" + img;
            if (img != "")
            {
                if (File.Exists(Server.MapPath(imagepath)))
                {
                    return imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
        }

        /// <summary>
        /// Bind Variant for product
        /// </summary>
        /// <param name="VarName">String VarName</param>
        /// <param name="VarValue">String VarValue</param>
        /// <returns>Returns Literal</returns>
        public System.Web.UI.WebControls.Literal BindVariantForProduct(String VarName, String VarValue)
        {
            string[] varname = VarName.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            string[] varvalue = VarValue.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            StringBuilder sbvartable = new StringBuilder();
            ltrvartable = new System.Web.UI.WebControls.Literal();
            if (varname.Length > 0)
            {
                for (int i = 0; i < varname.Length; i++)
                {
                    sbvartable.AppendLine("" + varname[i].ToString() + " : " + varvalue[i].ToString() + "<br />");
                }
            }
            if (sbvartable.ToString() != "")
            {
                ltrvartable.Text = sbvartable.ToString();
            }
            else
            {
                ltrvartable.Text = "";
            }
            return ltrvartable;
        }

        /// <summary>
        /// Bind Paging with Next and Previous link in SubCategory 's Products and Brand 's Products
        /// </summary>
        /// <param name="PageNumber">int PageNumber</param>
        /// <param name="NumberOfPages">int NumberOfPages</param>
        /// <returns>Returns Paging With Next and Previous link</returns>
        private String BindPageNumbers(int PageNumber, int NumberOfPages)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            String bre = "ViewRecentOrders";
            string pageurl = "/" + bre + "*";
            if (PageNumber > NumberOfPages)
                PageNumber = NumberOfPages;

            int CurrentOffset = (int)(PageNumber - 0.51) / 3;
            int FinalOffset = (int)(NumberOfPages - 0.51) / 3;



            if (viewAll == true)
                sb.AppendLine(" <span class='active'>Pages</span> ");
            else
                sb.AppendLine(" <a  href=\"" + pageurl + "All\">All Pages</a>");

            // sb.AppendLine("<");
            if (PageNumber > 1)
            {
                sb.AppendLine("<a  href=\"" + pageurl + (PageNumber - 1) + "\"> Previous</a>... ");
            }

            sb.Append(" [ ");

            if (viewAll == true)
            {
                for (int i = ((CurrentOffset * 3) + 1); i <= ((CurrentOffset + 1) * 3); i++)
                {
                    if (i > NumberOfPages)
                        break;
                    sb.AppendLine("<a href=\"" + pageurl + i + "\">" + i + "</a>" + " | ");
                }
            }
            else
            {
                for (int i = ((CurrentOffset * 3) + 1); i <= ((CurrentOffset + 1) * 3); i++)
                {
                    if (i > NumberOfPages)
                        break;
                    if (i != PageNumber)
                    {
                        sb.AppendLine("<a  href=\"" + pageurl + i + "\">" + i + "</a>" + " | ");
                    }
                    else
                    {
                        sb.AppendLine("<a class='active'><span>" + " " + i + "</span></a>" + " | ");
                    }
                }
            }
            if (PageNumber < NumberOfPages)
            {
                sb.AppendLine(" ...<a  href=\"" + pageurl + (PageNumber + 1) + "\">Next  </a> ");
            }
            return sb.ToString();
        }
    }
}