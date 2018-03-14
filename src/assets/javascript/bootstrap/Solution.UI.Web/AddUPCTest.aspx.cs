using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.Net;
using System.IO;
using Solution.Data;


namespace Solution.UI.Web
{
    public partial class AddUPCTest : System.Web.UI.Page
    {
        string ftpNewEggServerIP;
        string ftpNewEggUserID;
        string ftpNewEggPassword;
        protected void Page_Load(object sender, EventArgs e)
        {
            SQLAccess objdb = new SQLAccess();
            ftpNewEggServerIP = Convert.ToString(objdb.ExecuteScalarQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='NewEggFTPServer' And Storeid=9")); //"trade.marketplace.buy.com";
            ftpNewEggUserID = Convert.ToString(objdb.ExecuteScalarQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='NewEggFTPusername' And Storeid=9"));//"chris@acedepot.com";
            ftpNewEggPassword = Convert.ToString(objdb.ExecuteScalarQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='NewEggFTPpassword' And Storeid=9"));//"acedepot1";
        }

        public string[] GetFileListNewEgg()
        {
            string[] downloadFiles;
            StringBuilder result = new StringBuilder();
            FtpWebRequest reqFTP;
            try
            {
                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpNewEggServerIP + "/Outbound/OrderList/"));
                reqFTP.UseBinary = false;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential(ftpNewEggUserID, ftpNewEggPassword);
                reqFTP.Method = WebRequestMethods.Ftp.ListDirectory;
                WebResponse response = reqFTP.GetResponse();
                StreamReader reader = new StreamReader(response.GetResponseStream());
                //MessageBox.Show(reader.ReadToEnd());
                string line = reader.ReadLine();
                while (line != null)
                {
                    if (line.ToString().ToLower().IndexOf(".xml") > -1 || line.ToString().ToLower().IndexOf(".xls") > -1)
                    {
                        result.Append(line);
                        result.Append("\n");
                    }
                    line = reader.ReadLine();
                }
                result.Remove(result.ToString().LastIndexOf('\n'), 1);
                reader.Close();
                response.Close();
                //MessageBox.Show(response.StatusDescription);
                return result.ToString().Split('\n');
            }
            catch (Exception ex)
            {
                //System.Windows.Forms.MessageBox.Show(ex.Message);
                downloadFiles = null;
                return downloadFiles;
            }
        }

        protected void btnImport_Click(object sender, EventArgs e)
        {
            SQLAccess objdb = new SQLAccess();
            ftpNewEggServerIP = "";// Convert.ToString(objdb.ExecuteScalarQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='NewEggFTPServer' And Storeid=9")); //"trade.marketplace.buy.com";
            ftpNewEggUserID = "";// Convert.ToString(objdb.ExecuteScalarQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='NewEggFTPusername' And Storeid=9"));//"chris@acedepot.com";
            ftpNewEggPassword = "";// Convert.ToString(objdb.ExecuteScalarQuery("SELECT configValue FROM tb_ecomm_AppConfig WHERE Configname='NewEggFTPpassword' And Storeid=9"));//"acedepot1";

            string[] filenamesftp = GetFileListNewEgg();
            string Executablepath = @"E:\NewEggFiles\";
            string Existpath = Executablepath + "NewEggorderFiles";
            string Existpath1 = Executablepath + "NewEggTemp";

            try
            {
                if (!Directory.Exists(Existpath))
                {
                    Directory.CreateDirectory(Existpath);
                }
                if (!Directory.Exists(Existpath1))
                {
                    Directory.CreateDirectory(Existpath1);
                }
                else
                {
                    //string[] filenamesTemp = Directory.GetFiles(Existpath1);
                    //foreach (string filename in filenamesTemp)
                    //{
                    //    File.Delete(filename.ToString());
                    //}

                }
            }
            catch (Exception ex)
            {

            }

            foreach (string filename in filenamesftp)
            {

                DownloadNewegg(Existpath1, filename.ToString());
                try
                {

                    File.Copy(Existpath1 + "/" + filename.ToString(), Existpath + "/" + filename.ToString(), true);
                }
                catch (Exception ex)
                {
                    //WriteLog("NewEgg Order file Copy :" + ex.Message.ToString() + ex.StackTrace.ToString());
                }

            }
        }

        private void DownloadNewegg(string filePath, string fileName)
        {
            FtpWebRequest reqFTP;
            try
            {
                //filePath = <<The full path where the file is to be created.>>, 
                //fileName = <<Name of the file to be created(Need not be the name of the file on FTP server).>>
                FileStream outputStream = new FileStream(filePath + "\\" + fileName, FileMode.Create);

                reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri("ftp://" + ftpNewEggServerIP + "/Outbound/OrderList/" + fileName));
                reqFTP.Method = WebRequestMethods.Ftp.DownloadFile;
                reqFTP.UseBinary = false;
                reqFTP.UsePassive = false;
                reqFTP.Credentials = new NetworkCredential(ftpNewEggUserID, ftpNewEggPassword);
                FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
                Stream ftpStream = response.GetResponseStream();
                long cl = response.ContentLength;
                int bufferSize = 2048;
                int readCount;
                byte[] buffer = new byte[bufferSize];

                readCount = ftpStream.Read(buffer, 0, bufferSize);
                while (readCount > 0)
                {
                    outputStream.Write(buffer, 0, readCount);
                    readCount = ftpStream.Read(buffer, 0, bufferSize);
                }

                ftpStream.Close();
                outputStream.Close();
                response.Close();
            }
            catch (Exception ex)
            {
                // WriteLog("Buy Order file Download: " + ex.Message.ToString() + ex.StackTrace.ToString());

                //MessageBox.Show(ex.Message);
            }
        }
    }
}