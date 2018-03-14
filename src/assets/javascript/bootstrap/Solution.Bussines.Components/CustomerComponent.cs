using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using Solution.Bussines.Components.Common;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Customer Component Class Contains Customer related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CustomerComponent
    {
        #region Declaration

        CustomerDAC objCustomer = null;
        public static int _count = 0;
        #endregion

        #region All Constructors
        /// <summary>
        /// Constructor with out parameter that initializes StoreID when component file is loading
        /// </summary>
        #endregion

        #region  Key Function

        /// <summary>
        /// Get CustomerID by UserName
        /// </summary>
        /// <param name="UserName">String UserName</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns CustomerID</returns>
        public Int32 GetCustIDByUserName(String UserName, int StoreID)
        {
            objCustomer = new CustomerDAC();
            int CustID = objCustomer.GetCustIDByUserName(UserName, StoreID);
            return CustID;
        }

        /// <summary>
        /// Get Customer Lock out Status by CustomerID and StoreID
        /// </summary>
        /// <param name="CurrentCustomerID">Int32 CurrentCustomerID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Customer Lock out status Data as DataSet</returns>
        public DataSet GetCustomerLockOut(Int32 CurrentCustomerID, int StoreID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetCustomerLockOut(CurrentCustomerID, StoreID);
            return DSCustomer;
        }

        /// <summary>
        /// Get CustomerID by UserName, Password and StoreID
        /// </summary>
        /// <param name="UserName">String UserName</param>
        /// <param name="Password">String Password</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Customer Details as DataSet</returns>
        public DataSet GetCustIDByUserNamePassword(String UserName, String Password, int StoreID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSUserName = new DataSet();
            DSUserName = objCustomer.GetCustIDByUserNamePassword(UserName, Password, StoreID);
            return DSUserName;
        }

        /// <summary>
        /// Update Customer Lock Out Status
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <param name="IsLockOut">Int32 IsLockOut</param>
        /// <param name="FailedCount">Int32 FailedCount</param>
        /// <param name="FailedDate">DateTime FailedDate</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns bool value according to result of query</returns>
        public bool UpdateCustomerLockOut(Int32 CustID, int IsLockOut, Int32 FailedCount, DateTime FailedDate, int StoreID)
        {
            objCustomer = new CustomerDAC();
            bool isUpdated = objCustomer.UpdateCustomerLockOut(CustID, IsLockOut, FailedCount, FailedDate, StoreID);
            return isUpdated;
        }

        /// <summary>
        /// Ger the Customer Details for Forgot Password
        /// </summary>
        /// <param name="Email">String Email</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Customer Details</returns>
        public DataSet GetPasswordForForgotPassWord(String Email, int StoreID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSPassword = new DataSet();
            DSPassword = objCustomer.GetPasswordForForgotPassWord(Email, StoreID);
            return DSPassword;
        }

        /// <summary>
        /// Update Customer as a register customer or not
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="flag">Boolean flag</param>
        public void UpdateRegisterFlag(Int32 CustomerID, Boolean flag)
        {
            objCustomer = new CustomerDAC();
            objCustomer.UpdateRegisterFlag(CustomerID, flag);
        }

        /// <summary>
        /// Insert Customer for Client
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Recently Inserted customer identity value</returns>
        public Int32 InsertCustomer(tb_Customer tb_Customer, int StoreID)
        {
            int isAdded = 0;
            objCustomer = new CustomerDAC();
            tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreID);
            try
            {
                if (objCustomer.CheckDuplicateCustomer(tb_Customer) == 0)
                {
                    objCustomer = new CustomerDAC();
                    isAdded = objCustomer.InsertCustomer(tb_Customer, StoreID);
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
        /// Insert Customer for Admin
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Value of Recently Inserted Customer</returns>
        public Int32 InsertCustomerForAdmin(tb_Customer tb_Customer, Int32 StoreID)
        {
            int isAdded = 0;
            objCustomer = new CustomerDAC();
            tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreID);
            try
            {
                if (objCustomer.CheckDuplicateCustomer(tb_Customer) == 0)
                {
                    objCustomer = new CustomerDAC();
                    isAdded = objCustomer.InsertCustomer(tb_Customer, StoreID);
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
        /// Update Customer Billing Address
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="BillingAddressID">Int32 BillingAddressID</param>
        /// <returns>Returns True if updated or false when not updated</returns>
        public bool UpdateCustomerBillingAddress(Int32 CustomerID, Int32 BillingAddressID)
        {
            objCustomer = new CustomerDAC();
            bool isupdated = objCustomer.UpdateCustomerBillingAddress(CustomerID, BillingAddressID);
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
            objCustomer = new CustomerDAC();
            bool isupdated = objCustomer.UpdateCustomerShippingAddress(CustomerID, ShippingAddressID);
            return isupdated;
        }

        /// <summary>
        /// Insert Customer Address
        /// </summary>
        /// <param name="tb_Address">tb_Address tb_Address</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Identity value</returns>
        public Int32 InsertCustomerAddress(tb_Address tb_Address, int StoreID)
        {
            int isAdded = 0;
            try
            {
                objCustomer = new CustomerDAC();
                isAdded = objCustomer.InsertCustomerAddress(tb_Address, StoreID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Update Customer Address
        /// </summary>
        /// <param name="tb_Address">tb_Address tb_Address</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns 1 = True</returns>
        public Int32 UpdateCustomerAddress(tb_Address tb_Address, int StoreID)
        {
            int isUpdated = 0;
            try
            {
                objCustomer = new CustomerDAC();
                isUpdated = objCustomer.UpdateCustomerAddress(tb_Address, StoreID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Get Email Template 
        /// </summary>
        /// <param name="Lable">String Label</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Email Template</returns>
        public DataSet GetEmailTamplate(String Lable, int StoreID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSEmail = new DataSet();
            DSEmail = objCustomer.GetEmailTamplate(Lable, StoreID);
            return DSEmail;
        }

        /// <summary>
        /// Get Customer Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Customer Record fetched by CustomerID</returns>
        public DataSet GetCustomerDetailByCustID(Int32 CustomerID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetCustomerDetailByCustID(CustomerID);
            return DSCustomer;
        }

        /// <summary>
        /// Get Customer Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Customer Record fetched by CustomerID</returns>
        public DataSet GetAdminCustomerDetailByCustID(Int32 CustomerID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetAdminCustomerDetailByCustID(CustomerID);
            return DSCustomer;
        }

        /// <summary>
        /// Get Address Detail By AddressID
        /// </summary>
        /// <param name="AddressID">Int32 AddresID</param>
        /// <returns>Returns Address Record fetched by AddressID</returns>
        public DataSet GetAddressDetailByAddressID(Int32 AddressID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSAddress = new DataSet();
            DSAddress = objCustomer.GetAddressDetailByAddressID(AddressID);
            return DSAddress;
        }

        /// <summary>
        /// Get Address Detail By CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Both Address Record fetched by CustomerID</returns>
        public DataSet GetAddressDetailByCustID(Int32 CustomerID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSAddress = new DataSet();
            DSAddress = objCustomer.GetAddressDetailByCustID(CustomerID);
            return DSAddress;
        }

        /// <summary>
        /// Delete Address By AddressID
        /// </summary>
        /// <param name="AddressID">Int32 AddressID</param>
        /// <returns>Returns Boolean value in the form of True or False</returns>
        public bool DeleteAddressByAddressID(Int32 AddressID)
        {
            objCustomer = new CustomerDAC();
            bool isUpdate = objCustomer.DeleteAddressByAddressID(AddressID);
            return isUpdate;
        }

        /// <summary>
        /// Get Address Detail By CustomerID and AddressType
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="AddressType">Int32 AddressType</param>
        /// <returns>Returns count of Billing or Shipping Address</returns>
        public Int32 GetAddressDetailByCustIDandAddType(Int32 CustomerID, Int32 AddressType)
        {
            objCustomer = new CustomerDAC();
            Int32 isCount = objCustomer.GetAddressDetailByCustIDandAddType(CustomerID, AddressType);
            return isCount;
        }

        /// <summary>
        /// Update Customer Password
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <returns>Returns Status for Returns = 1 then True </returns>
        public bool UpdatePasswordByCustomerID(tb_Customer tb_Customer)
        {
            objCustomer = new CustomerDAC();
            bool isUpdate = objCustomer.UpdatePasswordByCustomerID(tb_Customer);
            return isUpdate;
        }

        /// <summary>
        /// Get Customer Details By Customer ID
        /// </summary>
        /// <param name="CustomerID">Int32 Customer ID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetCustomerDetails(Int32 CustomerID)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetCustomerDetails(CustomerID);
            return DSCustomer;
        }

        /// <summary>
        /// Add Customer After Order Placed
        /// </summary>
        /// <param name="OrderNumber">Int32 Order Number</param>
        /// <param name="Email">String Email</param>
        /// <param name="Password">String Password</param>
        /// <param name="StoreId">Int32 StoreID</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns Int Value</returns>
        public Int32 AddCustomerAfterorderPlaced(Int32 OrderNumber, string Email, string Password, Int32 StoreId, Int32 CustomerID)
        {
            objCustomer = new CustomerDAC();
            return objCustomer.AddCustomerAfterorderPlaced(OrderNumber, Email, Password, StoreId, CustomerID);
        }

        /// <summary>
        /// Check Blocked IPAddress By IPAddress 
        /// </summary>
        /// <param name="IPAddress">String IPAddress</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns true if exist else false</returns>
        public bool CheckBlockedIPAddressByIPAddress(String IPAddress, Int32 CustomerID)
        {
            objCustomer = new CustomerDAC();
            return objCustomer.CheckBlockedIPAddressByIPAddress(IPAddress, CustomerID);
        }

        /// <summary>
        /// Update Customer Details
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns 1 = Updated</returns>
        public Int32 UpdateCustomer(tb_Customer tb_Customer, int StoreID)
        {
            int isUpdated = 0;
            objCustomer = new CustomerDAC();
            tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreID);
            try
            {
                if (objCustomer.CheckDuplicateCustomer(tb_Customer) == 0)
                {
                    objCustomer = new CustomerDAC();
                    return objCustomer.UpdateCustomer(tb_Customer);
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
        /// Update Customer Details for Admin
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns 1 = Updated</returns>
        public Int32 UpdateCustomerAdmin(tb_Customer tb_Customer, int StoreID)
        {
            int isUpdated = 0;
            objCustomer = new CustomerDAC();
            tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreID);
            try
            {
                if (objCustomer.CheckDuplicateCustomer(tb_Customer) == 0)
                {
                    objCustomer = new CustomerDAC();
                    return objCustomer.UpdateCustomerAdmin(tb_Customer);
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
        /// Insert Block IPAddress
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="IPAddress">Int32 IPAddress</param>
        /// <returns>Returns Identity value</returns>
        public Int32 InsertBlockIPAddress(Int32 StoreID, Int32 CustomerID, String IPAddress)
        {
            objCustomer = new CustomerDAC();
            return objCustomer.InsertBlockIPAddress(StoreID, CustomerID, IPAddress);
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
            objCustomer = new CustomerDAC();
            return objCustomer.DeleteBlockIPAddress(StoreID, CustomerID, IPAddress);
        }

        #region Functions for Admin Side CustomerList Page

        /// <summary>
        /// Get Customer Details by Filter
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns Customer Details</returns>
        public List<ListCustomer> GetDataByFilter(int startIndex, int pageSize, string sortBy, int pStoreId, string pSearchValue, string pSearchemail, string pSearchzipcode, string pSearchphone, Boolean ppostback)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                List<ListCustomer> lstCustomerList = new List<ListCustomer>();
                if (string.IsNullOrEmpty(pSearchValue))
                {
                    pSearchValue = "";
                }
                if (string.IsNullOrEmpty(pSearchemail))
                {
                    pSearchemail = "";
                }

                if (string.IsNullOrEmpty(pSearchzipcode))
                {
                    pSearchzipcode = "";
                }

                if (string.IsNullOrEmpty(pSearchphone))
                {
                    pSearchphone = "";
                }
                //new {x=a.BillingAddressID,y=a.ShippingAddressID} equals new {x=address.AddressID,y=address.AddressID} 
                if (ppostback == true)
                {


                    if (pStoreId != -1)
                    {
                        var results1 = (from a in ctx.tb_Customer
                                        join address in ctx.tb_Address on a.BillingAddressID equals address.AddressID into JoindCustomer
                                        join address1 in ctx.tb_Address on a.ShippingAddressID equals address1.AddressID into JoindCustomer1
                                        from joinedData in JoindCustomer.DefaultIfEmpty()
                                        from joinedData1 in JoindCustomer1.DefaultIfEmpty()
                                        let fullnamef = a.FirstName + " " + a.LastName
                                        let fullnamebillf = joinedData.FirstName + " " + joinedData.LastName
                                        let fullnamel = a.LastName + " " + a.FirstName
                                        let fullnamebilll = joinedData.LastName + " " + joinedData.FirstName
                                        let fullnameshipf = joinedData1.FirstName + " " + joinedData1.LastName
                                        let fullnameshipl = joinedData1.LastName + " " + joinedData1.FirstName


                                        where ((fullnamef.Contains(pSearchValue) || fullnamebillf.Contains(pSearchValue) || fullnameshipf.Contains(pSearchValue) || fullnamel.Contains(pSearchValue) || fullnamebilll.Contains(pSearchValue) || fullnameshipl.Contains(pSearchValue) || a.FirstName.Contains(pSearchValue) || a.LastName.Contains(pSearchValue) || joinedData.FirstName.Contains(pSearchValue) || joinedData.LastName.Contains(pSearchValue)) && (a.Email.Contains(pSearchemail) || joinedData.Email.Contains(pSearchemail)) && (joinedData.ZipCode ?? "").Contains(pSearchzipcode) && (joinedData.Phone ?? "").Contains(pSearchphone))
                                        && a.tb_Store.StoreID == pStoreId
                                         && a.BillingAddressID != 0
                                         && a.ShippingAddressID != 0
                                        && a.Email != "" && ((System.Boolean?)a.Deleted ?? false) == false
                                        select new ListCustomer
                                        {
                                            CustomerID = a.CustomerID,
                                            StoreID = a.tb_Store.StoreID,
                                            StoreName = a.tb_Store.StoreName,
                                            CustomerName = a.FirstName + " " + a.LastName,
                                            Email = a.Email,
                                            Active = a.Active ?? false,
                                            IsRegistered = a.IsRegistered ?? false,
                                            Deleted = a.Deleted ?? false
                                        }).OrderBy(od => od.CustomerID).Distinct();
                        lstCustomerList = results1.ToList<ListCustomer>();
                    }
                    else
                    {
                        var results1 = (from a in ctx.tb_Customer
                                        join address in ctx.tb_Address on a.BillingAddressID equals address.AddressID into JoindCustomer
                                        join address1 in ctx.tb_Address on a.ShippingAddressID equals address1.AddressID into JoindCustomer1
                                        from joinedData in JoindCustomer.DefaultIfEmpty()
                                        from joinedData1 in JoindCustomer1.DefaultIfEmpty()
                                        let fullnamef = a.FirstName + " " + a.LastName
                                        let fullnamebillf = joinedData.FirstName + " " + joinedData.LastName
                                        let fullnamel = a.LastName + " " + a.FirstName
                                        let fullnamebilll = joinedData.LastName + " " + joinedData.FirstName
                                        let fullnameshipf = joinedData1.FirstName + " " + joinedData1.LastName
                                        let fullnameshipl = joinedData1.LastName + " " + joinedData1.FirstName
                                        where ((fullnamef.Contains(pSearchValue) || fullnamebillf.Contains(pSearchValue) || fullnameshipf.Contains(pSearchValue) || fullnamel.Contains(pSearchValue) || fullnamebilll.Contains(pSearchValue) || fullnameshipl.Contains(pSearchValue) || a.FirstName.Contains(pSearchValue) || a.LastName.Contains(pSearchValue) || joinedData.FirstName.Contains(pSearchValue) || joinedData.LastName.Contains(pSearchValue)) && (a.Email.Contains(pSearchemail) || joinedData.Email.Contains(pSearchemail)) && (joinedData.ZipCode ?? "").Contains(pSearchzipcode) && (joinedData.Phone ?? "").Contains(pSearchphone))
                                        && a.Email != "" && a.BillingAddressID != 0 && a.ShippingAddressID != 0 && ((System.Boolean?)a.Deleted ?? false) == false
                                        select new ListCustomer
                                        {
                                            CustomerID = a.CustomerID,
                                            StoreID = a.tb_Store.StoreID,
                                            StoreName = a.tb_Store.StoreName,
                                            CustomerName = a.FirstName + " " + a.LastName,
                                            Email = a.Email,
                                            Active = a.Active ?? false,
                                            IsRegistered = a.IsRegistered ?? false,
                                            Deleted = a.Deleted ?? false
                                        }).OrderBy(od => od.CustomerID).Distinct();
                        lstCustomerList = results1.ToList<ListCustomer>();
                        
                    
                    }

                    //Logic for searching
                    if (!string.IsNullOrEmpty(sortBy))
                    {

                        System.Reflection.PropertyInfo property = lstCustomerList.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

                        String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                        if (SortingOption.Length == 1)
                        {

                            lstCustomerList = lstCustomerList.OrderBy(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<ListCustomer>();
                        }
                        else if (SortingOption.Length == 2)
                        {
                            lstCustomerList = lstCustomerList.OrderByDescending(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<ListCustomer>();
                        }
                    }
                    else
                    {
                        lstCustomerList = lstCustomerList.OrderBy(o => o.CustomerName).ToList();
                    }
                    _count = lstCustomerList.Count;
                    lstCustomerList = lstCustomerList.Skip(startIndex).Take(pageSize).ToList();
                }
                return lstCustomerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Get Property Value

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns object</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }
        #endregion

        #region Get Count
        /// <summary>
        /// Get Total Count
        /// </summary>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns Total Record Count</returns>
        public int GetCount(int pStoreId, string pSearchValue, string pSearchemail, string pSearchzipcode, string pSearchphone, Boolean ppostback)
        {
            return _count;
        }
        #endregion

        /// <summary>
        /// Get the Details of customer by CustomerID for Customer List
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <returns>Returns customer data fetched by customerID</returns>
        public tb_Customer GetCustomerDataByID(Int32 CustomerID)
        {
            objCustomer = new CustomerDAC();
            return objCustomer.GetCustomerDataByID(CustomerID);
        }

        /// <summary>
        /// Update Customer for Customer List
        /// </summary>
        /// <param name="tb_Customer">tb_Customer tb_Customer</param>
        /// <returns>Returns Updated Customer Status</returns>
        public Int32 UpdateCustomerList(tb_Customer tb_Customer)
        {
            Int32 isUpdated = 0;
            try
            {
                objCustomer = new CustomerDAC();
                objCustomer.UpdateCustomerList(tb_Customer);
                isUpdated = tb_Customer.CustomerID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Delete Customer
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Val">Int32 Val</param>
        /// <returns>Returns Boolean value in the form of True or False</returns>
        public Int32 DeleteCustomerList(Int32 CustomerID, Int32 Val)
        {
            Int32 isUpdated = 0;
            try
            {
                objCustomer = new CustomerDAC();
                isUpdated = objCustomer.DeleteCustomerList(CustomerID, Val);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        #endregion

        /// <summary>
        /// Get the Customer Detail by StoreID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetCustomerDetailsbyStoreid(Int32 StoreID)
        {
            objCustomer = new CustomerDAC();
            return objCustomer.GetCustomerDetailsbyStoreid(StoreID);
        }

        /// <summary>
        /// Get the Customer by Customer name And Store id
        /// </summary>
        /// <param name="StoreID">Int32 Storeid</param>
        /// <param name="Customername">string customername</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCustomerDetailsbysearchvalue(Int32 StoreID, string Customername)
        {
            objCustomer = new CustomerDAC();
            return objCustomer.GetCustomerDetailsbysearchvalue(StoreID, Customername);
        }

        /// <summary>
        /// Get Customer For Phone Order List
        /// </summary>
        /// <param name="SId">Int32 SId</param>
        /// <param name="SearchVal">string SearchVal</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCustomerforPhoneOrder(Int32 SId, string SearchVal)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetCustomerforPhoneOrders(SId, SearchVal);
            return DSCustomer;
        }

        /// <summary>
        /// Get Customer Billing and Shipping Details
        /// </summary>
        /// <param name="CustId">Int32 CustId</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCustomerBillingandShippingDetails(Int32 CustId, Int32 StoreId)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetCustomerAddress(CustId, StoreId);
            return DSCustomer;
        }

        /// <summary>
        /// Get Customer Details
        /// </summary>
        /// <param name="CustId">Int32 CustId</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Dataset</returns>
        /// <returns>Returns Int32</returns>
        public Int32 GetBlankDetailsofCustomer(Int32 CustId, Int32 StoreId)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetCustomerAddress(CustId, StoreId);
            if (DSCustomer != null && DSCustomer.Tables.Count > 0 && DSCustomer.Tables[0].Rows.Count > 0)
            {
                if (DSCustomer.Tables[0].Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(DSCustomer.Tables[0].Rows[0]["Country"].ToString()) || string.IsNullOrEmpty(DSCustomer.Tables[0].Rows[0]["City"].ToString()) || string.IsNullOrEmpty(DSCustomer.Tables[0].Rows[0]["State"].ToString()) || string.IsNullOrEmpty(DSCustomer.Tables[0].Rows[0]["ZipCode"].ToString()))
                    {
                        return 0;
                    }
                }
                if (DSCustomer.Tables[1].Rows.Count > 0)
                {
                    if (string.IsNullOrEmpty(DSCustomer.Tables[1].Rows[0]["Country"].ToString()) || string.IsNullOrEmpty(DSCustomer.Tables[1].Rows[0]["City"].ToString()) || string.IsNullOrEmpty(DSCustomer.Tables[1].Rows[0]["State"].ToString()) || string.IsNullOrEmpty(DSCustomer.Tables[1].Rows[0]["ZipCode"].ToString()))
                    {
                        return 0;
                    }
                }
            }
            else
            {
                return 0;
            }
            return 1;
        }

        /// <summary>
        /// Get Facebook Customer Detail By Customer email ann StoreId
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Facebook Customer Record</returns>
        public DataSet GetCustomerforFacebookByCustID(String email, Int32 StoreId)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetCustomerDetailforFacebookByCustID(email, StoreId);
            return DSCustomer;
        }

        public bool InsertMembershipDiscount(tb_MembershipDiscount tb_MembershipDiscount)
        {
            objCustomer = new CustomerDAC();
            return objCustomer.InsertMembershipDiscount(tb_MembershipDiscount);
        }

        public bool UpdateMembershipDiscount(tb_MembershipDiscount tb_MembershipDiscount)
        {
            objCustomer = new CustomerDAC();
            return objCustomer.UpdateMembershipDiscount(tb_MembershipDiscount);
        }

        public DataSet GetMembershipDetails(int CustID, string DiscountType,Int32 Storeid, Int32 Mode)
        {
            objCustomer = new CustomerDAC();
            DataSet DSCustomer = new DataSet();
            DSCustomer = objCustomer.GetMembershipDetails(CustID, DiscountType,Storeid, Mode);
            return DSCustomer;
        }
        #endregion

        #region User Defined Class

        /// <summary>
        /// Properties
        /// </summary>
        public class ListCustomer
        {
            private int _CustomerID;
            public int CustomerID
            {
                get { return _CustomerID; }
                set { _CustomerID = value; }
            }

            private int _StoreID;
            public int StoreID
            {
                get { return _StoreID; }
                set { _StoreID = value; }
            }

            private string _StoreName;
            public string StoreName
            {
                get { return _StoreName; }
                set { _StoreName = value; }
            }

            private string _CustomerName;
            public string CustomerName
            {
                get { return _CustomerName; }
                set { _CustomerName = value; }
            }

            private string _Email;
            public string Email
            {
                get { return _Email; }
                set { _Email = value; }
            }

            private bool _Active;
            public bool Active
            {
                get { return _Active; }
                set { _Active = value; }
            }

            private bool _IsRegistered;
            public bool IsRegistered
            {
                get { return _IsRegistered; }
                set { _IsRegistered = value; }
            }

            private bool _Deleted;
            public bool Deleted
            {
                get { return _Deleted; }
                set { _Deleted = value; }
            }
        }
        #endregion

    }
}
