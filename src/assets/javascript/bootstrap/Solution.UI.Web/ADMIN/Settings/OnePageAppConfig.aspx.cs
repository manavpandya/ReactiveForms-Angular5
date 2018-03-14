using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Collections;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class OnePageAppConfig : BasePage
    {
        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/update.png";
            btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
            if (!IsPostBack)
            {
                bindstore();
                BindData(Convert.ToInt32(ddlStore.SelectedValue));
            }
        }

        /// <summary>
        /// Get Value to display App config value
        /// </summary>
        /// <param name="Ds">Datset Ds</param>
        /// <param name="StoreID">int StoreID</param>
        private void GetValue(DataSet Ds, int StoreID)
        {
            if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
            {
                Hashtable htAppConfig = new Hashtable();
                for (int cnt = 0; cnt < Ds.Tables[0].Rows.Count; cnt++)
                {
                    htAppConfig.Add((string.IsNullOrEmpty(Ds.Tables[0].Rows[cnt]["StoreID"].ToString()) ? "1" : Ds.Tables[0].Rows[cnt]["StoreID"].ToString()) + Ds.Tables[0].Rows[cnt]["configname"].ToString(), Ds.Tables[0].Rows[cnt]["configvalue"].ToString());

                }
                ViewState.Add("Hastable", htAppConfig);
                txtStorePhone.Text = (htAppConfig[StoreID + "StorePhoneNumber"] == null) ? "" : htAppConfig[StoreID + "StorePhoneNumber"].ToString();
                txtSETitle.Text = (htAppConfig[StoreID + "SiteSETitle"] == null) ? "" : htAppConfig[StoreID + "SiteSETitle"].ToString();
                txtSEKeywords.Text = (htAppConfig[StoreID + "SiteSEKeywords"] == null) ? "" : htAppConfig[StoreID + "SiteSEKeywords"].ToString();
                txtSeDescription.Text = (htAppConfig[StoreID + "SiteSEDescription"] == null) ? "" : htAppConfig[StoreID + "SiteSEDescription"].ToString();
                txtStoreClosedMsg.Text = (htAppConfig[StoreID + "StoreClosedMessage"] == null) ? "" : htAppConfig[StoreID + "StoreClosedMessage"].ToString();
                chkUseSSL.Checked = (htAppConfig[StoreID + "UseSSL"] == null) ? false : (htAppConfig[StoreID + "UseSSL"].ToString().ToLower() == "true") ? true : false;
                chkStoreOn.Checked = (htAppConfig[StoreID + "IsStoreon"] == null) ? false : (htAppConfig[StoreID + "IsStoreon"].ToString().ToLower() == "true") ? true : false;
            }
        }

        /// <summary>
        /// Bind data with grid
        /// </summary>
        /// <param name="StoreID">int StoreID</param>
        private void BindData(int StoreID)
        {
            ConfigurationComponent ConfigComponent = new ConfigurationComponent();
            DataSet DsAppConfig = ConfigComponent.GetMailConfig("", "", StoreID, DateTime.MaxValue, 1, 1);
            GetValue(DsAppConfig, StoreID);

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
                ddlStore.SelectedIndex = 0;
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
        /// Store Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlStore_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlStore.SelectedValue.ToString() == "0" || ddlStore.SelectedValue.ToString() == "-1")
            {
                AppConfig.StoreID = 1;
            }
            else
            {
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            if (ddlStore.SelectedItem != null)
                BindData(Convert.ToInt32(ddlStore.SelectedValue));
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (!chkStoreOn.Checked)
            {
                if (string.IsNullOrEmpty(txtStoreClosedMsg.Text.ToString().Trim()))
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter Store Closed Message.', 'Message','');});", true);
                return;
            }
            Hashtable hastable = new Hashtable();

            if (ViewState["Hastable"] != null)
            {
                hastable = (Hashtable)ViewState["Hastable"];
            }

            if (hastable != null)
            {
                DateTime UpdatedOn = DateTime.Now;
                int UpdatedBy = int.Parse(Session["AdminID"].ToString());

                UpdateThis("StorePhoneNumber", Convert.ToInt32(ddlStore.SelectedValue), txtStorePhone.Text.ToString(), UpdatedOn, UpdatedBy);
                UpdateThis("SiteSETitle", Convert.ToInt32(ddlStore.SelectedValue), txtSETitle.Text.ToString().Trim(), UpdatedOn, UpdatedBy);
                UpdateThis("SiteSEKeywords", Convert.ToInt32(ddlStore.SelectedValue), txtSEKeywords.Text.ToString().Trim(), UpdatedOn, UpdatedBy);
                UpdateThis("SiteSEDescription", Convert.ToInt32(ddlStore.SelectedValue), txtSeDescription.Text.ToString().Trim(), UpdatedOn, UpdatedBy);
                UpdateThis("StoreClosedMessage", Convert.ToInt32(ddlStore.SelectedValue), txtStoreClosedMsg.Text.ToString().Trim(), UpdatedOn, UpdatedBy);
                UpdateThis("UseSSL", Convert.ToInt32(ddlStore.SelectedValue), chkUseSSL.Checked.ToString(), UpdatedOn, UpdatedBy);
                UpdateThis("IsStoreon", Convert.ToInt32(ddlStore.SelectedValue), chkStoreOn.Checked.ToString(), UpdatedOn, UpdatedBy);

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('One Page Application Configuration Updated Successfully.', 'Message','');});", true);
            }
        }

        /// <summary>
        /// Function For Update Data
        /// </summary>
        /// <param name="ConfigName">string ConfigName</param>
        /// <param name="StoreID">int StoreID</param>
        /// <param name="ConfigValue">string ConfigValue</param>
        /// <param name="UpdatedOn">DateTime UpdatedOn</param>
        /// <param name="UpdatedBy">int UpdatedBy</param>
        private void UpdateThis(string ConfigName, int StoreID, string ConfigValue, DateTime UpdatedOn, int UpdatedBy)
        {
            ConfigurationComponent.UpdateMailConfig(ConfigName, ConfigValue, StoreID, UpdatedOn, UpdatedBy, 2);
        }

        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/Dashboard.aspx");
        }
    }
}