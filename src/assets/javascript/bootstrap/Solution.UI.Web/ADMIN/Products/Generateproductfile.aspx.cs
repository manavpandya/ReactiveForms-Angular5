using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Data;
using System.Text;
using System.Data;
using Solution.Bussines.Components;
using System.IO;
using System.Data.SqlClient;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class Generateproductfile : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
            sFieldValueToEscape = sFieldValueToEscape.Replace("\\r\\n", System.Environment.NewLine);
            if (sFieldValueToEscape.Contains(","))
            {
                if (sFieldValueToEscape.Contains("\""))
                {
                    return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
                }
                else
                {
                    return "\"" + sFieldValueToEscape + "\"";
                }
            }
            else
            {
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";
            }
        }
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }
        private void GetFeeddata()
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();

            dsorder = CommonComponent.GetCommonDataSet("EXEC [GetproductExportFeed]");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {

                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }
            }

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "features0")
                        {
                            args[c] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i][c].ToString().Trim(), @"<[^>]*>", String.Empty));

                            if (!string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString().Trim()))
                            {
                                string[] stralali = System.Text.RegularExpressions.Regex.Split(dsorder.Tables[0].Rows[i][c].ToString().Trim(), "<li>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                Int32 iTotal = 0;
                                if (stralali.Length > 0)
                                {

                                    for (int k = 0; k < stralali.Length; k++)
                                    {
                                        if (!string.IsNullOrEmpty(System.Text.RegularExpressions.Regex.Replace(stralali[k].ToString().Trim(), @"<[^>]*>", String.Empty)))
                                        {
                                            iTotal = iTotal + 1;
                                            if (iTotal > 14)
                                            {

                                                break;
                                            }
                                            else
                                            {
                                                args[c + iTotal] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(stralali[k].ToString().Trim(), @"<[^>]*>", String.Empty));
                                            }

                                        }

                                    }
                                }
                                if (iTotal > 14)
                                {
                                    c = c + 14;
                                }
                                else
                                {
                                    c = c + Convert.ToInt32(iTotal);
                                }

                            }
                        }
                        else if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "description")
                        {
                            args[c] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i][c].ToString().Trim(), @"<[^>]*>", String.Empty));
                        }
                        else if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "main image")
                        {
                            if (!string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString().Trim()))
                            {
                                if (System.IO.File.Exists(Server.MapPath("/resources/halfpricedraps/product/large/" + dsorder.Tables[0].Rows[i][c].ToString().Trim())))
                                {
                                    args[c] = "https://www.halfpricedrapes.com/resources/halfpricedraps/product/large/" + dsorder.Tables[0].Rows[i][c].ToString().Trim();
                                }
                                else
                                {
                                    args[c] = "";
                                }
                                string flname = dsorder.Tables[0].Rows[i][c].ToString().Trim().Replace(".jpg", "").Replace(".jpeg", "");
                                for (int j = 1; j < 6; j++)
                                {
                                    if (System.IO.File.Exists(Server.MapPath("/resources/halfpricedraps/product/large/" + flname + "_" + j.ToString().Trim() + ".jpg")))
                                    {
                                        args[c + j] = "https://www.halfpricedrapes.com/resources/halfpricedraps/product/large/" + flname + "_" + j.ToString().Trim() + ".jpg";

                                    }
                                    else
                                    {
                                        args[c + j] = "";
                                    }

                                }
                                c = c + 5;
                            }
                            else
                            {
                                args[c] = "";
                            }
                        }
                        else
                        {
                            args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                        }


                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
            }
            String FileName = "AllProduct_" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".csv";
            if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));


            String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
            WriteFile(sb.ToString(), FilePath);


            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                Response.Clear();
                Response.ClearContent();
                WriteFile(sb.ToString(), FilePath);
                Response.ContentType = "text/csv";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(FilePath);
                Response.End();
            }
        }


        private void GetFeeddataAll()
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();
             SqlDataAdapter Adpt = new SqlDataAdapter();
             SqlCommand cmd = new SqlCommand();
             SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString());
            try
            {
               
                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "EXEC [GetproductExport_backup_av_all]";
                cmd.CommandTimeout = 10000;
                Adpt.SelectCommand = cmd;
                Adpt.Fill(dsorder);
            }
            catch (Exception ex)
            {
              
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
          //  dsorder = CommonComponent.GetCommonDataSet("EXEC [GetproductExport_backup_av_all]");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {

                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }
            }

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "features0")
                        {
                            args[c] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i][c].ToString().Trim(), @"<[^>]*>", String.Empty));

                            if (!string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString().Trim()))
                            {
                                string[] stralali = System.Text.RegularExpressions.Regex.Split(dsorder.Tables[0].Rows[i][c].ToString().Trim(), "<li>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                Int32 iTotal = 0;
                                if (stralali.Length > 0)
                                {

                                    for (int k = 0; k < stralali.Length; k++)
                                    {
                                        if (!string.IsNullOrEmpty(System.Text.RegularExpressions.Regex.Replace(stralali[k].ToString().Trim(), @"<[^>]*>", String.Empty)))
                                        {
                                            iTotal = iTotal + 1;
                                            if (iTotal > 14)
                                            {

                                                break;
                                            }
                                            else
                                            {
                                                args[c + iTotal] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(stralali[k].ToString().Trim(), @"<[^>]*>", String.Empty));
                                            }

                                        }

                                    }
                                }
                                if (iTotal > 14)
                                {
                                    c = c + 14;
                                }
                                else
                                {
                                    c = c + Convert.ToInt32(iTotal);
                                }

                            }
                        }
                        else if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "description")
                        {
                            args[c] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i][c].ToString().Trim(), @"<[^>]*>", String.Empty));
                        }
                        else if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "image main")
                        {
                            if (!string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString().Trim()))
                            {
                                if (System.IO.File.Exists(Server.MapPath("/resources/halfpricedraps/product/large/" + dsorder.Tables[0].Rows[i][c].ToString().Trim())))
                                {
                                    args[c] = "https://www.halfpricedrapes.com/resources/halfpricedraps/product/large/" + dsorder.Tables[0].Rows[i][c].ToString().Trim();
                                }
                                else
                                {
                                    args[c] = "";
                                }
                                string flname = dsorder.Tables[0].Rows[i][c].ToString().Trim().Replace(".jpg", "").Replace(".jpeg", "");
                                for (int j = 1; j < 6; j++)
                                {
                                    if (System.IO.File.Exists(Server.MapPath("/resources/halfpricedraps/product/large/" + flname + "_" + j.ToString().Trim() + ".jpg")))
                                    {
                                        args[c + j] = "https://www.halfpricedrapes.com/resources/halfpricedraps/product/large/" + flname + "_" + j.ToString().Trim() + ".jpg";

                                    }
                                    else
                                    {
                                        args[c + j] = "";
                                    }

                                }
                                c = c + 5;
                            }
                            else
                            {
                                args[c] = "";
                            }
                        }
                        else
                        {
                            args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                        }


                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
            }
            String FileName = "AllProduct_" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".csv";
            if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));


            String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
          //  WriteFile(sb.ToString(), FilePath);


            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                Response.Clear();
                Response.ClearContent();
                WriteFile(sb.ToString(), FilePath);
                Response.ContentType = "text/csv";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(FilePath);
                Response.End();
            }
        }
        private void GetSwatchproductAll()
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();
            SqlDataAdapter Adpt = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString());
            try
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "EXEC [GetproductExportSwatch]";
                cmd.CommandTimeout = 10000;
                Adpt.SelectCommand = cmd;
                Adpt.Fill(dsorder);
            }
            catch (Exception ex)
            {

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
            //  dsorder = CommonComponent.GetCommonDataSet("EXEC [GetproductExport_backup_av_all]");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {

                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }
            }

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                         args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());

                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
            }
            String FileName = "AllSwatchProduct_" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".csv";
            if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));


            String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
            //  WriteFile(sb.ToString(), FilePath);


            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                Response.Clear();
                Response.ClearContent();
                WriteFile(sb.ToString(), FilePath);
                Response.ContentType = "text/csv";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(FilePath);
                Response.End();
            }
        }
         protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if(txtpassword.Text.ToString().Trim().ToLower() == "avyas#klm123")
            {
                GetFeeddata();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgpasword", "alert('Please enter valid password.');", true);
            }
            
        }


     
        protected void Button1_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();

            dsorder = CommonComponent.GetCommonDataSet("EXEC [GetproductExport]");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {

                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }
            }

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "features0")
                        {
                            args[c] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i][c].ToString().Trim(), @"<[^>]*>", String.Empty));

                            if (!string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString().Trim()))
                            {
                                string[] stralali = System.Text.RegularExpressions.Regex.Split(dsorder.Tables[0].Rows[i][c].ToString().Trim(), "<li>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                                Int32 iTotal = 0;
                                if (stralali.Length > 0)
                                {

                                    for (int k = 0; k < stralali.Length; k++)
                                    {
                                        if (!string.IsNullOrEmpty(System.Text.RegularExpressions.Regex.Replace(stralali[k].ToString().Trim(), @"<[^>]*>", String.Empty)))
                                        {
                                            iTotal = iTotal + 1;
                                            if (iTotal > 14)
                                            {
                                                
                                                break;
                                            }
                                            else
                                            {
                                                args[c + iTotal] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(stralali[k].ToString().Trim(), @"<[^>]*>", String.Empty));
                                            }
                                            
                                        }
                                       
                                    }
                                }
                                if (iTotal > 14)
                                {
                                    c = c + 14;
                                }
                                else
                                {
                                    c = c + Convert.ToInt32(iTotal);
                                }

                            }
                        }
                        else if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "description")
                        {
                            args[c] = _EscapeCsvField(System.Text.RegularExpressions.Regex.Replace(dsorder.Tables[0].Rows[i][c].ToString().Trim(), @"<[^>]*>", String.Empty));
                        }
                        else if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower() == "imagename")
                        {
                            if (!string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString().Trim()))
                            {
                                if (System.IO.File.Exists(Server.MapPath("/resources/halfpricedraps/product/medium/" + dsorder.Tables[0].Rows[i][c].ToString().Trim())))
                                {
                                    args[c] = "https://www.halfpricedrapes.com/resources/halfpricedraps/product/medium/" + dsorder.Tables[0].Rows[i][c].ToString().Trim();
                                }
                                else
                                {
                                    args[c] = "";
                                }
                                string flname = dsorder.Tables[0].Rows[i][c].ToString().Trim().Replace(".jpg", "").Replace(".jpeg", "");
                                for (int j = 1; j < 6; j++)
                                {
                                    if (System.IO.File.Exists(Server.MapPath("/resources/halfpricedraps/product/medium/" + flname + "_" + j.ToString().Trim() + ".jpg")))
                                    {
                                        args[c + j] = "https://www.halfpricedrapes.com/resources/halfpricedraps/product/medium/" + flname + "_" + j.ToString().Trim() + ".jpg";

                                    }
                                    else
                                    {
                                        args[c + j] = "";
                                    }

                                }
                                c = c + 5;
                            }
                            else
                            {
                                args[c] = "";
                            }
                        }
                        else
                        {
                            args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                        }


                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
            }
            String FileName = "AllProduct_" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".csv";
            if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));


            String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
            WriteFile(sb.ToString(), FilePath);


        }

        protected void btnexport_Click(object sender, EventArgs e)
        {
            if (txtpassword.Text.ToString().Trim().ToLower() == "avyas#klm123")
            {
                GetFeeddataAll();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgpasword", "alert('Please enter valid password.');", true);
            }
        }
        private void GetparentdataAll()
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();
            SqlDataAdapter Adpt = new SqlDataAdapter();
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString());
            try
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.Text;
                cmd.CommandText = "EXEC [GetproductExport_backup_av_all_parent]";
                cmd.CommandTimeout = 10000;
                Adpt.SelectCommand = cmd;
                Adpt.Fill(dsorder);
            }
            catch (Exception ex)
            {

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
            //  dsorder = CommonComponent.GetCommonDataSet("EXEC [GetproductExport_backup_av_all]");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {

                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }
            }

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {
                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {


                        args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());



                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
            }
            String FileName = "AllparentChildProduct_" + DateTime.Now.Month + "-" + DateTime.Now.Day + "-" + DateTime.Now.Year + "-" + DateTime.Now.Hour + "-" + DateTime.Now.Minute + "-" + DateTime.Now.Second + ".csv";
            if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));


            String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
            //  WriteFile(sb.ToString(), FilePath);


            if (!String.IsNullOrEmpty(sb.ToString()))
            {
                Response.Clear();
                Response.ClearContent();
                WriteFile(sb.ToString(), FilePath);
                Response.ContentType = "text/csv";
                Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                Response.TransmitFile(FilePath);
                Response.End();
            }
        }

        protected void btnparentsku_Click(object sender, EventArgs e)
        {
            if (txtpassword.Text.ToString().Trim().ToLower() == "avyas#klm123")
            {
                GetparentdataAll();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgpasword", "alert('Please enter valid password.');", true);
            }
        }

        protected void btnswatchexport_Click(object sender, EventArgs e)
        {
            if (txtpassword.Text.ToString().Trim().ToLower() == "avyas#klm123")
            {
                GetSwatchproductAll();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgpasword", "alert('Please enter valid password.');", true);
            }
        }

    }
}