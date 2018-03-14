using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using System.IO;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Promotions
{
    public partial class ExportProductFB : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindCategory();
                BinData();
                btnSave.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/save.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnCancel.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            }
        }

        /// <summary>
        /// Bind Selected Data 
        /// </summary>
        private void BinData()
        {
            DataSet dsData = CommonComponent.GetCommonDataSet("select * from TempQuery where Name='FacebookFeed'");
            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                ddlCategory.SelectedValue = dsData.Tables[0].Rows[0]["CategoryID"].ToString();
                if (dsData.Tables[0].Rows[0]["SubCategoryID"].ToString() != "0" && dsData.Tables[0].Rows[0]["CategoryID"].ToString() != "0")
                {
                    ddlSubCategory.Items.Clear();
                    DataSet dsDataS = CommonComponent.GetCommonDataSet("select * from tb_Category where StoreID = 1 and isnull(Active,0) = 1 and isnull(deleted,0) = 0 and CategoryID in (SELECT items FROM dbo.GetParentcatgory(" + ddlCategory.SelectedValue.ToString() + "))");
                    if (dsDataS != null && dsDataS.Tables.Count > 0 && dsDataS.Tables[0].Rows.Count > 0)
                    {
                        ddlSubCategory.DataSource = dsDataS.Tables[0];
                        ddlSubCategory.DataTextField = "Name";
                        ddlSubCategory.DataValueField = "CategoryID";
                        ddlSubCategory.DataBind();

                        ddlSubCategory.SelectedValue = dsData.Tables[0].Rows[0]["SubCategoryID"].ToString();
                    }
                    ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
                }

                if (dsData.Tables[0].Rows[0]["IsReadyMade"].ToString() == "1")
                {
                    chkReadyMade.Checked = true;
                }
                if (dsData.Tables[0].Rows[0]["IsCustom"].ToString() == "1")
                {
                    chkCustom.Checked = true;
                }
                if (dsData.Tables[0].Rows[0]["IsHw"].ToString() == "1")
                {
                    chkHardware.Checked = true;
                }
                if (dsData.Tables[0].Rows[0]["IsAll"].ToString() == "1")
                {
                    chkSelectAll.Checked = true;
                    chkReadyMade.Checked = true;
                    chkHardware.Checked = true;
                    chkCustom.Checked = true;
                }

                ddlStock.SelectedValue = dsData.Tables[0].Rows[0]["StockStatus"].ToString();

                lblLastUpdated.Text = Convert.ToDateTime(dsData.Tables[0].Rows[0]["UpdatedDate"].ToString()).ToString();
            }

        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void BindCategory()
        {
            DataSet dsData = CommonComponent.GetCommonDataSet("select * from tb_Category where StoreID = 1 and isnull(Active,0) = 1 and isnull(deleted,0) = 0 and CategoryID in (select CategoryID from tb_CategoryMapping where ParentCategoryID = 0)");
            if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
            {
                ddlCategory.DataSource = dsData.Tables[0];
                ddlCategory.DataTextField = "Name";
                ddlCategory.DataValueField = "CategoryID";
                ddlCategory.DataBind();
            }
            ddlCategory.Items.Insert(0, new ListItem("All", "0"));

            ddlSubCategory.Items.Clear();
            ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
        }

        /// <summary>
        /// Function for Remove comma From String
        /// </summary>
        /// <param name="sFieldValueToEscape">String sFieldValueToEscape</param>
        /// <returns>return String</returns>
        private string _EscapeCsvField(string sFieldValueToEscape)
        {
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
                return sFieldValueToEscape.Replace("\"", "\"\"");//sFieldValueToEscape;
            }
        }

        /// <summary>
        /// Writes the File
        /// </summary>
        /// <param name="Text">string Text</param>
        /// <param name="FileName">string FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);
            if (writer != null)
                writer.Close();
        }


        /// <summary>
        ///  Export Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnExport_Click(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedValue == "")
                return;

            //string StrExcel = "sku,title,description,sale_price,Inventory,availability,link,image_link,condition,gtin,product_type,color,pattern,material";
            string StrExcel = "";// "sku,title,description,sale_price,Inventory,availability,link,image_link,condition,gtin,product_type,color,pattern,material";
            DataSet Ds = new DataSet();

            string Query = string.Empty;

            if (!chkReadyMade.Checked && !chkMadetoOrder.Checked && !chkCustom.Checked && !chkHardware.Checked)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('select atleast one product type.', 'Message');});", true);
                return;
            }

            string strWhere = string.Empty;
            int isReadyMade = 0;
            int isCustom = 0;
            int isHW = 0;
            strWhere = " 1=1 ";
            //strWhere = " and 1=1 and(";

            if (chkReadyMade.Checked)
            {
                isReadyMade = 1;
                //strWhere += " isnull(Ismadetoready,0) = 1 OR ";
            }
            //else if (chkMadetoOrder.Checked)
            //{
            //    strWhere += " isnull(Ismadetoorder,0) = 1 OR ";
            //}
            if (chkCustom.Checked)
            {
                isCustom = 1;
                //strWhere += " isnull(Ismadetomeasure,0) = 1 OR ";
            }
            else if (chkHardware.Checked)
            {
                isHW = 1;
                //strWhere += " lower(isnull(SKU,'''')) like ''hdw-%'' OR ";
            }

            //strWhere += ")";

            //if (!chkHardware.Checked)
            //{
            //    strWhere += " and lower(isnull(SKU,'''')) not like ''hdw-%'' ";
            //}


            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = strWhere.Replace("OR )", ")");
            }

            if (ddlSubCategory.SelectedValue.ToString() != "0")
                Query = "exec GetProductExportFB @CategoryID=" + ddlSubCategory.SelectedValue.ToString() + ",@Where='" + strWhere + "',@IsReadyMade=" + isReadyMade + ",@IsCustom=" + isCustom + ",@IsHW=" + isHW + ",@Stock=" + ddlStock.SelectedValue.ToString();
            else
                Query = "exec GetProductExportFB " + ddlCategory.SelectedValue.ToString() + ",@Where='" + strWhere + "',@IsReadyMade=" + isReadyMade + ",@IsCustom=" + isCustom + ",@IsHW=" + isHW + ",@Stock=" + ddlStock.SelectedValue.ToString();

            Ds = CommonComponent.GetCommonDataSet(Query);

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Ds != null)
            {
                //Decimal Price = Decimal.Zero;
                for (int c = 0; c < Ds.Tables[0].Columns.Count; c++)
                {
                    if (Ds.Tables[0].Columns.Count - 1 == c)
                    {
                        StrExcel = StrExcel + Ds.Tables[0].Columns[c].ColumnName.ToString();
                    }
                    else
                    {
                        StrExcel = StrExcel + Ds.Tables[0].Columns[c].ColumnName.ToString() + ",";
                    }

                }
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    string strValue = "";
                    for (int iTotal = 0; iTotal < Ds.Tables[0].Columns.Count; iTotal++)
                    {
                        string strResponse = Ds.Tables[0].Rows[i][iTotal].ToString();
                        strResponse = strResponse.Replace("\t", "");
                        strResponse = strResponse.Replace("\" />", "\"/>");
                        strResponse = strResponse.Replace("\" >", "\">");
                        strValue += _EscapeCsvField(strResponse.ToString().Replace("\r\n", "")) + ",";
                    }
                    if (strValue.ToString().IndexOf(",") > -1)
                    {
                        strValue = strValue.Substring(0, strValue.Length - 1);
                    }
                    sb.AppendLine(strValue);
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(StrExcel.ToString());

                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    String FileName = string.Empty;

                    //String FileName = "Products_FB_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    FileName = "Half-Price-Drapes-Product-Feed.csv";

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));
                    }
                    if (File.Exists(Server.MapPath("~/Admin/Files/" + FileName)))
                    {
                        File.Delete(Server.MapPath("~/Admin/Files/" + FileName));
                    }

                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);

                    SendMail();

                    Response.Clear();
                    Response.ClearContent();
                    //Page.RegisterStartupScript("Success", "<script type='text/javascript'>alert('CSV file generated successfully..');</script>");
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Record(s) Found.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Record(s) Found.', 'Message');});", true);
                return;
            }
        }

        private void SendMail()
        {
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("/Admin/Dashboard.aspx");
        }

        protected void ddlCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedValue.ToString() != "0")
            {
                ddlSubCategory.Items.Clear();
                DataSet dsData = CommonComponent.GetCommonDataSet("select * from tb_Category where StoreID = 1 and isnull(Active,0) = 1 and isnull(deleted,0) = 0 and CategoryID in (SELECT items FROM dbo.GetParentcatgory(" + ddlCategory.SelectedValue.ToString() + "))");
                if (dsData != null && dsData.Tables.Count > 0 && dsData.Tables[0].Rows.Count > 0)
                {
                    ddlSubCategory.DataSource = dsData.Tables[0];
                    ddlSubCategory.DataTextField = "Name";
                    ddlSubCategory.DataValueField = "CategoryID";
                    ddlSubCategory.DataBind();
                }
                ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
            }
            else
            {
                ddlSubCategory.Items.Clear();
                ddlSubCategory.Items.Insert(0, new ListItem("Select", "0"));
            }
        }

        /// <summary>
        ///  Export Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (ddlCategory.SelectedValue == "")
                return;

            //string StrExcel = "sku,title,description,sale_price,Inventory,availability,link,image_link,condition,gtin,product_type,color,pattern,material";
            string StrExcel = "";// "sku,title,description,sale_price,Inventory,availability,link,image_link,condition,gtin,product_type,color,pattern,material";
            DataSet Ds = new DataSet();

            string Query = string.Empty;

            if (!chkReadyMade.Checked && !chkMadetoOrder.Checked && !chkCustom.Checked && !chkHardware.Checked)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('select atleast one product type.', 'Message');});", true);
                return;
            }

            string strWhere = string.Empty;
            int isReadyMade = 0;
            int isCustom = 0;
            int isHW = 0;
            strWhere = " 1=1 ";
            //strWhere = " and 1=1 and(";

            if (chkReadyMade.Checked)
            {
                isReadyMade = 1;
                //strWhere += " isnull(Ismadetoready,0) = 1 OR ";
            }
            //else if (chkMadetoOrder.Checked)
            //{
            //    strWhere += " isnull(Ismadetoorder,0) = 1 OR ";
            //}
            if (chkCustom.Checked)
            {
                isCustom = 1;
                //strWhere += " isnull(Ismadetomeasure,0) = 1 OR ";
            }

            if (chkHardware.Checked)
            {
                isHW = 1;
                //strWhere += " lower(isnull(SKU,'''')) like ''hdw-%'' OR ";
            }

            //strWhere += ")";

            //if (!chkHardware.Checked)
            //{
            //    strWhere += " and lower(isnull(SKU,'''')) not like ''hdw-%'' ";
            //}


            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = strWhere.Replace("OR )", ")");
            }

            if (ddlSubCategory.SelectedValue.ToString() != "0")
                Query = "exec GetProductExportFB @CategoryID=" + ddlSubCategory.SelectedValue.ToString() + ",@Where='" + strWhere + "',@IsReadyMade=" + isReadyMade + ",@IsCustom=" + isCustom + ",@IsHW=" + isHW + ",@Stock=" + ddlStock.SelectedValue.ToString();
            else
                Query = "exec GetProductExportFB " + ddlCategory.SelectedValue.ToString() + ",@Where='" + strWhere + "',@IsReadyMade=" + isReadyMade + ",@IsCustom=" + isCustom + ",@IsHW=" + isHW + ",@Stock=" + ddlStock.SelectedValue.ToString();

            Ds = CommonComponent.GetCommonDataSet(Query);

            if (!string.IsNullOrEmpty(Query))
            {
                string strQuery = string.Empty;
                strQuery = "update TempQuery set Query='" + Query.Replace("'", "''") + "'";
                if (ddlCategory.SelectedValue != "0")
                    strQuery += ",CategoryID=" + Convert.ToInt32(ddlCategory.SelectedValue.ToString());
                else
                    strQuery += ",CategoryID=0";

                if (ddlSubCategory.SelectedValue != "0")
                    strQuery += ",SubCategoryID=" + Convert.ToInt32(ddlSubCategory.SelectedValue.ToString());
                else
                    strQuery += ",SubCategoryID=0";

                if (chkReadyMade.Checked)
                    strQuery += ",IsReadyMade=1 ";
                else
                    strQuery += ",IsReadyMade=0 ";

                if (chkSelectAll.Checked)
                    strQuery += ",IsAll=1 ";
                else
                    strQuery += ",IsAll=0 ";

                if (chkCustom.Checked)
                    strQuery += ",IsCustom=1 ";
                else
                    strQuery += ",IsCustom=0 ";

                if (chkHardware.Checked)
                    strQuery += ",IsHw=1 ";
                else
                    strQuery += ",IsHw=0 ";

                strQuery += ",StockStatus=" + ddlStock.SelectedValue.ToString() + " ";

                strQuery += " where Name='FacebookFeed'";
                CommonComponent.GetScalarCommonData(strQuery);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Ds != null)
            {
                //Decimal Price = Decimal.Zero;
                for (int c = 0; c < Ds.Tables[0].Columns.Count; c++)
                {
                    if (Ds.Tables[0].Columns.Count - 1 == c)
                    {
                        StrExcel = StrExcel + Ds.Tables[0].Columns[c].ColumnName.ToString();
                    }
                    else
                    {
                        StrExcel = StrExcel + Ds.Tables[0].Columns[c].ColumnName.ToString() + ",";
                    }

                }
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    string strValue = "";
                    for (int iTotal = 0; iTotal < Ds.Tables[0].Columns.Count; iTotal++)
                    {
                        string strResponse = Ds.Tables[0].Rows[i][iTotal].ToString();
                        strResponse = strResponse.Replace("\t", "");
                        strResponse = strResponse.Replace("\" />", "\"/>");
                        strResponse = strResponse.Replace("\" >", "\">");
                        strValue += _EscapeCsvField(strResponse.ToString().Replace("\r\n", "")) + ",";
                    }
                    if (strValue.ToString().IndexOf(",") > -1)
                    {
                        strValue = strValue.Substring(0, strValue.Length - 1);
                    }
                    sb.AppendLine(strValue);
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(StrExcel.ToString());

                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    String FileName = string.Empty;

                    //String FileName = "Products_FB_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    FileName = "Half-Price-Drapes-Product-Feed.csv";

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));
                    }
                    if (File.Exists(Server.MapPath("~/Admin/Files/" + FileName)))
                    {
                        File.Delete(Server.MapPath("~/Admin/Files/" + FileName));
                    }

                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);

                    CommonComponent.ExecuteCommonData("update TempQuery set UpdatedDate  = getdate() where Name='FacebookFeed'");

                    Response.Clear();
                    Response.ClearContent();
                    //Page.RegisterStartupScript("Success", "<script type='text/javascript'>alert('CSV file generated successfully..');</script>");
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Record(s) Found.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Record(s) Found.', 'Message');});", true);
                return;
            }
        }
        protected void btnSaveNew_Click(object sender, EventArgs e)
        {

            if (ddlCategory.SelectedValue == "")
                return;

            //string StrExcel = "sku,title,description,sale_price,Inventory,availability,link,image_link,condition,gtin,product_type,color,pattern,material";
            string StrExcel = "";// "sku,title,description,sale_price,Inventory,availability,link,image_link,condition,gtin,product_type,color,pattern,material";
            DataSet Ds = new DataSet();

            string Query = string.Empty;

            if (!chkReadyMade.Checked && !chkMadetoOrder.Checked && !chkCustom.Checked && !chkHardware.Checked)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('select atleast one product type.', 'Message');});", true);
                return;
            }

            string strWhere = string.Empty;
            int isReadyMade = 0;
            int isCustom = 0;
            int isHW = 0;
            strWhere = " IsNewArrival=1 AND CAST(IsNewArrivalToDate AS DATE)>= CAST(GETDATE() AS DATE) AND CAST(IsNewArrivalFromDate AS DATE) <=CAST(GETDATE() AS DATE) and 1=1 ";
            //strWhere = " and 1=1 and(";

            if (chkReadyMade.Checked)
            {
                isReadyMade = 1;
                //strWhere += " isnull(Ismadetoready,0) = 1 OR ";
            }
            //else if (chkMadetoOrder.Checked)
            //{
            //    strWhere += " isnull(Ismadetoorder,0) = 1 OR ";
            //}
            if (chkCustom.Checked)
            {
                isCustom = 1;
                //strWhere += " isnull(Ismadetomeasure,0) = 1 OR ";
            }

            if (chkHardware.Checked)
            {
                isHW = 1;
                //strWhere += " lower(isnull(SKU,'''')) like ''hdw-%'' OR ";
            }

            //strWhere += ")";

            //if (!chkHardware.Checked)
            //{
            //    strWhere += " and lower(isnull(SKU,'''')) not like ''hdw-%'' ";
            //}


            if (!string.IsNullOrEmpty(strWhere))
            {
                strWhere = strWhere.Replace("OR )", ")");
            }

            if (ddlSubCategory.SelectedValue.ToString() != "0")
                Query = "exec GetProductExportArrivalFB @CategoryID=" + ddlSubCategory.SelectedValue.ToString() + ",@Where='" + strWhere + "',@IsReadyMade=" + isReadyMade + ",@IsCustom=" + isCustom + ",@IsHW=" + isHW + ",@Stock=" + ddlStock.SelectedValue.ToString();
            else
                Query = "exec GetProductExportArrivalFB " + ddlCategory.SelectedValue.ToString() + ",@Where='" + strWhere + "',@IsReadyMade=" + isReadyMade + ",@IsCustom=" + isCustom + ",@IsHW=" + isHW + ",@Stock=" + ddlStock.SelectedValue.ToString();

            Ds = CommonComponent.GetCommonDataSet(Query);

            if (!string.IsNullOrEmpty(Query))
            {
                string strQuery = string.Empty;
                strQuery = "update TempQuery set Query='" + Query.Replace("'", "''") + "'";
                if (ddlCategory.SelectedValue != "0")
                    strQuery += ",CategoryID=" + Convert.ToInt32(ddlCategory.SelectedValue.ToString());
                else
                    strQuery += ",CategoryID=0";

                if (ddlSubCategory.SelectedValue != "0")
                    strQuery += ",SubCategoryID=" + Convert.ToInt32(ddlSubCategory.SelectedValue.ToString());
                else
                    strQuery += ",SubCategoryID=0";

                if (chkReadyMade.Checked)
                    strQuery += ",IsReadyMade=1 ";
                else
                    strQuery += ",IsReadyMade=0 ";

                if (chkSelectAll.Checked)
                    strQuery += ",IsAll=1 ";
                else
                    strQuery += ",IsAll=0 ";

                if (chkCustom.Checked)
                    strQuery += ",IsCustom=1 ";
                else
                    strQuery += ",IsCustom=0 ";

                if (chkHardware.Checked)
                    strQuery += ",IsHw=1 ";
                else
                    strQuery += ",IsHw=0 ";

                strQuery += ",StockStatus=" + ddlStock.SelectedValue.ToString() + " ";

                strQuery += " where Name='FacebookFeed'";
                CommonComponent.GetScalarCommonData(strQuery);
            }

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (Ds != null)
            {
                //Decimal Price = Decimal.Zero;
                for (int c = 0; c < Ds.Tables[0].Columns.Count; c++)
                {
                    if (Ds.Tables[0].Columns.Count - 1 == c)
                    {
                        StrExcel = StrExcel + Ds.Tables[0].Columns[c].ColumnName.ToString();
                    }
                    else
                    {
                        StrExcel = StrExcel + Ds.Tables[0].Columns[c].ColumnName.ToString() + ",";
                    }

                }
                for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                {
                    string strValue = "";
                    for (int iTotal = 0; iTotal < Ds.Tables[0].Columns.Count; iTotal++)
                    {
                        string strResponse = Ds.Tables[0].Rows[i][iTotal].ToString();
                        strResponse = strResponse.Replace("\t", "");
                        strResponse = strResponse.Replace("\" />", "\"/>");
                        strResponse = strResponse.Replace("\" >", "\">");
                        strValue += _EscapeCsvField(strResponse.ToString().Replace("\r\n", "")) + ",";
                    }
                    if (strValue.ToString().IndexOf(",") > -1)
                    {
                        strValue = strValue.Substring(0, strValue.Length - 1);
                    }
                    sb.AppendLine(strValue);
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(StrExcel.ToString());

                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    String FileName = string.Empty;

                    //String FileName = "Products_FB_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    FileName = "New-Arrival-Product-Feed.csv";

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    {
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));
                    }
                    if (File.Exists(Server.MapPath("~/Admin/Files/" + FileName)))
                    {
                        File.Delete(Server.MapPath("~/Admin/Files/" + FileName));
                    }

                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);
                    WriteFile(sb.ToString(), FilePath);

                    CommonComponent.ExecuteCommonData("update TempQuery set UpdatedDate  = getdate() where Name='FacebookFeed'");

                    Response.Clear();
                    Response.ClearContent();
                    //Page.RegisterStartupScript("Success", "<script type='text/javascript'>alert('CSV file generated successfully..');</script>");
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Record(s) Found.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('No Record(s) Found.', 'Message');});", true);
                return;
            }
        }
    }

}