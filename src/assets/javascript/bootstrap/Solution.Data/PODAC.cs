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
using Solution.Bussines;
namespace Solution.Data
{
    /// <summary>
    /// PO Data Access Class Contains Purchase Order related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class PODAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

       
        /// <summary>
        /// Get PO Shipping Status
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>Returns Shipping Status in Sting</returns>
        public String GetPOShippingStatus(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_POCheckShippingStatus";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        #endregion

    }
}
