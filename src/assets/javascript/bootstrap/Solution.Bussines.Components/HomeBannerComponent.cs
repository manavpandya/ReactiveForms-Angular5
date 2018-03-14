using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Data;

using System.Web;

using Solution.Bussines.Entities;
using System.Data.Objects;

namespace Solution.Bussines.Components
{
    /// <summary>
    /// Home Banner Component Class Contains Home Page Banner related Business Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class HomeBannerComponent
    {
        /// <summary>
        /// Get Home Page Banner
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        /// <returns>Return Dataset</returns>
        public static DataSet GetHomeBanner(int StoreID)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetHomeBanner(StoreID);
            return DSCommon;
        }

        /// <summary>
        /// Get Home Page Banner
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Return tb_HomeBanner</returns>

        public DataSet GetHomeRotatorBanner(int StoreID, int BannerTypeId)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetHomeRotatorBanner(StoreID, BannerTypeId);
            return DSCommon;
        }
        public Int32 Insertbanner(tb_RotatorHomeBannerDetail objRoatator)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            return Convert.ToInt32(dac.Insertbanner(objRoatator));
        }
        public void UpdatebannerDetail(tb_RotatorHomeBannerDetail objRoatator)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            dac.Updatebanner(objRoatator);
        }
        public void DeleteBanner(tb_RotatorHomeBannerDetail objRoatator)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            dac.DeleteBanner(objRoatator);
        }
        public Int32 InsertRotatebanner(Int32 StoreId, Int32 BannerTypeId, Int32 Active)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            return Convert.ToInt32(dac.InsertRotatebanner(StoreId, BannerTypeId, Active));
        }
        public void UpdateRotatebanner(Int32 StoreId, Int32 BannerTypeId)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            dac.UpdateRotatebanner(StoreId, BannerTypeId);
        }
        public tb_HomeBanner getHomePageBanner(int Id)
        {
            HomeBannerDAC objBanner = new HomeBannerDAC();
            return objBanner.GetHomePageBanner(Id);

        }
        public tb_ItemBanners GetItemPageBanner(int Id)
        {
            HomeBannerDAC objBanner = new HomeBannerDAC();
            return objBanner.GetItemPageBanner(Id);

        }

        /// <summary>
        /// Update Home Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public Int32 UpdateHomeBanner(tb_HomeBanner Banner)
        {
            Int32 isUpdated = 0;
            try
            {
                HomeBannerDAC objBanner = new HomeBannerDAC();
                if (objBanner.CheckDuplicate(Banner) == 0)
                {
                    UpdateBanner(Banner);
                    isUpdated = Banner.BannerID;
                }
                else
                {
                    isUpdated = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }

        /// <summary>
        /// Update Item Banner
        /// </summary>
        /// <param name="Banner">tb_ItemBanners Banner</param>
        /// <returns>Return Int32</returns>
        public Int32 UpdateItemBanner(tb_ItemBanners Banner)
        {
            Int32 isUpdated = 0;
            try
            {
                HomeBannerDAC objBanner = new HomeBannerDAC();
                if (objBanner.CheckDuplicateItemBanner(Banner) == 0)
                {
                    UpdateItemPageBanner(Banner);
                    isUpdated = Banner.ItemBannerID;
                }
                else
                {
                    isUpdated = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isUpdated;
        }

        /// <summary>
        /// Update Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public int UpdateBanner(tb_HomeBanner Banner)
        {
            int RowsAffected = 0;
            HomeBannerDAC objBanner = new HomeBannerDAC();
            RowsAffected = objBanner.Update(Banner);
            return RowsAffected;
        }

        /// <summary>
        /// Update item Page Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public int UpdateItemPageBanner(tb_ItemBanners Banner)
        {
            int RowsAffected = 0;
            HomeBannerDAC objBanner = new HomeBannerDAC();
            RowsAffected = objBanner.UpdateItemBanner(Banner);
            return RowsAffected;
        }

        /// <summary>
        /// Create Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public Int32 CreateBanner(tb_HomeBanner Banner)
        {
            Int32 isAdded = 0;
            try
            {
                HomeBannerDAC objBanner = new HomeBannerDAC();
                if (objBanner.CheckDuplicate(Banner) == 0)
                {
                    objBanner.Create(Banner);
                    isAdded = Banner.BannerID;
                }
                else
                {
                    isAdded = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        /// <summary>
        /// Create Banner
        /// </summary>
        /// <param name="Banner">tb_HomeBanner Banner</param>
        /// <returns>Return Int32</returns>
        public Int32 CreateItemBanner(tb_ItemBanners Banner)
        {
            Int32 isAdded = 0;
            try
            {
                HomeBannerDAC objBanner = new HomeBannerDAC();
                if (objBanner.CheckDuplicateItemBanner(Banner) == 0)
                {
                    objBanner.CreateItemBanner(Banner);
                    isAdded = Banner.ItemBannerID;
                }
                else
                {
                    isAdded = -1;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isAdded;
        }

        #region Item Page Banner Function
        public DataSet GetItemPageBanners(Int32 BannerTypeID, Int32 StoreID)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetItemPageBanners(BannerTypeID, StoreID);
            return DSCommon;
        }
        public DataSet GetIemBannerTypes(Int32 StoreID)
        {
            HomeBannerDAC dac = new HomeBannerDAC();
            DataSet DSCommon = new DataSet();
            DSCommon = dac.GetIemBannerTypes(StoreID);
            return DSCommon;
        }

        #endregion
    }
}
