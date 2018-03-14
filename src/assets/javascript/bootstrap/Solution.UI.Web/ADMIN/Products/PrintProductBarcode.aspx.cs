using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Data;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using UCABarcode;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class PrintProductBarcode : BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindstore();
                AppConfig.StoreID = 1;
                if (ddlStore.SelectedIndex == 0)
                {
                    AppConfig.StoreID = 1;
                }
                else
                {
                    AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
                }
            }
            btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
            btnPrintBarcode.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/print-barcode.gif";
        }


        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        private void bindstore()
        {
            // int StoreID = Convert.ToInt32(Request.QueryString["StoreID"]);
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
            }
            // ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            //Store is selected dynamically from menu
            if (!string.IsNullOrEmpty(Request.QueryString["StoreID"]))
                ddlStore.SelectedValue = Request.QueryString["StoreID"].ToString();
            else
                ddlStore.SelectedIndex = 0;

            if (ddlStore.SelectedItem.Text.ToString().Trim().ToLower().IndexOf("amazon") > -1)
            {
                //btnAmazonPrice.Visible = true;
                //btnAmazonProduct.Visible = true;
                //btnAmozonUpdate.Visible = true;
            }
            else
            {
                // btnAmazonPrice.Visible = false;
                //btnAmazonProduct.Visible = false;
                //btnAmozonUpdate.Visible = false;
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnSearch_Click(null, null);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            ltrbarcode.Text = "";
            if (!string.IsNullOrEmpty(txtName.Text.ToString().Trim()))
            {
                string StrQuery = "Select DISTINCT Isnull(Sku,'') as SKU,UPC from tb_Product  Where isnull(Deleted,0)=0 and StoreId=" + ddlStore.SelectedValue + " and (Name like '%" + txtName.Text.ToString().Replace("'", "''") + "%' or SKU like '%" + txtName.Text.ToString().Replace("'", "''") + "%' OR UPC like '%" + txtName.Text.ToString().Replace("'", "''") + "%') and ISNULL(upc,'') <>'' and Isnull(Sku,'') <>''";
                DataSet DsProduct = new DataSet();
                DsProduct = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                StrQuery = "Select DISTINCT Isnull(Sku,'') as SKU,UPC from tb_ProductVariantValue Where ProductId in (SELECT ProductId FROM tb_Product  Where StoreId=" + ddlStore.SelectedValue + " and isnull(Deleted,0)=0) and (SKU like '%" + txtName.Text.ToString().Replace("'", "''") + "%'  OR UPC like '%" + txtName.Text.ToString().Replace("'", "''") + "%') and ISNULL(UPC,'') <>'' and Isnull(SKU,'') <>'' ";
                StrQuery += " UNION Select DISTINCT Isnull(Sku,'') as SKU,UPC from tb_ProductVariantValue Where ISNULL(UPC,'') <>'' and Isnull(SKU,'') <>'' AND productID IN (Select ProductID from tb_Product  Where isnull(Deleted,0)=0 and StoreId=" + ddlStore.SelectedValue + " and (Name like '%" + txtName.Text.ToString().Replace("'", "''") + "%' or SKU like '%" + txtName.Text.ToString().Replace("'", "''") + "%' OR UPC like '%" + txtName.Text.ToString().Replace("'", "''") + "%') and ISNULL(upc,'') <>'' and Isnull(Sku,'') <>'')";
                DataSet DsProduct1 = new DataSet();
                DsProduct1 = CommonComponent.GetCommonDataSet(StrQuery.ToString());
                DsProduct.Merge(DsProduct1);
                if (DsProduct != null && DsProduct.Tables.Count > 0 && DsProduct.Tables[0].Rows.Count > 0)
                {
                    ddlSku.DataSource = DsProduct.Tables[0];
                    ddlSku.DataTextField = "SKU";
                    ddlSku.DataValueField = "UPC";
                    ddlSku.DataBind();
                    trSku.Visible = true;
                    trBarcode.Visible = true;
                }
                else
                {
                    ddlSku.DataSource = null;
                    ddlSku.DataBind();
                    trSku.Visible = false;
                    trBarcode.Visible = false;
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "pagevalid", "alert('Record(s) not Found.');", true);
                    return;
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "pagevalid", "alert('Please Enter Name.');", true);
                return;
            }
        }

        //protected void ddlproducts_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    GetProductDetailsByProductID(Convert.ToInt32(ddlproducts.SelectedValue));

        //}
        protected void btnPrintBarcode_Click(object sender, EventArgs e)
        {
            ltrbarcode.Text = "";
            if (ddlStore.SelectedValue.ToString() != null && ddlStore.SelectedValue.ToString() != "" && !string.IsNullOrEmpty(ddlSku.SelectedValue.ToString().Trim()))
            {
                if (txtQuantity.Text.Trim() != "" && Convert.ToInt32(txtQuantity.Text.Trim()) > 0)
                {
                    string strImageWidth = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Configname='ZebraBrcodeWidth' and storeid=1"));
                    Int32 strImageWidth1 = Convert.ToInt32(strImageWidth) + 10;
                    String chkUPCExists = ddlSku.SelectedValue.ToString();
                    // chkUPCExists = Convert.ToString(CommonComponent.GetScalarCommonData("Select isnull(upc,'') as upc from tb_Product where ISNULL(upc,'')<>'' and (SKU ='" + ddlSku.SelectedValue.ToString().Trim().Replace("'", "''") + "') and StoreID=" + ddlStore.SelectedValue + ""));
                    if (chkUPCExists != null && chkUPCExists != "" && chkUPCExists.Length > 0)
                    {
                        String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodeZebraPath"));
                        if (System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + Convert.ToString(chkUPCExists) + ".png")))
                        {
                            //DataSet dsproducts = CommonComponent.GetCommonDataSet("Select isnull(upc,'') as upc, ProductID, SKU ,isnull(Name,'') as Name from tb_product where ISNULL(upc,'')<>'' and (SKU='" + ddlSku.SelectedValue.ToString().Replace("'", "''") + "') and StoreID=" + ddlStore.SelectedValue + "");
                            //if (dsproducts != null && dsproducts.Tables.Count > 0 && dsproducts.Tables[0].Rows.Count > 0)
                            //{
                            trBarcode.Visible = true;
                            //ltrbarcode.Text += "<table cellpadding='0' cellspacing='0'>";
                            //ltrbarcode.Text += "<tr>";
                            //ltrbarcode.Text += "<td>";
                            for (int i = 0; i < Convert.ToInt32(txtQuantity.Text.Trim()); i++)
                            {
                                if (Convert.ToInt32(txtQuantity.Text.Trim()) - 1 == i)
                                {
                                    ltrbarcode.Text += "<div style='width:" + strImageWidth1.ToString() + "px;'>";
                                }
                                else
                                {
                                    ltrbarcode.Text += "<div style='width:" + strImageWidth1.ToString() + "px;page-break-after: always;'>";
                                }
                                ltrbarcode.Text += "<table width='100%'>";
                                ltrbarcode.Text += "<tr>";
                                ltrbarcode.Text += "<td valign='top'>";
                                ltrbarcode.Text += "<span style='float:left;font-size:12px;padding-left:8px;color:#000000 !important;font-weight:bold;'> " + ddlSku.SelectedItem.Text.ToString() + "</span>";
                                ltrbarcode.Text += "</td>";
                                ltrbarcode.Text += "</tr>";
                                ltrbarcode.Text += "<tr>";
                                ltrbarcode.Text += "<td>";
                                ltrbarcode.Text += "<img src='" + FPath + "/UPC-" + chkUPCExists.Trim() + ".png" + "' width='" + strImageWidth + "px' />";
                                ltrbarcode.Text += "</td>";
                                ltrbarcode.Text += "</tr>";
                                ltrbarcode.Text += "</table>";
                                ltrbarcode.Text += "</div>";
                            }
                            //ltrbarcode.Text += "</tr>";
                            //ltrbarcode.Text += "</td>";
                            //ltrbarcode.Text += "</table>";

                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Msg1123", "PrintBarcode(" + txtQuantity.Text.Trim() + ");", true);
                            //}
                        }
                        else
                        {
                            GenerateBarcode(ddlSku.SelectedItem.Text.ToString().Replace("'", "''"), chkUPCExists);
                            Page.ClientScript.RegisterStartupScript(this.GetType(), "Msg11235656", "PrintBarcode(" + txtQuantity.Text.Trim() + ");", true);
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(this.GetType(), "Msg1123fdf", "alert('UPC Not Available For this Product.');", true);
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(this.GetType(), "Qtysdf", "alert('Please Enter Valid Quantity.');", true);
                    return;
                }
            }
        }
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
            if (imgVisol.Height >= FinHeight && imgVisol.Width >= FinWidth)
            {

                Bitmap newImage = new Bitmap(FinWidth, FinHeight);
                newImage.SetResolution(203, 203);
                using (Graphics gr = Graphics.FromImage(newImage))
                {
                    gr.SmoothingMode = SmoothingMode.AntiAlias;
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    gr.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    gr.DrawImage(imgVisol, new Rectangle(0, 0, FinWidth, FinHeight));
                    gr.Dispose();
                }

                newImage.Save(strFilePath);
                newImage.Dispose();
            }
            else
            {
                grPhoto.Clear(Color.White);
                grPhoto.InterpolationMode = InterpolationMode.HighQualityBicubic;
                Rectangle DestRect = new Rectangle(0, 0, destWidth, destHeight);
                Rectangle srcRect = new Rectangle(int.Parse("0"), int.Parse("0"), sourceWidth - (int.Parse("0")), sourceHeight - (int.Parse("0")));
                grPhoto.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
                grPhoto.DrawImage(imgVisol, DestRect, srcRect, GraphicsUnit.Pixel);

                GenerateImage(resizedPhoto, strFilePath, FinWidth, FinHeight);
            }
            resizedPhoto.Dispose();
            grPhoto.Dispose();
            imgVisol.Dispose();
        }
        private void GenerateImage(Bitmap extBMP, string DestFileName, int DefWidth, int DefHeight)
        {
            Encoder Enc = Encoder.SaveFlag;
            EncoderParameters EncParms = new EncoderParameters(1);
            EncoderParameter EncParm;
            ImageCodecInfo CodecInfo = GetEncoderInfo("image/png");
            EncParm = new EncoderParameter(Encoder.Quality, (long)600);
            EncParms.Param[0] = new EncoderParameter(Encoder.Quality, (long)600);

            if (extBMP != null && extBMP.Width < (DefWidth) && extBMP.Height < (DefHeight))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                //newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                newBMP.SetResolution(203, 203);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, startX, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
            }
            else if (extBMP != null && extBMP.Width < (DefWidth))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                //newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                newBMP.SetResolution(203, 203);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                g.DrawImage(extBMP, startX, 0);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();

            }
            else if (extBMP != null && extBMP.Height < (DefHeight))
            {
                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                // newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                newBMP.SetResolution(203, 203);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                int startY = (int)(DefHeight / 2) - (extBMP.Height / 2);
                g.DrawImage(extBMP, 0, startY);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
            }
            else if (extBMP != null)
            {

                Bitmap newBMP = new Bitmap(DefWidth, DefHeight);
                //newBMP.SetResolution(extBMP.HorizontalResolution, extBMP.VerticalResolution);
                newBMP.SetResolution(203, 203);
                Graphics g = Graphics.FromImage(newBMP);
                g.Clear(Color.White);
                //int startX = (int)(DefWidth / 2) - (extBMP.Width / 2);
                g.DrawImage(extBMP, 0, 0);
                newBMP.Save(DestFileName, CodecInfo, EncParms);
                newBMP.Dispose();
                extBMP.Dispose();
                //extBMP.Save(DestFileName, CodecInfo, EncParms);
                //extBMP.Dispose();
            }
        }
        /// <summary>
        /// Generates the Barcode
        /// </summary>
        /// <param name="UPCCode">string UPCCode</param>
        private void GenerateBarcode(String SKU, String UPCCode)
        {
            String FPath = Convert.ToString(AppLogic.AppConfigs("BarcodeZebraPath"));
            String FZPath = Convert.ToString(AppLogic.AppConfigs("BarcodeZebraPath"));
            CreateFolder(FPath.ToString());
            string strImageWidth = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Configname='ZebraBrcodeWidth' and storeid=1"));
            Int32 strImageWidth1 = Convert.ToInt32(strImageWidth) + 10;

            string strImageheight = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Configname='ZebraBrcodeHeight' and storeid=1"));
            string strScale = Convert.ToString(CommonComponent.GetScalarCommonData("SELECT TOP 1 Configvalue FROM tb_AppConfig WHERE Configname='ZebraBrcodeScale' and storeid=1"));
            if (!System.IO.File.Exists(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png")))
            {
                //DataSet dsproducts = CommonComponent.GetCommonDataSet("Select isnull(upc,'') as upc, ProductID, SKU ,isnull(Name,'') as Name from tb_product where ISNULL(upc,'')<>'' and SKU='" + SKU + "' and StoreID=" + ddlStore.SelectedValue + "");
                //if (dsproducts != null && dsproducts.Tables.Count > 0 && dsproducts.Tables[0].Rows.Count > 0)
                //{


                // OLD Start

                //DSBarCode.BarCodeCtrl bCodeControl = new DSBarCode.BarCodeCtrl();
                //bCodeControl.BarCode = UPCCode.Trim();
                //bCodeControl.VertAlign = DSBarCode.BarCodeCtrl.AlignType.Center;
                //bCodeControl.Weight = DSBarCode.BarCodeCtrl.BarCodeWeight.Small;
                //bCodeControl.BarCodeHeight = 70;
                //bCodeControl.ShowHeader = false;
                //bCodeControl.ShowFooter = true;
                //bCodeControl.FooterText = "UPC-" + UPCCode.Trim();// +System.Environment.NewLine + dsproducts.Tables[0].Rows[0]["Name"].ToString() + System.Environment.NewLine + dsproducts.Tables[0].Rows[0]["SKU"].ToString();                    
                //bCodeControl.Size = new System.Drawing.Size(250, 100);
                //bCodeControl.AutoSize = true;
                //bCodeControl.SaveImage(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"));

                // Old End

                //New Start 
                cUPCA upca = new cUPCA();
                if (UPCCode.Trim().Length == 12)
                {
                    UPCCode = UPCCode.Substring(0, 11) + upca.GetCheckSum(UPCCode).ToString();
                    System.Drawing.Image img;
                    img = upca.CreateBarCode(UPCCode, Convert.ToInt32(strScale), (Convert.ToInt32(strImageWidth) / Convert.ToInt32(strScale)), (Convert.ToInt32(strImageheight) / Convert.ToInt32(strScale)));
                    img.Save(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"));
                    img.Dispose();
                }
                //New End

                //}
                trBarcode.Visible = true;
                //ltrbarcode.Text += "<table cellpadding='0' cellspacing='0'>";
                //ltrbarcode.Text += "<tr>";
                //ltrbarcode.Text += "<td>";
                for (int i = 0; i < Convert.ToInt32(txtQuantity.Text.Trim()); i++)
                {
                    if (Convert.ToInt32(txtQuantity.Text.Trim()) - 1 == i)
                    {
                        ltrbarcode.Text += "<div style='width:" + strImageWidth1.ToString() + "px;'>";
                    }
                    else
                    {
                        ltrbarcode.Text += "<div style='width:" + strImageWidth1.ToString() + "px;page-break-after: always;'>";
                    }
                    //ltrbarcode.Text += "<div style='width:" + strImageWidth1 + "px;'>";
                    ltrbarcode.Text += "<table width='100%'>";
                    ltrbarcode.Text += "<tr>";
                    ltrbarcode.Text += "<td valign='top'>";
                    ltrbarcode.Text += "<span style='float:left;font-size:12px;padding-left:8px;color:#000000 !important;font-weight:bold;'> " + ddlSku.SelectedItem.Text.ToString() + "</span>";
                    ltrbarcode.Text += "</td>";
                    ltrbarcode.Text += "</tr>";
                    ltrbarcode.Text += "<tr>";
                    ltrbarcode.Text += "<td>";
                    ltrbarcode.Text += "<img src='" + FPath + "/UPC-" + UPCCode.Trim() + ".png" + "' width='" + strImageWidth + "px' />";
                    ltrbarcode.Text += "</td>";
                    ltrbarcode.Text += "</tr>";
                    ltrbarcode.Text += "</table>";
                    ltrbarcode.Text += "</div>";
                }
                //ltrbarcode.Text += "</tr>";
                //ltrbarcode.Text += "</td>";
                //ltrbarcode.Text += "</table>";

                //  ResizeImage(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"), 190, 100, Server.MapPath(FZPath + "/UPC-" + UPCCode.Trim() + ".png"));
                //File.Copy(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"), Server.MapPath(FZPath + "/UPC-" + UPCCode.Trim() + ".png"));
            }
            else
            {
                //  ResizeImage(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"), 190, 100, Server.MapPath(FZPath + "/UPC-" + UPCCode.Trim() + ".png"));
                // File.Copy(Server.MapPath(FPath + "/UPC-" + UPCCode.Trim() + ".png"), Server.MapPath(FZPath + "/UPC-" + UPCCode.Trim() + ".png"), true);
            }
        }
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            } return null;
        }
        /// <summary>
        /// Creates the Folder at Specified Path
        /// </summary>
        /// <param name="FPath">string FPath</param>
        private void CreateFolder(String FPath)
        {
            if (!Directory.Exists(Server.MapPath(FPath.ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(FPath.ToString()));
            }
        }
    }
}