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
using Solution.Bussines.Components.Common;
using System.Collections;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// TaxClass Component Class Contains Tex Class related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class TaxClassComponent
    {

        #region Declarations

        List<TaxClassEntity> lsttaxcls = new List<TaxClassEntity>();
        TaxClassDAC dac = new TaxClassDAC();
        private static int _count;
        private static bool _newFilter = false;
        private static string _Filter = "";
        private static int _TaxClassID = 0;
        private static bool _SearchByStoreName;
        private static string _SearchStore;
        private static string _SearchBy;
        private Int32 _StateID;
        private Int32 _TaxClassIDForState;
        private Decimal _TaxRate;
        private static DataView _DataSourceState = null;
        private static DataView _DataSourceZipcode = null;
        private Int32 _ZipTaxID;
        private string _ZipCode;
        private Int32 _StoreID;
        private string _OldZipCode;
        public static bool _afterDelete = false;
        public static string SearchBy
        {
            get { return _SearchBy; }
            set { _SearchBy = value; }
        }
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
        public static bool AfterDelete
        {
            get { return _afterDelete; }
            set { _afterDelete = value; }
        }
        public static int TaxClassID
        {
            get { return _TaxClassID; }
            set { _TaxClassID = value; }
        }
        public static bool SearchByStoreName
        {
            get { return _SearchByStoreName; }
            set { _SearchByStoreName = value; }
        }
        public static string SearchStore
        {
            get { return _SearchStore; }
            set { _SearchStore = value; }
        }
        public Int32 StateID
        {
            get { return _StateID; }
            set { _StateID = value; }
        }
        public Int32 TaxClassIDForState
        {
            get { return _TaxClassIDForState; }
            set { _TaxClassIDForState = value; }
        }
        public Decimal TaxRate
        {
            get { return _TaxRate; }
            set { _TaxRate = value; }
        }
        public static DataView DataSourceState
        {
            get { return _DataSourceState; }
            set { _DataSourceState = value; }
        }
        public static DataView DataSourceZipCode
        {
            get { return _DataSourceZipcode; }
            set { _DataSourceZipcode = value; }
        }
        public Int32 ZipTaxID
        {
            get { return _ZipTaxID; }
            set { _ZipTaxID = value; }
        }
        public string ZipCode
        {
            get { return _ZipCode; }
            set { _ZipCode = value; }
        }
        public Int32 StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }
        public string OldZipCode
        {
            get { return _OldZipCode; }
            set { _OldZipCode = value; }
        }

        #endregion

        /// <summary>
        ///  Get Tax CLass For Getting Particular Record From Table by Id
        /// </summary>
        /// <param name="StoreID">int StoreId</param>
        /// <returns>Returns Tax Records as a Dataset</returns>
        public static DataSet GetTaxClassByStoreID(Int32 StoreID)
        {
            TaxClassDAC dac = new TaxClassDAC();
            DataSet DSTaxClass = new DataSet();
            DSTaxClass = dac.GetTaxClassByStoreID(StoreID);
            return DSTaxClass;
        }

        /// <summary>
        /// Get The Data Source for Grid view after sorting ,searching and On First Time Load
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="CName">string CName</param>
        /// <param name="pStoreId">int pStoreId</param>
        /// <param name="pSearchBy">string pSearchBy</param>
        /// <param name="pSearchValue">string pSearchValue</param>
        /// <returns>return List Non generic Collection</returns>
        public IQueryable<TaxClassEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, string CName, int pStoreId, string pSearchBy, string pSearchValue)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            IQueryable<TaxClassEntity> results = from a in ctx.tb_TaxClass
                                                 where a.Deleted.Value == false
                                                 select new TaxClassEntity
                                                 {
                                                     TaxClassID = a.TaxClassID,
                                                     TaxCode = a.TaxCode,
                                                     TaxName = a.TaxName,
                                                     Deleted = a.Deleted.Value,
                                                     StoreID = a.tb_Store.StoreID,
                                                     StoreName = a.tb_Store.StoreName,
                                                     CreatedOn = a.CreatedOn.Value,
                                                 };

            if (string.IsNullOrEmpty(pSearchValue))
            {
                pSearchValue = "";
            }
            if (pStoreId != -1)
            {
                if (pSearchBy == "TaxName" && !string.IsNullOrEmpty(pSearchValue))
                {
                    results = results.Where(a => a.TaxName.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId).AsQueryable();
                }
                else if (pSearchBy == "TaxCode" && !string.IsNullOrEmpty(pSearchValue))
                {
                    results = results.Where(a => a.TaxCode.Contains(pSearchValue.Trim()) && a.StoreID == pStoreId).AsQueryable();
                }
                else
                {
                    results = results.Where(a => a.StoreID == pStoreId).AsQueryable();
                }
            }
            else
            {
                if (pSearchBy == "TaxName" && !string.IsNullOrEmpty(pSearchValue))
                {
                    results = results.Where(a => a.TaxName.Contains(pSearchValue.Trim())).AsQueryable();
                }
                else if (pSearchBy == "TaxCode" && !string.IsNullOrEmpty(pSearchValue))
                {
                    results = results.Where(a => a.TaxCode.Contains(pSearchValue.Trim())).AsQueryable();
                }
                else
                {
                    results = results.AsQueryable();
                }
            }
            _count = results.Count();
            if (!string.IsNullOrEmpty(sortBy))
            {
                System.Reflection.PropertyInfo property = lsttaxcls.GetType().GetGenericArguments()[0].GetType().GetProperty(sortBy);

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
                results = results.OrderBy(o => o.TaxName);
            }
            results = results.Skip(startIndex).Take(pageSize);
            return results;
        }

        /// <summary>
        /// For Checking Duplicate Tax Class Name and Code at the time of Inserting/Adding new Tax class
        /// </summary>
        /// <param name="tbltaxclass">tb_TaxClass tbltaxclass</param>
        /// <returns>Return Boolean Value True/False</returns>
        public bool CheckDuplicate(tb_TaxClass tbltaxclass)
        {
            if (dac.CheckDuplicate(tbltaxclass) == 0)
            {
                return false;
            }
            else
                return true;

        }

        /// <summary>
        /// Create Tax Class For Inserting/Adding new Tax class
        /// </summary>
        /// <param name="tbltaxclass">tb_TaxClass tbltaxclass</param>
        /// <returns>Return Id of the Created/Added Record</returns>
        public Int32 Createtaxclass(tb_TaxClass tbltaxclass)
        {
            Int32 isAdded = 0;
            try
            {
                dac.Createtaxclass(tbltaxclass);
                isAdded = tbltaxclass.TaxClassID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Get Property Value
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="property"></param>
        /// <returns>Returns Property Value</returns>
        private static object GetPropertyValue(object obj, string property)
        {
            System.Reflection.PropertyInfo propertyInfo = obj.GetType().GetProperty(property);
            return propertyInfo.GetValue(obj, null);
        }

        /// <summary>
        /// Get Count For Paging in Grid view
        /// </summary>
        public static int GetCount(string CName, int pStoreId, string pSearchBy, string pSearchValue)
        {
            return _count;
        }

        /// <summary>
        /// For Updating Tax Class
        /// </summary>
        /// <param name="tbltaxclass">Table Tax class</param>
        /// <returns>Returns int Tax Class Id Which is Updated  </returns>
        public Int32 UpdateTaxClass(tb_TaxClass tbltaxclass)
        {
            Int32 isUpdated = 0;
            try
            {
                dac.Update(tbltaxclass);
                isUpdated = tbltaxclass.TaxClassID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }

        /// <summary>
        /// Get Tax CLass For Getting Particular Record From Table by Id
        /// </summary>
        /// <param name="taxclassID">int taxclassID</param>
        /// <returns>Return table Tax class List</returns>
        public tb_TaxClass gettaxclass(int taxclassID)
        {
            return dac.GetTaxClass(taxclassID);
        }

        /// <summary>
        /// For Deleting Particular Record
        /// </summary>
        /// <param name="tblTaxclass">Table Tax class</param>
        /// <returns>return int Id which is Deleted</returns>
        public Int32 Deletetaxclass(tb_TaxClass tblTaxclass)
        {
            Int32 isUpdated = 0;
            try
            {
                dac.Update(tblTaxclass);
                isUpdated = tblTaxclass.TaxClassID;
                _afterDelete = true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }

        /// <summary>
        /// Default Constructor For TaxclassComponent 
        /// </summary>
        public TaxClassComponent()
        {

        }

        /// <summary>
        /// Constructor for TaxclassComponent For State Tax
        /// </summary>
        /// <param name="StateID">int StateID</param>
        /// <param name="TaxClassID">int TaxClassID</param>
        /// <param name="Taxrate">decimal Tax rate</param>
        public TaxClassComponent(int StateID, int TaxClassID, decimal Taxrate)
        {
            _StateID = StateID;
            _TaxClassIDForState = TaxClassID;
            _TaxRate = Taxrate;
        }


        /// <summary>
        ///Get State Tax sorted list  from State Tax Table
        /// </summary>
        /// <returns>StateTaxTable in SortedList</returns>
        public SortedList GetArrayList()
        {
            int cnt = 0;
            SortedList StateTaxTable = new SortedList();
            DataSet ds = GetStateTax(4);
            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                cnt++;
                StateTaxTable.Add(dr["StateID"].ToString() + "_" + dr["TaxClassID"].ToString(), new TaxClassComponent(Convert.ToInt32(dr["StateID"].ToString()), Convert.ToInt32(dr["TaxClassID"].ToString()), Math.Round(Convert.ToDecimal(dr["TaxRate"].ToString()), 2)));
            }

            return StateTaxTable;
        }

        /// <summary>
        ///Update/Insert State Tax  
        /// </summary>
        /// <returns>Return 1 or 0</returns>
        public int EditStateTax(int Mode)
        {
            return dac.EditStateTax(StateID, TaxClassIDForState, this.TaxRate, Mode);
        }

        /// <summary>
        ///Delete/Clear State Tax in the Store with Particular StateID
        /// </summary>
        /// <param name="StateID">StateID</param>
        /// <param name="Mode">Mode</param>
        /// <returns>Return 1 or 0</returns>
        public int DeleteStateTax(Int32 StateID, int Mode)
        {
            return dac.DeleteStateTax(StateID, Mode);
        }

        /// <summary>
        ///Get all States State Tax
        /// </summary>
        /// <returns>Return State Tax list Dataset</returns>
        public DataSet GetStateTax(int Mode)
        {
            return dac.GetStateTax(Mode);
        }

        /// <summary>
        /// Get All state
        /// </summary>
        /// <param name="Mode">Mode</param>
        /// <returns>return State list as a Dataset</returns>
        public DataSet GetState(int Mode)
        {
            return dac.GetState(Mode);
        }

        /// <summary>
        /// Search For State By Name
        /// </summary>
        /// <param name="SearchField">String SearchField</param>
        /// <param name="SearchValue">String SearchValue</param>
        public static DataView SearchForState(String SearchField, String SearchValue)
        {
            DataView Dv = new DataView();
            try
            {
                if (SearchValue.Length != 0)
                {
                    Dv = _DataSourceState;
                    if (SearchValue == "0123456789")
                    {
                        string SearchStr = String.Empty;
                        for (int i = 0; i <= 9; i++)
                        {
                            SearchStr += SearchField + " Like '" + i.ToString() + "%' ";
                            if (i < 9)
                                SearchStr += " Or ";
                        }
                        Dv.RowFilter = SearchStr;
                    }
                    else
                    {
                        String FilterStr = SearchField + " Like '%" + SearchValue.Trim() + "%'";
                        Dv.RowFilter = FilterStr;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return Dv;
        }

        /// <summary>
        /// Constructor For TaxClassComponent For ZipCode
        /// </summary>
        /// <param name="ZipCode">string ZipCode</param>
        /// <param name="TaxClassID">int TaxClassID</param>
        /// <param name="TaxRate">decimal TaxRate</param>
        /// <param name="Temp">int Temp For Separating Constructor for ZipCode From Constructor for State</param>
        public TaxClassComponent(string ZipCode, int TaxClassID, decimal TaxRate, int Temp)
        {
            _ZipCode = ZipCode;
            _TaxClassIDForState = TaxClassID;
            _TaxRate = TaxRate;
        }

        /// <summary>
        /// Search For Zip Code
        /// </summary>
        /// <param name="SearchField">SearchField</param>
        /// <param name="SearchValue">SearchValue</param>
        /// <returns>DataView</returns>
        public static DataView SearchForZipCode(String SearchField, String SearchValue)
        {
            DataView DvZipCode = new DataView();
            try
            {
                if (SearchValue.Length != 0)
                {
                    DvZipCode = _DataSourceZipcode;
                    if (SearchValue == "0123456789")
                    {
                        string SearchStr = String.Empty;
                        for (int i = 0; i <= 9; i++)
                        {
                            SearchStr += SearchField + " Like '" + i.ToString() + "%' ";
                            if (i < 9)
                                SearchStr += " Or ";
                        }
                        DvZipCode.RowFilter = SearchStr;
                    }
                    else
                    {
                        String FilterStr = SearchField + " Like '%" + SearchValue.Trim() + "%'";
                        DvZipCode.RowFilter = FilterStr;
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return DvZipCode;
        }

        /// <summary>
        ///Get ZipCode Tax sorted list  from ZipTax Table
        /// </summary>
        /// <returns>ZipTaxTable in SortedList</returns>
        public SortedList GetArrayListForZipCode()
        {
            SortedList ZipTaxTable = new SortedList();
            DataSet DsZipCode = GetZipCodeTax(1);
            if (DsZipCode != null && DsZipCode.Tables != null && DsZipCode.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in DsZipCode.Tables[0].Rows)
                {
                    ZipTaxTable.Add(dr["ZipCode"].ToString() + "_" + dr["TaxClassID"].ToString(), new TaxClassComponent(dr["ZipCode"].ToString(), Convert.ToInt32(dr["TaxClassID"].ToString()), Math.Round(Convert.ToDecimal(dr["TaxRate"].ToString()), 2), 1));
                }
            }
            return ZipTaxTable;
        }

        /// <summary>
        /// Get ZipCodeTax For Getting all ZipTax From Table
        /// </summary>
        /// <param name="Mode">Mode</param>
        /// <returns>Returns the Zip code tax list as a DataSet</returns>
        public DataSet GetZipCodeTax(int Mode)
        {
            return dac.GetZipCodeTax(Mode);
        }

        /// <summary>
        ///  Get All Zip Code
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns the Zip code list as a DataSet</returns>
        public DataSet GetZipCode(int Mode)
        {
            return dac.GetZipCode(Mode);
        }

        /// <summary>
        /// AddZipTax For Adding/Updating Zip Code and Tax Rate
        /// </summary>
        /// <param name="OldZipCode">string OldZipCode</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Result in int</returns>
        public int AddZipTax(string OldZipCode, int Mode)
        {
            return dac.AddZipTax(OldZipCode, this.ZipTaxID, this.ZipCode, this.TaxClassIDForState, this.TaxRate, Mode);
        }

        /// <summary>
        /// Delete ZipTax For Deleting ZipCode and Tax Rate
        /// </summary>
        /// <param name="ZipCode">string OldZipCode</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Result 1 if Deleted</returns>
        public int DeleteZipTax(string ZipCode, int Mode)
        {
            return dac.DeleteZipTax(ZipCode, Mode);
        }
    }

    public class TaxClassEntity
    {
        private int _TaxClassID;
        private int _CreatedBy;
        private string _StoreName;
        private string _TaxCode;
        private string _TaxName;
        private string _DisplayOrder;
        private int _StoreID; private int _UpdatedBy;
        private DateTime _CreatedOn;
        private DateTime _UpdatedOn;
        private bool _Deleted;

        public int TaxClassID
        {
            get { return _TaxClassID; }
            set { _TaxClassID = value; }
        }
        public int StoreID
        {
            get { return _StoreID; }
            set { _StoreID = value; }
        }
        public string StoreName
        {
            get { return _StoreName; }
            set { _StoreName = value; }
        }
        public string TaxCode
        {
            get { return _TaxCode; }
            set { _TaxCode = value; }
        }
        public string TaxName
        {
            get { return _TaxName; }
            set { _TaxName = value; }
        }
        public string DisplayOrder
        {
            get { return _DisplayOrder; }
            set { _DisplayOrder = value; }
        }
        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }
        public int UpdatedBy
        {
            get { return _UpdatedBy; }
            set { _UpdatedBy = value; }
        }
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public DateTime UpdatedOn
        {
            get { return _UpdatedOn; }
            set { _UpdatedOn = value; }
        }
        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }
    }

}
