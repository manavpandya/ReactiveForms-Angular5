using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Entities;
using System.Diagnostics;
namespace Solution.Data
{
    /// <summary>
    /// SearchLog Data Access Class Contains Search Log related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class SearchLogDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions
        /// <summary>
        /// Insert an email entry in search log table
        /// </summary>
        /// <param name="tb_SearchLog">tb_SearchLog tb_SearchLog</param>
        /// <returns>Returns an Identity value of recently inserted SearchLog record</returns>
        public tb_SearchLog InsertSearchLog(tb_SearchLog tb_SearchLog)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_SearchLog(tb_SearchLog);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return tb_SearchLog;
        }


        #endregion
    }
}
