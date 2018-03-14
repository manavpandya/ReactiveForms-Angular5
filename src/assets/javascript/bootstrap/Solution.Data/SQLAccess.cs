using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace Solution.Data
{
    /// <summary>
    /// SQL Access Contains commonly used Data Logic Functions     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    public class SQLAccess
    {
        public SQLAccess()
        {
            conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"]);
        }

        #region Declaration

        private SqlConnection conn = null;
        public SqlCommand cmd;
        public DataSet ds;

        #endregion

        #region Key Functions

        public bool ExecuteNonQuery(string Query)
        {

            bool result;
            cmd = new SqlCommand(Query, conn);

            try
            {
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                int index = cmd.ExecuteNonQuery();
                if (index != 1)
                {
                    result = false;
                }
                else
                {
                    result = true;
                }
            }

            catch (Exception ex)
            {
                result = false;
                HttpContext context = HttpContext.Current;
                CommonDAC.ErrorLog("SQLAccess.cs", ex.Message, ex.StackTrace);
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
                cmd.Dispose();
            }
            return result;
        }

        public Object ExecuteNonQuery(SqlCommand cmd)
        {
            Object Obj = new Object();
            try
            {
                cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                Obj = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Obj = null;
                HttpContext context = HttpContext.Current;
                CommonDAC.ErrorLog("SQLAccess.cs", ex.Message, ex.StackTrace);
                if (HttpContext.Current.Request.Url != null)
                {
                    CommonDAC.ErrorLog("Error Query=" + cmd.CommandText.ToString(), ex.Message, ex.StackTrace + HttpContext.Current.Request.Url.ToString());
                }
                else
                {
                    CommonDAC.ErrorLog("Error Query=" + cmd.CommandText.ToString(), ex.Message, ex.StackTrace);
                }

            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
                cmd.Dispose();
            }
            return Obj;
        }

        public Object ExecuteScalarQuery(string Query)
        {
            Object Obj = new Object();
            try
            {
                cmd = new SqlCommand();
                cmd.CommandText = Query;
                Obj = ExecuteScalarQuery(cmd);
            }
            catch (Exception ex)
            {
                Obj = null;
                HttpContext context = HttpContext.Current;
                CommonDAC.ErrorLog("SQLAccess.cs", ex.Message, ex.StackTrace);
                if (HttpContext.Current.Request.Url != null)
                {
                    CommonDAC.ErrorLog("Error Query=" + Query.ToString(), ex.Message, ex.StackTrace + HttpContext.Current.Request.Url.ToString());
                }
                else
                {
                    CommonDAC.ErrorLog("Error Query=" + Query.ToString(), ex.Message, ex.StackTrace);
                }
                
            }
            finally
            {
                
                cmd.Dispose();
            }
            return Obj;
        }

        public Object ExecuteScalarQuery(SqlCommand cmd)
        {
            Object Obj = new Object();
            try
            {
                cmd.Connection = conn;
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                Obj = cmd.ExecuteScalar();
            }
            catch (Exception ex)
            {
                Obj = null;
                HttpContext context = HttpContext.Current;
                CommonDAC.ErrorLog("SQLAccess.cs", ex.Message, ex.StackTrace);
                if (cmd.Parameters[0] != null)
                {
                    if (HttpContext.Current.Request.Url != null)
                    {
                        CommonDAC.ErrorLog("Sql Query", "", cmd.Parameters[0].Value.ToString() + HttpContext.Current.Request.Url.ToString());
                    }
                    else
                    {
                        CommonDAC.ErrorLog("Sql Query", "", cmd.Parameters[0].Value.ToString());
                    }
                    
                }
                else
                {
                    CommonDAC.ErrorLog("Sql Query", "", cmd.CommandText.ToString());
                }
                
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
                cmd.Dispose();
            }
            return Obj;
        }

        public DataSet GetDs(string Sql)
        {
            SqlDataAdapter Adpt = new SqlDataAdapter();
            try
            {
                ds = new DataSet();
                cmd = new SqlCommand();
                if (conn.State == ConnectionState.Closed)
                    conn.Open();
                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = Sql;
                Adpt.SelectCommand = cmd;
                Adpt.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
                HttpContext context = HttpContext.Current;
                CommonDAC.ErrorLog("SQLAccess.cs", ex.Message, ex.StackTrace);
                CommonDAC.ErrorLog("Sql Query", "",Sql.ToString());
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
                cmd.Dispose();
                Adpt.Dispose();
                
            }
            return ds;
        }

        public DataSet GetDs(SqlCommand cmd)
        {
            SqlDataAdapter Adpt = new SqlDataAdapter();
            try
            {
                ds = new DataSet();
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.Connection = conn;

                Adpt.SelectCommand = cmd;
                Adpt.Fill(ds);
            }
            catch (Exception ex)
            {
                ds = null;
                HttpContext context = HttpContext.Current;
                CommonDAC.ErrorLog("SQLAccess.cs", ex.Message, ex.StackTrace);
                CommonDAC.ErrorLog("Sql Query", "", cmd.CommandText.ToString());
                if (conn.State == ConnectionState.Open)
                    conn.Close();
            }
            finally
            {
                if (conn != null)
                    if (conn.State == ConnectionState.Open) conn.Close();
                cmd.Dispose();
                Adpt.Dispose();
            }
            return ds;
        }

        #endregion

    }
}
