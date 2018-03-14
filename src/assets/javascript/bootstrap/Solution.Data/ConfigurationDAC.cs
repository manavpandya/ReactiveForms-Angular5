using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Data;
using System.Web;
using Solution.Bussines.Entities;
using System.Data.Objects;
using System.Collections;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// Configuration Data Access Class Contains Application Configuration related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ConfigurationDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key functions

        #region Get MAil Config

        /// <summary>
        /// Get Mail Details from tb_AppConfig
        /// </summary>
        /// <param name="ConfigName">string ConfigName</param>
        /// <param name="ConfigValue">string ConfigValue</param>
        /// <param name="Storeid">int Storeid</param>
        /// <param name="UpdatedOn">DateTime UpdatedOn</param>
        /// <param name="UpdatedBy">int UpdatedBy</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns Dataset with store Id and ConfigId,configname and Configvalue</returns>

        public DataSet GetMailConfig(string ConfigName, string ConfigValue, int Storeid, DateTime UpdatedOn, int UpdatedBy, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_MailConfig";
            if (ConfigName != "" && ConfigName.Length > 0)
                cmd.Parameters.AddWithValue("@ConfigName", ConfigName);
            if (ConfigValue != "" && ConfigValue.Length > 0)
                cmd.Parameters.AddWithValue("@ConfigValue", ConfigValue);
            cmd.Parameters.AddWithValue("@StoreID", Storeid);
            cmd.Parameters.AddWithValue("@UpdatedOn", UpdatedOn);
            cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
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
        /// 
        public Object UpdateMailConfig(string ConfigName, string ConfigValue, int Storeid, DateTime UpdatedOn, int UpdatedBy, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_MailConfig";
            cmd.Parameters.AddWithValue("@ConfigName", ConfigName);
            cmd.Parameters.AddWithValue("@ConfigValue", ConfigValue);
            cmd.Parameters.AddWithValue("@StoreID", Storeid);
            cmd.Parameters.AddWithValue("@UpdatedBy", UpdatedBy);
            cmd.Parameters.AddWithValue("@UpdatedOn", UpdatedOn);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
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
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ImageSize";
            cmd.Parameters.AddWithValue("@StoreID", Storeid);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);

        }

        #endregion

        #region Get Image Size
        /// <summary>
        /// Get Image Size
        /// </summary>
        /// <param name="storeId">int storeId</param>
        /// <param name="ImageName">string ImageName</param>
        /// <returns>Returns Dataset of Get Image</returns>

        public DataSet GetImageSizeByType(int storeId, string ImageName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ImageSize";
            cmd.Parameters.AddWithValue("@StoreID", storeId);
            cmd.Parameters.AddWithValue("@Mode", 3);
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            return objSql.GetDs(cmd);
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

        public DataSet UpdateImageSize(string ImageName, int Storeid, string ImageSize, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ImageSize";
            cmd.Parameters.AddWithValue("@ImageName", ImageName);
            cmd.Parameters.AddWithValue("@ImageSize", ImageSize);
            cmd.Parameters.AddWithValue("@StoreID", Storeid);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        #endregion

        /// <summary>
        /// Insert new Application config details
        /// </summary>
        /// <param name="appConfig">A AppConfig object</param>
        /// <returns>Returns Appconfig Entity</returns>
        public tb_AppConfig Create(tb_AppConfig appConfig)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_AppConfig(appConfig);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return appConfig;
        }

        /// <summary>
        /// Get App config detail for Edit mode
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns tb_AppConfig table</returns>
        public tb_AppConfig GetAppConfig(int appConfigId)
        {
            tb_AppConfig appConfig = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                appConfig = ctx.tb_AppConfig.First(e => e.AppConfigID == appConfigId);
            }
            return appConfig;
        }

        /// <summary>
        /// Update store config detail
        /// </summary>
        /// <param name="store">tb_AppConfig appConfig</param>
        /// <returns>Returns Count of Rows affected</returns>
        public int Update(tb_AppConfig appConfig)
        {
            int RowsAffected = 0;
            try
            {
                RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
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
        /// Check App configuration is already inserted or not
        /// </summary>
        /// <param name="appConfig">tb_AppConfig appConfigt</param>
        /// <returns>Returns true if configname exists else false</returns>
        public int CheckDuplicate(tb_AppConfig appConfig)
        {
            Int32 isExists = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(appConfig.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var results = from a in ctx.tb_AppConfig
                          where a.Deleted.Value == false
                          select new
                          {
                              appconfig = a,
                              store = a.tb_Store
                          };
            isExists = (from a in results
                        where a.appconfig.ConfigName == appConfig.ConfigName && a.store.StoreID == ID
                        && a.appconfig.AppConfigID != appConfig.AppConfigID
                        select new { a.appconfig.AppConfigID }
                            ).Count();
            return isExists;
        }

        /// <summary>
        /// Get Values From tb_AppConfig table using AppconfigName and storeId
        /// </summary>
        /// <param name="AppConfigName">string AppConfigName</param>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns ConfigValue Values from tb_AppConfig table</returns>
        public String GetAppConfigByName(string AppConfigName, int StoreID)
        {
            SQLAccess objsql = new SQLAccess();
            object value = objsql.ExecuteScalarQuery("SELECT ISNULL(ConfigValue,'') AS ConfigValue FROM dbo.tb_AppConfig WHERE ConfigName='" + AppConfigName.Replace("'", "''") + "' AND ISNULL(Deleted,0) <> 1 AND StoreID=" + StoreID);
            if (value != null)
                return value.ToString();
            return string.Empty;
        }

        public DataSet GetConfigDescription(String Configname, int Storeid)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_UpdateConfig";
            cmd.Parameters.AddWithValue("@Configname", Configname);
            cmd.Parameters.AddWithValue("@StoreID", Storeid);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }
        public Int32 UpdateAppConfigvalue(String Configname, string flag, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_UpdateConfig";
            cmd.Parameters.AddWithValue("@Configname", Configname);
            cmd.Parameters.AddWithValue("@flag", flag);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
        }
        #endregion

        #region GetBreadCrum

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
        public String GetBreadCrum(int CategoryID, int ParentId, int Storeid, string Type, int Mode, int IsInsert)
        {
            string BreadCrumPath = "";
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_GetBreadCrumbPath";
            cmd.Parameters.AddWithValue("@ID", CategoryID);
            cmd.Parameters.AddWithValue("@PID", ParentId);
            cmd.Parameters.AddWithValue("@AppPath", "");
            cmd.Parameters.AddWithValue("@StoreID", Storeid);
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@IsInsert", IsInsert);
            BreadCrumPath = Convert.ToString(objSql.ExecuteScalarQuery(cmd));
            return BreadCrumPath;

        }

        #endregion
    }
}
