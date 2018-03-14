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
    /// Product Type Component Class Contains Product Type related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ProductTypeComponent
    {

        #region Key Function
        /// <summary>
        /// Get Product Type By StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the list of product type by StoreID</returns>
        public static DataSet GetProductTypeByStoreID(Int32 StoreID)
        {
            ProductTypeDAC dac = new ProductTypeDAC();
            DataSet DSProductType = new DataSet();
            DSProductType = dac.GetProductTypeByStoreID(StoreID);
            return DSProductType;
        }

        /// <summary>
        /// Get Product Type Delivery By StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the list of product type by StoreID</returns>
        public static DataSet GetProductTypeDeliveryByStoreID(Int32 StoreID)
        {
            ProductTypeDAC dac = new ProductTypeDAC();
            DataSet DSProductTypeDelivery = new DataSet();
            DSProductTypeDelivery = dac.GetProductTypeDeliveryByStoreID(StoreID);
            return DSProductTypeDelivery;

        }

        /// <summary>
        /// Get Product Type Delivery By StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the list of product type by StoreID</returns>
        public DataSet GetProductTypeByName(string Name, int StoreID)
        {
            ProductTypeDAC dac = new ProductTypeDAC();
            DataSet DSProductTypeDelivery = new DataSet();
            DSProductTypeDelivery = dac.GetProductTypeByName(Name, StoreID);
            return DSProductTypeDelivery;
        }

        #endregion

    }
}
