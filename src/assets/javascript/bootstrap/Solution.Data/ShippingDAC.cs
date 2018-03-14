using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// Shipping Component Class Contains Shipping related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class ShippingDAC
    {
        #region Declaration
        tb_ShippingServices tblShippingService = null;
        tb_ShippingMethods tblShippingMethod = null;
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions
        /// <summary>
        /// Get Active Shipping Services
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns shipping Services Data</returns>
        public DataSet GetShippingServices(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetShippingServices";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        #endregion

        /// <summary>
        ///  Select Shipping Service With respect to Store ID
        /// </summary>
        /// <param name="StoreID">int Store ID</param>
        /// <returns>Returns the shipping service list by storeID</returns>
        public DataSet SelectShippingService(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShippingServices_SelectServicesByStoreId";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Particular Record from Shipping Service fOR ShippingServiceID
        /// </summary>
        /// <param name="QauntityDiscountID">int ShippingServiceID</param>
        /// <returns>Return Shipping Services table object</returns>
        public tb_ShippingServices GetShippingService(int ShippingServiceID)
        {
            tblShippingService = null;

            {
                tblShippingService = ctx.tb_ShippingServices.First(e => e.ShippingServiceID == ShippingServiceID);
            }
            return tblShippingService;
        }

        /// <summary>
        ///  For Updating Shipping Service
        /// </summary>
        /// <param name="ShippingServicetbl">tb_ShippingServices ShippingServicetbl</param>
        public void Update(tb_ShippingServices ShippingServicetbl)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            ctx.SaveChanges();
        }

        /// <summary>
        /// For Checking Duplicate Shipping Service Name at the time of Inserting/Adding new Shipping Service
        /// </summary>
        /// <param name="tbltaxclass">Table ShippingServices</param>
        /// <returns>Returns row count if exist </returns>
        public int CheckDuplicate(tb_ShippingServices ShippingServicetbl)
        {
            //Int32 isExists = 0;
            //RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            //{
            //    isExists = (from a in ctx.tb_ShippingServices
            //                where a.ShippingService == ShippingServicetbl.ShippingService
            //                select new { a.ShippingServiceID }).Count();
            //}
            //return isExists;

            Int32 isExists = 0;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(ShippingServicetbl.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var results = from a in ctx.tb_ShippingServices
                          select new
                          {
                              ShippingServices = a,
                              store = a.tb_Store
                          };
            isExists = (from a in results
                        where a.ShippingServices.ShippingService == ShippingServicetbl.ShippingService
                        && a.ShippingServices.Active == false
                        && a.ShippingServices.ShippingServiceID != ShippingServicetbl.ShippingServiceID
                        && a.store.StoreID == ID
                        select new { a.ShippingServices.ShippingServiceID }).Count();
            return isExists;
        }

        /// <summary>
        /// Create Shipping Service For Inserting/Adding new Shipping Service
        /// </summary>
        /// <param name="ShippingServicetbl">tb_ShippingServices ShippingServicetbl</param>
        /// <returns>Returns the Shipping service table object</returns>
        public tb_ShippingServices CreateShippingService(tb_ShippingServices ShippingServicetbl)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_ShippingServices(ShippingServicetbl);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ShippingServicetbl;
        }

        /// <summary>
        /// For Checking Duplicate Shipping Method Name at the time of Inserting/Adding new Shipping Method
        /// </summary>
        /// <param name="name">Name of Shipping Method</param>
        /// <param name="ShippingService">ShippingService</param>
        /// <returns>Returns the row count if exists </returns>
        public int CheckDuplicateShipppingMethod(string name, string ShippingService)
        {

            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShippingMethods_CheckShippingMethod";
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@ShippingService", ShippingService);
            return Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));

        }

        /// <summary>
        /// Create Shipping Method For Inserting/Adding new Shipping Method
        /// </summary>
        /// <param name="ShippingServicetbl">tb_ShippingMethods ShippingMethodstbl</param>
        /// <returns>Returns the shipping method table object</returns>
        public tb_ShippingMethods CreateShippingMethod(tb_ShippingMethods ShippingMethodstbl)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_ShippingMethods(ShippingMethodstbl);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return ShippingMethodstbl;
        }

        /// <summary>
        /// Get Shipping Method fOR ShippingMethodID
        /// </summary>
        /// <param name="ShippingMethodID">int ShippingMethodID</param>
        /// <returns>Return Shipping Method table object</returns>
        public tb_ShippingMethods GetShippingMethod(int ShippingMethodID)
        {
            tblShippingMethod = null;
            {
                tblShippingMethod = ctx.tb_ShippingMethods.First(e => e.ShippingMethodID == ShippingMethodID);
            }
            return tblShippingMethod;
        }

        /// <summary>
        ///  For Updating ShippingMethod
        /// </summary>
        /// <param name="taxclass">tb_ShippingMethods ShippingMethod</param>
        public void UpdateShippingMethod(tb_ShippingMethods ShippingMethod)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            ctx.SaveChanges();
        }

        /// <summary>
        /// Get Fixed Shipping Methods
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="ShippingService">string ShippingService</param>
        /// <returns>Returns Fixed Shipping Methods in Dataset</returns>
        public DataSet GetFixedShippingMethods(Int32 StoreID, string ShippingService)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShippingMethods_GetFixedShippingMethods";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@ShippingService", ShippingService);

            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Mark Products as Shipped
        /// </summary>
        /// <param name="query">string query</param>
        /// <returns>Returns true if updated</returns>
        public bool MarkProductsShipped(string query)
        {
            bool isUpdateTrue = false;
            objSql = new SQLAccess();
            int isUpdate = Convert.ToInt32(objSql.ExecuteNonQuery(query));

            if (isUpdate > 0)
            {
                isUpdateTrue = true;
            }
            return isUpdateTrue;
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
        public bool MarkOrderAsShippedforShippingLabel(int OrderNumber, String ShippedVIA, String ShippingTrackingNumber, DateTime ShippedOn, string OrderStatus)
        {
            bool isUpdateTrue = false;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShippingLabel_MarkOrderAsShipped";
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@ShippedVIA", ShippedVIA);
            cmd.Parameters.AddWithValue("@ShippingTrackingNumber", ShippingTrackingNumber);
            cmd.Parameters.AddWithValue("@ShippedOn", ShippedOn);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);
            cmd.Parameters.AddWithValue("@Mode", 1);
            int isUpdate = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));

            if (isUpdate > 0)
            {
                isUpdateTrue = true;
            }
            return isUpdateTrue;
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
        public bool UpdateorderedShoppingcartitems(string TrackingNumber, string ProductId, string CourierName, int OrderedShoppingCartID, int ShippedQty, int WareHouseID,int Packid)
        {
            bool isUpdateTrue = false;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShippingLabel_MarkOrderAsShipped";
            cmd.Parameters.AddWithValue("@ShippingTrackingNumber", TrackingNumber);
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@CourierName", CourierName);
            cmd.Parameters.AddWithValue("@ShippedQty", ShippedQty);
            cmd.Parameters.AddWithValue("@OrderedShoppingCartID", OrderedShoppingCartID);
            cmd.Parameters.AddWithValue("@WareHouseID", WareHouseID);
            cmd.Parameters.AddWithValue("@PackId", Packid);
            cmd.Parameters.AddWithValue("@Mode", 2);
            
            //cmd.Parameters.AddWithValue("@ShippedOn", ShippedOn);
            int isUpdate = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));

            if (isUpdate > 0)
            {
                isUpdateTrue = true;
            }
            return isUpdateTrue;
        }

        /// <summary>
        /// Update Order status
        /// </summary>
        /// <param name="ShippingTrackingNumber">String ShippingTrackingNumber</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="OrderStatus">string OrderStatus</param>
        /// <returns>Returns true if updated</returns>
        public bool UpdateOrderstatus(String ShippingTrackingNumber, int OrderNumber, string OrderStatus)
        {
            bool isUpdateTrue = false;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShippingLabel_MarkOrderAsShipped";
            cmd.Parameters.AddWithValue("@ShippingTrackingNumber", ShippingTrackingNumber);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@OrderStatus", OrderStatus);
            cmd.Parameters.AddWithValue("@Mode", 3);
            int isUpdate = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));

            if (isUpdate > 0)
            {
                isUpdateTrue = true;
            }
            return isUpdateTrue;
        }


    }
}
