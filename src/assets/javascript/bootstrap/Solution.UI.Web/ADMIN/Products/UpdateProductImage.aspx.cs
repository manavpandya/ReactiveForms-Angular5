using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Collections;
using Solution.Bussines.Components.AdminCommon;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Solution.Data;
namespace Solution.UI.Web.ADMIN.Products
{
    public partial class UpdateProductImage : BasePage
    {

        private int StoreID = 1;

        tb_Product tb_Product = null;
        tb_ProductCategory tbProductCategory = null;

        public static string ProductTempPath = string.Empty;
        public static string ProductIconPath = string.Empty;
        public static string ProductMediumPath = string.Empty;
        public static string ProductLargePath = string.Empty;
        public static string ProductMicroPath = string.Empty;

        static public int productID = 0;
        static int finHeight;
        static int finWidth;
        static Size thumbNailSizeLarge = Size.Empty;
        static Size thumbNailSizeMediam = Size.Empty;
        static Size thumbNailSizeIcon = Size.Empty;
        static Size thumbNailSizeMicro = Size.Empty;
        ConfigurationComponent objConfiguration = new ConfigurationComponent();
        String StrVendorIDs = String.Empty;
        String StrVendorPriority = String.Empty;
        DataSet dsVendorids = new DataSet();
        public static DataSet dsVendor;
        int InventoryTotal = 0;
        DataSet dsWarehouse = new DataSet();
        //int pProductID = 0;
        int CurrentStoreID = 0;
        int OldStoreID = 0;
        public bool chkUPC = false;
        string StrUPC = string.Empty;
        string StrUPCReal = string.Empty;

        public static string Vendorsku = "";
        public static string Productsku = "";

        public static DataTable dt;
        DataColumn dcDopshipper;
        DataColumn dcSKU;
        DataColumn dcPriority;
        DataColumn dcVendorSKUID;
        DataColumn dcVendorID;
        DataColumn dcDopshipperName;

        public static DataTable dtAssembler;
        DataColumn dcProductName;
        DataColumn dcProductSKU;
        DataColumn dcQuantity;
        DataColumn dcProductID;
        protected void Page_Load(object sender, EventArgs e)
        {
            BindSize();
            if(!IsPostBack)
            {
                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Micro/");
                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                  
                imgbtnAlt1.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                imgbtnAlt2.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                imgbtnAlt3.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                imgbtnAlt4.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                imgbtnAlt5.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";

                imgbtnAlt1del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                imgbtnAlt2del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                imgbtnAlt3del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                imgbtnAlt4del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                imgbtnAlt5del.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";
                btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delet.gif";

               

                if(Request.QueryString["StoreID"]!=null && Request.QueryString["ProductID"]!=null)
                {
                    binddata(Convert.ToInt32(Request.QueryString["StoreID"].ToString()), Convert.ToInt32(Request.QueryString["ProductID"].ToString()));
                }
                
            }

            btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";

            btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
        }
        /// <summary>
        /// Bind Sizes
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
        /// Save button Click event for Add or Update Product
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {


            if (Request.QueryString["StoreID"] != null)
            {
                
                    UpdateProduct(Convert.ToInt32(Request.QueryString["ProductID"]), Convert.ToInt32(Request.QueryString["StoreID"]));
                    ScriptManager.RegisterClientScriptBlock(btnSave, btnSave.GetType(), "pagerefresh2", "window.opener.location.href=window.opener.location.href;window.close();", true);
                
               
            }
        }



        /// <summary>
        /// Function for Update Product
        /// </summary>
        /// <param name="ProductID">in ProductID</param>
        /// <param name="StoreId">int StoreId</param>
        public void UpdateProduct(int ProductID, int StoreId)
        {
            ProductComponent objProduct = new ProductComponent();
            tb_Product = new tb_Product();
            tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);
          //  tb_Product = SetValue(tb_Product);

           

               

                    #region Save and Update Image
                    string strImageName = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(tb_Product.SKU.ToString().Trim()))
                        {

                            string Imgname = "";

                            if (!string.IsNullOrEmpty(tb_Product.ImageName))
                            {
                                Imgname = Convert.ToString(tb_Product.ImageName.ToString());
                                strImageName = Imgname;
                            }
                            else
                            {
                                strImageName = RemoveSpecialCharacter(tb_Product.SKU.ToString().ToCharArray()) + "_" + ProductID + ".jpg";
                            }
                            if (ImgLarge.Src.ToLower().IndexOf("/temp/") > -1)
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


                            string strdelImage = "";
                            string strImageNameNew = strImageName.Replace(".jpg", "");
                            if (ImgAlt1.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File1"] != null)
                            {
                                if (ViewState["DelImage2"] != null)
                                {
                                    strdelImage = ViewState["DelImage"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_1.jpg", ViewState["File1"].ToString(), ImgAlt1, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=1 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt1.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_1.jpg' Where ImageNo=1 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt1.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",1,'" + txtalt1.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_1.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",1,'','" + strImageNameNew + "_1.jpg')");
                                }
                            }
                            if (ImgAlt2.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File2"] != null)
                            {
                                if (ViewState["DelImage2"] != null)
                                {
                                    strdelImage = ViewState["DelImage2"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_2.jpg", ViewState["File2"].ToString(), ImgAlt2, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=2 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt2.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_2.jpg' Where ImageNo=2 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt2.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",2,'" + txtalt2.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_2.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",2,'','" + strImageNameNew + "_2.jpg')");
                                }
                            }
                            if (ImgAlt3.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File3"] != null)
                            {
                                if (ViewState["DelImage3"] != null)
                                {
                                    strdelImage = ViewState["DelImage3"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_3.jpg", ViewState["File3"].ToString(), ImgAlt3, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=3 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt3.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_3.jpg' Where ImageNo=3 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt3.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",3,'" + txtalt3.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_3.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",3,'','" + strImageNameNew + "_3.jpg')");
                                }
                            }
                            if (ImgAlt4.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File4"] != null)
                            {
                                if (ViewState["DelImage4"] != null)
                                {
                                    strdelImage = ViewState["DelImage4"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_4.jpg", ViewState["File4"].ToString(), ImgAlt4, strdelImage);


                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=4 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt4.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_4.jpg' Where ImageNo= 4 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt4.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",4,'" + txtalt4.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_4.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",4,'','" + strImageNameNew + "_4.jpg')");
                                }

                            }
                            if (ImgAlt5.Src.ToLower().IndexOf("/temp/") > -1 && ViewState["File5"] != null)
                            {
                                if (ViewState["DelImage5"] != null)
                                {
                                    strdelImage = ViewState["DelImage5"].ToString();
                                }
                                else
                                {
                                    strdelImage = "";
                                }
                                SaveImageAlt(strImageNameNew + "_5.jpg", ViewState["File5"].ToString(), ImgAlt5, strdelImage);
                                if (Convert.ToInt32(CommonComponent.GetScalarCommonData("Select Count(*) from tb_ProductImgDesc Where ImageNo=5 and ProductId=" + ProductID + "")) > 0)
                                {
                                    CommonComponent.ExecuteCommonData("Update tb_ProductImgDesc set Description='" + txtalt5.Text.ToString().Replace("'", "''") + "',Imagename='" + strImageNameNew + "_5.jpg' Where ImageNo=5 and ProductId=" + ProductID + "");
                                }
                                else
                                {
                                    if (!string.IsNullOrEmpty(txtalt5.Text.ToString()))
                                        CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",5,'" + txtalt5.Text.ToString().Replace("'", "''") + "','" + strImageNameNew + "_5.jpg')");
                                    else CommonComponent.ExecuteCommonData("Insert into tb_ProductImgDesc (ProductId,ImageNo,Description,Imagename) Values(" + ProductID + ",5,'','" + strImageNameNew + "_5.jpg')");
                                }
                            }
                        }

                        tb_Product = new tb_Product();
                        tb_Product = objProduct.GetAllProductDetailsbyProductID(ProductID);

                        if (string.IsNullOrEmpty(ImgLarge.Src.ToString()) || ImgLarge.Src.ToString().ToLower().IndexOf("image_not_available") > -1)
                            tb_Product.ImageName = "";
                        else
                        {
                            if (ViewState["File"] != null && ImgLarge.Src.ToLower().IndexOf("/temp/") <= -1)
                            {
                                tb_Product.ImageName = ViewState["File"].ToString();
                            }
                            else
                            {
                                tb_Product.ImageName = strImageName;
                            }
                        }

                    }
                    catch { }

                    #endregion

                  

                   ProductComponent.UpdateProduct(tb_Product);
                   if (Request.QueryString["StoreID"] != null && Request.QueryString["ProductID"] != null)
                   {
                       binddata(Convert.ToInt32(Request.QueryString["StoreID"].ToString()), Convert.ToInt32(Request.QueryString["ProductID"].ToString()));
                   }
                 
                  
                }
            
        

        /// <summary>
        /// Cancel button Click event for Redirect to Product List Page
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (ImgLarge.Src.Trim() != "" && ImgLarge.Src.Trim().ToLower().IndexOf("/product/temp/") > -1)
                {
                    FileInfo flinfo = new FileInfo(Server.MapPath(ImgLarge.Src.Trim()));
                    if (flinfo.Exists)
                    {
                        flinfo.Delete();
                    }
                }
            }
            catch { }
            ViewState["DelImage"] = null;
            ScriptManager.RegisterClientScriptBlock(btnCancel, btnCancel.GetType(), "pagerefresh", "window.opener.location.href=window.opener.location.href;window.close();", true);
           // Response.Redirect("BulkImageUpload.aspx");
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

      public void   binddata(Int32 StoreID, Int32 ID)
        {
             DataSet DsProduct = new DataSet();
            DsProduct = ProductComponent.GetProductByProductID(ID);
            Random rm = new Random();
            ImgAlt1.Src = ProductMediumPath + "image_not_available.jpg?" + rm.Next(10000).ToString();
            ImgAlt2.Src = ProductMediumPath + "image_not_available.jpg?" + rm.Next(10000).ToString();
            ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg?" + rm.Next(10000).ToString();
            ImgAlt4.Src = ProductMediumPath + "image_not_available.jpg?" + rm.Next(10000).ToString();
            ImgAlt5.Src = ProductMediumPath + "image_not_available.jpg?" + rm.Next(10000).ToString();
            if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
            {
               #region Get Image

                string Imagename = DsProduct.Tables[0].Rows[0]["Imagename"].ToString();

                String strImageName = RemoveSpecialCharacter(Convert.ToString(DsProduct.Tables[0].Rows[0]["SKU"]).ToCharArray()) + "_" + ID.ToString() + ".jpg";

                if (Imagename.ToString().Trim().ToLower().IndexOf("http") > -1)
                {
                    System.Net.WebClient objClient = new System.Net.WebClient();
                    String strSavedImgPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/") + strImageName.ToString();
                    objClient.DownloadFile(Imagename.ToString(), Server.MapPath(strSavedImgPath));
                    if (File.Exists(Server.MapPath(strSavedImgPath)))
                    {
                        ImgLarge.Src = strSavedImgPath.ToString();
                        System.Drawing.Image objimg = System.Drawing.Image.FromFile(Server.MapPath(strSavedImgPath));
                        txtIconHeigth.Text = objimg.Height.ToString();
                        txtIconWidth.Text = objimg.Width.ToString();
                        ViewState["File"] = strImageName.ToString();
                    }
                    else
                    {
                        ImgLarge.Src = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Large/image_not_available.jpg");
                    }
                }
                else
                {
                    ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/");
                    if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                        Directory.CreateDirectory(Server.MapPath(ProductMediumPath));

                    string strFilePath = Server.MapPath(ProductMediumPath + Imagename);
                    string strIconPath = Server.MapPath(ProductMediumPath.Replace("Medium", "Icon") + Imagename);

                    btnDelete.Visible = false;
                    if (Request.QueryString["CloneID"] == null)
                    {
                        if (File.Exists(strFilePath))
                        {
                            ViewState["DelImage"] = Imagename;
                            btnDelete.Visible = true;
                            ImgLarge.Src = ProductMediumPath + Imagename;
                            System.Drawing.Image objimg = System.Drawing.Image.FromFile(strIconPath.Trim());
                            txtIconHeigth.Text = objimg.Height.ToString();
                            txtIconWidth.Text = objimg.Width.ToString();
                        }
                        else
                        {
                            ViewState["DelImage"] = null;
                            btnDelete.Visible = false;
                            ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                        }

                        DataSet dsImgdesc = new DataSet();
                      
                        dsImgdesc = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductImgDesc WHERE ProductId=" + DsProduct.Tables[0].Rows[0]["ProductId"].ToString() + "");
                        if (dsImgdesc != null && dsImgdesc.Tables.Count > 0 && dsImgdesc.Tables[0].Rows.Count > 0)
                        {
                            Random rd = new Random();
                            
                            for (int d = 0; d < dsImgdesc.Tables[0].Rows.Count; d++)
                            {
                                if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "1")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt1.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        
                                        imgbtnAlt1del.Visible = true;
                                        ViewState["DelImage1"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt1del.Visible = false;
                                        ImgAlt1.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt1.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }

                                }

                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "2")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt2.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt2del.Visible = true;
                                        ViewState["DelImage2"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt2del.Visible = false;
                                        ImgAlt2.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt2.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();
                                       
                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "3")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt3.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt3del.Visible = true;
                                        ViewState["DelImage3"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt3del.Visible = false;
                                        ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt3.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "4")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt4.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt4del.Visible = true;
                                        ViewState["DelImage4"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt4del.Visible = false;
                                        ImgAlt4.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt4.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "5")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        ImgAlt5.Src = ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString() + "?" + rd.Next(10000).ToString();
                                        imgbtnAlt5del.Visible = true;
                                        ViewState["DelImage5"] = dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString();
                                    }
                                    else
                                    {
                                        imgbtnAlt5del.Visible = false;
                                        ImgAlt5.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt5.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                            }

                        }

                    }
                    else
                    {
                        //AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                        string strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/") + Imagename);
                        if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk"))))
                        {
                            Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk")));
                        }
                        if (File.Exists(strFilePathOld.Replace("/Medium/", "/Large/")))
                        {
                            FileInfo flOld = new FileInfo(strFilePathOld.ToString().Replace("/Medium/", "/Large/"));

                            FileInfo fl = new FileInfo(strFilePath.ToString().Replace("/Medium/", "/Large/"));
                            ViewState["File"] = fl.Name.ToString();
                            AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreId"].ToString());
                            Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                            if (!Directory.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk") + "/Temp")))
                            {
                                Directory.CreateDirectory(Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk") + "/Temp"));
                            }
                            File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString()), true);
                            ImgLarge.Src = AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString();
                            lblMsg.Text = "";
                        }
                        else
                        {
                            ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
                            ViewState["File"] = null;
                        }
                        DataSet dsImgdesc = new DataSet();
                        dsImgdesc = CommonComponent.GetCommonDataSet("SELECT * FROM tb_ProductImgDesc WHERE ProductId=" + DsProduct.Tables[0].Rows[0]["ProductId"].ToString() + "");
                        if (dsImgdesc != null && dsImgdesc.Tables.Count > 0 && dsImgdesc.Tables[0].Rows.Count > 0)
                        {
                            Random rd = new Random();
                            for (int d = 0; d < dsImgdesc.Tables[0].Rows.Count; d++)
                            {
                                if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "1")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath.Replace("/Medium/", "/Large/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                      //  AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                       
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt1.Src = AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString();
                                        ViewState["File1"] = fl.Name.ToString();
                                    }
                                    else
                                    {
                                        
                                        ImgAlt1.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt1.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }

                                }

                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "2")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                                      //  AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                      
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt2.Src = AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString();
                                        ViewState["File2"] = fl.Name.ToString();
                                    }
                                    else
                                    {
                                         
                                        ImgAlt2.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt2.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();
                                        
                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "3")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                       // AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                     
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt3.Src = AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString();
                                        ViewState["File3"] = fl.Name.ToString();
                                        
                                    }
                                    else
                                    {
                                        
                                        ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt3.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();
                                        ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "4")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                      //  AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                       
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt4.Src = AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString();
                                        ViewState["File4"] = fl.Name.ToString();
                                        
                                    }
                                    else
                                    {
                                        
                                        ImgAlt4.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt4.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                                else if (dsImgdesc.Tables[0].Rows[d]["ImageNo"].ToString() == "5")
                                {
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString()) && File.Exists(Server.MapPath(ProductMediumPath + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString())))
                                    {
                                      //  AppConfig.StoreID = Convert.ToInt32(Request.QueryString["CloneID"].ToString());
                                        AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        strFilePathOld = Server.MapPath(string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/") + dsImgdesc.Tables[0].Rows[d]["Imagename"].ToString());
                                        FileInfo fl = new FileInfo(strFilePathOld);
                                      
                                        Solution.Bussines.Components.AdminCommon.clsvariables.LoadAllPath();
                                        File.Copy(strFilePathOld.Replace("/Medium/", "/Large/"), Server.MapPath(AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString()), true);
                                        ImgAlt5.Src = AppLogic.AppConfigs("ImagePathProductBulk") + "Temp/" + fl.Name.ToString();
                                        ViewState["File5"] = fl.Name.ToString();
                                      
                                    }
                                    else
                                    {
                                        
                                        ImgAlt5.Src = ProductMediumPath + "image_not_available.jpg?" + rd.Next(10000).ToString();
                                    }
                                    if (!string.IsNullOrEmpty(dsImgdesc.Tables[0].Rows[d]["Description"].ToString()))
                                    {
                                        txtalt5.Text = dsImgdesc.Tables[0].Rows[d]["Description"].ToString();

                                    }
                                }
                            }

                        }
                    }
                }
            } 
               #endregion
        }

        /// <summary>
        /// Save Image
        /// </summary>
        /// <param name="FileName">string FileName</param>
        protected void SaveImage(string FileName)
        {
            if ((Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString())))
            {
                AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
            }

            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
            ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Icon/");
            ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/");
            ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Large/");
            ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Micro/");

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
                    CreateImage("Medium", FileName);
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
                  //  DeleteTempFile("icon");
                    if ((ViewState["File1"] != null && ViewState["File"].ToString().Equals(ViewState["File1"].ToString())) || (ViewState["File2"] != null && ViewState["File"].ToString().Equals(ViewState["File2"].ToString())) || (ViewState["File3"] != null && ViewState["File"].ToString().Equals(ViewState["File3"].ToString())) || (ViewState["File4"] != null && ViewState["File"].ToString().Equals(ViewState["File4"].ToString())) || (ViewState["File5"] != null && ViewState["File"].ToString().Equals(ViewState["File5"].ToString())))
                    {

                    }
                    else
                    {
                        DeleteTempFile("icon");
                    }
                }
            }
        }
        protected void SaveImageAlt(string FileName, string tempFile, System.Web.UI.HtmlControls.HtmlImage img1option, string delfile)
        {
            if ((Request.QueryString["StoreID"] != null && !string.IsNullOrEmpty(Request.QueryString["StoreID"].ToString())))
            {
                AppConfig.StoreID = Convert.ToInt32(Request.QueryString["StoreID"].ToString());
            }

            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
            ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Icon/");
            ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Medium/");
            ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Large/");
            ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Micro/");

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

            if (img1option.Src.Contains(ProductTempPath))
            {
                try
                {
                    CreateImageAlt("Medium", FileName, img1option, delfile);
                    CreateImageAlt("Icon", FileName, img1option, delfile);
                    CreateImageAlt("Micro", FileName, img1option, delfile);
                    CreateImageAlt("Large", FileName, img1option, delfile);
                }
                catch (Exception ex)
                {
                    lblMsg.Text += "<br />" + ex.Message;
                }
                finally
                {
                    
                   // DeleteTempFileAlt("icon", tempFile);
                    if (FileName.Contains("_1.jpg"))
                    {

                        if ((ViewState["File2"] != null && tempFile.ToString().Equals(ViewState["File2"].ToString())) || (ViewState["File3"] != null && tempFile.ToString().Equals(ViewState["File3"].ToString())) || (ViewState["File4"] != null && tempFile.ToString().Equals(ViewState["File4"].ToString())) || (ViewState["File5"] != null && tempFile.ToString().Equals(ViewState["File5"].ToString())))
                        {

                        }
                        else
                        {
                            DeleteTempFileAlt("icon", tempFile);
                        }
                    }
                    else if (FileName.Contains("_2.jpg"))
                    {
                        if ((ViewState["File3"] != null && tempFile.ToString().Equals(ViewState["File3"].ToString())) || (ViewState["File4"] != null && tempFile.ToString().Equals(ViewState["File4"].ToString())) || (ViewState["File5"] != null && tempFile.ToString().Equals(ViewState["File5"].ToString())))
                        {

                        }
                        else
                        {
                            DeleteTempFileAlt("icon", tempFile);
                        }
                    }
                    else if (FileName.Contains("_3.jpg"))
                    {
                        if ((ViewState["File4"] != null && tempFile.ToString().Equals(ViewState["File4"].ToString())) || (ViewState["File5"] != null && tempFile.ToString().Equals(ViewState["File5"].ToString())))
                        {

                        }
                        else
                        {
                            DeleteTempFileAlt("icon", tempFile);
                        }

                    }
                    else if (FileName.Contains("_4.jpg"))
                    {
                        if ((ViewState["File5"] != null && tempFile.ToString().Equals(ViewState["File5"].ToString())))
                        {

                        }
                        else
                        {
                            DeleteTempFileAlt("icon", tempFile);
                        }
                    }
                    else if (FileName.Contains("_5.jpg"))
                    {
                        DeleteTempFileAlt("icon", tempFile);
                    }

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
                CommonComponent.ErrorLog("UpdateProductImage.aspx", ex.Message, ex.StackTrace);
            }
        }
        protected void DeleteTempFileAlt(string strsize, string tempfile)
        {
            try
            {
                if (strsize == "icon")
                {
                    string path = string.Empty;
                    if (tempfile != null && tempfile.ToString().Trim().Length > 0)
                    {
                        path = Server.MapPath(ProductTempPath + tempfile.ToString());
                    }

                    File.Delete(path);
                   
                }
            }
            catch (Exception ex)
            {
                CommonComponent.ErrorLog("UpdateProductImage.aspx", ex.Message, ex.StackTrace);
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

                        if (Request.QueryString["Mode"] != null)
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductLargePath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "medium":
                        strFilePath = Server.MapPath(ProductMediumPath + FileName);
                        if (Request.QueryString["Mode"] != null )
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductMediumPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "icon":
                        strFilePath = Server.MapPath(ProductIconPath + FileName);
                        if (Request.QueryString["Mode"] != null )
                        {
                            if (ViewState["DelImage"] != null && ViewState["DelImage"].ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductIconPath + ViewState["DelImage"].ToString());
                            }
                        }
                        break;
                    case "micro":
                        strFilePath = Server.MapPath(ProductMicroPath + FileName);
                        if (Request.QueryString["Mode"] != null )
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
                CommonComponent.ErrorLog("UpdateProductImage.aspx", ex.Message, ex.StackTrace);
            }

        }
        protected void CreateImageAlt(string Size, string FileName, System.Web.UI.HtmlControls.HtmlImage img1option, string delfile)
        {
            try
            {
                string strFile = null;
                String strPath = "";
                if (img1option.Src.ToString().IndexOf("?") > -1)
                {
                    strPath = img1option.Src.Split('?')[0];
                }
                else
                {
                    strPath = img1option.Src.ToString();
                }
                strFile = Server.MapPath(strPath);
                string strFilePath = "";
                Size = Size.ToLower();
                switch (Size)
                {
                    case "large":
                        strFilePath = Server.MapPath(ProductLargePath + FileName);

                        if (Request.QueryString["Mode"] != null )
                        {
                            if (delfile != null && delfile.ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductLargePath + delfile.ToString());
                            }
                        }
                        break;
                    case "medium":
                        strFilePath = Server.MapPath(ProductMediumPath + FileName);
                        if (Request.QueryString["Mode"] != null )
                        {
                            if (delfile != null && delfile.ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductMediumPath + delfile.ToString());
                            }
                        }
                        break;
                    case "icon":
                        strFilePath = Server.MapPath(ProductIconPath + FileName);
                        if (Request.QueryString["Mode"] != null )
                        {
                            if (delfile != null && delfile.ToString().Trim().Length > 0)
                            {
                                DeleteImage(ProductIconPath + delfile.ToString());
                            }
                        }
                        break;
                    case "micro":
                        strFilePath = Server.MapPath(ProductMicroPath + FileName);
                        if (Request.QueryString["Mode"] != null )
                        {
                            if (delfile != null && delfile.ToString().Trim().Length > 0)
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
                CommonComponent.ErrorLog("UpdateProductImage.aspx", ex.Message, ex.StackTrace);
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
                case "medium":
                    finHeight = thumbNailSizeMediam.Height;
                    finWidth = thumbNailSizeMediam.Width;
                    break;
                case "icon":
                    if (txtIconWidth.Text.Trim() != "" && txtIconHeigth.Text.Trim() != "")
                    {
                        Size CustomIconSize = new Size(Convert.ToInt32(txtIconWidth.Text.Trim()), Convert.ToInt32(txtIconHeigth.Text.Trim()));
                        finHeight = CustomIconSize.Height;
                        finWidth = CustomIconSize.Width;
                    }
                    else
                    {
                        finHeight = thumbNailSizeIcon.Height;
                        finWidth = thumbNailSizeIcon.Width;
                    }
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
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == resizeMimeType)
                    return codecs[i];
            return null;
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
                //CommonOperation.WriteLog("\r\n Error Description: " + ex.Message + "\r\n" + ex.StackTrace + " \r\n Method :->DeleteImage() \r\n Date: " + System.DateTime.Now + "\r\n");
            }
        }

        protected void imgbtnAlt1del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage1"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage1"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage1"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage1"].ToString());
            ViewState["DelImage1"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt1.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt1del.Visible = false;
        }
        protected void imgbtnAlt2del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage2"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage2"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage2"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage2"].ToString());
            ViewState["DelImage2"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt2.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt2del.Visible = false;
        }
        protected void imgbtnAlt3del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage3"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage3"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage3"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage3"].ToString());
            ViewState["DelImage3"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt3.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt3del.Visible = false;
        }
        protected void imgbtnAlt4del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage4"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage4"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage4"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage4"].ToString());
            ViewState["DelImage4"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt4.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt4del.Visible = false;
        }
        protected void imgbtnAlt5del_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage5"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage5"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage5"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage5"].ToString());
            ViewState["DelImage5"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgAlt5.Src = ProductMediumPath + "image_not_available.jpg";
            imgbtnAlt5del.Visible = false;
        }

        protected void imgbtnAlt1_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt1.FileName.Length > 0 && Path.GetExtension(flalt1.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt1.FileName.Length > 0)
                {
                    try
                    {
                        FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt1.FileName);
                        if (flinfo.Exists)
                        {
                            flinfo.Delete();
                        }
                    }
                    catch { }
                    ViewState["File1"] = flalt1.FileName.ToString();
                    flalt1.SaveAs(Server.MapPath(ProductTempPath) + flalt1.FileName);
                    ImgAlt1.Src = ProductTempPath + flalt1.FileName;
                    lblMsg.Text = "";
                }
                else
                {
                    ViewState["File1"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt1');});", true);
            }

        }
        protected void imgbtnAlt2_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt2.FileName.Length > 0 && Path.GetExtension(flalt2.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt2.FileName.Length > 0)
                {
                    try
                    {
                        FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt2.FileName);
                        if (flinfo.Exists)
                        {
                            flinfo.Delete();
                        }
                    }
                    catch { }
                    ViewState["File2"] = flalt2.FileName.ToString();
                    flalt2.SaveAs(Server.MapPath(ProductTempPath) + flalt2.FileName);
                    ImgAlt2.Src = ProductTempPath + flalt2.FileName;
                    lblMsg.Text = "";
                }
                else
                {
                    ViewState["File2"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt2');});", true);
            }

        }
        protected void imgbtnAlt3_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt3.FileName.Length > 0 && Path.GetExtension(flalt3.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt3.FileName.Length > 0)
                {
                    try
                    {
                        FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt3.FileName);
                        if (flinfo.Exists)
                        {
                            flinfo.Delete();
                        }
                    }
                    catch { }
                    ViewState["File3"] = flalt3.FileName.ToString();
                    flalt3.SaveAs(Server.MapPath(ProductTempPath) + flalt3.FileName);
                    ImgAlt3.Src = ProductTempPath + flalt3.FileName;
                    lblMsg.Text = "";
                }
                else
                {
                    ViewState["File3"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt3');});", true);
            }

        }
        protected void imgbtnAlt4_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt4.FileName.Length > 0 && Path.GetExtension(flalt4.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt4.FileName.Length > 0)
                {
                    try
                    {
                        FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt4.FileName);
                        if (flinfo.Exists)
                        {
                            flinfo.Delete();
                        }
                    }
                    catch { }
                    ViewState["File4"] = flalt4.FileName.ToString();
                    flalt4.SaveAs(Server.MapPath(ProductTempPath) + flalt4.FileName);
                    ImgAlt4.Src = ProductTempPath + flalt4.FileName;
                    lblMsg.Text = "";
                }
                else
                {
                    ViewState["File4"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt4');});", true);
            }

        }
        protected void imgbtnAlt5_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (flalt5.FileName.Length > 0 && Path.GetExtension(flalt5.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (flalt5.FileName.Length > 0)
                {
                    try
                    {
                        FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + flalt5.FileName);
                        if (flinfo.Exists)
                        {
                            flinfo.Delete();
                        }
                    }
                    catch { }
                    ViewState["File5"] = flalt5.FileName.ToString();
                    flalt5.SaveAs(Server.MapPath(ProductTempPath) + flalt5.FileName);
                    ImgAlt5.Src = ProductTempPath + flalt5.FileName;
                    lblMsg.Text = "";
                }
                else
                {
                    ViewState["File5"] = null;
                }
            }
            else
            {
                lblMsg.Text = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "PDFMsg", "$(document).ready( function() {jAlert('" + lblMsg.Text.ToString() + "', 'Message','ContentPlaceHolder1_flalt5');});", true);
            }

        }
        /// <summary>
        ///  Upload Image Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpload_Click(object sender, ImageClickEventArgs e)
        {
            GC.Collect();
            GC.WaitForPendingFinalizers();
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);
            ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProductBulk"), "Temp/");
            if (!Directory.Exists(Server.MapPath(ProductTempPath)))
                Directory.CreateDirectory(Server.MapPath(ProductTempPath));

            for (int j = 0; j < StoreArray.Length; j++)
                if (fuProductIcon.FileName.Length > 0 && Path.GetExtension(fuProductIcon.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;

            if (Flag)
            {
                if (fuProductIcon.FileName.Length > 0)
                {
                    try
                    {
                        FileInfo flinfo = new FileInfo(Server.MapPath(ProductTempPath) + fuProductIcon.FileName);
                        if (flinfo.Exists)
                        {
                            flinfo.Delete();
                        }
                    }
                    catch { }
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
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DeleteImage(ProductLargePath + ViewState["DelImage"].ToString());
            DeleteImage(ProductMediumPath + ViewState["DelImage"].ToString());
            DeleteImage(ProductIconPath + ViewState["DelImage"].ToString());
            DeleteImage(ProductMicroPath + ViewState["DelImage"].ToString());
            ViewState["DelImage"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgLarge.Src = ProductMediumPath + "image_not_available.jpg";
            btnDelete.Visible = false;
        }
    }
}