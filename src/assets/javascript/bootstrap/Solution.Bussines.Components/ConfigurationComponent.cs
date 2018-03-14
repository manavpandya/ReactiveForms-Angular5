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
    /// Configuration Component Class Contains Configuration Related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ConfigurationComponent
    {
        #region Variabel Declaration
        private static int _count;
        #endregion

        #region Get Mail Config

        /// <summary>
        /// Get Mail Details from tb_AppConfig
        /// </summary>
        /// <param name="ConfigName">string ConfigName</param>
        /// <param name="ConfigValue">string ConfigValue</param>
        /// <param name="Storeid">int Storeid</param>
        /// <param name="UpdatedOn">DateTime UpdatedOn</param>
        /// <param name="UpdatedBy">int UpdatedBy</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns Dataset with store Id and Config Id ,configname and Config value</returns>

        public DataSet GetMailConfig(string ConfigName, string ConfigValue, int Storeid, DateTime UpdatedOn, int UpdatedBy, int Mode)
        {
            ConfigurationDAC dac = new ConfigurationDAC();
            return dac.GetMailConfig(ConfigName, ConfigValue, Storeid, UpdatedOn, UpdatedBy, Mode);
        }

        #endregion

        #region Update Mail Config

        /// <summary>
        /// update Details Of MailConfig
        /// </summary>
        /// <param name="controlName">string controlname</param>
        /// <param name="txtValue">string txtValue</param>
        /// <param name="Storeid">int Storeid</param>
        /// /// <param name="UpdatedOn">DateTime UpdatedOn</param>
        /// <param name="UpdatedBy">int UpdatedBy</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns Object</returns>

        public static Object UpdateMailConfig(string controlname, string txtValue, int Storeid, DateTime UpdatedOn, int UpdatedBy, int Mode)
        {
            ConfigurationDAC dac = new ConfigurationDAC();
            return dac.UpdateMailConfig(controlname, txtValue, Storeid, UpdatedOn, UpdatedBy, Mode);
        }

        #endregion

        #region GetImagesize function

        /// <summary>
        /// Get Images Size
        /// </summary>
        /// <param name="Storeid">int Storeid</param>
        /// <returns>Returns Dataset with Images Size details</returns>
        public DataSet GetImagesize(int Storeid)
        {
            ConfigurationDAC dac = new ConfigurationDAC();
            return dac.GetImagesize(Storeid);
        }

        #endregion

        #region Get Image

        /// <summary>
        /// Get Image Size
        /// </summary>
        /// <param name="storeId">int storeId</param>
        /// <param name="ImageName">string ImageName</param>
        /// <returns>Returns Dataset of Get Image</returns>
        public DataSet GetImageSizeByType(int storeId, string ImageName)
        {
            ConfigurationDAC dac = new ConfigurationDAC();
            return dac.GetImageSizeByType(storeId, ImageName);
        }

        #endregion

        #region UpdateImageSize function

        /// <summary>
        /// Update Images Size
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        /// <param name="Storeid">int Storeid</param>
        /// <param name="ImageSize">string ImageSize</param>
        /// <param name="mode">int mode</param>
        /// <returns>Returns Image details</returns>

        public static DataSet UpdateImageSize(string ImageName, int Storeid, string ImageSize, int mode)
        {
            ConfigurationDAC dac = new ConfigurationDAC();

            return dac.UpdateImageSize(ImageName, Storeid, ImageSize, mode);
        }

        #endregion

        #region Get Data By Filter

        /// <summary>
        /// Get data to fill grid
        /// </summary>
        /// <param name="startIndex">int startIndex</param>
        /// <param name="pageSize">int pageSize</param>
        /// <param name="sortBy">string sortBy</param>
        /// <param name="StoreId">int StoreId</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns Config Value</returns>
        public IQueryable<ApplicationConfigEntity> GetDataByFilter(int startIndex, int pageSize, string sortBy, int StoreId, string SearchBy, string SearchValue)
        {
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
                IQueryable<ApplicationConfigEntity> results = from a in ctx.tb_AppConfig
                                                              where a.Deleted.Value == false
                                                              select new ApplicationConfigEntity
                                                              {
                                                                  AppConfigID = a.AppConfigID,
                                                                  ConfigName = a.ConfigName,
                                                                  ConfigValue = a.ConfigValue,
                                                                  StoreID = a.tb_Store.StoreID,
                                                                  StoreName = a.tb_Store.StoreName,
                                                                  CreatedOn = a.CreatedOn.Value
                                                              };

                if (string.IsNullOrEmpty(SearchValue))
                {
                    SearchValue = "";
                }
                else
                {
                    SearchValue = SearchValue.Trim();
                }
                if (StoreId != -1)
                {
                    //search by ConfigName and storeID
                    if (SearchBy == "ConfigName")
                    {
                        results = results.Where(a => a.ConfigName.Contains(SearchValue) && a.StoreID == StoreId).AsQueryable();
                    }
                    else if (SearchBy == "ConfigValue")
                    {
                        //Search by config value and storeID
                        results = results.Where(a => a.ConfigValue.Contains(SearchValue) && a.StoreID == StoreId).AsQueryable();
                    }
                }
                else //For all store
                {
                    if (SearchBy == "ConfigName")
                    {
                        //search by Config name for all store
                        results = results.Where(a => a.ConfigName.Contains(SearchValue)).AsQueryable();
                    }
                    else if (SearchBy == "ConfigValue")
                    {
                        //Search by configvalue for all store
                        results = results.Where(a => a.ConfigValue.Contains(SearchValue)).AsQueryable();
                    }
                    else
                    {
                        //Get all value(No Filter)
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
                    //Default sorting by Config Name
                    results = results.OrderBy(o => o.ConfigName);
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
        /// <param name="StoreId">int StoreId</param>
        /// <param name="SearchBy">string SearchBy</param>
        /// <param name="SearchValue">string SearchValue</param>
        /// <returns>Returns No Of Records</returns>
        public static int GetCount(int StoreId, string SearchBy, string SearchValue)
        {
            return _count;
        }

        /// <summary>
        /// Add new Application configuration
        /// </summary>
        /// <param name="appConfig">tb_AppConfig appConfig</param>
        /// <returns>Returns ID of created app config entity</returns>
        public Int32 CreateAppConfiguration(tb_AppConfig appConfig)
        {
            Int32 isAdded = 0;
            try
            {
                ConfigurationDAC dac = new ConfigurationDAC();
                dac.Create(appConfig);
                isAdded = appConfig.AppConfigID;
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Get App config detail for Edit mode
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns tb_AppConfig table</returns>
        public tb_AppConfig getAppConfig(int Id)
        {
            ConfigurationDAC dac = new ConfigurationDAC();
            return dac.GetAppConfig(Id);
        }

        /// <summary>
        /// Check App configuration is already inserted or not
        /// </summary>
        /// <param name="appConfig">tb_AppConfig appConfigt</param>
        /// <returns>Returns true if configname exists else false</returns>
        public bool CheckDuplicate(tb_AppConfig appConfig)
        {
            ConfigurationDAC dac = new ConfigurationDAC();
            if (dac.CheckDuplicate(appConfig) == 0)
            {
                return false;
            }
            else
                return true;
        }

        /// <summary>
        /// Update store config detail
        /// </summary>
        /// <param name="store">tb_AppConfig appConfig</param>
        /// <returns>Returns Count of Rows affected</returns>
        public Int32 UpdateAppConfiguration(tb_AppConfig appConfig)
        {
            int RowsAffected = 0;
            try
            {
                ConfigurationDAC dac = new ConfigurationDAC();
                RowsAffected = dac.Update(appConfig);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }

        #endregion

        #region Get BreadCrum
        /// <summary>
        /// Get breadcrumb Data
        /// </summary>
        /// <param name="CategoryID">int CategoryID</param>
        /// <param name="ParentId">int ParentId</param>
        /// <param name="Storeid">int Storeid</param>
        /// <param name="Type">string Type</param>
        /// <param name="Mode">int mode</param>
        /// <param name="IsInsert">int IsInsert</param>
        /// <returns>Returns string - BreadCrum Path</returns>
        public static string GetBreadCrum(int CategoryID, int ParentId, int Storeid, string Type, int mode, int IsInsert)
        {
            string BreadCrumPath = "";
            try
            {
                ConfigurationDAC ObjappDAC = new ConfigurationDAC();
                BreadCrumPath = ObjappDAC.GetBreadCrum(CategoryID, ParentId, Storeid, Type, mode, IsInsert);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return BreadCrumPath;
        }

        #endregion

        /// <summary>
        /// Get Values From tb_AppConfig table using AppconfigName and storeId
        /// </summary>
        /// <param name="AppConfigName">string AppConfigName</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns ConfigValue Values from tb_AppConfig table</returns>
        public String GetAppConfigByName(string AppConfigName, int StoreID)
        {
            string appconfigname = "";
            try
            {
                ConfigurationDAC ObjappDAC = new ConfigurationDAC();
                appconfigname = ObjappDAC.GetAppConfigByName(AppConfigName, StoreID);
            }
            catch { }
            return appconfigname;
        }

        public DataSet GetConfigDescription(String Configname, int Storeid)
        {
            ConfigurationDAC dac = new ConfigurationDAC();
            return dac.GetConfigDescription(Configname, Storeid);
        }
        public Int32 UpdateAppConfigvalue(String Configname, string flag, Int32 StoreID)
        {
            int RowsAffected = 0;
            try
            {
                ConfigurationDAC dac = new ConfigurationDAC();
                RowsAffected = dac.UpdateAppConfigvalue(Configname, flag, StoreID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return RowsAffected;
        }
    }

    /// <summary>
    /// Variable Declaration
    /// </summary>
    public class ApplicationConfigEntity
    {
        private int _AppConfigID;
        private int _StoreID;
        private string _StoreName;
        private string _ConfigName;
        private string _ConfigValue;
        private DateTime _CreatedOn;
        private bool _Deleted;
        public int AppConfigID
        {
            get { return _AppConfigID; }
            set { _AppConfigID = value; }
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
        public string ConfigName
        {
            get { return _ConfigName; }
            set { _ConfigName = value; }
        }
        public string ConfigValue
        {
            get { return _ConfigValue; }
            set { _ConfigValue = value; }
        }
        public DateTime CreatedOn
        {
            get { return _CreatedOn; }
            set { _CreatedOn = value; }
        }
        public bool Deleted
        {
            get { return _Deleted; }
            set { _Deleted = value; }
        }
    }
}
