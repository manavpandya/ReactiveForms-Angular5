using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.Common;
using System.Net.Mail;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text.RegularExpressions;
using System.Text;
using System.IO;
using System.Data;
namespace Solution.UI.Web.RMA.Amazon
{
    public partial class OrderDetails : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            CommonOperations.RedirectWithSSL(false);
            divback.HRef = Request.UrlReferrer.ToString();
            if (Session["CID"] != null)
            {
            }

            else
            {
                Response.Redirect("/login.aspx", true);
            }
            ltTitle.Text = "Order Details";
            Session["PageName"] = "Order Details";
            if (!IsPostBack)
            {
                if (Request.QueryString["ono"] != null && Request.QueryString["ono"].ToString().Length > 0)
                {

                    //if (Convert.ToString(Session["PageOlderOrder"]) == "true")
                    //    PrevPage.Text = "<a href='/ViewOldOrders.aspx'> View Older Orders</a>";
                    //else
                    //    PrevPage.Text = "<a href='/ViewRecentOrders.aspx'> View Recent And Open Orders</a>";

                    Binddata(Request.QueryString["ONo"].ToString().Trim());
                }
                else
                    Response.Redirect("/");
            }

        }


        private string GetConfigvalue(string configname, Int32 StoreId)
        {
            string strvalue = "";

            strvalue = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 isnull(ConfigVAlue,'') FROM tb_AppConfig WHERE Isnull(Deleted,0)=0 and Storeid=" + StoreId + " and ConfigName ='" + configname + "'"));

            return strvalue;
        }



        /// <summary>
        /// This Function Bind order data
        /// and Call method for display cart information ,Billing information based on order Number
        /// </summary>
        /// <param name="OrderNumber">String OrderNumber</param>
        protected void Binddata(string OrderNumber)
        {

            DataSet DsOrder = new DataSet();
            OrderComponent objOrder = new OrderComponent();
            DsOrder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(Request.QueryString["ono"].ToString()));
            if (DsOrder != null && DsOrder.Tables.Count > 0 && DsOrder.Tables[0].Rows.Count > 0)
            {
                Int32 TempStoreID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select StoreID from tb_Order where orderNumber=" + Convert.ToInt32(Request.QueryString["ono"].ToString()) + ""));
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.Append("<table style='color:#464646;float:left;width:100%'><tr><td align='left' width='80%'><b>Order Placed:</b> " + Convert.ToDateTime(DsOrder.Tables[0].Rows[0]["OrderDate"].ToString()).ToLongDateString());
                sb.Append("</td><tr><td colspan='2' align='left'> <b>" + GetConfigvalue("STORENAME",TempStoreID).ToString() + " Order Number:</b> " + Request.QueryString["ono"].ToString()); //<td align='right' width='20%'><a href='javascript:void(0);' onclick='javascript:window.history.back();' style='color:#000;font-weight:bold;text-decoration:underline;'>BACK</a></td>
                sb.Append("</td><tr><td colspan='2' align='left'> <b>Order Total:</b> $" + Math.Round(Convert.ToDecimal(DsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()), 2) + "</td></tr></table>");


                sb.Append("<br/><br/>");

                sb.Append(AddCartItem(Request.QueryString["ono"].ToString()));

                sb.Append("<table width='100%' cellspacing='0' cellpadding='0' border='0' class='table_none' style='margin-bottom:5px;'>");
                sb.Append("<tr><th colspan='2' style='text-align:left; background-color:#E8E8E8;'>Payment Information <a href='Invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(Convert.ToString(Request.QueryString["ono"].ToString()))) + "' style='color:#464646;float:right;'>Need to print an invoice?</a></th></tr>");
                sb.Append("<tr>");
                sb.Append("<td valign='top'>");
                sb.Append("<b>Payment Method: " + Convert.ToString(DsOrder.Tables[0].Rows[0]["PaymentMethod"].ToString()) + "</b><br/>");
                if (!string.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["CardNumber"].ToString()))
                {
                    string CardNumber = SecurityComponent.Decrypt(DsOrder.Tables[0].Rows[0]["CardNumber"].ToString());
                    string cardLast4Digit = "";
                    if (CardNumber.Length > 4)
                    {

                        for (int i = 0; i < CardNumber.Length - 4; i++)
                        {
                            cardLast4Digit += "*";
                        }

                        cardLast4Digit += "*" + CardNumber.ToString().Substring(CardNumber.Length - 4);

                    }
                    else
                    {
                        cardLast4Digit = "";
                    }

                    if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["Last4"].ToString()))
                    {
                        sb.Append(Convert.ToString(DsOrder.Tables[0].Rows[0]["CardType"].ToString()) + " | Last Four Digits:&nbsp;&nbsp;" + Convert.ToString(DsOrder.Tables[0].Rows[0]["Last4"].ToString()));
                        sb.Append("<br/><br/>");
                    }
                    else
                    {

                        sb.Append(Convert.ToString(DsOrder.Tables[0].Rows[0]["CardType"].ToString()) + " | Last Four Digits:&nbsp;&nbsp;" + cardLast4Digit);
                        sb.Append("<br/><br/>");
                    }
                }
                sb.Append("<br/><br/>");

                sb.Append("<b>Billing Address:</b><br/>");
                sb.Append(Convert.ToString(DsOrder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(DsOrder.Tables[0].Rows[0]["BillingLastName"].ToString()) + "<br/>");



                sb.Append(Convert.ToString(DsOrder.Tables[0].Rows[0]["BillingAddress1"].ToString()) + "<br />");

                if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                    sb.Append(Convert.ToString(DsOrder.Tables[0].Rows[0]["BillingAddress2"].ToString()) + "<br />");

                if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                    sb.Append(DsOrder.Tables[0].Rows[0]["BillingSuite"].ToString() + "<br />");

                sb.Append(DsOrder.Tables[0].Rows[0]["BillingCity"].ToString() + "<br />");
                sb.Append(DsOrder.Tables[0].Rows[0]["BillingState"].ToString() + "<br />");
                sb.Append(DsOrder.Tables[0].Rows[0]["BillingZip"].ToString() + "<br />");


                sb.Append(DsOrder.Tables[0].Rows[0]["BillingCountry"].ToString() + "<br / >");
                sb.Append(DsOrder.Tables[0].Rows[0]["BillingPhone"].ToString() + "<br />");
                sb.Append("</td>");
                sb.Append("<td style='text-align:right;vertical-align:top;'>");
                sb.Append("<table width='100%' class='table'><tr><td>Item(s) SubTotal: </td><td>$" + String.Format("{0:0.00}", Convert.ToDecimal(DsOrder.Tables[0].Rows[0]["OrderSubtotal"].ToString())) + "</td></tr>");


                sb.Append("<tr><td>Shipping & Handling: </td><td>$" + Math.Round(Convert.ToDecimal(DsOrder.Tables[0].Rows[0]["OrderShippingCosts"].ToString()), 2) + "</td></tr>");

                Decimal Discount = 0;
                Decimal QuantityDiscount = Math.Round(Convert.ToDecimal((DsOrder.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString())), 2);

                if (QuantityDiscount != 0)
                {
                    Discount += QuantityDiscount;
                    sb.Append("<tr><td>Quantity Discount: </td><td>$" + QuantityDiscount + "</td></tr>");
                }

                Decimal CustomerLevelDiscount = Math.Round(Convert.ToDecimal((DsOrder.Tables[0].Rows[0]["LevelDiscountAmount"].ToString())), 2);
                if (CustomerLevelDiscount != 0)
                {
                    Discount += CustomerLevelDiscount;
                    sb.Append("<tr><td>Customer Level Discount: </td><td>$" + CustomerLevelDiscount + "</td></tr>");
                }
                Decimal CouponDiscount = Math.Round(Convert.ToDecimal((DsOrder.Tables[0].Rows[0]["CouponDiscountAmount"].ToString())), 2);
                if (CouponDiscount != 0)
                {
                    Discount += CouponDiscount;
                    sb.Append("<tr><td>Coupon Discount: </td><td>$" + CouponDiscount + "</td></tr>");
                }

                Decimal CustomDiscount = Math.Round(Convert.ToDecimal((DsOrder.Tables[0].Rows[0]["CustomDiscount"].ToString())), 2);
                if (CustomDiscount != 0)
                {
                    Discount += CustomDiscount;
                    sb.Append("<tr><td>Custom Discount: </td><td>$" + CustomDiscount + "</td></tr>");
                }

                Decimal GiftCertificateDiscountAmt = Math.Round(Convert.ToDecimal((DsOrder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString())), 2);
                if (GiftCertificateDiscountAmt != 0)
                {
                    Discount += GiftCertificateDiscountAmt;
                    sb.Append("<tr><td>Gift Certificate Discount: </td><td>$" + GiftCertificateDiscountAmt + "</td></tr>");
                }
                sb.Append("<tr><td colspan='2' align='right'>--------</td></tr>");

                decimal totalbeforetax = (Convert.ToDecimal(DsOrder.Tables[0].Rows[0]["OrderSubtotal"]) + Convert.ToDecimal(DsOrder.Tables[0].Rows[0]["OrderShippingCosts"])) - Discount;

                if (totalbeforetax < 0)
                {
                    totalbeforetax = 0;
                }

                sb.Append("<tr><td>Total before Tax: </td><td>$" + totalbeforetax.ToString("F2") + "</td></tr>");

                sb.Append("<tr><td>Order Tax: </td><td>$" + Math.Round(Convert.ToDecimal(DsOrder.Tables[0].Rows[0]["OrderTax"].ToString()), 2) + "</td></tr>");

                sb.Append("<tr><td colspan='2' align='right'>--------</td></tr>");

                sb.Append("<tr><td>Grand Total: </td><td>$" + Math.Round(Convert.ToDecimal(DsOrder.Tables[0].Rows[0]["OrderTotal"].ToString()), 2) + "</td></tr></table>");

                sb.Append("</td>");
                sb.Append("</tr>");
                sb.Append("</table>");

                lblTable.Text = sb.ToString();
            }

        }

        /// <summary>
        ///Bind Cart Details By Order Number
        /// </summary>
        /// <param name="OrderNo">String OrderNo</param>
        /// <returns>Returns the output value as a string format which contains HTML</returns>
        public String AddCartItem(String OrderNo)
        {
            DataSet DsOrder = new DataSet();
            OrderComponent objOrder = new OrderComponent();
            DsOrder = objOrder.GetOrderDetailsByOrderID(Convert.ToInt32(Request.QueryString["ono"].ToString()));
            StringBuilder Table = new StringBuilder();
            if (DsOrder != null && DsOrder.Tables.Count > 0 && DsOrder.Tables[0].Rows.Count > 0)
            {
                Int32 TempStoreID = Convert.ToInt32(CommonComponent.GetScalarCommonData("select StoreID from tb_Order where orderNumber=" + Convert.ToInt32(Request.QueryString["ono"].ToString()) + ""));
                DataSet DsCartDetail = new DataSet();
                DsCartDetail = objOrder.GetCartItemForViewOldOrder(Convert.ToInt32(DsOrder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), Convert.ToInt32(DsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(GetConfigvalue("StoreID",TempStoreID)));




                for (Int32 i = 0; i < DsOrder.Tables[0].Rows.Count; i++)
                {

                    if (DsOrder.Tables[0].Rows[0]["ShippingTrackingNumber"].ToString() != null)
                    {



                        Table.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0' class='table_none'>");

                        if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippedOn"].ToString()))
                            Table.Append("<tr><th style='text-align:left;'  colspan='2'>Shipment #" + (i + 1) + ":  Shipped on " + Convert.ToDateTime(DsOrder.Tables[0].Rows[0]["ShippedOn"].ToString()).ToLongDateString() + " </th></tr>");
                        else
                        {

                            DataSet dtParsalShipping1 = new DataSet();
                            dtParsalShipping1 = objOrder.GetParsalShippinginviewOldOrder(Convert.ToInt32(DsCartDetail.Tables[0].Rows[0]["ProductID"].ToString()), Convert.ToInt32(DsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(GetConfigvalue("StoreID",TempStoreID)));

                            if (dtParsalShipping1 != null && dtParsalShipping1.Tables.Count > 0 && dtParsalShipping1.Tables[0].Rows.Count > 0)
                            {
                                Table.Append("<tr><th style='text-align:left;background-color:#E8E8E8; '  colspan='2'>Shipped  </th></tr>");
                            }
                            else
                                Table.Append("<tr><th style='text-align:left;background-color:#E8E8E8;' colspan='2'>Not Shipped </th></tr>");

                        }
                        Table.Append("<tr>");
                        Table.Append("<td valign='top' style=\"width:20%;\"><b>Shipping Address:</b><br/>");
                        Table.Append(DsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + DsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString() + "<br/>");

                        if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br />");


                        Table.Append(DsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br />");

                        if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br />");

                        if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br />");

                        Table.Append(DsOrder.Tables[0].Rows[0]["ShippingCity"].ToString() + "<br /> ");
                        Table.Append(DsOrder.Tables[0].Rows[0]["ShippingState"].ToString() + "<br />");
                        Table.Append(DsOrder.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />");


                        Table.Append(DsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString() + "<br />");
                        Table.Append(DsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString() + "<br /> ");

                        Table.Append("<br/><br/>");
                        Table.Append("<b>Shipping Method</b><br/>");
                        Table.Append(DsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString());
                        Table.Append("</td>");
                        Table.Append("<td valign='top'>");
                        Table.Append("<b>Items Ordered</b><br/>");


                        Table.Append(" <table border='0' cellpadding='0' cellspacing='0' class='table' width='100%'> ");
                        Table.Append("<tbody><tr >");
                        Table.Append("<th width='30%' align='left' valign='middle'  ><b>Product</b></th>");
                        Table.Append("<th width='20%' align='center' valign='middle' style='text-align:center;' ><b> SKU</b></th>");
                        Table.Append("<th width='20%' align='center' valign='middle' style='text-align:center;'><b>Price</b></th>");
                        Table.Append("<th width='10%'  align='center' valign='middle' style='text-align:center;' ><b>Quantity</b></th>");
                        Table.Append("<th  width='20%' style='text-align: center;'><b>Sub Total</b></th>");
                        Table.Append("</tr>");

                        for (Int32 j = 0; j < DsCartDetail.Tables[0].Rows.Count; j++)
                        {

                            Decimal NetPrice = Decimal.Zero;
                            Decimal SubTotal = Decimal.Zero;
                            NetPrice = Decimal.Zero;
                            NetPrice = Math.Round((Convert.ToDecimal(DsCartDetail.Tables[0].Rows[j]["SalePrice"].ToString()) * Convert.ToInt32(DsCartDetail.Tables[0].Rows[j]["Quantity"].ToString())), 2);
                            SubTotal += NetPrice;

                            Table.Append("<tr align='center'  valign='middle'>");
                            Table.Append("<tr >");
                            Table.Append("<td align='left' valign='top'>" + DsCartDetail.Tables[0].Rows[j]["Name"].ToString());
                            string[] Names = DsCartDetail.Tables[0].Rows[j]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] Values = DsCartDetail.Tables[0].Rows[j]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (Names.Length == Values.Length)
                            {
                                for (int iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                                {
                                    Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                                }
                            }
                            else
                            {
                                for (int iLoopValues = 0; iLoopValues < Values.Length; iLoopValues++)
                                {
                                    Table.Append("<br/>  - " + Values[iLoopValues]);
                                }
                            }
                            Table.Append("</td>");
                            Table.Append("<td  style='text-align : center;'>" + DsCartDetail.Tables[0].Rows[j]["SKU"].ToString() + "</td>");
                            Table.Append("<td style='text-align : right;'>$" + String.Format("{0:0.00}", Convert.ToDecimal(DsCartDetail.Tables[0].Rows[j]["SalePrice"].ToString())) + "</td>");
                            Table.Append("<td style='text-align : center;'>" + DsCartDetail.Tables[0].Rows[j]["Quantity"].ToString() + "</td>");
                            Table.Append("<td  style='text-align : right;'> $" + NetPrice.ToString() + "</td>");
                            Table.Append(" </tr>");
                        }
                        try
                        {
                            Table.Append("</tbody></table>");
                            Table.Append("</td>");
                            Table.Append("</tr></table>");
                            Table.Append("<br/>");

                        }
                        catch
                        {
                            Table.Append("</tbody></table>");
                            Table.Append("</td>");
                            Table.Append("</tr></table>");
                            Table.Append("<br/>");
                        }
                    }
                    else
                    {
                        if (i == 0)
                        {
                            Table.Append("<table width='100%' cellpadding='0' cellspacing='0' border='0' class='table'>");

                            if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippedOn"].ToString()))
                                Table.Append("<tr><th style='text-align:left;' colspan='2'>Shipment #1: Shipped on " + Convert.ToDateTime(DsOrder.Tables[0].Rows[0]["ShippedOn"].ToString()).ToLongDateString() + "</th></tr>");
                            else
                            {


                                DataSet dtParsalShipping1 = new DataSet();
                                dtParsalShipping1 = objOrder.GetParsalShippinginviewOldOrder(Convert.ToInt32(DsCartDetail.Tables[0].Rows[0]["ProductID"].ToString()), Convert.ToInt32(DsOrder.Tables[0].Rows[0]["OrderNumber"].ToString()), Convert.ToInt32(GetConfigvalue("StoreID",TempStoreID)));

                                if (dtParsalShipping1 != null && dtParsalShipping1.Tables.Count > 0 && dtParsalShipping1.Tables[0].Rows.Count > 0)
                                {
                                    Table.Append("<tr><th style='text-align:left;'  colspan='2'>Shipped  </th></tr>");
                                }
                                else
                                    Table.Append("<tr><th style='text-align:left;' colspan='2'>Not Shipped </th></tr>");

                            }
                            Table.Append("<tr>");
                            Table.Append("<td valign='top'><b>Shipping Address:</b><br/>");
                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingFirstName"].ToString() + " " + DsOrder.Tables[0].Rows[0]["ShippingLastName"].ToString() + "<br/>");

                            if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                                Table.Append(DsOrder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br />");


                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br />");

                            if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                                Table.Append(DsOrder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br />");

                            if (!String.IsNullOrEmpty(DsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                                Table.Append(DsOrder.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br />");

                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingCity"].ToString() + "<br /> ");
                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingState"].ToString() + "<br />");
                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />");


                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingCountry"].ToString() + "<br />");
                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingPhone"].ToString() + "<br /> ");

                            Table.Append("<br/><br/>");
                            Table.Append("<b>Shipping Method</b><br/>");
                            Table.Append(DsOrder.Tables[0].Rows[0]["ShippingMethod"].ToString());
                            Table.Append("</td>");
                            Table.Append("<td valign='top'>");
                            Table.Append("<b>Items Ordered</b><br/>");



                            Table.Append(" <table border='0' cellpadding='0' cellspacing='0' class='table' width='100%'> ");
                            Table.Append("<tbody><tr >");
                            Table.Append("<th width='30%' align='left' valign='middle'  ><b>Product</b></th>");
                            Table.Append("<th width='20%' align='center' valign='middle' style='text-align:center;' ><b> SKU</b></th>");
                            Table.Append("<th width='20%' align='center' valign='middle' style='text-align:center;'><b>Price</b></th>");
                            Table.Append("<th width='10%' align='center' valign='middle' style='text-align:center;' ><b>Quantity</b></th>");
                            Table.Append("<th width='20%' style='text-align: center;'><b>Sub Total</b></th>");
                            Table.Append("</tr>");
                        }

                        for (Int32 k = 0; k < DsCartDetail.Tables[0].Rows.Count; k++)
                        {
                            Decimal NetPrice = Decimal.Zero;
                            Decimal SubTotal = Decimal.Zero;
                            NetPrice = Decimal.Zero;
                            NetPrice = Math.Round(Convert.ToDecimal(DsCartDetail.Tables[0].Rows[k]["SalePrice"].ToString()) * Convert.ToDecimal(DsCartDetail.Tables[0].Rows[k]["Quantity"].ToString()), 2);
                            SubTotal += NetPrice;

                            Table.Append("<tr align='center'  valign='middle'>");
                            Table.Append("<tr >");
                            Table.Append("<td align='left' valign='top'>" + DsCartDetail.Tables[0].Rows[k]["Name"].ToString());
                            string[] Names = DsCartDetail.Tables[0].Rows[k]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] Values = DsCartDetail.Tables[0].Rows[k]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (Names.Length == Values.Length)
                            {
                                for (int iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                                {
                                    Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                                }
                            }
                            else
                            {
                                for (int iLoopValues = 0; iLoopValues < Values.Length; iLoopValues++)
                                {
                                    Table.Append("<br/>  - " + Values[iLoopValues]);
                                }
                            }
                            Table.Append("</td>");
                            Table.Append("<td  style='text-align : center;'>" + DsCartDetail.Tables[0].Rows[k]["SKU"].ToString() + "</td>");
                            Table.Append("<td style='text-align : right;'>$" + DsCartDetail.Tables[0].Rows[k]["SalePrice"].ToString() + "</td>");
                            Table.Append("<td style='text-align : center;'>" + DsCartDetail.Tables[0].Rows[k]["Quantity"].ToString() + "</td>");
                            Table.Append("<td  style='text-align : right;'> $" + NetPrice.ToString() + "</td>");
                            Table.Append(" </tr>");

                        }

                        Table.Append("</tbody></table>");
                        Table.Append("</td>");
                        Table.Append("</tr></table>");
                        Table.Append("<br/>");

                    }

                }
                Decimal SubTotalofOrder = Decimal.Zero;
                Decimal SubTotalAmount = Decimal.Zero;
                SubTotalAmount = Math.Round(SubTotalofOrder, 2);
            }
            else
            {
                Table.AppendLine("<font color='red' CLASS='font-red'>Your Shopping Cart is Empty.</font>");
            }

            return Table.ToString();
        }

    }
}