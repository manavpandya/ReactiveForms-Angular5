using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// GiftCardUsage Content Data Access Class Contains GiftCard Usage related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class GiftCardUsageDAC
    {
        private static SqlCommand cmd = null;
        private static SQLAccess objSql = null;

        /// <summary>
        /// Get Gift Card Usage List
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetGiftCardUsageList(int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "sp_GiftCardUsageList";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }
    }
}
