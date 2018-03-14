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
    public partial class Admin : System.Web.UI.Page
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
                imgSave.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme.ToString() + "/images/cancel.gif";
                FillFabricVendor();
                if (!string.IsNullOrEmpty(Request.QueryString["AdminID"]))
                {
                   
                    FillAdmin(Convert.ToInt32(Request.QueryString["AdminID"]));
                    lblTitle.Text = "Edit Admin";
                }
                else
                {
                    chkIsSalesManager.Checked = true;
                }
            }

            txtFName.Focus();
        }

        /// <summary>
        /// Fill State Data while Edit mode is Active 
        /// </summary>
        /// <param name="AdminID">int AdminID</param>
        private void FillAdmin(Int32 AdminID)
        {
            AdminComponent admincom = new AdminComponent();
            tb_Admin tbAdmin = admincom.getAdmin(AdminID);
            txtFName.Text = tbAdmin.FirstName;
            txtLName.Text = tbAdmin.LastName;
            txtEmail.Text = tbAdmin.EmailID;
            txtPassword.Text = SecurityComponent.Decrypt(tbAdmin.Password);
            txtPassword.Attributes.Add("value", txtPassword.Text);
            txtRetrypassword.Attributes.Add("value", txtPassword.Text);
            chkStatus.Checked = Convert.ToBoolean(tbAdmin.Active);
            if (!string.IsNullOrEmpty(tbAdmin.VendorId.ToString()))
            {
                ddlFabricvendor.SelectedValue = Convert.ToString(tbAdmin.VendorId.ToString());
            }
            else
            {
                ddlFabricvendor.SelectedValue = "0";
            }
            chkIsSalesManager.Checked = Convert.ToBoolean(tbAdmin.isSalesManager);

            if (tbAdmin.AdminType.ToString() == "2")
            {
                chkVendor.Checked = true;
            }
            else
            { 
                chkVendor.Checked = false;
            }
        }
        private void FillFabricVendor()
        {
            VendorDAC objVendorDAC = new VendorDAC();
            DataSet dsdropshipsku = objVendorDAC.GetVendorList(0);
            ddlFabricvendor.Items.Clear();
            if (dsdropshipsku != null && dsdropshipsku.Tables.Count > 0 && dsdropshipsku.Tables[0].Rows.Count > 0)
            {

                ddlFabricvendor.DataSource = dsdropshipsku.Tables[0];
                ddlFabricvendor.DataTextField = "Name";
                ddlFabricvendor.DataValueField = "VendorID";
                ddlFabricvendor.DataBind();
            }
            else
            {
                ddlFabricvendor.DataSource = null;
                ddlFabricvendor.DataBind();
            }
            ddlFabricvendor.Items.Insert(0, new ListItem("Select Fabric Vendor", "0"));
        }

        /// <summary>
        /// Fill Password TextBox Back while PostBack Event Occurs
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtPassword_PreRender(object sender, EventArgs e)
        {
            txtPassword.Attributes["value"] = txtPassword.Text.ToString();
        }

        /// <summary>
        /// Fill Retype Password TextBox Back while PostBack Event Occurs
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtRetrypassword_PreRender(object sender, EventArgs e)
        {
            txtRetrypassword.Attributes["value"] = txtRetrypassword.Text.ToString();
        }

        /// <summary>
        /// Cancel Button Click Event 
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("AdminList.aspx");
        }

        /// <summary>
        /// Save button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtFName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter first name.', 'Message','');});", true);
                txtFName.Focus();
                return;
            }
            else if (txtLName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter last name.', 'Message','');});", true);
                txtLName.Focus();
                return;
            }
            else if (txtEmail.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter email address.', 'Message','');});", true);
                txtEmail.Focus();
                return;
            }
            if (!new System.Text.RegularExpressions.Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*").Match(txtEmail.Text.ToString().Trim()).Success)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter valid email address.', 'Message','');});", true);
                txtEmail.Focus();
                return;
            }
            else if (txtPassword.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter password.', 'Message','');});", true);
                txtPassword.Focus();
                return;
            }
            else if (txtPassword.Text.Length < 6)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Password length must be 6 characters.', 'Message','');});", true);
                txtPassword.Focus();
                return;
            }
            else if (txtRetrypassword.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter retype password.', 'Message','');});", true);
                txtPassword.Focus();
                return;
            }
            else if (txtPassword.Text != txtRetrypassword.Text)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Retype password must match with password.', 'Message','');});", true);
                txtPassword.Focus();
                return;
            }

            AdminComponent objAdmin = new AdminComponent();
            tb_Admin tb_Admin = null;

            if (!string.IsNullOrEmpty(Request.QueryString["AdminID"]) && Convert.ToString(Request.QueryString["AdminID"]) != "0")
            {
                // code for Update Data into Database

                tb_Admin = objAdmin.getAdmin(Convert.ToInt32(Request.QueryString["AdminID"]));
                tb_Admin.FirstName = txtFName.Text;
                tb_Admin.LastName = txtLName.Text;
                tb_Admin.EmailID = txtEmail.Text;
                tb_Admin.Password = SecurityComponent.Encrypt(txtPassword.Text);
                tb_Admin.Deleted = false;
                tb_Admin.Active = Convert.ToBoolean(chkStatus.Checked);
                tb_Admin.isSalesManager = Convert.ToBoolean(chkIsSalesManager.Checked);
                tb_Admin.VendorId = Convert.ToInt32(ddlFabricvendor.SelectedValue.ToString());
                if (chkVendor.Checked == true)
                {
                    tb_Admin.AdminType = 2;
                }
                else
                {
                    tb_Admin.AdminType = 1;
                }
                Int32 isupdated = objAdmin.UpdateAdmin(tb_Admin);
                if (isupdated > 0)
                {
 
                    Response.Redirect("AdminList.aspx?status=updated");
                }
                else if (isupdated == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Admin already exists.', 'Message','');});", true);
                    return;
                }
            }
            else
            {
                // code for Inster Data into Database 

                tb_Admin = new tb_Admin();
                tb_Admin.FirstName = txtFName.Text;
                tb_Admin.LastName = txtLName.Text;
                tb_Admin.EmailID = txtEmail.Text;
                tb_Admin.Password = SecurityComponent.Encrypt(txtPassword.Text);
                tb_Admin.Deleted = false;
                tb_Admin.Active = Convert.ToBoolean(chkStatus.Checked);
                tb_Admin.isSalesManager = Convert.ToBoolean(chkIsSalesManager.Checked);
                tb_Admin.VendorId = Convert.ToInt32(ddlFabricvendor.SelectedValue.ToString());
                //tb_Admin.AdminType = 1;
                if (chkVendor.Checked == true)
                {
                    tb_Admin.AdminType = 2;
                }
                else
                {
                    tb_Admin.AdminType = 1;
                }

                Int32 isadded = objAdmin.CreateAdmin(tb_Admin);

                if (isadded > 0)
                {
                    Response.Redirect("AdminList.aspx?status=inserted");
                }
                else if (isadded == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Admin already exists.', 'Message','');});", true);
                    return;
                }
            }
        }
    }
}