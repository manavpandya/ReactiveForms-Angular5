using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using Solution.Data;
using System.Data;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Product Buy Component Class Contains Product Buy Buy related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ProductBuyComponent
    {
        /// <summary>
        /// Method for Insert Product
        /// </summary>
        /// <param name="product">tb_ProductBuy productBuy</param>
        /// <returns>Returns Inserted Unique ID of Product</returns>
        public static Int32 InsertProductBuy(tb_ProductBuy productBuy)
        {
            int proBuyID = 0;
            try
            {
                ProductBuyDAC dac = new ProductBuyDAC();
                dac.Create(productBuy);
                proBuyID = productBuy.ProductBuyID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proBuyID;
        }

        /// <summary>
        /// Update ProductBuy Table
        /// </summary>
        /// <param name="productBuy">tb_ProductBuy productBuy</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public static int UpdateProduct(tb_ProductBuy productBuy)
        {
            int RowsAffected = 0;
            try
            {
                ProductBuyDAC dac = new ProductBuyDAC();
                RowsAffected = dac.Update(productBuy);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }


        /// <summary>
        /// Get ProductBuy Data
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns the LIst of Product Buy Data as a Dataset</returns>
        public DataSet GetProductBuyData(int ProductID)
        {
            ProductBuyDAC objProductDAC = new ProductBuyDAC();
            return objProductDAC.GetProductBuyData(ProductID);
        }
    }
}
