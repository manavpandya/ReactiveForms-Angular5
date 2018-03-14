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

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class DashboardSetting : Solution.UI.Web.BasePage
    {
        #region Declaration

        StoreComponent stac = new StoreComponent();
        DashboardComponent dashcomp = new DashboardComponent();

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
                BindStore();
                BindAdminList();
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                btnClone.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/clone.gif";
                string theme = "/App_Themes/" + Page.Theme.ToString() + "/images/title-bg.jpg";
                test.Attributes.Add("style", "background:url(" + theme + ");height:26px;border:1px solid #696969;");
            }
            //BindCloneStore();
        }

        /// <summary>
        /// Binds the Admin List.
        /// </summary>
        private void BindAdminList()
        {
            DataSet dsAdminList = new DataSet();
            dashcomp = new DashboardComponent();
            dsAdminList = dashcomp.GetAdminListForDashboardSetting();
            if (dsAdminList != null && dsAdminList.Tables.Count > 0 && dsAdminList.Tables[0].Rows.Count > 0)
            {
                ddlAdmins.DataSource = dsAdminList;
                ddlAdmins.DataTextField = "AdminName";
                ddlAdmins.DataValueField = "AdminID";
                ddlAdmins.DataBind();
            }
        }

        /// <summary>
        /// Bind All Stores in Drop down
        /// </summary>
        public void BindStore()
        {
            List<tb_Store> Storelist = stac.GetStore();
            if (Storelist != null)
            {
                ddlstore.DataSource = Storelist;
                ddlstore.DataTextField = "StoreName";
                ddlstore.DataValueField = "StoreID";
            }
            else
            {
                ddlstore.DataSource = null;
            }
            ddlstore.DataBind();

            ddlstore.Items.Insert(0, new ListItem("All Store", "0"));

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlstore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlstore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;
        }

        public void BindCloneStore()
        {
            DataSet dsCloneStore = new DataSet();
            dashcomp = new DashboardComponent();
            dsCloneStore = dashcomp.GetCloneStoreID(Convert.ToInt32(ddlstore.SelectedValue));
            if (dsCloneStore != null && dsCloneStore.Tables.Count > 0 && dsCloneStore.Tables[0].Rows.Count > 0)
            {
                ddlCloneStore.DataSource = dsCloneStore;
                ddlCloneStore.DataTextField = "StoreName";
                ddlCloneStore.DataValueField = "StoreID";
            }
            else
            {
                ddlCloneStore.DataSource = null;
            }
            ddlCloneStore.DataBind();
            if (dsCloneStore != null && dsCloneStore.Tables.Count > 1 && dsCloneStore.Tables[1].Rows.Count > 0)
            {
                if (Convert.ToInt32(dsCloneStore.Tables[1].Rows[0]["ClnCount"]) != 0)
                    ddlCloneStore.Items.Insert(0, new ListItem("All Store", "0"));
            }

            if (ddlCloneStore.Items.Count > 0)
            {
                lblCloneHeader.Visible = true;
                ddlCloneStore.Visible = true;
                btnClone.Visible = true;
            }
            else
            {
                lblCloneHeader.Visible = false;
                ddlCloneStore.Visible = false;
                btnClone.Visible = false;
            }
        }
        protected void btnClone_Click(object sender, ImageClickEventArgs e)
        {
            dashcomp = new DashboardComponent();
            try
            {
                String strMessgae = Convert.ToString(dashcomp.CloneDashboardSetting(Convert.ToInt32(ddlCloneStore.SelectedValue), Convert.ToInt32(ddlstore.SelectedValue)));
                if (strMessgae.Trim() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "strMessgaeSuccess", "jAlert('" + strMessgae.Trim() + "','Message')", true);
                    return;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "strMessgaeFail", "jAlert('Problem while cloning','Message')", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "strMessgaeCatchError", "jAlert('" + ex.Message.ToString() + "','Message')", true);
                return;
            }
            grdLeftControls.DataBind();
            grdCenterControls.DataBind();
            grdRightControls.DataBind();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            int LeftRowCount = grdLeftControls.Rows.Count;
            for (int i = 0; i < LeftRowCount; i++)
            {
                Label lblSettingID = (Label)grdLeftControls.Rows[i].FindControl("lblSettingID");
                CheckBox chkisdisplay = (CheckBox)grdLeftControls.Rows[i].FindControl("chkIsDisplay");
                CheckBox chkIsAdminAllow = (CheckBox)grdLeftControls.Rows[i].FindControl("chkIsAdminAllow");
                TextBox txtdisplayorder = (TextBox)grdLeftControls.Rows[i].FindControl("txtdisplaypos");
                tb_DashboardSettings tbldashboard = null;
                tbldashboard = dashcomp.GetDashboardSetting(Convert.ToInt16(lblSettingID.Text));
                tbldashboard.DisplayPosition = Convert.ToInt32(txtdisplayorder.Text);
                if (chkisdisplay.Checked == true)
                    tbldashboard.IsDisplay = true;
                else
                    tbldashboard.IsDisplay = false;
                dashcomp.UpdateDashboardSetting(tbldashboard, Convert.ToBoolean(chkIsAdminAllow.Checked), Convert.ToInt32(ddlAdmins.SelectedValue));
            }
            grdLeftControls.DataBind();

            int CenterRowCount = grdCenterControls.Rows.Count;
            for (int i = 0; i < CenterRowCount; i++)
            {
                Label lblSettingID = (Label)grdCenterControls.Rows[i].FindControl("lblSettingID");
                CheckBox chkisdisplay = (CheckBox)grdCenterControls.Rows[i].FindControl("chkIsDisplay");
                CheckBox chkIsAdminAllow = (CheckBox)grdCenterControls.Rows[i].FindControl("chkIsAdminAllow");
                TextBox txtdisplayorder = (TextBox)grdCenterControls.Rows[i].FindControl("txtdisplaypos");
                tb_DashboardSettings tbldashboard = null;
                tbldashboard = dashcomp.GetDashboardSetting(Convert.ToInt16(lblSettingID.Text));
                tbldashboard.DisplayPosition = Convert.ToInt32(txtdisplayorder.Text);
                if (chkisdisplay.Checked == true)
                    tbldashboard.IsDisplay = true;
                else
                    tbldashboard.IsDisplay = false;
                dashcomp.UpdateDashboardSetting(tbldashboard, Convert.ToBoolean(chkIsAdminAllow.Checked), Convert.ToInt32(ddlAdmins.SelectedValue));
            }
            grdCenterControls.DataBind();

            int RightRowCount = grdRightControls.Rows.Count;
            for (int i = 0; i < RightRowCount; i++)
            {
                Label lblSettingID = (Label)grdRightControls.Rows[i].FindControl("lblSettingID");
                CheckBox chkisdisplay = (CheckBox)grdRightControls.Rows[i].FindControl("chkIsDisplay");
                CheckBox chkIsAdminAllow = (CheckBox)grdRightControls.Rows[i].FindControl("chkIsAdminAllow");
                TextBox txtdisplayorder = (TextBox)grdRightControls.Rows[i].FindControl("txtdisplaypos");
                tb_DashboardSettings tbldashboard = null;
                tbldashboard = dashcomp.GetDashboardSetting(Convert.ToInt16(lblSettingID.Text));
                tbldashboard.DisplayPosition = Convert.ToInt32(txtdisplayorder.Text);
                if (chkisdisplay.Checked == true)
                    tbldashboard.IsDisplay = true;
                else
                    tbldashboard.IsDisplay = false;
                dashcomp.UpdateDashboardSetting(tbldashboard, Convert.ToBoolean(chkIsAdminAllow.Checked), Convert.ToInt32(ddlAdmins.SelectedValue));
            }
            grdRightControls.DataBind();

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Dashboard Configuration Saved Successfully.', 'Message'); Tabdisplay('" + hdnCurrentTab.Value.ToString() + "');});", true);
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/Dashboard.aspx");
        }

        /// <summary>
        /// Admin Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlAdmins_SelectedIndexChanged(object sender, EventArgs e)
        {
            grdLeftControls.DataBind();
            grdCenterControls.DataBind();
            grdRightControls.DataBind();
        }
    }
}