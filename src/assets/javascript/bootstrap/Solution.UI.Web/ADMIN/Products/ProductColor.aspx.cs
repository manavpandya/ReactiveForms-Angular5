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
    public partial class ProductColor : BasePage
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
            btnAddToSelectionlist.ImageUrl = "~/App_Themes/" + Page.Theme + "/images/add-to-selection-list.png";

            if (!IsPostBack)
            {
                BindProductColor();
                //if (ddlProductColor.SelectedIndex == 0)
                //    ltrImagePath.Text = "<img alt=\"\" src=\"" + string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/") + "image_not_available.jpg" + "\">";
                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString().ToLower() == "edit")
                {
                    string StrVariId = Convert.ToString(Request.QueryString["VariId"]);
                    string StrProductID = Convert.ToString(Request.QueryString["ProductID"]);
                    BindOldData(StrVariId, StrProductID);
                }
            }
        }

        protected void BindOldData(string VariantValueId, string ProductId)
        {
            try
            {
                DataSet dsVariant = new DataSet();
                dsVariant = CommonComponent.GetCommonDataSet("Select ISNULL(VariImageName,'') as VariImageName,VariantID,VariantValue,DisplayOrder from tb_ProductVariantValue Where Productid=" + ProductId + " and VariantValueid=" + VariantValueId + "");
                if (dsVariant != null && dsVariant.Tables.Count > 0 && dsVariant.Tables[0].Rows.Count > 0)
                {
                    string StrVariName = Convert.ToString(dsVariant.Tables[0].Rows[0]["VariantValue"].ToString());
                    string StrVariImageName = Convert.ToString(dsVariant.Tables[0].Rows[0]["VariImageName"].ToString());
                   // ddlProductColor.ClearSelection();

                   // ddlProductColor.Items.FindByText(StrVariName).Selected = true;
                    bool IsImgExists = false;
                    string StrVariPath = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImagePath,'') as ImagePath from tb_ProductColor where ColorID=" + StrVariName.ToString() + ""));
                    if (!string.IsNullOrEmpty(StrVariPath))
                    {
                        string ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/");
                        string StrPath = ProductTempPath + StrVariPath.ToString();
                        if (File.Exists(Server.MapPath(StrPath)))
                        {
                            ltrImagePath.Text = "<img alt=\"\" src=\"" + StrPath.ToString() + "\">";
                            ImgLarge.Src = "";
                            ViewState["OldProductColorFile"] = StrVariPath.ToString();
                            ViewState["ProductColorFile"] = null;
                        }
                    }
                    if (IsImgExists == false)
                    {
                        if (!string.IsNullOrEmpty(StrVariImageName))
                        {
                            string ProductPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/");
                            string StrPath = ProductPath + StrVariImageName.ToString();
                            if (File.Exists(Server.MapPath(StrPath)))
                            {
                                ltrImagePath.Text = "";
                                ImgLarge.Src = StrPath.ToString();
                                ViewState["OldProductColorFile"] = null;
                                ViewState["ProductColorFile"] = StrVariImageName.ToString();
                            }
                        }
                    }
                }
            }
            catch { }
        }

        protected void BindProductColor()
        {
            DataSet dscolor = new DataSet();
            dscolor = CommonComponent.GetCommonDataSet("Select * from tb_productcolor where ISNULL(active,0)=1");
            if (dscolor != null && dscolor.Tables.Count > 0 && dscolor.Tables[0].Rows.Count > 0)
            {
                ddlProductColor.DataSource = dscolor.Tables[0];
                ddlProductColor.DataTextField = "ColorName";
                ddlProductColor.DataValueField = "ColorId";
                ddlProductColor.DataBind();
            }
            ddlProductColor.Items.Insert(0, new ListItem("Select Color", "0"));
        }

        /// <summary>
        ///  Add to Color Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnAddToSelectionlist_Click(object sender, ImageClickEventArgs e)
        {
            //if (ddlProductColor.SelectedIndex > 0)
            //{
                string StrId = "";
                string StrColorImgId = "";
                if (Request.QueryString["clientid"].ToString().IndexOf("arelatedColor") > -1)
                {
                    if (Request.QueryString["clientid"].ToString().IndexOf("_arelatedColor_") > -1)
                        StrId = Convert.ToString(Request.QueryString["clientid"].ToString().Replace("_arelatedColor_", "_txttitle_"));
                    else
                        StrId = Convert.ToString(Request.QueryString["clientid"].ToString().Replace("_arelatedColor", "_txtTitle"));

                    string StrValuePro = "";
                    if (ViewState["ProductColorFile"] != null)
                    {
                        StrValuePro = Convert.ToString(ViewState["ProductColorFile"]);
                    }
                    else
                    {
                        StrValuePro = "";//Convert.ToString(ViewState["OldProductColorFile"]);
                    }

                    if (Request.QueryString["clientid"].ToString().IndexOf("_arelatedColor_") > -1)
                        StrColorImgId = Convert.ToString(Request.QueryString["clientid"].ToString().Replace("_arelatedColor_", "_hdnProductColorNewImg_"));
                    else
                        StrColorImgId = Convert.ToString(Request.QueryString["clientid"].ToString().Replace("_arelatedColor", "_hdnProductColorNewImg"));

                    Page.ClientScript.RegisterClientScriptBlock(btnAddToSelectionlist.GetType(), "@closemsg", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').innerHTML = '" + ddlProductColor.SelectedItem.Text.ToString() + "';window.opener.document.getElementById('" + StrColorImgId.ToString() + "').value = '" + StrValuePro.ToString() + "';window.opener.document.getElementById('ContentPlaceHolder1_hdnrelatedcolor').value = '" + ddlProductColor.SelectedItem.Text.ToString() + "';window.close();", true);
                    //;window.close();", true);

                    //Page.ClientScript.RegisterClientScriptBlock(btnAddToSelectionlist.GetType(), "@closemsg", "window.opener.document.getElementById('" + Request.QueryString["clientid"].ToString() + "').innerHTML = '" + ddlProductColor.SelectedItem.Text.ToString() + "';window.opener.document.getElementById('" + StrColorImgId.ToString() + "').value = '" + StrValuePro.ToString() + "';window.opener.document.getElementById('" + StrId.ToString() + "').value = '" + ddlProductColor.SelectedItem.Text.ToString() + "';window.opener.document.getElementById('ContentPlaceHolder1_hdnrelatedcolor').value = '" + ddlProductColor.SelectedItem.Text.ToString() + "';window.close();", true);
                }
            //}
        }

        protected void ddlProductColor_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool IsImgExists = false;
            //if (ddlProductColor.SelectedIndex > 0)
            //{
                string StrVariPath = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImagePath,'') as ImagePath from tb_ProductColor where ColorID=" + ddlProductColor.SelectedValue.ToString() + ""));
                if (!string.IsNullOrEmpty(StrVariPath))
                {
                    string ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/");
                    string StrPath = ProductTempPath + StrVariPath.ToString();
                    if (File.Exists(Server.MapPath(StrPath)))
                    {
                        ltrImagePath.Text = "<img alt=\"\" src=\"" + StrPath.ToString() + "\">";
                        IsImgExists = true;
                        ViewState["OldProductColorFile"] = StrVariPath.ToString();
                    }
                }
                if (IsImgExists == false)
                {
                    ViewState["OldProductColorFile"] = null;
                    ltrImagePath.Text = "<img alt=\"\" src=\"" + string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/") + "image_not_available.jpg" + "\">";
                }
            //}
            //else
            //{
            //    ltrImagePath.Text = "<img alt=\"\" src=\"" + string.Concat(AppLogic.AppConfigs("ImagePathProductVariant"), "Color/") + "image_not_available.jpg" + "\">";
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgnew", "alert('Please Select Product Color');", true);
            //    return;
            //}
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
                    ViewState["ProductColorFile"] = fuProductIcon.FileName.ToString();
                    fuProductIcon.SaveAs(Server.MapPath(SearchProductTempPath) + fuProductIcon.FileName);
                    ImgLarge.Src = SearchProductTempPath + fuProductIcon.FileName;
                }
                else
                {
                    ViewState["ProductColorFile"] = null;
                }
            }
            else
            {
                string StrMsg = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + StrMsg.ToString() + "', 'Message','ContentPlaceHolder1_fuProductIcon');});", true);
            }

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
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg", "alert('Error Saving " + Size + " Image..Please check that Directory exists..')", true);
                else
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg1", "alert('" + ex.Message.ToString() + "')", true);
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
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@errmsg2", "alert('" + ex.Message.ToString() + "')", true);
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
            // Get image codes for all image formats 
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
                if (ViewState["ProductColorFile"] != null && ViewState["ProductColorFile"].ToString().Trim().Length > 0)
                {
                    path = Server.MapPath(SearchProductTempPath + ViewState["ProductColorFile"].ToString());
                }
                File.Delete(path);
            }
            catch { }
        }
    }
}