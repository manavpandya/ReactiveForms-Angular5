using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Entities;
using System.Diagnostics;


namespace Solution.Data
{
    /// <summary>
    /// Customer Data Access Class Contains Customer related Data Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CustomerDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get CustomerID by UserName
        /// </summary>
        /// <param name="UserName">String UserName</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns CustomerID</returns>
        public Int32 GetCustIDByUserName(String UserName, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Customer Lock out Status by CustomerID and StoreID
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <param name="StoreID">Int32 StoreID</param>        
        /// <returns>Returns Customer Lock out status Data as DataSet</returns>
        public DataSet GetCustomerLockOut(Int32 CustID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get CustomerID by UserName, Password and StoreID
        /// </summary>
        /// <param name="UserName">String UserName</param>
        /// <param name="Password">String Password</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Customer Details as DataSet</returns>
        public DataSet GetCustIDByUserNamePassword(String UserName, String Password, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@UserName", UserName);
            cmd.Parameters.AddWithValue("@Password", Password);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Customer Lock Out Status
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <param name="IsLockOut">Int32 IsLockOut</param>
        /// <param name="FailedCount">Int32 FailedCount</param>
        /// <param name="FailedDate">DateTime FailedDate</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns bool value according to result of query</returns>
        public bool UpdateCustomerLockOut(Int32 CustID, Int32 IsLockOut, Int32 FailedCount, DateTime FailedDate, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustID);
            cmd.Parameters.AddWithValue("@IsLockOut", IsLockOut);
            cmd.Parameters.AddWithValue("@FailedCount", FailedCount);
            cmd.Parameters.AddWithValue("@FailedDate", FailedDate);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 4);
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Ger the Customer Details for Forgot Password
        /// </summary>
        /// <param name="Email">String Email</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the Customer Details</returns>
        public DataSet GetPasswordForForgotPassWord(String Email, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@UserName", Email);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 5);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Customer as a register customer or not
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="flag">Boolean flag</param>
        public void UpdateRegisterFlag(Int32 CustomerID, Boolean flag)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Flag", flag);
            cmd.Parameters.AddWithValue("@Mode", 6);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Update Customer Billing Address
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="BillingAddressID">Int32 BillingAddressID</param>
        /// <returns>Returns True if updated or false when not updated</returns>
        public bool UpdateCustomerBillingAddress(Int32 CustomerID, Int32 BillingAddressID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@BillingAddressID", BillingAddressID);
            cmd.Parameters.AddWithValue("@Mode", 7);
            bool isupdated = Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
            return isupdated;
        }

        /// <summary>
        /// Update Customer Shipping Address
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="ShippingAddressID">Int32 ShippingAddressID</param>
        /// <returns>Returns True if updated or false when not updated</returns>
        public bool UpdateCustomerShippingAddress(Int32 CustomerID, Int32 ShippingAddressID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@ShippingAddressID", ShippingAddressID);
            cmd.Parameters.AddWithValue("@Mode", 8);
            bool isupdated = Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
            return isupdated;
        }

        /// <summary>
        /// Insert Customer 
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Recently Inserted customer identity value</returns>
        public Int32 InsertCustomer(tb_Customer tb_Customer, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_InsertCustomer";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Email", tb_Customer.Email);
            cmd.Parameters.AddWithValue("@Password", tb_Customer.Password);
            cmd.Parameters.AddWithValue("@FirstName", tb_Customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", tb_Customer.LastName);
            cmd.Parameters.AddWithValue("@BillingEqualShippingbit", tb_Customer.BillingEqualShippingbit);
            cmd.Parameters.AddWithValue("@IsRegistered", tb_Customer.IsRegistered);
            cmd.Parameters.AddWithValue("@IsLockedOut", tb_Customer.IsLockedOut);
            cmd.Parameters.AddWithValue("@FailedPasswordAttemptCount", tb_Customer.FailedPasswordAttemptCount);
            cmd.Parameters.AddWithValue("@LastIPAddress", tb_Customer.LastIPAddress);
            cmd.Parameters.AddWithValue("@CustomerLevelID", tb_Customer.CustomerLevelID);
            cmd.Parameters.AddWithValue("@Active", tb_Customer.Active);          
            cmd.Parameters.AddWithValue("@Deleted", tb_Customer.Deleted);
            cmd.Parameters.AddWithValue("@CreatedOn", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@NoDiscount", tb_Customer.NoDiscount);
            cmd.Parameters.AddWithValue("@CouponCode", tb_Customer.CouponCode);
            cmd.Parameters.AddWithValue("@FromDate", tb_Customer.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", tb_Customer.ToDate);
            cmd.Parameters.AddWithValue("@DiscountPercent", tb_Customer.DiscountPercent);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Customer Details
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns 1 = Updated</returns>
        public Int32 UpdateCustomer(tb_Customer tb_Customer)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_InsertCustomer";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CustomerID", tb_Customer.CustomerID);
            cmd.Parameters.AddWithValue("@Email", tb_Customer.Email);
            cmd.Parameters.AddWithValue("@Password", tb_Customer.Password);
            cmd.Parameters.AddWithValue("@FirstName", tb_Customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", tb_Customer.LastName);
            cmd.Parameters.AddWithValue("@BillingEqualShippingbit", tb_Customer.BillingEqualShippingbit);
            cmd.Parameters.AddWithValue("@CustomerLevelID", tb_Customer.CustomerLevelID);
            cmd.Parameters.AddWithValue("@LastIPAddress", tb_Customer.LastIPAddress);
            cmd.Parameters.AddWithValue("@Active", tb_Customer.Active);
            cmd.Parameters.AddWithValue("@Deleted", tb_Customer.Deleted);
            cmd.Parameters.AddWithValue("@IsRegistered", tb_Customer.IsRegistered);
            cmd.Parameters.AddWithValue("@UpdatedOn", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@UpdatedBy", tb_Customer.UpdatedBy);
            cmd.Parameters.AddWithValue("@NoDiscount", tb_Customer.NoDiscount);
            cmd.Parameters.AddWithValue("@Mode", 2);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Customer for Admin
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <returns>Returns 1 = Updated</returns>
        public Int32 UpdateCustomerAdmin(tb_Customer tb_Customer)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_InsertCustomer";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CustomerID", tb_Customer.CustomerID);
            cmd.Parameters.AddWithValue("@Email", tb_Customer.Email);
            cmd.Parameters.AddWithValue("@Password", tb_Customer.Password);
            cmd.Parameters.AddWithValue("@FirstName", tb_Customer.FirstName);
            cmd.Parameters.AddWithValue("@LastName", tb_Customer.LastName);
            cmd.Parameters.AddWithValue("@BillingEqualShippingbit", tb_Customer.BillingEqualShippingbit);
            cmd.Parameters.AddWithValue("@CustomerLevelID", tb_Customer.CustomerLevelID);
            cmd.Parameters.AddWithValue("@LastIPAddress", tb_Customer.LastIPAddress);
            cmd.Parameters.AddWithValue("@Active", tb_Customer.Active);
            cmd.Parameters.AddWithValue("@IsRegistered", tb_Customer.IsRegistered);
            cmd.Parameters.AddWithValue("@UpdatedOn", DateTime.Now.Date);
            cmd.Parameters.AddWithValue("@UpdatedBy", tb_Customer.UpdatedBy);
            cmd.Parameters.AddWithValue("@NoDiscount", tb_Customer.NoDiscount);
            cmd.Parameters.AddWithValue("@CouponCode", tb_Customer.CouponCode);
            cmd.Parameters.AddWithValue("@FromDate", tb_Customer.FromDate);
            cmd.Parameters.AddWithValue("@ToDate", tb_Customer.ToDate);
            cmd.Parameters.AddWithValue("@DiscountPercent", tb_Customer.DiscountPercent);
            cmd.Parameters.AddWithValue("@Mode", 3);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Check Duplicate Customer Entry for Insert and Update
        /// </summary>
        /// <param name="objCustomer">tb_Customer objCustomer</param>
        /// <returns>Returns Count of Duplicate Customer if Exists else returns zero</returns>
        public Int32 CheckDuplicateCustomer(tb_Customer objCustomer)
        {
            Int32 isExists = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(objCustomer.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var results = from a in ctx.tb_Customer
                          select new
                          {
                              cust = a,
                              store = a.tb_Store
                          };

            {
                isExists = (from a in results
                            where a.cust.Email == objCustomer.Email
                            && a.cust.CustomerID != objCustomer.CustomerID
                            && ((System.Boolean?)a.cust.Deleted ?? false) == false
                              && a.store.StoreID == ID && a.cust.Active == true
                            select new { a.cust.CustomerID }).Count();
            }
            return isExists;
        }

        /// <summary>
        /// Insert Customer Address
        /// </summary>
        /// <param name="tb_Address">tb_Address tb_Address</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity value </returns>
        public Int32 InsertCustomerAddress(tb_Address tb_Address, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Address_InsertAddress";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@CustomerID", tb_Address.CustomerID);
            cmd.Parameters.AddWithValue("@FirstName", tb_Address.FirstName);
            cmd.Parameters.AddWithValue("@LastName", tb_Address.LastName);
            cmd.Parameters.AddWithValue("@Company", tb_Address.Company);
            cmd.Parameters.AddWithValue("@Address1", tb_Address.Address1);
            cmd.Parameters.AddWithValue("@Address2", tb_Address.Address2);
            cmd.Parameters.AddWithValue("@City", tb_Address.City);
            cmd.Parameters.AddWithValue("@State", tb_Address.State);
            cmd.Parameters.AddWithValue("@Suite", tb_Address.Suite);
            cmd.Parameters.AddWithValue("@ZipCode", tb_Address.ZipCode);
            cmd.Parameters.AddWithValue("@Country", tb_Address.Country);
            cmd.Parameters.AddWithValue("@Phone", tb_Address.Phone);
            cmd.Parameters.AddWithValue("@Fax", tb_Address.Fax);
            cmd.Parameters.AddWithValue("@Email", tb_Address.Email);
            cmd.Parameters.AddWithValue("@PaymentMethodIDLastUsed", tb_Address.PaymentMethodIDLastUsed);
            cmd.Parameters.AddWithValue("@AddressType", tb_Address.AddressType);
            cmd.Parameters.AddWithValue("@CreatedOn", tb_Address.CreatedOn);
            cmd.Parameters.AddWithValue("@Deleted", tb_Address.Deleted);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Customer Address
        /// </summary>
        /// <param name="tb_Address">tb_Address tb_Address</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 = True </returns>
        public Int32 UpdateCustomerAddress(tb_Address tb_Address, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Address_InsertAddress";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@AddressID", tb_Address.AddressID);
            cmd.Parameters.AddWithValue("@CustomerID", tb_Address.CustomerID);
            cmd.Parameters.AddWithValue("@FirstName", tb_Address.FirstName);
            cmd.Parameters.AddWithValue("@LastName", tb_Address.LastName);
            cmd.Parameters.AddWithValue("@Company", tb_Address.Company);
            cmd.Parameters.AddWithValue("@Address1", tb_Address.Address1);
            cmd.Parameters.AddWithValue("@Address2", tb_Address.Address2);
            cmd.Parameters.AddWithValue("@City", tb_Address.City);
            cmd.Parameters.AddWithValue("@State", tb_Address.State);
            cmd.Parameters.AddWithValue("@Suite", tb_Address.Suite);
            cmd.Parameters.AddWithValue("@ZipCode", tb_Address.ZipCode);
            cmd.Parameters.AddWithValue("@Country", tb_Address.Country);
            cmd.Parameters.AddWithValue("@Phone", tb_Address.Phone);
            cmd.Parameters.AddWithValue("@Fax", tb_Address.Fax);
            cmd.Parameters.AddWithValue("@Email", tb_Address.Email);
            cmd.Parameters.AddWithValue("@AddressType", tb_Address.AddressType);
            cmd.Parameters.AddWithValue("@Mode", 2);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get Email Template
        /// </summary>
        /// <param name="Lable">String Label</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Email Template</returns>
        public DataSet GetEmailTamplate(String Lable, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@Label", Lable);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 9);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Customer Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Customer Record fetched by CustomerID</returns>
        public DataSet GetCustomerDetailByCustID(Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 10);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Customer Detail By CustomerID For Admin Side
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Customer Record fetched by CustomerID</returns>
        public DataSet GetAdminCustomerDetailByCustID(Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 19);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Address Detail By AddressID
        /// </summary>
        /// <param name="AddressID">Int32 AddresID</param>
        /// <returns>Returns Address Record fetched by AddressID</returns>
        public DataSet GetAddressDetailByAddressID(Int32 AddressID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@AddressID", AddressID);
            cmd.Parameters.AddWithValue("@Mode", 11);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Address Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Both Address Record fetched by CustomerID</returns>
        public DataSet GetAddressDetailByCustID(Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 12);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Address Detail By CustomerID and AddressType
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="AddressType">Int32 AddressType</param>
        /// <returns>Returns count of Billing or Shipping Address</returns>
        public Int32 GetAddressDetailByCustIDandAddType(Int32 CustomerID, Int32 AddressType)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@AddressType", AddressType);
            cmd.Parameters.AddWithValue("@Mode", 14);
            return Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Delete Address By AddressID
        /// </summary>
        /// <param name="AddressID">Int32 AddressID</param>
        /// <returns>Returns Boolean value in the form of True or False</returns>
        public bool DeleteAddressByAddressID(Int32 AddressID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@AddressID", AddressID);
            cmd.Parameters.AddWithValue("@Mode", 13);
            bool isDeleted = Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
            return isDeleted;
        }

        /// <summary>
        /// Update Customer Password
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <returns>Returns Status for Returns = 1 then True </returns>
        public bool UpdatePasswordByCustomerID(tb_Customer tb_Customer)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@CustomerID", tb_Customer.CustomerID);
            cmd.Parameters.AddWithValue("@Password", tb_Customer.Password);
            cmd.Parameters.AddWithValue("@HostIPAddrss", tb_Customer.LastIPAddress);
            cmd.Parameters.AddWithValue("@Mode", 15);
            bool isUpdate = Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
            return isUpdate;
        }

        /// <summary>
        /// Get Customer Details By Customer ID
        /// </summary>
        /// <param name="CustomerID">Int32 Customer ID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetCustomerDetails(Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_GetCustomerDetails";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Add Customer After Order Placed
        /// </summary>
        /// <param name="OrderNumber">Int32 Order Number</param>
        /// <param name="Email">String Email</param>
        /// <param name="Password">String Password</param>
        /// <param name="StoreId">Int32 StoreID</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns int value</returns>
        public Int32 AddCustomerAfterorderPlaced(Int32 OrderNumber, string Email, string Password, Int32 StoreId, Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_AddCustomer";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@Email", Email);
            cmd.Parameters.AddWithValue("@password", Password);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@CustomerId", CustomerID);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Check Blocked IPAddress By IPAddress By CustomerID
        /// </summary>
        /// <param name="IPAddress">String IPAddress</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns true if exist else false</returns>
        public bool CheckBlockedIPAddressByIPAddress(String IPAddress, Int32 CustomerID)
        {
            objSql = new SQLAccess();
            bool isExist = false;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@HostIPAddrss", IPAddress);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 16);
            Int32 cnt = Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
            if (cnt > 0)
            {
                isExist = true;
            }
            return isExist;
        }

        /// <summary>
        /// Insert Block IPAddress
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="IPAddress">Int32 IPAddress</param>
        /// <returns>Returns Identity value</returns>
        public Int32 InsertBlockIPAddress(Int32 StoreID, Int32 CustomerID, String IPAddress)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@HostIPAddrss", IPAddress);
            cmd.Parameters.AddWithValue("@StoreId", StoreID);
            cmd.Parameters.AddWithValue("@CustomerId", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 17);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Block IPAddress
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="IPAddress">Int32 IPAddress</param>
        /// <returns>Returns 1 = if Deleted</returns>
        public Int32 DeleteBlockIPAddress(Int32 StoreID, Int32 CustomerID, String IPAddress)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@HostIPAddrss", IPAddress);
            cmd.Parameters.AddWithValue("@StoreId", StoreID);
            cmd.Parameters.AddWithValue("@CustomerId", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 18);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get the Details of customer by CustomerID for Customer List
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns customer data fetched by customerID</returns>
        public tb_Customer GetCustomerDataByID(Int32 CustomerID)
        {
            tb_Customer tb_Customer = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                tb_Customer = ctx.tb_Customer.First(e => e.CustomerID == CustomerID);
            }
            return tb_Customer;
        }

        /// <summary>
        /// Update Customer for Customer List
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        ///  <returns>Returns Updated Customer Status</returns>
        public void UpdateCustomerList(tb_Customer tb_Customer)
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
        /// Delete Customer
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// /// <param name="Val">Int32 Val</param>
        /// <returns>Returns Boolean value in the form of True or False</returns>
        public Int32 DeleteCustomerList(Int32 CustomerID, Int32 Val)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int customerDeleted = 0;
            try
            {
                tb_Customer tb_customer = new tb_Customer();
                tb_customer = ctx.tb_Customer.First(c => c.CustomerID == CustomerID);
                tb_customer.Deleted = Convert.ToBoolean(Val);
                ctx.SaveChanges();
                customerDeleted = tb_customer.CustomerID;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return customerDeleted;
        }

        /// <summary>
        /// Get the Customer Detail by Storeid
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetCustomerDetailsbyStoreid(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_GetAllDetailforcoupon";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get the Customer by Customer name And Store id
        /// </summary>
        /// <param name="StoreID">Int32 Storeid</param>
        /// <param name="Customername">string customername</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCustomerDetailsbysearchvalue(Int32 SId, string customername)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_GetAllDetailforcoupon";
            cmd.Parameters.AddWithValue("@StoreID", SId);
            cmd.Parameters.AddWithValue("@Name", customername);
            cmd.Parameters.AddWithValue("@mode", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Customer For Phone Order List
        /// </summary>
        /// <param name="SId">Int32 SId</param>
        /// <param name="SearchVal">string SearchVal</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCustomerforPhoneOrders(Int32 StoreID, string SearchVal)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_CustomerPhoneOrderList]";
            cmd.Parameters.AddWithValue("@SearchVal", SearchVal);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Customer Billing and Shipping Details
        /// </summary>
        /// <param name="CustId">Int32 CustId</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCustomerAddress(Int32 CustId, Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_Customer_GetCustomerAddressDetails]";
            cmd.Parameters.AddWithValue("@CustomerID", CustId);
            cmd.Parameters.AddWithValue("@StoreID", StoreId);
            return objSql.GetDs(cmd);
        }
        #endregion

        /// <summary>
        /// Get Facebook Customer Detail By Customer email ann StoreId
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Facebook Customer Record</returns>
        public DataSet GetCustomerDetailforFacebookByCustID(String email, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer";
            cmd.Parameters.AddWithValue("@UserName", email);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 20);
            return objSql.GetDs(cmd);
        }

        public bool InsertMembershipDiscount(tb_MembershipDiscount tb_MembershipDiscount)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_MembershipDiscount";
            cmd.Parameters.AddWithValue("@CustID", tb_MembershipDiscount.CustID);
            cmd.Parameters.AddWithValue("@Discount", tb_MembershipDiscount.Discount);
            cmd.Parameters.AddWithValue("@DiscountObjectID",tb_MembershipDiscount. DiscountObjectID);
            cmd.Parameters.AddWithValue("@DiscountType", tb_MembershipDiscount.DiscountType);
            cmd.Parameters.AddWithValue("@CreatedBy", tb_MembershipDiscount.CreatedBy);
            cmd.Parameters.AddWithValue("@UpdatedBy", tb_MembershipDiscount.UpdatedBy);
            cmd.Parameters.AddWithValue("@StoreID", tb_MembershipDiscount.StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);

            bool isupdated = Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
            return isupdated;
        }

        public bool UpdateMembershipDiscount(tb_MembershipDiscount tb_MembershipDiscount)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_MembershipDiscount";
          //  cmd.Parameters.AddWithValue("@MembershipDiscountID", tb_MembershipDiscount.MembershipDiscountID);  
            cmd.Parameters.AddWithValue("@CustID", tb_MembershipDiscount.CustID);
            cmd.Parameters.AddWithValue("@Discount", tb_MembershipDiscount.Discount);
            cmd.Parameters.AddWithValue("@DiscountObjectID", tb_MembershipDiscount.DiscountObjectID);
            cmd.Parameters.AddWithValue("@DiscountType", tb_MembershipDiscount.DiscountType);
            cmd.Parameters.AddWithValue("@CreatedBy", tb_MembershipDiscount.CreatedBy);
            cmd.Parameters.AddWithValue("@UpdatedBy", tb_MembershipDiscount.UpdatedBy);
            cmd.Parameters.AddWithValue("@StoreID", tb_MembershipDiscount.StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);

            bool isupdated = Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
            return isupdated;
        }


        public DataSet GetMembershipDetails(int CustID, string DiscountType,Int32 Storeid, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_MembershipDiscount";
            cmd.Parameters.AddWithValue("@CustID",CustID);
            cmd.Parameters.AddWithValue("@DiscountType",DiscountType);
            cmd.Parameters.AddWithValue("@StoreId", Storeid);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }
    }
}
