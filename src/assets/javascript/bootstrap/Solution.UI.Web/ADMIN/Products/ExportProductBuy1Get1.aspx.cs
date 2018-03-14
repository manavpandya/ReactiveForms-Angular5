﻿using System;
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
    public partial class ExportProductBuy1Get1 : BasePage
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
                        if (tempFieldName == "sku" || tempFieldName == "upc" || tempFieldName == "isbuy1get1" || tempFieldName == "startdate" || tempFieldName == "enddate" || tempFieldName == "price")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",sku,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",upc,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",isbuy1get1,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",startdate,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",enddate,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",price,") > -1)
                        {

                        }
                        else
                        {
                            lblMsg.Text = "Please Specify SKU,UPC,IsBuy1Get1,Startdate,Enddate,Price in file.";
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
                        lblMsg.Text = "Please Specify SKU,UPC,IsBuy1Get1,Startdate,Enddate,Price  in file.";
                        lblMsg.Style.Add("color", "#FF0000");
                        lblMsg.Style.Add("font-weight", "normal");
                    }
                    //for (int cnt = 0; cnt < chkFields.Items.Count; cnt++)
                    //    chkFields.Items[cnt].Selected = true;
                }
                else
                {
                    lblMsg.Text = "Please Specify SKU,UPC,IsBuy1Get1,Startdate,Enddate,Price  in file.";
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
            Dscompare = CommonComponent.GetCommonDataSet("Exec GuiGetExportProductBuy1Get1 1");
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
                        if (String.IsNullOrEmpty(dt.Rows[i]["IsBuy1Get1"].ToString()))
                        {
                            dt.Rows[i]["IsBuy1Get1"] = 0;
                            dt.AcceptChanges();
                        }
                        bool IsBuy1Get1 = false;
                        if (!String.IsNullOrEmpty(dt.Rows[i]["IsBuy1Get1"].ToString()))
                        {

                            if (dt.Rows[i]["IsBuy1Get1"].ToString().Trim().ToLower() == "yes" || dt.Rows[i]["IsBuy1Get1"].ToString().Trim().ToLower() == "1")
                            {
                                IsBuy1Get1 = true;
                            }
                        }


                        bool BeforeIsBuy1Get1 = false;
                        DateTime Buy1Fromdate;
                        DateTime Buy1Todate;
                        DateTime BeforeBuy1fromdate;
                        DateTime beforeBuy1todate;
                        Decimal beforebuy1price = decimal.Zero;
                        Decimal buy1price = decimal.Zero;
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

                            if (Dr[0]["IsBuy1Get1"].ToString().ToLower() == "yes" || Dr[0]["IsBuy1Get1"].ToString().ToLower() == "1")
                            {
                                BeforeIsBuy1Get1 = true;

                            }
                            //try
                            //{
                            //    BeforeBuy1fromdate = Convert.ToDateTime(Dr[0]["Fromdate"].ToString());

                            //}
                            //catch { }

                            //try
                            //{
                            //    beforeBuy1todate = Convert.ToDateTime(Dr[0]["Todate"].ToString());
                            //}
                            //catch { }


                            //try
                            //{
                            //    Buy1Fromdate = Convert.ToDateTime(dt.Rows[i]["Startdate"].ToString());

                            //}
                            //catch
                            //{

                            //}
                            Decimal.TryParse(Dr[0]["price"].ToString(), out beforebuy1price);
                            DateTime.TryParse(Dr[0]["Fromdate"].ToString(), out BeforeBuy1fromdate);
                            DateTime.TryParse(Dr[0]["Todate"].ToString(), out beforeBuy1todate);

                            DateTime.TryParse(dt.Rows[i]["Startdate"].ToString(), out Buy1Fromdate);

                            DateTime.TryParse(dt.Rows[i]["Enddate"].ToString(), out Buy1Todate);
                            //Buy1Todate = Convert.ToDateTime(dt.Rows[i]["Enddate"].ToString());
                            Decimal.TryParse(dt.Rows[i]["price"].ToString(), out buy1price);
                            pid = Convert.ToInt32(Dr[0]["Productid"].ToString());
                            if (buy1price <= 0)
                            {
                                buy1price = 0;
                            }



                            if (dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''").ToLower().IndexOf("-cus") > -1)
                            {
                                buy1price = 0;
                            }

                            ///////////////check buy1get1 process


                            if (IsBuy1Get1 == true && child == true)
                            {
                                bool validfrom = false;
                                bool validend = false;

                                if (!String.IsNullOrEmpty(dt.Rows[i]["Startdate"].ToString()))
                                {


                                    if (!String.IsNullOrEmpty(dt.Rows[i]["Enddate"].ToString()))
                                    {

                                        try
                                        {
                                            Buy1Fromdate = Convert.ToDateTime(dt.Rows[i]["Startdate"].ToString());
                                            validfrom = true;
                                        }
                                        catch
                                        {
                                            adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Invalid Startdate");
                                            errorcounter++;
                                        }


                                        try
                                        {
                                            Buy1Todate = Convert.ToDateTime(dt.Rows[i]["Enddate"].ToString());
                                            validend = true;
                                        }
                                        catch
                                        {
                                            adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Invalid Enddate");
                                            errorcounter++;
                                        }


                                        if (validfrom && validend)
                                        {
                                            if ((Buy1Fromdate.ToString() != "" || Buy1Fromdate.ToString("MM/dd/yyyy") != "01/01/0001") && (Buy1Todate.ToString() != "" || Buy1Todate.ToString("MM/dd/yyyy") != "01/01/0001") && Buy1Todate > Buy1Fromdate)
                                            {
                                                if (Buy1Todate.Date >= DateTime.Now.Date)
                                                {
                                                    Int32 dsVariantVal2 = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(isnull(OnSale,0)) as OnSale  from tb_ProductVariantValue where  cast(OnSaleFromdate as date) <= cast(getdate() as date) and  cast(OnSaleTodate as date) >= cast(getdate() as date)  and isnull(OnSale,0)=1 and productid in (Select distinct ProductID  from tb_ProductVariantValue Where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "' and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'  and productid in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1))"));
                                                    if (dsVariantVal2 <= 0)
                                                    {
                                                        //  dsVariantVal2 = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(isnull(IsSaleclearance,0)) as IsSaleclearance  from tb_product Where sku='" + dt.Rows[i]["SKU"].ToString().Trim() + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim() + "' and isnull(IsSaleclearance,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1"));
                                                        // if (dsVariantVal2 <= 0)
                                                        // {
                                                        if (BeforeIsBuy1Get1 == IsBuy1Get1 && IsBuy1Get1 == false)
                                                        {

                                                        }
                                                        else if (BeforeIsBuy1Get1 == IsBuy1Get1 && IsBuy1Get1 == true)
                                                        {
                                                            if (BeforeBuy1fromdate.Date == Buy1Fromdate.Date && beforeBuy1todate.Date == Buy1Todate.Date && beforebuy1price == buy1price)
                                                            {

                                                            }
                                                            else
                                                            {
                                                                productids += pid + ",";
                                                                CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set Buy1Price=" + buy1price + ", Buy1Get1=1,Buy1Fromdate='" + Buy1Fromdate + "',Buy1Todate='" + Buy1Todate + "', OnSale=0 where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and ProductID in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1) ");
                                                                CommonComponent.ExecuteCommonData("Exec GuiInsertBuy1Get1Log '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + IsBuy1Get1 + ",'" + Buy1Fromdate + "','" + Buy1Todate + "'," + BeforeIsBuy1Get1 + ",'" + BeforeBuy1fromdate + "','" + beforeBuy1todate + "','Auto'," + Session["AdminID"].ToString() + "," + buy1price + "," + beforebuy1price + "");
                                                                CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                                                counter++;
                                                                //update
                                                            }
                                                        }
                                                        else if (BeforeIsBuy1Get1 != IsBuy1Get1)
                                                        {
                                                            //update 
                                                            if (IsBuy1Get1 == false)
                                                            {
                                                                productids += pid + ",";
                                                                CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set Buy1Get1=0,Buy1Price=0  where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and ProductID in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1) ");
                                                                CommonComponent.ExecuteCommonData("Exec GuiInsertBuy1Get1Log '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + IsBuy1Get1 + ",'" + Buy1Fromdate + "','" + Buy1Todate + "'," + BeforeIsBuy1Get1 + ",'" + BeforeBuy1fromdate + "','" + beforeBuy1todate + "','Auto'," + Session["AdminID"].ToString() + "," + buy1price + "," + beforebuy1price + "");
                                                                CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                                                counter++;
                                                            }
                                                            else
                                                            {
                                                                productids += pid + ",";
                                                                CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set Buy1Get1=1,Buy1Price=" + buy1price + ",Buy1Fromdate='" + Buy1Fromdate + "',Buy1Todate='" + Buy1Todate + "', OnSale=0 where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and ProductID in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1) ");
                                                                CommonComponent.ExecuteCommonData("Exec GuiInsertBuy1Get1Log '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + IsBuy1Get1 + ",'" + Buy1Fromdate + "','" + Buy1Todate + "'," + BeforeIsBuy1Get1 + ",'" + BeforeBuy1fromdate + "','" + beforeBuy1todate + "','Auto'," + Session["AdminID"].ToString() + "," + buy1price + "," + beforebuy1price + "");
                                                                CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                                                counter++;
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
                                                        adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Product is already as onsale");
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
                            else if (IsBuy1Get1 == false && child == true)
                            {
                                if (BeforeIsBuy1Get1 != IsBuy1Get1)
                                {
                                    //update
                                    productids += pid + ",";
                                    CommonComponent.ExecuteCommonData("update tb_ProductVariantValue set Buy1Get1=0,Buy1Price=0  where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and ProductID in (select ProductID from tb_Product where isnull(Active,0)=1 and ISNULL(Deleted,0)=0 and StoreID=1) ");
                                    CommonComponent.ExecuteCommonData("Exec GuiInsertBuy1Get1Log '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + IsBuy1Get1 + ",'" + Buy1Fromdate + "','" + Buy1Todate + "'," + BeforeIsBuy1Get1 + ",'" + BeforeBuy1fromdate + "','" + beforeBuy1todate + "','Auto'," + Session["AdminID"].ToString() + "," + buy1price + "," + beforebuy1price + "");
                                    CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                    counter++;
                                }

                            }
                            else if (parent == true)
                            {
                                if (IsBuy1Get1 != BeforeIsBuy1Get1)
                                {
                                    if (IsBuy1Get1 == true)
                                    {
                                        Int32 dsVariantVal2 = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(isnull(IsSaleclearance,0)) as IsSaleclearance  from tb_product Where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and isnull(IsSaleclearance,0)=1 and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1"));
                                        if (dsVariantVal2 <= 0)
                                        {
                                            //update
                                            productids += pid + ",";
                                            CommonComponent.ExecuteCommonData("update tb_product set IsShowBuy1Get1=1 where sku='" + dt.Rows[i]["SKU"].ToString().Trim() + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim() + "' and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1");
                                            CommonComponent.ExecuteCommonData("Exec GuiInsertBuy1Get1Log '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + IsBuy1Get1 + ",'" + Buy1Fromdate + "','" + Buy1Todate + "'," + BeforeIsBuy1Get1 + ",'" + BeforeBuy1fromdate + "','" + beforeBuy1todate + "','Auto'," + Session["AdminID"].ToString() + "," + buy1price + "," + beforebuy1price + "");
                                            CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                            counter++;
                                        }
                                        else
                                        {
                                            adderror(i, dt.Rows[i]["SKU"].ToString().Trim(), "Product is already as onsale");
                                            errorcounter++;
                                        }

                                    }
                                    else if (IsBuy1Get1 == false)
                                    {
                                        //update
                                        productids += pid + ",";
                                        CommonComponent.ExecuteCommonData("update tb_product set IsShowBuy1Get1=0 where sku='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "'  and upc='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "' and isnull(active,0)=1 and isnull(deleted,0)=0 and storeid=1");
                                        CommonComponent.ExecuteCommonData("Exec GuiInsertBuy1Get1Log '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'," + IsBuy1Get1 + ",'" + Buy1Fromdate + "','" + Buy1Todate + "'," + BeforeIsBuy1Get1 + ",'" + BeforeBuy1fromdate + "','" + beforeBuy1todate + "','Auto'," + Session["AdminID"].ToString() + "," + buy1price + "," + beforebuy1price + "");
                                        CommonComponent.ExecuteCommonData("Exec GuiUpdateSalePrice '" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "','" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'");
                                        counter++;
                                    }
                                }
                            }





                        }
                        else
                        {

                            Int32 countinactive = Convert.ToInt32(CommonComponent.GetScalarCommonData("select COUNT(*)  from tb_ProductVariantValue where ISNULL(sku,'')='" + dt.Rows[i]["SKU"].ToString().Trim().Replace("'", "''") + "' and isnull(upc,'')='" + dt.Rows[i]["upc"].ToString().Trim().Replace("'", "''") + "'  and ISNULL(VarActive,0)=0 and ProductID in (select ProductID from tb_Product where  ISNULL(Deleted,0)=0 and StoreID=1)"));
                            if (countinactive>0)
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



                    //if (!String.IsNullOrEmpty(productids) && productids.Length > 2)
                    //{
                    //    string[] arr = productids.ToString().Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                    //    if (arr.Length > 0)
                    //    {
                    //        for (int kk = 0; kk < arr.Length; kk++)
                    //        {
                    //            try
                    //            {
                    //                Int32 cnt = Convert.ToInt32(CommonComponent.GetScalarCommonData("select count(ProductID) from tb_ProductVariantValue  Where isnull(VarActive,0)=1 and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(Buy1Get1,0)=1 and isnull(Buy1Price,0)<>0 and ProductID=" + arr[kk].ToString() + ""));
                    //                if (cnt > 0)
                    //                {
                    //                    Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("select min(A.price) from (select isnull(Buy1Price,0) as Price from tb_ProductVariantValue  Where isnull(VarActive,0)=1 and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(Buy1Get1,0)=1 and isnull(Buy1Price,0)<>0 and ProductID=" + arr[kk].ToString() + " union all SELECT (isnull(Variantprice,0)) as Price FROM tb_ProductVariantValue WHERE   isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1 and isnull(Buy1Price,0)<=0 and isnull(VarActive,0)=1 and (cast(Buy1Fromdate as date) <=  cast(GETDATE() as date) and cast(Buy1Todate as date) >=cast(GETDATE() as date)) and isnull(VariantValue,'') not like '%custom%' and ISNULL(Buy1Get1,0)=1 and ProductID=" + arr[kk].ToString() + ") as A"));
                    //                    if (sprice > Decimal.Zero)
                    //                    {
                    //                        CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + arr[kk].ToString() + "");

                    //                    }
                    //                }
                    //                else
                    //                {
                    //                    Decimal sprice = Convert.ToDecimal(CommonComponent.GetScalarCommonData("SELECT MIn(isnull(Variantprice,0)) FROM tb_ProductVariantValue WHERE ProductID=" + arr[kk].ToString() + " and isnull(Variantprice,0) > 0 and isnull(VarActive,0)=1"));
                    //                    if (sprice > Decimal.Zero)
                    //                    {
                    //                        CommonComponent.ExecuteCommonData("update tb_Product SET  Saleprice='" + sprice + "' WHERE ProductID=" + arr[kk].ToString() + "");

                    //                    }
                    //                }

                    //            }
                    //            catch
                    //            {

                    //            }
                    //        }
                    //    }
                    //}

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
            ds = CommonComponent.GetCommonDataSet("Exec GuiGetExportProductBuy1Get1 1");
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
                        args[2] = Convert.ToString(dvCust.Table.Rows[i]["IsBuy1Get1"]);
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
                        Decimal Price = Decimal.Zero;
                        Decimal.TryParse(dvCust.Table.Rows[i]["Price"].ToString(), out Price);
                        args[5] = Convert.ToDecimal(Price);

                        sb.AppendLine(string.Format("{0},\"{1}\",\"{2}\",\"{3}\",\"{4}\",\"{5}\"", args));
                    }
                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {

                    DateTime dt = DateTime.Now;
                    String FileName = "ProductList_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine("SKU,UPC,IsBuy1Get1,Startdate,Enddate,Price");
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