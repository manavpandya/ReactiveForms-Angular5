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

namespace Solution.Data
{
    /// <summary>
    /// HeaderBanner Data Access Class Contains Home Page Header Banner related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class HomeBannerDAC
    {
        #region Declaration
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        #endregion

        #region Key Functions
        /// <summary>
        /// Get Header Details
        /// </summary>
        /// <returns> Get Header Details</returns>
        public DataSet GetHomeBanner(int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_HomeBanner";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }
        public DataSet GetHomeRotatorBanner(int StoreID, int BannerTypeId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_HomeRotatorBanner";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@BannerTypeId", BannerTypeId);
            return objSql.GetDs(cmd);
        }
        public Int32 Insertbanner(tb_RotatorHomeBannerDetail objRoatator)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_InsertHomeRotatorBanner";
            cmd.Parameters.AddWithValue("@BannerID", objRoatator.BannerID);
            cmd.Parameters.AddWithValue("@StoreID", objRoatator.StoreID);
            cmd.Parameters.AddWithValue("@Title", objRoatator.Title);
            cmd.Parameters.AddWithValue("@BannerURL", objRoatator.BannerURL);
            cmd.Parameters.AddWithValue("@ImageName", objRoatator.ImageName);
            cmd.Parameters.AddWithValue("@Active", objRoatator.Active);
            cmd.Parameters.AddWithValue("@HomeRotatorId", objRoatator.HomeRotatorId);
            cmd.Parameters.AddWithValue("@Pagination", objRoatator.Pagination);
            cmd.Parameters.AddWithValue("@Mode", 1);
            cmd.Parameters.AddWithValue("@IsMain", objRoatator.IsMain);
            cmd.Parameters.AddWithValue("@DisplayOrder", objRoatator.DisplayOrder);
            cmd.Parameters.AddWithValue("@StartDate", objRoatator.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", objRoatator.EndDate);
            
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            objSql.ExecuteScalarQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);

             
        }
        public void Updatebanner(tb_RotatorHomeBannerDetail objRoatator)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_InsertHomeRotatorBanner";
            cmd.Parameters.AddWithValue("@BannerID", objRoatator.BannerID);
            cmd.Parameters.AddWithValue("@StoreID", objRoatator.StoreID);
            cmd.Parameters.AddWithValue("@Title", objRoatator.Title);
            cmd.Parameters.AddWithValue("@BannerURL", objRoatator.BannerURL);
            cmd.Parameters.AddWithValue("@ImageName", objRoatator.ImageName);
            cmd.Parameters.AddWithValue("@Active", objRoatator.Active);
            cmd.Parameters.AddWithValue("@HomeRotatorId", objRoatator.HomeRotatorId);
            cmd.Parameters.AddWithValue("@Pagination", objRoatator.Pagination);
            cmd.Parameters.AddWithValue("@Mode", 2);
            cmd.Parameters.AddWithValue("@IsMain", objRoatator.IsMain);
            cmd.Parameters.AddWithValue("@DisplayOrder", objRoatator.DisplayOrder);
            cmd.Parameters.AddWithValue("@LinkTarget", objRoatator.LinkTarget);
            cmd.Parameters.AddWithValue("@StartDate", objRoatator.StartDate);
            cmd.Parameters.AddWithValue("@EndDate", objRoatator.EndDate);
            objSql.ExecuteNonQuery(cmd);
        }
        public void DeleteBanner(tb_RotatorHomeBannerDetail objRoatator)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_InsertHomeRotatorBanner";
            cmd.Parameters.AddWithValue("@BannerID", objRoatator.BannerID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            objSql.ExecuteNonQuery(cmd);
        }
        public Int32 InsertRotatebanner(Int32 StoreId, Int32 BannerTypeId, Int32 Active)
        {
            objSql = new SQLAccess();
            return Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_RotatorHomebanner(StoreID,BannerTypeId,IsActive) VALUES (" + StoreId + "," + BannerTypeId + "," + Active + "); SELECT SCOPE_IDENTITY();"));
        }
        public void UpdateRotatebanner(Int32 StoreId, Int32 BannerTypeId)
        {
            objSql = new SQLAccess();
            objSql.ExecuteNonQuery("UPDATE tb_RotatorHomebanner SET IsActive=1 WHERE StoreId=" + StoreId + " AND BannerTypeId=" + BannerTypeId + "; UPDATE tb_RotatorHomebanner SET IsActive=0 WHERE StoreId=" + StoreId + " AND BannerTypeId <> " + BannerTypeId + "");
        }
        /// <summary>
        /// Get Home Page Banner
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Return tb_HomeBanner</returns>
        public tb_HomeBanner GetHomePageBanner(int Id)
        {

            tb_HomeBanner Homeb = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                Homeb = ctx.tb_HomeBanner.First(e => e.BannerID == Id);
            }
            return Homeb;
        }

        /// <summary>
        /// Get Item Page Banner
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Return tb_ItemBanners</returns>
        public tb_ItemBanners GetItemPageBanner(int Id)
        {

            tb_ItemBanners Homeb = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                Homeb = ctx.tb_ItemBanners.First(e => e.ItemBannerID == Id);
            }
            return Homeb;
        }

        /// <summary>
        /// Update Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public int Update(tb_HomeBanner Banner)
        {
            int RowsAffected = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;//)

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
        /// Update Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public int UpdateItemBanner(tb_ItemBanners Banner)
        {
            int RowsAffected = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;//)

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
        /// Check Duplicate Value
        /// </summary>
        /// <param name="objchkBanner">tb_HomeBanner objchkBanners</param>
        /// <returns>Returns Value already Exist or Not</returns>
        public Int32 CheckDuplicate(tb_HomeBanner objchkBanner)
        {
            Int32 isExeist = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExeist = (from a in ctx.tb_HomeBanner
                            where a.Title == objchkBanner.Title
                            && a.BannerURL == objchkBanner.BannerURL
                            && a.BannerID != objchkBanner.BannerID
                            select new { a.BannerID }).Count();
            }
            return isExeist;
        }

        /// <summary>
        /// Check Duplicate Value
        /// </summary>
        /// <param name="objchkBanner">tb_HomeBanner objchkBanners</param>
        /// <returns>Returns Value already Exist or Not</returns>
        public Int32 CheckDuplicateItemBanner(tb_ItemBanners objchkBanner)
        {
            Int32 isExeist = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExeist = (from a in ctx.tb_ItemBanners
                            where a.BannerTitle == objchkBanner.BannerTitle
                            && a.BannerUrl == objchkBanner.BannerUrl
                            && a.ItemBannerID != objchkBanner.ItemBannerID
                            && a.ItemBannerTypeID == objchkBanner.ItemBannerTypeID
                            select new { a.ItemBannerID }).Count();
            }
            return isExeist;
        }

        /// <summary>
        /// Create Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public tb_HomeBanner Create(tb_HomeBanner objBanner)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_HomeBanner(objBanner);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objBanner;
        }

        /// <summary>
        /// Create Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public tb_ItemBanners CreateItemBanner(tb_ItemBanners objBanner)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_ItemBanners(objBanner);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objBanner;
        }
        #endregion


        #region Item Page Banner Function
        public DataSet GetItemPageBanners(Int32 BannerTypeID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ItemBanner_GetItemPageBannerDetails";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@ItemBannerTypeID", BannerTypeID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }
        public DataSet GetIemBannerTypes(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ItemBanner_GetItemPageBannerDetails";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }

        #endregion
    }
}
