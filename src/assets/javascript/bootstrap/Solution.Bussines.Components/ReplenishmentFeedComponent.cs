using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Data.Objects;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// ReplenishmentFeed Component Class Contains Feed Related functions  
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2015.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ReplenishmentFeedComponent
    {
        #region Bussiness Intelligence Report
        public DataSet GetStoreList()
        {
            ReplenishmentFeedDAC objFeed = new ReplenishmentFeedDAC();
            DataSet _dsStore = new DataSet();
            _dsStore = objFeed.GetStoreList();
            return _dsStore;

        }
        public DataSet GetExistingStoreList()
        {
            ReplenishmentFeedDAC objFeed = new ReplenishmentFeedDAC();
            DataSet _dsStore = new DataSet();
            _dsStore = objFeed.GetExistingStoreList();
            return _dsStore;

        }
        public DataSet GetFieldDetails(Int32 StoreId)
        {
            ReplenishmentFeedDAC objFeed = new ReplenishmentFeedDAC();
            DataSet _dsfield = new DataSet();
            _dsfield = objFeed.GetFieldDetails(StoreId);
            return _dsfield;

        }
        public DataSet GetMappingColumnName()
        {
            ReplenishmentFeedDAC objFeed = new ReplenishmentFeedDAC();
            DataSet _dscolumnname = new DataSet();
            _dscolumnname = objFeed.GetMappingColumnName();
            return _dscolumnname;

        }
        public DataSet GetSearchData(string Subcategory, string Sku_upc, string Fabriccode, string Datefrom, string Dateto)
        {
            ReplenishmentFeedDAC objFeed = new ReplenishmentFeedDAC();
            DataSet _dscolumnname = new DataSet();
            _dscolumnname = objFeed.GetSearchData(Subcategory, Sku_upc, Fabriccode, Datefrom, Dateto);
            return _dscolumnname;

        }


        #endregion
    }
}
