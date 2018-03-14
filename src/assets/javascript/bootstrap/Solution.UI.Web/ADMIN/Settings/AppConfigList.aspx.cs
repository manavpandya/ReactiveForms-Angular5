using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class AppConfigList : Solution.UI.Web.BasePage
    {
        #region Declaration
        public static bool isDescendConfigName = false;
        public static bool isDescendConfigValue = false;
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
                btnSearch.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/search.gif) no-repeat transparent; width: 67px; height: 23px; border:none;cursor:pointer;");
                btnShowall.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/showall.png) no-repeat transparent; width: 70px; height: 23px; border:none;cursor:pointer;");
                btnDeleteConfig.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/Images/delet.gif) no-repeat transparent; width: 57px; height: 23px; border:none;cursor:pointer;");
                if (!string.IsNullOrEmpty(Request.QueryString["status"]))
                {
                    String strStatus = Convert.ToString(Request.QueryString["status"]);
                    if (strStatus == "inserted")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Application configuration inserted successfully.', 'Message','');});", true);

                    }
                    else if (strStatus == "updated")
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Application configuration updated successfully.', 'Message','');});", true);

                    }
                }
                bindstore();
            }
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
            grdApplicationConfig.DataBind();
            if (grdApplicationConfig.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        ///  Search Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            //Filter record based on selected field and searchvalue
            grdApplicationConfig.PageIndex = 0;
            grdApplicationConfig.DataBind();
            if (grdApplicationConfig.Rows.Count == 0)
                trBottom.Visible = false;
        }

        /// <summary>
        /// Sort GridView in Asc or DESC order
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Sorting(object sender, EventArgs e)
        {
            ImageButton lb = (ImageButton)sender;
            if (lb != null)
            {
                if (lb.CommandArgument == "ASC")
                {
                    grdApplicationConfig.Sort(lb.CommandName.ToString(), SortDirection.Ascending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    if (lb.ID == "btnConfigName")
                    {
                        isDescendConfigName = false;
                    }
                    else if (lb.ID == "btnConfigValue")
                    {
                        isDescendConfigValue = false;
                    }

                    lb.AlternateText = "Descending Order";
                    lb.ToolTip = "Descending Order";
                    lb.CommandArgument = "DESC";
                }
                else if (lb.CommandArgument == "DESC")
                {
                    grdApplicationConfig.Sort(lb.CommandName.ToString(), SortDirection.Descending);
                    lb.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    if (lb.ID == "btnConfigName")
                    {
                        isDescendConfigName = true;
                    }
                    else if (lb.ID == "btnConfigValue")
                    {
                        isDescendConfigValue = true;
                    }
                    lb.AlternateText = "Ascending Order";
                    lb.ToolTip = "Ascending Order";
                    lb.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Delete Config Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnDeleteConfig_Click(object sender, EventArgs e)
        {
            ConfigurationComponent objAppComp = new ConfigurationComponent();
            int totalRowCount = grdApplicationConfig.Rows.Count;
            for (int i = 0; i < totalRowCount; i++)
            {
                HiddenField hdn = (HiddenField)grdApplicationConfig.Rows[i].FindControl("hdnConfigid");
                CheckBox chk = (CheckBox)grdApplicationConfig.Rows[i].FindControl("chkSelect");
                if (chk.Checked == true)
                {
                    tb_AppConfig objAppConfig = null;
                    objAppConfig = objAppComp.getAppConfig(Convert.ToInt16(hdn.Value));
                    objAppConfig.Deleted = true;
                    objAppComp.UpdateAppConfiguration(objAppConfig);
                }
            }
            grdApplicationConfig.DataBind();
        }

        /// <summary>
        /// Application Config Gridview Row Command Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewCommandEventArgs e</param>
        protected void grdApplicationConfig_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "edit")
            {
                int ConfigId = Convert.ToInt32(e.CommandArgument);
                Response.Redirect("AppConfiguration.aspx?AppConfigID=" + ConfigId); //Edit application configuration

            }

            else if (e.CommandName == "DeleteAppConfig")
            {
                int ConfigId = Convert.ToInt32(e.CommandArgument);
                ConfigurationComponent objAppComp = new ConfigurationComponent();
                tb_AppConfig objAppConfig = null;
                objAppConfig = objAppComp.getAppConfig(ConfigId);
                objAppConfig.Deleted = true;
                objAppComp.UpdateAppConfiguration(objAppConfig); //delete application configuratin
                this.Response.Redirect("AppConfigList.aspx", false);
            }
        }

        /// <summary>
        /// Bind Store dropdown
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
            ddlStore.Items.Insert(0, new ListItem("All Stores", "-1"));
            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;
        }

        /// <summary>
        /// Application Config Gridview Row Data Bound Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">GridViewRowEventArgs e</param>
        protected void grdApplicationConfig_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (grdApplicationConfig.Rows.Count > 0)
            {
                trBottom.Visible = true;
            }
            else
            {
                trBottom.Visible = false;
            }
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ((ImageButton)e.Row.FindControl("_editLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/Images/Edit.gif";
                ((ImageButton)e.Row.FindControl("_deleteLinkButton")).ImageUrl = "/App_Themes/" + Page.Theme + "/images/Delete.gif";
            }
            if (e.Row.RowType == DataControlRowType.Header)
            {
                if (isDescendConfigName == false)
                {
                    ImageButton btnConfigName = (ImageButton)e.Row.FindControl("btnConfigName");
                    btnConfigName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnConfigName.AlternateText = "Ascending Order";
                    btnConfigName.ToolTip = "Ascending Order";
                    btnConfigName.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnConfigName = (ImageButton)e.Row.FindControl("btnConfigName");
                    btnConfigName.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnConfigName.AlternateText = "Descending Order";
                    btnConfigName.ToolTip = "Descending Order";
                    btnConfigName.CommandArgument = "ASC";
                }
                if (isDescendConfigValue == false)
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnConfigValue");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date.png";
                    btnConfigValue.AlternateText = "Ascending Order";
                    btnConfigValue.ToolTip = "Ascending Order";
                    btnConfigValue.CommandArgument = "DESC";
                }
                else
                {
                    ImageButton btnConfigValue = (ImageButton)e.Row.FindControl("btnConfigValue");
                    btnConfigValue.ImageUrl = "/App_Themes/" + Page.Theme + "/icon/order-date-up.png";
                    btnConfigValue.AlternateText = "Descending Order";
                    btnConfigValue.ToolTip = "Descending Order";
                    btnConfigValue.CommandArgument = "ASC";
                }
            }
        }

        /// <summary>
        ///  Show All Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnShowall_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            ddlStore.SelectedIndex = 0;
            grdApplicationConfig.PageIndex = 0;
            grdApplicationConfig.DataBind();
        }
    }
}