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
    /// NewsSubscribtion Data Access Class Contains News Subscribtion related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    
    public class NewsSubscribtionDAC
    {
       
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get News Sub Subscription List
        /// </summary>
        /// <param name="Email">string Email</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetNewsSubscribtionList(string Email, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_NewsSubscribtion";
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// update News Subscription
        /// </summary>
        /// <param name="News">tb_NewsSubscription News</param>
        /// <returns>Returns Int32</returns>
        public int Update(tb_NewsSubscription News)
        {
            int RowsAffected = 0;
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
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
        /// Function For Create News Subscription
        /// </summary>
        /// <param name="News">tb_NewsSubscription News</param>
        /// <returns>Returns Identity value for inserted record</returns>
        public tb_NewsSubscription Create(tb_NewsSubscription News)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_NewsSubscription(News);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return News;
        }

        /// <summary>
        ///  For Delete News By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void Delete(int Id)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_NewsSubscription News = ctx.tb_NewsSubscription.First(c => c.NewsSubscriptionID == Id);
            ctx.DeleteObject(News);
            ctx.SaveChanges();
        }
        #endregion
    }
}
