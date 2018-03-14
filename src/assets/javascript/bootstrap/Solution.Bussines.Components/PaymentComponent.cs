using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using System.Collections;
using Solution.Bussines.Entities;
using System.Data.Objects;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Payment Component Class Contains Payment related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class PaymentComponent
    {
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        List<PaymentComponentEntity> lstpay = new List<PaymentComponentEntity>();
        PaymentDAC paydac = new PaymentDAC();
        private static int _count;
        public static bool _afterDelete = false;

        /// <summary>
        /// Get filtered Data of Payment
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns the LIst of Payment Data</returns>
        public IQueryable<PaymentComponentEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue)
        {
            IQueryable<PaymentComponentEntity> results = from a in ctx.tb_Payment
                                                         where a.Deleted.Value == false
                                                         select new PaymentComponentEntity
                                                         {
                                                             PaymentID = a.PaymentID,
                                                             PaymentType = a.PaymentType,
                                                             Active = a.Active.Value,
                                                             StoreID = a.tb_Store.StoreID,
                                                             StoreName = a.tb_Store.StoreName
                                                         };
            if (pSearchValue != null)
            {
                if (pStoreId != 0)
                {
                    results = results.Where(a => a.PaymentType.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pStoreId == 0)
                {
                    results = results.Where(a => a.PaymentType.Contains(pSearchValue.Trim())).AsQueryable();
                }
            }
            else
            {
                pSearchValue = "";
                if (pStoreId != 0)
                {
                    results = results.Where(a => a.PaymentType.Contains(pSearchValue) && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pStoreId == 0)
                {
                    results = results.OrderBy(o => o.PaymentType);
                }
            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lstpay.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
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
                results = results.OrderBy(o => o.PaymentType);
            }
            results = results.Skip(startIndex).Take(pageSize).AsQueryable();
            return results;
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns the object</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Get the Count for GetDataByFilter() function
        /// </summary>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns row count</returns>
        public static int GetCount(string CName, int pStoreId, string pSearchValue)
        {
            return _count;
        }

        /// <summary>
        /// Get all values for specific payment type
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns the List of Payment Type</returns>
        public tb_Payment getPaymentType(int Id)
        {
            return paydac.getPaymentType(Id);
        }

        /// <summary>
        /// Function for deleting payment type
        /// </summary>
        /// <param name="pay">detail of payment type</param>
        /// <returns>Returns integer</returns>
        public Int32 Deletepaymenttype(tb_Payment pay)
        {
            Int32 isUpdated = 0;
            try
            {
                paydac.Update(pay);
                isUpdated = pay.PaymentID;
                _afterDelete = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Create payment type
        /// </summary>
        /// <param name="tblpay">tb_Payment tblpay</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 Createpaymenttype(tb_Payment tblpay)
        {
            Int32 isAdded = 0;
            try
            {
                paydac.Createpaymenttype(tblpay);
                isAdded = tblpay.PaymentID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Update Payment Data
        /// </summary>
        /// <param name="tblpay">tb_Payment tblpay</param>
        /// <returns>Returns the Updated PaymentID</returns>
        public Int32 Updatepaymenttype(tb_Payment tblpay)
        {
            Int32 isUpdated = 0;
            try
            {
                paydac.Update(tblpay);
                isUpdated = tblpay.PaymentID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Function for checking duplicate payment type
        /// </summary>
        /// <param name="tblpay">tb_Payment tblpay</param>
        /// <returns>Returns true if exist</returns>
        public bool CheckDuplicate(tb_Payment tblpay)
        {
            if (paydac.CheckDuplicate(tblpay) == 0)
            {
                return false;
            }
            else
                return true;
        }
    }
    public class PaymentComponentEntity
    {
        private int _PaymentID;

        public int PaymentID
        {
            get { return _PaymentID; }
            set { _PaymentID = value; }
        }

        private int _StoreID;

        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }

        private string _StoreName;

        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }

        private string _PaymentType;

        public string PaymentType
        {
            get { return _PaymentType; }
            set { _PaymentType = value; }
        }

        private string _Discription;

        public string Discription
        {
            get { return _Discription; }
            set { _Discription = value; }
        }

        private int _CreatedBy;

        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        private int _UpdatedBy;

        public int UpdatedBy
        {
            get { return _UpdatedBy; }
            set { _UpdatedBy = value; }
        }

        private DateTime _CreatedOn;

        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }

        private DateTime _UpdatedOn;

        public DateTime UpdatedOn
        {
            get { return _UpdatedOn; }
            set { _UpdatedOn = value; }
        }

        public bool _Active;

        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        public bool _Deleted;

        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }

    }
}
