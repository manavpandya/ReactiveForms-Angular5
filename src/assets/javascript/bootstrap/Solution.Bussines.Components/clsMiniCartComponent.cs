using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Data.Objects;
using Solution.Bussines.Components.Common;
using System.IO;
using System.Web;
namespace Solution.Bussines.Components
{
    /// <summary>
    /// Mini Cart Component Class Contains Mini Cart related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0ove
    /// 
    /// </summary>
    /// 
    public class clsMiniCartComponent
    {
        #region Local Variable
        int intCustID = 0;
        #endregion

        #region Constructor
        public clsMiniCartComponent(int CustomerID)
        {
            intCustID = CustomerID;
        }
        #endregion

        #region GetMiniCart
        /// <summary>
        /// Get Mini Cart
        /// </summary>
        /// <returns>Returns String - Minicart</returns>
        public string GetMiniCart()
        {
            System.Text.StringBuilder Table = new StringBuilder();

            try
            {
                decimal NetPrice = decimal.Zero;
                decimal NormalPrice = decimal.Zero;
                decimal swatchprice = decimal.Zero;
                decimal SubTotal = decimal.Zero;
                decimal OrderTotal = decimal.Zero;

                decimal QtyDiscount = decimal.Zero;
                string CustomerID = Convert.ToString(System.Web.HttpContext.Current.Session["CustID"]);
                int Customerid = Convert.ToInt32(System.Web.HttpContext.Current.Session["CustID"]);
                if (CustomerID != null)
                {
                    DataSet dsShoppingCart = ShoppingCartComponent.GetCartDetailByCustomerID(Customerid);
                    if (dsShoppingCart != null && dsShoppingCart.Tables.Count > 0 && dsShoppingCart.Tables[0].Rows.Count > 0)
                    {
                        NetPrice = 0;
                        NormalPrice = 0;
                        swatchprice = 0;
                        Table.Append("<table class='view_shopping_main' style='float:left;width:288px;' cellpadding='0' cellspacing='0'>");
                        Table.Append("<tr class=\"view_shopping\" style='float:left;'> " +
                                    " <td align=\"left\" valign=\"top\" > <table width='100%'><tr style=\"padding-top:0px; width:300px;\"><td class='shopping_burron' style=\"padding-top:5px; width:290px;\"><p>Your Shopping Cart</p> </td><td>" +
                                    "<a href=\"javascript:resetHover();hideMiniCart();\"><img   src=\"/images/remove-minicart.png\" alt=\"Close\" title=\"Close\" align=\"absmiddle\" style=\"padding: 0px 0pt 0pt; border: medium none;\"></a> </td></tr></table></td>" +
                                    " </tr>");

                        Int32 TotalQty = 0;
                        Decimal SwatchQty = Decimal.Zero;
                        // String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_ShoppingCartItems WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Customerid.ToString() + " Order By ShoppingCartID DESC) "));
                        String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(tb_ShoppingCartItems.Quantity,0) * case when isnull(tb_product.saleprice,0) > 0 then tb_product.saleprice else isnull(tb_product.price,0) end),0) FROM tb_ShoppingCartItems INNER JOIN tb_product ON tb_Product.ProductID=tb_ShoppingCartItems.ProductID  WHERE IsProductType=0 AND ShoppingCartID in (SELECT Top 1 ShoppingCartID FROM tb_ShoppingCart WHERE CustomerID=" + Customerid.ToString() + " Order By ShoppingCartID DESC) "));

                        //if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToDecimal(strswatchQtyy) > Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == Decimal.Zero)
                        //{
                        SwatchQty = Convert.ToDecimal(AppLogic.AppConfigs("SwatchMaxlength").ToString());
                        //}
                        for (int CartItemNo = 0; CartItemNo < dsShoppingCart.Tables[0].Rows.Count; CartItemNo++)
                        {
                            Int32 Isorderswatch = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + " and ItemType='Swatch' "));
                            Decimal pp = Decimal.Zero;
                            swatchprice = 0;
                            if (Isorderswatch == 1)
                            {
                                pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT cast(" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString() + " as money) * case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ""));
                                swatchprice = (pp) / Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString());
                            }
                            if (SwatchQty > Decimal.Zero)
                            {


                                if (Isorderswatch == 1 && Convert.ToDecimal(pp) >= SwatchQty)
                                {


                                    NetPrice = pp - SwatchQty; //Convert.ToDecimal(pp.ToString()) * Convert.ToInt32(SwatchQty);
                                    NormalPrice = (pp - SwatchQty) / Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString());// Convert.ToDecimal(pp);
                                    SwatchQty = Decimal.Zero;
                                    //QtyDiscount += Item.m_QuantityDiscount;
                                    SubTotal += NetPrice;

                                }
                                else if (Isorderswatch == 1)
                                {
                                    //pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ""));
                                    NetPrice = Decimal.Zero;
                                    NormalPrice = Decimal.Zero;
                                    //QtyDiscount += Item.m_QuantityDiscount;
                                    SubTotal += NetPrice;
                                    SwatchQty = SwatchQty - pp;
                                }
                                else
                                {

                                    NetPrice = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[CartItemNo]["Price"].ToString()) * Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString());
                                    NormalPrice = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[CartItemNo]["Price"].ToString());
                                    //QtyDiscount += Item.m_QuantityDiscount;
                                    SubTotal += NetPrice;



                                }

                            }
                            else
                            {

                                if (Isorderswatch == 1)
                                {
                                    NetPrice = pp;
                                    NormalPrice = pp / Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString());
                                    //QtyDiscount += Item.m_QuantityDiscount;
                                    SubTotal += NetPrice;
                                }
                                else
                                {
                                    NetPrice = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[CartItemNo]["Price"].ToString()) * Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString());
                                    NormalPrice = Convert.ToDecimal(dsShoppingCart.Tables[0].Rows[CartItemNo]["Price"].ToString());
                                    //QtyDiscount += Item.m_QuantityDiscount;
                                    SubTotal += NetPrice;
                                }

                            }






                            string strProductName = System.Web.HttpContext.Current.Server.HtmlEncode(dsShoppingCart.Tables[0].Rows[CartItemNo]["Name"].ToString());
                            TotalQty += Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString());
                            if (strProductName.Length > 50)
                            {
                                strProductName = "&nbsp;&nbsp;" + strProductName.Substring(0, 30) + "...";
                            }
                            else
                            {
                                strProductName = "&nbsp;&nbsp;" + strProductName;
                            }
                            String Name = "";
                            if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                            {
                                Name = "txtQtychild-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["VariantNames"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["VariantValues"].ToString().Replace("-", "~hpd~");

                            }
                            else
                            {
                                Name = "txtQty-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["VariantNames"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["VariantValues"].ToString().Replace("-", "~hpd~");
                            }
                            Table.Append("<tr class='color_text' ><td  style=\"text-align:left;\" colspan='2'>");

                            if ((dsShoppingCart.Tables[0].Rows[CartItemNo]["Maincategory"].ToString() == "") || (dsShoppingCart.Tables[0].Rows[CartItemNo]["Maincategory"] == null))
                            {
                                if (strProductName.ToLower().IndexOf("gift card") > -1)
                                {
                                    //Table.Append("<a  href='/gi-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["CustomCartID"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ".aspx'>" + strProductName + "</a>");
                                    if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("" + strProductName + "");
                                    }
                                    else
                                    {
                                        Table.Append("<a  style='color:#B92127;' href='/gi-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductURL"].ToString() + "'>" + strProductName + "</a>");
                                    }
                                }
                                else
                                {
                                    //Table.Append("<a  href='/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ".aspx'>" + strProductName + "</a>");
                                    if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("" + strProductName + "");
                                    }
                                    else
                                    {
                                        Table.Append("<a style='color:#B92127;' href='/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductURL"].ToString() + "'>" + strProductName + "</a>");
                                    }
                                }
                            }
                            else
                            {
                                if (strProductName.ToLower().IndexOf("gift card") > -1)
                                {
                                    //Table.Append("<a  href='/gi-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["CustomCartID"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ".aspx'>" + strProductName + "</a>");
                                    if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("" + strProductName + "");
                                    }
                                    else
                                    {
                                        Table.Append("<a style='color:#B92127;'  href='/gi-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductURL"].ToString() + "'>" + strProductName + "</a>");
                                    }
                                }
                                else
                                {
                                    //Table.Append("<a  href='/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Maincategory"].ToString() + "/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ".aspx'>" + strProductName + "</a>");
                                    if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("" + strProductName + "");
                                    }
                                    else
                                    {
                                        Table.Append("<a  style='color:#B92127;' href='/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductURL"].ToString() + "'>" + strProductName + "</a>");
                                    }
                                }
                            }
                            string[] names = dsShoppingCart.Tables[0].Rows[CartItemNo]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            string[] values = dsShoppingCart.Tables[0].Rows[CartItemNo]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            if (values.Length > 0)
                                Table.Append(" <br/> ");
                            for (int iLoopValues = 0; iLoopValues < values.Length && names.Length == values.Length; iLoopValues++)
                            {
                                if (iLoopValues > 0)
                                {
                                    if (names[iLoopValues].ToLower().IndexOf("estimated delivery") > -1)
                                    {
                                        Table.Append("<br/>&nbsp;&nbsp;" + names[iLoopValues] + ": <span style='color:#B92127;'>" + values[iLoopValues] + "</span>");
                                    }
                                    else
                                    {
                                        Table.Append("<br/>&nbsp;&nbsp;" + names[iLoopValues] + ": " + values[iLoopValues]);
                                    }
                                }
                                else
                                {
                                    if (names[iLoopValues].ToLower().IndexOf("estimated delivery") > -1)
                                    {
                                        Table.Append("&nbsp;&nbsp;" + names[iLoopValues] + ": <span style='color:#B92127;'>" + values[iLoopValues] + "</span>");
                                    }
                                    else
                                    {
                                        Table.Append("&nbsp;&nbsp;" + names[iLoopValues] + ": " + values[iLoopValues]);
                                    }
                                }
                            }
                            if (values.Length > 0)
                                Table.Append(" ");
                            Table.Append("</td></tr>");

                            Table.Append("<tr class='round_main' style=' float: left; padding: 0pt 0pt 0pt 1px;width: 330px;'><td class='color_main' colspan='2' valign=\"top\"  align=\"left\" style=\"padding: 3px;\">");

                            Table.Append(" <table width=\"100%\" border=\"0\" cellspacing=\"0\" cellpadding=\"3\" >");


                            Table.Append("<tr>");

                            if ((dsShoppingCart.Tables[0].Rows[CartItemNo]["Maincategory"].ToString() == "") || (dsShoppingCart.Tables[0].Rows[CartItemNo]["Maincategory"].ToString() == null))
                            {

                                Table.Append("<td   align=\"left\" valign=\"middle\">");
                                Table.Append("<div class='color_main_img' align=\"center\">");
                                if (strProductName.ToLower().IndexOf("gift card") > -1)
                                {
                                    // Table.Append("<a href='/gi-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["CustomCartID"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ".aspx'><img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\"></a>");
                                    if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("<img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\">");
                                    }
                                    else
                                    {
                                        Table.Append("<a  href='/gi-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductURL"].ToString() + "'><img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\"></a>");
                                    }
                                }
                                else
                                {
                                    //Table.Append("<a href='/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ".aspx'><img height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\" style='border:none;' width=\"85\"></a>");
                                    if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("<img height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\" style='border:none;' width=\"85\">");
                                    }
                                    else
                                    {
                                        Table.Append("<a  href='/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductURL"].ToString() + "'><img height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\" style='border:none;' width=\"85\"></a>");
                                    }
                                }
                                Table.Append("</div>");
                                Table.Append("</td>");
                            }
                            else
                            {

                                Table.Append("<td   align=\"left\" valign=\"middle\">");
                                Table.Append("<div class='color_main_bg' align=\"center\">");
                                if (strProductName.ToLower().IndexOf("gift card") > -1)
                                {
                                    // Table.Append("<a href='/gi-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["CustomCartID"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ".aspx'><img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\"></a>");
                                    if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("<img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\">");
                                    }
                                    else
                                    {
                                        Table.Append("<a href='/gi-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductURL"].ToString() + "'><img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\"></a>");
                                    }
                                }
                                else
                                {

                                    //Table.Append("<a href='/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Maincategory"].ToString() + "/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["sename"].ToString() + "-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + ".aspx'><img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\"></a>");
                                    if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                                    {
                                        Table.Append("<img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\">");
                                    }
                                    else
                                    {
                                        Table.Append("<a href='/" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductURL"].ToString() + "'><img style='padding-top:7px;' height=\"68\" Title=\"" + strProductName + "\" src=\"" + GetMicroImage(dsShoppingCart.Tables[0].Rows[CartItemNo]["ImageName"].ToString()) + "\" alt=\"" + strProductName + "\"  width=\"85\"></a>");
                                    }
                                }
                                Table.Append("</div>");
                                Table.Append("</td>");
                            }
                            Table.Append("<td valign='top' align='center' style='line-height:20px;float:left;' >");
                            Table.Append("<table    border='0' cellpadding='3' cellspacing='0' ><tr class='price_div_02'><td class='price_div' style='width:204px;'><p> Price</p> <div style=\"float:left;\">:</div>");
                            if (swatchprice > Decimal.Zero && NormalPrice == Decimal.Zero)
                            {
                                Table.Append("<input type='hidden' id='hdnp-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Productid"].ToString() + "' value='" + Math.Round((NormalPrice), 2).ToString() + "' /><span style=\"text-decoration:line-through;\">$" + string.Format("{0:0.00}", Math.Round((swatchprice), 2)) + "</span><br /><span id='spn-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Productid"].ToString() + "'  style='float:left;font-size:12px;font-weight:bold;width:70px;margin-top: 0px;'>&nbsp;$" + string.Format("{0:0.00}", Math.Round((NormalPrice), 2)) + "</span>");
                            }
                            else
                            {
                                Table.Append("<input type='hidden' id='hdnp-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Productid"].ToString() + "' value='" + Math.Round((NormalPrice), 2).ToString() + "' /><span id='spn-" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Productid"].ToString() + "'  style='float:left;font-size:12px;font-weight:bold;width:70px;margin-top: 0px;'>$" + string.Format("{0:0.00}", Math.Round((NormalPrice), 2)) + "</span>");
                            }


                            if (Convert.ToInt32(dsShoppingCart.Tables[0].Rows[CartItemNo]["RelatedproductID"].ToString()) > 0)
                            {
                                Table.Append("<div class=\"price_div_remove\"></div></td></tr><tr class='price_div_two' style='width:204px;'><td><p style=''>Quantity</p><div style=\"float:left;\">:</div>");
                                Table.Append("<span><input readonly=\"true\"  class=\"color_field\" width=\"30px\" maxlength=\"3\" type='text' id='" + Name.Replace("'", "&#39;") + "' name='" + Name.Replace("'", "&#39;") + "' value='" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString() + "' onkeypress=\"return CheckQty(event);\"></span>");
                            }
                            else
                            {
                                Table.Append("<div class=\"price_div_remove\"><a  href=\"javascript:void(0);\" onclick=\"javascript:RemoveProduct(" + dsShoppingCart.Tables[0].Rows[CartItemNo]["CustomCartID"].ToString() + ");\">[Remove]</a></div></td></tr><tr class='price_div_two' style='width:204px;'><td><p style=''>Quantity</p><div style=\"float:left;\">:</div>");
                                Table.Append("<span style=\"width:48px !important;\"><input  class=\"color_field\" width=\"30px\" maxlength=\"3\" type='text' id='" + Name.Replace("'", "&#39;") + "' name='" + Name.Replace("'", "&#39;") + "' value='" + dsShoppingCart.Tables[0].Rows[CartItemNo]["Qty"].ToString() + "' onkeypress=\"return CheckQty(event);\"></span><span><a href=\"javascript:void(0);\" onclick=\"javascript:UpdateProduct();\"><img src=\"/images/update-qty-btn.jpg\"></a></span>");
                            }
                            Table.Append("<input type='hidden' id='Product" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + "' name='Product" + dsShoppingCart.Tables[0].Rows[CartItemNo]["ProductID"].ToString() + "' value='" + Name.Replace("'", "&#39;") + "'> ");
                            Table.Append("</td></tr></table></td>");
                            Table.Append("</tr>");
                            Table.Append("</table>");
                            Table.Append("</tr>");
                        }
                        OrderTotal = SubTotal - QtyDiscount;
                        if (TotalQty > 1)
                            Table.Append("<tr style='width: 331px;' class='color_text_new' ><td style='width: 330px;'><div class=\"item_cart\">" + TotalQty.ToString() + " Items in Cart</div> <div class=\"item_cart_1\" style='padding-right: 0px;float:left; padding-left: 135px;'> Total: <font id='tt' runat='server' style='margin-top: 0px;font-weight:bold;font-size:12px;color:#B92127;'>$" + string.Format("{0:0.00}", Math.Round(SubTotal, 2)) + "</font></div></td></tr>");
                        else
                            Table.Append("<tr style='width: 331px;' class='color_text_new' ><td style='width: 330px;'><div class=\"item_cart\">" + TotalQty.ToString() + " Item in Cart</div> <div class=\"item_cart_1\" style='padding-right: 0px;float:left; padding-left: 135px;'> Total: <font id='tt' runat='server' style='margin-top: 0px;font-weight:bold;font-size:12px;color:#B92127;'>$" + string.Format("{0:0.00}", Math.Round(SubTotal, 2)) + "</font></div></td></tr>");
                        Table.Append("<tr><td><div class=\"color_text1\" style='width:301px;'><a href=\"/\" ><img style='padding: 0px 0px 3px; cursor: pointer; margin-top: 0px;float:left;' src=\"/images/keep-shopping-btn.jpg\" border=\"0\"  /></a><a href=\"/CheckoutCommon.aspx\" id=\"btnCheckout\"><img class=\"img_right\" style='float:right;margin:0px;padding:0px;' src=\"/images/checkout-minicart.png\"></a></div></td>");
                        Table.Append("</tr>");
                        Table.Append("</table>");
                        //Table.Append("</td>");
                        //Table.Append("</tr>");
                        //Table.Append("</table></td></tr>");
                    }
                    //Table.Append("</table>");
                }
            }
            catch { }
            return Table.ToString();
        }

        #endregion

        #region Get Micro Image
        /// <summary>
        /// Get Micro Image for Shopping Cart product
        /// </summary>
        /// <param name="img">Image Name</param>
        /// <returns>Returns Micro Image Path</returns>
        public String GetMicroImage(String img)
        {
            clsvariables.LoadAllPath();
            String[] AllowedExtensions = AppLogic.AppConfigs("AllowedExtensions").Split(',');
            String imagepath = String.Empty;
            Random rd = new Random();
            imagepath = AppLogic.AppConfigs("ImagePathProduct") + "Micro/" + img;
            if (img != "")
            {
                if (File.Exists(AppLogic.AppConfigs("Live_Contant_Server_Path") + (imagepath)))
                {
                    return AppLogic.AppConfigs("Live_Contant_Server") + imagepath + "?" + rd.Next(1000).ToString();
                }
            }
            else
            {
                return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
            }

            return string.Concat(AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathProduct") + "Micro/image_not_available.jpg");
        }
        #endregion
    }
}
