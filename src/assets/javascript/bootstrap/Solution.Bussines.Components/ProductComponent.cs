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
    /// Product Component Class Contains Product related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class ProductComponent
    {
        #region Declaration

        public static int _count;
        List<ProductsComponentEntity> lstproduct = new List<ProductsComponentEntity>();
        ProductDAC prodac = new ProductDAC();

        #endregion

        #region Key Functions

        /// <summary>
        /// Inserts a Product row
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns Inserted Unique ID of Product</returns>
        public static Int32 InsertProduct(tb_Product product)
        {
            int proId = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                dac.Create(product);
                proId = product.ProductID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proId;
        }

        /// <summary>
        /// Insert Amazon product
        /// </summary>
        /// <param name="productamazon">tb_ProductAmazon productamazon</param>
        /// <returns>Returns the Identity Value</returns>
        public static Int32 InsertProductAmazon(tb_ProductAmazon productamazon)
        {
            int proId = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                dac.CreateAmazon(productamazon);
                proId = productamazon.ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proId;
        }

        /// <summary>
        /// Update Display Order For Index Page
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="DisplayOrder">String DisplayOrder</param>
        public static void UpdateDisplayOrderForIndexPage(string Option, Int32 ProductId, string DisplayOrder)
        {
            ProductDAC dac = new ProductDAC();
            dac.UpdateDisplayOrderForIndexPage(Option, ProductId, DisplayOrder);

        }



        /// <summary>
        /// Insert a Product Category
        /// </summary>
        /// <param name="ProductCategory">tb_ProductCategory ProductCategory</param>
        /// <returns>Returns the Identity value</returns>
        public static Int32 InsertProductCategory(tb_ProductCategory ProductCategory)
        {
            int productCategoryId = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                dac.CreateProductCategory(ProductCategory);
                productCategoryId = ProductCategory.ProductCategoryID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return productCategoryId;
        }

        /// <summary>
        /// Method for Update Product
        /// </summary>
        /// <param name="product">entity product</param>
        /// <returns>Returns Count of Rows affected</returns>
        public static int UpdateProduct(tb_Product product)
        {
            int RowsAffected = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                RowsAffected = dac.Update(product);
            }
            catch (Exception ex)
            {
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
            ProductDAC dac = new ProductDAC();
            return dac.GetAllProductDetailsbyProductID(ProductID);
        }

        /// <summary>
        /// Get Product Details by AmazonRefID
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <returns>Returns a product details for particular Amazon product</returns>
        public tb_ProductAmazon GetAllProductDetailsbyAmazonRefID(Int32 ProductID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetAllProductDetailsbyAmazonRefID(ProductID);
        }

        /// <summary>
        /// Update product image
        /// </summary>
        /// <param name="product">tb_Product product</param>
        /// <returns>Returns affected rows count</returns>
        public static int UpdateProductImage(tb_Product product)
        {
            int RowsAffected = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                RowsAffected = dac.UpdateProductImage(product);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }

        /// <summary>
        /// Get Products by StoreID
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the product list by StoreID</returns>
        public DataSet GetProductsByStoreID(int StoreID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductByStoreId(StoreID);
        }

        /// <summary>
        /// Get the ProductList for displaying in Gridview
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="ProductTypeId">int ProductTypeId</param>
        /// <param name="CategoryId">int CategoryId</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="status">string status</param>
        /// <returns>Returns the filtered product list </returns>
        public DataSet GetProductfilterData(int startIndex, int pageSize, string sortBy, int StoreID, int ProductTypeId, int ProductTypeDeliveryId, int CategoryId, string SearchBy, string SearchValue, string status)
        {
            ProductDAC dac = new ProductDAC();
            if (!string.IsNullOrEmpty(SearchValue))
                SearchValue = SearchValue.Trim();
            DataSet dsProduct = dac.GetProductListByCategory(startIndex, pageSize, sortBy, StoreID, ProductTypeId, ProductTypeDeliveryId, CategoryId, SearchBy, SearchValue, status);
            if (dsProduct != null && dsProduct.Tables[0].Rows.Count > 0)
                _count = Convert.ToInt32(dsProduct.Tables[1].Rows[0][0]);
            else
                _count = 0;

            try
            {
                if (startIndex > 50)
                {
                    Int32 pagenum = startIndex / pageSize;
                    pagenum = pagenum;
                    HttpContext.Current.Session["SearchGridpage"] = pagenum.ToString();
                }
                else
                {
                    HttpContext.Current.Session["SearchGridpage"] = "0";
                }
                HttpContext.Current.Session["StatusSearch"] = status.ToString();
            }
            catch { }


            return dsProduct;

        }

        /// <summary>
        /// Get Total number of record
        /// </summary>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="ProductTypeId">int ProductTypeId</param>
        /// <param name="CategoryId">int CategoryId</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="status">string status</param>
        /// <returns>Returns Total number of records</returns>
        public static int GetCount(int StoreId, int ProductTypeId, int ProductTypeDeliveryId, int CategoryId, string SearchBy, string SearchValue, string status)
        {
            return _count;
        }

        /// <summary>
        /// Get Display Order By ProductID And CategoryID
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <returns>Returns the display orders for category by product and category ids</returns>
        public static DataSet GetDisplayOrderByProductIDAndCategoryID(Int32 ProductId, Int32 CategoryId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetDisplayOrderByProductIDAndCategoryID(ProductId, CategoryId);
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
        public static int UpdateMultiplePriceForProduct(string Field, decimal Value, int ProductID, int StoreID, string Flag, string Operation)
        {
            int RowsAffected = 0;
            ProductDAC dac = new ProductDAC();
            RowsAffected = dac.UpdateMultiplePriceForProduct(Field, Value, ProductID, StoreID, Flag, Operation);
            return RowsAffected;
        }

        /// <summary>
        /// Update Multiple Product
        /// </summary>
        /// <param name="UpdateQuery">string UpdateQuery</param>
        /// <param name="ProductIDs">int ProductID</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public static int UpdateMultipleProduct(string UpdateQuery, int ProductID)
        {
            int RowsAffected = 0;
            ProductDAC dac = new ProductDAC();
            RowsAffected = dac.UpdateMultiplePoduct(UpdateQuery, ProductID);
            return RowsAffected;
        }

        /// <summary>
        /// Method to display product by different option like IsFeatured,IsBestSeller,IsNewArrival
        /// </summary>
        /// <param name="DisplayOption">string DisplayOption</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="ProductCount">Int32 ProductCount</param>
        /// <returns>Returns the Product List as a Dataset</returns>
        public static DataSet DisplyProductByOption(string DisplayOption, Int32 StoreID, Int32 ProductCount)
        {
            ProductDAC dac = new ProductDAC();
            return dac.DisplyProductByOption(DisplayOption, StoreID, ProductCount);
        }

        /// <summary>
        /// Method to Get All New Arrival Product
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <returns>Returns the List of All New Arrival Products</returns>
        public static DataSet GetAllNewArrivalProduct(string Option)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetAllNewArrivalProduct(Option);
        }

        /// <summary>
        /// Get Secondary Colors Names and Image Path
        /// </summary>
        /// <param name="strColors">String strColor</param>
        /// <returns>RErutns the Dataset of Color Name Image Names</returns>
        public static DataSet GetSecondaryColorsImagePath(String strColors)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetSecondaryColorsImagePath(strColors);
            return DSProduct;
        }

        /// <summary>
        /// Method to Get All New Arrival Product for Admin
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the List of All New Arrival Products for Admin</returns>
        public static DataSet GetAllNewArrivalProductAdmin(Int32 StoreId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetAllNewArrivalProductAdmin(StoreId);
        }

        /// <summary>
        /// Method for Get Product Details By ProductID while Edit Product
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns Product Details in Dataset by ProductID</returns>
        public static DataSet GetProductByProductID(Int32 ProductID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductByProductID(ProductID);
            return DSProduct;
        }

        /// <summary>
        /// Method for Get Product By AmazonRefID while Edit Product
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns Product Details in Dataset by AmazonRefID</returns>
        public static DataSet GetProductByAmazonRefID(Int32 ProductID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductByAmazonRefID(ProductID);
            return DSProduct;
        }

        /// <summary>
        /// Get product Details
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Product Details Data in DataSet</returns>
        public static DataSet GetProductDetailByID(Int32 ProductID, int StoreID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductDetailByID(ProductID, StoreID);
            return DSProduct;
        }

        /// <summary>
        /// Method to Get Product Detail By Category ID and Store ID
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Product Details</returns>
        public static DataSet GetProductDetailByCategoryID(int CategoryID, int StoreID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductDetailByCategoryID(CategoryID, StoreID);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details by Order Price</returns>
        public static DataSet GetProductDetailsByOrderPrice(int CategoryID, int StoreID, string Option,string color)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductDetailsByOrderPrice(CategoryID, StoreID, Option, color);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details by Order Price</returns>
        public static DataSet GetFreeSwatchProductDetailsByOrderPrice(string CategoryID, int StoreID, string Option)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetFreeSwatchProductDetailsByOrderPrice(CategoryID, StoreID, Option);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details by Order Price</returns>
        public static DataSet GetProductDetailsSalesOutlet(int CategoryID, int StoreID, string Option)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductDetailsSalesOutlet(CategoryID, StoreID, Option);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Details by Order Price</returns>
        public static DataSet GetSearchProductDetailsByOrderPrice(int StoreID, string Option, string StrWhrClus)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetSearchProductDetailsByOrderPrice(StoreID, Option, StrWhrClus);
        }

        /// <summary>
        /// Method to get Product detail by category id and store id
        /// </summary>
        /// <param name="CategoryID">int CategoryId</param>
        /// <param name="StoreID">int StoreId</param>
        /// <param name="Option">string Option</param>
        /// <returns>Returns Products Data</returns>
        public static DataSet GetProductDetailsByShopByPrice(int PriceRangeId, int StoreID, string Option)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductDetailsByShopByPrice(PriceRangeId, StoreID, Option);
        }


        /// <summary>
        /// Update Product By Display Order Price Inventory
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="Qty">string Qty</param>
        /// <param name="SalePrice">string SalePrice</param>
        public static void UpdateProductByDisplayOrderPriceInventory(string Option, Int32 ProductId, string Qty, string SalePrice)
        {
            ProductDAC dac = new ProductDAC();
            dac.UpdateProductByDisplayOrderPriceInventory(Option, ProductId, Qty, SalePrice);

        }

        /// <summary>
        /// Get Product Variant By product ID
        /// </summary>
        /// <param name="ProductId">Int32 Product ID</param>
        /// <returns>Returns the list of product variant by prouctID</returns> 
        public static DataSet GetProductVariantByproductID(Int32 ProductId)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSVariant = new DataSet();
            DSVariant = dac.GetProductVariantByproductID(ProductId);
            return DSVariant;
        }

        /// <summary>
        /// Get Product Variant By productID and Engraving
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returnsReturns the product variant list by productID></returns>
        public static DataSet GetProductVariantByproductIDandEngraving(Int32 ProductId, Int32 Mode)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSVariant = new DataSet();
            DSVariant = dac.GetProductVariantByproductIDandEngraving(ProductId, Mode);
            return DSVariant;
        }
        public static DataSet GetProductVariantByproductIDyahoo(Int32 ProductId, Int32 Mode, Int32 variantValueid)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSVariant = new DataSet();
            DSVariant = dac.GetProductVariantByproductIDyahoo(ProductId, Mode, variantValueid);
            return DSVariant;
        }

        /// <summary>
        /// Get Product Rating
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        /// <returns>Returns the List of Product Rating</returns>
        public static DataSet GetProductRating(Int32 ProductId)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductRating(ProductId);
            return DSProduct;
        }

        /// <summary>
        /// Get Recently Product Without Current Product
        /// </summary>
        /// <param name="ProductId">int product Id</param>
        /// <returns></returns>
        public static DataSet GetRecentlyProduct(string ProductId, int StoreID, int OrgProductID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetRecentlyProduct(ProductId, StoreID, OrgProductID);
            return DSProduct;
        }

        /// <summary>
        /// Get Recently Product Without Current Product
        /// </summary>
        /// <param name="ProductId">int product Id</param>
        /// <param name="OrgProductID">int OrgProductID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Recently Product Details By ProductID</returns>
        public DataSet GetRecentlyViewProduct(string ProductId, int StoreID, int OrgProductID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetRecentlyProduct(ProductId, StoreID, OrgProductID);
            return DSProduct;
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
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetYoumayalsoLikeProduct(ProductId, TopRpoduct, StoreID);
            return DSProduct;
        }

        /// <summary>
        /// Get the Related Product List
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the Related Product List</returns>
        public DataSet GetRelatedProduct(Int32 ProductId, int StoreID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetRelatedProduct(ProductId, StoreID);
            return DSProduct;
        }

        /// <summary>
        /// Get Related Product Add to Cart
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns the Related product for Add to Cart</returns>
        public DataSet GetRelatedProductAddTocart(Int32 ProductId, Int32 StoreId)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetRelatedProductAddTocart(ProductId, StoreId);
            return DSProduct;
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
            ProductDAC dac = new ProductDAC();
            return dac.AddOrUpdateRating(CustomerID, ProductID, Comments, EmailID, Rating, StoreID, Name);
        }

        /// <summary>
        /// Get product Image name
        /// </summary>
        /// <param name="ProductID">int Product ID</param>
        /// <returns>Returns the image name for particular product</returns>
        public static DataSet GetproductImagename(Int32 ProductID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetproductImagename(ProductID);

        }

        /// <summary>
        /// Update Product For View
        /// </summary>
        /// <param name="ProductId">int32 ProductID</param>
        public void UpdateProductForView(Int32 ProductId)
        {
            ProductDAC dac = new ProductDAC();
            dac.UpdateProductForView(ProductId);
        }

        /// <summary>
        /// Get Products By StoreID
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns the List of Products by StoreID</returns>
        public static DataSet GetProductByStoreID(Int32 StoreID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductByStoreID(StoreID);
        }

        /// <summary>
        /// Method to Get Index Page Config value
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the List of Index Page Config value</returns>
        public static DataSet GetIndexPageConfig(Int32 StoreID, string Name)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductByStoreID(StoreID, Name);
        }

        /// <summary>
        /// Update Product Feature 
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="status">bool status</param>
        /// <param name="StoreID">Int32 StoreID</param>
        public static void UpdateProductFeature(Int32 ProductID, bool status, Int32 StoreID)
        {
            ProductDAC dac = new ProductDAC();
            dac.UpdateProductFeature(ProductID, status, StoreID);
        }


        /// <summary>
        /// Update Product Best Seller
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="status">bool status</param>
        /// <param name="StoreID">Int32 StoreID</param>
        public static void UpdateProductBestSeller(Int32 ProductID, bool status, Int32 StoreID)
        {
            ProductDAC dac = new ProductDAC();
            dac.UpdateProductBestSeller(ProductID, status, StoreID);
        }


        /// <summary>
        /// Update New Arrival Product
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="status">bool status</param>
        /// <param name="StoreID">Int32 StoreID</param>
        public static void UpdateProductNewArrival(Int32 ProductID, bool status, Int32 StoreID)
        {
            ProductDAC dac = new ProductDAC();
            dac.UpdateProductNewArrival(ProductID, status, StoreID);
        }

        /// <summary>
        /// Get Search Data For Product
        /// </summary>
        /// <param name="SearchText">SearchText</param>
        /// <param name="PriceFrom">PriceFrom</param>
        /// <param name="PriceTo">PriceTo</param>
        /// <param name="CategoryId">CategoryId</param>
        /// <param name="IsSearchInDescription">IsSearchInDescription</param>
        /// <param name="StoreId">StoreId</param>
        /// <returns>DataSet</returns>
        public static DataSet GetSearchDataForProduct(String SearchText, String PriceFrom, String PriceTo, String CategoryId, bool IsSearchInDescription, Int32 StoreId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetSearchData(SearchText, PriceFrom, PriceTo, CategoryId, IsSearchInDescription, StoreId);
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
        public static int UpdateProduct(string Field, decimal Value, int ProductID, int StoreID, string Flag, string Operation)
        {
            int RowsAffected = 0;
            ProductDAC dac = new ProductDAC();
            RowsAffected = dac.UpdateProduct(Field, Value, ProductID, StoreID, Flag, Operation);
            return RowsAffected;
        }

        /// <summary>
        /// Method to Get Product Variant Count
        /// </summary>
        /// <param name="StoreID">Int32 ProductID</param>
        /// <returns>Returns the count of total Product Variant</returns>
        public DataSet GetProductVariantcount(Int32 ProductId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductVariantTotalCount(ProductId);
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
        public static int SaveProductVariant(Int32 ProductId, string VariantName, string VariantValue, decimal VariantPrice, Int32 DisplayOrder, String SKU, String Header, String UPC, Int32 IsParent)
        {
            ProductDAC dac = new ProductDAC();
            return dac.SaveProductVariant(ProductId, VariantName, VariantValue, VariantPrice, DisplayOrder, SKU, Header, UPC, IsParent);
        }

        /// <summary>
        /// Get Product Variant Value
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantId">Int32 ProductId</param>
        /// <returns>Returns the Details o Product Variant value</returns>
        public static DataSet GetProductVariantValues(Int32 ProductId, Int32 VariantId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductVariantValue(ProductId, VariantId);
        }

        /// <summary>
        /// Delete Variant Value for particular product and variant
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantId">Int32 VariantId</param>
        /// <param name="VariantValueId">Int32 VariantValueId</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteVariantValues(Int32 ProductId, Int32 VariantId, Int32 VariantValueId)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteVariantValue(ProductId, VariantId, VariantValueId));
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
        public static int InsertProductVariantValue(Int32 VariantId, Int32 ProductId, string VariantName, string VariantValue, decimal VariantPrice, Int32 DisplayOrder, String SKU, String Header, String UPC)
        {
            ProductDAC dac = new ProductDAC();
            return dac.InsertProductVariantValue(VariantId, ProductId, VariantName, VariantValue, VariantPrice, DisplayOrder, SKU, Header, UPC);
        }

        /// <summary>
        /// Update Product Variant Value
        /// </summary>
        /// <param name="VariantId">Int32 VariantId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantValueId">Int32 VariantValueId</param>
        /// <param name="VariantPrice">decimal VariantPrice</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        /// <returns>Returns String</returns>
        public static int UpdateProductVariantValue(Int32 VariantId, Int32 ProductId, Int32 VariantValueId, decimal VariantPrice, Int32 DisplayOrder, String SKU, String Header, String UPC)
        {
            ProductDAC dac = new ProductDAC();
            return dac.UpdateProductVariantValue(VariantId, ProductId, VariantValueId, VariantPrice, DisplayOrder, SKU, Header, UPC);
        }

        /// <summary>
        /// Delete Variant Option
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="VariantId">Int32 VariantId</param>
        /// <param name="VariantValueId"></param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteVariantOption(Int32 ProductId, Int32 VariantId)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteVariantOption(ProductId, VariantId));
        }

        /// <summary>
        /// Get Search Product Value
        /// </summary>
        /// <param name="StoreIdId">Int32 StoreIdId</param>
        /// <param name="Whrclus">String Whrclus</param>
        /// <returns>Returns Search product Value</returns>
        public static DataSet GetSearchProductVal(Int32 StoreIdId, String WhrClus)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetSearchProductValue(StoreIdId, WhrClus);
        }


        /// <summary>
        /// Method to Generate RSS 
        /// </summary>
        /// <param name="AppPath">String AppPath</param>
        /// <param name="StoreID">String StoreID</param>
        /// <returns>Returns the Product RSS List</returns>
        public DataSet GenerateProductRSS(String AppPath, String StoreID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GenerateProductRSS(AppPath, StoreID);
        }


        /// <summary>
        /// Get the Data source for Grid view after searching,sorting and on first time load
        /// </summary>
        /// <param name="startIndex">Starting index of page</param>
        /// <param name="pageSize">Grid view Page size</param>
        /// <param name="sortBy">Sorting Order</param>
        /// <param name="CName">Counting Variable</param>
        /// <param name="pStoreId">Store id</param>
        /// <param name="pSearchValue">searching value variable</param>
        /// <returns>return IQueryable</returns>      
        public IQueryable<ProductsComponentEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchBy)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            IQueryable<ProductsComponentEntity> results = from a in ctx.tb_Rating
                                                          select new ProductsComponentEntity
                                                          {
                                                              RatingID = a.RatingID,
                                                              ApprovedDate = (a.ApprovedDate ?? DateTime.Now),
                                                              CreatedOn = a.CreatedOn.Value,
                                                              Comments = a.Comments,
                                                              IsApproved = a.IsApproved.Value,
                                                              Rating = a.Rating.Value,
                                                              //CustomerID=a.tb_Customer.CustomerID,
                                                              CustomerName = a.Name,
                                                              StoreID = a.tb_Store.StoreID,
                                                              StoreName = a.tb_Store.StoreName,
                                                              ProductName = a.tb_Product.Name,
                                                              ProductID = a.tb_Product.ProductID
                                                          };


            if (pStoreId != 0)
            {
                if (pSearchBy == "1")
                {
                    results = results.Where(a => a.IsApproved == 1 && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pSearchBy == "0")
                {
                    results = results.Where(a => a.IsApproved == 0 && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pSearchBy == "-1")
                {
                    results = results.Where(a => a.IsApproved == -1 && a.StoreID == pStoreId).AsQueryable();
                }
                else
                {
                    results = results.Where(a => a.IsApproved == 1 && a.StoreID == pStoreId).AsQueryable();
                }
            }
            else
            {
                if (pSearchBy == "1")
                {
                    results = results.Where(a => a.IsApproved == 1).AsQueryable();
                }
                else if (pSearchBy == "0")
                {
                    results = results.Where(a => a.IsApproved == 0).AsQueryable();
                }
                else if (pSearchBy == "-1")
                {
                    results = results.Where(a => a.IsApproved == -1).AsQueryable();
                }
                else
                {
                    results = results.AsQueryable();
                }
            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lstproduct.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    results = results.OrderByField(SortingOption[0].ToString(), true);
                }
                else if (SortingOption.Length == 2)
                {
                    results = results.OrderByField(SortingOption[0].ToString(), false);
                }
            }
            else
            {
                results = results.OrderBy(o => o.ProductName);
            }
            results = results.Skip(startIndex).Take(pageSize);
            return results;
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns Object</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Get No Of Rows
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns Total no of count</returns>
        public static int GetCount(string CName, int pStoreId, string pSearchBy)
        {
            return _count;
        }

        /// <summary>
        /// Gets the rating of product
        /// </summary>
        /// <param name="Id">ProductID</param>
        /// <returns>Returns tb_Rating</returns>
        public tb_Rating GetRatingDetail(int Id)
        {
            return prodac.GetRatingDetail(Id);
        }

        /// <summary>
        /// Updates Product Rating Review
        /// </summary>
        /// <param name="tb_rating">tb_Rating tb_rating</param>
        public void UpdateReview(tb_Rating tb_rating)
        {
            prodac.UpdateReview(tb_rating);
        }

        /// <summary>
        /// Get Product list By Search
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Name">string ProductName</param>
        /// <returns>Returns DataSet</returns>
        public static DataSet GetProductBySearch(Int32 StoreID, string Name)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductByStoreID(StoreID, Name);
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
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductForYahooStore(StoreID, StDateCreatedON, EndDateCreatedON, StDateUpdatedON, EndDateUpdatedON, Criteria);
            return DSProduct;
        }


        /// <summary>
        /// Save Clone Product
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <returns>Returns the Identity Value</returns>
        public static int SaveCloneProductID(Int32 StoreId, Int32 ProdudtId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.SaveCloneProduct(StoreId, ProdudtId);
        }

        /// <summary>
        /// Get Product Detail By ProductID
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns a Product Details By ProductID</returns>
        public tb_Product GetProductDetailByProductID(int ProductID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductDetailByProductID(ProductID);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetProductFabricDetails(Int32 FabricTypeID, Int32 Mode)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductFabricDetails(FabricTypeID, Mode);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetFabricVendorPortalDetails(Int32 FabricTypeID, Int32 Mode)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetFabricVendorPortalDetails(FabricTypeID, Mode);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetFabricVendorPortalDetailsForPopup(Int32 FabricTypeID, Int32 FabricCodeId, Int32 Mode)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetFabricVendorPortalDetailsForPopup(FabricTypeID, FabricCodeId, Mode);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetFabricVendorPortalCodeDetails(Int32 FabricTypeID, Int32 FabricCodeId, Int32 Mode)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetFabricVendorPortalDetails(FabricTypeID, FabricCodeId, Mode);
        }

        /// <summary>
        /// Get eBay Category
        /// </summary>
        /// <returns>Returns the eBay Category List</returns>
        public DataSet GeteBayCategory()
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GeteBayCategory();
        }

        /// <summary>
        /// Get eBay Store Category
        /// </summary>
        /// <returns>Returns the eBay Store category List</returns>
        public DataSet GetebayStoreCategory()
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GeteBayStoreCategory();
        }

        /// <summary>
        /// Get Quantity Discount Table By ProductID
        /// </summary>
        /// <param name="ProductID">string ProductID</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Quantity Discount list By ProductID</returns>
        public DataSet GetQuantityDiscountTableByProductID(string ProductID, string StoreID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetQuantityDiscountTableByProductID(ProductID, StoreID);
        }

        /// <summary>
        /// Get Quantity Discount Table By Item
        /// </summary>
        /// <param name="ProductID">string ProductID</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Quantity Discount list By Item</returns>
        public DataSet GetQuantityDiscountTableByItem(string ProductID, string StoreID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetQuantityDiscountTableByItem(ProductID, StoreID);
        }

        /// <summary>
        /// Save Clone Product Variant
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="NewProductId">Int32 NewProductId</param>
        /// <returns>Returns the Identity Value</returns>
        public static int SaveCloneProductVariant(Int32 StoreId, Int32 ProdudtId, Int32 NewProductId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.SaveCloneProductVariant(StoreId, ProdudtId, NewProductId);
        }

        /// <summary>
        /// Insert Gift card product
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertGiftcardproduct(Int32 StoreId, Int32 ProdudtId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.InsertGiftcardproduct(StoreId, ProdudtId);
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
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetGiftCardProductList(StoreID, Issearch, SearchBy, SearchValue);
        }

        /// <summary>
        /// Get Search Product Value
        /// </summary>
        /// <param name="StoreIdId">Int32 StoreIdId</param>
        /// <param name="Whrclus">String Whrclus</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns the Searched Product List</returns>
        public static DataSet GetSearchProductValforCategory(Int32 StoreIdId, String WhrClus, int Mode)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetSearchProductValue(StoreIdId, WhrClus, Mode);
        }

        /// <summary>
        /// Delete Gift Card Product
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteGiftCardProduct(Int32 ProductId, int StoreID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteGiftCardProduct(ProductId, StoreID));
        }

        /// <summary>
        /// Get WareHouse Details By ID
        /// </summary>
        /// <param name="WarehouseID">int WarehouseID</param>
        /// <returns>Returns the WareHouse Details By ID</returns>
        public tb_WareHouse GetWarehouseByID(int WarehouseID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetWarehouseByID(WarehouseID);
        }

        /// <summary>
        /// Update WareHouse
        /// </summary>
        /// <param name="warehouse">tb_WareHouse warehouse</param>
        /// <returns>Returns Count of number of rows Affected</returns>
        public Int32 UpdateWarehouse(tb_WareHouse warehouse)
        {
            int RowsAffected = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                RowsAffected = dac.UpdateWarehouse(warehouse);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }

        /// <summary>
        /// Create WareHouse
        /// </summary>
        /// <param name="warehouse">tb_WareHouse warehouse</param>
        /// <returns>Returns Identity Value</returns>
        public Int32 CreateWarehouse(tb_WareHouse warehouse)
        {
            Int32 isAdded = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                dac.CreateWarehouse(warehouse);
                isAdded = warehouse.WareHouseID;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Get Filtered WareHouse List
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the Filtered WareHouse List</returns>
        public static IQueryable<tb_WareHouse> GetDataByFilterForWarehouse(int startIndex, int pageSize, string sortBy, string SearchValue)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            if (string.IsNullOrEmpty(SearchValue))
            {
                SearchValue = "";
            }
            else
            {
                SearchValue = SearchValue.Trim();
            }
            IQueryable<tb_WareHouse> result = from warehouse in ctx.tb_WareHouse
                                              where warehouse.Deleted == false &&
                                              warehouse.Active == true &&
                                              warehouse.Name.Contains(SearchValue)
                                              select warehouse;
            _count = result.Count();

            //Logic for searching
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = result.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    result = result.OrderByField(SortingOption[0].ToString(), true);
                }
                else if (SortingOption.Length == 2)
                {
                    result = result.OrderByField(SortingOption[0].ToString(), false);
                }
            }
            else
            {
                //Default sorting by Name
                result = result.OrderBy(o => o.Name);
            }
            result = result.Skip(startIndex).Take(pageSize);
            return result;
        }

        /// <summary>
        /// Get Count for GetDataByFilterForWarehouse()
        /// </summary>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the Row count</returns>
        public static int GetCount(string SearchValue)
        {
            return _count;
        }

        /// <summary>
        /// Get Warehouse List
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="ProductID">int ProductID</param>
        /// <returns>Returns the WareHouse List as a Dataset</returns>
        public DataSet GetWarehouseList(int Mode, int ProductID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetWarehouseList(Mode, ProductID);
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
            ProductDAC dac = new ProductDAC();
            return dac.InsertUpdateWarehouse(WarehouseID, ProductID, Inventory, mode, PreferredLocation);
        }

        /// <summary>
        /// Get All Best Seller Product
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Best Seller Product List</returns>
        public int UpdateProductinventory(int ProductID, int Inventory, int StoreID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.UpdateProductinventory(ProductID, Inventory, StoreID);
        }

        /// <summary>
        /// Get All Best Seller Product
        /// </summary>
        /// <param name="Option">string Option</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Best Seller Product List</returns>
        public static DataSet GetAllBestSellerProduct(string Option, string StoreID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetAllBestSellerProduct(Option, StoreID);
        }

        /// <summary>
        /// Get Optional Products Details By IDs
        /// </summary>
        /// <param name="SKUs">String SKUs</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns the Optional Product Details by ID</returns>
        public static DataSet GetOptinalProductDetailByID(String SKUs, Int32 StoreID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetOptinalProductDetailByID(SKUs, StoreID);
            return DSProduct;
        }

        /// <summary>
        /// Get Gift Card By StoreID
        /// </summary>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Gift Card By StoreID</returns>
        public DataSet GetGiftCardByStoreID(string StoreID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetGiftCardByStoreID(StoreID);
            return DSProduct;
        }

        /// <summary>
        /// Get Product By ID For Gift
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="StoreID">string StoreID</param>
        /// <returns>Returns the Product By ID for Gift Certificate</returns>
        public DataSet GetProductByIDForGift(Int32 ProductID, string StoreID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductByIDForGift(ProductID, StoreID);
            return DSProduct;
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
        public string AddGiftCard(Int32 CustomerID, Int32 ProductID, Int32 OrderNumber, String EmailFrom, String EmailTo, String Emailname, String Emailmessage, Int32 StoreId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.AddGiftCard(CustomerID, ProductID, OrderNumber, EmailFrom, EmailTo, Emailname, Emailmessage, StoreId);
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
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.getLatestProductInquiry(StoreId, Mode, SearchFrom, SearchTo, OrderByColumn, OrderBy);
            return DSProduct;
        }

        /// <summary>
        /// Delete Product Inquiry
        /// </summary>
        /// <param name="StoreId">int StoreId</param>
        /// <param name="IDs">string IDs</param>
        /// <returns>Returns 1 if Deleted</returns>
        public void deleteProductInquiry(int StoreId, string IDs)
        {
            ProductDAC dac = new ProductDAC();
            dac.deleteProductInquiry(StoreId, IDs);
        }

        /// <summary>
        /// Get Product Detail By product ID
        /// </summary>
        /// <param name="ProductID">Int Product ID</param>
        /// <returns>Returns Product Data in DataSet</returns>
        public static DataSet GetProductDetailByID(Int32 ProductID)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductDetailByID(ProductID);
            return DSProduct;
        }

        /// <summary>
        /// Get Variant Warehouse List
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="ProductID">int ProductID</param>
        /// <param name="VariantID">int VariantID</param>
        /// <param name="VariantValueID">int VariantValueID</param>
        /// <returns>Returns the WareHouse List of Variant Value as a Dataset</returns>
        public DataSet GetVariantWarehouseList(int Mode, int ProductID, int VariantID, int VariantValueID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetVariantWarehouseList(Mode, ProductID, VariantID, VariantValueID);
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
            ProductDAC dac = new ProductDAC();
            return dac.InsertUpdateVariantWarehouse(WarehouseID, ProductID, Inventory, mode, VariantID, VariantValueID, CreatedBy, UpdatedBy, PreferredLocation);
        }

        /// <summary>
        /// GetProduct Fabric Type
        /// </summary>
        /// <returns>Returns the Fabric Types of Product as a Dataset</returns>
        public DataSet GetProductFabricType()
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductFabricType();
        }

        /// <summary>
        /// GetProduct Fabric Code
        /// </summary>
        /// <returns>Returns the Fabric Code of Product as a Dataset</returns>
        public DataSet GetProductFabricCode(Int32 FabricTypeID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductFabricCode(FabricTypeID);
        }

        /// <summary>
        /// GetProduct Fabric Code
        /// </summary>
        /// <returns>Returns the Fabric Code of Product as a Dataset</returns>
        public DataSet GetProductFabricWidth(Int32 FabricCodeID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductFabricWidth(FabricCodeID);
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
            ProductDAC dac = new ProductDAC();
            return dac.Insert_Update_Delete_FabricType(FabricTypeID, FabricType, DisplayOrder, Active, Mode);
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
            ProductDAC dac = new ProductDAC();
            return dac.Insert_Update_Delete_FabricCode(FabricTypeID, FabricCodeID, Code, Name, DisplayOrder, Active, Mode);
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
            ProductDAC dac = new ProductDAC();
            return dac.Insert_Update_Delete_FabricWidth(hdnFabricCodeID, FabricWidthID, Width, DisplayOrder, Active, Mode);
        }

        /// <summary>
        /// Get Product Rating
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        /// <returns>Returns the List of Product Rating</returns>
        public static DataSet GetProductRatingCount(Int32 ProductId, string StrSort)
        {
            ProductDAC dac = new ProductDAC();
            DataSet DSProduct = new DataSet();
            DSProduct = dac.GetProductRatingCount(ProductId, StrSort);
            return DSProduct;
        }

        public DataSet GetProductFabricType(Int32 FabricTypeID)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductFabricType(FabricTypeID);
        }

        /// <summary>
        /// GetProduct Fabric Code
        /// </summary>
        /// <returns>Returns the Fabric Code of Product as a Dataset</returns>
        public DataSet GetProductFabricCode(Int32 FabricTypeID, String Code)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductFabricCode(FabricTypeID, Code);
        }

        /// <summary>
        ///Get Product Fabric Details
        /// </summary>
        /// <returns>Returns Product Fabric Details</returns>
        public DataSet GetFabricVendorPortalDetails(Int32 FabricTypeID, Int32 Mode, string Code)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetFabricVendorPortalDetails(FabricTypeID, Mode, Code);
        }
        ///// <summary>
        /////Get Product Fabric Details
        ///// </summary>
        ///// <returns>Returns Product Fabric Details</returns>
        //public DataSet GetFabricVendorPortalDetails(Int32 FabricTypeID, Int32 Mode)
        //{
        //    ProductDAC dac = new ProductDAC();
        //    return dac.GetFabricVendorPortalDetails(FabricTypeID, Mode);
        //}

        #region Product Feature

        public IQueryable<ProductFeatureEntity> GetDataByFilterProductFeature(int startIndex, int pageSize, string sortBy, string SearchBy, string SearchValue, int StoreID)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                IQueryable<ProductFeatureEntity> results = from a in ctx.tb_ProductFeature
                                                           from b in ctx.tb_Store
                                                           where a.tb_Store.StoreID == b.StoreID
                                                            && ((System.Boolean?)a.tb_Store.Deleted ?? false) == false
                                                           select new ProductFeatureEntity
                                                           {
                                                               FeatureId = a.FeatureId,
                                                               Name = a.Name,
                                                               StoreID = a.tb_Store.StoreID,
                                                               StoreName = a.tb_Store.StoreName,
                                                               Active = a.Active ?? false
                                                           };

                if (string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = "";
                }
                else
                {
                    SearchValue = SearchValue.Trim();
                }
                if (StoreID != -1)
                {
                    if (SearchBy == "Feature Name")
                    {

                        results = results.Where(a => a.Name.Contains(SearchValue) && a.StoreID == StoreID).AsQueryable();
                    }
                    else
                    {
                        results = results.Where(a => a.StoreID == StoreID).AsQueryable();
                    }
                }
                else
                {
                    if (SearchBy == "Feature Name")
                    {

                        results = results.Where(a => a.Name.Contains(SearchValue)).AsQueryable();
                    }
                }

                _count = results.Count();
                //Logic for searching
                if (!string.IsNullOrEmpty(sortBy))
                {
                    System.Reflection.PropertyInfo property = results.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);
                    String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    if (SortingOption.Length == 1)
                    {
                        results = results.OrderByField(SortingOption[0].ToString(), true);
                    }
                    else if (SortingOption.Length == 2)
                    {
                        results = results.OrderByField(SortingOption[0].ToString(), false);
                    }
                }
                else
                {
                    //Default sorting by Label
                    results = results.OrderBy(o => o.Name);
                }
                results = results.Skip(startIndex).Take(pageSize);
                return results;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// Get Total number of records used for paging
        /// </summary>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="StoreID">int StoreID</param>        
        /// <returns>Returns Total number of records</returns>
        public static int GetCountProductFeature(string SearchBy, string SearchValue, int StoreID)
        {
            return _count;
        }

        public Int32 CreateProductFeature(tb_ProductFeature feature)
        {
            Int32 isAdded = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                dac.CreateProductFeature(feature);
                isAdded = feature.FeatureId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        public tb_ProductFeature GetProductFeatureByID(int FeatureId)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetProductFeatureByID(FeatureId);
        }

        public Int32 UpdateProductFeature(tb_ProductFeature feature)
        {
            int RowsAffected = 0;
            try
            {
                ProductDAC dac = new ProductDAC();
                RowsAffected = dac.UpdateProductFeature(feature);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }

        public Int32 DeleteFeatureList(Int32 FeatureId)
        {
            Int32 isDeleted = 0;
            try
            {
                isDeleted = prodac.DeleteFeatureList(FeatureId);

                if (isDeleted > 0)
                {


                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isDeleted;
        }

        public bool CheckDuplicate(tb_ProductFeature feature)
        {
            ProductDAC dac = new ProductDAC();
            if (dac.CheckDuplicate(feature) == 0)
            {
                return false;
            }
            else
                return true;
        }

        public DataSet GetproductFeature(Int32 Featureid, Int32 Storeid, Int32 Mode)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetproductFeature(Featureid, Storeid, Mode);
        }

        #endregion
        public DataSet GetItemchannelReport(string SearchValue)
        {
            ProductDAC dac = new ProductDAC();
            return dac.GetItemchannelReport(SearchValue);
        }


        #region Product Color Setups
        public DataSet GetProductColorList(int ColorID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductColorList(ColorID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductColorbyID(int ColorID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductColorList(ColorID, false, null, null, 5);
        }


        /// <summary>
        /// Insert product color
        /// </summary>
        /// <param name="ColorId">Int32 ColorId</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductColor(Int32 ColorID, string ColorName, string ImageName, bool IsActive, bool Deleted, int DisplayOrder)
        {
            ProductDAC dac = new ProductDAC();
            if (ColorID > 0)
                return dac.InsertUpdateProductColor(ColorID, ColorName, ImageName, IsActive, Deleted, 4, DisplayOrder);
            else
                return dac.InsertUpdateProductColor(ColorID, ColorName, ImageName, IsActive, Deleted, 1, DisplayOrder);
        }

        /// <summary>
        /// Delete Product Color
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductColor(Int32 ColorID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductColor(ColorID));
        }
        #endregion

        #region Product Size Setups

        /// <summary>
        /// Get Product Size List
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Issearch">bool Issearch</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns the List of Gift Card Product List</returns>
        public DataSet GetProductSizeList(int SizeID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductSizeList(SizeID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductSizeListbyID(int SizeID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductSizeList(SizeID, false, null, null, 5);
        }

        public DataSet GetProductSizebyID(int SizeID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductSizeList(SizeID, false, null, null, 5);
        }

        /// <summary>
        /// Insert product size
        /// </summary>
        /// <param name="ColorId">Int32 sizeId</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductSize(Int32 SizeID, string SizeName, Int32 Width, Int32 Length, bool IsActive, bool Deleted, int DisplayOrder, string ImageName)
        {
            ProductDAC dac = new ProductDAC();
            if (SizeID > 0)
                return dac.InsertUpdateProductSize(SizeID, SizeName, Width, Length, IsActive, Deleted, 4, DisplayOrder, ImageName);
            else
                return dac.InsertUpdateProductSize(SizeID, SizeName, Width, Length, IsActive, Deleted, 1, DisplayOrder, ImageName);
        }
        /// <summary>
        /// Delete Product Size
        /// </summary>
        /// <param name="SizeID">Int32 SizeID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductSize(Int32 SizeID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductSize(SizeID));
        }

        #endregion

        #region Product Header Setups
        public DataSet GetProductHeaderList(int HeaderID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductHeaderList(HeaderID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductHeaderbyID(int HeaderID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductHeaderList(HeaderID, false, null, null, 5);
        }


        /// <summary>
        /// Insert product header
        /// </summary>
        /// <param name="HeaderID">Int32 HeaderID</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductHeader(Int32 HeaderID, string HeaderName, string Description, bool IsActive, bool Deleted, int DisplayOrder, string ImageName)
        {
            ProductDAC dac = new ProductDAC();
            if (HeaderID > 0)
                return dac.InsertUpdateProductHeader(HeaderID, HeaderName, Description, IsActive, Deleted, 4, DisplayOrder, ImageName);
            else
                return dac.InsertUpdateProductHeader(HeaderID, HeaderName, Description, IsActive, Deleted, 1, DisplayOrder, ImageName);
        }

        /// <summary>
        /// Delete Product Color
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductHeader(Int32 HeaderID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductHeader(HeaderID));
        }
        #endregion

        #region Product Room Setups
        public DataSet GetProductRoomList(int RoomID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductRoomList(RoomID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductRoombyID(int RoomID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductRoomList(RoomID, false, null, null, 5);
        }


        /// <summary>
        /// Insert product header
        /// </summary>
        /// <param name="RoomID">Int32 RoomID</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductRoom(Int32 RoomID, string RoomName, string Description, bool IsActive, bool Deleted, int DisplayOrder, string ImageName)
        {
            ProductDAC dac = new ProductDAC();
            if (RoomID > 0)
                return dac.InsertUpdateProductRoom(RoomID, RoomName, Description, IsActive, Deleted, 4, DisplayOrder, ImageName);
            else
                return dac.InsertUpdateProductRoom(RoomID, RoomName, Description, IsActive, Deleted, 1, DisplayOrder, ImageName);
        }

        /// <summary>
        /// Delete Product Color
        /// </summary>
        /// <param name="RoomId">Int32 RoomId</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductRoom(Int32 RoomID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductRoom(RoomID));
        }
        #endregion

        #region Product Feature Group Type Setups
        public DataSet GetProductFeatureGroupTypeList(int FeatureGroupTypeID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductFeatureGroupTypeList(FeatureGroupTypeID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductFeatureGroupTypebyID(int FeatureGroupTypeID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductFeatureGroupTypeList(FeatureGroupTypeID, false, null, null, 5);
        }


        /// <summary>
        /// Insert product FeatureGroupType
        /// </summary>
        /// <param name="FeatureGroupTypeID">Int32 FeatureGroupTypeID</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductFeatureGroupType(Int32 FeatureGroupTypeID, string FeatureGroupName, bool IsActive, bool Deleted, int DisplayOrder)
        {
            ProductDAC dac = new ProductDAC();
            if (FeatureGroupTypeID > 0)
                return dac.InsertUpdateProductFeatureGroupType(FeatureGroupTypeID, FeatureGroupName, IsActive, Deleted, 4, DisplayOrder);
            else
                return dac.InsertUpdateProductFeatureGroupType(FeatureGroupTypeID, FeatureGroupName, IsActive, Deleted, 1, DisplayOrder);
        }

        /// <summary>
        /// Delete Product Color
        /// </summary>
        /// <param name="RoomId">Int32 RoomId</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductFeatureGroupType(Int32 FeatureGroupTypeID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductFeatureGroupType(FeatureGroupTypeID));
        }
        #endregion

        #region Product Feature Setups
        public DataSet GetProductFeatureList(int FeatureID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductFeatureList(FeatureID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductFeaturebyID(int FeatureID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductFeatureList(FeatureID, false, null, null, 5);
        }

        /// <summary>
        /// Insert product header
        /// </summary>
        /// <param name="FeatureID">Int32 FeatureID</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductFeature(Int32 FeatureID, string FeatureName, int FeatureGroupType, bool IsActive, bool Deleted, int DisplayOrder, string ImageName)
        {
            ProductDAC dac = new ProductDAC();
            if (FeatureID > 0)
                return dac.InsertUpdateProductFeature(FeatureID, FeatureName, FeatureGroupType, IsActive, Deleted, 4, DisplayOrder, ImageName);
            else
                return dac.InsertUpdateProductFeature(FeatureID, FeatureName, FeatureGroupType, IsActive, Deleted, 1, DisplayOrder, ImageName);
        }

        /// <summary>
        /// Delete Product Feature
        /// </summary>
        /// <param name="FeatureId">Int32 FeatureId</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductFeature(Int32 FeatureID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductFeature(FeatureID));
        }
        #endregion

        #region Product Pattern Setups
        public DataSet GetProductPatternList(int PatternID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductPatternList(PatternID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductPatternbyID(int PatternID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductPatternList(PatternID, false, null, null, 5);
        }


        /// <summary>
        /// Insert product Pattern
        /// </summary>
        /// <param name="RoomID">Int32 PatternID</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductPattern(Int32 PatternID, string PatternName, string Description, bool IsActive, bool Deleted, int DisplayOrder, string ImageName)
        {
            ProductDAC dac = new ProductDAC();
            if (PatternID > 0)
                return dac.InsertUpdateProductPattern(PatternID, PatternName, Description, IsActive, Deleted, 4, DisplayOrder, ImageName);
            else
                return dac.InsertUpdateProductPattern(PatternID, PatternName, Description, IsActive, Deleted, 1, DisplayOrder, ImageName);
        }

        /// <summary>
        /// Delete Product Pattern
        /// </summary>
        /// <param name="PatternId">Int32 PatternId</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductPattern(Int32 PatternID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductPattern(PatternID));
        }
        #endregion

        #region Product Style Setups
        public DataSet GetProductStyleList(int StyleID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductStyleList(StyleID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductStylebyID(int StyleID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductStyleList(StyleID, false, null, null, 5);
        }


        /// <summary>
        /// Insert product Pattern
        /// </summary>
        /// <param name="RoomID">Int32 PatternID</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductStyle(Int32 StyleID, string StyleName, string Description, bool IsActive, bool Deleted, int DisplayOrder, string ImageName)
        {
            ProductDAC dac = new ProductDAC();
            if (StyleID > 0)
                return dac.InsertUpdateProductStyle(StyleID, StyleName, Description, IsActive, Deleted, 4, DisplayOrder, ImageName);
            else
                return dac.InsertUpdateProductStyle(StyleID, StyleName, Description, IsActive, Deleted, 1, DisplayOrder, ImageName);
        }

        /// <summary>
        /// Delete Product Pattern
        /// </summary>
        /// <param name="PatternId">Int32 PatternId</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductStyle(Int32 StyleID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductStyle(StyleID));
        }
        #endregion

        #region Product Fabric Group Type Setups
        public DataSet GetProductFabricGroupTypeList(int FabricGroupTypeID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductFabricGroupTypeList(FabricGroupTypeID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductFabricGroupTypebyID(int FabricGroupTypeID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductFabricGroupTypeList(FabricGroupTypeID, false, null, null, 5);
        }

        /// <summary>
        /// Insert product Fabric Group Type
        /// </summary>
        /// <param name="FabricGroupTypeID">Int32 FabricGroupTypeID</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductFabricGroupType(Int32 FabricGroupTypeID, string FabricGroupName, bool IsActive, bool Deleted, int DisplayOrder)
        {
            ProductDAC dac = new ProductDAC();
            if (FabricGroupTypeID > 0)
                return dac.InsertUpdateProductFabricGroupType(FabricGroupTypeID, FabricGroupName, IsActive, Deleted, 4, DisplayOrder);
            else
                return dac.InsertUpdateProductFabricGroupType(FabricGroupTypeID, FabricGroupName, IsActive, Deleted, 1, DisplayOrder);
        }

        /// <summary>
        /// Delete Product FabricGroupType
        /// </summary>
        /// <param name="FabricGroupTypeID">Int32 FabricGroupTypeID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductFabricGroupType(Int32 FabricGroupTypeID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductFabricGroupType(FabricGroupTypeID));
        }
        #endregion

        #region Product Fabric Setups
        public DataSet GetProductFabricList(int FabricID, bool Issearch, string SearchBy, string SearchValue)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductFabricList(FabricID, Issearch, SearchBy, SearchValue, 2);
        }

        public DataSet GetProductFabricbyID(int FabricID)
        {
            ProductDAC objProductDAC = new ProductDAC();
            return objProductDAC.GetProductFabricList(FabricID, false, null, null, 5);
        }

        /// <summary>
        /// Insert product Fabric
        /// </summary>
        /// <param name="FabricID">Int32 FabricID</param>
        /// <returns>Returns Identity Value</returns>
        public static int InsertUpdateProductFabric(Int32 FabricID, string FabricName, int FabricGroupType, bool IsActive, bool Deleted, int DisplayOrder, string ImageName)
        {
            ProductDAC dac = new ProductDAC();
            if (FabricID > 0)
                return dac.InsertUpdateProductFabric(FabricID, FabricName, FabricGroupType, IsActive, Deleted, 4, DisplayOrder, ImageName);
            else
                return dac.InsertUpdateProductFabric(FabricID, FabricName, FabricGroupType, IsActive, Deleted, 1, DisplayOrder, ImageName);
        }

        /// <summary>
        /// Delete Product Fabric
        /// </summary>
        /// <param name="FabricID">Int32 FabricID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public static int DeleteProductFabric(Int32 FabricID)
        {
            ProductDAC dac = new ProductDAC();
            return Convert.ToInt32(dac.DeleteProductFabric(FabricID));
        }
        #endregion


    }

    public class ProductFeatureEntity
    {
        private string _Name;
        private Int32 _StoreID;
        private int _FeatureId;
        private bool _Active;
        private string _StoreName;

        public string Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        public int FeatureId
        {
            get { return _FeatureId; }
            set { _FeatureId = value; }
        }

        public Int32 StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }
        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }
        public bool Active
        {
            get { return _Active; }
            set { _Active = value; }
        }
    }

    public class ProductsComponentEntity
    {
        private int _RatingID;

        public int RatingID
        {
            get { return _RatingID; }
            set { _RatingID = value; }
        }

        private int _StoreID;

        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }

        private int _ProductID;

        public int ProductID
        {
            get { return _ProductID; }
            set { _ProductID = value; }
        }
        private int _CustomerID;

        public int CustomerID
        {
            get { return _CustomerID; }
            set { _CustomerID = value; }
        }

        private int _Rating;

        public int Rating
        {
            get { return _Rating; }
            set { _Rating = value; }
        }

        private string _Comments;

        public string Comments
        {
            get { return _Comments; }
            set { _Comments = value; }
        }
        private string _CustomerName;

        public string CustomerName
        {
            get { return _CustomerName; }
            set { _CustomerName = value; }
        }
        private string _StoreName;

        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }

        private string _ProductName;

        public string ProductName
        {
            get { return _ProductName; }
            set { _ProductName = value; }
        }

        private int _IsApproved;

        public int IsApproved
        {
            get { return _IsApproved; }
            set { _IsApproved = value; }
        }

        private DateTime _ApprovedDate;

        public DateTime ApprovedDate
        {
            get { return _ApprovedDate; }
            set { _ApprovedDate = value; }
        }

        private DateTime _CreatedOn;

        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
    }
}
