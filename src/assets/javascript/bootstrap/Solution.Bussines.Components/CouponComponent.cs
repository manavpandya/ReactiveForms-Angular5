using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using Solution.Bussines.Entities;
using System.Data.Objects;
using System.Collections;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Coupon Component Class Contains Coupon Related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class CouponComponent
    {
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        List<CouponComponentEntity> lstcoupon = new List<CouponComponentEntity>();
        CouponDAC coupondac = new CouponDAC();
        private static int _count;
        public static bool _afterDelete = false;

        /// <summary>
        /// Method to Get Discount Price/Percentage by Coupon Code
        /// </summary>
        /// <param name="CouponCode">string CouponCode</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Discount Price/Percentage</returns>
        public static DataSet GetDiscountByCouponCode(string CouponCode, int StoreID)
        {
            CouponDAC dac = new CouponDAC();
            return dac.GetDiscountByCouponCode(CouponCode, StoreID);
        }

        /// <summary>
        /// Get Discount By Coupon Code By SQL Function
        /// </summary>
        /// <param name="CouponCode">String CouponCode</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Error message or discount amount</returns>
        public static String GetDiscountByCouponCodeFunction(String CouponCode, Int32 CustomerID, Int32 StoreID)
        {
            CouponDAC dac = new CouponDAC();
            return dac.GetDiscountByCouponCodeFunction(CouponCode, CustomerID, StoreID);
        }

        /// <summary>
        /// Function to provide data source to gridview on searching,sorting and on first time load
        /// </summary>
        /// <param name="startIndex">int startindex</param>
        /// <param name="pageSize">int Pagesize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string searchvalue</param>
        /// <returns>Returns IQueryable</returns>
        //public IQueryable<CouponComponentEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue)
        //{
        //    IQueryable<CouponComponentEntity> results = from a in ctx.tb_Coupons
        //                                                where a.Deleted.Value == false
        //                                                select new CouponComponentEntity
        //                                                {
        //                                                    CouponID = a.CouponID,
        //                                                    Couponcode = a.CouponCode,
        //                                                    DiscountAmount = a.DiscountAmount.Value,
        //                                                    DiscountPercent = a.DiscountPercent.Value,
        //                                                    Discription = a.Description,
        //                                                    ExpirationDate = a.ExpirationDate.Value,
        //                                                    ExpiredAfterNUses = a.ExpiredAfterNUses.Value,
        //                                                    ExpiresAfterOneUsageByEachCustomer = a.ExpiresAfterOneUsageByEachCustomer.Value,
        //                                                    ExpiresonFirstUseByAnyCustomer = a.ExpiresonFirstUseByAnyCustomer.Value,
        //                                                    ValidForCategory = a.ValidForCategory,
        //                                                    ValidforProduct = a.ValidforProduct,
        //                                                    ValidforCustomer = a.ValidforCustomer,
        //                                                    RequiredMinimumOrderTotal = a.RequiredMinimumOrderTotal.Value,
        //                                                    StoreName = a.tb_Store.StoreName,
        //                                                    StoreID = a.tb_Store.StoreID,
        //                                                };
        //    if (pSearchValue != null)
        //    {
        //        if (pStoreId != 0)
        //        {
        //            results = results.Where(a => a.Couponcode.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId
        //                ).AsQueryable();
        //        }
        //        else if (pStoreId == 0)
        //        {
        //            results = results.Where(a => a.Couponcode.Contains(pSearchValue.Trim())).AsQueryable();
        //        }
        //    }
        //    else
        //    {
        //        pSearchValue = "";
        //        if (pStoreId != 0)
        //        {
        //            results = results.Where(a => a.Couponcode.Contains(pSearchValue) && a.StoreID == pStoreId).AsQueryable();
        //        }
        //        else if (pStoreId == 0)
        //        {
        //            results = results.OrderBy(o => o.Couponcode);
        //        }
        //    }
        //    _count = results.Count();
        //    if (!string.IsNullOrEmpty(sortBy))
        //    {
        //        System.Reflection.PropertyInfo property = lstcoupon.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
        //        String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //        if (SortingOption.Length == 1)
        //        {
        //            results = results.OrderByField(SortingOption[0].ToString(), true);
        //        }
        //        else if (SortingOption.Length == 2)
        //        {
        //            results = results.OrderByField(SortingOption[0].ToString(), false);
        //        }
        //    }
        //    else
        //    {
        //        results = results.OrderBy(o => o.Couponcode);
        //    }
        //    results = results.Skip(startIndex).Take(pageSize).AsQueryable();
        //    return results;
        //}



        public IQueryable<CouponComponentEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue, int ploginid)
        {
            IQueryable<CouponComponentEntity> results = from a in ctx.tb_Coupons
                                                        where a.Deleted.Value == false
                                                        select new CouponComponentEntity
                                                        {
                                                            CouponID = a.CouponID,
                                                            Couponcode = a.CouponCode,
                                                            DiscountAmount = a.DiscountAmount.Value,
                                                            DiscountPercent = a.DiscountPercent.Value,
                                                            Discription = a.Description,
                                                            ExpirationDate = a.ExpirationDate.Value,
                                                            ExpiredAfterNUses = a.ExpiredAfterNUses.Value,
                                                            ExpiresAfterOneUsageByEachCustomer = a.ExpiresAfterOneUsageByEachCustomer.Value,
                                                            ExpiresonFirstUseByAnyCustomer = a.ExpiresonFirstUseByAnyCustomer.Value,
                                                            ValidForCategory = a.ValidForCategory,
                                                            ValidforProduct = a.ValidforProduct,
                                                            ValidforCustomer = a.ValidforCustomer,
                                                            RequiredMinimumOrderTotal = a.RequiredMinimumOrderTotal.Value,
                                                            StoreName = a.tb_Store.StoreName,
                                                            StoreID = a.tb_Store.StoreID,
                                                            CreatedBy = a.CreatedBy.Value
                                                        };
            if (pSearchValue != null)
            {
                if (pStoreId != 0)
                {
                    if (ploginid != 0)
                    {
                        results = results.Where(a => a.Couponcode.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId && a.CreatedBy == ploginid
                            ).AsQueryable();
                    }
                    else
                    {
                        results = results.Where(a => a.Couponcode.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId
                               ).AsQueryable();

                    }
                }
                else if (pStoreId == 0)
                {
                    if (ploginid != 0)
                    {
                        results = results.Where(a => a.Couponcode.Contains(pSearchValue.Trim()) && a.CreatedBy == ploginid).AsQueryable();
                    }
                    else
                    {
                        results = results.Where(a => a.Couponcode.Contains(pSearchValue.Trim())).AsQueryable();
                    }
                }



            }
            else
            {
                pSearchValue = "";
                if (pStoreId != 0)
                {
                    if (ploginid != 0)
                    {
                        results = results.Where(a => a.Couponcode.Contains(pSearchValue) && a.StoreID == pStoreId && a.CreatedBy == ploginid).AsQueryable();
                    }
                    else
                    {
                        results = results.Where(a => a.Couponcode.Contains(pSearchValue) && a.StoreID == pStoreId).AsQueryable();
                    }
                }
                else if (pStoreId == 0)
                {
                    if (ploginid != 0)
                    {
                        results = results.Where(a => a.CreatedBy == ploginid).AsQueryable().OrderBy(o => o.Couponcode);
                    }
                    else
                    {
                        results = results.OrderBy(o => o.Couponcode);
                    }
                }


            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lstcoupon.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
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
                results = results.OrderBy(o => o.Couponcode);
            }
            results = results.Skip(startIndex).Take(pageSize).AsQueryable();
            return results;
        }





        /// <summary>
        ///Get PropertyValue Function 
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
        /// GetCount Function gets the count of tb_Coupons
        /// </summary>
        /// <param name="CName">Counting variable</param>
        /// <param name="pStoreId">Store ID</param>
        /// <param name="pSearchValue">Search Value</param>
        /// <returns>Total count</returns>
        public static int GetCount(string CName, int pStoreId, string pSearchValue, int ploginid)
        {
            return _count;
        }

        /// <summary>
        /// Get details of coupon
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns table tb_Coupons</returns>
        public tb_Coupons Getcoupon(int Id)
        {
            return coupondac.Getcoupon(Id);
        }

        /// <summary>
        /// Function to delete coupon
        /// </summary>
        /// <param name="coupon">tb_Coupons coupon</param>
        /// <returns>ID of deleted Coupon</returns>
        public Int32 Deletecoupon(tb_Coupons coupon)
        {
            Int32 isUpdated = 0;
            try
            {
                coupondac.Update(coupon);
                isUpdated = coupon.CouponID;
                _afterDelete = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Function to Update Coupon details
        /// </summary>
        /// <param name="tblcoupon">tb_Coupons tblcoupon</param>
        /// <returns>ID of Updated Coupon</returns>
        public Int32 Updatecoupon(tb_Coupons tblcoupon)
        {
            Int32 isUpdated = 0;
            try
            {
                coupondac.Update(tblcoupon);
                isUpdated = tblcoupon.CouponID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Check for Duplicate records 
        /// </summary>
        /// <param name="tblcoupon">tb_Coupons tblcoupon</param>
        /// <returns>No of Duplicate Records</returns>
        public bool CheckDuplicate(tb_Coupons tblcoupon)
        {
            if (coupondac.CheckDuplicate(tblcoupon) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Function to Create Coupon
        /// </summary>
        /// <param name="tblcoupon">tb_Coupons tblcoupon</param>
        /// <returns>Identity Value of inserted Coupon</returns>
        public Int32 Createcoupon(tb_Coupons tblcoupon)
        {
            Int32 isAdded = 0;
            try
            {
                coupondac.Createcoupon(tblcoupon);
                isAdded = tblcoupon.CouponID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Get the Coupon Usage by Coupon Code
        /// </summary>
        /// <param name="couponcode">string couponcode</param>
        /// <returns>Coupon Usage Dataset</returns>
        public DataSet GetCouponUsagebyCouponCode(string couponcode)
        {
            return coupondac.GetCouponUsagebyCouponCode(couponcode);
        }

        /// <summary>
        ///  Get the Coupon Usage by Coupon Code and Store Id
        /// </summary>
        /// <param name="couponcode">string couponcode</param>
        /// <param name="storeid">int32 storeid</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCouponUsage(string couponcode, Int32 storeid)
        {
            return coupondac.GetCouponUsage(couponcode, storeid);
        }

        public Int32 CreatecouponUsage(tb_CouponUsage tblCouponUsage)
        {
            Int32 isAdded = 0;
            try
            {
                coupondac.CreatecouponUsage(tblCouponUsage);
                isAdded = tblCouponUsage.CouponUsageID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

    }
    public class CouponComponentEntity
    {
        private int _CouponID;

        public int CouponID
        {
            get { return _CouponID; }
            set { _CouponID = value; }
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

        private string _Couponcode;

        public string Couponcode
        {
            get { return _Couponcode; }
            set { _Couponcode = value; }
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

        public DateTime _ExpirationDate;

        public DateTime ExpirationDate
        {
            get { return _ExpirationDate; }
            set { _ExpirationDate = value; }
        }

        public bool _Deleted;

        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }

        public bool _ExpiresonFirstUseByAnyCustomer;

        public bool ExpiresonFirstUseByAnyCustomer
        {
            get { return _ExpiresonFirstUseByAnyCustomer; }
            set { _ExpiresonFirstUseByAnyCustomer = value; }
        }

        public bool _ExpiresAfterOneUsageByEachCustomer;

        public bool ExpiresAfterOneUsageByEachCustomer
        {
            get { return _ExpiresAfterOneUsageByEachCustomer; }
            set { _ExpiresAfterOneUsageByEachCustomer = value; }
        }

        public int _ExpiredAfterNUses;

        public int ExpiredAfterNUses
        {
            get { return _ExpiredAfterNUses; }
            set { _ExpiredAfterNUses = value; }
        }

        public Decimal _OrderTotal;

        public Decimal OrderTotal
        {
            get { return _OrderTotal; }
            set { _OrderTotal = value; }
        }

        public Decimal _DiscountPercent;

        public Decimal DiscountPercent
        {
            get { return _DiscountPercent; }
            set { _DiscountPercent = value; }
        }

        public Decimal _DiscountAmount;

        public Decimal DiscountAmount
        {
            get { return _DiscountAmount; }
            set { _DiscountAmount = value; }
        }

        public bool _DiscountAllowIncludeFreeShipping;

        public bool DiscountAllowIncludeFreeShipping
        {
            get { return _DiscountAllowIncludeFreeShipping; }
            set { _DiscountAllowIncludeFreeShipping = value; }
        }

        public Decimal _RequiredMinimumOrderTotal;

        public Decimal RequiredMinimumOrderTotal
        {
            get { return _RequiredMinimumOrderTotal; }
            set { _RequiredMinimumOrderTotal = value; }
        }

        public String _ValidforCustomer;

        public String ValidforCustomer
        {
            get { return _ValidforCustomer; }
            set { _ValidforCustomer = value; }
        }

        public String _ValidforProduct;

        public String ValidforProduct
        {
            get { return _ValidforProduct; }
            set { _ValidforProduct = value; }
        }

        public String _ValidForCategory;

        public String ValidForCategory
        {
            get { return _ValidForCategory; }
            set { _ValidForCategory = value; }
        }

        private String _Expires;

        public String Expires
        {
            get
            {
                if (ExpiresonFirstUseByAnyCustomer)
                {
                    return _Expires = "On First Use By Any Customer";
                }
                else
                {
                    if (ExpiresAfterOneUsageByEachCustomer)
                    {
                        return _Expires = "After One Usage By Each Customer";
                    }
                    else
                    {
                        return _Expires = "After " + ExpiredAfterNUses.ToString() + " Uses";
                    }
                }
            }
            set
            {
                if (ExpiresonFirstUseByAnyCustomer)
                {
                    _Expires = "";
                }
                else
                {
                    if (ExpiresAfterOneUsageByEachCustomer)
                    {
                        _Expires = "After One Usage By Each Customer";
                    }
                    else
                    {
                        _Expires = "After " + ExpiredAfterNUses.ToString() + " Uses";
                    }
                }
            }

        }
    }
}
