using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using System.Data;
using Solution.Bussines.Entities;
using Solution.Bussines.Components.Common;

namespace Solution.UI.Web
{
    public partial class oneclicksettings : System.Web.UI.Page
    {

        #region Local Variables

        CreditCardComponent objCreditCard = null;
        CustomerComponent objCustomer = null;
        tb_CreditCardDetails tb_CreditCardDetails = null;
        tb_CreditCardTypes tb_CreditCardTypes = null;

        #endregion

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            if (Session["CustID"] == null)
            {
                Response.Redirect("/Login.aspx");
                return;
            }
            Session["PageName"] = "1-Click Settings";
            if (!IsPostBack)
            {

                if (Session["CustID"] != null && Session["CustID"].ToString().Length > 0)
                {
                    FillCountry();
                    FillCreditCardTypeYear();
                    GetBillingAddress(Convert.ToInt32(Session["CustID"].ToString()));
                    GetShippingAddress(Convert.ToInt32(Session["CustID"].ToString()));
                    GetPaymentMethod(Convert.ToInt32(Session["CustID"].ToString()));
                    DefaultOneClickSettings(Convert.ToInt32(Session["CustID"].ToString()));
                }
            }

            ltbrTitle.Text = "1-Click Settings";
            ltTitle.Text = "1-Click Settings";
        }
        
        /// <summary>
        /// Gets the Customer Billing Address
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        public void GetBillingAddress(Int32 CustomerID)
        {
            objCreditCard = new CreditCardComponent();
            DataSet dsBillAddress = new DataSet();
            dsBillAddress = objCreditCard.GetAddressForByCustId_AddType(CustomerID, 0);

            if (dsBillAddress != null && dsBillAddress.Tables[0].Rows.Count > 0)
            {
                ddlBillingAddress.DataTextField = "Address";
                ddlBillingAddress.DataValueField = "AddressID";
                ddlBillingAddress.DataSource = dsBillAddress;
                ddlBillingAddress.DataBind();
            }
            ddlBillingAddress.Items.Insert(0, new ListItem("Select Billing Address", "0"));
            ddlBillingAddress.Items.Insert(1, new ListItem("Add New", "-12"));
            pnlAddressDetails.Visible = false;
        }

        /// <summary>
        /// Gets the Customer Shipping Address
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        public void GetShippingAddress(Int32 CustomerID)
        {
            objCreditCard = new CreditCardComponent();
            DataSet dsShippAddress = new DataSet();
            dsShippAddress = objCreditCard.GetAddressForByCustId_AddType(CustomerID, 1);

            if (dsShippAddress != null && dsShippAddress.Tables[0].Rows.Count > 0)
            {
                ddlShippingAddress.DataTextField = "Address";
                ddlShippingAddress.DataValueField = "AddressID";
                ddlShippingAddress.DataSource = dsShippAddress;
                ddlShippingAddress.DataBind();
            }
            ddlShippingAddress.Items.Insert(0, new ListItem("Select Shipping Address", "0"));
            ddlShippingAddress.Items.Insert(1, new ListItem("Add New", "-12"));
            pnlAddressDetails.Visible = false;
        }
        
        /// <summary>
        /// Gets the Payment Method
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        public void GetPaymentMethod(Int32 CustomerID)
        {
            objCreditCard = new CreditCardComponent();
            DataSet dsPayment = new DataSet();
            dsPayment = objCreditCard.GetCardDetailByCustomerId(CustomerID);

            if (dsPayment != null && dsPayment.Tables[0].Rows.Count > 0)
            {
                ddlPaymentMethod.DataTextField = "PaymentMethod";
                ddlPaymentMethod.DataValueField = "CardID";
                ddlPaymentMethod.DataSource = dsPayment;
                ddlPaymentMethod.DataBind();
            }

            ddlPaymentMethod.Items.Insert(0, new ListItem("Select Payment Method", "0"));
            ddlPaymentMethod.Items.Insert(1, new ListItem("Add New", "-12"));
            pnlCreditCard.Visible = false;
        }
        
        /// <summary>
        /// Save Default One Click Settings for Customer
        /// </summary>
        /// <param name="CustomerID">int CustomerID</param>
        public void DefaultOneClickSettings(Int32 CustomerID)
        {
            objCustomer = new CustomerComponent();

            DataSet dsOneClickSettings = new DataSet();
            dsOneClickSettings = objCustomer.GetCustomerDetailByCustID(CustomerID);

            if (dsOneClickSettings != null && dsOneClickSettings.Tables.Count > 0 && dsOneClickSettings.Tables[0].Rows.Count > 0)
            {
                ddlBillingAddress.SelectedValue = Convert.ToString(dsOneClickSettings.Tables[0].Rows[0]["BillingAddressID"]);
                ddlShippingAddress.SelectedValue = Convert.ToString(dsOneClickSettings.Tables[0].Rows[0]["ShippingAddressID"]);
                ddlPaymentMethod.SelectedValue = Convert.ToString(dsOneClickSettings.Tables[0].Rows[0]["CreditCardID"]);
            }
        }

        /// <summary>
        ///  Address Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddressSubmit_Click(object sender, ImageClickEventArgs e)
        {
            int AddressType = Convert.ToInt32(ViewState["AddressType"].ToString());
            int CustomerID = Convert.ToInt32(Session["CustID"].ToString());
            string AddType = "";
            if (AddressType == 0)
                AddType = "Billing";
            else
                AddType = "Shipping";

            if (ddlBillingAddress.SelectedValue == "-12" || ddlShippingAddress.SelectedValue == "-12")
            {
                int IsAdded = InsertBillAddress(CustomerID);

                if (IsAdded > 0)
                {
                    if (AddressType == 0)
                    {
                        ddlBillingAddress.Items.Clear();
                        GetBillingAddress(CustomerID);
                    }
                    else if (AddressType == 1)
                    {
                        ddlShippingAddress.Items.Clear();
                        GetShippingAddress(CustomerID);
                    }

                    DefaultOneClickSettings(CustomerID);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your 1-Click " + AddType + " Address is added successfully.');", true);
                    ClearAddress();
                    pnlAddressDetails.Visible = false;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem while inserting your 1-Click " + AddType + " address.');", true);
                    return;
                }
            }
        }

        /// <summary>
        ///  Address Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnAddressCancel_Click(object sender, ImageClickEventArgs e)
        {
            DefaultOneClickSettings(Convert.ToInt32(Session["CustID"]));
            ClearAddress();
            pnlAddressDetails.Visible = false;
        }

        /// <summary>
        ///  Card Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCardSubmit_Click(object sender, ImageClickEventArgs e)
        {
            int CardID = 0;
            if (!CheckCardExpirationDate())
            {
                objCreditCard = new CreditCardComponent();
                objCustomer = new CustomerComponent();
                tb_CreditCardDetails = new tb_CreditCardDetails();

                tb_CreditCardDetails = SetCreditCardValues();

                if (objCreditCard.CheckCreditCardIsExists(Convert.ToInt32(Session["CustID"]), SecurityComponent.Encrypt(txtCardNumber.Text.ToString().Trim()), Convert.ToInt32(ddlCardType.SelectedValue.ToString()), 0))
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Credit Card Detail already exists.');", true);
                    return;
                }
                else
                {
                    CardID = objCreditCard.InsertCreditCardDetails(tb_CreditCardDetails, Convert.ToInt32(Session["CustID"]));
                    if (CardID > 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your Credit Card Detail is added successfully.');", true);
                        GetPaymentMethod(Convert.ToInt32(Session["CustID"]));
                        DefaultOneClickSettings(Convert.ToInt32(Session["CustID"]));
                        Clear();
                        pnlCreditCard.Visible = false;
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding Credit Card Detail, please try again.');", true);
                        return;
                    }
                }
            }
        }

        /// <summary>
        ///  Card Cancel Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnCardCancel_Click(object sender, ImageClickEventArgs e)
        {
            DefaultOneClickSettings(Convert.ToInt32(Session["CustID"]));
            Clear();
            pnlCreditCard.Visible = false;
        }

        /// <summary>
        ///  Submit Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnsubmit_Click(object sender, ImageClickEventArgs e)
        {
            if ((ddlBillingAddress.SelectedIndex == 0 || ddlBillingAddress.SelectedValue == "-12"))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select Proper Billing Address.');", true);
                return;
            }
            else if ((ddlShippingAddress.SelectedIndex == 0 || ddlShippingAddress.SelectedValue == "-12"))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please Select Proper Shipping Address.');", true);
                return;
            }
            else
            {
                if (ddlPaymentMethod.SelectedValue == "-12")
                {
                    ddlPaymentMethod.SelectedIndex = 0;
                    pnlCreditCard.Visible = false;
                }

                objCreditCard = new CreditCardComponent();
                tb_Customer tb_Customer = new tb_Customer();
                tb_Customer.CustomerID = Convert.ToInt32(Session["CustID"]);
                tb_Customer.BillingAddressID = Convert.ToInt32(ddlBillingAddress.SelectedValue.ToString());
                tb_Customer.ShippingAddressID = Convert.ToInt32(ddlShippingAddress.SelectedValue.ToString());
                tb_Customer.CreditCardID = Convert.ToInt32(ddlPaymentMethod.SelectedValue.ToString());

                bool IsAdded = objCreditCard.SetOneClickDefaultValues(tb_Customer);

                if (IsAdded)
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Your 1 - Click Settings Saved Successfully.');", true);
                    ClearAddress();
                    Clear();
                    DefaultOneClickSettings(Convert.ToInt32(Session["CustID"]));
                    pnlAddressDetails.Visible = false;
                    pnlCreditCard.Visible = false;
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problems while saving your 1 - Click Settings.');", true);
                    return;
                }
            }
        }

        /// <summary>
        /// Validate Card expiration Date
        /// </summary>
        /// <returns>Returns True if Proper Validation of Data</returns>
        public bool CheckCardExpirationDate()
        {
            if ((Convert.ToInt32(ddlyear.SelectedValue) <= DateTime.Now.Year) && Convert.ToInt32(ddlmonth.SelectedValue) <= DateTime.Now.Month)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Please select valid Credit Card Expiration Date.');", true);
                Page.MaintainScrollPositionOnPostBack = false;
                ddlmonth.Focus();
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Set Credit Card Value into table for Insert or Update
        /// </summary>
        /// <returns>Returns tb_CreditCardDetails Table Object</returns>
        public tb_CreditCardDetails SetCreditCardValues()
        {
            tb_CreditCardDetails = new tb_CreditCardDetails();
            try
            {
                tb_CreditCardDetails.tb_CustomerReference.EntityKey = new EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Customer", "CustomerID", Convert.ToInt32(Session["CustID"]));
                tb_CreditCardDetails.NameOnCard = txtcardName.Text.ToString().Trim();
                tb_CreditCardDetails.CardNumber = SecurityComponent.Encrypt(txtCardNumber.Text.ToString().Trim());
                tb_CreditCardDetails.CardTypeID = Convert.ToInt32(ddlCardType.SelectedValue);
                tb_CreditCardDetails.CardVerificationCode = txtCardVarificationCode.Text.Trim();
                tb_CreditCardDetails.CardExpirationMonth = Convert.ToInt32(ddlmonth.SelectedValue.ToString());
                tb_CreditCardDetails.CardExpirationYear = Convert.ToInt32(ddlyear.SelectedValue.ToString());
                tb_CreditCardDetails.DefaultBillingAddressID = Convert.ToInt32(ddlBillingAddress.SelectedValue.ToString());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return tb_CreditCardDetails;
        }

        /// <summary>
        /// Bind both Country Drop Down List
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
        /// Store Drop Down Selected Index Changed Event
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
                    if (ddlcountry.SelectedValue.ToString() != "1")
                    {
                        ddlstate.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    }
                    ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "SetOtherVisible(false);", true);
                }

                else
                {
                    ddlstate.DataSource = null;
                    ddlstate.DataBind();
                    ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlstate.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlstate.SelectedIndex = 0;

                }
            }
            else
            {
                ddlstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlstate.SelectedIndex = 0;
            }
        }

        /// <summary>
        /// Fill Credit Card Type and Year
        /// </summary>
        private void FillCreditCardTypeYear()
        {
            objCreditCard = new CreditCardComponent();
            DataSet dsCCType = new DataSet();

            dsCCType = objCreditCard.GetAllCarTypeByStoreID(Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsCCType != null && dsCCType.Tables.Count > 0 && dsCCType.Tables[0].Rows.Count > 0)
            {
                ddlCardType.DataSource = dsCCType.Tables[0];
                ddlCardType.DataTextField = "CardType";
                ddlCardType.DataValueField = "CardTypeID";
                ddlCardType.DataBind();
            }
            ddlCardType.Items.Insert(0, new ListItem("Select Card Type", "0"));

            for (int i = DateTime.Now.Year; i < DateTime.Now.Year + 10; i++)
                ddlyear.Items.Add(i.ToString());
        }

        /// <summary>
        /// Billing Address Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlBillingAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlBillingAddress.SelectedValue == "-12")
            {
                pnlAddressDetails.Visible = true;
                ViewState["AddressType"] = "0";
                lblAddressTitle.Text = "Billing Address";

                if (ViewState["ShippingAddress"] != null)
                {
                    ddlShippingAddress.SelectedValue = Convert.ToString(ViewState["ShippingAddress"]);
                }
                else
                {
                    ddlShippingAddress.SelectedIndex = 0;
                }

                ClearAddress();
            }
            else
            {
                if (ddlShippingAddress.SelectedValue == "-12")
                {
                    pnlAddressDetails.Visible = true;
                    lblAddressTitle.Text = "Shipping Address";
                    ViewState["AddressType"] = "1";
                }
                else
                {
                    pnlAddressDetails.Visible = false;
                    lblAddressTitle.Text = "";
                    ViewState["AddressType"] = null;
                }
            }
            if (ddlBillingAddress.SelectedValue != "-12" && ddlBillingAddress.SelectedIndex != 0)
            {
                ViewState["BillingAddress"] = ddlBillingAddress.SelectedValue.ToString();
            }
        }

        /// <summary>
        /// Shipping Address Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlShippingAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlShippingAddress.SelectedValue == "-12")
            {
                pnlAddressDetails.Visible = true;
                ViewState["AddressType"] = "1";
                lblAddressTitle.Text = "Shipping Address";

                if (ViewState["BillingAddress"] != null)
                {
                    ddlBillingAddress.SelectedValue = Convert.ToString(ViewState["BillingAddress"]);
                }
                else
                {
                    ddlBillingAddress.SelectedIndex = 0;
                }
                ClearAddress();
            }
            else
            {
                if (ddlBillingAddress.SelectedValue == "-12")
                {
                    pnlAddressDetails.Visible = true;
                    lblAddressTitle.Text = "Billing Address";
                    ViewState["AddressType"] = "0";
                }
                else
                {
                    pnlAddressDetails.Visible = false;
                    lblAddressTitle.Text = "";
                    ViewState["AddressType"] = null;
                }
            }
            if (ddlShippingAddress.SelectedValue != "-12" && ddlShippingAddress.SelectedIndex != 0)
            {
                ViewState["ShippingAddress"] = ddlShippingAddress.SelectedValue.ToString();
            }
        }

        /// <summary>
        /// Payment Method Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlPaymentMethod_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPaymentMethod.SelectedValue == "-12")
            {
                pnlCreditCard.Visible = true;
                Clear();
            }
            else
            {
                pnlCreditCard.Visible = false;
            }
            if (ddlPaymentMethod.SelectedValue != "-12" && ddlPaymentMethod.SelectedIndex != 0)
            {
                ViewState["PaymentMethod"] = ddlBillingAddress.SelectedValue.ToString();
            }
        }

        /// <summary>
        /// Clear Credit Card Fields
        /// </summary>
        public void Clear()
        {
            txtcardName.Text = "";
            txtCardNumber.Text = "";
            ddlCardType.SelectedIndex = 0;
            txtCardVarificationCode.Text = "";
            ddlmonth.SelectedIndex = 0;
            ddlyear.SelectedIndex = 0;
        }

        /// <summary>
        /// Clear Address Fields
        /// </summary>
        public void ClearAddress()
        {
            txtFirstname.Text = "";
            txtLastname.Text = "";
            txtCompany.Text = "";
            txtaddressLine1.Text = "";
            txtAddressLine2.Text = "";
            txtsuite.Text = "";
            txtCity.Text = "";
            txtZipCode.Text = "";
            txtphone.Text = "";
            txtEmail.Text = "";
            txtFax.Text = "";
            if (ddlcountry.Items.FindByText("United States") != null)
            {
                ddlcountry.Items.FindByText("United States").Selected = true;
            }
            else
            {
                ddlcountry.SelectedIndex = 0;
            }
            ddlcountry_SelectedIndexChanged(null, null);
        }

        /// <summary>
        /// Insert Billing Address
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <returns>Returns boolean True= Inserted and False=Not inserted</returns>
        public Int32 InsertBillAddress(Int32 CustID)
        {
            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Address.CustomerID = CustID;
            tb_Address.FirstName = txtFirstname.Text;
            tb_Address.LastName = txtLastname.Text;
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
            tb_Address.PaymentMethodIDLastUsed = "";
            if (ViewState["AddressType"] != null)
            {
                tb_Address.AddressType = Convert.ToInt32(ViewState["AddressType"]);
            }

            tb_Address.CreatedOn = DateTime.Now.Date;
            tb_Address.Deleted = false;
            int isadded = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            return isadded;
        }
    }
}