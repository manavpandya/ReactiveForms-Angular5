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
    public partial class ExportProductFinalsale : BasePage
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
                        if (tempFieldName == "sku" || tempFieldName == "upc" || tempFieldName == "onsale" || tempFieldName == "startdate" || tempFieldName == "enddate" || tempFieldName == "saleprice")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",sku,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",upc,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",onsale,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",startdate,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",enddate,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",saleprice,") > -1)
                        {

                        }
                        else
                        {
                            lblMsg.Text = "Please Specify SKU,UPC,Onsale,Startdate,Enddate,SalePrice in file.";
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
                        lblMsg.Text = "Please Specify SKU,UPC,Onsale,Startdate,Enddate,SalePrice in file.";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    lblMsg.Text = "Please Specify SKU,UPC,Onsale,Startdate,Enddate,SalePrice in file.";
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


            derror.Columns.Add("SrNo", typeof(int));
            derror.Columns.Add("SKU", typeof(string));
            derror.Columns.Add("Error", typeof(string));
            derror.AcceptChanges();
            DataSet Dscompare = new DataSet();
            Dscompare = CommonComponent.GetCommonDataSet("Exec GuiGetExportProductFinalsale 1");
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
                        if (String.IsNullOrEmpty(dt.Rows[i]["OnSale"].ToString()))
                        {
                            dt.Rows[i]["OnSale"] = 0;
                            dt.AcceptChanges();
                        }
                        bool OnSale = false;
                        if (!String.IsNullOrEmpty(dt.Rows[i]["OnSale"].ToString()))
                        {

                            if (dt.Rows[i]["OnSale"].ToString().Trim().ToLower() == "yes" || dt.Rows[i]["OnSale"].ToString().Trim().ToLower() == "1")
                            {
                                OnSale = true;
                            }
                        }


                        bool BeforeOnSale = false;
                        DateTime OnsaleFromdate;
                        DateTime OnsaleTodate;
                        DateTime Beforeonsalefromdate;
                        DateTime beforeonsaletodate;
                        Decimal beforeonsaleprice = decimal.Zero;
                        Decimal onsaleprice = decimal.Zero;
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

                            if (Dr[0]["OnSale"].ToString().ToLower() == "yes" || Dr[0]["OnSale"].ToString().ToLower() == "1")
                            {
                                BeforeOnSale = true;

                            }

                            Decimal.TryParse(Dr[0]["saleprice"].ToString(), out beforeonsaleprice);


                            DateTime.TryParse(Dr[0]["Fromdate"].ToString(), out Beforeonsalefromdate);
                            DateTime.TryParse(Dr[0]["Todate"].ToString(), out beforeonsaletodate);

                            DateTime.TryParse(dt.Rows[i]["Startdate"].ToString(), out OnsaleFromdate);

                            DateTime.TryParse(dt.Rows[i]["Enddate"].ToString(), out OnsaleTodate);
                            //Buy1Todate = Convert.ToDateTime(dt.Rows[i]["Enddate"].ToString());

                            Decimal.TryParse(dt.Rows[i]["saleprice"].ToString(), out onsaleprice);

                            ///////////////check buy1get1 process


                            if (OnSale == true && child == true)
                            {
                                bool validfrom = false;
                                bool validend = false;

                                if (!String.IsNullOrEmpty(dt.Rows[i]["Startdate"].ToString()))
                                {


                                    if (!String.IsNullOrEmpty(dt.Rows[i]["Enddate"].ToString()))
                                    {

                                        try
                                        {
                                            OnsaleFromdate = Convert.ToDateTime(dt.Rows[i]["Startdate"].ToString());
                                            validfrom = true;
                                        }
                                        catch
                                        {
                                            adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Invalid Startdate");
                                            errorcounter++;
                                        }


                                        try
                                        {
                                            OnsaleTodate = Convert.ToDateTime(dt.Rows[i]["Enddate"].ToString());
                                            validend = true;
                                        }
                                        catch
                                        {
                                            adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Invalid Enddate");
                                            errorcounter++;
                                        }


                                        if (validfrom && validend)
                                        {
                                            if ((OnsaleFromdate.ToString() != "" || OnsaleFromdate.ToString("MM/dd/yyyy") != "01/01/0001") && (OnsaleTodate.ToString() != "" || OnsaleTodate.ToString("MM/dd/yyyy") != "01/01/0001") && OnsaleTodate > OnsaleFromdate)
                                            {
                                                if (OnsaleTodate.Date >= DateTime.Now.Date)
                                                {
                                                    Int32 dsVariantVal2 = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(isnull(Buy1Get1,0)) as Buy1Get1  from tb_ProductVariantValue where cast(Buy1Fromdate as date) <= cast(getdate() as date) and  cast(Buy1Todate as date) >= cast(getdate() as date) and isnull(Buy1Get1,0)=1 and productid in (Select distinct ProductID  from tb_ProductVariantValue Where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "' and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'  and productid in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1))"));
                                                    if (dsVariantVal2 <= 0)
                                                    {
                                                        //  dsVariantVal2 = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(isnull(IsSaleclearance,0)) as IsSaleclearance  from tb_product Where sku='" + dt.Rows[i]["SKU"].ToString().Trim() + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim() + "' and isnull(IsSaleclearance,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1"));
                                                        // if (dsVariantVal2 <= 0)
                                                        // {
                                                        if (BeforeOnSale == OnSale && OnSale == false)
                                                        {

                                                        }
                                                        else if (BeforeOnSale == OnSale && OnSale == true)
                                                        {
                                                            if (Beforeonsalefromdate.Date == OnsaleFromdate.Date && beforeonsaletodate.Date == OnsaleTodate.Date && onsaleprice == beforeonsaleprice)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                if (onsaleprice >= Decimal.Zero)
                                                                {


                                                                    CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set Buy1Get1=0, OnSale=1,OnSaleFromdate='" + OnsaleFromdate + "',OnSaleTodate='" + OnsaleTodate + "',OnSalePrice=" + onsaleprice + " where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and ProductID in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1) ");
                                                                    CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + OnSale + ",'" + OnsaleFromdate + "','" + OnsaleTodate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Auto'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                                                                    CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                                                    counter++;
                                                                }
                                                                else
                                                                {
                                                                    adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Saleprice should be greater then zero.");
                                                                    errorcounter++;
                                                                }
                                                                //update
                                                            }
                                                        }
                                                        else if (BeforeOnSale != OnSale)
                                                        {
                                                            //update 
                                                            if (OnSale == false)
                                                            {
                                                                CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set OnSale=0  where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and ProductID in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1) ");
                                                                CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + OnSale + ",'" + OnsaleFromdate + "','" + OnsaleTodate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Auto'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                                                                CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                                                counter++;
                                                            }
                                                            else
                                                            {
                                                                if (onsaleprice >= Decimal.Zero)
                                                                {
                                                                    CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set Buy1Get1=0,OnSale=1,OnSaleFromdate='" + OnsaleFromdate + "',OnSaleTodate='" + OnsaleTodate + "',OnSalePrice=" + onsaleprice + " where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and ProductID in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1) ");
                                                                    CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + OnSale + ",'" + OnsaleFromdate + "','" + OnsaleTodate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Auto'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                                                                    CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                                                    counter++;
                                                                }
                                                                else
                                                                {
                                                                    adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Saleprice should be greater then zero.");
                                                                    errorcounter++;
                                                                }

                                                            }

                                                        }
                                                        //}
                                                        //else
                                                        //{
                                                        //    adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Product is already as onsale");
                                                        //    errorcounter++;
                                                        //}
                                                    }
                                                    else
                                                    {
                                                        adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Product is already as Buy1Get1");
                                                        errorcounter++;
                                                    }
                                                }
                                                else
                                                {
                                                    adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Enddate should be greater or equal to today's date");
                                                    errorcounter++;
                                                }
                                            }
                                            else
                                            {
                                                adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Startdate should be less then Enddate");
                                                errorcounter++;

                                            }
                                        }


                                    }
                                    else
                                    {
                                        adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Enddate Missing");
                                        errorcounter++;
                                    }

                                }
                                else
                                {
                                    adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Startdate Missing");
                                    errorcounter++;
                                }



                            }
                            else if (OnSale == false && child == true)
                            {
                                if (BeforeOnSale != OnSale)
                                {
                                    //update
                                    CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set OnSale=0  where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and ProductID in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1) ");
                                    CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + OnSale + ",'" + OnsaleFromdate + "','" + OnsaleTodate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Auto'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                                    CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                    counter++;
                                }

                            }
                            else if (parent == true)
                            {
                                if (OnSale != BeforeOnSale)
                                {
                                    if (OnSale == true)
                                    {
                                        //Int32 dsVariantVal2 = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(isnull(IsSaleclearance,0)) as IsSaleclearance  from tb_product Where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and isnull(IsSaleclearance,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1"));
                                        //if (dsVariantVal2 <= 0)
                                        //{
                                        //update
                                        if (onsaleprice > Decimal.Zero)
                                        {
                                            CommonComponent.ExecuteCommonData("update tb_product set IsSaleclearance=1,SalePrice=" + onsaleprice + " where sku='" + dt.Rows[i]["SKU"].ToString().Trim() + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim() + "' and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1");
                                            CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + OnSale + ",'" + OnsaleFromdate + "','" + OnsaleTodate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Auto'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                                            counter++;
                                        }
                                        else
                                        {
                                            adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Saleprice should be greater then zero.");
                                            errorcounter++;
                                        }

                                        //}
                                        //else
                                        //{
                                        //    adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Product is already as Buy1Get1");
                                        //    errorcounter++;
                                        //}

                                    }
                                    else if (OnSale == false)
                                    {
                                        //update
                                        CommonComponent.ExecuteCommonData("update tb_product set IsSaleclearance=0 where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1");
                                        CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + OnSale + ",'" + OnsaleFromdate + "','" + OnsaleTodate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Auto'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                                        counter++;
                                    }
                                }
                                else if (OnSale == BeforeOnSale && onsaleprice != beforeonsaleprice)
                                {
                                    if (OnSale == true)
                                    {

                                        //update
                                        if (onsaleprice > Decimal.Zero)
                                        {
                                            CommonComponent.ExecuteCommonData("update tb_product set IsSaleclearance=1,SalePrice=" + onsaleprice + " where sku='" + dt.Rows[i]["SKU"].ToString().Trim() + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim() + "' and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1");
                                            CommonComponent.ExecuteCommonData("Exec GuiInsertonsaleLog '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + OnSale + ",'" + OnsaleFromdate + "','" + OnsaleTodate + "'," + BeforeOnSale + ",'" + Beforeonsalefromdate + "','" + beforeonsaletodate + "','Auto'," + Session["AdminID"].ToString() + "," + onsaleprice + "," + beforeonsaleprice + "");
                                            counter++;
                                        }
                                        else
                                        {
                                            adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Saleprice should be greater then zero.");
                                            errorcounter++;
                                        }


                                    }

                                }
                            }





                        }
                        else
                        {

                            Int32 countinactive = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(*)  from tb_ProductVariantValue where ISNULL(sku,'')='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "' and isnull(upc,'')='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'  and ISNULL(VarActive,0)=0 and ProductID in (select ProductID from tb_Product where  ISNULL(Deleted,0)=0 and StoreID=1)"));
                            if (countinactive > 0)
                            {
                                adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "SKU Inactive");
                                errorcounter++;
                            }
                            else
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
            ds = CommonComponent.GetCommonDataSet("Exec GuiGetExportProductFinalsale 1");
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
                        args[2] = Convert.ToString(dvCust.Table.Rows[i]["Onsale"]);
                        if (!String.IsNullOrEmpty(dvCust.Table.Rows[i]["Fromdate"].ToString()) && (dvCust.Table.Rows[i]["Fromdate"].ToString().IndexOf("1/1/1900") > -1 || dvCust.Table.Rows[i]["Fromdate"].ToString().IndexOf("01/01/1900") > -1))
                        {
                            args[3] = "";
                        }
                        else
                        {
                            args[3] = Convert.ToDateTime(Convert.ToString(dvCust.Table.Rows[i]["Fromdate"])).ToString("MM/dd/yyyy");
                        }
                        if (!String.IsNullOrEmpty(dvCust.Table.Rows[i]["Todate"].ToString()) && (dvCust.Table.Rows[i]["Todate"].ToString().IndexOf("1/1/1900") > -1 || dvCust.Table.Rows[i]["Todate"].ToString().IndexOf("01/01/1900") > -1))
                        {
                            args[4] = "";
                        }
                        else
                        {
                            args[4] = Convert.ToDateTime(Convert.ToString(dvCust.Table.Rows[i]["Todate"])).ToString("MM/dd/yyyy");
                        }
                        args[5] = Convert.ToString(dvCust.Table.Rows[i]["saleprice"]);

                        sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"", args));
                    }
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "ProductList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine("SKU,UPC,Onsale,Startdate,Enddate,SalePrice");
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