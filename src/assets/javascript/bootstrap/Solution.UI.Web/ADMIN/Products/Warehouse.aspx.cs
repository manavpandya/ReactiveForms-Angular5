using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;

namespace Solution.UI.Web.ADMIN.Products
{
    public partial class Warehouse : Solution.UI.Web.BasePage
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
                btnCancel.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";
                FillCountry();
                if (!string.IsNullOrEmpty(Request.QueryString["WarehouseID"]) && Convert.ToString(Request.QueryString["WarehouseID"]) != "0")
                {
                    lblHeader.Text = "Update Warehouse";
                    tb_WareHouse warehouse = new tb_WareHouse();
                    ProductComponent objComp = new ProductComponent();
                    warehouse = objComp.GetWarehouseByID(Convert.ToInt32(Request.QueryString["WarehouseID"]));
                    txtWarehouseName.Text = warehouse.Name;
                    txtAddress1.Text = warehouse.Address1;
                    txtAddress2.Text = warehouse.Address2;
                    txtSuite.Text = warehouse.Suite;
                    txtCity.Text = warehouse.City;
                    txtZipCode.Text = warehouse.ZipCode;
                    ddlcountry.SelectedValue = warehouse.Country.ToString();
                    ddlState.SelectedValue = warehouse.State.ToString();
                    ddlState.ClearSelection();
                    if (ddlState.Items.FindByText(Convert.ToString(warehouse.State.ToString())) == null)
                    {
                        if (ddlState.Items.FindByText("Other") != null)
                        {
                            ddlState.Items.FindByText("Other").Selected = true;
                            ClientScript.RegisterStartupScript(typeof(Page), "State", "MakeBillingOtherVisible();", true);
                            txtOtherState.Text = Convert.ToString(warehouse.State.ToString());
                        }
                    }

                    else
                    {
                        ddlState.Items.FindByText(Convert.ToString(warehouse.State.ToString())).Selected = true;
                    }
                }
                else
                {
                    lblHeader.Text = "Add Warehouse";
                }
            }
        }

        /// <summary>
        /// Fills the Country
        /// </summary>
        public void FillCountry()
        {
            ddlcountry.Items.Clear();

            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();

            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlcountry.DataSource = dscountry.Tables[0];
                ddlcountry.DataTextField = "Name";
                ddlcountry.DataValueField = "CountryID";
                ddlcountry.DataBind();
                ddlcountry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
            else
            {
                ddlcountry.DataSource = null;
                ddlcountry.DataBind();
                ddlcountry.DataSource = null;
                ddlcountry.DataBind();
            }

            if (ddlcountry.Items.FindByText("United States") != null)
            {
                ddlcountry.Items.FindByText("United States").Selected = true;
            }
            ddlcountry_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlcountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlState.Items.Clear();
            if (ddlState.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlcountry.SelectedValue.ToString()));

                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlState.DataSource = dsState.Tables[0];
                    ddlState.DataTextField = "Name";
                    ddlState.DataValueField = "StateID";
                    ddlState.DataBind();
                    ddlState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlState.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));

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
            //if (ddlState.SelectedValue.ToString() == "-11")
            //{
            //    ClientScript.RegisterStartupScript(typeof(Page), "State", "MakeBillingOtherVisible();", true);
            //}
        }

        /// <summary>
        ///  Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCancel_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("WarehouseList.aspx");
        }

        /// <summary>
        ///  Save Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnSave_Click(object sender, ImageClickEventArgs e)
        {
            if (txtWarehouseName.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter Warehouse Name.', 'Message');});", true);
                return;
            }
            if (txtAddress1.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter Address1.', 'Message');});", true);
                return;
            }
            if (txtCity.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter City.', 'Message');});", true);
                return;
            }
            if (ddlcountry.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select Country.', 'Message');});", true);
                return;
            }
            if (ddlState.SelectedIndex == 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please select State.', 'Message');});", true);
                return;
            }
            //if (txtOtherState.Text == "")
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please specify State.', 'Message');});", true);
            //    return;
            //}
            if (txtZipCode.Text == "")
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Please enter ZipCode.', 'Message');});", true);
                return;
            }
            int WarehouseID;
            tb_WareHouse warehouse = new tb_WareHouse();
            ProductComponent objComponent = new ProductComponent();
            if (!string.IsNullOrEmpty(Request.QueryString["WarehouseID"]) && Convert.ToString(Request.QueryString["WarehouseID"]) != "0")
            {
                warehouse = objComponent.GetWarehouseByID(Convert.ToInt32(Request.QueryString["WarehouseID"]));
                warehouse.Name = txtWarehouseName.Text.Trim();
                warehouse.Address1 = txtAddress1.Text.Trim();
                warehouse.Address2 = txtAddress2.Text.Trim();
                warehouse.Suite = txtSuite.Text.Trim();
                warehouse.City = txtCity.Text.Trim();
                warehouse.ZipCode = txtZipCode.Text;
                warehouse.Country = Convert.ToInt32(ddlcountry.SelectedValue.ToString());
                if (ddlState.SelectedValue == "-11")
                {
                    warehouse.State = txtOtherState.Text;
                }
                else
                {
                    warehouse.State = ddlState.SelectedItem.Text;
                }
                warehouse.UpdatedBy = Convert.ToInt32(Session["AdminID"]);
                warehouse.UpdatedOn = DateTime.Now;
                WarehouseID = objComponent.UpdateWarehouse(warehouse);
                if (WarehouseID > 0)
                    Response.Redirect("WarehouseList.aspx?status=updated");
            }
            else
            {
                warehouse.Name = txtWarehouseName.Text.Trim();
                warehouse.Address1 = txtAddress1.Text.Trim();
                warehouse.Address2 = txtAddress2.Text.Trim();
                warehouse.Suite = txtSuite.Text.Trim();
                warehouse.City = txtCity.Text.Trim();
                warehouse.ZipCode = txtZipCode.Text;
                warehouse.Country = Convert.ToInt32(ddlcountry.SelectedValue.ToString());
                if (ddlState.SelectedValue == "-11")
                {
                    warehouse.State = txtOtherState.Text;
                }
                else
                {
                    warehouse.State = ddlState.SelectedItem.Text;
                }
                warehouse.Active = true;
                warehouse.Deleted = false;
                warehouse.CreatedBy = Convert.ToInt32(Session["AdminID"]);
                warehouse.CreatedOn = DateTime.Now;
                WarehouseID = objComponent.CreateWarehouse(warehouse);
                if (WarehouseID > 0)
                    Response.Redirect("WarehouseList.aspx?status=inserted");
            }
        }
    }
}