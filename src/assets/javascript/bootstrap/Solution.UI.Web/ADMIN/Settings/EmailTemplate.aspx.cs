using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Settings
{
    public partial class EmailTemplate : BasePage
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
                btnSaveTemplate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                btnCancelTemplate.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                bindstore();
                if (!string.IsNullOrEmpty(Request.QueryString["EmailTemplateID"]) && Convert.ToString(Request.QueryString["EmailTemplateID"]) != "0")
                {
                    tb_EmailTemplate MailTemplate = new tb_EmailTemplate();
                    EmailTemplateComponent objEmailTempComponent = new EmailTemplateComponent();
                    MailTemplate = objEmailTempComponent.GetEmailTemplateByID(Convert.ToInt32(Request.QueryString["EmailTemplateID"]));
                    txtLabel.Text = MailTemplate.Label;
                    txtSubject.Text = MailTemplate.Subject;
                    txtMailBody.Text = MailTemplate.EmailBody;
                    txtcc.Text = MailTemplate.EmailCC;
                    txtto.Text =  MailTemplate.EmailTo;
                    if (!string.IsNullOrEmpty(MailTemplate.IsPOTemplate.ToString()) && Convert.ToBoolean(MailTemplate.IsPOTemplate))
                    {
                        chkIsPO.Checked = true;
                    }
                    else { chkIsPO.Checked = false; }
                    lblHeader.Text = "Edit Email Template";
                    ddlStore.Enabled = false;
                    int StoreId = MailTemplate.tb_StoreReference.Value.StoreID;
                    ddlStore.SelectedValue = StoreId.ToString();
                }
            }
        }

        /// <summary>
        ///  Save Template Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSaveTemplate_Click(object sender, ImageClickEventArgs e)
        {
            int TemplateID = 0;
            tb_EmailTemplate MailTemplate = new tb_EmailTemplate();
            EmailTemplateComponent objEmailTempComponent = new EmailTemplateComponent();

            if (txtMailBody.Text.Trim() == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter Email Body.', 'Message','');});", true);
                return;
            }
            int StoreId1 = Convert.ToInt32(ddlStore.SelectedItem.Value.ToString());
            MailTemplate.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
            if (!string.IsNullOrEmpty(Request.QueryString["EmailTemplateID"]) && Convert.ToString(Request.QueryString["EmailTemplateID"]) != "0")
            {
                MailTemplate = objEmailTempComponent.GetEmailTemplateByID(Convert.ToInt32(Request.QueryString["EmailTemplateID"]));
                MailTemplate.Label = txtLabel.Text.Trim();
                if (objEmailTempComponent.CheckDuplicate(MailTemplate))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Email Template with same Label already exists.', 'Message');});", true);
                    return;
                }
                MailTemplate.EmailTo = txtto.Text.ToString();
                MailTemplate.EmailCC = txtcc.Text.ToString();
                MailTemplate.Subject = txtSubject.Text.Trim();
                MailTemplate.EmailBody = txtMailBody.Text.Trim();
                MailTemplate.IsPOTemplate = chkIsPO.Checked;
                TemplateID = objEmailTempComponent.UpdateEmailTemplate(MailTemplate);
                if (TemplateID > 0)
                    Response.Redirect("EmailTemplateList.aspx?status=updated");
            }
            else
            {
                MailTemplate.Label = txtLabel.Text.Trim();
                if (objEmailTempComponent.CheckDuplicate(MailTemplate))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Email Template with same Label already exists.', 'Message');});", true);
                    return;
                }
                MailTemplate.EmailTo = txtto.Text.ToString();
                MailTemplate.EmailCC = txtcc.Text.ToString();
                MailTemplate.Subject = txtSubject.Text.Trim();
                MailTemplate.EmailBody = txtMailBody.Text.Trim();
                MailTemplate.IsPOTemplate = chkIsPO.Checked;
                TemplateID = objEmailTempComponent.CreateEmailTemplate(MailTemplate);
                 
                if (TemplateID > 0)
                    Response.Redirect("EmailTemplateList.aspx?status=inserted");
            }
        }

        /// <summary>
        ///  Cancel Template Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancelTemplate_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("EmailTemplateList.aspx");
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
    }
}