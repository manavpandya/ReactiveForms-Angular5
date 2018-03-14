using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Diagnostics;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Order Component Class Contains Order related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class OrderComponent
    {
        private static int _count = 0;
        RedTag_CCTV_Ecomm_DBEntities ctx = new RedTag_CCTV_Ecomm_DBEntities();

        #region Key Functions

        /// <summary>
        /// Get Chart Details For Order Statistic Report
        /// </summary>
        /// <param name="Duration">string Duration</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="StartDate">DateTime StartDate</param>
        /// <param name="EndDate">DateTime EndDate</param>
        /// <returns>Returns the Chart Details for Order Statistics Report as a Dataset</returns>
        public static DataSet GetChartDetailsForOrderStatisticReport(String Duration, Int32 StoreID, DateTime StartDate, DateTime EndDate, String status)
        {
            OrderDAC dac = new OrderDAC();
            DataSet DSOrderchart = new DataSet();
            DSOrderchart = dac.GetChartDetailsForOrderStatisticReport(Duration, StoreID, StartDate, EndDate, status);
            return DSOrderchart;
        }

        /// <summary>
        /// Function For Insert OrderComments
        /// </summary>
        /// <param name="objOrderComment">tb_OrderComment objOrderComment</param>
        /// <returns>Returns Identity Value of CommentID</returns>
        public int InsertOrderComments(tb_OrderComment objOrderComment)
        {
            int isAdded = 0;
            try
            {
                OrderDAC objOrder = new OrderDAC();
                objOrder.InsertOrderComments(objOrderComment);
                isAdded = objOrderComment.CommentID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Get All Order Comments by OrderID
        /// </summary>
        /// <param name="OrderID">int OrderID</param>
        /// <returns>Returns the List of Comments for OrderID, passed in parameter</returns>
        public List<tb_OrderComment> GetOrderCommentsByID(int OrderID)
        {
            List<tb_OrderComment> objOrderComment = new List<tb_OrderComment>();
            OrderDAC objOrder = new OrderDAC();
            objOrderComment = objOrder.GetOrderCommentsByID(OrderID);
            return objOrderComment;
        }

        /// <summary>
        /// Get order List By StoreId
        /// </summary>
        /// <param name="PageNumber">int PageNumber</param>
        /// <param name="PageSize">int PageSize</param>
        /// <param name="Search">string Search</param>
        /// <param name="StoreId">int StoreId</param>
        /// <returns>Returns the Order LIst by StoreID</returns>
        public static DataSet GetorderListByStoreId(Int32 PageNumber, Int32 PageSize, string Search, Int32 StoreId)
        {
            OrderDAC dac = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = dac.GetorderListByStoreId(PageNumber, PageSize, Search, StoreId);
            return DSOrder;
        }

        /// <summary>
        /// Get Order List For Multi Capture
        /// </summary>
        /// <param name="strWhereClus">String strWhereClus</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns the Order LIst for Multi Capture as a Dataset</returns>
        public static DataSet GetorderListForMultiCapture(String strWhereClus, Int32 Mode)
        {
            OrderDAC dac = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = dac.GetorderListForMultiCapture(strWhereClus, Mode);
            return DSOrder;
        }

        /// <summary>
        /// Get Order List By StoreId for Bulk Print
        /// </summary>
        /// <param name="PageNumber">int PageNumber</param>
        /// <param name="PageSize">int PageSize</param>
        /// <param name="Search">string Search</param>
        /// <param name="StoreId">int StoreId</param>
        /// <returns>Returns the Order List for bulk print as a Dataset</returns>
        public static DataSet GetorderListByStoreIdforBulkPrint(Int32 PageNumber, Int32 PageSize, string Search, Int32 StoreId)
        {
            OrderDAC dac = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = dac.GetorderListByStoreIdforBulkPrint(PageNumber, PageSize, Search, StoreId);
            return DSOrder;
        }

        /// <summary>
        /// Get Product Data By Cart id For Display List
        /// </summary>
        /// <param name="CartId">int CartID </param>
        /// <returns>Returns List of Cart Details</returns>
        public DataSet GetProductList(Int32 CartId)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = objOrder.GetProductListByCartId(CartId);
            return DSProduct;
        }

        /// <summary>
        /// Get Invoice Product List
        /// </summary>
        /// <param name="CartId">int CartId</param>
        /// <returns>Returns the Invoice Product List for particular cart</returns>
        public DataSet GetInvoiceProductList(Int32 CartId)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = objOrder.GetInvoiceProductList(CartId);
            return DSProduct;
        }

        /// <summary>
        /// Get Order Details By OrderNumber
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns Dataset</returns>
        public static DataSet GetOrderDetailsByOrderNumber(Int32 OrderNumber)
        {
            OrderDAC dac = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = dac.GetOrderDetailsByOrderNumber(OrderNumber);
            return DSOrder;
        }

        /// <summary>
        /// Update Order Status When Comment Added
        /// </summary>
        /// <param name="OrderNumber">Int OrderNumber</param>
        /// <param name="Orderstatus">String OrderStatus</param>
        /// <param name="OrderNotes">String OrderNotes</param>
        public void UpdateOrderStatus(Int32 OrderNumber, String Orderstatus, String OrderNotes)
        {
            OrderDAC dac = new OrderDAC();
            dac.UpdateOrderStatus(OrderNumber, Orderstatus, OrderNotes);
        }

        /// <summary>
        ///  Get Payment Gateway Default Selected
        /// </summary>
        /// <param name="PaymentMethod">string paymentMethod</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Payment Gateway List</returns>
        public DataSet GetPaymentGateway(string PaymentMethod, int StoreID)
        {
            OrderDAC objPayment = new OrderDAC();
            DataSet DSPayment = new DataSet();
            DSPayment = objPayment.GetPaymentGateway(PaymentMethod, StoreID);
            return DSPayment;
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
            OrderDAC objPayment = new OrderDAC();
            return Convert.ToInt32(objPayment.AddOrder(objOrder, OrderNumber, StoreID));
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
            OrderDAC objPayment = new OrderDAC();
            return Convert.ToInt32(objPayment.AddPhoneOrder(objOrder, OrderNumber, StoreID));
        }

        /// <summary>
        /// Add Customer When Phone Order
        /// </summary>
        ///<param name="objAddress">tb_Order objAddress</param>
        ///<param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 PhoneOrderAddCustomer(tb_Order objOrder, Int32 StoreID)
        {
            OrderDAC objCustomer = new OrderDAC();
            return Convert.ToInt32(objCustomer.PhoneOrderAddCustomer(objOrder, StoreID));
        }

        /// <summary>
        /// Get Order Details By OrderID
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns a Order Details by OrderID</returns>
        public DataSet GetOrderDetailsByOrderID(Int32 OrderNumber)
        {
            OrderDAC objPayment = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objPayment.GetOrderDetailsByOrderID(OrderNumber);
            return DSOrder;
        }
        
        /// <summary>
        ///  Get Order Details By OrderID
        /// </summary>
        /// <param name="PaymentMethod">Int32 OrderNumber</param>
        /// /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Dataset Order data</returns>
        public DataSet GetViewRecentOrderByOrderID(Int32 OrderNumber, int StoreID)
        {
            OrderDAC objPayment = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objPayment.GetViewRecentOrderByOrderID(OrderNumber, StoreID);
            return DSOrder;
        }

        /// <summary>
        ///  Get RMA Order Details By OrderID
        /// </summary>
        /// <param name="PaymentMethod">Int32 OrderNumber</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Dataset RMA Order data</returns>
        public DataSet GetViewRecentForRMAOrderByOrderID(Int32 OrderNumber, int StoreID)
        {
            OrderDAC objPayment = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objPayment.GetViewRecentForRMAOrderByOrderID(OrderNumber, StoreID);
            return DSOrder;
        }

        /// <summary>
        /// Add Order Failed Transaction 
        /// </summary>
        /// <param name="objFailedTransaction">tb_FailedTransaction objFailedTransaction</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 AddOrderFailedTransaction(tb_FailedTransaction objFailedTransaction, Int32 StoreID)
        {
            OrderDAC objFailed = new OrderDAC();
            return Convert.ToInt32(objFailed.AddOrderFailedTransaction(objFailedTransaction, StoreID));
        }

        /// <summary>
        /// Get Invoice Products for Update Order
        /// </summary>
        /// <param name="ShoppingCartID">Int32 ShoppingCartID</param>
        /// <returns>Returns the Invoice Products By Shopping Cart ID</returns>
        public DataSet GetInvoiceProducts(Int32 ShoppingCartID)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetInvoiceProducts(ShoppingCartID);
            return DSOrder;
        }

        /// <summary>
        /// Get Next/Previous Order
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>Returns a order according to next or previous</returns>
        public DataSet GetnextpreviousOrder(Int32 OrderNumber)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetnextpreviousOrder(OrderNumber);
            return DSOrder;
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
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetOrderNobyCustomerID(CustomerID, StoreID);
            return DSOrder;
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
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetCartItemForViewRecentOrder(OrderedShoppingCartID, OrderNo, StoreID);
            return DSOrder;
        }

        /// <summary>
        /// Get View Order Paging
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Paging Details for Order</returns>
        public DataSet GetViewOrderPaging(Int32 CustomerID, int StoreID)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetViewOrderPaging(CustomerID, StoreID);
            return DSOrder;
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
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetViewOldOrderNobyCustomerID(CustomerID, StoreID);
            return DSOrder;
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
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetCartItemForViewOldOrder(OrderedShoppingCartID, OrderNo, StoreID);
            return DSOrder;
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
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetParsalShippinginviewOldOrder(OrderedShoppingCartID, OrderNo, StoreID);
            return DSOrder;
        }

        /// <summary>
        /// Get CartID By CustomerID
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Cart ID of particular CustomerID </returns>
        public DataSet GetCartIDByCustID(Int32 CustomerID, int StoreID)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetCartIDByCustID(CustomerID, StoreID);
            return DSOrder;
        }

        /// <summary>
        /// Get Only List of Order Numbers by CustomerID
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns only list of Order Numbers By CustomerID</returns>
        public DataSet GetOnlyOrderNobyCustomerID(Int32 CustomerID, int StoreID)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetOnlyOrderNobyCustomerID(CustomerID, StoreID);
            return DSOrder;
        }

        /// <summary>
        /// Get Order By Product Details
        /// </summary>
        /// <param name="SearchKeyword">string SearchKeyword</param>
        /// <returns>Returns the Order Details by particular product Details</returns>
        public string GetOrderByProductDetails(string SearchKeyword)
        {
            OrderDAC objOrderProduct = new OrderDAC();
            return Convert.ToString(objOrderProduct.GetOrderByProductDetails(SearchKeyword));
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
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetSaleReportRecord(Param, StartDate, EndDate, StoreID);
            return DSOrder;
        }

        #endregion

        /// <summary>
        /// Get CartID By CustomerID for PhoneOrders
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns CartID by CustomerID for Phone Order</returns>
        public DataSet GetCartIDByCustIDPhoneOrder(Int32 CustomerID, int StoreID)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet DSOrder = new DataSet();
            DSOrder = objOrder.GetCartIDByCustIDPhoneOrders(CustomerID, StoreID);
            return DSOrder;
        }

        /// <summary>
        /// Get the datasource for the gridview
        /// </summary>
        /// <param name="startIndex">Int32 startindex</param>
        /// <param name="pStoreId">Int32 storeid</param>
        /// <param name="pageSize">Int32 Pagesize</param>
        /// <returns>Returns IQueryable</returns>
        public IQueryable<OrderComponentEntity> GetDataByFilter(Int32 startIndex, DateTime pStartDate, DateTime pEndDate, Int32 pStoreId, Int32 pageSize)
        {
            IQueryable<OrderComponentEntity> results = from a in ctx.tb_FailedTransaction
                                                       join b in ctx.tb_Customer on a.CustomerID equals b.CustomerID

                                                       select new OrderComponentEntity
                                                       {
                                                           TransactionID = a.TransactionID,
                                                           CustomerName = b.FirstName + " " + b.LastName,
                                                           OrderNumber = a.OrderNumber.Value,
                                                           PaymentGateway = a.PaymentGateway,
                                                           PaymentMethod = a.Paymentmethod,
                                                           TransactionResult = a.TransactionResult,
                                                           IPAddress = a.IPAddress,
                                                           OrderDate = a.OrderDate ?? DateTime.Now,
                                                           StoreID = a.tb_Store.StoreID,
                                                           StoreName = a.tb_Store.StoreName,
                                                           FailedTxnNote = a.FailedTxnNote,
                                                           IsEmailAlert = a.IsEmailAlert ?? true
                                                       };

            DateTime startDate = new DateTime(pStartDate.Year, pStartDate.Month, pStartDate.Day, 0, 0, 0);
            DateTime endDate = new DateTime(pEndDate.Year, pEndDate.Month, pEndDate.Day, 23, 59, 59);



            if (pStoreId == 0)
            {

                results = results.Where(a => a.OrderDate >= startDate && a.OrderDate <= endDate).AsQueryable();
                results = results.OrderByDescending(g => g.OrderDate);


            }
            else
            {
                results = results.OrderByDescending(g => g.OrderDate).Where(v => v.StoreID == pStoreId && v.OrderDate >= startDate && v.OrderDate <= endDate).AsQueryable();
            }
            _count = results.Count();

            results = results.Skip(startIndex).Take(pageSize).AsQueryable();


            return results;
        }


        /// <summary>
        /// Count the records in gridview
        /// </summary>
        /// <param name="pStoreId">Int32 storeID</param>
        /// <returns>Returns no of Count</returns>
        public static Int32 GetCount(Int32 pStoreId, DateTime pStartDate, DateTime pEndDate)
        {

            return _count;

        }

        /// <summary>
        /// Gets the details of particular failed transaction
        /// </summary>
        /// <param name="TransactionID">Int32 TransactionID</param>
        /// <returns>Returns tb_FailedTransaction Table Object</returns>
        public tb_FailedTransaction GetFailedTransaction(Int32 TransactionID)
        {
            OrderDAC orderdac = new OrderDAC();
            return orderdac.GetFailedTransaction(TransactionID);
        }

        /// <summary>
        /// Deletes the particular Failed Transaction
        /// </summary>
        /// <param name="TransactionID">Int32 TransactionID</param>
        public void DeleteFailedTransaction(Int32 TransactionID)
        {
            OrderDAC orderdac = new OrderDAC();
            orderdac.DeleteFailedTransaction(TransactionID);
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
            OrderDAC objOrder = new OrderDAC();
            objOrder.InsertOrderlog(StatusID, OrderNumber, Description, LogBy);
        }


        /// <summary>
        /// Update Inventory By OrderNumber
        /// </summary>
        /// <param name="StatusID">Int OrderNumber</param>
        /// <param name="OrderNumber">Int direction</param>       
        public void UpdateInventoryByOrderNumber(int OrderNumber, int direction)
        {
            OrderDAC objOrder = new OrderDAC();
            objOrder.UpdateInventoryByOrderNumber(OrderNumber, direction);
        }


        /// <summary>
        /// Get the OrderLog for Order Number
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        /// <returns>Returns Order Logs List</returns>
        public DataSet GetOrderLog(string OrderNumber)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet dsorderlog = new DataSet();
            dsorderlog = objOrder.GetOrderLog(OrderNumber);
            return dsorderlog;
        }

        /// <summary>
        /// Get order detail for particular Order
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns the Order Details by Order Number</returns>
        public DataSet GetDetailforOrder(string OrderNumber)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet dsorder = new DataSet();
            dsorder = objOrder.GetDetailforOrder(OrderNumber);
            return dsorder;
        }

        /// <summary>
        /// Get Shipping Detail for Cart Number
        /// </summary>
        /// <param name="CartNumber">string CartNumber</param>
        /// <returns>Returns the Shipping Details from Cart Number</returns>
        public DataSet GetShippingDetailforCartNumber(string OrderNumber)
        {
            OrderDAC objOrder = new OrderDAC();
            DataSet dsorder = new DataSet();
            dsorder = objOrder.GetShippingDetailforCartNumber(OrderNumber);
            return dsorder;
        }

        /// <summary>
        /// Update Print Status for Order
        /// </summary>
        /// <param name="OrderNumber">string OrderNumber</param>
        /// <returns>Returns the True if Updated</returns>
        public bool UpdatePrintStatusforOrder(string order)
        {
            OrderDAC dac = new OrderDAC();
            return dac.UpdatePrintStatusforOrder(order);
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
            OrderDAC objPayment = new OrderDAC();
            return Convert.ToInt32(objPayment.YahooAddOrder(objOrder, OrderNumber, StoreID));
        }
        /// <summary>
        /// Refund GiftCard Amount By order number
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns Message According to Result</returns>
        public String RefundGiftCardAmount(Int32 OrderNumber)
        {
            OrderDAC dac = new OrderDAC();
            return dac.RefundGiftCardAmount(OrderNumber);
        }
        public class OrderComponentEntity
        {
            private int _StoreID;

            public int StoreID
            {
                get { return _StoreID; }
                set { _StoreID = value; }

            }

            private int _TransactionID;

            public int TransactionID
            {
                get { return _TransactionID; }
                set { _TransactionID = value; }

            }

            private string _CustomerName;

            public string CustomerName
            {
                get { return _CustomerName; }
                set { _CustomerName = value; }
            }

            private int _OrderNumber;

            public int OrderNumber
            {
                get { return _OrderNumber; }
                set { _OrderNumber = value; }
            }

            private string _StoreName;

            public string StoreName
            {
                get { return _StoreName; }
                set { _StoreName = value; }
            }

            private string _PaymentGateway;

            public string PaymentGateway
            {
                get { return _PaymentGateway; }
                set { _PaymentGateway = value; }
            }

            private string _PaymentMethod;

            public string PaymentMethod
            {
                get { return _PaymentMethod; }
                set { _PaymentMethod = value; }
            }

            private string _TransactionResult;

            public string TransactionResult
            {
                get { return _TransactionResult; }
                set { _TransactionResult = value; }
            }

            private string _IPAddress;

            public string IPAddress
            {
                get { return _IPAddress; }
                set { _IPAddress = value; }
            }

            private DateTime _OrderDate;

            public DateTime OrderDate
            {
                get { return _OrderDate; }
                set { _OrderDate = value; }
            }

            private Boolean _IsEmailAlert;

            public Boolean IsEmailAlert
            {
                get { return _IsEmailAlert; }
                set { _IsEmailAlert = value; }
            }

            private string _FailedTxnNote;

            public string FailedTxnNote
            {
                get { return _FailedTxnNote; }
                set { _FailedTxnNote = value; }
            }
        }

        /// <summary>
        /// Gets IPAddress Detail
        /// </summary>
        /// <param name="IPaddress">string IPaddress</param>
        /// <returns>Returns IP Address Details</returns>
        public bool GetIPAddressDetail(string IPaddress)
        {
            OrderDAC objOrder = new OrderDAC();
            if (objOrder.GetIPAddressDetail(IPaddress) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Deletes IPAddress Detail
        /// </summary>
        /// <param name="IPaddress">string IPaddress</param>
        public void DeleteIPAddressDetail(string IPaddress)
        {
            OrderDAC objOrder = new OrderDAC();
            objOrder.DeleteIPAddressDetail(IPaddress);
        }

        /// <summary>
        /// Inserts IP Address Detail
        /// </summary>
        /// <param name="tb_restricted">tb_RestrictedIP tb_restricted</param>
        public void InsertIPAddressDetail(tb_RestrictedIP tb_restricted)
        {
            OrderDAC objOrder = new OrderDAC();
            objOrder.InsertIPAddressDetail(tb_restricted);
        }

        /// <summary>
        /// Import Order Cart for Change Orders
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns the Order Cart Details for Change order</returns>
        public static int ImportOrderCartforChangeOrder(int OrderNumber)
        {
            OrderDAC dac = new OrderDAC();
            return dac.ImportOrderCartforChangeOrders(OrderNumber);
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
        public bool InsertCartItemChangeOrder(int OrderNumber, int ProductID, int Quantity, Decimal Price, string VariantNames, string VariantValues,decimal DicPrice)
        {
            OrderDAC objOrder = new OrderDAC();
            if (Convert.ToBoolean(objOrder.InsertCartItemChangeOrder(OrderNumber, ProductID, Quantity, Price, VariantNames, VariantValues, DicPrice)))
            {
                return true;
            }
            else
                return false;
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
            OrderDAC objOrder = new OrderDAC();
            if (Convert.ToBoolean(objOrder.UpdateCartItemChangeOrder(OrderNumber, ProductID, Quantity, VariantNames, VariantValues)))
            {
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// Get Order By Order Number
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <returns>Returns the Order Details by Order Number</returns>
        public tb_Order GetOrderByOrderNumber(int OrderNumber)
        {
            OrderDAC dac = new OrderDAC();
            return dac.GetOrderByOrderNumber(OrderNumber);
        }

        /// <summary>
        /// Update Order Details
        /// </summary>
        /// <param name="order">tb_Order order</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public int UpdateOrder(tb_Order order)
        {
            OrderDAC dac = new OrderDAC();
            return dac.UpdateOrder(order);
        }


        /// <summary>
        /// Insert Vendor Purchase Order Details
        /// </summary>
        /// <param name="vendorid">int vendorid</param>
        /// <param name="podate">datetime podate</param>
        /// <returns>Returns True if Inserted</returns>
        public bool InsertVendorPrurchaseOrderDetails(int vendorid, DateTime podate)
        {
            OrderDAC objorder = new OrderDAC();
            if (Convert.ToBoolean(objorder.InsertVendorPrurchaseOrderDetails(vendorid, podate)))
            {
                return true;
            }
            else
            {
                return false;
            }
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
            OrderDAC objorder = new OrderDAC();
            if (Convert.ToBoolean(objorder.InsertVendorPrurchaseOrderItemDetails(vendorid, productid, quantity, price, podate)))
            {
                return true;
            }
            else
            {
                return false;
            }
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
            OrderDAC objorder = new OrderDAC();
            if (Convert.ToBoolean(objorder.InsertVendorPrurchaseOrderItemDetailsForVendorQuote(vendorid, productid, quantity, price, podate, VendorQuoteRequestID, VendorQuoteReqDetailsID)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Select Vendor Purchase Order Detail
        /// </summary>
        /// <param name="vendorid">int vendorid</param>
        /// <param name="podate">int podate</param>
        /// <returns>Returns the List of Vendor purchase order details</returns>
        public DataSet SelectVendorPurchaseOrderDeatial(int vendorid, System.DateTime podate)
        {
            DataSet dsvendordetails = new DataSet();
            OrderDAC objorder = new OrderDAC();
            dsvendordetails = objorder.SelectVendorPurchaseOrderDeatial(vendorid, podate);
            return dsvendordetails;
        }

        /// <summary>
        /// Select Purchase Order Details Vendor
        /// </summary>
        /// <param name="vendorid">int vendorid</param>
        /// <param name="podate">datetime podate</param>
        /// <returns>Returns the List of Vendor purchase order details</returns>
        public DataSet SelectPurchaserOrderDetailsVendor(int vendorid, System.DateTime podate)
        {
            DataSet dsvendordetails = new DataSet();
            OrderDAC objorder = new OrderDAC();
            dsvendordetails = objorder.SelectPurchaserOrderDetailsVendor(vendorid, podate);
            return dsvendordetails;
        }

        /// <summary>
        /// Get Ref Number By OrderNumber
        /// </summary>
        /// <param name="OrderNumber">OrderNumber</param>
        /// <returns>Ref Number</returns>
        public DataSet GetRefNumberByOrderNumber(Int32 OrderNumber)
        {
            DataSet ds = new DataSet();
            OrderDAC objorder = new OrderDAC();
            ds = objorder.GetRefNumberByOrderNumber(OrderNumber);
            return ds;
        }
        public DataSet GetInvoiceProductsWithMarryproduct(Int32 OrderID)
        {
            DataSet ds = new DataSet();
            OrderDAC objorder = new OrderDAC();
            ds = objorder.GetInvoiceProductsWithMarryproduct(OrderID);
            return ds;
        }
        public DataSet GetShippedItemsforOrderwithoutPOpackage(Int32 OrderID)
        {
            DataSet ds = new DataSet();
            OrderDAC objorder = new OrderDAC();
            ds = objorder.GetShippedItemsforOrderwithoutPOpackage(OrderID);
            return ds;
        }
        public DataSet GetShippedItemsforOrderPOPicking(Int32 OrderID, Int32 storeid)
        {
            DataSet ds = new DataSet();
            OrderDAC objorder = new OrderDAC();
            ds = objorder.GetShippedItemsforOrderPOPicking(OrderID, storeid);
            return ds;
        }
        
    }
}
