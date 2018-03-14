using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Topic Component Class Contains Topic related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class TopicComponent
    {
        #region Declaration
        int _count;
        #endregion

        #region Key Function

        /// <summary>
        /// Get the Topic List
        /// </summary>
        /// <param name="TopicName">string TopicName</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the List of Topic List as a Dataset</returns>
        public static DataSet GetTopicList(string TopicName, int Storeid)
        {
            DataSet DSTopic = new DataSet();
            TopicDAC dac = new TopicDAC();
            DSTopic = dac.GetTopicList(TopicName, Storeid);
            return DSTopic;
        }


        /// <summary>
        /// Get Topic Details by StoreID
        /// </summary>
        /// <param name="TopicName">int StoreID</param>
        /// <returns>Topic Details</returns>
        public DataSet GetTopicListByStoreID(int StoreID)
        {
            DataSet DSTopic = new DataSet();
            TopicDAC dac = new TopicDAC();
            DSTopic = dac.GetTopicListByStoreID(StoreID);
            return DSTopic;
        }

        /// <summary>
        /// Get data to fill grid
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="StoreId">int StoreId</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns IQueryable</returns>
        public IQueryable<TopicEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, int StoreId, string SearchValue)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                List<TopicEntity> lstTopic = new List<TopicEntity>();
                IQueryable<TopicEntity> results = from a in ctx.tb_Topic
                                                  where a.Deleted.Value == false
                                                  select new TopicEntity
                                                              {
                                                                  TopicID = a.TopicID,
                                                                  TopicName = a.TopicName,
                                                                  CreatedOn = a.CreatedOn.Value,
                                                                  StoreName = a.tb_Store.StoreName,
                                                                  StoreID = a.tb_Store.StoreID
                                                              };
                if (string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = "";
                }
                else
                {
                    SearchValue = SearchValue.Trim();
                }
                if (StoreId != -1)
                {
                    //search by TopicName and StoreId
                    results = results.Where(a => a.StoreID == StoreId && a.TopicName.Contains(SearchValue)).AsQueryable();
                }
                else
                {
                    //Search by topicName
                    results = results.Where(a => a.TopicName.Contains(SearchValue)).AsQueryable();
                }
                _count = results.Count();
                //Logic for searching
                if (!string.IsNullOrEmpty(sortBy))
                {
                    System.Reflection.PropertyInfo property = results.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                    String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (SortingOption.Length == 1)
                    {
                        results = results.OrderByField(SortingOption[0].ToString(), true);
                    }
                    else if (SortingOption.Length == 2)
                    {
                        results = results.OrderByField(SortingOption[0].ToString(), false);
                    }
                }
                else
                {
                    //Default sorting by Config Name
                    results = results.OrderBy(o => o.TopicName);
                }
                results = results.Skip(startIndex).Take(pageSize);
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Get total count used for paging
        /// </summary>
        /// <param name="StoreId">int StoreId</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns Total Count</returns>
        public int GetCount(int StoreId, string SearchValue)
        {
            return _count;
        }

        /// <summary>
        /// Add new Topic
        /// </summary>
        /// <param name="topic">tb_Topic topic</param>
        /// <returns>Returns Id of new inserted Topic</returns>
        public Int32 CreateTopic(tb_Topic topic)
        {
            Int32 isAdded = 0;
            try
            {
                TopicDAC dac = new TopicDAC();
                dac.Create(topic);
                isAdded = topic.TopicID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Update Topic
        /// </summary>
        /// <param name="topic">tb_Topic topic</param>
        /// <returns>Returns Id of updated Topic</returns>
        public Int32 Update(tb_Topic topic)
        {
            int RowsAffected = 0;
            try
            {
                TopicDAC dac = new TopicDAC();
                RowsAffected = dac.Update(topic);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RowsAffected;
        }

        /// <summary>
        /// Get Topic  detail for Edit mode
        /// </summary>
        /// <param name="Id">Get Topic  detail for selected Id</param>
        /// <returns>Returns tb_Topic object</returns>
        public tb_Topic getTopicByID(int Id)
        {
            TopicDAC dac = new TopicDAC();
            return dac.GetTopicByID(Id);
        }

        /// <summary>
        /// Check Topic name is already inserted or not
        /// </summary>
        /// <param name="custLevel">tb_Topic topic</param>
        /// <returns>returns true if found else false</returns>
        public bool CheckDuplicate(tb_Topic topic)
        {
            TopicDAC dac = new TopicDAC();
            if (dac.CheckDuplicate(topic) == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Get Topic According StoreID
        /// </summary>
        /// <param name="TopicName">TopicName</param>
        /// <param name="StoreID">StoreID</param>
        /// <returns>Topic dataset</returns>
        public static DataSet GetTopicAccordingStoreID(string TopicName, Int32 StoreID)
        {
            DataSet DSTopic = new DataSet();
            TopicDAC dac = new TopicDAC();
            DSTopic = dac.GetTopicAccordingStoreID(TopicName, StoreID);
            return DSTopic;
        }


        #endregion
    }

    public class TopicEntity
    {
        private int _TopicID;
        private int _StoreID;
        private string _TopicName;
        private DateTime _CreatedOn;
        private string _StoreName;

        public int TopicID
        {
            get { return _TopicID; }
            set { _TopicID = value; }
        }
        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }
        public string TopicName
        {
            get { return _TopicName; }
            set { _TopicName = value; }
        }
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }
    }
}
