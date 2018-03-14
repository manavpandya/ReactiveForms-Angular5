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
    /// RMA Data Access Class Contains Return Merchandise related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class RMADAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region User Define Property

        private int _ReturnItemID;
        private int _OrderedCustomerID;
        private int _OrderedNumber;
        private String _CustomerName;
        private String _CustomerEMail;
        private DateTime _OrderDate;
        private int _ProductID;
        private int _Quantity;
        private int _OrderedCustomCartID;
        
        private String _ReturnReason;
        private String _AdditionalInformation;
        private bool _Deleted;
        private DateTime _CreatedOn;
        private bool _isArrived;
        private String _ReturnNotes;
        private String _ReturnImages;
        private String _ReturnFee;
        private bool _IsReturn;
        private String _ReturnType;
        private String _RetunitemNotes;
        private bool _isReturnrequest;
        private int _MailLogID;

        public int ReturnItemID { get { return _ReturnItemID; } set { _ReturnItemID = value; } }
        public int OrderedCustomerID { get { return _OrderedCustomerID; } set { _OrderedCustomerID = value; } }
        public int OrderedNumber { get { return _OrderedNumber; } set { _OrderedNumber = value; } }
        public String CustomerName { get { return _CustomerName; } set { _CustomerName = value; } }
        public string CustomerEMail { get { return _CustomerEMail; } set { _CustomerEMail = value; } }
        public DateTime OrderDate { get { return _OrderDate; } set { _OrderDate = value; } }
        public int ProductID { get { return _ProductID; } set { _ProductID = value; } }
        public int Quantity { get { return _Quantity; } set { _Quantity = value; } }
        public String ReturnReason { get { return _ReturnReason; } set { _ReturnReason = value; } }
        public String AdditionalInformation { get { return _AdditionalInformation; } set { _AdditionalInformation = value; } }
        public bool Deleted { get { return _Deleted; } set { _Deleted = value; } }
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public bool isArrived { get { return _isArrived; } set { _isArrived = value; } }
        public String ReturnNotes { get { return _ReturnNotes; } set { _ReturnNotes = value; } }
        public String ReturnImages { get { return _ReturnImages; } set { _ReturnImages = value; } }
        public String ReturnFee { get { return _ReturnFee; } set { _ReturnFee = value; } }
        public bool IsReturn { get { return _IsReturn; } set { _IsReturn = value; } }
        public String ReturnType { get { return _ReturnType; } set { _ReturnType = value; } }
        public String RetunitemNotes { get { return _RetunitemNotes; } set { _RetunitemNotes = value; } }
        public bool isReturnrequest { get { return _isReturnrequest; } set { _isReturnrequest = value; } }
        public int MailLogID { get { return _MailLogID; } set { _MailLogID = value; } }
        public int OrderedCustomCartID { get { return _OrderedCustomCartID; } set { _OrderedCustomCartID = value; } }
        
        #endregion

        /// <summary>
        /// Add Item Into Cart
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns>Returns the Identity Value</returns>
        public Int32 AddReturnItem()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_RMA_ReturnItem";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.AddWithValue("@OrderedCustomerID", this.OrderedCustomerID);
            cmd.Parameters.AddWithValue("@OrderedNumber", this.OrderedNumber);
            cmd.Parameters.AddWithValue("@CustomerName", this.CustomerName);
            cmd.Parameters.AddWithValue("@CustomerEmail ", this.CustomerEMail);
            cmd.Parameters.AddWithValue("@OrderDate", this.OrderDate);
            cmd.Parameters.AddWithValue("@ProductId ", this.ProductID);
            cmd.Parameters.AddWithValue("@Quantity", this.Quantity);
            cmd.Parameters.AddWithValue("@ReturnReason", this.ReturnReason);
            cmd.Parameters.AddWithValue("@AdditionalInformation ", this.AdditionalInformation);
            cmd.Parameters.AddWithValue("@Deleted", this.Deleted);
            cmd.Parameters.AddWithValue("@CreatedOn", this.CreatedOn);
            cmd.Parameters.AddWithValue("@ReturnFee", this.ReturnFee);
            cmd.Parameters.AddWithValue("@ReturnType", this.ReturnType);
            cmd.Parameters.AddWithValue("@mode", 1);
            cmd.Parameters.AddWithValue("@OrderedCustomCartID", this.OrderedCustomCartID);
            
            return Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
            //objSql.ExecuteNonQuery(cmd);
            //return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get RMA Details by OrderedCustomerID 
        /// </summary>
        /// <param name="ID"></param>
        /// <returns>Returns the RMA list by CustomerID</returns>
        public DataSet GetRMAListByID(int ID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_RMA_ReturnItem";
            cmd.Parameters.AddWithValue("@OrderedCustomerID", ID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Ordered Shopping Cart Items Form Return Item
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="ProductID">string ProductID</param>
        /// <returns>Returns the List of Shopping Cart Items for RMA</returns>
        public DataSet GetOrderedShoppingCartItemsFormRetrunItem(int ID, string ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_RMA_ReturnItem";
            cmd.Parameters.AddWithValue("@OrderedNumber", ID);
            cmd.Parameters.AddWithValue("@ProductId", ProductID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get RMA Product By RMAID
        /// </summary>
        /// <param name="ReturnItemID">int ReturnItemID</param>
        /// <returns>Returns the list of products by ReturnItemID</returns>
        public DataSet GetRMAProductByRMAID(int ReturnItemID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_RMA_ReturnItem";
            cmd.Parameters.AddWithValue("@ReturnID", ReturnItemID);
            cmd.Parameters.AddWithValue("@Mode", 4);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Return Item
        /// </summary>
        /// <param name="StrID">String StrID</param>
        /// <param name="searchstr">String searchstr</param>
        /// <returns>Returns all return items list</returns>
        //public DataSet GetAllReturnItem(String StrID, String searchstr)
        //{
        //    String query = "select s.StoreName,isnull(ReturnType,'') as ReturnType,isnull(IsReturn,0) as IsReturn,ReturnItemID As id,ReturnItemID,'RMA-'+ convert(nvarchar(50),ReturnItemID) as 'RMANo',OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,o.OrderDate,tb_Product.ProductID,tb_Product.Name as ProductName,Quantity,ReturnReason,AdditionalInformation,tb_ReturnItem.Deleted As 'Status',tb_ReturnItem.CreatedOn,isnull(tb_ReturnItem.isReturnrequest,0) as isReturnrequest from tb_ReturnItem"
        //        + " join tb_Product on tb_ReturnItem.ProductID=tb_Product.ProductID inner join tb_order o on o.ordernumber=ORderedNumber inner join tb_store s on s.storeid=o.storeid  where tb_Product.storeid=" + StrID + searchstr + " order by tb_ReturnItem.CreatedOn desc";
        //    objSql = new SQLAccess();
        //    cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = query;
        //    return objSql.GetDs(cmd);
        //}

        public DataSet GetAllReturnItem(String StrID, String searchstr)
        {
            //String query = "select s.StoreName,isnull(ReturnType,'') as ReturnType,isnull(IsReturn,0) as IsReturn,ReturnItemID As id,ReturnItemID,'RMA-'+ convert(nvarchar(50),ReturnItemID) as 'RMANo',OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,o.OrderDate,tb_Product.ProductID,tb_Product.Name as ProductName,Quantity,ReturnReason,AdditionalInformation,tb_ReturnItem.Deleted As 'Status',tb_ReturnItem.CreatedOn,isnull(tb_ReturnItem.isReturnrequest,0) as isReturnrequest from tb_ReturnItem"
            //    + " join tb_Product on tb_ReturnItem.ProductID=tb_Product.ProductID inner join tb_order o on o.ordernumber=ORderedNumber inner join tb_store s on s.storeid=o.storeid  where tb_Product.storeid=" + StrID + searchstr + " order by tb_ReturnItem.CreatedOn desc";

            string query = @"select  s.StoreName,isnull(ReturnType,'') as ReturnType,isnull(IsReturn,0) as IsReturn,tb_Return.ReturnID As id,tb_Return.ReturnID,
'RMA-'+ convert(nvarchar(50),tb_Return.ReturnID) as 'RMANo',OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,o.OrderDate,
tb_Product.ProductID,tb_Product.Name as ProductName,Quantity,ReturnReason,AdditionalInformation,tb_Return.Deleted As 'Status',
tb_Return.CreatedOn,isnull(tb_Return.isReturnrequest,0) as isReturnrequest
 from tb_ReturnItem
inner join tb_Return on tb_ReturnItem.ReturnID=tb_Return.ReturnID inner join tb_order o on o.OrderNumber=ORderedNumber
inner join tb_Product on tb_ReturnItem.ProductID=tb_Product.ProductID inner join tb_store s
 on s.storeid=o.storeid where tb_Product.storeid=" + StrID + searchstr + "  order by tb_Return.CreatedOn desc";


            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Return Item
        /// </summary>
        /// <param name="searchstr">String searchstr</param>
        /// <returns>Returns all return items list</returns>
        //public DataSet GetAllReturnItem(String searchstr)
        //{
        //    String query = "select s.StoreName,isnull(ReturnType,'') as ReturnType,isnull(IsReturn,0) as IsReturn,ReturnItemID As id,ReturnItemID,'RMA-'+ convert(nvarchar(50),ReturnItemID) as 'RMANo',OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,o.OrderDate,tb_Product.ProductID,tb_Product.Name as ProductName,Quantity,ReturnReason,AdditionalInformation,tb_ReturnItem.Deleted As 'Status',tb_ReturnItem.CreatedOn,isnull(tb_ReturnItem.isReturnrequest,0) as isReturnrequest from tb_ReturnItem"
        //        + " join tb_Product on tb_ReturnItem.ProductID=tb_Product.ProductID inner join tb_order o on o.ordernumber=ORderedNumber inner join tb_store s on s.storeid=o.storeid " + searchstr + " order by tb_ReturnItem.CreatedOn desc";
        //    objSql = new SQLAccess();
        //    cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.Text;
        //    cmd.CommandText = query;
        //    return objSql.GetDs(cmd);
        //}


        public DataSet GetAllReturnItem(String searchstr)
        {
            //String query = "select s.StoreName,isnull(ReturnType,'') as ReturnType,isnull(IsReturn,0) as IsReturn,ReturnItemID As id,ReturnItemID,'RMA-'+ convert(nvarchar(50),ReturnItemID) as 'RMANo',OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,o.OrderDate,tb_Product.ProductID,tb_Product.Name as ProductName,Quantity,ReturnReason,AdditionalInformation,tb_ReturnItem.Deleted As 'Status',tb_ReturnItem.CreatedOn,isnull(tb_ReturnItem.isReturnrequest,0) as isReturnrequest from tb_ReturnItem"
            //    + " join tb_Product on tb_ReturnItem.ProductID=tb_Product.ProductID inner join tb_order o on o.ordernumber=ORderedNumber inner join tb_store s on s.storeid=o.storeid " + searchstr + " order by tb_ReturnItem.CreatedOn desc";

            string query = @"select  s.StoreName,isnull(ReturnType,'') as ReturnType,isnull(IsReturn,0) as IsReturn,tb_Return.ReturnID As id,tb_Return.ReturnID,
'RMA-'+ convert(nvarchar(50),tb_Return.ReturnID) as 'RMANo',OrderedCustomerID,OrderedNumber,CustomerName,CustomerEMail,o.OrderDate,
tb_Product.ProductID,tb_Product.Name as ProductName,Quantity,ReturnReason,AdditionalInformation,tb_Return.Deleted As 'Status',
tb_Return.CreatedOn,isnull(tb_Return.isReturnrequest,0) as isReturnrequest



 from tb_ReturnItem
inner join tb_Return on tb_ReturnItem.ReturnID=tb_Return.ReturnID inner join tb_order o on o.OrderNumber=ORderedNumber
inner join tb_Product on tb_ReturnItem.ProductID=tb_Product.ProductID inner join tb_store s
 on s.storeid=o.storeid " + searchstr + " order by tb_Return.CreatedOn desc";
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = query;
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Ordered Shopping Cart Items Form Return Item
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <param name="ProductID">string ProductID</param>
        /// <returns>Returns the List of Shopping Cart Items for RMA</returns>
        public DataSet GetOrderedShoppingCartItemsFormRetrunItem(int ID, string ProductID, int customcartid)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_RMA_ReturnItem";
            cmd.Parameters.AddWithValue("@OrderedNumber", ID);
            cmd.Parameters.AddWithValue("@ProductId", ProductID);
            cmd.Parameters.AddWithValue("@OrderedCustomCartID", customcartid);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }
    }
}
