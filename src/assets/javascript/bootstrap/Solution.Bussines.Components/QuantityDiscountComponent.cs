using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using System.Data.Objects;
using Solution.Data;
using System.Data;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;
using Solution.Bussines.Components;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// QuantityDiscountComponent -This Class Contains Quantity Discount related Business Logic Functions 
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class QuantityDiscountComponent
    {
        List<QuantityDiscountEntity> lsttaxcls = new List<QuantityDiscountEntity>();
        QuantityDiscountDAC ObjQauntityDiscount = new QuantityDiscountDAC();

        private static int _count;
        private static int _QuantityDiscountID = 0;
        public static int QuantityDiscountID
        {
            get { return _QuantityDiscountID; }
            set { _QuantityDiscountID = value; }
        }




        /// <summary>
        /// Get The Data Source for Grid view after sorting ,searching and On First Time Load
        /// </summary>
        /// <param name="startIndex">For Starting index of Grid View</param>
        /// <param name="pageSize">Page Size of Grid view</param>
        /// <param name="sortBy">Sort By For Sorting Grid view</param>
        /// <param name="CName">For Control Name</param>
        /// <param name="pStoreId">Searching Parameter Store Id For searching</param>
        /// <param name="pSearchValue">Value For parameter For Searching</param>
        /// <returns>return IQueryable Non generic Collection</returns>
        public IQueryable<QuantityDiscountEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            IQueryable<QuantityDiscountEntity> results = from a in ctx.tb_QauntityDiscount
                                                         select new QuantityDiscountEntity
                                                           {
                                                               QuantityDiscountID = a.QuantityDiscountID,
                                                               StoreID = a.tb_Store.StoreID,
                                                               StoreName = a.tb_Store.StoreName,
                                                               Name = a.Name,
                                                               CreatedOn = a.CreatedOn.Value,
                                                           };
            if (!string.IsNullOrEmpty(pSearchValue))
            {
                if (pStoreId != -1)
                {
                    results = results.Where(a => a.Name.Contains(pSearchValue) && a.StoreID == pStoreId).AsQueryable();
                }
                else
                {
                    results = results.Where(a => a.Name.Contains(pSearchValue)).AsQueryable();
                }
            }
            else
            {
                if (pStoreId != -1)
                {
                    results = results.Where(a => a.StoreID == pStoreId).AsQueryable();
                }
                else
                {

                }
            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lsttaxcls.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

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
                results = results.OrderBy(o => o.Name);
            }
            results = results.Skip(startIndex).Take(pageSize);
            return results;
        }

        /// <summary>
        /// For Checking Duplicate Quantity Discount Name at the time of Inserting/Adding new Tax class
        /// </summary>
        /// <param name="tblQauntityDiscount">Table Tax class</param>
        /// <returns>Return Boolean Value True/False</returns>
        public bool CheckDuplicate(tb_QauntityDiscount tblQauntityDiscount)
        {
            if (ObjQauntityDiscount.CheckDuplicate(tblQauntityDiscount) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// CreateQuantityDiscount For Inserting/Adding new Quantity Discount 
        /// </summary>
        /// <param name="tblQauntityDiscount">Table Quantity Discount</param>
        /// <returns>Return Id of the Created/Added Record</returns>
        public Int32 CreateQauntityDiscount(tb_QauntityDiscount tblQauntityDiscount)
        {
            Int32 isAdded = 0;
            try
            {
                ObjQauntityDiscount.CreateQauntityDiscount(tblQauntityDiscount);
                isAdded = tblQauntityDiscount.QuantityDiscountID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns Object</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// GetCount For Paging in Grid View
        /// </summary>
        /// <param name="CName"></param>
        /// <param name="pStoreId"></param>
        /// <param name="pSearchValue"></param>
        /// <returns>Returns No of Rows Count</returns>
        public static int GetCount(string CName, int pStoreId, string pSearchValue)
        {
            return _count;
        }

        /// <summary>
        /// For Updating Quantity Discount
        /// </summary>
        /// <param name="tblQauntityDiscount">Table Quantity Discount</param>
        /// <returns>Return int QauntityDiscountId Which is Updated  </returns>
        public Int32 UpdateQauntityDiscount(tb_QauntityDiscount tblQauntityDiscount)
        {
            Int32 isUpdated = 0;
            try
            {
                ObjQauntityDiscount.Update(tblQauntityDiscount);
                isUpdated = tblQauntityDiscount.QuantityDiscountID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Get Particular Record from Quantity Discount for QauntityDiscountID
        /// </summary>
        /// <param name="QauntityDiscountID">int QauntityDiscountID</param>
        /// <returns>Return Quantity Discount Table Object</returns>
        public tb_QauntityDiscount GetQauntityDiscount(int QauntityDiscountID)
        {
            return ObjQauntityDiscount.GetQauntityDiscount(QauntityDiscountID);
        }

        /// <summary>
        /// Get the Quantity discount list
        /// </summary>
        /// <param name="QauntityDiscountID">int QauntityDiscountID</param>
        /// <returns>Returns the quantity discount Table object</returns>
        public List<tb_QuantityDiscountTable> GetQauntityDiscountnew(int QauntityDiscountID)
        {
            return ObjQauntityDiscount.GetQauntityDiscountnew(QauntityDiscountID);
        }

        /// <summary>
        /// Update Quantity Discount Table
        /// </summary>
        /// <param name="QuantityDiscountTableID">int QuantityDiscountTableID</param>
        /// <param name="QuantityDiscountID">int QuantityDiscountID</param>
        /// <param name="LowQuantity">int LowQuantity</param>
        /// <param name="HighQuantity">int HighQuantity</param>
        /// <param name="DiscountPercent">decimal DiscountPercent</param>
        /// <returns>Returns the ID which is updated</returns>
        public int UpdateQuantityDiscountTable(int QuantityDiscountTableID, int QuantityDiscountID, int low, int high, decimal Discount)
        {
            int Id = 0;

            Id = ObjQauntityDiscount.UpdateQuantityDiscountTable(QuantityDiscountTableID, QuantityDiscountID, low, high, Discount);
            return Id;
        }

        /// <summary>
        /// Add Quantity Discount Table 
        /// </summary>
        /// <param name="QuantityDiscountID">int QuantityDiscountID</param>
        /// <param name="LowQuantity">int LowQuantity</param>
        /// <param name="HighQuantity">int HighQuantity</param>
        /// <param name="DiscountPercent">decimal DiscountPercent</param>
        /// <returns>Returns the Identity Value</returns>
        public int AddQuantityDiscountTable(int QuantityDiscountID, int low, int high, decimal Discount)
        {
            int Id = 0;

            Id = ObjQauntityDiscount.AddQuantityDiscountTable(QuantityDiscountID, low, high, Discount);
            return Id;
        }

        /// <summary>
        /// Delete Quantity Discount
        /// </summary>
        /// <param name="QuantityDiscountID">Int32 QuantityDiscountID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteQauntityDiscount(int QuantityDiscountID)
        {
            int Id = 0;

            Id = ObjQauntityDiscount.DeleteQauntityDiscount(QuantityDiscountID);
            return Id;
        }

        /// <summary>
        /// Delete Quantity Discount Table
        /// </summary>
        /// <param name="QuantityDiscountTableID">Int32 QuantityDiscountTableID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteQuantityDiscountTable(int QuantityDiscountID)
        {
            int Id = 0;

            Id = ObjQauntityDiscount.DeleteQuantityDiscountTable(QuantityDiscountID);
            return Id;
        }

        /// <summary>
        /// Check Duplicate for update
        /// </summary>
        /// <param name="tblQauntityDiscount">tb_QauntityDiscount tblQauntityDiscount</param>
        /// <param name="oldname">string oldname</param>
        /// <returns>Returns the row count if exists</returns>
        public bool CheckDuplicateforupdate(tb_QauntityDiscount tblQauntityDiscount, string oldname)
        {
            if (ObjQauntityDiscount.CheckDuplicateforupdate(tblQauntityDiscount, oldname) == 0)
            {
                return false;
            }
            else
                return true;
        }
    }

    public class QuantityDiscountEntity
    {
        private int _QuantityDiscountID;
        private int _CreatedBy;
        private string _StoreName;

        private string _Name;
        private int _StoreID;
        private int _UpdatedBy;
        private DateTime _CreatedOn;
        private DateTime _UpdatedOn;

        public int QuantityDiscountID
        {
            get { return _QuantityDiscountID; }
            set { _QuantityDiscountID = value; }
        }
        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }
        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public int UpdatedBy
        {
            get { return _UpdatedBy; }
            set { _UpdatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return _UpdatedOn; }
            set { _UpdatedOn = value; }
        }
    }
}
