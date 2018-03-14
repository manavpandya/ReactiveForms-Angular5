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
using System.Text.RegularExpressions;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class Invoice_Sendmail : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["ONo"] != null)
            {
                int OrderNumber = 0;
                Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                Int32 StoreID = 0;
                Int32.TryParse(Convert.ToString(CommonComponent.GetScalarCommonData("Select StoreID from tb_Order where OrderNumber=" + OrderNumber)), out StoreID);
                AppConfig.StoreID = StoreID;
                string url = "http://" + Request.Url.Authority + Request.ApplicationPath.TrimEnd('/');
                ImgStoreLogo.Src = url + "/Images/Store_" + StoreID.ToString() + ".png";

                btnSendmail.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/send-invoice.png";
            }
            if (!IsPostBack)
            {
                BindInvoiceSignature();
                if (Request.QueryString["ONo"] != null)
                {
                    BindRefNumberDetails();
                    int OrderNumber = 0;
                    bool chkOrder = Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                    if (chkOrder)
                    {
                        ltrorderNumber.Text = OrderNumber.ToString();
                        GetOrderDetails(OrderNumber);
                        SendMail(OrderNumber);
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
                ltrshippingMethod.Text = objDsorder.Tables[0].Rows[0]["ShippingMethod"].ToString();
                ltrpaymentMethod.Text = objDsorder.Tables[0].Rows[0]["PaymentMethod"].ToString();
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
                #region Generate Barcode From OrderNo. By Girish
                String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                CreateFolder(FPath.ToString());
                if (!System.IO.File.Exists(Server.MapPath(FPath + "/ONo-" + OrderNumber.ToString() + ".png")))
                {
                    DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                    bCodeControl.BarCode = OrderNumber.ToString();
                    bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                    bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                    bCodeControl.BarCodeHeight = 80;
                    bCodeControl.ShowHeader = false;
                    bCodeControl.ShowFooter = true;
                    bCodeControl.FooterText = "ONo-" + OrderNumber.ToString();
                    bCodeControl.Size = new System.Drawing.Size(250, 150);
                    bCodeControl.SaveImage(Server.MapPath(FPath + "/ONo-" + OrderNumber + ".png"));
                    bCodeControl.Dispose();
                }
                imgOrderBarcode.Src = "http://www.halfpricedrapes.us" + FPath + "/ONo-" + OrderNumber + ".png";

                #endregion
                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["FirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["LastName"].ToString());
                ltrName.Text = Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString());
                if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["Transactionstatus"].ToString()) && objDsorder.Tables[0].Rows[0]["Transactionstatus"].ToString().ToLower().Trim() == "canceled")
                {
                    idcanceledtag.Src = "http://www.halfpricedrapes.us/images/watermark_canceled.png";
                    idcanceledtag.Visible = true;
                }
                else if (!string.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["Orderstatus"].ToString()) && objDsorder.Tables[0].Rows[0]["Orderstatus"].ToString().ToLower().Trim() == "canceled")
                {
                    idcanceledtag.Src = "http://www.halfpricedrapes.us/images/watermark_canceled.png";
                    idcanceledtag.Visible = true;
                }
                else
                {
                    idcanceledtag.Visible = false;
                }
                ltrAddress.Text = "";
                ltrAddress.Text += "<table width=\"60%\" cellspacing=\"0\" cellpadding=\"0\" align=\"left\" class=\"popup_cantain\">";
                ltrAddress.Text += "<tbody>";
                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "<b>Account</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "<b>Billing Address</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "<b>Shipping Address</b>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "Name:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td class=\"font-bold\">";
                ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingLastName"].ToString()); ;
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td class=\"font-bold\">";
                ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingFirstName"].ToString()) + " " + Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingLastName"].ToString()); ;
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "Company:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCompany"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingCompany"].ToString();
                else
                    ltrAddress.Text += "-";

                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingCompany"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "Address1:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingAddress1"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingAddress1"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingAddress1"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "Address2:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingAddress2"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingAddress2"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingAddress2"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "Suite:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingSuite"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingSuite"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingSuite"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "City:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCity"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingCity"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingCity"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "State:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingState"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingState"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingState"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingState"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "Zip:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingZip"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingZip"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";

                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingZip"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";

                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "Country:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString()))
                    ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["BillingCountry"].ToString());
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";

                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString()))
                    ltrAddress.Text += Convert.ToString(objDsorder.Tables[0].Rows[0]["ShippingCountry"].ToString());
                else
                    ltrAddress.Text += "-";

                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "<tr>";
                ltrAddress.Text += "<td>";
                ltrAddress.Text += "Phone:";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["BillingPhone"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";

                ltrAddress.Text += "<td>";
                if (!String.IsNullOrEmpty(objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString()))
                    ltrAddress.Text += objDsorder.Tables[0].Rows[0]["ShippingPhone"].ToString();
                else
                    ltrAddress.Text += "-";
                ltrAddress.Text += "</td>";
                ltrAddress.Text += "</tr>";
                ltrAddress.Text += "</tbody>";
                ltrAddress.Text += "</table>";

                BindCart(Convert.ToInt32(OrderNumber.ToString()), objDsorder);
            }
        }

        /// <summary>
        /// Creates the folder for specified path.
        /// </summary>
        /// <param name="FPath">String FPath</param>
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }

        /// <summary>
        /// Bind Order Cart By Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <param name="dsOrderdata">DataSet dsOrderdata</param>
        private void BindCart(Int32 OrderNumber, DataSet dsOrderdata)
        {
            OrderComponent objOrder = new OrderComponent();
            decimal AdjustmentAmount = 0;
            decimal Subtotal = 0;
            decimal RefundAmt = 0;
            decimal FinalTotal = 0;
            DataSet dsCart = new DataSet();
            dsCart = objOrder.GetInvoiceProductsWithMarryproduct(OrderNumber);
            Int32 SwatchQty = 0;

            decimal GrandSwatchSubTotal = decimal.Zero;
            String strswatchQtyy = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(sum(isnull(Quantity,0)),0) FROM tb_OrderedShoppingCartItems WHERE IsProductType=0 AND OrderedShoppingCartID in (SELECT Top 1 OrderedShoppingCartID FROM tb_OrderedShoppingCart WHERE CustomerID=" + dsOrderdata.Tables[0].Rows[0]["CustomerID"].ToString() + " Order By OrderedShoppingCartID DESC) "));
            if (!string.IsNullOrEmpty(strswatchQtyy) && Convert.ToInt32(strswatchQtyy) > Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString()) && SwatchQty == 0)
            {
                SwatchQty = Convert.ToInt32(strswatchQtyy) - Convert.ToInt32(AppLogic.AppConfigs("SwatchMaxlength").ToString());
            }
            bool IsCouponDiscount = false;
            ltrCart.Text = "<table width=\"100%\" cellspacing=\"0\" cellpadding=\"0\" border=\"1\" class=\"datatable\" style=\"border-collapse: collapse;\">";
            ltrCart.Text += "<tr style=\"line-height: 50px; background-color: rgb(242,242,242);\">";
            ltrCart.Text += "<th valign=\"middle\" align=\"left\" style=\"width: 55%\">";
            ltrCart.Text += "<b>Product</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 10%; text-align: center;\">";
            ltrCart.Text += "<b>SKU</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"width: 10%; text-align: center;\">";
            ltrCart.Text += "<b>UPC</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "<th valign=\"top\" align=\"center\" style=\"width: 10%; text-align: center;\">";
            ltrCart.Text += "<b>Price</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "###coupondiscount###";
            ltrCart.Text += "<th valign=\"middle\" align=\"center\" style=\"text-align: center;\">";
            ltrCart.Text += "<b>Quantity</b>";
            ltrCart.Text += "</th>";
            ltrCart.Text += "<th style=\"text-align: right;\">";
            ltrCart.Text += "<b>Sub Total</b>";
            ltrCart.Text += "</th>";
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
                    string sku = "";
                    string strupimage = "";
                    for (int j = 0; j < variantValue.Length; j++)
                    {
                        if (variantName.Length > j)
                        {
                            ltrCart.Text += variantName[j].ToString().Replace("Estimated Delivery", "Estimated Ship Date") + " : " + variantValue[j].ToString() + "<br />";
                            SQLAccess objSql = new SQLAccess();
                            DataSet dsoption = new DataSet();

                            dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + dsCart.Tables[0].Rows[i]["productId"] + " AND VariantValue='" + variantValue[j].ToString().ToLower().Replace("(buy 1 get 1 free)", "").Replace("(on sale)", "").Replace("'", "''") + "'");
                            if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["SKU"].ToString()))
                                {
                                    sku += "<br/>" + dsoption.Tables[0].Rows[0]["SKU"].ToString();
                                }

                            }
                            if (variantValue[j].ToString().IndexOf("($") > -1)
                            {
                                string strll = variantValue[j].ToString().Substring(0, variantValue[j].ToString().LastIndexOf("($"));
                                dsoption = objSql.GetDs("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + dsCart.Tables[0].Rows[i]["productId"] + " and isnull(upc,'')<>'' AND VariantValue='" + strll.ToString().ToLower().Replace("(buy 1 get 1 free)", "").Replace("(on sale)", "").Replace("'", "''") + "'");
                            }
                            if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0 && string.IsNullOrEmpty(strupimage))
                            {
                                if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                                {

                                    String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                                    CreateFolder(FPath.ToString());
                                    if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                    {
                                        strupimage = "<img width=\"160px\" src=\"http://www.halfpricedrapes.us" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                    }
                                    else
                                    {
                                        if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                        {
                                            try
                                            {
                                                DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                                                bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                                                bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                                                bCodeControl.BarCodeHeight = 70;
                                                bCodeControl.ShowHeader = false;
                                                bCodeControl.ShowFooter = true;
                                                bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                bCodeControl.Size = new System.Drawing.Size(250, 100);
                                                bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                                                bCodeControl.Dispose();
                                            }
                                            catch
                                            {

                                            }
                                            if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                            {
                                                strupimage = "<img width=\"160px\" src=\"http://www.halfpricedrapes.us" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                            }
                                        }
                                    }

                                }
                            }


                        }
                    }

                    if (variantName == null || variantName.Length == 0)
                    {
                        DataSet dsoption = new DataSet();
                        dsoption = CommonComponent.GetCommonDataSet("SELECT UPC FROM tb_Product WHERE ProductID=" + dsCart.Tables[0].Rows[i]["productId"] + " and isnull(upc,'')<>'' AND SKU='" + dsCart.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "'");

                        if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0 && string.IsNullOrEmpty(strupimage))
                        {
                            if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                            {

                                String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                                CreateFolder(FPath.ToString());
                                if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                {
                                    strupimage = "<img width=\"160px\" src=\"http://www.halfpricedrapes.us" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                }
                                else
                                {
                                    if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                    {
                                        try
                                        {
                                            DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                                            bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                            bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                                            bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                                            bCodeControl.BarCodeHeight = 70;
                                            bCodeControl.ShowHeader = false;
                                            bCodeControl.ShowFooter = true;
                                            bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                            bCodeControl.Size = new System.Drawing.Size(250, 100);
                                            bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                                            bCodeControl.Dispose();
                                        }
                                        catch
                                        {

                                        }
                                        if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                        {
                                            strupimage = "<img width=\"160px\" src=\"http://www.halfpricedrapes.us" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                        }
                                    }
                                }

                            }
                        }
                        else
                        {

                            dsoption = CommonComponent.GetCommonDataSet("SELECT UPC FROM tb_Product WHERE StoreId=1 and isnull(deleted,0)=0 AND SKU='" + dsCart.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "' and isnull(upc,'')<>''");
                            if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count == 0 && string.IsNullOrEmpty(strupimage))
                            {
                                dsoption = CommonComponent.GetCommonDataSet("SELECT Top 1 SKU,UPC,Header FROM tb_ProductVariantValue WHERE SKU=" + dsCart.Tables[0].Rows[i]["SKU"].ToString() + " and isnull(upc,'')<>''");
                            }
                            if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0 && string.IsNullOrEmpty(strupimage))
                            {
                                if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                                {

                                    String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                                    CreateFolder(FPath.ToString());
                                    if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                    {
                                        strupimage = "<img width=\"160px\" src=\"http://www.halfpricedrapes.us" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                    }
                                    else
                                    {
                                        if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                        {
                                            try
                                            {
                                                DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                                                bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                                                bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                                                bCodeControl.BarCodeHeight = 70;
                                                bCodeControl.ShowHeader = false;
                                                bCodeControl.ShowFooter = true;
                                                bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                bCodeControl.Size = new System.Drawing.Size(250, 100);
                                                bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                                                bCodeControl.Dispose();
                                            }
                                            catch
                                            {

                                            }
                                            if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                            {
                                                strupimage = "<img width=\"160px\" src=\"http://www.halfpricedrapes.us" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strupimage))
                        {
                            DataSet dsoption = new DataSet();

                            if (dsCart.Tables[0].Rows[i]["MerchantSKU"].ToString().Replace("'", "''").ToLower().IndexOf("-cus") > -1)
                            {
                                dsoption = CommonComponent.GetCommonDataSet("SELECT SKU,UPC,Header FROM tb_ProductVariantValue WHERE ProductID=" + dsCart.Tables[0].Rows[i]["productId"].ToString().Replace("'", "''") + "  and isnull(upc,'')<>'' AND SKU='" + dsCart.Tables[0].Rows[i]["MerchantSKU"].ToString().Replace("'", "''") + "'");
                            }
                            if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0)
                            {
                            }
                            else
                            {

                                dsoption = CommonComponent.GetCommonDataSet("SELECT UPC FROM tb_Product WHERE StoreId=1 and isnull(deleted,0)=0 AND SKU='" + dsCart.Tables[0].Rows[i]["SKU"].ToString().Replace("'", "''") + "' and isnull(upc,'')<>''");
                            }

                            if (dsoption != null && dsoption.Tables.Count > 0 && dsoption.Tables[0].Rows.Count > 0 && string.IsNullOrEmpty(strupimage))
                            {
                                if (!string.IsNullOrEmpty(dsoption.Tables[0].Rows[0]["UPC"].ToString()))
                                {

                                    String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodePath"));
                                    CreateFolder(FPath.ToString());
                                    if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                    {
                                        strupimage = "<img width=\"160px\" src=\"http://www.halfpricedrapes.us" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                    }
                                    else
                                    {
                                        if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                        {
                                            try
                                            {
                                                DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                                                bCodeControl.BarCode = dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                                                bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                                                bCodeControl.BarCodeHeight = 70;
                                                bCodeControl.ShowHeader = false;
                                                bCodeControl.ShowFooter = true;
                                                bCodeControl.FooterText = "UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim();
                                                bCodeControl.Size = new System.Drawing.Size(250, 100);
                                                bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png"));
                                                bCodeControl.Dispose();
                                            }
                                            catch
                                            {

                                            }
                                            if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png")))
                                            {
                                                strupimage = "<img width=\"160px\" src=\"http://www.halfpricedrapes.us" + FPath + "/UPC-" + dsoption.Tables[0].Rows[0]["UPC"].ToString().Trim() + ".png" + "\" />";
                                            }
                                        }
                                    }

                                }
                            }
                        }
                    }
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td style=\"text-align: center;\">";
                    if (!string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["MerchantSKU"].ToString()))
                    {
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["MerchantSKU"].ToString();
                    }
                    else
                    {
                        ltrCart.Text += dsCart.Tables[0].Rows[i]["SKU"].ToString();
                    }
                    ltrCart.Text += "</td>";
                    ltrCart.Text += "<td style=\"text-align: center;\">";

                    ltrCart.Text += strupimage.ToString();

                    ltrCart.Text += "</td>";

                    decimal CouponDiscount = 0;
                    if (SwatchQty != 0)
                    {
                        Int32 Isorderswatch = 0;// Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT 1 FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["ProductID"].ToString() + " and ItemType='Swatch'"));
                        if (Isorderswatch == 1)
                        {
                            if (Convert.ToInt32(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) >= SwatchQty)
                            {
                                Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["ProductID"].ToString() + ""));
                                //price
                                ltrCart.Text += "<td style=\"text-align: right;\">";
                                ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(pp));
                                ltrCart.Text += "</td>";
                                //discount price

                                if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                {
                                    decimal.TryParse(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString().ToString(), out CouponDiscount);
                                    ltrCart.Text += "<td style=\"text-align: right;\">" + Convert.ToDecimal(CouponDiscount).ToString("C") + "</td>";
                                }
                                //qty
                                ltrCart.Text += "<td style=\"text-align: center;\">";
                                ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                                ltrCart.Text += "</td>";
                                //totalprice
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
                                if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                {
                                    GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()) * Convert.ToDecimal(SwatchQty));
                                }
                                else
                                {
                                    GrandSwatchSubTotal += Convert.ToDecimal(Convert.ToDecimal(pp.ToString()) * Convert.ToDecimal(SwatchQty));
                                }
                                SwatchQty = 0;
                            }
                            else
                            {
                                Decimal pp = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT case when isnull(saleprice,0) > 0 then saleprice else isnull(price,0) end  FROM tb_Product WHERE Productid=" + dsCart.Tables[0].Rows[i]["ProductID"].ToString() + ""));
                                //price
                                ltrCart.Text += "<td style=\"text-align: right;\">";
                                ltrCart.Text += "$" + String.Format("{0:0.00}", Convert.ToDecimal(pp));
                                ltrCart.Text += "</td>";
                                //discount price

                                if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                                {
                                    decimal.TryParse(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString().ToString(), out CouponDiscount);
                                    ltrCart.Text += "<td style=\"text-align: right;\">" + Convert.ToDecimal(CouponDiscount).ToString("C") + "</td>";
                                }
                                //qty
                                ltrCart.Text += "<td style=\"text-align: center;\">";
                                ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                                ltrCart.Text += "</td>";
                                //totalprice
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
                            //price
                            ltrCart.Text += "<td style=\"text-align: right;\">";
                            ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C");
                            ltrCart.Text += "</td>";
                            //discountprice
                            if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                            {
                                decimal.TryParse(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString(), out CouponDiscount);
                                ltrCart.Text += "<td style=\"text-align: right;\">" + Convert.ToDecimal(CouponDiscount).ToString("C") + "</td>";
                            }
                            //qty
                            ltrCart.Text += "<td style=\"text-align: center;\">";
                            ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                            ltrCart.Text += "</td>";
                            ltrCart.Text += "<td style=\"text-align: right;\">";
                            //totalprice
                            decimal Totalprice = 0;
                            if (CouponDiscount > 0 && IsCouponDiscount == true)
                            {
                                Totalprice = Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) * Convert.ToDecimal(CouponDiscount);
                            }
                            else
                            {
                                Totalprice = Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) * Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString());
                            }
                            ltrCart.Text += Convert.ToDecimal(Totalprice.ToString()).ToString("C");

                            GrandSwatchSubTotal += Totalprice;
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

                        decimal Totalprice = 0;
                        if (CouponDiscount > 0 && IsCouponDiscount == true)
                        {
                            Totalprice = Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) * Convert.ToDecimal(CouponDiscount);
                        }
                        else
                        {
                            Totalprice = Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) * Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString());
                        }
                        ltrCart.Text += Convert.ToDecimal(Totalprice.ToString()).ToString("C");

                        GrandSwatchSubTotal += Totalprice;
                        ltrCart.Text += "</td>";
                        ltrCart.Text += "</tr>";

                    }

                    //ltrCart.Text += "</td>";
                    //ltrCart.Text += "<td style=\"text-align: right;\">";
                    //ltrCart.Text += Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString()).ToString("C");
                    //ltrCart.Text += "</td>";

                    //decimal CouponDiscount = 0;
                    //if (IsCouponDiscount == true && !string.IsNullOrEmpty(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString()))
                    //{
                    //    decimal.TryParse(dsCart.Tables[0].Rows[i]["DiscountPrice"].ToString(), out CouponDiscount);
                    //    ltrCart.Text += "<td style=\"text-align: right;\">" + Convert.ToDecimal(CouponDiscount).ToString("C") + "</td>";
                    //}
                    //ltrCart.Text += "<td style=\"text-align: center;\">";
                    //ltrCart.Text += dsCart.Tables[0].Rows[i]["Quantity"].ToString();
                    //ltrCart.Text += "</td>";
                    //ltrCart.Text += "<td style=\"text-align: right;\">";
                    //decimal Totalprice = 0;
                    //if (CouponDiscount > 0 && IsCouponDiscount == true)
                    //{
                    //    Totalprice = Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) * Convert.ToDecimal(CouponDiscount);
                    //}
                    //else
                    //{
                    //    Totalprice = Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Quantity"].ToString()) * Convert.ToDecimal(dsCart.Tables[0].Rows[i]["Price"].ToString());
                    //}
                    //ltrCart.Text += Convert.ToDecimal(Totalprice.ToString()).ToString("C");

                    //ltrCart.Text += "</td>";
                    //ltrCart.Text += "</tr>";
                }
            }

            string StrColapan = "";
            if (IsCouponDiscount == true)
            {
                StrColapan = "6";
            }
            else { StrColapan = "5"; }

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
                else
                {
                    Subtotal = Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["OrderSubtotal"].ToString()) + AdjustmentAmount;
                }
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
                Decimal Discount = custDist + Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["LevelDiscountAmount"]) + Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["CouponDiscountAmount"]) + Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["QuantityDiscountAmount"]), 2);

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
                {
                    FinalTotal = Math.Round(Convert.ToDecimal(dsOrderdata.Tables[0].Rows[0]["OrderTotal"].ToString()), 2) + AdjustmentAmount - RefundAmt;
                }

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
            if (IsCouponDiscount == true && ltrCart.Text.ToString().ToLower().IndexOf("###coupondiscount###") > -1)
            {
                string StrCoupon = "<th valign=\"top\" align=\"center\" style=\"width: 10%; text-align: center;\"><b>Discount Price</b></th>";
                ltrCart.Text = ltrCart.Text.Replace("###coupondiscount###", StrCoupon.ToString().Trim());
            }
            else
            {
                ltrCart.Text = ltrCart.Text.Replace("###coupondiscount###", "");
            }





            ltrCart.Text += "</table>";

            if (!string.IsNullOrEmpty(dsOrderdata.Tables[0].Rows[0]["OrderNotes"].ToString()))
            {
                trordernotes.Visible = true;
                ltordernotes.Text = dsOrderdata.Tables[0].Rows[0]["OrderNotes"].ToString();
            }
            if (!string.IsNullOrEmpty(dsOrderdata.Tables[0].Rows[0]["Notes"].ToString()))
            {
                trcustnotes.Visible = true;
                ltcustomernotes.Text = dsOrderdata.Tables[0].Rows[0]["Notes"].ToString();
            }
        }

        /// <summary>
        /// function for Bind Invoice Signature
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

        /// <summary>
        ///  Invoice Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            int OrderNumber = 0;
            bool chkOrder = Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
            if (chkOrder)
            {
                SendMail(OrderNumber);
                OrderComponent objOrderlog = new OrderComponent();
                objOrderlog.InsertOrderlog(13, Convert.ToInt32(OrderNumber), "", Convert.ToInt32(Session["AdminID"].ToString()));
            }
        }

        /// <summary>
        ///  Sendmail Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSendmail_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtCustEmail.Text.ToString()))
            {
                int OrderNumber = 0;
                bool chkOrder = Int32.TryParse(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()), out OrderNumber);
                if (chkOrder)
                {
                    string Body = "";
                    //  string url = "Invoicemail.aspx?ONo=" + OrderNumber.ToString();//Request.Url.ToString();
                    string url = "http://www.halfpricedrapes.us/Admin/Orders/" + "Invoicefullemail.aspx?ONo=" + OrderNumber.ToString();
                    WebRequest NewWebReq = WebRequest.Create(url);
                    WebResponse newWebRes = NewWebReq.GetResponse();
                    string format = newWebRes.ContentType;
                    Stream ftprespstrm = newWebRes.GetResponseStream();
                    StreamReader reader;
                    reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
                    Body = reader.ReadToEnd().ToString();
                    Body = txtEmailBody.Text.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"visibility:hidden;\"");
                    Body = Body.Replace("id=\"contatfooterdetail\">", "id=\"contatfooterdetail\">" + AppLogic.AppConfigs("Templatefootercommunication").ToString());
                    AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
                    try
                    {
                        string ToID = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT isnull(EmailID,'') FROM tb_ContactEmail WHERE Subject='Order Update'"));
                        if (!string.IsNullOrEmpty(ToID))
                        {
                            if (Session["AdminID"] != null)
                            {
                                String email = Convert.ToString(CommonComponent.GetScalarCommonData("select EmailID from tb_Admin where AdminID=" + Convert.ToInt32(Session["AdminID"].ToString()) + ""));
                                if (!String.IsNullOrEmpty(email))
                                {
                                    ToID = ToID + ";" + email;

                                }
                            }

                            CommonOperations.SendMail(ToID + ";" + txtCustEmail.Text.ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                        }
                        else
                        {
                            CommonOperations.SendMail(txtCustEmail.Text.ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
                        }


                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.Tabdisplay(" + hdntabid.Value + ");jAlert('Mail has been sent successfully.','Message'); window.parent.document.getElementById('prepage').style.display = 'none'; ", true);
                    }
                    catch
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.Tabdisplay(" + hdntabid.Value + ");jAlert('Mail Sending Problem.','Error');window.parent.document.getElementById('prepage').style.display = 'none';", true);
                    }
                    OrderComponent objOrderlog = new OrderComponent();
                    objOrderlog.InsertOrderlog(13, Convert.ToInt32(OrderNumber), "", Convert.ToInt32(Session["AdminID"].ToString()));
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@valid", "alert('Please enter Valid Email');", true);
                return;
            }
        }

        /// <summary>
        /// Order Receipt Send To Customer by Email 
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNUmber</param>
        public void SendMail(Int32 OrderNumber)
        {
            string Body = "";
            //  string url = "Invoicemail.aspx?ONo=" + OrderNumber.ToString();//Request.Url.ToString();
            string url = "http://www.halfpricedrapes.us/Admin/Orders/" + "Invoicemail.aspx?ONo=" + OrderNumber.ToString();
            WebRequest NewWebReq = WebRequest.Create(url);
            WebResponse newWebRes = NewWebReq.GetResponse();
            string format = newWebRes.ContentType;
            Stream ftprespstrm = newWebRes.GetResponseStream();
            StreamReader reader;
            reader = new StreamReader(ftprespstrm, System.Text.Encoding.UTF8);
            Body = reader.ReadToEnd().ToString();
            Body = Body.Replace("class=\"Printinvoice\"", "class=\"Printinvoice\" style=\"visibility:hidden;\"");
            //Body = Body.Replace("<body ", "<body style='background:none;' ");

            string[] stroverstock = Regex.Split(Body, "\"body12\"", RegexOptions.IgnoreCase);
            if (stroverstock.Length > 0)
            {
                string strdata = stroverstock[1].ToString();
                strdata = strdata.Substring(strdata.IndexOf("<table") - 6);
                strdata = strdata.Substring(0, strdata.LastIndexOf("</table>") + 8);
                strdata = @"<style type='text/css'>
        .datatable table
        {
            border: 1px solid #eeeeee;
        }
        .datatable tr.alter_row
        {
            background-color: #f9f9f9;
        }
        .datatable td
        {
            padding: 2px 2px;
            text-align: left;
            border: 1px solid #eeeeee;
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            color: #4c4c4c;
            line-height: 16px;
        }
        .datatable th
        {
            padding: 2px 3px;
            text-align: left;
            border: 1px solid #eeeeee;
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            font-weight: bold;
            color: #4c4c4c;
            line-height: 16px;
        }
        .receiptfont
        {
            font: 11px/14px Verdana, Arial, Helvetica, sans-serif;
            color: #4c4c4c;
        }
        .receiptlineheight
        {
            height: 15px;
        }
        .popup_cantain
        {
            font: 11px/14px Verdana,Arial,Helvetica,sans-serif;
            color: #4C4C4C;
            text-decoration: none;
        }
        .popup_cantain a
        {
            font: 11px/14px Verdana,Arial,Helvetica,sans-serif;
            color: #FE0000;
            text-decoration: none;
        }
        .popup_cantain a:hover
        {
            font: 11px/14px Verdana,Arial,Helvetica,sans-serif;
            color: #000;
            text-decoration: underline;
        }
        .Printinvoice
        {
        }
    </style>
    <style type='text/css' media='print'>
        .Printinvoice
        {
            display: none;
        }
    </style>" + strdata;
                txtEmailBody.Text = strdata.ToString();



            }
            else
            {
                txtEmailBody.Text = Body.ToString();
            }


            //AlternateView av = AlternateView.CreateAlternateViewFromString(Body, null, "text/html");
            //try
            //{
            //    CommonOperations.SendMail(hdmemail.Value.ToString(), "Receipt for Order #" + OrderNumber.ToString() + " from " + AppLogic.AppConfigs("StoreName").ToString(), Body, Request.UserHostAddress.ToString(), true, av);
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.Tabdisplay(" + hdntabid.Value + ");jAlert('Mail has been sent successfully.','Message'); window.parent.document.getElementById('prepage').style.display = 'none'; ", true);
            //}
            //catch
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "window.parent.Tabdisplay(" + hdntabid.Value + ");jAlert('Mail Sending Problem.','Error');window.parent.document.getElementById('prepage').style.display = 'none';", true);
            //}
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
        /// <param name="state">object state.</param>
        protected override void SavePageStateToPersistenceMedium(object state)
        {
            LosFormatter los = new LosFormatter();
            System.IO.StringWriter sw = new System.IO.StringWriter();
            los.Serialize(sw, state);
            string vs = sw.ToString();
            Session[Session.SessionID] = vs;
        }

        #endregion

        /// <summary>
        /// Bind Ref Number Details
        /// </summary>
        public void BindRefNumberDetails()
        {
            OrderComponent objordercomp = new OrderComponent();
            DataSet dsorder = objordercomp.GetRefNumberByOrderNumber(Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString())));
            if (dsorder != null && dsorder.Tables.Count > 0)
            {
                if (dsorder.Tables[0].Rows.Count > 0 && dsorder.Tables[1].Rows.Count > 0)
                {
                    trRefOrderNo.Visible = true;
                    ltrstore.Text = dsorder.Tables[0].Rows[0]["StoreName"].ToString() + " Order #";
                    if (!string.IsNullOrEmpty(dsorder.Tables[1].Rows[0]["RefOrderID"].ToString()) && dsorder.Tables[1].Rows[0]["RefOrderID"].ToString() != "0")
                    {
                        ltrRef.Text = dsorder.Tables[1].Rows[0]["RefOrderID"].ToString();
                    }
                    else
                    {
                        trRefOrderNo.Visible = false;
                    }
                }
                else
                {
                    trRefOrderNo.Visible = false;
                }
            }
        }
    }
}