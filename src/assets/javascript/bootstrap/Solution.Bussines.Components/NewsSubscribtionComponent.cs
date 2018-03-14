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
    /// News Subscribtion Component Class Contains News Subscribtion related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class NewsSubscribtionComponent
    {
        #region Declaration
        int _count;
        public static IQueryable<NewsSubscribtionEntity> results;
        #endregion

        #region Key Functions

        /// <summary>
        /// Get News Subscription List
        /// </summary>
        /// <param name="Email">string Email</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetNewsSubscribtionList(string Email, Int32 StoreID)
        {
            DataSet DSNews = new DataSet();
            NewsSubscribtionDAC dac = new NewsSubscribtionDAC();
            DSNews = dac.GetNewsSubscribtionList(Email, StoreID);
            return DSNews;
        }

        /// <summary>
        /// update News Subscription
        /// </summary>
        /// <param name="News">tb_NewsSubscription News</param>
        /// <returns>Returns Int32</returns>
        public Int32 Update(tb_NewsSubscription News)
        {
            int RowsAffected = 0;
            try
            {
                NewsSubscribtionDAC dac = new NewsSubscribtionDAC();
                RowsAffected = dac.Update(News);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return RowsAffected;
        }
        #endregion

        /// <summary>
        /// Function For Create News Subscription
        /// </summary>
        /// <param name="News">tb_NewsSubscription News</param>
        /// <returns>Returns Identity value for inserted record</returns>
        public Int32 CreateNewsSubscription(tb_NewsSubscription News)
        {
            Int32 isAdded = 0;
            try
            {
                NewsSubscribtionDAC ObjNews = new NewsSubscribtionDAC();
                ObjNews.Create(News);
                isAdded = News.NewsSubscriptionID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// For Delete News By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void delNews(int Id)
        {
            NewsSubscribtionDAC ObjNews = new NewsSubscribtionDAC();
            ObjNews.Delete(Id);
        }

        /// <summary>
        /// Get News Subscription List
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns IQueryable</returns>
        public IQueryable<NewsSubscribtionEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, int StoreID, string SearchValue)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                if (string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = "";
                }
                else
                {
                    SearchValue = SearchValue.Trim();
                }

                results = from a in ctx.tb_NewsSubscription
                          where a.Email.Contains(SearchValue)
                          select new NewsSubscribtionEntity
                          {
                              NewsSubscriptionID = a.NewsSubscriptionID,
                              Email = a.Email,
                              CreatedOn = a.CreatedOn.Value,
                              StoreName = a.tb_Store.StoreName,
                              StoreID = a.tb_Store.StoreID
                          };
                if (StoreID != -1)
                {
                    results = results.Where(a => a.StoreID == StoreID).AsQueryable();
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
                    results = results.OrderByDescending(o => o.CreatedOn);
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
        /// <param name="StoreID">int StoreID</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns No of count</returns>
        public int GetCount(int StoreID, string SearchValue)
        {
            return _count;
        }
    }

    /// <summary>
    /// Variable And Properties
    /// </summary>
    public class NewsSubscribtionEntity
    {
        private int _NewsSubscriptionID;
        private int _StoreID;
        private string _Email;
        private DateTime _CreatedOn;
        private string _StoreName;

        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }

        public int NewsSubscriptionID
        {
            get { return _NewsSubscriptionID; }
            set { _NewsSubscriptionID = value; }
        }
        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }
        public string Email
        {
            get { return _Email; }
            set { _Email = value; }
        }
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }

    }
}


