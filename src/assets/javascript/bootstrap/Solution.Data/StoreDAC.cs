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
    /// Store Component Class Contains Store related Data Logic function.     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 
    public class StoreDAC
    {
        #region Variables and property
        private static SqlCommand cmd = null;
        private static SQLAccess objSql = null;
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        #endregion

        #region Key Functions

        /// <summary>
        /// Get Store List
        /// </summary>
        /// <returns>Returns the List of Stores as a Dataset</returns>
        public static DataSet GetStoreList()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_StoreList";
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Store List By Menu
        /// </summary>
        /// <returns>Returns the Stores List as a Dataset</returns>
        public DataSet GetStoreListByMenu()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_StoreList";
            //cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Store List
        /// </summary>
        /// <returns>Returns tb_Store Store List</returns>
        public List<tb_Store> SelectStore()
        {
            List<tb_Store> resultSet = null;
            resultSet = ctx.tb_Store.ToList().OrderBy(tb_Store => tb_Store.StoreID).Where(s => s.Deleted.Value == false).ToList();
            return resultSet;
        }

        /// <summary>
        /// Check whether duplicate record is inserted or not
        /// </summary>
        /// <param name="store">tb_Store store</param>
        /// <returns>Returns Value - if value >0 , store with same name already exists</returns>
        public int CheckDuplicate(tb_Store store)
        {
            Int32 isExists = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExists = (from a in ctx.tb_Store
                            where a.StoreName == store.StoreName && a.Deleted == false
                            && a.StoreID != store.StoreID
                            select new { a.StoreID }).Count();
            }
            return isExists;
        }

        /// <summary>
        /// Insert new store config details
        /// </summary>
        /// <param name="store">tb_Store store</param>
        /// <returns>Returns the Store Table object</returns>
        public tb_Store Create(tb_Store store)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_Store(store);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return store;
        }

        /// <summary>
        /// Pop up selected Store detail for edit mode
        /// </summary>
        /// <param name="storeId">int storeId</param>
        /// <returns>Returns the Store Data by storeID</returns>
        public tb_Store GetStore(int storeId)
        {
            tb_Store store = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                store = ctx.tb_Store.First(e => e.StoreID == storeId);
            }
            return store;
        }

        /// <summary>
        /// Update Store Details
        /// </summary>
        /// <param name="store">tb_Store store</param>
        /// <returns>Returns Count of Rows affected</returns>
        public int Update(tb_Store store)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int RowsAffected = 0;
            try
            {
                RowsAffected = ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return RowsAffected;
        }

        /// <summary>
        /// Get the Yahoo Store List
        /// </summary>
        /// <returns>Returns the Yahoo Store list as a Dataset</returns>
        public static DataSet GetYahooStoreList()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_StoreList";
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);

        }
        #endregion
    }
}
