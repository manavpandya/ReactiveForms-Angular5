using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.IO;
using System.Drawing;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class ProductCloneOptions : BasePage
    {
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
            if (!IsPostBack)
            {
                ProductTempPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Temp/");
                ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");
            }
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
            btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
        }

        /// <summary>
        /// Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["Id"] != null)
            {
                Int32 Result = Convert.ToInt32(ProductComponent.SaveCloneProductID(Convert.ToInt32(Request.QueryString["StoreId"]), Convert.ToInt32(Request.QueryString["Id"])));
                if (Result > 0)
                {
                    try
                    {
                        DataSet DsProduct = new DataSet();
                        DsProduct = CommonComponent.GetCommonDataSet("Select * From tb_Product Where ProductId=" + Convert.ToInt32(Request.QueryString["Id"]) + " And storeId=" + Convert.ToInt32(Request.QueryString["StoreId"]) + "");
                        if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
                        {
                            ProductIconPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Icon/");
                            ProductMediumPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Medium/");
                            ProductLargePath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Large/");
                            ProductMicroPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "Micro/");

                            if (!Directory.Exists(Server.MapPath(ProductMediumPath)))
                                Directory.CreateDirectory(Server.MapPath(ProductMediumPath));
                            string Imagename = Convert.ToString(DsProduct.Tables[0].Rows[0]["Imagename"].ToString());

                            #region Save Product Images

                            string Sku = Convert.ToString(DsProduct.Tables[0].Rows[0]["sku"].ToString());

                            string strFilePath = Server.MapPath(ProductMediumPath + Imagename);
                            if (File.Exists(strFilePath) && !string.IsNullOrEmpty(Imagename.ToString()))
                            {
                                File.Copy(strFilePath, Server.MapPath(ProductMediumPath + Sku + "_" + Result.ToString() + ".jpg"));
                            }

                            string strFileLargePath = Server.MapPath(ProductLargePath + Imagename);
                            if (File.Exists(strFileLargePath) && !string.IsNullOrEmpty(Imagename.ToString()))
                            {
                                File.Copy(strFileLargePath, Server.MapPath(ProductLargePath + Sku + "_" + Result.ToString() + ".jpg"));
                            }

                            string strFileIconPath = Server.MapPath(ProductIconPath + Imagename);
                            if (File.Exists(strFileIconPath) && !string.IsNullOrEmpty(Imagename.ToString()))
                            {
                                File.Copy(strFileIconPath, Server.MapPath(ProductIconPath + Sku + "_" + Result.ToString() + ".jpg"));
                            }

                            string strFileMicroPath = Server.MapPath(ProductMicroPath + Imagename);
                            if (File.Exists(strFileMicroPath) && !string.IsNullOrEmpty(Imagename.ToString()))
                            {
                                File.Copy(strFileMicroPath, Server.MapPath(ProductMicroPath + Sku + "_" + Result.ToString() + ".jpg"));
                            }
                            //update
                            CommonComponent.ExecuteCommonData("Update tb_product set Imagename='" + Sku + "_" + Result.ToString() + ".jpg' Where ProductId=" + Result + " And storeId=" + Convert.ToInt32(Request.QueryString["StoreId"]) + "");
                            string[] ArrFileName = Imagename.Split('.');
                            string filename = ArrFileName[0].ToString();

                            // Copy more Image for Product
                            try
                            {
                                for (int i = 1; i < 26; i++)
                                {
                                    if (File.Exists(Server.MapPath(ProductLargePath + filename + "_" + i.ToString() + ".jpg")) == true)
                                    {
                                        File.Copy(Server.MapPath(ProductLargePath + filename + "_" + i.ToString() + ".jpg"), Server.MapPath(ProductLargePath + Sku + "_" + Result.ToString() + "_" + i.ToString() + ".jpg"), true);
                                    }
                                    if (File.Exists(Server.MapPath(ProductMediumPath + filename + "_" + i.ToString() + ".jpg")) == true)
                                    {
                                        File.Copy(Server.MapPath(ProductMediumPath + filename + "_" + i.ToString() + ".jpg"), Server.MapPath(ProductMediumPath + Sku + "_" + Result.ToString() + "_" + i.ToString() + ".jpg"), true);
                                    }
                                    if (File.Exists(Server.MapPath(ProductIconPath + filename + "_" + i.ToString() + ".jpg")) == true)
                                    {
                                        File.Copy(Server.MapPath(ProductIconPath + filename + "_" + i.ToString() + ".jpg"), Server.MapPath(ProductIconPath + Sku + "_" + Result.ToString() + "_" + i.ToString() + ".jpg"), true);
                                    }
                                    if (File.Exists(Server.MapPath(ProductMicroPath + filename + "_" + i.ToString() + ".jpg")) == true)
                                    {
                                        File.Copy(Server.MapPath(ProductMicroPath + filename + "_" + i.ToString() + ".jpg"), Server.MapPath(ProductMicroPath + Sku + "_" + Result.ToString() + "_" + i.ToString() + ".jpg"), true);
                                    }
                                }
                            }
                            catch { }

                            #endregion

                            //update Pdf file
                            #region Save .Pdf File

                            string ProductPdfPath = string.Concat(AppLogic.AppConfigs("ImagePathProduct"), "PDF/");
                            if (!string.IsNullOrEmpty(ProductPdfPath.ToString()))
                            {
                                string Pdffile = Server.MapPath(ProductPdfPath + Convert.ToString(DsProduct.Tables[0].Rows[0]["PDFName"].ToString()));
                                string ext = System.IO.Path.GetExtension(Pdffile);
                                if (ext == ".pdf")
                                {
                                    if (File.Exists(Pdffile))
                                    {
                                        File.Copy(Pdffile, Server.MapPath(ProductPdfPath + Convert.ToString(Sku + "_" + Result.ToString() + ".pdf")), true);
                                        CommonComponent.ExecuteCommonData("Update tb_product set PDFName='" + Sku + "_" + Result.ToString() + ".pdf' Where ProductId=" + Result + " And storeId=" + Convert.ToInt32(Request.QueryString["StoreId"]) + "");
                                    }
                                }
                            }
                            #endregion
                        }
                    }
                    catch { }

                    Page.ClientScript.RegisterStartupScript(this.GetType(), "set", "alert('Product Clone Successfully.'); window.close(); window.opener.location.href='Product.aspx?StoreID=" + Convert.ToInt32(Request.QueryString["StoreId"]) + "&ID=" + Convert.ToInt32(Result) + "&Mode=edit'; ", true);
                    return;
                }
                else
                {

                }
            }
        }
    }
}