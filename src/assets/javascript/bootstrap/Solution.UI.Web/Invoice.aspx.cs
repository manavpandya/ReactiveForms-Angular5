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
    public partial class Invoice : System.Web.UI.Page
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
                BindInvoiceSignature();
                if (Request.QueryString["ONo"] != null)
                {
                    int OrderNumber = 0;
                    bool chkOrder = Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                    if (chkOrder)
                    {
                        ltrorderNumber.Text = OrderNumber.ToString();
                        GetOrderDetails(OrderNumber);
                        trHeaderMenu.Visible = false;
                        // trStoreBanner.Visible = false;
                        trTwitter.Visible = false;
                        trGooglePlus.Visible = false;
                        trPinterest.Visible = false;
                        trFacebook.Visible = false;

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
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["RefOrderID"].ToString()))
                {
                    ltrorderNumber.Text += " (Ref. Order No. : " + objDsorder.Tables[0].Rows[0]["RefOrderID"].ToString() + ")";
                }
                ltrOrderdate.Text = objDsorder.Tables[0].Rows[0]["OrderDate"].ToString();
                ltrshippingMethod.Text = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();
                ltrpaymentMethod.Text = objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString();
                try
                {
                    //ImgStoreLogo.Src = AppLogic.AppConfigs("LIVE_SERVER") + "/Images/Store_" + Convert.ToString(objDsorder.Tables[0].Rows[0]["StoreID"]) + ".png";
                    string url = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
                    ImgStoreLogo.Src = url + "/Images/Store_" + Convert.ToString(objDsorder.Tables[0].Rows[0]["StoreID"]) + ".png";
                }
                catch (Exception ex)
                {
                    ImgStoreLogo.Src = AppLogic.AppConfigs("LIVE_SERVER") + "/Images/Logo.png";
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
                                ltrCardNumber.Text += "*";
                            }
                            ltrCardNumber.Text += CardNumber.ToString().Substring(CardNumber.Length - 4);
                        }
                        else
                        {
                            ltrCardNumber.Text = "";
                        }
                        trcard.Visible = true;
                    }

                }
                else
                {
                    trcard.Visible = false;
                }

                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["LastName"].ToString());
                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString());

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

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td valign='Top' colspan=2>";
                ltrAddress.Text += "<table width=\"100%\" class=\"popup_cantain\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\"><tr><td valign='Top'>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCompany"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingCompany"].ToString() + "<br/>";
                if (BillingAddress1.ToString() != "")
                    ltrAddress.Text += BillingAddress1.ToString() + "<br/>";
                if (BillingAddress2.ToString() != "")
                    ltrAddress.Text += BillingAddress2.ToString() + "<br/>";
                if (BillingSuite.ToString() != "")
                    ltrAddress.Text += BillingSuite.ToString() + "<br/>";
                if (BillingCity.ToString() != "")
                    ltrAddress.Text += BillingCity.ToString() + "<br/>";
                if (BillingState.ToString() != "")
                    ltrAddress.Text += BillingState.ToString() + "<br/>";
                if (BillingZip.ToString() != "")
                    ltrAddress.Text += BillingZip.ToString() + "<br/>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()))
                {
                    if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()) + ", <br/>";
                    }
                    else
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()) + "<br/>";
                    }
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString();
                ltrAddress.Text += "</td>";



                ltrAddress.Text += "<td valign='Top'>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString() + "<br/>";
                if (ShippingAddress1.ToString() != "")
                    ltrAddress.Text += ShippingAddress1.ToString() + "<br/>";
                if (ShippingAddress2.ToString() != "")
                    ltrAddress.Text += ShippingAddress2.ToString() + "<br/>";
                if (ShippingSuite.ToString() != "")
                    ltrAddress.Text += ShippingSuite.ToString() + "<br/>";
                if (ShippingCity.ToString() != "")
                    ltrAddress.Text += ShippingCity.ToString() + "<br/>";
                if (ShippingState.ToString() != "")
                    ltrAddress.Text += ShippingState.ToString() + "<br/>";
                if (ShippingZip.ToString() != "")
                    ltrAddress.Text += ShippingZip.ToString() + "<br/>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                {
                    if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + ", <br/>";
                    }
                    else
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + "<br/>";
                    }
                }
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString();
                ltrAddress.Text += "</td></tr></table>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";


                #region Old Order Billing Shipping Detail
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}

                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "</tr>";
                //ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Address2:";
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingAddress2"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "</tr>";
                //ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Suite:";
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingSuite"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "</tr>";
                //ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "City:";
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCity"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingCity"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "</tr>";
                //ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "State:";
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingState"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingState"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingState"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingState"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "</tr>";
                //ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Zip:";
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingZip"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingZip"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "</tr>";
                //ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Country:";
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()))
                //{
                //    DataSet dsCountry = new DataSet();
                //    dsCountry = CommonComponent.GetCommonDataSet("SELECT Name FROM tb_Country WHERE CountryId=" + objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString() + "");
                //    if (dsCountry != null && dsCountry.Tables.Count > 0 && dsCountry.Tables[0].Rows.Count > 0)
                //    {
                //        ltrAddress.Text += Convert.ToString(dsCountry.Tables[0].Rows[0]["Name"].ToString());
                //    }
                //    else
                //    {
                //        ltrAddress.Text += "-";
                //    }

                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";

                //ltrAddress.Text += "</tr>";
                //ltrAddress.Text += "<tr>";
                //ltrAddress.Text += "<td>";
                //ltrAddress.Text += "Phone:";
                //ltrAddress.Text += "</td>";
                //ltrAddress.Text += "<td>";
                //if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                //{
                //    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString();
                //}
                //else
                //{
                //    ltrAddress.Text += "-";
                //}
                //ltrAddress.Text += "</td>";

                //  ltrAddress.Text += "</tr>";
                #endregion

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
            decimal GrandSwatchSubTotal = decimal.Zero;
            OrderComponent objOrder = new OrderComponent();
            decimal AdjustmentAmount = 0;
            decimal total = 0;
            decimal Subtotal = 0;
            decimal RefundAmt = 0;
            decimal FinalTotal = 0;
            bool IsCouponDiscount = false;
            DataSet dsCart = new DataSet();
            //  dsCart = objOrder.GetProductList(OrderNumber);
            Int32 SwatchQty = 0;

            String strswatchQtyy = "";
            if (Session["CustID"] == null)
            {
                if (Session["CID"] != null)
                {
                    strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_OrderedShoppingCartItems WHERE IsProductType=0 AND OrderedShoppingCartID in (SELECT Top 1 OrderedShoppingCartID FROM tb_OrderedShoppingCart WHERE CustomerID=" + Session["CID"].ToString() + " Order By OrderedShoppingCartID DESC) "));
                }
            }
            else
            {
                strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_OrderedShoppingCartItems WHERE IsProductType=0 AND OrderedShoppingCartID in (SELECT Top 1 OrderedShoppingCartID FROM tb_OrderedShoppingCart WHERE CustomerID=" + Session["CustID"].ToString() + " Order By OrderedShoppingCartID DESC) "));
            }
            if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
            {
                SwatchQty = Convert.ToInt32(strswatchQtyy) - Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString());
            }



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
            ltrCart.Text += "###coupondiscount###";
            ltrCart.Text += "<td valign=\"middle\" align=\"center\" style=\"text-align: center;\">";
            ltrCart.Text += "<b>Quantity</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "<td style=\"text-align: right;\">";
            ltrCart.Text += "<b>Sub Total</b>";
            ltrCart.Text += "</td>";
            ltrCart.Text += "</tr>";
            if (dsCart != null && dsCart.Tables.Count > 0 && dsCart.Tables[0].Rows.Count > 0)
            {
                for (int k = 0; k < dsCart.Tables[0].Rows.Count; k++)
                {
                    decimal CouponDiscount = 0;
                    if (!string.IsNullOrEmpty(dsCart.Tables[0].Rows[k]["DiscountPrice"].ToString()))
                    {
                        decimal.TryParse(dsCart.Tables[0].Rows[k]["DiscountPrice"].ToString(), out CouponDiscount);
                        if (CouponDiscount > Decimal.Zero)
                        {
                            IsCouponDiscount = true;
                        }
                    }
                }

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
                    //ltrCart.Text += dsCart.Tables[0].Rows[i]["SKU"].ToString();
                    ltrCart.Text += dsCart.Tables[0].Rows[i]["CartSKU"].ToString();

                    ltrCart.Text += "</td>";


                    decimal CouponDiscount = 0;



                    if (SwatchQty != 0)
                    {
                        Int32 Isorderswatch = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + " and ItemType='Swatch'"));
                        if (Isorderswatch == 1)
                        {
                            if (Convert.ToInt32(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) >= SwatchQty)
                            {
                                Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + ""));

                                ltrCart.Text += "<td style=\"text-align: right;\">";
                                ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(pp));
                                ltrCart.Text += "</td>";


                                if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                {
                                    decimal.TryParse(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString().ToString(), out CouponDiscount);
                                    ltrCart.Text += "<td style=\"text-align: right;\">" + Convert.ToDecimal(CouponDiscount).ToString("C") + "</td>";
                                }

                                ltrCart.Text += "<td style=\"text-align: center;\">";
                                //  lblNettotal.Text = String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty)));
                                if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                {
                                    GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()) * Convert.ToDecimal(SwatchQty));
                                }
                                else
                                {
                                    GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty));
                                }
                                ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                                ltrCart.Text += "</td>";
                                ltrCart.Text += "<td style=\"text-align: right;\">";
                                if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                {
                                    ltrCart.Text += String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()) * Convert.ToDecimal(SwatchQty)));
                                }
                                else
                                {
                                    ltrCart.Text += String.Format("{0:0.00}", Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty)));
                                }
                                ltrCart.Text += "</td>";
                                ltrCart.Text += "</tr>";
                                SwatchQty = 0;
                            }
                            else
                            {
                                Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["RefProductID"].ToString() + ""));
                                ltrCart.Text += "<td style=\"text-align: right;\">";
                                ltrCart.Text += String.Format("{0:0.00}", Convert.ToDecimal(pp));
                                ltrCart.Text += "</td>";
                                if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                {
                                    GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()) * Convert.ToDecimal(SwatchQty));
                                }
                                else
                                {
                                    GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty));
                                }
                                SwatchQty = SwatchQty - Convert.ToInt32(dsCart.Tables[0].Rows[i]["Quantity"].ToString());
                            }


                        }

                        else
                        {

                            ltrCart.Text += "<td style=\"text-align: right;\">";
                            ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C");
                            ltrCart.Text += "</td>";

                            if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                            {
                                decimal.TryParse(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString(), out CouponDiscount);
                                ltrCart.Text += "<td style=\"text-align: right;\">" + Convert.ToDecimal(CouponDiscount).ToString("C") + "</td>";
                            }
                            ltrCart.Text += "<td style=\"text-align: center;\">";
                            ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                            ltrCart.Text += "</td>";
                            ltrCart.Text += "<td style=\"text-align: right;\">";
                            ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["TotalPrice"].ToString()).ToString("C");
                            GrandSwatchSubTotal += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["TotalPrice"].ToString());
                            ltrCart.Text += "</td>";
                            ltrCart.Text += "</tr>";
                        }
                    }
                    else
                    {


                        ltrCart.Text += "<td style=\"text-align: right;\">";
                        ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C");
                        ltrCart.Text += "</td>";

                        if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                        {
                            decimal.TryParse(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString(), out CouponDiscount);
                            ltrCart.Text += "<td style=\"text-align: right;\">" + Convert.ToDecimal(CouponDiscount).ToString("C") + "</td>";
                        }
                        ltrCart.Text += "<td style=\"text-align: center;\">";
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td style=\"text-align: right;\">";
                        ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["TotalPrice"].ToString()).ToString("C");
                        GrandSwatchSubTotal += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["TotalPrice"].ToString());
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";

                    }



                }


                if (IsCouponDiscount == true && ltrCart.Text.ToString().ToLower().IndexOf("###coupondiscount###") > -1)
                {
                    string StrCoupon = "<td valign=\"top\" align=\"center\" style=\"width: 10%; text-align: center;\"><b>Discount Price</b></td>";
                    ltrCart.Text = ltrCart.Text.Replace("###coupondiscount###", StrCoupon.ToString().Trim());
                }
                else
                {
                    ltrCart.Text = ltrCart.Text.Replace("###coupondiscount###", "");
                }
                string StrColapan = "";
                if (IsCouponDiscount == true)
                {
                    StrColapan = "5";
                }
                else { StrColapan = "4"; }
                if (dsOrderdata != null && dsOrderdata.Tables.Count > 0 && dsOrderdata.Tables[0].Rows.Count > 0)
                {
                    if (dsOrderdata.Tables[0].Rows[0]["AdjustmentAmount"] != null)
                    {
                        AdjustmentAmount = Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["AdjustmentAmount"].ToString()), 2);
                    }


                    if (GrandSwatchSubTotal > Decimal.Zero)
                    {
                        Subtotal = GrandSwatchSubTotal + AdjustmentAmount;

                    }
                    else { Subtotal = Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["OrderSubtotal"].ToString()) + AdjustmentAmount; }



                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"" + StrColapan + "\">";
                    ltrCart.Text += "Sub Total:";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 16%;\">";
                    ltrCart.Text += Subtotal.ToString("C");
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";

                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"" + StrColapan + "\">";
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
                        ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"" + StrColapan + "\">";
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
                        ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"" + StrColapan + "\">";
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
                    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"" + StrColapan + "\">";
                    ltrCart.Text += "<span>Discount:</span>";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td valign=\"top\" style=\"text-align: right; height: 21px; width: 15%;\">";
                    ltrCart.Text += Math.Round(Discount, 2).ToString("C");
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";


                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"" + StrColapan + "\">";
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
                        ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"height: 21px; text-align: right\" colspan=\"" + StrColapan + "\">";
                        ltrCart.Text += "Adjustment Amount:";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td valign=\"top\" style=\"width: 15%; text-align: right; height: 21px;\">";
                        ltrCart.Text += "-" + Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["RefundedAmount"].ToString()).ToString("C");
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";
                    }

                    if (dsOrderdata.Tables[0].Rows[0]["OrderTotal"] != null)

                    { FinalTotal = Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["OrderTotal"].ToString()), 2) + AdjustmentAmount - RefundAmt; }

                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td valign=\"top\" align=\"right\" style=\"text-align: right; height: 21px;\" colspan=\"" + StrColapan + "\">";
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
        }

        /// <summary>
        /// Binds the Invoice Signature.
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
            else
            {
                dsTopic = CommonComponent.GetCommonDataSet("SELECT 'Thank You,<br>'+StoreName as InvoiceSignature FROM dbo.tb_Store WHERE StoreID=" + Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
                if (dsTopic != null && dsTopic.Tables.Count > 0 && dsTopic.Tables[0].Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(dsTopic.Tables[0].Rows[0]["InvoiceSignature"].ToString()))
                    {
                        ltInvoiceSignature.Text = dsTopic.Tables[0].Rows[0]["InvoiceSignature"].ToString();
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
}