using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Collections;

namespace Solution.Data
{
    /// <summary>
    /// Vendor Data Access Class Contains Vendor Related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class VendorDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Email Template List
        /// </summary>
        /// <returns>Returns the List of Email template</returns>
        public List<tb_EmailTemplate> GetEmailtemplateList()
        {
            List<tb_EmailTemplate> lstTemplate = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            lstTemplate = (from MailTemplate in ctx.tb_EmailTemplate
                           select MailTemplate).ToList();
            return lstTemplate;
        }

        /// <summary>
        /// Add New Vendor
        /// </summary>
        /// <param name="vendor">tb_Vendor vendor</param>
        /// <returns>Insert / Update Record and returns a tb_vendor object</returns>
        public tb_Vendor Create(tb_Vendor vendor)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_Vendor(vendor);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return vendor;
        }

        /// <summary>
        /// Get Vendor detail by VendorID
        /// </summary>
        /// <param name="VendorID">ID of selected Vendor</param>
        /// <returns>A object of selected VendorID</returns>
        public tb_Vendor GetVendorByID(int VendorID)
        {
            tb_Vendor vendor = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                vendor = ctx.tb_Vendor.FirstOrDefault(e => e.VendorID == VendorID);
            }
            return vendor;
        }

        /// <summary>
        /// Edit Vendor details
        /// </summary>
        /// <param name="vendor">A vendor object</param>
        /// <returns>Number of rows affected</returns>
        public int Update(tb_Vendor vendor)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int RowsAffected = 0;
            try
            {
                RowsAffected = ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return RowsAffected;
        }

        /// <summary>
        /// Get Vendor SKU by VendorID
        /// </summary>
        /// <param name="VendorID">Int32 VendorID</param>
        /// <returns>Returns Vendor SKU data in Dataset</returns>
        public DataSet GetVendorSKUbyVendorID(Int32 VendorID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_VendorOptions";
            cmd.Parameters.AddWithValue("@VendorID", VendorID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        //For Client Functions
        /// <summary>
        /// Get All Vendor
        /// </summary>
        /// <returns>List of All Vendor as a Dataset</returns>
        public DataSet GetAllVendor()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Vendor_GetAllVendor";
            //  cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Get Vendor detail by VendorID
        /// </summary>
        /// <param name="VendorID">ID of selected Vendor</param>
        /// <returns>A object of selected VendorID</returns>
        public tb_VendorSKU GetVendorSKUByID(int VendorID)
        {
            tb_VendorSKU vendor = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                vendor = ctx.tb_VendorSKU.FirstOrDefault(e => e.VendorSKUID == VendorID);
            }
            return vendor;
        }


        /// <summary>
        /// Add New Vendor
        /// </summary>
        /// <param name="vendor">tb_Vendor vendor</param>
        /// <returns>Insert / Update Record and returns a tb_vendor object</returns>
        public tb_VendorSKU Create(tb_VendorSKU vendor)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_VendorSKU(vendor);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return vendor;
        }
        /// <summary>
        /// Edit Vendor details
        /// </summary>
        /// <param name="vendor">A vendor object</param>
        /// <returns>Number of rows affected</returns>
        public int UpdateVendorsku(tb_VendorSKU vendor)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int RowsAffected = 0;
            try
            {
                RowsAffected = ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return RowsAffected;
        }


        /// <summary>
        /// Get Email Template by ID
        /// </summary>
        /// <param name="TemplateID">Int32 TemplateID</param>
        /// <returns>Returns Email Template data in Dataset</returns>
        public DataSet GetEmailTemplateByID(Int32 TemplateID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_EmailTemplate_GetEmailTemplateByID";
            cmd.Parameters.AddWithValue("@TemplateID", TemplateID);
            return objSql.GetDs(cmd);
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
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetPurchaseOrder";
            cmd.Parameters.AddWithValue("@OrderNumber", ono);
            cmd.Parameters.AddWithValue("@ProductIDs", Pids);
            cmd.Parameters.AddWithValue("@CustomIDs", customids);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Mark Products Shipped for Vendor PO
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="ProductIds">ArrayList ProductIds</param>
        /// <param name="TrackingNumber">ArrayList TrackingNumber</param>
        /// <param name="CourierName">ArrayList CourierName</param>
        /// <param name="ShippedDateList">ArrayList ShippedDateList</param>
        /// <param name="ShippedQty">ArrayList ShippedQty</param>
        /// <param name="ShippedNote">ArrayList ShippedNote</param>
        /// <param name="customcartid">ArrayList customcartid</param>
        /// <returns>returns 1 = True</returns>
        public bool MarkProductsShippedforVendorPO(int OrderNumber, ArrayList ProductIds, ArrayList TrackingNumber, ArrayList CourierName, ArrayList ShippedDateList, ArrayList ShippedQty, ArrayList ShippedNote, ArrayList customcartid)
        {
            objSql = new SQLAccess();
            string Query = string.Empty;
            for (int iLoopIds = 0; iLoopIds < ProductIds.Count; iLoopIds++)
            {
                Query += " if exists (select 1 from tb_OrderShippedItems where OrderNumber=" + OrderNumber + " and RefProductID=" + ProductIds[iLoopIds]
                     + ") begin update tb_OrderShippedItems set ShippedQty=isnull(ShippedQty,0)+" + ShippedQty[iLoopIds] + ",ShippedVia=(case when isnull(ShippedVia,'')='' then '" + CourierName[iLoopIds] + "' else ShippedVia end),ShippedNote=isnull(ShippedNote,'')+', " + ShippedNote[iLoopIds]
                     + "' where OrderNumber=" + OrderNumber + " and RefProductID=" + ProductIds[iLoopIds]
                     + " end else begin insert into tb_OrderShippedItems(OrderNumber,RefProductID,TrackingNumber,ShippedVia,Shipped,ShippedOn,ShippedQty,ShippedNote) values(" + OrderNumber + "," + ProductIds[iLoopIds]
                     + ",'" + TrackingNumber[iLoopIds] + "','" + CourierName[iLoopIds] + "',1,'" + ShippedDateList[iLoopIds] + "'," + ShippedQty[iLoopIds] + ",'" + ShippedNote[iLoopIds] + "')end";
                //+ " if exists (select 1 from tb_LockProducts where OrderNumber=" + OrderNumber + " and ProductID=" + ProductIds[iLoopIds] + " and ordercustomcartId=" + customcartid[iLoopIds] + " and IsCompleted=0"
                //+ ") begin update tb_LockProducts set IsCompleted=1,Quantity=" + ShippedQty[iLoopIds] + ",MarkQuantity=" + ShippedQty[iLoopIds] + ",ispo=1 "
                //+ " where OrderNumber=" + OrderNumber + " and ProductID=" + ProductIds[iLoopIds] + " and ordercustomcartId=" + customcartid[iLoopIds] + " and IsCompleted=0"
                //+ " end else begin  insert into tb_LockProducts(OrderNumber,ProductID,Quantity,IsCompleted,MarkQuantity,ispo,ordercustomcartId) values (" + OrderNumber + "," + ProductIds[iLoopIds] + "," + ShippedQty[iLoopIds] + ",1," + ShippedQty[iLoopIds] + ",1," + customcartid[iLoopIds] + ")end";
            }
            if (!string.IsNullOrEmpty(Query))
            {
                return Convert.ToBoolean(objSql.ExecuteNonQuery(Query));
            }
            return false;
        }

        /// <summary>
        /// Get Vendor Products for PO
        /// </summary>
        /// <param name="Pono">Int32 Pono</param>
        /// <returns> Returns all Vendor products for displaying in PO</returns>
        public DataSet GetVendorProductsForPO(int Pono)
        {
            objSql = new SQLAccess();
            return objSql.GetDs(@"SELECT  dbo.tb_Product.Name, dbo.tb_PurchaseOrderItems.Quantity, dbo.tb_PurchaseOrderItems.PONumber,dbo.tb_PurchaseOrder.OrderNumber,
                                    dbo.tb_PurchaseOrderItems.ProductID, dbo.tb_PurchaseOrderItems.Price, ISNULL(dbo.tb_PurchaseOrderItems.IsShipped, 0) 
                                    AS IsShipped,ISNULL(dbo.tb_PurchaseOrderItems.Ispaid, 0) as Ispaid, dbo.tb_Product.SKU, dbo.tb_Vendor.Name AS Vname, dbo.tb_Vendor.Email, 
                                    dbo.tb_PurchaseOrder.OrderNumber,tb_OrderShippedItems.TrackingNumber, dbo.tb_PurchaseOrder.PODate,dbo.tb_Vendor.Phone,isnull(tb_OrderShippedItems.ShippedVia,'') as ShippedVia,isnull(tb_OrderShippedItems.ShippedOn,'') as ShippedOn 
                                    FROM dbo.tb_Product INNER JOIN
                                    dbo.tb_PurchaseOrderItems ON dbo.tb_Product.ProductID = dbo.tb_PurchaseOrderItems.ProductID INNER JOIN
                                    dbo.tb_PurchaseOrder ON dbo.tb_PurchaseOrderItems.PONumber = dbo.tb_PurchaseOrder.PONumber INNER JOIN
                                    dbo.tb_Vendor ON dbo.tb_PurchaseOrder.VendorID = dbo.tb_Vendor.VendorId LEFT OUTER JOIN
                                    dbo.tb_VendorQuoteReply AS rep ON rep.VendorID = dbo.tb_PurchaseOrder.VendorID AND  
                                    rep.RefProductID = dbo.tb_PurchaseOrderItems.ProductID AND rep.OrderNumber = dbo.tb_PurchaseOrder.OrderNumber 
                                    inner join dbo.tb_OrderShippedItems on tb_OrderShippedItems.RefProductID = dbo.tb_PurchaseOrderItems.ProductID  
                                    WHERE dbo.tb_PurchaseOrderItems.PONumber= " + Pono + " and tb_OrderShippedItems.Ordernumber = dbo.tb_PurchaseOrder.OrderNumber");

        }

        /// <summary>
        /// Get Vendor Products for Warehouse
        /// </summary>
        /// <param name="Pono">Int32 Pono</param>
        /// <returns>Returns all vendor products for displaying in WareHouse </returns>
        public DataSet GetVendorProductsforWarehouse(int Pono)
        {
            objSql = new SQLAccess();
            return objSql.GetDs(@"SELECT  dbo.tb_Product.Name, dbo.tb_PurchaseOrderItems.Quantity, dbo.tb_PurchaseOrderItems.PONumber,dbo.tb_PurchaseOrder.OrderNumber,
                                dbo.tb_PurchaseOrderItems.ProductID, dbo.tb_PurchaseOrderItems.Price, ISNULL(dbo.tb_PurchaseOrderItems.IsShipped, 0) 
                                AS IsShipped,ISNULL(dbo.tb_PurchaseOrderItems.Ispaid, 0) as Ispaid,  dbo.tb_Product.SKU, dbo.tb_Vendor.Name AS Vname, dbo.tb_Vendor.Email, 
                                dbo.tb_PurchaseOrder.OrderNumber,dbo.tb_PurchaseOrderItems.TrackingNumber, dbo.tb_PurchaseOrder.PODate,dbo.tb_Vendor.Phone,isnull(tb_PurchaseOrderItems.ShippedVia,'') as ShippedVia,isnull(tb_PurchaseOrderItems.ShippedOn,'') as ShippedOn 
                                FROM dbo.tb_Product INNER JOIN
                                dbo.tb_PurchaseOrderItems ON dbo.tb_Product.ProductID = dbo.tb_PurchaseOrderItems.ProductID INNER JOIN
                                dbo.tb_PurchaseOrder ON dbo.tb_PurchaseOrderItems.PONumber = dbo.tb_PurchaseOrder.PONumber INNER JOIN
                                dbo.tb_Vendor ON dbo.tb_PurchaseOrder.VendorID = dbo.tb_Vendor.VendorId LEFT OUTER JOIN
                                dbo.tb_VendorQuoteReply AS rep ON rep.VendorID = dbo.tb_PurchaseOrder.VendorID AND  
                                rep.RefProductID = dbo.tb_PurchaseOrderItems.ProductID AND rep.OrderNumber = dbo.tb_PurchaseOrder.OrderNumber 
                                WHERE dbo.tb_PurchaseOrderItems.PONumber=" + Pono + " AND tb_PurchaseOrder.OrderNumber=0");

        }

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
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_VendorQuoteRequest";
            cmd.Parameters.AddWithValue("@VendorID", VendorId);
            cmd.Parameters.AddWithValue("@VendorQuoteRequestID", VendorQuoteReqId);
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@ProductName", Name);
            cmd.Parameters.AddWithValue("@ProductOption", ProductOption);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@Mode", 1);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

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
        public int SaveVendorQuoteReply(Int32 VendorQuoteRequestID, Int32 VendorId, Int32 ProductId, Int32 Quantity, String Name, String ProductOption, String Notes, Decimal Price, Int32 AvailDays, String Location)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_VendorQuoteReply";

            cmd.Parameters.AddWithValue("@VendorQuoteRequestID", VendorQuoteRequestID);
            cmd.Parameters.AddWithValue("@VendorID", VendorId);
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@ProductName", Name);
            cmd.Parameters.AddWithValue("@ProductOption", ProductOption);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@AvailableDays", AvailDays);
            cmd.Parameters.AddWithValue("@Notes", Notes);
            cmd.Parameters.AddWithValue("@Location", Location);
            cmd.Parameters.AddWithValue("@Mode", 1);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        public DataSet GetDropShipperList()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DropShipper";
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        public DataSet GetVendorList(int status)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Vendor_GetVendorList";
            cmd.Parameters.AddWithValue("@IsDropshipper", status);
            return objSql.GetDs(cmd);
        }

        public DataSet GetDropShipperListSearched(string Name)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.CommandText = "usp_DropShipper";
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }


        public DataSet GetDropShipperListbyvendor(int vendorid)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@VendorID", vendorid);
            cmd.CommandText = "usp_DropShipper";
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }
        public DataSet GetProductList(Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AssemblerProduct";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }
        public DataSet GetProductListSearched(string Name, Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AssemblerProduct";
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        public void InsertDropshipProduct(Int32 ProductID, Int32 VendorID, Int32 VendorSKUID, string VendorSKU, string Priority)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Vendor_InsertVendorSKU";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@VendorID", VendorID);
            cmd.Parameters.AddWithValue("@VendorSKUID", VendorSKUID);
            cmd.Parameters.AddWithValue("@VendorSKU", VendorSKU);
            cmd.Parameters.AddWithValue("@Priority", Priority);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
        }

        public DataSet GetDropShipperListByProductID(Int32 ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetDropShipperListByProductID";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            return objSql.GetDs(cmd);
        }
        public DataSet GetAllAssemblerProductSKUByProductID(Int32 ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetAllAssemblerProductSKUByProductID";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            return objSql.GetDs(cmd);
        }


        public DataSet GetProductSKUListByProductID(Int32 RefProductID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetProductSKUListByProductID";
            cmd.Parameters.AddWithValue("@RefProductID", RefProductID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        public DataSet GetProductTypeDeliveryByID(int ProductDeliveryID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetProductTypeDeliveryByID";
            cmd.Parameters.AddWithValue("@ProductDeliveryID", ProductDeliveryID);
            return objSql.GetDs(cmd);
        }


        public void InsertAssemblerProduct(Int32 RefProductID, Int32 ProductID, Int32 Quantity, Int32 CreatedBy, Int32 UpdatedBy)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Assembly_InsertProductAssembly";
            cmd.Parameters.AddWithValue("@RefProductID", RefProductID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
        }

        #endregion
    }
}
