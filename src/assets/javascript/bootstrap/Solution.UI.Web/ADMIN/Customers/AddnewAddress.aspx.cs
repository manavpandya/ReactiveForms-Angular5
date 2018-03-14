using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Solution.Bussines.Components;
using Solution.Bussines.Components.AdminCommon;
using Solution.Bussines.Entities;


namespace Solution.UI.Web.ADMIN.Customers
{
    public partial class AddnewAddress1 : BasePage
    {
        #region Local Variable
        CustomerComponent objCustomer = null;
        int AddressID = 0;
        #endregion


        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["CustID"] == null)
            {

            }
            CommonOperations.RedirectWithSSL(true);
            if (Request.QueryString["AddID"] != null)
            {
                AddressID = Convert.ToInt32(Request.QueryString["AddID"].ToString());
            }

            if (!IsPostBack)
            {

                imgSave.ImageUrl = "/App_Themes/" + Page.Theme + "/images/save.gif";
                imgCancle.ImageUrl = "/App_Themes/" + Page.Theme + "/images/cancel.gif";

                if (Request.QueryString["AddType"] != null && Request.QueryString["Type"] != null)
                {

                    Session["IsAnonymous"] = "false";
                    FillCountry();

                    string AddType = Request.QueryString["AddType"].ToString();
                    string type = Request.QueryString["Type"].ToString();

                    if (AddType == "billingadd")
                    {
                        // this.ltTitle.Text = "Billing Address";
                        // this.ltbrTitle.Text = "Billing Address";
                        lblHeader.Text = "Billing Address";
                    }
                    else
                    {
                        //this.ltTitle.Text = "Shipping Address";
                        // this.ltbrTitle.Text = "Shipping Address";
                        lblHeader.Text = "Shipping Address";
                    }


                    if (Request.QueryString["Type"].ToString() != "new")
                    {
                        if (Request.QueryString["CustID"] != null && Request.QueryString["CustID"].ToString().Trim().Length > 0)
                        {
                            FillAddressData(Convert.ToInt32(Request.QueryString["CustID"].ToString()));
                        }
                    }
                    else
                    {
                        ClearField();
                    }
                }
            }
            Page.MaintainScrollPositionOnPostBack = true;
        }

        /// <summary>
        /// Fill Address Data  while in Edit mode
        /// </summary>
        /// <param name="CustomerID">Int32 CustomerID</param>
        private void FillAddressData(Int32 CustomerID)
        {
            objCustomer = new CustomerComponent();
            DataSet dsCustomerDetail = new DataSet();

            dsCustomerDetail = objCustomer.GetCustomerDetailByCustID(Convert.ToInt32(Request.QueryString["CustID"]));
            if (dsCustomerDetail != null && dsCustomerDetail.Tables.Count > 0 && dsCustomerDetail.Tables[0].Rows.Count > 0)
            {
                DataSet dsGetAddress = new DataSet();
                dsGetAddress = objCustomer.GetAddressDetailByAddressID(AddressID);
                if (dsGetAddress != null && dsGetAddress.Tables.Count > 0 && dsGetAddress.Tables[0].Rows.Count > 0)
                {
                    txtFirstname.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["FirstName"]);
                    txtLastname.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["LastName"]);
                    txtCompany.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Company"]);
                    txtaddressLine1.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Address1"]);
                    txtAddressLine2.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Address2"]);
                    txtsuite.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Suite"]);
                    txtCity.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["City"]);

                    if (ddlcountry.Items.FindByValue(Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Country"])) != null)
                    {
                        ddlcountry.Items.FindByValue(Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Country"])).Selected = true;
                        ddlcountry_SelectedIndexChanged(null, null);
                    }
                    try
                    {
                        ddlcountry.SelectedValue = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Country"]);
                    }
                    catch
                    {
                        ddlcountry.SelectedIndex = 0;
                    }

                    if (ddlstate.Items.FindByText(Convert.ToString(dsGetAddress.Tables[0].Rows[0]["State"])) == null)
                    {
                        if (ddlstate.Items.FindByText("Other") != null)
                        {
                            ddlstate.Items.FindByText("Other").Selected = true;
                            ClientScript.RegisterStartupScript(typeof(Page), "State", "MakeOtherVisible();", true);
                            txtOtherState.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["State"]);
                        }
                    }
                    else
                    {
                        ddlstate.Items.FindByText(Convert.ToString(dsGetAddress.Tables[0].Rows[0]["State"])).Selected = true;
                    }
                    txtZipCode.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["ZipCode"]);
                    txtphone.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Phone"]);
                    txtFax.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Fax"]);
                    txtEmail.Text = Convert.ToString(dsGetAddress.Tables[0].Rows[0]["Email"]);

                    string AddType = Request.QueryString["AddType"].ToString();
                    if (AddType.ToString().ToLower() == "billingadd")
                        AddType = "BillingAddressID";
                    else
                        AddType = "ShippingAddressID";



                    Int32 CustId = Convert.ToInt32(CommonComponent.GetScalarCommonData("SELECT CustomerId FROM tb_Customer WHERE CustomerId=" + Request.QueryString["CustID"] + " and (BillingAddressId=" + Request.QueryString["addid"].ToString() + " OR ShippingAddressId=" + Request.QueryString["addid"] + ")"));
                    if (CustId > 0)
                    {
                        chkDefaultAddress.Checked = true;
                        chkDefaultAddress.Enabled = false;
                        lblmsgdefault.Visible = true;
                    }
                    else
                    {
                        chkDefaultAddress.Checked = false;
                        lblmsgdefault.Visible = false;
                    }
                }
            }
        }

        /// <summary>
        /// Clear Address Fields
        /// </summary>
        private void ClearField()
        {
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtCompany.Text = "";
            txtaddressLine1.Text = "";
            txtAddressLine2.Text = "";
            txtsuite.Text = "";
            txtCity.Text = "";
            ddlcountry.SelectedIndex = 0;
            ddlstate.Items.Clear();
            ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
            ClientScript.RegisterStartupScript(typeof(Page), "Visible", "SetOtherVisible(false);", true);
            txtZipCode.Text = "";
            txtphone.Text = "";
            txtFax.Text = "";
            txtEmail.Text = "";
            //chkDefaultAddress.Checked = false;
        }

        /// <summary>
        /// Bind both Country Drop down list
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
            ddlstate.Items.Clear();
            if (ddlcountry.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlcountry.SelectedValue.ToString()));

                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlstate.DataSource = dsState.Tables[0];
                    ddlstate.DataTextField = "Name";
                    ddlstate.DataValueField = "StateID";
                    ddlstate.DataBind();
                    ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlstate.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    ClientScript.RegisterStartupScript(typeof(Page), "Visible", "SetOtherVisible(false);", true);

                }
                else
                {
                    ddlstate.DataSource = null;
                    ddlstate.DataBind();
                    ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlstate.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlstate.Items.FindByText("Other").Selected = true;
                    ClientScript.RegisterStartupScript(typeof(Page), "Visible", "SetOtherVisible(true);", true);
                }
            }
            else
            {
                ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlstate.Items.Insert(1, new ListItem("Other", "-11"));
                ddlstate.Items.FindByText("Other").Selected = true;
                ClientScript.RegisterStartupScript(typeof(Page), "Visible", "SetOtherVisible(true);", true);

            }
        }

        /// <summary>
        ///  Finish Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnFinish_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) != "edit")
                {
                    if (Request.QueryString["AddType"] != null && Request.QueryString["AddType"].ToString() == "billingadd")
                    {
                        if (InsertBillingAddress())
                        {
                            if (chkDefaultAddress.Checked)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InsertB", "jAlert('Billing Address is added successfully.','Success');window.parent.location.href=window.parent.location.href;", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InsertB", "jAlert('Billing Address is added successfully.','Success');window.parent.document.getElementById('ContentPlaceHolder1_btnrefressaddress').click();", true);
                            }
                            


                            //Response.Redirect("AddressBook.aspx?msg=Billing Address is added successfully...");
                            //Response.Redirect("AddressBook.aspx");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert", "jAlert('Problem in adding billing address, please try again.');", true);
                            return;
                        }
                    }
                    if (Request.QueryString["AddType"] != null && Request.QueryString["AddType"].ToString() == "shippingadd")
                    {
                        if (InsertShippingAddress())
                        {
                            if (chkDefaultAddress.Checked)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InsertS", "jAlert('Shipping Address is added successfully.','Success');window.parent.location.href=window.parent.location.href;", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "InsertS", "jAlert('Shipping Address is added successfully.','Success');window.parent.document.getElementById('ContentPlaceHolder1_btnrefressaddress').click();", true);
                            }
                            //Response.Redirect("AddressBook.aspx?msg=Shipping Address is added successfully...");
                            // Response.Redirect("AddressBook.aspx");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailInsert", "jAlert('Problem in adding shipping address, please try again.');", true);
                            return;
                        }
                    }
                }
                else if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) == "edit")
                {
                    if (Request.QueryString["AddType"] != null && Request.QueryString["AddType"].ToString() == "billingadd")
                    {
                        if (UpdateBillingAddress())
                        {

                            if (chkDefaultAddress.Checked)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateB", "jAlert('Billing Address is updated successfully.');window.parent.location.href=window.parent.location.href;", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateB", "jAlert('Billing Address is updated successfully.');window.parent.document.getElementById('ContentPlaceHolder1_btnrefressaddress').click();", true);
                            }
                            
                            // Response.Redirect("AddressBook.aspx?msg=Billing Address is updated successfully...");
                            // Response.Redirect("AddressBook.aspx");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdating", "jAlert('Problem in updating billing address, please try again.');", true);
                            return;
                        }
                    }
                    if (Request.QueryString["AddType"] != null && Request.QueryString["AddType"].ToString() == "shippingadd")
                    {
                        if (UpdateShippingAddress())
                        {
                            if (chkDefaultAddress.Checked)
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateS", "jAlert('Shipping Address is updated successfully.');window.parent.location.href=window.parent.location.href;", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "UpdateS", "jAlert('Shipping Address is updated successfully.');window.parent.document.getElementById('ContentPlaceHolder1_btnrefressaddress').click();", true);
                            }
                            
                            //Response.Redirect("AddressBook.aspx?msg=Shipping Address is updated successfully...");
                            // Response.Redirect("AddressBook.aspx");
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdating", "jAlert('Problem in updating shipping address, please try again.');", true);
                            return;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// function for setting Insert billing Address
        /// </summary>
        /// <returns>Returns a Address table which contains Billing data to be insert</returns>
        protected bool InsertBillingAddress()
        {
            bool IsBillingAddressAdded = false;
            objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetBillOrShipValue(0); // 0 = Billing Address

            int AddID = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (AddID > 0)
            {
                IsBillingAddressAdded = true;
            }
            if (chkDefaultAddress.Checked)
            {
                IsBillingAddressAdded = objCustomer.UpdateCustomerBillingAddress(Convert.ToInt32(Request.QueryString["CustID"]), AddID);
            }
            ClearField();
            return IsBillingAddressAdded;
        }

        /// <summary>
        /// function for setting Insert shipping Address
        /// </summary>
        /// <returns>Returns a Address table which contains Shipping data to be Insert</returns>
        protected bool InsertShippingAddress()
        {
            bool IsShippingAddressAdded = false;

            objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetBillOrShipValue(1); // 1 = Shipping Address

            int AddID = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (AddID > 0)
            {
                IsShippingAddressAdded = true;
            }
            if (chkDefaultAddress.Checked)
            {
                IsShippingAddressAdded = objCustomer.UpdateCustomerShippingAddress(Convert.ToInt32(Request.QueryString["CustID"]), AddID);
            }
            ClearField();
            return IsShippingAddressAdded;
        }

        /// <summary>
        /// function for setting Update billing Address
        /// </summary>
        /// <returns>Returns a Address table which contains Billing data to be update</returns>
        protected bool UpdateBillingAddress()
        {
            bool IsBillingAddressUpdate = false;

            objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetBillOrShipValue(0); // 0 = Billing Address

            int UpdateID = objCustomer.UpdateCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (UpdateID > 0)
            {
                IsBillingAddressUpdate = true;
            }
            if (chkDefaultAddress.Checked)
            {
                IsBillingAddressUpdate = objCustomer.UpdateCustomerBillingAddress(Convert.ToInt32(Request.QueryString["CustID"]), AddressID);
            }
            else
            {
                IsBillingAddressUpdate = objCustomer.UpdateCustomerBillingAddress(Convert.ToInt32(Request.QueryString["CustID"]), 0);
            }
            ClearField();
            return IsBillingAddressUpdate;

        }

        /// <summary>
        /// function for setting Update Shipping Address
        /// </summary>
        /// <returns>Returns a Address table which contains Shipping data to be update</returns>
        protected bool UpdateShippingAddress()
        {
            bool IsShippingAddressUpdated = false;

            objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetBillOrShipValue(1); // 1 = Shipping Address

            int UpdateID = objCustomer.UpdateCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (UpdateID > 0)
            {
                IsShippingAddressUpdated = true;
            }
            if (chkDefaultAddress.Checked)
            {
                IsShippingAddressUpdated = objCustomer.UpdateCustomerShippingAddress(Convert.ToInt32(Request.QueryString["CustID"]), AddressID);
            }
            else
            {
                IsShippingAddressUpdated = objCustomer.UpdateCustomerShippingAddress(Convert.ToInt32(Request.QueryString["CustID"]), 0);
            }
            ClearField();
            return IsShippingAddressUpdated;

        }

        /// <summary>
        /// Set Billing or Shipping Address Value
        /// </summary>
        /// <param name="AddressType">Int32 AddressType</param>
        /// <returns>Returns tb_Address Table Object</returns>
        public tb_Address SetBillOrShipValue(Int32 AddressType)
        {
            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();


            tb_Address.CustomerID = Convert.ToInt32(Request.QueryString["CustID"]);
            tb_Address.FirstName = txtFirstname.Text;
            tb_Address.LastName = txtLastname.Text;
            tb_Address.AddressID = AddressID;
            tb_Address.Address1 = txtaddressLine1.Text;
            tb_Address.Address2 = txtAddressLine2.Text;
            tb_Address.City = txtCity.Text;
            tb_Address.Company = txtCompany.Text;
            if (ddlstate.SelectedValue == "-11")
            {
                tb_Address.State = txtOtherState.Text;
            }
            else
            {
                tb_Address.State = ddlstate.SelectedItem.Text;
            }
            tb_Address.Suite = txtsuite.Text;
            tb_Address.ZipCode = txtZipCode.Text;
            tb_Address.Country = Convert.ToInt32(ddlcountry.SelectedValue.ToString());
            tb_Address.Phone = txtphone.Text;
            tb_Address.Fax = txtFax.Text;
            tb_Address.Email = txtEmail.Text;
            tb_Address.AddressType = AddressType;
            if (Request.QueryString["type"] != null && Convert.ToString(Request.QueryString["type"]) != "edit")
            {
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.CreatedOn = DateTime.Now.Date;
                tb_Address.Deleted = false;
            }
            return tb_Address;
        }


        protected void imgSave_Click(object sender, ImageClickEventArgs e)
        {
        }

        protected void imgCancle_Click(object sender, ImageClickEventArgs e)
        {
        }
    }
}