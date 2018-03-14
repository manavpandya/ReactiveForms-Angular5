using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using System.Collections;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// Coupon Data Access Class Contains Coupon related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class CouponDAC
    {
        #region Declaration

        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Method to Get Discount Price/Percentage by Coupon Code
        /// </summary>
        /// <param name="CouponCode">string CouponCode</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Discount Price/Percentage</returns>
        public DataSet GetDiscountByCouponCode(string CouponCode, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Coupons";
            cmd.Parameters.AddWithValue("@Opt", 1);
            cmd.Parameters.AddWithValue("@CouponCode", CouponCode);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Discount By Coupon Code By SQL Function
        /// </summary>
        /// <param name="CouponCode">String CouponCode</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Error message or discount amount</returns>
        public String GetDiscountByCouponCodeFunction(String CouponCode, Int32 CustomerID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "Select dbo.GetCouponDiscount ('" + CouponCode + "', " + CustomerID + ", " + StoreID + ")";
            String result = Convert.ToString(objSql.ExecuteScalarQuery(cmd));
            return result;
        }

        /// <summary>
        /// Get details of coupon
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns table tb_Coupons</returns>
        public tb_Coupons Getcoupon(int Id)
        {
            tb_Coupons coupon = null;
            {
                coupon = ctx.tb_Coupons.First(e => e.CouponID == Id);
            }
            return coupon;
        }

        /// <summary>
        /// Function to delete coupon
        /// </summary>
        /// <param name="credit">tb_Coupons coupon</param>
        /// <returns>Returns Int32</returns>
        public void Update(tb_Coupons coupon)
        {
            ctx.SaveChanges();
        }

        /// <summary>
        /// Function to create coupon
        /// </summary>
        /// <param name="tblcoupon">table detail of coupon</param>
        /// <returns>return table tb_Coupons</returns>
        public tb_Coupons Createcoupon(tb_Coupons tblcoupon)
        {
            try
            {
                ctx.AddTotb_Coupons(tblcoupon);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tblcoupon;
        }

        /// <summary>
        /// Check for Duplicate records 
        /// </summary>
        /// <param name="tblcoupon">table detail of coupon</param>
        /// <returns>Returns Int32</returns>
        public Int32 CheckDuplicate(tb_Coupons tblcoupon)
        {
            Int32 isExists = 0;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(tblcoupon.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var results = from a in ctx.tb_Coupons
                          select new
                          {
                              coupons = a,
                              store = a.tb_Store
                          };
            isExists = (from a in results
                        where a.coupons.CouponCode == tblcoupon.CouponCode
                        && a.coupons.Deleted == false
                        && a.coupons.CouponID != tblcoupon.CouponID
                        && a.store.StoreID == ID
                        select new { a.coupons.CouponID }).Count();
            return isExists;
        }

        /// <summary>
        /// Get the coupon usage by coupon code
        /// </summary>
        /// <param name="couponcode">string couponcode</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCouponUsagebyCouponCode(string couponcode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Coupons";
            cmd.Parameters.AddWithValue("@Opt", 2);
            cmd.Parameters.AddWithValue("@CouponCode", couponcode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        ///  Get the coupon usage by coupon code and store id
        /// </summary>
        /// <param name="couponcode">string couponcode</param>
        /// <param name="storeid">int32 storeid</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCouponUsage(string couponcode, Int32 storeid)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Coupons";
            cmd.Parameters.AddWithValue("@Opt", 3);
            cmd.Parameters.AddWithValue("@CouponCode", couponcode);
            cmd.Parameters.AddWithValue("@StoreID", storeid);
            return objSql.GetDs(cmd);
        }

        public tb_CouponUsage CreatecouponUsage(tb_CouponUsage tblCouponUsage)
        {
            try
            {
                ctx.AddTotb_CouponUsage(tblCouponUsage);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tblCouponUsage;
        }

        #endregion
    }
}
