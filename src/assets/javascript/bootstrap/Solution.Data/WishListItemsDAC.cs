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
    /// WishListItems Data Access Class Contains WishListItems related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class WishListItemsDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        #endregion

        #region Key Functions

        /// <summary>
        /// Update the WistList record of Customer from Old CustomerID with New CustomerID
        /// </summary>
        /// <param name="NewCustomerID">Int32 NewCustomerID</param>
        /// <param name="OldCustomerID">Int32 OldCustomerID</param>
        public void UpdateCustomerForWishList(Int32 NewCustomerID, Int32 OldCustomerID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WishListItems";
            cmd.Parameters.AddWithValue("@NewCustomerID", NewCustomerID);
            cmd.Parameters.AddWithValue("@OldCustomerID", OldCustomerID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Check Blocked IPAddress By IPAddress By StoreID
        /// </summary>
        /// <param name="IPAddress">String IPAddress</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns true if exist else false</returns>
        public bool CheckBlockedIPAddressByIPAddress(String IPAddress, Int32 StoreID)
        {
            objSql = new SQLAccess();
            bool isExist = false;
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WishListItems";
            cmd.Parameters.AddWithValue("@HostIPAddrss", IPAddress);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            Int32 cnt = Convert.ToInt32(objSql.ExecuteScalarQuery(cmd));
            if (cnt > 0)
            {
                isExist = true;
            }
            return isExist;
        }

        /// <summary>
        /// Get All Wish List Item BY CustomerID
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns All Item List of WishList for CustomerID</returns>
        public DataSet GetAllWishListItemBYCustID(Int32 CustomerID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WishListItems";
            cmd.Parameters.AddWithValue("@CustomerID", @CustomerID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Remove Items From Wish List
        /// </summary>
        /// <param name="WishListID">Int32 WishListID</param>
        /// <returns>Returns 1 = Removed</returns>
        public Int32 RemoveItemsFromWishList(Int32 WishListID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WishListItems";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@WishListID", WishListID);
            cmd.Parameters.AddWithValue("@Mode", 4);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Items of Wish List
        /// </summary>
        /// <param name="WishListID">Int32 WishListID</param>
        /// <param name="Quantity">Int32 Quantity</param>
        /// <returns>Returns 1 = Updated</returns>
        public Int32 UpdateItemsOfWishList(Int32 WishListID, Int32 Quantity)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WishListItems";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@WishListID", WishListID);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@Mode", 5);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Add Product From Wish List to Add To Cart
        /// </summary>
        /// <param name="WishListID">Int32 WishListID</param>
        /// <returns>Return Result as a String</returns>
        public String WishListToAddtoCart(Int32 WishListID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WishListItems_WishListToAddtoCart";
            cmd.Parameters.AddWithValue("@WishListID", WishListID);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
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
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WishList_InsertItems";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Quantity", Quantity);
            cmd.Parameters.AddWithValue("@Price", Price);
            cmd.Parameters.AddWithValue("@VariantNameId", "");
            cmd.Parameters.AddWithValue("@VariantValueId", "");
            cmd.Parameters.AddWithValue("@VariantName", VariantNameId);
            cmd.Parameters.AddWithValue("@VariantValue", VariantValueId);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }


        #endregion
    }
}
