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
using System.Collections;

namespace Solution.Data
{
    /// <summary>
    /// Manufacture Data Access Class Contains Manufacture related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ManufactureDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        private static RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();
        #endregion

        #region Key Functions

        /// <summary>
        /// Get Manufacture 
        /// </summary>
        /// <param name="StoreID">Unique StoreID</param>
        /// <returns>Manufacture Details</returns>
        public DataSet GetManufactureByStoreID(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Manufacture_GetManufactureByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method for Save Manufacturer Data in tb_Manufacturer
        /// </summary>
        /// <param name="Manufacturer">Manufacture Table with parameter</param>
        /// <returns>table of manufacture</returns>
        public tb_Manufacture CreateManufacturer(tb_Manufacture Manufacturer)
        {
            ctxRedtag.AddTotb_Manufacture(Manufacturer);
            ctxRedtag.SaveChanges();
            return Manufacturer;
        }

        /// <summary>
        /// Method to Get All Manufacturer Detail
        /// </summary>
        /// <returns>All Manufacture</returns>
        public DataSet GetAllManufactureDetail()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Manufacture_GetAllManufacture";
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to delete Manufacture from Manufacture table
        /// </summary>
        /// <param name="manufactureid">int manufacture id</param>
        public void DeleteManufacture(int manufactureid)
        {

            tb_Manufacture manufacture = ctxRedtag.tb_Manufacture.FirstOrDefault(m => m.ManufactureID == manufactureid);
            ctxRedtag.DeleteObject(manufacture);
            ctxRedtag.SaveChanges();
        }

        /// <summary>
        /// Method to check manufacture name exist or not
        /// </summary>
        /// <param name="ManufactureName"></param>
        /// <returns>Total count of manufactures</returns>
        public int CheckManufactureName(string ManufactureName, Int32 StrStoreId)
        {
            int cQuery = (from m in ctxRedtag.tb_Manufacture
                          where m.tb_Store.StoreID == StrStoreId
                          && m.Name.ToLower().Trim() == ManufactureName.ToLower().Trim()
                          select m).Count();
            return cQuery;
        }

        /// <summary>
        /// Method to get manufacture detail by manufacture id and store id
        /// </summary>
        /// <param name="manufactureId">manufacture Id</param>
        /// <param name="StoreId">Unique StoreId</param>
        /// <returns>manufacture Details</returns>
        public DataSet GetManufactureByManID(int manufactureId, int StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Manufacture_GetManufactureByManID";
            cmd.Parameters.AddWithValue("@ManufactureID", manufactureId);
            cmd.Parameters.AddWithValue("@StoreID", StoreId);
            cmd.Parameters.AddWithValue("@opt", 1);
            return objSql.GetDs(cmd);

        }

        /// <summary>
        /// Method to get manufacture detail by name and store id
        /// </summary>
        /// <param name="Name">Manufacture Name</param>
        /// <param name="StoreId">Unique StoreID</param>
        /// <returns>Manufacture Details</returns>
        public DataSet GetManufactureByName(string Name, int StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Manufacture_GetManufactureByManID";
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@StoreID", StoreId);
            cmd.Parameters.AddWithValue("@opt", 2);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Method to update Manufacture detail by manufacture id
        /// </summary>
        /// <param name="Manufacture">Manufacture table</param>
        public void Update(tb_Manufacture Manufacture)
        {
            ctxRedtag.tb_Manufacture.Single(tb_Manufacture => tb_Manufacture.ManufactureID == Manufacture.ManufactureID);
            ctxRedtag.ApplyPropertyChanges("tb_Manufacture", Manufacture);
            ctxRedtag.SaveChanges();
        }
        #endregion


    }
}
