using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Transactions;
using Solution.Data;
using System.Web;
using Solution.Bussines.Components.Common;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Shopping Cart Component Class Contains Shopping Cart related Business Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class ShoppingCartComponent
    {
        #region Declaration

        public Int32 StoreID = 0;
        ShoppingCartDAC objShoppingCart = null;
        #endregion

        #region Constructors

        /// <summary>
        /// Constructor with out parameter that initializes StoreID when component file is loading
        /// </summary>
        public ShoppingCartComponent()
        {
            StoreID = AppConfig.StoreID;
        }

        #endregion

        #region  Key Functions

        /// <summary>
        /// Delete Cart Items by CustomerID 
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        public void DeleteCartItems(Int32 CustomerID)
        {
            objShoppingCart = new ShoppingCartDAC();
            objShoppingCart.DeleteCartItems(CustomerID);
        }

        /// <summary>
        /// Update CustomerID for Shopping cart with New CustomerID from Old CustomerID
        /// </summary>
        /// <param name="NewCustomerID">Int32 NewCustomerID</param>
        /// <param name="OldCustomerID">Int32 OldCustomerID</param>
        public void UpdateCustomerForCart(Int32 NewCustomerID, Int32 OldCustomerID)
        {
            objShoppingCart = new ShoppingCartDAC();
            objShoppingCart.UpdateCustomerForCart(NewCustomerID, OldCustomerID);
        }

        /// <summary>
        /// Get Shopping Cart Details By Customer ID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns>Returns the Cart Details of particular customer</returns>
        public static DataSet GetCartDetailByCustomerID(Int32 CustomerID)
        {
            ShoppingCartDAC dac = new ShoppingCartDAC();
            return dac.GetCartDetailByCustomerID(CustomerID);
        }

        /// <summary>
        /// Get Phone Order Shopping Cart Details By Customer ID
        /// </summary>
        /// <param name="CustomerID"></param>
        /// <returns>Returns the Cart Details of particular customer for phone order</returns>
        public static DataSet GetPhoneOrderCartDetailByCustomerID(Int32 CustomerID)
        {
            ShoppingCartDAC dac = new ShoppingCartDAC();
            return dac.GetPhoneOrderCartDetailByCustomerID(CustomerID);
        }

        /// <summary>
        /// Method to Delete Item in Shopping Cart 
        /// </summary>
        /// <param name="CustomCartID">Int32 CustomCartID</param>
        public void DeleteCartItemByCustomCartID(Int32 CustomCartID)
        {
            objShoppingCart = new ShoppingCartDAC();
            objShoppingCart.DeleteCartItemByCustomCartID(CustomCartID);
        }

        /// <summary>
        /// Update Cart Item Qty By CustomCartID
        /// </summary>
        /// <param name="CustomCartID">Int32 CustomCartID</param>
        /// <param name="Qty">Int32 Qty</param>
        public void UpdateCartItemQtyByCustomCartID(Int32 CustomCartID, Int32 Qty)
        {
            objShoppingCart = new ShoppingCartDAC();
            objShoppingCart.UpdateCartItemQtyByCustomCartID(CustomCartID, Qty);
        }

        /// <summary>
        /// Add Item Into Cart
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns>Returns String</returns>
        public string AddItemIntoCart(Int32 CustomerID, Int32 ProductID, Int32 Qty, decimal Price, string VariantNameId, string VariantValueId, string VariantName, string VariantValue, Int32 parentProductId)
        {
            objShoppingCart = new ShoppingCartDAC();
            return objShoppingCart.AddItemIntoCart(CustomerID, ProductID, Qty, Price, VariantNameId, VariantValueId, VariantName, VariantValue, parentProductId);
        }

        /// <summary>
        /// Get Sales Tax
        /// </summary>
        /// <param name="stateName">String StateName</param>
        /// <param name="zipCode">String ZipCode</param>
        /// <param name="orderAmount">Decimal OrderAmount </param>
        /// <param name="StoreId">Int CurrentStoreId</param>
        /// <returns>Returns Decimal Value</returns>
        public Decimal GetSalesTax(string stateName, string zipCode, decimal orderAmount, int StoreId)
        {
            objShoppingCart = new ShoppingCartDAC();
            return objShoppingCart.GetSalesTax(stateName, zipCode, orderAmount, StoreId);
        }
        /// <summary>
        /// Get Sales Tax Details
        /// </summary>
        /// <param name="stateName">String StateName</param>
        /// <param name="zipCode">String ZipCode</param>
        /// <param name="orderAmount">Decimal OrderAmount </param>
        /// <param name="StoreId">Int CurrentStoreId</param>
        /// <returns>Returns Decimal Value</returns>
        public Decimal GetSalesTaxDetails(string stateName, string zipCode, decimal orderAmount, int StoreId)
        {
            objShoppingCart = new ShoppingCartDAC();
            return objShoppingCart.GetSalesTaxDetails(stateName, zipCode, orderAmount, StoreId);
        }

        /// <summary>
        /// Add cart into WishList
        /// </summary>
        /// <param name="CustomerId">Int32 CustomerId</param>
        public void AddToWishList(Int32 CustomerId)
        {
            objShoppingCart = new ShoppingCartDAC();
            objShoppingCart.AddToWishList(CustomerId);
        }


        /// <summary>
        /// Add Item Into Cart Reorder
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns>Returns the string value</returns>
        public string AddItemIntoCartReorder(Int32 CustomerID, Int32 ProductID, Int32 Qty, decimal Price, string VariantNameId, string VariantValueId, string VariantName, string VariantValue)
        {
            objShoppingCart = new ShoppingCartDAC();
            return objShoppingCart.AddItemIntoCartReorder(CustomerID, ProductID, Qty, Price, VariantNameId, VariantValueId, VariantName, VariantValue);
        }

        /// <summary>
        /// Get Ordered Shopping Cart Items By CartId
        /// </summary>
        /// <param name="ONo">Int32 ONo</param>
        /// <returns>Returns the shopping cart items for particular CartID as a Dataset</returns>
        public DataSet GetOrderedShoppingCartItemsByCartId(Int32 CartID)
        {
            objShoppingCart = new ShoppingCartDAC();
            return objShoppingCart.GetOrderedShoppingCartItemsByCartId(CartID);

        }

        /// <summary>
        /// Add Gift Item Into Cart
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// /// <returns>Returns the string value</returns>
        public string AddGiftItemIntoCart(Int32 CustomerID, Int32 ProductID, Int32 Qty, decimal Price, string VariantNameId, string VariantValueId, string VariantName, string VariantValue)
        {
            objShoppingCart = new ShoppingCartDAC();
            return objShoppingCart.AddGiftItemIntoCart(CustomerID, ProductID, Qty, Price, VariantNameId, VariantValueId, VariantName, VariantValue);
        }



        /// <summary>
        /// Add Item Into Cart
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="Qty">Int32 Qty</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns>Returns String</returns>
        public string RMAAddItemIntoCart(Int32 CustomerID, Int32 ProductID, Int32 Qty, decimal Price, string VariantNameId, string VariantValueId, string VariantName, string VariantValue, Int32 parentProductId, Int32 ShoppingCartID)
        {
            objShoppingCart = new ShoppingCartDAC();
            return objShoppingCart.RMAAddItemIntoCart(CustomerID, ProductID, Qty, Price, VariantNameId, VariantValueId, VariantName, VariantValue, parentProductId, ShoppingCartID);

        }
        #endregion
    }
}
