using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Xml;
using System.Web;

namespace Solution.Data
{
    /// <summary>
    ///  Common Data Access Class Contains  Common related Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public class CommonDAC
    {
        #region Declaration

        private SqlCommand cmd = null;
        private SQLAccess objSql = null;

        #endregion

        #region Key Functions

        /// <summary>
        /// Get Category name
        /// </summary>
        /// <param name="CatName">string CatName</param>
        /// <param name="ParentName">string ParentName</param>
        /// <param name="GrandName">string GrandName</param>
        /// <param name="StoreID"> int StoreID</param>
        /// <param name="CategoryID">int CategoryID</param>
        /// <param name="Mode">int Mode</param>
        /// <returns>Returns Category Dataset with Details</returns>
        public Object GetCategoryID(string CatName, string ParentName, string GrandName, int StoreID, int CategoryID, int Mode)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Global";
            cmd.Parameters.AddWithValue("@CatName", CatName);

            if (ParentName != "" && ParentName.Length > 0)
                cmd.Parameters.AddWithValue("@ParentName", ParentName);

            if (GrandName != "" && GrandName.Length > 0)
                cmd.Parameters.AddWithValue("@GrandName", GrandName);

            cmd.Parameters.AddWithValue("@StoreID", StoreID);

            if (CategoryID > 0)
                cmd.Parameters.AddWithValue("@CategoryID", CategoryID);
            cmd.Parameters.AddWithValue("@Mode", Mode);

            return objSql.ExecuteScalarQuery(cmd);
        }

        /// <summary>
        /// Get Dataset from Common SP
        /// </summary>
        /// <param name="Query">string Query</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetCommonDataSet(string Query)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Common_Dataset";
            cmd.Parameters.AddWithValue("@Query", Query);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Scalar Data From Common SP
        /// </summary>
        /// <param name="Query">string Query</param>
        /// <returns>Returns Dataset</returns>
        public Object GetScalarCommonData(string Query)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Common_Dataset";
            cmd.Parameters.AddWithValue("@Query", Query);
            return objSql.ExecuteScalarQuery(cmd);
        }

        /// <summary>
        /// Execute from Common SP
        /// </summary>
        /// <param name="Query">string Query</param>
        /// <returns>Returns Dataset </returns>
        public Object ExecuteCommonData(string Query)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Common_Dataset";
            cmd.Parameters.AddWithValue("@Query", Query);
            return objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Execute from Common SP
        /// </summary>
        /// <param name="Query">string Query</param>
        /// <returns>Returns Dataset</returns>
        public Object ExecuteDatabaseBackup(string Path)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_DatabaseBackup";
            cmd.Parameters.AddWithValue("@BackupPath", Path);
            return objSql.ExecuteNonQuery(cmd);
        }

        /// <summary>
        /// Error Log
        /// </summary>
        /// <param name="PageName">string PageName</param>
        /// <param name="Error">string Error</param>
        /// <param name="ErrorDetails">string ErrorDetails</param>
        public static void ErrorLog(string PageName, string Error, string ErrorDetails)
        {
            try
            {
                HttpContext context = HttpContext.Current;
                FileInfo info = new FileInfo(HttpContext.Current.Server.MapPath(@"/ErrorLog.xml"));

                if (info.Exists)
                {
                    try
                    {
                        long s2 = info.Length;
                        if (s2 > (long)10240000)
                        {
                            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/temp/")))
                                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/temp/"));
                            info.MoveTo(HttpContext.Current.Server.MapPath("/temp/ErrorLog_" + String.Format("{0:MM_dd_yyyy_hh_mm_ss}", Convert.ToDateTime(DateTime.Now)) + ".xml"));
                            info = new FileInfo(HttpContext.Current.Server.MapPath(@"/ErrorLog.xml"));
                        }
                    }
                    catch
                    {
                        info = new FileInfo(HttpContext.Current.Server.MapPath(@"/ErrorLog.xml"));
                    }
                }


                XmlDocument xmlDoc = new XmlDocument();
                if (!File.Exists(info.FullName))
                {

                    XmlTextWriter textWritter = new XmlTextWriter(info.FullName, null);
                    textWritter.WriteStartDocument();
                    textWritter.WriteStartElement("Root");
                    textWritter.WriteEndElement();

                    textWritter.Close();
                }
                xmlDoc.Load(info.FullName);

                XmlElement subRoot = xmlDoc.CreateElement("ErrorLogInfo");

                //DateTime
                XmlElement appendedElementDateTime = xmlDoc.CreateElement("DateTime");
                XmlText xmlTextDateTime = xmlDoc.CreateTextNode(DateTime.Now.ToString());
                appendedElementDateTime.AppendChild(xmlTextDateTime);
                subRoot.AppendChild(appendedElementDateTime);
                xmlDoc.DocumentElement.AppendChild(subRoot);

                //Page
                XmlElement appendedElementPage = xmlDoc.CreateElement("Page");
                XmlText xmlTextPage = xmlDoc.CreateTextNode(PageName);
                appendedElementPage.AppendChild(xmlTextPage);
                subRoot.AppendChild(appendedElementPage);
                xmlDoc.DocumentElement.AppendChild(subRoot);

                //Method
                XmlElement appendedElementIPAddress = xmlDoc.CreateElement("IPAddress");
                XmlText xmlTextIPAddress = xmlDoc.CreateTextNode(context.Request.UserHostAddress.ToString());
                appendedElementIPAddress.AppendChild(xmlTextIPAddress);
                subRoot.AppendChild(appendedElementIPAddress);
                xmlDoc.DocumentElement.AppendChild(subRoot);

                //Error
                XmlElement appendedElementError = xmlDoc.CreateElement("Error");
                XmlText xmlTextError = xmlDoc.CreateTextNode(Error);
                appendedElementError.AppendChild(xmlTextError);
                subRoot.AppendChild(appendedElementError);
                xmlDoc.DocumentElement.AppendChild(subRoot);

                //StackTrace
                XmlElement appendedElementStackTrace = xmlDoc.CreateElement("StackTrace");
                XmlText xmlTextStackTrace = xmlDoc.CreateTextNode(ErrorDetails);
                appendedElementStackTrace.AppendChild(xmlTextStackTrace);
                subRoot.AppendChild(appendedElementStackTrace);
                xmlDoc.DocumentElement.AppendChild(subRoot);

                //Save Doc
                xmlDoc.Save(info.FullName);
            }
            catch { }






        }

        /// <summary>
        /// Get Dataset For Export Customer List
        /// </summary>
        /// <param name="SearchValue">Search Value</param>
        /// <param name="StoreId">Store Id</param>
        /// <returns>Returns Dataset DSCommon</returns>
        public DataSet GetCustomerExport(string SearchValue, string StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Customer_Export";
            cmd.Parameters.AddWithValue("@SearchValue", SearchValue);
            cmd.Parameters.AddWithValue("@StoreId", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Columns name From Table Product for Export
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetProductColumns(int Mode, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_Export";
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        /// Get Records in Dataset For Export Product
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="Query">String Query</param>
        /// /// <param name="StoreID">int StoreID</param>
        /// <returns>Returns Dataset</returns>
        public DataSet GetProductExport(int Mode, string Query, int StoreID)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Product_Export";
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Query1", Query);
            return objSql.GetDs(cmd);
        }

        /// <summary>
        ///  Get Yahoo Report
        /// </summary>
        /// <param name="Mode">int Mode</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="Fromdate">DateTime Fromdate</param>
        /// <param name="ToDate">DateTime ToDate</param>
        /// <returns>Returns Yahoo Report using filtered Data</returns>
        public DataSet GetYahooReports(int Mode, int StoreID, DateTime Fromdate, DateTime ToDate)
        {
            objSql = new SQLAccess();
            cmd = new SqlCommand();
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = "usp_Report_YahooReports";
            cmd.Parameters.AddWithValue("@Mode", Mode);
            cmd.Parameters.AddWithValue("@StoreID", StoreID);
            cmd.Parameters.AddWithValue("@Fromdate", Fromdate);
            cmd.Parameters.AddWithValue("@ToDate", ToDate);
            return objSql.GetDs(cmd);
        }

        #endregion
    }
}
