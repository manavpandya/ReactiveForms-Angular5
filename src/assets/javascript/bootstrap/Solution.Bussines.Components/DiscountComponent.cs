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

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Discount Component Class Contains Discount related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
   
    public class DiscountComponent
    {
      
        #region Key Function

        /// <summary>
        /// Get Quantity Discount by StoreID
        /// </summary>
        /// <param name="StoreID">Int StoreID</param>
        /// <returns>Returns Quantity Discount Data in DataSet</returns>
        public static DataSet GetQuantityDiscountByStoreID(Int32 StoreID)
        {
            DataSet DSQuantity = new DataSet();
            DiscountDAC dac = new DiscountDAC();
            DSQuantity = dac.GetQuantityDiscountByStoreID(StoreID);
            return DSQuantity;
        }

        #endregion

    }
}
