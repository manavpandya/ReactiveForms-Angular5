using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;

namespace Solution.Bussines.Components
{

    /// <summary>
    /// Header Component Class Contains Header related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class HeaderlinkComponent
    {
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        private static int _count;
        public static bool _afterDelete = false;
        HeaderLinkDAC headerdac = new HeaderLinkDAC();
        List<HeaderlinkComponentEntity> lstheader = new List<HeaderlinkComponentEntity>();

        #region Key Functions

        /// <summary>
        /// Get Header Links
        /// </summary>
        /// <param name="Type">string Type</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Return Dataset - Header List</returns>

        public static DataSet GetHeaderLinks(string Type, Int32 StoreID)
        {
            HeaderLinkDAC dac = new HeaderLinkDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetHeaderLinks(Type, StoreID);
            return DSCommon;
        }

        /// <summary>
        /// Get the Data source for Grid view after searching and on first time load
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">Store id</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns IQueryable</returns>      
        public IQueryable<HeaderlinkComponentEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchValue)
        {
            IQueryable<HeaderlinkComponentEntity> results = from a in ctx.tb_HeaderLinks
                                                            select new HeaderlinkComponentEntity
                                                            {
                                                                PageId = a.PageId,
                                                                PageName = a.PageName,
                                                                PageURL = a.PageURL,
                                                                Type = a.Type,
                                                                DisplayOrder = a.DisplayOrder.Value,
                                                                StoreName = a.tb_Store.StoreName,
                                                                StoreId = a.tb_Store.StoreID
                                                            };
            if (pSearchValue != null)
            {
                if (pStoreId != 0)
                {
                    results = results.Where(a => a.PageName.Contains(pSearchValue.Trim()) && a.StoreId == pStoreId).AsQueryable();
                }
                else if (pStoreId == 0)
                {
                    results = results.Where(a => a.PageName.Contains(pSearchValue.Trim())).AsQueryable();
                }
            }
            else
            {
                pSearchValue = "";
                if (pStoreId != 0)
                {
                    results = results.Where(a => a.PageName.Contains(pSearchValue) && a.StoreId == pStoreId).AsQueryable();
                }
                else if (pStoreId == 0)
                {
                    results = results.OrderBy(o => o.PageName);
                }
            }
            _count = results.Count();
            results = results.OrderBy(o => o.DisplayOrder).ThenBy(o => o.StoreName);
            results = results.Skip(startIndex).Take(pageSize).AsQueryable();
            return results;
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj">object obj</param>
        /// <param name="property">string property</param>
        /// <returns>Returns object</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Get Total Count
        /// </summary>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>Returns Total Record Count</returns>
        public static int GetCount(string CName, int pStoreId, string pSearchValue)
        {
            return _count;
        }

        /// <summary>
        /// Deletes record from tb_headerlink table
        /// </summary>
        /// <param name="ID">page Id</param>
        public void DeleteHeaderLink(Int32 ID)
        {
            headerdac.DeleteHeaderLink(ID);
        }

        /// <summary>
        /// Get all values for specific Header Link
        /// </summary>
        /// <param name="Id">page id</param>
        /// <returns>table tb_HeaderLinks</returns>
        public tb_HeaderLinks GetHeaderLink(int Id)
        {
            return headerdac.GetHeaderLink(Id);
        }

        /// <summary>
        /// Function for updating header link
        /// </summary>
        /// <param name="tb_header">tb_HeaderLinks tb_header</param>
        /// <returns>Returns Int32</returns>
        public Int32 UpdateHeaderLink(tb_HeaderLinks tb_header)
        {
            Int32 isUpdated = 0;
            try
            {
                headerdac.UpdateHeaderLink(tb_header);
                isUpdated = tb_header.PageId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isUpdated;
        }

        /// <summary>
        /// Function for checking duplicate header link
        /// </summary>
        /// <param name="tb_header">tb_HeaderLinks tb_header</param>
        /// <returns>Returns Value True or False</returns>
        public bool CheckDuplicateforHeaderLink(tb_HeaderLinks tb_header)
        {
            if (headerdac.CheckDuplicateforHeaderLink(tb_header) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Creates VendorSKU
        /// </summary>
        /// <param name="tb_vendor">table tb_vendor</param>
        /// <returns>Returns Int32</returns>
        public Int32 CreateHeaderLink(tb_HeaderLinks tb_header)
        {
            Int32 isAdded = 0;
            try
            {
                headerdac.CreateHeaderLink(tb_header);
                isAdded = tb_header.PageId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }
        #endregion
    }

    /// <summary>
    /// Variable and Properties 
    /// </summary>

    public class HeaderlinkComponentEntity
    {
        private int _PageId;

        public int PageId
        {
            get { return _PageId; }
            set { _PageId = value; }
        }

        private string _PageName;

        public string PageName
        {
            get { return _PageName; }
            set { _PageName = value; }
        }

        private string _PageURL;

        public string PageURL
        {
            get { return _PageURL; }
            set { _PageURL = value; }
        }

        private string _Discription;

        public string Discription
        {
            get { return _Discription; }
            set { _Discription = value; }
        }

        private string _StoreName;

        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }

        private int _StoreId;

        public int StoreId
        {
            get { return _StoreId; }
            set { _StoreId = value; }
        }

        private string _Type;

        public string Type
        {
            get { return _Type; }
            set { _Type = value; }
        }
        public int _DisplayOrder;

        public int DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }

    }
}
