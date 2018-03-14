using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class AddProductVariantColor : BasePage
    {
        public static string SearchProductTempPath = string.Empty;
        public static string SearchProductPath = string.Empty;
        ConfigurationComponent objConfiguration = new ConfigurationComponent();
        static int finHeight;
        static int finWidth;
        static Size thumbNailSizeIcon = Size.Empty;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            BindSize();
            if (!IsPostBack)
            {
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";

                if (!string.IsNullOrEmpty(Request.QueryString["ColorID"]))
                {
                    BindData(Convert.ToInt32(Request.QueryString["ColorID"]));
                    lblTitle.Text = "Edit Product Color Variant";
                }
            }
            txtColorName.Focus();
        }

        /// <summary>
        /// Fill State Data while Edit mode is Active 
        /// </summary>
        /// <param name="StateID">int StateID</param>
        private void BindData(Int32 SearchTypeID)
        {
            DataSet dsSearchpro = new DataSet();
            dsSearchpro = CommonComponent.GetCommonDataSet("Select ColorID,ColorName,IsNULL(ImagePath,'') as ImagePath,ISNULL(Active,0) as Active from tb_ProductColor where ColorID=" + SearchTypeID + "");
            if (dsSearchpro != null && dsSearchpro.Tables.Count > 0 && dsSearchpro.Tables[0].Rows.Count > 0)
            {
                txtColorName.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["ColorName"]);
                string Imagename = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["ImagePath"]);
                chkActive.Checked = Convert.ToBoolean(dsSearchpro.Tables[0].Rows[0]["Active"]);
                AppConfig.StoreID = 1;
                SearchProductPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/");
                if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                    Directory.CreateDirectory(Server.MapPath(SearchProductPath));
                Random rnd = new Random();
                if (!string.IsNullOrEmpty(Imagename))
                {
                    string strFilePath = Server.MapPath(SearchProductPath + Imagename);
                    btnDelete.Visible = false;

                    if (File.Exists(strFilePath))
                    {
                        ViewState["DelImage"] = Imagename;
                        btnDelete.Visible = true;
                        ImgLarge.Src = SearchProductPath + Imagename + "?" + rnd.Next(10000);
                    }
                    else
                    {
                        ViewState["DelImage"] = null;
                        btnDelete.Visible = false;
                        ImgLarge.Src = SearchProductPath + "image_not_available.jpg?" + rnd.Next(10000);
                    }
                }
                else
                {
                    ViewState["DelImage"] = null;
                    btnDelete.Visible = false;
                    ImgLarge.Src = SearchProductPath + "image_not_available.jpg?" + rnd.Next(10000);
                }
            }
        }

        #region Button Click Events

        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ProductVariantColorList.aspx");
        }

        /// <summary>
        /// Save button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtColorName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Color Name.', 'Message','');});", true);
                txtColorName.Focus();
                return;
            }

            Int32 Active = 0;
            if (chkActive.Checked)
                Active = 1;

            if (!string.IsNullOrEmpty(Request.QueryString["ColorID"]) && Convert.ToString(Request.QueryString["ColorID"]) != "0")
            {
                int chkDuplicate = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_ProductColor where ColorName='" + txtColorName.Text.Trim().ToString() + "' and ISNULL(Active,0)=1 AND ColorID <> " + Convert.ToString(Request.QueryString["ColorID"]) + ""));
                if (chkDuplicate > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Color Name already exists.', 'Message','');});", true);
                    return;
                }
                else
                {
                    string Imgname = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImagePath,'') as ImagePath from tb_ProductColor where ColorID=" + Convert.ToString(Request.QueryString["ColorID"]) + ""));
                    string strImageName = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(txtColorName.Text.ToString().Trim()))
                        {
                            strImageName = RemoveSpecialCharacter(txtColorName.Text.ToString().ToCharArray()) + "_" + Convert.ToString(Request.QueryString["ColorID"]) + ".jpg";

                            if (ImgLarge.Src.Contains(SearchProductTempPath))
                            {
                                SaveImage(strImageName);
                            }
                            else if (!string.IsNullOrEmpty(Imgname.ToString()))
                            {
                                if (File.Exists(Server.MapPath(SearchProductPath + Imgname)))
                                    File.Move(Server.MapPath(SearchProductPath + Imgname), Server.MapPath(SearchProductPath + strImageName));

                                try
                                {
                                    Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                                    objcompress.compressimage(Server.MapPath(SearchProductPath + strImageName));
                                }
                                catch
                                {

                                }
                            }
                        }
                        if (string.IsNullOrEmpty(ImgLarge.Src.ToString()) || ImgLarge.Src.ToString().ToLower().Contains("image_not_available"))
                            strImageName = "";
                    }
                    catch { }
                    CommonComponent.ExecuteCommonData("Update tb_ProductColor set ColorName='" + txtColorName.Text.Trim().ToString().Replace("'", "''") + "',Active=" + Active + ",ImagePath='" + strImageName + "' where ColorID = " + Convert.ToString(Request.QueryString["ColorID"]) + "");
                    Response.Redirect("ProductVariantColorList.aspx?status=updated");
                }
            }
            else
            {
                int chkDuplicate = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_ProductColor where SearchValue='" + txtColorName.Text.Trim().ToString() + "' and ISNULL(Active,0)=1"));
                if (chkDuplicate > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Color Name already exists.', 'Message','');});", true);
                    return;
                }
                else
                {
                    Int32 SearchId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Insert into tb_ProductColor (ColorName,ImagePath,CreatedOn,Active) values('" + txtColorName.Text.Trim().ToString().Replace("'", "''") + "','',Getdate()," + Active + ") Select Scope_identity();"));
                    if (SearchId > 0)
                    {
                        string strImageName = "";
                        if (!string.IsNullOrEmpty(txtColorName.Text.ToString().Trim()))
                        {
                            strImageName = RemoveSpecialCharacter(txtColorName.Text.ToString().ToCharArray()) + "_" + SearchId + ".jpg";
                            SaveImage(strImageName);
                        }
                        CommonComponent.ExecuteCommonData("Update tb_ProductColor set ImagePath='" + strImageName + "' where ColorID = " + SearchId + "");
                        Response.Redirect("ProductVariantColorList.aspx?status=inserted");
                    }
                }
            }
        }
        #endregion

        /// <summary>
        /// Save Image
        /// </summary>
        /// <param name="FileName">string FileName</param>
        protected void SaveImage(string FileName)
        {
            SearchProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/Temp/");
            SearchProductPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/");

            //create Micro folder 
            if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                Directory.CreateDirectory(Server.MapPath(SearchProductPath));

            CommonOperations.SaveOnContentServer(Server.MapPath(SearchProductPath));
            if (ImgLarge.Src.Contains(SearchProductTempPath))
            {
                try
                {
                    CreateImage("icon", FileName);
                }
                catch (Exception ex)
                {
                }
                finally
                {

                }
            }
        }

        /// <summary>
        ///  Create Image
        /// </summary>
        /// <param name="Size">string Size</param>
        /// <param name="FileName">string FileName</param>
        protected void CreateImage(string Size, string FileName)
        {
            try
            {
                string strFile = null;
                String strPath = "";
                if (ImgLarge.Src.ToString().IndexOf("?") > -1)
                {
                    strPath = ImgLarge.Src.Split('?')[0];
                }
                else
                {
                    strPath = ImgLarge.Src.ToString();
                }
                strFile = Server.MapPath(strPath);
                string strFilePath = "";
                Size = Size.ToLower();
                switch (Size)
                {
                    case "icon":
                        strFilePath = Server.MapPath(SearchProductPath + FileName);
                        if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToLower() == "edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(SearchProductPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                }
                ResizePhoto(strFile, Size, strFilePath);
            }
            catch (Exception ex)
            {
                if (ex.Source == "System.Drawing")
                    lblMsg.Text = "<br />Error Saving " + Size + " Image..Please check that Directory exists..";
                else
                    lblMsg.Text += "<br />" + ex.Message;
            }

        }

        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImage(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(ImageName)))
                    File.Delete(Server.MapPath(ImageName));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ImageName));
            }
            catch (Exception ex)
            {
                lblMsg.Text += "<br />" + ex.Message;
            }
        }

        /// <summary>
        /// Resize Photo
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="Size">string Size</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizePhoto(string strFile, string Size, string strFilePath)
        {
            switch (Size)
            {
                case "icon":
                    finHeight = thumbNailSizeIcon.Height;
                    finWidth = thumbNailSizeIcon.Width;
                    break;
            }
            ResizeImage(strFile, finWidth, finHeight, strFilePath);
        }

        private void BindSize()
        {

            DataSet dsIconWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "SearchProductIconWidth");
            DataSet dsIconHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "SearchProductIconHeight");
            if ((dsIconWidth != null && dsIconWidth.Tables.Count > 0 && dsIconWidth.Tables[0].Rows.Count > 0) && (dsIconHeight != null && dsIconHeight.Tables.Count > 0 && dsIconHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeIcon = new Size(Convert.ToInt32(77), Convert.ToInt32(77));
            }
        }

        /// <summary>
        /// Resize Images
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="FinWidth">int FinWidth</param>
        /// <param name="FinHeight">int FinHeight</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizeImage(string strFile, int FinWidth, int FinHeight, string strFilePath)
        {
            System.Drawing.Image imgVisol = System.Drawing.Image.FromFile(strFile);
            float resizePercent = 0;
            int resizedHeight = imgVisol.Height;
            int resizedWidth = imgVisol.Width;

            if (imgVisol.Height >= FinHeight && imgVisol.Width >= FinWidth)
            {
                float resizePercentHeight = 0;
                float resizePercentWidth = 0;
                resizePercentHeight = (FinHeight * 100) / imgVisol.Height;
                resizePercentWidth = (FinWidth * 100) / imgVisol.Width;
                if (resizePercentHeight < resizePercentWidth)
                {
                    resizedHeight = FinHeight;
                    resizedWidth = (int)Math.Round(resizePercentHeight * imgVisol.Width / 100.0);
                }
                if (resizePercentHeight >= resizePercentWidth)
                {
                    resizedWidth = FinWidth;
                    resizedHeight = (int)Math.Round(resizePercentWidth * imgVisol.Height / 100.0);
                }
            }
            else if (imgVisol.Width >= FinWidth && imgVisol.Height <= FinHeight)
            {
                resizedWidth = FinWidth;
                resizePercent = (FinWidth * 100) / imgVisol.Width;
                resizedHeight = (int)Math.Round((imgVisol.Height * resizePercent) / 100.0);
            }

            else if (imgVisol.Width <= FinWidth && imgVisol.Height >= FinHeight)
            {
                resizePercent = (FinHeight * 100) / imgVisol.Height;
                resizedHeight = FinHeight;
                resizedWidth = (int)Math.Round(resizePercent * imgVisol.Width / 100.0);
            }

            Bitmap resizedPhoto = new Bitmap(resizedWidth, resizedHeight, PixelFormat.Format24bppRgb);
            Graphics grPhoto = Graphics.FromImage(resizedPhoto);

            int destWidth = resizedWidth;
            int destHeight = resizedHeight;
            int sourceWidth = imgVisol.Width;
            int sourceHeight = imgVisol.Height;

            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle DestRect = new Rectangle(0, 0, destWidth, destHeight);
            Rectangle srcRect = new Rectangle(0, 0, sourceWidth, sourceHeight);
            grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grPhoto.DrawImage(imgVisol, DestRect, srcRect, GraphicsUnit.Pixel);
            GenerateImage(resizedPhoto, strFilePath, FinWidth, FinHeight);
            resizedPhoto.Dispose();
            grPhoto.Dispose();
            imgVisol.Dispose();
        }

        /// <summary>
        /// Generate Image
        /// </summary>
        /// <param name="extBMP">Bitmap extBMP</param>
        /// <param name="DestFileName">string DestFileName</param>
        /// <param name="DefWidth">int DefWidth</param>
        /// <param name="DefHeight">int DefHeight</param>
        private void GenerateImage(Bitmap extBMP, string DestFileName, int DefWidth, int DefHeight)
        {
            Encoder Enc = Encoder.SaveFlag;
            EncoderParameters EncParms = new EncoderParameters(1);
            EncoderParameter EncParm;
            ImageCodecInfo CodecInfo = GetEncoderInfo("image/jpeg");
            EncParm = new EncoderParameter(Encoder.Quality, (long)600);
            EncParms.Param[0] = new EncoderParameter(Encoder.Quality, (long)600);

            if (extBMP != null && extBMP.Width < (DefWidth) && extBMP.Height < (DefHeight))
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
                try
                {
                    Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                    objcompress.compressimage(DestFileName);
                }
                catch
                {

                }
                CommonOperations.SaveOnContentServer(DestFileName);

            }
            else if (extBMP != null && extBMP.Width < (DefWidth))
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
                try
                {
                    Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                    objcompress.compressimage(DestFileName);
                }
                catch
                {

                }
                CommonOperations.SaveOnContentServer(DestFileName);

            }
            else if (extBMP != null && extBMP.Height < (DefHeight))
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
                try
                {
                    Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                    objcompress.compressimage(DestFileName);
                }
                catch
                {

                }
                CommonOperations.SaveOnContentServer(DestFileName);

            }
            else if (extBMP != null)
            {
                extBMP.Save(DestFileName, CodecInfo, EncParms);
                extBMP.Dispose();
                try
                {
                    Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                    objcompress.compressimage(DestFileName);
                }
                catch
                {

                }
                CommonOperations.SaveOnContentServer(DestFileName);

            }
        }

        /// <summary>
        /// Get Encoder Information
        /// </summary>
        /// <param name="resizeMimeType">string resizeMimeType</param>
        /// <returns>Returns the ImageCodecInfo Object</returns>
        private static ImageCodecInfo GetEncoderInfo(string resizeMimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();
            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == resizeMimeType)
                    return codecs[i];
            return null;
        }

        /// <summary>
        /// Delete Temp files
        /// </summary>
        /// <param name="strsize">string strsize</param>
        protected void DeleteTempFile(string strsize)
        {
            try
            {
                string path = string.Empty;
                if (ViewState["File"] != null && ViewState["File"].ToString().Trim().Length > 0)
                {
                    path = Server.MapPath(SearchProductTempPath + ViewState["File"].ToString());
                }
                File.Delete(path);
            }
            catch { }
        }

        /// <summary>
        /// Function for Remove Special Characters
        /// </summary>
        /// <param name="charr">char[] charr</param>
        /// <returns>Returns String value after Remove Special Character</returns>
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
        ///  Upload Image Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            SearchProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/Temp/");
            if (!Directory.Exists(Server.MapPath(SearchProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(SearchProductTempPath));
            for (int j = 0; j < StoreArray.Length; j++)
                if (fuProductIcon.FileName.Length > 0 && Path.GetExtension(fuProductIcon.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;
            if (Flag)
            {
                if (fuProductIcon.FileName.Length > 0)
                {
                    ViewState["File"] = fuProductIcon.FileName.ToString();
                    fuProductIcon.SaveAs(Server.MapPath(SearchProductTempPath) + fuProductIcon.FileName);
                    ImgLarge.Src = SearchProductTempPath + fuProductIcon.FileName;
                }
                else
                {
                    ViewState["File"] = null;
                }
            }
            else
            {
                string StrMsg = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + StrMsg.ToString() + "', 'Message','ContentPlaceHolder1_fuProductIcon');});", true);
            }

        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (File.Exists(Server.MapPath(SearchProductPath + ViewState["DelImage"].ToString())))
                    File.Delete(Server.MapPath(SearchProductPath + ViewState["DelImage"].ToString()));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(SearchProductPath + ViewState["DelImage"].ToString()));
            }
            catch { }
            ViewState["DelImage"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgLarge.Src = SearchProductPath + "image_not_available.jpg";
            btnDelete.Visible = false;
        }
    }
}