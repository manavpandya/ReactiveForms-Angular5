using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using System.Data;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// ProductBuy Data Access Class Contains Product Buy related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ProductBuyDAC
    {
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        /// <summary>
        /// Inserts a Product row
        /// </summary>
        /// <param name="product">tb_ProductBuy productBuy</param>
        /// <returns>Returns tb_ProductBuy Table Object</returns>
        public tb_ProductBuy Create(tb_ProductBuy productBuy)
        {
            ctx.AddTotb_ProductBuy(productBuy);
            ctx.SaveChanges();
            return productBuy;
        }

        /// <summary>
        /// Update ProductBuy Table
        /// </summary>
        /// <param name="productBuy">tb_ProductBuy productBuy</param>
        /// <returns>Returns 1 if Updated/returns>
        public int Update(tb_ProductBuy productBuy)
        {
            int RowsAffected = 0;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductBuy_UpdateProductBuy";
            cmd.Parameters.AddWithValue("@ProductBuyID", productBuy.ProductBuyID);
            cmd.Parameters.AddWithValue("@ShippingRateExpedited", productBuy.ShippingRateExpedited);
            cmd.Parameters.AddWithValue("@ShippingRateStandard", productBuy.ShippingRateStandard);
            cmd.Parameters.AddWithValue("@OfferExpeditedShipping", productBuy.OfferExpeditedShipping);
            cmd.Parameters.AddWithValue("@Ass_Type", productBuy.Ass_Type);
            cmd.Parameters.AddWithValue("@Board_Type", productBuy.Board_Type);
            cmd.Parameters.AddWithValue("@Brightness", productBuy.Brightness);
            cmd.Parameters.AddWithValue("@Card_Type", productBuy.Card_Type);
            cmd.Parameters.AddWithValue("@Closure", productBuy.Closure);
            cmd.Parameters.AddWithValue("@Dispenser_Type", productBuy.Dispenser_Type);
            cmd.Parameters.AddWithValue("@Easel_Type", productBuy.Easel_Type);
            return RowsAffected = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Get ProductBuy Data
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns the LIst of Product Buy Data as a Dataset</returns>
        public DataSet GetProductBuyData(int ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductBuy_GetProductBuyData";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            return objSql.GetDs(cmd);
        }


    }
}
