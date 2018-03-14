using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Data.Objects;
using Solution.Data;
using System.Linq.Expressions;
using System.IO;
using System.Xml;
using Solution.Bussines.Components.Common;
using System.Web;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Common Component Class Contains Common related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CommonComponent
    {
        #region Declaration

        private static int CatID = 0;

        #endregion

        #region  Key Functions

        /// <summary>
        /// Get Category name
        /// </summary>
        /// <param name="CatName">string CatName</param>
        /// <param name="ParentName">string ParentName</param>
        /// <param name="GrandName">string GrandName</param>
        /// <param name="StoreID"> int StoreID</param>
        /// <param name="CategoryID">int CategoryID</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns Category Dataset with Details</returns>
        public static int GetCategoryID(string CatName, string ParentName, string GrandName, int StoreID, int CategoryID, int Mode)
        {
            CommonDAC dac = new CommonDAC();
            CatID = Convert.ToInt32(dac.GetCategoryID(CatName, ParentName, GrandName, StoreID, CategoryID, Mode));
            return CatID;
        }

        /// <summary>
        /// Get Dataset from Common SP
        /// </summary>
        /// <param name="Query">string Query</param>
        /// <returns>Returns Dataset</returns>
        public static DataSet GetCommonDataSet(string Query)
        {
            CommonDAC dac = new CommonDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetCommonDataSet(Query);
            return DSCommon;
        }

        /// <summary>
        /// Get Scalar Data From Common SP
        /// </summary>
        /// <param name="Query">string Query</param>
        /// <returns>Returns Object</returns>
        public static Object GetScalarCommonData(string Query)
        {
            Object objScaler = null;
            CommonDAC dac = new CommonDAC();
            objScaler = dac.GetScalarCommonData(Query);
            return objScaler;
        }

        /// <summary>
        /// Execute from Common SP
        /// </summary>
        /// <param name="Query">string Query</param>
        /// <returns>Returns Object</returns>
        public static Object ExecuteCommonData(string Query)
        {
            Object objExecute = null;
            CommonDAC dac = new CommonDAC();
            objExecute = dac.ExecuteCommonData(Query);
            return objExecute;
        }

        /// <summary>
        /// Execute from Common SP
        /// </summary>
        /// <param name="Query">string Query</param>
        /// <returns>Returns Object</returns>
        public static Object ExecuteDatabaseBackup(string Query)
        {
            Object objExecute = null;
            CommonDAC dac = new CommonDAC();
            objExecute = dac.ExecuteDatabaseBackup(Query);
            return objExecute;
        }

        /// <summary>
        /// Error Log
        /// </summary>
        /// <param name="PageName">string PageName</param>
        /// <param name="Error">string Error</param>
        /// <param name="ErrorDetails">string ErrorDetails</param>
        public static void ErrorLog(string PageName, string Error, string ErrorDetails)
        {
            CommonDAC.ErrorLog(PageName, Error, ErrorDetails);
        }

        /// <summary>
        /// Get Dataset For Export Customer List
        /// </summary>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="StoreId">string StoreId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCustomerExport(string SearchValue, string StoreId)
        {
            CommonDAC dac = new CommonDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetCustomerExport(SearchValue, StoreId);
            return DSCommon;
        }

        /// <summary>
        /// Get Columns name From Table Product for Export
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetProductColumns(int Mode, int StoreID)
        {
            CommonDAC dac = new CommonDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetProductColumns(Mode, StoreID);
            return DSCommon;
        }

        /// <summary>
        /// Get Records in Dataset For Export Product
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="Query">String Query</param>
        /// /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetProductExport(int Mode, String Query, int StoreID)
        {
            CommonDAC dac = new CommonDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetProductExport(Mode, Query, StoreID);
            return DSCommon;
        }

        /// <summary>
        ///  Get Yahoo Report
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Fromdate">DateTime Fromdate</param>
        /// <param name="ToDate">DateTime ToDate</param>
        /// <returns>Returns Yahoo Report using filtered Data</returns>
        public DataSet GetYahooReports(int Mode, int StoreID, DateTime Fromdate, DateTime ToDate)
        {
            CommonDAC dac = new CommonDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetYahooReports(Mode, StoreID, Fromdate, ToDate);
            return DSCommon;
        }
        #endregion
    }

    public static class extensionmethods
    {
        /// <summary>
        /// Method for Sorting Ascending/Descending
        /// </summary>
        /// <param name="SortField">string SortField</param>
        /// <param name="Ascending">bool Ascending</param>
        /// <returns>Returns Ascending/Descending Value</returns>
        public static IQueryable<T> OrderByField<T>(this IQueryable<T> q, string SortField, bool Ascending)
        {
            var param = Expression.Parameter(typeof(T), "p");
            var prop = Expression.Property(param, SortField);
            var exp = Expression.Lambda(prop, param);
            string method = Ascending ? "OrderBy" : "OrderByDescending";
            Type[] types = new Type[] { q.ElementType, exp.Body.Type };
            var mce = Expression.Call(typeof(Queryable), method, types, q.Expression, exp);
            return q.Provider.CreateQuery<T>(mce);
        }
    }
}
