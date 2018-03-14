using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using Solution.Bussines.Entities;
using System.Data.Objects;
using System.Data;


namespace Solution.Bussines.Components
{
    /// <summary>
    /// Shipping Component Class Contains Shipping related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class ShippingComponent
    {
        #region Declaration
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        List<ShippingEntity> lstShipping = new List<ShippingEntity>();
        ShippingDAC ObjShipping = new ShippingDAC();

        public static bool _afterDelete = false;
        private static int _count;
        private static int _ShippingServiceID = 0;
        private static int _ShippingMethodID = 0;
        private static DataSet DSCommon = null;
        public static int ShippingServiceID
        {
            get { return _ShippingServiceID; }
            set { _ShippingServiceID = value; }
        }
        public static bool AfterDelete
        {
            get { return _afterDelete; }
            set { _afterDelete = value; }
        }
        public static int ShippingMethodID
        {
            get { return _ShippingMethodID; }
            set { _ShippingMethodID = value; }
        }

        #endregion

        #region  Key Function
        /// <summary>
        /// Get Active Shipping Services
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns shipping Services Data</returns>
        public DataSet GetShippingServices(Int32 StoreID)
        {
            ShippingDAC objShipping = new ShippingDAC();
            return objShipping.GetShippingServices(StoreID);
        }

        /// <summary>
        /// Get The Data Source for Grid view after sorting ,searching and On First Time Load
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns IQueryable Non generic Collection</returns>
        public IQueryable<ShippingEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue)
        {
            IQueryable<ShippingEntity> results = from a in ctx.tb_ShippingServices
                                                 select new ShippingEntity
                                                 {
                                                     ShippingServiceID = a.ShippingServiceID,
                                                     StoreID = a.tb_Store.StoreID,
                                                     StoreName = a.tb_Store.StoreName,
                                                     ShippingService = a.ShippingService,
                                                     Active = a.Active.Value,
                                                 };
            if (!string.IsNullOrEmpty(pSearchValue))
            {
                if (pStoreId != -1)
                {
                    results = results.Where(a => a.ShippingService.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId).AsQueryable();
                }
                else
                {
                    results = results.Where(a => a.ShippingService.Contains(pSearchValue.Trim())).AsQueryable();
                }
            }
            else
            {
                if (pStoreId != -1)
                {
                    results = results.Where(a => a.StoreID == pStoreId).AsQueryable();
                }
                else
                {

                }
            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lstShipping.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    results = results.OrderByField(SortingOption[0].ToString(), true);

                }
                else if (SortingOption.Length == 2)
                {
                    results = results.OrderByField(SortingOption[0].ToString(), false);

                }
            }
            else
            {
                results = results.OrderBy(o => o.ShippingService);
            }
            results = results.Skip(startIndex).Take(pageSize);
            return results;


        }

        /// <summary>
        /// For Checking Duplicate Shipping Service Name at the time of Inserting/Adding new Shipping Service
        /// </summary>
        /// <param name="tbltaxclass">Table ShippingServices</param>
        /// <returns>Returns row count if exist </returns>
        public bool CheckDuplicate(tb_ShippingServices tblShippingService)
        {
            if (ObjShipping.CheckDuplicate(tblShippingService) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Create Shipping Service For Inserting/Adding new Shipping Service
        /// </summary>
        /// <param name="tblShippingService">tb_ShippingServices tblShippingService</param>
        /// <returns>Return Id of the Created/Added Record</returns>
        public Int32 CreateShippingService(tb_ShippingServices tblShippingService)
        {
            Int32 isAdded = 0;
            try
            {
                ObjShipping.CreateShippingService(tblShippingService);
                isAdded = tblShippingService.ShippingServiceID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// GetPropertyValue
        /// </summary>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// GetCount For Paging in Grid View
        /// </summary>
        /// <param name="CName"></param>
        /// <param name="pStoreId"></param>
        /// <param name="pSearchValue"></param>
        /// <returns></returns>
        public static int GetCount(string CName, int pStoreId, string pSearchValue)
        {
            return _count;
        }

        /// <summary>
        /// For Updating Shipping Service
        /// </summary>
        /// <param name="tblShippingService">Table ShippingServices</param>
        /// <returns>Return int ShippingServiceID Which is Updated</returns>
        public Int32 UpdateShippingService(tb_ShippingServices tblShippingService)
        {
            Int32 isUpdated = 0;
            try
            {
                ObjShipping.Update(tblShippingService);
                isUpdated = tblShippingService.ShippingServiceID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// GetShippingService For Getting Particular Record From Table by Id
        /// </summary>
        /// <param name="ShippingServiceID">ShippingServiceID is The Id of the Record to Select</param>
        /// <returns>Return Shipping Services table object</returns>
        public tb_ShippingServices GetShippingService(int ShippingServiceID)
        {
            return ObjShipping.GetShippingService(ShippingServiceID);
        }

        /// <summary>
        /// Get The Data Source for Select Method for Grid view after sorting ,searching and On First Time Load
        /// </summary>
        /// <param name="startIndex">For Starting index of Grid View</param>
        /// <param name="pageSize">Page Size of Grid view</param>
        /// <param name="sortBy">Sort By For Sorting Grid view</param>
        /// <param name="CName">For Control Name</param>
        /// <param name="pStoreId">Searching Parameter Store Id For searching By Store</param>
        /// <param name="pSearchValue">Value For parameter For Searching</param>
        /// <param name="pShippingServiceID">Searching parameter ShippingServiceID For Searching By ShippingService</param>
        /// <returns>return IQueryable Non generic Collection</returns>
        public IQueryable<ShippingEntity> GetDataByFilterForSM(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue, int pShippingServiceID)
        {


            IQueryable<ShippingEntity> results = from a in ctx.tb_ShippingMethods
                                                 where a.Deleted.Value == false
                                                 select new ShippingEntity
                                                 {
                                                     ShippingMethodID = a.ShippingMethodID,
                                                     Name = a.Name,
                                                     ShippingService = a.tb_ShippingServices.ShippingService,
                                                     ShippingServiceID = a.tb_ShippingServices.ShippingServiceID,
                                                     StoreID = a.tb_ShippingServices.tb_Store.StoreID,
                                                     StoreName = a.tb_ShippingServices.tb_Store.StoreName,
                                                     Active = a.Active.Value,
                                                     ShowOnClient = a.ShowOnClient.Value,
                                                     ShowOnAdmin = a.ShowOnAdmin.Value,
                                                     RealTimeShipping = a.isRTShipping.Value,
                                                 };
            if (!string.IsNullOrEmpty(pSearchValue))
            {
                if (pStoreId != -1 && pShippingServiceID != -1)
                {
                    results = results.Where(a => a.Name.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId && a.ShippingServiceID == pShippingServiceID).AsQueryable();
                }
                else if (pStoreId != -1 && pShippingServiceID == -1)
                {
                    results = results.Where(a => a.Name.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId).AsQueryable();
                }
                else
                {
                    results = results.Where(a => a.Name.Contains(pSearchValue.Trim())).AsQueryable();
                }
            }
            else
            {
                if (pStoreId != -1 && pShippingServiceID != -1)
                {
                    results = results.Where(a => a.StoreID == pStoreId && a.ShippingServiceID == pShippingServiceID).AsQueryable();
                }
                else if (pStoreId != -1 && pShippingServiceID == -1)
                {
                    results = results.Where(a => a.StoreID == pStoreId).AsQueryable();
                }
                else
                {
                    results = results.AsQueryable();
                }
            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lstShipping.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    results = results.OrderByField(SortingOption[0].ToString(), true);

                }
                else if (SortingOption.Length == 2)
                {
                    results = results.OrderByField(SortingOption[0].ToString(), false);

                }
            }
            else
            {
                results = results.OrderBy(o => o.Name);
            }
            results = results.Skip(startIndex).Take(pageSize);
            return results;

        }

        /// <summary>
        /// GetCount For Paging in Grid View For Shipping Method
        /// </summary>
        /// <param name="CName"></param>
        /// <param name="pStoreId"></param>
        /// <param name="pSearchValue"></param>
        /// <returns></returns>
        public static int GetCountForSM(string CName, int pStoreId, string pSearchValue, int pShippingServiceID)
        {
            return _count;
        }

        /// <summary>
        /// GetShippingMethod By ID
        /// </summary>
        /// <param name="ShippingMethodID">int ShippingMethodID</param>
        /// <returns>Return Shipping Method table object</returns>
        public tb_ShippingMethods GetShippingMethod(int ShippingMethodID)
        {
            return ObjShipping.GetShippingMethod(ShippingMethodID);
        }

        /// <summary>
        /// For Deleting Particular Record
        /// </summary>
        /// <param name="tblTaxclass">Table ShippingMethods</param>
        /// <returns>return int Id which is Deleted</returns>
        public Int32 DeleteShippingMethod(tb_ShippingMethods tblShippingMethod)
        {
            Int32 isUpdated = 0;
            try
            {
                ObjShipping.UpdateShippingMethod(tblShippingMethod);
                isUpdated = tblShippingMethod.ShippingMethodID;
                _afterDelete = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }

        /// <summary>
        /// For Checking Duplicate Shipping Method Name at the time of Inserting/Adding new Tax class
        /// </summary>
        /// <param name="name">Name of Shipping Method</param>
        /// <param name="ShippingService">ShippingService</param>
        /// <returns>Return Boolean Value True/False</returns>
        public bool CheckDuplicateShipppingMethod(string name, string ShippingService)
        {
            if (ObjShipping.CheckDuplicateShipppingMethod(name, ShippingService) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// CreateShippingMethod For Inserting/Adding new Shipping Method
        /// </summary>
        /// <param name="tblShippingService">tb_ShippingMethods tblShippingMethods</param>
        /// <returns>Return Id of the Created/Added Record</returns>
        public Int32 CreateShippingMethod(tb_ShippingMethods tblShippingMethods)
        {
            Int32 isAdded = 0;
            try
            {
                ObjShipping.CreateShippingMethod(tblShippingMethods);
                isAdded = tblShippingMethods.ShippingMethodID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// For Updating Shipping Method
        /// </summary>
        /// <param name="tblShippingService">Table Shipping Method</param>
        /// <returns>Return int ShippingMethodID Which is Updated  </returns>
        public Int32 UpdateShippingMethod(tb_ShippingMethods tblShippingMethod)
        {
            Int32 isUpdated = 0;
            try
            {
                ObjShipping.UpdateShippingMethod(tblShippingMethod);
                isUpdated = tblShippingMethod.ShippingMethodID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Select Shipping Service With respect to Store ID
        /// </summary>
        /// <param name="StoreID">int Store Id</param>
        /// <returns>DataSet</returns>
        public DataSet SelectShippingService(Int32 StoreID)
        {
            DSCommon = new DataSet();
            DSCommon = ObjShipping.SelectShippingService(StoreID);
            return DSCommon;
        }

        /// <summary>
        /// Get Fixed Shipping Methods
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="ShippingService">string ShippingService</param>
        /// <returns>Returns Fixed Shipping Methods in Dataset</returns>
        public DataSet GetFixedShippingMethods(Int32 StoreID, string ShippingService)
        {
            DataSet dsFixedShippingMethods = new DataSet();
            ShippingDAC ObjShipping = new ShippingDAC();
            dsFixedShippingMethods = ObjShipping.GetFixedShippingMethods(StoreID, ShippingService);
            return dsFixedShippingMethods;
        }

        /// <summary>
        /// Mark Products as Shipped
        /// </summary>
        /// <param name="query">string query</param>
        /// <returns>Returns true if updated</returns>
        public static bool MarkProductsShipped(string query)
        {
            ShippingDAC ObjShipping = new ShippingDAC();
            return ObjShipping.MarkProductsShipped(query);
        }

        /// <summary>
        /// Mark Order As Shipped for Shipping Label
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="ShippedVIA">String ShippedVIA</param>
        /// <param name="ShippingTrackingNumber">String ShippingTrackingNumber</param>
        /// <param name="ShippedOn">DateTime ShippedOn</param>
        /// <param name="OrderStatus">string OrderStatus</param>
        /// <returns>Returns true if marked successfully</returns>
        public static bool MarkOrderAsShippedforShippingLabel(int OrderNumber, String ShippedVIA, String ShippingTrackingNumber, DateTime ShippedOn, string OrderStatus)
        {
            ShippingDAC ObjShipping = new ShippingDAC();
            return ObjShipping.MarkOrderAsShippedforShippingLabel(OrderNumber, ShippedVIA, ShippingTrackingNumber, ShippedOn, OrderStatus);
        }


        /// <summary>
        /// Update ordered Shopping cart items
        /// </summary>
        /// <param name="TrackingNumber">string TrackingNumber</param>
        /// <param name="ProductId">string ProductId</param>
        /// <param name="CourierName">string CourierName</param>
        /// <param name="OrderedShoppingCartID">int OrderedShoppingCartID</param>
        /// <param name="ShippedQty">int ShippedQty</param>
        /// <param name="WareHouseID">int WareHouseID</param>
        /// <param name="Packid">int Packid</param>
        /// <returns>Returns true if updated</returns>
        public static bool UpdateorderedShoppingcartitems(string TrackingNumber, string ProductId, string CourierName, int OrderedShoppingCartID, int ShippedQty, int WareHouseID, int Packid)
        {

            ShippingDAC ObjShipping = new ShippingDAC();
            return ObjShipping.UpdateorderedShoppingcartitems(TrackingNumber, ProductId, CourierName, OrderedShoppingCartID, ShippedQty, WareHouseID, Packid);
        }
        /// <summary>
        /// Update Order status
        /// </summary>
        /// <param name="ShippingTrackingNumber">String ShippingTrackingNumber</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="OrderStatus">string OrderStatus</param>
        /// <returns>Returns true if updated</returns>
        public static bool UpdateOrderstatus(string TrackingNumber, int OrderNumber, string OrderStatus)
        {
            ShippingDAC ObjShipping = new ShippingDAC();
            return ObjShipping.UpdateOrderstatus(TrackingNumber, OrderNumber, OrderStatus);
        }
        #endregion
    }
    public class ShippingEntity
    {
        private int _ShippingServiceID;
        private int _CreatedBy;
        private string _StoreName;
        private string _ShippingService;
        private int _StoreID;
        private int _UpdatedBy;
        private DateTime _CreatedOn;
        private DateTime _UpdatedOn;
        private string _Name;
        private int _ShippingMethodID;

        public bool _Active;
        public int ShippingServiceID
        {
            get { return _ShippingServiceID; }
            set { _ShippingServiceID = value; }
        }
        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }
        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }
        public string ShippingService
        {
            get { return _ShippingService; }
            set { _ShippingService = value; }
        }
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public int UpdatedBy
        {
            get { return _UpdatedBy; }
            set { _UpdatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return _UpdatedOn; }
            set { _UpdatedOn = value; }
        }
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }

        #region  Property for Shipping Method
        public int ShippingMethodID
        {
            get { return _ShippingMethodID; }
            set { _ShippingMethodID = value; }
        }
        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public bool _ShowOnClient;
        public bool ShowOnClient
        {
            get { return _ShowOnClient; }
            set { _ShowOnClient = value; }
        }
        public bool _RealTimeShipping;
        public bool RealTimeShipping
        {
            get { return _RealTimeShipping; }
            set { _RealTimeShipping = value; }
        }
        public bool _ShowOnAdmin;
        public bool ShowOnAdmin
        {
            get { return _ShowOnAdmin; }
            set { _ShowOnAdmin = value; }
        }
        #endregion
    }
}
