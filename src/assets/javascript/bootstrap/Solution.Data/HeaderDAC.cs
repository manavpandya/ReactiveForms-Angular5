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
    public class HeaderDAC
    {
        #region Declaration

        private System.Data.SqlClient.SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion



        /// <summary>
        /// Insert new Topic
        /// </summary>
        /// <param name="topic">tb_Topic topic</param>
        /// <returns>Returns the tb_Topic Object</returns>
        public tb_HeaderText Create(tb_HeaderText HeaderText)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_HeaderText(HeaderText);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return HeaderText;
        }
        /// <summary>
        /// Pop up selected Topic detail for edit mode
        /// </summary>
        /// <param name="TopicID">To get Topic based on selected Id</param>
        /// <returns>Returns tb_Topic object</returns>
        public tb_HeaderText GetHeaderByID(int HeaderID)
        {
            tb_HeaderText topic = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                topic = ctx.tb_HeaderText.First(e => e.HeaderID == HeaderID);
            }
            return topic;
        }

        /// <summary>
        /// Update Topic Record
        /// </summary>
        /// <param name="topic">tb_Topic topic</param>
        /// <returns>Returns the Affected rows</returns>
        public int Update(tb_HeaderText topic)
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
    }
}
