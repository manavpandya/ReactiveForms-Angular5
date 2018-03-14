using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// Customer Quote Component Class Contains Customer Quote related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CustomerQuoteDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Products for Customer Quote
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <param name="SearchField">string SearchField</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="OrderField">string OrderField</param>
        /// <param name="Order">string Order</param>
        /// <returns>Returns Products Data for Customer Quote</returns>
        public DataSet GetCustomerQuoteProducts(Int32 StoreID, Int32 Mode, string SearchField, string SearchValue, string OrderField, string Order)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CustomerQuote_GetProducts";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SearchField", SearchField);
            cmd.Parameters.AddWithValue("@SearchValue", SearchValue);
            cmd.Parameters.AddWithValue("@OrderField", OrderField);
            cmd.Parameters.AddWithValue("@Order", Order);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Customer Quote
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns CustomerQuoteID of inserted customer level</returns>
        public Int32 AddCustomerQuote(Int32 CustomerID, Int32 StoreID, Int32 LoginID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CustomerQuote";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@LoginID", LoginID);

            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Insert Customer Quote Items
        /// </summary>
        /// <param name="CustomerQuoteID">Int32 CustomerQuoteID</param>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="SKU">String SKU</param>
        /// <param name="Name">String Name</param>
        /// <param name="Options">String Options</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="Quantity">Int32 Quantity</param>
        /// <param name="Notes">String Notes</param>
        /// <returns>Returns Id of Inserted Customer Quote Items</returns>
        public Boolean AddCustomerQuoteItems(Int32 CustomerQuoteID, Int32 ProductID, String SKU, String Name, String Options, Decimal Price, Int32 Quantity, String Notes)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CustomerQuoteItems";
            cmd.Parameters.AddWithValue("@CustomerQuoteID", CustomerQuoteID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@SKU", SKU);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Options", Options);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@Notes", Notes);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Insert Customer Quote Revised
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="QuoteNumber">String QuoteNumber</param>
        /// <returns>Returns Id of Customer Quote Revised</returns>
        public Int32 AddCustomerQuoteRevised(Int32 CustomerID, Int32 StoreID, String QuoteNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CustomerQuote";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@QuoteNumber", QuoteNumber);
            cmd.Parameters.AddWithValue("@Mode", 4);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// For Delete CustomerQuote By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void Delete(int Id)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_CustomerQuote CustomerQuote = ctx.tb_CustomerQuote.First(c => c.CustomerQuoteID == Id);
            ctx.DeleteObject(CustomerQuote);

            tb_CustomerQuoteItems CustomerQuote1 = ctx.tb_CustomerQuoteItems.First(c => c.CustomerQuoteItemID == Id);
            ctx.DeleteObject(CustomerQuote1);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Delete CustomerQuote using CustomerQuoteID
        /// </summary>
        /// <param name="AddressID">Int32 AddressID</param>
        /// <returns>Returns Boolean value in the form of True or False</returns>
        public bool DeleteCustomerQuotesID(Int32 CustomerQuoteID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CustomerQuote";
            cmd.Parameters.AddWithValue("@CustomerQuoteID", CustomerQuoteID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            bool isDeleted = Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
            return isDeleted;
        }
        #endregion
    }
}
