using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.Data
{
    public class CustomerLevelDAC
    {
        /// <summary>
        /// CustomerLevel Data Access Class Contains Customer Level related Data Logic function     
        /// <author>
        /// Kaushalam Team © Kaushalam Inc. 2012.
        /// </author>
        /// Version 1.0
        /// </summary>

        #region Create New Customer Level
        /// <summary>
        /// Add new Customer Level
        /// </summary>
        /// <param name="custLevel">tb_CustomerLevel custLevel</param>
        /// <returns>Returns Id of inserted customer level</returns>
        public tb_CustomerLevel Create(tb_CustomerLevel custLevel)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_CustomerLevel(custLevel);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return custLevel;
        }
        #endregion

        #region Get Customer Level

        /// <summary>
        /// Get Customer Level  detail for Edit mode
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns tb_CustomerLevel List</returns>
        public tb_CustomerLevel GetCustomerLevel(int custLevelID)
        {
            tb_CustomerLevel custLevel = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                custLevel = ctx.tb_CustomerLevel.First(e => e.CustomerLevelID == custLevelID);
            }
            return custLevel;
        }
        #endregion

        #region Update Customer Level
        /// <summary>
        /// Delete Customer Level
        /// </summary>
        /// <param name="custLevel">tb_CustomerLevel custLevel</param>
        /// <returns>Returns Deleted Customer Level Value</returns>
        public void Update(tb_CustomerLevel custLevel)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;//)

            ctx.SaveChanges();
        }
        #endregion

        /// <summary>
        /// Get All Customer levels 
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns List of All Customer levels</returns>
        public List<tb_CustomerLevel> GetAllCustomerLevel(Int32 StoreID)
        {
            List<tb_CustomerLevel> tb_CustomerLevel = new List<tb_CustomerLevel>();
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_CustomerLevel = ctx.tb_CustomerLevel.OrderBy(cl => cl.Name).Where(c => c.Deleted == false && c.tb_Store.StoreID == StoreID).ToList();
            return tb_CustomerLevel;
        }

        #region Check Duplicate
        /// <summary>
        /// Check Customer Level is already inserted or not
        /// </summary>
        /// <param name="custLevel">tb_CustomerLevel custLevel</param>
        /// <returns>Returns Duplicate Record</returns>
        public int CheckDuplicate(tb_CustomerLevel custLevel)
        {
            Int32 isExists = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(custLevel.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var result = (from a in ctx.tb_CustomerLevel
                          where a.Deleted.Value == false
                          select new
                          {
                              CustLevel = a,
                              store = a.tb_Store
                          });
            isExists = (from a in result
                        where a.CustLevel.Name == custLevel.Name && a.store.StoreID == ID
                        select new { a.CustLevel.CustomerLevelID }
                            ).Count();
            return isExists;
        }
        #endregion
    }
}
