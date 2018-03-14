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
    public class ReplenishmentFeedDAC
    {
        #region Declaration
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        #endregion
        
        #region Business Intelligence Report
        public DataSet GetStoreList()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Replenishment_StoreList";
         
            return objSql.GetDs(cmd);
        }
        public DataSet GetExistingStoreList()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Replenishment_ExistingStoreList";

            return objSql.GetDs(cmd);
        }
        public DataSet GetFieldDetails(Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Replenishment_FeedtemplateDetail";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            return objSql.GetDs(cmd);
        }
        public DataSet GetMappingColumnName()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Replenishment_MappingColumnName";
             
            return objSql.GetDs(cmd);
        }
        public DataSet GetSearchData(string Subcategory, string Sku_upc, string Fabriccode, string Datefrom, string Dateto)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Replenishment_SearchData";
            cmd.Parameters.AddWithValue("@Subcategory", Subcategory);
            cmd.Parameters.AddWithValue("@SKU_UPC", Sku_upc);
            cmd.Parameters.AddWithValue("@fabriccode", Fabriccode);
            cmd.Parameters.AddWithValue("@Datefrom", Datefrom);
            cmd.Parameters.AddWithValue("@Dateto", Dateto);
            return objSql.GetDs(cmd);
        }
        #endregion
    }
}
