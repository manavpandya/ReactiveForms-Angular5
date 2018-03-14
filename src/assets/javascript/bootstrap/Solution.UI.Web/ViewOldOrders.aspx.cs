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

namespace Solution.UI.Web
{
    public partial class viewoldorders : System.Web.UI.Page
    {
        #region local variables

        System.Web.UI.WebControls.Literal ltrvartable = null;

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonOperations.RedirectWithSSL(true);
            ltbrTitle.Text = "View Old Order";
            ltTitle.Text = "View Old Order";
            Session["PageOlderOrder"] = "true";
            if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
            }
            else
            {
                Response.Redirect("/login.aspx", true);
            }

            if (!IsPostBack)
            {
                if (Session["CustID"] != null)
                {
                    GetViewOldOrderDetails();
                }
                else
                {
                    Response.Redirect("/");
                }
            }
        }

        /// <summary>
        /// View Old Order Details
        /// </summary>
        private void GetViewOldOrderDetails()
        {
            //for Paging
            OrderComponent objOrder = new OrderComponent();
            DataSet DsOrderPaging = new DataSet();
            DsOrderPaging = objOrder.GetViewOrderPaging(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            // end for Paging 

            DataSet DsOrder = new DataSet();
            DsOrder = objOrder.GetViewOldOrderNobyCustomerID(Convert.ToInt32(Session["CustID"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (DsOrder != null && DsOrder.Tables.Count > 0 && DsOrder.Tables[0].Rows.Count > 0)
            {
                StringBuilder Table = new StringBuilder();
                DataRow[] dr = DsOrder.Tables[0].Select("isnull(IsNew,0) = 0 AND isnull(shippedon,'') <> '' AND isnull(shippedvia,'') <> '' AND isnull(shippingTrackingNumber,'') <> ''");
                if (dr != null && dr.Length > 0)
                {
                    foreach (DataRow drOrderNums in dr)
                    {
                        DataSet dsOrder = new DataSet();
                        dsOrder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(drOrderNums["OrderNumber"].ToString()));

                        Table.Append("<table width='100%' style='float:left;' align='left' border='0' cellpadding='0' cellspacing='0' class='table-none' >");
                        Table.Append("<tr>");
                        Table.Append("<td valign='top' style=\"border-right: 1px solid #DFDFDF;width:30%;align:left;\">");
                        Table.Append("<table width='100%' border='0'  cellpadding='0' cellspacing='0' valign='top' >");
                        Table.Append("<tr>");
                        Table.Append("<td class='tabl' valign='top' align='left'><span id='ctl00_ContentPlaceHolder1_lblTable2'>Order Date");
                        Table.Append("<br/><strong>" + Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["OrderDate"].ToString()).ToLongDateString() + "</strong><br />");
                        Table.Append("<br/><br/><a href=\"/orderdetails.aspx?ono=" + Convert.ToString(drOrderNums["OrderNumber"].ToString()) + "\">View Order Details</a> | <a href='invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(Convert.ToString(drOrderNums["OrderNumber"].ToString()))) + "' target=\"_blank\"\">View Invoice</a>  ");
                        Table.Append("</td>");
                        Table.Append("</tr>");
                        Table.Append("<tr align='left'>");
                        Table.Append("<td>Order Number: " + Convert.ToString(drOrderNums["OrderNumber"].ToString()) + "");
                        Table.Append("</td>");
                        Table.Append("</tr>");
                        Table.Append("<tr align='left'>");
                        Table.Append("<td>Shipping Method: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString()) + "");
                        Table.Append("</td>");
                        Table.Append("</tr>");
                        Table.Append("<tr align='left'>");
                        Table.Append("<td>Recipient: " + Convert.ToString(dsOrder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsOrder.Tables[0].Rows[0]["LastName"].ToString()) + "");
                        Table.Append("</td>");
                        Table.Append("</tr>");
                        Table.Append("<tr align='left'>");
                        Table.Append("<td>Order Total: $" + Convert.ToString(Math.Round(Convert.ToDecimal(dsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()), 2)) + "");
                        Table.Append("</td>");
                        Table.Append("</tr>");

                        if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString()))
                        {
                            Table.Append("<tr>");
                            Table.Append("<td>Order Status: " + dsOrder.Tables[0].Rows[0]["OrderStatus"].ToString() + "");
                            Table.Append("</td>");
                            Table.Append("</tr>");
                        }

                        Table.Append("<tr align='left'>");
                        Table.Append("<td>");
                        //Table.Append("<a href=\"javascript:void(0);\" onclick=\"reorder(" + drOrderNums["OrderNumber"].ToString() + ");\"><img src=\"/images/reorder.png\" /></a>");
                        Table.Append("<table>");
                        Table.Append("<tr>");
                        Table.Append("<td><a href=\"javascript:void(0);\" onclick=\"reorder(" + drOrderNums["OrderNumber"].ToString() + ");\"><img src=\"/images/reorder.png\" /></a></td>");
                        Table.Append("<td><a href=\"javascript:void(0);\" onclick=\"window.location.href='/ViewRMARecentOrders.aspx?Ono=" + Server.UrlEncode(SecurityComponent.Encrypt(drOrderNums["OrderNumber"].ToString())) + "&type=1'\"><img src=\"/images/return_item.png\" /></a></td>");
                        Table.Append("</tr>");
                        Table.Append("</table>");
                        Table.Append("</td>");
                        Table.Append("</tr>");
                        Table.Append("</table>");

                        Table.Append("</td>");
                        Table.Append("<td valign='top'>");

                        Table.Append("<table width='100%' align='left' cellpadding='0' cellspacing='0'>");
                        String ProductID = String.Empty;
                        String ODate = String.Empty;

                        DataSet DsCartDetail = new DataSet();
                        DsCartDetail = objOrder.GetCartItemForViewOldOrder(Convert.ToInt32(dsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), Convert.ToInt32(dsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                        for (Int32 i = 0; i < dsOrder.Tables[0].Rows.Count; i++)
                        {

                            if (dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString() != null)
                            {
                                Table.Append("<tr>");
                                if (dsOrder.Tables[0].Rows[0]["ShippedOn"].ToString() != "Null")
                                {
                                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString()) && !String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippedvia"].ToString()) && !String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippedon"].ToString()))
                                    {
                                        Table.Append("<strong style='font-size:11px;color:#000;'>Shipped</strong><br/>");
                                    }

                                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippedOn"].ToString()))
                                    {
                                        Table.Append("Shipped On: <b> " + Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["ShippedOn"].ToString()).ToLongDateString() + "</b> <br/>");
                                    }

                                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString()))
                                    {
                                        Table.Append("Tracking Number: <b> " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString()) + "</b> <br/>");
                                    }
                                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippedvia"].ToString()))
                                    {
                                        Table.Append("Shipped Via: <b> " + Convert.ToString(dsOrder.Tables[0].Rows[0]["shippedvia"].ToString()) + "</b> <br/>");
                                    }
                                }
                                else
                                {
                                    DataSet dtParsalShipping1 = new DataSet();
                                    dtParsalShipping1 = objOrder.GetParsalShippinginviewOldOrder(Convert.ToInt32(DsCartDetail.Tables[0].Rows[0]["ProductID"].ToString()), Convert.ToInt32(DsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                                    if (dtParsalShipping1 != null && dtParsalShipping1.Tables.Count > 0 && dtParsalShipping1.Tables[0].Rows.Count > 0)
                                    {
                                        Table.Append("<strong style='font-size:11px;color:#000;'>Shipped</strong><br/>");
                                        Table.Append("Shipped On: <b> " + Convert.ToDateTime(dtParsalShipping1.Tables[0].Rows[0]["ShippedOn"].ToString()).ToLongDateString() + "</b> <br/>");
                                        if (!String.IsNullOrEmpty(dtParsalShipping1.Tables[0].Rows[0]["TrackingNumber"].ToString()))
                                        {
                                            Table.Append("Tracking Number: <b> " + dtParsalShipping1.Tables[0].Rows[0]["TrackingNumber"].ToString() + "</b> <br/>");
                                        }
                                        if (!String.IsNullOrEmpty(dtParsalShipping1.Tables[0].Rows[0]["ShippedVia"].ToString()))
                                        {
                                            Table.Append("Shipped Via: <b> " + dtParsalShipping1.Tables[0].Rows[0]["ShippedVia"].ToString() + "</b> <br/>");
                                        }
                                    }
                                }
                                Table.Append("</td>");
                                Table.Append("</tr>");

                                if (DsCartDetail != null && DsCartDetail.Tables.Count > 0 && DsCartDetail.Tables[0].Rows.Count > 0)
                                {
                                    for (Int32 j = 0; j < DsCartDetail.Tables[0].Rows.Count; j++)
                                    {
                                        Table.Append("<tr>");
                                        if ((DsCartDetail.Tables[0].Rows[j]["MainCategory"].ToString() == null) || (DsCartDetail.Tables[0].Rows[j]["MainCategory"].ToString() == ""))
                                        {
                                            if (DsCartDetail.Tables[0].Rows[j]["VariantNames"].ToString() != "" && DsCartDetail.Tables[0].Rows[j]["VariantValues"].ToString() != "")
                                            {
                                                BindVariantForProduct(DsCartDetail.Tables[0].Rows[j]["VariantNames"].ToString(), DsCartDetail.Tables[0].Rows[j]["VariantValues"].ToString());
                                            }
                                            else
                                            {
                                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                                ltrvartable.Text = string.Empty;
                                            }
                                            //Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["RefProductID"].ToString()) + ".aspx'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\"></a>");
                                            if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                                            {
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\">");
                                                Table.Append("</td>");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ProductURL"].ToString()) + "'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                            }
                                            //Table.Append("<td align='left' width='83%' valign='top'><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "</a></a><br />" + ltrvartable.Text + "");
                                            if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                                            {
                                                Table.Append("<td align='left' width='83%' valign='top'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "<br />" + ltrvartable.Text + "");
                                                Table.Append("</td>");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='83%' valign='top'><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ProductURL"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                                Table.Append("</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[j]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[j]["VariantValues"].ToString()))
                                            {
                                                BindVariantForProduct(Convert.ToString(DsCartDetail.Tables[0].Rows[j]["VariantNames"].ToString()), Convert.ToString(DsCartDetail.Tables[0].Rows[j]["VariantValues"].ToString()));
                                            }
                                            else
                                            {
                                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                                ltrvartable.Text = string.Empty;
                                            }
                                            //Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["MainCategory"].ToString()) + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["RefProductID"].ToString()) + ".aspx'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\"></a>");
                                            if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                                            {
                                                Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\">");
                                                Table.Append("</td>");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ProductURL"].ToString()) + "'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                            }
                                            //Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["MainCategory"].ToString()) + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                            if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[i]["RelatedproductID"].ToString()) > 0)
                                            {
                                                Table.Append("<td align='left'  width='83%' valign='top' >" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "<br />" + ltrvartable.Text + "");
                                                Table.Append("</td>");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["ProductURL"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[j]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                                Table.Append("</td>");
                                            }

                                        }
                                        Table.Append("</tr>");
                                    }
                                }
                                Table.Append("<tr>");
                            }
                            else
                            {
                                Table.Append("<tr>");
                                Table.Append("<td colspan='3' align='left' style='border-bottom:1px solid #DFDFDF'>");

                                if (dsOrder.Tables[0].Rows[0]["ShippedOn"].ToString() != null)
                                {
                                    Table.Append("<strong style='font-size:11px;color:#000;'>Shipped</strong><br/>");
                                    Table.Append("Shipped On: <b> " + Convert.ToDateTime(dsOrder.Tables[0].Rows[0]["ShippedOn"].ToString()).ToLongDateString() + "</b> <br/>");
                                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString()))
                                    {
                                        Table.Append("Tracking Number: <b> " + Convert.ToString(dsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString()) + "</b> <br/>");
                                    }
                                    if (!String.IsNullOrEmpty(dsOrder.Tables[0].Rows[0]["shippedvia"].ToString()))
                                    {
                                        Table.Append("Shipped Via: <b> " + Convert.ToString(dsOrder.Tables[0].Rows[0]["shippedvia"].ToString()) + "</b> <br/>");
                                    }
                                }
                                else
                                {
                                    DataSet dtParsalShipping = new DataSet();
                                    dtParsalShipping = objOrder.GetParsalShippinginviewOldOrder(Convert.ToInt32(DsCartDetail.Tables[0].Rows[0]["ProductID"].ToString()), Convert.ToInt32(DsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                                    if (dtParsalShipping != null && dtParsalShipping.Tables.Count > 0 && dtParsalShipping.Tables[0].Rows.Count > 0)
                                    {
                                        Table.Append("<strong style='font-size:11px;color:#000;'>Shipped</strong><br/>");
                                        Table.Append("Shipped On: <b> " + Convert.ToDateTime(dtParsalShipping.Tables[0].Rows[0]["ShippedOn"].ToString()).ToLongDateString() + "</b> <br/>");
                                        if (!String.IsNullOrEmpty(dtParsalShipping.Tables[0].Rows[0]["TrackingNumber"].ToString()))
                                        {
                                            Table.Append("Tracking Number: <b> " + dtParsalShipping.Tables[0].Rows[0]["TrackingNumber"].ToString() + "</b> <br/>");
                                        }
                                        if (!String.IsNullOrEmpty(dtParsalShipping.Tables[0].Rows[0]["ShippedVia"].ToString()))
                                        {
                                            Table.Append("Shipped Via: <b> " + dtParsalShipping.Tables[0].Rows[0]["ShippedVia"].ToString() + "</b> <br/>");
                                        }
                                    }
                                    else
                                    {
                                        Table.Append("<strong>Not Shipped </strong><br/>");
                                    }
                                }

                                Table.Append("</td>");
                                Table.Append("</tr>");

                                if (DsCartDetail != null && DsCartDetail.Tables.Count > 0 && DsCartDetail.Tables[0].Rows.Count > 0)
                                {
                                    for (Int32 k = 0; k < DsCartDetail.Tables[0].Rows.Count; k++)
                                    {
                                        Table.Append("<tr>");
                                        if ((DsCartDetail.Tables[0].Rows[k]["MainCategory"].ToString() == null) || (DsCartDetail.Tables[0].Rows[k]["MainCategory"].ToString() == ""))
                                        {
                                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[k]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[k]["VariantValues"].ToString()))
                                            {
                                                BindVariantForProduct(DsCartDetail.Tables[0].Rows[k]["VariantNames"].ToString(), DsCartDetail.Tables[0].Rows[k]["VariantValues"].ToString());
                                            }
                                            else
                                            {
                                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                                ltrvartable.Text = string.Empty;
                                            }
                                            //Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["RefProductID"].ToString()) + ".aspx'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\"></a>");
                                            if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[k]["RelatedproductID"].ToString()) > 0)
                                            {
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\">");
                                                Table.Append("</td>");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='10%' style='vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ProductURL"].ToString()) + "'><img width='110pxc' style='border-width:0px;'  Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                            }
                                           // Table.Append("<td align='left' width='83%' valign='top'><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "</a></a><br />" + ltrvartable.Text + "");
                                            if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[k]["RelatedproductID"].ToString()) > 0)
                                            {
                                                Table.Append("<td align='left' width='83%' valign='top'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "<br />" + ltrvartable.Text + "");
                                                Table.Append("</td>");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='83%' valign='top'><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ProductURL"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "</a></a><br />" + ltrvartable.Text + "");
                                                Table.Append("</td>");
                                            }
                                        }
                                        else
                                        {
                                            if (!string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[k]["VariantNames"].ToString()) && !string.IsNullOrEmpty(DsCartDetail.Tables[0].Rows[k]["VariantValues"].ToString()))
                                            {
                                                BindVariantForProduct(Convert.ToString(DsCartDetail.Tables[0].Rows[k]["VariantNames"].ToString()), Convert.ToString(DsCartDetail.Tables[0].Rows[k]["VariantValues"].ToString()));
                                            }
                                            else
                                            {
                                                ltrvartable = new System.Web.UI.WebControls.Literal();
                                                ltrvartable.Text = string.Empty;
                                            }
                                           // Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["MainCategory"].ToString()) + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["RefProductID"].ToString()) + ".aspx'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\"></a>");
                                            if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[k]["RelatedproductID"].ToString()) > 0)
                                            {
                                                Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\">");
                                                Table.Append("</td>");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left' width='10%' style='border-right:0px;vAlign:top;' ><a href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ProductURL"].ToString()) + "'><img  style='border-width:0px;' Title=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\" src=\"" + GetMicroImage(Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ImageName"].ToString())) + "\" alt=\"" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "\"></a>");
                                                Table.Append("</td>");
                                            }
                                           // Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["MainCategory"].ToString()) + "/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["SEName"].ToString()) + "-" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["RefProductID"].ToString()) + ".aspx'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                            if (Convert.ToInt32(DsCartDetail.Tables[0].Rows[k]["RelatedproductID"].ToString()) > 0)
                                            {
                                                Table.Append("<td align='left'  width='83%' valign='top' >" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "<br />" + ltrvartable.Text + "");
                                                Table.Append("</td>");
                                            }
                                            else
                                            {
                                                Table.Append("<td align='left'  width='83%' valign='top' ><a  href='/" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["ProductURL"].ToString()) + "'>" + Convert.ToString(DsCartDetail.Tables[0].Rows[k]["Name"].ToString()) + "</a><br />" + ltrvartable.Text + "");
                                                Table.Append("</td>");
                                            }
                                        }
                                        Table.Append("</tr>");
                                    }
                                }
                            }
                            Table.Append("</table>");
                            Table.Append("</td>");
                            Table.Append("</tr>");
                            Table.Append("</table>");
                        }
                    }
                    lblTable.Text = Table.ToString();
                }
                else
                    lblMsg.Text = "Order does not exist for this Customer.";

            }
            else
                lblMsg.Text = "Order does not exist for this Customer.";
        }

        /// <summary>
        /// Gets the Micro Image
        /// </summary>
        /// <param name="img">String img</param>
        /// <returns>Returns the Micro Image Path</returns>
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
        /// Bind Variant for Product
        /// </summary>
        /// <param name="VarName">String VarName</param>
        /// <param name="VarValue">String VarValue</param>
        /// <returns>Returns Variant Details for Product as a Literal Control</returns>
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
        ///  Re Order Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnReorder_Click(object sender, ImageClickEventArgs e)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet DsOrder = new DataSet();
            string strResult = "";
            ShoppingCartComponent objShopping = new ShoppingCartComponent();

            DsOrder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(hdnortder.Value.ToString()));

            DataSet dsOrdercompletedetail = new DataSet();
            dsOrdercompletedetail = objOrder.GetCartItemForViewOldOrder(Convert.ToInt32(DsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), Convert.ToInt32(hdnortder.Value.ToString()), Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            bool checkProduct = false;
            string Productmesage = "";
            if (dsOrdercompletedetail != null && dsOrdercompletedetail.Tables.Count > 0 && dsOrdercompletedetail.Tables[0].Rows.Count > 0)
            {
                for (Int32 i = 0; i < dsOrdercompletedetail.Tables[0].Rows.Count; i++)
                {
                    string VariantNamesId = "";
                    string VariantvaluesId = "";
                    string[] strvariantNames = dsOrdercompletedetail.Tables[0].Rows[i]["VariantNames"].ToString().Replace("'", "''").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] strvariantValues = dsOrdercompletedetail.Tables[0].Rows[i]["VariantValues"].ToString().Replace("'", "''").Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    for (int iVar = 0; iVar < strvariantNames.Length; iVar++)
                    {
                        if (strvariantValues.Length > iVar)
                        {
                            String VariantNameId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VariantId FROM tb_ProductVariant WHERE Productid=" + dsOrdercompletedetail.Tables[0].Rows[i]["ProductId"].ToString() + " AND VariantName='" + strvariantNames[iVar].ToString().Replace("'", "''") + "'"));
                            VariantNamesId += VariantNameId.ToString() + ",";
                            String VariantValueId = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT VariantValueID FROM tb_ProductVariantValue WHERE VariantID=" + VariantNameId + " AND VariantValue ='" + strvariantValues[iVar].ToString().Replace("'", "''") + "' AND Productid=" + dsOrdercompletedetail.Tables[0].Rows[i]["ProductId"].ToString() + ""));
                            VariantvaluesId += VariantValueId.ToString() + ",";
                        }
                    }

                    strResult = Convert.ToString(objShopping.AddItemIntoCartReorder(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(dsOrdercompletedetail.Tables[0].Rows[i]["ProductID"].ToString()), Convert.ToInt32(dsOrdercompletedetail.Tables[0].Rows[i]["Quantity"].ToString()), Convert.ToDecimal(dsOrdercompletedetail.Tables[0].Rows[i]["SalePrice"].ToString()), VariantNamesId, VariantvaluesId, "", ""));
                    if (strResult.ToLower() != "success")
                    {
                        checkProduct = true;
                        Productmesage += dsOrdercompletedetail.Tables[0].Rows[i]["Name"].ToString() + @": \n" + strResult.ToString() + @" \n";
                    }
                }

                if (!checkProduct)
                {
                    if (Session["NoOfCartItems"] == null)
                    {
                        Session["NoOfCartItems"] = dsOrdercompletedetail.Tables[0].Rows[0]["Quantity"].ToString();
                    }
                    Response.Redirect("/addtoCart.aspx");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('" + Productmesage.Replace("'", @"\'") + "');", true);
                }
            }
        }
    }
}