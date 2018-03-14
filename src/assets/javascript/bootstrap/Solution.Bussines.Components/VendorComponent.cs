using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Data;
using System.Collections;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Vendor Component Class Contains Vendor related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class VendorComponent
    {
        #region Declaration
        private static int _count;
        private static int _count1;
        #endregion

        #region User Defined Class

        /// <summary>
        /// Properties
        /// </summary>
        public class ListVendorSKU
        {
            private int _VendorSKUID;
            public int VendorSKUID
            {
                get { return _VendorSKUID; }
                set { _VendorSKUID = value; }
            }

            private int _VendorID;
            public int VendorID
            {
                get { return _VendorID; }
                set { _VendorID = value; }
            }

            private string _VendorSKU;
            public string VendorSKU
            {
                get { return _VendorSKU; }
                set { _VendorSKU = value; }
            }

            private string _ProductName;
            public string ProductName
            {
                get { return _ProductName; }
                set { _ProductName = value; }
            }

            private int _CreatedBy;
            public int CreatedBy
            {
                get { return _CreatedBy; }
                set { _CreatedBy = value; }
            }

            private DateTime _CreatedOn;
            public DateTime CreatedOn
            {
                get { return _CreatedOn; }
                set { _CreatedOn = value; }
            }

            private String _Name;
            public String Name
            {
                get { return _Name; }
                set { _Name = value; }
            }

        }
        #endregion
        #region KeyFunctions
        /// <summary>
        /// Get data to Fill grid
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns IQueryable</returns>
        public static IQueryable<tb_Vendor> GetDataByFilter(int startIndex, int pageSize, string sortBy, string SearchValue)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            if (string.IsNullOrEmpty(SearchValue))
            {
                SearchValue = "";
            }
            else
            {
                SearchValue = SearchValue.Trim();
            }
            IQueryable<tb_Vendor> result = from vendor in ctx.tb_Vendor
                                           where vendor.Deleted == false &&
                                           vendor.Name.Contains(SearchValue)
                                           select vendor;
            _count = result.Count();

            //Logic for searching
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = result.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    result = result.OrderByField(SortingOption[0].ToString(), true);
                }
                else if (SortingOption.Length == 2)
                {
                    result = result.OrderByField(SortingOption[0].ToString(), false);
                }
            }
            else
            {
                //Default sorting by Name
                result = result.OrderBy(o => o.Name);
            }
            result = result.Skip(startIndex).Take(pageSize);
            return result;
        }

        //Client Side Functions
        /// <summary>
        /// Get Data of All  Vendor
        /// </summary>
        /// <returns>Returns All Vendor Details</returns>
        public DataSet GetAllVendor()
        {
            VendorDAC objVendor = new VendorDAC();
            DataSet DSCountry = new DataSet();
            DSCountry = objVendor.GetAllVendor();
            return DSCountry;
        }



        /// <summary>
        /// Get Vendor SKU data to Fill grid
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns IQueryable</returns>
        public List<ListVendorSKU> GetVendorSKUDataByFilter(int startIndex, int pageSize, string sortBy, string SearchValue)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                List<ListVendorSKU> lstVendorSKU = new List<ListVendorSKU>();
                if (string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = "";
                }
                else
                {
                    SearchValue = SearchValue.Trim();
                }
                var results1 = (from vendorsku in ctx.tb_VendorSKU
                                join vendor in ctx.tb_Vendor on vendorsku.VendorID equals vendor.VendorID
                                where vendorsku.Deleted == false && (vendorsku.VendorSKU.Contains(SearchValue) || vendor.Name.Contains(SearchValue) || vendorsku.ProductName.Contains(SearchValue))
                                select new ListVendorSKU
                                {
                                    VendorSKUID = vendorsku.VendorSKUID,
                                    Name = vendor.Name,
                                    VendorSKU = vendorsku.VendorSKU,
                                    ProductName = vendorsku.ProductName
                                }).OrderBy(od => od.VendorSKUID).Distinct();

                lstVendorSKU = results1.ToList<ListVendorSKU>();
                //Logic for searching
                if (!string.IsNullOrEmpty(sortBy))
                {

                    System.Reflection.PropertyInfo property = lstVendorSKU.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

                    String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (SortingOption.Length == 1)
                    {

                        lstVendorSKU = lstVendorSKU.OrderBy(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<ListVendorSKU>();
                    }
                    else if (SortingOption.Length == 2)
                    {
                        lstVendorSKU = lstVendorSKU.OrderByDescending(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<ListVendorSKU>();
                    }
                }
                else
                {
                    lstVendorSKU = lstVendorSKU.OrderBy(o => o.Name).ToList();
                }
                _count = lstVendorSKU.Count;
                lstVendorSKU = lstVendorSKU.Skip(startIndex).Take(pageSize).ToList();
                return lstVendorSKU;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



        /// <summary>
        /// To count total number of record that are used for paging
        /// </summary>
        /// <param name="SearchValue">Search Value</param>
        /// <returns>Total no of records</returns>
        public static int GetCount(string SearchValue)
        {
            return _count;
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


        /// <summary>
        /// Add new Vendor
        /// </summary>
        /// <param name="vendor">tb_Vendor vendor</param>
        /// <returns>ID of created Vendor</returns>
        public Int32 CreateVendorSKu(tb_VendorSKU vendor)
        {
            Int32 isAdded = 0;
            try
            {
                VendorDAC dac = new VendorDAC();
                dac.Create(vendor);
                isAdded = vendor.VendorSKUID;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }



        /// <summary>
        /// Update Vendor detail
        /// </summary>
        /// <param name="vendor">tb_Vendor vendor</param>
        /// <returns>Number of affected row</returns>
        public Int32 UpdateVendorSKU(tb_VendorSKU vendor)
        {
            int RowsAffected = 0;
            try
            {
                VendorDAC dac = new VendorDAC();
                RowsAffected = dac.UpdateVendorsku(vendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }


        /// <summary>
        /// Get Vendor by VendorID
        /// </summary>
        /// <param name="VendorID">ID of selected Vendor</param>
        /// <returns>A object of selected Vendor</returns>
        public tb_VendorSKU GetVendorSKUByID(int VendorID)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetVendorSKUByID(VendorID);
        }

        /// <summary>
        /// Get Email Template List
        /// </summary>
        /// <returns>Returns the List of Email template</returns>
        public static List<tb_EmailTemplate> GetEmailtemplateList()
        {
            VendorDAC dac = new VendorDAC();
            List<tb_EmailTemplate> lstTemplate = dac.GetEmailtemplateList();
            return lstTemplate;
        }

        /// <summary>
        /// Add new Vendor
        /// </summary>
        /// <param name="vendor">tb_Vendor vendor</param>
        /// <returns>ID of created Vendor</returns>
        public Int32 CreateVendor(tb_Vendor vendor)
        {
            Int32 isAdded = 0;
            try
            {
                VendorDAC dac = new VendorDAC();
                dac.Create(vendor);
                isAdded = vendor.VendorID;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Get Vendor by VendorID
        /// </summary>
        /// <param name="VendorID">ID of selected Vendor</param>
        /// <returns>A object of selected Vendor</returns>
        public tb_Vendor GetVendorByID(int VendorID)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetVendorByID(VendorID);
        }

        /// <summary>
        /// Update Vendor detail
        /// </summary>
        /// <param name="vendor">tb_Vendor vendor</param>
        /// <returns>Number of affected row</returns>
        public Int32 UpdateVendor(tb_Vendor vendor)
        {
            int RowsAffected = 0;
            try
            {
                VendorDAC dac = new VendorDAC();
                RowsAffected = dac.Update(vendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }


        /// <summary>
        /// Get Vendor SKU by VendorID
        /// </summary>
        /// <param name="VendorID">Int32 VendorID</param>
        /// <returns>Returns Vendor SKU data in Dataset</returns>
        public static DataSet GetVendorSKUbyVendorID(Int32 VendorID)
        {
            VendorDAC dac = new VendorDAC();
            DataSet DsVendorSKU = new DataSet();
            DsVendorSKU = dac.GetVendorSKUbyVendorID(VendorID);
            return DsVendorSKU;
        }


        /// <summary>
        /// Get Email Template by ID
        /// </summary>
        /// <param name="TemplateID">Int32 TemplateID</param>
        /// <returns>Returns Email Template data in Dataset</returns>
        public static DataSet GetEmailTemplateByID(Int32 TemplateID)
        {
            VendorDAC dac = new VendorDAC();
            DataSet dsTemplate = new DataSet();
            dsTemplate = dac.GetEmailTemplateByID(TemplateID);
            return dsTemplate;
        }

        /// <summary>
        /// Get the Purchase order
        /// </summary>
        /// <param name="ono">Int32 ono</param>
        /// <param name="Pids">string Pids</param>
        /// <param name="customids">string customIDs</param>
        /// <returns>Returns the list of purchase order as a dataset</returns>
        public DataSet GetPurchaseOrder(Int32 ono, string Pids, string customids)
        {
            VendorDAC dac = new VendorDAC();
            DataSet dsTemplate = new DataSet();
            dsTemplate = dac.GetPurchaseOrder(ono, Pids, customids);
            return dsTemplate;
        }

        /// <summary>
        /// Mark Products Shipped for Vendor PO
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <param name="ProductIds">ArrayList ProductIds</param>
        /// <param name="TrackingNumber">ArrayList TrackingNumber</param>
        /// <param name="CourierName">ArrayList CourierName</param>
        /// <param name="ShippedDateList">ArrayList ShippedDateList</param>
        /// <param name="ShippedQty">ArrayList ShippedQty</param>
        /// <param name="ShippedNote">ArrayList ShippedNote</param>
        /// <param name="customcartid">ArrayList customcartid</param>
        /// <returns>returns 1 if True</returns>
        public bool MarkProductsShippedforVendorPO(int OrderNumber, ArrayList ProductIds, ArrayList TrackingNumber, ArrayList CourierName, ArrayList ShippedDateList, ArrayList ShippedQty, ArrayList ShippedNote, ArrayList customcartid)
        {
            VendorDAC dac = new VendorDAC();
            return dac.MarkProductsShippedforVendorPO(OrderNumber, ProductIds, TrackingNumber, CourierName, ShippedDateList, ShippedQty, ShippedNote, customcartid);
        }

        /// <summary>
        /// Get Vendor Products for PO
        /// </summary>
        /// <param name="Pono">Int32 Pono</param>
        /// <returns> Returns all Vendor products for displaying in PO</returns>
        public DataSet GetVendorProductsForPO(int Pono)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetVendorProductsForPO(Pono);
        }

        /// <summary>
        /// Get Vendor Products for Warehouse
        /// </summary>
        /// <param name="Pono">Int32 Pono</param>
        /// <returns>Returns all vendor products for displaying in WareHouse </returns>
        public DataSet GetVendorProductsforWarehouse(int Pono)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetVendorProductsforWarehouse(Pono);
        }

        #region Vendor Payment

        /// <summary>
        /// Get Data By Filter For Vendor Payment
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="VendorID">int VendorID</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Records for Vendor Payment</returns>
        public static IQueryable<VendorPaymentEntity> GetDataByFilterForVendorPayment(int startIndex, int pageSize, string sortBy, int VendorID, string SearchBy, string SearchValue)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            if (string.IsNullOrEmpty(SearchValue))
            {
                SearchValue = "";
            }
            else
            {
                SearchValue = SearchValue.Trim();
            }
            IQueryable<VendorPaymentEntity> result = from vendor in ctx.tb_Vendor
                                                     join
                                                     vendorpayment in ctx.tb_VendorPayment on
                                                     vendor.VendorID equals vendorpayment.VendorID
                                                     where vendor.Deleted == false && vendor.Active == true
                                                     select new VendorPaymentEntity
                                                     {
                                                         VendorID = vendor.VendorID,
                                                         VendorPaymentID = vendorpayment.VendorPaymentID,
                                                         Name = vendor.Name,
                                                         PaidBy = vendorpayment.PaidBy,
                                                         TransactionReference = vendorpayment.TransactionReference,
                                                         TransactionDate = vendorpayment.TransactionDate.Value,
                                                         PurchaseOrders = vendorpayment.PurchaseOrders,
                                                         POAmount = vendorpayment.POAmount.Value,
                                                         PaidAmount = vendorpayment.PaidAmount.Value
                                                     };
            if (VendorID != -1)
            {
                //search by VendorName and VendorID
                if (SearchBy == "Name")
                {
                    result = result.Where(a => a.Name.Contains(SearchValue) && a.VendorID == VendorID).AsQueryable();
                }
                else if (SearchBy == "PaidBy")
                {
                    //Search by PaidBy  and VendorID
                    result = result.Where(a => a.PaidBy.Contains(SearchValue) && a.VendorID == VendorID).AsQueryable();
                }
            }
            else //For all VendorID
            {
                if (SearchBy == "Name")
                {
                    //search by Vendor name for all store
                    result = result.Where(a => a.Name.Contains(SearchValue)).AsQueryable();
                }
                else if (SearchBy == "PaidBy")
                {
                    //Search by PaidBy for all Vendor
                    result = result.Where(a => a.PaidBy.Contains(SearchValue)).AsQueryable();
                }
                else
                {
                    //Get all value(No Filter)
                }
            }

            _count1 = result.Count();

            //Logic for searching
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = result.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    result = result.OrderByField(SortingOption[0].ToString(), true);
                }
                else if (SortingOption.Length == 2)
                {
                    result = result.OrderByField(SortingOption[0].ToString(), false);
                }
            }
            else
            {
                //Default sorting by Name
                result = result.OrderBy(o => o.Name);
            }
            result = result.Skip(startIndex).Take(pageSize);
            return result;
        }

        /// <summary>
        /// To count total number of record that are used for paging
        /// </summary>
        /// <param name="VendorID">int VendorID</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns Total no of records</returns>
        public static int GetCountForVendorPayment(int VendorID, string SearchBy, string SearchValue)
        {
            return _count1;
        }


        #endregion

        #region Vendor Quote Request

        /// <summary>
        /// Save Vendor Quote Request
        /// </summary>
        /// <param name="VendorQuoteReqId">Int32 VendorQuoteReqId</param>
        /// <param name="VendorId">Int32 VendorId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="Quantity">Int32 Quantity</param>
        /// <param name="Name">String Name</param>
        /// <param name="ProductOption">String ProductOption</param>
        /// <param name="Notes">String Notes</param>
        /// <returns>Returns Identity Value of Inserted Record</returns>
        public int SaveVendorQuoteRequest(Int32 VendorQuoteReqId, Int32 VendorId, Int32 ProductId, Int32 Quantity, String Name, String ProductOption, String Notes)
        {
            VendorDAC dac = new VendorDAC();
            return dac.SaveVendorQuoteRequest(VendorQuoteReqId, VendorId, ProductId, Quantity, Name, ProductOption, Notes);
        }
        #endregion

        #region Vendor Quote Reply
        /// <summary>
        /// Save Vendor Quote Reply
        /// </summary>
        /// <param name="VendorQuoteRequestID">Int32 VendorQuoteRequestID</param>
        /// <param name="VendorId">Int32 VendorId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="Quantity">Int32 Quantity</param>
        /// <param name="Name">String Name</param>
        /// <param name="ProductOption">String ProductOption</param>
        /// <param name="Notes">String Notes</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="AvailDays">Int32 AvailDays</param>
        /// <param name="Location">String Location</param>
        /// <returns>Returns the Identity Value of Inserted Record</returns>
        public int SaveVendorQuoteReply(Int32 VendorQuoteRequestID, Int32 VendorId, Int32 ProductId, Int32 Quantity, String Name, String ProductOption, String Notes, decimal Price, Int32 AvailDays, String Location)
        {
            VendorDAC dac = new VendorDAC();
            return dac.SaveVendorQuoteReply(VendorQuoteRequestID, VendorId, ProductId, Quantity, Name, ProductOption, Notes, Price, AvailDays, Location);
        }
        #endregion


        public DataSet GetDropShipperList()
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetDropShipperList();
        }
        public DataSet GetDropShipperListSearched(string Name)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetDropShipperListSearched(Name);
        }

        public DataSet GetDropShipperListbyvendor(int vendorid)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetDropShipperListbyvendor(vendorid);
        }
        public DataSet GetProductList(int storeId)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetProductList(storeId);
        }
        public DataSet GetProductListSearched(string Name, Int32 StoreId)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetProductListSearched(Name, StoreId);
        }

        public void InsertDropshipProduct(Int32 ProductID, Int32 VendorID, Int32 VendorSKUID, string VendorSKU, string Priority)
        {
            VendorDAC dac = new VendorDAC();
            dac.InsertDropshipProduct(ProductID, VendorID, VendorSKUID, VendorSKU, Priority);
        }

        public DataSet GetDropShipperListByProductID(Int32 ProductID)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetDropShipperListByProductID(ProductID);
        }

        public DataSet GetAllAssemblerProductSKUByProductID(Int32 ProductID)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetAllAssemblerProductSKUByProductID(ProductID);
        }
        public DataSet GetProductTypeDeliveryByID(int ProductDeliveryID)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetProductTypeDeliveryByID(ProductDeliveryID);
        }

        public void InsertAssemblerProduct(Int32 RefProductID, Int32 ProductID, Int32 Quantity, Int32 CreatedBy, Int32 UpdatedBy)
        {
            VendorDAC dac = new VendorDAC();
            dac.InsertAssemblerProduct(RefProductID, ProductID, Quantity, CreatedBy, UpdatedBy);
        }
        public DataSet GetProductSKUListByProductID(Int32 ProductID, Int32 StoredID)
        {
            VendorDAC dac = new VendorDAC();
            return dac.GetProductSKUListByProductID(ProductID, StoredID);
        }

        #endregion
    }

    public class VendorPaymentEntity
    {
        private int _VendorID;

        public int VendorID
        {
            get { return _VendorID; }
            set { _VendorID = value; }
        }

        private string _PaidBy;

        public string PaidBy
        {
            get { return _PaidBy; }
            set { _PaidBy = value; }
        }

        private string _TransactionReference;

        public string TransactionReference
        {
            get { return _TransactionReference; }
            set { _TransactionReference = value; }
        }

        private DateTime _TransactionDate;

        public DateTime TransactionDate
        {
            get { return _TransactionDate; }
            set { _TransactionDate = value; }
        }

        private string _PurchaseOrders;

        public string PurchaseOrders
        {
            get { return _PurchaseOrders; }
            set { _PurchaseOrders = value; }
        }

        private decimal _POAmount;

        public decimal POAmount
        {
            get { return _POAmount; }
            set { _POAmount = value; }
        }

        private decimal _PaidAmount;

        public decimal PaidAmount
        {
            get { return _PaidAmount; }
            set { _PaidAmount = value; }
        }

        private int _VendorPaymentID;

        public int VendorPaymentID
        {
            get { return _VendorPaymentID; }
            set { _VendorPaymentID = value; }
        }


        private string _Name;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
    }

}
