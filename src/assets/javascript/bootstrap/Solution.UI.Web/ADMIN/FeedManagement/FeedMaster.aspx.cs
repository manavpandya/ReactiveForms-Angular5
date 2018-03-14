using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Data;
using System.Web.UI.HtmlControls;

namespace Solution.UI.Web.ADMIN.FeedManagement
{
    public partial class FeedMaster : BasePage
    {
        tb_FeedMaster tbFeedMaster = null;
        static int finHeight;
        static int finWidth;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btncancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                btnUploadHoverImage.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnDeleteHover.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";
                btnUploadTabImage.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnDeleteTab.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";

                ImgTab.Src = AppLogic.AppConfigs("FeedImagePath") + "image_not_available.jpg";
                ImgHover.Src = AppLogic.AppConfigs("FeedImagePath") + "image_not_available.jpg";
                bindstore();
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
                {
                    ddlStore.Enabled = false;
                    BindData(Convert.ToInt32(Request.QueryString["Id"]));
                    lblTitle.Text = "Update Feed Master";
                }
            }
        }

        /// <summary>
        /// Binds the Data of Feed Details into Gridview.
        /// </summary>
        /// <param name="ID">int ID</param>
        public void BindData(int ID)
        {
            DataSet dsfeedDetails = new DataSet();
            FeedComponent objfeed = new FeedComponent();
            dsfeedDetails = objfeed.GetDatabyFeedId(ID);
            if (dsfeedDetails != null && dsfeedDetails.Tables.Count > 0 & dsfeedDetails.Tables[0].Rows.Count > 0)
            {
                txtFeedName.Text = dsfeedDetails.Tables[0].Rows[0]["FeedName"].ToString();
                chkIsBase.Checked = Convert.ToBoolean(dsfeedDetails.Tables[0].Rows[0]["ISBase"].ToString());
                ddlStore.SelectedValue = dsfeedDetails.Tables[0].Rows[0]["StoreId"].ToString();
                string Imagepath = AppLogic.AppConfigs("FeedImagePath");
                if (Imagepath != null && Imagepath.ToString() != "")
                {
                    if (!Directory.Exists(Server.MapPath(Imagepath)))
                        Directory.CreateDirectory(Server.MapPath(Imagepath));

                    if (File.Exists(Server.MapPath(Imagepath + "/" + ID.ToString() + ".jpg")) == true)
                    {
                        ImgTab.Src = Imagepath + "/" + ID.ToString() + ".jpg";
                        btnDeleteTab.Visible = true;
                    }
                    else
                    {
                        ImgTab.Src = AppLogic.AppConfigs("FeedImagePath") + "image_not_available.jpg";
                        btnDeleteTab.Visible = false;
                    }

                    if (File.Exists(Server.MapPath(Imagepath + "/" + ID.ToString() + "_hover.jpg")) == true)
                    {
                        ImgHover.Src = Imagepath + "/" + ID.ToString() + "_hover.jpg";
                        btnDeleteHover.Visible = true;
                    }
                    else
                    {
                        ImgHover.Src = AppLogic.AppConfigs("FeedImagePath") + "image_not_available.jpg";
                        btnDeleteHover.Visible = false;
                    }
                }
            }
        }


        /// <summary>
        /// Bind All Stores into Drop Down
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

            try
            {
                int SID = Convert.ToInt32(AppLogic.AppConfigs("StoreId"));
                ListItem itm = ddlStore.Items.FindByValue(SID.ToString());
                ddlStore.SelectedIndex = ddlStore.Items.IndexOf(itm);
            }
            catch { }

        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtFeedName.Text.ToString().Trim()))
            {
                if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
                {
                    Update(Convert.ToInt32(Request.QueryString["Id"]));
                }
                else
                {
                    InsertFeed();
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msgInsert", "alert('Please Enter Feed Name.');", true);
            }
        }


        /// <summary>
        /// Updates the specified feed id.
        /// </summary>
        /// <param name="FeedId">int FeedId.</param>
        protected void Update(int FeedId)
        {
            Int32 ChkName = 0;
            if (chkIsBase.Checked)
                ChkName = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(ISNULL([IsBase],0)) as TotBase From tb_FeedMaster wHERE StoreId=" + Convert.ToInt32(ddlStore.SelectedValue) + " And isBase=1 and FeedId<>" + FeedId + ""));
            if (ChkName > 0 && hdnIsBase.Value == "0")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chnageBaseFeed", "confirmIsBase();", true);
                return;
            }
            FeedComponent FeedComponent = new FeedComponent();
            tbFeedMaster = new tb_FeedMaster();
            tbFeedMaster = FeedComponent.getFeed(FeedId);
            tbFeedMaster.FeedName = txtFeedName.Text.ToString().Trim();
            tbFeedMaster.IsBase = chkIsBase.Checked;
            Int32 StoreId = Convert.ToInt32(ddlStore.SelectedValue);
            tbFeedMaster.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(StoreId));


            Int32 ResultFeedID = Convert.ToInt32(FeedComponent.UpdateFeed(tbFeedMaster));
            if (ResultFeedID > 0)
            {

                if (hdnIsBase.Value == "1")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_FeedMaster SET IsBase=0;UPDATE dbo.tb_FeedMaster SET IsBase=1 WHERE FeedID=" + ResultFeedID + ";");
                }

                if (ViewState["DeleteTabImage"] != null)
                {
                    DeleteImage(Convert.ToString(ViewState["DeleteTabImage"]));
                }
                if (ViewState["DeleteHoverImage"] != null)
                {
                    DeleteImage(Convert.ToString(ViewState["DeleteHoverImage"]));
                }

                string Imagepath = AppLogic.AppConfigs("FeedImagePath");
                string FName = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImageName,'') As ImageName From tb_FeedMaster Where FeedID=" + Request.QueryString["ID"].ToString().Trim() + ""));
                if (!string.IsNullOrEmpty(Imagepath))
                {
                    if (!string.IsNullOrEmpty(FName) && ViewState["FileTab"] != null)
                    {
                        if (File.Exists(Server.MapPath(Imagepath + FName.ToString())))
                        {
                            File.Delete(Server.MapPath(Imagepath + FName.ToString()));
                        }
                    }
                    if (!string.IsNullOrEmpty(FName) && ViewState["FileHover"] != null)
                    {
                        if (File.Exists(Server.MapPath(Imagepath + FName.ToString().Replace(".jpg", "") + "_hover.jpg")))
                        {
                            File.Delete(Server.MapPath(Imagepath + FName.ToString() + "_hover.jpg"));
                        }
                    }

                    if (ImgTab.Src != null && ImgTab.Src.ToString() != "")
                    {
                        string strImageName = "";
                        strImageName = Request.QueryString["ID"].ToString() + ".jpg";
                        SaveImage(strImageName, Convert.ToInt32(Request.QueryString["ID"].ToString().Trim()));
                    }
                    if (ImgHover.Src != null && ImgHover.Src.ToString() != "")
                    {
                        SaveImageHover(Request.QueryString["ID"].ToString().Trim() + "_hover.jpg", Convert.ToInt32(Request.QueryString["ID"].ToString().Trim()));
                    }
                }

                hdnIsBase.Value = "0";
                Response.Redirect("/Admin/FeedManagement/ListFeedMaster.aspx?status=update");
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg2", "$(document).ready( function() {jAlert('Record Updated Successfully.', 'Message'); window.location.href='/Admin/FeedManagement/ListFeedMaster.aspx';});", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkSku", "$(document).ready( function() {jAlert('Feed with same name already exists. specify another FeedName', 'Message','ContentPlaceHolder1_txtFeedName');});", true);
                return;
            }
        }


        /// <summary>
        /// Inserts the feed.
        /// </summary>
        public void InsertFeed()
        {
            Int32 ChkName = 0;
            if (chkIsBase.Checked)
                ChkName = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select COUNT(ISNULL([IsBase],0)) as TotBase From tb_FeedMaster wHERE StoreId=" + Convert.ToInt32(ddlStore.SelectedValue) + " And isBase=1"));
            if (ChkName > 0 && hdnIsBase.Value == "0")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chnageBaseFeed", "confirmIsBase();", true);
                return;
            }
            tbFeedMaster = new tb_FeedMaster();
            tbFeedMaster.FeedName = txtFeedName.Text.ToString().Trim();
            tbFeedMaster.IsBase = chkIsBase.Checked;
            tbFeedMaster.CreatedOn = DateTime.Now;
            Int32 StoreId = Convert.ToInt32(ddlStore.SelectedValue);
            tbFeedMaster.tb_StoreReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(StoreId));
            FeedComponent FeedComponent = new FeedComponent();
            Int32 ResultFeedID = Convert.ToInt32(FeedComponent.InsertFeed(tbFeedMaster));
            if (ResultFeedID > 0)
            {
                if (hdnIsBase.Value == "1")
                {
                    CommonComponent.ExecuteCommonData("UPDATE tb_FeedMaster SET IsBase=0;UPDATE dbo.tb_FeedMaster SET IsBase=1 WHERE FeedID=" + ResultFeedID + ";");
                }
                try
                {
                    string strSql = "CREATE TABLE [dbo].[tb_FeedFieldValues_" + ResultFeedID.ToString() + "](";
                    strSql += "[ValueID] [int] IDENTITY(1,1) NOT NULL,";
                    strSql += "[FieldID] [int] NULL,";
                    strSql += "[FieldValue] [varchar](max) NULL,";
                    strSql += "	[ProductID] [int] NULL)";
                    CommonComponent.ExecuteCommonData(strSql);
                }
                catch { }

                if (ImgTab.Src != null && ImgTab.Src.ToString() != "")
                {
                    string strImageName = "";
                    strImageName = ResultFeedID.ToString() + ".jpg";
                    SaveImage(strImageName, ResultFeedID);
                    try
                    {
                        FileInfo fl = new FileInfo(ImgTab.Src.ToString());

                        if (File.Exists(Server.MapPath("/FeedManagement/images/Temp/" + fl.Name.ToString())))
                        {
                            File.Delete(Server.MapPath("/FeedManagement/images/Temp/" + fl.Name.ToString()));
                        }
                    }
                    catch { }

                }
                if (ImgHover.Src != null && ImgHover.Src.ToString() != "")
                {
                    SaveImageHover(ResultFeedID.ToString() + "_hover.jpg", Convert.ToInt32(ResultFeedID));
                    try
                    {
                        FileInfo fl = new FileInfo(ImgHover.Src.ToString());

                        if (File.Exists(Server.MapPath("/FeedManagement/images/Temp/" + fl.Name.ToString())))
                        {
                            File.Delete(Server.MapPath("/FeedManagement/images/Temp/" + fl.Name.ToString()));
                        }
                    }
                    catch { }
                }
                hdnIsBase.Value = "0";
                Response.Redirect("/Admin/FeedManagement/ListFeedMaster.aspx?status=insert");
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "@msg", "$(document).ready( function() {jAlert('Record Inserted Successfully.', 'Message'); window.location.href='/Admin/FeedManagement/ListFeedMaster.aspx';});", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "chkfeedInsert", "$(document).ready( function() {jAlert('Feed with same name already exists. specify another FeedName', 'Message','ContentPlaceHolder1_txtFeedName');});", true);
                return;
            }
        }

        /// <summary>
        ///  Save Image Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SaveImage(string FileName, Int32 ResultFeedID)
        {
            if (ImgTab.Src.ToLower().Contains("temp"))
            {
                try
                {
                    string strOrgImageName = Convert.ToString(ViewState["DelTagImage"]);
                    CreateImage("temp", FileName, ImgTab);
                    try
                    {
                        CommonComponent.ExecuteCommonData("Update tb_FeedMaster set ImageName='" + ResultFeedID.ToString() + ".jpg" + "' where FeedID=" + ResultFeedID.ToString());
                    }
                    catch { }
                }
                catch { }
            }
        }

        /// <summary>
        ///  Save Image Hover Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void SaveImageHover(string FileName, Int32 ResultFeedID)
        {
            if (ImgHover.Src.ToLower().Contains("temp"))
            {
                try
                {
                    string strOrgImageName = Convert.ToString(ViewState["DelTagImage"]);
                    CreateImage("temp", FileName, ImgHover);
                    try
                    {
                        CommonComponent.ExecuteCommonData("Update tb_FeedMaster set ImageName='" + ResultFeedID.ToString() + ".jpg" + "' where FeedID=" + ResultFeedID.ToString());
                    }
                    catch { }
                }
                catch { }
            }
        }

        /// <summary>
        /// Creates the Image
        /// </summary>
        /// <param name="Size">string Size</param>
        /// <param name="FileName">string FileName</param>
        /// <param name="imgSrc">string imgSrc</param>
        protected void CreateImage(string Size, string FileName, HtmlImage imgSrc)
        {
            string strFile = null;
            string FeedTempPath = string.Concat(AppLogic.AppConfigs("FeedImagePath"), "Temp/");
            string FeedImagePath = AppLogic.AppConfigs("FeedImagePath");
            String strPath = "";
            if (imgSrc.Src.ToString().IndexOf("?") > -1)
            {
                strPath = imgSrc.Src.Split('?')[0];
            }
            else
            {
                strPath = imgSrc.Src.ToString();
            }
            strFile = Server.MapPath(strPath);
            string strFilePath = "";
            Size = Size.ToLower();
            strFilePath = Server.MapPath(FeedImagePath + FileName);

            //if (Request.QueryString["Mode"] == "edit")
            //{
            //    if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
            //    {
            //        DeleteImage(FeedImagePath + ViewState["DelImage"].ToString());
            //    }
            //}

            ResizePhoto(strFile, Size, strFilePath);
            try
            {
                if (File.Exists(Server.MapPath(ImgTab.Src.ToString())))
                {
                    //ViewState["ImageTag"] = ImgTab.Src.ToString();
                    //File.Delete(Server.MapPath(ViewState["ImageTag"].ToString()));
                    File.Delete(Server.MapPath(ImgTab.Src.ToString()));
                }
                if (FileName.ToString().Trim().ToLower().Contains("hover"))
                {
                    if (File.Exists(Server.MapPath(ImgHover.Src.ToString())))
                    {
                        //ViewState["ImageTagHover"] = ImgHover.Src.ToString();
                        //File.Delete(Server.MapPath(ViewState["ImageTagHover"].ToString()));
                        File.Delete(Server.MapPath(ImgHover.Src.ToString()));
                    }
                }
            }
            catch { }
        }

        #region Resize Image

        /// <summary>
        /// Resizes the Photo
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="Size">string Size</param>
        /// <param name="strFilePath">string strFilePath</param>
        public void ResizePhoto(string strFile, string Size, string strFilePath)
        {
            if (Size == "large")
            {
                File.Copy(strFile, strFilePath, true);
                CommonOperations.SaveOnContentServer(strFilePath);
            }
            else
                ResizeImage(strFile, finWidth, finHeight, strFilePath);
        }


        /// <summary>
        /// Resizes the image.
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
        /// Generates the image.
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
                g.Dispose();
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
                g.Dispose();
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
                g.Dispose();
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
        /// Gets the Encoder Information
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

        #endregion

        #region Upload File to Temp Folder

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUploadTabImage_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            string ProductTempPath = string.Concat(AppLogic.AppConfigs("FeedImagePath"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (FlTabUpload.FileName.Length > 0 && Path.GetExtension(FlTabUpload.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (FlTabUpload.FileName.Length > 0)
                {
                    ViewState["FileTab"] = FlTabUpload.FileName.ToString();
                    FlTabUpload.SaveAs(Server.MapPath(ProductTempPath) + FlTabUpload.FileName);
                    ImgTab.Src = ProductTempPath + FlTabUpload.FileName;

                    System.Drawing.Image imgVisol = System.Drawing.Image.FromFile(Server.MapPath(ImgTab.Src));
                    int resizedHeight = imgVisol.Height;
                    int resizedWidth = imgVisol.Width;
                    finHeight = resizedHeight;
                    finWidth = resizedWidth;
                }
                else
                {
                    ViewState["FileTab"] = null;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed" + "', 'Message','ContentPlaceHolder1_fuProductIcon');});", true);
            }
        }

        /// <summary>
        ///  Upload Hover Image Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUploadHoverImage_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            string ProductTempPath = string.Concat(AppLogic.AppConfigs("FeedImagePath"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (FlHoverUpload.FileName.Length > 0 && Path.GetExtension(FlHoverUpload.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (FlHoverUpload.FileName.Length > 0)
                {
                    ViewState["FileHover"] = FlHoverUpload.FileName.ToString();
                    FlHoverUpload.SaveAs(Server.MapPath(ProductTempPath) + FlHoverUpload.FileName);
                    ImgHover.Src = ProductTempPath + FlHoverUpload.FileName;

                    System.Drawing.Image imgVisol = System.Drawing.Image.FromFile(Server.MapPath(ImgHover.Src));
                    int resizedHeight = imgVisol.Height;
                    int resizedWidth = imgVisol.Width;
                    finHeight = resizedHeight;
                    finWidth = resizedWidth;
                }
                else
                {
                    ViewState["FileHover"] = null;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed" + "', 'Message','ContentPlaceHolder1_fuProductIcon');});", true);
            }
        }

        #endregion

        /// <summary>
        /// Delete Image function
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImage(string ImageName)
        {
            if (File.Exists(Server.MapPath(ImageName)))
                File.Delete(Server.MapPath(ImageName));
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btncancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ListFeedMaster.aspx");
        }

        /// <summary>
        ///  Delete Tab Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnDeleteTab_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["DeleteTabImage"] = ImgTab.Src.ToString();
            ImgTab.Src = AppLogic.AppConfigs("FeedImagePath") + "image_not_available.jpg";
        }

        /// <summary>
        ///  Delete Hover Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnDeleteHover_Click(object sender, ImageClickEventArgs e)
        {
            ViewState["DeleteHoverImage"] = ImgHover.Src.ToString();
            ImgHover.Src = AppLogic.AppConfigs("FeedImagePath") + "image_not_available.jpg";
        }
    }
}