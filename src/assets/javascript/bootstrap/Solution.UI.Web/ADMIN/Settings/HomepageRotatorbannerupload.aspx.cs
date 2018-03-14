﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using System.IO;
using System.Net;


namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class HomepageRotatorbannerupload : BasePage
    {
        public Int32 GroupRotaterid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                imgSave1.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                //imgCancel1.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                imgSave2.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                // imgCancel2.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                imgSave3.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgSave4.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgback.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/back.png";
                imgDateSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";

                // imgCancel3.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (Request.UrlReferrer != null)
                {
                    ViewState["urlrefferer"] = Request.UrlReferrer.ToString();
                }
                HomeBannerComponent objHome = new HomeBannerComponent();
                ViewState["Imagebannrpath"] = CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='ImagePathBanner' AND storeid=" + Request.QueryString["Storeid"].ToString() + " AND isnull(Deleted,0)=0");
                if (Request.QueryString["grouptype"] != null && Request.QueryString["grouptype"].ToString() == "new")
                {
                    if (Request.QueryString["hid"] == null)
                    {
                        GroupRotaterid = Convert.ToInt32(objHome.InsertRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 1));
                    }

                }
                else
                {
                    if (Request.QueryString["hid"] != null && Request.QueryString["hid"].ToString() != null)
                    {
                        GroupRotaterid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select HomeRotatorId from tb_RotatorHomeBannerDetail where BannerID=" + Request.QueryString["hid"].ToString() + ""));
                    }

                }
                Getbanner();
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {
                    lblImgSize.Text = "Size should be 1920 x 700 ";
                    lblImgSizeMobile.Text = "Size should be 640 x 800";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
                {
                    lblImgSize.Text = "Size should be 1050 x 578 ";
                    //lblImgSize1.Text = "Size should be 713 x 310  ";
                    //lblImgSize2.Text = "Size should be 713 x 310 ";
                    //lblImgSize3.Text = "Size should be 380 x 115  ";
                    lblImgSize1.Text = "Size should be 510 x 578  ";
                    lblImgSize2.Text = "Size should be 510 x 578 ";
                    lblImgSize3.Text = "Size should be 510 x 578  ";
                    lblImgSize4.Text = "Size should be 510 x 578  ";


                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
                {
                    lblImgSize.Text = "Size should be 590 x 377 ";
                    lblImgSize1.Text = "Size should be 446 x 623 ";
                    lblImgSize2.Text = "Size should be 446 x 623 ";
                    lblImgSize3.Text = "Size should be 446 x 623 ";
                }

            }
            else
            {
                if (Request.QueryString["grouptype"] != null && Request.QueryString["grouptype"].ToString() == "new")
                {
                    if (Request.QueryString["hid"] == null)
                    {
                        GroupRotaterid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 HomeRotatorId from tb_RotatorHomebanner order by CreatedOn desc"));
                    }

                }
                else
                {
                    if (Request.QueryString["hid"] != null && Request.QueryString["hid"].ToString() != null)
                    {
                        GroupRotaterid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select HomeRotatorId from tb_RotatorHomeBannerDetail where BannerID=" + Request.QueryString["hid"].ToString() + ""));
                    }

                }
            }
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidesmall", "document.getElementById('tdSmallbanner').style.display='none';", true);
            }
        }
        private Boolean CheckImageSize(FileUpload ff, int width, int height)
        {



            try
            {
                if (!Directory.Exists(Server.MapPath("/Resources/temp")))
                {
                    Directory.CreateDirectory(Server.MapPath("/Resources/temp"));

                }
                FileInfo file = new FileInfo(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));
                //if (File.Exists(file))
                //{
                //    File.Delete(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                //}
                if (file.Exists)
                {
                    file.Delete();
                }

                ff.SaveAs(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                System.Drawing.Image img = System.Drawing.Image.FromFile(Server.MapPath("/Resources/temp" + "/" + ff.FileName.ToString()));

                if (img.Width == width && img.Height == height)
                {
                    img.Dispose();
                    return true;

                }
                else
                {
                    img.Dispose();
                    return false;
                }
            }
            catch
            {

            }

            return false;
        }
        private void Getbanner()
        {
            imgDateSave.Visible = true;
            HomeBannerComponent objbanner = new HomeBannerComponent();
            DataSet dsbanner = new DataSet();

            String Displayorder = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(DisplayOrder,0) as DisplayOrder from tb_RotatorHomebanner where HomeRotatorId=" + GroupRotaterid + ""));
            txtgroupdisplayorder.Text = Displayorder;
            dsbanner = objbanner.GetHomeRotatorBanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), GroupRotaterid);
            ViewState["dsbanner"] = dsbanner;
            Int32 j = 0;
            if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
            {


                grdbannerlist.DataSource = dsbanner;
                grdbannerlist.DataBind();

                if (Request.QueryString["hid"] != null)
                {
                    DataRow[] drmain = dsbanner.Tables[0].Select("isnull(ismain,0) =1 AND BannerId=" + Request.QueryString["hid"].ToString() + "");
                    if (drmain != null && drmain.Length > 0)
                    {
                        foreach (DataRow dr1 in drmain)
                        {
                            if (txtStartDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["startDate"].ToString())))
                            {
                                txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["startDate"].ToString())));
                            }
                            if (txtEndDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["EndDate"].ToString())))
                            {
                                txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["EndDate"].ToString())));
                            }
                            TxtBannerURL.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            TxtDisplayOrder.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            ddlTarget.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            chkActive.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            imghdn.Value = Convert.ToString(dr1["ImageName"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdn.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner.Src = ViewState["Imagebannrpath"].ToString() + imghdn.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            else
                            {
                                imgBanner.Src = "/App_Themes/" + Page.Theme + "/images/spacer.png";
                            }
                            hdnid.Value = Convert.ToString(dr1["BannerID"].ToString());


                            imghdnMobile.Value = Convert.ToString(dr1["ImageName"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdnMobile.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBannerMobile.Src = ViewState["Imagebannrpath"].ToString() +"mobile/" + imghdnMobile.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            else
                            {
                                imgBannerMobile.Src = "/App_Themes/" + Page.Theme + "/images/spacer.png";
                            }
                            hdnidMobile.Value = Convert.ToString(dr1["BannerID"].ToString());

                            TxtbannerTitle.Text = Convert.ToString(dr1["Title"].ToString());
                            txtLeftBannerText.Text = Convert.ToString(dr1["LeftBannerText"].ToString());
                            ddlPosition.SelectedValue = Convert.ToString(dr1["Pagination"].ToString());
                        }
                    }
                }
                else
                {
                    DataRow[] drmain = dsbanner.Tables[0].Select("isnull(ismain,0) =1 ");
                    if (drmain != null && drmain.Length > 0)
                    {
                        foreach (DataRow dr1 in drmain)
                        {
                            if (txtStartDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["startDate"].ToString())))
                            {
                                txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["startDate"].ToString())));
                            }
                            if (txtEndDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["EndDate"].ToString())))
                            {
                                txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["EndDate"].ToString())));
                            }
                            break;
                        }
                    }
                }



                DataRow[] dr = dsbanner.Tables[0].Select("isnull(ismain,0) =0");
                if (dr != null && dr.Length > 0)
                {


                    foreach (DataRow dr1 in dr)
                    {
                        j++;
                        if (j == 1)
                        {
                            if (txtStartDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["startDate"].ToString())))
                            {
                                txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["startDate"].ToString())));
                            }
                            if (txtEndDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["EndDate"].ToString())))
                            {
                                txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["EndDate"].ToString())));
                            }

                            TxtBannerURL1.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            TxtDisplayOrder1.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            ddlTarget1.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            chkActive1.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            imghdn1.Value = Convert.ToString(dr1["ImageName"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdn1.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner1.Src = ViewState["Imagebannrpath"].ToString() + imghdn1.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            hdnid1.Value = Convert.ToString(dr1["BannerID"].ToString());
                            TxtbannerTitle1.Text = Convert.ToString(dr1["Title"].ToString());
                            imgDateSave.Visible = true;
                        }
                        if (j == 2)
                        {
                            if (txtStartDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["startDate"].ToString())))
                            {
                                txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["startDate"].ToString())));
                            }
                            if (txtEndDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["EndDate"].ToString())))
                            {
                                txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["EndDate"].ToString())));
                            }
                            TxtBannerURL2.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            TxtDisplayOrder2.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            ddlTarget2.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            chkActive2.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            imghdn2.Value = Convert.ToString(dr1["ImageName"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdn2.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner2.Src = ViewState["Imagebannrpath"].ToString() + imghdn2.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            hdnid2.Value = Convert.ToString(dr1["BannerID"].ToString());
                            TxtbannerTitle2.Text = Convert.ToString(dr1["Title"].ToString());
                            imgDateSave.Visible = true;
                        }
                        if (j == 3)
                        {
                            if (txtStartDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["startDate"].ToString())))
                            {
                                txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["startDate"].ToString())));
                            }
                            if (txtEndDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["EndDate"].ToString())))
                            {
                                txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["EndDate"].ToString())));
                            }
                            TxtBannerURL3.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            TxtDisplayOrder3.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            ddlTarget3.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            chkActive3.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            imghdn3.Value = Convert.ToString(dr1["ImageName"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdn3.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner3.Src = ViewState["Imagebannrpath"].ToString() + imghdn3.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            hdnid3.Value = Convert.ToString(dr1["BannerID"].ToString());
                            TxtbannerTitle3.Text = Convert.ToString(dr1["Title"].ToString());
                            imgDateSave.Visible = true;
                        }
                        if (j == 4)
                        {
                            if (txtStartDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["startDate"].ToString())))
                            {
                                txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["startDate"].ToString())));
                            }
                            if (txtEndDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["EndDate"].ToString())))
                            {
                                txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["EndDate"].ToString())));
                            }
                            TxtBannerURL4.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            TxtDisplayOrder4.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            ddlTarget4.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            chkActive4.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            imghdn4.Value = Convert.ToString(dr1["ImageName"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdn4.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner4.Src = ViewState["Imagebannrpath"].ToString() + imghdn4.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            hdnid4.Value = Convert.ToString(dr1["BannerID"].ToString());
                            TxtbannerTitle4.Text = Convert.ToString(dr1["Title"].ToString());
                            imgDateSave.Visible = true;
                        }
                    }

                }
            }
            else
            {
                grdbannerlist.DataSource = null;
                grdbannerlist.DataBind();

            }
        }
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["dsbanner"] != null && hdnid.Value.ToString().Trim() == "0")
            {

                DataSet dsbanner = new DataSet();
                dsbanner = (DataSet)ViewState["dsbanner"];
                if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = dsbanner.Tables[0].Select("isnull(ismain,0) =1 AND Active=1");
                    if (dr != null && dr.Length > 4)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgforactive", "jAlert('You can not upload more then 5 banner!','Information')", true);
                        return;
                    }
                }
            }

            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = 0;
            //Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebanner WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
            //if (Rotatrid > 0)
            //{
            //    if (Request.QueryString["hid"] == null)
            //    {
            //        objHome.UpdateRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
            //    }
            //}
            //else
            //{

            //if(Request.QueryString["hid"]==null)
            //{
            //    Rotatrid = Convert.ToInt32(objHome.InsertRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 1));
            //}
            //else
            //{
            //    if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString()!="")
            //    {
            //        Rotatrid = Convert.ToInt32(Request.QueryString["id"].ToString());
            //    }
            //}

            // GroupRotaterid = Rotatrid;
            //}
            tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            if (chkActive.Checked)
            {
                objRotatorhome.Active = true;
            }
            else
            {
                objRotatorhome.Active = false;
            }
            objRotatorhome.BannerURL = TxtBannerURL.Text.ToString();
            objRotatorhome.Title = TxtbannerTitle.Text.ToString();
            //if(!String.IsNullOrEmpty(txtLeftBannerText.Text))
            //{
            //objRotatorhome.LeftBannerText = Convert.ToString(txtLeftBannerText.Text);
            //}
            if (TxtDisplayOrder.Text.ToString().Trim() == "")
            {
                objRotatorhome.DisplayOrder = 0;
            }
            else
            {
                objRotatorhome.DisplayOrder = Convert.ToInt32(TxtDisplayOrder.Text.ToString());
            }
            objRotatorhome.IsMain = 1;
            objRotatorhome.HomeRotatorId = GroupRotaterid;
            objRotatorhome.StoreID = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            objRotatorhome.Pagination = ddlPosition.SelectedValue.ToString();
            objRotatorhome.LinkTarget = ddlTarget.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                objRotatorhome.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            }
            if (!String.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                objRotatorhome.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            }

            if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));
            string filename = "";
            if (FileUploadBanner.HasFile)
            {
                bool checksize = false;
                string ErrorMessage = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {
                    checksize = CheckImageSize(FileUploadBanner, 1920, 700);
                    ErrorMessage = "Please upload banner1 of Size 1920 x 700 ";
                }

                if (checksize == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "jAlert('" + ErrorMessage + "','Message')", true);
                    return;
                }

                
                FileInfo fl = new FileInfo(FileUploadBanner.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));


                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        break;
                    }
                }

                imgBanner.Src = ViewState["Imagebannrpath"].ToString() + "/" + filename;
            }
            else
            {
                objRotatorhome.ImageName = imghdn.Value.ToString();
            }


            if (FileUploadBannerMobile.HasFile)
            {
                bool checksize = false;
                string ErrorMessage = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {
                    checksize = CheckImageSize(FileUploadBannerMobile, 640, 800);
                    ErrorMessage = "Please upload banner1 of Size 640 x 800 ";
                }

                if (checksize == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "jAlert('" + ErrorMessage + "','Message')", true);
                    return;
                }

                //string filename = "";
                FileInfo fl = new FileInfo(FileUploadBannerMobile.FileName.ToString());

                //for (int i = 1; i < 1000; i++)
                {
                   // filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename)))
                    {
                        if (filename == imghdn.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename));

                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename));
                            }
                            catch { }
                            FileUploadBannerMobile.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename));
                            //break;
                        }
                    }
                    else
                    {
                        FileUploadBannerMobile.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename));
                       // break;
                    }
                }

                imgBannerMobile.Src = ViewState["Imagebannrpath"].ToString() + "/mobile/" + filename;
            }


            Int32 rId = 0;
            if (hdnid.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;

                rId = objHome.Insertbanner(objRotatorhome);
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid.Value.ToString());
                objHome.UpdatebannerDetail(objRotatorhome);
                rId = Convert.ToInt32(hdnid.Value.ToString());

            }

            CommonComponent.ExecuteCommonData("update tb_RotatorHomeBannerDetail set LeftBannerText='" + txtLeftBannerText.Text.ToString().Replace("'", "''") + "' where BannerID=" + rId + "");

            hdnid.Value = "0";
            //imghdn.Value = "";
            //TxtbannerTitle.Text = "";
            //TxtBannerURL.Text = "";
            //ddlPosition.SelectedIndex = 0;
            //ddlTarget.SelectedIndex = 0;
            //TxtDisplayOrder.Text = "";
            //txtLeftBannerText.Text = "";
            //chkActive.Checked = false;
            BindBanner();
            lblMsg.Text = "Record Saved Successfully.";
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
            {
                Response.Redirect("HomePagebannerlist.aspx");

            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "")
                {
                    Response.Redirect("HomePagebannerlist.aspx");
                }
            }

            //  imgBanner.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/spacer.png";
            Getbanner();
            GetHTMl();
        }
        protected void imgSave1_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebanner WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
            //if (Rotatrid > 0)
            //{
            //    if (Request.QueryString["hid"] == null)
            //    {
            //        objHome.UpdateRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
            //    }
            //}
            //else
            //{

            //    Rotatrid = Convert.ToInt32(objHome.InsertRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 1));
            //}
            tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            if (chkActive1.Checked)
            {
                objRotatorhome.Active = true;
            }
            else
            {
                objRotatorhome.Active = false;
            }
            objRotatorhome.Title = TxtbannerTitle1.Text.ToString();
            objRotatorhome.BannerURL = TxtBannerURL1.Text.ToString();
            if (TxtDisplayOrder1.Text.ToString().Trim() == "")
            {
                objRotatorhome.DisplayOrder = 0;
            }
            else
            {
                objRotatorhome.DisplayOrder = Convert.ToInt32(TxtDisplayOrder1.Text.ToString());
            }
            objRotatorhome.IsMain = 0;
            objRotatorhome.HomeRotatorId = GroupRotaterid;
            objRotatorhome.StoreID = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            objRotatorhome.Pagination = "";
            objRotatorhome.LinkTarget = ddlTarget1.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                objRotatorhome.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            }
            if (!String.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                objRotatorhome.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            }

            if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner1.HasFile)
            {
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner1.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn1.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner1.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner1.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        break;
                    }
                }

                imgBanner1.Src = ViewState["Imagebannrpath"].ToString() + "/" + filename;
            }
            else
            {
                objRotatorhome.ImageName = imghdn1.Value.ToString();
            }
            Int32 rid = 0;
            if (hdnid1.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;
                rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid1.Value.ToString());
                objHome.UpdatebannerDetail(objRotatorhome);

            }
            hdnid1.Value = "0";
            imghdn1.Value = "";

            TxtBannerURL1.Text = "";

            ddlTarget1.SelectedIndex = 0;
            TxtDisplayOrder1.Text = "";
            chkActive1.Checked = false;
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "")
                {
                    BindBanner();
                    Response.Redirect("HomePagebannerlist.aspx");
                }
            }
            BindBanner();
            lblMsg1.Text = "Record Saved Successfully.";
            Getbanner();
            GetHTMl();

        }
        protected void imgSave2_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebanner WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
            //if (Rotatrid > 0)
            //{
            //    if (Request.QueryString["hid"] == null)
            //    {
            //        objHome.UpdateRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
            //    }
            //}
            //else
            //{

            //    Rotatrid = Convert.ToInt32(objHome.InsertRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 1));
            //}
            tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            if (chkActive2.Checked)
            {
                objRotatorhome.Active = true;
            }
            else
            {
                objRotatorhome.Active = false;
            }
            objRotatorhome.Title = TxtbannerTitle2.Text.ToString();
            objRotatorhome.BannerURL = TxtBannerURL2.Text.ToString();

            if (TxtDisplayOrder2.Text.ToString().Trim() == "")
            {
                objRotatorhome.DisplayOrder = 0;
            }
            else
            {
                objRotatorhome.DisplayOrder = Convert.ToInt32(TxtDisplayOrder2.Text.ToString());
            }
            objRotatorhome.IsMain = 0;
            objRotatorhome.HomeRotatorId = GroupRotaterid;
            objRotatorhome.StoreID = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            objRotatorhome.Pagination = "";
            objRotatorhome.LinkTarget = ddlTarget2.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                objRotatorhome.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            }
            if (!String.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                objRotatorhome.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            }

            if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner2.HasFile)
            {
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner2.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn2.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner2.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner2.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        break;
                    }
                }
            }
            else
            {
                objRotatorhome.ImageName = imghdn2.Value.ToString();
            }
            Int32 rid = 0;
            if (hdnid2.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;
                rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid2.Value.ToString());
                objHome.UpdatebannerDetail(objRotatorhome);

            }
            hdnid2.Value = "0";
            imghdn2.Value = "";

            TxtBannerURL2.Text = "";

            ddlTarget2.SelectedIndex = 0;
            TxtDisplayOrder2.Text = "";
            chkActive2.Checked = false;
            lblMsg2.Text = "Record Saved Successfully.";
            Getbanner();
            GetHTMl();
        }

        protected void imgSave3_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebanner WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
            //if (Rotatrid > 0)
            //{
            //    if (Request.QueryString["hid"] == null)
            //    {
            //        objHome.UpdateRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
            //    }
            //}
            //else
            //{

            //    Rotatrid = Convert.ToInt32(objHome.InsertRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 1));
            //}
            tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            if (chkActive3.Checked)
            {
                objRotatorhome.Active = true;
            }
            else
            {
                objRotatorhome.Active = false;
            }
            objRotatorhome.Title = TxtbannerTitle3.Text.ToString();
            objRotatorhome.BannerURL = TxtBannerURL3.Text.ToString();
            if (TxtDisplayOrder3.Text.ToString().Trim() == "")
            {
                objRotatorhome.DisplayOrder = 0;
            }
            else
            {
                objRotatorhome.DisplayOrder = Convert.ToInt32(TxtDisplayOrder3.Text.ToString());
            }
            objRotatorhome.IsMain = 0;
            objRotatorhome.HomeRotatorId = GroupRotaterid;
            objRotatorhome.StoreID = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            objRotatorhome.Pagination = "";
            objRotatorhome.LinkTarget = ddlTarget3.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                objRotatorhome.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            }
            if (!String.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                objRotatorhome.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            }

            if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner3.HasFile)
            {
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner3.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn3.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner3.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner3.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        break;
                    }
                }
            }
            else
            {
                objRotatorhome.ImageName = imghdn3.Value.ToString();
            }
            Int32 rid = 0;
            if (hdnid3.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;
                rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid3.Value.ToString());
                objHome.UpdatebannerDetail(objRotatorhome);

            }

            hdnid3.Value = "0";
            imghdn3.Value = "";

            TxtBannerURL3.Text = "";
            ddlTarget3.SelectedIndex = 0;
            TxtDisplayOrder3.Text = "";
            chkActive3.Checked = false;
            Getbanner();
            lblMsg3.Text = "Record Saved Successfully.";
            GetHTMl();
        }
        protected void imgSave4_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebanner WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
            //if (Rotatrid > 0)
            //{
            //    if (Request.QueryString["hid"] == null)
            //    {
            //        objHome.UpdateRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()));
            //    }
            //}
            //else
            //{

            //    Rotatrid = Convert.ToInt32(objHome.InsertRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 1));
            //}
            tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            if (chkActive4.Checked)
            {
                objRotatorhome.Active = true;
            }
            else
            {
                objRotatorhome.Active = false;
            }
            objRotatorhome.Title = TxtbannerTitle4.Text.ToString();
            objRotatorhome.BannerURL = TxtBannerURL4.Text.ToString();

            if (TxtDisplayOrder4.Text.ToString().Trim() == "")
            {
                objRotatorhome.DisplayOrder = 0;
            }
            else
            {
                objRotatorhome.DisplayOrder = Convert.ToInt32(TxtDisplayOrder4.Text.ToString());
            }
            objRotatorhome.IsMain = 0;
            objRotatorhome.HomeRotatorId = GroupRotaterid;
            objRotatorhome.StoreID = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            objRotatorhome.Pagination = "";
            objRotatorhome.LinkTarget = ddlTarget4.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                objRotatorhome.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            }
            if (!String.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                objRotatorhome.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            }

            if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner4.HasFile)
            {
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner4.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn4.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner4.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner4.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        break;
                    }
                }
            }
            else
            {
                objRotatorhome.ImageName = imghdn4.Value.ToString();
            }
            Int32 rid = 0;
            if (hdnid4.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;
                rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid4.Value.ToString());
                objHome.UpdatebannerDetail(objRotatorhome);

            }
            hdnid4.Value = "0";
            imghdn4.Value = "";

            TxtBannerURL4.Text = "";

            ddlTarget4.SelectedIndex = 0;
            TxtDisplayOrder4.Text = "";
            chkActive4.Checked = false;
            lblMsg4.Text = "Record Saved Successfully.";
            Getbanner();
            GetHTMl();
        }
        private void GetHTMl()
        {
            string strHTML = "";
            HomeBannerComponent objbanner = new HomeBannerComponent();
            DataSet dsbanner = new DataSet();

            dsbanner = objbanner.GetHomeRotatorBanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["id"].ToString()));
            string strPosition = "";
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
            {
                strHTML += "<div id=\"banner-part\">";
                strHTML += "<div class=\"main-banner\">";
                strHTML += "<div id=\"slider1\" class=\"contentslide\">";
                strHTML += "<div class=\"opacitylayer\">";
                if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = dsbanner.Tables[0].Select("isnull(IsMain,0)=1");
                    foreach (DataRow dr1 in dr)
                    {
                        strPosition = dr1["Pagination"].ToString();
                        if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
                        {
                            strHTML += "<div class=\"contentdiv\">";
                            strHTML += "<div class=\"index-banner-1\">";
                            if (dr1["BannerURL"].ToString().Trim() != "")
                            {
                                strHTML += " <div class=\"banner1\"><a href=\"" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img style=\"cursor:pointer;\"  src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></a></div>";
                            }
                            else
                            {
                                strHTML += " <div class=\"banner1\"><img  src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></div>";
                            }
                            strHTML += " </div>";
                            strHTML += " </div>";
                        }
                    }

                }
                strHTML += " </div>";
                strHTML += " </div>";
                strHTML += " </div>";

                if (strPosition.ToString().ToLower() == "left")
                {
                    strHTML += " <div class=\"pagination\" id=\"paginate-slider1\"></div>";
                }
                else if (strPosition.ToString().ToLower() == "right")
                {
                    strHTML += " <div class=\"pagination-right\" id=\"paginate-slider1\"></div>";
                }
                else if (strPosition.ToString().ToLower() == "top")
                {
                    strHTML += " <div class=\"pagination-top\" id=\"paginate-slider1\"></div>";
                }
                else if (strPosition.ToString().ToLower() == "bottom")
                {
                    strHTML += " <div class=\"pagination-bottom\" id=\"paginate-slider1\"></div>";
                }
                strHTML += " <div id=\"play-btn\" class=\"play\"> <a href=\"javascript:ContentSlider(slider1,8000);\">&nbsp;</a> </div>";
                strHTML += " <div id=\"paginate-new\" class=\"pause\"><a href=\"javascript:cancelautorun();\">&nbsp;</a></div>";
                strHTML += " </div>";
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
            {

                strHTML += "<div id=\"banner-part\">";
                strHTML += "<div class=\"main-banner\">";
                strHTML += "<div id=\"slider1\" class=\"contentslide\">";
                strHTML += "<div class=\"opacitylayer\">";

                if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = dsbanner.Tables[0].Select("isnull(IsMain,0)=1");
                    foreach (DataRow dr1 in dr)
                    {
                        strPosition = dr1["Pagination"].ToString();
                        if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
                        {
                            strHTML += "<div class=\"contentdiv\">";
                            strHTML += "<div class=\"index-banner-1\">";
                            if (dr1["BannerURL"].ToString().Trim() != "")
                            {
                                strHTML += " <div class=\"banner1\"><a href=\"" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img style=\"cursor:pointer;\" src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></a></div>";
                            }
                            else
                            {
                                strHTML += " <div class=\"banner1\"><img  src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></div>";
                            }
                            strHTML += " </div>";
                            strHTML += " </div>";
                        }
                    }

                }
                strHTML += " </div>";
                strHTML += " </div>";
                strHTML += " </div>";
                if (strPosition.ToString().ToLower() == "left")
                {
                    strHTML += " <div class=\"pagination\" id=\"paginate-slider1\"></div>";
                }
                else if (strPosition.ToString().ToLower() == "right")
                {
                    strHTML += " <div class=\"pagination-right\" id=\"paginate-slider1\"></div>";
                }
                else if (strPosition.ToString().ToLower() == "top")
                {
                    strHTML += " <div class=\"pagination-top\" id=\"paginate-slider1\"></div>";
                }
                else if (strPosition.ToString().ToLower() == "bottom")
                {
                    strHTML += " <div class=\"pagination-bottom\" id=\"paginate-slider1\"></div>";
                }
                strHTML += " <div id=\"play-btn\" class=\"play\"> <a href=\"javascript:ContentSlider(slider1,8000);\">&nbsp;</a> </div>";
                strHTML += " <div id=\"paginate-new\" class=\"pause\"><a href=\"javascript:cancelautorun();\">&nbsp;</a></div>";
                strHTML += " </div>";

                DataRow[] drSmall = dsbanner.Tables[0].Select("isnull(IsMain,0)=0");
                int Count = 0;
                strHTML += " <div class=\"small-banner\">";
                foreach (DataRow dr1 in drSmall)
                {

                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
                    {
                        Count++;
                        if (Count % 2 == 0)
                        {
                            if (dr1["BannerURL"].ToString().Trim() != "")
                            {
                                strHTML += "<div class=\"index-banner-right\"> <a title=\"" + dr1["Title"].ToString() + "\" href=\"/" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></a></div>";
                            }
                            else
                            {
                                strHTML += "<div class=\"index-banner-right\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></div>";
                            }
                        }
                        else
                        {
                            if (dr1["BannerURL"].ToString().Trim() != "")
                            {
                                strHTML += "<div class=\"index-banner-left\"> <a title=\"" + dr1["Title"].ToString() + "\" href=\"/" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></a></div>";
                            }
                            else
                            {
                                strHTML += "<div class=\"index-banner-left\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></div>";
                            }
                        }
                    }
                }
                strHTML += " </div>";
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
            {

                strHTML += "<div id=\"banner\">";
                strHTML += "<div id='slider1' class='contentslide'>";

                strHTML += "<div class='opacitylayer' style='filter:progid:DXImageTransform.Microsoft.alpha(opacity=100);-moz-opacity:1; opacity: 1;'>";
                if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
                {
                    DataRow[] dr = dsbanner.Tables[0].Select("isnull(IsMain,0)=1");

                    foreach (DataRow dr1 in dr)
                    {
                        strPosition = dr1["Pagination"].ToString();
                        if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
                        {
                            strHTML += "<div class='contentdiv'>";
                            strHTML += "<div class='index_banner_row2'>";
                            if (dr1["BannerURL"].ToString().Trim() != "")
                            {
                                strHTML += " <div class='banner2'><a href=\"" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img style='cursor:pointer;'  src='" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "' alt='" + dr1["Title"].ToString() + "' Title='" + dr1["Title"].ToString() + "' /></a></div>";
                            }
                            else
                            {
                                strHTML += " <div class='banner2'><img  src='" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "' alt='" + dr1["Title"].ToString() + "' Title='" + dr1["Title"].ToString() + "' /></div>";
                            }
                            strHTML += " </div>";
                            strHTML += " </div>";
                        }
                    }

                }
                if (strPosition.ToString().ToLower() == "left")
                {
                    strHTML += " <div class='pagination-left' id='paginate-slider1' style='width:160px;'></div>";
                }
                else if (strPosition.ToString().ToLower() == "right")
                {
                    strHTML += " <div class='pagination-right' id='paginate-slider1' style='width:160px;'></div>";
                }
                else if (strPosition.ToString().ToLower() == "top")
                {
                    strHTML += " <div class='pagination-top' id='paginate-slider1' style='width:160px;'></div>";
                }
                else if (strPosition.ToString().ToLower() == "bottom")
                {
                    strHTML += " <div class='pagination-bottom' id='paginate-slider1' style='width:160px;'></div>";
                }
                strHTML += " <div id='play-btn' class='play'> <a href='javascript:ContentSlider(slider1,8000);'>&nbsp;</a> </div>";
                strHTML += " <div id='paginate-new' class='pause'><a href='javascript:cancelautorun();'>&nbsp;</a></div>";
                strHTML += " </div>";
                strHTML += " </div>";
                strHTML += " </div>";

                strHTML += "<div class=\"sub-banner\">";
                DataRow[] drSmall = dsbanner.Tables[0].Select("isnull(IsMain,0)=0");
                foreach (DataRow dr1 in drSmall)
                {
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
                    {
                        if (dr1["BannerURL"].ToString().Trim() != "")
                        {
                            strHTML += "<div class=\"sub-banner-1\"> <a title=\"" + dr1["Title"].ToString() + "\" href=\"/engravingfonts\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></a></div>";
                        }
                        else
                        {
                            strHTML += "<div class=\"sub-banner-1\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></div>";
                        }
                    }
                }
                strHTML += " </div>";
            }

            Int32 StoreId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOp 1 StoreId FROM tb_HtmlText WHERE StoreId=" + Request.QueryString["Storeid"] + " AND HtmlType=1"));
            if (StoreId > 0)
            {
                CommonComponent.ExecuteCommonData("UPDATE tb_HtmlText SET HtmlText='" + strHTML.Replace("'", "''") + "' WHERE StoreId=" + Request.QueryString["Storeid"] + " AND HtmlType=1");
            }
            else
            {
                CommonComponent.ExecuteCommonData("INSERT INTO tb_HtmlText (StoreId,HtmlText,HtmlType) values (" + Request.QueryString["Storeid"].ToString() + ",'" + strHTML.Replace("'", "''") + "',1)");
            }
        }
        protected void grdbannerlist_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.HtmlControls.HtmlInputHidden hdnActive = (System.Web.UI.HtmlControls.HtmlInputHidden)e.Row.FindControl("hdnActive");
                Literal ltStatus = (Literal)e.Row.FindControl("ltStatus");
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/edit-icon.png";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/delete-icon.png";
                if (hdnActive.Value.ToString().ToLower() == "true" || hdnActive.Value.ToString().ToLower() == "1")
                {
                    ltStatus.Text = "<span style='background-color: #58B058;color: #fff;width:50px;height:18px;font-size: 12px;line-height: 18px;padding-bottom: 3px;padding-top: 3px;'>&nbsp;&nbsp;<i class=\"icon-ok icon-white\"></i>&nbsp;&nbsp;</span>";
                }
                else
                {
                    ltStatus.Text = "<span style='background-color: #D14641;color: #fff;width:50px;height:18px;font-size: 12px;line-height: 18px;padding-bottom: 3px;padding-top: 3px;'>&nbsp;&nbsp;<i class=\"icon-remove\"></i>&nbsp;&nbsp;</span>";
                }
                if (Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "ismain")) == 0)
                {
                    e.Row.Visible = false;
                }
            }
        }

        protected void grdbannerlist_RowEditing(object sender, GridViewEditEventArgs e)
        {
            Int32 id = Convert.ToInt32(grdbannerlist.DataKeys[e.NewEditIndex].Value.ToString());
            hdnid.Value = id.ToString();
            if (ViewState["dsbanner"] != null)
            {

                DataSet dsbanner = new DataSet();
                dsbanner = (DataSet)ViewState["dsbanner"];
                if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
                {
                    grdbannerlist.DataSource = dsbanner;
                    grdbannerlist.DataBind();

                    DataRow[] dr = dsbanner.Tables[0].Select("isnull(BannerID,0) =" + id.ToString() + "");
                    if (dr != null && dr.Length > 0)
                    {
                        foreach (DataRow dr1 in dr)
                        {
                            string filename = Convert.ToString(dr1["ImageName"].ToString());
                            imghdn.Value = filename.ToString();
                            TxtbannerTitle.Text = Convert.ToString(dr1["Title"].ToString());
                            TxtBannerURL.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            TxtDisplayOrder.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            chkActive.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            ddlPosition.Text = Convert.ToString(dr1["Pagination"].ToString());
                            ddlTarget.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdn.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner.Src = ViewState["Imagebannrpath"].ToString() + imghdn.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            else
                            {
                                imgBanner.Src = "/App_Themes/" + Page.Theme + "/images/spacer.png";
                            }
                            txtLeftBannerText.Text = Convert.ToString(dr1["LeftBannerText"].ToString());
                        }
                    }
                }
            }


        }

        protected void grdbannerlist_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int32 id = Convert.ToInt32(grdbannerlist.DataKeys[e.RowIndex].Value.ToString());
            string filename = Convert.ToString(DataBinder.Eval(grdbannerlist.Rows[e.RowIndex].DataItem, "ImageName"));
            HomeBannerComponent objHome = new HomeBannerComponent();
            tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            objRotatorhome.BannerID = id;
            objHome.DeleteBanner(objRotatorhome);
            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
            {
                try
                {
                    File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                    CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                }
                catch { }
            }
            hdnid.Value = "0";
            TxtbannerTitle.Text = "";
            imghdn.Value = "";
            TxtBannerURL.Text = "";
            ddlPosition.SelectedIndex = 0;
            ddlTarget.SelectedIndex = 0;
            TxtDisplayOrder.Text = "";
            txtLeftBannerText.Text = "";
            chkActive.Checked = false;
            imgBanner.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/spacer.png";
            Getbanner();
            GetHTMl();
        }

        protected void imgback_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["urlrefferer"] != null)
            {
                Response.Redirect(ViewState["urlrefferer"].ToString());

            }
            else
            {
                Response.Redirect("HomepageRatotingbanner.aspx");
            }
        }


        protected void imgDateSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!String.IsNullOrEmpty(txtStartDate.Text.Trim()) && !String.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {
                CommonComponent.ExecuteCommonData(@"UPDATE tb_RotatorHomeBannerDetail SET  tb_RotatorHomeBannerDetail.StartDate ='" + Convert.ToDateTime(txtStartDate.Text.Trim()) + @"',tb_RotatorHomeBannerDetail.EndDate='" + Convert.ToDateTime(txtEndDate.Text.Trim()) + @"'
                                                FROM dbo.tb_RotatorHomebanner INNER JOIN
                                                dbo.tb_RotatorHomeBannerDetail ON dbo.tb_RotatorHomebanner.HomeRotatorId = dbo.tb_RotatorHomeBannerDetail.HomeRotatorId
                                                WHERE tb_RotatorHomebanner.HomeRotatorId =" + GroupRotaterid + @" AND tb_RotatorHomebanner.StoreID=" + Convert.ToString(Request.QueryString["storeid"]) + @"");
            }
            if (String.IsNullOrEmpty(txtStartDate.Text.Trim()) && String.IsNullOrEmpty(txtEndDate.Text.Trim()))
            {

                CommonComponent.ExecuteCommonData("UPDATE tb_RotatorHomeBannerDetail SET tb_RotatorHomeBannerDetail.StartDate=null,tb_RotatorHomeBannerDetail.EndDate=null FROM dbo.tb_RotatorHomebanner INNER JOIN dbo.tb_RotatorHomeBannerDetail ON dbo.tb_RotatorHomebanner.HomeRotatorId = dbo.tb_RotatorHomeBannerDetail.HomeRotatorId WHERE tb_RotatorHomebanner.HomeRotatorId =" + GroupRotaterid + " AND tb_RotatorHomebanner.StoreID=" + Convert.ToString(Request.QueryString["storeid"]) + "");
            }

            Int32 DO = 0;
            Int32.TryParse(txtgroupdisplayorder.Text, out DO);
            CommonComponent.ExecuteCommonData("update tb_RotatorHomebanner set DisplayOrder=" + DO + " where HomeRotatorId=" + GroupRotaterid + "");
            BindBanner();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DateUpdated", "jAlert('Date updated successfully.','Message')", true);
        }
        //private void BindBanner()
        //{

        //    try
        //    {
        //        string strbanner = "";
        //        DataSet dsGroupCount = new DataSet();
        //        //BannerComponent objbanner = new BannerComponent();
        //        //objbanner.Mode = 1;
        //        dsGroupCount = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBanner 1");// objbanner.GetHomePageRotatingBanner();
        //        if (dsGroupCount != null && dsGroupCount.Tables.Count > 0 && dsGroupCount.Tables[0].Rows.Count > 0)
        //        {

        //            DataSet dsBanner = new DataSet();
        //            // objbanner.Mode = 2;
        //            dsBanner = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBanner 2"); //objbanner.GetHomePageRotatingBanner();
        //            if (dsBanner != null && dsBanner.Tables.Count > 0 && dsBanner.Tables[0].Rows.Count > 0)
        //            {
        //                Random rd = new Random();
        //                // ltBanner.Text += "<div>";

        //                for (int temp = 0; temp < dsGroupCount.Tables[0].Rows.Count; temp++)
        //                {
        //                    if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 1)
        //                    {
        //                        DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + "");


        //                        if (dr.Length > 0)
        //                        {
        //                            strbanner += "<div>";
        //                            if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
        //                                strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
        //                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
        //                            strbanner += "</div>";
        //                        }


        //                    }
        //                    else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 2)
        //                    {


        //                        int maincount = 0;
        //                        int subcount = 0;
        //                        //if (temp == 0)
        //                        //{
        //                        //  //  ltBanner.Text += "<div class=\"item active\">";
        //                        //}
        //                        //else
        //                        //{
        //                        //  //  ltBanner.Text += "<div class=\"item\">";

        //                        //}
        //                        DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=1");
        //                        maincount = dr.Length;

        //                        DataRow[] drsubcount = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
        //                        subcount = drsubcount.Length;


        //                        if (maincount > 0 || subcount > 0)
        //                        {
        //                            strbanner += "<div>";
        //                            if (dr.Length > 0)
        //                            {

        //                                strbanner += "<div class=\"kt-banner-left\">";
        //                                if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
        //                                    strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
        //                                else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
        //                                strbanner += "</div>";
        //                            }

        //                            dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
        //                            if (dr.Length > 0)
        //                            {
        //                                strbanner += "<div class=\"kt-banner-right\">";
        //                                if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
        //                                    strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner").ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
        //                                else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
        //                                strbanner += "</div>";
        //                            }

        //                            strbanner += "</div>";
        //                        }
        //                    }
        //                }




        //                //  ltBanner.Text += "</div>";
        //            }



        //        }

        //        CommonComponent.ExecuteCommonData("update tb_HomehtmlContent SET SiteContent='" + strbanner.Replace("'", "''") + "',MobileContent='" + strbanner.Replace("'", "''").Replace("/" + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "/", "/" + AppLogic.AppConfigs("ImagePathBanner").ToString().ToLower() + "/mobile/") + "'");

        //    }
        //    catch (Exception ex)
        //    {
        //        //  log.Error("Error: ", ex);
        //    }
        //    // log.Debug("BindBanner End");

        //}


        private void BindBanner()
        {


            try
            {
                string Live_Contant_Server = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 ConfigValue from tb_AppConfig where ConfigName='Live_Contant_Server' and StoreID=1 and isnull(Deleted,0)=0"));
                if (String.IsNullOrEmpty(Live_Contant_Server))
                {
                    Live_Contant_Server = AppLogic.AppConfigs("Live_Contant_Server").ToString();
                }

                string ImagePathBanner = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 ConfigValue from tb_AppConfig where ConfigName='ImagePathBanner' and StoreID=1 and isnull(Deleted,0)=0"));
                if (String.IsNullOrEmpty(ImagePathBanner))
                {
                    ImagePathBanner = AppLogic.AppConfigs("ImagePathBanner").ToString();
                }
                string strbanner = "";
                DataSet dsGroupCount = new DataSet();
                //BannerComponent objbanner = new BannerComponent();
                //objbanner.Mode = 1;
                dsGroupCount = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBanner 1");// objbanner.GetHomePageRotatingBanner();
                if (dsGroupCount != null && dsGroupCount.Tables.Count > 0 && dsGroupCount.Tables[0].Rows.Count > 0)
                {

                    DataSet dsBanner = new DataSet();
                    // objbanner.Mode = 2;
                    dsBanner = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBanner 2"); //objbanner.GetHomePageRotatingBanner();
                    if (dsBanner != null && dsBanner.Tables.Count > 0 && dsBanner.Tables[0].Rows.Count > 0)
                    {
                        Random rd = new Random();
                        // ltBanner.Text += "<div>";

                        for (int temp = 0; temp < dsGroupCount.Tables[0].Rows.Count; temp++)
                        {
                            if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 1)
                            {
                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + "");


                                if (dr.Length > 0)
                                {
                                    strbanner += "<div>";
                                    if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                    else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                    strbanner += "</div>";
                                }


                            }
                            else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 2)
                            {


                                int maincount = 0;
                                int subcount = 0;
                                //if (temp == 0)
                                //{
                                //  //  ltBanner.Text += "<div class=\"item active\">";
                                //}
                                //else
                                //{
                                //  //  ltBanner.Text += "<div class=\"item\">";

                                //}
                                DataRow[] dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=1");
                                maincount = dr.Length;

                                DataRow[] drsubcount = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                subcount = drsubcount.Length;


                                if (maincount > 0 || subcount > 0)
                                {
                                    strbanner += "<div>";
                                    if (dr.Length > 0)
                                    {

                                        strbanner += "<div class=\"kt-banner-left\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"kt-banner-right\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathBanner.ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    strbanner += "</div>";
                                }
                            }
                        }




                        //  ltBanner.Text += "</div>";
                    }



                }

                CommonComponent.ExecuteCommonData("update tb_HomehtmlContent SET SiteContent='" + strbanner.Replace("'", "''") + "',MobileContent='" + strbanner.Replace("'", "''").Replace("/" + ImagePathBanner.ToString().ToLower() + "/", "/" + ImagePathBanner.ToString().ToLower() + "/mobile/") + "'");

            }
            catch (Exception ex)
            {
                //  log.Error("Error: ", ex);
            }
            // log.Debug("BindBanner End");

        }

        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {

            Response.Redirect("HomePagebannerlist.aspx");

        }
    }


    public class CompressimagePanda
    {

        public void compressimage(string Fromfile)
        {
            try
            {


                string key = Convert.ToString(CommonComponent.GetScalarCommonData("select ConfigValue from tb_AppConfig where ConfigName='keyCompressimagePanda' and StoreID=1 and isnull(Deleted,0)=0"));
                string input = Fromfile;
                //   FileInfo f1 = new FileInfo(Fromfile);
                string output = Fromfile;

                string url = "https://api.tinify.com/shrink";

                WebClient client = new WebClient();
                string auth = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("api:" + key));
                client.Headers.Add(HttpRequestHeader.Authorization, "Basic " + auth);

                try
                {
                    client.UploadData(url, File.ReadAllBytes(input));
                    /* Compression was successful, retrieve output from Location header. */
                    client.DownloadFile(client.ResponseHeaders["Location"], output);
                }
                catch (WebException)
                {
                    /* Something went wrong! You can parse the JSON body for details. */
                    //Console.WriteLine("Compression failed.");
                }
            }
            catch { }
        }
    }
}