using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class Vendor : Solution.UI.Web.BasePage
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
                btnSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                btnCancle.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                FillCountry();
                GetEmailTemplateList();
                if (!string.IsNullOrEmpty(Request.QueryString["VendorID"]) && Convert.ToString(Request.QueryString["VendorID"]) != "0")
                {
                    lblHeader.Text = "Edit Vendor/DropShipper";
                    tb_Vendor vendor = new tb_Vendor();
                    VendorComponent objVendorComponent = new VendorComponent();
                    vendor = objVendorComponent.GetVendorByID(Convert.ToInt32(Request.QueryString["VendorID"]));
                    txtVendorName.Text = vendor.Name;
                    txtEmail.Text = vendor.Email;
                    txtAddress.Text = vendor.Address;

                    if (ddlCountry.Items.FindByValue(vendor.Country.ToString()) != null)
                    {
                        ddlCountry.SelectedIndex = -1;
                        ddlCountry.Items.FindByValue(vendor.Country.ToString()).Selected = true;
                        ddlCountry_SelectedIndexChanged(null, null);
                    }
                    if (!string.IsNullOrEmpty(vendor.State))
                    {
                        if (ddlState.Items.FindByText(vendor.State.ToString()) == null)
                        {
                            txtother.Text = vendor.State;
                            if (ddlState.Items.FindByText("Others") != null)
                            {
                                ddlState.Items.FindByText("Others").Selected = true;
                                ClientScript.RegisterStartupScript(typeof(Page), "Billing", "MakeBillingOtherVisible();", true);
                            }
                        }

                        else
                        {
                            if (ddlState.Items.FindByText(vendor.State.ToString()) != null)
                            {
                                ddlState.SelectedIndex = -1;
                                ddlState.Items.FindByText(vendor.State.ToString()).Selected = true;
                            }
                        }
                    }
                    txtCity.Text = vendor.City;
                    txtZipcode.Text = vendor.Zipcode;
                    txtPhone.Text = vendor.Phone;
                    chkStatus.Checked = vendor.Active.Value;
                    chkIsDropShipper.Checked = vendor.IsDropshipper.Value;
                    ddlEmailTemplate.SelectedValue = vendor.EMailTemplate.ToString();
                }
            }
        }

        /// <summary>
        /// Gets the Email Template List
        /// </summary>
        private void GetEmailTemplateList()
        {
            ddlEmailTemplate.DataSource = VendorComponent.GetEmailtemplateList();
            ddlEmailTemplate.DataTextField = "Label";
            ddlEmailTemplate.DataValueField = "TemplateID";
            ddlEmailTemplate.DataBind();
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            int VendorID;
            tb_Vendor vendor = new tb_Vendor();
            VendorComponent objVendorComponent = new VendorComponent();
            if (!string.IsNullOrEmpty(Request.QueryString["VendorID"]) && Convert.ToString(Request.QueryString["VendorID"]) != "0")
            {
                vendor = objVendorComponent.GetVendorByID(Convert.ToInt32(Request.QueryString["VendorID"]));
                vendor.Name = txtVendorName.Text.Trim();
                vendor.Email = txtEmail.Text.Trim();
                vendor.Address = txtAddress.Text.Trim();
                vendor.Country = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
                if (ddlState.SelectedValue == "-11")
                    vendor.State = txtother.Text.Trim();
                else
                    vendor.State = ddlState.SelectedItem.Text;
                vendor.City = txtCity.Text.Trim();
                vendor.Zipcode = txtZipcode.Text.Trim();
                vendor.Phone = txtPhone.Text.Trim();
                vendor.Active = chkStatus.Checked;
                vendor.IsDropshipper = chkIsDropShipper.Checked;
                vendor.EMailTemplate = Convert.ToInt32(ddlEmailTemplate.SelectedValue);
                VendorID = objVendorComponent.UpdateVendor(vendor);
                if (VendorID > 0)
                    Response.Redirect("VendorList.aspx?status=updated");
            }
            else
            {
                vendor.Name = txtVendorName.Text.Trim();
                vendor.Email = txtEmail.Text.Trim();
                vendor.Address = txtAddress.Text.Trim();
                vendor.Country = Convert.ToInt32(ddlCountry.SelectedValue.ToString());
                if (ddlState.SelectedValue == "-11")
                    vendor.State = txtother.Text.Trim();
                else
                    vendor.State = ddlState.SelectedItem.Text;
                vendor.City = txtCity.Text.Trim();
                vendor.Zipcode = txtZipcode.Text.Trim();
                vendor.Phone = txtPhone.Text.Trim();
                vendor.Active = chkStatus.Checked;
                vendor.IsDropshipper = chkIsDropShipper.Checked;
                vendor.Deleted = false;
                vendor.EMailTemplate = Convert.ToInt32(ddlEmailTemplate.SelectedValue);
                VendorID = objVendorComponent.CreateVendor(vendor);
                if (VendorID > 0)
                    Response.Redirect("VendorList.aspx?status=inserted");
            }
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancle_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("VendorList.aspx");
        }

        /// <summary>
        /// Fills the Country Drop down
        /// </summary>
        private void FillCountry()
        {
            ddlCountry.Items.Clear();
            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();
            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlCountry.DataSource = dscountry.Tables[0];
                ddlCountry.DataTextField = "Name";
                ddlCountry.DataValueField = "CountryID";
                ddlCountry.DataBind();
                ddlCountry.Items.Insert(0, new ListItem("Select Country", "0"));


            }
            else
            {
                ddlCountry.DataSource = null;
                ddlCountry.DataBind();

            }

            if (ddlCountry.Items.FindByText("United States") != null)
            {
                ddlCountry.Items.FindByText("United States").Selected = true;
            }
            if (ddlCountry.Items.FindByText("United States") != null)
            {
                ddlCountry.Items.FindByText("United States").Selected = true;
            }
            ddlCountry_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlState.Items.Clear();
            if (ddlCountry.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlCountry.SelectedValue.ToString()));

                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlState.DataSource = dsState.Tables[0];
                    ddlState.DataTextField = "Name";
                    ddlState.DataValueField = "StateID";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlState.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "SetBillingOtherVisible(false);", true);

                }

                else
                {
                    ddlState.DataSource = null;
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlState.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlState.SelectedIndex = 0;
                }
            }
            else
            {
                ddlState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlState.SelectedIndex = 0;
            }

            ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "MakeBillingOtherVisible();", true);
        }
    }
}