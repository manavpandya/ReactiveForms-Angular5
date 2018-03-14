using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Solution.Bussines.Entities;

namespace Solution.Data
{
    /// <summary>
    /// Admin Configuration Data Access Class Contains Admin Configuration Related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class AppConfigDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions
        /// <summary>
        /// Appconfig Details
        /// </summary>
        /// <param name="Storeid">Unique StoreId</param>
        /// <returns>Get Appconfig List</returns>
        public DataSet GetAppConfig(int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_AppConfig]";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }
        #endregion
    }
}
