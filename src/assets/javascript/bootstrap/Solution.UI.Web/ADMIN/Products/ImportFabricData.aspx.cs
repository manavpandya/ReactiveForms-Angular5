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
    public partial class ImportFabricData : BasePage
    {
        string StrFileName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            btnImport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/Import.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnexport1.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/sample-format-file.png) no-repeat transparent; width: 163px; height: 23px; border:none;cursor:pointer;");
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
                        if (tempFieldName == "fabric code" || tempFieldName == "name" || tempFieldName == "safety lock" || tempFieldName == "min alert qty" || tempFieldName == "delivery days" || tempFieldName == "per yard retail price" || tempFieldName == "discontinue" || tempFieldName == "active" || tempFieldName == "vendor" || tempFieldName == "upc" || tempFieldName == "minwidth" || tempFieldName == "maxwidth" || tempFieldName == "minlength" || tempFieldName == "maxlength")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",fabric code,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",name,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",safety lock,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",min alert qty,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",delivery days,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",per yard retail price,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",discontinue,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",active,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",vendor,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",upc,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",minwidth,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",maxwidth,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",minlength,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",maxlength,") > -1)
                        {

                        }
                        else
                        {
                            lblMsg.Text = "File Does not contain all columns";
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
                        //lblMsg.Text = "Please Specify SKU,SalePrice,BaseCustomPrice,DisplayOrder,Weight in file.";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    //lblMsg.Text = "Please Specify SKU,SalePrice,BaseCustomPrice,DisplayOrder,Weight in file.";
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
                        //lblMsg.Text = "Product Imported Successfully";
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

            DataSet DsVendors = new DataSet();

            DsVendors = CommonComponent.GetCommonDataSet("select VendorID,Name from tb_Vendor where isnull(Active,0)=1 and isnull(Deleted,0)=0");
            if (dt.Rows.Count > 0)
            {
                CommonComponent.ExecuteCommonData("DELETE FROM tb_importfabriccodedata");
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    string yardprice = "0";
                    string days = "0";
                    string minqty = "0";




                    string safetylock = "0";
                    bool Discontinue = false;
                    bool Active = false;
                    int vendorid = 0;
                    string UPC = "";

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Per Yard Retail Price"].ToString()))
                    {
                        yardprice = dt.Rows[i]["Per Yard Retail Price"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Min Alert Qty"].ToString()))
                    {
                        minqty = dt.Rows[i]["Min Alert Qty"].ToString();
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["Delivery Days"].ToString()))
                    {
                        days = dt.Rows[i]["Delivery Days"].ToString();
                    }


                    if (!String.IsNullOrEmpty(dt.Rows[i]["safety lock"].ToString()))
                    {
                        safetylock = dt.Rows[i]["safety lock"].ToString();
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Discontinue"].ToString()))
                    {
                        if (dt.Rows[i]["Discontinue"].ToString().Trim().ToLower() == "yes" || dt.Rows[i]["Discontinue"].ToString().Trim().ToLower() == "true")
                        {
                            Discontinue = true;
                        }
                        else
                        {
                            Discontinue = false;
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Active"].ToString()))
                    {
                        if (dt.Rows[i]["Active"].ToString().Trim().ToLower() == "yes" || dt.Rows[i]["Active"].ToString().Trim().ToLower() == "true")
                        {
                            Active = true;
                        }
                        else
                        {
                            Active = false;
                        }
                    }

                    if (!String.IsNullOrEmpty(dt.Rows[i]["Vendor"].ToString()))
                    {
                        if (DsVendors != null && DsVendors.Tables.Count > 0 && DsVendors.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] dr = DsVendors.Tables[0].Select("name='" + dt.Rows[i]["Vendor"].ToString().Trim() + "'");
                            if (dr.Length > 0)
                            {
                                vendorid = Convert.ToInt32(dr[0]["VendorID"].ToString());
                            }
                        }

                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["UPC"].ToString()))
                    {

                        UPC = dt.Rows[i]["UPC"].ToString().Trim().Replace("'", "''");
                    }
                    Int32 MinWidth=0;
                    Int32 MaxWidth=0;
                    Int32 MinLength = 0;
                    Int32 MaxLength = 0;
                    if (!String.IsNullOrEmpty(dt.Rows[i]["MinWidth"].ToString()))
                    {
                        Int32.TryParse(dt.Rows[i]["MinWidth"].ToString(), out MinWidth);
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["MaxWidth"].ToString()))
                    {
                        Int32.TryParse(dt.Rows[i]["MaxWidth"].ToString(), out MaxWidth);
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["MinLength"].ToString()))
                    {
                        Int32.TryParse(dt.Rows[i]["MinLength"].ToString(), out MinLength);
                    }
                    if (!String.IsNullOrEmpty(dt.Rows[i]["MaxLength"].ToString()))
                    {
                        Int32.TryParse(dt.Rows[i]["MaxLength"].ToString(), out MaxLength);
                    }
                    //if (String.IsNullOrEmpty(dt.Rows[i]["SalePrice"].ToString()))
                    //{
                    //    dt.Rows[i]["SalePrice"] = 0;
                    //    dt.AcceptChanges();
                    //}
                    //if (String.IsNullOrEmpty(dt.Rows[i]["BaseCustomPrice"].ToString()))
                    //{
                    //    dt.Rows[i]["BaseCustomPrice"] = 0;
                    //    dt.AcceptChanges();
                    //}
                    //if (String.IsNullOrEmpty(dt.Rows[i]["DisplayOrder"].ToString()))
                    //{
                    //    dt.Rows[i]["DisplayOrder"] = 0;
                    //    dt.AcceptChanges();
                    //}
                    //if (String.IsNullOrEmpty(dt.Rows[i]["Weight"].ToString()))
                    //{
                    //    dt.Rows[i]["Weight"] = 0;
                    //    dt.AcceptChanges();
                    //}
                    //try
                    //{
                    //    CommonComponent.ExecuteCommonData("update tb_product set SalePrice=" + dt.Rows[i]["SalePrice"].ToString() + ",DisplayOrder=" + dt.Rows[i]["DisplayOrder"].ToString() + ",[Weight]=" + dt.Rows[i]["Weight"].ToString() + " where storeid=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0 and SKU = '" + dt.Rows[i]["SKU"].ToString() + "'");
                    //    CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set VariantPrice=" + dt.Rows[i]["SalePrice"].ToString() + ", BaseCustomPrice=" + dt.Rows[i]["BaseCustomPrice"].ToString() + ",DisplayOrder=" + dt.Rows[i]["DisplayOrder"].ToString() + ",[Weight]=" + dt.Rows[i]["Weight"].ToString() + " where  SKU = '" + dt.Rows[i]["SKU"].ToString() + "' and ProductID in (select ProductID from tb_Product where StoreID=1 and isnull(Active,0)=1 and isnull(Deleted,0)=0)");
                    //}
                    //catch { }

                    string strquery = "INSERT INTO tb_importfabriccodedata(Code, Name,minqty, deliverydays,yardprice,safetylock,Discontinue,active,vendorid,FabricUPC,MinWidth,MaxWidth,MinLength,MaxLength) VALUES (";
                    strquery += "'" + dt.Rows[i]["Fabric Code"].ToString().Replace("'", "''") + "','" + dt.Rows[i]["Name"].ToString().Replace("'", "''") + "'," + minqty + "," + days + "," + yardprice + "," + safetylock + ",'" + Discontinue + "','" + Active + "'," + vendorid + ",'" + UPC + "'," + MinWidth + "," + MaxWidth + "," + MinLength + "," + MaxLength + "";
                    strquery += ")";
                    CommonComponent.ExecuteCommonData(strquery);
                }

                DataSet dsdat = new DataSet();

                dsdat = CommonComponent.GetCommonDataSet("SELECT * FROM tb_importfabriccodedata");
                if (dsdat != null && dsdat.Tables.Count > 0 && dsdat.Tables[0].Rows.Count > 0)
                {
                    lblMsg.Text = Convert.ToString(CommonComponent.GetScalarCommonData("EXEC usp_importfabriccodesdata"));
                }

            }
            else
            {
                return false;


            }


            return true;
        }
    }
}