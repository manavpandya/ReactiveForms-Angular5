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
    /// Feed Component Class Contains Feed related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class FeedDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Function For Create Country
        /// </summary>
        /// <param name="objCountry">tb_FeedMaster objCountry</param>
        /// <returns>Returns table </returns>
        public tb_FeedMaster Create(tb_FeedMaster objCountry)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_FeedMaster(objCountry);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCountry;
        }

        /// <summary>
        /// Sub Method for Feed Master
        /// </summary>
        /// <param name="Feed">tb_FeedMaster Feed</param>
        /// <returns>Returns Object</returns>
        public void Update(tb_FeedMaster Feed)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;//)

            try
            {
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }

        }

        /// <summary>
        ///  Get Feed Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Return a Feed record for edit in table form</returns>
        public tb_FeedMaster getFeed(int Id)
        {
            tb_FeedMaster cntr = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                cntr = ctx.tb_FeedMaster.FirstOrDefault(e => e.FeedID == Id);
            }
            return cntr;
        }

        /// <summary>
        /// Check Duplicate Record function for Insert and Update
        /// </summary>
        /// <param name="objchkFeedMaster">tb_FeedMaster objchkFeedMaster</param>
        /// <returns>Returns no. of record count when specified record is exist in Database</returns>
        public Int32 CheckDuplicate(tb_FeedMaster objchkFeedMaster)
        {
            Int32 isExeist = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExeist = (from a in ctx.tb_FeedMaster
                            where a.FeedName == objchkFeedMaster.FeedName
                            && a.FeedID != objchkFeedMaster.FeedID
                            select new { a.FeedID }).Count();
            }
            return isExeist;
        }

        /// <summary>
        /// For Delete Feed By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void Delete(int Id)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_FeedMaster cust = ctx.tb_FeedMaster.First(c => c.FeedID == Id);
            ctx.DeleteObject(cust);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Get Feed Details
        /// </summary>
        /// <param name="SId">Int32 SId</param>
        /// <param name="SearchVal">string SearchVal</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedDetails(Int32 StoreID, string SearchVal)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_Feed]";
            cmd.Parameters.AddWithValue("@SearchVal", SearchVal);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Feed Details using FeedId
        /// </summary>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedDataByFeedId(Int32 FeedID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_Feed]";
            cmd.Parameters.AddWithValue("@FeedID", FeedID);
            cmd.Parameters.AddWithValue("@Mode", 4);
            return objSql.GetDs(cmd);
        }
        #endregion

        /// <summary>
        /// Check Duplicate Feed Field Value
        /// </summary>
        /// <param name="objFeedField">tb_FeedFieldMaster objFeedField</param>
        /// <returns>Returns Int32</returns>
        public Int32 CheckDuplicateFeedField(tb_FeedFieldMaster objFeedField)
        {
            Int32 isExists = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                try
                {
                    isExists = (from cust in ctx.tb_FeedFieldMaster
                                where cust.FieldName == objFeedField.FieldName
                                && cust.tb_FeedMasterReference.EntityKey.EntityKeyValues[0].Key.ToString() == objFeedField.tb_FeedMasterReference.EntityKey.EntityKeyValues[0].Value
                                && cust.StoreId == objFeedField.StoreId
                                && cust.FieldID != objFeedField.FieldID
                                select new { cust.FieldID }).Count();
                }
                catch { }
            }
            return isExists;
        }

        /// <summary>
        /// Insert Feed Field Value
        /// </summary>
        /// <param name="tb_FeedFieldMaster">tb_FeedFieldMaster tb_FeedFieldMaster</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Id of inserted Feed Field</returns>
        public Int32 InsertFeedField(tb_FeedFieldMaster tb_FeedFieldMaster, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_FeedFieldMaster";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@FieldID", tb_FeedFieldMaster.FieldID);
            cmd.Parameters.AddWithValue("@FeedID", tb_FeedFieldMaster.tb_FeedMasterReference.EntityKey.EntityKeyValues[0].Value);
            cmd.Parameters.AddWithValue("@FieldName", tb_FeedFieldMaster.FieldName);
            cmd.Parameters.AddWithValue("@FieldTypeID", tb_FeedFieldMaster.tb_FeedFieldTypeMasterReference.EntityKey.EntityKeyValues[0].Value);
            cmd.Parameters.AddWithValue("@FieldDescription", tb_FeedFieldMaster.FieldDescription);
            cmd.Parameters.AddWithValue("@FieldLimit", tb_FeedFieldMaster.FieldLimit);
            cmd.Parameters.AddWithValue("@FieldHeight", tb_FeedFieldMaster.FieldHeight);
            cmd.Parameters.AddWithValue("@FieldWidth", tb_FeedFieldMaster.FieldWidth);
            cmd.Parameters.AddWithValue("@isRequired", tb_FeedFieldMaster.isRequired);
            cmd.Parameters.AddWithValue("@DisplayOrder", tb_FeedFieldMaster.DisplayOrder);
            cmd.Parameters.AddWithValue("@DefautValue", tb_FeedFieldMaster.DefautValue);

            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get Feed Field Data
        /// </summary>
        /// <param name="FieldId">Int32 FieldId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetDatabyFeedFieldIds(Int32 FieldId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_FeedFieldMaster";
            cmd.Parameters.AddWithValue("@FieldID", FieldId);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Feed Field Value DataList
        /// </summary>
        /// <param name="FieldId">Int32 FieldId</param>
        /// <param name="TypeId">Int32 TypeId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFieldValueByIds(Int32 FieldId, Int32 TypeId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_FeedFieldMaster";
            cmd.Parameters.AddWithValue("@FieldID", FieldId);
            cmd.Parameters.AddWithValue("@TypeID", TypeId);
            cmd.Parameters.AddWithValue("@Mode", 4);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Feed Master details by Store
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedMasterByStores(Int32 StoreId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_FeedFieldMaster";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@Mode", 5);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Feed Details 
        /// </summary>
        /// <param name="StoreId">Int32 StoreId</param>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedDetailsforFieldData(Int32 StoreId, Int32 FeedId)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_FeedFieldMaster";
            cmd.Parameters.AddWithValue("@StoreId", StoreId);
            cmd.Parameters.AddWithValue("@FeedId", FeedId);
            cmd.Parameters.AddWithValue("@Mode", 6);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Feed Product
        /// </summary>
        /// <param name="FeedId">Int32 FeedId</param>
        /// <param name="FeedName">String FeedName</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetFeedProduct(Int32 FeedId, String FeedName)
        {
            objSql = new SQLAccess();
            DataSet dsGetFeedProduct = new DataSet();
            cmd = new SqlCommand("usp_Feed_ProductSearching");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.Add("@FeedId", SqlDbType.Int).Value = FeedId;
            cmd.Parameters.Add("@FeedName", SqlDbType.NVarChar).Value = FeedName;
            dsGetFeedProduct = objSql.GetDs(cmd);
            return dsGetFeedProduct;
        }
    }
}
