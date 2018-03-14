using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.IO;
using Solution.Bussines.Entities;
using System.Net;
using System.Net.Mail;
using Solution.ShippingMethods;
using System.Text;

namespace Solution.UI.Web
{
    public partial class CheckOutCustomerQuote : System.Web.UI.Page
    {
      
        public static decimal FinalTotal = decimal.Zero;
        public static decimal FreeShippingAmount = decimal.Zero;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;
            if (Session["CustID"] == null || Session["CustID"].ToString() == "")
            {
                Response.Redirect("/index.aspx");
            }

            CommonOperations.RedirectWithSSL(true);

            if (!IsPostBack)
            {
                Session["CustomLevelDiscount"] = null;
                GetCreditCardType();

                if (!string.IsNullOrEmpty(AppLogic.AppConfigs("FreeShippingLimit").ToString().Trim()))
                {
                    FreeShippingAmount = Convert.ToDecimal(AppLogic.AppConfigs("FreeShippingLimit"));
                }
                else
                { }

                if (Session["PaymentMethod"] != null)
                {
                    ltrMethodName.Text = Session["PaymentMethod"].ToString();
                    OrderComponent objPayment = new OrderComponent();
                    DataSet dsPayment = new DataSet();
                    dsPayment = objPayment.GetPaymentGateway(Session["PaymentMethod"].ToString(), Convert.ToInt32(AppLogic.AppConfigs("StoreId").ToString()));
                    if (dsPayment != null && dsPayment.Tables.Count > 0 && dsPayment.Tables[0].Rows.Count > 0)
                    {
                        Session["PaymentGateway"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["PaymentService"].ToString());
                        Session["PaymentGatewayStatus"] = Convert.ToString(dsPayment.Tables[0].Rows[0]["InitialPaymentStatus"].ToString());
                    }
                }
                else
                {
                    ltrMethodName.Text = "UnKnown";
                }

                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    GetCustomerDetails(Convert.ToInt32(Session["CustID"].ToString()));
                    if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
                    {
                        btnPlaceOrder.Attributes.Add("onclick", "return ValidationLoginuser();");
                    }
                    else
                    {
                        tblpayment.Visible = false;
                        btnPlaceOrder.Attributes.Add("onclick", "return ValidationLoginuserOtherPayment();");
                    }
                    tblBillAddEntry.Visible = false;
                    tblShippAddressEntry.Visible = false;
                    tblBillAddress.Visible = true;
                    tblShipAddress.Visible = true;
                    txtNameOnCard.Focus();
                    // BindShippingMethod();
                }
                else
                {
                    FillCountry();
                    tblBillAddEntry.Visible = true;
                    tblShippAddressEntry.Visible = true;
                    tblBillAddress.Visible = false;
                    tblShipAddress.Visible = false;
                    if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
                    {
                        btnPlaceOrder.Attributes.Add("onclick", "return ValidationNotLogin();");
                    }
                    else
                    {
                        tblpayment.Visible = false;
                        btnPlaceOrder.Attributes.Add("onclick", "return ValidationNotLoginOtherPayment();");
                    }
                    txtBillFirstname.Focus();

                    #region Fill Automatic Country, ZipCode and Shipping Methods

                    if (Session["SelectedZipCode"] != null && Convert.ToString(Session["SelectedZipCode"]) != "")
                    {
                        try
                        {
                            txtBillZipCode.Text = Convert.ToString(Session["SelectedZipCode"]);
                            txtShipZipCode.Text = Convert.ToString(Session["SelectedZipCode"]);
                            hdnZipCode.Value = Convert.ToString(Session["SelectedZipCode"]);
                        }
                        catch
                        {
                            txtBillZipCode.Text = "";
                            txtShipZipCode.Text = "";
                            hdnZipCode.Value = "";
                        }
                    }

                    if (Session["SelectedCountry"] != null && Convert.ToString(Session["SelectedCountry"]) != "")
                    {
                        hdncountry.Value = Convert.ToString(Session["SelectedCountry"]);
                        try
                        {
                            ddlBillcountry.SelectedValue = Convert.ToString(Session["SelectedCountry"]);
                        }
                        catch
                        {
                            if (ddlBillcountry.Items.FindByText("United States") != null)
                            {
                                ddlBillcountry.Items.FindByText("United States").Selected = true;
                            }
                            else
                            {
                                ddlBillcountry.SelectedIndex = 0;
                            }
                            txtBillZipCode.Text = "";
                        }
                        try
                        {
                            ddlShipCounry.SelectedValue = Convert.ToString(Session["SelectedCountry"]);
                        }
                        catch
                        {
                            if (ddlShipCounry.Items.FindByText("United States") != null)
                            {
                                ddlShipCounry.Items.FindByText("United States").Selected = true;
                            }
                            else
                            {
                                ddlShipCounry.SelectedIndex = 0;
                            }
                            txtShipZipCode.Text = "";
                        }
                    }
                    BindOrderSummary();
                    BindShippingMethod();

                    #endregion

                }

                BindOrderSummary();
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    BindShippingMethod();
                }
            }
            else
            { }
        }

        /// <summary>
        ///  State Other Selection
        /// </summary>
        private void StateOtherSelection()
        {
            string strScript = "";
            if (ddlShipState.SelectedValue == "-11")
            {
                strScript += "SetShippingOtherVisible(true);";
            }
            else
            {
                strScript += "SetShippingOtherVisible(false);";
            }
            if (ddlBillstate.SelectedValue == "-11")
            {
                strScript += "SetBillingOtherVisible(true);";
            }
            else
            {
                strScript += "SetBillingOtherVisible(false);";
            }
            if (UseShippingAddress.Checked)
            {
                strScript += "SetBillingShippingVisible(true);";
            }
            else
            {
                strScript += "SetBillingShippingVisible(false);";
            }
            Page.ClientScript.RegisterStartupScript(typeof(Page), "ShippingVisible", strScript, true);
        }

        /// <summary>
        /// Bind Order Summary By Customer ID
        /// </summary>
        private void BindOrderSummary()
        {
            if (Session["CustID"] != null && Session["CustID"].ToString() != "")
            {
                DataSet dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Convert.ToInt32(Session["CustID"].ToString()));
                ViewState["Weight"] = null;
                if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                {
                    ltrCart.Text = "<table cellspacing=\"0\" cellpadding=\"0\" border=\"0\" width=\"100%\" class=\"table\">";
                    ltrCart.Text += "<tbody><tr>";
                    ltrCart.Text += "<th width=\"24%\">";
                    ltrCart.Text += "Image";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "<th align=\"left\" width=\"48%\">";
                    ltrCart.Text += "Product";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "<th align=\"center\" width=\"13%\">";
                    ltrCart.Text += "Quantity";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "<th align=\"center\" width=\"15%\">";
                    ltrCart.Text += "Price";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "</tr>";

                    for (int i = 0; i < dsShoppingCart.Tables[0].Rows.Count; i++)
                    {
                        ltrCart.Text += "<tr>";
                        ltrCart.Text += "<td align=\"center\" valign=\"top\">";
                        Random rd = new Random();
                        string strmainPath = AppLogic.AppConfigs("ImagePathProduct").ToString();
                        if (!string.IsNullOrEmpty(dsShoppingCart.Tables[0].Rows[i]["ImageName"].ToString()))
                        {
                            string strimgName = strmainPath + "micro/" + dsShoppingCart.Tables[0].Rows[i]["ImageName"].ToString();
                            if (File.Exists(Server.MapPath(strimgName)))
                            {
                                strimgName = strimgName + "?" + rd.Next(1000).ToString();
                                ltrCart.Text += "<img height=\"57\" width=\"98\" title=\"\" alt=\"\" src=\"" + strimgName.ToString() + "\">";
                            }
                            else
                            {
                                ltrCart.Text += "<img height=\"57\" width=\"98\" title=\"\" alt=\"\" src=\"" + strmainPath + "micro/image_not_available.jpg\" />";
                            }
                        }
                        else
                        {
                            ltrCart.Text += "<img height=\"57\" width=\"98\" title=\"\" alt=\"\" src=\"" + strmainPath + "micro/image_not_available.jpg\" />";
                        }
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td valign=\"top\">";
                        // ltrCart.Text += "<a title=\"" + Server.HtmlEncode(dsShoppingCart.Tables[0].Rows[i]["Name"].ToString()) + "\" href=\"/" + dsShoppingCart.Tables[0].Rows[i]["Maincategory"].ToString() + "/" + dsShoppingCart.Tables[0].Rows[i]["Sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[i]["ProductID"].ToString() + ".aspx\">" + dsShoppingCart.Tables[0].Rows[i]["Name"].ToString() + "</a><br />";
                        ltrCart.Text += "" + dsShoppingCart.Tables[0].Rows[i]["Name"].ToString() + "<br />";

                        string[] variantName = dsShoppingCart.Tables[0].Rows[i]["VariantNames"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] variantValue = dsShoppingCart.Tables[0].Rows[i]["VariantValues"].ToString().Trim().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);

                        for (int j = 0; j < variantValue.Length; j++)
                        {
                            if (variantName.Length > j)
                            {
                                ltrCart.Text += variantName[j].ToString() + " : " + variantValue[j].ToString() + "<br />";
                            }
                        }

                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td align=\"center\" valign=\"top\">";
                        ltrCart.Text += dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString();
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td align=\"right\" valign=\"top\">";
                        // Check Quantity Discount 

                        Decimal QtyDiscount = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SElect ISNULL(qt.DiscountPercent,0) as DiscountPercent from tb_QuantityDiscountTable as qt " +
                                            " inner join tb_QauntityDiscount ON qt.QuantityDiscountID = dbo.tb_QauntityDiscount.QuantityDiscountID  " +
                                            " Where qt.LowQuantity<=" + dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString() + " and qt.HighQuantity>=" + dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString() + " and tb_QauntityDiscount.QuantityDiscountID in (Select QuantityDiscountID from  " +
                                            " tb_Product Where StoreId=" + AppLogic.AppConfigs("StoreId").ToString() + " and ProductId=" + dsShoppingCart.Tables[0].Rows[i]["ProductId"].ToString() + ") "));

                        if (QtyDiscount > Decimal.Zero)
                        {
                            decimal PriceValue = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString());
                            QtyDiscount = Math.Round((Convert.ToDecimal(PriceValue.ToString()) * QtyDiscount) / 100, 2);
                            string dd = string.Format("{0:0.00}", QtyDiscount);

                            if (QtyDiscount > 0)
                            {
                                //decimal QtyDisPrice = Convert.ToDecimal(Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString()) - QtyDiscount);
                                decimal QtyDisPrice = (Convert.ToDecimal(PriceValue) * Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString())) - (Convert.ToDecimal(dd) * Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Qty"].ToString()));

                                ltrCart.Text += "<s>$" + String.Format("{0:0.00}", Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString())) + "</s><br />";
                                ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(QtyDisPrice.ToString()));
                            }
                            else
                                ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString()));
                        }
                        else
                        {
                            ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[i]["Price"].ToString()));
                        }
                        // End

                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";
                        DataSet dsWeight = new DataSet();
                        dsWeight = ProductComponent.GetproductImagename(Convert.ToInt32(dsShoppingCart.Tables[0].Rows[i]["ProductId"].ToString()));
                        decimal objdecimal = decimal.Zero;
                        if (dsWeight != null && dsWeight.Tables.Count > 0 && dsWeight.Tables[0].Rows.Count > 0)
                        {
                            if (Convert.ToBoolean(dsWeight.Tables[0].Rows[0]["IsFreeShipping"]))
                            {
                                objdecimal = 0;
                            }
                            else
                            {
                                objdecimal = Convert.ToDecimal(dsWeight.Tables[0].Rows[0]["weight"].ToString());
                            }
                        }
                        if (ViewState["Weight"] != null)
                        {
                            objdecimal += Convert.ToDecimal(ViewState["Weight"].ToString());
                            ViewState["Weight"] = objdecimal.ToString();
                        }
                        else
                        {
                            ViewState["Weight"] = objdecimal.ToString();
                        }
                    }

                    Decimal SubTotal = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[0]["SubTotal"].ToString());
                    ViewState["SubTotal"] = SubTotal.ToString();
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Subtotal :";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td align=\"right\" valign=\"middle\">";
                    if (Session["QtyDiscount"] != null && Convert.ToDecimal(Session["QtyDiscount"].ToString()) > Decimal.Zero)
                    {
                        ltrCart.Text += "<s>$" + String.Format("{0:0.00}", SubTotal) + "</s><br />";
                        SubTotal = SubTotal - Convert.ToDecimal(Session["QtyDiscount"].ToString());
                        ltrCart.Text += "$" + String.Format("{0:0.00}", SubTotal);
                    }
                    else
                    {
                        ltrCart.Text += "$" + String.Format("{0:0.00}", SubTotal);
                    }
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Discount :";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td align=\"right\" valign=\"middle\">";
                    if (Session["Discount"] != null && Session["Discount"].ToString() != "")
                    {
                        ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(Session["Discount"].ToString()));
                        SubTotal = SubTotal - Convert.ToDecimal(Session["Discount"].ToString());
                    }
                    else
                    {
                        ltrCart.Text += "$0.00";
                    }
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                    if (ViewState["CustomerLevelID"] != null && Convert.ToInt32(ViewState["CustomerLevelID"].ToString()) > 0)
                    {
                        ltrCart.Text += "<tr>";
                        ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                        ltrCart.Text += "Customer Level Discount :";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "<td align=\"right\" valign=\"middle\">###customlevelDiscount###";
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";
                    }
                    //if (Session["QtyDiscount"] != null && Convert.ToDecimal(Session["QtyDiscount"].ToString()) > Decimal.Zero)
                    //{
                    //    ltrCart.Text += "<tr>";
                    //    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    //    ltrCart.Text += "Quantity Discount :";
                    //    ltrCart.Text += "</td>";
                    //    ltrCart.Text += "<td align=\"right\" valign=\"middle\">" + String.Format("{0:0.00}", Convert.ToDecimal(Session["QtyDiscount"].ToString())) + "";
                    //    ltrCart.Text += "</td>";
                    //    ltrCart.Text += "</tr>";
                    //}

                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Shipping :";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td align=\"right\" valign=\"middle\">";
                    string strShippingName = "";
                    string strShippingCharge = "0.00";

                    if (ViewState["CustomerLevelID"] != null && ViewState["CustomerLevelFreeShipping"] == null)
                    {
                        bool IsFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasFreeShipping,0) AS LevelHasFreeShipping FROM dbo.tb_CustomerLevel WHERE CustomerLevelID=" + Convert.ToInt32(ViewState["CustomerLevelID"]) + ""));

                        if (IsFreeShipping)
                        {
                            ViewState["CustomerLevelFreeShipping"] = "1";
                        }
                        else
                        {
                            if (rdoShippingMethod.SelectedIndex > -1)
                            {
                                strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();

                                if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                                {
                                    strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                                    strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();
                                    ViewState["strShippingCharge"] = strShippingCharge.ToString();
                                }
                            }
                        }
                    }
                    else
                    {
                        if (rdoShippingMethod.SelectedIndex > -1)
                        {
                            strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();

                            if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                            {
                                strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                                strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();
                                ViewState["strShippingCharge"] = strShippingCharge.ToString();
                            }
                        }
                    }

                    if (strShippingCharge.ToString() != "")
                    {
                        ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(strShippingCharge.ToString()));
                        SubTotal = SubTotal + Convert.ToDecimal(strShippingCharge.ToString());
                    }
                    else
                    {
                        ltrCart.Text += "$0.00";
                    }
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<td colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "Sale Tax :";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td align=\"right\" valign=\"middle\">";

                    #region Custom Level Discount Start

                    Decimal CustomerlevelDiscount = Decimal.Zero;
                    if (ViewState["CustomerLevelID"] != null && Convert.ToInt32(ViewState["CustomerLevelID"].ToString()) > 0 && Session["CustomLevelDiscount"] == null)
                    {
                        DataSet dsDiscount = new DataSet();
                        dsDiscount = CommonComponent.GetCommonDataSet("SELECT isnull(LevelDiscountPercent,0) as LevelDiscountPercent,isnull(LevelDiscountAmount,0) as LevelDiscountAmount FROM tb_CustomerLevel WHERE CustomerLevelID=" + ViewState["CustomerLevelID"].ToString() + "");
                        if (dsDiscount != null && dsDiscount.Tables.Count > 0 && dsDiscount.Tables[0].Rows.Count > 0)
                        {
                            Decimal CustomePersantage = Convert.ToDecimal(dsDiscount.Tables[0].Rows[0]["LevelDiscountPercent"].ToString());
                            if (CustomePersantage == Decimal.Zero)
                            {
                                CustomerlevelDiscount = Convert.ToDecimal(dsDiscount.Tables[0].Rows[0]["LevelDiscountAmount"].ToString());
                            }
                            else
                            {
                                if (ViewState["SubTotal"] != null)
                                {
                                    CustomerlevelDiscount = (Convert.ToDecimal(ViewState["SubTotal"]) * CustomePersantage) / 100;
                                }
                            }
                            Session["CustomLevelDiscount"] = CustomerlevelDiscount.ToString();
                        }
                    }
                    else
                    {
                        if (Session["CustomLevelDiscount"] != null)
                        {
                            CustomerlevelDiscount = Convert.ToDecimal(Session["CustomLevelDiscount"]);
                        }
                    }
                    //if (Session["QtyDiscount"] != null && Convert.ToDecimal(Session["QtyDiscount"].ToString()) > Decimal.Zero)
                    //{
                    //    SubTotal = SubTotal - Convert.ToDecimal(Session["QtyDiscount"].ToString());
                    //}
                    SubTotal = SubTotal - CustomerlevelDiscount;

                    decimal OrderTax = decimal.Zero;
                    if (Session["CustID"] != null && Session["CustID"].ToString() != null && Session["UserName"] != null && Session["UserName"].ToString() != "")
                    {
                        //OrderTax = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), SubTotal);
                        //ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);

                        bool IsFreeTax = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT ISNULL(LevelHasnoTax,0) AS LevelHasnoTax FROM dbo.tb_CustomerLevel WHERE CustomerLevelID=" + ViewState["CustomerLevelID"].ToString() + ""));
                        if (IsFreeTax)
                        {
                            ltrCart.Text += "$" + String.Format("{0:0.00}", 0);
                        }
                        else
                        {
                            OrderTax = SaleTax(hdnState.Value.ToString(), hdnZipCode.Value.ToString(), SubTotal);
                            ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                        }
                    }
                    else
                    {
                        if (UseShippingAddress.Checked == false && ddlBillstate.SelectedIndex > 0)
                        {
                            if (ddlBillstate.SelectedValue.ToString() == "-11")
                            {
                                OrderTax = SaleTax(txtBillother.Text.ToString(), txtBillZipCode.Text.ToString(), SubTotal);
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            }
                            else
                            {
                                OrderTax = SaleTax(ddlBillstate.SelectedItem.Text.ToString(), txtBillZipCode.Text.ToString(), SubTotal);
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            }
                        }
                        else if (UseShippingAddress.Checked && ddlShipState.SelectedIndex > 0)
                        {
                            if (ddlShipState.SelectedValue.ToString() == "-11")
                            {
                                OrderTax = SaleTax(txtShipOther.Text.ToString(), txtShipZipCode.Text.ToString(), SubTotal);
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            }
                            else
                            {
                                OrderTax = SaleTax(ddlShipState.SelectedItem.Text.ToString(), txtShipZipCode.Text.ToString(), SubTotal);
                                ltrCart.Text += "$" + String.Format("{0:0.00}", OrderTax);
                            }
                        }
                        else
                        {
                            ltrCart.Text += "$0.00";
                        }

                    }
                    SubTotal = SubTotal + OrderTax;
                    ltrCart.Text = ltrCart.Text.ToString().Replace("###customlevelDiscount###", "$" + String.Format("{0:0.00}", Convert.ToDecimal(CustomerlevelDiscount.ToString())));

                    #endregion

                    if (SubTotal < 0)
                    {
                        ViewState["OrderTotal"] = 0;
                    }
                    else
                    {
                        ViewState["OrderTotal"] = SubTotal.ToString();
                    }
                    FinalTotal = Convert.ToDecimal(SubTotal.ToString());
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                    ltrCart.Text += "<tr>";
                    ltrCart.Text += "<th colspan=\"3\" align=\"right\" valign=\"top\">";
                    ltrCart.Text += "<strong>Total :</strong>";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "<th align=\"right\" valign=\"middle\">";
                    ltrCart.Text += "<strong>$" + String.Format("{0:0.00}", SubTotal) + "</strong>";
                    ltrCart.Text += "</th>";
                    ltrCart.Text += "</tr>";
                    ltrCart.Text += "</tbody></table>";
                }
                else
                {
                    ltrCart.Text = "Your Cart is Empty.";
                }
            }
        }

        /// <summary>
        /// Get Sales Tax By ZipCode or StateName
        /// </summary>
        /// <param name="stateName">string StateName</param>
        /// <param name="zipCode">String ZipCode</param>
        /// <param name="orderAmount">string OrderAmount</param>
        /// <returns>Returns Sale Tax as a Decimal Value</returns>
        private decimal SaleTax(string stateName, string zipCode, decimal orderAmount)
        {
            ShoppingCartComponent objTax = new ShoppingCartComponent();
            decimal salesTax = decimal.Zero;
            salesTax = Convert.ToDecimal(objTax.GetSalesTax(stateName, zipCode, orderAmount, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
            Session["SalesTax"] = salesTax.ToString();
            return salesTax;
        }

        #region Place Order

        /// <summary>
        ///  Place Order Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnPlaceOrder_Click(object sender, ImageClickEventArgs e)
        {
            if (rdoShippingMethod != null && rdoShippingMethod.Items.Count == 0)
            {
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && (Session["UserName"] == null || Session["UserName"].ToString() == ""))
                {
                    GetShippingMethodByBillAddress();
                }
                BindShippingMethod();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select Shipping Method.');", true);
                return;
            }
            else
            {
                if (rdoShippingMethod.SelectedIndex < 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select Shipping Method.');", true);
                    return;
                }
            }
            if (Session["PaymentGateway"] == null || Session["PaymentGateway"].ToString() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Payment Method Not Found.');", true);
                return;
            }
            if (Session["CustID"] != null && Session["CustID"].ToString() != null && Session["UserName"] != null && Session["UserName"].ToString() != "")
            {
                AddOrderWithLogin();
            }
            else
            {
                AddOrderWithoutLogin();
            }
        }

        /// <summary>
        /// Order Placing With Login user
        /// </summary>
        private void AddOrderWithLogin()
        {
            if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
            {
                if (txtNameOnCard.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Name of Card.');", true);
                    txtNameOnCard.Focus();
                    return;
                }
                else if (ddlCardType.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select Card Type.');", true);
                    ddlCardType.Focus();
                    return;
                }
                else if (txtCardNumber.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Card Number.');", true);
                    txtCardNumber.Focus();
                    return;
                }
                else if (txtCSC.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Card Verification Code.');", true);
                    txtCSC.Focus();
                    return;
                }
                if (txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().IndexOf("*") < -1)
                {
                    bool chkflg = false;
                    long crdNumber = 0;
                    chkflg = long.TryParse(txtCardNumber.Text.ToString(), out crdNumber);
                    if (!chkflg)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter valid Numeric Card Number.');", true);
                        txtCardNumber.Focus();
                        return;
                    }
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().Length < 16)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Credit Card Number must be 16 digit long.');", true);
                    txtCardNumber.Focus();
                    return;
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCardNumber.Text.ToString().Trim() != "" && (txtCardNumber.Text.ToString().Trim().Length < 15 || txtCardNumber.Text.ToString().Trim().Length > 15))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Credit Card Number must be 15 digit long.');", true);
                    txtCardNumber.Focus();
                    return;
                }

                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCSC.Text.ToString().Trim() != "" && (txtCSC.Text.ToString().Trim().Length < 3 || txtCSC.Text.ToString().Trim().Length > 3))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Card Verification Code must be 4 digit long.');", true);
                    txtCSC.Focus();
                    return;
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCSC.Text.ToString().Trim() != "" && txtCSC.Text.ToString().Trim().Length < 4)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Card Verification Code must be 3 digit long.');", true);
                    txtCSC.Focus();
                    return;
                }
                if (ddlMonth.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select Month.');", true);
                    ddlMonth.Focus();
                    return;
                }
                else if (ddlYear.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select Year.');", true);
                    ddlYear.Focus();
                    return;
                }
                else if ((Convert.ToInt32(ddlYear.SelectedValue) > DateTime.Now.Year) || (Convert.ToInt32(ddlYear.SelectedValue) == DateTime.Now.Year && Convert.ToInt32(ddlMonth.SelectedValue) >= DateTime.Now.Month))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Valid Expiration Date.');", true);
                    return;
                }
            }
            string strShippingName = "";
            string strShippingCharge = "0.00";
            if (rdoShippingMethod.SelectedIndex > -1)
            {
                strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();

                if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                {
                    strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                    strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();

                    strShippingName = strShippingName.Substring(0, strShippingName.ToString().ToLower().IndexOf("($"));
                    strShippingName = strShippingName.ToString().Trim();
                }
            }

            BindOrderSummary();
            tb_Order objOrder = new tb_Order();
            objOrder.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
            if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
            {
                objOrder.CardType = ddlCardType.SelectedItem.Text.ToString();
                objOrder.CardVarificationCode = txtCSC.Text.ToString();
                objOrder.CardName = txtNameOnCard.Text.ToString();
                objOrder.CardExpirationMonth = ddlMonth.SelectedValue.ToString();
                objOrder.CardExpirationYear = ddlYear.SelectedValue.ToString();
                if (txtCardNumber.Text.ToString().IndexOf("*") > -1)
                {
                    if (Session["CardNumber"] != null)
                    {
                        objOrder.CardNumber = SecurityComponent.Encrypt(Session["CardNumber"].ToString());
                    }
                }
                else
                {
                    objOrder.CardNumber = SecurityComponent.Encrypt(txtCardNumber.Text.ToString());
                }
            }
            objOrder.ShippingMethod = strShippingName.ToString();
            objOrder.Deleted = false;
            if (Session["Discount"] != null && Session["Discount"].ToString() != "")
            {
                objOrder.CouponDiscountAmount = Convert.ToDecimal(Session["Discount"].ToString());
            }
            else
            {
                objOrder.CouponDiscountAmount = decimal.Zero;
            }
            if (Session["QtyDiscount"] != null && Session["QtyDiscount"].ToString() != "")
            {
                objOrder.QuantityDiscountAmount = Convert.ToDecimal(Session["QtyDiscount"].ToString());
            }
            else
            {
                objOrder.QuantityDiscountAmount = decimal.Zero;
            }

            if (Session["CouponCode"] != null && Session["CouponCode"].ToString() != "")
            {
                objOrder.CouponCode = Convert.ToString(Session["CouponCode"].ToString());
            }
            else
            {
                objOrder.CouponCode = "";
            }
            objOrder.Notes = txtOrderNotes.Text.ToString();
            objOrder.OrderNotes = txtOrderNotes.Text.ToString();
            if (ViewState["OrderTotal"] != null)
            {
                objOrder.OrderTotal = Convert.ToDecimal(ViewState["OrderTotal"].ToString());
            }
            if (ViewState["SubTotal"] != null)
            {
                objOrder.OrderSubtotal = Convert.ToDecimal(ViewState["SubTotal"].ToString());
            }
            if (ViewState["strShippingCharge"] != null)
            {
                objOrder.OrderShippingCosts = Convert.ToDecimal(ViewState["strShippingCharge"].ToString());
            }
            if (Session["SalesTax"] != null)
            {
                objOrder.OrderTax = Convert.ToDecimal(Session["SalesTax"].ToString());
            }
            if (Session["PaymentMethod"] != null)
            {
                objOrder.PaymentMethod = Session["PaymentMethod"].ToString().ToUpper();
            }
            if (Session["PaymentGateway"] != null)
            {
                objOrder.PaymentGateway = Session["PaymentGateway"].ToString().ToUpper();
            }
            if (Session["CustomLevelDiscount"] != null)
            {
                objOrder.LevelDiscountAmount = Convert.ToDecimal(Session["CustomLevelDiscount"].ToString());
            }
            objOrder.IsNew = true;
            objOrder.LastIPAddress = Request.UserHostAddress.ToString();
            OrderComponent objAddOrder = new OrderComponent();
            Int32 OrderNumber = 0;
            OrderNumber = Convert.ToInt32(objAddOrder.AddOrder(objOrder, OrderNumber, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
            if (OrderNumber > 0)
            {
                try
                {
                    OrderComponent objOrderlog = new OrderComponent();
                    objOrderlog.InsertOrderlog(1, Convert.ToInt32(OrderNumber), "", Convert.ToInt32(Session["CustID"].ToString()));
                }
                catch { }
                string strResult = OrderPayment(Session["PaymentGateway"].ToString().ToUpper(), OrderNumber, Convert.ToDecimal(objOrder.OrderTotal), GetCustomerDetailsForpayment(Convert.ToInt32(Session["CustID"].ToString())));
                if (strResult.ToUpper() == "OK")
                {
                    Session["PaymentGateway"] = null;
                    Session["PaymentGatewayStatus"] = null;
                    Session["SalesTax"] = null;
                    Session["CouponCode"] = null;
                    Session["Discount"] = null;
                    Session["PaymentMethod"] = null;
                    Session["ONo"] = OrderNumber.ToString();
                    Session["NoOfCartItems"] = null;
                    Session["SelectedCountry"] = null;
                    Session["SelectedZipCode"] = null;
                    Session["SelectedShippingMethod"] = null;
                    Session["CustomLevelDiscount"] = null;

                    if (Session["CustomerQuoteID"] != null)
                        CommonComponent.ExecuteCommonData("update tb_customerquote set OrderNumber=" + OrderNumber + " where CustomerQuoteID="
                            + Convert.ToInt32(SecurityComponent.Decrypt(Session["CustomerQuoteID"].ToString())));

                    Response.Redirect("/orderreceived.aspx");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('" + strResult.ToString() + "');", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Billing Email Address Already Exists, Please Enter different Email Address.');", true);
                return;
            }
        }

        /// <summary>
        /// Order Placing Without Login user
        /// </summary>
        private void AddOrderWithoutLogin()
        {
            if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
            {
                if (txtNameOnCard.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Name of Card.');", true);
                    txtNameOnCard.Focus();
                    return;
                }
                else if (ddlCardType.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select Card Type.');", true);
                    ddlCardType.Focus();
                    return;
                }
                else if (txtCardNumber.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Card Number.');", true);
                    txtCardNumber.Focus();
                    return;
                }
                else if (txtCSC.Text.ToString().Trim() == "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Card Verification Code.');", true);
                    txtCSC.Focus();
                    return;
                }
                if (txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().IndexOf("*") < -1)
                {
                    bool chkflg = false;
                    long crdNumber = 0;
                    chkflg = long.TryParse(txtCardNumber.Text.ToString(), out crdNumber);
                    if (!chkflg)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter valid Numeric Card Number');", true);
                        txtCardNumber.Focus();
                        return;
                    }
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCardNumber.Text.ToString().Trim() != "" && txtCardNumber.Text.ToString().Trim().Length < 16)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Credit Card Number must be 16 digit long.');", true);
                    txtCardNumber.Focus();
                    return;
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCardNumber.Text.ToString().Trim() != "" && (txtCardNumber.Text.ToString().Trim().Length < 15 || txtCardNumber.Text.ToString().Trim().Length > 15))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Credit Card Number must be 15 digit long.');", true);
                    txtCardNumber.Focus();
                    return;
                }

                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() != "amex" && ddlCardType.SelectedItem.Text.ToString().ToLower() != "american express") && txtCSC.Text.ToString().Trim() != "" && (txtCSC.Text.ToString().Trim().Length < 3 || txtCSC.Text.ToString().Trim().Length > 3))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Card Verification Code must be 4 digit long.');", true);
                    txtCSC.Focus();
                    return;
                }
                if ((ddlCardType.SelectedItem.Text.ToString().ToLower() == "amex" || ddlCardType.SelectedItem.Text.ToString().ToLower() == "american express") && txtCSC.Text.ToString().Trim() != "" && txtCSC.Text.ToString().Trim().Length < 4)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Card Verification Code must be 3 digit long.');", true);
                    txtCSC.Focus();
                    return;
                }
                if (ddlMonth.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select Month.');", true);
                    ddlMonth.Focus();
                    return;
                }
                else if (ddlYear.SelectedIndex == 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select Year.');", true);
                    ddlYear.Focus();
                    return;
                }
                else if ((Convert.ToInt32(ddlYear.SelectedValue) > DateTime.Now.Year) || (Convert.ToInt32(ddlYear.SelectedValue) == DateTime.Now.Year && Convert.ToInt32(ddlMonth.SelectedValue) >= DateTime.Now.Month))
                {

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Enter Valid Expiration Date.');", true);
                    return;
                }
            }
            string strShippingName = "";
            string strShippingCharge = "0.00";
            if (rdoShippingMethod.SelectedIndex > -1)
            {
                strShippingName = rdoShippingMethod.SelectedItem.Text.ToString();

                if (strShippingName.ToString().ToLower().IndexOf("($") > -1)
                {
                    strShippingCharge = strShippingName.Substring(strShippingName.ToString().ToLower().IndexOf("($") + 2, strShippingName.ToString().Length - strShippingName.ToString().ToLower().IndexOf("($") - 2);
                    strShippingCharge = strShippingCharge.Replace("(", "").Replace("$", "").Replace(")", "").Trim();

                    strShippingName = strShippingName.Substring(0, strShippingName.ToString().ToLower().IndexOf("($"));
                    strShippingName = strShippingName.ToString().Trim();
                }
            }

            BindOrderSummary();
            tb_Order objOrder = new tb_Order();
            objOrder.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
            objOrder.FirstName = txtBillFirstname.Text.ToString();
            objOrder.LastName = txtBillLastname.Text.ToString();
            objOrder.Email = txtBillEmail.Text.ToString();
            objOrder.BillingFirstName = txtBillFirstname.Text.ToString();
            objOrder.BillingLastName = txtBillLastname.Text.ToString();
            objOrder.BillingEmail = txtBillEmail.Text.ToString();
            objOrder.BillingAddress1 = txtBillAddressLine1.Text.ToString();
            objOrder.BillingAddress2 = txtBillAddressLine2.Text.ToString();
            objOrder.BillingCity = txtBillCity.Text.ToString();
            objOrder.BillingState = ddlBillstate.SelectedItem.Text.ToString();
            objOrder.BillingCountry = ddlBillcountry.SelectedItem.Text.ToString();
            objOrder.BillingPhone = txtBillphone.Text.ToString();
            objOrder.BillingZip = txtBillZipCode.Text.ToString();
            objOrder.BillingSuite = txtBillSuite.Text.ToString();
            objOrder.BillingCompany = "";
            Session["BillEmail"] = Convert.ToString(txtBillEmail.Text.ToString());
            if (UseShippingAddress.Checked == false)
            {
                objOrder.ShippingFirstName = txtBillFirstname.Text.ToString();
                objOrder.ShippingLastName = txtBillLastname.Text.ToString();
                objOrder.ShippingEmail = txtBillEmail.Text.ToString();
                objOrder.ShippingAddress1 = txtBillAddressLine1.Text.ToString();
                objOrder.ShippingAddress2 = txtBillAddressLine2.Text.ToString();
                objOrder.ShippingCity = txtBillCity.Text.ToString();
                objOrder.ShippingState = ddlBillstate.SelectedItem.Text.ToString();
                objOrder.ShippingCountry = ddlBillcountry.SelectedItem.Text.ToString();
                objOrder.ShippingPhone = txtBillphone.Text.ToString();
                objOrder.ShippingZip = txtBillZipCode.Text.ToString();
                objOrder.ShippingSuite = txtBillSuite.Text.ToString();
                objOrder.ShippingCompany = "";
            }
            else
            {
                objOrder.ShippingFirstName = txtShipFirstName.Text.ToString();
                objOrder.ShippingLastName = txtShipLastName.Text.ToString();
                objOrder.ShippingEmail = txtShipEmailAddress.Text.ToString();
                objOrder.ShippingAddress1 = txtshipAddressLine1.Text.ToString();
                objOrder.ShippingAddress2 = txtshipAddressLine2.Text.ToString();
                objOrder.ShippingCity = txtShipCity.Text.ToString();
                objOrder.ShippingState = ddlShipState.SelectedItem.Text.ToString();
                objOrder.ShippingCountry = ddlShipCounry.SelectedItem.Text.ToString();
                objOrder.ShippingPhone = txtShipPhone.Text.ToString();
                objOrder.ShippingZip = txtShipZipCode.Text.ToString();
                objOrder.ShippingSuite = txtShipSuite.Text.ToString();
                objOrder.ShippingCompany = "";
            }

            if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower() == "creditcard")
            {
                objOrder.CardType = ddlCardType.SelectedItem.Text.ToString();
                objOrder.CardVarificationCode = txtCSC.Text.ToString();
                objOrder.CardName = txtNameOnCard.Text.ToString();
                objOrder.CardExpirationMonth = ddlMonth.SelectedValue.ToString();
                objOrder.CardExpirationYear = ddlYear.SelectedValue.ToString();
                objOrder.CardNumber = SecurityComponent.Encrypt(txtCardNumber.Text.ToString());
            }
            objOrder.ShippingMethod = strShippingName.ToString();
            objOrder.Deleted = false;
            if (Session["Discount"] != null && Session["Discount"].ToString() != "")
            {
                objOrder.CouponDiscountAmount = Convert.ToDecimal(Session["Discount"].ToString());
            }
            else
            {
                objOrder.CouponDiscountAmount = decimal.Zero;
            }
            if (Session["CouponCode"] != null && Session["CouponCode"].ToString() != "")
            {
                objOrder.CouponCode = Convert.ToString(Session["CouponCode"].ToString());
            }
            else
            {
                objOrder.CouponCode = "";
            }
            objOrder.Notes = txtOrderNotes.Text.ToString();
            objOrder.OrderNotes = txtOrderNotes.Text.ToString();
            if (ViewState["OrderTotal"] != null)
            {
                objOrder.OrderTotal = Convert.ToDecimal(ViewState["OrderTotal"].ToString());
            }
            if (ViewState["SubTotal"] != null)
            {
                objOrder.OrderSubtotal = Convert.ToDecimal(ViewState["SubTotal"].ToString());
            }
            if (ViewState["strShippingCharge"] != null)
            {
                objOrder.OrderShippingCosts = Convert.ToDecimal(ViewState["strShippingCharge"].ToString());
            }
            if (Session["SalesTax"] != null)
            {
                objOrder.OrderTax = Convert.ToDecimal(Session["SalesTax"].ToString());
            }
            if (Session["PaymentMethod"] != null)
            {
                objOrder.PaymentMethod = Session["PaymentMethod"].ToString().ToUpper();
            }
            if (Session["PaymentGateway"] != null)
            {
                objOrder.PaymentGateway = Session["PaymentGateway"].ToString().ToUpper();
            }
            objOrder.IsNew = true;
            objOrder.LastIPAddress = Request.UserHostAddress.ToString();
            OrderComponent objAddOrder = new OrderComponent();
            Int32 OrderNumber = 0;

            OrderNumber = Convert.ToInt32(objAddOrder.AddOrder(objOrder, OrderNumber, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));

            if (OrderNumber > 0)
            {
                try
                {
                    OrderComponent objOrderlog = new OrderComponent();
                    objOrderlog.InsertOrderlog(1, Convert.ToInt32(OrderNumber), "", Convert.ToInt32(Session["CustID"].ToString()));
                }
                catch { }
                objOrder.CardNumber = SecurityComponent.Decrypt(objOrder.CardNumber.ToString());
                string strResult = OrderPayment(Session["PaymentGateway"].ToString().ToUpper(), OrderNumber, Convert.ToDecimal(objOrder.OrderTotal), objOrder);
                if (strResult.ToUpper() == "OK")
                {
                    Session["PaymentGateway"] = null;
                    Session["PaymentGatewayStatus"] = null;
                    Session["SalesTax"] = null;
                    Session["CouponCode"] = null;
                    Session["Discount"] = null;
                    Session["PaymentMethod"] = null;
                    Session["ONo"] = OrderNumber.ToString();
                    Session["NoOfCartItems"] = null;
                    Session["SelectedCountry"] = null;
                    Session["SelectedZipCode"] = null;
                    Session["SelectedShippingMethod"] = null;

                    if (Session["CustomerQuoteID"] != null)
                        CommonComponent.ExecuteCommonData("update tb_customerquote set OrderNumber=" + OrderNumber + " where CustomerQuoteID="
                            + Convert.ToInt32(SecurityComponent.Decrypt(Session["CustomerQuoteID"].ToString())));

                    Response.Redirect("/orderreceived.aspx");
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('" + strResult.ToString() + "');", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Billing Email Address Already Exists, Please Enter different Email Address.');", true);
                return;
            }
        }

        /// <summary>
        /// Order Payment Transaction
        /// </summary>
        /// <param name="PayementGateWay">String PayementGateWay</param>
        /// <param name="OrderNumber">String OrderNumber</param>
        /// <returns>Returns the result as a String according to execution</returns>
        private string OrderPayment(string PayementGateWay, Int32 OrderNumber, decimal orderTotal, tb_Order objorder)
        {
            LinkpointComponent objLinkpoint = new LinkpointComponent();
            tb_Order objOrder = new tb_Order();
            String AVSResult = String.Empty;
            String AuthorizationResult = String.Empty;
            String AuthorizationCode = String.Empty;
            String AuthorizationTransID = String.Empty;
            String TransactionCommand = String.Empty;
            String TransactionResponse = String.Empty;
            string Status = "";
            OrderComponent objDsorder = new OrderComponent();
            DataSet dsOrder = new DataSet();
            dsOrder = objDsorder.GetOrderDetailsByOrderID(OrderNumber);

            if (orderTotal > decimal.Zero)
            {
                if (Session["PaymentMethod"] != null && Session["PaymentMethod"].ToString().ToLower().Trim() == "creditcard")
                {
                    if (Session["PaymentGateway"] != null && Session["PaymentGateway"].ToString().ToLower().Trim() == "linkpoint")
                    {
                        Status = "OK";
                        //Status = objLinkpoint.ProcessCard(OrderNumber, Session["PaymentGatewayStatus"].ToString().ToLower(), "", "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (Session["PaymentGateway"] != null && Session["PaymentGateway"].ToString().ToLower().Trim() == "paypal")
                    {
                        PayPalComponent objPaypal = new PayPalComponent();
                        Status = objPaypal.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(Session["CustID"].ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), Session["PaymentGatewayStatus"].ToString(), objorder, "", objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                    else if (Session["PaymentGateway"] != null && Session["PaymentGateway"].ToString().ToLower().Trim() == "authorizenet")
                    {
                        Status = AuthorizeNetComponent.ProcessCardForClientSide(OrderNumber, Convert.ToInt32(Session["CustID"].ToString()), orderTotal, AppLogic.AppConfigBool("UseLiveTransactions"), Session["PaymentGatewayStatus"].ToString(), objorder, objorder, "", "", "", out AVSResult, out AuthorizationResult, out AuthorizationCode, out AuthorizationTransID, out TransactionCommand, out TransactionResponse);
                    }
                }
            }
            else
            {
                Status = "OK";
            }

            objOrder = new tb_Order();
            if (Status.ToUpper() == "OK")
            {
                objOrder.OrderNumber = OrderNumber;
                objOrder.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
                objOrder.AVSResult = AVSResult;
                objOrder.AuthorizationResult = AuthorizationResult;
                objOrder.AuthorizationCode = AuthorizationCode;
                objOrder.AuthorizationPNREF = AuthorizationTransID;
                objOrder.TransactionCommand = TransactionCommand;
                if (Session["PaymentGatewayStatus"] != null && Session["PaymentGatewayStatus"].ToString().ToLower().IndexOf("auth") > -1)
                {
                    objOrder.TransactionStatus = "AUTHORIZED";
                    objOrder.AuthorizedOn = DateTime.Now;
                }
                else
                {
                    objOrder.TransactionStatus = "CAPTURED";
                    objOrder.CapturedOn = DateTime.Now;
                }
                OrderComponent objUpdateOrder = new OrderComponent();
                Int32 updateOrder = Convert.ToInt32(objUpdateOrder.AddOrder(objOrder, OrderNumber, Convert.ToInt32(AppLogic.AppConfigs("StoreID"))));
                SendMail(OrderNumber);
            }
            else
            {
                objDsorder = new OrderComponent();
                tb_FailedTransaction objFailed = new tb_FailedTransaction();
                objFailed.OrderNumber = Convert.ToInt32(OrderNumber);
                objFailed.CustomerID = Convert.ToInt32(Session["CustID"].ToString());
                objFailed.PaymentGateway = Convert.ToString(Session["PaymentGateway"].ToString());
                objFailed.Paymentmethod = Convert.ToString(Session["PaymentMethod"].ToString());
                objFailed.TransactionCommand = Convert.ToString(TransactionCommand);
                objFailed.TransactionResult = Convert.ToString(TransactionResponse);
                objFailed.OrderDate = DateTime.Now;
                objFailed.IPAddress = Request.UserHostAddress.ToString();
                Int32 FaileId = Convert.ToInt32(objDsorder.AddOrderFailedTransaction(objFailed, Convert.ToInt32(1)));
                SendMailForFailedTransaction(OrderNumber);
            }

            return Status;
        }

        /// <summary>
        /// Sends the Mail for Failed Transaction.
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        public void SendMailForFailedTransaction(Int32 OrderNumber)
        {
            string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
            string Body = "";
            string url = AppLogic.AppConfigs("LIVE_SERVER") + "/" + "FailedTransactionEmail.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
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
                CommonOperations.SendMail(ToID, "Failed Order Transaction for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
            }
            catch { }
        }
        #endregion

        /// <summary>
        /// Order Receipt Send To Customer & Admin
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNUmber</param>
        public void SendMail(Int32 OrderNumber)
        {

            string ToID = AppLogic.AppConfigs("MailMe_ToAddress");
            string Body = "";
            //      string url = Request.Url.Scheme + "://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/') + "/" + "invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
            string url = AppLogic.AppConfigs("LIVE_SERVER") + "/" + "invoice.aspx?ONo=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNumber.ToString()));
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
                CommonOperations.SendMail(ToID, "New Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
            }
            catch { }
            try
            {
                if (ViewState["BillEmail"] != null)
                {
                    CommonOperations.SendMail(ViewState["BillEmail"].ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                }
                else
                {
                    CommonOperations.SendMail(txtBillEmail.Text.ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                }
            }
            catch { }
        }

        /// <summary>
        /// Bind both Country Drop down list
        /// </summary>
        public void FillCountry()
        {
            ddlBillcountry.Items.Clear();
            ddlShipCounry.Items.Clear();

            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();

            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlBillcountry.DataSource = dscountry.Tables[0];
                ddlBillcountry.DataTextField = "Name";
                ddlBillcountry.DataValueField = "CountryID";
                ddlBillcountry.DataBind();
                ddlBillcountry.Items.Insert(0, new ListItem("Select Country", "0"));

                ddlShipCounry.DataSource = dscountry.Tables[0];
                ddlShipCounry.DataTextField = "Name";
                ddlShipCounry.DataValueField = "CountryID";
                ddlShipCounry.DataBind();
                ddlShipCounry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
            else
            {
                ddlBillcountry.DataSource = null;
                ddlBillcountry.DataBind();
                ddlShipCounry.DataSource = null;
                ddlShipCounry.DataBind();
            }

            if (ddlBillcountry.Items.FindByText("United States") != null)
            {
                ddlBillcountry.Items.FindByText("United States").Selected = true;
            }
            if (ddlShipCounry.Items.FindByText("United States") != null)
            {
                ddlShipCounry.Items.FindByText("United States").Selected = true;
            }

            ddlBillcountry_SelectedIndexChanged(null, null);
            ddlShipCounry_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Billing Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlBillcountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBillstate.Items.Clear();
            if (ddlBillcountry.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlBillcountry.SelectedValue.ToString()));

                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlBillstate.DataSource = dsState.Tables[0];
                    ddlBillstate.DataTextField = "Name";
                    ddlBillstate.DataValueField = "StateID";
                    ddlBillstate.DataBind();
                    ddlBillstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlBillstate.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    ddlBillstate.SelectedIndex = 0;
                }
                else
                {
                    ddlBillstate.DataSource = null;
                    ddlBillstate.DataBind();
                    ddlBillstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlBillstate.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlBillstate.SelectedIndex = 0;
                }
            }
            else
            {
                ddlBillstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlBillstate.Items.Insert(1, new ListItem("Other", "-11"));
                ddlBillstate.SelectedIndex = 0;
            }
            if (UseShippingAddress.Checked)
            {
                pnlShippingDetails.Attributes.Add("style", "display:block");
            }
            else
            {
                pnlShippingDetails.Attributes.Add("style", "display:none");
            }
            if (UseShippingAddress.Checked == false)
            {
                GetShippingMethodByBillAddress();
                BindShippingMethod();
                BindOrderSummary();
            }
            StateOtherSelection();
        }

        #region Bind Shipping State

        /// <summary>
        /// Shipping Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlShipCounry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlShipState.Items.Clear();
            if (ddlShipCounry.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlShipCounry.SelectedValue.ToString()));
                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlShipState.DataSource = dsState.Tables[0];
                    ddlShipState.DataTextField = "Name";
                    ddlShipState.DataValueField = "StateID";
                    ddlShipState.DataBind();
                    ddlShipState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlShipState.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    ddlShipState.SelectedIndex = 0;
                }
                else
                {
                    ddlShipState.DataSource = null;
                    ddlShipState.DataBind();
                    ddlShipState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlShipState.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlShipState.SelectedIndex = 0;
                }
            }
            else
            {
                ddlShipState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlShipState.Items.Insert(1, new ListItem("Other", "-11"));
                ddlShipState.SelectedIndex = 0;
            }
            if (UseShippingAddress.Checked)
            {
                pnlShippingDetails.Attributes.Add("style", "display:block");
            }
            else
            {
                pnlShippingDetails.Attributes.Add("style", "display:none");
            }
            if (UseShippingAddress.Checked)
            {
                GetShippingMethodByShipAddress();
                BindShippingMethod();
                BindOrderSummary();
            }
            StateOtherSelection();
        }

        /// <summary>
        /// Bind Shipping Method By ZipCode
        /// </summary>
        /// <param name="Country">String CountryCode</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        private void BindShippingMethod()
        {
            bool CheckFreeShaippingOrder = false;
            string strUSPSMessage = "";
            string strUPSMessage = "";
            lblMsg.Text = "";
            StateComponent objState = new StateComponent();
            string stateName = Convert.ToString(objState.GetStateCodeByName(hdnState.Value.ToString()));

            rdoShippingMethod.Items.Clear();
            if (hdnZipCode.Value.ToString() == "" || stateName == "")
            {
                lblMsg.Visible = true;
                lblMsg.Text = "Select State/Province and Enter Zip Code";
                return;
            }

            ShippingComponent objShipping = new ShippingComponent();
            DataSet objShipServices = new DataSet();
            objShipServices = objShipping.GetShippingServices(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));


            CountryComponent objCountry = new CountryComponent();
            string CountryCode = Convert.ToString(objCountry.GetCountryCodeByName(hdncountry.Value.ToString()));
            decimal Weight = decimal.Zero;
            if (hdncountry.Value.ToString() == "0" || hdncountry.Value.ToString().Trim() == "")
            {
                return;
            }

            if (ViewState["Weight"] != null)
            {
                Weight = Convert.ToDecimal(ViewState["Weight"].ToString());
            }
            if (Weight == 0)
            {
                Weight = 1;

            }

            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));

            DataTable UPSTable = new DataTable();
            UPSTable.Columns.Add("ShippingMethodName", typeof(String));
            UPSTable.Columns.Add("Price", typeof(decimal));


            DataTable USPSTable = new DataTable();
            USPSTable.Columns.Add("ShippingMethodName", typeof(String));
            USPSTable.Columns.Add("Price", typeof(decimal));



            if (objShipServices != null && objShipServices.Tables.Count > 0 && objShipServices.Tables[0].Rows.Count > 0)
            {
                if (objShipServices.Tables[0].Select("ShippingService='UPS'").Length > 0)
                {
                    UPSTable = UPSMethodBind(CountryCode.ToString(), stateName, hdnZipCode.Value.ToString(), Weight, "UPS", ref strUPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='USPS'").Length > 0)
                {
                    EndiciaService objRate = new EndiciaService();
                    USPSTable = objRate.EndiciaGetRates(hdnZipCode.Value.ToString(), CountryCode.ToString(), Convert.ToDouble(Weight), ref strUSPSMessage);
                }
                if (objShipServices.Tables[0].Select("ShippingService='FEDEX'").Length > 0)
                { }
                if (UPSTable != null && UPSTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(UPSTable);
                }
                if (USPSTable != null && USPSTable.Rows.Count > 0)
                {
                    ShippingTable.Merge(USPSTable);
                }
                bool IsDiscountAllowIncludeFreeShipping = false;
                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    if (ViewState["CustomerLevelFreeShipping"] != null && Convert.ToString(ViewState["CustomerLevelFreeShipping"]) == "1")
                    {

                        if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                        {
                            String strFreeShipping = "Standard Shipping($0.00)";
                            DataRow dataRow = ShippingTable.NewRow();
                            dataRow["ShippingMethodName"] = strFreeShipping;
                            dataRow["Price"] = 0;
                            ShippingTable.Rows.Add(dataRow);
                            CheckFreeShaippingOrder = true;
                        }
                    }

                }

                if (CheckFreeShaippingOrder == false)
                {
                    if (Session["CouponCode"] != null && Session["CouponCode"].ToString() != "")
                    {
                        IsDiscountAllowIncludeFreeShipping = Convert.ToBoolean(CommonComponent.GetScalarCommonData("SELECT DiscountAllowIncludeFreeShipping FROM dbo.tb_Coupons WHERE CouponCode='" + Session["CouponCode"].ToString() + "'"));
                    }
                }

                if (CheckFreeShaippingOrder == false && IsDiscountAllowIncludeFreeShipping == true)
                {
                    String strFreeShipping = "Standard Shipping($0.00)";
                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = strFreeShipping;
                    dataRow["Price"] = 0;
                    ShippingTable.Rows.Add(dataRow);
                    CheckFreeShaippingOrder = true;
                }
                else if (CheckFreeShaippingOrder == false)
                {

                    if (FinalTotal > FreeShippingAmount)
                    {
                        if (ShippingTable != null && ShippingTable.Rows.Count > 0 && ShippingTable.Select("Price='0'").Length <= 0)
                        {
                            String strFreeShipping = "Standard Shipping($0.00)";
                            DataRow dataRow = ShippingTable.NewRow();
                            dataRow["ShippingMethodName"] = strFreeShipping;
                            dataRow["Price"] = 0;
                            ShippingTable.Rows.Add(dataRow);
                            CheckFreeShaippingOrder = true;

                        }
                    }
                }
                if (ShippingTable != null && ShippingTable.Rows.Count > 0)
                {
                    DataView dvShipping = ShippingTable.DefaultView;
                    dvShipping.Sort = "Price asc";
                    rdoShippingMethod.DataSource = dvShipping.ToTable();
                    rdoShippingMethod.DataTextField = "ShippingMethodName";
                    rdoShippingMethod.DataValueField = "ShippingMethodName";
                    rdoShippingMethod.DataBind();
                }


                if (Session["SelectedShippingMethod"] != null && Convert.ToString(Session["SelectedShippingMethod"]) != "")
                {
                    if (rdoShippingMethod.Items.Count > 0)
                    {
                        rdoShippingMethod.ClearSelection();
                        for (int i = 0; i < rdoShippingMethod.Items.Count; i++)
                        {
                            String[] strSplitValue = rdoShippingMethod.Items[i].Text.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            String[] strSplitSessionValue = Session["SelectedShippingMethod"].ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (strSplitValue[0] != null)
                            {
                                if (Convert.ToString(strSplitValue[0]) == Convert.ToString(strSplitSessionValue[0]))
                                {
                                    rdoShippingMethod.Items[i].Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    BindOrderSummary();
                }

                if (strUSPSMessage != "" && strUPSMessage != "")
                {
                    lblMsg.Text = strUPSMessage + strUSPSMessage;
                    lblMsg.Visible = true;
                }
                else if (strUSPSMessage != "")
                {
                    lblMsg.Text = strUSPSMessage;
                    lblMsg.Visible = true;
                }
                else if (strUPSMessage != "")
                {
                    lblMsg.Text = strUPSMessage;
                    lblMsg.Visible = true;
                }
            }


        }

        /// <summary>
        /// UPS Method Bind
        /// </summary>
        /// <param name="Country">String CountryCode</param>
        /// <param name="State">String State</param>
        /// <param name="ZipCode">String ZipCode</param>
        /// <param name="Weight">Decimal Weight</param>
        private DataTable UPSMethodBind(string Country, string State, string ZipCode, decimal Weight, string ServiceName, ref string StrMessage)
        {
            if (ZipCode == "" || State == "")
            {
                return null;
            }

            DataTable ShippingTable = new DataTable();
            ShippingTable.Columns.Add("ShippingMethodName", typeof(String));
            ShippingTable.Columns.Add("Price", typeof(decimal));


            UPS obj = new UPS(AppLogic.AppConfigs("Shipping.OriginAddress"), AppLogic.AppConfigs("Shipping.OriginAddress2"), AppLogic.AppConfigs("Shipping.OriginCity"), AppLogic.AppConfigs("Shipping.OriginState"), AppLogic.AppConfigs("Shipping.OriginZip"), AppLogic.AppConfigs("Shipping.OriginCountry"));
            obj.DestinationCountryCode = Country;
            obj.DestinationStateProvince = State;
            obj.DestinationZipPostalCode = ZipCode;
            StringBuilder tmpRealTimeShipping = new StringBuilder(4096);
            StringBuilder tmpFixedShipping = new StringBuilder(4096);

            if (Weight > decimal.Zero)
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(Weight), Convert.ToDecimal(0), true));
            }
            else
            {
                tmpRealTimeShipping.Append((string)obj.UPSGetRates(Convert.ToDecimal(1), Convert.ToDecimal(0), true));
            }
            string strResult = tmpRealTimeShipping.ToString();

            #region Get Fixed Shipping Methods

            try
            {
                ShippingComponent objShipping = new ShippingComponent();
                DataSet dsFixedShippingMethods = new DataSet();
                dsFixedShippingMethods = objShipping.GetFixedShippingMethods(Convert.ToInt32(AppLogic.AppConfigs("StoreID")), ServiceName);
                if (dsFixedShippingMethods != null && dsFixedShippingMethods.Tables.Count > 0 && dsFixedShippingMethods.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsFixedShippingMethods.Tables[0].Rows.Count; i++)
                    {

                        tmpFixedShipping.Append((string)dsFixedShippingMethods.Tables[0].Rows[i]["ShippingMethod"] + ",");
                    }
                }
            }
            catch { }

            #endregion

            string[] strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (strResult.ToString().ToLower().IndexOf("error") > -1)
            {
                //lblMsg.Visible = true;
                StrMessage = strResult + "<br />";
                // lblMsg.Text += strResult + "<br />";
                strResult = tmpFixedShipping.ToString();

                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    //  rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));
                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    ShippingTable.Rows.Add(dataRow);



                }
            }
            else
            {
                strResult = tmpFixedShipping.ToString() + strResult;
                strMethod = strResult.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < strMethod.Length; i++)
                {
                    string[] strMethodname = strMethod[i].ToString().Split("|".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    string Shippingname = "";
                    if (strMethodname.Length > 0)
                    {
                        Shippingname = strMethodname[0].ToString();
                    }
                    if (strMethodname.Length > 1)
                    {
                        Shippingname += "($" + string.Format("{0:0.00}", Convert.ToDecimal(strMethodname[1].ToString())) + ")";
                    }
                    // rdoShippingMethod.Items.Add(new ListItem(Shippingname, Shippingname));

                    DataRow dataRow = ShippingTable.NewRow();
                    dataRow["ShippingMethodName"] = Shippingname;
                    dataRow["Price"] = Convert.ToDecimal(strMethodname[1].ToString());
                    ShippingTable.Rows.Add(dataRow);


                }

                //if (Session["CustID"] != null && Session["CustID"].ToString() != "" && Session["UserName"] != null && Session["UserName"].ToString() != "")
                //{
                //    if (ViewState["CustomerLevelFreeShipping"] != null && Convert.ToString(ViewState["CustomerLevelFreeShipping"]) == "1")
                //    {
                //        bool IsContainFreeShipping = false;
                //        for (int i = 0; i < rdoShippingMethod.Items.Count; i++)
                //        {
                //            if (rdoShippingMethod.Items[i].Text.ToString().Trim().ToLower().IndexOf("free shipping") > -1 || rdoShippingMethod.Items[i].Text.ToString().Trim().ToLower().IndexOf("freeshipping") > -1 || rdoShippingMethod.Items[i].Text.ToString().Trim().ToLower().IndexOf("$0") > -1 || rdoShippingMethod.Items[i].Text.ToString().Trim().ToLower().IndexOf("0.00") > -1)
                //            {
                //                IsContainFreeShipping = true;
                //                rdoShippingMethod.Items[i].Selected = true;
                //                break;
                //            }
                //        }
                //        if (!IsContainFreeShipping)
                //        {
                //            String strFreeShipping = "Standard Shipping($0.00)";
                //            rdoShippingMethod.Items.Insert(0, new ListItem(strFreeShipping, strFreeShipping));
                //            rdoShippingMethod.Items[0].Selected = true;

                //            DataRow dataRow = ShippingTable.NewRow();
                //            dataRow["ShippingMethodName"] = strFreeShipping;
                //            dataRow["Price"] = 0;
                //            ShippingTable.Rows.Add(dataRow);



                //        }
                //    }
                //}

                //if (Session["SelectedShippingMethod"] != null && Convert.ToString(Session["SelectedShippingMethod"]) != "")
                //{
                //    if (rdoShippingMethod.Items.Count > 0)
                //    {
                //        rdoShippingMethod.ClearSelection();
                //        for (int i = 0; i < rdoShippingMethod.Items.Count; i++)
                //        {
                //            String[] strSplitValue = rdoShippingMethod.Items[i].Text.ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //            String[] strSplitSessionValue = Session["SelectedShippingMethod"].ToString().Split("(".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                //            if (strSplitValue[0] != null)
                //            {
                //                if (Convert.ToString(strSplitValue[0]) == Convert.ToString(strSplitSessionValue[0]))
                //                {
                //                    rdoShippingMethod.Items[i].Selected = true;
                //                    break;
                //                }
                //            }
                //        }
                //    }
                //}

            }

            return ShippingTable;
        }

        #endregion

        /// <summary>
        /// Get Credit Card Type By StoreID
        /// </summary>
        private void GetCreditCardType()
        {
            CreditCardComponent objCreditcard = new CreditCardComponent();
            DataSet dsCard = new DataSet();
            ddlCardType.Items.Clear();
            dsCard = objCreditcard.GetAllCarTypeByStoreID(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (dsCard != null && dsCard.Tables.Count > 0 && dsCard.Tables[0].Rows.Count > 0)
            {
                ddlCardType.DataSource = dsCard;
                ddlCardType.DataTextField = "CardType";
                ddlCardType.DataValueField = "CardTypeID";
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
       
        #region Get Shipping Method By ZipCode And State

        /// <summary>
        /// Get Shipping Method By ZipCode And State
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtBillZipCode_TextChanged(object sender, EventArgs e)
        {
            if (UseShippingAddress.Checked == false)
            {
                GetShippingMethodByBillAddress();
                BindShippingMethod();
                BindOrderSummary();
            }
            StateOtherSelection();
        }

        /// <summary>
        /// Get Shipping Method By ZipCode And State
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtShipZipCode_TextChanged(object sender, EventArgs e)
        {
            if (UseShippingAddress.Checked)
            {
                GetShippingMethodByShipAddress();
                BindShippingMethod();
                BindOrderSummary();
            }
            StateOtherSelection();
        }

        /// <summary>
        /// Bill State Drop Down Selected Index Changed Event for Get Shipping Method
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlBillstate_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UseShippingAddress.Checked == false)
            {
                GetShippingMethodByBillAddress();
                BindShippingMethod();
                BindOrderSummary();
            }
            StateOtherSelection();
        }

        /// <summary>
        /// Ship State Drop Down Selected Index Changed Event for Get Shipping Method
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlShipState_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UseShippingAddress.Checked)
            {
                GetShippingMethodByShipAddress();
                BindShippingMethod();
                BindOrderSummary();
            }
            StateOtherSelection();
        }

        /// <summary>
        /// Gets the Shipping Method by Bill Address
        /// </summary>
        private void GetShippingMethodByBillAddress()
        {
            if (UseShippingAddress.Checked == false)
            {
                hdnZipCode.Value = txtBillZipCode.Text.ToString();
                if (ddlBillstate.SelectedValue == "-11")
                {
                    hdnState.Value = txtBillother.Text.ToString();
                }
                else
                {
                    hdnState.Value = ddlBillstate.SelectedItem.Text.ToString();
                }
                hdncountry.Value = ddlBillcountry.SelectedValue.ToString();

            }
            else
            {
                hdnZipCode.Value = txtShipZipCode.Text.ToString();
                if (ddlShipState.SelectedValue == "-11")
                {
                    hdnState.Value = txtShipOther.Text.ToString();
                }
                else
                {
                    hdnState.Value = ddlShipState.SelectedItem.Text.ToString();
                }
                hdncountry.Value = ddlShipCounry.SelectedValue.ToString();
            }
        }

        /// <summary>
        /// Gets the Shipping Method by Ship Address.
        /// </summary>
        private void GetShippingMethodByShipAddress()
        {
            hdnZipCode.Value = txtShipZipCode.Text.ToString();
            if (ddlShipState.SelectedValue == "-11")
            {
                hdnState.Value = txtShipOther.Text.ToString();
            }
            else
            {
                hdnState.Value = ddlShipState.SelectedItem.Text.ToString();
            }
            hdncountry.Value = ddlShipCounry.SelectedValue.ToString();
        }

        #endregion

        #region Get Customer Details

        /// <summary>
        /// Get Customer Details by CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 Customer ID</param>
        private void GetCustomerDetails(Int32 CustomerID)
        {
            DataSet dsCustomer = new DataSet();
            CustomerComponent objCust = new CustomerComponent();
            dsCustomer = objCust.GetCustomerDetails(CustomerID);

            //Billing Address
            ltrBillAdd.Text = "";

            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                ltrBillAdd.Text += "<strong>" + Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString()) + "</strong><br />";
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address1"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address1"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Address2"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address2"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Suite"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Suite"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["City"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["City"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["State"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString()) + "<br />";
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString()) + "<br />";
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CountryName"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["CountryName"].ToString()) + "<br />";
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Phone"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Phone"].ToString()) + "<br />";
                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["Email"].ToString()))
                {
                    ltrBillAdd.Text += Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                    ViewState["BillEmail"] = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());

                }

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CustomerLevelID"].ToString()))
                {
                    ViewState["CustomerLevelID"] = dsCustomer.Tables[0].Rows[0]["CustomerLevelID"].ToString();
                }

                // Credit Card Details

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["NameOnCard"].ToString()))
                {
                    txtNameOnCard.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["NameOnCard"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString()))
                {
                    ddlCardType.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardTypeID"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString()))
                {
                    txtCSC.Text = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardVerificationCode"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString()))
                {
                    string CardNumber = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString());
                    if (CardNumber.Length > 4)
                    {
                        for (int i = 0; i < CardNumber.Length - 4; i++)
                        {
                            txtCardNumber.Text += "*";
                        }
                        txtCardNumber.Text += CardNumber.ToString().Substring(CardNumber.Length - 4);
                        Session["CardNumber"] = CardNumber.ToString();
                    }
                    else
                    {
                        txtCardNumber.Text = "";
                    }
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString()))
                {
                    ddlMonth.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationMonth"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString()))
                {
                    ddlYear.SelectedValue = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CardExpirationYear"].ToString());
                }
            }
            else
            {
                ltrBillAdd.Text = "N/A";
            }

            //Shipping Address
            ltrShipAdd.Text = "";
            if (dsCustomer != null && dsCustomer.Tables.Count > 1 && dsCustomer.Tables[1].Rows.Count > 0)
            {
                ltrShipAdd.Text += "<strong>" + Convert.ToString(dsCustomer.Tables[1].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(dsCustomer.Tables[1].Rows[0]["LastName"].ToString()) + "</strong><br />";
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Address1"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address1"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Address2"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address2"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Suite"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Suite"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["City"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["City"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["State"].ToString()))
                {
                    hdnState.Value = Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString());

                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString()))
                {
                    hdnZipCode.Value = Convert.ToString(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString());

                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["CountryName"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["CountryName"].ToString()) + "<br />";
                    hdncountry.Value = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Country"].ToString());
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Phone"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Phone"].ToString()) + "<br />";
                }
                if (!string.IsNullOrEmpty(dsCustomer.Tables[1].Rows[0]["Email"].ToString()))
                {
                    ltrShipAdd.Text += Convert.ToString(dsCustomer.Tables[1].Rows[0]["Email"].ToString());
                }
            }
            else
            {
                ltrShipAdd.Text = "";
            }
        }

        /// <summary>
        /// Gets the Customer Details for Payment
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns tb_Order Table Object</returns>
        private tb_Order GetCustomerDetailsForpayment(Int32 CustomerID)
        {
            DataSet dsCustomer = new DataSet();
            CustomerComponent objCust = new CustomerComponent();
            dsCustomer = objCust.GetCustomerDetails(CustomerID);

            //Billing Address
            tb_Order objorderData = new tb_Order();

            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                objorderData.FirstName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                objorderData.LastName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                objorderData.Email = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                objorderData.BillingFirstName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["FirstName"].ToString());
                objorderData.BillingLastName = Convert.ToString(dsCustomer.Tables[0].Rows[0]["LastName"].ToString());
                objorderData.BillingAddress1 = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address1"].ToString());
                objorderData.BillingAddress2 = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Address2"].ToString());
                objorderData.BillingSuite = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Suite"].ToString());
                objorderData.BillingCity = Convert.ToString(dsCustomer.Tables[0].Rows[0]["City"].ToString());
                objorderData.BillingState = Convert.ToString(dsCustomer.Tables[0].Rows[0]["State"].ToString());
                objorderData.BillingZip = Convert.ToString(dsCustomer.Tables[0].Rows[0]["ZipCode"].ToString());
                objorderData.BillingCountry = Convert.ToString(dsCustomer.Tables[0].Rows[0]["CountryName"].ToString());
                objorderData.BillingPhone = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Phone"].ToString());
                objorderData.BillingEmail = Convert.ToString(dsCustomer.Tables[0].Rows[0]["Email"].ToString());
                objorderData.CardName = Convert.ToString(txtNameOnCard.Text.ToString());
                objorderData.CardType = Convert.ToString(ddlCardType.SelectedItem.Text.ToString());
                objorderData.CardVarificationCode = Convert.ToString(txtCSC.Text.ToString());
                if (txtCardNumber.Text.ToString().IndexOf("*") > -1)
                {
                    objorderData.CardNumber = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["CardNumber"].ToString());
                }
                else
                {
                    objorderData.CardNumber = txtCardNumber.Text.ToString();
                }
                objorderData.CardExpirationMonth = ddlMonth.SelectedValue.ToString();
                objorderData.CardExpirationYear = ddlYear.SelectedValue.ToString();
                // Credit Card Details
            }

            //Shipping Address
            if (dsCustomer != null && dsCustomer.Tables.Count > 1 && dsCustomer.Tables[1].Rows.Count > 0)
            {
                objorderData.ShippingFirstName = Convert.ToString(dsCustomer.Tables[1].Rows[0]["FirstName"].ToString());
                objorderData.ShippingLastName = Convert.ToString(dsCustomer.Tables[1].Rows[0]["LastName"].ToString());
                objorderData.ShippingAddress1 = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address1"].ToString());
                objorderData.ShippingAddress2 = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Address2"].ToString());
                objorderData.ShippingSuite = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Suite"].ToString());
                objorderData.ShippingCity = Convert.ToString(dsCustomer.Tables[1].Rows[0]["City"].ToString());
                objorderData.ShippingState = Convert.ToString(dsCustomer.Tables[1].Rows[0]["State"].ToString());
                objorderData.ShippingZip = Convert.ToString(dsCustomer.Tables[1].Rows[0]["ZipCode"].ToString());
                objorderData.ShippingCountry = Convert.ToString(dsCustomer.Tables[1].Rows[0]["CountryName"].ToString());
                objorderData.ShippingPhone = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Phone"].ToString());
                objorderData.ShippingEmail = Convert.ToString(dsCustomer.Tables[1].Rows[0]["Email"].ToString());
            }
            return objorderData;
        }

        #endregion

        /// <summary>
        /// Shipping Method Radio Button Selected Index Changed for Add Shipping Amount in Cart Details
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void rdoShippingMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindOrderSummary();
            StateOtherSelection();
        }

        #region Remove ViewState From page

        /// <summary>
        /// Loads any saved view-state information to the <see cref="T:System.Web.UI.Page" /> object.
        /// </summary>
        /// <returns>The saved view state.</returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            if (Session[Session.SessionID] != null)
                return (new LosFormatter().Deserialize((string)Session[Session.SessionID]));
            return null;
        }

        /// <summary>
        /// Saves any view-state and control-state information for the page.
        /// </summary>
        /// <param name="state">The state.</param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            LosFormatter los = new LosFormatter();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            los.Serialize(sw, state);
            string vs = sw.ToString();
            Session[Session.SessionID] = vs;
        }

        #endregion
    }
}