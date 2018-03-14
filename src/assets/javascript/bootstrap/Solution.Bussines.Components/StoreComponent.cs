using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Reflection;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Store Component Class Contains Store related Business Logic function.    
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class StoreComponent
    {
        #region Variables and property
        private static int _count;
        StoreDAC dacStore = new StoreDAC();
        private static DataSet DSCommon = null;

        #endregion

        #region Key Function

        /// <summary>
        /// Method to fetch data based on page size and filtering
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the Store List for display in gridview</returns>
        public static IQueryable<StoreConfigurationEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string SearchValue)
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
            IQueryable<StoreConfigurationEntity> result = (from storeList in ctx.tb_Store
                                                           where storeList.StoreName.Contains(SearchValue) &&
                                                           storeList.Deleted == false
                                                           select new StoreConfigurationEntity
                                                           {
                                                               StoreId = storeList.StoreID,
                                                               StoreName = storeList.StoreName
                                                           });
            _count = result.Count();

            //Logic for searching
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = result.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    result = result.OrderByField(SortingOption[0].ToString(), true);
                }
                else if (SortingOption.Length == 2)
                {
                    result = result.OrderByField(SortingOption[0].ToString(), false);
                }
            }
            else
            {
                //Default sorting by Store Name
                result = result.OrderBy(o => o.StoreName);
            }
            result = result.Skip(startIndex).Take(pageSize);
            return result;
        }

        /// <summary>
        /// Get Property of class from string name
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns the Object</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// To count total number of record that are used for paging
        /// </summary>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the Record count</returns>
        public static int GetCount(string SearchValue)
        {
            return _count;
        }

        /// <summary>
        /// Insert new Store config 
        /// </summary>
        /// <param name="store">tb_Store store</param>
        /// <returns>Returns Identity Value of inserted record</returns>
        public Int32 CreateStoreConfiguration(tb_Store store)
        {
            Int32 isAdded = 0;
            try
            {
                StoreDAC dac = new StoreDAC();
                dac.Create(store);
                isAdded = store.StoreID;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Check whether duplicate record is inserted or not
        /// </summary>
        /// <param name="store">tb_Store store</param>
        /// <returns>Returns Value - if value >0 , store with same name already exists</returns>
        public bool CheckDuplicate(tb_Store store)
        {
            StoreDAC dac = new StoreDAC();
            if (dac.CheckDuplicate(store) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Pop up selected Store detail for edit mode
        /// </summary>
        /// <param name="ID">int ID</param>
        /// <returns>Returns the Store Data by storeID</returns>
        public tb_Store getStore(int ID)
        {
            StoreDAC dac = new StoreDAC();
            return dac.GetStore(ID);
        }

        /// <summary>
        /// Update store config detail
        /// </summary>
        /// <param name="store">tb_Store store</param>
        /// <returns>Returns Count of Rows affected</returns>
        public Int32 UpdateStoreConfiguration(tb_Store store)
        {
            int RowsAffected = 0;
            try
            {
                StoreDAC dac = new StoreDAC();
                {
                    RowsAffected = dac.Update(store);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }

        /// <summary>
        /// Get Store List
        /// </summary>
        /// <returns>Returns tb_Store Store List</returns>
        public List<tb_Store> GetStore()
        {
            return dacStore.SelectStore();
        }

        /// <summary>
        /// Get Store List
        /// </summary>
        /// <returns>Returns the List of Stores as a Dataset</returns>
        public static DataSet GetStoreList()
        {
            DSCommon = new DataSet();
            DSCommon = StoreDAC.GetStoreList();
            return DSCommon;
        }

        /// <summary>
        /// Get Store List By Menu
        /// </summary>
        /// <returns>Returns the Stores List as a Dataset</returns>
        public static DataSet GetStoreListByMenu()
        {
            DSCommon = new DataSet();
            StoreDAC dac = new StoreDAC();
            DSCommon = dac.GetStoreListByMenu();
            return DSCommon;
        }

        /// <summary>
        /// Get the Yahoo Store List
        /// </summary>
        /// <returns>Returns the Yahoo Store list as a Dataset</returns>
        public static DataSet GetYahooStoreList()
        {
            DSCommon = new DataSet();
            DSCommon = StoreDAC.GetYahooStoreList();
            return DSCommon;
        }
        #endregion
    }

    public class StoreConfigurationEntity
    {
        private Int32 _StoreId;
        private Int32 _ParentStoreId;
        private string _StoreName;
        public Int32 StoreId
        {
            get { return _StoreId; }
            set { _StoreId = value; }
        }
        public Int32 ParentStoreId
        {
            get { return _ParentStoreId; }
            set { _ParentStoreId = value; }
        }
        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }
    }
}
