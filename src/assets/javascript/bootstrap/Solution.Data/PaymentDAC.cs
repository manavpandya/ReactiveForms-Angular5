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
    /// <summary>
    /// Payment Data Access Class Contains Payment related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class PaymentDAC
    {
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

        /// <summary>
        /// Create payment type
        /// </summary>
        /// <param name="tblpayment">tb_Payment tblpayment</param>
        /// <returns>Returns tb_Payment Table Object</returns>
        public tb_Payment Createpaymenttype(tb_Payment tblpayment)
        {
            try
            {
                ctx.AddTotb_Payment(tblpayment);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tblpayment;
        }

        /// <summary>
        /// Update Payment Data
        /// </summary>
        /// <param name="pay">tb_Payment pay</param>
        public void Update(tb_Payment pay)
        {
            ctx.SaveChanges();
        }

        /// <summary>
        /// Get all values for specific payment type
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns the List of Payment Type</returns>
        public tb_Payment getPaymentType(int Id)
        {
            tb_Payment payment = null;
            {
                payment = ctx.tb_Payment.First(e => e.PaymentID == Id);
            }
            return payment;
        }

        /// <summary>
        /// Function for checking duplicate payment type
        /// </summary>
        /// <param name="tblpay">tb_Payment tblpay</param>
        /// <returns>Returns Row Count if exist</returns>
        public Int32 CheckDuplicate(tb_Payment tblpay)
        {
            Int32 isExists = 0;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(tblpay.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var results = from a in ctx.tb_Payment
                          select new
                          {
                              payment = a,
                              store = a.tb_Store
                          };
            isExists = (from a in results
                        where a.payment.PaymentType == tblpay.PaymentType
                        && a.payment.Deleted == false
                        && a.payment.PaymentID != tblpay.PaymentID
                        && a.store.StoreID == ID
                        select new { a.payment.PaymentID }).Count();
            return isExists;
        }


    }
}
