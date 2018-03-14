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
    /// Product Type Data Access Class Contains Product Type related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ProductTypeDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Product Type By StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the list of product type by StoreID</returns>
        public DataSet GetProductTypeByStoreID(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductType_GetProductTypeByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Product Type Delivery By StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the list of product type by StoreID</returns>
        public DataSet GetProductTypeDeliveryByStoreID(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductTypeDelivery_GetProductTypeDeliveryByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product Type Delivery By StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the list of product type by StoreID</returns>
        public DataSet GetProductTypeByName(string Name, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductType_GetProductTypeByName";
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        #endregion

    }
}
