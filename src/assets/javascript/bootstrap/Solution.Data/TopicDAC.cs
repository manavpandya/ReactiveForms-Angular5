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
    /// Topic Data Access Class Contains Topic Related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class TopicDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get the Topic List
        /// </summary>
        /// <param name="TopicName">string TopicName</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the List of Topic List as a Dataset</returns>
        public DataSet GetTopicList(string TopicName, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Topic";
            cmd.Parameters.AddWithValue("@TopicName", TopicName);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }



        /// <summary>
        /// Insert new Topic
        /// </summary>
        /// <param name="topic">tb_Topic topic</param>
        /// <returns>Returns the tb_Topic Object</returns>
        public tb_Topic Create(tb_Topic topic)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_Topic(topic);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return topic;
        }

        /// <summary>
        /// Pop up selected Topic detail for edit mode
        /// </summary>
        /// <param name="TopicID">To get Topic based on selected Id</param>
        /// <returns>Returns tb_Topic object</returns>
        public tb_Topic GetTopicByID(int TopicID)
        {
            tb_Topic topic = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                topic = ctx.tb_Topic.First(e => e.TopicID == TopicID);
            }
            return topic;
        }

       /// <summary>
       /// Update Topic Record
       /// </summary>
        /// <param name="topic">tb_Topic topic</param>
       /// <returns>Returns the Affected rows</returns>
        public int Update(tb_Topic topic)
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
        /// Check Topic Name already exists or not
        /// </summary>
        /// <param name="topic">tb_Topic topic</param>
        /// <returns>Returns record count if fount else zero</returns>
        public int CheckDuplicate(tb_Topic topic)
        {
            Int32 isExists = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(topic.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var result = (from a in ctx.tb_Topic
                          where a.Deleted.Value == false
                          select new
                          {
                              topic = a,
                              store = a.tb_Store
                          });
            isExists = (from a in result
                        where a.topic.TopicName == topic.TopicName && a.store.StoreID == ID
                        && a.topic.TopicID != topic.TopicID
                        select new { a.topic.TopicID }
                            ).Count();
            return isExists;
        }



        /// <summary>
        /// Get Topic List By StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the list of topic list by StoreID</returns>
        public DataSet GetTopicListByStoreID(int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Topic";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Get Topic According StoreID
        /// </summary>
        /// <param name="TopicName">TopicName</param>
        /// <param name="StoreID">StoreID</param>
        /// <returns>Topic dataset</returns>
        public DataSet GetTopicAccordingStoreID(string TopicName, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Topic";
            cmd.Parameters.AddWithValue("@TopicName", TopicName);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        #endregion

    }
}
