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
    public partial class EditState : Solution.UI.Web.BasePage
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
                FillCountry();
                if (!string.IsNullOrEmpty(Request.QueryString["StateID"]))
                {
                    FillState(Convert.ToInt32(Request.QueryString["StateID"]));
                    lblTitle.Text = "Edit State";
                }
            }
            txtStateName.Focus();
        }

        #region Fill Functions

        /// <summary>
        /// Fill State Data while Edit mode is Active 
        /// </summary>
        /// <param name="StateID">int StateID</param>
        private void FillState(Int32 StateID)
        {
            StateComponent stac = new StateComponent();
            tb_State tb_State = stac.getState(StateID);
            txtStateName.Text = tb_State.Name;
            txtabbreviation.Text = tb_State.Abbreviation;
            ddlCountry.SelectedValue = tb_State.CountryID.ToString();
            if (Convert.ToString(tb_State.DisplayOrder) == "999")
            {
                txtDisplayOrder.Text = "";
            }
            else
            {
                txtDisplayOrder.Text = Convert.ToString(tb_State.DisplayOrder);
            }

        }

        /// <summary>
        /// Fill Country Dropdown List
        /// </summary>
        private void FillCountry()
        {
            StateComponent statCountryList = new StateComponent();
            List<tb_Country> CountryList = statCountryList.GetAllCountry();

            if (CountryList.Count > 0)
            {
                ddlCountry.DataSource = CountryList;
                ddlCountry.DataTextField = "Name";
                ddlCountry.DataValueField = "CountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
            else
            {
                ddlCountry.Items.Clear();
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
        }

        #endregion

        #region Button Click Events

        /// <summary>
        /// Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("StateList.aspx");
        }

        /// <summary>
        /// Save button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtStateName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter state name.', 'Message','');});", true);
                txtStateName.Focus();
                return;
            }
            else if (ddlCountry.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select country.', 'Message','');});", true);
                ddlCountry.Focus();
                return;
            }
            else if (txtabbreviation.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter abbreviation.', 'Message','');});", true);
                txtabbreviation.Focus();
                return;
            }

            StateComponent counS = new StateComponent();
            tb_State tb_State = null;

            if (!string.IsNullOrEmpty(Request.QueryString["StateID"]) && Convert.ToString(Request.QueryString["StateID"]) != "0")
            {
                // code for Update Data into Database

                tb_State = counS.getState(Convert.ToInt32(Request.QueryString["StateID"]));
                tb_State.Name = txtStateName.Text;
                tb_State.Abbreviation = txtabbreviation.Text;
                tb_State.CountryID = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
                tb_State.Deleted = false;
                if (txtDisplayOrder.Text == "")
                {
                    tb_State.DisplayOrder = 999;
                }
                else
                {
                    tb_State.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                }
                tb_State.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                tb_State.UpdatedOn = DateTime.Now;

                Int32 isupdated = counS.UpdateState(tb_State);
                if (isupdated > 0)
                {
                    Response.Redirect("StateList.aspx?status=updated");
                }
                else if (isupdated == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('State already exists.', 'Message','');});", true);
                    return;
                }
            }
            else
            {
                // code for Inster Data into Database

                tb_State = new tb_State();
                tb_State.Name = txtStateName.Text;
                tb_State.Abbreviation = txtabbreviation.Text;
                tb_State.CountryID = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
                tb_State.Deleted = false;
                if (txtDisplayOrder.Text == "")
                {
                    tb_State.DisplayOrder = 999;
                }
                else
                {
                    tb_State.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                }
                tb_State.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tb_State.CreatedOn = DateTime.Now;

                Int32 isadded = counS.CreateState(tb_State); //Create State Line

                if (isadded > 0)
                {
                    Response.Redirect("StateList.aspx?status=inserted");
                }
                else if (isadded == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('State already exists.', 'Message','');});", true);
                    return;
                }
            }
        }

        #endregion
    }
}

