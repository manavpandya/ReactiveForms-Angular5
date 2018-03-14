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
    public partial class EditItemPageBanner : BasePage
    {
        HomeBannerComponent objItemBanner = new HomeBannerComponent();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";

                bindstore();
                FillItemBannerType();

                if (!string.IsNullOrEmpty(Request.QueryString["ItemBannerID"]) && Convert.ToString(Request.QueryString["ItemBannerID"]) != "0")
                {
                    lblTitle.Text = "Edit Item Page Banner";
                    FillData(Convert.ToInt32(Request.QueryString["ItemBannerID"]));
                }
                else
                {
                    lblTitle.Text = "Add Item Page Banner";
                }
            }
        }

        public void FillItemBannerType()
        {
            DataSet dsItemBanner = new DataSet();
            dsItemBanner = objItemBanner.GetIemBannerTypes(Convert.ToInt32(ddlStore.SelectedValue));
            if (dsItemBanner != null && dsItemBanner.Tables.Count > 0 && dsItemBanner.Tables[0].Rows.Count > 0)
            {
                ddlBannerType.DataSource = dsItemBanner;
                ddlBannerType.DataTextField = "BannerTypeName";
                ddlBannerType.DataValueField = "ItemBannerTypeID";
                ddlBannerType.DataBind();
            }
            else
            {
                ddlBannerType.DataSource = dsItemBanner;
                ddlBannerType.DataBind();
            }
            if (ddlBannerType.SelectedValue.Trim() == "1")
            {
                lblBigImgSize.Attributes.Add("Style", "display:''");
                lblSmallImgSize.Attributes.Add("Style", "display:none");
            }
            else
            {
                lblBigImgSize.Attributes.Add("Style", "display:none");
                lblSmallImgSize.Attributes.Add("Style", "display:''");
            }
        }



        private void FillData(Int32 ItemBannerID)
        {
            Random rnd = new Random();
            HomeBannerComponent HomeBannerCom = new HomeBannerComponent();
            tb_ItemBanners tb_itembanners = new tb_ItemBanners();
            tb_itembanners = HomeBannerCom.GetItemPageBanner(Convert.ToInt32(Request.QueryString["ItemBannerID"]));
            if (tb_itembanners != null)
            {
                ddlStore.Enabled = false;
                TxtbannerTitle.Text = Convert.ToString(tb_itembanners.BannerTitle.Trim());
                TxtBannerURL.Text = Convert.ToString(tb_itembanners.BannerUrl.Trim());
                ddlBannerType.ClearSelection();
                ddlBannerType.SelectedValue = Convert.ToString(tb_itembanners.ItemBannerTypeID);
                if (ddlBannerType.SelectedValue.Trim() == "1")
                {
                    lblBigImgSize.Attributes.Add("Style", "display:''");
                    lblSmallImgSize.Attributes.Add("Style", "display:none");
                }
                else
                {
                    lblBigImgSize.Attributes.Add("Style", "display:none");
                    lblSmallImgSize.Attributes.Add("Style", "display:''");
                }
                ddlBannerType.Enabled = false;
                txtStartDate.Text = Convert.ToString(Convert.ToDateTime(tb_itembanners.StartDate).ToShortDateString());
                txtEndDate.Text = Convert.ToString(Convert.ToDateTime(tb_itembanners.EndDate).ToShortDateString());
                txtAllowedProducts.Text = Convert.ToString(tb_itembanners.Products.Trim());
                String strImagePath = AppLogic.AppConfigs("ImagePathBanner") + "Item";
                if (Convert.ToString(tb_itembanners.BannerImage.ToString()) != "")
                {
                    if (File.Exists(Server.MapPath(strImagePath + "/" + tb_itembanners.BannerImage.Trim())))
                    {
                        imgBanner.Src = strImagePath + "/" + tb_itembanners.BannerImage.Trim() + "?" + rnd.Next();
                        imgBanner.Visible = true;
                    }
                }
                TxtDisplayOrder.Text = Convert.ToString(tb_itembanners.DisplayOrder);
                chkActive.Checked = Convert.ToBoolean(tb_itembanners.Active.Value);
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
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (TxtbannerTitle.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Item Page Banner Title.', 'Message');});", true);
                return;
            }
            if (txtStartDate.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Select Start Date.', 'Message');});", true);
                return;
            }
            if (txtEndDate.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Select End Date.', 'Message');});", true);
                return;
            }
            if (Convert.ToDateTime(txtStartDate.Text) > Convert.ToDateTime(txtEndDate.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Select Valid Date for Duration.', 'Message');});", true);
                return;
            }
            if (Request.QueryString["ItemBannerID"] == null && FileUploadBanner.HasFile == false)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select banner to upload.', 'Message');});", true);
                return;
            }
            HomeBannerComponent HomeBannerCom = new HomeBannerComponent();
            tb_ItemBanners tb_itembanners = new tb_ItemBanners();
            CreateFolder(AppLogic.AppConfigs("ImagePathBanner") + "Item");

            if (!string.IsNullOrEmpty(Request.QueryString["ItemBannerID"]) && Convert.ToString(Request.QueryString["ItemBannerID"]) != "0")
            {
                ddlStore.Enabled = false;
                tb_itembanners = HomeBannerCom.GetItemPageBanner(Convert.ToInt32(Request.QueryString["ItemBannerID"]));
                tb_itembanners.ItemBannerTypeID = Convert.ToInt32(ddlBannerType.SelectedValue);
                tb_itembanners.BannerTitle = TxtbannerTitle.Text;
                tb_itembanners.BannerUrl = TxtBannerURL.Text;
                tb_itembanners.Active = chkActive.Checked;
                if (TxtDisplayOrder.Text == "")
                {
                    tb_itembanners.DisplayOrder = 999;
                }
                else
                {
                    tb_itembanners.DisplayOrder = Convert.ToInt32(TxtDisplayOrder.Text);
                }

                tb_itembanners.UpdatedOn = DateTime.Now;
                tb_itembanners.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                tb_itembanners.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                tb_itembanners.EndDate = Convert.ToDateTime(txtEndDate.Text);
                tb_itembanners.Products = Convert.ToString(txtAllowedProducts.Text.ToString());

                Int32 isupdated = HomeBannerCom.UpdateItemBanner(tb_itembanners);
                bool IsImageUploaded = false;

                if (isupdated > 0)
                {
                    if (FileUploadBanner.FileName.Length > 0)
                    {
                        IsImageUploaded = true;
                        FileUploadBanner.PostedFile.SaveAs(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "Item/" + isupdated + ".jpg"));
                    }
                    if (IsImageUploaded)
                    {
                        CommonComponent.ExecuteCommonData("UPDATE dbo.tb_ItemBanners SET BannerImage='" + isupdated + ".jpg' WHERE ItemBannerID=" + isupdated);
                    }
                    Response.Redirect("ItemPageBanner.aspx?status=updated");
                }
                else if (isupdated == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record already exists.', 'Message');});", true);
                    return;
                }
            }
            else
            {
                tb_itembanners.BannerTitle = TxtbannerTitle.Text.Trim();
                tb_itembanners.ItemBannerTypeID = Convert.ToInt32(ddlBannerType.SelectedValue);
                tb_itembanners.BannerUrl = TxtBannerURL.Text.Trim();
                tb_itembanners.Active = chkActive.Checked;
                if (TxtDisplayOrder.Text == "")
                {
                    tb_itembanners.DisplayOrder = 999;
                }
                else
                {
                    tb_itembanners.DisplayOrder = Convert.ToInt32(TxtDisplayOrder.Text);
                }

                tb_itembanners.CreatedOn = DateTime.Now;
                tb_itembanners.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tb_itembanners.StartDate = Convert.ToDateTime(txtStartDate.Text.Trim());
                tb_itembanners.EndDate = Convert.ToDateTime(txtEndDate.Text);
                tb_itembanners.Products = Convert.ToString(txtAllowedProducts.Text.ToString());

                Int32 isAdded = HomeBannerCom.CreateItemBanner(tb_itembanners);
                bool IsImageUploaded = false;

                if (isAdded > 0)
                {
                    if (FileUploadBanner.FileName.Length > 0)
                    {
                        IsImageUploaded = true;
                        FileUploadBanner.PostedFile.SaveAs(Server.MapPath(AppLogic.AppConfigs("ImagePathBanner") + "Item/" + isAdded + ".jpg"));
                    }
                    if (IsImageUploaded)
                    {
                        CommonComponent.ExecuteCommonData("UPDATE dbo.tb_ItemBanners SET BannerImage='" + isAdded + ".jpg' WHERE ItemBannerID=" + isAdded);
                    }
                    Response.Redirect("ItemPageBanner.aspx?status=Inserted");
                }
                else if (isAdded == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Record already exists.', 'Message');});", true);
                    return;
                }
            }
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
        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("ItemPageBanner.aspx");
        }

    }
}