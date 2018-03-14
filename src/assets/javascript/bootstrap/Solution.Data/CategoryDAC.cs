using System.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using System.Collections;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// Category Data Access Class Contains Category related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CategoryDAC
    {

        #region Declartion

        RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Category Details by CategoryID
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>
        /// <returns>Returns Category Details</returns>
        public tb_Category GetCategoryByCategoryID(int CategoryID)
        {
            tb_Category category = null;
            category = ctxRedtag.tb_Category.First(e => e.CategoryID == CategoryID);
            return category;
        }

        /// <summary>
        /// Method for get category by Store Id
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category List</returns>
        public DataSet GetCategoryByStoreID(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetCategoryByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@opt", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method for get category by storeID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Option">Int32 Option</param>
        /// <returns>Returns Category List</returns>

        public DataSet GetCategoryByStoreID(Int32 StoreID, Int32 Option)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetCategoryByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@opt", Option);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get Parent Category By Store ID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Parent Category By StoreId</returns>
        public DataSet GetParentCategoryByStoreId(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetCategoryByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@opt", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method for Save data in Category 
        /// </summary>
        /// <param name="categoryMapping">tb_Category category</param>
        /// <returns>Returns tb_Category table</returns>
        public tb_Category createCategory(tb_Category category)
        {
            ctxRedtag.AddTotb_Category(category);
            ctxRedtag.SaveChanges();
            return category;
        }

        /// <summary>
        /// Method to get category detail by category name
        /// </summary>
        /// <param name="category">tb_Category category</param>
        /// <returns>Returns tb_Category List </returns>
        public List<tb_Category> GetCategory(tb_Category category)
        {
            List<tb_Category> result = null;
            result = ctxRedtag.tb_Category.Where(cat => cat.Name.Equals(category.Name)).ToList();
            return result;
        }

        /// <summary>
        /// Method for get all category detail
        /// </summary>
        /// <returns>Returns filtered tb_Category table</returns>
        public List<tb_Category> GetCategory()
        {
            List<tb_Category> result = null;
            result = ctxRedtag.tb_Category.OrderBy(tb_Category => tb_Category.CategoryID).ToList();
            return result;
        }

        /// <summary>
        /// Method for category detail by store Id
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category Details</returns>
        public DataSet getCategorydetails(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_FeaturedCategory";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@opt", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get Category details by all store
        /// </summary>
        /// <returns>Returns Catgory Details using all Stores</returns>
        public DataSet getCategorydetailsallstore()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_getallCategorydetail";
            cmd.Parameters.AddWithValue("@mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get category detail by category name
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public DataSet getCategorydetails(string categoryName, Int32 storeId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllDetail";
            cmd.Parameters.AddWithValue("@Name", categoryName);
            cmd.Parameters.AddWithValue("@mode", 2);
            cmd.Parameters.AddWithValue("@StoreID", storeId);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get category detail by category name
        /// </summary>
        /// <param name="categoryName"></param>
        /// <returns></returns>
        public DataSet getCategorydetailsforsearch(string categoryName, Int32 storeId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllDetail";
            cmd.Parameters.AddWithValue("@Name", categoryName);
            cmd.Parameters.AddWithValue("@mode", 12);
            cmd.Parameters.AddWithValue("@StoreID", storeId);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get category detail by parent category name
        /// </summary>
        /// <param name="parentcategoryName"></param>
        /// <returns></returns>
        public DataSet getCategorydetailsbyparentcategoryname(string parentcategoryName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_getallCategorydetail";
            cmd.Parameters.AddWithValue("@Name", parentcategoryName);
            cmd.Parameters.AddWithValue("@mode", 4);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to delete Category
        /// </summary>
        /// <param name="categoryId">Int32 CategoryId</param>
        public void Deletecategory(Int32 categoryId)
        {
            int query = (from catmap in ctxRedtag.tb_CategoryMapping
                         where catmap.CategoryID == categoryId
                         select catmap).Count();
            if (query >= 1)
            {
                tb_CategoryMapping catMap = ctxRedtag.tb_CategoryMapping.FirstOrDefault(tb_CategoryMapping => tb_CategoryMapping.ParentCategoryID == categoryId || tb_CategoryMapping.CategoryID == categoryId);
                ctxRedtag.DeleteObject(catMap);
                ctxRedtag.SaveChanges();
            }

            tb_Category cat = ctxRedtag.tb_Category.FirstOrDefault(c => c.CategoryID == categoryId);
            ctxRedtag.DeleteObject(cat);
            ctxRedtag.SaveChanges();
        }

        /// <summary>
        ///  Method for get category detail by store id and categoryid
        /// </summary>
        /// <param name="categoryId">Int32 categoryId</param>
        /// <param name="storeId">Int32 storeId</param>
        /// <returns>Returns Category Details</returns>
        public DataSet getCatdetailbycatid(Int32 categoryId, Int32 storeId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllDetail";
            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
            cmd.Parameters.AddWithValue("@StoreID", storeId);
            cmd.Parameters.AddWithValue("@mode", 5);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method for update category
        /// </summary>
        /// <param name="category">tb_Category category</param>
        /// <returns>Returns tb_Category category update</returns>
        public void Update(tb_Category category)
        {
            ctxRedtag.tb_Category.Single(tb_Category => tb_Category.CategoryID == category.CategoryID);
            //ctxRedtag.Attach(ctxRedtag.tb_Category.Single(tb_Category => tb_Category.CategoryID == category.CategoryID));
            ctxRedtag.ApplyPropertyChanges("tb_Category", category);
            ctxRedtag.SaveChanges();
        }

        /// <summary>
        /// Method to check record is duplicate or not
        /// </summary>
        /// <param name="category">tb_Category category</param>
        /// <returns>Returns value of Duplicate category Exist or Not</returns>
        public Int32 checkduplicate(tb_Category category)
        {
            Int32 isExist = 0;
            isExist = (from cat in ctxRedtag.tb_Category
                       where cat.Name.ToUpper().Trim() == category.Name.ToUpper().Trim()
                       select cat
                         ).Count();
            return isExist;
        }

        /// <summary>
        /// Method to Get Category By Category Name
        /// </summary>
        /// <param name="category">tb_Category Category</param>
        /// <returns>Returns Category Data in IEnumerable</returns>
        public IEnumerable GetCategorybyname(tb_Category category)
        {
            var resultVar = (from cat in ctxRedtag.tb_Category.Include("tb_Store")
                             join catMap in ctxRedtag.tb_CategoryMapping on cat.CategoryID equals catMap.CategoryID
                             orderby cat.CategoryID
                             select new { cat, catMap, cat.tb_Store.StoreID });
            return resultVar;
        }

        /// <summary>
        /// Method for Save data in Category Mapping
        /// </summary>
        /// <param name="categoryMapping">tb_CategoryMapping categoryMapping</param>
        /// <returns>Returns tb_CategoryMapping table</returns>
        public tb_CategoryMapping createCategoryMapping(tb_CategoryMapping categoryMapping)
        {
            ctxRedtag.AddTotb_CategoryMapping(categoryMapping);
            ctxRedtag.SaveChanges();
            return categoryMapping;
        }

        /// <summary>
        /// Method to update category mapping
        /// </summary>
        /// <param name="categoryMapping">tb_CategoryMapping categoryMapping</param>
        /// <returns>Returns tb_CategoryMapping table</returns>
        public void update(tb_CategoryMapping categoryMapping)
        {
            ctxRedtag.tb_CategoryMapping.FirstOrDefault(tb_CategoryMapping => tb_CategoryMapping.CategoryID == categoryMapping.CategoryID);
            ctxRedtag.ApplyPropertyChanges("tb_CategoryMapping", categoryMapping);
            ctxRedtag.SaveChanges();
        }

        /// <summary>
        /// Update Category Mapping
        /// </summary>
        /// <param name="CategoryId">Int32 categoryId</param>
        /// <param name="ParentCategoryId">Int32 parentCategoryId</param>
        /// <param name="CategoryMappingID">Int32 CategoryMappingID</param>
        public void Update(Int32 categoryId, Int32 parentCategoryId, Int32 CategoryMappingID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CategoryMapping_Update";
            cmd.Parameters.AddWithValue("@CategoryID", categoryId);
            cmd.Parameters.AddWithValue("@ParentCategoryID", parentCategoryId);
            cmd.Parameters.AddWithValue("@CategoryMappingID", CategoryMappingID);
            cmd.Parameters.AddWithValue("@opt", 1);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Delete Category Mapping using CategoryMappingID
        /// </summary>
        /// <param name="CategoryMappingID">Int32 CategoryMappingID</param>
        public void delete(Int32 CategoryMappingID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CategoryMapping_Update";
            cmd.Parameters.AddWithValue("@CategoryMappingID", CategoryMappingID);
            cmd.Parameters.AddWithValue("@opt", 2);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Method to display top 6 featured category on Index Page
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Top 6 featured category For Index Page</returns>
        public DataSet GetFeaturedCategory(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_FeaturedCategory";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Opt", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to Subcategory Detail By Category and StoreID
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Subcategory Details</returns>
        public DataSet GetSubCategoryByMainCategory(Int32 CategoryID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllDetail";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@mode", 6);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get parent category name by category id
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>        
        /// <returns>Returns ParentcategoryName</returns>
        public DataSet GetParentCategoryNamebyCategoryID(Int32 CategoryID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllDetail";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@mode", 7);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get Category Detail With Parent Category ID
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <returns>Returns Category Detail With ParentCategoryID</returns>
        public DataSet GetCategoryDetailBycategoryIdWithParentCategoryID(Int32 CategoryID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllDetail";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@mode", 8);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to get Category Detail With Parent CategoryID
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <returns>Returns Category Detail With ParentCategoryID</returns>
        public DataSet GetCategoryDetailBycategoryIdWithParentID(Int32 CategoryID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllDetail";
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@mode", 11);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Category Feature
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="IsFeatured">bool IsFeatured</param>
        public void UpdateCategoryFeature(Int32 CategoryID, Int32 StoreID, bool IsFeatured)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "usp_Category_FeaturedCategory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@IsFeatured", IsFeatured);
            cmd.Parameters.AddWithValue("@opt", 2);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Update Category Display Order
        /// </summary>
        /// <param name="CategoryId">Int32 CategoryId</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        public void UpdateCategoryDisplayOrder(Int32 CategoryId, Int32 StoreId, Int32 DisplayOrder)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "usp_Category_FeaturedCategory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@StoreID", StoreId);
            cmd.Parameters.AddWithValue("@CategoryID", CategoryId);
            cmd.Parameters.AddWithValue("@DisplayOrder", DisplayOrder);
            cmd.Parameters.AddWithValue("@opt", 4);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Get Category SEName By ProductID
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <returns>Returns Category SEName</returns>
        public string GetCategorySENameByProductID(Int32 ProductID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetCategorySENameByProductID";
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        #endregion

        /// <summary>
        /// Method to get category detail by ProductID
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="storeId">Int32 StoreId</param>
        /// <returns>Returns Category Details</returns>
        public DataSet getCategorydetailsbyProduct(Int32 ProductId, Int32 storeId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllDetail";
            cmd.Parameters.AddWithValue("@ProductID", ProductId);
            cmd.Parameters.AddWithValue("@mode", 9);
            cmd.Parameters.AddWithValue("@StoreID", storeId);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Categories With Search
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="FieldName">String FieldName</param>
        /// <param name="FieldValue">String FieldValue</param>
        /// <returns>Returns List of All Category according to Search Option</returns>
        public DataSet GetAllCategoriesWithsearch(Int32 StoreID, String FieldName, String FieldValue, String Status)
        {
            String strCondition = "";
            bool IsActive = true;
            if (StoreID > 0)
            {
                strCondition += "  AND t1.StoreID=" + StoreID;
            }
            if (FieldName == "ParentCatName" && FieldValue != "")
            {
                strCondition += " AND t1.ParentName LIKE '%" + FieldValue + "%'";
            }
            else if (FieldName == "Name" && FieldValue != "")
            {
                strCondition += " AND t1.Name LIKE '%" + FieldValue + "%'";
            }
            if (Status == "InActive")
            {
                IsActive = false;
            }
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Category_GetAllCategoryWithSearch";
            cmd.Parameters.AddWithValue("@Condition", strCondition);
            cmd.Parameters.AddWithValue("@IsActive", IsActive);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Delete Product Wise Category(s) 
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        public void DeleteCategoryforProduct(Int32 ProductId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "usp_DeleteCategory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ProductId", ProductId);
            objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Search Category for Edit Category Form
        /// </summary>
        /// <param name="CategoryName">String CategoryName</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category List as a DataSet</returns>
        public DataSet SearchCategory(String CategoryName, Int32 Mode, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "usp_Category_CategoryTreeList";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CatName", CategoryName);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Expanded Category for Edit Category Form
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category List as a DataSet</returns>
        public DataSet ExpandedCategory(Int32 CategoryID, Int32 Mode, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "usp_Category_ExpandSelectedCategory";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Category For Searching
        /// </summary>
        /// <param name="SearchTerm">String SearchTerm</param>
        /// <param name="StrCategory">String StrCategory</param>
        /// <param name="SearchInDescription">bool SearchInDescription</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Category from Searching</returns>

        public DataSet GetCategoryForSearchData(String SearchTerm, String StrCategory, bool SearchInDescription, Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_SearchCategory";
            if (!String.IsNullOrEmpty(StrCategory.ToString()))
            {
                StrCategory = " and categoryid in (" + StrCategory + ")";
            }
            string[] Words = new string[5];
            Words = SearchTerm.Split(" ".ToCharArray(), 5, StringSplitOptions.RemoveEmptyEntries);
            //--- Search in Category Name
            string SearchValue = " And (";
            foreach (String SearchTerm1 in SearchTerm.Trim().Split('+'))
            {
                foreach (String StrWords in SearchTerm1.Trim().Split(' '))
                {
                    if (StrWords.Trim().Length > 0 && StrWords.Trim() != "&" && StrWords.Trim() != "+")
                    {
                        SearchValue += "Name like '%" + StrWords + "%' or ";
                    }
                }
            }
            SearchValue = SearchValue.Substring(0, SearchValue.Length - 3);
            SearchValue += ")";
            //--- Search in Category Description
            string SearchInDesc = " or (";
            if (SearchInDescription == true)
            {
                foreach (String SearchTerm1 in SearchTerm.Trim().Split('+'))
                {
                    foreach (String StrWords in SearchTerm1.Trim().Split(' '))
                    {
                        if (StrWords.Trim().Length > 0 && StrWords.Trim() != "&" && StrWords.Trim() != "+")
                        {
                            SearchInDesc += "Description like '%" + StrWords + "%' or ";
                        }
                    }
                }
            }
            SearchInDesc = SearchInDesc.Substring(0, SearchInDesc.Length - 3);
            SearchInDesc += ")";
            // ---- End Desc
            //--- Search in Category Summary
            string SearchSummary = " or (";
            if (SearchInDescription == true)
            {
                foreach (String SearchTerm1 in SearchTerm.Trim().Split('+'))
                {
                    foreach (String StrWords in SearchTerm1.Trim().Split(' '))
                    {
                        if (StrWords.Trim().Length > 0 && StrWords.Trim() != "&" && StrWords.Trim() != "+")
                        {
                            SearchSummary += "Description like '%" + StrWords + "%' or ";
                        }
                    }
                }
            }
            SearchSummary = SearchSummary.Substring(0, SearchSummary.Length - 3);
            SearchSummary += ")";
            // ---- End Summary
            cmd.Parameters.AddWithValue("@SearchTerm", SearchValue);
            cmd.Parameters.AddWithValue("@CategoryId", StrCategory);
            cmd.Parameters.AddWithValue("@SearchInDescription", SearchInDescription);
            cmd.Parameters.AddWithValue("@Description", SearchInDesc);
            cmd.Parameters.AddWithValue("@StoreId", StoreId.ToString());
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Price Range
        /// </summary>
        /// <param name="storeID">string storeID</param>
        /// <returns>Returns All Price Range</returns>
        public DataSet GetAllPriceRange(string storeID)
        {
            String Query = "select PriceRangeId,PriceRangeName,MinPrice,MaxPrice,SEName from tb_PriceRange where Deleted=0 and status=1 and Storeid = " + storeID + "order by minprice";
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = Query;
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Check Product for Price Range
        /// </summary>
        /// <param name="MinPrice">string MinPrice</param>
        /// <param name="MaxPrice">string MaxPrice</param>
        /// <param name="storeID">string storeID</param>
        /// <returns>Returns Count of Product for Price Range</returns>
        public Object CheckProductforPriceRange(string MinPrice, string MaxPrice, string storeID)
        {
            String Query = "select count(Productid) from tb_Product where Deleted=0 and storeid=" + storeID + " and Active=1 and (Case When (SalePrice Is Not Null And SalePrice!=0) Then SalePrice Else Price End) between " + MinPrice + " and " + MaxPrice;
            objSql = new SQLAccess();
            return objSql.ExecuteScalarQuery(Query);
        }
        public DataSet GetSearchCategoryValue(Int32 StoreIdId, String Whrclus, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_CategorySearchforPopupCategory";
            cmd.Parameters.AddWithValue("@StoreId", StoreIdId);
            cmd.Parameters.AddWithValue("@WhrClus", Whrclus);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }
    }
}
