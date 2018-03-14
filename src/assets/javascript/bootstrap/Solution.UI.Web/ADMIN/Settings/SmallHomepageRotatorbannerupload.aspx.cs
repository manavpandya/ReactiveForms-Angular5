using System;
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
using Solution.Data;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class SmallHomepageRotatorbannerupload : BasePage
    {
        public Int32 GroupRotaterid = 0;
        protected void Page_Load(object sender, EventArgs e)
        {


            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidesmall", "document.getElementById('tdSmallbanner').style.display='none';document.getElementById('tdSmallbanner2').style.display='none';", true);
                tdSmallbanner.Visible = false;
                tdSmallbanner2.Visible = false;
                tdSmallbanner3.Visible = false;
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
            {
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidesmall", "document.getElementById('tdSmallbanner2').style.display='none';", true);
                tdSmallbanner2.Visible = false;
                tdSmallbanner3.Visible = false;
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
            {
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidesmall", "document.getElementById('tdSmallbanner2').style.display='none';", true);
                tdSmallbanner3.Visible = false;
            }
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
                imgSave5.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgback.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/back.png";
                imgDateSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";

                // imgCancel3.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (Request.UrlReferrer != null)
                {
                    ViewState["urlrefferer"] = Request.UrlReferrer.ToString();
                }
                HomeBannerComponent objHome = new HomeBannerComponent();
                ViewState["ImagebannrpathSmall"] = CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='ImagePathHomePageBanner' AND storeid=" + Request.QueryString["Storeid"].ToString() + " AND isnull(Deleted,0)=0");
                if (Request.QueryString["grouptype"] != null && Request.QueryString["grouptype"].ToString() == "new")
                {
                    if (Request.QueryString["hid"] == null)
                    {
                        // GroupRotaterid = Convert.ToInt32(objHome.InsertRotatebannerEFF(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 1));
                        SQLAccess objSql = new SQLAccess();
                        GroupRotaterid = Convert.ToInt32(objSql.ExecuteScalarQuery("INSERT INTO tb_RotatorHomebannerSmall(StoreID,BannerTypeId,IsActive) VALUES (" + Convert.ToInt32(Request.QueryString["StoreId"].ToString()) + "," + Convert.ToInt32(Request.QueryString["Id"].ToString()) + ",1); SELECT SCOPE_IDENTITY();"));
                    }

                }
                else
                {
                    if (Request.QueryString["hid"] != null && Request.QueryString["hid"].ToString() != null)
                    {
                        GroupRotaterid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select HomeRotatorId from tb_RotatorHomeBannerDetailSmall where BannerID=" + Request.QueryString["hid"].ToString() + ""));
                    }

                }
                Getbanner();
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {

                    lblImgSize.Text = "Size should be 1570 x 557 or 780 X 90 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
                {
                    lblImgSize.Text = "Size should be 775 x 533 ";
                    //lblImgSize1.Text = "Size should be 713 x 310  ";
                    //lblImgSize2.Text = "Size should be 713 x 310 ";
                    //lblImgSize3.Text = "Size should be 380 x 115  ";
                    lblImgSize1.Text = "Size should be 775 x 533  ";
                    lblImgSize2.Text = "Size should be 775 x 533 ";
                    lblImgSize3.Text = "Size should be 775 x 533  ";
                    lblImgSize4.Text = "Size should be 378 x 352  ";


                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
                {
                    lblImgSize.Text = "Size should be 510 x 475 ";
                    lblImgSize1.Text = "Size should be 510 x 475 ";
                    lblImgSize2.Text = "Size should be 510 x 475 ";
                    lblImgSize3.Text = "Size should be 510 x 475 ";
                    lblImgSize4.Text = "Size should be 378 x 352 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
                {
                    lblImgSize.Text = "Size should be 378 x 352 ";
                    lblImgSize1.Text = "Size should be 378 x 352 ";
                    lblImgSize2.Text = "Size should be 378 x 352 ";
                    lblImgSize3.Text = "Size should be 378 x 352 ";
                    lblImgSize4.Text = "Size should be 378 x 352 ";
                }

            }
            else
            {
                if (Request.QueryString["grouptype"] != null && Request.QueryString["grouptype"].ToString() == "new")
                {
                    if (Request.QueryString["hid"] == null)
                    {
                        GroupRotaterid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select top 1 HomeRotatorId from tb_RotatorHomebannerSmall order by CreatedOn desc"));
                    }

                }
                else
                {
                    if (Request.QueryString["hid"] != null && Request.QueryString["hid"].ToString() != null)
                    {
                        GroupRotaterid = Convert.ToInt32(CommonComponent.GetScalarCommonData("select HomeRotatorId from tb_RotatorHomeBannerDetailSmall where BannerID=" + Request.QueryString["hid"].ToString() + ""));
                    }

                }
            }
            //if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidesmall", "document.getElementById('tdSmallbanner').style.display='none';document.getElementById('tdSmallbanner2').style.display='none';", true);
            //}
            //else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidesmall", "document.getElementById('tdSmallbanner2').style.display='none';", true);
            //}
        }
        private void Getbanner()
        {
            imgDateSave.Visible = true;
            HomeBannerComponent objbanner = new HomeBannerComponent();
            DataSet dsbanner = new DataSet();

            String Displayorder = Convert.ToString(CommonComponent.GetScalarCommonData("select isnull(DisplayOrder,0) as DisplayOrder from tb_RotatorHomebannerSmall where HomeRotatorId=" + GroupRotaterid + ""));
            txtgroupdisplayorder.Text = Displayorder;
            //  dsbanner = objbanner.GetHomeRotatorBanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), GroupRotaterid);
            dsbanner = CommonComponent.GetCommonDataSet("EXEC usp_HomeRotatorBannerSmall " + GroupRotaterid + "," + Convert.ToInt32(Request.QueryString["StoreId"].ToString()) + "");
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
                            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + imghdn.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner.Src = ViewState["ImagebannrpathSmall"].ToString() + imghdn.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            else
                            {
                                imgBanner.Src = "/App_Themes/" + Page.Theme + "/images/spacer.png";
                            }
                            hdnid.Value = Convert.ToString(dr1["BannerID"].ToString());
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
                            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + imghdn1.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner1.Src = ViewState["ImagebannrpathSmall"].ToString() + imghdn1.Value.ToString() + "?" + rd.Next(10000).ToString();
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
                            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + imghdn2.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner2.Src = ViewState["ImagebannrpathSmall"].ToString() + imghdn2.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            hdnid2.Value = Convert.ToString(dr1["BannerID"].ToString());
                            TxtbannerTitle2.Text = Convert.ToString(dr1["Title"].ToString());
                            imgDateSave.Visible = true;
                        }
                        //if (j == 3)
                        //{
                            //if (txtStartDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["startDate"].ToString())))
                            //{
                            //    txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["startDate"].ToString())));
                            //}
                            //if (txtEndDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["EndDate"].ToString())))
                            //{
                            //    txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["EndDate"].ToString())));
                            //}
                            //TxtBannerURL3.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            //TxtDisplayOrder3.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            //ddlTarget3.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            //chkActive3.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            //imghdn3.Value = Convert.ToString(dr1["ImageName"].ToString());
                            //if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + imghdn3.Value.ToString())))
                            //{
                            //    Random rd = new Random();
                            //    imgBanner3.Src = ViewState["ImagebannrpathSmall"].ToString() + imghdn3.Value.ToString() + "?" + rd.Next(10000).ToString();
                            //}
                            //hdnid3.Value = Convert.ToString(dr1["BannerID"].ToString());
                            //TxtbannerTitle3.Text = Convert.ToString(dr1["Title"].ToString());
                            //imgDateSave.Visible = true;
                        //}
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
                            TxtBannerURL4.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            TxtDisplayOrder4.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            ddlTarget4.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            chkActive4.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            imghdn4.Value = Convert.ToString(dr1["ImageName"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + imghdn4.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner4.Src = ViewState["ImagebannrpathSmall"].ToString() + imghdn4.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            hdnid4.Value = Convert.ToString(dr1["BannerID"].ToString());
                            TxtbannerTitle4.Text = Convert.ToString(dr1["Title"].ToString());
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
                            TxtBannerURL5.Text = Convert.ToString(dr1["BannerURL"].ToString());
                            TxtDisplayOrder5.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                            ddlTarget5.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                            chkActive5.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                            imghdn5.Value = Convert.ToString(dr1["ImageName"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + imghdn5.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner5.Src = ViewState["ImagebannrpathSmall"].ToString() + imghdn5.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                            hdnid5.Value = Convert.ToString(dr1["BannerID"].ToString());
                            TxtbannerTitle5.Text = Convert.ToString(dr1["Title"].ToString());
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
                        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgforactive", "jAlert('You can not upload more then 5 banner!','Information')", true);
                        //return;
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

            if (!Directory.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner.HasFile)
            {
                bool checksize = false;
                string ErrorMessage = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {
                    checksize = CheckImageSize(FileUploadBanner, 1570, 557);
                    ErrorMessage = "Please upload banner1 of Size 1570 x 557 ";
                    if (checksize == false)
                    {
                        checksize = CheckImageSize(FileUploadBanner, 780, 90);
                        
                    }
                    
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
                {
                    checksize = CheckImageSize(FileUploadBanner, 775, 533);
                    ErrorMessage = "Please upload banner1 of Size 775 x 533 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
                {
                    checksize = CheckImageSize(FileUploadBanner, 510, 475);
                    ErrorMessage = "Please upload banner1 of Size 510 x 475 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
                {
                    checksize = CheckImageSize(FileUploadBanner, 378, 352);
                    ErrorMessage = "Please upload banner1 of Size 378 x 352 ";
                }

                if (checksize == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "jAlert('" + ErrorMessage + "','Message')", true);
                    return;
                }


                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));


                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        break;
                    }
                }

                imgBanner.Src = ViewState["ImagebannrpathSmall"].ToString() + "/" + filename;
            }
            else
            {
                objRotatorhome.ImageName = imghdn.Value.ToString();
            }


            Int32 rId = 0;
            if (hdnid.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;

                //rId = objHome.Insertbanner(objRotatorhome);
                rId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",1,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid.Value.ToString());
                rId = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",2,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));
                //  objHome.UpdatebannerDetail(objRotatorhome);
                rId = Convert.ToInt32(hdnid.Value.ToString());

            }

            CommonComponent.ExecuteCommonData("update tb_RotatorHomeBannerDetailSmall set LeftBannerText='" + txtLeftBannerText.Text.ToString().Replace("'", "''") + "' where BannerID=" + rId + "");

            hdnid.Value = "0";
            //imghdn.Value = "";
            //TxtbannerTitle.Text = "";
            //TxtBannerURL.Text = "";
            //ddlPosition.SelectedIndex = 0;
            //ddlTarget.SelectedIndex = 0;
            //TxtDisplayOrder.Text = "";
            //txtLeftBannerText.Text = "";
            //chkActive.Checked = false;
            //  BindBanner();
            getfinalhtml();
            lblMsg.Text = "Record Saved Successfully.";
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
            {
                Response.Redirect("SmallHomePagebannerlist.aspx");

            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "")
                {
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "" && imgBanner2.Src != "")
                {
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "" && imgBanner2.Src != "" && imgBanner4.Src != "")
                {
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }

            //  imgBanner.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/spacer.png";
            Getbanner();
            // GetHTMl();
        }
        protected void imgSave1_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebannerSmall WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
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

            if (!Directory.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner1.HasFile)
            {

                bool checksize = false;
                string ErrorMessage = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {
                    checksize = CheckImageSize(FileUploadBanner1, 1570, 557);
                    ErrorMessage = "Please upload banner2 of Size 1570 x 557 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
                {
                    checksize = CheckImageSize(FileUploadBanner1, 775, 533);
                    ErrorMessage = "Please upload banner2 of Size 775 x 533 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
                {
                    checksize = CheckImageSize(FileUploadBanner1, 510, 475);
                    ErrorMessage = "Please upload banner2 of Size 510 x 475 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
                {
                    checksize = CheckImageSize(FileUploadBanner1, 378, 352);
                    ErrorMessage = "Please upload banner2 of Size 378 x 352 ";
                }
                if (checksize == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "jAlert('" + ErrorMessage + "','Message')", true);
                    return;
                }



                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner1.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn1.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner1.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner1.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        break;
                    }
                }

                imgBanner1.Src = ViewState["ImagebannrpathSmall"].ToString() + "/" + filename;
            }
            else
            {
                objRotatorhome.ImageName = imghdn1.Value.ToString();
            }
            Int32 rid = 0;
            if (hdnid1.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;
                // rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",1,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid1.Value.ToString());
                //objHome.UpdatebannerDetail(objRotatorhome);
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",2,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));

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
                    // BindBanner();
                    getfinalhtml();
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "" && imgBanner2.Src != "")
                {
                    getfinalhtml();
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }
            else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "" && imgBanner2.Src != "" && imgBanner4.Src != "")
                {
                    getfinalhtml();
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }
            // BindBanner();
            getfinalhtml();
            lblMsg1.Text = "Record Saved Successfully.";
            Getbanner();
            //GetHTMl();

        }
        protected void imgSave2_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebannerSmall WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
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
            //if (chkActive2.Checked)
            //{
            //    objRotatorhome.Active = true;
            //}
            //else
            //{
            //    objRotatorhome.Active = false;
            //}
            objRotatorhome.Active = true;
            objRotatorhome.Title = TxtbannerTitle2.Text.ToString();
            objRotatorhome.BannerURL = TxtBannerURL2.Text.ToString();

            if (TxtDisplayOrder2.Text.ToString().Trim() == "")
            {
                objRotatorhome.DisplayOrder = 2;
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

            if (!Directory.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner2.HasFile)
            {


                bool checksize = false;
                string ErrorMessage = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {
                    checksize = CheckImageSize(FileUploadBanner2, 1570, 557);
                    ErrorMessage = "Please upload banner3 of Size 1570 x 557 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
                {
                    checksize = CheckImageSize(FileUploadBanner2, 775, 533);
                    ErrorMessage = "Please upload banner3 of Size 775 x 533 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
                {
                    checksize = CheckImageSize(FileUploadBanner2, 510, 475);
                    ErrorMessage = "Please upload banner3 of Size 510 x 475 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
                {
                    checksize = CheckImageSize(FileUploadBanner2, 378, 352);
                    ErrorMessage = "Please upload banner3 of Size 378 X 352 ";
                }
                if (checksize == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "jAlert('" + ErrorMessage + "','Message')", true);
                    return;
                }
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner2.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn2.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner2.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner2.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        break;
                    }
                }
                imgBanner2.Src = ViewState["ImagebannrpathSmall"].ToString() + "/" + filename;
            }
            else
            {
                objRotatorhome.ImageName = imghdn2.Value.ToString();
            }
            Int32 rid = 0;
            if (hdnid2.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;
                // rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",1,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid2.Value.ToString());
                //   objHome.UpdatebannerDetail(objRotatorhome);
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",2,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));

            }
            hdnid2.Value = "0";
            imghdn2.Value = "";

            TxtBannerURL2.Text = "";

            ddlTarget2.SelectedIndex = 0;
            TxtDisplayOrder2.Text = "";
            chkActive2.Checked = false;
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "" && imgBanner2.Src != "")
                {
                    //BindBanner();
                    getfinalhtml();
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }
            lblMsg2.Text = "Record Saved Successfully.";
            Getbanner();
            // GetHTMl();
        }
        protected void imgSave3_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebannerSmall WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
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

            if (!Directory.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner3.HasFile)
            {
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner3.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn3.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner3.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner3.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
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
                // rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",1,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid3.Value.ToString());
                // objHome.UpdatebannerDetail(objRotatorhome);
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",2,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));

            }

            hdnid3.Value = "0";
            imghdn3.Value = "";

            TxtBannerURL3.Text = "";
            ddlTarget3.SelectedIndex = 0;
            TxtDisplayOrder3.Text = "";
            chkActive3.Checked = false;
            Getbanner();
            lblMsg3.Text = "Record Saved Successfully.";
            //GetHTMl();
        }

        protected void imgSave4_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebannerSmall WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
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
            //if (chkActive4.Checked)
            //{
                objRotatorhome.Active = true;
            //}
            //else
            //{
            //    objRotatorhome.Active = false;
            //}
            objRotatorhome.Title = TxtbannerTitle4.Text.ToString();
            objRotatorhome.BannerURL = TxtBannerURL4.Text.ToString();

            if (TxtDisplayOrder4.Text.ToString().Trim() == "")
            {
                objRotatorhome.DisplayOrder = 3;
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

            if (!Directory.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner4.HasFile)
            {

                bool checksize = false;
                string ErrorMessage = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {
                    checksize = CheckImageSize(FileUploadBanner4, 1570, 557);
                    ErrorMessage = "Please upload banner4 of Size 1570 x 557 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
                {
                    checksize = CheckImageSize(FileUploadBanner4, 775, 533);
                    ErrorMessage = "Please upload banner4 of Size 775 x 533 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
                {
                    checksize = CheckImageSize(FileUploadBanner4, 510, 475);
                    ErrorMessage = "Please upload banner4 of Size 510 x 475 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
                {
                    checksize = CheckImageSize(FileUploadBanner4, 378, 352);
                    ErrorMessage = "Please upload banner4 of Size 378 x 352 ";
                }
                if (checksize == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "jAlert('" + ErrorMessage + "','Message')", true);
                    return;
                }
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner4.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn4.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner4.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner4.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        break;
                    }
                    imgBanner4.Src = ViewState["ImagebannrpathSmall"].ToString() + "/" + filename;
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
                // rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",1,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid4.Value.ToString());
                //  objHome.UpdatebannerDetail(objRotatorhome);
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",2,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));

            }
            hdnid4.Value = "0";
            imghdn4.Value = "";

            TxtBannerURL4.Text = "";

            ddlTarget4.SelectedIndex = 0;
            TxtDisplayOrder4.Text = "";
            chkActive4.Checked = false;
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "" && imgBanner2.Src != "" && imgBanner4.Src != "")
                {
                    //BindBanner();
                    getfinalhtml();
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }
            lblMsg4.Text = "Record Saved Successfully.";
            Getbanner();
            //GetHTMl();
        }
        protected void imgSave5_Click(object sender, ImageClickEventArgs e)
        {
            HomeBannerComponent objHome = new HomeBannerComponent();
            Int32 Rotatrid = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOP 1 HomeRotatorId FROM tb_RotatorHomebannerSmall WHERE StoreID=" + Request.QueryString["StoreId"] + " AND BannerTypeId=" + Request.QueryString["Id"].ToString() + ""));
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
            //if (chkActive4.Checked)
            //{
            objRotatorhome.Active = true;
            //}
            //else
            //{
            //    objRotatorhome.Active = false;
            //}
            objRotatorhome.Title = TxtbannerTitle5.Text.ToString();
            objRotatorhome.BannerURL = TxtBannerURL5.Text.ToString();

            if (TxtDisplayOrder5.Text.ToString().Trim() == "")
            {
                objRotatorhome.DisplayOrder = 4;
            }
            else
            {
                objRotatorhome.DisplayOrder = Convert.ToInt32(TxtDisplayOrder5.Text.ToString());
            }
            objRotatorhome.IsMain = 0;
            objRotatorhome.HomeRotatorId = GroupRotaterid;
            objRotatorhome.StoreID = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            objRotatorhome.Pagination = "";
            objRotatorhome.LinkTarget = ddlTarget5.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(txtStartDate.Text.ToString()))
            {
                objRotatorhome.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            }
            if (!String.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                objRotatorhome.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            }

            if (!Directory.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString())))
            {
                Directory.CreateDirectory(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()));

            }
            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            if (FileUploadBanner5.HasFile)
            {

                bool checksize = false;
                string ErrorMessage = "";
                if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                {
                    checksize = CheckImageSize(FileUploadBanner5, 1570, 557);
                    ErrorMessage = "Please upload banner4 of Size 1570 x 557 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
                {
                    checksize = CheckImageSize(FileUploadBanner5, 775, 533);
                    ErrorMessage = "Please upload banner4 of Size 775 x 533 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
                {
                    checksize = CheckImageSize(FileUploadBanner5, 510, 475);
                    ErrorMessage = "Please upload banner4 of Size 510 x 475 ";
                }
                else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
                {
                    checksize = CheckImageSize(FileUploadBanner5, 780, 90);
                    ErrorMessage = "Please upload Small banner of Size 780 x 90 ";
                }
                if (checksize == false)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Error", "jAlert('" + ErrorMessage + "','Message')", true);
                    return;
                }
                string filename = "";
                FileInfo fl = new FileInfo(FileUploadBanner5.FileName.ToString());

                for (int i = 1; i < 1000; i++)
                {
                    filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "_small_" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn5.Value.ToString())
                        {
                            try
                            {
                                File.Delete(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch { }
                            FileUploadBanner5.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            objRotatorhome.ImageName = filename;
                            try
                            {
                                CompressimagePanda objcompress = new CompressimagePanda();
                                objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            }
                            catch
                            {

                            }
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                            break;
                        }
                    }
                    else
                    {
                        FileUploadBanner5.SaveAs(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        objRotatorhome.ImageName = filename;
                        try
                        {
                            CompressimagePanda objcompress = new CompressimagePanda();
                            objcompress.compressimage(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        }
                        catch
                        {

                        }
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                        break;
                    }
                    imgBanner5.Src = ViewState["ImagebannrpathSmall"].ToString() + "/" + filename;
                }
            }
            else
            {
                objRotatorhome.ImageName = imghdn4.Value.ToString();
            }
            Int32 rid = 0;
            if (hdnid5.Value.ToString() == "0")
            {
                objRotatorhome.BannerID = 0;
                // rid = Convert.ToInt32(objHome.Insertbanner(objRotatorhome));
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",1,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));
            }
            else
            {
                objRotatorhome.BannerID = Convert.ToInt32(hdnid5.Value.ToString());
                //  objHome.UpdatebannerDetail(objRotatorhome);
                rid = Convert.ToInt32(CommonComponent.GetScalarCommonData("Exec usp_InsertHomeRotatorBannerSmall " + objRotatorhome.BannerID + "," + objRotatorhome.StoreID + ",'" + objRotatorhome.Title + "','" + objRotatorhome.BannerURL + "','" + objRotatorhome.ImageName + "'," + objRotatorhome.DisplayOrder + "," + objRotatorhome.Active + "," + objRotatorhome.HomeRotatorId + ",'" + objRotatorhome.Pagination + "'," + objRotatorhome.IsMain + ",2,'" + objRotatorhome.LinkTarget + "','" + objRotatorhome.StartDate + "','" + objRotatorhome.EndDate + "'"));

            }
            hdnid5.Value = "0";
            imghdn5.Value = "";

            TxtBannerURL5.Text = "";

            ddlTarget5.SelectedIndex = 0;
            TxtDisplayOrder5.Text = "";
            chkActive5.Checked = false;
            if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "4")
            {
                if (imgBanner.Src != "" && imgBanner1.Src != "" && imgBanner2.Src != "" && imgBanner4.Src != "" && imgBanner5.Src != "")
                {
                    //BindBanner();
                    getfinalhtml();
                    Response.Redirect("SmallHomePagebannerlist.aspx");
                }
            }
            lblMsg5.Text = "Record Saved Successfully.";
            Getbanner();
            //GetHTMl();
        }
        private void GetHTMl()
        {
            string strHTML = "";
            //HomeBannerComponent objbanner = new HomeBannerComponent();
            //DataSet dsbanner = new DataSet();

            ////  dsbanner = objbanner.GetHomeRotatorBanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["id"].ToString()));
            //dsbanner = CommonComponent.GetCommonDataSet("Exec usp_HomeRotatorBannerSmall " + Convert.ToInt32(Request.QueryString["id"].ToString()) + "," + Convert.ToInt32(Request.QueryString["StoreId"].ToString()) + "");
            //string strPosition = "";
            //if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
            //{
            //    strHTML += "<div id=\"banner-part\">";
            //    strHTML += "<div class=\"main-banner\">";
            //    strHTML += "<div id=\"slider1\" class=\"contentslide\">";
            //    strHTML += "<div class=\"opacitylayer\">";
            //    if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow[] dr = dsbanner.Tables[0].Select("isnull(IsMain,0)=1");
            //        foreach (DataRow dr1 in dr)
            //        {
            //            strPosition = dr1["Pagination"].ToString();
            //            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
            //            {
            //                strHTML += "<div class=\"contentdiv\">";
            //                strHTML += "<div class=\"index-banner-1\">";
            //                if (dr1["BannerURL"].ToString().Trim() != "")
            //                {
            //                    strHTML += " <div class=\"banner1\"><a href=\"" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img style=\"cursor:pointer;\"  src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></a></div>";
            //                }
            //                else
            //                {
            //                    strHTML += " <div class=\"banner1\"><img  src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></div>";
            //                }
            //                strHTML += " </div>";
            //                strHTML += " </div>";
            //            }
            //        }

            //    }
            //    strHTML += " </div>";
            //    strHTML += " </div>";
            //    strHTML += " </div>";

            //    if (strPosition.ToString().ToLower() == "left")
            //    {
            //        strHTML += " <div class=\"pagination\" id=\"paginate-slider1\"></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "right")
            //    {
            //        strHTML += " <div class=\"pagination-right\" id=\"paginate-slider1\"></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "top")
            //    {
            //        strHTML += " <div class=\"pagination-top\" id=\"paginate-slider1\"></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "bottom")
            //    {
            //        strHTML += " <div class=\"pagination-bottom\" id=\"paginate-slider1\"></div>";
            //    }
            //    strHTML += " <div id=\"play-btn\" class=\"play\"> <a href=\"javascript:ContentSlider(slider1,8000);\">&nbsp;</a> </div>";
            //    strHTML += " <div id=\"paginate-new\" class=\"pause\"><a href=\"javascript:cancelautorun();\">&nbsp;</a></div>";
            //    strHTML += " </div>";
            //}
            //else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
            //{

            //    strHTML += "<div id=\"banner-part\">";
            //    strHTML += "<div class=\"main-banner\">";
            //    strHTML += "<div id=\"slider1\" class=\"contentslide\">";
            //    strHTML += "<div class=\"opacitylayer\">";

            //    if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow[] dr = dsbanner.Tables[0].Select("isnull(IsMain,0)=1");
            //        foreach (DataRow dr1 in dr)
            //        {
            //            strPosition = dr1["Pagination"].ToString();
            //            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
            //            {
            //                strHTML += "<div class=\"contentdiv\">";
            //                strHTML += "<div class=\"index-banner-1\">";
            //                if (dr1["BannerURL"].ToString().Trim() != "")
            //                {
            //                    strHTML += " <div class=\"banner1\"><a href=\"" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img style=\"cursor:pointer;\" src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></a></div>";
            //                }
            //                else
            //                {
            //                    strHTML += " <div class=\"banner1\"><img  src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></div>";
            //                }
            //                strHTML += " </div>";
            //                strHTML += " </div>";
            //            }
            //        }

            //    }
            //    strHTML += " </div>";
            //    strHTML += " </div>";
            //    strHTML += " </div>";
            //    if (strPosition.ToString().ToLower() == "left")
            //    {
            //        strHTML += " <div class=\"pagination\" id=\"paginate-slider1\"></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "right")
            //    {
            //        strHTML += " <div class=\"pagination-right\" id=\"paginate-slider1\"></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "top")
            //    {
            //        strHTML += " <div class=\"pagination-top\" id=\"paginate-slider1\"></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "bottom")
            //    {
            //        strHTML += " <div class=\"pagination-bottom\" id=\"paginate-slider1\"></div>";
            //    }
            //    strHTML += " <div id=\"play-btn\" class=\"play\"> <a href=\"javascript:ContentSlider(slider1,8000);\">&nbsp;</a> </div>";
            //    strHTML += " <div id=\"paginate-new\" class=\"pause\"><a href=\"javascript:cancelautorun();\">&nbsp;</a></div>";
            //    strHTML += " </div>";

            //    DataRow[] drSmall = dsbanner.Tables[0].Select("isnull(IsMain,0)=0");
            //    int Count = 0;
            //    strHTML += " <div class=\"small-banner\">";
            //    foreach (DataRow dr1 in drSmall)
            //    {

            //        if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
            //        {
            //            Count++;
            //            if (Count % 2 == 0)
            //            {
            //                if (dr1["BannerURL"].ToString().Trim() != "")
            //                {
            //                    strHTML += "<div class=\"index-banner-right\"> <a title=\"" + dr1["Title"].ToString() + "\" href=\"/" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></a></div>";
            //                }
            //                else
            //                {
            //                    strHTML += "<div class=\"index-banner-right\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></div>";
            //                }
            //            }
            //            else
            //            {
            //                if (dr1["BannerURL"].ToString().Trim() != "")
            //                {
            //                    strHTML += "<div class=\"index-banner-left\"> <a title=\"" + dr1["Title"].ToString() + "\" href=\"/" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></a></div>";
            //                }
            //                else
            //                {
            //                    strHTML += "<div class=\"index-banner-left\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></div>";
            //                }
            //            }
            //        }
            //    }
            //    strHTML += " </div>";
            //}
            //else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
            //{

            //    strHTML += "<div id=\"banner\">";
            //    strHTML += "<div id='slider1' class='contentslide'>";

            //    strHTML += "<div class='opacitylayer' style='filter:progid:DXImageTransform.Microsoft.alpha(opacity=100);-moz-opacity:1; opacity: 1;'>";
            //    if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
            //    {
            //        DataRow[] dr = dsbanner.Tables[0].Select("isnull(IsMain,0)=1");

            //        foreach (DataRow dr1 in dr)
            //        {
            //            strPosition = dr1["Pagination"].ToString();
            //            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
            //            {
            //                strHTML += "<div class='contentdiv'>";
            //                strHTML += "<div class='index_banner_row2'>";
            //                if (dr1["BannerURL"].ToString().Trim() != "")
            //                {
            //                    strHTML += " <div class='banner2'><a href=\"" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img style='cursor:pointer;'  src='" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "' alt='" + dr1["Title"].ToString() + "' Title='" + dr1["Title"].ToString() + "' /></a></div>";
            //                }
            //                else
            //                {
            //                    strHTML += " <div class='banner2'><img  src='" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "' alt='" + dr1["Title"].ToString() + "' Title='" + dr1["Title"].ToString() + "' /></div>";
            //                }
            //                strHTML += " </div>";
            //                strHTML += " </div>";
            //            }
            //        }

            //    }
            //    if (strPosition.ToString().ToLower() == "left")
            //    {
            //        strHTML += " <div class='pagination-left' id='paginate-slider1' style='width:160px;'></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "right")
            //    {
            //        strHTML += " <div class='pagination-right' id='paginate-slider1' style='width:160px;'></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "top")
            //    {
            //        strHTML += " <div class='pagination-top' id='paginate-slider1' style='width:160px;'></div>";
            //    }
            //    else if (strPosition.ToString().ToLower() == "bottom")
            //    {
            //        strHTML += " <div class='pagination-bottom' id='paginate-slider1' style='width:160px;'></div>";
            //    }
            //    strHTML += " <div id='play-btn' class='play'> <a href='javascript:ContentSlider(slider1,8000);'>&nbsp;</a> </div>";
            //    strHTML += " <div id='paginate-new' class='pause'><a href='javascript:cancelautorun();'>&nbsp;</a></div>";
            //    strHTML += " </div>";
            //    strHTML += " </div>";
            //    strHTML += " </div>";

            //    strHTML += "<div class=\"sub-banner\">";
            //    DataRow[] drSmall = dsbanner.Tables[0].Select("isnull(IsMain,0)=0");
            //    foreach (DataRow dr1 in drSmall)
            //    {
            //        if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
            //        {
            //            if (dr1["BannerURL"].ToString().Trim() != "")
            //            {
            //                strHTML += "<div class=\"sub-banner-1\"> <a title=\"" + dr1["Title"].ToString() + "\" href=\"/engravingfonts\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></a></div>";
            //            }
            //            else
            //            {
            //                strHTML += "<div class=\"sub-banner-1\"><img title=\"" + dr1["Title"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" src=\"" + ViewState["ImagebannrpathSmall"].ToString() + "/" + dr1["ImageName"].ToString() + "\"></div>";
            //            }
            //        }
            //    }
            //    strHTML += " </div>";
            //}

            //Int32 StoreId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOp 1 StoreId FROM tb_HtmlTextSmall WHERE StoreId=" + Request.QueryString["Storeid"] + " AND HtmlType=1"));
            //if (StoreId > 0)
            //{
            //    CommonComponent.ExecuteCommonData("UPDATE tb_HtmlTextSmall SET HtmlText='" + strHTML.Replace("'", "''") + "' WHERE StoreId=" + Request.QueryString["Storeid"] + " AND HtmlType=1");
            //}
            //else
            //{
            //    CommonComponent.ExecuteCommonData("INSERT INTO tb_HtmlTextSmall (StoreId,HtmlText,HtmlType) values (" + Request.QueryString["Storeid"].ToString() + ",'" + strHTML.Replace("'", "''") + "',1)");
            //}
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
                            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + imghdn.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner.Src = ViewState["ImagebannrpathSmall"].ToString() + imghdn.Value.ToString() + "?" + rd.Next(10000).ToString();
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
            //  objHome.DeleteBanner(objRotatorhome);
            CommonComponent.ExecuteCommonData("DELETE FROM tb_RotatorHomeBannerDetailSmall  WHERE BannerID=" + objRotatorhome.BannerID + "");
            if (File.Exists(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename)))
            {
                try
                {
                    File.Delete(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
                    CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["ImagebannrpathSmall"].ToString() + "/" + filename));
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
            // GetHTMl();
        }

        protected void imgback_Click(object sender, ImageClickEventArgs e)
        {
            if (ViewState["urlrefferer"] != null)
            {
                Response.Redirect(ViewState["urlrefferer"].ToString());

            }
            else
            {
                Response.Redirect("SmallHomepageRatotingbanner.aspx");
            }
        }


        protected void imgDateSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!string.IsNullOrEmpty(txtStartDate.Text.ToString()) && !string.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {


                CommonComponent.ExecuteCommonData(@"UPDATE tb_RotatorHomeBannerDetailSmall SET  tb_RotatorHomeBannerDetailSmall.StartDate ='" + Convert.ToDateTime(txtStartDate.Text.Trim()) + @"',tb_RotatorHomeBannerDetailSmall.EndDate='" + Convert.ToDateTime(txtEndDate.Text.Trim()) + @"'
                                                FROM dbo.tb_RotatorHomebannerSmall INNER JOIN
                                                dbo.tb_RotatorHomeBannerDetailSmall ON dbo.tb_RotatorHomebannerSmall.HomeRotatorId = dbo.tb_RotatorHomeBannerDetailSmall.HomeRotatorId
                                                WHERE tb_RotatorHomebannerSmall.HomeRotatorId =" + GroupRotaterid + @" AND tb_RotatorHomebannerSmall.StoreID=" + Convert.ToString(Request.QueryString["storeid"]) + @"");
            }
            else if (string.IsNullOrEmpty(txtStartDate.Text.ToString()) && string.IsNullOrEmpty(txtEndDate.Text.ToString()))
            {
                CommonComponent.ExecuteCommonData("UPDATE tb_RotatorHomeBannerDetailSmall SET  tb_RotatorHomeBannerDetailSmall.StartDate =null,tb_RotatorHomeBannerDetailSmall.EndDate=null FROM dbo.tb_RotatorHomebannerSmall INNER JOIN dbo.tb_RotatorHomeBannerDetailSmall ON dbo.tb_RotatorHomebannerSmall.HomeRotatorId = dbo.tb_RotatorHomeBannerDetailSmall.HomeRotatorId WHERE tb_RotatorHomebannerSmall.HomeRotatorId =" + GroupRotaterid + " AND tb_RotatorHomebannerSmall.StoreID=" + Convert.ToString(Request.QueryString["storeid"]) + "");
            }
            Int32 DO = 0;
            Int32.TryParse(txtgroupdisplayorder.Text, out DO);
            CommonComponent.ExecuteCommonData("update tb_RotatorHomebannerSmall set DisplayOrder=" + DO + " where HomeRotatorId=" + GroupRotaterid + "");
            // BindBanner();
            getfinalhtml();
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DateUpdated", "jAlert('Date updated successfully.','Message')", true);
        }


        private void getfinalhtml()
        {

            string Live_Contant_Server = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 ConfigValue from tb_AppConfig where ConfigName='Live_Contant_Server' and StoreID=1 and isnull(Deleted,0)=0"));
            if (String.IsNullOrEmpty(Live_Contant_Server))
            {
                Live_Contant_Server = AppLogic.AppConfigs("Live_Contant_Server").ToString();
            }


            string ImagePathHomePageBanner = Convert.ToString(CommonComponent.GetScalarCommonData("select top 1 ConfigValue from tb_AppConfig where ConfigName='ImagePathHomePageBanner' and StoreID=1 and isnull(Deleted,0)=0"));
            if (String.IsNullOrEmpty(ImagePathHomePageBanner))
            {
                ImagePathHomePageBanner = AppLogic.AppConfigs("ImagePathHomePageBanner").ToString();
            }

            try
            {
                string strbanner = "";
                DataSet dsGroupCount = new DataSet();
                //BannerComponent objbanner = new BannerComponent();
                //objbanner.Mode = 1;
                dsGroupCount = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall 1");// objbanner.GetHomePageRotatingBanner();
                if (dsGroupCount != null && dsGroupCount.Tables.Count > 0 && dsGroupCount.Tables[0].Rows.Count > 0)
                {

                    DataSet dsBanner = new DataSet();
                    // objbanner.Mode = 2;
                    dsBanner = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall 2"); //objbanner.GetHomePageRotatingBanner();
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
                                    strbanner += "<div class=\"margin-top-20 col-layout layout-1-col wow slideInUp animated\">";
                                    strbanner += "<div class=\"row\">";
                                    strbanner += "<div class=\"col-sm-12 col-xs-12 text-center\">";

                                    if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                        strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                    else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                    strbanner += "</div>";
                                    strbanner += "</div>";
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
                                    strbanner += "<div class=\"margin-top-20 col-layout layout-2-col wow slideInUp animated\">";
                                    strbanner += "<div class=\"row\">";
                                    if (dr.Length > 0)
                                    {

                                        strbanner += "<div class=\"col-sm-6 col-xs-6 xs-mrg-b-20\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-sm-6 col-xs-6\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    strbanner += "</div>";
                                    strbanner += "</div>";
                                }
                            }
                            else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 3)
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
                                    strbanner += "<div class=\"margin-top-20 col-layout layout-3-col wow slideInUp animated\">";
                                    strbanner += "<div class=\"row\">";

                                    if (dr.Length > 0)
                                    {

                                        strbanner += "<div class=\"col-sm-4 col-xs-4 xs-mrg-b-20\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-sm-4 col-xs-4 xs-mrg-b-20\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";


                                        if (dr.Length > 1)
                                        {


                                            strbanner += "<div class=\"col-sm-4 col-xs-4\">";
                                            if (Convert.ToString(dr[1]["BannerURL"]) != "" && Convert.ToString(dr[1]["StoreID"]) != "")
                                                strbanner += "<a href=\"" + Convert.ToString(dr[1]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "'   /></a>";
                                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"])) + "' /></a>";
                                            strbanner += "</div>";
                                        }
                                    }

                                    strbanner += "</div>";
                                    strbanner += "</div>";
                                }
                            }
                            else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 4)
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
                                    strbanner += "<div class=\"margin-top-20 col-layout layout-3-col wow slideInUp animated\">";
                                    strbanner += "<div class=\"row\">";

                                    if (dr.Length > 0)
                                    {

                                        strbanner += "<div class=\"col-md-3 col-sm-6 col-xs-12 xs-mrg-b-20 sm-mrg-b-20\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"col-md-3 col-sm-6 col-xs-12 xs-mrg-b-20 sm-mrg-b-20\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";


                                        if (dr.Length > 1)
                                        {


                                            strbanner += "<div class=\"col-md-3 col-sm-6 col-xs-12 xs-mrg-b-20\">";
                                            if (Convert.ToString(dr[1]["BannerURL"]) != "" && Convert.ToString(dr[1]["StoreID"]) != "")
                                                strbanner += "<a href=\"" + Convert.ToString(dr[1]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "'   /></a>";
                                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"])) + "' /></a>";
                                            strbanner += "</div>";
                                        }
                                        if (dr.Length > 2)
                                        {


                                            strbanner += "<div class=\"col-md-3 col-sm-6 col-xs-12\">";
                                            if (Convert.ToString(dr[2]["BannerURL"]) != "" && Convert.ToString(dr[2]["StoreID"]) != "")
                                                strbanner += "<a href=\"" + Convert.ToString(dr[2]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[2]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[2]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"]).Trim()) + "'   /></a>";
                                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[2]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[2]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[2]["Title"])) + "' /></a>";
                                            strbanner += "</div>";
                                        }
                                        
                                    }

                                    strbanner += "</div>";
                                    strbanner += "</div>";
                                    //if (dr.Length > 0 && dr.Length > 3)
                                    //{
                                    //    strbanner += "<div class=\"margin-top-20 col-layout layout-3-col wow slideInUp animated\">";

                                    //    strbanner += "<div class=\"row\">";
                                    //    strbanner += "<div class=\"col-sm-12 col-xs-12 text-center\">";
                                        
                                    //    if (Convert.ToString(dr[3]["BannerURL"]) != "" && Convert.ToString(dr[3]["StoreID"]) != "")
                                    //    strbanner += "<a href=\"" + Convert.ToString(dr[3]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[3]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToLower() + "" + dr[3]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[3]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[3]["Title"]).Trim()) + "'   /></a>";
                                    //     else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[3]["Title"]).Trim()) + "\"><img src='" + Live_Contant_Server + ImagePathHomePageBanner.ToString().ToLower() + "" + dr[3]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[3]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[3]["Title"])) + "' /></a>";
                                    //    strbanner += "</div>";
                                    //    strbanner += "</div>";

                                    //    strbanner += "</div>";
                                    //}
                                   
                                }
                            }
                        }




                        //  ltBanner.Text += "</div>";
                    }



                }


                CommonComponent.ExecuteCommonData("update tb_topic set Description='" + strbanner.ToString().Replace("'", "''") + "' where Title='HomePageBanner' and StoreID=1");
            }
            catch { }
        }

        //private void getfinalhtml()
        //{
        //    try
        //    {
        //        string strbanner = "";
        //        DataSet dsGroupCount = new DataSet();
        //        //BannerComponent objbanner = new BannerComponent();
        //        //objbanner.Mode = 1;
        //        dsGroupCount = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall 1");// objbanner.GetHomePageRotatingBanner();
        //        if (dsGroupCount != null && dsGroupCount.Tables.Count > 0 && dsGroupCount.Tables[0].Rows.Count > 0)
        //        {

        //            DataSet dsBanner = new DataSet();
        //            // objbanner.Mode = 2;
        //            dsBanner = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall 2"); //objbanner.GetHomePageRotatingBanner();
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
        //                            strbanner += "<div class=\"margin-top-20 col-layout layout-1-col wow slideInUp animated\">";
        //                            strbanner += "<div class=\"row\">";
        //                            strbanner += "<div class=\"col-sm-12 col-xs-12\">";

        //                            if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
        //                                strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
        //                            else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
        //                            strbanner += "</div>";
        //                            strbanner += "</div>";
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
        //                            strbanner += "<div class=\"margin-top-20 col-layout layout-2-col wow slideInUp animated\">";
        //                            strbanner += "<div class=\"row\">";
        //                            if (dr.Length > 0)
        //                            {

        //                                strbanner += "<div class=\"col-sm-6 col-xs-6 xs-mrg-b-20\">";
        //                                if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
        //                                    strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
        //                                else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
        //                                strbanner += "</div>";
        //                            }

        //                            dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
        //                            if (dr.Length > 0)
        //                            {
        //                                strbanner += "<div class=\"col-sm-6 col-xs-6\">";
        //                                if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
        //                                    strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
        //                                else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
        //                                strbanner += "</div>";
        //                            }

        //                            strbanner += "</div>";
        //                            strbanner += "</div>";
        //                        }
        //                    }
        //                    else if (Convert.ToInt32(dsGroupCount.Tables[0].Rows[temp]["BannerTypeId"].ToString()) == 3)
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
        //                            strbanner += "<div class=\"margin-top-20 col-layout layout-3-col wow slideInUp animated\">";
        //                            strbanner += "<div class=\"row\">";

        //                            if (dr.Length > 0)
        //                            {

        //                                strbanner += "<div class=\"col-sm-4 col-xs-4 xs-mrg-b-20\">";
        //                                if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
        //                                    strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
        //                                else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
        //                                strbanner += "</div>";
        //                            }

        //                            dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
        //                            if (dr.Length > 0)
        //                            {
        //                                strbanner += "<div class=\"col-sm-4 col-xs-4 xs-mrg-b-20\">";
        //                                if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
        //                                    strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
        //                                else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
        //                                strbanner += "</div>";


        //                                if (dr.Length > 1)
        //                                {


        //                                    strbanner += "<div class=\"col-sm-4 col-xs-4\">";
        //                                    if (Convert.ToString(dr[1]["BannerURL"]) != "" && Convert.ToString(dr[1]["StoreID"]) != "")
        //                                        strbanner += "<a href=\"" + Convert.ToString(dr[1]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "'   /></a>";
        //                                    else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[1]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[1]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[1]["Title"])) + "' /></a>";
        //                                    strbanner += "</div>";
        //                                }
        //                            }

        //                            strbanner += "</div>";
        //                            strbanner += "</div>";
        //                        }
        //                    }
        //                }




        //                //  ltBanner.Text += "</div>";
        //            }



        //        }


        //        CommonComponent.ExecuteCommonData("update tb_topic set Description='" + strbanner.ToString().Replace("'", "''") + "' where Title='HomePageBanner' and StoreID=1");
        //    }
        //    catch { }
        //}
        private void BindBanner()
        {

            try
            {
                string strbanner = "";
                DataSet dsGroupCount = new DataSet();
                //BannerComponent objbanner = new BannerComponent();
                //objbanner.Mode = 1;
                dsGroupCount = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall 1");// objbanner.GetHomePageRotatingBanner();
                if (dsGroupCount != null && dsGroupCount.Tables.Count > 0 && dsGroupCount.Tables[0].Rows.Count > 0)
                {

                    DataSet dsBanner = new DataSet();
                    // objbanner.Mode = 2;
                    dsBanner = CommonComponent.GetCommonDataSet("EXEC GuiGetHomeRotatingBannerSmall 2"); //objbanner.GetHomePageRotatingBanner();
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
                                        strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                    else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
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
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    dr = dsBanner.Tables[0].Select("HomeRotatorId=" + dsGroupCount.Tables[0].Rows[temp]["HomeRotatorId"].ToString() + " and ismain=0");
                                    if (dr.Length > 0)
                                    {
                                        strbanner += "<div class=\"kt-banner-right\">";
                                        if (Convert.ToString(dr[0]["BannerURL"]) != "" && Convert.ToString(dr[0]["StoreID"]) != "")
                                            strbanner += "<a href=\"" + Convert.ToString(dr[0]["BannerURL"]) + "\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "'   /></a>";
                                        else strbanner += "<a href=\"javascript:void(0);\" title=\"" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"]).Trim()) + "\"><img src='" + AppLogic.AppConfigs("Live_Contant_Server") + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "" + dr[0]["ImageName"].ToString().ToLower() + "?" + rd.Next(1000).ToString() + "'  alt='" + Convert.ToString(dr[0]["Title"]).Trim() + "' title='" + Server.HtmlEncode(Convert.ToString(dr[0]["Title"])) + "' /></a>";
                                        strbanner += "</div>";
                                    }

                                    strbanner += "</div>";
                                }
                            }
                        }




                        //  ltBanner.Text += "</div>";
                    }



                }

                CommonComponent.ExecuteCommonData("update tb_HomehtmlContentSmall SET SiteContent='" + strbanner.Replace("'", "''") + "',MobileContent='" + strbanner.Replace("'", "''").Replace("/" + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "/", "/" + AppLogic.AppConfigs("ImagePathHomePageBanner").ToString().ToLower() + "/mobile/") + "'");

            }
            catch (Exception ex)
            {
                //  log.Error("Error: ", ex);
            }
            // log.Debug("BindBanner End");

        }

        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {

            Response.Redirect("SmallHomePagebannerlist.aspx");

        }




    }
}