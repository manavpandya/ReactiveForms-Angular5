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
    public class SubjectStatusDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        /// <summary>
        ///  Get SubjectStatus Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns a SubjectStatus record for edit</returns>
        public tb_ContactEmail getSubjectStatus(int Id)
        {
            tb_ContactEmail cntr = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                cntr = ctx.tb_ContactEmail.First(e => e.ID == Id);
            }
            return cntr;
        }


        /// <summary>
        /// Check Duplicate Record function for Insert and Update
        /// </summary>
        /// <param name="objchkConuntry">tb_SubjectStatus objchkConuntry</param>
        /// <returns>Returns no. of record count when specified record is exist in Database</returns>
        public Int32 CheckDuplicate(tb_ContactEmail objchkConuntry)
        {
            Int32 isExeist = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExeist = (from a in ctx.tb_ContactEmail
                            where a.Subject == objchkConuntry.Subject
                           
                            && a.ID != objchkConuntry.ID
                            select new { a.ID }).Count();
            }
            return isExeist;
        }


        /// <summary>
        /// Update State Method for SubjectStatus
        /// </summary>
        /// <param name="SubjectStatus">tb_SubjectStatus objSubjectStatus</param>
        public void Update(tb_ContactEmail SubjectStatus)
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
        /// Function For Create SubjectStatus
        /// </summary>
        /// <param name="objSubjectStatus">tb_SubjectStatus objSubjectStatus</param>
        /// <returns>SubjectStatus table </returns>
        public tb_ContactEmail Create(tb_ContactEmail objSubjectStatus)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_ContactEmail(objSubjectStatus);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objSubjectStatus;
        }
    }
}
