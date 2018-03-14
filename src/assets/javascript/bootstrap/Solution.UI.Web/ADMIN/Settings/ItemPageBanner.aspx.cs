using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.AdminCommon;
using System.Data;
using System.IO;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class ItemPageBanner : BasePage
    {
        Random rnd = new Random();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                bindstore();
                GetBigTypeBanner();
                GetSmallTypeBanner();
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
        public void GetBigTypeBanner()
        {
            HomeBannerComponent objItemBanner = new HomeBannerComponent();
            DataSet dsBannerType = new DataSet();
            dsBannerType = objItemBanner.GetItemPageBanners(1, Convert.ToInt32(ddlStore.SelectedValue));

            if (dsBannerType != null && dsBannerType.Tables.Count > 0 && dsBannerType.Tables[0].Rows.Count > 0)
            {
                gvBigBannerType.DataSource = dsBannerType;
                gvBigBannerType.DataBind();
            }
            else
            {
                gvBigBannerType.DataSource = null;
                gvBigBannerType.DataBind();
            }
        }

        public void GetSmallTypeBanner()
        {
            HomeBannerComponent objItemBanner = new HomeBannerComponent();
            DataSet dsBannerType = new DataSet();
            dsBannerType = objItemBanner.GetItemPageBanners(2, Convert.ToInt32(ddlStore.SelectedValue));

            if (dsBannerType != null && dsBannerType.Tables.Count > 0 && dsBannerType.Tables[0].Rows.Count > 0)
            {
                gvSmallBannerType.DataSource = dsBannerType;
                gvSmallBannerType.DataBind();
            }
            else
            {
                gvSmallBannerType.DataSource = null;
                gvSmallBannerType.DataBind();
            }
        }

        /// <summary>
        /// Big Banner Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvBigBannerType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int ItemBannerID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("/Admin/Settings/EditItemPageBanner.aspx?ItemBannerID=" + ItemBannerID);
            }
            else if (e.CommandName == "delete")
            {
                int ItemBannerID = Convert.ToInt32(e.CommandArgument);
                CommonComponent.ExecuteCommonData("DELETE FROM dbo.tb_ItemBanners WHERE ItemBannerID=" + ItemBannerID);
                GetBigTypeBanner();
            }

            else if (e.CommandName == "ActiveBanner")
            {
                int ItemBannerID = Convert.ToInt32(e.CommandArgument);
                CommonComponent.ExecuteCommonData("UPDATE dbo.tb_ItemBanners SET Active=0 WHERE ItemBannerID<>" + ItemBannerID + ";UPDATE dbo.tb_ItemBanners SET Active=1 WHERE ItemBannerID=" + ItemBannerID);
                GetBigTypeBanner();
            }
        }

        /// <summary>
        /// Small Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void gvSmallBannerType_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int ItemBannerID = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("/Admin/Settings/EditItemPageBanner.aspx?ItemBannerID=" + ItemBannerID);
            }
            else if (e.CommandName == "delete")
            {
                int ItemBannerID = Convert.ToInt32(e.CommandArgument);
                CommonComponent.ExecuteCommonData("DELETE FROM dbo.tb_ItemBanners WHERE ItemBannerID=" + ItemBannerID);
                GetSmallTypeBanner();
            }
            else if (e.CommandName == "ActiveBanner")
            {
                int ItemBannerID = Convert.ToInt32(e.CommandArgument);
                CommonComponent.ExecuteCommonData("UPDATE dbo.tb_ItemBanners SET Active=0 WHERE ItemBannerID<>" + ItemBannerID + ";UPDATE dbo.tb_ItemBanners SET Active=1 WHERE ItemBannerID=" + ItemBannerID);
                GetSmallTypeBanner();
            }
        }
        protected void gvBigBannerType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delete-icon.png";
                Image imgBannerImage = (Image)e.Row.FindControl("imgBannerImage");
                Image imgIsActive = (Image)e.Row.FindControl("imgIsActive");

                String strImageName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "BannerImage"));
                if (strImageName.Trim() != "")
                {
                    String strImagePath = AppLogic.AppConfigs("ImagePathBanner") + "Item";

                    if (File.Exists(Server.MapPath(strImagePath + "/" + strImageName.Trim())))
                    {
                        imgBannerImage.ImageUrl = strImagePath + "/" + strImageName.Trim() + "?" + rnd.Next();
                        imgBannerImage.Visible = true;
                    }
                }

                Boolean IsActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Active"));
                if (IsActive)
                {
                    imgIsActive.ImageUrl = "/Admin/Images/active.gif";
                    imgIsActive.ToolTip = "this is active banner.";
                }
                else
                {
                    imgIsActive.ImageUrl = "/Admin/Images/in-active.gif";
                    imgIsActive.ToolTip = "Click here to active this banner.";
                }
            }
        }
        protected void gvSmallBannerType_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/delete-icon.png";
                Image imgBannerImage = (Image)e.Row.FindControl("imgBannerImage");
                Image imgIsActive = (Image)e.Row.FindControl("imgIsActive");

                String strImageName = Convert.ToString(DataBinder.Eval(e.Row.DataItem, "BannerImage"));
                if (strImageName.Trim() != "")
                {
                    String strImagePath = AppLogic.AppConfigs("ImagePathBanner") + "Item";

                    if (File.Exists(Server.MapPath(strImagePath + "/" + strImageName.Trim())))
                    {
                        imgBannerImage.ImageUrl = strImagePath + "/" + strImageName.Trim() + "?" + rnd.Next();
                        imgBannerImage.Visible = true;
                    }
                }

                Boolean IsActive = Convert.ToBoolean(DataBinder.Eval(e.Row.DataItem, "Active"));
                if (IsActive)
                {
                    imgIsActive.ImageUrl = "/Admin/Images/active.gif";
                    imgIsActive.ToolTip = "this is active banner.";
                }
                else
                {
                    imgIsActive.ImageUrl = "/Admin/Images/in-active.gif";
                    imgIsActive.ToolTip = "Click here to active this banner.";
                }
            }
        }

        protected void gvSmallBannerType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gvBigBannerType_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}