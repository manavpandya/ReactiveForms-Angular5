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


namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class AdminTemplateRights : BasePage
    {
        #region Declaration
        AdminRightsComponent objAdminRightComponent = null;
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Request.QueryString["Inserted"] != null && Request.QueryString["Inserted"].ToString() != "")
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "RightsInserted", "jAlert('Page Rights Saved Successfully.','Message');", true);
                }
                else if (Request.QueryString["Updated"] != null && Request.QueryString["Updated"].ToString() != "")
                { Page.ClientScript.RegisterStartupScript(Page.GetType(), "RightsUpdated", "jAlert('Page Rights Updated Successfully.','Message');", true); }
               
                tdtemplatelist.Visible = true;
                GetRightList();              
                GetTemplatename();
                GetPageRightList();
            }
          //  btnUpdateRights.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/update.png) no-repeat transparent; width: 69px; height: 23px; border:none;cursor:pointer;");
            btnUpdatePageRight.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/update.png) no-repeat transparent; width: 69px; height: 23px; border:none;cursor:pointer;");
            btnaddnew.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/add-admin-rights-template.png) no-repeat transparent; width: 200px; height: 23px; border:none;cursor:pointer;");
            
        
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
            DataSet dsPageRights = objAdminRightComponent.GetAdminTemplatePageRightList();
            if (dsPageRights != null && dsPageRights.Tables.Count > 0 && dsPageRights.Tables[0].Rows.Count > 0)
            {
                gvAdminPageRights.DataSource = dsPageRights;
            }
            else
            {
                gvAdminPageRights.DataSource = null;
            }
            gvAdminPageRights.DataBind();
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
            
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Admin Template Rights Updated Successfully.', 'Message','');});", true);
        }

        protected void btnUpdatePageRight_Click(object sender, EventArgs e)
        {
            AdminRightsComponent objAdmin = new AdminRightsComponent();
            if (txtTemplate.Text != null)
            {
                tb_admin_Templaterights AdminTemplate = new tb_admin_Templaterights();
                AdminTemplate.Name = txtTemplate.Text.Trim();
                if (objAdmin.CheckDuplicateTemplatename(AdminTemplate))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert(' Template with same Name already exists.', 'Message');});", true);
                    return;
                }
            }
            if (gvAdminPageRights.Rows.Count > 0)
            {
                String Rights = SetAdminRightForUpdate();

                for (int i = 0; i < gvAdminPageRights.Rows.Count; i++)
                {
                    Label lblID = (Label)gvAdminPageRights.Rows[i].FindControl("lblID");
                    // Label lblCompareAdminID = (Label)gvAdminPageRights.Rows[i].FindControl("lblCompareAdminID");
                    Label lblInnerRightsID = (Label)gvAdminPageRights.Rows[i].FindControl("lblInnerRightsID");
                    CheckBox chkIsListed = (CheckBox)gvAdminPageRights.Rows[i].FindControl("chkIsListed");
                  
                    Int32 IsAdded = objAdmin.Insert_Update_TemplatePageRightsForAdmin(Convert.ToInt32(lblID.Text), txtTemplate.Text, Convert.ToInt32(lblInnerRightsID.Text), Convert.ToBoolean(chkIsListed.Checked), Convert.ToInt32(Session["AdminID"]), Rights);
                }

              //  GetPageRightList();          
               // GetTemplatename();
              
                if (ddltemplatename.SelectedValue != "0")
                {
                    Response.Redirect("AdminTemplateRights.aspx?Updated=true");
                }
                else { Response.Redirect("AdminTemplateRights.aspx?Inserted=true"); }
            }


        }
        protected void btnaddnew_Click(object sender, EventArgs e)
        {
            if (hdnaddnew.Value == "0")
            {
                tdtemplatelist.Visible = false;
                tdAddtemplatelist.Visible = true;
                btnUpdatePageRight.Attributes.Add("style", "background: url(/App_Themes/" + Page.Theme.ToString() + "/images/save.gif) no-repeat transparent; width: 69px; height: 23px; border:none;cursor:pointer;");
                btnUpdatePageRight.ToolTip = "Save";
                GetTemplatename();               
                GetPageRightList();
                CleartabRights();
                hdnaddnew.Value = "1";
                btnaddnew.Visible = false;
            }
            else
            {
                tdtemplatelist.Visible = true;
                tdAddtemplatelist.Visible = false;
                hdnaddnew.Value = "0";
                btnaddnew.Visible = true;
            }
        }

        protected void gvAdminPageRights_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAdminPageRights.PageIndex = e.NewPageIndex;
            GetPageRightList();
        }
        protected void ddltemplatename_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddltemplatename.SelectedValue != "0")
            {
                GetTemplateByName();
            }
            else {
                CleartabRights();
                GetPageRightList();            
            }
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
                    gvAdminPageRights.DataSource = dsadminRights.Tables[1];
                }
                else
                {
                    gvAdminPageRights.DataSource = null;
                }
                gvAdminPageRights.DataBind();

            
           
        }
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
        private void CleartabRights()
        {
            for (int cntchk = 0; cntchk < chklrights.Items.Count; cntchk++)
                chklrights.Items[cntchk].Selected = false;
        }
    }
}