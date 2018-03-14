using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using Solution.Bussines.Entities;
namespace Solution.Data
{
    /// <summary>
    /// HeaderLink Data Access Class Contains Header Link related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class HeaderLinkDAC
    {
        #region Declaration
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
        #endregion

        #region Key Functions

        /// <summary>
        /// Get Header Links
        /// </summary>
        /// <param name="Type">string Type</param>
        /// <param name="StoreID">Int32 StoreID</param>
        /// <returns>Return Dataset - Header List</returns>
        public DataSet GetHeaderLinks(string Type, Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_HeaderLinks";
            cmd.Parameters.AddWithValue("@Type", Type);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Deletes record from tb_headerlink table
        /// </summary>
        /// <param name="ID">page Id</param>
        public void DeleteHeaderLink(Int32 Id)
        {
            tb_HeaderLinks header = ctx.tb_HeaderLinks.First(c => c.PageId == Id);
            ctx.DeleteObject(header);
            ctx.SaveChanges();
        }

        /// <summary>
        /// Get all values for specific Header Link
        /// </summary>
        /// <param name="Id">page id</param>
        /// <returns>table tb_HeaderLinks</returns>
        public tb_HeaderLinks GetHeaderLink(int Id)
        {
            tb_HeaderLinks tb_header = null;
            {
                tb_header = ctx.tb_HeaderLinks.First(e => e.PageId == Id);
            }
            return tb_header;
        }

        /// <summary>
        /// Function for updating header link
        /// </summary>
        /// <param name="tb_header">detail of header link</param>
        public void UpdateHeaderLink(tb_HeaderLinks tb_header)
        {
            ctx.SaveChanges();
        }

        /// <summary>
        /// Function for checking duplicate header link
        /// </summary>
        /// <param name="tb_header">detail of header link</param>
        /// <returns>return Integer </returns>
        public Int32 CheckDuplicateforHeaderLink(tb_HeaderLinks tb_header)
        {
            Int32 isExists = 0;
            int ID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(tb_header.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            var results = from a in ctx.tb_HeaderLinks
                          select new
                          {
                              headerlink = a,
                              store = a.tb_Store
                          };
            isExists = (from a in results
                        where a.headerlink.PageName == tb_header.PageName
                        && a.headerlink.PageURL == tb_header.PageURL
                        && a.store.StoreID == ID
                        select new { a.headerlink.PageId }).Count();
            return isExists;
        }

        /// <summary>
        /// Creates HeaderLink
        /// </summary>
        /// <param name="tb_header">table tb_header</param>
        /// <returns>Returns tb_HeaderLinks</returns>
        public tb_HeaderLinks CreateHeaderLink(tb_HeaderLinks tb_header)
        {
            try
            {
                ctx.AddTotb_HeaderLinks(tb_header);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tb_header;
        }

        #endregion
    }
}
