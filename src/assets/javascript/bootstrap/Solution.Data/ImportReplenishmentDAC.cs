using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Entities;
using System.Diagnostics;

namespace Solution.Data
{
   public class ImportReplenishmentDAC
    {
       
       #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion


        /// <summary>
        /// Insert Replenishment File Detail
        /// </summary>
        /// <param name="FileName">String FileName</param>
        /// <param name="CreatedBy">Int32 CreatedBy</param>
        /// <param name="Mode">Int32 Mode</param>
        /// <returns>Returns Identity Values</returns>
        public Int32 InsertReplenishmentFileLOg(String FileName, Int32 CreatedBy, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ReplenishmentFileLog";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@FileName", FileName);
            cmd.Parameters.AddWithValue("@CreatedBy", CreatedBy);
            cmd.Parameters.AddWithValue("@Mode", Mode);
           
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }




        /// <summary>
        /// Update Replenishment File Name
        /// </summary>
        /// <param name="FileName">String FileName</param>
        /// <param name="FileID">Int32 FileID</param>
        /// <param name="Mode">Int32 Mode</param>
       
        public Int32 UpdateReplenishmentFileName(String FileName, Int32 FileID, Int32 Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ReplenishmentFileLog";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@FileName", FileName);
            cmd.Parameters.AddWithValue("@FileID", FileID);
            cmd.Parameters.AddWithValue("@Mode", Mode);

            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// get last updated Replenishment File data
        /// </summary>
       public DataSet GetLastFileModified()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ReplenishmentFileLog";
            cmd.Parameters.AddWithValue("@Mode", 3);
          
            return objSql.GetDs(cmd);
            
        }

    }
}
