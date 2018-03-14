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
    public partial class SearchProductType : BasePage
    {
        public static string SearchProductTempPath = string.Empty;
        public static string SearchProductPath = string.Empty;
        public static string SearchProductTempIndexImagePath = string.Empty;
        public static string SearchProductIndexImagePath = string.Empty;
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
            SearchProductTempIndexImagePath = string.Concat(AppLogic.AppConfigs("SecondaryColorImages"), "Temp2/");
            SearchProductIndexImagePath = string.Concat(AppLogic.AppConfigs("SecondaryColorImages"), "HomePage/");
            BindSize();
            if (!IsPostBack)
            {
                if (Request.QueryString["SearchColor"] != null)
                {
                    ddlSearchType.SelectedValue = "1"; //for Color
                }
                else if (Request.QueryString["SearchPattern"] != null)
                {
                    ddlSearchType.SelectedValue = "2"; //for pattern
                }
                else if (Request.QueryString["SearchFabric"] != null)
                {
                    ddlSearchType.SelectedValue = "3"; //for fabric
                }
                else if (Request.QueryString["SearchStyle"] != null)
                {
                    ddlSearchType.SelectedValue = "8"; //for Style
                }
                else if (Request.QueryString["SearchFeature"] != null)
                {
                    ddlSearchType.SelectedValue = "4"; //for Style
                }
                else if (Request.QueryString["SearchHeader"] != null)
                {
                    ddlSearchType.SelectedValue = "5"; //for Style
                }
                else if (Request.QueryString["SearchCustomCalculator"] != null)
                {
                    ddlSearchType.SelectedValue = "6"; //for CustomStyle
                }
                else if (Request.QueryString["SearchSecondaryColorStyle"] != null)
                {
                    ddlSearchType.SelectedValue = "1"; //for Color
                }
                else if (Request.QueryString["SearchOptionCalculator"] != null)
                {
                    ddlSearchType.SelectedValue = "7"; //for Color
                }


                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btnUpload.ImageUrl = "/App_Themes/" + Page.Theme + "/images/upload.gif";
                btnDelete.ImageUrl = "/App_Themes/" + Page.Theme + "/images/delet.gif";

                if (!string.IsNullOrEmpty(Request.QueryString["SearchTypeID"]))
                {
                    BindData(Convert.ToInt32(Request.QueryString["SearchTypeID"]));
                    lblTitle.Text = "Edit Search Product Type";
                }
            }

            if (ddlSearchType.SelectedValue.ToString().Trim() == "1")
            {
                lblSize.Text = "Size should be 120 x 120";
            }
            else if (ddlSearchType.SelectedValue.ToString().Trim() == "2")
            {
                lblSize.Text = "Size should be 216 x 116";
            }
            else
            {
                lblSize.Text = "";
            }
            ddlSearchType.Focus();
        }

        /// <summary>
        /// Fill State Data while Edit mode is Active 
        /// </summary>
        /// <param name="StateID">int StateID</param>
        private void BindData(Int32 SearchTypeID)
        {
            DataSet dsSearchpro = new DataSet();
            dsSearchpro = CommonComponent.GetCommonDataSet("Select SearchId,ISNULL(SearchValue,'') as SearchValue,ISNULL(SearchType,0) as SearchType,ISNULL(Price,0) as Price,ISNULL(PerInch,0) as PerInch,ISNULL(ImageName,'') as ImageName,ISNULL(Deleted,0) as Deleted,Createdon,ISNULL(Active,1) as Active,Isnull(DisplayOrder,0) as DisplayOrder,isnull(SEKeywords,'') as SEKeywords,isnull(SEDescription,'') as SEDescription,isnull(SETitle,'') as SETitle,isnull(PageTitle,'') as PageTitle,isnull(PageDescription,'') as PageDescription from tb_ProductSearchType where SearchId=" + SearchTypeID + "");
            if (dsSearchpro != null && dsSearchpro.Tables.Count > 0 && dsSearchpro.Tables[0].Rows.Count > 0)
            {
                txtSearchName.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["SearchValue"]);
                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["Price"].ToString().Trim()))
                    txtPrice.Text = "0.00";
                else
                    txtPrice.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["Price"].ToString().Trim()), 2));

                if (string.IsNullOrEmpty(dsSearchpro.Tables[0].Rows[0]["PerInch"].ToString().Trim()))
                    txtPerInch.Text = "0.00";
                else
                    txtPerInch.Text = Convert.ToString(Math.Round(Convert.ToDecimal(dsSearchpro.Tables[0].Rows[0]["PerInch"].ToString().Trim()), 2));

                txtDisplayOrder.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["DisplayOrder"]);

                ddlSearchType.SelectedValue = dsSearchpro.Tables[0].Rows[0]["SearchType"].ToString();
                string Imagename = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["ImageName"]);
                chkActive.Checked = Convert.ToBoolean(dsSearchpro.Tables[0].Rows[0]["Active"]);
                txtSEDescription.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["SEDescription"]);
                txtSEKeyword.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["SEKeywords"]);
                txtSETitle.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["SETitle"]);
                txtPageTitle.Text = Convert.ToString(dsSearchpro.Tables[0].Rows[0]["PageTitle"]);
                txtPageDescription.Text = Server.HtmlDecode(dsSearchpro.Tables[0].Rows[0]["PageDescription"].ToString());

                AppConfig.StoreID = 1;
                SearchProductPath = AppLogic.AppConfigs("SecondaryColorImages");
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



                if (!string.IsNullOrEmpty(Imagename))
                {
                    string strFilePath = Server.MapPath(SearchProductIndexImagePath + Imagename);
                    btnDeleteHomeImage.Visible = false;

                    if (File.Exists(strFilePath))
                    {
                        ViewState["DelHomeImage"] = Imagename;
                        btnDeleteHomeImage.Visible = true;
                        ImgIndexImage.Src = SearchProductIndexImagePath + Imagename + "?" + rnd.Next(10000);
                    }
                    else
                    {
                        ViewState["DelHomeImage"] = null;
                        btnDeleteHomeImage.Visible = false;
                        ImgIndexImage.Src = SearchProductIndexImagePath + "image_not_available.jpg?" + rnd.Next(10000);
                    }
                }
                else
                {
                    ViewState["DelHomeImage"] = null;
                    btnDeleteHomeImage.Visible = false;
                    ImgIndexImage.Src = SearchProductIndexImagePath + "image_not_available.jpg?" + rnd.Next(10000);
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
            if (Request.QueryString["ProductStoreID"] != null && Request.QueryString["ProductID"] != null)
            {
                if (Request.QueryString["SearchColor"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchColor=Color");
                }
                else if (Request.QueryString["SearchPattern"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchPattern=Pattern");
                }
                else if (Request.QueryString["SearchFabric"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchFabric=Fabric");
                }
                else if (Request.QueryString["SearchFeature"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchFeature=Feature");
                }
                else if (Request.QueryString["SearchStyle"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchStyle=Style");
                }
                else if (Request.QueryString["SearchHeader"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchHeader=Header");
                }
                else if (Request.QueryString["SearchCustomCalculator"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchCustomCalculator=Style");
                }
                else if (Request.QueryString["SearchOptionCalculator"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchOptionCalculator=option");
                }

                else if (Request.QueryString["SearchSecondaryColorStyle"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchSecondaryColorStyle=Color");
                }

            }
            else if (Request.QueryString["ProductStoreID"] != null)
            {
                if (Request.QueryString["SearchColor"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchColor=Color");
                }
                else if (Request.QueryString["SearchPattern"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchPattern=Pattern");
                }
                else if (Request.QueryString["SearchFabric"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchFabric=Fabric");
                }
                else if (Request.QueryString["SearchFeature"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchFeature=Feature");
                }
                else if (Request.QueryString["SearchStyle"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchStyle=Style");
                }
                else if (Request.QueryString["SearchHeader"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchHeader=Header");
                }
                else if (Request.QueryString["SearchCustomCalculator"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchCustomCalculator=Style");
                }
                else if (Request.QueryString["SearchOptionCalculator"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchOptionCalculator=option");
                }
                else if (Request.QueryString["SearchSecondaryColorStyle"] != null)
                {
                    Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchSecondaryColorStyle=Color");
                }

            }
            else
            {
                Response.Redirect("SearchProductTypeList.aspx");
            }
        }

        /// <summary>
        /// Save button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            AppConfig.StoreID = 1;
            if (ddlSearchType.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select Search Type.', 'Message','');});", true);
                ddlSearchType.Focus();
                return;
            }
            else if (txtSearchName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter Search Name.', 'Message','');});", true);
                txtSearchName.Focus();
                return;
            }

            decimal Price = 0;
            if (!string.IsNullOrEmpty(txtPrice.Text.Trim()) && Convert.ToDecimal(txtPrice.Text) > 0)
                Price = Convert.ToDecimal(txtPrice.Text.Trim());

            decimal PerInch = 0;
            if (!string.IsNullOrEmpty(txtPerInch.Text.Trim()) && Convert.ToDecimal(txtPerInch.Text) > 0)
                PerInch = Convert.ToDecimal(txtPerInch.Text.Trim());

            Int32 Active = 0;
            if (chkActive.Checked)
                Active = 1;

            Int32 DisplayOrder = 0;
            if (!string.IsNullOrEmpty(txtDisplayOrder.Text.Trim()) && Convert.ToInt32(txtDisplayOrder.Text) > 0)
                DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text.Trim());

            if (!string.IsNullOrEmpty(Request.QueryString["SearchTypeID"]) && Convert.ToString(Request.QueryString["SearchTypeID"]) != "0")
            {
                int chkDuplicate = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_ProductSearchType where SearchValue='" + txtSearchName.Text.Trim().ToString() + "' and SearchType=" + ddlSearchType.SelectedValue + " and ISNULL(Active,0)=1 and isnull(Deleted,0)=0 AND SearchId <> " + Convert.ToString(Request.QueryString["SearchTypeID"]) + ""));
                if (chkDuplicate > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Search Product Value already exists.', 'Message','');});", true);
                    return;
                }
                else
                {
                    string Imgname = Convert.ToString(CommonComponent.GetScalarCommonData("Select ISNULL(ImageName,'') as ImageName from tb_ProductSearchType where SearchId=" + Convert.ToString(Request.QueryString["SearchTypeID"]) + ""));
                    string strImageName = "";
                    try
                    {
                        if (!string.IsNullOrEmpty(txtSearchName.Text.ToString().Trim()))
                        {
                            strImageName = RemoveSpecialCharacter(txtSearchName.Text.ToString().ToCharArray()) + "_" + Convert.ToString(Request.QueryString["SearchTypeID"]) + ".jpg";

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





                    try
                    {
                        strImageName = RemoveSpecialCharacter(txtSearchName.Text.ToString().ToCharArray()) + "_" + Convert.ToString(Request.QueryString["SearchTypeID"]) + ".jpg";
                        if (ImgIndexImage.Src.Contains(SearchProductTempIndexImagePath))
                        {
                            if (!Directory.Exists(Server.MapPath(SearchProductIndexImagePath)))
                                Directory.CreateDirectory(Server.MapPath(SearchProductIndexImagePath));
                            //for (int j = 0; j < StoreArray.Length; j++)
                            //    if (FileUploadhomeImage.FileName.Length > 0 && Path.GetExtension(FileUploadhomeImage.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                            //        Flag = true;
                            //if (Flag)
                            {
                                // if (FileUploadhomeImage.FileName.Length > 0)
                                {

                                    if (File.Exists(Server.MapPath(SearchProductTempIndexImagePath + ViewState["DelHomeImage"].ToString())))
                                        File.Copy(Server.MapPath(SearchProductTempIndexImagePath + ViewState["DelHomeImage"].ToString()), Server.MapPath(SearchProductIndexImagePath + strImageName), true);
                                    ImgIndexImage.Src = SearchProductIndexImagePath + strImageName;
                                    ViewState["DelHomeImage"] = strImageName.ToString();
                                    try
                                    {
                                        Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                                        objcompress.compressimage(Server.MapPath(SearchProductIndexImagePath) + strImageName);
                                    }
                                    catch
                                    {

                                    }
                                    try
                                    {
                                        CommonOperations.SaveOnContentServer(Server.MapPath(SearchProductIndexImagePath) + strImageName);
                                    }
                                    catch
                                    {

                                    }
                                }
                                //else
                                //{
                                //    ViewState["DelHomeImage"] = null;
                                //}
                            }
                        }
                        else if (!string.IsNullOrEmpty(Imgname.ToString()))
                        {
                            if (File.Exists(Server.MapPath(SearchProductIndexImagePath + Imgname)))
                                File.Move(Server.MapPath(SearchProductIndexImagePath + Imgname), Server.MapPath(SearchProductIndexImagePath + strImageName));

                            try
                            {
                                Solution.UI.Web.ADMIN.Settings.CompressimagePanda objcompress = new Solution.UI.Web.ADMIN.Settings.CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(SearchProductIndexImagePath + strImageName));
                            }
                            catch
                            {

                            }

                        }

                    }
                    catch
                    {


                    }


                    CommonComponent.ExecuteCommonData("Update tb_ProductSearchType set SearchValue='" + txtSearchName.Text.Trim().ToString().Replace("'", "''") + "',SearchType=" + ddlSearchType.SelectedValue + ",Active=" + Active + ",price=" + Price + ",PerInch=" + PerInch + ",ImageName='" + strImageName + "',DisplayOrder=" + DisplayOrder + " where SearchId = " + Convert.ToString(Request.QueryString["SearchTypeID"]) + "");

                    CommonComponent.ExecuteCommonData("Update tb_ProductSearchType set SEKeywords='" + txtSEKeyword.Text.Trim().ToString().Replace("'", "''") + "',SEDescription='" + txtSEDescription.Text.Trim().ToString().Replace("'", "''") + "',SETitle='" + txtSETitle.Text.Trim().ToString().Replace("'", "''") + "' where SearchId = " + Convert.ToString(Request.QueryString["SearchTypeID"]) + "");
                    CommonComponent.ExecuteCommonData("Update tb_ProductSearchType set PageTitle='" + txtPageTitle.Text.Trim().ToString().Replace("'", "''") + "',PageDescription='" + txtPageDescription.Text.Trim().ToString().Replace("'", "''") + "' where SearchId = " + Convert.ToString(Request.QueryString["SearchTypeID"]) + "");
                    Response.Redirect("SearchProductTypeList.aspx?status=updated");
                }
            }
            else
            {
                int chkDuplicate = Convert.ToInt32(CommonComponent.GetScalarCommonData("Select ISNULL(COUNT(*),0) as TotCnt from tb_ProductSearchType where SearchValue='" + txtSearchName.Text.Trim().ToString() + "' and SearchType= " + ddlSearchType.SelectedValue + " and ISNULL(Active,0)=1 AND isnull(Deleted,0)=0 "));
                if (chkDuplicate > 0)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Search Product Value already exists.', 'Message','');});", true);
                    return;
                }
                else
                {
                    Int32 SearchId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Insert into tb_ProductSearchType (SearchValue,SearchType,Price,PerInch,ImageName,Deleted,Createdon,Active,DisplayOrder) values('" + txtSearchName.Text.Trim().ToString().Replace("'", "''") + "'," + ddlSearchType.SelectedValue + "," + Price + "," + PerInch + ",'',0,Getdate()," + Active + "," + DisplayOrder + ") Select Scope_identity();"));
                    if (SearchId > 0)
                    {

                        CommonComponent.ExecuteCommonData("Update tb_ProductSearchType set SEKeywords='" + txtSEKeyword.Text.Trim().ToString().Replace("'", "''") + "',SEDescription='" + txtSEDescription.Text.Trim().ToString().Replace("'", "''") + "',SETitle='" + txtSETitle.Text.Trim().ToString().Replace("'", "''") + "' where SearchId = " + SearchId + "");
                        CommonComponent.ExecuteCommonData("Update tb_ProductSearchType set PageTitle='" + txtPageTitle.Text.Trim().ToString().Replace("'", "''") + "',PageDescription='" + txtPageDescription.Text.Trim().ToString().Replace("'", "''") + "' where SearchId = " + SearchId + "");
                        string strImageName = "";
                        if (!string.IsNullOrEmpty(txtSearchName.Text.ToString().Trim()))
                        {
                            strImageName = RemoveSpecialCharacter(txtSearchName.Text.ToString().ToCharArray()) + "_" + SearchId + ".jpg";
                            SaveImage(strImageName);
                        }
                        CommonComponent.ExecuteCommonData("Update tb_ProductSearchType set ImageName='" + strImageName + "' where SearchId = " + SearchId + "");
                        if (Request.QueryString["ProductStoreID"] != null && Request.QueryString["ProductID"] != null)
                        {
                            if (Request.QueryString["SearchColor"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchColor=Color");
                            }
                            else if (Request.QueryString["SearchPattern"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchPattern=Pattern");
                            }
                            else if (Request.QueryString["SearchFabric"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchFabric=Fabric");
                            }
                            else if (Request.QueryString["SearchFeature"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchFeature=Feature");
                            }
                            else if (Request.QueryString["SearchStyle"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchStyle=Style");
                            }
                            else if (Request.QueryString["SearchHeader"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchHeader=Header");
                            }
                            else if (Request.QueryString["SearchCustomCalculator"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchCustomCalculator=Style");
                            }
                            else if (Request.QueryString["SearchOptionCalculator"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchOptionCalculator=option");
                            }

                            else if (Request.QueryString["SearchSecondaryColorStyle"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&ID=" + Request.QueryString["ProductID"].ToString() + "&Mode=edit&SearchSecondaryColorStyle=Color");
                            }
                        }
                        else if (Request.QueryString["ProductStoreID"] != null)
                        {
                            if (Request.QueryString["SearchColor"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchColor=Color");
                            }
                            else if (Request.QueryString["SearchPattern"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchPattern=Pattern");
                            }
                            else if (Request.QueryString["SearchFabric"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchFabric=Fabric");
                            }
                            else if (Request.QueryString["SearchFeature"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchFeature=Feature");
                            }
                            else if (Request.QueryString["SearchStyle"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchStyle=Style");
                            }
                            else if (Request.QueryString["SearchHeader"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchHeader=Header");
                            }
                            else if (Request.QueryString["SearchCustomCalculator"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchCustomCalculator=Style");
                            }
                            else if (Request.QueryString["SearchOptionCalculator"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchOptionCalculator=Option");
                            }

                            else if (Request.QueryString["SearchSecondaryColorStyle"] != null)
                            {
                                Response.Redirect("Product.aspx?StoreID=" + Request.QueryString["ProductStoreID"].ToString() + "&SearchSecondaryColorStyle=Color");
                            }

                        }
                        else
                        {
                            Response.Redirect("SearchProductTypeList.aspx?status=inserted");
                        }
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
            SearchProductTempPath = string.Concat(AppLogic.AppConfigs("SecondaryColorImages"), "Temp/");
            SearchProductPath = string.Concat(AppLogic.AppConfigs("SecondaryColorImages"));

            //create Micro folder 
            if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                Directory.CreateDirectory(Server.MapPath(SearchProductPath));

            CommonOperations.SaveOnContentServer(Server.MapPath(SearchProductPath));
            if (ImgLarge.Src.Contains(SearchProductTempPath))
            {
                try
                {
                    CreateImage("icon", FileName);
                    if (ddlSearchType.SelectedItem.Text.ToString().ToLower().IndexOf("color") > -1 || ddlSearchType.SelectedItem.Text.ToString().ToLower().IndexOf("pattern") > -1)
                    {
                        CreateImage("medium", FileName);
                    }
                }
                catch (Exception ex)
                {
                }
                finally
                {
                    //DeleteTempFile("icon");
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
                    case "medium":
                        SearchProductPath = SearchProductPath + "Medium/";
                        if (!Directory.Exists(Server.MapPath(SearchProductPath)))
                            Directory.CreateDirectory(Server.MapPath(SearchProductPath));

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
                    if (ddlSearchType.SelectedItem.Text.ToString().ToLower().IndexOf("header") > -1)
                    {
                        finHeight = 29;
                        finWidth = 123;
                    }

                    else
                    {
                        finHeight = thumbNailSizeIcon.Height;
                        finWidth = thumbNailSizeIcon.Width;
                    }
                    break;
                case "medium":
                    if (ddlSearchType.SelectedItem.Text.ToString().ToLower().IndexOf("color") > -1)
                    {
                        finHeight = 120;
                        finWidth = 120;
                    }
                    else if (ddlSearchType.SelectedItem.Text.ToString().ToLower().IndexOf("pattern") > -1)
                    {
                        finHeight = 116;
                        finWidth = 216;
                    }
                    else
                    {
                        finHeight = thumbNailSizeIcon.Height;
                        finWidth = thumbNailSizeIcon.Width;
                    }
                    break;

            }
            ResizeImage(strFile, finWidth, finHeight, strFilePath);
        }

        private void BindSize()
        {
            AppConfig.StoreID = 1;
            DataSet dsIconWidth = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "SearchProductIconWidth");
            DataSet dsIconHeight = objConfiguration.GetImageSizeByType(AppConfig.StoreID, "SearchProductIconHeight");
            if ((dsIconWidth != null && dsIconWidth.Tables.Count > 0 && dsIconWidth.Tables[0].Rows.Count > 0) && (dsIconHeight != null && dsIconHeight.Tables.Count > 0 && dsIconHeight.Tables[0].Rows.Count > 0))
            {
                thumbNailSizeIcon = new Size(Convert.ToInt32(dsIconWidth.Tables[0].Rows[0]["Size"].ToString().Trim()), Convert.ToInt32(dsIconHeight.Tables[0].Rows[0]["Size"].ToString().Trim()));
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
                //float resizePercentHeight = 0;
                //float resizePercentWidth = 0;
                //resizePercentHeight = (FinHeight * 100) / imgVisol.Height;
                //resizePercentWidth = (FinWidth * 100) / imgVisol.Width;
                //if (resizePercentHeight < resizePercentWidth)
                //{
                //    resizedHeight = FinHeight;
                //    resizedWidth = (int)Math.Round(resizePercentHeight * imgVisol.Width / 100.0);
                //}
                //if (resizePercentHeight >= resizePercentWidth)
                //{
                //    resizedWidth = FinWidth;
                //    resizedHeight = (int)Math.Round(resizePercentWidth * imgVisol.Height / 100.0);
                //}
                resizedWidth = FinWidth;
                resizedHeight = FinHeight;
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
            SearchProductTempPath = string.Concat(AppLogic.AppConfigs("SecondaryColorImages"), "Temp/");
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
        protected void btnUploadHomeImage_Click(object sender, ImageClickEventArgs e)
        {
            bool Flag = false;
            StringArrayConverter Storeconvertor = new StringArrayConverter();
            string sImageextension = AppLogic.AppConfigs("AllowedExtensions");
            Array StoreArray = (Array)Storeconvertor.ConvertFrom(sImageextension);

            if (!Directory.Exists(Server.MapPath(SearchProductIndexImagePath)))
                Directory.CreateDirectory(Server.MapPath(SearchProductIndexImagePath));



            if (!Directory.Exists(Server.MapPath(SearchProductTempIndexImagePath)))
                Directory.CreateDirectory(Server.MapPath(SearchProductTempIndexImagePath));
            for (int j = 0; j < StoreArray.Length; j++)
                if (FileUploadhomeImage.FileName.Length > 0 && Path.GetExtension(FileUploadhomeImage.FileName.ToString().ToLower()) == StoreArray.GetValue(j).ToString().ToLower())
                    Flag = true;
            if (Flag)
            {
                if (FileUploadhomeImage.FileName.Length > 0)
                {
                    ViewState["DelHomeImage"] = FileUploadhomeImage.FileName.ToString();
                    FileUploadhomeImage.SaveAs(Server.MapPath(SearchProductTempIndexImagePath) + FileUploadhomeImage.FileName);
                    ImgIndexImage.Src = SearchProductTempIndexImagePath + FileUploadhomeImage.FileName;

                }
                else
                {
                    ViewState["DelHomeImage"] = null;
                }
            }
            else
            {
                string StrMsg = "Only " + AppLogic.AppConfigs("AllowedExtensions") + " Images are allowed";
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "ImageMsg", "$(document).ready( function() {jAlert('" + StrMsg.ToString() + "', 'Message','ContentPlaceHolder1_FileUploadhomeImage');});", true);
            }
        }

        protected void btnDeleteHomeImage_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (File.Exists(Server.MapPath(SearchProductIndexImagePath + ViewState["DelHomeImage"].ToString())))
                    File.Delete(Server.MapPath(SearchProductIndexImagePath + ViewState["DelHomeImage"].ToString()));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(SearchProductIndexImagePath + ViewState["DelHomeImage"].ToString()));
            }
            catch { }
            ViewState["DelHomeImage"] = null;
            Response.Cache.SetExpires(DateTime.Now - TimeSpan.FromDays(1));
            ImgIndexImage.Src = SearchProductIndexImagePath + "image_not_available.jpg";
            btnDeleteHomeImage.Visible = false;
        }
    }
}