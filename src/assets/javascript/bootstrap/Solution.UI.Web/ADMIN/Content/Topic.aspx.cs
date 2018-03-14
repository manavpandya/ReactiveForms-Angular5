using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Entities;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;

namespace Solution.UI.Web.ADMIN.Content
{
    public partial class Topic : Solution.UI.Web.BasePage
    {
        #region Variable declaration
        tb_Topic topic = null;
        TopicComponent objTopicComp = null;
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
                btnCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                bindstore();
                if (!string.IsNullOrEmpty(Request.QueryString["TopicID"]) && Convert.ToString(Request.QueryString["TopicID"]) != "0")
                {
                    topic = new tb_Topic();
                    objTopicComp = new TopicComponent();
                    //Display selected Topic detail for edit mode
                    topic = objTopicComp.getTopicByID(Convert.ToInt32(Request.QueryString["TopicID"]));
                    lblHeader.Text = "Edit Static Website Page";
                    drpStoreName.Enabled = false;
                    txtTitle.Text = topic.Title;
                    txtTopicName.Text = topic.TopicName;
                    ckeditordescription.Text = topic.Description;
                    txtSEDescription.Text = topic.SEDescription;
                    txtSEKeywords.Text = topic.SEKeywords;
                    txtSETitle.Text = topic.SETitle;
                    if (!string.IsNullOrEmpty(topic.ShowOnSiteMap.ToString()))
                        chkShowOnSiteMap.Checked = Convert.ToBoolean(topic.ShowOnSiteMap);
                    int StoreId = topic.tb_StoreReference.Value.StoreID;
                    drpStoreName.SelectedValue = StoreId.ToString();
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
            topic = new tb_Topic();
            objTopicComp = new TopicComponent();
            if (!string.IsNullOrEmpty(Request.QueryString["TopicID"]) && Convert.ToString(Request.QueryString["TopicID"]) != "0")
            {
                topic = objTopicComp.getTopicByID(Convert.ToInt32(Request.QueryString["TopicID"]));
                topic.TopicName = txtTopicName.Text.Trim();
                topic.Title = txtTitle.Text.Trim();
                topic.TopicName = txtTopicName.Text.Trim();
                topic.Description = ckeditordescription.Text.Trim();
                topic.SEDescription = txtSEDescription.Text.Trim();
                topic.SEKeywords = txtSEKeywords.Text.Trim();
                topic.SETitle = txtSETitle.Text.Trim();
                string sename = CommonOperations.RemoveSpecialCharacter(txtTitle.Text.Trim().ToCharArray());
                topic.SEName = sename;
                topic.UpdatedBy = Convert.ToInt32(Session["AdminID"].ToString());
                topic.UpdatedOn = System.DateTime.Now;
                topic.ShowOnSiteMap = chkShowOnSiteMap.Checked;
                if (objTopicComp.CheckDuplicate(topic))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Static Website Page Name already exists.', 'Message');});", true);
                    return;
                }

                Int32 isupdated = objTopicComp.Update(topic);
                if (txtTopicName.Text.ToString().ToLower() == "checkoutpolicy")
                {
                    CommonComponent.ExecuteCommonData("update tb_appconfig set configvalue='" + ckeditordescription.Text.Trim().Replace("'", "''") + "' where configname='checkoutploicy' and storeid=1");
                }
                if (isupdated > 0)
                {
                    Response.Redirect("tOPICList.aspx?status=updated");
                }
            }
            else
            {
                topic.TopicName = txtTopicName.Text.Trim();
                int StoreId1 = Convert.ToInt32(drpStoreName.SelectedItem.Value.ToString());
                topic.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", StoreId1);
                //Check Topic name already exists or not
                if (objTopicComp.CheckDuplicate(topic))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Static Website Page Name already exists.', 'Message');});", true);
                    return;
                }

                topic.Title = txtTitle.Text.Trim();
                topic.TopicName = txtTopicName.Text.Trim();
                topic.Description = ckeditordescription.Text.Trim();
                topic.SEDescription = txtSEDescription.Text.Trim();
                topic.SEKeywords = txtSEKeywords.Text.Trim();
                topic.SETitle = txtSETitle.Text.Trim();
                string sename = CommonOperations.RemoveSpecialCharacter(txtTitle.Text.Trim().ToCharArray());
                topic.SEName = sename;
                topic.CreatedBy = Convert.ToInt32(Session["AdminID"].ToString());
                topic.CreatedOn = System.DateTime.Now;
                topic.ShowOnSiteMap = chkShowOnSiteMap.Checked;
                topic.Deleted = false;
                Int32 isadded = objTopicComp.CreateTopic(topic);

                if (isadded > 0)
                {
                    Response.Redirect("TopicList.aspx?status=inserted");
                }
            }
        }

        /// <summary>
        /// Bind Store Details with dropdown
        /// </summary>
        private void bindstore()
        {
            StoreComponent objStorecomponent = new StoreComponent();
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                drpStoreName.DataSource = storeDetail;
                drpStoreName.DataTextField = "StoreName";
                drpStoreName.DataValueField = "StoreID";
                drpStoreName.DataBind();
            }

        }

        /// <summary>
        /// Cancel button click event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("TopicList.aspx");
        }



    }
}