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
    /// Gift Card Usage Component Class Contains Gift Card Usage Related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class GiftCardUsageComponent
    {
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        private static int _count;
        /// <summary>
        /// Get Gift Card Usage List
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetGiftCardUsageList(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue)
        {
            GiftCardUsageDAC objDAC = new GiftCardUsageDAC();
            DataSet ds = objDAC.GetGiftCardUsageList(pStoreId);
            _count = ds.Tables[0].Rows.Count;
            return ds;
        }

        /// <summary>
        /// Get Gift Card Usage Details
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <returns>Returns IQueryable</returns>
        public IQueryable<GiftCardUsageEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, int pStoreId)
        {
            IQueryable<GiftCardUsageEntity> results = from gu in ctx.tb_GiftCardUsage
                                                      join g in ctx.tb_GiftCard on gu.GiftCardID equals g.GiftCardID
                                                      join c in ctx.tb_Customer on gu.CustomerID equals c.CustomerID
                                                      join s in ctx.tb_Store on gu.StoreID equals s.StoreID

                                                      select new GiftCardUsageEntity
                                                      {
                                                          GiftCardUsageID = gu.GiftCardUsageID,
                                                          SerialNumber = g.SerialNumber,
                                                          Amount = gu.Amount.Value,
                                                          StoreName = s.StoreName,
                                                          CreatedOn = (DateTime?)gu.CreatedOn,
                                                          CustomerName = c.Email,
                                                          StoreID = gu.StoreID.Value,
                                                          OrderNumber = gu.OrderNumber.Value
                                                      };


            if (pStoreId != 0)
            {
                results = results.Where(a => a.StoreID == pStoreId).AsQueryable();
            }
            _count = results.Count();
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
                results = results.OrderBy(o => o.CreatedOn);
            }
            results = results.Skip(startIndex).Take(pageSize).AsQueryable();
            return results;
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns object</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Get No of Records
        /// </summary>
        /// <param name="pStoreId"></param>
        /// <returns>Returns Count of total No of Records</returns>
        public static int GetCount(int pStoreId)
        {
            return _count;
        }
    }

    /// <summary>
    /// Gift Card Usage
    /// </summary>
    public class GiftCardUsageEntity
    {
        private int _GiftCardUsageID;

        public int GiftCardUsageID
        {
            get { return _GiftCardUsageID; }
            set { _GiftCardUsageID = value; }
        }

        private string _SerialNumber;

        public string SerialNumber
        {
            get { return _SerialNumber; }
            set { _SerialNumber = value; }
        }

        private string _StoreName;

        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }

        private decimal _Amount;

        public decimal Amount
        {
            get { return _Amount; }
            set { _Amount = value; }
        }

        private DateTime? _CreatedOn;

        public DateTime? CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }

        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }

        private int _StoreID;

        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }

        private int _OrderNumber;

        public int OrderNumber
        {
            get { return _OrderNumber; }
            set { _OrderNumber = value; }
        }
    }
}
