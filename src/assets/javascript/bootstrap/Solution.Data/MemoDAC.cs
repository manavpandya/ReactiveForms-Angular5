using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;

namespace Solution.Data
{
    /// <summary>
    /// Memo Data Access Class Contains Memo Related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class MemoDAC
    {

        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion


        #region Key Functions

        /// <summary>
        /// Insert new Memo
        /// </summary>
        /// <param name="Memo">tb_Memo Memo</param>
        /// <returns>Returns the tb_Memo Object</returns>
        public tb_Memo Create(tb_Memo Memo)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_Memo(Memo);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Memo;
        }

        /// <summary>
        /// Insert Memo Reply
        /// </summary>
        /// <param name="Memo">tb_MemoReply MemoReply</param>
        /// <returns>Returns the tb_MemoReply Object</returns>
        public tb_MemoReply CreateMemoReply(tb_MemoReply MemoReply)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_MemoReply(MemoReply);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return MemoReply;
        }

        #endregion

        //For Client Functions
        /// <summary>
        /// Get All Memo
        /// </summary>
        /// <returns>List of All Memo as a Dataset</returns>
        public DataSet GetMemoByID(int MemoID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Memo_GetMemoByID";
            if (MemoID > 0)
                cmd.Parameters.AddWithValue("@MemoID", MemoID);
            return objSql.GetDs(cmd);
        }


        //For Client Functions
        /// <summary>
        /// Get All Memo
        /// </summary>
        /// <returns>List of All Memo as a Dataset</returns>
        public DataSet GetMemoByuserID(int UserID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Memo_GetMemoByUserID";
            if (UserID > 0)
                cmd.Parameters.AddWithValue("@UserID", UserID);
            return objSql.GetDs(cmd);
        }


        //For Client Functions
        /// <summary>
        /// Get All Memo
        /// </summary>
        /// <returns>List of All Memo Reply as a Dataset</returns>
        public DataSet GetMemoReply(int MemoID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Memo_GetMemoReply";
            if (MemoID > 0)
                cmd.Parameters.AddWithValue("@MemoID", MemoID);
            return objSql.GetDs(cmd);
        }




    }
}
