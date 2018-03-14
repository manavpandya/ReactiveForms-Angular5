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
    /// Warehouse Data Access Class Contains Warehouse related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class WarehouseDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Warehouse Product List By StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Return Product list for display as a Dataset</returns>
        public DataSet GetWarehouseProductListByStoreID(int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Warehouse";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        #endregion

    }
}
