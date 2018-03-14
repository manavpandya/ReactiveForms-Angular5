using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Warehouse Component Class Contains Warehouse related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class WarehouseComponent
    {
        #region Declaration
        int _count;
        #endregion

        #region Key Function

        /// <summary>
        /// Get Warehouse Product List By StoreID
        /// </summary>
        /// <param name="Storeid">Int32 StoreID</param>
        /// <returns>Return Product list for display as a Dataset</returns>
        public DataSet GetWarehouseProductListByStoreID(int Storeid)
        {
            DataSet DSWarehouse = new DataSet();
            WarehouseDAC dac = new WarehouseDAC();
            DSWarehouse = dac.GetWarehouseProductListByStoreID(Storeid);
            return DSWarehouse;
        }

        #endregion
    }
}
