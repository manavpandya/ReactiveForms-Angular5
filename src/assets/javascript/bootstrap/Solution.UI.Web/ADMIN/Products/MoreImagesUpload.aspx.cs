using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Text;
using System.Drawing.Imaging;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class MoreImagesUpload : BasePage
    {
        int lastimageid = 0;
        String ImageName = String.Empty;
        public static string ProductTempPath = string.Empty;
        public static string ProductIconPath = string.Empty;
        public static string ProductMediumPath = string.Empty;
        public static string ProductLargePath = string.Empty;
        public static string ProductMicroPath = string.Empty;
        static int finHeight;
        static int finWidth;
        static Size thumbNailSizeLarge = Size.Empty;
        static Size thumbNailSizeMediam = Size.Empty;
        static Size thumbNailSizeIcon = Size.Empty;
        static Size thumbNailSizeMicro = Size.Empty;
        ConfigurationComponent objConfiguration = new ConfigurationComponent();

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
                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");

                String ProductID = Convert.ToString(Request.QueryString["ID"]);
                DataSet DsProduct = new DataSet();
                DsProduct = ProductComponent.GetProductByProductID(Convert.ToInt32(ProductID));
                if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
                {
                    string Imagename = Convert.ToString(DsProduct.Tables[0].Rows[0]["Imagename"]);
                    lblProductName.Text = Convert.ToString(DsProduct.Tables[0].Rows[0]["name"]);
                    SetMainImage(Imagename);

                    if (!string.IsNullOrEmpty(Imagename.ToString()))
                    {
                        String[] StrArr = Imagename.ToString().Split('.');
                        Imagename = StrArr[0];
                    }

                    string SKU = Convert.ToString(DsProduct.Tables[0].Rows[0]["SKU"]);
                    ViewState["ImageName"] = Imagename;
                    ViewState["ProductID"] = ProductID;
                    LoadImages(Convert.ToInt32(ProductID));
                }
                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            }
        }

        /// <summary>
        /// Set Main Image of Product
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        private void SetMainImage(string ImageName)
        {
            if (!string.IsNullOrEmpty(ImageName))
            {
                String FinalImagename = ProductIconPath + ImageName;
                StringBuilder sb = new StringBuilder();

                if (File.Exists(Server.MapPath(FinalImagename)))
                {
                    sb.Append("<img title=\"Main Image\" src=\"" + FinalImagename + "\" style=\"border:solid 1px #eeeeee;\">&nbsp;");
                }
                ltOldimages.Text = sb.ToString();
                if (sb == null || sb.ToString() == "")
                {
                    tblOldImage.Visible = false;
                }
                else
                {
                    tblOldImage.Visible = true;
                }
            }
        }

        /// <summary>
        ///  Bind Size       
        /// </summary>
        private void BindSize()
        {
            DataSet dsIconWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductIconWidth");
            DataSet dsIconHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductIconHeight");
            if ((dsIconWidth != null && dsIconWidth.Tables.Count > 0 && dsIconWidth.Tables[0].Rows.Count > 0) && (dsIconHeight != null && dsIconHeight.Tables.Count > 0 && dsIconHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeIcon = new Size(Convert.ToInt32(dsIconWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsIconHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsLargeWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductLargeWidth");
            DataSet dsLargeHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductLargeHeight");
            if ((dsLargeWidth != null && dsLargeWidth.Tables.Count > 0 && dsLargeWidth.Tables[0].Rows.Count > 0) && (dsLargeHeight != null && dsLargeHeight.Tables.Count > 0 && dsLargeHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeLarge = new Size(Convert.ToInt32(dsLargeWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsLargeHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsMediumWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductMediumWidth");
            DataSet dsMediumHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductMediumHeight");
            if ((dsMediumWidth != null && dsMediumWidth.Tables.Count > 0 && dsMediumWidth.Tables[0].Rows.Count > 0) && (dsMediumHeight != null && dsMediumHeight.Tables.Count > 0 && dsMediumHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeMediam = new Size(Convert.ToInt32(dsMediumWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMediumHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

            DataSet dsMicroWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductMicroWidth");
            DataSet dsMicroHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "ProductMicroHeight");
            if ((dsMicroWidth != null && dsMicroWidth.Tables.Count > 0 && dsMicroWidth.Tables[0].Rows.Count > 0) && (dsMicroHeight != null && dsMicroHeight.Tables.Count > 0 && dsMicroHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeMicro = new Size(Convert.ToInt32(dsMicroWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsMicroHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
            }

        }

        /// <summary>
        /// Upload More Images Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            if (ltOldimages.Text != "")
            {
                if (ViewState["ProductID"] != null && ViewState["ProductID"].ToString() != "")
                {
                    LoadImages(Convert.ToInt32(ViewState["ProductID"].ToString()));
                }
                if (fileUploder.FileName != "")
                {
                    bool Flag = false;
                    String Extension = String.Empty;
                    StringArrayConverter Storeconvertor = new StringArrayConverter();
                    Array StoreArray = (Array)Storeconvertor.ConvertFrom(AppLogic.AppConfigs("AllowedExtensions"));

                    if (!Directory.Exists(Server.MapPath(ProductLargePath)))
                    {
                        Directory.CreateDirectory(Server.MapPath(ProductLargePath));
                    }

                    for (int j = 0; j < StoreArray.Length; j++)
                    {
                        if (fileUploder.FileName.Length > 0)
                        {
                            if (Path.GetExtension(fileUploder.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString())
                            {
                                Extension = StoreArray.GetValue(j).ToString();
                                Flag = true;
                            }
                        }
                    }
                    if (Flag)
                    {
                        if (fileUploder.FileName.Length > 0)
                        {
                            Extension = ".jpg";
                            int lastimageindex = 0;
                            int productid = 0;
                            if (ViewState["ProductID"] != null && ViewState["ProductID"].ToString() != "")
                            {
                                productid = Convert.ToInt32(ViewState["ProductID"].ToString());
                            }
                            if (ViewState["LastImageid"] != null && ViewState["LastImageid"].ToString() != "")
                            {
                                lastimageindex = Convert.ToInt32(ViewState["LastImageid"].ToString());
                            }
                            else
                            {
                                lastimageindex = 1;
                            }
                            fileUploder.SaveAs(Server.MapPath(ProductTempPath + fileUploder.FileName).ToString());
                            SaveImages(ViewState["ImageName"].ToString() + "_" + lastimageindex + Extension);
                            ImageName = ViewState["ImageName"].ToString();

                            if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo= " + lastimageindex + " and ProductId=" + ViewState["ProductID"] + "")) > 0)
                            {
                                CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtDescription.Text.ToString().Replace("'", "''") + "' Where ImageNo= " + lastimageindex + " and ProductId=" + ViewState["ProductID"] + "");
                            }
                            else
                            {
                                if (!string.IsNullOrEmpty(txtDescription.Text.ToString()))
                                    CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description) Values(" + ViewState["ProductID"] + "," + lastimageindex + ",'" + txtDescription.Text.ToString().Replace("'", "''") + "')");
                                else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description) Values(" + ViewState["ProductID"] + "," + lastimageindex + ",'')");
                            }
                            txtDescription.Text = "";
                            LoadImages(productid);
                        }
                    }
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Please Upload Main Image ...');window.close();", true);
            }
        }

        /// <summary>
        /// Check if Images Exists
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        /// <returns>Return the Status as a string format</returns>
        private String CheckImagesExists(string ImageName)
        {
            String FinalImageName = String.Empty;
            Boolean Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(AppLogic.AppConfigs("AllowedExtensions"));
            ImageName = ProductLargePath + ImageName;

            for (int j = 0; j < StoreArray.Length; j++)
            {
                if (File.Exists(Server.MapPath(ImageName + StoreArray.GetValue(j))))
                {
                    Flag = true;
                    ImageName = ImageName + StoreArray.GetValue(j);
                    break;
                }

            }
            if (!Flag)
            {
                return "NotAvailable";
            }
            else
            {
                return ImageName;
            }

        }

        /// <summary>
        /// Gets the Index for New Image
        /// </summary>
        /// <param name="LastIndex">int LastIndex </param>
        /// <param name="MImageName">string MImageName</param>
        private void GetIndexForNewImage(int LastIndex, String MImageName)
        {
            Boolean First = true;
            for (int i = 1; i <= LastIndex; i++)
            {
                String OtherImageName = CheckImagesExists(MImageName + "_" + i);
                if (OtherImageName == "NotAvailable")
                {
                    if (First)
                    {
                        lastimageid = i;
                        ViewState["LastImageid"] = lastimageid;
                        First = false;
                    }
                }
            }
            if (lastimageid == 0)
            {
                lastimageid = LastIndex + 1;
                ViewState["LastImageid"] = lastimageid;
            }
        }

        /// <summary>
        /// Generate HTML For More Images
        /// </summary>
        /// <param name="MImageName">string MImageName</param>
        /// <param name="ProductId">int ProductId</param>
        private void GenerateHTMLForImages(String MImageName, Int32 ProductId)
        {
            String SearchPartten = MImageName + "_*";
            System.IO.FileInfo[] array = new System.IO.DirectoryInfo(Server.MapPath(ProductLargePath)).GetFiles(SearchPartten, System.IO.SearchOption.TopDirectoryOnly);

            Array.Sort(array, delegate(System.IO.FileInfo f1, System.IO.FileInfo f2)
            {
                return f1.CreationTime.CompareTo(f2.CreationTime);
            });
            string noimg = AppLogic.AppConfigs("Noofimages").ToString();
            int NoofImages = Convert.ToInt32((string.IsNullOrEmpty(noimg)) ? "3" : noimg);
            StringBuilder sb = new StringBuilder();
            if (array.Length > 0)
            {
                sb.AppendLine("<table><tbody><tr>");
                for (int i = 0; i < array.Length && i < NoofImages; i++)
                {
                    String[] SearchImageName = array[i].Name.ToString().Split('.');
                    String OtherImageName = CheckImagesExists(SearchImageName[0]);
                    if (OtherImageName != "NotAvailable")
                    {
                        if (((i) % 3) == 0)
                        {
                            sb.AppendLine("</tr><tr>");
                        }
                        sb.AppendLine("<td id='Col_" + i + "_" + ProductId + "'>");
                        sb.AppendLine("<table  width='190' border='0' align='center' cellpadding='0' cellspacing='0'><tr>");
                        sb.AppendLine("<td align='center' valign='middle' style='font-size:12px;color:#212121;font-family:Arial'  >" + (i + 1) + "</td></tr>");
                        sb.AppendLine("<tr><td  align='center' valign='middle' class='mian_bg'><table width='190' border='0' align='center' cellpadding='0' cellspacing='0'>");
                        sb.AppendLine("<tr><td align='center' valign='middle' class='big_img'><img id='Img_" + i + "_" + ProductId + "' src='" + OtherImageName + "' width='120' height='120' style=\"border:solid 1px #eeeeee;\" /></td></tr>");
                        sb.AppendLine("<tr><td height='30' align='center' valign='middle'><input type='button'  id='Delete_" + i + "_" + ProductId + "' onclick=\"javascript:DeleteImage('" + OtherImageName + "');\" style='background:url(/App_Themes/" + Page.Theme + "/images/delet.gif) no-repeat;width:56px;height:22px;border:0;cursor:pointer' title='Delete' />&nbsp;&nbsp;<input type='button'  id='Clear_" + i + "_" + ProductId + "' onclick=\"javascript:ClearImage('" + OtherImageName + "');\" style='background:url(/App_Themes/" + Page.Theme + "/images/clear.gif) no-repeat;width:56px;height:22px;border:0;cursor:pointer' title='Clear Image Description' /></td></tr>");
                        sb.AppendLine("</table></td></tr>");
                        sb.AppendLine("</table></td>");
                    }
                }
                sb.AppendLine("</tr></tbody></table>");
                ltMoreimages.Text = sb.ToString();
            }
            else
            {
                ltMoreimages.Text = "";
            }
        }

        /// <summary>
        /// Count Total No Of Images
        /// </summary>
        /// <param name="MImageName">string MImageName</param>
        private void countTotalNoOfImages(string MImageName)
        {
            String SearchPartten = MImageName + "_*";
            System.IO.FileInfo[] array = new System.IO.DirectoryInfo(Server.MapPath(ProductLargePath)).GetFiles(SearchPartten, System.IO.SearchOption.TopDirectoryOnly);

            Array.Sort(array, delegate(System.IO.FileInfo f1, System.IO.FileInfo f2)
            {
                return f1.CreationTime.CompareTo(f2.CreationTime);
            });

            int NoofImages = 0;
            try
            {
                NoofImages = Convert.ToInt32(AppLogic.AppConfigs("Noofimages").ToString());
            }
            catch { NoofImages = 25; }

            if (array.Length >= NoofImages)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Your Can Not Upload More than " + NoofImages + " Images.');", true);
            }
        }

        /// <summary>
        /// Load Images by ProductId
        /// </summary>
        /// <param name="ProductId">int ProductId</param>
        private void LoadImages(int ProductId)
        {
            String MImageName = ViewState["ImageName"].ToString();
            if (MImageName != "")
            {
                String FinalImagename = ProductLargePath + MImageName + "_";
                String SearchPartten = MImageName + "_*";
                String[] Arrayofimages = Directory.GetFiles(Server.MapPath(ProductLargePath), SearchPartten, System.IO.SearchOption.TopDirectoryOnly);
                StringBuilder sb = new StringBuilder();
                System.IO.FileInfo[] array = new System.IO.DirectoryInfo(Server.MapPath(ProductLargePath)).GetFiles(SearchPartten, System.IO.SearchOption.TopDirectoryOnly);

                Array.Sort(array, delegate(System.IO.FileInfo f1, System.IO.FileInfo f2)
                {
                    return f1.CreationTime.CompareTo(f2.CreationTime);
                });

                GetIndexForNewImage(array.Length, MImageName);
                GenerateHTMLForImages(MImageName, ProductId);
                countTotalNoOfImages(MImageName);

                try
                {
                    File.Delete(Server.MapPath(ProductTempPath + fileUploder.FileName).ToString());
                    CommonOperations.DeleteFileFromContentServer(Server.MapPath(ProductTempPath + fileUploder.FileName).ToString());
                }
                catch { }
            }
        }

        /// <summary>
        /// Save Images of Product
        /// </summary>
        /// <param name="FileName">string FileName</param>
        private void SaveImages(string FileName)
        {
            CreateImage("Medium", FileName);
            CreateImage("Small", FileName);
            CreateImage("micro", FileName);
            CreateImage("Large", FileName);
        }

        /// <summary>
        /// Create Image
        /// </summary>
        /// <param name="Size">string Size</param>
        /// <param name="FileName">string FileName</param>
        protected void CreateImage(string Size, string FileName)
        {
            try
            {
                string strFile = null;
                strFile = Server.MapPath(ProductTempPath + fileUploder.FileName);
                string strFilePath = "";
                Size = Size.ToLower();
                switch (Size)
                {
                    case "large":
                        strFilePath = Server.MapPath(ProductLargePath + FileName);
                        break;
                    case "medium":
                        strFilePath = Server.MapPath(ProductMediumPath + FileName);
                        break;
                    case "small":
                        strFilePath = Server.MapPath(ProductIconPath + FileName);
                        break;
                    case "micro":
                        strFilePath = Server.MapPath(ProductMicroPath + FileName);
                        break;
                }
                ResizePhoto(strFile, Size, strFilePath);
            }
            catch { }
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
                case "medium":
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
        /// Resize Image
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="FinWidth">int FinWidth</param>
        /// <param name="FinHeight">int FinHeight</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizeImage(string strFile, int FinWidth, int FinHeight, string strFilePath)
        {
            System.Drawing.Image imgRedTag = System.Drawing.Image.FromFile(strFile);
            float resizePercent = 0;
            int resizedHeight = imgRedTag.Height;
            int resizedWidth = imgRedTag.Width;

            if (imgRedTag.Height >= FinHeight && imgRedTag.Width >= FinWidth)
            {
                float resizePercentHeight = 0;
                float resizePercentWidth = 0;
                resizePercentHeight = (FinHeight * 100) / imgRedTag.Height;
                resizePercentWidth = (FinWidth * 100) / imgRedTag.Width;
                if (resizePercentHeight < resizePercentWidth)
                {
                    resizedHeight = FinHeight;
                    resizedWidth = (int)Math.Round(resizePercentHeight * imgRedTag.Width / 100.0);
                }
                if (resizePercentHeight >= resizePercentWidth)
                {
                    resizedWidth = FinWidth;
                    resizedHeight = (int)Math.Round(resizePercentWidth * imgRedTag.Height / 100.0);
                }
            }
            else if (imgRedTag.Width >= FinWidth && imgRedTag.Height <= FinHeight)
            {
                resizedWidth = FinWidth;
                resizePercent = (FinWidth * 100) / imgRedTag.Width;
                resizedHeight = (int)Math.Round((imgRedTag.Height * resizePercent) / 100.0);
            }

            else if (imgRedTag.Width <= FinWidth && imgRedTag.Height >= FinHeight)
            {
                resizePercent = (FinHeight * 100) / imgRedTag.Height;
                resizedHeight = FinHeight;
                resizedWidth = (int)Math.Round(resizePercent * imgRedTag.Width / 100.0);
            }

            Bitmap resizedPhoto = new Bitmap(resizedWidth, resizedHeight, PixelFormat.Format24bppRgb);
            Graphics grPhoto = Graphics.FromImage(resizedPhoto);

            int destWidth = resizedWidth;
            int destHeight = resizedHeight;
            int sourceWidth = imgRedTag.Width;
            int sourceHeight = imgRedTag.Height;

            grPhoto.Clear(Color.White);
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle DestRect = new Rectangle(0, 0, destWidth, destHeight);
            Rectangle srcRect = new Rectangle(0, 0, sourceWidth, sourceHeight);
            grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            grPhoto.DrawImage(imgRedTag, DestRect, srcRect, GraphicsUnit.Pixel);

            GenerateImage(resizedPhoto, strFilePath, FinWidth, FinHeight);

            resizedPhoto.Dispose();
            grPhoto.Dispose();
            imgRedTag.Dispose();
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
            System.Drawing.Imaging.Encoder Enc = System.Drawing.Imaging.Encoder.SaveFlag;
            EncoderParameters EncParms = new EncoderParameters(1);
            EncoderParameter EncParm;
            ImageCodecInfo CodecInfo = GetEncoderInfo("image/jpeg");
            EncParm = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)600);
            EncParms.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, (long)600);

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
        /// Function for Remove Special Characters
        /// </summary>
        /// <param name="charr">char[] charr</param>
        /// <returns>Returns Result as  a String Format</returns>
        /// 
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
        ///  Delete Image Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteImg_Click(object sender, EventArgs e)
        {
            if (hdnimageUrl.Value != "")
            {
                string StrValue = hdnimageUrl.Value.ToString();

                if (!string.IsNullOrEmpty(StrValue.ToString()))
                {
                    String[] StrArr = StrValue.ToString().Split('/');
                    int tot = StrArr.Count();
                    if (tot > 0)
                    {
                        StrValue = StrArr[tot - 1];

                        string[] StrImgVal = StrValue.ToString().Split('_');
                        if (StrImgVal.Length > 2 && Request.QueryString["ID"] != null)
                        {
                            string[] ImgId = StrImgVal[2].ToString().Split('.');
                            if (ImgId.Length > 0)
                            {
                                try
                                {
                                    CommonComponent.ExecuteCommonData("Delete from tb_ProductImgDesc where ProductId=" + Request.QueryString["ID"].ToString() + " and ImageNo=" + ImgId[0].ToString() + "");
                                }
                                catch { }
                            }
                        }


                        if (File.Exists(Server.MapPath(ProductLargePath + StrValue)))
                        {
                            File.Delete(Server.MapPath(ProductLargePath + StrValue));
                        }
                        if (File.Exists(Server.MapPath(ProductMediumPath + StrValue)))
                        {
                            File.Delete(Server.MapPath(ProductMediumPath + StrValue));
                        }
                        if (File.Exists(Server.MapPath(ProductMicroPath + StrValue)))
                        {
                            File.Delete(Server.MapPath(ProductMicroPath + StrValue));
                        }
                        if (File.Exists(Server.MapPath(ProductIconPath + StrValue)))
                        {
                            File.Delete(Server.MapPath(ProductIconPath + StrValue));
                        }

                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Image Deleted Successfully.');", true);
                        LoadImages(Convert.ToInt32(Request.QueryString["ID"]));
                    }
                }
            }
        }

        /// <summary>
        /// Clear Img Desc
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnClearDesc_Click(object sender, EventArgs e)
        {
            if (hdnimageUrl.Value != "")
            {
                string StrValue = hdnimageUrl.Value.ToString();

                if (!string.IsNullOrEmpty(StrValue.ToString()))
                {
                    String[] StrArr = StrValue.ToString().Split('/');
                    int tot = StrArr.Count();
                    if (tot > 0)
                    {
                        StrValue = StrArr[tot - 1];

                        string[] StrImgVal = StrValue.ToString().Split('_');
                        if (StrImgVal.Length > 2 && Request.QueryString["ID"] != null)
                        {
                            string[] ImgId = StrImgVal[2].ToString().Split('.');
                            if (ImgId.Length > 0)
                            {
                                try
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='' where ProductId=" + Request.QueryString["ID"].ToString() + " and ImageNo=" + ImgId[0].ToString() + "");
                                }
                                catch { }
                            }
                        }
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "alert('Image Description Cleared Successfully.');", true);
                        LoadImages(Convert.ToInt32(Request.QueryString["ID"]));
                    }
                }
            }
        }
    }
}