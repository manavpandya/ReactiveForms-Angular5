using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Configuration
{
    public partial class SubjectStatus : BasePage
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
                if (!string.IsNullOrEmpty(Request.QueryString["SubjectStatusID"]))
                {
                    FillSubjectStatus(Convert.ToInt32(Request.QueryString["SubjectStatusID"]));
                    lblTitle.Text = "Edit Subject Status";
                }
            }

            txtSubject.Focus();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtSubject.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter SubjctStatus name.', 'Message','');});", true);
                return;
            }
            else if (txtEmailID.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please Enter EmailID.', 'Message','');});", true);
                return;
            }


            SubjectStatusComponent counC = new SubjectStatusComponent();
            tb_ContactEmail tb_ContactEmail = null;



            if (!string.IsNullOrEmpty(Request.QueryString["SubjectStatusID"]) && Convert.ToString(Request.QueryString["SubjectStatusID"]) != "0")
            {
                tb_ContactEmail = counC.getSubjectStatus(Convert.ToInt32(Request.QueryString["SubjectStatusID"]));
                tb_ContactEmail.Subject = txtSubject.Text;
                tb_ContactEmail.EmailID = txtEmailID.Text;
                if(chkSelect.Checked)
                {
                    tb_ContactEmail.Active = true;
                }
                else
                {
                    tb_ContactEmail.Active = false;
                }
                

                Int32 isupdated = counC.UpdateSubjectStatus(tb_ContactEmail);
                if (isupdated > 0)
                {
                    Response.Redirect("SubjectStatusList.aspx?status=updated");
                }
                else if (isupdated == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Subject already exists.', 'Message','');});", true);
                    return;
                }
            }
            else
            {
                tb_ContactEmail = new tb_ContactEmail();
                tb_ContactEmail.Subject = txtSubject.Text;
                tb_ContactEmail.EmailID = txtEmailID.Text;
                if (chkSelect.Checked)
                {
                    tb_ContactEmail.Active = true;
                }
                else
                {
                    tb_ContactEmail.Active = false;
                }
                tb_ContactEmail.Deleted = false;
                

                Int32 isadded = counC.CreateSubjectStatus(tb_ContactEmail);

                if (isadded > 0)
                {
                    Response.Redirect("SubjectStatusList.aspx?status=inserted");
                }
                else if (isadded == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Subject already exists.', 'Message','');});", true);
                    return;
                }
            }

        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("SubjectStatusList.aspx");
        }

        /// <summary>
        /// Fill Country Data while Edit mode is Active 
        /// </summary>
        /// <param name="CountryID">int CountryID</param>
        private void FillSubjectStatus(Int32 CountryID)
        {
            SubjectStatusComponent LoadCountry = new SubjectStatusComponent();
            tb_ContactEmail tb_ContactEmail = LoadCountry.getSubjectStatus(CountryID);

            txtSubject.Text = tb_ContactEmail.Subject;
            txtEmailID.Text = tb_ContactEmail.EmailID;
            if(tb_ContactEmail.Active==true)
            {
                chkSelect.Checked = true;
            }
           

        }
    }
}