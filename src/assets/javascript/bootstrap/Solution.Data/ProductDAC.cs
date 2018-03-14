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
using System.IO;

namespace Solution.Data
{
    /// <summary>
    /// Product Data Access Class Contains Product related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ProductDAC
    {

        #region Declaration

        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Inserts a Product row
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns Product table object</returns>
        public tb_Product Create(tb_Product product)
        {

            try
            {
                ctx.AddTotb_Product(product);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }

            return product;
        }

        /// <summary>
        /// Inserts a Product Amazon row
        /// </summary>
        /// <param name="product">A Product Amazon object</param>
        /// <returns>Returns Product table object</returns>
        public tb_ProductAmazon CreateAmazon(tb_ProductAmazon productAmazon)
        {

            try
            {
                ctx.AddTotb_ProductAmazon(productAmazon);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }

            return productAmazon;
        }

        /// <summary>
        /// Update Display Order For Index Page
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="DisplayOrder">String DisplayOrder</param>
        public void UpdateDisplayOrderForIndexPage(string Option, Int32 ProductId, string DisplayOrder)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByCategoryId";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@Option", Option);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Opt", 2);
            objSql.ExecuteNonQuery(cmd);
        }


        /// <summary>
        /// Insert a Product Category
        /// </summary>
        /// <param name="productCategory">tb_ProductCategory productCategory</param>
        /// <returns>Returns the Product Category Table Object</returns>
        public tb_ProductCategory CreateProductCategory(tb_ProductCategory productCategory)
        {
            try
            {
                int query = (from pc in ctx.tb_ProductCategory
                             where pc.CategoryID == productCategory.CategoryID &&
                             pc.ProductID == productCategory.ProductID
                             select pc).Count();
                if (query >= 1)
                {
                    tb_ProductCategory proCategory = ctx.tb_ProductCategory.FirstOrDefault(tb_ProductCategory => tb_ProductCategory.CategoryID == productCategory.CategoryID && tb_ProductCategory.ProductID == productCategory.ProductID);
                    ctx.DeleteObject(proCategory);
                    ctx.SaveChanges();
                }

                ctx.AddTotb_ProductCategory(productCategory);
                ctx.SaveChanges();
            }
            catch (Exception ae)
            {
                Debug.WriteLine(ae.Message);
                throw ae;

            }
            return productCategory;
        }

        /// <summary>
        /// Get Display Order By ProductID And CategoryID
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <returns>Returns the display orders for category by product and category ids</returns>
        public DataSet GetDisplayOrderByProductIDAndCategoryID(Int32 ProductID, Int32 CategoryID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByCategoryId";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@opt", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Updates a Product row
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns the affected rows count</returns>
        public int Update(tb_Product product)
        {
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
        /// Get Product Details by Product ID
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <returns>Returns a product details for particular product</returns>
        public tb_Product GetAllProductDetailsbyProductID(Int32 ProductID)
        {
            tb_Product tb_Product = new tb_Product();
            tb_Product = ctx.tb_Product.Single(p => p.ProductID == ProductID);
            return tb_Product;
        }

        /// <summary>
        /// Get Product Details by AmazonRefID
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <returns>Returns a product details for particular Amazon product</returns>
        public tb_ProductAmazon GetAllProductDetailsbyAmazonRefID(Int32 ProductID)
        {
            tb_ProductAmazon tb_ProductAmazon = new tb_ProductAmazon();
            tb_ProductAmazon = ctx.tb_ProductAmazon.SingleOrDefault(p => p.AmazonRefID == ProductID);
            return tb_ProductAmazon;
        }

        /// <summary>
        /// Update product image
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns affected rows count</returns>
        public int UpdateProductImage(tb_Product product)
        {
            int RowsAffected = 0;
            try
            {
                tb_Product tb_Product = new tb_Product();
                tb_Product = ctx.tb_Product.Single(p => p.ProductID == product.ProductID);
                tb_Product.ImageName = product.ImageName;
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
        /// Method to display product by different option like IsFeatured,IsBestSeller,IsNewArrival
        /// </summary>
        /// <param name="DisplayOption">string DisplayOption</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="ProductCount">Int32 ProductCount</param>
        /// <returns>Returns the Product List as a Dataset</returns>
        public DataSet DisplyProductByOption(string DisplayOption, Int32 StoreID, Int32 ProductCount)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByFeatured";
            cmd.Parameters.AddWithValue("@DisplayOption", DisplayOption);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Display", ProductCount);
            cmd.Parameters.AddWithValue("@opt", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to Get All New Arrival Product
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <returns>Returns the List of All New Arrival Products</returns>
        public DataSet GetAllNewArrivalProduct(string Option)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "usp_Product_GetProductByFeatured";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DisplayOption", Option);
                cmd.Parameters.AddWithValue("@opt", 2);
            }
            catch { }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to Get All New Arrival Product for Admin
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the List of All New Arrival Products for Admin</returns>
        public DataSet GetAllNewArrivalProductAdmin(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "usp_Product_GetProductByFeatured";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
                cmd.Parameters.AddWithValue("@opt", 3);
            }
            catch { }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method for Get Product Details By ProductID while Edit Product
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns Product Details in Dataset by ProductID</returns>
        public DataSet GetProductByProductID(Int32 ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByProductID";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method for Get Product By AmazonRefID while Edit Product
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns Product Details in Dataset by AmazonRefID</returns>
        public DataSet GetProductByAmazonRefID(Int32 ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByAmazonRefID";
            cmd.Parameters.AddWithValue("@AmazonRefID", ProductID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get the Product List By StoreID
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the LIst of Product By StoreID</returns>
        public DataSet GetProductByStoreID(int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Data to Fill Grid
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int endIndex</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="ProductTypeId">int ProductTypeID</param>
        /// <param name="CategoryId">int CategoryID</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="status">string Status</param>
        /// <returns>Returns the filtered product List by category</returns>
        public DataSet GetProductListByCategory(int startIndex, int endIndex, string sortBy, int StoreID, int ProductTypeID, int ProductTypeDeliveryId, int CategoryID, string SearchBy, string SearchValue, string Status)
        {
            int startValue;
            int EndValue;
            startValue = (Convert.ToInt32(startIndex)) + 1;
            EndValue = (Convert.ToInt32(startIndex)) + endIndex;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductList";
            cmd.Parameters.AddWithValue("@StartIndex", startValue);
            cmd.Parameters.AddWithValue("@EndIndex", EndValue);
            cmd.Parameters.AddWithValue("@SortBy", string.IsNullOrEmpty(sortBy) ? "tb_product.ProductID desc" : sortBy);
            cmd.Parameters.AddWithValue("@ProductTypeID", ProductTypeID);
            cmd.Parameters.AddWithValue("@ProductTypeDeliveryId", ProductTypeDeliveryId);
            cmd.Parameters.AddWithValue("@CategoryId", CategoryID);
            cmd.Parameters.AddWithValue("@StoreId", StoreID == -1 ? 0 : StoreID);
            cmd.Parameters.AddWithValue("@SearchBy", SearchBy);
            cmd.Parameters.AddWithValue("@SearchValue", string.IsNullOrEmpty(SearchValue) ? string.Empty : SearchValue);
            cmd.Parameters.AddWithValue("@Status", string.IsNullOrEmpty(Status) ? string.Empty : Status);


            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Secondary Colors Names and Image Path
        /// </summary>
        /// <param name="strColors">String strColor</param>
        /// <returns>RErutns the Dataset of Color Name Image Names</returns>
        public DataSet GetSecondaryColorsImagePath(String strColors)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetSecondaryColorsImagePath";
            cmd.Parameters.AddWithValue("@Colors", strColors);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Get product Details
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Product Details Data in DataSet</returns>
        public DataSet GetProductDetailByID(Int32 ProductID, int StoreID)
        {

            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductDetailByID";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);

            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to Get Product Detail By Category ID and Store ID
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Product Details</returns>
        public DataSet GetProductDetailByCategoryID(Int32 CategoryID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByCategoryId";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Opt", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details by Order Price</returns>
        public DataSet GetProductDetailsByOrderPrice(Int32 CategoryID, Int32 StoreID, string Option,string color)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_OrderByPrice";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Option", Option);
            cmd.Parameters.AddWithValue("@color", color);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details by Order Price</returns>
        public DataSet GetFreeSwatchProductDetailsByOrderPrice(string CategoryID, Int32 StoreID, string Option)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_FreeSwatchProduct_OrderByPrice";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Option", Option);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details For Sales Outlet</returns>
        public DataSet GetProductDetailsSalesOutlet(Int32 CategoryID, Int32 StoreID, string Option)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_Salesoutlet";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Option", Option);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details by Order Price</returns>
        public DataSet GetSearchProductDetailsByOrderPrice(Int32 StoreID, string Option, string StrWhrClus)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_SearchList";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@StrWhrClus", StrWhrClus);
            cmd.Parameters.AddWithValue("@Option", Option);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        ///<param name="PriceRangeId">Int32 PriceRangeId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details by Shop By Price</returns>
        public DataSet GetProductDetailsByShopByPrice(Int32 PriceRangeId, Int32 StoreID, string Option)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_ShopbyPrice";
            cmd.Parameters.AddWithValue("@PriceRangeId", PriceRangeId);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Option", Option);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Update Product By Display Order Price Inventory
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="Qty">string Qty</param>
        /// <param name="SalePrice">string SalePrice</param>
        public void UpdateProductByDisplayOrderPriceInventory(string Option, Int32 ProductId, string Qty, string SalePrice)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByCategoryId";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@Option", Option);
            if (Option != "price")
                cmd.Parameters.AddWithValue("@DisplayOrder", Qty);
            else
                cmd.Parameters.AddWithValue("@Price", Qty);
            cmd.Parameters.AddWithValue("@SPrice", SalePrice);
            cmd.Parameters.AddWithValue("@Opt", 2);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Get Product Variant By product ID
        /// </summary>
        /// <param name="ProductId">Int32 Product ID</param>
        /// <returns>Returns the list of product variant by prouctID</returns>
        public DataSet GetProductVariantByproductID(Int32 ProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductVariantByproductID";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product Variant By productID and Engraving
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Returns the product variant list by productID></returns>
        public DataSet GetProductVariantByproductIDandEngraving(Int32 ProductId, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductVariantByproductIDandEngraving";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }
        public DataSet GetProductVariantByproductIDyahoo(Int32 ProductId, Int32 Mode, Int32 variantValueid)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductVariantByproductIDyahoo";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@variantValueid", variantValueid);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product Rating
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        /// <returns>Returns the List of Product Rating</returns>
        public DataSet GetProductRating(Int32 ProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductRating";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Recently Product Without Current Product
        /// </summary>
        /// <param name="ProductId">int product Id</param>
        /// <param name="OrgProductID">int OrgProductID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Recently Product Details By ProductID</returns>
        public DataSet GetRecentlyProduct(string ProductId, int StoreID, int OrgProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetRecentlyProduct";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@OrgProductID", OrgProductID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get You may also Like Product
        /// </summary>
        /// <param name="ProductId">int product Id</param>
        /// <param name="TopRpoduct">Int32 TopRpoduct</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the You may also like product list</returns>
        public DataSet GetYoumayalsoLikeProduct(Int32 ProductId, Int32 TopRpoduct, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetYoumayalsoLikeProduct";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@topProduct", TopRpoduct);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get the Related Product List
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Related Product List</returns>
        public DataSet GetRelatedProduct(Int32 ProductId, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetRelatedProduct";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Related Product Add to Cart
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns the Related product for Add to Cart</returns>
        public DataSet GetRelatedProductAddTocart(Int32 ProductId, Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetRelatedProductAddTocart";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@StoreID", StoreId);


            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Add Or Update Rating
        /// </summary>
        /// <param name="CustomerID">Int32 Customer ID</param>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="Comments">string Comments</param>
        /// <param name="EmailID">string EmailID</param>
        /// <param name="Rating">string Rating</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Name">string Name</param>
        /// <returns>Returns the String Value</returns>
        public string AddOrUpdateRating(Int32 CustomerID, Int32 ProductID, string Comments, string EmailID, Int32 Rating, Int32 StoreID, string Name)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Rating_AddorUpdateRating";
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Comments", Comments);
            cmd.Parameters.AddWithValue("@EmailID", EmailID);
            cmd.Parameters.AddWithValue("@Rating", Rating);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Name", Name);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get product Image name
        /// </summary>
        /// <param name="ProductID">int Product ID</param>
        /// <returns>Returns the image name for particular product</returns>
        public DataSet GetproductImagename(Int32 ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetproductImagename";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Product For View
        /// </summary>
        /// <param name="ProductId">int32 ProductID</param>
        public void UpdateProductForView(Int32 ProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_UpdateProductForView";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Get Products By StoreID
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the List of Products by StoreID</returns>
        public DataSet GetProductByStoreId(int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID.ToString());
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Method to Get Index Page Config value
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the List of Index Page Config value</returns>
        public DataSet GetIndexPageConfig(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_IndexPageConfig";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@opt", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product list By Search
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Name">string ProductName</param>
        /// <returns>Returns DataSet</returns>
        public DataSet GetProductByStoreID(Int32 StoreID, string ProductName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_IndexPageConfig";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Name", ProductName);
            cmd.Parameters.AddWithValue("@opt", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Product Feature 
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="status">bool status</param>
        /// <param name="StoreID">Int32 StoreID</param>
        public void UpdateProductFeature(Int32 ProductID, bool status, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_IndexPageConfig";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@opt", 3);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Update Product Best Seller
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="status">bool status</param>
        /// <param name="StoreID">Int32 StoreID</param>
        public void UpdateProductBestSeller(Int32 ProductID, bool status, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_IndexPageConfig";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@opt", 4);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Update New Arrival Product
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="status">bool status</param>
        /// <param name="StoreID">Int32 StoreID</param>
        public void UpdateProductNewArrival(Int32 ProductID, bool status, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_IndexPageConfig";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@status", status);
            cmd.Parameters.AddWithValue("@opt", 5);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Get Search Data
        /// </summary>
        /// <param name="SearchText">String SearchText</param>
        /// <param name="PriceFrom">String PriceFrom</param>
        /// <param name="PriceTo">String PriceTo</param>
        /// <param name="CategoryId">String CategoryId</param>
        /// <param name="IsSearchInDescription">bool IsSearchInDescription</param>
        /// <param name="StoreId">Int32 StoreID</param>
        /// <returns>Returns Searched Data</returns>
        public DataSet GetSearchData(String SearchText, String PriceFrom, String PriceTo, String CategoryId, bool IsSearchInDescription, Int32 StoreId)
        {
            decimal decPriceFrom = 0;
            decimal decPriceTo = 0;
            decimal.TryParse(PriceFrom, out decPriceFrom);
            decimal.TryParse(PriceTo, out decPriceTo);

            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_SearchTerm";

            string[] Words = new string[5];
            Words = SearchText.Split(" ".ToCharArray(), 5, StringSplitOptions.RemoveEmptyEntries);
            try
            {
                cmd.Parameters.AddWithValue("@Word1", Words[0]);
                cmd.Parameters.AddWithValue("@Word2", Words[1]);
                cmd.Parameters.AddWithValue("@Word3", Words[2]);
                cmd.Parameters.AddWithValue("@Word4", Words[3]);
                cmd.Parameters.AddWithValue("@Word5", Words[4]);
            }
            catch { }

            if (!string.IsNullOrEmpty(CategoryId))
                cmd.Parameters.AddWithValue("@Categories", CategoryId.ToString());

            if (decPriceFrom >= 0 && decPriceTo > 0)
            {
                cmd.Parameters.AddWithValue("@PriceFrom", SqlDbType.Money).Value = decPriceFrom;
                cmd.Parameters.AddWithValue("@PriceTo", SqlDbType.Money).Value = decPriceTo;
            }
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@ExactMatch", SqlDbType.Bit).Value = false;
            cmd.Parameters.AddWithValue("@SearchInDescription", SqlDbType.Bit).Value = IsSearchInDescription;
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Product detail based on selected criteria
        /// </summary>
        /// <param name="Field">string Field</param>
        /// <param name="Value">decimal Value</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Flag">string Flag</param>
        /// <param name="Operation">string Operation</param>
        /// <returns>Count of number of rows Affected</returns>
        public int UpdateProduct(string Field, decimal Value, int ProductID, int StoreID, string Flag, string Operation)
        {
            int RowsAffected = 0;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_UpdateProduct";
            cmd.Parameters.AddWithValue("@FieldName", Field);
            cmd.Parameters.AddWithValue("@FieldValue", Value);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Flag", Flag);
            cmd.Parameters.AddWithValue("@Operation", Operation);
            SqlParameter spm = new SqlParameter("@RowCount", DbType.Int32);
            spm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(spm);
            objSql.ExecuteNonQuery(cmd);
            RowsAffected = (Int32)cmd.Parameters["@RowCount"].Value;
            return RowsAffected;
        }
        #endregion

        /// <summary>
        /// Get Product For Yahoo Store
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="StDateCreatedON">SqlDateTime StDateCreatedON</param>
        /// <param name="EndDateCreatedON">SqlDateTime EndDateCreatedON</param>
        /// <param name="StDateUpdatedON">SqlDateTime StDateUpdatedON</param>
        /// <param name="EndDateUpdatedON">SqlDateTime EndDateUpdatedON</param>
        /// <param name="Criteria">String Criteria</param>
        /// <returns>Returns the List of Yahoo Products</returns>
        public DataSet GetProductForYahooStore(int StoreID, System.Data.SqlTypes.SqlDateTime StDateCreatedON, System.Data.SqlTypes.SqlDateTime EndDateCreatedON, System.Data.SqlTypes.SqlDateTime StDateUpdatedON, System.Data.SqlTypes.SqlDateTime EndDateUpdatedON, String Criteria)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Export_ExportProductDataForYahoo";
            cmd.Parameters.AddWithValue("@storeid", StoreID);
            cmd.Parameters.AddWithValue("@stdateCreatedOn", StDateCreatedON);
            cmd.Parameters.AddWithValue("@enddateCreatedOn", EndDateCreatedON);
            cmd.Parameters.AddWithValue("@stdateUpdatedon", StDateUpdatedON);
            cmd.Parameters.AddWithValue("@enddateUpdatedOn", EndDateUpdatedON);
            cmd.Parameters.AddWithValue("@Criteria", Criteria);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// Method to Get Product Variant Count
        /// </summary>
        /// <param name="StoreID">Int32 ProductID</param>
        /// <returns>Returns the count of total Product Variant</returns>
        public DataSet GetProductVariantTotalCount(Int32 ProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductVariant";
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Save the Product Variant
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantName">string VariantName</param>
        /// <param name="VariantValue">string VariantValue</param>
        /// <param name="VariantPrice">decimal VariantPrice</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <returns>Returns the  Identity Value</returns>
        public int SaveProductVariant(Int32 ProductId, string VariantName, string VariantValue, decimal VariantPrice, Int32 DisplayOrder, String SKU, String Header, String UPC, Int32 IsParent)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductVariant";
            cmd.Parameters.AddWithValue("@VariantName", VariantName);
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@VariantValue", VariantValue);
            cmd.Parameters.AddWithValue("@VariantPrice", VariantPrice);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@SKU", SKU);
            cmd.Parameters.AddWithValue("@Header", Header);
            cmd.Parameters.AddWithValue("@UPC", UPC);
            cmd.Parameters.AddWithValue("@Mode", 2);
            cmd.Parameters.AddWithValue("@IsParent", IsParent);

            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Save Clone Product
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <returns>Returns the Identity Value</returns>
        public int SaveCloneProduct(Int32 StoreId, Int32 ProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductClone";
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@Mode", 1);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Save Clone Product Variant
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="NewProductId">Int32 NewProductId</param>
        /// <returns>Returns the Identity Value</returns>
        public int SaveCloneProductVariant(Int32 StoreId, Int32 ProductId, Int32 NewProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductClone";
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@Mode", 1);
            cmd.Parameters.AddWithValue("@NewProductId", NewProductId);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Product detail based on selected criteria
        /// </summary>
        /// <param name="Field">string Field</param>
        /// <param name="Value">decimal Value</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Flag">string Flag</param>
        /// <param name="Operation">string Operation</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public int UpdateMultiplePriceForProduct(string Field, decimal Value, int ProductID, int StoreID, string Flag, string Operation)
        {
            int RowsAffected = 0;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_UpdateProduct";
            cmd.Parameters.AddWithValue("@FieldName", Field);
            cmd.Parameters.AddWithValue("@FieldValue", Value);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Flag", Flag);
            cmd.Parameters.AddWithValue("@Operation", Operation);
            SqlParameter spm = new SqlParameter("@RowCount", DbType.Int32);
            spm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(spm);
            objSql.ExecuteNonQuery(cmd);
            RowsAffected = (Int32)cmd.Parameters["@RowCount"].Value;
            return RowsAffected;
        }


        /// <summary>
        /// Update Multiple Product
        /// </summary>
        /// <param name="UpdateQuery">string UpdateQuery</param>
        /// <param name="ProductIDs">int ProductID</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public int UpdateMultiplePoduct(string UpdateQuery, int ProductID)
        {
            int RowsAffected = 0;
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_UpdateMultipleProduct";
            cmd.Parameters.AddWithValue("@UpdateQuery", UpdateQuery);
            cmd.Parameters.AddWithValue("@ProductIDs", ProductID);
            SqlParameter spm = new SqlParameter("@RowCount", DbType.Int32);
            spm.Direction = ParameterDirection.Output;
            cmd.Parameters.Add(spm);
            objSql.ExecuteNonQuery(cmd);
            RowsAffected = (Int32)cmd.Parameters["@RowCount"].Value;
            return RowsAffected;
        }


        /// <summary>
        /// Get Product Variant Value
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantId">Int32 ProductId</param>
        /// <returns>Returns the Details o Product Variant value</returns>
        public DataSet GetProductVariantValue(Int32 ProductId, Int32 VariantId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductVariant";
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@VariantId", VariantId);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Delete Variant Value for particular product and variant
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantId">Int32 VariantId</param>
        /// <param name="VariantValueId">Int32 VariantValueId</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteVariantValue(Int32 ProductId, Int32 VariantId, Int32 VariantValueId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductVariant";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@VariantId", VariantId);
            cmd.Parameters.AddWithValue("@VariantValueId", VariantValueId);
            cmd.Parameters.AddWithValue("@Mode", 4);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Insert Product Variant Value
        /// </summary>
        /// <param name="VariantId">Int32 VariantId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantName">string VariantName</param>
        /// <param name="VariantValue">string VariantValue</param>
        /// <param name="VariantPrice">decimal VariantPrice</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <returns>Returns Identity Value</returns>
        public int InsertProductVariantValue(Int32 VariantId, Int32 ProductId, string VariantName, string VariantValue, decimal VariantPrice, Int32 DisplayOrder, String SKU, String Header, String UPC)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductVariant";
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@VariantValue", VariantValue);
            cmd.Parameters.AddWithValue("@VariantPrice", VariantPrice);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@SKU", SKU);
            cmd.Parameters.AddWithValue("@Header", Header);
            cmd.Parameters.AddWithValue("@UPC", UPC);
            cmd.Parameters.AddWithValue("@VariantId", VariantId);
            cmd.Parameters.AddWithValue("@Mode", 5);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Product Variant Value
        /// </summary>
        /// <param name="VariantId">Int32 VariantId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantValueId">Int32 VariantValueId</param>
        /// <param name="VariantPrice">decimal VariantPrice</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public int UpdateProductVariantValue(Int32 VariantId, Int32 ProductId, Int32 VariantValueId, decimal VariantPrice, Int32 DisplayOrder, String SKU, String Header, String UPC)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductVariant";
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@VariantPrice", VariantPrice);
            cmd.Parameters.AddWithValue("@VariantValueId", VariantValueId);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@VariantId", VariantId);
            cmd.Parameters.AddWithValue("@SKU", SKU);
            cmd.Parameters.AddWithValue("@Header", Header);
            cmd.Parameters.AddWithValue("@UPC", UPC);
            cmd.Parameters.AddWithValue("@Mode", 6);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }
        /// <summary>
        /// Delete Variant Option
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantId">Int32 VariantId</param>
        /// <param name="VariantValueId"></param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteVariantOption(Int32 ProductId, Int32 VariantId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductVariant";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@VariantId", VariantId);
            cmd.Parameters.AddWithValue("@Mode", 7);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Get Search Product Value
        /// </summary>
        /// <param name="StoreIdId">Int32 StoreIdId</param>
        /// <param name="Whrclus">String Whrclus</param>
        /// <returns>Returns Search product Value</returns>
        public DataSet GetSearchProductValue(Int32 StoreIdId, String Whrclus)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductVariant";
            cmd.Parameters.AddWithValue("@StoreId", StoreIdId);
            cmd.Parameters.AddWithValue("@WhrClus", Whrclus);
            cmd.Parameters.AddWithValue("@Mode", 8);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Gets the rating of product
        /// </summary>
        /// <param name="Id">ProductID</param>
        /// <returns>Returns Rating Table Object</returns>
        public tb_Rating GetRatingDetail(int Id)
        {
            tb_Rating rating = null;
            {
                rating = ctx.tb_Rating.First(e => e.RatingID == Id);
            }
            return rating;
        }

        /// <summary>
        /// Updates Product Rating Review
        /// </summary>
        /// <param name="tb_rating">tb_Rating tb_rating</param>
        public void UpdateReview(tb_Rating tb_rating)
        {
            ctx.SaveChanges();
        }

        /// <summary>
        /// Get Product Detail By ProductID
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns a Product Details By ProductID</returns>
        public tb_Product GetProductDetailByProductID(int ProductID)
        {
            tb_Product product = null;
            product = ctx.tb_Product.First(e => e.ProductID == ProductID);
            return product;
        }


        /// <summary>
        /// Method to Generate RSS 
        /// </summary>
        /// <param name="AppPath">String AppPath</param>
        /// <param name="StoreID">String StoreID</param>
        /// <returns>Returns the Product RSS List</returns>
        public DataSet GenerateProductRSS(String AppPath, String StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GenerateRSS";
            cmd.Parameters.AddWithValue("@AppPath", AppPath);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get eBay Category
        /// </summary>
        /// <returns>Returns the eBay Category List</returns>
        public DataSet GeteBayCategory()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GeteBayCategory";
            return objSql.GetDs(cmd);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetProductFabricDetails(Int32 FabricTypeID, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductFabricDetails";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetFabricVendorPortalDetails(Int32 FabricTypeID, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetFabricVendorPortalDetails";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        ///// <summary>
        /////Get Product Fabric Details
        ///// </summary>
        ///// <returns>Returns Product Fabric Details</returns>
        //public DataSet GetFabricVendorPortalDetails(Int32 FabricTypeID, Int32 Mode)
        //{
        //    objSql = new SQLAccess();
        //    cmd = new SqlCommand();
        //    cmd.CommandType = CommandType.StoredProcedure;
        //    cmd.CommandText = "usp_Product_GetFabricVendorPortalDetails";
        //    cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
        //    cmd.Parameters.AddWithValue("@Mode", Mode);
        //    return objSql.GetDs(cmd);
        //}


        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetFabricVendorPortalDetails(Int32 FabricTypeID, Int32 Mode, string Code)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetFabricVendorPortalDetails";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@Code", Code);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetFabricVendorPortalDetailsForPopup(Int32 FabricTypeID, Int32 FabricCodeId, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetFabricVendorPortalDetails";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@FabricCodeId", FabricCodeId);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetFabricVendorPortalDetails(Int32 FabricTypeID, Int32 FabricCodeId, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetFabricVendorPortalDetails";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@FabricCodeId", FabricCodeId);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get eBay Store Category
        /// </summary>
        /// <returns>Returns the eBay Store category List</returns>
        public DataSet GeteBayStoreCategory()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GeteBayStoreCategory";
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Quantity Discount Table By ProductID
        /// </summary>
        /// <param name="ProductID">string ProductID</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Quantity Discount list By ProductID</returns>
        public DataSet GetQuantityDiscountTableByProductID(string ProductID, string StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT CONVERT(VARCHAR(10),LowQuantity)+' - '+CONVERT(VARCHAR(10),HighQuantity) as QuantityRange,Convert(varchar,Convert(float, DiscountPercent))+' %' as DiscountPercent  From tb_QuantityDiscountTable Where QuantityDiscountID IN (SELECT QuantityDiscountID FROM dbo.tb_Product WHERE   QuantityDiscountID <> 0 and  storeid = " + StoreID + " and  ProductID=" + ProductID + ")";
            return objSql.GetDs(cmd);

        }

        /// <summary>
        /// Get Quantity Discount Table By Item
        /// </summary>
        /// <param name="ProductID">string ProductID</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Quantity Discount list By Item</returns>
        public DataSet GetQuantityDiscountTableByItem(string ProductID, string StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT isnull(price,0) as price,isnull(saleprice,0) as saleprice FROM tb_product WHERE Productid=" + ProductID + " and storeid = " + StoreID + "";

            return objSql.GetDs(cmd);

        }

        /// <summary>
        /// Insert Gift card product
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <returns>Returns Identity Value</returns>
        public int InsertGiftcardproduct(Int32 StoreId, Int32 ProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GiftcardProduct";
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@Mode", 1);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get Gift Card Product List
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetGiftCardProductList(int StoreID, bool Issearch, string SearchBy, string SearchValue)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GiftcardProduct";
            cmd.Parameters.AddWithValue("@StoreId", StoreID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Search Product Value
        /// </summary>
        /// <param name="StoreIdId">Int32 StoreIdId</param>
        /// <param name="Whrclus">String Whrclus</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns the Searched Product List</returns>
        public DataSet GetSearchProductValue(Int32 StoreIdId, String Whrclus, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WarehouseProduct";
            cmd.Parameters.AddWithValue("@StoreId", StoreIdId);
            cmd.Parameters.AddWithValue("@WhrClus", Whrclus);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Delete Gift Card Product
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteGiftCardProduct(Int32 ProductId, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GiftcardProduct";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Get WareHouse Details By ID
        /// </summary>
        /// <param name="WarehouseID">int WarehouseID</param>
        /// <returns>Returns the WareHouse Details By ID</returns>
        public tb_WareHouse GetWarehouseByID(int WarehouseID)
        {
            tb_WareHouse warehouse = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                warehouse = ctx.tb_WareHouse.First(e => e.WareHouseID == WarehouseID);
            }
            return warehouse;
        }

        /// <summary>
        /// Update WareHouse
        /// </summary>
        /// <param name="warehouse">tb_WareHouse warehouse</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public int UpdateWarehouse(tb_WareHouse warehouse)
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
        /// Create WareHouse
        /// </summary>
        /// <param name="warehouse">tb_WareHouse warehouse</param>
        /// <returns>Returns WareHouse Table Object</returns>
        public tb_WareHouse CreateWarehouse(tb_WareHouse warehouse)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_WareHouse(warehouse);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return warehouse;
        }

        /// <summary>
        /// Get Warehouse List
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns the WareHouse List as a Dataset</returns>
        public DataSet GetWarehouseList(int Mode, int ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WareHouse_GetWarehouseList";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Warehouse
        /// </summary>
        /// <param name="WarehouseID">int WarehouseID</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Inventory">int Inventory</param>
        /// <param name="mode">int mode</param>
        /// <returns>Returns the Identity value for Insert or 1 for if Updated Record</returns>
        public int InsertUpdateWarehouse(int WarehouseID, int ProductID, int Inventory, int mode, Boolean PreferredLocation)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_InsertUpdateWarehouse";
            cmd.Parameters.AddWithValue("@WarehouseID", WarehouseID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Inventory", Inventory);
            cmd.Parameters.AddWithValue("@PreferredLocation", PreferredLocation);
            cmd.Parameters.AddWithValue("@Mode", mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Product inventory
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Inventory">int Inventory</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns 1 if Updated/returns>
        public int UpdateProductinventory(int ProductID, int Inventory, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_UpdateProductInventory";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@Inventory", Inventory);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get All Best Seller Product
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Best Seller Product List</returns>
        public DataSet GetAllBestSellerProduct(string Option, string StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            try
            {
                cmd.CommandText = "usp_Product_GetProductByFeatured";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@DisplayOption", Option);
                cmd.Parameters.AddWithValue("@StoreID", StoreID);
                cmd.Parameters.AddWithValue("@opt", 5);
            }
            catch { }
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Optional Products Details By IDs
        /// </summary>
        /// <param name="SKUs">String SKUs</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the Optional Product Details by ID</returns>
        public DataSet GetOptinalProductDetailByID(String SKUs, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetOptinalProductDetailByID";
            cmd.Parameters.AddWithValue("@SKUs", SKUs);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Gift Card By StoreID
        /// </summary>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Gift Card By StoreID</returns>
        public DataSet GetGiftCardByStoreID(string StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT tb_Product.Name, tb_Product.ProductID, tb_Product.SEKeywords, tb_Product.SEDescription,tb_Product.SEName, tb_Product.ImageName as ImageName, " +
                             " tb_Product.SKU,tb_Product.MainCategory,tb_Product.TagName,ISNULL(tb_Product.Price,0) as RegularPrice,ISNULL(tb_Product.Price,0) as Price,(CASE WHEN (SalePrice IS NOT NULL AND SalePrice != 0) THEN SalePrice ELSE Price END) AS SalePrice ,ToolTip " +
                             " FROM tb_Product INNER JOIN  tb_GiftCardProduct ON tb_Product.ProductID = tb_GiftCardProduct.ProductID  " +
                             " WHERE (tb_Product.Price <> 0) AND  ((CASE WHEN (SalePrice IS NOT NULL AND SalePrice != 0)  " +
                             " THEN SalePrice ELSE Price END) <> 0 OR  (CASE WHEN (SalePrice IS NOT NULL AND SalePrice != 0) THEN SalePrice ELSE Price END) <> NULL)  " +
                             " AND (tb_Product.Active = 1) AND (ISNULL(tb_Product.Deleted,0) = 0) And tb_GiftCardProduct.StoreID=" + StoreID + " order by SalePrice";
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Product By ID For Gift
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Product By ID for Gift Certificate</returns>
        public DataSet GetProductByIDForGift(Int32 ProductID, string StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "SELECT  tb_Product.SKU,tb_Product.SEName, tb_Product.Avail,tb_Product.ImageName, tb_Product.Price, (Case When (SalePrice Is Not Null And SalePrice!=0) Then SalePrice Else Price End) As SalePrice,tb_Product.ToolTip, ISNull(Convert(nvarchar(max),tb_Product.Description),'') as Description , " +
                               " tb_Product.Name,tb_Product.TagName, tb_Product.Inventory,tb_Product.SEDescription,tb_Product.SEKeywords,(Case When (tb_Product.SETitle Is Not Null And tb_Product.SETitle!='') Then tb_Product.SETitle Else tb_Product.Name End) As SETitle,tb_Product.Discontinue,isnull(tb_Product.SurCharge,0) as 'SurCharge',tb_Product.MainCategory FROM tb_Product  " +
                               " WHERE(ISNULL(tb_Product.Deleted,0)=0) AND (tb_Product.Active = 1)  " +
                               " AND (tb_Product.ProductID = " + ProductID + ") And  tb_Product.StoreID=" + StoreID + "";
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Add Gift Card
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="OrderNumber">int OrderNumber</param>
        /// <param name="EmailFrom">string EmailFrom</param>
        /// <param name="EmailTo">string EmailTo</param>
        /// <param name="Emailname">string Emailname</param>
        /// <param name="Emailmessage">string Emailmessage</param>
        /// <param name="StoreId">int StoreID</param>
        /// <returns>Returns Identity Value</returns>
        public String AddGiftCard(Int32 CustomerID, Int32 ProductID, Int32 OrderNumber, String EmailFrom, String EmailTo, String Emailname, String Emailmessage, Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GiftCard";
            cmd.Parameters.AddWithValue("@ProductId", ProductID);
            cmd.Parameters.AddWithValue("@CustomerID", CustomerID);
            cmd.Parameters.AddWithValue("@OrderNumber", OrderNumber);
            cmd.Parameters.AddWithValue("@EmailFrom", EmailFrom);
            cmd.Parameters.AddWithValue("@EmailTo", EmailTo);
            cmd.Parameters.AddWithValue("@Emailmessage", Emailmessage);
            cmd.Parameters.AddWithValue("@RecipientName", Emailname);
            cmd.Parameters.AddWithValue("@StoreId", StoreId);

            Random r = new Random();
            string strRandomFileName = Path.GetRandomFileName() + Path.GetRandomFileName();
            string strDest = null;
            for (int iIndex = 0; iIndex < r.Next(8, 12); iIndex++)
            {
                int iTemp = Convert.ToInt16(strRandomFileName[iIndex]);
                if (iTemp != 46)
                    strDest += strRandomFileName[iIndex].ToString();
            }
            String SerialNumber = strDest;

            cmd.Parameters.AddWithValue("@SerialNumber", SerialNumber);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            Int32 f = Convert.ToInt32(paramReturnval.Value);
            if (f > 0)
                return SerialNumber;
            else
                return "0";
        }

        /// <summary>
        /// get Latest Product Inquiry
        /// </summary>
        /// <param name="StoreId">int StoreId</param>
        /// <param name="Mode">int Mode</param>
        /// <param name="SearchFrom">string SearchFrom</param>
        /// <param name="SearchTo">string SearchTo</param>
        /// <param name="OrderByColumn">string OrderByColumn</param>
        /// <param name="OrderBy">string OrderBy</param>
        /// <returns>Returns the Latest Product inquiry List</returns>
        public DataSet getLatestProductInquiry(int StoreId, int Mode, string SearchFrom, string SearchTo, string OrderByColumn, string OrderBy)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_LatestProductInquiry";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@SearchFrom", SearchFrom);
            cmd.Parameters.AddWithValue("@SearchTo", SearchTo);
            cmd.Parameters.AddWithValue("@OrderByColumn", OrderByColumn);
            cmd.Parameters.AddWithValue("@OrderBy", OrderBy);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Delete Product Inquiry
        /// </summary>
        /// <param name="StoreId">int StoreId</param>
        /// <param name="IDs">string IDs</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int deleteProductInquiry(int StoreId, string IDs)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_LatestProductInquiry";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@IDs", IDs);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        /// <summary>
        /// Get product Details
        /// </summary>
        /// <param name="Customers">int product Id</param>
        /// <returns>Returns Product Data in DataSet</returns>
        public DataSet GetProductDetailByID(Int32 ProductID)
        {

            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductDetailByID";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Variant Warehouse List
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns the WareHouse List of Variant Value as a Dataset</returns>
        public DataSet GetVariantWarehouseList(int Mode, int ProductID, int VariantID, int VariantValueID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WareHouse_GetVariantWarehouseList";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@VariantID", VariantID);
            cmd.Parameters.AddWithValue("@VariantValueID", VariantValueID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Variant Warehouse
        /// </summary>
        /// <param name="WarehouseID">int WarehouseID</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="Inventory">int Inventory</param>
        /// <param name="mode">int mode</param>
        /// <returns>Returns the Identity value for Insert or 1 for if Updated Record</returns>
        public int InsertUpdateVariantWarehouse(int WarehouseID, int ProductID, int Inventory, int mode, int VariantID, int VariantValueID, int CreatedBy, int UpdatedBy, Boolean PreferredLocation)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_InsertUpdateVariantWarehouse";
            cmd.Parameters.AddWithValue("@WarehouseID", WarehouseID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@VariantID", VariantID);
            cmd.Parameters.AddWithValue("@VariantValueID", VariantValueID);
            cmd.Parameters.AddWithValue("@Inventory", Inventory);
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
            cmd.Parameters.AddWithValue("@PreferredLocation", PreferredLocation);
            cmd.Parameters.AddWithValue("@Mode", mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// GetProduct Fabric Type
        /// </summary>
        /// <returns>Returns the Fabric Types of Product as a Dataset</returns>
        public DataSet GetProductFabricType()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_ProductFabricDetails";
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Fabric Type
        /// </summary>
        /// <param name="FabricTypeID">Int32 FabricTypeID</param>
        /// <param name="FabricType">String FabricType</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 Insert_Update_Delete_FabricType(Int32 FabricTypeID, String FabricType, Int32 DisplayOrder, Boolean Active, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_InsertUpdateProductFabric";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@FabricTypeName", FabricType);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// GetProduct Fabric Code
        /// </summary>
        /// <returns>Returns the Fabric Code of Product as a Dataset</returns>
        public DataSet GetProductFabricCode(Int32 FabricTypeID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_ProductFabricDetails";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// GetProduct Fabric Width
        /// </summary>
        /// <returns>Returns the Fabric Width of Product as a Dataset</returns>
        public DataSet GetProductFabricWidth(Int32 FabricCodeID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_ProductFabricDetails";
            cmd.Parameters.AddWithValue("@FabricCodeID", FabricCodeID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Fabric Code
        /// </summary>
        /// <param name="FabricTypeID">Int32 FabricTypeID</param>
        /// <param name="FabricCodeID">Int32 FabricCodeID</param>
        /// <param name="Code">String Code</param>
        /// <param name="Name">String Name</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Return ID</returns>
        public Int32 Insert_Update_Delete_FabricCode(Int32 FabricTypeID, Int32 FabricCodeID, String Code, String Name, Int32 DisplayOrder, Boolean Active, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_InsertUpdateProductFabricCode";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@FabricCodeID", FabricCodeID);
            cmd.Parameters.AddWithValue("@Code", Code);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Insert Update Delete Fabric Code
        /// </summary>
        /// <param name="FabricTypeID">Int32 FabricTypeID</param>
        /// <param name="FabricCodeID">Int32 FabricCodeID</param>
        /// <param name="Code">String Code</param>
        /// <param name="Name">String Name</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Return ID</returns>
        public Int32 Insert_Update_Delete_FabricWidth(Int32 hdnFabricCodeID, Int32 FabricWidthID, String Width, Int32 DisplayOrder, Boolean Active, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_InsertUpdateProductFabricWidth";
            cmd.Parameters.AddWithValue("@FabricWidthID", FabricWidthID);
            cmd.Parameters.AddWithValue("@FabricCodeID", hdnFabricCodeID);
            cmd.Parameters.AddWithValue("@Width", Width);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get Product Rating
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        /// <returns>Returns the List of Product Rating</returns>
        public DataSet GetProductRatingCount(Int32 ProductId, string SearchSort)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_GetProductRatingCount";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@Option", SearchSort);
            return objSql.GetDs(cmd);
        }


        /// <summary>
        /// GetProduct Fabric Type
        /// </summary>
        /// <returns>Returns the Fabric Types of Product as a Dataset</returns>
        public DataSet GetProductFabricType(Int32 FabricTypeID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_ProductFabricDetails";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@Mode", 4);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// GetProduct Fabric Code
        /// </summary>
        /// <returns>Returns the Fabric Code of Product as a Dataset</returns>
        public DataSet GetProductFabricCode(Int32 FabricTypeID, string Code)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_ProductFabricDetails";
            cmd.Parameters.AddWithValue("@FabricTypeID", FabricTypeID);
            cmd.Parameters.AddWithValue("@Code", Code);
            cmd.Parameters.AddWithValue("@Mode", 5);
            return objSql.GetDs(cmd);
        }

        #region Product Feature

        public tb_ProductFeature GetProductFeatureByID(int FeatureId)
        {
            tb_ProductFeature feature = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                feature = ctx.tb_ProductFeature.First(e => e.FeatureId == FeatureId);
            }
            return feature;
        }


        public tb_ProductFeature CreateProductFeature(tb_ProductFeature Feature)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_ProductFeature(Feature);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Feature;
        }
        public int UpdateProductFeature(tb_ProductFeature Feature)
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

        public Int32 DeleteFeatureList(Int32 FeatureId)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int Deleted = 0;
            try
            {
                tb_ProductFeature tb_ProductFeature = new tb_ProductFeature();
                tb_ProductFeature = ctx.tb_ProductFeature.First(c => c.FeatureId == FeatureId);
                ctx.DeleteObject(tb_ProductFeature);
                //  tb_ProductFeature.Deleted = Convert.ToBoolean(Val);
                ctx.SaveChanges();
                Deleted = tb_ProductFeature.FeatureId;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                Deleted = 0;
                throw ex;
            }
            return Deleted;
        }
        public int CheckDuplicate(tb_ProductFeature Feature)
        {
            Int32 isExists = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(Feature.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            isExists = (from a in ctx.tb_ProductFeature
                        where a.Name == Feature.Name && a.FeatureId != Feature.FeatureId && a.tb_Store.StoreID == ID
                        select new { a.FeatureId }
                           ).Count();
            return isExists;
        }

        public DataSet GetproductFeature(Int32 Featureid, Int32 Storeid, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetproductFeature";
            cmd.Parameters.AddWithValue("@Featureid", Featureid);
            cmd.Parameters.AddWithValue("@Storeid", Storeid);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        #endregion
        public DataSet GetItemchannelReport(string SearchValue)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_ItemchannelReport";
            cmd.Parameters.AddWithValue("@Sku", SearchValue);
            return objSql.GetDs(cmd);
        }

        #region Product Color Setups ADMIN
        /// <summary>
        /// Get Product Color List
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductColorList(int ColorID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductColor";
            cmd.Parameters.AddWithValue("@ColorID", ColorID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Fabric Type
        /// </summary>
        /// <param name="FabricTypeID">Int32 FabricTypeID</param>
        /// <param name="FabricType">String FabricType</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductColor(Int32 ColorID, String ColorName, string ImageName, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductColor";
            cmd.Parameters.AddWithValue("@ColorID", ColorID);
            cmd.Parameters.AddWithValue("@ColorName", ColorName);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Gift Card Product
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductColor(Int32 ColorID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductColor";
            cmd.Parameters.AddWithValue("@ColorID", ColorID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }
        #endregion

        #region Product Size Setups ADMIN

        /// <summary>
        /// Get Product Size List
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductSizeList(int SizeID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductSize";
            cmd.Parameters.AddWithValue("@SizeID", SizeID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete ProductSize
        /// </summary>
        /// <param name="FabricTypeID">Int32 SizeID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductSize(Int32 SizeID, String SizeName, Int32 Width, Int32 Length, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder, string ImageName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductSize";
            cmd.Parameters.AddWithValue("@SizeID", SizeID);
            cmd.Parameters.AddWithValue("@SizeName", SizeName);
            cmd.Parameters.AddWithValue("@Width", Width);
            cmd.Parameters.AddWithValue("@Length", Length);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Gift Card Product
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductSize(Int32 SizeID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductSize";
            cmd.Parameters.AddWithValue("@SizeID", SizeID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }
        #endregion

        #region Product Header Setups ADMIN

        /// <summary>
        /// Get Product Header List
        /// </summary>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductHeaderList(int HeaderID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductHeader";
            cmd.Parameters.AddWithValue("@HeaderID", HeaderID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Header
        /// </summary>
        /// <param name="HeaderID">Int32 HeaderID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductHeader(Int32 HeaderID, String HeaderName, string Description, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder, string ImageName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductHeader";
            cmd.Parameters.AddWithValue("@HeaderID", HeaderID);
            cmd.Parameters.AddWithValue("@HeaderName", HeaderName);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Product Header
        /// </summary>
        /// <param name="HeaderID">Int32 HeaderID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductHeader(Int32 HeaderID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductHeader";
            cmd.Parameters.AddWithValue("@HeaderID", HeaderID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        #endregion

        #region Product Room Setups ADMIN

        /// <summary>
        /// Get Product Header List
        /// </summary>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductRoomList(int RoomID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductRoom";
            cmd.Parameters.AddWithValue("@RoomID", RoomID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Header
        /// </summary>
        /// <param name="HeaderID">Int32 HeaderID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductRoom(Int32 RoomID, String RoomName, string Description, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder, string ImageName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductRoom";
            cmd.Parameters.AddWithValue("@RoomID", RoomID);
            cmd.Parameters.AddWithValue("@RoomName", RoomName);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Product Header
        /// </summary>
        /// <param name="HeaderID">Int32 HeaderID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductRoom(Int32 RoomID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductRoom";
            cmd.Parameters.AddWithValue("@RoomID", RoomID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        #endregion

        #region Product Feature Group Type Setups ADMIN

        /// <summary>
        /// Get Product Header List
        /// </summary>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductFeatureGroupTypeList(int FeatureGroupTypeID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFeatureGroupType";
            cmd.Parameters.AddWithValue("@FeatureGroupTypeID", FeatureGroupTypeID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Header
        /// </summary>
        /// <param name="HeaderID">Int32 HeaderID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductFeatureGroupType(Int32 FeatureGroupTypeID, String FeatureGroupName, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFeatureGroupType";
            cmd.Parameters.AddWithValue("@FeatureGroupTypeID", FeatureGroupTypeID);
            cmd.Parameters.AddWithValue("@FeatureGroupName", FeatureGroupName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Product Header
        /// </summary>
        /// <param name="HeaderID">Int32 HeaderID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductFeatureGroupType(Int32 FeatureGroupTypeID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFeatureGroupType";
            cmd.Parameters.AddWithValue("@FeatureGroupTypeID", FeatureGroupTypeID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        #endregion

        #region Product Feature Setups ADMIN

        /// <summary>
        /// Get Product Feature List
        /// </summary>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductFeatureList(int FeatureID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFeature";
            cmd.Parameters.AddWithValue("@FeatureID", FeatureID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Feature
        /// </summary>
        /// <param name="FeatureID">Int32 FeatureID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductFeature(Int32 FeatureID, String FeatureName, int FeatureGroupType, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder, string ImageName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFeature";
            cmd.Parameters.AddWithValue("@FeatureID", FeatureID);
            cmd.Parameters.AddWithValue("@FeatureName", FeatureName);
            cmd.Parameters.AddWithValue("@FeatureGroupType", FeatureGroupType);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Product Header
        /// </summary>
        /// <param name="HeaderID">Int32 FeatureID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductFeature(Int32 FeatureID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFeature";
            cmd.Parameters.AddWithValue("@FeatureID", FeatureID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        #endregion

        #region Product Pattern Setups ADMIN

        /// <summary>
        /// Get Product Pattern List
        /// </summary>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductPatternList(int PatternID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductPattern";
            cmd.Parameters.AddWithValue("@PatternID", PatternID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Pattern
        /// </summary>
        /// <param name="PatternID">Int32 PatternID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductPattern(Int32 PatternID, String PatternName, string Description, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder, string ImageName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductPattern";
            cmd.Parameters.AddWithValue("@PatternID", PatternID);
            cmd.Parameters.AddWithValue("@PatternName", PatternName);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Product Pattern
        /// </summary>
        /// <param name="PatternID">Int32 PatternID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductPattern(Int32 PatternID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductPattern";
            cmd.Parameters.AddWithValue("@PatternID", PatternID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        #endregion

        #region Product Style Setups ADMIN

        /// <summary>
        /// Get Product Pattern List
        /// </summary>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductStyleList(int StyleID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductStyle";
            cmd.Parameters.AddWithValue("@StyleID", StyleID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Style
        /// </summary>
        /// <param name="StyleID">Int32 StyleID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductStyle(Int32 StyleID, String StyleName, string Description, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder, string ImageName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductStyle";
            cmd.Parameters.AddWithValue("@StyleID", StyleID);
            cmd.Parameters.AddWithValue("@StyleName", StyleName);
            cmd.Parameters.AddWithValue("@Description", Description);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Product Pattern
        /// </summary>
        /// <param name="PatternID">Int32 PatternID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductStyle(Int32 StyleID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductPattern";
            cmd.Parameters.AddWithValue("@StyleID", StyleID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        #endregion

        #region Product Fabric Group Type Setups ADMIN

        /// <summary>
        /// Get Product Header List
        /// </summary>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductFabricGroupTypeList(int FabricGroupTypeID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFabricGroupType";
            cmd.Parameters.AddWithValue("@FabricGroupTypeID", FabricGroupTypeID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete FabricGroupType 
        /// </summary>
        /// <param name="FabricGroupTypeID">Int32 FabricGroupTypeID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductFabricGroupType(Int32 FabricGroupTypeID, String FabricGroupName, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFabricGroupType";
            cmd.Parameters.AddWithValue("@FabricGroupTypeID", FabricGroupTypeID);
            cmd.Parameters.AddWithValue("@FabricGroupName", FabricGroupName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Product FabricGroupType
        /// </summary>
        /// <param name="FabricGroupTypeID">Int32 FabricGroupTypeID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductFabricGroupType(Int32 FabricGroupTypeID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFabricGroupType";
            cmd.Parameters.AddWithValue("@FabricGroupTypeID", FabricGroupTypeID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        #endregion

        #region Product Fabric Setups ADMIN

        /// <summary>
        /// Get Product Fabric List
        /// </summary>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductFabricList(int FabricID, bool Issearch, string SearchBy, string SearchValue, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFabric";
            cmd.Parameters.AddWithValue("@FabricID", FabricID);
            cmd.Parameters.AddWithValue("@issearch", Issearch);
            cmd.Parameters.AddWithValue("@searchby", SearchBy);
            cmd.Parameters.AddWithValue("@searchvalue", SearchValue);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Insert Update Delete Fabric
        /// </summary>
        /// <param name="FabricID">Int32 FabricID</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <param name="Active">Boolean Active</param>
        /// <returns>Return the ID </returns>
        public Int32 InsertUpdateProductFabric(Int32 FabricID, String FabricName, int FabricGroupType, Boolean Active, bool Deleted, Int32 Mode, Int32 DisplayOrder, string ImageName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFabric";
            cmd.Parameters.AddWithValue("@FabricID", FabricID);
            cmd.Parameters.AddWithValue("@FabricName", FabricName);
            cmd.Parameters.AddWithValue("@FabricGroupType", FabricGroupType);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@Active", Active);
            cmd.Parameters.AddWithValue("@Deleted", Deleted);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Product Fabric
        /// </summary>
        /// <param name="FabricID">Int32 FabricID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteProductFabric(Int32 FabricID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ProductFabric";
            cmd.Parameters.AddWithValue("@FabricID", FabricID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }

        #endregion

        #region Question and answer module
        /// <summary>
        /// Get Question, answer and reply dataset
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="StDateCreatedON">SqlDateTime StDateCreatedON</param>
        /// <param name="EndDateCreatedON">SqlDateTime EndDateCreatedON</param>
        /// <param name="StDateUpdatedON">SqlDateTime StDateUpdatedON</param>
        /// <param name="EndDateUpdatedON">SqlDateTime EndDateUpdatedON</param>
        /// <param name="Criteria">String Criteria</param>
        /// <returns>Returns the List of Yahoo Products</returns>
        public DataSet GetQAR(int StoreID, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_getQAR";
            cmd.Parameters.AddWithValue("@StoreId", StoreID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }
        #endregion

    }
}
