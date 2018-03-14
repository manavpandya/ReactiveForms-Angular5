using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using Solution.Data;
using LumenWorks.Framework.IO.Csv;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ExportProductDis : BasePage
    {
        string StrFileName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnexport1.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
        }
        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string StrFileName)
        {
            try
            {
                string docPath = Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName;
                if (File.Exists(Server.MapPath(docPath)))
                {
                    File.Delete(Server.MapPath(docPath));
                }

            }
            catch (Exception ex)
            {

            }

        }

        /// <summary>
        /// Bind Check box based on columns specified in CSV File
        /// </summary>
        /// <param name="FileName">FileName</param>
        private void FillMapping(string FileName)
        {

            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + FileName);
            StreamReader reader = info.OpenText();
            string File = reader.ReadToEnd();
            reader.Close();
            using (CsvReader csv = new CsvReader(new StringReader(File), true))
            {
                int FieldCount = csv.FieldCount;
                string FieldStrike = ",";

                if (FieldCount > 0)
                {
                    string[] FieldNames = csv.GetFieldHeaders();

                    foreach (string FieldName in FieldNames)
                    {
                        string tempFieldName = FieldName.ToLower();
                        if (tempFieldName == "sku" || tempFieldName == "upc" || tempFieldName == "discontinue")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",sku,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",upc,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",discontinue,") > -1)
                        {

                        }
                        else
                        {
                            lblMsg.Text = "Please Specify SKU,UPC,Discontinue in file.";
                            lblMsg.Style.Add("color", "#FF0000");
                            lblMsg.Style.Add("font-weight", "normal");
                        }
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1)
                    {
                        //if (ddlFabricType.SelectedIndex > 0)
                        //{
                        //    ddlFabricType_SelectedIndexChanged(null, null);
                        //}

                        //BindData();

                    }
                    else
                    {
                        lblMsg.Text = "Please Specify SKU,UPC,Discontinue in file.";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    lblMsg.Text = "Please Specify SKU,UPC,Discontinue in file.";
                    lblMsg.Style.Add("color", "#FF0000");
                    lblMsg.Style.Add("font-weight", "normal");
                }
                csv.Dispose();
            }
        }



        protected void btnImport_Click(object sender, EventArgs e)
        {
            try
            {
                if (uploadCSV.HasFile && Path.GetExtension(uploadCSV.FileName).ToLower() == ".csv")
                {
                    lblMsg.Text = "";
                    if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/")))
                        Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/"));
                    StrFileName = uploadCSV.FileName;
                    DeleteDocument(StrFileName);
                    uploadCSV.SaveAs(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + StrFileName);
                    //StrFileName = Server.MapPath(AppLogic.AppConfigs("CategoryImportPath") + "CategoryCSV/ImportCSV/") + StrFileName;
                    FillMapping(uploadCSV.FileName);
                }
                else
                {
                    lblMsg.Text = "Please upload .CSV File.";
                    lblMsg.Style.Add("color", "#FF0000");
                    lblMsg.Style.Add("font-weight", "normal");
                    return;

                }
                if (!string.IsNullOrEmpty(StrFileName))
                {

                    DataTable dtCSV = LoadCSV(StrFileName);
                    if (InsertDataInDataBase(dtCSV) && lblMsg.Text == "")
                    {
                        // contVerify.Visible = false;
                        lblMsg.Text = "File Imported Successfully";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                        lblMsg.Visible = true;
                        return;

                    }


                }
                else
                {
                    lblMsg.Text += "Sorry file not found. Please retry uploading.";
                    lblMsg.Style.Add("color", "#FF0000");
                    lblMsg.Style.Add("font-weight", "normal");
                    lblMsg.Visible = true;
                }
            }
            catch { }
        }

        /// <summary>
        /// Display CSV File data in Grid
        /// </summary>
        /// <param name="FileName">FileName</param>
        /// <returns>DataTable</returns>
        private DataTable LoadCSV(string FileName)
        {
            FileInfo info = new FileInfo(Server.MapPath(AppLogic.AppConfigs("ProductImportPath") + "ProductCSV/ImportCSV/") + FileName);
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

        private bool InsertDataInDataBase(DataTable dt)
        {
            ltrmsg.Text = "";
            int counter = 0;
            int errorcounter = 0;
            DataTable derror = new DataTable();
            derror.Columns.Add("SrNo", typeof(int));
            derror.Columns.Add("SKU", typeof(string));
            derror.Columns.Add("Error", typeof(string));
            derror.AcceptChanges();
            DataSet Dscompare = new DataSet();
            Dscompare = CommonComponent.GetCommonDataSet("Exec GuiGetExportProduct 1");
            if (Dscompare != null && Dscompare.Tables.Count > 0 && Dscompare.Tables[0].Rows.Count > 0)
            {


                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {


                        if (String.IsNullOrEmpty(dt.Rows[i]["upc"].ToString()))
                        {
                            dt.Rows[i]["upc"] = "";
                            dt.AcceptChanges();
                        }
                        if (String.IsNullOrEmpty(dt.Rows[i]["discontinue"].ToString()))
                        {
                            dt.Rows[i]["discontinue"] = 0;
                            dt.AcceptChanges();
                        }
                        bool Discontinue = false;
                        if (!String.IsNullOrEmpty(dt.Rows[i]["discontinue"].ToString()))
                        {

                            if (dt.Rows[i]["discontinue"].ToString().Trim().ToLower() == "yes" || dt.Rows[i]["discontinue"].ToString().Trim().ToLower() == "1")
                            {
                                Discontinue = true;
                            }
                        }
                        bool BeforeDiscontinue = false;
                        DataRow[] Dr = Dscompare.Tables[0].Select("SKU='" + dt.Rows[i]["SKU"].ToString().Replace("'", "''").Trim() + "' and UPC='" + dt.Rows[i]["UPC"].ToString().Replace("'", "''").Trim() + "'");
                        if (Dr.Length > 0)
                        {
                            if (Dr[0]["discontinue"].ToString().ToLower() == "yes" || Dr[0]["discontinue"].ToString().ToLower() == "1")
                            {
                                BeforeDiscontinue = true;
                            }
                        }
                        else
                        {
                            errorcounter++;
                            DataRow derr = derror.NewRow();
                            derr["SrNo"] = i + 2;
                            derr["SKU"] = dt.Rows[i]["SKU"].ToString().Trim();
                            derr["Error"] = "Mismatch";
                            derror.Rows.Add(derr);
                            derror.AcceptChanges();

                        }

                        try
                        {

                            if (Dr.Length > 0)
                            {
                                if (Discontinue != BeforeDiscontinue)
                                {
                                    counter++;
                                    if (Discontinue)
                                    {
                                        CommonComponent.ExecuteCommonData("update tb_product set Discontinue='" + Discontinue + "',DiscontinuedOn='" + DateTime.Now + "' where storeid=1  and isnull(Deleted,0)=0 and SKU = '" + dt.Rows[i]["SKU"].ToString().Trim() + "' and UPC='" + dt.Rows[i]["UPC"].ToString().Trim() + "'");

                                    }
                                    else
                                    {
                                        CommonComponent.ExecuteCommonData("update tb_product set Discontinue='" + Discontinue + "' where storeid=1 and isnull(Deleted,0)=0 and SKU = '" + dt.Rows[i]["SKU"].ToString().Trim() + "' and UPC='" + dt.Rows[i]["UPC"].ToString().Trim() + "'");
                                    }



                                    CommonComponent.ExecuteCommonData("Exec GuiInsertDiscontinueLog '" + dt.Rows[i]["SKU"].ToString() + "','Auto'," + Session["AdminID"].ToString() + "," + Discontinue + ",'" + dt.Rows[i]["UPC"].ToString() + "'," + BeforeDiscontinue + "");
                                }
                            }



                        }
                        catch { }


                    }

                    String strErrors = "<center>" + counter.ToString() + " Records Updated Successfully!</center>";
                    if (errorcounter > 0 && derror.Rows.Count > 0)
                    {
                       
                        if (derror.Rows.Count > 0)
                        {
                            strErrors += "<br/><table class=\"table table-bordered table-striped table-condensed cf\">";
                            strErrors += "<thead><tr class=\"cf\">";
                            strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">#</th>";
                            strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">SKU</th>";
                            strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">Excel Row #s</th>";
                            strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">Error Description</th>";
                            strErrors += "</tr></thead>";
                            for (int k = 0; k < derror.Rows.Count; k++)
                            {


                                strErrors += "<tbody>";
                                strErrors += "<tr>";
                                strErrors += "<td align=\"left\">";
                                strErrors += k + 1;
                                strErrors += "</td>";
                                strErrors += "<td align=\"left\">";
                                strErrors += derror.Rows[k][1].ToString().Replace("'", "''");
                                strErrors += "</td>";
                                strErrors += "<td align=\"left\">";
                                strErrors += derror.Rows[k][0].ToString().Replace("'", "''");
                                strErrors += "</td>";
                                strErrors += "<td align=\"left\">";
                                strErrors += derror.Rows[k][2].ToString().Replace("'", "''");
                                strErrors += "</td>";
                                strErrors += "</tr>";
                                strErrors += "</tbody>";



                            }
                            strErrors += "</table>";
                        }


                        
                    }
                    ltrmsg.Text = strErrors;

                }
                else
                {
                    ltrmsg.Text = "Record Not Found";
                    return false;


                }
            }


            return true;
        }
        /// <summary>
        /// WriteFile For Writing Into File
        /// </summary>
        /// <param name="Text">String Text</param>
        /// <param name="FileName">String FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            FileInfo info = new FileInfo(FileName);
            writer = info.AppendText();
            writer.Write(Text);

            if (writer != null)
                writer.Close();
        }

        protected void btnexport1_Click(object sender, EventArgs e)
        {
            DataView dvCust = new DataView();
            DataSet ds = new DataSet();
            ds = CommonComponent.GetCommonDataSet("Exec GuiGetExportProduct 1");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dvCust = ds.Tables[0].DefaultView;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (dvCust != null)
                {
                    for (int i = 0; i < dvCust.Table.Rows.Count; i++)
                    {
                        object[] args = new object[7];
                        args[0] = Convert.ToString(dvCust.Table.Rows[i]["SKU"]);
                        args[1] = Convert.ToString(dvCust.Table.Rows[i]["UPC"]);

                        args[2] = Convert.ToString(dvCust.Table.Rows[i]["Discontinue"]);

                        sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\"", args));
                    }
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "ProductList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine("SKU,UPC,Discontinue");
                    sb.AppendLine(FullString);

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
    }
}