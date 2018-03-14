using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Solution.Bussines.Entities;
using System.Diagnostics;
using System.Data;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// QuantityDiscountDAC -This Class Contains Quantity Discount related Data Logic Functions 
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>/// 

    public class QuantityDiscountDAC
    {
        tb_QauntityDiscount QauntityDiscount = null;
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        /// <summary>
        /// Get Particular Record from Quantity Discount for QauntityDiscountID
        /// </summary>
        /// <param name="QauntityDiscountID">int QauntityDiscountID</param>
        /// <returns>Return Quantity Discount Table Object</returns>
        public tb_QauntityDiscount GetQauntityDiscount(int QauntityDiscountID)
        {
            QauntityDiscount = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                QauntityDiscount = ctx.tb_QauntityDiscount.First(e => e.QuantityDiscountID == QauntityDiscountID);
            }
            return QauntityDiscount;
        }

        /// <summary>
        ///  For Updating Quantity Discount
        /// </summary>
        /// <param name="QauntityDiscount">Return Quantity Discount Table Object</param>
        public void Update(tb_QauntityDiscount QauntityDiscount)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            ctx.SaveChanges();
        }

        /// <summary>
        /// For Checking Duplicate Quantity Discount Name at the time of Inserting/Adding new Tax class
        /// </summary>
        /// <param name="tbltaxclass">Table Quantity Discount</param>
        /// <returns>Return row count if exist</returns>
        public int CheckDuplicate(tb_QauntityDiscount tblQauntityDiscount)
        {
            Int32 isExists = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExists = (from a in ctx.tb_QauntityDiscount
                            where a.Name == tblQauntityDiscount.Name
                            select new { a.QuantityDiscountID }).Count();
            }
            return isExists;
        }

        /// <summary>
        /// Create QauntityDiscount For Inserting/Adding new Quantity Discount
        /// </summary>
        /// <param name="tblQauntityDiscount">tb_QauntityDiscount tblQauntityDiscount</param>
        /// <returns>Returns the Quantity discount table object</returns>
        public tb_QauntityDiscount CreateQauntityDiscount(tb_QauntityDiscount tblQauntityDiscount)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_QauntityDiscount(tblQauntityDiscount);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tblQauntityDiscount;
        }

        /// <summary>
        /// Get the Quantity discount list
        /// </summary>
        /// <param name="QauntityDiscountID">int QauntityDiscountID</param>
        /// <returns>Returns the quantity discount Table object</returns>
        public List<tb_QuantityDiscountTable> GetQauntityDiscountnew(int QauntityDiscountID)
        {
            List<tb_QuantityDiscountTable> lstobj = new List<tb_QuantityDiscountTable>();

            //    QauntityDiscount = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                lstobj = ctx.tb_QuantityDiscountTable.Where(e => e.QuantityDiscountID == QauntityDiscountID).ToList();
            }
            return lstobj;
        }

        /// <summary>
        /// Add Quantity Discount Table 
        /// </summary>
        /// <param name="QuantityDiscountID">int QuantityDiscountID</param>
        /// <param name="LowQuantity">int LowQuantity</param>
        /// <param name="HighQuantity">int HighQuantity</param>
        /// <param name="DiscountPercent">decimal DiscountPercent</param>
        /// <returns>Returns the Identity Value</returns>
        public int AddQuantityDiscountTable(int QuantityDiscountID, int LowQuantity, int HighQuantity, decimal DiscountPercent)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_QuantityDiscount_UpdateQuantityDiscountTable";

            cmd.Parameters.Add("@QuantityDiscountID", SqlDbType.Int).Value = QuantityDiscountID;
            cmd.Parameters.Add("@LowQuantity", SqlDbType.Int).Value = LowQuantity;
            cmd.Parameters.Add("@HighQuantity", SqlDbType.Int).Value = HighQuantity;
            cmd.Parameters.Add("@DiscountPercent", SqlDbType.Money).Value = DiscountPercent;
            cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = 1;
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            object res = objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);

        }


        /// <summary>
        /// Update Quantity Discount Table
        /// </summary>
        /// <param name="QuantityDiscountTableID">int QuantityDiscountTableID</param>
        /// <param name="QuantityDiscountID">int QuantityDiscountID</param>
        /// <param name="LowQuantity">int LowQuantity</param>
        /// <param name="HighQuantity">int HighQuantity</param>
        /// <param name="DiscountPercent">decimal DiscountPercent</param>
        /// <returns>Returns the ID which is updated</returns>
        public int UpdateQuantityDiscountTable(int QuantityDiscountTableID, int QuantityDiscountID, int LowQuantity, int HighQuantity, decimal DiscountPercent)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_QuantityDiscount_UpdateQuantityDiscountTable";
            cmd.Parameters.Add("@QuantityDiscountTableID", SqlDbType.Int).Value = QuantityDiscountTableID;
            cmd.Parameters.Add("@QuantityDiscountID", SqlDbType.Int).Value = QuantityDiscountID;
            cmd.Parameters.Add("@LowQuantity", SqlDbType.Int).Value = LowQuantity;
            cmd.Parameters.Add("@HighQuantity", SqlDbType.Int).Value = HighQuantity;
            cmd.Parameters.Add("@DiscountPercent", SqlDbType.Money).Value = DiscountPercent;
            cmd.Parameters.Add("@Mode", SqlDbType.Int).Value = 2;
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            object res = objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Quantity Discount
        /// </summary>
        /// <param name="QuantityDiscountID">Int32 QuantityDiscountID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteQauntityDiscount(Int32 QuantityDiscountID)
        {
            objSql = new SQLAccess();
            SqlCommand myCommand = new SqlCommand("usp_QuantityDiscount_DeleteQuantityDiscount");
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add("@QuantityDiscountID", SqlDbType.Int).Value = QuantityDiscountID;

            myCommand.Parameters.Add("@Mode", SqlDbType.Int).Value = 1;
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(paramReturnval);
            object res = objSql.ExecuteNonQuery(myCommand);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Delete Quantity Discount Table
        /// </summary>
        /// <param name="QuantityDiscountTableID">Int32 QuantityDiscountTableID</param>
        /// <returns>Returns 1 if Deleted</returns>
        public int DeleteQuantityDiscountTable(Int32 QuantityDiscountTableID)
        {
            objSql = new SQLAccess();
            SqlCommand myCommand = new SqlCommand("usp_QuantityDiscount_UpdateQuantityDiscountTable");
            myCommand.CommandType = CommandType.StoredProcedure;
            myCommand.Parameters.Add("@QuantityDiscountTableID", SqlDbType.Int).Value = QuantityDiscountTableID;

            myCommand.Parameters.Add("@Mode", SqlDbType.Int).Value = 3;
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            myCommand.Parameters.Add(paramReturnval);
            object res = objSql.ExecuteNonQuery(myCommand);
            return Convert.ToInt32(paramReturnval.Value);
        }

        /// <summary>
        /// Check Duplicate for update
        /// </summary>
        /// <param name="tblQauntityDiscount">tb_QauntityDiscount tblQauntityDiscount</param>
        /// <param name="oldname">string oldname</param>
        /// <returns>Returns  the row count if exists</returns>
        public int CheckDuplicateforupdate(tb_QauntityDiscount tblQauntityDiscount, string oldname)
        {
            Int32 isExists = 0;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExists = (from a in ctx.tb_QauntityDiscount
                            where a.Name == tblQauntityDiscount.Name && a.Name != oldname
                            select new { a.QuantityDiscountID }).Count();
            }
            return isExists;
        }

    }
}
