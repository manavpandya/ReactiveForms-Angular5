﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.Common;
using System.Data;

namespace Solution.UI.Web.RMA.HPDAmazon
{
    public partial class ReturnPackageSlip : System.Web.UI.Page
    {
        string OrderNo = string.Empty;
        Int32 ProductID = 0;
        Int32 Return_Qty = 0;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Bind Data if Store Credit
                if (Request.QueryString["StoreCredit"] != null)
                {
                    if (Request.QueryString["ono"] != null && Request.QueryString["prodid"] != null)
                    {
                        Int32 OrderNumber = 0;
                        Int32.TryParse(Server.UrlDecode(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString())), out OrderNumber);
                        OrderComponent objOrder = new OrderComponent();
                        DataSet objDsorder = new DataSet();
                        objDsorder = objOrder.GetOrderDetailsByOrderID(OrderNumber);
                        if (objDsorder != null && objDsorder.Tables.Count > 0 && objDsorder.Tables[0].Rows.Count > 0)
                        {
                            Solution.Data.RMADAC objReturnItem = new Data.RMADAC();
                            objReturnItem.OrderedCustomerID = Convert.ToInt32(objDsorder.Tables[0].Rows[0]["CustomerId"].ToString());
                            objReturnItem.OrderedNumber = Convert.ToInt32(OrderNumber);
                            objReturnItem.CustomerName = objDsorder.Tables[0].Rows[0]["FirstName"].ToString() + " " + objDsorder.Tables[0].Rows[0]["LastName"].ToString();
                            objReturnItem.CustomerEMail = objDsorder.Tables[0].Rows[0]["Email"].ToString();
                            objReturnItem.OrderDate = Convert.ToDateTime(objDsorder.Tables[0].Rows[0]["OrderDate"].ToString());
                            objReturnItem.ProductID = Convert.ToInt32(Request.QueryString["prodid"].ToString());
                            objReturnItem.Quantity = Convert.ToInt32(Request.QueryString["Qty"]);
                            objReturnItem.ReturnReason = "Store Credit";
                            objReturnItem.AdditionalInformation = "Store Credit";
                            objReturnItem.ReturnFee = "0";
                            objReturnItem.ReturnType = "SC";
                            objReturnItem.Deleted = false;
                            objReturnItem.CreatedOn = DateTime.Now;

                            int ReturnID = Convert.ToInt32(objReturnItem.AddReturnItem());
                            Response.Redirect("ReturnPackageSlip.aspx?ID=" + ReturnID + "&Page=returnmerchandise");
                        }
                    }
                }
                // Bind Data if Return Item 
                if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString().Length > 0)
                {
                    lblRMANo.Text = "RMA-" + Request.QueryString["ID"].ToString() + "";

                    if (!System.IO.Directory.Exists(Server.MapPath("/Admin/Images/Barcodes/")))
                    {
                        System.IO.Directory.CreateDirectory(Server.MapPath("/Admin/Images/Barcodes/"));
                    }
                    if (!System.IO.File.Exists(Server.MapPath("~/Admin/Images/Barcodes/RMA-" + Request.QueryString["ID"].ToString() + ".png")))
                    {
                        DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                        bCodeControl.BarCode = Request.QueryString["ID"].ToString();
                        bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                        bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                        bCodeControl.BarCodeHeight = 80;
                        bCodeControl.ShowHeader = false;
                        bCodeControl.ShowFooter = true;
                        bCodeControl.FooterText = "RMA-" + Request.QueryString["ID"].ToString();
                        bCodeControl.Size = new System.Drawing.Size(250, 150);
                        bCodeControl.SaveImage(Server.MapPath("~/Admin/Images/Barcodes/RMA-" + Request.QueryString["ID"].ToString() + ".png"));
                    }
                    imgOrderBarcode.Src = "/Admin/Images/Barcodes/RMA-" + Request.QueryString["ID"].ToString() + ".png";

                    RMAComponent objReturnItem = new RMAComponent();
                    DataSet dsReturn = new DataSet();
                    dsReturn = objReturnItem.GetRetrunItemByID(Convert.ToInt32(Request.QueryString["ID"].ToString()));
                    if (dsReturn != null && dsReturn.Tables[0].Rows.Count > 0)
                    {
                        OrderNo = dsReturn.Tables[0].Rows[0]["orderedNumber"].ToString();
                        if (Request.QueryString["Page"] != null && Request.QueryString["Page"].ToString().ToLower().Trim() == "returnmerchandise")
                        {
                            ltback.Text = "<a  href='ViewRMARecentOrders.aspx?ono=" + Server.UrlEncode(SecurityComponent.Encrypt(OrderNo.ToString())) + "'><img title='Back' alt='Back' src='/images/back.jpg' border='0'/></a>";
                        }
                        ProductID = Convert.ToInt32(dsReturn.Tables[0].Rows[0]["ProductID"].ToString());
                        lblReturnDate.Text = dsReturn.Tables[0].Rows[0]["CreatedOn"].ToString();
                        Return_Qty = Convert.ToInt32(dsReturn.Tables[0].Rows[0]["Quantity"].ToString());
                        GetOrderDetails(Convert.ToInt32(OrderNo));
                        //AppConfig.StoreID = Convert.ToInt32(dsReturn.Tables[0].Rows[0]["StoreID"].ToString());
                        string liveservername = AppLogic.AppConfigs("LIVE_SERVER_NAME");
                        //ltLink.Text = "<b>" + AppLogic.AppConfigs("StoreName").ToString() + "<b><br/> " + liveservername + "";
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
                ltrorderNumber.Text = OrderNumber.ToString();
                string reforder = "";
                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["reforderid"].ToString().Trim()))
                {
                    if (!String.IsNullOrEmpty(Convert.ToString(objDsorder.Tables[0].Rows[0]["reforderid"].ToString().Trim())) && objDsorder.Tables[0].Rows[0]["reforderid"].ToString() != "0")
                        reforder = "(Ref. Order Number: " + objDsorder.Tables[0].Rows[0]["reforderid"].ToString() + " )";
                }
                ltrorderNumber.Text += "  " + reforder;

                DataSet dsWarehouse = new DataSet();
                dsWarehouse = CommonComponent.GetCommonDataSet(" Select tb_WareHouse.name,Address1,Address2,Suite,City,State,ZipCode,tb_WareHouse.Country,tb_Country.Name as CountryName,ISNULL(tb_OrderedShoppingCartItems.WareHouseID,0) as WareHouseID from tb_OrderedShoppingCartItems " +
                                                               " Inner Join tb_WareHouse on tb_WareHouse.WareHouseID=tb_OrderedShoppingCartItems.WareHouseID inner Join tb_Country on tb_WareHouse.Country=tb_Country.CountryID " +
                                                               " where RefProductID=" + ProductID + " and OrderedShoppingCartID=" + objDsorder.Tables[0].Rows[0]["ShoppingCardID"].ToString() + "");
                string strFromAddress = "";
                if (dsWarehouse != null && dsWarehouse.Tables.Count > 0 && dsWarehouse.Tables[0].Rows.Count > 0)
                {
                    strFromAddress = dsWarehouse.Tables[0].Rows[0]["name"].ToString() + "<br/>"
                    + dsWarehouse.Tables[0].Rows[0]["Address1"].ToString() + "<br/>";
                    if (!String.IsNullOrEmpty(dsWarehouse.Tables[0].Rows[0]["Address2"].ToString()))
                        strFromAddress += dsWarehouse.Tables[0].Rows[0]["Address2"].ToString() + "<br/>";
                    if (!String.IsNullOrEmpty(dsWarehouse.Tables[0].Rows[0]["Suite"].ToString()))
                        strFromAddress += dsWarehouse.Tables[0].Rows[0]["Suite"].ToString() + "<br/>";
                    strFromAddress += dsWarehouse.Tables[0].Rows[0]["City"].ToString() + ", " + dsWarehouse.Tables[0].Rows[0]["State"].ToString() + ", " + dsWarehouse.Tables[0].Rows[0]["ZipCode"].ToString()
                        + "<br/>" + dsWarehouse.Tables[0].Rows[0]["CountryName"].ToString();
                }
                else
                {
                    //Bind Origin Address
                    strFromAddress = AppLogic.AppConfigs("Shipping.OriginContactName") + "<br/>"
                   + AppLogic.AppConfigs("Shipping.OriginAddress") + "<br/>";
                    if (!String.IsNullOrEmpty(AppLogic.AppConfigs("Shipping.OriginAddress2")))
                        strFromAddress += AppLogic.AppConfigs("Shipping.OriginAddress2") + "<br/>";
                    strFromAddress += AppLogic.AppConfigs("Shipping.OriginCity") + ", " + AppLogic.AppConfigs("Shipping.OriginState")
                        + "<br/>" + AppLogic.AppConfigs("Shipping.OriginZip");
                }
                ltrShipToAddr.Text = strFromAddress.ToString();




                ltrOrderdate.Text = objDsorder.Tables[0].Rows[0]["OrderDate"].ToString();
                ltrshippingMethod.Text = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();
               // ltrpaymentMethod.Text = objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString();
                //if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString()) && objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString().ToLower() == "creditcard")
                //{
                //    if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["CardNumber"].ToString()))
                //    {
                //        string CardNumber = SecurityComponent.Decrypt(objDsorder.Tables[0].Rows[0]["CardNumber"].ToString());
                //        if (CardNumber.Length > 4)
                //        {
                //            for (int i = 0; i < CardNumber.Length - 4; i++)
                //            {
                //                ltrCardNumber.Text += "*";
                //            }
                //            ltrCardNumber.Text += CardNumber.ToString().Substring(CardNumber.Length - 4);
                //        }
                //        else
                //        {
                //            ltrCardNumber.Text = "";
                //        }
                //        trcard.Visible = true;
                //    }

                //}
                //else
                //{
                //    trcard.Visible = false;
                //}

                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["LastName"].ToString());
                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString());

                ltrAddress.Text = "";
                ltrAddress.Text += "<table width=\"100%\" class=\"popup_cantain\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\">";
                ltrAddress.Text += "<tbody>";
                ltrAddress.Text += "<tr>";

                ltrAddress.Text += "<td width=\"50%\">";
                ltrAddress.Text += "<b>Billing Address</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "<b>Shipping Address</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";

                ltrAddress.Text += "<td class=\"font-bold\">";
                ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString()); ;
                ltrAddress.Text += "</td>";

                ltrAddress.Text += "<td class=\"font-bold\">";
                ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString()); ;
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";

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
                ltrAddress.Text += "</tr>";
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
                ltrAddress.Text += "<td>";
                ltrAddress.Text += BillingAddress1.ToString() + "" + BillingSuite.ToString() + "" + BillingCity.ToString() + "";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += ShippingAddress1.ToString() + "" + ShippingSuite.ToString() + "" + ShippingCity.ToString() + "";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += BillingState.ToString() + "" + BillingZip.ToString();
                ltrAddress.Text += "</td>";

                ltrAddress.Text += "<td>";
                ltrAddress.Text += ShippingState.ToString() + "" + ShippingZip.ToString();
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                ltrAddress.Text += "<tr>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()))
                {
                    ltrAddress.Text += "<td>";
                    if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()) + ", ";
                    }
                    else
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString());
                    }
                    ltrAddress.Text += "</td>";
                }

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                {
                    ltrAddress.Text += "<td>";
                    if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()) + ", ";
                    }
                    else
                    {
                        ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString());
                    }
                    ltrAddress.Text += "</td>";
                }
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
        /// <param name="dsOrderdata">DataSet dsOrderdata</param>
        private void BindCart(Int32 OrderNumber, DataSet dsOrderdata)
        {
            RMAComponent objRMAOrder = new RMAComponent();

            DataSet dsCart = new DataSet();
            dsCart = objRMAOrder.GetOrderedShoppingCartItemsFormRetrunItem(Convert.ToInt32(dsOrderdata.Tables[0].Rows[0]["OrderNumber"].ToString()), ProductID.ToString());
            ltrCart.Text = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\" class=\"pop_border_new2\" style=\"border-collapse: collapse;\">";
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
            ltrCart.Text += "<td style=\"text-align: center;\">";
            ltrCart.Text += "<b>Status</b>";
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
                    ltrCart.Text += Return_Qty;
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td style=\"text-align: right;\">";
                    ltrCart.Text += "Returned";
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "</tr>";
                }
            }
            ltrCart.Text += "</table>";
        }
    }
}