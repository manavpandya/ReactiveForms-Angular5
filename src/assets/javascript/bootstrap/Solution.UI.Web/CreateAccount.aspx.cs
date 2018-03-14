using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Solution.Bussines.Components;
using Solution.Bussines.Entities;
using System.Data;
using System.Text.RegularExpressions;
using Solution.Bussines.Components.Common;
using System.Net.Mail;

namespace Solution.UI.Web
{
    public partial class CreateAccount : System.Web.UI.Page
    {

        bool IsContinueCheckOut = false;

        /// <summary>
        /// Page Load Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void Page_Load(object sender, EventArgs e)
        {

            CommonOperations.RedirectWithSSL(true);
            if (!IsPostBack)
            {
                FillCountry();
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "SetBillShipVisible", "SetBillingShippingVisible();", true);
                if (Session["NoOfCartItems"] != null && Session["NoOfCartItems"].ToString() != "0")
                {
                    btnContinueCheckout.Visible = true;
                    btnFinish.Visible = false;
                }
                else
                {
                    btnContinueCheckout.Visible = false;
                    btnFinish.Visible = true;
                }
                if (Session["UserName"] != null && Session["UserName"].ToString() != "")
                {
                    if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
                    {
                        spanbreadcrmbs.InnerText = "Update Account Detail";
                        h1tag.InnerText = "Update Account Detail";
                    }
                    ViewState["Mode"] = "edit";
                    if (Session["CustID"] != null && Session["CustID"].ToString().Trim().Length > 0)
                        BindCustomer(Convert.ToInt32(Session["CustID"].ToString()));
                }

            }
            txtUsername.Focus();
            Page.MaintainScrollPositionOnPostBack = true;
        }

        /// <summary>
        /// Binds the Customers
        /// </summary>
        /// <param name="CustId">int CustId</param>
        protected void BindCustomer(Int32 CustId)
        {
            DataSet dsCustomer = new DataSet();
            CustomerComponent CustomDetails = new CustomerComponent();
            dsCustomer = CustomDetails.GetCustomerDetailByCustID(CustId);
            if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
            {
                txtUsername.Text = dsCustomer.Tables[0].Rows[0]["Email"].ToString();
                txtpassword.Text = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["password"].ToString());
                txtconfirmpassword.Text = SecurityComponent.Decrypt(dsCustomer.Tables[0].Rows[0]["password"].ToString());

                if (!string.IsNullOrEmpty(dsCustomer.Tables[0].Rows[0]["BillingEqualShippingbit"].ToString()) && Convert.ToBoolean(dsCustomer.Tables[0].Rows[0]["BillingEqualShippingbit"].ToString()))
                {
                    pnlShippingDetails.Attributes.Add("style", "display:block");
                }
                else
                {
                    pnlShippingDetails.Attributes.Add("style", "display:none");
                }

                DataSet dsAddress = new DataSet();
                dsAddress = CustomDetails.GetCustomerBillingandShippingDetails(CustId, Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
                if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[0].Rows.Count > 0)
                {
                    txtBillFirstname.Text = dsAddress.Tables[0].Rows[0]["FirstName"].ToString();
                    txtBillLastname.Text = dsAddress.Tables[0].Rows[0]["LastName"].ToString();

                    txtBilladdressLine1.Text = dsAddress.Tables[0].Rows[0]["Address1"].ToString();
                    txtBillAddressLine2.Text = dsAddress.Tables[0].Rows[0]["Address2"].ToString();
                    txtBillCity.Text = dsAddress.Tables[0].Rows[0]["City"].ToString();
                    txtBillingCompany.Text = dsAddress.Tables[0].Rows[0]["Company"].ToString();
                    if (ddlBillcountry.Items.FindByValue(dsAddress.Tables[0].Rows[0]["Country"].ToString()) != null)
                    {
                        ddlBillcountry.SelectedIndex = -1;
                        ddlBillcountry.Items.FindByValue(dsAddress.Tables[0].Rows[0]["Country"].ToString()).Selected = true;
                        ddlBillcountry_SelectedIndexChanged(null, null);
                    }
                    if (ddlBillstate.Items.FindByText(dsAddress.Tables[0].Rows[0]["state"].ToString()) == null)
                    {
                        txtBillingOtherState.Text = dsAddress.Tables[0].Rows[0]["state"].ToString();
                        if (ddlBillstate.Items.FindByText("Others") != null)
                        {
                            ddlBillstate.Items.FindByText("Others").Selected = true;
                            ClientScript.RegisterStartupScript(typeof(Page), "Billing", "MakeBillingOtherVisible();", true);
                        }
                    }
                    else
                    {
                        if (ddlBillstate.Items.FindByText(dsAddress.Tables[0].Rows[0]["state"].ToString()) != null)
                        {
                            ddlBillstate.SelectedIndex = -1;
                            ddlBillstate.Items.FindByText(dsAddress.Tables[0].Rows[0]["state"].ToString()).Selected = true;
                        }
                    }

                    txtBillsuite.Text = dsAddress.Tables[0].Rows[0]["Suite"].ToString();
                    txtBillZipCode.Text = dsAddress.Tables[0].Rows[0]["ZipCode"].ToString();
                    txtBillphone.Text = dsAddress.Tables[0].Rows[0]["Phone"].ToString();
                    txtBillingFax.Text = dsAddress.Tables[0].Rows[0]["Fax"].ToString();
                    txtBillEmail.Text = dsAddress.Tables[0].Rows[0]["Email"].ToString();
                }

                if (dsAddress != null && dsAddress.Tables.Count > 0 && dsAddress.Tables[1].Rows.Count > 0)
                {
                    txtShipFirstname.Text = dsAddress.Tables[1].Rows[0]["FirstName"].ToString();
                    txtShipLastname.Text = dsAddress.Tables[1].Rows[0]["LastName"].ToString();
                    txtShipAddressLine1.Text = dsAddress.Tables[1].Rows[0]["Address1"].ToString();
                    txtshipAddressLine2.Text = dsAddress.Tables[1].Rows[0]["Address2"].ToString();
                    txtShipCity.Text = dsAddress.Tables[1].Rows[0]["City"].ToString();
                    txtShippingCompany.Text = dsAddress.Tables[1].Rows[0]["Company"].ToString();

                    if (ddlShipCounry.Items.FindByValue(dsAddress.Tables[1].Rows[0]["Country"].ToString()) != null)
                    {
                        ddlShipCounry.SelectedIndex = -1;
                        ddlShipCounry.Items.FindByValue(dsAddress.Tables[1].Rows[0]["Country"].ToString()).Selected = true;
                        ddlShipCounry_SelectedIndexChanged(null, null);
                    }
                    if (ddlShipState.Items.FindByText(dsAddress.Tables[1].Rows[0]["state"].ToString()) == null)
                    {
                        txtShippingOtherState.Text = dsAddress.Tables[1].Rows[0]["state"].ToString();
                        if (ddlShipState.Items.FindByText("Others") != null)
                        {
                            ddlShipState.Items.FindByText("Others").Selected = true;
                            ClientScript.RegisterStartupScript(typeof(Page), "Billing", "MakeShippingOtherVisible();", true);
                        }
                    }
                    else
                    {
                        if (ddlShipState.Items.FindByText(dsAddress.Tables[1].Rows[0]["state"].ToString()) != null)
                        {
                            ddlShipState.SelectedIndex = -1;
                            ddlShipState.Items.FindByText(dsAddress.Tables[1].Rows[0]["state"].ToString()).Selected = true;
                        }
                    }
                    txtShipSuite.Text = dsAddress.Tables[1].Rows[0]["Suite"].ToString();
                    txtShipZipCode.Text = dsAddress.Tables[1].Rows[0]["ZipCode"].ToString();
                    txtShipPhone.Text = dsAddress.Tables[1].Rows[0]["Phone"].ToString();
                    txtShippingFax.Text = dsAddress.Tables[1].Rows[0]["Fax"].ToString();
                    txtShipEmailAddress.Text = dsAddress.Tables[1].Rows[0]["Email"].ToString();
                }
            }
        }

        /// <summary>
        /// Bind both Country Drop down list
        /// </summary>
        public void FillCountry()
        {
            ddlBillcountry.Items.Clear();
            ddlShipCounry.Items.Clear();

            CountryComponent objCountry = new CountryComponent();
            DataSet dscountry = new DataSet();
            dscountry = objCountry.GetAllCountries();

            if (dscountry != null && dscountry.Tables.Count > 0 && dscountry.Tables[0].Rows.Count > 0)
            {
                ddlBillcountry.DataSource = dscountry.Tables[0];
                ddlBillcountry.DataTextField = "Name";
                ddlBillcountry.DataValueField = "CountryID";
                ddlBillcountry.DataBind();
                ddlBillcountry.Items.Insert(0, new ListItem("Select Country", "0"));

                ddlShipCounry.DataSource = dscountry.Tables[0];
                ddlShipCounry.DataTextField = "Name";
                ddlShipCounry.DataValueField = "CountryID";
                ddlShipCounry.DataBind();
                ddlShipCounry.Items.Insert(0, new ListItem("Select Country", "0"));
            }
            else
            {
                ddlBillcountry.DataSource = null;
                ddlBillcountry.DataBind();
                ddlShipCounry.DataSource = null;
                ddlShipCounry.DataBind();
            }

            if (ddlBillcountry.Items.FindByText("United States") != null)
            {
                ddlBillcountry.Items.FindByText("United States").Selected = true;
            }
            if (ddlShipCounry.Items.FindByText("United States") != null)
            {
                ddlShipCounry.Items.FindByText("United States").Selected = true;
            }

            ddlBillcountry_SelectedIndexChanged(null, null);
            ddlShipCounry_SelectedIndexChanged(null, null);
        }


        /// <summary>
        /// Bill Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlBillcountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlBillstate.Items.Clear();
            if (ddlBillcountry.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlBillcountry.SelectedValue.ToString()));

                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlBillstate.DataSource = dsState.Tables[0];
                    ddlBillstate.DataTextField = "Name";
                    ddlBillstate.DataValueField = "StateID";
                    ddlBillstate.DataBind();
                    ddlBillstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    if (ddlBillcountry.SelectedValue.ToString() != "1")
                    {
                        ddlBillstate.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    }

                    ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "SetBillingOtherVisible(false);", true);

                }

                else
                {
                    ddlBillstate.DataSource = null;
                    ddlBillstate.DataBind();
                    ddlBillstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlBillstate.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlBillstate.SelectedIndex = 0;
                }
            }
            else
            {
                ddlBillstate.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlBillstate.SelectedIndex = 0;
            }
            if (UseShippingAddress.Checked)
            {
                pnlShippingDetails.Attributes.Add("style", "display:block");
            }
            else
            {
                pnlShippingDetails.Attributes.Add("style", "display:none");
            }
            ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "MakeBillingOtherVisible();", true);
        }

        /// <summary>
        /// Ship Country Drop Down Selected Index Changed Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void ddlShipCounry_SelectedIndexChanged(object sender, EventArgs e)
        {
            ddlShipState.Items.Clear();
            if (ddlShipCounry.SelectedIndex != 0)
            {
                StateComponent objState = new StateComponent();
                DataSet dsState = new DataSet();
                dsState = objState.GetAllState(Convert.ToInt32(ddlShipCounry.SelectedValue.ToString()));
                if (dsState != null && dsState.Tables.Count > 0 && dsState.Tables[0].Rows.Count > 0)
                {
                    ddlShipState.DataSource = dsState.Tables[0];
                    ddlShipState.DataTextField = "Name";
                    ddlShipState.DataValueField = "StateID";
                    ddlShipState.DataBind();
                    ddlShipState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    if (ddlShipCounry.SelectedValue.ToString() != "1")
                    {
                        ddlShipState.Items.Insert(dsState.Tables[0].Rows.Count + 1, new ListItem("Other", "-11"));
                    }
                    ClientScript.RegisterStartupScript(typeof(Page), "ShippingVisible", "SetShippingOtherVisible(false);", true);

                }

                else
                {
                    ddlShipState.DataSource = null;
                    ddlShipState.DataBind();
                    ddlShipState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                    ddlShipState.Items.Insert(1, new ListItem("Other", "-11"));
                    ddlShipState.SelectedIndex = 0;
                }
            }
            else
            {
                ddlShipState.Items.Insert(0, new ListItem("Select State/Province", "0"));
                ddlShipState.SelectedIndex = 0;
            }
            if (UseShippingAddress.Checked)
            {
                pnlShippingDetails.Attributes.Add("style", "display:block");
            }
            else
            {
                pnlShippingDetails.Attributes.Add("style", "display:none");
            }
            ClientScript.RegisterStartupScript(typeof(Page), "ShippingVisible", "MakeShippingOtherVisible();", true);

        }

        /// <summary>
        /// Finish button click  event that Insert customer data into Database
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnFinish_Click(object sender, ImageClickEventArgs e)
        {
            if (this.txtCodeshow.Text != this.Session["CaptchaImageText"].ToString())
            {

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CaptchaAlert", "<script type='text/javascript'>alert('Incorrect Verification Code, try again.');</script>");


                this.txtCodeshow.Text = "";
                txtCodeshow.Focus();
                if (UseShippingAddress.Checked)
                {
                    pnlShippingDetails.Attributes.Add("style", "display:block");
                }
                else
                {
                    pnlShippingDetails.Attributes.Add("style", "display:none");
                }
                ClientScript.RegisterStartupScript(typeof(Page), "BillingVisible", "MakeBillingOtherVisible();", true);
                ClientScript.RegisterStartupScript(typeof(Page), "ShippingVisible", "MakeShippingOtherVisible();", true);
            }
            else
            {
                if (Session["CustId"] != null && (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit"))
                {
                    CustomerComponent objCustomer = new CustomerComponent();
                    DataSet dsCustomer = new DataSet();
                    dsCustomer = objCustomer.GetAdminCustomerDetailByCustID(Convert.ToInt32(Session["CustId"]));
                    int CustID = 0;
                    int BillingAddressID = 0;
                    int ShippingAddressID = 0;
                    if (dsCustomer != null && dsCustomer.Tables.Count > 0 && dsCustomer.Tables[0].Rows.Count > 0)
                    {
                        CustID = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["CustomerID"]);
                        BillingAddressID = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["BillingAddressID"]);
                        ShippingAddressID = Convert.ToInt32(dsCustomer.Tables[0].Rows[0]["ShippingAddressID"]);
                    }
                    if (Request.QueryString["Mode"] != null && Request.QueryString["Mode"].ToString() == "edit")
                    {
                        int Customerupdated = 0;
                        objCustomer = new CustomerComponent();
                        tb_Customer tb_Customer = new tb_Customer();
                        tb_Customer.CustomerID = Convert.ToInt32(Session["CustId"]);
                        tb_Customer.Email = txtUsername.Text;
                        tb_Customer.Password = SecurityComponent.Encrypt(txtpassword.Text);
                        tb_Customer.FirstName = txtBillFirstname.Text;
                        tb_Customer.LastName = txtBillLastname.Text;
                        if (UseShippingAddress.Checked)
                        {
                            tb_Customer.BillingEqualShippingbit = false;
                        }
                        else
                        {
                            tb_Customer.BillingEqualShippingbit = true;
                        }
                        tb_Customer.IsRegistered = true;
                        tb_Customer.IsLockedOut = false;
                        tb_Customer.FailedPasswordAttemptCount = 0;
                        tb_Customer.LastIPAddress = Request.UserHostAddress.ToString();
                        tb_Customer.Active = true;
                        tb_Customer.Deleted = false;
                        tb_Customer.CreatedOn = DateTime.Now.Date;
                        tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
                        Customerupdated = objCustomer.UpdateCustomer(tb_Customer, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                        if (Customerupdated == -1)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdatew", "$(document).ready( function() {jAlert('Customer already exist with same email address.', 'Message');});", true);
                            return;
                        }
                        else if (Customerupdated == 1)
                        {
                            //if (ViewState["IsPasswordChanged"] != null && ViewState["IsPasswordChanged"].ToString() == "1")
                            //{
                            //    try
                            //    {
                            //        SendMail();
                            //    }
                            //    catch { }
                            //}

                            if (UpdateBillingAddress(BillingAddressID, CustID) && UpdateShippingAddress(ShippingAddressID, CustID))
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Updateq", "$(document).ready( function() {jAlert('Customer Updated Successfully.', 'Message');});", true);
                                Response.Redirect("AddToCart.aspx", true);
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdate", "$(document).ready( function() {jAlert('Problem in Updating billing and shipping address, please try again.', 'Message');});", true);
                                return;
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdate1", "$(document).ready( function() {jAlert('Problem in Updating customer, please try again.', 'Message');});", true);
                            return;
                        }
                    }
                }
                else
                {
                    #region Customer In

                    int CustomerID = 0;
                    bool IsCustomerAdded = false;
                    CustomerComponent objCustomer = new CustomerComponent();
                    tb_Customer tb_Customer = new tb_Customer();
                    tb_Customer.Email = txtUsername.Text;
                    tb_Customer.Password = SecurityComponent.Encrypt(txtpassword.Text);
                    tb_Customer.FirstName = txtBillFirstname.Text;
                    tb_Customer.LastName = txtBillLastname.Text;
                    if (UseShippingAddress.Checked)
                    {
                        tb_Customer.BillingEqualShippingbit = false;
                    }
                    else
                    {
                        tb_Customer.BillingEqualShippingbit = true;
                    }
                    tb_Customer.IsRegistered = true;
                    tb_Customer.IsLockedOut = false;
                    tb_Customer.FailedPasswordAttemptCount = 0;
                    tb_Customer.LastIPAddress = Request.UserHostAddress.ToString();
                    tb_Customer.Active = true;
                    tb_Customer.Deleted = false;
                    tb_Customer.CreatedOn = DateTime.Now.Date;
                    tb_Customer.tb_StoreReference.EntityKey = new System.Data.EntityKey("RedTag_CCTV_Ecomm_DBEntities.tb_Store", "StoreID", Convert.ToInt32(AppLogic.AppConfigs("StoreId")));

                    CustomerID = objCustomer.InsertCustomer(tb_Customer, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

                    if (CustomerID != -1)
                    {
                        System.Web.HttpCookie custCookie = new System.Web.HttpCookie("ecommcustomer", CustomerID.ToString());
                        custCookie.Expires = DateTime.Now.AddYears(1);
                        Response.Cookies.Add(custCookie);
                        ViewState["CustmerID"] = CustomerID.ToString();

                        if (InsertBillAddress(CustomerID) && InsertShippAddress(CustomerID))
                        {
                            IsCustomerAdded = true;
                        }


                        if (IsCustomerAdded)
                        {
                            CommonOperations.RegisterCart(CustomerID, false);
                            Session["CustID"] = CustomerID.ToString();
                            Session["UserName"] = txtUsername.Text.ToString();
                            Session["IsAnonymous"] = "false";
                            Session["FirstName"] = txtBillFirstname.Text;

                            SendMail();

                            if (CustomerID > 0)
                            {
                                if (IsContinueCheckOut)
                                {
                                    if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
                                    {
                                        InsertWishListItems();
                                    }
                                    else if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "1")
                                    {
                                        ShoppingCartComponent objWishlist = new ShoppingCartComponent();
                                        objWishlist.AddToWishList(Convert.ToInt32(Session["CustID"].ToString()));
                                        Response.Redirect("/Wishlist.aspx", true);
                                    }
                                    else
                                    {
                                        if (Session["PaymentMethod"] != null)
                                        {
                                            Response.Redirect("/CheckOutCommon.aspx", true);
                                        }
                                        else
                                        {
                                            Response.Redirect("/CheckOutCommon.aspx", true);
                                        }
                                    }
                                }
                                else
                                {
                                    if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "2")
                                    {
                                        InsertWishListItems();
                                    }
                                    else if (Request.QueryString["wishlist"] != null && Convert.ToString(Request.QueryString["wishlist"]) == "1")
                                    {
                                        Response.Redirect("/CheckOutCommon.aspx", true);
                                    }
                                    else
                                    {
                                        Response.Redirect("thankyou.aspx?Page=createaccount", true);
                                    }
                                }
                            }
                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Exists", "alert('Problem while Creating Account');", true);
                            return;
                        }
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Exists", "alert('Customer already exists with same email address please select different email address.');", true);
                        return;
                    }
                }
                    #endregion
            }
        }


        /// <summary>
        /// Insert WishList Item into WishList Table
        /// </summary>
        private void InsertWishListItems()
        {
            int IsAdded = 0;
            if (Session["WishListProduct"] != null)
            {
                DataTable dtProduct = new DataTable();
                dtProduct = (DataTable)Session["WishListProduct"];

                if (dtProduct != null && dtProduct.Rows.Count > 0)
                {
                    WishListItemsComponent objWishList = new WishListItemsComponent();
                    IsAdded = objWishList.AddWishListItems(Convert.ToInt32(Session["CustID"]), Convert.ToInt32(dtProduct.Rows[0]["ProductID"]), Convert.ToInt32(dtProduct.Rows[0]["Quantity"]), Convert.ToDecimal(dtProduct.Rows[0]["Price"]), Convert.ToString(dtProduct.Rows[0]["VariantNameId"]), Convert.ToString(dtProduct.Rows[0]["VariantValueId"]));

                    if (IsAdded > 0)
                    {
                        Session["WishListProduct"] = null;
                        Response.Redirect("WishList.aspx", true);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Problem while adding product into wishlist.', 'Message');});", true);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// Maintains Password While Post back
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtpassword_PreRender(object sender, EventArgs e)
        {
            txtpassword.Attributes["value"] = txtpassword.Text.ToString();
        }

        /// <summary>
        /// Maintains Password While Post back
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void txtconfirmpassword_PreRender(object sender, EventArgs e)
        {
            txtconfirmpassword.Attributes["value"] = txtconfirmpassword.Text.ToString();
        }


        /// <summary>
        /// Insert Billing Address
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <returns>Returns Boolean True= Inserted and False=Not inserted</returns>
        public bool InsertBillAddress(Int32 CustID)
        {
            bool IsBillAddressInserted = false;

            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();


            tb_Address.CustomerID = CustID;
            tb_Address.FirstName = txtBillFirstname.Text;
            tb_Address.LastName = txtBillLastname.Text;

            tb_Address.Address1 = txtBilladdressLine1.Text;
            tb_Address.Address2 = txtBillAddressLine2.Text;
            tb_Address.City = txtBillCity.Text;
            tb_Address.Company = txtBillingCompany.Text;
            if (ddlBillstate.SelectedValue == "-11")
            {
                tb_Address.State = txtBillingOtherState.Text;
            }
            else
            {
                tb_Address.State = ddlBillstate.SelectedItem.Text;
            }
            tb_Address.Suite = txtBillsuite.Text;
            tb_Address.ZipCode = txtBillZipCode.Text;
            tb_Address.Country = Convert.ToInt32(ddlBillcountry.SelectedValue.ToString());
            tb_Address.Phone = txtBillphone.Text;
            tb_Address.Fax = txtBillingFax.Text;
            tb_Address.Email = txtBillEmail.Text;
            tb_Address.PaymentMethodIDLastUsed = "";
            tb_Address.AddressType = 0;
            tb_Address.CreatedOn = DateTime.Now.Date;
            tb_Address.Deleted = false;
            int isadded = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (isadded > 0)
            {
                IsBillAddressInserted = objCustomer.UpdateCustomerBillingAddress(CustID, isadded);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding billing address, please try again...');", true);
                IsBillAddressInserted = false;
            }

            return IsBillAddressInserted;
        }

        /// <summary>
        /// Insert Shipping Address
        /// </summary>
        /// <param name="CustID">Int32 CustID</param>
        /// <returns>Returns Boolean True= Inserted and False=Not inserted</returns>
        public bool InsertShippAddress(Int32 CustID)
        {
            bool IsShippAddressInserted = false;

            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Address.CustomerID = CustID;

            if (UseShippingAddress.Checked == false)
            {
                tb_Address.FirstName = txtBillFirstname.Text;
                tb_Address.LastName = txtBillLastname.Text;

                tb_Address.Address1 = txtBilladdressLine1.Text;
                tb_Address.Address2 = txtBillAddressLine2.Text;
                tb_Address.City = txtBillCity.Text;
                tb_Address.Company = txtBillingCompany.Text;
                if (ddlBillstate.SelectedValue == "-11")
                {
                    tb_Address.State = txtBillingOtherState.Text;
                }
                else
                {
                    tb_Address.State = ddlBillstate.SelectedItem.Text;
                }
                tb_Address.Suite = txtBillsuite.Text;
                tb_Address.ZipCode = txtBillZipCode.Text;
                tb_Address.Country = Convert.ToInt32(ddlBillcountry.SelectedValue.ToString());
                tb_Address.Phone = txtBillphone.Text;
                tb_Address.Fax = txtBillingFax.Text;
                tb_Address.Email = txtBillEmail.Text;
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.AddressType = 1;
                tb_Address.CreatedOn = DateTime.Now.Date;
                tb_Address.Deleted = false;
            }
            else
            {
                tb_Address.FirstName = txtShipFirstname.Text;
                tb_Address.LastName = txtShipLastname.Text;

                tb_Address.Address1 = txtShipAddressLine1.Text;
                tb_Address.Address2 = txtshipAddressLine2.Text;
                tb_Address.City = txtShipCity.Text;
                tb_Address.Company = txtShippingCompany.Text;

                if (ddlBillstate.SelectedValue == "-11")
                {
                    tb_Address.State = txtShippingOtherState.Text;
                }
                else
                {
                    tb_Address.State = ddlShipState.SelectedItem.Text;
                }
                tb_Address.Suite = txtShipSuite.Text;
                tb_Address.ZipCode = txtShipZipCode.Text;
                tb_Address.Country = Convert.ToInt32(ddlShipCounry.SelectedValue.ToString());
                tb_Address.Phone = txtShipPhone.Text;
                tb_Address.Fax = txtShippingFax.Text;
                tb_Address.Email = txtShipEmailAddress.Text;
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.AddressType = 1;
                tb_Address.CreatedOn = DateTime.Now.Date;
                tb_Address.Deleted = false;
            }


            int isadded = objCustomer.InsertCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreID")));
            if (isadded > 0)
            {
                IsShippAddressInserted = objCustomer.UpdateCustomerShippingAddress(CustID, isadded);
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "alert('Problem in adding billing address, please try again...');", true);
                IsShippAddressInserted = false;
            }

            return IsShippAddressInserted;
        }

        /// <summary>
        /// Sending Mail to Customer EmailID
        /// </summary>
        private void SendMail()
        {
            CustomerComponent objCustomer = new CustomerComponent();
            DataSet dsCreateAccount = new DataSet();
            dsCreateAccount = objCustomer.GetEmailTamplate("CreateAccount", Convert.ToInt32(AppLogic.AppConfigs("StoreID")));

            if (dsCreateAccount != null && dsCreateAccount.Tables.Count > 0 && dsCreateAccount.Tables[0].Rows.Count > 0)
            {
                String strBody = "";
                String strSubject = "";
                strBody = dsCreateAccount.Tables[0].Rows[0]["EmailBody"].ToString();
                strSubject = dsCreateAccount.Tables[0].Rows[0]["Subject"].ToString();

                strSubject = Regex.Replace(strSubject, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);
                strSubject = Regex.Replace(strSubject, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STOREPATH###", AppLogic.AppConfigs("STOREPATH").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###LIVE_SERVER###", AppLogic.AppConfigs("LIVE_SERVER").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###STORENAME###", AppLogic.AppConfigs("STORENAME").ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###FIRSTNAME###", txtBillFirstname.Text.ToString() + ' ' + txtBillLastname.Text.ToString(), RegexOptions.IgnoreCase);

                strBody = Regex.Replace(strBody, "###USERNAME###", txtUsername.Text.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###StoreID###", Convert.ToString(AppLogic.AppConfigs("StoreID")), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###PASSWORD###", txtpassword.Text.ToString(), RegexOptions.IgnoreCase);
                strBody = Regex.Replace(strBody, "###YEAR###", DateTime.Now.Year.ToString(), RegexOptions.IgnoreCase);


                AlternateView av = AlternateView.CreateAlternateViewFromString(strBody.ToString(), null, "text/html");
                CommonOperations.SendMail(txtUsername.Text.ToString().Trim(), strSubject.ToString(), strBody.ToString(), Request.UserHostAddress.ToString(), true, av);
            }
        }

        /// <summary>
        ///  Continue Check-Out Button Click Event
        /// </summary>
        /// <param name="sender">object sender</param>
        /// <param name="e">EventArgs e</param>
        protected void btnContinueCheckout_Click(object sender, ImageClickEventArgs e)
        {
            IsContinueCheckOut = true;
            btnFinish_Click(null, null);
        }

        /// <summary>
        /// function for setting Update billing Address
        /// </summary>
        /// <returns>Returns a Address table which contains Billing data to be update</returns>
        protected bool UpdateBillingAddress(Int32 AddressID, Int32 CustomerID)
        {
            bool IsBillingAddressUpdate = false;
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetBillingValue(AddressID, CustomerID);

            int UpdateID = objCustomer.UpdateCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
            if (UpdateID > 0)
            {
                IsBillingAddressUpdate = true;
            }
            return IsBillingAddressUpdate;
        }

        /// <summary>
        /// function for setting Update shipping Address
        /// </summary>
        /// <returns>Returns a Address table which contains shipping data to be update</returns>
        protected bool UpdateShippingAddress(Int32 AddressID, Int32 CustomerID)
        {
            bool IsShippingAddressUpdate = false;
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Address tb_Address = new tb_Address();
            tb_Address = SetShippingValue(AddressID, CustomerID);

            int UpdateID = objCustomer.UpdateCustomerAddress(tb_Address, Convert.ToInt32(AppLogic.AppConfigs("StoreId")));
            if (UpdateID > 0)
            {
                IsShippingAddressUpdate = true;
            }
            return IsShippingAddressUpdate;
        }

        /// <summary>
        /// Set Billing Address Value
        /// </summary>
        /// <param name="AddressID">int AddressID</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns tb_Address Table Object</returns>
        public tb_Address SetBillingValue(Int32 AddressID, Int32 CustomerID)
        {
            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Address.CustomerID = CustomerID;
            tb_Address.FirstName = txtBillFirstname.Text;
            tb_Address.LastName = txtBillLastname.Text;
            tb_Address.AddressID = AddressID;
            tb_Address.Address1 = txtBilladdressLine1.Text;
            tb_Address.Address2 = txtBillAddressLine2.Text;
            tb_Address.City = txtBillCity.Text;
            tb_Address.Company = txtBillingCompany.Text;
            if (ddlBillstate.SelectedValue == "-11")
            {
                tb_Address.State = txtBillingOtherState.Text;
            }
            else
            {
                tb_Address.State = ddlBillstate.SelectedItem.Text;
            }
            tb_Address.Suite = txtBillsuite.Text;
            tb_Address.ZipCode = txtBillZipCode.Text;
            tb_Address.Country = Convert.ToInt32(ddlBillcountry.SelectedValue.ToString());
            tb_Address.Phone = txtBillphone.Text;
            tb_Address.Fax = txtBillingFax.Text;
            tb_Address.Email = txtBillEmail.Text;
            tb_Address.AddressType = 0;
            if (Request.QueryString["Mode"] != null && Convert.ToString(Request.QueryString["Mode"]) != "edit")
            {
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.CreatedOn = DateTime.Now.Date;
                tb_Address.Deleted = false;
            }
            return tb_Address;
        }

        /// <summary>
        /// Set Shipping Address Value
        /// </summary>
        /// <param name="AddressID">int AddressID</param>
        /// <param name="CustomerID">int CustomerID</param>
        /// <returns>Returns tb_Address Table Object</returns>
        public tb_Address SetShippingValue(Int32 AddressID, Int32 CustomerID)
        {
            tb_Address tb_Address = new tb_Address();
            CustomerComponent objCustomer = new CustomerComponent();
            tb_Address.CustomerID = CustomerID;

            if (UseShippingAddress.Checked == false)
            {
                tb_Address.FirstName = txtBillFirstname.Text;
                tb_Address.LastName = txtBillLastname.Text;
                tb_Address.AddressID = AddressID;
                tb_Address.Address1 = txtBilladdressLine1.Text;
                tb_Address.Address2 = txtBillAddressLine2.Text;
                tb_Address.City = txtBillCity.Text;
                tb_Address.Company = txtBillingCompany.Text;
                if (ddlBillstate.SelectedValue == "-11")
                {
                    tb_Address.State = txtBillingOtherState.Text;
                }
                else
                {
                    tb_Address.State = ddlBillstate.SelectedItem.Text;
                }
                tb_Address.Suite = txtBillsuite.Text;
                tb_Address.ZipCode = txtBillZipCode.Text;
                tb_Address.Country = Convert.ToInt32(ddlBillcountry.SelectedValue.ToString());
                tb_Address.Phone = txtBillphone.Text;
                tb_Address.Fax = txtBillingFax.Text;
                tb_Address.Email = txtBillEmail.Text;
            }
            else
            {
                tb_Address.FirstName = txtShipFirstname.Text;
                tb_Address.LastName = txtShipLastname.Text;
                tb_Address.AddressID = AddressID;
                tb_Address.Address1 = txtShipAddressLine1.Text;
                tb_Address.Address2 = txtshipAddressLine2.Text;
                tb_Address.City = txtShipCity.Text;
                tb_Address.Company = txtShippingCompany.Text;
                if (ddlShipState.SelectedValue == "-11")
                {
                    tb_Address.State = txtShippingOtherState.Text;
                }
                else
                {
                    tb_Address.State = ddlShipState.SelectedItem.Text;
                }
                tb_Address.Suite = txtShipSuite.Text;
                tb_Address.ZipCode = txtShipZipCode.Text;
                tb_Address.Country = Convert.ToInt32(ddlShipCounry.SelectedValue.ToString());
                tb_Address.Phone = txtShipPhone.Text;
                tb_Address.Fax = txtShippingFax.Text;
                tb_Address.Email = txtShipEmailAddress.Text;
            }
            tb_Address.AddressType = 1;
            if (Request.QueryString["Mode"] != null && Convert.ToString(Request.QueryString["Mode"]) != "edit")
            {
                tb_Address.PaymentMethodIDLastUsed = "";
                tb_Address.CreatedOn = DateTime.Now.Date;
                tb_Address.Deleted = false;
            }
            return tb_Address;
        }

        protected void imgbtnavaibility_Click(object sender, ImageClickEventArgs e)
        {

            DataSet dsCust = CommonComponent.GetCommonDataSet("Select * from tb_Customer Where Email='" + txtUsername.Text + "' and storeid =" + AppLogic.AppConfigs("StoreID") + "  and Isnull(IsRegistered,0) =1 ");
            if (dsCust != null && dsCust.Tables.Count > 0 && dsCust.Tables[0].Rows.Count > 0)
            {
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer already exist with same email address.', 'Message');});", true);
                // Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdatew", "$(document).ready( function() {jAlert('Customer already exist with same email address.', 'Message');});", true);
                //return;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "checkEmailAd();", true);
            }
            else
            {
                //DataSet dsnewCust = CommonComponent.GetCommonDataSet("Select top 1 * from tb_Customer_Temp Where Email='" + txtUsername.Text + "'");
                //if (dsnewCust != null && dsnewCust.Tables.Count > 0 && dsnewCust.Tables[0].Rows.Count > 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "msg", "checkEmailAd();", true);
                //}
                //else
                //{
                //Page.ClientScript.RegisterStartupScript(Page.GetType(), "msg", "$(document).ready( function() {jAlert('Customer does not exist.', 'Message');});", true);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "FailUpdatew", "$(document).ready( function() {jAlert('Customer does not exist.', 'Message');});", true);
                return;
                //}
            }
        }


        protected void Button2_Click(object sender, EventArgs e)
        {
            string email = Server.UrlEncode(SecurityComponent.Encrypt(txtUsername.Text));
            Session["forgotemail"] = SecurityComponent.Encrypt(txtUsername.Text).ToString();
            Response.Redirect("/ForgotPassword.aspx?Email=" + email + "", true);
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "pageredirect", "window.location.href='/forgotpassword.aspx?Email=" + email + "';", true);
        }




    }
}