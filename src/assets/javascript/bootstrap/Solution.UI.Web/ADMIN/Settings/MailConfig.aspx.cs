using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using Solution.Bussines.Components.AdminCommon;
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace Solution.UI.Web.ADMIN.Settings
{

    public partial class OnePageMailConfig : BasePage
    {

        #region Declaration

        StoreComponent objStorecomponent = new StoreComponent();
        ConfigurationComponent ConfigurationC = new ConfigurationComponent();

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMsg.Text = "";

            if (!Page.IsPostBack)
            {
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancel.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                imgTestMail.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/Test-Account-Settings.png";

                bindstore();
                BindData(ddlStore.SelectedValue);
            }
        }

        /// <summary>
        ///  Function For Bind Data in Text Box
        /// </summary>
        /// <param name="StoreID">string StoreID</param>
        public void BindData(string StoreID)
        {
            DataSet DsAppConfig = ConfigurationC.GetMailConfig("", "", Convert.ToInt32(StoreID), DateTime.MaxValue, 1, 1);
            GetValue(DsAppConfig, StoreID);

        }

        /// <summary>
        /// GetValue For Getting value for particular  ConfigName
        /// </summary>
        /// <param name="Ds">Dataset Ds</param>
        /// <param name="StoreID">string StoreID</param>
        public void GetValue(DataSet Ds, string StoreID)
        {
            try
            {
                ClearData();
                if (Ds != null && Ds.Tables.Count > 0 && Ds.Tables[0].Rows.Count > 0)
                {
                    Hashtable ht = new Hashtable();
                    for (int cnt = 0; cnt < Ds.Tables[0].Rows.Count; cnt++)
                    {
                        try { ht.Add((string.IsNullOrEmpty(Ds.Tables[0].Rows[cnt]["StoreID"].ToString()) ? "1" : Ds.Tables[0].Rows[cnt]["StoreID"].ToString()) + Ds.Tables[0].Rows[cnt]["configname"].ToString(), Ds.Tables[0].Rows[cnt]["configvalue"].ToString()); }
                        catch { }
                    }

                    ViewState.Add("Hastable", ht);
                    txtContactMail_ToAddress.Text = (ht[StoreID + "ContactMail_ToAddress"] == null) ? "" : ht[StoreID + "ContactMail_ToAddress"].ToString();
                    txtHost.Text = (ht[StoreID + "Host"] == null) ? "" : ht[StoreID + "Host"].ToString();
                    txtMailFrom.Text = (ht[StoreID + "MailFrom"] == null) ? "" : ht[StoreID + "MailFrom"].ToString();
                    txtMailMe_ToAddress.Text = (ht[StoreID + "MailMe_ToAddress"] == null) ? "" : ht[StoreID + "MailMe_ToAddress"].ToString();
                    txtMailPassword.Text = (ht[StoreID + "MailPassword"] == null) ? "" : ht[StoreID + "MailPassword"].ToString();
                    txtMailPassword.Attributes.Add("value", txtMailPassword.Text);
                    txtMailUserName.Text = (ht[StoreID + "MailUserName"] == null) ? "" : ht[StoreID + "MailUserName"].ToString();
                    chkSendCustomerRegistrationMail.Checked = (ht[StoreID + "SendCustomerRegistrationMail"] == null) ? false : (ht[StoreID + "SendCustomerRegistrationMail"].ToString().ToLower() == "true") ? true : false;
                }
            }
            catch (Exception ex)
            {
                lblMsg.Text = ex.Message;
            }
        }

        /// <summary>
        /// Clear Data For Removing value from Text Box 
        /// </summary>
        private void ClearData()
        {
            txtContactMail_ToAddress.Text = "";
            txtHost.Text = "";
            txtMailFrom.Text = "";
            txtMailMe_ToAddress.Text = "";
            txtMailPassword.Text = "";
            txtMailUserName.Text = "";
            chkSendCustomerRegistrationMail.Checked = false;

        }

        /// <summary>
        /// Bind Store For Binding Store in Drop down list
        /// </summary>
        private void bindstore()
        {
            var storeDetail = objStorecomponent.GetStore();
            if (storeDetail.Count > 0 && storeDetail != null)
            {
                ddlStore.DataSource = storeDetail;
                ddlStore.DataTextField = "StoreName";
                ddlStore.DataValueField = "StoreID";
                ddlStore.DataBind();
                ddlStore.Items.Insert(0, new ListItem("Select Store", "0"));
            }
            else
            {
                ddlStore.Items.Clear();
                ddlStore.Items.Insert(0, new ListItem("Select Store", "0"));
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
        /// Save Button Click Event (Update Data On Save button Click)
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            lblMsg.Text = "";
            Hashtable hastable = new Hashtable();

            if (ViewState["Hastable"] != null)
            {
                hastable = (Hashtable)ViewState["Hastable"];
            }

            if (hastable != null && ddlStore.SelectedIndex != -1)
            {
                DateTime UpdatedOn = DateTime.Now;
                int UpdatedBy = int.Parse(Session["AdminID"].ToString());

                UpdateThis(hastable, "ContactMail_ToAddress", ddlStore.SelectedValue, txtContactMail_ToAddress.Text, UpdatedOn, UpdatedBy);
                UpdateThis(hastable, "Host", ddlStore.SelectedValue, txtHost.Text, UpdatedOn, UpdatedBy);
                UpdateThis(hastable, "MailFrom", ddlStore.SelectedValue, txtMailFrom.Text, UpdatedOn, UpdatedBy);
                UpdateThis(hastable, "MailMe_ToAddress", ddlStore.SelectedValue, txtMailMe_ToAddress.Text, UpdatedOn, UpdatedBy);
                UpdateThis(hastable, "MailPassword", ddlStore.SelectedValue, txtMailPassword.Text, UpdatedOn, UpdatedBy);
                UpdateThis(hastable, "MailUserName", ddlStore.SelectedValue, txtMailUserName.Text, UpdatedOn, UpdatedBy);
                UpdateThis(hastable, "SendCustomerRegistrationMail", ddlStore.SelectedValue, Convert.ToString(chkSendCustomerRegistrationMail.Checked), UpdatedOn, UpdatedBy);

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Mail Configuration Updated Successfully.', 'Message','');});", true);
            }
        }

        /// <summary>
        /// Function For Update Data
        /// </summary>
        /// <param name="hastable">HashTable hastable</param>
        /// <param name="controlname">string controlname</param>
        /// <param name="StoreID">string StoreID</param>
        /// <param name="txtValue">string txtValue</param>
        /// <param name="UpdatedOn">DateTime UpdatedOn</param>
        /// <param name="UpdatedBy">int UpdatedBy</param>
        private void UpdateThis(Hashtable hastable, string controlname, string StoreID, string txtValue, DateTime UpdatedOn, int UpdatedBy)
        {
            ConfigurationComponent.UpdateMailConfig(controlname, txtValue, Convert.ToInt32(StoreID), UpdatedOn, UpdatedBy, 2);
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

            if (ddlStore.SelectedIndex != -1)
                BindData(ddlStore.SelectedValue);

        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("/Admin/Dashboard.aspx");
        }

        /// <summary>
        ///  Test Mail Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgTestMail_Click(object sender, ImageClickEventArgs e)
        {
            bool istrue = SendTestMail();

            if (istrue)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Test Mail has been sent Successfully.', 'Message');});", true);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msgnotexists", "$(document).ready( function() {ShowForGotPassword();jAlert('Test Mail has not been sent ', 'Message','txtEmail');});", true);
                return;
            }
        }

        /// <summary>
        /// Sends the Test Mail
        /// </summary>
        /// <returns>Returns true if Sent, false otherwise</returns>
        private bool SendTestMail()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            bool IsSent = false;
            DataSet dsMailTemplate = new DataSet();
            dsMailTemplate = objCustomer.GetEmailTamplate("TestMailForMailconfig", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsMailTemplate != null && dsMailTemplate.Tables.Count > 0 && dsMailTemplate.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                String strSubject = "";
                strBody = dsMailTemplate.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsMailTemplate.Tables[0].Rows[0]["Subject"].ToString();

                strSubject = Regex.Replace(strSubject, "###STORENAME###", ddlStore.SelectedItem.Text.ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###StoreID###", AppLogic.AppConfigs("StoreID").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", ddlStore.SelectedItem.Text.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);

                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendTestMail(txtMailUserName.Text.ToString().Trim(), txtMailPassword.Text.ToString().Trim(), txtHost.Text.ToString().Trim(), txtMailFrom.Text.ToString().Trim(), txtMailMe_ToAddress.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
                IsSent = true;
            }

            return IsSent;
        }

    }
}