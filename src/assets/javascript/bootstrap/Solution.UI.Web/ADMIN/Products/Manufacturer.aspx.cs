using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
namespace Solution.UI.Web.ADMIN.Products
{

    /// <summary>
    /// Manufacturer Page For user to maintain data by UI
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>
    /// 
    public partial class Manufacturer : System.Web.UI.Page
    {
        static Size ThumbNailSizeIcon = Size.Empty;
        static Size ThumbNailSizeMicro = Size.Empty;
        static int finHeight;
        static int finWidth;
        static String BrandTempPath = string.Empty;
        static String BrandIconPath = string.Empty;
        static String BrandMicroPath = string.Empty;

        #region Declaration

        #region components
        RedTag_CCTV_Ecomm_DBEntities ctxRedtag = new RedTag_CCTV_Ecomm_DBEntities();
        StoreComponent objStorecomponent = new StoreComponent();
        ManufactureComponent objManufacturercomponent = new ManufactureComponent();
        #endregion

        #region Entities
        tb_Manufacture tbManufacturer = new tb_Manufacture();
        #endregion

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            txtDisplayorder.Attributes.Add("onkeypress", "return isNumberKey(event)");
            //clsvariables.LoadAllPath();
            if (!Page.IsPostBack)
            {
                bindstore();
                if (Request.QueryString["ProductStoreID"] != null)
                {
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(Request.QueryString["ProductStoreID"].ToString()));
                }

                if (Request.QueryString["Mode"] != null && Request.QueryString["ID"] != null && Request.QueryString["storeId"] != null)
                {

                    bindExistingDetailsOfManufacture();
                }
            }
            BindSizes();
            ibtnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
            ibtnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
            ibtnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
            ibtnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete.gif";
        }

        /// <summary>
        /// Binds the Existing Details of Manufacture
        /// </summary>
        private void bindExistingDetailsOfManufacture()
        {
            if (Request.QueryString["ID"] != null)
            {
                lblTitle.Text = "Add Manufacture";
                DataSet dsMan = ManufactureComponent.GetManufactureByManID(Convert.ToInt16(Request.QueryString["ID"]), Convert.ToInt16(Request.QueryString["storeId"]));
                if (dsMan != null && dsMan.Tables[0].Rows.Count > 0)
                {
                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(ddlStore.Items.FindByValue(dsMan.Tables[0].Rows[0]["StoreID"].ToString()));
                    txtname.Text = dsMan.Tables[0].Rows[0]["Name"].ToString();
                    txtDescription.InnerText = dsMan.Tables[0].Rows[0]["Description"].ToString();
                    if (dsMan.Tables[0].Rows[0]["DisplayOrder"].ToString() != null)
                        txtDisplayorder.Text = dsMan.Tables[0].Rows[0]["DisplayOrder"].ToString();
                    else
                        txtDisplayorder.Text = "0";

                    txtSedescription.InnerText = dsMan.Tables[0].Rows[0]["SEDescription"].ToString();
                    txtsekeywords.InnerText = dsMan.Tables[0].Rows[0]["SEKeywords"].ToString();
                    bool status = Convert.ToBoolean(dsMan.Tables[0].Rows[0]["Active"].ToString());
                    if (status == true)
                    {
                        chkStatus.Checked = true;
                    }
                    if (dsMan.Tables[0].Rows[0]["ImageName"].ToString() != "")
                    {
                        BindImage(dsMan.Tables[0].Rows[0]["ImageName"].ToString());
                        ViewState["imageName"] = dsMan.Tables[0].Rows[0]["ImageName"].ToString();
                    }
                }
            }
            else
            {

                lblTitle.Text = "Update Manufacture";
            }
        }

        /// <summary>
        /// Binds the Image
        /// </summary>
        /// <param name="imageName">string imageName</param>
        private void BindImage(string imageName)
        {
            BrandTempPath = string.Concat(AppLogic.AppConfigs("ImagePathBrand"), "Temp/");
            BrandIconPath = string.Concat(AppLogic.AppConfigs("ImagePathBrand"), "Icon/");
            BrandMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathBrand"), "Micro/");
            imageName = BrandIconPath + imageName;
            if (File.Exists(Server.MapPath(imageName)))
            {
                imgBrand.Src = imageName;
                ibtnDelete.Visible = true;

            }
            else
            {
                imgBrand.Src = string.Concat(BrandIconPath, "image_not_available.jpg");
                ibtnDelete.Visible = false;
            }
        }

        /// <summary>
        /// Binds the Image Sizes
        /// </summary>
        private void BindSizes()
        {
            try
            {
                //ThumbNailSizeIcon = new Size(Convert.ToInt32((string.IsNullOrEmpty(AppLogic.AppConfigs("ProductIconWidth").ToString())) ? "0" : AppLogic.AppConfigs("ProductIconWidth").ToString()), Convert.ToInt32((string.IsNullOrEmpty(AppLogic.AppConfigs("ProductIconHeight").ToString())) ? "0" : AppLogic.AppConfigs("ProductIconHeight").ToString()));
                //ThumbNailSizeMicro = new Size(Convert.ToInt32((string.IsNullOrEmpty(AppLogic.AppConfigs("ProductMicroWidth").ToString())) ? "0" : AppLogic.AppConfigs("ProductMicroWidth").ToString()), Convert.ToInt32((string.IsNullOrEmpty(AppLogic.AppConfigs("ProductMicroHeight").ToString())) ? "0" : AppLogic.AppConfigs("ProductMicroHeight").ToString()));
                ThumbNailSizeIcon = new Size(Convert.ToInt32((string.IsNullOrEmpty("100")) ? "0" : "100"), Convert.ToInt32((string.IsNullOrEmpty("100")) ? "0" : "100"));
                ThumbNailSizeMicro = new Size(Convert.ToInt32((string.IsNullOrEmpty("100")) ? "0" : "100"), Convert.ToInt32((string.IsNullOrEmpty("100")) ? "0" : "100"));

            }
            catch { }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            ddlStore.Items.Insert(0, "- Select Store -");
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (ddlStore.SelectedItem.Text != "- Select Store -" && txtname.Text.Trim() != "")
            {

                if (Request.QueryString["ID"] != null)
                {
                    Update();
                }
                else
                {
                    #region insert
                    #region check whether name is exist or not
                    Int32 StoreId = 1;
                    if (ddlStore.SelectedValue == "0" || ddlStore.SelectedIndex == 0)
                    {
                        StoreId = 1;
                    }
                    else { StoreId = Convert.ToInt32(ddlStore.SelectedValue.ToString()); }

                    int count = ManufactureComponent.CheckManufactureName(txtname.Text.Trim(), Convert.ToInt32(StoreId));
                    if (count > 0)
                    {
                        lblMsg.Text = "Manufacture Name Already Exists!";
                    }
                    else
                    {
                        lblMsg.Text = "";
                        tbManufacturer.Name = txtname.Text.Trim();
                        string seName = CommonOperations.RemoveSpecialCharacter(txtname.Text.Trim().ToCharArray());
                        tbManufacturer.Description = txtDescription.InnerText.Trim();
                        tbManufacturer.SEKeywords = txtsekeywords.InnerText.Trim();
                        tbManufacturer.SEDescription = txtSedescription.InnerText.Trim();
                        tbManufacturer.SEName = seName;
                        if (txtDisplayorder.Text != null && txtDisplayorder.Text != "")
                            tbManufacturer.DisplayOrder = Convert.ToInt16(txtDisplayorder.Text.Trim());
                        tbManufacturer.CreatedOn = DateTime.UtcNow;
                        tbManufacturer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(ddlStore.SelectedValue));
                        if (chkStatus.Checked == true)
                        {
                            tbManufacturer.Active = true;
                        }
                        else
                        {
                            tbManufacturer.Active = false;
                        }
                        tbManufacturer.Deleted = false;
                        if (ViewState["File"] != null)
                        {
                            tbManufacturer.ImageName = ViewState["File"].ToString();
                            SaveImage(ViewState["File"].ToString());
                        }
                        ManufactureComponent.CreateManufacturer(tbManufacturer);

                        clearSelection();
                        if (Request.QueryString["ProductStoreID"] != null && Request.QueryString["ProductID"] != null)
                        {
                            Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit");
                        }
                        else if (Request.QueryString["ProductStoreID"] != null)
                        {
                            Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "");
                        }
                        else
                        {
                            Response.Redirect("ListManufacturer.aspx");
                        }
                    }
                    #endregion
                    #endregion
                }
            }
        }

        /// <summary>
        ///Update Method for Manufacturer
        /// </summary>
        private void Update()
        {
            tbManufacturer.ManufactureID = Convert.ToInt16(Request.QueryString["ID"]);
            tbManufacturer.Name = txtname.Text;
            tbManufacturer.Description = txtDescription.InnerText;
            tbManufacturer.Deleted = false;
            if (chkStatus.Checked == true)
            {
                tbManufacturer.Active = true;
            }
            else
            {
                tbManufacturer.Active = false;
            }
            tbManufacturer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Manufacture", "StoreID", Convert.ToInt32(ddlStore.SelectedValue));
            string seName = CommonOperations.RemoveSpecialCharacter(txtname.Text.Trim().ToCharArray());
            tbManufacturer.SEName = seName;
            tbManufacturer.SEDescription = txtSedescription.InnerText.Trim();
            tbManufacturer.SEKeywords = txtsekeywords.InnerText.Trim();
            if (txtDisplayorder.Text.Trim() != "" && txtDisplayorder.Text.Trim().Length > 0)
            {
                tbManufacturer.DisplayOrder = Convert.ToInt16(txtDisplayorder.Text.Trim());
            }
            tbManufacturer.UpdatedOn = DateTime.UtcNow;

            if (ViewState["File"] != null)
            {
                SaveImage(ViewState["File"].ToString());
                tbManufacturer.ImageName = ViewState["File"].ToString();
            }
            else
            {
                if (!imgBrand.Src.ToString().Contains("image_not_available"))
                {
                    if (ViewState["imageName"] != null)
                    {
                        string imageName = ViewState["imageName"].ToString();
                        SaveImage(imageName);
                        tbManufacturer.ImageName = imageName;
                    }
                }
            }
            ManufactureComponent.Update(tbManufacturer);
            Response.Redirect("ListManufacturer.aspx");
        }

        /// <summary>
        /// Saves the Manufacture Image
        /// </summary>
        /// <param name="FileName">string FileName</param>
        protected void SaveImage(string FileName)
        {

            //  clsvariables.LoadAllPath();
            BrandTempPath = string.Concat(AppLogic.AppConfigs("ImagePathBrand"), "Temp/");
            BrandIconPath = string.Concat(AppLogic.AppConfigs("ImagePathBrand"), "Icon/");
            BrandMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathBrand"), "Micro/");

            if (!Directory.Exists(Server.MapPath(BrandTempPath)))
                Directory.CreateDirectory(Server.MapPath(BrandTempPath));

            if (!Directory.Exists(Server.MapPath(BrandIconPath)))
                Directory.CreateDirectory(Server.MapPath(BrandIconPath));

            if (!Directory.Exists(Server.MapPath(BrandMicroPath)))
                Directory.CreateDirectory(Server.MapPath(BrandMicroPath));

            if (imgBrand.Src.Contains(BrandTempPath))
            {
                try
                {
                    CreateImage("icon", FileName);
                    CreateImage("micro", FileName);
                    DeleteImage(FileName);
                }
                catch (Exception ex)
                {
                    lblMsg.Text += "<br />" + ex.Message;
                }
            }
        }

        /// <summary>
        /// Creates the Manufacture Image
        /// </summary>
        /// <param name="Size">string Size</param>
        /// <param name="FileName">string FileName</param>
        protected void CreateImage(string Size, string FileName)
        {
            try
            {
                string strFile = null;
                strFile = Server.MapPath(imgBrand.Src);
                string strFilePath = "";
                Size = Size.ToLower();
                switch (Size)
                {
                    case "icon":
                        strFilePath = Server.MapPath(BrandIconPath + FileName);
                        if (Request.QueryString["Mode"] == "Edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(Solution.Bussines.Components.AdminCommon.clsvariables.PathBIconImage() + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "micro":
                        strFilePath = Server.MapPath(BrandMicroPath + FileName);
                        if (Request.QueryString["Mode"] == "Edit")
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(Solution.Bussines.Components.AdminCommon.clsvariables.PathCMicroImage() + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                }
                ResizePhoto2(strFile, Size, strFilePath);
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
        /// Resizes the photo of Manufacture
        /// </summary>
        /// <param name="strFile">string strFile</param>
        /// <param name="Size">string Size</param>
        /// <param name="strFilePath">string strFilePath</param>
        public static void ResizePhoto2(string strFile, string Size, string strFilePath)
        {

            System.Drawing.Image imgVisol = System.Drawing.Image.FromFile(strFile);
            float resizePercent = 0;
            int resizedHeight = imgVisol.Height;
            int resizedWidth = imgVisol.Width;


            switch (Size)
            {
                case "icon":
                    finHeight = ThumbNailSizeIcon.Height;
                    finWidth = ThumbNailSizeIcon.Width;
                    break;
                case "micro":
                    finHeight = ThumbNailSizeMicro.Height;
                    finWidth = ThumbNailSizeMicro.Width;
                    break;
            }

            if (imgVisol.Height >= finHeight && imgVisol.Width >= finWidth)
            {
                float resizePercentHeight = 0;
                float resizePercentWidth = 0;
                resizePercentHeight = (finHeight * 100) / imgVisol.Height;
                resizePercentWidth = (finWidth * 100) / imgVisol.Width;
                if (resizePercentHeight < resizePercentWidth)
                {
                    resizedHeight = finHeight;
                    resizedWidth = (int)resizePercentHeight * imgVisol.Width / 100;
                }
                if (resizePercentHeight >= resizePercentWidth)
                {
                    resizedWidth = finWidth;
                    resizedHeight = (int)resizePercentWidth * imgVisol.Height / 100;
                }
            }
            else if (imgVisol.Width >= finWidth && imgVisol.Height <= finHeight)
            {
                resizedWidth = finWidth;
                resizePercent = (finWidth * 100) / imgVisol.Width;
                resizedHeight = (int)(imgVisol.Height * resizePercent) / 100;
            }

            else if (imgVisol.Width <= finWidth && imgVisol.Height >= finHeight)
            {
                resizePercent = (finHeight * 100) / imgVisol.Height;
                resizedHeight = finHeight;
                resizedWidth = (int)resizePercent * imgVisol.Width / 100;
            }


            Bitmap resizedPhoto = new Bitmap(resizedWidth, resizedHeight, PixelFormat.Format24bppRgb);
            resizedPhoto.SetResolution(imgVisol.HorizontalResolution, imgVisol.VerticalResolution);
            Graphics grPhoto = Graphics.FromImage(resizedPhoto);
            int resizedQuality = 100;


            // Encoder parameter for image quality 
            EncoderParameter qualityParam = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, resizedQuality);
            string img_ContentType = "";
            //if (img_ContentType == "image/gif")
            img_ContentType = "image/" + Path.GetExtension(strFile) + "";
            // Image codec 
            ImageCodecInfo imgCodec = GetEncoderInfo(img_ContentType);

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;
            int destWidth = resizedWidth;
            int destHeight = resizedHeight;
            int sourceWidth = imgVisol.Width;
            int sourceHeight = imgVisol.Height;
            grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
            Rectangle DestRect = new Rectangle(0, 0, destWidth, destHeight);
            Rectangle srcRect = new Rectangle(4, 4, sourceWidth - 5, sourceHeight - 5);

            grPhoto.DrawImage(imgVisol, DestRect, srcRect, GraphicsUnit.Pixel);

            CommonOperations.AdjustExtraSpaceInImage(resizedPhoto, strFilePath, finWidth, finHeight);

            resizedPhoto.Dispose();
            grPhoto.Dispose();
            imgVisol.Dispose();
        }

        /// <summary>
        /// Gets the Encoder Information
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
        /// Deletes Manufacture Image
        /// </summary>
        /// <param name="ImageName">string ImageName</param>
        protected void DeleteImage(string ImageName)
        {
            try
            {
                if (File.Exists(Server.MapPath(BrandTempPath) + ImageName))
                    File.Delete(Server.MapPath(BrandTempPath) + ImageName);
            }
            catch { }

        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            clearSelection();
            // Response.Redirect("ListManufacturer.aspx");
            if (Request.QueryString["ProductStoreID"] != null && Request.QueryString["ProductID"] != null)
            {
                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit");
            }
            else if (Request.QueryString["ProductStoreID"] != null)
            {
                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "");
            }
            else
            {
                Response.Redirect("ListManufacturer.aspx");
            }
        }

        /// <summary>
        /// Clears All Controls Selection
        /// </summary>
        private void clearSelection()
        {
            txtDescription.InnerText = "";
            txtDisplayorder.Text = "";
            txtname.Text = "";
            txtSedescription.InnerText = "";
            txtsekeywords.InnerText = "";

            ddlStore.ClearSelection();
        }

        /// <summary>
        ///  Update Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnUpload_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            //clsvariables.LoadAllPath();
            BrandTempPath = string.Concat(AppLogic.AppConfigs("ImagePathBrand"), "Temp/");
            if (!Directory.Exists(Server.MapPath(BrandTempPath)))
                Directory.CreateDirectory(Server.MapPath(BrandTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (fuBanner.FileName.Length > 0 && Path.GetExtension(fuBanner.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;


            if (Flag)
            {

                if (fuBanner.FileName.Length > 0)
                {
                    ViewState["File"] = fuBanner.FileName.ToString();
                    fuBanner.SaveAs(Server.MapPath(BrandTempPath) + fuBanner.FileName);
                    imgBrand.Src = BrandTempPath + fuBanner.FileName;
                    lblMsg.Text = "";
                    ibtnDelete.Visible = true;
                }
            }
            else
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";

        }

        /// <summary>
        ///  Delete Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ibtnDelete_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                string ImageName = "";
                if (ViewState["DelImage"] != null)
                    ImageName = ViewState["DelImage"].ToString();
                try
                {
                    if (File.Exists(Server.MapPath(BrandIconPath + ImageName)))
                        File.Delete(Server.MapPath(BrandIconPath + ImageName));
                    if (File.Exists(Server.MapPath(BrandMicroPath + ImageName)))
                        File.Delete(Server.MapPath(BrandMicroPath + ImageName));

                    CommonOperations.DeleteFileFromContentServer(BrandIconPath + ImageName);
                    CommonOperations.DeleteFileFromContentServer(BrandMicroPath + ImageName);
                }
                catch (Exception ex)
                {
                    CommonComponent.ErrorLog("Manufacturer.aspx", ex.Message, ex.StackTrace);
                }
                ViewState["DelImage"] = null;
                Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
                string strFilePath = imgBrand.Src.ToString();
                imgBrand.Src = Solution.Bussines.Components.AdminCommon.clsvariables.PathBIconImageNotAvailable();
                try
                {
                    if (strFilePath.ToString().IndexOf("image_not_available.gif") < -1)
                    {
                        File.Delete(Server.MapPath(strFilePath.ToString()));
                        CommonOperations.DeleteFileFromContentServer(Server.MapPath(strFilePath.ToString()));
                    }
                }
                catch
                {
                }
                ibtnDelete.Visible = false;
            }
            catch { }
        }
    }
}