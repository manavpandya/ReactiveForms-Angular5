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


namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class HomepageSmallbanner : BasePage
    {
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
                imgback.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/button/back.png";
                imgDateSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";

                // imgCancel3.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (Request.UrlReferrer != null)
                {
                    ViewState["urlrefferer"] = Request.UrlReferrer.ToString();
                }
                if (Request.QueryString["Storeid"] != null)
                {
                    ViewState["Imagebannrpath"] = CommonComponent.GetScalarCommonData("SELECT TOP 1 ConfigValue FROM tb_AppConfig WHERE ConfigName='ImagePathSmallBanner' AND storeid=" + Request.QueryString["Storeid"].ToString() + " AND isnull(Deleted,0)=0");
                    Getbanner();
                }
                //if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
                //{

                lblImgSize.Text = "Size should be 480 x 180 ";
                //}
                //else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "2")
                //{
                //    lblImgSize.Text = "Size should be 1440 x 500 ";
                //    lblImgSize1.Text = "Size should be 713 x 310  ";
                //    lblImgSize2.Text = "Size should be 713 x 310 ";
                //    lblImgSize3.Text = "Size should be 380 x 115  ";

                //}
                //else if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "3")
                //{
                //    lblImgSize.Text = "Size should be 590 x 377 ";
                //    lblImgSize1.Text = "Size should be 380 x 115 ";
                //    lblImgSize2.Text = "Size should be 380 x 115 ";
                //    lblImgSize3.Text = "Size should be 380 x 115 ";
                //}
                if (txtStartDate.Text.ToString().Trim() == "")
                {
                    txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(DateTime.Now.ToString())));
                }
                if (txtEndDate.Text.ToString().Trim() == "")
                {
                    txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(DateTime.Now.AddDays(30).ToString())));
                }
            }
            //if (Request.QueryString["id"] != null && Request.QueryString["id"].ToString() == "1")
            //{
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "hidesmall", "document.getElementById('tdSmallbanner').style.display='none';", true);
            //}
        }
        private void Getbanner()
        {
            HomeBannerComponent objbanner = new HomeBannerComponent();
            DataSet dsbanner = new DataSet();

            dsbanner = CommonComponent.GetCommonDataSet("SELECT * FROM tb_HomesmallBannerDetail WHERE StoreId=" + Request.QueryString["StoreId"].ToString() + ""); //objbanner.GetHomeRotatorBanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["id"].ToString()));
            ViewState["dsbanner"] = dsbanner;
            Int32 j = 0;
            if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
            {
                grdbannerlist.DataSource = dsbanner;
                grdbannerlist.DataBind();
                if (Request.QueryString["hid"] != null)
                {
                    //DataRow[] drmain = dsbanner.Tables[0].Select("isnull(ismain,0) =1 AND BannerId=" + Request.QueryString["hid"].ToString() + "");
                    //if (drmain != null && drmain.Length > 0)
                    //{
                    //    foreach (DataRow dr1 in drmain)
                    //    {
                    //        if (txtStartDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["startDate"].ToString())))
                    //        {
                    //            txtStartDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["startDate"].ToString())));
                    //        }
                    //        if (txtEndDate.Text.ToString().Trim() == "" && !string.IsNullOrEmpty(Convert.ToString(dr1["EndDate"].ToString())))
                    //        {
                    //            txtEndDate.Text = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Convert.ToString(dr1["EndDate"].ToString())));
                    //        }
                    //        TxtBannerURL.Text = Convert.ToString(dr1["BannerURL"].ToString());
                    //        TxtDisplayOrder.Text = Convert.ToString(dr1["DisplayOrder"].ToString());
                    //        ddlTarget.Text = Convert.ToString(dr1["LinkTarget"].ToString());
                    //        chkActive.Checked = Convert.ToBoolean(dr1["Active"].ToString());
                    //        imghdn.Value = Convert.ToString(dr1["ImageName"].ToString());
                    //        if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdn.Value.ToString())))
                    //        {
                    //            Random rd = new Random();
                    //            imgBanner.Src = ViewState["Imagebannrpath"].ToString() + imghdn.Value.ToString() + "?" + rd.Next(10000).ToString();
                    //        }
                    //        hdnid.Value = Convert.ToString(dr1["BannerID"].ToString());
                    //        TxtbannerTitle.Text = Convert.ToString(dr1["Title"].ToString());
                    //        ddlPosition.SelectedValue = Convert.ToString(dr1["Pagination"].ToString());
                    //    }
                    //}
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
            }
            else
            {
                grdbannerlist.DataSource = null;
                grdbannerlist.DataBind();

            }
        }
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (Request.QueryString["Storeid"] == null)
            {
                return;
            }
            //if (ViewState["dsbanner"] != null && hdnid.Value.ToString().Trim() == "0")
            //{


            //}

            //HomeBannerComponent objHome = new HomeBannerComponent();
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

            //    //Rotatrid = Convert.ToInt32(objHome.InsertRotatebanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["Id"].ToString()), 1));
            //}
            //tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            //if (chkActive.Checked)
            //{
            //    objRotatorhome.Active = true;
            //}
            //else
            //{
            //    objRotatorhome.Active = false;
            //}
            //objRotatorhome.BannerURL = TxtBannerURL.Text.ToString();
            //objRotatorhome.Title = TxtbannerTitle.Text.ToString();
            //if (TxtDisplayOrder.Text.ToString().Trim() == "")
            //{
            //    objRotatorhome.DisplayOrder = 0;
            //}
            //else
            //{
            //    objRotatorhome.DisplayOrder = Convert.ToInt32(TxtDisplayOrder.Text.ToString());
            //}
            //objRotatorhome.IsMain = 1;
            //objRotatorhome.HomeRotatorId = Rotatrid;
            //objRotatorhome.StoreID = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            //objRotatorhome.Pagination = ddlPosition.SelectedValue.ToString();
            //objRotatorhome.LinkTarget = ddlTarget.SelectedValue.ToString();
            //objRotatorhome.StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            //objRotatorhome.EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());

            //if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
            //{
            //    Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));

            //}
            ////CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString()), Convert.ToInt32(Request.QueryString["StoreID"]));

            //if (FileUploadBanner.HasFile)
            //{
            //    string filename = "";
            //    FileInfo fl = new FileInfo(FileUploadBanner.FileName.ToString());

            //    for (int i = 1; i < 10; i++)
            //    {
            //        filename = Request.QueryString["id"].ToString() + "_Store_" + Request.QueryString["storeid"].ToString() + "_" + i.ToString() + "" + fl.Extension.ToString();
            //        if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
            //        {
            //            if (filename == imghdn.Value.ToString())
            //            {
            //                File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
            //                FileUploadBanner.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
            //                objRotatorhome.ImageName = filename;
            //                // CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename), Convert.ToInt32(Request.QueryString["StoreID"]));
            //                break;
            //            }
            //        }
            //        else
            //        {
            //            FileUploadBanner.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
            //            objRotatorhome.ImageName = filename;
            //            //CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename), Convert.ToInt32(Request.QueryString["StoreID"]));
            //            break;
            //        }
            //    }
            //}
            //else
            //{
            //    objRotatorhome.ImageName = imghdn.Value.ToString();
            //}
            //if (hdnid.Value.ToString() == "0")
            //{
            //    objRotatorhome.BannerID = 0;
            //    objHome.Insertbanner(objRotatorhome);
            //}
            //else
            //{
            //    objRotatorhome.BannerID = Convert.ToInt32(hdnid.Value.ToString());
            //    objHome.UpdatebannerDetail(objRotatorhome);

            //}
            Int32 Active = 0;
            if (chkActive.Checked)
            {
                Active = 1;
            }
            else
            {
                Active = 0;
            }
            string BannerURL = TxtBannerURL.Text.ToString();
            string Title = TxtbannerTitle.Text.ToString();
            Int32 DisplayOrder = 0;
            if (TxtDisplayOrder.Text.ToString().Trim() == "")
            {
                DisplayOrder = 0;
            }
            else
            {
                DisplayOrder = Convert.ToInt32(TxtDisplayOrder.Text.ToString());
            }
            Int32 IsMain = 1;

            Int32 StoreID = Convert.ToInt32(Request.QueryString["storeid"].ToString());
            string Pagination = ddlPosition.SelectedValue.ToString();
            string LinkTarget = ddlTarget.SelectedValue.ToString();
            DateTime StartDate = Convert.ToDateTime(txtStartDate.Text.ToString());
            DateTime EndDate = Convert.ToDateTime(txtEndDate.Text.ToString());
            string strdescription = txtDescription.Text.ToString();
            if (hdnid.Value.ToString() == "0")
            {
                string strSql = "";
                strSql = "INSERT INTO tb_HomesmallBannerDetail(StoreID,Title,BannerURL,ImageName,DisplayOrder,Active,CreatedOn,Description,Pagination,IsMain,LinkTarget,StartDate,EndDate)";
                strSql += " values (" + Request.QueryString["StoreId"].ToString() + ",'" + TxtbannerTitle.Text.ToString().Replace("'", "''") + "','" + TxtBannerURL.Text.ToString().Replace("'", "''") + "',''," + TxtDisplayOrder.Text.ToString() + "," + Active.ToString() + ",getdate(),'" + txtDescription.Text.ToString().Replace("'", "''") + "','" + ddlPosition.SelectedValue.ToString() + "',1,'" + ddlTarget.SelectedValue.ToString() + "','" + StartDate.ToString() + "','" + EndDate.ToString() + "'); SELECT SCOPE_IDENTITY();";
                Int32 bannerId = Convert.ToInt32(CommonComponent.GetScalarCommonData(strSql));
                if (FileUploadBanner.HasFile)
                {
                    string filename = "";
                    FileInfo fl = new FileInfo(FileUploadBanner.FileName.ToString());
                    filename = bannerId.ToString() + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
                    {
                        if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
                        {
                            Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));
                        }
                        if (filename == imghdn.Value.ToString())
                        {
                            File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            FileUploadBanner.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        }
                    }
                    else
                    {
                        if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
                        {
                            Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));
                        }
                        FileUploadBanner.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                    }
                    CommonComponent.ExecuteCommonData("UPDATE tb_HomesmallBannerDetail SET ImageName='" + filename + "' WHERE BannerID=" + bannerId + "");
                }
            }
            else
            {
                Int32 bannerId = Convert.ToInt32(hdnid.Value.ToString());
                string filename = "";
                if (FileUploadBanner.HasFile)
                {
                    if (!Directory.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString())))
                    {
                        Directory.CreateDirectory(Server.MapPath(ViewState["Imagebannrpath"].ToString()));
                    }
                    FileInfo fl = new FileInfo(FileUploadBanner.FileName.ToString());
                    filename = bannerId.ToString() + "" + fl.Extension.ToString();
                    if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
                    {
                        if (filename == imghdn.Value.ToString())
                        {
                            File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            FileUploadBanner.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                            CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        }
                    }
                    else
                    {
                        FileUploadBanner.SaveAs(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                        CommonOperations.SaveOnContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));

                    }

                }
                else
                {
                    filename = imghdn.Value.ToString();

                }
                CommonComponent.ExecuteCommonData("UPDATE tb_HomesmallBannerDetail SET Title='" + TxtbannerTitle.Text.ToString().Replace("'", "''") + "',BannerURL='" + TxtBannerURL.Text.ToString().Replace("'", "''") + "',DisplayOrder=" + TxtDisplayOrder.Text.ToString() + ",Active=" + Active.ToString() + ",Description='" + txtDescription.Text.ToString().Replace("'", "''") + "',Pagination='" + ddlPosition.SelectedValue.ToString() + "',LinkTarget='" + ddlTarget.SelectedValue.ToString() + "',StartDate='" + StartDate.ToString() + "',EndDate='" + EndDate.ToString() + "', ImageName='" + filename + "' WHERE BannerID=" + bannerId + "");
            }
            hdnid.Value = "0";
            imghdn.Value = "";
            TxtbannerTitle.Text = "";
            TxtBannerURL.Text = "";
            ddlPosition.SelectedIndex = 0;
            ddlTarget.SelectedIndex = 0;
            TxtDisplayOrder.Text = "";
            chkActive.Checked = false;
            txtDescription.Text = "";
            lblMsg.Text = "Record Saved Successfully.";

            imgBanner.Src = "/App_Themes/" + Page.Theme.ToString() + "/images/spacer.png";
            Getbanner();
            GetHTMl();
        }
        protected void imgSave1_Click(object sender, ImageClickEventArgs e)
        {


        }
        protected void imgSave2_Click(object sender, ImageClickEventArgs e)
        {

        }
        protected void imgSave3_Click(object sender, ImageClickEventArgs e)
        {

        }
        private void GetHTMl()
        {
            //string strHTML = "";
            //HomeBannerComponent objbanner = new HomeBannerComponent();
            //DataSet dsbanner = new DataSet();

            //dsbanner = objbanner.GetHomeRotatorBanner(Convert.ToInt32(Request.QueryString["StoreId"].ToString()), Convert.ToInt32(Request.QueryString["id"].ToString()));
            //string strPosition = "";

            //strHTML += "<div id=\"banner-part\">";
            //strHTML += "<div class=\"main-banner\">";
            //strHTML += "<div id=\"slider1\" class=\"contentslide\">";
            //strHTML += "<div class=\"opacitylayer\">";
            //if (dsbanner != null && dsbanner.Tables.Count > 0 && dsbanner.Tables[0].Rows.Count > 0)
            //{
            //    DataRow[] dr = dsbanner.Tables[0].Select("isnull(IsMain,0)=1");
            //    foreach (DataRow dr1 in dr)
            //    {
            //        strPosition = dr1["Pagination"].ToString();
            //        if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString()).ToString() + "/" + dr1["ImageName"].ToString()))
            //        {
            //            strHTML += "<div class=\"contentdiv\">";
            //            strHTML += "<div class=\"index-banner-1\">";
            //            if (dr1["BannerURL"].ToString().Trim() != "")
            //            {
            //                strHTML += " <div class=\"banner1\"><a href=\"" + dr1["BannerURL"].ToString() + "\" target=\"" + dr1["LinkTarget"].ToString().Trim() + "\"><img style=\"cursor:pointer;\"  src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></a></div>";
            //            }
            //            else
            //            {
            //                strHTML += " <div class=\"banner1\"><img  src=\"" + ViewState["Imagebannrpath"].ToString() + "/" + dr1["ImageName"].ToString() + "\" alt=\"" + dr1["Title"].ToString() + "\" Title=\"" + dr1["Title"].ToString() + "\" /></div>";
            //            }
            //            strHTML += " </div>";
            //            strHTML += " </div>";
            //        }
            //    }

            //}
            //strHTML += " </div>";
            //strHTML += " </div>";
            //strHTML += " </div>";

            //if (strPosition.ToString().ToLower() == "left")
            //{
            //    strHTML += " <div class=\"pagination\" id=\"paginate-slider1\"></div>";
            //}
            //else if (strPosition.ToString().ToLower() == "right")
            //{
            //    strHTML += " <div class=\"pagination-right\" id=\"paginate-slider1\"></div>";
            //}
            //else if (strPosition.ToString().ToLower() == "top")
            //{
            //    strHTML += " <div class=\"pagination-top\" id=\"paginate-slider1\"></div>";
            //}
            //else if (strPosition.ToString().ToLower() == "bottom")
            //{
            //    strHTML += " <div class=\"pagination-bottom\" id=\"paginate-slider1\"></div>";
            //}
            //strHTML += " <div id=\"play-btn\" class=\"play\"> <a href=\"javascript:ContentSlider(slider1,8000);\">&nbsp;</a> </div>";
            //strHTML += " <div id=\"paginate-new\" class=\"pause\"><a href=\"javascript:cancelautorun();\">&nbsp;</a></div>";
            //strHTML += " </div>";




            //Int32 StoreId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT TOp 1 StoreId FROM tb_HtmlText WHERE StoreId=" + Request.QueryString["Storeid"] + " AND HtmlType=1"));
            //if (StoreId > 0)
            //{
            //    CommonComponent.ExecuteCommonData("UPDATE tb_HtmlText SET HtmlText='" + strHTML.Replace("'", "''") + "' WHERE StoreId=" + Request.QueryString["Storeid"] + " AND HtmlType=1");
            //}
            //else
            //{
            //    CommonComponent.ExecuteCommonData("INSERT INTO tb_HtmlText (StoreId,HtmlText,HtmlType) values (" + Request.QueryString["Storeid"].ToString() + ",'" + strHTML.Replace("'", "''") + "',1)");
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
                    ltStatus.Text = "Yes";//"<span style='background-color: #58B058;color: #fff;width:50px;height:18px;font-size: 12px;line-height: 18px;padding-bottom: 3px;padding-top: 3px;'>&nbsp;&nbsp;<i class=\"icon-ok icon-white\"></i>&nbsp;&nbsp;</span>";
                }
                else
                {
                    ltStatus.Text = "No";// "<span style='background-color: #D14641;color: #fff;width:50px;height:18px;font-size: 12px;line-height: 18px;padding-bottom: 3px;padding-top: 3px;'>&nbsp;&nbsp;<i class=\"icon-remove\"></i>&nbsp;&nbsp;</span>";
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
                            txtDescription.Text = Convert.ToString(dr1["Description"].ToString());
                            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + imghdn.Value.ToString())))
                            {
                                Random rd = new Random();
                                imgBanner.Src = ViewState["Imagebannrpath"].ToString() + imghdn.Value.ToString() + "?" + rd.Next(10000).ToString();
                            }
                        }
                    }
                }
            }


        }

        protected void grdbannerlist_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            Int32 id = Convert.ToInt32(grdbannerlist.DataKeys[e.RowIndex].Value.ToString());
            string filename = Convert.ToString(DataBinder.Eval(grdbannerlist.Rows[e.RowIndex].DataItem, "ImageName"));
            //HomeBannerComponent objHome = new HomeBannerComponent();
            //tb_RotatorHomeBannerDetail objRotatorhome = new tb_RotatorHomeBannerDetail();
            //objRotatorhome.BannerID = id;
          //  objHome.DeleteBanner(objRotatorhome);
            CommonComponent.ExecuteCommonData("Delete from tb_HomesmallBannerDetail where BannerID=" + id + "");
           
            if (File.Exists(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename)))
            {
                File.Delete(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
                CommonOperations.DeleteFileFromContentServer(Server.MapPath(ViewState["Imagebannrpath"].ToString() + "/" + filename));
            }
            hdnid.Value = "0";
            TxtbannerTitle.Text = "";
            imghdn.Value = "";
            TxtBannerURL.Text = "";
            ddlPosition.SelectedIndex = 0;
            ddlTarget.SelectedIndex = 0;
            TxtDisplayOrder.Text = "";
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
            CommonComponent.ExecuteCommonData(@"UPDATE tb_RotatorHomeBannerDetail SET  tb_RotatorHomeBannerDetail.StartDate ='" + Convert.ToDateTime(txtStartDate.Text.Trim()) + @"',tb_RotatorHomeBannerDetail.EndDate='" + Convert.ToDateTime(txtEndDate.Text.Trim()) + @"'
                                                FROM dbo.tb_RotatorHomebanner INNER JOIN
                                                dbo.tb_RotatorHomeBannerDetail ON dbo.tb_RotatorHomebanner.HomeRotatorId = dbo.tb_RotatorHomeBannerDetail.HomeRotatorId
                                                WHERE tb_RotatorHomebanner.BannerTypeId =" + Convert.ToString(Request.QueryString["id"]) + @" AND tb_RotatorHomebanner.StoreID=" + Convert.ToString(Request.QueryString["storeid"]) + @"");

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "DateUpdated", "jAlert('Date updated successfully.','Message')", true);
        }
    }
}