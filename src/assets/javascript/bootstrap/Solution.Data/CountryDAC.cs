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
    /// Country Component Class Contains Country related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CountryDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Function For Create Country
        /// </summary>
        /// <param name="objCountry">tb_Country objCountry</param>
        /// <returns>Country table </returns>
        public tb_Country Create(tb_Country objCountry)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            try
            {
                ctx.AddTotb_Country(objCountry);
                ctx.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return objCountry;
        }

        /// <summary>
        /// Update State Method for Country
        /// </summary>
        /// <param name="Country">tb_Country objCountry</param>
        public void Update(tb_Country Country)
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
        ///  Get Country Data By ID for Edit functionality
        /// </summary>
        /// <param name="Id">int Id</param>
        /// <returns>Returns a country record for edit</returns>
        public tb_Country getCountry(int Id)
        {
            tb_Country cntr = null;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                cntr = ctx.tb_Country.First(e => e.CountryID == Id);
            }
            return cntr;
        }

        /// <summary>
        /// Check Duplicate Record function for Insert and Update
        /// </summary>
        /// <param name="objchkConuntry">tb_Country objchkConuntry</param>
        /// <returns>Returns no. of record count when specified record is exist in Database</returns>
        public Int32 CheckDuplicate(tb_Country objchkConuntry)
        {
            Int32 isExeist = 0;

            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            {
                isExeist = (from a in ctx.tb_Country
                            where a.Name == objchkConuntry.Name
                            && a.TwoLetterISOCode == objchkConuntry.TwoLetterISOCode
                            && a.CountryID != objchkConuntry.CountryID
                            select new { a.CountryID }).Count();
            }
            return isExeist;
        }

        /// <summary>
        /// For Delete Country By ID
        /// </summary>
        /// <param name="Id">int Id</param>
        public void Delete(int Id)
        {
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_Country cust = ctx.tb_Country.First(c => c.CountryID == Id);
            ctx.DeleteObject(cust);
            ctx.SaveChanges();
        }

        //For Client Functions
        /// <summary>
        /// Get All Countries
        /// </summary>
        /// <returns>List of All Countries as a Dataset</returns>
        public DataSet GetAllCountries()
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "[usp_Country_FillCountry]";
            cmd.Parameters.AddWithValue("@Mode", 1);
            return objSql.GetDs(cmd);
        }
        /// <summary>
        /// Get Two letter Country Code by name
        /// </summary>
        /// <param name="CountryName">string CountryName</param>
        /// <returns>Returns Country Code</returns>
        public string GetCountryCodeByName(string CountryName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Country_GetCountryCodeByName";
            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Two letter Country Code by name
        /// </summary>
        /// <param name="CountryName">string CountryName</param>
        /// <returns>Returns Country Code</returns>
        public string GetTwoCountryCodeByName(string CountryName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Country_GetCountryTwoCodeByName";
            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Three letter Country Code by name
        /// </summary>
        /// <param name="CountryName">string CountryName</param>
        /// <returns>Returns Three Letter ISO Code for Country</returns>
        public string GetCountryThreeLetterISOCodeByName(string CountryName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Country_GetCountryThreeLetterISOCodeByName";
            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Two letter Country Code by name For shipping Label
        /// </summary>
        /// <param name="CountryName">string CountryName</param>
        /// <returns>Returns string Country Code</returns>
        public string GetCountryCodeByNameForShippingLabel(string CountryName)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Country_GetCountryCodeByNameForShippingLabel";
            cmd.Parameters.AddWithValue("@CountryName", CountryName);
            cmd.Parameters.AddWithValue("@Mode", 1);
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }

        /// <summary>
        /// Get Country name for shipping label
        /// </summary>
        /// <param name="CountryID">Int32 CountryID</param>
        /// <returns>Returns Country name for shipping label</returns>
        public string GetCountryNameByCodeForShippingLabel(Int32 CountryID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandText = "select Name from tb_country where CountryID=" + CountryID;
            return Convert.ToString(objSql.ExecuteScalarQuery(cmd));
        }
        #endregion
    }
}
