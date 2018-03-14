using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Data;
using System.Net;
using System.IO;
using Ionic.Zip;
using SqlDataReader = System.Data.SqlClient.SqlDataReader;
using System.Text;
using System.Collections;
using System.Net.Mail;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class GenerateYahooCSV : BasePage
    {
        #region Local Variables

        StringBuilder sbCSV = new StringBuilder();
        String catCSSClass = String.Empty;
        String storeID = String.Empty;
        DateTime dtStartDate = DateTime.MinValue;
        DateTime dtEndDate = DateTime.MinValue;
        String yStoreURL = String.Empty;
        DataSet dsStore = new DataSet();
        string strServer = string.Empty;
        string strImagesPath = string.Empty;
        DataSet Ds = new DataSet();
        ProductComponent objProduct = new ProductComponent();

        string strCopyImagePath = string.Empty;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (!IsPostBack)
            {
                btnGenerate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/generate-yahoo-csv.png";
                btnGenerateImages.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/generate-images.png";
                txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(DateTime.Now));
                BindYahooStore();
            }
            else
            {
                String[] formkeys = Request.Form.AllKeys;
                foreach (String s in formkeys)
                {
                    //For Delete Product from Cart and Bind Cart again  
                    if (s.Contains("btnDownload-"))
                    {
                        try
                        {
                            String p = Request.Form.Get(s);
                            downloadfile(p);
                        }
                        catch
                        { }
                    }
                }
            }
        }

        /// <summary>
        /// Bind All Yahoo Stores in Drop down
        /// </summary>
        private void BindYahooStore()
        {
            DataSet dsStoreList = StoreComponent.GetYahooStoreList();
            if (dsStoreList != null && dsStoreList.Tables.Count > 0 && dsStoreList.Tables[0].Rows.Count > 0)
            {
                ddlStore.DataSource = dsStoreList.Tables[0];
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
            }
            else
            {
                ddlStore.DataSource = null;
            }
            ddlStore.DataBind();
        }

        /// <summary>
        /// Generate CSV file button click event
        /// </summary>
        /// <param name="sender">Sender Object</param>
        /// <param name="e">Event Argument</param>
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            lblMsg.Text = "";
            if (txtStartDate.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please Select Start Date...');", true);
                return;
            }

            if (txtEndDate.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please Select End Date...');", true);
                return;
            }


            SQLAccess dbAccess = new SQLAccess();

            if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("amazon") > -1)
            {
                // CustomizableAmazonCSV();
            }
            else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("yahoo") > -1)
            {
                bool flag = true;
                foreach (ListItem li in lbFields.Items)
                {
                    if (li.Selected)
                    {
                        flag = false;
                    }
                }
                if (flag)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please Select Fields');", true);
                    return;
                }
                CustomizableYahooCSV();
            }
        }


        #region Functions for CSV Generates

        /// <summary>
        /// Generates Customizable the Yahoo CSV
        /// </summary>
        private void CustomizableYahooCSV()
        {
            try
            {
                if (ddlCriteria.SelectedValue != "1")
                {
                    if (txtStartDate.Text.ToString().Trim() != "" && txtEndDate.Text.ToString().Trim() != null)
                    {
                        if (Convert.ToDateTime(txtEndDate.Text) >= Convert.ToDateTime(txtStartDate.Text))
                            SetValues();
                        else
                        {
                            lblMsg.Text = "Start Date should be smaller than End Date.";
                            return;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Please select Date.";
                        return;
                    }
                }
                else
                    SetValues();

                Int32 cnt = 0;
                String strfields = "", strFieldsNames = "";
                foreach (ListItem li in lbFields.Items)
                {
                    if (li.Selected)
                    {
                        strfields += "{" + cnt + "},";
                        strFieldsNames += li.Text + ",";
                        cnt++;
                    }

                }
                if (strfields.Length > 1)
                {
                    strfields = strfields.Substring(0, strfields.Length - 1);
                    strFieldsNames = strFieldsNames.Substring(0, strFieldsNames.Length - 1);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        object[] args = new object[cnt];

                        Int32 j = 0;

                        foreach (ListItem li in lbFields.Items)
                        {
                            if (li.Selected)
                            {
                                if (li.Value.Trim().ToLower() == "abstract")
                                {
                                    args[j] = "";
                                }
                                else
                                {
                                    //Ds.Tables[0].Rows[i][li.Value] = System.Text.RegularExpressions.Regex.Replace(Ds.Tables[0].Rows[i][li.Value].ToString().Trim(), @"<[^>]*>", String.Empty);
                                    //Ds.Tables[0].AcceptChanges();
                                    string strResponse = Ds.Tables[0].Rows[i][li.Value].ToString();
                                    strResponse = strResponse.Replace("\t", "");
                                    strResponse = strResponse.Replace("\" />", "\"/>");
                                    strResponse = strResponse.Replace("\" >", "\">");
                                    strResponse = strResponse.Replace("©", "&copy;");
                                    strResponse = strResponse.Replace("®", "&reg;");

                                    args[j] = _EscapeCsvField(strResponse.ToString().Replace("\r\n", ""));
                                    if (li.Value.Trim().ToLower() == "options")
                                    {
                                        args[j] = args[j].ToString().Replace("~", Environment.NewLine + Environment.NewLine);
                                    }
                                }

                                j++;
                            }
                        }

                        sb.AppendLine(string.Format(strfields, args));
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "NoData", "jAlert('No Record(s) Found.','Message')", true);
                    return;
                }
                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(strFieldsNames);
                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    String FileName = "YahooProducts_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";
                    String CSVPath = Convert.ToString(AppLogic.AppConfigs("YahooProductCSV"));
                    if (!Directory.Exists(Server.MapPath(CSVPath)))
                        Directory.CreateDirectory(Server.MapPath(CSVPath));

                    Response.Clear();
                    Response.ClearContent();
                    String FilePath = Server.MapPath(CSVPath + FileName);
                    WriteFile(sb.ToString(), FilePath);
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
                else
                {
                    lblMsg.Text = "No Record(s) Found.";
                }
            }
            catch
            { }
        }

        /// <summary>
        /// Generate Customizable the Amazon CSV
        /// </summary>
        private void CustomizableAmazonCSV()
        {
            try
            {
                if (ddlCriteria.SelectedValue != "1")
                {
                    if (txtStartDate.Text.ToString().Trim() != "" && txtEndDate.Text.ToString().Trim() != "")
                    {
                        if (Convert.ToDateTime(txtEndDate.Text) >= Convert.ToDateTime(txtStartDate.Text))
                            SetValues();
                        else
                        {
                            lblMsg.Text = "Start Date should be smaller than End Date.";
                            return;
                        }
                    }
                    else
                    {
                        lblMsg.Text = "Please select Date.";
                        return;
                    }
                }
                else
                    SetValues();

                Int32 cnt = 0;
                String strfields = "", strFieldsNames = "";
                if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                {
                    for (int iCol = 0; iCol < Ds.Tables[0].Columns.Count; iCol++)
                    {
                        strfields += "{" + cnt + "}\t";
                        strFieldsNames += Ds.Tables[0].Columns[iCol].ToString() + "\t";
                        cnt++;
                    }
                }
                if (strfields.Length > 1)
                {
                    strfields = strfields.Substring(0, strfields.Length - 1);
                    strFieldsNames = strFieldsNames.Substring(0, strFieldsNames.Length - 1);
                }

                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                if (Ds != null && Ds.Tables[0].Rows.Count > 0)
                {
                    object[] args = new object[cnt];
                    for (int i = 0; i < Ds.Tables[0].Rows.Count; i++)
                    {

                        Int32 j = 0;

                        for (int iCol = 0; iCol < Ds.Tables[0].Columns.Count; iCol++)
                        {
                            if (!string.IsNullOrEmpty(Ds.Tables[0].Rows[i][Ds.Tables[0].Columns[iCol].ToString()].ToString()))
                            {
                                string strResponse = Ds.Tables[0].Rows[i][Ds.Tables[0].Columns[iCol].ToString()].ToString();
                                strResponse = strResponse.Replace("\t", "");
                                strResponse = strResponse.Replace("\" />", "\"/>");
                                strResponse = strResponse.Replace("\" >", "\">");
                                strResponse = strResponse.Replace("©", "&copy;");
                                strResponse = strResponse.Replace("®", "&reg;");
                                args[iCol] = strResponse.ToString().Replace("\r\n", "");
                            }
                            else
                            {
                                args[iCol] = "";
                            }
                        }
                        j++;
                        sb.AppendLine(string.Format(strfields, args));
                    }

                }

                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    //if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("canada") > -1)
                    //{
                    sb.AppendLine("TemplateType=ConsumerElectronics\tVersion=1.7/1.2.9\tThis row for Amazon.com use only.  Do not modify or delete.");
                    //}
                    sb.AppendLine(strFieldsNames);
                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    String FileName = "AmazonProducts_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".txt";
                    String CSVPath = Convert.ToString(AppLogic.AppConfigs("YahooProductCSV"));
                    if (!Directory.Exists(Server.MapPath(CSVPath)))
                        Directory.CreateDirectory(Server.MapPath(CSVPath));

                    //try
                    //{
                    Response.Clear();
                    Response.ClearContent();
                    String FilePath = Server.MapPath(CSVPath + FileName);
                    WriteFile(sb.ToString(), FilePath);
                    Response.ContentType = "text/plain";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + FileName);
                    Response.TransmitFile(FilePath);
                    Response.End();
                    //}
                    //catch { }
                }
            }
            catch
            { }
        }
        #endregion


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
                return "\"" + sFieldValueToEscape.Replace("\"", "\"\"") + "\"";//sFieldValueToEscape;
            }
        }

        /// <summary>
        /// Write data in the file
        /// </summary>
        /// <param name="Text">string Text</param>
        /// <param name="FileName">string FileName</param>
        private void WriteFile(String Text, string FileName)
        {
            StreamWriter writer = null;
            try
            {
                FileInfo info = new FileInfo(FileName);
                writer = info.AppendText();
                writer.Write(Text);
            }
            catch { }
            finally
            {
                if (writer != null)
                    writer.Close();
            }
        }

        /// <summary>
        /// Sets the Values
        /// </summary>
        private void SetValues()
        {
            AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            if (ddlCriteria.SelectedValue != "1")
                if (txtStartDate.Text.ToString().Trim() != "" && txtEndDate.Text.ToString().Trim() != "")
                {
                    if (Convert.ToDateTime(txtEndDate.Text) < Convert.ToDateTime(txtStartDate.Text))
                    {
                        lblMsg.Text = "Start Date should be smaller than End Date. ";
                        return;
                    }
                }
                else
                {
                    lblMsg.Text = "Please select Date.";
                    return;
                }

            if (txtStartDate.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please Select Start Date...');", true);
                return;
            }
            else
            {
                dtStartDate = Convert.ToDateTime(txtStartDate.Text.ToString().Trim());
            }

            if (txtEndDate.Text.ToString().Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please Select End Date...');", true);
                return;
            }
            else
            {
                dtEndDate = Convert.ToDateTime(txtEndDate.Text.ToString().Trim());
            }


            strServer = AppLogic.AppConfigs("Live_Server").TrimEnd('/');
            strImagesPath = AppLogic.AppConfigs("ImagePathProduct").TrimEnd('/');

            if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("yahoo") > -1)
            {
                Ds = objProduct.GetProductForYahooStore(Convert.ToInt32(ddlStore.SelectedValue.ToString()), dtStartDate, dtEndDate, dtStartDate, dtEndDate, ddlCriteria.SelectedItem.Text.ToString());
            }
            else if (ddlStore.SelectedItem.Text.ToString().ToLower().IndexOf("amazon") > -1)
            {
                Ds = null;// objProduct.GetProductForAmazonStore(Convert.ToInt32(ddlStore.SelectedValue.ToString()), dtStartDate, dtEndDate, dtStartDate, dtEndDate, ddlCriteria.SelectedItem.Text.ToString());
            }
            strCopyImagePath = AppLogic.AppConfigs("ImagePathProduct").ToString();
        }

        /// <summary>
        ///  Generate Images Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnGenerateImages_Click(object sender, EventArgs e)
        {
            if (ddlCriteria.SelectedValue != "1")
                if (txtStartDate.Text.ToString().Trim() != null && txtEndDate.Text.ToString().Trim() != "")
                {
                    if (Convert.ToDateTime(txtEndDate.Text) >= Convert.ToDateTime(txtStartDate.Text))
                        SetValues();
                    else
                    {
                        lblMsg.Text = "Start Date should be smaller than End Date.";
                        return;
                    }
                }
                else
                {
                    lblMsg.Text = "Please select Date.";
                    return;
                }

            SetValues();

            if (Ds != null && Ds.Tables[0].Rows.Count > 0)
            {
                AppLogic.ApplicationStart();
                String strMainImagePath = string.Empty;
                String strMore1ImagePath = string.Empty;
                String strMore2ImagePath = string.Empty;
                String strMore3ImagePath = string.Empty;
                string imagepath = Convert.ToString(AppLogic.AppConfigs("YahooZipImages")) + DateTime.Now.Ticks.ToString() + "/";

                string ImageURL = string.Empty;
                string ImagePath = string.Empty;

                if (!System.IO.Directory.Exists(Server.MapPath(imagepath + "Main-" + DateTime.Now.ToShortDateString().Replace("/", "-"))))
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath(imagepath + "Main-" + DateTime.Now.ToShortDateString().Replace("/", "-")));
                }

                strMainImagePath = Server.MapPath(imagepath + "Main-" + DateTime.Now.ToShortDateString().Replace("/", "-"));

                foreach (DataRow dr in Ds.Tables[0].Rows)
                {
                    ImageURL = dr["ImageURL"].ToString();
                    ImagePath = Server.MapPath(strCopyImagePath + "Large/" + dr["Imagename"].ToString());

                    try
                    {
                        try
                        {
                            FileInfo fl = new FileInfo(ImagePath.ToString());
                            string filename = fl.Name.ToString().Replace(fl.Extension.ToString(), "");
                            if (File.Exists(Server.MapPath(strCopyImagePath + "Large/" + filename.ToString().Replace(".jpg.jpg", ".jpg") + ".jpg")))
                            {
                                File.Copy(Server.MapPath(strCopyImagePath + "Large/" + filename.ToString().Replace(".jpg.jpg", ".jpg") + ".jpg"), strMainImagePath + "/" + filename.ToString() + ".jpg");
                            }

                            //For More1 Large Images
                            if (File.Exists(Server.MapPath(strCopyImagePath + "Large/" + filename.ToString().Replace(".jpg.jpg", ".jpg") + "_1.jpg")))
                            {
                                File.Copy(Server.MapPath(strCopyImagePath + "Large/" + filename.ToString().Replace(".jpg.jpg", ".jpg") + "_1.jpg"), strMore1ImagePath + "/" + filename.ToString() + "_1.jpg");
                            }
                            //For More2 Large Images
                            if (File.Exists(Server.MapPath(strCopyImagePath + "Large/" + filename.ToString().Replace(".jpg.jpg", ".jpg") + "_2.jpg")))
                            {
                                File.Copy(Server.MapPath(strCopyImagePath + "Large/" + filename.ToString().Replace(".jpg.jpg", ".jpg") + "_2.jpg"), strMore2ImagePath + "/" + filename.ToString() + "_2.jpg");
                            }
                            //For More3 Large Images
                            if (File.Exists(Server.MapPath(strCopyImagePath + "Large/" + filename.ToString().Replace(".jpg.jpg", ".jpg") + "_3.jpg")))
                            {
                                File.Copy(Server.MapPath(strCopyImagePath + "Large/" + filename.ToString().Replace(".jpg.jpg", ".jpg") + "_3.jpg"), strMore3ImagePath + "/" + filename.ToString() + "_3.jpg");
                            }
                        }
                        catch
                        {
                        }
                    }
                    catch (Exception ex) { lblMsg.Text = ex.Message; }
                }
                try
                {
                    savezipfiles(strMainImagePath, strMore1ImagePath, strMore2ImagePath, strMore3ImagePath);
                    lblMsg1.Text = "Please click on Following links to Download images.<br>  Images saved in <a style='color:#000;text-decoration:underline' href='" + AppLogic.AppConfigs("LIVE_SERVER") + imagepath.ToString() + "' target='_blank'>" + imagepath + "</a> folder. ";
                    lblMsg1.ForeColor = System.Drawing.Color.Black;
                }
                catch (Exception ex) { lblMsg.Text = ex.Message; }

            }
            else
            {
                lblMsg.Text = "No Record(s) Found.";
            }
        }

        /// <summary>
        /// Save Zip Files to Folder
        /// </summary>
        /// <param name="strMainImagePath">string strMainImagePath</param>
        /// <param name="strMore1ImagePath">string strMore1ImagePath</param>
        /// <param name="strMore2ImagePath">string strMore2ImagePath</param>
        /// <param name="strMore3ImagePath">string strMore3ImagePath</param>
        private void savezipfiles(string strMainImagePath, string strMore1ImagePath, string strMore2ImagePath, string strMore3ImagePath)
        {
            DirectoryInfo d = new DirectoryInfo(strMainImagePath);
            Decimal MaxSize = 1024 * 1024 * 10;
            string Links = "";

            ZipFile z = new ZipFile();
            Decimal TotalSize = 0;
            int cnt = 0;
            DateTime dt = DateTime.Now;
            String strRandom = Convert.ToString(dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second);
            foreach (FileInfo f in d.GetFiles())
            {
                z.AddFile(f.FullName, "");
                TotalSize += f.Length;
                if (TotalSize > MaxSize)
                {
                    string zipfolder = strMainImagePath + "-Zip";
                    cnt++;
                    if (!Directory.Exists(zipfolder))
                        Directory.CreateDirectory(zipfolder);
                    string strzip = zipfolder + "/Main-" + cnt + "_" + strRandom + ".zip";
                    z.Save(strzip);

                    TotalSize = 0;
                    Links = Links + "<input style='display:none;' type='submit' runat='server' value='" + strzip + "' name='btnDownload-main-" + cnt + "'  id='btnDownload-main-" + cnt + "' onclick='submit' /> <a href='#' onclick=\"document.getElementById('btnDownload-main-" + cnt + "').click();\">Main-" + cnt + "_" + strRandom + ".zip</a> <br/>";
                    z = new ZipFile();
                }
            }
            if (TotalSize > 0 && z.Entries.Count > 0)
            {
                string zipfolder = strMainImagePath + "-Zip";
                if (!Directory.Exists(zipfolder))
                    Directory.CreateDirectory(zipfolder);
                cnt++;
                string strzip = zipfolder + "/Main-" + cnt + "_" + strRandom + ".zip";
                z.Save(strzip);

                TotalSize = 0;
                Links = Links + "<input style='display:none;' onclick='submit' type='submit' runat='server' name='btnDownload-main-" + cnt + "' value='" + strzip + "' id='btnDownload-main-" + cnt + "' /> <a href='#' onclick=\"document.getElementById('btnDownload-main-" + cnt + "').click();\">Main-" + cnt + "_" + strRandom + ".zip</a> <br/>";

            }

            lblLinks.Text = Links;
        }

        /// <summary>
        /// Download File from file path
        /// </summary>
        /// <param name="filepath">string filepath</param>
        private void downloadfile(string filepath)
        {
            try
            {
                FileInfo file = new FileInfo(filepath);
                if (file.Exists)
                {
                    Response.ClearContent();
                    Response.ContentType = ReturnExtension(file.Extension.ToLower()); ;
                    Response.AddHeader("content-disposition", "attachment; filename=" + file.Name);

                    FileStream sourceFile = new FileStream(file.FullName, FileMode.Open);
                    long FileSize;
                    FileSize = sourceFile.Length;
                    byte[] getContent = new byte[(int)FileSize];
                    sourceFile.Read(getContent, 0, (int)sourceFile.Length);
                    sourceFile.Close();
                    Response.BinaryWrite(getContent);

                }
            }

            catch (Exception ex) { }
        }

        /// <summary>
        /// Return String Extension
        /// </summary>
        /// <param name="fileExtension">string fileExtension</param>
        /// <returns>Returns the Extension</returns>
        private string ReturnExtension(string fileExtension)
        {
            switch (fileExtension)
            {
                case ".htm":
                case ".html":
                case ".log":
                    return "text/HTML";
                case ".txt":
                    return "text/plain";
                case ".doc":
                    return "application/ms-word";
                case ".tiff":
                case ".tif":
                    return "image/tiff";
                case ".asf":
                    return "video/x-ms-asf";
                case ".avi":
                    return "video/avi";
                case ".zip":
                    return "application/zip";
                case ".xls":
                case ".csv":
                    return "application/vnd.ms-excel";
                case ".gif":
                    return "image/gif";
                case ".jpg":
                case "jpeg":
                    return "image/jpeg";
                case ".bmp":
                    return "image/bmp";
                case ".wav":
                    return "audio/wav";
                case ".mp3":
                    return "audio/mpeg3";
                case ".mpg":
                case "mpeg":
                    return "video/mpeg";
                case ".rtf":
                    return "application/rtf";
                case ".asp":
                    return "text/asp";
                case ".pdf":
                    return "application/pdf";
                case ".fdf":
                    return "application/vnd.fdf";
                case ".ppt":
                    return "application/mspowerpoint";
                case ".dwg":
                    return "image/vnd.dwg";
                case ".msg":
                    return "application/msoutlook";
                case ".xml":
                case ".sdxl":
                    return "application/xml";
                case ".xdp":
                    return "application/vnd.adobe.xdp+xml";
                default:
                    return "application/octet-stream";
            }
        }

        /// <summary>
        ///  Download Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDownload_click(object sender, EventArgs e)
        {
            String downloadIndex = Request.Form["selDownload"].ToString();
            try
            {
                Hashtable htFiles = new Hashtable();
                string[] listoffiles = new string[100];
                int Count = 0;
                string strAllUniqueKeys = "";//hfAllUniqueTypes.Value.ToLower();
                foreach (String Key in Request.Form.Keys)
                {
                    String strKey = "," + Key.ToLower() + ",";
                    if (strAllUniqueKeys.Contains(strKey))
                    {
                        string[] files = Request.Form.GetValues(Key);
                        files.CopyTo(listoffiles, Count);
                        Count += files.Length;
                    }
                }
                bool MissingFiles = false;
                foreach (String strPath in listoffiles)
                {
                    if (string.IsNullOrEmpty(strPath))
                        break;

                    string[] strPortions = strPath.Split("*".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);//name,sku,type
                    if (strPortions.Length < 3)
                    { MissingFiles = true; continue; }

                    if (!System.IO.Directory.Exists(Server.MapPath("/EmbrFiles/" + strPortions[1] + "/" + strPortions[2])))
                    { MissingFiles = true; continue; }

                    if (!htFiles.ContainsKey(strPortions[1]))
                        htFiles.Add(strPortions[1], strPortions[0]);

                    if (!htFiles.ContainsKey(strPortions[1] + "/" + strPortions[2]))
                        htFiles.Add(strPortions[1] + "/" + strPortions[2], strPortions[0] + "/" + strPortions[2]);
                }

                string fileName = String.Format("{0}-{1}.zip", "ObjCustomer.Name", DateTime.Now.ToString("yyyy-MM-dd-HHmmss"));
                ZipFile z = GetZipFile(htFiles, MissingFiles);
                z.Save(Server.MapPath("/EmbrFiles/" + fileName));

                if (downloadIndex == "1")//Send Email
                {
                    String strMailBody = ""; ;

                    Attachment zipAttachment = new Attachment(Server.MapPath("/EmbrFiles/" + fileName));
                    Page.RegisterStartupScript("alertSucces", "<script type='text/javascript'>alert('An Email was successfully sent to your Email address with requested files. Thank you.');</script>");
                }
                else //download file
                {
                    string path = Server.MapPath("/EmbrFiles/" + fileName);
                    SendFile(path);
                }
            }
            catch
            {
                if (downloadIndex == "1")//Send Email
                    Page.RegisterStartupScript("ErrorScript", "<script type='text/javascript'>alert('Error sending email. Please retry..');</script>");
                else
                    Page.RegisterStartupScript("ErrorScript", "<script type='text/javascript'>alert('Error downloading the file. Please retry..');</script>");
            }
        }

        /// <summary>
        /// Gets the Zip File
        /// </summary>
        /// <param name="htFiles">Hashtable htFiles</param>
        /// <param name="MissingFiles">bool MissingFiles</param>
        /// <returns>Returns the ZipFile</returns>
        private ZipFile GetZipFile(Hashtable htFiles, bool MissingFiles)
        {
            ZipFile z = new ZipFile();
            foreach (object strSourcePath in htFiles.Keys)
            {
                string strDestPath = htFiles[strSourcePath].ToString();
                z.AddDirectoryByName("files/" + strDestPath);

                String[] filePaths = System.IO.Directory.GetFiles(Server.MapPath("/EmbrFiles/" + strSourcePath));
                foreach (String filePath in filePaths)
                {
                    if (filePath.EndsWith("Thumbs.db")) continue;
                    z.AddFile(filePath, "files/" + strDestPath);
                }
            }

            if (MissingFiles && System.IO.File.Exists(Server.MapPath("/EmbrFiles/Missing.txt")))
                z.AddFile(Server.MapPath("/EmbrFiles/Missing.txt"), "");
            return z;
        }

        /// <summary>
        /// Sends the File
        /// </summary>
        /// <param name="filePath">string filePath</param>
        private void SendFile(string filePath)
        {
            using (System.IO.MemoryStream ms = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(filePath)))
            {
                long dataLengthToRead = ms.Length;
                int blockSize = dataLengthToRead >= 5000 ? 5000 : (int)dataLengthToRead;
                byte[] buffer = new byte[dataLengthToRead];

                Response.Clear();

                // Clear the content of the response
                Response.ClearContent();
                Response.ClearHeaders();

                // Buffer response so that page is sent
                // after processing is complete.
                Response.BufferOutput = true;

                // Add the file name and attachment,
                // which will force the open/cancel/save dialog to show, to the header
                string fileName = System.IO.Path.GetFileName(filePath);
                Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);

                // bypass the Open/Save/Cancel dialog
                //Response.AddHeader("Content-Disposition", "inline; filename=" + doc.FileName);

                // Add the file size into the response header
                Response.AddHeader("Content-Length", ms.Length.ToString());

                // Set the ContentType
                Response.ContentType = "application/octet-stream";

                // Write the document into the response
                while (dataLengthToRead > 0 && Response.IsClientConnected)
                {
                    Int32 lengthRead = ms.Read(buffer, 0, blockSize);
                    Response.OutputStream.Write(buffer, 0, lengthRead);
                    Response.Flush();
                    dataLengthToRead = dataLengthToRead - lengthRead;
                }

                Response.Flush();
                Response.Close();

                try
                {
                    System.IO.File.Delete(filePath);
                }
                catch { }

                // End the response
                Response.End();
            }
        }

    }
}