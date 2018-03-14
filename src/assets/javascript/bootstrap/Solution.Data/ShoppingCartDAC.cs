using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// ShoppingCart Data Access Class Contains ShoppingCart related Data Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class ShoppingCartDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Delete Cart Items by CustomerID 
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        public void DeleteCartItems(Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShoppingCart";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Update CustomerID for Shopping cart with New CustomerID from Old CustomerID
        /// </summary>
        /// <param name="NewCustomerID">Int32 NewCustomerID</param>
        /// <param name="OldCustomerID">Int32 OldCustomerID</param>
        public void UpdateCustomerForCart(Int32 NewCustomerID, Int32 OldCustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShoppingCart";
            cmd.Parameters.AddWithValue("@NewCustomerID", NewCustomerID);
            cmd.Parameters.AddWithValue("@OldCustomerID", OldCustomerID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Get Shopping Cart Details By Customer ID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns>Returns the Cart Details of particular customer</returns>
        public DataSet GetCartDetailByCustomerID(Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AddToCartByCustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@opt", 1);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Phone Order Shopping Cart Details By Customer ID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns>Returns the Cart Details of particular customer for phone order</returns>
        public DataSet GetPhoneOrderCartDetailByCustomerID(Int32 CustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_PhoneOrder_GetCartByCustomerID";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to Delete Item in Shopping Cart 
        /// </summary>
        /// <param name="CustomCartID">Int32 CustomCartID</param>
        public void DeleteCartItemByCustomCartID(Int32 CustomCartID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AddToCartByCustomerID";
            cmd.Parameters.AddWithValue("@CustomCartID", CustomCartID);
            cmd.Parameters.AddWithValue("@opt", 2);
            objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to update shopping cart item qty
        /// </summary>
        /// <param name="CustomCartID">Int32 CustomCartID</param>
        /// <param name="Qty">Int32 Qty</param>
        public void UpdateCartItemQtyByCustomCartID(Int32 CustomCartID, Int32 Qty)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AddToCartByCustomerID";
            cmd.Parameters.AddWithValue("@CustomCartID", CustomCartID);
            cmd.Parameters.AddWithValue("@Quantity", Qty);
            cmd.Parameters.AddWithValue("@opt", 3);
            objSql.GetDs(cmd);
        }

        /// <summary>
        /// Add Item Into Cart
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns></returns>
        public string AddItemIntoCart(Int32 CustomerID, Int32 ProductID, Int32 Qty, decimal Price, string VariantNameId, string VariantValueId, string VariantName, string VariantValue, Int32 parentProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShoppingCartItems_AddorUpdateItem";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Qty", Qty);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@VariantNameId", VariantNameId);
            cmd.Parameters.AddWithValue("@VariantValueId", VariantValueId);
            cmd.Parameters.AddWithValue("@VariantName", VariantName);
            cmd.Parameters.AddWithValue("@VariantValue", VariantValue);
            cmd.Parameters.AddWithValue("@parentProductId", parentProductId);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }



        /// <summary>
        /// Add Item Into Cart
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns></returns>
        public string RMAAddItemIntoCart(Int32 CustomerID, Int32 ProductID, Int32 Qty, decimal Price, string VariantNameId, string VariantValueId, string VariantName, string VariantValue, Int32 parentProductId, Int32 ShoppingCartID)
        {

            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.CommandText = "usp_RMAShoppingCartItems_AddorUpdateItem";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Qty", Qty);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@VariantNameId", VariantNameId);
            cmd.Parameters.AddWithValue("@VariantValueId", VariantValueId);
            cmd.Parameters.AddWithValue("@VariantName", VariantName);
            cmd.Parameters.AddWithValue("@VariantValue", VariantValue);
            cmd.Parameters.AddWithValue("@parentProductId", parentProductId);
            cmd.Parameters.AddWithValue("@ShoppingCartID1", ShoppingCartID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));

        }

        /// <summary>
        /// Get Sales Tax
        /// </summary>
        /// <param name="stateName">String StateName</param>
        /// <param name="zipCode">String ZipCode</param>
        /// <param name="orderAmount">Decimal OrderAmount </param>
        /// <param name="StoreId">int CurrentStoreId</param>
        /// <returns>Returns Decimal Value</returns>
        public Decimal GetSalesTax(string stateName, string zipCode, decimal orderAmount, int StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetOrderSalesTax";
            cmd.Parameters.AddWithValue("@StateName", stateName);
            cmd.Parameters.AddWithValue("@zipCode", zipCode);
            cmd.Parameters.AddWithValue("@orderAmount", orderAmount);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            return Convert.ToDecimal(objSql.ExecuteScalarQuery(cmd));
        }
        /// <summary>
        /// Get Sales Tax Details
        /// </summary>
        /// <param name="stateName">String StateName</param>
        /// <param name="zipCode">String ZipCode</param>
        /// <param name="orderAmount">Decimal OrderAmount </param>
        /// <param name="StoreId">int CurrentStoreId</param>
        /// <returns>Returns Decimal Value</returns>
        public Decimal GetSalesTaxDetails(string stateName, string zipCode, decimal orderAmount, int StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetOrderTaxRate";
            cmd.Parameters.AddWithValue("@StateName", stateName);
            cmd.Parameters.AddWithValue("@zipCode", zipCode);
            cmd.Parameters.AddWithValue("@orderAmount", orderAmount);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            return Convert.ToDecimal(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Add cart into WishList
        /// </summary>
        /// <param name="CustomerId">Int32 CustomerId</param>
        public void AddToWishList(Int32 CustomerId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WishListItems_AddCartInWishList";
            cmd.Parameters.AddWithValue("@CustomerId", CustomerId);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Add Item Into Cart reorder
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns>Returns the string value</returns>
        public string AddItemIntoCartReorder(Int32 CustomerID, Int32 ProductID, Int32 Qty, decimal Price, string VariantNameId, string VariantValueId, string VariantName, string VariantValue)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShoppingCartItems_AddorUpdateItemReorder";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Qty", Qty);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@VariantNameId", VariantNameId);
            cmd.Parameters.AddWithValue("@VariantValueId", VariantValueId);
            cmd.Parameters.AddWithValue("@VariantName", VariantName);
            cmd.Parameters.AddWithValue("@VariantValue", VariantValue);
            cmd.Parameters.AddWithValue("@Mode", 1);
            //SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.NVarChar);
            //paramReturnval.Direction = ParameterDirection.ReturnValue;
            //cmd.Parameters.Add(paramReturnval);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Ordered Shopping Cart Items By CartId
        /// </summary>
        /// <param name="ONo">Int32 ONo</param>
        /// <returns>Returns the shopping cart items for particular CartID as a Dataset</returns>
        public DataSet GetOrderedShoppingCartItemsByCartId(Int32 ONo)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ShippingLabel_GetShoppingCart";
            cmd.Parameters.AddWithValue("@OrderNumber", ONo);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Add Gift Item Into Cart
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns>Returns the string value</returns>
        public string AddGiftItemIntoCart(Int32 CustomerID, Int32 ProductID, Int32 Qty, decimal Price, string VariantNameId, string VariantValueId, string VariantName, string VariantValue)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GiftShoppingCartItems_AddorUpdateItem";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Qty", Qty);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@VariantNameId", VariantNameId);
            cmd.Parameters.AddWithValue("@VariantValueId", VariantValueId);
            cmd.Parameters.AddWithValue("@VariantName", VariantName);
            cmd.Parameters.AddWithValue("@VariantValue", VariantValue);
            cmd.Parameters.AddWithValue("@Mode", 1);
            //SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.NVarChar);
            //paramReturnval.Direction = ParameterDirection.ReturnValue;
            //cmd.Parameters.Add(paramReturnval);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }
        #endregion
    }
}
