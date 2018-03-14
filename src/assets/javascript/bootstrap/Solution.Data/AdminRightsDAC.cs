using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// Admin Rights Data Access Class Contains Admin Rights Related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class AdminRightsDAC
    {
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        public DataTable dtAdminRightsList = null;

        /// <summary>
        /// Get Admin by ID
        /// </summary>
        /// <param name="AdminID">int AdminID</param>
        /// <returns>tb_Admin admin</returns>
        public tb_Admin GetAdminByID(int AdminID)
        {
            tb_Admin admin = null;
            RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();
            admin = ctxRedtag.tb_Admin.FirstOrDefault(e => e.AdminID == AdminID);
            return admin;
        }
        /// <summary>
        /// Get Admin Rights List for Page wise
        /// </summary>
        /// <returns>Returns Admin Rights List for Page wise</returns>
        public DataSet GetAdminPageRightList(Int32 AdminID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminRights_GetAdminRightDetail";
            cmd.Parameters.AddWithValue("@AdminID", AdminID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Update Admin Rights
        /// </summary>
        /// <param name="admin">tb_Admin admin</param>
        /// <returns>No of rows affected</returns>
        public int UpdateAdminRights(tb_Admin admin)
        {
            RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();
            int RowsAffected = 0;
            try
            {
                ctxRedtag.tb_Admin.FirstOrDefault(tb_Admin => tb_Admin.AdminID == admin.AdminID);
                ctxRedtag.ApplyPropertyChanges("tb_Admin", admin);
                RowsAffected = ctxRedtag.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return RowsAffected;
        }

        /// <summary>
        /// Insert Update Page Rights For Admin
        /// </summary>
        /// <param name="AdminID">Int32 AdminID</param>
        /// <param name="ID">Int32 ID</param>
        /// <param name="InnerRightsID">Int32 InnerRightsID</param>
        /// <param name="IsListed">Boolean IsListed</param>
        /// <param name="CreatedBy">Int32 CreatedBy</param>
        /// <returns>Returns the Identity Value</returns>
        public Int32 Insert_Update_PageRightsForAdmin(Int32 MainAdminID, Int32 CompareAdminID, Int32 ID, Int32 InnerRightsID, Boolean IsListed, Int32 CreatedBy)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminRights_Insert_Update_PageRightsForAdmin";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@MainAdminID", MainAdminID);
            cmd.Parameters.AddWithValue("@CompareAdminID", CompareAdminID);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@InnerRightsID", InnerRightsID);
            cmd.Parameters.AddWithValue("@IsListed", IsListed);
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Get All Page Rights By AdminID
        /// </summary>
        /// <param name="AdminID">Int32 AdminID</param>
        public void GetAllPageRightsByAdminID(Int32 AdminID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            DataSet dsright = new DataSet();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminRights_GetAdminRightDetail";
            cmd.Parameters.AddWithValue("@AdminID", AdminID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            dsright = objSql.GetDs(cmd);
            if (dsright != null && dsright.Tables.Count > 0 && dsright.Tables[0].Rows.Count > 0)
            {
                dtAdminRightsList = dsright.Tables[0];
                System.Web.HttpContext.Current.Session["dtAdminRightsList"] = dsright.Tables[0];
            }
        }

        /// <summary>
        /// Get Admin Rights For Page
        /// </summary>
        /// <param name="TabName">String TabName</param>
        /// <param name="PageName">String PageName</param>
        /// <returns>Returns a DataRow that contains Rights Information</returns>
        public DataRow GetAdminRightsForPage(String TabName, String PageName)
        {
            DataRow drRight = null;
            DataTable dtTemp = new DataTable();
            if (System.Web.HttpContext.Current.Session["dtAdminRightsList"] != null)
            {
                dtTemp = (DataTable)System.Web.HttpContext.Current.Session["dtAdminRightsList"];
                if (dtTemp != null && dtTemp.Rows.Count > 0)
                {
                    drRight = dtTemp.Select("TabName = '" + TabName + "' AND PageName = '" + PageName + "'")[0];
                }
            }
            return drRight;
        }

        #region Admin Template

        public DataSet GetAdminTemplatePageRightList()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminTemplateRights_GetAdminRightDetail";
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        public DataSet GetAdminTemplatePageName()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminTemplateRights_GetAdminRightDetail";
            cmd.Parameters.AddWithValue("@Mode", 2);
            return objSql.GetDs(cmd);
        }
        public DataSet GetAdminTemplatePageDetailByName(String Name)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminTemplateRights_GetAdminRightDetail";
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@Mode", 3);
            return objSql.GetDs(cmd);
        }

        public Int32 Insert_Update_TemplatePageRightsForAdmin(Int32 ID, String Name, Int32 InnerRightsID, Boolean IsListed, Int32 CreatedBy, String Rights)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminRights_Insert_Update_TemplatePageRightsForAdmin";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@Name", Name);
            cmd.Parameters.AddWithValue("@InnerRightsID", InnerRightsID);
            cmd.Parameters.AddWithValue("@IsListed", IsListed);
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("@Rights", Rights);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }
        public int CheckDuplicateTemplatename(tb_admin_Templaterights AdminTemplate)
        {
            Int32 isExists = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            // int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(EmailTemplate.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            isExists = (from a in ctx.tb_admin_Templaterights
                        where a.Name == AdminTemplate.Name
                        select new { a.Name }
                           ).Count();
            return isExists;
        }

        #endregion
        public Int32 Insert_Update_AssignPageRightsForAdmin(Int32 MainAdminID, Int32 CompareAdminID, Int32 ID, Int32 InnerRightsID, Boolean IsListed, Int32 CreatedBy, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminRights_Insert_Update_AssignPageRightsForAdmin";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@MainAdminID", MainAdminID);
            cmd.Parameters.AddWithValue("@CompareAdminID", CompareAdminID);
            cmd.Parameters.AddWithValue("@ID", ID);
            cmd.Parameters.AddWithValue("@InnerRightsID", InnerRightsID);
            cmd.Parameters.AddWithValue("@IsListed", IsListed);
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }


        public DataSet GetAdminAssignPageRightList(Int32 AdminID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AdminRights_GetAdminAssignRightDetail";
            cmd.Parameters.AddWithValue("@AdminID", AdminID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }
    }
}
