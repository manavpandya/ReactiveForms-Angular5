using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;

namespace Solution.Data
{
    /// <summary>
    /// EmailTemplate Data Access Class Contains Email Template related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class EmailTemplateDAC
    {
        #region KeyFunctions

        /// <summary>
        /// Create New Email Template
        /// </summary>
        /// <param name="EmailTemplate">tb_EmailTemplate EmailTemplate</param>
        /// <returns>A tb_EmailTemplate entity</returns>
        public tb_EmailTemplate Create(tb_EmailTemplate EmailTemplate)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_EmailTemplate(EmailTemplate);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {   
                throw ex;
            }
            return EmailTemplate;
        }

        /// <summary>
        /// Get Email Template by ID
        /// </summary>
        /// <param name="TemplateID">int TemplateID</param>
        /// <returns>Returns EmailTemplate by List </returns>
        public tb_EmailTemplate GetEmailTemplateByID(int TemplateID)
        {
            tb_EmailTemplate template = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                template = ctx.tb_EmailTemplate.First(e=>e.TemplateID==TemplateID);
            }
            return template;
        }

        /// <summary>
        /// Update Email template
        /// </summary>
        /// <param name="template">tb_EmailTemplate template</param>
        /// <returns>Returns No of Rows Affected</returns>
        public int Update(tb_EmailTemplate EmailTemplate)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int RowsAffected = 0;
            try
            {
                RowsAffected = ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw ex;
            }
            return RowsAffected;
        }

        /// <summary>
        /// Check whether Email Template already exists or not
        /// </summary>
        /// <param name="EmailTemplate">tb_EmailTemplate EmailTemplate</param>
        /// <returns>count to identify whether Email Template already exists or not: count>0 already exists</returns>
        public int CheckDuplicate(tb_EmailTemplate EmailTemplate)
        {
            Int32 isExists = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(EmailTemplate.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            isExists = (from a in ctx.tb_EmailTemplate
                        where a.Label == EmailTemplate.Label && a.TemplateID != EmailTemplate.TemplateID && a.tb_Store.StoreID == ID
                        select new { a.TemplateID }
                           ).Count();
            return isExists;
        }
        #endregion
    }
}
