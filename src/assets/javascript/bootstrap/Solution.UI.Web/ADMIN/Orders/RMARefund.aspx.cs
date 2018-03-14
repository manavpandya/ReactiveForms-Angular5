using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using System.Text;
using System.Net.Mail;

namespace Solution.UI.Web.ADMIN.Orders
{
    public partial class RMARefund : BasePage
    {
        public int OrderNumber;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            Cart();
            if (!IsPostBack)
            {
                OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()));
                RefundCartBind();
            }
        }

        /// <summary>
        /// Bind Cart
        /// </summary>
        private void Cart()
        {
            String[] formkeys = Request.Form.AllKeys; foreach (String s in formkeys)
            {
                //For Update Quantity of Product in Cart and Bind Cart again
                if (!string.IsNullOrEmpty(s))
                {
                    if (s.Contains("RefundAmount"))
                    {
                        try
                        {
                            String str = "";
                            str = Request.Form[s].ToString();
                            String[] p = s.Split(':');
                            int ProductID = 0;
                            Int32 Ordernumber = 0;
                            Decimal RefundAmount = 0;
                            Int32.TryParse(p[2].ToString(), out ProductID);
                            Int32.TryParse(p[3].ToString(), out Ordernumber);
                            Decimal.TryParse(str, out RefundAmount);
                            if (RefundAmount != Decimal.Zero)
                                RefundOrderbyProductID(ProductID, RefundAmount, Ordernumber);
                        }
                        catch { }
                    }
                }
            }
        }

        /// <summary>
        /// Refunds the cart bind.
        /// </summary>
        private void RefundCartBind()
        {
            try
            {
                decimal QtyDiscount = 0;
                bool transctionstate = false;
                bool isauthorizeRefund = false;
                bool isRefunded = false;
                bool isShipped = false;
                bool isfullRefund = false;
                String Name = String.Empty;
                //SCartID = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT tb_OrderShoppingCart.OrderedShoppingCartID FROM tb_OrderShoppingCart Where OrderNumber=" + Convert.ToInt32(OrderNumber)) + "");
                StringBuilder Table = new StringBuilder();
                //if (SCartID != -111)
                //{
                DataSet DsCItems = new DataSet();
                DataSet dsRefundCheck = new DataSet();
                //DsCItems = ObjOSItems.GetOrderedShoppingCartItemsAndReturnIDByOrderId(Convert.ToInt32(OrderNumber));
                string Query = "SELECT isnull(tb_ReturnItem.ReturnFee,'0') as ReturnFee,tb_Product.Name,isnull(isauthorizeRefund,'0') as isauthorizeRefund,isnull(IsRefunded,0) as IsRefunded,tb_Product.SKU, tb_OrderedShoppingCartItems.Quantity, " +
                                " tb_OrderedShoppingCartItems.RefProductID,isnull(tb_OrderedShoppingCartItems.RefundAmount,0) as 'RefundAmount', " +
                                " tb_OrderedShoppingCartItems.VariantNames,tb_OrderedShoppingCartItems.VariantValues, " +
                                " isnull(s.TrackingNumber,o.TrackingNumber) TrackingNumber,isnull(s.ShippedVia,o.ShippedVia) ShippedVia, " +
                                " (isnull(s.Shipped,0) | o.shipped) as Shipped,isnull(s.ShippedOn,o.ShippedOn) ShippedOn,  " +
                                " tb_Product.Description,tb_OrderedShoppingCartItems.Price As SalePrice, tb_ReturnItem.ReturnItemID,isnull(tb_ReturnItem.Quantity,0) as Return_Qty  " +
                                " FROM tb_Product INNER JOIN tb_OrderedShoppingCartItems " +
                                " left outer join (select RefProductID,OrderNumber,trackingNumber,ShippedVia,Shipped,ShippedOn from  " +
                                " tb_OrderShippedItems where ordernumber=" + OrderNumber + ") s on s.RefProductID=tb_OrderedShoppingCartItems.RefProductID  " +
                                " inner join (select convert(bit,isnull(len(ShippedOn),0)) as shipped,ShoppingCardID,ShippingTrackingNumber " +
                                " as TrackingNumber,ShippedVia,ShippedOn,ordernumber from tb_Order where ordernumber=" + OrderNumber + ") o ON  " +
                                " o.ShoppingCardID=tb_OrderedShoppingCartItems.OrderedShoppingCartID on " +
                                " tb_Product.ProductID = tb_OrderedShoppingCartItems.RefProductID   " +
                                " inner JOIN  dbo.tb_ReturnItem ON dbo.tb_Product.ProductID = dbo.tb_ReturnItem.ProductID and tb_OrderedShoppingCartItems.OrderedCustomCartID=tb_ReturnItem.OrderedCustomCartID AND  o.OrderNumber = dbo.tb_ReturnItem.OrderedNumber  " +
                                " Where tb_ReturnItem.ReturnType='RR' ";
                DsCItems = CommonComponent.GetCommonDataSet(Query);

                if (DsCItems != null && DsCItems.Tables[0].Rows.Count > 0)
                {
                    //dsRefundCheck = ObjOrder.GetOrderForRefundedOrderById(OrderNumber);
                    string Query01 = " SELECT tb_Payment.PaymentType as PaymentMethod,VoidedOn,tb_Order.StoreId,CapturedOn,RefundedOn,CustomerID,Email, " +
                                    " TransactionStatus as TransactionState,PaymentGateway,CaptureTXCommand,CaptureTXResult,OrderSubTotal,OrderTotal, " +
                                    " isnull(AdjustmentAmount,0) as AdjustmentAmount  " +
                                    " FROM tb_Payment INNER JOIN  tb_Order ON tb_Payment.PaymentType =tb_Order.PaymentMethod  " +
                                    " Where OrderNumber=" + OrderNumber + "";
                    dsRefundCheck = CommonComponent.GetCommonDataSet(Query01);
                    if (dsRefundCheck != null && dsRefundCheck.Tables[0].Rows.Count > 0)
                    {
                        if (dsRefundCheck.Tables[0].Rows[0]["TransactionState"].ToString() == AppLogic.ro_TXStateCaptured || dsRefundCheck.Tables[0].Rows[0]["TransactionState"].ToString() == AppLogic.ro_TXStateRefunded)
                        {
                            transctionstate = true;
                        }
                    }
                    Table.Append(" <table border='0' cellpadding='0' cellspacing='0' class='datatable' width='100%'> ");
                    Table.Append("<tbody><tr  >");
                    Table.Append("<th align='center' valign='middle' style='width:7%' ><b>RMA No.</b></th>");
                    Table.Append("<th align='left' valign='middle' style='width:30%' ><b>Product</b></th>");
                    Table.Append("<th align='left' valign='middle' style='width:10%' ><b> SKU</b></th>");
                    Table.Append("<th align='center' valign='middle' style='width:10%'><b>Price</b></th>");
                    Table.Append("<th valign='middle' style='width: 7%;text-align:center;'><b>Returned Quantity</b></th>");
                    Table.Append("<th valign='middle' style='width: 10%;text-align:center;'><b>Sub Total</b></th>");
                    Table.Append("<th valign='middle' style='width: 15%;text-align:center;'><b>Return Fee</b></th>");
                    if (transctionstate == true)
                    {
                        Table.Append("<th valign='middle' style='width: 10%;text-align:center;'><b>Refund Amount</b></th>");
                        Table.Append("<th align='center' valign='middle' style='width: 10%;text-align:center;'><b>Refund<br/>Status</b></th>");
                    }

                    Table.Append("</tr>");
                    decimal TPrice = 0;
                    decimal QtyDiscountPercent = 0;

                    for (int i = 0; i < DsCItems.Tables[0].Rows.Count; i++)
                    {
                        decimal Price1 = 0;
                        decimal PTemp = 0;

                        isRefunded = Convert.ToBoolean(DsCItems.Tables[0].Rows[i]["IsRefunded"].ToString());
                        isShipped = !string.IsNullOrEmpty(DsCItems.Tables[0].Rows[i]["ShippedOn"].ToString());
                        //isauthorizeRefund = Convert.ToBoolean(DsCItems.Tables[0].Rows[i]["isauthorizeRefund"].ToString());
                        isauthorizeRefund = true;

                        if (DsCItems.Tables[0].Rows[i]["SalePrice"].ToString() != null && DsCItems.Tables[0].Rows[i]["SalePrice"].ToString() != "")
                            PTemp = Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["SalePrice"].ToString());
                        else
                            PTemp = 0;
                        PTemp = Math.Round(PTemp, 2);
                        Price1 = PTemp * Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["Quantity"].ToString());
                        QtyDiscountPercent = GetQtyDiscount(Convert.ToInt32(DsCItems.Tables[0].Rows[i]["RefProductID"].ToString()), Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString()));
                        QtyDiscount += (Price1 * QtyDiscountPercent) / 100;
                        QtyDiscount = Math.Round(QtyDiscount, 2);
                        TPrice += Price1;
                        Table.Append("<tr align='center'  valign='middle'>");
                        Table.Append("<tr >");
                        if (string.IsNullOrEmpty(DsCItems.Tables[0].Rows[i]["ReturnItemID"].ToString()))
                        {
                            Table.Append("<td align='center' style='text-align:center;' valign='top'> - ");
                        }
                        else
                            Table.Append("<td align='center' style='text-align:center;' valign='top'>" + DsCItems.Tables[0].Rows[i]["ReturnItemID"].ToString());

                        Table.Append("<td align='left' valign='top'>" + DsCItems.Tables[0].Rows[i]["Name"].ToString());

                        string[] Names = DsCItems.Tables[0].Rows[i]["VariantNames"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        string[] Values = DsCItems.Tables[0].Rows[i]["VariantValues"].ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        int iLoopValues = 0;
                        for (iLoopValues = 0; iLoopValues < Values.Length && Names.Length == Values.Length; iLoopValues++)
                        {
                            Table.Append("<br/>" + Names[iLoopValues] + ": " + Values[iLoopValues]);
                        }
                        Table.Append("</td>");
                        Table.Append("<td  align='left' >" + DsCItems.Tables[0].Rows[i]["SKU"].ToString() + "</td>");
                        Table.Append("<td  STYLE='text-align : right;'>$" + PTemp.ToString() + "</td>");
                        int return_qtu = Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Return_Qty"].ToString());
                        Table.Append("<td STYLE='text-align : center;'>" + DsCItems.Tables[0].Rows[i]["Return_Qty"].ToString() + "</td>");
                        decimal subtotal;
                        if (return_qtu > 0)
                        {
                            subtotal = Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["Return_Qty"].ToString()) * Convert.ToDecimal(PTemp.ToString());
                        }
                        else
                        {
                            return_qtu = Convert.ToInt32(DsCItems.Tables[0].Rows[i]["Quantity"].ToString());
                            subtotal = Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["Quantity"].ToString()) * Convert.ToDecimal(PTemp.ToString());
                        }
                        Table.Append("<td  STYLE='text-align : right;'>$" + Math.Round(subtotal, 2) + "</td>");
                        Table.Append("<td  STYLE='text-align : center;'>" + DsCItems.Tables[0].Rows[i]["ReturnFee"].ToString() + "%</td>");

                        decimal TotalDiscount = 0;
                        DataSet asDiscountAmount = new DataSet();
                        asDiscountAmount = CommonComponent.GetCommonDataSet("Select SUM(ISNULL(tb_OrderedShoppingCartItems.Quantity,0))  as TotQty, " +
                                                           " SUM(ISNULL(CustomDiscount,0)+ISNULL(CouponDiscountAmount,0)+ ISNULL(GiftCertificateDiscountAmount,0)+ISNULL(GiftWrapAmt,0)+ISNULL(LevelDiscountAmount,0)+ISNULL(QuantityDiscountAmount,0)) as TotDiscountAmount " +
                                                           " from tb_Order Inner Join tb_OrderedShoppingCartItems on tb_OrderedShoppingCartItems.OrderedShoppingCartID =tb_Order.ShoppingCardID " +
                                                           " where OrderNumber = " + OrderNumber + "");

                        if (asDiscountAmount != null && asDiscountAmount.Tables.Count > 0 && asDiscountAmount.Tables[0].Rows.Count > 0)
                        {
                            TotalDiscount = Convert.ToDecimal(Convert.ToDecimal(asDiscountAmount.Tables[0].Rows[0]["TotDiscountAmount"].ToString()) / Convert.ToInt32(asDiscountAmount.Tables[0].Rows[0]["TotQty"].ToString()));
                        }
                        Decimal returnamt = 0;
                        if (TotalDiscount > 0)
                        {
                            returnamt = subtotal - TotalDiscount * (return_qtu);
                            returnamt = returnamt * (Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["ReturnFee"].ToString()) / 100);
                        }
                        else
                        {
                            returnamt = subtotal * (Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["ReturnFee"].ToString()) / 100);
                            returnamt = subtotal - returnamt;
                        }
                        if (returnamt < 0) { returnamt = 0; }
                        Name = "RefundAmount:" + 0 + ":" + DsCItems.Tables[0].Rows[i]["RefProductID"].ToString() + ":" + OrderNumber;
                        if (dsRefundCheck != null && dsRefundCheck.Tables[0].Rows.Count > 0)
                        {
                            if (dsRefundCheck.Tables[0].Rows[0]["TransactionState"].ToString() == AppLogic.ro_TXStateRefunded)
                            {
                                isfullRefund = true;
                            }
                        }

                        if (transctionstate)
                        {
                            if (isShipped)
                            {
                                if (isauthorizeRefund)
                                {
                                    if (isRefunded)
                                    {
                                        Table.Append("<td align='center' valign='middle'>$" + Math.Round(Convert.ToDecimal(DsCItems.Tables[0].Rows[i]["RefundAmount"].ToString()), 2) + "</td>");
                                        Table.Append("<td STYLE='text-align : center;'>Refunded</td>");
                                    }
                                    else if (isfullRefund)
                                    {
                                        Table.Append("<td align='center' valign='middle'>--</td>");
                                        Table.Append("<td STYLE='text-align : center;'>Refunded</td>");
                                    }
                                    else
                                    {
                                        Table.Append("<td align='center' valign='middle'>$" + returnamt.ToString("f2") + "<input  type='text' runat='server'  class='wish_list_quantity' name='" + Name + "' id='" + Name + "' size='7' value='" + returnamt + "' maxlength='8' style='display:none;' /></td>");
                                        Table.Append("<td STYLE='text-align : center;'><a onclick=\"if(confirm('Are you sure to refund : $" + returnamt.ToString("f2") + " Amount')){RefundProductClick('" + i + "_" + DsCItems.Tables[0].Rows[i]["RefProductID"].ToString() + "','" + returnamt.ToString("f2") + "');} else {return false;}\"><img src='/App_Themes/" + Page.Theme + "/images/Refund.png' /></a></td>");
                                    }
                                }
                                else
                                {
                                    Table.Append("<td align='center' valign='middle'>" + returnamt.ToString("f2") + "<input  type='text' runat='server'   class='wish_list_quantity' name='" + Name + "' id='" + Name + "' size='7' value='" + returnamt + "' maxlength='8'  style='display:none;'  /></td>");
                                    Table.Append("<td STYLE='text-align : center;'>Not Authorized</td>");
                                }
                            }
                            else
                            {
                                Table.Append("<td align='center' valign='middle'>-</td>");
                                Table.Append("<td STYLE='text-align : center;'>Not Shipped</td>");
                            }
                        }
                        Table.Append(" </tr>");
                    }
                    Table.Append("</tbody></table>");
                    TPrice = (TPrice - QtyDiscount);
                    TPrice = Math.Round(TPrice, 2);
                }
                else
                {
                    Table.AppendLine("<font color='red' CLASS='font-red'>Your Shopping Cart is Empty.</font>");
                }
                //}
                //else
                //{
                //    Table.Append(" <table border='0' cellpadding='0' cellspacing='0' class='tablecart' width='100%'> ");
                //    Table.Append("<tbody><tr style='line-height: 50px;' >");
                //    Table.Append("<th align='left' valign='middle' style='width:70%' ><b>Product</b></th>");
                //    Table.Append("<th align='center' valign='middle' style='width:10%'><b>Price</b></th>");
                //    Table.Append("<th align='center' valign='middle' ><b>Quantity</b></th>");
                //    Table.Append("<th style='text-align: right;'><b>Sub Total:</b></th>");
                //    Table.Append("</tr>");
                //    Table.Append("<tr align='center'  valign='middle'>");
                //    Table.Append("<td  align='left' >Order Catalog</td>");
                //    Table.Append("<td  >$5.00</td>");
                //    Table.Append("<td >1</td>");
                //    Table.Append("<td  STYLE='text-align:right;'> $5.00</td>");
                //    Table.Append(" </tr>");
                //    Table.Append("</tbody></table>");
                //}
                ltCartRefund.Text = Table.ToString();

            }
            catch (Exception ex)
            {

            }
        }

        /// <summary>
        /// Gets the Order for Refunded Order by Productid
        /// </summary>
        /// <param name="OrderNo">int OrderNo</param>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns the Order List for refunded order by product id</returns>
        public DataSet GetOrderForRefundedOrderByProductId(Int32 OrderNo, int ProductID)
        {
            DataSet dsOrderreturn = new DataSet();
            string Query = " SELECT tb_Payment.PaymentType as PaymentMethod,VoidedOn,tb_Order.StoreId,CapturedOn,RefundedOn,CustomerID,Email,TransactionStatus as TransactionState,PaymentGateway,CaptureTXCommand,CaptureTXResult,OrderSubTotal,OrderTotal, " +
                            " dbo.tb_OrderedShoppingCartItems.RefProductID, dbo.tb_OrderedShoppingCartItems.Price,  dbo.tb_OrderedShoppingCartItems.Quantity,(dbo.tb_OrderedShoppingCartItems.Quantity)*(dbo.tb_OrderedShoppingCartItems.Price) as PriceTotal  " +
                            " FROM  tb_Payment INNER JOIN tb_Order ON tb_Payment.PaymentType = tb_Order.PaymentMethod  " +
                            " inner join dbo.tb_OrderedShoppingCartItems ON  dbo.tb_Order.ShoppingCardID = dbo.tb_OrderedShoppingCartItems.OrderedShoppingCartID  " +
                            " Where OrderNumber=" + OrderNo + " and tb_OrderedShoppingCartItems.RefProductID=" + ProductID + "";
            dsOrderreturn = CommonComponent.GetCommonDataSet(Query);
            return dsOrderreturn;
        }

        /// <summary>
        ///  Refund Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void BtnRefundProduct_Click(object sender, ImageClickEventArgs e)
        {
            Decimal refundAmount = Decimal.Zero;
            String RefundReason = "";
            DataSet ds = null;
            ds = new DataSet();
            string[] strProdSplit = hdnRefundProductID.Value.Split('_');
            int ProductID = Convert.ToInt32(strProdSplit[1].ToString());
            OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()));
            ds = GetOrderForRefundedOrderByProductId(OrderNumber, ProductID);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Int32 StoreID = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["StoreID"].ToString(), out StoreID);
                if (StoreID > 0)
                    AppConfig.StoreID = StoreID;
                refundAmount = Convert.ToDecimal(refundamaut.Value.ToString());
                //refundAmount = Convert.ToDecimal(ds.Tables[0].Rows[0]["PriceTotal"].ToString());
                string PMName = ds.Tables[0].Rows[0]["PaymentMethod"].ToString();
                string PM = PMName.Replace(" ", String.Empty).ToUpper();

                if (ds.Tables[0].Rows[0]["TransactionState"].ToString() == AppLogic.ro_TXStateCaptured || ds.Tables[0].Rows[0]["TransactionState"].ToString() == AppLogic.ro_TXStateRefunded)
                {
                    Decimal OrderTotal = 0;
                    Decimal.TryParse(ds.Tables[0].Rows[0]["OrderTotal"].ToString(), out OrderTotal);
                    if (refundAmount == Decimal.Zero)
                    {
                        lblMsgRefund.Text = "Invalid refund amount specified.";
                        return;
                    }
                    String Status = AppLogic.ro_OK;

                    if (PM == AppLogic.ro_PMCreditCard || PM == AppLogic.ro_PMPayPal || PM == AppLogic.ro_PMPayPalExpress)
                    {
                        //Status = Gateway.ProcessRefundByOrderANDProductID(Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerID"].ToString()), OrderNumber, 0, refundAmount, RefundReason, null, ProductID);
                        Status = AuthorizeNetComponent.RefundOrder(Convert.ToInt32(OrderNumber.ToString()), Convert.ToDecimal(refundAmount), RefundReason.ToString());
                    }
                    else
                    {
                        // Status = Gateway.ForceRefundStatus(OrderNumber);
                        Status = ForceRefundStatus(OrderNumber);
                    }
                    if (Status == AppLogic.ro_OK)
                    {
                        try
                        {
                            string strRefundAmount = string.Empty;
                            if (refundAmount != Decimal.Zero)
                                strRefundAmount = refundAmount.ToString("f2");
                            else
                                strRefundAmount = OrderTotal.ToString("f2");
                            CommonComponent.ExecuteCommonData("update op set op.IsRefunded=1 from tb_OrderedShoppingCartItems op join tb_Order o on op.OrderedShoppingCartID=o.ShoppingCartID and o.OrderNumber=" + OrderNumber + " and op.RefProductID=" + ProductID);
                            SendMail(ds.Tables[0].Rows[0]["Email"].ToString().Trim(), OrderNumber, strRefundAmount);
                        }
                        catch { }
                    }
                    lblMsgRefund.Text = "Refund Status: " + Status + "";
                }
                else
                {
                    lblMsgRefund.Text = "This transaction has not yet been Captured. Use Void if required. The transaction state (" + ds.Tables[0].Rows[0]["TransactionState"].ToString() + ") is not " + AppLogic.ro_TXModeAuthCapture + ".";
                }
            }
            else
            {
                lblMsgRefund.Text = "ORDER NOT FOUND";
            }
        }

        /// <summary>
        /// function for Force refund
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns the result as a String according to execution</returns>
        public static String ForceRefundStatus(int OrderNumber)
        {
            DataSet dsOrder = new DataSet();
            dsOrder = CommonComponent.GetCommonDataSet("Select * from tb_Order where OrderNumber=" + OrderNumber.ToString());
            if (dsOrder != null && dsOrder.Tables.Count > 0 && dsOrder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsOrder.Tables[0].Rows.Count; i++)
                {
                    if (!string.IsNullOrEmpty(dsOrder.Tables[0].Rows[i]["CapturedOn"].ToString()))
                    {
                        // make sure inventory was restored. safe to call repeatedly. proc protects against deducting twice
                        //   CommonComponent.ExecuteCommonData("EXEC usp_Product_AdjustInventory " + OrderNumber.ToString() + ",1, 0");
                    }
                }
                // update transactionstate
                CommonComponent.ExecuteCommonData("update tb_Order set RefundTXCommand='" + "ADMIN FORCED REFUND" + "', RefundReason='" + "ADMIN FORCED REFUND" + "', TransactionStatus='" + (AppLogic.ro_TXStateRefunded + "', RefundedOn=dateadd(hour,-2,getdate()), IsNew=0 where OrderNumber=" + OrderNumber.ToString()));
            }
            return AppLogic.ro_OK;
        }

        /// <summary>
        /// send refund order email to specified address
        /// </summary>
        /// <param name="ToID">string ToID</param>
        /// <param name="ONO">int ONO</param>
        /// <param name="RefundAmount">String RefundAmount</param>
        public void SendMail(string ToID, int ONO, string RefundAmount)
        {
            try
            {
                MailMessage Msg = new MailMessage();
                string host = AppLogic.AppConfigs("Host");
                string username = AppLogic.AppConfigs("MailUserName");
                string password = AppLogic.AppConfigs("MailPassword");
                string FromId = AppLogic.AppConfigs("MailFrom");
                string body = "Your order from " + AppLogic.AppConfigs("StoreName") + " with order number " + ONO + " has been refunded. ";

                StringBuilder sw = new StringBuilder();
                String Body = "";
                sw.Append("<html>");
                sw.Append("<head id='Head1' runat='server'>");
                sw.Append("<meta content='text/html; charset=iso-8859-1' http-equiv='Content-Type'/>");
                sw.Append("<title>Welcome to " + AppLogic.AppConfigs("StoreName") + "</title>");
                sw.Append("<link type='text/css' rel='stylesheet' href='../Client/style.css' />    ");
                sw.Append("<style type='text/css'>");
                sw.Append("<!-- " +
                          "body {margin-top: 6px; margin-right: 6px;	margin-bottom: 6px;	margin-left: 6px;	background-color: #ffffff;} " +
                           "img{ border:0px;} " +
                            ".toll_free_font {font-family: Arial, Helvetica, sans-serif;	font-size: 11px;	font-weight: normal;	color: #8c8c8c;	text-decoration: none;} " +
                             ".top_links{ background-color:#131313; font-family:Arial, Helvetica, sans-serif; font-size:12px; font-weight:normal; color:#FFFFFF; word-spacing:20px;} " +
                               ".top_links span{ color:#999999;} " +
                                 ".top_links a{ color:#ffffff; word-spacing:0px; text-decoration:none;} " +
                                 ".top_links a:hover{ background-color:#131313; font-family:Arial, Helvetica, sans-serif; font-size:12px; font-weight:normal; color:#CCCCCC; text-decoration:none;} " +
                                 ".content_font_gray {font-family: Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #606060;	text-decoration: none;} " +
                                 ".content_font_orange {	font-family: Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #ff7e00;	text-decoration: none;} " +
                                 "a.content_font_orange:hover {font-family: Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #333333;	text-decoration: none;} " +
                                   ".userbox_bg {font-family: Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #545656;	text-decoration: none;	background-color: #eeeeee;} " +
                                    ".horizone_line { border-bottom:#999999 solid 1px;} " +
                                     ".footer_font {font-family: Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #999999;	text-decoration: none;	line-height: 18px;} " +
                                       "a.footer_font:hover {font-family: Arial, Helvetica, sans-serif;	font-size: 12px;	font-weight: normal;	color: #333333;	text-decoration: none;	line-height: 18px;} " +
                                         "--> " +
                                         "</style> ");
                sw.Append("</head>");
                sw.Append("<body>");
                sw.Append("<table cellspacing='0' cellpadding='0' align='center' width='598' style='border: 1px solid rgb(102, 102, 102);'>");
                sw.Append("<tr>");
                sw.Append("<td>");
                sw.Append("<table cellspacing='0' cellpadding='0' border='0' align='center' width='98%'>");
                sw.Append("<tr>");

                sw.Append("<td height='50'>");
                sw.Append("<table cellspacing='0' cellpadding='0' border='0' align='center' width='95%'>");
                sw.Append("<tr>");
                sw.Append("<td width='50%'>");
                sw.Append("<a  href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "'>");
                sw.Append("<img  src=\"" + AppLogic.AppConfigs("LIVE_SERVER") + "/Client/images/mail_logo.jpg\"/>");
                sw.Append("</a></td>");
                sw.Append("</tr>");
                sw.Append("</table></td>");
                sw.Append("</tr>");

                sw.Append("<tr>");
                sw.Append("<td><table cellspacing='0' cellpadding='0' border='0' width='100%'>");
                sw.Append("<tbody><tr>");
                sw.Append("<td height='26' align='center' valign='middle' class='top_links'>");
                sw.Append("<a href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/t-AboutUs.aspx'>About Us</a>   |     <a href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/ContactUs.aspx'>Contact Us</a>   |   <a href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "/Login.aspx'>Login</a>");
                sw.Append("</td></tr></tbody></table></td>");
                sw.Append("</tr>");

                sw.Append("<tr>");
                sw.Append("<td><img height='220' width='596' src='" + AppLogic.AppConfigs("LIVE_SERVER") + "/Client/images/mail_banner.jpg'/></td>");
                sw.Append("</tr>");

                sw.Append("<tr>");
                sw.Append("<td valign='top'><img height='16' width='1' src='" + AppLogic.AppConfigs("LIVE_SERVER") + "/Client/images/spacer.gif'/></td>");
                sw.Append("</tr>");

                sw.Append("<tr>");
                sw.Append("<td><table cellspacing='0' cellpadding='0' border='0' align='center' width='95%'>");
                sw.Append("<tr>");

                sw.Append("<td align='left' rowspan='1' valign='middle'>");
                sw.Append("</td>");


                sw.Append("Your order from " + AppLogic.AppConfigs("StoreName") + " with order number " + ONO + " has been refunded. <br/>");
                sw.Append("Refund Amount:" + RefundAmount);

                sw.Append("<tr class='receiptfont'>");
                sw.Append("<td class='receiptfont'>");
                sw.Append("<br>");
                sw.Append(" Thank You,<br>");
                sw.Append("<br>");
                sw.Append("<a class='content_font_orange'  href='" + AppLogic.AppConfigs("LIVE_SERVER").ToString() + "'>" + AppLogic.AppConfigs("LIVE_SERVER_NAME").ToString() + "</a>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("</td></tr></tbody></table></td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td><table cellspacing='0' cellpadding='0' border='0' align='center' width='95%'>");
                sw.Append("<tbody><tr>");
                sw.Append("<td class='horizone_line'> </td>");
                sw.Append("</tr>");
                sw.Append("<tr>");
                sw.Append("<td height='25' align='left' valign='middle'>");
                sw.Append("<span class='footer_font'>" + AppLogic.AppConfigs("FooterRight") + " </span>");
                sw.Append("</td></tr></tbody></table></td>");
                sw.Append("</tr>");

                sw.Append("</table>");
                sw.Append("</td>");
                sw.Append("</tr>");
                sw.Append("</table>");
                sw.Append("</body></html>");

                //string path = Server.MapPath("../images/logo_white_bg.gif");
                //LinkedResource logo = new LinkedResource(path);
                //logo.ContentId = "companylogo";
                AlternateView av1 = AlternateView.CreateAlternateViewFromString(sw.ToString(), null, "text/html");
                string Subject = "Order Was Refunded";
                CommonOperations.SendMail(ToID.ToString().Trim(), Subject.ToString(), sw.ToString(), Request.UserHostAddress.ToString(), true, av1);
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// Refunds the Order by Product ID
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="RefundAmount">decimal RefundAmount</param>
        /// <param name="Ordernumber">int Ordernumber</param>
        public void RefundOrderbyProductID(Int32 ProductID, Decimal RefundAmount, Int32 Ordernumber)
        {
            Decimal refundAmount = Decimal.Zero;
            String RefundReason = "";
            DataSet ds = null;
            ds = new DataSet();
            refundAmount = RefundAmount;
            Int32 OrderNumber = Ordernumber;
            ds = GetOrderForRefundedOrderByProductId(OrderNumber, ProductID);
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                Int32 StoreID = 0;
                Int32.TryParse(ds.Tables[0].Rows[0]["StoreID"].ToString(), out StoreID);
                if (StoreID > 0)
                    AppConfig.StoreID = StoreID;
                string PMName = ds.Tables[0].Rows[0]["PaymentMethod"].ToString();
                string PM = PMName.Replace(" ", String.Empty).ToUpper();

                if (ds.Tables[0].Rows[0]["TransactionState"].ToString() == AppLogic.ro_TXStateCaptured || ds.Tables[0].Rows[0]["TransactionState"].ToString() == AppLogic.ro_TXStateRefunded)
                {
                    Decimal OrderTotal = 0;
                    Decimal.TryParse(ds.Tables[0].Rows[0]["OrderTotal"].ToString(), out OrderTotal);
                    if (refundAmount == Decimal.Zero)
                    {
                        lblMsgRefund.Text = "Invalid refund amount specified.";
                        return;
                    }
                    String Status = AppLogic.ro_OK;

                    if (PM == AppLogic.ro_PMCreditCard || PM == AppLogic.ro_PMPayPal || PM == AppLogic.ro_PMPayPalExpress)
                    {
                        //Status = Gateway.ProcessRefundByOrderANDProductID(Convert.ToInt32(ds.Tables[0].Rows[0]["CustomerID"].ToString()), OrderNumber, 0, refundAmount, RefundReason, null, ProductID);
                        Status = AuthorizeNetComponent.RefundOrder(Convert.ToInt32(OrderNumber.ToString()), Convert.ToDecimal(refundAmount), RefundReason.ToString());
                    }
                    else
                    {
                        Status = ForceRefundStatus(OrderNumber);
                    }
                    if (Status == AppLogic.ro_OK)
                    {
                        try
                        {
                            string strRefundAmount = string.Empty;
                            if (refundAmount != Decimal.Zero)
                                strRefundAmount = refundAmount.ToString("f2");
                            else
                                strRefundAmount = OrderTotal.ToString("f2");
                            CommonComponent.ExecuteCommonData("update op set op.IsRefunded=1,op.RefundAmount = " + refundAmount + " from tb_OrderedShoppingCartItems op join tb_Order o on op.OrderedShoppingCartID=o.ShoppingCartID and o.OrderNumber=" + OrderNumber + " and op.RefProductID=" + ProductID);
                            CommonComponent.ExecuteCommonData("update tb_returnitem set IsReturn=1,ReturnType='RR' where OrderedNumber=" + OrderNumber + " and Productid=" + ProductID);
                            Int32 TotalShoppingCartItems = 0;
                            Int32 TotalRefundedItems = 0;
                            try
                            {
                                TotalShoppingCartItems = Convert.ToInt32("select count(1) as TotalItems from tb_OrderedShoppingCartItems where OrderedShoppingcartid = (select ShoppingCardID from tb_Order where Ordernumber = " + OrderNumber + ")");
                                TotalRefundedItems = Convert.ToInt32("select count(1) as TotalRefundItems from tb_OrderedShoppingCartItems where  isRefunded = 1 and OrderedShoppingcartid = (select ShoppingCardID from tb_Order where Ordernumber = " + OrderNumber + ")");
                            }
                            catch { }

                            if (TotalShoppingCartItems != 0 && TotalRefundedItems != 0 && TotalShoppingCartItems == TotalRefundedItems)
                                CommonComponent.ExecuteCommonData("update tb_Order set TransactionStatus='" + AppLogic.ro_TXStateRefunded + "', RefundedOn=dateadd(hour,-2,getdate()), IsNew=0,RefundedAmount=isnull(RefundedAmount,0)+" + RefundAmount + " where OrderNumber=" + OrderNumber.ToString());
                            else
                                CommonComponent.ExecuteCommonData("update tb_Order set  RefundedOn=dateadd(hour,-2,getdate()), IsNew=0,RefundedAmount=isnull(RefundedAmount,0)+" + RefundAmount + " where OrderNumber=" + OrderNumber.ToString());

                            OrderNumber = Convert.ToInt32(SecurityComponent.Decrypt(Request.QueryString["ONo"].ToString()));
                            RefundCartBind();
                            string popup = "<script language='javascript' ID='script1'>" + "window.open('ShippingLabelRefund.aspx?ONo=" + OrderNumber + "&PID=" + ProductID + "','newwindow', 'top=0,left=50,width=900,height=650,dependant = no, alwaysRaised = no, menubar=no, resizable=no ,scrollbars=yes, toolbar=no,status=no')" + "</script>";
                            System.Web.UI.ScriptManager.RegisterStartupScript((System.Web.UI.Page)System.Web.HttpContext.Current.Handler, typeof(System.Web.UI.Page), "script1", popup, false);
                        }
                        catch (Exception ex)
                        {
                        }
                    }
                    lblMsgRefund.Text = "Refund Status: " + Status + "";

                }
                else
                {
                    lblMsgRefund.Text = "This transaction has not yet been Captured. Use Void if required. The transaction state (" + ds.Tables[0].Rows[0]["TransactionState"].ToString() + ") is not " + AppLogic.ro_TXModeAuthCapture + ".";
                }
            }
            else
            {
                lblMsgRefund.Text = "ORDER NOT FOUND";
            }
        }

        /// <summary>
        /// Get Quantity Discount by the productId and by the quantity
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Qty">int Qty</param>
        /// <returns>Returns the discount value as a decimal</returns>
        public decimal GetQtyDiscount(int ProductID, int Qty)
        {
            string Query = " SELECT tb_QuantityDiscountTable.DiscountPercent FROM   tb_Product Inner Join " +
                            " tb_QuantityDiscountTable ON tb_Product.QuantityDiscountID = tb_QuantityDiscountTable.QuantityDiscountID  " +
                            " WHERE (tb_QuantityDiscountTable.LowQuantity <=" + Qty + ") AND (tb_QuantityDiscountTable.HighQuantity >=" + Qty + ") AND  tb_Product.ProductID =" + ProductID + "";
            DataSet Ds = CommonComponent.GetCommonDataSet(Query);
            if (Ds.Tables[0].Rows.Count > 0)
                return Convert.ToDecimal(Ds.Tables[0].Rows[0]["DiscountPercent"].ToString());
            else
                return 0;
        }

        /// <summary>
        ///  Refund Product Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnRefundProductClick_Click(object sender, EventArgs e)
        {
            Random r = new Random();
            System.Text.StringBuilder sw = new StringBuilder();
            sw.Append("<script language='javascript' type='text/javascript'>");
            sw.Append("window.open('refundorder.aspx?ordernumber=" + SecurityComponent.Decrypt(Request.QueryString["Ono"].ToString()) + "&Refundvalue=" + refundamaut.Value.ToString() + "','RefundOrder" + r.Next(1, 100000).ToString() + "','toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=yes,resizable=yes,copyhistory=no,width=600,height=500,left=0,top=0');");
            sw.Append("</script>");
            Page.ClientScript.RegisterClientScriptBlock(this.GetType(), "script", sw.ToString());
        }
    }
}