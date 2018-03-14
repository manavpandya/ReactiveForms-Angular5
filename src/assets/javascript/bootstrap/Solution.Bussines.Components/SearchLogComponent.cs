using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Data.Objects;
using Solution.Bussines.Components.Common;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Search Log Class Contains Search Log related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 
    public class SearchLogComponent
    {
        #region Declaration

        public Int32 StoreID = 0;
        SearchLogDAC objSearchLog = null;
        #endregion

        #region All Constructors
        /// <summary>
        /// Constructor with out parameter that initializes StoreID when component file is loading
        /// </summary>
        public SearchLogComponent()
        {
            StoreID = 1;// AppConfig.StoreID;
        }

        #endregion

        #region  Key Functions
        /// <summary>
        /// Insert an email entry in search log table
        /// </summary>
        /// <param name="tb_SearchLog">tb_SearchLog tb_SearchLog</param>
        /// <returns>Returns an Identity value of recently inserted SearchLog record</returns>
        public Int32 InsertSearchLog(tb_SearchLog tb_SearchLog)
        {
            int isAdded = 0;

            try
            {
                SearchLogDAC objSearchLog = new SearchLogDAC();
                objSearchLog.InsertSearchLog(tb_SearchLog);
                isAdded = tb_SearchLog.SearchLogID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }
        #endregion

    }


}
