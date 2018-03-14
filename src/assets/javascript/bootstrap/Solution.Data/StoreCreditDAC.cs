using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Data;

namespace Solution.Data
{
   public class StoreCreditDAC
    {
       RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        /// <summary>
        /// Get all values for specific StoreCredit type
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns the List of StoreCredit Type</returns>
       public tb_StoreCredit getStoreCreditType(int Id)
        {
            tb_StoreCredit StoreCredit = null;
            {
                StoreCredit = ctx.tb_StoreCredit.First(e => e.ID == Id);
            }
            return StoreCredit;
        }


        /// <summary>
        /// Update StoreCredit Data
        /// </summary>
        /// <param name="pay">tb_StoreCredit pay</param>
        public void Update(tb_StoreCredit pay)
        {
            ctx.SaveChanges();
        }


        /// <summary>
        /// Function for checking duplicate StoreCredit type
        /// </summary>
        /// <param name="tblpay">tb_StoreCredit tblpay</param>
        /// <returns>Returns Row Count if exist</returns>
        public Int32 CheckDuplicate(tb_StoreCredit tblpay)
        {
            Int32 isExists = 0;

            var results = from a in ctx.tb_StoreCredit
                          select new
                          {
                              StoreCredit = a,
                             
                          };
            isExists = (from a in results
                        where a.StoreCredit.StoreCredit == tblpay.StoreCredit
                        && a.StoreCredit.Deleted == false
                        && a.StoreCredit.ID != tblpay.ID
                       
                        select new { a.StoreCredit.ID }).Count();
            return isExists;
        }

        /// <summary>
        /// Create StoreCredit type
        /// </summary>
        /// <param name="tblStoreCredit">tb_StoreCredit tblStoreCredit</param>
        /// <returns>Returns tb_StoreCredit Table Object</returns>
        public tb_StoreCredit CreateStoreCredittype(tb_StoreCredit tblStoreCredit)
        {
            try
            {
                ctx.AddTotb_StoreCredit(tblStoreCredit);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tblStoreCredit;
        }

    }
}
