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
using System.Data;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class EditHomePageBanner : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                lblTitle.Text = "Edit Home Page Banner";

                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                if (!string.IsNullOrEmpty(Request.QueryString["ID"]))
                {
                    FillCountry(Convert.ToInt32(Request.QueryString["ID"]));
                }
                bindstore();
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
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
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (TxtbannerTitle.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter Banner Title.', 'Message');});", true);
                return;
            }
            if (TxtBannerURL.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter Banner URL.', 'Message');});", true);
                return;
            }

            HomeBannerComponent HomeBannerCom = new HomeBannerComponent();
            tb_HomeBanner tb_HomeBanner = new tb_HomeBanner();

            if (!string.IsNullOrEmpty(Request.QueryString["ID"]) && Convert.ToString(Request.QueryString["ID"]) != "0")
            {
                ddlStore.Enabled = false;
                tb_HomeBanner = HomeBannerCom.getHomePageBanner(Convert.ToInt32(Request.QueryString["ID"]));
                tb_HomeBanner.Title = TxtbannerTitle.Text;
                tb_HomeBanner.BannerURL = TxtBannerURL.Text;
                tb_HomeBanner.Active = chkActive.Checked;
                if (TxtDisplayOrder.Text == "")
                {
                    tb_HomeBanner.DisplayOrder = 999;
                }
                else
                {
                    tb_HomeBanner.DisplayOrder = Convert.ToInt32(TxtDisplayOrder.Text);
                }

                tb_HomeBanner.CreatedOn = DateTime.Now;

                Int32 isupdated = HomeBannerCom.UpdateHomeBanner(tb_HomeBanner);
                if (FileUploadBanner.FileName.Length > 0)
                {
                    FileUploadBanner.PostedFile.SaveAs(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "Home/" + isupdated + ".jpg"));
                }
                if (isupdated > 0)
                {
                    Response.Redirect("HomePageBanner.aspx?status=updated");
                }
                else if (isupdated == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record already exists.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                ddlStore.Enabled = true;
                tb_HomeBanner.Title = TxtbannerTitle.Text;
                tb_HomeBanner.BannerURL = TxtBannerURL.Text;
                //tb_HomeBanner.Active = true;
                tb_HomeBanner.Active = chkActive.Checked;
                if (TxtDisplayOrder.Text == "")
                {
                    tb_HomeBanner.DisplayOrder = 999;
                }
                else
                {
                    tb_HomeBanner.DisplayOrder = Convert.ToInt32(TxtDisplayOrder.Text);
                }

                tb_HomeBanner.CreatedOn = DateTime.Now;

                int StoreId = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                tb_HomeBanner.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId);
                Int32 isadded = HomeBannerCom.CreateBanner(tb_HomeBanner);
                if (FileUploadBanner.FileName.Length > 0)
                {
                    FileUploadBanner.PostedFile.SaveAs(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "Home/" + isadded + ".jpg"));
                }
                if (isadded > 0)
                {
                    Response.Redirect("HomePageBanner.aspx?status=inserted");
                }
                else if (isadded == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record already exists.', 'Message');});", true);
                    return;
                }

            }


        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("HomePageBanner.aspx");
        }

        /// <summary>
        /// Fills the Country
        /// </summary>
        /// <param name="Id">int Id</param>
        private void FillCountry(Int32 Id)
        {

            HomeBannerComponent LoadBanner = new HomeBannerComponent();
            tb_HomeBanner tb_HomeBanner = LoadBanner.getHomePageBanner(Id);
            TxtbannerTitle.Text = tb_HomeBanner.Title;
            TxtBannerURL.Text = tb_HomeBanner.BannerURL;
            chkActive.Checked = Convert.ToBoolean(tb_HomeBanner.Active);
            if (Convert.ToString(tb_HomeBanner.DisplayOrder) == "999")
            {
                TxtDisplayOrder.Text = "";
            }
            else
            {
                TxtDisplayOrder.Text = Convert.ToString(tb_HomeBanner.DisplayOrder);
            }
            if (System.IO.File.Exists(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "Home/" + Request.QueryString["ID"].ToString() + ".jpg")))
            {
                imgBanner.Src = AppLogic.AppConfigs("ImagePathBanner") + "Home/" + Request.QueryString["ID"].ToString() + ".jpg";
                imgBanner.Visible = true;
            }
            ddlStore.Enabled = false;
        }
    }
}