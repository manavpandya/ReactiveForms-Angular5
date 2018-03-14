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
    /// Admin Component Class Contains Admin related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class AdminComponent
    {
        #region  Key Functions
        public static Int32 BIStoreID = 0;
        /// <summary>
        /// Get Selected Theme of particular Store
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns selected Theme Name for particular store</returns>
        public static string GetTheme(Int32 StoreID)
        {
            AdminDAC dac = new AdminDAC();
            return dac.GetTheme(StoreID);
        }

        /// <summary>
        /// Get AdminID for MasterAdmin
        /// </summary>
        /// <returns>Returns Details of Admin Master using Id</returns>
        public int GetMasterAdminID()
        {
            AdminDAC dac = new AdminDAC();
            return dac.GetMasterAdminID();
        }

        /// <summary>
        /// Get admin login username and password
        /// </summary>
        /// <param name="UserName">string UserName</param>
        /// <param name="Password">string Password</param>
        /// <returns>Returns UserName and Password for login</returns>
        public static DataSet GetAdminForLogin(string UserName, string Password)
        {
            DataSet DSCommon = new DataSet();
            AdminDAC dac = new AdminDAC();
            DSCommon = dac.GetAdminForLogin(UserName, Password);
            return DSCommon;
        }

        /// <summary>
        /// Function For Create Admin
        /// </summary>
        /// <param name="Admin">tb_Admin tb_Admin</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 CreateAdmin(tb_Admin tb_Admin)
        {
            Int32 isAdded = 0;
            try
            {
                AdminDAC ObjAdmin = new AdminDAC();
                if (ObjAdmin.CheckDuplicate(tb_Admin) == 0)
                {
                    ObjAdmin.Create(tb_Admin);
                    isAdded = tb_Admin.AdminID;
                }
                else
                {
                    isAdded = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Update Method for Admin
        /// </summary>
        /// <param name="Admin">tb_Admin tb_Admin</param>
        /// <returns>Returns AdminID of current updated record</returns>
        public Int32 UpdateAdmin(tb_Admin tb_Admin)
        {
            Int32 isUpdated = 0;
            try
            {
                AdminDAC ObjAdmin = new AdminDAC();
                if (ObjAdmin.CheckDuplicate(tb_Admin) == 0)
                {
                    UpdateAdmins(tb_Admin);
                    isUpdated = tb_Admin.AdminID;
                }
                else
                {
                    isUpdated = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }

        /// <summary>
        /// Get Admin Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns a Admin record for edit in table form</returns>
        public tb_Admin getAdmin(int Id)
        {
            AdminDAC objAdmin = new AdminDAC();
            return objAdmin.getAdmin(Id);
        }

        /// <summary>
        /// Sub Method for Update Admin
        /// </summary>
        /// <param name="Admin">tb_Admin tb_Admin</param>
        /// <returns>Returns Update Admin Details</returns>
        private tb_Admin UpdateAdmins(tb_Admin tb_Admin)
        {
            AdminDAC ObjAdmin = new AdminDAC();
            ObjAdmin.Update(tb_Admin);
            return tb_Admin;
        }

        /// <summary>
        /// For Delete Admin By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public int delAdmin(int Id)
        {
            int IsDeleted = 0;
            try
            {
                AdminDAC ObjAdmin = new AdminDAC();
                IsDeleted = ObjAdmin.Delete(Id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return IsDeleted;
        }

        /// <summary>
        /// Get Password By Email Address
        /// </summary>
        /// <param name="Email">String Email</param>
        /// <returns>Returns Password</returns>
        public tb_Admin GetPasswordByEmail(String Email)
        {
            AdminDAC objAdmin = new AdminDAC();
            return objAdmin.GetPasswordByEmail(Email);
        }

        /// <summary>
        /// Method Data for Searching and Filtering functionality
        /// </summary>
        /// <param name="realIndex">int realIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">int sortBy</param>
        /// <param name="Filter">int Filter</param>
        /// <param name="ObjParameter">ObjectParameter ObjParameter</param>
        /// <returns>Returns Filtered Admin Data for display.</returns>
        public List<tb_Admin> GetDataByFilter(int realIndex, int pageSize, string sortBy, string Filter, ObjectParameter ObjParameter)
        {
            List<tb_Admin> ObjAdmin = new List<tb_Admin>();
            RedTag_CCTV_Ecomm_DBEntities objAdminEntity = new RedTag_CCTV_Ecomm_DBEntities();
            ObjAdmin = objAdminEntity.usp_Admin(realIndex, pageSize, sortBy, Filter, ObjParameter).ToList<tb_Admin>();
            return ObjAdmin;
        }

        #endregion

        #region Grid Functions

        #region Variable And Properties

        private static int _count;
        private static bool _newFilter = false;
        private static string _Filter = "";
        private static int _AdminId = 0;

        public static bool NewFilter
        {
            get { return _newFilter; }
            set { _newFilter = value; }
        }

        public static string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        public static int AdminID
        {
            get { return _AdminId; }
            set { _AdminId = value; }
        }


        #endregion

        #region Delete

        private static bool _afterDelete = false;

        /// <summary>
        /// Gets or sets the after delete flag.
        /// </summary>
        public static bool AfterDelete
        {
            get { return _afterDelete; }
            set { _afterDelete = value; }
        }

        #endregion

        #region Listing States Data & Total Count State

        /// <summary>
        ///  Methods for Searching and Filtering functionality
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">int sortBy</param>
        /// <param name="CName">int CName</param>
        /// <returns>Returns filtered admin table</returns>
        public static List<tb_Admin> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName)
        {
            if (string.IsNullOrEmpty(sortBy))
                sortBy = CName + " Asc";
            else if (sortBy.EndsWith(" DESC"))
            {
                sortBy = sortBy.Replace(",", " DESC,");
            }
            int realIndex = (_newFilter ? 0 : startIndex);
            ObjectParameter countParameter = new ObjectParameter("count", typeof(int));
            List<tb_Admin> objAdmin = new List<tb_Admin>();
            AdminComponent obj = new AdminComponent();
            objAdmin = obj.GetDataByFilter(realIndex, pageSize, sortBy, Filter, countParameter).ToList();
            _count = (int)countParameter.Value;
            return objAdmin;
        }

        /// <summary>
        /// Get total number of record of Admin table
        /// </summary>
        /// <param name="CName">string CName</param>
        /// <returns> Return count of rows for Admin table as int</returns>
        public static int GetCount(string CName)
        {
            return (_afterDelete ? _count : _count);
        }

        #endregion

        #endregion

        #region Bussiness Intelligence Report

        /// <summary>
        /// All Time Revenue
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeRevenue(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeRevenue(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get App Time Average Revenue
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeAverageRevenue(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeAverageRevenue(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// All Time Number Of Order
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeNumberOrder(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeOrderNumber(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// All Time Average Order
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeAverageOrder(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeAverageOrders(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get All Time Average Customer
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeAverageCustomer(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeAverageCustomer(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get All Time New Customer
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeNewCustomer(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeNewCustomer(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get Customer by country and state
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="Country">Country</param>
        /// <param name="State">State</param>
        /// <returns></returns>
        public DataSet GetCustomerByCountryState(string DBAction, DateTime? StartDate, DateTime? EndDate, int Country, string State)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetCustomerByCountryState(BIStoreID, DBAction, StartDate, EndDate, Country, State);
            return DSProduct;
        }

        /// <summary>
        /// Get Customer by revenue
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetCustomersByRevenue(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeCustomers_Revenue(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get Customer by Order
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetCustomersByOrder(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeCustomers_ByOrder(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get Top 25 Products
        /// </summary>
        /// <param name="DBAction">SP Acrtion</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeTop25Products(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeTop25Products(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }


        /// <summary>
        /// Get Top 25 Brands list
        /// </summary>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeTop25Brands(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeTop25Brands(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get All Time Top 25 Category
        /// </summary>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeTop25Category(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeTop25Category(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get Top 25 Selling Days
        /// </summary>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetTop25SellingDays(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetTop25SellingDays(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }
        /// <summary>
        /// Get TOp 3 Month by Revenu
        /// </summary>
        /// <param name="DBAction">SP Action</param>
        /// <returns></returns>
        public DataSet GetTop3SellingMonth_ByRevenue(string DBAction)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetTop3SellingMonth_ByRevenue(BIStoreID, DBAction);
            return DSProduct;
        }

        /// <summary>
        /// Get TOp 3 Month by Order
        /// </summary>
        /// <param name="DBAction">SP Action</param>
        /// <returns></returns>
        public DataSet GetTop3SellingMonth_ByOrder(string DBAction)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetTop3SellingMonth_ByOrder(BIStoreID, DBAction);
            return DSProduct;
        }

        /// <summary>
        /// Get Top 25 Selling Days by order
        /// </summary>
        /// <param name="DbAction">SP Action</param>
        /// <returns></returns>
        public DataSet GedtTop25SellingDaysByOrder(string DBAction)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GedtTop25SellingDaysByOrder(BIStoreID, DBAction);
            return DSProduct;
        }

        /// <summary>
        /// Get Top 3 Selling time by revenue
        /// </summary>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeTop3SellingTime_ByRevenue(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeTop3SellingTime_ByRevenue(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get Top 3 Selling time by order
        /// </summary>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeTop3SellingTime_ByOrder(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetAllTimeTop3SellingTime_ByOrder(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get Revenue by country and state
        /// </summary>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="Country">Country</param>
        /// <param name="State">State</param>
        /// <returns></returns>
        public DataSet GetOrderRevenuByCountryState(string DBAction, DateTime? StartDate, DateTime? EndDate, string Country, string State)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetOrderRevenue_ByCountry(BIStoreID, DBAction, StartDate, EndDate, Country, State);
            return DSProduct;
        }

        /// <summary>
        /// Get Store wise sales
        /// </summary>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetStoreSalesReport(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetStoreSalesReport(DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get Top 10 state
        /// </summary>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="OrderBy">Order by</param>
        /// <returns></returns>
        public DataSet GetTop10State(string DBAction, DateTime? StartDate, DateTime? EndDate, string OrderBy)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.Top10State(BIStoreID, DBAction, StartDate, EndDate, OrderBy);
            return DSProduct;
        }

        /// <summary>
        /// Order Status
        /// </summary>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetOrderStatus(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetOrderStatus(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        /// <summary>
        /// Get Top 10 Prorudct by review
        /// </summary>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetTop10ProductByReview(string DBAction, DateTime? StartDate, DateTime? EndDate)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetTop10ProductByReview(BIStoreID, DBAction, StartDate, EndDate);
            return DSProduct;
        }

        #region Product Summary
        /// <summary>
        /// Product Summary
        /// </summary>
        /// <param name="StoreID">StoreId</param>
        /// <param name="Producttext">Product Text</param>
        /// <returns></returns>
        public DataSet GetVariantFromParent(int StoreID, string Producttext)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetVariantFromParent(StoreID, Producttext);
            return DSProduct;
        }

        /// <summary>
        /// Get Product Summary by order
        /// </summary>
        /// <param name="StoreID">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="ProductSearch">Search Keyword</param>
        /// <param name="VariantName">Varinat Value</param>
        /// <returns></returns>
        public DataSet GetProductSummaryByOrder(int StoreID, string DBAction, string ProductSearch, string VariantName)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductSummaryByOrder(StoreID, DBAction, ProductSearch, VariantName);
            return DSProduct;
        }

        /// <summary>
        /// Get Product Summary by sales channel
        /// </summary>
        /// <param name="DbAction">SP Action</param>
        /// <param name="ProductSearch">Search Keyword</param>
        /// <param name="VariantName">Varinat Value</param>
        /// <returns></returns>
        public DataSet GetProductSummaryBySalesChanel(string DBAction, string ProductSearch, string VariantName)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductSummaryBySalesChanel(DBAction, ProductSearch, VariantName);
            return DSProduct;
        }

        /// <summary>
        /// Get Product Summary By Price
        /// </summary>
        /// <param name="StoreId">Stote Id</param>
        /// <param name="ProductSearch">Product Search</param>
        /// <param name="VariantName">Variant Name</param>
        /// /// <param name="DbAction">SP Action</param>
        /// <returns></returns>
        public DataSet GetProductSummaryByPrice(int StoreID, string ProductSearch, string VariantName, string DBAction)
        {
            AdminDAC dac = new AdminDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductSummaryByPrice(StoreID, ProductSearch, VariantName, DBAction);
            return DSProduct;
        }

        #endregion
        #endregion
    }
}
