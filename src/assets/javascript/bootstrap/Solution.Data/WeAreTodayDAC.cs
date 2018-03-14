using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Solution.Data
{
    public class WeAreTodayDAC
    {

        #region Declaration

        
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions


        
        public void InsertWeareToday(DateTime StartDate, DateTime EndDate, int CreateBy)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WeAreToday_Insert";
            cmd.Parameters.AddWithValue("@StartDate", StartDate);
            cmd.Parameters.AddWithValue("@EndDate", EndDate);
            cmd.Parameters.AddWithValue("@CreateBy", CreateBy);
            objSql.ExecuteNonQuery(cmd);
        }

        public DataSet GetWeareToday()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_WeAreToday_Get";
            return objSql.GetDs(cmd);
        }

        public int DeleteWeareToday(int WearetodayID)
        {
            int rValue = 0;
            try
            {
                objSql = new SQLAccess();
                cmd = new SqlCommand();
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "usp_WeAreToday_Delete";
                cmd.Parameters.AddWithValue("@WearetodayID", WearetodayID);
                objSql.ExecuteNonQuery(cmd);
                rValue = 1;
            }
            catch
            {
               rValue = 0;
            }
            return rValue;
        
        }
        #endregion
    }
}
