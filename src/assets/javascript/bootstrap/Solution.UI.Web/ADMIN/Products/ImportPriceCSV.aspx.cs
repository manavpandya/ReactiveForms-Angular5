using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.Odbc;
using LumenWorks.Framework.IO.Csv;
using Solution.Data;
using Solution.Bussines.Components;
using System.Text;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ImportPriceCSV : BasePage
    {
        #region Declaration
        private string StrFileName
        {
            get
            {
                if (ViewState["FileName"] == null)
                {
                    return "";
                }
                else
                {
                    return (ViewState["FileName"].ToString());
                }
            }
            set
            {
                ViewState["FileName"] = value;
            }
        }
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            btnImport.ImageUrl = "/App_Themes/" + Page.Theme + "/images/Import.gif";
        }

        /// <summary>
        /// Upload Button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            if (uploadCSV.HasFile && Path.GetExtension(uploadCSV.FileName).ToLower() == ".csv")
            {
                StrFileName = uploadCSV.FileName;
                uploadCSV.SaveAs(Server.MapPath("/Resources/halfpricedraps/ProductCSV/ImportCSV/") + StrFileName);
                FillMapping(uploadCSV.FileName);
            }
            else
            {
                lblMsg.Text = "Please upload appropriate file.";
                lblMsg.Style.Add("color", "#FF0000");
                lblMsg.Style.Add("font-weight", "normal");

            }
        }

        /// <summary>
        /// Display CSV File data in Grid
        /// </summary>
        /// <param name="FileName">string FileName</param>
        /// <returns>Returns the DataTable for Display</returns>
        private DataTable LoadCSV(string FileName)
        {
            FileInfo info = new FileInfo(Server.MapPath("/Resources/halfpricedraps/ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string[] FieldNames = csv.GetFieldHeaders();
                DataTable dtCSV = new DataTable();
                DataColumn columnID = new DataColumn();
                columnID.Caption = "Number";
                columnID.ColumnName = "Number";
                columnID.AllowDBNull = false;
                columnID.AutoIncrement = true;
                columnID.AutoIncrementSeed = 1;
                columnID.AutoIncrementStep = 1;
                dtCSV.Columns.Add(columnID);
                foreach (string FieldName in FieldNames)
                    dtCSV.Columns.Add(FieldName);
                while (csv.ReadNextRecord())
                {
                    DataRow dr = dtCSV.NewRow();
                    for (int i = 0; i < FieldCount; i++)
                    {
                        string FieldName = FieldNames[i];
                        if (!dr.Table.Columns.Contains(FieldName))
                        { continue; }

                        dr[FieldName] = csv[i];
                    }
                    dtCSV.Rows.Add(dr);
                }
                dtCSV.AcceptChanges();
                return dtCSV;
            }
        }

        /// <summary>
        /// Bind Check box Based on Columns Specified in CSV File
        /// </summary>
        /// <param name="FileName">string FileName</param>
        private void FillMapping(string FileName)
        {
            FileInfo info = new FileInfo(Server.MapPath("/Resources/halfpricedraps/ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string FieldStrike = "";
                chkFields.Items.Clear();
                if (FieldCount > 0)
                {
                    string[] FieldNames = csv.GetFieldHeaders();
                    foreach (string FieldName in FieldNames)
                    {
                        string tempFieldName = FieldName.ToLower();
                        if (tempFieldName == "sku" || tempFieldName == "price" || tempFieldName == "saleprice" || tempFieldName == "inventory" || tempFieldName == "weight")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {
                        if (FieldStrike.ToLower().Contains("price"))
                            chkFields.Items.Add("Price");
                        if (FieldStrike.ToLower().Contains("saleprice"))
                            chkFields.Items.Add("Sale Price");
                        if (FieldStrike.ToLower().Contains("inventory"))
                            chkFields.Items.Add("Inventory");
                        if (FieldStrike.ToLower().Contains("weight"))
                            chkFields.Items.Add("Weight");
                        BindData();
                        contVerify.Visible = true;
                    }
                    else
                    {
                        lblMsg.Text = "Please specify SKU, Price and SalePrice, Inventory and Weight in file.";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                    }
                    for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                        chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    lblMsg.Text = "Please specify SKU, Price and SalePrice, Inventory and Weight in file.";
                    lblMsg.Style.Add("color", "#FF0000");
                    lblMsg.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
        }

        /// <summary>
        /// Bind Data into Gridview
        /// </summary>
        private void BindData()
        {
            DataTable dtCSV = LoadCSV(StrFileName);
            if (dtCSV.Rows.Count > 0)
            {
                grdCSV.DataSource = dtCSV;
                grdCSV.DataBind();
            }
            else
                lblMsg.Text = "No data exists in file.";
            lblMsg.Style.Add("color", "#FF0000");
            lblMsg.Style.Add("font-weight", "normal");
        }
        
        /// <summary>
        /// Insert Grid Data into database
        /// </summary>
        /// <param name="dtCSV">DataTable dtCSV</param>
        /// <returns>true if Inserted, false otherwise</returns>
        private bool InsertDataInDataBase(DataTable dtCSV)
        {
            if (dtCSV != null && dtCSV.Rows.Count > 0)
            {
                SQLAccess oSQLAccess = new SQLAccess();
                int count = 0, Inventory = 0;
                string Error = string.Empty;
                lblMsg.Text = "";

                decimal Price = 0;
                decimal SalePrice = 0;
                decimal Weight = 0;
                for (int iLoopRows = 0; iLoopRows < dtCSV.Rows.Count; iLoopRows++)
                {
                    string SKU = (!dtCSV.Rows[iLoopRows].Table.Columns.Contains("sku")) ? string.Empty : dtCSV.Rows[iLoopRows]["sku"].ToString() + "";
                    Price = (!dtCSV.Rows[iLoopRows].Table.Columns.Contains("Price")) ? decimal.Zero : dtCSV.Rows[iLoopRows]["Price"] != "" ? Convert.ToDecimal(dtCSV.Rows[iLoopRows]["Price"]) : decimal.Zero;
                    SalePrice = (!dtCSV.Rows[iLoopRows].Table.Columns.Contains("SalePrice")) ? decimal.Zero : dtCSV.Rows[iLoopRows]["SalePrice"] != "" ? Convert.ToDecimal(dtCSV.Rows[iLoopRows]["SalePrice"]) : decimal.Zero;
                    Inventory = (!dtCSV.Rows[iLoopRows].Table.Columns.Contains("Inventory")) ? 0 : dtCSV.Rows[iLoopRows]["Inventory"] != "" ? Convert.ToInt32(dtCSV.Rows[iLoopRows]["Inventory"]) : 0;
                    Weight = (!dtCSV.Rows[iLoopRows].Table.Columns.Contains("Weight")) ? decimal.Zero : dtCSV.Rows[iLoopRows]["Weight"] != "" ? Convert.ToDecimal(dtCSV.Rows[iLoopRows]["Weight"]) : decimal.Zero;

                    if (Price > SalePrice && Inventory > 0 && Price > 0 && Weight > 0 && SalePrice > 0 && !string.IsNullOrEmpty(SKU))
                    {
                        string sql = "Update tb_Product set sku=sku";
                        sql += (dtCSV.Rows[iLoopRows].Table.Columns.Contains("price") && !string.IsNullOrEmpty(dtCSV.Rows[iLoopRows]["price"].ToString()) && chkFields.Items.FindByValue("price") != null && chkFields.Items.FindByValue("price").Selected) ? ",  price=" + dtCSV.Rows[iLoopRows]["price"].ToString() : string.Empty;
                        sql += (dtCSV.Rows[iLoopRows].Table.Columns.Contains("saleprice") && !string.IsNullOrEmpty(dtCSV.Rows[iLoopRows]["saleprice"].ToString()) && chkFields.Items.FindByValue("saleprice") != null && chkFields.Items.FindByValue("saleprice").Selected) ? ",  saleprice=" + dtCSV.Rows[iLoopRows]["saleprice"].ToString() : string.Empty;
                        sql += (dtCSV.Rows[iLoopRows].Table.Columns.Contains("inventory") && !string.IsNullOrEmpty(dtCSV.Rows[iLoopRows]["inventory"].ToString()) && chkFields.Items.FindByValue("inventory") != null && chkFields.Items.FindByValue("inventory").Selected) ? ",  inventory=" + dtCSV.Rows[iLoopRows]["inventory"].ToString() : string.Empty;
                        sql += (dtCSV.Rows[iLoopRows].Table.Columns.Contains("weight") && !string.IsNullOrEmpty(dtCSV.Rows[iLoopRows]["weight"].ToString()) && chkFields.Items.FindByValue("weight") != null && chkFields.Items.FindByValue("weight").Selected) ? ",  weight=" + dtCSV.Rows[iLoopRows]["weight"].ToString() : string.Empty;
                        sql += " where sku='" + SKU.Replace("'", "''") + "'";
                        try
                        {
                            if (oSQLAccess.ExecuteNonQuery(sql.ToString().ToLower().Replace("null", "0")))
                            {
                                count++;
                            }
                            else
                            {
                                Error += SKU + ",";
                            }
                        }
                        catch (Exception ex)
                        {
                            Error += SKU + ", ";
                            lblMsg.Text = "Dose not update these SKU : " + Error;
                        }
                    }
                    else
                    {
                        Error += SKU + ", ";
                    }
                }

                if (string.IsNullOrEmpty(Error))
                {
                    if (count > 0)
                    {
                        lblMsg.Text = "Database Updated Successfully..." + "<br/>" +
                            "Total updated SKU :" + count.ToString();
                    }
                }
                else
                {
                    lblMsg.Text = "Dose not update these SKU : " + Error.Substring(0, Error.Length - 1) +
                        "<br/>" +
                        "Total No of updated SKU :" + count.ToString();
                }

                lblMsg.Style.Add("color", "#FF0000");
                lblMsg.Style.Add("font-weight", "normal");
                return true;
            }
            return false;

        }

        /// <summary>
        /// Import button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnImport_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(StrFileName))
            {
                DataTable dtCSV = LoadCSV(StrFileName);
                if (InsertDataInDataBase(dtCSV) && lblMsg.Text == "")
                {
                    contVerify.Visible = false;
                    return;
                }
            }
            else
                lblMsg.Text += "Sorry file not found. Please retry uploading.";
        }

        /// <summary>
        ///  CSV Gridview Page Index Changing Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewPageEventArgs e</param>
        protected void grdCSV_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            grdCSV.PageIndex = e.NewPageIndex;
            BindData();
        }
    }
}