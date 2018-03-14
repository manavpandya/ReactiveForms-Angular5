using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;
namespace Solution.Data
{
    /// <summary>
    /// Order Data Access Class Contains Order related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class OrderDAC
    {
        #region Declaration
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        #endregion

        #region Key Functions

        /// <summary>
        /// Get Chart Details For Order Statistic Report
        /// </summary>
        /// <param name="Duration">string Duration</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="StartDate">DateTime StartDate</param>
        /// <param name="EndDate">DateTime EndDate</param>
        /// <returns>Returns the Chart Details for Order Statistics Report as a Dataset</returns>
        public DataSet GetChartDetailsForOrderStatisticReport(String Duration, Int32 StoreID, DateTime StartDate, DateTime EndDate, String Status)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_OrderStatistics";
            cmd.Parameters.AddWithValue("@Duration", Duration);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@Status", Status);

            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get order List By StoreId
        /// </summary>
        /// <param name="PageNumber">int PageNumber</param>
        /// <param name="PageSize">int PageSize</param>
        /// <param name="Search">string Search</param>
        /// <param name="StoreId">int StoreId</param>
        /// <returns>Returns the Order LIst by StoreID</returns>
        public DataSet GetorderListByStoreId(Int32 PageNumber, Int32 PageSize, string Search, Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_GetorderListByStoreId";
            cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
            cmd.Parameters.AddWithValue("@PageSize", PageSize);
            cmd.Parameters.AddWithValue("@Filter", Search);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            return objSql.GetDs(cmd);

        }

        /// <summary>
        /// Get Order List For Multi Capture
        /// </summary>
        /// <param name="strWhereClus">String strWhereClus</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns the Order LIst for Multi Capture as a Dataset</returns>
        public DataSet GetorderListForMultiCapture(String strWhereClus, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_GetOrderListForMultiCapture";
            cmd.Parameters.AddWithValue("@StrWhere", strWhereClus);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Order List By StoreId for Bulk Print
        /// </summary>
        /// <param name="PageNumber">int PageNumber</param>
        /// <param name="PageSize">int PageSize</param>
        /// <param name="Search">string Search</param>
        /// <param name="StoreId">int StoreId</param>
        /// <returns>Returns the Order List for bulk print as a Dataset</returns>
        public DataSet GetorderListByStoreIdforBulkPrint(Int32 PageNumber, Int32 PageSize, string Search, Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_GetOrderListByStoreIDforBulkPrint";
            cmd.Parameters.AddWithValue("@PageNumber", PageNumber);
            cmd.Parameters.AddWithValue("@PageSize", PageSize);
            cmd.Parameters.AddWithValue("@Filter", Search);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            return objSql.GetDs(cmd);

        }



        /// <summary>
        /// Function For Insert OrderComments
        /// </summary>
        /// <param name="objOrderComments">tb_OrderComment objOrderComments</param>
        /// <returns>Returns tb_OrderComment Table Object</returns>
        public tb_OrderComment InsertOrderComments(tb_OrderComment objOrderComments)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_OrderComment(objOrderComments);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objOrderComments;
        }

        /// <summary>
        /// Get All Order Comments by OrderID
        /// </summary>
        /// <param name="OrderID">int OrderID</param>
        /// <returns>Returns the List of Comments for OrderID, passed in parameter</returns>
        public List<tb_OrderComment> GetOrderCommentsByID(int OrderID)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            List<tb_OrderComment> objordercomment = new List<tb_OrderComment>();
            objordercomment = ctx.tb_OrderComment.Where(oc => ((System.Int32?)oc.OrderNumber ?? 0) == OrderID).OrderByDescending(od => od.CommentID).ToList<tb_OrderComment>();
            return objordercomment;
        }

        /// <summary>
        /// Get Product List By Cart Id
        /// </summary>
        /// <param name="CartId">int CartId</param>
        /// <returns>Returns the Product List for Cart by CartID</returns>
        public DataSet GetProductListByCartId(Int32 CartId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_OrderedShoppingCartItems_GetProductList";
            cmd.Parameters.AddWithValue("@CartId", CartId);
            return objSql.GetDs(cmd);

        }

        /// <summary>
        /// Get Invoice Product List
        /// </summary>
        /// <param name="CartId">int CartId</param>
        /// <returns>Returns the Invoice Product List for particular cart</returns>
        public DataSet GetInvoiceProductList(Int32 CartId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Invoice_GetProductDetails";
            cmd.Parameters.AddWithValue("@CartId", CartId);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Get Order Details By Order Number
        /// </summary>
        /// <param name="OrderNumber">int Order Number</param>
        /// <returns></returns>
        public DataSet GetOrderDetailsByOrderNumber(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_GetOrderDetailsByOrderNumber";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);

            return objSql.GetDs(cmd);

        }

        /// <summary>
        /// Update Order Status When Comment Added
        /// </summary>
        /// <param name="OrderNumber">Int OrderNumber</param>
        /// <param name="Orderstatus">String OrderStatus</param>
        /// <param name="OrderNotes">string OrderNotes</param>
        public void UpdateOrderStatus(Int32 OrderNumber, String Orderstatus, String OrderNotes)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_UpdateOrderStatus";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@Orderstatus", Orderstatus);
            cmd.Parameters.AddWithValue("@OrderNotes", OrderNotes);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        ///  Get Payment Gateway Default Selected
        /// </summary>
        /// <param name="PaymentMethod">string paymentMethod</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Payment Gateway List</returns>
        public DataSet GetPaymentGateway(string PaymentMethod, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Payment_GetPaymentGateway";
            cmd.Parameters.AddWithValue("@paymentMethod", PaymentMethod);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Order Details By OrderID
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns a Order Details by OrderID</returns>
        public DataSet GetOrderDetailsByOrderID(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_GetOrderDetailsByOrderID";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        ///  Get Order Details By OrderID
        /// </summary>
        /// <param name="PaymentMethod">Int32 OrderNumber</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Dataset Order data</returns>
        public DataSet GetViewRecentOrderByOrderID(Int32 OrderNumber, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_ViewRecentOrder";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        ///  Get RMA Order Details By OrderID
        /// </summary>
        /// <param name="PaymentMethod">Int32 OrderNumber</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Dataset RMA Order data</returns>
        public DataSet GetViewRecentForRMAOrderByOrderID(Int32 OrderNumber, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_ViewRMARecentOrder";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Add New Order
        /// </summary>
        /// <param name="objOrder">tb_Order objOrder</param>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 AddOrder(tb_Order objOrder, Int32 OrderNumber, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_AddOrder";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@IsNew", objOrder.IsNew);
            cmd.Parameters.AddWithValue("@ShoppingCardID", 0);
            cmd.Parameters.AddWithValue("@CustomerID", objOrder.CustomerID);
            cmd.Parameters.AddWithValue("@FirstName", objOrder.FirstName);
            cmd.Parameters.AddWithValue("@LastName", objOrder.LastName);
            cmd.Parameters.AddWithValue("@Email", objOrder.Email);
            cmd.Parameters.AddWithValue("@Notes", objOrder.Notes);
            cmd.Parameters.AddWithValue("@GiftPackNote", objOrder.GiftPackNote);
            cmd.Parameters.AddWithValue("@BillingEqualsShipping", objOrder.BillingEqualsShipping);
            cmd.Parameters.AddWithValue("@BillingEmail", objOrder.BillingEmail);
            cmd.Parameters.AddWithValue("@BillingFirstName", objOrder.BillingFirstName);
            cmd.Parameters.AddWithValue("@BillingLastName", objOrder.BillingLastName);
            cmd.Parameters.AddWithValue("@BillingCompany", objOrder.BillingCompany);
            cmd.Parameters.AddWithValue("@BillingAddress1", objOrder.BillingAddress1);
            cmd.Parameters.AddWithValue("@BillingAddress2", objOrder.BillingAddress2);
            cmd.Parameters.AddWithValue("@BillingSuite", objOrder.BillingSuite);
            cmd.Parameters.AddWithValue("@BillingCity", objOrder.BillingCity);
            cmd.Parameters.AddWithValue("@BillingState", objOrder.BillingState);
            cmd.Parameters.AddWithValue("@BillingZip", objOrder.BillingZip);
            cmd.Parameters.AddWithValue("@BillingCountry", objOrder.BillingCountry);
            cmd.Parameters.AddWithValue("@BillingPhone", objOrder.BillingPhone);
            cmd.Parameters.AddWithValue("@ShippingEmail", objOrder.ShippingEmail);
            cmd.Parameters.AddWithValue("@ShippingLastName", objOrder.ShippingLastName);
            cmd.Parameters.AddWithValue("@ShippingFirstName", objOrder.ShippingFirstName);

            cmd.Parameters.AddWithValue("@ShippingCompany", objOrder.ShippingCompany);
            cmd.Parameters.AddWithValue("@ShippingAddress1", objOrder.ShippingAddress1);
            cmd.Parameters.AddWithValue("@ShippingAddress2", objOrder.ShippingAddress2);
            cmd.Parameters.AddWithValue("@ShippingSuite", objOrder.ShippingSuite);
            cmd.Parameters.AddWithValue("@ShippingCity", objOrder.ShippingCity);
            cmd.Parameters.AddWithValue("@ShippingState", objOrder.ShippingState);
            cmd.Parameters.AddWithValue("@ShippingZip", objOrder.ShippingZip);
            cmd.Parameters.AddWithValue("@ShippingCountry", objOrder.ShippingCountry);
            cmd.Parameters.AddWithValue("@ShippingPhone", objOrder.ShippingPhone);
            cmd.Parameters.AddWithValue("@ShippingMethod", objOrder.ShippingMethod);
            cmd.Parameters.AddWithValue("@OkToEmail", objOrder.OkToEmail);
            cmd.Parameters.AddWithValue("@CardName", objOrder.CardName);
            cmd.Parameters.AddWithValue("@CardType", objOrder.CardType);
            cmd.Parameters.AddWithValue("@CardNumber", objOrder.CardNumber);
            cmd.Parameters.AddWithValue("@CardVarificationCode", objOrder.CardVarificationCode);
            cmd.Parameters.AddWithValue("@CardExpirationMonth", objOrder.CardExpirationMonth);
            cmd.Parameters.AddWithValue("@CardExpirationYear", objOrder.CardExpirationYear);
            cmd.Parameters.AddWithValue("@CouponCode", objOrder.CouponCode);
            cmd.Parameters.AddWithValue("@CouponDiscountAmount", objOrder.CouponDiscountAmount);
            cmd.Parameters.AddWithValue("@GiftCertiSerialNumber", objOrder.GiftCertiSerialNumber);
            cmd.Parameters.AddWithValue("@GiftCertificateDiscountAmount", objOrder.GiftCertificateDiscountAmount);
            cmd.Parameters.AddWithValue("@QuantityDiscountAmount", objOrder.QuantityDiscountAmount);
            cmd.Parameters.AddWithValue("@LevelDiscountPercent", objOrder.LevelDiscountPercent);
            cmd.Parameters.AddWithValue("@LevelDiscountAmount", objOrder.LevelDiscountAmount);
            cmd.Parameters.AddWithValue("@CustomDiscount", objOrder.CustomDiscount);
            cmd.Parameters.AddWithValue("@OrderSubtotal", objOrder.OrderSubtotal);
            cmd.Parameters.AddWithValue("@OrderTax", objOrder.OrderTax);
            cmd.Parameters.AddWithValue("@OrderShippingCosts", objOrder.OrderShippingCosts);
            cmd.Parameters.AddWithValue("@OrderTotal", objOrder.OrderTotal);
            cmd.Parameters.AddWithValue("@AuthorizationCode", objOrder.AuthorizationCode);
            cmd.Parameters.AddWithValue("@AuthorizationResult", objOrder.AuthorizationResult);
            cmd.Parameters.AddWithValue("@AuthorizationPNREF", objOrder.AuthorizationPNREF);
            cmd.Parameters.AddWithValue("@TransactionCommand", objOrder.TransactionCommand);
            cmd.Parameters.AddWithValue("@LastIPAddress", objOrder.LastIPAddress);
            cmd.Parameters.AddWithValue("@PaymentGateway", objOrder.PaymentGateway);
            cmd.Parameters.AddWithValue("@PaymentMethod", objOrder.PaymentMethod);
            cmd.Parameters.AddWithValue("@ShippingTrackingNumber", objOrder.ShippingTrackingNumber);
            cmd.Parameters.AddWithValue("@ShippedVIA", objOrder.ShippedVIA);
            cmd.Parameters.AddWithValue("@OrderStatus", objOrder.OrderStatus);
            cmd.Parameters.AddWithValue("@TransactionStatus", objOrder.TransactionStatus);
            cmd.Parameters.AddWithValue("@AVSResult", objOrder.AVSResult);
            cmd.Parameters.AddWithValue("@Cvc2Response", objOrder.Cvc2Response);
            cmd.Parameters.AddWithValue("@CaptureTxCommand", objOrder.CaptureTxCommand);
            cmd.Parameters.AddWithValue("@CaptureTXResult", objOrder.CaptureTXResult);
            cmd.Parameters.AddWithValue("@VoidTXCommand", objOrder.VoidTXCommand);
            cmd.Parameters.AddWithValue("@VoidTXResult", objOrder.VoidTXResult);
            cmd.Parameters.AddWithValue("@RefundTXCommand", objOrder.RefundTXCommand);
            cmd.Parameters.AddWithValue("@RefundTXResult", objOrder.RefundTXResult);
            cmd.Parameters.AddWithValue("@RefundReason", objOrder.RefundReason);
            cmd.Parameters.AddWithValue("@CartType", objOrder.CartType);
            cmd.Parameters.AddWithValue("@Last4", objOrder.Last4);
            cmd.Parameters.AddWithValue("@AuthorizedOn", objOrder.AuthorizedOn);
            cmd.Parameters.AddWithValue("@CapturedOn", objOrder.CapturedOn);
            cmd.Parameters.AddWithValue("@RefundedOn", objOrder.RefundedOn);
            cmd.Parameters.AddWithValue("@VoidedOn", objOrder.VoidedOn);
            cmd.Parameters.AddWithValue("@FraudedOn", objOrder.FraudedOn);
            cmd.Parameters.AddWithValue("@ShippedOn", objOrder.ShippedOn);
            cmd.Parameters.AddWithValue("@Deleted", objOrder.Deleted);
            cmd.Parameters.AddWithValue("@ReferralLink", objOrder.ReferralLink);
            cmd.Parameters.AddWithValue("@Referrer", objOrder.Referrer);
            cmd.Parameters.AddWithValue("@RefundedAmount", objOrder.RefundedAmount);
            cmd.Parameters.AddWithValue("@ChargeAmount", objOrder.ChargeAmount);
            cmd.Parameters.AddWithValue("@AuthorizedAmount", objOrder.AuthorizedAmount);
            cmd.Parameters.AddWithValue("@AdjustmentAmount", objOrder.AdjustmentAmount);
            cmd.Parameters.AddWithValue("@AdjustmentCapturedOn", objOrder.AdjustmentCapturedOn);
            cmd.Parameters.AddWithValue("@OrderNotes", objOrder.OrderNotes);
            cmd.Parameters.AddWithValue("@isGiftWrap", objOrder.isGiftWrap);
            cmd.Parameters.AddWithValue("@GiftWrapAmt", objOrder.GiftWrapAmt);
            cmd.Parameters.AddWithValue("@SalesRepName", objOrder.SalesRepName);
            cmd.Parameters.AddWithValue("@THUB_POSTED_TO_ACCOUNTING", objOrder.THUB_POSTED_TO_ACCOUNTING);
            cmd.Parameters.AddWithValue("@THUB_POSTED_DATE", objOrder.THUB_POSTED_DATE);
            cmd.Parameters.AddWithValue("@THUB_ACCOUNTING_REF", objOrder.THUB_ACCOUNTING_REF);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Add Phone Order
        /// </summary>
        /// <param name="objOrder">tb_Order objOrder</param>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 AddPhoneOrder(tb_Order objOrder, Int32 OrderNumber, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_AddPhoneOrder";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@IsNew", objOrder.IsNew);
            cmd.Parameters.AddWithValue("@ShoppingCardID", 0);
            cmd.Parameters.AddWithValue("@CustomerID", objOrder.CustomerID);
            cmd.Parameters.AddWithValue("@FirstName", objOrder.FirstName);
            cmd.Parameters.AddWithValue("@LastName", objOrder.LastName);
            cmd.Parameters.AddWithValue("@Email", objOrder.Email);
            cmd.Parameters.AddWithValue("@Notes", objOrder.Notes);
            cmd.Parameters.AddWithValue("@GiftPackNote", objOrder.GiftPackNote);
            cmd.Parameters.AddWithValue("@BillingEqualsShipping", objOrder.BillingEqualsShipping);
            cmd.Parameters.AddWithValue("@BillingEmail", objOrder.BillingEmail);
            cmd.Parameters.AddWithValue("@BillingFirstName", objOrder.BillingFirstName);
            cmd.Parameters.AddWithValue("@BillingLastName", objOrder.BillingLastName);
            cmd.Parameters.AddWithValue("@BillingCompany", objOrder.BillingCompany);
            cmd.Parameters.AddWithValue("@BillingAddress1", objOrder.BillingAddress1);
            cmd.Parameters.AddWithValue("@BillingAddress2", objOrder.BillingAddress2);
            cmd.Parameters.AddWithValue("@BillingSuite", objOrder.BillingSuite);
            cmd.Parameters.AddWithValue("@BillingCity", objOrder.BillingCity);
            cmd.Parameters.AddWithValue("@BillingState", objOrder.BillingState);
            cmd.Parameters.AddWithValue("@BillingZip", objOrder.BillingZip);
            cmd.Parameters.AddWithValue("@BillingCountry", objOrder.BillingCountry);
            cmd.Parameters.AddWithValue("@BillingPhone", objOrder.BillingPhone);
            cmd.Parameters.AddWithValue("@ShippingEmail", objOrder.ShippingEmail);
            cmd.Parameters.AddWithValue("@ShippingLastName", objOrder.ShippingLastName);
            cmd.Parameters.AddWithValue("@ShippingFirstName", objOrder.ShippingFirstName);

            cmd.Parameters.AddWithValue("@ShippingCompany", objOrder.ShippingCompany);
            cmd.Parameters.AddWithValue("@ShippingAddress1", objOrder.ShippingAddress1);
            cmd.Parameters.AddWithValue("@ShippingAddress2", objOrder.ShippingAddress2);
            cmd.Parameters.AddWithValue("@ShippingSuite", objOrder.ShippingSuite);
            cmd.Parameters.AddWithValue("@ShippingCity", objOrder.ShippingCity);
            cmd.Parameters.AddWithValue("@ShippingState", objOrder.ShippingState);
            cmd.Parameters.AddWithValue("@ShippingZip", objOrder.ShippingZip);
            cmd.Parameters.AddWithValue("@ShippingCountry", objOrder.ShippingCountry);
            cmd.Parameters.AddWithValue("@ShippingPhone", objOrder.ShippingPhone);
            cmd.Parameters.AddWithValue("@ShippingMethod", objOrder.ShippingMethod);
            cmd.Parameters.AddWithValue("@OkToEmail", objOrder.OkToEmail);
            cmd.Parameters.AddWithValue("@CardName", objOrder.CardName);
            cmd.Parameters.AddWithValue("@CardType", objOrder.CardType);
            cmd.Parameters.AddWithValue("@CardNumber", objOrder.CardNumber);
            cmd.Parameters.AddWithValue("@CardVarificationCode", objOrder.CardVarificationCode);
            cmd.Parameters.AddWithValue("@CardExpirationMonth", objOrder.CardExpirationMonth);
            cmd.Parameters.AddWithValue("@CardExpirationYear", objOrder.CardExpirationYear);
            cmd.Parameters.AddWithValue("@CouponCode", objOrder.CouponCode);
            cmd.Parameters.AddWithValue("@CouponDiscountAmount", objOrder.CouponDiscountAmount);
            cmd.Parameters.AddWithValue("@GiftCertiSerialNumber", objOrder.GiftCertiSerialNumber);
            cmd.Parameters.AddWithValue("@GiftCertificateDiscountAmount", objOrder.GiftCertificateDiscountAmount);
            cmd.Parameters.AddWithValue("@QuantityDiscountAmount", objOrder.QuantityDiscountAmount);
            cmd.Parameters.AddWithValue("@LevelDiscountPercent", objOrder.LevelDiscountPercent);
            cmd.Parameters.AddWithValue("@LevelDiscountAmount", objOrder.LevelDiscountAmount);
            cmd.Parameters.AddWithValue("@CustomDiscount", objOrder.CustomDiscount);
            cmd.Parameters.AddWithValue("@OrderSubtotal", objOrder.OrderSubtotal);
            cmd.Parameters.AddWithValue("@OrderTax", objOrder.OrderTax);
            cmd.Parameters.AddWithValue("@OrderShippingCosts", objOrder.OrderShippingCosts);
            cmd.Parameters.AddWithValue("@OrderTotal", objOrder.OrderTotal);
            cmd.Parameters.AddWithValue("@AuthorizationCode", objOrder.AuthorizationCode);
            cmd.Parameters.AddWithValue("@AuthorizationResult", objOrder.AuthorizationResult);
            cmd.Parameters.AddWithValue("@AuthorizationPNREF", objOrder.AuthorizationPNREF);
            cmd.Parameters.AddWithValue("@TransactionCommand", objOrder.TransactionCommand);
            cmd.Parameters.AddWithValue("@LastIPAddress", objOrder.LastIPAddress);
            cmd.Parameters.AddWithValue("@PaymentGateway", objOrder.PaymentGateway);
            cmd.Parameters.AddWithValue("@PaymentMethod", objOrder.PaymentMethod);
            cmd.Parameters.AddWithValue("@ShippingTrackingNumber", objOrder.ShippingTrackingNumber);
            cmd.Parameters.AddWithValue("@ShippedVIA", objOrder.ShippedVIA);
            cmd.Parameters.AddWithValue("@OrderStatus", objOrder.OrderStatus);
            cmd.Parameters.AddWithValue("@TransactionStatus", objOrder.TransactionStatus);
            cmd.Parameters.AddWithValue("@AVSResult", objOrder.AVSResult);
            cmd.Parameters.AddWithValue("@Cvc2Response", objOrder.Cvc2Response);
            cmd.Parameters.AddWithValue("@CaptureTxCommand", objOrder.CaptureTxCommand);
            cmd.Parameters.AddWithValue("@CaptureTXResult", objOrder.CaptureTXResult);
            cmd.Parameters.AddWithValue("@VoidTXCommand", objOrder.VoidTXCommand);
            cmd.Parameters.AddWithValue("@VoidTXResult", objOrder.VoidTXResult);
            cmd.Parameters.AddWithValue("@RefundTXCommand", objOrder.RefundTXCommand);
            cmd.Parameters.AddWithValue("@RefundTXResult", objOrder.RefundTXResult);
            cmd.Parameters.AddWithValue("@RefundReason", objOrder.RefundReason);
            cmd.Parameters.AddWithValue("@CartType", objOrder.CartType);
            cmd.Parameters.AddWithValue("@Last4", objOrder.Last4);
            cmd.Parameters.AddWithValue("@AuthorizedOn", objOrder.AuthorizedOn);
            cmd.Parameters.AddWithValue("@CapturedOn", objOrder.CapturedOn);
            cmd.Parameters.AddWithValue("@RefundedOn", objOrder.RefundedOn);
            cmd.Parameters.AddWithValue("@VoidedOn", objOrder.VoidedOn);
            cmd.Parameters.AddWithValue("@FraudedOn", objOrder.FraudedOn);
            cmd.Parameters.AddWithValue("@ShippedOn", objOrder.ShippedOn);
            cmd.Parameters.AddWithValue("@Deleted", objOrder.Deleted);
            cmd.Parameters.AddWithValue("@ReferralLink", objOrder.ReferralLink);
            cmd.Parameters.AddWithValue("@Referrer", objOrder.Referrer);
            cmd.Parameters.AddWithValue("@RefundedAmount", objOrder.RefundedAmount);
            cmd.Parameters.AddWithValue("@ChargeAmount", objOrder.ChargeAmount);
            cmd.Parameters.AddWithValue("@AuthorizedAmount", objOrder.AuthorizedAmount);
            cmd.Parameters.AddWithValue("@AdjustmentAmount", objOrder.AdjustmentAmount);
            cmd.Parameters.AddWithValue("@AdjustmentCapturedOn", objOrder.AdjustmentCapturedOn);
            cmd.Parameters.AddWithValue("@OrderNotes", objOrder.OrderNotes);
            cmd.Parameters.AddWithValue("@isGiftWrap", objOrder.isGiftWrap);
            cmd.Parameters.AddWithValue("@GiftWrapAmt", objOrder.GiftWrapAmt);
            cmd.Parameters.AddWithValue("@SalesRepName", objOrder.SalesRepName);
            cmd.Parameters.AddWithValue("@THUB_POSTED_TO_ACCOUNTING", objOrder.THUB_POSTED_TO_ACCOUNTING);
            cmd.Parameters.AddWithValue("@THUB_POSTED_DATE", objOrder.THUB_POSTED_DATE);
            cmd.Parameters.AddWithValue("@THUB_ACCOUNTING_REF", objOrder.THUB_ACCOUNTING_REF);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@Storecreaditno", objOrder.Storecreaditno);
            cmd.Parameters.AddWithValue("@StorecreaditAmont", objOrder.StorecreaditAmont);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Add Customer When Phone Order
        /// </summary>
        ///<param name="objAddress">tb_Order objAddress</param>
        ///<param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 PhoneOrderAddCustomer(tb_Order objAddress, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_PhoneOrder_AddCustomer";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@FirstName", objAddress.FirstName);
            cmd.Parameters.AddWithValue("@LastName", objAddress.LastName);
            cmd.Parameters.AddWithValue("@Email", objAddress.Email);
            cmd.Parameters.AddWithValue("@BillingEmail", objAddress.BillingEmail);
            cmd.Parameters.AddWithValue("@BillingFirstName", objAddress.BillingFirstName);
            cmd.Parameters.AddWithValue("@BillingLastName", objAddress.BillingLastName);
            cmd.Parameters.AddWithValue("@BillingCompany", objAddress.BillingCompany);
            cmd.Parameters.AddWithValue("@BillingAddress1", objAddress.BillingAddress1);
            cmd.Parameters.AddWithValue("@BillingAddress2", objAddress.BillingAddress2);
            cmd.Parameters.AddWithValue("@BillingSuite", objAddress.BillingSuite);
            cmd.Parameters.AddWithValue("@BillingCity", objAddress.BillingCity);
            cmd.Parameters.AddWithValue("@BillingState", objAddress.BillingState);
            cmd.Parameters.AddWithValue("@BillingZip", objAddress.BillingZip);
            cmd.Parameters.AddWithValue("@BillingCountry", objAddress.BillingCountry);
            cmd.Parameters.AddWithValue("@BillingPhone", objAddress.BillingPhone);
            cmd.Parameters.AddWithValue("@ShippingEmail", objAddress.ShippingEmail);
            cmd.Parameters.AddWithValue("@ShippingLastName", objAddress.ShippingLastName);
            cmd.Parameters.AddWithValue("@ShippingFirstName", objAddress.ShippingFirstName);
            cmd.Parameters.AddWithValue("@ShippingCompany", objAddress.ShippingCompany);
            cmd.Parameters.AddWithValue("@ShippingAddress1", objAddress.ShippingAddress1);
            cmd.Parameters.AddWithValue("@ShippingAddress2", objAddress.ShippingAddress2);
            cmd.Parameters.AddWithValue("@ShippingSuite", objAddress.ShippingSuite);
            cmd.Parameters.AddWithValue("@ShippingCity", objAddress.ShippingCity);
            cmd.Parameters.AddWithValue("@ShippingState", objAddress.ShippingState);
            cmd.Parameters.AddWithValue("@ShippingZip", objAddress.ShippingZip);
            cmd.Parameters.AddWithValue("@ShippingCountry", objAddress.ShippingCountry);
            cmd.Parameters.AddWithValue("@ShippingPhone", objAddress.ShippingPhone);
            cmd.Parameters.AddWithValue("@LastIPAddress", objAddress.LastIPAddress);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Add Order Failed Transaction 
        /// </summary>
        /// <param name="objFailedTransaction">tb_FailedTransaction objFailedTransaction</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 AddOrderFailedTransaction(tb_FailedTransaction objFailedTransaction, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_FailedTransaction";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@CustomerID", objFailedTransaction.CustomerID);
            cmd.Parameters.AddWithValue("@OrderNumber", objFailedTransaction.OrderNumber);
            cmd.Parameters.AddWithValue("@OrderDate", objFailedTransaction.OrderDate);
            cmd.Parameters.AddWithValue("@PaymentGateway", objFailedTransaction.PaymentGateway);
            cmd.Parameters.AddWithValue("@Paymentmethod", objFailedTransaction.Paymentmethod);
            cmd.Parameters.AddWithValue("@TransactionCommand", objFailedTransaction.TransactionCommand);
            cmd.Parameters.AddWithValue("@TransactionResult", objFailedTransaction.TransactionResult);
            cmd.Parameters.AddWithValue("@IPAddress", objFailedTransaction.IPAddress);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get Invoice Products for Update Order
        /// </summary>
        /// <param name="ShoppingCartID">Int32 ShoppingCartID</param>
        /// <returns>Returns the Invoice Products By Shopping Cart ID</returns>
        public DataSet GetInvoiceProducts(Int32 ShoppingCartID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_GetInvoiceProducts";
            cmd.Parameters.AddWithValue("@ShoppingCartID", ShoppingCartID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Next/Previous Order
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>Returns a order according to next or previous</returns>
        public DataSet GetnextpreviousOrder(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_GetnextpreviousOrder";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            return objSql.GetDs(cmd);
        }

        #endregion

        #region For View Recent Order
        /// <summary>
        /// Get Order Number by CustomerID
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Order Number By Customer ID</returns>
        public DataSet GetOrderNobyCustomerID(Int32 CustomerID, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Cart Item For View Recent Order
        /// </summary>
        /// <param name="OrderedShoppingCartID">int OrderedShoppingCartID</param>
        /// <param name="OrderNo">int OrderNo</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Cart Items List for view Recent order</returns>
        public DataSet GetCartItemForViewRecentOrder(Int32 OrderedShoppingCartID, Int32 OrderNo, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@ShoppingCardID", OrderedShoppingCartID);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNo);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get View Order Paging
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Paging Details for Order</returns>
        public DataSet GetViewOrderPaging(Int32 CustomerID, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }
        #endregion

        #region For View old Order

        /// <summary>
        /// Get View Old Order Number by CustomerID
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Old orders number for listing by customer id</returns>
        public DataSet GetViewOldOrderNobyCustomerID(Int32 CustomerID, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 4);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Cart Items For View Old Order
        /// </summary>
        /// <param name="OrderedShoppingCartID">int OrderedShoppingCartID</param>
        /// <param name="OrderNo">int OrderNo</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Cart Items list for old order by Order number</returns>
        public DataSet GetCartItemForViewOldOrder(Int32 OrderedShoppingCartID, Int32 OrderNo, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@ShoppingCardID", OrderedShoppingCartID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNo);
            cmd.Parameters.AddWithValue("@Mode", 5);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Partial shipping in view Old Order
        /// </summary>
        /// <param name="OrderedShoppingCartID">int OrderedShoppingCartID</param>
        /// <param name="OrderNo">int OrderNo</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Partial shipping Details for view Old Order</returns>
        public DataSet GetParsalShippinginviewOldOrder(Int32 OrderedShoppingCartID, Int32 OrderNo, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@ShoppingCardID", OrderedShoppingCartID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNo);
            cmd.Parameters.AddWithValue("@Mode", 6);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get CartID By CustomerID
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Cart ID of particular CustomerID </returns>
        public DataSet GetCartIDByCustID(Int32 CustomerID, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 7);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Only List of Order Numbers by CustomerID
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns only list of Order Numbers By CustomerID</returns>
        public DataSet GetOnlyOrderNobyCustomerID(Int32 CustomerID, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Order By Product Details
        /// </summary>
        /// <param name="SearchKeyword">string SearchKeyword</param>
        /// <returns>Returns the Order Details by particular product Details</returns>
        public string GetOrderByProductDetails(string SearchKeyword)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_OrderedShoppingCartItems_GetProductByProductDetails";
            cmd.Parameters.AddWithValue("@SearchKeayword", SearchKeyword);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Sale Report Record
        /// </summary>
        /// <param name="Param">String Parameter</param>
        /// <param name="StartDate">DateTime Start date</param>
        /// <param name="EndDate">DateTime EndDate</param>
        /// <param name="StoreID">int32 StoreID</param>
        /// <returns>Returns the Sale list Records for Report</returns>
        public DataSet GetSaleReportRecord(string Param, DateTime StartDate, DateTime EndDate, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_SaleReport";
            cmd.Parameters.AddWithValue("@param", Param);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }
        #endregion

        /// <summary>
        /// Get CartID By CustomerID for PhoneOrders
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns CartID by CustomerID for Phone Order</returns>
        public DataSet GetCartIDByCustIDPhoneOrders(Int32 CustomerID, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ViewOrderForMyAccount";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 7);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Gets the details of particular failed transaction
        /// </summary>
        /// <param name="TransactionID">Int32 TransactionID</param>
        /// <returns>Returns tb_FailedTransaction Table Object</returns>
        public tb_FailedTransaction GetFailedTransaction(Int32 TransactionID)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_FailedTransaction transaction = null;
            {
                transaction = ctx.tb_FailedTransaction.First(e => e.TransactionID == TransactionID);
            }
            return transaction;
        }

        /// <summary>
        /// Deletes the particular Failed Transaction
        /// </summary>
        /// <param name="TransactionID">Int32 TransactionID</param>
        public void DeleteFailedTransaction(Int32 TransactionID)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_FailedTransaction transaction = ctx.tb_FailedTransaction.First(c => c.TransactionID == TransactionID);
            ctx.DeleteObject(transaction);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Inserts the OrderLog
        /// </summary>
        /// <param name="StatusID">Int StatusID</param>
        /// <param name="OrderNumber">Int OrderNumber</param>
        /// <param name="Description">String Description</param>
        /// <param name="LogBy">Int LogBy</param>
        public void InsertOrderlog(int StatusID, int OrderNumber, string Description, int LogBy)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_OrderLog";
            cmd.Parameters.AddWithValue("@StatusID", StatusID);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@LogBy", LogBy);
            cmd.Parameters.AddWithValue("@mode", 1);
            objSql.ExecuteNonQuery(cmd);
        }



        /// <summary>
        /// Update Inventory By OrderNumber
        /// </summary>
        /// <param name="StatusID">Int OrderNumber</param>
        /// <param name="OrderNumber">Int direction</param>
        public void UpdateInventoryByOrderNumber(int OrderNumber, int direction)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Inventory_UpdateInventoryByOrderNumber";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@direction", direction);
            objSql.ExecuteNonQuery(cmd);
        }




        /// <summary>
        /// Get the OrderLog for Order Number
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        /// <returns>Returns Order Logs List</returns>
        public DataSet GetOrderLog(string OrderNumber)
        {
            objSql = new SQLAccess();
            DataSet dsorderlog = new DataSet();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_OrderLog";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@mode", 2);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Gets IPAddress Detail
        /// </summary>
        /// <param name="IPaddress">string IPaddress</param>
        /// <returns>Returns IP Address Details</returns>
        public Int32 GetIPAddressDetail(string IPaddress)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = new RedTag_CCTV_Ecomm_DBEntities();
            tb_RestrictedIP tb_restricted = new tb_RestrictedIP();
            Int32 IsRestricted = (from a in ctx.tb_RestrictedIP
                                  where a.IPAddress == IPaddress
                                  select new { a.RestrictedIPID }).Count();
            return IsRestricted;
        }

        /// <summary>
        /// Deletes IPAddress Detail
        /// </summary>
        /// <param name="IPaddress">string IPaddress</param>
        public void DeleteIPAddressDetail(string IPaddress)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = new RedTag_CCTV_Ecomm_DBEntities();
            tb_RestrictedIP tb_restricted = ctx.tb_RestrictedIP.First(c => c.IPAddress == IPaddress);
            ctx.DeleteObject(tb_restricted);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Inserts IP Address Detail
        /// </summary>
        /// <param name="tb_restricted">tb_RestrictedIP tb_restricted</param>
        public void InsertIPAddressDetail(tb_RestrictedIP tb_restricted)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = new RedTag_CCTV_Ecomm_DBEntities();
            ctx.AddTotb_RestrictedIP(tb_restricted);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Import Order Cart for Change Orders
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns the Order Cart Details for Change order</returns>
        public int ImportOrderCartforChangeOrders(int OrderNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_ChangeOrderedShoppingCart]";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@Mode", 1);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Insert Cart Item Change Order
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Quantity">int Quantity</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNames">string VariantNames</param>
        /// <param name="VariantValues">string VariantValues</param>
        /// <returns>Returns True if Inserted</returns>
        public bool InsertCartItemChangeOrder(int OrderNumber, int ProductID, int Quantity, Decimal Price, string VariantNames, string VariantValues, decimal DicPrice)
        {
            objSql = new SQLAccess();

            string Query = "Insert into [tb_ChangeOrderShoppingCartItems](OrderNumber,ProductID,Quantity,Price,VariantNames,Variantvalues,InventoryWasReduced,ProductName,OrderedShoppingCartID,Avail,DiscountPrice) " +
            " select @OrderNumber,@ProductID,@Quantity,@Price,isnull(@VariantNames,''),isnull(@VariantValues,''),1,Name,(Select OrderedShoppingCartID From tb_OrderedShoppingCart where OrderedShoppingCartID " +
                    " in (select ShoppingCardID from tb_Order where OrderNumber=@OrderNumber)),Avail,@DicPrice from tb_Product where ProductID=@ProductID ";

            Query += " Declare @AssproductId int " +
            " DECLARE @AssQty int" +
            " DECLARE @ProductName varchar(500) " +
            " SET @AssproductId = 0" +
            " SET @AssQty = 0" +
            " DECLARE @AssPrice money" +
            " SET @AssPrice=0" +
            " DECLARE TableCursor CURSOR FOR" +
            " SELECT ProductID,Quantity FROM tb_ProductAssembly WHERE RefProductID = @ProductID " +
            " OPEN TableCursor" +
            " FETCH NEXT FROM TableCursor INTO @AssproductId,@AssQty " +
            " WHILE @@FETCH_STATUS = 0" +
            " BEGIN" +
            " SELECT @AssPrice=(case when isnull(SalePrice,0) > 0 then SalePrice else Price end),@ProductName=Name FROM tb_product WHERE  ProductID=@AssproductId  " +
            " INSERT INTO  tb_ChangeOrderShoppingCartItems(OrderNumber, ProductID, Price, Quantity,CategoryID,VariantNames,VariantValues,RelatedproductID,InventoryWasReduced,ProductName,OrderedShoppingCartID)  " +
            " VALUES(@OrderNumber,@AssproductId,@AssPrice,(@Quantity * @AssQty),0,'','',@ProductID,1,@ProductName,(Select OrderedShoppingCartID From tb_OrderedShoppingCart where OrderedShoppingCartID in (select ShoppingCardID from tb_Order where OrderNumber=@OrderNumber)))   " +

            " FETCH NEXT FROM TableCursor INTO @AssproductId,@AssQty " +
            " END" +
            " CLOSE TableCursor" +
            " DEALLOCATE TableCursor";

            SqlCommand cmd = new SqlCommand(Query);
            cmd.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = OrderNumber;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Quantity;
            cmd.Parameters.Add("@Price", SqlDbType.Money).Value = Price;
            cmd.Parameters.Add("@DicPrice", SqlDbType.Money).Value = DicPrice;
            cmd.Parameters.Add("@VariantNames", SqlDbType.NVarChar).Value = VariantNames;
            cmd.Parameters.Add("@VariantValues", SqlDbType.NVarChar).Value = VariantValues;
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Update Cart Item Change Order
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Quantity">int Quantity</param>
        /// <param name="VariantNames">string VariantNames</param>
        /// <param name="VariantValues">string VariantValues</param>
        /// <returns>Returns True if Updated</returns>
        public bool UpdateCartItemChangeOrder(int OrderNumber, int ProductID, int Quantity, string VariantNames, string VariantValues)
        {
            objSql = new SQLAccess();
            string Query = "Update [tb_ChangeOrderShoppingCartItems] set Quantity=@Quantity where ProductID=@ProductID"
                + " and (isnull(VariantNames,'')=@VariantNames and isnull(VariantValues,'')=@VariantValues)  " +
                " and OrderNumber=@OrderNumber";

            Query += " DECLARE @AssproductId1 int " +
                    " DECLARE @AssQty1 int " +
                    " SET @AssproductId1 = 0 " +
                    " SET @AssQty1 = 0 " +
                    " DECLARE TableCursor CURSOR FOR " +
                    " SELECT ProductID,Quantity FROM tb_ProductAssembly WHERE RefProductID =@ProductID " +
                    " OPEN TableCursor " +
                    " FETCH NEXT FROM TableCursor INTO @AssproductId1,@AssQty1 " +
                    " WHILE @@FETCH_STATUS = 0 " +
                    " BEGIN " +
                    " UPDATE tb_ChangeOrderShoppingCartItems SET Quantity =(@Quantity * @AssQty1) WHERE RelatedproductID =@ProductID AND ProductID=@AssproductId1 AND OrderNumber=@OrderNumber " +
                    " FETCH NEXT FROM TableCursor INTO @AssproductId1,@AssQty1 " +
                    " END " +
                    " CLOSE TableCursor " +
                    " DEALLOCATE TableCursor";

            SqlCommand cmd = new SqlCommand(Query);
            cmd.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = OrderNumber;
            cmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = ProductID;
            cmd.Parameters.Add("@Quantity", SqlDbType.Int).Value = Quantity;
            cmd.Parameters.Add("@VariantNames", SqlDbType.NVarChar).Value = VariantNames;
            cmd.Parameters.Add("@VariantValues", SqlDbType.NVarChar).Value = VariantValues;
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Get Order By Order Number
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns the Order Details by Order Number</returns>
        public tb_Order GetOrderByOrderNumber(int OrderNumber)
        {
            tb_Order objOrder = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                objOrder = ctx.tb_Order.FirstOrDefault(e => e.OrderNumber == OrderNumber);
            }
            return objOrder;
        }

        /// <summary>
        /// Update Order Details
        /// </summary>
        /// <param name="order">tb_Order order</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public int UpdateOrder(tb_Order order)
        {
            int RowsAffected = 0;
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                RowsAffected = ctx.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            return RowsAffected;
        }


        /// <summary>
        /// Insert Vendor Purchase Order Details
        /// </summary>
        /// <param name="vendorid">int vendorid</param>
        /// <param name="podate">datetime podate</param>
        /// <returns>Returns True if Inserted</returns>
        public bool InsertVendorPrurchaseOrderDetails(int vendorid, DateTime podate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_PurchaseOrderItemReport";
            cmd.Parameters.AddWithValue("@vendorid", vendorid);
            cmd.Parameters.AddWithValue("@podate", podate);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));



        }

        /// <summary>
        /// Insert Vendor Purchase Order Item Details
        /// </summary>
        /// <param name="vendorid">int vendorid</param>
        /// <param name="productid">int productid</param>
        /// <param name="quantity">int quantity</param>
        /// <param name="price">double price</param>
        /// <param name="podate">datetime podate</param>
        /// <returns>Returns True if Inserted</returns>
        public bool InsertVendorPrurchaseOrderItemDetails(int vendorid, int productid, int quantity, double price, DateTime podate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_PurchaseOrderItemReport";
            cmd.Parameters.AddWithValue("@vendorid", vendorid);
            cmd.Parameters.AddWithValue("@productid", productid);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@podate", podate);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));

        }

        /// <summary>
        /// Insert Purchase Order Item Details
        /// </summary>
        /// <param name="vendorid">int vendorid</param>
        /// <param name="productid">int productid</param>
        /// <param name="quantity">int quantity</param>
        /// <param name="price">double price</param>
        /// <param name="podate">datetime podate</param>
        /// <param name="VendorQuoteRequestID">int VendorQuoteRequestID</param>
        /// <param name="VendorQuoteReqDetailsID">int VendorQuoteReqDetailsID</param>
        /// <returns>Returns True if Inserted</returns>
        public bool InsertVendorPrurchaseOrderItemDetailsForVendorQuote(int vendorid, int productid, int quantity, double price, DateTime podate, Int32 VendorQuoteRequestID, Int32 VendorQuoteReqDetailsID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_PurchaseOrderItemReport";
            cmd.Parameters.AddWithValue("@vendorid", vendorid);
            cmd.Parameters.AddWithValue("@productid", productid);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@podate", podate);
            cmd.Parameters.AddWithValue("@VendorQuoteRequestID", VendorQuoteRequestID);
            cmd.Parameters.AddWithValue("@VendorQuoteReqDetailsID", VendorQuoteReqDetailsID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Select Vendor Purchase Order Detail
        /// </summary>
        /// <param name="vendorid">int vendorid</param>
        /// <param name="podate">int podate</param>
        /// <returns>Returns the List of Vendor purchase order details</returns>
        public DataSet SelectVendorPurchaseOrderDeatial(int vendorid, DateTime podate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_PurchaseOrderItemReport";
            cmd.Parameters.AddWithValue("@vendorid", vendorid);
            cmd.Parameters.AddWithValue("@podate", podate);
            cmd.Parameters.AddWithValue("@Mode", 2);

            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Select Purchase Order Details Vendor
        /// </summary>
        /// <param name="vendorid">int vendorid</param>
        /// <param name="podate">datetime podate</param>
        /// <returns>Returns the List of Vendor purchase order details</returns>
        public DataSet SelectPurchaserOrderDetailsVendor(int vendorid, System.DateTime podate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_PurchaseOrderItemReport";
            cmd.Parameters.AddWithValue("@vendorid", vendorid);
            cmd.Parameters.AddWithValue("@podate", podate);
            cmd.Parameters.AddWithValue("@Mode", 4);

            return objSql.GetDs(cmd);

        }

        /// <summary>
        /// Get order detail for particular Order
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns the Order Details by Order Number</returns>
        public DataSet GetDetailforOrder(string OrderNumber)
        {
            objSql = new SQLAccess();
            DataSet dsorderlog = new DataSet();
            cmd = new SqlCommand();
            cmd.CommandText = "select * from tb_order where OrderNumber=" + OrderNumber;
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Shipping Detail for Cart Number
        /// </summary>
        /// <param name="CartNumber">string CartNumber</param>
        /// <returns>Returns the Shipping Details from Cart Number</returns>
        public DataSet GetShippingDetailforCartNumber(string CartNumber)
        {
            objSql = new SQLAccess();
            DataSet dsorderlog = new DataSet();
            cmd = new SqlCommand();
            cmd.CommandText = "select distinct TrackingNumber,ShippedVia,PackId from tb_OrderedShoppingCartItems where OrderedShoppingCartID=" + CartNumber;
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Print Status for Order
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        /// <returns>Returns the True if Updated</returns>
        public bool UpdatePrintStatusforOrder(String OrderNumber)
        {
            objSql = new SQLAccess();
            string Query = "update tb_Order set IsPrinted=1 where OrderNumber in (" + OrderNumber + ")";
            SqlCommand cmd = new SqlCommand(Query);
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Refund GiftCard Amount By order number
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns Message According to Result</returns>
        public String RefundGiftCardAmount(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_RefundGiftCardAmount";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.NVarChar);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToString(paramReturnval.Value);
        }

        /// <summary>
        /// Add Yahoo Order
        /// </summary>
        /// <param name="objOrder">tb_Order objOrder</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 YahooAddOrder(tb_Order objOrder, Int32 OrderNumber, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_YahooAddOrder";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@IsNew", objOrder.IsNew);
            cmd.Parameters.AddWithValue("@ShoppingCardID", objOrder.ShoppingCardID);
            cmd.Parameters.AddWithValue("@CustomerID", objOrder.CustomerID);
            cmd.Parameters.AddWithValue("@FirstName", objOrder.FirstName);
            cmd.Parameters.AddWithValue("@LastName", objOrder.LastName);
            cmd.Parameters.AddWithValue("@Email", objOrder.Email);
            cmd.Parameters.AddWithValue("@Notes", objOrder.Notes);
            cmd.Parameters.AddWithValue("@GiftPackNote", objOrder.GiftPackNote);
            cmd.Parameters.AddWithValue("@BillingEqualsShipping", objOrder.BillingEqualsShipping);
            cmd.Parameters.AddWithValue("@BillingEmail", objOrder.BillingEmail);
            cmd.Parameters.AddWithValue("@BillingFirstName", objOrder.BillingFirstName);
            cmd.Parameters.AddWithValue("@BillingLastName", objOrder.BillingLastName);
            cmd.Parameters.AddWithValue("@BillingCompany", objOrder.BillingCompany);
            cmd.Parameters.AddWithValue("@BillingAddress1", objOrder.BillingAddress1);
            cmd.Parameters.AddWithValue("@BillingAddress2", objOrder.BillingAddress2);
            cmd.Parameters.AddWithValue("@BillingSuite", objOrder.BillingSuite);
            cmd.Parameters.AddWithValue("@BillingCity", objOrder.BillingCity);
            cmd.Parameters.AddWithValue("@BillingState", objOrder.BillingState);
            cmd.Parameters.AddWithValue("@BillingZip", objOrder.BillingZip);
            cmd.Parameters.AddWithValue("@BillingCountry", objOrder.BillingCountry);
            cmd.Parameters.AddWithValue("@BillingPhone", objOrder.BillingPhone);
            cmd.Parameters.AddWithValue("@ShippingEmail", objOrder.ShippingEmail);
            cmd.Parameters.AddWithValue("@ShippingLastName", objOrder.ShippingLastName);
            cmd.Parameters.AddWithValue("@ShippingFirstName", objOrder.ShippingFirstName);

            cmd.Parameters.AddWithValue("@ShippingCompany", objOrder.ShippingCompany);
            cmd.Parameters.AddWithValue("@ShippingAddress1", objOrder.ShippingAddress1);
            cmd.Parameters.AddWithValue("@ShippingAddress2", objOrder.ShippingAddress2);
            cmd.Parameters.AddWithValue("@ShippingSuite", objOrder.ShippingSuite);
            cmd.Parameters.AddWithValue("@ShippingCity", objOrder.ShippingCity);
            cmd.Parameters.AddWithValue("@ShippingState", objOrder.ShippingState);
            cmd.Parameters.AddWithValue("@ShippingZip", objOrder.ShippingZip);
            cmd.Parameters.AddWithValue("@ShippingCountry", objOrder.ShippingCountry);
            cmd.Parameters.AddWithValue("@ShippingPhone", objOrder.ShippingPhone);
            cmd.Parameters.AddWithValue("@ShippingMethod", objOrder.ShippingMethod);
            cmd.Parameters.AddWithValue("@OkToEmail", objOrder.OkToEmail);
            cmd.Parameters.AddWithValue("@CardName", objOrder.CardName);
            cmd.Parameters.AddWithValue("@CardType", objOrder.CardType);
            cmd.Parameters.AddWithValue("@CardNumber", objOrder.CardNumber);
            cmd.Parameters.AddWithValue("@CardVarificationCode", objOrder.CardVarificationCode);
            cmd.Parameters.AddWithValue("@CardExpirationMonth", objOrder.CardExpirationMonth);
            cmd.Parameters.AddWithValue("@CardExpirationYear", objOrder.CardExpirationYear);
            cmd.Parameters.AddWithValue("@CouponCode", objOrder.CouponCode);
            cmd.Parameters.AddWithValue("@CouponDiscountAmount", objOrder.CouponDiscountAmount);
            cmd.Parameters.AddWithValue("@GiftCertiSerialNumber", objOrder.GiftCertiSerialNumber);
            cmd.Parameters.AddWithValue("@GiftCertificateDiscountAmount", objOrder.GiftCertificateDiscountAmount);
            cmd.Parameters.AddWithValue("@QuantityDiscountAmount", objOrder.QuantityDiscountAmount);
            cmd.Parameters.AddWithValue("@LevelDiscountPercent", objOrder.LevelDiscountPercent);
            cmd.Parameters.AddWithValue("@LevelDiscountAmount", objOrder.LevelDiscountAmount);
            cmd.Parameters.AddWithValue("@CustomDiscount", objOrder.CustomDiscount);
            cmd.Parameters.AddWithValue("@OrderSubtotal", objOrder.OrderSubtotal);
            cmd.Parameters.AddWithValue("@OrderTax", objOrder.OrderTax);
            cmd.Parameters.AddWithValue("@OrderShippingCosts", objOrder.OrderShippingCosts);
            cmd.Parameters.AddWithValue("@OrderTotal", objOrder.OrderTotal);
            cmd.Parameters.AddWithValue("@AuthorizationCode", objOrder.AuthorizationCode);
            cmd.Parameters.AddWithValue("@AuthorizationResult", objOrder.AuthorizationResult);
            cmd.Parameters.AddWithValue("@AuthorizationPNREF", objOrder.AuthorizationPNREF);
            cmd.Parameters.AddWithValue("@TransactionCommand", objOrder.TransactionCommand);
            cmd.Parameters.AddWithValue("@LastIPAddress", objOrder.LastIPAddress);
            cmd.Parameters.AddWithValue("@PaymentGateway", objOrder.PaymentGateway);
            cmd.Parameters.AddWithValue("@PaymentMethod", objOrder.PaymentMethod);
            cmd.Parameters.AddWithValue("@ShippingTrackingNumber", objOrder.ShippingTrackingNumber);
            cmd.Parameters.AddWithValue("@ShippedVIA", objOrder.ShippedVIA);
            cmd.Parameters.AddWithValue("@OrderStatus", objOrder.OrderStatus);
            cmd.Parameters.AddWithValue("@TransactionStatus", objOrder.TransactionStatus);
            cmd.Parameters.AddWithValue("@AVSResult", objOrder.AVSResult);
            cmd.Parameters.AddWithValue("@Cvc2Response", objOrder.Cvc2Response);
            cmd.Parameters.AddWithValue("@CaptureTxCommand", objOrder.CaptureTxCommand);
            cmd.Parameters.AddWithValue("@CaptureTXResult", objOrder.CaptureTXResult);
            cmd.Parameters.AddWithValue("@VoidTXCommand", objOrder.VoidTXCommand);
            cmd.Parameters.AddWithValue("@VoidTXResult", objOrder.VoidTXResult);
            cmd.Parameters.AddWithValue("@RefundTXCommand", objOrder.RefundTXCommand);
            cmd.Parameters.AddWithValue("@RefundTXResult", objOrder.RefundTXResult);
            cmd.Parameters.AddWithValue("@RefundReason", objOrder.RefundReason);
            cmd.Parameters.AddWithValue("@CartType", objOrder.CartType);
            cmd.Parameters.AddWithValue("@Last4", objOrder.Last4);
            cmd.Parameters.AddWithValue("@AuthorizedOn", objOrder.AuthorizedOn);
            cmd.Parameters.AddWithValue("@CapturedOn", objOrder.CapturedOn);
            cmd.Parameters.AddWithValue("@RefundedOn", objOrder.RefundedOn);
            cmd.Parameters.AddWithValue("@VoidedOn", objOrder.VoidedOn);
            cmd.Parameters.AddWithValue("@FraudedOn", objOrder.FraudedOn);
            cmd.Parameters.AddWithValue("@ShippedOn", objOrder.ShippedOn);
            cmd.Parameters.AddWithValue("@Deleted", objOrder.Deleted);
            cmd.Parameters.AddWithValue("@ReferralLink", objOrder.ReferralLink);
            cmd.Parameters.AddWithValue("@Referrer", objOrder.Referrer);
            cmd.Parameters.AddWithValue("@RefundedAmount", objOrder.RefundedAmount);
            cmd.Parameters.AddWithValue("@ChargeAmount", objOrder.ChargeAmount);
            cmd.Parameters.AddWithValue("@AuthorizedAmount", objOrder.AuthorizedAmount);
            cmd.Parameters.AddWithValue("@AdjustmentAmount", objOrder.AdjustmentAmount);
            cmd.Parameters.AddWithValue("@AdjustmentCapturedOn", objOrder.AdjustmentCapturedOn);
            cmd.Parameters.AddWithValue("@OrderNotes", objOrder.OrderNotes);
            cmd.Parameters.AddWithValue("@isGiftWrap", objOrder.isGiftWrap);
            cmd.Parameters.AddWithValue("@GiftWrapAmt", objOrder.GiftWrapAmt);
            cmd.Parameters.AddWithValue("@SalesRepName", objOrder.SalesRepName);
            cmd.Parameters.AddWithValue("@THUB_POSTED_TO_ACCOUNTING", objOrder.THUB_POSTED_TO_ACCOUNTING);
            cmd.Parameters.AddWithValue("@THUB_POSTED_DATE", objOrder.THUB_POSTED_DATE);
            cmd.Parameters.AddWithValue("@THUB_ACCOUNTING_REF", objOrder.THUB_ACCOUNTING_REF);
            cmd.Parameters.AddWithValue("@RefOrderID", objOrder.RefOrderID);
            cmd.Parameters.AddWithValue("@CouponCodeDescription", objOrder.CouponCodeDescription);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get Ref Number By OrderNumber
        /// </summary>
        /// <param name="OrderNumber">OrderNumber</param>
        /// <returns>Ref Number</returns>
        public DataSet GetRefNumberByOrderNumber(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            DataSet dsorder = new DataSet();
            cmd = new SqlCommand();
            cmd.CommandText = "select StoreName from tb_Store where StoreID =(select StoreID from tb_order where OrderNumber=" + OrderNumber + ") select RefOrderID from tb_order where OrderNumber=" + OrderNumber;
            return objSql.GetDs(cmd);
        }
        public DataSet GetInvoiceProductsWithMarryproduct(Int32 OrderID)
        {
            SQLAccess dbAccess = new SQLAccess();
            string Query = "select ISNULL(p.amazonproductid,'') as amazonproductid, ISNULL(osi.SKU,'') as MerchantSKU,ISNULL(osi.OrderItemID,'') OrderItemID,isnull(mp.OptionSku,'') as OptionSku,ISNULL(osi.DiscountPrice,0) as DiscountPrice,osi.refproductid as productid,isnull(osi.ProductName,mp.name) as Productname,mp.SKU,osi.VariantNames,osi.VariantValues,osi.Quantity as quantity," +
                " osi.Price as price,isnull(lp.upgradeQuantity,0) as UpgradeProductQty,p.ProductID as UpgradeProductID," +
                //" isnull(round(case when isnull(p.saleprice,0) =0 then p.price else p.saleprice end,2),0) as UpgradePrice," +
                " isnull(lp.UpgradePrice,0) as UpgradePrice, " +
                " (select 'PO-'+convert(nvarchar(10),poi.PONumber)+',' from tb_PurChaseOrderItems poi inner join tb_PurchaseOrder po on poi.PONumber=po.PONumber " +
                " and poi.Productid=osi.RefProductID and po.OrderNumber=o.OrderNumber for xml path('')) as PONumber from tb_orderedshoppingcartitems osi  inner join tb_order o on o.shoppingcardid=osi.orderedshoppingcartid " +
                " left join tb_product mp on mp.productid=osi.refproductid left join tb_lockproducts lp on lp.OrderNumber=o.OrderNumber and " +
                " lp.Productid=osi.refproductid AND lp.upgradeQuantity <> 0 " +
                //" and  isnull(lp.isCompleted,0)=1 " +
                " left join tb_product p on p.SKU = lp.marryproducts and p.StoreID=o.StoreID " +
                " where o.ordernumber=" + OrderID + " order by osi.OrderedCustomCartID ";
            return dbAccess.GetDs(Query);
        }

        public DataSet GetShippedItemsforOrderwithoutPOpackage(Int32 OrderID)
        {
            string query = "";
            SQLAccess dbAccess = new SQLAccess();
            query = "select Row_Number() over(Order By lp.productid asc) as Rowid, lp.productid as Refproductid,isnull(osi.Productname,p.name) as Productname,p.SKU,(lp.quantity) as quantity,convert(varchar(max),round(osi.price,2)) as price,  convert(varchar(max),round((osi.price*(lp.quantity)) ,2)) as SubTotal,'main' as type,osi.VariantNames as VariantNames,osi.VariantValues as VariantValues  " +
                    " from tb_orderedshoppingcartitems osi   " +
                    " inner join tb_order o on o.shoppingcardid=osi.orderedshoppingcartid   " +
                    " left join tb_product p on p.productid=osi.refproductid   " +
                    " inner join tb_lockproducts lp on lp.OrderNumber=o.OrderNumber and lp.productid=osi.refproductid " +
                    " where o.ordernumber=" + OrderID + " and isnull(isCompleted,0)=1 and lp.quantity <> 0 and isnull(lp.ispo,0)=0 And isnull(lp.ordercustomcartId,0) <> 0 " +
                    " UNION ALL   " +
                    " select DISTINCT Row_Number() over(Order By lp.productid asc) as Rowid,lp.productid as Refproductid,p.name as Productname,p.SKU,(lp.upgradeQuantity) as quantity,convert(varchar(max),round(case when isnull(p.saleprice,0) =0 then p.price else p.saleprice end,2)) as price,  convert(varchar(max),round((case when isnull(p.saleprice,0) =0 then p.price else p.saleprice end*(lp.upgradeQuantity)) ,2)) as SubTotal,'upgrade' as type,'' as VariantNames,'' as VariantValues from   tb_order o  " +
                    " inner join tb_lockproducts lp on lp.OrderNumber=o.OrderNumber  " +
                    " inner join tb_product p on   p.SKU = lp.marryproducts  " +
                    " where p.storeid=o.storeid and o.ordernumber=" + OrderID + " and isnull(isCompleted,0)=1 AND lp.upgradeQuantity <> 0 and isnull(lp.ispo,0)=0 ";

            return dbAccess.GetDs(query);
            //return GetResults(query);
        }
        public DataSet GetShippedItemsforOrderPOPicking(Int32 OrderID, Int32 storeid)
        {

            SQLAccess dbAccess = new SQLAccess();
            string query = "select Row_Number() over(Order By p.productid asc) as Rowid,isnull(p.upc,'') as upc, p.productid,osi.ProductName as Productname,p.SKU,(osi.quantity) as quantity,convert(varchar(max),round(osi.price,2)) as price, " +
                             " convert(varchar(max),round((osi.price*(osi.quantity)) ,2)) as SubTotal,'main' as type,osi.VariantNames,osi.VariantValues,p.weight,p.Length,p.width,p.height,'Drop Ship' as wtype,isnull(p.Location,'') as Location,osi.OrderedCustomCartID as ordercustomcartId from tb_orderedshoppingcartitems osi " +
                             " inner join tb_order o on o.shoppingcardid=osi.orderedshoppingcartid " +
                             " inner join tb_product p on p.productid=osi.refproductid " +
                             " where o.ordernumber=" + OrderID + " UNION ALL " +
                             "select Row_Number() over(Order By lp.productid asc) as Rowid,isnull(p.upc,'') as upc, lp.productid,osi.ProductName as Productname,p.SKU,(lp.quantity) as quantity,convert(varchar(max),round(osi.price,2)) as price, " +
                             " convert(varchar(max),round((osi.price*(lp.quantity)) ,2)) as SubTotal,'lock' as type,osi.VariantNames,osi.VariantValues,p.weight,p.Length,p.width,p.height,'In Warehouse' as wtype,isnull(p.Location,'') as Location,lp.ordercustomcartId FROM         dbo.tb_LockProducts AS lp INNER JOIN " +
                             "  dbo.tb_Order AS o ON lp.OrderNumber = o.OrderNumber AND lp.quantity <> 0 INNER JOIN " +
                             " dbo.tb_OrderedShoppingCartItems AS osi INNER JOIN dbo.tb_Product AS p ON p.ProductID = osi.refproductid ON lp.ProductID = osi.refproductid AND  lp.ordercustomcartId = osi.OrderedCustomCartID " +
                                 " where o.ordernumber=" + OrderID + " and lp.productid=osi.refproductid UNION ALL " +
                                 " select DISTINCT Row_Number() over(Order By lp.productid asc) as Rowid,isnull(p.upc,'') as upc,lp.productid,p.name as Productname,p.SKU,(lp.upgradeQuantity) as quantity,convert(varchar(max)," +
                                 "round(case when isnull(p.saleprice,0) =0 then p.price else p.saleprice end,2)) as price,  convert(varchar(max),round(" +
                                 "(case when isnull(p.saleprice,0) =0 then p.price else p.saleprice end*(lp.upgradeQuantity)) ,2)) as SubTotal,'upgrade' as type,'' as VariantNames,'' as VariantValues,p.weight,p.Length,p.width,p.height,'In Warehouse' as wtype,isnull(p.Location,'') as Location,lp.ordercustomcartId from  " +
                                 " tb_order o inner join tb_lockproducts lp on lp.OrderNumber=o.OrderNumber inner join tb_product p on " +
                                 "  p.SKU = lp.marryproducts where p.storeid=" + storeid + " and o.ordernumber=" + OrderID + " AND lp.upgradeQuantity <> 0";

            return dbAccess.GetDs(query);
        }

    }
}
