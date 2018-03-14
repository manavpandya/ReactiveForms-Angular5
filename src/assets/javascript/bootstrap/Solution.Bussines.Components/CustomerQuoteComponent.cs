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
    /// Customer Quote Component Class Contains Customer Quote related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CustomerQuoteComponent
    {
        #region Declaration

        public static int _count = 0;
        CustomerQuoteDAC objCustomer = null;
        #endregion

        #region  Key Functions

        /// <summary>
        /// Get Products for Customer Quote
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <param name="SearchField">string SearchField</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="OrderField">string OrderField</param>
        /// <param name="Order">string Order</param>
        /// <returns>Returns Products Data for Customer Quote</returns>
        public DataSet GetCustomerQuoteProducts(Int32 StoreID, Int32 Mode, string SearchField, string SearchValue, string OrderField, string Order)
        {
            CustomerQuoteDAC dac = new CustomerQuoteDAC();
            return dac.GetCustomerQuoteProducts(StoreID, Mode, SearchField, SearchValue, OrderField, Order);
        }


        /// <summary>
        /// Insert Customer Quote
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns CustomerQuoteID of inserted customer level</returns>
        public Int32 AddCustomerQuote(Int32 CustomerID, Int32 StoreID, Int32 LoginID)
        {
            CustomerQuoteDAC dac = new CustomerQuoteDAC();
            int CustomerQuoteID = dac.AddCustomerQuote(CustomerID, StoreID, LoginID);
            return CustomerQuoteID;
        }


        /// <summary>
        /// Insert Customer Quote Items
        /// </summary>
        /// <param name="CustomerQuoteID">Int32 CustomerQuoteID</param>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="SKU">String SKU</param>
        /// <param name="Name">String Name</param>
        /// <param name="Options">String Options</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="Quantity">Int32 Quantity</param>
        /// <param name="Notes">String Notes</param>
        /// <returns>Returns Id of Inserted Customer Quote Items</returns>
        public Boolean AddCustomerQuoteItems(Int32 CustomerQuoteID, Int32 ProductID, String SKU, String Name, String Options, Decimal Price, Int32 Quantity, String Notes)
        {
            CustomerQuoteDAC dac = new CustomerQuoteDAC();
            bool isUpdated = dac.AddCustomerQuoteItems(CustomerQuoteID, ProductID, SKU, Name, Options, Price, Quantity, Notes);
            return isUpdated;
        }

        /// <summary>
        /// Insert Customer Quote Revised
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="QuoteNumber">String QuoteNumber</param>
        /// <returns>Returns Id of Customer Quote Revised</returns>
        public Int32 AddCustomerQuoteRevised(Int32 CustomerID, Int32 StoreID, String QuoteNumber)
        {
            CustomerQuoteDAC dac = new CustomerQuoteDAC();
            int CustomerQuoteID = dac.AddCustomerQuoteRevised(CustomerID, StoreID, QuoteNumber);
            return CustomerQuoteID;
        }

        /// <summary>
        /// Get List of Customer Quote
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns Customer Quote Details</returns>
        //public List<ListCustomerQuoteComponent> GetDataByFilter(int startIndex, int pageSize, string sortBy, int pStoreId, string pSearchValue)
        //{
        //    try
        //    {
        //        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        //        List<ListCustomerQuoteComponent> lstCustomerList = new List<ListCustomerQuoteComponent>();
        //        //To join tb_Customer and tb_Store

        //        if (string.IsNullOrEmpty(pSearchValue))
        //        {
        //            pSearchValue = "";
        //        }
        //        if (pStoreId != -1)
        //        {

        //            var results1 = (from a in ctx.tb_CustomerQuote
        //                            join address in ctx.tb_Customer on a.CustomerID equals address.CustomerID
        //                            where (a.QuoteNumber.Contains(pSearchValue) || address.FirstName.Contains(pSearchValue) || address.LastName.Contains(pSearchValue) || address.Email.Contains(pSearchValue) || address.tb_Store.StoreName.Contains(pSearchValue))
        //                        && a.StoreID == pStoreId
        //                        && address.Email != "" && ((System.Boolean?)address.Deleted ?? false) == false
        //                            select new ListCustomerQuoteComponent
        //                            {
        //                                CustomerQuoteID = a.CustomerQuoteID,
        //                                CustomerID = a.CustomerID ?? 0,
        //                                QuoteNumber = a.QuoteNumber,
        //                                OrderNumber = a.OrderNumber ?? 0,
        //                                CreatedOn = a.CreatedOn.Value,
        //                                StoreID = a.StoreID ?? 0,
        //                                IsRevised = a.IsRevised ?? false,
        //                                CustomerName = address.FirstName + " " + address.LastName,
        //                                Email = address.Email

        //                            }).OrderBy(od => od.CustomerID).Distinct();
        //            lstCustomerList = results1.ToList<ListCustomerQuoteComponent>();
        //        }
        //        else
        //        {

        //            var results1 = (from a in ctx.tb_CustomerQuote
        //                            join address in ctx.tb_Customer on a.CustomerID equals address.CustomerID
        //                            where (a.QuoteNumber.Contains(pSearchValue) || address.FirstName.Contains(pSearchValue) || address.LastName.Contains(pSearchValue) || address.Email.Contains(pSearchValue) || address.tb_Store.StoreName.Contains(pSearchValue))
        //                        && address.Email != "" && ((System.Boolean?)address.Deleted ?? false) == false
        //                            select new ListCustomerQuoteComponent
        //                            {
        //                                CustomerQuoteID = a.CustomerQuoteID,
        //                                CustomerID = a.CustomerID ?? 0,
        //                                QuoteNumber = a.QuoteNumber,
        //                                OrderNumber = a.OrderNumber ?? 0,
        //                                CreatedOn = a.CreatedOn.Value,
        //                                StoreID = a.StoreID ?? 0,
        //                                IsRevised = a.IsRevised ?? false,
        //                                CustomerName = address.FirstName + " " + address.LastName,
        //                                Email = address.Email

        //                            }).OrderBy(od => od.CustomerID).Distinct();
        //            lstCustomerList = results1.ToList<ListCustomerQuoteComponent>();
        //        }

        //        //Logic for searching
        //        if (!string.IsNullOrEmpty(sortBy))
        //        {

        //            System.Reflection.PropertyInfo property = lstCustomerList.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

        //            String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        //            if (SortingOption.Length == 1)
        //            {

        //                lstCustomerList = lstCustomerList.OrderBy(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<ListCustomerQuoteComponent>();
        //            }
        //            else if (SortingOption.Length == 2)
        //            {
        //                lstCustomerList = lstCustomerList.OrderByDescending(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<ListCustomerQuoteComponent>();
        //            }
        //        }
        //        else
        //        {
        //            lstCustomerList = lstCustomerList.OrderBy(o => o.CustomerQuoteID).ToList();
        //        }
        //        _count = lstCustomerList.Count;
        //        lstCustomerList = lstCustomerList.Skip(startIndex).Take(pageSize).ToList();
        //        return lstCustomerList;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        public List<ListCustomerQuoteComponent> GetDataByFilter(int startIndex, int pageSize, string sortBy, int pStoreId, string pSearchValue, int ploginid)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                List<ListCustomerQuoteComponent> lstCustomerList = new List<ListCustomerQuoteComponent>();
                //To join tb_Customer and tb_Store

                if (string.IsNullOrEmpty(pSearchValue))
                {
                    pSearchValue = "";
                }
                if (pStoreId != -1)
                {
                    if (ploginid != 0)
                    {
                        var results1 = (from a in ctx.tb_CustomerQuote
                                        join address in ctx.tb_Customer on a.CustomerID equals address.CustomerID
                                        where (a.QuoteNumber.Contains(pSearchValue) || address.FirstName.Contains(pSearchValue) || address.LastName.Contains(pSearchValue) || address.Email.Contains(pSearchValue) || address.tb_Store.StoreName.Contains(pSearchValue))
                                    && a.StoreID == pStoreId && a.CreatedBy == ploginid
                                    && address.Email != "" && ((System.Boolean?)address.Deleted ?? false) == false
                                        select new ListCustomerQuoteComponent
                                        {
                                            CustomerQuoteID = a.CustomerQuoteID,
                                            CustomerID = a.CustomerID ?? 0,
                                            QuoteNumber = a.QuoteNumber,
                                            OrderNumber = a.OrderNumber ?? 0,
                                            CreatedOn = a.CreatedOn.Value,
                                            StoreID = a.StoreID ?? 0,
                                            IsRevised = a.IsRevised ?? false,
                                            CustomerName = address.FirstName + " " + address.LastName,
                                            Email = address.Email,
                                            

                                        }).OrderBy(od => od.CustomerID).Distinct();
                        lstCustomerList = results1.ToList<ListCustomerQuoteComponent>();
                    }
                    else
                    {
                        var results1 = (from a in ctx.tb_CustomerQuote
                                        join address in ctx.tb_Customer on a.CustomerID equals address.CustomerID
                                        join admin in ctx.tb_Admin on a.CreatedBy equals admin.AdminID
                                        where (a.QuoteNumber.Contains(pSearchValue) || address.FirstName.Contains(pSearchValue) || address.LastName.Contains(pSearchValue) || address.Email.Contains(pSearchValue) || address.tb_Store.StoreName.Contains(pSearchValue))
                                    && a.StoreID == pStoreId
                                    && address.Email != "" && ((System.Boolean?)address.Deleted ?? false) == false
                                        select new ListCustomerQuoteComponent
                                        {
                                            CustomerQuoteID = a.CustomerQuoteID,
                                            CustomerID = a.CustomerID ?? 0,
                                            QuoteNumber = a.QuoteNumber,
                                            OrderNumber = a.OrderNumber ?? 0,
                                            CreatedOn = a.CreatedOn.Value,
                                            StoreID = a.StoreID ?? 0,
                                            IsRevised = a.IsRevised ?? false,
                                            CustomerName = address.FirstName + " " + address.LastName,
                                            Email = address.Email,
                                            CreatedName = admin.FirstName + " " + admin.LastName

                                        }).OrderBy(od => od.CustomerID).Distinct();
                        lstCustomerList = results1.ToList<ListCustomerQuoteComponent>();
                    }


                }
                else
                {
                    if (ploginid != 0)
                    {
                        var results1 = (from a in ctx.tb_CustomerQuote
                                        join address in ctx.tb_Customer on a.CustomerID equals address.CustomerID
                                        where (a.QuoteNumber.Contains(pSearchValue) || address.FirstName.Contains(pSearchValue) || address.LastName.Contains(pSearchValue) || address.Email.Contains(pSearchValue) || address.tb_Store.StoreName.Contains(pSearchValue))
                                    && address.Email != "" && ((System.Boolean?)address.Deleted ?? false) == false && a.CreatedBy == ploginid
                                        select new ListCustomerQuoteComponent
                                        {
                                            CustomerQuoteID = a.CustomerQuoteID,
                                            CustomerID = a.CustomerID ?? 0,
                                            QuoteNumber = a.QuoteNumber,
                                            OrderNumber = a.OrderNumber ?? 0,
                                            CreatedOn = a.CreatedOn.Value,
                                            StoreID = a.StoreID ?? 0,
                                            IsRevised = a.IsRevised ?? false,
                                            CustomerName = address.FirstName + " " + address.LastName,
                                            Email = address.Email

                                        }).OrderBy(od => od.CustomerID).Distinct();
                        lstCustomerList = results1.ToList<ListCustomerQuoteComponent>();
                    }
                    else
                    {
                        var results1 = (from a in ctx.tb_CustomerQuote
                                        join address in ctx.tb_Customer on a.CustomerID equals address.CustomerID
                                        join admin in ctx.tb_Admin on a.CreatedBy equals admin.AdminID
                                        where (a.QuoteNumber.Contains(pSearchValue) || address.FirstName.Contains(pSearchValue) || address.LastName.Contains(pSearchValue) || address.Email.Contains(pSearchValue) || address.tb_Store.StoreName.Contains(pSearchValue))
                                    && address.Email != "" && ((System.Boolean?)address.Deleted ?? false) == false
                                        select new ListCustomerQuoteComponent
                                        {
                                            CustomerQuoteID = a.CustomerQuoteID,
                                            CustomerID = a.CustomerID ?? 0,
                                            QuoteNumber = a.QuoteNumber,
                                            OrderNumber = a.OrderNumber ?? 0,
                                            CreatedOn = a.CreatedOn.Value,
                                            StoreID = a.StoreID ?? 0,
                                            IsRevised = a.IsRevised ?? false,
                                            CustomerName = address.FirstName + " " + address.LastName,
                                            Email = address.Email,
                                            CreatedName = admin.FirstName + " " + admin.LastName

                                        }).OrderBy(od => od.CustomerID).Distinct();
                        lstCustomerList = results1.ToList<ListCustomerQuoteComponent>();
                    }
                }

                //Logic for searching
                if (!string.IsNullOrEmpty(sortBy))
                {

                    System.Reflection.PropertyInfo property = lstCustomerList.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

                    String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (SortingOption.Length == 1)
                    {

                        lstCustomerList = lstCustomerList.OrderBy(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<ListCustomerQuoteComponent>();
                    }
                    else if (SortingOption.Length == 2)
                    {
                        lstCustomerList = lstCustomerList.OrderByDescending(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<ListCustomerQuoteComponent>();
                    }
                }
                else
                {
                    lstCustomerList = lstCustomerList.OrderByDescending(o => o.CustomerQuoteID).ToList();
                }
                _count = lstCustomerList.Count;
                lstCustomerList = lstCustomerList.Skip(startIndex).Take(pageSize).ToList();
                return lstCustomerList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// For Delete CustomerQuote By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void delCustomerQuote(int Id)
        {
            CustomerQuoteDAC ObjCustomerQuote = new CustomerQuoteDAC();
            ObjCustomerQuote.Delete(Id);
        }

        /// <summary>
        /// Delete CustomerQuote using CustomerQuoteID
        /// </summary>
        /// <param name="AddressID">Int32 AddressID</param>
        /// <returns>Returns Boolean value in the form of True or False</returns>
        public bool DeleteCustomerQuotesID(Int32 CustomerQuoteID)
        {
            objCustomer = new CustomerQuoteDAC();
            bool isUpdate = objCustomer.DeleteCustomerQuotesID(CustomerQuoteID);
            return isUpdate;
        }

        #endregion

        #region User Defined Class

        /// <summary>
        /// Variable And Properties
        /// </summary>
        public class ListCustomerQuoteComponent
        {
            private Int32 _CustomerQuoteID;
            private Int32 _CustomerID;
            private DateTime _CreatedOn;
            private Int32 _OrderNumber;
            private Int32 _CustomerQuoteItemID;
            private Int32 _ProductID;
            private Int32 _StoreID;
            private String _Name;
            private String _Notes;
            private String _Options;
            private Int32 _Quantity;
            private Decimal _Price;
            private String _QuoteNumber;
            private bool _IsRevised;
            private string _CustomerName;
            private string _CreatedName;

            private string _Email;

            #region Property Section
            public string Email
            {
                get { return _Email; }
                set { _Email = value; }
            }

            public string CustomerName
            {
                get { return _CustomerName; }
                set { _CustomerName = value; }
            }
            public string CreatedName
            {
                get { return _CreatedName; }
                set { _CreatedName = value; }
            }
            public Int32 CustomerQuoteID
            {
                get { return _CustomerQuoteID; }
                set { _CustomerQuoteID = value; }

            }
            public Int32 CustomerID
            {
                get { return _CustomerID; }
                set { _CustomerID = value; }

            }
            public String QuoteNumber
            {
                get { return _QuoteNumber; }
                set { _QuoteNumber = value; }
            }
            public Boolean IsRevised
            {
                get { return _IsRevised; }
                set { _IsRevised = value; }
            }
            public DateTime CreatedOn
            {
                get { return _CreatedOn; }
                set { _CreatedOn = value; }

            }
            public Int32 OrderNumber
            {
                get { return _OrderNumber; }
                set { _OrderNumber = value; }
            }
            public Int32 CustomerQuoteItemID
            {
                get { return _CustomerQuoteItemID; }
                set { _CustomerQuoteItemID = value; }
            }
            public Int32 ProductID
            {
                get { return _ProductID; }
                set { _ProductID = value; }
            }
            public Int32 StoreID
            {
                get { return _StoreID; }
                set { _StoreID = value; }
            }
            public String Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
            public String Notes
            {
                get { return _Notes; }
                set { _Notes = value; }
            }
            public String Options
            {
                get { return _Options; }
                set { _Options = value; }
            }
            public Int32 Quantity
            {
                get { return _Quantity; }
                set { _Quantity = value; }
            }
            public Decimal Price
            {
                get { return _Price; }
                set { _Price = value; }

            }

            #endregion
        }
        #endregion

        #region Get Property Value

        /// <summary>
        /// Get Object Properties
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns Object Properties</returns>
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
        /// <returns>Returns Total Count Records</returns>
        public int GetCount(int pStoreId, string pSearchValue, int ploginid)
        {
            return _count;
        }
        #endregion
    }
}
