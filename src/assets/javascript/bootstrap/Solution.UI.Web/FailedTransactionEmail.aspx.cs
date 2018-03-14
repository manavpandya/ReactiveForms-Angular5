using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Data;

namespace Solution.UI.Web
{
    public partial class FailedTransactionEmail : System.Web.UI.Page
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonOperations.RedirectWithSSL(false);
            if (!IsPostBack)
            {
                //BindInvoiceSignature();
                if (Request.QueryString["ONo"] != null)
                {
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
                ltrOrderdate.Text = objDsorder.Tables[0].Rows[0]["OrderDate"].ToString();
                // ltrshippingMethod.Text = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();
                ltrpaymentMethod.Text = objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString();
                ltrCustEmail.Text = objDsorder.Tables[0].Rows[0]["Email"].ToString();

                String Reason = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(TransactionResult,'') from tb_FailedTransaction  where OrderNumber=" + OrderNumber + ""));
                if (!string.IsNullOrEmpty(Reason.ToString().Trim()))
                {
                    try
                    {
                        String[] Reson01 = Reason.Split('|');
                        if (Reson01.Length > 0 && Reson01.Length > 2)
                        {
                            ltrFailedReson.Text = Reson01[3].ToString().Trim();
                        }
                    }
                    catch { ltrFailedReson.Text = "--"; }
                }

                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString()) && objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString().ToLower() == "creditcard")
                {
                    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CardNumber"].ToString()))
                    {
                        string CardNumber = SecurityComponent.Decrypt(objDsorder.Tables[0].Rows[0]["CardNumber"].ToString());
                        if (CardNumber.Length > 4)
                        {
                            for (int i = 0; i < CardNumber.Length - 4; i++)
                            {
                                // ltrCardNumber.Text += "*";
                            }
                            //ltrCardNumber.Text += CardNumber.ToString().Substring(CardNumber.Length - 4);
                        }
                        else
                        {
                            //ltrCardNumber.Text = "";
                        }
                        //trcard.Visible = true;
                    }

                }
                else
                {
                    //trcard.Visible = false;
                }
                ltrAddress.Text = "";
                ltrAddress.Text += "<table width=\"100%\" class=\"popup_cantain\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\">";
                ltrAddress.Text += "<tbody>";
                ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "<b>Account</b>";
                //ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td width=\"50%\">";
                ltrAddress.Text += "<b>Billing Address</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "<b>Shipping Address</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Name:";
                //ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td class=\"font-bold\">";
                ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString()); ;
                ltrAddress.Text += "</td>";

                ltrAddress.Text += "<td class=\"font-bold\">";
                ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString()); ;
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Company:";
                //ltrAddress.Text += "</td>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCompany"].ToString()))
                {
                    ltrAddress.Text += "<td>";
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingCompany"].ToString();
                    ltrAddress.Text += "</td>";
                }

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                {
                    ltrAddress.Text += "<td>";
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString();
                    ltrAddress.Text += "</td>";
                }
                //else
                //{
                //    ltrAddress.Text += "-";
                //}


                ltrAddress.Text += "</tr>";

                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Address1:";
                //ltrAddress.Text += "</td>";



                String BillingSuite = "";
                String BillingAddress2 = "";
                String BillingCity = "";
                String BillingZip = "";
                String BillingAddress1 = "";
                String BillingState = "";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                {
                    BillingSuite = " #" + objDsorder.Tables[0].Rows[0]["BillingSuite"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                {
                    BillingAddress2 = " " + objDsorder.Tables[0].Rows[0]["BillingAddress2"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCity"].ToString()))
                {
                    BillingCity = " " + objDsorder.Tables[0].Rows[0]["BillingCity"].ToString() + ",";
                }

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingZip"].ToString()))
                {
                    BillingZip = " " + objDsorder.Tables[0].Rows[0]["BillingZip"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingAddress1"].ToString()))
                {
                    BillingAddress1 = " " + objDsorder.Tables[0].Rows[0]["BillingAddress1"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingState"].ToString()))
                {
                    BillingState = " " + objDsorder.Tables[0].Rows[0]["BillingState"].ToString() + ",";
                }


                String ShippingSuite = "";
                String ShippingAddress2 = "";
                String ShippingCity = "";
                String ShippingZip = "";
                String ShippingAddress1 = "";
                String ShippingState = "";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                {
                    ShippingSuite = " #" + objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                {
                    ShippingAddress2 = " " + objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                {
                    ShippingCity = " " + objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString() + ",";
                }

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                {
                    ShippingZip = " " + objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                {
                    ShippingAddress1 = " " + objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString() + ",";
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingState"].ToString()))
                {
                    ShippingState = " " + objDsorder.Tables[0].Rows[0]["ShippingState"].ToString() + ",";
                }

                string BillingCountry = "";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()))
                {
                    if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    {
                        BillingCountry = Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()) + ", ";
                    }
                    else
                    {
                        BillingCountry = Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString());
                    }
                }

                string ShippingCountry = "";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                {
                    if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    {
                        ShippingCountry = Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + ", ";
                    }
                    else
                    {
                        ShippingCountry = Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString());
                    }
                }

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += BillingAddress1.ToString() + "" + BillingAddress2.ToString() + "" + BillingSuite.ToString() + "" + BillingCity.ToString() + "" + BillingState.ToString() + "" + BillingZip.ToString() + "" + BillingCountry;
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += ShippingAddress1.ToString() + "" + ShippingAddress2.ToString() + "" + ShippingSuite.ToString() + "" + ShippingCity.ToString() + "" + ShippingState.ToString() + "" + ShippingZip.ToString() + "" + ShippingCountry;
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()) && !String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                {
                    ltrAddress.Text += "<tr>";
                    ltrAddress.Text += "<td>";
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString();
                    ltrAddress.Text += "</td>";
                    ltrAddress.Text += "<td>";
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString();
                    ltrAddress.Text += "</td>";
                    ltrAddress.Text += "</tr>";
                }
                ltrAddress.Text += "</tbody>";
                ltrAddress.Text += "</table>";
                BindCart(Convert.ToInt32(objDsorder.Tables[0].Rows[0]["ShoppingCardID"].ToString()), objDsorder);
            }
        }

        /// <summary>
        /// Bind Order Cart By Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        private void BindCart(Int32 OrderNumber, DataSet dsOrderdata)
        {
            OrderComponent objOrder = new OrderComponent();
            decimal AdjustmentAmount = 0;
            decimal total = 0;
            decimal Subtotal = 0;
            decimal RefundAmt = 0;
            decimal FinalTotal = 0;
            DataSet dsCart = new DataSet();
            //  dsCart = objOrder.GetProductList(OrderNumber);
            dsCart = objOrder.GetInvoiceProductList(OrderNumber);
            ltrCart.Text = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\" class=\"pop_border_new2\" style=\"border-collapse: collapse;\">";
            //ltrCart.Text += "<tr style=\"line-height: 50px; background-color: rgb(242,242,242);\">";
            ltrCart.Text += "<tr style=\"line-height: 30px;\">";
            ltrCart.Text += "<td valign=\"middle\" align=\"left\" style=\"width: 55%\">";
            ltrCart.Text += "<b>Product</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td valign=\"middle\" align=\"center\" style=\"width: 10%; text-align: center;\">";
            ltrCart.Text += "<b>SKU</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td valign=\"top\" align=\"center\" style=\"width: 10%; text-align: center;\">";
            ltrCart.Text += "<b>Price</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td valign=\"middle\" align=\"center\" style=\"text-align: center;\">";
            ltrCart.Text += "<b>Quantity</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td style=\"text-align: right;\">";
            ltrCart.Text += "<b>Sub Total</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "</tr>";
            if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsCart.Tables[0].Rows.Count; i++)
                {
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td valign=\"top\" align=\"left\">";
                    ltrCart.Text += dsCart.Tables[0].Rows[i]["ProductName"].ToString() + "<br/>";

                    string[] variantName = dsCart.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string[] variantValue = dsCart.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                    for (int j = 0; j < variantValue.Length; j++)
                    {
                        if (variantName.Length > j)
                        {
                            ltrCart.Text += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                        }
                    }

                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td style=\"text-align: center;\">";
                    ltrCart.Text += dsCart.Tables[0].Rows[i]["SKU"].ToString();
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td style=\"text-align: right;\">";
                    ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C");
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td style=\"text-align: center;\">";
                    ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td style=\"text-align: right;\">";
                    ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["TotalPrice"].ToString()).ToString("C");
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                }
            }
            if (dsOrderdata != null && dsOrderdata.Tables.Count > 0 && dsOrderdata.Tables[0].Rows.Count > 0)
            {
                if (dsOrderdata.Tables[0].Rows[0]["AdjustmentAmount"] != null)
                {
                    AdjustmentAmount = Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["AdjustmentAmount"].ToString()), 2);
                }

                Subtotal = Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["OrderSubtotal"].ToString()) + AdjustmentAmount;
                ltrCart.Text += "<tr>";
                ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                ltrCart.Text += "Sub Total:";
                ltrCart.Text += "</td>";
                ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 16%;\">";
                ltrCart.Text += Subtotal.ToString("C");
                ltrCart.Text += "</td>";
                ltrCart.Text += "</tr>";

                ltrCart.Text += "<tr>";
                ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                ltrCart.Text += "<span >Shipping:</span>";
                ltrCart.Text += "</td>";
                ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 15%;\">";
                ltrCart.Text += Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["OrderShippingCosts"].ToString()).ToString("C");
                ltrCart.Text += "</td>";
                ltrCart.Text += "</tr>";
                //if (!string.IsNullOrEmpty(dsOrderdata.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()) && (Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()) > 0))
                //{
                //    ltrCart.Text += "<tr>";
                //    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                //    ltrCart.Text += "<span>Customer Level Discount:</span>";
                //    ltrCart.Text += "</td>";
                //    ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 15%;\">";
                //    ltrCart.Text += Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["LevelDiscountAmount"].ToString()).ToString("C");
                //    ltrCart.Text += "</td>";
                //    ltrCart.Text += "</tr>";
                //}
                if (!string.IsNullOrEmpty(dsOrderdata.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()) && (Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()) > 0))
                {
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                    ltrCart.Text += "<span>Quantity Discount:</span>";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 15%;\">";
                    ltrCart.Text += Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["QuantityDiscountAmount"].ToString()).ToString("C");
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                }
                //ltrCart.Text += "<tr>";
                //ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                //ltrCart.Text += "<span>Discount:</span>";
                //ltrCart.Text += "</td>";
                //ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 15%;\">";
                //ltrCart.Text += Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["CouponDiscountAmount"].ToString()).ToString("C");
                //ltrCart.Text += "</td>";
                //ltrCart.Text += "</tr>";

                if (!string.IsNullOrEmpty(dsOrderdata.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString()) && (Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["GiftCertificateDiscountAmount"].ToString()) > 0))
                {
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                    ltrCart.Text += "<span>Gift Certificate Discount:</span>";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 15%;\">";
                    ltrCart.Text += Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["GiftCertificateDiscountAmount"]), 2).ToString("C");
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                }
                Decimal custDist = 0;
                Decimal.TryParse(Convert.ToString(dsOrderdata.Tables[0].Rows[0]["CustomDiscount"]), out custDist);
                //Decimal Discount = custDist + Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["LevelDiscountAmount"]) + Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["CouponDiscountAmount"]) + Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["QuantityDiscountAmount"]), 2);

                Decimal Discount = custDist + Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["LevelDiscountAmount"]) + Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["CouponDiscountAmount"]), 2);
                ltrCart.Text += "<tr>";
                ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                ltrCart.Text += "<span>Discount:</span>";
                ltrCart.Text += "</td>";
                ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 15%;\">";
                ltrCart.Text += Math.Round(Discount, 2).ToString("C");
                ltrCart.Text += "</td>";
                ltrCart.Text += "</tr>";


                ltrCart.Text += "<tr>";
                ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                ltrCart.Text += "Order Tax:";
                ltrCart.Text += "</td>";
                ltrCart.Text += "<td valign=\"top\" style=\"width: 15%; text-align: right; height: 21px;\">";
                ltrCart.Text += Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["OrderTax"].ToString()).ToString("C");
                ltrCart.Text += "</td>";
                ltrCart.Text += "</tr>";


                if (dsOrderdata.Tables[0].Rows[0]["RefundedAmount"] != null && Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["RefundedAmount"]) > 0)
                {
                    RefundAmt = Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["RefundedAmount"].ToString()), 2);
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"4\">";
                    ltrCart.Text += "Adjustment Amount:";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"top\" style=\"width: 15%; text-align: right; height: 21px;\">";
                    ltrCart.Text += "-" + Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["RefundedAmount"].ToString()).ToString("C");
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                }

                if (dsOrderdata.Tables[0].Rows[0]["OrderTotal"] != null)
                {
                    FinalTotal = Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["OrderTotal"].ToString()), 2) + AdjustmentAmount - RefundAmt;
                }

                ltrCart.Text += "<tr>";
                ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"text-align: right; height: 21px;\" colspan=\"4\">";
                ltrCart.Text += "Total:";
                ltrCart.Text += "</td>";
                ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; width: 15%; height: 21px;\">";
                if (FinalTotal < 0)
                {
                    ltrCart.Text += "$0.00";
                }
                else
                {
                    ltrCart.Text += FinalTotal.ToString("C");
                }
                ltrCart.Text += "</td>";
                ltrCart.Text += "</tr>";
            }
            ltrCart.Text += "</table>";
        }

        /// <summary>
        /// Binds the Invoice Signature
        /// </summary>
        private void BindInvoiceSignature()
        {
            DataSet dsTopic = new DataSet();
            dsTopic = TopicComponent.GetTopicList("InvoiceSignature", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
            {
                if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["Description"].ToString()))
                {
                    ltInvoiceSignature.Text = dsTopic.Tables[0].Rows[0]["Description"].ToString();
                }
                else
                {
                    ltInvoiceSignature.Text = "";
                }
                dsTopic.Dispose();
            }
        }
    }
}