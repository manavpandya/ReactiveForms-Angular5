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
   public class StoreCreditComponent
    {
       StoreCreditDAC paydac = new StoreCreditDAC();
        /// <summary>
        /// Get all values for specific StoreCredit type
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns the List of StoreCredit Type</returns>
       public tb_StoreCredit getStoreCreditType(int Id)
        {
            return paydac.getStoreCreditType(Id);
        }


        /// <summary>
        /// Update StoreCredit Data
        /// </summary>
        /// <param name="tblpay">tb_StoreCredit tblpay</param>
        /// <returns>Returns the Updated StoreCreditID</returns>
        public Int32 UpdateStoreCredittype(tb_StoreCredit tblpay)
        {
            Int32 isUpdated = 0;
            try
            {
                paydac.Update(tblpay);
                isUpdated = tblpay.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }


        /// <summary>
        /// Function for checking duplicate StoreCredit type
        /// </summary>
        /// <param name="tblpay">tb_StoreCredit tblpay</param>
        /// <returns>Returns true if exist</returns>
        public bool CheckDuplicate(tb_StoreCredit tblpay)
        {
            if (paydac.CheckDuplicate(tblpay) == 0)
            {
                return false;
            }
            else
                return true;
        }


        /// <summary>
        /// Create StoreCredit type
        /// </summary>
        /// <param name="tblpay">tb_StoreCredit tblpay</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 CreateStoreCredittype(tb_StoreCredit tblpay)
        {
            Int32 isAdded = 0;
            try
            {
                paydac.CreateStoreCredittype(tblpay);
                isAdded = tblpay.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }
    }

   public class StoreCreditComponentEntity
   {
       private int _ID;

       public int ID
       {
           get { return _ID; }
           set { _ID = value; }
       }

      
       private string _StoreCredit;

       public string StoreCredit
       {
           get { return _StoreCredit; }
           set { _StoreCredit = value; }
       }

       private double _StoreCreditAmount;

       public double StoreCreditAmount
       {
           get { return _StoreCreditAmount; }
           set { _StoreCreditAmount = value; }
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
