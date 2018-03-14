using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Solution.Bussines.Entities;
namespace Solution.Data
{
    /// <summary>
    /// Order Data Access Class Contains Order related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class DashboardDAC
    {
        #region Declaration
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        #endregion

        #region Key Functions
        /// <summary>
        ///  Get Top 10 Customer
        /// </summary>
        /// <returns>Returns Dataset - top 10 Customers</returns>

        public DataSet GetTop10Customer(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_GetTop10Customer";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Top 5 Items By Sales
        /// </summary>
        /// <returns>Returns List of top 5 items by max sales</returns>
        public DataSet GetTop5ItemsBySales(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_GetTop5ItemsBySales";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Latest Top 10 Orders
        /// </summary>
        /// <returns>Returns List of Top 10 Orders</returns>
        public DataSet GetTopTenOrders(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Order_GetTopTenOrder";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Key Performance Indicators for DashBoard
        /// </summary>
        /// <returns>Returns the List of All Key Performance Indicators as a separate Tables in one Dataset </returns>
        public DataSet GetAllKeyPerformanceIndicator(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_KeyPerformanceIndicator";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Dashboard Settings
        /// </summary>
        /// <param name="AdminID">Int32 AdminID</param>
        /// /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the List of All Setting of dashboard Store wise</returns>
        public DataSet GetAllDashboardSettings(Int32 StoreID, Int32 AdminID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_DashboardSettings";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@AdminID", AdminID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Search Log list for Dashboard
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Search Log List</returns>
        public DataSet GetSearchLog(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_GetSearchLog";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Order and Amount Calculations for Displaying in Chart in Dashboard
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Duration">String Duration</param>
        /// <returns>Returns Details of Orders and Amount according to durations</returns>
        public DataSet GetChartDetails(Int32 StoreID, String Duration)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_GetChartDetails";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Duration", Duration);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Revenue and Quantity Details for Displaying in KPI Meter
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Duration">String Duration</param>
        /// <returns>Returns Details of Revenue and Quantity according to durations</returns>
        public DataSet GetKPIMeter(Int32 StoreID, String Duration)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_KPIMeter";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Duration", Duration);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Top 10 Low Inventory List for Dashboard
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns> Returns Top 10 Low Inventory List</returns>
        public DataSet GetLowInventory(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_LowInventory";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Agent Monthly Sales List
        /// </summary>
        /// <param name="Duration">String Duration</param>
        /// <returns> Returns Agent Monthly Sales List</returns>
        public DataSet getagentmonthlysales(Int32 StoreID, String Duration)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_getagentmonthlysales";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Duration", Duration);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Sales Agent OrderList
        /// </summary>
        /// <returns>Returns Sales Agent OrderList</returns>
        public DataSet getsalesagentorderlist(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_getsalesagentorderlist";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Low Inventory List for Report
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns> Returns Low Inventory List for Report</returns>
        public DataSet GetLowInventoryForReport(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_LowInventory";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Gets the details of dashboard setting
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>Returns tb_DashboardSettings</returns>
        public tb_DashboardSettings GetDashboardSetting(int id)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_DashboardSettings tb_dashboard = null;
            tb_dashboard = ctx.tb_DashboardSettings.First(e => e.SettingID == id);
            return tb_dashboard;
        }

        /// <summary>
        /// Get the Admin List for Dashboard Setting
        /// </summary>
        /// <returns>Returns Dataset of Admin List</returns>
        public DataSet GetAdminListForDashboardSetting()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_AdminListForDashboardSetting";
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert/Updates the dashboard setting
        /// </summary>
        /// <param name="tb_dashboard">table tb_DashboardSettings</param>
        /// <param name="IsAdminAllow">Boolean IsAdminAllow</param>
        /// <param name="AdminID">Int32 AdminID</param>
        /// <returns>Returns Updated Status</returns>
        public Int32 Insert_Update_dashboardAdminRights(Int32 SettingID, Boolean IsAdminAllow, Int32 AdminID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_AdminListForDashboardSetting";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@SettingID", SettingID);
            cmd.Parameters.AddWithValue("@AdminID", AdminID);
            if (IsAdminAllow == true)
            {
                cmd.Parameters.AddWithValue("@Mode", 2);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Mode", 3);
            }

            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Updates the dashboard setting
        /// </summary>
        /// <param name="tb_dashboard">table tb_DashboardSettings</param>
        public void Update(tb_DashboardSettings tb_dashboard)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            ctx.SaveChanges();
        }

        /// <summary>
        /// Get Customer Profit by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetProfitableCustomer(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_Report_GetProfitableCustomer]";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Product Sales by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetProductBySales(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_GetAllItemsBySales";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Recent Added Product by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetRecentAddedProduct(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_GetRecentAddedProduct";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Recent Customer by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetRecentCustomer(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_GetRecentCustomer";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Product Inquiry by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetProductInquiry(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_GetProductInquiry";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Search record in Low Inventory report
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns Low Inventory Product List </returns>
        public DataSet SearchLowInventoryProduct(Int32 CategoryID, int StoreID, string SearchBy, string SearchValue)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_SearchLowInventory";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@SearchBy", SearchBy);
            cmd.Parameters.AddWithValue("@Searchvalue", SearchValue);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        ///  Get Chart Details for Yahoo Details
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="FromDate">DateTime FromDate</param>
        /// <param name="Todate">DateTime Todate</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <param name="ReportMode">String ReportMode</param>
        /// <returns>Returns Dataset List</returns>
        public DataSet GetChartDetailsforYahooDetails(Int32 StoreID, DateTime FromDate, DateTime Todate, Int32 Mode, String ReportMode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_YahooReports";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@FromDate", FromDate);
            cmd.Parameters.AddWithValue("@ToDate", Todate);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@ReportMode", ReportMode);
            return objSql.GetDs(cmd);
        }

        public String CloneDashboardSetting(Int32 MasterStoreID, Int32 CloneStoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_CloneDashboardSettings";
            cmd.Parameters.AddWithValue("@MasterStoreID", MasterStoreID);
            cmd.Parameters.AddWithValue("@CloneStoreID", CloneStoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            String strMessage = Convert.ToString(objSql.ExecuteScalarQuery(cmd));
            return strMessage;
        }
        public DataSet GetCloneStoreID(Int32 MasterStoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_CloneDashboardSettings";
            cmd.Parameters.AddWithValue("@MasterStoreID", MasterStoreID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }



        /// <summary>
        /// Get Top 10 Best Selling Colors
        /// </summary>
        /// <returns>Returns List of top 10 best selling colors</returns>
        public DataSet GetTop10BestSellingColors(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Dashboard_GetTop10BestSellingColor";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product Sales by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetProductColorBySales(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_GetAllItemsColorBySales";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }



        /// <summary>
        /// Get Top 10 Best Selling Pattern
        /// </summary>
        /// <returns>Returns List of top 10 best selling pattern</returns>
        public DataSet GetTop10BestSellingPattern(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Dashboard_GetTop10BestSellingPattern";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);

        }


        /// <summary>
        /// Get Product pattern Sales by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetProductPatternBySales(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_GetAllItemsPatternBySales";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Sales Agent Own Sales
        /// </summary>
        /// <returns>Returns List of Sales Agent Own Sales</returns>

        public DataSet GetSalesAgentOwnSale(Int32 StoreID, Int32 LoginID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Dashboard_GetSalesAgentOwnSales";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
                cmd.Parameters.AddWithValue("@LoginID", LoginID);

            }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Top 10 Coupons
        /// </summary>
        /// <returns>Returns List of  Top 10 Coupons</returns>

        public DataSet GetTop10Coupons(Int32 StoreID, Int32 LoginID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_CouponList";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            if (LoginID > 0)
            {
                cmd.Parameters.AddWithValue("@LoginID", LoginID);
            }

            return objSql.GetDs(cmd);
        }








        /// <summary>
        /// Get Sales Agent Own Sales Report by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>

        public DataSet GetSalesAgentOwnSalesReport(Int32 StoreID, Int32 LoginID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_GetSalesAgentOwnSales";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
                cmd.Parameters.AddWithValue("@LoginID", LoginID);

            }
            return objSql.GetDs(cmd);
        }



        /// <summary>
        /// Get Top 10 Best Selling Pattern
        /// </summary>
        /// <returns>Returns List of top 10 best selling pattern</returns>
        public DataSet GetTop10SalesAgentOwnOpenQuotes(Int32 StoreID, Int32 LoginID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Dashboard_GetTopTenSalesAgentOwnOpenQuotes";
            if (LoginID > 0)
            {
                cmd.Parameters.AddWithValue("@LoginID", LoginID);
            }
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);

            }
            return objSql.GetDs(cmd);

        }



        /// <summary>
        /// Get Low Inventory Alert List for Report
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns> Returns Low Inventory List for Report</returns>
        public DataSet GetLowInventoryAlertForReport(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_LowInventoryAlert";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get  Low Inventory  Remainder List for Dashboard
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns> Returns Low Inventory Remainder List</returns>
        public DataSet GetLowInventoryAlert(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_LowInventoryAlert";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Search record in Low Inventory Alert report
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns Low Inventory Alert Product List </returns>
        public DataSet SearchLowInventoryProductAlert(Int32 CategoryID, int StoreID, string SearchBy, string SearchValue)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_SearchLowInventoryAlert";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@SearchBy", SearchBy);
            cmd.Parameters.AddWithValue("@Searchvalue", SearchValue);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get New Arrival Product by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetNewArrivalProduct(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DashBoard_GetNewArrivalProduct";
            if (StoreID > 0)
            {
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
            }
            return objSql.GetDs(cmd);
        }


        #endregion
    }
}
