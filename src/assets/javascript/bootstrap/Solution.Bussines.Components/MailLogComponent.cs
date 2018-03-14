using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Data;
using Solution.Data;
using Solution.Bussines.Entities;
using System.Data.Objects;
using Solution.Bussines.Components.Common;


namespace Solution.Bussines.Components
{
    /// <summary>
    /// MailLog Component Class Contains MailLog related Business Logic function     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class MailLogComponent
    {

        #region Declaration

        public Int32 StoreID = 0;
        MailLogDAC objMailLog = null;
        #endregion

        #region All Constructors
        /// <summary>
        /// Constructor with out parameter that initializes StoreID when component file is loading
        /// </summary>
        public MailLogComponent()
        {
            StoreID = AppConfig.StoreID;
        }

        #endregion

        #region  Key Functions
        /// <summary>
        /// Insert an email entry in Mail log table
        /// </summary>
        /// <param name="tb_MailLog">tb_MailLog tb_MailLog</param>
        /// <returns>Return an Identity value of recently inserted mail log record</returns>
        public Int32 InsertMailLog(tb_MailLog tb_MailLog)
        {
            int isAdded = 0;

            try
            {
                MailLogDAC objMaillog = new MailLogDAC();
                objMaillog.InsertMailLog(tb_MailLog);
                isAdded = tb_MailLog.MailID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return isAdded;
        }

        /// <summary>
        /// Insert Availability Notification
        /// </summary>
        /// <param name="tb_Availability">tb_AvailabilityNotification tb_Availability</param>
        /// <param name="ProductID">Int32 ProductID</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns Identity Values</returns>
        public Int32 InsertAvailabilityNotification(tb_AvailabilityNotification tb_Availability, Int32 ProductID)
        {
            objMailLog = new MailLogDAC();
            return objMailLog.InsertAvailabilityNotification(tb_Availability, ProductID, StoreID);
        }

        /// <summary>
        /// Update Availability Notification for MailSent and MailID
        /// </summary>
        /// <param name="tb_Availability">tb_AvailabilityNotification tb_Availability</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Returns 1 = if Updated</returns>
        public Int32 UpdateAvailabilityNotification(tb_AvailabilityNotification tb_Availability)
        {
            objMailLog = new MailLogDAC();
            return objMailLog.UpdateAvailabilityNotification(tb_Availability, StoreID);
        }

        #endregion
    }
}
