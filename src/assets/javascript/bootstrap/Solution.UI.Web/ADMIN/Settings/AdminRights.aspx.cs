using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using Solution.Data;
using System.Data;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class AdminRights : BasePage
    {
        #region Declaration
        AdminRightsComponent objAdminRightComponent = null;
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
                bindstore();
                GetTemplatename();

                GetRightList();
                GetAdminList();
                GetPageRightList();
               
            }
            btnUpdateRights.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/update.png) no-repeat transparent; width: 69px; height: 23px; border:none;cursor:pointer;");
            btnUpdatePageRight.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/update.png) no-repeat transparent; width: 69px; height: 23px; border:none;cursor:pointer;");
        }

        /// <summary>
        /// Show Admin Rights List
        /// </summary>
        private void GetRightList()
        {
            objAdminRightComponent = new AdminRightsComponent();
            List<tb_AdminRights> lstRights = objAdminRightComponent.GetRightList();
            if (lstRights != null && lstRights.Count() > 0)
            {
                chklrights.Items.Clear();
                chklrights.DataSource = lstRights;
                chklrights.DataTextField = "Name";
                chklrights.DataValueField = "RightsID";
                chklrights.DataBind();
            }
        }

        /// <summary>
        /// Show Admin Rights List
        /// </summary>
        private void GetPageRightList()
        {
            objAdminRightComponent = new AdminRightsComponent();
            DataSet dsPageRights = objAdminRightComponent.GetAdminAssignPageRightList(Convert.ToInt32(ddlAdmins.SelectedValue.ToString()));
            if (dsPageRights != null && dsPageRights.Tables.Count > 0 && dsPageRights.Tables[0].Rows.Count > 0)
            {
                gvAdminPageRights.DataSource = dsPageRights;
            }
            else
            {
                gvAdminPageRights.DataSource = null;
            }
            gvAdminPageRights.DataBind();

          //  String StoreId =Convert.ToString(CommonComponent.GetScalarCommonData("select Top 1 IsNull(StoreID,-1) From tb_AdminInnerRights where AdminId=" + ddlAdmins.SelectedValue.ToString()));
          // if(StoreId !=null && StoreId != "")
           // ddlStore.SelectedValue = StoreId;
          // else
            //   ddlStore.SelectedValue = "-1";
        }

        /// <summary>
        /// Bind AdminList to Admin dropdown
        /// </summary>
        private void GetAdminList()
        {
            objAdminRightComponent = new AdminRightsComponent();
            ddlAdmins.DataSource = objAdminRightComponent.GetAdminList(0);
            ddlAdmins.DataTextField = "FirstName";
            ddlAdmins.DataValueField = "AdminID";
            ddlAdmins.DataBind();
            ddlAdmins.SelectedIndex = 0;
            BindRightWithAdmin(Convert.ToInt32(ddlAdmins.SelectedItem.Value));
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetTemplatename()
        {
            AdminRightsComponent objAdminRightComponent = new AdminRightsComponent();
            DataSet ds = objAdminRightComponent.GetAdminTemplatePageName();
            if (ds != null && ds.Tables[0].Rows.Count > 0)
            {
                ddltemplatename.DataSource = ds;
                ddltemplatename.DataTextField = "NAME";
                ddltemplatename.DataBind();
                ddltemplatename.Items.Insert(0, new ListItem("Select Template ", "0"));
            }
            else
            {
                ddltemplatename.DataSource = "";
                ddltemplatename.DataBind();
            }
        }
        /// <summary>
        /// Bind RightsCheckList based on Selected Admin
        /// </summary>
        /// <param name="AdminID">int AdminID</param>
        /// 
        private void BindRightWithAdmin(int AdminID)
        {
            objAdminRightComponent = new AdminRightsComponent();
            //set default false to Rights
            for (int cntchk = 0; cntchk < chklrights.Items.Count; cntchk++)
                chklrights.Items[cntchk].Selected = false;
            //Get AdminRights for selected Admin
            List<AdminEntity> admin = objAdminRightComponent.GetAdminList(AdminID);
            if (admin != null && admin[0].Rights != null && admin.Count() > 0)
            {
                string[] Rights = admin[0].Rights.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int cnt = 0; cnt < Rights.Length; cnt++)
                {
                    for (int cntchk = 0; cntchk < chklrights.Items.Count; cntchk++)
                    {
                        if (chklrights.Items[cntchk].Value == Rights[cnt].ToString())
                            chklrights.Items[cntchk].Selected = true;
                    }
                }
                if (admin[0].Templatename != null)
                    ddltemplatename.SelectedValue = admin[0].Templatename;
                else
                    ddltemplatename.SelectedValue = "0";

            }
            else { ddltemplatename.SelectedValue = "0"; }
        }

        /// <summary>
        /// Admin Drop down Selection Change Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlAdmins_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindRightWithAdmin(Convert.ToInt32(ddlAdmins.SelectedItem.Value));
            GetPageRightList();
        }

        protected void ddltemplatename_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltemplatename.SelectedValue != "0")
            {
                GetTemplateByName();
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "checkAllfalse();", true);
            }
            //else
            //{
            //    BindRightWithAdmin(Convert.ToInt32(ddlAdmins.SelectedItem.Value));
            //  //  CleartabRights();
            //    GetPageRightList();

            //}
        }
        private void GetTemplateByName()
        {
            objAdminRightComponent = new AdminRightsComponent();
            //set default false to Rights
            for (int cntchk = 0; cntchk < chklrights.Items.Count; cntchk++)
                chklrights.Items[cntchk].Selected = false;
            //Get AdminRights for selected Admin

            DataSet dsadminRights = objAdminRightComponent.GetAdminTemplatePageDetailByName(ddltemplatename.SelectedItem.Text.ToString());
            if (dsadminRights != null && dsadminRights.Tables[0].Rows.Count > 0)
            {
                string rights = dsadminRights.Tables[0].Rows[0]["Rights"].ToString();
                string[] Rights = rights.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                for (int cnt = 0; cnt < Rights.Length; cnt++)
                {
                    for (int cntchk = 0; cntchk < chklrights.Items.Count; cntchk++)
                    {
                        if (chklrights.Items[cntchk].Value == Rights[cnt].ToString())
                            chklrights.Items[cntchk].Selected = true;
                    }
                }
            }
            if (dsadminRights != null && dsadminRights.Tables.Count > 0 && dsadminRights.Tables[1].Rows.Count > 0)
            {
                dsadminRights.Tables[1].Columns.Add(new DataColumn("AdminID"));
                for (int i = 0; i <= dsadminRights.Tables[1].Rows.Count - 1; i++)
                {
                    dsadminRights.Tables[1].Rows[i]["AdminID"] = ddlAdmins.SelectedValue;
                }
                gvAdminPageRights.DataSource = dsadminRights.Tables[1];
            }
            else
            {
                gvAdminPageRights.DataSource = null;
            }
            gvAdminPageRights.DataBind();



        }

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
                ddlStore.SelectedIndex = -1;
              

            }

            if (!string.IsNullOrEmpty(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString()))
            {
                ddlStore.SelectedValue = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["StoreID"].ToString());
                AppConfig.StoreID = Convert.ToInt32(ddlStore.SelectedValue.ToString());
            }
            else
                AppConfig.StoreID = 1;

            ddlStore.Items.Insert(0, new ListItem("Select Store", "-1"));
            ddlStore.Items.Insert(0, new ListItem("All Store", "0"));
           
          
        }

        /// <summary>
        /// Set Rights for Admin
        /// </summary>
        /// <returns>Returns string of RightIDs</returns>
        private string SetAdminRightForUpdate()
        {
            string rights = string.Empty;
            for (int cnt = 0; cnt < chklrights.Items.Count; cnt++)
            {
                if (chklrights.Items[cnt].Selected)
                    rights += chklrights.Items[cnt].Value.ToString().Trim() + ",";
            }
            if (rights.Length > 1 && rights.Contains(","))
                rights = rights.Substring(0, rights.Length - 1);
            return rights;
        }

        /// <summary>
        /// Update button click event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnUpdateRights_Click(object sender, EventArgs e)
        {
         
          //  Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Admin Rights Updated Successfully.', 'Message','');});", true);
        }

        protected void btnUpdatePageRight_Click(object sender, EventArgs e)
        {

            tb_Admin Admin = new tb_Admin();
            objAdminRightComponent = new AdminRightsComponent();
            Admin = objAdminRightComponent.GetAdminByID(Convert.ToInt32(ddlAdmins.SelectedItem.Value));
            Admin.Rights = SetAdminRightForUpdate();

            if (ddltemplatename.SelectedValue != "0")
            {
                Admin.TemplateName = ddltemplatename.SelectedValue;
            }
            int rowsAffected = objAdminRightComponent.UpdateAdminRights(Admin);
          
         
             AdminRightsComponent objAdmin = new AdminRightsComponent();
            if (gvAdminPageRights.Rows.Count > 0)
            {
                for (int i = 0; i < gvAdminPageRights.Rows.Count; i++)
                {
                    Label lblID = (Label)gvAdminPageRights.Rows[i].FindControl("lblID");
                    Label lblCompareAdminID = (Label)gvAdminPageRights.Rows[i].FindControl("lblCompareAdminID");
                    Label lblInnerRightsID = (Label)gvAdminPageRights.Rows[i].FindControl("lblInnerRightsID");
                    CheckBox chkIsListed = (CheckBox)gvAdminPageRights.Rows[i].FindControl("chkIsListed");
                    int StoreID = Convert.ToInt32(ddlStore.SelectedValue);
                    Int32 IsAdded = objAdmin.Insert_Update_AssignPageRightsForAdmin(Convert.ToInt32(ddlAdmins.SelectedValue.ToString()), Convert.ToInt32(lblCompareAdminID.Text), Convert.ToInt32(lblID.Text), Convert.ToInt32(lblInnerRightsID.Text), Convert.ToBoolean(chkIsListed.Checked), Convert.ToInt32(Session["AdminID"]), StoreID);
                }

             //   if(ddltemplatename.SelectedValue !="0")
               // CommonComponent.ExecuteCommonData("update tb_admin set TemplateName='" + ddltemplatename.SelectedValue.ToString() +"'  where AdminID="+ ddlAdmins.SelectedValue.ToString());
                //GetAdminList();
                GetPageRightList();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "RightsInserted", "jAlert('Page Rights Saved Successfully.','Message');", true);
            }
        }

        protected void gvAdminPageRights_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdminPageRights.PageIndex = e.NewPageIndex;
            GetPageRightList();
        }

    }
}