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
    /// Dashboard Component Class Contains Order,Customer and product related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class DashboardComponent
    {
        #region Local Variables

        // Notice - Start -  Don't Delete this "ControlStoreID" because it is used for Controls of Dashboard
        public static Int32 ControlStoreID = 0;
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        private static int _count;
        DashboardDAC dashdac = new DashboardDAC();
        // Notice End

        #endregion

        #region Grid Function
        /// <summary>
        ///  Get Top 10 Customer
        /// </summary>
        /// <returns>Returns Dataset - top 10 Customers</returns>

        public static DataSet GetTop10Customer(int ControlStoreID)
        {
            DataSet DSCustomer = new DataSet();
            DashboardDAC dac = new DashboardDAC();
            DSCustomer = dac.GetTop10Customer(ControlStoreID);
            return DSCustomer;
        }

        /// <summary>
        /// Get Top 5 Items By Sales
        /// </summary>
        /// <returns>Returns List of top 5 items by max sales</returns>
        public static DataSet GetTop5ItemsBySales(int ControlStoreID)
        {
            DataSet dsTop5Items = new DataSet();
            DashboardDAC objDashBoaerd = new DashboardDAC();
            dsTop5Items = objDashBoaerd.GetTop5ItemsBySales(ControlStoreID);
            return dsTop5Items;
        }

        /// <summary>
        /// Get Latest Top 10 Orders
        /// </summary>
        /// <returns>Returns List of Top 10 Orders</returns>
        public static DataSet GetTopTenOrders()
        {
            DataSet dsTopTenOrders = new DataSet();
            DashboardDAC objDashBoaerd = new DashboardDAC();
            dsTopTenOrders = objDashBoaerd.GetTopTenOrders(ControlStoreID);
            return dsTopTenOrders;
        }

        /// <summary>
        /// Get All Key Performance Indicators for DashBoard
        /// </summary>
        /// <returns>Returns the List of All Key Performance Indicators as a separate Tables in one Dataset </returns>
        public static DataSet GetAllKeyPerformanceIndicator()
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetAllKeyPerformanceIndicator(ControlStoreID);
        }

        /// <summary>
        /// Get All Dashboard Settings
        /// </summary>
        /// <param name="AdminID">Int32 AdminID</param>
        /// <returns>Returns the List of All Setting of dashboard Store wise</returns>
        public static DataSet GetAllDashboardSettings(Int32 AdminID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetAllDashboardSettings(ControlStoreID, AdminID);
        }

        /// <summary>
        /// Get Search Log list for Dashboard
        /// </summary>       
        /// <returns>Returns Search Log List</returns>
        public static DataSet GetSearchLog(int ControlStoreID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetSearchLog(ControlStoreID);
        }

        /// <summary>
        /// Get Order and Amount Calculations for Displaying in Chart in Dashboard
        /// </summary>
        /// <param name="Duration">String Duration</param>
        /// <returns>Returns Details of Orders and Amount according to durations</returns>
        public static DataSet GetChartDetails(String Duration)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetChartDetails(ControlStoreID, Duration);
        }

        /// <summary>
        /// Get Revenue and Quantity Details for Displaying in KPI Meter
        /// </summary>
        /// <param name="Duration">String Duration</param>
        /// <returns>Returns Details of Revenue and Quantity according to durations</returns>
        public static DataSet GetKPIMeter(String Duration)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetKPIMeter(ControlStoreID, Duration);
        }

        /// <summary>
        /// Get Top 10 Low Inventory List for Dashboard
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns> Returns Top 10 Low Inventory List</returns>
        public static DataSet GetLowInventory(int ControlStoreID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetLowInventory(ControlStoreID);
        }

        /// <summary>
        /// Get Agent Monthly Sales List
        /// </summary>
        /// <param name="Duration">String Duration</param>
        /// <returns> Returns Agent Monthly Sales List</returns>
        public static DataSet getagentmonthlysales(String Duration)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.getagentmonthlysales(ControlStoreID, Duration);
        }

        /// <summary>
        /// Get Sales Agent OrderList
        /// </summary>
        /// <returns>Returns Sales Agent OrderList</returns>
        public static DataSet getsalesagentorderlist()
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.getsalesagentorderlist(ControlStoreID);
        }

        /// <summary>
        /// Get Low Inventory List for Report
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns> Returns Low Inventory List for Report</returns>
        public static DataSet GetLowInventoryForReport(Int32 StoreID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetLowInventoryForReport(StoreID);
        }
        
        /// <summary>
        /// Updates the dashboard setting
        /// </summary>
        /// <param name="tb_dashboard">table tb_DashboardSettings</param>
        /// <param name="IsAdminAllow">Boolean IsAdminAllow</param>
        /// <param name="AdminID">Int32 AdminID</param>
        /// <returns>Returns Updated Status</returns>
        public Int32 UpdateDashboardSetting(tb_DashboardSettings tb_dashboard, Boolean IsAdminAllow, Int32 AdminID)
        {
            Int32 isUpdated = 0;
            try
            {
                dashdac.Update(tb_dashboard);
                isUpdated = tb_dashboard.SettingID;
                Insert_Update_dashboardAdminRights(tb_dashboard.SettingID, IsAdminAllow, AdminID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Insert/Updates the dashboard setting
        /// </summary>
        /// <param name="tb_dashboard">table tb_DashboardSettings</param>
        /// <param name="IsAdminAllow">Boolean IsAdminAllow</param>
        /// <param name="AdminID">Int32 AdminID</param>
        /// <returns>Returns Updated Status</returns>
        private Int32 Insert_Update_dashboardAdminRights(Int32 SettingID, Boolean IsAdminAllow, Int32 AdminID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.Insert_Update_dashboardAdminRights(SettingID, IsAdminAllow, AdminID);
        }

        /// <summary>
        /// Counts the number of record
        /// </summary>
        /// <param name="pStoreId">Store ID</param>
        /// <param name="Position">String Position</param>
        /// <returns> Returns Total Record Count </returns>
        public static int GetCount(Int32 pStoreId, Int32 pAdminId, String Position)
        {
            return _count;
        }

        /// <summary>
        /// Gets the details of dashboard setting
        /// </summary>
        /// <param name="id">int id</param>
        /// <returns>Returns tb_DashboardSettings</returns>
        public tb_DashboardSettings GetDashboardSetting(int id)
        {
            return dashdac.GetDashboardSetting(id);
        }

        /// <summary>
        /// Gets the data source for LeftControlGridview on first time load and after filtering
        /// </summary>
        /// <param name="startIndex">Int32 startIndex</param>
        /// <param name="pStoreId">Int32 pStoreId</param>
        /// <param name="Position">String Position</param>
        /// <param name="pAdminId">Int32 pAdminId</param>
        /// <param name="pageSize">Int32 pageSize</param>
        /// <returns>Returns List of Dashboard Component Entity </returns>
        public List<DashboardComponentEntity> GetDataByFilterControl(Int32 startIndex, Int32 pStoreId, String Position, Int32 pAdminId, Int32 pageSize)
        {
            var results = from a in ctx.tb_DashboardSettings
                          join c in ctx.tb_DashboardUserRights
                          on a.SettingID equals c.SettingID into Ljoin
                          from Gjoin in Ljoin.Where(g => g.AdminID == pAdminId).DefaultIfEmpty()
                          where a.Position == Position
                          select new DashboardComponentEntity
                          {
                              SettingID = a.SettingID,
                              SectionName = a.SectionName,
                              IsDisplay = a.IsDisplay.Value,
                              DisplayPosition = a.DisplayPosition.Value,
                              StoreID = a.StoreID.Value,
                              IsAdminAllow = Gjoin.AdminID == null || Gjoin.AdminID == 0 ? false : true
                          };


            results = results.OrderBy(g => g.DisplayPosition).Where(v => v.StoreID == pStoreId);
            _count = results.Count();
            results = results.Skip(startIndex).Take(pageSize).AsQueryable();
            return results.ToList<DashboardComponentEntity>();
        }

        /// <summary>
        /// Get Customer Profit by Store ID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public static DataSet GetProfitableCustomer(Int32 StoreID)
        {
            DataSet DSCustomer = new DataSet();
            DashboardDAC dac = new DashboardDAC();
            DSCustomer = dac.GetProfitableCustomer(StoreID);
            return DSCustomer;
        }

        /// <summary>
        /// Get Product Sales by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public static DataSet GetProductBySales(Int32 StoreID)
        {
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.GetProductBySales(StoreID);
            return dsProduct;
        }

        /// <summary>
        /// Get Recent Added Product by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public static DataSet GetRecentAddedProduct()
        {
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.GetRecentAddedProduct(ControlStoreID);
            return dsProduct;
        }

        /// <summary>
        /// Get Recent Customer by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public static DataSet GetRecentCustomer()
        {
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.GetRecentCustomer(ControlStoreID);
            return dsProduct;
        }
        /// <summary>
        /// Get Product Inquiry by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public static DataSet GetProductInquiry()
        {
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.GetProductInquiry(ControlStoreID);
            return dsProduct;
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
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.SearchLowInventoryProduct(CategoryID, StoreID, SearchBy, SearchValue);
            return dsProduct;
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
        public static DataSet GetChartDetailsforYahooDetails(Int32 StoreId, DateTime FromDate, DateTime Todate, Int32 Mode, String ReportMode)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetChartDetailsforYahooDetails(StoreId, FromDate, Todate, Mode, ReportMode);
        }

        /// <summary>
        /// Get the Admin List for Dashboard Setting
        /// </summary>
        /// <returns> Dataset of Admin List</returns>
        public DataSet GetAdminListForDashboardSetting()
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            DataSet dsAdminList = new DataSet();
            dsAdminList = objDashBoard.GetAdminListForDashboardSetting();
            return dsAdminList;
        }

        public String CloneDashboardSetting(Int32 MasterStoreID, Int32 CloneStoreID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.CloneDashboardSetting(MasterStoreID, CloneStoreID);
        }

        /// <summary>
        /// Get the Clone Store List for Dashboard Setting
        /// </summary>
        /// <returns> Dataset of Clone Store List</returns>
        public DataSet GetCloneStoreID(Int32 MasterStoreID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            DataSet dsCloneStoreList = new DataSet();
            dsCloneStoreList = objDashBoard.GetCloneStoreID(MasterStoreID);
            return dsCloneStoreList;
        }


        /// <summary>
        /// Get Top 10 best selling colors
        /// </summary>
        /// <returns>Returns top 10 best selling colors</returns>
        public static DataSet GetTop10BestSellingColors(int StoreID)
        {
            DataSet dsTop510Colors = new DataSet();
            DashboardDAC objDashBoaerd = new DashboardDAC();
            dsTop510Colors = objDashBoaerd.GetTop10BestSellingColors(StoreID);
            return dsTop510Colors;
        }


        /// <summary>
        /// Get Product color Sales by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public static DataSet GetProductColorBySales(Int32 StoreID)
        {
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.GetProductColorBySales(StoreID);
            return dsProduct;
        }


        /// <summary>
        /// Get Top 10 best selling colors
        /// </summary>
        /// <returns>Returns top 10 best selling patterns</returns>

        public static DataSet GetTop10BestSellingPattern(int StoreID)
        {
            DataSet dsTop10Pattern = new DataSet();
            DashboardDAC objDashBoaerd = new DashboardDAC();
            dsTop10Pattern = objDashBoaerd.GetTop10BestSellingPattern(StoreID);
            return dsTop10Pattern;

        }

        /// <summary>
        /// Get Product pattern Sales by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>

        public static DataSet GetProductPatternBySales(Int32 StoreID)
        {
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.GetProductPatternBySales(StoreID);
            return dsProduct;

        }



        /// <summary>
        /// Get sales agent own sales
        /// </summary>
        /// <returns>Returns List sales agent own  Orders</returns>
        public static DataSet GetSalesAgentOwnSale(int StoreID, int LoginID)
        {
            DataSet dsSalesAgentOrders = new DataSet();
            DashboardDAC objDashBoaerd = new DashboardDAC();
            dsSalesAgentOrders = objDashBoaerd.GetSalesAgentOwnSale(StoreID, LoginID);
            return dsSalesAgentOrders;
        }



        /// <summary>
        /// Get Sales Agent Own Sale
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>


        public static DataSet GetSalesAgentOwnSalesReport(Int32 StoreID, int LoginID)
        {
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.GetSalesAgentOwnSalesReport(StoreID, LoginID);
            return dsProduct;
        }


        /// <summary>
        /// Get Top 10 Coupons
        /// </summary>
        /// <returns>Returns top 10 Coupons</returns>
        public static DataSet GetTop10Coupons(int StoreID, int LoginID)
        {
            DataSet dsTop10Coupons = new DataSet();
            DashboardDAC objDashBoaerd = new DashboardDAC();
            dsTop10Coupons = objDashBoaerd.GetTop10Coupons(StoreID, LoginID);
            return dsTop10Coupons;
        }

        /// <summary>
        /// Get Top 10 sales agent own quotes
        /// </summary>
        /// <returns>Returns Top 10 sales agent own quotes</returns>
        public static DataSet GetTop10SalesAgentOwnOpenQuotes(int StoreID, int LoginID)
        {
            DataSet dsTop10Pattern = new DataSet();
            DashboardDAC objDashBoaerd = new DashboardDAC();
            dsTop10Pattern = objDashBoaerd.GetTop10SalesAgentOwnOpenQuotes(StoreID, LoginID);
            return dsTop10Pattern;

        }

        /// <summary>
        /// Get Low Inventory Alert List for Report
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns> Returns Low Inventory List for Report</returns>
        public static DataSet GetLowInventoryAlertForReport(Int32 StoreID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetLowInventoryAlertForReport(StoreID);
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
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.SearchLowInventoryProductAlert(CategoryID, StoreID, SearchBy, SearchValue);
            return dsProduct;
        }

        /// <summary>
        /// Get  Low Inventory Remainder List for Dashboard
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns> Returns Top 10 Low Inventory List</returns>
        public static DataSet GetLowInventoryAlert(int ControlStoreID)
        {
            DashboardDAC objDashBoard = new DashboardDAC();
            return objDashBoard.GetLowInventoryAlert(ControlStoreID);
        }


        /// <summary>
        /// Get New Arrival Product by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public static DataSet GetNewArrivalProduct()
        {
            DataSet dsProduct = new DataSet();
            DashboardDAC objDashBoard = new DashboardDAC();
            dsProduct = objDashBoard.GetNewArrivalProduct(ControlStoreID);
            return dsProduct;
        }

        #region Properties

        /// <summary>
        /// Variable And Properties
        /// </summary>
        public class DashboardComponentEntity
        {
            private int _SettingID;

            public int SettingID
            {
                get { return _SettingID; }
                set { _SettingID = value; }
            }

            private string _SectionName;

            public string SectionName
            {
                get { return _SectionName; }
                set { _SectionName = value; }
            }

            private bool _IsDisplay;

            public bool IsDisplay
            {
                get { return _IsDisplay; }
                set { _IsDisplay = value; }
            }

            private int _StoreID;

            public int StoreID
            {
                get { return _StoreID; }
                set { _StoreID = value; }
            }

            public int _DisplayPosition;

            public int DisplayPosition
            {
                get { return _DisplayPosition; }
                set { _DisplayPosition = value; }
            }
            private bool? _IsAdminAllow;

            public bool? IsAdminAllow
            {
                get { return _IsAdminAllow; }
                set { _IsAdminAllow = value; }
            }

        }
        #endregion

        #endregion
    }
}
