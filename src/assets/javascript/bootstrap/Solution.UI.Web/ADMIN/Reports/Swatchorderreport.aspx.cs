using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Components.AdminCommon;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Net;
using System.IO;
using System.Data.SqlClient;

namespace Solution.UI.Web.ADMIN.Reports
{
    public partial class Swatchorderreport : BasePage
    {
        public bool isAscend = false;
        public int TotalQty = 0;
        public decimal Totalprice = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                txtFromdate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now.AddMonths(-3)));
                txtTodate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                //GetOrderListByStoreId(1, pageSize);
                
            }
            
            btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
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
        /// <summary>
        /// Export Button Click Event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            DataView dvCust = new DataView();
            DataSet ds = new DataSet();
           // ds = CommonComponent.GetCommonDataSet("EXEC GuiGetSwatchordernotordercome '" + txtFromdate.Text.ToString() + "','" + txtTodate.Text.ToString() + "'");
            SqlDataAdapter Adpt = new SqlDataAdapter();
            SqlConnection conn = new SqlConnection(System.Configuration.ConfigurationSettings.AppSettings["RedTag_CCTV_Ecomm_DBEntities"].ToString());
            SqlCommand cmd = new SqlCommand("GuiGetSwatchordernotordercome", conn);
            try
            {

                if (conn.State == ConnectionState.Closed)
                    conn.Open();

                cmd.Connection = conn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@datefrom", txtFromdate.Text.ToString().Trim());
                cmd.Parameters.AddWithValue("@dateto", txtTodate.Text.ToString().Trim());
                cmd.CommandTimeout = 10000;
                Adpt.SelectCommand = cmd;
                Adpt.Fill(ds);
               

                 
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

            string column = "";
            string columnnom = "";
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
               
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0  )
                {
                    for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                    {
                        if (ds.Tables[0].Columns.Count - 1 == i)
                        {
                            column += ds.Tables[0].Columns[i].ColumnName.ToString();
                            columnnom += "{" + i.ToString() + "}";
                        }
                        else
                        {
                            column += ds.Tables[0].Columns[i].ColumnName.ToString() + ",";
                            columnnom += "{" + i.ToString() + "},";
                        }
                    }
                    
                    sb.AppendLine(column);
                }

                if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {

                        object[] args = new object[ds.Tables[0].Columns.Count];
                        for (int c = 0; c < ds.Tables[0].Columns.Count; c++)
                        {
                             
                             
                                args[c] = _EscapeCsvField(ds.Tables[0].Rows[i][c].ToString().Trim());
                            
                        }
                        sb.AppendLine(string.Format(columnnom, args));
                    }
                    
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "SwatchOrderList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    //sb.Remove(0, sb.Length);
                    //sb.AppendLine(column);
                    //sb.AppendLine(FullString);

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                return;
            }

        }

        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text"></param>
        /// <param name="FileName"></param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }
    }
}