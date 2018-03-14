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
    /// WishListItems Component Class Contains WishListItems related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class WishListItemsComponent
    {
        #region Declaration

        public Int32 StoreID = 0;
        WishListItemsDAC objWishListItems = null;
        #endregion

        #region All Constructors

        /// <summary>
        /// Constructor with out parameter that initializes StoreID when component file is loading
        /// </summary>
        public WishListItemsComponent()
        {
            StoreID = AppConfig.StoreID;
        }

        #endregion

        #region  Key Function

        /// <summary>
        /// Update the WistList records of Customer from Old CustomerID with New CustomerID
        /// </summary>
        /// <param name="NewCustomerID">Int32 NewCustomerID</param>
        /// <param name="OldCustomerID">Int32 OldCustomerID</param>
        public void UpdateCustomerForWishList(Int32 NewCustomerID, Int32 OldCustomerID)
        {
            objWishListItems = new WishListItemsDAC();
            objWishListItems.UpdateCustomerForWishList(NewCustomerID, OldCustomerID);
        }


        /// <summary>
        /// Check Blocked IPAddress By IPAddress By StoreID
        /// </summary>
        /// <param name="IPAddress">String IPAddress</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns true if exist else false</returns>
        public bool CheckBlockedIPAddressByIPAddress(String IPAddress)
        {
            objWishListItems = new WishListItemsDAC();
            return objWishListItems.CheckBlockedIPAddressByIPAddress(IPAddress, StoreID);
        }


        /// <summary>
        /// Get All Wish List Item BY CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns All Item List of WishList for CustomerID</returns>
        public DataSet GetAllWishListItemBYCustID(Int32 CustomerID)
        {
            objWishListItems = new WishListItemsDAC();
            return objWishListItems.GetAllWishListItemBYCustID(CustomerID, StoreID);
        }

        /// <summary>
        /// Remove Items From Wish List
        /// </summary>
        /// <param name="WishListID">Int32 WishListID</param>
        /// <returns>Returns 1 = Removed</returns>
        public Int32 RemoveItemsFromWishList(Int32 WishListID)
        {
            objWishListItems = new WishListItemsDAC();
            return objWishListItems.RemoveItemsFromWishList(WishListID);
        }

        /// <summary>
        /// Update Items of Wish List
        /// </summary>
        /// <param name="WishListID">Int32 WishListID</param>
        /// <param name="Quantity">Int32 Quantity</param>
        /// <returns>Returns 1 = Updated</returns>
        public Int32 UpdateItemsOfWishList(Int32 WishListID, Int32 Quantity)
        {
            objWishListItems = new WishListItemsDAC();
            return objWishListItems.UpdateItemsOfWishList(WishListID, Quantity);
        }

        /// <summary>
        /// Add Product From Wish List to Add To Cart
        /// </summary>
        /// <param name="WishListID">Int32 WishListID</param>
        /// <returns>Return Result as a String</returns>
        public String WishListToAddtoCart(Int32 WishListID)
        {
            objWishListItems = new WishListItemsDAC();
            return objWishListItems.WishListToAddtoCart(WishListID);
        }

        /// <summary>
        /// Insert Items directly into WishList From Login & Create Account Page
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="Quantity"> Int32 Quantity</param>
        /// <param name="Price">Decimal Price</param>
        /// <param name="VariantNameId">String VariantNameId</param>
        /// <param name="VariantValueId">String VariantValueId</param>
        /// <returns>Return Identity ID of Inserted Record</returns>
        public Int32 AddWishListItems(Int32 CustomerID, Int32 ProductID, Int32 Quantity, Decimal Price, String VariantNameId, String VariantValueId)
        {
            objWishListItems = new WishListItemsDAC();
            return objWishListItems.AddWishListItems(CustomerID, ProductID, Quantity, Price, VariantNameId, VariantValueId);
        }

        #endregion
    }
}
