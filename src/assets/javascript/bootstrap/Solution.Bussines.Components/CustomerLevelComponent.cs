using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using Solution.Data;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Customer Level Component Class Contains Customer Level Related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class CustomerLevelComponent
    {
        #region Declaration
        int _count = 0;
        #endregion

        #region Key Function

        /// <summary>
        /// Get Data to fill grid
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Return Customer Level Details</returns>
        public List<CustomerLevelEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, int pStoreId, string pSearchValue)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                List<CustomerLevelEntity> lstCustomerLevel = new List<CustomerLevelEntity>();
                var results = from a in ctx.tb_CustomerLevel
                              where a.Deleted.Value == false
                              select new
                              {
                                  CustLevel = a,
                                  store = a.tb_Store
                              };
                _count = results.Count();
                if (string.IsNullOrEmpty(pSearchValue))
                {
                    pSearchValue = "";
                }
                else
                {
                    pSearchValue = pSearchValue.Trim();
                }
                if (pStoreId != -1)
                {
                    var results1 = from a in results
                                   where a.CustLevel.Name.Contains(pSearchValue) &&
                                   a.store.StoreID == pStoreId
                                   select new CustomerLevelEntity
                                   {
                                       CustomerLevelID = a.CustLevel.CustomerLevelID,
                                       Name = a.CustLevel.Name,
                                       LevelDiscountAmount = a.CustLevel.LevelDiscountAmount.Value,
                                       LevelDiscountPercent = a.CustLevel.LevelDiscountPercent.Value,
                                       LevelHasFreeShipping = a.CustLevel.LevelHasFreeShipping.Value,
                                       LevelHasnoTax = a.CustLevel.LevelHasnoTax.Value,
                                       DisplayOrder = a.CustLevel.DisplayOrder.Value,
                                       StoreName = a.store.StoreName
                                   };
                    lstCustomerLevel = results1.ToList();
                }
                else
                {
                    var results1 = from a in results
                                   where a.CustLevel.Name.Contains(pSearchValue)
                                   select new CustomerLevelEntity
                                   {
                                       CustomerLevelID = a.CustLevel.CustomerLevelID,
                                       Name = a.CustLevel.Name,
                                       LevelDiscountAmount = a.CustLevel.LevelDiscountAmount.Value,
                                       LevelDiscountPercent = a.CustLevel.LevelDiscountPercent.Value,
                                       LevelHasFreeShipping = a.CustLevel.LevelHasFreeShipping.Value,
                                       LevelHasnoTax = a.CustLevel.LevelHasnoTax.Value,
                                       DisplayOrder = a.CustLevel.DisplayOrder.Value,
                                       StoreName = a.store.StoreName
                                   };
                    lstCustomerLevel = results1.ToList();
                }

                //Logic for searching
                if (!string.IsNullOrEmpty(sortBy))
                {

                    System.Reflection.PropertyInfo property = lstCustomerLevel.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

                    String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (SortingOption.Length == 1)
                    {

                        lstCustomerLevel = lstCustomerLevel.OrderBy(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<CustomerLevelEntity>();
                    }
                    else if (SortingOption.Length == 2)
                    {

                        lstCustomerLevel = lstCustomerLevel.OrderByDescending(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<CustomerLevelEntity>();
                    }
                }
                else
                {
                    lstCustomerLevel = lstCustomerLevel.OrderBy(o => o.Name).ToList();
                }
                lstCustomerLevel = lstCustomerLevel.Skip(startIndex).Take(pageSize).ToList();
                return lstCustomerLevel;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        /// <summary>
        /// Get Property name from string value
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns Property Value</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Get total no of records used for paging
        /// </summary>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns Total Records for Paging</returns>
        public int GetCount(int pStoreId, string pSearchValue)
        {
            return _count;
        }

        /// <summary>
        /// Add new Customer Level
        /// </summary>
        /// <param name="custLevel">tb_CustomerLevel custLevel</param>
        /// <returns>Returns Id of inserted customer level</returns>
        public Int32 CreateCustomerLevel(tb_CustomerLevel custLevel)
        {
            Int32 isAdded = 0;
            try
            {
                CustomerLevelDAC ObjAppDAC = new CustomerLevelDAC();
                ObjAppDAC.Create(custLevel);
                isAdded = custLevel.CustomerLevelID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Get Customer Level  detail for Edit mode
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns tb_CustomerLevel List</returns>
        public tb_CustomerLevel getCustLevel(int Id)
        {
            CustomerLevelDAC objAppDAC = new CustomerLevelDAC();
            return objAppDAC.GetCustomerLevel(Id);
        }

        /// <summary>
        /// Check Customer Level is already inserted or not
        /// </summary>
        /// <param name="custLevel">tb_CustomerLevel custLevel</param>
        /// <returns>Returns Duplicate Record</returns>
        public bool CheckDuplicate(tb_CustomerLevel custLevel)
        {
            CustomerLevelDAC ObjAppDAC = new CustomerLevelDAC();
            if (ObjAppDAC.CheckDuplicate(custLevel) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Update Customer level detail
        /// </summary>
        /// <param name="custLevel">tb_CustomerLevel custLevel</param>
        /// <returns>Returns Id of updated customer level</returns>
        public Int32 UpdateCustomerLevel(tb_CustomerLevel custLevel)
        {
            Int32 isUpdated = 0;
            try
            {
                CustomerLevelDAC ObjappDAC = new CustomerLevelDAC();
                ObjappDAC.Update(custLevel);
                isUpdated = custLevel.CustomerLevelID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }

        /// <summary>
        /// Delete Customer Level
        /// </summary>
        /// <param name="custLevel">tb_CustomerLevel custLevel</param>
        /// <returns>Returns Deleted Customer Level Value</returns>
        public Int32 DeleteCustomerLevel(tb_CustomerLevel custLevel)
        {
            Int32 isUpdated = 0;
            try
            {
                CustomerLevelDAC ObjappDAC = new CustomerLevelDAC();
                ObjappDAC.Update(custLevel);
                isUpdated = custLevel.CustomerLevelID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }

        /// <summary>
        /// Get All Customer levels 
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns List of All Customer levels</returns>
        public List<tb_CustomerLevel> GetAllCustomerLevel(Int32 StoreID)
        {
            CustomerLevelDAC objcuslevelDAC = new CustomerLevelDAC();
            return objcuslevelDAC.GetAllCustomerLevel(StoreID);
        }
    }
        #endregion

    /// <summary>
    /// Properties
    /// </summary>
    public class CustomerLevelEntity
    {
        private int _CustomerLevelID;

        public int CustomerLevelID
        {
            get { return _CustomerLevelID; }
            set { _CustomerLevelID = value; }
        }

        private int _StoreID;

        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }

        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }

        private decimal _LevelDiscountAmount;

        public decimal LevelDiscountAmount
        {
            get { return _LevelDiscountAmount; }
            set { _LevelDiscountAmount = value; }
        }

        private decimal _LevelDiscountPercent;

        public decimal LevelDiscountPercent
        {
            get { return _LevelDiscountPercent; }
            set { _LevelDiscountPercent = value; }
        }

        private bool _LevelHasFreeShipping;

        public bool LevelHasFreeShipping
        {
            get { return _LevelHasFreeShipping; }
            set { _LevelHasFreeShipping = value; }
        }

        private bool _LevelHasnoTax;

        public bool LevelHasnoTax
        {
            get { return _LevelHasnoTax; }
            set { _LevelHasnoTax = value; }
        }

        private int _DisplayOrder;

        public int DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }

        private string _StoreName;

        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }


    }
}


