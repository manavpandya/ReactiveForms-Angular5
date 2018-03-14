using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class AppConfiguration : Solution.UI.Web.BasePage
    {
        #region Variable declaration
        tb_AppConfig appConfig = null;
        ConfigurationComponent objAppComp = null;
        int StoreId = 0;
        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                bindstore();
                if (!string.IsNullOrEmpty(Request.QueryString["AppConfigID"]) && Convert.ToString(Request.QueryString["AppConfigID"]) != "0")
                {
                    appConfig = new tb_AppConfig();
                    objAppComp = new ConfigurationComponent();
                    //Display selected App configuration detail for edit mode
                    appConfig = objAppComp.getAppConfig(Convert.ToInt32(Request.QueryString["AppConfigID"]));

                    lblHeader.Text = "Edit Application Configuration";
                    ddlStore.Enabled = false;
                    txtConfigName.Text = appConfig.ConfigName;
                    txtConfigValue.Text = appConfig.ConfigValue;
                    txtDesc.Text = appConfig.Description;
                    StoreId = appConfig.tb_StoreReference.Value.StoreID;
                    ddlStore.SelectedValue = StoreId.ToString();


                }
            }
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {

            objAppComp = new ConfigurationComponent();
            appConfig = new tb_AppConfig();
            //Update application configuration
            if (!string.IsNullOrEmpty(Request.QueryString["AppConfigID"]) && Convert.ToString(Request.QueryString["AppConfigID"]) != "0")
            {
                appConfig = objAppComp.getAppConfig(Convert.ToInt32(Request.QueryString["AppConfigID"]));
                appConfig.ConfigName = txtConfigName.Text.Trim();
                appConfig.ConfigValue = txtConfigValue.Text.Trim();
                appConfig.Description = txtDesc.Text.Trim();
                appConfig.Deleted = false;
                appConfig.UpdatedOn = DateTime.Now;
                appConfig.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                if (objAppComp.CheckDuplicate(appConfig))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Application Configuration already exists.', 'Message');});", true);
                    return;
                }
                Int32 isupdated = objAppComp.UpdateAppConfiguration(appConfig);
                if (isupdated > 0)
                {
                    Response.Redirect("AppConfigList.aspx?status=updated");
                }

            }
            else
            {
                //insert new store configuration
                appConfig.ConfigName = txtConfigName.Text.Trim();
                int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
                appConfig.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                //Check config name already exists or not
                if (objAppComp.CheckDuplicate(appConfig))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Application Configuration already exists.', 'Message');});", true);
                    return;
                }
                appConfig.ConfigValue = txtConfigValue.Text.Trim();
                appConfig.Description = txtDesc.Text.Trim();
                appConfig.CreatedOn = DateTime.Now;
                appConfig.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                appConfig.Deleted = false;
                objAppComp = new ConfigurationComponent();

                Int32 isadded = objAppComp.CreateAppConfiguration(appConfig);

                if (isadded > 0)
                {
                    Response.Redirect("AppConfigList.aspx?status=inserted");
                }

            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("AppConfigList.aspx");
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
            }
        }
    }
}