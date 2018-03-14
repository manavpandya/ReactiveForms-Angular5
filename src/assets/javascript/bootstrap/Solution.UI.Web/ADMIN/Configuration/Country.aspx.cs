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
    /// <summary>
    /// Country Insert form Contains Country related Code for Insert and Edit Country     
    /// <author>
    /// Kaushalam Team © Kaushalam Inc. 2012.
    /// </author>
    /// Version 1.0
    /// </summary>

    public partial class EditCountry : BasePage
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
                if (!string.IsNullOrEmpty(Request.QueryString["CountryID"]))
                {
                    FillCountry(Convert.ToInt32(Request.QueryString["CountryID"]));
                    lblTitle.Text = "Edit Country";
                }
            }

            txtCountryName.Focus();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">ImageClickEventArgs e</param>
        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtCountryName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter country name.', 'Message','');});", true);
                return;
            }
            else if (txttwoISOCode.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please two letter ISO code.', 'Message','');});", true);
                return;
            }


            CountryComponent counC = new CountryComponent();
            tb_Country tb_Country = null;



            if (!string.IsNullOrEmpty(Request.QueryString["CountryID"]) && Convert.ToString(Request.QueryString["CountryID"]) != "0")
            {
                tb_Country = counC.getCountry(Convert.ToInt32(Request.QueryString["CountryID"]));
                tb_Country.Name = txtCountryName.Text;
                tb_Country.TwoLetterISOCode = txttwoISOCode.Text;
                tb_Country.ThreeLetterISOCode = txtthreeISOCode.Text;
                tb_Country.NumericISOCode = txtNumISOCode.Text;
                tb_Country.Deleted = false;
                if (txtDisplayOrder.Text == "")
                {
                    tb_Country.DisplayOrder = 999;
                }
                else
                {
                    tb_Country.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                }
                tb_Country.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                tb_Country.UpdatedOn = DateTime.Now;

                Int32 isupdated = counC.UpdateCountry(tb_Country);
                if (isupdated > 0)
                {
                    Response.Redirect("CountryList.aspx?status=updated");
                }
                else if (isupdated == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Country already exists.', 'Message','');});", true);
                    return;
                }
            }
            else
            {
                tb_Country = new tb_Country();
                tb_Country.Name = txtCountryName.Text;
                tb_Country.TwoLetterISOCode = txttwoISOCode.Text;
                tb_Country.ThreeLetterISOCode = txtthreeISOCode.Text;
                tb_Country.NumericISOCode = txtNumISOCode.Text;
                tb_Country.Deleted = false;
                if (txtDisplayOrder.Text == "")
                {
                    tb_Country.DisplayOrder = 999;
                }
                else
                {
                    tb_Country.DisplayOrder = Convert.ToInt32(txtDisplayOrder.Text);
                }
                tb_Country.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                tb_Country.CreatedOn = DateTime.Now;


                Int32 isadded = counC.CreateCountry(tb_Country);

                if (isadded > 0)
                {
                    Response.Redirect("CountryList.aspx?status=inserted");
                }
                else if (isadded == -1)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Country already exists.', 'Message','');});", true);
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
            Response.Redirect("CountryList.aspx");
        }

        /// <summary>
        /// Fill Country Data while Edit mode is Active 
        /// </summary>
        /// <param name="CountryID">int CountryID</param>
        private void FillCountry(Int32 CountryID)
        {
            CountryComponent LoadCountry = new CountryComponent();
            tb_Country tb_Country = LoadCountry.getCountry(CountryID);
            txtCountryName.Text = tb_Country.Name;
            txttwoISOCode.Text = tb_Country.TwoLetterISOCode;
            txtthreeISOCode.Text = tb_Country.ThreeLetterISOCode;
            txtNumISOCode.Text = tb_Country.NumericISOCode;
            if (Convert.ToString(tb_Country.DisplayOrder) == "999")
            {
                txtDisplayOrder.Text = "";
            }
            else
            {
                txtDisplayOrder.Text = Convert.ToString(tb_Country.DisplayOrder);
            }

        }
    }
}