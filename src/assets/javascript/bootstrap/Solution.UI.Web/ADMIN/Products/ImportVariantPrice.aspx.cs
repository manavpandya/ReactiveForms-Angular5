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
    public partial class ImportVariantPrice : BasePage
    {
        string StrFileName = "";
        DataTable derror = new DataTable();
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
                        if (tempFieldName == "sku" || tempFieldName == "upc" || tempFieldName == "price" || tempFieldName == "basecustomprice")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",sku,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",upc,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",price,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",basecustomprice,") > -1)
                        {

                        }
                        else
                        {
                            lblMsg.Text = "Please Specify SKU,UPC,Price,BaseCustomPrice in file.";
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
                        lblMsg.Text = "Please Specify SKU,UPC,Price,BaseCustomPrice in file.";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    lblMsg.Text = "Please Specify SKU,UPC,Price,BaseCustomPrice in file.";
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



        private void adderror(int srno, string sku, string error)
        {
            DataRow derr = derror.NewRow();
            derr["SrNo"] = srno + 2;
            derr["SKU"] = sku;
            derr["Error"] = error;
            derror.Rows.Add(derr);
            derror.AcceptChanges();
        }

        private bool InsertDataInDataBase(DataTable dt)
        {
            ltrmsg.Text = "";
            int counter = 0;
            int errorcounter = 0;
            string productids = ",";
            int pid = 0;

            derror.Columns.Add("SrNo", typeof(int));
            derror.Columns.Add("SKU", typeof(string));
            derror.Columns.Add("Error", typeof(string));
            derror.AcceptChanges();
            DataSet Dscompare = new DataSet();
            Dscompare = CommonComponent.GetCommonDataSet("Exec GuiGetExportChildProduct 1");
            if (Dscompare != null && Dscompare.Tables.Count > 0 && Dscompare.Tables[0].Rows.Count > 0)
            {


                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        bool parent = false;
                        bool child = false;
                        if (String.IsNullOrEmpty(dt.Rows[i]["sku"].ToString()))
                        {
                            dt.Rows[i]["sku"] = "";
                            dt.AcceptChanges();
                        }
                        if (String.IsNullOrEmpty(dt.Rows[i]["upc"].ToString()))
                        {
                            dt.Rows[i]["upc"] = "";
                            dt.AcceptChanges();
                        }
                        if (String.IsNullOrEmpty(dt.Rows[i]["Price"].ToString()))
                        {
                            dt.Rows[i]["Price"] = 0;
                            dt.AcceptChanges();
                        }


                        if (String.IsNullOrEmpty(dt.Rows[i]["BasecustomPrice"].ToString()))
                        {
                            dt.Rows[i]["BasecustomPrice"] = 0;
                            dt.AcceptChanges();
                        }




                        Decimal beforePrice = decimal.Zero;
                        Decimal Price = decimal.Zero;

                        Decimal Basecustomprice = decimal.Zero;
                        Decimal beforeBasecustomprice = decimal.Zero;



                        DataRow[] Dr = Dscompare.Tables[0].Select("SKU='" + dt.Rows[i]["SKU"].ToString().Replace("'", "''").Trim() + "' and UPC='" + dt.Rows[i]["UPC"].ToString().Replace("'", "''").Trim() + "'");
                        if (Dr.Length > 0)
                        {
                            if (Dr[0]["type"].ToString().ToLower() == "parent")
                            {
                                parent = true;
                            }
                            else if (Dr[0]["type"].ToString().ToLower() == "child")
                            {
                                child = true;

                            }


                            Decimal.TryParse(Dr[0]["price"].ToString(), out beforePrice);
                            Decimal.TryParse(Dr[0]["Basecustomprice"].ToString(), out beforeBasecustomprice);





                            Decimal.TryParse(dt.Rows[i]["Price"].ToString(), out Price);
                            Decimal.TryParse(dt.Rows[i]["Basecustomprice"].ToString(), out Basecustomprice);


                            pid = Convert.ToInt32(Dr[0]["Productid"].ToString());
                            if (Price <= 0)
                            {
                                Price = 0;
                            }

                            if (Basecustomprice <= 0)
                            {
                                Basecustomprice = 0;
                            }





                         


                            if (child == true)
                            {
                                bool diffbasecustomprice = false;
                                bool diffprice = false;

                                if (Basecustomprice > 0)
                                {
                                    if (Basecustomprice != beforeBasecustomprice)
                                    {
                                        diffbasecustomprice = true;
                                    }
                                }
                                else
                                {
                                    diffbasecustomprice = true;
                                }



                                if (Price > 0)
                                {
                                    if (Price != beforePrice)
                                    {
                                        diffprice = true;
                                    }
                                }
                                else
                                {
                                    diffprice = true;
                                }

                                if(Price<=0 && Basecustomprice <=0)
                                {
                                    adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Please Specify Price and BaseCustomPrice");
                                    errorcounter++;
                                }

                                if (diffprice == true || diffbasecustomprice == true)
                                {

                                    if (Price > 0 && Basecustomprice > 0)
                                    {
                                       
                                        CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set VariantPrice=" + Price + ",BasecustomPrice=" + Basecustomprice + " where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and productid in (select distinct ProductID from tb_Product where isnull(Deleted,0)=0 and isnull(storeid,0)=1)");
                                        CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                        CommonComponent.ExecuteCommonData("Exec GuiInsertPriceLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + Price + "," + beforePrice + "," + Basecustomprice + "," + beforeBasecustomprice + ",'Auto'," + Session["AdminID"].ToString() + "");
                                        counter++;
                                    }
                                    else if (Price > 0 && diffprice==true)
                                    {
                                       
                                        CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set VariantPrice=" + Price + " where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and productid in (select distinct ProductID from tb_Product where isnull(Deleted,0)=0 and isnull(storeid,0)=1)");
                                        CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                        CommonComponent.ExecuteCommonData("Exec GuiInsertPriceLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + Price + "," + beforePrice + "," + Basecustomprice + "," + beforeBasecustomprice + ",'Auto'," + Session["AdminID"].ToString() + "");
                                        counter++;
                                    }
                                    else if (Basecustomprice > 0 && diffbasecustomprice==true)
                                    {
                                       
                                        CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set BasecustomPrice=" + Basecustomprice + " where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and productid in (select distinct ProductID from tb_Product where isnull(Deleted,0)=0 and isnull(storeid,0)=1)");
                                        CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                        CommonComponent.ExecuteCommonData("Exec GuiInsertPriceLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + Price + "," + beforePrice + "," + Basecustomprice + "," + beforeBasecustomprice + ",'Auto'," + Session["AdminID"].ToString() + "");
                                        counter++;
                                    }



                                }



                            }











                        }
                        else
                        {

                            //Int32 countinactive = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(*)  from tb_ProductVariantValue where ISNULL(sku,'')='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "' and isnull(upc,'')='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'  and ISNULL(VarActive,0)=0 and ProductID in (select ProductID from tb_Product where  ISNULL(Deleted,0)=0 and StoreID=1)"));
                            //if (countinactive > 0)
                            //{
                            //    adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "SKU Inactive");
                            //    errorcounter++;
                            //}
                            //else
                            {
                                adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "SKU-UPC Mismatch");
                                errorcounter++;
                            }



                        }



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
                                strErrors += derror.Rows[k][2].ToString();
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
            ds = CommonComponent.GetCommonDataSet("Exec GuiGetExportChildProduct 1");
            if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                dvCust = ds.Tables[0].DefaultView;
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (dvCust != null)
                {
                    for (int i = 0; i < dvCust.Table.Rows.Count; i++)
                    {
                        object[] args = new object[4];
                        args[0] = Convert.ToString(dvCust.Table.Rows[i]["SKU"]);
                        args[1] = Convert.ToString(dvCust.Table.Rows[i]["UPC"]);

                        Decimal Price = Decimal.Zero;
                        Decimal.TryParse(dvCust.Table.Rows[i]["Price"].ToString(), out Price);
                        args[2] = Convert.ToDecimal(Price);


                        Decimal BaseCustomPrice = Decimal.Zero;
                        Decimal.TryParse(dvCust.Table.Rows[i]["BaseCustomPrice"].ToString(), out BaseCustomPrice);

                        args[3] = Convert.ToDecimal(BaseCustomPrice);



                        sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\"", args));
                    }
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "ProductList_PriceImport_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine("SKU,UPC,Price,BaseCustomPrice");
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