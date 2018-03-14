using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Entities;


namespace Solution.Data
{
    /// <summary>
    /// ContactUs Data Access Class Contains Contact Us related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class ContactUsDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Insert Contact Us Details into table
        /// </summary>
        /// <param name="tb_ContactUs">tb_ContactUs tb_ContactUs</param>
        /// <returns>Returns New Generated Row Id from tb_ContactUs table</returns>
        public Int32 InsertContactUsDetail(tb_ContactUs tb_ContactUs)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_ContactUs";
            SqlParameter paramReturnval = new SqlParameter("@Returnval", SqlDbType.Int, 4);
            paramReturnval.Direction = ParameterDirection.ReturnValue;
            cmd.Parameters.Add(paramReturnval);
            cmd.Parameters.AddWithValue("@ContactUsID", tb_ContactUs.ContactUsID);
            cmd.Parameters.AddWithValue("@Subject", tb_ContactUs.Subject);
            cmd.Parameters.AddWithValue("@Name", tb_ContactUs.Name);
            cmd.Parameters.AddWithValue("@Email", tb_ContactUs.Email);
            cmd.Parameters.AddWithValue("@Message", tb_ContactUs.Message);
            cmd.Parameters.AddWithValue("@Country", tb_ContactUs.Country);
            cmd.Parameters.AddWithValue("@City", tb_ContactUs.City);
            cmd.Parameters.AddWithValue("@State", tb_ContactUs.State);
            cmd.Parameters.AddWithValue("@PhoneNumber", tb_ContactUs.PhoneNumber);
            cmd.Parameters.AddWithValue("@ZipCode", tb_ContactUs.ZipCode);
            cmd.Parameters.AddWithValue("@ContactDate", tb_ContactUs.ContactDate);
            cmd.Parameters.AddWithValue("@Address", tb_ContactUs.Address);
            cmd.Parameters.AddWithValue("@Quoteid", tb_ContactUs.Quoteid);
            cmd.Parameters.AddWithValue("@Mode", 1);
            objSql.ExecuteNonQuery(cmd);
            return Convert.ToInt32(paramReturnval.Value);
        }
        #endregion
    }
}
