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
using System.Collections;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Category Component Class Contains Category related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CategoryComponent
    {
        #region Declaration

        private static int _count;
        RedTag_CCTV_Ecomm_DBEntities ctxRedTag = new RedTag_CCTV_Ecomm_DBEntities();

        #region List Class for StateList
        /// <summary>
        /// Created dynamic table for Listing State Data
        /// </summary>
        public class CategoryTable
        {
            public int _CategoryID;
            public int _StoreID;
            public string _Name;
            public DateTime _CreatedOn;
            public int _DisplayOrder;
            public bool _Active;
            public int _CategoryMappingID;
            public int _ParentCategoryID;
            public bool _IsFeatured;
            public string _UpdatedBy;
            public DateTime? _UpdatedOn;
            public string _ParentCategoryName;

            public string UpdatedBy
            {
                get { return _UpdatedBy; }
                set { _UpdatedBy = value; }
            }

            public string ParentCategoryName
            {
                get { return _ParentCategoryName; }
                set { _ParentCategoryName = value; }
            }

            public DateTime? UpdatedOn
            {
                get { return _UpdatedOn; }
                set { _UpdatedOn = value; }
            }

            public int CategoryID
            {
                get { return _CategoryID; }
                set { _CategoryID = value; }
            }
            public int StoreID
            {
                get { return _StoreID; }
                set { _StoreID = value; }
            }
            public string Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
            public int DisplayOrder
            {
                get { return _DisplayOrder; }
                set { _DisplayOrder = value; }
            }
            public bool Active
            {
                get { return _Active; }
                set { _Active = value; }
            }
            public DateTime CreatedOn
            {
                get { return _CreatedOn; }
                set { _CreatedOn = value; }
            }
            public int CategoryMappingID
            {
                get { return _CategoryMappingID; }
                set { _CategoryMappingID = value; }
            }
            public int ParentCategoryID
            {
                get { return _ParentCategoryID; }
                set { _ParentCategoryID = value; }
            }
            public bool IsFeatured
            {
                get { return _IsFeatured; }
                set { _IsFeatured = value; }
            }


        }
        #endregion

        #endregion

        #region Properties

        private static bool _newFilter = false;
        private static string _Filter = "";
        private static int _CategoryID = 0;

        public static bool NewFilter
        {
            get { return _newFilter; }
            set { _newFilter = value; }
        }

        public static string Filter
        {
            get { return _Filter; }
            set { _Filter = value; }
        }

        public static int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Category Details by CategoryID
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>
        /// <returns>Returns Category Details</returns>
        public tb_Category GetCategoryByCategoryID(int CategoryID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetCategoryByCategoryID(CategoryID);
        }

        /// <summary>
        /// Search Category for Edit Category Form
        /// </summary>
        /// <param name="CategoryName">String CategoryName</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category List as a DataSet</returns>
        public static DataSet SearchCategory(String CategoryName, Int32 Mode, Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.SearchCategory(CategoryName, Mode, StoreID);
        }

        /// <summary>
        /// Expanded Category for Edit Category Form
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category List as a DataSet</returns>
        public static DataSet ExpandedCategory(Int32 CategoryID, Int32 Mode, Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.ExpandedCategory(CategoryID, Mode, StoreID);
        }

        /// <summary>
        /// Method for Save data in Category Mapping
        /// </summary>
        /// <param name="categoryMapping">tb_CategoryMapping categoryMapping</param>
        /// <returns>Returns tb_CategoryMapping table</returns>
        public tb_CategoryMapping CreateCategorymapping(tb_CategoryMapping categoryMapping)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.createCategoryMapping(categoryMapping);
        }

        /// <summary>
        /// Method for update data in Category Mapping
        /// </summary>
        /// <param name="categoryMapping">tb_CategoryMapping categorymapping</param>
        /// <returns>Returns tb_CategoryMapping table</returns>
        public tb_CategoryMapping updateCategory(tb_CategoryMapping categorymapping)
        {
            CategoryDAC dac = new CategoryDAC();
            dac.update(categorymapping);
            return categorymapping;
        }

        /// <summary>
        /// Update Category
        /// </summary>
        /// <param name="CategoryId">Int32 CategoryId</param>
        /// <param name="ParentCategoryId">Int32 ParentCategoryId</param>
        /// <param name="CategoryMappingID">Int32 CategoryMappingID</param>
        public void Update(Int32 CategoryId, Int32 ParentCategoryId, Int32 CategoryMappingID)
        {
            CategoryDAC dac = new CategoryDAC();
            dac.Update(CategoryId, ParentCategoryId, CategoryMappingID);
        }

        /// <summary>
        /// Delete Category Mapping using CategoryMappingID
        /// </summary>
        /// <param name="CategoryMappingID">Int32 CategoryMappingID</param>
        public void delete(Int32 CategoryMappingID)
        {
            CategoryDAC dac = new CategoryDAC();
            dac.delete(CategoryMappingID);
        }

        /// <summary>
        /// Method for get category by Store Id
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category List</returns>
        public static DataSet GetCategoryByStoreID(Int32 StoreID)
        {
            DataSet DSCategory = new DataSet();
            CategoryDAC dac = new CategoryDAC();
            DSCategory = dac.GetCategoryByStoreID(StoreID);
            return DSCategory;
        }

        /// <summary>
        /// Method for get category by Store Id
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="Option">Int32 Option</param>
        /// <returns>Returns Category List</returns>
        public static DataSet GetCategoryByStoreID(Int32 StoreID, Int32 Option)
        {
            DataSet DSCategory = new DataSet();
            CategoryDAC dac = new CategoryDAC();
            DSCategory = dac.GetCategoryByStoreID(StoreID, Option);
            return DSCategory;
        }

        /// <summary>
        /// Method to get Parent Category By Store ID
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Parent Category By StoreId</returns>
        public DataSet GetParentCategoryByStoreId(Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            DataSet DSParentCategory = new DataSet();
            DSParentCategory = dac.GetParentCategoryByStoreId(StoreID);
            return DSParentCategory;
        }

        /// <summary>
        /// Method for Save data in Category 
        /// </summary>
        /// <param name="categoryMapping">tb_Category category</param>
        /// <returns>Returns tb_Category table</returns>
        public Int32 CreateCategory(tb_Category category)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.createCategory(category).CategoryID;
        }

        /// <summary>
        /// Method to get category detail by category name
        /// </summary>
        /// <param name="name">tb_Category name</param>
        /// <returns>Returns tb_Category table </returns>
        public List<tb_Category> GetCategory(tb_Category name)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetCategory(name);
        }

        /// <summary>
        ///  Method for get all category detail
        /// </summary>
        /// <returns>Returns filtered tb_Category table</returns>
        public List<tb_Category> GetCategory()
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetCategory();
        }

        /// <summary>
        /// Method for category detail by store Id
        /// </summary>
        /// <param name="storeId">Int32 StoreID</param>
        /// <returns>Returns Category Details</returns>
        public DataSet getCategoryDetailsbyStoreId(Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.getCategorydetails(StoreID);
        }

        /// <summary>
        /// Method for all Category detail of all store
        /// </summary>
        /// <returns>Returns Category Details using all Stores</returns>
        public DataSet getCategorydetaisbyallstore()
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.getCategorydetailsallstore();
        }

        /// <summary>
        /// Method for get category detail by Name
        /// </summary>
        /// <param name="Name">string Name</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category Dataset</returns>
        public DataSet getcategorydetailsbyname(string Name, Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.getCategorydetails(Name, StoreID);
        }

        /// <summary>
        /// Method for get category detail by Name
        /// </summary>
        /// <param name="Name">string Name</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Category Dataset</returns>
        public DataSet getcategorydetailsbynameforsearch(string Name, Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.getCategorydetailsforsearch(Name, StoreID);
        }

        /// <summary>
        /// Method for get category by parent category name
        /// </summary>
        /// <param name="parentCategoryName">string parentCategoryName</param>
        /// <returns>Returns Category Dataset</returns>

        public DataSet getCategorydetailsbyparentcategoryname(string parentCategoryName)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.getCategorydetailsbyparentcategoryname(parentCategoryName);
        }

        /// <summary>
        /// Method for delete Category 
        /// </summary>
        /// <param name="categoryId">Int32 CategoryId</param>
        public void Deletecategory(Int32 categoryId)
        {
            CategoryDAC dac = new CategoryDAC();
            dac.Deletecategory(categoryId);
        }

        /// <summary>
        ///  Method for get category detail by store id and category id
        /// </summary>
        /// <param name="categoryId">Int32 categoryId</param>
        /// <param name="storeId">Int32 storeId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet getCatdetailbycatid(Int32 categoryId, Int32 storeId)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.getCatdetailbycatid(categoryId, storeId);
        }

        /// <summary>
        /// Method for update category
        /// </summary>
        /// <param name="category">tb_Category category</param>
        /// <returns>Returns tb_Category category Update Status</returns>
        public tb_Category updateCategory(tb_Category category)
        {
            CategoryDAC dac = new CategoryDAC();
            dac.Update(category);
            return category;
        }

        /// <summary>
        /// Method for get data to display category by paging
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">int sortBy</param>
        /// <returns>Returns Category Data in IEnumerable </returns>
        public IEnumerable GetDatas(Int32 startIndex, Int32 pageSize, string sortBy)
        {
            var resultVar = (from cat in ctxRedTag.tb_Category.Include("tb_Store")
                             join catMap in ctxRedTag.tb_CategoryMapping on cat.CategoryID equals catMap.CategoryID
                             orderby cat.CategoryID
                             select new { cat, catMap, cat.tb_Store.StoreID }).Skip(startIndex).Take(pageSize).ToList();
            return resultVar;
        }

        /// <summary>
        /// Get All Categories With Search
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="FieldName">String FieldName</param>
        /// <param name="FieldValue">String FieldValue</param>
        /// <param name="Status">String Status</param>
        /// <returns>Returns List of All Category according to Search Option</returns>
        public static DataSet GetAllCategoriesWithsearch(Int32 StoreID, String FieldName, String FieldValue, String Status)
        {
            CategoryDAC dac = new CategoryDAC();
            DataSet dsCategoryLiost = new DataSet();
            dsCategoryLiost = dac.GetAllCategoriesWithsearch(StoreID, FieldName, FieldValue, Status);
            return dsCategoryLiost;
        }

        /// <summary>
        /// get Category details with filter option
        /// </summary>
        /// <param name="startIndex">int startIndex </param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="status">string status</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <returns>Returns Category Table</returns>

        public static List<CategoryTable> GetDataByFilter(Int32 startIndex, Int32 pageSize, string sortBy, string SearchValue, string status, int StoreID, string SearchBy)
        {
            int realIndex = (_newFilter ? 0 : startIndex);
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            List<CategoryTable> objCategory = new List<CategoryTable>();
            bool IsActive = false;
            if (status == "Active")
                IsActive = true;
            else
                IsActive = false;

            if (string.IsNullOrEmpty(Filter))
            {
                Filter = "";
            }
            else
            {
                Filter = Filter.Trim();
            }
            if (SearchBy == "Name" || SearchBy == "")
            {
                objCategory = (from categoryList in ctx.tb_Category
                               join admin in ctx.tb_Admin on categoryList.UpdatedBy equals admin.AdminID into joined
                               from joinedData in joined.DefaultIfEmpty()
                               where categoryList.Name.Contains(Filter)
                               && categoryList.Active == IsActive
                                 && ((System.Boolean?)categoryList.Deleted ?? false) == false

                               select new CategoryTable
                               {
                                   CategoryID = categoryList.CategoryID,
                                   Name = categoryList.Name,
                                   UpdatedBy = joinedData.FirstName + " " + joinedData.LastName,
                                   UpdatedOn = categoryList.UpdatedOn.Value,
                                   Active = categoryList.Active ?? false,
                                   DisplayOrder = categoryList.DisplayOrder ?? 999,
                                   CreatedOn = categoryList.CreatedOn ?? DateTime.Now,
                                   StoreID = categoryList.tb_Store.StoreID
                               }).OrderBy(c => c.Name).Skip(startIndex).Take(pageSize).ToList<CategoryTable>();
            }
            if (SearchBy == "ParentCatName")
            {
                objCategory = (from parentlist in ctx.tb_Category
                               join mapping in ctx.tb_CategoryMapping on
                                   parentlist.CategoryID equals mapping.CategoryID
                               join admin in ctx.tb_Admin on parentlist.UpdatedBy equals admin.AdminID into joined
                               from joinedData in joined.DefaultIfEmpty()
                               where parentlist.Name.Contains(Filter) &&
                                          mapping.ParentCategoryID == 0 && ((System.Boolean?)parentlist.Deleted ?? false) == false

                               select new CategoryTable
                               {
                                   CategoryID = parentlist.CategoryID,
                                   Name = parentlist.Name,
                                   UpdatedBy = joinedData.FirstName + " " + joinedData.LastName,
                                   UpdatedOn = parentlist.UpdatedOn.Value,
                                   Active = parentlist.Active ?? false,
                                   DisplayOrder = parentlist.DisplayOrder ?? 999,
                                   CreatedOn = parentlist.CreatedOn ?? DateTime.Now,
                                   StoreID = parentlist.tb_Store.StoreID
                               }).OrderBy(c => c.Name).Skip(startIndex).Take(pageSize).Distinct().ToList<CategoryTable>();
            }

            if (StoreID != -1)
            {
                objCategory = objCategory.Where(c => c.StoreID == StoreID).ToList();
            }
            _count = objCategory.Count();

            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = objCategory.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

                String[] SortingOption = sortBy.Split(" ".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (SortingOption.Length == 1)
                {
                    objCategory = objCategory.OrderBy(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<CategoryTable>();
                }
                else if (SortingOption.Length == 2)
                {
                    objCategory = objCategory.OrderByDescending(e => GetPropertyValue(e, SortingOption[0].ToString())).ToList<CategoryTable>();
                }
            }
            return objCategory;

        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj"> object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns Object</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Get Total Count
        /// </summary>
        /// <param name="status">string status</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <returns>Returns Total Count</returns>

        public static int GetCount(string status, int StoreID, string SearchValue, string SearchBy)
        {
            return _count;
        }

        /// <summary>
        /// Method to check record is duplicate or not
        /// </summary>
        /// <param name="category">tb_Category category</param>
        /// <returns>Returns value of Duplicate category Exists or Not</returns>
        public Int32 checkduplicate(tb_Category category)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.checkduplicate(category);
        }

        /// <summary>
        /// Method to Get Category By Category Name
        /// </summary>
        /// <param name="category">tb_Category Category</param>
        /// <returns>Returns Category Data in IEnumerable</returns>
        public IEnumerable GetCategorybyname(tb_Category category)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetCategorybyname(category);
        }

        /// <summary>
        /// Method to display top 6 featured category on Index Page
        /// </summary>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Top 6 featured category For Index Page</returns>
        public static DataSet GetFeaturedCategory(Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetFeaturedCategory(StoreID);
        }

        /// <summary>
        /// Method to Subcategory Detail By Category and StoreID
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Subcategory Details</returns>
        public static DataSet GetSubCategoryByCategoryId(Int32 CategoryId, Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetSubCategoryByMainCategory(CategoryId, StoreID);
        }

        /// <summary>
        /// Method to get parent category name by category id
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>        
        /// <returns>Returns ParentcategoryName</returns>
        public static DataSet GetParentCategoryNamebyCategoryID(Int32 CategoryID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetParentCategoryNamebyCategoryID(CategoryID);
        }

        /// <summary>
        /// Method to get Category Detail With Parent Category ID
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <returns>Returns Category Detail With ParentCategoryID</returns>
        public static DataSet GetCategoryDetailBycategoryIdWithParentCategoryID(Int32 CategoryID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetCategoryDetailBycategoryIdWithParentCategoryID(CategoryID);
        }

        /// <summary>
        /// Method to get Category Detail With Parent CategoryID
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <returns>Returns Category Detail With ParentCategoryID</returns>
        public static DataSet GetCategoryDetailBycategoryIdWithParentID(Int32 CategoryID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetCategoryDetailBycategoryIdWithParentID(CategoryID);
        }

        /// <summary>
        /// Update Category Feature
        /// </summary>
        /// <param name="CategoryID">Int32 CategoryID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <param name="IsFeatured">bool IsFeatured</param>
        public static void UpdateCategoryFeature(Int32 CategoryID, Int32 StoreID, bool IsFeatured)
        {
            CategoryDAC dac = new CategoryDAC();
            dac.UpdateCategoryFeature(CategoryID, StoreID, IsFeatured);
        }

        /// <summary>
        /// Update Category Display Order
        /// </summary>
        /// <param name="CategoryId">Int32 CategoryId</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="DisplayOrder">Int32 DisplayOrder</param>
        public static void UpdateCategoryDisplayOrder(Int32 CategoryId, Int32 StoreId, Int32 DisplayOrder)
        {
            CategoryDAC dac = new CategoryDAC();
            dac.UpdateCategoryDisplayOrder(CategoryId, StoreId, DisplayOrder);
        }

        /// <summary>
        /// Get Category SEName By ProductID
        /// </summary>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <returns>Returns Category SEName</returns>
        public string GetCategorySENameByProductID(Int32 ProductID)
        {
            CategoryDAC dac = new CategoryDAC();
            return Convert.ToString(dac.GetCategorySENameByProductID(ProductID));
        }

        #endregion

        /// <summary>
        /// Method to get category detail by ProductID
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        /// <param name="storeId">Int32 StoreId</param>
        /// <returns>Returns Category Details</returns>
        public DataSet getcategorydetailsbyProductID(Int32 ProductID, Int32 StoreID)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.getCategorydetailsbyProduct(ProductID, StoreID);
        }

        /// <summary>
        /// Delete Product Wise Category(s) 
        /// </summary>
        /// <param name="ProductId">Int32 ProductId</param>
        public static void DeleteProductCategory(Int32 ProductId)
        {
            CategoryDAC dac = new CategoryDAC();
            dac.DeleteCategoryforProduct(ProductId);
        }

        /// <summary>
        /// Get Category For Searching
        /// </summary>
        /// <param name="SearchTerm">String SearchTerm</param>
        /// <param name="StrCategory">String StrCategory</param>
        /// <param name="SearchInDescription">bool SearchInDescription</param>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Category from Searching</returns>
        public DataSet GetCategoryForSearch(String SearchTerm, String StrCategory, bool SearchInDescription, Int32 StoreId)
        {
            CategoryDAC dac = new CategoryDAC();
            return dac.GetCategoryForSearchData(SearchTerm, StrCategory, SearchInDescription, StoreId);
        }

        /// <summary>
        /// Get All Price Range
        /// </summary>
        /// <param name="storeID">string storeID</param>
        /// <returns>Returns All Price Range</returns>
        public DataSet GetAllPriceRange(string storeID)
        {
            CategoryDAC objCategory = new CategoryDAC();
            DataSet dspricerange = new DataSet();
            dspricerange = objCategory.GetAllPriceRange(storeID);
            return dspricerange;
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
            CategoryDAC objCategory = new CategoryDAC();
            return objCategory.CheckProductforPriceRange(MinPrice, MaxPrice, storeID);
        }
        public DataSet GetSearchCategoryValue(Int32 StoreId, String Whrclus, int Mode)
        {
            CategoryDAC objCategory = new CategoryDAC();
            return objCategory.GetSearchCategoryValue(StoreId, Whrclus, Mode);
        }
    }
}
