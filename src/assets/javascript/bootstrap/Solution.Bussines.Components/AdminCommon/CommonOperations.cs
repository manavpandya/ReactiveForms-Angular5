using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Drawing.Imaging;
using System.Web;
using System.IO;
using System.Net.Mail;
using Solution.Bussines.Entities;
using System.Data;


namespace Solution.Bussines.Components.AdminCommon
{
    public class CommonOperations
    {
        /// <summary>
        /// Create Folder
        /// </summary>
        /// <param name="folderName">folderName</param>
        public static void CreateFolder(string folderName)
        {
            if (System.IO.Directory.Exists(folderName) == false)
            {
                System.IO.Directory.CreateDirectory(folderName);
            }
        }

        /// <summary>
        /// Adjust Extra Space In Image
        /// </summary>
        /// <param name="extBMP">extBMP</param>
        /// <param name="DestFileName">DestFileName</param>
        /// <param name="DefWidth">DefWidth</param>
        /// <param name="DefHeight">DefHeight</param>
        public static void AdjustExtraSpaceInImage(Bitmap extBMP, string DestFileName, int DefWidth, int DefHeight)
        {
            Encoder Enc = Encoder.SaveFlag;
            EncoderParameters EncParms = new EncoderParameters(1);
            EncoderParameter EncParm;
            ImageCodecInfo CodecInfo = GetEncoderInfo("image/jpeg");
            EncParm = new EncoderParameter(Encoder.Quality, (long)300);
            EncParms.Param[0] = new EncoderParameter(Encoder.Quality, (long)300);
            if (extBMP != null && extBMP.Width < (DefWidth + 4) && extBMP.Height < (DefHeight + 4))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, startX, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null && extBMP.Width < (DefWidth + 4))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                g.DrawImage(extBMP, startX, 0);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null && extBMP.Height < (DefHeight + 4))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, 0, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null)
            {
                extBMP.Save(DestFileName, CodecInfo, EncParms);
                extBMP.Dispose();
                SaveOnContentServer(DestFileName);
            }
        }

        /// <summary>
        /// Get Encoder Info
        /// </summary>
        /// <param name="resizeMimeType">resizeMimeType</param>
        /// <returns>ImageCodecInfo</returns>
        private static ImageCodecInfo GetEncoderInfo(string resizeMimeType)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == resizeMimeType)
                    return codecs[i];
            return null;
        }

        /// <summary>
        /// Save On Content Server
        /// </summary>
        /// <param name="strCurrentSavePath">string strCurrentSavePath</param>
        public static void SaveOnContentServer(string strCurrentSavePath)
        {
            try
            {
                string strContentPhysicalPath = AppLogic.AppConfigs("ContentServerPhysicalPath");
                if (string.IsNullOrEmpty(strContentPhysicalPath)
                    || string.IsNullOrEmpty(strCurrentSavePath))
                    return;

                strContentPhysicalPath = strContentPhysicalPath.Trim('/') + "/" + strCurrentSavePath.ToLower().Replace(HttpContext.Current.Server.MapPath("/").ToLower(), "").Trim('/');
                strContentPhysicalPath = strContentPhysicalPath.Replace("\\", "/");
                string strDestDirectoryPath = strContentPhysicalPath.Substring(0, strContentPhysicalPath.LastIndexOf('/'));
                if (!System.IO.Directory.Exists(strDestDirectoryPath))
                    System.IO.Directory.CreateDirectory(strDestDirectoryPath);
                System.IO.File.Copy(strCurrentSavePath, strContentPhysicalPath, true);
            }
            catch { }
        }

        /// <summary>
        /// Delete File From Content Server
        /// </summary>
        /// <param name="strCurrentFilePath">string strCurrentFilePath</param>
        public static void DeleteFileFromContentServer(string strCurrentFilePath)
        {
            try
            {
                string strContentPhysicalPath = AppLogic.AppConfigs("ContentServerPhysicalPath");
                if (string.IsNullOrEmpty(strContentPhysicalPath)
                    || string.IsNullOrEmpty(strCurrentFilePath))
                    return;

                strContentPhysicalPath = strContentPhysicalPath.Trim('/') + "/" + strCurrentFilePath.ToLower().Replace(HttpContext.Current.Server.MapPath("/").ToLower(), "").Trim('/');
                strContentPhysicalPath = strContentPhysicalPath.Replace("\\", "/");
                System.IO.File.Delete(strContentPhysicalPath);
            }
            catch { }
        }

        /// <summary>
        /// Remove Special Character
        /// </summary>
        /// <param name="charr">charr</param>
        /// <returns>string</returns>
        public static string RemoveSpecialCharacter(char[] charr)
        {
            string res = "";
            string value = new string(charr);

            value = value.Replace('~', '-');
            value = value.Replace('!', '-');
            value = value.Replace('@', '-');
            value = value.Replace('#', '-');
            value = value.Replace('$', '-');
            value = value.Replace('%', '-');
            value = value.Replace('^', '-');
            value = value.Replace('&', '-');
            value = value.Replace('*', '-');
            value = value.Replace('(', '-');
            value = value.Replace(')', '-');
            value = value.Replace('_', '-');
            value = value.Replace('+', '-');
            value = value.Replace('|', '-');
            value = value.Replace('\\', '-');
            value = value.Replace('/', '-');
            value = value.Replace('?', '-');
            value = value.Replace('\'', '-');
            value = value.Replace('"', '-');
            value = value.Replace(' ', '-');
            value = value.Replace('>', '-');
            value = value.Replace('<', '-');
            value = value.Replace('.', '-');
            value = value.Replace(',', '-');
            value = value.Replace(':', '-');
            value = value.Replace("'", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");
            value = value.Replace("--", "-");

            res = value;
            return res;

        }

        /// <summary>
        /// get the Currency String For GateWay Without Exchange Rate
        /// </summary>
        /// <param name="amt">Decimal Amount</param>
        /// <returns>get the Currency String For GateWay Without EXchange Rate</returns>
        static public String CurrencyStringForGatewayWithoutExchangeRate(decimal amt)
        {
            String s = amt.ToString("#.00", new System.Globalization.CultureInfo("en-US"));
            if (s == ".00")
            {
                s = "0.00";
            }
            return s;
        }

        /// <summary>
        /// get the Currency String For DataBase With Hour ExchangeRate
        /// </summary>
        /// <param name="amt">Decimal Amount</param>
        /// <returns>return the Currency String For DataBase With Hour ExchangeRate</returns>
        public static String CurrencyStringForDBWithoutExchangeRate(decimal amt)
        {
            String tmpS = amt.ToString("C", new System.Globalization.CultureInfo("en-US"));
            if (tmpS.StartsWith("("))
            {
                tmpS = "-" + tmpS.Replace("(", "").Replace(")", "");
            }
            return tmpS.Replace("$", "").Replace(",", "");
        }

        /// <summary>
        /// This function set Any String to format of SEName 
        /// </summary>
        /// <param name="Word">String for SEName</param>
        /// <returns>SEName</returns>
        public static String SetSEName(String Word)
        {
            String Temp = "";
            Temp = Word.Replace("\"", "-").ToString().Replace(":", "-").Replace("!", "-").Replace("@", "-").Replace("?", "-").Replace("%", "-").Replace("*", "").Replace("$", "-").ToString();
            return Temp;
        }

        #region SendMail
        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="MailTo">MailToID</param>
        /// <param name="MailSubject">Subject for Email</param>
        /// <param name="MailBody">Mail Body</param>
        /// <param name="IPAddress">IPAddress of the machine, from where user is working</param>
        /// <param name="IsBodyHtml">Set true if you are sending Html message.</param>
        /// <param name="Attachment">Attachment if want to send as attachment. like logo. Set null if nothing.</param>
        /// <param name="FromID">Set this parameter if you want to set your email id as FromID, otherwise admin id will set default. </param>
        public static Int32 SendMail(String MailTo, String MailSubject, String MailBody, String IPAddress, bool IsBodyHtml, AlternateView Attachment)
        {
            String host = AppLogic.AppConfigs("Host");
            String username = AppLogic.AppConfigs("MailUserName");
            String password = AppLogic.AppConfigs("MailPassword");
            String FromID = AppLogic.AppConfigs("MailFrom");

            MailMessage Msg = new MailMessage();
            SmtpClient MailObj = new SmtpClient(host);
            MailObj.UseDefaultCredentials = false;
            MailObj.Credentials = new System.Net.NetworkCredential(username, password);


            if (AppLogic.AppConfigs("MailPort") != "")
            {
                MailObj.Port = Convert.ToInt32(AppLogic.AppConfigs("MailPort"));
                MailObj.EnableSsl = true;
            }
            string ReplyTo = "";

            if (MailSubject.ToString().ToLower().IndexOf("receipt for order #") > -1 || MailSubject.ToString().ToLower().IndexOf("package shipped") > -1)
            {
                ReplyTo = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT top 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='Replytomail' and Storeid=1 and isnull(Deleted,0)=0"));
            }

            MailObj.DeliveryMethod = SmtpDeliveryMethod.Network;
            //if (!string.IsNullOrEmpty(ReplyTo))
            //{
            //    Msg.From = new MailAddress(ReplyTo, ReplyTo);
            //}
            //else
            //{
                Msg.From = new MailAddress(FromID, FromID);
            //}

            String[] MailID = MailTo.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (Int32 i = 0; i < MailID.Length; i++)
                Msg.To.Add(new MailAddress(MailID[i].ToString()));
            Msg.Subject = MailSubject;
            MailBody = MailBody.Replace("###contactfooterdetail###", AppLogic.AppConfigs("Templatefootercommunication").ToString());
            MailBody = System.Text.RegularExpressions.Regex.Replace(MailBody, "###contactfooterdetail###", Convert.ToString(AppLogic.AppConfigs("Templatefootercommunication")), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Msg.Body = MailBody;
            Msg.IsBodyHtml = IsBodyHtml;
            if (Attachment != null)
                Msg.AlternateViews.Add(Attachment);
            try
            {


                if (MailSubject.ToString().ToLower().IndexOf("receipt for order #") > -1 || MailSubject.ToString().ToLower().IndexOf("package shipped") > -1)
                {

                    if (!string.IsNullOrEmpty(ReplyTo))
                    {
                        Msg.ReplyTo = new MailAddress(ReplyTo, ReplyTo);
                    }
                }
            }
            catch
            {

            }



            MailLogComponent objMailLog = new MailLogComponent();
            tb_MailLog tb_MailLog = new tb_MailLog();
            tb_MailLog.FromMail = Msg.From.ToString();
            tb_MailLog.ToEmail = Msg.To.ToString();
            tb_MailLog.Subject = Msg.Subject.ToString();
            tb_MailLog.IPAddress = IPAddress;
            tb_MailLog.MailDate = DateTime.Now;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            tb_MailLog.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            try
            {
                MailObj.Send(Msg);
                tb_MailLog.Body = Msg.Body.ToString();

                //Solution.Data.SQLAccess objsqlSentmsg = new Solution.Data.SQLAccess();
                //objsqlSentmsg.ExecuteNonQuery("Insert Into tb_EmailList([From],[To],Subject,Body,SentOn,IsAttachment,IsIncomming,AttachmentName,CreatedOn)Values('" + FromID.ToString() + "','" + MailTo.ToString() + "','" + MailSubject.ToString().Replace("'", "''") + "','" + MailBody.ToString().Replace("'", "''") + "','" + DateTime.Now.Date.ToString() + "','" + Convert.ToBoolean(0) + "','" + Convert.ToBoolean(0) + "','','" + DateTime.Now.Date.ToString() + "')");
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("CommonOperations.cs", ex.Message, ex.StackTrace);

                String Body = "<br/><br/>Problem in sending mail from user id: <b>" + Msg.From.ToString() + "</b><br/>";
                Body += "To ID: <b>" + Msg.To.ToString() + "</b><br/>";
                Body += "<br/><br/><b>Error Description: <b/>" + ex.Message.ToString();
                Body += "<br/><br/>Message Body: " + MailBody.ToString();
                tb_MailLog.Body = Body;
                tb_MailLog.Subject = "Failure :" + Msg.Subject.ToString();
            }
            int intMailID = objMailLog.InsertMailLog(tb_MailLog);
            return intMailID;
        }

        /// <summary>
        /// Send Mail With Reply To
        /// </summary>
        /// <param name="ReplyTo">ReplyTo</param>
        /// <param name="MailTo">MailToID</param>
        /// <param name="MailSubject">Subject for Email</param>
        /// <param name="MailBody">Mail Body</param>
        /// <param name="IPAddress">IPAddress of the machine, from where user is working</param>
        /// <param name="IsBodyHtml">Set true if you are sending Html message.</param>
        /// <param name="Attachment">Attachment if want to send as attachment. like logo. Set null if nothing.</param>
        /// <param name="FromID">Set this parameter if you want to set your email id as FromID, otherwise admin id will set default. </param>
        /// <returns>Identity Value</returns>
        public static Int32 SendMailWithReplyTo(String ReplyTo, String MailTo, String MailSubject, String MailBody, String IPAddress, bool IsBodyHtml, AlternateView Attachment)
        {
            String host = AppLogic.AppConfigs("Host");
            String username = AppLogic.AppConfigs("MailUserName");
            String password = AppLogic.AppConfigs("MailPassword");
            String FromID = AppLogic.AppConfigs("MailFrom");

            MailMessage Msg = new MailMessage();
            SmtpClient MailObj = new SmtpClient(host);
            MailObj.UseDefaultCredentials = false;
            MailObj.Credentials = new System.Net.NetworkCredential(username, password);

            if (AppLogic.AppConfigs("MailPort") != "")
            {
                MailObj.Port = Convert.ToInt32(AppLogic.AppConfigs("MailPort"));
                MailObj.EnableSsl = true;
            }

            MailObj.DeliveryMethod = SmtpDeliveryMethod.Network;
            Msg.From = new MailAddress(FromID, FromID);
            String[] MailID = MailTo.Split(';');
            for (Int32 i = 0; i < MailID.Length; i++)
                Msg.To.Add(new MailAddress(MailID[i].ToString()));
            Msg.Subject = MailSubject;
            MailBody = MailBody.Replace("###contactfooterdetail###", AppLogic.AppConfigs("Templatefootercommunication").ToString());
            MailBody = System.Text.RegularExpressions.Regex.Replace(MailBody, "###contactfooterdetail###", Convert.ToString(AppLogic.AppConfigs("Templatefootercommunication")), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Msg.Body = MailBody;
            Msg.IsBodyHtml = IsBodyHtml;
            Msg.ReplyTo = new MailAddress(ReplyTo, ReplyTo);
            if (Attachment != null)
                Msg.AlternateViews.Add(Attachment);

            MailLogComponent objMailLog = new MailLogComponent();
            tb_MailLog tb_MailLog = new tb_MailLog();
            tb_MailLog.FromMail = Msg.From.ToString();
            tb_MailLog.ToEmail = Msg.To.ToString();
            tb_MailLog.Subject = Msg.Subject.ToString();
            tb_MailLog.IPAddress = IPAddress;
            tb_MailLog.MailDate = DateTime.Now;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            tb_MailLog.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            try
            {
                MailObj.Send(Msg);
                tb_MailLog.Body = Msg.Body.ToString();
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("CommonOperations.cs", ex.Message, ex.StackTrace);

                String Body = "<br/><br/>Problem in sending mail from user id: <b>" + Msg.From.ToString() + "</b><br/>";
                Body += "To ID: <b>" + Msg.To.ToString() + "</b><br/>";
                Body += "<br/><br/><b>Error Description: <b/>" + ex.Message.ToString();
                Body += "<br/><br/>Message Body: " + MailBody.ToString();
                tb_MailLog.Body = Body;
                tb_MailLog.Subject = "Failure :" + Msg.Subject.ToString();
            }
            int intMailID = objMailLog.InsertMailLog(tb_MailLog);
            return intMailID;
        }

        /// <summary>
        /// Send Mail Attachment
        /// </summary>
        /// <param name="MailFrom">Mail From</param>
        /// <param name="MailTo">MailToID</param>
        /// <param name="MailSubject">Subject for Email</param>
        /// <param name="MailBody">Mail Body</param>
        /// <param name="IPAddress">IPAddress of the machine, from where user is working</param>
        /// <param name="IsBodyHtml">Set true if you are sending Html message.</param>
        /// <param name="Attachment">Attachment if want to send as attachment. like logo. Set null if nothing.</param>
        /// <param name="FromID">Set this parameter if you want to set your email id as FromID, otherwise admin id will set default. </param>
        /// <returns>Identity Value</returns>
        public static Int32 SendMailAttachment(string MailFrom, string MailTo, string MailCC, string MailBCC, string MailSubject, string MailBody, string IPAddress, bool IsBodyHtml, AlternateView Attachment, string filename)
        {
            MailMessage Msg = new MailMessage();
            string host = AppLogic.AppConfigs("Host");
            string username = AppLogic.AppConfigs("MailUserName");
            string password = AppLogic.AppConfigs("MailPassword");
            string FromID = MailFrom;

            SmtpClient MailObj = new SmtpClient(host);
            MailObj.UseDefaultCredentials = false;
            MailObj.Credentials = new System.Net.NetworkCredential(username, password);

            if (AppLogic.AppConfigs("MailPort") != "")
            {
                MailObj.Port = Convert.ToInt32(AppLogic.AppConfigs("MailPort"));
                MailObj.EnableSsl = true;
            }

            MailObj.DeliveryMethod = SmtpDeliveryMethod.Network;
            Msg.From = new MailAddress(FromID, FromID);
            string[] MailID = MailTo.Split(';');
            for (int i = 0; i < MailID.Length; i++)
                Msg.To.Add(new MailAddress(MailID[i].ToString()));

            if (!string.IsNullOrEmpty(MailCC.Trim()))
            {
                string[] MailIDCC = MailCC.Split(';');
                for (int i = 0; i < MailIDCC.Length; i++)
                    Msg.CC.Add(new MailAddress(MailIDCC[i].ToString()));
            }
            if (!string.IsNullOrEmpty(MailBCC.Trim()))
            {
                string[] MailIDBCC = MailBCC.Split(';');
                for (int i = 0; i < MailIDBCC.Length; i++)
                    Msg.Bcc.Add(new MailAddress(MailIDBCC[i].ToString()));
            }

            Msg.Subject = MailSubject;
            MailBody = MailBody.Replace("###contactfooterdetail###", AppLogic.AppConfigs("Templatefootercommunication").ToString());
            MailBody = System.Text.RegularExpressions.Regex.Replace(MailBody, "###contactfooterdetail###", Convert.ToString(AppLogic.AppConfigs("Templatefootercommunication")), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Msg.Body = MailBody;
            Msg.IsBodyHtml = IsBodyHtml;
            if (Attachment != null)
                Msg.AlternateViews.Add(Attachment);

            try
            {
                if (filename != null && filename.ToString() != "")
                {
                    Msg.Attachments.Add(new System.Net.Mail.Attachment(filename.ToString()));
                }
            }
            catch { }

            MailLogComponent objMailLog = new MailLogComponent();
            tb_MailLog tb_MailLog = new tb_MailLog();
            tb_MailLog.FromMail = Msg.From.ToString();
            tb_MailLog.ToEmail = Msg.To.ToString();
            tb_MailLog.Subject = Msg.Subject.ToString();
            tb_MailLog.IPAddress = IPAddress;
            tb_MailLog.MailDate = DateTime.Now;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_MailLog.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            try
            {
                MailObj.Send(Msg);
                tb_MailLog.Body = Msg.Body.ToString();
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("CommonOperations.cs", ex.Message, ex.StackTrace);

                string Body = "<br/><br/>Problem in sending mail from user id: <b>" + Msg.From.ToString() + "</b><br/>";
                Body += "To ID: <b>" + Msg.To.ToString() + "</b><br/>";
                Body += "<br/><br/><b>Error Description: <b/>" + ex.Message.ToString();
                Body += "<br/><br/>Message Body: " + MailBody.ToString();
                tb_MailLog.Body = Body;
                tb_MailLog.Subject = MailSubject;
            }
            Int32 MailLogID = 0;
            Int32.TryParse(Convert.ToString(objMailLog.InsertMailLog(tb_MailLog)), out MailLogID);
            return MailLogID;
        }

        /// <summary>
        /// Send Mail
        /// </summary>
        /// <param name="MailTo">MailToID</param>
        /// <param name="MailSubject">Subject for Email</param>
        /// <param name="MailBody">Mail Body</param>
        /// <param name="IPAddress">IPAddress of the machine, from where user is working</param>
        /// <param name="IsBodyHtml">Set true if you are sending Html message.</param>
        /// <param name="Attachment">Attachment if want to send as attachment. like logo. Set null if nothing.</param>
        /// <param name="FromID">Set this parameter if you want to set your email id as FromID, otherwise admin id will set default. </param>
        /// <returns>Identity Value</returns>
        public static Int32 SendMail(string MailFrom, string MailTo, string MailCC, string MailBCC, string MailSubject, string MailBody, string IPAddress, bool IsBodyHtml, AlternateView Attachment)
        {
            MailMessage Msg = new MailMessage();
            string host = AppLogic.AppConfigs("Host");
            string username = AppLogic.AppConfigs("MailUserName");
            string password = AppLogic.AppConfigs("MailPassword");
            string FromID = MailFrom;

            SmtpClient MailObj = new SmtpClient(host);
            MailObj.UseDefaultCredentials = false;
            MailObj.Credentials = new System.Net.NetworkCredential(username, password);

            if (AppLogic.AppConfigs("MailPort") != "")
            {
                MailObj.Port = Convert.ToInt32(AppLogic.AppConfigs("MailPort"));
                MailObj.EnableSsl = true;
            }

            MailObj.DeliveryMethod = SmtpDeliveryMethod.Network;
            Msg.From = new MailAddress(FromID, FromID);
            string[] MailID = MailTo.Split(';');
            for (int i = 0; i < MailID.Length; i++)
                Msg.To.Add(new MailAddress(MailID[i].ToString()));

            if (!string.IsNullOrEmpty(MailCC.Trim()))
            {
                string[] MailIDCC = MailCC.Split(';');
                for (int i = 0; i < MailIDCC.Length; i++)
                    Msg.CC.Add(new MailAddress(MailIDCC[i].ToString()));
            }
            if (!string.IsNullOrEmpty(MailBCC.Trim()))
            {
                string[] MailIDBCC = MailBCC.Split(';');
                for (int i = 0; i < MailIDBCC.Length; i++)
                    Msg.Bcc.Add(new MailAddress(MailIDBCC[i].ToString()));
            }

            Msg.Subject = MailSubject;
            MailBody = MailBody.Replace("###contactfooterdetail###", AppLogic.AppConfigs("Templatefootercommunication").ToString());
            MailBody = System.Text.RegularExpressions.Regex.Replace(MailBody, "###contactfooterdetail###", Convert.ToString(AppLogic.AppConfigs("Templatefootercommunication")), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Msg.Body = MailBody;
            Msg.IsBodyHtml = IsBodyHtml;
            if (Attachment != null)
                Msg.AlternateViews.Add(Attachment);

            MailLogComponent objMailLog = new MailLogComponent();
            tb_MailLog tb_MailLog = new tb_MailLog();
            tb_MailLog.FromMail = Msg.From.ToString();
            tb_MailLog.ToEmail = Msg.To.ToString();
            tb_MailLog.Subject = Msg.Subject.ToString();
            tb_MailLog.IPAddress = IPAddress;
            tb_MailLog.MailDate = DateTime.Now;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_MailLog.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            try
            {
                //MailObj.Send(Msg);
                tb_MailLog.Body = Msg.Body.ToString();
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("CommonOperations.cs", ex.Message, ex.StackTrace);

                string Body = "<br/><br/>Problem in sending mail from user id: <b>" + Msg.From.ToString() + "</b><br/>";
                Body += "To ID: <b>" + Msg.To.ToString() + "</b><br/>";
                Body += "<br/><br/><b>Error Description: <b/>" + ex.Message.ToString();
                Body += "<br/><br/>Message Body: " + MailBody.ToString();
                tb_MailLog.Body = Body;
                tb_MailLog.Subject = MailSubject;
            }
            Int32 MailLogID = 0;
            Int32.TryParse(Convert.ToString(objMailLog.InsertMailLog(tb_MailLog)), out MailLogID);
            return MailLogID;
        }

        /// <summary>
        /// Send Mail With Attachment For Email Management
        /// </summary>
        /// <param name="MailTo">MailToID</param>
        /// <param name="MailSubject">Subject for Email</param>
        /// <param name="MailBody">Mail Body</param>
        /// <param name="IPAddress">IPAddress of the machine, from where user is working</param>
        /// <param name="IsBodyHtml">Set true if you are sending Html message.</param>
        /// <param name="Attachment">Attachment if want to send as attachment. like logo. Set null if nothing.</param>
        /// <param name="FromID">Set this parameter if you want to set your email id as FromID, otherwise admin id will set default. </param>
        /// <returns>Identity Value</returns>
        public static Exception SendMailWithAttachmentForEmailManagement(string MailTo, string MailSubject, string WithMailBody, string MailBody, string IPAddress, bool IsBodyHtml, string Attachment, string AttachOnlyName, string concateCc, string concateBcc, int StoreID, string MapPath, string FwdAttachments, int OrderNum, AlternateView av, int AgentID)
        {
            Exception RetEx = new Exception();
            MailMessage Msg = new MailMessage();
            string host = AppLogic.AppConfigs("Host");
            string username = AppLogic.AppConfigs("MailUserName");
            string password = AppLogic.AppConfigs("MailPassword");
            string FromID = AppLogic.AppConfigs("MailFrom");

            bool isattactrue = false;

            SmtpClient MailObj = new SmtpClient(host);
            MailObj.UseDefaultCredentials = false;
            MailObj.Credentials = new System.Net.NetworkCredential(username, password);
            if (AppLogic.AppConfigs("MailPort") != "")
            {
                MailObj.Port = Convert.ToInt32(AppLogic.AppConfigs("MailPort"));
                MailObj.EnableSsl = true;
            }
            MailObj.DeliveryMethod = SmtpDeliveryMethod.Network;
            Msg.From = new MailAddress(FromID, FromID);
            string[] MailID = MailTo.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < MailID.Length; i++)
                Msg.To.Add(new MailAddress(MailID[i].ToString()));
            Msg.Subject = MailSubject;
            MailBody = MailBody.Replace("###contactfooterdetail###", AppLogic.AppConfigs("Templatefootercommunication").ToString());
            MailBody = System.Text.RegularExpressions.Regex.Replace(MailBody, "###contactfooterdetail###", Convert.ToString(AppLogic.AppConfigs("Templatefootercommunication")), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Msg.Body = MailBody;
            Msg.IsBodyHtml = IsBodyHtml;

            if (av != null)
            {
                Msg.AlternateViews.Add(av);
            }

            if (concateCc != "")
            {
                string[] Cccnt = concateCc.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < Cccnt.Length; i++)
                {
                    Msg.CC.Add(Cccnt[i].ToString());
                }
            }
            String MailCC = Convert.ToString(AppLogic.AppConfigs("MailCC"));
            if (MailCC != "")
            {
                Msg.CC.Add(MailCC);
            }

            if (concateBcc != "")
            {
                string[] Bcccnt = concateBcc.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < Bcccnt.Length; i++)
                {
                    Msg.Bcc.Add(Bcccnt[i].ToString());
                }
            }

            if (Attachment != "")
            {
                string[] Attachmentscnt = Attachment.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < Attachmentscnt.Length; i++)
                {
                    System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(Attachmentscnt[i].ToString());
                    Msg.Attachments.Add(att);
                    Msg.Attachments.Dispose();
                    //att.Dispose();
                    isattactrue = true;
                }
            }
            if (FwdAttachments != "")
            {
                string[] FwdAttachmentscnt = FwdAttachments.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < FwdAttachmentscnt.Length; i++)
                {
                    System.Net.Mail.Attachment att = new System.Net.Mail.Attachment(FwdAttachmentscnt[i].ToString());
                    Msg.Attachments.Add(att);
                    Msg.Attachments.Dispose();
                    att.Dispose();
                    isattactrue = true;
                }
            }

            MailLogComponent objMailLog = new MailLogComponent();
            tb_MailLog tb_MailLog = new tb_MailLog();
            tb_MailLog.FromMail = Msg.From.ToString();
            tb_MailLog.ToEmail = Msg.To.ToString();
            tb_MailLog.Subject = Msg.Subject.ToString();
            tb_MailLog.IPAddress = IPAddress;
            tb_MailLog.MailDate = DateTime.Now;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;
            tb_MailLog.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            //ObjMail.DateSent = Convert.ToDateTime(DateTime.Now.Date.ToString());
            //ObjMail.IsIncomming = false;
            //ObjMail.IsAttachment = isattactrue;
            //ObjMail.IsRead = false;
            //ObjMail.AttachmentName = AttachOnlyName.ToString();
            try
            {
                MailObj.Send(Msg);
                tb_MailLog.Body = Msg.Body.ToString();

                RetEx = null;
                Solution.Data.SQLAccess objsqlSentmsg = new Solution.Data.SQLAccess();
                int GetNewSentMailID = Convert.ToInt32(objsqlSentmsg.ExecuteScalarQuery("Insert Into tb_EmailList([From],[To],Subject,Body,SentOn,IsAttachment,IsIncomming,AttachmentName,CreatedOn,isRead,Cc,Bcc,OrderNumber,AgentID,IsReadAgent)Values('" + FromID.ToString() + "','" + MailTo.ToString() + "','" + MailSubject.ToString().Replace("'", "''") + "','" + WithMailBody.ToString().Replace("'", "''") + "','" + DateTime.Now.ToString() + "','" + isattactrue + "','" + Convert.ToBoolean(0) + "','" + AttachOnlyName.ToString() + "','" + DateTime.Now.ToString() + "',0,'" + concateCc.Trim().Replace("'", "''") + "','" + concateBcc.Trim().Replace("'", "''") + "'," + Convert.ToInt32(OrderNum) + "," + AgentID + ",'" + Convert.ToBoolean(0) + "'); SELECT SCOPE_IDENTITY();"));
                if (GetNewSentMailID > 0)
                {
                    //int GetNewSentMailID = Convert.ToInt32(objsqlSentmsg.ExecuteScalerQuery("Select Max(MailID) as NewSentMailID from tb_Ecomm_EmailList"));
                    try
                    {
                        if (Attachment != "")
                        {
                            string strNewFolder = MapPath + "\\MailID_" + GetNewSentMailID.ToString();
                            if (!System.IO.Directory.Exists(strNewFolder))
                            {
                                System.IO.Directory.CreateDirectory(strNewFolder);
                            }
                            string[] AttachmentsDelete = Attachment.Split(";".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                            for (int i = 0; i < AttachmentsDelete.Length; i++)
                            {
                                if (System.IO.File.Exists(AttachmentsDelete[i].ToString()))
                                {
                                    FileInfo fil = new FileInfo(AttachmentsDelete[i].ToString());
                                    //System.IO.File.Copy(AttachmentsDelete[i].ToString(), strNewFolder + "\\" + fil.Name.ToString());
                                    System.IO.File.Move(AttachmentsDelete[i].ToString(), strNewFolder + "\\" + fil.Name.ToString());
                                    //System.IO.File.Delete(AttachmentsDelete[i]);
                                }
                            }
                        }
                    }
                    catch { }
                }
            }
            catch (Exception ex)
            {
                string Body = "<br/><br/>Problem in sending mail from user id: <b>" + Msg.From.ToString() + "</b><br/>";
                Body += "To ID: <b>" + Msg.To.ToString() + "</b><br/>";
                Body += "<br/><br/><b>Error Description: <b/>" + ex.Message.ToString();
                Body += "<br/><br/>Message Body: " + MailBody.ToString();
                tb_MailLog.Body = Body;
                tb_MailLog.Subject = MailSubject;
                RetEx = ex;
            }

            Int32 MailLogID = 0;
            Int32.TryParse(Convert.ToString(objMailLog.InsertMailLog(tb_MailLog)), out MailLogID);

            return RetEx;
        }

        /// <summary>
        /// Send Test Mail
        /// </summary>
        /// <param name="MailUserName">MailUserName</param>
        /// <param name="MailPassword">MailPassword</param>
        /// <param name="MailHost">MailHost</param>
        /// <param name="MailTo">MailToID</param>
        /// <param name="MailSubject">Subject for Email</param>
        /// <param name="MailBody">Mail Body</param>
        /// <param name="IPAddress">IPAddress of the machine, from where user is working</param>
        /// <param name="IsBodyHtml">Set true if you are sending Html message.</param>
        /// <param name="Attachment">Attachment if want to send as attachment. like logo. Set null if nothing.</param>
        /// <param name="FromID">Set this parameter if you want to set your email id as FromID, otherwise admin id will set default. </param>
        /// <returns>Identity Value</returns>
        public static Int32 SendTestMail(String MailUserName, String MailPassword, String MailHost, String MailFrom, String MailTo, String MailSubject, String MailBody, String IPAddress, bool IsBodyHtml, AlternateView Attachment)
        {
            String host = MailHost;
            String username = MailUserName;
            String password = MailPassword;
            String FromID = MailFrom;

            MailMessage Msg = new MailMessage();
            SmtpClient MailObj = new SmtpClient(host);
            MailObj.UseDefaultCredentials = false;
            MailObj.Credentials = new System.Net.NetworkCredential(username, password);


            if (AppLogic.AppConfigs("MailPort") != "")
            {
                MailObj.Port = Convert.ToInt32(AppLogic.AppConfigs("MailPort"));
                MailObj.EnableSsl = true;
            }

            MailObj.DeliveryMethod = SmtpDeliveryMethod.Network;
            Msg.From = new MailAddress(FromID, FromID);
            String[] MailID = MailTo.Split(';');
            for (Int32 i = 0; i < MailID.Length; i++)
                Msg.To.Add(new MailAddress(MailID[i].ToString()));
            Msg.Subject = MailSubject;
            MailBody = MailBody.Replace("###contactfooterdetail###", AppLogic.AppConfigs("Templatefootercommunication").ToString());
            MailBody = System.Text.RegularExpressions.Regex.Replace(MailBody, "###contactfooterdetail###", Convert.ToString(AppLogic.AppConfigs("Templatefootercommunication")), System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            Msg.Body = MailBody;
            Msg.IsBodyHtml = IsBodyHtml;
            if (Attachment != null)
                Msg.AlternateViews.Add(Attachment);


            MailLogComponent objMailLog = new MailLogComponent();
            tb_MailLog tb_MailLog = new tb_MailLog();
            tb_MailLog.FromMail = Msg.From.ToString();
            tb_MailLog.ToEmail = Msg.To.ToString();
            tb_MailLog.Subject = Msg.Subject.ToString();
            tb_MailLog.IPAddress = IPAddress;
            tb_MailLog.MailDate = DateTime.Now;
            RedTag_CCTV_Ecomm_DBEntities ctx = RedTag_CCTV_Ecomm_DBEntities.Context;

            tb_MailLog.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            try
            {
                MailObj.Send(Msg);
                tb_MailLog.Body = Msg.Body.ToString();

                //Solution.Data.SQLAccess objsqlSentmsg = new Solution.Data.SQLAccess();
                //objsqlSentmsg.ExecuteNonQuery("Insert Into tb_EmailList([From],[To],Subject,Body,SentOn,IsAttachment,IsIncomming,AttachmentName,CreatedOn)Values('" + FromID.ToString() + "','" + MailTo.ToString() + "','" + MailSubject.ToString().Replace("'", "''") + "','" + MailBody.ToString().Replace("'", "''") + "','" + DateTime.Now.Date.ToString() + "','" + Convert.ToBoolean(0) + "','" + Convert.ToBoolean(0) + "','','" + DateTime.Now.Date.ToString() + "')");
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("CommonOperations.cs", ex.Message, ex.StackTrace);

                String Body = "<br/><br/>Problem in sending mail from user id: <b>" + Msg.From.ToString() + "</b><br/>";
                Body += "To ID: <b>" + Msg.To.ToString() + "</b><br/>";
                Body += "<br/><br/><b>Error Description: <b/>" + ex.Message.ToString();
                Body += "<br/><br/>Message Body: " + MailBody.ToString();
                tb_MailLog.Body = Body;
                tb_MailLog.Subject = "Failure :" + Msg.Subject.ToString();
            }
            int intMailID = objMailLog.InsertMailLog(tb_MailLog);
            return intMailID;
        }

        #endregion

        /// <summary>
        /// Register Cart
        /// </summary>
        /// <param name="CustID">CustomerID</param>
        /// <param name="IsAnonymousCust">Is the Customer is anonymous</param>
        /// <returns></returns>
        public static bool RegisterCart(Int32 CustID, bool IsAnonymousCust)
        {
            bool flag = false;
            CustomerComponent objCustomer = new CustomerComponent();
            ShoppingCartComponent objShoppingCart = new ShoppingCartComponent();
            WishListItemsComponent objWishListItems = new WishListItemsComponent();
            if (System.Web.HttpContext.Current.Session["UserName"] == null && System.Web.HttpContext.Current.Session["CustID"] == null)
            {
                flag = false;
            }
            else if (System.Web.HttpContext.Current.Session["UserName"] == null && System.Web.HttpContext.Current.Session["CustID"] != null)
            {
                flag = true;

                if (!IsAnonymousCust)
                {
                    objCustomer.UpdateRegisterFlag(CustID, true);
                }

                if (CustID.ToString() != System.Web.HttpContext.Current.Session["CustID"].ToString())
                {
                    objShoppingCart.DeleteCartItems(CustID);
                    objShoppingCart.UpdateCustomerForCart(CustID, Convert.ToInt32(System.Web.HttpContext.Current.Session["CustID"].ToString()));
                    objWishListItems.UpdateCustomerForWishList(CustID, Convert.ToInt32(System.Web.HttpContext.Current.Session["CustID"].ToString()));
                }
            }
            else
            {
                flag = true;
                objCustomer.UpdateRegisterFlag(CustID, true);
            }
            return flag;
        }

        /// <summary>
        /// Redirect the Page with SecureSocketLayer
        /// </summary>
        /// <param name="RedirectWithToSSL">Contain the true or false value for SSL</param>
        public static void RedirectWithSSL(Boolean RedirectWithToSSL)
        {
            //if (HttpContext.Current.Request.HttpMethod.ToLower() == "post")
            //    return;
            //String httpurl = HttpContext.Current.Request.Url.ToString();
            //if (RedirectWithToSSL)
            //{
            //    if (httpurl.ToLower().StartsWith("http://") && !httpurl.ToLower().StartsWith("https://"))
            //    {
            //        if (AppLogic.AppConfigBool("UseSSL"))
            //        {
            //            string checkUrl = httpurl.ToLower();

            //            if (HttpContext.Current.Request.RawUrl.Contains("/Rewriter.aspx?404;"))
            //                httpurl = HttpContext.Current.Request.RawUrl.Replace("/Rewriter.aspx?404;", "");
            //            else
            //                httpurl = HttpContext.Current.Request.RawUrl.ToLower().Replace("/rewriter.aspx?404;", "");

            //            if (httpurl.Contains("index.aspx"))
            //                httpurl = "http://" + HttpContext.Current.Request.Url.Authority + "/";

            //            httpurl = httpurl.Replace("http", "https").ToString();

            //            if (AppLogic.AppConfigBool("UseLiveRewritePath"))
            //                httpurl = httpurl.Replace(":" + HttpContext.Current.Request.Url.Port, "");
            //            HttpContext.Current.Response.Redirect(httpurl);
            //        }
            //    }
            //}
            //else
            //{
            //    string checkUrl = httpurl.ToLower();
            //    if (checkUrl.StartsWith("https://"))
            //    {

            //        if (HttpContext.Current.Request.RawUrl.Contains("/Rewriter.aspx?404;"))
            //            httpurl = HttpContext.Current.Request.RawUrl.Replace("/Rewriter.aspx?404;", "");
            //        else
            //            httpurl = HttpContext.Current.Request.RawUrl.ToLower().Replace("/rewriter.aspx?404;", "");

            //        if (httpurl.Contains("index.aspx"))
            //            httpurl = "http://" + HttpContext.Current.Request.Url.Authority + "/";

            //        httpurl = httpurl.Replace("https", "http").ToString();

            //        if (AppLogic.AppConfigBool("UseLiveRewritePath"))
            //            httpurl = httpurl.Replace(":" + HttpContext.Current.Request.Url.Port, "");
            //        HttpContext.Current.Response.Redirect(httpurl);
            //    }
            //}


            String httpurl = HttpContext.Current.Request.Url.ToString().ToLower();

            if (HttpContext.Current.Request.HttpMethod.ToLower() == "post")
                return;
            if (RedirectWithToSSL)
            {
                if (AppLogic.AppConfigBool("UseSSL"))
                {

                    if (httpurl.ToLower().StartsWith("http://") && !httpurl.ToLower().StartsWith("https://"))
                    {
                        httpurl = httpurl.Replace("http://", "https://").ToString();

                        if (AppLogic.AppConfigBool("UseLiveRewritePath"))
                            httpurl = httpurl.Replace(":" + HttpContext.Current.Request.Url.Port, "");
                        HttpContext.Current.Response.Redirect(httpurl);
                    }
                }
            }
            else
            {
                if (httpurl.StartsWith("https://"))
                {
                    httpurl = httpurl.Replace("https", "http").ToString();

                    if (AppLogic.AppConfigBool("UseLiveRewritePath"))
                        httpurl = httpurl.Replace(":" + HttpContext.Current.Request.Url.Port, "");

                    HttpContext.Current.Response.Redirect(httpurl);
                }
            }
        }
    }
}