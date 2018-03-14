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

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Change Order Component Class Contains Change Order related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class ChangeOrderComponent
    {

        #region Key Function

        /// <summary>
        /// Get Change Order Cart Details using Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>Returns Order Cart Details</returns>
        public static DataSet GetChangeOrderCartByOrderNumber(Int32 OrderNumber)
        {
            DataSet DSchangeorder = new DataSet();
            ChangeOrderDAC changeorder = new ChangeOrderDAC();
            DSchangeorder = changeorder.GetChangeOrderCartByOrderNumber(OrderNumber);
            return DSchangeorder;
        }

        /// <summary>
        /// Delete Records From Change Order cart
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="VariantNames">String VariantNames</param>
        /// <param name="VariantValues">String VariantValues</param>
        /// <returns>Returns Status of Order Deleted or Not</returns>
        public bool DeleteChangeOrderCartItem(Int32 OrderNumber, Int32 ProductID, String VariantNames, String VariantValues)
        {
            ChangeOrderDAC changeorder = new ChangeOrderDAC();
            return Convert.ToBoolean(changeorder.DeleteChangeOrderCartItem(OrderNumber, ProductID, VariantNames, VariantValues));
        }
        #endregion

        /// <summary>
        /// Update Order at Review Time
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <param name="SubTotal">decimal SubTotal</param>
        /// <param name="Total">decimal Total</param>
        /// <param name="ShippingCost">decimal ShippingCost</param>
        /// <param name="ShippingMethodID">Int32 ShippingMethodID</param>
        /// <param name="ShippingMethod">string ShippingMethod</param>
        /// <param name="Discount">decimal Discount</param>
        /// <returns>Returns Status of Updated Cart</returns>
        public bool UpdateChangedCart(Int32 OrderNumber, decimal SubTotal, decimal Total, decimal ShippingCost, Int32 ShippingMethodID, string ShippingMethod, decimal Discount)
        {
            ChangeOrderDAC changeorder = new ChangeOrderDAC();
            return Convert.ToBoolean(changeorder.UpdateChangedCart(OrderNumber, SubTotal, Total, ShippingCost, ShippingMethodID, ShippingMethod, Discount));
        }

        /// <summary>
        /// Save Change Order in "Save Click"
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        public void ExportOrderCart(int OrderNumber)
        {
            ChangeOrderDAC changeorder = new ChangeOrderDAC();
            changeorder.ExportOrderCart(OrderNumber);
        }
    }
}
