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
    /// Change Order Data Access Class Contains Change Order Related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>    

    public class ChangeOrderDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Change Order Cart Details using Order Number
        /// </summary>
        /// <param name="OrderNumber">Int32 OrderNumber</param>
        /// <returns>Returns Order Cart Details</returns>
        public DataSet GetChangeOrderCartByOrderNumber(Int32 OrderNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "select isnull(ci.RelatedproductID,0) as RelatedproductID,ISNULL(DiscountPrice,0) as DiscountPrice,ci.ProductID,p.Name,p.SKU,ci.[Quantity],ci.[Price] as Price,(ISNULL(ci.[Price],0) * ISNULL(ci.[Quantity],0)) as SalePrice,isnull(ci.[VariantNames],'') as 'VariantNames', " +
                                                        " isnull(ci.[VariantValues],'') as 'VariantValues',ci.createdon from tb_ChangeOrderShoppingCartItems ci  " +
                                                        " join tb_Product p on ci.ProductID=p.ProductID where OrderNumber=" + OrderNumber + " order by createdon";
            return objSql.GetDs(cmd);
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
            objSql = new SQLAccess();
            string _Query = String.Empty;
            _Query = "declare @Price as Money set @Price=(Select top 1 (ISNULL(Price,0) * ISNULL(Quantity,0)) from tb_ChangeOrderShoppingCartItems where ProductID=" + ProductID
                + " And ((isnull(VariantNames,'')=@VariantNames And isnull(VariantValues,'')=@VariantValues )  ) And OrderNumber=@OrderNumber)"
                + " update tb_ChangeOrderedShoppingCart set SubTotal=SubTotal- @Price,Total=Total-@Price where OrderNumber=@OrderNumber "
                + "  Delete From tb_ChangeOrderShoppingCartItems Where ProductID=" + ProductID + " And "
                + " ((isnull(VariantNames,'')=@VariantNames  And isnull(VariantValues,'')=@VariantValues ) )  "
                + " And OrderNumber=@OrderNumber";

            _Query += " Delete from tb_ChangeOrderShoppingCartItems Where isnull(RelatedproductID,0) <> 0 AND RelatedproductID in (" + ProductID.ToString() + ") and OrderNumber=" + OrderNumber + " ";

            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = _Query;
            cmd.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = OrderNumber;
            cmd.Parameters.Add("@VariantNames", SqlDbType.NVarChar).Value = VariantNames;
            cmd.Parameters.Add("@VariantValues", SqlDbType.NVarChar).Value = VariantValues;
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
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
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ChangeOrderedShoppingCart";
            cmd.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = OrderNumber;
            cmd.Parameters.Add("@SubTotal", SqlDbType.Money).Value = SubTotal;
            cmd.Parameters.Add("@ShippingAmount", SqlDbType.Money).Value = ShippingCost;
            cmd.Parameters.Add("@ShippingMethod", SqlDbType.NVarChar).Value = ShippingMethod;
            cmd.Parameters.Add("@ShippingMethodID", SqlDbType.Int).Value = ShippingMethodID;
            cmd.Parameters.Add("@DiscountAmount", SqlDbType.Money).Value = Discount;
            cmd.Parameters.Add("@Total", SqlDbType.Money).Value = Total;
            cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = 2;
            return Convert.ToBoolean(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Save Change Order in "Save Click"
        /// </summary>
        /// <param name="OrderNumber">int OrderNumber</param>
        public void ExportOrderCart(int OrderNumber)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand("usp_ChangeOrderedShoppingCart");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@OrderNumber", SqlDbType.Int).Value = OrderNumber;
            cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = 4;
            objSql.ExecuteNonQuery(cmd);
        }
    }
}
