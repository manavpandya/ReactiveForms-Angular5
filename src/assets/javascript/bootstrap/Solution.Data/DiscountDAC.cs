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
    /// Discount Data Access Class Contains Discount related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class DiscountDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Quantity Discount by StoreID
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Quantity Discount Data in DataSet</returns>
        public DataSet GetQuantityDiscountByStoreID(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_QuantityDiscount_GetQuantityDiscountByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        #endregion

    }
}
