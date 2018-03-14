using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using System.Net;
using System.IO;
using System.Net.Mail;
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class AmazonPackingSlip : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ONo"] != null)
            {
                int OrderNumber = 0;
                Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                Int32 StoreID = 0;
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                AppConfig.StoreID = StoreID;
                ImgStoreLogo.Src = "/Images/Store_" + StoreID.ToString() + ".png";
            }
            if (!IsPostBack)
            {
                if (Request.QueryString["ONo"] != null)
                {
                    //BindRefNumberDetails();
                    int OrderNumber = 0;
                    bool chkOrder = Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                    if (chkOrder)
                    {
                        ltrorderNumber.Text = OrderNumber.ToString();
                        GetOrderDetails(OrderNumber);
                    }
                }
            }
        }

        /// <summary>
        /// Bind Order Details For Receipt
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        private void GetOrderDetails(Int32 OrderNumber)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet objDsorder = new DataSet();
            objDsorder = objOrder.GetOrderDetailsByOrderID(OrderNumber);
            if (objDsorder != null && objDsorder.Tables.Count > 0 && objDsorder.Tables[0].Rows.Count > 0)
            {
                string StrDate = "";
                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["OrderDate"].ToString()))
                {
                    StrDate = Convert.ToString(string.Format("{0:MMM dd,yyyy}", Convert.ToDateTime(objDsorder.Tables[0].Rows[0]["OrderDate"].ToString())));
                }
                ltrOrderdate.Text = StrDate.ToString();
                ltrorderNumber.Text = objDsorder.Tables[0].Rows[0]["RefOrderID"].ToString();

                ltrshippingMethod.Text = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();

                decimal ShippingCost = 0;
                decimal.TryParse(objDsorder.Tables[0].Rows[0]["OrderShippingCosts"].ToString(), out ShippingCost);

                decimal OrderTax = 0;
                decimal.TryParse(objDsorder.Tables[0].Rows[0]["OrderTax"].ToString(), out OrderTax);

                Decimal LevelDiscountAmount = 0;
                Decimal.TryParse(Convert.ToString(objDsorder.Tables[0].Rows[0]["LevelDiscountAmount"]), out LevelDiscountAmount);
                Decimal CouponDiscountAmount = 0;
                Decimal.TryParse(Convert.ToString(objDsorder.Tables[0].Rows[0]["CouponDiscountAmount"]), out CouponDiscountAmount);
                Decimal QuantityDiscountAmount = 0;
                Decimal.TryParse(Convert.ToString(objDsorder.Tables[0].Rows[0]["QuantityDiscountAmount"]), out QuantityDiscountAmount);
                Decimal GiftCertificateDiscountAmount = 0;
                Decimal.TryParse(Convert.ToString(objDsorder.Tables[0].Rows[0]["GiftCertificateDiscountAmount"]), out GiftCertificateDiscountAmount);
                Decimal custDist = 0;
                Decimal.TryParse(Convert.ToString(objDsorder.Tables[0].Rows[0]["CustomDiscount"]), out custDist);

                Decimal Discount = custDist + Math.Round(Convert.ToDecimal(LevelDiscountAmount) + Convert.ToDecimal(CouponDiscountAmount) + Convert.ToDecimal(QuantityDiscountAmount) + Convert.ToDecimal(GiftCertificateDiscountAmount), 2);

                decimal Ordertotal = 0;
                decimal.TryParse(objDsorder.Tables[0].Rows[0]["OrderTotal"].ToString(), out Ordertotal);

                decimal OrderSubTotal = 0;
                decimal.TryParse(objDsorder.Tables[0].Rows[0]["OrderSubTotal"].ToString(), out OrderSubTotal);


                //ltrshippedvia.Text = objDsorder.Tables[0].Rows[0]["shippedvia"].ToString();

                string StrShippedVia = "";
                StrShippedVia = Convert.ToString(CommonComponent.GetScalarCommonData("Select distinct cast(ISNULL(ShippedVia,0)+',' as varchar(max)) from tb_OrderShippedItems Where OrderNumber=" + OrderNumber + " for xml path('')"));
                if (!string.IsNullOrEmpty(StrShippedVia) && StrShippedVia.Length > 0)
                {
                    StrShippedVia = StrShippedVia.Substring(0, StrShippedVia.Length - 1);
                }
                //ltrshippedvia.Text = StrShippedVia.ToString();

                ltrShiptoName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString());
                ltrShiptoName1.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString());

                ltrBillingName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString());

                if (AppLogic.AppConfigs("SellerName") != null)
                {
                    ltrSellerName.Text = Convert.ToString(AppLogic.AppConfigs("SellerName"));
                }

                string StrShipAddr = "";
                //StrShipAddr = Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString()) + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString() + ", ";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingState"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingState"].ToString() + " ";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                    StrShipAddr += objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString() + "<br />";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                    StrShipAddr += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + "<br />";

                ltrAddress.Text += StrShipAddr.ToString();
                ltrShippingAddr.Text += StrShipAddr.ToString();

                BindCart(Convert.ToInt32(OrderNumber.ToString()), objDsorder, ShippingCost, Discount, OrderTax, OrderSubTotal, Ordertotal);

                DataSet dsTopic = new DataSet();
                dsTopic = TopicComponent.GetTopicList("AmazonPackingSlipTopic", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    if (dsTopic.Tables[0].Rows[0]["Description"].ToString() == "")
                    {
                        ltrOverstockInstruction.Text = "";
                    }
                    else
                    {
                        ltrOverstockInstruction.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                    }
                    dsTopic.Dispose();
                }
            }
        }

        /// <summary>
        /// Bind Order Cart By Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <param name="dsOrderdata">DataSet dsOrderdata</param>
        private void BindCart(Int32 OrderNumber, DataSet dsOrderdata, decimal ShippingCost, decimal Discount, decimal OrderTax, decimal OrderSubTotal, decimal Ordertotal)
        {
            OrderComponent objOrder = new OrderComponent();
            DataSet dsCart = new DataSet();
            dsCart = objOrder.GetInvoiceProductsWithMarryproduct(OrderNumber);
            DataSet dsPreferred = new DataSet();
            decimal Grandtotal = 0;

            ltrCart.Text = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\" class=\"datatable\" style=\"border-collapse: collapse;\">";
            ltrCart.Text += "<tr style=\"line-height: 50px; background-color: rgb(242,242,242);\">";

            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 10%;text-align: center;\">";
            ltrCart.Text += "<b>Quantity</b>";
            ltrCart.Text += "</th>";

            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 50%\">";
            ltrCart.Text += "<b>Product Details</b>";
            ltrCart.Text += "</th>";

            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 15%; text-align: center;\">";
            ltrCart.Text += "<b>Price</b>";
            ltrCart.Text += "</th>";

            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 20%; text-align: center;\">";
            ltrCart.Text += "<b>Total</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "</tr>";

            if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                {
                    bool titem = false;
                    if (Request.QueryString["Pid"] != null)
                    {
                        string strpids = "~" + Request.QueryString["Pid"].ToString();
                        if (strpids.IndexOf("~" + dsCart.Tables[0].Rows[i]["ProductID"].ToString() + "~") > -1)
                        {
                            titem = true;
                        }
                    }
                    else
                    {
                        titem = true;
                    }
                    if (titem == true)
                    {
                        ltrCart.Text += "<tr>";

                        ltrCart.Text += "<td style=\"text-align: center;\" valign=\"top\">";
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                        ltrCart.Text += "</td>";

                        ltrCart.Text += "<td valign=\"top\" align=\"left\" valign=\"top\">";
                        ltrCart.Text += "<b>" + dsCart.Tables[0].Rows[i]["ProductName"].ToString() + "</b><br/>";
                        string sku = "";
                        string[] variantName = dsCart.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantValue = dsCart.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        for (int j = 0; j < variantValue.Length; j++)
                        {
                            if (variantName.Length > j)
                            {
                                ltrCart.Text += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                                SQLAccess objSql = new SQLAccess();
                                DataSet dsoption = new DataSet();
                                dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + dsCart.Tables[0].Rows[i]["productId"] + " AND VariantValue='" + variantValue[j].ToString().Replace("'", "''") + "'");
                                if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                                {
                                    if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["SKU"].ToString()))
                                    {
                                        sku += "<br/>" + dsoption.Tables[0].Rows[0]["SKU"].ToString();
                                    }
                                }
                            }
                        }

                        string MerchantSKU = dsCart.Tables[0].Rows[i]["MerchantSKU"].ToString();
                        string OrderItemID = dsCart.Tables[0].Rows[i]["OrderItemID"].ToString();
                        string Condition = "New";
                        //if (!string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["amazonproductid"].ToString()))
                        //{
                        //    string RefAmazonProId = Convert.ToString(dsCart.Tables[0].Rows[i]["amazonproductid"].ToString());
                        //    Condition = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ConditionType,'New') from tb_ProductAmazon Where AmazonRefID=" + RefAmazonProId + ""));
                        //}
                        //Condition = "New";

                        DataSet dsAsin = CommonComponent.GetCommonDataSet("Select ISNULL(Asin,'') as ASIN,isnull(ProductSetID,'') as ProductSetID from tb_Product Where ProductId=" + dsCart.Tables[0].Rows[i]["productId"] + "");
                        

                        ltrCart.Text += "<br />";
                        if (!string.IsNullOrEmpty(MerchantSKU))
                        {
                            ltrCart.Text += "<b>SKU: </b>" + MerchantSKU.ToString() + "<br />";
                        }
                        if (dsAsin != null && dsAsin.Tables.Count > 0 && dsAsin.Tables[0].Rows.Count > 0)
                        {
                            if (!string.IsNullOrEmpty(dsAsin.Tables[0].Rows[0]["ASIN"].ToString()))
                            {
                                ltrCart.Text += "<b>ASIN: </b>" + dsAsin.Tables[0].Rows[0]["ASIN"].ToString() + "<br />";
                            }
                            if (!string.IsNullOrEmpty(dsAsin.Tables[0].Rows[0]["ProductSetID"].ToString()))
                            {
                                ltrCart.Text += "<b>Listing ID: </b>" + dsAsin.Tables[0].Rows[0]["ProductSetID"].ToString() + "<br />";
                            }
                        }
                        if (!string.IsNullOrEmpty(OrderItemID))
                        {
                            ltrCart.Text += "<b>Order Item ID: </b>" + OrderItemID.ToString() + "<br />";
                        }
                        if (!string.IsNullOrEmpty(Condition))
                        {
                            ltrCart.Text += "<b>Condition: </b>" + Condition + "<br />";
                        }
                        ltrCart.Text += "<b>Comments: </b>In original package<br />";

                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td style=\"text-align: center;\" valign=\"top\">";
                        ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C");
                        ltrCart.Text += "</td>";

                        if (i == 0)
                        {
                            ltrCart.Text += "###orderlastrow###";
                        }

                        ltrCart.Text += "</tr>";
                    }
                }

                string StrTotal = "";
                if (ltrCart.Text.ToString().ToLower().IndexOf("###orderlastrow###") > -1)
                {
                    StrTotal += "<td style=\"text-align: center;\"  valign=\"top\" rowspan=\"" + dsCart.Tables[0].Rows.Count + "\">";
                    StrTotal += "<table cellpadding=\"0\" align=\"right\" width=\"45%\" cellspacing=\"0\" style=\"border: none;\" border=\"0\">";

                    if (OrderSubTotal > decimal.Zero)
                        StrTotal += "<tr><td align=\"left\" style=\"border:none !Important;text-align: right;\" valign=\"middle\">Subtotal:</td><td align=\"right\" style=\"text-align: right;border:none !Important\" valign=\"middle\">" + OrderSubTotal.ToString("C") + "</td></tr>";

                    if (ShippingCost > decimal.Zero)
                        StrTotal += "<tr><td align=\"left\" style=\"border:none !Important;text-align: right;\" valign=\"middle\">Shipping:</td><td align=\"right\" style=\"text-align: right;border:none !Important\" valign=\"middle\">" + ShippingCost.ToString("C") + "</td></tr>";

                    if (Discount > decimal.Zero)
                        StrTotal += "<tr><td align=\"left\" style=\"border:none !Important;text-align: right;\" valign=\"middle\">Discount:</td><td align=\"right\" style=\"text-align: right;border:none !Important\" valign=\"middle\">" + Discount.ToString("C") + "</td></tr>";

                    if (OrderTax > decimal.Zero)
                        StrTotal += "<tr><td align=\"left\" style=\"border:none !Important;text-align: right;\" valign=\"middle\">Order Tax:</td><td align=\"right\" style=\"text-align: right;border:none !Important\" valign=\"middle\">" + OrderTax.ToString("C") + "</td></tr>";

                    StrTotal += "<tr><td align=\"left\" style=\"border:none !Important\" valign=\"middle\" colspan=\"2\"><hr style=\"border: 1px dotted #e7e7e7; border-style: none none dotted; width: 100%;text-align:right;\"></td></tr>";

                    StrTotal += "<tr><td align=\"left\" align=\"left\" style=\"border:none !Important;text-align: right;\" valign=\"middle\"><b>Total:<b></td><td align=\"right\" style=\"text-align: right;border:none !Important\" valign=\"middle\">" + Ordertotal.ToString("C") + "</td></tr>";
                    StrTotal += "</table>";
                    StrTotal += "</td>";
                    ltrCart.Text = ltrCart.Text.Replace("###orderlastrow###", StrTotal);
                }

                ltrCart.Text += "<tr>";
                ltrCart.Text += "<td style=\"text-align: right; padding-right: 9px;\" colspan=\"4\">";
                ltrCart.Text += "<span style=\"font-weight: bold; font-size: 14px;\">ORDER TOTAL : " + Ordertotal.ToString("C") + "</span>";
                ltrCart.Text += "</td>";
                ltrCart.Text += "</tr>";

            }
            ltrCart.Text += "</table>";
        }
    }
}