using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Entities;
using System.Diagnostics;

namespace Solution.Data
{
    /// <summary>
    /// Admin Data Access Class Contains Admin Related Data Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class AdminDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get admin login username and password
        /// </summary>
        /// <param name="UserName">string UserName</param>
        /// <param name="Password">string Password</param>
        /// <returns>Returns UserName and Password for login</returns>

        public DataSet GetAdminForLogin(string UserName, string Password)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminLogin";
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Password", Password);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get AdminID for MasterAdmin
        /// </summary>
        /// <returns>Returns Details of Admin Master using Id</returns>
        public int GetMasterAdminID()
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_Admin Admin = ctx.tb_Admin.First(a => a.AdminType == 0);
            return Admin.AdminID;
        }

        /// <summary>
        /// Function For Create Admin
        /// </summary>
        /// <param name="objAdmin">tb_Admin objAdmin</param>
        /// <returns>Returns a admin table object that contains created record </returns>
        public tb_Admin Create(tb_Admin objAdmin)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_Admin(objAdmin);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objAdmin;
        }

        /// <summary>
        /// Sub Method for Update Admin
        /// </summary>
        /// <param name="Admin">tb_Admin tb_Admin</param>
        /// <returns>Update Admin Details</returns>
        public void Update(tb_Admin Admin)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;//)

            try
            {
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
        }

        /// <summary>
        /// Get Admin Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns Admin Record for edit fetched by id </returns>
        public tb_Admin getAdmin(int Id)
        {
            tb_Admin Admin = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                Admin = ctx.tb_Admin.FirstOrDefault(e => e.AdminID == Id);
            }
            return Admin;
        }

        /// <summary>
        /// Check Duplicate Record function for Insert and Update
        /// </summary>
        /// <param name="objchkAdmin">tb_Admin objchkAdmin</param>
        /// <returns>Returns no. of record count when specified record is exist in Database</returns>
        public Int32 CheckDuplicate(tb_Admin objchkAdmin)
        {
            Int32 isExists = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExists = (from a in ctx.tb_Admin
                            where a.EmailID == objchkAdmin.EmailID
                            && a.AdminID != objchkAdmin.AdminID
                            && ((System.Boolean?)a.Deleted ?? false) == false
                            select new { a.AdminID }).Count();
            }
            return isExists;
        }

        /// <summary>
        /// For Delete Admin By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public int Delete(int Id)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_Admin adm = ctx.tb_Admin.First(c => c.AdminID == Id);
            ctx.DeleteObject(adm);
            return Convert.ToInt32(ctx.SaveChanges());
        }

        /// <summary>
        /// Get Selected Theme of particular Store
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns selected Theme Name for particular store</returns>
        public string GetTheme(Int32 StoreID)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            objSql = new SQLAccess();
            return Convert.ToString(objSql.ExecuteScalarQuery(@"SELECT dbo.tb_Themes.Color FROM  dbo.tb_Store INNER JOIN  dbo.tb_StoreThemes ON dbo.tb_Store.StoreID = dbo.tb_StoreThemes.StoreID INNER JOIN
                      dbo.tb_Themes ON dbo.tb_StoreThemes.ThemeID = dbo.tb_Themes.ThemeID WHERE dbo.tb_StoreThemes.StoreID=" + StoreID + @""));
        }

        /// <summary>
        /// Get Password By Email Address
        /// </summary>
        /// <param name="Email">String Email</param>
        /// <returns>Returns Password</returns>
        public tb_Admin GetPasswordByEmail(String Email)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_Admin tb_admin = ctx.tb_Admin.FirstOrDefault(c => c.EmailID == Email && c.Deleted == false);
            return tb_admin;
        }

        #endregion

        #region Business Intelligence Report

        /// <summary>
        /// Get All Time Revenue
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeRevenue(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeRevenu";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Time Average Revenue
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeAverageRevenue(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeAverageRevenue";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Time Average Order
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeAverageOrders(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeAverageOrders";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        //GetAllTimeAverageCustomer
        public DataSet GetAllTimeAverageCustomer(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeAverageCustomer";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Time Order Number
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeOrderNumber(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeOrderNumber";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All TIme New Customer
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeNewCustomer(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeNewCustomer";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Customer By Country and state
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="Country">Country</param>
        /// <param name="State">State</param>
        /// <returns></returns>
        public DataSet GetCustomerByCountryState(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate, int Country, string State)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllCustomerByCountry";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@Country", Country);
            cmd.Parameters.AddWithValue("@State", State);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Time Top 25 Products
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeTop25Products(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeTop25Product";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Top 25 Brands list
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeTop25Brands(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeTop25Brand";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Time Top 25 Category
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">Sp Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetAllTimeTop25Category(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeTop25Category";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Order Rewvenue by country
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <param name="Country">Country</param>
        /// <param name="State">State</param>
        /// <returns></returns>
        public DataSet GetOrderRevenue_ByCountry(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate, string Country, string State)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeOrderRevenue_ByCountry";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@Country", Country);
            cmd.Parameters.AddWithValue("@State", State);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All time top 25 selling days
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns>List of Days with sold quantity</returns>
        public DataSet GetTop25SellingDays(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_MaximumSellingDays";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Time Top 3 Month by Revenue
        /// </summary>
        /// <param name="StoreId">Store ID</param>
        /// <param name="DBAction">SP Action</param>
        /// <returns></returns>
        public DataSet GetTop3SellingMonth_ByRevenue(int StoreId, string DbAction)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeTop3SellingMonth_ByRevenu";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Time Top 3 Month By Order
        /// </summary>
        /// <param name="StoreId">Store ID</param>
        /// <param name="DBAction">SP Action</param>
        /// <returns></returns>
        public DataSet GetTop3SellingMonth_ByOrder(int StoreId, string DbAction)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeTop3SellingMonth_ByOrder";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Top 25 Selling Days by order
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <returns></returns>
        public DataSet GedtTop25SellingDaysByOrder(int StoreId, string DbAction)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_Top20SellingDaysByOrder";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Top 3 Selling time duration by Revenue
        /// </summary>
        /// <param name="StoreId">Store ID</param>
        /// <param name="DbAction">DBAction</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns>List of Time Duration with Revenue</returns>
        public DataSet GetAllTimeTop3SellingTime_ByRevenue(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeTop3SellingTime_ByRevenue";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Top 3 Selling time duration by Order
        /// </summary>
        /// <param name="StoreId">Store ID</param>
        /// <param name="DbAction">DBAction</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns>List of Time Duration with Total Order</returns>
        public DataSet GetAllTimeTop3SellingTime_ByOrder(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllTimeTop3SellingTime_ByOrder";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Customer by maximum revenue 
        /// </summary>
        /// <param name="StoreId">Store ID</param>
        /// <param name="DbAction">DBAction</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns>List of Customer with revenue and total order</returns>
        public DataSet GetAllTimeCustomers_Revenue(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllCustomerByRevenue";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get All Customer by maximum Order 
        /// </summary>
        /// <param name="StoreId">Store ID</param>
        /// <param name="DbAction">DBAction</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns>List of Customer with revenue and total order</returns>
        public DataSet GetAllTimeCustomers_ByOrder(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_AllCustomerByOrder";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Store Wise Sales Report
        /// </summary>
        /// <param name="DbAction">DBAction</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns>List of Store with Revenue and Total orders</returns>
        public DataSet GetStoreSalesReport(string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_SalesStoreReport";
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Top 10 State by Review or State
        /// </summary>
        /// <param name="StoreId">Store ID</param>
        /// <param name="DbAction">DBAction</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns>List of Store with Revenue and Total orders</returns>
        /// <param name="OrderBy">Revenue/Order</param>
        /// <returns></returns>
        public DataSet Top10State(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate, string OrderBy)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_Top10StateBy_RevenueOrder";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@OrderBy", OrderBy);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Order Status
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetOrderStatus(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_OrderStatus";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Top 10 review by product
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="StartDate">Start Date</param>
        /// <param name="EndDate">End Date</param>
        /// <returns></returns>
        public DataSet GetTop10ProductByReview(int StoreId, string DbAction, DateTime? StartDate, DateTime? EndDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_Top10Product_ByReview";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Get Varint from parent
        /// </summary>
        /// <param name="StoreId">Store Id</param>
        /// <param name="ProductText">Product Text</param>
        /// <returns></returns>
        public DataSet GetVariantFromParent(int StoreId, string ProductText)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetVariantFromProductSKU";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@ProductText", ProductText);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product Summary by order
        /// </summary>
        /// <param name="StoreId">Stote Id</param>
        /// <param name="DbAction">SP Action</param>
        /// <param name="ProductSearch">Product Search</param>
        /// <param name="VariantName">Variant Name</param>
        /// <returns></returns>
        public DataSet GetProductSummaryByOrder(int StoreId, string DbAction, string ProductSearch, string VariantName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_ProductSummaryByOrder";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@ProductSearch", ProductSearch);
            cmd.Parameters.AddWithValue("@VariantName", VariantName);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product Summary by sales channel
        /// </summary>
        /// <param name="DbAction">SP Action</param>
        /// <param name="ProductSearch">Search Keyword</param>
        /// <param name="VariantName">Varinat Value</param>
        /// <returns></returns>
        public DataSet GetProductSummaryBySalesChanel(string DbAction, string ProductSearch, string VariantName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_ProductSummaryBySaleschannel";
            cmd.Parameters.AddWithValue("@ProductSearch", ProductSearch);
            cmd.Parameters.AddWithValue("@VariantName", VariantName);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product Summary By Price
        /// </summary>
        /// <param name="StoreId">Stote Id</param>
        /// <param name="ProductSearch">Product Search</param>
        /// <param name="VariantName">Variant Name</param>
        /// /// <param name="DbAction">SP Action</param>
        /// <returns></returns>
        public DataSet GetProductSummaryByPrice(int StoreId, string ProductSearch, string VariantName, string DbAction)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_BIReport_ProductSummaryByPriceChange";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@ProductSearch", ProductSearch);
            cmd.Parameters.AddWithValue("@VariantName", VariantName);
            cmd.Parameters.AddWithValue("@DbAction", DbAction);
            return objSql.GetDs(cmd);
        }
        #endregion

    }
}
