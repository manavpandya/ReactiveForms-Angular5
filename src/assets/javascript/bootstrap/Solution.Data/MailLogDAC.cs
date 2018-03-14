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
    /// <summary>
    /// Mail Log Data Access Class Contains Mail Log related Data Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 

    public class MailLogDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions
        /// <summary>
        ///  Insert an email entry in Mail log table
        /// </summary>
        /// <param name="tb_MailLog">tb_MailLog tb_MailLog</param>
        /// <returns>Return a record recently inserted as a table form</returns>
        public tb_MailLog InsertMailLog(tb_MailLog tb_MailLog)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_MailLog(tb_MailLog);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            return tb_MailLog;
        }

        /// <summary>
        /// Insert Availability Notification
        /// </summary>
        /// <param name="tb_Availability">tb_AvailabilityNotification tb_Availability</param>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Values</returns>
        public Int32 InsertAvailabilityNotification(tb_AvailabilityNotification tb_Availability, Int32 ProductID, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AvailabilityNotification";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@ProductID", ProductID);
            cmd.Parameters.AddWithValue("@FirstName", tb_Availability.FirstName);
            cmd.Parameters.AddWithValue("@LastName", tb_Availability.LastName);
            cmd.Parameters.AddWithValue("@Telephone", tb_Availability.Telephone);
            cmd.Parameters.AddWithValue("@Email", tb_Availability.Email);
            cmd.Parameters.AddWithValue("@MailSent", tb_Availability.MailSent);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Update Availability Notification for MailSent and MailID
        /// </summary>
        /// <param name="tb_Availability">tb_AvailabilityNotification tb_Availability</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 = if Updated</returns>
        public Int32 UpdateAvailabilityNotification(tb_AvailabilityNotification tb_Availability, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_AvailabilityNotification";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@AvailabilityID", tb_Availability.AvailabilityID);
            cmd.Parameters.AddWithValue("@MailID", tb_Availability.MailID);
            cmd.Parameters.AddWithValue("@MailSent", tb_Availability.MailSent);
            cmd.Parameters.AddWithValue("@Mode", 2);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        #endregion

    }
}
