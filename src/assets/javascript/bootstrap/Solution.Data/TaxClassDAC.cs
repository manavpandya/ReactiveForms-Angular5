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
    /// Tex Class DAC-This Class Contains Tex Class related Data Logic Functions.     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class TaxClassDAC
    {
        private SqlCommand cmd = null;
        private SQLAccess objSql = null;
        tb_TaxClass taxclass = null;

        /// <summary>
        /// Get Tax CLass For Getting Particular Record From Table by Id
        /// </summary>
        /// <param name="StoreID">int StoreId</param>
        /// <returns>Returns Tax Records as a Dataset</returns>
        public DataSet GetTaxClassByStoreID(Int32 StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_TaxClass_GetTaxClassByStoreID";
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Tax CLass For Getting Particular Record From Table by Id
        /// </summary>
        /// <param name="taxclassID">int taxclassID</param>
        /// <returns>Return table Tax class List</returns>
        public tb_TaxClass GetTaxClass(int taxclassID)
        {
            taxclass = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                taxclass = ctx.tb_TaxClass.First(e => e.TaxClassID == taxclassID);
            }
            return taxclass;
        }

        /// <summary>
        /// For Updating Tax Class
        /// </summary>
        /// <param name="tbltaxclass">Table Tax class</param>
        public void Update(tb_TaxClass taxclass)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            ctx.SaveChanges();
        }

        /// <summary>
        /// For Checking Duplicate Tax Class Name and Code at the time of Inserting/Adding new Tax class
        /// </summary>
        /// <param name="tbltaxclass">tb_TaxClass tbltaxclass</param>
        /// <returns>Return Boolean Value True/False<</returns>
        public int CheckDuplicate(tb_TaxClass tbltaxclass)
        {
            Int32 isExists = 0;
            int StoreID = Convert.ToInt32(((System.Data.Objects.DataClasses.EntityReference)(tbltaxclass.tb_StoreReference)).EntityKey.EntityKeyValues[0].Value);
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExists = (from a in ctx.tb_TaxClass
                            where a.Deleted == false && (a.TaxCode == tbltaxclass.TaxCode || a.TaxName == tbltaxclass.TaxName) &&
                           a.tb_Store.StoreID == StoreID
                            select new { a.TaxClassID }).Count();
            }
            return isExists;
        }

        /// <summary>
        /// Create Tax Class For Inserting/Adding new Tax class
        /// </summary>
        /// <param name="tbltaxclasss">tb_TaxClass tbltaxclasss</param>
        /// <returns>Return Id of the Created/Added Record</returns>
        public tb_TaxClass Createtaxclass(tb_TaxClass tbltaxclasss)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            try
            {
                ctx.AddTotb_TaxClass(tbltaxclasss);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tbltaxclasss;
        }


        #region Method and Fuction for State Tax Rate

        /// <summary>
        ///Update State Tax in the Store if it exist else Insert new State Tax
        /// </summary>
        /// <param name="StateID">int StateID</param>
        /// <param name="TaxClassID">int TaxClassID</param>
        /// <param name="TaxRate">decimal TaxRate</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Return 1 or 0</returns>
        public int EditStateTax(int StateID, int TaxClassID, decimal TaxRate, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_StateTax";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            cmd.Parameters.AddWithValue("@TaxClassID", TaxClassID);
            cmd.Parameters.AddWithValue("@TaxRate", TaxRate);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            int index = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
            return index;
        }

        /// <summary>
        ///Delete State Tax From the Store with Particular StateID
        /// </summary>
        /// <param name="StateID">StateID</param>
        /// <param name="Mode">Mode</param>
        /// <returns>Return 1 or 0</returns>
        public int DeleteStateTax(Int32 StateID, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_StateTax";
            cmd.Parameters.AddWithValue("@StateID", StateID);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            int index = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
            return index;
        }

        /// <summary>
        /// Get StateTax For Getting all StateTax From Table
        /// </summary>
        /// <param name="Mode">Mode</param>
        /// <returns>Return State Tax list Dataset</returns>
        public DataSet GetStateTax(int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_StateTax";
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All State
        /// </summary>
        /// <param name="Mode">Mode</param>
        /// <returns>return State list as a Dataset</returns>
        public DataSet GetState(int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_StateTax";
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        #endregion

        #region  Method and Fuction for Zip Code Tax Rate
        /// <summary>
        /// Get ZipCodeTax For Getting all ZipTax From Table
        /// </summary>
        /// <param name="Mode">Mode</param>
        /// <returns>Returns the Zip code tax list as a DataSet</returns>
        public DataSet GetZipCodeTax(int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ZipCodeTax";
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get All Zip Code
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns the Zip code list as a DataSet</returns>
        public DataSet GetZipCode(int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ZipCodeTax";
            cmd.Parameters.AddWithValue("@Mode", Mode);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// AddZipTax For Adding/Updating Zip Code and Tax Rate
        /// </summary>
        /// <param name="OldZipCode">String Old Zip Code</param>
        /// <param name="ZipTaxID">int ZipTax ID</param>
        /// <param name="ZipCode">String  ZipCode</param>
        /// <param name="TaxClassID">int TaxClassID</param>
        /// <param name="TaxRate">decimal Tax Rate</param>
        /// <param name="Mode">Mode</param>
        /// <returns>Result in int</returns>
        public int AddZipTax(string OldZipCode, int ZipTaxID, string ZipCode, int TaxClassID, decimal TaxRate, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ZipCodeTax";
            cmd.Parameters.AddWithValue("@ZipTaxID", ZipTaxID);
            cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
            cmd.Parameters.AddWithValue("@TaxClassID", TaxClassID);
            cmd.Parameters.AddWithValue("@TaxRate", TaxRate);
            cmd.Parameters.AddWithValue("@OldZipCode", OldZipCode);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            int index = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
            return index;
        }

        /// <summary>
        /// Delete ZipTax For Deleting ZipCode and Tax Rate
        /// </summary>
        /// <param name="ZipCode">string ZipCode</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Result 1 if Deleted</returns>
        public int DeleteZipTax(string ZipCode, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ZipCodeTax";
            cmd.Parameters.AddWithValue("@ZipCode", ZipCode);
            cmd.Parameters.AddWithValue("@Mode", Mode);
            int index = Convert.ToInt32(objSql.ExecuteNonQuery(cmd));
            return index;
        }

        #endregion
    }


}
