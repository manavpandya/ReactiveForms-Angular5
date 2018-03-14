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
  public  class GenerateInventoryFeedDAC
    {

        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        /// <summary>
        /// get Sales channel partner list
        /// </summary>
        public DataSet GetSalesPartnerList()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetSalesChannelPartnerList";
            cmd.Parameters.AddWithValue("@Mode", 1);

            return objSql.GetDs(cmd);

        }


        /// <summary>
        /// get Sales channel partner Feed Template
        /// </summary>
        public DataSet GetChannelPartnerFeedTemplate( Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetChannelPartnerFeedFields";
            cmd.Parameters.AddWithValue("@Mode", 1);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);

        }




        public void InsertFeedLog(String FileName, Int32 GeneratedBy, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetChannelPartnerFeedFields";
            cmd.Parameters.AddWithValue("@Mode", 2);
            cmd.Parameters.AddWithValue("@GeneratedBy", GeneratedBy);
            cmd.Parameters.AddWithValue("@FileName", FileName);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            objSql.ExecuteNonQuery(cmd);
        }
    }
}
