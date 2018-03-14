using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using AjaxControlToolkit;
using System.IO;
using Solution.Bussines.Components.Common;
using System.Threading;
using LumenWorks.Framework.IO.Csv;
using System.Data;
using Solution.Bussines.Components;

namespace Solution.UI.Web.REPLENISHMENTMANAGEMENT
{
    public partial class ImportReplenishmentData : System.Web.UI.Page
    {
        string fpath = "";
        Int32 Totalsku = 0;
        DataTable dterror = new DataTable();
        DataTable dtwaring = new DataTable();
        DataTable TempCSV = new DataTable();
        double ReplacementAddDays = Convert.ToDouble(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='ReplacementAddDays' and storeid=1"));
        int ReplacementAddDaysValidation = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='ReplacementAddDaysvalidation' and storeid=1"));
        int ReplacementAddDaysValidationETA = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='ReplacementAddDaysValidationETA' and storeid=1"));

        Int32 replnishedsku = 0;
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

        private Int32 FileID
        {
            get
            {
                if (ViewState["FileID"] == null)
                {
                    return 0;
                }
                else
                {
                    return (Convert.ToInt32(ViewState["FileID"].ToString()));
                }
            }
            set
            {
                ViewState["FileID"] = value;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["AdminID"] == null)
            {
                Response.Redirect("/Admin/login.aspx");
            }

            if (!IsPostBack)
            {
                AddColumnsErrortable();
                ImportReplenishmentComponent objrep = new ImportReplenishmentComponent();
                DataSet dslast = new DataSet();
                dslast = objrep.GetLastFileModified();
                if (dslast != null && dslast.Tables.Count > 0 && dslast.Tables[0].Rows.Count > 0)
                {
                    lbllastupdate.Visible = true;
                    lbllastupdatevalue.Visible = true;
                    lbllastupdateby.Visible = true;
                    lbllastupdatebyvalue.Visible = true;
                    //   DateTime time = Convert.ToDateTime(dslast.Tables[0].Rows[0]["createdon"].ToString());
                    lbllastupdatevalue.Text = String.Format("{0:MM/dd/yyyy hh:mm tt}", Convert.ToDateTime(dslast.Tables[0].Rows[0]["createdon"].ToString()));

                    //   TimeZoneInfo pstZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
                    //   DateTime pstDateTime = TimeZoneInfo.ConvertTimeFromUtc(Convert.ToDateTime(dslast.Tables[0].Rows[0]["createdon"].ToString()), pstZone);
                    //   lbllastupdatevalue.Text = pstDateTime.ToString();

                    //  TimeZoneInfo yourZone = TimeZoneInfo.FindSystemTimeZoneById("Pakistan Standard Time");
                    //   DateTime dt1 = DateTime.ParseExact("" + dslast.Tables[0].Rows[0]["createdon"].ToString() + " PST".Replace("PST", "+2"), "yyyy-mm-dd HH:mm z", yourZone);
                    lbllastupdatebyvalue.Text = dslast.Tables[0].Rows[0]["name"].ToString();

                }
                else
                {
                    lbllastupdate.Visible = false;
                    lbllastupdatevalue.Visible = false;
                    lbllastupdateby.Visible = false;
                    lbllastupdatebyvalue.Visible = false;
                }

            }
        }




        protected void btnimportfile_Click(object sender, EventArgs e)
        {
            try
            {
                if (Uploadcsv.HasFile && Path.GetExtension(Uploadcsv.FileName).ToLower() == ".csv")
                {
                    lblerrorsku.Text = "";
                    lblMsg.Text = "";
                    lblerrorsku.Visible = false;
                    lblerrorskumsg.Visible = false;

                    btnimportfile.Enabled = true;
                    StrFileName = Uploadcsv.FileName;
                    //Int32 Count = Convert.ToInt32(CommonComponent.GetScalarCommonData("select max(FileID)+1 from tb_ReplenishementFileLog"));


                    ImportReplenishmentComponent objRepCom = new ImportReplenishmentComponent();
                    FileID = objRepCom.InsertReplenishmentFileLOg(StrFileName, Convert.ToInt32(Session["AdminID"].ToString()), 1);
                    StrFileName = StrFileName.ToString().ToLower().Replace(".csv", "");
                    StrFileName = StrFileName + "_" + FileID + ".csv";

                    objRepCom.UpdateReplenishmentFileName(StrFileName, FileID, 2);
                    fpath = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(Configvalue,'') from tb_appconfig where Configname='ReplineshementFilePath' and storeid=1 "));

                    if (!Directory.Exists(Server.MapPath(fpath + "ProductCSV/ImportReplenishemntCSV/")))
                        Directory.CreateDirectory(Server.MapPath(fpath + "ProductCSV/ImportReplenishemntCSV/"));
                    // DeleteDocument(StrFileName);
                    Uploadcsv.SaveAs(Server.MapPath(fpath + "ProductCSV/ImportReplenishemntCSV/") + StrFileName);

                    FillMapping(StrFileName);

                    if(TempCSV!=null && TempCSV.Rows.Count>0)
                    {
                        DataRow[] ddd=TempCSV.Select("isnull(Flag,'') <>'0'");
                        if(ddd.Length>0)
                        {
                            lblreplenishedskucount.Text = ddd.Length.ToString();
                            btnUploadReplenishmentFile.Visible = true;
                            //if (Convert.ToInt32(lblskucount.Text) > 0)
                                btnuploadnext.Enabled = true;
                        }
                    }
                    else
                    {
                        if (Convert.ToInt32(lblreplenishedskucount.Text) < Convert.ToInt32(lblskucount.Text))
                        {
                            btnuploadnext.Enabled = false;
                            btnUploadReplenishmentFile.Visible = false;

                        }
                        else
                        {
                            btnUploadReplenishmentFile.Visible = true;
                            if (Convert.ToInt32(lblskucount.Text) > 0)
                                btnuploadnext.Enabled = true;
                        }

                    }


                   

                    divReset.Visible = true;
                    divFileName.Visible = true;
                    lblFileName.Text = ViewState["FileName"].ToString();

                    divImport.Visible = false;
                    divUpload.Visible = false;
                    //  Page.ClientScript.RegisterStartupScript(Page.GetType(), "Message", "alert('Your file has been uploaded successfully!!!');", true);

                }
                else
                {

                    ViewState["FileName"] = "";
                    ViewState["FileID"] = "";
                    //lblMsg.Text = "Please Select .csv File";
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message.ToString() + ex.StackTrace.ToString());
            }
        }



        /// <summary>
        /// Deletes the document.
        /// </summary>
        /// <param name="CustomerDoc">String CustomerDoc</param>
        private void DeleteDocument(string StrFileName)
        {
            try
            {
                string docPath = Server.MapPath(fpath + "ProductCSV/ImportReplenishemntCSV/") + StrFileName;
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

            FileInfo info = new FileInfo(Server.MapPath(fpath + "ProductCSV/ImportReplenishemntCSV/") + FileName);
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
                        string tempFieldName = FieldName.ToLower().Trim();
                        if (tempFieldName == "sku" || tempFieldName == "name" || tempFieldName == "po#1" || tempFieldName == "quantity ordered1" || tempFieldName == "shipping date1" || tempFieldName == "eta atl1" || tempFieldName == "po#2" || tempFieldName == "quantity ordered2" || tempFieldName == "shipping date2" || tempFieldName == "eta atl2" || tempFieldName == "po#3" || tempFieldName == "quantity ordered3" || tempFieldName == "shipping date3" || tempFieldName == "eta atl3" || tempFieldName == "po#4" || tempFieldName == "quantity ordered4" || tempFieldName == "shipping date4" || tempFieldName == "eta atl4")
                            FieldStrike += tempFieldName + ",";
                    }
                    if (FieldStrike.ToString().Length > 1)
                    {
                        if (FieldStrike.ToString().ToLower().IndexOf(",sku,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",name,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",po#1,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",quantity ordered1,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",shipping date1,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",eta atl1,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",po#2,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",quantity ordered2,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",shipping date2,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",eta atl2,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",po#3,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",quantity ordered3,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",shipping date3,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",eta atl3,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",po#4,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",quantity ordered4,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",shipping date4,") > -1 && FieldStrike.ToString().ToLower().IndexOf(",eta atl4,") > -1)
                        {

                        }
                        else
                        {
                            lblMsg.Text = "File format is not per specify";
                            ltrErrors.Text = "File format is not per specify";
                            btnuploadnext.Enabled = false;

                        }
                    }
                    else
                    {
                        // lblMsg.Text = "Please Specify SKU,Name,PO#1,Quantity Ordered1,Shipping Date1,PO#2,Quantity Ordered2,Shipping Date2,PO#3,Quantity Ordered3,Shipping Date3,PO#4,Quantity Ordered4,Shipping Date4 in file.";
                        lblMsg.Text = "File format is not per specify";
                        ltrErrors.Text = "File format is not per specify";
                        btnuploadnext.Enabled = false;
                    }
                    if (FieldStrike.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries).Length > 1 && String.IsNullOrEmpty(lblMsg.Text.ToString()))
                    {

                        BindData();

                    }
                    else
                    {
                        // lblMsg.Text = "Please Specify SKU,Name,PO#1,Quantity Ordered1,Shipping Date1,PO#2,Quantity Ordered2,Shipping Date2,PO#3,Quantity Ordered3,Shipping Date3,PO#4,Quantity Ordered4,Shipping Date4 in file.";
                        lblMsg.Text = "File format is not per specify";
                        ltrErrors.Text = "File format is not per specify";
                        btnuploadnext.Enabled = false;

                    }

                }
                else
                {
                    // lblMsg.Text = "Please Specify SKU,Name,PO#1,Quantity Ordered1,Shipping Date1,PO#2,Quantity Ordered2,Shipping Date2,PO#3,Quantity Ordered3,Shipping Date3,PO#4,Quantity Ordered4,Shipping Date4 in file.";
                    lblMsg.Text = "File format is not per specify";
                    ltrErrors.Text = "File format is not per specify";
                    btnuploadnext.Enabled = false;

                }
                csv.Dispose();
            }
        }
        /// <summary>
        /// Bind Data with Gridview
        /// </summary>
        private void BindData()
        {
            DataTable dtCSV = LoadCSV(StrFileName);
            if (dtCSV.Rows.Count > 0)
            {

                validatefiledata(dtCSV);

            }
            else
                lblMsg.Text = "No data exists in file.";


        }


        private void AddTemptable(DataTable dt)
        {
            TempCSV.Clear();
            TempCSV = dt.Copy();
            if (TempCSV != null && TempCSV.Rows.Count > 0)
            {

                DataColumn col1 = new DataColumn("Flag", typeof(string));
                TempCSV.Columns.Add(col1);
            }

        }


        private void AddColumnsErrortable()
        {
            DataColumn col1 = new DataColumn("Error Type", typeof(string));
            dterror.Columns.Add(col1);
            DataColumn col2 = new DataColumn("Excel Row Number", typeof(string));
            dterror.Columns.Add(col2);
            DataColumn col3 = new DataColumn("Product SKU", typeof(string));
            dterror.Columns.Add(col3);

            dterror.AcceptChanges();

        }

        private void InsertErrorinTemp(String ErrorType, String ExcelRowNumber, String ProductSKU)
        {
            DataRow dr = null;
            if (dterror.Columns.Count > 0)
            {
                dr = dterror.NewRow();
                dr["Error Type"] = ErrorType;
                dr["Excel Row Number"] = ExcelRowNumber;
                dr["Product SKU"] = ProductSKU;
                dterror.Rows.Add(dr);
                dterror.AcceptChanges();
            }
            else
            {
                AddColumnsErrortable();
                dr = dterror.NewRow();
                dr["Error Type"] = ErrorType;
                dr["Excel Row Number"] = ExcelRowNumber;
                dr["Product SKU"] = ProductSKU;
                dterror.Rows.Add(dr);
                dterror.AcceptChanges();

            }
        }


        private void UpdateFlag(string flag, string ExcelRowNumber)
        {

            ExcelRowNumber = (Convert.ToInt32(ExcelRowNumber) - 1).ToString();
            if (TempCSV != null && TempCSV.Rows.Count > 0)
            {
                DataRow[] drtemp = TempCSV.Select("Number='" + ExcelRowNumber + "'");

                if (drtemp.Length > 0)
                {
                    if(!String.IsNullOrEmpty(drtemp[0]["Flag"].ToString()))
                    {
                        int f = 0;
                        Int32.TryParse(drtemp[0]["Flag"].ToString(), out f);
                        int fout = 0;
                        Int32.TryParse(flag.ToString(), out fout);
                        if(fout<f)
                        {
                            drtemp[0]["Flag"] = flag.ToString().Trim();
                            TempCSV.AcceptChanges();
                        }

                    }
                    else
                    {
                        drtemp[0]["Flag"] = flag.ToString().Trim();
                        TempCSV.AcceptChanges();
                    }
                   


                   
                }
            }
        }


        private bool CheckDuplicateSku(String Sku, DataTable Dt)
        {

            String ErrorType = "Duplicate SKUs";
            String ExcelRowNumber = "";
            String ProductSKU = "";
            DataRow[] drerror = Dt.Select("sku ='" + Sku + "'");
            if (drerror.Length > 1)
            {
                for (int i = 0; i < drerror.Length; i++)
                {
                    ExcelRowNumber += Convert.ToInt32(drerror[i]["Number"].ToString()) + 1 + ",";
                    ProductSKU = drerror[i]["sku"].ToString();
                }


                if (ExcelRowNumber.ToString().Length > 2)
                {
                    ExcelRowNumber = ExcelRowNumber.ToString().Remove(ExcelRowNumber.ToString().LastIndexOf(","));
                }

                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                UpdateFlag("0", ExcelRowNumber);
                return false;

            }
            else
            {
                return true;
            }

        }





        private bool CheckSkuExist(String Sku, DataTable Dt)
        {
            String ErrorType = "Invalid SKU";
            String ExcelRowNumber = "";
            String ProductSKU = "";
            string checksku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_product where sku='" + Sku + "' and storeid=1 and  isnull(deleted,0)=0"));
            if (String.IsNullOrEmpty(checksku))
            {
                string checksubsku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_ProductVariantValue where sku='" + Sku + "' and productid in (select productid from tb_product where storeid=1  and isnull(deleted,0)=0)"));
                if (String.IsNullOrEmpty(checksubsku))
                {
                    DataRow[] drerror = Dt.Select("sku ='" + Sku + "'");
                    if (drerror.Length > 0)
                    {
                        for (int i = 0; i < drerror.Length; i++)
                        {
                            ExcelRowNumber += Convert.ToInt32(drerror[i]["Number"].ToString()) + 1 + ",";
                            ProductSKU = drerror[i]["sku"].ToString();
                        }


                        if (ExcelRowNumber.ToString().Length > 0)
                        {
                            ExcelRowNumber = ExcelRowNumber.ToString().Remove(ExcelRowNumber.ToString().LastIndexOf(","));
                        }
                        UpdateFlag("0", ExcelRowNumber);
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        return false;

                    }
                    else
                    {
                        return false;
                    }


                }
                else
                {
                    checksubsku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_ProductVariantValue where sku='" + Sku + "' and isnull(varactive,0)=1 and productid in (select productid from tb_product where storeid=1 and isnull(active,0)=1 and isnull(deleted,0)=0)"));
                    if (String.IsNullOrEmpty(checksubsku))
                    {
                        InsertErrorinTemp("Product is Inactive  SKU : " + Sku, "0", "");
                    }
                    return true;
                }

            }
            else
            {


                checksku = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 isnull(sku,'') from tb_product where sku='" + Sku + "' and storeid=1 and isnull(active,0)=1 and  isnull(deleted,0)=0"));
                if (String.IsNullOrEmpty(checksku))
                {
                    InsertErrorinTemp("Product is Inactive  SKU : " + Sku, "0", "");
                }
                return true;
            }

        }


        private bool CheckPOUnique(string po1, string po2, string po3, string po4, String Sku, Int32 RowNumber, Int32 POBlocks)
        {
            try
            {
                String ErrorType = "Invalid PO # Value";
                String ExcelRowNumber = "";
                String ProductSKU = "";

                DataTable tb = new DataTable();
                DataColumn col1 = new DataColumn("PO", typeof(string));
                tb.Columns.Add(col1);
                DataColumn col2 = new DataColumn("num", typeof(string));
                tb.Columns.Add(col2);
                tb.AcceptChanges();
                Int32 er = 0;

                if (POBlocks >= 1)
                {
                    if (!String.IsNullOrEmpty(po1))
                    {
                        DataRow dd = tb.NewRow();
                        dd["PO"] = po1;
                        dd["num"] = "1";
                        tb.Rows.Add(dd);
                        tb.AcceptChanges();
                    }
                }
                if (POBlocks >= 2)
                {
                    if (!String.IsNullOrEmpty(po2))
                    {
                        DataRow dd = tb.NewRow();
                        dd["PO"] = po2;
                        dd["num"] = "2";
                        tb.Rows.Add(dd);
                        tb.AcceptChanges();
                    }
                }
                if (POBlocks >= 3)
                {
                    if (!String.IsNullOrEmpty(po2))
                    {
                        DataRow dd = tb.NewRow();
                        dd["PO"] = po3;
                        dd["num"] = "3";
                        tb.Rows.Add(dd);
                        tb.AcceptChanges();
                    }
                }
                if (POBlocks >= 4)
                {
                    if (!String.IsNullOrEmpty(po2))
                    {
                        DataRow dd = tb.NewRow();
                        dd["PO"] = po4;
                        dd["num"] = "4";
                        tb.Rows.Add(dd);
                        tb.AcceptChanges();
                    }
                }

                if (tb.Rows.Count > 0)
                {
                    for (int j = 0; j < tb.Rows.Count; j++)
                    {
                        string q = Convert.ToString(tb.Rows[j][0].ToString());
                        for (int k = j + 1; k < tb.Rows.Count; k++)
                        {
                            string yy = Convert.ToString(tb.Rows[k][0].ToString());
                            if (q.ToString().ToLower() == yy.ToString().ToLower())
                            {
                                //string pp = Convert.ToString(tb.Rows[j][1].ToString());
                                string pp = Convert.ToString(tb.Rows[k][1].ToString());
                                ErrorType = "Invalid PO " + pp + " Value";
                                ExcelRowNumber = RowNumber.ToString();
                                ProductSKU = Sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag((Convert.ToInt32(pp) - 1).ToString(), ExcelRowNumber);
                                er++;
                            }
                        }


                    }


                    if (er > 0)
                    {
                        //ExcelRowNumber = RowNumber.ToString();
                        //ProductSKU = Sku;
                        //InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        return false;
                    }
                }
                else
                {
                    return true;
                }


                return true;
            }
            catch (Exception ex)
            {
                return false;
            }



        }


        private bool CheckDeliveryDateinsequence(string sku, string po1, string po2, string po3, string po4, string qty1, string qty2, string qty3, string qty4, string shipping1, string shipping2, string shipping3, string shipping4, Int32 RowNumber, string eta1, string eta2, string eta3, string eta4)
        {
            DateTime today = DateTime.Now;

            Int32 checkpocounter = 0;
            Int32 ErrorPOCounter = 0;
            Int32 Error = 0;


            if (!String.IsNullOrEmpty(sku))
            {

                if (String.IsNullOrEmpty(po1) && String.IsNullOrEmpty(qty1) && String.IsNullOrEmpty(shipping1) && String.IsNullOrEmpty(eta1))
                {
                    ErrorPOCounter++;

                }
                else
                {
                    if (ErrorPOCounter == 0)
                        checkpocounter = 1;
                }

                if (String.IsNullOrEmpty(po2) && String.IsNullOrEmpty(qty2) && String.IsNullOrEmpty(shipping2) && String.IsNullOrEmpty(eta2) && ErrorPOCounter == 0)
                {
                    ErrorPOCounter++;

                }
                else
                {
                    if (ErrorPOCounter == 0)
                        checkpocounter = 2;
                }

                if (String.IsNullOrEmpty(po3) && String.IsNullOrEmpty(qty3) && String.IsNullOrEmpty(shipping3) && String.IsNullOrEmpty(eta3) && ErrorPOCounter == 0)
                {
                    ErrorPOCounter++;

                }
                else
                {
                    if (ErrorPOCounter == 0)
                        checkpocounter = 3;
                }

                if (String.IsNullOrEmpty(po4) && String.IsNullOrEmpty(qty4) && String.IsNullOrEmpty(shipping4) && String.IsNullOrEmpty(eta4) && ErrorPOCounter == 0)
                {
                    ErrorPOCounter++;

                }
                else
                {
                    if (ErrorPOCounter == 0)
                        checkpocounter = 4;

                }

                //////////check Order Quantity validation

                for (int i = 1; i <= checkpocounter; i++)
                {

                    if (i == 1)
                    {
                        if (!String.IsNullOrEmpty(qty1))
                        {
                            Int32 q1 = 0;
                            Int32.TryParse(qty1, out q1);
                            if (q1 > 0)
                            {

                            }
                            else
                            {
                                String ErrorType = "Invalid Quantity 1 Value";
                                String ExcelRowNumber = Convert.ToString(RowNumber);
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }
                        else
                        {
                            String ErrorType = "Missing Quantity 1 Value";
                            String ExcelRowNumber = Convert.ToString(RowNumber);
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("0", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 2)
                    {
                        if (!String.IsNullOrEmpty(qty2))
                        {
                            Int32 q2 = 0;
                            Int32.TryParse(qty2, out q2);
                            if (q2 > 0)
                            {

                            }
                            else
                            {
                                String ErrorType = "Invalid Quantity 2 Value";
                                String ExcelRowNumber = Convert.ToString(RowNumber);
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }
                        else
                        {
                            String ErrorType = "Missing Quantity 2 Value";
                            String ExcelRowNumber = Convert.ToString(RowNumber);
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 3)
                    {
                        if (!String.IsNullOrEmpty(qty3))
                        {
                            Int32 q3 = 0;
                            Int32.TryParse(qty3, out q3);
                            if (q3 > 0)
                            {

                            }
                            else
                            {
                                String ErrorType = "Invalid Quantity 3 Value";
                                String ExcelRowNumber = Convert.ToString(RowNumber);
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }
                        else
                        {
                            String ErrorType = "Missing Quantity 3 Value";
                            String ExcelRowNumber = Convert.ToString(RowNumber);
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;
                        }

                    }



                    if (i == 4)
                    {
                        if (!String.IsNullOrEmpty(qty4))
                        {
                            Int32 q4 = 0;
                            Int32.TryParse(qty4, out q4);
                            if (q4 > 0)
                            {

                            }
                            else
                            {
                                String ErrorType = "Invalid Quantity 4 Value";
                                String ExcelRowNumber = Convert.ToString(RowNumber);
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }
                        else
                        {
                            String ErrorType = "Missing Quantity 4 Value";
                            String ExcelRowNumber = Convert.ToString(RowNumber);
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;
                        }

                    }




                }

                ////////////////// check PONumber validation

                //Int32 PO1 = 0;
                //Int32 PO2 = 0;
                //Int32 PO3 = 0;
                //Int32 PO4 = 0;
                //Int32.TryParse(po1, out PO1);
                //Int32.TryParse(po2, out PO2);
                //Int32.TryParse(po3, out PO3);
                //Int32.TryParse(po4, out PO4);


                if (checkpocounter >= 1)
                {
                    if (!String.IsNullOrEmpty(po1))
                    {



                        //if (PO1 > 0)
                        //{

                        //}
                        //else
                        //{
                        //    String ErrorType = "Invalid Value for 'PO1'";
                        //    String ExcelRowNumber = RowNumber.ToString();
                        //    String ProductSKU = sku;
                        //    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                        //    Error++;

                        //}
                    }
                    else
                    {
                        String ErrorType = "Missing PO 1 Value";
                        String ExcelRowNumber = RowNumber.ToString();
                        String ProductSKU = sku;
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        UpdateFlag("0", ExcelRowNumber);

                        Error++;

                    }

                }
                if (checkpocounter >= 2)
                {
                    if (!String.IsNullOrEmpty(po2))
                    {
                        //if (PO2 > 0)
                        //{

                        //}
                        //else
                        //{
                        //    String ErrorType = "Invalid Value for 'PO2'";
                        //    String ExcelRowNumber = RowNumber.ToString();
                        //    String ProductSKU = sku;
                        //    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                        //    Error++;

                        //}
                    }
                    else
                    {
                        String ErrorType = "Missing PO 2 Value";
                        String ExcelRowNumber = RowNumber.ToString();
                        String ProductSKU = sku;
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        UpdateFlag("1", ExcelRowNumber);

                        Error++;

                    }

                }
                if (checkpocounter >= 3)
                {
                    if (!String.IsNullOrEmpty(po3))
                    {
                        //if (PO3 > 0)
                        //{

                        //}
                        //else
                        //{
                        //    String ErrorType = "Invalid Value for 'PO3'";
                        //    String ExcelRowNumber = RowNumber.ToString();
                        //    String ProductSKU = sku;
                        //    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                        //    Error++;

                        //}
                    }
                    else
                    {
                        String ErrorType = "Missing PO 3 Value";
                        String ExcelRowNumber = RowNumber.ToString();
                        String ProductSKU = sku;
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        UpdateFlag("2", ExcelRowNumber);
                        Error++;

                    }

                }

                if (checkpocounter >= 4)
                {
                    if (!String.IsNullOrEmpty(po3))
                    {
                        //if (PO4 > 0)
                        //{

                        //}
                        //else
                        //{
                        //    String ErrorType = "Invalid Value for 'PO4'";
                        //    String ExcelRowNumber = RowNumber.ToString();
                        //    String ProductSKU = sku;
                        //    InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                        //    Error++;

                        //}
                    }
                    else
                    {
                        String ErrorType = "Missing PO 4 Value";
                        String ExcelRowNumber = RowNumber.ToString();
                        String ProductSKU = sku;
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        UpdateFlag("3", ExcelRowNumber);
                        Error++;

                    }

                }

                if (!CheckPOUnique(po1, po2, po3, po4, sku, RowNumber, checkpocounter))
                {
                    Error++;
                }



                //////////////check shipping date validation
                DateTime shippingdate1;
                DateTime shippingdate2;
                DateTime shippingdate3;
                DateTime shippingdate4;
                for (int i = 1; i <= checkpocounter; i++)
                {

                    if (i == 1)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(shipping1))
                            {
                                shippingdate1 = Convert.ToDateTime(shipping1);
                            }
                            else
                            {
                                String ErrorType = "Missing Date 1 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid Date 1 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("0", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 2)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(shipping2))
                            {
                                shippingdate2 = Convert.ToDateTime(shipping2);
                            }
                            else
                            {
                                String ErrorType = "Missing Date 2 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid Date 3 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 3)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(shipping3))
                            {
                                shippingdate3 = Convert.ToDateTime(shipping3);
                            }
                            else
                            {
                                String ErrorType = "Missing Date 3 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid Date 3 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;
                        }

                    }

                    if (i == 4)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(shipping4))
                            {
                                shippingdate4 = Convert.ToDateTime(shipping4);
                            }
                            else
                            {
                                String ErrorType = "Missing Date 4 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid Date 4 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;
                        }

                    }



                }




                ////////////////////validation for Date 1 < Date 2 < Date 3 < Date 4
                DateTime Etadate1;
                DateTime.TryParse(shipping1, out Etadate1);
                DateTime Etadate2;
                DateTime.TryParse(shipping2, out Etadate2);
                DateTime Etadate3;
                DateTime.TryParse(shipping3, out Etadate3);
                DateTime Etadate4;
                DateTime.TryParse(shipping4, out Etadate4);



                if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate1 = DateTime.Now;
                }
                if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate2 = DateTime.Now;
                }
                if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    //Etadate3 = DateTime.Now;
                }
                if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate4 = DateTime.Now;
                }
                //if ((Etadate1.Date < Etadate2.Date) && (Etadate2.Date < Etadate3.Date) && (Etadate3.Date < Etadate4.Date))
                //{

                //}
                //else
                //{

                //}
                if (checkpocounter >= 1)
                {
                    try
                    {


                        if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            Error++;
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("0", ExcelRowNumber);
                        }
                        else
                        {
                            if (today.Date.AddDays(-ReplacementAddDaysValidation) <= Etadate1.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong Date 1 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }

                }
                if (checkpocounter >= 2)
                {
                    try
                    {


                        if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etadate1.Date <= Etadate2.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong Date 2 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }


                if (checkpocounter >= 3)
                {
                    try
                    {


                        if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etadate2.Date <= Etadate3.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong Date 3 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }

                if (checkpocounter >= 4)
                {
                    try
                    {


                        if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etadate3.Date <= Etadate4.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong Date 4 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }












                //////////////check eta date validation




                DateTime etaatl1;
                DateTime etaatl2;
                DateTime etaatl3;
                DateTime etaatl4;
                for (int i = 1; i <= checkpocounter; i++)
                {

                    if (i == 1)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(eta1))
                            {
                                etaatl1 = Convert.ToDateTime(eta1);
                            }
                            else
                            {
                                String ErrorType = "Missing ETA ATL1 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid ETA ATL1 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("0", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 2)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(eta2))
                            {
                                etaatl2 = Convert.ToDateTime(eta2);
                            }
                            else
                            {
                                String ErrorType = "Missing ETA ATL2 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid ETA ATL3 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;
                        }

                    }


                    if (i == 3)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(eta3))
                            {
                                etaatl3 = Convert.ToDateTime(eta3);
                            }
                            else
                            {
                                String ErrorType = "Missing ETA ATL3 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid ETA ATL3 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;
                        }

                    }

                    if (i == 4)
                    {
                        try
                        {
                            if (!String.IsNullOrEmpty(eta4))
                            {
                                etaatl4 = Convert.ToDateTime(eta4);
                            }
                            else
                            {
                                String ErrorType = "Missing ETA ATL4 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }
                        catch
                        {


                            String ErrorType = "Invalid ETA ATL4 Value";
                            String ExcelRowNumber = RowNumber.ToString();
                            String ProductSKU = sku;
                            InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;
                        }

                    }



                }




                ////////////////////validation for Date 1 < Date 2 < Date 3 < Date 4
                DateTime Etaatldate1;
                DateTime.TryParse(eta1, out Etaatldate1);
                DateTime Etaatldate2;
                DateTime.TryParse(eta2, out Etaatldate2);
                DateTime Etaatldate3;
                DateTime.TryParse(eta3, out Etaatldate3);
                DateTime Etaatldate4;
                DateTime.TryParse(eta4, out Etaatldate4);



                if (Etaatldate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate1 = DateTime.Now;
                }
                if (Etaatldate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate2 = DateTime.Now;
                }
                if (Etaatldate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    //Etadate3 = DateTime.Now;
                }
                if (Etaatldate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                {
                    // Etadate4 = DateTime.Now;
                }
                //if ((Etadate1.Date < Etadate2.Date) && (Etadate2.Date < Etadate3.Date) && (Etadate3.Date < Etadate4.Date))
                //{

                //}
                //else
                //{

                //}
                if (checkpocounter >= 1)
                {
                    try
                    {


                        if (Etaatldate1.ToString() == "" || Etaatldate1.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            Error++;
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("0", ExcelRowNumber);
                        }
                        else
                        {
                            if (today.Date.AddDays(-ReplacementAddDaysValidationETA) <= Etaatldate1.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong ETA ATL1 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("0", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }

                }
                if (checkpocounter >= 2)
                {
                    try
                    {


                        if (Etaatldate2.ToString() == "" || Etaatldate2.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("1", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etaatldate1.Date <= Etaatldate2.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong ETA ATL2 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("1", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }


                if (checkpocounter >= 3)
                {
                    try
                    {


                        if (Etaatldate3.ToString() == "" || Etaatldate3.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("2", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etaatldate2.Date <= Etaatldate3.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong ETA ATL3 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("2", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }

                if (checkpocounter >= 4)
                {
                    try
                    {


                        if (Etaatldate4.ToString() == "" || Etaatldate4.ToString("MM/dd/yyyy") == "01/01/0001")
                        {
                            String ExcelRowNumber = RowNumber.ToString();
                            UpdateFlag("3", ExcelRowNumber);
                            Error++;

                        }
                        else
                        {
                            if (Etaatldate3.Date <= Etaatldate4.Date)
                            {

                            }
                            else
                            {
                                String ErrorType = "Wrong ETA ATL4 Value";
                                String ExcelRowNumber = RowNumber.ToString();
                                String ProductSKU = sku;
                                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                                UpdateFlag("3", ExcelRowNumber);
                                Error++;
                            }
                        }

                    }
                    catch
                    {

                    }
                }


            }
            else
            {
                String ErrorType = "Sku Should not be Blank";
                String ExcelRowNumber = Convert.ToString(RowNumber);
                String ProductSKU = sku;
                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                UpdateFlag("0", ExcelRowNumber);
                Error++;
            }

            if (checkpocounter == 0)
            {

                String ErrorType = "Remove sku or add PO Details in File";
                String ExcelRowNumber = Convert.ToString(RowNumber);
                String ProductSKU = sku;
                InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                InsertErrorinTemp("No Replenishment Details Found", "0", "");
                UpdateFlag("0", ExcelRowNumber);
                Error++;
            }
            if (Error > 0)
            {
                return false;

            }
            else
            {
                return true;

            }



            //  return true;
        }

        private void validatefiledata(DataTable Dt)
        {
            AddTemptable(Dt);
            Int32 po1 = 0;
            Int32 po2 = 0;
            Int32 po3 = 0;
            Int32 po4 = 0;
            Int32 qty1 = 0;
            Int32 qty2 = 0;
            Int32 qty3 = 0;
            Int32 qty4 = 0;
            DateTime shipping1;
            DateTime shipping2;
            DateTime shipping3;
            DateTime shipping4;
            String sku = "";
            String Name = "";
            Int32 error = 0;
            Int32 error2 = 0;
            int flag = 0;



            lblskucount.Text = Dt.Rows.Count.ToString();
            Totalsku = Dt.Rows.Count;
            lblerrorsku.Text = ",";
            for (int i = 0; i < Dt.Rows.Count; i++)
            {
                error = 0;
                error2 = 0;

                try
                {
                    if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
                    {
                        sku = Dt.Rows[i]["sku"].ToString();

                        //string checksku = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(sku,'') from tb_product where sku='" + sku + "' and storeid=1 and isnull(active,0)=1 and isnull(deleted,0)=0"));
                        //if(String.IsNullOrEmpty(checksku))
                        //{
                        //    string checksubsku = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(sku,'') from tb_ProductVariantValue where sku='" + sku + "' and productid in (select productid from tb_product where storeid=1 and isnull(active,0)=1 and isnull(deleted,0)=0)"));
                        //    if (String.IsNullOrEmpty(checksubsku))
                        //    {
                        //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //        error2++;
                        //    }

                        //}

                        if (!CheckSkuExist(sku, Dt))
                        {
                            if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                                lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            error2++;
                        }

                        if (!CheckDuplicateSku(sku, Dt))
                        {
                            if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                                lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            error2++;
                        }


                    }
                    else
                    {
                        String ErrorType = "Missing SKU";
                        String ExcelRowNumber = Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Number"].ToString()) + 1);
                        String ProductSKU = "";
                        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                        InsertErrorinTemp("No Replenishment Details Found", "0", "");
                        UpdateFlag("0", ExcelRowNumber);
                        error++;
                    }
                }
                catch
                {
                    if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    error++;

                }

                if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
                {



                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["name"].ToString()))
                        {
                            Name = Dt.Rows[i]["name"].ToString();
                        }
                        else
                        {
                            //error++;
                        }
                    }
                    catch
                    {
                        error++;
                    }


                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#1"].ToString()))
                        {
                            // po1 = Convert.ToInt32(Dt.Rows[i]["PO#1"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#2"].ToString()))
                        {
                            // po2 = Convert.ToInt32(Dt.Rows[i]["PO#2"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#3"].ToString()))
                        {
                            // po3 = Convert.ToInt32(Dt.Rows[i]["PO#3"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#4"].ToString()))
                        {
                            //po4 = Convert.ToInt32(Dt.Rows[i]["PO#4"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }



                    //if (!CheckPOUnique(po1, po2, po3, po4, sku, Convert.ToInt32(Dt.Rows[i]["Number"].ToString())))
                    //{
                    //    error++;
                    //}


                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered1"].ToString()))
                        {
                            qty1 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered1"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered2"].ToString()))
                        {
                            qty2 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered2"].ToString());
                        }
                        else
                        {
                            //    if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //        lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //    error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }
                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered3"].ToString()))
                        {
                            qty3 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered3"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }

                    try
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered4"].ToString()))
                        {
                            qty4 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered4"].ToString());
                        }
                        else
                        {
                            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                            //error++;
                        }
                    }
                    catch
                    {
                        //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                        //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        //error++;
                    }


                    if (!CheckDeliveryDateinsequence(sku, Convert.ToString(Dt.Rows[i]["PO#1"]), Convert.ToString(Dt.Rows[i]["PO#2"]), Convert.ToString(Dt.Rows[i]["PO#3"]), Convert.ToString(Dt.Rows[i]["PO#4"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered1"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered2"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered3"]), Convert.ToString(Dt.Rows[i]["Quantity Ordered4"]), Convert.ToString(Dt.Rows[i]["Shipping Date1"]), Convert.ToString(Dt.Rows[i]["Shipping Date2"]), Convert.ToString(Dt.Rows[i]["Shipping Date3"]), Convert.ToString(Dt.Rows[i]["Shipping Date4"]), Convert.ToInt32(Dt.Rows[i]["Number"].ToString()) + 1, Convert.ToString(Dt.Rows[i]["ETA ATL1"]), Convert.ToString(Dt.Rows[i]["ETA ATL2"]), Convert.ToString(Dt.Rows[i]["ETA ATL3"]), Convert.ToString(Dt.Rows[i]["ETA ATL4"])))
                    {
                        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                        error++;
                    }


                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(Dt.Rows[i]["Shipping Date1"].ToString()))
                    //        {
                    //            shipping1 = Convert.ToDateTime(Dt.Rows[i]["Shipping Date1"].ToString());
                    //        }
                    //        else
                    //        {
                    //            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //            //error++;
                    //        }
                    //    }
                    //    catch
                    //    {


                    //        String ErrorType = "Invalid Date Format for 'Date 1'";
                    //        String ExcelRowNumber = Dt.Rows[i]["Number"].ToString();
                    //        String ProductSKU = sku;
                    //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);

                    //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //        error++;
                    //    }


                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(Dt.Rows[i]["Shipping Date2"].ToString()))
                    //        {
                    //            shipping2 = Convert.ToDateTime(Dt.Rows[i]["Shipping Date2"].ToString());
                    //        }
                    //        else
                    //        {
                    //            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //            //error++;
                    //        }
                    //    }
                    //    catch
                    //    {
                    //        String ErrorType = "Invalid Date Format for 'Date 2'";
                    //        String ExcelRowNumber = Dt.Rows[i]["Number"].ToString();
                    //        String ProductSKU = sku;
                    //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                    //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //        error++;



                    //    }

                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(Dt.Rows[i]["Shipping Date3"].ToString()))
                    //        {
                    //            shipping3 = Convert.ToDateTime(Dt.Rows[i]["Shipping Date3"].ToString());
                    //        }
                    //        else
                    //        {
                    //            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //            //error++;
                    //        }
                    //    }
                    //    catch
                    //    {
                    //        String ErrorType = "Invalid Date Format for 'Date 3'";
                    //        String ExcelRowNumber = Dt.Rows[i]["Number"].ToString();
                    //        String ProductSKU = sku;
                    //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                    //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //        error++;
                    //    }


                    //    try
                    //    {
                    //        if (!String.IsNullOrEmpty(Dt.Rows[i]["Shipping Date4"].ToString()))
                    //        {
                    //            shipping4 = Convert.ToDateTime(Dt.Rows[i]["Shipping Date4"].ToString());
                    //        }
                    //        else
                    //        {
                    //            //if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            //    lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //            //error++;
                    //        }
                    //    }
                    //    catch
                    //    {
                    //        String ErrorType = "Invalid Date Format for 'Date 4'";
                    //        String ExcelRowNumber = Dt.Rows[i]["Number"].ToString();
                    //        String ProductSKU = sku;
                    //        InsertErrorinTemp(ErrorType, ExcelRowNumber, ProductSKU);
                    //        if (lblerrorsku.Text.ToString().IndexOf("," + Dt.Rows[i]["sku"].ToString() + ",") <= -1)
                    //            lblerrorsku.Text += Dt.Rows[i]["sku"].ToString() + ",";
                    //        error++;
                    //    }

                }

                // Int32 c = dterror.Rows.Count;

                if (error == 0 && error2 == 0)
                {
                    replnishedsku = replnishedsku + 1;
                    String ExcelRowNumber = Convert.ToString(Convert.ToInt32(Dt.Rows[i]["Number"].ToString()) + 1);
                    UpdateFlag("4", ExcelRowNumber);
                }
                else
                {
                    //if (lblMsg.Text.ToString().IndexOf("Data does not match field data format") <= -1)
                    //    lblMsg.Text += "Data does not match field data format";
                }

            }

            ViewState["temptable"] = TempCSV;
            String strErrors = "";
            if (dterror.Rows.Count > 0)
            {
                strErrors += "<table class=\"table table-bordered table-striped table-condensed cf\">";
                strErrors += "<thead><tr class=\"cf\">";
                strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">#</th>";
                strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">SKU</th>";
                strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">Excel Row #s</th>";
                strErrors += "<th class=\"nt1\" align=\"left\" valign=\"middle\" scope=\"col\">Error Description</th>";
                strErrors += "</tr></thead>";
                for (int k = 0; k < dterror.Rows.Count; k++)
                {

                    if (dterror.Rows[k][1].ToString().Replace("'", "''") == "0")
                    {
                        strErrors += "<tbody>";
                        strErrors += "<tr>";
                        strErrors += "<td align=\"center\" colspan=\"4\" style=\"color:red;\">";
                        strErrors += "Warning Message: " + dterror.Rows[k][0].ToString().Replace("'", "''");
                        strErrors += "</td>";
                        strErrors += "</tr>";
                        strErrors += "</tbody>";
                    }
                    else
                    {
                        strErrors += "<tbody>";
                        strErrors += "<tr>";
                        strErrors += "<td align=\"left\">";
                        strErrors += k + 1;
                        strErrors += "</td>";
                        strErrors += "<td align=\"left\">";
                        strErrors += dterror.Rows[k][2].ToString().Replace("'", "''");
                        strErrors += "</td>";
                        strErrors += "<td align=\"left\">";
                        strErrors += dterror.Rows[k][1].ToString().Replace("'", "''");
                        strErrors += "</td>";
                        strErrors += "<td align=\"left\">";
                        strErrors += dterror.Rows[k][0].ToString().Replace("'", "''");
                        strErrors += "</td>";
                        strErrors += "</tr>";
                        strErrors += "</tbody>";
                    }


                }
                strErrors += "</table>";
            }
            ltrErrors.Text = "";
            ltrErrors.Text = strErrors;
            if (String.IsNullOrEmpty(strErrors.ToString()))
            {
                // diverror.Visible = false;
                ltrErrors.Text = "No errors found";
                //lblMsg.Text = "Successfull";
            }

            lblreplenishedskucount.Text = replnishedsku.ToString();
            try
            {
                lblerrorsku.Text = lblerrorsku.Text.ToString().Remove(lblerrorsku.Text.ToString().LastIndexOf(","));
            }
            catch { }
            if (!String.IsNullOrEmpty(lblerrorsku.Text) && lblerrorsku.Text.ToString().Length > 2)
            {
                lblerrorsku.Visible = false;
                lblerrorskumsg.Visible = false;
            }
            else
            {
                lblerrorsku.Visible = false;
                lblerrorskumsg.Visible = false;
            }


        }


        /// <summary>
        /// Display CSV File data in Grid
        /// </summary>
        /// <param name="FileName">FileName</param>
        /// <returns>DataTable</returns>
        private DataTable LoadCSV(string FileName)
        {
            fpath = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(Configvalue,'') from tb_appconfig where Configname='ReplineshementFilePath' and storeid=1 "));
            FileInfo info = new FileInfo(Server.MapPath(fpath + "ProductCSV/ImportReplenishemntCSV/") + FileName);
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

        protected void btnUploadReplenishmentFile_Click(object sender, EventArgs e)
        {

            int counter = 0;
            if (Convert.ToInt32(lblreplenishedskucount.Text.ToString()) > 0 && !String.IsNullOrEmpty(StrFileName))
            {
                DataTable Dt = new DataTable();
             

                if(ViewState["temptable"]!=null)
                {
                    Dt = (DataTable)ViewState["temptable"];
                }
                else
                {
                    Dt = LoadCSV(StrFileName);
                }
                if (Dt != null && Dt.Rows.Count > 0)
                {

                    CommonComponent.ExecuteCommonData("INSERT INTO tb_Replenishment_Backup(ProductID, VariantValueID, PO1, qty1,shipping1, Etadate1, PO2, qty2,shipping2, Etadate2, PO3, qty3,shipping3, Etadate3, PO4, qty4,shipping4, Etadate4, CreatedOn, IsDiscontinued, IsRepenishable, IsBackorderable, LeadTime, sku, Name, CreatedBy, UpdatedBy, UpdatedOn, FileID) SELECT ProductID, VariantValueID, PO1, qty1,shipping1, Etadate1, PO2, qty2,shipping2, Etadate2, PO3, qty3,shipping3, Etadate3, PO4, qty4,shipping4, Etadate4, CreatedOn, IsDiscontinued, IsRepenishable, IsBackorderable, LeadTime, sku, Name, CreatedBy, UpdatedBy, UpdatedOn, FileID FROM tb_Replenishment");
                    CommonComponent.ExecuteCommonData("DELETE FROM tb_Replenishment");
                    for (int i = 0; i < Dt.Rows.Count; i++)
                    {
                        if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()) && Dt.Rows[i]["flag"].ToString()!="0")
                        {

                            int checkflag = 0;
                            Int32.TryParse(Dt.Rows[i]["flag"].ToString(), out checkflag);

                            string po1 = "";
                            string po2 = "";
                            string po3 = "";
                            string po4 = "";
                            Int32 qty1 = 0;
                            Int32 qty2 = 0;
                            Int32 qty3 = 0;
                            Int32 qty4 = 0;
                            TextBox txtShippingdate1 = new TextBox();
                            TextBox txtShippingdate2 = new TextBox();
                            TextBox txtShippingdate3 = new TextBox();
                            TextBox txtShippingdate4 = new TextBox();
                            TextBox txtEtadate1 = new TextBox();
                            TextBox txtEtadate2 = new TextBox();
                            TextBox txtEtadate3 = new TextBox();
                            TextBox txtEtadate4 = new TextBox();

                            String sku = "";
                            String Name = "";
                            String PID = "";
                            String VariantValueID = "";



                            sku = Dt.Rows[i]["sku"].ToString();
                           // if (lblerrorsku.Text.ToString().IndexOf("," + sku + ",") <= -1)
                           // {


                                try
                                {
                                    if (!String.IsNullOrEmpty(Dt.Rows[i]["sku"].ToString()))
                                    {
                                        sku = Dt.Rows[i]["sku"].ToString();
                                    }

                                }
                                catch
                                {

                                }

                                try
                                {
                                    if (!String.IsNullOrEmpty(Dt.Rows[i]["name"].ToString()))
                                    {
                                        Name = Dt.Rows[i]["name"].ToString();
                                    }
                                    else
                                    {

                                    }
                                }
                                catch
                                {

                                }




                                if(checkflag>=1)
                                {

                                    try
                                    {
                                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#1"].ToString()))
                                        {
                                            po1 = Convert.ToString(Dt.Rows[i]["PO#1"].ToString());
                                        }
                                        else
                                        {
                                        }
                                    }
                                    catch
                                    {

                                    }


                                    try
                                    {
                                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered1"].ToString()))
                                        {
                                            qty1 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered1"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        DateTime shipppingdate1;
                                        DateTime.TryParse(Dt.Rows[i]["Shipping Date1"].ToString(), out shipppingdate1);
                                        if (shipppingdate1.ToString() == "" || shipppingdate1.ToString("MM/dd/yyyy") == "01/01/0001")
                                        {
                                            // txtEtadate1.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                        }
                                        else
                                        {

                                            //txtEtadate1.Text = Etadate1.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                            txtShippingdate1.Text = shipppingdate1.ToString("MM/dd/yyyy");
                                        }
                                    }
                                    catch { }

                                    try
                                    {
                                        DateTime Etadate1;
                                        DateTime.TryParse(Dt.Rows[i]["ETA ATL1"].ToString(), out Etadate1);

                                        if (Etadate1.ToString() == "" || Etadate1.ToString("MM/dd/yyyy") == "01/01/0001")
                                        {
                                            // txtEtadate1.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                        }
                                        else
                                        {

                                            //txtEtadate1.Text = Etadate1.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                            txtEtadate1.Text = Etadate1.ToString("MM/dd/yyyy");
                                        }
                                    }
                                    catch { }

                                }






                                if(checkflag>=2)
                                {
                                    try
                                    {
                                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#2"].ToString()))
                                        {
                                            po2 = Convert.ToString(Dt.Rows[i]["PO#2"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered2"].ToString()))
                                        {
                                            qty2 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered2"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch
                                    {

                                    }


                                    try
                                    {
                                        DateTime shipppingdate2;
                                        DateTime.TryParse(Dt.Rows[i]["Shipping Date2"].ToString(), out shipppingdate2);
                                        if (shipppingdate2.ToString() == "" || shipppingdate2.ToString("MM/dd/yyyy") == "01/01/0001")
                                        {
                                            //  txtEtadate2.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                        }
                                        else
                                        {
                                            //txtEtadate2.Text = Etadate2.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                            txtShippingdate2.Text = shipppingdate2.ToString("MM/dd/yyyy");
                                        }
                                    }
                                    catch { }

                                    try
                                    {
                                        DateTime Etadate2;
                                        DateTime.TryParse(Dt.Rows[i]["ETA ATL2"].ToString(), out Etadate2);
                                        if (Etadate2.ToString() == "" || Etadate2.ToString("MM/dd/yyyy") == "01/01/0001")
                                        {
                                            //  txtEtadate2.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                        }
                                        else
                                        {
                                            //txtEtadate2.Text = Etadate2.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                            txtEtadate2.Text = Etadate2.ToString("MM/dd/yyyy");
                                        }
                                    }
                                    catch { }

                                }




                                if(checkflag>=3)
                                {
                                    try
                                    {
                                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#3"].ToString()))
                                        {
                                            po3 = Convert.ToString(Dt.Rows[i]["PO#3"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered3"].ToString()))
                                        {
                                            qty3 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered3"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch
                                    {

                                    }


                                    try
                                    {
                                        DateTime shipppingdate3;
                                        DateTime.TryParse(Dt.Rows[i]["Shipping Date3"].ToString(), out shipppingdate3);
                                        if (shipppingdate3.ToString() == "" || shipppingdate3.ToString("MM/dd/yyyy") == "01/01/0001")
                                        {
                                            // txtEtadate3.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                        }
                                        else
                                        {
                                            //txtEtadate3.Text = Etadate3.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                            txtShippingdate3.Text = shipppingdate3.ToString("MM/dd/yyyy");
                                        }
                                    }
                                    catch { }


                                    try
                                    {
                                        DateTime Etadate3;
                                        DateTime.TryParse(Dt.Rows[i]["ETA ATL3"].ToString(), out Etadate3);
                                        if (Etadate3.ToString() == "" || Etadate3.ToString("MM/dd/yyyy") == "01/01/0001")
                                        {
                                            // txtEtadate3.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                        }
                                        else
                                        {
                                            //txtEtadate3.Text = Etadate3.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                            txtEtadate3.Text = Etadate3.ToString("MM/dd/yyyy");
                                        }
                                    }
                                    catch { }
                                }



                                if(checkflag>=4)
                                {
                                    try
                                    {
                                        if (!String.IsNullOrEmpty(Dt.Rows[i]["PO#4"].ToString()))
                                        {
                                            po4 = Convert.ToString(Dt.Rows[i]["PO#4"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {
                                        if (!String.IsNullOrEmpty(Dt.Rows[i]["Quantity Ordered4"].ToString()))
                                        {
                                            qty4 = Convert.ToInt32(Dt.Rows[i]["Quantity Ordered4"].ToString());
                                        }
                                        else
                                        {

                                        }
                                    }
                                    catch
                                    {

                                    }
                                    try
                                    {



                                        DateTime shipppingdate4;
                                        DateTime.TryParse(Dt.Rows[i]["Shipping Date4"].ToString(), out shipppingdate4);

                                        if (shipppingdate4.ToString() == "" || shipppingdate4.ToString("MM/dd/yyyy") == "01/01/0001")
                                        {
                                            //  txtEtadate4.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                        }
                                        else
                                        {
                                            //txtEtadate4.Text = Etadate4.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                            txtShippingdate4.Text = shipppingdate4.ToString("MM/dd/yyyy");
                                        }
                                    }
                                    catch
                                    {

                                    }

                                    try
                                    {



                                        DateTime Etadate4;
                                        DateTime.TryParse(Dt.Rows[i]["ETA ATL4"].ToString(), out Etadate4);

                                        if (Etadate4.ToString() == "" || Etadate4.ToString("MM/dd/yyyy") == "01/01/0001")
                                        {
                                            //  txtEtadate4.Text = DateTime.Now.AddDays(ReplacementAddDays).ToString();
                                        }
                                        else
                                        {
                                            //txtEtadate4.Text = Etadate4.AddDays(ReplacementAddDays).ToString("MM/dd/yyyy");
                                            txtEtadate4.Text = Etadate4.ToString("MM/dd/yyyy");
                                        }

                                    }
                                    catch { }
                                }

                               
                               


                                try
                                {

                                    PID = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 productid from tb_product where isnull(sku,'')='" + sku + "' and isnull(deleted,0)=0 and storeid=1 order by productid Desc"));
                                }
                                catch { }
                                try
                                {
                                    if (String.IsNullOrEmpty(PID))
                                    {
                                        DataSet dsproduct = new DataSet();
                                        dsproduct = (CommonComponent.GetCommonDataSet("select top 1 VariantValueID,productid from tb_ProductVariantValue where isnull(sku,'')='" + sku + "' and productid in (select productid from tb_product where isnull(deleted,0)=0 and storeid=1) order by VariantValueID Desc "));
                                        if (dsproduct != null && dsproduct.Tables.Count > 0 && dsproduct.Tables[0].Rows.Count > 0)
                                        {
                                            PID = dsproduct.Tables[0].Rows[0]["productid"].ToString();
                                            VariantValueID = dsproduct.Tables[0].Rows[0]["VariantValueID"].ToString();
                                        }

                                    }
                                    else
                                    {
                                        VariantValueID = "0";
                                    }
                                }
                                catch
                                {

                                }


                                if (!String.IsNullOrEmpty(PID) && !String.IsNullOrEmpty(VariantValueID))
                                {
                                    Int32 count = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select count(productid) from tb_Replenishment where ProductID=" + PID + " and VariantValueID=" + VariantValueID + ""));
                                    if (count > 0)
                                    {
                                        //update

                                        CommonComponent.ExecuteCommonData("update [dbo].[tb_Replenishment] set [PO1]='" + po1 + "',[qty1]=" + qty1 + ",[Shipping1]='" + txtShippingdate1.Text + "',[Etadate1]='" + txtEtadate1.Text + "',[PO2]='" + po2 + "',[qty2]=" + qty2 + ",[Shipping2]='" + txtShippingdate2.Text + "',[Etadate2]='" + txtEtadate2.Text + "',[PO3]='" + po3 + "',[qty3]=" + qty3 + ",[Shipping3]='" + txtShippingdate3.Text + "',[Etadate3]='" + txtEtadate3.Text + "',[PO4]='" + po4 + "',[qty4]=" + qty4 + ",[Shipping4]='" + txtShippingdate4.Text + "',[Etadate4]='" + txtEtadate4.Text + "',sku='" + sku + "',name='" + Name.ToString().Replace("'","''") + "',FileID=" + ViewState["FileID"].ToString() + ",UpdatedBy=" + Session["AdminID"].ToString() + ",UpdatedOn=getdate() where VariantValueID=" + VariantValueID + " and Productid=" + PID + " ");

                                    }
                                    else
                                    {
                                        //insert
                                        CommonComponent.ExecuteCommonData("insert into [dbo].[tb_Replenishment] ([ProductID],[VariantValueID],[PO1],[qty1],[Shipping1],[Etadate1],[PO2],[qty2],[Shipping2],[Etadate2],[PO3],[qty3],[Shipping3],[Etadate3],[PO4],[qty4],[Shipping4],[Etadate4],[CreatedOn],[sku],[Name],[CreatedBy],[FileID]) values (" + PID + "," + VariantValueID + ",'" + po1 + "'," + qty1 + ",'" + txtShippingdate1.Text + "','" + txtEtadate1.Text + "','" + po2 + "'," + qty2 + ",'" + txtShippingdate2.Text + "','" + txtEtadate2.Text + "','" + po3 + "'," + qty3 + ",'" + txtShippingdate3.Text + "','" + txtEtadate3.Text + "','" + po4 + "'," + qty4 + ",'" + txtShippingdate4.Text + "','" + txtEtadate4.Text + "',getdate(),'" + sku + "','" + Name.ToString().Replace("'", "''") + "'," + Session["AdminID"].ToString() + "," + ViewState["FileID"].ToString() + ")");

                                    }

                                    counter++;
                                }



                           // }
                        }




                    }

                    ViewState["temptable"] = null;
                    CommonComponent.ExecuteCommonData("update tb_ReplenishementFileLog set LastUpdatedBy=0");
                    CommonComponent.ExecuteCommonData("update tb_ReplenishementFileLog set LastUpdatedBy=1 where FileID=" + ViewState["FileID"].ToString() + "");
                    prepage.Style.Add("dispaly", "none");
                    // lblsuccessmsg.Visible = true;
                    lblsuccessmsg.Text = "" + counter.ToString() + " Records Updated Successfully!";

                    btnUploadReplenishmentFile.Enabled = false;
                    ViewState["FileName"] = "";
                    ViewState["FileID"] = "";
                    btnverifyback.Attributes.Remove("onclick");

                    if (Request.QueryString["ID"] != null && Request.QueryString["ID"].ToString() != "")
                    {
                        btnverifyback.HRef = "/Admin/products/Product.aspx?StoreID=1&ID=" + Request.QueryString["ID"].ToString() + "&Mode=edit";
                    }
                    else
                    {
                        btnverifyback.HRef = "/REPLENISHMENTMANAGEMENT/dashboard.aspx";
                    }



                    // Response.Redirect("dashboard.aspx");
                    btnverifyback.InnerText = "DONE";
                    uploadtext.InnerText = "Click on Done button to return to the Dashboard page.";
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "btnuploadnext", "document.getElementById('ContentPlaceHolder1_btnuploadnext').click();", true);

                }
            }

        }



        protected void lnkbtnReset_Click(object sender, EventArgs e)
        {


            divFileName.Visible = false;
            divReset.Visible = false;

            divUpload.Visible = true;
            divImport.Visible = true;
            btnimportfile.Enabled = false;
        }


    }
}