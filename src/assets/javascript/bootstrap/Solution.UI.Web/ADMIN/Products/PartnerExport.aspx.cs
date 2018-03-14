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
using System.Text;
using System.Net;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class PartnerExport : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindstore();
                btnExport.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/export.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnUpload.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/upload.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnDownload.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/download.gif) no-repeat transparent; width: 80px; height: 23px; border:none;cursor:pointer;");
                bindstore();
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            else
            {
                ddlStore.DataSource = null;
                ddlStore.DataBind();
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ViewState["LastExportFileName"] = null;
            trupload.Visible = false;
            if (ddlStore.SelectedItem.Text.ToString().Trim() != "")
            {
                if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("wayfailr") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("lnt") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("bellacor") > -1)
                {
                    GenerateWayfailrProducts(ddlStore.SelectedItem.Text.ToString().Trim());
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore", "jAlert('Please Select Store','Message');", true);
                return;
            }
        }

        private void GenerateWayfailrProducts(string StoreName)
        {
            StringBuilder sb = new StringBuilder();
            DataSet dsorder = new DataSet();
            SecurityComponent objsec = new SecurityComponent();
            dsorder = CommonComponent.GetCommonDataSet("EXEC [usp_PartnerProductExport] '" + StoreName.ToString().Trim().ToLower() + "'");
            string column = "";
            string columnnom = "";
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Columns.Count; i++)
                {
                    if (dsorder.Tables[0].Columns.Count - 1 == i)
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString();
                        columnnom += "{" + i.ToString() + "}";
                    }
                    else
                    {
                        column += dsorder.Tables[0].Columns[i].ColumnName.ToString() + ",";
                        columnnom += "{" + i.ToString() + "},";
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                return;
            }

            sb.AppendLine(column);
            if (dsorder != null && dsorder.Tables.Count > 0 && dsorder.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < dsorder.Tables[0].Rows.Count; i++)
                {

                    object[] args = new object[dsorder.Tables[0].Columns.Count];
                    for (int c = 0; c < dsorder.Tables[0].Columns.Count; c++)
                    {
                        if (dsorder.Tables[0].Columns[c].ColumnName.ToString().ToLower().Trim() == "credit card number")
                        {
                            if (string.IsNullOrEmpty(dsorder.Tables[0].Rows[i][c].ToString().Trim()))
                            {
                                args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                            }
                            else
                            {
                                args[c] = _EscapeCsvField(SecurityComponent.Decrypt(dsorder.Tables[0].Rows[i][c].ToString().Trim()));
                            }

                        }
                        else
                        {
                            args[c] = _EscapeCsvField(dsorder.Tables[0].Rows[i][c].ToString().Trim());
                        }
                    }
                    sb.AppendLine(string.Format(columnnom, args));
                }
                if (!String.IsNullOrEmpty(sb.ToString()))
                {
                    string FullString = sb.ToString();
                    sb.Remove(0, sb.Length);
                    sb.AppendLine(FullString);

                    DateTime dt = DateTime.Now;
                    string StrStorename = "";
                    if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("wayfailr") > -1)
                    {
                        StrStorename = "wayfailr";
                    }
                    else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("lnt") > -1)
                    {
                        StrStorename = "lnt";
                    }
                    else if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("bellacor") > -1)
                    {
                        StrStorename = "bellacor";
                    }

                    String FileName = StrStorename.ToString() + "_" + dt.Month + "-" + dt.Day + "-" + dt.Year + "-" + dt.Hour + "-" + dt.Minute + "-" + dt.Second + ".csv";

                    if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                        Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                    ViewState["LastExportFileName"] = null;
                    String FilePath = Server.MapPath("~/Admin/Files/" + FileName);

                    ViewState["LastExportFileName"] = FileName.ToString();
                    WriteFile(sb.ToString(), FilePath);
                }
                if (ViewState["LastExportFileName"] != null)
                {
                    trupload.Visible = true;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Data Exported Successfully.','Message');", true);
                }
                else
                {
                    trupload.Visible = false;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('Record(s) not Found.','Message');", true);
                    return;
                }
            }
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

        private void UplaodFileOnftp(string filefullpathname, string ftphost, string ftpusername, string ftppassword, string ftpuploadpath)
        {

            string CompleteDPath = "ftp://" + ftphost + "/" + ftpuploadpath + "/";
            string FileName = filefullpathname;
            string ftpServerIP = ftphost;
            string ftpUserID = ftpusername;
            string ftpPassword = ftppassword;

            FileInfo fileInf = new FileInfo(FileName);

            string uri = "ftp://" + ftpServerIP + "/" + fileInf.Name;

            FtpWebRequest reqFTP;

            // Create FtpWebRequest object from the Uri provided

            //reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/httpdocs/client/images/BuyOrderFiles/" + fileInf.Name));
            if (ftpuploadpath == "")
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + fileInf.Name));
            }
            else
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpServerIP + "/" + ftpuploadpath + "/" + fileInf.Name));
            }


            // Provide the WebPermission Credintials

            reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);



            // By default KeepAlive is true, where the control connection is not closed

            // after a command is executed.

            reqFTP.KeepAlive = false;

            reqFTP.UsePassive = false;

            // Specify the command to be executed.

            reqFTP.Method = WebRequestMethods.Ftp.UploadFile;



            // Specify the data transfer type.

            reqFTP.UseBinary = true;



            // Notify the server about the size of the uploaded file

            reqFTP.ContentLength = fileInf.Length;



            // The buffer size is set to 2kb

            int buffLength = 2048;

            byte[] buff = new byte[buffLength];

            int contentLen;



            // Opens a file stream (System.IO.FileStream) to read the file to be uploaded

            FileStream fs = fileInf.OpenRead();



            try
            {

                // Stream to which the file to be upload is written

                Stream strm = reqFTP.GetRequestStream();



                // Read from the file stream 2kb at a time

                contentLen = fs.Read(buff, 0, buffLength);



                // Till Stream content ends

                while (contentLen != 0)
                {

                    // Write Content from the file stream to the FTP Upload Stream

                    strm.Write(buff, 0, contentLen);

                    contentLen = fs.Read(buff, 0, buffLength);

                }



                // Close the file stream and the Request Stream

                strm.Close();

                fs.Close();

            }

            catch (Exception ex)
            {

                //MessageBox.Show(ex.Message, "Upload Error");

            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            // Code for upload at ftp
            if (ViewState["LastExportFileName"] != null)
            {
                String FilePath = Server.MapPath("~/Admin/Files/" + ViewState["LastExportFileName"].ToString());
                if (File.Exists(FilePath) && ddlStore.SelectedValue.ToString() != "")
                {
                    string StrFTPFolderpath = "";
                    string ftphost = "";
                    string ftpusername = "";
                    string ftppassword = "";

                    ftphost = Convert.ToString(CommonComponent.GetScalarCommonData("SElect top 1 ISNULL(ConfigValue,'') as ConfigValue from tb_appconfig Where StoreID=" + ddlStore.SelectedValue.ToString() + " and ConfigName ='FTPHost'"));
                    ftpusername = Convert.ToString(CommonComponent.GetScalarCommonData("SElect top 1 ISNULL(ConfigValue,'') as ConfigValue from tb_appconfig Where StoreID=" + ddlStore.SelectedValue.ToString() + " and ConfigName ='FTPusername'"));
                    ftppassword = Convert.ToString(CommonComponent.GetScalarCommonData("SElect top 1 ISNULL(ConfigValue,'') as ConfigValue from tb_appconfig Where StoreID=" + ddlStore.SelectedValue.ToString() + " and ConfigName ='FTPpassword'"));
                    StrFTPFolderpath = Convert.ToString(CommonComponent.GetScalarCommonData("SElect top 1 ISNULL(ConfigValue,'') as ConfigValue from tb_appconfig Where StoreID=" + ddlStore.SelectedValue.ToString() + " and ConfigName ='FTPfoldername'"));

                    if (!string.IsNullOrEmpty(ftphost.ToString().Trim()) && !string.IsNullOrEmpty(ftpusername.ToString().Trim()) && !string.IsNullOrEmpty(ftppassword.ToString().Trim()))
                    {
                        UplaodFileOnftp(FilePath.ToString(), ftphost, ftpusername, ftppassword, StrFTPFolderpath.ToString());
                    }
                }
            }
        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            if (ViewState["LastExportFileName"] != null)
            {
                String FilePath = Server.MapPath("~/Admin/Files/" + ViewState["LastExportFileName"].ToString());
                if (!Directory.Exists(Server.MapPath("~/Admin/Files/")))
                    Directory.CreateDirectory(Server.MapPath("~/Admin/Files/"));

                if (File.Exists(FilePath))
                {
                    Response.Clear();
                    Response.ClearContent();
                    Response.ContentType = "text/csv";
                    Response.AppendHeader("Content-Disposition", "attachment; filename=" + ViewState["LastExportFileName"].ToString());
                    Response.TransmitFile(FilePath);
                    Response.End();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Validstore1", "jAlert('File not found.','Message');", true);
                return;
            }
        }

        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            trupload.Visible = false;
            ViewState["LastExportFileName"] = null;
        }
    }
}