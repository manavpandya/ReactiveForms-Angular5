using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.IO;
using Solution.Bussines.Entities;
using System.Data;
using System.Collections;
using Solution.Bussines.Components.AdminCommon;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace Solution.UI.Web.ADMIN.Product
{
    public partial class AddProductPattern : BasePage
    {
        tb_Product tb_Product = null;
        public static string ProductTempPath = string.Empty;
        public static string ProductIconPath = string.Empty;
        public static string ProductMediumPath = string.Empty;
        public static string ProductLargePath = string.Empty;
        public static string ProductMicroPath = string.Empty;
        static public int PatternID = 0;
        static int finHeight;
        static int finWidth;
        ConfigurationComponent objConfiguration = new ConfigurationComponent();
        static Size thumbNailSizeLarge = Size.Empty;
        static Size thumbNailSizeMediam = Size.Empty;
        static Size thumbNailSizeIcon = Size.Empty;
        static Size thumbNailSizeMicro = Size.Empty;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delete.gif";
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";

                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Iphone/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Micro/");

                BindSize();
                if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null)
                {
                    BindData(Convert.ToInt32(Request.QueryString["ID"]));
                }
                else
                {

                    ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                }
            }
        }


        /// <summary>
        /// Bind Product Data
        /// </summary>
        /// <param name="ID">int ID</param>
        public void BindData(Int32 ID)
        {
            DataSet DsProduct = new DataSet();
            ProductComponent objPro = new ProductComponent();
            DsProduct = objPro.GetProductPatternbyID(ID);

            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
                txtPattern.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["PatternName"]);
                txtDescription.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["PatternDescription"]);
                txtDisplayOrder.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["DisplayOrderBy"]);
                chkIsActive.Checked = Convert.ToBoolean(DsProduct.Tables[0].Rows[0]["Active"]);
                hdnimage.Value = Convert.ToString(DsProduct.Tables[0].Rows[0]["ImgName"]);
                #region Get Image
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Large/");
                if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                    Directory.CreateDirectory(Server.MapPath(ProductMediumPath));
                string Imagename = DsProduct.Tables[0].Rows[0]["ImgName"].ToString();
                string strFilePath = Server.MapPath(ProductMediumPath + Imagename);

                btnDelete.Visible = false;
                if (File.Exists(strFilePath))
                {
                    ViewState["DelImage"] = Imagename;
                    btnDelete.Visible = true;
                    ImgLarge.Src = ProductMediumPath + Imagename;
                }
                else
                {
                    ViewState["DelImage"] = null;
                    btnDelete.Visible = false;
                    ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                }
                #endregion

            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "Edit")
            {
                UpdateProduct(Convert.ToInt32(Request.QueryString["ID"]));
            }
            else
            {
                InsertProduct();
            }
        }


        /// <summary>
        /// Inserts the product
        /// </summary>
        public void InsertProduct()
        {
            Int32 IsExists = Convert.ToInt32(CommonComponent.GetScalarCommonData("select isnull(count(1),0) as cCount from tb_Pattern where PatternName = '" + txtPattern.Text.ToString().Trim().Replace("'", "''") + "' and isnull(Deleted,0) = 0"));
            if (IsExists > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "IsExists", "$(document).ready( function() {jAlert('Pattern name is already exists specify another Pattern name', 'Message','ContentPlaceHolder1_txtPattern');});", true);
                txtPattern.Focus();
                return;
            }
            else
            {
                string strImageName = "";

                int retColorID = 0;
                retColorID = ProductComponent.InsertUpdateProductPattern(0, txtPattern.Text.ToString().Trim(), txtDescription.Text.ToString().Trim(), chkIsActive.Checked ? true : false, false, Convert.ToInt32(txtDisplayOrder.Text.ToString()), strImageName);

                if (!string.IsNullOrEmpty(txtPattern.Text.ToString().Trim()) && retColorID > 0)
                {
                    strImageName = RemoveSpecialCharacter(Convert.ToString(txtPattern.Text.ToString()).ToCharArray()) + "_" + PatternID + ".jpg";
                    strImageName = strImageName.Replace(" ", "").Replace("&", "-");
                    SaveImage(strImageName);
                    CommonComponent.ExecuteCommonData("update tb_Pattern set ImgName = '" + strImageName.ToString().Trim().Replace("'", "''") + "' where PatternID = " + retColorID);
                }
                Response.Redirect("ProductPatternList.aspx?Insert=true");
            }
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="ProductID">int ProductID</param>
        public void UpdateProduct(int PatternID)
        {
            ProductComponent objProduct = new ProductComponent();

            Int32 ChkSKu = Convert.ToInt32(CommonComponent.GetScalarCommonData("select ISNULL(count(PatternName),0) as TotCnt From tb_Pattern Where PatternName='" + txtPattern.Text.ToString().Trim() + "' And  PatternID <>" + PatternID + " AND ISNULL(Deleted,0)=0 "));
            if (ChkSKu > 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('Pattern with same name already exists specify another Pattern', 'Message','ContentPlaceHolder1_txtPattern');});", true);
                txtPattern.Focus();
                return;
            }
            else
            {
                #region Save and Update Image
                string strImageName = "";
                try
                {
                    if (!string.IsNullOrEmpty(txtPattern.Text.ToString().Trim()))
                    {
                        if (!string.IsNullOrEmpty(hdnimage.Value))
                            strImageName = hdnimage.Value.ToString();
                        else
                            strImageName = RemoveSpecialCharacter(Convert.ToString(txtPattern.Text.ToString()).ToCharArray()) + "_" + PatternID + ".jpg";
                        strImageName = strImageName.Replace(" ", "").Replace("&", "-");
                        string Imgname = strImageName;
                        if (ImgLarge.Src.Contains(ProductTempPath))
                        {
                            SaveImage(strImageName);
                        }
                        else if (!string.IsNullOrEmpty(Imgname.ToString()))
                        {
                            if (File.Exists(Server.MapPath(ProductLargePath + Imgname)))
                                File.Move(Server.MapPath(ProductLargePath + Imgname), Server.MapPath(ProductLargePath + strImageName));

                            if (File.Exists(Server.MapPath(ProductMediumPath + Imgname)))
                                File.Move(Server.MapPath(ProductMediumPath + Imgname), Server.MapPath(ProductMediumPath + strImageName));

                            if (File.Exists(Server.MapPath(ProductMicroPath + Imgname)))
                                File.Move(Server.MapPath(ProductMicroPath + Imgname), Server.MapPath(ProductMicroPath + strImageName));

                            if (File.Exists(Server.MapPath(ProductIconPath + Imgname)))
                                File.Move(Server.MapPath(ProductIconPath + Imgname), Server.MapPath(ProductIconPath + strImageName));
                        }
                    }

                    if (string.IsNullOrEmpty(ImgLarge.Src.ToString()) || ImgLarge.Src.ToString().ToLower().Contains("image_not_available"))
                        strImageName = "";
                    else
                        CommonOperations.SaveOnContentServer(Server.MapPath(ImgLarge.Src.ToString().ToLower()));
                    ProductComponent.InsertUpdateProductPattern(PatternID, txtPattern.Text.ToString().Trim(), txtDescription.Text.ToString().Trim(), chkIsActive.Checked ? true : false, false, Convert.ToInt32(txtDisplayOrder.Text.ToString()), strImageName);

                }
                catch { }

                #endregion

                Response.Redirect("ProductPatternList.aspx?Update=true");
            }
        }

        /// <summary>
        /// Function for Remove Special Characters
        /// </summary>
        /// <param name="charr">char[] charr</param>
        /// <returns>Returns the String Value</returns>
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

        #region Image Upload and resize

        /// <summary>
        /// Save Image
        /// </summary>
        /// <param name="FileName">string FileName</param>
        protected void SaveImage(string FileName)
        {
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Temp/");
            ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Icon/");
            ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/IPhone/");
            ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Large/");
            ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Micro/");

            //create icon folder 
            if (!Directory.Exists(Server.MapPath(ProductIconPath)))
                Directory.CreateDirectory(Server.MapPath(ProductIconPath));

            //create Medium folder 
            if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                Directory.CreateDirectory(Server.MapPath(ProductMediumPath));

            //create Large folder 
            if (!Directory.Exists(Server.MapPath(ProductLargePath)))
                Directory.CreateDirectory(Server.MapPath(ProductLargePath));

            //create Micro folder 
            if (!Directory.Exists(Server.MapPath(ProductMicroPath)))
                Directory.CreateDirectory(Server.MapPath(ProductMicroPath));

            CommonOperations.SaveOnContentServer(Server.MapPath(ProductIconPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductMediumPath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductLargePath));
            CommonOperations.SaveOnContentServer(Server.MapPath(ProductMicroPath));

            if (ImgLarge.Src.Contains(ProductTempPath))
            {
                try
                {
                    CreateImage("IPhone", FileName);
                    CreateImage("Icon", FileName);
                    CreateImage("Micro", FileName);
                    CreateImage("Large", FileName);
                }
                catch (Exception ex)
                {
                    lblMsg.Text += "<br />" + ex.Message;
                }
                finally
                {
                    DeleteTempFile("icon");
                }
            }
        }

        /// <summary>
        /// Delete Temp files
        /// </summary>
        /// <param name="strsize">string strsize</param>
        protected void DeleteTempFile(string strsize)
        {
            try
            {
                if (strsize == "icon")
                {
                    string path = string.Empty;
                    if (ViewState["File"] != null && ViewState["File"].ToString().Trim().Length > 0)
                    {
                        path = Server.MapPath(ProductTempPath + ViewState["File"].ToString());
                    }

                    File.Delete(path);
                }
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("Product.aspx", ex.Message, ex.StackTrace);
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
                    case "large":
                        strFilePath = Server.MapPath(ProductLargePath + FileName);

                        if (Request.QueryString["Mode"] == "Edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductLargePath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "iphone":
                        strFilePath = Server.MapPath(ProductMediumPath + FileName);
                        if (Request.QueryString["Mode"] == "Edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductMediumPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "icon":
                        strFilePath = Server.MapPath(ProductIconPath + FileName);
                        if (Request.QueryString["Mode"] == "Edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductIconPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "micro":
                        strFilePath = Server.MapPath(ProductMicroPath + FileName);
                        if (Request.QueryString["Mode"] == "Edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductMicroPath + ViewState["DelImage"].ToString());
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
                CommonComponent.ErrorLog("Product.aspx", ex.Message, ex.StackTrace);
            }

        }

        /// <summary>
        /// Delete Image
        /// </summary>
        /// <param name="ImageName"string ImageName></param>
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
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
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
                case "iphone":
                    finHeight = thumbNailSizeMediam.Height;
                    finWidth = thumbNailSizeMediam.Width;
                    break;
                case "icon":
                    finHeight = thumbNailSizeIcon.Height;
                    finWidth = thumbNailSizeIcon.Width;
                    break;
                case "micro":
                    finHeight = thumbNailSizeMicro.Height;
                    finWidth = thumbNailSizeMicro.Width;
                    break;

            }
            if (Size == "large")
            {
                File.Copy(strFile, strFilePath, true);
                CommonOperations.SaveOnContentServer(strFilePath);
            }
            else
                ResizeImage(strFile, finWidth, finHeight, strFilePath);
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
        /// GenerateImage
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
                CommonOperations.SaveOnContentServer(DestFileName);
            }
            else if (extBMP != null)
            {
                extBMP.Save(DestFileName, CodecInfo, EncParms);
                extBMP.Dispose();
                CommonOperations.SaveOnContentServer(DestFileName);
            }
        }

        /// <summary>
        /// GetEncoderInfo
        /// </summary>
        /// <param name="resizeMimeType">string resizeMimeType</param>
        /// <returns>Returns the ImageCodecInfo object</returns>
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
        /// Bind Sizes for Generating image
        /// </summary>
        private void BindSize()
        {
            DataSet dsIconWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "PatternIconWidth");
            DataSet dsIconHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "PatternIconHeight");
            if ((dsIconWidth != null && dsIconWidth.Tables.Count > 0 && dsIconWidth.Tables[0].Rows.Count > 0) && (dsIconHeight != null && dsIconHeight.Tables.Count > 0 && dsIconHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeIcon = new Size(Convert.ToInt32(dsIconWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsIconHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            //DataSet dsLargeWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductLargeWidth");
            //DataSet dsLargeHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductLargeHeight");
            //if ((dsLargeWidth != null && dsLargeWidth.Tables.Count > 0 && dsLargeWidth.Tables[0].Rows.Count > 0) && (dsLargeHeight != null && dsLargeHeight.Tables.Count > 0 && dsLargeHeight.Tables[0].Rows.Count > 0))
            //{
            //    thumbNailSizeLarge = new Size(Convert.ToInt32(dsLargeWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsLargeHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            //}

            DataSet dsMediumWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "PatternIphoneWidth");
            DataSet dsMediumHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "PatternIphoneHeight");
            if ((dsMediumWidth != null && dsMediumWidth.Tables.Count > 0 && dsMediumWidth.Tables[0].Rows.Count > 0) && (dsMediumHeight != null && dsMediumHeight.Tables.Count > 0 && dsMediumHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeMediam = new Size(Convert.ToInt32(dsMediumWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMediumHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsMicroWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "PatternMicroWidth");
            DataSet dsMicroHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "PatternMicroHeight");
            if ((dsMicroWidth != null && dsMicroWidth.Tables.Count > 0 && dsMicroWidth.Tables[0].Rows.Count > 0) && (dsMicroHeight != null && dsMicroHeight.Tables.Count > 0 && dsMicroHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeMicro = new Size(Convert.ToInt32(dsMicroWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMicroHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

        }

        #endregion

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
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (fuProductIcon.FileName.Length > 0 && Path.GetExtension(fuProductIcon.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fuProductIcon.FileName.Length > 0)
                {
                    ViewState["File"] = fuProductIcon.FileName.ToString();
                    fuProductIcon.SaveAs(Server.MapPath(ProductTempPath) + fuProductIcon.FileName);
                    ImgLarge.Src = ProductTempPath + fuProductIcon.FileName;
                    lblMsg.Text = "";
                }
                else
                {
                    ViewState["File"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_fuProductIcon');});", true);
            }

        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDelete_Click(object sender, ImageClickEventArgs e)
        {
            if (ImgLarge.Src.ToString().Trim().IndexOf("image_not_available") > -1)
            {

            }
            else
            {
                if (ViewState["DelImage"] != null)
                {
                    ImgLarge.Src = "";
                    String strProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Pattern/Icon/");
                    if (!Directory.Exists(Server.MapPath(strProductMediumPath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(strProductMediumPath));
                    }
                    string Imagename = Convert.ToString(ViewState["DelImage"]);
                    string strFilePath = Server.MapPath(strProductMediumPath + Imagename);

                    if (File.Exists(strFilePath))
                    {
                        File.Delete(strFilePath);
                    }
                    btnDelete.Visible = false;

                    ImgLarge.Src = strProductMediumPath + "image_not_available.jpg";
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('Problem while deleting image.', 'Message');});", true);
                    return;
                }

            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ProductPatternList.aspx");
        }

    }
}