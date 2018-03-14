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
    /// State Component Class Contains State related Data Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class StateDAC
    {

        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        ///  Function For Create State
        /// </summary>
        /// <param name="objState">tb_State objState</param>
        /// <returns>Returns the State Table Object</returns>    
        public tb_State Create(tb_State objState)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_State(objState);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objState;
        }

        /// <summary>
        /// Update State Method for State
        /// </summary>
        /// <param name="State">tb_State objState</param>
        public void Update(tb_State State)
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
        /// Get State Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns State Record for edit fetched by id</returns>
        public tb_State getState(int Id)
        {
            tb_State state = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                state = ctx.tb_State.First(e => e.StateID == Id);
            }
            return state;
        }

        /// <summary>
        /// Check Duplicate Record function for Insert and Update
        /// </summary>
        /// <param name="objchkState">tb_State objchkState</param>
        /// <returns>Returns no. of record count when specified record is exist in Database</returns>
        public Int32 CheckDuplicate(tb_State objchkState)
        {
            Int32 isExists = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExists = (from a in ctx.tb_State
                            where a.Name == objchkState.Name
                            && a.Abbreviation == objchkState.Abbreviation
                            && a.CountryID == objchkState.CountryID
                            && a.StateID != objchkState.StateID
                            select new { a.StateID }).Count();
            }
            return isExists;
        }

        /// <summary>
        ///  For Delete State By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void Delete(int Id)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_State sta = ctx.tb_State.First(c => c.StateID == Id);
            ctx.DeleteObject(sta);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Get All Country List for Fill Country Dropdown
        /// </summary>
        /// <returns>Returns List of Country for display</returns>
        public List<tb_Country> GetAllCountry()
        {
            List<tb_Country> resultsList = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            resultsList = ctx.tb_Country.OrderBy(tb_Country => tb_Country.Name).ToList();
            return resultsList;
        }

        // For Client side Functions
        /// <summary>
        /// <param name="CountryID">Int32 CountryID</param>
        /// Get All States
        /// </summary>
        /// <returns>Returns List of All States as a Dataset</returns>
        public DataSet GetAllStates(Int32 CountryID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_State_FillState]";
            cmd.Parameters.AddWithValue("@CountryID", CountryID);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Two letter State code By Name
        /// </summary>
        /// <param name="CountryName">string StateName</param>
        /// <returns>Returns string State Code</returns>
        public string GetStateCodeByName(string StateName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_State_GetStateCodeByName";
            cmd.Parameters.AddWithValue("@Statename", StateName);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        #endregion

    }
}
